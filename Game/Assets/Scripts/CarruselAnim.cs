using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarruselAnim : MonoBehaviour
{
    [SerializeField] Sprite[] carrusel;
    [SerializeField] ScriptableBool isTutorial;
    int currentCarrusel;

    private void Start()
    {
        currentCarrusel = 0;
        GetComponent<SpriteRenderer>().sprite = carrusel[currentCarrusel];
        StartCoroutine(StartCarrusel());
    }

    IEnumerator StartCarrusel()
    {
        yield return new WaitForSeconds(1);

        if (currentCarrusel > carrusel.Length)
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

        currentCarrusel++;
        GetComponent<SpriteRenderer>().sprite = carrusel[currentCarrusel];

        StartCoroutine(StartCarrusel());
        yield return null;
    }
}
