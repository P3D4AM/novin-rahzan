using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Stimulsoft.Report.Viewer;

[SuppressUnmanagedCodeSecurity]
public sealed class StiRawPrinterHelper
{
	private struct DOC_INFO_1
	{
		[MarshalAs(UnmanagedType.LPTStr)]
		public string pDocName;

		[MarshalAs(UnmanagedType.LPTStr)]
		public string pOutputFile;

		[MarshalAs(UnmanagedType.LPTStr)]
		public string pDataType;
	}

	[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

	[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern int StartDocPrinter(IntPtr hPrinter, int Level, ref DOC_INFO_1 pDocInfo);

	[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool StartPagePrinter(IntPtr hPrinter);

	[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	private static extern bool WritePrinter(IntPtr hPrinter, byte[] data, int buf, ref int pcWritten);

	[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool EndPagePrinter(IntPtr hPrinter);

	[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool EndDocPrinter(IntPtr hPrinter);

	[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool ClosePrinter(IntPtr hPrinter);

	public static void SendStringToPrinter(string printerName, string docName, string text)
	{
		if (!string.IsNullOrEmpty(StiOptions.Print.DotMatrixFormatSequence))
		{
			SendStringToPrinter(printerName, docName, StiOptions.Print.DotMatrixFormatSequence, Encoding.ASCII);
		}
		SendStringToPrinter(printerName, docName, text, Encoding.ASCII);
	}

	public static void SendStringToPrinter(string printerName, string docName, string text, Encoding encoding)
	{
		IntPtr phPrinter = IntPtr.Zero;
		DOC_INFO_1 pDocInfo = default(DOC_INFO_1);
		pDocInfo.pDataType = "RAW";
		if (!OpenPrinter(printerName, ref phPrinter, 0))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		pDocInfo.pDocName = ((docName == null) ? "" : docName);
		if (StartDocPrinter(phPrinter, 1, ref pDocInfo) == 0)
		{
			ClosePrinter(phPrinter);
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		if (!StartPagePrinter(phPrinter))
		{
			EndDocPrinter(phPrinter);
			ClosePrinter(phPrinter);
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		int pcWritten = 0;
		byte[] bytes = encoding.GetBytes(text);
		if (!WritePrinter(phPrinter, bytes, bytes.Length, ref pcWritten))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		EndPagePrinter(phPrinter);
		EndDocPrinter(phPrinter);
		ClosePrinter(phPrinter);
	}
}
