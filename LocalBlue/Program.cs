using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Threading.Tasks;
using System.Linq;
using LocalBlue;

Server.Start();

Console.WriteLine("Starting Bluetooth device discovery...");


while (Server.isRunning == true)
    {

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

                if (services != null && services.Contains(Server.appServiceGuid))
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

    };



    //}

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
}