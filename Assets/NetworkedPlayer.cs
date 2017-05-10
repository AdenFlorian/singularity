﻿using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System;
using Newtonsoft.Json;

public class NetworkedPlayer : MonoBehaviour
{
	public int Id;
	public bool playerControlled;

	UdpClient _udpClient = new UdpClient();
	Transform _transform;

	Vector3 latestNetworkPosition;

	float timer;
	float timeToSend = .2f;

	void Start()
	{
		_transform = GetComponent<Transform>();
	}

	void Update()
	{
		timer += Time.deltaTime;
		
        if (playerControlled == false)
        {
            _transform.position = latestNetworkPosition;
        }
		else
		{
			if (timer >= timeToSend)
			{
				timer -= timeToSend;
                try
                {
                    var netPosition = new NetworkPosition(Id, _transform.position);
                    var netPositionJson = JsonConvert.SerializeObject(netPosition);
                    var sendBytes = Encoding.ASCII.GetBytes(netPositionJson);

                    _udpClient.Send(sendBytes, sendBytes.Length, "127.0.0.1", 20547);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
			}
		}
	}

	public void Move(Vector3 newPosition)
	{
		latestNetworkPosition = newPosition;
	}
}