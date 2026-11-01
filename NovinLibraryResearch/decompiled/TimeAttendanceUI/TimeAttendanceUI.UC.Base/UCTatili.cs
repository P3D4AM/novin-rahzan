using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace TimeAttendanceUI.UC.Base;

public class UCTatili : UserControl
{
	public delegate void ColorTatiliChanedEventHandler();

	private IContainer components;

	public LabelX lBLRasmi;

	public LabelX LBLGhRasmi;

	public Color Rasmi
	{
		get
		{
			return ((Control)lBLRasmi).ForeColor;
		}
		set
		{
			((Control)lBLRasmi).ForeColor = value;
		}
	}

	public Color GhRasmi
	{
		get
		{
			return ((Control)LBLGhRasmi).ForeColor;
		}
		set
		{
			((Control)LBLGhRasmi).ForeColor = value;
		}
	}

	public event ColorTatiliChanedEventHandler ColorTatiliChanedEvent;

	public UCTatili()
	{
		InitializeComponent();
	}

	private void OnColorTatiliChanedEvent()
	{
		if (this.ColorTatiliChanedEvent != null)
		{
			this.ColorTatiliChanedEvent();
		}
	}

	private void LBL_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		ColorDialog val = new ColorDialog();
		val.Color = ((Control)(LabelX)sender).ForeColor;
		if ((int)((CommonDialog)val).ShowDialog() == 1)
		{
			((Control)(LabelX)sender).ForeColor = val.Color;
			OnColorTatiliChanedEvent();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		lBLRasmi = new LabelX();
		LBLGhRasmi = new LabelX();
		((Control)this).SuspendLayout();
		((Control)lBLRasmi).Anchor = (AnchorStyles)13;
		((BaseItemControl)lBLRasmi).BackgroundStyle.Class = "";
		((BaseItemControl)lBLRasmi).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lBLRasmi).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lBLRasmi).ForeColor = Color.Red;
		((Control)lBLRasmi).Location = new Point(4, 22);
		((Control)lBLRasmi).Name = "lBLRasmi";
		((Control)lBLRasmi).Size = new Size(105, 23);
		((Control)lBLRasmi).TabIndex = 30;
		((Control)lBLRasmi).Text = "[تعطیلی]";
		lBLRasmi.TextAlignment = (StringAlignment)1;
		((Control)lBLRasmi).Click += LBL_Click;
		((Control)LBLGhRasmi).Anchor = (AnchorStyles)13;
		((BaseItemControl)LBLGhRasmi).BackgroundStyle.Class = "";
		((BaseItemControl)LBLGhRasmi).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LBLGhRasmi).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLGhRasmi).ForeColor = Color.Black;
		((Control)LBLGhRasmi).Location = new Point(4, 51);
		((Control)LBLGhRasmi).Name = "LBLGhRasmi";
		((Control)LBLGhRasmi).Size = new Size(105, 23);
		((Control)LBLGhRasmi).TabIndex = 31;
		((Control)LBLGhRasmi).Text = "[غیر تعطیلی]";
		LBLGhRasmi.TextAlignment = (StringAlignment)1;
		((Control)LBLGhRasmi).Click += LBL_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)LBLGhRasmi);
		((Control)this).Controls.Add((Control)(object)lBLRasmi);
		((Control)this).Name = "UCTatili";
		((Control)this).Size = new Size(112, 111);
		((Control)this).ResumeLayout(false);
	}
}
