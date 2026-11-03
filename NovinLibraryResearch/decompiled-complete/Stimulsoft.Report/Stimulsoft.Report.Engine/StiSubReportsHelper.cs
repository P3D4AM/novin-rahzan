using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Engine;

public class StiSubReportsHelper
{
	public const double SpecialSubReportHeight = 100000000000.0;

	public static bool RenderExternalSubReportsWithoutHelpOfUnlimitedHeightPagesOldMode;

	public static StiDataBand GetMasterDataBand(StiContainer parent)
	{
		parent = parent.Parent;
		while (true)
		{
			if (parent is StiDataBand)
			{
				return parent as StiDataBand;
			}
			if (parent is StiPage)
			{
				return null;
			}
			if (parent == null)
			{
				break;
			}
			if (parent is StiChildBand)
			{
				StiContainer master = (parent as StiChildBand).GetMaster();
				if (master != null)
				{
					parent = master;
					continue;
				}
			}
			parent = parent.Parent;
		}
		return null;
	}

	public static StiBand GetParentBand(StiContainer parent)
	{
		parent = parent.Parent;
		while (true)
		{
			if (parent is StiBand)
			{
				return parent as StiBand;
			}
			if (parent is StiPage)
			{
				return null;
			}
			if (parent == null)
			{
				break;
			}
			parent = parent.Parent;
		}
		return null;
	}

	public static void RenderSubReport(StiContainer containerOfSubReport, StiSubReport subReport)
	{
		StiPage subReportPage = subReport.SubReportPage;
		double height = subReportPage?.Height ?? 0.0;
		StiEngine engine = subReport.Report.Engine;
		if (subReportPage != null)
		{
			subReportPage.PageInfoV2.MasterDataBand = GetMasterDataBand(subReport);
		}
		foreach (StiDataSource dataSource in subReport.Report.Dictionary.DataSources)
		{
			dataSource.InitForSubreport = true;
		}
		if (subReportPage != null)
		{
			subReportPage.UnlimitedBreakable = false;
		}
		try
		{
			StiReport externalSubReport = subReport.GetExternalSubReport();
			StiFillParametersEventArgs stiFillParametersEventArgs = new StiFillParametersEventArgs();
			subReport.InvokeFillParameters(subReport, stiFillParametersEventArgs);
			if (stiFillParametersEventArgs.Value != null && stiFillParametersEventArgs.Value.Count > 0)
			{
				StiReport stiReport = externalSubReport ?? subReport.Report;
				foreach (KeyValuePair<string, object> item in stiFillParametersEventArgs.Value)
				{
					stiReport[item.Key] = item.Value;
				}
			}
			if (externalSubReport != null)
			{
				StiContainer stiContainer = RenderExternalSubReport(subReport, externalSubReport);
				containerOfSubReport.Components.AddRange(stiContainer.Components);
			}
			else
			{
				StiContainer stiContainer2 = RenderInternalSubReport(subReport);
				containerOfSubReport.Components.AddRange(stiContainer2.Components);
			}
			CorrectComponentRecursive(containerOfSubReport, containerOfSubReport.Width);
		}
		finally
		{
			subReport.Report.Engine = engine;
			if (subReportPage != null)
			{
				subReportPage.Height = height;
				subReportPage.PageInfoV2.MasterDataBand = null;
			}
			foreach (StiDataSource dataSource2 in subReport.Report.Dictionary.DataSources)
			{
				dataSource2.InitForSubreport = false;
			}
		}
	}

	private static void CorrectComponentRecursive(StiContainer container, double right)
	{
		lock (((ICollection)container.Components).SyncRoot)
		{
			foreach (StiComponent component in container.Components)
			{
				if (component.Left > right)
				{
					component.Left = right;
				}
				if (component.Right > right)
				{
					component.Width = right - component.Left;
				}
				if (component is StiContainer stiContainer)
				{
					CorrectComponentRecursive(stiContainer, right - stiContainer.Left);
				}
			}
		}
	}

	private static StiContainer RenderInternalSubReport(StiSubReport subReport)
	{
		StiPage subReportPage = subReport.SubReportPage;
		StiContainer stiContainer = new StiContainer();
		if (subReportPage == null)
		{
			return stiContainer;
		}
		stiContainer.Width = subReportPage.Width;
		stiContainer.Height = 100000000000.0;
		StiBookmark currentBookmark = subReportPage.CurrentBookmark;
		StiBookmark parentBookmark = subReportPage.ParentBookmark;
		StiBookmark currentPointer = subReportPage.CurrentPointer;
		StiBookmark parentPointer = subReportPage.ParentPointer;
		subReportPage.CurrentBookmark = subReport.CurrentBookmark;
		subReportPage.ParentBookmark = subReport.ParentBookmark;
		stiContainer.CurrentBookmark = subReport.ParentBookmark;
		subReportPage.CurrentPointer = subReport.CurrentPointer;
		subReportPage.ParentPointer = subReport.ParentPointer;
		stiContainer.CurrentPointer = subReport.ParentPointer;
		StiEngine stiEngine = new StiEngine(subReport.Report);
		stiEngine.ParserConversionStore = (Hashtable)subReport.Report.Engine.ParserConversionStore.Clone();
		if (subReport.Report.Engine.HashDataSourceReferencesCounter != null)
		{
			stiEngine.HashDataSourceReferencesCounter = (Hashtable)subReport.Report.Engine.HashDataSourceReferencesCounter.Clone();
		}
		stiEngine.TemplatePage = subReportPage;
		stiEngine.TemplateContainer = subReport.SubReportPage;
		stiEngine.ContainerForRender = stiContainer;
		subReport.Report.Engine = stiEngine;
		stiEngine.SetNewPageParameters();
		stiEngine.FreeSpace = 100000000000.0;
		stiEngine.PositionBottomY = 100000000000.0;
		stiEngine.NewList();
		StiPageHelper.RenderPage(subReportPage);
		stiEngine.FinalClear();
		stiEngine.ParserConversionStore = null;
		double num = stiEngine.PositionBottomY - stiEngine.PositionY;
		lock (((ICollection)stiContainer.Components).SyncRoot)
		{
			foreach (StiComponent component in stiContainer.Components)
			{
				if (component.Top >= stiEngine.PositionY && component.Top - num >= 0.0)
				{
					component.Top -= num;
				}
			}
		}
		StiPostProcessProviderV2.PostProcessPrimitivesInContainer(stiContainer);
		subReportPage.CurrentBookmark = currentBookmark;
		subReportPage.ParentBookmark = parentBookmark;
		subReportPage.CurrentPointer = currentPointer;
		subReportPage.ParentPointer = parentPointer;
		return stiContainer;
	}

	private static StiContainer RenderExternalSubReport(StiSubReport subReport, StiReport externalReport)
	{
		StiContainer stiContainer = new StiContainer();
		if (externalReport != null)
		{
			bool renderExternalSubReportsWithHelpOfUnlimitedHeightPages = StiOptions.Engine.RenderExternalSubReportsWithHelpOfUnlimitedHeightPages;
			if (renderExternalSubReportsWithHelpOfUnlimitedHeightPages)
			{
				foreach (StiPage page in externalReport.Pages)
				{
					page.UnlimitedHeight = true;
					page.UnlimitedBreakable = false;
					page.Height = 100000000000.0;
				}
			}
			if (!externalReport.IsDocument)
			{
				externalReport.Render(showProgress: false);
			}
			double num = 90000000000.0;
			double num2 = 0.0;
			double num3 = 0.0;
			int num4 = 0;
			foreach (StiPage renderedPage in externalReport.RenderedPages)
			{
				double num5 = 0.0;
				double num6 = 100000000000.0;
				double num7 = 0.0;
				double num8 = 0.0;
				foreach (StiComponent component in renderedPage.Components)
				{
					if (component.Top > num)
					{
						num6 = Math.Min(component.Top, num6);
					}
					else
					{
						num5 = Math.Max(component.Bottom, num5);
					}
					if (!renderExternalSubReportsWithHelpOfUnlimitedHeightPages)
					{
						if (component.Right > num7)
						{
							num7 = component.Right;
						}
						if (component.Bottom > num8)
						{
							num8 = component.Bottom;
						}
					}
				}
				double num9 = num6 - num5;
				if (renderExternalSubReportsWithHelpOfUnlimitedHeightPages || RenderExternalSubReportsWithoutHelpOfUnlimitedHeightPagesOldMode || renderedPage.UnlimitedHeight)
				{
					foreach (StiComponent component2 in renderedPage.Components)
					{
						component2.Top += num3;
						component2.Left += num2;
						component2.GrowToHeight = false;
						component2.CanShrink = false;
						if (component2.Top >= num)
						{
							component2.Top -= num9;
						}
					}
					stiContainer.Components.AddRange(renderedPage.Components);
				}
				else
				{
					StiContainer stiContainer2 = new StiContainer();
					stiContainer2.Name = $"SubPage{num4++}";
					stiContainer2.Width = num7;
					stiContainer2.Height = num8;
					stiContainer2.Top = num3;
					stiContainer2.Left = num2;
					stiContainer2.Components.AddRange(renderedPage.Components);
					stiContainer2.Page = subReport.Page;
					stiContainer2.TagValue = "*NotCheckSizeMark*";
					foreach (StiComponent component3 in renderedPage.Components)
					{
						component3.GrowToHeight = false;
						component3.CanShrink = false;
						component3.Page = subReport.Page;
						if (component3.Top >= num)
						{
							component3.Top -= num9;
						}
					}
					stiContainer.Components.Add(stiContainer2);
					if (stiContainer2.Height < subReport.Height && num4 < externalReport.RenderedPages.Count)
					{
						StiNewPageContainer stiNewPageContainer = new StiNewPageContainer
						{
							Top = num3 + num8,
							Height = subReport.Height
						};
						stiContainer.Components.Add(stiNewPageContainer);
						num3 += stiNewPageContainer.Height;
					}
				}
				renderedPage.Components.Clear();
				num3 = ((!(renderedPage.Height > num)) ? ((!renderExternalSubReportsWithHelpOfUnlimitedHeightPages && !RenderExternalSubReportsWithoutHelpOfUnlimitedHeightPagesOldMode) ? (num3 + num8) : (num3 + renderedPage.Height)) : (num3 + (num5 + (100000000000.0 - num6))));
			}
		}
		return stiContainer;
	}

	public static void RenderDataBandsInContainer(StiContainer containerOfDataBands, StiContainer container)
	{
		RenderDataBandsInContainer(containerOfDataBands, container, skipStaticBands: false);
	}

	public static void RenderDataBandsInContainer(StiContainer containerOfDataBands, StiContainer container, bool skipStaticBands)
	{
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		containerOfDataBands.CanGrow = true;
		StiEngine engine = container.Report.Engine;
		double positionFromTop = 0.0;
		double positionFromBottom = 0.0;
		if (container.Report.Engine.Page != null)
		{
			positionFromTop = container.Report.Engine.Page.PageInfoV2.PositionFromTop;
			positionFromBottom = container.Report.Engine.Page.PageInfoV2.PositionFromBottom;
		}
		try
		{
			double num = 100000000000.0;
			bool flag = false;
			StiContainer stiContainer = new StiContainer();
			stiContainer.Width = container.Width;
			stiContainer.Height = num;
			StiEngine stiEngine = new StiEngine(container.Report);
			stiEngine.DenyChangeThread = true;
			stiEngine.ParserConversionStore = (Hashtable)container.Report.Engine.ParserConversionStore.Clone();
			if (container.Report.Engine.HashDataSourceReferencesCounter != null)
			{
				stiEngine.HashDataSourceReferencesCounter = (Hashtable)container.Report.Engine.HashDataSourceReferencesCounter.Clone();
			}
			if (container.Report.Engine.HashParentStyles != null && container.Report.Engine.HashParentStyles.Count > 0)
			{
				stiEngine.HashParentStyles = (Hashtable)container.Report.Engine.HashParentStyles.Clone();
			}
			stiEngine.Page = container.Report.Engine.Page;
			stiEngine.TemplatePage = container.Report.Engine.TemplatePage;
			stiEngine.TemplateContainer = container;
			stiEngine.ContainerForRender = stiContainer;
			container.Report.Engine = stiEngine;
			stiEngine.SetNewPageParameters();
			stiEngine.FreeSpace = num;
			stiEngine.PositionBottomY = num;
			stiEngine.NewList(skipStaticBands);
			stiContainer.Components.Clear();
			foreach (StiComponent component in containerOfDataBands.Components)
			{
				if (component.DockStyle != StiDockStyle.None)
				{
					stiContainer.Components.Add(component);
				}
			}
			RectangleD dockRegion = stiContainer.GetDockRegion(stiContainer, useColumns: false);
			foreach (StiComponent component2 in stiContainer.Components)
			{
				component2.Parent = containerOfDataBands;
			}
			stiContainer.Components.Clear();
			stiEngine.CrossFreeSpace = engine.TemplatePage.Width - (container.Left + ((RectangleD)(ref dockRegion)).Left);
			stiEngine.FreeSpace = ((RectangleD)(ref dockRegion)).Height;
			stiEngine.PositionX = ((RectangleD)(ref dockRegion)).Left;
			stiEngine.PositionY = ((RectangleD)(ref dockRegion)).Top;
			foreach (StiComponent component3 in container.Components)
			{
				component3.ParentBookmark = container.CurrentBookmark;
				component3.ParentPointer = container.CurrentPointer;
				if (component3.ComponentType == StiComponentType.Master)
				{
					if (component3 is StiCrossDataBand)
					{
						flag = true;
					}
					if (component3 is IStiRenderMaster stiRenderMaster)
					{
						stiRenderMaster.RenderMaster();
					}
				}
			}
			stiEngine.FinalClear();
			stiEngine.ParserConversionStore = null;
			stiEngine.HashParentStyles = null;
			if (!flag)
			{
				double num2 = stiEngine.PositionBottomY - stiEngine.PositionY;
				if (containerOfDataBands.Bottom - stiEngine.PositionY > num - stiEngine.PositionBottomY && !container.CanShrink)
				{
					num2 = num - containerOfDataBands.Height;
				}
				foreach (StiComponent component4 in stiContainer.Components)
				{
					if (component4.Top >= stiEngine.PositionY && component4.Top - num2 >= 0.0)
					{
						component4.Top -= num2;
					}
				}
			}
			else
			{
				double num3 = 0.0;
				foreach (StiComponent component5 in stiContainer.Components)
				{
					num3 = Math.Max(component5.Right, num3);
				}
				int num4 = 1;
				while (true)
				{
					decimal num5 = (decimal)engine.TemplatePage.Width * (decimal)num4;
					if ((decimal)(num3 + containerOfDataBands.Left) <= num5)
					{
						break;
					}
					num4++;
				}
				containerOfDataBands.ContainerInfoV2.SetSegmentPerWidth = num4;
			}
			containerOfDataBands.Components.AddRange(stiContainer.Components);
			if (flag)
			{
				return;
			}
			foreach (StiComponent component6 in containerOfDataBands.Components)
			{
				if (component6.Right > containerOfDataBands.Width)
				{
					component6.Width = containerOfDataBands.Width - component6.Left;
				}
				if (!(component6 is StiContainer stiContainer2))
				{
					continue;
				}
				foreach (StiComponent component7 in stiContainer2.Components)
				{
					if (component7.Left > containerOfDataBands.Width)
					{
						component7.Left = containerOfDataBands.Width;
					}
					if (component7.Right + component6.Left > containerOfDataBands.Width)
					{
						component7.Width = containerOfDataBands.Width - (component7.Left + component6.Left);
					}
				}
			}
		}
		finally
		{
			container.Report.Engine = engine;
			if (container.Report.Engine.Page != null)
			{
				container.Report.Engine.Page.PageInfoV2.PositionFromTop = positionFromTop;
				container.Report.Engine.Page.PageInfoV2.PositionFromBottom = positionFromBottom;
			}
		}
	}
}
