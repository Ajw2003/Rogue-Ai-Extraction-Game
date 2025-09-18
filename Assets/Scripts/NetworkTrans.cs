using System.Collections.Generic;
using UnityEngine;
using Netcode;
using Unity.Netcode;

public class NetworkTrans : NetworkBehaviour
{
   public static NetworkTrans instance;

   private void Awake()
   {
      if (instance != null)
      {
         Destroy(this.gameObject);
      }
      else
      {
         instance = this;
      }
   }

   [ServerRpc(RequireOwnership = false)]

   public void SendAChatServerRPC(string msg, ulong name)
   {
      ChatFromServerClientRPC(msg, name);
   }

   [ClientRpc]
   private void ChatFromServerClientRPC(string msg, ulong name)
   {
      GameManager.instance.SendMessageToChat(msg, name, false);
   }

   [ServerRpc(RequireOwnership = false)]
   public void AddToServerDictonaryServerRPC(ulong steamId, string steamName, ulong clientId)
   {
      GameManager.instance.SendMessageToChat($"{steamName} has joined", clientId, true);
   }

   [ServerRpc(RequireOwnership = false)]
   public void RemoveFromServerDictonaryServerRPC(ulong steamId)
   {
      
   }

   [ClientRpc]
   private void RemovePlayerFromDictonaryClientRPC(ulong steamId)
   {
      //remove from dictonary
   }

   [ClientRpc]
   public void UpdateClientsPlayerInfoClientRPC(ulong steamId, string steamName, ulong clientId)
   {
      GameManager.instance.AddPlayerToDictionary(clientId, steamName, steamId);
   }

   [ServerRpc(RequireOwnership = false)]
   public void IsTheClientReadyServerRPC(bool ready, ulong clientId)
   {
      
   }

   [ClientRpc]
   private void AClientIsReadyingClientRPC(bool ready, ulong clientId)
   {
      foreach (KeyValuePair<ulong,GameObject> player in GameManager.instance.playerinfo)
      {
         if (player.Key == clientId)
         {
            player.Value.GetComponent<PlayerInfo>().isReady = ready;
            player.Value.GetComponent<PlayerInfo>().readyImage.SetActive(ready);

            if (NetworkManager.Singleton.IsHost)
            {
               //check if players ready
            }
         }
      }
   }
}
