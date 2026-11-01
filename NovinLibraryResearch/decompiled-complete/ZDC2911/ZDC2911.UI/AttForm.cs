using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911.Business;
using ZDC2911.Entity;

namespace ZDC2911.UI;

public class AttForm : Form
{
	private Device device;

	private DeviceConnection deviceConnection;

	private IContainer components = null;

	private GroupBox groupBox3;

	private Label lbl_MessageBeginDate;

	private Label lbl_UserID;

	private ComboBox cbo_MessageSN;

	private Label lbl_MessageSN;

	private NumericUpDown nud_MessageID;

	private DateTimePicker dtp_MessageEndDatetime;

	private DateTimePicker dtp_MessageBeginDatetime;

	private Label lbl_MessageEndDate;

	private TextBox txt_MessageContent;

	private Label lbl_Content;

	private ComboBox cbo_MessageSound;

	private Label lbl_MessageSound;

	private Button btn_MessageSet;

	private Button btn_MessageGet;

	private CheckBox chk_MessageEnable;

	private ComboBox cbo_MessageType;

	private Label lbl_MessageType;

	public AttForm(DeviceCommEty deviceEty)
	{
		InitializeComponent();
		device = deviceEty.Device;
		deviceConnection = deviceEty.DeviceConnection;
	}

	private void AttForm_Load(object sender, EventArgs e)
	{
		InitData.InitMessageSN(cbo_MessageSN);
		dtp_MessageBeginDatetime.MinDate = InitData.MinDateTime;
		dtp_MessageBeginDatetime.MaxDate = InitData.MaxDateTime;
		dtp_MessageEndDatetime.MinDate = InitData.MinDateTime;
		dtp_MessageEndDatetime.MaxDate = InitData.MaxDateTime;
		dtp_MessageEndDatetime.Value = dtp_MessageBeginDatetime.Value.AddDays(1.0);
	}

	private void NumericUpDown_Selected(object sender, EventArgs e)
	{
		NumericUpDown val = (NumericUpDown)((sender is NumericUpDown) ? sender : null);
		int length = val.Value.ToString().Length;
		((UpDownBase)val).Select(0, length);
	}

	private void btn_MessageGet_Click(object sender, EventArgs e)
	{
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_MessageSN).SelectedIndex)
		{
			MessageBox.Show("Please select SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_MessageSN).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = 64;
			if (deviceConnection.GetProperty((DeviceProperty)15, obj, ref device, ref obj2))
			{
				byte[] bytes = Encoding.Unicode.GetBytes((string)obj2);
				byte[] array = new byte[84];
				Array.Copy(bytes, ((ListControl)cbo_MessageSN).SelectedIndex * 84, array, 0, array.Length);
				chk_MessageEnable.Checked = Convert.ToBoolean(array[0]);
				((ListControl)cbo_MessageType).SelectedIndex = array[1];
				((ListControl)cbo_MessageSound).SelectedIndex = array[2];
				dtp_MessageBeginDatetime.Value = new DateTime(array[6] + 2000, array[7], array[8], array[9], array[10], 0);
				dtp_MessageEndDatetime.Value = new DateTime(array[11] + 2000, array[12], array[13], array[14], array[15], 0);
				nud_MessageID.Value = BitConverter.ToUInt64(array, 16);
				((Control)txt_MessageContent).Text = Encoding.Unicode.GetString(array, 24, 60).Replace("\0", "");
			}
			else
			{
				MessageBox.Show("Get Message Settings Fail.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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

	private void btn_MessageSet_Click(object sender, EventArgs e)
	{
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_MessageSN).SelectedIndex)
		{
			MessageBox.Show("Please select SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_MessageSN).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_MessageType).SelectedIndex)
		{
			MessageBox.Show("Please select Type", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_MessageType).Focus();
			return;
		}
		if (((ListControl)cbo_MessageType).SelectedIndex != 0 && 0m == nud_MessageID.Value)
		{
			MessageBox.Show("Please input ID", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)nud_MessageID).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_MessageSound).SelectedIndex)
		{
			MessageBox.Show("Please select Sound", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_MessageSound).Focus();
			return;
		}
		if (string.IsNullOrEmpty(((Control)txt_MessageContent).Text.Trim()))
		{
			MessageBox.Show("Please Input Content.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txt_MessageContent).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = 64;
			if (!deviceConnection.GetProperty((DeviceProperty)15, obj, ref device, ref obj2))
			{
				MessageBox.Show("Set Message Fail.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			byte[] bytes = Encoding.Unicode.GetBytes((string)obj2);
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84] = Convert.ToByte(chk_MessageEnable.Checked);
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 1] = (byte)((ListControl)cbo_MessageType).SelectedIndex;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 2] = (byte)((ListControl)cbo_MessageSound).SelectedIndex;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 3] = 0;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 4] = 0;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 5] = 0;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 6] = (byte)(dtp_MessageBeginDatetime.Value.Year - 2000);
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 7] = (byte)dtp_MessageBeginDatetime.Value.Month;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 8] = (byte)dtp_MessageBeginDatetime.Value.Day;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 9] = (byte)dtp_MessageBeginDatetime.Value.Hour;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 10] = (byte)dtp_MessageBeginDatetime.Value.Minute;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 11] = (byte)(dtp_MessageEndDatetime.Value.Year - 2000);
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 12] = (byte)dtp_MessageEndDatetime.Value.Month;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 13] = (byte)dtp_MessageEndDatetime.Value.Day;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 14] = (byte)dtp_MessageEndDatetime.Value.Hour;
			bytes[((ListControl)cbo_MessageSN).SelectedIndex * 84 + 15] = (byte)dtp_MessageEndDatetime.Value.Minute;
			byte[] bytes2 = BitConverter.GetBytes((ulong)nud_MessageID.Value);
			Array.Copy(bytes2, 0, bytes, ((ListControl)cbo_MessageSN).SelectedIndex * 84 + 16, bytes2.Length);
			byte[] bytes3 = Encoding.Unicode.GetBytes(((Control)txt_MessageContent).Text.Trim());
			byte[] array = new byte[60];
			Array.Copy(bytes3, 0, array, 0, bytes3.Length);
			Array.Copy(array, 0, bytes, ((ListControl)cbo_MessageSN).SelectedIndex * 84 + 24, array.Length);
			obj2 = Encoding.Unicode.GetString(bytes);
			obj = 64;
			if (deviceConnection.SetProperty((DeviceProperty)15, obj, device, obj2))
			{
				MessageBox.Show("Set Message Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set Message Fail.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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
		groupBox3 = new GroupBox();
		chk_MessageEnable = new CheckBox();
		btn_MessageSet = new Button();
		btn_MessageGet = new Button();
		txt_MessageContent = new TextBox();
		lbl_Content = new Label();
		cbo_MessageType = new ComboBox();
		lbl_MessageType = new Label();
		cbo_MessageSound = new ComboBox();
		lbl_MessageSound = new Label();
		dtp_MessageEndDatetime = new DateTimePicker();
		dtp_MessageBeginDatetime = new DateTimePicker();
		nud_MessageID = new NumericUpDown();
		lbl_MessageEndDate = new Label();
		lbl_MessageBeginDate = new Label();
		lbl_UserID = new Label();
		cbo_MessageSN = new ComboBox();
		lbl_MessageSN = new Label();
		((Control)groupBox3).SuspendLayout();
		((ISupportInitialize)nud_MessageID).BeginInit();
		((Control)this).SuspendLayout();
		((Control)groupBox3).Controls.Add((Control)(object)chk_MessageEnable);
		((Control)groupBox3).Controls.Add((Control)(object)btn_MessageSet);
		((Control)groupBox3).Controls.Add((Control)(object)btn_MessageGet);
		((Control)groupBox3).Controls.Add((Control)(object)txt_MessageContent);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_Content);
		((Control)groupBox3).Controls.Add((Control)(object)cbo_MessageType);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_MessageType);
		((Control)groupBox3).Controls.Add((Control)(object)cbo_MessageSound);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_MessageSound);
		((Control)groupBox3).Controls.Add((Control)(object)dtp_MessageEndDatetime);
		((Control)groupBox3).Controls.Add((Control)(object)dtp_MessageBeginDatetime);
		((Control)groupBox3).Controls.Add((Control)(object)nud_MessageID);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_MessageEndDate);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_MessageBeginDate);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_UserID);
		((Control)groupBox3).Controls.Add((Control)(object)cbo_MessageSN);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_MessageSN);
		((Control)groupBox3).Location = new Point(12, 12);
		((Control)groupBox3).Name = "groupBox3";
		((Control)groupBox3).Size = new Size(532, 187);
		((Control)groupBox3).TabIndex = 0;
		groupBox3.TabStop = false;
		((Control)groupBox3).Text = "Message Settings";
		((Control)chk_MessageEnable).AutoSize = true;
		((Control)chk_MessageEnable).Location = new Point(343, 22);
		((Control)chk_MessageEnable).Name = "chk_MessageEnable";
		((Control)chk_MessageEnable).Size = new Size(60, 16);
		((Control)chk_MessageEnable).TabIndex = 2;
		((Control)chk_MessageEnable).Text = "Enable";
		((ButtonBase)chk_MessageEnable).UseVisualStyleBackColor = true;
		((Control)btn_MessageSet).Location = new Point(445, 152);
		((Control)btn_MessageSet).Name = "btn_MessageSet";
		((Control)btn_MessageSet).Size = new Size(75, 23);
		((Control)btn_MessageSet).TabIndex = 16;
		((Control)btn_MessageSet).Text = "Set";
		((ButtonBase)btn_MessageSet).UseVisualStyleBackColor = true;
		((Control)btn_MessageSet).Click += btn_MessageSet_Click;
		((Control)btn_MessageGet).Location = new Point(364, 152);
		((Control)btn_MessageGet).Name = "btn_MessageGet";
		((Control)btn_MessageGet).Size = new Size(75, 23);
		((Control)btn_MessageGet).TabIndex = 15;
		((Control)btn_MessageGet).Text = "Get";
		((ButtonBase)btn_MessageGet).UseVisualStyleBackColor = true;
		((Control)btn_MessageGet).Click += btn_MessageGet_Click;
		((Control)txt_MessageContent).Location = new Point(107, 125);
		((TextBoxBase)txt_MessageContent).MaxLength = 30;
		((Control)txt_MessageContent).Name = "txt_MessageContent";
		((Control)txt_MessageContent).Size = new Size(413, 21);
		((Control)txt_MessageContent).TabIndex = 14;
		((Control)lbl_Content).AutoSize = true;
		((Control)lbl_Content).Location = new Point(48, 128);
		((Control)lbl_Content).Name = "lbl_Content";
		((Control)lbl_Content).Size = new Size(53, 12);
		((Control)lbl_Content).TabIndex = 13;
		((Control)lbl_Content).Text = "Content:";
		lbl_Content.TextAlign = (ContentAlignment)16;
		cbo_MessageType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_MessageType).FormattingEnabled = true;
		cbo_MessageType.Items.AddRange(new object[3] { "All", "User", "Group" });
		((Control)cbo_MessageType).Location = new Point(107, 46);
		((Control)cbo_MessageType).Name = "cbo_MessageType";
		((Control)cbo_MessageType).Size = new Size(150, 20);
		((Control)cbo_MessageType).TabIndex = 4;
		((Control)lbl_MessageType).AutoSize = true;
		((Control)lbl_MessageType).Location = new Point(66, 49);
		((Control)lbl_MessageType).Name = "lbl_MessageType";
		((Control)lbl_MessageType).Size = new Size(35, 12);
		((Control)lbl_MessageType).TabIndex = 3;
		((Control)lbl_MessageType).Text = "Type:";
		lbl_MessageType.TextAlign = (ContentAlignment)16;
		cbo_MessageSound.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_MessageSound).FormattingEnabled = true;
		cbo_MessageSound.Items.AddRange(new object[3] { "Default", "User Defined 1", "User Defined 2" });
		((Control)cbo_MessageSound).Location = new Point(107, 99);
		((Control)cbo_MessageSound).Name = "cbo_MessageSound";
		((Control)cbo_MessageSound).Size = new Size(150, 20);
		((Control)cbo_MessageSound).TabIndex = 12;
		((Control)lbl_MessageSound).AutoSize = true;
		((Control)lbl_MessageSound).Location = new Point(60, 102);
		((Control)lbl_MessageSound).Name = "lbl_MessageSound";
		((Control)lbl_MessageSound).Size = new Size(41, 12);
		((Control)lbl_MessageSound).TabIndex = 11;
		((Control)lbl_MessageSound).Text = "Sound:";
		lbl_MessageSound.TextAlign = (ContentAlignment)16;
		dtp_MessageEndDatetime.CustomFormat = "yyyy-MM-dd HH:mm";
		dtp_MessageEndDatetime.Format = (DateTimePickerFormat)8;
		((Control)dtp_MessageEndDatetime).Location = new Point(370, 72);
		dtp_MessageEndDatetime.MaxDate = new DateTime(2099, 12, 31, 0, 0, 0, 0);
		dtp_MessageEndDatetime.MinDate = new DateTime(2010, 1, 1, 0, 0, 0, 0);
		((Control)dtp_MessageEndDatetime).Name = "dtp_MessageEndDatetime";
		((Control)dtp_MessageEndDatetime).Size = new Size(150, 21);
		((Control)dtp_MessageEndDatetime).TabIndex = 10;
		dtp_MessageBeginDatetime.CustomFormat = "yyyy-MM-dd HH:mm";
		dtp_MessageBeginDatetime.Format = (DateTimePickerFormat)8;
		((Control)dtp_MessageBeginDatetime).Location = new Point(107, 72);
		dtp_MessageBeginDatetime.MaxDate = new DateTime(2099, 12, 31, 0, 0, 0, 0);
		dtp_MessageBeginDatetime.MinDate = new DateTime(2010, 1, 1, 0, 0, 0, 0);
		((Control)dtp_MessageBeginDatetime).Name = "dtp_MessageBeginDatetime";
		((Control)dtp_MessageBeginDatetime).Size = new Size(150, 21);
		((Control)dtp_MessageBeginDatetime).TabIndex = 8;
		((Control)nud_MessageID).Location = new Point(370, 44);
		nud_MessageID.Maximum = new decimal(new int[4] { -1486618625, 232830643, 0, 0 });
		((Control)nud_MessageID).Name = "nud_MessageID";
		((Control)nud_MessageID).Size = new Size(150, 21);
		((Control)nud_MessageID).TabIndex = 6;
		((Control)nud_MessageID).Click += NumericUpDown_Selected;
		((Control)nud_MessageID).Enter += NumericUpDown_Selected;
		((Control)lbl_MessageEndDate).AutoSize = true;
		((Control)lbl_MessageEndDate).Location = new Point(281, 78);
		((Control)lbl_MessageEndDate).Name = "lbl_MessageEndDate";
		((Control)lbl_MessageEndDate).Size = new Size(83, 12);
		((Control)lbl_MessageEndDate).TabIndex = 9;
		((Control)lbl_MessageEndDate).Text = "End Datetime:";
		lbl_MessageEndDate.TextAlign = (ContentAlignment)32;
		((Control)lbl_MessageBeginDate).AutoSize = true;
		((Control)lbl_MessageBeginDate).Location = new Point(6, 78);
		((Control)lbl_MessageBeginDate).Name = "lbl_MessageBeginDate";
		((Control)lbl_MessageBeginDate).Size = new Size(95, 12);
		((Control)lbl_MessageBeginDate).TabIndex = 7;
		((Control)lbl_MessageBeginDate).Text = "Begin Datetime:";
		lbl_MessageBeginDate.TextAlign = (ContentAlignment)32;
		((Control)lbl_UserID).AutoSize = true;
		((Control)lbl_UserID).Location = new Point(341, 49);
		((Control)lbl_UserID).Name = "lbl_UserID";
		((Control)lbl_UserID).Size = new Size(23, 12);
		((Control)lbl_UserID).TabIndex = 5;
		((Control)lbl_UserID).Text = "ID:";
		lbl_UserID.TextAlign = (ContentAlignment)16;
		cbo_MessageSN.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_MessageSN).FormattingEnabled = true;
		((Control)cbo_MessageSN).Location = new Point(107, 20);
		((Control)cbo_MessageSN).Name = "cbo_MessageSN";
		((Control)cbo_MessageSN).Size = new Size(150, 20);
		((Control)cbo_MessageSN).TabIndex = 1;
		((Control)lbl_MessageSN).AutoSize = true;
		((Control)lbl_MessageSN).Location = new Point(78, 23);
		((Control)lbl_MessageSN).Name = "lbl_MessageSN";
		((Control)lbl_MessageSN).Size = new Size(23, 12);
		((Control)lbl_MessageSN).TabIndex = 0;
		((Control)lbl_MessageSN).Text = "SN:";
		lbl_MessageSN.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(557, 497);
		((Control)this).Controls.Add((Control)(object)groupBox3);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "AttForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Message Settings";
		((Form)this).Load += AttForm_Load;
		((Control)groupBox3).ResumeLayout(false);
		((Control)groupBox3).PerformLayout();
		((ISupportInitialize)nud_MessageID).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
