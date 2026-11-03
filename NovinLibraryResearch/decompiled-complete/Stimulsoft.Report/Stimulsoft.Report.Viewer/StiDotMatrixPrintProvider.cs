using System;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Print;

namespace Stimulsoft.Report.Viewer;

public class StiDotMatrixPrintProvider
{
	public static void PrintToDotMatrixPrinter(StiReport report)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		PrinterSettings val = new PrinterSettings();
		PrintToDotMatrixPrinter(report, val.PrinterName, Encoding.ASCII);
	}

	public static void PrintToDotMatrixPrinter(StiReport report, string printerName)
	{
		PrintToDotMatrixPrinter(report, printerName, Encoding.ASCII);
	}

	public static void PrintToDotMatrixPrinter(StiReport report, string printerName, Encoding encoding)
	{
		PrintToDotMatrixPrinter(report, printerName, encoding, -1, -1);
	}

	public static void PrintToDotMatrixPrinter(StiReport report, string printerName, Encoding encoding, int fromPage, int toPage)
	{
		PrintToDotMatrixPrinter(report, printerName, encoding, drawBorder: true, StiTxtBorderType.UnicodeSingle, killSpaceLines: true, killSpaceGraphLines: false, putFeedPageCode: true, cutLongLines: false, 1.25f, 1.25f, fromPage, toPage, useEscapeCodes: false, null);
	}

	public static void PrintToDotMatrixPrinter(StiReport report, string printerName, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool putFeedPageCode, bool cutLongLines, int fromPage, int toPage)
	{
		PrintToDotMatrixPrinter(report, printerName, encoding, drawBorder, borderType, killSpaceLines: true, killSpaceGraphLines: false, putFeedPageCode, cutLongLines, 1.25f, 1.25f, fromPage, toPage, useEscapeCodes: false, null);
	}

	public static void PrintToDotMatrixPrinter(StiReport report, string printerName, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool killSpaceLines, bool killSpaceGraphLines, bool putFeedPageCode, bool cutLongLines, float zoomX, float zoomY, int fromPage, int toPage, bool useEscapeCodes, string escapeCodesCollectionName)
	{
		report.InvokePrinting();
		report.RegReportDataSources();
		if (!report.IsRendered)
		{
			report.Render(showProgress: true);
		}
		StiLogService.Write(report.GetType(), "Printing report to dot-matrix printer");
		try
		{
			Print(report.CheckNeedsCompiling() ? report.CompiledReport : report, printerName, encoding, drawBorder, borderType, killSpaceLines, killSpaceGraphLines, putFeedPageCode, cutLongLines, zoomX, zoomY, fromPage, toPage, useEscapeCodes, escapeCodesCollectionName);
		}
		catch (Exception e)
		{
			StiLogService.Write(report.GetType(), "Printing report to dot-matrix printer...ERROR");
			StiLogService.Write(report.GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		report.InvokePrinted();
	}

	public static string GetReportForDotMatrixReport(StiReport report, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool killSpaceLines, bool killSpaceGraphLines, bool putFeedPageCode, bool cutLongLines, float zoomX, float zoomY, StiPagesRange pageRange, bool useEscapeCodes, string escapeCodesCollectionName)
	{
		if (report.CheckNeedsCompiling() && report.CompiledReport == null)
		{
			return string.Empty;
		}
		MemoryStream memoryStream = new MemoryStream();
		StreamReader streamReader = new StreamReader(memoryStream);
		StiTxtExportService stiTxtExportService = new StiTxtExportService();
		stiTxtExportService.ExportTxt(report.CheckNeedsCompiling() ? report.CompiledReport : report, memoryStream, encoding, drawBorder, borderType, killSpaceLines, killSpaceGraphLines, putFeedPageCode, cutLongLines, zoomX, zoomY, pageRange, useEscapeCodes, escapeCodesCollectionName);
		memoryStream.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		memoryStream.Close();
		return result;
	}

	public static void Print(StiReport report, string printerName, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool killSpaceLines, bool killSpaceGraphLines, bool putFeedPageCode, bool cutLongLines, float zoomX, float zoomY, int fromPage, int toPage, bool useEscapeCodes, string escapeCodesCollectionName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		PrintDocument val = new PrintDocument();
		try
		{
			if (!string.IsNullOrEmpty(printerName))
			{
				val.PrinterSettings.PrinterName = printerName;
			}
			int num = report.GetTotalRenderedPageCount();
			if (num < 1)
			{
				num = 1;
			}
			if (toPage == -1 || toPage == 0)
			{
				toPage = num;
			}
			if (fromPage < 1)
			{
				fromPage = 1;
			}
			if (toPage < 1)
			{
				toPage = 1;
			}
			fromPage = Math.Min(fromPage, num);
			toPage = Math.Min(toPage, num);
			val.PrinterSettings.FromPage = fromPage;
			val.PrinterSettings.ToPage = toPage;
			DialogResult val2 = (DialogResult)1;
			if (report.PrinterSettings.ShowDialog)
			{
				PrintDialog val3 = new PrintDialog();
				try
				{
					val3.UseEXDialog = StiPrintProvider.UseEXDialog;
					val3.AllowPrintToFile = false;
					val3.AllowSomePages = true;
					val3.AllowSelection = true;
					val3.Document = val;
					val3.Document.DocumentName = report.ReportAlias;
					val2 = ((CommonDialog)val3).ShowDialog();
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			if ((int)val2 == 1)
			{
				string reportForDotMatrixReport = GetReportForDotMatrixReport(report, null, drawBorder, borderType, killSpaceLines, killSpaceGraphLines, putFeedPageCode, cutLongLines, zoomX, zoomY, new StiPagesRange(StiRangeType.Pages, $"{val.PrinterSettings.FromPage}-{val.PrinterSettings.ToPage}", 0), useEscapeCodes, escapeCodesCollectionName);
				StiRawPrinterHelper.SendStringToPrinter(val.PrinterSettings.PrinterName, report.ReportAlias, reportForDotMatrixReport, encoding);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
