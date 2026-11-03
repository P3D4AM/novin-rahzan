using System;
using System.Collections;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiReportV1Builder
{
	public static void RenderSingleReport(StiReport masterReport, StiRenderState renderState)
	{
		try
		{
			masterReport.IsStopped = false;
			masterReport.IndexName = 1;
			StiLogService.Write(masterReport.GetType(), "Rendering report");
			masterReport.IsRendered = false;
			if (masterReport.CheckNeedsCompiling())
			{
				if (masterReport.Progress != null)
				{
					masterReport.StatusString = StiLocalization.Get("Report", "CompilingReport");
				}
				if (masterReport.CompiledReport == null)
				{
					masterReport.Compile();
				}
				if (masterReport.CompiledReport == null)
				{
					return;
				}
				masterReport.CompiledReport.Tag = masterReport.Tag;
				masterReport.CompiledReport.Progress = masterReport.Progress;
				masterReport.CompiledReport.ParentReport = masterReport;
				masterReport.IsRendering = true;
				masterReport.CompiledReport.IsRendering = true;
				StiRenderProviderV1.Render(masterReport.CompiledReport, renderState);
				masterReport.DisposeCachedRichText();
				masterReport.DisposeCachedRichTextFormat();
				if (masterReport.CompiledReport != null)
				{
					masterReport.CompiledReport.DisposeCachedRichText();
					masterReport.CompiledReport.DisposeCachedRichTextFormat();
					masterReport.IsRendered = masterReport.CompiledReport.IsRendered;
					if (StiOptions.Engine.NotClearParentReport)
					{
						masterReport.CompiledReport.ParentReport = null;
					}
				}
				masterReport.IsRendering = true;
				masterReport.InvokeRefreshViewer();
			}
			else
			{
				masterReport.IsRendering = true;
				StiRenderProviderV1.Render(masterReport, renderState);
				masterReport.InvokeRefreshViewer();
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(masterReport.GetType(), "Rendering report...ERROR");
			StiLogService.Write(masterReport.GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			masterReport.IsRendering = false;
			if (masterReport.CompiledReport != null)
			{
				masterReport.CompiledReport.IsRendering = false;
			}
		}
	}

	public static void RenderSubReports(StiReport ownerReport, StiRenderState renderState)
	{
		if (ownerReport.CheckNeedsCompiling() && ownerReport.CompiledReport == null)
		{
			ownerReport.Compile();
		}
		ownerReport.RenderedPages.Clear();
		foreach (StiReport subReport in ownerReport.SubReports)
		{
			if (subReport.CheckNeedsCompiling() && subReport.CompiledReport == null)
			{
				subReport.Compile();
			}
		}
		foreach (StiReport subReport2 in ownerReport.SubReports)
		{
			subReport2.RenderedPages = ownerReport.RenderedPages;
		}
		Hashtable hashtable = new Hashtable();
		ownerReport.IsStopped = false;
		ownerReport.InvokeBeginRender();
		ownerReport.PageNumber = 1;
		ownerReport.EngineV1.RealPageNumber = 1;
		StiReport stiReport3 = null;
		if (ownerReport.Pages[0].GetComponents().Count > 0)
		{
			ownerReport.SubReportsMasterReport = ownerReport;
			stiReport3 = ownerReport;
			if (ownerReport.CompiledReport != null)
			{
				stiReport3 = ownerReport.CompiledReport;
				ownerReport.CompiledReport.SubReportsMasterReport = ownerReport;
				ownerReport.CompiledReport.RenderedPages.Clear();
			}
			StiReportsCollection subReports = ownerReport.SubReports;
			ownerReport.SubReports = null;
			renderState.IsSubReportMode = true;
			ownerReport.Render(renderState);
			ownerReport.SubReports = subReports;
			ownerReport.SubReportsMasterReport = null;
		}
		int num = 0;
		StiReport stiReport4 = null;
		foreach (StiReport subReport3 in ownerReport.SubReports)
		{
			if (subReport3.SubReportsResetPageNumber && stiReport4 != null)
			{
				stiReport4.totalPageCountValue = ownerReport.totalPageCountValue;
				stiReport4.EngineV1.ProcessResetPageNumber(stiReport4);
				ownerReport.PageNumber = 1;
				stiReport4.EngineV1.PageInProgress = null;
			}
			try
			{
				subReport3.SubReportsMasterReport = ownerReport;
				if (subReport3.CompiledReport != null)
				{
					subReport3.CompiledReport.SubReportsMasterReport = ownerReport;
				}
				if (stiReport4 != null)
				{
					subReport3.Pages[0].PrintOnPreviousPage = subReport3.SubReportsPrintOnPreviousPage;
					if (subReport3.CompiledReport != null)
					{
						subReport3.CompiledReport.Pages[0].PrintOnPreviousPage = subReport3.SubReportsPrintOnPreviousPage;
					}
				}
				int count = ownerReport.RenderedPages.Count;
				subReport3.Totals = new Hashtable();
				subReport3.Render(renderState.ShowProgress);
				Hashtable hashtable2 = new Hashtable();
				StiReport stiReport6 = subReport3;
				if (stiReport6.CompiledReport != null)
				{
					stiReport6 = subReport3.CompiledReport;
				}
				object[] array = new object[stiReport6.Totals.Keys.Count];
				stiReport6.Totals.Keys.CopyTo(array, 0);
				object[] array2 = array;
				foreach (object key in array2)
				{
					if (!(stiReport6.Totals[key] is ArrayList arrayList))
					{
						continue;
					}
					ArrayList arrayList3 = (ArrayList)(hashtable2[key] = new ArrayList());
					foreach (StiRuntimeVariables item in arrayList)
					{
						arrayList3.Add(item.Clone());
					}
				}
				hashtable[num++] = hashtable2;
				if (subReport3.ReportUnit != ownerReport.ReportUnit)
				{
					for (int j = count; j < ownerReport.RenderedPages.Count; j++)
					{
						StiPage stiPage = ownerReport.RenderedPages[j];
						stiPage.Convert(subReport3.Unit, ownerReport.Unit);
					}
				}
			}
			finally
			{
				subReport3.SubReportsMasterReport = null;
				if (subReport3.CompiledReport != null)
				{
					subReport3.CompiledReport.SubReportsMasterReport = null;
				}
			}
			if (subReport3.IsStopped)
			{
				ownerReport.IsStopped = subReport3.IsStopped;
				break;
			}
			ownerReport.Bookmark.Bookmarks.AddRange(subReport3.Bookmark.Bookmarks);
			stiReport4 = subReport3;
		}
		num = 0;
		foreach (StiReport subReport4 in ownerReport.SubReports)
		{
			StiReport stiReport8 = subReport4;
			if (stiReport8.CompiledReport != null)
			{
				stiReport8 = stiReport8.CompiledReport;
			}
			stiReport8.Totals = hashtable[num++] as Hashtable;
			stiReport8.TotalPageCount = ownerReport.PageNumber - 1;
			stiReport8.EngineV1.ProcessResetPageNumber(stiReport8);
			stiReport8.InvokeEndRender();
		}
		foreach (StiReport subReport5 in ownerReport.SubReports)
		{
			subReport5.EngineV1.PageInProgress = null;
			subReport5.Totals = null;
			subReport5.EngineV1.ReportPageNumbers = null;
			subReport5.EngineV1.ReportTotalPageCounts = null;
		}
		if (stiReport3 != null)
		{
			stiReport3.TotalPageCount = ownerReport.PageNumber - 1;
			stiReport3.EngineV1.ProcessResetPageNumber(stiReport3);
			stiReport3.InvokeEndRender();
			stiReport3.EngineV1.PageInProgress = null;
		}
		if (stiReport3 == null || stiReport3 != ownerReport)
		{
			ownerReport.InvokeEndRender();
		}
		foreach (StiPage renderedPage in ownerReport.RenderedPages)
		{
			renderedPage.Report = ((ownerReport.CompiledReport != null) ? ownerReport.CompiledReport : ownerReport);
		}
		ownerReport.IsRendered = true;
		ownerReport.NeedsCompiling = false;
		if (ownerReport.CompiledReport != null)
		{
			ownerReport.CompiledReport.IsRendered = true;
			ownerReport.CompiledReport.NeedsCompiling = false;
		}
		if (ownerReport.ParentReport != null)
		{
			ownerReport.ParentReport.IsRendered = true;
			ownerReport.ParentReport.NeedsCompiling = false;
		}
		StiPostProcessProviderV1.PostProcessPages(ownerReport.RenderedPages);
		ownerReport.InvokeRefreshViewer();
	}
}
