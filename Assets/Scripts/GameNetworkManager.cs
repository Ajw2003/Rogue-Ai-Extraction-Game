using System;
using UnityEngine;
using Unity.Netcode;
using Steamworks;
using Steamworks.Data;
using Netcode.Transports.Facepunch;
public class GameNetworkManager : MonoBehaviour
{
    public static GameNetworkManager instance { get; private set; } = null;

    private FacepunchTransport _transport = null;
    
    public Lobby? currentLobby {get; private set;} = null;

    public ulong hostId;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _transport = GetComponent<FacepunchTransport>();
        
        SteamMatchmaking.OnLobbyCreated += SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += SteamMatchmakingOnOnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += SteamMatchmakingOnOnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmakingOnOnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite += SteamMatchmakingOnOnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated += SteamMatchmakingOnOnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested += SteamFriendsOnOnGameLobbyJoinRequested;
        
    }

    private void OnDestroy()
    {
        SteamMatchmaking.OnLobbyCreated -= SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= SteamMatchmakingOnOnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= SteamMatchmakingOnOnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= SteamMatchmakingOnOnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite -= SteamMatchmakingOnOnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated -= SteamMatchmakingOnOnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested -= SteamFriendsOnOnGameLobbyJoinRequested;
        
        if(NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnServerStarted -= SingletonOnOnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= SingletonOnOnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= SingletonOnOnClientDisconnectCallback;
    }

    private void OnApplicationQuit()
    {
        OnDisconnected();
    }

    //when accepting invite or joining friend
    private async void SteamFriendsOnOnGameLobbyJoinRequested(Lobby lobby, SteamId steamId)
    {
        RoomEnter joinedLobby = await lobby.Join();
        if (joinedLobby != RoomEnter.Success)
        {
            Debug.Log(joinedLobby);
        }
        else
        {
            currentLobby = lobby;
            Debug.Log("Joined lobby");
            GameManager.instance.ConnectedAsClient();
        }
    }

    private void SteamMatchmakingOnOnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId steamId)
    {
        Debug.Log("lobby created");
    }

    private void SteamMatchmakingOnOnLobbyInvite(Friend steamId, Lobby lobby)
    {
        Debug.Log($"invite from{steamId}");
    }

    private void SteamMatchmakingOnOnLobbyMemberLeave(Lobby lobby, Friend steamId)
    {
        Debug.Log($"leave from{steamId}");
    }

    private void SteamMatchmakingOnOnLobbyMemberJoined(Lobby lobby, Friend steamId)
    {
        Debug.Log($"joined {steamId}");
    }

    private void SteamMatchmakingOnOnLobbyEntered(Lobby lobby)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            return;
        }

        if (currentLobby != null) StartClient(currentLobby.Value.Owner.Id);
    }

    private void SteamMatchmaking_OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result != Result.OK)
        {
            Debug.Log(result);
            return;
        }

        lobby.SetPublic();
        lobby.SetJoinable(true);
        lobby.SetGameServer(lobby.Owner.Id);
    }

    public async void StartHost(int maxMembers)
    {
        NetworkManager.Singleton.OnServerStarted += SingletonOnOnServerStarted;
        NetworkManager.Singleton.StartHost();
        currentLobby = await SteamMatchmaking.CreateLobbyAsync(maxMembers);
        GameManager.instance.myClientId = NetworkManager.Singleton.LocalClientId;
    }

    public void StartClient(SteamId steamId)
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += SingletonOnOnClientDisconnectCallback;
        _transport.targetSteamId = steamId;
        GameManager.instance.myClientId = NetworkManager.Singleton.LocalClientId;
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started");
        }
    }

    public void OnDisconnected()
    {
        currentLobby?.Leave();
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.OnServerStarted -= SingletonOnOnServerStarted;
        }
        else
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SingletonOnOnClientConnectedCallback;
        }
        NetworkManager.Singleton.Shutdown(true);
        GameManager.instance.OnDisconnected();
        Debug.Log("Disconnected");
    }

    private void SingletonOnOnClientDisconnectCallback(ulong obj)
    {
    }

    private void SingletonOnOnClientConnectedCallback(ulong obj)
    {
       
    }

    private void SingletonOnOnServerStarted()
    {
        Debug.Log("host started");
        GameManager.instance.HostCreated();
    }
}
