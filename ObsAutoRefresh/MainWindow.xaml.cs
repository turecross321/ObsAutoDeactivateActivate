using System.Collections.ObjectModel;
using System.Windows;
using OBSWebsocketDotNet.Types;
using UsbMonitor;

namespace ObsAutoRefresh;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ObsAutoRefreshClient? _client;
    private SaveData? _save;
    private UsbMonitorManager? _usbMonitorManager;
    private readonly ObservableCollection<string> _videoCaptureDevicesSource = new ObservableCollection<string>();
    private readonly ObservableCollection<string> _videoCaptureDevicesToRefreshSource = new ObservableCollection<string>();
    
    public MainWindow()
    {
        this.InitializeComponent();
        this.LoadSave();
        
        this.VideoCaptureDevices.ItemsSource = this._videoCaptureDevicesSource;
        this.VideoCaptureDevicesToRefresh.ItemsSource = this._videoCaptureDevicesToRefreshSource;
    }
    
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        this._client?.Disconnect();
        
        Log("Window is closing.");
        
        base.OnClosing(e);
    }

    private void LoadSave()
    {
        this.Log("Loading save file");
        this._save = SaveData.Load() ?? new SaveData
        {
            Host = "ws://localhost",
            Port = 4455,
            Password = "",
            VideoCaptureDevicesToRefresh = []
        };

        this.HostInput.Text = this._save.Host;
        this.PortInput.Text = this._save.Port.ToString();
        this.PasswordInput.Password = this._save.Password;
    }

    private void Save()
    {
        this.Log("Saving savefile");
        this._save?.Save();
    }
    
    private void Log(string message)
    {
        Console.WriteLine(message);
        
        Dispatcher.Invoke(() =>
        {
            this.Output.AppendText(message + Environment.NewLine);
            this.Output.ScrollToEnd();
        });
    }

    private void OnConnectClick(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(this.PortInput.Text, out int port))
        {
            Log("Failed to parse port. Please enter a valid port.\n");
            return;
        }

        this._save!.Host = this.HostInput.Text;
        this._save!.Port = int.Parse(this.PortInput.Text);
        this._save!.Password = this.PasswordInput.Password;
        this.Save();

        this._client = new ObsAutoRefreshClient();
        this._client.ConnectEvent += OnClientConnectEvent;
        this._client.DisconnectEvent += OnClientDisconnectEvent;
        this._client.Log += Log;

        this._client.Connect(this.HostInput.Text, port, this.PasswordInput.Password);
    }

    private void OnClientDisconnectEvent()
    {
        this.Dispatcher.Invoke(() =>
        {
            this.ConnectedPanel.Visibility = Visibility.Collapsed;
            this.NotConnectedPanel.Visibility = Visibility.Visible;
            if (this._usbMonitorManager != null)
            {
                this._usbMonitorManager.UsbDeviceInterface -= MOnUsbDeviceInterface;
                this._usbMonitorManager.Stop();
                this._usbMonitorManager = null;
            }
        });
    }

    private void OnClientConnectEvent()
    {
        this.Dispatcher.Invoke(() =>
        {
            this.ConnectedPanel.Visibility = Visibility.Visible;
            this.NotConnectedPanel.Visibility = Visibility.Collapsed;
            FetchVideoCaptures();
            SetVideoCaptureDevicesToRefreshFromSave();
            
            this._usbMonitorManager = new UsbMonitorManager(this);
            this._usbMonitorManager.UsbDeviceInterface += MOnUsbDeviceInterface;
        });
    }
    
    private void OnLoadVideoCapturesClick(object sender, RoutedEventArgs e)
    {
        this.FetchVideoCaptures();
    }

    private void SetVideoCaptureDevicesToRefreshFromSave()
    {
        this._videoCaptureDevicesToRefreshSource.Clear();
        
        foreach (string device in this._save!.VideoCaptureDevicesToRefresh)
        {
            this._videoCaptureDevicesToRefreshSource.Add(device);
        }
    }
    
    private void FetchVideoCaptures()
    {
        if (this._client == null)
            return;
        
        List<InputBasicInfo> inputs = this._client.FetchVideoCaptures();

        this._videoCaptureDevicesSource.Clear();
        foreach (InputBasicInfo input in inputs)
        {
            this._videoCaptureDevicesSource.Add(input.InputName);
        }
    }
    
    private void OnDisconnectClick(object sender, RoutedEventArgs e)
    {
        this._client?.Disconnect();
    }

    private void OnAddSelectedSourcesClick(object sender, RoutedEventArgs e)
    {
        if (this._save == null)
        {
            Log("Tried to apply selected sources without a save?");
            return;
        }
        
        
        foreach (object obj in this.VideoCaptureDevices.SelectedItems)
        {
            string name = (string)obj;
            this._videoCaptureDevicesToRefreshSource.Add(name);
        }

        this._save.VideoCaptureDevicesToRefresh = this._videoCaptureDevicesToRefreshSource.ToList();
        Save();
    }

    private void OnRemoveSelectedSourcesClick(object sender, RoutedEventArgs e)
    {
        if (this._save == null)
        {
            Log("Tried to apply selected sources without a save?");
            return;
        }
    
        // Create a temporary list to hold items to remove
        var itemsToRemove = new List<string>();

        foreach (object obj in this.VideoCaptureDevicesToRefresh.SelectedItems)
        {
            string name = (string)obj;
            itemsToRemove.Add(name);
        }

        // Now iterate over the temporary list to modify the original collection
        foreach (string name in itemsToRemove)
        {
            this._videoCaptureDevicesToRefreshSource.Remove(name);
            this._save.VideoCaptureDevicesToRefresh.Remove(name);
        }
        
        Save();
    }
    

    private void OnRefreshDevicesClick(object sender, RoutedEventArgs e)
    {
        this.RefreshDevices();
    }

    private void RefreshDevices()
    {
        this._client?.RefreshDevices(this._videoCaptureDevicesToRefreshSource.ToArray());
    }
    
    private void MOnUsbDeviceInterface(object? sender, UsbEventArgs e)
    {
        if (e.Action == UsbDeviceChangeEvent.Arrival)
            this.RefreshDevices();
    }
}