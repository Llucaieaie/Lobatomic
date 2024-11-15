using UnityEngine;
using TMPro;

public class TextInputManagerForClient : MonoBehaviour
{
    public TMP_InputField nameInputField; // Referencia al TMP_InputField del Nombre
    public TMP_InputField IPInputField;   // Referencia al TMP_InputField de la IP
    public ClientUDP client;   // Referencia al script con la variable pública

    // Método que se llamará cuando cambie el texto del InputField
    public void OnNameChanged()
    {
        client.clientName = nameInputField.text;
    }

    public void OnIPChanged()
    {
        client.serverIP = IPInputField.text;
    }
}
