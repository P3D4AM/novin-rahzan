using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using AxAXIMAGELib;

namespace FKAttendDllCSSample;

public class frmLog : Form
{
	public CheckBox chkSave;

	public Button cmdUsbGlog;

	public Button cmdUsbSLog;

	internal ListView gridLogView;

	internal OpenFileDialog dlgOpen;

	public CheckBox chkReadMark;

	public Button cmdEmptyGLogData;

	public Button cmdEmptySLogData;

	public Button cmdGlogData;

	public Button cmdSLogData;

	public Label lblEnrollData;

	public Label lblTotal;

	public Label lblMessage;

	private AxAxImage axAxImage1;

	private Container components = null;

	private const long DEF_MAX_LOGCOUNT = 200000L;

	private const long DEF_MAX_DISPCOUNT = 30000L;

	private const short DEF_MUL_TWIPS = 15;

	private int mnGridHeight;

	private int mngrdIndex;

	private ListView[] mgrdLogView;

	public frmLog()
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Expected O, but got Unknown
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Expected O, but got Unknown
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Expected O, but got Unknown
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af8: Expected O, but got Unknown
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd2: Expected O, but got Unknown
		ListViewItem val = new ListViewItem(new string[1] { "" }, -1, SystemColors.WindowText, SystemColors.Window, new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134));
		ListViewItem val2 = new ListViewItem(new string[1] { "" }, -1, SystemColors.WindowText, SystemColors.Window, new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134));
		ListViewItem val3 = new ListViewItem(new string[1] { "" }, -1, SystemColors.WindowText, SystemColors.Window, new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134));
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmLog));
		chkSave = new CheckBox();
		cmdUsbGlog = new Button();
		cmdUsbSLog = new Button();
		gridLogView = new ListView();
		dlgOpen = new OpenFileDialog();
		chkReadMark = new CheckBox();
		cmdEmptyGLogData = new Button();
		cmdEmptySLogData = new Button();
		cmdGlogData = new Button();
		cmdSLogData = new Button();
		lblEnrollData = new Label();
		lblTotal = new Label();
		lblMessage = new Label();
		axAxImage1 = new AxAxImage();
		((ISupportInitialize)axAxImage1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)chkSave).BackColor = SystemColors.Control;
		((Control)chkSave).Cursor = Cursors.Default;
		((Control)chkSave).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkSave).ForeColor = SystemColors.ControlText;
		((Control)chkSave).Location = new Point(438, 69);
		((Control)chkSave).Name = "chkSave";
		((Control)chkSave).RightToLeft = (RightToLeft)0;
		((Control)chkSave).Size = new Size(104, 20);
		((Control)chkSave).TabIndex = 33;
		((Control)chkSave).Text = "Save to file";
		((ButtonBase)chkSave).UseVisualStyleBackColor = false;
		((Control)cmdUsbGlog).BackColor = SystemColors.Control;
		((Control)cmdUsbGlog).Cursor = Cursors.Default;
		((Control)cmdUsbGlog).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdUsbGlog).ForeColor = SystemColors.ControlText;
		((Control)cmdUsbGlog).Location = new Point(549, 430);
		((Control)cmdUsbGlog).Name = "cmdUsbGlog";
		((Control)cmdUsbGlog).RightToLeft = (RightToLeft)0;
		((Control)cmdUsbGlog).Size = new Size(106, 56);
		((Control)cmdUsbGlog).TabIndex = 27;
		((Control)cmdUsbGlog).Text = "Read USB GLogData";
		((ButtonBase)cmdUsbGlog).UseVisualStyleBackColor = false;
		((Control)cmdUsbGlog).Click += cmdUsbGlog_Click;
		((Control)cmdUsbSLog).BackColor = SystemColors.Control;
		((Control)cmdUsbSLog).Cursor = Cursors.Default;
		((Control)cmdUsbSLog).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdUsbSLog).ForeColor = SystemColors.ControlText;
		((Control)cmdUsbSLog).Location = new Point(229, 430);
		((Control)cmdUsbSLog).Name = "cmdUsbSLog";
		((Control)cmdUsbSLog).RightToLeft = (RightToLeft)0;
		((Control)cmdUsbSLog).Size = new Size(105, 56);
		((Control)cmdUsbSLog).TabIndex = 24;
		((Control)cmdUsbSLog).Text = "Read USB SLogData";
		((ButtonBase)cmdUsbSLog).UseVisualStyleBackColor = false;
		((Control)cmdUsbSLog).Click += cmdUsbSLog_Click;
		((Control)gridLogView).Font = new Font("Times New Roman", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		gridLogView.FullRowSelect = true;
		gridLogView.GridLines = true;
		gridLogView.Items.AddRange((ListViewItem[])(object)new ListViewItem[3] { val, val2, val3 });
		((Control)gridLogView).Location = new Point(14, 94);
		gridLogView.MultiSelect = false;
		((Control)gridLogView).Name = "gridLogView";
		((Control)gridLogView).Size = new Size(644, 328);
		((Control)gridLogView).TabIndex = 32;
		gridLogView.UseCompatibleStateImageBehavior = false;
		gridLogView.View = (View)1;
		((Control)gridLogView).DoubleClick += gridLogView_DoubleClick;
		((Control)chkReadMark).BackColor = SystemColors.Control;
		((Control)chkReadMark).Cursor = Cursors.Default;
		((Control)chkReadMark).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkReadMark).ForeColor = SystemColors.ControlText;
		((Control)chkReadMark).Location = new Point(557, 69);
		((Control)chkReadMark).Name = "chkReadMark";
		((Control)chkReadMark).RightToLeft = (RightToLeft)0;
		((Control)chkReadMark).Size = new Size(96, 20);
		((Control)chkReadMark).TabIndex = 28;
		((Control)chkReadMark).Text = "ReadMark";
		((ButtonBase)chkReadMark).UseVisualStyleBackColor = false;
		((Control)cmdEmptyGLogData).BackColor = SystemColors.Control;
		((Control)cmdEmptyGLogData).Cursor = Cursors.Default;
		((Control)cmdEmptyGLogData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdEmptyGLogData).ForeColor = SystemColors.ControlText;
		((Control)cmdEmptyGLogData).Location = new Point(442, 430);
		((Control)cmdEmptyGLogData).Name = "cmdEmptyGLogData";
		((Control)cmdEmptyGLogData).RightToLeft = (RightToLeft)0;
		((Control)cmdEmptyGLogData).Size = new Size(106, 56);
		((Control)cmdEmptyGLogData).TabIndex = 26;
		((Control)cmdEmptyGLogData).Text = "Empty GLogData";
		((ButtonBase)cmdEmptyGLogData).UseVisualStyleBackColor = false;
		((Control)cmdEmptyGLogData).Click += cmdEmptyGLogData_Click;
		((Control)cmdEmptySLogData).BackColor = SystemColors.Control;
		((Control)cmdEmptySLogData).Cursor = Cursors.Default;
		((Control)cmdEmptySLogData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdEmptySLogData).ForeColor = SystemColors.ControlText;
		((Control)cmdEmptySLogData).Location = new Point(121, 430);
		((Control)cmdEmptySLogData).Name = "cmdEmptySLogData";
		((Control)cmdEmptySLogData).RightToLeft = (RightToLeft)0;
		((Control)cmdEmptySLogData).Size = new Size(107, 56);
		((Control)cmdEmptySLogData).TabIndex = 23;
		((Control)cmdEmptySLogData).Text = "Empty SLogData";
		((ButtonBase)cmdEmptySLogData).UseVisualStyleBackColor = false;
		((Control)cmdEmptySLogData).Click += cmdEmptySLogData_Click;
		((Control)cmdGlogData).BackColor = SystemColors.Control;
		((Control)cmdGlogData).Cursor = Cursors.Default;
		((Control)cmdGlogData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGlogData).ForeColor = SystemColors.ControlText;
		((Control)cmdGlogData).Location = new Point(335, 430);
		((Control)cmdGlogData).Name = "cmdGlogData";
		((Control)cmdGlogData).RightToLeft = (RightToLeft)0;
		((Control)cmdGlogData).Size = new Size(106, 56);
		((Control)cmdGlogData).TabIndex = 25;
		((Control)cmdGlogData).Text = "Read GLogData";
		((ButtonBase)cmdGlogData).UseVisualStyleBackColor = false;
		((Control)cmdGlogData).Click += cmdGlogData_Click;
		((Control)cmdSLogData).BackColor = SystemColors.Control;
		((Control)cmdSLogData).Cursor = Cursors.Default;
		((Control)cmdSLogData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSLogData).ForeColor = SystemColors.ControlText;
		((Control)cmdSLogData).Location = new Point(14, 430);
		((Control)cmdSLogData).Name = "cmdSLogData";
		((Control)cmdSLogData).RightToLeft = (RightToLeft)0;
		((Control)cmdSLogData).Size = new Size(106, 56);
		((Control)cmdSLogData).TabIndex = 22;
		((Control)cmdSLogData).Text = "Read SLogData";
		((ButtonBase)cmdSLogData).UseVisualStyleBackColor = false;
		((Control)cmdSLogData).Click += cmdSLogData_Click;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(14, 70);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(73, 19);
		((Control)lblEnrollData).TabIndex = 31;
		((Control)lblEnrollData).Text = "Log Data :";
		((Control)lblTotal).AutoSize = true;
		((Control)lblTotal).BackColor = SystemColors.Control;
		((Control)lblTotal).Cursor = Cursors.Default;
		((Control)lblTotal).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblTotal).ForeColor = SystemColors.ControlText;
		((Control)lblTotal).Location = new Point(124, 70);
		((Control)lblTotal).Name = "lblTotal";
		((Control)lblTotal).RightToLeft = (RightToLeft)0;
		((Control)lblTotal).Size = new Size(46, 19);
		((Control)lblTotal).TabIndex = 30;
		((Control)lblTotal).Text = "Total :";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(14, 21);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(880, 28);
		((Control)lblMessage).TabIndex = 29;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((AxHost)axAxImage1).Enabled = true;
		((Control)axAxImage1).Location = new Point(665, 94);
		((Control)axAxImage1).Name = "axAxImage1";
		((AxHost)axAxImage1).OcxState = (State)componentResourceManager.GetObject("axAxImage1.OcxState");
		((Control)axAxImage1).Size = new Size(229, 328);
		((Control)axAxImage1).TabIndex = 34;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(906, 514);
		((Control)this).Controls.Add((Control)(object)axAxImage1);
		((Control)this).Controls.Add((Control)(object)chkSave);
		((Control)this).Controls.Add((Control)(object)cmdUsbGlog);
		((Control)this).Controls.Add((Control)(object)cmdUsbSLog);
		((Control)this).Controls.Add((Control)(object)gridLogView);
		((Control)this).Controls.Add((Control)(object)chkReadMark);
		((Control)this).Controls.Add((Control)(object)cmdEmptyGLogData);
		((Control)this).Controls.Add((Control)(object)cmdEmptySLogData);
		((Control)this).Controls.Add((Control)(object)cmdGlogData);
		((Control)this).Controls.Add((Control)(object)cmdSLogData);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblTotal);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmLog";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Empty GLogData";
		((Form)this).Closed += frmLog_Closed;
		((Form)this).Load += frmLog_Load;
		((ISupportInitialize)axAxImage1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void frmLog_Load(object sender, EventArgs e)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		chkReadMark.CheckState = (CheckState)1;
		chkSave.CheckState = (CheckState)0;
		mnGridHeight = ((Control)gridLogView).Height;
		long num = 6L;
		if (num * 30000 < 200000)
		{
			num++;
		}
		if (num < 0)
		{
			num = 0L;
		}
		frmLog_Closed(sender, e);
		mgrdLogView = (ListView[])(object)new ListView[num];
		mgrdLogView[0] = gridLogView;
		if (num > 0)
		{
			for (int i = 1; i < num; i++)
			{
				mgrdLogView[i] = new ListView();
				((Control)mgrdLogView[i]).Name = "gridListView" + i;
				((Control)mgrdLogView[i]).Left = ((Control)gridLogView).Left;
				((Control)mgrdLogView[i]).Top = ((Control)gridLogView).Top;
				((Control)mgrdLogView[i]).Width = ((Control)gridLogView).Width;
				((Control)mgrdLogView[i]).Height = 0;
				mgrdLogView[i].HeaderStyle = (ColumnHeaderStyle)0;
				mgrdLogView[i].View = gridLogView.View;
				mgrdLogView[i].GridLines = gridLogView.GridLines;
				((Control)mgrdLogView[i]).Visible = false;
				((Control)this).Controls.Add((Control)(object)mgrdLogView[i]);
			}
		}
		OwnerEnableButtons(abEnableFlag: true);
	}

	private void frmLog_Closed(object sender, EventArgs e)
	{
		if (mgrdLogView == null || mgrdLogView.GetUpperBound(0) <= 0)
		{
			return;
		}
		ListView[] array = mgrdLogView;
		foreach (ListView val in array)
		{
			if (val != null && ((Control)val).Name != ((Control)gridLogView).Name)
			{
				((Control)this).Controls.Remove((Control)(object)val);
				((Component)(object)val).Dispose();
			}
		}
	}

	private void cmdSLogData_Click(object sender, EventArgs e)
	{
		OwnerEnableButtons(abEnableFlag: false);
		funcGetSuperLogData(abUSBFlag: false);
		OwnerEnableButtons(abEnableFlag: true);
	}

	private void cmdEmptySLogData_Click(object sender, EventArgs e)
	{
		((Control)cmdEmptySLogData).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdEmptySLogData).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_EmptySuperLogData(FKAttendDLL.nCommHandleIndex);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdEmptySLogData).Enabled = true;
	}

	private void cmdUsbSLog_Click(object sender, EventArgs e)
	{
		OwnerEnableButtons(abEnableFlag: false);
		funcGetSuperLogData(abUSBFlag: true);
		OwnerEnableButtons(abEnableFlag: true);
	}

	private void cmdGlogData_Click(object sender, EventArgs e)
	{
		OwnerEnableButtons(abEnableFlag: false);
		funcGetGeneralLogData(abUSBFlag: false);
		OwnerEnableButtons(abEnableFlag: true);
	}

	private void cmdEmptyGLogData_Click(object sender, EventArgs e)
	{
		((Control)cmdEmptyGLogData).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdEmptyGLogData).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_EmptyGeneralLogData(FKAttendDLL.nCommHandleIndex);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdEmptyGLogData).Enabled = true;
	}

	private void cmdUsbGlog_Click(object sender, EventArgs e)
	{
		OwnerEnableButtons(abEnableFlag: false);
		funcGetGeneralLogData(abUSBFlag: true);
		OwnerEnableButtons(abEnableFlag: true);
	}

	private void funcGetSuperLogData(bool abUSBFlag)
	{
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Invalid comparison between Unknown and I4
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Invalid comparison between Unknown and I4
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		int apnSEnrollNumber = 0;
		int apnGEnrollNumber = 0;
		int apnMachinePrivilege = 0;
		int apnBackupNumber = 0;
		DateTime apnDateTime = DateTime.Now;
		string text = "";
		string text2 = "";
		((Control)lblMessage).Text = "Waiting...";
		((Control)lblTotal).Text = "Total : 0";
		Application.DoEvents();
		string[] array = new string[6] { "", "SEnrollNo", "GEnrollNo", "Manipulation", "BackupNo", "DateTime" };
		((Control)mgrdLogView[0]).Height = mnGridHeight;
		mgrdLogView[0].Items.Clear();
		mgrdLogView[0].Columns.Clear();
		string[] array2 = array;
		foreach (string text3 in array2)
		{
			mgrdLogView[0].Columns.Add(text3, 80, (HorizontalAlignment)2);
		}
		mgrdLogView[0].Columns[0].Width = 48;
		mgrdLogView[0].Columns[0].TextAlign = (HorizontalAlignment)1;
		mgrdLogView[0].Columns[3].Width = 140;
		mgrdLogView[0].Columns[3].TextAlign = (HorizontalAlignment)0;
		mgrdLogView[0].Columns[5].Width = 140;
		Application.DoEvents();
		for (int j = 1; j < mgrdLogView.GetUpperBound(0); j++)
		{
			if (mgrdLogView[j] == null)
			{
				continue;
			}
			((Control)mgrdLogView[j]).Visible = false;
			((Control)mgrdLogView[j]).Height = 0;
			mgrdLogView[j].Items.Clear();
			mgrdLogView[j].Columns.Clear();
			foreach (ColumnHeader column in mgrdLogView[0].Columns)
			{
				ColumnHeader val = column;
				mgrdLogView[j].Columns.Add("", val.Width, val.TextAlign);
			}
		}
		int num;
		if (abUSBFlag)
		{
			((FileDialog)dlgOpen).InitialDirectory = Directory.GetCurrentDirectory();
			((FileDialog)dlgOpen).Filter = "SLog Files (*.txt)|*.txt|All Files (*.*)|*.*";
			((FileDialog)dlgOpen).FilterIndex = 1;
			if ((int)((CommonDialog)dlgOpen).ShowDialog() != 1)
			{
				return;
			}
			text = ((FileDialog)dlgOpen).FileName;
			((FileDialog)dlgOpen).FileName = "";
			if (text == "")
			{
				return;
			}
		}
		else
		{
			num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
			if (num != 1)
			{
				((Control)lblMessage).Text = "No Device";
				return;
			}
		}
		((Control)this).Cursor = Cursors.WaitCursor;
		Application.DoEvents();
		num = ((!abUSBFlag) ? FKAttendDLL.FK_LoadSuperLogData(anReadMark: ((int)chkReadMark.CheckState == 1) ? 1 : 0, anHandleIndex: FKAttendDLL.nCommHandleIndex) : FKAttendDLL.FK_USBLoadSuperLogDataFromFile(FKAttendDLL.nCommHandleIndex, text));
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		else
		{
			((Control)lblMessage).Text = "Getting...";
			Application.DoEvents();
			int j = 1;
			while (true)
			{
				num = FKAttendDLL.FK_GetSuperLogData(FKAttendDLL.nCommHandleIndex, ref apnSEnrollNumber, ref apnGEnrollNumber, ref apnMachinePrivilege, ref apnBackupNumber, ref apnDateTime);
				ListViewItem val2;
				switch (num)
				{
				case -7:
					num = 1;
					break;
				case 1:
					val2 = new ListViewItem();
					val2.Text = Convert.ToString(j).Trim();
					val2.SubItems.Add(apnSEnrollNumber.ToString().Trim());
					val2.SubItems.Add(apnGEnrollNumber.ToString().Trim());
					text2 = apnMachinePrivilege switch
					{
						3 => "Enroll User", 
						4 => "Enroll Manager", 
						5 => "Delete Fp Data", 
						6 => "Delete Password", 
						7 => "Delete Card Data", 
						8 => "Delete All LogData", 
						9 => "Modify System Info", 
						10 => "Modify System Time", 
						11 => "Modify Log Setting", 
						12 => "Modify Comm Setting", 
						13 => "Pass Time Set", 
						14 => "Door Set Log", 
						_ => "--", 
					};
					val2.SubItems.Add(text2);
					text2 = ((apnBackupNumber != 10) ? ((apnBackupNumber != 11) ? ((apnBackupNumber >= 10) ? "--" : ("Fp-" + apnBackupNumber.ToString().Trim())) : "Card") : "Password");
					goto IL_0573;
				}
				break;
				IL_0573:
				val2.SubItems.Add(text2);
				text2 = apnDateTime.Year + apnDateTime.Month.ToString("/0#") + apnDateTime.Day.ToString("/0#") + apnDateTime.Hour.ToString(" 0#") + apnDateTime.Minute.ToString(":0#") + apnDateTime.Second.ToString(":0#");
				val2.SubItems.Add(text2);
				mgrdLogView[0].Items.Add(val2);
				val2 = null;
				((Control)lblTotal).Text = "Total : " + j;
				Application.DoEvents();
				j++;
				bool flag = true;
			}
			if (num == 1)
			{
				if (abUSBFlag)
				{
					((Control)lblMessage).Text = "USBReadSuperLogDataFromFile OK";
				}
				else
				{
					((Control)lblMessage).Text = "ReadAllSuperLogData OK";
				}
			}
			else
			{
				((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			}
		}
		((Control)this).Cursor = Cursors.Default;
		if (!abUSBFlag)
		{
			FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		}
	}

	private void funcGeneralLogDataGridFormat()
	{
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[5] { "", "EnrollNo", "VerifyMode", "InOutMode", "DateTime" };
		((Control)mgrdLogView[0]).Height = mnGridHeight;
		mgrdLogView[0].Items.Clear();
		mgrdLogView[0].Columns.Clear();
		string[] array2 = array;
		foreach (string text in array2)
		{
			mgrdLogView[0].Columns.Add(text, 80, (HorizontalAlignment)2);
		}
		mgrdLogView[0].Columns[0].Width = 48;
		mgrdLogView[0].Columns[0].TextAlign = (HorizontalAlignment)1;
		mgrdLogView[0].Columns[2].Width = 140;
		mgrdLogView[0].Columns[4].Width = 140;
		Application.DoEvents();
		for (int j = 1; j < mgrdLogView.GetUpperBound(0); j++)
		{
			if (mgrdLogView[j] == null)
			{
				continue;
			}
			((Control)mgrdLogView[j]).Visible = false;
			((Control)mgrdLogView[j]).Left = ((Control)mgrdLogView[0]).Left;
			((Control)mgrdLogView[j]).Top = ((Control)mgrdLogView[0]).Top;
			((Control)mgrdLogView[j]).Width = ((Control)mgrdLogView[0]).Width;
			((Control)mgrdLogView[j]).Height = 0;
			mgrdLogView[j].Items.Clear();
			mgrdLogView[j].Columns.Clear();
			foreach (ColumnHeader column in mgrdLogView[0].Columns)
			{
				ColumnHeader val = column;
				mgrdLogView[j].Columns.Add("", val.Width, val.TextAlign);
			}
		}
		Application.DoEvents();
	}

	private bool funcShowGeneralLogDataToGrid(int anCount, int aSEnrollNumber, int aVerifyMode, int aInOutMode, DateTime adwDate)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		bool result = true;
		if (anCount <= 1)
		{
			mngrdIndex = 0;
		}
		else if ((long)(mngrdIndex + 1) * 30000L < anCount)
		{
			mngrdIndex++;
			if (mngrdIndex > mgrdLogView.GetUpperBound(0))
			{
				return false;
			}
			int num = mnGridHeight;
			int top = ((Control)mgrdLogView[0]).Top;
			for (int i = 0; i < mngrdIndex; i++)
			{
				((Control)mgrdLogView[i]).Top = top + (i - 1) * (num / mngrdIndex);
				((Control)mgrdLogView[i]).Height = num / mngrdIndex;
			}
			((Control)mgrdLogView[mngrdIndex]).Visible = true;
		}
		ListViewItem val = new ListViewItem();
		val.Text = anCount.ToString().Trim();
		val.SubItems.Add(aSEnrollNumber.ToString().Trim());
		val.SubItems.Add(aVerifyMode switch
		{
			1 => "Fp", 
			2 => "Password", 
			3 => "Card", 
			4 => "Fp+Password", 
			5 => "Fp+Card", 
			6 => "Password+Fp", 
			7 => "Card+Fp", 
			8 => "Job No", 
			9 => "Card+Pass", 
			11 => "Close Door", 
			12 => "Hand Open", 
			14 => "Prog Open", 
			15 => "PC Close", 
			16 => "Illegal Open", 
			17 => "Illegal Close", 
			18 => "Cover Open", 
			19 => "Cover Close", 
			20 => "Face", 
			21 => "Face+Card", 
			22 => "Face+Pass", 
			_ => "--", 
		});
		val.SubItems.Add(aInOutMode switch
		{
			0 => "General", 
			1 => "IN1", 
			2 => "IN2", 
			3 => "IN3", 
			4 => "OUT1", 
			5 => "OUT2", 
			6 => "OUT3", 
			_ => "--", 
		});
		string text = adwDate.Year + adwDate.Month.ToString("/0#") + adwDate.Day.ToString("/0#") + adwDate.Hour.ToString(" 0#") + adwDate.Minute.ToString(":0#") + adwDate.Second.ToString(":0#");
		val.SubItems.Add(text);
		mgrdLogView[mngrdIndex].Items.Add(val);
		val = null;
		((Control)lblTotal).Text = "Total : " + anCount;
		Application.DoEvents();
		return result;
	}

	private void funcGetGeneralLogData(bool abUSBFlag)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Invalid comparison between Unknown and I4
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Invalid comparison between Unknown and I4
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Invalid comparison between Unknown and I4
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Invalid comparison between Unknown and I4
		int apnEnrollNumber = 0;
		int apnVerifyMode = 0;
		int apnInOutMode = 0;
		DateTime apnDateTime = DateTime.MinValue;
		int num = 0;
		string text = "";
		((Control)lblMessage).Text = "Waiting...";
		((Control)lblTotal).Text = "Total : 0";
		Application.DoEvents();
		funcGeneralLogDataGridFormat();
		if (abUSBFlag)
		{
			((FileDialog)dlgOpen).InitialDirectory = Directory.GetCurrentDirectory();
			((FileDialog)dlgOpen).Filter = "GLog Files (*.txt)|*.txt|All Files (*.*)|*.*";
			((FileDialog)dlgOpen).FilterIndex = 1;
			if ((int)((CommonDialog)dlgOpen).ShowDialog() != 1)
			{
				return;
			}
			text = ((FileDialog)dlgOpen).FileName;
			((FileDialog)dlgOpen).FileName = "";
		}
		else if (FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0) == 0)
		{
			((Control)lblMessage).Text = "No Device";
			return;
		}
		((Control)this).Cursor = Cursors.WaitCursor;
		Application.DoEvents();
		if (abUSBFlag)
		{
			num = FKAttendDLL.FK_USBLoadGeneralLogDataFromFile(FKAttendDLL.nCommHandleIndex, text);
		}
		else
		{
			int num2 = (((int)chkReadMark.CheckState == 1) ? 1 : 0);
			if ((int)chkSave.CheckState == 1)
			{
				text = ((num2 != 0) ? (Directory.GetCurrentDirectory() + "\\Log.txt") : (Directory.GetCurrentDirectory() + "\\AllLog.txt"));
				if (Directory.Exists(text))
				{
					File.Delete(text);
				}
				string text2 = "No." + Convert.ToChar(9) + "EnrNo" + Convert.ToChar(9) + "Verify" + Convert.ToChar(9) + "InOut" + Convert.ToChar(9) + "DateTime" + Convert.ToChar(13) + Convert.ToChar(10);
				using (File.Create(text))
				{
				}
				using FileStream fileStream2 = File.OpenWrite(text);
				byte[] bytes = new ASCIIEncoding().GetBytes(text2.ToCharArray());
				fileStream2.Write(bytes, 0, text2.Length);
			}
			num = FKAttendDLL.FK_LoadGeneralLogData(FKAttendDLL.nCommHandleIndex, num2);
		}
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		else
		{
			((Control)lblMessage).Text = "Getting...";
			Application.DoEvents();
			int num3 = 1;
			while (true)
			{
				num = FKAttendDLL.FK_GetGeneralLogData(FKAttendDLL.nCommHandleIndex, ref apnEnrollNumber, ref apnVerifyMode, ref apnInOutMode, ref apnDateTime);
				switch (num)
				{
				case -7:
					num = 1;
					break;
				case 1:
					if (!abUSBFlag && (int)chkSave.CheckState == 1)
					{
						string text2 = funcMakeGeneralLogFileData(num3, apnEnrollNumber, apnVerifyMode, apnInOutMode, apnDateTime);
						using FileStream fileStream3 = File.OpenWrite(text);
						byte[] bytes2 = new ASCIIEncoding().GetBytes(text2.ToCharArray());
						fileStream3.Seek(0L, SeekOrigin.End);
						fileStream3.Write(bytes2, 0, text2.Length);
					}
					if (funcShowGeneralLogDataToGrid(num3, apnEnrollNumber, apnVerifyMode, apnInOutMode, apnDateTime))
					{
						goto IL_03af;
					}
					break;
				}
				break;
				IL_03af:
				num3++;
				bool flag = true;
			}
			if (num == 1)
			{
				if (abUSBFlag)
				{
					((Control)lblMessage).Text = "USBReadGeneralLogDataFromFile OK";
				}
				else
				{
					((Control)lblMessage).Text = "ReadGeneralLogData OK";
				}
			}
			else
			{
				((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			}
		}
		((Control)this).Cursor = Cursors.Default;
		if (!abUSBFlag)
		{
			FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		}
	}

	private void OwnerEnableButtons(bool abEnableFlag)
	{
		bool openFlag = FrmConnectDeviceHysoon.GfrmMain.OpenFlag;
		((Control)cmdSLogData).Enabled = openFlag && abEnableFlag;
		((Control)cmdEmptySLogData).Enabled = openFlag && abEnableFlag;
		((Control)cmdUsbSLog).Enabled = abEnableFlag;
		((Control)cmdGlogData).Enabled = openFlag && abEnableFlag;
		((Control)cmdEmptyGLogData).Enabled = openFlag && abEnableFlag;
		((Control)cmdUsbGlog).Enabled = abEnableFlag;
		Application.DoEvents();
	}

	private string funcMakeGeneralLogFileData(long anCount, long aSEnrollNumber, long aVerifyMode, long aInOutMode, DateTime adwDate)
	{
		string text = anCount.ToString() + Convert.ToChar(9) + aSEnrollNumber + Convert.ToChar(9) + aVerifyMode + Convert.ToChar(9) + aInOutMode + Convert.ToChar(9);
		string text2 = adwDate.Year + adwDate.Month.ToString("/0#") + adwDate.Day.ToString("/0#") + adwDate.Hour.ToString(" 0#") + adwDate.Minute.ToString(":0#") + adwDate.Second.ToString(":0#");
		return text + text2 + Convert.ToChar(13) + Convert.ToChar(10);
	}

	private void gridLogView_DoubleClick(object sender, EventArgs e)
	{
		if (mngrdIndex < 0)
		{
			return;
		}
		ListViewItem val = mgrdLogView[mngrdIndex].SelectedItems[0];
		int anEnrollNumber = Convert.ToInt32(val.SubItems[0].Text);
		DateTime dateTime = Convert.ToDateTime(val.SubItems[4].Text);
		int year = dateTime.Year;
		int month = dateTime.Month;
		int day = dateTime.Day;
		int hour = dateTime.Hour;
		int minute = dateTime.Minute;
		int second = dateTime.Second;
		((Control)lblMessage).Text = "Getting log photo ...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			return;
		}
		byte[] abytPhotoImage = new byte[0];
		int anPhotoLength = 0;
		num = FKAttendDLL.FK_GetLogPhoto(FKAttendDLL.nCommHandleIndex, anEnrollNumber, year, month, day, hour, minute, second, abytPhotoImage, ref anPhotoLength);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
			return;
		}
		abytPhotoImage = new byte[anPhotoLength];
		num = FKAttendDLL.FK_GetLogPhoto(FKAttendDLL.nCommHandleIndex, anEnrollNumber, year, month, day, hour, minute, second, abytPhotoImage, ref anPhotoLength);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		else
		{
			((Control)lblMessage).Text = "GetLogPhoto OK";
			object obj = new VariantWrapper(abytPhotoImage);
			axAxImage1.SetJpgImage(ref obj);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
	}
}
