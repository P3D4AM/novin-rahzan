using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ADODB;

namespace FKAttendDllCSSample;

public class frmEnroll : Form
{
	public Button cmdBenumbManager;

	internal OpenFileDialog dlgOpen;

	internal SaveFileDialog dlgSave;

	public Button cmdUSBSetAllData;

	public Button cmdUSBGetAllData;

	internal Label lblDBCount;

	public TextBox txtEnrollNumber;

	public Button cmdGetEnrollData;

	public Button cmdEmptyEnrollData;

	public Button cmdDisableUser;

	public ComboBox cmbBackupNumber;

	public ComboBox cmbPrivilege;

	public Button cmdDel;

	public Button cmdClearData;

	public Button cmdSetEnrollData;

	public Button cmdDeleteEnrollData;

	public Button cmdModifyPrivilege;

	public Button cmdEnableUser;

	public Button cmdSetAllEnrollData;

	public Button cmdGetAllEnrollData;

	public Button cmdGetEnrollInfo;

	public ListBox lstEnrollData;

	public Label lblEnrollNum;

	public Label lblTotal;

	public Label lblPrivilege;

	public Label lblEnrollData;

	public Label lblBackupNumber;

	public Label lblMessage;

	internal HScrollBar scbAdoScroll;

	private Container components = null;

	private bool mbGetState;

	private const int FP_DATA_SIZE = 1680;

	private const int FACE_DATA_SIZE = 20080;

	private const int PWD_DATA_SIZE = 40;

	private byte[] mbytCurEnrollData = new byte[20080];

	private int mnCurPassword = 0;

	private const string mcstrAdoConn = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=false;Data Source=";

	private Connection mAdoConnEnroll = (Connection)new ConnectionClass();

	private Recordset mAdoRstEnroll = (Recordset)new RecordsetClass();

	private Recordset mAdoRstTmp = (Recordset)new RecordsetClass();

	private object[] mListBoxItemBackupNumber;

	public frmEnroll()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
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
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_085c: Expected O, but got Unknown
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Expected O, but got Unknown
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Expected O, but got Unknown
		//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa3: Expected O, but got Unknown
		//IL_0b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Expected O, but got Unknown
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Expected O, but got Unknown
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Expected O, but got Unknown
		//IL_0e2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e38: Expected O, but got Unknown
		//IL_0f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1b: Expected O, but got Unknown
		//IL_0ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1001: Expected O, but got Unknown
		//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e7: Expected O, but got Unknown
		//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cd: Expected O, but got Unknown
		//IL_128e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1298: Expected O, but got Unknown
		//IL_1353: Unknown result type (might be due to invalid IL or missing references)
		//IL_135d: Expected O, but got Unknown
		//IL_141e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1428: Expected O, but got Unknown
		//IL_14e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f0: Expected O, but got Unknown
		//IL_15ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b8: Expected O, but got Unknown
		//IL_1673: Unknown result type (might be due to invalid IL or missing references)
		//IL_167d: Expected O, but got Unknown
		//IL_1779: Unknown result type (might be due to invalid IL or missing references)
		//IL_1783: Expected O, but got Unknown
		cmdBenumbManager = new Button();
		dlgOpen = new OpenFileDialog();
		dlgSave = new SaveFileDialog();
		cmdUSBSetAllData = new Button();
		cmdUSBGetAllData = new Button();
		lblDBCount = new Label();
		txtEnrollNumber = new TextBox();
		cmdGetEnrollData = new Button();
		cmdEmptyEnrollData = new Button();
		cmdDisableUser = new Button();
		cmbBackupNumber = new ComboBox();
		cmbPrivilege = new ComboBox();
		cmdDel = new Button();
		cmdClearData = new Button();
		cmdSetEnrollData = new Button();
		cmdDeleteEnrollData = new Button();
		cmdModifyPrivilege = new Button();
		cmdEnableUser = new Button();
		cmdSetAllEnrollData = new Button();
		cmdGetAllEnrollData = new Button();
		cmdGetEnrollInfo = new Button();
		lstEnrollData = new ListBox();
		lblEnrollNum = new Label();
		lblTotal = new Label();
		lblPrivilege = new Label();
		lblEnrollData = new Label();
		lblBackupNumber = new Label();
		lblMessage = new Label();
		scbAdoScroll = new HScrollBar();
		((Control)this).SuspendLayout();
		((Control)cmdBenumbManager).BackColor = SystemColors.Control;
		((Control)cmdBenumbManager).Cursor = Cursors.Default;
		((Control)cmdBenumbManager).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdBenumbManager).ForeColor = SystemColors.ControlText;
		((Control)cmdBenumbManager).Location = new Point(255, 477);
		((Control)cmdBenumbManager).Name = "cmdBenumbManager";
		((Control)cmdBenumbManager).RightToLeft = (RightToLeft)0;
		((Control)cmdBenumbManager).Size = new Size(216, 35);
		((Control)cmdBenumbManager).TabIndex = 83;
		((Control)cmdBenumbManager).Text = "Benumb All Managers";
		((ButtonBase)cmdBenumbManager).UseVisualStyleBackColor = false;
		((Control)cmdBenumbManager).Click += cmdBenumbManager_Click;
		((Control)cmdUSBSetAllData).BackColor = SystemColors.Control;
		((Control)cmdUSBSetAllData).Cursor = Cursors.Default;
		((Control)cmdUSBSetAllData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdUSBSetAllData).ForeColor = SystemColors.ControlText;
		((Control)cmdUSBSetAllData).Location = new Point(255, 317);
		((Control)cmdUSBSetAllData).Name = "cmdUSBSetAllData";
		((Control)cmdUSBSetAllData).RightToLeft = (RightToLeft)0;
		((Control)cmdUSBSetAllData).Size = new Size(216, 36);
		((Control)cmdUSBSetAllData).TabIndex = 64;
		((Control)cmdUSBSetAllData).Text = "Set All Enroll Data(USB)";
		((ButtonBase)cmdUSBSetAllData).UseVisualStyleBackColor = false;
		((Control)cmdUSBSetAllData).Click += cmdUSBSetAllData_Click;
		((Control)cmdUSBGetAllData).BackColor = SystemColors.Control;
		((Control)cmdUSBGetAllData).Cursor = Cursors.Default;
		((Control)cmdUSBGetAllData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdUSBGetAllData).ForeColor = SystemColors.ControlText;
		((Control)cmdUSBGetAllData).Location = new Point(255, 282);
		((Control)cmdUSBGetAllData).Name = "cmdUSBGetAllData";
		((Control)cmdUSBGetAllData).RightToLeft = (RightToLeft)0;
		((Control)cmdUSBGetAllData).Size = new Size(216, 36);
		((Control)cmdUSBGetAllData).TabIndex = 63;
		((Control)cmdUSBGetAllData).Text = "Get All Enroll Data(USB)";
		((ButtonBase)cmdUSBGetAllData).UseVisualStyleBackColor = false;
		((Control)cmdUSBGetAllData).Click += cmdUSBGetAllData_Click;
		((Control)lblDBCount).BackColor = Color.White;
		lblDBCount.BorderStyle = (BorderStyle)2;
		((Control)lblDBCount).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblDBCount).ForeColor = Color.Blue;
		((Control)lblDBCount).Location = new Point(36, 594);
		((Control)lblDBCount).Name = "lblDBCount";
		((Control)lblDBCount).Size = new Size(85, 28);
		((Control)lblDBCount).TabIndex = 81;
		lblDBCount.TextAlign = (ContentAlignment)32;
		txtEnrollNumber.AcceptsReturn = true;
		((Control)txtEnrollNumber).BackColor = SystemColors.Window;
		((Control)txtEnrollNumber).Cursor = Cursors.IBeam;
		((Control)txtEnrollNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEnrollNumber).ForeColor = SystemColors.WindowText;
		((Control)txtEnrollNumber).Location = new Point(151, 79);
		((TextBoxBase)txtEnrollNumber).MaxLength = 8;
		((Control)txtEnrollNumber).Name = "txtEnrollNumber";
		((Control)txtEnrollNumber).RightToLeft = (RightToLeft)0;
		((Control)txtEnrollNumber).Size = new Size(81, 26);
		((Control)txtEnrollNumber).TabIndex = 71;
		((Control)cmdGetEnrollData).BackColor = SystemColors.Control;
		((Control)cmdGetEnrollData).Cursor = Cursors.Default;
		((Control)cmdGetEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdGetEnrollData).Location = new Point(255, 79);
		((Control)cmdGetEnrollData).Name = "cmdGetEnrollData";
		((Control)cmdGetEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdGetEnrollData).Size = new Size(216, 36);
		((Control)cmdGetEnrollData).TabIndex = 58;
		((Control)cmdGetEnrollData).Text = "Get Enroll Data";
		((ButtonBase)cmdGetEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdGetEnrollData).Click += cmdGetEnrollData_Click;
		((Control)cmdEmptyEnrollData).BackColor = SystemColors.Control;
		((Control)cmdEmptyEnrollData).Cursor = Cursors.Default;
		((Control)cmdEmptyEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdEmptyEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdEmptyEnrollData).Location = new Point(255, 514);
		((Control)cmdEmptyEnrollData).Name = "cmdEmptyEnrollData";
		((Control)cmdEmptyEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdEmptyEnrollData).Size = new Size(216, 36);
		((Control)cmdEmptyEnrollData).TabIndex = 69;
		((Control)cmdEmptyEnrollData).Text = "Empty Enroll Data";
		((ButtonBase)cmdEmptyEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdEmptyEnrollData).Click += cmdEmptyEnrollData_Click;
		((Control)cmdDisableUser).BackColor = SystemColors.Control;
		((Control)cmdDisableUser).Cursor = Cursors.Default;
		((Control)cmdDisableUser).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdDisableUser).ForeColor = SystemColors.ControlText;
		((Control)cmdDisableUser).Location = new Point(363, 402);
		((Control)cmdDisableUser).Name = "cmdDisableUser";
		((Control)cmdDisableUser).RightToLeft = (RightToLeft)0;
		((Control)cmdDisableUser).Size = new Size(108, 37);
		((Control)cmdDisableUser).TabIndex = 67;
		((Control)cmdDisableUser).Text = "Disable User";
		((ButtonBase)cmdDisableUser).UseVisualStyleBackColor = false;
		((Control)cmdDisableUser).Click += cmdDisableUser_Click;
		((Control)cmbBackupNumber).BackColor = SystemColors.Window;
		((Control)cmbBackupNumber).Cursor = Cursors.Default;
		cmbBackupNumber.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbBackupNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbBackupNumber).ForeColor = SystemColors.WindowText;
		((Control)cmbBackupNumber).Location = new Point(151, 119);
		((Control)cmbBackupNumber).Name = "cmbBackupNumber";
		((Control)cmbBackupNumber).RightToLeft = (RightToLeft)0;
		((Control)cmbBackupNumber).Size = new Size(81, 27);
		((Control)cmbBackupNumber).TabIndex = 79;
		((Control)cmbPrivilege).BackColor = SystemColors.Window;
		((Control)cmbPrivilege).Cursor = Cursors.Default;
		cmbPrivilege.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbPrivilege).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbPrivilege).ForeColor = SystemColors.WindowText;
		((Control)cmbPrivilege).Location = new Point(151, 151);
		((Control)cmbPrivilege).Name = "cmbPrivilege";
		((Control)cmbPrivilege).RightToLeft = (RightToLeft)0;
		((Control)cmbPrivilege).Size = new Size(81, 27);
		((Control)cmbPrivilege).TabIndex = 78;
		((Control)cmdDel).BackColor = SystemColors.Control;
		((Control)cmdDel).Cursor = Cursors.Default;
		((Control)cmdDel).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdDel).ForeColor = SystemColors.ControlText;
		((Control)cmdDel).Location = new Point(139, 592);
		((Control)cmdDel).Name = "cmdDel";
		((Control)cmdDel).RightToLeft = (RightToLeft)0;
		((Control)cmdDel).Size = new Size(96, 30);
		((Control)cmdDel).TabIndex = 57;
		((Control)cmdDel).Text = "Delete DB";
		((ButtonBase)cmdDel).UseVisualStyleBackColor = false;
		((Control)cmdDel).Click += cmdDel_Click;
		((Control)cmdClearData).BackColor = SystemColors.Control;
		((Control)cmdClearData).Cursor = Cursors.Default;
		((Control)cmdClearData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdClearData).ForeColor = SystemColors.ControlText;
		((Control)cmdClearData).Location = new Point(255, 551);
		((Control)cmdClearData).Name = "cmdClearData";
		((Control)cmdClearData).RightToLeft = (RightToLeft)0;
		((Control)cmdClearData).Size = new Size(216, 36);
		((Control)cmdClearData).TabIndex = 70;
		((Control)cmdClearData).Text = "Clear All Data(E,GL,SL) ";
		((ButtonBase)cmdClearData).UseVisualStyleBackColor = false;
		((Control)cmdClearData).Click += cmdClearData_Click;
		((Control)cmdSetEnrollData).BackColor = SystemColors.Control;
		((Control)cmdSetEnrollData).Cursor = Cursors.Default;
		((Control)cmdSetEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdSetEnrollData).Location = new Point(255, 115);
		((Control)cmdSetEnrollData).Name = "cmdSetEnrollData";
		((Control)cmdSetEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdSetEnrollData).Size = new Size(216, 36);
		((Control)cmdSetEnrollData).TabIndex = 59;
		((Control)cmdSetEnrollData).Text = "Set Enroll Data";
		((ButtonBase)cmdSetEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdSetEnrollData).Click += cmdSetEnrollData_Click;
		((Control)cmdDeleteEnrollData).BackColor = SystemColors.Control;
		((Control)cmdDeleteEnrollData).Cursor = Cursors.Default;
		((Control)cmdDeleteEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdDeleteEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdDeleteEnrollData).Location = new Point(255, 151);
		((Control)cmdDeleteEnrollData).Name = "cmdDeleteEnrollData";
		((Control)cmdDeleteEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdDeleteEnrollData).Size = new Size(216, 36);
		((Control)cmdDeleteEnrollData).TabIndex = 60;
		((Control)cmdDeleteEnrollData).Text = "Delete Enroll Data";
		((ButtonBase)cmdDeleteEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdDeleteEnrollData).Click += cmdDeleteEnrollData_Click;
		((Control)cmdModifyPrivilege).BackColor = SystemColors.Control;
		((Control)cmdModifyPrivilege).Cursor = Cursors.Default;
		((Control)cmdModifyPrivilege).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdModifyPrivilege).ForeColor = SystemColors.ControlText;
		((Control)cmdModifyPrivilege).Location = new Point(255, 440);
		((Control)cmdModifyPrivilege).Name = "cmdModifyPrivilege";
		((Control)cmdModifyPrivilege).RightToLeft = (RightToLeft)0;
		((Control)cmdModifyPrivilege).Size = new Size(216, 36);
		((Control)cmdModifyPrivilege).TabIndex = 68;
		((Control)cmdModifyPrivilege).Text = "ModifyPrivilege";
		((ButtonBase)cmdModifyPrivilege).UseVisualStyleBackColor = false;
		((Control)cmdModifyPrivilege).Click += cmdModifyPrivilege_Click;
		((Control)cmdEnableUser).BackColor = SystemColors.Control;
		((Control)cmdEnableUser).Cursor = Cursors.Default;
		((Control)cmdEnableUser).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdEnableUser).ForeColor = SystemColors.ControlText;
		((Control)cmdEnableUser).Location = new Point(255, 402);
		((Control)cmdEnableUser).Name = "cmdEnableUser";
		((Control)cmdEnableUser).RightToLeft = (RightToLeft)0;
		((Control)cmdEnableUser).Size = new Size(108, 37);
		((Control)cmdEnableUser).TabIndex = 66;
		((Control)cmdEnableUser).Text = "Enable User";
		((ButtonBase)cmdEnableUser).UseVisualStyleBackColor = false;
		((Control)cmdEnableUser).Click += cmdEnableUser_Click;
		((Control)cmdSetAllEnrollData).BackColor = SystemColors.Control;
		((Control)cmdSetAllEnrollData).Cursor = Cursors.Default;
		((Control)cmdSetAllEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetAllEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdSetAllEnrollData).Location = new Point(255, 235);
		((Control)cmdSetAllEnrollData).Name = "cmdSetAllEnrollData";
		((Control)cmdSetAllEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdSetAllEnrollData).Size = new Size(216, 36);
		((Control)cmdSetAllEnrollData).TabIndex = 62;
		((Control)cmdSetAllEnrollData).Text = "Set All Enroll Data";
		((ButtonBase)cmdSetAllEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdSetAllEnrollData).Click += cmdSetAllEnrollData_Click;
		((Control)cmdGetAllEnrollData).BackColor = SystemColors.Control;
		((Control)cmdGetAllEnrollData).Cursor = Cursors.Default;
		((Control)cmdGetAllEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetAllEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdGetAllEnrollData).Location = new Point(255, 199);
		((Control)cmdGetAllEnrollData).Name = "cmdGetAllEnrollData";
		((Control)cmdGetAllEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdGetAllEnrollData).Size = new Size(216, 36);
		((Control)cmdGetAllEnrollData).TabIndex = 61;
		((Control)cmdGetAllEnrollData).Text = "Get All Enroll Data";
		((ButtonBase)cmdGetAllEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdGetAllEnrollData).Click += cmdGetAllEnrollData_Click;
		((Control)cmdGetEnrollInfo).BackColor = SystemColors.Control;
		((Control)cmdGetEnrollInfo).Cursor = Cursors.Default;
		((Control)cmdGetEnrollInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetEnrollInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetEnrollInfo).Location = new Point(255, 365);
		((Control)cmdGetEnrollInfo).Name = "cmdGetEnrollInfo";
		((Control)cmdGetEnrollInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetEnrollInfo).Size = new Size(216, 36);
		((Control)cmdGetEnrollInfo).TabIndex = 65;
		((Control)cmdGetEnrollInfo).Text = "Get Enroll Info";
		((ButtonBase)cmdGetEnrollInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetEnrollInfo).Click += cmdGetEnrollInfo_Click;
		((Control)lstEnrollData).BackColor = SystemColors.Window;
		((Control)lstEnrollData).Cursor = Cursors.Default;
		((Control)lstEnrollData).Font = new Font("Courier New", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lstEnrollData).ForeColor = SystemColors.WindowText;
		lstEnrollData.ItemHeight = 15;
		((Control)lstEnrollData).Location = new Point(15, 225);
		((Control)lstEnrollData).Name = "lstEnrollData";
		((Control)lstEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lstEnrollData).Size = new Size(220, 364);
		((Control)lstEnrollData).TabIndex = 73;
		((Control)lblEnrollNum).AutoSize = true;
		((Control)lblEnrollNum).BackColor = SystemColors.Control;
		((Control)lblEnrollNum).Cursor = Cursors.Default;
		((Control)lblEnrollNum).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollNum).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollNum).Location = new Point(15, 87);
		((Control)lblEnrollNum).Name = "lblEnrollNum";
		((Control)lblEnrollNum).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollNum).Size = new Size(105, 19);
		((Control)lblEnrollNum).TabIndex = 80;
		((Control)lblEnrollNum).Text = "Enroll Number :";
		((Control)lblTotal).AutoSize = true;
		((Control)lblTotal).BackColor = SystemColors.Control;
		((Control)lblTotal).Cursor = Cursors.Default;
		((Control)lblTotal).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblTotal).ForeColor = SystemColors.ControlText;
		((Control)lblTotal).Location = new Point(143, 197);
		((Control)lblTotal).Name = "lblTotal";
		((Control)lblTotal).RightToLeft = (RightToLeft)0;
		((Control)lblTotal).Size = new Size(51, 19);
		((Control)lblTotal).TabIndex = 77;
		((Control)lblTotal).Text = "Total : ";
		((Control)lblPrivilege).AutoSize = true;
		((Control)lblPrivilege).BackColor = SystemColors.Control;
		((Control)lblPrivilege).Cursor = Cursors.Default;
		((Control)lblPrivilege).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblPrivilege).ForeColor = SystemColors.ControlText;
		((Control)lblPrivilege).Location = new Point(15, 159);
		((Control)lblPrivilege).Name = "lblPrivilege";
		((Control)lblPrivilege).RightToLeft = (RightToLeft)0;
		((Control)lblPrivilege).Size = new Size(67, 19);
		((Control)lblPrivilege).TabIndex = 76;
		((Control)lblPrivilege).Text = "Privilege :";
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(15, 197);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(99, 19);
		((Control)lblEnrollData).TabIndex = 74;
		((Control)lblEnrollData).Text = "Enrolled Data :";
		((Control)lblBackupNumber).AutoSize = true;
		((Control)lblBackupNumber).BackColor = SystemColors.Control;
		((Control)lblBackupNumber).Cursor = Cursors.Default;
		((Control)lblBackupNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblBackupNumber).ForeColor = SystemColors.ControlText;
		((Control)lblBackupNumber).Location = new Point(15, 119);
		((Control)lblBackupNumber).Name = "lblBackupNumber";
		((Control)lblBackupNumber).RightToLeft = (RightToLeft)0;
		((Control)lblBackupNumber).Size = new Size(117, 19);
		((Control)lblBackupNumber).TabIndex = 72;
		((Control)lblBackupNumber).Text = "Backup Number :";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(15, 15);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(456, 29);
		((Control)lblMessage).TabIndex = 75;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((ScrollBar)scbAdoScroll).LargeChange = 2;
		((Control)scbAdoScroll).Location = new Point(19, 592);
		((ScrollBar)scbAdoScroll).Maximum = 1;
		((Control)scbAdoScroll).Name = "scbAdoScroll";
		((Control)scbAdoScroll).Size = new Size(120, 30);
		((Control)scbAdoScroll).TabIndex = 82;
		((ScrollBar)scbAdoScroll).Scroll += new ScrollEventHandler(scbAdoScroll_Scroll);
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(494, 648);
		((Control)this).Controls.Add((Control)(object)cmdUSBGetAllData);
		((Control)this).Controls.Add((Control)(object)lblDBCount);
		((Control)this).Controls.Add((Control)(object)txtEnrollNumber);
		((Control)this).Controls.Add((Control)(object)lblEnrollNum);
		((Control)this).Controls.Add((Control)(object)lblTotal);
		((Control)this).Controls.Add((Control)(object)lblPrivilege);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblBackupNumber);
		((Control)this).Controls.Add((Control)(object)cmdGetEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdEmptyEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdDisableUser);
		((Control)this).Controls.Add((Control)(object)cmbBackupNumber);
		((Control)this).Controls.Add((Control)(object)cmbPrivilege);
		((Control)this).Controls.Add((Control)(object)cmdDel);
		((Control)this).Controls.Add((Control)(object)cmdClearData);
		((Control)this).Controls.Add((Control)(object)cmdSetEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdDeleteEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdModifyPrivilege);
		((Control)this).Controls.Add((Control)(object)cmdEnableUser);
		((Control)this).Controls.Add((Control)(object)cmdSetAllEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdGetAllEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdGetEnrollInfo);
		((Control)this).Controls.Add((Control)(object)lstEnrollData);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)scbAdoScroll);
		((Control)this).Controls.Add((Control)(object)cmdBenumbManager);
		((Control)this).Controls.Add((Control)(object)cmdUSBSetAllData);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmEnroll";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Manage Enroll Data";
		((Form)this).Load += frmEnroll_Load;
		((Form)this).Closed += frmEnroll_Closed;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void frmEnroll_Load(object sender, EventArgs e)
	{
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Invalid comparison between Unknown and I4
		mbGetState = false;
		mListBoxItemBackupNumber = new object[26]
		{
			enumBackupNumberType.BACKUP_FP_0,
			"Fp-0",
			enumBackupNumberType.BACKUP_FP_1,
			"Fp-1",
			enumBackupNumberType.BACKUP_FP_2,
			"Fp-2",
			enumBackupNumberType.BACKUP_FP_3,
			"Fp-3",
			enumBackupNumberType.BACKUP_FP_4,
			"Fp-4",
			enumBackupNumberType.BACKUP_FP_5,
			"Fp-5",
			enumBackupNumberType.BACKUP_FP_6,
			"Fp-6",
			enumBackupNumberType.BACKUP_FP_7,
			"Fp-7",
			enumBackupNumberType.BACKUP_FP_8,
			"Fp-8",
			enumBackupNumberType.BACKUP_FP_9,
			"Fp-9",
			enumBackupNumberType.BACKUP_PSW,
			"Pass",
			enumBackupNumberType.BACKUP_CARD,
			"Card",
			enumBackupNumberType.BACKUP_FACE,
			"Face"
		};
		((Control)lblMessage).Text = "";
		((Control)txtEnrollNumber).Text = "1";
		cmbBackupNumber.Items.Clear();
		for (int i = 1; i <= mListBoxItemBackupNumber.GetUpperBound(0); i += 2)
		{
			cmbBackupNumber.Items.Add(mListBoxItemBackupNumber[i]);
		}
		((ListControl)cmbBackupNumber).SelectedIndex = 0;
		cmbPrivilege.Items.Clear();
		cmbPrivilege.Items.Add((object)"User");
		cmbPrivilege.Items.Add((object)"Manager");
		((ListControl)cmbPrivilege).SelectedIndex = 0;
		lstEnrollData.Items.Clear();
		DBWithItemEnable(abEnableFlag: false);
		string text = Application.StartupPath + "\\datEnrollDat.mdb";
		if (!File.Exists(text))
		{
			((FileDialog)dlgOpen).InitialDirectory = Application.StartupPath;
			((FileDialog)dlgOpen).Filter = "DB Files (*.mdb)|*.mdb|All Files (*.*)|*.*";
			((FileDialog)dlgOpen).FilterIndex = 1;
			if ((int)((CommonDialog)dlgOpen).ShowDialog() != 1)
			{
				goto IL_037b;
			}
			text = ((FileDialog)dlgOpen).FileName;
			((FileDialog)dlgOpen).FileName = "";
		}
		frmEnroll_Closed(sender, e);
		((_Connection)mAdoConnEnroll).CommandTimeout = 300;
		((_Connection)mAdoConnEnroll).ConnectionTimeout = 300;
		((_Connection)mAdoConnEnroll).ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=false;Data Source=" + text;
		((_Connection)mAdoConnEnroll).Open("", "", "", -1);
		if (((_Connection)mAdoConnEnroll).State == 1)
		{
			((_Recordset)mAdoRstEnroll).CacheSize = 1000;
			((_Recordset)mAdoRstEnroll).CursorLocation = (CursorLocationEnum)3;
			((_Recordset)mAdoRstEnroll).Open((object)"select * from tblEnroll", (object)mAdoConnEnroll, (CursorTypeEnum)3, (LockTypeEnum)2, -1);
			if (((_Recordset)mAdoRstEnroll).State == 1 && !((_Recordset)mAdoRstEnroll).BOF)
			{
				((_Recordset)mAdoRstEnroll).MoveFirst();
			}
			AdoEnroll_MoveComplete();
			((_Recordset)mAdoRstTmp).CacheSize = 1000;
			((_Recordset)mAdoRstTmp).CursorLocation = (CursorLocationEnum)3;
			DBWithItemEnable(abEnableFlag: true);
		}
		goto IL_037b;
		IL_037b:
		if (!FrmConnectDeviceHysoon.GfrmMain.OpenFlag)
		{
			disableButtons();
		}
	}

	private void frmEnroll_Closed(object sender, EventArgs e)
	{
		if (((_Recordset)mAdoRstEnroll).State == 1)
		{
			((_Recordset)mAdoRstEnroll).Close();
		}
		if (((_Connection)mAdoConnEnroll).State == 1)
		{
			((_Connection)mAdoConnEnroll).Close();
		}
	}

	private void scbAdoScroll_Scroll(object sender, ScrollEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		if (((_Recordset)mAdoRstEnroll).RecordCount > 0)
		{
			ScrollEventType type = e.Type;
			ScrollEventType val = type;
			if ((int)val != 0)
			{
				if ((int)val != 1)
				{
					return;
				}
				if (!((_Recordset)mAdoRstEnroll).EOF)
				{
					((_Recordset)mAdoRstEnroll).MoveNext();
					if (((_Recordset)mAdoRstEnroll).EOF)
					{
						((_Recordset)mAdoRstEnroll).MoveLast();
					}
				}
			}
			else if (!((_Recordset)mAdoRstEnroll).BOF)
			{
				((_Recordset)mAdoRstEnroll).MovePrevious();
				if (((_Recordset)mAdoRstEnroll).BOF)
				{
					((_Recordset)mAdoRstEnroll).MoveFirst();
				}
			}
		}
		AdoEnroll_MoveComplete();
	}

	private void AdoEnroll_MoveComplete()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		int anEnrollNumber = 0;
		string astrEnrollName = "";
		int anBackupNumber = 0;
		int anPrivilege = 0;
		if (!mbGetState)
		{
			int num = Convert.ToInt32(((_Recordset)mAdoRstEnroll).AbsolutePosition);
			if (num < 0)
			{
				num = 0;
			}
			((Control)lblDBCount).Text = "  " + num + "/" + ((_Recordset)mAdoRstEnroll).RecordCount;
			if (((_Recordset)mAdoRstEnroll).RecordCount > 0)
			{
				readEnrollDataFromDB(ref anEnrollNumber, ref anBackupNumber, ref anPrivilege, ref astrEnrollName, abdispFlag: true);
			}
		}
	}

	private void cmdDel_Click(object sender, EventArgs e)
	{
		((Control)cmdDel).Enabled = false;
		Application.DoEvents();
		if (((_Recordset)mAdoRstTmp).State == 1)
		{
			((_Recordset)mAdoRstTmp).Close();
		}
		((_Recordset)mAdoRstTmp).Open((object)"Delete From tblEnroll", (object)mAdoConnEnroll, (CursorTypeEnum)3, (LockTypeEnum)2, -1);
		((_Recordset)mAdoRstEnroll).Requery(-1);
		AdoEnroll_MoveComplete();
		((Control)cmdDel).Enabled = true;
	}

	private void zeroArray(byte[] aArray)
	{
		for (int i = 0; i < aArray.Length; i++)
		{
			aArray[i] = 0;
		}
	}

	private void cmdGetEnrollData_Click(object sender, EventArgs e)
	{
		int apnMachinePrivilege = 1;
		((Control)cmdGetEnrollData).Enabled = false;
		lstEnrollData.Items.Clear();
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetEnrollData).Enabled = true;
			return;
		}
		int anEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int curSelBackupNumberFromCombo = getCurSelBackupNumberFromCombo();
		Array.Clear(mbytCurEnrollData, 0, mbytCurEnrollData.Length);
		num = FKAttendDLL.FK_GetEnrollData(FKAttendDLL.nCommHandleIndex, anEnrollNumber, curSelBackupNumberFromCombo, ref apnMachinePrivilege, mbytCurEnrollData, ref mnCurPassword);
		if (num == 1)
		{
			if (apnMachinePrivilege == 1)
			{
				((ListControl)cmbPrivilege).SelectedIndex = 1;
			}
			else
			{
				((ListControl)cmbPrivilege).SelectedIndex = 0;
			}
			showEnrollDataToListbox(curSelBackupNumberFromCombo, mbytCurEnrollData);
			((Control)lblMessage).Text = "GetEnrollData OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetEnrollData).Enabled = true;
	}

	private void cmdSetEnrollData_Click(object sender, EventArgs e)
	{
		((Control)cmdSetEnrollData).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int anEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int curSelBackupNumberFromCombo = getCurSelBackupNumberFromCombo();
		int anMachinePrivilege = ((((ListControl)cmbPrivilege).SelectedIndex == 1) ? 1 : 0);
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetEnrollData).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, anEnrollNumber, curSelBackupNumberFromCombo, anMachinePrivilege, mbytCurEnrollData, mnCurPassword);
		if (num == 1)
		{
			((Control)lblMessage).Text = "Saving...";
			Application.DoEvents();
			num = FKAttendDLL.FK_SaveEnrollData(FKAttendDLL.nCommHandleIndex);
			if (num == 1)
			{
				((Control)lblMessage).Text = "SetEnrollData OK";
			}
		}
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetEnrollData).Enabled = true;
	}

	private void cmdDeleteEnrollData_Click(object sender, EventArgs e)
	{
		((Control)cmdDeleteEnrollData).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdDeleteEnrollData).Enabled = true;
			return;
		}
		int anEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int curSelBackupNumberFromCombo = getCurSelBackupNumberFromCombo();
		num = FKAttendDLL.FK_DeleteEnrollData(FKAttendDLL.nCommHandleIndex, anEnrollNumber, curSelBackupNumberFromCombo);
		if (num == 1)
		{
			((Control)lblMessage).Text = "DeleteEnrollData OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdDeleteEnrollData).Enabled = true;
	}

	private void cmdGetAllEnrollData_Click(object sender, EventArgs e)
	{
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Invalid comparison between Unknown and I4
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Invalid comparison between Unknown and I4
		int apnEnrollNumber = 0;
		int apnBackupNumber = 0;
		int apnMachinePrivilege = 0;
		int apnEnable = 0;
		((Control)cmdGetAllEnrollData).Enabled = false;
		lstEnrollData.Items.Clear();
		string text = ((Control)this).Text;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetAllEnrollData).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_ReadAllUserID(FKAttendDLL.nCommHandleIndex);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
			((Control)cmdGetAllEnrollData).Enabled = true;
			return;
		}
		((Control)this).Cursor = Cursors.WaitCursor;
		mbGetState = true;
		while (true)
		{
			string astrEnrollName = new string(' ', 256);
			while (true)
			{
				num = FKAttendDLL.FK_GetAllUserID(FKAttendDLL.nCommHandleIndex, ref apnEnrollNumber, ref apnBackupNumber, ref apnMachinePrivilege, ref apnEnable);
				if (num != 1)
				{
					if (num == -7)
					{
						num = 1;
					}
					goto IL_01c6;
				}
				DialogResult val;
				while (true)
				{
					num = FKAttendDLL.FK_GetEnrollData(FKAttendDLL.nCommHandleIndex, apnEnrollNumber, apnBackupNumber, ref apnMachinePrivilege, mbytCurEnrollData, ref mnCurPassword);
					if (num == 1)
					{
						break;
					}
					val = MessageBox.Show(FKAttendDLL.ReturnResultPrint(num) + ": Retry (y) / Get Next (n) ?", "GetEnrollData", (MessageBoxButtons)3);
					if ((int)val == 6)
					{
						continue;
					}
					goto IL_017a;
				}
				break;
				IL_017a:
				if ((int)val == 7)
				{
					continue;
				}
				if ((int)val != 2)
				{
					break;
				}
				goto IL_01c6;
				IL_01c6:
				mbGetState = false;
				Application.DoEvents();
				if (((_Recordset)mAdoRstEnroll).RecordCount > 1)
				{
					((_Recordset)mAdoRstEnroll).MoveFirst();
					((_Recordset)mAdoRstEnroll).MoveLast();
				}
				((Control)this).Text = text;
				((Control)this).Cursor = Cursors.Default;
				if (num == 1)
				{
					((Control)lblMessage).Text = "GetAllEnrollData OK";
				}
				else
				{
					((Control)lblMessage).Text = "GetAllEnrollData Error : " + FKAttendDLL.ReturnResultPrint(num);
				}
				FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
				((Control)cmdGetAllEnrollData).Enabled = true;
				return;
			}
			saveEnrollDataToDB(apnEnrollNumber, apnBackupNumber, apnMachinePrivilege, astrEnrollName);
			((Control)this).Text = apnEnrollNumber.ToString("0000#");
			Application.DoEvents();
			bool flag = true;
		}
	}

	private void cmdSetAllEnrollData_Click(object sender, EventArgs e)
	{
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Invalid comparison between Unknown and I4
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Invalid comparison between Unknown and I4
		int anEnrollNumber = 0;
		string astrEnrollName = "";
		int anBackupNumber = 0;
		int anPrivilege = 0;
		string text = "";
		((Control)cmdSetAllEnrollData).Enabled = false;
		lstEnrollData.Items.Clear();
		string text2 = ((Control)this).Text;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetAllEnrollData).Enabled = true;
			return;
		}
		mbGetState = true;
		((Control)this).Cursor = Cursors.WaitCursor;
		if (((_Recordset)mAdoRstEnroll).RecordCount > 0)
		{
			((_Recordset)mAdoRstEnroll).MoveLast();
			((_Recordset)mAdoRstEnroll).MoveFirst();
			while (true)
			{
				if (!readEnrollDataFromDB(ref anEnrollNumber, ref anBackupNumber, ref anPrivilege, ref astrEnrollName, abdispFlag: false))
				{
					text = "SetAllEnrollData Error";
					break;
				}
				int apnSupportFlag = 0;
				FKAttendDLL.FK_IsSupportedEnrollData(FKAttendDLL.nCommHandleIndex, anBackupNumber, ref apnSupportFlag);
				if (apnSupportFlag == 0)
				{
					((_Recordset)mAdoRstEnroll).MoveNext();
					if (((_Recordset)mAdoRstEnroll).EOF)
					{
						break;
					}
					continue;
				}
				num = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, anEnrollNumber, anBackupNumber, anPrivilege, mbytCurEnrollData, mnCurPassword);
				if (num != 1)
				{
					text = "SetAllEnrollData Error";
					DialogResult val = MessageBox.Show(FKAttendDLL.ReturnResultPrint(num) + ": Retry (y) / Move Next (n) ?", "SetEnrollData", (MessageBoxButtons)3);
					if ((int)val == 6)
					{
						continue;
					}
					if ((int)val == 2)
					{
						break;
					}
				}
				Label obj = lblMessage;
				string[] obj2 = new string[6]
				{
					"ID = ",
					anEnrollNumber.ToString("000#"),
					", FpNo = ",
					anBackupNumber.ToString(),
					", Count = ",
					null
				};
				PositionEnum absolutePosition = ((_Recordset)mAdoRstEnroll).AbsolutePosition;
				obj2[5] = ((object)(PositionEnum)(ref absolutePosition)).ToString();
				((Control)obj).Text = string.Concat(obj2);
				absolutePosition = ((_Recordset)mAdoRstEnroll).AbsolutePosition;
				((Control)this).Text = ((object)(PositionEnum)(ref absolutePosition)).ToString().Trim();
				Application.DoEvents();
				((_Recordset)mAdoRstEnroll).MoveNext();
				if (((_Recordset)mAdoRstEnroll).EOF)
				{
					break;
				}
			}
		}
		((Control)this).Text = text2;
		((Control)this).Cursor = Cursors.Default;
		mbGetState = false;
		if (num == 1)
		{
			((Control)lblMessage).Text = "Saving...";
			Application.DoEvents();
			num = FKAttendDLL.FK_SaveEnrollData(FKAttendDLL.nCommHandleIndex);
			if (num == 1)
			{
				((Control)lblMessage).Text = "SetAllEnrollData OK";
			}
			else
			{
				((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			}
		}
		else
		{
			((Control)lblMessage).Text = text + " : " + FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetAllEnrollData).Enabled = true;
	}

	private void cmdUSBGetAllData_Click(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int apnEnrollNumber = 0;
		int apnBackupNumber = 0;
		int apnMachinePrivilege = 0;
		int apnEnableFlag = 0;
		((FileDialog)dlgOpen).InitialDirectory = Directory.GetCurrentDirectory();
		((FileDialog)dlgOpen).Filter = "DAT Files (*.dat)|*.dat|All Files (*.*)|*.*";
		((FileDialog)dlgOpen).FilterIndex = 1;
		if ((int)((CommonDialog)dlgOpen).ShowDialog() != 1)
		{
			return;
		}
		string fileName = ((FileDialog)dlgOpen).FileName;
		((FileDialog)dlgOpen).FileName = "";
		((Control)cmdUSBGetAllData).Enabled = false;
		lstEnrollData.Items.Clear();
		string text = ((Control)this).Text;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		frmSelectFKFirmwareVer frmSelectFKFirmwareVer2 = new frmSelectFKFirmwareVer();
		((Form)frmSelectFKFirmwareVer2).ShowDialog();
		num = FKAttendDLL.FK_SetUDiskFileFKModel(FKAttendDLL.nCommHandleIndex, frmSelectFKFirmwareVer2.FKFirmware);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			((Control)cmdUSBGetAllData).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_USBReadAllEnrollDataFromFile(FKAttendDLL.nCommHandleIndex, fileName);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			((Control)cmdUSBGetAllData).Enabled = true;
			return;
		}
		((Control)this).Cursor = Cursors.WaitCursor;
		mbGetState = true;
		while (true)
		{
			string apstrEnrollName = new string(' ', 256);
			num = FKAttendDLL.FK_USBGetOneEnrollData(FKAttendDLL.nCommHandleIndex, ref apnEnrollNumber, ref apnBackupNumber, ref apnMachinePrivilege, mbytCurEnrollData, ref mnCurPassword, ref apnEnableFlag, ref apstrEnrollName);
			switch (num)
			{
			case -7:
				num = 1;
				break;
			case 1:
				goto IL_01a6;
			}
			break;
			IL_01a6:
			saveEnrollDataToDB(apnEnrollNumber, apnBackupNumber, apnMachinePrivilege, apstrEnrollName);
			((Control)this).Text = apnEnrollNumber.ToString("0000#");
			Application.DoEvents();
			bool flag = true;
		}
		mbGetState = false;
		Application.DoEvents();
		if (((_Recordset)mAdoRstEnroll).RecordCount > 1)
		{
			((_Recordset)mAdoRstEnroll).MoveFirst();
			((_Recordset)mAdoRstEnroll).MoveLast();
		}
		((Control)this).Text = text;
		((Control)this).Cursor = Cursors.Default;
		if (num == 1)
		{
			((Control)lblMessage).Text = "GetAllEnrollData(USB) OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		((Control)cmdUSBGetAllData).Enabled = true;
	}

	private void cmdUSBSetAllData_Click(object sender, EventArgs e)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Invalid comparison between Unknown and I4
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Invalid comparison between Unknown and I4
		int anEnrollNumber = 0;
		string text = "";
		int anBackupNumber = 0;
		int anPrivilege = 0;
		int num = 0;
		lstEnrollData.Items.Clear();
		string text2 = "";
		((FileDialog)dlgSave).InitialDirectory = Directory.GetCurrentDirectory();
		((FileDialog)dlgSave).Filter = "DAT Files (*.dat)|*.dat|All Files (*.*)|*.*";
		((FileDialog)dlgSave).FilterIndex = 1;
		if ((int)((CommonDialog)dlgSave).ShowDialog() != 1)
		{
			return;
		}
		string fileName = ((FileDialog)dlgSave).FileName;
		((FileDialog)dlgSave).FileName = "";
		((Control)cmdUSBSetAllData).Enabled = false;
		string text3 = ((Control)this).Text;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		frmSelectFKFirmwareVer frmSelectFKFirmwareVer2 = new frmSelectFKFirmwareVer();
		((Form)frmSelectFKFirmwareVer2).ShowDialog();
		num = FKAttendDLL.FK_SetUDiskFileFKModel(FKAttendDLL.nCommHandleIndex, frmSelectFKFirmwareVer2.FKFirmware);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			((Control)cmdUSBSetAllData).Enabled = true;
			return;
		}
		mbGetState = true;
		((Control)this).Cursor = Cursors.WaitCursor;
		if (((_Recordset)mAdoRstEnroll).RecordCount > 0)
		{
			((_Recordset)mAdoRstEnroll).MoveLast();
			((_Recordset)mAdoRstEnroll).MoveFirst();
			while (true)
			{
				IL_0157:
				text = new string(' ', 256);
				while (true)
				{
					if (!readEnrollDataFromDB(ref anEnrollNumber, ref anBackupNumber, ref anPrivilege, ref text, abdispFlag: false))
					{
						text2 = "SetAllEnrollData(USB) Error";
						break;
					}
					int anEnableFlag = 1;
					num = FKAttendDLL.FK_USBSetOneEnrollData(FKAttendDLL.nCommHandleIndex, anEnrollNumber, anBackupNumber, anPrivilege, mbytCurEnrollData, mnCurPassword, anEnableFlag, text);
					if (num != 1)
					{
						if (num == 0)
						{
							((_Recordset)mAdoRstEnroll).MoveNext();
							if (((_Recordset)mAdoRstEnroll).EOF)
							{
								break;
							}
							continue;
						}
						text2 = "USBSetOneEnrollData Error";
						DialogResult val = MessageBox.Show(FKAttendDLL.ReturnResultPrint(num) + ": Retry (y) / Move Next (n) ?", text2, (MessageBoxButtons)3);
						if ((int)val == 6)
						{
							continue;
						}
						if ((int)val == 2)
						{
							break;
						}
					}
					Label obj = lblMessage;
					string[] obj2 = new string[6]
					{
						"ID = ",
						anEnrollNumber.ToString("000#"),
						", FpNo = ",
						anBackupNumber.ToString(),
						", Count = ",
						null
					};
					PositionEnum absolutePosition = ((_Recordset)mAdoRstEnroll).AbsolutePosition;
					obj2[5] = ((object)(PositionEnum)(ref absolutePosition)).ToString();
					((Control)obj).Text = string.Concat(obj2);
					absolutePosition = ((_Recordset)mAdoRstEnroll).AbsolutePosition;
					((Control)this).Text = ((object)(PositionEnum)(ref absolutePosition)).ToString().Trim();
					Application.DoEvents();
					((_Recordset)mAdoRstEnroll).MoveNext();
					if (((_Recordset)mAdoRstEnroll).EOF)
					{
						break;
					}
					goto IL_0157;
				}
				break;
			}
		}
		((Control)this).Text = text3;
		((Control)this).Cursor = Cursors.Default;
		mbGetState = false;
		if (num == 1)
		{
			num = FKAttendDLL.FK_USBWriteAllEnrollDataToFile(FKAttendDLL.nCommHandleIndex, fileName);
			if (num == 1)
			{
				((Control)lblMessage).Text = "USBWriteAllEnrollDataToFile OK";
			}
			else
			{
				((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			}
		}
		else
		{
			((Control)lblMessage).Text = text2;
		}
		((Control)cmdUSBSetAllData).Enabled = true;
	}

	private void cmdGetEnrollInfo_Click(object sender, EventArgs e)
	{
		int apnEnrollNumber = 0;
		int apnBackupNumber = 0;
		int apnMachinePrivilege = 0;
		int apnEnable = 0;
		((Control)cmdGetEnrollInfo).Enabled = false;
		((Control)lblEnrollData).Text = "User IDs";
		lstEnrollData.Items.Clear();
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetEnrollInfo).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_ReadAllUserID(FKAttendDLL.nCommHandleIndex);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
			((Control)cmdGetEnrollInfo).Enabled = true;
			return;
		}
		int num2 = 0;
		lstEnrollData.Items.Add((object)" No.         EnNo           Fp        Priv   Enable");
		while (true)
		{
			num = FKAttendDLL.FK_GetAllUserID(FKAttendDLL.nCommHandleIndex, ref apnEnrollNumber, ref apnBackupNumber, ref apnMachinePrivilege, ref apnEnable);
			string text;
			string text2;
			string text3;
			switch (num)
			{
			case -7:
				num = 1;
				break;
			case 1:
				text = ((apnMachinePrivilege != 1) ? "User" : "Man");
				text2 = FuncStringFromBackupNumber(apnBackupNumber);
				text3 = ((apnEnable != 1) ? "D" : "E");
				goto IL_016a;
			}
			break;
			IL_016a:
			lstEnrollData.Items.Add((object)(num2.ToString("000#") + "    " + apnEnrollNumber.ToString("0000000#") + "      " + text2 + "     " + text + "       " + text3));
			num2++;
			((Control)lblTotal).Text = "Total : " + num2;
			Application.DoEvents();
			bool flag = true;
		}
		if (num == 1)
		{
			((Control)lblMessage).Text = "GetEnrollInfo OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetEnrollInfo).Enabled = true;
	}

	private void FuncSetUserEnableStatus(int abEnableFlag)
	{
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int anEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int curSelBackupNumberFromCombo = getCurSelBackupNumberFromCombo();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			return;
		}
		num = FKAttendDLL.FK_EnableUser(FKAttendDLL.nCommHandleIndex, anEnrollNumber, curSelBackupNumberFromCombo, abEnableFlag);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
	}

	private void cmdEnableUser_Click(object sender, EventArgs e)
	{
		((Control)cmdEnableUser).Enabled = false;
		FuncSetUserEnableStatus(1);
		((Control)cmdEnableUser).Enabled = true;
	}

	private void cmdDisableUser_Click(object sender, EventArgs e)
	{
		((Control)cmdDisableUser).Enabled = false;
		FuncSetUserEnableStatus(0);
		((Control)cmdDisableUser).Enabled = true;
	}

	private void cmdModifyPrivilege_Click(object sender, EventArgs e)
	{
		((Control)cmdModifyPrivilege).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int anEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int curSelBackupNumberFromCombo = getCurSelBackupNumberFromCombo();
		int anMachinePrivilege = ((((ListControl)cmbPrivilege).SelectedIndex == 1) ? 1 : 0);
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdModifyPrivilege).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_ModifyPrivilege(FKAttendDLL.nCommHandleIndex, anEnrollNumber, curSelBackupNumberFromCombo, anMachinePrivilege);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdModifyPrivilege).Enabled = true;
	}

	private void cmdBenumbManager_Click(object sender, EventArgs e)
	{
		((Control)cmdBenumbManager).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		long num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdBenumbManager).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_BenumbAllManager(FKAttendDLL.nCommHandleIndex);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdBenumbManager).Enabled = true;
	}

	private void cmdEmptyEnrollData_Click(object sender, EventArgs e)
	{
		((Control)cmdEmptyEnrollData).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdEmptyEnrollData).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_EmptyEnrollData(FKAttendDLL.nCommHandleIndex);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdEmptyEnrollData).Enabled = true;
	}

	private void cmdClearData_Click(object sender, EventArgs e)
	{
		((Control)cmdClearData).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdClearData).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_ClearKeeperData(FKAttendDLL.nCommHandleIndex);
		if (num == 1)
		{
			((Control)lblMessage).Text = "ClearKeeperData OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdClearData).Enabled = true;
	}

	private void DBWithItemEnable(bool abEnableFlag)
	{
		((Control)scbAdoScroll).Enabled = abEnableFlag;
		((Control)lblDBCount).Enabled = abEnableFlag;
		((Control)cmdDel).Enabled = abEnableFlag;
		((Control)cmdGetAllEnrollData).Enabled = abEnableFlag;
		((Control)cmdSetAllEnrollData).Enabled = abEnableFlag;
		((Control)cmdUSBGetAllData).Enabled = abEnableFlag;
		((Control)cmdUSBSetAllData).Enabled = abEnableFlag;
	}

	private void disableButtons()
	{
		((Control)cmdGetEnrollData).Enabled = false;
		((Control)cmdSetEnrollData).Enabled = false;
		((Control)cmdDeleteEnrollData).Enabled = false;
		((Control)cmdGetAllEnrollData).Enabled = false;
		((Control)cmdSetAllEnrollData).Enabled = false;
		((Control)cmdGetEnrollInfo).Enabled = false;
		((Control)cmdEnableUser).Enabled = false;
		((Control)cmdDisableUser).Enabled = false;
		((Control)cmdModifyPrivilege).Enabled = false;
		((Control)cmdBenumbManager).Enabled = false;
		((Control)cmdClearData).Enabled = false;
		((Control)cmdEmptyEnrollData).Enabled = false;
	}

	private void showByteArrayToListbox(ref ListBox aListBox, byte[] abytArray, uint aLenToShow)
	{
		aListBox.Items.Clear();
		int num = (int)aLenToShow;
		if (num > abytArray.Length)
		{
			num = abytArray.Length;
		}
		int i;
		string text;
		for (i = 0; i < num / 8; i++)
		{
			text = "";
			for (int j = 0; j < 8; j++)
			{
				text += abytArray[i * 8 + j].ToString("X2");
				text += " ";
			}
			lstEnrollData.Items.Add((object)text);
		}
		text = "";
		for (int j = i * 8; j < num; j++)
		{
			text = text + abytArray[j].ToString("X2") + " ";
		}
		lstEnrollData.Items.Add((object)text);
	}

	private void showEnrollDataToListbox(int anBackupNumber, byte[] abytEnrollData)
	{
		lstEnrollData.Items.Clear();
		((Control)lblEnrollData).Text = "Enrolled Data :";
		((Control)lblTotal).Text = "";
		if (anBackupNumber == 10 || anBackupNumber == 11)
		{
			showByteArrayToListbox(ref lstEnrollData, abytEnrollData, 40u);
		}
		else if (anBackupNumber >= 0 && anBackupNumber <= 9)
		{
			showByteArrayToListbox(ref lstEnrollData, abytEnrollData, 1680u);
		}
		else if (anBackupNumber == 12)
		{
			showByteArrayToListbox(ref lstEnrollData, abytEnrollData, 20080u);
		}
	}

	private void convFpDataToSaveInDbForCompatibility(byte[] abytSrc, ref byte[] abytDest)
	{
		int num = abytSrc.Length / 4;
		int num2 = num * 5;
		byte[] array = new byte[num2];
		byte[] array2 = new byte[4];
		for (int i = 0; i < num; i++)
		{
			array2[0] = abytSrc[i * 4];
			array2[1] = abytSrc[i * 4 + 1];
			array2[2] = abytSrc[i * 4 + 2];
			array2[3] = abytSrc[i * 4 + 3];
			int num3 = BitConverter.ToInt32(array2, 0);
			array[i * 5] = 1;
			if (num3 < 0)
			{
				if (num3 == int.MinValue)
				{
					array[i * 5] = 2;
					num3 = int.MaxValue;
				}
				else
				{
					array[i * 5] = 0;
					num3 = -num3;
				}
			}
			array2 = BitConverter.GetBytes(num3);
			array[i * 5 + 1] = array2[3];
			array[i * 5 + 2] = array2[2];
			array[i * 5 + 3] = array2[1];
			array[i * 5 + 4] = array2[0];
		}
		abytDest = array;
	}

	private void saveEnrollDataToDB(int anEnrollNumber, int anBackupNumber, int anPrivilege, string astrEnrollName)
	{
		string text = "select count(*) from tblEnroll where EnrollNumber=" + anEnrollNumber + " and FingerNumber=" + anBackupNumber;
		if (((_Recordset)mAdoRstTmp).State == 1)
		{
			((_Recordset)mAdoRstTmp).Close();
		}
		((_Recordset)mAdoRstTmp).Open((object)text, (object)mAdoConnEnroll, (CursorTypeEnum)3, (LockTypeEnum)2, -1);
		long num = Convert.ToInt32(((_Recordset)mAdoRstTmp).Fields[(object)0].Value);
		((_Recordset)mAdoRstTmp).Close();
		if (num > 0)
		{
			((Control)lblMessage).Text = "Double ID : " + anEnrollNumber.ToString("0000#") + "-" + anBackupNumber;
			lstEnrollData.Items.Add((object)((Control)lblMessage).Text);
			return;
		}
		object value = Missing.Value;
		((_Recordset)mAdoRstEnroll).AddNew(value, value);
		((_Recordset)mAdoRstEnroll).Fields[(object)"EnrollNumber"].Value = anEnrollNumber;
		((_Recordset)mAdoRstEnroll).Fields[(object)"FingerNumber"].Value = anBackupNumber;
		((_Recordset)mAdoRstEnroll).Fields[(object)"Privilige"].Value = anPrivilege;
		if (((_Recordset)mAdoRstEnroll).Fields[(object)"EnrollName"] != null || ((_Recordset)mAdoRstEnroll).Fields[(object)"EnrollName"].Value != null)
		{
			((_Recordset)mAdoRstEnroll).Fields[(object)"EnrollName"].Value = astrEnrollName.Trim();
		}
		if (anBackupNumber == 10 || anBackupNumber == 11)
		{
			byte[] array = new byte[40];
			Array.Copy(mbytCurEnrollData, array, 40);
			((_Recordset)mAdoRstEnroll).Fields[(object)"FPdata"].Value = array;
		}
		else if (anBackupNumber >= 0 && anBackupNumber <= 9)
		{
			byte[] array = new byte[1680];
			Array.Copy(mbytCurEnrollData, array, 1680);
			byte[] abytDest = new byte[1];
			convFpDataToSaveInDbForCompatibility(array, ref abytDest);
			((_Recordset)mAdoRstEnroll).Fields[(object)"FPdata"].Value = abytDest;
		}
		else if (anBackupNumber == 12)
		{
			byte[] array = new byte[20080];
			Array.Copy(mbytCurEnrollData, array, 20080);
			((_Recordset)mAdoRstEnroll).Fields[(object)"FPdata"].Value = array;
		}
		((_Recordset)mAdoRstEnroll).Update(value, value);
		((_Recordset)mAdoRstEnroll).UpdateBatch((AffectEnum)3);
		AdoEnroll_MoveComplete();
		((Control)lblMessage).Text = anEnrollNumber.ToString("0000#") + "-" + anBackupNumber;
		((Control)txtEnrollNumber).Text = anEnrollNumber.ToString().Trim();
		((ListControl)cmbBackupNumber).SelectedIndex = getComboItemIndexFromBackupNumber(anBackupNumber);
		if (anPrivilege == 1)
		{
			((ListControl)cmbPrivilege).SelectedIndex = 1;
		}
		else
		{
			((ListControl)cmbPrivilege).SelectedIndex = 0;
		}
		Application.DoEvents();
	}

	private void convFpDataAfterReadFromDbForCompatibility(byte[] abytSrc, ref byte[] abytDest)
	{
		int num = abytSrc.Length / 5;
		int num2 = num * 4;
		if (num2 < 1680)
		{
			num2 = 1680;
		}
		byte[] array = new byte[num2];
		byte[] array2 = new byte[4];
		for (int i = 0; i < num; i++)
		{
			array2[0] = abytSrc[i * 5 + 4];
			array2[1] = abytSrc[i * 5 + 3];
			array2[2] = abytSrc[i * 5 + 2];
			array2[3] = abytSrc[i * 5 + 1];
			int num3 = BitConverter.ToInt32(array2, 0);
			if (abytSrc[i * 5] == 0)
			{
				num3 = -num3;
			}
			else if (abytSrc[i * 5] == 2)
			{
				num3 = int.MinValue;
			}
			array2 = BitConverter.GetBytes(num3);
			array[i * 4 + 3] = array2[3];
			array[i * 4 + 2] = array2[2];
			array[i * 4 + 1] = array2[1];
			array[i * 4] = array2[0];
		}
		abytDest = array;
	}

	private bool readEnrollDataFromDB(ref int anEnrollNumber, ref int anBackupNumber, ref int anPrivilege, ref string astrEnrollName, bool abdispFlag)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		if (((_Recordset)mAdoRstEnroll).RecordCount <= 0)
		{
			return false;
		}
		if ((int)((_Recordset)mAdoRstEnroll).AbsolutePosition <= 0)
		{
			return false;
		}
		if ((int)((_Recordset)mAdoRstEnroll).Fields[(object)"EnrollNumber"].Value <= 0)
		{
			return false;
		}
		anEnrollNumber = (int)((_Recordset)mAdoRstEnroll).Fields[(object)"EnrollNumber"].Value;
		((Control)txtEnrollNumber).Text = anEnrollNumber.ToString().Trim();
		anBackupNumber = (int)((_Recordset)mAdoRstEnroll).Fields[(object)"FingerNumber"].Value;
		((ListControl)cmbBackupNumber).SelectedIndex = getComboItemIndexFromBackupNumber(anBackupNumber);
		anPrivilege = (int)((_Recordset)mAdoRstEnroll).Fields[(object)"Privilige"].Value;
		if (anPrivilege == 1)
		{
			((ListControl)cmbPrivilege).SelectedIndex = 1;
		}
		else
		{
			((ListControl)cmbPrivilege).SelectedIndex = 0;
		}
		if (anBackupNumber >= 0 && anBackupNumber <= 9)
		{
			byte[] array = (byte[])((_Recordset)mAdoRstEnroll).Fields[(object)"FPdata"].Value;
			byte[] abytDest = new byte[1680];
			if (array.Length < 1680)
			{
				Array.Clear(mbytCurEnrollData, 0, mbytCurEnrollData.Length);
			}
			else
			{
				convFpDataAfterReadFromDbForCompatibility(array, ref abytDest);
				Array.Copy(abytDest, mbytCurEnrollData, 1680);
			}
		}
		else if (anBackupNumber == 10 || anBackupNumber == 11)
		{
			byte[] array2 = new byte[0];
			try
			{
				array2 = (byte[])((_Recordset)mAdoRstEnroll).Fields[(object)"FPdata"].Value;
			}
			catch
			{
				array2 = new byte[0];
				mnCurPassword = (int)((_Recordset)mAdoRstEnroll).Fields[(object)"Password"].Value;
			}
			if (array2.Length < 40)
			{
				Array.Clear(mbytCurEnrollData, 0, mbytCurEnrollData.Length);
			}
			else
			{
				Array.Copy(array2, mbytCurEnrollData, 40);
			}
		}
		else if (anBackupNumber == 12)
		{
			byte[] array3 = (byte[])((_Recordset)mAdoRstEnroll).Fields[(object)"FPdata"].Value;
			if (array3.Length < 20080)
			{
				Array.Clear(mbytCurEnrollData, 0, mbytCurEnrollData.Length);
			}
			else
			{
				Array.Copy(array3, mbytCurEnrollData, 20080);
			}
		}
		astrEnrollName = ((_Recordset)mAdoRstEnroll).Fields[(object)"EnrollNumber"].Value.ToString();
		if (abdispFlag)
		{
			showEnrollDataToListbox(anBackupNumber, mbytCurEnrollData);
		}
		return true;
	}

	private int getCurSelBackupNumberFromCombo()
	{
		int num = ((ListControl)cmbBackupNumber).SelectedIndex;
		if (num < 0)
		{
			num = 0;
		}
		return (int)mListBoxItemBackupNumber[num * 2];
	}

	private int getComboItemIndexFromBackupNumber(int anBackupNumber)
	{
		for (int i = 0; i < mListBoxItemBackupNumber.GetUpperBound(0); i += 2)
		{
			if ((int)mListBoxItemBackupNumber[i] == anBackupNumber)
			{
				return i / 2;
			}
		}
		return -1;
	}

	private string FuncStringFromBackupNumber(int anBackupNumber)
	{
		string result = "        ";
		for (int i = 0; i < mListBoxItemBackupNumber.GetUpperBound(0); i += 2)
		{
			if ((int)mListBoxItemBackupNumber[i] == anBackupNumber)
			{
				result = (string)mListBoxItemBackupNumber[i + 1];
				if (anBackupNumber != 10 && anBackupNumber != 11)
				{
					result += " ";
				}
				return result;
			}
		}
		return result;
	}
}
