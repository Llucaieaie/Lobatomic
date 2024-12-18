using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start1vs1Game : MonoBehaviour
{
    private Button startButton;
    public OnlineGameManager lobbyManager;

    private void Start()
    {
        startButton = GetComponent<Button>();
        startButton.interactable = false;
    }

    private void Update()
    {
        //if (lobbyManager.Player1.GetComponent<PlayerDataManager>().isControlled && lobbyManager.Player2.GetComponent<PlayerDataManager>().isControlled)
        //{
        //    startButton.interactable = true;
        //}
    }

    public void StartGame()
    {
        SceneManager.LoadScene("VersusScene");
    }
}
