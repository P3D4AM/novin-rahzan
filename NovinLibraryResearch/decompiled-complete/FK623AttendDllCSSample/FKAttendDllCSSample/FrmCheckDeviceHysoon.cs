using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FKAttendDllCSSample.Properties;
using PacketParser;
using PacketParser.BusinessLayer;
using RegisterManagement;
using TimeAttendanceDAL.Utility;

namespace FKAttendDllCSSample;

public class FrmCheckDeviceHysoon : Form
{
	public Label LbSubject;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	public GroupBox Frame2;

	public Label LblPortNo;

	public Label LblIpAddress;

	public TextBox TxtPortNo;

	public TextBox TxtIpAddress;

	private GroupBox groupBox1;

	public TextBox TxtDeviceIdByLan;

	public TextBox TxtLangByLan;

	public Label lblSerialNo;

	public Label label10;

	public Label Label1;

	public TextBox TxtTimeByLan;

	public TextBox txtSerialNoByLan;

	public Label label9;

	public TextBox txtProductCodeByLan;

	public Label LblMachineNumber;

	private GroupBox PnlTest;

	private PictureBox PicSetLangByLan;

	public Button CmdOpenByLan;

	public Label label8;

	public Label label2;

	private PictureBox PicSetTimeByLan;

	private PictureBox PicPing;

	public Label label7;

	public Label label3;

	private PictureBox PicTimeByLan;

	private PictureBox PicConfigByLan;

	public Label label6;

	public Label label4;

	private PictureBox PicSerialByLan;

	private PictureBox PicNameDeviceByLan;

	public Label label5;

	private GroupBox groupBox3;

	public TextBox TxtDeviceIdByUsb;

	public TextBox TxtLangByUsb;

	public Label label13;

	public Label label14;

	public Label label15;

	public TextBox TxtTimeByUsb;

	public TextBox txtSerialNoByUsb;

	public Label label16;

	public TextBox txtProductCodeByUsb;

	public Label label17;

	private GroupBox groupBox4;

	private PictureBox PicSetLangByUsb;

	public Button CmdOpenByUsb;

	public Label label18;

	private PictureBox PicSetTimeByUsb;

	public Label label20;

	public Label label21;

	private PictureBox PicTimeByUsb;

	private PictureBox PicConfigByUsb;

	public Label label22;

	public Label label23;

	private PictureBox PicSerialByUsb;

	private PictureBox PicNameDeviceByUsb;

	public Label label24;

	private Container _components = null;

	public static FrmCheckDeviceHysoon GfrmMain;

	public bool OpenFlag;

	public FrmCheckDeviceHysoon()
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
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected O, but got Unknown
		//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_1024: Unknown result type (might be due to invalid IL or missing references)
		//IL_102e: Expected O, but got Unknown
		//IL_11aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b4: Expected O, but got Unknown
		//IL_1d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9c: Expected O, but got Unknown
		//IL_205d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2067: Expected O, but got Unknown
		//IL_2370: Unknown result type (might be due to invalid IL or missing references)
		//IL_237a: Expected O, but got Unknown
		//IL_243e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2448: Expected O, but got Unknown
		//IL_25c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ce: Expected O, but got Unknown
		//IL_3079: Unknown result type (might be due to invalid IL or missing references)
		//IL_3083: Expected O, but got Unknown
		//IL_3098: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a2: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmCheckDeviceHysoon));
		LbSubject = new Label();
		tabControl1 = new TabControl();
		tabPage1 = new TabPage();
		Frame2 = new GroupBox();
		LblPortNo = new Label();
		LblIpAddress = new Label();
		TxtPortNo = new TextBox();
		TxtIpAddress = new TextBox();
		groupBox1 = new GroupBox();
		TxtDeviceIdByLan = new TextBox();
		TxtLangByLan = new TextBox();
		lblSerialNo = new Label();
		label10 = new Label();
		Label1 = new Label();
		TxtTimeByLan = new TextBox();
		txtSerialNoByLan = new TextBox();
		label9 = new Label();
		txtProductCodeByLan = new TextBox();
		LblMachineNumber = new Label();
		PnlTest = new GroupBox();
		PicSetLangByLan = new PictureBox();
		CmdOpenByLan = new Button();
		label8 = new Label();
		label2 = new Label();
		PicSetTimeByLan = new PictureBox();
		PicPing = new PictureBox();
		label7 = new Label();
		label3 = new Label();
		PicTimeByLan = new PictureBox();
		PicConfigByLan = new PictureBox();
		label6 = new Label();
		label4 = new Label();
		PicSerialByLan = new PictureBox();
		PicNameDeviceByLan = new PictureBox();
		label5 = new Label();
		tabPage2 = new TabPage();
		groupBox3 = new GroupBox();
		TxtDeviceIdByUsb = new TextBox();
		TxtLangByUsb = new TextBox();
		label13 = new Label();
		label14 = new Label();
		label15 = new Label();
		TxtTimeByUsb = new TextBox();
		txtSerialNoByUsb = new TextBox();
		label16 = new Label();
		txtProductCodeByUsb = new TextBox();
		label17 = new Label();
		groupBox4 = new GroupBox();
		PicSetLangByUsb = new PictureBox();
		CmdOpenByUsb = new Button();
		label18 = new Label();
		PicSetTimeByUsb = new PictureBox();
		label20 = new Label();
		label21 = new Label();
		PicTimeByUsb = new PictureBox();
		PicConfigByUsb = new PictureBox();
		label22 = new Label();
		label23 = new Label();
		PicSerialByUsb = new PictureBox();
		PicNameDeviceByUsb = new PictureBox();
		label24 = new Label();
		((Control)tabControl1).SuspendLayout();
		((Control)tabPage1).SuspendLayout();
		((Control)Frame2).SuspendLayout();
		((Control)groupBox1).SuspendLayout();
		((Control)PnlTest).SuspendLayout();
		((ISupportInitialize)PicSetLangByLan).BeginInit();
		((ISupportInitialize)PicSetTimeByLan).BeginInit();
		((ISupportInitialize)PicPing).BeginInit();
		((ISupportInitialize)PicTimeByLan).BeginInit();
		((ISupportInitialize)PicConfigByLan).BeginInit();
		((ISupportInitialize)PicSerialByLan).BeginInit();
		((ISupportInitialize)PicNameDeviceByLan).BeginInit();
		((Control)tabPage2).SuspendLayout();
		((Control)groupBox3).SuspendLayout();
		((Control)groupBox4).SuspendLayout();
		((ISupportInitialize)PicSetLangByUsb).BeginInit();
		((ISupportInitialize)PicSetTimeByUsb).BeginInit();
		((ISupportInitialize)PicTimeByUsb).BeginInit();
		((ISupportInitialize)PicConfigByUsb).BeginInit();
		((ISupportInitialize)PicSerialByUsb).BeginInit();
		((ISupportInitialize)PicNameDeviceByUsb).BeginInit();
		((Control)this).SuspendLayout();
		((Control)LbSubject).BackColor = Color.Transparent;
		((Control)LbSubject).Cursor = Cursors.Default;
		((Control)LbSubject).Font = new Font("B Traffic", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)LbSubject).ForeColor = SystemColors.HotTrack;
		((Control)LbSubject).Location = new Point(2, 17);
		((Control)LbSubject).Name = "LbSubject";
		((Control)LbSubject).RightToLeft = (RightToLeft)0;
		((Control)LbSubject).Size = new Size(612, 34);
		((Control)LbSubject).TabIndex = 69;
		((Control)LbSubject).Text = "بررسی و کانفیگ دستگاه های هایسون";
		LbSubject.TextAlign = (ContentAlignment)2;
		((Control)tabControl1).Controls.Add((Control)(object)tabPage1);
		((Control)tabControl1).Controls.Add((Control)(object)tabPage2);
		((Control)tabControl1).Location = new Point(12, 54);
		((Control)tabControl1).Name = "tabControl1";
		((Control)tabControl1).RightToLeft = (RightToLeft)1;
		tabControl1.RightToLeftLayout = true;
		tabControl1.SelectedIndex = 0;
		((Control)tabControl1).Size = new Size(602, 378);
		((Control)tabControl1).TabIndex = 104;
		((Control)tabPage1).Controls.Add((Control)(object)Frame2);
		((Control)tabPage1).Controls.Add((Control)(object)groupBox1);
		((Control)tabPage1).Controls.Add((Control)(object)PnlTest);
		tabPage1.Location = new Point(4, 25);
		((Control)tabPage1).Name = "tabPage1";
		((Control)tabPage1).Padding = new Padding(3);
		((Control)tabPage1).Size = new Size(594, 349);
		tabPage1.TabIndex = 0;
		((Control)tabPage1).Text = "ارتباط LAN";
		tabPage1.UseVisualStyleBackColor = true;
		((Control)tabPage1).Click += tabPage1_Click;
		((Control)Frame2).BackColor = Color.Transparent;
		((Control)Frame2).Controls.Add((Control)(object)LblPortNo);
		((Control)Frame2).Controls.Add((Control)(object)LblIpAddress);
		((Control)Frame2).Controls.Add((Control)(object)TxtPortNo);
		((Control)Frame2).Controls.Add((Control)(object)TxtIpAddress);
		((Control)Frame2).ForeColor = SystemColors.ControlText;
		((Control)Frame2).Location = new Point(311, 18);
		((Control)Frame2).Name = "Frame2";
		((Control)Frame2).RightToLeft = (RightToLeft)1;
		((Control)Frame2).Size = new Size(257, 117);
		((Control)Frame2).TabIndex = 105;
		Frame2.TabStop = false;
		((Control)Frame2).Text = "تنظيمات ارتباط با دستگاه";
		((Control)LblPortNo).AutoSize = true;
		((Control)LblPortNo).BackColor = Color.Transparent;
		((Control)LblPortNo).Cursor = Cursors.Default;
		((Control)LblPortNo).ForeColor = SystemColors.ControlText;
		((Control)LblPortNo).Location = new Point(159, 72);
		((Control)LblPortNo).Name = "LblPortNo";
		((Control)LblPortNo).RightToLeft = (RightToLeft)1;
		((Control)LblPortNo).Size = new Size(90, 16);
		((Control)LblPortNo).TabIndex = 15;
		((Control)LblPortNo).Text = "شماره پورت* :";
		LblPortNo.TextAlign = (ContentAlignment)64;
		((Control)LblIpAddress).AutoSize = true;
		((Control)LblIpAddress).BackColor = Color.Transparent;
		((Control)LblIpAddress).Cursor = Cursors.Default;
		((Control)LblIpAddress).ForeColor = SystemColors.ControlText;
		((Control)LblIpAddress).Location = new Point(159, 37);
		((Control)LblIpAddress).Name = "LblIpAddress";
		((Control)LblIpAddress).RightToLeft = (RightToLeft)1;
		((Control)LblIpAddress).Size = new Size(59, 16);
		((Control)LblIpAddress).TabIndex = 14;
		((Control)LblIpAddress).Text = "آي پي* :";
		LblIpAddress.TextAlign = (ContentAlignment)64;
		TxtPortNo.AcceptsReturn = true;
		((Control)TxtPortNo).BackColor = SystemColors.Window;
		((Control)TxtPortNo).Cursor = Cursors.IBeam;
		((Control)TxtPortNo).ForeColor = SystemColors.WindowText;
		((Control)TxtPortNo).Location = new Point(6, 70);
		((TextBoxBase)TxtPortNo).MaxLength = 0;
		((Control)TxtPortNo).Name = "TxtPortNo";
		((Control)TxtPortNo).RightToLeft = (RightToLeft)0;
		((Control)TxtPortNo).Size = new Size(147, 23);
		((Control)TxtPortNo).TabIndex = 13;
		((Control)TxtPortNo).Text = "5005";
		TxtIpAddress.AcceptsReturn = true;
		((Control)TxtIpAddress).BackColor = SystemColors.Window;
		((Control)TxtIpAddress).Cursor = Cursors.IBeam;
		((Control)TxtIpAddress).ForeColor = SystemColors.WindowText;
		((Control)TxtIpAddress).Location = new Point(6, 35);
		((TextBoxBase)TxtIpAddress).MaxLength = 0;
		((Control)TxtIpAddress).Name = "TxtIpAddress";
		((Control)TxtIpAddress).RightToLeft = (RightToLeft)0;
		((Control)TxtIpAddress).Size = new Size(147, 23);
		((Control)TxtIpAddress).TabIndex = 12;
		((Control)TxtIpAddress).Text = "192.168.1.224";
		((Control)groupBox1).Controls.Add((Control)(object)TxtDeviceIdByLan);
		((Control)groupBox1).Controls.Add((Control)(object)TxtLangByLan);
		((Control)groupBox1).Controls.Add((Control)(object)lblSerialNo);
		((Control)groupBox1).Controls.Add((Control)(object)label10);
		((Control)groupBox1).Controls.Add((Control)(object)Label1);
		((Control)groupBox1).Controls.Add((Control)(object)TxtTimeByLan);
		((Control)groupBox1).Controls.Add((Control)(object)txtSerialNoByLan);
		((Control)groupBox1).Controls.Add((Control)(object)label9);
		((Control)groupBox1).Controls.Add((Control)(object)txtProductCodeByLan);
		((Control)groupBox1).Controls.Add((Control)(object)LblMachineNumber);
		((Control)groupBox1).Location = new Point(311, 141);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).RightToLeft = (RightToLeft)1;
		((Control)groupBox1).Size = new Size(257, 190);
		((Control)groupBox1).TabIndex = 104;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "اطلاعات دستگاه";
		TxtDeviceIdByLan.AcceptsReturn = true;
		((Control)TxtDeviceIdByLan).BackColor = SystemColors.Window;
		((Control)TxtDeviceIdByLan).Cursor = Cursors.IBeam;
		((Control)TxtDeviceIdByLan).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtDeviceIdByLan).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TxtDeviceIdByLan).Location = new Point(6, 31);
		((TextBoxBase)TxtDeviceIdByLan).MaxLength = 32;
		((Control)TxtDeviceIdByLan).Name = "TxtDeviceIdByLan";
		((Control)TxtDeviceIdByLan).RightToLeft = (RightToLeft)0;
		((Control)TxtDeviceIdByLan).Size = new Size(149, 23);
		((Control)TxtDeviceIdByLan).TabIndex = 107;
		TxtLangByLan.AcceptsReturn = true;
		((Control)TxtLangByLan).BackColor = SystemColors.Window;
		((Control)TxtLangByLan).Cursor = Cursors.IBeam;
		((Control)TxtLangByLan).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtLangByLan).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TxtLangByLan).Location = new Point(6, 160);
		((TextBoxBase)TxtLangByLan).MaxLength = 32;
		((Control)TxtLangByLan).Name = "TxtLangByLan";
		((Control)TxtLangByLan).RightToLeft = (RightToLeft)0;
		((Control)TxtLangByLan).Size = new Size(149, 23);
		((Control)TxtLangByLan).TabIndex = 105;
		((Control)TxtLangByLan).Visible = false;
		((Control)lblSerialNo).AutoSize = true;
		((Control)lblSerialNo).BackColor = Color.Transparent;
		((Control)lblSerialNo).Cursor = Cursors.Default;
		((Control)lblSerialNo).ForeColor = SystemColors.ControlText;
		((Control)lblSerialNo).Location = new Point(161, 101);
		((Control)lblSerialNo).Name = "lblSerialNo";
		((Control)lblSerialNo).RightToLeft = (RightToLeft)1;
		((Control)lblSerialNo).Size = new Size(91, 16);
		((Control)lblSerialNo).TabIndex = 87;
		((Control)lblSerialNo).Text = "سريال دستگاه:";
		lblSerialNo.TextAlign = (ContentAlignment)64;
		((Control)label10).AutoSize = true;
		((Control)label10).BackColor = Color.Transparent;
		((Control)label10).Cursor = Cursors.Default;
		((Control)label10).ForeColor = SystemColors.ControlText;
		((Control)label10).Location = new Point(161, 165);
		((Control)label10).Name = "label10";
		((Control)label10).RightToLeft = (RightToLeft)1;
		((Control)label10).Size = new Size(79, 16);
		((Control)label10).TabIndex = 106;
		((Control)label10).Text = "زبان دستگاه:";
		label10.TextAlign = (ContentAlignment)64;
		((Control)label10).Visible = false;
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = Color.Transparent;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(161, 68);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)1;
		((Control)Label1).Size = new Size(77, 16);
		((Control)Label1).TabIndex = 88;
		((Control)Label1).Text = " نام دستگاه:";
		Label1.TextAlign = (ContentAlignment)64;
		TxtTimeByLan.AcceptsReturn = true;
		((Control)TxtTimeByLan).BackColor = SystemColors.Window;
		((Control)TxtTimeByLan).Cursor = Cursors.IBeam;
		((Control)TxtTimeByLan).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtTimeByLan).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TxtTimeByLan).Location = new Point(6, 128);
		((TextBoxBase)TxtTimeByLan).MaxLength = 32;
		((Control)TxtTimeByLan).Name = "TxtTimeByLan";
		((Control)TxtTimeByLan).RightToLeft = (RightToLeft)0;
		((Control)TxtTimeByLan).Size = new Size(149, 23);
		((Control)TxtTimeByLan).TabIndex = 103;
		txtSerialNoByLan.AcceptsReturn = true;
		((Control)txtSerialNoByLan).BackColor = SystemColors.Window;
		((Control)txtSerialNoByLan).Cursor = Cursors.IBeam;
		((Control)txtSerialNoByLan).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtSerialNoByLan).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)txtSerialNoByLan).Location = new Point(6, 96);
		((TextBoxBase)txtSerialNoByLan).MaxLength = 32;
		((Control)txtSerialNoByLan).Name = "txtSerialNoByLan";
		((Control)txtSerialNoByLan).RightToLeft = (RightToLeft)0;
		((Control)txtSerialNoByLan).Size = new Size(149, 23);
		((Control)txtSerialNoByLan).TabIndex = 85;
		((Control)label9).AutoSize = true;
		((Control)label9).BackColor = Color.Transparent;
		((Control)label9).Cursor = Cursors.Default;
		((Control)label9).ForeColor = SystemColors.ControlText;
		((Control)label9).Location = new Point(161, 133);
		((Control)label9).Name = "label9";
		((Control)label9).RightToLeft = (RightToLeft)1;
		((Control)label9).Size = new Size(94, 16);
		((Control)label9).TabIndex = 104;
		((Control)label9).Text = "ساعت دستگاه:";
		label9.TextAlign = (ContentAlignment)64;
		txtProductCodeByLan.AcceptsReturn = true;
		((Control)txtProductCodeByLan).BackColor = SystemColors.Window;
		((Control)txtProductCodeByLan).Cursor = Cursors.IBeam;
		((Control)txtProductCodeByLan).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtProductCodeByLan).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)txtProductCodeByLan).Location = new Point(6, 64);
		((TextBoxBase)txtProductCodeByLan).MaxLength = 32;
		((Control)txtProductCodeByLan).Name = "txtProductCodeByLan";
		((Control)txtProductCodeByLan).RightToLeft = (RightToLeft)0;
		((Control)txtProductCodeByLan).Size = new Size(149, 23);
		((Control)txtProductCodeByLan).TabIndex = 86;
		((Control)LblMachineNumber).AutoSize = true;
		((Control)LblMachineNumber).BackColor = Color.Transparent;
		((Control)LblMachineNumber).Cursor = Cursors.Default;
		((Control)LblMachineNumber).ForeColor = SystemColors.ControlText;
		((Control)LblMachineNumber).Location = new Point(161, 34);
		((Control)LblMachineNumber).Name = "LblMachineNumber";
		((Control)LblMachineNumber).RightToLeft = (RightToLeft)1;
		((Control)LblMachineNumber).Size = new Size(93, 16);
		((Control)LblMachineNumber).TabIndex = 71;
		((Control)LblMachineNumber).Text = "شماره دستگاه:";
		LblMachineNumber.TextAlign = (ContentAlignment)64;
		((Control)PnlTest).Controls.Add((Control)(object)PicSetLangByLan);
		((Control)PnlTest).Controls.Add((Control)(object)CmdOpenByLan);
		((Control)PnlTest).Controls.Add((Control)(object)label8);
		((Control)PnlTest).Controls.Add((Control)(object)label2);
		((Control)PnlTest).Controls.Add((Control)(object)PicSetTimeByLan);
		((Control)PnlTest).Controls.Add((Control)(object)PicPing);
		((Control)PnlTest).Controls.Add((Control)(object)label7);
		((Control)PnlTest).Controls.Add((Control)(object)label3);
		((Control)PnlTest).Controls.Add((Control)(object)PicTimeByLan);
		((Control)PnlTest).Controls.Add((Control)(object)PicConfigByLan);
		((Control)PnlTest).Controls.Add((Control)(object)label6);
		((Control)PnlTest).Controls.Add((Control)(object)label4);
		((Control)PnlTest).Controls.Add((Control)(object)PicSerialByLan);
		((Control)PnlTest).Controls.Add((Control)(object)PicNameDeviceByLan);
		((Control)PnlTest).Controls.Add((Control)(object)label5);
		((Control)PnlTest).Location = new Point(26, 18);
		((Control)PnlTest).Name = "PnlTest";
		((Control)PnlTest).Size = new Size(279, 313);
		((Control)PnlTest).TabIndex = 106;
		PnlTest.TabStop = false;
		((Control)PicSetLangByLan).Location = new Point(6, 280);
		((Control)PicSetLangByLan).Name = "PicSetLangByLan";
		((Control)PicSetLangByLan).Size = new Size(26, 26);
		PicSetLangByLan.SizeMode = (PictureBoxSizeMode)1;
		PicSetLangByLan.TabIndex = 102;
		PicSetLangByLan.TabStop = false;
		((Control)PicSetLangByLan).Visible = false;
		((Control)CmdOpenByLan).BackColor = SystemColors.Control;
		((Control)CmdOpenByLan).Cursor = Cursors.Default;
		((Control)CmdOpenByLan).ForeColor = SystemColors.ControlText;
		((Control)CmdOpenByLan).Location = new Point(6, 26);
		((Control)CmdOpenByLan).Name = "CmdOpenByLan";
		((Control)CmdOpenByLan).RightToLeft = (RightToLeft)0;
		((Control)CmdOpenByLan).Size = new Size(257, 37);
		((Control)CmdOpenByLan).TabIndex = 81;
		((Control)CmdOpenByLan).Text = "بررسي دستگاه";
		((ButtonBase)CmdOpenByLan).UseVisualStyleBackColor = false;
		((Control)CmdOpenByLan).Click += CmdOpenByLan_Click;
		((Control)label8).AutoSize = true;
		((Control)label8).BackColor = Color.Transparent;
		((Control)label8).Cursor = Cursors.Default;
		((Control)label8).ForeColor = SystemColors.ControlText;
		((Control)label8).Location = new Point(54, 284);
		((Control)label8).Name = "label8";
		((Control)label8).RightToLeft = (RightToLeft)1;
		((Control)label8).Size = new Size(184, 16);
		((Control)label8).TabIndex = 101;
		((Control)label8).Text = "تنظيم زبان فارسي روي دستگاه:";
		label8.TextAlign = (ContentAlignment)4;
		((Control)label8).Visible = false;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(54, 77);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)1;
		((Control)label2).Size = new Size(122, 16);
		((Control)label2).TabIndex = 89;
		((Control)label2).Text = "بررسي ارتباط شبکه:";
		label2.TextAlign = (ContentAlignment)4;
		((Control)PicSetTimeByLan).Location = new Point(6, 247);
		((Control)PicSetTimeByLan).Name = "PicSetTimeByLan";
		((Control)PicSetTimeByLan).Size = new Size(26, 26);
		PicSetTimeByLan.SizeMode = (PictureBoxSizeMode)1;
		PicSetTimeByLan.TabIndex = 100;
		PicSetTimeByLan.TabStop = false;
		((Control)PicPing).Location = new Point(6, 73);
		((Control)PicPing).Name = "PicPing";
		((Control)PicPing).Size = new Size(26, 26);
		PicPing.SizeMode = (PictureBoxSizeMode)1;
		PicPing.TabIndex = 90;
		PicPing.TabStop = false;
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = Color.Transparent;
		((Control)label7).Cursor = Cursors.Default;
		((Control)label7).ForeColor = SystemColors.ControlText;
		((Control)label7).Location = new Point(54, 251);
		((Control)label7).Name = "label7";
		((Control)label7).RightToLeft = (RightToLeft)1;
		((Control)label7).Size = new Size(130, 16);
		((Control)label7).TabIndex = 99;
		((Control)label7).Text = "تنظيم ساعت دستگاه:";
		label7.TextAlign = (ContentAlignment)4;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = Color.Transparent;
		((Control)label3).Cursor = Cursors.Default;
		((Control)label3).ForeColor = SystemColors.ControlText;
		((Control)label3).Location = new Point(54, 113);
		((Control)label3).Name = "label3";
		((Control)label3).RightToLeft = (RightToLeft)1;
		((Control)label3).Size = new Size(133, 16);
		((Control)label3).TabIndex = 91;
		((Control)label3).Text = "تست ارتباط با دستگاه:";
		label3.TextAlign = (ContentAlignment)4;
		((Control)PicTimeByLan).Location = new Point(6, 215);
		((Control)PicTimeByLan).Name = "PicTimeByLan";
		((Control)PicTimeByLan).Size = new Size(26, 26);
		PicTimeByLan.SizeMode = (PictureBoxSizeMode)1;
		PicTimeByLan.TabIndex = 98;
		PicTimeByLan.TabStop = false;
		((Control)PicConfigByLan).Location = new Point(6, 109);
		((Control)PicConfigByLan).Name = "PicConfigByLan";
		((Control)PicConfigByLan).Size = new Size(26, 26);
		PicConfigByLan.SizeMode = (PictureBoxSizeMode)1;
		PicConfigByLan.TabIndex = 92;
		PicConfigByLan.TabStop = false;
		((Control)label6).AutoSize = true;
		((Control)label6).BackColor = Color.Transparent;
		((Control)label6).Cursor = Cursors.Default;
		((Control)label6).ForeColor = SystemColors.ControlText;
		((Control)label6).Location = new Point(54, 219);
		((Control)label6).Name = "label6";
		((Control)label6).RightToLeft = (RightToLeft)1;
		((Control)label6).Size = new Size(134, 16);
		((Control)label6).TabIndex = 97;
		((Control)label6).Text = "دريافت ساعت دستگاه:";
		label6.TextAlign = (ContentAlignment)4;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = Color.Transparent;
		((Control)label4).Cursor = Cursors.Default;
		((Control)label4).ForeColor = SystemColors.ControlText;
		((Control)label4).Location = new Point(54, 149);
		((Control)label4).Name = "label4";
		((Control)label4).RightToLeft = (RightToLeft)1;
		((Control)label4).Size = new Size(113, 16);
		((Control)label4).TabIndex = 93;
		((Control)label4).Text = "دريافت نام دستگاه:";
		label4.TextAlign = (ContentAlignment)4;
		((Control)PicSerialByLan).Location = new Point(6, 181);
		((Control)PicSerialByLan).Name = "PicSerialByLan";
		((Control)PicSerialByLan).Size = new Size(26, 26);
		PicSerialByLan.SizeMode = (PictureBoxSizeMode)1;
		PicSerialByLan.TabIndex = 96;
		PicSerialByLan.TabStop = false;
		((Control)PicNameDeviceByLan).Location = new Point(6, 145);
		((Control)PicNameDeviceByLan).Name = "PicNameDeviceByLan";
		((Control)PicNameDeviceByLan).Size = new Size(26, 26);
		PicNameDeviceByLan.SizeMode = (PictureBoxSizeMode)1;
		PicNameDeviceByLan.TabIndex = 94;
		PicNameDeviceByLan.TabStop = false;
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = Color.Transparent;
		((Control)label5).Cursor = Cursors.Default;
		((Control)label5).ForeColor = SystemColors.ControlText;
		((Control)label5).Location = new Point(54, 185);
		((Control)label5).Name = "label5";
		((Control)label5).RightToLeft = (RightToLeft)1;
		((Control)label5).Size = new Size(131, 16);
		((Control)label5).TabIndex = 95;
		((Control)label5).Text = "دريافت سريال دستگاه:";
		label5.TextAlign = (ContentAlignment)4;
		((Control)tabPage2).Controls.Add((Control)(object)groupBox3);
		((Control)tabPage2).Controls.Add((Control)(object)groupBox4);
		tabPage2.Location = new Point(4, 22);
		((Control)tabPage2).Name = "tabPage2";
		((Control)tabPage2).Padding = new Padding(3);
		((Control)tabPage2).Size = new Size(594, 352);
		tabPage2.TabIndex = 1;
		((Control)tabPage2).Text = "ارتباط USB";
		tabPage2.UseVisualStyleBackColor = true;
		((Control)groupBox3).Controls.Add((Control)(object)TxtDeviceIdByUsb);
		((Control)groupBox3).Controls.Add((Control)(object)TxtLangByUsb);
		((Control)groupBox3).Controls.Add((Control)(object)label13);
		((Control)groupBox3).Controls.Add((Control)(object)label14);
		((Control)groupBox3).Controls.Add((Control)(object)label15);
		((Control)groupBox3).Controls.Add((Control)(object)TxtTimeByUsb);
		((Control)groupBox3).Controls.Add((Control)(object)txtSerialNoByUsb);
		((Control)groupBox3).Controls.Add((Control)(object)label16);
		((Control)groupBox3).Controls.Add((Control)(object)txtProductCodeByUsb);
		((Control)groupBox3).Controls.Add((Control)(object)label17);
		((Control)groupBox3).Location = new Point(321, 29);
		((Control)groupBox3).Name = "groupBox3";
		((Control)groupBox3).RightToLeft = (RightToLeft)1;
		((Control)groupBox3).Size = new Size(257, 302);
		((Control)groupBox3).TabIndex = 107;
		groupBox3.TabStop = false;
		((Control)groupBox3).Text = "اطلاعات دستگاه";
		TxtDeviceIdByUsb.AcceptsReturn = true;
		((Control)TxtDeviceIdByUsb).BackColor = SystemColors.Window;
		((Control)TxtDeviceIdByUsb).Cursor = Cursors.IBeam;
		((Control)TxtDeviceIdByUsb).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtDeviceIdByUsb).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TxtDeviceIdByUsb).Location = new Point(6, 31);
		((TextBoxBase)TxtDeviceIdByUsb).MaxLength = 32;
		((Control)TxtDeviceIdByUsb).Name = "TxtDeviceIdByUsb";
		((Control)TxtDeviceIdByUsb).RightToLeft = (RightToLeft)0;
		((Control)TxtDeviceIdByUsb).Size = new Size(149, 23);
		((Control)TxtDeviceIdByUsb).TabIndex = 107;
		TxtLangByUsb.AcceptsReturn = true;
		((Control)TxtLangByUsb).BackColor = SystemColors.Window;
		((Control)TxtLangByUsb).Cursor = Cursors.IBeam;
		((Control)TxtLangByUsb).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtLangByUsb).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TxtLangByUsb).Location = new Point(6, 160);
		((TextBoxBase)TxtLangByUsb).MaxLength = 32;
		((Control)TxtLangByUsb).Name = "TxtLangByUsb";
		((Control)TxtLangByUsb).RightToLeft = (RightToLeft)0;
		((Control)TxtLangByUsb).Size = new Size(149, 23);
		((Control)TxtLangByUsb).TabIndex = 105;
		((Control)TxtLangByUsb).Visible = false;
		((Control)label13).AutoSize = true;
		((Control)label13).BackColor = Color.Transparent;
		((Control)label13).Cursor = Cursors.Default;
		((Control)label13).ForeColor = SystemColors.ControlText;
		((Control)label13).Location = new Point(161, 101);
		((Control)label13).Name = "label13";
		((Control)label13).RightToLeft = (RightToLeft)1;
		((Control)label13).Size = new Size(91, 16);
		((Control)label13).TabIndex = 87;
		((Control)label13).Text = "سريال دستگاه:";
		label13.TextAlign = (ContentAlignment)64;
		((Control)label14).AutoSize = true;
		((Control)label14).BackColor = Color.Transparent;
		((Control)label14).Cursor = Cursors.Default;
		((Control)label14).ForeColor = SystemColors.ControlText;
		((Control)label14).Location = new Point(161, 165);
		((Control)label14).Name = "label14";
		((Control)label14).RightToLeft = (RightToLeft)1;
		((Control)label14).Size = new Size(79, 16);
		((Control)label14).TabIndex = 106;
		((Control)label14).Text = "زبان دستگاه:";
		label14.TextAlign = (ContentAlignment)64;
		((Control)label14).Visible = false;
		((Control)label15).AutoSize = true;
		((Control)label15).BackColor = Color.Transparent;
		((Control)label15).Cursor = Cursors.Default;
		((Control)label15).ForeColor = SystemColors.ControlText;
		((Control)label15).Location = new Point(161, 68);
		((Control)label15).Name = "label15";
		((Control)label15).RightToLeft = (RightToLeft)1;
		((Control)label15).Size = new Size(77, 16);
		((Control)label15).TabIndex = 88;
		((Control)label15).Text = " نام دستگاه:";
		label15.TextAlign = (ContentAlignment)64;
		TxtTimeByUsb.AcceptsReturn = true;
		((Control)TxtTimeByUsb).BackColor = SystemColors.Window;
		((Control)TxtTimeByUsb).Cursor = Cursors.IBeam;
		((Control)TxtTimeByUsb).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)TxtTimeByUsb).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TxtTimeByUsb).Location = new Point(6, 128);
		((TextBoxBase)TxtTimeByUsb).MaxLength = 32;
		((Control)TxtTimeByUsb).Name = "TxtTimeByUsb";
		((Control)TxtTimeByUsb).RightToLeft = (RightToLeft)0;
		((Control)TxtTimeByUsb).Size = new Size(149, 23);
		((Control)TxtTimeByUsb).TabIndex = 103;
		txtSerialNoByUsb.AcceptsReturn = true;
		((Control)txtSerialNoByUsb).BackColor = SystemColors.Window;
		((Control)txtSerialNoByUsb).Cursor = Cursors.IBeam;
		((Control)txtSerialNoByUsb).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtSerialNoByUsb).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)txtSerialNoByUsb).Location = new Point(6, 96);
		((TextBoxBase)txtSerialNoByUsb).MaxLength = 32;
		((Control)txtSerialNoByUsb).Name = "txtSerialNoByUsb";
		((Control)txtSerialNoByUsb).RightToLeft = (RightToLeft)0;
		((Control)txtSerialNoByUsb).Size = new Size(149, 23);
		((Control)txtSerialNoByUsb).TabIndex = 85;
		((Control)label16).AutoSize = true;
		((Control)label16).BackColor = Color.Transparent;
		((Control)label16).Cursor = Cursors.Default;
		((Control)label16).ForeColor = SystemColors.ControlText;
		((Control)label16).Location = new Point(161, 133);
		((Control)label16).Name = "label16";
		((Control)label16).RightToLeft = (RightToLeft)1;
		((Control)label16).Size = new Size(94, 16);
		((Control)label16).TabIndex = 104;
		((Control)label16).Text = "ساعت دستگاه:";
		label16.TextAlign = (ContentAlignment)64;
		txtProductCodeByUsb.AcceptsReturn = true;
		((Control)txtProductCodeByUsb).BackColor = SystemColors.Window;
		((Control)txtProductCodeByUsb).Cursor = Cursors.IBeam;
		((Control)txtProductCodeByUsb).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtProductCodeByUsb).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)txtProductCodeByUsb).Location = new Point(6, 64);
		((TextBoxBase)txtProductCodeByUsb).MaxLength = 32;
		((Control)txtProductCodeByUsb).Name = "txtProductCodeByUsb";
		((Control)txtProductCodeByUsb).RightToLeft = (RightToLeft)0;
		((Control)txtProductCodeByUsb).Size = new Size(149, 23);
		((Control)txtProductCodeByUsb).TabIndex = 86;
		((Control)label17).AutoSize = true;
		((Control)label17).BackColor = Color.Transparent;
		((Control)label17).Cursor = Cursors.Default;
		((Control)label17).ForeColor = SystemColors.ControlText;
		((Control)label17).Location = new Point(161, 34);
		((Control)label17).Name = "label17";
		((Control)label17).RightToLeft = (RightToLeft)1;
		((Control)label17).Size = new Size(93, 16);
		((Control)label17).TabIndex = 71;
		((Control)label17).Text = "شماره دستگاه:";
		label17.TextAlign = (ContentAlignment)64;
		((Control)groupBox4).Controls.Add((Control)(object)PicSetLangByUsb);
		((Control)groupBox4).Controls.Add((Control)(object)CmdOpenByUsb);
		((Control)groupBox4).Controls.Add((Control)(object)label18);
		((Control)groupBox4).Controls.Add((Control)(object)PicSetTimeByUsb);
		((Control)groupBox4).Controls.Add((Control)(object)label20);
		((Control)groupBox4).Controls.Add((Control)(object)label21);
		((Control)groupBox4).Controls.Add((Control)(object)PicTimeByUsb);
		((Control)groupBox4).Controls.Add((Control)(object)PicConfigByUsb);
		((Control)groupBox4).Controls.Add((Control)(object)label22);
		((Control)groupBox4).Controls.Add((Control)(object)label23);
		((Control)groupBox4).Controls.Add((Control)(object)PicSerialByUsb);
		((Control)groupBox4).Controls.Add((Control)(object)PicNameDeviceByUsb);
		((Control)groupBox4).Controls.Add((Control)(object)label24);
		((Control)groupBox4).Location = new Point(26, 18);
		((Control)groupBox4).Name = "groupBox4";
		((Control)groupBox4).Size = new Size(279, 313);
		((Control)groupBox4).TabIndex = 109;
		groupBox4.TabStop = false;
		((Control)PicSetLangByUsb).Location = new Point(6, 247);
		((Control)PicSetLangByUsb).Name = "PicSetLangByUsb";
		((Control)PicSetLangByUsb).Size = new Size(26, 26);
		PicSetLangByUsb.SizeMode = (PictureBoxSizeMode)1;
		PicSetLangByUsb.TabIndex = 102;
		PicSetLangByUsb.TabStop = false;
		((Control)PicSetLangByUsb).Visible = false;
		((Control)CmdOpenByUsb).BackColor = SystemColors.Control;
		((Control)CmdOpenByUsb).Cursor = Cursors.Default;
		((Control)CmdOpenByUsb).ForeColor = SystemColors.ControlText;
		((Control)CmdOpenByUsb).Location = new Point(6, 26);
		((Control)CmdOpenByUsb).Name = "CmdOpenByUsb";
		((Control)CmdOpenByUsb).RightToLeft = (RightToLeft)0;
		((Control)CmdOpenByUsb).Size = new Size(257, 37);
		((Control)CmdOpenByUsb).TabIndex = 81;
		((Control)CmdOpenByUsb).Text = "بررسي دستگاه";
		((ButtonBase)CmdOpenByUsb).UseVisualStyleBackColor = false;
		((Control)CmdOpenByUsb).Click += CmdOpenByUsb_Click;
		((Control)label18).AutoSize = true;
		((Control)label18).BackColor = Color.Transparent;
		((Control)label18).Cursor = Cursors.Default;
		((Control)label18).ForeColor = SystemColors.ControlText;
		((Control)label18).Location = new Point(54, 251);
		((Control)label18).Name = "label18";
		((Control)label18).RightToLeft = (RightToLeft)1;
		((Control)label18).Size = new Size(184, 16);
		((Control)label18).TabIndex = 101;
		((Control)label18).Text = "تنظيم زبان فارسي روي دستگاه:";
		label18.TextAlign = (ContentAlignment)4;
		((Control)label18).Visible = false;
		((Control)PicSetTimeByUsb).Location = new Point(6, 214);
		((Control)PicSetTimeByUsb).Name = "PicSetTimeByUsb";
		((Control)PicSetTimeByUsb).Size = new Size(26, 26);
		PicSetTimeByUsb.SizeMode = (PictureBoxSizeMode)1;
		PicSetTimeByUsb.TabIndex = 100;
		PicSetTimeByUsb.TabStop = false;
		((Control)label20).AutoSize = true;
		((Control)label20).BackColor = Color.Transparent;
		((Control)label20).Cursor = Cursors.Default;
		((Control)label20).ForeColor = SystemColors.ControlText;
		((Control)label20).Location = new Point(54, 218);
		((Control)label20).Name = "label20";
		((Control)label20).RightToLeft = (RightToLeft)1;
		((Control)label20).Size = new Size(130, 16);
		((Control)label20).TabIndex = 99;
		((Control)label20).Text = "تنظيم ساعت دستگاه:";
		label20.TextAlign = (ContentAlignment)4;
		((Control)label21).AutoSize = true;
		((Control)label21).BackColor = Color.Transparent;
		((Control)label21).Cursor = Cursors.Default;
		((Control)label21).ForeColor = SystemColors.ControlText;
		((Control)label21).Location = new Point(54, 80);
		((Control)label21).Name = "label21";
		((Control)label21).RightToLeft = (RightToLeft)1;
		((Control)label21).Size = new Size(133, 16);
		((Control)label21).TabIndex = 91;
		((Control)label21).Text = "تست ارتباط با دستگاه:";
		label21.TextAlign = (ContentAlignment)4;
		((Control)PicTimeByUsb).Location = new Point(6, 182);
		((Control)PicTimeByUsb).Name = "PicTimeByUsb";
		((Control)PicTimeByUsb).Size = new Size(26, 26);
		PicTimeByUsb.SizeMode = (PictureBoxSizeMode)1;
		PicTimeByUsb.TabIndex = 98;
		PicTimeByUsb.TabStop = false;
		((Control)PicConfigByUsb).Location = new Point(6, 76);
		((Control)PicConfigByUsb).Name = "PicConfigByUsb";
		((Control)PicConfigByUsb).Size = new Size(26, 26);
		PicConfigByUsb.SizeMode = (PictureBoxSizeMode)1;
		PicConfigByUsb.TabIndex = 92;
		PicConfigByUsb.TabStop = false;
		((Control)label22).AutoSize = true;
		((Control)label22).BackColor = Color.Transparent;
		((Control)label22).Cursor = Cursors.Default;
		((Control)label22).ForeColor = SystemColors.ControlText;
		((Control)label22).Location = new Point(54, 186);
		((Control)label22).Name = "label22";
		((Control)label22).RightToLeft = (RightToLeft)1;
		((Control)label22).Size = new Size(134, 16);
		((Control)label22).TabIndex = 97;
		((Control)label22).Text = "دريافت ساعت دستگاه:";
		label22.TextAlign = (ContentAlignment)4;
		((Control)label23).AutoSize = true;
		((Control)label23).BackColor = Color.Transparent;
		((Control)label23).Cursor = Cursors.Default;
		((Control)label23).ForeColor = SystemColors.ControlText;
		((Control)label23).Location = new Point(54, 116);
		((Control)label23).Name = "label23";
		((Control)label23).RightToLeft = (RightToLeft)1;
		((Control)label23).Size = new Size(113, 16);
		((Control)label23).TabIndex = 93;
		((Control)label23).Text = "دريافت نام دستگاه:";
		label23.TextAlign = (ContentAlignment)4;
		((Control)PicSerialByUsb).Location = new Point(6, 148);
		((Control)PicSerialByUsb).Name = "PicSerialByUsb";
		((Control)PicSerialByUsb).Size = new Size(26, 26);
		PicSerialByUsb.SizeMode = (PictureBoxSizeMode)1;
		PicSerialByUsb.TabIndex = 96;
		PicSerialByUsb.TabStop = false;
		((Control)PicNameDeviceByUsb).Location = new Point(6, 112);
		((Control)PicNameDeviceByUsb).Name = "PicNameDeviceByUsb";
		((Control)PicNameDeviceByUsb).Size = new Size(26, 26);
		PicNameDeviceByUsb.SizeMode = (PictureBoxSizeMode)1;
		PicNameDeviceByUsb.TabIndex = 94;
		PicNameDeviceByUsb.TabStop = false;
		((Control)label24).AutoSize = true;
		((Control)label24).BackColor = Color.Transparent;
		((Control)label24).Cursor = Cursors.Default;
		((Control)label24).ForeColor = SystemColors.ControlText;
		((Control)label24).Location = new Point(54, 152);
		((Control)label24).Name = "label24";
		((Control)label24).RightToLeft = (RightToLeft)1;
		((Control)label24).Size = new Size(131, 16);
		((Control)label24).TabIndex = 95;
		((Control)label24).Text = "دريافت سريال دستگاه:";
		label24.TextAlign = (ContentAlignment)4;
		((Form)this).AutoScaleBaseSize = new Size(6, 16);
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(626, 444);
		((Control)this).Controls.Add((Control)(object)tabControl1);
		((Control)this).Controls.Add((Control)(object)LbSubject);
		((Control)this).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmCheckDeviceHysoon";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "فرم کانفیگ دستگاه هاي هایسون";
		((Control)tabControl1).ResumeLayout(false);
		((Control)tabPage1).ResumeLayout(false);
		((Control)Frame2).ResumeLayout(false);
		((Control)Frame2).PerformLayout();
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)PnlTest).ResumeLayout(false);
		((Control)PnlTest).PerformLayout();
		((ISupportInitialize)PicSetLangByLan).EndInit();
		((ISupportInitialize)PicSetTimeByLan).EndInit();
		((ISupportInitialize)PicPing).EndInit();
		((ISupportInitialize)PicTimeByLan).EndInit();
		((ISupportInitialize)PicConfigByLan).EndInit();
		((ISupportInitialize)PicSerialByLan).EndInit();
		((ISupportInitialize)PicNameDeviceByLan).EndInit();
		((Control)tabPage2).ResumeLayout(false);
		((Control)groupBox3).ResumeLayout(false);
		((Control)groupBox3).PerformLayout();
		((Control)groupBox4).ResumeLayout(false);
		((Control)groupBox4).PerformLayout();
		((ISupportInitialize)PicSetLangByUsb).EndInit();
		((ISupportInitialize)PicSetTimeByUsb).EndInit();
		((ISupportInitialize)PicTimeByUsb).EndInit();
		((ISupportInitialize)PicConfigByUsb).EndInit();
		((ISupportInitialize)PicSerialByUsb).EndInit();
		((ISupportInitialize)PicNameDeviceByUsb).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		GfrmMain = this;
		FKAttendDLL.nCommHandleIndex = 0;
		OpenFlag = false;
		((Control)TxtIpAddress).Text = "192.168.1.224";
		RegisterManagement.RegisterOCXs_NovinP(Application.StartupPath);
	}

	private void frmMain_Closed(object sender, EventArgs e)
	{
		cmdCloseComm_Click(sender, new EventArgs());
	}

	private void ResetFormByLan()
	{
		try
		{
			PicPing.Image = null;
			PicConfigByLan.Image = null;
			PicNameDeviceByLan.Image = null;
			PicSerialByLan.Image = null;
			PicTimeByLan.Image = null;
			PicSetTimeByLan.Image = null;
			PicSetLangByLan.Image = null;
			((Control)txtSerialNoByLan).ResetText();
			((Control)txtProductCodeByLan).ResetText();
			((Control)TxtTimeByLan).ResetText();
			((Control)TxtLangByLan).ResetText();
			((Control)TxtDeviceIdByLan).ResetText();
		}
		catch (Exception)
		{
		}
	}

	private void ResetFormByUsb()
	{
		try
		{
			PicConfigByUsb.Image = null;
			PicNameDeviceByUsb.Image = null;
			PicSerialByUsb.Image = null;
			PicTimeByUsb.Image = null;
			PicSetTimeByUsb.Image = null;
			PicSetLangByUsb.Image = null;
			((Control)txtSerialNoByUsb).ResetText();
			((Control)txtProductCodeByUsb).ResetText();
			((Control)TxtTimeByUsb).ResetText();
			((Control)TxtLangByUsb).ResetText();
			((Control)TxtDeviceIdByUsb).ResetText();
		}
		catch (Exception)
		{
		}
	}

	private void cmdCloseComm_Click(object sender, EventArgs e)
	{
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		if (OpenFlag)
		{
			FKAttendDLL.FK_DisConnect(FKAttendDLL.nCommHandleIndex);
			OpenFlag = false;
		}
	}

	private bool GetProuctData()
	{
		string astrItem = "";
		((Control)txtSerialNoByLan).Text = "";
		((Control)txtProductCodeByLan).Text = "";
		long num = FuncGetProductData(1L, ref astrItem);
		if (num == 1)
		{
			((Control)txtSerialNoByLan).Text = astrItem;
			PicSerialByLan.Image = (Image)(object)Resources.tik;
		}
		else
		{
			PicSerialByLan.Image = (Image)(object)Resources.cancell;
		}
		num = FuncGetProductData(3L, ref astrItem);
		if (num == 1)
		{
			((Control)txtProductCodeByLan).Text = astrItem;
			PicNameDeviceByLan.Image = (Image)(object)Resources.tik;
		}
		else
		{
			PicNameDeviceByLan.Image = (Image)(object)Resources.cancell;
		}
		return true;
	}

	private bool GetProuctDataByUsb()
	{
		string astrItem = "";
		((Control)txtSerialNoByUsb).Text = "";
		((Control)txtProductCodeByUsb).Text = "";
		long num = FuncGetProductData(1L, ref astrItem);
		if (num == 1)
		{
			((Control)txtSerialNoByUsb).Text = astrItem;
			PicSerialByUsb.Image = (Image)(object)Resources.tik;
		}
		else
		{
			PicSerialByUsb.Image = (Image)(object)Resources.cancell;
		}
		num = FuncGetProductData(3L, ref astrItem);
		if (num == 1)
		{
			((Control)txtProductCodeByUsb).Text = astrItem;
			PicNameDeviceByUsb.Image = (Image)(object)Resources.tik;
		}
		else
		{
			PicNameDeviceByUsb.Image = (Image)(object)Resources.cancell;
		}
		return true;
	}

	private long FuncGetProductData(long anIndex, ref string astrItem)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		string apstrValue = new string(' ', 256);
		long num = FKAttendDLL.FK_GetProductData(FKAttendDLL.nCommHandleIndex, (int)anIndex, ref apstrValue);
		if (num != 1)
		{
			MessageBox.Show("خطا? در?افت اطلاعات از دستگاه \r\n " + FKAttendDLL.ReturnResultPrint(num));
			return num;
		}
		astrItem = apstrValue.Trim();
		return num;
	}

	private string GetDeviceTime()
	{
		DateTime apnDateTime = DateTime.Now;
		string text = "";
		int num = FKAttendDLL.FK_GetDeviceTime(FKAttendDLL.nCommHandleIndex, ref apnDateTime);
		if (num == 1)
		{
			text = apnDateTime.ToShamsiDateTime() ?? "";
			((Control)TxtTimeByLan).Text = text;
			PicTimeByLan.Image = (Image)(object)Resources.tik;
		}
		else
		{
			PicTimeByLan.Image = (Image)(object)Resources.cancell;
		}
		return text;
	}

	private string GetDeviceTimeByUsb()
	{
		DateTime apnDateTime = DateTime.Now;
		string text = "";
		int num = FKAttendDLL.FK_GetDeviceTime(FKAttendDLL.nCommHandleIndex, ref apnDateTime);
		if (num == 1)
		{
			text = apnDateTime.ToShamsiDateTime() ?? "";
			((Control)TxtTimeByUsb).Text = text;
			PicTimeByUsb.Image = (Image)(object)Resources.tik;
		}
		else
		{
			PicTimeByUsb.Image = (Image)(object)Resources.cancell;
		}
		return text;
	}

	private void SetDeviceTime()
	{
		DateTime now = DateTime.Now;
		int num = FKAttendDLL.FK_SetDeviceTime(FKAttendDLL.nCommHandleIndex, now);
		if (num != 1)
		{
			PicSetTimeByLan.Image = (Image)(object)Resources.cancell;
		}
		else
		{
			PicSetTimeByLan.Image = (Image)(object)Resources.tik;
		}
		Thread.Sleep(100);
		GetDeviceTime();
		Application.DoEvents();
	}

	private void SetDeviceTimeByUsb()
	{
		DateTime now = DateTime.Now;
		int num = FKAttendDLL.FK_SetDeviceTime(FKAttendDLL.nCommHandleIndex, now);
		if (num != 1)
		{
			PicSetTimeByUsb.Image = (Image)(object)Resources.cancell;
		}
		else
		{
			PicSetTimeByUsb.Image = (Image)(object)Resources.tik;
		}
		Thread.Sleep(100);
		GetDeviceTimeByUsb();
		Application.DoEvents();
	}

	private void GetDeviceLanguage()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		int apnValue = 0;
		int anInfoIndex = 3;
		long num = FKAttendDLL.FK_GetDeviceInfo(FKAttendDLL.nCommHandleIndex, anInfoIndex, ref apnValue);
		if (num == 1)
		{
			if (apnValue == 0)
			{
				((Control)TxtLangByLan).Text = "فارسي";
			}
			else
			{
				((Control)TxtLangByLan).Text = "؟؟؟؟";
			}
		}
		else
		{
			MessageBox.Show("خطا در خواندن زبان دستگاه");
		}
	}

	private void SetDeviceLanguage()
	{
		int num = 0;
		int anInfoIndex = 3;
		num = 0;
		int num2 = FKAttendDLL.FK_SetDeviceInfo(FKAttendDLL.nCommHandleIndex, anInfoIndex, num);
		if ((long)num2 == 1)
		{
			PicSetLangByLan.Image = (Image)(object)Resources.tik;
			((Control)TxtLangByLan).Text = "فارسي";
		}
		else
		{
			PicSetLangByLan.Image = (Image)(object)Resources.cancell;
			((Control)TxtLangByLan).Text = "????";
		}
	}

	private void GetDeviceId()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		int apnValue = 0;
		int anInfoIndex = 2;
		long num = FKAttendDLL.FK_GetDeviceInfo(FKAttendDLL.nCommHandleIndex, anInfoIndex, ref apnValue);
		if (num == 1)
		{
			((Control)TxtDeviceIdByLan).Text = apnValue.ToString();
		}
		else
		{
			MessageBox.Show("خطا در خواندن زبان دستگاه");
		}
	}

	private void GetDeviceIdByUsb()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		int apnValue = 0;
		int anInfoIndex = 2;
		long num = FKAttendDLL.FK_GetDeviceInfo(FKAttendDLL.nCommHandleIndex, anInfoIndex, ref apnValue);
		if (num == 1)
		{
			((Control)TxtDeviceIdByUsb).Text = apnValue.ToString();
		}
		else
		{
			MessageBox.Show("خطا در خواندن زبان دستگاه");
		}
	}

	private void tabPage1_Click(object sender, EventArgs e)
	{
	}

	private void CmdOpenByUsb_Click(object sender, EventArgs e)
	{
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		int anMachineNo = 1;
		try
		{
			((Control)CmdOpenByUsb).Text = "در حال بررسي دستگاه...";
			ResetFormByUsb();
			Application.DoEvents();
			int anLicense = 1261;
			((Control)CmdOpenByUsb).Enabled = false;
			FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectUSB(anMachineNo, anLicense);
			if (FKAttendDLL.nCommHandleIndex > 0)
			{
				OpenFlag = true;
				PicConfigByUsb.Image = (Image)(object)Resources.tik;
				int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
				if (num != 1)
				{
					MessageBox.Show(" خطای دسترسی به \r\n No Device");
				}
				else if (GetProuctDataByUsb())
				{
					GetDeviceTimeByUsb();
					Application.DoEvents();
					Thread.Sleep(100);
					SetDeviceTimeByUsb();
					Application.DoEvents();
					Thread.Sleep(100);
					GetDeviceIdByUsb();
					Application.DoEvents();
				}
			}
			else
			{
				long anResultCode = FKAttendDLL.nCommHandleIndex;
				MessageBox.Show("ارتباط با دستگاه برقرار نشد \r\n " + FKAttendDLL.ReturnResultPrint(anResultCode), "خطای برقراری ارتباط", (MessageBoxButtons)0, (MessageBoxIcon)48);
				PicConfigByUsb.Image = (Image)(object)Resources.cancell;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		finally
		{
			cmdCloseComm_Click(sender, e);
			((Control)CmdOpenByUsb).Enabled = true;
			((Control)CmdOpenByUsb).Text = "بررسي دستگاه";
		}
	}

	private void CmdOpenByLan_Click(object sender, EventArgs e)
	{
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)CmdOpenByLan).Text = "در حال بررسي دستگاه...";
			ResetFormByLan();
			Application.DoEvents();
			int anTimeOut = 5000;
			int anLicense = 1261;
			int anProtocolType = 0;
			((Control)CmdOpenByLan).Enabled = false;
			string astrIpAddress = ((Control)TxtIpAddress).Text.Trim();
			int anNetPort = Convert.ToInt32(((Control)TxtPortNo).Text);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = Utility.Ping(((Control)TxtIpAddress).Text);
			if (returnedSaveFuncInfo.HasError)
			{
				PicPing.Image = (Image)(object)Resources.cancell;
				return;
			}
			PicPing.Image = (Image)(object)Resources.tik;
			FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectNet(1, astrIpAddress, anNetPort, anTimeOut, anProtocolType, 0, anLicense);
			if (FKAttendDLL.nCommHandleIndex > 0)
			{
				OpenFlag = true;
				PicConfigByLan.Image = (Image)(object)Resources.tik;
				int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
				if (num != 1)
				{
					MessageBox.Show(" خطای دسترسی به \r\n No Device");
				}
				else if (GetProuctData())
				{
					GetDeviceTime();
					Application.DoEvents();
					Thread.Sleep(100);
					SetDeviceTime();
					Application.DoEvents();
					Thread.Sleep(100);
					GetDeviceId();
					Application.DoEvents();
				}
			}
			else
			{
				long anResultCode = FKAttendDLL.nCommHandleIndex;
				MessageBox.Show("ارتباط با دستگاه برقرار نشد \r\n " + FKAttendDLL.ReturnResultPrint(anResultCode), "خطای برقراری ارتباط", (MessageBoxButtons)0, (MessageBoxIcon)48);
				PicConfigByLan.Image = (Image)(object)Resources.cancell;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		finally
		{
			cmdCloseComm_Click(sender, e);
			((Control)CmdOpenByLan).Enabled = true;
			((Control)CmdOpenByLan).Text = "بررسي دستگاه";
		}
	}
}
