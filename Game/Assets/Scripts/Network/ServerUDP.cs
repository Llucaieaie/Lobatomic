using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ServerUDP : MonoBehaviour
{
    // Public fields
    public GameObject UItextObj;
    public string hostName = "";

    // Private fields
    Socket socket;
    TextMeshProUGUI UItext;
    string serverText;

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

        Thread newConnection = new Thread(Receive);
        newConnection.Start();
    }

    /// <summary>
    /// Send/Recieve
    /// </summary>
    void Receive()
    {
        int recv;
        byte[] data = new byte[1024];

        serverText = serverText + "\n" + "Waiting for new Client...";

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        while (true)
        {
            recv = socket.ReceiveFrom(data, ref Remote);
            string playerName = Encoding.UTF8.GetString(data, 0, recv);

            //serverText = serverText + "\n" + "Message received from {0}:" + Remote.ToString();
            //serverText = serverText + "\n" + Encoding.ASCII.GetString(data, 0, recv);
            serverText += $"\nNombre recibido del cliente {Remote}: {playerName}";

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
        serverText += "\nRespuesta enviada al cliente.";
    }
}