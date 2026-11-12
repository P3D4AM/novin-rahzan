using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Exceptions;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.Events;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Properties;
using Stimulsoft.Report.Design.Ribbon;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Design;

public class StiRibbonDesigner : Office2007RibbonForm, IStiDesigner, IStiDesignerBase
{
	private ThumbButtonManager thumbButtonManager;

	private ButtonItem previousSelectedTab;

	private bool lockZoom;

	private bool isPreview;

	private bool lockSliderItemTimerZoom;

	private double stepScale = StiScale.Step;

	private bool initEvents;

	private bool startScreen;

	private IContainer components;

	private StiDesignerControl designerControl;

	private Bar bar1;

	private ButtonItem sbpUnit;

	private ButtonItem sbpComponent;

	private ItemContainer icZoom;

	private ItemContainer icPageViewMode;

	private ButtonItem biZoomPageWidth;

	private ButtonItem biZoomPageHeight;

	private ButtonItem biZoomOnePage;

	private LabelItem lbZoom;

	private SliderItem siZoom;

	private LabelItem liSeparator2;

	private LabelItem labelItem1;

	private ButtonItem biZoom100;

	private Timer timerZoomSlider;

	private LabelItem sbpLocation;

	private LabelItem sbpReportCloudProcess;

	private ExtendedButtonItem sbpReportChecker;

	private ButtonItem tbCentimeters;

	private ButtonItem tbHundredthsOfInch;

	private ButtonItem tbInches;

	private ButtonItem tbMillimeters;

	private Bar tbStatusBar;

	private ItemContainer icPageControl;

	private ButtonItem tbFirstPage;

	private ButtonItem tbPrevPage;

	internal ButtonItem tbPageControl;

	private ButtonItem tbNextPage;

	private ButtonItem tbLastPage;

	internal ExtendedButtonItem sbpReportCheckerViewer;

	private LabelItem liSeparator;

	private ItemContainer itemContainer1;

	private ItemContainer itemContainer2;

	private ButtonItem tbViewModeSinglePage;

	private ButtonItem tbViewModeContinuous;

	private ButtonItem tbViewModeMultiplePages;

	private LabelItem labelItem2;

	private SliderItem siZoomViewer;

	private LabelItem labelItem3;

	private Timer timerZoomSliderViewer;

	private ButtonItem btRefresh;

	private LabelItem sbpPageInfo;

	private ButtonItem btStop;

	internal ProgressBarItem progressState;

	public static bool RunInDesignMode
	{
		get
		{
			return StiOptions.Designer.RunInDesignMode;
		}
		set
		{
			StiOptions.Designer.RunInDesignMode = value;
		}
	}

	public static bool CanDesignerChangeReportFileName
	{
		get
		{
			return StiOptions.Designer.CanDesignerChangeReportFileName;
		}
		set
		{
			StiOptions.Designer.CanDesignerChangeReportFileName = value;
		}
	}

	public static FormWindowState DesignerWindowState
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return StiOptions.Designer.DesignerWindowState;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			StiOptions.Designer.DesignerWindowState = value;
		}
	}

	public static bool ShowDesignerInTaskbar
	{
		get
		{
			return StiOptions.Designer.ShowDesignerInTaskbar;
		}
		set
		{
			StiOptions.Designer.ShowDesignerInTaskbar = value;
		}
	}

	public static bool ShowPreviewInMdi
	{
		get
		{
			return StiOptions.Designer.ShowPreviewInMdi;
		}
		set
		{
			StiOptions.Designer.ShowPreviewInMdi = value;
		}
	}

	public static bool DontSaveDockingPanelsConfig
	{
		get
		{
			return StiOptions.Designer.DontSaveDockingPanelsConfig;
		}
		set
		{
			StiOptions.Designer.DontSaveDockingPanelsConfig = value;
		}
	}

	public static bool DontSaveDesignerConfig
	{
		get
		{
			return StiOptions.Designer.DontSaveDesignerConfig;
		}
		set
		{
			StiOptions.Designer.DontSaveDesignerConfig = value;
		}
	}

	public static bool DontSaveEditorConfig
	{
		get
		{
			return StiOptions.Designer.DontSaveEditorConfig;
		}
		set
		{
			StiOptions.Designer.DontSaveEditorConfig = value;
		}
	}

	public static bool DontAskSaveReport
	{
		get
		{
			return StiOptions.Designer.DontAskSaveReport;
		}
		set
		{
			StiOptions.Designer.DontAskSaveReport = value;
		}
	}

	public static string DefaultDockingPanelsConfigPath
	{
		get
		{
			return StiOptions.Designer.DefaultDockingPanelsConfigPath;
		}
		set
		{
			StiOptions.Designer.DefaultDockingPanelsConfigPath = value;
		}
	}

	public static string DefaultDesignerConfigPath
	{
		get
		{
			return StiOptions.Designer.DefaultDesignerConfigPath;
		}
		set
		{
			StiOptions.Designer.DefaultDesignerConfigPath = value;
		}
	}

	public static string DefaultRibbonDesignerConfigPath
	{
		get
		{
			return StiOptions.Designer.DefaultRibbonDesignerConfigPath;
		}
		set
		{
			StiOptions.Designer.DefaultRibbonDesignerConfigPath = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiDesignerBase DesignerControl => designerControl;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDesignerInfo Info => DesignerControl.Info;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsModalMode
	{
		get
		{
			return designerControl.IsModalMode;
		}
		set
		{
			designerControl.IsModalMode = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiNodesMemory NodesMemory => designerControl.NodesMemory;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool UseAliases
	{
		get
		{
			return designerControl.UseAliases;
		}
		set
		{
			designerControl.UseAliases = value;
		}
	}

	[DefaultValue(null)]
	public StiReport Report
	{
		get
		{
			return designerControl?.Report;
		}
		set
		{
			if (designerControl != null)
			{
				designerControl.Report = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Hashtable TextAliasesHash => designerControl.TextAliasesHash;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection ComponentsOnReport => designerControl.ComponentsOnReport;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection ComponentsOnPage => designerControl.ComponentsOnPage;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection SelectedTextComponentsOnPage => designerControl.SelectedTextComponentsOnPage;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection SelectedComponentsOnPage => designerControl.SelectedComponentsOnPage;

	public static event DragEventHandler DragDropToPage
	{
		add
		{
			StiOptions.Engine.GlobalEvents.DragDropToPageInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.DragDropToPageInDesigner -= value;
		}
	}

	public static event DragEventHandler DragEnterToPage
	{
		add
		{
			StiOptions.Engine.GlobalEvents.DragEnterToPageInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.DragEnterToPageInDesigner -= value;
		}
	}

	public static event EventHandler DragLeaveFromPage
	{
		add
		{
			StiOptions.Engine.GlobalEvents.DragLeaveFromPageInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.DragLeaveFromPageInDesigner -= value;
		}
	}

	public static event DragEventHandler DragOverPage
	{
		add
		{
			StiOptions.Engine.GlobalEvents.DragOverPageInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.DragOverPageInDesigner -= value;
		}
	}

	public static event QueryContinueDragEventHandler QueryContinueDragOverPage
	{
		add
		{
			StiOptions.Engine.GlobalEvents.QueryContinueDragOverPageInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.QueryContinueDragOverPageInDesigner -= value;
		}
	}

	public static event EventHandler LoadingDesigner
	{
		add
		{
			StiOptions.Engine.GlobalEvents.LoadingDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.LoadingDesigner -= value;
		}
	}

	public static event EventHandler LoadedDesigner
	{
		add
		{
			StiOptions.Engine.GlobalEvents.LoadedDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.LoadedDesigner -= value;
		}
	}

	public static event CancelEventHandler ClosingDesigner
	{
		add
		{
			StiOptions.Engine.GlobalEvents.ClosingDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.ClosingDesigner -= value;
		}
	}

	public static event EventHandler ClosedDesigner
	{
		add
		{
			StiOptions.Engine.GlobalEvents.ClosedDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.ClosedDesigner -= value;
		}
	}

	public static event StiSavingObjectEventHandler SavingReport
	{
		add
		{
			StiOptions.Engine.GlobalEvents.SavingReportInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.SavingReportInDesigner -= value;
		}
	}

	public static event EventHandler SavedReport
	{
		add
		{
			StiOptions.Engine.GlobalEvents.SavedReportInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.SavedReportInDesigner -= value;
		}
	}

	public static event StiLoadingObjectEventHandler LoadingReport
	{
		add
		{
			StiOptions.Engine.GlobalEvents.LoadingReportInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.LoadingReportInDesigner -= value;
		}
	}

	public static event EventHandler LoadedReport
	{
		add
		{
			StiOptions.Engine.GlobalEvents.LoadedReportInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.LoadedReportInDesigner -= value;
		}
	}

	public static event StiCreatingObjectEventHandler CreatingReport
	{
		add
		{
			StiOptions.Engine.GlobalEvents.CreatingReportInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.CreatingReportInDesigner -= value;
		}
	}

	public static event EventHandler CreatedReport
	{
		add
		{
			StiOptions.Engine.GlobalEvents.CreatedReportInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.CreatedReportInDesigner -= value;
		}
	}

	public static event EventHandler SavingPage
	{
		add
		{
			StiOptions.Engine.GlobalEvents.SavingPageInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.SavingPageInDesigner -= value;
		}
	}

	public static event EventHandler LoadingPage
	{
		add
		{
			StiOptions.Engine.GlobalEvents.LoadingPageInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.LoadingPageInDesigner -= value;
		}
	}

	public static event EventHandler PreviewingReport
	{
		add
		{
			StiOptions.Engine.GlobalEvents.PreviewingReportInDesigner += value;
		}
		remove
		{
			StiOptions.Engine.GlobalEvents.PreviewingReportInDesigner -= value;
		}
	}

	public static string GetDefaultDesignerConfigPath()
	{
		return StiDesignerControl.GetDefaultDesignerConfigPath();
	}

	public static string GetDefaultDockingPanelsConfigPath()
	{
		return StiDesignerControl.GetDefaultDockingPanelsConfigPath();
	}

	public static string GetDefaultRibbonDockingPanelsConfigPath()
	{
		return StiDesignerControl.GetDefaultRibbonDockingPanelsConfigPath();
	}

	protected override void WndProc(ref Message m)
	{
		if (StiOptions.Windows.Windows7TaskBarSupport && Windows7Taskbar.IsWindowsVistaOrGreater)
		{
			if (((Message)(ref m)).Msg == Windows7Taskbar.TaskbarButtonCreatedMessage)
			{
				IntPtr handle = ((Control)this).Handle;
				Windows7Taskbar.SetThumbnailTooltip(handle, "Stimulsoft Reports Designer");
				thumbButtonManager = new ThumbButtonManager(handle);
				ThumbButton thumbButton = thumbButtonManager.CreateThumbButton(101u, Resources.IcoReportOpen, Loc.Get("MainMenu", "menuFileReportOpen").Replace("&", "").Replace("...", ""));
				thumbButton.Clicked += delegate
				{
					StiActions.MainMenu.InvokePageOpen(designerControl);
				};
				ThumbButton thumbButton2 = thumbButtonManager.CreateThumbButton(102u, Resources.IcoReportSave, Loc.Get("MainMenu", "menuFileReportSave").Replace("&", ""));
				thumbButton2.Clicked += delegate
				{
					if (designerControl.Report != null)
					{
						StiActions.MainMenu.InvokeReportSave(designerControl);
					}
				};
				ThumbButton thumbButton3 = thumbButtonManager.CreateThumbButton(103u, Resources.IcoCreate, Loc.Get("Wizards", "title"));
				thumbButton3.Clicked += delegate
				{
					StiActions.MainMenu.InvokeReportNewBlank(designerControl);
				};
				thumbButtonManager.AddThumbButtons(thumbButton3, thumbButton, thumbButton2);
			}
			if (thumbButtonManager != null)
			{
				thumbButtonManager.DispatchMessage(ref m);
			}
		}
		base.WndProc(ref m);
	}

	public StiQuickButton[] GetQuickButtons(StiComponent component)
	{
		return designerControl.GetQuickButtons(component);
	}

	public void ShowReportCheckerError(StiReportCheckerErrorParameters parameters)
	{
		designerControl.ShowReportCheckerError(parameters);
	}

	public double XToPage(int x)
	{
		return designerControl.XToPage(x);
	}

	public double YToPage(int y)
	{
		return designerControl.YToPage(y);
	}

	public void UndoRedoSave(string name)
	{
		designerControl.UndoRedoSave(name);
	}

	public void UndoRedoSave(string name, bool onlyCurrentPage)
	{
		designerControl.UndoRedoSave(name, onlyCurrentPage);
	}

	public StiComponentsCollection GetSelected()
	{
		return designerControl.GetSelected();
	}

	public void Invalidate(StiComponent component)
	{
		designerControl.Invalidate(component);
	}

	public void ShowDesigner()
	{
		((Control)this).Show();
	}

	public void ShowDesigner(object win32Window)
	{
		((Form)this).Show((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
	}

	public DialogResult ShowDialogDesigner()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).ShowDialog();
	}

	public DialogResult ShowDialogDesigner(object win32Window)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).ShowDialog((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
	}

	internal void RibbonTabPreview_CheckedChanged(object sender, EventArgs e)
	{
		previousSelectedTab = ((CheckedChangedEventArgs)e).OldItem;
		isPreview = ((RibbonTabItem)sender).Checked;
		ShowPreview();
		if (isPreview)
		{
			designerControl.RibbonDesignerUserControl?.ResetShortcutOpenFile();
			designerControl.DesignerPreviewControl.ViewerControl?.InitShortcuts();
		}
		else
		{
			designerControl.RibbonDesignerUserControl?.InitShortcutOpenFile();
			designerControl.DesignerPreviewControl.ViewerControl?.ResetShortcuts();
		}
	}

	private void DesignerControl_RefreshStatusBar(object sender, EventArgs e)
	{
		if (designerControl == null || designerControl.Report == null)
		{
			if (sbpComponent != null)
			{
				sbpComponent.Text = string.Empty;
			}
			if (sbpLocation != null)
			{
				sbpLocation.Text = string.Empty;
			}
			if (sbpUnit != null)
			{
				sbpUnit.Text = string.Empty;
				sbpUnit.Enabled = false;
			}
			sbpReportChecker.Enabled = false;
		}
		else if (Report != null)
		{
			if (sbpUnit != null)
			{
				sbpUnit.Enabled = true;
			}
			if (Report.GetCurrentPage() is IStiForm && sbpUnit != null)
			{
				sbpUnit.Text = Loc.Get("PropertyEnum", "StiReportUnitTypePixels");
			}
			else if (Report.GetCurrentPage() is IStiDashboard && sbpUnit != null)
			{
				sbpUnit.Text = Loc.Get("PropertyEnum", "StiMapTypePoints");
			}
			else
			{
				sbpUnit.Text = Report.GetCurrentPage().Unit.ToString();
			}
			if (designerControl != null && designerControl.SelectedObject != null && sbpComponent != null)
			{
				object selectedObject = designerControl.SelectedObject;
				if (selectedObject is StiReport)
				{
					sbpComponent.Text = ((StiReport)selectedObject).ReportName;
				}
				if (selectedObject is StiComponent)
				{
					sbpComponent.Text = ((StiComponent)selectedObject).Name;
				}
			}
			sbpReportChecker.Enabled = true;
			if (sbpLocation != null && designerControl.CurrentTool != null)
			{
				sbpLocation.Text = designerControl.CurrentTool.GetStatusBarInformation();
			}
		}
		if (sbpUnit != null)
		{
			sbpUnit.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowUnits;
		}
		if (sbpReportChecker != null)
		{
			sbpReportChecker.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowReportChecker;
		}
		if (sbpLocation != null)
		{
			sbpLocation.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowLocation;
		}
		if (sbpComponent != null)
		{
			sbpComponent.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowComponentName;
		}
		if (designerControl.IsDashboardSelected)
		{
			sbpUnit.Visible = false;
		}
		CheckStateStatusBarViewer();
	}

	private void ButtonCentimeters_Click(object sender, EventArgs e)
	{
		designerControl.Report.Unit = new StiCentimetersUnit();
		((Control)designerControl).Refresh();
		designerControl.InvokeSelectedObjectsChanged();
	}

	private void ButtonHundredthsOfInch_Click(object sender, EventArgs e)
	{
		designerControl.Report.Unit = new StiHundredthsOfInchUnit();
		((Control)designerControl).Refresh();
		designerControl.InvokeSelectedObjectsChanged();
	}

	private void ButtonInches_Click(object sender, EventArgs e)
	{
		designerControl.Report.Unit = new StiInchesUnit();
		((Control)designerControl).Refresh();
		designerControl.InvokeSelectedObjectsChanged();
	}

	private void ButtonMillimeters_Click(object sender, EventArgs e)
	{
		designerControl.Report.Unit = new StiMillimetersUnit();
		((Control)designerControl).Refresh();
		designerControl.InvokeSelectedObjectsChanged();
	}

	private void ButtonUnit_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		if (designerControl != null)
		{
			tbCentimeters.Checked = designerControl.Report.Unit is StiCentimetersUnit;
			tbHundredthsOfInch.Checked = designerControl.Report.Unit is StiHundredthsOfInchUnit;
			tbInches.Checked = designerControl.Report.Unit is StiInchesUnit;
			tbMillimeters.Checked = designerControl.Report.Unit is StiMillimetersUnit;
			tbCentimeters.Enabled = true;
			tbHundredthsOfInch.Enabled = true;
			tbInches.Enabled = true;
			tbMillimeters.Enabled = true;
		}
		else
		{
			tbCentimeters.Enabled = false;
			tbHundredthsOfInch.Enabled = false;
			tbInches.Enabled = false;
			tbMillimeters.Enabled = false;
		}
	}

	private void ButtonZoomPageWidth_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoomPageWidth(designerControl);
	}

	private void ButtonZoomPageHeight_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoomPageHeight(designerControl);
	}

	private void ButtonZoomOnePage_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoomOnePage(designerControl);
	}

	private void ButtonZoom100_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoom100(designerControl);
	}

	private void DesignerControl_LocalizationChanged(object sender, EventArgs e)
	{
		Localize();
	}

	private void SliderZoom_ValueChanged(object sender, EventArgs e)
	{
		if (!lockZoom)
		{
			UpdateLabel();
			if (Report != null)
			{
				timerZoomSlider.Enabled = true;
			}
		}
	}

	private void TimerZoomSlider_Tick(object sender, EventArgs e)
	{
		if (Report != null)
		{
			Report.Info.Zoom = (float)siZoom.Value / 100f;
			timerZoomSlider.Enabled = false;
			designerControl.Builder.InvalidateRulers();
			designerControl.Builder.BuildReport();
		}
	}

	private void This_Click(object sender, EventArgs e)
	{
		Point position = Cursor.Position;
		if (position.X > ((Control)this).Width - 10 && position.Y < 30)
		{
			((Form)this).Close();
		}
	}

	private void DesignerControl_RefreshDesigner(object sender, EventArgs e)
	{
		lockZoom = true;
		if (Report != null)
		{
			UpdateLabel();
			siZoom.Value = (int)(Report.Info.Zoom * 100.0);
			siZoom.Enabled = true;
			biZoom100.Enabled = true;
			biZoomOnePage.Enabled = true;
			biZoomPageWidth.Enabled = true;
			biZoomPageHeight.Enabled = true;
			sbpUnit.Enabled = true;
			((Control)tbStatusBar).Enabled = true;
		}
		else
		{
			siZoom.Enabled = false;
			biZoom100.Enabled = false;
			biZoomOnePage.Enabled = false;
			biZoomPageWidth.Enabled = false;
			biZoomPageHeight.Enabled = false;
			sbpUnit.Enabled = false;
			((Control)tbStatusBar).Enabled = false;
		}
		lockZoom = false;
	}

	private void This_Load(object sender, EventArgs e)
	{
		if (CheckUnsaveReport())
		{
			startScreen = false;
		}
		if (startScreen)
		{
			CheckStartScreenMode();
		}
		CheckLicenseExpired();
		RemoveSystemPartsInDesigners();
	}

	public void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl.BlockFormClose)
		{
			previewControl.ViewerControl.BlockFormClose = false;
			((CancelEventArgs)(object)e).Cancel = true;
		}
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
		StiElementActionHelper.Hide();
	}

	protected override void OnMaximumSizeChanged(EventArgs e)
	{
		((Form)this).OnMaximumSizeChanged(e);
		StiElementActionHelper.Hide();
	}

	protected override void OnMinimumSizeChanged(EventArgs e)
	{
		((Form)this).OnMinimumSizeChanged(e);
		StiElementActionHelper.Hide();
	}

	private void Renewal_Click(object sender, EventArgs e)
	{
		if (sender is LabelItem { Tag: not null } labelItem)
		{
			StiProcess.Start(labelItem.Tag.ToString());
		}
	}

	private void ButtonReportChecker_Click(object sender, EventArgs e)
	{
		((StiDesignerControl)DesignerControl).RunReportChecker(sender as ExtendedButtonItem);
	}

	private void TabControl_SelectedTabChanging(object sender, TabStripTabChangingEventArgs e)
	{
		int num = designerControl.TabControl.Tabs.IndexOf(e.NewTab);
		isPreview = num == designerControl.PagePreviewIndex;
		if (StiDashboardHelper.IsDashboardPresent(Report))
		{
			((Control)this).SuspendLayout();
			bar1.Visible = !isPreview;
			tbStatusBar.Visible = isPreview;
			((Control)this).ResumeLayout();
			return;
		}
		((Control)this).SuspendLayout();
		bar1.Visible = !isPreview;
		tbStatusBar.Visible = isPreview;
		((Control)this).ResumeLayout();
		StiDesignerPreviewControl designerPreviewControl = GetDesignerPreviewControl();
		if (designerPreviewControl != null)
		{
			if (isPreview)
			{
				designerPreviewControl.StateRenderChanged += DesignerPreviewControl_StateRenderChanged;
			}
			else
			{
				designerPreviewControl.StateRenderChanged -= DesignerPreviewControl_StateRenderChanged;
			}
		}
	}

	private void DesignerPreviewControl_StateRenderChanged(object sender, EventArgs e)
	{
		StiDesignerPreviewControl stiDesignerPreviewControl = sender as StiDesignerPreviewControl;
		btStop.Visible = stiDesignerPreviewControl.StateRender;
		btRefresh.Visible = !stiDesignerPreviewControl.StateRender;
		sbpReportCheckerViewer.Enabled = !stiDesignerPreviewControl.StateRender;
		icPageControl.Enabled = !stiDesignerPreviewControl.StateRender;
		((Control)tbStatusBar).Invalidate();
	}

	private void GlobalEvents_PreviewingReportInDesigner(object sender, EventArgs e)
	{
		((StiDesignerControl)sender).Report.Progress = new StiPreviewProgressInformation(progressState);
		Application.DoEvents();
	}

	private void ButtonFirstPage_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.FirstPage();
		}
	}

	private void ButtonPrevPage_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.PrevPage();
			CheckStateStatusBarViewer();
		}
	}

	private void ButtonNextPage_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.NextPage();
			CheckStateStatusBarViewer();
		}
	}

	private void ButtonLastPage_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.LastPage();
			CheckStateStatusBarViewer();
		}
	}

	private void ButtonViewModeSinglePage_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.InvokeViewModeSinglePage();
			CheckStateStatusBarViewer();
		}
	}

	private void ButtonViewModeContinuous_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.InvokeViewModeContinuous();
			CheckStateStatusBarViewer();
		}
	}

	private void ButtonViewModeMultiplePages_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.InvokeViewModeMultiplePages();
			CheckStateStatusBarViewer();
		}
	}

	private void ButtonJumpToPage_Click(object sender, EventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null && previewControl.ViewerControl != null)
		{
			previewControl.ViewerControl.JumpToPage();
			CheckStateStatusBarViewer();
		}
	}

	private void ZoomViewer_ValueChanging(object sender, CancelIntValueEventArgs e)
	{
		timerZoomSliderViewer.Enabled = true;
	}

	private void TimerZoomSliderViewer_Tick(object sender, EventArgs e)
	{
		try
		{
			if (lockSliderItemTimerZoom)
			{
				return;
			}
			StiDesignerPreviewControl stiDesignerPreviewControl = designerControl?.Builder?.GetPreviewControl();
			if (stiDesignerPreviewControl != null && stiDesignerPreviewControl.ViewerControl != null)
			{
				if ((decimal)siZoomViewer.Value != (decimal)(stiDesignerPreviewControl.ViewerControl.Zoom * 100.0))
				{
					lockSliderItemTimerZoom = true;
					stiDesignerPreviewControl.ViewerControl.ClearZoomFields();
					stiDesignerPreviewControl.ViewerControl.SetZoom(siZoomViewer.Value);
					lockSliderItemTimerZoom = false;
				}
				timerZoomSliderViewer.Enabled = false;
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonStop_Click(object sender, EventArgs e)
	{
		GetDesignerPreviewControl()?.DoStop();
	}

	private void ButtonRefresh_Click(object sender, EventArgs e)
	{
		if (!designerControl.LockReportPreview)
		{
			StiDesignerPreviewControl designerPreviewControl = GetDesignerPreviewControl();
			if (designerPreviewControl != null)
			{
				CheckStateStatusBarViewer();
				Application.DoEvents();
				designerPreviewControl.DoRefresh();
			}
		}
	}

	internal void ViewerClose(object sender, EventArgs e)
	{
		if (designerControl.RibbonDesignerUserControl.RibbonTabPreview != null)
		{
			if (previousSelectedTab != null)
			{
				previousSelectedTab.Checked = true;
			}
			else
			{
				designerControl.RibbonDesignerUserControl.ribbonTabHome.Checked = true;
			}
		}
		else
		{
			designerControl.TabControl.SelectedTabIndex = 0;
		}
	}

	internal void RefreshControlState(object sender, EventArgs e)
	{
		CheckStateStatusBarViewer();
	}

	private void Actions_OpenReportStart(object sender, EventArgs e)
	{
		sbpReportCloudProcess.Invoke((Action)delegate
		{
			sbpReportCloudProcess.Text = Loc.Get("DesignerFx", "LoadingReport") + "... 0%";
			sbpReportCloudProcess.Visible = true;
		});
	}

	private void Actions_OpenReportProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		sbpReportCloudProcess.Invoke((Action)delegate
		{
			sbpReportCloudProcess.Text = string.Format("{0}... {1}%", Loc.Get("DesignerFx", "LoadingReport"), e.ProgressPercentage);
		});
	}

	private void Actions_OpenReportCompleted(object sender, EventArgs e)
	{
		sbpReportCloudProcess.Invoke((Action)delegate
		{
			sbpReportCloudProcess.Visible = false;
		});
	}

	private void Actions_SaveReportStart(object sender, EventArgs e)
	{
		designerControl.IsCloudSaving = true;
		sbpReportCloudProcess.Invoke((Action)delegate
		{
			sbpReportCloudProcess.Text = Loc.Get("DesignerFx", "SavingReport") + "... 0%";
			sbpReportCloudProcess.Visible = true;
		});
	}

	private void Actions_SaveReportProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		sbpReportCloudProcess.Invoke((Action)delegate
		{
			sbpReportCloudProcess.Text = string.Format("{0}... {1}%", StiLocalization.Get("DesignerFx", "SavingReport"), e.ProgressPercentage);
		});
	}

	private void Actions_SaveReportCompleted(object sender, EventArgs e)
	{
		designerControl.IsCloudSaving = false;
		sbpReportCloudProcess.Invoke((Action)delegate
		{
			sbpReportCloudProcess.Visible = false;
		});
	}

	private static void RemoveSystemPartsInDesigners()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
			{
				Form val = item;
				if (val is StiRibbonDesigner stiRibbonDesigner)
				{
					SubItemsCollection quickToolbarItems = stiRibbonDesigner.RibbonControl.QuickToolbarItems;
					BaseItem baseItem = quickToolbarItems.Cast<BaseItem>().FirstOrDefault((BaseItem i) => i.Name == "sys_caption_icon");
					if (baseItem != null)
					{
						quickToolbarItems.Remove(baseItem);
					}
					baseItem = quickToolbarItems.Cast<BaseItem>().FirstOrDefault((BaseItem i) => i.Name == "sys_caption_separator");
					if (baseItem != null)
					{
						quickToolbarItems.Remove(baseItem);
					}
				}
			}
		}
		catch
		{
		}
	}

	internal static bool IsEmpty(StiReport report)
	{
		if (report == null)
		{
			return true;
		}
		if (report.Pages.Count == 0)
		{
			return true;
		}
		if (report.Pages.Count == 1 && report.Pages[0].Components.Count == 0)
		{
			return true;
		}
		return false;
	}

	internal void ShowPreview()
	{
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		StiElementActionHelper.Hide();
		if (bar1 != null)
		{
			bar1.Visible = !isPreview;
		}
		if (tbStatusBar != null)
		{
			tbStatusBar.Visible = isPreview && !StiDashboardHelper.IsDashboardPresent(designerControl?.Report);
		}
		((Control)this).ResumeLayout();
		if (designerControl != null)
		{
			((Control)designerControl).Visible = !isPreview;
			if (designerControl.IsViewerPresent && designerControl.DesignerPreviewControl != null)
			{
				((Control)designerControl.DesignerPreviewControl).Visible = false;
			}
			if (designerControl.LockTabControlRefresh)
			{
				return;
			}
		}
		if (isPreview)
		{
			((Control)designerControl).Visible = false;
			if (StiDashboardHelper.IsDashboardPresent(designerControl?.Report))
			{
				ShowDashboardPreview();
			}
			else
			{
				ShowReportPreview();
			}
		}
		else
		{
			StiDesignerPreviewControl designerPreviewControl = GetDesignerPreviewControl();
			designerPreviewControl.StateRenderChanged -= DesignerPreviewControl_StateRenderChanged;
			if (Report == null)
			{
				return;
			}
			Report.IsStopped = true;
			Report.IsRendering = false;
			Report.ResetSelection();
			Report.GetCurrentPage().Select();
			designerControl?.AddSelectedToPropertyGrid(updateSelectedObjects: true);
			if (designerControl != null)
			{
				foreach (Control item in (ArrangedElementCollection)((Control)designerControl.TabControl).Controls[designerControl.TabControl.SelectedTabIndex].Controls)
				{
					Control val = item;
					if (val is StiPageViewControl)
					{
						val.Focus();
						break;
					}
				}
			}
			if (designerControl != null && designerControl.IsViewerPresent)
			{
				designerControl.UpdateDesignerPreviewControl1(null, null);
			}
			if (StiDashboardHelper.IsDashboardPresent(designerControl?.Report))
			{
				Control val2 = ((((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0].Name == "dashboardViewerControl") ? ((Control)this).Controls[0] : null);
				if (val2 != null)
				{
					if (val2.Parent != null && val2.Parent.Controls.Contains(val2))
					{
						val2.Parent.Controls.Remove(val2);
					}
					((Component)(object)val2).Dispose();
				}
				designerControl?.RibbonControl?.ReleaseExpanded();
			}
			Report.CompiledReport = null;
			Report.IsStopped = true;
			designerControl?.DesignerPreviewControl.ClearOldReport();
		}
		Form obj = ((Control)this).FindForm();
		if (obj != null)
		{
			((Control)obj).Invalidate();
		}
		StiDesignerControl stiDesignerControl = designerControl;
		if (stiDesignerControl != null)
		{
			((Control)stiDesignerControl).Refresh();
		}
	}

	private void ShowDashboardPreview()
	{
		designerControl.RibbonControl.StopExpanded();
		designerControl.SaveReportBeforePreview();
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		try
		{
			designerControl.Report.ResetRenderedState();
			designerControl.InvokeClearingCompilerResults();
			designerControl.CheckHelper?.BuildReportRenderingMessages(designerControl.Report);
			designerControl.CheckHelper?.BuildChecks(designerControl.Report);
			if (!isPreview)
			{
				return;
			}
			if (designerControl.CheckHelper.ErrorsCount > 0)
			{
				designerControl.InvokeShowingCompilerResults(designerControl.Report.CompilerResults);
			}
			if (designerControl.CheckHelper.ErrorsCount == 0)
			{
				if (designerControl.Report.CompiledReport != null)
				{
					designerControl.Report.CompiledReport.Key = StiKeyHelper.GenerateKey();
					StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(designerControl);
					CreateDashboardViewer(designerControl.Report.CompiledReport);
				}
				else if (designerControl.Report.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode)
				{
					StiReport stiReport = StiReportCopyHelper.Copy(designerControl.Report);
					stiReport.Key = StiKeyHelper.GenerateKey();
					StiOptions.Engine.GlobalEvents.InvokePreviewingReportInDesigner(designerControl);
					CreateDashboardViewer(stiReport);
				}
			}
			if (designerControl.CheckHelper.IsMessagesPresent)
			{
				if (!isPreview)
				{
					return;
				}
				designerControl.IsReportPreviewMode = true;
				StiCheckStatusEventArgs e = new StiCheckStatusEventArgs(designerControl.CheckHelper.ErrorsCount, designerControl.CheckHelper.WarningsCount, designerControl.CheckHelper.InformationMessagesCount, designerControl.CheckHelper.ReportRenderingMessagesCount);
				designerControl.InvokeShowingReportCheckerResults(e);
			}
			else
			{
				designerControl.InvokeClearingReportCheckerResults();
			}
			designerControl.UpdateCheckHelperButton();
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(IStiDesignerBase), ex);
			StiExceptionProvider.Show(ex);
		}
	}

	private void CreateDashboardViewer(StiReport report)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		UserControl viewerControl = StiDashboardViewerAssembly.GetViewerControl(report, designerControl.TabControl.SelectedTabIndex);
		if (viewerControl == null)
		{
			MessageBox.Show((IWin32Window)(object)((Control)this).FindForm(), "Dashboard Viewer from the 'Stimulsoft.Dashboard.Viewer.dll' wasn't found!", "Designer");
			return;
		}
		((Control)viewerControl).Location = new Point(0, designerControl.RibbonControl.GetQatToolbarHeight() + StiScale.YYI(58.0));
		((Control)viewerControl).Name = "dashboardViewerControl";
		((Control)viewerControl).Size = new Size(603, 449);
		((Control)viewerControl).TabIndex = 0;
		if (isPreview)
		{
			((Control)this).Controls.Add((Control)(object)viewerControl);
			((Control)this).Controls.SetChildIndex((Control)(object)viewerControl, 0);
			((Control)viewerControl).Dock = (DockStyle)5;
			((Control)viewerControl).Focus();
			Timer timer = new Timer();
			timer.Interval = 500;
			timer.Tick += delegate
			{
				timer.Stop();
				((Control)viewerControl).Focus();
				((Component)(object)timer).Dispose();
			};
			timer.Start();
		}
	}

	private void ShowReportPreview()
	{
		if (designerControl == null || !designerControl.IsViewerPresent)
		{
			return;
		}
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		StiDesignerPreviewControl designerPreviewControl = GetDesignerPreviewControl();
		designerPreviewControl.StateRenderChanged += DesignerPreviewControl_StateRenderChanged;
		designerControl.SaveReportBeforePreview();
		designerControl.UpdateDesignerPreviewControl2(null, null);
		if (designerControl != null && !((Control)designerControl).Visible)
		{
			((Control)designerControl.DesignerPreviewControl).Visible = true;
			if (designerControl.DesignerPreviewControl != null)
			{
				designerControl.DesignerPreviewControl.ViewerControl.InitShortcuts();
			}
			if (designerControl.RibbonDesignerUserControl != null)
			{
				designerControl.RibbonDesignerUserControl.ResetShortcutOpenFile();
			}
		}
	}

	private void DoScaling()
	{
		string stepName = StiScale.StepName;
		StiResourcesHelper.SetImage((ButtonItem)sbpReportChecker, "Stimulsoft.Report.Design.Resources.Office2013.Check.Checker" + stepName + ".png", scale: false);
		StiResourcesHelper.SetImage((ButtonItem)sbpReportCheckerViewer, "Stimulsoft.Report.Design.Resources.Office2013.Check.Checker" + stepName + ".png", scale: false);
		biZoom100.Image = StiReportDesignImages.Actions.Zoom100();
		biZoomOnePage.Image = StiReportDesignImages.Actions.ZoomSize();
		biZoomPageWidth.Image = StiReportDesignImages.Actions.ZoomWidth();
		biZoomPageHeight.Image = StiReportDesignImages.Actions.ZoomHeight();
		sbpReportChecker.Image = StiReportDesignImages.Actions.Run();
		sbpReportCheckerViewer.Image = StiReportDesignImages.Actions.Run();
		biZoom100.ImagePaddingHorizontal = StiScale.I20;
		biZoom100.ImagePaddingVertical = StiScale.I10;
		biZoomOnePage.ImagePaddingHorizontal = StiScale.I20;
		biZoomOnePage.ImagePaddingVertical = StiScale.I10;
		biZoomPageWidth.ImagePaddingHorizontal = StiScale.I20;
		biZoomPageWidth.ImagePaddingVertical = StiScale.I10;
		biZoomPageHeight.ImagePaddingHorizontal = StiScale.I20;
		biZoomPageHeight.ImagePaddingVertical = StiScale.I10;
		if (stepScale == 1.5)
		{
			tbViewModeSinglePage.Image = (Image)(object)Resources.ViewModeSinglePage_x1_5;
			tbViewModeContinuous.Image = (Image)(object)Resources.ViewModeContinuous_x1_5;
			tbViewModeMultiplePages.Image = (Image)(object)Resources.ViewModeMultiplePages_x1_5;
			tbFirstPage.Image = (Image)(object)Resources.PageFirst_x1_5;
			tbPrevPage.Image = (Image)(object)Resources.PagePrevious_x1_5;
			tbNextPage.Image = (Image)(object)Resources.PageNext_x1_5;
			tbLastPage.Image = (Image)(object)Resources.PageLast_x1_5;
		}
		if (stepScale >= 2.0)
		{
			tbViewModeSinglePage.Image = (Image)(object)Resources.ViewModeSinglePage_x2;
			tbViewModeContinuous.Image = (Image)(object)Resources.ViewModeContinuous_x2;
			tbViewModeMultiplePages.Image = (Image)(object)Resources.ViewModeMultiplePages_x2;
			tbFirstPage.Image = (Image)(object)Resources.PageFirst_x2;
			tbPrevPage.Image = (Image)(object)Resources.PagePrevious_x2;
			tbNextPage.Image = (Image)(object)Resources.PageNext_x2;
			tbLastPage.Image = (Image)(object)Resources.PageLast_x2;
		}
		tbFirstPage.ImagePaddingHorizontal = StiScale.I20;
		tbFirstPage.ImagePaddingVertical = StiScale.I10;
		tbPrevPage.ImagePaddingHorizontal = StiScale.I20;
		tbPrevPage.ImagePaddingVertical = StiScale.I10;
		tbNextPage.ImagePaddingHorizontal = StiScale.I20;
		tbNextPage.ImagePaddingVertical = StiScale.I10;
		tbLastPage.ImagePaddingHorizontal = StiScale.I20;
		tbLastPage.ImagePaddingVertical = StiScale.I10;
		tbViewModeSinglePage.ImagePaddingHorizontal = StiScale.I20;
		tbViewModeSinglePage.ImagePaddingVertical = StiScale.I10;
		tbViewModeContinuous.ImagePaddingHorizontal = StiScale.I20;
		tbViewModeContinuous.ImagePaddingVertical = StiScale.I10;
		tbViewModeMultiplePages.ImagePaddingHorizontal = StiScale.I20;
		tbViewModeMultiplePages.ImagePaddingVertical = StiScale.I10;
		progressState.Width = StiScale.I(96);
		progressState.IsRenderMode = true;
	}

	private void UpdateLabel()
	{
		if (Report != null)
		{
			lbZoom.Text = (int)(Report.Info.Zoom * 100.0) + "%";
		}
	}

	private void Localize()
	{
		biZoom100.Text = Loc.Get("Zoom", "ZoomTo100");
		biZoomOnePage.Text = Loc.Get("Zoom", "OnePage");
		biZoomPageWidth.Text = Loc.Get("Zoom", "PageWidth");
		biZoomPageHeight.Text = Loc.Get("Zoom", "PageHeight");
		biZoom100.Tooltip = Loc.Get("Zoom", "ZoomTo100");
		biZoomOnePage.Tooltip = Loc.Get("Zoom", "OnePage");
		biZoomPageWidth.Tooltip = Loc.Get("Zoom", "PageWidth");
		biZoomPageHeight.Tooltip = Loc.Get("Zoom", "PageHeight");
		tbCentimeters.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeCentimeters");
		tbHundredthsOfInch.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch");
		tbInches.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeInches");
		tbMillimeters.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeMillimeters");
		sbpReportChecker.Text = Loc.Get("MainMenu", "menuCheckIssues");
		sbpReportCheckerViewer.Text = Loc.Get("MainMenu", "menuCheckIssues");
		btRefresh.Text = Loc.Get("PropertyMain", "Refresh");
		btStop.Text = Loc.Get("PropertyMain", "Stop");
	}

	private StiDesignerPreviewControl GetDesignerPreviewControl()
	{
		if (StiOptions.Designer.Ribbon.ShowTabPreview)
		{
			return designerControl.DesignerPreviewControl;
		}
		if (designerControl.PagePreviewIndex >= designerControl.TabControl.Tabs.Count)
		{
			return null;
		}
		ControlCollection controls = designerControl.TabControl.Tabs[designerControl.PagePreviewIndex].AttachedControl.Controls;
		if (((ArrangedElementCollection)controls).Count > 0 && controls[0] is StiDesignerPreviewControl)
		{
			return controls[0] as StiDesignerPreviewControl;
		}
		return null;
	}

	private void SetDelegateViewerEvent(string eventViewer, string methodDesigner)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl != null)
		{
			MethodInfo method = typeof(StiRibbonDesigner).GetMethod(methodDesigner, BindingFlags.Instance | BindingFlags.NonPublic);
			EventInfo @event = previewControl.ViewerControl.Viewer.GetType().GetEvent(eventViewer);
			if (!(@event == null))
			{
				Type eventHandlerType = @event.EventHandlerType;
				Delegate handler = Delegate.CreateDelegate(eventHandlerType, this, method);
				@event.AddEventHandler(previewControl.ViewerControl.Viewer, handler);
			}
		}
	}

	private void CheckStateStatusBarViewer()
	{
		if (designerControl.Builder == null || !isPreview)
		{
			return;
		}
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl == null || previewControl.ViewerControl == null)
		{
			return;
		}
		if (!initEvents)
		{
			initEvents = true;
			SetDelegateViewerEvent("Close", "ViewerClose");
			SetDelegateViewerEvent("RefreshControlState", "RefreshControlState");
		}
		tbFirstPage.Enabled = previewControl.ViewerControl.GetFirstPageEnabled;
		tbPrevPage.Enabled = previewControl.ViewerControl.GetPreviousPageEnabled;
		icPageControl.Enabled = previewControl.ViewerControl.GetPageControlEnabled;
		tbNextPage.Enabled = previewControl.ViewerControl.GetNextPageEnabled;
		tbLastPage.Enabled = previewControl.ViewerControl.GetLastPageEnabled;
		tbPageControl.Enabled = previewControl.ViewerControl.GetPageControlEnabled;
		tbViewModeSinglePage.Enabled = previewControl.ViewerControl.GetViewModeSinglePageEnabled;
		tbViewModeContinuous.Enabled = previewControl.ViewerControl.GetViewModeContinuousEnabled;
		tbViewModeMultiplePages.Enabled = previewControl.ViewerControl.GetViewModeMultiplePagesEnabled;
		tbFirstPage.Visible = previewControl.ViewerControl.GetFirstPageVisibility;
		tbPrevPage.Visible = previewControl.ViewerControl.GetPreviousPageVisibility;
		icPageControl.Visible = previewControl.ViewerControl.GetPageControlVisibility;
		tbNextPage.Visible = previewControl.ViewerControl.GetNextPageVisibility;
		tbLastPage.Visible = previewControl.ViewerControl.GetLastPageVisibility;
		tbViewModeSinglePage.Visible = previewControl.ViewerControl.GetViewModeSinglePageVisibility;
		tbViewModeContinuous.Visible = previewControl.ViewerControl.GetViewModeContinuousVisibility;
		tbViewModeMultiplePages.Visible = previewControl.ViewerControl.GetViewModeMultiplePagesVisibility;
		siZoomViewer.Value = (int)(previewControl.ViewerControl.Zoom * 100.0);
		labelItem2.Text = $"{siZoomViewer.Value}%";
		StiPageViewMode getPageViewMode = previewControl.ViewerControl.GetPageViewMode;
		tbViewModeSinglePage.Checked = getPageViewMode == StiPageViewMode.SinglePage;
		tbViewModeContinuous.Checked = getPageViewMode == StiPageViewMode.Continuous;
		tbViewModeMultiplePages.Checked = getPageViewMode == StiPageViewMode.MultiplePages;
		if (!tbViewModeSinglePage.Visible)
		{
			if (tbViewModeContinuous.Visible)
			{
				tbViewModeContinuous.BeginGroup = true;
			}
			else if (tbViewModeMultiplePages.Visible)
			{
				tbViewModeMultiplePages.BeginGroup = true;
			}
		}
		else
		{
			if (tbViewModeContinuous.BeginGroup)
			{
				tbViewModeContinuous.BeginGroup = false;
			}
			if (tbViewModeMultiplePages.BeginGroup)
			{
				tbViewModeMultiplePages.BeginGroup = false;
			}
		}
		previewControl.ViewerControl.RefreshPageNofM();
		tbPageControl.Text = previewControl.ViewerControl.GetPageNofM();
		tbViewModeSinglePage.Refresh();
		tbViewModeContinuous.Refresh();
		tbViewModeMultiplePages.Refresh();
		((Control)tbStatusBar).Invalidate();
		tbPageControl.Refresh();
	}

	private void AttachCloudReportActions()
	{
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiAccountSettings.CurrentAccount.Actions.SaveReportStart += Actions_SaveReportStart;
			StiAccountSettings.CurrentAccount.Actions.SaveReportCompleted += Actions_SaveReportCompleted;
			StiAccountSettings.CurrentAccount.Actions.SaveReportProgressChanged += Actions_SaveReportProgressChanged;
			StiAccountSettings.CurrentAccount.Actions.OpenReportStart += Actions_OpenReportStart;
			StiAccountSettings.CurrentAccount.Actions.OpenReportCompleted += Actions_OpenReportCompleted;
			StiAccountSettings.CurrentAccount.Actions.OpenReportProgressChanged += Actions_OpenReportProgressChanged;
		}
	}

	private void SetWarningText(string textLong, string textShot, string url, EventHandler click)
	{
		labelItem1.Text = textShot;
		labelItem1.Tooltip = textLong;
		labelItem1.Tag = url;
		labelItem1.Click += click;
	}

	private void ClearWarningText()
	{
		labelItem1.Text = string.Empty;
		labelItem1.Tooltip = string.Empty;
		labelItem1.Tag = null;
		labelItem1.Click -= Renewal_Click;
	}

	private Color GetWarningColor(int outDay)
	{
		switch (outDay)
		{
		case 0:
		case 10:
			return Color.Yellow;
		case 30:
			return Color.White;
		default:
			return Color.White;
		}
	}

	internal void CheckLicenseExpired()
	{
		bool flag = !StiLicenseKeyValidator.IsValidOnAnyPlatform(StiLicense.LicenseKey);
		if (flag)
		{
			SetWarningText(string.Empty, StiLocalizationExt.Get("Publish", "TrialVersion"), string.Empty, null);
		}
		if (StiAccountSettings.CurrentAccount == null || flag)
		{
			return;
		}
		string textLongExpired = string.Empty;
		string textShotExpired = string.Empty;
		string renewelUrl = string.Empty;
		int outDay = 0;
		using BackgroundWorker backgroundWorker = new BackgroundWorker();
		backgroundWorker.DoWork += delegate
		{
			GetTextLicenseExpired(out textLongExpired, out textShotExpired, out renewelUrl, out outDay);
		};
		backgroundWorker.RunWorkerCompleted += delegate
		{
			ClearWarningText();
			if (!string.IsNullOrEmpty(textLongExpired) && !string.IsNullOrEmpty(textShotExpired))
			{
				SetWarningText(textLongExpired, textShotExpired, renewelUrl, Renewal_Click);
				labelItem1.ForeColor = GetWarningColor(outDay);
			}
		};
		backgroundWorker.RunWorkerAsync();
	}

	private void GetTextLicenseExpired(out string longText, out string shotText, out string renewelUrl, out int outDay)
	{
		longText = string.Empty;
		shotText = string.Empty;
		renewelUrl = string.Empty;
		outDay = 0;
		if (!StiAccountSettings.CurrentAccount.AccountChecker.GetProductExpiresInfo(out longText, out shotText, out renewelUrl))
		{
			if (StiAccountSettings.CurrentAccount.AccountChecker.GetProductExpiresInfo(10, out longText, out shotText, out renewelUrl))
			{
				outDay = 10;
			}
			else if (StiAccountSettings.CurrentAccount.AccountChecker.GetProductExpiresInfo(30, out longText, out shotText, out renewelUrl))
			{
				outDay = 30;
			}
		}
	}

	private void CheckStartScreenMode()
	{
		if (StiAccountSettings.CurrentAccount != null && StiSettings.GetStr("Designer", "StartScreen", "Welcome") == "Welcome")
		{
			ApplicationButton ribbonApplicationButton = designerControl.GetRibbonApplicationButton();
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			stiRibbonBackStageUserControl.EnableStartScreen();
			if (stiRibbonBackStageUserControl.StartScreenVisible)
			{
				ribbonApplicationButton.Expanded = true;
			}
		}
	}

	private bool CheckUnsaveReport()
	{
		if (StiAccountSettings.CurrentAccount == null)
		{
			return false;
		}
		try
		{
			if (StiUnsaveReportRepository.IsAvailableReports())
			{
				IStiUnsaveReportWindow unsaveReportWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetUnsaveReportWindow();
				if (unsaveReportWindow.ShowDialog(((Control)this).Handle).GetValueOrDefault())
				{
					ApplicationButton ribbonApplicationButton = designerControl.GetRibbonApplicationButton();
					ribbonApplicationButton.Expanded = false;
					designerControl.OpenUnsavedReportFile(unsaveReportWindow.SelectedReportPath);
					StiUnsaveReportRepository.DeleteUnsavedReportFile(unsaveReportWindow.SelectedReportPath);
					return true;
				}
				StiUnsaveReportRepository.DeleteRootUnsavedReportFolder();
			}
		}
		catch
		{
		}
		return false;
	}

	protected override void OnClosed(EventArgs e)
	{
		((Form)this).OnClosed(e);
		StiFormHelper.SaveStateForm((Form)(object)this, "DesignerWindow", allowMaximize: true);
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		base.OnPaint(e);
		if (!isPreview)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		Point point = ((Control)this).PointToClient(((Control)designerControl.RibbonControl).PointToScreen(new Point(0, ((Control)designerControl.RibbonControl).Bottom - 1)));
		Point point2 = new Point(0, ((Control)this).Height - 1);
		Bar statusBar = GetStatusBar();
		if (statusBar != null)
		{
			point2 = ((Control)this).PointToClient(((Control)statusBar).Parent.PointToScreen(new Point(0, ((Control)statusBar).Top)));
		}
		Control val = ((((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0].Name == "dashboardViewerControl") ? ((Control)this).Controls[0] : null);
		if (val != null)
		{
			Type type = ((object)val).GetType();
			IStiDashboard surface = type.GetProperty("Dashboard").GetValue(val, null) as IStiDashboard;
			Color dashboardBackColor = StiDashboardStyleHelper.GetDashboardBackColor(surface, isViewer: true);
			StiDrawing.FillRectangle(graphics, dashboardBackColor, 1.0, point.Y, 6.0, point2.Y - point.Y);
			StiDrawing.FillRectangle(graphics, dashboardBackColor, ((Control)this).Width - 6, point.Y, 5.0, point2.Y - point.Y);
			if (statusBar == null)
			{
				StiDrawing.FillRectangle(graphics, dashboardBackColor, 1.0, val.Bottom, ((Control)this).Width - 2, 1.0);
			}
			return;
		}
		SolidBrush val2 = new SolidBrush(Color.FromArgb(230, 230, 230));
		try
		{
			graphics.FillRectangle((Brush)(object)val2, 1, point.Y, 6, point2.Y - point.Y);
			graphics.FillRectangle((Brush)(object)val2, ((Control)this).Width - 6, point.Y, 5, point2.Y - point.Y);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public StiRibbonDesigner()
		: this(null)
	{
	}

	public StiRibbonDesigner(StiReport report)
		: this(report, showWelcomeScreen: false)
	{
	}

	public StiRibbonDesigner(StiReport report, bool showWelcomeScreen)
		: this(report, null, showWelcomeScreen)
	{
	}

	public StiRibbonDesigner(StiReport report, string cloudReport, bool showWelcomeScreen)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Invalid comparison between Unknown and I4
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		((Form)this).KeyPreview = true;
		startScreen = showWelcomeScreen;
		if (!IsEmpty(report))
		{
			startScreen = false;
		}
		if (startScreen)
		{
			report = null;
		}
		DoScaling();
		siZoom.LabelWidth = StiScale.XXI(36.0);
		siZoom.Width = StiScale.XXI(130.0);
		siZoomViewer.LabelWidth = StiScale.XXI(36.0);
		siZoomViewer.Width = StiScale.XXI(130.0);
		lbZoom.Width = StiScale.XXI(60.0);
		labelItem2.Width = StiScale.XXI(60.0);
		((Form)this).ShowInTaskbar = StiOptions.Designer.ShowDesignerInTaskbar;
		base.WindowState = DesignerWindowState;
		((Form)this).StartPosition = (FormStartPosition)0;
		if (StiOptions.Designer.DesignerIcon != null)
		{
			((Form)this).Icon = StiOptions.Designer.DesignerIcon;
		}
		Point position = Point.Empty;
		Size size = StiScale.SizeI(664, 446);
		FormWindowState wndState = (FormWindowState)0;
		StiFormHelper.LoadStateForm((Form)(object)this, "DesignerWindow", ref position, ref size, ref wndState, allowMaximize: true);
		if (StiOptions.Designer.DesignerWindowSize.HasValue)
		{
			size = StiOptions.Designer.DesignerWindowSize.GetValueOrDefault();
		}
		if ((int)wndState == 0)
		{
			base.WindowState = (FormWindowState)0;
		}
		SetBoundsCore(position.X, position.Y, size.Width, size.Height, (BoundsSpecified)15);
		if ((int)wndState == 2)
		{
			base.WindowState = (FormWindowState)2;
		}
		EnableGlass = false;
		StiDesignerControl stiDesignerControl = new StiDesignerControl(report, cloudReport, isRibbonGuiMode: true);
		((Control)stiDesignerControl).Padding = new Padding(0, 3, 0, 0);
		((Control)stiDesignerControl).Dock = (DockStyle)5;
		((Control)stiDesignerControl).Location = new Point(0, 0);
		((Control)stiDesignerControl).Name = "designerControl";
		((Control)stiDesignerControl).Size = StiScale.SizeI(603, 449);
		((Control)stiDesignerControl).TabIndex = 0;
		stiDesignerControl.ShowRibbonStatusBar = false;
		designerControl = stiDesignerControl;
		((Control)this).Controls.Add((Control)(object)designerControl);
		((Control)this).Controls.SetChildIndex((Control)(object)designerControl, 0);
		StiRibbonDesignerUserControl ribbon = new StiRibbonDesignerUserControl(designerControl)
		{
			RibbonDesigner = this
		};
		designerControl.RibbonControl = ribbon.RibbonControl;
		((Control)this).Controls.Add((Control)(object)ribbon.RibbonControl);
		ribbon.RibbonControl.SetStyle();
		designerControl.EnableRibbonBackState();
		StiRibbonToolboxToolbarService stiRibbonToolboxToolbarService = StiRibbonToolboxToolbarService.GetService(designerControl);
		if (stiRibbonToolboxToolbarService == null)
		{
			stiRibbonToolboxToolbarService = new StiRibbonToolboxToolbarService();
		}
		stiRibbonToolboxToolbarService.Init(designerControl);
		StiRibbonGuiService service = StiRibbonGuiService.GetService();
		ribbon.ShowLocalizationMenu = service.ShowLocalizationMenu;
		ribbon.ShowOfficeButton = service.ShowOfficeButton;
		ribbon.ShowTabHome = service.ShowTabHome;
		ribbon.ShowTabHomeBarAlignment = service.ShowTabHomeBarAlignment;
		ribbon.ShowTabHomeBarBorders = service.ShowTabHomeBarBorders;
		ribbon.ShowTabHomeBarClipboard = service.ShowTabHomeBarClipboard;
		ribbon.ShowTabHomeBarFont = service.ShowTabHomeBarFont;
		ribbon.ShowTabHomeBarStyle = service.ShowTabHomeBarStyle;
		ribbon.ShowTabHomeBarTextFormat = service.ShowTabHomeBarTextFormat;
		ribbon.ShowTabLayout = service.ShowTabLayout;
		ribbon.ShowTabLayoutBarArrange = service.ShowTabLayoutBarArrange;
		ribbon.ShowTabPage = service.ShowTabPage;
		ribbon.ShowTabPageBarPageSetup = service.ShowTabPageBarPageSetup;
		ribbon.ShowTabViewBarViewOptions = service.ShowTabViewBarViewOptions;
		ribbon.ShowMainMenuNew = service.ShowMainMenuNew;
		ribbon.ShowMainMenuReportOpen = service.ShowMainMenuReportOpen;
		ribbon.ShowMainMenuReportSave = service.ShowMainMenuReportSave;
		ribbon.ShowMainMenuReportSaveAs = service.ShowMainMenuReportSaveAs;
		ribbon.ShowMainMenuCheckForIssues = service.ShowMainMenuCheckForIssues;
		ribbon.ShowMainMenuReportSetup = service.ShowMainMenuReportSetup;
		ribbon.ShowMainMenuRecentFiles = service.ShowMainMenuRecentFiles;
		ribbon.ShowMainMenuClose = service.ShowMainMenuClose;
		ribbon.ShowMainMenuOptions = service.ShowMainMenuOptions;
		ribbon.ShowMainMenuExit = service.ShowMainMenuExit;
		ribbon.ShowTabPreview = service.ShowTabPreview;
		if (!ribbon.ShowMainMenuNew)
		{
			ribbon.ShowMainMenuReportWizardNew = false;
		}
		if (StiOSVersion.IsWindows11())
		{
			base.IsWin11 = true;
			base.TopLeftCornerSize = (base.TopRightCornerSize = (base.BottomLeftCornerSize = (base.BottomRightCornerSize = StiScale.I8)));
		}
		if (designerControl.StyleTool != null)
		{
			designerControl.StyleTool.Deactivated += delegate
			{
				ribbon.CopyStyleButton.Checked = false;
			};
		}
		UpdateLabel();
		if (report != null)
		{
			siZoom.Value = (int)report.Info.Zoom * 100;
		}
		designerControl.LocalizationChanged += DesignerControl_LocalizationChanged;
		designerControl.RefreshDesigner += DesignerControl_RefreshDesigner;
		designerControl.RefreshStatusBar += DesignerControl_RefreshStatusBar;
		designerControl.TabControl.SelectedTabChanging += TabControl_SelectedTabChanging;
		Localize();
		((Control)this).Text = designerControl.GetDesignerTitle();
		designerControl.ShowingReportCheckerResults += delegate(object sender, StiCheckStatusEventArgs e)
		{
			designerControl.UpdateCheckHelperButton(sbpReportChecker, e.Errors, e.Warnings, e.InformationMessages, e.RenderingMessages);
		};
		designerControl.ClearingReportCheckerResults += delegate
		{
			designerControl.RefreshCheckHelperButton(sbpReportChecker);
		};
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		AttachCloudReportActions();
		StiOptions.Engine.GlobalEvents.PreviewingReportInDesigner += GlobalEvents_PreviewingReportInDesigner;
		((Form)this).Load += This_Load;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
		if (designerControl != null)
		{
			designerControl.LocalizationChanged -= DesignerControl_LocalizationChanged;
			designerControl.RefreshDesigner -= DesignerControl_RefreshDesigner;
			designerControl.RefreshStatusBar -= DesignerControl_RefreshStatusBar;
			designerControl = null;
		}
	}

	private void InitializeComponent()
	{
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Expected O, but got Unknown
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Expected O, but got Unknown
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d3: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonDesigner));
		bar1 = new Bar();
		sbpUnit = new ButtonItem();
		tbCentimeters = new ButtonItem();
		tbHundredthsOfInch = new ButtonItem();
		tbInches = new ButtonItem();
		tbMillimeters = new ButtonItem();
		sbpReportChecker = new ExtendedButtonItem();
		sbpComponent = new ButtonItem();
		sbpLocation = new LabelItem();
		sbpReportCloudProcess = new LabelItem();
		labelItem1 = new LabelItem();
		icZoom = new ItemContainer();
		icPageViewMode = new ItemContainer();
		biZoomPageWidth = new ButtonItem();
		biZoomPageHeight = new ButtonItem();
		biZoomOnePage = new ButtonItem();
		biZoom100 = new ButtonItem();
		lbZoom = new LabelItem();
		siZoom = new SliderItem();
		liSeparator2 = new LabelItem();
		timerZoomSlider = new Timer(components);
		tbStatusBar = new Bar();
		icPageControl = new ItemContainer();
		tbFirstPage = new ButtonItem();
		tbPrevPage = new ButtonItem();
		tbPageControl = new ButtonItem();
		tbNextPage = new ButtonItem();
		tbLastPage = new ButtonItem();
		sbpReportCheckerViewer = new ExtendedButtonItem();
		btRefresh = new ButtonItem();
		btStop = new ButtonItem();
		progressState = new ProgressBarItem();
		sbpPageInfo = new LabelItem();
		liSeparator = new LabelItem();
		itemContainer1 = new ItemContainer();
		itemContainer2 = new ItemContainer();
		tbViewModeSinglePage = new ButtonItem();
		tbViewModeContinuous = new ButtonItem();
		tbViewModeMultiplePages = new ButtonItem();
		labelItem2 = new LabelItem();
		siZoomViewer = new SliderItem();
		labelItem3 = new LabelItem();
		timerZoomSliderViewer = new Timer(components);
		((ISupportInitialize)bar1).BeginInit();
		((ISupportInitialize)tbStatusBar).BeginInit();
		((Control)this).SuspendLayout();
		((Control)bar1).AccessibleDescription = "DotNetBar Bar (bar1)";
		((Control)bar1).AccessibleName = "DotNetBar Bar";
		((Control)bar1).AccessibleRole = (AccessibleRole)23;
		bar1.BarType = eBarType.StatusBar;
		((Control)bar1).Dock = (DockStyle)2;
		((Control)bar1).Font = new Font("Segoe UI", 9f);
		bar1.Items.AddRange(new BaseItem[7] { sbpUnit, sbpReportChecker, sbpComponent, sbpLocation, sbpReportCloudProcess, labelItem1, icZoom });
		bar1.Location = new Point(5, 543);
		((Control)bar1).Margin = new Padding(0);
		bar1.Name = "bar1";
		bar1.Size = new Size(1190, 26);
		bar1.Stretch = true;
		bar1.Style = eDotNetBarStyle.Office2007;
		bar1.TabIndex = 0;
		bar1.TabStop = false;
		((Control)bar1).Text = "bar1";
		sbpUnit.AutoExpandOnClick = true;
		sbpUnit.Name = "sbpUnit";
		sbpUnit.PopupSide = ePopupSide.Top;
		sbpUnit.SubItems.AddRange(new BaseItem[4] { tbCentimeters, tbHundredthsOfInch, tbInches, tbMillimeters });
		sbpUnit.Text = "Unit";
		sbpUnit.PopupOpen += ButtonUnit_PopupOpen;
		tbCentimeters.Name = "tbCentimeters";
		tbCentimeters.Text = "Centimeters";
		tbCentimeters.Click += ButtonCentimeters_Click;
		tbHundredthsOfInch.Name = "tbHundredthsOfInch";
		tbHundredthsOfInch.Text = "HundredthsOfInch";
		tbHundredthsOfInch.Click += ButtonHundredthsOfInch_Click;
		tbInches.Name = "tbInches";
		tbInches.Text = "Inches";
		tbInches.Click += ButtonInches_Click;
		tbMillimeters.Name = "tbMillimeters";
		tbMillimeters.Text = "Millimeters";
		tbMillimeters.Click += ButtonMillimeters_Click;
		sbpReportChecker.AutoExpandOnClick = true;
		sbpReportChecker.BeginGroup = true;
		sbpReportChecker.ButtonStyle = eButtonStyle.ImageAndText;
		sbpReportChecker.Image = (Image)componentResourceManager.GetObject("sbpReportChecker.Image");
		sbpReportChecker.Name = "sbpReportChecker";
		sbpReportChecker.Text = "Report Checker";
		sbpReportChecker.Click += ButtonReportChecker_Click;
		sbpComponent.BeginGroup = true;
		sbpComponent.Name = "sbpComponent";
		sbpComponent.Text = "Component";
		sbpLocation.BeginGroup = true;
		sbpLocation.Name = "sbpLocation";
		sbpLocation.Text = "Location";
		sbpReportCloudProcess.BeginGroup = true;
		sbpReportCloudProcess.Name = "sbpReportCloudProcess";
		sbpReportCloudProcess.Visible = false;
		labelItem1.ItemAlignment = eItemAlignment.Far;
		labelItem1.Name = "labelItem1";
		icZoom.BackgroundStyle.Class = "Office2007StatusBarBackground2";
		icZoom.BackgroundStyle.CornerType = eCornerType.Square;
		icZoom.HorizontalItemAlignment = eHorizontalItemsAlignment.Right;
		icZoom.Name = "icZoom";
		icZoom.SubItems.AddRange(new BaseItem[4] { icPageViewMode, lbZoom, siZoom, liSeparator2 });
		icZoom.TitleStyle.CornerType = eCornerType.Square;
		icPageViewMode.BackgroundStyle.CornerType = eCornerType.Square;
		icPageViewMode.BeginGroup = true;
		icPageViewMode.Name = "icPageViewMode";
		icPageViewMode.SubItems.AddRange(new BaseItem[4] { biZoomPageWidth, biZoomPageHeight, biZoomOnePage, biZoom100 });
		icPageViewMode.TitleStyle.CornerType = eCornerType.Square;
		biZoomPageWidth.Image = (Image)componentResourceManager.GetObject("biZoomPageWidth.Image");
		biZoomPageWidth.ImageIndex = 26;
		biZoomPageWidth.ImagePaddingHorizontal = 6;
		biZoomPageWidth.ImagePaddingVertical = 4;
		biZoomPageWidth.Name = "biZoomPageWidth";
		biZoomPageWidth.Text = "Page Width";
		biZoomPageWidth.Click += ButtonZoomPageWidth_Click;
		biZoomPageHeight.Image = (Image)componentResourceManager.GetObject("biZoomPageHeight.Image");
		biZoomPageHeight.ImageIndex = 27;
		biZoomPageHeight.ImagePaddingHorizontal = 6;
		biZoomPageHeight.ImagePaddingVertical = 4;
		biZoomPageHeight.Name = "biZoomPageHeight";
		biZoomPageHeight.Text = "Page Height";
		biZoomPageHeight.Click += ButtonZoomPageHeight_Click;
		biZoomOnePage.Image = (Image)componentResourceManager.GetObject("biZoomOnePage.Image");
		biZoomOnePage.ImageIndex = 28;
		biZoomOnePage.ImagePaddingHorizontal = 6;
		biZoomOnePage.ImagePaddingVertical = 4;
		biZoomOnePage.Name = "biZoomOnePage";
		biZoomOnePage.Text = "One Page";
		biZoomOnePage.Click += ButtonZoomOnePage_Click;
		biZoom100.Image = (Image)componentResourceManager.GetObject("biZoom100.Image");
		biZoom100.Name = "biZoom100";
		biZoom100.Text = "buttonItem3";
		biZoom100.Click += ButtonZoom100_Click;
		lbZoom.Name = "lbZoom";
		lbZoom.Text = "100%";
		lbZoom.TextAlignment = (StringAlignment)1;
		lbZoom.Width = 40;
		siZoom.ClickRepeatInterval = 50;
		siZoom.LabelVisible = false;
		siZoom.LabelWidth = 36;
		siZoom.Maximum = 400;
		siZoom.Minimum = 10;
		siZoom.Name = "siZoom";
		siZoom.Step = 10;
		siZoom.Text = "100%";
		siZoom.Value = 100;
		siZoom.Width = 130;
		siZoom.ValueChanged += SliderZoom_ValueChanged;
		liSeparator2.Name = "liSeparator2";
		liSeparator2.Width = 10;
		timerZoomSlider.Tick += TimerZoomSlider_Tick;
		((Control)tbStatusBar).AccessibleDescription = "";
		((Control)tbStatusBar).AccessibleName = "";
		((Control)tbStatusBar).AccessibleRole = (AccessibleRole)22;
		tbStatusBar.BarType = eBarType.StatusBar;
		((Control)tbStatusBar).Dock = (DockStyle)2;
		((Control)tbStatusBar).Font = new Font("Segoe UI", 9f);
		tbStatusBar.Items.AddRange(new BaseItem[8] { icPageControl, sbpReportCheckerViewer, btRefresh, btStop, progressState, sbpPageInfo, liSeparator, itemContainer1 });
		tbStatusBar.Location = new Point(5, 569);
		((Control)tbStatusBar).Margin = new Padding(0);
		tbStatusBar.Name = "tbStatusBar";
		tbStatusBar.RoundCorners = false;
		tbStatusBar.Size = new Size(1190, 26);
		tbStatusBar.Stretch = true;
		tbStatusBar.Style = eDotNetBarStyle.Office2007;
		tbStatusBar.TabIndex = 74;
		tbStatusBar.TabStop = false;
		((Control)tbStatusBar).Text = "tbStatusBar";
		tbStatusBar.Visible = false;
		icPageControl.BackgroundStyle.CornerType = eCornerType.Square;
		icPageControl.BeginGroup = true;
		icPageControl.Name = "icPageControl";
		icPageControl.SubItems.AddRange(new BaseItem[5] { tbFirstPage, tbPrevPage, tbPageControl, tbNextPage, tbLastPage });
		icPageControl.TitleStyle.CornerType = eCornerType.Square;
		tbFirstPage.BeginGroup = true;
		tbFirstPage.Image = (Image)(object)Resources.PageFirst;
		tbFirstPage.ImageIndex = 4;
		tbFirstPage.Name = "tbFirstPage";
		tbFirstPage.Text = "First Page";
		tbFirstPage.Click += ButtonFirstPage_Click;
		tbPrevPage.Image = (Image)(object)Resources.PagePrevious;
		tbPrevPage.ImageIndex = 5;
		tbPrevPage.Name = "tbPrevPage";
		tbPrevPage.Text = "buttonItem3";
		tbPrevPage.Click += ButtonPrevPage_Click;
		tbPageControl.AutoExpandOnClick = true;
		tbPageControl.Name = "tbPageControl";
		tbPageControl.PopupSide = ePopupSide.Top;
		tbPageControl.Shortcuts.Add(eShortcut.CtrlG);
		tbPageControl.Text = "Page 1 of 3";
		tbPageControl.Click += ButtonJumpToPage_Click;
		tbNextPage.Image = (Image)(object)Resources.PageNext;
		tbNextPage.ImageIndex = 6;
		tbNextPage.Name = "tbNextPage";
		tbNextPage.Text = "buttonItem4";
		tbNextPage.Click += ButtonNextPage_Click;
		tbLastPage.Image = (Image)(object)Resources.PageLast;
		tbLastPage.ImageIndex = 7;
		tbLastPage.Name = "tbLastPage";
		tbLastPage.Text = "Last Page";
		tbLastPage.Click += ButtonLastPage_Click;
		sbpReportCheckerViewer.AutoExpandOnClick = true;
		sbpReportCheckerViewer.BeginGroup = true;
		sbpReportCheckerViewer.ButtonStyle = eButtonStyle.ImageAndText;
		sbpReportCheckerViewer.Name = "sbpReportCheckerViewer";
		sbpReportCheckerViewer.Text = "Report Checker";
		sbpReportCheckerViewer.Click += ButtonReportChecker_Click;
		btRefresh.BeginGroup = true;
		btRefresh.Name = "btRefresh";
		btRefresh.Text = "Refresh";
		btRefresh.Click += ButtonRefresh_Click;
		btStop.BeginGroup = true;
		btStop.Name = "btStop";
		btStop.Text = "Stop";
		btStop.Visible = false;
		btStop.Click += ButtonStop_Click;
		progressState.BackStyle.CornerType = eCornerType.Square;
		progressState.BeginGroup = true;
		progressState.ChunkGradientAngle = 0f;
		progressState.MenuVisibility = eMenuVisibility.VisibleAlways;
		progressState.Name = "progressState";
		progressState.RecentlyUsed = false;
		progressState.TextVisible = true;
		progressState.Visible = false;
		sbpPageInfo.BeginGroup = true;
		sbpPageInfo.Name = "sbpPageInfo";
		liSeparator.ItemAlignment = eItemAlignment.Far;
		liSeparator.Name = "liSeparator";
		liSeparator.Width = 20;
		itemContainer1.BackgroundStyle.Class = "Office2007StatusBarBackground2";
		itemContainer1.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer1.Name = "itemContainer1";
		itemContainer1.SubItems.AddRange(new BaseItem[4] { itemContainer2, labelItem2, siZoomViewer, labelItem3 });
		itemContainer1.TitleStyle.CornerType = eCornerType.Square;
		itemContainer2.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer2.BeginGroup = true;
		itemContainer2.Name = "itemContainer2";
		itemContainer2.SubItems.AddRange(new BaseItem[3] { tbViewModeSinglePage, tbViewModeContinuous, tbViewModeMultiplePages });
		itemContainer2.TitleStyle.CornerType = eCornerType.Square;
		tbViewModeSinglePage.Image = (Image)(object)Resources.ViewModeSinglePage;
		tbViewModeSinglePage.ImageIndex = 26;
		tbViewModeSinglePage.ImagePaddingHorizontal = 6;
		tbViewModeSinglePage.ImagePaddingVertical = 4;
		tbViewModeSinglePage.Name = "tbViewModeSinglePage";
		tbViewModeSinglePage.Shortcuts.Add(eShortcut.ShiftF2);
		tbViewModeSinglePage.Text = "Single Page";
		tbViewModeSinglePage.Click += ButtonViewModeSinglePage_Click;
		tbViewModeContinuous.Image = (Image)(object)Resources.ViewModeContinuous;
		tbViewModeContinuous.ImageIndex = 27;
		tbViewModeContinuous.ImagePaddingHorizontal = 6;
		tbViewModeContinuous.ImagePaddingVertical = 4;
		tbViewModeContinuous.Name = "tbViewModeContinuous";
		tbViewModeContinuous.Shortcuts.Add(eShortcut.ShiftF3);
		tbViewModeContinuous.Text = "Continuous";
		tbViewModeContinuous.Click += ButtonViewModeContinuous_Click;
		tbViewModeMultiplePages.Image = (Image)(object)Resources.ViewModeMultiplePages;
		tbViewModeMultiplePages.ImageIndex = 28;
		tbViewModeMultiplePages.ImagePaddingHorizontal = 6;
		tbViewModeMultiplePages.ImagePaddingVertical = 4;
		tbViewModeMultiplePages.Name = "tbViewModeMultiplePages";
		tbViewModeMultiplePages.Shortcuts.Add(eShortcut.ShiftF4);
		tbViewModeMultiplePages.Text = "Multiple Pages";
		tbViewModeMultiplePages.Click += ButtonViewModeMultiplePages_Click;
		labelItem2.Name = "labelItem2";
		labelItem2.Text = "100%";
		labelItem2.TextAlignment = (StringAlignment)1;
		labelItem2.Width = 60;
		siZoomViewer.ClickRepeatInterval = 50;
		siZoomViewer.LabelVisible = false;
		siZoomViewer.LabelWidth = 36;
		siZoomViewer.Maximum = 400;
		siZoomViewer.Minimum = 10;
		siZoomViewer.Name = "siZoomViewer";
		siZoomViewer.Step = 10;
		siZoomViewer.Text = "100%";
		siZoomViewer.Value = 100;
		siZoomViewer.Width = 130;
		siZoomViewer.ValueChanging += ZoomViewer_ValueChanging;
		labelItem3.Name = "labelItem3";
		labelItem3.Width = 10;
		timerZoomSliderViewer.Tick += TimerZoomSliderViewer_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1200, 597);
		((Control)this).Controls.Add((Control)(object)bar1);
		((Control)this).Controls.Add((Control)(object)tbStatusBar);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).MinimumSize = new Size(300, 198);
		((Control)this).Name = "StiRibbonDesigner";
		((Control)this).Text = "StiDesigner";
		((Control)this).Click += This_Click;
		((ISupportInitialize)bar1).EndInit();
		((ISupportInitialize)tbStatusBar).EndInit();
		((Control)this).ResumeLayout(false);
	}

	bool IStiDesigner.get_ShowInTaskbar()
	{
		return ((Form)this).ShowInTaskbar;
	}

	void IStiDesigner.set_ShowInTaskbar(bool value)
	{
		((Form)this).ShowInTaskbar = value;
	}

	Form IStiDesigner.get_MdiParent()
	{
		return ((Form)this).MdiParent;
	}

	void IStiDesigner.set_MdiParent(Form value)
	{
		((Form)this).MdiParent = value;
	}

	bool IStiDesigner.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IStiDesigner.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void IStiDesignerBase.AddOwnedForm(Form form)
	{
		((Form)this).AddOwnedForm(form);
	}
}
