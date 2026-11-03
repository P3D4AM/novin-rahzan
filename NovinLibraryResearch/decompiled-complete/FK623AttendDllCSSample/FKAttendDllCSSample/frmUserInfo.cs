using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmUserInfo : Form
{
	private struct ExtCmd_USERVID
	{
		public ExtCmdStructHeader CmdHeader;

		public int UserID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public byte[] UserVID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] UserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 448)]
		public byte[] resered;

		public void Init(string asCmdCode, int aUserID)
		{
			CmdHeader = default(ExtCmdStructHeader);
			CmdHeader.Init(asCmdCode, 1, 644);
			UserVID = new byte[64];
			UserName = new byte[128];
			resered = new byte[448];
			UserID = aUserID;
		}

		public void SetUserVID(string usrVId)
		{
			FKAttendDLL.StringToByteArrayUtf16(usrVId, UserVID);
		}

		public string GetUserVID()
		{
			return FKAttendDLL.ByteArrayUtf16ToString(UserVID);
		}

		public void SetUserName(string usrName)
		{
			FKAttendDLL.StringToByteArrayUtf16(usrName, UserName);
		}

		public string GetUserName()
		{
			return FKAttendDLL.ByteArrayUtf16ToString(UserName);
		}
	}

	public TextBox txtEnrollNumber;

	public TextBox txtName;

	public Button cmdGetUserNameVID;

	public Button cmdSetUserNameVID;

	public Label lblMessage;

	public Label lblEnrollNum;

	public Label Label3;

	public TextBox txtVID;

	public Label label1;

	public const int VER_USERCUSTOMINFO_V1 = 1;

	public const int SIZE_USERCUSTOMINFO_V1 = 644;

	public const int SIZE_USERVID = 64;

	public static string EXTCMD_GET_USERCUSTOMINFO = "GetUserCustomInfo";

	public static string EXTCMD_SET_USERCUSTOMINFO = "SetUserCustomInfo";

	public Button cmdGetUserName;

	public Button cmdSetUserName;

	private Container components = null;

	private const short NAMESIZE = 400;

	private int[] glngUserName = new int[400];

	public frmUserInfo()
	{
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
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Expected O, but got Unknown
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Expected O, but got Unknown
		txtEnrollNumber = new TextBox();
		txtName = new TextBox();
		cmdGetUserNameVID = new Button();
		cmdSetUserNameVID = new Button();
		lblMessage = new Label();
		lblEnrollNum = new Label();
		Label3 = new Label();
		txtVID = new TextBox();
		label1 = new Label();
		cmdGetUserName = new Button();
		cmdSetUserName = new Button();
		((Control)this).SuspendLayout();
		txtEnrollNumber.AcceptsReturn = true;
		((Control)txtEnrollNumber).BackColor = SystemColors.Window;
		((Control)txtEnrollNumber).Cursor = Cursors.IBeam;
		((Control)txtEnrollNumber).ForeColor = SystemColors.WindowText;
		((Control)txtEnrollNumber).Location = new Point(119, 60);
		((TextBoxBase)txtEnrollNumber).MaxLength = 8;
		((Control)txtEnrollNumber).Name = "txtEnrollNumber";
		((Control)txtEnrollNumber).RightToLeft = (RightToLeft)0;
		((Control)txtEnrollNumber).Size = new Size(114, 26);
		((Control)txtEnrollNumber).TabIndex = 21;
		txtName.AcceptsReturn = true;
		((Control)txtName).BackColor = SystemColors.Window;
		((Control)txtName).Cursor = Cursors.IBeam;
		((Control)txtName).ForeColor = SystemColors.WindowText;
		((Control)txtName).Location = new Point(119, 93);
		((TextBoxBase)txtName).MaxLength = 10;
		((Control)txtName).Name = "txtName";
		((Control)txtName).RightToLeft = (RightToLeft)0;
		((Control)txtName).Size = new Size(114, 26);
		((Control)txtName).TabIndex = 33;
		((Control)cmdGetUserNameVID).BackColor = SystemColors.Control;
		((Control)cmdGetUserNameVID).Cursor = Cursors.Default;
		((Control)cmdGetUserNameVID).ForeColor = SystemColors.ControlText;
		((Control)cmdGetUserNameVID).Location = new Point(272, 168);
		((Control)cmdGetUserNameVID).Name = "cmdGetUserNameVID";
		((Control)cmdGetUserNameVID).RightToLeft = (RightToLeft)0;
		((Control)cmdGetUserNameVID).Size = new Size(169, 32);
		((Control)cmdGetUserNameVID).TabIndex = 18;
		((Control)cmdGetUserNameVID).Text = "Get User Name, VID";
		((ButtonBase)cmdGetUserNameVID).UseVisualStyleBackColor = false;
		((Control)cmdGetUserNameVID).Click += cmdGetUserNameVID_Click;
		((Control)cmdSetUserNameVID).BackColor = SystemColors.Control;
		((Control)cmdSetUserNameVID).Cursor = Cursors.Default;
		((Control)cmdSetUserNameVID).ForeColor = SystemColors.ControlText;
		((Control)cmdSetUserNameVID).Location = new Point(272, 129);
		((Control)cmdSetUserNameVID).Name = "cmdSetUserNameVID";
		((Control)cmdSetUserNameVID).RightToLeft = (RightToLeft)0;
		((Control)cmdSetUserNameVID).Size = new Size(169, 32);
		((Control)cmdSetUserNameVID).TabIndex = 17;
		((Control)cmdSetUserNameVID).Text = "Set User Name , VID";
		((ButtonBase)cmdSetUserNameVID).UseVisualStyleBackColor = false;
		((Control)cmdSetUserNameVID).Click += cmdSetUserNameVID_Click;
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(15, 14);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(426, 33);
		((Control)lblMessage).TabIndex = 27;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)32;
		((Control)lblEnrollNum).AutoSize = true;
		((Control)lblEnrollNum).BackColor = SystemColors.Control;
		((Control)lblEnrollNum).Cursor = Cursors.Default;
		((Control)lblEnrollNum).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollNum).Location = new Point(9, 64);
		((Control)lblEnrollNum).Name = "lblEnrollNum";
		((Control)lblEnrollNum).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollNum).Size = new Size(105, 19);
		((Control)lblEnrollNum).TabIndex = 24;
		((Control)lblEnrollNum).Text = "Enroll Number :";
		lblEnrollNum.TextAlign = (ContentAlignment)16;
		((Control)Label3).AutoSize = true;
		((Control)Label3).BackColor = SystemColors.Control;
		((Control)Label3).Cursor = Cursors.Default;
		((Control)Label3).ForeColor = SystemColors.ControlText;
		((Control)Label3).Location = new Point(11, 96);
		((Control)Label3).Name = "Label3";
		((Control)Label3).RightToLeft = (RightToLeft)0;
		((Control)Label3).Size = new Size(53, 19);
		((Control)Label3).TabIndex = 23;
		((Control)Label3).Text = "Name :";
		Label3.TextAlign = (ContentAlignment)16;
		txtVID.AcceptsReturn = true;
		((Control)txtVID).BackColor = SystemColors.Window;
		((Control)txtVID).Cursor = Cursors.IBeam;
		((Control)txtVID).ForeColor = SystemColors.WindowText;
		((Control)txtVID).Location = new Point(120, 126);
		((TextBoxBase)txtVID).MaxLength = 10;
		((Control)txtVID).Name = "txtVID";
		((Control)txtVID).RightToLeft = (RightToLeft)0;
		((Control)txtVID).Size = new Size(114, 26);
		((Control)txtVID).TabIndex = 37;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = SystemColors.Control;
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).ForeColor = SystemColors.ControlText;
		((Control)label1).Location = new Point(12, 129);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)0;
		((Control)label1).Size = new Size(75, 19);
		((Control)label1).TabIndex = 36;
		((Control)label1).Text = "Virtual ID :";
		label1.TextAlign = (ContentAlignment)16;
		((Control)cmdGetUserName).BackColor = SystemColors.Control;
		((Control)cmdGetUserName).Cursor = Cursors.Default;
		((Control)cmdGetUserName).ForeColor = SystemColors.ControlText;
		((Control)cmdGetUserName).Location = new Point(272, 89);
		((Control)cmdGetUserName).Name = "cmdGetUserName";
		((Control)cmdGetUserName).RightToLeft = (RightToLeft)0;
		((Control)cmdGetUserName).Size = new Size(169, 30);
		((Control)cmdGetUserName).TabIndex = 39;
		((Control)cmdGetUserName).Text = "Get User Name";
		((ButtonBase)cmdGetUserName).UseVisualStyleBackColor = false;
		((Control)cmdGetUserName).Click += cmdGetUserName_Click;
		((Control)cmdSetUserName).BackColor = SystemColors.Control;
		((Control)cmdSetUserName).Cursor = Cursors.Default;
		((Control)cmdSetUserName).ForeColor = SystemColors.ControlText;
		((Control)cmdSetUserName).Location = new Point(272, 50);
		((Control)cmdSetUserName).Name = "cmdSetUserName";
		((Control)cmdSetUserName).RightToLeft = (RightToLeft)0;
		((Control)cmdSetUserName).Size = new Size(169, 30);
		((Control)cmdSetUserName).TabIndex = 38;
		((Control)cmdSetUserName).Text = "Set User Name";
		((ButtonBase)cmdSetUserName).UseVisualStyleBackColor = false;
		((Control)cmdSetUserName).Click += cmdSetUserName_Click;
		((Form)this).AutoScaleBaseSize = new Size(7, 19);
		((Form)this).ClientSize = new Size(458, 219);
		((Control)this).Controls.Add((Control)(object)cmdGetUserName);
		((Control)this).Controls.Add((Control)(object)cmdSetUserName);
		((Control)this).Controls.Add((Control)(object)txtVID);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)txtEnrollNumber);
		((Control)this).Controls.Add((Control)(object)txtName);
		((Control)this).Controls.Add((Control)(object)cmdGetUserNameVID);
		((Control)this).Controls.Add((Control)(object)cmdSetUserNameVID);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)lblEnrollNum);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Font = new Font("Times New Roman", 12f);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Location = new Point(2, 28);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmUserInfo";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "User Information";
		((Form)this).Load += frmUserInfo_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void frmUserInfo_Load(object sender, EventArgs e)
	{
		((Control)txtEnrollNumber).Text = "1";
	}

	private void cmdGetUserNameVID_Click(object sender, EventArgs e)
	{
		((Control)cmdGetUserNameVID).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetUserNameVID).Enabled = true;
			return;
		}
		int num2 = 708;
		byte[] array = new byte[num2];
		ExtCmd_USERVID extCmd_USERVID = default(ExtCmd_USERVID);
		int aUserID = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		extCmd_USERVID.Init(EXTCMD_GET_USERCUSTOMINFO, aUserID);
		FKAttendDLL.ConvertStructureToByteArray(extCmd_USERVID, array);
		num = FKAttendDLL.FK_ExtCommand(FKAttendDLL.nCommHandleIndex, array);
		extCmd_USERVID = (ExtCmd_USERVID)FKAttendDLL.ConvertByteArrayToStructure(array, typeof(ExtCmd_USERVID));
		if (num == 1)
		{
			((Control)txtName).Text = extCmd_USERVID.GetUserName();
			((Control)txtVID).Text = extCmd_USERVID.GetUserVID();
			((Control)lblMessage).Text = "GetUserName, VID OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetUserNameVID).Enabled = true;
	}

	private void cmdSetUserNameVID_Click(object sender, EventArgs e)
	{
		((Control)cmdSetUserNameVID).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetUserNameVID).Enabled = true;
			return;
		}
		int num2 = 708;
		byte[] array = new byte[num2];
		ExtCmd_USERVID extCmd_USERVID = default(ExtCmd_USERVID);
		int aUserID = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		extCmd_USERVID.Init(EXTCMD_SET_USERCUSTOMINFO, aUserID);
		extCmd_USERVID.SetUserVID(((Control)txtVID).Text.Trim());
		extCmd_USERVID.SetUserName(((Control)txtName).Text.Trim());
		FKAttendDLL.ConvertStructureToByteArray(extCmd_USERVID, array);
		num = FKAttendDLL.FK_ExtCommand(FKAttendDLL.nCommHandleIndex, array);
		if (num == 1)
		{
			((Control)lblMessage).Text = "SetUserName, VID OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetUserNameVID).Enabled = true;
	}

	private void cmdGetUserName_Click(object sender, EventArgs e)
	{
		((Control)cmdGetUserName).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetUserName).Enabled = true;
			return;
		}
		int anEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		string apstrUserName = new string(' ', 256);
		num = FKAttendDLL.FK_GetUserName(FKAttendDLL.nCommHandleIndex, anEnrollNumber, ref apstrUserName);
		if (num == 1)
		{
			((Control)txtName).Text = apstrUserName;
			((Control)lblMessage).Text = "GetUserName OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetUserName).Enabled = true;
	}

	private void cmdSetUserName_Click(object sender, EventArgs e)
	{
		((Control)cmdSetUserName).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetUserName).Enabled = true;
			return;
		}
		int anEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		num = FKAttendDLL.FK_SetUserName(FKAttendDLL.nCommHandleIndex, anEnrollNumber, ((Control)txtName).Text.Trim());
		if (num == 1)
		{
			((Control)lblMessage).Text = "SetUserName OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetUserName).Enabled = true;
	}
}
