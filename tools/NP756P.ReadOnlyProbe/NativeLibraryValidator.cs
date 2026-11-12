using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// Stage A: static validation of the vendor native libraries WITHOUT loading
    /// any of them (pure file/PE-header parsing).
    ///
    /// Verified facts from the repo (see docs/np756p-readonly-research.md):
    ///  - all seven DLLs are x86 (i386);
    ///  - FK623Attend.dll imports FK_GetLastError from FKAttend.dll;
    ///  - FKAttend.dll imports FV_* functions from FKViaDev.dll and LFWViaDev.dll
    ///    and depends on FKPwdEncDec.dll, FpDataConv.dll, FaceDataConv.dll;
    ///  - all of them also depend on OS DLLs (ws2_32, gdi32, gdiplus, ...).
    /// </summary>
    public sealed class NativeLibraryValidator
    {
        public const ushort MachineX86 = 0x014C;
        public const ushort MachineX64 = 0x8664;

        /// <summary>The minimum set needed to load FK623Attend.dll (no blind copying of _dlls).</summary>
        public static readonly string[] RequiredDlls =
        {
            "FK623Attend.dll", "FKAttend.dll", "FKViaDev.dll", "LFWViaDev.dll",
            "FKPwdEncDec.dll", "FpDataConv.dll", "FaceDataConv.dll"
        };

        /// <summary>The only SDK functions this probe is allowed to call.</summary>
        public static readonly string[] AllowedFunctions =
        {
            "FK_ConnectNet", "FK_GetProductData", "FK_GetDeviceInfo",
            "FK_GetDeviceTime", "FK_DisConnect", "FK_GetLastError"
        };

        private static readonly HashSet<string> SystemDlls = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "kernel32.dll", "user32.dll", "gdi32.dll", "gdiplus.dll", "ws2_32.dll",
            "advapi32.dll", "comctl32.dll", "comdlg32.dll", "msimg32.dll", "imm32.dll",
            "ole32.dll", "oleaut32.dll", "shell32.dll", "shlwapi.dll", "setupapi.dll",
            "winmm.dll", "winspool.drv", "oleacc.dll", "msvcrt.dll", "ntdll.dll",
            "msvcr80.dll", "msvcr90.dll", "msvcr100.dll", "msvcr110.dll", "msvcr120.dll",
            "ucrtbase.dll", "crypt32.dll", "version.dll"
        };

        public sealed class DllEntry
        {
            public string FileName;
            public string Status;          // FOUND / MISSING / NOT-A-PE / WRONG-ARCH
            public string Architecture;    // x86 / x64 / -
            public string PredictedError;  // DllNotFoundException / BadImageFormatException / ""
            public List<string> Exports = new List<string>();
            public List<string> ImportDlls = new List<string>();
            public List<string> MissingDependencies = new List<string>();
        }

        public sealed class ValidationReport
        {
            public bool DirectoryExists = true;
            public string Directory;
            public List<DllEntry> Entries = new List<DllEntry>();
            public List<string> MissingAllowedFunctions = new List<string>();
            public bool Succeeded;
            public string FailureSummary = "";

            public List<DllEntry> MissingFiles
            {
                get
                {
                    List<DllEntry> r = new List<DllEntry>();
                    foreach (DllEntry e in Entries)
                    {
                        if (e.Status == "MISSING") { r.Add(e); }
                    }
                    return r;
                }
            }

            public List<DllEntry> WrongArchFiles
            {
                get
                {
                    List<DllEntry> r = new List<DllEntry>();
                    foreach (DllEntry e in Entries)
                    {
                        if (e.Status == "WRONG-ARCH") { r.Add(e); }
                    }
                    return r;
                }
            }
        }

        public static ValidationReport Validate(string dllDir)
        {
            ValidationReport report = new ValidationReport();
            report.Directory = dllDir;
            if (string.IsNullOrEmpty(dllDir) || !Directory.Exists(dllDir))
            {
                report.DirectoryExists = false;
                report.Succeeded = false;
                report.FailureSummary = "DLL directory does not exist: " + (dllDir ?? "<null>");
                return report;
            }

            foreach (string fileName in RequiredDlls)
            {
                DllEntry entry = new DllEntry();
                entry.FileName = fileName;
                string path = Path.Combine(dllDir, fileName);
                if (!File.Exists(path))
                {
                    entry.Status = "MISSING";
                    entry.Architecture = "-";
                    entry.PredictedError = "DllNotFoundException";
                    report.Entries.Add(entry);
                    continue;
                }

                byte[] bytes;
                try
                {
                    bytes = File.ReadAllBytes(path);
                }
                catch (Exception ex)
                {
                    entry.Status = "NOT-A-PE";
                    entry.Architecture = "-";
                    entry.PredictedError = "IOException: " + ex.Message;
                    report.Entries.Add(entry);
                    continue;
                }

                ushort machine;
                List<string> exports;
                List<string> imports;
                string error;
                if (!TryReadPeInfo(bytes, out machine, out exports, out imports, out error))
                {
                    entry.Status = "NOT-A-PE";
                    entry.Architecture = "-";
                    entry.PredictedError = error;
                    report.Entries.Add(entry);
                    continue;
                }

                entry.Exports = exports;
                entry.ImportDlls = imports;
                if (machine == MachineX86)
                {
                    entry.Status = "FOUND";
                    entry.Architecture = "x86";
                }
                else if (machine == MachineX64)
                {
                    entry.Status = "WRONG-ARCH";
                    entry.Architecture = "x64";
                    // An x64 DLL loaded from an x86 process always fails this way.
                    entry.PredictedError = "BadImageFormatException";
                }
                else
                {
                    entry.Status = "WRONG-ARCH";
                    entry.Architecture = "0x" + machine.ToString("X4");
                    entry.PredictedError = "BadImageFormatException";
                }

                // Resolve non-system dependencies against the same directory.
                foreach (string dep in imports)
                {
                    if (SystemDlls.Contains(dep)) { continue; }
                    string depPath = Path.Combine(dllDir, dep);
                    if (!File.Exists(depPath))
                    {
                        entry.MissingDependencies.Add(dep);
                    }
                }
                report.Entries.Add(entry);
            }

            // The entry DLL must expose every allowed function.
            foreach (DllEntry entry in report.Entries)
            {
                if (!string.Equals(entry.FileName, "FK623Attend.dll", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                foreach (string fn in AllowedFunctions)
                {
                    bool found = false;
                    foreach (string ex in entry.Exports)
                    {
                        if (string.Equals(ex, fn, StringComparison.OrdinalIgnoreCase)) { found = true; break; }
                    }
                    if (!found) { report.MissingAllowedFunctions.Add(fn); }
                }
            }

            // Overall verdict.
            report.Succeeded = true;
            if (report.MissingFiles.Count > 0)
            {
                report.Succeeded = false;
                StringBuilder sb = new StringBuilder();
                foreach (DllEntry e in report.MissingFiles)
                {
                    sb.Append(e.FileName).Append(" ");
                }
                report.FailureSummary = "Missing native DLL(s): " + sb.ToString().Trim() +
                    " (expected beside the probe or in --dll-dir; loading would raise DllNotFoundException)";
            }
            else if (report.WrongArchFiles.Count > 0)
            {
                report.Succeeded = false;
                StringBuilder sb = new StringBuilder();
                foreach (DllEntry e in report.WrongArchFiles)
                {
                    sb.Append(e.FileName).Append(" ");
                }
                report.FailureSummary = "Architecture mismatch in: " + sb.ToString().Trim() +
                    " (process is x86; loading would raise BadImageFormatException)";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (DllEntry e in report.Entries)
                {
                    foreach (string dep in e.MissingDependencies)
                    {
                        sb.Append(e.FileName).Append(" -> ").Append(dep).Append(" ");
                    }
                }
                if (sb.Length > 0)
                {
                    report.Succeeded = false;
                    report.FailureSummary = "Missing native dependency chain: " + sb.ToString().Trim();
                }
                else if (report.MissingAllowedFunctions.Count > 0)
                {
                    report.Succeeded = false;
                    report.FailureSummary = "FK623Attend.dll does not export: " +
                        string.Join(", ", report.MissingAllowedFunctions.ToArray());
                }
            }
            return report;
        }

        /// <summary>
        /// Minimal PE reader: machine type, export names and imported DLL names.
        /// Layout constants follow the IMAGE_NT_HEADERS / IMAGE_EXPORT_DIRECTORY
        /// structures (NumberOfNames at +24, AddressOfNames at +32, ...).
        /// Returns false with a reason when the file is not a valid PE image.
        /// </summary>
        public static bool TryReadPeInfo(byte[] b, out ushort machine, out List<string> exports, out List<string> imports, out string error)
        {
            machine = 0;
            exports = new List<string>();
            imports = new List<string>();
            error = null;

            if (b == null || b.Length < 0x100) { error = "file too small"; return false; }
            if (b[0] != (byte)'M' || b[1] != (byte)'Z') { error = "no MZ signature"; return false; }
            int peOff = ReadU32(b, 0x3C);
            if (peOff <= 0 || peOff + 24 > b.Length) { error = "bad PE offset"; return false; }
            if (b[peOff] != (byte)'P' || b[peOff + 1] != (byte)'E' || b[peOff + 2] != 0 || b[peOff + 3] != 0)
            {
                error = "no PE signature"; return false;
            }

            machine = ReadU16(b, peOff + 4);
            int nSections = ReadU16(b, peOff + 6);
            int optSize = ReadU16(b, peOff + 20);
            int opt = peOff + 24;
            if (opt + optSize > b.Length) { error = "truncated optional header"; return false; }
            ushort magic = ReadU16(b, opt);
            int dd = opt + (magic == 0x20B ? 112 : 96);
            if (dd + 16 > b.Length) { error = "truncated data directories"; return false; }

            int exportRva = ReadU32(b, dd);
            int importRva = ReadU32(b, dd + 8);

            List<object[]> sections = new List<object[]>(); // [va, vsz, raw, rsz]
            int secOff = opt + optSize;
            for (int i = 0; i < nSections; i++)
            {
                int o = secOff + i * 40;
                if (o + 40 > b.Length) { break; }
                sections.Add(new object[]
                {
                    ReadU32(b, o + 12), // virtual address
                    ReadU32(b, o + 8),  // virtual size
                    ReadU32(b, o + 20), // pointer to raw data
                    ReadU32(b, o + 16)  // size of raw data
                });
            }

            // ---- exports ----
            if (exportRva > 0)
            {
                int eo = RvaToOffset(sections, exportRva);
                if (eo >= 0)
                {
                    int nNames = ReadU32(b, eo + 24);
                    int namesRva = ReadU32(b, eo + 32);
                    int no = RvaToOffset(sections, namesRva);
                    if (no >= 0 && nNames > 0 && nNames < 100000)
                    {
                        for (int i = 0; i < nNames; i++)
                        {
                            int p = ReadU32(b, no + 4 * i);
                            int po = RvaToOffset(sections, p);
                            if (po >= 0)
                            {
                                string s = ReadCString(b, po);
                                if (s.Length > 0) { exports.Add(s); }
                            }
                        }
                    }
                }
            }

            // ---- imports ----
            if (importRva > 0)
            {
                int io = RvaToOffset(sections, importRva);
                if (io >= 0)
                {
                    for (int i = 0; ; i++)
                    {
                        int ent = io + i * 20;
                        if (ent + 20 > b.Length) { break; }
                        int nameRva = ReadU32(b, ent + 12);
                        if (nameRva == 0) { break; }
                        int no = RvaToOffset(sections, nameRva);
                        if (no >= 0)
                        {
                            string dll = ReadCString(b, no);
                            if (dll.Length > 0) { imports.Add(dll); }
                        }
                    }
                }
            }
            return true;
        }

        private static int RvaToOffset(List<object[]> sections, int rva)
        {
            if (rva <= 0) { return -1; }
            foreach (object[] s in sections)
            {
                int va = (int)s[0];
                int vsz = (int)s[1];
                int raw = (int)s[2];
                int rsz = (int)s[3];
                int span = Math.Max(vsz, rsz);
                if (va <= rva && rva < va + span)
                {
                    int off = raw + (rva - va);
                    if (off >= 0 && off < int.MaxValue) { return off; }
                }
            }
            return -1;
        }

        private static string ReadCString(byte[] b, int off)
        {
            if (off < 0 || off >= b.Length) { return ""; }
            int end = off;
            int limit = Math.Min(b.Length, off + 512);
            while (end < limit && b[end] != 0) { end++; }
            return Encoding.ASCII.GetString(b, off, end - off);
        }

        private static ushort ReadU16(byte[] b, int o)
        {
            return (ushort)(b[o] | (b[o + 1] << 8));
        }

        private static int ReadU32(byte[] b, int o)
        {
            return b[o] | (b[o + 1] << 8) | (b[o + 2] << 16) | (b[o + 3] << 24);
        }
    }
}
