using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Threading.Tasks;
using System.Linq;
using LocalBlue;

var appServiceGuid = new Guid("f6845322-085c-4847-907d-375132a872ab");
var 

BluetoothListener listener = new BluetoothListener(appServiceGuid);
listener.ServiceName = "localblue";
listener.Start();
Console.WriteLine("Bluetooth listener started, waiting for incoming connections...");

Console.WriteLine("Starting Bluetooth device discovery...");

Task.Run(async () =>
{
    while (true)
    {
        try
        {
            var remoteBridge = listener.AcceptBluetoothClient();
            RemoteClient remoteClient = new1 RemoteClient(remoteBridge);
            Console.WriteLine("Incoming connection accepted.");

            Task.Run(() => remoteClient.Handle());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting incoming connection: {ex.Message}");
        }
    }
});

BluetoothClient bluetoothClient = new BluetoothClient();
var devices = bluetoothClient.DiscoverDevices();

if (devices.Count == 0)
{
    Console.WriteLine("No nearby Bluetooth devices found.");
}
else
{
    Console.WriteLine("Discovered Bluetooth devices:");

    foreach (var device in devices)
    {
        Console.WriteLine($"Found {device.DeviceName}. Checking services... ({device.DeviceAddress})");

        try
        {
            var services = device.InstalledServices?.ToArray();

            if (services != null && services.Contains(appServiceGuid))
            {
                Console.WriteLine($"- Device: '{device.DeviceName}' ({device.DeviceAddress}) - Running localblue");
            }
            else
            {
                Console.WriteLine($"- Device: {device.DeviceName} does not have the localblue service.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"- Unable to retrieve services for device: {device.DeviceName}. Error: {ex.Message}");
        }
    }
}

void HandleIncomingConnection(BluetoothClient client)
{
    try
    {
        var stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);

        Console.WriteLine($"Received message from remote client: {message}");

        string response = "ACCEPTED";
        byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
        stream.Write(responseBytes, 0, responseBytes.Length);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error handling incoming connection: {ex.Message}");
    }
    finally
    {
        client.Close();
    }
}