using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiStaticBandsHelper
{
	private bool denyReportBands;

	private bool denyPageBands;

	internal StiEngine Engine { get; }

	internal double ReservedFreeSpace { get; private set; }

	internal double ReservedCrossFreeSpace { get; private set; }

	internal double ReservedPositionX { get; private set; }

	internal double ReservedPositionY { get; private set; }

	internal double ReservedPositionBottomY { get; private set; }

	public void Render()
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		if (!Engine.Threads.IsActive && !Engine.DenyChangeThread)
		{
			if (Engine.TemplatePage.TitleBeforeHeader)
			{
				RenderTitleBeforeHeader();
			}
			else
			{
				RenderHeaderBeforeTitle();
			}
			Engine.DenyRenderMasterComponentsInContainer = true;
			StiPageHelper.RenderSimpleComponents(Engine.TemplatePage, Engine.ContainerForRender);
			Engine.DenyRenderMasterComponentsInContainer = false;
		}
		if (Engine.Page != null)
		{
			Engine.Page.PageInfoV2.PositionFromTop = Engine.PositionY;
			Engine.Page.PageInfoV2.PositionFromBottom = Engine.PositionBottomY;
		}
		RectangleD dockRegion = Engine.ContainerForRender.GetDockRegion(Engine.ContainerForRender, useColumns: false);
		Engine.CrossFreeSpace = ((RectangleD)(ref dockRegion)).Width;
		Engine.FreeSpace = ((RectangleD)(ref dockRegion)).Height;
		if (Engine.ColumnsOnPanel.Count > 1 && Engine.ColumnsOnPanel.RightToLeft && !Engine.IsCrossBandsMode)
		{
			Engine.PositionX = ((RectangleD)(ref dockRegion)).Right - Engine.ColumnsOnPanel.GetColumnWidth();
		}
		else
		{
			Engine.PositionX = ((RectangleD)(ref dockRegion)).Left;
		}
		Engine.PositionY = ((RectangleD)(ref dockRegion)).Y;
		Engine.PositionBottomY = ((RectangleD)(ref dockRegion)).Bottom;
		ReservedCrossFreeSpace = Engine.CrossFreeSpace;
		ReservedFreeSpace = Engine.FreeSpace;
		ReservedPositionX = Engine.PositionX;
		ReservedPositionY = Engine.PositionY;
		ReservedPositionBottomY = Engine.PositionBottomY;
		if (!StiOptions.Engine.ForceNewPageForExtraColumns)
		{
			Engine.PositionY = Engine.OffsetNewColumnY + ReservedPositionY;
		}
	}

	private void RenderTitleBeforeHeader()
	{
		if (!denyReportBands)
		{
			bool flag = denyReportBands;
			bool flag2 = denyPageBands;
			denyPageBands = true;
			denyReportBands = true;
			RenderReportTitleBands();
			denyPageBands = flag2;
			denyReportBands = flag;
		}
		if (!denyPageBands)
		{
			bool flag3 = denyPageBands;
			bool flag4 = denyReportBands;
			denyPageBands = true;
			denyReportBands = true;
			RenderPageHeaderBands();
			RenderPageFooterBands();
			denyPageBands = flag3;
			denyReportBands = flag4;
		}
	}

	private void RenderHeaderBeforeTitle()
	{
		if (!denyPageBands)
		{
			bool flag = denyPageBands;
			denyPageBands = true;
			RenderPageHeaderBands();
			RenderPageFooterBands();
			denyPageBands = flag;
		}
	}

	private void RenderReportTitleBands()
	{
		if (Engine.TemplatePage.PageInfoV2.IsReportTitlesRendered)
		{
			return;
		}
		foreach (StiComponent component in Engine.TemplatePage.Components)
		{
			if (component is StiReportTitleBand stiReportTitleBand)
			{
				stiReportTitleBand.ParentBookmark = Engine.ContainerForRender.CurrentBookmark;
				bool flag = stiReportTitleBand.DoBookmark();
				stiReportTitleBand.ParentPointer = Engine.ContainerForRender.CurrentPointer;
				stiReportTitleBand.DoPointer(!flag);
				Engine.RenderBand(stiReportTitleBand);
			}
		}
		Engine.TemplatePage.PageInfoV2.IsReportTitlesRendered = true;
	}

	private void RenderPageHeaderBands()
	{
		StiComponentsCollection pageHeaders = GetPageHeaders();
		foreach (StiComponent item in pageHeaders)
		{
			if (item is StiPageHeaderBand stiPageHeaderBand)
			{
				stiPageHeaderBand.ParentBookmark = Engine.ContainerForRender.CurrentBookmark;
				bool flag = stiPageHeaderBand.DoBookmark();
				stiPageHeaderBand.ParentPointer = Engine.ContainerForRender.CurrentPointer;
				stiPageHeaderBand.DoPointer(!flag);
				Engine.RenderBand(stiPageHeaderBand);
			}
		}
	}

	private void RenderPageFooterBands()
	{
		StiComponentsCollection pageFooters = GetPageFooters();
		foreach (StiComponent item in pageFooters)
		{
			if (item is StiPageFooterBand stiPageFooterBand)
			{
				stiPageFooterBand.ParentBookmark = Engine.ContainerForRender.CurrentBookmark;
				bool flag = stiPageFooterBand.DoBookmark();
				stiPageFooterBand.ParentPointer = Engine.ContainerForRender.CurrentPointer;
				stiPageFooterBand.DoPointer(!flag);
				Engine.RenderBand(stiPageFooterBand);
			}
		}
	}

	private StiComponentsCollection GetPageHeaders()
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		int num = Engine.Report.Pages.IndexOf(Engine.TemplatePage);
		int num2;
		for (num2 = num; num2 >= 0; num2--)
		{
			StiPage stiPage = Engine.Report.Pages[num2];
			if (!stiPage.PrintHeadersFootersFromPreviousPage)
			{
				break;
			}
		}
		if (num2 <= -1)
		{
			num2 = 0;
		}
		for (int i = num2; i <= num; i++)
		{
			StiPage page = Engine.Report.Pages[i];
			stiComponentsCollection.AddRange(GetPageHeadersFromPage(page));
		}
		return stiComponentsCollection;
	}

	private StiComponentsCollection GetPageFooters()
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		int num = Engine.Report.Pages.IndexOf(Engine.TemplatePage);
		int num2;
		for (num2 = num; num2 >= 0; num2--)
		{
			StiPage stiPage = Engine.Report.Pages[num2];
			if (!stiPage.PrintHeadersFootersFromPreviousPage)
			{
				break;
			}
		}
		if (num2 <= -1)
		{
			num2 = 0;
		}
		for (int i = num2; i <= num; i++)
		{
			StiPage page = Engine.Report.Pages[i];
			stiComponentsCollection.AddRange(GetPageFootersFromPage(page));
		}
		return stiComponentsCollection;
	}

	private StiComponentsCollection GetPageHeadersFromPage(StiPage page)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent component2 in page.Components)
		{
			if (component2 is StiPageHeaderBand component)
			{
				stiComponentsCollection.Add(component);
			}
		}
		return stiComponentsCollection;
	}

	private StiComponentsCollection GetPageFootersFromPage(StiPage page)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent component2 in page.Components)
		{
			if (component2 is StiPageFooterBand component)
			{
				stiComponentsCollection.Add(component);
			}
		}
		return stiComponentsCollection;
	}

	internal StiStaticBandsHelper(StiEngine engine)
	{
		Engine = engine;
	}
}
