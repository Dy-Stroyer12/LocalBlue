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
        public string ClientName { get; set; }
        public string DeviceAddress { get; set; }
        public BluetoothClient Bridge { get; set; } = new BluetoothClient();

        public RemoteClient()
        {

        }
    }
}
