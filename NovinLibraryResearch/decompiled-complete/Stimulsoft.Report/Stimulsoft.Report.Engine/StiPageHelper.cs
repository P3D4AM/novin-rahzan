using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Engine;

internal class StiPageHelper
{
	public const string CheckCanShrinkLaterMark = "*CheckCanShrinkLaterMark*";

	internal static void CreateListOfDataBands(StiPage page, List<StiDataBand> dataBandsOnPage, List<StiDataBand> dataBandsInContainers, List<StiSubReport> subReportsOnPage, List<StiCrossTab> crossTabsOnPage)
	{
		string text = null;
		StiComponentsCollection components = page.GetComponents();
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent item2 in components)
			{
				if (item2 is StiDataBand stiDataBand && (stiDataBand.MasterComponent == null || stiDataBand.MasterComponent == page.PageInfoV2.MasterDataBand))
				{
					if (stiDataBand is StiTable && !stiDataBand.Enabled)
					{
						continue;
					}
					bool flag = false;
					if (!stiDataBand.IsBusinessObjectEmpty)
					{
						if (stiDataBand.BusinessObject.ParentBusinessObject != null)
						{
							StiDataBand stiDataBand2 = null;
							if (stiDataBand.Parent is StiPage && dataBandsOnPage.Count > 0)
							{
								stiDataBand2 = dataBandsOnPage[dataBandsOnPage.Count - 1];
							}
							else if (dataBandsInContainers.Count > 0)
							{
								stiDataBand2 = dataBandsInContainers[dataBandsInContainers.Count - 1];
							}
							if (stiDataBand2 != null)
							{
								if (!stiDataBand2.IsBusinessObjectEmpty && stiDataBand2.BusinessObjectGuid == stiDataBand.BusinessObject.ParentBusinessObject.Guid)
								{
									flag = true;
								}
								else if (stiDataBand.BusinessObject.ParentBusinessObject.Guid == text)
								{
									flag = true;
								}
							}
						}
						text = stiDataBand.BusinessObjectGuid;
					}
					if (!flag)
					{
						if (stiDataBand.Parent is StiPage)
						{
							dataBandsOnPage.Add(stiDataBand);
						}
						else if (StiSubReportsHelper.GetParentBand(stiDataBand) == null)
						{
							dataBandsInContainers.Add(stiDataBand);
						}
						text = null;
					}
				}
				if (item2 is StiSubReport { Enabled: not false } stiSubReport && StiSubReportsHelper.GetParentBand(stiSubReport) == null && !page.PrintOnPreviousPage)
				{
					subReportsOnPage.Add(stiSubReport);
				}
				if (item2 is StiCrossTab item && item2.Enabled && StiSubReportsHelper.GetParentBand(item2 as StiContainer) == null)
				{
					crossTabsOnPage.Add(item);
				}
			}
		}
		bool flag2 = true;
		while (flag2)
		{
			flag2 = false;
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < dataBandsOnPage.Count; i++)
			{
				StiDataBand stiDataBand3 = dataBandsOnPage[i];
				if (stiDataBand3.IsBusinessObjectEmpty)
				{
					continue;
				}
				StiBusinessObject businessObject = stiDataBand3.BusinessObject;
				if (businessObject.ParentBusinessObject != null && hashtable[businessObject.ParentBusinessObject] == null && stiDataBand3.MasterComponent == null)
				{
					StiDataBand stiDataBand4 = new StiDataBand
					{
						BusinessObjectGuid = businessObject.ParentBusinessObject.Guid,
						Height = 0.0,
						Page = page,
						Parent = stiDataBand3.Parent,
						Name = stiDataBand3.Name + "_" + i
					};
					stiDataBand4.Prepare();
					if (!stiDataBand4.DataBandInfoV2.DetailDataBands.Contains(stiDataBand3))
					{
						stiDataBand4.DataBandInfoV2.DetailDataBands.Add(stiDataBand3);
					}
					dataBandsOnPage[i] = stiDataBand4;
					flag2 = true;
				}
				else
				{
					hashtable[businessObject] = businessObject;
				}
			}
		}
	}

	internal static List<StiReportTitleBand> GetReportTitles(StiPage page)
	{
		List<StiReportTitleBand> list = new List<StiReportTitleBand>();
		if (page.TitleBeforeHeader)
		{
			return list;
		}
		lock (((ICollection)page.Components).SyncRoot)
		{
			foreach (StiComponent component in page.Components)
			{
				if (component is StiReportTitleBand item)
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	internal static List<StiReportSummaryBand> GetReportSummaries(StiPage page)
	{
		List<StiReportSummaryBand> list = new List<StiReportSummaryBand>();
		lock (((ICollection)page.Components).SyncRoot)
		{
			foreach (StiComponent component in page.Components)
			{
				if (component is StiReportSummaryBand item)
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	internal static void RenderSimpleComponents(StiPage page, StiContainer outContainer)
	{
		int num = 0;
		double num2 = 0.0;
		if (page != null)
		{
			num = ((page.Columns < 2) ? 1 : page.Columns);
			num2 = page.GetColumnWidth() + page.ColumnGaps;
		}
		for (int i = 0; i < num; i++)
		{
			page.Report.Column = i + 1;
			lock (((ICollection)page.Components).SyncRoot)
			{
				foreach (StiComponent component in page.Components)
				{
					if (component.ComponentType != 0)
					{
						continue;
					}
					component.ParentBookmark = outContainer.CurrentBookmark;
					component.ParentPointer = outContainer.CurrentPointer;
					StiComponent stiComponent2 = null;
					Hashtable hashtable = new Hashtable();
					if (CheckContainerForBandsAndOtherContainers(component, hashtable) && hashtable.Count > 0)
					{
						foreach (StiComponent key in hashtable.Keys)
						{
							key.CanShrink = false;
						}
						stiComponent2 = component.Render();
						foreach (StiComponent key2 in hashtable.Keys)
						{
							key2.CanShrink = true;
							if (stiComponent2 != null && stiComponent2.TagValue == null)
							{
								stiComponent2.TagValue = "*CheckCanShrinkLaterMark*";
							}
						}
						hashtable.Clear();
					}
					else
					{
						stiComponent2 = component.Render();
					}
					if (stiComponent2 == null)
					{
						continue;
					}
					if (page != null && page.Columns > 1 && !(stiComponent2 is StiCrossLinePrimitive))
					{
						if (page.RightToLeft)
						{
							stiComponent2.Left += page.Width - num2 * (double)(i + 1) + page.ColumnGaps;
						}
						else
						{
							stiComponent2.Left += num2 * (double)i;
						}
					}
					outContainer.Components.Add(stiComponent2);
					if (!StiOptions.Engine.AllowInteractionInChartWithComponents || !(stiComponent2 is StiChart stiChart) || stiChart.ChartInfoV2.InteractiveComps == null)
					{
						continue;
					}
					foreach (StiText interactiveComp in stiChart.ChartInfoV2.InteractiveComps)
					{
						interactiveComp.Left += stiComponent2.Left;
						interactiveComp.Top += stiComponent2.Top;
						outContainer.Components.Add(interactiveComp);
					}
				}
			}
		}
		StiContainerHelper.CheckSize(page.Report.Engine.ContainerForRender);
		Hashtable hashtable2 = new Hashtable();
		StiComponentsCollection components = outContainer.GetComponents();
		lock (((ICollection)page.Components).SyncRoot)
		{
			foreach (StiComponent item in components)
			{
				if (item is StiContainer stiContainer)
				{
					ArrayList arrayList = hashtable2[stiContainer.Name] as ArrayList;
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						hashtable2[stiContainer.Name] = arrayList;
					}
					arrayList.Add(stiContainer);
				}
			}
		}
		foreach (ArrayList value in hashtable2.Values)
		{
			int num3 = 1;
			lock (((ICollection)value).SyncRoot)
			{
				foreach (StiContainer item2 in value)
				{
					item2.ContainerInfoV2.RenderStep = num3;
					num3++;
				}
			}
		}
		page.Report.Column = 1;
		outContainer.DockToContainer();
	}

	private static bool CheckContainerForBandsAndOtherContainers(StiComponent component, Hashtable hash)
	{
		if (component is StiContainer stiContainer && stiContainer.Components.Count > 0)
		{
			if (stiContainer.CanShrink)
			{
				hash[stiContainer] = null;
			}
			{
				IEnumerator enumerator = stiContainer.Components.GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						StiComponent stiComponent = (StiComponent)enumerator.Current;
						if (stiComponent is StiBand)
						{
							return true;
						}
						if (stiComponent == stiContainer)
						{
							return false;
						}
						if (CheckContainerForBandsAndOtherContainers(stiComponent, hash))
						{
							return true;
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
		return false;
	}

	internal static void PrepareBookmark(StiPage page)
	{
		page.ParentBookmark = page.Report.Bookmark;
		page.DoBookmark();
	}

	internal static void PreparePointer(StiPage page)
	{
		page.ParentPointer = page.Report.Pointer;
		page.DoPointer();
	}

	internal static void RenderPage(StiPage page)
	{
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		page.InvokeBeginRender();
		if (page.ResetPageNumber)
		{
			if (page.PageInfoV2 != null && page.PageInfoV2.IndexOfStartRenderedPages != -1)
			{
				page.Report.Engine.PageNumbers.ResetPageNumber(page.PageInfoV2.IndexOfStartRenderedPages);
			}
			else
			{
				page.Report.Engine.PageNumbers.ResetPageNumber();
			}
		}
		List<StiReportTitleBand> reportTitles = GetReportTitles(page);
		List<StiReportSummaryBand> reportSummaries = GetReportSummaries(page);
		List<StiDataBand> list = new List<StiDataBand>();
		List<StiDataBand> list2 = new List<StiDataBand>();
		List<StiSubReport> list3 = new List<StiSubReport>();
		List<StiCrossTab> list4 = new List<StiCrossTab>();
		CreateListOfDataBands(page, list, list2, list3, list4);
		StiEngine engine = page.Report.Engine;
		lock (((ICollection)list2).SyncRoot)
		{
			foreach (StiDataBand item in list2)
			{
				StiEngine stiEngine = engine.Threads.CreateContainerEngine(item.Parent.Name, page.Report, engine, page.PageInfoV2.IndexOfStartRenderedPages);
				page.Report.Engine.SlaveEngines[item.Parent.Name] = stiEngine;
				stiEngine.PrintOnAllPagesIgnoreList = engine.PrintOnAllPagesIgnoreList;
			}
		}
		lock (((ICollection)list3).SyncRoot)
		{
			foreach (StiSubReport item2 in list3)
			{
				StiEngine value = engine.Threads.CreateContainerEngine(item2.Name, page.Report, engine, page.PageInfoV2.IndexOfStartRenderedPages);
				page.Report.Engine.SlaveEngines[item2.Name] = value;
			}
		}
		lock (((ICollection)list4).SyncRoot)
		{
			foreach (StiCrossTab item3 in list4)
			{
				StiEngine value2 = engine.Threads.CreateContainerEngine(item3.Name, page.Report, engine, page.PageInfoV2.IndexOfStartRenderedPages);
				page.Report.Engine.SlaveEngines[item3.Name] = value2;
			}
		}
		if (list.Count > 0)
		{
			int num = 1;
			lock (((ICollection)list).SyncRoot)
			{
				foreach (StiDataBand item4 in list)
				{
					if (item4 is StiTable && !item4.Enabled)
					{
						continue;
					}
					try
					{
						if (num == 1)
						{
							page.Report.Engine.IsFirstDataBandOnPage = true;
							item4.DataBandInfoV2.ReportTitles = reportTitles;
						}
						if (num == list.Count)
						{
							page.Report.Engine.IsLastDataBandOnPage = true;
							item4.DataBandInfoV2.ReportSummaries = reportSummaries;
						}
						item4.ParentBookmark = ((page.Report.Engine.Page != null) ? page.Report.Engine.Page.CurrentBookmark : page.CurrentBookmark);
						item4.ParentPointer = ((page.Report.Engine.Page != null) ? page.Report.Engine.Page.CurrentPointer : page.CurrentPointer);
						item4.RenderMaster();
					}
					finally
					{
						item4.DataBandInfoV2.ReportTitles = null;
						item4.DataBandInfoV2.ReportSummaries = null;
						page.Report.Engine.IsFirstDataBandOnPage = false;
						page.Report.Engine.IsLastDataBandOnPage = false;
					}
					num++;
				}
			}
		}
		else
		{
			if (reportTitles != null)
			{
				lock (((ICollection)reportTitles).SyncRoot)
				{
					foreach (StiReportTitleBand item5 in reportTitles)
					{
						if (item5.PrintIfEmpty)
						{
							item5.ParentBookmark = ((page.Report.Engine.Page != null) ? page.Report.Engine.Page.CurrentBookmark : page.CurrentBookmark);
							bool flag = item5.DoBookmark();
							item5.ParentPointer = ((page.Report.Engine.Page != null) ? page.Report.Engine.Page.CurrentPointer : page.CurrentPointer);
							item5.DoPointer(!flag);
							page.Report.Engine.RenderBand(item5);
						}
					}
				}
			}
			if (reportSummaries != null)
			{
				lock (((ICollection)reportSummaries).SyncRoot)
				{
					foreach (StiReportSummaryBand item6 in reportSummaries)
					{
						if (item6.PrintIfEmpty)
						{
							item6.ParentBookmark = ((page.Report.Engine.Page != null) ? page.Report.Engine.Page.CurrentBookmark : page.CurrentBookmark);
							bool flag2 = item6.DoBookmark();
							item6.ParentPointer = ((page.Report.Engine.Page != null) ? page.Report.Engine.Page.CurrentPointer : page.CurrentPointer);
							item6.DoPointer(!flag2);
							page.Report.Engine.RenderBand(item6);
						}
					}
				}
			}
		}
		try
		{
			foreach (StiDataBand item7 in list2)
			{
				StiEngine stiEngine2 = engine.SlaveEngines[item7.Parent.Name] as StiEngine;
				if (stiEngine2.FreeSpace == 0.0)
				{
					stiEngine2.FreeSpace = item7.Parent.Height;
				}
				if (item7.Parent != null && item7.Parent.CanGrow && item7.Parent.Parent is StiPage && stiEngine2.FreeSpace < 50000000000.0 && engine.FreeSpace < 50000000000.0)
				{
					stiEngine2.FreeSpace = engine.PositionBottomY - item7.Parent.Top;
					if (stiEngine2.ContainerForRender != null)
					{
						stiEngine2.ContainerForRender.Height = stiEngine2.FreeSpace;
					}
					if (page.UnlimitedHeight)
					{
						stiEngine2.FreeSpace = 99999999900.0;
					}
				}
				page.Report.Engine = stiEngine2;
				if (stiEngine2.ContainerForRender != null)
				{
					item7.ParentBookmark = stiEngine2.ContainerForRender.CurrentBookmark;
					item7.ParentPointer = stiEngine2.ContainerForRender.CurrentPointer;
				}
				item7.RenderMaster();
			}
		}
		finally
		{
			page.Report.Engine = engine;
		}
		try
		{
			foreach (StiSubReport item8 in list3)
			{
				StiEngine stiEngine3 = engine.SlaveEngines[item8.Name] as StiEngine;
				page.Report.Engine = stiEngine3;
				StiContainer stiContainer = new StiContainer(item8.ClientRectangle)
				{
					Name = "Temp Container",
					CanBreak = true
				};
				StiSubReportsHelper.RenderSubReport(stiContainer, item8);
				double num2 = 0.0;
				foreach (StiComponent component in stiContainer.Components)
				{
					num2 = Math.Max(component.Bottom, num2);
				}
				stiContainer.Height = num2;
				if (stiEngine3.ContainerForRender != null)
				{
					stiEngine3.RenderContainer(stiContainer);
				}
			}
		}
		finally
		{
			page.Report.Engine = engine;
		}
		try
		{
			foreach (StiCrossTab item9 in list4)
			{
				StiEngine stiEngine4 = engine.SlaveEngines[item9.Name] as StiEngine;
				page.Report.Engine = stiEngine4;
				StiCrossTabParams stiCrossTabParams = new StiCrossTabParams();
				StiCrossTabHelper.CreateCross(item9);
				while (!stiCrossTabParams.RenderingIsFinished && stiEngine4.ContainerForRender != null)
				{
					if (item9.HorAlignment == StiCrossHorAlignment.None)
					{
						stiEngine4.ContainerForRender.Width = stiEngine4.ContainerForRender.Parent.Width - stiEngine4.ContainerForRender.Left;
					}
					else
					{
						stiEngine4.ContainerForRender.Width = stiEngine4.ContainerForRender.Parent.Width;
					}
					bool flag3 = false;
					if (stiEngine4.ContainerForRender.Parent.CanShrink && stiEngine4.ContainerForRender.Parent.Height == 0.0)
					{
						StiComponent stiComponent2 = page.GetComponents()[stiEngine4.ContainerForRender.Parent.Name];
						flag3 = true;
						stiEngine4.ContainerForRender.Parent.Width = stiComponent2.Width;
						stiEngine4.ContainerForRender.Parent.Height = stiComponent2.Height;
						stiEngine4.ContainerForRender.Width = stiComponent2.Width;
						stiEngine4.ContainerForRender.Height = stiComponent2.Height;
					}
					else
					{
						stiEngine4.ContainerForRender.Height = stiEngine4.ContainerForRender.Parent.Height - stiEngine4.ContainerForRender.Top;
					}
					RectangleD clientRectangle = stiEngine4.ContainerForRender.ClientRectangle;
					stiCrossTabParams.DestinationRectangle = ((RectangleD)(ref clientRectangle)).ToRectangleM();
					stiCrossTabParams.DestinationContainer = stiEngine4.ContainerForRender;
					StiCrossTabV2Builder stiCrossTabV2Builder = StiV2Builder.GetBuilder(typeof(StiCrossTab)) as StiCrossTabV2Builder;
					stiCrossTabV2Builder.RenderCrossTab(stiCrossTabParams, item9);
					if (flag3)
					{
						SizeD actualSize = stiEngine4.ContainerForRender.Parent.GetActualSize();
						stiEngine4.ContainerForRender.Parent.Width = ((SizeD)(ref actualSize)).Width;
						stiEngine4.ContainerForRender.Parent.Height = ((SizeD)(ref actualSize)).Height;
					}
					if (!stiCrossTabParams.RenderingIsFinished)
					{
						if (stiEngine4.ContainerForRender.Parent.Height > 99999999900.0)
						{
							StiNewPageContainer stiNewPageContainer = new StiNewPageContainer
							{
								Top = stiEngine4.PositionY,
								Height = ((stiEngine4.TemplatePage.Height < 99999999900.0) ? stiEngine4.TemplatePage.Height : stiEngine4.TemplatePage.Unit.ConvertFromHInches(1170.0))
							};
							stiEngine4.PositionY += stiNewPageContainer.Height;
							stiEngine4.FreeSpace -= stiNewPageContainer.Height;
							stiEngine4.OffsetNewColumnY = stiEngine4.PositionY - stiEngine4.StaticBands.ReservedPositionY;
							stiEngine4.ContainerForRender.Parent.Components.Add(stiNewPageContainer);
							StiContainer stiContainer2 = stiEngine4.ContainerForRender.Clone(cloneProperties: true, cloneComponents: false) as StiContainer;
							stiContainer2.Top = stiEngine4.PositionY;
							stiEngine4.ContainerForRender.Parent.Components.Add(stiContainer2);
							stiEngine4.ContainerForRender = stiContainer2;
						}
						else
						{
							stiEngine4.NewDestination();
						}
					}
				}
				StiCrossTabHelper.ClearCross(item9);
			}
		}
		finally
		{
			page.Report.Engine = engine;
		}
		page.InvokeEndRender();
		page.InvokeColumnEndRender();
		page.IsRendered = true;
		lock (engine.SlaveEngines.Values.SyncRoot)
		{
			foreach (StiEngine value3 in engine.SlaveEngines.Values)
			{
				value3.FinalClear();
			}
		}
		page.Report.Engine.SlaveEngines.Clear();
	}

	public static void RenderOverlays(StiPage masterPage, StiPage renderedPage)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected I4, but got Unknown
		StiComponentsCollection overlays = masterPage.PageInfoV2.Overlays;
		if (overlays == null)
		{
			return;
		}
		List<StiComponent> list = new List<StiComponent>();
		List<StiComponent> list2 = new List<StiComponent>();
		List<StiComponent> list3 = new List<StiComponent>();
		foreach (StiOverlayBand item in overlays)
		{
			StiComponent stiComponent = item.Render();
			if (stiComponent != null)
			{
				stiComponent.DockStyle = StiDockStyle.None;
				renderedPage.Components.Add(stiComponent);
				StiVertAlignment vertAlignment = item.VertAlignment;
				switch ((int)vertAlignment)
				{
				case 0:
					list.Add(stiComponent);
					break;
				case 1:
					list2.Add(stiComponent);
					break;
				case 2:
					list3.Add(stiComponent);
					break;
				}
			}
		}
		double num = 0.0 - masterPage.Margins.Top;
		lock (((ICollection)list).SyncRoot)
		{
			foreach (StiComponent item2 in list)
			{
				item2.Top = num;
				num += item2.Height;
			}
		}
		double num2 = masterPage.Height + masterPage.Margins.Bottom;
		lock (((ICollection)list3).SyncRoot)
		{
			foreach (StiComponent item3 in list3)
			{
				item3.Top = num2 - item3.Height;
				num2 -= item3.Height;
			}
		}
		double num3 = 0.0;
		lock (((ICollection)list2).SyncRoot)
		{
			foreach (StiComponent item4 in list2)
			{
				num3 += item4.Height;
			}
		}
		num = (masterPage.Height - num3) / 2.0;
		lock (((ICollection)list2).SyncRoot)
		{
			foreach (StiComponent item5 in list2)
			{
				item5.Top = num;
				num += item5.Height;
			}
		}
	}

	internal static StiPage GetPageFromTemplate(StiPage templatePage)
	{
		StiPage stiPage = templatePage.Clone(cloneProperties: false, cloneComponents: false) as StiPage;
		StiWatermark watermark = stiPage.Watermark;
		if (watermark != null)
		{
			if (StiOptions.Engine.Watermark.AllowExpression)
			{
				watermark.Text = StiExpressionHelper.ParseText(stiPage, watermark.Text);
			}
			if (!string.IsNullOrEmpty(watermark.EnabledExpression))
			{
				watermark.Enabled = StiExpressionHelper.ParseBool(stiPage, watermark.EnabledExpression);
			}
		}
		if (templatePage.PageInfoV2.RenderedCount > 0)
		{
			stiPage.Guid = StiGuidUtils.NewGuid();
		}
		stiPage.InvokeEvents();
		RenderOverlays(templatePage, stiPage);
		templatePage.PageInfoV2.RenderedCount++;
		return stiPage;
	}
}
