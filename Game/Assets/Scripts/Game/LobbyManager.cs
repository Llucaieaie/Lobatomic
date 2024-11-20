using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyTileMap;

    public GameObject Player1;
    public GameObject Player2;

    public ServerUDP serverUDP;
    public ClientUDP clientUDP;

    public bool isHost;

    private ConcurrentQueue<PlayerData> playerDataQueue = new ConcurrentQueue<PlayerData>();

    void Start()
    {
        Player1.GetComponent<PlayerDataManager>().data.Id = 0;
        Player2.GetComponent<PlayerDataManager>().data.Id = 1;
    }

    void Update()
    {
        PlayerMovementOnline player1Movement = Player1.GetComponent<PlayerMovementOnline>();
        PlayerMovementOnline player2Movement = Player2.GetComponent<PlayerMovementOnline>();

        if (isHost)
        {
            // If is host, control Player1
            if (!player1Movement.dataManager.isControlled) player1Movement.dataManager.isControlled = true;
            if (player2Movement.dataManager.isControlled) player2Movement.dataManager.isControlled = false;

            PlayerData playerData = Player1.GetComponent<PlayerDataManager>().data;
            serverUDP.SendPlayerData(playerData);
        }
        else
        {
            // If is client, control Player2
            if (player1Movement.dataManager.isControlled) player1Movement.dataManager.isControlled = false;
            if (!player2Movement.dataManager.isControlled) player2Movement.dataManager.isControlled = true;

            PlayerData playerData = Player2.GetComponent<PlayerDataManager>().data;
            clientUDP.SendPlayerData(playerData);
        }

        // Process data from queue
        while (playerDataQueue.TryDequeue(out PlayerData playerData))
        {
            if (playerData.Id == 0)
            {
                Player1.GetComponent<PlayerDataManager>().SetPlayerValues(playerData);
            }
            else if (playerData.Id == 1)
            {
                Player2.GetComponent<PlayerDataManager>().SetPlayerValues(playerData);
            }

            // Convert the position string back to Vector3
            Vector3 position = playerData.Position;
        }
    }
    
    public void EnqueuePlayerData(PlayerData pData)
    {
        playerDataQueue.Enqueue(pData);
    }
}