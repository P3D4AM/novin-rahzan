using System;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// Abstraction over the native SDK so the probe orchestration can be unit tested
    /// with a mock channel (no native DLL is ever loaded inside the test suite).
    /// </summary>
    public interface IDeviceChannel
    {
        /// <summary>True after a successful Connect and before Disconnect.</summary>
        bool IsConnected { get; }

        /// <summary>Raw SDK handle (0 or negative when not connected).</summary>
        int Handle { get; }

        /// <summary>
        /// Value of FK_GetLastError captured after the most recent failed read.
        /// Zero when it was not consulted. It is read only with a valid handle.
        /// </summary>
        int LastError { get; }

        /// <summary>Calls FK_ConnectNet once with the given options; returns the raw handle.</summary>
        int Connect(ProbeOptions options);

        /// <summary>Calls FK_GetProductData; returns the raw SDK status code (1 = success).</summary>
        int GetProductData(int dataIndex, out string value);

        /// <summary>Calls FK_GetDeviceInfo; returns the raw SDK status code (1 = success).</summary>
        int GetDeviceInfo(int infoIndex, out int value);

        /// <summary>Calls FK_GetDeviceTime; returns the raw SDK status code (1 = success).</summary>
        int GetDeviceTime(out DateTime deviceTime);

        /// <summary>Calls FK_DisConnect (void); safe to call only with a valid handle.</summary>
        void Disconnect();
    }
}
