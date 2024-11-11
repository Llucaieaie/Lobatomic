using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewModeTextScript : MonoBehaviour
{
    [SerializeField] private RectTransform imageTransform; // La referencia a la imagen (RectTransform)
    [SerializeField] private float scaleFactor = 0.6f; // Cu�nto cambia el tama�o
    [SerializeField] private float speed = 2f; // La velocidad del efecto
    [SerializeField] private float baseScale = 1f; // Tama�o base de la imagen

    private void Update()
    {
        // Calculamos el nuevo tama�o usando una funci�n sinusoidal
        float scale = baseScale + Mathf.Sin(Time.time * speed) * scaleFactor;
        imageTransform.localScale = new Vector3(scale, scale, 1f);
    }
}
