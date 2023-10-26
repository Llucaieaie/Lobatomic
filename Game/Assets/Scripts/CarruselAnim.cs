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
        yield return new WaitForSeconds(1);

        if (currentCarrusel >= carrusel.Length)
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
        else 
        {
            StartCoroutine(StartCarrusel());
        }

        currentCarrusel++;
        GetComponent<Image>().sprite = carrusel[currentCarrusel];

        yield return null;
    }
}
