using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    void Update()
    {
        // Detecta si se hace clic en cualquier lugar de la pantalla
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            Destroy(gameObject); // Destruye el objeto actual
        }
    }
}
