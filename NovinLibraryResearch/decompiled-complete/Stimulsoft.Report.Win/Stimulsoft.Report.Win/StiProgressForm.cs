using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Win;

public class StiProgressForm : Form, IStiProgressForm
{
	private IContainer components;

	private Label lbProgress;

	public Button btCancel;

	private Panel panelLabel;

	private Panel panelProgress;

	private ProgressBar progressBar;

	private Panel panelFooter;

	private Panel panel1;

	public bool AllowUseDoEvents { get; set; } = true;


	public int Maximum
	{
		get
		{
			return progressBar.Maximum;
		}
		set
		{
			if (((Control)progressBar).InvokeRequired)
			{
				((Control)progressBar).Invoke((Delegate)(EventHandler)delegate
				{
					progressBar.Maximum = value;
				});
				return;
			}
			progressBar.Maximum = value;
			if (AllowUseDoEvents)
			{
				Application.DoEvents();
			}
		}
	}

	public int Minimum
	{
		get
		{
			return progressBar.Minimum;
		}
		set
		{
			if (((Control)progressBar).InvokeRequired)
			{
				((Control)progressBar).Invoke((Delegate)(EventHandler)delegate
				{
					progressBar.Minimum = value;
				});
				return;
			}
			progressBar.Minimum = value;
			if (AllowUseDoEvents)
			{
				Application.DoEvents();
			}
		}
	}

	public int Value
	{
		get
		{
			return progressBar.Value;
		}
		set
		{
			if (((Control)progressBar).InvokeRequired)
			{
				((Control)progressBar).Invoke((Delegate)(EventHandler)delegate
				{
					progressBar.Value = value;
				});
				return;
			}
			progressBar.Value = value;
			if (AllowUseDoEvents)
			{
				Application.DoEvents();
			}
		}
	}

	public string ProgressText
	{
		get
		{
			return ((Control)lbProgress).Text;
		}
		set
		{
			if (((Control)lbProgress).InvokeRequired)
			{
				((Control)lbProgress).Invoke((Delegate)(EventHandler)delegate
				{
					((Control)lbProgress).Text = value;
					((Control)lbProgress).Invalidate();
				});
				return;
			}
			((Control)lbProgress).Text = value;
			if (AllowUseDoEvents)
			{
				Application.DoEvents();
			}
		}
	}

	public bool IsBreaked { get; set; }

	public ProgressBarStyle ProgressBarStyle
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return progressBar.Style;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)progressBar).InvokeRequired)
			{
				((Control)progressBar).Invoke((Delegate)(EventHandler)delegate
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					progressBar.Style = value;
				});
				return;
			}
			progressBar.Style = value;
			if (AllowUseDoEvents)
			{
				Application.DoEvents();
			}
		}
	}

	public bool ProgressBarVisible
	{
		get
		{
			return ((Control)progressBar).Visible;
		}
		set
		{
			if (((Control)progressBar).InvokeRequired)
			{
				((Control)progressBar).Invoke((Delegate)(EventHandler)delegate
				{
					((Control)progressBar).Visible = value;
				});
				return;
			}
			((Control)progressBar).Visible = value;
			if (AllowUseDoEvents)
			{
				Application.DoEvents();
			}
		}
	}

	public bool CancelButtonEnabled
	{
		get
		{
			return ((Control)btCancel).Enabled;
		}
		set
		{
			if (((Control)btCancel).InvokeRequired)
			{
				((Control)btCancel).Invoke((Delegate)(EventHandler)delegate
				{
					((Control)btCancel).Enabled = value;
				});
				return;
			}
			((Control)btCancel).Enabled = value;
			if (AllowUseDoEvents)
			{
				Application.DoEvents();
			}
		}
	}

	public StiProgressForm()
	{
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		DoScale();
		if (!((Component)this).DesignMode)
		{
			Localize();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Report", "PreparingReport");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScale()
	{
		((Form)this).ClientSize = new Size(StiScale.XXI(371.0), StiScale.YYI(125.0));
		((Control)panelFooter).Size = new Size(StiScale.XXI(347.0), StiScale.YYI(29.0));
		((Control)panelLabel).Size = new Size(StiScale.XXI(347.0), StiScale.YYI(40.0));
		((Control)panelProgress).Size = new Size(StiScale.XXI(347.0), StiScale.YYI(30.0));
		((Control)panel1).Size = new Size(StiScale.XXI(347.0), StiScale.YYI(8.0));
		((Control)btCancel).Location = new Point(StiScale.XXI(106.0), StiScale.YYI(3.0));
		((Control)btCancel).Size = new Size(StiScale.XXI(134.0), StiScale.YYI(23.0));
		((Control)progressBar).Location = new Point(StiScale.XXI(12.0), StiScale.YYI(6.0));
		((Control)progressBar).Size = new Size(StiScale.XXI(323.0), StiScale.YYI(15.0));
	}

	public void CancelButtonFocus()
	{
		((Control)btCancel).Focus();
	}

	public void InvalidateProgress()
	{
		((Control)lbProgress).Invalidate();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		IsBreaked = true;
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			IsBreaked = true;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		lbProgress = new Label();
		btCancel = new Button();
		panelLabel = new Panel();
		panelProgress = new Panel();
		progressBar = new ProgressBar();
		panelFooter = new Panel();
		panel1 = new Panel();
		((Control)panelLabel).SuspendLayout();
		((Control)panelProgress).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lbProgress).Dock = (DockStyle)5;
		((Control)lbProgress).Location = new Point(12, 12);
		((Control)lbProgress).Name = "lbProgress";
		((Control)lbProgress).Size = new Size(322, 31);
		((Control)lbProgress).TabIndex = 0;
		((Control)lbProgress).Text = "ProgressText";
		lbProgress.TextAlign = (ContentAlignment)32;
		((Control)btCancel).BackColor = Color.White;
		((Control)btCancel).Location = new Point(106, 3);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(134, 23);
		((Control)btCancel).TabIndex = 0;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)panelLabel).Controls.Add((Control)(object)lbProgress);
		((Control)panelLabel).Dock = (DockStyle)2;
		((Control)panelLabel).Location = new Point(12, 12);
		((Control)panelLabel).Name = "panelLabel";
		((Control)panelLabel).Size = new Size(347, 40);
		((Control)panelLabel).TabIndex = 1;
		panelLabel.TabStop = false;
		((Control)panelProgress).Controls.Add((Control)(object)progressBar);
		((Control)panelProgress).Dock = (DockStyle)2;
		((Control)panelProgress).Location = new Point(12, 12);
		((Control)panelProgress).Name = "panelProgress";
		((Control)panelProgress).Size = new Size(347, 30);
		((Control)panelProgress).TabIndex = 1;
		panelProgress.TabStop = false;
		((Control)progressBar).Location = new Point(12, 6);
		((Control)progressBar).Name = "progressBar";
		((Control)progressBar).Size = new Size(323, 15);
		((Control)progressBar).TabIndex = 1;
		((Control)panelFooter).Controls.Add((Control)(object)btCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		((Control)panelFooter).Location = new Point(12, 100);
		((Control)panelFooter).Margin = new Padding(0);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(347, 29);
		((Control)panelFooter).TabIndex = 2;
		((Control)panel1).Dock = (DockStyle)2;
		((Control)panel1).Location = new Point(12, 92);
		((Control)panel1).Margin = new Padding(0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(347, 8);
		((Control)panel1).TabIndex = 3;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(371, 125);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)panelLabel);
		((Control)this).Controls.Add((Control)(object)panelProgress);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiProgressForm";
		((Control)this).Padding = new Padding(12, 12, 12, 8);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Preparing Report";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panelLabel).ResumeLayout(false);
		((Control)panelProgress).ResumeLayout(false);
		((Control)panelFooter).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
