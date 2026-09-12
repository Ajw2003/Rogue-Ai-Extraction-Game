using System.Collections.Generic;
using NUnit.Framework;
using RogueAi.Castle;
using RogueAi.Spells;
using UnityEngine;

namespace RogueAi.Tests
{
    /// <summary>
    /// EditMode tests for the Milestone 2a procedural castle generator, socket compatibility and
    /// A* path validation. Also re-runs the M1 Levenshtein check to guard against regressions.
    /// </summary>
    public class CastleGeneratorTests
    {
        private readonly List<Object> _spawned = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (var o in _spawned)
                if (o != null)
                    Object.DestroyImmediate(o);
            _spawned.Clear();
        }

        private ProceduralCastleGenerator MakeGenerator()
        {
            var go = new GameObject("CastleGen");
            _spawned.Add(go);
            return go.AddComponent<ProceduralCastleGenerator>();
        }

        [Test]
        public void Test_DeterministicGeneration()
        {
            var gen = MakeGenerator();
            ProceduralCastleData a = gen.Generate(42);
            // Snapshot before regenerating (Generate clears/rebuilds internal state).
            var snapshot = new List<ProceduralCastleData.PlacedModule>(a.PlacedModules);

            ProceduralCastleData b = gen.Generate(42);

            Assert.AreEqual(snapshot.Count, b.PlacedModules.Count,
                "Same seed must yield the same number of modules.");
            for (int i = 0; i < snapshot.Count; i++)
            {
                Assert.AreEqual(snapshot[i].RoomId, b.PlacedModules[i].RoomId, $"RoomId mismatch at {i}");
                Assert.AreEqual(snapshot[i].Zone, b.PlacedModules[i].Zone, $"Zone mismatch at {i}");
                Assert.AreEqual(snapshot[i].GridPosition, b.PlacedModules[i].GridPosition,
                    $"GridPosition mismatch at {i}");
                Assert.That(Vector3.Distance(snapshot[i].Position, b.PlacedModules[i].Position),
                    Is.LessThan(0.001f), $"Position mismatch at {i}");
                Assert.That(Quaternion.Angle(snapshot[i].Rotation, b.PlacedModules[i].Rotation),
                    Is.LessThan(0.01f), $"Rotation mismatch at {i}");
            }
        }

        [Test]
        public void Test_AllZonesPresent()
        {
            var gen = MakeGenerator();
            ProceduralCastleData data = gen.Generate(12345);

            var zones = new HashSet<CastleZone>();
            foreach (var pm in data.PlacedModules)
                zones.Add(pm.Zone);

            foreach (CastleZone z in System.Enum.GetValues(typeof(CastleZone)))
                Assert.IsTrue(zones.Contains(z), $"Zone {z} missing from generated castle.");
        }

        [Test]
        public void Test_PathValidatorFindsPath()
        {
            var gen = MakeGenerator();
            ProceduralCastleData data = gen.Generate(42);

            bool found = CastlePathValidator.ValidatePath(data, out List<Vector2Int> path);
            Assert.IsTrue(found, "Expected a walkable crypt->extraction path for seed 42.");
            Assert.IsNotNull(path);
            Assert.Greater(path.Count, 0, "Path should contain at least one cell.");
        }

        [Test]
        public void Test_PathValidatorOnEmptyData()
        {
            var empty = new ProceduralCastleData(0);
            bool found = CastlePathValidator.ValidatePath(empty, out List<Vector2Int> path);
            Assert.IsFalse(found, "Empty layout must not produce a path.");
            Assert.IsNotNull(path);
            Assert.AreEqual(0, path.Count);
        }

        [Test]
        public void Test_SocketCompatibilityRules()
        {
            Assert.IsTrue(SocketPoint.AreTypesCompatible(SocketType.Door, SocketType.Door));
            Assert.IsTrue(SocketPoint.AreTypesCompatible(SocketType.Staircase, SocketType.Staircase));
            Assert.IsTrue(SocketPoint.AreTypesCompatible(SocketType.Window, SocketType.Window));
            Assert.IsTrue(SocketPoint.AreTypesCompatible(SocketType.ArchOpening, SocketType.ArchOpening));
            Assert.IsTrue(SocketPoint.AreTypesCompatible(SocketType.MurderHole, SocketType.WallSegment));
            Assert.IsTrue(SocketPoint.AreTypesCompatible(SocketType.WallSegment, SocketType.MurderHole));

            Assert.IsFalse(SocketPoint.AreTypesCompatible(SocketType.Door, SocketType.Window));
            Assert.IsFalse(SocketPoint.AreTypesCompatible(SocketType.Staircase, SocketType.Door));
            Assert.IsFalse(SocketPoint.AreTypesCompatible(SocketType.MurderHole, SocketType.MurderHole));
        }

        [Test]
        public void Test_ExtractionExitAssigned()
        {
            var gen = MakeGenerator();
            ProceduralCastleData data = gen.Generate(777);
            Assert.GreaterOrEqual(data.ExtractionExitIndex, 0, "An extraction exit must be assigned.");
            Assert.IsTrue(data.PlacedModules[data.ExtractionExitIndex].IsExtractionExit);
            Assert.AreEqual(CastleZone.OuterBailey,
                data.PlacedModules[data.ExtractionExitIndex].Zone,
                "Extraction exit should be an OuterBailey module.");
        }

        [Test]
        public void Test_LevenshteinFromM1_StillPasses()
        {
            Assert.AreEqual(1, MisfireEngine.LevenshteinDistance("IGNIS", "AGNIS"));
            Assert.AreEqual(1, MisfireEngine.LevenshteinDistance("FRANGO", "FRANCO"));
            Assert.AreEqual(0, MisfireEngine.LevenshteinDistance("PORTA", "PORTA"));
            Assert.AreEqual(5, MisfireEngine.LevenshteinDistance("", "PORTA"));
        }
    }
}
