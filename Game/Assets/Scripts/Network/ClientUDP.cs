﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    Socket socket;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    [SerializeField] public string playerName = "";
    string clientText;

    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();

    }
    public void StartClient()
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Introduzca un nombre válido para conectar al servidor.");
            return; // Si está vacío, termina la función
        }
        
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        UItext.text = clientText;
    }

    void Send()
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.1.103"), 9050); // Ojo con enseñar la ip
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //string handshake = "Hello World";

        // Enviar nombre
        byte[] data = Encoding.UTF8.GetBytes(playerName);

        socket.SendTo(data, 0, data.Length, SocketFlags.None, ipep);
        clientText += $"\nNombre enviado al servidor: {playerName}";

        Thread receive = new Thread(Receive);
        receive.Start();
    }

    void Receive()
    {
        IPEndPoint sender;
        EndPoint Remote = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = new byte[1024];
        int recv = socket.ReceiveFrom(data, ref Remote);

        clientText = ("Message received from {0}: " + Remote.ToString());
        clientText = clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);
    }

    public string SetPlayerName(string pname)
    {
        playerName = pname;
        return playerName;
    }
}