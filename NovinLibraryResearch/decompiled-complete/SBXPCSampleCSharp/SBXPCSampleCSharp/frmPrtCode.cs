using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmPrtCode : Form
{
	private AxSBXPC bpc;

	private IContainer components = null;

	public ToolTip ToolTip1;

	public TextBox txtSerialNo;

	public TextBox txtProductCode;

	public Button cmdGetProductCode;

	public Button cmdGetBackupNumber;

	public TextBox txtBackupNo;

	public Label lblSerialNo;

	public Button cmdGetSerialNumber;

	public Label lblBackuplNo;

	public ToolTip toolTip2;

	public Button cmdExit;

	public Label Label1;

	public Label lblMessage;

	public frmPrtCode(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void cmdGetSerialNumber_Click(object sender, EventArgs e)
	{
		string lpszSerialNumber = "";
		int dwErrorCode = 0;
		((Control)txtSerialNo).Text = "";
		((Control)lblMessage).Text = "Waiting...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.GetSerialNumber(util.gMachineNumber, ref lpszSerialNumber))
		{
			((Control)txtSerialNo).Text = lpszSerialNumber;
			((Control)lblMessage).Text = "Success";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdGetBackupNumber_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Waiting...";
		((Control)txtBackupNo).Text = "";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int backupNumber = bpc.GetBackupNumber(util.gMachineNumber);
		if (backupNumber != 0)
		{
			((Control)txtBackupNo).Text = Convert.ToString(backupNumber);
			((Control)lblMessage).Text = "Success";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdGetProductCode_Click(object sender, EventArgs e)
	{
		string lpszProductCode = "";
		int dwErrorCode = 0;
		((Control)txtProductCode).Text = "";
		((Control)lblMessage).Text = "Waiting...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.GetProductCode(util.gMachineNumber, ref lpszProductCode))
		{
			((Control)txtProductCode).Text = lpszProductCode;
			((Control)lblMessage).Text = "Success";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void frmPrtCode_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void frmPrtCode_Load(object sender, EventArgs e)
	{
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Expected O, but got Unknown
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		components = new Container();
		ToolTip1 = new ToolTip(components);
		txtSerialNo = new TextBox();
		txtProductCode = new TextBox();
		cmdGetProductCode = new Button();
		cmdGetBackupNumber = new Button();
		txtBackupNo = new TextBox();
		lblSerialNo = new Label();
		cmdGetSerialNumber = new Button();
		lblBackuplNo = new Label();
		toolTip2 = new ToolTip(components);
		cmdExit = new Button();
		Label1 = new Label();
		lblMessage = new Label();
		((Control)this).SuspendLayout();
		txtSerialNo.AcceptsReturn = true;
		((Control)txtSerialNo).BackColor = SystemColors.Window;
		((Control)txtSerialNo).Cursor = Cursors.IBeam;
		((Control)txtSerialNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtSerialNo).ForeColor = SystemColors.WindowText;
		((Control)txtSerialNo).Location = new Point(165, 62);
		((TextBoxBase)txtSerialNo).MaxLength = 32;
		((Control)txtSerialNo).Name = "txtSerialNo";
		((Control)txtSerialNo).RightToLeft = (RightToLeft)0;
		((Control)txtSerialNo).Size = new Size(319, 26);
		((Control)txtSerialNo).TabIndex = 24;
		txtProductCode.AcceptsReturn = true;
		((Control)txtProductCode).BackColor = SystemColors.Window;
		((Control)txtProductCode).Cursor = Cursors.IBeam;
		((Control)txtProductCode).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtProductCode).ForeColor = SystemColors.WindowText;
		((Control)txtProductCode).Location = new Point(165, 126);
		((TextBoxBase)txtProductCode).MaxLength = 32;
		((Control)txtProductCode).Name = "txtProductCode";
		((Control)txtProductCode).RightToLeft = (RightToLeft)0;
		((Control)txtProductCode).Size = new Size(319, 26);
		((Control)txtProductCode).TabIndex = 28;
		((Control)cmdGetProductCode).BackColor = SystemColors.Control;
		((Control)cmdGetProductCode).Cursor = Cursors.Default;
		((Control)cmdGetProductCode).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetProductCode).ForeColor = SystemColors.ControlText;
		((Control)cmdGetProductCode).Location = new Point(263, 183);
		((Control)cmdGetProductCode).Name = "cmdGetProductCode";
		((Control)cmdGetProductCode).RightToLeft = (RightToLeft)0;
		((Control)cmdGetProductCode).Size = new Size(121, 45);
		((Control)cmdGetProductCode).TabIndex = 27;
		((Control)cmdGetProductCode).Text = "Get ProductCode";
		((ButtonBase)cmdGetProductCode).UseVisualStyleBackColor = true;
		((Control)cmdGetProductCode).Click += cmdGetProductCode_Click;
		((Control)cmdGetBackupNumber).BackColor = SystemColors.Control;
		((Control)cmdGetBackupNumber).Cursor = Cursors.Default;
		((Control)cmdGetBackupNumber).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetBackupNumber).ForeColor = SystemColors.ControlText;
		((Control)cmdGetBackupNumber).Location = new Point(136, 182);
		((Control)cmdGetBackupNumber).Name = "cmdGetBackupNumber";
		((Control)cmdGetBackupNumber).RightToLeft = (RightToLeft)0;
		((Control)cmdGetBackupNumber).Size = new Size(121, 45);
		((Control)cmdGetBackupNumber).TabIndex = 26;
		((Control)cmdGetBackupNumber).Text = "Get BackupNumber";
		((ButtonBase)cmdGetBackupNumber).UseVisualStyleBackColor = true;
		((Control)cmdGetBackupNumber).Click += cmdGetBackupNumber_Click;
		txtBackupNo.AcceptsReturn = true;
		((Control)txtBackupNo).BackColor = SystemColors.Window;
		((Control)txtBackupNo).Cursor = Cursors.IBeam;
		((Control)txtBackupNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtBackupNo).ForeColor = SystemColors.WindowText;
		((Control)txtBackupNo).Location = new Point(165, 94);
		((TextBoxBase)txtBackupNo).MaxLength = 32;
		((Control)txtBackupNo).Name = "txtBackupNo";
		((Control)txtBackupNo).RightToLeft = (RightToLeft)0;
		((Control)txtBackupNo).Size = new Size(319, 26);
		((Control)txtBackupNo).TabIndex = 25;
		((Control)lblSerialNo).AutoSize = true;
		((Control)lblSerialNo).BackColor = SystemColors.Control;
		((Control)lblSerialNo).Cursor = Cursors.Default;
		((Control)lblSerialNo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblSerialNo).ForeColor = SystemColors.ControlText;
		((Control)lblSerialNo).Location = new Point(28, 66);
		((Control)lblSerialNo).Name = "lblSerialNo";
		((Control)lblSerialNo).RightToLeft = (RightToLeft)0;
		((Control)lblSerialNo).Size = new Size(115, 19);
		((Control)lblSerialNo).TabIndex = 30;
		((Control)lblSerialNo).Text = "Serial Number :";
		lblSerialNo.TextAlign = (ContentAlignment)4;
		((Control)cmdGetSerialNumber).BackColor = SystemColors.Control;
		((Control)cmdGetSerialNumber).Cursor = Cursors.Default;
		((Control)cmdGetSerialNumber).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetSerialNumber).ForeColor = SystemColors.ControlText;
		((Control)cmdGetSerialNumber).Location = new Point(12, 182);
		((Control)cmdGetSerialNumber).Name = "cmdGetSerialNumber";
		((Control)cmdGetSerialNumber).RightToLeft = (RightToLeft)0;
		((Control)cmdGetSerialNumber).Size = new Size(121, 45);
		((Control)cmdGetSerialNumber).TabIndex = 22;
		((Control)cmdGetSerialNumber).Text = "Get SerialNumber";
		((ButtonBase)cmdGetSerialNumber).UseVisualStyleBackColor = true;
		((Control)cmdGetSerialNumber).Click += cmdGetSerialNumber_Click;
		((Control)lblBackuplNo).AutoSize = true;
		((Control)lblBackuplNo).BackColor = SystemColors.Control;
		((Control)lblBackuplNo).Cursor = Cursors.Default;
		((Control)lblBackuplNo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblBackuplNo).ForeColor = SystemColors.ControlText;
		((Control)lblBackuplNo).Location = new Point(28, 98);
		((Control)lblBackuplNo).Name = "lblBackuplNo";
		((Control)lblBackuplNo).RightToLeft = (RightToLeft)0;
		((Control)lblBackuplNo).Size = new Size(127, 19);
		((Control)lblBackuplNo).TabIndex = 32;
		((Control)lblBackuplNo).Text = "Backup Number :";
		lblBackuplNo.TextAlign = (ContentAlignment)4;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(390, 182);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(94, 45);
		((Control)cmdExit).TabIndex = 23;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = true;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(28, 130);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(108, 19);
		((Control)Label1).TabIndex = 31;
		((Control)Label1).Text = "Product Code :";
		Label1.TextAlign = (ContentAlignment)4;
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 22);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(495, 28);
		((Control)lblMessage).TabIndex = 29;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(519, 241);
		((Control)this).Controls.Add((Control)(object)txtSerialNo);
		((Control)this).Controls.Add((Control)(object)txtProductCode);
		((Control)this).Controls.Add((Control)(object)cmdGetProductCode);
		((Control)this).Controls.Add((Control)(object)cmdGetBackupNumber);
		((Control)this).Controls.Add((Control)(object)txtBackupNo);
		((Control)this).Controls.Add((Control)(object)lblSerialNo);
		((Control)this).Controls.Add((Control)(object)cmdGetSerialNumber);
		((Control)this).Controls.Add((Control)(object)lblBackuplNo);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmPrtCode";
		((Control)this).Text = "frmPrtCode";
		((Form)this).Load += frmPrtCode_Load;
		((Form)this).FormClosed += new FormClosedEventHandler(frmPrtCode_FormClosed);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
