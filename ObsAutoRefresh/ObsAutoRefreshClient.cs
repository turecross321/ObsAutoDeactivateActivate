using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types;

namespace ObsAutoRefresh;

public class ObsAutoRefreshClient
{
    private readonly OBSWebsocket _obs;

    public event Action<string>? Log;
    public event Action? ConnectEvent;
    public event Action? DisconnectEvent;

    public ObsAutoRefreshClient()
    {
        this._obs = new OBSWebsocket();
        this._obs.Connected += ObsOnConnected;
        this._obs.Disconnected += ObsOnDisconnected;
    }

    public void Connect(string ip, int port, string password)
    {
        string url = $"{ip}:{port}";
        LogMessage($"Connecting to {ip}");
        this._obs.ConnectAsync(url, password);
    }

    public void RefreshDevices(string[] devices)
    {
        foreach (string device in devices)
        {
            LogMessage($"Refreshing {device}");
            this._obs.SetInputSettings(device, new JObject());
        }
    }

    public void Disconnect()
    {
        this._obs.Disconnect();
    }

    public List<InputBasicInfo> FetchVideoCaptures()
    {
        LogMessage("Fetching video captures");
        return this._obs.GetInputList("dshow_input");
    }
    
    private void LogMessage(string message)
    {
        this.Log?.Invoke($"{message}");
    }
    
    private void ObsOnDisconnected(object? sender, ObsDisconnectionInfo e)
    {
        LogMessage("Disconnected from OBS");
        this.DisconnectEvent?.Invoke();
    }

    private void ObsOnConnected(object? sender, EventArgs e)
    {
        LogMessage("Connected to OBS");
        this.ConnectEvent.Invoke();
    }
}