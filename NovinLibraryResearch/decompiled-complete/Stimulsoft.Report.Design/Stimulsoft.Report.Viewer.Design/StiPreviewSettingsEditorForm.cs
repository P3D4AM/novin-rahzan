using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Viewer.Design;

public class StiPreviewSettingsEditorForm : Form
{
	public enum StartTab
	{
		Report,
		Dashboard
	}

	private StartTab startTab;

	private StiReport report;

	private StiWinViewerWrapper viewerControl;

	private StiPreviewSettings settings;

	private IContainer components;

	private CheckBox cbToolbar;

	private CheckBox cbVerScrollBar;

	private CheckBox cbHorScrollBar;

	private CheckBox cbContextMenu;

	private CheckBox cbPrint;

	private CheckBox cbOpen;

	private CheckBox cbSave;

	private CheckBox cbPageNew;

	private CheckBox cbPageDelete;

	private CheckBox cbPageDesign;

	private CheckBox cbZoom;

	private CheckBox cbPageControl;

	private StiGroupLine glButtons;

	private Button btOk;

	private Button btCancel;

	private CheckBox cbPageViewMode;

	private CheckBox cbPageSize;

	private CheckBox cbSendEMail;

	private CheckBox cbBookmarks;

	private CheckBox cbThumbs;

	private CheckBox cbEditor;

	private CheckBox cbFind;

	private CheckBox cbStatusBar;

	private CheckBox cbParameters;

	private CheckBox cbClose;

	private CheckBox cbResources;

	private Panel panelViewer;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageReport;

	private StiCloudTabulatorPage pageDashboard;

	private CheckBox checkBoxShowRefreshButton;

	private CheckBox checkBoxShowSaveButton;

	private CheckBox checkBoxShowOpenButton;

	private CheckBox checkBoxShowToolBar;

	public StiToolBar toolBarSample;

	private StiToolButton buttonSampleRefresh;

	private StiToolButton buttonSampleOpen;

	private StiToolButton buttonSampleSave;

	private ToolTip toolTip1;

	private CheckBox checkBoxShowFullScreen;

	private StiToolButton buttonSampleFullScreen;

	private CheckBox checkBoxShowEditButton;

	private StiToolButton buttonSampleEdit;

	private CheckBox checkBoxShowExports;

	private CheckBox checkBoxShowReportSnaphots;

	public StiToolBar toolBarPreviewMode;

	private StiComboBox comboBoxPreviewMode;

	private Label labelPreviewMode;

	private string LocPreviewModeStandard => Loc.GetEnum("StiPreviewModeStandard");

	private string LocPreviewModeDotMatrix => Loc.GetEnum("StiPreviewModeDotMatrix");

	private string LocPreviewModeStandardAndDotMatrix => Loc.GetEnum("StiPreviewModeStandardAndDotMatrix");

	private StiPreviewMode ReportPreviewMode
	{
		get
		{
			string text = ((ComboBox)comboBoxPreviewMode).SelectedItem as string;
			if (text == LocPreviewModeStandard)
			{
				return StiPreviewMode.Standard;
			}
			if (text == LocPreviewModeDotMatrix)
			{
				return StiPreviewMode.DotMatrix;
			}
			return StiPreviewMode.StandardAndDotMatrix;
		}
		set
		{
			switch (report.PreviewMode)
			{
			case StiPreviewMode.Standard:
				((ComboBox)comboBoxPreviewMode).SelectedItem = LocPreviewModeStandard;
				break;
			case StiPreviewMode.DotMatrix:
				((ComboBox)comboBoxPreviewMode).SelectedItem = LocPreviewModeDotMatrix;
				break;
			case StiPreviewMode.StandardAndDotMatrix:
				((ComboBox)comboBoxPreviewMode).SelectedItem = LocPreviewModeStandardAndDotMatrix;
				break;
			}
		}
	}

	public StiPreviewSettingsEditorForm(StiReport report, StartTab startTab)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.report = report;
			this.startTab = startTab;
			settings = (StiPreviewSettings)report.PreviewSettings;
			cbClose.Checked = false;
			cbPrint.Checked = (settings & StiPreviewSettings.Print) > StiPreviewSettings.None;
			cbOpen.Checked = (settings & StiPreviewSettings.Open) > StiPreviewSettings.None;
			cbSave.Checked = (settings & StiPreviewSettings.Save) > StiPreviewSettings.None;
			cbSendEMail.Checked = (settings & StiPreviewSettings.SendEMail) > StiPreviewSettings.None;
			cbClose.Checked = (settings & StiPreviewSettings.Close) > StiPreviewSettings.None;
			cbPageControl.Checked = (settings & StiPreviewSettings.PageControl) > StiPreviewSettings.None;
			cbEditor.Checked = (settings & StiPreviewSettings.EditorTool) > StiPreviewSettings.None;
			cbFind.Checked = (settings & StiPreviewSettings.FindTool) > StiPreviewSettings.None;
			cbPageNew.Checked = (settings & StiPreviewSettings.PageNew) > StiPreviewSettings.None;
			cbPageDelete.Checked = (settings & StiPreviewSettings.PageDelete) > StiPreviewSettings.None;
			cbPageDesign.Checked = (settings & StiPreviewSettings.PageDesign) > StiPreviewSettings.None;
			cbPageSize.Checked = (settings & StiPreviewSettings.PageSize) > StiPreviewSettings.None;
			cbPageViewMode.Checked = (settings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None;
			cbVerScrollBar.Checked = (settings & StiPreviewSettings.VertScrollBar) > StiPreviewSettings.None;
			cbHorScrollBar.Checked = (settings & StiPreviewSettings.HorScrollBar) > StiPreviewSettings.None;
			cbZoom.Checked = (settings & StiPreviewSettings.Zoom) > StiPreviewSettings.None;
			cbBookmarks.Checked = (settings & StiPreviewSettings.Bookmarks) > StiPreviewSettings.None;
			cbParameters.Checked = (settings & StiPreviewSettings.Parameters) > StiPreviewSettings.None;
			cbResources.Checked = (settings & StiPreviewSettings.Resources) > StiPreviewSettings.None;
			cbThumbs.Checked = (settings & StiPreviewSettings.Thumbs) > StiPreviewSettings.None;
			cbContextMenu.Checked = (settings & StiPreviewSettings.ContextMenu) > StiPreviewSettings.None;
			cbStatusBar.Checked = (settings & StiPreviewSettings.StatusBar) > StiPreviewSettings.None;
			cbToolbar.Checked = (settings & StiPreviewSettings.Toolbar) > StiPreviewSettings.None;
			checkBoxShowToolBar.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowToolBar) > StiDashboardViewerSettings.None;
			checkBoxShowRefreshButton.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowRefreshButton) > StiDashboardViewerSettings.None;
			checkBoxShowOpenButton.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowOpenButton) > StiDashboardViewerSettings.None;
			checkBoxShowFullScreen.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowFullScreenButton) > StiDashboardViewerSettings.None;
			checkBoxShowEditButton.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowEditButton) > StiDashboardViewerSettings.None;
			checkBoxShowSaveButton.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowMenuButton) > StiDashboardViewerSettings.None;
			checkBoxShowReportSnaphots.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowReportSnapshots) > StiDashboardViewerSettings.None;
			checkBoxShowExports.Checked = (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowExports) > StiDashboardViewerSettings.None;
			((Control)checkBoxShowReportSnaphots).Visible = StiAccountSettings.CurrentAccount != null;
			((Control)checkBoxShowExports).Visible = StiAccountSettings.CurrentAccount != null;
			Localize();
			DoScaling();
			((Form)this).ShowIcon = false;
			if (!((Component)this).DesignMode)
			{
				InitializeViewer();
			}
			ReportPreviewMode = report.PreviewMode;
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageDashboard.Image = StiReportDesignImages.Tasks.Dashboard(StiImageSize.Double);
		pageReport.Image = StiReportDesignImages.Tasks.Report(StiImageSize.Double);
		((ButtonBase)buttonSampleEdit).Image = StiReportDesignImages.Dashboards.Edit();
		((ButtonBase)buttonSampleFullScreen).Image = StiReportDesignImages.Dashboards.FullScreen();
		((ButtonBase)buttonSampleSave).Image = StiReportDesignImages.Dashboards.Save();
		((ButtonBase)buttonSampleOpen).Image = StiReportDesignImages.Dashboards.Open();
		((ButtonBase)buttonSampleRefresh).Image = StiReportDesignImages.Dashboards.Refresh();
	}

	private void InitializeViewer()
	{
		StiReport stiReport = report.Clone() as StiReport;
		StiWinViewerWrapper stiWinViewerWrapper = new StiWinViewerWrapper(report, removeShortcuts: false, allowRibbon: false);
		((Control)stiWinViewerWrapper).Dock = (DockStyle)5;
		((Control)stiWinViewerWrapper).Name = "viewerControl";
		stiWinViewerWrapper.Report = stiReport;
		stiWinViewerWrapper.ShowZoom = true;
		stiWinViewerWrapper.PreviewSettings = (StiPreviewSettings)stiReport.PreviewSettings;
		viewerControl = stiWinViewerWrapper;
		((Control)panelViewer).Controls.Add((Control)(object)viewerControl);
	}

	private void Localize()
	{
		try
		{
			((Control)this).Text = Loc.GetCleaned("PropertyMain", "PreviewSettings");
			((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
			((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
			((Control)cbPageNew).Text = Loc.Get("FormViewer", "PageNew");
			((Control)cbPageDelete).Text = Loc.GetCleaned("FormViewer", "PageDelete");
			((Control)cbPageDesign).Text = Loc.GetCleaned("FormViewer", "PageDesign");
			((Control)cbPageSize).Text = Loc.GetCleaned("FormViewer", "PageSize");
			((Control)cbStatusBar).Text = Loc.GetCleaned("FormViewer", "StatusBar");
			((Control)cbZoom).Text = Loc.GetCleaned("FormViewer", "Zoom");
			((Control)cbPrint).Text = Loc.GetCleaned("FormViewer", "Print");
			((Control)cbOpen).Text = Loc.GetCleaned("FormViewer", "Open");
			((Control)cbSave).Text = Loc.GetCleaned("FormViewer", "Save");
			((Control)cbBookmarks).Text = Loc.GetCleaned("FormViewer", "Bookmarks");
			((Control)cbParameters).Text = Loc.GetCleaned("FormViewer", "Parameters");
			((Control)cbResources).Text = Loc.GetCleaned("PropertyMain", "Resources");
			((Control)cbThumbs).Text = Loc.GetCleaned("FormViewer", "Thumbnails");
			((Control)cbClose).Text = Loc.GetCleaned("FormViewer", "Close");
			((Control)cbSendEMail).Text = Loc.GetCleaned("FormViewer", "SendEMail");
			((Control)cbFind).Text = Loc.GetCleaned("FormViewer", "Find");
			((Control)cbEditor).Text = Loc.GetCleaned("FormViewer", "Editor");
			((Control)cbPageControl).Text = Loc.GetCleaned("FormViewer", "PageControl");
			((Control)cbPageViewMode).Text = Loc.GetCleaned("FormViewer", "ViewMode");
			((Control)labelPreviewMode).Text = Loc.GetMain("PreviewMode");
			((Control)cbHorScrollBar).Text = Loc.GetCleaned("FormViewer", "HorScrollBar");
			((Control)cbVerScrollBar).Text = Loc.GetCleaned("FormViewer", "VerScrollBar");
			((Control)cbToolbar).Text = Loc.GetCleaned("FormViewer", "Toolbar");
			((Control)cbContextMenu).Text = Loc.GetCleaned("FormViewer", "ContextMenu");
			((Control)pageReport).Text = Loc.GetCleaned("Components", "StiReport");
			((Control)pageDashboard).Text = Loc.GetCleaned("Components", "StiDashboard");
			((Control)checkBoxShowToolBar).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("FormViewer", "Toolbar"));
			((Control)checkBoxShowRefreshButton).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("PropertyMain", "Refresh"));
			((Control)checkBoxShowOpenButton).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("Buttons", "Open"));
			((Control)checkBoxShowEditButton).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("QueryBuilder", "Edit"));
			((Control)checkBoxShowFullScreen).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("FormViewer", "FullScreen"));
			((Control)checkBoxShowSaveButton).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("Buttons", "Save"));
			((Control)checkBoxShowReportSnaphots).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("Permissions", "ItemReportSnapshots"));
			((Control)checkBoxShowExports).Text = string.Format(Loc.Get("Buttons", "ShowSpecific"), Loc.Get("Services", "categoryExport"));
			((ComboBox)comboBoxPreviewMode).BeginUpdate();
			((ComboBox)comboBoxPreviewMode).Items.Clear();
			((ComboBox)comboBoxPreviewMode).Items.Add((object)LocPreviewModeStandard);
			((ComboBox)comboBoxPreviewMode).Items.Add((object)LocPreviewModeStandardAndDotMatrix);
			((ComboBox)comboBoxPreviewMode).Items.Add((object)LocPreviewModeDotMatrix);
			((ComboBox)comboBoxPreviewMode).EndUpdate();
			toolTip1.SetToolTip((Control)(object)toolBarSample, Loc.Get("FormViewer", "Toolbar"));
			toolTip1.SetToolTip((Control)(object)buttonSampleRefresh, Loc.Get("PropertyMain", "Refresh"));
			toolTip1.SetToolTip((Control)(object)buttonSampleOpen, Loc.Get("Buttons", "Open"));
			toolTip1.SetToolTip((Control)(object)buttonSampleEdit, Loc.Get("QueryBuilder", "Edit"));
			toolTip1.SetToolTip((Control)(object)buttonSampleFullScreen, Loc.Get("FormViewer", "FullScreen"));
			toolTip1.SetToolTip((Control)(object)buttonSampleSave, Loc.Get("Buttons", "Save"));
		}
		catch
		{
		}
	}

	private void CheckBoxToolbar_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowToolbar = cbToolbar.Checked;
		}
	}

	private void CheckBoxVerScrollBar_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowVertScrollBar = cbVerScrollBar.Checked;
		}
	}

	private void CheckBoxHorScrollBar_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowHorScrollBar = cbHorScrollBar.Checked;
		}
	}

	private void CheckBoxContextMenu_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowContextMenu = cbContextMenu.Checked;
		}
	}

	private void CheckBoxPrint_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowPrint = cbPrint.Checked;
		}
	}

	private void CheckBoxOpen_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowOpen = cbOpen.Checked;
		}
	}

	private void CheckBoxSave_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowSave = cbSave.Checked;
		}
	}

	private void CheckBoxPageNew_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowPageNew = cbPageNew.Checked;
		}
	}

	private void CheckBoxPageDelete_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowPageDelete = cbPageDelete.Checked;
		}
	}

	private void CheckBoxPageDesign_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowPageDesign = cbPageDesign.Checked;
		}
	}

	private void CheckBoxEditor_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowEditorTool = cbEditor.Checked;
		}
	}

	private void CheckBoxZoom_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowZoom = cbZoom.Checked;
		}
	}

	private void CheckBoxPageControl_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowPageControl = cbPageControl.Checked;
		}
	}

	private void CheckBoxSendByEMail_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowSendEMail = cbSendEMail.Checked;
		}
	}

	private void CheckBoxThumbsButton_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowThumbsPanel = cbThumbs.Checked;
		}
	}

	private void CheckBoxParameters_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowParametersButton = cbParameters.Checked;
		}
	}

	private void CheckBoxResources_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowResourcesButton = cbResources.Checked;
		}
	}

	private void CheckBoxBookmarksButton_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowBookmarksPanel = cbBookmarks.Checked;
		}
	}

	private void CheckBoxPageViewMode_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowPageViewMode = cbPageViewMode.Checked;
		}
	}

	private void CheckBoxPageSize_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowPageSize = cbPageSize.Checked;
		}
	}

	private void CheckBoxToolFind_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowFind = cbFind.Checked;
		}
	}

	private void CheckBoxStatusBar_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowStatusBar = cbStatusBar.Checked;
		}
	}

	private void CheckBoxClose_CheckedChanged(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ShowCloseButton = cbClose.Checked;
		}
	}

	private void CheckBoxShowToolBar_CheckedChanged(object sender, EventArgs e)
	{
		((Control)toolBarSample).Enabled = checkBoxShowToolBar.Checked;
	}

	private void CheckBoxShowRefreshButton_CheckedChanged(object sender, EventArgs e)
	{
		((Control)buttonSampleRefresh).Enabled = checkBoxShowRefreshButton.Checked;
	}

	private void CheckBoxShowOpenButon_CheckedChanged(object sender, EventArgs e)
	{
		((Control)buttonSampleOpen).Enabled = checkBoxShowOpenButton.Checked;
	}

	private void CheckBoxShowFullScreen_CheckedChanged(object sender, EventArgs e)
	{
		((Control)buttonSampleFullScreen).Enabled = checkBoxShowFullScreen.Checked;
	}

	private void CheckBoxShowSaveButton_CheckedChanged(object sender, EventArgs e)
	{
		((Control)buttonSampleSave).Enabled = checkBoxShowSaveButton.Checked;
	}

	private void CheckBoxShowEditButton_CheckedChanged(object sender, EventArgs e)
	{
		((Control)buttonSampleEdit).Enabled = checkBoxShowEditButton.Checked;
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			StiDashboardViewerSettings stiDashboardViewerSettings = StiDashboardViewerSettings.None;
			if (checkBoxShowToolBar.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowToolBar;
			}
			if (checkBoxShowRefreshButton.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowRefreshButton;
			}
			if (checkBoxShowOpenButton.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowOpenButton;
			}
			if (checkBoxShowFullScreen.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowFullScreenButton;
			}
			if (checkBoxShowEditButton.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowEditButton;
			}
			if (checkBoxShowSaveButton.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowMenuButton;
			}
			if (checkBoxShowReportSnaphots.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowReportSnapshots;
			}
			if (checkBoxShowExports.Checked)
			{
				stiDashboardViewerSettings |= StiDashboardViewerSettings.ShowExports;
			}
			report.PreviewSettings = (int)viewerControl.PreviewSettings;
			report.DashboardViewerSettings = stiDashboardViewerSettings;
			report.PreviewMode = ReportPreviewMode;
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		viewerControl?.InvokeZoomOnePage();
		if (startTab == StartTab.Dashboard)
		{
			Timer timer = new Timer
			{
				Interval = 100
			};
			timer.Start();
			timer.Tick += delegate
			{
				timer.Stop();
				TabControl.SelectedIndex = 1;
				((Component)(object)timer).Dispose();
			};
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
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
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
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
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fa: Expected O, but got Unknown
		//IL_1732: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a38: Expected O, but got Unknown
		//IL_1abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Expected O, but got Unknown
		//IL_1b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5a: Expected O, but got Unknown
		//IL_1be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1beb: Expected O, but got Unknown
		//IL_1c72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7c: Expected O, but got Unknown
		//IL_20ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f8: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiPreviewSettingsEditorForm));
		toolTip1 = new ToolTip(components);
		btOk = new Button();
		btCancel = new Button();
		TabControl = new StiCloudTabulator();
		pageReport = new StiCloudTabulatorPage();
		toolBarPreviewMode = new StiToolBar();
		labelPreviewMode = new Label();
		comboBoxPreviewMode = new StiComboBox();
		panelViewer = new Panel();
		cbResources = new CheckBox();
		cbPageControl = new CheckBox();
		cbClose = new CheckBox();
		cbPrint = new CheckBox();
		cbPageNew = new CheckBox();
		cbParameters = new CheckBox();
		cbHorScrollBar = new CheckBox();
		cbThumbs = new CheckBox();
		cbPageSize = new CheckBox();
		cbZoom = new CheckBox();
		cbStatusBar = new CheckBox();
		cbFind = new CheckBox();
		cbPageViewMode = new CheckBox();
		cbPageDelete = new CheckBox();
		cbToolbar = new CheckBox();
		cbVerScrollBar = new CheckBox();
		cbSave = new CheckBox();
		cbOpen = new CheckBox();
		cbEditor = new CheckBox();
		cbContextMenu = new CheckBox();
		cbPageDesign = new CheckBox();
		cbSendEMail = new CheckBox();
		cbBookmarks = new CheckBox();
		pageDashboard = new StiCloudTabulatorPage();
		checkBoxShowExports = new CheckBox();
		checkBoxShowReportSnaphots = new CheckBox();
		checkBoxShowEditButton = new CheckBox();
		toolBarSample = new StiToolBar();
		buttonSampleRefresh = new StiToolButton();
		buttonSampleOpen = new StiToolButton();
		buttonSampleEdit = new StiToolButton();
		buttonSampleFullScreen = new StiToolButton();
		buttonSampleSave = new StiToolButton();
		checkBoxShowToolBar = new CheckBox();
		checkBoxShowRefreshButton = new CheckBox();
		checkBoxShowFullScreen = new CheckBox();
		checkBoxShowSaveButton = new CheckBox();
		checkBoxShowOpenButton = new CheckBox();
		glButtons = new StiGroupLine();
		((Control)TabControl).SuspendLayout();
		((Control)pageReport).SuspendLayout();
		((Control)toolBarPreviewMode).SuspendLayout();
		((Control)pageDashboard).SuspendLayout();
		((Control)toolBarSample).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(674, 13);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(74, 26);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(754, 13);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(74, 26);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)TabControl).Controls.Add((Control)(object)pageReport);
		((Control)TabControl).Controls.Add((Control)(object)pageDashboard);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(840, 629);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		((Control)pageReport).Controls.Add((Control)(object)toolBarPreviewMode);
		((Control)pageReport).Controls.Add((Control)(object)panelViewer);
		((Control)pageReport).Controls.Add((Control)(object)cbResources);
		((Control)pageReport).Controls.Add((Control)(object)cbPageControl);
		((Control)pageReport).Controls.Add((Control)(object)cbClose);
		((Control)pageReport).Controls.Add((Control)(object)cbPrint);
		((Control)pageReport).Controls.Add((Control)(object)cbPageNew);
		((Control)pageReport).Controls.Add((Control)(object)cbParameters);
		((Control)pageReport).Controls.Add((Control)(object)cbHorScrollBar);
		((Control)pageReport).Controls.Add((Control)(object)cbThumbs);
		((Control)pageReport).Controls.Add((Control)(object)cbPageSize);
		((Control)pageReport).Controls.Add((Control)(object)cbZoom);
		((Control)pageReport).Controls.Add((Control)(object)cbStatusBar);
		((Control)pageReport).Controls.Add((Control)(object)cbFind);
		((Control)pageReport).Controls.Add((Control)(object)cbPageViewMode);
		((Control)pageReport).Controls.Add((Control)(object)cbPageDelete);
		((Control)pageReport).Controls.Add((Control)(object)cbToolbar);
		((Control)pageReport).Controls.Add((Control)(object)cbVerScrollBar);
		((Control)pageReport).Controls.Add((Control)(object)cbSave);
		((Control)pageReport).Controls.Add((Control)(object)cbOpen);
		((Control)pageReport).Controls.Add((Control)(object)cbEditor);
		((Control)pageReport).Controls.Add((Control)(object)cbContextMenu);
		((Control)pageReport).Controls.Add((Control)(object)cbPageDesign);
		((Control)pageReport).Controls.Add((Control)(object)cbSendEMail);
		((Control)pageReport).Controls.Add((Control)(object)cbBookmarks);
		pageReport.Dock = (DockStyle)5;
		pageReport.Image = (Image)componentResourceManager.GetObject("pageReport.Image");
		pageReport.InvisibleVisible = true;
		((Control)pageReport).Location = new Point(0, 0);
		((Control)pageReport).Margin = new Padding(1);
		((Control)pageReport).Name = "pageReport";
		((Control)pageReport).Padding = new Padding(0, 0, 1, 0);
		((Control)pageReport).Size = new Size(721, 629);
		((Control)pageReport).TabIndex = 0;
		((Control)pageReport).Text = "Report";
		((Control)pageReport).Visible = false;
		((Control)toolBarPreviewMode).BackColor = Color.LightGray;
		((Control)toolBarPreviewMode).Controls.Add((Control)(object)labelPreviewMode);
		((Control)toolBarPreviewMode).Controls.Add((Control)(object)comboBoxPreviewMode);
		toolBarPreviewMode.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBarPreviewMode).Dock = (DockStyle)2;
		toolBarPreviewMode.LineStyle = StiToolBar.StiToolBarLineStyle.Top;
		((Control)toolBarPreviewMode).Location = new Point(0, 585);
		((Control)toolBarPreviewMode).Name = "toolBarPreviewMode";
		((Control)toolBarPreviewMode).Padding = new Padding(4);
		((Control)toolBarPreviewMode).Size = new Size(720, 44);
		((Control)toolBarPreviewMode).TabIndex = 24;
		((Control)toolBarPreviewMode).Text = "stiToolBar1";
		((Control)labelPreviewMode).BackColor = Color.White;
		((Control)labelPreviewMode).ForeColor = Color.DimGray;
		((Control)labelPreviewMode).Location = new Point(12, 12);
		((Control)labelPreviewMode).Name = "labelPreviewMode";
		((Control)labelPreviewMode).Size = new Size(160, 21);
		((Control)labelPreviewMode).TabIndex = 4;
		((Control)labelPreviewMode).Text = "PreviewMode";
		labelPreviewMode.TextAlign = (ContentAlignment)16;
		((Control)comboBoxPreviewMode).BackColor = SystemColors.Window;
		comboBoxPreviewMode.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxPreviewMode).ForeColor = SystemColors.ControlText;
		((Control)comboBoxPreviewMode).Location = new Point(178, 12);
		((ComboBox)comboBoxPreviewMode).MaxDropDownItems = 20;
		((Control)comboBoxPreviewMode).Name = "comboBoxPreviewMode";
		((Control)comboBoxPreviewMode).Size = new Size(160, 21);
		((Control)comboBoxPreviewMode).TabIndex = 3;
		((Control)panelViewer).Anchor = (AnchorStyles)13;
		((Control)panelViewer).Location = new Point(12, 12);
		((Control)panelViewer).Name = "panelViewer";
		((Control)panelViewer).Size = new Size(697, 307);
		((Control)panelViewer).TabIndex = 0;
		cbResources.Checked = true;
		cbResources.CheckState = (CheckState)1;
		((Control)cbResources).Location = new Point(344, 429);
		((Control)cbResources).Name = "cbResources";
		((Control)cbResources).Size = new Size(160, 20);
		((Control)cbResources).TabIndex = 16;
		((Control)cbResources).Text = "Resources";
		cbResources.CheckedChanged += CheckBoxResources_CheckedChanged;
		cbPageControl.Checked = true;
		cbPageControl.CheckState = (CheckState)1;
		((Control)cbPageControl).Location = new Point(178, 333);
		((Control)cbPageControl).Name = "cbPageControl";
		((Control)cbPageControl).Size = new Size(160, 20);
		((Control)cbPageControl).TabIndex = 5;
		((Control)cbPageControl).Text = "Page Control";
		cbPageControl.CheckedChanged += CheckBoxPageControl_CheckedChanged;
		cbClose.Checked = true;
		cbClose.CheckState = (CheckState)1;
		((Control)cbClose).Location = new Point(510, 429);
		((Control)cbClose).Name = "cbClose";
		((Control)cbClose).Size = new Size(160, 20);
		((Control)cbClose).TabIndex = 23;
		((Control)cbClose).Text = "Close";
		cbClose.CheckedChanged += CheckBoxClose_CheckedChanged;
		cbPrint.Checked = true;
		cbPrint.CheckState = (CheckState)1;
		((Control)cbPrint).Location = new Point(12, 333);
		((Control)cbPrint).Name = "cbPrint";
		((Control)cbPrint).Size = new Size(160, 20);
		((Control)cbPrint).TabIndex = 1;
		((Control)cbPrint).Text = "Print";
		cbPrint.CheckedChanged += CheckBoxPrint_CheckedChanged;
		cbPageNew.Checked = true;
		cbPageNew.CheckState = (CheckState)1;
		((Control)cbPageNew).Location = new Point(178, 429);
		((Control)cbPageNew).Name = "cbPageNew";
		((Control)cbPageNew).Size = new Size(160, 20);
		((Control)cbPageNew).TabIndex = 8;
		((Control)cbPageNew).Text = "Page New";
		cbPageNew.CheckedChanged += CheckBoxPageNew_CheckedChanged;
		cbParameters.Checked = true;
		cbParameters.CheckState = (CheckState)1;
		((Control)cbParameters).Location = new Point(344, 397);
		((Control)cbParameters).Name = "cbParameters";
		((Control)cbParameters).Size = new Size(160, 20);
		((Control)cbParameters).TabIndex = 15;
		((Control)cbParameters).Text = "Parameters";
		cbParameters.CheckedChanged += CheckBoxParameters_CheckedChanged;
		cbHorScrollBar.Checked = true;
		cbHorScrollBar.CheckState = (CheckState)1;
		((Control)cbHorScrollBar).Location = new Point(510, 333);
		((Control)cbHorScrollBar).Name = "cbHorScrollBar";
		((Control)cbHorScrollBar).Size = new Size(160, 20);
		((Control)cbHorScrollBar).TabIndex = 20;
		((Control)cbHorScrollBar).Text = "Hor ScrollBar";
		cbHorScrollBar.CheckedChanged += CheckBoxHorScrollBar_CheckedChanged;
		cbThumbs.Checked = true;
		cbThumbs.CheckState = (CheckState)1;
		((Control)cbThumbs).Location = new Point(344, 461);
		((Control)cbThumbs).Name = "cbThumbs";
		((Control)cbThumbs).Size = new Size(160, 20);
		((Control)cbThumbs).TabIndex = 17;
		((Control)cbThumbs).Text = "Thumbs";
		cbThumbs.CheckedChanged += CheckBoxThumbsButton_CheckedChanged;
		cbPageSize.Checked = true;
		cbPageSize.CheckState = (CheckState)1;
		((Control)cbPageSize).Location = new Point(178, 525);
		((Control)cbPageSize).Name = "cbPageSize";
		((Control)cbPageSize).Size = new Size(160, 20);
		((Control)cbPageSize).TabIndex = 11;
		((Control)cbPageSize).Text = "Page Size";
		cbPageSize.CheckedChanged += CheckBoxPageSize_CheckedChanged;
		cbZoom.Checked = true;
		cbZoom.CheckState = (CheckState)1;
		((Control)cbZoom).Location = new Point(344, 493);
		((Control)cbZoom).Name = "cbZoom";
		((Control)cbZoom).Size = new Size(160, 20);
		((Control)cbZoom).TabIndex = 18;
		((Control)cbZoom).Text = "Zoom";
		cbZoom.CheckedChanged += CheckBoxZoom_CheckedChanged;
		cbStatusBar.Checked = true;
		cbStatusBar.CheckState = (CheckState)1;
		((Control)cbStatusBar).Location = new Point(344, 333);
		((Control)cbStatusBar).Name = "cbStatusBar";
		((Control)cbStatusBar).Size = new Size(160, 20);
		((Control)cbStatusBar).TabIndex = 13;
		((Control)cbStatusBar).Text = "Status Bar";
		cbStatusBar.CheckedChanged += CheckBoxStatusBar_CheckedChanged;
		cbFind.Checked = true;
		cbFind.CheckState = (CheckState)1;
		((Control)cbFind).Location = new Point(178, 365);
		((Control)cbFind).Name = "cbFind";
		((Control)cbFind).Size = new Size(160, 20);
		((Control)cbFind).TabIndex = 6;
		((Control)cbFind).Text = "Find";
		cbFind.CheckedChanged += CheckBoxToolFind_CheckedChanged;
		cbPageViewMode.Checked = true;
		cbPageViewMode.CheckState = (CheckState)1;
		((Control)cbPageViewMode).Location = new Point(178, 557);
		((Control)cbPageViewMode).Name = "cbPageViewMode";
		((Control)cbPageViewMode).Size = new Size(160, 20);
		((Control)cbPageViewMode).TabIndex = 12;
		((Control)cbPageViewMode).Text = "Page View Mode";
		cbPageViewMode.CheckedChanged += CheckBoxPageViewMode_CheckedChanged;
		cbPageDelete.Checked = true;
		cbPageDelete.CheckState = (CheckState)1;
		((Control)cbPageDelete).Location = new Point(178, 461);
		((Control)cbPageDelete).Name = "cbPageDelete";
		((Control)cbPageDelete).Size = new Size(160, 20);
		((Control)cbPageDelete).TabIndex = 9;
		((Control)cbPageDelete).Text = "Page Delete";
		cbPageDelete.CheckedChanged += CheckBoxPageDelete_CheckedChanged;
		cbToolbar.Checked = true;
		cbToolbar.CheckState = (CheckState)1;
		((Control)cbToolbar).Location = new Point(344, 525);
		((Control)cbToolbar).Name = "cbToolbar";
		((Control)cbToolbar).Size = new Size(160, 20);
		((Control)cbToolbar).TabIndex = 19;
		((Control)cbToolbar).Text = "Toolbar";
		cbToolbar.CheckedChanged += CheckBoxToolbar_CheckedChanged;
		cbVerScrollBar.Checked = true;
		cbVerScrollBar.CheckState = (CheckState)1;
		((Control)cbVerScrollBar).Location = new Point(510, 365);
		((Control)cbVerScrollBar).Name = "cbVerScrollBar";
		((Control)cbVerScrollBar).Size = new Size(160, 20);
		((Control)cbVerScrollBar).TabIndex = 21;
		((Control)cbVerScrollBar).Text = "Ver ScrollBar";
		cbVerScrollBar.CheckedChanged += CheckBoxVerScrollBar_CheckedChanged;
		cbSave.Checked = true;
		cbSave.CheckState = (CheckState)1;
		((Control)cbSave).Location = new Point(12, 397);
		((Control)cbSave).Name = "cbSave";
		((Control)cbSave).Size = new Size(160, 20);
		((Control)cbSave).TabIndex = 3;
		((Control)cbSave).Text = "Save";
		cbSave.CheckedChanged += CheckBoxSave_CheckedChanged;
		cbOpen.Checked = true;
		cbOpen.CheckState = (CheckState)1;
		((Control)cbOpen).Location = new Point(12, 365);
		((Control)cbOpen).Name = "cbOpen";
		((Control)cbOpen).Size = new Size(160, 20);
		((Control)cbOpen).TabIndex = 2;
		((Control)cbOpen).Text = "Open";
		cbOpen.CheckedChanged += CheckBoxOpen_CheckedChanged;
		cbEditor.Checked = true;
		cbEditor.CheckState = (CheckState)1;
		((Control)cbEditor).Location = new Point(178, 397);
		((Control)cbEditor).Name = "cbEditor";
		((Control)cbEditor).Size = new Size(160, 20);
		((Control)cbEditor).TabIndex = 7;
		((Control)cbEditor).Text = "Editor";
		cbEditor.CheckedChanged += CheckBoxEditor_CheckedChanged;
		cbContextMenu.Checked = true;
		cbContextMenu.CheckState = (CheckState)1;
		((Control)cbContextMenu).Location = new Point(510, 397);
		((Control)cbContextMenu).Name = "cbContextMenu";
		((Control)cbContextMenu).Size = new Size(160, 20);
		((Control)cbContextMenu).TabIndex = 22;
		((Control)cbContextMenu).Text = "Context Menu";
		cbContextMenu.CheckedChanged += CheckBoxContextMenu_CheckedChanged;
		cbPageDesign.Checked = true;
		cbPageDesign.CheckState = (CheckState)1;
		((Control)cbPageDesign).Location = new Point(178, 493);
		((Control)cbPageDesign).Name = "cbPageDesign";
		((Control)cbPageDesign).Size = new Size(160, 20);
		((Control)cbPageDesign).TabIndex = 10;
		((Control)cbPageDesign).Text = "Page Design";
		cbPageDesign.CheckedChanged += CheckBoxPageDesign_CheckedChanged;
		cbSendEMail.Checked = true;
		cbSendEMail.CheckState = (CheckState)1;
		((Control)cbSendEMail).Location = new Point(12, 429);
		((Control)cbSendEMail).Name = "cbSendEMail";
		((Control)cbSendEMail).Size = new Size(160, 20);
		((Control)cbSendEMail).TabIndex = 4;
		((Control)cbSendEMail).Text = "Send E-Mail";
		cbSendEMail.CheckedChanged += CheckBoxSendByEMail_CheckedChanged;
		cbBookmarks.Checked = true;
		cbBookmarks.CheckState = (CheckState)1;
		((Control)cbBookmarks).Location = new Point(344, 365);
		((Control)cbBookmarks).Name = "cbBookmarks";
		((Control)cbBookmarks).Size = new Size(160, 20);
		((Control)cbBookmarks).TabIndex = 14;
		((Control)cbBookmarks).Text = "Bookmarks";
		cbBookmarks.CheckedChanged += CheckBoxBookmarksButton_CheckedChanged;
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowExports);
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowReportSnaphots);
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowEditButton);
		((Control)pageDashboard).Controls.Add((Control)(object)toolBarSample);
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowToolBar);
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowRefreshButton);
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowFullScreen);
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowSaveButton);
		((Control)pageDashboard).Controls.Add((Control)(object)checkBoxShowOpenButton);
		pageDashboard.Dock = (DockStyle)5;
		pageDashboard.Image = (Image)componentResourceManager.GetObject("pageDashboard.Image");
		pageDashboard.InvisibleVisible = false;
		((Control)pageDashboard).Location = new Point(0, 0);
		((Control)pageDashboard).Name = "pageDashboard";
		((Control)pageDashboard).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDashboard).Size = new Size(721, 591);
		((Control)pageDashboard).TabIndex = 1;
		((Control)pageDashboard).Text = "Dashboard";
		((Control)pageDashboard).Visible = false;
		checkBoxShowExports.Checked = true;
		checkBoxShowExports.CheckState = (CheckState)1;
		((Control)checkBoxShowExports).Location = new Point(38, 299);
		((Control)checkBoxShowExports).Name = "checkBoxShowExports";
		((Control)checkBoxShowExports).Size = new Size(436, 20);
		((Control)checkBoxShowExports).TabIndex = 55;
		((Control)checkBoxShowExports).Text = "Show Exports";
		checkBoxShowReportSnaphots.Checked = true;
		checkBoxShowReportSnaphots.CheckState = (CheckState)1;
		((Control)checkBoxShowReportSnaphots).Location = new Point(38, 264);
		((Control)checkBoxShowReportSnaphots).Name = "checkBoxShowReportSnaphots";
		((Control)checkBoxShowReportSnaphots).Size = new Size(436, 20);
		((Control)checkBoxShowReportSnaphots).TabIndex = 54;
		((Control)checkBoxShowReportSnaphots).Text = "Show Report Snaphots";
		checkBoxShowEditButton.Checked = true;
		checkBoxShowEditButton.CheckState = (CheckState)1;
		((Control)checkBoxShowEditButton).Location = new Point(38, 165);
		((Control)checkBoxShowEditButton).Name = "checkBoxShowEditButton";
		((Control)checkBoxShowEditButton).Size = new Size(436, 20);
		((Control)checkBoxShowEditButton).TabIndex = 3;
		((Control)checkBoxShowEditButton).Text = "Show Edit Button";
		checkBoxShowEditButton.CheckedChanged += CheckBoxShowEditButton_CheckedChanged;
		((Control)toolBarSample).Anchor = (AnchorStyles)13;
		((Control)toolBarSample).BackColor = Color.LightGray;
		((Control)toolBarSample).Controls.Add((Control)(object)buttonSampleRefresh);
		((Control)toolBarSample).Controls.Add((Control)(object)buttonSampleOpen);
		((Control)toolBarSample).Controls.Add((Control)(object)buttonSampleEdit);
		((Control)toolBarSample).Controls.Add((Control)(object)buttonSampleFullScreen);
		((Control)toolBarSample).Controls.Add((Control)(object)buttonSampleSave);
		toolBarSample.ControlStyle = StiControlStyle.Office2013Blue;
		toolBarSample.LineStyle = StiToolBar.StiToolBarLineStyle.All;
		((Control)toolBarSample).Location = new Point(12, 12);
		((Control)toolBarSample).Name = "toolBarSample";
		((Control)toolBarSample).Padding = new Padding(4);
		((Control)toolBarSample).Size = new Size(695, 36);
		((Control)toolBarSample).TabIndex = 53;
		((Control)toolBarSample).Text = "stiToolBar1";
		((Control)buttonSampleRefresh).Dock = (DockStyle)4;
		buttonSampleRefresh.Draw3DButton = false;
		buttonSampleRefresh.HotDragDrop = false;
		((ButtonBase)buttonSampleRefresh).Image = (Image)componentResourceManager.GetObject("buttonSampleRefresh.Image");
		((Control)buttonSampleRefresh).Location = new Point(551, 4);
		((Control)buttonSampleRefresh).Name = "buttonSampleRefresh";
		((Control)buttonSampleRefresh).Size = new Size(28, 28);
		((Control)buttonSampleRefresh).TabIndex = 4;
		((ButtonBase)buttonSampleRefresh).UseVisualStyleBackColor = false;
		((Control)buttonSampleOpen).Dock = (DockStyle)4;
		buttonSampleOpen.Draw3DButton = false;
		buttonSampleOpen.HotDragDrop = false;
		((ButtonBase)buttonSampleOpen).Image = (Image)componentResourceManager.GetObject("buttonSampleOpen.Image");
		((Control)buttonSampleOpen).Location = new Point(579, 4);
		((Control)buttonSampleOpen).Name = "buttonSampleOpen";
		((Control)buttonSampleOpen).Size = new Size(28, 28);
		((Control)buttonSampleOpen).TabIndex = 3;
		((ButtonBase)buttonSampleOpen).UseVisualStyleBackColor = false;
		((Control)buttonSampleEdit).Dock = (DockStyle)4;
		buttonSampleEdit.Draw3DButton = false;
		buttonSampleEdit.HotDragDrop = false;
		((ButtonBase)buttonSampleEdit).Image = (Image)componentResourceManager.GetObject("buttonSampleEdit.Image");
		((Control)buttonSampleEdit).Location = new Point(607, 4);
		((Control)buttonSampleEdit).Name = "buttonSampleEdit";
		((Control)buttonSampleEdit).Size = new Size(28, 28);
		((Control)buttonSampleEdit).TabIndex = 6;
		((ButtonBase)buttonSampleEdit).UseVisualStyleBackColor = false;
		((Control)buttonSampleFullScreen).Dock = (DockStyle)4;
		buttonSampleFullScreen.Draw3DButton = false;
		buttonSampleFullScreen.HotDragDrop = false;
		((ButtonBase)buttonSampleFullScreen).Image = (Image)componentResourceManager.GetObject("buttonSampleFullScreen.Image");
		((Control)buttonSampleFullScreen).Location = new Point(635, 4);
		((Control)buttonSampleFullScreen).Name = "buttonSampleFullScreen";
		((Control)buttonSampleFullScreen).Size = new Size(28, 28);
		((Control)buttonSampleFullScreen).TabIndex = 5;
		((ButtonBase)buttonSampleFullScreen).UseVisualStyleBackColor = false;
		((Control)buttonSampleSave).Dock = (DockStyle)4;
		buttonSampleSave.Draw3DButton = false;
		buttonSampleSave.HotDragDrop = false;
		((ButtonBase)buttonSampleSave).Image = (Image)componentResourceManager.GetObject("buttonSampleSave.Image");
		((Control)buttonSampleSave).Location = new Point(663, 4);
		((Control)buttonSampleSave).Name = "buttonSampleSave";
		((Control)buttonSampleSave).Size = new Size(28, 28);
		((Control)buttonSampleSave).TabIndex = 2;
		((ButtonBase)buttonSampleSave).UseVisualStyleBackColor = false;
		checkBoxShowToolBar.Checked = true;
		checkBoxShowToolBar.CheckState = (CheckState)1;
		((Control)checkBoxShowToolBar).Location = new Point(38, 68);
		((Control)checkBoxShowToolBar).Name = "checkBoxShowToolBar";
		((Control)checkBoxShowToolBar).Size = new Size(436, 20);
		((Control)checkBoxShowToolBar).TabIndex = 0;
		((Control)checkBoxShowToolBar).Text = "Show Toolbar";
		checkBoxShowToolBar.CheckedChanged += CheckBoxShowToolBar_CheckedChanged;
		checkBoxShowRefreshButton.Checked = true;
		checkBoxShowRefreshButton.CheckState = (CheckState)1;
		((Control)checkBoxShowRefreshButton).Location = new Point(38, 99);
		((Control)checkBoxShowRefreshButton).Name = "checkBoxShowRefreshButton";
		((Control)checkBoxShowRefreshButton).Size = new Size(436, 20);
		((Control)checkBoxShowRefreshButton).TabIndex = 1;
		((Control)checkBoxShowRefreshButton).Text = "Show Refresh Button";
		checkBoxShowRefreshButton.CheckedChanged += CheckBoxShowRefreshButton_CheckedChanged;
		checkBoxShowFullScreen.Checked = true;
		checkBoxShowFullScreen.CheckState = (CheckState)1;
		((Control)checkBoxShowFullScreen).Location = new Point(38, 198);
		((Control)checkBoxShowFullScreen).Name = "checkBoxShowFullScreen";
		((Control)checkBoxShowFullScreen).Size = new Size(436, 20);
		((Control)checkBoxShowFullScreen).TabIndex = 4;
		((Control)checkBoxShowFullScreen).Text = "Show FullScreen Button";
		checkBoxShowFullScreen.CheckedChanged += CheckBoxShowFullScreen_CheckedChanged;
		checkBoxShowSaveButton.Checked = true;
		checkBoxShowSaveButton.CheckState = (CheckState)1;
		((Control)checkBoxShowSaveButton).Location = new Point(38, 231);
		((Control)checkBoxShowSaveButton).Name = "checkBoxShowSaveButton";
		((Control)checkBoxShowSaveButton).Size = new Size(436, 20);
		((Control)checkBoxShowSaveButton).TabIndex = 5;
		((Control)checkBoxShowSaveButton).Text = "Show Save Button";
		checkBoxShowSaveButton.CheckedChanged += CheckBoxShowSaveButton_CheckedChanged;
		checkBoxShowOpenButton.Checked = true;
		checkBoxShowOpenButton.CheckState = (CheckState)1;
		((Control)checkBoxShowOpenButton).Location = new Point(38, 132);
		((Control)checkBoxShowOpenButton).Name = "checkBoxShowOpenButton";
		((Control)checkBoxShowOpenButton).Size = new Size(436, 20);
		((Control)checkBoxShowOpenButton).TabIndex = 2;
		((Control)checkBoxShowOpenButton).Text = "Show Open Button";
		checkBoxShowOpenButton.CheckedChanged += CheckBoxShowOpenButon_CheckedChanged;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 629);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(840, 49);
		((Control)glButtons).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(840, 678);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPreviewSettingsEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Preview Settings";
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageReport).ResumeLayout(false);
		((Control)toolBarPreviewMode).ResumeLayout(false);
		((Control)pageDashboard).ResumeLayout(false);
		((Control)toolBarSample).ResumeLayout(false);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
