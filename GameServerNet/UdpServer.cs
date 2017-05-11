using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameServerNet
{
    public class UdpServer
    {
        UdpClient _udpClient;

        public void Start()
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 20547);
            _udpClient = new UdpClient(ipEndPoint);

            Task.Run(async () =>
            {
                Console.WriteLine(nameof(Start));
                while (true)
                {
                    Console.WriteLine("Starting Receive...");
                    var udpReceiveResult = await _udpClient.ReceiveAsync();

                    OnMessageReceived(udpReceiveResult);
                }
            });
        }

        void OnMessageReceived(UdpReceiveResult result)
        {

            var resultString = Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine("Received: " + resultString);

            var udpMessage = JsonConvert.DeserializeObject<UdpMessage>(resultString);

            switch (udpMessage.Event)
            {
                case "natpunch":
                    SendNatPunch(result);
                    break;
                default:
                    break;
            }
        }

        void SendNatPunch(UdpReceiveResult result)
        {
            var natPunchPayload = new UdpMessage
            {
                Event = "natpunchresponse",
                Data = new NatPunchResponse
                {
                    PublicIpAddress = result.RemoteEndPoint.Address.ToString(),
                    PublicPort = result.RemoteEndPoint.Port
                }
            };

            var payloadJson = JsonConvert.SerializeObject(natPunchPayload);

            var payloadBytes = Encoding.UTF8.GetBytes(payloadJson);

            _udpClient.SendAsync(payloadBytes, payloadBytes.Length, result.RemoteEndPoint);
        }
    }
}