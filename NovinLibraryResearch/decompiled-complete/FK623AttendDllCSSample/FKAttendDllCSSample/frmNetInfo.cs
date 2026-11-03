using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmNetInfo : Form
{
	public Button cmbSetNetInfo;

	public Button cmbGetNetInfo;

	public CheckBox chkSerReq;

	public TextBox txtServerPort;

	public Label Label1;

	public TextBox txtServerIP;

	public Label lblSerVerIP;

	public Label lblMessage;

	public ToolTip ToolTip1;

	private IContainer components;

	public frmNetInfo()
	{
		InitializeComponent();
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Expected O, but got Unknown
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Expected O, but got Unknown
		components = new Container();
		cmbSetNetInfo = new Button();
		cmbGetNetInfo = new Button();
		chkSerReq = new CheckBox();
		txtServerPort = new TextBox();
		Label1 = new Label();
		txtServerIP = new TextBox();
		lblSerVerIP = new Label();
		lblMessage = new Label();
		ToolTip1 = new ToolTip(components);
		((Control)this).SuspendLayout();
		((Control)cmbSetNetInfo).BackColor = SystemColors.Control;
		((Control)cmbSetNetInfo).Cursor = Cursors.Default;
		((Control)cmbSetNetInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbSetNetInfo).ForeColor = SystemColors.ControlText;
		((Control)cmbSetNetInfo).Location = new Point(233, 189);
		((Control)cmbSetNetInfo).Name = "cmbSetNetInfo";
		((Control)cmbSetNetInfo).RightToLeft = (RightToLeft)0;
		((Control)cmbSetNetInfo).Size = new Size(113, 33);
		((Control)cmbSetNetInfo).TabIndex = 12;
		((Control)cmbSetNetInfo).Text = "Set NetInfo";
		((ButtonBase)cmbSetNetInfo).UseVisualStyleBackColor = false;
		((Control)cmbSetNetInfo).Click += cmbSetNetInfo_Click;
		((Control)cmbGetNetInfo).BackColor = SystemColors.Control;
		((Control)cmbGetNetInfo).Cursor = Cursors.Default;
		((Control)cmbGetNetInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbGetNetInfo).ForeColor = SystemColors.ControlText;
		((Control)cmbGetNetInfo).Location = new Point(108, 189);
		((Control)cmbGetNetInfo).Name = "cmbGetNetInfo";
		((Control)cmbGetNetInfo).RightToLeft = (RightToLeft)0;
		((Control)cmbGetNetInfo).Size = new Size(113, 33);
		((Control)cmbGetNetInfo).TabIndex = 11;
		((Control)cmbGetNetInfo).Text = "Get NetInfo";
		((ButtonBase)cmbGetNetInfo).UseVisualStyleBackColor = false;
		((Control)cmbGetNetInfo).Click += cmbGetNetInfo_Click;
		((Control)chkSerReq).BackColor = SystemColors.Control;
		((Control)chkSerReq).Cursor = Cursors.Default;
		((Control)chkSerReq).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkSerReq).ForeColor = SystemColors.ControlText;
		((Control)chkSerReq).Location = new Point(196, 141);
		((Control)chkSerReq).Name = "chkSerReq";
		((Control)chkSerReq).RightToLeft = (RightToLeft)0;
		((Control)chkSerReq).Size = new Size(145, 33);
		((Control)chkSerReq).TabIndex = 10;
		((Control)chkSerReq).Text = "ServerRequest";
		((ButtonBase)chkSerReq).UseVisualStyleBackColor = false;
		txtServerPort.AcceptsReturn = true;
		((Control)txtServerPort).BackColor = SystemColors.Window;
		((Control)txtServerPort).Cursor = Cursors.IBeam;
		((Control)txtServerPort).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtServerPort).ForeColor = SystemColors.WindowText;
		((Control)txtServerPort).Location = new Point(196, 109);
		((TextBoxBase)txtServerPort).MaxLength = 32;
		((Control)txtServerPort).Name = "txtServerPort";
		((Control)txtServerPort).RightToLeft = (RightToLeft)0;
		((Control)txtServerPort).Size = new Size(150, 27);
		((Control)txtServerPort).TabIndex = 9;
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(36, 109);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(90, 19);
		((Control)Label1).TabIndex = 14;
		((Control)Label1).Text = "Server Port : ";
		txtServerIP.AcceptsReturn = true;
		((Control)txtServerIP).BackColor = SystemColors.Window;
		((Control)txtServerIP).Cursor = Cursors.IBeam;
		((Control)txtServerIP).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtServerIP).ForeColor = SystemColors.WindowText;
		((Control)txtServerIP).Location = new Point(196, 69);
		((TextBoxBase)txtServerIP).MaxLength = 32;
		((Control)txtServerIP).Name = "txtServerIP";
		((Control)txtServerIP).RightToLeft = (RightToLeft)0;
		((Control)txtServerIP).Size = new Size(150, 27);
		((Control)txtServerIP).TabIndex = 8;
		((Control)lblSerVerIP).AutoSize = true;
		((Control)lblSerVerIP).BackColor = SystemColors.Control;
		((Control)lblSerVerIP).Cursor = Cursors.Default;
		((Control)lblSerVerIP).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblSerVerIP).ForeColor = SystemColors.ControlText;
		((Control)lblSerVerIP).Location = new Point(36, 69);
		((Control)lblSerVerIP).Name = "lblSerVerIP";
		((Control)lblSerVerIP).RightToLeft = (RightToLeft)0;
		((Control)lblSerVerIP).Size = new Size(133, 19);
		((Control)lblSerVerIP).TabIndex = 15;
		((Control)lblSerVerIP).Text = "Server IP Address : ";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(36, 23);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(313, 28);
		((Control)lblMessage).TabIndex = 13;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Form)this).AutoScaleBaseSize = new Size(7, 19);
		((Form)this).ClientSize = new Size(384, 238);
		((Control)this).Controls.Add((Control)(object)cmbGetNetInfo);
		((Control)this).Controls.Add((Control)(object)chkSerReq);
		((Control)this).Controls.Add((Control)(object)txtServerPort);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)txtServerIP);
		((Control)this).Controls.Add((Control)(object)lblSerVerIP);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)cmbSetNetInfo);
		((Control)this).Font = new Font("Times New Roman", 12f);
		((Form)this).Location = new Point(4, 25);
		((Control)this).Name = "frmNetInfo";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Get && Set Net Information";
		((Form)this).Load += frmNetInfo_Load;
		((Form)this).Closed += frmNetInfo_Closed;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void cmbGetNetInfo_Click(object sender, EventArgs e)
	{
		string apstrServerIPAddress = "";
		int apnServerPort = 0;
		int apnServerRequest = 0;
		((Control)cmbGetNetInfo).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmbGetNetInfo).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_GetServerNetInfo(FKAttendDLL.nCommHandleIndex, ref apstrServerIPAddress, ref apnServerPort, ref apnServerRequest);
		if (num == 1)
		{
			((Control)txtServerIP).Text = apstrServerIPAddress;
			((Control)txtServerPort).Text = Convert.ToString(apnServerPort);
			if (apnServerRequest == 1)
			{
				chkSerReq.CheckState = (CheckState)1;
			}
			else
			{
				chkSerReq.CheckState = (CheckState)0;
			}
		}
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmbGetNetInfo).Enabled = true;
	}

	private void cmbSetNetInfo_Click(object sender, EventArgs e)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		((Control)cmbSetNetInfo).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmbSetNetInfo).Enabled = true;
			return;
		}
		string astrServerIPAddress = ((Control)txtServerIP).Text.Trim();
		int anServerPort = Convert.ToInt32(((Control)txtServerPort).Text.Trim());
		num = FKAttendDLL.FK_SetServerNetInfo(anServerRequest: ((int)chkSerReq.CheckState == 1) ? 1 : 0, nHandleIndex: FKAttendDLL.nCommHandleIndex, astrServerIPAddress: astrServerIPAddress, anServerPort: anServerPort);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmbSetNetInfo).Enabled = true;
	}

	private void frmNetInfo_Load(object sender, EventArgs e)
	{
		((Control)txtServerIP).Text = "0.0.0.0";
		((Control)txtServerPort).Text = Convert.ToString(0);
	}

	private void frmNetInfo_Closed(object sender, EventArgs e)
	{
		((Control)FrmConnectDeviceHysoon.GfrmMain).Visible = true;
	}
}
