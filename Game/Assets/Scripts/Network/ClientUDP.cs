using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    // Public fields
    public GameObject UItextObj;
    public string clientName = "";
    public string serverIP = "";  // Ojo con enseñar la ip
    public LobbyManager lobbyManager;

    // Private fields
    Socket socket;
    TextMeshProUGUI UItext;
    string clientText;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UItext.text = clientText;
    }

    /// <summary>
    /// Start Client
    /// </summary>
    public void StartClient()
    {
        if (string.IsNullOrEmpty(clientName))
        {
            clientName = "Dr.Mini Mini";
        }
        
        if (!string.IsNullOrEmpty(serverIP))
        {
            Thread mainThread = new Thread(SendConnectionRequest);
            mainThread.Start();

            lobbyManager.AddPlayer();
        }
    }

    void SendConnectionRequest()
    { 
        // Establecer socket y endpoint
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Enviar nombre
        byte[] data = Encoding.UTF8.GetBytes(clientName);
        socket.SendTo(data, 0, data.Length, SocketFlags.None, ipep);

        clientText += $"\nName sent to server: {clientName}";

        Thread receive = new Thread(ReceiveServerConfirmation);
        receive.Start();
    }

    void ReceiveServerConfirmation()
    {
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;

        int recv = socket.ReceiveFrom(data, ref Remote);
        string message = Encoding.ASCII.GetString(data, 0, recv);
        clientText += $"\nConfirmation recieved from server: {message}";
    }

    /// <summary>
    /// Send/Recieve
    /// </summary>
    void Send(string message)
    {
        // Establecer socket y endpoint
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Enviar mensaje
        byte[] data = Encoding.UTF8.GetBytes(message);
        socket.SendTo(data, 0, data.Length, SocketFlags.None, ipep);

        clientText += $"\nMessage sent to server: {message}";

        Thread receive = new Thread(Recieve);
        receive.Start();
    }

    void Recieve()
    {
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;

        int recv = socket.ReceiveFrom(data, ref Remote);
        string message = Encoding.ASCII.GetString(data, 0, recv);
        clientText += $"\nRecieved message from server: {message}";
    }
}