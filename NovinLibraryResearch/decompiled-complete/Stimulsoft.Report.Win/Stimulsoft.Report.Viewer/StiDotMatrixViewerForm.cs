using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

namespace Stimulsoft.Report.Viewer;

public class StiDotMatrixViewerForm : Form, IStiViewerForm
{
	public StiDotMatrixViewerControl viewerControl;

	private Container components;

	private ThumbButtonManager thumbButtonManager;

	private bool Lock;

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
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDotMatrixViewerForm));
		viewerControl = new StiDotMatrixViewerControl();
		((Control)this).SuspendLayout();
		((Control)viewerControl).Dock = (DockStyle)5;
		((Control)viewerControl).Location = new Point(0, 0);
		((Control)viewerControl).Name = "viewerControl";
		viewerControl.Report = null;
		((Control)viewerControl).Size = new Size(640, 477);
		((Control)viewerControl).TabIndex = 0;
		viewerControl.Close += ViewerControl_Close;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Form)this).ClientSize = new Size(640, 477);
		((Control)this).Controls.Add((Control)(object)viewerControl);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).Name = "StiDotMatrixViewerForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiViewerForm";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(StiViewerForm_KeyDown);
		((Control)this).ResumeLayout(false);
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
				thumbButtonManager.AddThumbButtons((ThumbButton[])(object)new ThumbButton[3] { val, val2, val3 });
			}
			if (thumbButtonManager != null)
			{
				thumbButtonManager.DispatchMessage(ref m);
			}
		}
		((Form)this).WndProc(ref m);
	}

	private void ViewerControl_Close(object sender, EventArgs e)
	{
		if (!Lock)
		{
			((Form)this).Close();
		}
	}

	private void StiViewerForm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	public StiDotMatrixViewerForm()
		: this(null)
	{
	}

	public StiDotMatrixViewerForm(StiReport report)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Form)this).WindowState = PreviewWindowState;
			((Form)this).ShowInTaskbar = StiOptions.Viewer.DotMatrix.ShowInTaskbar;
			Report = report;
			((Form)this).Icon = StiOptions.Viewer.Windows.ViewerIcon ?? StiAppIcons.GetIcon();
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
