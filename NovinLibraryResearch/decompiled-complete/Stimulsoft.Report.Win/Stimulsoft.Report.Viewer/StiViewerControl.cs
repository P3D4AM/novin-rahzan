using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Viewer;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.MathFormula;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Print;
using Stimulsoft.Report.Units;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Win.Trial;

namespace Stimulsoft.Report.Viewer;

[ToolboxBitmap(typeof(StiProgressInformation), "Toolbox.StiViewerControl.bmp")]
[Guid("66B9E805-F686-4555-84F5-66BAC15717C9")]
public class StiViewerControl : UserControl, IStiViewerControl, IStiApplyStyle, IStiViewerAccessor
{
	private class BookmarkComponentInfo
	{
		public int IndexComp;

		public int IndexList;

		public StiPage Page;
	}

	private delegate void ShowPrintDialog();

	private Hashtable chartComponentToInteraction = new Hashtable();

	private Timer chartInteractionRecallTimer;

	private StiChart chartInteractionLatestChart;

	private Point chartInteractionPoint;

	private RectangleD chartInteractionRect;

	private bool chartInteractionDragEnabled;

	private Point chartInteractionDragStartPoint;

	private StiCellGeom chartInteractionDragGeom;

	internal Bar tbTool;

	internal ButtonItem tbPrint;

	internal ButtonItem tbOpen;

	internal ButtonItem tbSave;

	private Panel panelView;

	internal StiVScrollBar vScrollBar;

	private Panel panelVertCorr;

	private StiViewerTreeView tvBookmarks;

	private ImageList ilBookmarks;

	private IContainer components;

	private StiPageViewControl view;

	internal StiHScrollBar hScrollBar;

	private ContextMenuBar contextMenuBar1;

	private ButtonItem contextMenuPreview;

	private ButtonItem cm200;

	private ButtonItem cm150;

	private ButtonItem cm100;

	private ButtonItem cm75;

	private ButtonItem cm50;

	private ButtonItem cm25;

	private ButtonItem cm10;

	private ButtonItem cmZoomOnePage;

	private ButtonItem cmZoomTwoPages;

	private ButtonItem cmZoomPageWidth;

	private ButtonItem contextMenuBookmarks;

	internal ButtonItem miExpandAll;

	internal ButtonItem miCollapseAll;

	private Panel panelDialogs;

	private Splitter splParameters;

	private Splitter splBookmarks;

	private Splitter splThumbs;

	internal ButtonItem tbZoomOnePage;

	internal ButtonItem tbZoomTwoPages;

	internal ButtonItem tbZoomMultiplePages;

	internal ButtonItem tbZoomPageWidth;

	internal ButtonItem tbFullScreen;

	private Panel panelHorizontal;

	internal ButtonItem tbDotMatrix;

	internal ButtonItem tbBookmarks;

	internal ButtonItem tbThumbs;

	internal ButtonItem tbParameters;

	internal ButtonItem tbResources;

	public ButtonItem tbFind;

	internal ButtonItem tbEditor;

	private Timer timerZoomSlider;

	private Bar tbStatusBar;

	private ItemContainer icZoom;

	private ItemContainer icPageViewMode;

	private ButtonItem tbViewModeSinglePage;

	private ButtonItem tbViewModeContinuous;

	private ButtonItem tbViewModeMultiplePages;

	private LabelItem lbZoom;

	private SliderItem siZoom;

	private LabelItem liSeparator;

	internal ButtonItem tbSendEMail;

	private ItemContainer icPageControl;

	private ButtonItem tbFirstPage;

	private ButtonItem tbPrevPage;

	internal ButtonItem tbPageControl;

	internal ButtonItem tbPageNew;

	internal ButtonItem tbPageDelete;

	internal ButtonItem tbPageDesign;

	internal ButtonItem tbPageSize;

	private ButtonItem tbNextPage;

	private ButtonItem tbLastPage;

	private ItemContainer icZoomPages;

	internal ButtonItem tbClose;

	private LabelItem liSeparator2;

	internal ButtonItem tbSaveDocumentFile;

	private Bar tbFindBar;

	private ButtonItem tbFindClose;

	private LabelItem liSeparator3;

	private ButtonItem tbFindNext;

	private ProgressBarItem pbFindProgress;

	private CheckBoxItem cbFindMatchCase;

	private CheckBoxItem cbFindMatchWholeWord;

	private LabelItem liFindWhat;

	private StiViewerTextBox tbFindValue;

	internal ControlContainerItem ciMultiPagesUserControl;

	internal StiMultiPagesUserControl multiPagesUserControl;

	private ControlContainerItem ciFindValue;

	private StiThumbsControl lbThumbs;

	internal StiVScrollBar scrollBarThumbs;

	internal ButtonItem tbSendEMailDocumentFile;

	private TabStrip tabStripReports;

	private ButtonItem tbFindPrevious;

	private ButtonItem tbHelp;

	private ScrollableControl listResources;

	private ToolTip toolTip;

	private Timer timerPrint;

	private bool blockHandMove;

	private StiNodesMemory nodesMemory = new StiNodesMemory();

	private StiFindHelper findHelper = new StiFindHelper();

	private StiComponent lastComponent;

	private StiComponentsCollection componentsOnPages;

	private List<bool> pageIsProcessed;

	private Hashtable bookmarksHash;

	private Hashtable componentsMouseOver = new Hashtable();

	private double restoreZoom;

	private StiViewMode restoreViewMode;

	private bool blockSaveConfig;

	private bool blockBookmarkPanelState;

	private bool ignoreParametersRefreshingOnce;

	private bool lockResourcesState;

	private bool isLeftMouseButtonPressed;

	private bool isLatestLeftMouseButtonPressed;

	private StiReport mainReport;

	private Point lastPosition = Point.Empty;

	private bool isEditableComponentsPresent = true;

	private bool isHorizontalScrollBarVisible = true;

	private bool lockSingleMode;

	private bool lockRefreshReports;

	private Timer refreshReportTimer;

	private Hashtable reportsZooms = new Hashtable();

	private Hashtable reportsHScrollBars = new Hashtable();

	private Hashtable reportsVScrollBars = new Hashtable();

	private bool isZoomPageWidth;

	private bool isZoomOnePage;

	private bool isZoomTwoPages;

	private bool isZoomMultiplePages;

	private int zoomMultiplePagesByWidth = 1;

	private int zoomMultiplePagesByHeight = 1;

	private bool lockSliderItemZoom;

	private bool lockSliderItemTimerZoom;

	private StiReport report;

	private int currentPrintPage;

	private StiPageViewMode pageViewMode = StiPageViewMode.MultiplePages;

	private double zoom = 1.0;

	private bool showPrint = true;

	private bool showOpen = true;

	private bool showSave = true;

	private bool showSendEMail = true;

	private bool showBookmarksPanel = true;

	private bool showParametersButton = true;

	private bool showResourcesButton = true;

	private bool showThumbsPanel = true;

	private bool showEditor = true;

	private bool showFind = true;

	private bool showFullScreen = true;

	private bool showZoomOnePage = true;

	private bool showZoomTwoPages = true;

	private bool showZoomMultiplePages = true;

	private bool showZoomPageWidth = true;

	private bool showSliderZoom = true;

	private bool showPageViewMode = true;

	private bool showViewModeContinuous = true;

	private bool showViewModeMultiplePages = true;

	private bool showDotMatrixModeButton = true;

	private bool showPageControl = true;

	private bool showFirstPage = true;

	private bool showPreviousPage = true;

	private bool showNextPage = true;

	private bool showLastPage = true;

	private bool showPageNew = true;

	private bool showPageDelete = true;

	private bool showPageDesign = true;

	private bool showPageSize = true;

	private bool showToolbar = true;

	private bool showHorScrollBar = true;

	private bool showVertScrollBar = true;

	private bool showStatusBar = true;

	private bool showContextMenu = true;

	private bool showCloseButton;

	private static Cursor cursorHand;

	private static Cursor cursorHandMove;

	private Hashtable storedInteractionComponentStyles;

	private Hashtable storedInteractionComponentSelected;

	[Browsable(false)]
	public ToolTip ToolTip => toolTip;

	private double ScaleX => StiScale.X;

	private double ScaleY => StiScale.Y;

	bool IStiViewerAccessor.BlockFormClose { get; set; }

	[Category("Report")]
	public StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			StiMathFormulaCache.Clear();
			StiMapDrawingCache.Clear();
			if (report == value)
			{
				return;
			}
			if (((BaseItem)tbParameters).Visible)
			{
				((BaseItem)tbParameters).Enabled = false;
				tbParameters.Checked = false;
			}
			if (!lockRefreshReports)
			{
				Reports.Clear();
				((Control)tabStripReports).Visible = false;
				((CollectionBase)(object)tabStripReports.Tabs).Clear();
				mainReport = null;
				Parameters.CleanAllVariableStates();
			}
			Clear();
			LoadedReportFileName = null;
			if (report != null)
			{
				StopRefreshReportTimer();
				report.RefreshViewer -= Report_RefreshViewer;
				report.Paint -= Report_Paint;
				report.GotoComp -= Report_GotoComponent;
				if (report.CompiledReport != null)
				{
					report.CompiledReport.RefreshViewer -= Report_RefreshViewer;
					report.Paint -= Report_Paint;
					report.GotoComp -= Report_GotoComponent;
				}
				if (report.InteractionCollapsingStates != null)
				{
					report.InteractionCollapsingStates.Clear();
					report.InteractionCollapsingStates = null;
				}
				StiViewerFitTextHelper.RemoveReport(report);
			}
			if (value != null)
			{
				value.RefreshViewer += Report_RefreshViewer;
				value.Paint += Report_Paint;
				value.GotoComp += Report_GotoComponent;
				if (value.CompiledReport != null)
				{
					value.CompiledReport.RefreshViewer += Report_RefreshViewer;
					value.Paint += Report_Paint;
					value.GotoComp += Report_GotoComponent;
				}
				StiViewerFitTextHelper.AddReport(value);
			}
			lbThumbs.SelectedIndex = 0;
			CurrentPrintPage = 0;
			if (report != null)
			{
				report.Info.ViewMode = restoreViewMode;
				report.Info.Zoom = restoreZoom;
			}
			report = value;
			if (value != null)
			{
				restoreViewMode = value.Info.ViewMode;
				restoreZoom = value.Info.Zoom;
				value.Info.Zoom = Zoom;
				if (value.CompiledReport != null)
				{
					value.CompiledReport.Info.Zoom = Zoom;
				}
				if (mainReport == null)
				{
					mainReport = Report;
					((Control)tabStripReports).Visible = false;
				}
				BuildResourcesList();
				if (report.RefreshTime > 0)
				{
					StartRefreshReportTimer(report.RefreshTime);
				}
				view.AnimationEngine.Start(isFirstStart: true);
			}
			IsFindActivated = false;
			((Control)tbTool).Invalidate();
			RefreshInteractions();
			ignoreParametersRefreshingOnce = true;
			Parameters.RefreshDialogs();
			((Control)this).Refresh();
			ignoreParametersRefreshingOnce = false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<StiReport> Reports { get; } = new List<StiReport>();


	internal SuperTooltip SuperToolTip { get; set; } = new SuperTooltip();


	internal eTooltipColor CurrentTooltipColor { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal ArrayList PageParams { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal IStiProgressInformation Progress { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal StiTextEditor TextEditor { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string LoadedReportFileName { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool HideDisabledPagesBeforeExport { get; set; }

	private int DistancePage => Math.Max(StiScale.I5, StiScale.I(10.0 * ((double)((Control)view).Width / GetMaxPageWidth()) * ((Report != null) ? Report.Info.Zoom : 1.0)));

	[Browsable(false)]
	public Panel PanelDialogs => panelDialogs;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IgnoreApplyStyle { get; set; }

	[DefaultValue("Viewer")]
	[Category("Parameters")]
	public string CategoryInConfiguration { get; set; } = "Viewer";


	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool IsDesignMode => ((Component)this).DesignMode;

	private StiPreviewSettings Settings
	{
		get
		{
			if (CurrentReport == null)
			{
				return StiPreviewSettings.Default;
			}
			return (StiPreviewSettings)CurrentReport.PreviewSettings;
		}
	}

	internal StiParametersAccessor Parameters { get; set; }

	private StiViewerConfigService Config { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal int CurrentPrintPage
	{
		get
		{
			if (CurrentReport == null)
			{
				return 0;
			}
			return CurrentReport.CurrentPrintPage;
		}
		set
		{
			if (CurrentReport == null)
			{
				return;
			}
			CurrentReport.CurrentPrintPage = value;
			if (value >= 0 && PageParams != null)
			{
				if (value != lbThumbs.SelectedIndex && ((Control)lbThumbs).Visible && CurrentReport != null && CurrentReport.RenderedPages.Count > 0)
				{
					lbThumbs.SelectedIndex = value;
				}
				if (pageViewMode == StiPageViewMode.SinglePage)
				{
					UpdateScrollBarsInSingleMode();
				}
				ScrollToCurPage();
			}
			if (currentPrintPage != value)
			{
				OnCurrentPageChanged(EventArgs.Empty);
				currentPrintPage = value;
			}
			UpdateControlsState();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int CurrentPage
	{
		get
		{
			if (CurrentReport == null)
			{
				return 0;
			}
			int startPageNumber = 0;
			int endPageNumber = 0;
			CurrentReport.GetPageNumber(CurrentPrintPage, out startPageNumber, out endPageNumber);
			return startPageNumber;
		}
		set
		{
			int num = ((CurrentReport == null) ? 1 : CurrentReport.GetTotalRenderedPageCount());
			if (value < 1)
			{
				value = 1;
			}
			if (value > num)
			{
				value = num;
			}
			int num2 = 1;
			int num3 = 0;
			if (CurrentReport != null)
			{
				foreach (StiPage renderedPage in CurrentReport.RenderedPages)
				{
					if (num2 >= value)
					{
						break;
					}
					num2 += renderedPage.SegmentPerWidth * renderedPage.SegmentPerHeight;
					num3++;
				}
			}
			CurrentPrintPage = num3;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int TotalPageCount
	{
		get
		{
			if (CurrentReport == null)
			{
				return 0;
			}
			return CurrentReport.GetTotalRenderedPageCount();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPreviewSettings PreviewSettings
	{
		get
		{
			StiPreviewSettings stiPreviewSettings = StiPreviewSettings.None;
			if (ShowPageViewMode)
			{
				stiPreviewSettings |= StiPreviewSettings.PageViewMode;
			}
			if (ShowVertScrollBar)
			{
				stiPreviewSettings |= StiPreviewSettings.VertScrollBar;
			}
			if (ShowHorScrollBar)
			{
				stiPreviewSettings |= StiPreviewSettings.HorScrollBar;
			}
			if (ShowPrint)
			{
				stiPreviewSettings |= StiPreviewSettings.Print;
			}
			if (ShowOpen)
			{
				stiPreviewSettings |= StiPreviewSettings.Open;
			}
			if (ShowSave)
			{
				stiPreviewSettings |= StiPreviewSettings.Save;
			}
			if (ShowCloseButton)
			{
				stiPreviewSettings |= StiPreviewSettings.Close;
			}
			if (ShowSendEMail)
			{
				stiPreviewSettings |= StiPreviewSettings.SendEMail;
			}
			if (ShowPageNew)
			{
				stiPreviewSettings |= StiPreviewSettings.PageNew;
			}
			if (ShowPageDelete)
			{
				stiPreviewSettings |= StiPreviewSettings.PageDelete;
			}
			if (ShowPageDesign)
			{
				stiPreviewSettings |= StiPreviewSettings.PageDesign;
			}
			if (ShowPageSize)
			{
				stiPreviewSettings |= StiPreviewSettings.PageSize;
			}
			if (ShowEditor)
			{
				stiPreviewSettings |= StiPreviewSettings.EditorTool;
			}
			if (ShowFind)
			{
				stiPreviewSettings |= StiPreviewSettings.FindTool;
			}
			if (ShowZoom)
			{
				stiPreviewSettings |= StiPreviewSettings.Zoom;
			}
			if (ShowPageControl)
			{
				stiPreviewSettings |= StiPreviewSettings.PageControl;
			}
			if (ShowBookmarksPanel)
			{
				stiPreviewSettings |= StiPreviewSettings.Bookmarks;
			}
			if (ShowThumbsPanel)
			{
				stiPreviewSettings |= StiPreviewSettings.Thumbs;
			}
			if (ShowContextMenu)
			{
				stiPreviewSettings |= StiPreviewSettings.ContextMenu;
			}
			if (ShowToolbar)
			{
				stiPreviewSettings |= StiPreviewSettings.Toolbar;
			}
			if (ShowParametersButton)
			{
				stiPreviewSettings |= StiPreviewSettings.Parameters;
			}
			if (ShowResourcesButton)
			{
				stiPreviewSettings |= StiPreviewSettings.Resources;
			}
			if (ShowStatusBar)
			{
				stiPreviewSettings |= StiPreviewSettings.StatusBar;
			}
			return stiPreviewSettings;
		}
		set
		{
			showPageViewMode = (value & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None;
			showVertScrollBar = (value & StiPreviewSettings.VertScrollBar) > StiPreviewSettings.None;
			showHorScrollBar = (value & StiPreviewSettings.HorScrollBar) > StiPreviewSettings.None;
			showPrint = (value & StiPreviewSettings.Print) > StiPreviewSettings.None;
			showOpen = (value & StiPreviewSettings.Open) > StiPreviewSettings.None;
			showSave = (value & StiPreviewSettings.Save) > StiPreviewSettings.None;
			showCloseButton = (value & StiPreviewSettings.Close) > StiPreviewSettings.None;
			showSendEMail = (value & StiPreviewSettings.SendEMail) > StiPreviewSettings.None;
			showPageNew = (value & StiPreviewSettings.PageNew) > StiPreviewSettings.None;
			showPageDelete = (value & StiPreviewSettings.PageDelete) > StiPreviewSettings.None;
			showPageDesign = (value & StiPreviewSettings.PageDesign) > StiPreviewSettings.None;
			showPageSize = (value & StiPreviewSettings.PageSize) > StiPreviewSettings.None;
			showEditor = (value & StiPreviewSettings.EditorTool) > StiPreviewSettings.None;
			showFind = (value & StiPreviewSettings.FindTool) > StiPreviewSettings.None;
			showPageControl = (value & StiPreviewSettings.PageControl) > StiPreviewSettings.None;
			showBookmarksPanel = (value & StiPreviewSettings.Bookmarks) > StiPreviewSettings.None;
			showThumbsPanel = (value & StiPreviewSettings.Thumbs) > StiPreviewSettings.None;
			showContextMenu = (value & StiPreviewSettings.ContextMenu) > StiPreviewSettings.None;
			showToolbar = (value & StiPreviewSettings.Toolbar) > StiPreviewSettings.None;
			showParametersButton = (value & StiPreviewSettings.Parameters) > StiPreviewSettings.None;
			showResourcesButton = (value & StiPreviewSettings.Resources) > StiPreviewSettings.None;
			showStatusBar = (value & StiPreviewSettings.StatusBar) > StiPreviewSettings.None;
		}
	}

	[Category("Parameters")]
	[DefaultValue(false)]
	[Browsable(false)]
	public bool FocusOnLoad { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IgnoreEscapeKey { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string FindValue
	{
		get
		{
			return ((Control)tbFindValue).Text;
		}
		set
		{
			((Control)tbFindValue).Text = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(StiPageViewMode.MultiplePages)]
	[Browsable(false)]
	public StiPageViewMode PageViewMode
	{
		get
		{
			return pageViewMode;
		}
		set
		{
			if (pageViewMode != value)
			{
				pageViewMode = value;
				tbViewModeSinglePage.Checked = pageViewMode == StiPageViewMode.SinglePage;
				tbViewModeContinuous.Checked = pageViewMode == StiPageViewMode.Continuous;
				tbViewModeMultiplePages.Checked = pageViewMode == StiPageViewMode.MultiplePages;
				InvokePageViewModeChanged();
			}
		}
	}

	internal StiReport CurrentReport
	{
		get
		{
			if (report == null)
			{
				return null;
			}
			if (report.CompiledReport != null)
			{
				return report.CompiledReport;
			}
			return report;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(StiGuiStandardStyle.Office2007)]
	[Obsolete("Please use StiOptions.Windows.GlobalGuiStyle instead Style property of StiViewerControl!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiGuiStandardStyle Style
	{
		get
		{
			return StiGuiStandardStyle.Default;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public double Zoom
	{
		get
		{
			return zoom;
		}
		set
		{
			if (zoom != value)
			{
				zoom = value;
				InvokeZoomChanged();
			}
			if (CurrentReport != null)
			{
				CurrentReport.Info.Zoom = value;
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPrint
	{
		get
		{
			return showPrint;
		}
		set
		{
			if (showPrint != value)
			{
				showPrint = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowOpen
	{
		get
		{
			return showOpen;
		}
		set
		{
			if (showOpen != value)
			{
				showOpen = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowSave
	{
		get
		{
			return showSave;
		}
		set
		{
			if (showSave != value)
			{
				showSave = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	[Obsolete("Please use ShowSave property instead ShowExport property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowExport
	{
		get
		{
			return ShowSave;
		}
		set
		{
			ShowSave = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowSendEMail
	{
		get
		{
			return showSendEMail;
		}
		set
		{
			if (showSendEMail != value)
			{
				showSendEMail = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowSaveDocumentFile { get; set; } = true;


	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowSendEMailDocumentFile { get; set; } = true;


	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowBookmarksPanel
	{
		get
		{
			return showBookmarksPanel;
		}
		set
		{
			if (showBookmarksPanel != value)
			{
				showBookmarksPanel = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowParametersButton
	{
		get
		{
			return showParametersButton;
		}
		set
		{
			if (showParametersButton != value)
			{
				showParametersButton = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowResourcesButton
	{
		get
		{
			return showResourcesButton;
		}
		set
		{
			if (showResourcesButton != value)
			{
				showResourcesButton = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowThumbsPanel
	{
		get
		{
			return showThumbsPanel;
		}
		set
		{
			if (showThumbsPanel != value)
			{
				showThumbsPanel = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	public bool ThumbsPanelEnabled
	{
		get
		{
			return ShowThumbsPanel;
		}
		set
		{
			ShowThumbsPanel = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	[Obsolete("'ShowSelectTool' property is obsolete. Report Viewer does not use more Select Tool.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowSelectTool { get; set; } = true;


	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	[Obsolete("'ShowHandTool' property is obsolete. Report Viewer does not use more Hand Tool.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowHandTool { get; set; } = true;


	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	[Obsolete("Please use ShowEditor property instead ShowEditorTool property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowEditorTool
	{
		get
		{
			return ShowEditor;
		}
		set
		{
			ShowEditor = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowEditor
	{
		get
		{
			return showEditor;
		}
		set
		{
			if (showEditor != value)
			{
				showEditor = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	[Obsolete("Please use ShowFind property instead ShowFindTool property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFindTool
	{
		get
		{
			return ShowFind;
		}
		set
		{
			ShowFind = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowFind
	{
		get
		{
			return showFind;
		}
		set
		{
			if (showFind != value)
			{
				showFind = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowFullScreen
	{
		get
		{
			return showFullScreen;
		}
		set
		{
			if (showFullScreen != value)
			{
				showFullScreen = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowZoomOnePage
	{
		get
		{
			return showZoomOnePage;
		}
		set
		{
			if (showZoomOnePage != value)
			{
				showZoomOnePage = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowZoomTwoPages
	{
		get
		{
			return showZoomTwoPages;
		}
		set
		{
			if (showZoomTwoPages != value)
			{
				showZoomTwoPages = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowZoomMultiplePages
	{
		get
		{
			return showZoomMultiplePages;
		}
		set
		{
			if (showZoomMultiplePages != value)
			{
				showZoomMultiplePages = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowZoomPageWidth
	{
		get
		{
			return showZoomPageWidth;
		}
		set
		{
			if (showZoomPageWidth != value)
			{
				showZoomPageWidth = value;
				UpdateControlsState();
			}
		}
	}

	[Browsable(false)]
	public bool ShowZoom
	{
		get
		{
			return ShowSliderZoom;
		}
		set
		{
			ShowSliderZoom = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	[Obsolete("Please use property ShowSliderZoom.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowComboBoxZoom
	{
		get
		{
			return ShowSliderZoom;
		}
		set
		{
			ShowSliderZoom = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowSliderZoom
	{
		get
		{
			return showSliderZoom;
		}
		set
		{
			if (showSliderZoom != value)
			{
				showSliderZoom = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	[Obsolete("Button 'ZoomIn' is not used more in Report Viewer.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowZoomIn { get; set; } = true;


	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	[Obsolete("Button 'ZoomOut' is not used more in Report Viewer.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowZoomOut { get; set; } = true;


	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowViewModeSinglePage
	{
		get
		{
			return showPageViewMode;
		}
		set
		{
			if (showPageViewMode != value)
			{
				showPageViewMode = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowViewModeContinuous
	{
		get
		{
			return showViewModeContinuous;
		}
		set
		{
			if (showViewModeContinuous != value)
			{
				showViewModeContinuous = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowViewModeMultiplePages
	{
		get
		{
			return showViewModeMultiplePages;
		}
		set
		{
			if (showViewModeMultiplePages != value)
			{
				showViewModeMultiplePages = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	[Browsable(false)]
	public bool ShowPageViewMode
	{
		get
		{
			return ShowViewModeSinglePage;
		}
		set
		{
			ShowZoomOnePage = value;
			ShowZoomTwoPages = value;
			ShowZoomMultiplePages = value;
			ShowZoomPageWidth = value;
			ShowFullScreen = value;
			ShowViewModeSinglePage = value;
			ShowViewModeContinuous = value;
			ShowViewModeMultiplePages = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowDotMatrixModeButton
	{
		get
		{
			return showDotMatrixModeButton;
		}
		set
		{
			if (showDotMatrixModeButton != value)
			{
				showDotMatrixModeButton = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPageControl
	{
		get
		{
			return showPageControl;
		}
		set
		{
			if (showPageControl != value)
			{
				showPageControl = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowFirstPage
	{
		get
		{
			return showFirstPage;
		}
		set
		{
			if (showFirstPage != value)
			{
				showFirstPage = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPreviousPage
	{
		get
		{
			return showPreviousPage;
		}
		set
		{
			if (showPreviousPage != value)
			{
				showPreviousPage = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowNextPage
	{
		get
		{
			return showNextPage;
		}
		set
		{
			if (showNextPage != value)
			{
				showNextPage = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowLastPage
	{
		get
		{
			return showLastPage;
		}
		set
		{
			if (showLastPage != value)
			{
				showLastPage = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPageNew
	{
		get
		{
			return showPageNew;
		}
		set
		{
			if (showPageNew != value)
			{
				showPageNew = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPageDelete
	{
		get
		{
			return showPageDelete;
		}
		set
		{
			if (showPageDelete != value)
			{
				showPageDelete = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPageDesign
	{
		get
		{
			return showPageDesign;
		}
		set
		{
			if (showPageDesign != value)
			{
				showPageDesign = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPageSize
	{
		get
		{
			return showPageSize;
		}
		set
		{
			if (showPageSize != value)
			{
				showPageSize = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public virtual bool ShowToolbar
	{
		get
		{
			return showToolbar;
		}
		set
		{
			if (showToolbar != value)
			{
				showToolbar = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowHorScrollBar
	{
		get
		{
			return showHorScrollBar;
		}
		set
		{
			if (showHorScrollBar != value)
			{
				showHorScrollBar = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowVertScrollBar
	{
		get
		{
			return showVertScrollBar;
		}
		set
		{
			if (showVertScrollBar != value)
			{
				showVertScrollBar = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowStatusBar
	{
		get
		{
			return showStatusBar;
		}
		set
		{
			if (showStatusBar != value)
			{
				showStatusBar = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowContextMenu
	{
		get
		{
			return showContextMenu;
		}
		set
		{
			if (showContextMenu != value)
			{
				showContextMenu = value;
				UpdateControlsState();
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(false)]
	public bool ShowCloseButton
	{
		get
		{
			return showCloseButton;
		}
		set
		{
			if (showCloseButton != value)
			{
				showCloseButton = value;
				UpdateControlsState();
			}
		}
	}

	[Browsable(false)]
	public bool GetPrintVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPrint;
			}
			if (Config != null && Config.PrintEnabled.HasValue)
			{
				return Config.PrintEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Print) > StiPreviewSettings.None)
			{
				return ShowPrint;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetOpenVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowOpen;
			}
			if (CurrentReport != null && CurrentReport.RenderedPages.CacheMode)
			{
				return false;
			}
			if (Config != null && Config.OpenEnabled.HasValue)
			{
				return Config.OpenEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Open) > StiPreviewSettings.None)
			{
				return ShowOpen;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetSaveVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowSave;
			}
			if (Config != null && Config.SaveEnabled.HasValue)
			{
				return Config.SaveEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Save) > StiPreviewSettings.None)
			{
				return ShowSave;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetSendEMailVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowSendEMail;
			}
			if (Config != null && Config.SendEMailEnabled.HasValue)
			{
				return Config.SendEMailEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.SendEMail) > StiPreviewSettings.None)
			{
				return ShowSendEMail;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetSaveDocumentFileVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowSaveDocumentFile;
			}
			if (CurrentReport != null && CurrentReport.RenderedPages.CacheMode)
			{
				return false;
			}
			if (Config != null && Config.SaveDocumentFileEnabled.HasValue)
			{
				return Config.SaveDocumentFileEnabled.GetValueOrDefault(true);
			}
			return ShowSaveDocumentFile;
		}
	}

	[Browsable(false)]
	public bool GetSendEMailDocumentFileVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowSendEMailDocumentFile;
			}
			if (CurrentReport != null && CurrentReport.RenderedPages.CacheMode)
			{
				return false;
			}
			if (Config != null && Config.SendEMailDocumentFileEnabled.HasValue)
			{
				return Config.SendEMailDocumentFileEnabled.GetValueOrDefault(true);
			}
			return ShowSendEMailDocumentFile;
		}
	}

	[Browsable(false)]
	public bool GetBookmarksPanelVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowBookmarksPanel;
			}
			if (StiOptions.Viewer.Windows.ShowBookmarksPanel.HasValue)
			{
				return StiOptions.Viewer.Windows.ShowBookmarksPanel.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Bookmarks) > StiPreviewSettings.None)
			{
				return ShowBookmarksPanel;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetParametersButtonVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowParametersButton;
			}
			if (StiOptions.Viewer.Windows.ShowParametersButton.HasValue)
			{
				return StiOptions.Viewer.Windows.ShowParametersButton.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Parameters) > StiPreviewSettings.None)
			{
				return ShowParametersButton;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetResourcesButtonVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowResourcesButton;
			}
			if (StiOptions.Viewer.Windows.ShowResourcesButton.HasValue)
			{
				return StiOptions.Viewer.Windows.ShowResourcesButton.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Resources) > StiPreviewSettings.None)
			{
				return ShowResourcesButton;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetThumbsPanelVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowThumbsPanel;
			}
			if (StiOptions.Viewer.Windows.ShowThumbsPanel.HasValue)
			{
				return StiOptions.Viewer.Windows.ShowThumbsPanel.GetValueOrDefault(true);
			}
			if (StiOptions.Viewer.Windows.ShowThumbsButton.HasValue)
			{
				return StiOptions.Viewer.Windows.ShowThumbsButton.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Thumbs) > StiPreviewSettings.None)
			{
				return ShowThumbsPanel;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetEditorVisibility
	{
		get
		{
			if (Config != null && Config.ToolEditorEnabled.HasValue)
			{
				return Config.ToolEditorEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.EditorTool) > StiPreviewSettings.None && ShowEditor)
			{
				return GetEditorEnabled;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetEditorVisibilityInRibbonMode
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowEditor;
			}
			if (Config != null && Config.ToolEditorEnabled.HasValue)
			{
				return Config.ToolEditorEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.EditorTool) > StiPreviewSettings.None && ShowEditor)
			{
				return GetEditorEnabled;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetFindVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowFind;
			}
			if (Config != null && Config.ToolFindEnabled.HasValue)
			{
				return Config.ToolFindEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.FindTool) > StiPreviewSettings.None)
			{
				return ShowFind;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetFullScreenVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowFullScreen;
			}
			if (Config != null && Config.FullScreenEnabled.HasValue)
			{
				return Config.FullScreenEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None)
			{
				return ShowFullScreen;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoomOnePageVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowZoomOnePage;
			}
			if (Config != null && Config.ZoomEnabled.HasValue)
			{
				return Config.ZoomEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None)
			{
				return ShowZoomOnePage;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoomTwoPagesVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowZoomTwoPages;
			}
			if (Config != null && Config.ZoomEnabled.HasValue)
			{
				return Config.ZoomEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None && ShowZoomTwoPages)
			{
				return pageViewMode == StiPageViewMode.MultiplePages;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoomMultiplePagesVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowZoomMultiplePages;
			}
			if (Config != null && Config.ZoomEnabled.HasValue)
			{
				return Config.ZoomEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None && ShowZoomMultiplePages)
			{
				return pageViewMode == StiPageViewMode.MultiplePages;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoomPageWidthVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowZoomPageWidth;
			}
			if (Config != null && Config.ZoomEnabled.HasValue)
			{
				return Config.ZoomEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None)
			{
				return ShowZoomPageWidth;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetSliderZoomVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowSliderZoom;
			}
			if (Config != null && Config.ZoomEnabled.HasValue)
			{
				return Config.ZoomEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Zoom) > StiPreviewSettings.None)
			{
				return ShowSliderZoom;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetViewModeSinglePageVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageViewMode;
			}
			if (Config != null && Config.PageViewModeEnabled.HasValue)
			{
				return Config.PageViewModeEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None)
			{
				return ShowPageViewMode;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetViewModeContinuousVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageViewMode;
			}
			if (Config != null && Config.PageViewModeEnabled.HasValue)
			{
				return Config.PageViewModeEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None)
			{
				return ShowPageViewMode;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetViewModeMultiplePagesVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageViewMode;
			}
			if (Config != null && Config.PageViewModeEnabled.HasValue)
			{
				return Config.PageViewModeEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None)
			{
				return ShowPageViewMode;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetDotMatrixModeButtonVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowDotMatrixModeButton;
			}
			if (CurrentReport != null && (CurrentReport.PreviewMode == StiPreviewMode.DotMatrix || CurrentReport.PreviewMode == StiPreviewMode.StandardAndDotMatrix))
			{
				return ShowDotMatrixModeButton;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetFirstPageVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowFirstPage;
			}
			if (Config != null && Config.FirstPageEnabled.HasValue)
			{
				return Config.FirstPageEnabled.GetValueOrDefault(true);
			}
			return ShowFirstPage;
		}
	}

	[Browsable(false)]
	public bool GetPreviousPageVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPreviousPage;
			}
			if (Config != null && Config.PreviousPageEnabled.HasValue)
			{
				return Config.PreviousPageEnabled.GetValueOrDefault(true);
			}
			return ShowPreviousPage;
		}
	}

	[Browsable(false)]
	public bool GetPageControlVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageControl;
			}
			if (Config != null && Config.PageControlEnabled.HasValue)
			{
				return Config.PageControlEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageControl) > StiPreviewSettings.None)
			{
				return ShowPageControl;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetNextPageVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowNextPage;
			}
			if (Config != null && Config.NextPageEnabled.HasValue)
			{
				return Config.NextPageEnabled.GetValueOrDefault(true);
			}
			return ShowNextPage;
		}
	}

	[Browsable(false)]
	public bool GetLastPageVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowLastPage;
			}
			if (Config != null && Config.LastPageEnabled.HasValue)
			{
				return Config.LastPageEnabled.GetValueOrDefault(true);
			}
			return ShowLastPage;
		}
	}

	[Browsable(false)]
	public bool GetPageNewVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageNew;
			}
			if (CurrentReport != null && CurrentReport.RenderedPages.CacheMode)
			{
				return false;
			}
			if (Config != null && Config.PageNewEnabled.HasValue)
			{
				return Config.PageNewEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageNew) > StiPreviewSettings.None)
			{
				return ShowPageNew;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetPageDeleteVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageDelete;
			}
			if (CurrentReport != null && CurrentReport.RenderedPages.CacheMode)
			{
				return false;
			}
			if (Config != null && Config.PageDeleteEnabled.HasValue)
			{
				return Config.PageDeleteEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageDelete) > StiPreviewSettings.None)
			{
				return ShowPageDelete;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetPageDesignVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageDesign;
			}
			if (CurrentReport != null && CurrentReport.RenderedPages.CacheMode)
			{
				return false;
			}
			if (Config != null && Config.PageDesignEnabled.HasValue)
			{
				return Config.PageDesignEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageDesign) > StiPreviewSettings.None)
			{
				return ShowPageDesign;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetPageSizeVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowPageSize;
			}
			if (CurrentReport != null && CurrentReport.RenderedPages.CacheMode)
			{
				return false;
			}
			if (Config != null && Config.PageSizeEnabled.HasValue)
			{
				return Config.PageSizeEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.PageSize) > StiPreviewSettings.None)
			{
				return ShowPageSize;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetCloseVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowCloseButton;
			}
			if (Config != null && Config.CloseEnabled.HasValue)
			{
				return Config.CloseEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Close) <= StiPreviewSettings.None)
			{
				return ShowCloseButton;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetHorScrollBarVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowHorScrollBar;
			}
			if ((Settings & StiPreviewSettings.HorScrollBar) > StiPreviewSettings.None && isHorizontalScrollBarVisible)
			{
				return ShowHorScrollBar;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetVertScrollBarVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowVertScrollBar;
			}
			if ((Settings & StiPreviewSettings.VertScrollBar) > StiPreviewSettings.None)
			{
				return ShowVertScrollBar;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetToolbarVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowToolbar;
			}
			if (Config != null && Config.ToolbarEnabled.HasValue)
			{
				return Config.ToolbarEnabled.GetValueOrDefault(true);
			}
			if ((Settings & StiPreviewSettings.Toolbar) > StiPreviewSettings.None)
			{
				return ShowToolbar;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetStatusBarVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowStatusBar;
			}
			if ((Settings & StiPreviewSettings.StatusBar) > StiPreviewSettings.None)
			{
				return ShowStatusBar;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetContextMenuVisibility
	{
		get
		{
			if (IsDesignMode)
			{
				return ShowContextMenu;
			}
			if ((Settings & StiPreviewSettings.ContextMenu) > StiPreviewSettings.None)
			{
				return ShowContextMenu;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetPrintEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetOpenEnabled => true;

	[Browsable(false)]
	public bool GetSaveEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetExportEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetSendEMailEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetBookmarksPanelEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetThumbsPanelEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetParametersPanelEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return ((ArrangedElementCollection)((Control)panelDialogs).Controls).Count > 0;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetResourcesPanelEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return ((ArrangedElementCollection)((Control)listResources).Controls).Count > 0;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetEditorEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null)
				{
					return isEditableComponentsPresent;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetEditorEnabledInRibbon
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null)
				{
					return isEditableComponentsPresent;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetFindEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetFullScreenEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetZoomOnePageEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetZoomTwoPagesEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetZoomMultiplePagesEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetZoomPageWidthEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetSliderZoomEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetViewModeSinglePageEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetViewModeContinuousEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetViewModeMultiplePagesEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetDotMatrixModeButtonEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetFirstPageEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null && PageParams != null && PageParams.Count > 1)
				{
					return CurrentPrintPage > 0;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetPreviousPageEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null && PageParams != null && PageParams.Count > 1)
				{
					return CurrentPrintPage > 0;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetPageControlEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetNextPageEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null && PageParams != null && PageParams.Count > 1)
				{
					return CurrentPrintPage < PageParams.Count - 1;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetLastPageEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null && PageParams != null && PageParams.Count > 1)
				{
					return CurrentPrintPage < PageParams.Count - 1;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetPageNewEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetPageDeleteEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null && PageParams != null)
				{
					return PageParams.Count > 1;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetPageDesignEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null && PageParams != null)
				{
					return PageParams.Count > 0;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetPageSizeEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				if (CurrentReport != null && PageParams != null)
				{
					return PageParams.Count > 0;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetCloseEnabled => true;

	[Browsable(false)]
	public bool GetHorScrollBarEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetVertScrollBarEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetContextMenuEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetFindBarEnabled
	{
		get
		{
			if (!IsDesignMode)
			{
				return CurrentReport != null;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool GetEditorChecked
	{
		get
		{
			if (CurrentReport != null)
			{
				return IsEditorActivated;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetFindChecked
	{
		get
		{
			if (CurrentReport != null)
			{
				return IsFindActivated;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoomOnePageChecked
	{
		get
		{
			if (CurrentReport != null)
			{
				return isZoomOnePage;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoomTwoPagesChecked
	{
		get
		{
			if (CurrentReport != null)
			{
				return isZoomTwoPages;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoomMultiplePagesChecked => false;

	[Browsable(false)]
	public bool GetZoomPageWidthChecked
	{
		get
		{
			if (CurrentReport != null)
			{
				return isZoomPageWidth;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoom10Checked
	{
		get
		{
			if (CurrentReport != null)
			{
				return Zoom == 0.1;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoom25Checked
	{
		get
		{
			if (CurrentReport != null)
			{
				return Zoom == 0.25;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoom50Checked
	{
		get
		{
			if (CurrentReport != null)
			{
				return Zoom == 0.5;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoom75Checked
	{
		get
		{
			if (CurrentReport != null)
			{
				return Zoom == 0.75;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoom100Checked
	{
		get
		{
			if (CurrentReport != null)
			{
				return Zoom == 1.0;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoom150Checked
	{
		get
		{
			if (CurrentReport != null)
			{
				return Zoom == 1.5;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetZoom200Checked
	{
		get
		{
			if (CurrentReport != null)
			{
				return Zoom == 2.0;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool GetBookmarksButtonChecked => tbBookmarks.Checked;

	[Browsable(false)]
	public bool GetThumbsButtonChecked => tbThumbs.Checked;

	[Browsable(false)]
	public bool GetParametersButtonChecked => tbParameters.Checked;

	[Browsable(false)]
	public bool GetResourcesButtonChecked => ((Control)listResources).Visible;

	[Category("Parameters")]
	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsEditorActivated
	{
		get
		{
			if (tbEditor.Checked && ((BaseItem)tbEditor).Visible)
			{
				return ((BaseItem)tbEditor).Enabled;
			}
			return false;
		}
		set
		{
			if (tbEditor.Checked != value)
			{
				tbEditor.Checked = value;
				if (!value)
				{
					CloseEditor();
					StiOptions.Engine.GlobalEvents.InvokeDisableEditorMode(CurrentReport, EventArgs.Empty);
				}
				else
				{
					TextEditor.EnableEditorTool();
					StiOptions.Engine.GlobalEvents.InvokeEnableEditorMode(CurrentReport, EventArgs.Empty);
				}
			}
		}
	}

	[Category("Parameters")]
	[DefaultValue(false)]
	[Browsable(false)]
	public virtual bool IsFindActivated
	{
		get
		{
			return tbFind.Checked;
		}
		set
		{
			if (tbFind.Checked == value || (CurrentReport == null && value))
			{
				return;
			}
			tbFind.Checked = value;
			tbFindBar.Visible = tbFind.Checked;
			tbFindBar.PaddingTop = StiScale.I2;
			((Control)tbFindBar).Height = StiScale.YYI(29.0);
			if (!value)
			{
				((Control)tbFindValue).Text = "";
				if (CurrentReport != null)
				{
					findHelper.CloseFind(CurrentReport);
				}
				InvokeFindClose();
			}
			else
			{
				((Control)tbFindValue).Focus();
			}
			SetZoomByDefault();
		}
	}

	[Category("Parameters")]
	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsHandModeActivated { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Bar ToolBar => tbTool;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Bar StatusBar => tbStatusBar;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Bar FindBar => tbFindBar;

	[Browsable(false)]
	public StiPageViewControl View => view;

	[Browsable(false)]
	public TreeView BookmarksTreeView => (TreeView)(object)tvBookmarks;

	[Browsable(false)]
	public StiVScrollBar VScrollBar => vScrollBar;

	[Browsable(false)]
	public StiHScrollBar HScrollBar => hScrollBar;

	public static Cursor CursorHand
	{
		get
		{
			if (StiScale.Factor != 1.0)
			{
				return Cursors.Hand;
			}
			if (cursorHand == (Cursor)null)
			{
				cursorHand = StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.Hand.cur");
			}
			return cursorHand;
		}
	}

	public static Cursor CursorHandMove
	{
		get
		{
			if (StiScale.Factor > 2.0)
			{
				return Cursors.Hand;
			}
			if (cursorHandMove != (Cursor)null)
			{
				return cursorHandMove;
			}
			if (cursorHandMove == (Cursor)null)
			{
				cursorHandMove = ((StiScale.Factor == 1.0) ? StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.HandMove.cur") : StiCursorUtils.GetCursor("Stimulsoft.Report", "Stimulsoft.Report.Cursors.HandMove_x2.cur"));
			}
			return cursorHandMove;
		}
	}

	public IStiReportTabHelper ReportTabHelper { get; set; }

	public event EventHandler FindClose;

	public event EventHandler PageViewModeChanged;

	[Category("PreviewControl")]
	public event EventHandler RefreshControlState;

	[Category("PreviewControl")]
	public event EventHandler Close;

	[Category("PreviewControl")]
	public event StiCloseDrillDownPageEventHandler CloseDrillDownPage;

	[Category("PreviewControl")]
	public event EventHandler ClickPrintButton;

	[Category("PreviewControl")]
	public event EventHandler ClickOpenButton;

	[Category("PreviewControl")]
	public event EventHandler ClickSaveButton;

	[Category("PreviewControl")]
	public event EventHandler ClickSaveSendEMailDocumentButton;

	[Category("PreviewControl")]
	public event EventHandler ClickExportButton;

	[Category("PreviewControl")]
	public event EventHandler ClickSendEMailButton;

	[Category("PreviewControl")]
	public event EventHandler ClickPageNewButton;

	[Category("PreviewControl")]
	public event EventHandler ClickPageDeleteButton;

	[Category("PreviewControl")]
	public event EventHandler ClickPageDesignButton;

	[Category("PreviewControl")]
	public event EventHandler ClickPageSizeButton;

	[Category("PreviewControl")]
	public event EventHandler CurrentPageChanged;

	[Category("PreviewControl")]
	public event StiTextEditorEventHandler TextEditorOpen;

	[Category("PreviewControl")]
	public event StiTextEditorEventHandler TextEditorClose;

	[Category("PreviewControl")]
	public event EventHandler ProcessSendEMail;

	[Category("PreviewControl")]
	public event EventHandler ProcessExport;

	[Category("PreviewControl")]
	public event EventHandler ClickComponent;

	[Category("PreviewControl")]
	public event EventHandler DoubleClickComponent;

	[Category("PreviewControl")]
	public event EventHandler MouseEnterComponent;

	[Category("PreviewControl")]
	public event EventHandler MouseLeaveComponent;

	[Category("PreviewControl")]
	public event EventHandler ZoomChanged;

	public static event EventHandler LoadPreviewControl
	{
		add
		{
			StiOptions.Engine.GlobalEvents.LoadViewerControl += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.LoadViewerControl -= value;
		}
	}

	public static event EventHandler SavingDocument
	{
		add
		{
			StiOptions.Engine.GlobalEvents.SavingDocumentInViewer += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.SavingDocumentInViewer -= value;
		}
	}

	public static event EventHandler LoadingDocument
	{
		add
		{
			StiOptions.Engine.GlobalEvents.LoadingDocumentInViewer += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.LoadingDocumentInViewer -= value;
		}
	}

	public static event EventHandler PrintingDocument
	{
		add
		{
			StiOptions.Engine.GlobalEvents.PrintingDocumentInViewer += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.PrintingDocumentInViewer -= value;
		}
	}

	private void ProcessChartInteractionDispose()
	{
		StiChartInteractionHelper.ResetChartGeomCache();
		try
		{
			if (chartInteractionRecallTimer != null)
			{
				((Component)(object)chartInteractionRecallTimer).Dispose();
				chartInteractionRecallTimer = null;
			}
		}
		catch
		{
		}
	}

	private void StopRecallTimer()
	{
		if (chartInteractionRecallTimer != null)
		{
			chartInteractionRecallTimer.Stop();
		}
	}

	private void ProcessChartInteractionMouseUp(Point pts, StiComponent comp, RectangleD rect)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (chartInteractionDragEnabled)
		{
			return;
		}
		StopRecallTimer();
		if (!(comp is StiChart))
		{
			return;
		}
		StiCellGeom chartGeom = null;
		IStiGeomInteraction interaction = StiChartInteractionHelper.GetInteraction(comp as StiChart, rect, pts.X, pts.Y, out chartGeom, (float)(Zoom * StiScale.X));
		if (interaction != null)
		{
			StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
			interaction.InvokeMouseUp(stiInteractionOptions);
			if (stiInteractionOptions.UpdateContext)
			{
				InvalidateComponent(comp);
				StiChartInteractionHelper.ResetChartGeomCache();
			}
		}
	}

	private void ProcessChartInteractionMouseDown(Point pts, StiComponent comp, RectangleD rect)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		ProcessChartInteractionMouseDown(pts, comp, rect, calledFromTimer: false);
	}

	private void ProcessChartInteractionMouseDown(Point pts, StiComponent comp, RectangleD rect, bool calledFromTimer)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		if (chartInteractionDragEnabled || !(comp is StiChart))
		{
			return;
		}
		StiCellGeom chartGeom;
		IStiGeomInteraction interaction = StiChartInteractionHelper.GetInteraction(comp as StiChart, rect, pts.X, pts.Y, out chartGeom, (float)(Zoom * StiScale.X));
		if (interaction == null)
		{
			return;
		}
		StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
		if (calledFromTimer)
		{
			stiInteractionOptions.IsRecalled = true;
		}
		if (interaction is StiCellGeom geom && chartGeom != null)
		{
			RectangleF rect2 = chartGeom.GetRect(geom);
			stiInteractionOptions.MousePoint = new PointF((float)((double)((float)pts.X - rect2.X) - ((RectangleD)(ref rect)).X), (float)((double)((float)pts.Y - rect2.Y) - ((RectangleD)(ref rect)).Y));
			chartInteractionDragGeom = geom;
		}
		interaction.InvokeMouseDown(stiInteractionOptions);
		if (stiInteractionOptions.UpdateContext)
		{
			InvalidateComponent(comp);
			StiChartInteractionHelper.ResetChartGeomCache();
			if (stiInteractionOptions.RecallEvent)
			{
				if (!calledFromTimer)
				{
					if (chartInteractionRecallTimer == null)
					{
						chartInteractionRecallTimer = new Timer();
					}
					else
					{
						StopRecallTimer();
					}
					chartInteractionRecallTimer.Interval = (int)stiInteractionOptions.RecallTime.TotalMilliseconds;
					chartInteractionRecallTimer.Tick += delegate
					{
						//IL_000e: Unknown result type (might be due to invalid IL or missing references)
						ProcessChartInteractionMouseDown(chartInteractionPoint, chartInteractionLatestChart, chartInteractionRect, calledFromTimer: true);
					};
					chartInteractionLatestChart = comp as StiChart;
					chartInteractionPoint = pts;
					chartInteractionRect = rect;
					chartInteractionRecallTimer.Start();
				}
				else if (calledFromTimer && chartInteractionRecallTimer != null)
				{
					chartInteractionRecallTimer.Stop();
					chartInteractionRecallTimer.Interval = (int)stiInteractionOptions.RecallTime.TotalMilliseconds;
					chartInteractionRecallTimer.Start();
				}
			}
		}
		if (stiInteractionOptions.DragEnabled && !chartInteractionDragEnabled)
		{
			chartInteractionDragEnabled = true;
			chartInteractionDragStartPoint = pts;
		}
	}

	private StiChartInteractionResult ProcessChartInteractionOldLeaveNewEnter(Point pts, StiComponent comp, RectangleD rect)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		StiChartInteractionResult stiChartInteractionResult = new StiChartInteractionResult();
		if (chartInteractionDragEnabled || !(comp is StiChart))
		{
			return stiChartInteractionResult;
		}
		StiCellGeom chartGeom = null;
		IStiGeomInteraction interaction = StiChartInteractionHelper.GetInteraction(comp as StiChart, rect, pts.X, pts.Y, out chartGeom, (float)(Zoom * StiScale.X));
		IStiGeomInteraction stiGeomInteraction = chartComponentToInteraction[comp] as IStiGeomInteraction;
		if (stiGeomInteraction != interaction)
		{
			bool flag = false;
			if (stiGeomInteraction != null)
			{
				StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
				stiGeomInteraction.InvokeMouseLeave(stiInteractionOptions);
				if (stiInteractionOptions.UpdateContext)
				{
					flag = true;
				}
				StopRecallTimer();
			}
			if (interaction != null)
			{
				StiInteractionOptions stiInteractionOptions2 = new StiInteractionOptions();
				interaction.InvokeMouseEnter(stiInteractionOptions2);
				if (stiInteractionOptions2.UpdateContext)
				{
					flag = true;
				}
				stiChartInteractionResult.ToolTipString = stiInteractionOptions2.InteractionToolTip;
				stiChartInteractionResult.HyperlinkString = stiInteractionOptions2.InteractionHyperlink;
			}
			chartComponentToInteraction[comp] = interaction;
			if (flag)
			{
				InvalidateComponent(comp);
			}
		}
		else if (interaction != null)
		{
			StiInteractionOptions stiInteractionOptions3 = new StiInteractionOptions();
			interaction.InvokeMouseEnter(stiInteractionOptions3);
			stiChartInteractionResult.ToolTipString = stiInteractionOptions3.InteractionToolTip;
			stiChartInteractionResult.HyperlinkString = stiInteractionOptions3.InteractionHyperlink;
		}
		return stiChartInteractionResult;
	}

	private void ProcessChartInteractionMouseLeave(StiComponent comp)
	{
		if (!chartInteractionDragEnabled && chartComponentToInteraction[comp] is IStiGeomInteraction stiGeomInteraction)
		{
			StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
			stiGeomInteraction.InvokeMouseLeave(stiInteractionOptions);
			StopRecallTimer();
			if (stiInteractionOptions.UpdateContext)
			{
				InvalidateComponent(comp);
			}
		}
	}

	private StiChartInteractionResult ProcessChartInteractionMouseEnter(Point pts, StiComponent comp, RectangleD rect)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		StiChartInteractionResult stiChartInteractionResult = new StiChartInteractionResult();
		if (chartInteractionDragEnabled)
		{
			return stiChartInteractionResult;
		}
		if (!(comp is StiChart stiChart))
		{
			return stiChartInteractionResult;
		}
		StiCellGeom chartGeom = null;
		IStiGeomInteraction interaction = StiChartInteractionHelper.GetInteraction(stiChart, rect, pts.X, pts.Y, out chartGeom, (float)(Zoom * StiScale.X));
		if (interaction == null)
		{
			return stiChartInteractionResult;
		}
		chartComponentToInteraction[stiChart] = interaction;
		StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
		interaction.InvokeMouseEnter(stiInteractionOptions);
		if (stiInteractionOptions.UpdateContext)
		{
			InvalidateComponent(stiChart);
		}
		stiChartInteractionResult.ToolTipString = stiInteractionOptions.InteractionToolTip;
		stiChartInteractionResult.HyperlinkString = stiInteractionOptions.InteractionHyperlink;
		return stiChartInteractionResult;
	}

	private StiChartInteractionResult ProcessChartInteractionClick(Point pts, StiComponent comp, RectangleD rect)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		StiChartInteractionResult stiChartInteractionResult = new StiChartInteractionResult();
		if (chartInteractionDragEnabled)
		{
			return stiChartInteractionResult;
		}
		StiCellGeom chartGeom = null;
		if (comp is StiChart)
		{
			IStiGeomInteraction interaction = StiChartInteractionHelper.GetInteraction(comp as StiChart, rect, pts.X, pts.Y, out chartGeom, (float)(Zoom * StiScale.X), typeof(StiSeriesLabelsGeom));
			if (interaction == null)
			{
				return stiChartInteractionResult;
			}
			StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
			interaction.InvokeClick(stiInteractionOptions);
			if (stiInteractionOptions.UpdateContext)
			{
				InvalidateComponent(comp);
				StiChartInteractionHelper.ResetChartGeomCache();
			}
			stiChartInteractionResult.HyperlinkString = stiInteractionOptions.InteractionHyperlink;
			stiChartInteractionResult.InteractionData = stiInteractionOptions.SeriesInteractionData;
		}
		return stiChartInteractionResult;
	}

	private void ProcessChartInteractionDragStop()
	{
		chartInteractionDragEnabled = false;
	}

	private void ProcessChartInteractionDrag(Point pts)
	{
		if (!chartInteractionDragEnabled)
		{
			return;
		}
		StiCellGeom stiCellGeom = chartInteractionDragGeom;
		if (stiCellGeom == null)
		{
			return;
		}
		StiInteractionOptions stiInteractionOptions = new StiInteractionOptions();
		stiInteractionOptions.DragDelta = new SizeF(pts.X - chartInteractionDragStartPoint.X, pts.Y - chartInteractionDragStartPoint.Y);
		if (!stiInteractionOptions.DragDelta.IsEmpty)
		{
			stiCellGeom.InvokeDrag(stiInteractionOptions);
			if (stiInteractionOptions.UpdateContext)
			{
				((Control)this).Invalidate();
				StiChartInteractionHelper.ResetChartGeomCache();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		if (disposing)
		{
			Clear();
			Report = null;
			mainReport = null;
			Config = null;
			findHelper.ProgressChanged -= Find_ProgressChanged;
			foreach (ButtonItem item in (CollectionBase)(object)((BaseItem)tbSave).SubItems)
			{
				ButtonItem val = item;
				((BaseItem)val).Tag = null;
			}
			foreach (ButtonItem item2 in (CollectionBase)(object)((BaseItem)tbSendEMail).SubItems)
			{
				ButtonItem val2 = item2;
				((BaseItem)val2).Tag = null;
			}
			Reports.Clear();
			reportsZooms.Clear();
			reportsHScrollBars.Clear();
			reportsVScrollBars.Clear();
			components?.Dispose();
			((Component)(object)SuperToolTip)?.Dispose();
			ProcessChartInteractionDispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	public StiViewerControl()
		: this(null)
	{
	}

	public StiViewerControl(StiReport report)
		: this(report, removeShortcuts: false)
	{
	}

	public StiViewerControl(StiReport report, bool removeShortcuts)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Expected O, but got Unknown
		try
		{
			InitializeComponent();
			if (!((Component)this).DesignMode)
			{
				ReportTabHelper = new StiReportTabHelper(this);
				Parameters = new StiParametersAccessor((IStiViewerAccessor)(object)this);
				Config = StiOptions.Services.ViewerConfig.FirstOrDefault();
				CurrentTooltipColor = (eTooltipColor)19;
				liFindWhat.ForeColor = Color.Black;
				tbFindNext.ForeColor = Color.Black;
				tbFindBar.BarType = (eBarType)0;
				StiOptions.Localization.LoadDefault();
				DoScaling();
				if (!StiOptions.Appearance.ShowTooltipsHelp)
				{
					((CollectionBase)(object)((BaseItem)tbHelp).Shortcuts).Clear();
				}
				if (removeShortcuts || StiOptions.Viewer.DisableShortcutInViewer)
				{
					((CollectionBase)(object)((BaseItem)tbSaveDocumentFile).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbFindPrevious).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbFindNext).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbPageControl).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbSaveDocumentFile).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbPageNew).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbPageDelete).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbPageDesign).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbPageSize).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbPrint).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbOpen).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbSave).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbSendEMail).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbBookmarks).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbThumbs).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbParameters).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbResources).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbEditor).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbFind).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbZoomMultiplePages).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbZoomOnePage).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbZoomPageWidth).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbZoomTwoPages).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbFullScreen).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbDotMatrix).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbViewModeContinuous).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbViewModeMultiplePages).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbViewModeSinglePage).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbZoomPageWidth).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbZoomTwoPages).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbZoomOnePage).Shortcuts).Clear();
					((CollectionBase)(object)((BaseItem)tbFullScreen).Shortcuts).Clear();
				}
				tbViewModeSinglePage.Checked = true;
				((Control)this).MouseWheel += new MouseEventHandler(View_MouseWheel);
				((UserControl)this).Load += This_FirstLoad;
				view.ViewerControl = this;
				lbThumbs.viewerControl = this;
				TextEditor = new StiTextEditor(this);
				((Control)this).SetStyle((ControlStyles)2, true);
				((Control)this).SetStyle((ControlStyles)8192, true);
				((Control)this).SetStyle((ControlStyles)16, true);
				((Control)this).SetStyle((ControlStyles)65536, true);
				((Control)this).SetStyle((ControlStyles)4, false);
				Report = report;
				Localize();
				multiPagesUserControl.viewerControl = this;
				BuildExportMenu();
				RefreshPageNofM();
				findHelper.ProgressChanged += Find_ProgressChanged;
				ApplyStyle();
				LoadConfig();
				UpdateControlsState();
				if (StiOptions.Viewer.AllowUseDragDrop)
				{
					((Control)this).AllowDrop = true;
					((Control)this).DragDrop += new DragEventHandler(This_DragDrop);
					((Control)this).DragOver += new DragEventHandler(This_DragOver);
				}
				if (ScaleX > 1.0)
				{
					ilBookmarks.ImageSize = new Size(32, 32);
					ilBookmarks.Images.Add("Report.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Report_x1_5.png"));
					ilBookmarks.Images.Add("Item.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Item_x1_5.png"));
					ilBookmarks.Images.Add("Items.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Items_x1_5.png"));
					ilBookmarks.Images.Add("Report.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Report_x2.png"));
					ilBookmarks.Images.Add("Item.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Item_x2.png"));
					ilBookmarks.Images.Add("Items.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Items_x2.png"));
					cbFindMatchCase.CheckSignSize = new Size(StiScale.XXI(13.0), StiScale.YYI(13.0));
					cbFindMatchWholeWord.CheckSignSize = new Size(StiScale.XXI(13.0), StiScale.YYI(13.0));
				}
				((Control)multiPagesUserControl).Size = new Size(StiScale.XXI(161.0), StiScale.YYI(108.0));
				((Control)scrollBarThumbs).Width = SystemInformation.VerticalScrollBarWidth;
				((Control)VScrollBar).Width = SystemInformation.VerticalScrollBarWidth;
				((Control)panelHorizontal).Height = SystemInformation.HorizontalScrollBarHeight;
				((BaseItem)cbFindMatchCase).ThemeAware = true;
				((BaseItem)cbFindMatchWholeWord).ThemeAware = true;
			}
		}
		catch
		{
		}
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Expected O, but got Unknown
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Expected O, but got Unknown
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd6: Expected O, but got Unknown
		//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Expected O, but got Unknown
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Expected O, but got Unknown
		//IL_114e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1158: Expected O, but got Unknown
		//IL_128c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1296: Expected O, but got Unknown
		//IL_1326: Unknown result type (might be due to invalid IL or missing references)
		//IL_1330: Expected O, but got Unknown
		//IL_13cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d6: Expected O, but got Unknown
		//IL_1414: Unknown result type (might be due to invalid IL or missing references)
		//IL_141e: Expected O, but got Unknown
		//IL_15f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fd: Expected O, but got Unknown
		//IL_183c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1846: Expected O, but got Unknown
		//IL_1f00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0a: Expected O, but got Unknown
		//IL_1f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f86: Expected O, but got Unknown
		//IL_1f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9d: Expected O, but got Unknown
		//IL_1faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb4: Expected O, but got Unknown
		//IL_1fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fcb: Expected O, but got Unknown
		//IL_1fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe2: Expected O, but got Unknown
		//IL_2084: Unknown result type (might be due to invalid IL or missing references)
		//IL_208e: Expected O, but got Unknown
		//IL_2341: Unknown result type (might be due to invalid IL or missing references)
		//IL_234b: Expected O, but got Unknown
		//IL_23fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2407: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiViewerControl));
		panelView = new Panel();
		view = new StiPageViewControl();
		contextMenuBar1 = new ContextMenuBar();
		contextMenuPreview = new ButtonItem();
		cm200 = new ButtonItem();
		cm150 = new ButtonItem();
		cm100 = new ButtonItem();
		cm75 = new ButtonItem();
		cm50 = new ButtonItem();
		cm25 = new ButtonItem();
		cm10 = new ButtonItem();
		cmZoomOnePage = new ButtonItem();
		cmZoomTwoPages = new ButtonItem();
		cmZoomPageWidth = new ButtonItem();
		contextMenuBookmarks = new ButtonItem();
		miExpandAll = new ButtonItem();
		miCollapseAll = new ButtonItem();
		tvBookmarks = new StiViewerTreeView();
		ilBookmarks = new ImageList(components);
		vScrollBar = new StiVScrollBar();
		panelHorizontal = new Panel();
		hScrollBar = new StiHScrollBar();
		panelVertCorr = new Panel();
		listResources = new ScrollableControl();
		panelDialogs = new Panel();
		splThumbs = new Splitter();
		lbThumbs = new StiThumbsControl();
		scrollBarThumbs = new StiVScrollBar();
		splBookmarks = new Splitter();
		splParameters = new Splitter();
		tbFindBar = new Bar();
		tbFindValue = new StiViewerTextBox();
		tbFindClose = new ButtonItem();
		liSeparator3 = new LabelItem();
		liFindWhat = new LabelItem();
		ciFindValue = new ControlContainerItem();
		tbFindNext = new ButtonItem();
		tbFindPrevious = new ButtonItem();
		cbFindMatchCase = new CheckBoxItem();
		cbFindMatchWholeWord = new CheckBoxItem();
		pbFindProgress = new ProgressBarItem();
		tbStatusBar = new Bar();
		icPageControl = new ItemContainer();
		tbFirstPage = new ButtonItem();
		tbPrevPage = new ButtonItem();
		tbPageControl = new ButtonItem();
		tbNextPage = new ButtonItem();
		tbLastPage = new ButtonItem();
		liSeparator = new LabelItem();
		icZoom = new ItemContainer();
		icPageViewMode = new ItemContainer();
		tbViewModeSinglePage = new ButtonItem();
		tbViewModeContinuous = new ButtonItem();
		tbViewModeMultiplePages = new ButtonItem();
		lbZoom = new LabelItem();
		siZoom = new SliderItem();
		liSeparator2 = new LabelItem();
		tabStripReports = new TabStrip();
		tbTool = new Bar();
		tbPrint = new ButtonItem();
		tbOpen = new ButtonItem();
		tbSave = new ButtonItem();
		tbSaveDocumentFile = new ButtonItem();
		tbSendEMail = new ButtonItem();
		tbSendEMailDocumentFile = new ButtonItem();
		tbPageNew = new ButtonItem();
		tbPageDelete = new ButtonItem();
		tbPageDesign = new ButtonItem();
		tbPageSize = new ButtonItem();
		tbBookmarks = new ButtonItem();
		tbParameters = new ButtonItem();
		tbResources = new ButtonItem();
		tbThumbs = new ButtonItem();
		tbFind = new ButtonItem();
		tbEditor = new ButtonItem();
		icZoomPages = new ItemContainer();
		tbFullScreen = new ButtonItem();
		tbZoomOnePage = new ButtonItem();
		tbZoomTwoPages = new ButtonItem();
		tbZoomMultiplePages = new ButtonItem();
		ciMultiPagesUserControl = new ControlContainerItem();
		multiPagesUserControl = new StiMultiPagesUserControl();
		tbZoomPageWidth = new ButtonItem();
		tbDotMatrix = new ButtonItem();
		tbClose = new ButtonItem();
		tbHelp = new ButtonItem();
		toolTip = new ToolTip(components);
		timerZoomSlider = new Timer(components);
		((Control)panelView).SuspendLayout();
		((Control)view).SuspendLayout();
		((ISupportInitialize)contextMenuBar1).BeginInit();
		((Control)panelHorizontal).SuspendLayout();
		((ISupportInitialize)tbFindBar).BeginInit();
		tbFindBar.SuspendLayout();
		((ISupportInitialize)tbStatusBar).BeginInit();
		((ISupportInitialize)tbTool).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panelView).Controls.Add((Control)(object)view);
		((Control)panelView).Controls.Add((Control)(object)vScrollBar);
		((Control)panelView).Controls.Add((Control)(object)panelHorizontal);
		((Control)panelView).Controls.Add((Control)(object)listResources);
		((Control)panelView).Controls.Add((Control)(object)panelDialogs);
		((Control)panelView).Controls.Add((Control)(object)splThumbs);
		((Control)panelView).Controls.Add((Control)(object)lbThumbs);
		((Control)panelView).Controls.Add((Control)(object)scrollBarThumbs);
		((Control)panelView).Controls.Add((Control)(object)splBookmarks);
		((Control)panelView).Controls.Add((Control)(object)splParameters);
		((Control)panelView).Controls.Add((Control)(object)tvBookmarks);
		((Control)panelView).Controls.Add((Control)(object)tbFindBar);
		((Control)panelView).Controls.Add((Control)(object)tbStatusBar);
		((Control)panelView).Controls.Add((Control)(object)tabStripReports);
		((Control)panelView).Controls.Add((Control)(object)tbTool);
		((Control)panelView).Dock = (DockStyle)5;
		((Control)panelView).Location = new Point(0, 0);
		((Control)panelView).Name = "panelView";
		((Control)panelView).Size = new Size(669, 443);
		((Control)panelView).TabIndex = 45;
		((Control)view).BackColor = Color.FromArgb(150, 182, 224);
		((Panel)view).BorderStyle = (BorderStyle)0;
		contextMenuBar1.SetContextMenuEx((Control)(object)view, (BaseItem)(object)contextMenuPreview);
		((Control)view).Controls.Add((Control)(object)contextMenuBar1);
		((Control)view).Dock = (DockStyle)5;
		((Control)view).Location = new Point(244, 53);
		((Control)view).Margin = new Padding(0);
		((Control)view).Name = "view";
		((Control)view).Size = new Size(221, 269);
		((Control)view).TabIndex = 1;
		((Panel)view).TabStop = true;
		view.ViewerControl = null;
		((Control)view).Click += View_Click;
		((Control)view).DoubleClick += View_DoubleClick;
		((Control)view).MouseDown += new MouseEventHandler(View_MouseDown);
		((Control)view).MouseLeave += View_MouseLeave;
		((Control)view).MouseMove += new MouseEventHandler(View_MouseMove);
		((Control)view).MouseUp += new MouseEventHandler(View_MouseUp);
		((Bar)contextMenuBar1).DockSide = (eDockSide)5;
		((Control)contextMenuBar1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Bar)contextMenuBar1).Items.AddRange((BaseItem[])(object)new BaseItem[2]
		{
			(BaseItem)contextMenuPreview,
			(BaseItem)contextMenuBookmarks
		});
		((Bar)contextMenuBar1).Location = new Point(64, 48);
		((Bar)contextMenuBar1).Name = "contextMenuBar1";
		((Bar)contextMenuBar1).Size = new Size(75, 47);
		((Bar)contextMenuBar1).Stretch = true;
		((Bar)contextMenuBar1).Style = (eDotNetBarStyle)8;
		((Bar)contextMenuBar1).TabIndex = 0;
		((Bar)contextMenuBar1).TabStop = false;
		((Control)contextMenuBar1).Text = "contextMenuBar1";
		contextMenuPreview.AutoExpandOnClick = true;
		((BaseItem)contextMenuPreview).Name = "contextMenuPreview";
		((BaseItem)contextMenuPreview).SubItems.AddRange((BaseItem[])(object)new BaseItem[10]
		{
			(BaseItem)cm200,
			(BaseItem)cm150,
			(BaseItem)cm100,
			(BaseItem)cm75,
			(BaseItem)cm50,
			(BaseItem)cm25,
			(BaseItem)cm10,
			(BaseItem)cmZoomOnePage,
			(BaseItem)cmZoomTwoPages,
			(BaseItem)cmZoomPageWidth
		});
		((BaseItem)contextMenuPreview).Text = "Preview";
		((BaseItem)cm200).Name = "cm200";
		((BaseItem)cm200).Text = "200%";
		((BaseItem)cm200).Click += ItemZoom_Click;
		((BaseItem)cm150).Name = "cm150";
		((BaseItem)cm150).Text = "150%";
		((BaseItem)cm150).Click += ItemZoom_Click;
		((BaseItem)cm100).Name = "cm100";
		((BaseItem)cm100).Text = "100%";
		((BaseItem)cm100).Click += ItemZoom_Click;
		((BaseItem)cm75).Name = "cm75";
		((BaseItem)cm75).Text = "75%";
		((BaseItem)cm75).Click += ItemZoom_Click;
		((BaseItem)cm50).Name = "cm50";
		((BaseItem)cm50).Text = "50%";
		((BaseItem)cm50).Click += ItemZoom_Click;
		((BaseItem)cm25).Name = "cm25";
		((BaseItem)cm25).Text = "25%";
		((BaseItem)cm25).Click += ItemZoom_Click;
		((BaseItem)cm10).Name = "cm10";
		((BaseItem)cm10).Text = "10%";
		((BaseItem)cm10).Click += ItemZoom_Click;
		((BaseItem)cmZoomOnePage).BeginGroup = true;
		cmZoomOnePage.ImageIndex = 11;
		((BaseItem)cmZoomOnePage).Name = "cmZoomOnePage";
		((BaseItem)cmZoomOnePage).Text = "One Page";
		((BaseItem)cmZoomOnePage).Click += ButtonZoomOnePage_Click;
		cmZoomTwoPages.ImageIndex = 12;
		((BaseItem)cmZoomTwoPages).Name = "cmZoomTwoPages";
		((BaseItem)cmZoomTwoPages).Text = "Two Pages";
		((BaseItem)cmZoomTwoPages).Click += ButtonZoomTwoPages_Click;
		cmZoomPageWidth.ImageIndex = 15;
		((BaseItem)cmZoomPageWidth).Name = "cmZoomPageWidth";
		((BaseItem)cmZoomPageWidth).Text = "Page Width";
		((BaseItem)cmZoomPageWidth).Click += ButtonZoomPageWidth_Click;
		contextMenuBookmarks.AutoExpandOnClick = true;
		((BaseItem)contextMenuBookmarks).Name = "contextMenuBookmarks";
		((BaseItem)contextMenuBookmarks).SubItems.AddRange((BaseItem[])(object)new BaseItem[2]
		{
			(BaseItem)miExpandAll,
			(BaseItem)miCollapseAll
		});
		((BaseItem)contextMenuBookmarks).Text = "Bookmarks";
		miExpandAll.ImageIndex = 10;
		((BaseItem)miExpandAll).Name = "miExpandAll";
		((BaseItem)miExpandAll).Text = "Expand All";
		((BaseItem)miExpandAll).Click += ButtonExpandAll_Click;
		miCollapseAll.ImageIndex = 9;
		((BaseItem)miCollapseAll).Name = "miCollapseAll";
		((BaseItem)miCollapseAll).Text = "Collapse All";
		((BaseItem)miCollapseAll).Click += ButtonCollapseAll_Click;
		contextMenuBar1.SetContextMenuEx((Control)(object)tvBookmarks, (BaseItem)(object)contextMenuBookmarks);
		((Control)tvBookmarks).Dock = (DockStyle)3;
		((TreeView)tvBookmarks).ImageIndex = 0;
		((TreeView)tvBookmarks).ImageList = ilBookmarks;
		((Control)tvBookmarks).Location = new Point(0, 53);
		tvBookmarks.Mirrored = false;
		((Control)tvBookmarks).Name = "tvBookmarks";
		((TreeView)tvBookmarks).SelectedImageIndex = 0;
		((Control)tvBookmarks).Size = new Size(136, 334);
		((Control)tvBookmarks).TabIndex = 52;
		((Control)tvBookmarks).Visible = false;
		((TreeView)tvBookmarks).AfterSelect += new TreeViewEventHandler(TreeViewBookmarks_AfterSelect);
		ilBookmarks.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilBookmarks.ImageStream");
		ilBookmarks.TransparentColor = Color.Magenta;
		ilBookmarks.Images.SetKeyName(0, "");
		ilBookmarks.Images.SetKeyName(1, "");
		ilBookmarks.Images.SetKeyName(2, "");
		ilBookmarks.Images.SetKeyName(3, "Report.png");
		ilBookmarks.Images.SetKeyName(4, "Item.png");
		ilBookmarks.Images.SetKeyName(5, "Items.png");
		((Control)vScrollBar).BackColor = Color.FromArgb(255, 255, 255);
		((Control)vScrollBar).Dock = (DockStyle)4;
		((Control)vScrollBar).ForeColor = Color.Black;
		((ScrollBar)vScrollBar).LargeChange = 50;
		((Control)vScrollBar).Location = new Point(465, 53);
		((Control)vScrollBar).Name = "vScrollBar";
		((Control)vScrollBar).Size = new Size(16, 269);
		((Control)vScrollBar).TabIndex = 42;
		((ScrollBar)vScrollBar).Scroll += new ScrollEventHandler(VerticalScrollBar_Scroll);
		((Control)panelHorizontal).Controls.Add((Control)(object)hScrollBar);
		((Control)panelHorizontal).Controls.Add((Control)(object)panelVertCorr);
		((Control)panelHorizontal).Dock = (DockStyle)2;
		((Control)panelHorizontal).Location = new Point(244, 322);
		((Control)panelHorizontal).Name = "panelHorizontal";
		((Control)panelHorizontal).Size = new Size(237, 16);
		((Control)panelHorizontal).TabIndex = 40;
		((Control)panelHorizontal).Resize += PanelHorizontal_Resize;
		((Control)hScrollBar).BackColor = Color.FromArgb(255, 255, 255);
		((Control)hScrollBar).Dock = (DockStyle)5;
		((Control)hScrollBar).ForeColor = Color.Black;
		((Control)hScrollBar).Location = new Point(0, 0);
		((Control)hScrollBar).Name = "hScrollBar";
		((Control)hScrollBar).Size = new Size(221, 16);
		((Control)hScrollBar).TabIndex = 68;
		((ScrollBar)hScrollBar).Scroll += new ScrollEventHandler(HorizontalScrollBar_Scroll);
		((Control)panelVertCorr).Dock = (DockStyle)4;
		((Control)panelVertCorr).Location = new Point(221, 0);
		((Control)panelVertCorr).Name = "panelVertCorr";
		((Control)panelVertCorr).Size = new Size(16, 16);
		((Control)panelVertCorr).TabIndex = 41;
		listResources.AutoScroll = true;
		((Control)listResources).BackColor = Color.White;
		((Control)listResources).Dock = (DockStyle)2;
		((Control)listResources).Location = new Point(244, 338);
		((Control)listResources).Name = "listResources";
		((Control)listResources).Size = new Size(237, 49);
		((Control)listResources).TabIndex = 0;
		((Control)listResources).Visible = false;
		((Control)panelDialogs).BackColor = Color.FromArgb(150, 182, 224);
		((Control)panelDialogs).Dock = (DockStyle)3;
		((Control)panelDialogs).Location = new Point(144, 53);
		((Control)panelDialogs).Name = "panelDialogs";
		((Control)panelDialogs).Size = new Size(100, 334);
		((Control)panelDialogs).TabIndex = 64;
		((Control)panelDialogs).Visible = false;
		((Control)splThumbs).BackColor = Color.White;
		((Control)splThumbs).Dock = (DockStyle)4;
		((Control)splThumbs).ForeColor = Color.Black;
		((Control)splThumbs).Location = new Point(481, 53);
		splThumbs.MinExtra = 130;
		splThumbs.MinSize = 130;
		((Control)splThumbs).Name = "splThumbs";
		((Control)splThumbs).Size = new Size(4, 334);
		((Control)splThumbs).TabIndex = 71;
		splThumbs.TabStop = false;
		splThumbs.SplitterMoved += new SplitterEventHandler(Splitter_Moved);
		((Panel)lbThumbs).BorderStyle = (BorderStyle)0;
		((Control)lbThumbs).Dock = (DockStyle)4;
		((Control)lbThumbs).Location = new Point(485, 53);
		((Control)lbThumbs).Name = "lbThumbs";
		lbThumbs.SelectedIndex = 0;
		((Control)lbThumbs).Size = new Size(168, 334);
		((Control)lbThumbs).TabIndex = 0;
		((Panel)lbThumbs).TabStop = true;
		((Control)lbThumbs).SizeChanged += LabelThumbs_SizeChanged;
		((Control)scrollBarThumbs).BackColor = Color.FromArgb(255, 255, 255);
		((Control)scrollBarThumbs).Dock = (DockStyle)4;
		((Control)scrollBarThumbs).ForeColor = Color.Black;
		((ScrollBar)scrollBarThumbs).LargeChange = 50;
		((Control)scrollBarThumbs).Location = new Point(653, 53);
		((Control)scrollBarThumbs).Name = "scrollBarThumbs";
		((Control)scrollBarThumbs).Size = new Size(16, 334);
		((Control)scrollBarThumbs).TabIndex = 43;
		((ScrollBar)scrollBarThumbs).Scroll += new ScrollEventHandler(ScrollBarThumbs_Scroll);
		((Control)splBookmarks).BackColor = Color.White;
		((Control)splBookmarks).ForeColor = Color.Black;
		((Control)splBookmarks).Location = new Point(140, 53);
		splBookmarks.MinSize = 24;
		((Control)splBookmarks).Name = "splBookmarks";
		((Control)splBookmarks).Size = new Size(4, 334);
		((Control)splBookmarks).TabIndex = 70;
		splBookmarks.TabStop = false;
		splBookmarks.SplitterMoved += new SplitterEventHandler(Splitter_Moved);
		((Control)splParameters).BackColor = Color.White;
		((Control)splParameters).ForeColor = Color.Black;
		((Control)splParameters).Location = new Point(136, 53);
		splParameters.MinSize = 24;
		((Control)splParameters).Name = "splParameters";
		((Control)splParameters).Size = new Size(4, 334);
		((Control)splParameters).TabIndex = 70;
		splParameters.TabStop = false;
		((Control)splParameters).Visible = false;
		splParameters.SplitterMoved += new SplitterEventHandler(Splitter_Moved);
		((Control)tbFindBar).Controls.Add((Control)(object)tbFindValue);
		((Control)tbFindBar).Dock = (DockStyle)2;
		tbFindBar.DockSide = (eDockSide)5;
		((Control)tbFindBar).Font = new Font("Segoe UI", 9f);
		tbFindBar.Items.AddRange((BaseItem[])(object)new BaseItem[9]
		{
			(BaseItem)tbFindClose,
			(BaseItem)liSeparator3,
			(BaseItem)liFindWhat,
			(BaseItem)ciFindValue,
			(BaseItem)tbFindNext,
			(BaseItem)tbFindPrevious,
			(BaseItem)cbFindMatchCase,
			(BaseItem)cbFindMatchWholeWord,
			(BaseItem)pbFindProgress
		});
		tbFindBar.Location = new Point(0, 387);
		tbFindBar.Name = "tbFindBar";
		tbFindBar.PaddingBottom = 0;
		tbFindBar.PaddingTop = 2;
		tbFindBar.RoundCorners = false;
		tbFindBar.Size = new Size(669, 28);
		tbFindBar.Stretch = true;
		tbFindBar.Style = (eDotNetBarStyle)8;
		tbFindBar.TabIndex = 73;
		tbFindBar.TabStop = false;
		((Control)tbFindBar).Text = "bar1";
		tbFindBar.Visible = false;
		((Control)tbFindBar).VisibleChanged += ButtonFindBar_VisibleChanged;
		((Control)tbFindValue).BackColor = Color.FromArgb(255, 255, 255);
		((TextBoxX)tbFindValue).Border.Class = "TextBoxBorder";
		((TextBoxX)tbFindValue).Border.CornerType = (eCornerType)1;
		((Control)tbFindValue).ForeColor = Color.Black;
		((Control)tbFindValue).Location = new Point(109, 3);
		((Control)tbFindValue).Name = "tbFindValue";
		((Control)tbFindValue).Size = new Size(140, 23);
		((Control)tbFindValue).TabIndex = 1;
		((Control)tbFindValue).KeyDown += new KeyEventHandler(TextBoxFindValue_KeyDown);
		((BaseItem)tbFindClose).Name = "tbFindClose";
		((BaseItem)tbFindClose).Click += ButtonFindClose_Click;
		((BaseItem)liSeparator3).Name = "liSeparator3";
		liSeparator3.Width = 10;
		((BaseItem)liFindWhat).Name = "liFindWhat";
		((BaseItem)liFindWhat).Text = "Find What:";
		ciFindValue.AllowItemResize = false;
		ciFindValue.Control = (Control)(object)tbFindValue;
		ciFindValue.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)ciFindValue).Name = "ciFindValue";
		tbFindNext.ButtonStyle = (eButtonStyle)2;
		((BaseItem)tbFindNext).Name = "tbFindNext";
		((BaseItem)tbFindNext).Shortcuts.Add((eShortcut)118);
		((BaseItem)tbFindNext).Text = "Next";
		((BaseItem)tbFindNext).Click += ButtonFindNext_Click;
		tbFindPrevious.ButtonStyle = (eButtonStyle)2;
		((BaseItem)tbFindPrevious).Name = "tbFindPrevious";
		((BaseItem)tbFindPrevious).Shortcuts.Add((eShortcut)65654);
		((BaseItem)tbFindPrevious).Click += ButtonFindPrevious_Click;
		((BaseItem)cbFindMatchCase).Name = "cbFindMatchCase";
		((BaseItem)cbFindMatchCase).Text = "Match case";
		((BaseItem)cbFindMatchWholeWord).Name = "cbFindMatchWholeWord";
		((BaseItem)cbFindMatchWholeWord).Text = "Match whole word";
		pbFindProgress.BackStyle.CornerType = (eCornerType)1;
		pbFindProgress.ChunkGradientAngle = 0f;
		pbFindProgress.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)pbFindProgress).Name = "pbFindProgress";
		pbFindProgress.RecentlyUsed = false;
		((BaseItem)pbFindProgress).Visible = false;
		pbFindProgress.Width = 120;
		((Control)tbStatusBar).AccessibleDescription = "bar1 (tbStatusBar)";
		((Control)tbStatusBar).AccessibleName = "bar1";
		((Control)tbStatusBar).AccessibleRole = (AccessibleRole)22;
		tbStatusBar.BarType = (eBarType)2;
		((Control)tbStatusBar).Dock = (DockStyle)2;
		tbStatusBar.DockSide = (eDockSide)5;
		((Control)tbStatusBar).Font = new Font("Segoe UI", 9f);
		tbStatusBar.Items.AddRange((BaseItem[])(object)new BaseItem[3]
		{
			(BaseItem)icPageControl,
			(BaseItem)liSeparator,
			(BaseItem)icZoom
		});
		tbStatusBar.Location = new Point(0, 415);
		tbStatusBar.Name = "tbStatusBar";
		tbStatusBar.PaddingTop = 3;
		tbStatusBar.RoundCorners = false;
		tbStatusBar.Size = new Size(669, 28);
		tbStatusBar.Stretch = true;
		tbStatusBar.Style = (eDotNetBarStyle)8;
		tbStatusBar.TabIndex = 72;
		tbStatusBar.TabStop = false;
		((Control)tbStatusBar).Text = "bar1";
		icPageControl.BackgroundStyle.CornerType = (eCornerType)1;
		((BaseItem)icPageControl).BeginGroup = true;
		((BaseItem)icPageControl).Name = "icPageControl";
		((BaseItem)icPageControl).SubItems.AddRange((BaseItem[])(object)new BaseItem[5]
		{
			(BaseItem)tbFirstPage,
			(BaseItem)tbPrevPage,
			(BaseItem)tbPageControl,
			(BaseItem)tbNextPage,
			(BaseItem)tbLastPage
		});
		icPageControl.TitleStyle.CornerType = (eCornerType)1;
		((BaseItem)tbFirstPage).BeginGroup = true;
		tbFirstPage.ImageIndex = 4;
		((BaseItem)tbFirstPage).Name = "tbFirstPage";
		((BaseItem)tbFirstPage).Text = "First Page";
		((BaseItem)tbFirstPage).Click += ButtonFirstPage_Click;
		tbPrevPage.ImageIndex = 5;
		((BaseItem)tbPrevPage).Name = "tbPrevPage";
		((BaseItem)tbPrevPage).Text = "buttonItem3";
		((BaseItem)tbPrevPage).Click += ButtonPrevPage_Click;
		tbPageControl.AutoExpandOnClick = true;
		((BaseItem)tbPageControl).Name = "tbPageControl";
		((PopupItem)tbPageControl).PopupSide = (ePopupSide)3;
		((BaseItem)tbPageControl).Shortcuts.Add((eShortcut)131143);
		((BaseItem)tbPageControl).Text = "Page 1 of 3";
		((BaseItem)tbPageControl).Click += ButtonJumpToPage_Click;
		tbNextPage.ImageIndex = 6;
		((BaseItem)tbNextPage).Name = "tbNextPage";
		((BaseItem)tbNextPage).Text = "buttonItem4";
		((BaseItem)tbNextPage).Click += ButtonNextPage_Click;
		tbLastPage.ImageIndex = 7;
		((BaseItem)tbLastPage).Name = "tbLastPage";
		((BaseItem)tbLastPage).Text = "Last Page";
		((BaseItem)tbLastPage).Click += ButtonLastPage_Click;
		((BaseItem)liSeparator).ItemAlignment = (eItemAlignment)1;
		((BaseItem)liSeparator).Name = "liSeparator";
		liSeparator.Width = 20;
		icZoom.BackgroundStyle.Class = "Office2007StatusBarBackground2";
		icZoom.BackgroundStyle.CornerType = (eCornerType)1;
		((BaseItem)icZoom).Name = "icZoom";
		((BaseItem)icZoom).SubItems.AddRange((BaseItem[])(object)new BaseItem[4]
		{
			(BaseItem)icPageViewMode,
			(BaseItem)lbZoom,
			(BaseItem)siZoom,
			(BaseItem)liSeparator2
		});
		icZoom.TitleStyle.CornerType = (eCornerType)1;
		icPageViewMode.BackgroundStyle.CornerType = (eCornerType)1;
		((BaseItem)icPageViewMode).BeginGroup = true;
		((BaseItem)icPageViewMode).Name = "icPageViewMode";
		((BaseItem)icPageViewMode).SubItems.AddRange((BaseItem[])(object)new BaseItem[3]
		{
			(BaseItem)tbViewModeSinglePage,
			(BaseItem)tbViewModeContinuous,
			(BaseItem)tbViewModeMultiplePages
		});
		icPageViewMode.TitleStyle.CornerType = (eCornerType)1;
		tbViewModeSinglePage.ImageIndex = 26;
		tbViewModeSinglePage.ImagePaddingHorizontal = 6;
		tbViewModeSinglePage.ImagePaddingVertical = 4;
		((BaseItem)tbViewModeSinglePage).Name = "tbViewModeSinglePage";
		((BaseItem)tbViewModeSinglePage).Shortcuts.Add((eShortcut)65649);
		((BaseItem)tbViewModeSinglePage).Text = "Single Page";
		((BaseItem)tbViewModeSinglePage).Click += ButtonViewModeSinglePage_Click;
		tbViewModeContinuous.ImageIndex = 27;
		tbViewModeContinuous.ImagePaddingHorizontal = 6;
		tbViewModeContinuous.ImagePaddingVertical = 4;
		((BaseItem)tbViewModeContinuous).Name = "tbViewModeContinuous";
		((BaseItem)tbViewModeContinuous).Shortcuts.Add((eShortcut)65650);
		((BaseItem)tbViewModeContinuous).Text = "Continuous";
		((BaseItem)tbViewModeContinuous).Click += ButtonViewModeContinuous_Click;
		tbViewModeMultiplePages.ImageIndex = 28;
		tbViewModeMultiplePages.ImagePaddingHorizontal = 6;
		tbViewModeMultiplePages.ImagePaddingVertical = 4;
		((BaseItem)tbViewModeMultiplePages).Name = "tbViewModeMultiplePages";
		((BaseItem)tbViewModeMultiplePages).Shortcuts.Add((eShortcut)65651);
		((BaseItem)tbViewModeMultiplePages).Text = "Multiple Pages";
		((BaseItem)tbViewModeMultiplePages).Click += ButtonViewModeMultiplePages_Click;
		((BaseItem)lbZoom).Name = "lbZoom";
		((BaseItem)lbZoom).Text = "100%";
		lbZoom.TextAlignment = (StringAlignment)1;
		lbZoom.Width = 60;
		((BaseItem)siZoom).ClickRepeatInterval = 50;
		siZoom.LabelVisible = false;
		siZoom.LabelWidth = 36;
		siZoom.Maximum = 400;
		siZoom.Minimum = 10;
		((BaseItem)siZoom).Name = "siZoom";
		siZoom.Step = 10;
		((BaseItem)siZoom).Text = "100%";
		siZoom.Value = 100;
		siZoom.Width = 130;
		siZoom.ValueChanged += SliderZoom_ValueChanged;
		((BaseItem)liSeparator2).Name = "liSeparator2";
		liSeparator2.Width = 10;
		tabStripReports.AutoHideSystemBox = true;
		tabStripReports.AutoSelectAttachedControl = true;
		tabStripReports.CanReorderTabs = true;
		tabStripReports.CloseButtonVisible = true;
		((Control)tabStripReports).Dock = (DockStyle)1;
		((Control)tabStripReports).ForeColor = Color.Black;
		((Control)tabStripReports).Location = new Point(0, 27);
		((Control)tabStripReports).Name = "tabStripReports";
		tabStripReports.SelectedTab = null;
		tabStripReports.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		((Control)tabStripReports).Size = new Size(669, 26);
		tabStripReports.Style = (eTabStripStyle)9;
		tabStripReports.TabAlignment = (eTabStripAlignment)2;
		((Control)tabStripReports).TabIndex = 74;
		tabStripReports.TabLayoutType = (eTabLayoutType)2;
		((Control)tabStripReports).Text = "tabStrip1";
		((Control)tabStripReports).Visible = false;
		tabStripReports.SelectedTabChanged += new SelectedTabChangedEventHandler(TabStripReports_SelectedTabChanged);
		tabStripReports.SelectedTabChanging += new SelectedTabChangingEventHandler(TabStripReports_SelectedTabChanging);
		tabStripReports.NavigateBack += new UserActionEventHandler(TabStripReports_NavigateBack);
		tabStripReports.NavigateForward += new UserActionEventHandler(TabStripReports_NavigateForward);
		tabStripReports.TabItemClose += new UserActionEventHandler(TabStripReports_TabItemClose);
		tabStripReports.TabRemoved += TabStripReports_TabRemoved;
		((Control)tabStripReports).TabIndexChanged += TabStripReports_TabIndexChanged;
		((Control)tbTool).AccessibleDescription = "DotNetBar Bar (tbTool)";
		((Control)tbTool).AccessibleName = "DotNetBar Bar";
		((Control)tbTool).AccessibleRole = (AccessibleRole)22;
		tbTool.CanAutoHide = false;
		tbTool.CanUndock = false;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.DockSide = (eDockSide)5;
		((Control)tbTool).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		tbTool.GrabHandleStyle = (eGrabHandleStyle)10;
		tbTool.Items.AddRange((BaseItem[])(object)new BaseItem[18]
		{
			(BaseItem)tbPrint,
			(BaseItem)tbOpen,
			(BaseItem)tbSave,
			(BaseItem)tbSendEMail,
			(BaseItem)tbPageNew,
			(BaseItem)tbPageDelete,
			(BaseItem)tbPageDesign,
			(BaseItem)tbPageSize,
			(BaseItem)tbBookmarks,
			(BaseItem)tbParameters,
			(BaseItem)tbResources,
			(BaseItem)tbThumbs,
			(BaseItem)tbFind,
			(BaseItem)tbEditor,
			(BaseItem)icZoomPages,
			(BaseItem)tbDotMatrix,
			(BaseItem)tbClose,
			(BaseItem)tbHelp
		});
		tbTool.ItemSpacing = 1;
		tbTool.Location = new Point(0, 0);
		tbTool.Name = "tbTool";
		tbTool.PaddingBottom = 2;
		tbTool.PaddingTop = 2;
		tbTool.RoundCorners = false;
		tbTool.Size = new Size(669, 27);
		tbTool.Stretch = true;
		tbTool.Style = (eDotNetBarStyle)8;
		tbTool.TabIndex = 51;
		tbTool.TabStop = false;
		((Control)tbTool).Text = "stiToolBar1";
		tbPrint.ButtonStyle = (eButtonStyle)2;
		tbPrint.ImageIndex = 0;
		((BaseItem)tbPrint).Name = "tbPrint";
		((BaseItem)tbPrint).Shortcuts.Add((eShortcut)131152);
		((BaseItem)tbPrint).Text = "Print";
		((BaseItem)tbPrint).Click += ButtonPrint_Click;
		tbOpen.ButtonStyle = (eButtonStyle)2;
		tbOpen.ImageIndex = 1;
		((BaseItem)tbOpen).Name = "tbOpen";
		((BaseItem)tbOpen).Shortcuts.Add((eShortcut)131151);
		((BaseItem)tbOpen).Text = "Open";
		((BaseItem)tbOpen).Click += ButtonOpen_Click;
		tbSave.AutoExpandOnClick = true;
		tbSave.ButtonStyle = (eButtonStyle)2;
		tbSave.ImageIndex = 2;
		((BaseItem)tbSave).Name = "tbSave";
		((BaseItem)tbSave).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)tbSaveDocumentFile });
		((BaseItem)tbSave).Text = "Save";
		((PopupItem)tbSave).PopupOpen += new PopupOpenEventHandler(ButtonSave_PopupOpen);
		((BaseItem)tbSaveDocumentFile).Name = "tbSaveDocumentFile";
		((BaseItem)tbSaveDocumentFile).Shortcuts.Add((eShortcut)131155);
		((BaseItem)tbSaveDocumentFile).Text = "Document File...";
		((BaseItem)tbSaveDocumentFile).Click += ButtonSave_Click;
		tbSendEMail.AutoExpandOnClick = true;
		tbSendEMail.ImageIndex = 24;
		((BaseItem)tbSendEMail).Name = "tbSendEMail";
		((BaseItem)tbSendEMail).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)tbSendEMailDocumentFile });
		((BaseItem)tbSendEMail).Text = "\\";
		((PopupItem)tbSendEMail).PopupOpen += new PopupOpenEventHandler(ButtonSendEMail_PopupOpen);
		((BaseItem)tbSendEMail).Click += ButtonSendEMail_Click;
		((BaseItem)tbSendEMailDocumentFile).Name = "tbSendEMailDocumentFile";
		((BaseItem)tbSendEMailDocumentFile).Text = "Document File...";
		((BaseItem)tbSendEMailDocumentFile).Click += ButtonSendEMailDocumentFile_Click;
		((BaseItem)tbPageNew).BeginGroup = true;
		tbPageNew.ImageIndex = 21;
		((BaseItem)tbPageNew).Name = "tbPageNew";
		((BaseItem)tbPageNew).Shortcuts.Add((eShortcut)196686);
		((BaseItem)tbPageNew).Text = "New";
		((BaseItem)tbPageNew).Click += ButtonPageNew_Click;
		tbPageDelete.ImageIndex = 23;
		((BaseItem)tbPageDelete).Name = "tbPageDelete";
		((BaseItem)tbPageDelete).Shortcuts.Add((eShortcut)196676);
		((BaseItem)tbPageDelete).Text = "Delete";
		((BaseItem)tbPageDelete).Click += ButtonPageDelete_Click;
		tbPageDesign.ImageIndex = 22;
		((BaseItem)tbPageDesign).Name = "tbPageDesign";
		((BaseItem)tbPageDesign).Shortcuts.Add((eShortcut)196677);
		((BaseItem)tbPageDesign).Text = "Edit";
		((BaseItem)tbPageDesign).Click += ButtonPageDesign_Click;
		tbPageSize.ImageIndex = 25;
		((BaseItem)tbPageSize).Name = "tbPageSize";
		((BaseItem)tbPageSize).Shortcuts.Add((eShortcut)196691);
		((BaseItem)tbPageSize).Text = "Size";
		((BaseItem)tbPageSize).Click += ButtonPageSize_Click;
		tbBookmarks.AutoCheckOnClick = true;
		((BaseItem)tbBookmarks).AutoCollapseOnClick = false;
		((BaseItem)tbBookmarks).BeginGroup = true;
		tbBookmarks.Checked = true;
		tbBookmarks.ImageIndex = 8;
		((BaseItem)tbBookmarks).Name = "tbBookmarks";
		((BaseItem)tbBookmarks).Shortcuts.Add((eShortcut)131138);
		((BaseItem)tbBookmarks).Text = "Bookmarks";
		tbBookmarks.CheckedChanged += ButtonBookmarks_CheckedChanged;
		tbParameters.AutoCheckOnClick = true;
		((BaseItem)tbParameters).AutoCollapseOnClick = false;
		((BaseItem)tbParameters).Enabled = false;
		tbParameters.ImageIndex = 16;
		((BaseItem)tbParameters).Name = "tbParameters";
		((BaseItem)tbParameters).Shortcuts.Add((eShortcut)196688);
		((BaseItem)tbParameters).Text = "Parameters";
		tbParameters.CheckedChanged += ButtonParameters_CheckedChanged;
		tbResources.AutoCheckOnClick = true;
		((BaseItem)tbResources).AutoCollapseOnClick = false;
		((BaseItem)tbResources).Enabled = false;
		tbResources.ImageIndex = 16;
		((BaseItem)tbResources).Name = "tbResources";
		((BaseItem)tbResources).Shortcuts.Add((eShortcut)131154);
		((BaseItem)tbResources).Text = "Resources";
		tbResources.CheckedChanged += ButtonResources_CheckedChanged;
		tbThumbs.AutoCheckOnClick = true;
		((BaseItem)tbThumbs).AutoCollapseOnClick = false;
		tbThumbs.Checked = true;
		tbThumbs.ImageIndex = 16;
		((BaseItem)tbThumbs).Name = "tbThumbs";
		((BaseItem)tbThumbs).Shortcuts.Add((eShortcut)131156);
		((BaseItem)tbThumbs).Text = "Thumbnails";
		((BaseItem)tbThumbs).Click += ButtonThumbs_Click;
		((BaseItem)tbFind).BeginGroup = true;
		tbFind.ImageIndex = 20;
		((BaseItem)tbFind).Name = "tbFind";
		((BaseItem)tbFind).Shortcuts.Add((eShortcut)131142);
		((BaseItem)tbFind).Text = "Find";
		((BaseItem)tbFind).Click += ButtonToolFind_Click;
		((BaseItem)tbEditor).Enabled = false;
		tbEditor.ImageIndex = 18;
		((BaseItem)tbEditor).Name = "tbEditor";
		((BaseItem)tbEditor).Shortcuts.Add((eShortcut)131141);
		((BaseItem)tbEditor).Text = "Editor";
		tbEditor.CheckedChanged += ButtonToolEditor_CheckedChanged;
		((BaseItem)tbEditor).Click += ButtonToolEditor_Click;
		icZoomPages.BackgroundStyle.CornerType = (eCornerType)1;
		((BaseItem)icZoomPages).BeginGroup = true;
		((BaseItem)icZoomPages).Name = "icZoomPages";
		((BaseItem)icZoomPages).SubItems.AddRange((BaseItem[])(object)new BaseItem[5]
		{
			(BaseItem)tbFullScreen,
			(BaseItem)tbZoomOnePage,
			(BaseItem)tbZoomTwoPages,
			(BaseItem)tbZoomMultiplePages,
			(BaseItem)tbZoomPageWidth
		});
		icZoomPages.TitleStyle.CornerType = (eCornerType)1;
		((BaseItem)tbFullScreen).BeginGroup = true;
		tbFullScreen.ImageIndex = 29;
		((BaseItem)tbFullScreen).Name = "tbFullScreen";
		((BaseItem)tbFullScreen).Shortcuts.Add((eShortcut)113);
		((BaseItem)tbFullScreen).Text = "Full Screen";
		((BaseItem)tbFullScreen).Click += ButtonFullScreen_Click;
		tbZoomOnePage.ImageIndex = 11;
		((BaseItem)tbZoomOnePage).Name = "tbZoomOnePage";
		((BaseItem)tbZoomOnePage).Shortcuts.Add((eShortcut)114);
		((BaseItem)tbZoomOnePage).Click += ButtonZoomOnePage_Click;
		tbZoomTwoPages.ImageIndex = 12;
		((BaseItem)tbZoomTwoPages).Name = "tbZoomTwoPages";
		((BaseItem)tbZoomTwoPages).Shortcuts.Add((eShortcut)115);
		((BaseItem)tbZoomTwoPages).Click += ButtonZoomTwoPages_Click;
		tbZoomMultiplePages.AutoExpandOnClick = true;
		tbZoomMultiplePages.ImageIndex = 13;
		((BaseItem)tbZoomMultiplePages).Name = "tbZoomMultiplePages";
		((PopupItem)tbZoomMultiplePages).PopupSide = (ePopupSide)4;
		((PopupItem)tbZoomMultiplePages).PopupType = (ePopupType)1;
		((BaseItem)tbZoomMultiplePages).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)ciMultiPagesUserControl });
		ciMultiPagesUserControl.AllowItemResize = true;
		ciMultiPagesUserControl.Control = (Control)(object)multiPagesUserControl;
		ciMultiPagesUserControl.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)ciMultiPagesUserControl).Name = "ciMultiPagesUserControl";
		((BaseItem)ciMultiPagesUserControl).Text = "controlContainerItem1";
		((Control)multiPagesUserControl).BackColor = Color.White;
		((Control)multiPagesUserControl).Location = new Point(0, 0);
		((Control)multiPagesUserControl).Name = "multiPagesUserControl";
		((Control)multiPagesUserControl).Size = new Size(161, 108);
		((Control)multiPagesUserControl).TabIndex = 0;
		tbZoomPageWidth.ImageIndex = 15;
		((BaseItem)tbZoomPageWidth).Name = "tbZoomPageWidth";
		((BaseItem)tbZoomPageWidth).Shortcuts.Add((eShortcut)116);
		((BaseItem)tbZoomPageWidth).Click += ButtonZoomPageWidth_Click;
		((BaseItem)tbDotMatrix).BeginGroup = true;
		tbDotMatrix.ImageIndex = 33;
		((BaseItem)tbDotMatrix).Name = "tbDotMatrix";
		((BaseItem)tbDotMatrix).Shortcuts.Add((eShortcut)131140);
		((BaseItem)tbDotMatrix).Text = "DotMatrix";
		((BaseItem)tbDotMatrix).Visible = false;
		((BaseItem)tbDotMatrix).Click += ButtonDotMatrix_Click;
		((BaseItem)tbClose).BeginGroup = true;
		tbClose.ButtonStyle = (eButtonStyle)2;
		tbClose.FixedSize = new Size(70, 1);
		((BaseItem)tbClose).Name = "tbClose";
		((BaseItem)tbClose).Text = "Close";
		((BaseItem)tbClose).Visible = false;
		((BaseItem)tbClose).Click += ButtonClose_Click;
		((BaseItem)tbHelp).ItemAlignment = (eItemAlignment)1;
		((BaseItem)tbHelp).Name = "tbHelp";
		((BaseItem)tbHelp).Shortcuts.Add((eShortcut)112);
		((BaseItem)tbHelp).Click += This_HelpClick;
		timerZoomSlider.Tick += TimerZoomSlider_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelView);
		((Control)this).Name = "StiViewerControl";
		((Control)this).Size = new Size(669, 443);
		((UserControl)this).Load += This_Load;
		((Control)this).Resize += This_Resize;
		((Control)panelView).ResumeLayout(false);
		((Control)view).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar1).EndInit();
		((Control)panelHorizontal).ResumeLayout(false);
		((ISupportInitialize)tbFindBar).EndInit();
		tbFindBar.ResumeLayout(false);
		((ISupportInitialize)tbStatusBar).EndInit();
		((ISupportInitialize)tbTool).EndInit();
		((Control)this).ResumeLayout(false);
	}

	void IStiViewerAccessor.CheckParametersButton()
	{
		tbParameters.Checked = true;
	}

	void IStiViewerAccessor.UncheckParametersButton()
	{
		tbParameters.Checked = false;
	}

	void IStiViewerAccessor.EnableParametersButton()
	{
		((BaseItem)tbParameters).Enabled = true;
	}

	IStiReport IStiViewerAccessor.GetReport()
	{
		return (IStiReport)(object)report;
	}

	IStiReport IStiViewerAccessor.GetCurrentReport()
	{
		return (IStiReport)(object)CurrentReport;
	}

	Panel IStiViewerAccessor.GetPanelDialogs()
	{
		return panelDialogs;
	}

	IContainer IStiViewerAccessor.GetIContainer()
	{
		return components;
	}

	bool IStiViewerAccessor.OnSubmit()
	{
		StiCacheCleaner.Clean((IStiApp)(object)Report);
		return false;
	}

	bool IStiViewerAccessor.OnReset()
	{
		return false;
	}

	void IStiViewerAccessor.OnShowParameters()
	{
	}

	public void ApplyStyle()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		StiThumbsControl stiThumbsControl = lbThumbs;
		Color backColor = (((Control)view).BackColor = StiGuiHelper.GetWorkspaceColor());
		((Control)stiThumbsControl).BackColor = backColor;
		StiViewerTreeView stiViewerTreeView = tvBookmarks;
		StiThumbsControl stiThumbsControl2 = lbThumbs;
		StiPageViewControl stiPageViewControl = view;
		BorderStyle val = (BorderStyle)0;
		((Panel)stiPageViewControl).BorderStyle = (BorderStyle)0;
		BorderStyle borderStyle = (((Panel)stiThumbsControl2).BorderStyle = val);
		((TreeView)stiViewerTreeView).BorderStyle = borderStyle;
		tabStripReports.Style = (eTabStripStyle)11;
		StiGuiHelper.ApplyStyle((Control)(object)this);
		Panel obj = panelDialogs;
		backColor = (((Control)listResources).BackColor = StiGuiHelper.GetControlColor());
		((Control)obj).BackColor = backColor;
		Splitter obj2 = splBookmarks;
		Splitter obj3 = splParameters;
		Color color2 = (((Control)splThumbs).BackColor = StiColorUtils.Dark(Color.White, (byte)40));
		backColor = (((Control)obj3).BackColor = color2);
		((Control)obj2).BackColor = backColor;
	}

	public void Localize()
	{
		SuperToolTip.RemoveAllSuperTooltip();
		try
		{
			((BaseItem)tbSave).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Save"));
			((BaseItem)tbSendEMail).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "SendEMail"));
			((BaseItem)tbPrint).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Print"));
			((BaseItem)tbOpen).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((BaseItem)tbSaveDocumentFile).Text = StiLocalization.Get("FormViewer", "DocumentFile").Replace("...", "");
			((BaseItem)tbSendEMailDocumentFile).Text = StiLocalization.Get("FormViewer", "DocumentFile").Replace("...", "");
			((BaseItem)tbFind).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Find"));
			((BaseItem)tbEditor).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Editor"));
			((BaseItem)tbBookmarks).Text = StiLocalization.Get("FormViewer", "Bookmarks");
			((BaseItem)tbThumbs).Text = StiLocalization.Get("FormViewer", "Thumbnails");
			((BaseItem)tbParameters).Text = StiLocalization.Get("FormViewer", "Parameters");
			((BaseItem)tbResources).Text = StiLocalization.Get("PropertyMain", "Resources");
			((BaseItem)tbClose).Text = StiLocalization.Get("FormViewer", "Close");
			((BaseItem)tbFullScreen).Text = StiLocalization.Get("FormViewer", "FullScreen");
			((BaseItem)tbDotMatrix).Text = StiLocalization.Get("Export", "DotMatrixMode");
			((BaseItem)tbPageSize).Text = StiLocalization.Get("FormViewer", "PageSize");
			((BaseItem)cmZoomPageWidth).Text = StiLocalization.Get("FormViewer", "ZoomPageWidth");
			((BaseItem)cmZoomOnePage).Text = StiLocalization.Get("FormViewer", "ZoomOnePage");
			((BaseItem)cmZoomTwoPages).Text = StiLocalization.Get("FormViewer", "ZoomTwoPages");
			((BaseItem)tbPageNew).Text = StiLocalization.Get("FormViewer", "PageNew");
			((BaseItem)tbPageDelete).Text = StiLocalization.Get("FormViewer", "PageDelete");
			((BaseItem)tbPageDesign).Text = StiLocalization.Get("FormViewer", "PageDesign");
			((BaseItem)miCollapseAll).Text = StiLocalization.Get("FormViewer", "CollapseAll");
			((BaseItem)miExpandAll).Text = StiLocalization.Get("FormViewer", "ExpandAll");
			((BaseItem)tbPageControl).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "GoToPage"));
			((BaseItem)tbViewModeSinglePage).Text = StiLocalization.Get("FormViewer", "PageViewModeSinglePage");
			((BaseItem)tbViewModeContinuous).Text = StiLocalization.Get("FormViewer", "PageViewModeContinuous");
			((BaseItem)tbViewModeMultiplePages).Text = StiLocalization.Get("FormViewer", "PageViewModeMultiplePages");
			((BaseItem)tbFirstPage).Text = StiLocalization.Get("FormViewer", "FirstPage");
			((BaseItem)tbPrevPage).Text = StiLocalization.Get("FormViewer", "PrevPage");
			((BaseItem)tbNextPage).Text = StiLocalization.Get("FormViewer", "NextPage");
			((BaseItem)tbLastPage).Text = StiLocalization.Get("FormViewer", "LastPage");
			((BaseItem)liFindWhat).Text = StiHint.CreateHint(StiLocalization.Get("FormViewerFind", "FindWhat"));
			((BaseItem)tbFindNext).Text = StiHint.CreateHint(StiLocalization.Get("FormViewerFind", "FindNext"));
			((BaseItem)cbFindMatchCase).Text = StiHint.CreateHint(StiLocalization.Get("Editor", "MatchCase"));
			((BaseItem)cbFindMatchWholeWord).Text = StiHint.CreateHint(StiLocalization.Get("Editor", "MatchWholeWord"));
			((BaseItem)tbFindNext).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewerFind", "FindNext"));
			((BaseItem)cbFindMatchCase).Tooltip = StiHint.CreateHint(StiLocalization.Get("Editor", "MatchCase"));
			((BaseItem)cbFindMatchWholeWord).Tooltip = StiHint.CreateHint(StiLocalization.Get("Editor", "MatchWholeWord"));
			LocalizeSuperToolTip();
		}
		catch
		{
		}
	}

	private void LocalizeSuperToolTip()
	{
		if (StiOptions.Viewer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			string url = "user-manual/index.html?viewer_reports.htm#toolbar";
			SetToolTipEx((BaseItem)(object)tbPrint, globalMeasureGraphics, ((BaseItem)tbPrint).Text + " (Ctrl + P)", "Print", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbOpen, globalMeasureGraphics, ((BaseItem)tbOpen).Text + " (Ctrl + O)", "Open", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbSave, globalMeasureGraphics, ((BaseItem)tbSave).Text, "Save", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbSendEMail, globalMeasureGraphics, ((BaseItem)tbSendEMail).Text, "SendEMail", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbPageNew, globalMeasureGraphics, ((BaseItem)tbPageNew).Text + " (Ctrl + Shift + N)", "PageNew", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbPageDelete, globalMeasureGraphics, ((BaseItem)tbPageDelete).Text + " (Ctrl + Shift + D)", "PageDelete", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbPageDesign, globalMeasureGraphics, ((BaseItem)tbPageDesign).Text.Replace("...", string.Empty) + " (Ctrl + Shift + E)", "PageDesign", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbPageSize, globalMeasureGraphics, ((BaseItem)tbPageSize).Text.Replace("...", "") + " (Ctrl + Shift + S)", "PageSize", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbClose, globalMeasureGraphics, ((BaseItem)tbClose).Text, "Close", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbDotMatrix, globalMeasureGraphics, ((BaseItem)tbDotMatrix).Text, "DotMatrixMode", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbZoomPageWidth, globalMeasureGraphics, StiLocalization.Get("FormViewer", "ZoomPageWidth") + " (F5)", "ZoomPageWidth", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbZoomOnePage, globalMeasureGraphics, StiLocalization.Get("FormViewer", "ZoomOnePage") + " (F3)", "ZoomOnePage", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbZoomTwoPages, globalMeasureGraphics, StiLocalization.Get("FormViewer", "ZoomTwoPages") + " (F4)", "ZoomTwoPages", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbZoomMultiplePages, globalMeasureGraphics, StiLocalization.Get("FormViewer", "ZoomMultiplePages"), "ZoomMultiplePages", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbFullScreen, globalMeasureGraphics, ((BaseItem)tbFullScreen).Text + " (F2)", "FullScreen", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbBookmarks, globalMeasureGraphics, ((BaseItem)tbBookmarks).Text + " (Ctrl + B)", "Bookmarks", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbThumbs, globalMeasureGraphics, ((BaseItem)tbThumbs).Text + " (Ctrl + T)", "Thumbnails", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbResources, globalMeasureGraphics, ((BaseItem)tbResources).Text + " (Ctrl + R)", "Resources", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbParameters, globalMeasureGraphics, ((BaseItem)tbParameters).Text + " (Ctrl + Shift + P)", "Parameters", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbEditor, globalMeasureGraphics, ((BaseItem)tbEditor).Text + " (Ctrl + E)", "ToolEditor", headerVisible: true, url);
			url = "user-manual/index.html?viewer_reports.htm#statusbar";
			SetToolTipEx((BaseItem)(object)tbFirstPage, globalMeasureGraphics, ((BaseItem)tbFirstPage).Text, "PageFirst", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbPrevPage, globalMeasureGraphics, ((BaseItem)tbPrevPage).Text, "PagePrevious", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbNextPage, globalMeasureGraphics, ((BaseItem)tbNextPage).Text, "PageNext", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbLastPage, globalMeasureGraphics, ((BaseItem)tbLastPage).Text, "PageLast", headerVisible: true, url);
			SetToolTipEx((BaseItem)(object)tbPageControl, globalMeasureGraphics, ((BaseItem)tbPageControl).Text + " (Ctrl + G)", "PageGoTo", headerVisible: true, url);
			url = "user-manual/index.html?viewer_reports.htm#searchpanel";
			SetToolTipEx((BaseItem)(object)tbFind, globalMeasureGraphics, ((BaseItem)tbFind).Text + " (Ctrl + F)", "Find", headerVisible: true, url);
		}
		else
		{
			((BaseItem)tbPrint).Tooltip = ((BaseItem)tbPrint).Text;
			((BaseItem)tbOpen).Tooltip = ((BaseItem)tbOpen).Text;
			((BaseItem)tbSave).Tooltip = ((BaseItem)tbSave).Text;
			((BaseItem)tbSendEMail).Tooltip = ((BaseItem)tbSendEMail).Text;
			((BaseItem)tbPageNew).Tooltip = ((BaseItem)tbPageNew).Text;
			((BaseItem)tbPageDelete).Tooltip = ((BaseItem)tbPageDelete).Text;
			((BaseItem)tbPageDesign).Tooltip = ((BaseItem)tbPageDesign).Text;
			((BaseItem)tbPageSize).Tooltip = ((BaseItem)tbPageSize).Text;
			((BaseItem)tbBookmarks).Tooltip = ((BaseItem)tbBookmarks).Text;
			((BaseItem)tbThumbs).Tooltip = ((BaseItem)tbThumbs).Text;
			((BaseItem)tbParameters).Tooltip = ((BaseItem)tbParameters).Text;
			((BaseItem)tbResources).Tooltip = ((BaseItem)tbResources).Text;
			((BaseItem)tbFind).Tooltip = ((BaseItem)tbFind).Text;
			((BaseItem)tbEditor).Tooltip = ((BaseItem)tbEditor).Text;
			((BaseItem)tbFullScreen).Tooltip = ((BaseItem)tbFullScreen).Text;
			((BaseItem)tbZoomOnePage).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomOnePage"));
			((BaseItem)tbZoomTwoPages).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomTwoPages"));
			((BaseItem)tbZoomMultiplePages).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomMultiplePages"));
			((BaseItem)tbZoomPageWidth).Tooltip = StiHint.CreateHint(StiLocalization.Get("FormViewer", "ZoomPageWidth"));
			((BaseItem)tbFirstPage).Tooltip = ((BaseItem)tbFirstPage).Text;
			((BaseItem)tbPrevPage).Tooltip = ((BaseItem)tbPrevPage).Text;
			((BaseItem)tbPageControl).Tooltip = ((BaseItem)tbPageControl).Text;
			((BaseItem)tbNextPage).Tooltip = ((BaseItem)tbNextPage).Text;
			((BaseItem)tbLastPage).Tooltip = ((BaseItem)tbLastPage).Text;
			((BaseItem)tbViewModeSinglePage).Tooltip = ((BaseItem)tbViewModeSinglePage).Text;
			((BaseItem)tbViewModeContinuous).Tooltip = ((BaseItem)tbViewModeContinuous).Text;
			((BaseItem)tbViewModeMultiplePages).Tooltip = ((BaseItem)tbViewModeMultiplePages).Text;
			((BaseItem)tbDotMatrix).Tooltip = ((BaseItem)tbDotMatrix).Text;
			((BaseItem)tbClose).Tooltip = ((BaseItem)tbClose).Text;
			((BaseItem)tbFindClose).Tooltip = ((BaseItem)tbFindClose).Text;
			((BaseItem)tbFindPrevious).Tooltip = ((BaseItem)tbFindPrevious).Text;
			((BaseItem)tbFindNext).Tooltip = ((BaseItem)tbFindNext).Text;
			((BaseItem)cbFindMatchCase).Tooltip = ((BaseItem)cbFindMatchCase).Text;
			((BaseItem)cbFindMatchWholeWord).Tooltip = ((BaseItem)cbFindMatchWholeWord).Text;
		}
	}

	private void SetToolTipEx(BaseItem item, Graphics g, string header, string localizeName, bool headerVisible, string url)
	{
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		if (item == null)
		{
			return;
		}
		string text = StiLocalization.Get("HelpViewer", localizeName);
		SizeF sizeF = SuperToolTip.MeasureHeaderText(g, header);
		SizeF sizeF2 = SuperToolTip.MeasureBodyText(g, text, (int)(280.0 * ScaleX));
		int num = ((sizeF.Width > sizeF2.Width) ? ((int)Math.Round((double)sizeF.Width + 0.5) + (int)(15.0 * ScaleX)) : ((int)Math.Round((double)sizeF2.Width + 0.5) + (int)(25.0 * ScaleX)));
		int num2 = 0;
		SuperTooltipInfo val;
		if (StiOptions.Appearance.ShowTooltipsHelp)
		{
			string text2 = StiLocalization.Get("HelpDesigner", "PressF1");
			val = new SuperTooltipInfo(header, text2, text, (Image)null, (Image)null, CurrentTooltipColor)
			{
				Hyperlink = url,
				BodyImage = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Help" + StiScale.StepName + ".png")
			};
			SizeF sizeF3 = SuperToolTip.MeasureFooterText(g, text2);
			sizeF3.Width += (int)(40.0 * ScaleX);
			if (sizeF3.Width > (float)num)
			{
				num = (int)sizeF3.Width;
			}
			num2 = (int)(27.0 * ScaleY);
		}
		else
		{
			val = new SuperTooltipInfo(header, (string)null, text, (Image)null, (Image)null, CurrentTooltipColor);
		}
		val.HeaderVisible = headerVisible;
		val.CustomSize = new Size(num, (int)Math.Round(sizeF2.Height + sizeF.Height) + num2 + (int)(15.0 * ScaleY));
		SuperToolTip.SetSuperTooltip((IComponent)item, val);
	}

	private void SetToolTipEx(BaseItem item, Graphics g, string header, string localizeName, Size size, Image image, string url)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		if (item == null)
		{
			return;
		}
		string text = StiLocalization.Get("HelpViewer", localizeName);
		SuperTooltipInfo val;
		if (StiOptions.Appearance.ShowTooltipsHelp)
		{
			string text2 = StiLocalization.Get("HelpDesigner", "PressF1");
			SizeF sizeF = SuperToolTip.MeasureFooterText(g, text2);
			sizeF.Width += (int)(40.0 * ScaleX);
			val = new SuperTooltipInfo(header, text2, text, image, (Image)null, CurrentTooltipColor)
			{
				Hyperlink = url,
				BodyImage = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Help" + StiScale.StepName + ".png")
			};
			if (sizeF.Width > (float)size.Width)
			{
				size.Width = (int)sizeF.Width;
			}
		}
		else
		{
			val = new SuperTooltipInfo(header, (string)null, text, image, (Image)null, CurrentTooltipColor);
		}
		val.CustomSize = size;
		SuperToolTip.SetSuperTooltip((IComponent)item, val);
	}

	[Browsable(false)]
	public void SetBookmarksButtonChecked(bool value)
	{
		tbBookmarks.Checked = value;
		UpdateControlsState();
	}

	[Browsable(false)]
	public void SetThumbsButtonChecked(bool value)
	{
		tbThumbs.Checked = value;
		UpdateControlsState();
	}

	[Browsable(false)]
	public void SetParametersButtonChecked(bool value)
	{
		tbParameters.Checked = value;
		UpdateControlsState();
	}

	[Browsable(false)]
	public void SetResourcesButtonChecked(bool value)
	{
		tbResources.Checked = value;
		UpdateControlsState();
	}

	private void DoScaling()
	{
		ButtonItem obj = tbSendEMail;
		obj.DropArrowShift += StiScale.I(StiScale.Factor - 1.0) * 2;
		cmZoomOnePage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomOnePage" + StiScale.StepName + ".png");
		cmZoomTwoPages.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomTwoPages" + StiScale.StepName + ".png");
		cmZoomPageWidth.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomPageWidth" + StiScale.StepName + ".png");
		miExpandAll.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ExpandAll" + StiScale.StepName + ".png");
		miCollapseAll.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.CollapseAll" + StiScale.StepName + ".png");
		tbFindClose.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.FindClose" + StiScale.StepName + ".png");
		tbFindNext.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.FindNext" + StiScale.StepName + ".png");
		tbFindPrevious.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.FindPrevious" + StiScale.StepName + ".png");
		tbFirstPage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageFirst" + StiScale.StepName + ".png");
		tbPrevPage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PagePrevious" + StiScale.StepName + ".png");
		tbNextPage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageNext" + StiScale.StepName + ".png");
		tbLastPage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageLast" + StiScale.StepName + ".png");
		tbViewModeSinglePage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ViewModeSinglePage" + StiScale.StepName + ".png");
		tbViewModeContinuous.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ViewModeContinuous" + StiScale.StepName + ".png");
		tbViewModeMultiplePages.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ViewModeMultiplePages" + StiScale.StepName + ".png");
		tbPrint.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportPrint" + StiScale.StepName + ".png");
		tbSave.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportSave" + StiScale.StepName + ".png");
		tbOpen.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportOpen" + StiScale.StepName + ".png");
		tbSaveDocumentFile.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.SaveDocumentFile" + StiScale.StepName + ".png");
		tbSendEMail.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.SendEMail" + StiScale.StepName + ".png");
		tbSendEMailDocumentFile.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.SaveDocumentFile" + StiScale.StepName + ".png");
		tbPageNew.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageNew" + StiScale.StepName + ".png");
		tbPageDelete.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageDelete" + StiScale.StepName + ".png");
		tbPageDesign.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageDesign" + StiScale.StepName + ".png");
		tbPageSize.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.PageSize" + StiScale.StepName + ".png");
		tbBookmarks.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Bookmarks" + StiScale.StepName + ".png");
		tbParameters.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Parameters" + StiScale.StepName + ".png");
		tbResources.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Resources" + StiScale.StepName + ".png");
		tbThumbs.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Thumbnails" + StiScale.StepName + ".png");
		tbFind.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Find" + StiScale.StepName + ".png");
		tbEditor.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Editor" + StiScale.StepName + ".png");
		tbFullScreen.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.FullScreen" + StiScale.StepName + ".png");
		tbZoomOnePage.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomOnePage" + StiScale.StepName + ".png");
		tbZoomTwoPages.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomTwoPages" + StiScale.StepName + ".png");
		tbZoomMultiplePages.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomMultiplePages" + StiScale.StepName + ".png");
		tbZoomPageWidth.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ZoomPageWidth" + StiScale.StepName + ".png");
		tbDotMatrix.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.DotMatrix" + StiScale.StepName + ".png");
		tbHelp.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Help" + StiScale.StepName + ".png");
		tbFirstPage.ImagePaddingHorizontal = StiScale.I20;
		tbFirstPage.ImagePaddingVertical = StiScale.I10;
		tbPrevPage.ImagePaddingHorizontal = StiScale.I20;
		tbPrevPage.ImagePaddingVertical = StiScale.I10;
		tbNextPage.ImagePaddingHorizontal = StiScale.I20;
		tbNextPage.ImagePaddingVertical = StiScale.I10;
		tbLastPage.ImagePaddingHorizontal = StiScale.I20;
		tbLastPage.ImagePaddingVertical = StiScale.I10;
		tbViewModeContinuous.ImagePaddingHorizontal = StiScale.I20;
		tbViewModeContinuous.ImagePaddingVertical = StiScale.I10;
		tbViewModeMultiplePages.ImagePaddingHorizontal = StiScale.I20;
		tbViewModeMultiplePages.ImagePaddingVertical = StiScale.I10;
		tbViewModeSinglePage.ImagePaddingHorizontal = StiScale.I20;
		tbViewModeSinglePage.ImagePaddingVertical = StiScale.I10;
		lbZoom.Width = StiScale.I(60);
		siZoom.Width = StiScale.I(130);
	}

	private static string ProcessHyperlink(string hyperlink)
	{
		if (hyperlink.StartsWith("###"))
		{
			return hyperlink.Substring(3);
		}
		if (hyperlink.StartsWith("##"))
		{
			return hyperlink.Substring(2);
		}
		if (hyperlink.StartsWith("#"))
		{
			return hyperlink.Substring(1);
		}
		return hyperlink;
	}

	private void AddMainReportToTabs()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		TabItem val = new TabItem(components);
		val.Text = (string.IsNullOrEmpty(CurrentReport.ReportAlias) ? CurrentReport.ReportName : CurrentReport.ReportAlias);
		val.Tooltip = CurrentReport.ReportDescription;
		val.Click += NewItem_ClickClose;
		if (mainReport == null)
		{
			mainReport = Report;
		}
		Reports.Add(Report);
		tabStripReports.Tabs.Add(val);
	}

	public void AddReportToTabs(StiReport newReport, StiDrillDownMode mode, bool checkMainReportTab = true)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (newReport == null)
		{
			return;
		}
		if (checkMainReportTab && ((CollectionBase)(object)tabStripReports.Tabs).Count == 0)
		{
			AddMainReportToTabs();
		}
		TabItem val = new TabItem(components);
		val.Click += NewItem_ClickClose;
		val.Text = newReport.ReportAlias;
		val.Tooltip = newReport.ReportDescription;
		val.Tag = newReport;
		Reports.Add(newReport);
		tabStripReports.Tabs.Add(val);
		if (StiOptions.Viewer.DrillDownSingleTab || mode == StiDrillDownMode.SinglePage)
		{
			while (((CollectionBase)(object)tabStripReports.Tabs).Count > 2)
			{
				Reports.RemoveAt(1);
				((CollectionBase)(object)tabStripReports.Tabs).RemoveAt(1);
			}
		}
	}

	private void TimerUpdateDashboardTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		Timer val = (Timer)sender;
		val.Stop();
		if (mainReport == null || !(val.Tag is TabItem) || ((TabItem)val.Tag).Parent == null || !(((TabItem)val.Tag).Tag is StiReport))
		{
			((Component)(object)val).Dispose();
			return;
		}
		StiReport stiReport = (StiReport)((TabItem)val.Tag).Tag;
		if (stiReport.RenderedPages.Count != 1)
		{
			((Component)(object)val).Dispose();
			return;
		}
		string name = stiReport.RenderedPages[0].Name;
		foreach (StiPage page in mainReport.Pages)
		{
			page.Enabled = page.Name == name;
		}
		mainReport.Render(showProgress: true);
		stiReport.RenderedPages.Clear();
		foreach (StiPage page2 in mainReport.Pages)
		{
			page2.Enabled = true;
			if (page2.Name == name)
			{
				stiReport.RenderedPages.Add(page2);
			}
		}
	}

	internal bool AllowExportService(StiExportService service)
	{
		if (service is StiPdfExportService && StiOptions.Viewer.Windows.Exports.ShowPdf)
		{
			return true;
		}
		if (service is StiXpsExportService && StiOptions.Viewer.Windows.Exports.ShowXps)
		{
			return true;
		}
		if (service is StiPpt2007ExportService && StiOptions.Viewer.Windows.Exports.ShowPpt2007)
		{
			return true;
		}
		if (service is StiHtmlExportService && StiOptions.Viewer.Windows.Exports.ShowHtml)
		{
			return true;
		}
		if (service is StiHtml5ExportService && StiOptions.Viewer.Windows.Exports.ShowHtml5)
		{
			return true;
		}
		if (service is StiMhtExportService && StiOptions.Viewer.Windows.Exports.ShowMht)
		{
			return true;
		}
		if (service is StiTxtExportService && StiOptions.Viewer.Windows.Exports.ShowText)
		{
			return true;
		}
		if (service is StiRtfExportService && StiOptions.Viewer.Windows.Exports.ShowRtf)
		{
			return true;
		}
		if (service is StiWord2007ExportService && StiOptions.Viewer.Windows.Exports.ShowWord2007)
		{
			return true;
		}
		if (service is StiOdtExportService && StiOptions.Viewer.Windows.Exports.ShowOdt)
		{
			return true;
		}
		if (service is StiExcelExportService && StiOptions.Viewer.Windows.Exports.ShowExcel)
		{
			return true;
		}
		if (service is StiExcelXmlExportService && StiOptions.Viewer.Windows.Exports.ShowExcelXml)
		{
			return true;
		}
		if (service is StiExcel2007ExportService && StiOptions.Viewer.Windows.Exports.ShowExcel2007)
		{
			return true;
		}
		if (service is StiOdsExportService && StiOptions.Viewer.Windows.Exports.ShowOds)
		{
			return true;
		}
		if (service is StiDataExportService && StiOptions.Viewer.Windows.Exports.ShowData)
		{
			return true;
		}
		if (service is StiImageExportService && StiOptions.Viewer.Windows.Exports.ShowImage)
		{
			return true;
		}
		return false;
	}

	private void BuildExportMenu()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		if (!StiOptions.Services.Exports.Any((StiExportService s) => ((StiService)s).ServiceEnabled))
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		StiStringCollection val = new StiStringCollection();
		foreach (StiExportService item in StiOptions.Services.Exports.Where((StiExportService s) => ((StiService)s).ServiceEnabled))
		{
			if (AllowExportService(item) && hashtable[item.GroupCategory] == null)
			{
				hashtable.Add(item.GroupCategory, item.GroupCategory);
				val.Add(item.GroupCategory);
			}
		}
		foreach (string item2 in (CollectionBase)(object)val)
		{
			int num = 0;
			foreach (StiExportService item3 in StiOptions.Services.Exports.Where((StiExportService s) => ((StiService)s).ServiceEnabled))
			{
				if (AllowExportService(item3) && !(item2 != item3.GroupCategory))
				{
					string text2 = ((item3 is StiImageExportService) ? StiLocalization.Get("Export", "ExportTypeImageFile") : ((!(item3 is StiDataExportService)) ? item3.ExportNameInMenu : StiLocalization.Get("Export", "ExportTypeDataFile")));
					if ((!(item3.GroupCategory == "Image") || item3 is StiImageExportService) && !(item3.GroupCategory == "Meta") && (!(item3.GroupCategory == "Excel") || item3 is StiOdsExportService || item3 is StiExcelExportService) && (!(item3.GroupCategory == "Web") || item3 is StiHtmlExportService) && (!(item3.GroupCategory == "Data") || item3 is StiDataExportService))
					{
						text2 = text2.Replace("...", "");
						ButtonItem val2 = new ButtonItem(item3.ExportNameInMenu, text2)
						{
							Image = (Image)(object)GetImageExport(item3),
							ImageFixedSize = new Size(StiScale.XXI(16.0), StiScale.YYI(16.0)),
							BeginGroup = (num == 0),
							Tag = item3
						};
						((BaseItem)val2).Click += ItemExport_Click;
						((BaseItem)tbSave).SubItems.Add((BaseItem)(object)val2);
						ButtonItem val3 = new ButtonItem(item3.ExportNameInMenu, text2)
						{
							Image = (Image)(object)GetImageExport(item3),
							ImageFixedSize = new Size(StiScale.XXI(16.0), StiScale.YYI(16.0)),
							BeginGroup = (num == 0),
							Tag = item3
						};
						((BaseItem)val3).Click += ItemSendEMail_Click;
						((BaseItem)tbSendEMail).SubItems.Add((BaseItem)(object)val3);
					}
					num++;
				}
			}
		}
	}

	private Bitmap GetImageExport(StiExportService service)
	{
		Bitmap val = null;
		string pathImageOffice2013FromBitmapService = Stimulsoft.Report.Export.StiImageHelper.GetPathImageOffice2013FromBitmapService((StiService)(object)service, StiScale.Step);
		if (!string.IsNullOrEmpty(pathImageOffice2013FromBitmapService))
		{
			val = StiImageUtils.GetImage("Stimulsoft.Report", pathImageOffice2013FromBitmapService);
		}
		if (val == null)
		{
			val = StiServiceBitmapAttribute.GetImage(((object)service).GetType());
		}
		return val;
	}

	internal void UpdateControlsState()
	{
		if (tbTool.Items == null)
		{
			return;
		}
		((BaseItem)tbPrint).Visible = GetPrintVisibility;
		((BaseItem)tbOpen).Visible = GetOpenVisibility;
		((BaseItem)tbSave).Visible = GetSaveVisibility;
		((BaseItem)tbSendEMail).Visible = GetSendEMailVisibility;
		((BaseItem)tbBookmarks).Visible = GetBookmarksPanelVisibility;
		StiViewerTreeView stiViewerTreeView = tvBookmarks;
		bool visible = (((Control)splBookmarks).Visible = GetBookmarksPanelVisibility && GetBookmarksButtonChecked);
		((Control)stiViewerTreeView).Visible = visible;
		((BaseItem)tbThumbs).Visible = GetThumbsPanelVisibility;
		((BaseItem)tbParameters).Visible = GetParametersButtonVisibility;
		((BaseItem)tbResources).Visible = GetResourcesButtonVisibility;
		((Control)splThumbs).Visible = GetThumbsPanelVisibility && GetThumbsButtonChecked;
		((Control)lbThumbs).Visible = GetThumbsPanelVisibility && GetThumbsButtonChecked;
		((Control)scrollBarThumbs).Visible = GetThumbsPanelVisibility && GetThumbsButtonChecked;
		((BaseItem)tbFind).Visible = GetFindVisibility;
		((BaseItem)tbEditor).Visible = GetEditorVisibility;
		((BaseItem)tbFullScreen).Visible = GetFullScreenVisibility;
		((BaseItem)tbZoomOnePage).Visible = GetZoomOnePageVisibility;
		((BaseItem)tbZoomTwoPages).Visible = GetZoomTwoPagesVisibility;
		((BaseItem)tbZoomMultiplePages).Visible = GetZoomMultiplePagesVisibility;
		((BaseItem)tbZoomPageWidth).Visible = GetZoomPageWidthVisibility;
		LabelItem obj = lbZoom;
		visible = (((BaseItem)siZoom).Visible = GetSliderZoomVisibility);
		((BaseItem)obj).Visible = visible;
		((BaseItem)tbViewModeSinglePage).Visible = GetViewModeSinglePageVisibility;
		((BaseItem)tbViewModeContinuous).Visible = GetViewModeContinuousVisibility;
		((BaseItem)tbViewModeMultiplePages).Visible = GetViewModeMultiplePagesVisibility;
		((BaseItem)tbDotMatrix).Visible = GetDotMatrixModeButtonVisibility;
		((BaseItem)tbFirstPage).Visible = GetFirstPageVisibility;
		((BaseItem)tbPrevPage).Visible = GetPreviousPageVisibility;
		((BaseItem)icPageControl).Visible = GetPageControlVisibility;
		((BaseItem)tbNextPage).Visible = GetNextPageVisibility;
		((BaseItem)tbLastPage).Visible = GetLastPageVisibility;
		((BaseItem)tbPageControl).Visible = GetPageControlVisibility;
		((BaseItem)tbPageNew).Visible = GetPageNewVisibility;
		((BaseItem)tbPageDelete).Visible = GetPageDeleteVisibility;
		((BaseItem)tbPageDesign).Visible = GetPageDesignVisibility;
		((BaseItem)tbPageSize).Visible = GetPageSizeVisibility;
		((BaseItem)tbClose).Visible = GetCloseVisibility;
		tbTool.Visible = !(this is StiRibbonViewerControl) && GetToolbarVisibility;
		tbStatusBar.Visible = GetStatusBarVisibility;
		((Control)panelHorizontal).Visible = GetHorScrollBarVisibility;
		((Control)vScrollBar).Visible = GetVertScrollBarVisibility;
		ButtonItem obj2 = contextMenuPreview;
		ButtonItem obj3 = cm10;
		ButtonItem obj4 = cm25;
		ButtonItem obj5 = cm50;
		ButtonItem obj6 = cm75;
		ButtonItem obj7 = cm100;
		ButtonItem obj8 = cm150;
		ButtonItem obj9 = cm200;
		ButtonItem obj10 = cmZoomOnePage;
		ButtonItem obj11 = cmZoomPageWidth;
		bool flag2 = (((BaseItem)cmZoomTwoPages).Visible = GetContextMenuVisibility);
		bool flag4 = (((BaseItem)obj11).Visible = flag2);
		bool flag6 = (((BaseItem)obj10).Visible = flag4);
		bool flag8 = (((BaseItem)obj9).Visible = flag6);
		bool flag10 = (((BaseItem)obj8).Visible = flag8);
		bool flag12 = (((BaseItem)obj7).Visible = flag10);
		bool flag14 = (((BaseItem)obj6).Visible = flag12);
		bool flag16 = (((BaseItem)obj5).Visible = flag14);
		bool flag18 = (((BaseItem)obj4).Visible = flag16);
		visible = (((BaseItem)obj3).Visible = flag18);
		((BaseItem)obj2).Visible = visible;
		if (((BaseItem)contextMenuPreview).Visible)
		{
			contextMenuBar1.SetContextMenuEx((Control)(object)view, (BaseItem)(object)contextMenuPreview);
		}
		else
		{
			contextMenuBar1.SetContextMenuEx((Control)(object)view, (BaseItem)null);
		}
		((BaseItem)tbHelp).Visible = StiOptions.Appearance.ShowFormsHelp;
		((BaseItem)tbPrint).Enabled = GetPrintEnabled;
		((BaseItem)tbOpen).Enabled = GetOpenEnabled;
		((BaseItem)tbSave).Enabled = GetSaveEnabled;
		((BaseItem)tbSendEMail).Enabled = GetSendEMailEnabled;
		((BaseItem)tbBookmarks).Enabled = GetBookmarksPanelEnabled;
		((BaseItem)tbThumbs).Enabled = GetThumbsPanelEnabled;
		((BaseItem)tbFind).Enabled = GetFindEnabled;
		((BaseItem)tbEditor).Enabled = GetEditorEnabled;
		((BaseItem)tbResources).Enabled = GetResourcesPanelEnabled;
		((BaseItem)tbFullScreen).Enabled = GetFullScreenEnabled;
		((BaseItem)tbZoomOnePage).Enabled = GetZoomOnePageEnabled;
		((BaseItem)tbZoomTwoPages).Enabled = GetZoomTwoPagesEnabled;
		((BaseItem)tbZoomMultiplePages).Enabled = GetZoomMultiplePagesEnabled;
		((BaseItem)tbZoomPageWidth).Enabled = GetZoomPageWidthEnabled;
		LabelItem obj12 = lbZoom;
		visible = (((BaseItem)siZoom).Enabled = GetSliderZoomEnabled);
		((BaseItem)obj12).Enabled = visible;
		((BaseItem)tbViewModeSinglePage).Enabled = GetViewModeSinglePageEnabled;
		((BaseItem)tbViewModeContinuous).Enabled = GetViewModeContinuousEnabled;
		((BaseItem)tbViewModeMultiplePages).Enabled = GetViewModeMultiplePagesEnabled;
		((BaseItem)tbDotMatrix).Enabled = GetDotMatrixModeButtonEnabled;
		((BaseItem)tbFirstPage).Enabled = GetFirstPageEnabled;
		((BaseItem)tbPrevPage).Enabled = GetPreviousPageEnabled;
		((BaseItem)icPageControl).Enabled = GetPageControlEnabled;
		((BaseItem)tbNextPage).Enabled = GetNextPageEnabled;
		((BaseItem)tbLastPage).Enabled = GetLastPageEnabled;
		((BaseItem)tbPageControl).Enabled = GetPageControlEnabled;
		((BaseItem)tbPageNew).Enabled = GetPageNewEnabled;
		((BaseItem)tbPageDelete).Enabled = GetPageDeleteEnabled;
		((BaseItem)tbPageDesign).Enabled = GetPageDesignEnabled;
		((BaseItem)tbPageSize).Enabled = GetPageSizeEnabled;
		((BaseItem)tbClose).Enabled = GetCloseEnabled;
		((Control)panelHorizontal).Enabled = GetHorScrollBarEnabled;
		((Control)vScrollBar).Enabled = GetVertScrollBarEnabled;
		ButtonItem obj13 = contextMenuPreview;
		ButtonItem obj14 = cm10;
		ButtonItem obj15 = cm25;
		ButtonItem obj16 = cm50;
		ButtonItem obj17 = cm75;
		ButtonItem obj18 = cm100;
		ButtonItem obj19 = cm150;
		flag8 = (((BaseItem)cm200).Enabled = GetContextMenuEnabled);
		flag10 = (((BaseItem)obj19).Enabled = flag8);
		flag12 = (((BaseItem)obj18).Enabled = flag10);
		flag14 = (((BaseItem)obj17).Enabled = flag12);
		flag16 = (((BaseItem)obj16).Enabled = flag14);
		flag18 = (((BaseItem)obj15).Enabled = flag16);
		visible = (((BaseItem)obj14).Enabled = flag18);
		((BaseItem)obj13).Enabled = visible;
		((BaseItem)cmZoomOnePage).Enabled = GetZoomOnePageEnabled;
		((BaseItem)cmZoomPageWidth).Enabled = GetZoomPageWidthEnabled;
		((BaseItem)cmZoomTwoPages).Enabled = GetZoomTwoPagesEnabled;
		ButtonItem obj20 = tbFindClose;
		LabelItem obj21 = liFindWhat;
		ButtonItem obj22 = tbFindPrevious;
		ButtonItem obj23 = tbFindNext;
		StiViewerTextBox stiViewerTextBox = tbFindValue;
		CheckBoxItem obj24 = cbFindMatchCase;
		flag10 = (((BaseItem)cbFindMatchWholeWord).Enabled = GetFindBarEnabled);
		flag12 = (((BaseItem)obj24).Enabled = flag10);
		flag14 = (((Control)stiViewerTextBox).Enabled = flag12);
		flag16 = (((BaseItem)obj23).Enabled = flag14);
		flag18 = (((BaseItem)obj22).Enabled = flag16);
		visible = (((BaseItem)obj21).Enabled = flag18);
		((BaseItem)obj20).Enabled = visible;
		tbFind.Checked = GetFindChecked;
		tbEditor.Checked = GetEditorChecked;
		tbZoomOnePage.Checked = GetZoomOnePageChecked;
		tbZoomTwoPages.Checked = GetZoomTwoPagesChecked;
		tbZoomMultiplePages.Checked = GetZoomMultiplePagesChecked;
		tbZoomPageWidth.Checked = GetZoomPageWidthChecked;
		cmZoomOnePage.Checked = GetZoomOnePageChecked;
		cmZoomPageWidth.Checked = GetZoomPageWidthChecked;
		cmZoomTwoPages.Checked = GetZoomTwoPagesChecked;
		cm10.Checked = GetZoom10Checked;
		cm25.Checked = GetZoom25Checked;
		cm50.Checked = GetZoom50Checked;
		cm75.Checked = GetZoom75Checked;
		cm100.Checked = GetZoom100Checked;
		cm150.Checked = GetZoom150Checked;
		cm200.Checked = GetZoom200Checked;
		if (!((BaseItem)tbPageNew).Visible)
		{
			if (((BaseItem)tbPageDelete).Visible)
			{
				((BaseItem)tbPageDelete).BeginGroup = true;
			}
			else if (((BaseItem)tbPageDesign).Visible)
			{
				((BaseItem)tbPageDesign).BeginGroup = true;
			}
			else if (((BaseItem)tbPageSize).Visible)
			{
				((BaseItem)tbPageSize).BeginGroup = true;
			}
		}
		else
		{
			if (((BaseItem)tbPageDelete).BeginGroup)
			{
				((BaseItem)tbPageDelete).BeginGroup = false;
			}
			if (((BaseItem)tbPageDesign).BeginGroup)
			{
				((BaseItem)tbPageDesign).BeginGroup = false;
			}
			if (((BaseItem)tbPageSize).BeginGroup)
			{
				((BaseItem)tbPageSize).BeginGroup = false;
			}
		}
		if (!((BaseItem)tbBookmarks).Visible)
		{
			if (((BaseItem)tbParameters).Visible)
			{
				((BaseItem)tbParameters).BeginGroup = true;
			}
			else if (((BaseItem)tbThumbs).Visible)
			{
				((BaseItem)tbThumbs).BeginGroup = true;
			}
			else if (((BaseItem)tbResources).Visible)
			{
				((BaseItem)tbResources).BeginGroup = true;
			}
		}
		else if (((BaseItem)tbParameters).BeginGroup)
		{
			((BaseItem)tbParameters).BeginGroup = false;
		}
		else if (((BaseItem)tbThumbs).BeginGroup)
		{
			((BaseItem)tbThumbs).BeginGroup = false;
		}
		else if (((BaseItem)tbResources).BeginGroup)
		{
			((BaseItem)tbResources).BeginGroup = false;
		}
		if (!((BaseItem)tbFullScreen).Visible)
		{
			if (((BaseItem)tbZoomOnePage).Visible)
			{
				((BaseItem)tbZoomOnePage).BeginGroup = true;
			}
			else if (((BaseItem)tbZoomTwoPages).Visible)
			{
				((BaseItem)tbZoomTwoPages).BeginGroup = true;
			}
			else if (((BaseItem)tbZoomMultiplePages).Visible)
			{
				((BaseItem)tbZoomMultiplePages).BeginGroup = true;
			}
			else if (((BaseItem)tbZoomPageWidth).Visible)
			{
				((BaseItem)tbZoomPageWidth).BeginGroup = true;
			}
		}
		else
		{
			if (((BaseItem)tbZoomOnePage).BeginGroup)
			{
				((BaseItem)tbZoomOnePage).BeginGroup = false;
			}
			if (((BaseItem)tbZoomTwoPages).BeginGroup)
			{
				((BaseItem)tbZoomTwoPages).BeginGroup = false;
			}
			if (((BaseItem)tbZoomMultiplePages).BeginGroup)
			{
				((BaseItem)tbZoomMultiplePages).BeginGroup = false;
			}
			if (((BaseItem)tbZoomPageWidth).BeginGroup)
			{
				((BaseItem)tbZoomPageWidth).BeginGroup = false;
			}
		}
		if (!((BaseItem)tbViewModeSinglePage).Visible)
		{
			if (((BaseItem)tbViewModeContinuous).Visible)
			{
				((BaseItem)tbViewModeContinuous).BeginGroup = true;
			}
			else if (((BaseItem)tbViewModeMultiplePages).Visible)
			{
				((BaseItem)tbViewModeMultiplePages).BeginGroup = true;
			}
		}
		else
		{
			if (((BaseItem)tbViewModeContinuous).BeginGroup)
			{
				((BaseItem)tbViewModeContinuous).BeginGroup = false;
			}
			if (((BaseItem)tbViewModeMultiplePages).BeginGroup)
			{
				((BaseItem)tbViewModeMultiplePages).BeginGroup = false;
			}
		}
		if (!((BaseItem)tbFind).Visible)
		{
			((BaseItem)tbEditor).BeginGroup = true;
		}
		else if (((BaseItem)tbEditor).BeginGroup)
		{
			((BaseItem)tbEditor).BeginGroup = false;
		}
		if (IsDesignMode)
		{
			tbTool.Visible = false;
			tbTool.Visible = ShowToolbar;
		}
		((Control)tbTool).Invalidate(true);
		((Control)tbStatusBar).Invalidate(true);
		((Control)tbTool).Refresh();
		((Control)tbStatusBar).Refresh();
		InvokeRefreshControlState();
		RefreshSeparatorIndexes();
	}

	private void RefreshSeparatorIndexes()
	{
		((Control)panelDialogs).MinimumSize = new Size(StiScale.I(280), ((Control)panelDialogs).MinimumSize.Height);
		splParameters.MinSize = StiScale.I(280);
		int num = ((Control)tvBookmarks).Parent.Controls.IndexOf((Control)(object)tvBookmarks);
		if (num > 0 && ((Control)tvBookmarks).Parent == ((Control)splBookmarks).Parent)
		{
			((Control)tvBookmarks).Parent.Controls.SetChildIndex((Control)(object)splBookmarks, num - 1);
		}
		num = ((Control)panelDialogs).Parent.Controls.IndexOf((Control)(object)panelDialogs);
		if (num > 0 && ((Control)panelDialogs).Parent == ((Control)splParameters).Parent)
		{
			((Control)panelDialogs).Parent.Controls.SetChildIndex((Control)(object)splParameters, num - 1);
		}
	}

	public void ActivateBookmarksPanel()
	{
		ShowBookmarksPanel = true;
		SetBookmarksButtonChecked(value: true);
	}

	public void ActivateThumbsPanel()
	{
		ShowThumbsPanel = true;
		SetThumbsButtonChecked(value: true);
	}

	public void HideBookmarksPanel()
	{
		ShowBookmarksPanel = false;
		SetBookmarksButtonChecked(value: false);
	}

	public void HideThumbsPanel()
	{
		ShowThumbsPanel = false;
		SetThumbsButtonChecked(value: false);
	}

	private void Clear()
	{
		((TreeView)tvBookmarks).Nodes.Clear();
		lastComponent = null;
		componentsOnPages?.Clear();
		componentsOnPages = null;
		bookmarksHash?.Clear();
		bookmarksHash = null;
		pageIsProcessed?.Clear();
		pageIsProcessed = null;
		PageParams?.Clear();
		PageParams = null;
		componentsMouseOver.Clear();
		((Control)listResources).Controls.Clear();
		((Control)listResources).Visible = false;
	}

	public override void Refresh()
	{
		if (((Control)view).InvokeRequired)
		{
			((Control)view).Invoke((Delegate)(EventHandler)delegate
			{
				RefreshInternal();
			});
		}
		else
		{
			RefreshInternal();
		}
	}

	private void RefreshInternal()
	{
		bool flag = blockSaveConfig;
		try
		{
			blockSaveConfig = true;
			if (CurrentReport != null)
			{
				foreach (StiPage renderedPage in CurrentReport.RenderedPages)
				{
					renderedPage.Report = CurrentReport;
					StiComponentsCollection stiComponentsCollection = renderedPage.GetComponents();
					foreach (StiComponent item in stiComponentsCollection)
					{
						item.Page = renderedPage;
						item.Report = CurrentReport;
					}
				}
				BuildBookmarks();
				PreparePreview();
				SetZoomByDefault();
				CurrentPrintPage = 0;
				((Control)view).Invalidate(((Control)this).ClientRectangle);
			}
			else
			{
				((Control)view).Invalidate();
			}
			((Control)lbThumbs).Invalidate();
			StiReport currentReport = CurrentReport;
			if (currentReport != null && !currentReport.IsReportRenderingAfterSubmit && !ignoreParametersRefreshingOnce)
			{
				Parameters.CheckRequestFromUser();
			}
			UpdateControlsState();
			RefreshPageNofM();
		}
		finally
		{
			blockSaveConfig = flag;
		}
	}

	public void InvalidatePageRect(Rectangle pageRect)
	{
		((Control)view).Invalidate(new Rectangle(pageRect.X, pageRect.Y, pageRect.Width + 1, pageRect.Height + 1));
	}

	public void InvalidateComponent(StiComponent comp)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		RectangleD componentRect = GetComponentRect(comp);
		((RectangleD)(ref componentRect)).Inflate(1.0, 1.0);
		InvalidatePageRect(((RectangleD)(ref componentRect)).ToRectangle());
	}

	private StiPagesCollection GetVisiblePages()
	{
		StiPagesCollection stiPagesCollection = new StiPagesCollection(CurrentReport);
		if (pageViewMode == StiPageViewMode.SinglePage)
		{
			if (CurrentReport != null && CurrentReport.RenderedPages.Count > CurrentPrintPage)
			{
				stiPagesCollection.Add(CurrentReport.RenderedPages[CurrentPrintPage]);
			}
		}
		else if (PageParams != null)
		{
			foreach (StiPageParamView pageParam in PageParams)
			{
				Rectangle pageRect = GetPageRect(pageParam);
				if (((Control)view).ClientRectangle.IntersectsWith(pageRect))
				{
					stiPagesCollection.Add(pageParam.Page);
				}
			}
		}
		return stiPagesCollection;
	}

	private void UpdateScrollBars(int totalWidth, int totalHeight)
	{
		if (((Control)view).Width > 0 && ((Control)view).Height > 0)
		{
			((ScrollBar)vScrollBar).Maximum = totalHeight;
			((ScrollBar)vScrollBar).LargeChange = ((Control)view).Height;
			((ScrollBar)vScrollBar).SmallChange = ((Control)view).Height / 10;
			if (((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
			{
				((ScrollBar)vScrollBar).LargeChange = ((ScrollBar)vScrollBar).Maximum;
			}
			if (totalHeight < ((Control)view).Height)
			{
				((Control)vScrollBar).Enabled = false;
				((ScrollBar)vScrollBar).Value = 0;
			}
			else
			{
				((Control)vScrollBar).Enabled = true;
			}
			if (((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
			{
				((ScrollBar)vScrollBar).Value = 0;
			}
			else if (((ScrollBar)vScrollBar).Value + ((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
			{
				((ScrollBar)vScrollBar).Value = ((ScrollBar)vScrollBar).Maximum - ((ScrollBar)vScrollBar).LargeChange;
			}
			((ScrollBar)hScrollBar).Maximum = totalWidth;
			((ScrollBar)hScrollBar).LargeChange = ((Control)view).Width;
			((ScrollBar)hScrollBar).SmallChange = ((Control)view).Width / 10;
			if (((ScrollBar)hScrollBar).LargeChange > ((ScrollBar)hScrollBar).Maximum)
			{
				((ScrollBar)hScrollBar).LargeChange = ((ScrollBar)hScrollBar).Maximum;
			}
			if (totalWidth < ((Control)view).Width)
			{
				isHorizontalScrollBarVisible = false;
				((Control)panelHorizontal).Visible = false;
				((ScrollBar)hScrollBar).Value = 0;
			}
			else
			{
				isHorizontalScrollBarVisible = true;
				((Control)panelHorizontal).Visible = GetHorScrollBarVisibility;
			}
			if (((ScrollBar)hScrollBar).LargeChange > ((ScrollBar)hScrollBar).Maximum)
			{
				((ScrollBar)hScrollBar).Value = 0;
			}
			else if (((ScrollBar)hScrollBar).Value + ((ScrollBar)hScrollBar).LargeChange >= ((ScrollBar)hScrollBar).Maximum)
			{
				((ScrollBar)hScrollBar).Value = ((ScrollBar)hScrollBar).Maximum - ((ScrollBar)hScrollBar).LargeChange;
			}
			if (((Control)hScrollBar).Visible)
			{
				((Control)hScrollBar).Invalidate();
			}
			((Control)vScrollBar).Invalidate();
		}
	}

	private void UpdateScrollBarsInSingleMode()
	{
		if (!lockSingleMode && PageParams != null && PageParams.Count != 0)
		{
			lockSingleMode = true;
			if (CurrentPrintPage >= PageParams.Count)
			{
				CurrentPrintPage = PageParams.Count - 1;
			}
			if (CurrentPrintPage < 0)
			{
				CurrentPrintPage = 0;
			}
			StiPageParamView stiPageParamView = PageParams[CurrentPrintPage] as StiPageParamView;
			int num = stiPageParamView.Width + DistancePage * 2;
			int num2 = stiPageParamView.Height + DistancePage * 2;
			if (((Control)view).Width - StiScale.I8 > num)
			{
				stiPageParamView.X = (((Control)view).Width - num) / 2;
			}
			else
			{
				stiPageParamView.X = DistancePage;
			}
			if (((Control)view).Height - StiScale.I8 > num2)
			{
				stiPageParamView.Y = (((Control)view).Height - num2) / 2;
			}
			else
			{
				stiPageParamView.Y = DistancePage;
			}
			UpdateScrollBars(num, num2);
			lockSingleMode = false;
		}
	}

	private void PrepareReport()
	{
		if (((Control)view).InvokeRequired)
		{
			((Control)view).Invoke((Delegate)(EventHandler)delegate
			{
				PrepareReportInternal();
			});
		}
		else
		{
			PrepareReportInternal();
		}
	}

	private void PrepareReportInternal()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		if (CurrentReport == null || ((Control)view).Width <= 0 || ((Control)view).Height <= 0)
		{
			return;
		}
		PageParams = new ArrayList();
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			ArrayList pageParams = PageParams;
			StiPageParamView stiPageParamView = new StiPageParamView();
			StiUnit unit = renderedPage.Unit;
			RectangleD displayRectangle = renderedPage.DisplayRectangle;
			stiPageParamView.Width = (int)unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width * Zoom * StiScale.Factor);
			StiUnit unit2 = renderedPage.Unit;
			displayRectangle = renderedPage.DisplayRectangle;
			stiPageParamView.Height = (int)unit2.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height * Zoom * StiScale.Factor);
			stiPageParamView.Page = renderedPage;
			pageParams.Add(stiPageParamView);
		}
		if (pageViewMode == StiPageViewMode.SinglePage)
		{
			UpdateScrollBarsInSingleMode();
			componentsOnPages = new StiComponentsCollection();
			if (CurrentReport.RenderedPages.CacheMode)
			{
				PrepareBookmarksHash();
				return;
			}
			{
				foreach (StiPage renderedPage2 in CurrentReport.RenderedPages)
				{
					componentsOnPages.AddRange(renderedPage2.GetComponents());
				}
				return;
			}
		}
		int distancePage = DistancePage;
		int num = 0;
		int num2 = 0;
		while (num < CurrentReport.RenderedPages.Count)
		{
			int num3 = distancePage;
			int num4 = 0;
			while (num < CurrentReport.RenderedPages.Count)
			{
				StiPageParamView stiPageParamView2 = (StiPageParamView)PageParams[num];
				num3 += stiPageParamView2.Width + distancePage;
				if (pageViewMode == StiPageViewMode.SinglePage)
				{
					stiPageParamView2.Line = 0;
					num++;
					num4++;
					break;
				}
				if (pageViewMode == StiPageViewMode.Continuous)
				{
					stiPageParamView2.Line = num2;
					num++;
					num4++;
					break;
				}
				if (pageViewMode != StiPageViewMode.MultiplePages)
				{
					continue;
				}
				if (num3 >= ((Control)view).Width)
				{
					if (num4 == 0)
					{
						stiPageParamView2.Line = num2;
						num++;
					}
					break;
				}
				num4++;
				stiPageParamView2.Line = num2;
				num++;
			}
			num2++;
		}
		int num5 = distancePage;
		int num6 = 0;
		int num7 = distancePage;
		num = 0;
		while (num < PageParams.Count)
		{
			StiPageParamView stiPageParamView3 = (StiPageParamView)PageParams[num];
			num2 = stiPageParamView3.Line;
			int num8 = distancePage;
			int num9 = 0;
			int num10 = 0;
			for (int i = num; i < PageParams.Count && num2 == ((StiPageParamView)PageParams[i]).Line; i++)
			{
				StiPageParamView stiPageParamView4 = (StiPageParamView)PageParams[i];
				num8 += stiPageParamView4.Width + distancePage;
				num9 = Math.Max(stiPageParamView4.Height, num9);
				num10++;
			}
			num5 += num9 + distancePage;
			num6 = Math.Max(num8, num6);
			int num11 = (((Control)view).Width - num8) / 2;
			if (num11 < distancePage)
			{
				num11 = distancePage;
			}
			int num12 = num11;
			for (; num < PageParams.Count && num2 == ((StiPageParamView)PageParams[num]).Line; num++)
			{
				StiPageParamView stiPageParamView5 = (StiPageParamView)PageParams[num];
				stiPageParamView5.X = (StiOptions.Viewer.IsRightToLeft ? (((Control)view).Width - num12 - stiPageParamView5.Width - distancePage) : num12);
				stiPageParamView5.Y = num7 + (num9 - stiPageParamView5.Height) / 2;
				num12 += stiPageParamView5.Width + distancePage;
			}
			num7 += num9 + distancePage;
		}
		UpdateScrollBars(num6, num5);
		componentsOnPages = new StiComponentsCollection();
		if (CurrentReport.RenderedPages.CacheMode)
		{
			PrepareBookmarksHash();
			return;
		}
		foreach (StiPage renderedPage3 in CurrentReport.RenderedPages)
		{
			componentsOnPages.AddRange(renderedPage3.GetComponents());
		}
	}

	private void PrepareBookmarksHash()
	{
		if (!CurrentReport.RenderedPages.CacheMode)
		{
			return;
		}
		componentsOnPages?.Clear();
		bookmarksHash?.Clear();
		bookmarksHash = null;
		pageIsProcessed?.Clear();
		pageIsProcessed = null;
		bookmarksHash = new Hashtable();
		MakeBookmarksHash(CurrentReport.Bookmark);
		pageIsProcessed = new List<bool>();
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			pageIsProcessed.Add(renderedPage.Components.Count > 0);
			AddComponentsWithBookmark(renderedPage);
		}
	}

	private bool FindComponentInHash(StiBookmark bookmark)
	{
		if (!CurrentReport.RenderedPages.CacheMode)
		{
			return true;
		}
		if (CurrentReport.Bookmark == bookmark)
		{
			return true;
		}
		bool flag = !string.IsNullOrWhiteSpace(bookmark.ComponentGuid);
		bool flag2 = !string.IsNullOrWhiteSpace(bookmark.Text);
		if (!(flag || flag2))
		{
			return true;
		}
		object obj = null;
		if (flag)
		{
			obj = bookmarksHash[bookmark.ComponentGuid];
		}
		if (obj == null && flag2)
		{
			obj = bookmarksHash[bookmark.Text];
		}
		if (obj == null)
		{
			for (int i = 0; i < CurrentReport.RenderedPages.Count; i++)
			{
				if (!pageIsProcessed[i])
				{
					StiPage stiPage = CurrentReport.RenderedPages[i];
					CurrentReport.RenderedPages.GetPage(stiPage);
					AddComponentsWithBookmark(stiPage);
					pageIsProcessed[i] = true;
					if (flag)
					{
						obj = bookmarksHash[bookmark.ComponentGuid];
					}
					if (obj == null && flag2)
					{
						obj = bookmarksHash[bookmark.Text];
					}
					if (obj != null)
					{
						break;
					}
				}
			}
		}
		obj = null;
		if (flag)
		{
			obj = bookmarksHash[bookmark.ComponentGuid];
		}
		if (obj == null && flag2)
		{
			obj = bookmarksHash[bookmark.Text];
		}
		if (obj != null && obj is BookmarkComponentInfo)
		{
			try
			{
				BookmarkComponentInfo bookmarkComponentInfo = (BookmarkComponentInfo)obj;
				if (bookmarkComponentInfo.IndexList != -1)
				{
					CurrentReport.RenderedPages.GetPage(bookmarkComponentInfo.Page);
					StiComponent value = bookmarkComponentInfo.Page.Components[bookmarkComponentInfo.IndexList];
					componentsOnPages[bookmarkComponentInfo.IndexComp] = value;
				}
			}
			catch
			{
			}
			return true;
		}
		return false;
	}

	private void AddComponentsWithBookmark(StiComponent comp)
	{
		if (!string.IsNullOrWhiteSpace(comp.Guid) && bookmarksHash.ContainsKey(comp.Guid))
		{
			if (!(bookmarksHash[comp.Guid] is BookmarkComponentInfo bookmarkComponentInfo))
			{
				componentsOnPages.Add(comp);
				bookmarksHash[comp.Guid] = new BookmarkComponentInfo
				{
					IndexComp = componentsOnPages.Count - 1,
					IndexList = comp.Page.Components.IndexOf(comp),
					Page = comp.Page
				};
			}
			else
			{
				int num = CurrentReport.RenderedPages.IndexOf(bookmarkComponentInfo.Page);
				int num2 = CurrentReport.RenderedPages.IndexOf(comp.Page);
				int num3 = comp.Page.Components.IndexOf(comp);
				if (num2 < num || (num2 == num && num3 < bookmarkComponentInfo.IndexList))
				{
					bookmarkComponentInfo.Page = comp.Page;
					bookmarkComponentInfo.IndexList = num3;
					componentsOnPages[bookmarkComponentInfo.IndexComp] = comp;
				}
			}
		}
		else if (comp.BookmarkValue != null && bookmarksHash.ContainsKey(comp.BookmarkValue.ToString()))
		{
			if (!(bookmarksHash[comp.BookmarkValue.ToString()] is BookmarkComponentInfo bookmarkComponentInfo2))
			{
				componentsOnPages.Add(comp);
				bookmarksHash[comp.BookmarkValue.ToString()] = new BookmarkComponentInfo
				{
					IndexComp = componentsOnPages.Count - 1,
					IndexList = comp.Page.Components.IndexOf(comp),
					Page = comp.Page
				};
			}
			else
			{
				int num4 = CurrentReport.RenderedPages.IndexOf(bookmarkComponentInfo2.Page);
				int num5 = CurrentReport.RenderedPages.IndexOf(comp.Page);
				int num6 = comp.Page.Components.IndexOf(comp);
				if (num5 < num4 || (num5 == num4 && num6 < bookmarkComponentInfo2.IndexList))
				{
					bookmarkComponentInfo2.Page = comp.Page;
					bookmarkComponentInfo2.IndexList = num6;
					componentsOnPages[bookmarkComponentInfo2.IndexComp] = comp;
				}
			}
		}
		if (!(comp is StiContainer stiContainer))
		{
			return;
		}
		foreach (StiComponent component in stiContainer.Components)
		{
			AddComponentsWithBookmark(component);
		}
	}

	private void MakeBookmarksHash(StiBookmark bookmark)
	{
		if (!string.IsNullOrWhiteSpace(bookmark.ComponentGuid))
		{
			bookmarksHash[bookmark.ComponentGuid] = null;
		}
		if (!string.IsNullOrWhiteSpace(bookmark.Text))
		{
			bookmarksHash[bookmark.Text] = null;
		}
		if (bookmark.Bookmarks == null)
		{
			return;
		}
		foreach (StiBookmark bookmark2 in bookmark.Bookmarks)
		{
			MakeBookmarksHash(bookmark2);
		}
	}

	public RectangleD GetComponentRect(StiComponent component)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		if (component != null)
		{
			RectangleD result = component.ComponentToPage(component.DisplayRectangle);
			StiPageParamView pageParamView = GetPageParamView(component);
			if (pageParamView != null)
			{
				((RectangleD)(ref result)).X = XToScreen(((RectangleD)(ref result)).Left, component.Page) + (double)pageParamView.X - (double)((ScrollBar)hScrollBar).Value;
				((RectangleD)(ref result)).Y = YToScreen(((RectangleD)(ref result)).Top, component.Page) + (double)pageParamView.Y - (double)((ScrollBar)vScrollBar).Value;
				((RectangleD)(ref result)).Width = component.Page.Unit.ConvertToHInches(((RectangleD)(ref result)).Width) * Zoom * StiScale.Factor;
				((RectangleD)(ref result)).Height = component.Page.Unit.ConvertToHInches(((RectangleD)(ref result)).Height) * Zoom * StiScale.Factor;
				return result;
			}
		}
		return RectangleD.Empty;
	}

	private double XToScreen(double x, StiPage page)
	{
		return page.Unit.ConvertToHInches(x + page.Margins.Left) * Zoom * StiScale.Factor;
	}

	private double YToScreen(double y, StiPage page)
	{
		return page.Unit.ConvertToHInches(y + page.Margins.Top) * Zoom * StiScale.Factor;
	}

	private double GetMaxPageWidth()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (PageParams == null)
		{
			return 0.0;
		}
		double num = 0.0;
		foreach (StiPageParamView pageParam in PageParams)
		{
			double val = num;
			StiUnit unit = pageParam.Page.Unit;
			RectangleD displayRectangle = pageParam.Page.DisplayRectangle;
			num = Math.Max(val, unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width) * StiScale.Factor);
		}
		return num;
	}

	private double GetMaxPageHeight()
	{
		if (PageParams == null)
		{
			return 0.0;
		}
		double num = 0.0;
		foreach (StiPageParamView pageParam in PageParams)
		{
			num = Math.Max(num, pageParam.Page.Unit.ConvertToHInches(pageParam.Page.PageHeight) * StiScale.Factor);
		}
		return num;
	}

	public Rectangle GetPageRect(StiPageParamView view)
	{
		return new Rectangle(view.X - ((ScrollBar)hScrollBar).Value, view.Y - ((ScrollBar)vScrollBar).Value, view.Width, view.Height);
	}

	internal StiPageParamView GetPageParamView(StiComponent component)
	{
		if (PageParams == null)
		{
			return null;
		}
		StiPageParamView result = null;
		StiPage page = component.Page;
		foreach (StiPageParamView pageParam in PageParams)
		{
			if (pageParam.Page == page)
			{
				result = pageParam;
				break;
			}
		}
		return result;
	}

	public void PreparePreview()
	{
		if (CurrentReport == null)
		{
			return;
		}
		PrepareReport();
		BuildThumbs();
		isEditableComponentsPresent = false;
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			stiComponentsCollection.AddRange(renderedPage.GetComponents());
		}
		int num = 0;
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item is IStiEditable && ((IStiEditable)item).Editable)
			{
				isEditableComponentsPresent = true;
				num++;
				break;
			}
		}
		if (num == 0)
		{
			if (IsEditorActivated)
			{
				CloseEditor();
			}
		}
		else if (IsEditorActivated)
		{
			IsEditorActivated = false;
			IsEditorActivated = true;
		}
		if (FocusOnLoad)
		{
			((Control)view).Focus();
		}
	}

	internal void RefreshPageNofM()
	{
		((BaseItem)tbPageControl).Text = GetPageNofMContent();
		((BaseItem)tbPageControl).Refresh();
		((Control)tbTool).Invalidate(true);
	}

	private string GetPageNofMContent()
	{
		return string.Format(StiLocalization.Get("FormViewer", "PageNofM"), CurrentPage.ToString(), TotalPageCount.ToString());
	}

	public void SetPageNofM(string text)
	{
		try
		{
			text = text.TrimStart(new char[0]);
			int num = 0;
			for (int i = 0; i < text.Length && char.IsDigit(text[i]); i++)
			{
				num++;
			}
			if (num > 0)
			{
				text = text.Substring(0, num);
				CurrentPage = int.Parse(text);
				ScrollToCurPage(fullSize: true);
				((Control)view).Invalidate();
			}
		}
		catch
		{
		}
	}

	public string GetPageNofM()
	{
		return ((BaseItem)tbPageControl).Text;
	}

	public void SwitchBookmarks()
	{
		tbBookmarks.Checked = !tbBookmarks.Checked;
		InvokeBookmarks();
	}

	public void BuildBookmarks()
	{
		if (((Control)tvBookmarks).InvokeRequired)
		{
			((Control)tvBookmarks).Invoke((Delegate)(EventHandler)delegate
			{
				BuildBookmarksInternal();
			});
		}
		else
		{
			BuildBookmarksInternal();
		}
	}

	private void BuildBookmarksInternal()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		if (CurrentReport?.Bookmark != null)
		{
			((TreeView)tvBookmarks).BeginUpdate();
			nodesMemory.SaveExpNodes((TreeView)(object)tvBookmarks);
			((TreeView)tvBookmarks).Nodes.Clear();
			if ((int)CurrentReport.EngineVersion == 2)
			{
				((TreeView)tvBookmarks).Nodes.Add(StiBookmarksV2Helper.GetTreeNodeFromBookmarks(CurrentReport.Bookmark));
			}
			else
			{
				((TreeView)tvBookmarks).Nodes.Add(StiBookmarksV1Helper.GetTreeNode(CurrentReport.Bookmark));
			}
			((TreeView)tvBookmarks).Nodes[0].Expand();
			nodesMemory.LoadExpNodes((TreeView)(object)tvBookmarks, restoreSelectedNode: true);
			((TreeView)tvBookmarks).EndUpdate();
			if (!blockBookmarkPanelState && CurrentReport != null && !CurrentReport.IsInteractionRendering)
			{
				tbBookmarks.Checked = ((TreeView)tvBookmarks).Nodes.Count > 0 && ((TreeView)tvBookmarks).Nodes[0].Nodes.Count > 0;
			}
		}
	}

	private void ClearBookmarksLast()
	{
		if (lastComponent != null)
		{
			lastComponent.HighlightState = StiHighlightState.Hide;
		}
	}

	private void GotoBookmarkComponent(StiComponent component)
	{
		ClearBookmarksLast();
		lastComponent = component;
		component.HighlightState = StiHighlightState.Active;
		ScrollToComponent(component);
		CurrentPrintPage = CurrentReport.RenderedPages.IndexOf(component.Page);
	}

	public void LoadConfig()
	{
		blockSaveConfig = true;
		StiSettings.Load();
		tbThumbs.Checked = StiSettings.GetBool(CategoryInConfiguration, "ThumbsPanelEnabled", tbThumbs.Checked);
		tbBookmarks.Checked = StiSettings.GetBool(CategoryInConfiguration, "BookmarksPanelEnabled", tbBookmarks.Checked);
		int @int = StiSettings.GetInt(CategoryInConfiguration, "BookmarksPanelWidth", ((Control)tvBookmarks).Width, isZoom: true);
		@int = Math.Max(@int, StiScale.XXI(100.0));
		@int = Math.Min(@int, StiScale.XXI(600.0));
		if (@int > 0)
		{
			((Control)tvBookmarks).Width = @int;
		}
		@int = StiSettings.GetInt(CategoryInConfiguration, "ThumbsPanelWidth", ((Control)lbThumbs).Width, isZoom: true);
		@int = Math.Max(@int, StiScale.XXI(100.0));
		@int = Math.Min(@int, StiScale.XXI(600.0));
		((Control)lbThumbs).Width = @int;
		if (StiSettings.Get(CategoryInConfiguration, "PageViewMode") is StiPageViewMode)
		{
			PageViewMode = (StiPageViewMode)StiSettings.Get(CategoryInConfiguration, "PageViewMode");
		}
		SetZoom(StiSettings.GetDouble(CategoryInConfiguration, "Zoom", Zoom) * 100.0);
		isZoomPageWidth = StiSettings.GetBool(CategoryInConfiguration, "isZoomPageWidth", isZoomPageWidth);
		isZoomOnePage = StiSettings.GetBool(CategoryInConfiguration, "isZoomOnePage", isZoomOnePage);
		isZoomTwoPages = StiSettings.GetBool(CategoryInConfiguration, "isZoomTwoPages", isZoomTwoPages);
		isZoomMultiplePages = StiSettings.GetBool(CategoryInConfiguration, "isZoomMultiplePages", isZoomMultiplePages);
		zoomMultiplePagesByWidth = StiSettings.GetInt(CategoryInConfiguration, "zoomMultiplePagesByWidth", zoomMultiplePagesByWidth);
		zoomMultiplePagesByHeight = StiSettings.GetInt(CategoryInConfiguration, "zoomMultiplePagesByHeight", zoomMultiplePagesByHeight);
		cbFindMatchCase.Checked = StiSettings.GetBool(CategoryInConfiguration, "MatchCase", cbFindMatchCase.Checked);
		cbFindMatchWholeWord.Checked = StiSettings.GetBool(CategoryInConfiguration, "MatchWholeWord", cbFindMatchWholeWord.Checked);
		SetZoomByDefault();
		blockSaveConfig = false;
	}

	public void SaveConfig()
	{
		if (!blockSaveConfig)
		{
			StiSettings.Load();
			StiSettings.Set(CategoryInConfiguration, "BookmarksPanelWidth", ((Control)tvBookmarks).Width, isZoom: true);
			StiSettings.Set(CategoryInConfiguration, "ParametersPanelWidth", ((Control)panelDialogs).Width, isZoom: true);
			StiSettings.Set(CategoryInConfiguration, "ThumbsPanelWidth", ((Control)lbThumbs).Width, isZoom: true);
			StiSettings.Set(CategoryInConfiguration, "ThumbsPanelEnabled", tbThumbs.Checked);
			StiSettings.Set(CategoryInConfiguration, "ParametersPanelEnabled", tbParameters.Checked);
			StiSettings.Set(CategoryInConfiguration, "ResourcesPanelEnabled", tbResources.Checked);
			StiSettings.Set(CategoryInConfiguration, "BookmarksPanelEnabled", tbBookmarks.Checked);
			StiSettings.Set(CategoryInConfiguration, "PageViewMode", pageViewMode);
			StiSettings.Set(CategoryInConfiguration, "Zoom", Zoom);
			StiSettings.Set(CategoryInConfiguration, "isZoomPageWidth", isZoomPageWidth);
			StiSettings.Set(CategoryInConfiguration, "isZoomOnePage", isZoomOnePage);
			StiSettings.Set(CategoryInConfiguration, "isZoomTwoPages", isZoomTwoPages);
			StiSettings.Set(CategoryInConfiguration, "isZoomMultiplePages", isZoomMultiplePages);
			StiSettings.Set(CategoryInConfiguration, "zoomMultiplePagesByWidth", zoomMultiplePagesByWidth);
			StiSettings.Set(CategoryInConfiguration, "zoomMultiplePagesByHeight", zoomMultiplePagesByHeight);
			StiSettings.Set(CategoryInConfiguration, "MatchCase", cbFindMatchCase.Checked);
			StiSettings.Set(CategoryInConfiguration, "MatchWholeWord", cbFindMatchWholeWord.Checked);
			StiSettings.Save();
		}
	}

	public StiComponent FindComponentByGuid(string value)
	{
		if (value != null && value.Contains("#GUID#"))
		{
			value = value.Substring(value.IndexOf("#GUID#") + 6);
		}
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			if (!string.IsNullOrWhiteSpace(renderedPage.Guid) && renderedPage.Guid == value)
			{
				return renderedPage;
			}
			StiComponentsCollection stiComponentsCollection = renderedPage.GetComponents();
			foreach (StiComponent item in stiComponentsCollection)
			{
				if (!string.IsNullOrWhiteSpace(item.Guid) && item.Guid == value)
				{
					return item;
				}
			}
		}
		return null;
	}

	public StiComponent FindComponentByTag(string value)
	{
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			if (renderedPage.TagValue != null && renderedPage.TagValue.ToString() == value)
			{
				return renderedPage;
			}
			StiComponentsCollection stiComponentsCollection = renderedPage.GetComponents();
			foreach (StiComponent item in stiComponentsCollection)
			{
				if (item.TagValue != null && item.TagValue.ToString() == value)
				{
					return item;
				}
			}
		}
		return null;
	}

	public StiComponent FindComponentByBookmark(string value)
	{
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			if (renderedPage.BookmarkValue != null && renderedPage.BookmarkValue.ToString() == value)
			{
				return renderedPage;
			}
			StiComponentsCollection stiComponentsCollection = renderedPage.GetComponents();
			foreach (StiComponent item in stiComponentsCollection)
			{
				if (item.BookmarkValue != null && item.BookmarkValue.ToString() == value)
				{
					return item;
				}
			}
		}
		return null;
	}

	public void OpenFind()
	{
		ButtonToolFind_Click(null, EventArgs.Empty);
	}

	public void CloseFind()
	{
		IsFindActivated = false;
	}

	public void OpenEditor()
	{
		ButtonToolEditor_Click(null, EventArgs.Empty);
	}

	public void SwitchEditor()
	{
		ButtonToolEditor_Click(null, EventArgs.Empty);
	}

	public void CloseEditor()
	{
		bool flag = false;
		if (TextEditor != null)
		{
			TextEditor.CloseEditor();
			flag = true;
		}
		StiComponentsCollection stiComponentsCollection = Report?.GetRenderedComponents();
		if (stiComponentsCollection != null)
		{
			foreach (StiComponent item in stiComponentsCollection)
			{
				if (item is IStiEditable && ((IStiEditable)item).Editable)
				{
					item.HighlightState = StiHighlightState.Hide;
					flag = true;
				}
			}
		}
		if (flag)
		{
			((Control)view).Invalidate();
		}
	}

	public void ScrollToComponent(StiComponent component)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		RectangleD componentRect = GetComponentRect(component);
		ScrollToRect(((RectangleD)(ref componentRect)).ToRectangle());
	}

	public void ScrollToRect(Rectangle rect)
	{
		if ((rect.Left < 0 || rect.Right > ((Control)view).Width) && ((Control)hScrollBar).Enabled)
		{
			int num = ((ScrollBar)hScrollBar).Value;
			if (rect.Left < 0)
			{
				num += rect.Left - (((Control)view).Width >> 2);
			}
			if (rect.Left > ((Control)view).Width)
			{
				num += rect.Left - ((Control)view).Width + (((Control)view).Width >> 2);
			}
			if (num < 0)
			{
				num = 0;
			}
			if (num + ((ScrollBar)hScrollBar).LargeChange > ((ScrollBar)hScrollBar).Maximum)
			{
				num = ((ScrollBar)hScrollBar).Maximum - ((ScrollBar)hScrollBar).LargeChange;
			}
			((ScrollBar)hScrollBar).Value = num;
		}
		bool flag = CurrentReport != null && Zoom < 0.25;
		if ((rect.Top < 0 || (!flag && rect.Bottom > ((Control)this).Bottom - 100) || (flag && rect.Top > (int)((double)((Control)view).Height * 0.9))) && ((Control)vScrollBar).Enabled)
		{
			int value = ((ScrollBar)vScrollBar).Value;
			value = ((!flag) ? (value - (100 - rect.Top)) : (value - ((int)((double)((Control)view).Height * 0.9) - rect.Top)));
			if (value < 0)
			{
				value = 0;
			}
			if (value + ((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
			{
				value = ((ScrollBar)vScrollBar).Maximum - ((ScrollBar)vScrollBar).LargeChange;
			}
			((ScrollBar)vScrollBar).Value = value;
		}
	}

	public void ScrollToCurPage()
	{
		ScrollToCurPage(fullSize: false);
	}

	public void ScrollToCurPage(bool fullSize)
	{
		if (PageParams == null || PageParams.Count <= 0 || PageParams.Count <= CurrentPrintPage || CurrentPrintPage < 0)
		{
			return;
		}
		StiPageParamView stiPageParamView = (StiPageParamView)PageParams[CurrentPrintPage];
		Rectangle pageRect = GetPageRect(stiPageParamView);
		if (!(!((Control)view).ClientRectangle.IntersectsWith(pageRect) || fullSize))
		{
			return;
		}
		if (stiPageParamView.Line == 0)
		{
			((ScrollBar)vScrollBar).Value = 0;
			return;
		}
		StiVScrollBar obj = vScrollBar;
		((ScrollBar)obj).Value = ((ScrollBar)obj).Value + pageRect.Top;
		if (((ScrollBar)vScrollBar).Value < 0)
		{
			((ScrollBar)vScrollBar).Value = 0;
		}
		if (((ScrollBar)vScrollBar).Value + ((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
		{
			if (((ScrollBar)vScrollBar).Maximum >= ((ScrollBar)vScrollBar).LargeChange)
			{
				((ScrollBar)vScrollBar).Value = ((ScrollBar)vScrollBar).Maximum - ((ScrollBar)vScrollBar).LargeChange;
			}
			else
			{
				((ScrollBar)vScrollBar).Value = 0;
			}
		}
	}

	internal void ClearZoomFields()
	{
		isZoomMultiplePages = false;
		isZoomPageWidth = false;
		isZoomOnePage = false;
		isZoomTwoPages = false;
	}

	private bool SetZoomByDefault()
	{
		if (isZoomPageWidth)
		{
			SetZoomPageWidth();
			return true;
		}
		if (isZoomOnePage)
		{
			SetZoomOnePage();
			return true;
		}
		if (isZoomTwoPages)
		{
			SetZoomTwoPages();
			return true;
		}
		if (isZoomMultiplePages)
		{
			SetZoomMultiplePages(zoomMultiplePagesByWidth, zoomMultiplePagesByHeight);
			return true;
		}
		return false;
	}

	public void SetZoom(string textZoom)
	{
		try
		{
			textZoom = textZoom.Replace("%", "").Replace(" ", "");
			double num = double.Parse(textZoom);
			SetZoom(num);
		}
		catch
		{
		}
	}

	public void SetZoom(double zoom)
	{
		if (double.IsInfinity(zoom) || double.IsNaN(zoom) || double.IsNegativeInfinity(zoom) || double.IsPositiveInfinity(zoom))
		{
			zoom = 100.0;
		}
		zoom = Math.Max(zoom, 10.0);
		zoom = Math.Min(zoom, 400.0);
		zoom = (double)Math.Round((decimal)zoom, 2);
		lockSliderItemZoom = true;
		siZoom.Value = (int)zoom;
		lockSliderItemZoom = false;
		Zoom = zoom * 0.01;
		((ScrollBar)vScrollBar).Value = 0;
		((ScrollBar)hScrollBar).Value = 0;
		if (CurrentReport != null)
		{
			int num = CurrentPrintPage;
			PrepareReport();
			CurrentPrintPage = num;
			ScrollToCurPage(fullSize: true);
			((Control)view).Invalidate();
			SaveConfig();
		}
		UpdateControlsState();
	}

	public void SetZoomPageWidth()
	{
		ButtonZoomPageWidth_Click(this, EventArgs.Empty);
	}

	public void SetZoomOnePage()
	{
		ButtonZoomOnePage_Click(this, EventArgs.Empty);
	}

	public void SetZoomTwoPages()
	{
		ButtonZoomTwoPages_Click(this, EventArgs.Empty);
	}

	public void SetZoomMultiplePages(int pagesByWidth, int pagesByHeight)
	{
		if (CurrentReport != null)
		{
			pagesByWidth = Math.Max(pagesByWidth, 1);
			pagesByHeight = Math.Max(pagesByHeight, 1);
			if (pageViewMode != StiPageViewMode.MultiplePages)
			{
				PageViewMode = StiPageViewMode.MultiplePages;
			}
			double num = GetMaxPageWidth() * (double)pagesByWidth;
			double val = (double)(((Control)view).Width - DistancePage * 7 - StiScale.I2) / num * 100.0;
			double num2 = GetMaxPageHeight() * (double)pagesByHeight;
			double val2 = (double)(((Control)view).Height - DistancePage * 7 - StiScale.I2) / num2 * 100.0;
			double value = Math.Min(val, val2);
			value = Math.Round(value, 0);
			SetZoom(value);
			ClearZoomFields();
			isZoomMultiplePages = true;
			zoomMultiplePagesByWidth = pagesByWidth;
			zoomMultiplePagesByHeight = pagesByHeight;
		}
	}

	[Obsolete]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetZoomMultiplePages()
	{
		SetZoomMultiplePages(1, 1);
	}

	public void SwitchThumbnails()
	{
		tbThumbs.Checked = !tbThumbs.Checked;
		InvokeThumbs();
	}

	public void BuildThumbs()
	{
		BuildThumbsScrollBar();
		((Control)lbThumbs).Invalidate();
	}

	private void BuildThumbsScrollBar()
	{
		Size thumbsItemSize = GetThumbsItemSize();
		if (CurrentReport == null)
		{
			((Control)scrollBarThumbs).Enabled = false;
			return;
		}
		int num = thumbsItemSize.Height * CurrentReport.RenderedPages.Count;
		if (num < ((Control)lbThumbs).ClientRectangle.Height)
		{
			((ScrollBar)scrollBarThumbs).Maximum = 100;
			((ScrollBar)scrollBarThumbs).Minimum = 0;
			((ScrollBar)scrollBarThumbs).LargeChange = 100;
			((Control)scrollBarThumbs).Enabled = false;
			((ScrollBar)scrollBarThumbs).Value = 0;
			((Control)scrollBarThumbs).Invalidate();
		}
		else
		{
			((ScrollBar)scrollBarThumbs).Maximum = num;
			((ScrollBar)scrollBarThumbs).Minimum = 0;
			if (((Control)lbThumbs).ClientRectangle.Height > 0)
			{
				((ScrollBar)scrollBarThumbs).LargeChange = ((Control)lbThumbs).ClientRectangle.Height;
			}
			((Control)scrollBarThumbs).Enabled = true;
			((ScrollBar)scrollBarThumbs).Value = 0;
			((Control)scrollBarThumbs).Invalidate();
		}
		lbThumbs.ScrollToSelectedIndex();
	}

	internal Size GetThumbsItemSize()
	{
		int width = ((Control)lbThumbs).ClientRectangle.Width;
		int height = (int)((double)width * 1.3);
		return new Size(width, height);
	}

	public void InvokePrint()
	{
		ButtonPrint_Click(this, EventArgs.Empty);
	}

	public void InvokeOpen()
	{
		ButtonOpen_Click(this, EventArgs.Empty);
	}

	public void InvokeSave()
	{
		ButtonSave_Click(this, EventArgs.Empty);
	}

	public void InvokePageNew()
	{
		ButtonPageNew_Click(this, EventArgs.Empty);
	}

	public void InvokePageDelete()
	{
		ButtonPageDelete_Click(this, EventArgs.Empty);
	}

	public void InvokePageDesign()
	{
		ButtonPageDesign_Click(this, EventArgs.Empty);
	}

	public void InvokePageSize()
	{
		ButtonPageSize_Click(this, EventArgs.Empty);
	}

	public void InvokeBookmarks()
	{
		ButtonBookmarks_CheckedChanged(this, EventArgs.Empty);
	}

	public void InvokeThumbs()
	{
		ButtonThumbs_Click(this, EventArgs.Empty);
	}

	[Obsolete("Report Viewer does not use more Select Tool.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InvokeToolSelect()
	{
		throw new NotImplementedException();
	}

	[Obsolete("Report Viewer does not use more Hand Tool.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InvokeToolHand()
	{
		throw new NotImplementedException();
	}

	public void InvokeToolEditor()
	{
		ButtonToolEditor_Click(this, EventArgs.Empty);
	}

	public void InvokeToolFind()
	{
		ButtonToolFind_Click(this, EventArgs.Empty);
	}

	public void InvokeFullScreen()
	{
		ButtonFullScreen_Click(this, EventArgs.Empty);
	}

	public void InvokeZoomOnePage()
	{
		ButtonZoomOnePage_Click(this, EventArgs.Empty);
	}

	public void InvokeZoomTwoPages()
	{
		ButtonZoomTwoPages_Click(this, EventArgs.Empty);
	}

	public void InvokeZoomMultiplePages(int pagesByWidth, int pagesByHeight)
	{
		SetZoomMultiplePages(pagesByWidth, pagesByHeight);
	}

	public void InvokeZoomPageWidth()
	{
		ButtonZoomPageWidth_Click(this, EventArgs.Empty);
	}

	public void InvokeDotMatrix()
	{
		ButtonDotMatrix_Click(this, EventArgs.Empty);
	}

	public void InvokeFirstPage()
	{
		ButtonFirstPage_Click(this, EventArgs.Empty);
	}

	public void InvokePrevPage()
	{
		ButtonPrevPage_Click(this, EventArgs.Empty);
	}

	public void InvokeNextPage()
	{
		ButtonNextPage_Click(this, EventArgs.Empty);
	}

	public void InvokeLastPage()
	{
		ButtonLastPage_Click(this, EventArgs.Empty);
	}

	public void InvokeJumpToPage()
	{
		ButtonJumpToPage_Click(this, EventArgs.Empty);
	}

	public void InvokeViewModeSinglePage()
	{
		ButtonViewModeSinglePage_Click(this, EventArgs.Empty);
	}

	public void InvokeViewModeContinuous()
	{
		ButtonViewModeContinuous_Click(this, EventArgs.Empty);
	}

	public void InvokeViewModeMultiplePages()
	{
		ButtonViewModeMultiplePages_Click(this, EventArgs.Empty);
	}

	public void InvokeFindPrevious()
	{
		ButtonFindPrevious_Click(this, EventArgs.Empty);
	}

	public void InvokeFindNext()
	{
		ButtonFindNext_Click(this, EventArgs.Empty);
	}

	internal void SetCurrentPage(int newCurrentPage)
	{
		if (CurrentReport == null || newCurrentPage < 0 || newCurrentPage > CurrentReport.RenderedPages.Count - 1)
		{
			return;
		}
		int num = CurrentPrintPage;
		if (num != -1)
		{
			Rectangle pageRect = GetPageRect((StiPageParamView)PageParams[newCurrentPage]);
			if (pageRect.Y > 30 || pageRect.Y < -30)
			{
				StiVScrollBar obj = vScrollBar;
				((ScrollBar)obj).Value = ((ScrollBar)obj).Value + pageRect.Y;
			}
			InvalidatePageRect(pageRect);
		}
		CurrentPrintPage = newCurrentPage;
		((Control)View).Invalidate();
	}

	public void NextPage()
	{
		try
		{
			if (PageViewMode == StiPageViewMode.SinglePage && IsEditorActivated)
			{
				SwitchEditor();
			}
			SetCurrentPage(CurrentPrintPage + 1);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	public void LastPage()
	{
		if (CurrentReport == null)
		{
			return;
		}
		try
		{
			if (PageViewMode == StiPageViewMode.SinglePage && IsEditorActivated)
			{
				SwitchEditor();
			}
			SetCurrentPage(CurrentReport.RenderedPages.Count - 1);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	public void PrevPage()
	{
		try
		{
			if (PageViewMode == StiPageViewMode.SinglePage && IsEditorActivated)
			{
				SwitchEditor();
			}
			SetCurrentPage(CurrentPrintPage - 1);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	public void FirstPage()
	{
		try
		{
			if (PageViewMode == StiPageViewMode.SinglePage && IsEditorActivated)
			{
				SwitchEditor();
			}
			SetCurrentPage(0);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	public void JumpToPage()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		if (CurrentReport == null)
		{
			return;
		}
		try
		{
			StiGoToPageForm stiGoToPageForm = new StiGoToPageForm(TotalPageCount);
			try
			{
				if (Form.ActiveForm != null)
				{
					((Form)stiGoToPageForm).Owner = Form.ActiveForm;
				}
				if ((int)((Form)stiGoToPageForm).ShowDialog() == 1 && stiGoToPageForm.PageNumber != -1)
				{
					if (PageViewMode == StiPageViewMode.SinglePage && IsEditorActivated)
					{
						SwitchEditor();
					}
					SetCurrentPage(stiGoToPageForm.PageNumber - 1);
				}
			}
			finally
			{
				((IDisposable)stiGoToPageForm)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void StartRefreshReportTimer(int timeout)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (refreshReportTimer == null)
		{
			refreshReportTimer = new Timer();
			refreshReportTimer.Tick += RefreshReportTimer_Tick;
		}
		else
		{
			refreshReportTimer.Stop();
		}
		refreshReportTimer.Interval = timeout * 1000;
		refreshReportTimer.Start();
	}

	private void StopRefreshReportTimer()
	{
		if (refreshReportTimer != null)
		{
			refreshReportTimer.Stop();
			refreshReportTimer.Tick -= RefreshReportTimer_Tick;
			((Component)(object)refreshReportTimer).Dispose();
			refreshReportTimer = null;
		}
	}

	private void BuildResourcesList()
	{
		((Control)listResources).Controls.Clear();
		if (Report != null)
		{
			int num = 4;
			int num2 = 0;
			int num3 = num;
			int y = num;
			List<StiResource> list = (from v in Report.Dictionary.Resources.ToList()
				where v.AvailableInTheViewer && v.Content != null
				select v).ToList();
			foreach (StiResource item in list)
			{
				Panel val = CreateResourceButton(item, num3, y);
				num2 = ((Control)val).Width;
				((Control)listResources).Controls.Add((Control)(object)val);
				num3 += num2 + num;
			}
		}
		lockResourcesState = true;
		((BaseItem)tbResources).Enabled = ((ArrangedElementCollection)((Control)listResources).Controls).Count > 0;
		tbResources.Checked = ((ArrangedElementCollection)((Control)listResources).Controls).Count > 0;
		((Control)listResources).Visible = ((ArrangedElementCollection)((Control)listResources).Controls).Count > 0;
		lockResourcesState = false;
		if (((Control)listResources).Visible)
		{
			CorrectListResourcesSize();
		}
	}

	public string GetSizeResource(long value, int decimals = 1)
	{
		long num = value;
		int num2 = 0;
		while (num >= 900)
		{
			num >>= 10;
			num2++;
		}
		string[] array = new string[9] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
		return ((double)value / Math.Pow(1024.0, num2)).ToString($"N{decimals}").TrimEnd(new char[1] { '0' }).TrimEnd(new char[1] { ',' })
			.TrimEnd(new char[1] { '.' }) + " " + array[num2];
	}

	private Panel CreateResourceButton(StiResource resource, int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		return (Panel)new StiDownloadResourceButton
		{
			Left = x,
			Top = y,
			BackgroundColor = StiGuiHelper.GetColorRulerLightBackground(),
			BorderColor = StiGuiHelper.GetColorRulerLightBorder(),
			HighlightColor = StiGuiHelper.GetColorRulerSelectionBackground(),
			Text = resource.Name,
			Description = GetSizeResource(resource.Content.Length),
			Content = resource.Content,
			ContentType = resource.Type.ToString(),
			SaveDialogFilter = StiResourceTypeHelper.GetFilter(resource.Type),
			FileExtension = StiResourceTypeHelper.GetExtension(resource.Type, resource.Content)
		};
	}

	private void CorrectListResourcesSize()
	{
		if (!((ScrollProperties)listResources.HorizontalScroll).Visible && ((Control)listResources).Size.Height == 66)
		{
			((Control)listResources).Size = new Size(((Control)listResources).Size.Width, 49);
		}
		if (((ScrollProperties)listResources.HorizontalScroll).Visible && ((Control)listResources).Size.Height == 49)
		{
			((Control)listResources).Size = new Size(((Control)listResources).Size.Width, 66);
		}
	}

	private void InvokeFindClose()
	{
		this.FindClose?.Invoke(this, EventArgs.Empty);
	}

	private void InvokePageViewModeChanged()
	{
		this.PageViewModeChanged?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnRefreshControlState(object sender)
	{
	}

	public void InvokeRefreshControlState()
	{
		this.RefreshControlState?.Invoke(this, EventArgs.Empty);
		OnRefreshControlState(this);
	}

	protected virtual void OnClose(object sender)
	{
		if (CurrentReport != null && IgnoreEscapeKey)
		{
			SaveConfig();
			View.ViewerControl = null;
			Report = null;
		}
	}

	public void InvokeClose()
	{
		this.Close?.Invoke(this, EventArgs.Empty);
		OnClose(this);
	}

	protected void InvokeCloseDrillDownPage(int tabIndex)
	{
		StiCloseDrillDownPageEventArgs e = new StiCloseDrillDownPageEventArgs(tabIndex);
		this.CloseDrillDownPage?.Invoke(this, e);
	}

	protected virtual void OnClickPrintButton(object sender)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (CurrentReport != null)
		{
			CloseEditor();
			if (!StiOptions.Engine.GlobalEvents.InvokePrintingDocumentInViewer(CurrentReport))
			{
				((BaseItem)tbPrint).Enabled = false;
				timerPrint = new Timer();
				timerPrint.Tick += TimerPrint_Tick;
				timerPrint.Start();
			}
		}
	}

	private void TimerPrint_Tick(object sender, EventArgs e)
	{
		try
		{
			timerPrint.Stop();
			timerPrint.Tick -= TimerPrint_Tick;
			((Component)(object)timerPrint).Dispose();
			timerPrint = null;
			CurrentReport.Print();
		}
		finally
		{
			((BaseItem)tbPrint).Enabled = true;
		}
	}

	public void InvokeClickPrintButton()
	{
		try
		{
			if (this.ClickPrintButton != null)
			{
				this.ClickPrintButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickPrintButton(this);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnClickOpenButton(object sender)
	{
		StiLoadDialog stiLoadDialog = new StiLoadDialog(this);
		stiLoadDialog.Load();
		stiLoadDialog.Clear();
	}

	public void InvokeClickOpenButton()
	{
		try
		{
			if (this.ClickOpenButton != null)
			{
				this.ClickOpenButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickOpenButton(this);
			}
			UpdateControlsState();
			StiOptions.Engine.GlobalEvents.InvokeDocumentLoadedInViewer(this);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnClickSaveButton(object sender)
	{
		OnClickSaveButton(sender, sendEMail: false);
	}

	protected virtual void OnClickSaveButton(object sender, bool sendEMail)
	{
		StiSaveDialog stiSaveDialog = new StiSaveDialog(this, sendEMail);
		stiSaveDialog.Save();
		stiSaveDialog.Clear();
	}

	public void InvokeClickSaveButton()
	{
		try
		{
			if (this.ClickSaveButton != null)
			{
				this.ClickSaveButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickSaveButton(this);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnClickSaveSendEMailDocumentButton(object sender)
	{
		OnClickSaveButton(sender, sendEMail: true);
	}

	public void InvokeClickSaveSendEMailDocumentButton()
	{
		try
		{
			if (this.ClickSaveSendEMailDocumentButton != null)
			{
				this.ClickSaveSendEMailDocumentButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickSaveSendEMailDocumentButton(this);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	public void InvokeClickExportButton()
	{
		try
		{
			if (this.ClickExportButton != null)
			{
				this.ClickExportButton(this, EventArgs.Empty);
			}
			else
			{
				((PopupItem)tbSave).PopupMenu(((BaseItem)tbSave).ScreenRectangle.X, ((BaseItem)tbSave).ScreenRectangle.Bottom);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	public void InvokeClickSendEMailButton()
	{
		try
		{
			if (this.ClickSendEMailButton != null)
			{
				this.ClickSendEMailButton(this, EventArgs.Empty);
			}
			else
			{
				((PopupItem)tbSendEMail).PopupMenu(((BaseItem)tbSendEMail).ScreenRectangle.X, ((BaseItem)tbSendEMail).ScreenRectangle.Bottom);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnClickPageNewButton(object sender)
	{
		if (CurrentReport != null)
		{
			CurrentReport.RenderedPages.Add(new StiPage(CurrentReport));
			PrepareReport();
			CurrentPrintPage = CurrentReport.RenderedPages.Count - 1;
			ScrollToCurPage(fullSize: true);
			BuildThumbs();
			((Control)view).Invalidate();
			CurrentReport.IsEditedInViewer = true;
		}
	}

	public void InvokeClickPageNewButton()
	{
		try
		{
			if (this.ClickPageNewButton != null)
			{
				this.ClickPageNewButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickPageNewButton(this);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnClickPageDeleteButton(object sender)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		if (CurrentReport != null && (int)MessageBox.Show(StiLocalization.Get("FormViewer", "qnPageDelete"), StiLocalization.Get("FormViewer", "title"), (MessageBoxButtons)4) == 6)
		{
			int index = CurrentPrintPage;
			if (CurrentPrintPage == CurrentReport.RenderedPages.Count - 1)
			{
				CurrentPrintPage--;
			}
			CurrentReport.RenderedPages.RemoveAt(index);
			PrepareReport();
			ScrollToCurPage(fullSize: true);
			BuildThumbs();
			((Control)view).Invalidate();
			CurrentReport.IsEditedInViewer = true;
		}
	}

	public void InvokeClickPageDeleteButton()
	{
		try
		{
			if (this.ClickPageDeleteButton != null)
			{
				this.ClickPageDeleteButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickPageDeleteButton(this);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnClickPageDesignButton(object sender)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		if (CurrentReport != null)
		{
			int num = CurrentPrintPage;
			DialogResult val = CurrentReport.RenderedPages[CurrentPrintPage].DesignPage();
			PrepareReport();
			CurrentPrintPage = num;
			ScrollToCurPage(fullSize: true);
			BuildThumbs();
			((Control)view).Invalidate();
			if ((int)val == 6)
			{
				CurrentReport.IsEditedInViewer = true;
			}
		}
	}

	public void InvokeClickPageDesignButton()
	{
		try
		{
			if (this.ClickPageDesignButton != null)
			{
				this.ClickPageDesignButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickPageDesignButton(this);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnClickPageSizeButton(object sender)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		StiPage page = CurrentReport.RenderedPages[CurrentPrintPage];
		StiPageSizeForm stiPageSizeForm = new StiPageSizeForm(page);
		try
		{
			if ((int)((Form)stiPageSizeForm).ShowDialog() == 1)
			{
				StiResizeReportOptions stiResizeReportOptions = (StiResizeReportOptions)0;
				if (stiPageSizeForm.checkBoxRebuildReport.Checked && ((Control)stiPageSizeForm.checkBoxRebuildReport).Enabled)
				{
					stiResizeReportOptions |= StiResizeReportOptions.RebuildReport;
				}
				if (stiPageSizeForm.cbScaleContent.Checked)
				{
					stiResizeReportOptions |= StiResizeReportOptions.RescaleContent;
				}
				if (stiPageSizeForm.rbAll.Checked)
				{
					stiResizeReportOptions |= StiResizeReportOptions.ProcessAllPages;
				}
				if (stiPageSizeForm.IsPageOrientationChanged)
				{
					stiResizeReportOptions |= StiResizeReportOptions.PageOrientationChanged;
				}
				int num = CurrentPrintPage;
				StiResizeReportHelper.ResizeReport(CurrentReport, stiPageSizeForm.PageOrientation, stiPageSizeForm.PaperKind, stiPageSizeForm.Margins, stiPageSizeForm.PageWidth, stiPageSizeForm.PageHeight, stiResizeReportOptions, CurrentPrintPage);
				PrepareReport();
				CurrentPrintPage = num;
				ScrollToCurPage(fullSize: true);
				((Control)view).Invalidate();
				CurrentReport.IsEditedInViewer = true;
			}
		}
		finally
		{
			((IDisposable)stiPageSizeForm)?.Dispose();
		}
	}

	public void InvokeClickPageSizeButton()
	{
		try
		{
			if (this.ClickPageSizeButton != null)
			{
				this.ClickPageSizeButton(this, EventArgs.Empty);
			}
			else
			{
				OnClickPageSizeButton(this);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected virtual void OnCurrentPageChanged(EventArgs e)
	{
		this.CurrentPageChanged?.Invoke(this, e);
	}

	protected virtual void OnTextEditorOpen(StiTextEditorEventArgs e)
	{
		this.TextEditorOpen?.Invoke(this, e);
	}

	internal void InvokeTextEditorOpen(Control editor)
	{
		OnTextEditorOpen(new StiTextEditorEventArgs(editor));
	}

	protected virtual void OnTextEditorClose(StiTextEditorEventArgs e)
	{
		this.TextEditorClose?.Invoke(this, e);
	}

	internal void InvokeTextEditorClose(Control editor)
	{
		OnTextEditorClose(new StiTextEditorEventArgs(editor));
	}

	protected virtual void OnProcessSendEMail(object sender)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		StiExportService stiExportService = sender as StiExportService;
		List<StiPage> list = ((!HideDisabledPagesBeforeExport) ? null : CurrentReport?.Pages?.ToList());
		try
		{
			if (HideDisabledPagesBeforeExport)
			{
				(from p in CurrentReport?.Pages?.ToList()
					where !p.Enabled
					select p).ToList().ForEach(delegate(StiPage p)
				{
					CurrentReport.Pages.Remove(p);
				});
			}
			stiExportService.Export(CurrentReport, null, sendEMail: true);
		}
		catch (Exception ex)
		{
			if (ex is IOException)
			{
				MessageBox.Show(ex.Message, "IOException");
				return;
			}
			throw;
		}
		finally
		{
			if (list != null)
			{
				CurrentReport.Pages.Clear();
				list.ForEach(delegate(StiPage p)
				{
					CurrentReport.Pages.Add(p);
				});
			}
		}
	}

	public void InvokeProcessSendEMail(StiExportService service)
	{
		if (this.ProcessSendEMail != null)
		{
			this.ProcessSendEMail(service, EventArgs.Empty);
		}
		else
		{
			OnProcessSendEMail(service);
		}
	}

	protected virtual void OnProcessExport(object sender)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		StiExportService stiExportService = sender as StiExportService;
		List<StiPage> list = ((!HideDisabledPagesBeforeExport) ? null : CurrentReport?.Pages?.ToList());
		try
		{
			if (HideDisabledPagesBeforeExport)
			{
				(from p in CurrentReport?.Pages?.ToList()
					where !p.Enabled
					select p).ToList().ForEach(delegate(StiPage p)
				{
					CurrentReport.Pages.Remove(p);
				});
			}
			stiExportService.Export(CurrentReport, null, sendEMail: false);
		}
		catch (Exception ex)
		{
			if (ex is IOException)
			{
				MessageBox.Show(ex.Message, "IOException");
				return;
			}
			throw;
		}
		finally
		{
			if (list != null)
			{
				CurrentReport.Pages.Clear();
				list.ForEach(delegate(StiPage p)
				{
					CurrentReport.Pages.Add(p);
				});
			}
		}
	}

	public void InvokeProcessExport(StiExportService service)
	{
		if (this.ProcessExport != null)
		{
			this.ProcessExport(service, EventArgs.Empty);
		}
		else
		{
			OnProcessExport(service);
		}
	}

	public void InvokeClickComponent(object sender, EventArgs e)
	{
		InvokeClickComponentInteraction(sender as StiComponent);
		this.ClickComponent?.Invoke(sender, EventArgs.Empty);
	}

	public void InvokeDoubleClickComponent(object sender, EventArgs e)
	{
		this.DoubleClickComponent?.Invoke(sender, EventArgs.Empty);
	}

	public void InvokeMouseEnterComponent(object sender, EventArgs e)
	{
		InvokeMouseEnterComponentInteraction(sender as StiComponent);
		this.MouseEnterComponent?.Invoke(sender, EventArgs.Empty);
	}

	public void InvokeMouseLeaveComponent(object sender, EventArgs e)
	{
		InvokeMouseLeaveComponentInteraction(sender as StiComponent);
		this.MouseLeaveComponent?.Invoke(sender, EventArgs.Empty);
	}

	protected virtual void OnZoomChanged(EventArgs e)
	{
		this.ZoomChanged?.Invoke(this, e);
	}

	internal void InvokeZoomChanged()
	{
		OnZoomChanged(EventArgs.Empty);
	}

	private void NewItem_ClickClose(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		TabItem val = (TabItem)((sender is TabItem) ? sender : null);
		MouseEventArgs val2 = (MouseEventArgs)(object)((e is MouseEventArgs) ? e : null);
		if (val2 != null && val != null && (int)val2.Button == 4194304)
		{
			int num = tabStripReports.Tabs.IndexOf(val);
			InvokeCloseDrillDownPage(num);
			int index = num;
			if (num == ((CollectionBase)(object)tabStripReports.Tabs).Count - 1)
			{
				num--;
			}
			Reports.RemoveAt(index);
			lockRefreshReports = true;
			Report = Reports[num];
			lockRefreshReports = false;
			tabStripReports.SelectedTabIndex = num;
			((CollectionBase)(object)tabStripReports.Tabs).RemoveAt(index);
		}
	}

	private void ButtonSave_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		((BaseItem)tbSaveDocumentFile).Visible = GetSaveDocumentFileVisibility;
	}

	private void ButtonSendEMail_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		((BaseItem)tbSendEMailDocumentFile).Visible = GetSendEMailDocumentFileVisibility;
	}

	private void ButtonFindBar_VisibleChanged(object sender, EventArgs e)
	{
		PrepareReport();
	}

	internal void ButtonClose_Click(object sender, EventArgs e)
	{
		try
		{
			CloseEditor();
			InvokeClose();
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	protected override void OnCreateControl()
	{
		if (StiOptions.Viewer.IsRightToLeft)
		{
			tvBookmarks.Mirrored = true;
			((Control)this).RightToLeft = (RightToLeft)1;
			((Control)view).RightToLeft = (RightToLeft)1;
			((BaseItem)tbFirstPage).BeginGroup = false;
		}
		else
		{
			((Control)this).RightToLeft = (RightToLeft)0;
		}
		((UserControl)this).OnCreateControl();
		StiOptions.Engine.GlobalEvents.InvokeLoadViewerControl(this);
	}

	internal void ButtonFullScreen_Click(object sender, EventArgs e)
	{
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			SaveConfig();
			StiFullScreenViewerForm stiFullScreenViewerForm = new StiFullScreenViewerForm(CurrentReport);
			try
			{
				((Form)stiFullScreenViewerForm).WindowState = (FormWindowState)2;
				stiFullScreenViewerForm.ViewerControl.PageViewMode = PageViewMode;
				stiFullScreenViewerForm.ViewerControl.CurrentPrintPage = CurrentPrintPage;
				stiFullScreenViewerForm.ViewerControl.ShowBookmarksPanel = false;
				stiFullScreenViewerForm.ViewerControl.ShowCloseButton = false;
				stiFullScreenViewerForm.ViewerControl.ShowEditor = false;
				stiFullScreenViewerForm.ViewerControl.ShowFind = false;
				stiFullScreenViewerForm.ViewerControl.ShowHorScrollBar = false;
				stiFullScreenViewerForm.ViewerControl.ShowOpen = false;
				stiFullScreenViewerForm.ViewerControl.ShowPageControl = false;
				stiFullScreenViewerForm.ViewerControl.ShowPageDelete = false;
				stiFullScreenViewerForm.ViewerControl.ShowPageDesign = false;
				stiFullScreenViewerForm.ViewerControl.ShowPageNew = false;
				stiFullScreenViewerForm.ViewerControl.ShowPageSize = false;
				stiFullScreenViewerForm.ViewerControl.ShowPageViewMode = false;
				stiFullScreenViewerForm.ViewerControl.ShowPrint = false;
				stiFullScreenViewerForm.ViewerControl.ShowSave = false;
				stiFullScreenViewerForm.ViewerControl.ShowSendEMail = false;
				stiFullScreenViewerForm.ViewerControl.ShowThumbsPanel = false;
				stiFullScreenViewerForm.ViewerControl.ShowToolbar = false;
				stiFullScreenViewerForm.ViewerControl.ShowVertScrollBar = false;
				stiFullScreenViewerForm.ViewerControl.ShowZoom = false;
				stiFullScreenViewerForm.ViewerControl.ThumbsPanelEnabled = false;
				stiFullScreenViewerForm.ViewerControl.ShowStatusBar = false;
				stiFullScreenViewerForm.ViewerControl.FocusOnLoad = true;
				((Form)stiFullScreenViewerForm).ShowDialog();
				SaveConfig();
			}
			finally
			{
				((IDisposable)stiFullScreenViewerForm)?.Dispose();
			}
			((Control)view).Invalidate();
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void Report_RefreshViewer(object sender, EventArgs e)
	{
		((Control)this).Refresh();
	}

	private void Report_Paint(object sender, EventArgs e)
	{
		if (sender is StiComponent comp)
		{
			InvalidateComponent(comp);
		}
		else
		{
			((Control)view).Invalidate();
		}
	}

	private void Report_GotoComponent(object sender, StiGotoCompEventArgs e)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			stiComponentsCollection.Add(renderedPage);
			stiComponentsCollection.AddRange(renderedPage.GetComponents());
		}
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item == e.Component)
			{
				ScrollToComponent(item);
				break;
			}
		}
	}

	private void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
	{
		StiControlPaint.ScrollWindow(((Control)view).Handle, 0, ((ScrollBar)vScrollBar).Value - e.NewValue);
	}

	private void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
	{
		StiControlPaint.ScrollWindow(((Control)view).Handle, ((ScrollBar)hScrollBar).Value - e.NewValue, 0);
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		InvokeClickOpenButton();
	}

	private void ButtonSave_Click(object sender, EventArgs e)
	{
		InvokeClickSaveButton();
	}

	private void ButtonSendEMailDocumentFile_Click(object sender, EventArgs e)
	{
		InvokeClickSaveSendEMailDocumentButton();
	}

	private void ButtonSendEMail_Click(object sender, EventArgs e)
	{
		InvokeClickSendEMailButton();
	}

	private void ButtonPrint_Click(object sender, EventArgs e)
	{
		if (StiPrintProvider.UseEXDialog && StiOptions.Viewer.AllowAsyncPrinting)
		{
			ShowPrintDialog showPrintDialog = InvokeClickPrintButton;
			((Control)this).BeginInvoke((Delegate)showPrintDialog);
		}
		else
		{
			InvokeClickPrintButton();
		}
	}

	internal void ButtonBookmarks_CheckedChanged(object sender, EventArgs e)
	{
		StiViewerTreeView stiViewerTreeView = tvBookmarks;
		bool visible = (((Control)splBookmarks).Visible = tbBookmarks.Checked);
		((Control)stiViewerTreeView).Visible = visible;
		RefreshSeparatorIndexes();
		PreparePreview();
		if (tbZoomPageWidth.Checked)
		{
			ButtonZoomPageWidth_Click(null, e);
		}
		((Control)view).Invalidate();
		SaveConfig();
	}

	internal void ButtonThumbs_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateControlsState();
			PreparePreview();
			if (tbZoomPageWidth.Checked)
			{
				ButtonZoomPageWidth_Click(null, e);
			}
			((Control)view).Invalidate();
			SaveConfig();
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	internal void ButtonParameters_CheckedChanged(object sender, EventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		if (((BaseItem)tbParameters).Enabled)
		{
			if (tbParameters.Checked)
			{
				((Control)splParameters).Visible = (int)((Control)panelDialogs).Dock == 3;
				((Control)panelDialogs).Visible = true;
				if (!Parameters.LockRequestFromUser)
				{
					Parameters.ShowDialogRequestFromUserVariables();
				}
			}
			else
			{
				((Control)panelDialogs).Visible = false;
				((Control)splParameters).Visible = false;
			}
		}
		else
		{
			((Control)panelDialogs).Visible = false;
			((Control)splParameters).Visible = false;
		}
	}

	internal void ButtonResources_CheckedChanged(object sender, EventArgs e)
	{
		if (!lockResourcesState)
		{
			((Control)listResources).Visible = ((BaseItem)tbResources).Enabled && tbResources.Checked;
		}
	}

	internal void ButtonToolEditor_Click(object sender, EventArgs e)
	{
		try
		{
			IsEditorActivated = !IsEditorActivated;
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	internal void ButtonToolEditor_CheckedChanged(object sender, EventArgs e)
	{
		if (!tbEditor.Checked)
		{
			CloseEditor();
		}
	}

	internal void ButtonToolFind_Click(object sender, EventArgs e)
	{
		try
		{
			IsEditorActivated = false;
			IsFindActivated = !IsFindActivated;
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ButtonExpandAll_Click(object sender, EventArgs e)
	{
		((TreeView)tvBookmarks).ExpandAll();
	}

	private void ButtonCollapseAll_Click(object sender, EventArgs e)
	{
		((TreeView)tvBookmarks).CollapseAll();
	}

	private void Splitter_Moved(object sender, SplitterEventArgs e)
	{
		PrepareReport();
		((Control)view).Invalidate();
		SaveConfig();
		if (sender == splThumbs)
		{
			((Control)lbThumbs).Refresh();
		}
		if (tbZoomPageWidth.Checked)
		{
			ButtonZoomPageWidth_Click(null, (EventArgs)(object)e);
		}
	}

	private void TreeViewBookmarks_AfterSelect(object sender, TreeViewEventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		try
		{
			if (CurrentReport == null)
			{
				return;
			}
			if (((TreeView)tvBookmarks).SelectedNode != null)
			{
				if ((int)CurrentReport.EngineVersion == 1)
				{
					ArrayList arrayList = new ArrayList();
					TreeNode val = ((TreeView)tvBookmarks).SelectedNode;
					while (val.Parent != null)
					{
						arrayList.Add(val.Tag);
						val = val.Parent;
					}
					int i = 0;
					StiComponent stiComponent = null;
					StiPage stiPage = null;
					for (int num = arrayList.Count - 1; num >= 0; num--)
					{
						string text = arrayList[num] as string;
						for (; i < componentsOnPages.Count; i++)
						{
							StiComponent stiComponent2 = componentsOnPages[i];
							if (stiPage != stiComponent2.Page)
							{
								stiPage = stiComponent2.Page;
								if (stiPage.BookmarkValue != null && stiPage.BookmarkValue.ToString() == text)
								{
									stiComponent = stiPage;
									break;
								}
							}
							if (stiComponent2.BookmarkValue != null && stiComponent2.BookmarkValue.ToString() == text)
							{
								stiComponent = stiComponent2;
								break;
							}
						}
						if (stiComponent != null)
						{
							GotoBookmarkComponent(stiComponent);
						}
						else
						{
							i = 0;
						}
					}
				}
				else
				{
					TreeNode val2 = ((TreeView)tvBookmarks).SelectedNode;
					StiBookmark stiBookmark = val2.Tag as StiBookmark;
					FindComponentInHash(stiBookmark);
					if (stiBookmark.ComponentGuid != null && stiBookmark.ComponentGuid.Length > 0)
					{
						StiComponent stiComponent3 = null;
						foreach (StiPage renderedPage in Report.RenderedPages)
						{
							if (renderedPage.Guid == stiBookmark.ComponentGuid)
							{
								stiComponent3 = renderedPage;
								break;
							}
						}
						if (stiComponent3 == null)
						{
							foreach (StiComponent componentsOnPage in componentsOnPages)
							{
								if (componentsOnPage.Guid == stiBookmark.ComponentGuid)
								{
									stiComponent3 = componentsOnPage;
									break;
								}
							}
						}
						if (stiComponent3 != null)
						{
							GotoBookmarkComponent(stiComponent3);
						}
					}
					else if (stiBookmark.IsManualBookmark)
					{
						StiComponent stiComponent5 = null;
						foreach (StiPage renderedPage2 in Report.RenderedPages)
						{
							if (renderedPage2.BookmarkValue != null && renderedPage2.BookmarkValue.ToString() == stiBookmark.Text)
							{
								stiComponent5 = renderedPage2;
								break;
							}
						}
						if (stiComponent5 == null)
						{
							foreach (StiComponent componentsOnPage2 in componentsOnPages)
							{
								if (componentsOnPage2.BookmarkValue != null && componentsOnPage2.BookmarkValue.ToString() == stiBookmark.Text)
								{
									stiComponent5 = componentsOnPage2;
									break;
								}
							}
						}
						if (stiComponent5 != null)
						{
							GotoBookmarkComponent(stiComponent5);
						}
					}
					else
					{
						ArrayList arrayList2 = new ArrayList();
						while (val2.Parent != null)
						{
							arrayList2.Add(stiBookmark.Text);
							val2 = val2.Parent;
						}
						int j = 0;
						StiComponent stiComponent7 = null;
						StiPage stiPage4 = null;
						for (int num2 = arrayList2.Count - 1; num2 >= 0; num2--)
						{
							string text2 = arrayList2[num2] as string;
							for (; j < componentsOnPages.Count; j++)
							{
								StiComponent stiComponent8 = componentsOnPages[j];
								if (stiPage4 != stiComponent8.Page)
								{
									stiPage4 = stiComponent8.Page;
									if (stiPage4.BookmarkValue != null && stiPage4.BookmarkValue.ToString() == text2)
									{
										stiComponent7 = stiPage4;
										break;
									}
								}
								if (stiComponent8.BookmarkValue != null && stiComponent8.BookmarkValue.ToString() == text2)
								{
									stiComponent7 = stiComponent8;
									break;
								}
							}
							if (stiComponent7 != null)
							{
								GotoBookmarkComponent(stiComponent7);
							}
							else
							{
								j = 0;
							}
						}
					}
				}
			}
			else
			{
				ClearBookmarksLast();
			}
			((Control)view).Invalidate();
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	internal void ButtonDotMatrix_Click(object sender, EventArgs e)
	{
		try
		{
			Report.IsStopped = false;
			Report.ShowDotMatrix(dialogForm: true);
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	internal void ButtonJumpToPage_Click(object sender, EventArgs e)
	{
		JumpToPage();
	}

	private void PanelView_SizeChanged(object sender, EventArgs e)
	{
		CorrectListResourcesSize();
	}

	private void PanelHorizontal_Resize(object sender, EventArgs e)
	{
		((Control)panelHorizontal).Height = SystemInformation.HorizontalScrollBarHeight;
	}

	private void This_Resize(object sender, EventArgs e)
	{
		if (CurrentReport != null && !SetZoomByDefault())
		{
			PrepareReport();
			((Control)view).Invalidate();
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		PreparePreview();
		if (FocusOnLoad)
		{
			((Control)view).Focus();
		}
		((Control)this).Invalidate();
	}

	private void This_HelpClick(object sender, EventArgs e)
	{
		StiHelpProvider.ShowHelpDefaultBrowser(SuperTooltip.CurrentSuperTooltipHyperlink);
	}

	private void This_DragOver(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
	}

	private void This_DragDrop(object sender, DragEventArgs e)
	{
		if (e.Data != null && e.Data.GetData(DataFormats.FileDrop) is string[] array && array.Length != 0)
		{
			new StiLoadDropDialog(this).Load(array[0]);
		}
	}

	internal void ButtonZoomPageWidth_Click(object sender, EventArgs e)
	{
		try
		{
			if (CurrentReport != null)
			{
				double value = (double)(((Control)view).Width - DistancePage * 2 - StiScale.I8) / GetMaxPageWidth() * 100.0;
				value = Math.Round(value, 0);
				SetZoom(value);
				ClearZoomFields();
				isZoomPageWidth = true;
				UpdateControlsState();
				SaveConfig();
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	internal void ButtonZoomOnePage_Click(object sender, EventArgs e)
	{
		try
		{
			if (CurrentReport != null)
			{
				double maxPageWidth = GetMaxPageWidth();
				double maxPageHeight = GetMaxPageHeight();
				double val = (double)(((Control)view).Width - DistancePage * 2 - StiScale.I2) / maxPageWidth * 100.0;
				double val2 = (double)(((Control)view).Height - DistancePage * 2 - StiScale.I2) / maxPageHeight * 100.0;
				double value = Math.Min(val, val2);
				value = Math.Round(value, 0);
				SetZoom(value);
				ClearZoomFields();
				isZoomOnePage = true;
				UpdateControlsState();
				SaveConfig();
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	internal void ButtonZoomTwoPages_Click(object sender, EventArgs e)
	{
		try
		{
			if (CurrentReport != null)
			{
				PageViewMode = StiPageViewMode.MultiplePages;
				double num = GetMaxPageWidth() * 2.0;
				double val = (double)(((Control)view).Width - DistancePage * 7 - StiScale.I2) / num * 100.0;
				double maxPageHeight = GetMaxPageHeight();
				double val2 = (double)(((Control)view).Height - DistancePage * 7 - StiScale.I2) / maxPageHeight * 100.0;
				double value = Math.Min(val, val2);
				value = Math.Round(value, 0);
				SetZoom(value);
				ClearZoomFields();
				isZoomTwoPages = true;
				UpdateControlsState();
				SaveConfig();
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ItemZoom_Click(object sender, EventArgs e)
	{
		try
		{
			ClearZoomFields();
			if (sender == cm200)
			{
				SetZoom(200.0);
			}
			else if (sender == cm150)
			{
				SetZoom(150.0);
			}
			else if (sender == cm100)
			{
				SetZoom(100.0);
			}
			else if (sender == cm75)
			{
				SetZoom(75.0);
			}
			else if (sender == cm50)
			{
				SetZoom(50.0);
			}
			else if (sender == cm25)
			{
				SetZoom(25.0);
			}
			else if (sender == cm10)
			{
				SetZoom(10.0);
			}
			SaveConfig();
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void SliderZoom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			((BaseItem)lbZoom).Text = siZoom.Value + "%";
			if (!lockSliderItemZoom)
			{
				timerZoomSlider.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void TimerZoomSlider_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!lockSliderItemTimerZoom)
			{
				ClearZoomFields();
				if ((decimal)siZoom.Value != (decimal)(Zoom * 100.0))
				{
					lockSliderItemTimerZoom = true;
					SetZoom(siZoom.Value);
					lockSliderItemTimerZoom = false;
					SaveConfig();
				}
				timerZoomSlider.Enabled = false;
				UpdateControlsState();
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ScrollBarThumbs_Scroll(object sender, ScrollEventArgs e)
	{
		((Control)lbThumbs).Invalidate();
	}

	private void LabelThumbs_SizeChanged(object sender, EventArgs e)
	{
		BuildThumbsScrollBar();
	}

	private void View_MouseUp(object sender, MouseEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		if (IsHandModeActivated)
		{
			((Control)this).OnMouseMove(e);
			IsHandModeActivated = false;
		}
		((Control)view).Cursor = Cursors.Arrow;
		isLeftMouseButtonPressed = false;
		if ((e.Button & 0x100000) > 0)
		{
			Point pts = ((Control)view).PointToClient(Cursor.Position);
			StiPagesCollection visiblePages = GetVisiblePages();
			foreach (StiPage item in visiblePages)
			{
				StiComponentsCollection stiComponentsCollection = item.GetComponents();
				stiComponentsCollection.Add(item);
				foreach (StiComponent item2 in stiComponentsCollection)
				{
					RectangleD componentRect = GetComponentRect(item2);
					if (((RectangleD)(ref componentRect)).Contains((double)pts.X, (double)pts.Y))
					{
						ProcessChartInteractionMouseUp(pts, item2, componentRect);
					}
				}
			}
		}
		ProcessChartInteractionDragStop();
	}

	private void View_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		lastPosition = Cursor.Position;
		((Control)view).Focus();
		if ((int)e.Button == 1048576)
		{
			isLeftMouseButtonPressed = true;
			isLatestLeftMouseButtonPressed = true;
			if (PageParams != null)
			{
				if (IsEditorActivated)
				{
					Point point = ((Control)view).PointToClient(Cursor.Position);
					{
						foreach (StiComponent component in TextEditor.Components)
						{
							RectangleD componentRect = GetComponentRect(component);
							if (!((Control)view).ClientRectangle.IntersectsWith(((RectangleD)(ref componentRect)).ToRectangle()) || !((RectangleD)(ref componentRect)).Contains((double)point.X, (double)point.Y))
							{
								continue;
							}
							if (component is StiCheckBox stiCheckBox)
							{
								string[] array = null;
								if (stiCheckBox.Values.IndexOf('/') != -1)
								{
									array = stiCheckBox.Values.Split(new char[1] { '/' });
								}
								else if (stiCheckBox.Values.IndexOf(';') != -1)
								{
									array = stiCheckBox.Values.Split(new char[1] { ';' });
								}
								else if (stiCheckBox.Values.IndexOf(',') != -1)
								{
									array = stiCheckBox.Values.Split(new char[1] { ',' });
								}
								if (stiCheckBox.CheckedValue == null || stiCheckBox.CheckedValue.ToString().ToLower(CultureInfo.InvariantCulture).Trim() == array[1].ToLower(CultureInfo.InvariantCulture).Trim())
								{
									stiCheckBox.CheckedValue = array[0];
								}
								else
								{
									stiCheckBox.CheckedValue = array[1];
								}
								InvalidatePageRect(((RectangleD)(ref componentRect)).ToRectangle());
							}
							else
							{
								TextEditor.CloseEditor();
								TextEditor.Component.HighlightState = StiHighlightState.Show;
								TextEditor.Index = TextEditor.Components.IndexOf(component);
								component.HighlightState = StiHighlightState.Active;
								TextEditor.OpenEditor();
							}
							break;
						}
						return;
					}
				}
				if (pageViewMode != 0)
				{
					Point pt = ((Control)view).PointToClient(Cursor.Position);
					int num = 0;
					foreach (StiPageParamView pageParam in PageParams)
					{
						Rectangle pageRect = GetPageRect(pageParam);
						if (pageRect.Contains(pt) && num != CurrentPrintPage)
						{
							int num2 = CurrentPrintPage;
							CurrentPrintPage = num;
							if (num2 != -1)
							{
								InvalidatePageRect(GetPageRect((StiPageParamView)PageParams[num2]));
							}
							InvalidatePageRect(pageRect);
							RefreshPageNofM();
							break;
						}
						num++;
					}
				}
			}
		}
		else
		{
			isLatestLeftMouseButtonPressed = false;
		}
		if ((e.Button & 0x100000) <= 0)
		{
			return;
		}
		Point pts = ((Control)view).PointToClient(Cursor.Position);
		StiPagesCollection visiblePages = GetVisiblePages();
		foreach (StiPage item in visiblePages)
		{
			StiComponentsCollection stiComponentsCollection = item.GetComponents();
			stiComponentsCollection.Add(item);
			foreach (StiComponent item2 in stiComponentsCollection)
			{
				RectangleD componentRect2 = GetComponentRect(item2);
				if (((RectangleD)(ref componentRect2)).Contains((double)pts.X, (double)pts.Y))
				{
					ProcessChartInteractionMouseDown(pts, item2, componentRect2);
				}
			}
		}
	}

	private void View_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		Point pts = ((Control)view).PointToClient(Cursor.Position);
		ProcessChartInteractionDrag(pts);
		if (isLeftMouseButtonPressed && !IsHandModeActivated)
		{
			int num = Math.Abs(lastPosition.X - Cursor.Position.X);
			int num2 = Math.Abs(lastPosition.Y - Cursor.Position.Y);
			double num3 = Math.Sqrt(num * num + num2 * num2);
			if (num3 > 10.0 && !blockHandMove && !chartInteractionDragEnabled)
			{
				IsHandModeActivated = true;
			}
		}
		if (CurrentReport == null || !(lastPosition != Cursor.Position))
		{
			return;
		}
		Cursor arrow = Cursors.Arrow;
		if (IsEditorActivated)
		{
			StiComponent stiComponent = null;
			Point point = ((Control)view).PointToClient(Cursor.Position);
			foreach (StiComponent component in TextEditor.Components)
			{
				RectangleD componentRect = GetComponentRect(component);
				if (((RectangleD)(ref componentRect)).Contains((double)point.X, (double)point.Y))
				{
					stiComponent = component;
					break;
				}
			}
			arrow = ((stiComponent == null) ? Cursors.Arrow : ((stiComponent is StiCheckBox) ? Cursors.Hand : Cursors.IBeam));
			((Control)view).Cursor = arrow;
			return;
		}
		if (IsHandModeActivated)
		{
			arrow = CursorHandMove;
			int num4 = lastPosition.X - Cursor.Position.X;
			int num5 = lastPosition.Y - Cursor.Position.Y;
			if (((Control)hScrollBar).Enabled)
			{
				int value = ((ScrollBar)hScrollBar).Value;
				if (((ScrollBar)hScrollBar).LargeChange > ((ScrollBar)hScrollBar).Maximum)
				{
					((ScrollBar)hScrollBar).Value = 0;
				}
				else if (num4 + ((ScrollBar)hScrollBar).Value + ((ScrollBar)hScrollBar).LargeChange > ((ScrollBar)hScrollBar).Maximum)
				{
					((ScrollBar)hScrollBar).Value = ((ScrollBar)hScrollBar).Maximum - ((ScrollBar)hScrollBar).LargeChange;
				}
				else if (num4 + ((ScrollBar)hScrollBar).Value < ((ScrollBar)hScrollBar).Minimum)
				{
					((ScrollBar)hScrollBar).Value = ((ScrollBar)hScrollBar).Minimum;
				}
				else
				{
					StiHScrollBar obj = hScrollBar;
					((ScrollBar)obj).Value = ((ScrollBar)obj).Value + num4;
				}
				if (value != ((ScrollBar)vScrollBar).Value)
				{
					StiControlPaint.ScrollWindow(((Control)view).Handle, value - ((ScrollBar)hScrollBar).Value, 0);
				}
			}
			if (((Control)vScrollBar).Enabled)
			{
				int value2 = ((ScrollBar)vScrollBar).Value;
				if (((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
				{
					((ScrollBar)vScrollBar).Value = 0;
				}
				else if (num5 + ((ScrollBar)vScrollBar).Value + ((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
				{
					((ScrollBar)vScrollBar).Value = ((ScrollBar)vScrollBar).Maximum - ((ScrollBar)vScrollBar).LargeChange;
				}
				else if (num5 + ((ScrollBar)vScrollBar).Value < ((ScrollBar)vScrollBar).Minimum)
				{
					((ScrollBar)vScrollBar).Value = ((ScrollBar)vScrollBar).Minimum;
				}
				else
				{
					StiVScrollBar obj2 = vScrollBar;
					((ScrollBar)obj2).Value = ((ScrollBar)obj2).Value + num5;
				}
				if (value2 != ((ScrollBar)vScrollBar).Value)
				{
					StiControlPaint.ScrollWindow(((Control)view).Handle, 0, value2 - ((ScrollBar)vScrollBar).Value);
				}
			}
			lastPosition = Cursor.Position;
		}
		else
		{
			Point point2 = ((Control)view).PointToClient(Cursor.Position);
			StiPagesCollection visiblePages = GetVisiblePages();
			string text = "";
			string text2 = "";
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (StiPage item in visiblePages)
			{
				StiComponentsCollection stiComponentsCollection = item.GetComponents();
				foreach (StiComponent item2 in stiComponentsCollection)
				{
					RectangleD componentRect2 = GetComponentRect(item2);
					if (((RectangleD)(ref componentRect2)).Contains((double)point2.X, (double)point2.Y))
					{
						if (item2.HyperlinkValue != null)
						{
							text = ProcessHyperlink(item2.HyperlinkValue.ToString());
						}
						if (item2 is StiText)
						{
							string text3 = StiTextHelper.CheckHyperlinkFromHtmlText(item2, componentRect2, point2);
							if (text3 != null)
							{
								text = text3;
							}
						}
						if (item2.ToolTipValue != null)
						{
							text2 = item2.ToolTipValue.ToString();
						}
						StiComponent stiComponent4 = item2;
						if (stiComponent4 != null && stiComponent4.Interaction != null)
						{
							StiInteraction interaction = stiComponent4.Interaction;
							if (interaction.SortingEnabled && interaction.SortingColumn != null && interaction.SortingColumn.Length > 0)
							{
								flag = true;
							}
							if (interaction.DrillDownEnabled && (interaction.DrillDownPageGuid != null || interaction.DrillDownReport != null))
							{
								flag3 = true;
							}
							if (interaction is StiBandInteraction { CollapsingEnabled: not false })
							{
								flag2 = true;
							}
							if (interaction is StiCrossHeaderInteraction { CollapsingEnabled: not false })
							{
								flag2 = true;
							}
						}
						if (componentsMouseOver[item2] == null)
						{
							componentsMouseOver.Add(item2, item2);
							item2.InvokeMouseEnter(item2, EventArgs.Empty);
							InvokeMouseEnterComponent(item2, EventArgs.Empty);
							StiChartInteractionResult stiChartInteractionResult = ProcessChartInteractionMouseEnter(point2, item2, componentRect2);
							if (!string.IsNullOrEmpty(stiChartInteractionResult.ToolTipString))
							{
								text2 = stiChartInteractionResult.ToolTipString;
							}
							if (!string.IsNullOrEmpty(stiChartInteractionResult.HyperlinkString))
							{
								text = stiChartInteractionResult.HyperlinkString;
							}
							if (!string.IsNullOrEmpty(text2) || !string.IsNullOrEmpty(text))
							{
								break;
							}
						}
						else
						{
							StiChartInteractionResult stiChartInteractionResult2 = ProcessChartInteractionOldLeaveNewEnter(point2, item2, componentRect2);
							if (!string.IsNullOrEmpty(stiChartInteractionResult2.ToolTipString))
							{
								text2 = stiChartInteractionResult2.ToolTipString;
							}
							if (!string.IsNullOrEmpty(stiChartInteractionResult2.HyperlinkString))
							{
								text = stiChartInteractionResult2.HyperlinkString;
							}
							if (!string.IsNullOrEmpty(text2) || !string.IsNullOrEmpty(text))
							{
								break;
							}
						}
					}
					else if (componentsMouseOver[item2] != null)
					{
						componentsMouseOver.Remove(item2);
						item2.InvokeMouseLeave(item2, EventArgs.Empty);
						InvokeMouseLeaveComponent(item2, EventArgs.Empty);
						ProcessChartInteractionMouseLeave(item2);
					}
				}
				if (text2 == null || text2.Length != 0)
				{
					continue;
				}
				StiPageParamView pageParamView = GetPageParamView(item);
				if (pageParamView != null && GetPageRect(pageParamView).Contains(point2.X, point2.Y))
				{
					if (item.ToolTipValue != null)
					{
						text2 = item.ToolTipValue.ToString();
						return;
					}
					if (item.HyperlinkValue != null)
					{
						text2 = ProcessHyperlink(item.HyperlinkValue.ToString());
						return;
					}
				}
			}
			Color foreColor = SystemColors.ControlText;
			if (text2 != null && text2.Length == 0 && StiOptions.Viewer.ShowHyperlinkToolTip)
			{
				text2 = text;
				foreColor = Color.Blue;
			}
			if (text.Length <= 0)
			{
				arrow = ((!(flag || flag2 || flag3)) ? Cursors.Arrow : Cursors.Hand);
			}
			else
			{
				arrow = Cursors.Hand;
				lastPosition = Cursor.Position;
			}
			toolTip.ForeColor = foreColor;
			if (text2 != null && text2.Contains("#GUID#"))
			{
				text2 = text2.Substring(0, text2.IndexOf("#GUID#"));
			}
			if (toolTip.GetToolTip((Control)(object)view) != text2)
			{
				toolTip.SetToolTip((Control)(object)view, text2);
			}
		}
		((Control)view).Cursor = arrow;
	}

	private void This_FirstLoad(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		((UserControl)this).Load -= This_FirstLoad;
		Timer val = new Timer
		{
			Interval = 3000
		};
		val.Tick += TrialTimer_Tick;
		val.Start();
	}

	private void TrialTimer_Tick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		((Timer)sender).Stop();
		if (StiDesignerAppStatus.IsRunning || StiLicenseKey.Get(StiLicense.Key) != null || ((Component)this).DesignMode)
		{
			return;
		}
		TimeSpan timeSpan = DateTime.Now - StiVersion.Created;
		if (timeSpan.Days <= 60)
		{
			return;
		}
		StiUsingTrialVersionForm stiUsingTrialVersionForm = new StiUsingTrialVersionForm(timeSpan.Days);
		try
		{
			((Form)stiUsingTrialVersionForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiUsingTrialVersionForm)?.Dispose();
		}
	}

	private void View_MouseLeave(object sender, EventArgs e)
	{
		if (componentsMouseOver != null)
		{
			foreach (StiComponent value in componentsMouseOver.Values)
			{
				value.InvokeMouseLeave(value, EventArgs.Empty);
				InvokeMouseLeaveComponent(value, EventArgs.Empty);
				ProcessChartInteractionMouseLeave(value);
			}
		}
		componentsMouseOver.Clear();
	}

	private void View_MouseWheel(object sender, MouseEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((Control.ModifierKeys & 0x20000) > 0)
		{
			if (StiOptions.Viewer.MouseWheelZoomingEnabled)
			{
				double num = Zoom * 1000.0;
				num += (double)e.Delta * Zoom;
				num = Math.Round(num / 1000.0, 2);
				if (num < 0.1)
				{
					num = 0.1;
				}
				if (num > 10.0)
				{
					num = 10.0;
				}
				Zoom = num;
				siZoom.Value = (int)(Zoom * 100.0);
				if (CurrentReport != null)
				{
					PrepareReport();
					((Control)view).Invalidate();
				}
				SaveConfig();
			}
		}
		else if (pageViewMode == StiPageViewMode.SinglePage && CurrentReport != null && CurrentReport.RenderedPages.Count > 1)
		{
			if (e.Delta > 0)
			{
				PrevPage();
			}
			else
			{
				NextPage();
			}
		}
		else if (((Control)vScrollBar).Enabled)
		{
			int value = ((ScrollBar)vScrollBar).Value;
			int num2 = ((ScrollBar)vScrollBar).Value - e.Delta;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 + ((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
			{
				num2 = ((ScrollBar)vScrollBar).Maximum - ((ScrollBar)vScrollBar).LargeChange;
			}
			((ScrollBar)vScrollBar).Value = num2;
			StiControlPaint.ScrollWindow(((Control)view).Handle, 0, value - num2);
		}
	}

	private void View_Click(object sender, EventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		try
		{
			bool flag = false;
			Point point = ((Control)view).PointToClient(Cursor.Position);
			StiPagesCollection visiblePages = GetVisiblePages();
			foreach (StiPage item in visiblePages)
			{
				StiComponentsCollection stiComponentsCollection = item.GetComponents();
				stiComponentsCollection.Add(item);
				foreach (StiComponent item2 in stiComponentsCollection)
				{
					RectangleD componentRect = GetComponentRect(item2);
					if (!((RectangleD)(ref componentRect)).Contains((double)point.X, (double)point.Y))
					{
						continue;
					}
					item2.InvokeClick(item2, EventArgs.Empty);
					CurrentReport.InvokeClick(item2, EventArgs.Empty);
					StiOptions.Engine.GlobalEvents.InvokeClick(item2, EventArgs.Empty);
					InvokeClickComponent(item2, EventArgs.Empty);
					StiChartInteractionResult stiChartInteractionResult = ProcessChartInteractionClick(point, item2, componentRect);
					if (!string.IsNullOrEmpty(stiChartInteractionResult.HyperlinkString))
					{
						ProcessHyperlinkString(stiChartInteractionResult.HyperlinkString);
					}
					if (isLatestLeftMouseButtonPressed && stiChartInteractionResult.InteractionData != null && stiChartInteractionResult.InteractionData.Series != null && stiChartInteractionResult.InteractionData.Series.Interaction != null && stiChartInteractionResult.InteractionData.Series.Interaction.DrillDownEnabled)
					{
						ProcessDrillDown(item2, null, stiChartInteractionResult.InteractionData);
						return;
					}
					if (!isLatestLeftMouseButtonPressed)
					{
						continue;
					}
					StiComponent stiComponent2 = item2;
					if (stiComponent2 != null && stiComponent2.Interaction != null && stiComponent2.Interaction.DrillDownEnabled)
					{
						int num = 0;
						foreach (TabItem item3 in (CollectionBase)(object)tabStripReports.Tabs)
						{
							TabItem val = item3;
							if (val.Tag == stiComponent2)
							{
								tabStripReports.SelectedTabIndex = num;
								stiComponent2 = null;
								break;
							}
							num++;
						}
					}
					if (stiComponent2 != null && stiComponent2.Interaction != null)
					{
						StiReport stiReport = Report.CompiledReport ?? Report;
						flag = ProcessInteractionSorting(stiComponent2, stiReport);
						if (flag)
						{
							break;
						}
						StiInteraction interaction = stiComponent2.Interaction;
						if (interaction != null && interaction.DrillDownEnabled)
						{
							ProcessDrillDown(item2, interaction, null);
							return;
						}
					}
					if (item2.HyperlinkValue != null)
					{
						ProcessHyperlinkString(item2.HyperlinkValue.ToString());
					}
					if (item2 is StiText)
					{
						string text = StiTextHelper.CheckHyperlinkFromHtmlText(item2, componentRect, point);
						if (text != null)
						{
							ProcessHyperlinkString(text);
						}
					}
				}
			}
			if (!flag)
			{
				foreach (StiPage item4 in visiblePages)
				{
					StiComponentsCollection stiComponentsCollection2 = item4.GetComponents();
					stiComponentsCollection2.Add(item4);
					foreach (StiComponent item5 in stiComponentsCollection2)
					{
						RectangleD componentRect2 = GetComponentRect(item5);
						if (!((RectangleD)(ref componentRect2)).Contains((double)point.X, (double)point.Y))
						{
							continue;
						}
						StiComponent stiComponent4 = item5;
						if (stiComponent4 == null || stiComponent4.Interaction == null)
						{
							continue;
						}
						StiReport stiReport2 = Report.CompiledReport ?? Report;
						StiInteraction interaction2 = stiComponent4.Interaction;
						if (stiComponent4.Interaction is StiBandInteraction { CollapsingEnabled: not false })
						{
							int num2 = 0;
							if (item5 is StiContainer)
							{
								num2 = ((StiContainer)item5).CollapsingIndex;
							}
							if (stiReport2.InteractionCollapsingStates == null)
							{
								stiReport2.InteractionCollapsingStates = new Hashtable();
							}
							string key = item5.Name;
							if (item5 is StiContainer { CollapsingTreePath: not null } stiContainer)
							{
								key = stiContainer.CollapsingTreePath + item5.Name;
							}
							Hashtable hashtable = stiReport2.InteractionCollapsingStates[key] as Hashtable;
							if (hashtable == null)
							{
								hashtable = new Hashtable();
								stiReport2.InteractionCollapsingStates[key] = hashtable;
							}
							hashtable[num2] = !StiDataBandV2Builder.IsCollapsed(item5 as StiContainer, isRendering: false);
							flag = true;
						}
						if (stiComponent4.Interaction is StiCrossHeaderInteraction { CollapsingEnabled: not false })
						{
							StiCrossHeader masterHeader = item5 as StiCrossHeader;
							StiCrossTabV2Builder.SetCollapsed(masterHeader, !StiCrossTabV2Builder.IsCollapsed(masterHeader));
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				int num3 = CurrentPrintPage;
				report.Rendering += View_Rendering;
				try
				{
					blockHandMove = true;
					CurrentReport.IsInteractionRendering = true;
					report.Render(showProgress: false);
					RefreshInteractions();
				}
				finally
				{
					CurrentReport.IsInteractionRendering = false;
					report.Rendering -= View_Rendering;
					RefreshPageNofM();
					((Control)this).Invalidate();
					blockHandMove = false;
				}
				CurrentPrintPage = num3;
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void View_Rendering(object sender, EventArgs e)
	{
		StiReport stiReport = sender as StiReport;
		((BaseItem)tbPageControl).Text = stiReport.StatusString + new string(' ', 5);
		((BaseItem)tbPageControl).Refresh();
	}

	private void View_DoubleClick(object sender, EventArgs e)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Point point = ((Control)view).PointToClient(Cursor.Position);
			StiPagesCollection visiblePages = GetVisiblePages();
			foreach (StiPage item in visiblePages)
			{
				StiComponentsCollection stiComponentsCollection = item.GetComponents();
				stiComponentsCollection.Add(item);
				foreach (StiComponent item2 in stiComponentsCollection)
				{
					RectangleD componentRect = GetComponentRect(item2);
					if (((RectangleD)(ref componentRect)).Contains((double)point.X, (double)point.Y))
					{
						item2.InvokeDoubleClick(item2, EventArgs.Empty);
						CurrentReport.InvokeDoubleClick(item2, EventArgs.Empty);
						StiOptions.Engine.GlobalEvents.InvokeDoubleClick(item2, EventArgs.Empty);
						InvokeDoubleClickComponent(item2, EventArgs.Empty);
					}
				}
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ButtonPageDesign_Click(object sender, EventArgs e)
	{
		InvokeClickPageDesignButton();
	}

	private void ButtonPageNew_Click(object sender, EventArgs e)
	{
		InvokeClickPageNewButton();
	}

	private void ButtonPageSize_Click(object sender, EventArgs e)
	{
		InvokeClickPageSizeButton();
	}

	private void ButtonPageDelete_Click(object sender, EventArgs e)
	{
		InvokeClickPageDeleteButton();
	}

	private void ButtonNextPage_Click(object sender, EventArgs e)
	{
		NextPage();
	}

	private void ButtonLastPage_Click(object sender, EventArgs e)
	{
		LastPage();
	}

	private void ButtonPrevPage_Click(object sender, EventArgs e)
	{
		PrevPage();
	}

	private void ButtonFirstPage_Click(object sender, EventArgs e)
	{
		FirstPage();
	}

	internal void ItemExport_Click(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			CloseEditor();
			Windows7Taskbar.ShowTaskbarProgress(IntPtr.Zero, (ThumbnailProgressState)1);
			StiExportService service = ((BaseItem)(ButtonItem)sender).Tag as StiExportService;
			InvokeProcessExport(service);
		}
		catch (IOException ex)
		{
			StiExceptionProvider.Show((Exception)ex, false, (Form)null);
		}
		catch (Exception ex2)
		{
			StiLogService.Write(((object)this).GetType(), "Export document...ERROR");
			StiLogService.Write(((object)this).GetType(), ex2);
			StiExceptionProvider.Show(ex2, false, (Form)null);
		}
		Windows7Taskbar.HideTaskbarProgress();
	}

	internal void ItemSendEMail_Click(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			CloseEditor();
			Windows7Taskbar.ShowTaskbarProgress(IntPtr.Zero, (ThumbnailProgressState)1);
			StiExportService service = ((BaseItem)(ButtonItem)sender).Tag as StiExportService;
			InvokeProcessSendEMail(service);
		}
		catch (IOException ex)
		{
			StiExceptionProvider.Show((Exception)ex, false, (Form)null);
		}
		catch (Exception ex2)
		{
			StiLogService.Write(((object)this).GetType(), "Export document...ERROR");
			StiLogService.Write(((object)this).GetType(), ex2);
			StiExceptionProvider.Show(ex2, false, (Form)null);
		}
		Windows7Taskbar.HideTaskbarProgress();
	}

	private void ButtonViewModeSinglePage_Click(object sender, EventArgs e)
	{
		try
		{
			int currentPage = CurrentPage;
			ClearZoomFields();
			PageViewMode = StiPageViewMode.SinglePage;
			SetZoomOnePage();
			CloseEditor();
			blockBookmarkPanelState = true;
			((Control)this).Refresh();
			blockBookmarkPanelState = false;
			SaveConfig();
			CurrentPage = currentPage;
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ButtonViewModeContinuous_Click(object sender, EventArgs e)
	{
		try
		{
			int currentPage = CurrentPage;
			ClearZoomFields();
			PageViewMode = StiPageViewMode.Continuous;
			SetZoomOnePage();
			CloseEditor();
			blockBookmarkPanelState = true;
			((Control)this).Refresh();
			blockBookmarkPanelState = false;
			SaveConfig();
			CurrentPage = currentPage;
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ButtonViewModeMultiplePages_Click(object sender, EventArgs e)
	{
		try
		{
			int currentPage = CurrentPage;
			ClearZoomFields();
			PageViewMode = StiPageViewMode.MultiplePages;
			CloseEditor();
			blockBookmarkPanelState = true;
			((Control)this).Refresh();
			blockBookmarkPanelState = false;
			if (zoomMultiplePagesByWidth > 0 && zoomMultiplePagesByHeight > 0)
			{
				SetZoomMultiplePages(zoomMultiplePagesByWidth, zoomMultiplePagesByHeight);
			}
			else
			{
				SetZoomMultiplePages(2, 2);
			}
			SaveConfig();
			CurrentPage = currentPage;
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ButtonFindNext_Click(object sender, EventArgs e)
	{
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((BaseItem)pbFindProgress).Visible = true;
			using BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += FindWorker_DoWork;
			backgroundWorker.RunWorkerAsync();
			while (backgroundWorker.IsBusy)
			{
				Application.DoEvents();
			}
			((BaseItem)pbFindProgress).Visible = false;
			if (findHelper.FindedComponents != null && findHelper.CurrentComponent < findHelper.FindedComponents.Count)
			{
				if (pageViewMode == StiPageViewMode.SinglePage)
				{
					StiComponent stiComponent = findHelper.FindedComponents[findHelper.CurrentComponent];
					int num = 0;
					foreach (StiPageParamView pageParam in PageParams)
					{
						if (pageParam.Page == stiComponent.Page)
						{
							if (CurrentPrintPage != num)
							{
								CurrentPrintPage = num;
							}
							break;
						}
						num++;
					}
				}
				ScrollToComponent(findHelper.FindedComponents[findHelper.CurrentComponent]);
			}
			((Control)View).Invalidate();
			if (findHelper.FindedComponents == null || findHelper.FindedComponents.Count == 0)
			{
				MessageBox.Show(Loc.Get("Editor", "CantFind"), Loc.Get("FormViewer", "title"));
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void ButtonFindPrevious_Click(object sender, EventArgs e)
	{
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((BaseItem)pbFindProgress).Visible = true;
			using BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += FindWorkerPrevious_DoWork;
			backgroundWorker.RunWorkerAsync();
			while (backgroundWorker.IsBusy)
			{
				Application.DoEvents();
			}
			((BaseItem)pbFindProgress).Visible = false;
			if (findHelper.FindedComponents != null && findHelper.CurrentComponent < findHelper.FindedComponents.Count)
			{
				if (pageViewMode == StiPageViewMode.SinglePage)
				{
					StiComponent stiComponent = findHelper.FindedComponents[findHelper.CurrentComponent];
					int num = 0;
					foreach (StiPageParamView pageParam in PageParams)
					{
						if (pageParam.Page == stiComponent.Page)
						{
							if (CurrentPrintPage != num)
							{
								CurrentPrintPage = num;
							}
							break;
						}
						num++;
					}
				}
				ScrollToComponent(findHelper.FindedComponents[findHelper.CurrentComponent]);
			}
			((Control)View).Invalidate();
			if (findHelper.FindedComponents == null || findHelper.FindedComponents.Count == 0)
			{
				MessageBox.Show(Loc.Get("Editor", "CantFind"), Loc.Get("FormViewer", "title"));
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void FindWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		findHelper.StartFind(((Control)tbFindValue).Text, Report, cbFindMatchCase.Checked, cbFindMatchWholeWord.Checked);
	}

	private void FindWorkerPrevious_DoWork(object sender, DoWorkEventArgs e)
	{
		findHelper.StartFindPrevious(((Control)tbFindValue).Text, Report, cbFindMatchCase.Checked, cbFindMatchWholeWord.Checked);
	}

	private void Find_ProgressChanged(object sender, StiProgressChangedEventArgs e)
	{
		((Control)tbFindBar).Invoke((Delegate)(EventHandler)delegate
		{
			pbFindProgress.Maximum = e.Maximum;
			pbFindProgress.Value = e.Value;
			((Control)tbFindBar).Invalidate();
		});
	}

	private void TextBoxFindValue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			ButtonFindNext_Click(null, null);
		}
		if ((int)e.KeyCode == 70 && (Control.ModifierKeys & 0x20000) > 0)
		{
			e.Handled = true;
			IsFindActivated = !IsFindActivated;
		}
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			CloseFind();
		}
	}

	private void ButtonFindClose_Click(object sender, EventArgs e)
	{
		CloseFind();
	}

	private void TabStripReports_TabIndexChanged(object sender, EventArgs e)
	{
		StiReport stiReport = Reports[((Control)tabStripReports).TabIndex];
		lockRefreshReports = true;
		Report = stiReport;
		lockRefreshReports = false;
	}

	private void TabStripReports_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
	{
		if (((CollectionBase)(object)tabStripReports.Tabs).Count == 0 || Reports.Count == 0)
		{
			return;
		}
		int num = tabStripReports.SelectedTabIndex;
		if (num < 0)
		{
			num = 0;
		}
		if (num >= Reports.Count)
		{
			num = Reports.Count - 1;
		}
		StiReport key = Reports[num];
		try
		{
			if (reportsZooms[key] is double)
			{
				Zoom = (double)reportsZooms[key];
			}
		}
		catch
		{
		}
		lockRefreshReports = true;
		Report = key;
		lockRefreshReports = false;
		try
		{
			if (reportsHScrollBars[key] is int)
			{
				((ScrollBar)HScrollBar).Value = (int)reportsHScrollBars[key];
			}
			if (reportsVScrollBars[key] is int)
			{
				((ScrollBar)VScrollBar).Value = (int)reportsVScrollBars[key];
			}
		}
		catch
		{
		}
	}

	private void TabStripReports_TabRemoved(object sender, EventArgs e)
	{
		if (((CollectionBase)(object)tabStripReports.Tabs).Count == 1)
		{
			((Control)tabStripReports).Visible = false;
			((CollectionBase)(object)tabStripReports.Tabs).Clear();
			Reports.Clear();
		}
	}

	private void TabStripReports_TabItemClose(object sender, TabStripActionEventArgs e)
	{
		int num = tabStripReports.SelectedTabIndex;
		InvokeCloseDrillDownPage(num);
		int index = num;
		if (num == ((CollectionBase)(object)tabStripReports.Tabs).Count - 1)
		{
			num--;
		}
		Reports.RemoveAt(index);
		lockRefreshReports = true;
		Report = Reports[num];
		lockRefreshReports = false;
		tabStripReports.SelectedTabIndex = num;
		((CollectionBase)(object)tabStripReports.Tabs).RemoveAt(index);
	}

	private void TabStripReports_SelectedTabChanging(object sender, TabStripTabChangingEventArgs e)
	{
		reportsZooms[Report] = Zoom;
		reportsHScrollBars[Report] = ((ScrollBar)HScrollBar).Value;
		reportsVScrollBars[Report] = ((ScrollBar)VScrollBar).Value;
	}

	private void TabStripReports_NavigateBack(object sender, TabStripActionEventArgs e)
	{
		if (tabStripReports.SelectedTabIndex > 0)
		{
			TabStrip obj = tabStripReports;
			int selectedTabIndex = obj.SelectedTabIndex;
			obj.SelectedTabIndex = selectedTabIndex - 1;
		}
	}

	private void TabStripReports_NavigateForward(object sender, TabStripActionEventArgs e)
	{
		if (tabStripReports.SelectedTabIndex < ((CollectionBase)(object)tabStripReports.Tabs).Count - 2)
		{
			TabStrip obj = tabStripReports;
			int selectedTabIndex = obj.SelectedTabIndex;
			obj.SelectedTabIndex = selectedTabIndex + 1;
		}
	}

	private void RefreshReportTimer_Tick(object sender, EventArgs e)
	{
		if (!Report.IsRendering)
		{
			Report.Render(showProgress: false);
		}
	}

	private void ProcessHyperlinkString(string hyperlink)
	{
		if (hyperlink.Length > 3 && hyperlink.Substring(0, 3) == "###")
		{
			StiComponent stiComponent = FindComponentByGuid(hyperlink.Substring(3, hyperlink.Length - 3));
			if (stiComponent != null)
			{
				SelectComponent(stiComponent);
			}
		}
		else if (hyperlink.Length > 2 && hyperlink.Substring(0, 2) == "##")
		{
			StiComponent stiComponent2 = FindComponentByTag(hyperlink.Substring(2, hyperlink.Length - 2));
			if (stiComponent2 != null)
			{
				SelectComponent(stiComponent2);
			}
		}
		else if (hyperlink.Length > 1 && hyperlink.Substring(0, 1) == "#")
		{
			StiComponent stiComponent3 = FindComponentByBookmark(hyperlink.Substring(1, hyperlink.Length - 1));
			if (stiComponent3 != null)
			{
				SelectComponent(stiComponent3);
			}
		}
		else if (hyperlink.Length > 0 && !StiOptions.Engine.GlobalEvents.InvokeHyperlinkClick(this, hyperlink))
		{
			try
			{
				StiProcess.Start(hyperlink);
			}
			catch (Exception ex)
			{
				StiLogService.Write(((object)this).GetType(), "Hyperlink start (" + hyperlink + ")...ERROR");
				StiLogService.Write(((object)this).GetType(), ex.Message);
			}
		}
	}

	private void SelectComponent(StiComponent findedComp)
	{
		((TreeView)tvBookmarks).SelectedNode = null;
		ClearBookmarksLast();
		lastComponent = findedComp;
		findedComp.HighlightState = StiHighlightState.Active;
		ScrollToComponent(findedComp);
		CurrentPrintPage = CurrentReport.RenderedPages.IndexOf(findedComp.Page);
		((Control)view).Invalidate();
	}

	private bool ProcessInteractionSorting(IStiInteraction interactionComp, StiReport report)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Invalid comparison between Unknown and I4
		StiInteraction interaction = interactionComp.Interaction;
		if (!interaction.SortingEnabled || string.IsNullOrEmpty(interaction.SortingColumn))
		{
			return false;
		}
		string sortDataBandName = interaction.GetSortDataBandName();
		string text = interaction.GetSortColumnsString();
		StiDataBand stiDataBand = ((interactionComp is IStiTableComponent) ? (((IStiTableCell)interactionComp).TableTag as StiDataBand) : (report.GetComponentByName(sortDataBandName) as StiDataBand));
		if (stiDataBand != null)
		{
			if (StiOptions.Engine.Interaction.ForceSortingWithFullTypeConversion)
			{
				if (report.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode)
				{
					if (stiDataBand.DataSource != null)
					{
						text = "{" + StiNameValidator.CorrectName(stiDataBand.DataSource.Name, report) + "." + text + "}";
					}
				}
				else
				{
					text = "{Get" + StiNameValidator.CorrectName(stiDataBand.Name + "." + text, report) + "_Sort}";
				}
			}
			if (stiDataBand.Sort == null || stiDataBand.Sort.Length == 0)
			{
				stiDataBand.Sort = StiSortHelper.AddColumnToSorting(stiDataBand.Sort, text, isAscending: true);
			}
			else
			{
				int columnIndexInSorting = StiSortHelper.GetColumnIndexInSorting(stiDataBand.Sort, text);
				if ((Control.ModifierKeys & 0x20000) > 0)
				{
					stiDataBand.Sort = ((columnIndexInSorting == -1) ? StiSortHelper.AddColumnToSorting(stiDataBand.Sort, text, isAscending: true) : StiSortHelper.ChangeColumnSortDirection(stiDataBand.Sort, text));
				}
				else if (columnIndexInSorting != -1)
				{
					StiInteractionSortDirection stiInteractionSortDirection = ((StiSortHelper.GetColumnSortDirection(stiDataBand.Sort, text) == StiInteractionSortDirection.Ascending) ? StiInteractionSortDirection.Descending : StiInteractionSortDirection.Ascending);
					stiDataBand.Sort = StiSortHelper.AddColumnToSorting(new string[0], text, stiInteractionSortDirection == StiInteractionSortDirection.Ascending);
				}
				else
				{
					stiDataBand.Sort = StiSortHelper.AddColumnToSorting(new string[0], text, isAscending: true);
				}
			}
			return true;
		}
		return false;
	}

	private bool ProcessDrillDownReport(StiComponent comp, StiInteraction interaction, StiSeriesInteractionData seriesInteraction, ref StiReport newReport)
	{
		try
		{
			string drillDownReport = null;
			if (interaction != null)
			{
				drillDownReport = interaction.DrillDownReport;
			}
			if (seriesInteraction != null && seriesInteraction.Series != null && seriesInteraction.Series.Interaction != null)
			{
				drillDownReport = ((StiSeriesInteraction)seriesInteraction.Series.Interaction).DrillDownReport;
			}
			StiCheckDrillDownReportEventArgs stiCheckDrillDownReportEventArgs = new StiCheckDrillDownReportEventArgs(drillDownReport);
			StiOptions.Engine.GlobalEvents.InvokeCheckDrillDownReport(CurrentReport, stiCheckDrillDownReportEventArgs);
			drillDownReport = stiCheckDrillDownReportEventArgs.DrillDownReport;
			if (!string.IsNullOrEmpty(drillDownReport))
			{
				if (StiHyperlinkProcessor.IsServerHyperlink(drillDownReport))
				{
					newReport = StiStimulsoftServerResource.GetDrillDownReport(comp, StiHyperlinkProcessor.GetServerNameFromHyperlink(drillDownReport));
				}
				else if (drillDownReport.ToLowerInvariant().EndsWith(".dll", StringComparison.InvariantCulture))
				{
					newReport = StiReport.GetReportFromAssembly(drillDownReport);
				}
				else
				{
					newReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
					if (comp.Report != null && comp.Report.GlobalizationManager != null && !(comp.Report.GlobalizationManager is StiNullGlobalizationManager))
					{
						newReport.GlobalizationManager = comp.Report.GlobalizationManager;
					}
					string resourceName = StiHyperlinkProcessor.GetResourceNameFromHyperlink(drillDownReport);
					if (resourceName != null)
					{
						resourceName = resourceName.ToLowerInvariant().Trim();
						StiResource stiResource = Report.Dictionary.Resources.ToList().FirstOrDefault((StiResource r) => r.Name != null && r.Name.ToLowerInvariant().Trim() == resourceName);
						if (stiResource != null)
						{
							switch (stiResource.Type)
							{
							case StiResourceType.Report:
								newReport.Load(stiResource.Content);
								break;
							case StiResourceType.ReportSnapshot:
								newReport.LoadDocument(stiResource.Content);
								break;
							}
						}
					}
					else
					{
						newReport.Load(drillDownReport);
					}
					newReport.Compile();
				}
				StiGetDrillDownReportEventArgs stiGetDrillDownReportEventArgs = new StiGetDrillDownReportEventArgs();
				stiGetDrillDownReportEventArgs.Report = newReport;
				comp.InvokeGetDrillDownReport(comp, stiGetDrillDownReportEventArgs);
				StiOptions.Engine.GlobalEvents.InvokeGetDrillDownReport(comp, stiGetDrillDownReportEventArgs);
				if (stiGetDrillDownReportEventArgs.Cancel)
				{
					return true;
				}
				FillDrillDownParameters(comp, newReport, seriesInteraction);
				newReport.Rendering += View_Rendering;
				newReport.Render(showProgress: false);
				newReport.Rendering -= View_Rendering;
			}
		}
		catch
		{
		}
		return false;
	}

	private void ProcessDrillDownPage(StiComponent comp, StiInteraction interaction, StiSeriesInteractionData seriesInteraction, ref StiReport newReport)
	{
		StiReport stiReport = ((mainReport.CompiledReport == null) ? mainReport : mainReport.CompiledReport);
		StiPage stiPage = null;
		if (interaction != null)
		{
			stiPage = interaction.DrillDownPage;
		}
		if (seriesInteraction != null && seriesInteraction.Series != null && seriesInteraction.Series.Interaction != null)
		{
			stiPage = ((StiSeriesInteraction)seriesInteraction.Series.Interaction).DrillDownPage;
		}
		if (stiPage == null)
		{
			foreach (StiPage page in stiReport.Pages)
			{
				if ((interaction != null && interaction.DrillDownPageGuid == page.Guid) || (seriesInteraction != null && ((StiSeries)seriesInteraction.Series).DrillDownPageGuid == page.Guid))
				{
					stiPage = page;
					break;
				}
			}
			if (stiPage == null && mainReport != null)
			{
				foreach (StiPage page2 in mainReport.Pages)
				{
					if ((interaction != null && interaction.DrillDownPageGuid == page2.Guid) || (seriesInteraction != null && ((StiSeries)seriesInteraction.Series).DrillDownPageGuid == page2.Guid))
					{
						stiPage = page2;
						stiReport = mainReport;
						break;
					}
				}
			}
		}
		if (stiPage == null)
		{
			return;
		}
		if (Report.IsCompiled || Report.CheckNeedsCompiling())
		{
			if (Report.CheckNeedsCompiling() && Report.CompiledReport == null)
			{
				Report.Compile();
			}
			newReport = StiActivator.CreateObject(report.CompiledReport.GetType()) as StiReport;
			StiReportResourcesLoader.LoadReportResourcesFromReport(Report, newReport);
			newReport.ApplyStyles();
		}
		else if (report.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode)
		{
			MemoryStream memoryStream = new MemoryStream();
			report.Save(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			newReport = new StiReport();
			newReport.Load(memoryStream);
		}
		else
		{
			newReport = StiActivator.CreateObject(report.GetType()) as StiReport;
			StiReportResourcesLoader.LoadReportResourcesFromReport(Report, newReport);
			newReport.ApplyStyles();
		}
		newReport.RegData(report.DataStore);
		newReport.RegBusinessObject(report.BusinessObjectsStore);
		Parameters.CopyVariableStatesToReport(newReport);
		foreach (StiPage page3 in newReport.Pages)
		{
			if (page3.Name == stiPage.Name)
			{
				page3.Enabled = true;
				page3.Skip = false;
			}
			else
			{
				page3.Enabled = false;
			}
		}
		StiComponentsCollection stiComponentsCollection = newReport.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item.Interaction != null && item.Interaction.DrillDownEnabled && item.Interaction.DrillDownPageGuid == stiPage.Guid)
			{
				item.Interaction.DrillDownPage = null;
			}
			if (!(item is StiChart))
			{
				continue;
			}
			StiChart stiChart = item as StiChart;
			foreach (StiSeries item2 in stiChart.Series)
			{
				if (item2.Interaction != null && ((StiSeriesInteraction)item2.Interaction).DrillDownEnabled && ((StiSeriesInteraction)item2.Interaction).DrillDownPageGuid == stiPage.Guid)
				{
					((StiSeriesInteraction)item2.Interaction).DrillDownPage = null;
				}
			}
		}
		FillDrillDownParameters(comp, newReport, seriesInteraction);
		newReport.Rendering += View_Rendering;
		try
		{
			newReport.IsInteractionRendering = true;
			newReport.Render(showProgress: false);
		}
		finally
		{
			newReport.IsInteractionRendering = false;
		}
		newReport.Rendering -= View_Rendering;
		if (stiReport.ReportAlias == newReport.ReportAlias)
		{
			newReport.ReportAlias = (string.IsNullOrEmpty(stiPage.Alias) ? stiPage.Name : stiPage.Alias);
		}
		if (stiReport.ReportDescription == newReport.ReportDescription)
		{
			newReport.ReportDescription = newReport.ReportAlias;
		}
	}

	private bool ProcessGetDrillDownReport(StiComponent comp, StiReport newReport, StiSeriesInteractionData seriesInteraction)
	{
		StiGetDrillDownReportEventArgs stiGetDrillDownReportEventArgs = new StiGetDrillDownReportEventArgs();
		comp.InvokeGetDrillDownReport(comp, stiGetDrillDownReportEventArgs);
		if (stiGetDrillDownReportEventArgs.Cancel || stiGetDrillDownReportEventArgs.Report == null)
		{
			return true;
		}
		FillDrillDownParameters(comp, newReport, seriesInteraction);
		newReport.Rendering += View_Rendering;
		newReport.Render(showProgress: false);
		newReport.Rendering -= View_Rendering;
		return false;
	}

	private void ProcessDrillDown(StiComponent comp, StiInteraction interaction, StiSeriesInteractionData seriesInteraction)
	{
		ReportTabHelper.AddMainReportTabIfNeed();
		StiReport newReport = null;
		string value = null;
		string value2 = null;
		if (interaction != null)
		{
			value = interaction.DrillDownReport;
			value2 = interaction.DrillDownPageGuid;
		}
		if (seriesInteraction?.Series?.Interaction != null)
		{
			value = ((StiSeriesInteraction)seriesInteraction.Series.Interaction).DrillDownReport;
			value2 = ((StiSeriesInteraction)seriesInteraction.Series.Interaction).DrillDownPageGuid;
		}
		if (!string.IsNullOrEmpty(value))
		{
			if (ProcessDrillDownReport(comp, interaction, seriesInteraction, ref newReport))
			{
				return;
			}
		}
		else if (!string.IsNullOrEmpty(value2))
		{
			ProcessDrillDownPage(comp, interaction, seriesInteraction, ref newReport);
		}
		else if (ProcessGetDrillDownReport(comp, newReport, seriesInteraction))
		{
			return;
		}
		if (newReport != null)
		{
			StiDrillDownMode mode = interaction?.DrillDownMode ?? StiDrillDownMode.MultiPage;
			ReportTabHelper.AddReportToTabs(comp?.Page, newReport, mode);
			reportsZooms[newReport] = Zoom;
			reportsHScrollBars[newReport] = ((ScrollBar)HScrollBar).Value;
			reportsVScrollBars[newReport] = ((ScrollBar)VScrollBar).Value;
			ReportTabHelper.SelectLastReportTab();
			StiGuiHelper.ApplyStyle((Control)(object)tabStripReports);
			ReportTabHelper.EnableReportTabs();
		}
	}

	internal void EnableReportTabs()
	{
		if (((Control)tabStripReports).Visible)
		{
			((Control)tabStripReports).Invalidate();
		}
		else
		{
			((Control)tabStripReports).Visible = true;
		}
	}

	internal void SelectLastReportTab()
	{
		tabStripReports.SelectedTabIndex = ((CollectionBase)(object)tabStripReports.Tabs).Count - 1;
		((ScrollBar)HScrollBar).Value = 0;
		((ScrollBar)VScrollBar).Value = 0;
	}

	internal void AddMainReportTabIfNeed()
	{
		if (((CollectionBase)(object)tabStripReports.Tabs).Count == 0)
		{
			AddMainReportToTabs();
		}
	}

	private void FillDrillDownParameters(StiComponent comp, StiReport report, StiSeriesInteractionData seriesInteractionData)
	{
		if (comp.DrillDownParameters != null && comp.DrillDownParameters.Count > 0)
		{
			foreach (KeyValuePair<string, object> drillDownParameter in comp.DrillDownParameters)
			{
				report[drillDownParameter.Key] = drillDownParameter.Value;
			}
		}
		if (seriesInteractionData != null)
		{
			report["Series"] = seriesInteractionData.Series;
			report["SeriesIndex"] = seriesInteractionData.Series.Chart.Series.IndexOf(seriesInteractionData.Series);
			report["SeriesIsElements"] = seriesInteractionData.IsElements;
			report["SeriesTitle"] = seriesInteractionData.Series.CoreTitle;
			if (seriesInteractionData.IsElements)
			{
				report["SeriesArgument"] = seriesInteractionData.Argument;
				report["SeriesValue"] = seriesInteractionData.Value;
				report["SeriesEndValue"] = seriesInteractionData.EndValue;
				report["SeriesPointIndex"] = seriesInteractionData.PointIndex;
				report["SeriesTag"] = seriesInteractionData.Tag;
				report["SeriesHyperlink"] = seriesInteractionData.Hyperlink;
				report["SeriesTooltip"] = seriesInteractionData.Tooltip;
			}
			string reportAlias = GetReportAlias(report, comp, seriesInteractionData);
			report["ReportAlias"] = reportAlias;
		}
	}

	private string GetReportAlias(StiReport report, StiComponent comp, StiSeriesInteractionData seriesInteractionData)
	{
		string expressionReportAlias = GetExpressionReportAlias(comp);
		if (!string.IsNullOrEmpty(expressionReportAlias))
		{
			StiText component = new StiText
			{
				Page = new StiPage(report)
			};
			return StiReportParser.Parse(expressionReportAlias, (IStiReportComponent)(object)component);
		}
		string text = ((seriesInteractionData.Series != null) ? seriesInteractionData.Series.CoreTitle : null);
		string text2 = ((seriesInteractionData.Argument != null) ? seriesInteractionData.Argument.ToString() : null);
		if (string.IsNullOrEmpty(text2))
		{
			text2 = seriesInteractionData.Value.ToString();
		}
		if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
		{
			return text + " - " + text2;
		}
		if (!string.IsNullOrEmpty(text2))
		{
			return text;
		}
		return text2;
	}

	private string GetExpressionReportAlias(StiComponent comp)
	{
		if (comp.Interaction == null)
		{
			return string.Empty;
		}
		if (comp.Interaction.DrillDownParameter1?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter1.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter2?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter2.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter3?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter3.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter4?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter4.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter5?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter5.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter6?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter6.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter7?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter7.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter8?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter8.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter9?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter9.Expression.Value;
		}
		if (comp.Interaction.DrillDownParameter10?.Name == "ReportAlias")
		{
			return comp.Interaction.DrillDownParameter10.Expression.Value;
		}
		return string.Empty;
	}

	private void RefreshInteractions()
	{
		if (Report == null || Report.RenderedPages.CacheMode)
		{
			return;
		}
		StiReport stiReport = ((Report.CompiledReport == null) ? Report : Report.CompiledReport);
		List<StiDataBand> list = new List<StiDataBand>();
		StiComponentsCollection stiComponentsCollection = stiReport.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item is StiDataBand { Sort: not null } stiDataBand && stiDataBand.Sort.Length != 0)
			{
				list.Add(stiDataBand);
			}
		}
		Hashtable listOfInteractions = GetListOfInteractions();
		foreach (StiDataBand item2 in list)
		{
			List<StiInteraction> list2 = new List<StiInteraction>();
			if (!(listOfInteractions[item2.Name] is List<StiInteraction> list3))
			{
				continue;
			}
			foreach (StiInteraction item3 in list3)
			{
				item3.SortingIndex = 0;
				item3.SortingDirection = StiInteractionSortDirection.None;
			}
			int num = 1;
			string text = "";
			bool flag = true;
			int num2 = 0;
			string[] sort = item2.Sort;
			foreach (string text2 in sort)
			{
				if (text2 != "ASC" && text2 != "DESC")
				{
					text = ((text.Length != 0) ? (text + "." + text2) : text2);
				}
				if (text2 == "ASC" || text2 == "DESC" || num2 == item2.Sort.Length - 1)
				{
					if (text.Length > 0)
					{
						foreach (StiInteraction item4 in list3)
						{
							string sortColumnsString = item4.GetSortColumnsString();
							if (text.Contains(sortColumnsString) || text.Contains(sortColumnsString.Replace(".", "_")))
							{
								item4.SortingDirection = ((!flag) ? StiInteractionSortDirection.Descending : StiInteractionSortDirection.Ascending);
								item4.SortingIndex = num;
								list3.Remove(item4);
								list2.Add(item4);
								break;
							}
						}
						text = "";
						num++;
						if (list3.Count == 0)
						{
							break;
						}
					}
					flag = text2 == "ASC";
				}
				num2++;
			}
			if (list2.Count == 1)
			{
				list2[0].SortingIndex = 0;
			}
		}
	}

	private Hashtable GetListOfInteractions()
	{
		Hashtable hashtable = new Hashtable();
		foreach (StiPage renderedPage in CurrentReport.RenderedPages)
		{
			StiComponentsCollection stiComponentsCollection = renderedPage.GetComponents();
			foreach (StiComponent item in stiComponentsCollection)
			{
				if (item == null)
				{
					continue;
				}
				StiInteraction interaction = ((IStiInteraction)item).Interaction;
				if (interaction != null && interaction.SortingEnabled)
				{
					string sortDataBandName = interaction.GetSortDataBandName();
					List<StiInteraction> list = hashtable[sortDataBandName] as List<StiInteraction>;
					if (list == null)
					{
						list = (List<StiInteraction>)(hashtable[sortDataBandName] = new List<StiInteraction>());
					}
					list.Add(interaction);
				}
			}
		}
		return hashtable;
	}

	private void InvokeMouseEnterComponentInteraction(StiComponent comp)
	{
		if (comp is StiContainer stiContainer && stiContainer.Interaction is StiBandInteraction && ((StiBandInteraction)stiContainer.Interaction).SelectionEnabled)
		{
			if (IsInteractionSelectedContainer(stiContainer))
			{
				InteractionSetMouseOverSelectedStyle(stiContainer);
			}
			else
			{
				InteractionSetMouseOverStyle(stiContainer);
			}
			InvalidateComponent(stiContainer);
		}
	}

	private void InvokeMouseLeaveComponentInteraction(StiComponent comp)
	{
		if (comp is StiContainer stiContainer && stiContainer.Interaction is StiBandInteraction && ((StiBandInteraction)stiContainer.Interaction).SelectionEnabled)
		{
			if (IsInteractionSelectedContainer(stiContainer))
			{
				InteractionSetSelectedStyle(stiContainer);
			}
			else
			{
				InteractionRestoreComponentStyle(stiContainer);
			}
			InvalidateComponent(stiContainer);
		}
	}

	private void InvokeClickComponentInteraction(StiComponent comp)
	{
		if (!isLatestLeftMouseButtonPressed || !(comp is StiContainer { Interaction: StiBandInteraction { SelectionEnabled: not false } interaction } stiContainer))
		{
			return;
		}
		InteractionSetSelectedStyle(stiContainer);
		SetInteractionComponentSelected(stiContainer);
		InvalidateComponent(stiContainer);
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		if (storedInteractionComponentSelected != null)
		{
			foreach (StiContainer value in storedInteractionComponentSelected.Values)
			{
				hashtable[value.Name] = CurrentReport.RenderedPages.IndexOf(value.Page);
				hashtable2[value.Name] = value.CollapsingIndex;
			}
		}
		storedInteractionComponentStyles.Clear();
		if (CurrentReport.GetComponentByName(stiContainer.Name) is IStiSelectedLine stiSelectedLine)
		{
			int num = CurrentPrintPage;
			report.Rendering += View_Rendering;
			try
			{
				blockHandMove = true;
				CurrentReport.IsInteractionRendering = true;
				stiSelectedLine.SelectedLine = (interaction.SelectedLine.HasValue ? (interaction.SelectedLine.GetValueOrDefault() + 1) : (stiContainer.CollapsingIndex + 1));
				report.NeedsCompiling = false;
				report.Render(showProgress: false);
				RefreshInteractions();
			}
			finally
			{
				CurrentReport.IsInteractionRendering = false;
				report.Rendering -= View_Rendering;
				RefreshPageNofM();
				((Control)this).Invalidate();
				blockHandMove = false;
			}
			CurrentPrintPage = num;
		}
		if (storedInteractionComponentSelected == null)
		{
			return;
		}
		ICollection values = storedInteractionComponentSelected.Values;
		storedInteractionComponentSelected = new Hashtable();
		foreach (StiContainer item in values)
		{
			int num2 = (int)hashtable[item.Name];
			int index = (int)hashtable2[item.Name];
			if (num2 != -1)
			{
				InteractionFindSelectedContainer(item.Name, index, num2, item.Top);
			}
		}
	}

	private void InteractionFindSelectedContainer(string name, int index, int pageIndex, double top)
	{
		StiContainer stiContainer = null;
		StiPage stiPage = CurrentReport.RenderedPages[pageIndex];
		StiComponentsCollection stiComponentsCollection = stiPage.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item.Name == name && item is StiContainer && ((StiContainer)item).CollapsingIndex == index && ((StiContainer)item).Top == top)
			{
				stiContainer = item as StiContainer;
				break;
			}
		}
		if (stiContainer == null)
		{
			foreach (StiPage renderedPage in CurrentReport.RenderedPages)
			{
				StiComponentsCollection stiComponentsCollection2 = renderedPage.GetComponents();
				foreach (StiComponent item2 in stiComponentsCollection2)
				{
					if (item2.Name == name && item2 is StiContainer && ((StiContainer)item2).CollapsingIndex == index && ((StiContainer)item2).Top == top)
					{
						stiContainer = item2 as StiContainer;
						break;
					}
				}
			}
		}
		if (stiContainer != null)
		{
			InteractionSetSelectedStyle(stiContainer);
			SetInteractionComponentSelected(stiContainer);
			InvalidateComponent(stiContainer);
		}
	}

	private void InteractionStoreComponentStyle(StiContainer container)
	{
		if (storedInteractionComponentStyles == null)
		{
			storedInteractionComponentStyles = new Hashtable();
		}
		if (storedInteractionComponentStyles[container] == null)
		{
			StiStyle stiStyle = new StiStyle();
			stiStyle.GetStyleFromComponent(container, StiStyleElements.All);
			storedInteractionComponentStyles[container] = stiStyle;
		}
	}

	private void InteractionRestoreComponentStyle(StiContainer container)
	{
		if (storedInteractionComponentStyles != null && storedInteractionComponentStyles[container] is StiStyle)
		{
			((StiStyle)storedInteractionComponentStyles[container]).SetStyleToComponent(container);
		}
	}

	private void InteractionSetSelectedStyle(StiContainer container)
	{
		InteractionStoreComponentStyle(container);
		StiBaseStyle stiBaseStyle = Report.Styles["InteractionSelectedStyle"];
		if (stiBaseStyle == null)
		{
			stiBaseStyle = StiOptions.Viewer.InteractionSelectedStyle;
		}
		stiBaseStyle.SetStyleToComponent(container);
	}

	private void InteractionSetMouseOverStyle(StiContainer container)
	{
		InteractionStoreComponentStyle(container);
		StiBaseStyle stiBaseStyle = Report.Styles["InteractionMouseOverStyle"];
		if (stiBaseStyle == null)
		{
			stiBaseStyle = StiOptions.Viewer.InteractionMouseOverStyle;
		}
		stiBaseStyle.SetStyleToComponent(container);
	}

	private void InteractionSetMouseOverSelectedStyle(StiContainer container)
	{
		InteractionStoreComponentStyle(container);
		StiBaseStyle stiBaseStyle = Report.Styles["InteractionMouseOverSelectedStyle"];
		if (stiBaseStyle == null)
		{
			stiBaseStyle = StiOptions.Viewer.InteractionMouseOverSelectedStyle;
		}
		stiBaseStyle.SetStyleToComponent(container);
	}

	private bool IsInteractionSelectedContainer(StiContainer container)
	{
		if (storedInteractionComponentSelected != null)
		{
			return storedInteractionComponentSelected[container.Name] == container;
		}
		return false;
	}

	private void SetInteractionComponentSelected(StiContainer container)
	{
		if (storedInteractionComponentSelected == null)
		{
			storedInteractionComponentSelected = new Hashtable();
		}
		if (storedInteractionComponentSelected[container.Name] is StiContainer stiContainer && stiContainer != container)
		{
			InteractionRestoreComponentStyle(stiContainer);
			InvalidateComponent(stiContainer);
		}
		storedInteractionComponentSelected[container.Name] = container;
	}
}
