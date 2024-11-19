using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyTileMap;
    public GameObject playerPrefab;

    public List<GameObject> players = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        lobbyTileMap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerPosition(int playerId, Vector3 position)
    {
        if (playerId < players.Count)
        {
            players[playerId].transform.position = position;
        }
        else
        {
            Debug.LogWarning($"Player ID {playerId} does not exist.");
        }
    }

    //public void AddPlayer(string name)
    //{
    //    GameObject p = Instantiate(playerPrefab);
    //    players.Add(p);

    //    p.GetComponentInChildren<TMP_Text>().text = name;
    //}
}
