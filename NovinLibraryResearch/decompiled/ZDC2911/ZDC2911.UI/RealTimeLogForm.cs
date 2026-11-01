using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911.IConvert;

namespace ZDC2911.UI;

public class RealTimeLogForm : Form
{
	private delegate void AddRecord(ListViewItem lvi);

	private int no = 1;

	private Zd2911Monitor listener;

	private IContainer components = null;

	private Label lbl_Port;

	private Button btn_Listen;

	private ListView lvw_Logs;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private ColumnHeader columnHeader3;

	private ColumnHeader columnHeader4;

	private ColumnHeader columnHeader5;

	private ColumnHeader columnHeader6;

	private ColumnHeader columnHeader7;

	private NumericUpDown nud_Port;

	private Label lbl_Mode;

	private ComboBox cbo_Mode;

	private Label lbl_SerialPort;

	private ComboBox cbo_SerialPort;

	private Label lbl_Baudrate;

	private ComboBox cbo_Baudrate;

	public RealTimeLogForm()
	{
		InitializeComponent();
	}

	private string GetLocalIPAddress()
	{
		IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
		IPAddress[] array = hostAddresses;
		foreach (IPAddress iPAddress in array)
		{
			if (iPAddress.AddressFamily.Equals(AddressFamily.InterNetwork))
			{
				return iPAddress.ToString();
			}
		}
		return string.Empty;
	}

	private void AddRecordToListView(ListViewItem lvi)
	{
		lvw_Logs.Items.Add(lvi);
	}

	private void btn_Listen_Click(object sender, EventArgs e)
	{
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		if (((Control)cbo_SerialPort).Enabled && -1 == ((ListControl)cbo_SerialPort).SelectedIndex)
		{
			MessageBox.Show("Please Selected Serail Port", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_SerialPort).Focus();
			return;
		}
		if (((Control)cbo_Baudrate).Enabled && -1 == ((ListControl)cbo_Baudrate).SelectedIndex)
		{
			MessageBox.Show("Please Selected Baudrate", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_Baudrate).Focus();
			return;
		}
		try
		{
			if (((Control)btn_Listen).Text.Trim().Equals("Listen"))
			{
				Monitor val = new Monitor();
				if (((ListControl)cbo_Mode).SelectedIndex == 0)
				{
					val.UDPAddress = GetLocalIPAddress();
					val.UDPPort = (int)nud_Port.Value;
					val.Mode = 0;
				}
				else
				{
					val.SerialPort = Convert.ToInt32(cbo_SerialPort.SelectedItem.ToString().Replace("COM", string.Empty));
					val.SerialBaudRate = Convert.ToInt32(cbo_Baudrate.SelectedItem);
					val.Mode = 1;
				}
				listener = Zd2911Monitor.CreateZd2911Monitor(val);
				listener.ReceiveHandler += new ReceiveHandler(listener_ReceiveHandler);
				listener.OpenListen();
				((Control)btn_Listen).Text = "Cancel";
			}
			else
			{
				listener.CloseListen();
				listener = null;
				((Control)btn_Listen).Text = "Listen";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void listener_ReceiveHandler(object sender, ReceiveEventArg e)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		Record record = e.record;
		string text = ConvertObject.GLogType(record.Verify);
		string text2 = ConvertObject.IOMode(record.Action);
		ListViewItem val = new ListViewItem(new string[7]
		{
			no.ToString(),
			record.DN.ToString(),
			record.DIN.ToString(),
			string.Empty,
			text,
			text2,
			record.Clock.ToString("yyyy-MM-dd HH:mm:ss")
		});
		((Control)this).BeginInvoke((Delegate)new AddRecord(AddRecordToListView), new object[1] { val });
		no++;
	}

	private void cbo_Mode_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (((ListControl)cbo_Mode).SelectedIndex)
		{
		case 0:
			((Control)nud_Port).Enabled = true;
			((Control)cbo_SerialPort).Enabled = false;
			((Control)cbo_Baudrate).Enabled = false;
			break;
		case 1:
		{
			((Control)nud_Port).Enabled = false;
			((Control)cbo_SerialPort).Enabled = true;
			((Control)cbo_Baudrate).Enabled = true;
			((ListControl)cbo_Baudrate).SelectedIndex = 0;
			string[] portNames = SerialPort.GetPortNames();
			if (portNames != null)
			{
				cbo_SerialPort.Items.Clear();
				string[] array = portNames;
				foreach (string text in array)
				{
					cbo_SerialPort.Items.Add((object)text);
				}
				((ListControl)cbo_SerialPort).SelectedIndex = 0;
			}
			break;
		}
		}
	}

	private void RealTimeLogForm_Load(object sender, EventArgs e)
	{
		((ListControl)cbo_Mode).SelectedIndex = 0;
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
		lbl_Port = new Label();
		btn_Listen = new Button();
		lvw_Logs = new ListView();
		columnHeader1 = new ColumnHeader();
		columnHeader2 = new ColumnHeader();
		columnHeader3 = new ColumnHeader();
		columnHeader4 = new ColumnHeader();
		columnHeader5 = new ColumnHeader();
		columnHeader6 = new ColumnHeader();
		columnHeader7 = new ColumnHeader();
		nud_Port = new NumericUpDown();
		lbl_Mode = new Label();
		cbo_Mode = new ComboBox();
		lbl_SerialPort = new Label();
		cbo_SerialPort = new ComboBox();
		lbl_Baudrate = new Label();
		cbo_Baudrate = new ComboBox();
		((ISupportInitialize)nud_Port).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lbl_Port).AutoSize = true;
		((Control)lbl_Port).Location = new Point(207, 14);
		((Control)lbl_Port).Name = "lbl_Port";
		((Control)lbl_Port).Size = new Size(59, 12);
		((Control)lbl_Port).TabIndex = 2;
		((Control)lbl_Port).Text = "UDP Port:";
		lbl_Port.TextAlign = (ContentAlignment)16;
		((Control)btn_Listen).Location = new Point(761, 8);
		((Control)btn_Listen).Name = "btn_Listen";
		((Control)btn_Listen).Size = new Size(75, 23);
		((Control)btn_Listen).TabIndex = 8;
		((Control)btn_Listen).Text = "Listen";
		((ButtonBase)btn_Listen).UseVisualStyleBackColor = true;
		((Control)btn_Listen).Click += btn_Listen_Click;
		lvw_Logs.Columns.AddRange((ColumnHeader[])(object)new ColumnHeader[7] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7 });
		lvw_Logs.FullRowSelect = true;
		lvw_Logs.GridLines = true;
		lvw_Logs.HideSelection = false;
		((Control)lvw_Logs).Location = new Point(12, 39);
		lvw_Logs.MultiSelect = false;
		((Control)lvw_Logs).Name = "lvw_Logs";
		((Control)lvw_Logs).Size = new Size(824, 503);
		((Control)lvw_Logs).TabIndex = 9;
		lvw_Logs.UseCompatibleStateImageBehavior = false;
		lvw_Logs.View = (View)1;
		columnHeader1.Text = "SN";
		columnHeader1.Width = 40;
		columnHeader2.Text = "Device No.";
		columnHeader2.TextAlign = (HorizontalAlignment)2;
		columnHeader2.Width = 90;
		columnHeader3.Text = "UserID";
		columnHeader3.TextAlign = (HorizontalAlignment)2;
		columnHeader3.Width = 120;
		columnHeader4.Text = "Name";
		columnHeader4.TextAlign = (HorizontalAlignment)2;
		columnHeader4.Width = 100;
		columnHeader5.Text = "Operation";
		columnHeader5.TextAlign = (HorizontalAlignment)2;
		columnHeader5.Width = 120;
		columnHeader6.Text = "Action";
		columnHeader6.TextAlign = (HorizontalAlignment)2;
		columnHeader6.Width = 150;
		columnHeader7.Text = "Datetime";
		columnHeader7.TextAlign = (HorizontalAlignment)2;
		columnHeader7.Width = 150;
		((Control)nud_Port).Location = new Point(272, 10);
		nud_Port.Maximum = new decimal(new int[4] { 99999, 0, 0, 0 });
		nud_Port.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)nud_Port).Name = "nud_Port";
		((Control)nud_Port).Size = new Size(57, 21);
		((Control)nud_Port).TabIndex = 3;
		nud_Port.Value = new decimal(new int[4] { 5055, 0, 0, 0 });
		((Control)lbl_Mode).AutoSize = true;
		((Control)lbl_Mode).Location = new Point(12, 15);
		((Control)lbl_Mode).Name = "lbl_Mode";
		((Control)lbl_Mode).Size = new Size(83, 12);
		((Control)lbl_Mode).TabIndex = 0;
		((Control)lbl_Mode).Text = "Monitor Mode:";
		lbl_Mode.TextAlign = (ContentAlignment)16;
		cbo_Mode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_Mode).FormattingEnabled = true;
		cbo_Mode.Items.AddRange(new object[2] { "UDP", "RS485" });
		((Control)cbo_Mode).Location = new Point(101, 11);
		((Control)cbo_Mode).Name = "cbo_Mode";
		((Control)cbo_Mode).Size = new Size(80, 20);
		((Control)cbo_Mode).TabIndex = 1;
		cbo_Mode.SelectedIndexChanged += cbo_Mode_SelectedIndexChanged;
		((Control)lbl_SerialPort).AutoSize = true;
		((Control)lbl_SerialPort).Location = new Point(356, 15);
		((Control)lbl_SerialPort).Name = "lbl_SerialPort";
		((Control)lbl_SerialPort).Size = new Size(77, 12);
		((Control)lbl_SerialPort).TabIndex = 4;
		((Control)lbl_SerialPort).Text = "Serial Port:";
		lbl_SerialPort.TextAlign = (ContentAlignment)16;
		cbo_SerialPort.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_SerialPort).FormattingEnabled = true;
		((Control)cbo_SerialPort).Location = new Point(439, 11);
		((Control)cbo_SerialPort).Name = "cbo_SerialPort";
		((Control)cbo_SerialPort).Size = new Size(70, 20);
		((Control)cbo_SerialPort).TabIndex = 5;
		((Control)lbl_Baudrate).AutoSize = true;
		((Control)lbl_Baudrate).Location = new Point(534, 14);
		((Control)lbl_Baudrate).Name = "lbl_Baudrate";
		((Control)lbl_Baudrate).Size = new Size(59, 12);
		((Control)lbl_Baudrate).TabIndex = 6;
		((Control)lbl_Baudrate).Text = "Baudrate:";
		lbl_Baudrate.TextAlign = (ContentAlignment)16;
		cbo_Baudrate.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_Baudrate).FormattingEnabled = true;
		cbo_Baudrate.Items.AddRange(new object[5] { "9600", "19200", "38400", "57600", "115200" });
		((Control)cbo_Baudrate).Location = new Point(599, 12);
		((Control)cbo_Baudrate).Name = "cbo_Baudrate";
		((Control)cbo_Baudrate).Size = new Size(121, 20);
		((Control)cbo_Baudrate).TabIndex = 7;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(848, 554);
		((Control)this).Controls.Add((Control)(object)cbo_Baudrate);
		((Control)this).Controls.Add((Control)(object)lbl_Baudrate);
		((Control)this).Controls.Add((Control)(object)cbo_SerialPort);
		((Control)this).Controls.Add((Control)(object)lbl_SerialPort);
		((Control)this).Controls.Add((Control)(object)cbo_Mode);
		((Control)this).Controls.Add((Control)(object)lbl_Mode);
		((Control)this).Controls.Add((Control)(object)nud_Port);
		((Control)this).Controls.Add((Control)(object)lvw_Logs);
		((Control)this).Controls.Add((Control)(object)btn_Listen);
		((Control)this).Controls.Add((Control)(object)lbl_Port);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "RealTimeLogForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Real Time Log";
		((Form)this).Load += RealTimeLogForm_Load;
		((ISupportInitialize)nud_Port).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
