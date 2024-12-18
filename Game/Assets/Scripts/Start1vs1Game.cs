using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start1vs1Game : MonoBehaviour
{
    private Button startButton;
    public OnlineGameManager onlineGameManager;

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
        SceneManager.LoadScene("VersusScene");
    }
}
