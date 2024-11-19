using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyTileMap;
    public GameObject playerPrefab;

    public GameObject Player1;
    public GameObject Player2;

    public ServerUDP serverUDP;
    public ClientUDP clientUDP;

    public bool isHost;

    void Start()
    {
        
    }

    void Update()
    {
        PlayerMovementOnline player1Movement = Player1.GetComponent<PlayerMovementOnline>();
        PlayerMovementOnline player2Movement = Player2.GetComponent<PlayerMovementOnline>();

        if (isHost)
        {
            // If host, control player1
            if (!player1Movement.isControlled) player1Movement.isControlled = true;
            if (player2Movement.isControlled) player2Movement.isControlled = false;

            Vector3 hostPosition = Player1.transform.position;
            serverUDP.SendPosition(hostPosition);
        }
        else
        {
            // If host, control player2
            if (player1Movement.isControlled) player1Movement.isControlled = false;
            if (!player2Movement.isControlled) player2Movement.isControlled = true;

            Vector3 clientPosition = Player2.transform.position;
            clientUDP.SendPosition(clientPosition);
        }
    }

    public void UpdatePlayerPosition(int playerId, Vector2 position)
    {
        if (playerId == 0)
            Player1.transform.position = position;
        else if (playerId == 1)
            Player2.transform.position = position;
    }
}
