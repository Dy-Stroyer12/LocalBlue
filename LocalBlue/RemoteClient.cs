using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using InTheHand.Net.Sockets;

namespace LocalBlue
{
    public class RemoteClient
    {
        public string ClientName { get; set; } = "Unknown";
        public string DeviceAddress { get; set; }
        public BluetoothClient Bridge { get; set; } = new BluetoothClient();
        
        public void Handle()
        {
            try
            {
                var stream = Bridge.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Received message from remote client: {message}");

                // Send an acknowledgment back to the client
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
                Bridge.Close();
            }
        }

        public RemoteClient(string deviceAddress)
        {
            DeviceAddress = deviceAddress;
        }
        public RemoteClient(string deviceAddress, BluetoothClient bridge) : base()
        {
            DeviceAddress = deviceAddress;
            Bridge = bridge;
        }
        
        public RemoteClient(BluetoothClient bridge) : base()
        {
            Bridge = bridge;
        }
    }
}
