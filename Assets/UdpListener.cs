using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using Newtonsoft.Json;

public class UdpState
{
    public IPEndPoint e;
    public UdpClient u;
}

public class UdpListener : MonoBehaviour
{
    const int port = 45998;
    static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
    static UdpClient _udpClient = new UdpClient(ipEndPoint);
    public NetworkedPlayer player1;
    public NetworkedPlayer player2;
	public static UdpListener Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BeginReceive();
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

		var netPosition = JsonConvert.DeserializeObject<NetworkPosition>(receiveString);

		if (netPosition.Id == 0)
		{
			UdpListener.Instance.player1.UpdateNetworkPosition(new Vector3(netPosition.positionX, netPosition.positionY, netPosition.positionZ));
		}
		else
		{
            UdpListener.Instance.player2.UpdateNetworkPosition(new Vector3(netPosition.positionX, netPosition.positionY, netPosition.positionZ));
		}

        Debug.Log("Received: " + receiveString);
		BeginReceive();
    }

	public static void BeginReceive()
	{
        var udpState = new UdpState();
        udpState.e = ipEndPoint;
        udpState.u = _udpClient;

        Debug.Log("Begin Receive...");
        _udpClient.BeginReceive(ReceiveCallback, udpState);
	}
}
