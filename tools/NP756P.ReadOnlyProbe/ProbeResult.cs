using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// Vendor status-code meanings, translated from
    /// TimeAttendanceDevices.FLRHysoon.FKAttendDLL2.ReturnResultPrint.
    /// The decompiled switch has no text for code -13; it is reported as unmapped
    /// rather than invented.
    /// </summary>
    public static class FkErrorText
    {
        public static string For(int code)
        {
            switch (code)
            {
                case 1: return "Successful";
                case 0: return "No support";
                case -1: return "Unknown error";
                case -2: return "No Open Comm";
                case -3: return "Write Error";
                case -4: return "Read Error";
                case -5: return "Parameter Error";
                case -6: return "Execution of command failed";
                case -7: return "End of data";
                case -8: return "Nonexistence data";
                case -9: return "Memory Allocating Error";
                case -10: return "License Error";
                case -11: return "Full enrolldata, cannot put enrolldata";
                case -12: return "This id is already existed";
                case -14: return "Full manager, cannot put manager";
                case -15: return "Mistake fp data version";
                default: return "Unmapped code (no text in vendor source)";
            }
        }
    }

    /// <summary>Outcome of a single probe stage.</summary>
    public enum StageStatus
    {
        Pending,
        Ok,
        Failed,
        Skipped
    }

    /// <summary>
    /// Everything the probe learned, rendered as the staged report. The network
    /// password is never stored here (guaranteed by construction and covered by a
    /// dedicated unit test).
    /// </summary>
    public sealed class ProbeResult
    {
        public sealed class FailureInfo
        {
            public string Stage;
            public string Function;
            public int RawCode;
            public bool HasRawCode;
            public int LastError;
            public bool HasLastError;
            public string ExceptionType;
            public string Message;
            public string NextStep;
        }

        // [1]/[2]
        public NativeLibraryValidator.ValidationReport Native = new NativeLibraryValidator.ValidationReport();

        // [3]
        public bool ConnectionAttempted;
        public int ConnectHandle;
        public int ConnectRawCode;
        public bool ConnectSucceeded;
        public long ConnectDurationMs;
        public string ConnectExceptionType;

        // [4]
        public string SerialNumber;
        public string ProductCode;
        public StageStatus ProductStage = StageStatus.Pending;
        public FailureInfo ProductFailure;

        // [5]
        public int DeviceMachineNumber;
        public bool HasDeviceMachineNumber;
        public int DeviceLanguage;
        public bool HasDeviceLanguage;
        public StageStatus DeviceInfoStage = StageStatus.Pending;
        public FailureInfo DeviceInfoFailure;

        // [6]
        public DateTime DeviceTime;
        public bool HasDeviceTime;
        public StageStatus DeviceTimeStage = StageStatus.Pending;
        public FailureInfo DeviceTimeFailure;

        // [7]
        public bool DisconnectCalled;
        public string DisconnectNote;

        public FailureInfo FirstFailure;
        public int ExitCode;

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine("NP756P READ-ONLY DIAGNOSTIC");
            sb.AppendLine("========================================");

            // [1] native libraries
            sb.AppendLine();
            sb.AppendLine("[1] Native Libraries");
            foreach (NativeLibraryValidator.DllEntry e in Native.Entries)
            {
                sb.AppendLine("    " + Pad(e.FileName, 24) + ": " + e.Status);
                if (e.MissingDependencies.Count > 0)
                {
                    sb.AppendLine("        missing dependencies : " + string.Join(", ", e.MissingDependencies.ToArray()));
                }
            }
            if (!Native.DirectoryExists)
            {
                sb.AppendLine("    DLL directory          : MISSING (" + (Native.Directory ?? "") + ")");
            }

            // [2] architecture
            sb.AppendLine();
            sb.AppendLine("[2] Architecture");
            string processArch = IntPtr.Size == 8 ? "x64" : "x86";
            sb.AppendLine("    " + Pad("Process", 24) + ": " + processArch);
            int nativeX86 = 0;
            int nativeOther = 0;
            foreach (NativeLibraryValidator.DllEntry e in Native.Entries)
            {
                if (e.Architecture == "x86") { nativeX86++; }
                else if (e.Architecture != "-") { nativeOther++; }
            }
            string sdkArch = nativeX86 > 0 && nativeOther == 0 ? "x86 (" + nativeX86 + " DLLs)"
                : (nativeOther > 0 ? "MIXED/UNKNOWN (" + nativeOther + " non-x86)" : "unknown");
            sb.AppendLine("    " + Pad("Native SDK", 24) + ": " + sdkArch);
            if (processArch != "x86")
            {
                sb.AppendLine("    " + Pad("WARNING", 24) + ": process is not x86; the native SDK cannot load here.");
            }

            // [3] connection
            sb.AppendLine();
            sb.AppendLine("[3] Connection");
            if (!ConnectionAttempted)
            {
                sb.AppendLine("    " + Pad("Result", 24) + ": SKIPPED (previous stage failed)");
            }
            else
            {
                // ProbeOptions fields only; the password never appears here.
                sb.AppendLine("    " + Pad("IP", 24) + ": " + ConnectionIp);
                sb.AppendLine("    " + Pad("Port", 24) + ": " + ConnectionPort.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("    " + Pad("Protocol", 24) + ": " + ConnectionProtocol);
                sb.AppendLine("    " + Pad("Machine Number", 24) + ": " + ConnectionMachine.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("    " + Pad("Timeout", 24) + ": " + ConnectionTimeout.ToString(CultureInfo.InvariantCulture) + " ms");
                sb.AppendLine("    " + Pad("License", 24) + ": " + ConnectionLicense.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("    " + Pad("Connect Result", 24) + ": " +
                    (ConnectSucceeded ? "SUCCESS (handle=" + ConnectHandle.ToString(CultureInfo.InvariantCulture) + ")"
                                      : "FAILED (handle=" + ConnectHandle.ToString(CultureInfo.InvariantCulture) + ")"));
                sb.AppendLine("    " + Pad("Elapsed", 24) + ": " + ConnectDurationMs.ToString(CultureInfo.InvariantCulture) + " ms");
                if (!ConnectSucceeded)
                {
                    sb.AppendLine("    " + Pad("Meaning", 24) + ": " + FkErrorText.For(ConnectRawCode));
                }
            }

            // [4] product
            sb.AppendLine();
            sb.AppendLine("[4] Product Information");
            sb.AppendLine("    " + Pad("Serial Number", 24) + ": " + StageValue(ProductStage, SerialNumber));
            sb.AppendLine("    " + Pad("Product Code", 24) + ": " + StageValue(ProductStage, ProductCode));

            // [5] device info
            sb.AppendLine();
            sb.AppendLine("[5] Device Information");
            sb.AppendLine("    " + Pad("Machine Number", 24) + ": " +
                (HasDeviceMachineNumber ? DeviceMachineNumber.ToString(CultureInfo.InvariantCulture) : StageWord(DeviceInfoStage)));
            sb.AppendLine("    " + Pad("Language", 24) + ": " +
                (HasDeviceLanguage ? DeviceLanguage.ToString(CultureInfo.InvariantCulture) + LanguageName(DeviceLanguage) : StageWord(DeviceInfoStage)));

            // [6] device time
            sb.AppendLine();
            sb.AppendLine("[6] Device Time");
            sb.AppendLine("    " + Pad("Device Time", 24) + ": " +
                (HasDeviceTime ? DeviceTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : StageWord(DeviceTimeStage)));

            // [7] disconnect
            sb.AppendLine();
            sb.AppendLine("[7] Disconnect");
            sb.AppendLine("    " + Pad("Result", 24) + ": " +
                (DisconnectCalled ? "OK (FK_DisConnect executed)" : "NOT NEEDED (no live handle)"));
            if (!string.IsNullOrEmpty(DisconnectNote))
            {
                sb.AppendLine("    " + Pad("Note", 24) + ": " + DisconnectNote);
            }

            // failure block
            FailureInfo f = FirstFailure;
            if (f != null)
            {
                sb.AppendLine();
                sb.AppendLine("-------- FAILURE DETAILS --------");
                sb.AppendLine("    Stage        : " + (f.Stage ?? "-"));
                if (!string.IsNullOrEmpty(f.Function)) { sb.AppendLine("    Function     : " + f.Function); }
                if (f.HasRawCode) { sb.AppendLine("    Raw Code     : " + f.RawCode.ToString(CultureInfo.InvariantCulture) + " (" + FkErrorText.For(f.RawCode) + ")"); }
                if (f.HasLastError) { sb.AppendLine("    Last Error   : " + f.LastError.ToString(CultureInfo.InvariantCulture) + " (" + FkErrorText.For(f.LastError) + ")"); }
                if (!string.IsNullOrEmpty(f.ExceptionType)) { sb.AppendLine("    Exception    : " + f.ExceptionType); }
                if (!string.IsNullOrEmpty(f.Message)) { sb.AppendLine("    Message      : " + f.Message); }
                if (!string.IsNullOrEmpty(f.NextStep)) { sb.AppendLine("    Next Step    : " + f.NextStep); }
            }
            sb.AppendLine();
            sb.AppendLine("========================================");
            return sb.ToString();
        }

        // connection mirror fields (copies of non-secret ProbeOptions values)
        public string ConnectionIp;
        public int ConnectionPort;
        public string ConnectionProtocol;
        public int ConnectionMachine;
        public int ConnectionTimeout;
        public int ConnectionLicense;

        private static string StageValue(StageStatus st, string value)
        {
            if (st == StageStatus.Ok && value != null) { return value; }
            return StageWord(st);
        }

        private static string StageWord(StageStatus st)
        {
            switch (st)
            {
                case StageStatus.Ok: return "(no value)";
                case StageStatus.Failed: return "FAILED";
                case StageStatus.Skipped: return "SKIPPED";
                default: return "PENDING";
            }
        }

        private static string LanguageName(int v)
        {
            // Vendor sample FrmCheckDeviceHysoon.GetDeviceLanguage: 0 = Farsi.
            if (v == 0) { return " (Farsi)"; }
            return "";
        }

        private static string Pad(string s, int width)
        {
            if (s == null) { s = ""; }
            if (s.Length >= width) { return s; }
            return s + new string(' ', width - s.Length);
        }
    }
}
