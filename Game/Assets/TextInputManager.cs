using UnityEngine;
using TMPro;

public class TextInputManager : MonoBehaviour
{
    public TMP_InputField inputField; // Referencia al TMP_InputField
    public ClientUDP gameManager;   // Referencia al script con la variable p�blica

    // M�todo que se llamar� cuando cambie el texto del InputField
    public void OnTextChanged()
    {
        gameManager.playerName = inputField.text;
    }
}
