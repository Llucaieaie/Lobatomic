using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInputManagerForServer : MonoBehaviour
{
    public TMP_InputField nameInputField; // Referencia al TMP_InputField del Nombre
    public TMP_InputField seedInputField; // Referencia al TMP_InputField del Nombre
    public ServerUDP server;              // Referencia al script con la variable pública

    public void OnNameChanged()
    {
        server.hostName = nameInputField.text;
    }

    public void OnSeedChanged()
    {
        if (int.TryParse(seedInputField.text, out int seedValue))
        {
            server.seed = seedValue;
        }
        else
        {
            Debug.LogError("Invalid value");
        }
    }
}
