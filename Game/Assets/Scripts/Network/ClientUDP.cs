﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Collections;
using System;

public class ClientUDP : MonoBehaviour
{
    public GameObject UItextObj;
    public LobbyManager lobbyManager;
    public GameObject createLobbyWindow;
    public GameObject invalidIPMessage;

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

            // Set lobby values =======================================
            lobbyManager.Player2.GetComponent<PlayerDataManager>().SetName(clientName);
            lobbyManager.SetPlayerActive(0, false);
            lobbyManager.gameObject.SetActive(true);
            lobbyManager.isHost = false;
            createLobbyWindow.SetActive(false);
        }
        catch (Exception e)
        {
            StartCoroutine(ShowErrorMessage());
        }
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
                byte[] receivedBytes = new byte[recv];
                System.Array.Copy(data, receivedBytes, recv);

                PlayerData playerData = PlayerData.Deserialize(receivedBytes);

                lobbyManager.EnqueuePlayerData(playerData);

                //Debug.Log($"Received PlayerData: Id={playerData.Id}, Name={playerData.Name}, Position={playerData.Position}");
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

    public void SendPlayerData(PlayerData playerData)
    {
        lobbyManager.SetPlayerActive(0, true);

        byte[] data = PlayerData.Serialize(playerData);

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket.SendTo(data, data.Length, SocketFlags.None, ipep);

        //clientText += $"\nPlayerData sent: {playerData.Name} at {playerData.Position}";
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