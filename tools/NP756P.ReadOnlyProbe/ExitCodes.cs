using System;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// Process exit codes of the probe, kept in one place so Program, ProbeRunner
    /// and the README describe the exact same behavior.
    /// </summary>
    public static class ExitCodes
    {
        /// <summary>All stages succeeded and the connection was closed.</summary>
        public const int Success = 0;

        /// <summary>Bad command line arguments or unusable password input
        /// (including a redirected console where the hidden prompt cannot run).</summary>
        public const int InputError = 1;

        /// <summary>Stage A failed: native DLL files missing / not PE / wrong
        /// architecture / unresolvable import-export chain.</summary>
        public const int NativeLibraryStageFailed = 2;

        /// <summary>FK_ConnectNet did not produce a live handle.</summary>
        public const int ConnectFailed = 3;

        /// <summary>A read stage failed, an exception hit a read call, or the
        /// final FK_DisConnect failed (a failed disconnect must not yield 0).</summary>
        public const int ReadStageFailed = 4;

        /// <summary>An unexpected exception escaped the normal flow.</summary>
        public const int UnexpectedException = 5;
    }

    /// <summary>
    /// Safe formatting of exceptions for console output: the type and a short
    /// message only. Never the stack trace, never inner exceptions, never
    /// collections of data - so nothing confidential can leak into a report that
    /// the operator copies to support.
    /// </summary>
    public static class ExceptionText
    {
        private const int MaxLength = 400;

        public static string SafeSummary(Exception ex)
        {
            if (ex == null) { return "<null exception>"; }
            string type = ex.GetType().Name;
            string message = ex.Message == null ? "" : ex.Message.Replace("\r", " ").Replace("\n", " ").Trim();
            string text = message.Length == 0 ? type : type + ": " + message;
            if (text.Length > MaxLength) { text = text.Substring(0, MaxLength) + " ..."; }
            return text;
        }
    }
}
