using System;
using System.Collections;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiReportV2Builder
{
	public static void RenderSingleReport(StiReport masterReport, StiRenderState renderState)
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		StiEngine engine = masterReport.Engine;
		StiReport report = null;
		if (engine != null)
		{
			report = masterReport.Engine.Report;
		}
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
				if (engine != null)
				{
					masterReport.CompiledReport.Engine = engine;
					masterReport.CompiledReport.Engine.Report = masterReport.CompiledReport;
				}
				else
				{
					masterReport.CompiledReport.Engine = new StiEngine(masterReport.CompiledReport);
				}
				masterReport.CompiledReport.Tag = masterReport.Tag;
				masterReport.CompiledReport.Progress = masterReport.Progress;
				masterReport.CompiledReport.ParentReport = masterReport;
				masterReport.IsRendering = true;
				masterReport.CompiledReport.IsRendering = true;
				masterReport.CompiledReport.RenderedWith = masterReport.RenderedWith;
				masterReport.CompiledReport.CacheTotals = masterReport.CacheTotals;
				masterReport.CompiledReport.CachedTotals = masterReport.CachedTotals;
				masterReport.CompiledReport.bookmarkValue.Engine = masterReport.bookmarkValue.Engine;
				StiRenderProviderV2.Render(masterReport.CompiledReport, renderState);
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
				if (engine != null)
				{
					masterReport.Engine = engine;
					masterReport.Engine.Report = masterReport;
				}
				else
				{
					masterReport.Engine = new StiEngine(masterReport);
				}
				masterReport.IsRendering = true;
				StiRenderProviderV2.Render(masterReport, renderState);
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
				if (engine == null)
				{
					masterReport.CompiledReport.Engine = null;
				}
			}
			if (engine == null)
			{
				masterReport.Engine = null;
			}
			else
			{
				engine.Report = report;
			}
		}
	}

	public static void RenderSubReports(StiReport ownerReport, StiRenderState renderState)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if (ownerReport.CheckNeedsCompiling() && ownerReport.CompiledReport == null)
		{
			ownerReport.Compile();
		}
		ownerReport.RenderedPages.Clear();
		lock (((ICollection)ownerReport.SubReports).SyncRoot)
		{
			foreach (StiReport subReport in ownerReport.SubReports)
			{
				if (subReport.EngineVersion != ownerReport.EngineVersion)
				{
					throw new Exception("A Value of the EngineVersion property of one of subreport is not equal to the value of the EngineVersion property of a master report.");
				}
			}
		}
		StiCalculationMode[] array = new StiCalculationMode[ownerReport.SubReports.Count];
		lock (((ICollection)ownerReport.SubReports).SyncRoot)
		{
			for (int i = 0; i < ownerReport.SubReports.Count; i++)
			{
				StiReport stiReport2 = ownerReport.SubReports[i];
				array[i] = stiReport2.CalculationMode;
				if (StiOptions.Engine.ForceInterpretationMode)
				{
					stiReport2.CalculationMode = StiCalculationMode.Interpretation;
				}
				if (stiReport2.CheckNeedsCompiling() && stiReport2.CompiledReport == null)
				{
					stiReport2.Compile();
				}
			}
		}
		lock (((ICollection)ownerReport.SubReports).SyncRoot)
		{
			foreach (StiReport subReport2 in ownerReport.SubReports)
			{
				subReport2.RenderedPages = ownerReport.RenderedPages;
			}
		}
		Hashtable hashtable = new Hashtable();
		ownerReport.IsStopped = false;
		ownerReport.InvokeBeginRender();
		StiReport stiReport4 = null;
		StiEngine stiEngine2;
		if (ownerReport.Pages[0].GetComponents().Count > 0)
		{
			if (ownerReport.CheckNeedsCompiling())
			{
				stiEngine2 = (ownerReport.Engine = new StiEngine(ownerReport.CompiledReport));
				ownerReport.CompiledReport.Engine = stiEngine2;
				ownerReport.CompiledReport.CurrentPrintPage = 0;
			}
			else
			{
				stiEngine2 = (ownerReport.Engine = new StiEngine(ownerReport));
				ownerReport.CurrentPrintPage = 0;
			}
			ownerReport.SubReportsMasterReport = ownerReport;
			stiReport4 = ownerReport;
			if (ownerReport.CompiledReport != null)
			{
				stiReport4 = ownerReport.CompiledReport;
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
		else
		{
			stiEngine2 = new StiEngine(ownerReport);
			ownerReport.CurrentPrintPage = 0;
			ownerReport.Engine = stiEngine2;
			if (ownerReport.CompiledReport != null)
			{
				ownerReport.CompiledReport.Engine = stiEngine2;
			}
		}
		if (ownerReport.CompiledReport != null)
		{
			ownerReport.CurrentPrintPage = ownerReport.CompiledReport.CurrentPrintPage;
		}
		int num = 0;
		StiReport stiReport5 = null;
		lock (((ICollection)ownerReport.SubReports).SyncRoot)
		{
			foreach (StiReport subReport3 in ownerReport.SubReports)
			{
				try
				{
					subReport3.SubReportsMasterReport = ownerReport;
					if (subReport3.CompiledReport != null)
					{
						subReport3.CompiledReport.SubReportsMasterReport = ownerReport;
						subReport3.Engine = stiEngine2;
						subReport3.CompiledReport.Engine = stiEngine2;
						subReport3.CompiledReport.CurrentPrintPage = ownerReport.CurrentPrintPage;
					}
					else
					{
						subReport3.Engine = stiEngine2;
						subReport3.CurrentPrintPage = ownerReport.CurrentPrintPage;
					}
					if (stiReport5 != null)
					{
						subReport3.Pages[0].PrintOnPreviousPage = subReport3.SubReportsPrintOnPreviousPage;
						if (subReport3.CompiledReport != null)
						{
							subReport3.CompiledReport.Pages[0].PrintOnPreviousPage = subReport3.SubReportsPrintOnPreviousPage;
						}
					}
					if (stiReport5 != null)
					{
						subReport3.Pages[0].ResetPageNumber = subReport3.SubReportsResetPageNumber;
						if (subReport3.CompiledReport != null)
						{
							subReport3.CompiledReport.Pages[0].ResetPageNumber = subReport3.SubReportsResetPageNumber;
						}
					}
					int count = ownerReport.RenderedPages.Count;
					if (stiEngine2.PageNumbers.PageNumbers.Count > 0)
					{
						stiEngine2.PageNumbers.PageNumbers[stiEngine2.PageNumbers.PageNumbers.Count - 1].FixedPosition = true;
					}
					subReport3.Totals = new Hashtable();
					subReport3.Render(new StiRenderState(renderState.ShowProgress)
					{
						IsSubReportMode = renderState.IsSubReportMode
					});
					ownerReport.CurrentPrintPage = ((subReport3.CompiledReport != null) ? subReport3.CompiledReport.CurrentPrintPage : subReport3.CurrentPrintPage);
					Hashtable hashtable2 = new Hashtable();
					StiReport stiReport7 = subReport3;
					if (stiReport7.CompiledReport != null)
					{
						stiReport7 = subReport3.CompiledReport;
					}
					object[] array2 = new object[stiReport7.Totals.Keys.Count];
					stiReport7.Totals.Keys.CopyTo(array2, 0);
					lock (array2.SyncRoot)
					{
						object[] array3 = array2;
						foreach (object obj in array3)
						{
							if (stiReport7.Totals[obj] is ArrayList arrayList)
							{
								ArrayList arrayList3 = (ArrayList)(hashtable2[obj] = new ArrayList());
								lock (((ICollection)arrayList).SyncRoot)
								{
									foreach (StiRuntimeVariables item in arrayList)
									{
										arrayList3.Add(item.Clone());
									}
								}
							}
							if (obj is string text && text.StartsWith("#%#"))
							{
								hashtable2[obj] = stiReport7.Totals[obj];
							}
						}
					}
					hashtable[num++] = hashtable2;
					foreach (DictionaryEntry anchor in stiReport7.Anchors)
					{
						if (!stiReport4.Anchors.ContainsKey(anchor.Key))
						{
							stiReport4.Anchors[anchor.Key] = anchor.Value;
						}
					}
					if (subReport3.ReportUnit != ownerReport.ReportUnit)
					{
						for (int k = count; k < ownerReport.RenderedPages.Count; k++)
						{
							StiPage stiPage = ownerReport.RenderedPages[k];
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
				ownerReport.Pointer.Bookmarks.AddRange(subReport3.Pointer.Bookmarks);
				stiReport5 = subReport3;
			}
		}
		stiEngine2.PageNumbers.ProcessPageNumbers();
		num = 0;
		lock (((ICollection)ownerReport.SubReports).SyncRoot)
		{
			foreach (StiReport subReport4 in ownerReport.SubReports)
			{
				StiReport stiReport9 = subReport4;
				if (stiReport9.CompiledReport != null)
				{
					stiReport9 = stiReport9.CompiledReport;
				}
				stiReport9.Totals = hashtable[num++] as Hashtable;
				stiReport9.InvokeEndRender();
			}
		}
		lock (((ICollection)ownerReport.SubReports).SyncRoot)
		{
			for (int l = 0; l < ownerReport.SubReports.Count; l++)
			{
				StiReport stiReport10 = ownerReport.SubReports[l];
				stiReport10.CalculationMode = array[l];
				stiReport10.Totals = null;
			}
		}
		if (stiReport4 != null)
		{
			stiReport4.Engine = stiEngine2;
			stiReport4.InvokeEndRender();
			stiReport4.Engine = null;
		}
		if (stiReport4 == null || stiReport4 != ownerReport)
		{
			ownerReport.InvokeEndRender();
		}
		lock (((ICollection)ownerReport.RenderedPages).SyncRoot)
		{
			foreach (StiPage renderedPage in ownerReport.RenderedPages)
			{
				renderedPage.Report = ownerReport.CompiledReport ?? ownerReport;
			}
		}
		ownerReport.Engine = null;
		ownerReport.IsRendered = true;
		if (ownerReport.CompiledReport != null)
		{
			ownerReport.CompiledReport.Engine = null;
			ownerReport.CompiledReport.IsRendered = true;
		}
		if (ownerReport.ParentReport != null)
		{
			ownerReport.ParentReport.Engine = null;
			ownerReport.ParentReport.IsRendered = true;
		}
		StiPostProcessProviderV2.PostProcessPages(ownerReport.RenderedPages);
		if (ownerReport != null && ownerReport.RenderedPages != null)
		{
			foreach (StiPage renderedPage2 in ownerReport.RenderedPages)
			{
				renderedPage2.Report = ownerReport;
			}
		}
		renderState.IsSubReportMode = false;
		ownerReport.InvokeRefreshViewer();
	}
}
