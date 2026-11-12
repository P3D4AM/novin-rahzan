using System;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// Real implementation over NativeMethods. Every method call happens ONLY here;
    /// this is the single place where a native vendor DLL gets loaded at runtime
    /// (on the operator's Windows machine, by explicit manual run).
    ///
    /// FK_GetLastError is consulted only when the handle is valid and the vendor
    /// read call returned an error code, exactly as allowed by the mission brief.
    /// </summary>
    public sealed class FkDeviceChannel : IDeviceChannel
    {
        private int _handle;

        public bool IsConnected
        {
            get { return _handle > 0; }
        }

        public int Handle
        {
            get { return _handle; }
        }

        public int LastError { get; private set; }

        public int Connect(ProbeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _handle = 0;
            LastError = 0;
            // Password is passed to the SDK only; it is never logged or rendered.
            _handle = NativeMethods.FK_ConnectNet(
                options.MachineNumber,
                options.Ip,
                options.Port,
                options.TimeoutMs,
                options.ProtocolValue,
                options.NetworkPassword,
                options.License);
            return _handle;
        }

        public int GetProductData(int dataIndex, out string value)
        {
            // Vendor pattern (FrmCheckDeviceHysoon.FuncGetProductData):
            // pre-fill a 256-space string, pass by ref, trim afterwards.
            string buffer = new string(' ', 256);
            int code = NativeMethods.FK_GetProductData(_handle, dataIndex, ref buffer);
            if (code == 1)
            {
                value = buffer == null ? "" : buffer.Trim();
            }
            else
            {
                value = null;
                CaptureLastError();
            }
            return code;
        }

        public int GetDeviceInfo(int infoIndex, out int value)
        {
            int v = 0;
            int code = NativeMethods.FK_GetDeviceInfo(_handle, infoIndex, ref v);
            value = v;
            if (code != 1)
            {
                CaptureLastError();
            }
            return code;
        }

        public int GetDeviceTime(out DateTime deviceTime)
        {
            // Vendor pattern (FrmCheckDeviceHysoon.GetDeviceTime):
            // initialize with DateTime.Now, pass by ref, use only when code == 1.
            DateTime dt = DateTime.Now;
            int code = NativeMethods.FK_GetDeviceTime(_handle, ref dt);
            deviceTime = dt;
            if (code != 1)
            {
                CaptureLastError();
            }
            return code;
        }

        public void Disconnect()
        {
            if (_handle > 0)
            {
                NativeMethods.FK_DisConnect(_handle);
            }
            _handle = 0;
        }

        private void CaptureLastError()
        {
            LastError = 0;
            if (_handle > 0)
            {
                // Only valid with a live handle (mission constraint).
                LastError = NativeMethods.FK_GetLastError(_handle);
            }
        }
    }
}
