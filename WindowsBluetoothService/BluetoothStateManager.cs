using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Radios;

namespace WindowsBluetoothService
{
    public class BluetoothStateManager
    {
        public Radio Bluetooth { get; private set; }

        public bool IsRadioAccessAllowed { get; private set; }

        public async Task<bool> EnsureBluetoothIsOnAsync()
        {
            var accessStatus = await RequestRadioAccess();

            IsRadioAccessAllowed = accessStatus == RadioAccessStatus.Allowed ? true : false;

            if (IsRadioAccessAllowed)
            {
                if (Bluetooth == null)
                {
                    var radios = await GetRadios();
                    Bluetooth= radios.FirstOrDefault(x=>x.Kind == RadioKind.Bluetooth);  
                }

                if(Bluetooth != null)
                {
                    if (Bluetooth.State == RadioState.Off)
                    {
                        await SetRadioState(Bluetooth);
                    }
                }
            }

            return Bluetooth?.State == RadioState.On;
        }

        private async Task<RadioAccessStatus> RequestRadioAccess()
        {
            var radioRequestOperation = Radio.RequestAccessAsync();
            var ocs = new OperationCompletionSource<RadioAccessStatus>(radioRequestOperation);
            var result = await ocs.Result;
            return result;
        }

        private async Task<IReadOnlyList<Radio>> GetRadios()
        {
            var getRadiosOperation = Radio.GetRadiosAsync();
            var ocs = new OperationCompletionSource<IReadOnlyList<Radio>>(getRadiosOperation);
            var radios = await ocs.Result;
            return radios;
        }

        private async Task<RadioAccessStatus> SetRadioState(Radio radio, RadioState radioState = RadioState.On)
        {
            var setRadioStateOperation = radio.SetStateAsync(radioState);
            var ocs = new OperationCompletionSource<RadioAccessStatus>(setRadioStateOperation);
            var radioStateSetResult = await ocs.Result;
            return radioStateSetResult;
        }

    }
}