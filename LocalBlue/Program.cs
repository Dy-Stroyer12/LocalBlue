using InTheHand.Net.Sockets;
using System;

Console.WriteLine("Searching for Bluetooth devices...");

BluetoothClient bluetoothClient = new BluetoothClient();
var devices = bluetoothClient.DiscoverDevices();

if (devices.Count == 0)
{
    Console.WriteLine("No devices found.");
}

foreach (var device in devices)
{
    Console.WriteLine($"Found device: {device.DeviceName} ({device.DeviceAddress})");
}
