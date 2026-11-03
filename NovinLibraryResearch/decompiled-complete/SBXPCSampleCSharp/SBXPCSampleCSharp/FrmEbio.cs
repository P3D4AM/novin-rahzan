using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class FrmEbio : Form
{
	private FrmMonitoringEbio frmMonitoringEbio;

	private bool mOpenFlag;

	private AxSBXPC bpc;

	private IContainer components = null;

	public Button cmdEnrollData;

	public TextBox txtIPAddress;

	public TextBox txtPortNo;

	public RadioButton optSerialDevice;

	public GroupBox Frame4;

	public Button cmdOpen;

	public Button cmdClose;

	public Label lblMachineNumber;

	public TextBox textBox1;

	public Label label4;

	public Label label3;

	public Label label1;

	private NumericUpDown TbxDeviceId;

	public FrmEbio()
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmEbio));
		InitializeComponent();
		bpc = new AxSBXPC();
		((ISupportInitialize)bpc).BeginInit();
		((AxHost)bpc).Enabled = true;
		((Control)bpc).Location = new Point(10, 9);
		((Control)bpc).Name = "bpc";
		((AxHost)bpc).OcxState = (State)componentResourceManager.GetObject("bpc.OcxState");
		((Control)bpc).Size = new Size(100, 50);
		((Control)bpc).TabIndex = 37;
		bpc.OnReceiveEventXML += SBXPC1_OnReceiveEventXML;
		((ISupportInitialize)bpc).EndInit();
		((Control)this).Controls.Add((Control)(object)bpc);
		frmMonitoringEbio = new FrmMonitoringEbio();
	}

	private void FrmMain_Load(object sender, EventArgs e)
	{
		((Control)txtIPAddress).Enabled = true;
		((Control)txtPortNo).Enabled = true;
		((Control)cmdOpen).Enabled = true;
		((Control)cmdClose).Enabled = false;
		((Control)cmdEnrollData).Enabled = false;
		mOpenFlag = false;
		((Control)TbxDeviceId).Text = Convert.ToString(1);
		bpc.DotNET();
	}

	private void cmdClose_Click(object sender, EventArgs e)
	{
		if (mOpenFlag)
		{
			bpc.Disconnect();
			mOpenFlag = false;
			((Control)cmdOpen).Enabled = true;
			((Control)cmdClose).Enabled = false;
			((Control)cmdEnrollData).Enabled = false;
		}
	}

	private void optSerialDevice_CheckedChanged(object sender, EventArgs e)
	{
		if (optSerialDevice.Checked)
		{
			((Control)txtIPAddress).Enabled = true;
			((Control)txtPortNo).Enabled = true;
			string text = ((Control)txtIPAddress).Text;
		}
	}

	private void optNetworkDevice_CheckedChanged(object sender, EventArgs e)
	{
		optSerialDevice.Checked = false;
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		if (mOpenFlag)
		{
			bpc.Disconnect();
		}
		((Form)this).Close();
	}

	private void SBXPC1_OnReceiveEventXML(object sender, _DSBXPCEvents_OnReceiveEventXMLEvent e)
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
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0857: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Expected O, but got Unknown
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Expected O, but got Unknown
		//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Expected O, but got Unknown
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Expected O, but got Unknown
		//IL_0cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcb: Expected O, but got Unknown
		//IL_0dce: Unknown result type (might be due to invalid IL or missing references)
		cmdEnrollData = new Button();
		txtIPAddress = new TextBox();
		txtPortNo = new TextBox();
		optSerialDevice = new RadioButton();
		Frame4 = new GroupBox();
		textBox1 = new TextBox();
		label4 = new Label();
		label3 = new Label();
		label1 = new Label();
		TbxDeviceId = new NumericUpDown();
		cmdOpen = new Button();
		cmdClose = new Button();
		lblMachineNumber = new Label();
		((Control)Frame4).SuspendLayout();
		((ISupportInitialize)TbxDeviceId).BeginInit();
		((Control)this).SuspendLayout();
		((Control)cmdEnrollData).BackColor = SystemColors.Control;
		((Control)cmdEnrollData).Cursor = Cursors.Default;
		((Control)cmdEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdEnrollData).Location = new Point(348, 194);
		((Control)cmdEnrollData).Margin = new Padding(4);
		((Control)cmdEnrollData).Name = "cmdEnrollData";
		((Control)cmdEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdEnrollData).Size = new Size(275, 39);
		((Control)cmdEnrollData).TabIndex = 3;
		((Control)cmdEnrollData).Text = "Enroll Data Management";
		((ButtonBase)cmdEnrollData).UseVisualStyleBackColor = false;
		txtIPAddress.AcceptsReturn = true;
		((Control)txtIPAddress).BackColor = SystemColors.Window;
		((Control)txtIPAddress).Cursor = Cursors.IBeam;
		((Control)txtIPAddress).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtIPAddress).ForeColor = SystemColors.WindowText;
		((Control)txtIPAddress).Location = new Point(224, 29);
		((Control)txtIPAddress).Margin = new Padding(4);
		((TextBoxBase)txtIPAddress).MaxLength = 0;
		((Control)txtIPAddress).Name = "txtIPAddress";
		((Control)txtIPAddress).RightToLeft = (RightToLeft)0;
		((Control)txtIPAddress).Size = new Size(129, 26);
		((Control)txtIPAddress).TabIndex = 8;
		((Control)txtIPAddress).Text = "192.168.1.224";
		txtPortNo.AcceptsReturn = true;
		((Control)txtPortNo).BackColor = SystemColors.Window;
		((Control)txtPortNo).Cursor = Cursors.IBeam;
		((Control)txtPortNo).Font = new Font("Tahoma", 9.75f);
		((Control)txtPortNo).ForeColor = SystemColors.WindowText;
		((Control)txtPortNo).Location = new Point(466, 73);
		((Control)txtPortNo).Margin = new Padding(4);
		((TextBoxBase)txtPortNo).MaxLength = 0;
		((Control)txtPortNo).Name = "txtPortNo";
		((Control)txtPortNo).RightToLeft = (RightToLeft)0;
		((Control)txtPortNo).Size = new Size(40, 23);
		((Control)txtPortNo).TabIndex = 7;
		((Control)txtPortNo).Text = "5005";
		((Control)optSerialDevice).BackColor = SystemColors.Control;
		((Control)optSerialDevice).Cursor = Cursors.Default;
		((Control)optSerialDevice).Font = new Font("Tahoma", 9.75f);
		((Control)optSerialDevice).ForeColor = SystemColors.ControlText;
		((Control)optSerialDevice).Location = new Point(28, 153);
		((Control)optSerialDevice).Margin = new Padding(4);
		((Control)optSerialDevice).Name = "optSerialDevice";
		((Control)optSerialDevice).RightToLeft = (RightToLeft)1;
		((Control)optSerialDevice).Size = new Size(595, 33);
		((Control)optSerialDevice).TabIndex = 34;
		optSerialDevice.TabStop = true;
		((Control)optSerialDevice).Text = "دریافت اطلاعات ثبت نام مشتریان از دستگاه و ذخیره در دیتابیس";
		((ButtonBase)optSerialDevice).UseVisualStyleBackColor = false;
		((Control)optSerialDevice).Click += optSerialDevice_CheckedChanged;
		((Control)Frame4).Anchor = (AnchorStyles)13;
		((Control)Frame4).BackColor = SystemColors.Control;
		((Control)Frame4).Controls.Add((Control)(object)textBox1);
		((Control)Frame4).Controls.Add((Control)(object)label4);
		((Control)Frame4).Controls.Add((Control)(object)label3);
		((Control)Frame4).Controls.Add((Control)(object)label1);
		((Control)Frame4).Controls.Add((Control)(object)txtPortNo);
		((Control)Frame4).Controls.Add((Control)(object)TbxDeviceId);
		((Control)Frame4).Controls.Add((Control)(object)txtIPAddress);
		((Control)Frame4).Controls.Add((Control)(object)cmdOpen);
		((Control)Frame4).Controls.Add((Control)(object)cmdClose);
		((Control)Frame4).Controls.Add((Control)(object)lblMachineNumber);
		((Control)Frame4).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Frame4).ForeColor = SystemColors.ControlText;
		((Control)Frame4).Location = new Point(12, 24);
		((Control)Frame4).Margin = new Padding(4);
		((Control)Frame4).Name = "Frame4";
		((Control)Frame4).Padding = new Padding(0);
		((Control)Frame4).RightToLeft = (RightToLeft)1;
		((Control)Frame4).Size = new Size(611, 111);
		((Control)Frame4).TabIndex = 36;
		Frame4.TabStop = false;
		((Control)Frame4).Text = "برقراری ارتباط با دستگاه";
		textBox1.AcceptsReturn = true;
		((Control)textBox1).BackColor = SystemColors.Window;
		((Control)textBox1).Cursor = Cursors.IBeam;
		((Control)textBox1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBox1).ForeColor = SystemColors.WindowText;
		((Control)textBox1).Location = new Point(224, 71);
		((Control)textBox1).Margin = new Padding(4);
		((TextBoxBase)textBox1).MaxLength = 0;
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).RightToLeft = (RightToLeft)0;
		((Control)textBox1).Size = new Size(129, 26);
		((Control)textBox1).TabIndex = 28;
		((Control)textBox1).Text = "123456";
		((Control)label4).Anchor = (AnchorStyles)9;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = SystemColors.Control;
		((Control)label4).Cursor = Cursors.Default;
		((Control)label4).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label4).ForeColor = SystemColors.ControlText;
		((Control)label4).Location = new Point(355, 76);
		((Control)label4).Margin = new Padding(4, 0, 4, 0);
		((Control)label4).Name = "label4";
		((Control)label4).RightToLeft = (RightToLeft)1;
		((Control)label4).Size = new Size(74, 16);
		((Control)label4).TabIndex = 27;
		((Control)label4).Text = "رمز دستگاه:";
		((Control)label3).Anchor = (AnchorStyles)9;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = SystemColors.Control;
		((Control)label3).Cursor = Cursors.Default;
		((Control)label3).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = SystemColors.ControlText;
		((Control)label3).Location = new Point(514, 79);
		((Control)label3).Margin = new Padding(4, 0, 4, 0);
		((Control)label3).Name = "label3";
		((Control)label3).RightToLeft = (RightToLeft)1;
		((Control)label3).Size = new Size(78, 16);
		((Control)label3).TabIndex = 26;
		((Control)label3).Text = "شماره پورت:";
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = SystemColors.Control;
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = SystemColors.ControlText;
		((Control)label1).Location = new Point(355, 34);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(92, 16);
		((Control)label1).TabIndex = 25;
		((Control)label1).Text = "آی پی دستگاه:";
		((Control)TbxDeviceId).Anchor = (AnchorStyles)9;
		((Control)TbxDeviceId).Font = new Font("Tahoma", 9.75f);
		((Control)TbxDeviceId).Location = new Point(467, 31);
		TbxDeviceId.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		TbxDeviceId.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)TbxDeviceId).Name = "TbxDeviceId";
		((Control)TbxDeviceId).Size = new Size(40, 23);
		((Control)TbxDeviceId).TabIndex = 24;
		TbxDeviceId.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)cmdOpen).BackColor = SystemColors.Control;
		((Control)cmdOpen).Cursor = Cursors.Default;
		((Control)cmdOpen).Font = new Font("Tahoma", 9.75f);
		((Control)cmdOpen).ForeColor = SystemColors.ControlText;
		((Control)cmdOpen).Location = new Point(4, 21);
		((Control)cmdOpen).Margin = new Padding(4);
		((Control)cmdOpen).Name = "cmdOpen";
		((Control)cmdOpen).RightToLeft = (RightToLeft)0;
		((Control)cmdOpen).Size = new Size(201, 39);
		((Control)cmdOpen).TabIndex = 23;
		((Control)cmdOpen).Text = "باز کردن ارتباط";
		((ButtonBase)cmdOpen).UseVisualStyleBackColor = false;
		((Control)cmdClose).BackColor = SystemColors.Control;
		((Control)cmdClose).Cursor = Cursors.Default;
		((Control)cmdClose).Font = new Font("Tahoma", 9.75f);
		((Control)cmdClose).ForeColor = SystemColors.ControlText;
		((Control)cmdClose).Location = new Point(4, 64);
		((Control)cmdClose).Margin = new Padding(4);
		((Control)cmdClose).Name = "cmdClose";
		((Control)cmdClose).RightToLeft = (RightToLeft)0;
		((Control)cmdClose).Size = new Size(201, 39);
		((Control)cmdClose).TabIndex = 22;
		((Control)cmdClose).Text = "قطع ارتباط";
		((ButtonBase)cmdClose).UseVisualStyleBackColor = false;
		((Control)cmdClose).Click += cmdClose_Click;
		((Control)lblMachineNumber).Anchor = (AnchorStyles)9;
		((Control)lblMachineNumber).AutoSize = true;
		((Control)lblMachineNumber).BackColor = SystemColors.Control;
		((Control)lblMachineNumber).Cursor = Cursors.Default;
		((Control)lblMachineNumber).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMachineNumber).ForeColor = SystemColors.ControlText;
		((Control)lblMachineNumber).Location = new Point(514, 34);
		((Control)lblMachineNumber).Margin = new Padding(4, 0, 4, 0);
		((Control)lblMachineNumber).Name = "lblMachineNumber";
		((Control)lblMachineNumber).RightToLeft = (RightToLeft)1;
		((Control)lblMachineNumber).Size = new Size(93, 16);
		((Control)lblMachineNumber).TabIndex = 21;
		((Control)lblMachineNumber).Text = "شماره دستگاه:";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(637, 593);
		((Control)this).Controls.Add((Control)(object)optSerialDevice);
		((Control)this).Controls.Add((Control)(object)Frame4);
		((Control)this).Controls.Add((Control)(object)cmdEnrollData);
		((Control)this).Font = new Font("Tahoma", 9.75f);
		((Form)this).Margin = new Padding(4);
		((Control)this).Name = "FrmEbio";
		((Control)this).Text = "فرم دریافت / ارسال اطلاعات ثبت نام مشتریان روی دستگاه ";
		((Form)this).Load += FrmMain_Load;
		((Control)Frame4).ResumeLayout(false);
		((Control)Frame4).PerformLayout();
		((ISupportInitialize)TbxDeviceId).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
