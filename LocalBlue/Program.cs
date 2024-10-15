using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Threading.Tasks;
using System.Linq;

var appServiceGuid = new Guid("f6845322-085c-4847-907d-375132a872ab");

BluetoothListener listener = new BluetoothListener(appServiceGuid);
listener.ServiceName = "localblue";
listener.Start();
Console.WriteLine("Bluetooth listener started, waiting for incoming connections...");

Console.WriteLine("Starting Bluetooth device discovery...");

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