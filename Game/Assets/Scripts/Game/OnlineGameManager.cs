using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
    public List<Vector3Int> occupiedTilePositions = new List<Vector3Int>();

    public MapGeneratorOnline mapGenerator;

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
            player1DataManager.data.destroyedTilePos.Clear();
        }
        else
        {
            // If is client, control Player2
            if (player1DataManager.isControlled) player1DataManager.isControlled = false;
            if (!player2DataManager.isControlled) player2DataManager.isControlled = true;

            clientUDP.SendPlayerData(player2DataManager.data);
            player2DataManager.data.destroyedTilePos.Clear();
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
            DestroyTileAtPosition(playerData.destroyedTilePos);
        }
    }

    public void SetPlayerActive(int id, bool active)
    {
        if (id == 0) Player1.SetActive(active);
        else Player2.SetActive(active);
    }

    public void SetCurrentTilesLists(List<GameObject> list)
    {
        currentTiles = new List<GameObject>(list);
        
        foreach (var tileGO in currentTiles)
        {
            Vector3Int tilePos = new Vector3Int((int)tileGO.transform.position.x, (int)tileGO.transform.position.y, (int)tileGO.transform.position.z);
            occupiedTilePositions.Add(tilePos);
        }
    }

    public void ClearTileList()
    {
        currentTiles.Clear();
        occupiedTilePositions.Clear();
    }

    public void EnqueuePlayerData(PlayerData pData)
    {
        playerDataQueue.Enqueue(pData);
    }

    // Las funciones se han actualizado para usar la posicion de la tile en vez de una ID para optimizar
    public void DestroyTileAtPosition(List<TilePosition> pList)
    {
        for (int i = 0; i < pList.Count; i++)
        {
            DestroyTileAtPosition(pList[i].GetPos());
        }
    }

    public void DestroyTileAtPosition(Vector3Int pos)
    {
        int index = occupiedTilePositions.Select((pos, index) => new { pos, index }).FirstOrDefault(item => item.pos == pos)?.index ?? -1;
        if (index != -1)
        {
            currentTiles[index].GetComponent<Tile>().OnExplosion();
            currentTiles.Remove(currentTiles[index]);
            occupiedTilePositions.Remove(pos);
        }
    }
}