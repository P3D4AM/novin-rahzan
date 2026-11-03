using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using RegisterManagement;

namespace FKAttendDllCSSample;

public class FrmConnectDeviceHysoon : Form
{
	public Button CmdBellInfo;

	public RadioButton OptSerialDevice;

	public RadioButton OptNetworkDevice;

	public Button CmdCloseComm;

	public Button CmdProuctData;

	public Button CmdLogData;

	public GroupBox Frame3;

	internal Label LblComTimeOutT;

	internal TextBox TxtComTimeOut;

	internal Label LblComTimeOut;

	public TextBox TxtTelNumber;

	public TextBox TxtWaitDialTime;

	public CheckBox ChkUsingModem;

	public Label LblTelphon;

	public Label LblWaitTime;

	public Label LblS;

	public ComboBox CmbCommBaudRate;

	public ComboBox CmbComPort;

	public Label LblComBaudRate;

	public Label LblComPort;

	public TextBox TxtMachineNumber;

	public GroupBox Frame2;

	internal Button CmdGetIp;

	public TextBox TxtDeviceName;

	public Label LblDeviceName;

	internal CheckBox ChkUdpFlag;

	public Label LblTimeOutT;

	public TextBox TxtTimeOut;

	public Label LblTimeOut;

	public TextBox TxtPassword;

	public TextBox TxtPortNo;

	public TextBox TxtIpAddress;

	public Label LblPassword;

	public Label LblPortNo;

	public Label LblIpAddress;

	public Label LblComLicense;

	public RadioButton OptUsbDevice;

	public Button CmdEnrollData;

	public Button CmdSystemInfo;

	public Button CmdOpenComm;

	public TextBox TxtLicense;

	public Label LblMachineNumber;

	public Label LblVer;

	public Label LbSubject;

	public Button CmdSetPostShiftInfo;

	public Button CmdPhotoManage;

	public Button CmdSetUserName;

	public Button CmdPassTimeHs;

	private Container _components = null;

	public static FrmConnectDeviceHysoon GfrmMain;

	public bool OpenFlag;

	public FrmConnectDeviceHysoon()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_components?.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Expected O, but got Unknown
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Expected O, but got Unknown
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Expected O, but got Unknown
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Expected O, but got Unknown
		//IL_0c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c39: Expected O, but got Unknown
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d04: Expected O, but got Unknown
		//IL_0dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de1: Expected O, but got Unknown
		//IL_0e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9c: Expected O, but got Unknown
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Expected O, but got Unknown
		//IL_101a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Expected O, but got Unknown
		//IL_110d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Expected O, but got Unknown
		//IL_11f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1202: Expected O, but got Unknown
		//IL_12cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d5: Expected O, but got Unknown
		//IL_139e: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a8: Expected O, but got Unknown
		//IL_16c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cf: Expected O, but got Unknown
		//IL_178a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1794: Expected O, but got Unknown
		//IL_182e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1838: Expected O, but got Unknown
		//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e2: Expected O, but got Unknown
		//IL_19b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bb: Expected O, but got Unknown
		//IL_1a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a83: Expected O, but got Unknown
		//IL_1b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b59: Expected O, but got Unknown
		//IL_1c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2e: Expected O, but got Unknown
		//IL_1cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d00: Expected O, but got Unknown
		//IL_1dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc5: Expected O, but got Unknown
		//IL_1e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8e: Expected O, but got Unknown
		//IL_1f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f54: Expected O, but got Unknown
		//IL_201d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2027: Expected O, but got Unknown
		//IL_20d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e2: Expected O, but got Unknown
		//IL_21c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d2: Expected O, but got Unknown
		//IL_22ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b8: Expected O, but got Unknown
		//IL_2394: Unknown result type (might be due to invalid IL or missing references)
		//IL_239e: Expected O, but got Unknown
		//IL_2484: Unknown result type (might be due to invalid IL or missing references)
		//IL_248e: Expected O, but got Unknown
		//IL_2566: Unknown result type (might be due to invalid IL or missing references)
		//IL_2570: Expected O, but got Unknown
		//IL_261e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2628: Expected O, but got Unknown
		//IL_26ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f6: Expected O, but got Unknown
		//IL_27b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27bd: Expected O, but got Unknown
		//IL_2899: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a3: Expected O, but got Unknown
		//IL_297f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2989: Expected O, but got Unknown
		//IL_2a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a6f: Expected O, but got Unknown
		//IL_2cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cdc: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmConnectDeviceHysoon));
		CmdBellInfo = new Button();
		OptSerialDevice = new RadioButton();
		OptNetworkDevice = new RadioButton();
		CmdCloseComm = new Button();
		CmdProuctData = new Button();
		CmdLogData = new Button();
		Frame3 = new GroupBox();
		LblComTimeOutT = new Label();
		TxtComTimeOut = new TextBox();
		LblComTimeOut = new Label();
		TxtTelNumber = new TextBox();
		TxtWaitDialTime = new TextBox();
		ChkUsingModem = new CheckBox();
		LblTelphon = new Label();
		LblWaitTime = new Label();
		LblS = new Label();
		CmbCommBaudRate = new ComboBox();
		CmbComPort = new ComboBox();
		LblComBaudRate = new Label();
		LblComPort = new Label();
		TxtMachineNumber = new TextBox();
		Frame2 = new GroupBox();
		CmdGetIp = new Button();
		TxtDeviceName = new TextBox();
		LblDeviceName = new Label();
		ChkUdpFlag = new CheckBox();
		LblTimeOutT = new Label();
		TxtTimeOut = new TextBox();
		LblTimeOut = new Label();
		TxtPassword = new TextBox();
		TxtPortNo = new TextBox();
		TxtIpAddress = new TextBox();
		LblPassword = new Label();
		LblPortNo = new Label();
		LblIpAddress = new Label();
		LblComLicense = new Label();
		OptUsbDevice = new RadioButton();
		CmdEnrollData = new Button();
		CmdSystemInfo = new Button();
		CmdOpenComm = new Button();
		TxtLicense = new TextBox();
		LblMachineNumber = new Label();
		LblVer = new Label();
		LbSubject = new Label();
		CmdSetPostShiftInfo = new Button();
		CmdPhotoManage = new Button();
		CmdSetUserName = new Button();
		CmdPassTimeHs = new Button();
		((Control)Frame3).SuspendLayout();
		((Control)Frame2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)CmdBellInfo).BackColor = SystemColors.Control;
		((Control)CmdBellInfo).Cursor = Cursors.Default;
		((Control)CmdBellInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdBellInfo).ForeColor = SystemColors.ControlText;
		((Control)CmdBellInfo).Location = new Point(284, 427);
		((Control)CmdBellInfo).Name = "CmdBellInfo";
		((Control)CmdBellInfo).RightToLeft = (RightToLeft)0;
		((Control)CmdBellInfo).Size = new Size(228, 38);
		((Control)CmdBellInfo).TabIndex = 79;
		((Control)CmdBellInfo).Text = "Bell Time";
		((ButtonBase)CmdBellInfo).UseVisualStyleBackColor = false;
		((Control)CmdBellInfo).Click += cmdBellInfo_Click;
		((Control)OptSerialDevice).BackColor = Color.FromArgb(192, 192, 255);
		((Control)OptSerialDevice).Cursor = Cursors.Default;
		((Control)OptSerialDevice).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)OptSerialDevice).ForeColor = SystemColors.ControlText;
		((Control)OptSerialDevice).Location = new Point(10, -1);
		((Control)OptSerialDevice).Name = "OptSerialDevice";
		((Control)OptSerialDevice).RightToLeft = (RightToLeft)0;
		((Control)OptSerialDevice).Size = new Size(148, 19);
		((Control)OptSerialDevice).TabIndex = 85;
		OptSerialDevice.TabStop = true;
		((Control)OptSerialDevice).Text = "Serial Device";
		((ButtonBase)OptSerialDevice).UseVisualStyleBackColor = false;
		OptSerialDevice.CheckedChanged += optSerialDevice_CheckedChanged;
		((Control)OptNetworkDevice).BackColor = Color.FromArgb(192, 192, 255);
		((Control)OptNetworkDevice).Cursor = Cursors.Default;
		((Control)OptNetworkDevice).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)OptNetworkDevice).ForeColor = SystemColors.ControlText;
		((Control)OptNetworkDevice).Location = new Point(14, -2);
		((Control)OptNetworkDevice).Name = "OptNetworkDevice";
		((Control)OptNetworkDevice).RightToLeft = (RightToLeft)0;
		((Control)OptNetworkDevice).Size = new Size(148, 23);
		((Control)OptNetworkDevice).TabIndex = 83;
		OptNetworkDevice.TabStop = true;
		((Control)OptNetworkDevice).Text = "Network Device";
		((ButtonBase)OptNetworkDevice).UseVisualStyleBackColor = false;
		OptNetworkDevice.CheckedChanged += optNetworkDevice_CheckedChanged;
		((Control)CmdCloseComm).BackColor = SystemColors.Control;
		((Control)CmdCloseComm).Cursor = Cursors.Default;
		((Control)CmdCloseComm).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdCloseComm).ForeColor = SystemColors.ControlText;
		((Control)CmdCloseComm).Location = new Point(402, 138);
		((Control)CmdCloseComm).Name = "CmdCloseComm";
		((Control)CmdCloseComm).RightToLeft = (RightToLeft)0;
		((Control)CmdCloseComm).Size = new Size(110, 37);
		((Control)CmdCloseComm).TabIndex = 82;
		((Control)CmdCloseComm).Text = "Close Comm";
		((ButtonBase)CmdCloseComm).UseVisualStyleBackColor = false;
		((Control)CmdCloseComm).Click += cmdCloseComm_Click;
		((Control)CmdProuctData).BackColor = SystemColors.Control;
		((Control)CmdProuctData).Cursor = Cursors.Default;
		((Control)CmdProuctData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdProuctData).ForeColor = SystemColors.ControlText;
		((Control)CmdProuctData).Location = new Point(284, 333);
		((Control)CmdProuctData).Name = "CmdProuctData";
		((Control)CmdProuctData).RightToLeft = (RightToLeft)0;
		((Control)CmdProuctData).Size = new Size(228, 36);
		((Control)CmdProuctData).TabIndex = 78;
		((Control)CmdProuctData).Text = "Get Product Data";
		((ButtonBase)CmdProuctData).UseVisualStyleBackColor = false;
		((Control)CmdProuctData).Click += cmdProuctData_Click;
		((Control)CmdLogData).BackColor = SystemColors.Control;
		((Control)CmdLogData).Cursor = Cursors.Default;
		((Control)CmdLogData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdLogData).ForeColor = SystemColors.ControlText;
		((Control)CmdLogData).Location = new Point(284, 237);
		((Control)CmdLogData).Name = "CmdLogData";
		((Control)CmdLogData).RightToLeft = (RightToLeft)0;
		((Control)CmdLogData).Size = new Size(228, 37);
		((Control)CmdLogData).TabIndex = 76;
		((Control)CmdLogData).Text = "Log Data Management";
		((ButtonBase)CmdLogData).UseVisualStyleBackColor = false;
		((Control)CmdLogData).Click += cmdLogData_Click;
		((Control)Frame3).BackColor = Color.Transparent;
		((Control)Frame3).Controls.Add((Control)(object)OptSerialDevice);
		((Control)Frame3).Controls.Add((Control)(object)LblComTimeOutT);
		((Control)Frame3).Controls.Add((Control)(object)TxtComTimeOut);
		((Control)Frame3).Controls.Add((Control)(object)LblComTimeOut);
		((Control)Frame3).Controls.Add((Control)(object)TxtTelNumber);
		((Control)Frame3).Controls.Add((Control)(object)TxtWaitDialTime);
		((Control)Frame3).Controls.Add((Control)(object)ChkUsingModem);
		((Control)Frame3).Controls.Add((Control)(object)LblTelphon);
		((Control)Frame3).Controls.Add((Control)(object)LblWaitTime);
		((Control)Frame3).Controls.Add((Control)(object)LblS);
		((Control)Frame3).Controls.Add((Control)(object)CmbCommBaudRate);
		((Control)Frame3).Controls.Add((Control)(object)CmbComPort);
		((Control)Frame3).Controls.Add((Control)(object)LblComBaudRate);
		((Control)Frame3).Controls.Add((Control)(object)LblComPort);
		((Control)Frame3).ForeColor = SystemColors.ControlText;
		((Control)Frame3).Location = new Point(18, 171);
		((Control)Frame3).Name = "Frame3";
		((Control)Frame3).RightToLeft = (RightToLeft)0;
		((Control)Frame3).Size = new Size(260, 209);
		((Control)Frame3).TabIndex = 86;
		Frame3.TabStop = false;
		((Control)LblComTimeOutT).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblComTimeOutT).Location = new Point(184, 88);
		((Control)LblComTimeOutT).Name = "LblComTimeOutT";
		((Control)LblComTimeOutT).Size = new Size(40, 24);
		((Control)LblComTimeOutT).TabIndex = 49;
		((Control)LblComTimeOutT).Text = "(ms)";
		LblComTimeOutT.TextAlign = (ContentAlignment)32;
		((Control)TxtComTimeOut).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtComTimeOut).Location = new Point(117, 88);
		((Control)TxtComTimeOut).Name = "TxtComTimeOut";
		((Control)TxtComTimeOut).Size = new Size(59, 26);
		((Control)TxtComTimeOut).TabIndex = 48;
		((Control)TxtComTimeOut).Text = "3000";
		((Control)LblComTimeOut).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblComTimeOut).Location = new Point(21, 88);
		((Control)LblComTimeOut).Name = "LblComTimeOut";
		((Control)LblComTimeOut).Size = new Size(88, 24);
		((Control)LblComTimeOut).TabIndex = 47;
		((Control)LblComTimeOut).Text = "TimeOut :";
		TxtTelNumber.AcceptsReturn = true;
		((Control)TxtTelNumber).BackColor = SystemColors.Window;
		((Control)TxtTelNumber).Cursor = Cursors.IBeam;
		((Control)TxtTelNumber).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtTelNumber).ForeColor = SystemColors.WindowText;
		((Control)TxtTelNumber).Location = new Point(180, 141);
		((TextBoxBase)TxtTelNumber).MaxLength = 0;
		((Control)TxtTelNumber).Name = "TxtTelNumber";
		((Control)TxtTelNumber).RightToLeft = (RightToLeft)0;
		((Control)TxtTelNumber).Size = new Size(62, 26);
		((Control)TxtTelNumber).TabIndex = 45;
		((Control)TxtTelNumber).Text = "801";
		TxtWaitDialTime.AcceptsReturn = true;
		((Control)TxtWaitDialTime).BackColor = SystemColors.Window;
		((Control)TxtWaitDialTime).Cursor = Cursors.IBeam;
		((Control)TxtWaitDialTime).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtWaitDialTime).ForeColor = SystemColors.WindowText;
		((Control)TxtWaitDialTime).Location = new Point(180, 173);
		((TextBoxBase)TxtWaitDialTime).MaxLength = 0;
		((Control)TxtWaitDialTime).Name = "TxtWaitDialTime";
		((Control)TxtWaitDialTime).RightToLeft = (RightToLeft)0;
		((Control)TxtWaitDialTime).Size = new Size(41, 26);
		((Control)TxtWaitDialTime).TabIndex = 42;
		((Control)TxtWaitDialTime).Text = "20";
		((Control)ChkUsingModem).BackColor = Color.Transparent;
		((Control)ChkUsingModem).Cursor = Cursors.Default;
		((Control)ChkUsingModem).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ChkUsingModem).ForeColor = SystemColors.ControlText;
		((Control)ChkUsingModem).Location = new Point(10, 120);
		((Control)ChkUsingModem).Name = "ChkUsingModem";
		((Control)ChkUsingModem).RightToLeft = (RightToLeft)0;
		((Control)ChkUsingModem).Size = new Size(126, 24);
		((Control)ChkUsingModem).TabIndex = 41;
		((Control)ChkUsingModem).Text = "Using Modem";
		((ButtonBase)ChkUsingModem).UseVisualStyleBackColor = false;
		ChkUsingModem.CheckedChanged += chkUsingModem_CheckedChanged;
		((Control)LblTelphon).BackColor = Color.Transparent;
		((Control)LblTelphon).Cursor = Cursors.Default;
		((Control)LblTelphon).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblTelphon).ForeColor = SystemColors.ControlText;
		((Control)LblTelphon).Location = new Point(111, 144);
		((Control)LblTelphon).Name = "LblTelphon";
		((Control)LblTelphon).RightToLeft = (RightToLeft)0;
		((Control)LblTelphon).Size = new Size(63, 20);
		((Control)LblTelphon).TabIndex = 46;
		((Control)LblTelphon).Text = "Tel No :";
		((Control)LblWaitTime).BackColor = Color.Transparent;
		((Control)LblWaitTime).Cursor = Cursors.Default;
		((Control)LblWaitTime).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblWaitTime).ForeColor = SystemColors.ControlText;
		((Control)LblWaitTime).Location = new Point(0, 174);
		((Control)LblWaitTime).Name = "LblWaitTime";
		((Control)LblWaitTime).RightToLeft = (RightToLeft)0;
		((Control)LblWaitTime).Size = new Size(177, 39);
		((Control)LblWaitTime).TabIndex = 44;
		((Control)LblWaitTime).Text = "Wait Time For Dialing :";
		((Control)LblS).BackColor = Color.Transparent;
		((Control)LblS).Cursor = Cursors.Default;
		((Control)LblS).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblS).ForeColor = SystemColors.ControlText;
		((Control)LblS).Location = new Point(227, 176);
		((Control)LblS).Name = "LblS";
		((Control)LblS).RightToLeft = (RightToLeft)0;
		((Control)LblS).Size = new Size(27, 20);
		((Control)LblS).TabIndex = 43;
		((Control)LblS).Text = "(s)";
		((Control)CmbCommBaudRate).BackColor = SystemColors.Window;
		((Control)CmbCommBaudRate).Cursor = Cursors.Default;
		CmbCommBaudRate.DropDownStyle = (ComboBoxStyle)2;
		((Control)CmbCommBaudRate).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)CmbCommBaudRate).ForeColor = SystemColors.WindowText;
		CmbCommBaudRate.Items.AddRange(new object[5] { "9600", "19200", "38400", "57600", "115200" });
		((Control)CmbCommBaudRate).Location = new Point(116, 56);
		((Control)CmbCommBaudRate).Name = "CmbCommBaudRate";
		((Control)CmbCommBaudRate).RightToLeft = (RightToLeft)0;
		((Control)CmbCommBaudRate).Size = new Size(105, 27);
		((Control)CmbCommBaudRate).TabIndex = 24;
		((Control)CmbComPort).BackColor = SystemColors.Window;
		((Control)CmbComPort).Cursor = Cursors.Default;
		CmbComPort.DropDownStyle = (ComboBoxStyle)2;
		((Control)CmbComPort).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)CmbComPort).ForeColor = SystemColors.WindowText;
		CmbComPort.Items.AddRange(new object[4] { "1", "2", "3", "4" });
		((Control)CmbComPort).Location = new Point(116, 24);
		((Control)CmbComPort).Name = "CmbComPort";
		((Control)CmbComPort).RightToLeft = (RightToLeft)0;
		((Control)CmbComPort).Size = new Size(105, 27);
		((Control)CmbComPort).TabIndex = 22;
		((Control)LblComBaudRate).AutoSize = true;
		((Control)LblComBaudRate).BackColor = Color.Transparent;
		((Control)LblComBaudRate).Cursor = Cursors.Default;
		((Control)LblComBaudRate).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblComBaudRate).ForeColor = SystemColors.ControlText;
		((Control)LblComBaudRate).Location = new Point(20, 60);
		((Control)LblComBaudRate).Name = "LblComBaudRate";
		((Control)LblComBaudRate).RightToLeft = (RightToLeft)0;
		((Control)LblComBaudRate).Size = new Size(84, 19);
		((Control)LblComBaudRate).TabIndex = 25;
		((Control)LblComBaudRate).Text = "Baudrate : ";
		LblComBaudRate.TextAlign = (ContentAlignment)16;
		((Control)LblComPort).AutoSize = true;
		((Control)LblComPort).BackColor = Color.Transparent;
		((Control)LblComPort).Cursor = Cursors.Default;
		((Control)LblComPort).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblComPort).ForeColor = SystemColors.ControlText;
		((Control)LblComPort).Location = new Point(20, 28);
		((Control)LblComPort).Name = "LblComPort";
		((Control)LblComPort).RightToLeft = (RightToLeft)0;
		((Control)LblComPort).Size = new Size(81, 19);
		((Control)LblComPort).TabIndex = 23;
		((Control)LblComPort).Text = "ComPort : ";
		LblComPort.TextAlign = (ContentAlignment)16;
		TxtMachineNumber.AcceptsReturn = true;
		((Control)TxtMachineNumber).BackColor = SystemColors.Window;
		((Control)TxtMachineNumber).Cursor = Cursors.IBeam;
		((Control)TxtMachineNumber).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtMachineNumber).ForeColor = SystemColors.WindowText;
		((Control)TxtMachineNumber).Location = new Point(207, 98);
		((TextBoxBase)TxtMachineNumber).MaxLength = 0;
		((Control)TxtMachineNumber).Name = "TxtMachineNumber";
		((Control)TxtMachineNumber).RightToLeft = (RightToLeft)0;
		((Control)TxtMachineNumber).Size = new Size(42, 26);
		((Control)TxtMachineNumber).TabIndex = 72;
		((Control)TxtMachineNumber).Text = "1";
		TxtMachineNumber.TextAlign = (HorizontalAlignment)2;
		((Control)Frame2).BackColor = Color.Transparent;
		((Control)Frame2).Controls.Add((Control)(object)OptNetworkDevice);
		((Control)Frame2).Controls.Add((Control)(object)CmdGetIp);
		((Control)Frame2).Controls.Add((Control)(object)TxtDeviceName);
		((Control)Frame2).Controls.Add((Control)(object)LblDeviceName);
		((Control)Frame2).Controls.Add((Control)(object)ChkUdpFlag);
		((Control)Frame2).Controls.Add((Control)(object)LblTimeOutT);
		((Control)Frame2).Controls.Add((Control)(object)TxtTimeOut);
		((Control)Frame2).Controls.Add((Control)(object)LblTimeOut);
		((Control)Frame2).Controls.Add((Control)(object)TxtPassword);
		((Control)Frame2).Controls.Add((Control)(object)TxtPortNo);
		((Control)Frame2).Controls.Add((Control)(object)TxtIpAddress);
		((Control)Frame2).Controls.Add((Control)(object)LblPassword);
		((Control)Frame2).Controls.Add((Control)(object)LblPortNo);
		((Control)Frame2).Controls.Add((Control)(object)LblIpAddress);
		((Control)Frame2).ForeColor = SystemColors.ControlText;
		((Control)Frame2).Location = new Point(18, 403);
		((Control)Frame2).Name = "Frame2";
		((Control)Frame2).RightToLeft = (RightToLeft)0;
		((Control)Frame2).Size = new Size(260, 231);
		((Control)Frame2).TabIndex = 84;
		Frame2.TabStop = false;
		((Control)CmdGetIp).Location = new Point(210, 88);
		((Control)CmdGetIp).Name = "CmdGetIp";
		((Control)CmdGetIp).Size = new Size(33, 23);
		((Control)CmdGetIp).TabIndex = 27;
		((Control)CmdGetIp).Text = "...";
		((Control)CmdGetIp).Click += cmdGetIP_Click;
		TxtDeviceName.AcceptsReturn = true;
		((Control)TxtDeviceName).BackColor = SystemColors.Window;
		((Control)TxtDeviceName).Cursor = Cursors.IBeam;
		((Control)TxtDeviceName).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtDeviceName).ForeColor = SystemColors.WindowText;
		((Control)TxtDeviceName).Location = new Point(120, 60);
		((TextBoxBase)TxtDeviceName).MaxLength = 0;
		((Control)TxtDeviceName).Name = "TxtDeviceName";
		((Control)TxtDeviceName).RightToLeft = (RightToLeft)0;
		((Control)TxtDeviceName).Size = new Size(122, 26);
		((Control)TxtDeviceName).TabIndex = 25;
		((Control)TxtDeviceName).Text = "FingerKeeper";
		((Control)LblDeviceName).BackColor = Color.Transparent;
		((Control)LblDeviceName).Cursor = Cursors.Default;
		((Control)LblDeviceName).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblDeviceName).ForeColor = SystemColors.ControlText;
		((Control)LblDeviceName).Location = new Point(10, 68);
		((Control)LblDeviceName).Name = "LblDeviceName";
		((Control)LblDeviceName).RightToLeft = (RightToLeft)0;
		((Control)LblDeviceName).Size = new Size(110, 19);
		((Control)LblDeviceName).TabIndex = 26;
		((Control)LblDeviceName).Text = "Device Name :";
		LblDeviceName.TextAlign = (ContentAlignment)16;
		((Control)ChkUdpFlag).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ChkUdpFlag).Location = new Point(120, 202);
		((Control)ChkUdpFlag).Name = "ChkUdpFlag";
		((Control)ChkUdpFlag).Size = new Size(64, 22);
		((Control)ChkUdpFlag).TabIndex = 24;
		((Control)ChkUdpFlag).Text = "UDP";
		((Control)LblTimeOutT).AutoSize = true;
		((Control)LblTimeOutT).BackColor = Color.Transparent;
		((Control)LblTimeOutT).Cursor = Cursors.Default;
		((Control)LblTimeOutT).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblTimeOutT).ForeColor = SystemColors.ControlText;
		((Control)LblTimeOutT).Location = new Point(184, 172);
		((Control)LblTimeOutT).Name = "LblTimeOutT";
		((Control)LblTimeOutT).RightToLeft = (RightToLeft)0;
		((Control)LblTimeOutT).Size = new Size(38, 19);
		((Control)LblTimeOutT).TabIndex = 23;
		((Control)LblTimeOutT).Text = "(ms)";
		LblTimeOutT.TextAlign = (ContentAlignment)16;
		TxtTimeOut.AcceptsReturn = true;
		((Control)TxtTimeOut).BackColor = SystemColors.Window;
		((Control)TxtTimeOut).Cursor = Cursors.IBeam;
		((Control)TxtTimeOut).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtTimeOut).ForeColor = SystemColors.WindowText;
		((Control)TxtTimeOut).Location = new Point(120, 172);
		((TextBoxBase)TxtTimeOut).MaxLength = 0;
		((Control)TxtTimeOut).Name = "TxtTimeOut";
		((Control)TxtTimeOut).RightToLeft = (RightToLeft)0;
		((Control)TxtTimeOut).Size = new Size(57, 26);
		((Control)TxtTimeOut).TabIndex = 22;
		((Control)TxtTimeOut).Text = "5000";
		((Control)LblTimeOut).BackColor = Color.Transparent;
		((Control)LblTimeOut).Cursor = Cursors.Default;
		((Control)LblTimeOut).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblTimeOut).ForeColor = SystemColors.ControlText;
		((Control)LblTimeOut).Location = new Point(10, 179);
		((Control)LblTimeOut).Name = "LblTimeOut";
		((Control)LblTimeOut).RightToLeft = (RightToLeft)0;
		((Control)LblTimeOut).Size = new Size(110, 20);
		((Control)LblTimeOut).TabIndex = 21;
		((Control)LblTimeOut).Text = "TimeOut :";
		LblTimeOut.TextAlign = (ContentAlignment)16;
		TxtPassword.AcceptsReturn = true;
		((Control)TxtPassword).BackColor = SystemColors.Window;
		((Control)TxtPassword).Cursor = Cursors.IBeam;
		((Control)TxtPassword).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtPassword).ForeColor = SystemColors.WindowText;
		((Control)TxtPassword).Location = new Point(120, 135);
		((TextBoxBase)TxtPassword).MaxLength = 0;
		((Control)TxtPassword).Name = "TxtPassword";
		((Control)TxtPassword).RightToLeft = (RightToLeft)0;
		((Control)TxtPassword).Size = new Size(57, 26);
		((Control)TxtPassword).TabIndex = 20;
		((Control)TxtPassword).Text = "0";
		TxtPortNo.AcceptsReturn = true;
		((Control)TxtPortNo).BackColor = SystemColors.Window;
		((Control)TxtPortNo).Cursor = Cursors.IBeam;
		((Control)TxtPortNo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtPortNo).ForeColor = SystemColors.WindowText;
		((Control)TxtPortNo).Location = new Point(120, 98);
		((TextBoxBase)TxtPortNo).MaxLength = 0;
		((Control)TxtPortNo).Name = "TxtPortNo";
		((Control)TxtPortNo).RightToLeft = (RightToLeft)0;
		((Control)TxtPortNo).Size = new Size(57, 26);
		((Control)TxtPortNo).TabIndex = 13;
		((Control)TxtPortNo).Text = "5005";
		TxtIpAddress.AcceptsReturn = true;
		((Control)TxtIpAddress).BackColor = SystemColors.Window;
		((Control)TxtIpAddress).Cursor = Cursors.IBeam;
		((Control)TxtIpAddress).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtIpAddress).ForeColor = SystemColors.WindowText;
		((Control)TxtIpAddress).Location = new Point(120, 27);
		((TextBoxBase)TxtIpAddress).MaxLength = 0;
		((Control)TxtIpAddress).Name = "TxtIpAddress";
		((Control)TxtIpAddress).RightToLeft = (RightToLeft)0;
		((Control)TxtIpAddress).Size = new Size(122, 26);
		((Control)TxtIpAddress).TabIndex = 12;
		((Control)TxtIpAddress).Text = "192.168.1.224";
		((Control)LblPassword).BackColor = Color.Transparent;
		((Control)LblPassword).Cursor = Cursors.Default;
		((Control)LblPassword).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblPassword).ForeColor = SystemColors.ControlText;
		((Control)LblPassword).Location = new Point(10, 142);
		((Control)LblPassword).Name = "LblPassword";
		((Control)LblPassword).RightToLeft = (RightToLeft)0;
		((Control)LblPassword).Size = new Size(110, 19);
		((Control)LblPassword).TabIndex = 19;
		((Control)LblPassword).Text = "Password :";
		LblPassword.TextAlign = (ContentAlignment)16;
		((Control)LblPortNo).BackColor = Color.Transparent;
		((Control)LblPortNo).Cursor = Cursors.Default;
		((Control)LblPortNo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblPortNo).ForeColor = SystemColors.ControlText;
		((Control)LblPortNo).Location = new Point(10, 105);
		((Control)LblPortNo).Name = "LblPortNo";
		((Control)LblPortNo).RightToLeft = (RightToLeft)0;
		((Control)LblPortNo).Size = new Size(110, 19);
		((Control)LblPortNo).TabIndex = 15;
		((Control)LblPortNo).Text = "Port Number :";
		LblPortNo.TextAlign = (ContentAlignment)16;
		((Control)LblIpAddress).BackColor = Color.Transparent;
		((Control)LblIpAddress).Cursor = Cursors.Default;
		((Control)LblIpAddress).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblIpAddress).ForeColor = SystemColors.ControlText;
		((Control)LblIpAddress).Location = new Point(10, 31);
		((Control)LblIpAddress).Name = "LblIpAddress";
		((Control)LblIpAddress).RightToLeft = (RightToLeft)0;
		((Control)LblIpAddress).Size = new Size(110, 19);
		((Control)LblIpAddress).TabIndex = 14;
		((Control)LblIpAddress).Text = "IP Address :";
		LblIpAddress.TextAlign = (ContentAlignment)16;
		((Control)LblComLicense).AutoSize = true;
		((Control)LblComLicense).BackColor = Color.Transparent;
		((Control)LblComLicense).Cursor = Cursors.Default;
		((Control)LblComLicense).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)LblComLicense).ForeColor = SystemColors.ControlText;
		((Control)LblComLicense).Location = new Point(316, 101);
		((Control)LblComLicense).Name = "LblComLicense";
		((Control)LblComLicense).RightToLeft = (RightToLeft)0;
		((Control)LblComLicense).Size = new Size(62, 19);
		((Control)LblComLicense).TabIndex = 73;
		((Control)LblComLicense).Text = "License :";
		((Control)OptUsbDevice).BackColor = Color.Transparent;
		((Control)OptUsbDevice).Cursor = Cursors.Default;
		((Control)OptUsbDevice).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)OptUsbDevice).ForeColor = SystemColors.ControlText;
		((Control)OptUsbDevice).Location = new Point(28, 138);
		((Control)OptUsbDevice).Name = "OptUsbDevice";
		((Control)OptUsbDevice).RightToLeft = (RightToLeft)0;
		((Control)OptUsbDevice).Size = new Size(148, 20);
		((Control)OptUsbDevice).TabIndex = 88;
		OptUsbDevice.TabStop = true;
		((Control)OptUsbDevice).Text = "USB Device";
		((ButtonBase)OptUsbDevice).UseVisualStyleBackColor = false;
		OptUsbDevice.CheckedChanged += optUSBDevice_CheckedChanged;
		((Control)CmdEnrollData).BackColor = SystemColors.Control;
		((Control)CmdEnrollData).Cursor = Cursors.Default;
		((Control)CmdEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdEnrollData).ForeColor = SystemColors.ControlText;
		((Control)CmdEnrollData).Location = new Point(284, 189);
		((Control)CmdEnrollData).Name = "CmdEnrollData";
		((Control)CmdEnrollData).RightToLeft = (RightToLeft)0;
		((Control)CmdEnrollData).Size = new Size(228, 37);
		((Control)CmdEnrollData).TabIndex = 75;
		((Control)CmdEnrollData).Text = "Enroll Data Management";
		((ButtonBase)CmdEnrollData).UseVisualStyleBackColor = false;
		((Control)CmdEnrollData).Click += cmdEnrollData_Click;
		((Control)CmdSystemInfo).BackColor = SystemColors.Control;
		((Control)CmdSystemInfo).Cursor = Cursors.Default;
		((Control)CmdSystemInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdSystemInfo).ForeColor = SystemColors.ControlText;
		((Control)CmdSystemInfo).Location = new Point(284, 285);
		((Control)CmdSystemInfo).Name = "CmdSystemInfo";
		((Control)CmdSystemInfo).RightToLeft = (RightToLeft)0;
		((Control)CmdSystemInfo).Size = new Size(228, 37);
		((Control)CmdSystemInfo).TabIndex = 77;
		((Control)CmdSystemInfo).Text = "System Info Management";
		((ButtonBase)CmdSystemInfo).UseVisualStyleBackColor = false;
		((Control)CmdSystemInfo).Click += cmdSystemInfo_Click;
		((Control)CmdOpenComm).BackColor = SystemColors.Control;
		((Control)CmdOpenComm).Cursor = Cursors.Default;
		((Control)CmdOpenComm).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdOpenComm).ForeColor = SystemColors.ControlText;
		((Control)CmdOpenComm).Location = new Point(284, 138);
		((Control)CmdOpenComm).Name = "CmdOpenComm";
		((Control)CmdOpenComm).RightToLeft = (RightToLeft)0;
		((Control)CmdOpenComm).Size = new Size(110, 37);
		((Control)CmdOpenComm).TabIndex = 81;
		((Control)CmdOpenComm).Text = "Open Comm";
		((ButtonBase)CmdOpenComm).UseVisualStyleBackColor = false;
		((Control)CmdOpenComm).Click += cmdOpenComm_Click;
		TxtLicense.AcceptsReturn = true;
		((Control)TxtLicense).BackColor = SystemColors.Window;
		((Control)TxtLicense).Cursor = Cursors.IBeam;
		((Control)TxtLicense).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtLicense).ForeColor = SystemColors.WindowText;
		((Control)TxtLicense).Location = new Point(383, 98);
		((TextBoxBase)TxtLicense).MaxLength = 0;
		((Control)TxtLicense).Name = "TxtLicense";
		((Control)TxtLicense).RightToLeft = (RightToLeft)0;
		((Control)TxtLicense).Size = new Size(54, 26);
		((Control)TxtLicense).TabIndex = 74;
		((Control)TxtLicense).Text = "1261";
		TxtLicense.TextAlign = (HorizontalAlignment)2;
		((Control)LblMachineNumber).AutoSize = true;
		((Control)LblMachineNumber).BackColor = Color.Transparent;
		((Control)LblMachineNumber).Cursor = Cursors.Default;
		((Control)LblMachineNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)LblMachineNumber).ForeColor = SystemColors.ControlText;
		((Control)LblMachineNumber).Location = new Point(71, 101);
		((Control)LblMachineNumber).Name = "LblMachineNumber";
		((Control)LblMachineNumber).RightToLeft = (RightToLeft)0;
		((Control)LblMachineNumber).Size = new Size(122, 19);
		((Control)LblMachineNumber).TabIndex = 71;
		((Control)LblMachineNumber).Text = "Machine Number :";
		((Control)LblVer).BackColor = Color.Transparent;
		((Control)LblVer).Cursor = Cursors.Default;
		((Control)LblVer).Font = new Font("Times New Roman", 12f, (FontStyle)3, (GraphicsUnit)3, (byte)0);
		((Control)LblVer).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)LblVer).Location = new Point(-1, 57);
		((Control)LblVer).Name = "LblVer";
		((Control)LblVer).RightToLeft = (RightToLeft)0;
		((Control)LblVer).Size = new Size(519, 22);
		((Control)LblVer).TabIndex = 70;
		((Control)LblVer).Text = "FK623Attend.dll (v2.8.10.6)";
		LblVer.TextAlign = (ContentAlignment)2;
		((Control)LbSubject).BackColor = Color.Transparent;
		((Control)LbSubject).Cursor = Cursors.Default;
		((Control)LbSubject).Font = new Font("Times New Roman", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LbSubject).ForeColor = Color.Red;
		((Control)LbSubject).Location = new Point(-1, 15);
		((Control)LbSubject).Name = "LbSubject";
		((Control)LbSubject).RightToLeft = (RightToLeft)0;
		((Control)LbSubject).Size = new Size(536, 31);
		((Control)LbSubject).TabIndex = 69;
		((Control)LbSubject).Text = "Novin Pardaz C# Sample (v2.3.5.0)";
		LbSubject.TextAlign = (ContentAlignment)2;
		((Control)CmdSetPostShiftInfo).BackColor = SystemColors.Control;
		((Control)CmdSetPostShiftInfo).Cursor = Cursors.Default;
		((Control)CmdSetPostShiftInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdSetPostShiftInfo).ForeColor = SystemColors.ControlText;
		((Control)CmdSetPostShiftInfo).Location = new Point(284, 476);
		((Control)CmdSetPostShiftInfo).Name = "CmdSetPostShiftInfo";
		((Control)CmdSetPostShiftInfo).RightToLeft = (RightToLeft)0;
		((Control)CmdSetPostShiftInfo).Size = new Size(228, 38);
		((Control)CmdSetPostShiftInfo).TabIndex = 89;
		((Control)CmdSetPostShiftInfo).Text = "Set PostInfo && ShiftInfo";
		((ButtonBase)CmdSetPostShiftInfo).UseVisualStyleBackColor = false;
		((Control)CmdSetPostShiftInfo).Click += cmdSetPostShiftInfo_Click;
		((Control)CmdPhotoManage).BackColor = SystemColors.Control;
		((Control)CmdPhotoManage).Cursor = Cursors.Default;
		((Control)CmdPhotoManage).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdPhotoManage).ForeColor = SystemColors.ControlText;
		((Control)CmdPhotoManage).Location = new Point(284, 525);
		((Control)CmdPhotoManage).Name = "CmdPhotoManage";
		((Control)CmdPhotoManage).RightToLeft = (RightToLeft)0;
		((Control)CmdPhotoManage).Size = new Size(228, 38);
		((Control)CmdPhotoManage).TabIndex = 90;
		((Control)CmdPhotoManage).Text = "Photo Management";
		((ButtonBase)CmdPhotoManage).UseVisualStyleBackColor = false;
		((Control)CmdPhotoManage).Click += cmdPhotoManage_Click;
		((Control)CmdSetUserName).BackColor = SystemColors.Control;
		((Control)CmdSetUserName).Cursor = Cursors.Default;
		((Control)CmdSetUserName).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdSetUserName).ForeColor = SystemColors.ControlText;
		((Control)CmdSetUserName).Location = new Point(284, 380);
		((Control)CmdSetUserName).Name = "CmdSetUserName";
		((Control)CmdSetUserName).RightToLeft = (RightToLeft)0;
		((Control)CmdSetUserName).Size = new Size(228, 36);
		((Control)CmdSetUserName).TabIndex = 91;
		((Control)CmdSetUserName).Text = "Set User Name";
		((ButtonBase)CmdSetUserName).UseVisualStyleBackColor = false;
		((Control)CmdSetUserName).Click += cmdSetUserName_Click;
		((Control)CmdPassTimeHs).BackColor = SystemColors.Control;
		((Control)CmdPassTimeHs).Cursor = Cursors.Default;
		((Control)CmdPassTimeHs).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)CmdPassTimeHs).ForeColor = SystemColors.ControlText;
		((Control)CmdPassTimeHs).Location = new Point(284, 573);
		((Control)CmdPassTimeHs).Name = "CmdPassTimeHs";
		((Control)CmdPassTimeHs).RightToLeft = (RightToLeft)0;
		((Control)CmdPassTimeHs).Size = new Size(228, 38);
		((Control)CmdPassTimeHs).TabIndex = 90;
		((Control)CmdPassTimeHs).Text = "Pass Time";
		((ButtonBase)CmdPassTimeHs).UseVisualStyleBackColor = false;
		((Control)CmdPassTimeHs).Click += cmdPassTimeHS_Click;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Control)this).BackColor = Color.FromArgb(192, 192, 255);
		((Form)this).ClientSize = new Size(530, 647);
		((Control)this).Controls.Add((Control)(object)CmdSetUserName);
		((Control)this).Controls.Add((Control)(object)CmdPassTimeHs);
		((Control)this).Controls.Add((Control)(object)CmdPhotoManage);
		((Control)this).Controls.Add((Control)(object)CmdSetPostShiftInfo);
		((Control)this).Controls.Add((Control)(object)CmdBellInfo);
		((Control)this).Controls.Add((Control)(object)CmdCloseComm);
		((Control)this).Controls.Add((Control)(object)CmdProuctData);
		((Control)this).Controls.Add((Control)(object)CmdLogData);
		((Control)this).Controls.Add((Control)(object)TxtMachineNumber);
		((Control)this).Controls.Add((Control)(object)LblComLicense);
		((Control)this).Controls.Add((Control)(object)TxtLicense);
		((Control)this).Controls.Add((Control)(object)LblMachineNumber);
		((Control)this).Controls.Add((Control)(object)OptUsbDevice);
		((Control)this).Controls.Add((Control)(object)CmdEnrollData);
		((Control)this).Controls.Add((Control)(object)CmdSystemInfo);
		((Control)this).Controls.Add((Control)(object)CmdOpenComm);
		((Control)this).Controls.Add((Control)(object)LblVer);
		((Control)this).Controls.Add((Control)(object)LbSubject);
		((Control)this).Controls.Add((Control)(object)Frame3);
		((Control)this).Controls.Add((Control)(object)Frame2);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmConnectDeviceHysoon";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "ارتباط با دستگاه حضور و غیاب ";
		((Form)this).Closed += frmMain_Closed;
		((Form)this).Load += frmMain_Load;
		((Control)Frame3).ResumeLayout(false);
		((Control)Frame3).PerformLayout();
		((Control)Frame2).ResumeLayout(false);
		((Control)Frame2).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void MyTest()
	{
		string astrVal = "إم\u00b8»ت؟µç×س¼¼ت\u064f";
		string astrUtf16Hex = FKAttendDLL.StringToUTF16HexString(astrVal);
		string text = FKAttendDLL.UTF16HexStringToString(astrUtf16Hex);
		((Control)TxtLicense).Text = Convert.ToString(1261);
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		GfrmMain = this;
		FKAttendDLL.nCommHandleIndex = 0;
		OptSerialDevice.Checked = false;
		OptNetworkDevice.Checked = false;
		OptUsbDevice.Checked = true;
		optNetworkDevice_CheckedChanged(OptNetworkDevice, e);
		EnableButtons(abEnableFlag: false);
		OpenFlag = false;
		((Control)TxtMachineNumber).Text = "1";
		((ListControl)CmbComPort).SelectedIndex = 0;
		((ListControl)CmbCommBaudRate).SelectedIndex = 2;
		RegisterManagement.RegisterOCXs_NovinP(Application.StartupPath);
	}

	private void frmMain_Closed(object sender, EventArgs e)
	{
		cmdCloseComm_Click(CmdCloseComm, new EventArgs());
	}

	private void cmdOpenComm_Click(object sender, EventArgs e)
	{
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		string astrTelNumber = "";
		int anWaitDialTime = 3000;
		((Control)CmdOpenComm).Enabled = false;
		int anMachineNo = Convert.ToInt32(((Control)TxtMachineNumber).Text);
		int anLicense = Convert.ToInt32(((Control)TxtLicense).Text);
		if (OptSerialDevice.Checked)
		{
			if (ChkUsingModem.Checked)
			{
				astrTelNumber = ((Control)TxtTelNumber).Text.Trim();
				anWaitDialTime = Convert.ToInt32(((Control)TxtWaitDialTime).Text.Trim());
				if (anWaitDialTime < 10 && anWaitDialTime > 60)
				{
					anWaitDialTime = 10;
					((Control)TxtWaitDialTime).Text = "10";
				}
				else
				{
					astrTelNumber = "";
					anWaitDialTime = 0;
				}
			}
			int anComPort = Convert.ToInt32(((Control)CmbComPort).Text.Trim());
			int anBaudRate = Convert.ToInt32(((Control)CmbCommBaudRate).Text.Trim());
			int anComTimeOut = Convert.ToInt32(((Control)TxtComTimeOut).Text.Trim());
			FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectComm(anMachineNo, anComPort, anBaudRate, astrTelNumber, anWaitDialTime, anLicense, anComTimeOut);
		}
		else if (OptNetworkDevice.Checked)
		{
			string astrIpAddress = ((Control)TxtIpAddress).Text.Trim();
			int anNetPort = Convert.ToInt32(((Control)TxtPortNo).Text);
			int anNetPassword = Convert.ToInt32(((Control)TxtPassword).Text);
			int anComTimeOut = Convert.ToInt32(((Control)TxtTimeOut).Text);
			int anProtocolType = (ChkUdpFlag.Checked ? 1 : 0);
			FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectNet(anMachineNo, astrIpAddress, anNetPort, anComTimeOut, anProtocolType, anNetPassword, anLicense);
		}
		else if (OptUsbDevice.Checked)
		{
			FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectUSB(anMachineNo, anLicense);
		}
		if (FKAttendDLL.nCommHandleIndex > 0)
		{
			OpenFlag = true;
			EnableButtons(abEnableFlag: true);
		}
		else
		{
			long anResultCode = FKAttendDLL.nCommHandleIndex;
			MessageBox.Show(FKAttendDLL.ReturnResultPrint(anResultCode), "error", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)CmdOpenComm).Enabled = true;
		}
	}

	private void cmdCloseComm_Click(object sender, EventArgs e)
	{
		if (OpenFlag)
		{
			FKAttendDLL.FK_DisConnect(FKAttendDLL.nCommHandleIndex);
			OpenFlag = false;
			EnableButtons(abEnableFlag: false);
		}
	}

	private void cmdEnrollData_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmEnroll());
	}

	private void cmdLogData_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmLog());
	}

	private void cmdSystemInfo_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmSystemInfo());
	}

	private void cmdProuctData_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmProductData());
	}

	private void cmdBellInfo_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmBellInfo());
	}

	private void cmdSetPostShiftInfo_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmPostShiftInfo());
	}

	private void cmdPhotoManage_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmPhotoManage());
	}

	private void cmdSetUserName_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmUserInfo());
	}

	private void cmdSetPassTime_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmPasstimeInfo());
	}

	private void cmdSetAdjustInfo_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmAdjust());
	}

	private void cmdNetInfo_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmNetInfo());
	}

	private void ShowChildForm(Form afrmChild)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (afrmChild != null)
		{
			((Control)this).Hide();
			afrmChild.Owner = (Form)(object)this;
			afrmChild.ShowDialog();
			((Component)(object)afrmChild).Dispose();
			((Control)this).Show();
		}
	}

	private void EnableButtons(bool abEnableFlag)
	{
		((Control)CmdOpenComm).Enabled = !abEnableFlag;
		((Control)CmdCloseComm).Enabled = abEnableFlag;
		((Control)CmdSystemInfo).Enabled = abEnableFlag;
		((Control)CmdProuctData).Enabled = abEnableFlag;
		((Control)CmdBellInfo).Enabled = abEnableFlag;
		((Control)CmdSetUserName).Enabled = abEnableFlag;
		((Control)CmdSetPostShiftInfo).Enabled = abEnableFlag;
		((Control)CmdPhotoManage).Enabled = abEnableFlag;
		((Control)CmdPassTimeHs).Enabled = abEnableFlag;
		((Control)OptSerialDevice).Enabled = !abEnableFlag;
		((Control)OptNetworkDevice).Enabled = !abEnableFlag;
		((Control)OptUsbDevice).Enabled = !abEnableFlag;
	}

	private void EnableControls(long anEnableFlag)
	{
		((Control)LblComPort).Enabled = false;
		((Control)CmbComPort).Enabled = false;
		((Control)LblComBaudRate).Enabled = false;
		((Control)CmbCommBaudRate).Enabled = false;
		((Control)ChkUsingModem).Enabled = false;
		((Control)LblTelphon).Enabled = false;
		((Control)TxtTelNumber).Enabled = false;
		((Control)LblWaitTime).Enabled = false;
		((Control)TxtWaitDialTime).Enabled = false;
		((Control)LblS).Enabled = false;
		((Control)TxtComTimeOut).Enabled = false;
		((Control)LblComTimeOut).Enabled = false;
		((Control)LblComTimeOutT).Enabled = false;
		((Control)LblIpAddress).Enabled = false;
		((Control)TxtIpAddress).Enabled = false;
		((Control)LblDeviceName).Enabled = false;
		((Control)TxtDeviceName).Enabled = false;
		((Control)LblPortNo).Enabled = false;
		((Control)TxtPortNo).Enabled = false;
		((Control)LblPassword).Enabled = false;
		((Control)TxtPassword).Enabled = false;
		((Control)LblTimeOut).Enabled = false;
		((Control)TxtTimeOut).Enabled = false;
		((Control)LblTimeOutT).Enabled = false;
		((Control)ChkUdpFlag).Enabled = false;
		switch (anEnableFlag)
		{
		case 1L:
			((Control)LblComPort).Enabled = true;
			((Control)CmbComPort).Enabled = true;
			((Control)LblComBaudRate).Enabled = true;
			((Control)CmbCommBaudRate).Enabled = true;
			((Control)TxtComTimeOut).Enabled = true;
			((Control)LblComTimeOut).Enabled = true;
			((Control)LblComTimeOutT).Enabled = true;
			((Control)ChkUsingModem).Enabled = true;
			if (ChkUsingModem.Checked)
			{
				((Control)LblTelphon).Enabled = true;
				((Control)TxtTelNumber).Enabled = true;
				((Control)LblWaitTime).Enabled = true;
				((Control)TxtWaitDialTime).Enabled = true;
				((Control)LblS).Enabled = true;
			}
			break;
		case 2L:
			((Control)LblIpAddress).Enabled = true;
			((Control)TxtIpAddress).Enabled = true;
			((Control)LblDeviceName).Enabled = true;
			((Control)TxtDeviceName).Enabled = true;
			((Control)LblPortNo).Enabled = true;
			((Control)TxtPortNo).Enabled = true;
			((Control)LblPassword).Enabled = true;
			((Control)TxtPassword).Enabled = true;
			((Control)LblTimeOut).Enabled = true;
			((Control)TxtTimeOut).Enabled = true;
			((Control)LblTimeOutT).Enabled = true;
			((Control)ChkUdpFlag).Enabled = true;
			break;
		}
	}

	private void optNetworkDevice_CheckedChanged(object sender, EventArgs e)
	{
		if (OptNetworkDevice.Checked)
		{
			OptSerialDevice.Checked = false;
			OptUsbDevice.Checked = false;
			EnableControls(2L);
		}
	}

	private void optUSBDevice_CheckedChanged(object sender, EventArgs e)
	{
		if (OptUsbDevice.Checked)
		{
			OptSerialDevice.Checked = false;
			OptNetworkDevice.Checked = false;
			EnableControls(3L);
		}
	}

	private void chkUsingModem_CheckedChanged(object sender, EventArgs e)
	{
		if (OptSerialDevice.Checked)
		{
			EnableControls(1L);
		}
	}

	private void optSerialDevice_CheckedChanged(object sender, EventArgs e)
	{
		if (OptSerialDevice.Checked)
		{
			OptUsbDevice.Checked = false;
			OptNetworkDevice.Checked = false;
			EnableControls(1L);
		}
	}

	private void cmdGetIP_Click(object sender, EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(((Control)TxtDeviceName).Text.Trim());
			IPAddress[] addressList = hostEntry.AddressList;
			((Control)TxtIpAddress).Text = addressList[0].ToString();
		}
		catch (SocketException ex)
		{
			MessageBox.Show(ex.Message);
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void cmdPassTimeHS_Click(object sender, EventArgs e)
	{
		ShowChildForm((Form)(object)new frmPassTimeHS());
	}
}
