using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911.Business;
using ZDC2911.Entity;

namespace ZDC2911.UI;

public class AlarmForm : Form
{
	private Device device;

	private DeviceConnection deviceConnection;

	private IContainer components = null;

	private GroupBox groupBox1;

	private ComboBox cbo_AlarmDN;

	private Label lbl_AlarmDN;

	private ComboBox cbo_AlarmMinute;

	private ComboBox cbo_AlarmHour;

	private Label label1;

	private Label lbl_AlarmDelay;

	private Label lbl_AlarmTime;

	private ComboBox cbo_AlarmCycle;

	private Label lbl_AlarmCycle;

	private Button btn_AlarmGet;

	private Button btn_AlarmSet;

	private NumericUpDown nud_AlarmDelay;

	public AlarmForm(DeviceCommEty deviceEty)
	{
		InitializeComponent();
		device = deviceEty.Device;
		deviceConnection = deviceEty.DeviceConnection;
	}

	private void AlarmForm_Load(object sender, EventArgs e)
	{
		InitData.InitAlarmDN(cbo_AlarmDN);
		InitData.InitHour(cbo_AlarmHour);
		InitData.InitMinuteOrSecond(cbo_AlarmMinute);
	}

	private void btn_AlarmGet_Click(object sender, EventArgs e)
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_AlarmDN).SelectedIndex)
		{
			MessageBox.Show("Please Select SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_AlarmDN).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = 65;
			if (deviceConnection.GetProperty((DeviceProperty)2, obj, ref device, ref obj2))
			{
				string s = (string)obj2;
				byte[] bytes = Encoding.Unicode.GetBytes(s);
				byte[] array = new byte[8];
				Array.Copy(bytes, ((ListControl)cbo_AlarmDN).SelectedIndex * 8, array, 0, array.Length);
				((ListControl)cbo_AlarmHour).SelectedIndex = array[0];
				((ListControl)cbo_AlarmMinute).SelectedIndex = array[1];
				((ListControl)cbo_AlarmCycle).SelectedIndex = array[2];
				nud_AlarmDelay.Value = array[3];
			}
			else
			{
				MessageBox.Show("Get Alarm Settings Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
		}
	}

	private void btn_AlarmSet_Click(object sender, EventArgs e)
	{
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_AlarmDN).SelectedIndex)
		{
			MessageBox.Show("Please Select SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_AlarmDN).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_AlarmHour).SelectedIndex)
		{
			MessageBox.Show("Please Select Time: Hour", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_AlarmHour).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_AlarmMinute).SelectedIndex)
		{
			MessageBox.Show("Please Select Time: Minute", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_AlarmMinute).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_AlarmCycle).SelectedIndex)
		{
			MessageBox.Show("Please Select Cycle", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_AlarmCycle).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = 65;
			if (!deviceConnection.GetProperty((DeviceProperty)2, obj, ref device, ref obj2))
			{
				MessageBox.Show("Set Alarm Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			string s = (string)obj2;
			byte[] bytes = Encoding.Unicode.GetBytes(s);
			bytes[((ListControl)cbo_AlarmDN).SelectedIndex * 8] = (byte)((ListControl)cbo_AlarmHour).SelectedIndex;
			bytes[((ListControl)cbo_AlarmDN).SelectedIndex * 8 + 1] = (byte)((ListControl)cbo_AlarmMinute).SelectedIndex;
			bytes[((ListControl)cbo_AlarmDN).SelectedIndex * 8 + 2] = (byte)((ListControl)cbo_AlarmCycle).SelectedIndex;
			bytes[((ListControl)cbo_AlarmDN).SelectedIndex * 8 + 3] = (byte)nud_AlarmDelay.Value;
			obj = 65;
			obj2 = Encoding.Unicode.GetString(bytes);
			if (deviceConnection.SetProperty((DeviceProperty)2, obj, device, obj2))
			{
				MessageBox.Show("Set Alarm Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set Alarm Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
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
		groupBox1 = new GroupBox();
		nud_AlarmDelay = new NumericUpDown();
		btn_AlarmGet = new Button();
		btn_AlarmSet = new Button();
		cbo_AlarmMinute = new ComboBox();
		cbo_AlarmHour = new ComboBox();
		label1 = new Label();
		lbl_AlarmDelay = new Label();
		lbl_AlarmTime = new Label();
		cbo_AlarmCycle = new ComboBox();
		cbo_AlarmDN = new ComboBox();
		lbl_AlarmCycle = new Label();
		lbl_AlarmDN = new Label();
		((Control)groupBox1).SuspendLayout();
		((ISupportInitialize)nud_AlarmDelay).BeginInit();
		((Control)this).SuspendLayout();
		((Control)groupBox1).Controls.Add((Control)(object)nud_AlarmDelay);
		((Control)groupBox1).Controls.Add((Control)(object)btn_AlarmGet);
		((Control)groupBox1).Controls.Add((Control)(object)btn_AlarmSet);
		((Control)groupBox1).Controls.Add((Control)(object)cbo_AlarmMinute);
		((Control)groupBox1).Controls.Add((Control)(object)cbo_AlarmHour);
		((Control)groupBox1).Controls.Add((Control)(object)label1);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_AlarmDelay);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_AlarmTime);
		((Control)groupBox1).Controls.Add((Control)(object)cbo_AlarmCycle);
		((Control)groupBox1).Controls.Add((Control)(object)cbo_AlarmDN);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_AlarmCycle);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_AlarmDN);
		((Control)groupBox1).Location = new Point(12, 12);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(544, 109);
		((Control)groupBox1).TabIndex = 0;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Alarm Settings";
		((Control)nud_AlarmDelay).Location = new Point(378, 49);
		nud_AlarmDelay.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		((Control)nud_AlarmDelay).Name = "nud_AlarmDelay";
		((Control)nud_AlarmDelay).Size = new Size(150, 21);
		((Control)nud_AlarmDelay).TabIndex = 9;
		((Control)btn_AlarmGet).Location = new Point(372, 75);
		((Control)btn_AlarmGet).Name = "btn_AlarmGet";
		((Control)btn_AlarmGet).Size = new Size(75, 23);
		((Control)btn_AlarmGet).TabIndex = 10;
		((Control)btn_AlarmGet).Text = "Get";
		((ButtonBase)btn_AlarmGet).UseVisualStyleBackColor = true;
		((Control)btn_AlarmGet).Click += btn_AlarmGet_Click;
		((Control)btn_AlarmSet).Location = new Point(453, 75);
		((Control)btn_AlarmSet).Name = "btn_AlarmSet";
		((Control)btn_AlarmSet).Size = new Size(75, 23);
		((Control)btn_AlarmSet).TabIndex = 11;
		((Control)btn_AlarmSet).Text = "Set";
		((ButtonBase)btn_AlarmSet).UseVisualStyleBackColor = true;
		((Control)btn_AlarmSet).Click += btn_AlarmSet_Click;
		cbo_AlarmMinute.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_AlarmMinute).FormattingEnabled = true;
		((Control)cbo_AlarmMinute).Location = new Point(468, 20);
		((Control)cbo_AlarmMinute).Name = "cbo_AlarmMinute";
		((Control)cbo_AlarmMinute).Size = new Size(60, 20);
		((Control)cbo_AlarmMinute).TabIndex = 5;
		cbo_AlarmHour.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_AlarmHour).FormattingEnabled = true;
		((Control)cbo_AlarmHour).Location = new Point(378, 20);
		((Control)cbo_AlarmHour).Name = "cbo_AlarmHour";
		((Control)cbo_AlarmHour).Size = new Size(60, 20);
		((Control)cbo_AlarmHour).TabIndex = 3;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(448, 23);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(17, 12);
		((Control)label1).TabIndex = 4;
		((Control)label1).Text = "：";
		label1.TextAlign = (ContentAlignment)32;
		((Control)lbl_AlarmDelay).AutoSize = true;
		((Control)lbl_AlarmDelay).Location = new Point(331, 51);
		((Control)lbl_AlarmDelay).Name = "lbl_AlarmDelay";
		((Control)lbl_AlarmDelay).Size = new Size(41, 12);
		((Control)lbl_AlarmDelay).TabIndex = 8;
		((Control)lbl_AlarmDelay).Text = "Delay:";
		lbl_AlarmDelay.TextAlign = (ContentAlignment)32;
		((Control)lbl_AlarmTime).AutoSize = true;
		((Control)lbl_AlarmTime).Location = new Point(337, 23);
		((Control)lbl_AlarmTime).Name = "lbl_AlarmTime";
		((Control)lbl_AlarmTime).Size = new Size(35, 12);
		((Control)lbl_AlarmTime).TabIndex = 2;
		((Control)lbl_AlarmTime).Text = "Time:";
		lbl_AlarmTime.TextAlign = (ContentAlignment)32;
		cbo_AlarmCycle.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_AlarmCycle).FormattingEnabled = true;
		cbo_AlarmCycle.Items.AddRange(new object[8] { "Every day", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
		((Control)cbo_AlarmCycle).Location = new Point(53, 48);
		((Control)cbo_AlarmCycle).Name = "cbo_AlarmCycle";
		((Control)cbo_AlarmCycle).Size = new Size(150, 20);
		((Control)cbo_AlarmCycle).TabIndex = 7;
		cbo_AlarmDN.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_AlarmDN).FormattingEnabled = true;
		((Control)cbo_AlarmDN).Location = new Point(53, 20);
		((Control)cbo_AlarmDN).Name = "cbo_AlarmDN";
		((Control)cbo_AlarmDN).Size = new Size(150, 20);
		((Control)cbo_AlarmDN).TabIndex = 1;
		((Control)lbl_AlarmCycle).AutoSize = true;
		((Control)lbl_AlarmCycle).Location = new Point(6, 51);
		((Control)lbl_AlarmCycle).Name = "lbl_AlarmCycle";
		((Control)lbl_AlarmCycle).Size = new Size(41, 12);
		((Control)lbl_AlarmCycle).TabIndex = 6;
		((Control)lbl_AlarmCycle).Text = "Cycle:";
		lbl_AlarmCycle.TextAlign = (ContentAlignment)32;
		((Control)lbl_AlarmDN).AutoSize = true;
		((Control)lbl_AlarmDN).Location = new Point(24, 23);
		((Control)lbl_AlarmDN).Name = "lbl_AlarmDN";
		((Control)lbl_AlarmDN).Size = new Size(23, 12);
		((Control)lbl_AlarmDN).TabIndex = 0;
		((Control)lbl_AlarmDN).Text = "SN:";
		lbl_AlarmDN.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(570, 333);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "AlarmForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Alarm Settings";
		((Form)this).Load += AlarmForm_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((ISupportInitialize)nud_AlarmDelay).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
