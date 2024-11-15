using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInputManagerForServer : MonoBehaviour
{
    public TMP_InputField nameInputField; // Referencia al TMP_InputField del Nombre
    public ServerUDP server;              // Referencia al script con la variable p�blica

    // M�todo que se llamar� cuando cambie el texto del InputField
    public void OnNameChanged()
    {
        server.hostName = nameInputField.text;
    }
}
