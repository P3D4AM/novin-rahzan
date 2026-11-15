using System;
using System.Collections.Generic;
using NP756P.ReadOnlyProbe;

namespace NP756P.ReadOnlyProbe.Tests
{
    /// <summary>
    /// In-memory fake of the SDK channel. Simulates vendor return codes, values
    /// and exceptions so the probe orchestration can be verified without any
    /// native DLL and without any device.
    ///
    /// It enforces the real contract of the probe: FK_GetProductData accepts only
    /// dataIndex 1 and 3, FK_GetDeviceInfo accepts only infoIndex 2 and 3 - any
    /// other index returns the vendor "Parameter Error" status (-5). It also
    /// records the exact connection options it received so tests can assert that
    /// the values actually forwarded to the SDK match the parsed command line.
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

        /// <summary>When set, Connect throws this exception instead of returning.</summary>
        public Exception ConnectException;

        /// <summary>Status code returned by the simulated read calls (1 = success).</summary>
        public int ProductStatus = 1;
        public int DeviceInfoStatus = 1;
        public int DeviceTimeStatus = 1;

        /// <summary>When set, thrown by the first GetProductData call.</summary>
        public Exception ProductException;

        /// <summary>When set, thrown by the first GetDeviceInfo call.</summary>
        public Exception DeviceInfoException;

        /// <summary>When set, thrown by the first GetDeviceTime call.</summary>
        public Exception DeviceTimeException;

        /// <summary>When set, thrown by Disconnect (FK_DisConnect itself never returns a code).</summary>
        public Exception DisconnectException;

        public string SerialValue = "SN-000123";
        public string ProductCodeValue = "NP756P";
        public int MachineValue = 7;
        public int LanguageValue = 0;
        public DateTime TimeValue = new DateTime(2026, 11, 12, 9, 30, 0);

        public ProbeOptions LastConnectOptions;

        /// <summary>Indices actually handed to GetProductData (must stay {1,3}).</summary>
        public readonly List<int> ReceivedProductIndices = new List<int>();

        /// <summary>Indices actually handed to GetDeviceInfo (must stay {2,3}).</summary>
        public readonly List<int> ReceivedDeviceInfoIndices = new List<int>();

        // exact values the channel saw in Connect(ProbeOptions)
        public string ReceivedIp;
        public int ReceivedPort;
        public int ReceivedMachine;
        public int ReceivedTimeout;
        public int ReceivedProtocol;
        public int ReceivedPassword;
        public int ReceivedLicense;

        public int Connect(ProbeOptions options)
        {
            ConnectCalls++;
            LastConnectOptions = options;
            if (options != null)
            {
                ReceivedIp = options.Ip;
                ReceivedPort = options.Port;
                ReceivedMachine = options.MachineNumber;
                ReceivedTimeout = options.TimeoutMs;
                ReceivedProtocol = options.ProtocolValue;
                ReceivedPassword = options.NetworkPassword;
                ReceivedLicense = options.License;
            }
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
            ReceivedProductIndices.Add(dataIndex);
            if (ProductException != null)
            {
                value = null;
                Exception ex = ProductException;
                ProductException = null;
                throw ex;
            }
            if (dataIndex != 1 && dataIndex != 3)
            {
                // the probe is only allowed to ask for serial (1) / product code (3)
                value = null;
                LastErrorCalls++;
                LastError = -5;
                return -5;   // Parameter Error
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
            ReceivedDeviceInfoIndices.Add(infoIndex);
            if (DeviceInfoException != null)
            {
                value = 0;
                Exception ex = DeviceInfoException;
                DeviceInfoException = null;
                throw ex;
            }
            if (infoIndex != 2 && infoIndex != 3)
            {
                // the probe is only allowed to ask for machine number (2) / language (3)
                value = 0;
                LastErrorCalls++;
                LastError = -5;
                return -5;   // Parameter Error
            }
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
            if (DeviceTimeException != null)
            {
                deviceTime = DateTime.MinValue;
                Exception ex = DeviceTimeException;
                DeviceTimeException = null;
                throw ex;
            }
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
            if (DisconnectException != null)
            {
                Exception ex = DisconnectException;
                DisconnectException = null;
                throw ex;
            }
            Handle = 0;
        }
    }
}
