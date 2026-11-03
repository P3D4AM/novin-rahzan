using System;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Print;

public class StiPrintProvider
{
	private int resCurrentPrintPage;

	private int resPageNumber;

	private int totalRenderedPageCount;

	private double zoom;

	private StiReport printedReport;

	private int fromPage;

	private int toPage;

	private PaperSource paperSource;

	private PaperSizeCollection paperSizesStore;

	private PaperSourceCollection paperSourcesStore;

	private PaperSource defaultPaperSource;

	private PaperSize allPagesPaperSize;

	private PaperSource allPagesPaperSource;

	public static bool SetPageSettings { get; set; }

	public static bool SetPaperSource { get; set; }

	public static PaperSize PaperSizeForUsing { get; set; }

	public static object LandscapeForUsing { get; set; }

	public static bool UseEXDialog { get; set; }

	public bool KeepCachedPaperSizeAndSource { get; set; }

	public static event StiQueryPageSettingsEventHandler QueryPageSettings;

	public virtual void Print(StiReport report, bool showPrintDialog, int fromPage, int toPage, short copies, PrinterSettings printerSettings)
	{
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Invalid comparison between Unknown and I4
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Invalid comparison between Unknown and I4
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			using MemoryStream memoryStream = new MemoryStream();
			StiReport document = new StiReport();
			try
			{
				StiDashboardExport.ExportDocument(report, memoryStream);
				memoryStream.Position = 0L;
				document.LoadDocument(memoryStream);
				report.RenderedPages.Clear();
				document.RenderedPages.ToList().ForEach(delegate(StiPage p)
				{
					p.Convert(StiUnit.GetUnitFromReportUnit(document.ReportUnit), StiUnit.GetUnitFromReportUnit(report.ReportUnit), isReportSnapshot: true);
					p.Report = report;
					report.RenderedPages.Add(p);
				});
			}
			finally
			{
				if (document != null)
				{
					((IDisposable)document).Dispose();
				}
			}
		}
		try
		{
			printedReport = report;
			if (printerSettings != null)
			{
				paperSource = printerSettings.DefaultPageSettings.PaperSource;
			}
			if (report.RenderedPages.Count <= 0)
			{
				return;
			}
			totalRenderedPageCount = report.GetTotalRenderedPageCount();
			if (totalRenderedPageCount < 1)
			{
				totalRenderedPageCount = 1;
			}
			if (toPage == -1 || toPage == 0)
			{
				toPage = totalRenderedPageCount;
			}
			if (fromPage < 1)
			{
				fromPage = 1;
			}
			if (toPage < 1)
			{
				toPage = 1;
			}
			fromPage = Math.Min(fromPage, totalRenderedPageCount);
			toPage = Math.Min(toPage, totalRenderedPageCount);
			if (!KeepCachedPaperSizeAndSource)
			{
				paperSizesStore = null;
				paperSourcesStore = null;
			}
			defaultPaperSource = null;
			PrintDocument val = new PrintDocument();
			try
			{
				val.QueryPageSettings += new QueryPageSettingsEventHandler(OnQueryPageSettings);
				val.PrintPage += new PrintPageEventHandler(OnPrintPage);
				if (printerSettings == null)
				{
					printerSettings = val.PrinterSettings;
					printerSettings.Collate = report.PrinterSettings.Collate;
					printerSettings.Duplex = report.PrinterSettings.Duplex;
					if (report.PrinterSettings.PrinterName != null)
					{
						foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
						{
							if (installedPrinter == report.PrinterSettings.PrinterName && report.PrinterSettings.PrinterName != null)
							{
								printerSettings.PrinterName = report.PrinterSettings.PrinterName;
								break;
							}
						}
					}
				}
				else if (StiOptions.Print.UsePrinterSettingsEntirely)
				{
					val.PrinterSettings = printerSettings;
				}
				else
				{
					val.PrinterSettings.PrinterName = printerSettings.PrinterName;
					val.PrinterSettings.Collate = printerSettings.Collate;
					val.PrinterSettings.Copies = printerSettings.Copies;
					val.PrinterSettings.Duplex = printerSettings.Duplex;
					val.PrinterSettings.FromPage = printerSettings.FromPage;
					val.PrinterSettings.ToPage = printerSettings.ToPage;
					val.PrinterSettings.PrintRange = printerSettings.PrintRange;
					val.PrinterSettings.PrintToFile = printerSettings.PrintToFile;
					val.PrinterSettings.ToPage = printerSettings.ToPage;
					val.PrinterSettings.DefaultPageSettings.Color = printerSettings.DefaultPageSettings.Color;
					val.PrinterSettings.DefaultPageSettings.PrinterResolution = printerSettings.DefaultPageSettings.PrinterResolution;
					try
					{
						if (!string.IsNullOrEmpty(printerSettings.PrintFileName))
						{
							val.PrinterSettings.PrintFileName = printerSettings.PrintFileName;
						}
					}
					catch
					{
					}
				}
				if (PaperSizeForUsing != null)
				{
					val.DefaultPageSettings.PaperSize = PaperSizeForUsing;
				}
				if (LandscapeForUsing is bool)
				{
					val.DefaultPageSettings.Landscape = (bool)LandscapeForUsing;
				}
				string text2 = report.ReportAlias;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = report.ReportName;
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "Stimulsoft Document";
				}
				val.PrinterSettings.Copies = Math.Max(copies, (short)1);
				val.PrinterSettings.MaximumPage = totalRenderedPageCount;
				val.PrinterSettings.FromPage = fromPage;
				val.PrinterSettings.ToPage = toPage;
				val.DocumentName = text2;
				val.PrintController = (PrintController)new StandardPrintController();
				if (showPrintDialog)
				{
					report.IsStopped = false;
					PrintDialog val2 = new PrintDialog();
					try
					{
						val2.UseEXDialog = UseEXDialog;
						val2.AllowPrintToFile = true;
						val2.AllowSomePages = true;
						val2.Document = val;
						allPagesPaperSize = null;
						allPagesPaperSource = null;
						PaperSize val3 = null;
						PaperSource val4 = null;
						if (StiOptions.Print.UsePaperSizeAndSourceFromPrintDialogIfTheyChanged || StiOptions.Print.UsePaperSizeAndSourceFromPrintDialogAlways)
						{
							val.DefaultPageSettings.PaperSize = printerSettings.DefaultPageSettings.PaperSize;
							val.DefaultPageSettings.PaperSource = printerSettings.DefaultPageSettings.PaperSource;
							val3 = val.DefaultPageSettings.PaperSize;
							val4 = val.DefaultPageSettings.PaperSource;
						}
						report.PrinterSettings.PrintDialogResult = ((CommonDialog)val2).ShowDialog();
						if (copies > 1 && val2.PrinterSettings.Copies == 1 && val2.PrinterSettings.MaximumCopies == 1)
						{
							val2.PrinterSettings.Copies = Math.Max(copies, (short)1);
						}
						if ((int)report.PrinterSettings.PrintDialogResult == 1)
						{
							val2.Document.DocumentName = text2;
							fromPage = val2.PrinterSettings.FromPage;
							toPage = val2.PrinterSettings.ToPage;
							if ((StiOptions.Print.UsePaperSizeAndSourceFromPrintDialogIfTheyChanged && val.DefaultPageSettings.PaperSize != val3) || StiOptions.Print.UsePaperSizeAndSourceFromPrintDialogAlways)
							{
								allPagesPaperSize = val.DefaultPageSettings.PaperSize;
							}
							if ((StiOptions.Print.UsePaperSizeAndSourceFromPrintDialogIfTheyChanged && val.DefaultPageSettings.PaperSource != val4) || StiOptions.Print.UsePaperSizeAndSourceFromPrintDialogAlways)
							{
								allPagesPaperSource = val.DefaultPageSettings.PaperSource;
							}
						}
						else
						{
							report.PrinterSettings.PrintDialogResult = (DialogResult)2;
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				else
				{
					report.PrinterSettings.PrintDialogResult = (DialogResult)0;
				}
				if ((int)report.PrinterSettings.PrintDialogResult == 1 || (int)report.PrinterSettings.PrintDialogResult == 0)
				{
					if (StiOptions.Print.StorePrinterSettingsInReportAfterPrintDialog)
					{
						report.PrinterSettings.Collate = val.PrinterSettings.Collate;
						report.PrinterSettings.Copies = val.PrinterSettings.Copies;
						report.PrinterSettings.Duplex = val.PrinterSettings.Duplex;
						report.PrinterSettings.PrinterName = val.PrinterSettings.PrinterName;
					}
					this.toPage = toPage;
					this.fromPage = fromPage;
					int num = 1;
					int copies2 = val.PrinterSettings.Copies;
					if (copies2 > 1 && val.PrinterSettings.MaximumCopies < copies2)
					{
						num = copies2;
						val.PrinterSettings.Copies = 1;
					}
					for (int i = 0; i < num; i++)
					{
						BeginPrint();
						try
						{
							val.Print();
						}
						catch (Exception ex)
						{
							if (StiExceptionProvider.Show(ex, true, (Form)null))
							{
								throw;
							}
						}
						finally
						{
							StopPrint();
						}
					}
				}
				else
				{
					report.IsStopped = true;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (!KeepCachedPaperSizeAndSource)
			{
				paperSizesStore = null;
				paperSourcesStore = null;
			}
			defaultPaperSource = null;
		}
		catch (Exception ex2)
		{
			if (StiExceptionProvider.Show(ex2, true, (Form)null))
			{
				throw;
			}
		}
	}

	private void BeginPrint()
	{
		if (!printedReport.RenderedPages.CacheMode)
		{
			foreach (StiPage renderedPage in printedReport.RenderedPages)
			{
				renderedPage.IsPrinted = false;
			}
		}
		printedReport.PageNumber = fromPage;
		resCurrentPrintPage = printedReport.CurrentPrintPage;
		resPageNumber = printedReport.PageNumber;
		printedReport.CurrentPrintPage = 0;
	}

	private void StopPrint()
	{
		printedReport.CurrentPrintPage = resCurrentPrintPage;
		printedReport.PageNumber = resPageNumber;
	}

	private object GetPaperSize(StiPage page, QueryPageSettingsEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (PaperSizeForUsing != null)
		{
			return PaperSizeForUsing;
		}
		if (paperSizesStore == null)
		{
			paperSizesStore = e.PageSettings.PrinterSettings.PaperSizes;
		}
		if ((int)page.PaperSize != 0)
		{
			foreach (PaperSize item in paperSizesStore)
			{
				PaperSize val = item;
				if (val.Kind == page.PaperSize)
				{
					return val;
				}
			}
		}
		return GetPaperSize(e, page);
	}

	private object GetPaperSize(QueryPageSettingsEventArgs e, StiPage page)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		int num = (int)Math.Round(page.Unit.ConvertToHInches(page.PageWidth), 0);
		int num2 = (int)Math.Round(page.Unit.ConvertToHInches(page.PageHeight), 0);
		int num3 = num;
		int num4 = num2;
		if (page.Orientation == StiPageOrientation.Landscape)
		{
			num3 = num2;
			num4 = num;
		}
		if (StiOptions.Print.FindCustomPaperSizeInStandardPaperSizes)
		{
			foreach (PaperSize item in paperSizesStore)
			{
				PaperSize val = item;
				if ((val.Width == num && val.Height == num2) || (val.Height == num && val.Width == num2))
				{
					return val;
				}
			}
		}
		return (object)new PaperSize("Custom", num3, num4);
	}

	private static void InvokeQueryPageSettings(StiReport report, StiQueryPageSettingsEventArgs e)
	{
		StiPrintProvider.QueryPageSettings?.Invoke(report, e);
	}

	private void OnPrintPage(object sender, PrintPageEventArgs e)
	{
		if (printedReport.IsStopped)
		{
			e.Cancel = true;
			return;
		}
		printedReport.CurrentPrintPage = printedReport.GetPageIndex(printedReport.PageNumber, out var x, out var y);
		int num = printedReport.CurrentPrintPage;
		if (num < 0)
		{
			num = 0;
		}
		if (num >= printedReport.RenderedPages.Count)
		{
			num = printedReport.RenderedPages.Count - 1;
		}
		StiPrintPageEventArgs e2 = new StiPrintPageEventArgs(num + 1, fromPage, toPage);
		StiOptions.Engine.GlobalEvents.InvokeReportPrintingPage(this, e2);
		StiPage stiPage = printedReport.RenderedPages[num];
		printedReport.RenderedPages.GetPage(stiPage);
		stiPage.CurrentWidthSegment = x;
		stiPage.CurrentHeightSegment = y;
		stiPage.IsPrinted = true;
		zoom = printedReport.Info.Zoom;
		if (stiPage.Report != null)
		{
			stiPage.Report.Info.Zoom = 1.0;
		}
		else
		{
			printedReport.Info.Zoom = 1.0;
		}
		printedReport.IsPrinting = true;
		stiPage.Paint(e.Graphics);
		printedReport.Info.Zoom = zoom;
		printedReport.IsPrinting = false;
		if (toPage >= fromPage)
		{
			printedReport.PageNumber++;
			if (printedReport.PageNumber > toPage || printedReport.PageNumber > totalRenderedPageCount)
			{
				e.HasMorePages = false;
			}
			else
			{
				e.HasMorePages = true;
			}
		}
		else
		{
			printedReport.PageNumber--;
			if (printedReport.PageNumber < toPage || printedReport.PageNumber < 1)
			{
				e.HasMorePages = false;
			}
			else
			{
				e.HasMorePages = true;
			}
		}
		StiOptions.Engine.GlobalEvents.InvokeReportPrintedPage(this, e2);
	}

	private void OnQueryPageSettings(object sender, QueryPageSettingsEventArgs e)
	{
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		int x = 0;
		int y = 0;
		int num = printedReport.GetPageIndex(printedReport.PageNumber, out x, out y);
		if (num < 0)
		{
			num = 0;
		}
		printedReport.CurrentPrintPage = num;
		StiPage stiPage = printedReport.RenderedPages[printedReport.CurrentPrintPage];
		printedReport.RenderedPages.GetPage(stiPage);
		e.PageSettings.Landscape = stiPage.Orientation == StiPageOrientation.Landscape;
		bool flag = false;
		bool flag2 = false;
		if (paperSourcesStore == null)
		{
			paperSourcesStore = e.PageSettings.PrinterSettings.PaperSources;
		}
		if (stiPage.PaperSourceOfFirstPage != null && stiPage.PaperSourceOfFirstPage.Trim().Length != 0 && (printedReport.CurrentPrintPage == 0 || printedReport.RenderedPages[printedReport.CurrentPrintPage].Name != printedReport.RenderedPages[printedReport.CurrentPrintPage - 1].Name))
		{
			foreach (PaperSource item in paperSourcesStore)
			{
				PaperSource val = item;
				if (val.SourceName == stiPage.PaperSourceOfFirstPage)
				{
					e.PageSettings.PaperSource = val;
					flag = true;
					flag2 = true;
					break;
				}
			}
		}
		if (stiPage.PaperSourceOfOtherPages != null && stiPage.PaperSourceOfOtherPages.Trim().Length != 0 && e.PageSettings.PaperSource != null && !flag)
		{
			foreach (PaperSource item2 in paperSourcesStore)
			{
				PaperSource val2 = item2;
				if (val2.SourceName == stiPage.PaperSourceOfOtherPages)
				{
					e.PageSettings.PaperSource = val2;
					flag2 = true;
					break;
				}
			}
		}
		if (defaultPaperSource == null)
		{
			defaultPaperSource = e.PageSettings.PrinterSettings.DefaultPageSettings.PaperSource;
		}
		if (!flag2)
		{
			e.PageSettings.PaperSource = defaultPaperSource;
		}
		object paperSize = GetPaperSize(stiPage, e);
		StiQueryPageSettingsEventArgs stiQueryPageSettingsEventArgs = new StiQueryPageSettingsEventArgs(paperSize, paperSource, e.PageSettings);
		InvokeQueryPageSettings(printedReport, stiQueryPageSettingsEventArgs);
		paperSize = stiQueryPageSettingsEventArgs.PaperSize;
		paperSource = stiQueryPageSettingsEventArgs.PaperSource;
		if (SetPaperSource && paperSource != null)
		{
			e.PageSettings.PaperSource = paperSource;
		}
		if (SetPageSettings && paperSize != null)
		{
			e.PageSettings.PaperSize = (PaperSize)paperSize;
		}
		if (allPagesPaperSize != null)
		{
			e.PageSettings.PaperSize = allPagesPaperSize;
		}
		if (allPagesPaperSource != null)
		{
			e.PageSettings.PaperSource = allPagesPaperSource;
		}
	}

	static StiPrintProvider()
	{
		SetPageSettings = true;
		SetPaperSource = true;
		bool flag = IntPtr.Size == 8;
		UseEXDialog |= flag;
	}
}
