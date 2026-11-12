using System;
using System.Collections.Generic;
using System.IO;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// Network protocols accepted by FK_ConnectNet (verified from vendor sources:
    /// TimeAttendanceDevices.FLRHysoon.clsHysoon => _vnProtocolType = 0 for TCP,
    /// and FKAttendDllCSSample FrmConnectDeviceHysoon: UDP checkbox => 1).
    /// </summary>
    public enum ProbeProtocol
    {
        Tcp = 0,
        Udp = 1
    }

    /// <summary>
    /// Connection settings for the probe. The network password is kept here but is
    /// NEVER rendered, logged or included in any report (see ToString()).
    /// Defaults follow the mission brief (192.168.1.75:4238, machine 1, timeout 5000,
    /// TCP, license 1261) and every value can be overridden on the command line.
    /// </summary>
    public sealed class ProbeOptions
    {
        public const string DefaultIp = "192.168.1.75";
        public const int DefaultPort = 4238;
        public const int DefaultMachineNumber = 1;
        public const int DefaultTimeoutMs = 5000;
        public const int DefaultLicense = 1261;

        public string Ip = DefaultIp;
        public int Port = DefaultPort;
        public int MachineNumber = DefaultMachineNumber;
        public int TimeoutMs = DefaultTimeoutMs;
        public ProbeProtocol Protocol = ProbeProtocol.Tcp;
        public int License = DefaultLicense;

        /// <summary>Numeric SDK network password. Never displayed or logged.</summary>
        public int NetworkPassword;

        /// <summary>Folder that must contain the vendor native DLLs. Null = exe folder.</summary>
        public string DllDir;

        public int ProtocolValue
        {
            get { return (int)Protocol; }
        }

        public string ProtocolName
        {
            get { return Protocol == ProbeProtocol.Tcp ? "TCP" : "UDP"; }
        }

        /// <summary>Sanitized description (password intentionally omitted).</summary>
        public override string ToString()
        {
            return "IP=" + Ip + " Port=" + Port + " Machine=" + MachineNumber +
                   " Timeout=" + TimeoutMs + "ms Protocol=" + ProtocolName +
                   " License=" + License + " DllDir=" + (DllDir ?? "<exe folder>") +
                   " Password=<hidden>";
        }

        public static bool TryParse(string[] args, out ProbeOptions options, out List<string> errors)
        {
            options = new ProbeOptions();
            errors = new List<string>();
            if (args == null)
            {
                return true;
            }

            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                string name;
                string value;
                int eq = arg.IndexOf('=');
                if (eq >= 0)
                {
                    name = arg.Substring(0, eq);
                    value = arg.Substring(eq + 1);
                }
                else
                {
                    name = arg;
                    if (i + 1 >= args.Length)
                    {
                        errors.Add("Missing value for " + arg);
                        break;
                    }
                    value = args[i + 1];
                    i++;
                }
                i++;

                switch (name)
                {
                    case "--ip":
                        if (IsValidIp(value)) { options.Ip = value; }
                        else { errors.Add("Invalid IPv4 address: " + Safe(value)); }
                        break;
                    case "--port":
                        int port;
                        if (!TryParseInt(value, out port)) { errors.Add("Port is not a number: " + Safe(value)); }
                        else if (!IsValidPort(port)) { errors.Add("Port out of range (1-65535): " + port); }
                        else { options.Port = port; }
                        break;
                    case "--machine":
                        int machine;
                        if (!TryParseInt(value, out machine)) { errors.Add("Machine number is not a number: " + Safe(value)); }
                        else if (!IsValidMachineNumber(machine)) { errors.Add("Machine number out of range (1-999999): " + machine); }
                        else { options.MachineNumber = machine; }
                        break;
                    case "--timeout":
                        int timeout;
                        if (!TryParseInt(value, out timeout)) { errors.Add("Timeout is not a number: " + Safe(value)); }
                        else if (!IsValidTimeout(timeout)) { errors.Add("Timeout out of range (100-600000 ms): " + timeout); }
                        else { options.TimeoutMs = timeout; }
                        break;
                    case "--protocol":
                        ProbeProtocol protocol;
                        if (!TryParseProtocol(value, out protocol)) { errors.Add("Protocol must be 'tcp' or 'udp': " + Safe(value)); }
                        else { options.Protocol = protocol; }
                        break;
                    case "--license":
                        int license;
                        if (!TryParseInt(value, out license)) { errors.Add("License is not a number: " + Safe(value)); }
                        else if (!IsValidLicense(license)) { errors.Add("License out of range (1-999999999): " + license); }
                        else { options.License = license; }
                        break;
                    case "--dll-dir":
                        if (string.IsNullOrWhiteSpace(value)) { errors.Add("--dll-dir is empty"); }
                        else if (!Directory.Exists(value)) { errors.Add("DLL directory does not exist: " + Safe(value)); }
                        else { options.DllDir = Path.GetFullPath(value); }
                        break;
                    case "--help":
                    case "-h":
                    case "/?":
                        errors.Add("__HELP__");
                        break;
                    default:
                        errors.Add("Unknown argument: " + Safe(name));
                        break;
                }
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Passwords must be numeric integers (vendor: FK_ConnectNet takes an int
        /// network password; clsHysoon stores it as int). Empty input is rejected so a
        /// missing password is never silently treated as 0.
        /// </summary>
        public static bool TryParsePassword(string raw, out int password, out string error)
        {
            password = 0;
            error = null;
            if (string.IsNullOrEmpty(raw))
            {
                error = "Password is empty. Enter the numeric network password of the device.";
                return false;
            }
            string trimmed = raw.Trim();
            if (trimmed.Length == 0)
            {
                error = "Password is empty. Enter the numeric network password of the device.";
                return false;
            }
            long v;
            if (!long.TryParse(trimmed, out v))
            {
                error = "Password must be a number (digits only).";
                return false;
            }
            if (v < 0 || v > int.MaxValue)
            {
                error = "Password is out of the allowed numeric range.";
                return false;
            }
            password = (int)v;
            return true;
        }

        public static bool TryParseProtocol(string raw, out ProbeProtocol protocol)
        {
            protocol = ProbeProtocol.Tcp;
            if (raw == null) { return false; }
            string t = raw.Trim().ToLowerInvariant();
            if (t == "tcp" || t == "0") { protocol = ProbeProtocol.Tcp; return true; }
            if (t == "udp" || t == "1") { protocol = ProbeProtocol.Udp; return true; }
            return false;
        }

        /// <summary>Strict dotted-quad IPv4 (hostnames are intentionally not accepted).</summary>
        public static bool IsValidIp(string ip)
        {
            if (string.IsNullOrEmpty(ip)) { return false; }
            if (ip != ip.Trim()) { return false; }
            string[] parts = ip.Split('.');
            if (parts.Length != 4) { return false; }
            foreach (string p in parts)
            {
                if (p.Length == 0 || p.Length > 3) { return false; }
                foreach (char c in p)
                {
                    if (c < '0' || c > '9') { return false; }
                }
                if (p.Length > 1 && p[0] == '0') { return false; } // no leading zeros
                int v;
                if (!int.TryParse(p, out v)) { return false; }
                if (v > 255) { return false; }
            }
            return true;
        }

        public static bool IsValidPort(int port)
        {
            return port >= 1 && port <= 65535;
        }

        public static bool IsValidMachineNumber(int machineNumber)
        {
            return machineNumber >= 1 && machineNumber <= 999999;
        }

        public static bool IsValidTimeout(int timeoutMs)
        {
            return timeoutMs >= 100 && timeoutMs <= 600000;
        }

        public static bool IsValidLicense(int license)
        {
            return license >= 1 && license <= 999999999;
        }

        private static bool TryParseInt(string raw, out int value)
        {
            value = 0;
            if (raw == null) { return false; }
            return int.TryParse(raw.Trim(), out value);
        }

        /// <summary>Keeps error messages short; never used for the password.</summary>
        private static string Safe(string s)
        {
            if (s == null) { return ""; }
            return s.Length <= 40 ? s : s.Substring(0, 40) + "...";
        }
    }
}
