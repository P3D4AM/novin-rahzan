using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Engine;

public sealed class StiRenderProviderV1
{
	public static void Render(StiReport report, StiRenderState state)
	{
		InitReport(report);
		if (!CheckDialogsInPreview(report))
		{
			ConnectToData(report);
			StiVariableHelper.FillItemsOfVariables((report.CompiledReport != null) ? report.CompiledReport : report);
			return;
		}
		StiNumberOfPass numberOfPass = GetNumberOfPass(report);
		bool flag = IsDialogsOnStartExist(report);
		if (!flag)
		{
			RenderFirstPass(report, numberOfPass);
		}
		StiReport subReportsMasterReport = report.SubReportsMasterReport;
		try
		{
			Hashtable hashtable = PrepareSubReportsAndDrillDownPages(report);
			if (subReportsMasterReport == null)
			{
				report.RenderedPages.Clear();
			}
			report.InvokeBeginRender();
			ClearTotals(report);
			ConnectToData(report);
			StiVariableHelper.FillItemsOfVariables((report.CompiledReport != null) ? report.CompiledReport : report);
			if (RenderFormsOnStart(report))
			{
				if (flag)
				{
					DisconnectFromData(report);
					RenderFirstPass(report, numberOfPass);
					ConnectToData(report);
				}
				if (subReportsMasterReport == null)
				{
					report.PageNumber = 1;
					report.EngineV1.RealPageNumber = 1;
				}
				else
				{
					report.PageNumber = subReportsMasterReport.PageNumber;
					report.EngineV1.RealPageNumber = subReportsMasterReport.EngineV1.RealPageNumber;
				}
				report.Bookmark.Bookmarks.Clear();
				report.Bookmark.Text = report.ReportAlias;
				if (report.Progress != null)
				{
					report.Progress.ShowProgressBar();
				}
				InitCacheMode(report);
				RenderReport(report, subReportsMasterReport, state);
				RenderFormsOnEnd(report);
				report.IsRendered = true;
			}
		}
		catch (Exception e)
		{
			report.IsStopped = true;
			StiLogService.Write(typeof(StiRenderProviderV1), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			foreach (StiPage page in report.Pages)
			{
				page.ProcessPageAfterRender();
			}
			report.CurrentPage = 0;
			report.CurrentPrintPage = 0;
			if (subReportsMasterReport == null && report.ReportPass != StiReportPass.First)
			{
				report.PageNumber = 1;
				report.EngineV1.RealPageNumber = 1;
				if (!report.RenderedPages.CacheMode)
				{
					StiPostProcessProviderV1.PostProcessPages(report.RenderedPages);
				}
				else if (report.RenderedPages.Count > 0)
				{
					StiPage stiPage2 = report.RenderedPages[report.RenderedPages.Count - 1];
					StiPostProcessProviderV1.PostProcessPage(stiPage2, stiPage2.PageInfoV1.PageNumber == 1, isLastPage: true);
				}
			}
			FinishAllPagesInNotCachedPagesArray(report);
			report.RenderedPages.ClearAllStates();
			RemoveAllPagesLessThenFromPageAndGreaterThenToPage(report, state);
			MadeCollate(report);
			if (report.ManualBookmark.Bookmarks.Count > 0)
			{
				report.bookmarkValue = report.ManualBookmark;
				report.ManualBookmark = null;
			}
			StiBookmarksV1Helper.PrepareBookmark(report.Bookmark);
			StiBookmarksV1Helper.Pack(report.Bookmark);
			if (StiOptions.Engine.DisconnectFromDataBeforeEndRender)
			{
				DisconnectFromData(report);
			}
			report.EngineV1.ProgressHelper.AllowCachedPagesCache = report.ReportPass != StiReportPass.First;
			if (subReportsMasterReport == null)
			{
				if (report.RenderedPages.Count == 0)
				{
					report.RenderedPages.Add(StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage", new object[1] { report }) as StiPage);
				}
				report.EngineV1.ProcessResetPageNumber(report);
				if (report.ReportPass != StiReportPass.First && report.EngineV1.ReportTotalPageCounts != null && report.EngineV1.ReportTotalPageCounts.Count > 0 && !report.RenderedPages.CacheMode)
				{
					int i;
					for (i = 0; i < report.EngineV1.ReportTotalPageCounts.Count; i++)
					{
						int num = (int)report.EngineV1.ReportTotalPageCounts[0];
						int num2 = (int)report.EngineV1.ReportTotalPageCounts[i];
						if (num != num2)
						{
							break;
						}
					}
					if (i == report.EngineV1.ReportTotalPageCounts.Count)
					{
						int totalRenderedPageCount = report.GetTotalRenderedPageCount();
						int num3 = (int)report.EngineV1.ReportTotalPageCounts[0];
						if (num3 != totalRenderedPageCount)
						{
							for (i = 0; i < report.EngineV1.ReportTotalPageCounts.Count; i++)
							{
								report.EngineV1.ReportTotalPageCounts[i] = report.RenderedPages.Count;
							}
						}
					}
				}
				report.InvokeEndRender();
				report.EngineV1.PageInProgress = null;
			}
			if (!StiOptions.Engine.DisconnectFromDataBeforeEndRender)
			{
				DisconnectFromData(report);
			}
			if (report.ReportPass == StiReportPass.Second)
			{
				report.ReportPass = StiReportPass.None;
			}
			FinishProgressForm(report);
			if (subReportsMasterReport == null)
			{
				ClearTotals(report);
				if (report.ReportPass != StiReportPass.First)
				{
					report.ParentReport = null;
				}
				report.EngineV1.ReportPageNumbers = null;
				report.EngineV1.ReportTotalPageCounts = null;
			}
		}
	}

	private static void ConnectToData(StiReport report)
	{
		report.StatusString = StiLocalization.Get("Report", "ConnectingToData");
		report.Dictionary.Connect();
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			dataSource.ResetData();
			dataSource.ResetDetailsRows();
			if (StiOptions.Dictionary.EnableConnectOnStartOnSecondPass && dataSource is StiSqlSource stiSqlSource)
			{
				stiSqlSource.ConnectOnStart = true;
			}
		}
		report.Dictionary.ConnectVirtualDataSources();
		report.Dictionary.ConnectCrossTabDataSources();
		report.Dictionary.ConnectDataTransformations();
		report.Dictionary.RegRelations(virtualSources: true);
	}

	private static void DisconnectFromData(StiReport report)
	{
		report.Dictionary.Disconnect();
	}

	private static bool IsDialogsOnStartExist(StiReport report)
	{
		foreach (StiPage page in report.Pages)
		{
			if (page is IStiForm { StartMode: StiFormStartMode.OnStart, Enabled: not false })
			{
				return true;
			}
		}
		return false;
	}

	private static bool RenderFormsOnStart(StiReport report)
	{
		if (!StiOptions.Configuration.IsWeb && report.ReportPass != StiReportPass.First)
		{
			report.StatusString = string.Empty;
			StiDialogsProvider provider = StiDialogsProvider.GetProvider(report);
			if (!provider.Render(report, StiFormStartMode.OnStart))
			{
				StiLogService.Write(typeof(StiRenderProviderV1), "Report stopped in Dialog Forms");
				report.IsStopped = true;
				return false;
			}
			if (report.Progress != null)
			{
				if (!report.Progress.IsVisible)
				{
					report.Progress.Show();
				}
				report.Progress.SetAllowClose(allows: true);
			}
		}
		return true;
	}

	private static bool RenderFormsOnEnd(StiReport report)
	{
		if (!StiOptions.Configuration.IsWeb && report.ReportPass != StiReportPass.First)
		{
			report.StatusString = string.Empty;
			StiDialogsProvider provider = StiDialogsProvider.GetProvider(report);
			if (!provider.Render(report, StiFormStartMode.OnEnd))
			{
				StiLogService.Write(typeof(StiRenderProviderV1), "Report stopped in Dialog Forms");
				report.IsStopped = true;
				return false;
			}
			if (report.Progress != null)
			{
				if (!report.Progress.IsVisible)
				{
					report.Progress.Show();
				}
				report.Progress.SetAllowClose(allows: true);
			}
		}
		return true;
	}

	private static bool CheckDialogsInPreview(StiReport report)
	{
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (variable.RequestFromUser && !report.IsReportRenderingAfterSubmit && report.RequestParameters)
			{
				report.IsStopped = false;
				report.IsRendered = true;
				if (report.ParentReport != null)
				{
					report.ParentReport.IsRendered = true;
				}
				return false;
			}
		}
		foreach (StiPage page in report.Pages)
		{
			if (page is StiForm { StartMode: StiFormStartMode.OnPreview } && !report.IsReportRenderingAfterSubmit)
			{
				report.IsStopped = false;
				report.IsRendered = true;
				if (report.ParentReport != null)
				{
					report.ParentReport.IsRendered = true;
				}
				return false;
			}
		}
		return true;
	}

	private static void ClearPagesWhichLessThenFromPageAndGreaterThenToPage(StiReport report, StiRenderState state)
	{
		if (report.RenderedPages.Count > 1 && report.ReportPass != StiReportPass.First)
		{
			int num = report.RenderedPages.Count - 2;
			if ((state.FromPage > 0 && state.FromPage > num) || (state.ToPage > 0 && state.ToPage < num))
			{
				report.RenderedPages[num].Components.Clear();
			}
			if (state.ToPage > 0 && state.ToPage < num && state.RenderOnlyPagesFromRange)
			{
				report.IsStopped = true;
			}
		}
	}

	private static void InitCacheMode(StiReport report)
	{
		if (report.ReportCacheMode != 0)
		{
			report.RenderedPages.CacheMode = report.ReportCacheMode == StiReportCacheMode.On;
			report.RenderedPages.CanUseCacheMode = true;
		}
	}

	private static void RemoveAllPagesLessThenFromPageAndGreaterThenToPage(StiReport report, StiRenderState state)
	{
		if (!state.DestroyPagesWhichNotInRange || report.ReportPass == StiReportPass.First)
		{
			return;
		}
		if (state.ToPage > 0)
		{
			while (state.ToPage < report.RenderedPages.Count - 1)
			{
				report.RenderedPages.RemoveAt(report.RenderedPages.Count - 1);
			}
		}
		int num = state.FromPage;
		while (num > 0 && report.RenderedPages.Count > 0)
		{
			report.RenderedPages.RemoveAt(0);
			num--;
		}
	}

	private static void FinishAllPagesInNotCachedPagesArray(StiReport report)
	{
		if (report.RenderedPages.CacheMode && report.RenderedPages.NotCachedPages != null && report.ReportPass != StiReportPass.First)
		{
			report.RenderedPages.NotCachedPages.Clear();
			report.RenderedPages.NotCachedPages = null;
		}
	}

	private static void ProcessPageToCache(StiReport report, StiPage page)
	{
		if (report.RenderedPages.CacheMode && report.ReportPass != StiReportPass.First)
		{
			if (report.RenderedPages.NotCachedPages == null)
			{
				report.RenderedPages.NotCachedPages = new List<StiPage>();
			}
			if (report.RenderedPages.NotCachedPages.IndexOf(page) == -1)
			{
				report.RenderedPages.NotCachedPages.Add(page);
			}
			StiPostProcessProviderV1.PostProcessPage(page, page.PageInfoV1.PageNumber == 1, isLastPage: false);
			StiPostProcessProviderV1.PostProcessPrimitives(page);
		}
	}

	private static void RenderFirstPass(StiReport report, StiNumberOfPass numberOfPass)
	{
		if (numberOfPass != StiNumberOfPass.DoublePass || report.ReportPass != 0)
		{
			return;
		}
		report.ReportPass = StiReportPass.First;
		report.Render();
		report.ReportPass = StiReportPass.Second;
		report.IsRendering = true;
		report.IsRendered = false;
		report.EngineV1.ReportPageNumbers = new Hashtable();
		report.EngineV1.ReportTotalPageCounts = new Hashtable();
		int num = 0;
		foreach (StiPage renderedPage in report.RenderedPages)
		{
			report.EngineV1.ReportPageNumbers[num] = renderedPage.PageInfoV1.PageNumber;
			report.EngineV1.ReportTotalPageCounts[num] = renderedPage.PageInfoV1.TotalPageCount;
			num++;
		}
		report.RenderedPages.Clear();
		ResetRenderStatus(report);
	}

	private static void ResetRenderStatus(StiReport report)
	{
		StiComponentsCollection components = report.GetComponents();
		foreach (StiComponent item in components)
		{
			item.PointerValue = null;
			item.BookmarkValue = null;
			item.TagValue = null;
			item.ToolTipValue = null;
			if (item is StiText stiText)
			{
				stiText.TextValue = null;
			}
		}
	}

	private static StiNumberOfPass GetNumberOfPass(StiReport report)
	{
		StiNumberOfPass numberOfPass = report.NumberOfPass;
		if (report.ReportPass == StiReportPass.None && numberOfPass == StiNumberOfPass.SinglePass)
		{
			if (!StiOptions.Engine.UseAdvancedPrintOnEngineV1)
			{
				return StiNumberOfPass.SinglePass;
			}
			StiComponentsCollection components = report.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item.PrintOn != 0)
				{
					return StiNumberOfPass.DoublePass;
				}
			}
		}
		return numberOfPass;
	}

	private static void ClearPagesForFirstPass(StiReport report)
	{
		if (report.ReportPass == StiReportPass.First && report.RenderedPages.Count >= 2)
		{
			report.RenderedPages[report.RenderedPages.Count - 2].Components.Clear();
		}
	}

	private static void MadeCollate(StiReport report)
	{
		if (report.RenderedPages.Count < 3 || report.Collate < 2)
		{
			return;
		}
		List<StiPage> list = new List<StiPage>();
		if (StiOptions.Engine.UseCollateOldMode)
		{
			for (int i = 0; i < report.Collate; i++)
			{
				for (int j = i; j < report.RenderedPages.Count; j += report.Collate)
				{
					list.Add(report.RenderedPages[j]);
				}
			}
		}
		else
		{
			int num = report.RenderedPages.Count / report.Collate;
			if (num * report.Collate < report.RenderedPages.Count)
			{
				num++;
			}
			for (int k = 0; k < num; k++)
			{
				for (int l = k; l < report.RenderedPages.Count; l += num)
				{
					list.Add(report.RenderedPages[l]);
				}
			}
		}
		report.RenderedPages.Clear();
		foreach (StiPage item in list)
		{
			report.RenderedPages.AddV2Internal(item);
		}
	}

	private static void InitReport(StiReport report)
	{
		report.CurrentPage = 0;
		report.CurrentPrintPage = 0;
		report.EngineV1.RequreResetPageNumber = false;
		report.IsStopped = false;
	}

	private static void ClearTotals(StiReport report)
	{
		report.Totals.Clear();
		if (report.ParentReport != null)
		{
			report.ParentReport.Totals.Clear();
		}
	}

	private static Hashtable PrepareSubReportsAndDrillDownPages(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		StiComponentsCollection components = report.GetComponents();
		foreach (StiPage page in report.Pages)
		{
			page.Skip = false;
			hashtable[page.Guid] = page;
		}
		foreach (StiComponent item in components)
		{
			if (item is StiSubReport { SubReportPageGuid: not null } stiSubReport && hashtable[stiSubReport.SubReportPageGuid] is StiPage stiPage2)
			{
				stiPage2.Skip = true;
			}
			if (item.Interaction != null && item.Interaction.DrillDownPageGuid != null && hashtable[item.Interaction.DrillDownPageGuid] is StiPage stiPage3)
			{
				stiPage3.Skip = true;
			}
			if (!(item is StiChart))
			{
				continue;
			}
			foreach (IStiSeries item2 in ((StiChart)item).Series)
			{
				if (item2.Interaction != null && item.Interaction != null && item.Interaction.DrillDownPageGuid != null)
				{
					if (hashtable[item.Interaction.DrillDownPageGuid] is StiPage stiPage4)
					{
						stiPage4.Skip = true;
					}
					break;
				}
			}
		}
		return hashtable;
	}

	private static void UpdateSystemVariables(StiReport report, StiReport masterReport, StiPage page)
	{
		report.PageNumber += page.SegmentPerWidth * page.SegmentPerHeight;
		report.EngineV1.RealPageNumber += page.SegmentPerWidth * page.SegmentPerHeight;
		report.TotalPageCount = report.PageNumber - 1;
		if (masterReport != null)
		{
			masterReport.PageNumber = report.PageNumber;
			masterReport.EngineV1.RealPageNumber = report.EngineV1.RealPageNumber;
			masterReport.TotalPageCount = report.TotalPageCount;
		}
	}

	private static void RenderReport(StiReport report, StiReport masterReport, StiRenderState state)
	{
		try
		{
			report.PageCopyNumber = 1;
			StiRender stiRender = new StiRender(report);
			while (true)
			{
				report.InvokeRendering();
				masterReport?.InvokeRendering();
				if (report.EngineV1 != null)
				{
					report.EngineV1.ProgressHelper.Process();
				}
				StiPage nextPage = stiRender.GetNextPage();
				if (nextPage == null || report.IsStopped)
				{
					break;
				}
				if (nextPage != null)
				{
					ProcessPageToCache(report, nextPage);
					report.RenderedPages.Add(nextPage);
					ClearPagesWhichLessThenFromPageAndGreaterThenToPage(report, state);
					ClearPagesForFirstPass(report);
					UpdateSystemVariables(report, masterReport, nextPage);
					if (report.Progress != null && report.Progress.IsBreaked)
					{
						report.IsStopped = true;
						break;
					}
					if (report.IsStopped || (report.StopBeforePage != 0 && report.StopBeforePage <= report.CurrentPrintPage))
					{
						break;
					}
				}
			}
		}
		catch (StiReportRenderingStopException)
		{
		}
		finally
		{
			foreach (StiPage renderedPage in report.RenderedPages)
			{
				renderedPage.UnPrepare();
			}
		}
	}

	private static void FinishProgressForm(StiReport report)
	{
		if (report.ReportPass != StiReportPass.First)
		{
			report.StatusString = StiLocalization.Get("Report", "FinishingReport");
			if (report.Progress != null)
			{
				report.Progress.HideProgressBar();
			}
			if (report.Progress != null)
			{
				report.Progress.SetAllowClose(allows: false);
				report.Progress.Update(report.StatusString, 90);
			}
		}
	}
}
