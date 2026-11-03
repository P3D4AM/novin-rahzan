using System.Collections;
using System.Drawing.Printing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;

namespace Stimulsoft.Report;

public sealed class StiResizeReportHelper
{
	private static void SetPageParameters(StiPage page, StiPageOrientation orientation, PaperKind paperSize, StiMargins margins, double pageWidth, double pageHeight, StiResizeReportOptions options)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		bool flag = (options & StiResizeReportOptions.PageOrientationChanged) > (StiResizeReportOptions)0;
		bool flag2 = (options & StiResizeReportOptions.RescaleContent) > (StiResizeReportOptions)0;
		bool allowPageMarginsRescaling = (options & StiResizeReportOptions.AllowPageMarginsRescaling) > (StiResizeReportOptions)0;
		double width = page.Width;
		double height = page.Height;
		if (flag)
		{
			page.Orientation = orientation;
		}
		page.PaperSize = paperSize;
		if (orientation == page.Orientation)
		{
			page.PageWidth = pageWidth;
			page.PageHeight = pageHeight;
			page.Margins = margins.Clone() as StiMargins;
		}
		else
		{
			page.PageWidth = pageHeight;
			page.PageHeight = pageWidth;
			page.Margins = new StiMargins(margins.Top, margins.Bottom, margins.Left, margins.Right);
		}
		if (page.Width <= 0.0)
		{
			page.Width = 0.1;
		}
		if (page.Height <= 0.0)
		{
			page.Height = 0.1;
		}
		if (flag2)
		{
			double factorX = page.Width / width;
			double factorY = page.Height / height;
			page.ResizePage(factorX, factorY, allowPageMarginsRescaling);
		}
		foreach (StiComponent component in page.Components)
		{
			component.DockToContainer();
		}
	}

	public static void ResizeReport(StiReport report, StiPageOrientation orientation, PaperKind paperSize, StiMargins margins, double pageWidth, double pageHeight, StiResizeReportOptions options)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		ResizeReport(report, orientation, paperSize, margins, pageWidth, pageHeight, options, -1);
	}

	public static void ResizeReport(StiReport report, StiPageOrientation orientation, PaperKind paperSize, StiMargins margins, double pageWidth, double pageHeight, StiResizeReportOptions options, int indexOfRenderedPage)
	{
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		bool flag = (options & StiResizeReportOptions.RebuildReport) > (StiResizeReportOptions)0;
		bool flag2 = (options & StiResizeReportOptions.ProcessAllPages) > (StiResizeReportOptions)0;
		bool showProgress = (options & StiResizeReportOptions.ShowProgressOnRebuildReport) > (StiResizeReportOptions)0;
		StiPage stiPage = null;
		if (indexOfRenderedPage < report.RenderedPages.Count && indexOfRenderedPage >= 0)
		{
			stiPage = report.RenderedPages[indexOfRenderedPage];
		}
		string text = ((stiPage != null) ? stiPage.Name : string.Empty);
		if (flag)
		{
			Hashtable hashtable = new Hashtable();
			StiComponentsCollection components = report.GetComponents();
			foreach (StiPage page in report.Pages)
			{
				hashtable[page.Guid] = page;
			}
			Hashtable hashtable2 = new Hashtable();
			foreach (StiComponent item in components)
			{
				if (item is StiSubReport { SubReportPageGuid: not null } stiSubReport && hashtable[stiSubReport.SubReportPageGuid] is StiPage key)
				{
					hashtable2[key] = item.Width;
				}
			}
			foreach (StiPage page2 in report.Pages)
			{
				if (!(page2 is StiForm) && (page2.Name == text || flag2))
				{
					object obj = hashtable2[page2];
					if (obj == null)
					{
						SetPageParameters(page2, orientation, paperSize, margins, pageWidth, pageHeight, options);
					}
					if (!flag2)
					{
						break;
					}
				}
			}
			foreach (StiPage page3 in report.Pages)
			{
				object obj2 = hashtable2[page3];
				if (obj2 != null)
				{
					double num = (double)obj2;
					if (page3.Width != num)
					{
						double pageWidth2 = page3.Width + page3.Margins.Left + page3.Margins.Right;
						page3.Width = num;
						SetPageParameters(page3, orientation, paperSize, margins, pageWidth2, pageHeight, options);
					}
				}
			}
			if (report.SubReports != null && report.SubReports.Count > 0)
			{
				foreach (StiReport subReport in report.SubReports)
				{
					StiReport report2 = subReport;
					if (subReport.CompiledReport != null)
					{
						report2 = subReport.CompiledReport;
					}
					ResizeReport(report2, orientation, paperSize, margins, pageWidth, pageHeight, options);
				}
			}
			report.IsRendered = false;
			report.Render(showProgress);
			report.InvokeRefreshViewer();
			return;
		}
		int num2 = 0;
		foreach (StiPage renderedPage in report.RenderedPages)
		{
			if (!(renderedPage is StiForm) && (num2 == indexOfRenderedPage || flag2))
			{
				SetPageParameters(renderedPage, orientation, paperSize, margins, pageWidth, pageHeight, options);
				if (!flag2)
				{
					break;
				}
			}
			num2++;
		}
	}
}
