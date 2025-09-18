using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject menu, lobbyMenu;

    [SerializeField] private GameObject chatPanel, textObject;
    
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private int maxMessages = 20;

    [SerializeField] private GameObject playerFeildBox, playerCard;
    
    private List<Message> messageList = new List<Message>();
    
    public Dictionary<ulong, GameObject> playerinfo = new Dictionary<ulong, GameObject>();
    
    public bool connected;
    public bool inGame;
    public bool isHost;
    public ulong myClientId;

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

    private void Update()
    {
        if (inputField.text != "")
        {
            if (inputField.text == " ")
            {
                inputField.text = "";
                inputField.DeactivateInputField();
                return;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                inputField.ActivateInputField();
                inputField.text = " ";
            }
        }
    }

    public void SendMessageToChat(string text, ulong player, bool server)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.RemoveAt(0);
        }
        Message newMessage = new Message();
        string name = "Server";
        
        newMessage.text = name + ": " + text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;
        
        messageList.Add(newMessage);
    }

    public void ClearChat()
    {
        messageList.Clear();
        GameObject[] chat = GameObject.FindGameObjectsWithTag("ChatMessage");
        foreach (GameObject chatMessage in chat)
        {
            Destroy(chatMessage);
        }
    }

    public class Message
    {
        public string text;
        public TMP_Text textObject;
    }

    public void HostCreated()
    {
        menu.SetActive(false);
        lobbyMenu.SetActive(true);
        connected = true;
        isHost = true;
    }

    public void ConnectedAsClient()
    {
        menu.SetActive(false);
        lobbyMenu.SetActive(true);
        connected = true;
        isHost = false;
    }

    public void OnDisconnected()
    {
        playerinfo.Clear();
        GameObject[] playerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
        foreach (GameObject card in playerCards)
        {
            Destroy(card);
        }
        menu.SetActive(true);
        lobbyMenu.SetActive(false);
        connected = false;
    }

    public void AddPlayerToDictionary(ulong clientId, string steamName, ulong steamId)
    {
        if (!playerinfo.ContainsKey(clientId))
        {
            PlayerInfo pi = Instantiate(playerCard, playerFeildBox.transform).GetComponent<PlayerInfo>();
            pi.steamName = steamName;
            pi.steamId = steamId;
            playerinfo.Add(clientId, pi.gameObject);
        }
    }

    public void UpdateClients()
    {
        foreach (KeyValuePair<ulong, GameObject> player in playerinfo)
        {
            ulong steamId = player.Value.GetComponent<PlayerInfo>().steamId;
            string steamName = player.Value.GetComponent<PlayerInfo>().steamName;
            ulong clientId = player.Key;
            
            NetworkTrans.instance.UpdateClientsPlayerInfoClientRPC(steamId, steamName, steamId);
        }
    }

    public void RemovePlayerFromDictionary(ulong steamId)
    {
        GameObject _value = null;
        ulong _key = 100;
        foreach (KeyValuePair<ulong, GameObject> player in playerinfo)
        {
            if (player.Value.GetComponent<PlayerInfo>().steamId == steamId)
            {
                _value = player.Value;
                _key = player.Key;
            }

            if (_key != 100)
            {
                playerinfo.Remove(_key);
            }

            if (_value != null)
            {
                Destroy(_value);
            }
        }


    }

    public void Quit()
    {
        Application.Quit();
    }
}   
