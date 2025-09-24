using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public static RoomSpawner Instance;
    
    [SerializeField] private List<GameObject> availableRooms = new();

    [SerializeField] private Grid grid;

    private Dictionary<Vector3Int, ProceduralRoom> _spawnedRooms = new();
    
    [SerializeField] private Vector3 generationBounds;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    public void GenerateRoomAtWorldPosition(Transform spawnTransform)
    {
        var gridCell = grid.WorldToCell(spawnTransform.position);
        var randomRoom = Random.Range(0, availableRooms.Count);
        Instantiate(availableRooms[randomRoom], grid.CellToWorld(gridCell), Quaternion.identity);
    }

    public bool CheckPlacementValid(Vector3 position)
    {
        var cell = grid.WorldToCell(position);

        var valid = true;

        // check if space is already occupied
        if (_spawnedRooms.ContainsKey(cell))
        {
            valid = false;
        }
        // check if space is beyond the bounds of the generationa
        else if(Mathf.Abs(cell.x) > generationBounds.x || Mathf.Abs(cell.z) > generationBounds.z)
        {
            valid = false;
        }
        
        return valid;
    }

    public void RegisterWithDictionary(ProceduralRoom room)
    {
        var cell = grid.WorldToCell(room.transform.position);
        _spawnedRooms.Add(cell, room);
    }
}
