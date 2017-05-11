using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

public class UdpState
{
    public IPEndPoint e;
    public UdpClient u;
}

public class UdpListener : MonoBehaviour
{
    const int port = 45998;
    //static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
    static UdpClient _udpClient = new UdpClient();
    public NetworkedPlayer player1;
    public NetworkedPlayer player2;
	public static UdpListener Instance;
    public Dictionary<Guid, NetworkedPlayer> NetworkedPlayers = new Dictionary<Guid, NetworkedPlayer>();
    IPEndPoint _gameServerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20547);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BeginReceive();
        DoIpDiscovery();
    }

    void Update()
    {

    }
	
	public static void ReceiveCallback(IAsyncResult ar)
    {
        Debug.Log("got message!");
        var u = (UdpClient)((UdpState)(ar.AsyncState)).u;
        var e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

        var receiveBytes = u.EndReceive(ar, ref e);
        var receiveString = Encoding.ASCII.GetString(receiveBytes);
        Debug.Log("Received: " + receiveString);

		var netPosition = JsonConvert.DeserializeObject<NetworkPosition>(receiveString);

        //NetworkedPlayers[]

		if (netPosition.Id.ToString() == "0")
		{
			UdpListener.Instance.player1.UpdateNetworkPosition(new Vector3(netPosition.positionX, netPosition.positionY, netPosition.positionZ));
		}
		else
		{
            UdpListener.Instance.player2.UpdateNetworkPosition(new Vector3(netPosition.positionX, netPosition.positionY, netPosition.positionZ));
		}

		BeginReceive();
    }

	public static void BeginReceive()
	{
        var udpState = new UdpState();
        udpState.e = new IPEndPoint(IPAddress.Any, 0);
        udpState.u = _udpClient;

        Debug.Log("Begin Receive...");
        _udpClient.BeginReceive(ReceiveCallback, udpState);
	}

    void DoIpDiscovery()
    {
        var payload = new UdpMessage
        {
            Event = "natpunch"
        };

        Send(JsonConvert.SerializeObject(payload));
    }

    void Send(string message)
    {
        var payloadBytes = Encoding.UTF8.GetBytes(message);
        _udpClient.Send(payloadBytes, payloadBytes.Length, _gameServerEndPoint);
    }
}
