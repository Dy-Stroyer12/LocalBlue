using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalBlue
{

    public static class Server
    {
        public static Guid appServiceGuid = new Guid("f6845322-085c-4847-907d-375132a872ab");
        static BluetoothListener listener = new BluetoothListener(appServiceGuid);
        public static bool isRunning = false;

        public static void Start()
        {
            listener.ServiceName = "localblue";
            listener.Start();
            Console.WriteLine("Bluetooth listener started, waiting for incoming connections...");
            Server.isRunning = true;
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (listener.Pending() == true)
                        {
                            Console.WriteLine("Found pending connection...");
                            var remoteBridge = listener.AcceptBluetoothClient();
                            RemoteClient remoteClient = new RemoteClient(remoteBridge);
                            Console.WriteLine("Incoming connection accepted.");

                            Task.Run(() => remoteClient.Handle());
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error accepting incoming connection: {ex.Message}");
                    }
                }
            });
        }

        public static void Stop()
        {
            isRunning = false;
        }

    }
}
