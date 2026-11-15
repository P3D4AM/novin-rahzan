using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NP756P.ReadOnlyProbe;

namespace NP756P.ReadOnlyProbe.Tests
{
    /// <summary>
    /// Device-free test runner. No native vendor DLL is loaded and no socket is
    /// opened anywhere in this suite: SDK behavior is simulated via MockDeviceChannel
    /// and the PE validator is tested with synthetic PE byte images plus (when the
    /// repository checkout is reachable) the two real reference DLLs
    /// FK623Attend.dll / FKAttend.dll - parsed only, never executed.
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

        private static void Skip(string name, string reason)
        {
            Console.WriteLine("SKIP  " + name + " (" + reason + ")");
        }

        private static int Main()
        {
            Console.WriteLine("NP756P.ReadOnlyProbe device-free test suite");
            Console.WriteLine("------------------------------------------------");

            TestArgumentParsing();
            TestValidationRules();
            TestPasswordRules();
            TestPeParser();
            TestPeParserOrdinalImports();
            TestPeParserRobustness();
            TestValidatorOnMissingDir();
            TestValidatorOnMissingDll();
            TestValidatorOnWrongArch();
            TestValidatorOnNotPeFile();
            TestValidatorOnTruncatedDll();
            TestValidatorOnRvaOutOfRange();
            TestValidatorOnGoodFakeChain();
            TestValidatorImportOrdinals();
            TestValidatorOnLockedFile();
            TestRunnerHappyPath();
            TestRunnerConnectFailure();
            TestRunnerConnectException();
            TestRunnerProductFailureSkipsRest();
            TestRunnerDeviceInfoFailure();
            TestRunnerDeviceTimeFailure();
            TestRunnerReadExceptions();
            TestRunnerDisconnectException();
            TestRunnerCleanupOnException();
            TestRunnerDisconnectOnlyWithValidHandle();
            TestMockIndexContract();
            TestMockOptionsForwarding();
            TestErrorTextMapping();
            TestExitCodes();
            TestExceptionTextSafety();
            TestPInvokeSurface();
            TestReportNeverContainsPassword();
            TestOptionsToStringNeverContainsPassword();
            TestRealVendorDlls();

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

        // ---------------- synthetic PE images ----------------

        /// <summary>Import description for the synthetic PE builder.</summary>
        private sealed class SyntheticImport
        {
            public string DllName;
            public string[] ByName = new string[0];
            public int[] ByOrdinal = new int[0];
        }

        /// <summary>
        /// Builds a structurally valid PE32 image with the given machine type,
        /// export names and imports (named and/or ordinal-based, each with a real
        /// OriginalFirstThunk/FirstThunk pair and hint/name entries).
        /// The file is padded to the full declared raw size so every table stays
        /// inside the file and the fail-closed parser accepts it.
        /// </summary>
        private static byte[] BuildSyntheticPe(ushort machine, string[] exportNames, SyntheticImport[] imports)
        {
            const int peOff = 0x80;
            const int optSize = 224;                 // PE32 optional header + 16 data dirs
            const int secOff = peOff + 24 + optSize;
            const int rawSec = 0x400;                // raw offset of the single section
            const int vaSec = 0x1000;                // RVA of the single section
            const int secSpan = 0x1000;              // covers RVA 0x1000..0x2000
            int fileSize = rawSec + secSpan;         // 0x1400 bytes on disk

            int expDirRva = vaSec;                   // 0x1000
            int funcsRva = vaSec + 0x40;             // 0x1040
            int namesRva = vaSec + 0x200;            // 0x1200
            int ordsRva = vaSec + 0x300;             // 0x1300
            int expStringsRva = vaSec + 0x380;       // 0x1380
            int dllNameRva = vaSec + 0x3F0;          // 0x13F0 (export strings live below it)
            int impDescRva = vaSec + 0x400;         // 0x1400
            int impDllNamesRva = vaSec + 0x500;      // 0x1500
            int thunkBaseRva = vaSec + 0x600;        // 0x1600 (per DLL: + i * 0x80)
            int hintNamesRva = vaSec + 0x700;        // 0x1700

            if (exportNames.Length > 64) { throw new InvalidOperationException("synthetic PE: too many exports"); }
            if (imports != null && imports.Length > 8) { throw new InvalidOperationException("synthetic PE: too many import DLLs"); }

            MemoryStream ms = new MemoryStream();
            BinaryWriter w = new BinaryWriter(ms);

            // DOS header
            w.Write((ushort)0x5A4D);
            ms.Position = 0x3C; w.Write((int)peOff);

            // NT headers
            ms.Position = peOff;
            w.Write(new byte[] { (byte)'P', (byte)'E', 0, 0 });
            w.Write(machine);
            w.Write((ushort)1);                    // one section
            w.Write((int)0);                        // timestamp
            w.Write((int)0);                        // symbol table
            w.Write((int)0);                        // symbols
            w.Write((ushort)optSize);
            w.Write((ushort)0x102);                 // characteristics

            // optional header (PE32), only the fields the parser needs are exact
            int opt = peOff + 24;
            ms.Position = opt;
            w.Write((ushort)0x10B);                 // magic PE32
            for (int i = 2; i < 88; i += 4) { ms.Position = opt + i; w.Write((int)0); }
            ms.Position = opt + 92; w.Write((int)16);  // NumberOfRvaAndSizes = 16
            ms.Position = opt + 96;                 // data directories ([0]=export, [1]=import)
            w.Write(expDirRva); w.Write((int)0x200);
            w.Write(imports != null && imports.Length > 0 ? impDescRva : 0);
            w.Write((int)0);

            // section header
            ms.Position = secOff;
            byte[] secName = new byte[8];
            byte[] name = Encoding.ASCII.GetBytes(".rdata");
            Array.Copy(name, secName, name.Length);
            w.Write(secName);
            w.Write((int)secSpan);                  // virtual size
            w.Write((int)vaSec);                    // virtual address
            w.Write((int)secSpan);                  // raw size
            w.Write((int)rawSec);                   // raw pointer
            w.Write((int)0); w.Write((int)0); w.Write((ushort)0); w.Write((ushort)0);
            w.Write((int)0x40000040);

            int Raw(int rva) { return rawSec + (rva - vaSec); }

            // export directory
            ms.Position = Raw(expDirRva);
            w.Write((int)0); w.Write((int)0); w.Write((ushort)0); w.Write((ushort)0);
            w.Write(dllNameRva);
            w.Write(1);                              // ordinal base = 1
            w.Write(exportNames.Length);             // NumberOfFunctions
            w.Write(exportNames.Length);             // NumberOfNames
            w.Write(funcsRva);
            w.Write(namesRva);
            w.Write(ordsRva);

            // export address table (all non-null)
            ms.Position = Raw(funcsRva);
            for (int i = 0; i < exportNames.Length; i++) { w.Write(vaSec + 0x800 + i); }

            // export name pointers + string storage
            int nextString = expStringsRva;
            List<int> nameRvas = new List<int>();
            foreach (string n in exportNames)
            {
                nameRvas.Add(nextString);
                nextString += n.Length + 1;
            }
            ms.Position = Raw(namesRva);
            foreach (int r in nameRvas) { w.Write(r); }

            ms.Position = Raw(ordsRva);
            for (int i = 0; i < exportNames.Length; i++) { w.Write((ushort)i); }

            ms.Position = Raw(expStringsRva);
            foreach (string n in exportNames) { w.Write(Encoding.ASCII.GetBytes(n)); w.Write((byte)0); }
            if (nextString > dllNameRva)
            {
                throw new InvalidOperationException("synthetic PE: export name strings overflow into the dll-name slot");
            }

            ms.Position = Raw(dllNameRva);
            w.Write(Encoding.ASCII.GetBytes("synth.dll")); w.Write((byte)0);

            // import descriptors + thunk arrays + hint/name entries
            if (imports != null && imports.Length > 0)
            {
                int dllNameCursor = impDllNamesRva;
                int hintCursor = hintNamesRva;
                for (int i = 0; i < imports.Length; i++)
                {
                    SyntheticImport imp = imports[i];
                    int thunkRva = thunkBaseRva + i * 0x80;
                    int nEntries = imp.ByOrdinal.Length + imp.ByName.Length;

                    // descriptor: OriginalFirstThunk, timestamp, forwarder, name, FirstThunk
                    ms.Position = Raw(impDescRva) + i * 20;
                    w.Write(thunkRva);
                    w.Write((int)0);
                    w.Write((int)0);
                    w.Write(dllNameCursor);
                    w.Write(thunkRva);

                    // DLL name string
                    ms.Position = Raw(dllNameCursor);
                    w.Write(Encoding.ASCII.GetBytes(imp.DllName)); w.Write((byte)0);
                    dllNameCursor += imp.DllName.Length + 1;

                    // thunks: ordinals flagged with 0x80000000, names as hint/name RVAs
                    ms.Position = Raw(thunkRva);
                    foreach (int ord in imp.ByOrdinal) { w.Write((int)(0x80000000 | (uint)ord)); }
                    foreach (string fn in imp.ByName)
                    {
                        w.Write(hintCursor);
                        // hint/name entry
                        long back = ms.Position;
                        ms.Position = Raw(hintCursor);
                        w.Write((ushort)0);                             // hint
                        w.Write(Encoding.ASCII.GetBytes(fn)); w.Write((byte)0);
                        hintCursor += 2 + fn.Length + 1;
                        ms.Position = back;
                    }
                    w.Write((int)0);                                     // thunk terminator
                }

                // all-zero terminator descriptor (full 20 bytes)
                ms.Position = Raw(impDescRva) + imports.Length * 20;
                for (int k = 0; k < 5; k++) { w.Write((int)0); }
            }

            // pad the image to the declared raw size
            if (ms.Length < fileSize)
            {
                ms.SetLength(fileSize);
            }
            byte[] buf = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buf, 0, buf.Length);
            return buf;
        }

        /// <summary>Compatibility shape: imports by DLL name only, no functions.</summary>
        private static byte[] BuildSyntheticPe(ushort machine, string[] exportNames, string[] importDlls)
        {
            SyntheticImport[] imports = null;
            if (importDlls != null && importDlls.Length > 0)
            {
                imports = new SyntheticImport[importDlls.Length];
                for (int i = 0; i < importDlls.Length; i++)
                {
                    imports[i] = new SyntheticImport { DllName = importDlls[i] };
                }
            }
            return BuildSyntheticPe(machine, exportNames, imports);
        }

        // ---------------- PE parser (synthetic images) ----------------

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

        private static void TestPeParserOrdinalImports()
        {
            string err;
            NativeLibraryValidator.PeInfo info;

            SyntheticImport[] imports =
            {
                new SyntheticImport { DllName = "FKAttend.dll", ByName = new[] { "FK_GetLastError" }, ByOrdinal = new[] { 1, 2, 3, 16, 161 } },
                new SyntheticImport { DllName = "KERNEL32.dll", ByName = new[] { "ExitProcess" } }
            };
            byte[] pe = BuildSyntheticPe(0x014C,
                new[] { "FK_ConnectNet", "FK_GetProductData", "FK_GetDeviceInfo", "FK_GetDeviceTime", "FK_DisConnect", "FK_GetLastError" },
                imports);

            Check("pe: ordinal and named imports parsed",
                NativeLibraryValidator.TryReadPeInfo(pe, out info, out err) &&
                info.ImportDllNames.Count == 2);

            if (info == null)
            {
                Console.WriteLine("      (synthetic ordinal PE did not parse: " + err + ")");
                return;
            }

            NativeLibraryValidator.ImportDllInfo fromFkAttend = null;
            foreach (NativeLibraryValidator.ImportDllInfo d in info.Imports)
            {
                if (string.Equals(d.DllName, "FKAttend.dll", StringComparison.OrdinalIgnoreCase)) { fromFkAttend = d; }
            }

            int ordinalCount = 0;
            int namedCount = 0;
            if (fromFkAttend != null)
            {
                foreach (NativeLibraryValidator.ImportedFunction f in fromFkAttend.Functions)
                {
                    if (f.ByOrdinal) { ordinalCount++; } else { namedCount++; }
                }
            }

            Check("pe: five ordinal imports from FKAttend extracted",
                fromFkAttend != null && ordinalCount == 5 &&
                fromFkAttend.Functions.Exists(delegate(NativeLibraryValidator.ImportedFunction f) { return f.ByOrdinal && f.Ordinal == 161; }));

            Check("pe: named import FK_GetLastError extracted",
                namedCount == 1 && fromFkAttend.Functions.Exists(delegate(NativeLibraryValidator.ImportedFunction f) { return !f.ByOrdinal && f.Name == "FK_GetLastError"; }));

            Check("pe: export ordinals occupied by base+index",
                info.ExportOrdinalBase == 1 && info.OrdinalExported(1) && info.OrdinalExported(6) &&
                !info.OrdinalExported(7) && !info.OrdinalExported(0));

            // OriginalFirstThunk must be preferred: rebuilding with an import whose
            // FirstThunk differs is checked implicitly by the validator tests below.
        }

        private static void TestPeParserRobustness()
        {
            string err = null;
            NativeLibraryValidator.PeInfo info;
            byte[] good = BuildSyntheticPe(0x014C,
                new[] { "FK_ConnectNet", "FK_GetProductData" },
                new[] { new SyntheticImport { DllName = "FKAttend.dll", ByOrdinal = new[] { 1 } } });

            // 1) truncated file: must report a parse error, never throw
            byte[] truncated = new byte[good.Length / 2];
            Array.Copy(good, truncated, truncated.Length);
            bool threw = false;
            bool ok = false;
            try
            {
                ok = NativeLibraryValidator.TryReadPeInfo(truncated, out info, out err);
            }
            catch (Exception)
            {
                threw = true;
            }
            Check("pe: truncated file reports parse error without throwing",
                !threw && !ok && err != null && err.StartsWith("PARSE-ERROR", StringComparison.Ordinal));

            // 2) RVA pointing outside every section (export name-pointer table)
            byte[] badNames = (byte[])good.Clone();
            int optOff = 0x80 + 24;                       // optional header
            int dd = optOff + 96;                         // PE32 data directories
            int expRva = BitConverter.ToInt32(badNames, dd);
            // raw offset of the export directory = rawSec(0x400) + (rva-0x1000)
            int expRaw = 0x400 + (expRva - 0x1000);
            badNames[expRaw + 32] = 0x00; badNames[expRaw + 33] = 0x90;  // namesRva = 0x9000
            badNames[expRaw + 34] = 0x00; badNames[expRaw + 35] = 0x00;
            threw = false; ok = true;
            try { ok = NativeLibraryValidator.TryReadPeInfo(badNames, out info, out err); }
            catch (Exception) { threw = true; }
            Check("pe: RVA outside sections is a parse error, not a crash",
                !threw && !ok && err != null && err.Contains("not mapped"));

            // 3) truncated import descriptor table (terminator removed by shortening the file)
            byte[] shortImp = BuildSyntheticPe(0x014C, new[] { "A" },
                new[] { new SyntheticImport { DllName = "FKAttend.dll", ByName = new[] { "X" } } });
            // file length = 0x1400; descriptor at raw 0x500; keep 0x520 bytes so the
            // descriptor body exists but the terminator descriptor is cut off.
            byte[] cut = new byte[0x400 + 0x120];
            Array.Copy(shortImp, cut, cut.Length);
            threw = false; ok = true;
            try { ok = NativeLibraryValidator.TryReadPeInfo(cut, out info, out err); }
            catch (Exception) { threw = true; }
            Check("pe: truncated import descriptor table is a parse error",
                !threw && !ok && err != null && err.Contains("truncated"));

            // 4) empty thunk array tolerated (zero functions from that DLL)
            byte[] emptyThunk = BuildSyntheticPe(0x014C, new[] { "A" }, new[] { "FKAttend.dll" });
            Check("pe: import with zero functions tolerated (name still recorded)",
                NativeLibraryValidator.TryReadPeInfo(emptyThunk, out info, out err) &&
                info.ImportDllNames.Contains("FKAttend.dll") &&
                info.Imports[0].Functions.Count == 0);
        }

        // ---------------- validator behavior ----------------

        private static string TempDir()
        {
            string d = Path.Combine(Path.GetTempPath(), "np756p_test_" + Guid.NewGuid().ToString("N").Substring(0, 8));
            Directory.CreateDirectory(d);
            return d;
        }

        private static SyntheticImport[] ChainImports(bool breakOrdinal)
        {
            // FK623Attend.dll -> FKAttend.dll: 1 named + 4 ordinal (ordinal 9 is
            // deliberately missing from the FKAttend export table when breakOrdinal).
            return new SyntheticImport[]
            {
                new SyntheticImport
                {
                    DllName = "FKAttend.dll",
                    ByName = new[] { "FK_GetLastError" },
                    ByOrdinal = breakOrdinal ? new[] { 1, 2, 3, 9 } : new[] { 1, 2, 3, 4 }
                }
            };
        }

        /// <summary>Writes the seven required DLLs; FK623Attend imports FKAttend
        /// (named + ordinal), FKAttend imports FKViaDev by ordinal and the rest by name.</summary>
        private static void WriteChain(
            string dir, bool breakOrdinal, string[] fk623Exports, string[] fkAttendExports)
        {
            // FK623Attend.dll
            File.WriteAllBytes(Path.Combine(dir, "FK623Attend.dll"),
                BuildSyntheticPe(0x014C, fk623Exports, ChainImports(breakOrdinal)));

            // FKAttend.dll imports FKPwdEncDec by ordinal (1,2) and FKViaDev by name
            SyntheticImport[] fkAttendImports =
            {
                new SyntheticImport { DllName = "FKPwdEncDec.dll", ByOrdinal = new[] { 1, 2 } },
                new SyntheticImport { DllName = "FKViaDev.dll", ByName = new[] { "?FV_ConnectNet@CFKViaDev@@QAEJJPADJJJ@Z" } }
            };
            File.WriteAllBytes(Path.Combine(dir, "FKAttend.dll"),
                BuildSyntheticPe(0x014C, fkAttendExports, fkAttendImports));

            File.WriteAllBytes(Path.Combine(dir, "FKViaDev.dll"),
                BuildSyntheticPe(0x014C, new[] { "?FV_ConnectNet@CFKViaDev@@QAEJJPADJJJ@Z" }, (SyntheticImport[])null));
            File.WriteAllBytes(Path.Combine(dir, "LFWViaDev.dll"),
                BuildSyntheticPe(0x014C, new[] { "FV_Sample_L" }, (SyntheticImport[])null));
            File.WriteAllBytes(Path.Combine(dir, "FKPwdEncDec.dll"),
                BuildSyntheticPe(0x014C, new[] { "P1", "P2" }, (SyntheticImport[])null));
            File.WriteAllBytes(Path.Combine(dir, "FpDataConv.dll"),
                BuildSyntheticPe(0x014C, new[] { "D1" }, (SyntheticImport[])null));
            File.WriteAllBytes(Path.Combine(dir, "FaceDataConv.dll"),
                BuildSyntheticPe(0x014C, new[] { "F1" }, (SyntheticImport[])null));
        }

        private static string[] DefaultExports()
        {
            return NativeLibraryValidator.AllowedFunctions;
        }

        private static string[] SmallVendorExports()
        {
            // FKAttend-like export table: only 4 named exports (ordinals 1..4)
            return new[] { "FK_Attend_A", "FK_Attend_B", "FK_Attend_C", "FK_GetLastError" };
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

        private static void TestValidatorOnNotPeFile()
        {
            // all seven files present, one of them exists but is plain text
            string dir = TempDir();
            WriteChain(dir, false, DefaultExports(), SmallVendorExports());
            string garbage = new string('X', 600);
            File.WriteAllText(Path.Combine(dir, "FKAttend.dll"), garbage);

            NativeLibraryValidator.ValidationReport r = NativeLibraryValidator.Validate(dir);
            NativeLibraryValidator.DllEntry fkAttend = null;
            foreach (NativeLibraryValidator.DllEntry e in r.Entries)
            {
                if (e.FileName == "FKAttend.dll") { fkAttend = e; }
            }
            Check("validator: existing non-PE file fails validation as NOT-A-PE",
                !r.Succeeded && fkAttend != null && fkAttend.Status == "NOT-A-PE" &&
                r.FailureSummary.Contains("FKAttend.dll") && r.FailureSummary.Contains("NOT-A-PE"));
        }

        private static void TestValidatorOnTruncatedDll()
        {
            string dir = TempDir();
            WriteChain(dir, false, DefaultExports(), SmallVendorExports());
            // truncate FK623Attend.dll (the DLL that actually has an import table)
            // so its thunk arrays no longer fit inside the file
            string target = Path.Combine(dir, "FK623Attend.dll");
            byte[] good = File.ReadAllBytes(target);
            byte[] broken = new byte[good.Length / 2];
            Array.Copy(good, broken, broken.Length);
            File.WriteAllBytes(target, broken);

            NativeLibraryValidator.ValidationReport r = NativeLibraryValidator.Validate(dir);
            NativeLibraryValidator.DllEntry fk623 = null;
            foreach (NativeLibraryValidator.DllEntry e in r.Entries)
            {
                if (e.FileName == "FK623Attend.dll") { fk623 = e; }
            }
            Check("validator: truncated DLL fails validation as PARSE-ERROR",
                !r.Succeeded && fk623 != null && fk623.Status == "PARSE-ERROR" &&
                !string.IsNullOrEmpty(fk623.StatusDetail));
        }

        private static void TestValidatorOnRvaOutOfRange()
        {
            string dir = TempDir();
            WriteChain(dir, false, DefaultExports(), SmallVendorExports());
            // patch FKViaDev.dll: export name-pointer table RVA -> 0x9000 (outside every section)
            string target = Path.Combine(dir, "FKViaDev.dll");
            byte[] bytes = File.ReadAllBytes(target);
            int optOff = 0x80 + 24;
            int dd = optOff + 96;
            int expRva = BitConverter.ToInt32(bytes, dd);
            int expRaw = 0x400 + (expRva - 0x1000);
            bytes[expRaw + 32] = 0x00; bytes[expRaw + 33] = 0x90;
            bytes[expRaw + 34] = 0x00; bytes[expRaw + 35] = 0x00;
            File.WriteAllBytes(target, bytes);

            NativeLibraryValidator.ValidationReport r = NativeLibraryValidator.Validate(dir);
            NativeLibraryValidator.DllEntry viaDev = null;
            foreach (NativeLibraryValidator.DllEntry e in r.Entries)
            {
                if (e.FileName == "FKViaDev.dll") { viaDev = e; }
            }
            Check("validator: RVA outside the file is a PARSE-ERROR failure",
                !r.Succeeded && viaDev != null && viaDev.Status == "PARSE-ERROR" &&
                viaDev.StatusDetail.Contains("not mapped"));
        }

        private static void TestValidatorOnGoodFakeChain()
        {
            string dir = TempDir();
            NativeLibraryValidator.ValidationReport r = ValidateChain(dir, 0x014C, false, false);
            Check("validator: complete x86 chain with all allowed exports succeeds",
                r.Succeeded && r.Entries.Count == 7 && r.MissingAllowedFunctions.Count == 0 &&
                r.ImportProblems.Count == 0);

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

        private static void TestValidatorImportOrdinals()
        {
            // ---- healthy ordinal chain: every required ordinal exists in the target ----
            string dir = TempDir();
            WriteChain(dir, false, DefaultExports(), SmallVendorExports());
            NativeLibraryValidator.ValidationReport good = NativeLibraryValidator.Validate(dir);
            Check("validator: resolvable ordinal imports pass validation",
                good.Succeeded && good.ImportProblems.Count == 0);

            NativeLibraryValidator.DllEntry fk623 = null;
            foreach (NativeLibraryValidator.DllEntry e in good.Entries)
            {
                if (e.FileName == "FK623Attend.dll") { fk623 = e; }
            }
            Check("validator: ordinal import counts reported per DLL",
                fk623 != null && fk623.TotalOrdinalImports == 4 && fk623.TotalNamedImports == 1);

            // ---- broken ordinal chain: ordinal 9 is not exported by FKAttend ----
            string dir2 = TempDir();
            WriteChain(dir2, true, DefaultExports(), SmallVendorExports());
            NativeLibraryValidator.ValidationReport bad = NativeLibraryValidator.Validate(dir2);
            Check("validator: missing import ordinal fails validation",
                !bad.Succeeded && bad.ImportProblems.Count == 1);

            if (bad.ImportProblems.Count == 1)
            {
                NativeLibraryValidator.ImportProblem p = bad.ImportProblems[0];
                Check("validator: missing ordinal reports source, target and ordinal",
                    p.SourceDll == "FK623Attend.dll" && p.TargetDll == "FKAttend.dll" &&
                    p.Function != null && p.Function.ByOrdinal && p.Function.Ordinal == 9 &&
                    bad.FailureSummary.Contains("FK623Attend.dll") &&
                    bad.FailureSummary.Contains("FKAttend.dll") &&
                    bad.FailureSummary.Contains("ordinal 9"));
            }
            else
            {
                Check("validator: missing ordinal reports source, target and ordinal", false);
            }

            // ---- missing NAMED import ----
            string dir3 = TempDir();
            WriteChain(dir3, false, DefaultExports(), SmallVendorExports());
            // rebuild FK623Attend with a named import that FKAttend does not export
            File.WriteAllBytes(Path.Combine(dir3, "FK623Attend.dll"),
                BuildSyntheticPe(0x014C, DefaultExports(),
                    new SyntheticImport[] { new SyntheticImport { DllName = "FKAttend.dll", ByName = new[] { "FK_NotThere" } } }));
            NativeLibraryValidator.ValidationReport badNamed = NativeLibraryValidator.Validate(dir3);
            Check("validator: missing named import fails validation",
                !badNamed.Succeeded && badNamed.ImportProblems.Count == 1 &&
                badNamed.FailureSummary.Contains("FK_NotThere") &&
                badNamed.FailureSummary.Contains("FKAttend.dll"));

            // ---- import target present but corrupt ----
            string dir4 = TempDir();
            WriteChain(dir4, false, DefaultExports(), SmallVendorExports());
            File.WriteAllText(Path.Combine(dir4, "FKPwdEncDec.dll"), new string('Z', 600));
            NativeLibraryValidator.ValidationReport badTarget = NativeLibraryValidator.Validate(dir4);
            Check("validator: present-but-corrupt import target is reported",
                !badTarget.Succeeded && badTarget.FailureSummary.Contains("FKPwdEncDec.dll"));
        }

        private static void TestValidatorOnLockedFile()
        {
            // IO error while reading: hold the file with FileShare.None so
            // File.ReadAllBytes must fail -> the entry must end IO-ERROR and the
            // validation must FAIL (never an implicit success).
            string dir = TempDir();
            ValidateChain(dir, 0x014C, false, false);   // write all seven
            string target = Path.Combine(dir, "FKPwdEncDec.dll");
            FileStream lockStream = null;
            NativeLibraryValidator.ValidationReport r;
            try
            {
                lockStream = new FileStream(target, FileMode.Open, FileAccess.Read, FileShare.None);
                r = NativeLibraryValidator.Validate(dir);
            }
            finally
            {
                if (lockStream != null) { lockStream.Dispose(); }
            }
            NativeLibraryValidator.DllEntry entry = null;
            foreach (NativeLibraryValidator.DllEntry e in r.Entries)
            {
                if (e.FileName == "FKPwdEncDec.dll") { entry = e; }
            }
            Check("validator: unreadable file fails validation (IO-ERROR)",
                !r.Succeeded && entry != null && entry.Status == "IO-ERROR");
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
                mock.DisconnectCalls == 1 && result.DisconnectCalled && result.DisconnectNeeded &&
                !result.DisconnectFailed);

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
                mock.DisconnectCalls == 0 && result.DisconnectCalled == false &&
                result.DisconnectNeeded == false);

            Check("runner: failure block contains FK_ConnectNet",
                result.FirstFailure != null && result.FirstFailure.Function == "FK_ConnectNet" &&
                result.FirstFailure.HasRawCode && result.FirstFailure.RawCode == -2);

            Check("runner: connect raw code rendered with vendor meaning",
                result.Render().Contains("SDK returned -2") &&
                result.Render().Contains("No Open Comm"));

            // zero return value: raw code is recorded as zero (not an exception)
            MockDeviceChannel mockZero = new MockDeviceChannel();
            mockZero.ConnectResult = 0;
            ProbeResult zero = ProbeRunner.Run(new ProbeOptions(), mockZero, GoodNative());
            Check("runner: connect returning zero records raw code 0 and exit 3",
                !zero.ConnectSucceeded && zero.ExitCode == 3 &&
                zero.FirstFailure != null && zero.FirstFailure.HasRawCode &&
                zero.FirstFailure.RawCode == 0);
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

            Check("runner: connect exception records NO raw code",
                result.FirstFailure != null && !result.FirstFailure.HasRawCode &&
                !result.ConnectHasRawCode);

            Check("runner: connect exception render avoids fake 'No support'",
                !result.Render().Contains("No support") &&
                result.Render().Contains("no SDK return value"));

            Check("runner: connect exception yields exit code 3 and skipped reads",
                result.ExitCode == 3 && mock.ProductCalls == 0 &&
                result.ProductStage == StageStatus.Skipped &&
                result.DeviceInfoStage == StageStatus.Skipped &&
                result.DeviceTimeStage == StageStatus.Skipped);

            Check("runner: no disconnect attempt after connect exception",
                mock.DisconnectCalls == 0 && result.DisconnectCalled == false);
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

        private static void TestRunnerDeviceInfoFailure()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.DeviceInfoStatus = -5;
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: device info failure fails only that stage",
                result.ProductStage == StageStatus.Ok &&
                result.DeviceInfoStage == StageStatus.Failed &&
                result.DeviceTimeStage == StageStatus.Skipped);

            Check("runner: device info failure records function and codes",
                result.FirstFailure != null &&
                result.FirstFailure.Function == "FK_GetDeviceInfo(2)" &&
                result.FirstFailure.HasRawCode && result.FirstFailure.RawCode == -5 &&
                result.FirstFailure.HasLastError && result.FirstFailure.LastError == -4);

            Check("runner: device info failure exit code 4 and disconnect executed",
                result.ExitCode == 4 && mock.DisconnectCalls == 1 &&
                !result.HasDeviceMachineNumber && !result.HasDeviceLanguage);
        }

        private static void TestRunnerDeviceTimeFailure()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.DeviceTimeStatus = -8;
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: device time failure fails only the time stage",
                result.ProductStage == StageStatus.Ok &&
                result.DeviceInfoStage == StageStatus.Ok &&
                result.DeviceTimeStage == StageStatus.Failed);

            Check("runner: device time failure reported with raw code",
                result.FirstFailure != null &&
                result.FirstFailure.Function == "FK_GetDeviceTime" &&
                result.FirstFailure.HasRawCode && result.FirstFailure.RawCode == -8);

            Check("runner: device time failure exit 4, no time value recorded",
                result.ExitCode == 4 && !result.HasDeviceTime && mock.DisconnectCalls == 1);
        }

        private static void TestRunnerReadExceptions()
        {
            // exception during FK_GetProductData
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.ProductException = new InvalidOperationException("native call blew up");
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: product exception names the exact SDK function",
                result.FirstFailure != null &&
                result.FirstFailure.Function == "FK_GetProductData(1)" &&
                result.FirstFailure.ExceptionType == "InvalidOperationException");

            Check("runner: product exception records no fake raw/last-error codes",
                result.FirstFailure != null && !result.FirstFailure.HasRawCode &&
                !result.FirstFailure.HasLastError);

            Check("runner: product exception fails product stage and skips the rest",
                result.ProductStage == StageStatus.Failed &&
                result.DeviceInfoStage == StageStatus.Skipped &&
                result.DeviceTimeStage == StageStatus.Skipped &&
                result.ExitCode == 4);

            // exception during FK_GetDeviceInfo
            MockDeviceChannel mockInfo = new MockDeviceChannel();
            mockInfo.DeviceInfoException = new AccessViolationException("bad memory");
            ProbeResult infoResult = ProbeRunner.Run(new ProbeOptions(), mockInfo, GoodNative());
            Check("runner: device info exception names FK_GetDeviceInfo(2)",
                infoResult.FirstFailure != null &&
                infoResult.FirstFailure.Function == "FK_GetDeviceInfo(2)" &&
                infoResult.FirstFailure.ExceptionType == "AccessViolationException" &&
                infoResult.ProductStage == StageStatus.Ok &&
                infoResult.DeviceInfoStage == StageStatus.Failed &&
                infoResult.DeviceTimeStage == StageStatus.Skipped);

            // exception during FK_GetDeviceTime
            MockDeviceChannel mockTime = new MockDeviceChannel();
            mockTime.DeviceTimeException = new InvalidOperationException("time read exploded");
            ProbeResult timeResult = ProbeRunner.Run(new ProbeOptions(), mockTime, GoodNative());
            Check("runner: device time exception names FK_GetDeviceTime",
                timeResult.FirstFailure != null &&
                timeResult.FirstFailure.Function == "FK_GetDeviceTime" &&
                !timeResult.FirstFailure.HasRawCode && !timeResult.FirstFailure.HasLastError &&
                timeResult.ProductStage == StageStatus.Ok &&
                timeResult.DeviceInfoStage == StageStatus.Ok &&
                timeResult.DeviceTimeStage == StageStatus.Failed &&
                timeResult.ExitCode == 4);
        }

        private static void TestRunnerDisconnectException()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            mock.DisconnectException = new InvalidOperationException("disconnect blew up");
            ProbeResult result = ProbeRunner.Run(new ProbeOptions(), mock, GoodNative());

            Check("runner: disconnect exception is recorded as DISCONNECT FAILED",
                result.DisconnectNeeded && result.DisconnectCalled && result.DisconnectFailed &&
                result.DisconnectExceptionType == "InvalidOperationException" &&
                result.FirstFailure != null && result.FirstFailure.Stage == "[7] Disconnect");

            Check("runner: disconnect failure prevents exit code 0",
                result.ExitCode == 4);

            Check("runner: disconnect failure never shown as NOT NEEDED",
                result.Render().Contains("DISCONNECT FAILED") &&
                !result.Render().Contains("NOT NEEDED"));
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
                mock.DisconnectCalls == 0 && result.DisconnectCalled == false &&
                result.DisconnectNeeded == false);
        }

        // ---------------- mock contract ----------------

        private static void TestMockIndexContract()
        {
            MockDeviceChannel mock = new MockDeviceChannel();

            string v;
            int bad = mock.GetProductData(2, out v);
            int bad2;
            int badInfo = mock.GetDeviceInfo(5, out bad2);

            Check("mock: GetProductData rejects indices other than 1 and 3",
                bad == -5);

            Check("mock: GetDeviceInfo rejects indices other than 2 and 3",
                badInfo == -5);

            // the runner itself must only ever ask for the allowed indices
            MockDeviceChannel runnerMock = new MockDeviceChannel();
            ProbeRunner.Run(new ProbeOptions(), runnerMock, GoodNative());
            bool indicesOk = runnerMock.ReceivedProductIndices.Count == 2 &&
                runnerMock.ReceivedProductIndices[0] == 1 &&
                runnerMock.ReceivedProductIndices[1] == 3 &&
                runnerMock.ReceivedDeviceInfoIndices.Count == 2 &&
                runnerMock.ReceivedDeviceInfoIndices[0] == 2 &&
                runnerMock.ReceivedDeviceInfoIndices[1] == 3;
            Check("mock: runner only requests product 1/3 and device info 2/3",
                indicesOk);
        }

        private static void TestMockOptionsForwarding()
        {
            MockDeviceChannel mock = new MockDeviceChannel();
            ProbeOptions options;
            List<string> errs;
            ProbeOptions.TryParse(
                new[] { "--ip", "10.20.30.40", "--port", "4310", "--machine", "9",
                        "--timeout", "8000", "--protocol", "udp", "--license", "77" },
                out options, out errs);
            options.NetworkPassword = 987654;
            ProbeRunner.Run(options, mock, GoodNative());

            Check("mock: connection options forwarded to the SDK unchanged",
                mock.ReceivedIp == "10.20.30.40" && mock.ReceivedPort == 4310 &&
                mock.ReceivedMachine == 9 && mock.ReceivedTimeout == 8000 &&
                mock.ReceivedProtocol == 1 && mock.ReceivedLicense == 77);

            Check("mock: password forwarded to the SDK (never rendered)",
                mock.ReceivedPassword == 987654);

            MockDeviceChannel mockDefault = new MockDeviceChannel();
            ProbeOptions def = new ProbeOptions();
            def.NetworkPassword = 1234;
            ProbeRunner.Run(def, mockDefault, GoodNative());
            Check("mock: default options match the mission defaults",
                mockDefault.ReceivedIp == "192.168.1.75" &&
                mockDefault.ReceivedPort == 4238 &&
                mockDefault.ReceivedMachine == 1 &&
                mockDefault.ReceivedTimeout == 5000 &&
                mockDefault.ReceivedProtocol == 0 &&
                mockDefault.ReceivedLicense == 1261);
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

        // ---------------- exit codes and exception text ----------------

        private static void TestExitCodes()
        {
            Check("exitcodes: full contract is unique and stable",
                ExitCodes.Success == 0 && ExitCodes.InputError == 1 &&
                ExitCodes.NativeLibraryStageFailed == 2 && ExitCodes.ConnectFailed == 3 &&
                ExitCodes.ReadStageFailed == 4 && ExitCodes.UnexpectedException == 5);

            // stage A failure keeps exit 2
            NativeLibraryValidator.ValidationReport badNative = new NativeLibraryValidator.ValidationReport();
            badNative.Succeeded = false;
            badNative.FailureSummary = "broken";
            ProbeResult stageA = ProbeRunner.Run(new ProbeOptions(), new MockDeviceChannel(), badNative);
            Check("exitcodes: native stage failure returns 2 without any connect attempt",
                stageA.ExitCode == 2 && !stageA.ConnectionAttempted &&
                stageA.ProductStage == StageStatus.Skipped &&
                stageA.DeviceInfoStage == StageStatus.Skipped &&
                stageA.DeviceTimeStage == StageStatus.Skipped);
        }

        private static void TestExceptionTextSafety()
        {
            Exception inner = new InvalidOperationException("inner secret detail");
            Exception outer = new ArgumentException("outer message with spaces", "paramName", inner);

            string summary = ExceptionText.SafeSummary(outer);
            Check("exceptiontext: summary shows type and message only",
                summary.StartsWith("ArgumentException") && summary.Contains("outer message with spaces") &&
                !summary.Contains("inner secret detail") &&       // inner never leaks
                !summary.Contains("at ") &&                       // no stack frames
                !summary.Contains("InvalidOperationException"));  // no inner type either

            // multi-line message collapses
            Exception multiline = new Exception("line1\r\nline2\r\nline3");
            string flat = ExceptionText.SafeSummary(multiline);
            Check("exceptiontext: newlines are flattened",
                !flat.Contains("\r") && !flat.Contains("\n") && flat.Contains("line1"));

            // very long messages are truncated, never dumped
            string longText = new string('A', 5000);
            string longSummary = ExceptionText.SafeSummary(new Exception(longText));
            Check("exceptiontext: oversized messages are truncated",
                longSummary.Length <= 404 && longSummary.EndsWith("..."));

            Check("exceptiontext: null exception handled",
                ExceptionText.SafeSummary(null) == "<null exception>");
        }

        // ---------------- P/Invoke surface ----------------

        private static void TestPInvokeSurface()
        {
            // NativeMethods is internal but compiled into this test assembly, so
            // reflection can enumerate the DllImport declarations directly.
            MethodInfo[] methods = typeof(NativeMethods).GetMethods(BindingFlags.Public | BindingFlags.Static);
            List<string> importedNames = new List<string>();
            foreach (MethodInfo m in methods)
            {
                DllImportAttribute attr = null;
                foreach (object a in m.GetCustomAttributes(typeof(DllImportAttribute), false))
                {
                    attr = (DllImportAttribute)a;
                }
                if (attr != null)
                {
                    importedNames.Add(string.IsNullOrEmpty(attr.EntryPoint) ? m.Name : attr.EntryPoint);
                }
            }

            string[] allowed =
            {
                "FK_ConnectNet", "FK_GetProductData", "FK_GetDeviceInfo",
                "FK_GetDeviceTime", "FK_DisConnect", "FK_GetLastError"
            };

            Check("pinvoke: exactly the six read-only functions are declared",
                importedNames.Count == 6);
            bool same = importedNames.Count == allowed.Length;
            foreach (string fn in allowed)
            {
                if (!importedNames.Contains(fn)) { same = false; }
            }
            foreach (string fn in importedNames)
            {
                if (!new List<string>(allowed).Contains(fn)) { same = false; }
            }
            Check("pinvoke: the declared set equals the allowed set", same);

            // mutating SDK functions must never appear as a DllImport entry point
            string[] forbidden =
            {
                "FK_SetDeviceTime", "FK_SetDeviceInfo", "FK_EnableDevice",
                "FK_EmptyEnrollData", "FK_EmptyGeneralLogData", "FK_EmptySuperLogData",
                "FK_DeleteEnrollData", "FK_PutEnrollData", "FK_SetUserName",
                "FK_SetEnrollPhoto", "FK_SetServerNetInfo", "FK_SetRealTimeInfo",
                "FK_HS_SetTimeZone", "FK_HS_SetUserWeekPassTime"
            };
            bool clean = true;
            foreach (string f in forbidden)
            {
                if (importedNames.Contains(f)) { clean = false; }
            }
            Check("pinvoke: no mutating function is declared in any form", clean);

            // every declaration targets the vendor entry DLL only
            bool allTargetEntryDll = true;
            foreach (MethodInfo m in methods)
            {
                foreach (object a in m.GetCustomAttributes(typeof(DllImportAttribute), false))
                {
                    DllImportAttribute attr = (DllImportAttribute)a;
                    if (attr.Value != "FK623Attend.dll") { allTargetEntryDll = false; }
                }
            }
            Check("pinvoke: all declarations target FK623Attend.dll", allTargetEntryDll);
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

            // exception path
            MockDeviceChannel mock3 = new MockDeviceChannel();
            mock3.ProductException = new Exception("boom");
            ProbeResult result3 = ProbeRunner.Run(options, mock3, GoodNative());
            Check("secrecy: exception report never contains the password",
                !result3.Render().Contains("987654"));

            // connect exception path
            MockDeviceChannel mock4 = new MockDeviceChannel();
            mock4.ConnectException = new DllNotFoundException("FK623Attend.dll");
            ProbeResult result4 = ProbeRunner.Run(options, mock4, GoodNative());
            Check("secrecy: connect-exception report never contains the password",
                !result4.Render().Contains("987654"));

            // disconnect exception path
            MockDeviceChannel mock5 = new MockDeviceChannel();
            mock5.DisconnectException = new Exception("boom on close");
            ProbeResult result5 = ProbeRunner.Run(options, mock5, GoodNative());
            Check("secrecy: disconnect-exception report never contains the password",
                !result5.Render().Contains("987654"));

            // global-catch helper never includes a stack trace or inner exception
            Exception withInner = new Exception("outer", new Exception("987654 in inner"));
            string safe = ExceptionText.SafeSummary(withInner);
            Check("secrecy: unexpected-exception summary never contains the password",
                !safe.Contains("987654"));
        }

        private static void TestOptionsToStringNeverContainsPassword()
        {
            ProbeOptions o = new ProbeOptions();
            o.NetworkPassword = 424242;
            string s = o.ToString();
            Check("secrecy: options ToString hides the password",
                !s.Contains("424242") && s.Contains("<hidden>"));
        }

        // ---------------- real vendor DLLs (reference data, parse-only) ----------------

        /// <summary>Walks up from the test assembly to find the repo checkout.</summary>
        private static string FindRepoDllsDir()
        {
            string dir = AppContext.BaseDirectory;
            for (int i = 0; i < 12 && dir != null; i++)
            {
                string candidate = Path.Combine(dir, "NovinLibraryResearch", "decompiled-complete", "_dlls");
                if (Directory.Exists(candidate) &&
                    File.Exists(Path.Combine(candidate, "FK623Attend.dll")) &&
                    File.Exists(Path.Combine(candidate, "FKAttend.dll")))
                {
                    return candidate;
                }
                dir = Path.GetDirectoryName(dir);
            }
            return null;
        }

        private static void TestRealVendorDlls()
        {
            string dllsDir = FindRepoDllsDir();
            if (dllsDir == null)
            {
                Skip("realdata: repo _dlls directory not reachable from test output", "skipped");
                return;
            }

            NativeLibraryValidator.PeInfo fk623;
            NativeLibraryValidator.PeInfo fkAttend;
            string err1, err2;
            bool ok623 = NativeLibraryValidator.TryReadPeInfo(File.ReadAllBytes(Path.Combine(dllsDir, "FK623Attend.dll")), out fk623, out err1);
            bool okAttend = NativeLibraryValidator.TryReadPeInfo(File.ReadAllBytes(Path.Combine(dllsDir, "FKAttend.dll")), out fkAttend, out err2);

            Check("realdata: both vendor reference DLLs parse as PE",
                ok623 && okAttend);

            if (!ok623 || !okAttend)
            {
                Console.WriteLine("      (" + (err1 ?? err2) + ")");
                return;
            }

            Check("realdata: FK623Attend.dll and FKAttend.dll are x86",
                fk623.Machine == 0x014C && fkAttend.Machine == 0x014C);

            bool allAllowed = true;
            foreach (string fn in NativeLibraryValidator.AllowedFunctions)
            {
                if (!fk623.ExportNames.Contains(fn)) { allAllowed = false; }
            }
            Check("realdata: FK623Attend.dll exports all six allowed functions", allAllowed);

            NativeLibraryValidator.ImportDllInfo fromAttend = null;
            foreach (NativeLibraryValidator.ImportDllInfo d in fk623.Imports)
            {
                if (string.Equals(d.DllName, "FKAttend.dll", StringComparison.OrdinalIgnoreCase)) { fromAttend = d; }
            }
            int ords = 0;
            int names = 0;
            if (fromAttend != null)
            {
                foreach (NativeLibraryValidator.ImportedFunction f in fromAttend.Functions)
                {
                    if (f.ByOrdinal) { ords++; } else { names++; }
                }
            }

            Check("realdata: FK623Attend imports 1 named function (FK_GetLastError) from FKAttend",
                fromAttend != null && names == 1 &&
                fromAttend.Functions.Exists(delegate(NativeLibraryValidator.ImportedFunction f) { return !f.ByOrdinal && f.Name == "FK_GetLastError"; }));

            Check("realdata: FK623Attend imports 160 functions from FKAttend by ordinal",
                ords == 160);

            bool allSatisfied = true;
            int[] missing = new int[0];
            List<int> missList = new List<int>();
            if (fromAttend != null)
            {
                foreach (NativeLibraryValidator.ImportedFunction f in fromAttend.Functions)
                {
                    if (f.ByOrdinal && !fkAttend.OrdinalExported(f.Ordinal))
                    {
                        allSatisfied = false;
                        missList.Add(f.Ordinal);
                    }
                }
                missing = missList.ToArray();
            }
            Check("realdata: every ordinal import is satisfied by FKAttend exports",
                allSatisfied && missing.Length == 0);

            // end-to-end: the full validator over the real seven-DLL chain
            NativeLibraryValidator.ValidationReport report = NativeLibraryValidator.Validate(dllsDir);
            Check("realdata: full Validate() over the real 7-DLL chain succeeds",
                report.Succeeded && report.ImportProblems.Count == 0 &&
                report.MissingAllowedFunctions.Count == 0);

            if (!report.Succeeded)
            {
                Console.WriteLine("      (realdata failure: " + report.FailureSummary + ")");
            }
        }
    }
}
