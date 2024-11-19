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
            // Si es host, controla Player1
            if (!player1Movement.isControlled) player1Movement.isControlled = true;
            if (player2Movement.isControlled) player2Movement.isControlled = false;

            PlayerData playerData = Player1.GetComponent<PlayerDataManager>().data;
            serverUDP.SendPlayerData(playerData);
        }
        else
        {
            // Si es cliente, controla Player2
            if (player1Movement.isControlled) player1Movement.isControlled = false;
            if (!player2Movement.isControlled) player2Movement.isControlled = true;

            PlayerData playerData = Player2.GetComponent<PlayerDataManager>().data;
            clientUDP.SendPlayerData(playerData);
        }

        // Procesar posiciones desde la cola
        while (playerDataQueue.TryDequeue(out PlayerData playerData))
        {
            // Convert the position string back to Vector3
            Vector3 position = playerData.Position;

            if (playerData.Id == 0)
            {
                Player1.GetComponent<PlayerDataManager>().SetPlayerValues(playerData);
            }
            else if (playerData.Id == 1)
            {
                Player2.GetComponent<PlayerDataManager>().SetPlayerValues(playerData);
            }
        }
    }
    
    public void EnqueuePlayerData(PlayerData pData)
    {
        playerDataQueue.Enqueue(pData);
    }
}