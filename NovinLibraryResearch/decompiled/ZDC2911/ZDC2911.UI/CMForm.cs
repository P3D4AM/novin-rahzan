using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Riss.Devices;
using ZDC2911.Entity;
using ZDC2911.IConvert;

namespace ZDC2911.UI;

public class CMForm : Form
{
	private Device device;

	private DeviceConnection deviceConnection;

	private DeviceCommEty deviceEty;

	private IContainer components = null;

	private GroupBox groupBox1;

	private ComboBox cbo_BaudRate;

	private ComboBox cbo_COMM;

	private Label lbl_BaudRate;

	private Label lbl_COMM;

	private RadioButton rdb_TCP;

	private RadioButton rdb_COMM;

	private RadioButton rdb_USB;

	private TextBox txt_IP;

	private Label lbl_Port;

	private Label lbl_IP;

	private Label lbl_ConnectionPwd;

	private Label lbl_DeviceId;

	private Button btn_OpenDevice;

	private Button btn_CloseDevice;

	private GroupBox group;

	private Button btn_GlogManagement;

	private Button btn_SlogManagement;

	private Button btn_AlarmSetting;

	private Button btn_AttendanceSetting;

	private Button btn_EnrollManagement;

	private Button btn_SystemSetting;

	private Button btn_RealTimeLog;

	private Button btn_NameDataSetting;

	private NumericUpDown nud_Pwd;

	private NumericUpDown nud_DN;

	private NumericUpDown nud_Port;

	public CMForm()
	{
		InitializeComponent();
	}

	public CMForm(int DeviceNumber, string Password, string IP, int TCP_Port)
	{
		InitializeComponent();
		int result = 0;
		int.TryParse(Password, out result);
		rdb_TCP.Checked = true;
		((Control)txt_IP).Text = IP;
		nud_Port.Value = TCP_Port;
		nud_DN.Value = DeviceNumber;
		nud_Pwd.Value = result;
	}

	public bool DisConnect()
	{
		deviceConnection.Close();
		((Control)btn_CloseDevice).Enabled = false;
		SetButtonEnabled(flag: false);
		((Control)btn_OpenDevice).Enabled = true;
		device = null;
		deviceConnection = null;
		deviceEty = null;
		return true;
	}

	public bool Connect()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			device = new Device();
			device.DN = (int)nud_DN.Value;
			device.Password = nud_Pwd.Value.ToString();
			device.Model = "ZDC2911";
			device.ConnectionModel = 5;
			if (rdb_USB.Checked)
			{
				device.CommunicationType = (CommunicationType)2;
			}
			else if (rdb_TCP.Checked)
			{
				if (string.IsNullOrEmpty(((Control)txt_IP).Text.Trim()))
				{
					MessageBox.Show("Please Input IP Address", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
					((Control)txt_IP).Focus();
					return false;
				}
				if (!ConvertObject.IsCorrenctIP(((Control)txt_IP).Text.Trim()))
				{
					MessageBox.Show("Illegal IP Address", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
					((Control)txt_IP).Focus();
					return false;
				}
				device.IpAddress = ((Control)txt_IP).Text.Trim();
				device.IpPort = (int)nud_Port.Value;
				device.CommunicationType = (CommunicationType)1;
			}
			else if (rdb_COMM.Checked)
			{
				device.SerialPort = Convert.ToInt32(cbo_COMM.SelectedItem.ToString().Replace("COM", string.Empty));
				device.Baudrate = Convert.ToInt32(cbo_BaudRate.SelectedItem);
				device.CommunicationType = (CommunicationType)0;
			}
			deviceConnection = DeviceConnection.CreateConnection(ref device);
			if (deviceConnection.Open() > 0)
			{
				deviceEty = new DeviceCommEty();
				deviceEty.Device = device;
				deviceEty.DeviceConnection = deviceConnection;
				((Control)btn_CloseDevice).Enabled = true;
				SetButtonEnabled(flag: true);
				((Control)btn_OpenDevice).Enabled = false;
				return true;
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public List<Record> GetALLGlog()
	{
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			List<DateTime> list = new List<DateTime>();
			list.Add(DateTime.Parse("2010/01/01"));
			list.Add(DateTime.Parse("2099/01/01"));
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj = false;
			obj2 = list;
			if (!deviceConnection.GetProperty((DeviceProperty)17, obj, ref device, ref obj2))
			{
				MessageBox.Show("Get All Glog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return null;
			}
			if ((int)obj2 == 0)
			{
				return null;
			}
			List<bool> list2 = new List<bool>();
			list2.Add(item: false);
			list2.Add(item: false);
			obj = list2;
			obj2 = list;
			if (deviceConnection.GetProperty((DeviceProperty)13, obj, ref device, ref obj2))
			{
				return (List<Record>)obj2;
			}
			MessageBox.Show("Get All Glog Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
			return null;
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
		}
	}

	public void SetButtonEnabled(bool flag)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)group).Controls)
		{
			Control val = item;
			if (((object)val).GetType().Equals(typeof(Button)))
			{
				val.Enabled = flag;
			}
		}
	}

	private void CMForm_Load(object sender, EventArgs e)
	{
		((ListControl)cbo_COMM).SelectedIndex = 0;
		((ListControl)cbo_BaudRate).SelectedIndex = 0;
		((Control)btn_CloseDevice).Enabled = false;
		SetButtonEnabled(flag: false);
	}

	private void btn_OpenDevice_Click(object sender, EventArgs e)
	{
		Connect();
	}

	private void btn_CloseDevice_Click(object sender, EventArgs e)
	{
		DisConnect();
	}

	private void btn_SystemSetting_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		SystemSettingForm systemSettingForm = new SystemSettingForm(deviceEty);
		((Form)systemSettingForm).ShowDialog();
	}

	private void btn_AlarmSetting_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		AlarmForm alarmForm = new AlarmForm(deviceEty);
		((Form)alarmForm).ShowDialog();
	}

	private void btn_AttendanceSetting_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		AttForm attForm = new AttForm(deviceEty);
		((Form)attForm).ShowDialog();
	}

	private void btn_EnrollManagement_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		EnrollForm enrollForm = new EnrollForm(deviceEty);
		((Form)enrollForm).ShowDialog();
	}

	private void btn_SlogManagement_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		SLogForm sLogForm = new SLogForm(deviceEty);
		((Form)sLogForm).ShowDialog();
	}

	private void btn_GlogManagement_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		GLogForm gLogForm = new GLogForm(deviceEty);
		((Form)gLogForm).ShowDialog();
	}

	private void btn_RealTimeLog_Click(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		RealTimeLogForm realTimeLogForm = new RealTimeLogForm();
		((Form)realTimeLogForm).ShowDialog();
	}

	private void btn_NameDataSetting_Click(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		NameDataSettingForm nameDataSettingForm = new NameDataSettingForm();
		((Form)nameDataSettingForm).ShowDialog();
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
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		groupBox1 = new GroupBox();
		nud_Port = new NumericUpDown();
		nud_Pwd = new NumericUpDown();
		nud_DN = new NumericUpDown();
		cbo_BaudRate = new ComboBox();
		cbo_COMM = new ComboBox();
		lbl_BaudRate = new Label();
		lbl_COMM = new Label();
		rdb_TCP = new RadioButton();
		rdb_COMM = new RadioButton();
		rdb_USB = new RadioButton();
		txt_IP = new TextBox();
		lbl_Port = new Label();
		lbl_IP = new Label();
		lbl_ConnectionPwd = new Label();
		lbl_DeviceId = new Label();
		btn_OpenDevice = new Button();
		btn_CloseDevice = new Button();
		group = new GroupBox();
		btn_GlogManagement = new Button();
		btn_SlogManagement = new Button();
		btn_AlarmSetting = new Button();
		btn_AttendanceSetting = new Button();
		btn_EnrollManagement = new Button();
		btn_SystemSetting = new Button();
		btn_RealTimeLog = new Button();
		btn_NameDataSetting = new Button();
		((Control)groupBox1).SuspendLayout();
		((ISupportInitialize)nud_Port).BeginInit();
		((ISupportInitialize)nud_Pwd).BeginInit();
		((ISupportInitialize)nud_DN).BeginInit();
		((Control)group).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupBox1).Controls.Add((Control)(object)nud_Port);
		((Control)groupBox1).Controls.Add((Control)(object)nud_Pwd);
		((Control)groupBox1).Controls.Add((Control)(object)nud_DN);
		((Control)groupBox1).Controls.Add((Control)(object)cbo_BaudRate);
		((Control)groupBox1).Controls.Add((Control)(object)cbo_COMM);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_BaudRate);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_COMM);
		((Control)groupBox1).Controls.Add((Control)(object)rdb_TCP);
		((Control)groupBox1).Controls.Add((Control)(object)rdb_COMM);
		((Control)groupBox1).Controls.Add((Control)(object)rdb_USB);
		((Control)groupBox1).Controls.Add((Control)(object)txt_IP);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_Port);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_IP);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_ConnectionPwd);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_DeviceId);
		((Control)groupBox1).Location = new Point(12, 12);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(238, 244);
		((Control)groupBox1).TabIndex = 0;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Connection Information";
		((Control)nud_Port).Location = new Point(83, 213);
		nud_Port.Maximum = new decimal(new int[4] { 99999, 0, 0, 0 });
		nud_Port.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)nud_Port).Name = "nud_Port";
		((Control)nud_Port).Size = new Size(144, 21);
		((Control)nud_Port).TabIndex = 14;
		nud_Port.Value = new decimal(new int[4] { 5005, 0, 0, 0 });
		((Control)nud_Pwd).Location = new Point(76, 41);
		nud_Pwd.Maximum = new decimal(new int[4] { 65535, 0, 0, 0 });
		((Control)nud_Pwd).Name = "nud_Pwd";
		((Control)nud_Pwd).Size = new Size(120, 21);
		((Control)nud_Pwd).TabIndex = 3;
		((Control)nud_DN).Location = new Point(77, 14);
		nud_DN.Maximum = new decimal(new int[4] { 65535, 0, 0, 0 });
		((Control)nud_DN).Name = "nud_DN";
		((Control)nud_DN).Size = new Size(120, 21);
		((Control)nud_DN).TabIndex = 1;
		nud_DN.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		cbo_BaudRate.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_BaudRate).FormattingEnabled = true;
		cbo_BaudRate.Items.AddRange(new object[5] { "9600", "19200", "38400", "57600", "115200" });
		((Control)cbo_BaudRate).Location = new Point(83, 138);
		((Control)cbo_BaudRate).Name = "cbo_BaudRate";
		((Control)cbo_BaudRate).Size = new Size(120, 20);
		((Control)cbo_BaudRate).TabIndex = 9;
		cbo_COMM.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_COMM).FormattingEnabled = true;
		cbo_COMM.Items.AddRange(new object[8] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8" });
		((Control)cbo_COMM).Location = new Point(83, 112);
		((Control)cbo_COMM).Name = "cbo_COMM";
		((Control)cbo_COMM).Size = new Size(120, 20);
		((Control)cbo_COMM).TabIndex = 7;
		((Control)lbl_BaudRate).AutoSize = true;
		((Control)lbl_BaudRate).Location = new Point(18, 141);
		((Control)lbl_BaudRate).Name = "lbl_BaudRate";
		((Control)lbl_BaudRate).Size = new Size(59, 12);
		((Control)lbl_BaudRate).TabIndex = 8;
		((Control)lbl_BaudRate).Text = "Baudrate:";
		lbl_BaudRate.TextAlign = (ContentAlignment)32;
		((Control)lbl_COMM).AutoSize = true;
		((Control)lbl_COMM).Location = new Point(24, 115);
		((Control)lbl_COMM).Name = "lbl_COMM";
		((Control)lbl_COMM).Size = new Size(53, 12);
		((Control)lbl_COMM).TabIndex = 6;
		((Control)lbl_COMM).Text = "ComPort:";
		lbl_COMM.TextAlign = (ContentAlignment)32;
		((Control)rdb_TCP).AutoSize = true;
		((Control)rdb_TCP).Location = new Point(8, 164);
		((Control)rdb_TCP).Name = "rdb_TCP";
		((Control)rdb_TCP).Size = new Size(131, 16);
		((Control)rdb_TCP).TabIndex = 10;
		rdb_TCP.TabStop = true;
		((Control)rdb_TCP).Text = "Network Connection";
		((ButtonBase)rdb_TCP).TextAlign = (ContentAlignment)32;
		((ButtonBase)rdb_TCP).UseVisualStyleBackColor = true;
		((Control)rdb_COMM).AutoSize = true;
		((Control)rdb_COMM).Location = new Point(8, 90);
		((Control)rdb_COMM).Name = "rdb_COMM";
		((Control)rdb_COMM).Size = new Size(125, 16);
		((Control)rdb_COMM).TabIndex = 5;
		rdb_COMM.TabStop = true;
		((Control)rdb_COMM).Text = "Serial Connection";
		((ButtonBase)rdb_COMM).TextAlign = (ContentAlignment)32;
		((ButtonBase)rdb_COMM).UseVisualStyleBackColor = true;
		((Control)rdb_USB).AutoSize = true;
		rdb_USB.Checked = true;
		((Control)rdb_USB).Location = new Point(8, 68);
		((Control)rdb_USB).Name = "rdb_USB";
		((Control)rdb_USB).Size = new Size(107, 16);
		((Control)rdb_USB).TabIndex = 4;
		rdb_USB.TabStop = true;
		((Control)rdb_USB).Text = "USB Connection";
		((ButtonBase)rdb_USB).TextAlign = (ContentAlignment)32;
		((ButtonBase)rdb_USB).UseVisualStyleBackColor = true;
		((Control)txt_IP).Location = new Point(83, 186);
		((Control)txt_IP).Name = "txt_IP";
		((Control)txt_IP).Size = new Size(144, 21);
		((Control)txt_IP).TabIndex = 12;
		((Control)txt_IP).Text = "192.168.1.225";
		((Control)lbl_Port).AutoSize = true;
		((Control)lbl_Port).Location = new Point(18, 215);
		((Control)lbl_Port).Name = "lbl_Port";
		((Control)lbl_Port).Size = new Size(59, 12);
		((Control)lbl_Port).TabIndex = 13;
		((Control)lbl_Port).Text = "UDP Port:";
		lbl_Port.TextAlign = (ContentAlignment)32;
		((Control)lbl_IP).AutoSize = true;
		((Control)lbl_IP).Location = new Point(6, 190);
		((Control)lbl_IP).Name = "lbl_IP";
		((Control)lbl_IP).Size = new Size(71, 12);
		((Control)lbl_IP).TabIndex = 11;
		((Control)lbl_IP).Text = "IP Address:";
		lbl_IP.TextAlign = (ContentAlignment)32;
		((Control)lbl_ConnectionPwd).AutoSize = true;
		((Control)lbl_ConnectionPwd).Location = new Point(11, 43);
		((Control)lbl_ConnectionPwd).Name = "lbl_ConnectionPwd";
		((Control)lbl_ConnectionPwd).Size = new Size(59, 12);
		((Control)lbl_ConnectionPwd).TabIndex = 2;
		((Control)lbl_ConnectionPwd).Text = "Comm Key:";
		lbl_ConnectionPwd.TextAlign = (ContentAlignment)32;
		((Control)lbl_DeviceId).AutoSize = true;
		((Control)lbl_DeviceId).Location = new Point(6, 17);
		((Control)lbl_DeviceId).Name = "lbl_DeviceId";
		((Control)lbl_DeviceId).Size = new Size(65, 12);
		((Control)lbl_DeviceId).TabIndex = 0;
		((Control)lbl_DeviceId).Text = "Device ID:";
		lbl_DeviceId.TextAlign = (ContentAlignment)32;
		((Control)btn_OpenDevice).Location = new Point(256, 24);
		((Control)btn_OpenDevice).Name = "btn_OpenDevice";
		((Control)btn_OpenDevice).Size = new Size(95, 23);
		((Control)btn_OpenDevice).TabIndex = 1;
		((Control)btn_OpenDevice).Text = "Open Device";
		((ButtonBase)btn_OpenDevice).UseVisualStyleBackColor = true;
		((Control)btn_OpenDevice).Click += btn_OpenDevice_Click;
		((Control)btn_CloseDevice).Location = new Point(357, 24);
		((Control)btn_CloseDevice).Name = "btn_CloseDevice";
		((Control)btn_CloseDevice).Size = new Size(95, 23);
		((Control)btn_CloseDevice).TabIndex = 2;
		((Control)btn_CloseDevice).Text = "Close Device";
		((ButtonBase)btn_CloseDevice).UseVisualStyleBackColor = true;
		((Control)btn_CloseDevice).Click += btn_CloseDevice_Click;
		((Control)group).Controls.Add((Control)(object)btn_GlogManagement);
		((Control)group).Controls.Add((Control)(object)btn_SlogManagement);
		((Control)group).Controls.Add((Control)(object)btn_AlarmSetting);
		((Control)group).Controls.Add((Control)(object)btn_AttendanceSetting);
		((Control)group).Controls.Add((Control)(object)btn_EnrollManagement);
		((Control)group).Controls.Add((Control)(object)btn_SystemSetting);
		((Control)group).Location = new Point(256, 53);
		((Control)group).Name = "group";
		((Control)group).Size = new Size(333, 112);
		((Control)group).TabIndex = 3;
		group.TabStop = false;
		((Control)group).Text = "Function";
		((Control)btn_GlogManagement).Location = new Point(168, 78);
		((Control)btn_GlogManagement).Name = "btn_GlogManagement";
		((Control)btn_GlogManagement).Size = new Size(156, 23);
		((Control)btn_GlogManagement).TabIndex = 5;
		((Control)btn_GlogManagement).Text = "GLog Management";
		((ButtonBase)btn_GlogManagement).UseVisualStyleBackColor = true;
		((Control)btn_GlogManagement).Click += btn_GlogManagement_Click;
		((Control)btn_SlogManagement).Location = new Point(6, 78);
		((Control)btn_SlogManagement).Name = "btn_SlogManagement";
		((Control)btn_SlogManagement).Size = new Size(156, 23);
		((Control)btn_SlogManagement).TabIndex = 4;
		((Control)btn_SlogManagement).Text = "SLog Mangement";
		((ButtonBase)btn_SlogManagement).UseVisualStyleBackColor = true;
		((Control)btn_SlogManagement).Click += btn_SlogManagement_Click;
		((Control)btn_AlarmSetting).Location = new Point(168, 20);
		((Control)btn_AlarmSetting).Name = "btn_AlarmSetting";
		((Control)btn_AlarmSetting).Size = new Size(156, 23);
		((Control)btn_AlarmSetting).TabIndex = 1;
		((Control)btn_AlarmSetting).Text = "Alarm Settings";
		((ButtonBase)btn_AlarmSetting).UseVisualStyleBackColor = true;
		((Control)btn_AlarmSetting).Click += btn_AlarmSetting_Click;
		((Control)btn_AttendanceSetting).Location = new Point(6, 49);
		((Control)btn_AttendanceSetting).Name = "btn_AttendanceSetting";
		((Control)btn_AttendanceSetting).Size = new Size(156, 23);
		((Control)btn_AttendanceSetting).TabIndex = 2;
		((Control)btn_AttendanceSetting).Text = "Message Settings";
		((ButtonBase)btn_AttendanceSetting).UseVisualStyleBackColor = true;
		((Control)btn_AttendanceSetting).Click += btn_AttendanceSetting_Click;
		((Control)btn_EnrollManagement).Location = new Point(168, 49);
		((Control)btn_EnrollManagement).Name = "btn_EnrollManagement";
		((Control)btn_EnrollManagement).Size = new Size(156, 23);
		((Control)btn_EnrollManagement).TabIndex = 3;
		((Control)btn_EnrollManagement).Text = "Employee Management";
		((ButtonBase)btn_EnrollManagement).UseVisualStyleBackColor = true;
		((Control)btn_EnrollManagement).Click += btn_EnrollManagement_Click;
		((Control)btn_SystemSetting).Location = new Point(6, 20);
		((Control)btn_SystemSetting).Name = "btn_SystemSetting";
		((Control)btn_SystemSetting).Size = new Size(156, 23);
		((Control)btn_SystemSetting).TabIndex = 0;
		((Control)btn_SystemSetting).Text = "System Settings";
		((ButtonBase)btn_SystemSetting).UseVisualStyleBackColor = true;
		((Control)btn_SystemSetting).Click += btn_SystemSetting_Click;
		((Control)btn_RealTimeLog).Location = new Point(262, 171);
		((Control)btn_RealTimeLog).Name = "btn_RealTimeLog";
		((Control)btn_RealTimeLog).Size = new Size(156, 23);
		((Control)btn_RealTimeLog).TabIndex = 4;
		((Control)btn_RealTimeLog).Text = "Real Time Log";
		((ButtonBase)btn_RealTimeLog).UseVisualStyleBackColor = true;
		((Control)btn_RealTimeLog).Click += btn_RealTimeLog_Click;
		((Control)btn_NameDataSetting).Location = new Point(424, 171);
		((Control)btn_NameDataSetting).Name = "btn_NameDataSetting";
		((Control)btn_NameDataSetting).Size = new Size(156, 23);
		((Control)btn_NameDataSetting).TabIndex = 5;
		((Control)btn_NameDataSetting).Text = "Name Data Settings";
		((ButtonBase)btn_NameDataSetting).UseVisualStyleBackColor = true;
		((Control)btn_NameDataSetting).Click += btn_NameDataSetting_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(600, 264);
		((Control)this).Controls.Add((Control)(object)btn_NameDataSetting);
		((Control)this).Controls.Add((Control)(object)btn_RealTimeLog);
		((Control)this).Controls.Add((Control)(object)group);
		((Control)this).Controls.Add((Control)(object)btn_CloseDevice);
		((Control)this).Controls.Add((Control)(object)btn_OpenDevice);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "CMForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "ZDC2911";
		((Form)this).Load += CMForm_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((ISupportInitialize)nud_Port).EndInit();
		((ISupportInitialize)nud_Pwd).EndInit();
		((ISupportInitialize)nud_DN).EndInit();
		((Control)group).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
