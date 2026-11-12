using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// NP756P READ-ONLY DIAGNOSTIC PROBE (net48 / x86 console).
    ///
    /// Reads only: FK_ConnectNet, FK_GetProductData, FK_GetDeviceInfo,
    /// FK_GetDeviceTime, FK_DisConnect (+ FK_GetLastError with a valid handle).
    /// It never writes device state, time, users, logs or settings, never calls
    /// FK_EnableDevice, never retries passwords and never scans ports.
    /// </summary>
    internal static class Program
    {
        private const string Header =
            "NP756P READ-ONLY DIAGNOSTIC  (Hysoon classic SDK, model 9308)\r\n" +
            "Standalone tool - independent of the Novin software\r\n" +
            "Read-only: no device state, clock, users or logs are modified.\r\n";

        private static int Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
            }
            catch
            {
                // keep default encoding if the console refuses UTF-8
            }

            Console.WriteLine(Header);

            ProbeOptions options;
            List<string> errors;
            if (!ProbeOptions.TryParse(args, out options, out errors))
            {
                foreach (string e in errors)
                {
                    if (e == "__HELP__")
                    {
                        PrintUsage();
                        return 0;
                    }
                    Console.WriteLine("Argument error: " + e);
                }
                PrintUsage();
                return 1;
            }

            // ---- hidden numeric password (never echoed, never logged) ----
            int password;
            string passwordError;
            string rawPassword = ReadHidden("Network password (numeric, input is hidden): ");
            if (!ProbeOptions.TryParsePassword(rawPassword, out password, out passwordError))
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + passwordError);
                return 1;
            }
            options.NetworkPassword = password;

            // ---- stage A: static validation of native libraries ----
            string dllDir = options.DllDir ?? AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Validating native SDK files in: " + dllDir);
            NativeLibraryValidator.ValidationReport native =
                NativeLibraryValidator.Validate(dllDir);

            if (options.DllDir != null)
            {
                // Best effort to let the OS loader find the DLLs (managed-only,
                // no extra native call is made).
                try { Environment.CurrentDirectory = options.DllDir; }
                catch { }
            }

            // ---- stages B..F through the real channel ----
            ProbeResult result = ProbeRunner.Run(options, new FkDeviceChannel(), native);

            Console.Write(result.Render());
            Console.WriteLine("Exit code: " + result.ExitCode);
            return result.ExitCode;
        }

        /// <summary>
        /// Reads a line without echoing characters (masked with '*').
        /// Works on Windows console; characters are never printed, Backspace is
        /// handled, Enter submits. The value itself is never displayed afterwards.
        /// </summary>
        private static string ReadHidden(string prompt)
        {
            Console.Write(prompt);
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length = sb.Length - 1;
                        Console.Write("\b \b");
                    }
                    continue;
                }
                if (key.KeyChar != '\0' && key.KeyChar >= 32 && key.KeyChar < 127)
                {
                    sb.Append(key.KeyChar);
                    Console.Write('*');
                }
            }
            return sb.ToString();
        }

        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: NP756P.ReadOnlyProbe [--ip 192.168.1.75] [--port 4238]");
            Console.WriteLine("                            [--machine 1] [--timeout 5000]");
            Console.WriteLine("                            [--protocol tcp|udp] [--license 1261]");
            Console.WriteLine("                            [--dll-dir C:\\path\\to\\vendor\\dlls]");
            Console.WriteLine();
            Console.WriteLine("The numeric network password is requested hidden at startup.");
            Console.WriteLine("Defaults: ip=192.168.1.75 port=4238 machine=1 timeout=5000 protocol=tcp license=1261.");
            Console.WriteLine("The probe exe must be able to load FK623Attend.dll and its six vendor");
            Console.WriteLine("companions (see README); pass --dll-dir or copy them beside the exe.");
        }
    }
}
