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
    /// Facts verified from the real repo binaries (see docs/np756p-readonly-research.md):
    ///  - all seven DLLs are x86 (i386);
    ///  - FK623Attend.dll imports from FKAttend.dll ONE named function
    ///    (FK_GetLastError) plus 160 functions BY ORDINAL (1..16 and 18..161);
    ///  - FKAttend.dll imports from FKPwdEncDec.dll/FpDataConv.dll/FaceDataConv.dll
    ///    by ordinal and from FKViaDev.dll/LFWViaDev.dll by (mangled) name;
    ///  - all of them also depend on OS DLLs (ws2_32, gdi32, gdiplus, ...), which
    ///    are not verified (they ship with Windows).
    ///
    /// The validator is fail-closed: a file is accepted only when it exists, its
    /// bytes can be read, it parses as a PE of the expected architecture, every
    /// imported vendor function/ordinal is resolvable against the target export
    /// table, and the entry DLL exports all allowed functions. Any other state
    /// (NOT-A-PE, PARSE-ERROR, IO-ERROR, MISSING, WRONG-ARCH) fails validation.
    ///
    /// Parser limitations (documented on purpose - no full-PE compatibility claim):
    ///  - only PE32 (0x10B) and PE32+ (0x20B) images are parsed;
    ///  - the import table is read through OriginalFirstThunk when present,
    ///    FirstThunk otherwise; DELAY imports and bound-import pre-resolution
    ///    are not parsed;
    ///  - forwarded export entries are treated as occupied slots but the
    ///    forwarder chain is not followed;
    ///  - .NET/mixed-mode metadata and TLS/reloc/debug directories are ignored;
    ///  - import verification covers vendor DLL pairs only (system DLL imports
    ///    are skipped because their export tables are not available here).
    /// </summary>
    public sealed class NativeLibraryValidator
    {
        public const ushort MachineX86 = 0x014C;
        public const ushort MachineX64 = 0x8664;

        /// <summary>Status of a file that is not a PE image at all (no MZ/PE signature).</summary>
        public const string StatusNotPe = "NOT-A-PE";

        /// <summary>Status of a file that carries a PE signature but whose
        /// export/import/section tables are corrupt, truncated or out of file.</summary>
        public const string StatusParseError = "PARSE-ERROR";

        /// <summary>Status of a file whose bytes could not be read at all.</summary>
        public const string StatusIoError = "IO-ERROR";

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

        // =============================================================
        // PE parse results
        // =============================================================

        /// <summary>One imported function: either by name or by ordinal.</summary>
        public sealed class ImportedFunction
        {
            public bool ByOrdinal;
            public int Ordinal;
            public string Name;

            public string Display
            {
                get { return ByOrdinal ? "#" + Ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture) : (Name ?? ""); }
            }
        }

        /// <summary>Functions imported from a single target DLL, in thunk order.</summary>
        public sealed class ImportDllInfo
        {
            public string DllName;
            public List<ImportedFunction> Functions = new List<ImportedFunction>();
        }

        /// <summary>Everything the static parser extracts from one PE image.</summary>
        public sealed class PeInfo
        {
            public ushort Machine;

            // exports
            public int ExportOrdinalBase;
            public int ExportFunctionCount;
            public List<string> ExportNames = new List<string>();
            /// <summary>Slot i is true when AddressOfFunctions[Base+i] is non-null
            /// (i.e. absolute ordinal Base+i really is exported).</summary>
            public bool[] ExportOrdinalOccupied;

            // imports
            public List<string> ImportDllNames = new List<string>();
            public List<ImportDllInfo> Imports = new List<ImportDllInfo>();

            public bool OrdinalExported(int ordinal)
            {
                if (ExportOrdinalOccupied == null || ExportOrdinalOccupied.Length == 0) { return false; }
                int idx = ordinal - ExportOrdinalBase;
                return idx >= 0 && idx < ExportOrdinalOccupied.Length && ExportOrdinalOccupied[idx];
            }

            public bool ExportsName(string name)
            {
                if (string.IsNullOrEmpty(name)) { return false; }
                foreach (string n in ExportNames)
                {
                    if (string.Equals(n, name, StringComparison.OrdinalIgnoreCase)) { return true; }
                }
                return false;
            }
        }

        /// <summary>An import that the target DLL does not satisfy.</summary>
        public sealed class ImportProblem
        {
            public string SourceDll;
            public string TargetDll;
            public ImportedFunction Function;

            public string Description()
            {
                if (Function == null) { return ""; }
                return SourceDll + " imports " + (Function.ByOrdinal
                    ? "ordinal " + Function.Ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture)
                    : "function '" + Function.Name + "'")
                    + " from " + TargetDll + " but the target does not export it";
            }
        }

        // =============================================================
        // validation report
        // =============================================================

        public sealed class DllEntry
        {
            public string FileName;
            // FOUND / MISSING / NOT-A-PE / PARSE-ERROR / IO-ERROR / WRONG-ARCH
            public string Status;
            /// <summary>Human-readable detail: parse reason for failures, or a
            /// compact export/import summary for healthy files.</summary>
            public string StatusDetail = "";
            public string Architecture;    // x86 / x64 / 0x???? / -
            public string PredictedError;  // DllNotFoundException / BadImageFormatException / "" ...
            public PeInfo Pe;

            public List<string> Exports = new List<string>();
            public List<string> ImportDlls = new List<string>();
            public List<ImportProblem> MissingImports = new List<ImportProblem>();
            public List<string> MissingDependencies = new List<string>();

            public int TotalOrdinalImports
            {
                get
                {
                    int n = 0;
                    if (Pe != null)
                    {
                        foreach (ImportDllInfo d in Pe.Imports)
                        {
                            foreach (ImportedFunction f in d.Functions)
                            {
                                if (f.ByOrdinal) { n++; }
                            }
                        }
                    }
                    return n;
                }
            }

            public int TotalNamedImports
            {
                get
                {
                    int n = 0;
                    if (Pe != null)
                    {
                        foreach (ImportDllInfo d in Pe.Imports)
                        {
                            foreach (ImportedFunction f in d.Functions)
                            {
                                if (!f.ByOrdinal) { n++; }
                            }
                        }
                    }
                    return n;
                }
            }
        }

        public sealed class ValidationReport
        {
            public bool DirectoryExists = true;
            public string Directory;
            public List<DllEntry> Entries = new List<DllEntry>();
            public List<string> MissingAllowedFunctions = new List<string>();
            public List<ImportProblem> ImportProblems = new List<ImportProblem>();
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

            /// <summary>Every entry that is not in the healthy FOUND/x86 state.</summary>
            public List<DllEntry> BadFiles
            {
                get
                {
                    List<DllEntry> r = new List<DllEntry>();
                    foreach (DllEntry e in Entries)
                    {
                        if (e.Status != "FOUND") { r.Add(e); }
                    }
                    return r;
                }
            }
        }

        // =============================================================
        // Validate
        // =============================================================

        public static ValidationReport Validate(string dllDir)
        {
            ValidationReport report = new ValidationReport();
            report.Directory = dllDir;
            List<string> problems = new List<string>();

            if (string.IsNullOrEmpty(dllDir) || !Directory.Exists(dllDir))
            {
                report.DirectoryExists = false;
                report.Succeeded = false;
                report.FailureSummary = "DLL directory does not exist: " + (dllDir ?? "<null>");
                return report;
            }

            Dictionary<string, DllEntry> requiredByName =
                new Dictionary<string, DllEntry>(StringComparer.OrdinalIgnoreCase);

            foreach (string fileName in RequiredDlls)
            {
                DllEntry entry = ParseOneDll(dllDir, fileName);
                report.Entries.Add(entry);
                requiredByName[fileName] = entry;
            }

            // ---- cross-check every vendor import against the target export table ----
            // extra (non-required) import targets that exist in the directory are parsed on demand
            Dictionary<string, PeInfo> extraTargets = new Dictionary<string, PeInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (DllEntry entry in report.Entries)
            {
                if (entry.Pe == null) { continue; }   // its own status already fails validation
                foreach (ImportDllInfo import in entry.Pe.Imports)
                {
                    if (SystemDlls.Contains(import.DllName)) { continue; }

                    PeInfo targetPe = null;
                    DllEntry targetEntry;
                    if (requiredByName.TryGetValue(import.DllName, out targetEntry))
                    {
                        targetPe = targetEntry.Pe;
                        if (targetPe == null)
                        {
                            // the target is one of the required DLLs and is itself unreadable:
                            // its own entry already fails validation; record the dependency too.
                            entry.MissingImports.Add(new ImportProblem
                            {
                                SourceDll = entry.FileName,
                                TargetDll = import.DllName,
                                Function = null,
                            });
                            problems.Add(entry.FileName + " -> " + import.DllName +
                                ": import target is not usable (its own status is " +
                                (targetEntry.Status ?? "?") + ")");
                            continue;
                        }
                    }
                    else
                    {
                        string depPath = Path.Combine(dllDir, import.DllName);
                        if (!File.Exists(depPath))
                        {
                            // plain missing dependency - recorded below via MissingDependencies
                            continue;
                        }
                        if (!extraTargets.TryGetValue(import.DllName, out targetPe))
                        {
                            byte[] bytes;
                            try { bytes = File.ReadAllBytes(depPath); }
                            catch { bytes = null; }
                            string err = null;
                            if (bytes == null || !TryReadPeInfo(bytes, out targetPe, out err))
                            {
                                targetPe = null;
                                problems.Add(entry.FileName + " -> " + import.DllName +
                                    ": import target is present but not a parseable PE (" +
                                    (err ?? "unreadable file") + ")");
                            }
                            extraTargets[import.DllName] = targetPe;   // may store null on purpose
                        }
                        if (targetPe == null) { continue; }
                    }

                    if (targetPe == null) { continue; }

                    foreach (ImportedFunction fn in import.Functions)
                    {
                        bool ok = fn.ByOrdinal ? targetPe.OrdinalExported(fn.Ordinal)
                                               : targetPe.ExportsName(fn.Name);
                        if (!ok)
                        {
                            ImportProblem p = new ImportProblem
                            {
                                SourceDll = entry.FileName,
                                TargetDll = import.DllName,
                                Function = fn
                            };
                            entry.MissingImports.Add(p);
                            report.ImportProblems.Add(p);
                            problems.Add(p.Description());
                        }
                    }
                }
            }

            // ---- the entry DLL must expose every allowed function ----
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

            // ---- verdict: fail closed, every problem is reported ----
            foreach (DllEntry e in report.Entries)
            {
                if (e.Status != "FOUND")
                {
                    problems.Insert(0, e.FileName + ": " + e.Status +
                        (string.IsNullOrEmpty(e.StatusDetail) ? "" : " (" + e.StatusDetail + ")"));
                }
                foreach (string dep in e.MissingDependencies)
                {
                    problems.Add(e.FileName + " -> " + dep +
                        " (file not found beside the probe; loading would raise DllNotFoundException)");
                }
            }
            if (report.MissingAllowedFunctions.Count > 0)
            {
                problems.Add("FK623Attend.dll does not export the required read-only functions: " +
                    string.Join(", ", report.MissingAllowedFunctions.ToArray()));
            }

            report.Succeeded = problems.Count == 0;
            if (!report.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string p in problems)
                {
                    if (sb.Length > 0) { sb.Append("; "); }
                    sb.Append(p);
                }
                string text = sb.ToString();
                report.FailureSummary = text.Length > 2000 ? text.Substring(0, 2000) + " ..." : text;
            }
            return report;
        }

        /// <summary>Reads one file into a DllEntry, never throwing.</summary>
        private static DllEntry ParseOneDll(string dllDir, string fileName)
        {
            DllEntry entry = new DllEntry();
            entry.FileName = fileName;
            string path = Path.Combine(dllDir, fileName);

            if (!File.Exists(path))
            {
                entry.Status = "MISSING";
                entry.Architecture = "-";
                entry.PredictedError = "DllNotFoundException";
                entry.StatusDetail = "file not found";
                return entry;
            }

            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                entry.Status = StatusIoError;
                entry.Architecture = "-";
                entry.PredictedError = "IOException (" + ex.GetType().Name + ")";
                entry.StatusDetail = "the file exists but its bytes could not be read (" +
                    ex.GetType().Name + ")";
                return entry;
            }

            PeInfo info;
            string error;
            if (!TryReadPeInfo(bytes, out info, out error))
            {
                entry.Status = error != null && error.StartsWith(StatusNotPe, StringComparison.Ordinal)
                    ? StatusNotPe : StatusParseError;
                entry.Architecture = "-";
                entry.PredictedError = entry.Status == StatusNotPe
                    ? "BadImageFormatException (not a PE image)"
                    : "BadImageFormatException / corrupt image";
                entry.StatusDetail = error ?? "unknown parse failure";
                return entry;
            }

            entry.Pe = info;
            entry.Exports = info.ExportNames;
            entry.ImportDlls = info.ImportDllNames;

            if (info.Machine == MachineX86)
            {
                entry.Status = "FOUND";
                entry.Architecture = "x86";
                entry.PredictedError = "";
            }
            else if (info.Machine == MachineX64)
            {
                entry.Status = "WRONG-ARCH";
                entry.Architecture = "x64";
                entry.PredictedError = "BadImageFormatException";
            }
            else
            {
                entry.Status = "WRONG-ARCH";
                entry.Architecture = "0x" + info.Machine.ToString("X4");
                entry.PredictedError = "BadImageFormatException";
            }

            int nonSys = 0;
            foreach (string dep in info.ImportDllNames)
            {
                if (SystemDlls.Contains(dep)) { continue; }
                nonSys++;
                string depPath = Path.Combine(dllDir, dep);
                if (!File.Exists(depPath))
                {
                    entry.MissingDependencies.Add(dep);
                }
            }
            entry.StatusDetail = (entry.Architecture == "x86" ? "x86" : entry.Architecture) +
                " - " + info.ExportNames.Count + " export name(s)" +
                (info.ExportOrdinalOccupied != null ? ", " + info.ExportOrdinalOccupied.Length + " export ordinal slot(s)" : "") +
                " - imports " + info.ImportDllNames.Count + " DLL(s) (" +
                (info.ImportDllNames.Count - nonSys) + " system, " + nonSys + " vendor)" +
                ", " + entry.TotalNamedImports + " named + " + entry.TotalOrdinalImports + " ordinal function(s)";
            return entry;
        }

        // =============================================================
        // PE reader
        // =============================================================

        /// <summary>
        /// Fully bounds-checked minimal PE reader: machine type, export table
        /// (names + ordinal occupancy) and import table (DLL names + named and
        /// ordinal-based functions). Layout constants follow IMAGE_NT_HEADERS /
        /// IMAGE_EXPORT_DIRECTORY / IMAGE_IMPORT_DESCRIPTOR.
        ///
        /// Fail-closed: any truncated table, RVA outside the file, impossible
        /// count or unmapped pointer is reported as an error instead of being
        /// silently skipped, and the parser never throws on malformed input
        /// (no IndexOutOfRangeException / OverflowException is possible).
        /// Error strings are prefixed with NOT-A-PE (image identity problems)
        /// or PARSE-ERROR (structural problems) so callers can classify them.
        /// </summary>
        public static bool TryReadPeInfo(byte[] b, out PeInfo info, out string error)
        {
            info = null;
            error = null;

            if (b == null || b.Length < 0x40)
            {
                error = StatusNotPe + ": file is smaller than a DOS header";
                return false;
            }
            if (b[0] != (byte)'M' || b[1] != (byte)'Z')
            {
                error = StatusNotPe + ": no MZ signature";
                return false;
            }
            int peOff = ReadU32(b, 0x3C);
            if (peOff <= 0 || peOff + 24 > b.Length)
            {
                error = StatusNotPe + ": PE header offset (0x" + peOff.ToString("X") + ") points outside the file";
                return false;
            }
            if (b[peOff] != (byte)'P' || b[peOff + 1] != (byte)'E' || b[peOff + 2] != 0 || b[peOff + 3] != 0)
            {
                error = StatusNotPe + ": no PE signature";
                return false;
            }

            int machine = ReadU16(b, peOff + 4);
            int nSections = ReadU16(b, peOff + 6);
            int optSize = ReadU16(b, peOff + 20);
            int opt = peOff + 24;

            if (nSections < 0 || nSections > 96)
            {
                error = StatusParseError + ": impossible section count (" + nSections + ")";
                return false;
            }
            if (optSize <= 0 || opt + optSize > b.Length)
            {
                error = StatusParseError + ": optional header is truncated (size " + optSize + ")";
                return false;
            }

            int magic = ReadU16(b, opt);
            bool pe32Plus = (magic == 0x20B);
            if (magic != 0x10B && magic != 0x20B)
            {
                error = StatusParseError + ": unknown optional-header magic 0x" + magic.ToString("X");
                return false;
            }

            int nRvaSizeFieldOffset = pe32Plus ? 108 : 92;
            int dd = opt + (pe32Plus ? 112 : 96);
            if (nRvaSizeFieldOffset + 4 > optSize)
            {
                error = StatusParseError + ": optional header too small for NumberOfRvaAndSizes";
                return false;
            }
            int nRva = ReadU32(b, opt + nRvaSizeFieldOffset);
            if (nRva < 0 || nRva > 16)
            {
                error = StatusParseError + ": impossible NumberOfRvaAndSizes (" + nRva + ")";
                return false;
            }
            if (nRva >= 2 && dd + 16 > opt + optSize)
            {
                error = StatusParseError + ": data directories extend beyond the optional header";
                return false;
            }

            int exportRva = nRva >= 1 ? ReadU32(b, dd) : 0;
            int importRva = nRva >= 2 ? ReadU32(b, dd + 8) : 0;

            // ---- section table ----
            List<object[]> sections = new List<object[]>(); // [va, vsz, raw, rsz]
            int secOff = opt + optSize;
            if (nSections > 0)
            {
                long tableEnd = (long)secOff + (long)nSections * 40L;
                if (tableEnd > b.Length)
                {
                    error = StatusParseError + ": section table is truncated (needs " + tableEnd +
                        " bytes, file has " + b.Length + ")";
                    return false;
                }
                for (int i = 0; i < nSections; i++)
                {
                    int o = secOff + i * 40;
                    sections.Add(new object[]
                    {
                        ReadU32(b, o + 12), // virtual address
                        ReadU32(b, o + 8),  // virtual size
                        ReadU32(b, o + 20), // pointer to raw data
                        ReadU32(b, o + 16)  // size of raw data
                    });
                }
            }

            PeInfo result = new PeInfo();
            result.Machine = (ushort)machine;

            // ---- export directory ----
            if (exportRva != 0)
            {
                string expErr;
                if (!ReadExportDirectory(b, sections, exportRva, result, out expErr))
                {
                    error = StatusParseError + ": " + expErr;
                    return false;
                }
            }

            // ---- import directory ----
            if (importRva != 0)
            {
                string impErr;
                if (!ReadImportDirectory(b, sections, importRva, pe32Plus, result, out impErr))
                {
                    error = StatusParseError + ": " + impErr;
                    return false;
                }
            }

            info = result;
            return true;
        }

        private static bool ReadExportDirectory(byte[] b, List<object[]> sections, int exportRva, PeInfo result, out string error)
        {
            error = null;
            int eo = RvaToOffset(sections, exportRva);
            if (eo < 0)
            {
                error = "export directory RVA 0x" + exportRva.ToString("X") + " is not mapped inside any section (RVA outside the file)";
                return false;
            }
            if (eo + 40 > b.Length)
            {
                error = "export directory is truncated (needs " + (eo + 40) + " bytes, file has " + b.Length + ")";
                return false;
            }

            int ordBase = ReadU32(b, eo + 16);
            int nFunc = ReadU32(b, eo + 20);
            int nNames = ReadU32(b, eo + 24);
            int funcsRva = ReadU32(b, eo + 28);
            int namesRva = ReadU32(b, eo + 32);
            int ordsRva = ReadU32(b, eo + 36);

            if (ordBase < 0 || nFunc < 0 || nNames < 0)
            {
                error = "export directory contains negative counts";
                return false;
            }
            if (nFunc > 1048576)
            {
                error = "impossible export function count (" + nFunc + ")";
                return false;
            }
            if (nNames > nFunc)
            {
                error = "export name count (" + nNames + ") exceeds the number of function slots (" + nFunc + ")";
                return false;
            }

            result.ExportOrdinalBase = ordBase;
            result.ExportFunctionCount = nFunc;
            result.ExportOrdinalOccupied = new bool[nFunc];

            if (nFunc > 0)
            {
                int funcsO = RvaToOffset(sections, funcsRva);
                if (funcsO < 0)
                {
                    error = "export address table RVA 0x" + funcsRva.ToString("X") + " is not mapped inside any section";
                    return false;
                }
                if ((long)funcsO + 4L * nFunc > b.Length)
                {
                    error = "export address table is truncated (needs " + (funcsO + 4L * nFunc) + " bytes, file has " + b.Length + ")";
                    return false;
                }
                for (int i = 0; i < nFunc; i++)
                {
                    result.ExportOrdinalOccupied[i] = ReadU32(b, funcsO + 4 * i) != 0;
                }
            }

            if (nNames > 0)
            {
                int namesO = RvaToOffset(sections, namesRva);
                if (namesO < 0)
                {
                    error = "export name-pointer table RVA 0x" + namesRva.ToString("X") + " is not mapped inside any section";
                    return false;
                }
                if ((long)namesO + 4L * nNames > b.Length)
                {
                    error = "export name-pointer table is truncated (needs " + (namesO + 4L * nNames) + " bytes, file has " + b.Length + ")";
                    return false;
                }
                int ordsO = RvaToOffset(sections, ordsRva);
                if (ordsO < 0)
                {
                    error = "export ordinal table RVA 0x" + ordsRva.ToString("X") + " is not mapped inside any section";
                    return false;
                }
                if ((long)ordsO + 2L * nNames > b.Length)
                {
                    error = "export ordinal table is truncated (needs " + (ordsO + 2L * nNames) + " bytes, file has " + b.Length + ")";
                    return false;
                }

                for (int i = 0; i < nNames; i++)
                {
                    int nameRva = ReadU32(b, namesO + 4 * i);
                    int no = RvaToOffset(sections, nameRva);
                    if (no < 0)
                    {
                        error = "export name RVA 0x" + nameRva.ToString("X") + " is not mapped inside any section";
                        return false;
                    }
                    string s = ReadCString(b, no);
                    if (s.Length == 0)
                    {
                        error = "export name at RVA 0x" + nameRva.ToString("X") + " is empty";
                        return false;
                    }
                    int ordIndex = ReadU16(b, ordsO + 2 * i);
                    if (ordIndex >= nFunc)
                    {
                        error = "export ordinal index " + ordIndex + " is out of range (function slots: " + nFunc + ")";
                        return false;
                    }
                    result.ExportNames.Add(s);
                }
            }
            return true;
        }

        private static bool ReadImportDirectory(byte[] b, List<object[]> sections, int importRva, bool pe32Plus, PeInfo result, out string error)
        {
            error = null;
            int io = RvaToOffset(sections, importRva);
            if (io < 0)
            {
                error = "import directory RVA 0x" + importRva.ToString("X") + " is not mapped inside any section (RVA outside the file)";
                return false;
            }

            int entrySize = pe32Plus ? 8 : 4;
            for (int i = 0; ; i++)
            {
                if (i > 4096)
                {
                    error = "too many import descriptors (no terminator found)";
                    return false;
                }
                int ent = io + i * 20;
                if (ent + 20 > b.Length)
                {
                    error = "import descriptor table is truncated (descriptor " + i + " needs " + (ent + 20) + " bytes, file has " + b.Length + ")";
                    return false;
                }
                int oft = ReadU32(b, ent);
                int ts = ReadU32(b, ent + 4);
                int fc = ReadU32(b, ent + 8);
                int nameRva = ReadU32(b, ent + 12);
                int ft = ReadU32(b, ent + 16);
                if (oft == 0 && ts == 0 && fc == 0 && nameRva == 0 && ft == 0)
                {
                    break;   // all-zero terminator
                }
                if (nameRva == 0)
                {
                    error = "import descriptor " + i + " has no DLL name (null RVA)";
                    return false;
                }
                int no = RvaToOffset(sections, nameRva);
                if (no < 0)
                {
                    error = "import DLL name RVA 0x" + nameRva.ToString("X") + " is not mapped inside any section";
                    return false;
                }
                string dll = ReadCString(b, no);
                if (dll.Length == 0)
                {
                    error = "import DLL name at descriptor " + i + " is empty";
                    return false;
                }

                ImportDllInfo import = new ImportDllInfo();
                import.DllName = dll;
                result.ImportDllNames.Add(dll);
                result.Imports.Add(import);

                // OriginalFirstThunk is the on-disk view of the thunk array; older
                // tools sometimes zero it, so FirstThunk is the fallback.
                int thunkRva = oft != 0 ? oft : ft;
                if (thunkRva == 0)
                {
                    error = "import descriptor for " + dll + " has no thunk array";
                    return false;
                }
                int to = RvaToOffset(sections, thunkRva);
                if (to < 0)
                {
                    error = "import thunk array RVA 0x" + thunkRva.ToString("X") + " (" + dll + ") is not mapped inside any section";
                    return false;
                }

                for (int j = 0; ; j++)
                {
                    if (j > 1048576)
                    {
                        error = "import thunk array for " + dll + " has no terminator";
                        return false;
                    }
                    int off = to + j * entrySize;
                    if (off + entrySize > b.Length)
                    {
                        error = "import thunk array for " + dll + " is truncated (entry " + j + " needs " + (off + entrySize) + " bytes, file has " + b.Length + ")";
                        return false;
                    }
                    ulong val = pe32Plus ? ReadU64(b, off) : (ulong)ReadU32(b, off);
                    if (val == 0) { break; }   // terminator of this DLL's thunks

                    ImportedFunction fn = new ImportedFunction();
                    if ((val & 0x8000000000000000UL) != 0 || (!pe32Plus && (val & 0x80000000UL) != 0))
                    {
                        fn.ByOrdinal = true;
                        fn.Ordinal = (int)(val & 0xFFFFUL);
                    }
                    else
                    {
                        int rva = (int)(val & 0x7FFFFFFFUL);
                        int ho = RvaToOffset(sections, rva);
                        if (ho < 0)
                        {
                            error = "import hint/name RVA 0x" + rva.ToString("X") + " (" + dll + ") is not mapped inside any section";
                            return false;
                        }
                        if (ho + 2 > b.Length)
                        {
                            error = "import hint/name entry for " + dll + " is truncated";
                            return false;
                        }
                        string name = ReadCString(b, ho + 2);
                        if (name.Length == 0)
                        {
                            error = "import function name for " + dll + " (hint/name at RVA 0x" + rva.ToString("X") + ") is empty";
                            return false;
                        }
                        fn.ByOrdinal = false;
                        fn.Name = name;
                    }
                    import.Functions.Add(fn);
                }
            }
            return true;
        }

        // =============================================================
        // compatibility overload (kept for callers that only need names)
        // =============================================================

        public static bool TryReadPeInfo(byte[] b, out ushort machine, out List<string> exports, out List<string> imports, out string error)
        {
            PeInfo info;
            bool ok = TryReadPeInfo(b, out info, out error);
            machine = ok ? info.Machine : (ushort)0;
            exports = ok ? info.ExportNames : new List<string>();
            imports = ok ? info.ImportDllNames : new List<string>();
            return ok;
        }

        // =============================================================
        // low-level helpers (bounds-safe)
        // =============================================================

        /// <summary>Maps an RVA to a file offset; returns -1 when the RVA is not
        /// covered by any section's mapped span or would land outside the file.</summary>
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
                    if (raw <= 0) { return -1; }   // no file data (e.g. BSS)
                    long off = (long)raw + (long)(rva - va);
                    if (off < 0 || off > int.MaxValue) { return -1; }
                    return (int)off;
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

        private static ulong ReadU64(byte[] b, int o)
        {
            return (ulong)(uint)ReadU32(b, o) | ((ulong)(uint)ReadU32(b, o + 4) << 32);
        }
    }
}
