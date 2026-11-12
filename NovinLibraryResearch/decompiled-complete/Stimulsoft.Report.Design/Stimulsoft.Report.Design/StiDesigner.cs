using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Properties;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Design;

public class StiDesigner : Form, IStiDesigner, IStiDesignerBase
{
	private ThumbButtonManager thumbButtonManager;

	private IContainer components;

	private StiDesignerControl designerControl;

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

	public static string DefaultTemplatesPath
	{
		get
		{
			return StiOptions.Designer.DefaultTemplatesPath;
		}
		set
		{
			StiOptions.Designer.DefaultTemplatesPath = value;
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

	public static string DefaultRibbonDesignerQatConfigPath
	{
		get
		{
			return StiOptions.Designer.DefaultRibbonDesignerQatConfigPath;
		}
		set
		{
			StiOptions.Designer.DefaultRibbonDesignerQatConfigPath = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiDesignerBase DesignerControl => designerControl;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDesignerInfo Info => designerControl.Info;

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
			return designerControl.Report;
		}
		set
		{
			designerControl.Report = value;
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

	public static string GetDefaultRibbonQatConfigPath()
	{
		return StiDesignerControl.GetDefaultRibbonQatConfigPath();
	}

	protected override void OnClosed(EventArgs e)
	{
		((Form)this).OnClosed(e);
		StiFormHelper.SaveStateForm((Form)(object)this, "DesignerWindow", allowMaximize: true);
		if (designerControl != null)
		{
			((Component)(object)designerControl).Dispose();
		}
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
				ThumbButton thumbButton = thumbButtonManager.CreateThumbButton(101u, Resources.IcoReportOpen, StiLocalization.Get("MainMenu", "menuFileReportOpen").Replace("&", "").Replace("...", ""));
				thumbButton.Clicked += delegate
				{
					StiActions.MainMenu.InvokePageOpen(designerControl);
				};
				ThumbButton thumbButton2 = thumbButtonManager.CreateThumbButton(102u, Resources.IcoReportSave, StiLocalization.Get("MainMenu", "menuFileReportSave").Replace("&", ""));
				thumbButton2.Clicked += delegate
				{
					if (designerControl.Report != null)
					{
						StiActions.MainMenu.InvokeReportSave(designerControl);
					}
				};
				ThumbButton thumbButton3 = thumbButtonManager.CreateThumbButton(103u, Resources.IcoCreate, StiLocalization.Get("Wizards", "title"));
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
		((Form)this).WndProc(ref m);
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

	private void StiDesigner_FormClosing(object sender, FormClosingEventArgs e)
	{
		StiDesignerPreviewControl previewControl = designerControl.Builder.GetPreviewControl();
		if (previewControl != null && previewControl.ViewerControl.BlockFormClose)
		{
			previewControl.ViewerControl.BlockFormClose = false;
			((CancelEventArgs)(object)e).Cancel = true;
		}
	}

	static StiDesigner()
	{
		ComboBoxEx.FontFamilies = StiFontCollection.GetFontFamilies();
		ComboBoxEx.FontCollection = StiFontCollection.Instance;
	}

	public StiDesigner()
		: this(null)
	{
	}

	public StiDesigner(StiReport report)
		: this(report, null)
	{
	}

	public StiDesigner(StiReport report, string cloudReport)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Invalid comparison between Unknown and I4
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		StiDesignerControl stiDesignerControl = new StiDesignerControl(report, cloudReport, isRibbonGuiMode: false);
		((Control)stiDesignerControl).Dock = (DockStyle)5;
		((Control)stiDesignerControl).Location = new Point(0, 0);
		((Control)stiDesignerControl).Name = "designerControl";
		((Control)stiDesignerControl).Size = new Size(603, 449);
		((Control)stiDesignerControl).TabIndex = 0;
		designerControl = stiDesignerControl;
		((Control)this).Controls.Add((Control)(object)designerControl);
		((Form)this).WindowState = DesignerWindowState;
		Point position = Point.Empty;
		Size size = new Size(664, 446);
		FormWindowState wndState = (FormWindowState)0;
		StiFormHelper.LoadStateForm((Form)(object)this, "DesignerWindow", ref position, ref size, ref wndState, allowMaximize: true);
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).ShowInTaskbar = StiOptions.Designer.ShowDesignerInTaskbar;
		if (StiOptions.Designer.DesignerWindowSize.HasValue)
		{
			size = StiOptions.Designer.DesignerWindowSize.GetValueOrDefault();
		}
		if ((int)wndState == 0)
		{
			((Form)this).WindowState = (FormWindowState)0;
		}
		((Form)this).SetBoundsCore(position.X, position.Y, size.Width, size.Height, (BoundsSpecified)15);
		if ((int)wndState == 2)
		{
			((Form)this).WindowState = (FormWindowState)2;
		}
		if (StiOptions.Designer.DesignerIcon != null)
		{
			((Form)this).Icon = StiOptions.Designer.DesignerIcon;
		}
		((Control)this).Text = designerControl.GetDesignerTitle();
		((Form)this).FormClosing += new FormClosingEventHandler(StiDesigner_FormClosing);
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
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDesigner));
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1184, 561);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).MinimumSize = new Size(1200, 600);
		((Control)this).Name = "StiDesigner";
		((Control)this).Text = "StiDesigner";
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
