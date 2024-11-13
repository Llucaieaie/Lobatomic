using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform imageRectTransform;
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = Vector3.one * 1.2f; // Tamaño al hacer hover (ajusta el valor)

    // Al hacer hover sobre el botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        imageRectTransform.localScale = hoverScale;
    }

    // Al salir del hover
    public void OnPointerExit(PointerEventData eventData)
    {
        imageRectTransform.localScale = normalScale;
    }
}
