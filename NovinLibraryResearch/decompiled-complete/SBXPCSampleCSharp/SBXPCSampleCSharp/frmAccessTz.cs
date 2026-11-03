using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmAccessTz : Form
{
	private AxSBXPC bpc;

	private const int DBTIMEZONE_COUNT = 50;

	private const int DBVMTIMEZONE_COUNT = 10;

	private const int DBTIMESECTION_COUNT = 8;

	private const int DBALLCOUNT = 400;

	private const int DBSYSTEMDEFAULT_VM = 5;

	private int[] timeZoneInfo;

	private IContainer components = null;

	public Button cmdExit;

	public Button cmdWrite;

	public Button cmdRead;

	public Button cmdUpdate;

	public Label lblEnrollData;

	public Label lblMessage;

	private DateTimePicker dtStart;

	private DateTimePicker dtEnd;

	public Label label1;

	private ListBox lstTimeZone;

	private ComboBox cmbVerifyMode;

	public Label label2;

	public frmAccessTz(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void frmAccessTz_Load(object sender, EventArgs e)
	{
		timeZoneInfo = new int[2000];
		dtStart.ShowUpDown = true;
		dtEnd.ShowUpDown = true;
		TimeZoneInit();
		DrawTimeZone();
	}

	private void TimeZoneInit()
	{
		for (int i = 0; i < 50; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				timeZoneInfo[(i * 8 + j) * 5] = 0;
				timeZoneInfo[(i * 8 + j) * 5 + 1] = 0;
				timeZoneInfo[(i * 8 + j) * 5 + 2] = 23;
				timeZoneInfo[(i * 8 + j) * 5 + 3] = 59;
				timeZoneInfo[(i * 8 + j) * 5 + 4] = 5;
			}
		}
	}

	private void DrawTimeZone()
	{
		string text = "";
		lstTimeZone.Items.Clear();
		for (int i = 0; i < 50; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				int num = timeZoneInfo[(i * 8 + j) * 5];
				int num2 = timeZoneInfo[(i * 8 + j) * 5 + 1];
				int num3 = timeZoneInfo[(i * 8 + j) * 5 + 2];
				int num4 = timeZoneInfo[(i * 8 + j) * 5 + 3];
				int num5 = ((i >= 10) ? 5 : timeZoneInfo[(i * 8 + j) * 5 + 4]);
				text = "[Tz.]" + $"{i:D2}-{j:D1} ";
				text = text + "[S]" + $"{num:D2}:{num2:D2} ";
				text = text + "[E]" + $"{num3:D2}:{num4:D2}" + " ";
				text = text + "[VM]" + cmbVerifyMode.Items[num5];
				lstTimeZone.Items.Add((object)text);
			}
		}
	}

	private void cmdRead_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwValue = GCHandle.Alloc(timeZoneInfo, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
		if (bpc.GetDeviceLongInfo(util.gMachineNumber, 3, ref dwValue))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
		DrawTimeZone();
	}

	private void frmAccessTz_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void cmdWrite_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwValue = GCHandle.Alloc(timeZoneInfo, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
		if (bpc.SetDeviceLongInfo(util.gMachineNumber, 3, ref dwValue))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
		DrawTimeZone();
	}

	private void lstTimeZone_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)lstTimeZone).SelectedIndex != -1)
		{
			int selectedIndex = ((ListControl)lstTimeZone).SelectedIndex;
			dtStart.Value = new DateTime(2000, 1, 1, timeZoneInfo[selectedIndex * 5], timeZoneInfo[selectedIndex * 5 + 1], 0);
			dtEnd.Value = new DateTime(2000, 1, 1, timeZoneInfo[selectedIndex * 5 + 2], timeZoneInfo[selectedIndex * 5 + 3], 0);
			if (selectedIndex < 80)
			{
				((ListControl)cmbVerifyMode).SelectedIndex = timeZoneInfo[selectedIndex * 5 + 4];
			}
			else
			{
				((ListControl)cmbVerifyMode).SelectedIndex = 5;
			}
		}
	}

	private void cmdUpdate_Click(object sender, EventArgs e)
	{
		if (((ListControl)lstTimeZone).SelectedIndex != -1)
		{
			int selectedIndex = ((ListControl)lstTimeZone).SelectedIndex;
			timeZoneInfo[selectedIndex * 5] = dtStart.Value.Hour;
			timeZoneInfo[selectedIndex * 5 + 1] = dtStart.Value.Minute;
			timeZoneInfo[selectedIndex * 5 + 2] = dtEnd.Value.Hour;
			timeZoneInfo[selectedIndex * 5 + 3] = dtEnd.Value.Minute;
			if (selectedIndex < 80)
			{
				timeZoneInfo[selectedIndex * 5 + 4] = ((ListControl)cmbVerifyMode).SelectedIndex;
			}
			DrawTimeZone();
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
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Expected O, but got Unknown
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Expected O, but got Unknown
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Expected O, but got Unknown
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d1: Expected O, but got Unknown
		//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Expected O, but got Unknown
		cmdExit = new Button();
		cmdWrite = new Button();
		cmdRead = new Button();
		cmdUpdate = new Button();
		lblEnrollData = new Label();
		lblMessage = new Label();
		dtStart = new DateTimePicker();
		dtEnd = new DateTimePicker();
		label1 = new Label();
		lstTimeZone = new ListBox();
		cmbVerifyMode = new ComboBox();
		label2 = new Label();
		((Control)this).SuspendLayout();
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(540, 468);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(104, 46);
		((Control)cmdExit).TabIndex = 76;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdWrite).BackColor = SystemColors.Control;
		((Control)cmdWrite).Cursor = Cursors.Default;
		((Control)cmdWrite).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdWrite).ForeColor = SystemColors.ControlText;
		((Control)cmdWrite).Location = new Point(540, 404);
		((Control)cmdWrite).Name = "cmdWrite";
		((Control)cmdWrite).RightToLeft = (RightToLeft)0;
		((Control)cmdWrite).Size = new Size(104, 46);
		((Control)cmdWrite).TabIndex = 75;
		((Control)cmdWrite).Text = "Write";
		((ButtonBase)cmdWrite).UseVisualStyleBackColor = false;
		((Control)cmdWrite).Click += cmdWrite_Click;
		((Control)cmdRead).BackColor = SystemColors.Control;
		((Control)cmdRead).Cursor = Cursors.Default;
		((Control)cmdRead).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdRead).ForeColor = SystemColors.ControlText;
		((Control)cmdRead).Location = new Point(540, 339);
		((Control)cmdRead).Name = "cmdRead";
		((Control)cmdRead).RightToLeft = (RightToLeft)0;
		((Control)cmdRead).Size = new Size(104, 46);
		((Control)cmdRead).TabIndex = 74;
		((Control)cmdRead).Text = "Read";
		((ButtonBase)cmdRead).UseVisualStyleBackColor = false;
		((Control)cmdRead).Click += cmdRead_Click;
		((Control)cmdUpdate).BackColor = SystemColors.Control;
		((Control)cmdUpdate).Cursor = Cursors.Default;
		((Control)cmdUpdate).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdUpdate).ForeColor = SystemColors.ControlText;
		((Control)cmdUpdate).Location = new Point(540, 130);
		((Control)cmdUpdate).Name = "cmdUpdate";
		((Control)cmdUpdate).RightToLeft = (RightToLeft)0;
		((Control)cmdUpdate).Size = new Size(104, 46);
		((Control)cmdUpdate).TabIndex = 72;
		((Control)cmdUpdate).Text = "Update";
		((ButtonBase)cmdUpdate).UseVisualStyleBackColor = false;
		((Control)cmdUpdate).Click += cmdUpdate_Click;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(12, 61);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(41, 19);
		((Control)lblEnrollData).TabIndex = 70;
		((Control)lblEnrollData).Text = "Start:";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 12);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(632, 28);
		((Control)lblMessage).TabIndex = 69;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		dtStart.CalendarFont = new Font("Times New Roman", 12f, (FontStyle)1);
		dtStart.CustomFormat = "hh:mm:ss";
		dtStart.DropDownAlign = (LeftRightAlignment)1;
		((Control)dtStart).Font = new Font("Times New Roman", 12f);
		dtStart.Format = (DateTimePickerFormat)4;
		((Control)dtStart).Location = new Point(67, 61);
		((Control)dtStart).Name = "dtStart";
		((Control)dtStart).Size = new Size(119, 26);
		((Control)dtStart).TabIndex = 77;
		dtStart.Value = new DateTime(2011, 10, 12, 10, 44, 0, 0);
		dtEnd.CalendarFont = new Font("Times New Roman", 12f, (FontStyle)1);
		((Control)dtEnd).Font = new Font("Times New Roman", 12f);
		dtEnd.Format = (DateTimePickerFormat)4;
		((Control)dtEnd).Location = new Point(67, 90);
		((Control)dtEnd).Name = "dtEnd";
		((Control)dtEnd).Size = new Size(119, 26);
		((Control)dtEnd).TabIndex = 79;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = SystemColors.Control;
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = SystemColors.ControlText;
		((Control)label1).Location = new Point(12, 90);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)0;
		((Control)label1).Size = new Size(36, 19);
		((Control)label1).TabIndex = 78;
		((Control)label1).Text = "End:";
		((Control)lstTimeZone).Font = new Font("Times New Roman", 12f);
		((ListControl)lstTimeZone).FormattingEnabled = true;
		lstTimeZone.ItemHeight = 19;
		((Control)lstTimeZone).Location = new Point(16, 129);
		((Control)lstTimeZone).Name = "lstTimeZone";
		((Control)lstTimeZone).Size = new Size(501, 384);
		((Control)lstTimeZone).TabIndex = 80;
		lstTimeZone.SelectedIndexChanged += lstTimeZone_SelectedIndexChanged;
		((Control)cmbVerifyMode).Font = new Font("Times New Roman", 12f);
		((ListControl)cmbVerifyMode).FormattingEnabled = true;
		cmbVerifyMode.Items.AddRange(new object[16]
		{
			"FACE|FP|CD|PWD", "CD&FP", "FP&PWD", "CD&PWD", "FP&CD&PWD", "System Default VM", "FP", "CD", "8-INVALID-DON'T SELECT ME", "ID&PWD",
			"10-INVALID-DON'T SELECT ME", "FACE", "CD&FACE", "FACE&PWD", "FACE&CD&PWD", "FACE&FP"
		});
		((Control)cmbVerifyMode).Location = new Point(206, 89);
		((Control)cmbVerifyMode).Name = "cmbVerifyMode";
		((Control)cmbVerifyMode).Size = new Size(249, 27);
		((Control)cmbVerifyMode).TabIndex = 81;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = SystemColors.Control;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(202, 65);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(88, 19);
		((Control)label2).TabIndex = 82;
		((Control)label2).Text = "Verify Mode:";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(662, 535);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)cmbVerifyMode);
		((Control)this).Controls.Add((Control)(object)lstTimeZone);
		((Control)this).Controls.Add((Control)(object)dtEnd);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)dtStart);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdWrite);
		((Control)this).Controls.Add((Control)(object)cmdRead);
		((Control)this).Controls.Add((Control)(object)cmdUpdate);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmAccessTz";
		((Control)this).Text = "frmAccessTz";
		((Form)this).FormClosing += new FormClosingEventHandler(frmAccessTz_FormClosing);
		((Form)this).Load += frmAccessTz_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
