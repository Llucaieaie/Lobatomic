using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewModeTextScript : MonoBehaviour
{
    [SerializeField] private RectTransform imageTransform; // La referencia a la imagen (RectTransform)
    [SerializeField] private float scaleFactor = 0.6f; // Cuánto cambia el tamaño
    [SerializeField] private float speed = 2f; // La velocidad del efecto
    [SerializeField] private float baseScale = 1f; // Tamaño base de la imagen

    private void Update()
    {
        // Calculamos el nuevo tamaño usando una función sinusoidal
        float scale = baseScale + Mathf.Sin(Time.time * speed) * scaleFactor;
        imageTransform.localScale = new Vector3(scale, scale, 1f);
    }
}
