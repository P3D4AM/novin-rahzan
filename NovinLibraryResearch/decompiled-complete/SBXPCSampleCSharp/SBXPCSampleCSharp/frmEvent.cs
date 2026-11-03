using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmEvent : Form
{
	private AxSBXPC bpc;

	private IContainer components = null;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	public Button cmdStartMoniter;

	public Button cmdEndMoniter;

	public Button cmdClear;

	private RadioButton radioSerialDevice;

	private RadioButton radioNetworkDevice;

	public Label label1;

	public Label lblEnrollData;

	public Label label2;

	public Label label3;

	private ComboBox cmbComPort;

	private TextBox txtPortNumber;

	private TextBox txtSourceIP;

	private ComboBox cmbBaudrate;

	private ListBox lstEvents;

	public frmEvent(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void frmEvent_FormClosing(object sender, FormClosingEventArgs e)
	{
		((CancelEventArgs)(object)e).Cancel = true;
		((Control)this).Visible = false;
	}

	private void frmEvent_Load(object sender, EventArgs e)
	{
		((ListControl)cmbComPort).SelectedIndex = 0;
		((ListControl)cmbBaudrate).SelectedIndex = 4;
		SwitchMode(bComport: true);
	}

	private void SwitchMode(bool bComport)
	{
		((Control)cmbComPort).Enabled = !bComport;
		((Control)cmbBaudrate).Enabled = !bComport;
		((Control)txtSourceIP).Enabled = bComport;
		((Control)txtPortNumber).Enabled = bComport;
	}

	public void ReceiveEvent(string eventXML)
	{
		string lpszValue = "";
		string lpszValue2 = "";
		string lpszValue3 = "";
		string lpszValue4 = "";
		string lpszValue5 = "";
		string lpszValue6 = "";
		string lpszXML = eventXML;
		int num = bpc.XML_ParseInt(ref lpszXML, "Year");
		int num2 = bpc.XML_ParseInt(ref lpszXML, "Month");
		int num3 = bpc.XML_ParseInt(ref lpszXML, "Day");
		int num4 = bpc.XML_ParseInt(ref lpszXML, "Hour");
		int num5 = bpc.XML_ParseInt(ref lpszXML, "Minute");
		int num6 = bpc.XML_ParseInt(ref lpszXML, "Second");
		int num7 = bpc.XML_ParseInt(ref lpszXML, "Weekday");
		int num8 = bpc.XML_ParseInt(ref lpszXML, "MachineID");
		bpc.XML_ParseString(ref lpszXML, "MachineType", ref lpszValue);
		bpc.XML_ParseString(ref lpszXML, "EventType", ref lpszValue2);
		string text = $"{num:D2}-";
		text += $"{num2:D2}-";
		text += $"{num3:D2} ";
		text += $"{num4:D2}:";
		text += $"{num5:D2}:";
		text += $"{num6:D2} ";
		text = text + "[" + lpszValue + ":";
		text = text + num8 + "] ";
		text = text + lpszValue2 + ", ";
		switch (lpszValue2)
		{
		case "Management Log":
		{
			int num10 = bpc.XML_ParseInt(ref lpszXML, "ManagerID");
			int num9 = bpc.XML_ParseInt(ref lpszXML, "UserID");
			bpc.XML_ParseString(ref lpszXML, "Action", ref lpszValue3);
			int num11 = bpc.XML_ParseLong(ref lpszXML, "Result");
			text = text + "Manager ID = " + $"{num10:D5}, ";
			text = text + "User ID = " + $"{num9:D5}, ";
			text = text + "Action = " + lpszValue3 + ", ";
			text = text + "Result = " + $"{num11:D}";
			break;
		}
		case "Time Log":
		{
			int num9 = bpc.XML_ParseInt(ref lpszXML, "UserID");
			bpc.XML_ParseString(ref lpszXML, "AttendanceStatus", ref lpszValue3);
			bpc.XML_ParseString(ref lpszXML, "VerificationMode", ref lpszValue4);
			bpc.XML_ParseString(ref lpszXML, "AntipassStatus", ref lpszValue5);
			bpc.XML_ParseString(ref lpszXML, "Photo", ref lpszValue6);
			text = text + "User ID = " + $"{num9:D5}, ";
			text = text + "AttendanceStatus = " + lpszValue3 + ", ";
			text = text + "VerificationMode = " + lpszValue4 + ", ";
			text = text + "AntipassStatus = " + lpszValue5 + ", ";
			text = text + "Photo = " + lpszValue6;
			break;
		}
		case "Verification Success":
		{
			int num9 = bpc.XML_ParseInt(ref lpszXML, "UserID");
			bpc.XML_ParseString(ref lpszXML, "VerificationMode", ref lpszValue3);
			text = text + "User ID = " + $"{num9:D5}, ";
			text = text + "VerificationMode = " + lpszValue3;
			break;
		}
		case "Verification Failure":
		{
			int num9 = bpc.XML_ParseInt(ref lpszXML, "UserID");
			bpc.XML_ParseString(ref lpszXML, "VerificationMode", ref lpszValue3);
			bpc.XML_ParseString(ref lpszXML, "ReasonOfFailure", ref lpszValue4);
			text = text + "User ID = " + $"{num9:D5}, ";
			text = text + "VerificationMode = " + lpszValue3 + ", ";
			text = text + "ReasonOfFailure = " + lpszValue4;
			break;
		}
		case "Alarm On":
		{
			int num9 = bpc.XML_ParseInt(ref lpszXML, "UserID");
			bpc.XML_ParseString(ref lpszXML, "AlarmType", ref lpszValue3);
			text = text + "User ID = " + $"{num9:D5}, ";
			text = text + "AlarmType = " + lpszValue3;
			break;
		}
		case "Alarm Off":
		{
			int num9 = bpc.XML_ParseInt(ref lpszXML, "UserID");
			bpc.XML_ParseString(ref lpszXML, "AlarmType", ref lpszValue3);
			bpc.XML_ParseString(ref lpszXML, "AlarmOffMethod", ref lpszValue4);
			text = text + "User ID = " + $"{num9:D5}";
			text = text + "AlarmType = " + lpszValue3 + ", ";
			text = text + "AlarmOffMethod = " + lpszValue4;
			break;
		}
		case "DoorBell":
			bpc.XML_ParseString(ref lpszXML, "InputType", ref lpszValue3);
			text = text + "Input Type = " + lpszValue3;
			break;
		}
		lstEvents.Items.Add((object)text);
	}

	private void cmbEndMoniter_Click(object sender, EventArgs e)
	{
		bpc.StopEventCapture();
		((Control)cmdStartMoniter).Enabled = true;
		((Control)cmdEndMoniter).Enabled = false;
	}

	private void cmdClear_Click(object sender, EventArgs e)
	{
		lstEvents.Items.Clear();
	}

	private void cmdStartMoniter_Click(object sender, EventArgs e)
	{
		if (radioNetworkDevice.Checked)
		{
			bool flag = bpc.StartEventCapture(0, util.pubIPAddrToLong(((Control)txtSourceIP).Text), Convert.ToInt32(((Control)txtPortNumber).Text));
		}
		else
		{
			bool flag = bpc.StartEventCapture(1, ((ListControl)cmbComPort).SelectedIndex + 1, Convert.ToInt32(((Control)cmbBaudrate).Text));
		}
		((Control)cmdStartMoniter).Enabled = false;
		((Control)cmdEndMoniter).Enabled = true;
	}

	private void radioNetwork_Click(object sender, EventArgs e)
	{
		if (radioNetworkDevice.Checked)
		{
			SwitchMode(bComport: true);
			radioSerialDevice.Checked = false;
		}
	}

	private void radioSerial_Click(object sender, EventArgs e)
	{
		if (radioSerialDevice.Checked)
		{
			SwitchMode(bComport: false);
			radioNetworkDevice.Checked = false;
		}
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
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Expected O, but got Unknown
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Expected O, but got Unknown
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Expected O, but got Unknown
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Expected O, but got Unknown
		//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c03: Expected O, but got Unknown
		//IL_0d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4e: Expected O, but got Unknown
		groupBox1 = new GroupBox();
		cmbBaudrate = new ComboBox();
		cmbComPort = new ComboBox();
		label1 = new Label();
		lblEnrollData = new Label();
		radioSerialDevice = new RadioButton();
		groupBox2 = new GroupBox();
		txtPortNumber = new TextBox();
		txtSourceIP = new TextBox();
		label2 = new Label();
		label3 = new Label();
		radioNetworkDevice = new RadioButton();
		cmdStartMoniter = new Button();
		cmdEndMoniter = new Button();
		cmdClear = new Button();
		lstEvents = new ListBox();
		((Control)groupBox1).SuspendLayout();
		((Control)groupBox2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupBox1).Controls.Add((Control)(object)cmbBaudrate);
		((Control)groupBox1).Controls.Add((Control)(object)cmbComPort);
		((Control)groupBox1).Controls.Add((Control)(object)label1);
		((Control)groupBox1).Controls.Add((Control)(object)lblEnrollData);
		((Control)groupBox1).Controls.Add((Control)(object)radioSerialDevice);
		((Control)groupBox1).Font = new Font("Times New Roman", 12f);
		((Control)groupBox1).ForeColor = Color.Black;
		((Control)groupBox1).Location = new Point(51, 23);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(289, 112);
		((Control)groupBox1).TabIndex = 0;
		groupBox1.TabStop = false;
		((ListControl)cmbBaudrate).FormattingEnabled = true;
		cmbBaudrate.Items.AddRange(new object[5] { "9600", "19200", "38400", "57600", "115200" });
		((Control)cmbBaudrate).Location = new Point(115, 70);
		((Control)cmbBaudrate).Name = "cmbBaudrate";
		((Control)cmbBaudrate).Size = new Size(132, 27);
		((Control)cmbBaudrate).TabIndex = 74;
		((ListControl)cmbComPort).DisplayMember = "0";
		((ListControl)cmbComPort).FormattingEnabled = true;
		cmbComPort.Items.AddRange(new object[8] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8" });
		((Control)cmbComPort).Location = new Point(115, 37);
		((Control)cmbComPort).Name = "cmbComPort";
		((Control)cmbComPort).Size = new Size(132, 27);
		((Control)cmbComPort).TabIndex = 73;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = SystemColors.Control;
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = SystemColors.ControlText;
		((Control)label1).Location = new Point(29, 74);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)0;
		((Control)label1).Size = new Size(67, 19);
		((Control)label1).TabIndex = 72;
		((Control)label1).Text = "Baudrate:";
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(29, 40);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(68, 19);
		((Control)lblEnrollData).TabIndex = 71;
		((Control)lblEnrollData).Text = "ComPort:";
		((Control)radioSerialDevice).AutoSize = true;
		((Control)radioSerialDevice).Location = new Point(6, 0);
		((Control)radioSerialDevice).Name = "radioSerialDevice";
		((Control)radioSerialDevice).Size = new Size(107, 23);
		((Control)radioSerialDevice).TabIndex = 1;
		radioSerialDevice.TabStop = true;
		((Control)radioSerialDevice).Text = "Serial Device";
		((ButtonBase)radioSerialDevice).UseVisualStyleBackColor = true;
		((Control)radioSerialDevice).Click += radioSerial_Click;
		((Control)groupBox2).Controls.Add((Control)(object)txtPortNumber);
		((Control)groupBox2).Controls.Add((Control)(object)txtSourceIP);
		((Control)groupBox2).Controls.Add((Control)(object)label2);
		((Control)groupBox2).Controls.Add((Control)(object)label3);
		((Control)groupBox2).Controls.Add((Control)(object)radioNetworkDevice);
		((Control)groupBox2).Font = new Font("Times New Roman", 12f);
		((Control)groupBox2).ForeColor = Color.Black;
		((Control)groupBox2).Location = new Point(411, 23);
		((Control)groupBox2).Name = "groupBox2";
		((Control)groupBox2).Size = new Size(289, 112);
		((Control)groupBox2).TabIndex = 1;
		groupBox2.TabStop = false;
		((Control)txtPortNumber).Location = new Point(132, 71);
		((Control)txtPortNumber).Name = "txtPortNumber";
		((Control)txtPortNumber).Size = new Size(134, 26);
		((Control)txtPortNumber).TabIndex = 76;
		((Control)txtPortNumber).Text = "5005";
		((Control)txtSourceIP).Location = new Point(132, 39);
		((Control)txtSourceIP).Name = "txtSourceIP";
		((Control)txtSourceIP).Size = new Size(134, 26);
		((Control)txtSourceIP).TabIndex = 75;
		((Control)txtSourceIP).Text = "0.0.0.0";
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = SystemColors.Control;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(30, 73);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(92, 19);
		((Control)label2).TabIndex = 74;
		((Control)label2).Text = "Port Number:";
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = SystemColors.Control;
		((Control)label3).Cursor = Cursors.Default;
		((Control)label3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = SystemColors.ControlText;
		((Control)label3).Location = new Point(30, 39);
		((Control)label3).Name = "label3";
		((Control)label3).RightToLeft = (RightToLeft)0;
		((Control)label3).Size = new Size(73, 19);
		((Control)label3).TabIndex = 73;
		((Control)label3).Text = "Source IP:";
		((Control)radioNetworkDevice).AutoSize = true;
		radioNetworkDevice.Checked = true;
		((Control)radioNetworkDevice).Location = new Point(6, 0);
		((Control)radioNetworkDevice).Name = "radioNetworkDevice";
		((Control)radioNetworkDevice).Size = new Size(128, 23);
		((Control)radioNetworkDevice).TabIndex = 0;
		radioNetworkDevice.TabStop = true;
		((Control)radioNetworkDevice).Text = "Network Device";
		((ButtonBase)radioNetworkDevice).UseVisualStyleBackColor = true;
		((Control)radioNetworkDevice).Click += radioNetwork_Click;
		((Control)cmdStartMoniter).BackColor = SystemColors.Control;
		((Control)cmdStartMoniter).Cursor = Cursors.Default;
		((Control)cmdStartMoniter).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdStartMoniter).ForeColor = SystemColors.ControlText;
		((Control)cmdStartMoniter).Location = new Point(126, 155);
		((Control)cmdStartMoniter).Name = "cmdStartMoniter";
		((Control)cmdStartMoniter).RightToLeft = (RightToLeft)0;
		((Control)cmdStartMoniter).Size = new Size(149, 30);
		((Control)cmdStartMoniter).TabIndex = 73;
		((Control)cmdStartMoniter).Text = "Start Moniter";
		((ButtonBase)cmdStartMoniter).UseVisualStyleBackColor = false;
		((Control)cmdStartMoniter).Click += cmdStartMoniter_Click;
		((Control)cmdEndMoniter).BackColor = SystemColors.Control;
		((Control)cmdEndMoniter).Cursor = Cursors.Default;
		((Control)cmdEndMoniter).Enabled = false;
		((Control)cmdEndMoniter).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdEndMoniter).ForeColor = SystemColors.ControlText;
		((Control)cmdEndMoniter).Location = new Point(298, 155);
		((Control)cmdEndMoniter).Name = "cmdEndMoniter";
		((Control)cmdEndMoniter).RightToLeft = (RightToLeft)0;
		((Control)cmdEndMoniter).Size = new Size(149, 30);
		((Control)cmdEndMoniter).TabIndex = 74;
		((Control)cmdEndMoniter).Text = "End Moniter";
		((ButtonBase)cmdEndMoniter).UseVisualStyleBackColor = false;
		((Control)cmdEndMoniter).Click += cmbEndMoniter_Click;
		((Control)cmdClear).BackColor = SystemColors.Control;
		((Control)cmdClear).Cursor = Cursors.Default;
		((Control)cmdClear).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdClear).ForeColor = SystemColors.ControlText;
		((Control)cmdClear).Location = new Point(472, 155);
		((Control)cmdClear).Name = "cmdClear";
		((Control)cmdClear).RightToLeft = (RightToLeft)0;
		((Control)cmdClear).Size = new Size(149, 30);
		((Control)cmdClear).TabIndex = 75;
		((Control)cmdClear).Text = "Clear";
		((ButtonBase)cmdClear).UseVisualStyleBackColor = false;
		((Control)cmdClear).Click += cmdClear_Click;
		((Control)lstEvents).Font = new Font("Times New Roman", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((ListControl)lstEvents).FormattingEnabled = true;
		lstEvents.ItemHeight = 15;
		((Control)lstEvents).Location = new Point(17, 208);
		((Control)lstEvents).Name = "lstEvents";
		((Control)lstEvents).Size = new Size(719, 274);
		((Control)lstEvents).TabIndex = 76;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(750, 505);
		((Control)this).Controls.Add((Control)(object)lstEvents);
		((Control)this).Controls.Add((Control)(object)cmdClear);
		((Control)this).Controls.Add((Control)(object)cmdEndMoniter);
		((Control)this).Controls.Add((Control)(object)cmdStartMoniter);
		((Control)this).Controls.Add((Control)(object)groupBox2);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Name = "frmEvent";
		((Control)this).Text = "frmEvent";
		((Form)this).Load += frmEvent_Load;
		((Form)this).FormClosing += new FormClosingEventHandler(frmEvent_FormClosing);
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)groupBox2).ResumeLayout(false);
		((Control)groupBox2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
