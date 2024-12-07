﻿using System.Net;
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
    private EndPoint client;
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

        // Set lobby values =======================================
        lobbyManager.Player1.GetComponent<PlayerDataManager>().SetName(hostName);
        lobbyManager.SetPlayerActive(1, false);
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
            byte[] receivedBytes = new byte[recv];
            System.Array.Copy(data, receivedBytes, recv);

            // Check if this Remote is already the client
            client ??= Remote;

            try
            {
                PlayerData playerData = PlayerData.Deserialize(receivedBytes);

                // Enqueue PlayerData instead of just position
                lobbyManager.EnqueuePlayerData(playerData);

                //Debug.Log($"Received PlayerData: Id={playerData.Id}, Name={playerData.Name}, Position={playerData.Position}");
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

        lobbyManager.SetPlayerActive(1, true);

        byte[] data = PlayerData.Serialize(playerData);

        socket.SendTo(data, data.Length, SocketFlags.None, client);
    }

    private void OnDestroy()
    {
        receiveThread?.Abort();
        socket?.Close();
    }
}