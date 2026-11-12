using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Panel;
using Stimulsoft.Dashboard.Export.Helpers;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Export.Tools;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Dashboard.Export;

public static class StiDashboardExportTools
{
	public static bool ExportToFile(IStiReport report, string path, StiDashboardExportSettings settings)
	{
		byte[] array = ExportToBytes(report, settings, showProgress: false);
		if (array != null)
		{
			File.WriteAllBytes(path, array);
			return true;
		}
		return false;
	}

	public static bool ExportToFile(IStiReport report, string path, StiDashboardExportSettings settings, bool showProgress)
	{
		byte[] array = ExportToBytes(report, settings, showProgress);
		if (array != null)
		{
			File.WriteAllBytes(path, array);
			return true;
		}
		return false;
	}

	public static bool ExportToFile(IStiElement element, string path, StiDashboardExportSettings settings)
	{
		byte[] array = ExportToBytes(element, settings, showProgress: false);
		if (array != null)
		{
			File.WriteAllBytes(path, array);
			return true;
		}
		return false;
	}

	public static bool ExportToFile(IStiElement element, string path, StiDashboardExportSettings settings, bool showProgress)
	{
		byte[] array = ExportToBytes(element, settings, showProgress);
		if (array != null)
		{
			File.WriteAllBytes(path, array);
			return true;
		}
		return false;
	}

	public static byte[] ExportToBytes(IStiReport report, StiDashboardExportSettings settings)
	{
		using Stream stream = ExportToStream(report, settings, showProgress: false);
		return ((MemoryStream)stream).ToArray();
	}

	public static byte[] ExportToBytes(IStiReport report, StiDashboardExportSettings settings, bool showProgress)
	{
		using Stream stream = ExportToStream(report, settings, showProgress);
		if (stream != null)
		{
			return ((MemoryStream)stream).ToArray();
		}
		return null;
	}

	public static byte[] ExportToBytes(IStiElement element, StiDashboardExportSettings settings)
	{
		using Stream stream = ExportToStream(element, settings, showProgress: false);
		return ((MemoryStream)stream).ToArray();
	}

	public static byte[] ExportToBytes(IStiElement element, StiDashboardExportSettings settings, bool showProgress)
	{
		using Stream stream = ExportToStream(element, settings, showProgress);
		if (stream != null)
		{
			return ((MemoryStream)stream).ToArray();
		}
		return null;
	}

	public static Stream ExportToStream(IStiReport report, StiDashboardExportSettings settings)
	{
		return ExportToStream((IStiAppCell)report, settings, showProgress: false);
	}

	public static Stream ExportToStream(IStiReport report, StiDashboardExportSettings settings, bool showProgress)
	{
		return ExportToStream((IStiAppCell)report, settings, showProgress);
	}

	public static Stream ExportToStream(IStiElement element, StiDashboardExportSettings settings)
	{
		return ExportToStream((IStiAppCell)element, settings, showProgress: false);
	}

	public static Stream ExportToStream(IStiElement element, StiDashboardExportSettings settings, bool showProgress)
	{
		return ExportToStream((IStiAppCell)element, settings, showProgress);
	}

	private static Stream ExportToStream(IStiAppCell cell, StiDashboardExportSettings settings, bool showProgress)
	{
		StiReport report = GetReport(cell);
		StiExportFormat stiExportFormat = ((settings == null) ? StiExportFormat.Document : StiExportFormatHelper.Convert(settings.Format));
		try
		{
			report?.InvokeExporting(stiExportFormat);
			if (!showProgress)
			{
				return ExportToStream(cell, settings, null);
			}
			StiGuiMode guiMode = (((cell is IStiReport && ((StiReport)cell).IsWpf) || (cell is IStiDashboard && ((IStiDashboard)cell).Report != null && ((IStiDashboard)cell).Report.IsWpf) || (cell is IStiElement && ((IStiElement)cell).Report != null && ((IStiElement)cell).Report.IsWpf)) ? StiGuiMode.Wpf : StiGuiMode.Gdi);
			StiExportService exportService = null;
			switch (stiExportFormat)
			{
			case StiExportFormat.Pdf:
				exportService = new StiPdfExportService();
				break;
			case StiExportFormat.Excel2007:
				exportService = new StiExcel2007ExportService();
				break;
			case StiExportFormat.Html:
				exportService = new StiHtmlExportService();
				break;
			case StiExportFormat.Data:
				exportService = new StiDataExportService();
				break;
			case StiExportFormat.Image:
				exportService = new StiImageExportService();
				break;
			}
			IStiProgressInformation progressInformation = StiGuiOptions.GetProgressInformation(guiMode);
			progressInformation.IsMarquee = true;
			progressInformation.Start(Loc.Get("Buttons", "Export"));
			progressInformation.Update(Loc.Get("Export", "Exporting"));
			if (exportService != null)
			{
				exportService.Progress = progressInformation;
			}
			Stream stream = null;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate
			{
				stream = ExportToStream(cell, settings, exportService);
			};
			backgroundWorker.RunWorkerAsync();
			while (backgroundWorker.IsBusy)
			{
				Application.DoEvents();
				if (progressInformation.IsBreaked)
				{
					if (exportService != null)
					{
						exportService.IsStopped = true;
					}
					progressInformation.Update(Loc.Get("Export", "CancelExport"));
					progressInformation.SetAllowClose(allows: false);
				}
			}
			if (progressInformation.IsBreaked)
			{
				stream = null;
			}
			backgroundWorker.Dispose();
			progressInformation.Close();
			if (exportService != null)
			{
				exportService.Progress = null;
			}
			return stream;
		}
		finally
		{
			report?.InvokeExported(stiExportFormat);
		}
	}

	private static Stream ExportToStream(IStiAppCell cell, StiDashboardExportSettings settings, StiExportService exportService)
	{
		using StiReport stiReport = new StiReport();
		StiReport owner = GetOwner(cell);
		try
		{
			if (owner != null)
			{
				owner.IsExporting = true;
			}
			stiReport.IsDocument = true;
			stiReport.ReportUnit = StiReportUnitType.HundredthsOfInch;
			StiPage page = stiReport.RenderedPages[0];
			if (cell is IStiReport)
			{
				RenderCellReport(cell, stiReport, page, settings);
			}
			else if (cell is IStiDashboard)
			{
				RenderCellDashboard(cell, page, settings);
			}
			else if (cell is IStiElement)
			{
				RenderCellElement(cell, stiReport, page, settings);
			}
			MoveAllComponentsToPages(stiReport);
			return CreateResultDocument(stiReport, exportService, settings);
		}
		finally
		{
			if (owner != null)
			{
				owner.IsExporting = false;
			}
		}
	}

	private static StiReport GetOwner(IStiAppCell cell)
	{
		if (cell is IStiElement)
		{
			return (cell as IStiElement).Report;
		}
		if (cell is StiReport)
		{
			return cell as StiReport;
		}
		return null;
	}

	private static void SetUpPage(StiPage page, IStiAppCell cell, StiDashboardExportSettings settings)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		if (settings is StiPdfDashboardExportSettings)
		{
			page.Orientation = ((StiPdfDashboardExportSettings)settings).Orientation;
			page.PaperSize = ((StiPdfDashboardExportSettings)settings).PaperSize;
		}
		if (settings is StiExcelDashboardExportSettings || settings is StiDataDashboardExportSettings || settings is StiHtmlDashboardExportSettings || settings is StiImageDashboardExportSettings)
		{
			page.Margins = new StiMargins(0.0);
		}
		if (settings is IStiSizeExportSettings { Width: >0.0, Height: >0.0 } stiSizeExportSettings)
		{
			page.Width = stiSizeExportSettings.Width;
			page.Height = stiSizeExportSettings.Height;
		}
		else if (!(settings is StiPdfDashboardExportSettings) && !(settings is StiDataDashboardExportSettings))
		{
			page.Width = ((StiComponent)cell).Width;
			page.Height = ((StiComponent)cell).Height;
		}
		StiAdvancedWatermark stiAdvancedWatermark = (StiAdvancedWatermark)(page.TagValue = (cell as IStiDashboardWatermark)?.DashboardWatermark);
		if (page.TagValue == null)
		{
			page.TagValue = "*Dashboards*";
		}
		if (stiAdvancedWatermark != null)
		{
			if (stiAdvancedWatermark.TextEnabled)
			{
				page.Watermark.Text = stiAdvancedWatermark.Text;
				page.Watermark.TextBrush = new StiSolidBrush(stiAdvancedWatermark.TextColor);
				page.Watermark.Font = stiAdvancedWatermark.TextFont;
				page.Watermark.Angle = stiAdvancedWatermark.TextAngle;
				page.Watermark.ShowBehind = true;
			}
			if (stiAdvancedWatermark.ImageEnabled)
			{
				page.Watermark.ImageBytes = stiAdvancedWatermark.ImageBytes;
				page.Watermark.AspectRatio = stiAdvancedWatermark.ImageAspectRatio;
				page.Watermark.ImageAlignment = stiAdvancedWatermark.ImageAlignment;
				page.Watermark.ImageMultipleFactor = stiAdvancedWatermark.ImageMultipleFactor;
				page.Watermark.ImageStretch = stiAdvancedWatermark.ImageStretch;
				page.Watermark.ImageTiling = stiAdvancedWatermark.ImageTiling;
				page.Watermark.ImageTransparency = stiAdvancedWatermark.ImageTransparency;
				page.Watermark.ShowImageBehind = true;
			}
		}
	}

	private static void RenderCellReport(IStiAppCell cell, StiReport document, StiPage page, StiDashboardExportSettings settings)
	{
		List<IStiDashboard> list = (from p in ((IStiReport)cell).FetchPages()
			where p is IStiDashboard
			select p).Cast<IStiDashboard>().ToList();
		for (int i = 0; i < list.Count; i++)
		{
			if (i > 0)
			{
				page = (StiPage)page.Clone(cloneProperties: true, cloneComponents: false);
				document.RenderedPages.Add(page);
			}
			SetUpPage(page, list[i], settings);
			RenderDashboard(page, list[i], settings);
		}
	}

	private static void RenderCellDashboard(IStiAppCell cell, StiPage page, StiDashboardExportSettings settings)
	{
		SetUpPage(page, cell, settings);
		RenderDashboard(page, (IStiDashboard)cell, settings);
	}

	private static void RenderCellElement(IStiAppCell cell, StiReport document, StiPage page, StiDashboardExportSettings settings)
	{
		document.ReportAlias = ((IStiElement)cell).Name;
		SetUpPage(page, cell, settings);
		RenderSingleElement(page, (IStiElement)cell, settings);
	}

	private static MemoryStream CreateResultDocument(StiReport document, StiExportService service, StiDashboardExportSettings settings)
	{
		MemoryStream memoryStream = new MemoryStream();
		StiExportFormat stiExportFormat = ((settings == null) ? StiExportFormat.Document : StiExportFormatHelper.Convert(settings.Format));
		if (stiExportFormat == StiExportFormat.Document)
		{
			document.SaveDocument(memoryStream);
		}
		else
		{
			StiExportSettings exportSettings = StiExportSettingsHelper.GetExportSettings(settings);
			if (stiExportFormat == StiExportFormat.Pdf && ((StiPdfDashboardExportSettings)settings).AutoPrint)
			{
				((StiPdfExportSettings)exportSettings).AutoPrintMode = StiPdfAutoPrintMode.Dialog;
			}
			document.ExportDocument(stiExportFormat, service, memoryStream, exportSettings);
		}
		return memoryStream;
	}

	private static void MoveAllComponentsToPages(StiReport document)
	{
		foreach (StiPage finalPage in document.RenderedPages)
		{
			(from comp in finalPage.Components.ToList()
				where comp is StiContainer
				select comp).Cast<StiContainer>().ToList().ForEach(delegate(StiContainer container)
			{
				int num = finalPage.Components.IndexOf(container) + 1;
				List<StiComponent> componentsList = container.GetComponentsList();
				for (int i = 0; i < componentsList.Count; i++)
				{
					componentsList[i].Page = finalPage;
					componentsList[i].SetDirectDisplayRectangle(componentsList[i].ComponentToPage(componentsList[i].DisplayRectangle));
					finalPage.Components.Insert(num + i, componentsList[i]);
				}
				container.Components.Clear();
			});
		}
	}

	private static void RenderDashboard(StiPage page, IStiDashboard dashboardElement, StiDashboardExportSettings settings)
	{
		if (dashboardElement != null)
		{
			if (settings != null)
			{
				settings.RenderSinglePage = true;
				settings.RenderSingleElement = false;
			}
			StiDashboard stiDashboard = dashboardElement as StiDashboard;
			IEnumerable<IStiElement> elements = from c in stiDashboard.GetElements(nested: false)
				where c.IsEnabled
				select c;
			double scaleX = page.Width / stiDashboard.Width;
			double scaleY = page.Height / stiDashboard.Height;
			page.Brush = new StiSolidBrush(StiDashboardStyleHelper.GetDashboardBackColor(dashboardElement, isViewer: false));
			RenderElements(page, elements, scaleX, scaleY, settings);
		}
	}

	internal static void RenderElements(StiContainer parent, IEnumerable<IStiElement> elements, double scaleX, double scaleY, StiDashboardExportSettings settings)
	{
		if (settings != null)
		{
			settings.RenderSingleElement = false;
		}
		foreach (IStiElement element in elements)
		{
			if (element is IStiFilterElement stiFilterElement && settings.ApplyDefaultFilters)
			{
				stiFilterElement.ApplyDefaultFilters();
			}
			StiComponent stiComponent = RenderElement(parent, element, scaleX, scaleY, settings);
			if (element is StiPanelElement stiPanelElement)
			{
				IEnumerable<IStiElement> elements2 = (from c in stiPanelElement.GetComponents().ToList()
					where c is IStiElement
					select c).Cast<IStiElement>();
				RenderElements(stiComponent as StiContainer, elements2, scaleX, scaleY, settings);
			}
		}
	}

	private static void RenderSingleElement(StiPage page, IStiElement element, StiDashboardExportSettings settings)
	{
		if (element != null && element.IsEnabled)
		{
			if (settings != null)
			{
				settings.RenderSingleElement = true;
			}
			bool flag = StiExportSettingsHelper.IsDataExport(settings);
			if (!flag)
			{
				page.Brush = new StiSolidBrush(StiDashboardStyleHelper.GetBackColor(element));
			}
			StiComponent stiComponent = element as StiComponent;
			double scaleX = page.Width / stiComponent.Width;
			double scaleY = page.Height / stiComponent.Height;
			StiComponent stiComponent2 = RenderElement(page, element, scaleX, scaleY, settings);
			stiComponent2.Left = 0.0;
			stiComponent2.Top = 0.0;
			if (!flag)
			{
				StiMargin margin = ((IStiMargin)element).Margin;
				stiComponent2.Left = margin.Left;
				stiComponent2.Top = margin.Top;
			}
		}
	}

	public static StiComponent RenderElement(StiContainer parent, IStiElement element, double scaleX, double scaleY, StiDashboardExportSettings settings)
	{
		return RenderElement(parent, element, scaleX, scaleY, settings, null, new List<StiAnimation>());
	}

	internal static StiComponent RenderElement(StiContainer parent, IStiElement element, double scaleX, double scaleY, StiDashboardExportSettings settings, List<double?[]> chartStartValues, List<StiAnimation> chartAnimations)
	{
		StiComponent stiComponent = element as StiComponent;
		bool flag = StiExportSettingsHelper.IsDataExport(settings);
		double num = Math.Round(stiComponent.Left * scaleX, MidpointRounding.AwayFromZero);
		double num2 = Math.Round(stiComponent.Top * scaleY, MidpointRounding.AwayFromZero);
		double num3 = Math.Round(stiComponent.Right * scaleX, MidpointRounding.AwayFromZero);
		double num4 = Math.Round(stiComponent.Bottom * scaleY, MidpointRounding.AwayFromZero);
		RectangleD rect = new RectangleD(num, num2, num3 - num, num4 - num2);
		if (!flag)
		{
			rect = StiLayoutExportHelper.ExcludeMargin(element, rect);
		}
		if (stiComponent.Parent is StiPanelElement)
		{
			IStiMargin stiMargin = stiComponent.Parent as IStiMargin;
			rect.X -= stiMargin.Margin.Left;
			rect.Y -= stiMargin.Margin.Top;
		}
		StiPanel stiPanel = new StiPanel(rect);
		stiPanel.Name = element.Name;
		bool allowOpacity = settings is StiImageDashboardExportSettings || settings is StiHtmlDashboardExportSettings || settings is StiPdfDashboardExportSettings;
		if (!flag)
		{
			stiPanel.Brush = new StiSolidBrush(StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity));
		}
		parent.Components.Add(stiPanel);
		if (stiPanel.Report != null && element.Report != null)
		{
			stiPanel.Report.Styles.Clear();
			stiPanel.Report.Styles.AddRange(element.Report.Styles);
		}
		rect = StiTitleExportHelper.Render(element, stiPanel, settings);
		if (!flag)
		{
			StiBorderExportHelper.Render(element, stiPanel, settings);
			StiBackColorExportHelper.Render(element, stiPanel);
			rect = StiLayoutExportHelper.ExcludePadding(element, rect);
			rect = StiLayoutExportHelper.ExcludeCornerRadius(element, rect);
		}
		StiExportTool stiExportTool = StiExportTool.GetTool(element.GetType());
		if (stiExportTool == null)
		{
			stiExportTool = new StiElementExportTool();
		}
		if (stiExportTool is StiChartElementExportTool)
		{
			((IStiChartElement)element).PreviousAnimations = chartAnimations;
			((StiChartElementExportTool)stiExportTool).Render(element, stiPanel, rect, settings, chartStartValues);
		}
		else if (stiExportTool is StiGaugeElementExportTool)
		{
			((IStiGaugeElement)element).PreviousAnimations = chartAnimations;
			((StiGaugeElementExportTool)stiExportTool).Render(element, stiPanel, rect, settings);
		}
		else if (!settings.IsDesignMode || !(element is IStiOnlineMapElement))
		{
			stiExportTool.Render(element, stiPanel, rect, settings);
		}
		return stiPanel;
	}

	private static StiReport GetReport(IStiAppCell cell)
	{
		if (cell is IStiReport)
		{
			return cell as StiReport;
		}
		if (cell is IStiDashboard)
		{
			return (cell as IStiDashboard).Report;
		}
		if (cell is IStiElement)
		{
			return ((IStiElement)cell).Report;
		}
		return null;
	}
}
