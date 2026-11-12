using System;
using NP756P.ReadOnlyProbe;

namespace NP756P.ReadOnlyProbe.Tests
{
    /// <summary>
    /// In-memory fake of the SDK channel. Simulates vendor return codes, values
    /// and exceptions so the probe orchestration can be verified without any
    /// native DLL and without any device.
    /// </summary>
    public sealed class MockDeviceChannel : IDeviceChannel
    {
        public bool IsConnected
        {
            get { return Handle > 0; }
        }

        public int Handle { get; private set; }
        public int LastError { get; private set; }

        public int ConnectCalls;
        public int DisconnectCalls;
        public int ProductCalls;
        public int DeviceInfoCalls;
        public int DeviceTimeCalls;
        public int LastErrorCalls;

        /// <summary>Raw handle returned by the simulated FK_ConnectNet.</summary>
        public int ConnectResult = 5;

        /// <summary>When true Connect throws this exception instead of returning.</summary>
        public Exception ConnectException;

        /// <summary>Status code returned by the simulated read calls (1 = success).</summary>
        public int ProductStatus = 1;
        public int DeviceInfoStatus = 1;
        public int DeviceTimeStatus = 1;

        /// <summary>When set, thrown by the first GetProductData call.</summary>
        public Exception ProductException;

        public string SerialValue = "SN-000123";
        public string ProductCodeValue = "NP756P";
        public int MachineValue = 7;
        public int LanguageValue = 0;
        public DateTime TimeValue = new DateTime(2026, 11, 12, 9, 30, 0);

        public ProbeOptions LastConnectOptions;

        public int Connect(ProbeOptions options)
        {
            ConnectCalls++;
            LastConnectOptions = options;
            if (ConnectException != null)
            {
                throw ConnectException;
            }
            Handle = ConnectResult;
            return Handle;
        }

        public int GetProductData(int dataIndex, out string value)
        {
            ProductCalls++;
            if (ProductException != null)
            {
                value = null;
                Exception ex = ProductException;
                ProductException = null;
                throw ex;
            }
            if (ProductStatus != 1)
            {
                value = null;
                LastErrorCalls++;
                LastError = -4;
                return ProductStatus;
            }
            value = dataIndex == 1 ? SerialValue : ProductCodeValue;
            return 1;
        }

        public int GetDeviceInfo(int infoIndex, out int value)
        {
            DeviceInfoCalls++;
            if (DeviceInfoStatus != 1)
            {
                value = 0;
                LastErrorCalls++;
                LastError = -4;
                return DeviceInfoStatus;
            }
            value = infoIndex == 2 ? MachineValue : LanguageValue;
            return 1;
        }

        public int GetDeviceTime(out DateTime deviceTime)
        {
            DeviceTimeCalls++;
            if (DeviceTimeStatus != 1)
            {
                deviceTime = DateTime.MinValue;
                LastErrorCalls++;
                LastError = -4;
                return DeviceTimeStatus;
            }
            deviceTime = TimeValue;
            return 1;
        }

        public void Disconnect()
        {
            DisconnectCalls++;
            Handle = 0;
        }
    }
}
