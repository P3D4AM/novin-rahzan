using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Report.Win.Ribbon;

namespace Stimulsoft.Report.Viewer;

public class StiRibbonDotMatrixViewerForm : Office2007RibbonForm, IStiViewerForm
{
	private bool Lock;

	private bool LockDoClose;

	private RibbonControl ribbonControl;

	private RibbonPanel dotMatrixViewerPanel;

	private Panel panelDotMatrixViewerControl;

	private StiRibbonDotMatrixViewerControl ribbonDotMatrixViewerControl;

	private RibbonTabItem tiDotMatrixViewer;

	private ThumbButtonManager thumbButtonManager;

	public static FormWindowState PreviewWindowState
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return StiOptions.Viewer.DotMatrix.ViewerWindowState;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			StiOptions.Viewer.DotMatrix.ViewerWindowState = value;
		}
	}

	public StiReport Report
	{
		get
		{
			return ribbonDotMatrixViewerControl.Report;
		}
		set
		{
			ribbonDotMatrixViewerControl.Report = value;
			if (value != null)
			{
				if (value.ReportAlias.Length == 0)
				{
					((Control)this).Text = StiLocalization.Get("FormViewer", "title");
				}
				else
				{
					((Control)this).Text = value.ReportAlias + "-" + StiLocalization.Get("FormViewer", "title");
				}
			}
		}
	}

	[Browsable(false)]
	public object ViewerOwner
	{
		get
		{
			return ((Form)this).MdiParent;
		}
		set
		{
			((Form)this).MdiParent = (Form)((value is Form) ? value : null);
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Expected O, but got Unknown
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonDotMatrixViewerForm));
		ribbonControl = new RibbonControl();
		dotMatrixViewerPanel = new RibbonPanel();
		tiDotMatrixViewer = new RibbonTabItem();
		panelDotMatrixViewerControl = new Panel();
		ribbonDotMatrixViewerControl = new StiRibbonDotMatrixViewerControl(showRibbonMenu: false);
		ribbonControl.SuspendLayout();
		((Control)this).SuspendLayout();
		ribbonControl.BackgroundStyle.CornerType = (eCornerType)1;
		ribbonControl.CanCustomize = false;
		ribbonControl.CaptionVisible = true;
		((Control)ribbonControl).Controls.Add((Control)(object)dotMatrixViewerPanel);
		((Control)ribbonControl).Dock = (DockStyle)1;
		ribbonControl.Items.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)tiDotMatrixViewer });
		ribbonControl.KeyTipsFont = new Font("Arial", 7f);
		((Control)ribbonControl).Location = new Point(0, 0);
		((Control)ribbonControl).Name = "ribbonControl";
		((Control)ribbonControl).Padding = new Padding(0, 0, 0, 0);
		ribbonControl.RibbonStripIndent = 0;
		((Control)ribbonControl).Size = new Size(938, 153);
		ribbonControl.Style = (eDotNetBarStyle)8;
		ribbonControl.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
		ribbonControl.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
		ribbonControl.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
		ribbonControl.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
		ribbonControl.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
		ribbonControl.SystemText.QatDialogAddButton = "&Add >>";
		ribbonControl.SystemText.QatDialogCancelButton = "Cancel";
		ribbonControl.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
		ribbonControl.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
		ribbonControl.SystemText.QatDialogOkButton = "OK";
		ribbonControl.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
		ribbonControl.SystemText.QatDialogRemoveButton = "&Remove";
		ribbonControl.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
		ribbonControl.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
		ribbonControl.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
		ribbonControl.TabGroupHeight = 14;
		((Control)ribbonControl).TabIndex = 0;
		((PanelControl)dotMatrixViewerPanel).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)dotMatrixViewerPanel).Dock = (DockStyle)5;
		dotMatrixViewerPanel.Location = new Point(0, 56);
		((Control)dotMatrixViewerPanel).Name = "dotMatrixViewerPanel";
		((Control)dotMatrixViewerPanel).Padding = new Padding(3, 0, 3, 3);
		dotMatrixViewerPanel.Size = new Size(938, 96);
		((PanelControl)dotMatrixViewerPanel).Style.CornerType = (eCornerType)1;
		((PanelControl)dotMatrixViewerPanel).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)dotMatrixViewerPanel).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)dotMatrixViewerPanel).TabIndex = 1;
		((ButtonItem)tiDotMatrixViewer).Checked = true;
		((BaseItem)tiDotMatrixViewer).Name = "tiDotMatrixViewer";
		tiDotMatrixViewer.Panel = dotMatrixViewerPanel;
		((BaseItem)tiDotMatrixViewer).Text = "Dot Matrix Viewer";
		((Control)panelDotMatrixViewerControl).Dock = (DockStyle)5;
		((Control)panelDotMatrixViewerControl).Location = new Point(5, 153);
		((Control)panelDotMatrixViewerControl).Name = "panelDotMatrixViewerControl";
		((Control)panelDotMatrixViewerControl).Size = new Size(938, 556);
		((Control)panelDotMatrixViewerControl).TabIndex = 1;
		((Control)ribbonDotMatrixViewerControl).Dock = (DockStyle)5;
		((Control)ribbonDotMatrixViewerControl).Location = new Point(5, 153);
		((Control)ribbonDotMatrixViewerControl).Name = "ribbonDotMatrixViewerControl";
		ribbonDotMatrixViewerControl.Report = null;
		ribbonDotMatrixViewerControl.ShowToolbar = false;
		((Control)ribbonDotMatrixViewerControl).Size = new Size(938, 556);
		((Control)ribbonDotMatrixViewerControl).TabIndex = 0;
		ribbonDotMatrixViewerControl.Close += ribbonDotMatrixViewerControl_Close;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(948, 711);
		((Control)this).Controls.Add((Control)(object)ribbonDotMatrixViewerControl);
		((Control)this).Controls.Add((Control)(object)panelDotMatrixViewerControl);
		((Control)this).Controls.Add((Control)(object)ribbonControl);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "StiRibbonDotMatrixViewerForm";
		((Form)this).FormClosed += new FormClosedEventHandler(StiRibbonDotMatrixViewerForm_FormClosed);
		ribbonControl.ResumeLayout(false);
		((Control)ribbonControl).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	private void ribbonDotMatrixViewerControl_Close(object sender, EventArgs e)
	{
		if (!Lock)
		{
			LockDoClose = true;
			((Form)this).Close();
		}
	}

	private void StiRibbonDotMatrixViewerForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		Lock = true;
		if (!LockDoClose)
		{
			ribbonDotMatrixViewerControl.InvokeClose();
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (StiOptions.Windows.Windows7TaskBarSupport && Windows7Taskbar.IsWindowsVistaOrGreater)
		{
			if (((Message)(ref m)).Msg == Windows7Taskbar.TaskbarButtonCreatedMessage)
			{
				IntPtr handle = ((Control)this).Handle;
				Windows7Taskbar.SetThumbnailTooltip(handle, "Stimulsoft Reports Viewer");
				thumbButtonManager = new ThumbButtonManager(handle);
				ThumbButton val = thumbButtonManager.CreateThumbButton(101u, StiImageUtils.IconFromBytes(StiImageUtils.GetByteArray("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportOpen_Ico.ico")), "Open Report");
				val.Clicked += delegate
				{
					ribbonDotMatrixViewerControl.InvokeClickOpenButton();
				};
				ThumbButton val2 = thumbButtonManager.CreateThumbButton(102u, StiImageUtils.IconFromBytes(StiImageUtils.GetByteArray("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportSave_Ico.ico")), "Save Report");
				val2.Clicked += delegate
				{
					if (ribbonDotMatrixViewerControl.Report != null)
					{
						ribbonDotMatrixViewerControl.InvokeClickSaveButton();
					}
				};
				ThumbButton val3 = thumbButtonManager.CreateThumbButton(103u, StiImageUtils.IconFromBytes(StiImageUtils.GetByteArray("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportPrint_Ico.ico")), "Print");
				val3.Clicked += delegate
				{
					if (ribbonDotMatrixViewerControl.Report != null)
					{
						ribbonDotMatrixViewerControl.InvokeClickPrintButton();
					}
				};
				thumbButtonManager.AddThumbButtons((ThumbButton[])(object)new ThumbButton[3] { val, val2, val3 });
			}
			if (thumbButtonManager != null)
			{
				thumbButtonManager.DispatchMessage(ref m);
			}
		}
		((RibbonForm)this).WndProc(ref m);
	}

	public object GetOwner(object form)
	{
		Form val = (Form)((form is Form) ? form : null);
		if (val != null)
		{
			return val.MdiParent;
		}
		return null;
	}

	public void ShowViewer()
	{
		((Control)this).Show();
	}

	public void ShowDialogViewer()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).ShowDialog();
	}

	public void ShowViewer(object win32Window)
	{
		((Form)this).Show((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
	}

	public void ShowDialogViewer(object win32Window)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).ShowDialog((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
	}

	private void LoadRibbonMenuDotMatrixViewer()
	{
		StiRibbonDotMatrixViewUserControl stiRibbonDotMatrixViewUserControl = new StiRibbonDotMatrixViewUserControl(ribbonDotMatrixViewerControl);
		int count = ((ArrangedElementCollection)((Control)stiRibbonDotMatrixViewUserControl).Controls).Count;
		for (int i = 0; i < count; i++)
		{
			Control obj = ((Control)stiRibbonDotMatrixViewUserControl).Controls[0];
			RibbonBar val = (RibbonBar)(object)((obj is RibbonBar) ? obj : null);
			if (val != null)
			{
				((Control)dotMatrixViewerPanel).Controls.Add((Control)(object)val);
			}
		}
	}

	public StiRibbonDotMatrixViewerForm()
		: this(null)
	{
	}

	public StiRibbonDotMatrixViewerForm(StiReport report)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		LoadRibbonMenuDotMatrixViewer();
		((ButtonItem)tiDotMatrixViewer).CustomColorName = "customDefault";
		((BaseItem)ribbonControl.RibbonStrip.CaptionContainerItem).SubItems.RemoveAt(0);
		((BaseItem)ribbonControl.RibbonStrip.CaptionContainerItem).SubItems.RemoveAt(0);
		ElementStyle backgroundStyle = ((ItemControl)ribbonControl.RibbonStrip).BackgroundStyle;
		MetroColorGeneratorParameters darkBlue = MetroColorGeneratorParameters.DarkBlue;
		backgroundStyle.BackColor = ((MetroColorGeneratorParameters)(ref darkBlue)).BaseColor;
		((RibbonForm)this).WindowState = PreviewWindowState;
		((Form)this).ShowInTaskbar = StiOptions.Viewer.DotMatrix.ShowInTaskbar;
		Report = report;
		if (StiOptions.Viewer.DotMatrix.ViewerIcon != null)
		{
			((Form)this).Icon = StiOptions.Viewer.DotMatrix.ViewerIcon;
		}
	}

	bool IStiViewerForm.get_ShowInTaskbar()
	{
		return ((Form)this).ShowInTaskbar;
	}

	void IStiViewerForm.set_ShowInTaskbar(bool value)
	{
		((Form)this).ShowInTaskbar = value;
	}

	bool IStiViewerForm.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IStiViewerForm.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}
}
