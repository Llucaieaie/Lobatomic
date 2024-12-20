using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    public ServerUDP serverUDP;
    public ClientUDP clientUDP;

    public bool isHost;

    private ConcurrentQueue<PlayerData> playerDataQueue = new ConcurrentQueue<PlayerData>();

    // LISTA DE TILES
    public List<GameObject> currentTiles = new List<GameObject>();

    void Start()
    {
        Player1.GetComponent<PlayerDataManager>().data.Id = 0;
        Player2.GetComponent<PlayerDataManager>().data.Id = 1;
        DontDestroyOnLoad(gameObject);
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
            player1DataManager.data.destroyedTileIDs.Clear();
        }
        else
        {
            // If is client, control Player2
            if (player1DataManager.isControlled) player1DataManager.isControlled = false;
            if (!player2DataManager.isControlled) player2DataManager.isControlled = true;

            clientUDP.SendPlayerData(player2DataManager.data);
            player2DataManager.data.destroyedTileIDs.Clear();
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

            // Destroy tiles according to recieved data
            DestroyTileByID(playerData.destroyedTileIDs);
        }
    }

    public void SetPlayerActive(int id, bool active)
    {
        if (id == 0) Player1.SetActive(active);
        else Player2.SetActive(active);
    }

    public void ClearTileList()
    {
        currentTiles.Clear();
    }

    public void EnqueuePlayerData(PlayerData pData)
    {
        playerDataQueue.Enqueue(pData);
    }

    public void DestroyTileByID(List<int> IDList)
    {
        foreach (int tileID in IDList)
        {
            DestroyTileByID(tileID);
        }
    }

    public void DestroyTileByID(int id)
    {
        List<GameObject> tilesToRemove = new List<GameObject>();

        foreach (var tileObj in currentTiles)
        {
            var tile = tileObj?.GetComponent<Tile>();
            if (tile != null && tile.tileID == id)
            {
                Debug.Log("OGM destroyed a tile with ID " + id);

                tile.OnExplosion();
                tilesToRemove.Add(tileObj); // Mark for elimination
            }
        }

        foreach (var tileObj in tilesToRemove)
        {
            currentTiles.Remove(tileObj);
        }
    }
}