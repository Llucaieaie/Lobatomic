using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ServerUDP : MonoBehaviour
{
    public GameObject UItextObj;
    public OnlineGameManager onlineGameManager;
    public GameObject createLobbyWindow;
    public string hostName = "";
    public int seed = 0;

    private Socket socket;
    private Thread receiveThread;
    private EndPoint client;
    private TextMeshProUGUI UItext;
    private string serverText;

    public GameObject PlayerDisconnectedPopUpPrefab;
    bool playerDisconnected;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(gameObject);
        playerDisconnected = false;
    }

    void Update()
    {
        UItext.text = serverText;

        if (playerDisconnected)
        {
            Debug.Log("Player disconnected");
            playerDisconnected = false;

            SceneManager.LoadScene("LobbyScene");
            Canvas canvas = FindObjectOfType<Canvas>();
            Instantiate(PlayerDisconnectedPopUpPrefab, canvas.transform);
        }
    }

    public void StartServer()
    {
        serverText = "Starting UDP Server...";
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        receiveThread = new Thread(ReceiveData);
        receiveThread.Start();

        // Set ogm values =======================================
        onlineGameManager.Player1.GetComponent<PlayerDataManager>().SetName(hostName);
        onlineGameManager.SetCurrentTilesLists(onlineGameManager.currentTiles);
        onlineGameManager.SetPlayerActive(1, false);
        onlineGameManager.gameObject.SetActive(true);
        onlineGameManager.isHost = true;
        createLobbyWindow.SetActive(false);
    }

    void ReceiveData()
    {
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;

        while (true)
        {
            int recv = 0;
            try
            {
                recv = socket.ReceiveFrom(data, ref Remote);
            }
            catch 
            {
                playerDisconnected = true;
                return;
            }
            byte[] receivedBytes = new byte[recv];
            System.Array.Copy(data, receivedBytes, recv);

            client ??= Remote;

            try
            {
                PlayerData playerData = PlayerData.Deserialize(receivedBytes);

                onlineGameManager.EnqueuePlayerData(playerData);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to deserialize PlayerData: {ex.Message}");
            }
        }
    }

    public void SendPlayerData(PlayerData playerData)
    {
        if (client == null) return;

        onlineGameManager.SetPlayerActive(1, true);

        byte[] data = PlayerData.Serialize(playerData);

        socket.SendTo(data, data.Length, SocketFlags.None, client);
    }

    public void SendCommandToClient(string command, int seed)
    {
        if (client == null) return;

        // Crea un mensaje con el comando y el parámetro
        string message = $"{command}|{seed}";
        byte[] data = Encoding.UTF8.GetBytes(message);

        // Envía el mensaje al cliente
        socket.SendTo(data, data.Length, SocketFlags.None, client);
    }

    private void OnDestroy()
    {
        receiveThread?.Abort();
        socket?.Close();
    }
}