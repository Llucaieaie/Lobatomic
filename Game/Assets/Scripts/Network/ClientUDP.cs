using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class ClientUDP : MonoBehaviour
{
    public GameObject UItextObj;
    public OnlineGameManager onlineGameManager;
    public GameObject createLobbyWindow;
    public GameObject invalidIPMessage;

    public string clientName = "";
    public string serverIP = "";
    
    private Socket socket;
    private Thread receiveThread;
    private TextMeshProUGUI UItext;
    private string clientText;

    private bool playRequest = false;
    
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        UItext.text = clientText;
        if (playRequest)
        {
            SceneManager.LoadScene("VersusScene");
            playRequest = false;
        }
    }

    public void StartClient()
    {
        // Configurar la dirección y el socket

        try
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), 9051);
            Debug.Log(localEndPoint);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEndPoint); // Asociar el socket al puerto local

            // Iniciar hilo de recepción
            receiveThread = new Thread(ReceiveData);
            receiveThread.Start();

            // Set ogm values =======================================
            onlineGameManager.Player2.GetComponent<PlayerDataManager>().SetName(clientName);
            onlineGameManager.SetPlayerActive(0, false);
            onlineGameManager.gameObject.SetActive(true);
            onlineGameManager.isHost = false;
            createLobbyWindow.SetActive(false);
        }
        catch (Exception e)
        {
            StartCoroutine(ShowErrorMessage());
        }
    }

    //void ReceiveData()
    //{
    //    byte[] data = new byte[1024];
    //    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

    //    while (true)
    //    {
    //        try
    //        {
    //            // Recibir datos del servidor
    //            int recv = socket.ReceiveFrom(data, ref remoteEndPoint);
    //            byte[] receivedBytes = new byte[recv];
    //            System.Array.Copy(data, receivedBytes, recv);

    //            PlayerData playerData = PlayerData.Deserialize(receivedBytes);

    //            onlineGameManager.EnqueuePlayerData(playerData);

    //            //Debug.Log($"Received PlayerData: Id={playerData.Id}, Name={playerData.Name}, Position={playerData.Position}");
    //        }
    //        catch (SocketException ex)
    //        {
    //            Debug.LogError($"Socket error while receiving data: {ex.Message}");
    //            break;
    //        }
    //        catch (System.Exception ex)
    //        {
    //            Debug.LogError($"Error while processing received data: {ex.Message}");
    //        }
    //    }
    //}

    public void SendPlayerData(PlayerData playerData)
    {
        onlineGameManager.SetPlayerActive(0, true);

        byte[] data = PlayerData.Serialize(playerData);

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket.SendTo(data, data.Length, SocketFlags.None, ipep);

        //clientText += $"\nPlayerData sent: {playerData.Name} at {playerData.Position}";
    }

    void ReceiveData()
    {
        byte[] data = new byte[1024];
        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            try
            {
                // Recibir datos del servidor
                int recv = socket.ReceiveFrom(data, ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data, 0, recv);

                if (message.Contains("|"))
                {
                    // Procesar como comando
                    string[] parts = message.Split('|');
                    string command = parts[0];
                    string parameter = parts.Length > 1 ? parts[1] : null;

                    ExecuteCommand(command, parameter);
                }
                else
                {
                    // Procesar como PlayerData
                    byte[] receivedBytes = new byte[recv];
                    System.Array.Copy(data, receivedBytes, recv);
                    PlayerData playerData = PlayerData.Deserialize(receivedBytes);
                    onlineGameManager.EnqueuePlayerData(playerData);
                }
            }
            catch (SocketException ex)
            {
                Debug.LogError($"Socket error while receiving data: {ex.Message}");
                break;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error while processing received data: {ex.Message}");
            }
        }
    }

    void ExecuteCommand(string command, string parameter)
    {
        switch (command)
        {
            case "TriggerFunction":
                Debug.Log($"Executing TriggerFunction with parameter: {parameter}");
                TriggerFunction(parameter);
                break;

            default:
                Debug.LogWarning($"Unknown command received: {command}");
                break;
        }
    }

    void TriggerFunction(string parameter)
    {
        Debug.Log($"TriggerFunction executed with parameter: {parameter}");
        onlineGameManager.ClearTileList();
        playRequest = true;
    }

    private void OnDestroy()
    {
        receiveThread?.Abort();
        socket?.Close();
    }

    IEnumerator ShowErrorMessage()
    {
        invalidIPMessage.SetActive(true);
        yield return new WaitForSeconds(2);
        invalidIPMessage.SetActive(false);
    }
}