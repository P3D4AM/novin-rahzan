using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Viewer;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

namespace Stimulsoft.Report.Viewer;

public class StiViewerForm : Form, IStiViewerForm
{
	private Container components;

	private ThumbButtonManager thumbButtonManager;

	private StiViewerControl viewerControl;

	private bool lockObject;

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

	public StiViewerControl ViewerControl
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

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
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
		((Form)this).WndProc(ref m);
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

	private void InitializeComponent()
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiViewerForm));
		viewerControl = new StiViewerControl();
		((Control)this).SuspendLayout();
		((Control)viewerControl).Dock = (DockStyle)5;
		((Control)viewerControl).Location = new Point(0, 0);
		((Control)viewerControl).Name = "viewerControl";
		((Control)viewerControl).RightToLeft = (RightToLeft)0;
		viewerControl.ShowZoom = true;
		((Control)viewerControl).Size = new Size(704, 446);
		((Control)viewerControl).TabIndex = 0;
		viewerControl.Close += ViewerControl_Close;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(704, 446);
		((Control)this).Controls.Add((Control)(object)viewerControl);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).Name = "StiViewerForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiViewerForm";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Control)this).ResumeLayout(false);
	}

	private void ViewerControl_Close(object sender, EventArgs e)
	{
		if (!lockObject)
		{
			((Form)this).Close();
		}
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		IStiViewerAccessor val = (IStiViewerAccessor)(object)viewerControl;
		if (val.BlockFormClose)
		{
			val.BlockFormClose = false;
			((CancelEventArgs)(object)e).Cancel = true;
		}
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).Close();
		}
	}

	public StiViewerForm()
		: this(null)
	{
	}

	public StiViewerForm(StiReport report)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		Report = report;
		((Form)this).WindowState = PreviewWindowState;
		((Form)this).ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
		((Control)this).MinimumSize = StiOptions.Viewer.Windows.MinimumSize;
		((Form)this).Icon = StiOptions.Viewer.Windows.ViewerIcon ?? StiAppIcons.GetIcon();
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
