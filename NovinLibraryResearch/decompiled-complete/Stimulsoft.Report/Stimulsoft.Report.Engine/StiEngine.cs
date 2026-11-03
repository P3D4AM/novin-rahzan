using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Engine;

public class StiEngine
{
	public const double SpecialContainerHeight = 100000000000.0;

	public const double SpecialContainerHeight2 = 99999999900.0;

	private Hashtable parserConversionStore;

	private Hashtable hashUseParentStyles;

	private Hashtable printOnAllPagesIgnoreList2 = new Hashtable();

	private Hashtable childsBandHash;

	private bool needResetPageNumberForNewPage;

	private bool flagRenderColumnsOnDataBandOnNewPage;

	private Hashtable componentPlacementRemakeTable;

	internal StiBookmark FirstPassPointer { get; set; }

	public bool IsDynamicBookmarksMode { get; set; }

	public bool IsCrossBandsMode { get; set; }

	public bool IsFirstDataBandOnPage { get; set; }

	public bool IsLastDataBandOnPage { get; set; }

	internal Hashtable PrintOnAllPagesIgnoreList { get; set; } = new Hashtable();


	internal StiProgressHelperV2 ProgressHelper { get; }

	internal StiBandsOnAllPages BandsOnAllPages { get; }

	internal bool DenyRenderMasterComponentsInContainer { get; set; }

	internal StiPrintAtBottom PrintAtBottom { get; }

	internal StiFootersOnAllPages FootersOnAllPages { get; }

	internal StiStaticBandsHelper StaticBands { get; }

	internal StiThreads Threads { get; }

	internal StiBreakableHelper Breakable { get; }

	internal bool DenyChangeThread { get; set; }

	internal Hashtable SlaveEngines { get; } = new Hashtable();


	internal StiEngine MasterEngine { get; set; }

	internal StiEmptyBandsV2Helper EmptyBands { get; }

	internal StiPageNumberHelper PageNumbers { get; }

	internal StiColumnsOnDataBand ColumnsOnDataBand { get; }

	internal StiColumnsOnPanel ColumnsOnPanel { get; }

	public double FreeSpace { get; set; }

	public double CrossFreeSpace { get; set; }

	public double PositionX { get; set; }

	public double PositionY { get; set; }

	public double PositionBottomY { get; set; }

	public StiContainer ContainerForRender { get; set; }

	public StiPage Page { get; set; }

	public StiPage TemplatePage { get; set; }

	public StiContainer TemplateContainer { get; set; }

	public StiReport Report { get; set; }

	public StiReport MasterReport { get; set; }

	public bool IgnoreUnlimitedHeightForNewPage { get; set; }

	internal Hashtable KeepFirstDetailTogetherList { get; } = new Hashtable();


	internal Hashtable KeepFirstDetailTogetherTablesList { get; } = new Hashtable();


	internal int StartIndexPageForPageTotal { get; set; } = -1;


	internal int IndexPageForPageTotal { get; set; } = -1;


	internal bool SilentMode { get; set; }

	internal StiRenderState RenderState { get; set; }

	internal StiIndex IndexOfLatestDataBand { get; set; }

	internal bool GenerateNewPageBeforeBand { get; set; }

	internal bool IgnoreSkipFirst { get; set; }

	internal bool GenerateNewColumnBeforeBand { get; set; }

	internal Hashtable PageBreakSkipFirstCollection { get; } = new Hashtable();


	internal int IndexOfStartList { get; set; }

	internal bool SkipFirstPageBeforePrintEvent { get; set; }

	internal bool FirstCallNewPage { get; set; }

	internal bool DenyClearPrintOnAllPagesIgnoreList { get; set; }

	internal Hashtable DuplicatesLastValues { get; set; }

	internal Hashtable AnchorsArguments { get; set; }

	internal Hashtable ParserConversionStore
	{
		get
		{
			return parserConversionStore ?? (parserConversionStore = new Hashtable());
		}
		set
		{
			parserConversionStore = value;
		}
	}

	internal Hashtable HashParentStyles { get; set; }

	internal Hashtable HashUseParentStyles
	{
		get
		{
			if (hashUseParentStyles == null)
			{
				hashUseParentStyles = new Hashtable();
				foreach (StiComponent component in Report.GetComponents())
				{
					if (!(component is StiContainer stiContainer))
					{
						continue;
					}
					foreach (StiComponent component2 in stiContainer.Components)
					{
						if (component2.UseParentStyles)
						{
							hashUseParentStyles[component] = null;
							break;
						}
					}
				}
			}
			return hashUseParentStyles;
		}
		set
		{
			hashUseParentStyles = value;
		}
	}

	internal object LastInvokeTextProcessValueEventArgsValue { get; set; }

	internal bool AtLeastOneDatabandRenderedOnPage { get; set; }

	internal double LastFreeSpaceOnPageAfterNewList { get; set; }

	internal List<StiBand> BandsInProgress { get; } = new List<StiBand>();


	internal bool AllowEndOfPageProcessing { get; set; }

	internal Hashtable HashCheckSize { get; set; }

	internal Hashtable HashDataSourceReferencesCounter { get; set; }

	internal double OffsetNewColumnY { get; set; }

	internal decimal LatestProgressValue { get; set; }

	public void NewList()
	{
		NewList(skipStaticBands: false);
	}

	public void NewList(bool skipStaticBands)
	{
		if (ContainerForRender != null)
		{
			if (!skipStaticBands && (ColumnsOnPanel.CurrentColumn == 1 || ColumnsOnPanel.Count < 2))
			{
				StaticBands.Render();
			}
			IndexOfLatestDataBand = null;
			BandsOnAllPages.Render();
			if (HashCheckSize != null && HashCheckSize.Count > StiOptions.Engine.ReportCache.AmountOfProcessedPagesForStartGCCollect * 1000)
			{
				HashCheckSize.Clear();
			}
		}
	}

	public void NewColumn()
	{
		NewColumn(ignoreKeepContainers: true);
	}

	internal void NewColumn(bool ignoreKeepContainers)
	{
		ColumnsOnPanel.CurrentColumn++;
		StiPage stiPage = Page ?? Report.RenderedPages[0];
		if ((ColumnsOnPanel.CurrentColumn > ColumnsOnPanel.Count && StiOptions.Engine.ForceNewPageForExtraColumns) || IsCrossBandsMode)
		{
			if (!IsCrossBandsMode)
			{
				stiPage.InvokeColumnEndRender();
				ColumnsOnPanel.CurrentColumn = 1;
				PositionX = 0.0;
			}
			NewPage(ignoreKeepContainers);
			return;
		}
		StiContainer containerForRender = ContainerForRender;
		stiPage.InvokeColumnEndRender();
		FinishColumns(containerForRender);
		StiContainer markerContainer = SearchStartOfKeepContainer(containerForRender, IndexOfStartList);
		if (ignoreKeepContainers)
		{
			markerContainer = null;
		}
		if (containerForRender != null && markerContainer != null && containerForRender.Components.IndexOf(markerContainer) == 0)
		{
			markerContainer = null;
		}
		ChangeEngineParamsByKeep(containerForRender, markerContainer);
		RenderFootersOnAllPages(ContainerForRender, IndexOfStartList, ref markerContainer);
		RenderPrintAtBottom(ContainerForRender, IndexOfStartList, markerContainer);
		RenderEmptyBands(containerForRender, markerContainer);
		SetNewColumnParameters();
		if (ColumnsOnPanel.CurrentColumn > ColumnsOnPanel.Count && !StiOptions.Engine.ForceNewPageForExtraColumns && ContainerForRender == containerForRender)
		{
			ColumnsOnPanel.CurrentColumn = 1;
			PositionX = (ColumnsOnPanel.RightToLeft ? (TemplateContainer.Width - ColumnsOnPanel.GetColumnWidth()) : 0.0);
			double num = 0.0;
			foreach (StiComponent component in containerForRender.Components)
			{
				if (component.Bottom > num)
				{
					num = component.Bottom;
				}
			}
			OffsetNewColumnY = num - StaticBands.ReservedPositionY;
			PositionY = num;
		}
		NewList();
		MoveKeepComponentsOnNextContainer(containerForRender, markerContainer);
		IndexOfStartList = ((ContainerForRender.Components.Count > 0) ? (ContainerForRender.Components.Count - 1) : 0);
		FinishResetPageNumberContainer(containerForRender, isFinal: true);
		FinishContainer(containerForRender);
		stiPage.InvokeColumnBeginRender();
	}

	public void NewPage()
	{
		NewPage(ignoreKeepContainers: true);
	}

	internal void NewPage(bool ignoreKeepContainers)
	{
		if (!IsCrossBandsMode && ContainerForRender != null && ContainerForRender.Height > 99999999900.0 && !StiOptions.Engine.ForceNewPageInSubReports)
		{
			StiNewPageContainer stiNewPageContainer = new StiNewPageContainer
			{
				Top = PositionY,
				Height = ((TemplatePage.Height < 99999999900.0) ? TemplatePage.Height : TemplatePage.Unit.ConvertFromHInches(1170.0))
			};
			PositionY += stiNewPageContainer.Height;
			FreeSpace -= stiNewPageContainer.Height;
			ContainerForRender.Components.Add(stiNewPageContainer);
			OffsetNewColumnY = PositionY - StaticBands.ReservedPositionY;
			return;
		}
		if (Threads.IsActive)
		{
			NewContainer(ignoreKeepContainers);
			return;
		}
		if (IsCrossBandsMode && Page != null)
		{
			decimal num = (decimal)(Page.PageWidth - Page.Margins.Left - Page.Margins.Right);
			while ((decimal)PositionX + num > (decimal)Page.Width)
			{
				Page.SegmentPerWidth++;
			}
			if (TemplatePage.UnlimitedBreakable)
			{
				PositionX += CrossFreeSpace;
				CrossFreeSpace = TemplatePage.Width;
			}
			else
			{
				CrossFreeSpace += TemplatePage.Width;
			}
			return;
		}
		if (Page != null && TemplatePage.UnlimitedHeight && !FirstCallNewPage && !IgnoreUnlimitedHeightForNewPage && ColumnsOnPanel.Count < 2)
		{
			while ((int)(decimal)(PositionY / TemplatePage.Height + 1.0) > Page.SegmentPerHeight)
			{
				Page.SegmentPerHeight++;
				if (!TemplatePage.UnlimitedBreakable)
				{
					FreeSpace += TemplatePage.Height;
				}
			}
			if (TemplatePage.UnlimitedBreakable)
			{
				PositionY += ((FreeSpace > 0.0) ? (TemplatePage.Height - PositionY % TemplatePage.Height) : FreeSpace);
				FreeSpace = PositionBottomY;
				while ((int)(decimal)(PositionY / TemplatePage.Height + 1.0) > Page.SegmentPerHeight)
				{
					Page.SegmentPerHeight++;
				}
			}
			return;
		}
		LastFreeSpaceOnPageAfterNewList = 0.0;
		FirstCallNewPage = false;
		if (!AtLeastOneDatabandRenderedOnPage && PrintOnAllPagesIgnoreList.Count > 0)
		{
			PrintOnAllPagesIgnoreList.Clear();
		}
		ColumnsOnPanel.CurrentColumn = 1;
		StiContainer containerForRender = ContainerForRender;
		if (!StiOptions.Engine.FixPageNumberInEvents)
		{
			TemplatePage.InvokeRendering();
		}
		if (Page != null)
		{
			ProcessPageAfterRendering(Page, final: false);
		}
		Page = StiPageHelper.GetPageFromTemplate(TemplatePage);
		PageNumbers.AddPageNumber(Report.CurrentPrintPage, Page.SegmentPerWidth, Page.SegmentPerHeight);
		if (Page.ResetPageNumber)
		{
			if (Page.PageInfoV2 != null && Page.PageInfoV2.IndexOfStartRenderedPages != -1)
			{
				Page.Report.Engine.PageNumbers.ResetPageNumber(Page.PageInfoV2.IndexOfStartRenderedPages);
			}
			else
			{
				Page.Report.Engine.PageNumbers.ResetPageNumber();
			}
		}
		if (needResetPageNumberForNewPage)
		{
			Page.Report.Engine.PageNumbers.ResetPageNumber(Report.CurrentPrintPage);
		}
		if (!StiOptions.Engine.FixPageNumberInEvents)
		{
			ProcessRendering();
		}
		AddPageToRenderedPages(Page);
		FinishColumns(containerForRender);
		StiPageHelper.PrepareBookmark(Page);
		StiPageHelper.PreparePointer(Page);
		Page.InvokeColumnBeginRender();
		StiContainer markerContainer = SearchStartOfKeepContainer(containerForRender, IndexOfStartList);
		if (ignoreKeepContainers)
		{
			markerContainer = null;
		}
		if (containerForRender != null && markerContainer != null && containerForRender.Components.IndexOf(markerContainer) == 0)
		{
			markerContainer = null;
		}
		ChangeEngineParamsByKeep(containerForRender, markerContainer);
		RenderFootersOnAllPages(containerForRender, IndexOfStartList, ref markerContainer);
		RenderPrintAtBottom(containerForRender, IndexOfStartList, markerContainer);
		RenderEmptyBands(containerForRender, markerContainer);
		OffsetNewColumnY = 0.0;
		SetNewPageParameters();
		ContainerForRender = Page;
		CorrectPrintOnAllPagesIgnoreListBeforeNewList(containerForRender, markerContainer, clear: false);
		NewList();
		CorrectPrintOnAllPagesIgnoreListBeforeNewList(containerForRender, markerContainer, clear: true);
		AtLeastOneDatabandRenderedOnPage = false;
		LastFreeSpaceOnPageAfterNewList = FreeSpace;
		MoveKeepComponentsOnNextContainer(containerForRender, markerContainer);
		IndexOfStartList = 0;
		FinishResetPageNumberContainer(containerForRender, isFinal: false);
		FinishContainer(containerForRender);
	}

	private void NewContainer(bool ignoreKeepContainers)
	{
		if (!AtLeastOneDatabandRenderedOnPage && PrintOnAllPagesIgnoreList.Count > 0)
		{
			PrintOnAllPagesIgnoreList.Clear();
		}
		ColumnsOnPanel.CurrentColumn = 1;
		StiContainer containerForRender = ContainerForRender;
		FinishColumns(containerForRender);
		StiContainer markerContainer = SearchStartOfKeepContainer(containerForRender, IndexOfStartList);
		if (ignoreKeepContainers)
		{
			markerContainer = null;
		}
		if (containerForRender != null && markerContainer != null && containerForRender.Components.IndexOf(markerContainer) == 0)
		{
			markerContainer = null;
		}
		ChangeEngineParamsByKeep(containerForRender, markerContainer);
		RenderFootersOnAllPages(containerForRender, IndexOfStartList, ref markerContainer);
		RenderPrintAtBottom(containerForRender, IndexOfStartList, markerContainer);
		RenderEmptyBands(containerForRender, markerContainer);
		Threads.NewPage();
		NewList();
		AtLeastOneDatabandRenderedOnPage = false;
		MoveKeepComponentsOnNextContainer(containerForRender, markerContainer);
		IndexOfStartList = 0;
		FinishResetPageNumberContainer(containerForRender, isFinal: false);
		FinishContainer(containerForRender);
		if (ContainerForRender != null && ContainerForRender.Parent is StiPage && ContainerForRender.CanGrow && MasterEngine != null && FreeSpace < 50000000000.0)
		{
			FreeSpace = MasterEngine.PositionBottomY - ContainerForRender.Top - PositionY;
			ContainerForRender.Height = FreeSpace;
		}
	}

	public void NewDestination()
	{
		NewDestination(ignoreKeepContainers: false);
	}

	public void NewDestination(bool ignoreKeepContainers)
	{
		if (ColumnsOnPanel.Count > 1)
		{
			NewColumn(ignoreKeepContainers);
		}
		else
		{
			NewPage(ignoreKeepContainers);
		}
	}

	public void AddFooterMarker(StiFooterBand footerMaster)
	{
		StiFooterMarkerContainer stiFooterMarkerContainer = new StiFooterMarkerContainer
		{
			Top = PositionY,
			Left = PositionX,
			Height = 0.0,
			Name = footerMaster.Name
		};
		stiFooterMarkerContainer.ContainerInfoV2.ParentBand = footerMaster;
		AddContainerToDestination(stiFooterMarkerContainer);
	}

	public void AddKeepLevelAtLatestDataBand()
	{
		if (IndexOfLatestDataBand == null)
		{
			return;
		}
		StiContainer stiContainer = ContainerForRender;
		int num = IndexOfLatestDataBand.Index;
		if (IndexOfLatestDataBand.IndexInColumnContainer != -1 && stiContainer.Components.Count > IndexOfLatestDataBand.IndexInColumnContainer)
		{
			stiContainer = stiContainer.Components[IndexOfLatestDataBand.IndexInColumnContainer] as StiContainer;
			StiColumnsContainer stiColumnsContainer = stiContainer as StiColumnsContainer;
			if (stiColumnsContainer.ColumnDirection == StiColumnDirection.AcrossThenDown)
			{
				num -= stiColumnsContainer.GetLengthOfLastRow() - 1;
			}
		}
		if (num < stiContainer.Components.Count && num >= 0)
		{
			stiContainer.Components.Insert(num, new StiLevelStartContainer());
		}
	}

	public void AddLevel()
	{
		if (ColumnsOnDataBand.Enabled)
		{
			StiColumnsContainer columns = ColumnsOnDataBand.GetColumns();
			columns.AddContainer(new StiLevelStartContainer());
		}
		else
		{
			AddContainerToDestination(new StiLevelStartContainer());
		}
	}

	public void RemoveLevel()
	{
		if (ColumnsOnDataBand.Enabled)
		{
			StiColumnsContainer columns = ColumnsOnDataBand.GetColumns();
			columns.AddContainer(new StiLevelEndContainer());
		}
		else
		{
			AddContainerToDestination(new StiLevelEndContainer());
		}
	}

	public void ClearCachedTotals()
	{
		if (Report != null && Report.CachedTotals != null)
		{
			Report.CachedTotals.Clear();
		}
	}

	private StiComponentsCollection GetChildBands(StiBand band)
	{
		if (childsBandHash == null)
		{
			childsBandHash = new Hashtable();
		}
		if (childsBandHash[band] is StiComponentsCollection result)
		{
			return result;
		}
		StiComponentsCollection childBands = StiBandV2Builder.GetChildBands(band);
		childsBandHash[band] = childBands;
		return childBands;
	}

	public void ClearPageBreakSkipFirst()
	{
		PageBreakSkipFirstCollection.Clear();
	}

	internal bool CanGenerateNewContainer(IStiPageBreak pageBreak)
	{
		if (pageBreak == null)
		{
			return true;
		}
		if (!pageBreak.SkipFirst)
		{
			return true;
		}
		if (PageBreakSkipFirstCollection[pageBreak] != null)
		{
			return true;
		}
		PageBreakSkipFirstCollection[pageBreak] = pageBreak;
		return false;
	}

	public void RemoveBandFromPageBreakSkipList(IStiPageBreak pageBreak)
	{
		if (pageBreak != null && PageBreakSkipFirstCollection[pageBreak] != null)
		{
			PageBreakSkipFirstCollection.Remove(pageBreak);
		}
	}

	internal void ProcessPageAfterRendering(StiPage page, bool final)
	{
		if (AllowEndOfPageProcessing)
		{
			StiComponentsCollection components = page.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item is StiText { ProcessAt: StiProcessAt.EndOfPage } stiText)
				{
					StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
					stiText.InvokeGetValue(item, stiGetValueEventArgs);
					stiText.Text.Value = stiGetValueEventArgs.Value;
				}
			}
		}
		StiRenderState renderState = RenderState;
		if (MasterEngine != null)
		{
			renderState = MasterEngine.RenderState;
		}
		StiRenderProviderV2.ProcessPageToCache(Report, page, final);
		if (renderState != null)
		{
			StiRenderProviderV2.ClearPagesWhichLessThenFromPageAndGreaterThenToPage(Report, renderState);
		}
		StiRenderProviderV2.ClearPagesForFirstPass(Report);
	}

	internal void ProcessLastPageAfterRendering()
	{
		StiPage page = Report.RenderedPages[Report.RenderedPages.Count - 1];
		ProcessPageAfterRendering(page, final: false);
	}

	private void ProcessRendering()
	{
		if (Report.IsStopped)
		{
			throw new StiReportRenderingStopException();
		}
		Report.InvokeRendering();
		if (MasterReport != null)
		{
			MasterReport.InvokeRendering();
			if (MasterReport.IsStopped)
			{
				throw new StiReportRenderingStopException();
			}
		}
		ProgressHelper.Process();
	}

	private void ReprocessRuntimeVariables(StiContainer container)
	{
		StiComponentsCollection components = container.GetComponents();
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent item in components)
			{
				if (!(item is StiSimpleText) || !(Report.Totals[item.Name] is ArrayList { Count: not 0 } arrayList))
				{
					continue;
				}
				lock (((ICollection)arrayList).SyncRoot)
				{
					foreach (StiRuntimeVariables item2 in arrayList)
					{
						if (item2.TextBox == item)
						{
							item2.PageIndex = Report.RenderedPages.Count;
							item2.CurrentPrintPage = Report.RenderedPages.Count;
						}
					}
				}
			}
		}
	}

	public void FinalClear()
	{
		ChangeEngineParamsByKeep(ContainerForRender, null);
		StiContainer markerContainer = null;
		RenderFootersOnAllPages(null, 0, ref markerContainer);
		RenderPrintAtBottom(null, 0, null);
		RenderEmptyBands(ContainerForRender, null);
		FinishColumns(ContainerForRender);
		FinishResetPageNumberContainer(ContainerForRender, isFinal: true);
		FinishContainer(null);
		EmptyBands.Clear();
		childsBandHash?.Clear();
		childsBandHash = null;
		DuplicatesLastValues?.Clear();
		DuplicatesLastValues = null;
		IndexOfLatestDataBand = null;
		HashCheckSize?.Clear();
		HashCheckSize = null;
		HashParentStyles?.Clear();
		HashParentStyles = null;
		ClearPageBreakSkipFirst();
		ProgressHelper?.Dispose();
	}

	private bool IsPrintAtBottomOrFooterOnAllPages(StiContainer container)
	{
		if (container == null)
		{
			return false;
		}
		if (container.ContainerInfoV2.ParentBand is IStiPrintAtBottom { PrintAtBottom: not false })
		{
			return true;
		}
		if (container.ContainerInfoV2.ParentBand is StiFooterBand stiFooterBand)
		{
			return stiFooterBand.PrintOnAllPages;
		}
		return false;
	}

	private void ChangeEngineParamsByKeep(StiContainer containerForRender, StiContainer selectedContainer)
	{
		if (selectedContainer == null)
		{
			return;
		}
		int num = containerForRender.Components.IndexOf(selectedContainer);
		for (int i = num; i < containerForRender.Components.Count; i++)
		{
			StiComponent stiComponent = containerForRender.Components[i];
			if (stiComponent is StiContainer && !IsPrintAtBottomOrFooterOnAllPages(stiComponent as StiContainer))
			{
				PositionY -= stiComponent.Height;
			}
			FreeSpace += stiComponent.Height;
		}
	}

	private void SetNewColumnParameters()
	{
		if (IsCrossBandsMode)
		{
			CrossFreeSpace = StaticBands.ReservedCrossFreeSpace;
			PositionX = StaticBands.ReservedPositionX;
			PositionY = StaticBands.ReservedPositionY;
			return;
		}
		FreeSpace = StaticBands.ReservedFreeSpace;
		if (ColumnsOnPanel.RightToLeft)
		{
			PositionX -= ColumnsOnPanel.GetColumnWidth() + ColumnsOnPanel.ColumnGaps;
		}
		else
		{
			PositionX += ColumnsOnPanel.GetColumnWidth() + ColumnsOnPanel.ColumnGaps;
		}
		PositionY = StaticBands.ReservedPositionY + OffsetNewColumnY;
		PositionBottomY = StaticBands.ReservedPositionBottomY;
	}

	internal void SetNewPageParameters()
	{
		if (IsCrossBandsMode)
		{
			CrossFreeSpace = TemplateContainer.Width;
			PositionX = 0.0;
			PositionY = 0.0;
		}
		else
		{
			FreeSpace = TemplateContainer.Height;
			if (ColumnsOnPanel.RightToLeft)
			{
				PositionX = TemplateContainer.Width - ColumnsOnPanel.GetColumnWidth();
			}
			else
			{
				PositionX = 0.0;
			}
			PositionY = 0.0;
			PositionBottomY = TemplateContainer.Height;
		}
		ColumnsOnPanel.CurrentColumn = 1;
	}

	private void ProcessNewContainerBefore(StiBand band)
	{
		needResetPageNumberForNewPage = band.ResetPageNumber;
		if (GenerateNewPageBeforeBand)
		{
			GenerateNewPageBeforeBand = false;
			bool flag = CanGenerateNewContainer(band as IStiPageBreak);
			bool flag2 = false;
			if (IgnoreSkipFirst)
			{
				IgnoreSkipFirst = false;
				NewPage();
				flag2 = true;
			}
			else if (flag)
			{
				NewPage();
				flag2 = true;
			}
			if (flag2 && band is StiDataBand && ((StiDataBand)band).Columns > 1 && ColumnsOnDataBand.GetColumns() == null)
			{
				ColumnsOnDataBand.RenderColumns(band as StiDataBand);
			}
		}
		else if (GenerateNewColumnBeforeBand)
		{
			GenerateNewColumnBeforeBand = false;
			bool flag3 = CanGenerateNewContainer(band as IStiPageBreak);
			if (IgnoreSkipFirst)
			{
				IgnoreSkipFirst = false;
				NewColumn();
			}
			else if (flag3)
			{
				NewColumn();
			}
		}
		else
		{
			if (!(band is IStiPageBreak stiPageBreak))
			{
				return;
			}
			if (stiPageBreak.NewPageBefore || stiPageBreak.NewColumnBefore)
			{
				double num = 100.0 * FreeSpace / ContainerForRender.Height;
				if ((double)stiPageBreak.BreakIfLessThan > num || stiPageBreak.BreakIfLessThan == 100f)
				{
					if (stiPageBreak.NewPageBefore && CanGenerateNewContainer(band as IStiPageBreak))
					{
						NewPage();
						ProcessNewContainerInDetailBands(band, newPage: true, newColumn: false);
					}
					else if (stiPageBreak.NewColumnBefore && CanGenerateNewContainer(band as IStiPageBreak))
					{
						NewColumn();
						ProcessNewContainerInDetailBands(band, newPage: false, newColumn: true);
					}
				}
			}
		}
		needResetPageNumberForNewPage = false;
	}

	private void ProcessNewContainerAfter(StiBand band)
	{
		if (!(band is IStiPageBreak stiPageBreak) || (!stiPageBreak.NewPageAfter && !stiPageBreak.NewColumnAfter))
		{
			return;
		}
		double num = 100.0 * FreeSpace / ContainerForRender.Height;
		if ((double)stiPageBreak.BreakIfLessThan > num || stiPageBreak.BreakIfLessThan == 100f)
		{
			if (stiPageBreak.NewPageAfter)
			{
				GenerateNewPageBeforeBand = true;
				IgnoreSkipFirst = true;
			}
			else if (stiPageBreak.NewColumnAfter)
			{
				GenerateNewColumnBeforeBand = true;
				IgnoreSkipFirst = true;
			}
		}
	}

	private void ProcessNewContainerInDetailBands(StiBand band, bool newPage, bool newColumn)
	{
		if (!(band is StiDataBand stiDataBand))
		{
			return;
		}
		lock (((ICollection)stiDataBand.DataBandInfoV2.DetailDataBands).SyncRoot)
		{
			foreach (StiBand detailDataBand in stiDataBand.DataBandInfoV2.DetailDataBands)
			{
				if (detailDataBand.Parent == stiDataBand.Parent)
				{
					continue;
				}
				StiEngine stiEngine = MasterEngine ?? this;
				if (!(stiEngine.SlaveEngines[detailDataBand.Parent.Name] is StiEngine stiEngine2))
				{
					continue;
				}
				if (newPage)
				{
					stiEngine2.NewPage();
				}
				else if (newColumn)
				{
					if (band.Page != null && band.Page.Columns < 2)
					{
						stiEngine2.NewPage();
					}
					else
					{
						stiEngine2.NewColumn();
					}
				}
			}
		}
	}

	private StiContainer SearchStartOfKeepContainer(StiContainer cont, int oldIndexOfStartList)
	{
		if (cont == null)
		{
			return null;
		}
		int num = oldIndexOfStartList;
		int num2 = -1;
		int num3 = -1;
		StiContainer stiContainer = null;
		while (num < cont.Components.Count)
		{
			StiLevelStartContainer stiLevelStartContainer = cont.Components[num] as StiLevelStartContainer;
			if (stiLevelStartContainer != null)
			{
				if (num2 == -1)
				{
					stiContainer = stiLevelStartContainer;
					num3 = num;
				}
				num2++;
			}
			StiLevelEndContainer stiLevelEndContainer = cont.Components[num] as StiLevelEndContainer;
			if (stiLevelEndContainer != null)
			{
				num2--;
				if (num2 < 0)
				{
					num2 = -1;
					stiContainer = null;
					num3 = -1;
				}
			}
			if (stiLevelStartContainer != null || stiLevelEndContainer != null)
			{
				cont.Components.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		if (stiContainer != null)
		{
			stiContainer = ((num3 < cont.Components.Count) ? (cont.Components[num3] as StiContainer) : null);
		}
		return stiContainer;
	}

	private void MoveKeepComponentsOnNextContainer(StiContainer cont, StiContainer selectedStartCont)
	{
		if (cont == null || selectedStartCont == null)
		{
			return;
		}
		int num = cont.Components.IndexOf(selectedStartCont);
		if (num == -1)
		{
			return;
		}
		List<StiContainer> list = new List<StiContainer>();
		for (int i = num; i < cont.Components.Count; i++)
		{
			if (!(cont.Components[i] is StiContainer stiContainer) || stiContainer is StiFooterMarkerContainer || stiContainer.ContainerInfoV2.IsAutoRendered)
			{
				continue;
			}
			list.Add(stiContainer);
			for (int j = i + 1; j < cont.Components.Count; j++)
			{
				StiContainer stiContainer2 = cont.Components[j] as StiContainer;
				bool flag = false;
				if (stiContainer2 != null)
				{
					if (stiContainer2.ContainerInfoV2.ParentBand is StiColumnHeaderBand)
					{
						flag = true;
					}
					if (stiContainer2.ContainerInfoV2.ParentBand is IStiPrintAtBottom { PrintAtBottom: not false })
					{
						break;
					}
				}
				bool flag2 = cont.Components[j].Top > stiContainer.Top;
				if (!flag && flag2)
				{
					cont.Components[j].Top -= stiContainer.Height;
				}
			}
		}
		lock (((ICollection)list).SyncRoot)
		{
			foreach (StiContainer item in list)
			{
				Breakable.SetCanBreak(item);
				cont.Components.Remove(item);
				item.Parent = null;
				if (!IsNeedToSkip(item.ContainerInfoV2.ParentBand))
				{
					RenderContainer(item, PrintAtBottom.CanProcess(item.ContainerInfoV2.ParentBand), FootersOnAllPages.CanProcess(item.ContainerInfoV2.ParentBand));
				}
			}
		}
		if (ContainerForRender != null)
		{
			int count = ContainerForRender.Components.Count;
			if (count > 0 && ContainerForRender.Components[count - 1] is StiContainer stiContainer3 && stiContainer3.ContainerInfoV2.ParentBand is StiDataBand)
			{
				PrintOnAllPagesIgnoreList.Clear();
				AtLeastOneDatabandRenderedOnPage = true;
			}
		}
	}

	private void CorrectPrintOnAllPagesIgnoreListBeforeNewList(StiContainer cont, StiContainer selectedStartCont, bool clear)
	{
		if (!clear)
		{
			if (cont == null || selectedStartCont == null)
			{
				return;
			}
			printOnAllPagesIgnoreList2.Clear();
			int num = cont.Components.IndexOf(selectedStartCont);
			for (int i = num; i < cont.Components.Count; i++)
			{
				if (cont.Components[i] is StiContainer stiContainer && !stiContainer.ContainerInfoV2.IsAutoRendered && (BandsOnAllPages.IsBandInBandsList(stiContainer.ContainerInfoV2.ParentBand) || (stiContainer.ContainerInfoV2.ParentBand is StiChildBand && BandsOnAllPages.IsBandInBandsList(((StiChildBand)stiContainer.ContainerInfoV2.ParentBand).GetMaster()))))
				{
					if (!PrintOnAllPagesIgnoreList.ContainsKey(stiContainer.ContainerInfoV2.ParentBand))
					{
						printOnAllPagesIgnoreList2[stiContainer.ContainerInfoV2.ParentBand] = stiContainer.ContainerInfoV2.ParentBand;
					}
					PrintOnAllPagesIgnoreList[stiContainer.ContainerInfoV2.ParentBand] = stiContainer.ContainerInfoV2.ParentBand;
				}
			}
			return;
		}
		foreach (DictionaryEntry item in printOnAllPagesIgnoreList2)
		{
			PrintOnAllPagesIgnoreList.Remove(item.Key);
		}
		printOnAllPagesIgnoreList2.Clear();
	}

	private bool IsNeedToPrintOddEven(StiBand band)
	{
		if (!(band is IStiPrintOnEvenOddPages stiPrintOnEvenOddPages))
		{
			return true;
		}
		if (stiPrintOnEvenOddPages.PrintOnEvenOddPages == StiPrintOnEvenOddPagesType.Ignore)
		{
			return true;
		}
		int num = Report.PageNumber & 1;
		if (stiPrintOnEvenOddPages.PrintOnEvenOddPages == StiPrintOnEvenOddPagesType.PrintOnEvenPages && num == 0)
		{
			return true;
		}
		if (stiPrintOnEvenOddPages.PrintOnEvenOddPages == StiPrintOnEvenOddPagesType.PrintOnOddPages && num == 1)
		{
			return true;
		}
		return false;
	}

	private bool IsNeedToSkip(StiBand band)
	{
		if (band == null)
		{
			return false;
		}
		if (PrintOnAllPagesIgnoreList[band] != null)
		{
			return true;
		}
		if (band is StiDataBand && ContainerForRender != null)
		{
			StiComponentsCollection components = ContainerForRender.Components;
			if (components.Count > 0 && components[components.Count - 1] is StiContainer stiContainer && stiContainer.ContainerInfoV2.ParentBand == band && stiContainer.ContainerInfoV2.IsAutoRendered)
			{
				return true;
			}
		}
		return false;
	}

	internal void RenderFootersOnAllPages(StiContainer outContainer, int startIndex, ref StiContainer markerContainer)
	{
		if (outContainer != null)
		{
			FootersOnAllPages.Render(outContainer, startIndex, ref markerContainer);
		}
		else
		{
			FootersOnAllPages.Render(ContainerForRender, startIndex, ref markerContainer);
		}
	}

	internal void RenderEmptyBands(StiContainer containerForRender, StiContainer selectedContainer)
	{
		if (containerForRender != null)
		{
			EmptyBands.Render(containerForRender, selectedContainer);
		}
	}

	internal void RenderPrintAtBottom(StiContainer container, int startIndex, StiContainer markerContainer)
	{
		PrintAtBottom.Render(container ?? ContainerForRender, startIndex, markerContainer);
	}

	public void FinishContainer(StiContainer containerForRender)
	{
		if (containerForRender == null)
		{
			return;
		}
		lock (((ICollection)containerForRender.Components).SyncRoot)
		{
			foreach (StiComponent component in containerForRender.Components)
			{
				if (component is StiContainer stiContainer && (stiContainer.ComponentType != StiComponentType.Static || stiContainer.Page == null || stiContainer.Page.Columns <= 1))
				{
					stiContainer.ContainerInfoV2.ParentBand = null;
				}
			}
		}
	}

	public void FinishResetPageNumberContainer(StiContainer containerForRender, bool isFinal)
	{
		if (containerForRender == null)
		{
			return;
		}
		lock (((ICollection)containerForRender.Components).SyncRoot)
		{
			foreach (StiComponent component in containerForRender.Components)
			{
				if (component is StiContainer stiContainer && stiContainer.ContainerInfoV2.ParentBand != null && stiContainer.ContainerInfoV2.ParentBand.ResetPageNumber && !stiContainer.ContainerInfoV2.IgnoreResetPageNumber && !stiContainer.ContainerInfoV2.IsAutoRendered)
				{
					int num = Report.RenderedPages.Count - 2;
					if (isFinal)
					{
						num++;
					}
					PageNumbers.ResetPageNumber(num);
				}
			}
		}
	}

	public void FinishColumns(StiContainer containerForRender)
	{
		if (containerForRender == null)
		{
			return;
		}
		int num = 0;
		while (num < containerForRender.Components.Count)
		{
			if (containerForRender.Components[num] is StiColumnsContainer stiColumnsContainer)
			{
				stiColumnsContainer.FinishColumns();
				containerForRender.Components.Remove(stiColumnsContainer);
				lock (((ICollection)stiColumnsContainer.Components).SyncRoot)
				{
					foreach (StiComponent component in stiColumnsContainer.Components)
					{
						component.Left += stiColumnsContainer.Left;
						component.Top += stiColumnsContainer.Top;
						component.Parent = containerForRender;
						containerForRender.Components.Insert(num, component);
						num++;
					}
				}
			}
			else
			{
				num++;
			}
		}
	}

	public void AddContainerToDestination(StiContainer container)
	{
		if (ContainerForRender != null)
		{
			if (container.ContainerInfoV2.SetSegmentPerWidth != -1 && Page != null && container.ContainerInfoV2.SetSegmentPerWidth > Page.SegmentPerWidth)
			{
				Page.SegmentPerWidth = container.ContainerInfoV2.SetSegmentPerWidth;
			}
			ContainerForRender.Components.Add(container);
		}
	}

	internal void InvokePageAfterPrint()
	{
		if (TemplatePage != null && TemplatePage.Report != null && TemplatePage.Report.RenderedPages.Count != 0)
		{
			StiPage stiPage = Report.RenderedPages[Report.RenderedPages.Count - 1];
			stiPage?.InvokeAfterPrint(StiOptions.Engine.UseTemplateForPagePrintEvents ? TemplatePage : stiPage, EventArgs.Empty);
		}
	}

	public void AddPageToRenderedPages(StiPage page)
	{
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		InvokePageAfterPrint();
		if (TemplatePage.StopBeforePrint != 0 && TemplatePage.PageInfoV2.RenderedCount - 1 == TemplatePage.StopBeforePrint)
		{
			throw new StiStopBeforePrintException();
		}
		if (Report.StopBeforePage != 0 && Report.StopBeforePage <= Report.CurrentPrintPage)
		{
			throw new StiStopBeforePageException();
		}
		if (StiOptions.Engine.FixPageNumberInEvents)
		{
			Report.CurrentPrintPage++;
			Report.RenderedPages.Add(page);
			ProcessRendering();
			TemplatePage.InvokeRendering();
		}
		else
		{
			Report.RenderedPages.Add(page);
			Report.CurrentPrintPage++;
		}
		if (Page.Report.RenderedPages.Count != 0)
		{
			if (!SkipFirstPageBeforePrintEvent)
			{
				Page.InvokeBeforePrint(StiOptions.Engine.UseTemplateForPagePrintEvents ? TemplatePage : Page, EventArgs.Empty);
			}
			SkipFirstPageBeforePrintEvent = false;
			Page.PaperSize = TemplatePage.PaperSize;
			Page.Orientation = TemplatePage.Orientation;
			Page.PageWidth = TemplatePage.PageWidth;
			Page.PageHeight = TemplatePage.PageHeight;
			Page.Margins = new StiMargins(TemplatePage.Margins.Left, TemplatePage.Margins.Right, TemplatePage.Margins.Top, TemplatePage.Margins.Bottom);
		}
	}

	private bool CheckFreeSpace(StiContainer container)
	{
		if (IsCrossBandsMode)
		{
			return CheckFreeSpace(container.Width);
		}
		bool flag = CheckFreeSpace(container.Height);
		if (flag)
		{
			Breakable.SetCanBreak(container);
			return false;
		}
		return flag;
	}

	private bool CheckFreeSpace(double value)
	{
		if (IsCrossBandsMode)
		{
			if ((decimal)value > (decimal)CrossFreeSpace)
			{
				NewDestination();
				return (decimal)value <= (decimal)CrossFreeSpace;
			}
		}
		else if ((decimal)value > (decimal)FreeSpace)
		{
			if (!TemplatePage.UnlimitedHeight || TemplatePage.Columns >= 2)
			{
				NewDestination();
				return true;
			}
			if (Page == null)
			{
				FreeSpace += TemplatePage.PageHeight - TemplatePage.Margins.Top - TemplatePage.Margins.Bottom;
				return false;
			}
			while ((int)((PositionY + value) / TemplatePage.Height + 1.0) > Page.SegmentPerHeight)
			{
				Page.SegmentPerHeight++;
				if (!TemplatePage.UnlimitedBreakable)
				{
					FreeSpace += TemplatePage.Height;
				}
			}
			if (value > FreeSpace + (TemplatePage.Height - PositionBottomY))
			{
				if (TemplatePage.UnlimitedBreakable)
				{
					if (PositionY > 0.0)
					{
						FreeSpace += TemplatePage.Height - PositionBottomY;
						bool ignoreUnlimitedHeightForNewPage = IgnoreUnlimitedHeightForNewPage;
						IgnoreUnlimitedHeightForNewPage = false;
						NewDestination();
						IgnoreUnlimitedHeightForNewPage = ignoreUnlimitedHeightForNewPage;
					}
					if (value > TemplatePage.Height)
					{
						return true;
					}
				}
				return false;
			}
		}
		return false;
	}

	private void StoreLatestDataBand(StiBand band)
	{
		if (!(band is StiDataBand))
		{
			return;
		}
		if (ColumnsOnDataBand.Enabled)
		{
			StiColumnsContainer columns = ColumnsOnDataBand.GetColumns();
			if (columns == null)
			{
				IndexOfLatestDataBand = new StiIndex(0, ContainerForRender.Components.Count);
				return;
			}
			int indexInColumnContainer = ContainerForRender.Components.IndexOf(columns);
			IndexOfLatestDataBand = new StiIndex(columns.Components.Count, indexInColumnContainer);
		}
		else
		{
			IndexOfLatestDataBand = new StiIndex(ContainerForRender.Components.Count);
		}
	}

	private void SetReportVariables(StiBand band)
	{
		band.SetReportVariables();
		if (ColumnsOnDataBand.Enabled && band is StiDataBand)
		{
			StiColumnsContainer columns = ColumnsOnDataBand.GetColumns();
			if (columns != null)
			{
				Report.Column = columns.GetCurrentColumn();
			}
		}
		else
		{
			Report.Column = ColumnsOnPanel.CurrentColumn;
		}
	}

	public bool CheckForDuplicate(string textName, string value, string tag)
	{
		if (DuplicatesLastValues == null)
		{
			DuplicatesLastValues = new Hashtable();
		}
		if (DuplicatesLastValues.ContainsKey(textName) && value == (string)DuplicatesLastValues[textName] && tag == (string)DuplicatesLastValues[textName + "_tag"])
		{
			return true;
		}
		DuplicatesLastValues[textName] = value;
		DuplicatesLastValues[textName + "_tag"] = tag;
		return false;
	}

	public void ResetProcessingDuplicates(string componentName)
	{
		if (DuplicatesLastValues != null && DuplicatesLastValues.ContainsKey(componentName))
		{
			DuplicatesLastValues[componentName] = null;
		}
	}

	public void ResetProcessingDuplicates(StiSimpleText component)
	{
		string key = $"{component.Left}_{component.Width}";
		if (DuplicatesLastValues != null && DuplicatesLastValues.ContainsKey(key))
		{
			DuplicatesLastValues[key] = null;
		}
	}

	public decimal GetSumTagsOnPage(StiPage page, string componentName)
	{
		decimal result = default(decimal);
		foreach (StiComponent component in page.GetComponents())
		{
			if (component.Name == componentName && component.TagValue != null)
			{
				result += Convert.ToDecimal(component.TagValue);
			}
		}
		return result;
	}

	public StiComponent GetComponentByNameFromRenderedPage(StiPage page, string componentName)
	{
		foreach (StiComponent component in page.GetComponents())
		{
			if (component.Name == componentName)
			{
				return component;
			}
		}
		return null;
	}

	public StiComponentsCollection RenderBand(StiBand band)
	{
		return RenderBand(band, ignorePageBreaks: false, allowRenderingEvents: false);
	}

	internal StiComponentsCollection RenderBand(StiBand band, bool ignorePageBreaks, bool allowRenderingEvents)
	{
		BandsInProgress.Add(band);
		try
		{
			StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
			bool flag = true;
			if (band is StiDataBand && ((StiDataBand)band).Columns > 1)
			{
				flag = false;
			}
			StiComponentsCollection stiComponentsCollection2 = null;
			if (flag)
			{
				stiComponentsCollection2 = GetChildBands(band);
				lock (((ICollection)stiComponentsCollection2).SyncRoot)
				{
					foreach (StiChildBand item in stiComponentsCollection2)
					{
						if (item.KeepChildTogether)
						{
							AddLevel();
						}
					}
				}
			}
			bool isChildsEnabled = true;
			StiContainer stiContainer = InternalRenderBand(band, ignorePageBreaks, allowRenderingEvents, ref isChildsEnabled);
			if (stiContainer != null)
			{
				stiComponentsCollection.Add(stiContainer);
			}
			if (flag)
			{
				lock (((ICollection)stiComponentsCollection2).SyncRoot)
				{
					foreach (StiChildBand item2 in stiComponentsCollection2)
					{
						bool enabled = item2.Enabled;
						if (!isChildsEnabled && !item2.PrintIfParentDisabled)
						{
							item2.Enabled = false;
						}
						bool isChildsEnabled2 = isChildsEnabled || item2.PrintIfParentDisabled;
						StiContainer stiContainer2 = InternalRenderBand(item2, ignorePageBreaks, allowRenderingEvents, ref isChildsEnabled2);
						if (stiContainer2 != null)
						{
							stiComponentsCollection.Add(stiContainer2);
						}
						if (item2.KeepChildTogether)
						{
							RemoveLevel();
						}
						item2.Enabled = enabled;
					}
				}
			}
			if (((band is StiHeaderBand && (band as StiHeaderBand).PrintOnAllPages) || (band is StiFooterBand && (band as StiFooterBand).PrintOnAllPages)) && stiContainer != null && stiContainer.Components.Count > 0)
			{
				StiEngine stiEngine = this;
				if (Report?.Engine != null)
				{
					stiEngine = Report.Engine;
				}
				if (stiEngine.componentPlacementRemakeTable == null)
				{
					stiEngine.componentPlacementRemakeTable = new Hashtable();
				}
				if (!stiEngine.componentPlacementRemakeTable.ContainsKey(band))
				{
					stiEngine.componentPlacementRemakeTable[band] = null;
					StiDataBand stiDataBand = null;
					if (band is StiHeaderBand)
					{
						stiDataBand = StiHeaderBandV2Builder.GetMaster(band as StiHeaderBand);
					}
					if (band is StiFooterBand)
					{
						stiDataBand = StiFooterBandV2Builder.GetMaster(band as StiFooterBand);
					}
					if (stiDataBand?.MasterComponent is StiDataBand)
					{
						stiEngine.componentPlacementRemakeTable[band] = stiDataBand.MasterComponent;
					}
				}
				object obj = stiEngine.componentPlacementRemakeTable[band];
				if (obj != null && obj is StiDataBand)
				{
					string componentPlacement = stiContainer.Components[0].ComponentPlacement;
					if (!string.IsNullOrEmpty(componentPlacement))
					{
						componentPlacement += $"_r{(obj as StiDataBand).Position}";
						foreach (StiComponent component in stiContainer.Components)
						{
							component.ComponentPlacement = componentPlacement;
						}
					}
				}
			}
			return stiComponentsCollection;
		}
		finally
		{
			BandsInProgress.RemoveAt(BandsInProgress.Count - 1);
		}
	}

	private StiContainer InternalRenderBand(StiBand band, bool ignorePageBreaks, bool allowRenderingEvents, ref bool isChildsEnabled)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = null;
		StoreLatestDataBand(band);
		StiBrush brush = null;
		StiBorderSides side = (StiBorderSides)0;
		if (band != null)
		{
			brush = band.Brush;
		}
		if (band != null && band.Border != null)
		{
			side = band.Border.Side;
		}
		bool enabled = band.Enabled;
		StiReport report = band.Report;
		StiBaseStyle stiBaseStyle = null;
		bool flag = false;
		if (band.UseParentStyles && band.Parent != null)
		{
			if (report != null && report.Engine != null && report.Engine.HashParentStyles != null && report.Engine.HashParentStyles.Count > 0)
			{
				stiBaseStyle = report.Engine.HashParentStyles[band.Parent] as StiBaseStyle;
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiBaseStyle.GetStyle(band.Parent);
			}
			if (stiBaseStyle != null)
			{
				stiBaseStyle.SetStyleToComponent(band);
				flag = true;
				band.GetComponentStyle()?.SetStyleToComponent(band);
			}
		}
		if (!StiOptions.Engine.UseParentStylesOldMode)
		{
			StiBaseStyle stiBaseStyle2 = StiOddEvenStylesHelper.ApplyOddEvenStyles(Report, band as StiDataBand, band);
			if (stiBaseStyle2 != null)
			{
				stiBaseStyle = stiBaseStyle2;
			}
		}
		band.InvokeBeforePrint(band, EventArgs.Empty);
		if (report != null && report.Engine != null && report.Engine.HashUseParentStyles.ContainsKey(band))
		{
			if (stiBaseStyle == null && !string.IsNullOrWhiteSpace(band.ComponentStyle))
			{
				stiBaseStyle = report.Styles[band.ComponentStyle];
			}
			if (report.Engine.HashParentStyles != null && report.Engine.HashParentStyles.Count > 0 && report.Engine.HashParentStyles[band] is StiBaseStyle stiBaseStyle3)
			{
				stiBaseStyle = stiBaseStyle3;
			}
			StiBaseStyle style = StiBaseStyle.GetStyle(band, stiBaseStyle);
			if (report.Engine.HashParentStyles == null)
			{
				report.Engine.HashParentStyles = new Hashtable();
			}
			report.Engine.HashParentStyles[band] = style;
			flag = true;
		}
		StiDataBand stiDataBand = band as StiDataBand;
		StiBusinessObject stiBusinessObject = stiDataBand?.BusinessObject;
		if (StiOptions.Engine.OldModeOfRenderingEventInEngineV2 && stiDataBand != null && allowRenderingEvents)
		{
			if (band.Enabled)
			{
				stiDataBand.InvokeRendering();
				stiDataBand.InvokeGroupRendering();
			}
			else if (stiDataBand.CalcInvisible)
			{
				stiDataBand.InvokeRendering();
				stiDataBand.InvokeGroupRendering();
			}
		}
		if (band is StiDataBand && !IsNeedToSkip(band) && !DenyClearPrintOnAllPagesIgnoreList)
		{
			PrintOnAllPagesIgnoreList.Clear();
			AtLeastOneDatabandRenderedOnPage = true;
		}
		if (!isChildsEnabled)
		{
			band.Enabled = false;
		}
		isChildsEnabled = band.Enabled;
		if (IsNeedToPrintOddEven(band) && !IsNeedToSkip(band) && band.IsEnabled)
		{
			SetReportVariables(band);
			if (!ignorePageBreaks)
			{
				ProcessNewContainerBefore(band);
			}
			int num = 0;
			if (stiDataBand != null)
			{
				num = stiDataBand.Position;
			}
			stiContainer = band.InternalRender() as StiContainer;
			if (stiDataBand != null)
			{
				stiContainer.ContainerInfoV2.DataBandPosition = num;
				bool flag2 = stiDataBand.FilterMethodHandler != null || (stiDataBand.Sort != null && stiDataBand.Sort.Length != 0) || (report.DataBandsUsedInPageTotals != null && Array.IndexOf(report.DataBandsUsedInPageTotals, band.Name) != -1);
				if (flag2 && stiDataBand.DataSource != null)
				{
					stiContainer.ContainerInfoV2.DataSourceRow = stiDataBand.DataSource.GetDataRow(stiDataBand.DataSource.Position);
				}
				if (stiBusinessObject != null && (stiDataBand.MasterComponent != null || flag2))
				{
					stiContainer.ContainerInfoV2.BusinessObjectCurrent = stiBusinessObject.Current;
				}
			}
			if (stiDataBand != null)
			{
				if (stiBusinessObject != null && !stiBusinessObject.isEnumeratorCreated)
				{
					stiBusinessObject.SetDetails();
				}
				int position = stiDataBand.Position;
				if (position < num)
				{
					stiDataBand.Position = num;
				}
				else
				{
					num = position;
				}
			}
			if (band.Parent != null && !band.IsCross)
			{
				if (band.Parent is StiPage)
				{
					if (band.ComponentType == StiComponentType.Static)
					{
						stiContainer.Width = ((StiPage)band.Parent).Width;
					}
					else
					{
						stiContainer.Width = ((StiPage)band.Parent).GetColumnWidth();
					}
				}
				else
				{
					stiContainer.Width = band.Parent.Width;
				}
			}
			StiPage page = stiContainer.Page;
			stiContainer.Page = band.Page;
			stiContainer.InvokeEvents();
			stiContainer.Page = page;
			stiContainer.ContainerInfoV2.ParentBand = band;
			if (band.ComponentType != StiComponentType.Static)
			{
				stiContainer.DockStyle = StiDockStyle.None;
			}
			if (StiOptions.Engine.UseParentStylesOldMode)
			{
				StiOddEvenStylesHelper.ApplyOddEvenStyles(Report, band as StiDataBand, stiContainer);
			}
			if (!SilentMode)
			{
				RenderContainer(stiContainer, PrintAtBottom.CanProcess(band), FootersOnAllPages.CanProcess(band));
				if (stiDataBand != null && stiDataBand.Position < num)
				{
					stiDataBand.Position = num;
				}
			}
			if (!ignorePageBreaks)
			{
				ProcessNewContainerAfter(band);
			}
		}
		if (!StiOptions.Engine.OldModeOfRenderingEventInEngineV2 && stiDataBand != null && allowRenderingEvents)
		{
			if (band.Enabled)
			{
				stiDataBand.InvokeRendering();
				stiDataBand.InvokeGroupRendering();
			}
			else if (stiDataBand.CalcInvisible)
			{
				stiDataBand.InvokeRendering();
				stiDataBand.InvokeGroupRendering();
			}
		}
		band.InvokeAfterPrint(band, EventArgs.Empty);
		if (flag && report.Engine.HashParentStyles != null)
		{
			report.Engine.HashParentStyles.Remove(band);
		}
		if (band != null)
		{
			band.Brush = brush;
		}
		if (band?.Border != null)
		{
			band.Border.Side = side;
		}
		band.Enabled = enabled;
		if (band != null)
		{
			StiContainer stiContainer2 = stiContainer;
			if (stiContainer2 != null && band.Border != stiContainer2.Border && ((object)band.Border).Equals((object?)stiContainer2.Border))
			{
				stiContainer2.Border = band.Border;
			}
		}
		CheckContainerOnTable(stiContainer);
		return stiContainer;
	}

	private void CheckContainerOnTable(StiContainer panel)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		if (panel?.ContainerInfoV2?.ParentBand == null)
		{
			return;
		}
		if (panel.ContainerInfoV2.ParentBand is StiHeaderBand)
		{
			if (!((StiHeaderBand)panel.ContainerInfoV2.ParentBand).HeaderBandInfoV2.IsTableHeader)
			{
				return;
			}
			StiComponentsCollection components = panel.GetComponents();
			{
				foreach (StiComponent item in components)
				{
					if (!(item is IStiTableCell { Join: not false, ParentJoinCell: not null } stiTableCell))
					{
						continue;
					}
					if (item.Parent != null)
					{
						StiContainer parent = item.Parent;
						SizeD minSize = (item.Parent.MaxSize = new SizeD(item.Parent.Width, item.Parent.Height));
						parent.MinSize = minSize;
					}
					StiComponent stiComponent2 = components[stiTableCell.ParentJoinCell.Name];
					if (stiComponent2 != null)
					{
						item.Height = stiComponent2.Parent.Bottom - item.Parent.Top;
						if (stiTableCell.CellDockStyle != StiDockStyle.None)
						{
							SizeD minSize = (item.MinSize = new SizeD(item.Width, item.Height));
							item.MaxSize = minSize;
						}
					}
				}
				return;
			}
		}
		if (panel.ContainerInfoV2.ParentBand is StiGroupHeaderBand)
		{
			if (!((StiGroupHeaderBand)panel.ContainerInfoV2.ParentBand).GroupHeaderBandInfoV2.IsTableGroupHeader)
			{
				return;
			}
			StiComponentsCollection components2 = panel.GetComponents();
			{
				foreach (StiComponent item2 in components2)
				{
					if (!(item2 is IStiTableCell { Join: not false, ParentJoinCell: not null } stiTableCell2))
					{
						continue;
					}
					if (item2.Parent != null)
					{
						StiContainer parent2 = item2.Parent;
						SizeD minSize = (item2.Parent.MaxSize = new SizeD(item2.Parent.Width, item2.Parent.Height));
						parent2.MinSize = minSize;
					}
					StiComponent stiComponent4 = components2[stiTableCell2.ParentJoinCell.Name];
					if (stiComponent4 != null)
					{
						item2.Height = stiComponent4.Parent.Bottom - item2.Parent.Top;
						if (stiTableCell2.CellDockStyle != StiDockStyle.None)
						{
							SizeD minSize = (item2.MinSize = new SizeD(item2.Width, item2.Height));
							item2.MaxSize = minSize;
						}
					}
				}
				return;
			}
		}
		if (panel.ContainerInfoV2.ParentBand is StiTable)
		{
			StiComponentsCollection components3 = panel.GetComponents();
			{
				foreach (StiComponent item3 in components3)
				{
					if (!(item3 is IStiTableCell { Join: not false, ParentJoinCell: not null } stiTableCell3))
					{
						continue;
					}
					if (item3.Parent != null)
					{
						StiContainer parent3 = item3.Parent;
						SizeD minSize = (item3.Parent.MaxSize = new SizeD(item3.Parent.Width, item3.Parent.Height));
						parent3.MinSize = minSize;
					}
					StiComponent stiComponent6 = components3[stiTableCell3.ParentJoinCell.Name];
					if (stiComponent6 != null)
					{
						item3.Height = stiComponent6.Parent.Bottom - item3.Parent.Top;
						if (stiTableCell3.CellDockStyle != StiDockStyle.None)
						{
							SizeD minSize = (item3.MinSize = new SizeD(item3.Width, item3.Height));
							item3.MaxSize = minSize;
						}
					}
				}
				return;
			}
		}
		if (panel.ContainerInfoV2.ParentBand is StiFooterBand)
		{
			if (!((StiFooterBand)panel.ContainerInfoV2.ParentBand).FooterBandInfoV2.IsTableFooter)
			{
				return;
			}
			StiComponentsCollection components4 = panel.GetComponents();
			{
				foreach (StiComponent item4 in components4)
				{
					if (!(item4 is IStiTableCell { Join: not false, ParentJoinCell: not null } stiTableCell4))
					{
						continue;
					}
					if (item4.Parent != null)
					{
						StiContainer parent4 = item4.Parent;
						SizeD minSize = (item4.Parent.MaxSize = new SizeD(item4.Parent.Width, item4.Parent.Height));
						parent4.MinSize = minSize;
					}
					StiComponent stiComponent8 = components4[stiTableCell4.ParentJoinCell.Name];
					if (stiComponent8 != null)
					{
						item4.Height = stiComponent8.Parent.Bottom - item4.Parent.Top;
						if (stiTableCell4.CellDockStyle != StiDockStyle.None)
						{
							SizeD minSize = (item4.MinSize = new SizeD(item4.Width, item4.Height));
							item4.MaxSize = minSize;
						}
					}
				}
				return;
			}
		}
		if (!(panel.ContainerInfoV2.ParentBand is StiGroupFooterBand) || !((StiGroupFooterBand)panel.ContainerInfoV2.ParentBand).GroupFooterBandInfoV2.IsTableGroupFooter)
		{
			return;
		}
		StiComponentsCollection components5 = panel.GetComponents();
		foreach (StiComponent item5 in components5)
		{
			if (!(item5 is IStiTableCell { Join: not false, ParentJoinCell: not null } stiTableCell5))
			{
				continue;
			}
			if (item5.Parent != null)
			{
				StiContainer parent5 = item5.Parent;
				SizeD minSize = (item5.Parent.MaxSize = new SizeD(item5.Parent.Width, item5.Parent.Height));
				parent5.MinSize = minSize;
			}
			StiComponent stiComponent10 = components5[stiTableCell5.ParentJoinCell.Name];
			if (stiComponent10 != null)
			{
				item5.Height = stiComponent10.Parent.Bottom - item5.Parent.Top;
				if (stiTableCell5.CellDockStyle != StiDockStyle.None)
				{
					SizeD minSize = (item5.MinSize = new SizeD(item5.Width, item5.Height));
					item5.MaxSize = minSize;
				}
			}
		}
	}

	internal StiContainer RenderContainer(StiContainer container)
	{
		return RenderContainer(container, isPrintAtBottom: false, isFooterOnAllPages: false);
	}

	private StiContainer RenderContainer(StiContainer container, bool isPrintAtBottom, bool isFooterOnAllPages)
	{
		if (container is StiColumnsContainer)
		{
			return InternalRenderColumnsContainer(container);
		}
		if (container.ContainerInfoV2.IsColumns && ColumnsOnDataBand.GetColumns(container) == null)
		{
			ColumnsOnDataBand.RenderColumns(container.ContainerInfoV2.ParentBand as StiDataBand);
		}
		if (container.ContainerInfoV2.ParentBand is StiDataBand { Columns: >1 } && ColumnsOnDataBand.Enabled)
		{
			return InternalRenderContainerToColumns(container, isPrintAtBottom, isFooterOnAllPages);
		}
		return InternalRenderContainer(container, isPrintAtBottom, isFooterOnAllPages);
	}

	private StiContainer InternalRenderColumnsContainer(StiContainer container)
	{
		if (IsCrossBandsMode)
		{
			container.Top = PositionY;
			container.Left = PositionX;
			AddContainerToDestination(container);
			CrossFreeSpace -= container.Width;
			PositionX += container.Width;
		}
		else
		{
			container.Left = PositionX;
			container.Top = PositionY;
			AddContainerToDestination(container);
			FreeSpace -= container.Height;
			PositionY += container.Height;
		}
		return container;
	}

	private StiContainer InternalRenderContainerToColumns(StiContainer container, bool isPrintAtBottom, bool isFooterOnAllPages)
	{
		container.ContainerInfoV2.IsColumns = true;
		StiColumnsContainer columns = ColumnsOnDataBand.GetColumns();
		if (columns == null)
		{
			return null;
		}
		double num = columns.HowMuchAdditionalSpaceNeeded(columns.Height, container);
		if (num > 0.0)
		{
			double num2 = 0.0;
			if (columns.Page != null && columns.Page.UnlimitedHeight && columns.Page.UnlimitedBreakable)
			{
				double num3 = columns.Page.PageHeight - columns.Page.Margins.Top - columns.Page.Margins.Bottom;
				if ((int)((columns.Top + columns.Height) / num3) != (int)((columns.Top + columns.Height + num) / num3))
				{
					num2 += num3 - (columns.Top + columns.Height) % num3;
					num -= num2;
				}
			}
			if (FreeSpace < num && !flagRenderColumnsOnDataBandOnNewPage && PositionY != 0.0 && LastFreeSpaceOnPageAfterNewList != FreeSpace && !(num > LastFreeSpaceOnPageAfterNewList))
			{
				if (columns.Page != null && columns.Page.UnlimitedHeight && !columns.Page.UnlimitedBreakable)
				{
					double positionY = PositionY;
					PositionY += container.Height;
					NewDestination();
					PositionY = positionY;
				}
				else
				{
					NewDestination();
				}
				StiColumnsContainer columns2 = ColumnsOnDataBand.GetColumns();
				if (columns2 != null && columns2.Name == columns.Name)
				{
					ColumnsOnDataBand.Enabled = true;
				}
				flagRenderColumnsOnDataBandOnNewPage = true;
				RenderContainer(container, isPrintAtBottom, isFooterOnAllPages);
				return container;
			}
			flagRenderColumnsOnDataBandOnNewPage = false;
			columns.Height += num + num2;
			PositionY += num;
			FreeSpace -= num;
		}
		columns.AddContainer(container);
		if (columns.Components.Count % columns.Columns == 0)
		{
			CheckBreakColumnsContainer(columns);
		}
		return container;
	}

	internal void CheckBreakColumnsContainer(StiColumnsContainer columns)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (columns != null && !(FreeSpace >= 0.0))
		{
			FreeSpace += columns.Height;
			PositionY -= columns.Height;
			columns.CanBreak = true;
			StiContainer stiContainer = Breakable.ProcessBreakable(columns);
			bool needSecondPass = false;
			SizeD actualSize = stiContainer.GetActualSize(isFirstPass: true, ref needSecondPass);
			stiContainer.Height = ((SizeD)(ref actualSize)).Height;
			RenderContainer(stiContainer);
			ColumnsOnDataBand.Enabled = true;
		}
	}

	private StiContainer InternalRenderContainer(StiContainer container, bool isPrintAtBottom, bool isFooterOnAllPages)
	{
		ColumnsOnDataBand.Enabled = false;
		if (ColumnsOnPanel.Count > 0)
		{
			if (container.ContainerInfoV2 != null && container.ContainerInfoV2.ParentBand != null && container.ContainerInfoV2.ParentBand.Parent is StiPage && container.ContainerInfoV2.ParentBand.ComponentType == StiComponentType.Static)
			{
				container.Width = ((StiPage)container.ContainerInfoV2.ParentBand.Parent).Width;
			}
			else
			{
				container.Width = ColumnsOnPanel.GetColumnWidth();
			}
		}
		if (IsNeedToSkip(container.ContainerInfoV2.ParentBand))
		{
			return null;
		}
		if (container.ContainerInfoV2.ParentBand is StiChildBand)
		{
			StiBand master = ((StiChildBand)container.ContainerInfoV2.ParentBand).GetMaster();
			if (master.ComponentType == StiComponentType.Static)
			{
				Breakable.SetCanBreak(container);
			}
		}
		bool flag = container.ContainerInfoV2.ParentBand != null && container.ContainerInfoV2.ParentBand.Page != null && container.ContainerInfoV2.ParentBand.Page.UnlimitedHeight;
		if (LastFreeSpaceOnPageAfterNewList > 0.0 && container.Height > LastFreeSpaceOnPageAfterNewList && !container.CanBreak && !flag)
		{
			Breakable.SetCanBreak(container);
			int num = -1;
			if (container.ContainerInfoV2.ParentBand is StiDataBand)
			{
				num = (container.ContainerInfoV2.ParentBand as StiDataBand).Position;
			}
			string text = $"Whilst the Can Break property was set to False the component {container.Name}{((num >= 0) ? $" (position {num})" : string.Empty)} was split because it couldn't be placed on the page entirely.";
			StiLogService.Write(text);
			Report.WriteToReportRenderingMessages(text);
		}
		container = Breakable.ProcessBreakable(container);
		bool canBreak = container.CanBreak;
		int count = Report.RenderedPages.Count;
		while (CheckFreeSpace(container))
		{
		}
		if (count != Report.RenderedPages.Count)
		{
			ReprocessRuntimeVariables(container);
		}
		if (canBreak != container.CanBreak)
		{
			container = Breakable.ProcessBreakable(container);
		}
		if (IsCrossBandsMode)
		{
			container.Top = PositionY;
			CrossFreeSpace -= container.Width;
		}
		else
		{
			container.Left = PositionX;
			FreeSpace = (double)(decimal)(FreeSpace - container.Height);
		}
		if (container.ContainerInfoV2.ParentBand is StiPageFooterBand)
		{
			container.Top = PositionBottomY - container.Height;
			PositionBottomY -= container.Height;
		}
		else
		{
			if (IsCrossBandsMode)
			{
				container.Left = PositionX;
			}
			else
			{
				container.Top = PositionY;
			}
			if (isFooterOnAllPages || isPrintAtBottom)
			{
				if (isFooterOnAllPages)
				{
					FootersOnAllPages.Add(container);
				}
				if (isPrintAtBottom)
				{
					PrintAtBottom.Add(container);
				}
			}
			else if (IsCrossBandsMode)
			{
				PositionX += container.Width;
			}
			else
			{
				PositionY += container.Height;
			}
		}
		AddContainerToDestination(container);
		if (container.ContainerInfoV2.ParentBand is StiDataBand && !DenyClearPrintOnAllPagesIgnoreList)
		{
			PrintOnAllPagesIgnoreList.Clear();
			AtLeastOneDatabandRenderedOnPage = true;
		}
		if (!IsCrossBandsMode && !flag && FreeSpace < 0.0)
		{
			NewDestination();
		}
		return container;
	}

	public StiEngine(StiReport report)
	{
		PageNumbers = new StiPageNumberHelper(this);
		EmptyBands = new StiEmptyBandsV2Helper(this);
		BandsOnAllPages = new StiBandsOnAllPages(this);
		PrintAtBottom = new StiPrintAtBottom(this);
		ProgressHelper = new StiProgressHelperV2(this);
		FootersOnAllPages = new StiFootersOnAllPages(this);
		ColumnsOnDataBand = new StiColumnsOnDataBand(this);
		ColumnsOnPanel = new StiColumnsOnPanel(this);
		StaticBands = new StiStaticBandsHelper(this);
		Breakable = new StiBreakableHelper(this);
		Threads = new StiThreads(this);
		if (report != null && report.Engine != null)
		{
			hashUseParentStyles = report.Engine.hashUseParentStyles;
		}
		Report = report;
	}
}
