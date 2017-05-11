using Newtonsoft.Json;

public class UdpMessage
{
    [JsonProperty("event")]
    public string Event;

    [JsonProperty("data")]
    public object Data;
}