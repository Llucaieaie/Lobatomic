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

        // Set lobby values =======================================
        //lobbyManager.Player2.GetComponent<PlayerMovementOnline>().data.Name = clientName;

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
            byte[] receivedBytes = new byte[recv];
            System.Array.Copy(data, receivedBytes, recv);

            try
            {
                PlayerData playerData = PlayerData.Deserialize(receivedBytes);

                Debug.Log($"Received PlayerData: Id={playerData.Id}, Name={playerData.Name}, Position={playerData.Position}");

                // Enqueue PlayerData instead of just position
                lobbyManager.EnqueuePlayerData(playerData);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to deserialize PlayerData: {ex.Message}");
            }
        }
    }

    public void SendPlayerData(PlayerData playerData)
    {
        byte[] data = PlayerData.Serialize(playerData);

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        socket.SendTo(data, data.Length, SocketFlags.None, ipep);

        clientText += $"\nPlayerData sent: {playerData.Name} at {playerData.Position}";
    }

    private void OnDestroy()
    {
        receiveThread?.Abort();
        socket?.Close();
    }
}