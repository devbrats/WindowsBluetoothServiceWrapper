using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace WindowsBluetoothService
{
    public static class BluetoothService
    {
        public static async Task<BluetoothLEDevice> FindDevice(ulong bluetoothAddress)
        {
            var findBluetoothDeviceOperation = BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothAddress);
            var ocs = new OperationCompletionSource<BluetoothLEDevice>(findBluetoothDeviceOperation);

            var device = await ocs.Result;

            return device;
        }

        public static async Task<GattDeviceServicesResult> GetGattServices(this BluetoothLEDevice device, BluetoothCacheMode cacheMode = BluetoothCacheMode.Uncached)
        {
            var fetchGATTServicesOperation = device.GetGattServicesAsync(cacheMode);
            var ocs = new OperationCompletionSource<GattDeviceServicesResult>(fetchGATTServicesOperation);

            var gattService = await ocs.Result;
            return gattService;
        }

        public static async Task<GattDeviceServicesResult> GetGattServiceWithId(this BluetoothLEDevice device, Guid serviceId, BluetoothCacheMode cacheMode = BluetoothCacheMode.Uncached)
        {
            var fetchGATTServiceWithIdOperation = device.GetGattServicesForUuidAsync(serviceId, cacheMode);
            var ocs = new OperationCompletionSource<GattDeviceServicesResult>(fetchGATTServiceWithIdOperation);

            var gattService = await ocs.Result;
            return gattService;
        }

        public static async Task<GattCharacteristicsResult> GetCharacteristics(this GattDeviceService gattService, BluetoothCacheMode cacheMode = BluetoothCacheMode.Uncached)
        {
            var getCharacteristicsOperation = gattService.GetCharacteristicsAsync(cacheMode);
            var ocs = new OperationCompletionSource<GattCharacteristicsResult>(getCharacteristicsOperation);

            var characteristicResult = await ocs.Result;
            return characteristicResult;

        }

        public static async Task<GattCharacteristicsResult> GetCharacteristic(this GattDeviceService gattService, Guid characteristicId, BluetoothCacheMode cacheMode = BluetoothCacheMode.Uncached)
        {
            var getCharactertisticsWithIdOperation = gattService.GetCharacteristicsForUuidAsync(characteristicId, cacheMode);
            var ocs = new OperationCompletionSource<GattCharacteristicsResult>(getCharactertisticsWithIdOperation);

            var characteristicResult = await ocs.Result;
            return characteristicResult;

        }

        public static async Task<GattReadResult> ReadCharacteristic(this GattCharacteristic characteristic, BluetoothCacheMode cacheMode = BluetoothCacheMode.Uncached)
        {
            var readCharacteristicOperation = characteristic.ReadValueAsync(cacheMode);
            var ocs = new OperationCompletionSource<GattReadResult>(readCharacteristicOperation);

            var readResult = await ocs.Result;
            return readResult;
        }

        public static async Task<GattCommunicationStatus> WriteCharacteristic(this GattCharacteristic characteristic, string data)
        {
            var buffer = data.WriteDataToBuffer();
            var writeToCharacteristicOperation = characteristic.WriteValueAsync(buffer);
            var ocs = new OperationCompletionSource<GattCommunicationStatus>(writeToCharacteristicOperation);

            var writeResult = await ocs.Result;
            return writeResult;
        }

        public static async Task<GattCommunicationStatus> WriteCharacteristic(this GattCharacteristic characteristic, IBuffer writeBuffer)
        {
            var writetoCharacteristicOperation = characteristic.WriteValueAsync(writeBuffer);
            var ocs = new OperationCompletionSource<GattCommunicationStatus>(writetoCharacteristicOperation);

            var writeResult = await ocs.Result;
            return writeResult;
        }

        public static async Task<GattCommunicationStatus> WriteCharacteristicConfiguration(this GattCharacteristic characteristic, GattClientCharacteristicConfigurationDescriptorValue configValue)
        {
            var writeConfigurationOperation = characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(configValue);
            var ocs = new OperationCompletionSource<GattCommunicationStatus>(writeConfigurationOperation);

            var configResult = await ocs.Result;
            return configResult;
        }

        public static byte[] ReadFromBuffer(this IBuffer buffer)
        {
            byte[] data;

            using (var reader = DataReader.FromBuffer(buffer))
            {
                data = new byte[reader.UnconsumedBufferLength];
                reader.ReadBytes(data);
            }
            return data;
        }

        public static IBuffer WriteDataToBuffer(this string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            var writer = new DataWriter();
            writer.WriteBytes(byteData);
            var buffer = writer.DetachBuffer();
            return buffer;
        }
    }
}