using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NP756P.ReadOnlyProbe;

namespace NP756P.ReadOnlyProbe.Tests
{
    /// <summary>
    /// Device-free test runner. No native vendor DLL is loaded and no socket is
    /// opened anywhere in this suite: SDK behavior is simulated via MockDeviceChannel
    /// and the PE validator is tested with synthetic PE byte images.
    /// Run: dotnet run -c Release (project NP756P.ReadOnlyProbe.Tests).
    /// </summary>
    internal static class TestRunner
    {
        private static int _passed;
        private static int _failed;
        private static readonly List<string> Failures = new List<string>();

        private static void Check(string name, bool condition)
        {
            if (condition) { _passed++; Console.WriteLine("PASS  " + name); }
            else { _failed++; Failures.Add(name); Console.WriteLine("FAIL  " + name); }
        }

        private static int Main()
        {
            Console.WriteLine("NP756P.ReadOnlyProbe device-free test suite");
            Console.WriteLine("------------------------------------------------");

            TestArgumentParsing();
            TestValidationRules();
            TestPasswordRules();
            TestPeParser();
            TestValidatorOnMissingDir();
            TestValidatorOnMissingDll();
            TestValidatorOnWrongArch();
            TestValidatorOnGoodFakeChain();
            TestRunnerHappyPath();
            TestRunnerConnectFailure();
            TestRunnerConnectException();
            TestRunnerProductFailureSkipsRest();
            TestRunnerCleanupOnException();
            TestRunnerDisconnectOnlyWithValidHandle();
            TestErrorTextMapping();
            TestReportNeverContainsPassword();
            TestOptionsToStringNeverContainsPassword();

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Total: " + (_passed + _failed) + "  Passed: " + _passed + "  Failed: " + _failed);
            foreach (string f in Failures)
            {
                Console.WriteLine("  FAILED CASE: " + f);
            }
            return _failed == 0 ? 0 : 1;
        }

        // ---------------- argument parsing ----------------

        private static void TestArgumentParsing()
        {
            ProbeOptions o;
            List<string> errs;

            Check("parse: empty args use defaults",
                ProbeOptions.TryParse(new string[0], out o, out errs) &&
                o.Ip == "192.168.1.75" && o.Port == 4238 && o.MachineNumber == 1 &&
                o.TimeoutMs == 5000 && o.Protocol == ProbeProtocol.Tcp && o.License == 1261 &&
                errs.Count == 0);

            Check("parse: null args use defaults",
                ProbeOptions.TryParse(null, out o, out errs) && o.Port == 4238 && errs.Count == 0);

            Check("parse: full space-separated args",
                ProbeOptions.TryParse(
                    new[] { "--ip", "10.0.0.9", "--port", "5005", "--machine", "3", "--timeout", "9000", "--protocol", "udp", "--license", "2000" },
                    out o, out errs) &&
                o.Ip == "10.0.0.9" && o.Port == 5005 && o.MachineNumber == 3 &&
                o.TimeoutMs == 9000 && o.Protocol == ProbeProtocol.Udp && o.License == 2000);

            Check("parse: equals-sign form",
                ProbeOptions.TryParse(new[] { "--ip=10.1.2.3", "--port=4238" }, out o, out errs) &&
                o.Ip == "10.1.2.3" && o.Port == 4238 && errs.Count == 0);

            Check("parse: unknown arg rejected",
                !ProbeOptions.TryParse(new[] { "--bogus", "1" }, out o, out errs));

            Check("parse: dangling value rejected",
                !ProbeOptions.TryParse(new[] { "--port" }, out o, out errs));

            string dir = Directory.GetCurrentDirectory();
            Check("parse: existing dll-dir accepted",
                ProbeOptions.TryParse(new[] { "--dll-dir", dir }, out o, out errs) && o.DllDir != null);

            Check("parse: non-existing dll-dir rejected",
                !ProbeOptions.TryParse(new[] { "--dll-dir", "/no/such/dir/__x" }, out o, out errs));

            Check("parse: protocol tcp maps to 0",
                ProbeOptions.TryParse(new[] { "--protocol", "tcp" }, out o, out errs) && o.ProtocolValue == 0);

            Check("parse: protocol udp maps to 1",
                ProbeOptions.TryParse(new[] { "--protocol", "udp" }, out o, out errs) && o.ProtocolValue == 1);
        }

        // ---------------- validation rules ----------------

        private static void TestValidationRules()
        {
            Check("ip: valid dotted quads",
                ProbeOptions.IsValidIp("192.168.1.75") && ProbeOptions.IsValidIp("0.0.0.0") &&
                ProbeOptions.IsValidIp("255.255.255.255") && ProbeOptions.IsValidIp("127.0.0.1"));

            Check("ip: invalid values rejected",
                !ProbeOptions.IsValidIp("") && !ProbeOptions.IsValidIp(null) &&
                !ProbeOptions.IsValidIp("192.168.1") && !ProbeOptions.IsValidIp("192.168.1.1.1") &&
                !ProbeOptions.IsValidIp("192.168.1.256") && !ProbeOptions.IsValidIp("a.b.c.d") &&
                !ProbeOptions.IsValidIp("192.168.01.5") && !ProbeOptions.IsValidIp(" 192.168.1.75") &&
                !ProbeOptions.IsValidIp("-1.2.3.4"));

            Check("port: valid range",
                ProbeOptions.IsValidPort(1) && ProbeOptions.IsValidPort(65535) && ProbeOptions.IsValidPort(4238));

            Check("port: invalid values rejected",
                !ProbeOptions.IsValidPort(0) && !ProbeOptions.IsValidPort(-1) &&
                !ProbeOptions.IsValidPort(65536) && !ProbeOptions.IsValidPort(100000));

            ProbeOptions o;
            List<string> errs;
            Check("port: non-numeric rejected",
                !ProbeOptions.TryParse(new[] { "--port", "abc" }, out o, out errs) && errs.Count > 0);
            Check("port: empty value rejected",
                !ProbeOptions.TryParse(new[] { "--port", "" }, out o, out errs));
            Check("port: null value rejected",
                !ProbeOptions.TryParse(new[] { "--port", null }, out o, out errs));

            Check("machine: valid range",
                ProbeOptions.IsValidMachineNumber(1) && ProbeOptions.IsValidMachineNumber(999999));

            Check("machine: invalid rejected",
                !ProbeOptions.IsValidMachineNumber(0) && !ProbeOptions.IsValidMachineNumber(-3) &&
                !ProbeOptions.IsValidMachineNumber(1000000));

            Check("timeout: valid range",
                ProbeOptions.IsValidTimeout(100) && ProbeOptions.IsValidTimeout(5000) && ProbeOptions.IsValidTimeout(600000));

            Check("timeout: invalid rejected",
                !ProbeOptions.IsValidTimeout(0) && !ProbeOptions.IsValidTimeout(99) &&
                !ProbeOptions.IsValidTimeout(600001));

            Check("protocol: invalid token rejected",
                !ProbeOptions.TryParse(new[] { "--protocol", "sctp" }, out o, out errs));
        }

        // ---------------- password rules ----------------

        private static void TestPasswordRules()
        {
            int p;
            string err;

            Check("password: numeric accepted",
                ProbeOptions.TryParsePassword("1234", out p, out err) && p == 1234 && err == null);

            Check("password: zero accepted as explicit value",
                ProbeOptions.TryParsePassword("0", out p, out err) && p == 0);

            Check("password: leading zeros accepted",
                ProbeOptions.TryParsePassword("0012", out p, out err) && p == 12);

            Check("password: surrounding spaces tolerated",
                ProbeOptions.TryParsePassword(" 456 ", out p, out err) && p == 456);

            Check("password: empty rejected",
                !ProbeOptions.TryParsePassword("", out p, out err) && err != null);
            Check("password: whitespace-only rejected",
                !ProbeOptions.TryParsePassword("   ", out p, out err));
            Check("password: non-numeric rejected",
                !ProbeOptions.TryParsePassword("12a4", out p, out err) &&
                !ProbeOptions.TryParsePassword("secret", out p, out err));
            Check("password: negative rejected",
                !ProbeOptions.TryParsePassword("-5", out p, out err));
            Check("password: overflow rejected",
                !ProbeOptions.TryParsePassword("99999999999", out p, out err));
        }

        // ---------------- PE parser (synthetic images) ----------------

        /// <summary>Builds a minimal but structurally valid PE32 image with the given machine,
        /// export names and import DLL names (all inside one .rdata section).</summary>
        private static byte[] BuildSyntheticPe(ushort machine, string[] exportNames, string[] importDlls)
        {
            const int peOff = 0x80;
            const int optSize = 224;             // PE32 optional header
            const int secOff = peOff + 24 + optSize;
            const int secCount = 1;
            const int rawSec = 0x400;            // section file offset
            const int vaSec = 0x1000;            // section RVA
            const int secLen = 0x400;

            MemoryStream ms = new MemoryStream();
            BinaryWriter w = new BinaryWriter(ms);

            // DOS header
            w.Write((ushort)0x5A4D);
            ms.Position = 0x3C; w.Write((int)peOff);

            // NT headers
            ms.Position = peOff;
            w.Write(new byte[] { (byte)'P', (byte)'E', 0, 0 });
            w.Write(machine);
            w.Write((ushort)secCount);
            w.Write((int)0);        // timestamp
            w.Write((int)0);        // symbol table
            w.Write((int)0);        // symbols
            w.Write((ushort)optSize);
            w.Write((ushort)0x102); // characteristics

            // optional header (PE32)
            int opt = peOff + 24;
            ms.Position = opt;
            w.Write((ushort)0x10B); // magic
            w.Write((ushort)0);      // linker
            w.Write((int)0); w.Write((int)0);
            w.Write((int)0x1000); w.Write((int)0x1000); // align
            w.Write((ushort)4); w.Write((ushort)0);    // subsystem version
            w.Write((int)0); w.Write((int)rawSec + secLen);
            for (int i = 0; i < 8; i++) { w.Write((int)0); } // stack/heap/loader flags
            w.Write((int)0x3);      // subsystem = console
            w.Write((ushort)0); w.Write((ushort)0);
            w.Write((int)0x40);    // dll characteristics
            for (int i = 0; i < 5; i++) { w.Write((int)0); } // 76..95 (fills to data directories)

            // data directories at opt+96: [0]=export, [1]=import
            int dd = opt + 96;
            int exportRva = 0x1000;
            int namesRva = 0x1120;
            int funcsRva = 0x1100;
            int ordsRva = 0x1140;
            int stringsRva = 0x1160;
            ms.Position = dd;
            w.Write(exportRva);
            w.Write((int)0);      // export size
            int importRva = importDlls.Length > 0 ? 0x1300 : 0;
            w.Write(importRva);
            w.Write((int)0);      // import size

            // section header
            ms.Position = secOff;
            byte[] secName = new byte[8];
            byte[] name = Encoding.ASCII.GetBytes(".rdata");
            Array.Copy(name, secName, name.Length);
            w.Write(secName);
            w.Write((int)secLen);     // virtual size
            w.Write((int)vaSec);      // virtual address
            w.Write((int)secLen);     // raw size
            w.Write((int)rawSec);     // raw pointer
            w.Write((int)0); w.Write((int)0); w.Write((ushort)0); w.Write((ushort)0);
            w.Write((int)0x40000040);

            // export directory
            ms.Position = rawSec;    // RVA 0x1000
            w.Write((int)0); w.Write((int)0); w.Write((ushort)0); w.Write((ushort)0);
            w.Write(0x10C0);          // dll name rva
            w.Write(1);              // base
            w.Write(exportNames.Length);
            w.Write(exportNames.Length);
            w.Write(funcsRva);
            w.Write(namesRva);
            w.Write(ordsRva);

            // function addresses
            ms.Position = rawSec + (funcsRva - vaSec);
            for (int i = 0; i < exportNames.Length; i++) { w.Write(0x1200 + i); }

            // name pointers
            ms.Position = rawSec + (namesRva - vaSec);
            int nextString = (int)stringsRva;
            List<int> nameRvas = new List<int>();
            for (int i = 0; i < exportNames.Length; i++)
            {
                nameRvas.Add(nextString);
                nextString += exportNames[i].Length + 1;
            }
            foreach (int r in nameRvas) { w.Write(r); }

            // ordinals
            ms.Position = rawSec + (ordsRva - vaSec);
            for (int i = 0; i < exportNames.Length; i++) { w.Write((ushort)i); }

            // export name strings
            ms.Position = rawSec + (stringsRva - vaSec);
            foreach (string n in exportNames) { w.Write(Encoding.ASCII.GetBytes(n)); w.Write((byte)0); }

            // dll name string at 0x10C0
            ms.Position = rawSec + (0x10C0 - vaSec);
            w.Write(Encoding.ASCII.GetBytes("synth.dll")); w.Write((byte)0);

            // import descriptors at 0x1300
            if (importDlls.Length > 0)
            {
                ms.Position = rawSec + (importRva - vaSec);
                int dllNameRva = 0x1380;
                foreach (string dll in importDlls)
                {
                    w.Write((int)0x1400); // original first thunk
                    w.Write((int)0); w.Write((int)0);
                    w.Write(dllNameRva);
                    w.Write((int)0x1400); // first thunk
                    dllNameRva += dll.Length + 1;
                }
                w.Write((long)0); // terminator

                ms.Position = rawSec + (0x1380 - vaSec);
                foreach (string dll in importDlls)
                {
                    w.Write(Encoding.ASCII.GetBytes(dll)); w.Write((byte)0);
                }
                // thunk arrays (empty)
                ms.Position = rawSec + (0x1400 - vaSec);
                w.Write((int)0); w.Write((int)0);
            }

            byte[] buf = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buf, 0, buf.Length);
            return buf;
        }

        private static void TestPeParser()
        {
            List<string> exports;
            List<string> imports;
            string err;
            ushort machine;

            byte[] x86pe = BuildSyntheticPe(0x014C,
                new[] { "FK_ConnectNet", "FK_DisConnect", "FK_GetProductData" },
                new[] { "kernel32.dll", "fkattend.dll" });

            Check("pe: valid synthetic x86 PE parses",
                NativeLibraryValidator.TryReadPeInfo(x86pe, out machine, out exports, out imports, out err) &&
                machine == 0x014C);

            Check("pe: export names extracted",
                exports.Contains("FK_ConnectNet") && exports.Contains("FK_DisConnect") &&
                exports.Contains("FK_GetProductData") && exports.Count == 3);

            Check("pe: import dlls extracted",
                imports.Contains("kernel32.dll") && imports.Contains("fkattend.dll") && imports.Count == 2);

            byte[] x64pe = BuildSyntheticPe(0x8664, new[] { "A" }, new string[0]);
            ushort m64;
            Check("pe: x64 machine detected",
                NativeLibraryValidator.TryReadPeInfo(x64pe, out m64, out exports, out imports, out err) &&
                m64 == 0x8664);

            byte[] noExp = BuildSyntheticPe(0x014C, new string[0], new string[0]);
            Check("pe: no exports tolerated",
                NativeLibraryValidator.TryReadPeInfo(noExp, out machine, out exports, out imports, out err) &&
                exports.Count == 0 && imports.Count == 0);

            byte[] notPe = Encoding.ASCII.GetBytes("MZ not really a PE image at all ..............");
            ushort dummy;
            List<string> l1, l2;
            Check("pe: non-PE rejected",
                !NativeLibraryValidator.TryReadPeInfo(notPe, out dummy, out l1, out l2, out err) && err != null);

            Check("pe: null rejected",
                !NativeLibraryValidator.TryReadPeInfo(null, out dummy, out l1, out l2, out err));
        }

        // ---------------- validator behavior ----------------

        private static string TempDir()
        {
            string d = Path.Combine(Path.GetTempPath(), "np756p_test_" + Guid.NewGuid().ToString("N").Substring(0, 8));
            Directory.CreateDirectory(d);
            return d;
        }

        private static NativeLibraryValidator.ValidationReport ValidateChain(
            string dir, ushort machine, bool omitOneDll, bool omitDependency)
        {
            string[] files =
            {
                "FK623Attend.dll", "FKAttend.dll", "FKViaDev.dll", "LFWViaDev.dll",
                "FKPwdEncDec.dll", "FpDataConv.dll", "FaceDataConv.dll"
            };
            int i = 0;
            foreach (string f in files)
            {
                if (omitOneDll && f == "FKViaDev.dll") { i++; continue; }
                string[] imports = (f == "FK623Attend.dll")
                    ? new[] { "kernel32.dll", "FKAttend.dll" }
                    : (f == "FKAttend.dll" && omitDependency
                        ? new[] { "kernel32.dll", "FKViaDev.dll", "NOT_PRESENT.dll" }
                        : new[] { "kernel32.dll", "user32.dll" });
                string[] exports = (f == "FK623Attend.dll")
                    ? NativeLibraryValidator.AllowedFunctions
                    : new[] { "FV_Sample_" + i };
                File.WriteAllBytes(Path.Combine(dir, f), BuildSyntheticPe(machine, exports, imports));
                i++;
            }
            return NativeLibraryValidator.Validate(dir);
        }

        private static void TestValidatorOnMissingDir()
        {
            NativeLibraryValidator.ValidationReport r =
                NativeLibraryValidator.Validate(Path.Combine(Path.GetTempPath(), "no_such_dir_zzz"));
            Check("validator: missing directory fails",
                !r.Succeeded && !r.DirectoryExists && r.FailureSummary.Length > 0);
        }

        private static void TestValidatorOnMissingDll()
        {
            string dir = TempDir();
            NativeLibraryValidator.ValidationReport r = ValidateChain(dir, 0x014C, true, false);
            Check("validator: missing FKViaDev.dll fails with DllNotFoundException prediction",
                !r.Succeeded && r.MissingFiles.Count == 1 &&
                r.MissingFiles[0].FileName == "FKViaDev.dll" &&
                r.MissingFiles[0].PredictedError == "DllNotFoundException" &&
                r.FailureSummary.Contains("FKViaDev.dll"));
        }

        private static void TestValidatorOnWrongArch()
        {
            string dir = TempDir();
            NativeLibraryValidator.ValidationReport r = ValidateChain(dir, 0x8664, false, false);
            Check("validator: x64 DLLs fail with BadImageFormatException prediction",
                !r.Succeeded && r.WrongArchFiles.Count == 7 &&
                r.WrongArchFiles[0].PredictedError == "BadImageFormatException");
        }

        private static void TestValidatorOnGoodFakeChain()
        {
            string dir = TempDir();
            NativeLibraryValidator.ValidationReport r = ValidateChain(dir, 0x014C, false, false);
            Check("validator: complete x86 chain with all allowed exports succeeds",
                r.Succeeded && r.Entries.Count == 7 && r.MissingAllowedFunctions.Count == 0);

            // dependency check: FKAttend.dll imports an unknown DLL that is absent
            string dir2 = TempDir();
            NativeLibraryValidator.ValidationReport r2 = ValidateChain(dir2, 0x014C, false, true);
            Check("validator: unresolved native dependency fails",
                !r2.Succeeded && r2.FailureSummary.Contains("NOT_PRESENT.dll"));

            // FK623Attend.dll missing an allowed export
            string dir3 = TempDir();
            string[] files =
            {
                "FK623Attend.dll", "FKAttend.dll", "FKViaDev.dll", "LFWViaDev.dll",
                "FKPwdEncDec.dll", "FpDataConv.dll", "FaceDataConv.dll"
            };
            foreach (string f in files)
            {
                string[] exports = (f == "FK623Attend.dll")
                    ? new[] { "FK_ConnectNet", "FK_DisConnect" }  // missing 4 allowed fns
                    : new[] { "FV_X" };
                string[] imports = (f == "FK623Attend.dll")
                    ? new[] { "kernel32.dll", "FKAttend.dll" }
                    : new[] { "kernel32.dll" };
                File.WriteAllBytes(Path.Combine(dir3, f), BuildSyntheticPe(0x014C, exports, imports));
            }
            NativeLibraryValidator.ValidationReport r3 = NativeLibraryValidator.Validate(dir3);
            Check("validator: missing allowed export detected",
                !r3.Succeeded && r3.MissingAllowedFunctions.Count == 4 &&
                r3.MissingAllowedFunctions.Contains("FK_GetProductData") &&
                r3.MissingAllowedFunctions.Contains("FK_GetLastError"));
        }

        // ---------------- probe runner orchestration ----------------

        private static NativeLibraryValidator.ValidationReport GoodNative()
        {
            NativeLibraryValidator.ValidationReport r = new NativeLibraryValidator.ValidationReport();
            r.Succeeded = true;
            r.Directory = "<fake>";
            foreach (string f in NativeLibraryValidator.RequiredDlls)
            {
                NativeLibraryValidator.DllEntry e = new NativeLibraryValidator.DllEntry();
                e.FileName = f;
                e.Status = "FOUND";
                e.Architecture = "x86";
                if (f == "FK623Attend.dll") { e.Exports.AddRange(NativeLibraryValidator.AllowedFunctions); }
                r.Entries.Add(e);
            }
            return r;
        }

        private static void TestRunnerHappyPath()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            ProbeOptions options = new ProbeOptions();
            options.NetworkPassword = 987654;
            ProbeResult result = ProbeRunner.Run(options, mock, GoodNative());

            Check("runner: happy path connects once",
                mock.ConnectCalls == 1 && result.ConnectSucceeded && result.ExitCode == 0);

            Check("runner: product data captured",
                result.ProductStage == StageStatus.Ok &&
                result.SerialNumber == "SN-000123" && result.ProductCode == "NP756P");

            Check("runner: device info captured",
                result.DeviceInfoStage == StageStatus.Ok && result.HasDeviceMachineNumber &&
                result.DeviceMachineNumber == 7 && result.HasDeviceLanguage && result.DeviceLanguage == 0);

            Check("runner: device time captured",
                result.DeviceTimeStage == StageStatus.Ok && result.HasDeviceTime &&
                result.DeviceTime == new DateTime(2026, 11, 12, 9, 30, 0));

            Check("runner: disconnect called exactly once",
                mock.DisconnectCalls == 1 && result.DisconnectCalled);

            Check("runner: no failure recorded",
                result.FirstFailure == null);

            string report = result.Render();
            Check("runner: report shows serial and machine",
                report.Contains("SN-000123") && report.Contains("NP756P") &&
                report.Contains("192.168.1.75") && report.Contains("4238") &&
                report.Contains("2026-11-12 09:30:00"));
        }

        private static void TestRunnerConnectFailure()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.ConnectResult = -2;
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: connect failure stops reads",
                !result.ConnectSucceeded && result.ExitCode == 3 &&
                result.ProductStage == StageStatus.Skipped &&
                result.DeviceInfoStage == StageStatus.Skipped &&
                result.DeviceTimeStage == StageStatus.Skipped);

            Check("runner: connect failure never reads",
                mock.ProductCalls == 0 && mock.DeviceInfoCalls == 0 && mock.DeviceTimeCalls == 0);

            Check("runner: no disconnect without live handle",
                mock.DisconnectCalls == 0 && result.DisconnectCalled == false);

            Check("runner: failure block contains FK_ConnectNet",
                result.FirstFailure != null && result.FirstFailure.Function == "FK_ConnectNet" &&
                result.FirstFailure.RawCode == -2);
        }

        private static void TestRunnerConnectException()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.ConnectException = new DllNotFoundException("FK623Attend.dll");
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: DllNotFoundException mapped in failure",
                result.FirstFailure != null &&
                result.FirstFailure.ExceptionType == "DllNotFoundException" &&
                result.FirstFailure.Message != null &&
                result.FirstFailure.Message.Contains("Native SDK DLL not found"));

            Check("runner: connect exception yields exit code 3",
                result.ExitCode == 3 && mock.ProductCalls == 0);
        }

        private static void TestRunnerProductFailureSkipsRest()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.ProductStatus = -4;
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: product failure skips device info and time",
                result.ProductStage == StageStatus.Failed &&
                result.DeviceInfoStage == StageStatus.Skipped &&
                result.DeviceTimeStage == StageStatus.Skipped);

            Check("runner: product failure reads only product",
                mock.ProductCalls == 1 && mock.DeviceInfoCalls == 0 && mock.DeviceTimeCalls == 0);

            Check("runner: disconnect still runs in finally",
                mock.DisconnectCalls == 1);

            Check("runner: raw code and last error reported",
                result.FirstFailure != null && result.FirstFailure.HasRawCode &&
                result.FirstFailure.RawCode == -4 && result.FirstFailure.HasLastError &&
                result.FirstFailure.LastError == -4);

            Check("runner: exit code 4 for read failure",
                result.ExitCode == 4);
        }

        private static void TestRunnerCleanupOnException()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.ProductException = new InvalidOperationException("native call blew up");
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: exception during read still disconnects (finally)",
                mock.DisconnectCalls == 1 && result.DisconnectCalled);

            Check("runner: exception type recorded",
                result.FirstFailure != null &&
                result.FirstFailure.ExceptionType == "InvalidOperationException");

            Check("runner: stages marked failed and exit code 4",
                result.ProductStage == StageStatus.Failed && result.ExitCode == 4);
        }

        private static void TestRunnerDisconnectOnlyWithValidHandle()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.ConnectResult = -5;
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());
            Check("runner: invalid handle means no disconnect call",
                mock.DisconnectCalls == 0 && result.DisconnectCalled == false);
        }

        // ---------------- vendor error text ----------------

        private static void TestErrorTextMapping()
        {
            Check("errors: vendor meanings match decompiled map",
                FkErrorText.For(1) == "Successful" &&
                FkErrorText.For(0) == "No support" &&
                FkErrorText.For(-2) == "No Open Comm" &&
                FkErrorText.For(-4) == "Read Error" &&
                FkErrorText.For(-5) == "Parameter Error" &&
                FkErrorText.For(-10) == "License Error" &&
                FkErrorText.For(-15) == "Mistake fp data version");

            Check("errors: -13 not invented (unmapped marker)",
                FkErrorText.For(-13).Contains("Unmapped"));
        }

        // ---------------- password secrecy ----------------

        private static void TestReportNeverContainsPassword()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            ProbeOptions options = new ProbeOptions();
            options.NetworkPassword = 987654;
            ProbeResult result = ProbeRunner.Run(options, mock, GoodNative());
            string report = result.Render();
            Check("secrecy: report never contains the password",
                !report.Contains("987654") && !report.ToLowerInvariant().Contains("password=987654"));

            // failing path too
            MockDeviceChannel mock2 = new MockDeviceChannel();
            mock2.ConnectResult = -2;
            ProbeResult result2 = ProbeRunner.Run(options, mock2, GoodNative());
            Check("secrecy: failure report never contains the password",
                !result2.Render().Contains("987654"));

            // exception path (message must never embed the password in reality;
            // the runner itself must not add it anywhere either)
            MockDeviceChannel mock3 = new MockDeviceChannel();
            mock3.ProductException = new Exception("boom");
            ProbeResult result3 = ProbeRunner.Run(options, mock3, GoodNative());
            Check("secrecy: exception report never contains the password",
                !result3.Render().Contains("987654"));
        }

        private static void TestOptionsToStringNeverContainsPassword()
        {
            ProbeOptions o = new ProbeOptions();
            o.NetworkPassword = 424242;
            string s = o.ToString();
            Check("secrecy: options ToString hides the password",
                !s.Contains("424242") && s.Contains("<hidden>"));
        }
    }
}
