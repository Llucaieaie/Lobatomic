using UnityEngine;
using TMPro;

public class TextInputManager : MonoBehaviour
{
    public TMP_InputField inputField; // Referencia al TMP_InputField
    public ClientUDP gameManager;   // Referencia al script con la variable pública

    // Método que se llamará cuando cambie el texto del InputField
    public void OnTextChanged()
    {
        gameManager.playerName = inputField.text;
    }
}
