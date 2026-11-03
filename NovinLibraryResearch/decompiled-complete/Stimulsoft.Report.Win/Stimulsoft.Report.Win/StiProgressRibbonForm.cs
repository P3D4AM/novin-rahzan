using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Report.Win;

public class StiProgressRibbonForm : Office2007Form, IStiProgressForm
{
	private bool allowUseDoEvents = true;

	private bool isBreaked;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX btCancel;

	private GroupPanel groupPanel1;

	private ProgressBar progressBar;

	private LabelX lbProgress;

	public bool AllowUseDoEvents
	{
		get
		{
			return allowUseDoEvents;
		}
		set
		{
			if (allowUseDoEvents != value)
			{
				allowUseDoEvents = value;
			}
		}
	}

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

	public bool IsBreaked
	{
		get
		{
			return isBreaked;
		}
		set
		{
			isBreaked = value;
		}
	}

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

	public StiProgressRibbonForm()
	{
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
		}
	}

	private void btCancel_Click(object sender, EventArgs e)
	{
		IsBreaked = true;
	}

	private void StiProgressForm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			IsBreaked = true;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Report", "PreparingReport");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	public void InvalidateProgress()
	{
		((Control)lbProgress).Invalidate();
	}

	public void CancelButtonFocus()
	{
		((Control)btCancel).Focus();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((OfficeForm)this).Dispose(disposing);
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
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		btCancel = new ButtonX();
		groupPanel1 = new GroupPanel();
		progressBar = new ProgressBar();
		lbProgress = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)panelEx1).Controls.Add((Control)(object)btCancel);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel1);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(325, 146);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 5;
		((Control)btCancel).AccessibleRole = (AccessibleRole)43;
		((Control)btCancel).Anchor = (AnchorStyles)0;
		btCancel.ColorTable = (eButtonColor)3;
		btCancel.Is20Button = false;
		((Control)btCancel).Location = new Point(116, 109);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(88, 23);
		((PopupItemControl)btCancel).Style = (eDotNetBarStyle)8;
		((Control)btCancel).TabIndex = 6;
		((Control)btCancel).Text = "Cancel";
		((Control)btCancel).Click += btCancel_Click;
		((Control)groupPanel1).Anchor = (AnchorStyles)15;
		((Control)groupPanel1).BackColor = Color.Transparent;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)groupPanel1).Controls.Add((Control)(object)progressBar);
		((Control)groupPanel1).Controls.Add((Control)(object)lbProgress);
		((Control)groupPanel1).Location = new Point(12, 12);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(301, 84);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 5;
		((Control)progressBar).Location = new Point(12, 47);
		((Control)progressBar).Name = "progressBar";
		((Control)progressBar).Size = new Size(250, 15);
		((Control)progressBar).TabIndex = 2;
		((Control)lbProgress).Anchor = (AnchorStyles)15;
		((Control)lbProgress).BackColor = Color.Transparent;
		((BaseItemControl)lbProgress).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lbProgress).Location = new Point(12, 12);
		((Control)lbProgress).Name = "lbProgress";
		((Control)lbProgress).Size = new Size(268, 25);
		((BaseItemControl)lbProgress).Style = (eDotNetBarStyle)8;
		((Control)lbProgress).TabIndex = 0;
		lbProgress.TextAlignment = (StringAlignment)1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(325, 146);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).DoubleBuffered = true;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiProgressRibbonForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiProgressRibbonForm";
		((Control)this).KeyDown += new KeyEventHandler(StiProgressForm_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
