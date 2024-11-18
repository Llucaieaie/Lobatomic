using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Collections.Generic;

public class ServerUDP : MonoBehaviour
{
    // Public fields
    public GameObject UItextObj;
    public string hostName = "";
    public LobbyManager lobbyManager;
    public GameObject createLobbyWindow;

    // Private fields
    Socket socket;
    TextMeshProUGUI UItext;
    string serverText;

    private List<EndPoint> clients = new List<EndPoint>();

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        UItext.text = serverText;
    }

    /// <summary>
    /// Start Server
    /// </summary>
    public void StartServer()
    {
        serverText = "Starting UDP Server...";

        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        Thread newConnection = new Thread(ReceiveClient);
        newConnection.Start();

        if (string.IsNullOrEmpty(hostName))
        {
            hostName = "Dr. MiniMini";
        }

        lobbyManager.ActivateMap();
    }

    void SendClientConfirmation(EndPoint Remote)
    {
        string response = "Name recieved correctly";
        byte[] data = Encoding.UTF8.GetBytes(response);

        // Enviar la respuesta al cliente
        socket.SendTo(data, 0, data.Length, SocketFlags.None, Remote);
        serverText += "\nConfirmation sent to client";

        lobbyManager.AddPlayer(hostName);
        createLobbyWindow.SetActive(false);
    }

    /// <summary>
    /// Send/Recieve
    /// </summary>
    void Recieve()
    {
        int recv;
        byte[] data = new byte[1024];

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        while (true)
        {
            recv = socket.ReceiveFrom(data, ref Remote);
            string playerName = Encoding.UTF8.GetString(data, 0, recv);

            serverText += $"\nRecieved message from client {Remote}: {playerName}";

            Thread sendThread = new Thread(() => Send(Remote));
            sendThread.Start();
        }
    }

    void Send(EndPoint Remote)
    {
        string response = "Nombre recibido correctamente.";
        byte[] data = Encoding.UTF8.GetBytes(response);

        // Enviar la respuesta al cliente
        socket.SendTo(data, 0, data.Length, SocketFlags.None, Remote);
        serverText += "\nAnswer sent to client";
    }

    void ReceiveClient()
    {
        int recv;
        byte[] data = new byte[1024];

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        while (true)
        {
            recv = socket.ReceiveFrom(data, ref Remote);
            string message = Encoding.UTF8.GetString(data, 0, recv);

            if (message.StartsWith("POS:"))
            {
                serverText += $"\nPosition received from {Remote}: {message}";
                BroadcastPosition(message, Remote);
            }
            else
            {
                serverText += $"\nMessage received from client {Remote}: {message}";
            }
        }
    }

    void BroadcastPosition(string message, EndPoint sender)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);

        foreach (EndPoint client in clients)
        {
            if (!client.Equals(sender))
            {
                socket.SendTo(data, data.Length, SocketFlags.None, client);
                serverText += $"\nPosition broadcasted to {client}";
            }
        }
    }
}

