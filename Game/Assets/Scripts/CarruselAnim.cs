using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarruselAnim : MonoBehaviour
{
    [SerializeField] Sprite[] carrusel;
    [SerializeField] ScriptableBool isTutorial;
    int currentCarrusel;

    private void Start()
    {
        currentCarrusel = 0;
        GetComponent<Image>().sprite = carrusel[currentCarrusel];
        StartCoroutine(StartCarrusel());
    }

    IEnumerator StartCarrusel()
    {

        if (isTutorial.Bool)
        {
            SceneManager.LoadScene("HappyTutotial");
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }

        yield return null;
    }
}
