using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] ScriptableBool isTutorial;
    public void PlayGame()
    {
        isTutorial.Bool = false; 
        SceneManager.LoadScene("AnimScene");
    }

    public void SkipIntro()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void PlayCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void TurntoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Tutorial()
    {
        isTutorial.Bool = true;
        SceneManager.LoadScene("HappyTutotial");
    }

    public void NextTutorial()
    {
        SceneManager.LoadScene("SadTutotial");
    }
}
