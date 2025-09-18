using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public bool connected;
    public bool inGame;
    public bool isHost;
    public ulong myClientId;

    public void HostCreated()
    {
        
    }

    public void ConnectedAsClient()
    {
        
    }

    public void OnDisconnected()
    {
        
    }

    public void Quit()
    {
        
    }
}   
