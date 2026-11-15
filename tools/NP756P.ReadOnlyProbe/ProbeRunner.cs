using System;
using System.Diagnostics;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>Identifies the probe read stage a SafeCall belongs to.</summary>
    internal enum ProbeStage
    {
        Product,
        DeviceInfo,
        DeviceTime
    }

    /// <summary>
    /// Orchestrates the probe stages in the fixed order A..F:
    ///   A) native library validation (static, no loading)
    ///   B) FK_ConnectNet (once, recorded)
    ///   C) FK_GetProductData 1 (serial) and 3 (product code)
    ///   D) FK_GetDeviceInfo 2 (machine number) and 3 (language)
    ///   E) FK_GetDeviceTime (read-only)
    ///   F) FK_DisConnect in a finally block whenever a live handle exists
    ///
    /// Failure bookkeeping rules:
    ///  - a raw SDK return value (negative or zero) is recorded as the raw code;
    ///    when Connect ends in an exception NO raw code is recorded at all
    ///    (a zero would be misread as "No support");
    ///  - when a read call ends in an exception the failure carries the exact
    ///    SDK function name, the exception type and NO fake raw/last-error codes;
    ///  - the stage whose call actually failed is FAILED; every later stage that
    ///    was never attempted is SKIPPED (never FAILED);
    ///  - the network password is used for Connect only and never recorded.
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
                result.DisconnectNeeded = false;
                result.DisconnectCalled = false;
                result.DisconnectNote = "not reached (native validation failed before any connection)";
                result.FirstFailure = new ProbeResult.FailureInfo
                {
                    Stage = "[1] Native Libraries",
                    Message = result.Native.FailureSummary,
                    NextStep = "Fix the reported DLL problem (copy the seven required vendor DLLs " +
                               "FK623Attend, FKAttend, FKViaDev, LFWViaDev, FKPwdEncDec, FpDataConv, " +
                               "FaceDataConv beside the probe exe or pass --dll-dir; all must be x86) " +
                               "and run again. No automatic copy, retry or port/password scan is performed."
                };
                result.ExitCode = ExitCodes.NativeLibraryStageFailed;
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
                sw.Stop();
                result.ConnectDurationMs = sw.ElapsedMilliseconds;
                result.ConnectSucceeded = false;
                result.ConnectExceptionType = ex.GetType().Name;
                // Exception path: the SDK returned nothing, so there is NO raw code.
                result.ConnectHandle = 0;
                result.ConnectHasRawCode = false;
                result.ConnectRawCode = 0;
                FailConnect(result, ex.GetType().Name, NativeMessage(ex), false);
                SkipReads(result);
                FinallyDisconnect(result, channel);
                result.ExitCode = ComputeExitCode(result);
                return result;
            }
            sw.Stop();
            result.ConnectDurationMs = sw.ElapsedMilliseconds;
            result.ConnectHasRawCode = true;
            result.ConnectRawCode = result.ConnectHandle;
            result.ConnectSucceeded = result.ConnectHandle > 0;

            if (!result.ConnectSucceeded)
            {
                // Raw return value path (negative or zero): the raw code is recorded.
                FailConnect(result, null, null, true);
                SkipReads(result);
                FinallyDisconnect(result, channel);
                result.ExitCode = ComputeExitCode(result);
                return result;
            }

            // ---------- Stages C..E ----------
            try
            {
                RunProductStage(result, channel);
                if (result.ProductStage != StageStatus.Ok)
                {
                    SkipStagesAfter(result, ProbeStage.Product);
                }
                else
                {
                    RunDeviceInfoStage(result, channel);
                    if (result.DeviceInfoStage != StageStatus.Ok)
                    {
                        SkipStagesAfter(result, ProbeStage.DeviceInfo);
                    }
                    else
                    {
                        RunDeviceTimeStage(result, channel);
                        // a failed time stage has no later stage to skip
                    }
                }
            }
            catch (Exception ex)
            {
                // Safety net: the exact failing function/stage has already been
                // recorded by SafeCall; stages that were never attempted stay
                // SKIPPED (Pending is resolved as Skipped here, never as Failed).
                MarkPendingAsSkipped(result);
                if (result.FirstFailure == null)
                {
                    AttachReadFailure(result, "[C-E] Read stage", "SDK call", null, null,
                        ex.GetType().Name, NativeMessage(ex));
                }
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
            int code1 = SafeCall(result, channel, "[4] Product Information", "FK_GetProductData(1)",
                ProbeStage.Product, delegate { return channel.GetProductData(1, out serial); });
            if (code1 != 1)
            {
                result.ProductStage = StageStatus.Failed;
                AttachReadFailure(result, "[4] Product Information", "FK_GetProductData(1)", code1,
                    channel.LastError, null, null);
                return;
            }
            result.SerialNumber = serial;

            string productCode = null;
            int code3 = SafeCall(result, channel, "[4] Product Information", "FK_GetProductData(3)",
                ProbeStage.Product, delegate { return channel.GetProductData(3, out productCode); });
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
            int code2 = SafeCall(result, channel, "[5] Device Information", "FK_GetDeviceInfo(2)",
                ProbeStage.DeviceInfo, delegate { return channel.GetDeviceInfo(2, out machine); });
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
            int code3 = SafeCall(result, channel, "[5] Device Information", "FK_GetDeviceInfo(3)",
                ProbeStage.DeviceInfo, delegate { return channel.GetDeviceInfo(3, out language); });
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
            int code = SafeCall(result, channel, "[6] Device Time", "FK_GetDeviceTime",
                ProbeStage.DeviceTime, delegate { return channel.GetDeviceTime(out time); });
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

        /// <summary>
        /// Runs one SDK read call. On exception the failure is recorded with the
        /// exact function name and no fake raw/last-error codes, the owning stage
        /// is marked FAILED, every later stage is marked SKIPPED, and the
        /// exception is rethrown so the runner-level finally can disconnect.
        /// </summary>
        private static int SafeCall(ProbeResult result, IDeviceChannel channel, string stageLabel, string function, ProbeStage stage, Func<int> call)
        {
            try
            {
                return call();
            }
            catch (Exception ex)
            {
                MarkStage(result, stage, StageStatus.Failed);
                SkipStagesAfter(result, stage);
                AttachReadFailure(result, stageLabel, function, null, null,
                    ex.GetType().Name, NativeMessage(ex));
                throw;
            }
        }

        private static void MarkStage(ProbeResult result, ProbeStage stage, StageStatus status)
        {
            if (stage == ProbeStage.Product) { result.ProductStage = status; }
            else if (stage == ProbeStage.DeviceInfo) { result.DeviceInfoStage = status; }
            else if (stage == ProbeStage.DeviceTime) { result.DeviceTimeStage = status; }
        }

        private static void SkipStagesAfter(ProbeResult result, ProbeStage stage)
        {
            if (stage == ProbeStage.Product)
            {
                result.DeviceInfoStage = StageStatus.Skipped;
                result.DeviceTimeStage = StageStatus.Skipped;
            }
            else if (stage == ProbeStage.DeviceInfo)
            {
                result.DeviceTimeStage = StageStatus.Skipped;
            }
        }

        private static void MarkPendingAsSkipped(ProbeResult result)
        {
            if (result.ProductStage == StageStatus.Pending) { result.ProductStage = StageStatus.Skipped; }
            if (result.DeviceInfoStage == StageStatus.Pending) { result.DeviceInfoStage = StageStatus.Skipped; }
            if (result.DeviceTimeStage == StageStatus.Pending) { result.DeviceTimeStage = StageStatus.Skipped; }
        }

        private static void FailConnect(ProbeResult result, string exceptionType, string message, bool hasRawCode)
        {
            if (result.FirstFailure == null)
            {
                result.FirstFailure = new ProbeResult.FailureInfo
                {
                    Stage = "[3] Connection",
                    Function = "FK_ConnectNet",
                    HasRawCode = hasRawCode,
                    RawCode = hasRawCode ? result.ConnectRawCode : 0,
                    ExceptionType = exceptionType,
                    Message = hasRawCode && message == null
                        ? "FK_ConnectNet returned " + result.ConnectRawCode +
                          ": " + FkErrorText.For(result.ConnectRawCode)
                        : message,
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
            // Was a disconnect needed at all?
            bool needed = channel != null && channel.IsConnected;
            result.DisconnectNeeded = needed;
            result.DisconnectCalled = false;

            if (!needed)
            {
                // No live handle: nothing to close; exit 0 remains reachable
                // (e.g. when connect itself failed).
                result.DisconnectNote = "no live handle (nothing to close)";
                return;
            }

            try
            {
                channel.Disconnect();
                result.DisconnectFailed = false;
            }
            catch (Exception ex)
            {
                result.DisconnectFailed = true;
                result.DisconnectExceptionType = ex.GetType().Name;
                result.DisconnectNote = "FK_DisConnect raised " + ex.GetType().Name;
                if (result.FirstFailure == null)
                {
                    result.FirstFailure = new ProbeResult.FailureInfo
                    {
                        Stage = "[7] Disconnect",
                        Function = "FK_DisConnect",
                        ExceptionType = ex.GetType().Name,
                        Message = NativeMessage(ex),
                        NextStep = "The connection may still be open on the device side. Report this output " +
                                   "to the developer; the probe sends nothing else on its own."
                    };
                }
            }
            finally
            {
                // The FK_DisConnect entry point was really invoked, even when it
                // raised: the attempt itself counts as "called".
                result.DisconnectCalled = true;
            }
        }

        private static int ComputeExitCode(ProbeResult result)
        {
            if (!result.ConnectSucceeded) { return ExitCodes.ConnectFailed; }

            // Zero is only allowed when the connection was actually closed
            // (or there never was a valid handle, handled above).
            if (result.DisconnectFailed || (result.DisconnectNeeded && !result.DisconnectCalled))
            {
                return ExitCodes.ReadStageFailed;
            }
            if (result.ProductStage == StageStatus.Failed ||
                result.DeviceInfoStage == StageStatus.Failed ||
                result.DeviceTimeStage == StageStatus.Failed ||
                result.FirstFailure != null)
            {
                return ExitCodes.ReadStageFailed;
            }
            return ExitCodes.Success;
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
