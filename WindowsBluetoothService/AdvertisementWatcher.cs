using System;
using Windows.Devices.Bluetooth.Advertisement;

namespace WindowsBluetoothService
{
    public class AdvertisementWatcher
    {
        private BluetoothLEAdvertisementWatcher watcher;
        private Guid _serviceId;

        /// <summary>
        /// Advertisement match with service id.
        /// </summary>
        public Advertisement Advertisement { get; private set; }

        /// <summary>
        /// Flag to check if watcher is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Creates a BluetoothLEAdvertisementWatcher based on filtering of serviceId.
        /// </summary>
        /// <param name="serviceId">Guid of the service which should be filtered by watcher.</param>
        public AdvertisementWatcher(Guid serviceId, Action<BluetoothLEAdvertisementWatcher, BluetoothLEAdvertisementReceivedEventArgs> callBackForWatcher = null)
        {
            _serviceId = serviceId;
            BluetoothLEAdvertisementFilter filter = new BluetoothLEAdvertisementFilter();
            filter.Advertisement.ServiceUuids.Add(serviceId);
            watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active,
                AdvertisementFilter = filter
            };

            watcher.Received += Watcher_Received;
            watcher.Received += (sender, args) =>
            {
                callBackForWatcher?.Invoke(sender, args);
            };
            watcher.AllowExtendedAdvertisements = true;
            IsRunning = false;
        }

        /// <summary>
        /// Start the watcher.
        /// </summary>
        public void Start()
        {
            IsRunning = true;
            watcher.Start();
        }

        public void WaitForCompletion()
        {
            while(IsRunning)
            {

            }
        }

        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            foreach (var id in args.Advertisement.ServiceUuids)
            {
                if (id == _serviceId)
                {
                    Advertisement = new Advertisement(args.BluetoothAddress, args.Advertisement.LocalName, args.RawSignalStrengthInDBm);
                    Stop();
                }
            }
        }

        /// <summary>
        /// Stop the watcher.
        /// </summary>
        private void Stop()
        {
            IsRunning = false;
            watcher.Stop();
        }

    }
}