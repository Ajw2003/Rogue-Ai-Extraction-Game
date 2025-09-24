using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeedManager : MonoBehaviour
{
    [SerializeField] private bool randomSeed;
    [SerializeField] private int seed;
    private int _activeSeed;
    
    private void Awake()
    {
        if (randomSeed)
        {
            return;
        }
        Random.InitState(seed);
    }

    /*private void Update()
    {
        UpdateSeed();
    }*/

    /*private void UpdateSeed()
    {
        _activeSeed += (int)(Time.deltaTime * 100);
        Random.InitState(_activeSeed);
    }*/
}
