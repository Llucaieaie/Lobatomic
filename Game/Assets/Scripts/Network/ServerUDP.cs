using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Collections.Generic;

public class ServerUDP : MonoBehaviour
{
    public GameObject UItextObj;
    public LobbyManager lobbyManager;
    public GameObject createLobbyWindow;
    public string hostName = "";

    private Socket socket;
    private Thread receiveThread;
    private List<EndPoint> clients = new List<EndPoint>();
    private TextMeshProUGUI UItext;
    private string serverText;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UItext.text = serverText;
    }

    public void StartServer()
    {
        serverText = "Starting UDP Server...";
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        receiveThread = new Thread(ReceiveData);
        receiveThread.Start();

        lobbyManager.gameObject.SetActive(true);
        lobbyManager.isHost = true;
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

            // Procesar posición o mensaje
            if (message.StartsWith("POS:"))
            {
                string[] parts = message.Substring(4).Split(',');
                float x = float.Parse(parts[0]);
                float y = float.Parse(parts[1]);
                Vector3 position = new Vector3(x, y, 0);

                lobbyManager.UpdatePlayerPosition(1, position); // Actualiza Player2
            }
            else if (!clients.Contains(Remote))
            {
                clients.Add(Remote);
                serverText += $"\nNew client connected: {Remote}";
            }
        }
    }

    public void SendPosition(Vector3 position)
    {
        if (clients.Count == 0) return;

        string message = $"POS:{position.x},{position.y}";
        byte[] data = Encoding.UTF8.GetBytes(message);

        foreach (EndPoint client in clients)
        {
            socket.SendTo(data, data.Length, SocketFlags.None, client);
        }

        serverText += $"\nPosition sent: {position}";
    }

    private void OnDestroy()
    {
        receiveThread?.Abort();
        socket?.Close();
    }
}
