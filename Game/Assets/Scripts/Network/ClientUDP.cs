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
            //Debug.LogWarning("Introduzca un nombre válido para conectar al servidor.");
            //return; // Si está vacío, termina la función
        }
        
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    /// <summary>
    /// Send/Recieve
    /// </summary>
    void Send()
    {
        // Establecer socket y endpoint
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Enviar nombre
        byte[] data = Encoding.UTF8.GetBytes(clientName);
        socket.SendTo(data, 0, data.Length, SocketFlags.None, ipep);

        clientText += $"\nNombre enviado al servidor: {clientName}";

        Thread receive = new Thread(Receive);
        receive.Start();
    }

    void Receive()
    {
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;

        int recv = socket.ReceiveFrom(data, ref Remote);
        string message = Encoding.ASCII.GetString(data, 0, recv);
        clientText += $"\nMensaje recibido del servidor: {message}";
    }
}