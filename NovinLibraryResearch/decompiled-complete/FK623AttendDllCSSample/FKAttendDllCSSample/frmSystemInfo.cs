using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmSystemInfo : Form
{
	public Button cmdGetDeviceTime;

	public Button cmdGetDeviceInfo;

	public Button cmdPowerOn;

	public Button PowerOffDevice;

	public Button cmdSetDeviceInfo;

	public ComboBox cmbSatus;

	public TextBox txtSetDevInfo;

	public Button cmdGetDeviceStaus;

	public CheckBox chkEnableDevice;

	public Label Label1;

	public Label lblStatus;

	public Label lblMessage;

	public Button cmdSetDeviceTime;

	private Container components = null;

	public frmSystemInfo()
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
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
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Expected O, but got Unknown
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Expected O, but got Unknown
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Expected O, but got Unknown
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Expected O, but got Unknown
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ce: Expected O, but got Unknown
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a96: Expected O, but got Unknown
		//IL_0b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5e: Expected O, but got Unknown
		cmdGetDeviceTime = new Button();
		cmdGetDeviceInfo = new Button();
		cmdPowerOn = new Button();
		PowerOffDevice = new Button();
		cmdSetDeviceInfo = new Button();
		cmbSatus = new ComboBox();
		txtSetDevInfo = new TextBox();
		cmdGetDeviceStaus = new Button();
		chkEnableDevice = new CheckBox();
		Label1 = new Label();
		lblStatus = new Label();
		lblMessage = new Label();
		cmdSetDeviceTime = new Button();
		((Control)this).SuspendLayout();
		((Control)cmdGetDeviceTime).BackColor = SystemColors.Control;
		((Control)cmdGetDeviceTime).Cursor = Cursors.Default;
		((Control)cmdGetDeviceTime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDeviceTime).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDeviceTime).Location = new Point(13, 71);
		((Control)cmdGetDeviceTime).Name = "cmdGetDeviceTime";
		((Control)cmdGetDeviceTime).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDeviceTime).Size = new Size(130, 32);
		((Control)cmdGetDeviceTime).TabIndex = 21;
		((Control)cmdGetDeviceTime).Text = "GetDeviceTime";
		((ButtonBase)cmdGetDeviceTime).UseVisualStyleBackColor = false;
		((Control)cmdGetDeviceTime).Click += cmdGetDeviceTime_Click;
		((Control)cmdGetDeviceInfo).BackColor = SystemColors.Control;
		((Control)cmdGetDeviceInfo).Cursor = Cursors.Default;
		((Control)cmdGetDeviceInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDeviceInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDeviceInfo).Location = new Point(13, 223);
		((Control)cmdGetDeviceInfo).Name = "cmdGetDeviceInfo";
		((Control)cmdGetDeviceInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDeviceInfo).Size = new Size(130, 32);
		((Control)cmdGetDeviceInfo).TabIndex = 26;
		((Control)cmdGetDeviceInfo).Text = "GetDeviceInfo";
		((ButtonBase)cmdGetDeviceInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetDeviceInfo).Click += cmdGetDeviceInfo_Click;
		((Control)cmdPowerOn).BackColor = SystemColors.Control;
		((Control)cmdPowerOn).Cursor = Cursors.Default;
		((Control)cmdPowerOn).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdPowerOn).ForeColor = SystemColors.ControlText;
		((Control)cmdPowerOn).Location = new Point(181, 71);
		((Control)cmdPowerOn).Name = "cmdPowerOn";
		((Control)cmdPowerOn).RightToLeft = (RightToLeft)0;
		((Control)cmdPowerOn).Size = new Size(130, 32);
		((Control)cmdPowerOn).TabIndex = 22;
		((Control)cmdPowerOn).Text = "PowerOnDevice";
		((ButtonBase)cmdPowerOn).UseVisualStyleBackColor = false;
		((Control)cmdPowerOn).Click += cmdPowerOn_Click;
		((Control)PowerOffDevice).BackColor = SystemColors.Control;
		((Control)PowerOffDevice).Cursor = Cursors.Default;
		((Control)PowerOffDevice).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)PowerOffDevice).ForeColor = SystemColors.ControlText;
		((Control)PowerOffDevice).Location = new Point(181, 111);
		((Control)PowerOffDevice).Name = "PowerOffDevice";
		((Control)PowerOffDevice).RightToLeft = (RightToLeft)0;
		((Control)PowerOffDevice).Size = new Size(130, 32);
		((Control)PowerOffDevice).TabIndex = 25;
		((Control)PowerOffDevice).Text = "PowerOffDevice";
		((ButtonBase)PowerOffDevice).UseVisualStyleBackColor = false;
		((Control)PowerOffDevice).Click += PowerOffDevice_Click;
		((Control)cmdSetDeviceInfo).BackColor = SystemColors.Control;
		((Control)cmdSetDeviceInfo).Cursor = Cursors.Default;
		((Control)cmdSetDeviceInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetDeviceInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdSetDeviceInfo).Location = new Point(181, 223);
		((Control)cmdSetDeviceInfo).Name = "cmdSetDeviceInfo";
		((Control)cmdSetDeviceInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdSetDeviceInfo).Size = new Size(130, 32);
		((Control)cmdSetDeviceInfo).TabIndex = 27;
		((Control)cmdSetDeviceInfo).Text = "SetDeviceInfo";
		((ButtonBase)cmdSetDeviceInfo).UseVisualStyleBackColor = false;
		((Control)cmdSetDeviceInfo).Click += cmdSetDeviceInfo_Click;
		((Control)cmbSatus).BackColor = SystemColors.Window;
		((Control)cmbSatus).Cursor = Cursors.Default;
		cmbSatus.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbSatus).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbSatus).ForeColor = SystemColors.WindowText;
		cmbSatus.Items.AddRange(new object[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
		((Control)cmbSatus).Location = new Point(121, 180);
		((Control)cmbSatus).Name = "cmbSatus";
		((Control)cmbSatus).RightToLeft = (RightToLeft)0;
		((Control)cmbSatus).Size = new Size(88, 27);
		((Control)cmbSatus).TabIndex = 32;
		txtSetDevInfo.AcceptsReturn = true;
		((Control)txtSetDevInfo).BackColor = SystemColors.Window;
		((Control)txtSetDevInfo).Cursor = Cursors.IBeam;
		((Control)txtSetDevInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtSetDevInfo).ForeColor = SystemColors.WindowText;
		((Control)txtSetDevInfo).Location = new Point(347, 180);
		((TextBoxBase)txtSetDevInfo).MaxLength = 0;
		((Control)txtSetDevInfo).Name = "txtSetDevInfo";
		((Control)txtSetDevInfo).RightToLeft = (RightToLeft)0;
		((Control)txtSetDevInfo).Size = new Size(98, 26);
		((Control)txtSetDevInfo).TabIndex = 29;
		((Control)cmdGetDeviceStaus).BackColor = SystemColors.Control;
		((Control)cmdGetDeviceStaus).Cursor = Cursors.Default;
		((Control)cmdGetDeviceStaus).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDeviceStaus).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDeviceStaus).Location = new Point(338, 223);
		((Control)cmdGetDeviceStaus).Name = "cmdGetDeviceStaus";
		((Control)cmdGetDeviceStaus).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDeviceStaus).Size = new Size(130, 32);
		((Control)cmdGetDeviceStaus).TabIndex = 28;
		((Control)cmdGetDeviceStaus).Text = "GetDeviceStatus";
		((ButtonBase)cmdGetDeviceStaus).UseVisualStyleBackColor = false;
		((Control)cmdGetDeviceStaus).Click += cmdGetDeviceStaus_Click;
		chkEnableDevice.Appearance = (Appearance)1;
		((Control)chkEnableDevice).BackColor = SystemColors.Control;
		((Control)chkEnableDevice).Cursor = Cursors.Default;
		((Control)chkEnableDevice).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkEnableDevice).ForeColor = SystemColors.ControlText;
		((Control)chkEnableDevice).Location = new Point(338, 71);
		((Control)chkEnableDevice).Name = "chkEnableDevice";
		((Control)chkEnableDevice).RightToLeft = (RightToLeft)0;
		((Control)chkEnableDevice).Size = new Size(130, 32);
		((Control)chkEnableDevice).TabIndex = 23;
		((Control)chkEnableDevice).Text = "DisableDevice";
		((ButtonBase)chkEnableDevice).TextAlign = (ContentAlignment)32;
		((ButtonBase)chkEnableDevice).UseVisualStyleBackColor = false;
		chkEnableDevice.CheckedChanged += chkEnableDevice_Click;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(242, 185);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(99, 23);
		((Control)Label1).TabIndex = 33;
		((Control)Label1).Text = "Status Value:";
		((Control)lblStatus).BackColor = SystemColors.Control;
		((Control)lblStatus).Cursor = Cursors.Default;
		((Control)lblStatus).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStatus).ForeColor = SystemColors.ControlText;
		((Control)lblStatus).Location = new Point(27, 185);
		((Control)lblStatus).Name = "lblStatus";
		((Control)lblStatus).RightToLeft = (RightToLeft)0;
		((Control)lblStatus).Size = new Size(88, 23);
		((Control)lblStatus).TabIndex = 31;
		((Control)lblStatus).Text = "Paramerter:";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(13, 15);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(463, 28);
		((Control)lblMessage).TabIndex = 30;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)cmdSetDeviceTime).BackColor = SystemColors.Control;
		((Control)cmdSetDeviceTime).Cursor = Cursors.Default;
		((Control)cmdSetDeviceTime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetDeviceTime).ForeColor = SystemColors.ControlText;
		((Control)cmdSetDeviceTime).Location = new Point(13, 111);
		((Control)cmdSetDeviceTime).Name = "cmdSetDeviceTime";
		((Control)cmdSetDeviceTime).RightToLeft = (RightToLeft)0;
		((Control)cmdSetDeviceTime).Size = new Size(130, 32);
		((Control)cmdSetDeviceTime).TabIndex = 24;
		((Control)cmdSetDeviceTime).Text = "SetDeviceTime";
		((ButtonBase)cmdSetDeviceTime).UseVisualStyleBackColor = false;
		((Control)cmdSetDeviceTime).Click += cmdSetDeviceTime_Click;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(489, 275);
		((Control)this).Controls.Add((Control)(object)cmdGetDeviceTime);
		((Control)this).Controls.Add((Control)(object)cmdGetDeviceInfo);
		((Control)this).Controls.Add((Control)(object)cmdPowerOn);
		((Control)this).Controls.Add((Control)(object)PowerOffDevice);
		((Control)this).Controls.Add((Control)(object)cmdSetDeviceInfo);
		((Control)this).Controls.Add((Control)(object)cmbSatus);
		((Control)this).Controls.Add((Control)(object)txtSetDevInfo);
		((Control)this).Controls.Add((Control)(object)cmdGetDeviceStaus);
		((Control)this).Controls.Add((Control)(object)chkEnableDevice);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblStatus);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)cmdSetDeviceTime);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmSystemInfo";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Manage System Info";
		((Form)this).Load += frmSystemInfo_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void frmSystemInfo_Load(object sender, EventArgs e)
	{
		((ListControl)cmbSatus).SelectedIndex = 0;
	}

	private void cmdGetDeviceTime_Click(object sender, EventArgs e)
	{
		DateTime apnDateTime = DateTime.Now;
		((Control)cmdGetDeviceTime).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetDeviceTime).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_GetDeviceTime(FKAttendDLL.nCommHandleIndex, ref apnDateTime);
		if (num == 1)
		{
			string text = "Date = " + apnDateTime.ToLongDateString() + ", Time = " + apnDateTime.ToLongTimeString();
			((Control)lblMessage).Text = text;
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetDeviceTime).Enabled = true;
	}

	private void cmdSetDeviceTime_Click(object sender, EventArgs e)
	{
		((Control)cmdSetDeviceTime).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetDeviceTime).Enabled = true;
			return;
		}
		DateTime now = DateTime.Now;
		num = FKAttendDLL.FK_SetDeviceTime(FKAttendDLL.nCommHandleIndex, now);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetDeviceTime).Enabled = true;
	}

	private void cmdPowerOn_Click(object sender, EventArgs e)
	{
		FKAttendDLL.FK_PowerOnAllDevice(FKAttendDLL.nCommHandleIndex);
	}

	private void PowerOffDevice_Click(object sender, EventArgs e)
	{
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_PowerOffDevice(FKAttendDLL.nCommHandleIndex);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
	}

	private void chkEnableDevice_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(anEnableFlag: ((int)chkEnableDevice.CheckState == 0) ? ((byte)1) : ((byte)0), anHandleIndex: FKAttendDLL.nCommHandleIndex);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		if ((int)chkEnableDevice.CheckState == 0)
		{
			((Control)chkEnableDevice).Text = "DisableDevice";
		}
		else
		{
			((Control)chkEnableDevice).Text = "EnableDevice";
		}
	}

	private void cmdGetDeviceInfo_Click(object sender, EventArgs e)
	{
		int apnValue = 0;
		((Control)cmdGetDeviceInfo).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		long num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetDeviceInfo).Enabled = true;
			return;
		}
		int num2 = ((ListControl)cmbSatus).SelectedIndex + 1;
		if (num2 == 11)
		{
			num2 = 24;
		}
		num = FKAttendDLL.FK_GetDeviceInfo(FKAttendDLL.nCommHandleIndex, num2, ref apnValue);
		if (num == 1)
		{
			switch (num2)
			{
			case 1:
				((Control)lblMessage).Text = "ManagerCount = " + apnValue;
				break;
			case 2:
				((Control)lblMessage).Text = "Machine Num = " + apnValue;
				break;
			case 3:
				((Control)lblMessage).Text = "Language = " + apnValue;
				break;
			case 4:
				((Control)lblMessage).Text = "PowerOffTime = " + apnValue;
				break;
			case 5:
				((Control)lblMessage).Text = "LockOperate = " + apnValue;
				break;
			case 6:
				((Control)lblMessage).Text = "GLogWarning = " + apnValue;
				break;
			case 7:
				((Control)lblMessage).Text = "SLogWarning = " + apnValue;
				break;
			case 8:
				((Control)lblMessage).Text = "ReVerifyTime = " + apnValue;
				break;
			case 9:
				((Control)lblMessage).Text = "Baudrate(" + apnValue + ") : ";
				switch (apnValue)
				{
				case 3:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "9600";
					break;
				case 4:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "19200";
					break;
				case 5:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "38400";
					break;
				case 6:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "57600";
					break;
				case 7:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "115200";
					break;
				default:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "--";
					break;
				}
				break;
			case 24:
				((Control)lblMessage).Text = "VerifyKind = ";
				switch (apnValue)
				{
				case 0:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "F / P / C";
					break;
				case 1:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "F + P";
					break;
				case 2:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "F + C";
					break;
				case 3:
					((Control)lblMessage).Text = ((Control)lblMessage).Text + "C";
					break;
				}
				break;
			case 10:
				((Control)lblMessage).Text = "DateSeperate = " + apnValue;
				break;
			default:
				((Control)lblMessage).Text = "--";
				break;
			}
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetDeviceInfo).Enabled = true;
	}

	private void cmdSetDeviceInfo_Click(object sender, EventArgs e)
	{
		int anValue = 0;
		((Control)cmdSetDeviceInfo).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if ((long)num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetDeviceInfo).Enabled = true;
			return;
		}
		int num2 = ((ListControl)cmbSatus).SelectedIndex + 1;
		if (FKAttendDLL.IsNumeric(((Control)txtSetDevInfo).Text))
		{
			anValue = Convert.ToInt32(((Control)txtSetDevInfo).Text);
		}
		if (num2 == 11)
		{
			num2 = 24;
		}
		num = FKAttendDLL.FK_SetDeviceInfo(FKAttendDLL.nCommHandleIndex, num2, anValue);
		if (num2 == 2 && (long)num == 1)
		{
			((Control)FrmConnectDeviceHysoon.GfrmMain.TxtMachineNumber).Text = anValue.ToString();
		}
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetDeviceInfo).Enabled = true;
	}

	private void cmdGetDeviceStaus_Click(object sender, EventArgs e)
	{
		int apnValue = 0;
		((Control)cmdGetDeviceStaus).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetDeviceStaus).Enabled = true;
			return;
		}
		int num2 = ((ListControl)cmbSatus).SelectedIndex + 1;
		num = FKAttendDLL.FK_GetDeviceStatus(FKAttendDLL.nCommHandleIndex, num2, ref apnValue);
		if (num == 1)
		{
			switch (num2)
			{
			case 1:
				((Control)lblMessage).Text = "Manager count = " + apnValue;
				break;
			case 2:
				((Control)lblMessage).Text = "User count = " + apnValue;
				break;
			case 3:
				((Control)lblMessage).Text = "Fp count = " + apnValue;
				break;
			case 4:
				((Control)lblMessage).Text = "Password count = " + apnValue;
				break;
			case 5:
				((Control)lblMessage).Text = "SLog count = " + apnValue;
				break;
			case 6:
				((Control)lblMessage).Text = "GLog count = " + apnValue;
				break;
			case 7:
				((Control)lblMessage).Text = "All SLog count = " + apnValue;
				break;
			case 8:
				((Control)lblMessage).Text = "All GLog count = " + apnValue;
				break;
			case 9:
				((Control)lblMessage).Text = "Card count = " + apnValue;
				break;
			default:
				((Control)lblMessage).Text = "--";
				break;
			}
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetDeviceStaus).Enabled = true;
	}
}
