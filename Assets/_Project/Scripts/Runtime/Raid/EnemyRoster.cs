using System;
using System.Collections.Generic;
using RogueAi.Castle;
using UnityEngine;

namespace RogueAi.Raid
{
    /// <summary>
    /// Which enemy stands where. One entry per enemy prefab, tagged with the castle zone it garrisons
    /// and a weight for how commonly it turns up there.
    ///
    /// This is the garrison's half of the risk curve that <see cref="RaidLootTable"/> draws for loot:
    /// the deep rooms hold the valuable things, and they hold the dangerous things for the same
    /// reason. A single enemy prefab would have made every room the same encounter.
    ///
    /// <see cref="GuardPlacementPlanner"/> already decides how many guards stand where and what they
    /// walk; the roster only answers "which one", so the plan stays a pure function of the seed.
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyRoster", menuName = "Plunderspell/Enemy Roster")]
    public class EnemyRoster : ScriptableObject
    {
        [Serializable]
        public class Entry
        {
            [Tooltip("Stable identifier, matching the source model name.")]
            public string EnemyId;

            [Tooltip("Which zone this enemy garrisons.")]
            public CastleZone Zone = CastleZone.OuterBailey;

            [Tooltip("Relative frequency within its zone. Higher is more common.")]
            [Min(1)]
            public int Weight = 10;

            [Tooltip("Prefab spawned for this enemy.")]
            public GameObject Prefab;
        }

        [Tooltip("Every enemy prefab, tagged by the zone it garrisons.")]
        public List<Entry> Entries = new List<Entry>();

        /// <summary>Every spawnable entry belonging to a zone, in authored order.</summary>
        public List<Entry> EntriesFor(CastleZone zone)
        {
            var result = new List<Entry>();
            for (int i = 0; i < Entries.Count; i++)
            {
                Entry entry = Entries[i];
                if (entry != null && entry.Zone == zone && entry.Prefab != null)
                    result.Add(entry);
            }
            return result;
        }

        /// <summary>
        /// Weighted pick from a zone's pool, or null when the zone has no entries. Takes the caller's
        /// RNG so the choice stays part of the seed-derived plan rather than a separate random source.
        /// </summary>
        public GameObject PickForZone(CastleZone zone, System.Random rng)
        {
            List<Entry> pool = EntriesFor(zone);
            if (pool.Count == 0)
                return null;

            int total = 0;
            for (int i = 0; i < pool.Count; i++)
                total += Mathf.Max(1, pool[i].Weight);

            int roll = rng.Next(0, total);
            for (int i = 0; i < pool.Count; i++)
            {
                roll -= Mathf.Max(1, pool[i].Weight);
                if (roll < 0)
                    return pool[i].Prefab;
            }
            return pool[pool.Count - 1].Prefab;
        }
    }
}
