using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start1vs1Game : MonoBehaviour
{
    private Button startButton;
    public OnlineGameManager onlineGameManager;

    public ServerUDP serverUDP;

    private void Start()
    {
        startButton = GetComponent<Button>();
        startButton.interactable = false;
    }

    private void Update()
    {
        if (onlineGameManager.Player1.activeInHierarchy && onlineGameManager.Player2.activeInHierarchy)
        {
            startButton.interactable = true;
        }
    }

    public void StartGame()
    {
        if (serverUDP.seed == 0)
        {
            serverUDP.seed = Random.Range(1, int.MaxValue);
            Debug.Log("Generated Seed: " + serverUDP.seed);
        }
        UnityEngine.Random.InitState(serverUDP.seed);

        serverUDP.SendCommandToClient("SetSeed", serverUDP.seed);
        SceneManager.LoadScene("VersusScene");
    }
}
