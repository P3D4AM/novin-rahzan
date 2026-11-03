using System;
using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiRender
{
	private bool requreResetPage;

	private bool checkPageAfterRender;

	public long RenderedComponents { get; set; }

	public long RenderStop { get; set; }

	public StiReport Report { get; set; }

	internal bool AllowCheckPrevPage { get; set; }

	internal StiPage PrevPage
	{
		get
		{
			if (Report == null || Report.RenderedPages == null || Report.RenderedPages.Count <= 0)
			{
				return null;
			}
			return Report.RenderedPages[Report.RenderedPages.Count - 1];
		}
	}

	internal StiComponentsCollection PageHeaders { get; } = new StiComponentsCollection();


	internal StiComponentsCollection PageFooters { get; } = new StiComponentsCollection();


	public StiPage GetNextPage()
	{
		StiPage stiPage = null;
		if (Report.CurrentPage >= Report.Pages.Count)
		{
			return null;
		}
		StiPage currentPage = Report.GetCurrentPage();
		currentPage.ParentBookmark = currentPage.Report.Bookmark;
		currentPage.ParentPointer = currentPage.Report.Pointer;
		currentPage.InvokeBeforePrint(currentPage, EventArgs.Empty);
		if (!currentPage.Enabled)
		{
			currentPage.InvokeAfterPrint(currentPage, EventArgs.Empty);
			currentPage.ProcessPageAfterRender();
			NextPage();
			return null;
		}
		if (currentPage.RenderedCount == 0)
		{
			currentPage.ProcessPageBeforeRender();
			currentPage.Prepare();
		}
		currentPage.InvokeRendering();
		stiPage = GetPageFromTemplate(currentPage);
		bool flag = stiPage.Components.Count == 0;
		stiPage.PageInfoV1.IsPrevPage = currentPage.PrintOnPreviousPage && !currentPage.PrintHeadersFootersFromPreviousPage;
		if (Report.EngineV1.RequreResetPageNumber || requreResetPage)
		{
			Report.EngineV1.ProcessResetPageNumber(Report);
			requreResetPage = false;
			Report.EngineV1.RequreResetPageNumber = false;
		}
		Report.EngineV1.PageInProgress = stiPage;
		bool flag2 = false;
		try
		{
			if (currentPage.PrintHeadersFootersFromPreviousPage)
			{
				if (!currentPage.PrintOnPreviousPage || (currentPage.PrintOnPreviousPage && flag))
				{
					AddPageHeadersAndFooters(currentPage);
				}
			}
			else
			{
				ClearPageHeadersAndFooters();
			}
			int num = 0;
			if (currentPage.PrintOnPreviousPage)
			{
				num = stiPage.GetComponentsCount();
			}
			flag2 = currentPage.Render(stiPage);
			if (currentPage.PrintOnPreviousPage)
			{
				int num2 = stiPage.GetComponentsCount();
				if (currentPage.Columns > 1)
				{
					num2 -= currentPage.Columns;
				}
				if (num2 != num && checkPageAfterRender)
				{
					CopyValues(stiPage, currentPage);
				}
			}
			checkPageAfterRender = false;
		}
		finally
		{
			if (currentPage.PrintHeadersFootersFromPreviousPage && (!currentPage.PrintOnPreviousPage || (currentPage.PrintOnPreviousPage && flag)))
			{
				RemovePageHeadersAndFooters(currentPage);
			}
		}
		currentPage.InvokeAfterPrint(this, EventArgs.Empty);
		bool flag3 = false;
		if (AllowCheckPrevPage && PrevPage != null && currentPage.PrintOnPreviousPage)
		{
			flag3 = true;
		}
		else
		{
			stiPage.PageInfoV1.PageNumber = Report.PageNumber;
		}
		AllowCheckPrevPage = false;
		if (currentPage.StopBeforePrint != 0 && currentPage.RenderedCount >= currentPage.StopBeforePrint)
		{
			flag2 = true;
		}
		if (flag2)
		{
			currentPage.InvokeEndRender();
			NextPage();
		}
		if (stiPage != null)
		{
			Report.CurrentPrintPage++;
		}
		if (flag3)
		{
			stiPage = null;
		}
		return stiPage;
	}

	private void PreparePrevPageForRenderColumns(StiPage page)
	{
		if (page.Columns <= 1)
		{
			return;
		}
		List<StiContainer> list = new List<StiContainer>();
		foreach (StiComponent component in page.Components)
		{
			if (component is StiContainer stiContainer && stiContainer.ContainerInfoV1.IsColumn)
			{
				list.Add(stiContainer);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		for (int i = 0; i < page.Columns - 1; i++)
		{
			StiContainer stiContainer2 = list[i];
			StiContainer stiContainer3 = list[i + 1];
			if (stiContainer2.Components.Count == 0 || stiContainer3.Components.Count == 0)
			{
				break;
			}
			double num = 0.0;
			foreach (StiComponent component2 in stiContainer2.Components)
			{
				num = Math.Max(component2.Bottom, num);
			}
			StiContainer stiContainer4 = new StiContainer();
			stiContainer4.Name = stiContainer2.Name + "_EmptyCont";
			stiContainer4.Height = stiContainer2.Height - num;
			stiContainer4.DockStyle = StiDockStyle.Bottom;
			stiContainer2.Components.Add(stiContainer4);
		}
	}

	private void MoveColumnContainersAndClonesToSpecialContainer(StiPage page)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = null;
		foreach (StiComponent component in page.Components)
		{
			if (component is StiContainer stiContainer2 && stiContainer2.ContainerInfoV1.IsColumn)
			{
				if (stiContainer == null)
				{
					stiContainer = new StiContainer();
				}
				stiContainer2.CanGrow = true;
				stiContainer2.CanShrink = true;
				SizeD actualSize = stiContainer2.GetActualSize();
				stiContainer2.Width = ((SizeD)(ref actualSize)).Width;
				stiContainer2.Height = ((SizeD)(ref actualSize)).Height;
				stiContainer.Components.Add(stiContainer2);
			}
		}
		if (stiContainer == null)
		{
			return;
		}
		int index = page.Components.IndexOf(stiContainer.Components[0]);
		page.Components.Insert(index, stiContainer);
		stiContainer.DockStyle = StiDockStyle.Top;
		foreach (StiContainer component2 in stiContainer.Components)
		{
			page.Components.Remove(component2);
			component2.Parent = stiContainer;
			component2.Page = page;
			component2.Top = 0.0;
		}
		stiContainer.CanGrow = true;
		stiContainer.CanShrink = true;
		SizeD actualSize2 = stiContainer.GetActualSize();
		stiContainer.Name = "test";
		stiContainer.Height = ((SizeD)(ref actualSize2)).Height;
	}

	private void ClearPageHeadersAndFooters()
	{
		PageHeaders.Clear();
		PageFooters.Clear();
	}

	private StiComponentsCollection GetPageHeaders(StiPage page)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent component in page.Components)
		{
			if (component is StiPageHeaderBand)
			{
				stiComponentsCollection.Add(component);
			}
		}
		return stiComponentsCollection;
	}

	private StiComponentsCollection GetPageFooters(StiPage page)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent component in page.Components)
		{
			if (component is StiPageFooterBand)
			{
				stiComponentsCollection.Add(component);
			}
		}
		return stiComponentsCollection;
	}

	private void AddPageHeadersAndFooters(StiPage page)
	{
		page.Components.InsertRange(0, PageHeaders);
		page.Components.InsertRange(0, PageFooters);
	}

	private void RemovePageHeadersAndFooters(StiPage page)
	{
		foreach (StiComponent pageHeader in PageHeaders)
		{
			page.Components.Remove(pageHeader);
		}
		foreach (StiComponent pageFooter in PageFooters)
		{
			page.Components.Remove(pageFooter);
		}
	}

	public StiPage GetPageFromTemplate(StiPage tmpPage)
	{
		if (AllowCheckPrevPage && PrevPage != null && tmpPage.PrintOnPreviousPage)
		{
			if (!tmpPage.PrintHeadersFootersFromPreviousPage)
			{
				MoveColumnContainersAndClonesToSpecialContainer(PrevPage);
				StiContainer stiContainer = new StiContainer();
				stiContainer.TagValue = PrevPage.TagValue;
				stiContainer.PointerValue = PrevPage.PointerValue;
				stiContainer.BookmarkValue = PrevPage.BookmarkValue;
				stiContainer.HyperlinkValue = PrevPage.HyperlinkValue;
				stiContainer.ToolTipValue = PrevPage.ToolTipValue;
				stiContainer.Name = "PrevPageCont";
				stiContainer.DockStyle = StiDockStyle.Top;
				stiContainer.Components.AddRange(PrevPage.Components);
				double num = 0.0;
				foreach (StiComponent component in stiContainer.Components)
				{
					if (component.DockStyle == StiDockStyle.Top || component.DockStyle == StiDockStyle.Bottom)
					{
						num += component.Height;
					}
				}
				stiContainer.Height = num;
				foreach (StiComponent component2 in stiContainer.Components)
				{
					component2.Parent = stiContainer;
				}
				PrevPage.Components.Clear();
				PrevPage.Components.Add(stiContainer);
			}
			checkPageAfterRender = true;
			if (tmpPage.PrintHeadersFootersFromPreviousPage)
			{
				PreparePrevPageForRenderColumns(PrevPage);
			}
			return PrevPage;
		}
		StiPage stiPage = tmpPage.Clone(cloneProperties: false, cloneComponents: false) as StiPage;
		stiPage.Guid = StiGuidUtils.NewGuid();
		return stiPage;
	}

	private void CopyValues(StiPage prevPage, StiPage tmpPage)
	{
		tmpPage.InvokeEvents();
		if (tmpPage.TagValue is string && ((string)tmpPage.TagValue).Length > 0)
		{
			PrevPage.TagValue = tmpPage.TagValue;
		}
		if (tmpPage.BookmarkValue is string && ((string)tmpPage.BookmarkValue).Length > 0)
		{
			PrevPage.BookmarkValue = tmpPage.BookmarkValue;
		}
		if (tmpPage.PointerValue is string && ((string)tmpPage.PointerValue).Length > 0)
		{
			PrevPage.PointerValue = tmpPage.PointerValue;
		}
		if (tmpPage.HyperlinkValue is string && ((string)tmpPage.HyperlinkValue).Length > 0)
		{
			PrevPage.HyperlinkValue = tmpPage.HyperlinkValue;
		}
		if (tmpPage.ToolTipValue is string && ((string)tmpPage.ToolTipValue).Length > 0)
		{
			PrevPage.ToolTipValue = tmpPage.ToolTipValue;
		}
		if (tmpPage.ExcelSheetValue != null && tmpPage.ExcelSheetValue.Length > 0)
		{
			PrevPage.ExcelSheetValue = tmpPage.ExcelSheetValue;
		}
	}

	public void NextPage()
	{
		if (Report.CurrentPage >= 0 && Report.CurrentPage < Report.Pages.Count)
		{
			StiPage page = Report.Pages[Report.CurrentPage];
			PageHeaders.InsertRange(0, GetPageHeaders(page));
			PageFooters.InsertRange(0, GetPageFooters(page));
		}
		if (Report.CurrentPage >= 0)
		{
			Report.Pages[Report.CurrentPage].ProcessPageAfterRender();
		}
		bool flag = false;
		if (Report.CurrentPage >= 0 && Report.CurrentPage < Report.Pages.Count)
		{
			StiPage stiPage = Report.Pages[Report.CurrentPage];
			stiPage.PageInfoV1.NumberOfCopiesToPrint--;
			flag = stiPage.PageInfoV1.NumberOfCopiesToPrint > 0;
			if (flag)
			{
				stiPage.ProcessPageBeforeRender();
				stiPage.Prepare();
				Report.PageCopyNumber = stiPage.NumberOfCopies - stiPage.PageInfoV1.NumberOfCopiesToPrint + 1;
			}
			else
			{
				Report.PageCopyNumber = 1;
			}
		}
		if (!flag)
		{
			do
			{
				AllowCheckPrevPage = true;
				Report.CurrentPage++;
			}
			while (Report.CurrentPage < Report.Pages.Count && (Report.Pages[Report.CurrentPage].Skip || !Report.Pages[Report.CurrentPage].Enabled));
		}
		if (Report.CurrentPage < Report.Pages.Count)
		{
			Report.Pages[Report.CurrentPage].InvokeBeginRender();
			if (Report.Pages[Report.CurrentPage].ResetPageNumber)
			{
				requreResetPage = true;
			}
		}
	}

	public StiRender(StiReport report)
	{
		foreach (StiPage page in report.Pages)
		{
			page.RenderedCount = 0;
		}
		foreach (StiPage page2 in report.Pages)
		{
			page2.PageInfoV1.NumberOfCopiesToPrint = Math.Max(page2.NumberOfCopies, 1);
		}
		Report = report;
		Report.CurrentPrintPage = 0;
		Report.CurrentPage = -1;
		NextPage();
	}
}
