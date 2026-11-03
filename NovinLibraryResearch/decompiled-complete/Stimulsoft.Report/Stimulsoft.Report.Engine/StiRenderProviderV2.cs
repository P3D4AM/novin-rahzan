using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Engine;

public sealed class StiRenderProviderV2
{
	private const string nullGuid = "nullGuid";

	public static void Render(StiReport report, StiRenderState state)
	{
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		StiComponentsCollection components = report.GetComponents();
		foreach (StiComponent item in components)
		{
			if (item is StiText { ProcessAt: StiProcessAt.EndOfPage })
			{
				report.Engine.AllowEndOfPageProcessing = true;
				break;
			}
		}
		InitReport(report);
		if (!CheckDialogsInPreview(report))
		{
			Hashtable dataSourcesUsedInRequestFromUsersVariables = StiDataSourceHelper.GetDataSourcesUsedInRequestFromUsersVariables(report);
			Hashtable hashtable = new Hashtable();
			foreach (StiDataSource dataSource in report.Dictionary.DataSources)
			{
				if (!dataSourcesUsedInRequestFromUsersVariables.ContainsKey(dataSource.Name))
				{
					hashtable[dataSource.Name] = dataSource.ConnectOnStart;
					dataSource.ConnectOnStart = false;
				}
			}
			ConnectToData(report);
			foreach (StiDataSource dataSource2 in report.Dictionary.DataSources)
			{
				if (hashtable.ContainsKey(dataSource2.Name))
				{
					dataSource2.ConnectOnStart = (bool)hashtable[dataSource2.Name];
				}
			}
			StiVariableHelper.FillItemsOfVariables(report.CompiledReport ?? report);
			if (report.Pages.Count > 0 && report.RenderedPages.Count == 1)
			{
				StiPage stiPage = report.Pages[0];
				StiPage stiPage2 = report.RenderedPages[0];
				stiPage2.PaperSize = stiPage.PaperSize;
				stiPage2.Orientation = stiPage.Orientation;
				if ((int)stiPage.PaperSize == 0)
				{
					stiPage2.Width = stiPage.Width;
					stiPage2.Height = stiPage.Height;
				}
			}
			return;
		}
		if (report.SubReportsMasterReport == null)
		{
			report.RenderedPages.Clear();
		}
		StiNumberOfPass numberOfPass = GetNumberOfPass(report);
		bool flag = IsDialogsOnStartExist(report);
		if (!flag)
		{
			RenderFirstPass(report, numberOfPass);
		}
		StiReport subReportsMasterReport = report.SubReportsMasterReport;
		Hashtable newTableComponents = new Hashtable();
		Hashtable hashtable2 = null;
		try
		{
			PrepareSubReportsAndDrillDownPages(report);
			if (subReportsMasterReport == null)
			{
				report.RenderedPages.Clear();
			}
			report.InvokeBeginRender();
			ClearTotals(report);
			foreach (StiDataSource dataSource3 in report.Dictionary.DataSources)
			{
				if (dataSource3 is StiCrossTabDataSource stiCrossTabDataSource && !string.IsNullOrEmpty(stiCrossTabDataSource.NameInSource))
				{
					report.GetComponentByName(stiCrossTabDataSource.NameInSource)?.Prepare();
				}
			}
			ConnectToData(report);
			bool haveVars = StiVariableHelper.FillItemsOfVariables(report.CompiledReport ?? report);
			StiVariableHelper.SetDefaultValueForRequestFromUserVariables(report.CompiledReport ?? report, haveVars);
			if (!RenderFormsOnStart(report))
			{
				return;
			}
			if (flag)
			{
				DisconnectFromData(report);
				RenderFirstPass(report, numberOfPass);
				ConnectToData(report);
			}
			report.Bookmark.Bookmarks.Clear();
			report.Bookmark.Text = report.ReportAlias;
			if (report.IsSecondPass)
			{
				report.Engine.FirstPassPointer = report.Pointer;
				report.Pointer = new StiBookmark();
				report.Pointer.Text = report.ReportAlias;
			}
			if (report.Progress != null)
			{
				report.Progress.ShowProgressBar();
			}
			if (report.ReportPass == StiReportPass.First || report.ReportPass == StiReportPass.None)
			{
				InitCacheMode(report);
			}
			StiComponentsCollection components2 = report.GetComponents();
			List<StiComponent> list = (from StiComponent c in components2
				where c is StiTable && c.Enabled
				select c).ToList();
			report.ContainsTables = list.Count != 0;
			if (list.Count > 0)
			{
				Hashtable hashtable3 = new Hashtable();
				bool containsTables = false;
				foreach (StiTable item2 in list)
				{
					if (item2.AutoWidth != 0)
					{
						containsTables = true;
					}
					StiDataBand stiDataBand = item2.StartRenderTableBand(ref newTableComponents);
					if (stiDataBand != null)
					{
						hashtable3.Add(stiDataBand.Name, stiDataBand);
						report.Engine.KeepFirstDetailTogetherTablesList[stiDataBand] = item2;
					}
				}
				if (hashtable3.Count != 0)
				{
					foreach (StiDataBand value6 in hashtable3.Values)
					{
						if (value6.MasterComponent != null && value6.MasterComponent is StiTable)
						{
							value6.MasterComponent = hashtable3[value6.MasterComponent.Name + "_DB"] as StiDataBand;
						}
					}
				}
				components2.Clear();
				components2 = report.GetComponents();
				report.ContainsTables = containsTables;
			}
			report.Engine.ParserConversionStore = new Hashtable();
			report.Engine.AnchorsArguments = new Hashtable();
			report.Engine.HashDataSourceReferencesCounter = new Hashtable();
			hashtable2 = new Hashtable();
			bool flag2 = report.CalculationMode == StiCalculationMode.Compilation;
			foreach (StiComponent item3 in components2)
			{
				item3.Prepare();
				if (!flag2)
				{
					if (!flag2 && item3.Conditions.Count > 0)
					{
						ArrayList arrayList = new ArrayList();
						foreach (StiBaseCondition condition in item3.Conditions)
						{
							StiCondition stiCondition = condition as StiCondition;
							if (condition is StiMultiCondition)
							{
								StiMultiCondition stiMultiCondition = condition as StiMultiCondition;
								if (!stiMultiCondition.FilterOn || stiMultiCondition.Filters.Count <= 0)
								{
									continue;
								}
								StringBuilder stringBuilder = new StringBuilder("{");
								for (int i = 0; i < stiMultiCondition.Filters.Count; i++)
								{
									StiFilter stiFilter = stiMultiCondition.Filters[i];
									stringBuilder.Append("(");
									stringBuilder.Append(StiDataHelper.GetFilterExpression(stiFilter, stiFilter.Column, report));
									stringBuilder.Append(")");
									if (i < stiMultiCondition.Filters.Count - 1)
									{
										stringBuilder.Append((stiMultiCondition.FilterMode == StiFilterMode.And) ? " && " : " || ");
									}
								}
								stringBuilder.Append("}");
								DictionaryEntry dictionaryEntry = new DictionaryEntry(stiMultiCondition, stringBuilder.ToString());
								arrayList.Add(dictionaryEntry);
							}
							else if (stiCondition != null)
							{
								string value = "{" + StiDataHelper.GetFilterExpression(stiCondition, stiCondition.Column, report) + "}";
								DictionaryEntry dictionaryEntry2 = new DictionaryEntry(stiCondition, value);
								arrayList.Add(dictionaryEntry2);
							}
						}
						if (arrayList.Count > 0)
						{
							report.Engine.ParserConversionStore["*StiConditionExpression*" + item3.Name] = arrayList;
						}
					}
					if (item3 is StiText stiText2)
					{
						StiParser.CheckForDataBandsUsedInPageTotals(stiText2, report);
					}
					if (item3 is StiRichText && StiOptions.Engine.FullTrust)
					{
						StiRichText stiRichText = item3 as StiRichText;
						string script = XmlConvert.DecodeName(stiRichText.Text.Value).Replace('\0', ' ');
						try
						{
							string text = GetRichTextToParse(script, stiRichText).Replace('\0', ' ');
							if (stiRichText.FullConvertExpression)
							{
								text = "ConvertRtf(" + text + ")";
							}
							if (text.Length > 0)
							{
								report.Engine.ParserConversionStore["*StiRichTextExpression*" + item3.Name] = text;
							}
						}
						catch (Exception ex)
						{
							string text2 = "Expression in Text property of '" + item3.Name + "' can't be evaluated! " + ex.Message;
							StiLogService.Write(((object)item3).GetType(), text2);
							StiLogService.Write(((object)item3).GetType(), ex.Message);
							report.WriteToReportRenderingMessages(text2);
						}
					}
					if (item3 is StiDataBand && (item3 as StiDataBand).RenderingEvent != null && !string.IsNullOrEmpty((item3 as StiDataBand).RenderingEvent.Script))
					{
						string script2 = (item3 as StiDataBand).RenderingEvent.Script;
						int num = script2.IndexOf("AddAnchor(", StringComparison.InvariantCulture);
						if (num != -1)
						{
							int num2 = script2.IndexOf(")", num, StringComparison.InvariantCulture);
							if (num2 != -1)
							{
								num += 10;
								string value2 = script2.Substring(num, num2 - num);
								report.Engine.AnchorsArguments[item3.Name] = value2;
								EventHandler value3 = StiRenderProviderV2_AddAnchor_Rendering;
								(item3 as StiDataBand).Rendering += value3;
								hashtable2[item3] = value3;
							}
						}
					}
					if (item3 is StiPage && (item3 as StiPage).RenderingEvent != null && !string.IsNullOrEmpty((item3 as StiPage).RenderingEvent.Script))
					{
						string script3 = (item3 as StiPage).RenderingEvent.Script;
						int num3 = script3.IndexOf("AddAnchor(", StringComparison.InvariantCulture);
						if (num3 != -1)
						{
							int num4 = script3.IndexOf(")", num3, StringComparison.InvariantCulture);
							if (num4 != -1)
							{
								num3 += 10;
								string value4 = script3.Substring(num3, num4 - num3);
								report.Engine.AnchorsArguments[item3.Name] = value4;
								EventHandler value5 = StiRenderProviderV2_AddAnchor_Rendering;
								(item3 as StiPage).Rendering += value5;
								hashtable2[item3] = value5;
							}
						}
					}
				}
				if (item3 is StiDataBand stiDataBand3)
				{
					if (!stiDataBand3.IsDataSourceEmpty)
					{
						object obj = report.Engine.HashDataSourceReferencesCounter[stiDataBand3.DataSourceName];
						int num5 = ((obj == null) ? 1 : ((int)obj + 1));
						report.Engine.HashDataSourceReferencesCounter[stiDataBand3.DataSourceName] = num5;
					}
					if (!stiDataBand3.IsBusinessObjectEmpty)
					{
						object obj2 = report.Engine.HashDataSourceReferencesCounter[stiDataBand3.BusinessObject.Name];
						int num6 = ((obj2 == null) ? 1 : ((int)obj2 + 1));
						report.Engine.HashDataSourceReferencesCounter[stiDataBand3.BusinessObject.Name] = num6;
					}
				}
			}
			RenderReport(report, subReportsMasterReport, state);
			if (!report.IsStopped)
			{
				RenderFormsOnEnd(report);
				report.IsRendered = true;
			}
			if (newTableComponents.Count > 0)
			{
				foreach (StiPage key in newTableComponents.Keys)
				{
					ArrayList arrayList2 = newTableComponents[key] as ArrayList;
					foreach (StiComponent item4 in arrayList2)
					{
						if (item4.Parent != null)
						{
							item4.Parent.Components.Remove(item4);
						}
					}
				}
			}
			foreach (StiComponent item5 in list)
			{
				item5.Enabled = true;
			}
		}
		catch (Exception e)
		{
			report.IsStopped = true;
			StiLogService.Write(typeof(StiRenderProviderV2), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			report.CurrentPage = 0;
			if (subReportsMasterReport == null)
			{
				report.CurrentPrintPage = 0;
			}
			if (subReportsMasterReport == null && report.ReportPass != StiReportPass.First)
			{
				if (!report.RenderedPages.CacheMode)
				{
					StiPostProcessProviderV2.PostProcessPages(report.RenderedPages);
				}
				else
				{
					report.RenderedPages.Flush();
					if (report.RenderedPages.NotCachedPages != null)
					{
						lock (((ICollection)report.RenderedPages.NotCachedPages).SyncRoot)
						{
							foreach (StiPage notCachedPage in report.RenderedPages.NotCachedPages)
							{
								StiPostProcessProviderV2.PostProcessPage(notCachedPage, IsFirstPage(report, notCachedPage), IsLastPage(report, notCachedPage));
								StiPostProcessProviderV2.PostProcessPrimitives(notCachedPage);
							}
						}
					}
				}
			}
			FinishAllPagesInNotCachedPagesArray(report);
			RemoveAllPagesLessThenFromPageAndGreaterThenToPage(report, state);
			MadeCollate(report);
			MadeMirrorMargins(report);
			StiBookmarksV2Helper.PrepareBookmark(report.Bookmark);
			StiBookmarksV2Helper.Pack(report.Bookmark);
			StiBookmarksV2Helper.PrepareBookmark(report.Pointer);
			StiBookmarksV2Helper.Pack(report.Pointer);
			if (StiOptions.Engine.DisconnectFromDataBeforeEndRender)
			{
				DisconnectFromData(report);
			}
			if (subReportsMasterReport == null)
			{
				if (report.RenderedPages.Count == 0)
				{
					report.RenderedPages.Add(StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage", new object[1] { report }) as StiPage);
				}
				report.Engine.ProgressHelper.AllowCachedPagesCache = report.ReportPass != StiReportPass.First;
				report.Engine.PageNumbers.ProcessPageNumbers();
				report.InvokeEndRender();
				report.Engine.PageNumbers.Clear();
				if (!report.RenderedPages.CacheMode)
				{
					foreach (StiPage renderedPage in report.RenderedPages)
					{
						StiComponentsCollection components3 = renderedPage.GetComponents();
						foreach (StiComponent item6 in components3)
						{
							if (item6 is StiContainer { ContainerInfoV2: not null } stiContainer)
							{
								stiContainer.ContainerInfoV2.DataSourceRow = null;
								stiContainer.ContainerInfoV2.BusinessObjectCurrent = null;
							}
						}
					}
				}
			}
			if (!StiOptions.Engine.DisconnectFromDataBeforeEndRender)
			{
				DisconnectFromData(report);
			}
			FinishProgressForm(report);
			if (report.ReportPass == StiReportPass.Second)
			{
				report.ReportPass = StiReportPass.None;
			}
			if (subReportsMasterReport == null)
			{
				ClearTotals(report);
				if (report.ReportPass != StiReportPass.First)
				{
					report.ParentReport = null;
				}
			}
			if (report.Engine.ParserConversionStore != null)
			{
				report.Engine.ParserConversionStore.Clear();
				report.Engine.ParserConversionStore = null;
			}
			StiComponentsCollection components4 = report.GetComponents();
			foreach (StiComponent item7 in components4)
			{
				item7.UnPrepare();
			}
			if (hashtable2 != null)
			{
				foreach (DictionaryEntry item8 in hashtable2)
				{
					if (item8.Key is StiDataBand)
					{
						(item8.Key as StiDataBand).Rendering -= item8.Value as EventHandler;
					}
					if (item8.Key is StiPage)
					{
						(item8.Key as StiPage).Rendering -= item8.Value as EventHandler;
					}
				}
			}
			report.Engine.KeepFirstDetailTogetherTablesList.Clear();
			StiTableOfContentsV2Builder.PostProcessTableOfContents(report);
		}
	}

	private static void StiRenderProviderV2_AddAnchor_Rendering(object sender, EventArgs e)
	{
		if (sender is StiContainer { Report: not null } stiContainer && stiContainer.Report.Engine != null && stiContainer.Report.Engine.AnchorsArguments.Contains(stiContainer.Name))
		{
			string text = (string)stiContainer.Report.Engine.AnchorsArguments[stiContainer.Name];
			if (!string.IsNullOrEmpty(text))
			{
				StiText component = new StiText
				{
					Name = "**AddAnchor**",
					Page = stiContainer.Report.Pages[0]
				};
				object value = StiParser.ParseTextValue("{" + text + "}", component);
				stiContainer.Report.AddAnchor(value);
			}
		}
	}

	internal static string GetRichTextToParse(string script, StiRichText richText)
	{
		bool flag = true;
		List<string> list = ((richText != null && richText.FullConvertExpression) ? StiCodeDomExpressionHelper.GetLexemFullRtf(null, script, richText) : StiCodeDomExpressionHelper.GetLexemSimple(null, script, richText));
		StringBuilder stringBuilder = new StringBuilder();
		if (list == null || list.Count == 0)
		{
			stringBuilder.Append(script);
		}
		foreach (string item in list)
		{
			if (!flag)
			{
				stringBuilder = ((richText == null || !richText.FullConvertExpression) ? stringBuilder.Append(" + ") : stringBuilder.Append(" , "));
			}
			stringBuilder = ((item[0] != '{' || item[item.Length - 1] != '}') ? stringBuilder.AppendFormat("@\"{0}\"", item.Replace("\"", "\"\"")) : stringBuilder.Append("ToString(" + item.Substring(1, item.Length - 2).Replace("\\", "\\\\") + ")"));
			flag = false;
		}
		return stringBuilder.ToString();
	}

	internal static void ConnectToData(StiReport report)
	{
		report.StatusString = ((StiLocalization.CultureName == "en") ? "Connecting to Data" : StiLocalization.Get("Report", "ConnectingToData"));
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
			if (page is StiForm { StartMode: StiFormStartMode.OnStart, Enabled: not false })
			{
				return true;
			}
		}
		return false;
	}

	private static bool RenderFormsOnStart(StiReport report)
	{
		if (report.IsInteractionRendering)
		{
			return true;
		}
		if (StiOptions.Configuration.IsWeb || report.ReportPass == StiReportPass.First)
		{
			return true;
		}
		report.StatusString = string.Empty;
		StiDialogsProvider provider = StiDialogsProvider.GetProvider(report);
		if (provider == null)
		{
			return true;
		}
		if (!provider.Render(report, StiFormStartMode.OnStart))
		{
			StiLogService.Write(typeof(StiRenderProviderV2), "Report stopped in Dialog Forms");
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
		return true;
	}

	private static void RenderFormsOnEnd(StiReport report)
	{
		if (report.IsInteractionRendering || StiOptions.Configuration.IsWeb || report.ReportPass == StiReportPass.First)
		{
			return;
		}
		report.StatusString = string.Empty;
		StiDialogsProvider provider = StiDialogsProvider.GetProvider(report);
		if (provider == null)
		{
			return;
		}
		if (!provider.Render(report, StiFormStartMode.OnEnd))
		{
			StiLogService.Write(typeof(StiRenderProviderV2), "Report stopped in Dialog Forms");
			report.IsStopped = true;
		}
		else if (report.Progress != null)
		{
			if (!report.Progress.IsVisible)
			{
				report.Progress.Show();
			}
			report.Progress.SetAllowClose(allows: true);
		}
	}

	private static bool CheckDialogsInPreview(StiReport report)
	{
		if (report.IsInteractionRendering)
		{
			return true;
		}
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

	internal static void ClearPagesWhichLessThenFromPageAndGreaterThenToPage(StiReport report, StiRenderState state)
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
			if (report.ReportCacheMode == StiReportCacheMode.On)
			{
				report.RenderedPages.CacheMode = true;
				report.RenderedPages.CheckCacheL2();
			}
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

	internal static void ProcessPageToCache(StiReport report, StiPage page, bool final)
	{
		if (!report.RenderedPages.CacheMode || report.ReportPass == StiReportPass.First)
		{
			return;
		}
		if (!final)
		{
			if (report.RenderedPages.NotCachedPages == null)
			{
				report.RenderedPages.NotCachedPages = new List<StiPage>();
			}
			if (report.RenderedPages.NotCachedPages.IndexOf(page) == -1)
			{
				report.RenderedPages.NotCachedPages.Add(page);
			}
		}
		else if (report.Engine != null && report.Engine.PageNumbers != null && report.Engine.PageNumbers.PageNumbers.Count > 0)
		{
			StiPostProcessProviderV2.PostProcessPage(page, IsFirstPage(report, page), IsLastPage(report, page), clearPage: true);
			StiPostProcessProviderV2.PostProcessPrimitives(page);
		}
	}

	internal static bool IsFirstPage(StiReport report, StiPage page)
	{
		if (report.Engine.PageNumbers == null)
		{
			return report.RenderedPages.IndexOf(page) < 1;
		}
		int val = report.RenderedPages.IndexOf(page);
		val = Math.Max(val, 0);
		val = Math.Min(val, report.Engine.PageNumbers.PageNumbers.Count - 1);
		StiPageNumber stiPageNumber = report.Engine.PageNumbers.PageNumbers[val];
		if (stiPageNumber.TotalPageCount != -1 && stiPageNumber.PageNumber != -1)
		{
			return stiPageNumber.PageNumber == 1;
		}
		if (val != 0)
		{
			return stiPageNumber.ResetPageNumber;
		}
		return true;
	}

	internal static bool IsLastPage(StiReport report, StiPage page)
	{
		if (report.Engine.PageNumbers != null)
		{
			int val = report.RenderedPages.IndexOf(page);
			val = Math.Max(val, 0);
			val = Math.Min(val, report.Engine.PageNumbers.PageNumbers.Count - 1);
			StiPageNumber stiPageNumber = report.Engine.PageNumbers.PageNumbers[val];
			if (stiPageNumber.TotalPageCount == -1 || stiPageNumber.PageNumber == -1)
			{
				if (val != report.RenderedPages.Count - 1)
				{
					if (val + 1 < report.Engine.PageNumbers.PageNumbers.Count)
					{
						return report.Engine.PageNumbers.PageNumbers[val + 1].ResetPageNumber;
					}
					return false;
				}
				return true;
			}
			return stiPageNumber.PageNumber == stiPageNumber.TotalPageCount;
		}
		int num = report.RenderedPages.IndexOf(page);
		return num == report.RenderedPages.Count - 1;
	}

	private static void RenderFirstPass(StiReport report, StiNumberOfPass numberOfPass)
	{
		if (numberOfPass == StiNumberOfPass.DoublePass && report.ReportPass == StiReportPass.None)
		{
			report.ReportPass = StiReportPass.First;
			StiEngine engine = report.Engine;
			report.Engine = null;
			int count = report.RenderedPages.Count;
			StiPagesCollection stiPagesCollection = null;
			if (count > 0)
			{
				stiPagesCollection = new StiPagesCollection(report);
				stiPagesCollection.AddRange(report.RenderedPages);
				StiPage value = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage", new object[1] { report }) as StiPage;
				for (int i = 0; i < report.RenderedPages.Count; i++)
				{
					report.RenderedPages[i] = value;
				}
			}
			int currentPrintPage = report.CurrentPrintPage;
			StiEngine stiEngine = new StiEngine(report);
			stiEngine.PageNumbers.ClearPageNumbersOnFinish = false;
			stiEngine.PageNumbers.PageNumbers = engine.PageNumbers.PageNumbers;
			stiEngine.ParserConversionStore = (Hashtable)engine.ParserConversionStore.Clone();
			if (engine.HashDataSourceReferencesCounter != null)
			{
				stiEngine.HashDataSourceReferencesCounter = (Hashtable)engine.HashDataSourceReferencesCounter.Clone();
			}
			report.Engine = stiEngine;
			try
			{
				report.Render();
			}
			catch
			{
				report.ReportPass = StiReportPass.None;
				throw;
			}
			engine.LatestProgressValue = report.Engine.LatestProgressValue;
			report.Engine = engine;
			stiEngine.ParserConversionStore = null;
			report.Engine.PageNumbers.ProcessPageNumbers();
			report.ReportPass = StiReportPass.Second;
			report.IsRendering = true;
			report.IsRendered = false;
			report.RenderedPages.Clear();
			if (count > 0)
			{
				report.RenderedPages.AddRange(stiPagesCollection);
				stiPagesCollection.Clear();
			}
			report.CurrentPrintPage = currentPrintPage;
		}
		report.ResetAggregateFunctions();
	}

	internal static void ClearPagesForFirstPass(StiReport report)
	{
		if (report.ReportPass == StiReportPass.First && report.RenderedPages.Count >= 2)
		{
			report.RenderedPages[report.RenderedPages.Count - 2].Components.Clear();
		}
	}

	private static StiNumberOfPass GetNumberOfPass(StiReport report)
	{
		StiNumberOfPass numberOfPass = report.NumberOfPass;
		if (report.ReportPass == StiReportPass.None && numberOfPass == StiNumberOfPass.SinglePass)
		{
			if (!StiOptions.Engine.UseAdvancedPrintOnEngineV2)
			{
				if (report.GetComponents().Cast<StiComponent>().Any((StiComponent c) => c is StiTableOfContents))
				{
					return StiNumberOfPass.DoublePass;
				}
				return StiNumberOfPass.SinglePass;
			}
			StiComponentsCollection components = report.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item.PrintOn != 0 && item.PrintOn != StiPrintOnType.OnlyFirstPage && item.PrintOn != StiPrintOnType.ExceptFirstPage)
				{
					return StiNumberOfPass.DoublePass;
				}
				if (item is StiTableOfContents)
				{
					return StiNumberOfPass.DoublePass;
				}
			}
		}
		return numberOfPass;
	}

	private static void MadeCollate(StiReport report)
	{
		if (report.RenderedPages.Count < 3 || report.Collate < 2)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		if (StiOptions.Engine.UseCollateOldMode)
		{
			for (int i = 0; i < report.Collate; i++)
			{
				for (int j = i; j < report.RenderedPages.Count; j += report.Collate)
				{
					arrayList.Add(report.RenderedPages.GetPageWithoutCache(j));
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
					arrayList.Add(report.RenderedPages.GetPageWithoutCache(l));
				}
			}
		}
		for (int m = 0; m < report.RenderedPages.Count; m++)
		{
			report.RenderedPages.SetPageWithoutCache(m, arrayList[m] as StiPage);
		}
	}

	private static void MadeMirrorMargins(StiReport report)
	{
		if (report.RenderedPages.Count < 2)
		{
			return;
		}
		for (int i = 1; i < report.RenderedPages.Count; i += 2)
		{
			StiPage pageWithoutCache = report.RenderedPages.GetPageWithoutCache(i);
			if (pageWithoutCache.MirrorMargins)
			{
				pageWithoutCache.Margins = new StiMargins(pageWithoutCache.Margins.Right, pageWithoutCache.Margins.Left, pageWithoutCache.Margins.Top, pageWithoutCache.Margins.Bottom);
			}
		}
	}

	private static void InitReport(StiReport report)
	{
		report.CurrentPage = 0;
		if (report.SubReportsMasterReport == null)
		{
			report.CurrentPrintPage = 0;
		}
		report.IsStopped = false;
		report.ResetAggregateFunctions();
	}

	private static void ClearTotals(StiReport report)
	{
		report.Totals.Clear();
		if (report.ParentReport != null)
		{
			report.ParentReport.Totals.Clear();
		}
	}

	internal static void PrepareSubReportsAndDrillDownPages(StiReport report)
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
			if (item.Interaction != null && item.Interaction.DrillDownEnabled && item.Interaction.DrillDownPageGuid != null && hashtable[item.Interaction.DrillDownPageGuid] is StiPage { DrillDownActivated: false } stiPage3)
			{
				stiPage3.Skip = true;
			}
			if (!(item is StiChart))
			{
				continue;
			}
			foreach (IStiSeries item2 in ((StiChart)item).Series)
			{
				if (item2.Interaction != null && ((StiSeriesInteraction)item2.Interaction).DrillDownEnabled && ((StiSeriesInteraction)item2.Interaction).DrillDownPageGuid != null)
				{
					if (hashtable[((StiSeriesInteraction)item2.Interaction).DrillDownPageGuid] is StiPage stiPage4)
					{
						stiPage4.Skip = true;
					}
					break;
				}
			}
		}
	}

	private static void RenderReport(StiReport report, StiReport masterReport, StiRenderState state)
	{
		try
		{
			int num = 0;
			foreach (StiPage page in report.Pages)
			{
				bool enabled = page.Enabled;
				try
				{
					report.Engine.TemplatePage = page;
					report.Engine.TemplateContainer = page;
					report.CurrentPrintPage++;
					if (!page.Skip)
					{
						page.InvokeBeforePrint(page, EventArgs.Empty);
						report.Engine.SkipFirstPageBeforePrintEvent = true;
					}
					report.CurrentPrintPage--;
					if (!page.Enabled || page.Skip)
					{
						if (!page.Skip)
						{
							report.Engine.SkipFirstPageBeforePrintEvent = false;
							page.InvokeAfterPrint(page, EventArgs.Empty);
						}
						continue;
					}
					if (report.Engine.MasterEngine != null)
					{
						report.Engine.MasterEngine.TemplatePage = page;
						report.Engine.MasterEngine.TemplateContainer = page;
					}
					for (int i = 0; i < page.NumberOfCopies; i++)
					{
						report.PageCopyNumber = i + 1;
						if (i > 0)
						{
							page.PageInfoV2.RenderedCount = 0;
							foreach (StiComponent component2 in page.GetComponents())
							{
								if (component2 is IStiPageBreak { NewPageBefore: not false, SkipFirst: not false } stiPageBreak)
								{
									report.Engine.RemoveBandFromPageBreakSkipList(stiPageBreak);
								}
							}
						}
						page.PageInfoV2.IndexOfStartRenderedPages = report.RenderedPages.Count;
						page.ParentBookmark = page.Report.Bookmark;
						page.CurrentBookmark = page.ParentBookmark;
						page.ParentPointer = page.Report.Pointer;
						page.CurrentPointer = page.ParentPointer;
						if (report.RenderedPages.Count == 0 || !page.PrintOnPreviousPage)
						{
							report.Engine.FirstCallNewPage = true;
							report.Engine.NewPage();
						}
						else if (page.PrintOnPreviousPage && num != 0)
						{
							StiPage stiPage2 = report.RenderedPages[report.RenderedPages.Count - 1];
							StiContainer stiContainer = null;
							string text = $"TAG##{report.Engine.ColumnsOnPanel.CurrentColumn}";
							foreach (StiComponent component3 in stiPage2.Components)
							{
								if (component3.Name == text)
								{
									stiContainer = component3 as StiContainer;
								}
							}
							if (stiContainer != null)
							{
								stiContainer.Height = report.Engine.PositionY - stiContainer.Top;
							}
							else
							{
								StiContainer container = new StiContainer
								{
									Name = $"TAG##{report.Engine.ColumnsOnPanel.CurrentColumn}",
									Top = 0.0,
									Height = report.Engine.PositionY,
									Left = (double)(report.Engine.ColumnsOnPanel.CurrentColumn - 1) * stiPage2.GetColumnWidth(),
									Width = ((stiPage2.Columns > 0) ? stiPage2.GetColumnWidth() : stiPage2.Width),
									TagValue = stiPage2.TagValue,
									PointerValue = stiPage2.PointerValue,
									BookmarkValue = stiPage2.BookmarkValue,
									HyperlinkValue = stiPage2.HyperlinkValue,
									ToolTipValue = stiPage2.ToolTipValue,
									Guid = stiPage2.Guid
								};
								stiPage2.TagValue = null;
								stiPage2.PointerValue = null;
								stiPage2.BookmarkValue = null;
								stiPage2.HyperlinkValue = null;
								stiPage2.ToolTipValue = null;
								stiPage2.Guid = null;
								report.Engine.AddContainerToDestination(container);
							}
							StiContainer container2 = new StiContainer
							{
								Name = $"TAG##{report.Engine.ColumnsOnPanel.CurrentColumn}",
								Top = report.Engine.PositionY,
								Height = report.Engine.PositionBottomY - report.Engine.PositionY,
								Left = (double)(report.Engine.ColumnsOnPanel.CurrentColumn - 1) * page.GetColumnWidth(),
								Width = ((page.Columns > 0) ? page.GetColumnWidth() : page.Width),
								TagValue = page.TagValue,
								PointerValue = page.PointerValue,
								BookmarkValue = page.BookmarkValue,
								HyperlinkValue = page.HyperlinkValue,
								ToolTipValue = page.ToolTipValue,
								Guid = page.Guid
							};
							page.TagValue = null;
							page.PointerValue = null;
							page.BookmarkValue = null;
							page.HyperlinkValue = null;
							page.ToolTipValue = null;
							report.Engine.AddContainerToDestination(container2);
							foreach (StiComponent component4 in page.Components)
							{
								if (component4 is StiCrossLinePrimitive { Guid: not null } stiCrossLinePrimitive && stiCrossLinePrimitive.Guid != "nullGuid")
								{
									StiComponent component = stiCrossLinePrimitive.Render();
									stiPage2.Components.Add(component);
								}
							}
						}
						StiPageHelper.RenderPage(page);
					}
				}
				catch (StiStopBeforePrintException)
				{
				}
				finally
				{
					page.Enabled = enabled;
				}
				report.Engine.ProcessLastPageAfterRendering();
				StiContainer markerContainer = null;
				report.Engine.RenderFootersOnAllPages(null, 0, ref markerContainer);
				report.Engine.RenderPrintAtBottom(null, 0, null);
				report.Engine.RenderEmptyBands(report.Engine.ContainerForRender, null);
				report.Engine.EmptyBands.Clear();
				num++;
			}
			report.Engine.InvokePageAfterPrint();
			RenderTable(report);
			report.Engine.FinalClear();
			report.ProgressOfRendering = 100;
		}
		catch (StiReportRenderingStopException)
		{
		}
		catch (StiStopBeforePageException)
		{
			report.Engine.PageNumbers.PageNumbers.RemoveAt(report.Engine.PageNumbers.PageNumbers.Count - 1);
		}
	}

	private static void RenderTable(StiReport report)
	{
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		if (!report.ContainsTables)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		ArrayList arrayList = new ArrayList();
		Graphics val = null;
		double num = 0.2;
		SizeD realSize;
		foreach (StiPage renderedPage in report.RenderedPages)
		{
			num = renderedPage.GridSize;
			Hashtable hashtable4 = new Hashtable();
			Hashtable hashtable5 = new Hashtable();
			ArrayList arrayList2 = new ArrayList();
			val = renderedPage.GetMeasureGraphics();
			StiComponentsCollection components = renderedPage.GetComponents();
			for (int i = 0; i < components.Count; i++)
			{
				if (!(components[i] is IStiTableCell stiTableCell))
				{
					continue;
				}
				Hashtable hashtable6 = null;
				StiTable stiTable = stiTableCell.TableTag as StiTable;
				if (stiTable.AutoWidth == StiTableAutoWidth.None)
				{
					continue;
				}
				if (stiTable.AutoWidth == StiTableAutoWidth.Table)
				{
					if (!hashtable.ContainsKey(stiTable))
					{
						hashtable6 = new Hashtable();
						hashtable.Add(stiTable, hashtable6);
						arrayList.Add(stiTable);
					}
					else
					{
						hashtable6 = hashtable[stiTable] as Hashtable;
					}
					if (!hashtable3.ContainsKey(stiTable.Name))
					{
						ArrayList value = new ArrayList { renderedPage };
						hashtable3.Add(stiTable.Name, value);
					}
					else if (!((ArrayList)hashtable3[stiTable.Name]).Contains(renderedPage))
					{
						((ArrayList)hashtable3[stiTable.Name]).Add(renderedPage);
					}
				}
				else if (!hashtable4.ContainsKey(stiTable))
				{
					hashtable6 = new Hashtable();
					hashtable4.Add(stiTable, hashtable6);
					arrayList2.Add(stiTable);
				}
				else
				{
					hashtable6 = hashtable4[stiTable] as Hashtable;
				}
				if (!hashtable6.ContainsKey(stiTableCell.Column))
				{
					ArrayList value2 = new ArrayList();
					hashtable6.Add(stiTableCell.Column, value2);
				}
				((ArrayList)hashtable6[stiTableCell.Column]).Add(stiTableCell);
			}
			foreach (Hashtable value3 in hashtable4.Values)
			{
				StiColumnSize stiColumnSize = new StiColumnSize(value3.Count);
				object[] array = new object[value3.Count];
				value3.Keys.CopyTo(array, 0);
				Array.Sort(array);
				for (int j = 0; j < value3.Count; j++)
				{
					if (!(value3[array[j]] is ArrayList arrayList3))
					{
						continue;
					}
					double num2 = 0.0;
					foreach (StiComponent item in arrayList3)
					{
						if (((IStiTableCell)item).FixedWidth)
						{
							stiColumnSize.SetFixedColumn(j, item.Width);
							num2 = -1.0;
							break;
						}
						switch (((IStiTableCell)item).CellType)
						{
						case StiTablceCellType.CheckBox:
						case StiTablceCellType.RichText:
							if (item.Width > num2)
							{
								num2 = item.Width;
							}
							break;
						case StiTablceCellType.Image:
						{
							StiTableCellImage stiTableCellImage = item as StiTableCellImage;
							bool canShrink = stiTableCellImage.CanShrink;
							stiTableCellImage.CanShrink = true;
							realSize = stiTableCellImage.GetRealSize();
							double width = ((SizeD)(ref realSize)).Width;
							stiTableCellImage.CanShrink = canShrink;
							if (width > num2)
							{
								num2 = width;
							}
							break;
						}
						case StiTablceCellType.Text:
						{
							StiTableCell stiTableCell2 = item as StiTableCell;
							if (stiTableCell2.WordWrap)
							{
								if (stiTableCell2.Width > num2)
								{
									num2 = stiTableCell2.Width;
								}
							}
							else if (!string.IsNullOrEmpty(stiTableCell2.Text.Value))
							{
								SizeF sizeF = val.MeasureString(stiTableCell2.Text.Value, stiTableCell2.Font);
								double num3 = stiTableCell2.Margins.Left + stiTableCell2.Margins.Right;
								double num4 = report.Unit.ConvertFromHInches((double)sizeF.Width + num3);
								if (num4 > num2)
								{
									num2 = num4;
								}
							}
							break;
						}
						}
					}
					if (num2 != -1.0)
					{
						stiColumnSize.SetWidth(j, num2);
					}
				}
				ArrayList arrayList4 = value3[0] as ArrayList;
				hashtable5.Add(((IStiTableCell)arrayList4[0]).TableTag, stiColumnSize);
			}
			for (int k = 0; k < arrayList2.Count; k++)
			{
				if (!(arrayList2[k] is StiTable stiTable2))
				{
					continue;
				}
				StiColumnSize stiColumnSize2 = (StiColumnSize)hashtable5[stiTable2];
				double num5 = 0.0;
				for (int l = 0; l < stiColumnSize2.Length; l++)
				{
					num5 += stiColumnSize2.GetWidth(l);
				}
				switch (stiTable2.AutoWidthType)
				{
				case StiTableAutoWidthType.None:
					if (renderedPage.Width < num5)
					{
						int segmentPerWidth = (int)(num5 / renderedPage.Width) + 1;
						renderedPage.SegmentPerWidth = segmentPerWidth;
					}
					break;
				case StiTableAutoWidthType.FullTable:
					if (num5 < stiTable2.Width)
					{
						double num10 = stiTable2.Width - num5;
						num10 /= (double)stiColumnSize2.GetCountNotFixedColumn();
						for (int m = 0; m < stiColumnSize2.Length; m++)
						{
							stiColumnSize2.Add(m, num10);
						}
					}
					if (num5 > stiTable2.Width)
					{
						double num11 = num5 - stiTable2.Width;
						num11 /= (double)stiColumnSize2.GetCountNotFixedColumn();
						for (int n = 0; n < stiColumnSize2.Length; n++)
						{
							stiColumnSize2.Subtract(n, num11);
						}
					}
					break;
				case StiTableAutoWidthType.LastColumns:
				{
					double num6 = Math.Abs(stiTable2.Width - num5);
					int num7 = stiColumnSize2.Length - 1;
					bool flag = false;
					double num8 = num * 3.0;
					if (num5 > stiTable2.Width)
					{
						while (!flag)
						{
							if (stiColumnSize2.GetFixed(num7))
							{
								if (num7 == 0)
								{
									flag = true;
								}
								else
								{
									num7--;
								}
							}
							else if (stiColumnSize2.GetWidth(num7) - num8 < num6)
							{
								double num9 = stiColumnSize2.GetWidth(num7) - num8;
								stiColumnSize2.Subtract(num7, num9);
								num6 -= num9;
								if (num7 == 0)
								{
									flag = true;
								}
								else
								{
									num7--;
								}
							}
							else
							{
								stiColumnSize2.Subtract(num7, num6);
								flag = true;
							}
						}
					}
					else
					{
						stiColumnSize2.AddLastNotFixed(num6);
					}
					break;
				}
				}
			}
			foreach (object key in hashtable4.Keys)
			{
				Hashtable hashtable8 = hashtable4[key] as Hashtable;
				StiColumnSize stiColumnSize3 = (StiColumnSize)hashtable5[key];
				stiColumnSize3.Normalize();
				object[] array2 = new object[hashtable8.Count];
				hashtable8.Keys.CopyTo(array2, 0);
				Array.Sort(array2);
				double num12 = 0.0;
				for (int num13 = 0; num13 < hashtable8.Count; num13++)
				{
					if (!(hashtable8[array2[num13]] is ArrayList arrayList5))
					{
						return;
					}
					foreach (StiComponent item2 in arrayList5)
					{
						item2.Width = stiColumnSize3.GetWidth(num13);
						item2.Left = num12;
					}
					num12 += stiColumnSize3.GetWidth(num13);
				}
			}
		}
		if (arrayList.Count <= 0)
		{
			return;
		}
		foreach (Hashtable value4 in hashtable.Values)
		{
			StiColumnSize stiColumnSize4 = new StiColumnSize(value4.Count);
			object[] array3 = new object[value4.Count];
			value4.Keys.CopyTo(array3, 0);
			Array.Sort(array3);
			for (int num14 = 0; num14 < value4.Count; num14++)
			{
				if (!(value4[array3[num14]] is ArrayList arrayList6))
				{
					continue;
				}
				double num15 = 0.0;
				foreach (StiComponent item3 in arrayList6)
				{
					if (((IStiTableCell)item3).FixedWidth)
					{
						stiColumnSize4.SetFixedColumn(num14, item3.Width);
						num15 = -1.0;
						break;
					}
					switch (((IStiTableCell)item3).CellType)
					{
					case StiTablceCellType.CheckBox:
					case StiTablceCellType.RichText:
						if (item3.Width > num15)
						{
							num15 = item3.Width;
						}
						break;
					case StiTablceCellType.Image:
					{
						StiTableCellImage stiTableCellImage2 = item3 as StiTableCellImage;
						bool canShrink2 = stiTableCellImage2.CanShrink;
						stiTableCellImage2.CanShrink = true;
						realSize = stiTableCellImage2.GetRealSize();
						double width2 = ((SizeD)(ref realSize)).Width;
						stiTableCellImage2.CanShrink = canShrink2;
						if (width2 > num15)
						{
							num15 = width2;
						}
						break;
					}
					case StiTablceCellType.Text:
					{
						StiTableCell stiTableCell3 = item3 as StiTableCell;
						if (stiTableCell3.WordWrap)
						{
							if (stiTableCell3.Width > num15)
							{
								num15 = stiTableCell3.Width;
							}
						}
						else if (!string.IsNullOrEmpty(stiTableCell3.Text.Value))
						{
							SizeF sizeF2 = val.MeasureString(stiTableCell3.Text.Value, stiTableCell3.Font);
							double num16 = stiTableCell3.Margins.Left + stiTableCell3.Margins.Right;
							double num17 = report.Unit.ConvertFromHInches((double)sizeF2.Width + num16);
							if (num17 > num15)
							{
								num15 = num17;
							}
						}
						break;
					}
					}
				}
				if (num15 != -1.0)
				{
					stiColumnSize4.SetWidth(num14, num15);
				}
			}
			ArrayList arrayList7 = value4[0] as ArrayList;
			hashtable2.Add(((IStiTableCell)arrayList7[0]).TableTag, stiColumnSize4);
		}
		foreach (StiTable item4 in arrayList)
		{
			if (item4 == null)
			{
				continue;
			}
			StiColumnSize stiColumnSize5 = (StiColumnSize)hashtable2[item4];
			double num18 = 0.0;
			for (int num19 = 0; num19 < stiColumnSize5.Length; num19++)
			{
				num18 += stiColumnSize5.GetWidth(num19);
			}
			switch (item4.AutoWidthType)
			{
			case StiTableAutoWidthType.None:
			{
				if (!(hashtable3[item4.Name] is ArrayList arrayList8) || !(((StiPage)arrayList8[0]).Width < num18))
				{
					break;
				}
				int segmentPerWidth2 = (int)(num18 / ((StiPage)arrayList8[0]).Width) + 1;
				foreach (StiPage item5 in arrayList8)
				{
					item5.SegmentPerWidth = segmentPerWidth2;
				}
				break;
			}
			case StiTableAutoWidthType.FullTable:
				if (num18 < item4.Width)
				{
					double num24 = item4.Width - num18;
					num24 /= (double)stiColumnSize5.GetCountNotFixedColumn();
					for (int num25 = 0; num25 < stiColumnSize5.Length; num25++)
					{
						stiColumnSize5.Add(num25, num24);
					}
				}
				if (num18 > item4.Width)
				{
					double num26 = num18 - item4.Width;
					num26 /= (double)stiColumnSize5.GetCountNotFixedColumn();
					for (int num27 = 0; num27 < stiColumnSize5.Length; num27++)
					{
						stiColumnSize5.Subtract(num27, num26);
					}
				}
				break;
			case StiTableAutoWidthType.LastColumns:
			{
				double num20 = Math.Abs(item4.Width - num18);
				int num21 = stiColumnSize5.Length - 1;
				bool flag2 = false;
				double num22 = num * 3.0;
				if (num18 > item4.Width)
				{
					while (!flag2)
					{
						if (stiColumnSize5.GetFixed(num21))
						{
							if (num21 == 0)
							{
								flag2 = true;
							}
							else
							{
								num21--;
							}
						}
						else if (stiColumnSize5.GetWidth(num21) - num22 < num20)
						{
							double num23 = stiColumnSize5.GetWidth(num21) - num22;
							stiColumnSize5.Subtract(num21, num23);
							num20 -= num23;
							if (num21 == 0)
							{
								flag2 = true;
							}
							else
							{
								num21--;
							}
						}
						else
						{
							stiColumnSize5.Subtract(num21, num20);
							flag2 = true;
						}
					}
				}
				else
				{
					stiColumnSize5.AddLastNotFixed(num20);
				}
				break;
			}
			}
		}
		foreach (object key2 in hashtable.Keys)
		{
			Hashtable hashtable10 = hashtable[key2] as Hashtable;
			object[] array4 = new object[hashtable10.Count];
			hashtable10.Keys.CopyTo(array4, 0);
			Array.Sort(array4);
			StiColumnSize stiColumnSize6 = (StiColumnSize)hashtable2[key2];
			stiColumnSize6.Normalize();
			double num28 = 0.0;
			for (int num29 = 0; num29 < hashtable10.Count; num29++)
			{
				if (!(hashtable10[array4[num29]] is ArrayList arrayList9))
				{
					return;
				}
				foreach (StiComponent item6 in arrayList9)
				{
					item6.Width = stiColumnSize6.GetWidth(num29);
					item6.Left = num28;
				}
				num28 += stiColumnSize6.GetWidth(num29);
			}
		}
	}

	private static void FinishProgressForm(StiReport report)
	{
		if (report.ReportPass != StiReportPass.First)
		{
			report.StatusString = ((StiLocalization.CultureName == "en") ? "Finishing Report" : StiLocalization.Get("Report", "FinishingReport"));
			if (report.Progress != null)
			{
				report.Progress.HideProgressBar();
				report.Progress.SetAllowClose(allows: false);
			}
		}
	}
}
