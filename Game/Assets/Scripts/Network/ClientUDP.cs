using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    public GameObject UItextObj;
    public LobbyManager lobbyManager;
    public GameObject createLobbyWindow;
    public string clientName = "";
    public string serverIP = "";

    private Socket socket;
    private Thread receiveThread;
    private TextMeshProUGUI UItext;
    private string clientText;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UItext.text = clientText;
    }

    public void StartClient()
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        byte[] data = Encoding.UTF8.GetBytes(clientName);
        socket.SendTo(data, data.Length, SocketFlags.None, ipep);

        clientText += $"\nName sent to server: {clientName}";

        receiveThread = new Thread(ReceiveData);
        receiveThread.Start();

        lobbyManager.gameObject.SetActive(true);
        lobbyManager.isHost = false;
        createLobbyWindow.SetActive(false);
    }

    void ReceiveData()
    {
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;

        while (true)
        {
            int recv = socket.ReceiveFrom(data, ref Remote);
            string message = Encoding.UTF8.GetString(data, 0, recv);

            // Procesar posición
            if (message.StartsWith("POS:"))
            {
                string[] parts = message.Substring(4).Split(',');
                float x = float.Parse(parts[0]);
                float y = float.Parse(parts[1]);
                Vector3 position = new Vector3(x, y, 0);

                lobbyManager.UpdatePlayerPosition(0, position); // Actualiza Player1
            }
        }
    }

    public void SendPosition(Vector3 position)
    {
        string message = $"POS:{position.x},{position.y}";
        byte[] data = Encoding.UTF8.GetBytes(message);

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket.SendTo(data, data.Length, SocketFlags.None, ipep);

        clientText += $"\nPosition sent: {position}";
    }

    private void OnDestroy()
    {
        receiveThread?.Abort();
        socket?.Close();
    }
}