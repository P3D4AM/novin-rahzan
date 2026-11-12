using System;
using System.Diagnostics;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// Orchestrates the probe stages in the fixed order A..F:
    ///   A) native library validation (static, no loading)
    ///   B) FK_ConnectNet (once, recorded)
    ///   C) FK_GetProductData 1 (serial) and 3 (product code)
    ///   D) FK_GetDeviceInfo 2 (machine number) and 3 (language)
    ///   E) FK_GetDeviceTime (read-only)
    ///   F) FK_DisConnect in a finally block whenever the handle is valid
    ///
    /// On any stage failure the dependent later stages are skipped and the failure
    /// is reported with function name, raw code, exception type and next step.
    /// The network password is used for Connect only and never recorded.
    /// </summary>
    public static class ProbeRunner
    {
        public static ProbeResult Run(ProbeOptions options, IDeviceChannel channel, NativeLibraryValidator.ValidationReport nativeReport)
        {
            if (options == null) { throw new ArgumentNullException("options"); }
            if (channel == null) { throw new ArgumentNullException("channel"); }

            ProbeResult result = new ProbeResult();
            result.Native = nativeReport ?? new NativeLibraryValidator.ValidationReport();

            // mirror non-secret connection fields for the report
            result.ConnectionIp = options.Ip;
            result.ConnectionPort = options.Port;
            result.ConnectionProtocol = options.ProtocolName;
            result.ConnectionMachine = options.MachineNumber;
            result.ConnectionTimeout = options.TimeoutMs;
            result.ConnectionLicense = options.License;

            // ---------- Stage A ----------
            if (!result.Native.Succeeded)
            {
                result.ConnectionAttempted = false;
                result.ProductStage = StageStatus.Skipped;
                result.DeviceInfoStage = StageStatus.Skipped;
                result.DeviceTimeStage = StageStatus.Skipped;
                result.DisconnectNote = "Not reached";
                result.FirstFailure = new ProbeResult.FailureInfo
                {
                    Stage = "[1] Native Libraries",
                    Message = result.Native.FailureSummary,
                    NextStep = "Copy the seven required vendor DLLs (FK623Attend, FKAttend, FKViaDev, " +
                               "LFWViaDev, FKPwdEncDec, FpDataConv, FaceDataConv) beside the probe exe " +
                               "or pass --dll-dir; then run again. No automatic copy or port/password retry is performed."
                };
                result.ExitCode = 2;
                return result;
            }

            // ---------- Stage B ----------
            result.ConnectionAttempted = true;
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                result.ConnectHandle = channel.Connect(options);
            }
            catch (Exception ex)
            {
                result.ConnectExceptionType = ex.GetType().Name;
                result.ConnectSucceeded = false;
                result.ConnectRawCode = result.ConnectHandle;
                sw.Stop();
                result.ConnectDurationMs = sw.ElapsedMilliseconds;
                FailConnect(result, ex.GetType().Name, NativeMessage(ex));
                SkipReads(result);
                FinallyDisconnect(result, channel);
                result.ExitCode = ComputeExitCode(result);
                return result;
            }
            sw.Stop();
            result.ConnectDurationMs = sw.ElapsedMilliseconds;
            result.ConnectSucceeded = result.ConnectHandle > 0;
            result.ConnectRawCode = result.ConnectHandle;

            if (!result.ConnectSucceeded)
            {
                FailConnect(result, null, null);
                SkipReads(result);
                FinallyDisconnect(result, channel);
                result.ExitCode = ComputeExitCode(result);
                return result;
            }

            // ---------- Stages C..E ----------
            try
            {
                RunProductStage(result, channel);
                if (result.ProductStage == StageStatus.Ok)
                {
                    RunDeviceInfoStage(result, channel);
                }
                else
                {
                    result.DeviceInfoStage = StageStatus.Skipped;
                    result.DeviceTimeStage = StageStatus.Skipped;
                }
                if (result.DeviceInfoStage == StageStatus.Ok)
                {
                    RunDeviceTimeStage(result, channel);
                }
                else
                {
                    result.DeviceTimeStage = StageStatus.Skipped;
                }
            }
            catch (Exception ex)
            {
                AttachReadFailure(result, "[C-E] Read stage", "SDK call", null, null, ex.GetType().Name, NativeMessage(ex));
                if (result.ProductStage != StageStatus.Ok) { result.ProductStage = StageStatus.Failed; }
                if (result.DeviceInfoStage != StageStatus.Ok) { result.DeviceInfoStage = StageStatus.Failed; }
                if (result.DeviceTimeStage != StageStatus.Ok) { result.DeviceTimeStage = StageStatus.Failed; }
            }
            finally
            {
                FinallyDisconnect(result, channel);
            }

            result.ExitCode = ComputeExitCode(result);
            return result;
        }

        private static void RunProductStage(ProbeResult result, IDeviceChannel channel)
        {
            string serial = null;
            int code1 = SafeCall(result, channel, delegate { return channel.GetProductData(1, out serial); });
            if (code1 != 1)
            {
                result.ProductStage = StageStatus.Failed;
                AttachReadFailure(result, "[4] Product Information", "FK_GetProductData(1)", code1,
                    channel.LastError, null, null);
                return;
            }
            result.SerialNumber = serial;

            string productCode = null;
            int code3 = SafeCall(result, channel, delegate { return channel.GetProductData(3, out productCode); });
            if (code3 != 1)
            {
                result.ProductStage = StageStatus.Failed;
                result.ProductCode = null;
                AttachReadFailure(result, "[4] Product Information", "FK_GetProductData(3)", code3,
                    channel.LastError, null, null);
                return;
            }
            result.ProductCode = productCode;
            result.ProductStage = StageStatus.Ok;
        }

        private static void RunDeviceInfoStage(ProbeResult result, IDeviceChannel channel)
        {
            int machine = 0;
            int code2 = SafeCall(result, channel, delegate { return channel.GetDeviceInfo(2, out machine); });
            if (code2 != 1)
            {
                result.DeviceInfoStage = StageStatus.Failed;
                AttachReadFailure(result, "[5] Device Information", "FK_GetDeviceInfo(2)", code2,
                    channel.LastError, null, null);
                return;
            }
            result.DeviceMachineNumber = machine;
            result.HasDeviceMachineNumber = true;

            int language = 0;
            int code3 = SafeCall(result, channel, delegate { return channel.GetDeviceInfo(3, out language); });
            if (code3 != 1)
            {
                result.DeviceInfoStage = StageStatus.Failed;
                result.HasDeviceLanguage = false;
                AttachReadFailure(result, "[5] Device Information", "FK_GetDeviceInfo(3)", code3,
                    channel.LastError, null, null);
                return;
            }
            result.DeviceLanguage = language;
            result.HasDeviceLanguage = true;
            result.DeviceInfoStage = StageStatus.Ok;
        }

        private static void RunDeviceTimeStage(ProbeResult result, IDeviceChannel channel)
        {
            DateTime time = DateTime.MinValue;
            int code = SafeCall(result, channel, delegate { return channel.GetDeviceTime(out time); });
            if (code != 1)
            {
                result.DeviceTimeStage = StageStatus.Failed;
                AttachReadFailure(result, "[6] Device Time", "FK_GetDeviceTime", code,
                    channel.LastError, null, null);
                return;
            }
            result.DeviceTime = time;
            result.HasDeviceTime = true;
            result.DeviceTimeStage = StageStatus.Ok;
        }

        private static int SafeCall(ProbeResult result, IDeviceChannel channel, Func<int> call)
        {
            try
            {
                return call();
            }
            catch (Exception ex)
            {
                AttachReadFailure(result, "[C-E] Read stage", "SDK call", 0, 0, ex.GetType().Name, NativeMessage(ex));
                throw;
            }
        }

        private static void FailConnect(ProbeResult result, string exceptionType, string message)
        {
            if (result.FirstFailure == null)
            {
                result.FirstFailure = new ProbeResult.FailureInfo
                {
                    Stage = "[3] Connection",
                    Function = "FK_ConnectNet",
                    HasRawCode = true,
                    RawCode = result.ConnectRawCode,
                    ExceptionType = exceptionType,
                    Message = message,
                    NextStep = "Connection failed. Verify device power/LAN, IP, TCP port, machine number " +
                               "and the numeric network password; check the firewall allows outbound TCP to the device. " +
                               "No automatic retry, port scan or alternate password is attempted."
                };
            }
        }

        private static void AttachReadFailure(ProbeResult result, string stage, string function, int? rawCode, int? lastError, string exceptionType, string message)
        {
            ProbeResult.FailureInfo f = new ProbeResult.FailureInfo
            {
                Stage = stage,
                Function = function,
                ExceptionType = exceptionType,
                Message = message
            };
            if (rawCode.HasValue) { f.HasRawCode = true; f.RawCode = rawCode.Value; }
            if (lastError.HasValue) { f.HasLastError = true; f.LastError = lastError.Value; }
            if (message == null && rawCode.HasValue) { f.Message = "SDK status " + rawCode.Value + ": " + FkErrorText.For(rawCode.Value); }
            f.NextStep = "Report this output to the developer. No additional command is sent automatically " +
                         "(no FK_EnableDevice, no state change, no retry).";
            if (result.FirstFailure == null) { result.FirstFailure = f; }
        }

        private static void SkipReads(ProbeResult result)
        {
            result.ProductStage = StageStatus.Skipped;
            result.DeviceInfoStage = StageStatus.Skipped;
            result.DeviceTimeStage = StageStatus.Skipped;
        }

        private static void FinallyDisconnect(ProbeResult result, IDeviceChannel channel)
        {
            try
            {
                if (channel.IsConnected)
                {
                    channel.Disconnect();
                    result.DisconnectCalled = true;
                }
                else
                {
                    result.DisconnectCalled = false;
                }
            }
            catch (Exception ex)
            {
                result.DisconnectNote = "Disconnect raised " + ex.GetType().Name;
            }
        }

        private static int ComputeExitCode(ProbeResult result)
        {
            if (!result.ConnectSucceeded) { return 3; }
            if (result.ProductStage == StageStatus.Failed ||
                result.DeviceInfoStage == StageStatus.Failed ||
                result.DeviceTimeStage == StageStatus.Failed ||
                result.FirstFailure != null)
            {
                return 4;
            }
            return 0;
        }

        private static string NativeMessage(Exception ex)
        {
            if (ex == null) { return null; }
            if (ex is DllNotFoundException) { return "Native SDK DLL not found: " + ex.Message; }
            if (ex is BadImageFormatException) { return "Native SDK has a different architecture (x86 expected): " + ex.Message; }
            return ex.Message;
        }
    }
}
