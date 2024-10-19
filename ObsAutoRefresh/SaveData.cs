using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ObsAutoRefresh;

public class SaveData
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Password { get; set; }
    public required List<string> VideoCaptureDevicesToRefresh { get; set; }

    private static string SavePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obsautorefresh.json");
    
    public static SaveData? Load()
    {
        if (!File.Exists(SavePath))
            return null;

        string text = File.ReadAllText(SavePath);
        
        return JsonSerializer.Deserialize<SaveData>(text);
    }

    public void Save()
    {
        string serialized = JsonSerializer.Serialize(this);
        File.WriteAllText(SavePath, serialized);
    }
}