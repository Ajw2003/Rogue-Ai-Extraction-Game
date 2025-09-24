using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralRoom : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new();

    [SerializeField] private bool randomizeRotation;

    [SerializeField] private List<float> validRotations = new();

    private void Awake()
    {
        // First check if the object is spawned on top of another object
        if(RoomSpawner.Instance.CheckPlacementValid(transform.position) == false)
        {
            // If placement is invalid, destroy self and return
            Destroy(gameObject);
            return;
        }
        
        // register this room in the room spawner's dictionary
        RoomSpawner.Instance.RegisterWithDictionary(this);

        GenerationProcess();
    }   

    private void GenerationProcess()
    {
        // pick a random valid rotation
        PickRandomRotation();
        
        // generate branches
        GenerateBranches();
    }

    private void GenerateBranches()
    {
        // We need to decide how many branches this room will have, then iterate through each one, a room can't have zero branches
        var branchCount = Random.Range(1, spawnPoints.Count);

        for (var i = 0; i < branchCount; i++)
        {
            var randomRoom = Random.Range(0, spawnPoints.Count);

            var pointsChecked = 0;
            while(RoomSpawner.Instance.CheckPlacementValid(spawnPoints[randomRoom].position) == false)
            {
                randomRoom = Random.Range(0, spawnPoints.Count);
                pointsChecked++;
                
                if (pointsChecked >= spawnPoints.Count)
                {
                    return;
                }
            }
            
            var validRoom = spawnPoints[randomRoom];
            RoomSpawner.Instance.GenerateRoomAtWorldPosition(validRoom);
        }
    }

    private void PickRandomRotation()
    {
        if (randomizeRotation == false)
        {
            return;
        }

        var index = Random.Range(0, validRotations.Count);

        var eulerAngle = new Vector3(0, validRotations[index], 0);
        transform.eulerAngles = eulerAngle;
    }
}
