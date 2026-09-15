using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueAi.Castle
{
    /// <summary>
    /// Serializable description of a single room prefab available to the generator.
    /// The <see cref="Prefab"/> reference is filled in by designers; the generation logic works
    /// with null prefabs (placement is computed from metadata) so the pipeline is testable.
    /// </summary>
    [Serializable]
    public class CastleRoomModuleData
    {
        [Tooltip("Stable identifier, must match the CastleRoomModule.RoomId on the prefab.")]
        public string RoomId;

        [Tooltip("Which concentric zone this room belongs to.")]
        public CastleZone Zone;

        [Tooltip("Prefab to instantiate. May be null in data-only/test scenarios.")]
        public GameObject Prefab;

        [Tooltip("Relative spawn frequency within its zone's weighted pool (>=1).")]
        public int Weight = 1;
    }

    /// <summary>
    /// ScriptableObject catalogue of every room module, grouped conceptually by zone. Feeds the
    /// <see cref="ProceduralCastleGenerator"/> weighted selection.
    /// </summary>
    [CreateAssetMenu(fileName = "CastleRoomRegistry", menuName = "RogueAi/Castle/Room Registry")]
    public class CastleRoomRegistry : ScriptableObject
    {
        [Tooltip("All room modules known to the generator.")]
        public List<CastleRoomModuleData> Modules = new List<CastleRoomModuleData>();

        /// <summary>Returns all modules that belong to the requested zone.</summary>
        public List<CastleRoomModuleData> GetModulesForZone(CastleZone zone)
        {
            var result = new List<CastleRoomModuleData>();
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] != null && Modules[i].Zone == zone)
                    result.Add(Modules[i]);
            }
            return result;
        }

        /// <summary>Finds a module entry by its RoomId, or null if not present.</summary>
        public CastleRoomModuleData GetById(string roomId)
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i] != null && Modules[i].RoomId == roomId)
                    return Modules[i];
            }
            return null;
        }
    }
}
