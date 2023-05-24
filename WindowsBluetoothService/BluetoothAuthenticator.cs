using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace WindowsBluetoothService
{
    public static class BluetoothAuthenticator
    {
        public async static Task<DevicePairingResult> PairingWithConfirmPinMatch(this BluetoothLEDevice device, TypedEventHandler<DeviceInformationCustomPairing,DevicePairingRequestedEventArgs> onPairingRequestedHandler)
        {
            var pairing = device.DeviceInformation.Pairing;
            pairing.Custom.PairingRequested += onPairingRequestedHandler;
            var pairingOperation = pairing.Custom.PairAsync(DevicePairingKinds.ConfirmPinMatch);

            var ocs = new OperationCompletionSource<DevicePairingResult>(pairingOperation);
            var pairingResult = await ocs.Result;

            return pairingResult;
        }

    }
}