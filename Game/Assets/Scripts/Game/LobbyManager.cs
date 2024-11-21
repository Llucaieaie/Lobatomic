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
        PlayerDataManager player1DataManager = Player1.GetComponent<PlayerDataManager>();
        PlayerDataManager player2DataManager = Player2.GetComponent<PlayerDataManager>();

        if (isHost)
        {
            // If is host, control Player1
            if (!player1DataManager.isControlled) player1DataManager.isControlled = true;
            if (player2DataManager.isControlled) player2DataManager.isControlled = false;

            serverUDP.SendPlayerData(player1DataManager.data);
        }
        else
        {
            // If is client, control Player2
            if (player1DataManager.isControlled) player1DataManager.isControlled = false;
            if (!player2DataManager.isControlled) player2DataManager.isControlled = true;

            clientUDP.SendPlayerData(player2DataManager.data);
        }

        // Process data from queue
        while (playerDataQueue.TryDequeue(out PlayerData playerData))
        {
            if (playerData.Id == 0)
            {
                player1DataManager.SetPlayerValues(playerData);
            }
            else if (playerData.Id == 1)
            {
                player2DataManager.SetPlayerValues(playerData);
            }
        }
    }

    public void SetPlayerActive(int id, bool active)
    {
        if (id == 0) Player1.SetActive(active);
        else Player2.SetActive(active);
    }

    public void EnqueuePlayerData(PlayerData pData)
    {
        playerDataQueue.Enqueue(pData);
    }
}