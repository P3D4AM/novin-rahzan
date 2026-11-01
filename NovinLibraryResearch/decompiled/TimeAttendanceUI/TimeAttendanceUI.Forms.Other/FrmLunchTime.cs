using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Other;

public class FrmLunchTime : Form
{
	private readonly Image _img;

	private IContainer components;

	private ButtonX BtnExit;

	public FrmLunchTime(Image img)
	{
		_img = img;
		InitializeComponent();
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void FrmLunchTime_Load(object sender, EventArgs e)
	{
		((Control)this).BackgroundImage = _img;
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
		BtnExit = new ButtonX();
		((Control)this).SuspendLayout();
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		BtnExit.ColorTable = (eButtonColor)3;
		BtnExit.DialogResult = (DialogResult)2;
		BtnExit.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnExit).Location = new Point(770, 7);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Size = new Size(27, 25);
		((PopupItemControl)BtnExit).Style = (eDotNetBarStyle)9;
		((Control)BtnExit).TabIndex = 2;
		((Control)BtnExit).Click += BtnExit_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)BtnExit;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.question;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).CancelButton = (IButtonControl)(object)BtnExit;
		((Form)this).ClientSize = new Size(800, 450);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)BtnExit);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)2;
		((Control)this).Name = "FrmLunchTime";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).TopMost = true;
		((Form)this).Load += FrmLunchTime_Load;
		((Control)this).ResumeLayout(false);
	}
}
