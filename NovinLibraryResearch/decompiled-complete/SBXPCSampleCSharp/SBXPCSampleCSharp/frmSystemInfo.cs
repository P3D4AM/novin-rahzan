using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmSystemInfo : Form
{
	private AxSBXPC bpc;

	private IContainer components = null;

	public Button cmdSetDeviceTime;

	public Button cmdGetDeviceTime;

	public Button cmdGetDeviceInfo;

	public Button cmdExit;

	public Button cmdPowerOn;

	public Button PowerOffDevice;

	public Button cmdSetDeviceInfo;

	public Button cmdEnableDevice;

	public ComboBox cmbSatus;

	public TextBox txtSetDevInfo;

	public Button cmdGetDeviceStaus;

	public CheckBox chkEnableDevice;

	public Label Label1;

	public Label lblStatus;

	public Label lblMessage;

	public frmSystemInfo(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private string GetWeekDay(int anDay)
	{
		return anDay switch
		{
			1 => "Sunday", 
			2 => "Monday", 
			3 => "Tuesday", 
			4 => "Wednesday", 
			5 => "Thursday", 
			6 => "Friday", 
			7 => "Saturday", 
			_ => "Sunday", 
		};
	}

	private void cmdGetDeviceTime_Click(object sender, EventArgs e)
	{
		int dwYear = 0;
		int dwMonth = 0;
		int dwDay = 0;
		int dwHour = 0;
		int dwMinute = 0;
		int dwSecond = 0;
		int dwDayOfWeek = 0;
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.GetDeviceTime(util.gMachineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond, ref dwDayOfWeek))
		{
			if (dwDayOfWeek == 0)
			{
				dwDayOfWeek = 7;
			}
			string text = "Date = " + Convert.ToString(dwYear) + "/" + Convert.ToString(dwMonth) + "/" + Convert.ToString(dwDay) + " , " + GetWeekDay(dwDayOfWeek) + " , Time = " + Convert.ToString(dwHour) + ":" + Convert.ToString(dwMinute) + ":" + Convert.ToString(dwSecond);
			((Control)lblMessage).Text = text;
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdSetDeviceTime_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDeviceTime(util.gMachineNumber))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
		Application.DoEvents();
	}

	private void cmdPowerOn_Click(object sender, EventArgs e)
	{
		bpc.PowerOnAllDevice();
	}

	private void PowerOffDevice_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (bpc.PowerOffDevice(util.gMachineNumber))
		{
			((Control)lblMessage).Text = "Success!";
			return;
		}
		bpc.GetLastError(ref dwErrorCode);
		((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
	}

	private void cmdEnableDevice_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = (chkEnableDevice.Checked ? 1 : 0);
		if (bpc.EnableDevice(util.gMachineNumber, num))
		{
			if (num == 1)
			{
				((Control)lblMessage).Text = "Enable Device Success!";
				((Control)cmdEnableDevice).Text = "DisableDevice";
			}
			else
			{
				((Control)lblMessage).Text = "Disable Device Success!";
				((Control)cmdEnableDevice).Text = "EnableDevice";
			}
			chkEnableDevice.Checked = !chkEnableDevice.Checked;
			Application.DoEvents();
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void cmdGetDeviceInfo_Click(object sender, EventArgs e)
	{
		int dwValue = 0;
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = ((ListControl)cmbSatus).SelectedIndex + 1;
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.GetDeviceInfo(util.gMachineNumber, num, ref dwValue))
		{
			switch (num)
			{
			case 1:
				((Control)lblMessage).Text = "(1) = ManagerCount = " + dwValue;
				break;
			case 2:
				((Control)lblMessage).Text = "(2) = Device ID = " + dwValue;
				break;
			case 3:
				((Control)lblMessage).Text = "(3) = Language = " + dwValue;
				break;
			case 4:
				((Control)lblMessage).Text = "(4) = PowerOffTime = " + dwValue;
				break;
			case 5:
				((Control)lblMessage).Text = "(5) = Lock release time = " + dwValue;
				break;
			case 6:
				((Control)lblMessage).Text = "(6) = GLogWarning = " + dwValue;
				break;
			case 7:
				((Control)lblMessage).Text = "(7) = SLogWarning = " + dwValue;
				break;
			case 8:
				((Control)lblMessage).Text = "(8) = ReVerifyTime = " + dwValue;
				break;
			case 9:
				((Control)lblMessage).Text = "(9) = Baudrate = " + dwValue;
				break;
			case 10:
				((Control)lblMessage).Text = "(10) = Parity check = " + dwValue;
				break;
			case 11:
				((Control)lblMessage).Text = "(11) = Stop bit = " + dwValue;
				break;
			case 12:
				((Control)lblMessage).Text = "(12) = Date Seperator = " + dwValue;
				break;
			case 13:
				((Control)lblMessage).Text = "(13) = Identificatin mode = " + dwValue;
				break;
			case 14:
				((Control)lblMessage).Text = "(14) = LockOperate = " + dwValue;
				break;
			case 15:
				((Control)lblMessage).Text = "(15) = Door sensor type = " + dwValue;
				break;
			case 16:
				((Control)lblMessage).Text = "(16) = Door open time limit = " + dwValue;
				break;
			case 17:
				((Control)lblMessage).Text = "(17) = Anti-pass = " + dwValue;
				break;
			case 18:
				((Control)lblMessage).Text = "(18) = Auto sleep time = " + dwValue;
				break;
			case 19:
				((Control)lblMessage).Text = "(19) = Daylight offset = " + dwValue;
				break;
			case 20:
				((Control)lblMessage).Text = "(20) = UDP Server = " + util.pubLongToIPAddr(dwValue);
				break;
			case 21:
				((Control)lblMessage).Text = "(21) = DHCP Use = " + dwValue;
				break;
			case 22:
				((Control)lblMessage).Text = "(22) = Main Lock Group = " + dwValue;
				break;
			}
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdSetDeviceInfo_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = ((ListControl)cmbSatus).SelectedIndex + 1;
		int num2 = ((num == 20) ? util.pubIPAddrToLong((((Control)txtSetDevInfo).Text == "") ? "0" : ((Control)txtSetDevInfo).Text) : Convert.ToInt32((((Control)txtSetDevInfo).Text == "") ? "0" : ((Control)txtSetDevInfo).Text));
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.SetDeviceInfo(util.gMachineNumber, num, num2))
		{
			((Control)lblMessage).Text = "Success!";
			if (num == 2)
			{
				util.gMachineNumber = num2;
				util.Sleep(1000);
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

	private void cmdGetDeviceStaus_Click(object sender, EventArgs e)
	{
		int dwValue = 0;
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = ((ListControl)cmbSatus).SelectedIndex + 1;
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.GetDeviceStatus(util.gMachineNumber, num, ref dwValue))
		{
			switch (num)
			{
			case 1:
				((Control)lblMessage).Text = "(1) = Manager count = " + dwValue;
				break;
			case 2:
				((Control)lblMessage).Text = "(2) = User count = " + dwValue;
				break;
			case 3:
				((Control)lblMessage).Text = "(3) = Fp count = " + dwValue;
				break;
			case 4:
				((Control)lblMessage).Text = "(4) = Password count = " + dwValue;
				break;
			case 5:
				((Control)lblMessage).Text = "(5) = SLog count = " + dwValue;
				break;
			case 6:
				((Control)lblMessage).Text = "(6) = GLog count = " + dwValue;
				break;
			case 7:
				((Control)lblMessage).Text = "(7) = Card count = " + dwValue;
				break;
			case 8:
				((Control)lblMessage).Text = "(8) = Alarm status = " + dwValue;
				break;
			case 9:
				((Control)lblMessage).Text = "(9) = Face Count = " + dwValue;
				break;
			case 10:
				((Control)lblMessage).Text = "(10) = SLog unread count = " + dwValue;
				break;
			case 11:
				((Control)lblMessage).Text = "(11) = GLog unread count = " + dwValue;
				break;
			}
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void frmSystemInfo_Load(object sender, EventArgs e)
	{
		((ListControl)cmbSatus).SelectedIndex = 0;
	}

	private void frmSystemInfo_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f6: Expected O, but got Unknown
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Expected O, but got Unknown
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Expected O, but got Unknown
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b37: Expected O, but got Unknown
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bee: Expected O, but got Unknown
		//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cac: Expected O, but got Unknown
		//IL_0d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Expected O, but got Unknown
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Expected O, but got Unknown
		cmdSetDeviceTime = new Button();
		cmdGetDeviceTime = new Button();
		cmdGetDeviceInfo = new Button();
		cmdExit = new Button();
		cmdPowerOn = new Button();
		PowerOffDevice = new Button();
		cmdSetDeviceInfo = new Button();
		cmdEnableDevice = new Button();
		cmbSatus = new ComboBox();
		txtSetDevInfo = new TextBox();
		cmdGetDeviceStaus = new Button();
		chkEnableDevice = new CheckBox();
		Label1 = new Label();
		lblStatus = new Label();
		lblMessage = new Label();
		((Control)this).SuspendLayout();
		((Control)cmdSetDeviceTime).BackColor = SystemColors.Control;
		((Control)cmdSetDeviceTime).Cursor = Cursors.Default;
		((Control)cmdSetDeviceTime).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetDeviceTime).ForeColor = SystemColors.ControlText;
		((Control)cmdSetDeviceTime).Location = new Point(10, 120);
		((Control)cmdSetDeviceTime).Name = "cmdSetDeviceTime";
		((Control)cmdSetDeviceTime).RightToLeft = (RightToLeft)0;
		((Control)cmdSetDeviceTime).Size = new Size(125, 32);
		((Control)cmdSetDeviceTime).TabIndex = 29;
		((Control)cmdSetDeviceTime).Text = "SetDeviceTime";
		((ButtonBase)cmdSetDeviceTime).UseVisualStyleBackColor = false;
		((Control)cmdSetDeviceTime).Click += cmdSetDeviceTime_Click;
		((Control)cmdGetDeviceTime).BackColor = SystemColors.Control;
		((Control)cmdGetDeviceTime).Cursor = Cursors.Default;
		((Control)cmdGetDeviceTime).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDeviceTime).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDeviceTime).Location = new Point(10, 78);
		((Control)cmdGetDeviceTime).Name = "cmdGetDeviceTime";
		((Control)cmdGetDeviceTime).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDeviceTime).Size = new Size(125, 32);
		((Control)cmdGetDeviceTime).TabIndex = 28;
		((Control)cmdGetDeviceTime).Text = "GetDeviceTime";
		((ButtonBase)cmdGetDeviceTime).UseVisualStyleBackColor = false;
		((Control)cmdGetDeviceTime).Click += cmdGetDeviceTime_Click;
		((Control)cmdGetDeviceInfo).BackColor = SystemColors.Control;
		((Control)cmdGetDeviceInfo).Cursor = Cursors.Default;
		((Control)cmdGetDeviceInfo).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDeviceInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDeviceInfo).Location = new Point(10, 221);
		((Control)cmdGetDeviceInfo).Name = "cmdGetDeviceInfo";
		((Control)cmdGetDeviceInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDeviceInfo).Size = new Size(125, 32);
		((Control)cmdGetDeviceInfo).TabIndex = 27;
		((Control)cmdGetDeviceInfo).Text = "GetDeviceInfo";
		((ButtonBase)cmdGetDeviceInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetDeviceInfo).Click += cmdGetDeviceInfo_Click;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(301, 120);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(125, 32);
		((Control)cmdExit).TabIndex = 26;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdPowerOn).BackColor = SystemColors.Control;
		((Control)cmdPowerOn).Cursor = Cursors.Default;
		((Control)cmdPowerOn).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdPowerOn).ForeColor = SystemColors.ControlText;
		((Control)cmdPowerOn).Location = new Point(152, 78);
		((Control)cmdPowerOn).Name = "cmdPowerOn";
		((Control)cmdPowerOn).RightToLeft = (RightToLeft)0;
		((Control)cmdPowerOn).Size = new Size(125, 32);
		((Control)cmdPowerOn).TabIndex = 25;
		((Control)cmdPowerOn).Text = "PowerOnDevice";
		((ButtonBase)cmdPowerOn).UseVisualStyleBackColor = false;
		((Control)cmdPowerOn).Click += cmdPowerOn_Click;
		((Control)PowerOffDevice).BackColor = SystemColors.Control;
		((Control)PowerOffDevice).Cursor = Cursors.Default;
		((Control)PowerOffDevice).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)PowerOffDevice).ForeColor = SystemColors.ControlText;
		((Control)PowerOffDevice).Location = new Point(152, 120);
		((Control)PowerOffDevice).Name = "PowerOffDevice";
		((Control)PowerOffDevice).RightToLeft = (RightToLeft)0;
		((Control)PowerOffDevice).Size = new Size(125, 32);
		((Control)PowerOffDevice).TabIndex = 24;
		((Control)PowerOffDevice).Text = "PowerOffDevice";
		((ButtonBase)PowerOffDevice).UseVisualStyleBackColor = false;
		((Control)PowerOffDevice).Click += PowerOffDevice_Click;
		((Control)cmdSetDeviceInfo).BackColor = SystemColors.Control;
		((Control)cmdSetDeviceInfo).Cursor = Cursors.Default;
		((Control)cmdSetDeviceInfo).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetDeviceInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdSetDeviceInfo).Location = new Point(152, 221);
		((Control)cmdSetDeviceInfo).Name = "cmdSetDeviceInfo";
		((Control)cmdSetDeviceInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdSetDeviceInfo).Size = new Size(125, 32);
		((Control)cmdSetDeviceInfo).TabIndex = 23;
		((Control)cmdSetDeviceInfo).Text = "SetDeviceInfo";
		((ButtonBase)cmdSetDeviceInfo).UseVisualStyleBackColor = false;
		((Control)cmdSetDeviceInfo).Click += cmdSetDeviceInfo_Click;
		((Control)cmdEnableDevice).BackColor = SystemColors.Control;
		((Control)cmdEnableDevice).Cursor = Cursors.Default;
		((Control)cmdEnableDevice).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdEnableDevice).ForeColor = SystemColors.ControlText;
		((Control)cmdEnableDevice).Location = new Point(301, 79);
		((Control)cmdEnableDevice).Name = "cmdEnableDevice";
		((Control)cmdEnableDevice).RightToLeft = (RightToLeft)0;
		((Control)cmdEnableDevice).Size = new Size(125, 32);
		((Control)cmdEnableDevice).TabIndex = 22;
		((Control)cmdEnableDevice).Text = "DisableDevice";
		((ButtonBase)cmdEnableDevice).UseVisualStyleBackColor = false;
		((Control)cmdEnableDevice).Click += cmdEnableDevice_Click;
		((Control)cmbSatus).BackColor = SystemColors.Window;
		((Control)cmbSatus).Cursor = Cursors.Default;
		cmbSatus.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbSatus).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbSatus).ForeColor = SystemColors.WindowText;
		cmbSatus.Items.AddRange(new object[24]
		{
			" 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9", "10",
			"11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
			"21", "22", "23", "24"
		});
		((Control)cmbSatus).Location = new Point(146, 176);
		((Control)cmbSatus).Name = "cmbSatus";
		((Control)cmbSatus).RightToLeft = (RightToLeft)0;
		((Control)cmbSatus).Size = new Size(88, 27);
		((Control)cmbSatus).TabIndex = 20;
		txtSetDevInfo.AcceptsReturn = true;
		((Control)txtSetDevInfo).BackColor = SystemColors.Window;
		((Control)txtSetDevInfo).Cursor = Cursors.IBeam;
		((Control)txtSetDevInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtSetDevInfo).ForeColor = SystemColors.WindowText;
		((Control)txtSetDevInfo).Location = new Point(358, 173);
		((TextBoxBase)txtSetDevInfo).MaxLength = 0;
		((Control)txtSetDevInfo).Name = "txtSetDevInfo";
		((Control)txtSetDevInfo).RightToLeft = (RightToLeft)0;
		((Control)txtSetDevInfo).Size = new Size(59, 26);
		((Control)txtSetDevInfo).TabIndex = 19;
		((Control)cmdGetDeviceStaus).BackColor = SystemColors.Control;
		((Control)cmdGetDeviceStaus).Cursor = Cursors.Default;
		((Control)cmdGetDeviceStaus).Font = new Font("Times New Roman", 11f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDeviceStaus).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDeviceStaus).Location = new Point(284, 221);
		((Control)cmdGetDeviceStaus).Name = "cmdGetDeviceStaus";
		((Control)cmdGetDeviceStaus).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDeviceStaus).Size = new Size(142, 32);
		((Control)cmdGetDeviceStaus).TabIndex = 17;
		((Control)cmdGetDeviceStaus).Text = "GetDeviceStatus";
		((ButtonBase)cmdGetDeviceStaus).UseVisualStyleBackColor = false;
		((Control)cmdGetDeviceStaus).Click += cmdGetDeviceStaus_Click;
		((Control)chkEnableDevice).BackColor = SystemColors.Control;
		((Control)chkEnableDevice).Cursor = Cursors.Default;
		((Control)chkEnableDevice).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)chkEnableDevice).ForeColor = SystemColors.ControlText;
		((Control)chkEnableDevice).Location = new Point(284, 84);
		((Control)chkEnableDevice).Name = "chkEnableDevice";
		((Control)chkEnableDevice).RightToLeft = (RightToLeft)0;
		((Control)chkEnableDevice).Size = new Size(15, 23);
		((Control)chkEnableDevice).TabIndex = 16;
		((ButtonBase)chkEnableDevice).UseVisualStyleBackColor = false;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(260, 178);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(97, 23);
		((Control)Label1).TabIndex = 21;
		((Control)Label1).Text = "Status Value:";
		((Control)lblStatus).BackColor = SystemColors.Control;
		((Control)lblStatus).Cursor = Cursors.Default;
		((Control)lblStatus).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStatus).ForeColor = SystemColors.ControlText;
		((Control)lblStatus).Location = new Point(18, 169);
		((Control)lblStatus).Name = "lblStatus";
		((Control)lblStatus).RightToLeft = (RightToLeft)0;
		((Control)lblStatus).Size = new Size(137, 41);
		((Control)lblStatus).TabIndex = 18;
		((Control)lblStatus).Text = "Status Paramerter:  Info Paramerter:";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(13, 27);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(414, 28);
		((Control)lblMessage).TabIndex = 15;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(450, 273);
		((Control)this).Controls.Add((Control)(object)cmdSetDeviceTime);
		((Control)this).Controls.Add((Control)(object)cmdGetDeviceTime);
		((Control)this).Controls.Add((Control)(object)cmdGetDeviceInfo);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdPowerOn);
		((Control)this).Controls.Add((Control)(object)PowerOffDevice);
		((Control)this).Controls.Add((Control)(object)cmdSetDeviceInfo);
		((Control)this).Controls.Add((Control)(object)cmdEnableDevice);
		((Control)this).Controls.Add((Control)(object)cmbSatus);
		((Control)this).Controls.Add((Control)(object)txtSetDevInfo);
		((Control)this).Controls.Add((Control)(object)cmdGetDeviceStaus);
		((Control)this).Controls.Add((Control)(object)chkEnableDevice);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblStatus);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmSystemInfo";
		((Control)this).Text = "frmSystemInfo";
		((Form)this).Load += frmSystemInfo_Load;
		((Form)this).FormClosed += new FormClosedEventHandler(frmSystemInfo_FormClosed);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
