using System.Collections.Generic;
using UnityEngine;

namespace RogueAi.Castle
{
    /// <summary>
    /// Deterministic, seed-driven procedural castle builder.
    ///
    /// The castle is laid out as concentric Chebyshev rings on a square grid, growing from the
    /// <c>CryptChamberFinal</c> at the origin outward through Keep, InnerWard, OuterBailey and
    /// CurtainWall. Every module is attached to an already-placed 4-neighbour, so the resulting
    /// floor plan is guaranteed to be a single 4-connected region — which is what lets the A*
    /// validator always find (or correctly reject) a crypt→extraction path.
    ///
    /// Because the entire layout is a pure function of the seed, only the seed is replicated over
    /// the network (see <see cref="CastleNetworkManager"/>); no mesh or transform data is sent.
    /// </summary>
    public class ProceduralCastleGenerator : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private CastleRoomRegistry registry;
        [SerializeField] public int defaultSeed = 12345;

        [Header("Layout")]
        [Tooltip("World units between adjacent grid cells.")]
        [SerializeField] private float cellSize = 12f;

        [Tooltip("Parent for instantiated rooms. Auto-created if left null.")]
        [SerializeField] private Transform roomContainer;

        /// <summary>The most recent layout produced by <see cref="Generate"/>.</summary>
        public ProceduralCastleData LastGenerated { get; private set; }

        public CastleRoomRegistry Registry { get => registry; set => registry = value; }

        private readonly List<GameObject> _instantiated = new List<GameObject>();

        // Zone build order from the center outward, paired with the ring radius and room-count range.
        private static readonly ZoneRing[] Rings =
        {
            new ZoneRing(CastleZone.Crypt,       1, 3, 5),
            new ZoneRing(CastleZone.Keep,        2, 4, 7),
            new ZoneRing(CastleZone.InnerWard,   3, 6, 10),
            new ZoneRing(CastleZone.OuterBailey, 4, 8, 14),
            new ZoneRing(CastleZone.CurtainWall, 5, 10, 16),
        };

        private readonly struct ZoneRing
        {
            public readonly CastleZone Zone;
            public readonly int Radius;
            public readonly int MinCount;
            public readonly int MaxCount;

            public ZoneRing(CastleZone zone, int radius, int min, int max)
            {
                Zone = zone;
                Radius = radius;
                MinCount = min;
                MaxCount = max;
            }
        }

        /// <summary>
        /// Builds a castle layout deterministically from <paramref name="seed"/>. If room prefabs are
        /// assigned in the registry they are instantiated; otherwise the returned data is a pure
        /// metadata layout (used by tests and by the network path before art is wired in).
        /// </summary>
        public ProceduralCastleData Generate(int seed)
        {
            ClearGenerated();

            var rng = new System.Random(seed);
            var data = new ProceduralCastleData(seed);

            // Maps an occupied grid cell -> index into data.PlacedModules.
            var occupied = new Dictionary<Vector2Int, int>();

            // 1 & 2. Place the crypt final chamber at the center origin.
            string cryptFinalId = PickCryptFinalId();
            var centerCell = Vector2Int.zero;
            int cryptIndex = PlaceModule(data, occupied, cryptFinalId, CastleZone.Crypt, centerCell,
                Vector2Int.up, isCryptEntry: true);
            data.CryptStartIndex = cryptIndex;

            // 3-6. Ring by ring outward.
            foreach (ZoneRing ring in Rings)
            {
                int count = rng.Next(ring.MinCount, ring.MaxCount + 1);
                // The crypt ring already spent one slot on the final chamber (the center).
                if (ring.Zone == CastleZone.Crypt)
                    count = Mathf.Max(0, count - 1);

                PlaceRing(data, occupied, rng, ring, count);
            }

            // 7. Mark one OuterBailey module as the extraction exit.
            AssignExtractionExit(data);

            LastGenerated = data;
            return data;
        }

        /// <summary>
        /// Places up to <paramref name="count"/> modules on a single Chebyshev ring, always
        /// attaching each to an already-placed 4-neighbour so the layout stays connected. Axis
        /// cells (which connect straight inward to the previous ring) are prioritised so that every
        /// adjacent zone pair is joined by at least two "staircase" links.
        /// </summary>
        private void PlaceRing(ProceduralCastleData data, Dictionary<Vector2Int, int> occupied,
            System.Random rng, ZoneRing ring, int count)
        {
            if (count <= 0)
                return;

            List<Vector2Int> candidates = RingCells(ring.Radius);
            Shuffle(candidates, rng);

            // Prioritise the four axis cells to guarantee >=2 inward (staircase) connections.
            candidates.Sort((a, b) =>
            {
                int aAxis = (a.x == 0 || a.y == 0) ? 0 : 1;
                int bAxis = (b.x == 0 || b.y == 0) ? 0 : 1;
                return aAxis.CompareTo(bAxis);
            });

            var weightedPool = registry != null ? registry.GetModulesForZone(ring.Zone) : null;
            int placed = 0;
            int inwardLinks = 0;

            // Iterate until we have placed `count` modules or a full pass adds nothing (ring full).
            bool progress = true;
            while (placed < count && progress)
            {
                progress = false;
                for (int i = 0; i < candidates.Count && placed < count; i++)
                {
                    Vector2Int cell = candidates[i];
                    if (occupied.ContainsKey(cell))
                        continue;

                    if (!TryFindPlacedNeighbour(occupied, cell, out Vector2Int neighbour))
                        continue;

                    string roomId = PickWeighted(weightedPool, rng, ring.Zone);
                    Vector2Int facing = neighbour - cell; // point the module toward its anchor
                    PlaceModule(data, occupied, roomId, ring.Zone, cell, facing, isCryptEntry: false);

                    // An inward link is a connection to a cell one ring closer to the center.
                    if (Chebyshev(neighbour) < ring.Radius)
                        inwardLinks++;

                    placed++;
                    progress = true;
                }
            }

            if (inwardLinks < 2)
            {
                Debug.LogWarning($"[CastleGen] Zone {ring.Zone} has only {inwardLinks} inward " +
                                 "staircase link(s); expected at least 2.");
            }
        }

        /// <summary>Records (and optionally instantiates) a single module, returning its index.</summary>
        private int PlaceModule(ProceduralCastleData data, Dictionary<Vector2Int, int> occupied,
            string roomId, CastleZone zone, Vector2Int cell, Vector2Int facing, bool isCryptEntry)
        {
            Vector3 worldPos = new Vector3(cell.x * cellSize, 0f, cell.y * cellSize);
            Quaternion rot = RotationForFacing(facing);

            var placed = new ProceduralCastleData.PlacedModule(roomId, worldPos, rot, zone, cell)
            {
                IsCryptEntry = isCryptEntry
            };

            int index = data.PlacedModules.Count;
            data.PlacedModules.Add(placed);
            occupied[cell] = index;

            InstantiateModule(roomId, zone, cell, worldPos, rot, isCryptEntry);
            return index;
        }

        /// <summary>Instantiates the prefab for a module if the registry provides one.</summary>
        private void InstantiateModule(string roomId, CastleZone zone, Vector2Int cell,
            Vector3 worldPos, Quaternion rot, bool isCryptEntry)
        {
            if (registry == null)
                return;

            CastleRoomModuleData entry = registry.GetById(roomId);
            if (entry == null || entry.Prefab == null)
                return; // data-only layout; designer prefab not yet assigned.

            EnsureContainer();
            GameObject go = Instantiate(entry.Prefab, worldPos, rot, roomContainer);
            _instantiated.Add(go);

            var module = go.GetComponent<CastleRoomModule>();
            if (module == null)
                module = go.AddComponent<CastleRoomModule>();

            module.Zone = zone;
            module.RoomId = roomId;
            module.GridPosition = cell;
            module.IsCryptEntry = isCryptEntry;
            module.PopulateSockets();
        }

        /// <summary>Marks the last-placed OuterBailey module as the extraction exit.</summary>
        private void AssignExtractionExit(ProceduralCastleData data)
        {
            for (int i = data.PlacedModules.Count - 1; i >= 0; i--)
            {
                if (data.PlacedModules[i].Zone == CastleZone.OuterBailey)
                {
                    var pm = data.PlacedModules[i];
                    pm.IsExtractionExit = true;
                    data.PlacedModules[i] = pm;
                    data.ExtractionExitIndex = i;

                    // Reflect on the instantiated module too, if present.
                    foreach (GameObject go in _instantiated)
                    {
                        if (go == null) continue;
                        var m = go.GetComponent<CastleRoomModule>();
                        if (m != null && m.GridPosition == pm.GridPosition && m.Zone == CastleZone.OuterBailey)
                        {
                            m.IsExtractionExit = true;
                            break;
                        }
                    }
                    return;
                }
            }

            Debug.LogWarning("[CastleGen] No OuterBailey module placed — extraction exit unassigned.");
        }

        /// <summary>Destroys every room GameObject instantiated by the last generation pass.</summary>
        public void ClearGenerated()
        {
            for (int i = 0; i < _instantiated.Count; i++)
            {
                GameObject go = _instantiated[i];
                if (go == null)
                    continue;
                if (Application.isPlaying)
                    Destroy(go);
                else
                    DestroyImmediate(go);
            }
            _instantiated.Clear();
        }

        // --- Selection helpers ---------------------------------------------------

        private string PickCryptFinalId()
        {
            const string fallback = "CryptChamberFinal";
            if (registry == null)
                return fallback;
            CastleRoomModuleData entry = registry.GetById(fallback);
            return entry != null ? entry.RoomId : fallback;
        }

        /// <summary>Weighted random RoomId from a zone pool; falls back to a synthetic id.</summary>
        private static string PickWeighted(List<CastleRoomModuleData> pool, System.Random rng,
            CastleZone zone)
        {
            if (pool == null || pool.Count == 0)
                return zone + "_Room";

            int total = 0;
            for (int i = 0; i < pool.Count; i++)
                total += Mathf.Max(1, pool[i].Weight);

            int roll = rng.Next(0, total);
            for (int i = 0; i < pool.Count; i++)
            {
                roll -= Mathf.Max(1, pool[i].Weight);
                if (roll < 0)
                    return pool[i].RoomId;
            }
            return pool[pool.Count - 1].RoomId;
        }

        // --- Grid helpers --------------------------------------------------------

        /// <summary>All grid cells whose Chebyshev distance from the origin equals <paramref name="r"/>.</summary>
        private static List<Vector2Int> RingCells(int r)
        {
            var cells = new List<Vector2Int>();
            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    if (Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) == r)
                        cells.Add(new Vector2Int(x, y));
                }
            }
            return cells;
        }

        private static int Chebyshev(Vector2Int c) => Mathf.Max(Mathf.Abs(c.x), Mathf.Abs(c.y));

        private static readonly Vector2Int[] FourDirs =
        {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1)
        };

        /// <summary>Finds a placed 4-neighbour of <paramref name="cell"/>, preferring inward ones.</summary>
        private static bool TryFindPlacedNeighbour(Dictionary<Vector2Int, int> occupied,
            Vector2Int cell, out Vector2Int neighbour)
        {
            int cellRing = Chebyshev(cell);
            neighbour = default;
            bool found = false;
            foreach (Vector2Int d in FourDirs)
            {
                Vector2Int n = cell + d;
                if (!occupied.ContainsKey(n))
                    continue;
                // Prefer an inward neighbour (closer to center) for the strongest connectivity.
                if (Chebyshev(n) < cellRing)
                {
                    neighbour = n;
                    return true;
                }
                if (!found)
                {
                    neighbour = n;
                    found = true;
                }
            }
            return found;
        }

        /// <summary>Deterministic Fisher–Yates shuffle driven by the seeded RNG.</summary>
        private static void Shuffle<T>(IList<T> list, System.Random rng)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>Yaw so the module's forward (+Z, local North) points along a grid facing.</summary>
        private static Quaternion RotationForFacing(Vector2Int facing)
        {
            if (facing == Vector2Int.zero)
                return Quaternion.identity;
            var dir = new Vector3(facing.x, 0f, facing.y);
            return Quaternion.LookRotation(dir, Vector3.up);
        }

        private void EnsureContainer()
        {
            if (roomContainer != null)
                return;
            var container = new GameObject("GeneratedCastle");
            container.transform.SetParent(transform, false);
            roomContainer = container.transform;
        }
    }
}
