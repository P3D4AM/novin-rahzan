using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmLockCtrl : Form
{
	private AxSBXPC bpc;

	private IContainer components = null;

	public Button cmdWarnCancel;

	public ToolTip ToolTip1;

	public Button cmdRestart;

	public Button cmdAutoRecover;

	public Button cmdUncondClose;

	public Button cmdUncondOpen;

	public Button cmdGetDoorStatus;

	public Button cmdDoorOpen;

	public Label lblMessage;

	public frmLockCtrl(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void cmdGetDoorStatus_Click(object sender, EventArgs e)
	{
		int dwValue = 0;
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.GetDoorStatus(util.gMachineNumber, ref dwValue))
		{
			switch (dwValue)
			{
			case 1:
				((Control)lblMessage).Text = "Uncond Door Open State!";
				break;
			case 2:
				((Control)lblMessage).Text = "Uncond Door Close State!";
				break;
			case 3:
				((Control)lblMessage).Text = "Door Open State!";
				break;
			case 4:
				((Control)lblMessage).Text = "Auto Recover State!";
				break;
			case 5:
				((Control)lblMessage).Text = "Door Close State!";
				break;
			case 6:
				((Control)lblMessage).Text = "Watching for Close!";
				break;
			case 7:
				((Control)lblMessage).Text = "Illegal open!";
				break;
			default:
				((Control)lblMessage).Text = "User State !";
				break;
			}
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdDoorOpen_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDoorStatus(util.gMachineNumber, 3))
		{
			((Control)lblMessage).Text = "Door Open Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdAutoRecover_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDoorStatus(util.gMachineNumber, 4))
		{
			((Control)lblMessage).Text = "Auto Recover Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdRestart_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDoorStatus(util.gMachineNumber, 5))
		{
			((Control)lblMessage).Text = "Reboot Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdUncondOpen_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDoorStatus(util.gMachineNumber, 1))
		{
			((Control)lblMessage).Text = "Uncond Door Open Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdUncondClose_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDoorStatus(util.gMachineNumber, 2))
		{
			((Control)lblMessage).Text = "Uncond Door Close Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdWarnCancel_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDoorStatus(util.gMachineNumber, 6))
		{
			((Control)lblMessage).Text = "Warning cancel Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void frmLockCrl_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void frmLockCrl_Load(object sender, EventArgs e)
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
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
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Expected O, but got Unknown
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Expected O, but got Unknown
		components = new Container();
		cmdWarnCancel = new Button();
		ToolTip1 = new ToolTip(components);
		cmdRestart = new Button();
		cmdAutoRecover = new Button();
		cmdUncondClose = new Button();
		cmdUncondOpen = new Button();
		cmdGetDoorStatus = new Button();
		cmdDoorOpen = new Button();
		lblMessage = new Label();
		((Control)this).SuspendLayout();
		((Control)cmdWarnCancel).BackColor = SystemColors.Control;
		((Control)cmdWarnCancel).Cursor = Cursors.Default;
		((Control)cmdWarnCancel).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdWarnCancel).ForeColor = SystemColors.ControlText;
		((Control)cmdWarnCancel).Location = new Point(283, 133);
		((Control)cmdWarnCancel).Name = "cmdWarnCancel";
		((Control)cmdWarnCancel).RightToLeft = (RightToLeft)0;
		((Control)cmdWarnCancel).Size = new Size(125, 33);
		((Control)cmdWarnCancel).TabIndex = 15;
		((Control)cmdWarnCancel).Text = "Warn Cancel";
		((ButtonBase)cmdWarnCancel).UseVisualStyleBackColor = false;
		((Control)cmdWarnCancel).Click += cmdWarnCancel_Click;
		((Control)cmdRestart).BackColor = SystemColors.Control;
		((Control)cmdRestart).Cursor = Cursors.Default;
		((Control)cmdRestart).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdRestart).ForeColor = SystemColors.ControlText;
		((Control)cmdRestart).Location = new Point(143, 134);
		((Control)cmdRestart).Name = "cmdRestart";
		((Control)cmdRestart).RightToLeft = (RightToLeft)0;
		((Control)cmdRestart).Size = new Size(124, 32);
		((Control)cmdRestart).TabIndex = 14;
		((Control)cmdRestart).Text = "Reboot";
		((ButtonBase)cmdRestart).UseVisualStyleBackColor = false;
		((Control)cmdRestart).Click += cmdRestart_Click;
		((Control)cmdAutoRecover).BackColor = SystemColors.Control;
		((Control)cmdAutoRecover).Cursor = Cursors.Default;
		((Control)cmdAutoRecover).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdAutoRecover).ForeColor = SystemColors.ControlText;
		((Control)cmdAutoRecover).Location = new Point(143, 95);
		((Control)cmdAutoRecover).Name = "cmdAutoRecover";
		((Control)cmdAutoRecover).RightToLeft = (RightToLeft)0;
		((Control)cmdAutoRecover).Size = new Size(124, 34);
		((Control)cmdAutoRecover).TabIndex = 13;
		((Control)cmdAutoRecover).Text = "Auto Recover";
		((ButtonBase)cmdAutoRecover).UseVisualStyleBackColor = false;
		((Control)cmdAutoRecover).Click += cmdAutoRecover_Click;
		((Control)cmdUncondClose).BackColor = SystemColors.Control;
		((Control)cmdUncondClose).Cursor = Cursors.Default;
		((Control)cmdUncondClose).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdUncondClose).ForeColor = SystemColors.ControlText;
		((Control)cmdUncondClose).Location = new Point(283, 94);
		((Control)cmdUncondClose).Name = "cmdUncondClose";
		((Control)cmdUncondClose).RightToLeft = (RightToLeft)0;
		((Control)cmdUncondClose).Size = new Size(126, 33);
		((Control)cmdUncondClose).TabIndex = 12;
		((Control)cmdUncondClose).Text = "Uncond Close";
		((ButtonBase)cmdUncondClose).UseVisualStyleBackColor = false;
		((Control)cmdUncondClose).Click += cmdUncondClose_Click;
		((Control)cmdUncondOpen).BackColor = SystemColors.Control;
		((Control)cmdUncondOpen).Cursor = Cursors.Default;
		((Control)cmdUncondOpen).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdUncondOpen).ForeColor = SystemColors.ControlText;
		((Control)cmdUncondOpen).Location = new Point(284, 57);
		((Control)cmdUncondOpen).Name = "cmdUncondOpen";
		((Control)cmdUncondOpen).RightToLeft = (RightToLeft)0;
		((Control)cmdUncondOpen).Size = new Size(126, 33);
		((Control)cmdUncondOpen).TabIndex = 11;
		((Control)cmdUncondOpen).Text = "Uncond Open";
		((ButtonBase)cmdUncondOpen).UseVisualStyleBackColor = false;
		((Control)cmdUncondOpen).Click += cmdUncondOpen_Click;
		((Control)cmdGetDoorStatus).BackColor = SystemColors.Control;
		((Control)cmdGetDoorStatus).Cursor = Cursors.Default;
		((Control)cmdGetDoorStatus).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDoorStatus).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDoorStatus).Location = new Point(7, 59);
		((Control)cmdGetDoorStatus).Name = "cmdGetDoorStatus";
		((Control)cmdGetDoorStatus).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDoorStatus).Size = new Size(122, 33);
		((Control)cmdGetDoorStatus).TabIndex = 10;
		((Control)cmdGetDoorStatus).Text = "Get DoorStatus";
		((ButtonBase)cmdGetDoorStatus).UseVisualStyleBackColor = false;
		((Control)cmdGetDoorStatus).Click += cmdGetDoorStatus_Click;
		((Control)cmdDoorOpen).BackColor = SystemColors.Control;
		((Control)cmdDoorOpen).Cursor = Cursors.Default;
		((Control)cmdDoorOpen).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDoorOpen).ForeColor = SystemColors.ControlText;
		((Control)cmdDoorOpen).Location = new Point(143, 57);
		((Control)cmdDoorOpen).Name = "cmdDoorOpen";
		((Control)cmdDoorOpen).RightToLeft = (RightToLeft)0;
		((Control)cmdDoorOpen).Size = new Size(125, 33);
		((Control)cmdDoorOpen).TabIndex = 9;
		((Control)cmdDoorOpen).Text = "Door Open";
		((ButtonBase)cmdDoorOpen).UseVisualStyleBackColor = false;
		((Control)cmdDoorOpen).Click += cmdDoorOpen_Click;
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(7, 14);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(401, 28);
		((Control)lblMessage).TabIndex = 8;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(417, 173);
		((Control)this).Controls.Add((Control)(object)cmdWarnCancel);
		((Control)this).Controls.Add((Control)(object)cmdRestart);
		((Control)this).Controls.Add((Control)(object)cmdAutoRecover);
		((Control)this).Controls.Add((Control)(object)cmdUncondClose);
		((Control)this).Controls.Add((Control)(object)cmdUncondOpen);
		((Control)this).Controls.Add((Control)(object)cmdGetDoorStatus);
		((Control)this).Controls.Add((Control)(object)cmdDoorOpen);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "frmLockCtrl";
		((Control)this).Text = "frmLockCrl";
		((Form)this).Load += frmLockCrl_Load;
		((Form)this).FormClosed += new FormClosedEventHandler(frmLockCrl_FormClosed);
		((Control)this).ResumeLayout(false);
	}
}
