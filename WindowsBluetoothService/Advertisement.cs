namespace WindowsBluetoothService
{
    public class Advertisement
    {
        public Advertisement(ulong bluetoothAddress, string name, short signalStrength)
        {
            BluetoothAddress = bluetoothAddress;
            Name = name;
            SignalStrength = signalStrength;
        }

        public ulong BluetoothAddress { get; set; }

        public string Name { get; set; }

        public short SignalStrength { get; set; }
    }
}