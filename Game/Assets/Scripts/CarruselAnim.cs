using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CarruselAnim : MonoBehaviour
{
    [SerializeField] ScriptableBool isTutorial;

    private void Update()
    {
        if (GetComponent<VideoPlayer>().isPaused)
        {
            if (isTutorial.Bool)
            {
                SceneManager.LoadScene("HappyTutotial");
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }
}
