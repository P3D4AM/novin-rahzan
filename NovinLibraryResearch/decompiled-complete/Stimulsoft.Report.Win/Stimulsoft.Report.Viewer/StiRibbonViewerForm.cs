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
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Report.Win.Ribbon;

namespace Stimulsoft.Report.Viewer;

public class StiRibbonViewerForm : Office2007RibbonForm, IStiViewerForm
{
	private bool Lock;

	private bool LockDoClose;

	private RibbonControl ribbonControl1;

	private RibbonPanel ribbonPanelPreview;

	private RibbonTabItem ribbonTIPreview;

	private StiPanel viewerPanel;

	private Container components;

	private StiRibbonViewerControl viewerControl;

	private ThumbButtonManager thumbButtonManager;

	public StiRibbonViewerControl ViewerControl
	{
		get
		{
			return viewerControl;
		}
		set
		{
			viewerControl = value;
		}
	}

	public static FormWindowState PreviewWindowState
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return StiOptions.Viewer.Windows.ViewerWindowState;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			StiOptions.Viewer.Windows.ViewerWindowState = value;
		}
	}

	[Browsable(false)]
	public virtual StiReport Report
	{
		get
		{
			return viewerControl.Report;
		}
		set
		{
			viewerControl.Report = value;
			if (value != null)
			{
				if (value.ReportAlias.Length == 0)
				{
					((Control)this).Text = StiOptions.Viewer.GetViewerTitleWithMask(null);
				}
				else
				{
					((Control)this).Text = StiOptions.Viewer.GetViewerTitleWithMask(value.ReportAlias);
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
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonViewerForm));
		ribbonControl1 = new RibbonControl();
		ribbonPanelPreview = new RibbonPanel();
		ribbonTIPreview = new RibbonTabItem();
		viewerPanel = new StiPanel();
		ribbonControl1.SuspendLayout();
		((Control)this).SuspendLayout();
		ribbonControl1.BackgroundStyle.CornerType = (eCornerType)1;
		ribbonControl1.CaptionVisible = true;
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanelPreview);
		((Control)ribbonControl1).Dock = (DockStyle)1;
		ribbonControl1.Items.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)ribbonTIPreview });
		ribbonControl1.KeyTipsFont = new Font("Arial", 7f);
		((Control)ribbonControl1).Location = new Point(0, 0);
		((Control)ribbonControl1).Name = "ribbonControl1";
		((Control)ribbonControl1).Padding = new Padding(0, 0, 0, 0);
		ribbonControl1.RibbonStripIndent = 0;
		((Control)ribbonControl1).Size = new Size(747, 153);
		ribbonControl1.Office2007ColorTable = (eOffice2007ColorScheme)0;
		ribbonControl1.Style = (eDotNetBarStyle)8;
		ribbonControl1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
		ribbonControl1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
		ribbonControl1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
		ribbonControl1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
		ribbonControl1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
		ribbonControl1.SystemText.QatDialogAddButton = "&Add >>";
		ribbonControl1.SystemText.QatDialogCancelButton = "Cancel";
		ribbonControl1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
		ribbonControl1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
		ribbonControl1.SystemText.QatDialogOkButton = "OK";
		ribbonControl1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
		ribbonControl1.SystemText.QatDialogRemoveButton = "&Remove";
		ribbonControl1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
		ribbonControl1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
		ribbonControl1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
		ribbonControl1.TabGroupHeight = 14;
		ribbonControl1.TabGroupsVisible = true;
		((Control)ribbonControl1).TabIndex = 0;
		((Control)ribbonControl1).Text = "ribbonControl";
		((PanelControl)ribbonPanelPreview).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)ribbonPanelPreview).Dock = (DockStyle)5;
		ribbonPanelPreview.Location = new Point(0, 57);
		((Control)ribbonPanelPreview).Name = "ribbonPanelPreview";
		((Control)ribbonPanelPreview).Padding = new Padding(3, 0, 3, 3);
		ribbonPanelPreview.Size = new Size(747, 93);
		((PanelControl)ribbonPanelPreview).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanelPreview).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanelPreview).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanelPreview).TabIndex = 1;
		((ButtonItem)ribbonTIPreview).Checked = true;
		((BaseItem)ribbonTIPreview).Name = "ribbonTIPreview";
		ribbonTIPreview.Panel = ribbonPanelPreview;
		((ButtonItem)ribbonTIPreview).Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(4);
		((BaseItem)ribbonTIPreview).Text = StiLocalization.Get("FormDesigner", "Preview");
		((Control)viewerPanel).Dock = (DockStyle)5;
		((Control)viewerPanel).Location = new Point(0, 153);
		((Control)viewerPanel).Name = "viewerPanel";
		((Control)viewerPanel).Size = new Size(747, 215);
		((Control)viewerPanel).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(757, 369);
		((Control)this).Controls.Add((Control)(object)viewerPanel);
		((Control)this).Controls.Add((Control)(object)ribbonControl1);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "StiRibbonViewerForm";
		((Form)this).ShowInTaskbar = false;
		((RibbonForm)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(StiRibbonViewerForm_FormClosed);
		ribbonControl1.ResumeLayout(false);
		((Control)ribbonControl1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((RibbonForm)this).Dispose(disposing);
	}

	private void ShowRibbonBar()
	{
		if (viewerControl != null && !viewerControl.GetToolbarVisibility)
		{
			((Control)ribbonControl1).Height = 30;
		}
		viewerControl.ShowToolbar = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		((RibbonForm)this).OnPaint(e);
		Point point = ((Control)this).PointToClient(((Control)ribbonControl1).PointToScreen(new Point(0, ((Control)ribbonControl1).Bottom - 1)));
		Bar statusBar = ((RibbonForm)this).GetStatusBar();
		if (statusBar != null)
		{
			Point point2 = ((Control)this).PointToClient(((Control)statusBar).Parent.PointToScreen(new Point(0, ((Control)statusBar).Top)));
			Rectangle rectangle = new Rectangle(1, point.Y, 6, point2.Y - point.Y);
			e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(230, 230, 230)), rectangle);
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
					viewerControl.InvokeClickOpenButton();
				};
				ThumbButton val2 = thumbButtonManager.CreateThumbButton(102u, StiImageUtils.IconFromBytes(StiImageUtils.GetByteArray("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportSave_Ico.ico")), "Save Report");
				val2.Clicked += delegate
				{
					if (viewerControl.Report != null)
					{
						viewerControl.InvokeClickSaveButton();
					}
				};
				ThumbButton val3 = thumbButtonManager.CreateThumbButton(103u, StiImageUtils.IconFromBytes(StiImageUtils.GetByteArray("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportPrint_Ico.ico")), "Print");
				val3.Clicked += delegate
				{
					if (viewerControl.Report != null)
					{
						viewerControl.InvokeClickPrintButton();
					}
				};
				thumbButtonManager.AddThumbButtons((ThumbButton[])(object)new ThumbButton[3] { val3, val, val2 });
			}
			if (thumbButtonManager != null)
			{
				thumbButtonManager.DispatchMessage(ref m);
			}
		}
		((RibbonForm)this).WndProc(ref m);
	}

	private void InitializeRibbonViewer(StiReport report)
	{
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		viewerControl = new StiRibbonViewerControl(report, removeShortcuts: false, showRibbonMenu: false);
		((Control)viewerControl).Name = "RibbonViewerControl";
		((Control)viewerControl).Dock = (DockStyle)5;
		viewerControl.Close += viewerControl_Close;
		((Control)viewerPanel).Controls.Add((Control)(object)viewerControl);
		((Control)ribbonPanelPreview).SuspendLayout();
		StiRibbonViewerUserControl stiRibbonViewerUserControl = new StiRibbonViewerUserControl(viewerControl);
		int count = ((ArrangedElementCollection)((Control)stiRibbonViewerUserControl).Controls).Count;
		for (int i = 0; i < count; i++)
		{
			Control obj = ((Control)stiRibbonViewerUserControl).Controls[0];
			RibbonBar val = (RibbonBar)(object)((obj is RibbonBar) ? obj : null);
			if (!(((Control)val).Name == "ribbonBarPlatforms") && val != null)
			{
				((Control)ribbonPanelPreview).Controls.Add((Control)(object)val);
			}
		}
		viewerControl?.Parameters?.CheckRequestFromUser();
		viewerControl.UpdateControlsState();
		((ButtonItem)ribbonTIPreview).CustomColorName = "customDefault";
		((BaseItem)ribbonControl1.RibbonStrip.CaptionContainerItem).SubItems.RemoveAt(0);
		((BaseItem)ribbonControl1.RibbonStrip.CaptionContainerItem).SubItems.RemoveAt(0);
		ElementStyle backgroundStyle = ((ItemControl)ribbonControl1.RibbonStrip).BackgroundStyle;
		MetroColorGeneratorParameters darkBlue = MetroColorGeneratorParameters.DarkBlue;
		backgroundStyle.BackColor = ((MetroColorGeneratorParameters)(ref darkBlue)).BaseColor;
		((Control)ribbonPanelPreview).ResumeLayout(false);
	}

	private void viewerControl_Close(object sender, EventArgs e)
	{
		if (!Lock)
		{
			LockDoClose = true;
			((Form)this).Close();
		}
	}

	private void StiRibbonViewerForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		Lock = true;
		if (!LockDoClose)
		{
			viewerControl.InvokeClose();
		}
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
		ShowRibbonBar();
	}

	public void ShowDialogViewer()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).ShowDialog();
	}

	public void ShowViewer(object win32Window)
	{
		((Form)this).Show((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
		ShowRibbonBar();
	}

	public void ShowDialogViewer(object win32Window)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).ShowDialog((IWin32Window)((win32Window is IWin32Window) ? win32Window : null));
	}

	private void biStartButtonOpen_Click(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.InvokeClickOpenButton();
		}
	}

	private void biStartButtonSave_Click(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.InvokeClickSaveButton();
		}
	}

	private void biStartButtonPrint_Click(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.InvokeClickPrintButton();
		}
	}

	private void biStartButtonClose_Click(object sender, EventArgs e)
	{
		if (viewerControl != null)
		{
			viewerControl.ButtonClose_Click(null, new EventArgs());
		}
	}

	private void sbClose_Click(object sender, EventArgs e)
	{
		Lock = true;
		if (!LockDoClose)
		{
			viewerControl.InvokeClose();
		}
		((Form)this).Close();
	}

	public StiRibbonViewerForm()
		: this(null)
	{
	}

	public StiRibbonViewerForm(StiReport report)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		InitializeRibbonViewer(report);
		((RibbonForm)this).EnableGlass = false;
		((RibbonForm)this).WindowState = PreviewWindowState;
		((Form)this).ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
		((Control)this).MinimumSize = StiOptions.Viewer.Windows.MinimumSize;
		if (StiOptions.Viewer.Windows.ViewerIcon != null)
		{
			((Form)this).Icon = StiOptions.Viewer.Windows.ViewerIcon;
		}
		else
		{
			((Form)this).Icon = StiAppIcons.GetIcon();
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
