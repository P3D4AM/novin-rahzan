using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiProgressHelperV2
{
	private List<StiDataBand> cachedDataBands;

	private Hashtable cachedDataBandsValues;

	private Hashtable cachedPages;

	private decimal latestProgress;

	private decimal latestProgressValue;

	internal StiEngine Engine { get; }

	private string FinishingReportString
	{
		get
		{
			if (!(StiLocalization.CultureName == "en"))
			{
				return StiLocalization.Get("Report", "FinishingReport");
			}
			return "Finishing Report";
		}
	}

	private string FirstPassString
	{
		get
		{
			if (!(StiLocalization.CultureName == "en"))
			{
				return StiLocalization.Get("Report", "FirstPass");
			}
			return "First Pass";
		}
	}

	private string SecondPassString
	{
		get
		{
			if (!(StiLocalization.CultureName == "en"))
			{
				return StiLocalization.Get("Report", "SecondPass");
			}
			return "Second Pass";
		}
	}

	public bool AllowCachedPagesCache { get; set; }

	internal void Dispose()
	{
		if (cachedDataBands != null)
		{
			cachedDataBands.Clear();
		}
		cachedDataBands = null;
		if (cachedDataBandsValues != null)
		{
			cachedDataBandsValues.Clear();
		}
		cachedDataBandsValues = null;
		if (cachedPages != null)
		{
			cachedPages.Clear();
		}
		cachedPages = null;
	}

	private void ScanReport()
	{
		if (Engine == null || cachedDataBands != null)
		{
			return;
		}
		StiReport report = Engine.Report;
		if (report == null)
		{
			return;
		}
		cachedDataBands = new List<StiDataBand>();
		cachedDataBandsValues = new Hashtable();
		foreach (StiPage page in report.Pages)
		{
			if (page.Skip)
			{
				continue;
			}
			StiComponentsCollection components = page.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item is StiDataBand { MasterComponent: null } stiDataBand)
				{
					cachedDataBands.Add(stiDataBand);
					cachedDataBandsValues[stiDataBand] = 0;
				}
			}
		}
	}

	internal void ProcessInCache(StiPage page)
	{
		if (AllowCachedPagesCache && page != null && page.Report != null && page.Report.RenderedPages != null)
		{
			if (cachedPages == null)
			{
				cachedPages = new Hashtable();
			}
			StiReport report = Engine.Report;
			cachedPages[page] = page;
			int num = -1;
			decimal num2 = page.Report.RenderedPages.Count;
			decimal num3 = cachedPages.Count;
			num3 /= num2;
			num3 = num3 * (100m - latestProgress) + latestProgress;
			report.ProgressOfRendering = (int)num3;
			num = (int)num3;
			report.StatusString = $"{FinishingReportString}: {100 * cachedPages.Count / page.Report.RenderedPages.Count}%";
			UpdateReportProgress(report, num);
			CheckProgressBreak(report);
			UpdateParentReportProgress(report);
		}
	}

	internal void Process()
	{
		if (AllowCachedPagesCache)
		{
			return;
		}
		StiReport stiReport = Engine.Report;
		if (stiReport.CompiledReport != null)
		{
			stiReport = stiReport.CompiledReport;
		}
		int progressValue = -1;
		string text = string.Empty;
		if (stiReport.ReportPass == StiReportPass.First)
		{
			text = FirstPassString;
		}
		if (stiReport.ReportPass == StiReportPass.Second)
		{
			text = SecondPassString;
		}
		if (StiOptions.Engine.UsePercentageProgress)
		{
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			ScanReport();
			if (cachedDataBands != null)
			{
				foreach (StiDataBand cachedDataBand in cachedDataBands)
				{
					num += (decimal)cachedDataBand.Count;
					int num3 = cachedDataBand.Position;
					if (cachedDataBand.DataSource != null)
					{
						if (cachedDataBand.DataSource.NameOfDataBandWhichInitDataSource == cachedDataBand.Name)
						{
							cachedDataBandsValues[cachedDataBand] = num3;
						}
						else
						{
							object obj = cachedDataBandsValues[cachedDataBand];
							if (obj is int)
							{
								num3 = (int)obj;
							}
						}
					}
					if (num3 <= cachedDataBand.Count)
					{
						num2 += (decimal)num3;
					}
					else
					{
						num2 += (decimal)cachedDataBand.Count;
					}
				}
			}
			if (num > 0m)
			{
				if (stiReport.ReportCacheMode == StiReportCacheMode.On)
				{
					num = (int)((double)(float)num * 1.3);
				}
				if (stiReport.NumberOfPass == StiNumberOfPass.DoublePass)
				{
					num *= 2m;
				}
				num2 = num2 / num * 100m;
			}
			else
			{
				num2 = 100m;
			}
			num2 = (latestProgressValue = Math.Max(latestProgressValue, num2));
			if (stiReport.ReportPass != StiReportPass.Second)
			{
				Engine.LatestProgressValue = num2;
			}
			else
			{
				num2 += Engine.LatestProgressValue;
			}
			latestProgress = num2;
			progressValue = (stiReport.ProgressOfRendering = (int)num2);
		}
		string text2 = ((StiLocalization.CultureName == "en") ? $"Page - {stiReport.CurrentPrintPage + 1}" : string.Format("{0} - {1}", StiLocalization.Get("Components", "StiPage"), stiReport.CurrentPrintPage + 1));
		stiReport.StatusString = ((stiReport.ReportPass != 0) ? (text + ": " + text2) : text2);
		UpdateReportProgress(stiReport, progressValue);
		CheckProgressBreak(stiReport);
		UpdateParentReportProgress(stiReport);
	}

	private static void CheckProgressBreak(StiReport report)
	{
		if (report.Progress == null || !report.Progress.IsBreaked)
		{
			return;
		}
		report.IsStopped = true;
		throw new StiReportRenderingStopException();
	}

	private static void UpdateReportProgress(StiReport report, int progressValue)
	{
		if (report.Progress == null)
		{
			return;
		}
		if (progressValue != -1)
		{
			if (progressValue > 100)
			{
				progressValue = 100;
			}
			report.Progress.IsMarquee = false;
			report.Progress.Update(report.StatusString, progressValue);
		}
		else
		{
			report.Progress.Update(report.StatusString);
		}
	}

	private static void UpdateParentReportProgress(StiReport report)
	{
		if (report.ParentReport != null && report.ParentReport.Progress != null)
		{
			report.ParentReport.Progress.Update(report.ParentReport.StatusString);
			if (report.ParentReport.Progress.IsBreaked)
			{
				report.ParentReport.IsStopped = true;
				throw new StiReportRenderingStopException();
			}
		}
	}

	internal StiProgressHelperV2(StiEngine engine)
	{
		Engine = engine;
	}
}
