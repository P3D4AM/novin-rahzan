using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmTrMode : Form
{
	private AxSBXPC bpc;

	private const int DB_TRMODE_COUNT = 10;

	private const int DB_TRMODE_BYTE_COUNT = 5;

	private int[] trModeInfo;

	private IContainer components = null;

	private ListBox lstTrMode;

	private DateTimePicker dtEnd;

	public Label label1;

	private DateTimePicker dtStart;

	public Button cmdExit;

	public Button cmdWrite;

	public Button cmdRead;

	public Button cmdUpdate;

	public Label lblEnrollData;

	public Label lblMessage;

	public Label label2;

	private ComboBox cmbTrMode;

	public Button cmdSaveFile;

	public Button cmdLoadFile;

	private OpenFileDialog OpenFileDlg;

	private SaveFileDialog OpenSaveDlg;

	public frmTrMode(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void frmTrMode_Load(object sender, EventArgs e)
	{
		trModeInfo = new int[50];
		InitTrMode();
		DrawTrModeList();
		dtStart.ShowUpDown = true;
		dtEnd.ShowUpDown = true;
	}

	private void InitTrMode()
	{
		for (int i = 0; i < 10; i++)
		{
			trModeInfo[i * 5] = 0;
			trModeInfo[i * 5 + 1] = 0;
			trModeInfo[i * 5 + 2] = 0;
			trModeInfo[i * 5 + 3] = 0;
			trModeInfo[i * 5 + 4] = 0;
		}
	}

	private void DrawTrModeList()
	{
		lstTrMode.Items.Clear();
		for (int i = 0; i < 10; i++)
		{
			string text = "[No.]" + $"{i:D2}" + " ";
			text += "[S]";
			text = text + $"{trModeInfo[i * 5 + 1]:D2}" + ":";
			text = text + $"{trModeInfo[i * 5 + 2]:D2}" + " ";
			text += "[E]";
			text = text + $"{trModeInfo[i * 5 + 3]:D2}" + ":";
			text = text + $"{trModeInfo[i * 5 + 4]:D2}" + " ";
			text += cmbTrMode.Items[trModeInfo[i * 5]];
			lstTrMode.Items.Add((object)text);
		}
	}

	private void frmTrMode_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void lstTrMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)lstTrMode).SelectedIndex != -1)
		{
			int num = ((ListControl)lstTrMode).SelectedIndex * 5;
			int hour = trModeInfo[num + 1];
			int minute = trModeInfo[num + 2];
			int hour2 = trModeInfo[num + 3];
			int minute2 = trModeInfo[num + 4];
			int selectedIndex = trModeInfo[num];
			dtStart.Value = new DateTime(2000, 1, 1, hour, minute, 0);
			dtEnd.Value = new DateTime(2000, 1, 1, hour2, minute2, 0);
			((ListControl)cmbTrMode).SelectedIndex = selectedIndex;
		}
	}

	private void cmdUpdate_Click(object sender, EventArgs e)
	{
		if (((ListControl)lstTrMode).SelectedIndex != -1)
		{
			int num = ((ListControl)lstTrMode).SelectedIndex * 5;
			trModeInfo[num + 1] = dtStart.Value.Hour;
			trModeInfo[num + 2] = dtStart.Value.Minute;
			trModeInfo[num + 3] = dtEnd.Value.Hour;
			trModeInfo[num + 4] = dtEnd.Value.Minute;
			trModeInfo[num] = ((ListControl)cmbTrMode).SelectedIndex;
			DrawTrModeList();
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
		int dwValue = GCHandle.Alloc(trModeInfo, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
		if (bpc.GetDeviceLongInfo(util.gMachineNumber, 5, ref dwValue))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
		DrawTrModeList();
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
		int dwValue = GCHandle.Alloc(trModeInfo, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
		if (bpc.SetDeviceLongInfo(util.gMachineNumber, 5, ref dwValue))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
		DrawTrModeList();
	}

	private void cmdLoadFile_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = ((CommonDialog)OpenFileDlg).ShowDialog();
		if ((int)val != 2)
		{
			if (!util.LoadDeviceLongInfo(((FileDialog)OpenFileDlg).FileName, trModeInfo))
			{
				MessageBox.Show("Failed to open file : " + ((FileDialog)OpenFileDlg).FileName);
			}
			else
			{
				DrawTrModeList();
			}
		}
	}

	private void cmdSaveFile_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = ((CommonDialog)OpenSaveDlg).ShowDialog();
		if ((int)val != 2 && !util.SaveDeviceLongInfo(((FileDialog)OpenSaveDlg).FileName, trModeInfo))
		{
			MessageBox.Show("Unexpected Error");
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
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Expected O, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Expected O, but got Unknown
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1c: Expected O, but got Unknown
		//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0c: Expected O, but got Unknown
		//IL_0d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Expected O, but got Unknown
		lstTrMode = new ListBox();
		dtEnd = new DateTimePicker();
		label1 = new Label();
		dtStart = new DateTimePicker();
		cmdExit = new Button();
		cmdWrite = new Button();
		cmdRead = new Button();
		cmdUpdate = new Button();
		lblEnrollData = new Label();
		lblMessage = new Label();
		label2 = new Label();
		cmbTrMode = new ComboBox();
		cmdSaveFile = new Button();
		cmdLoadFile = new Button();
		OpenFileDlg = new OpenFileDialog();
		OpenSaveDlg = new SaveFileDialog();
		((Control)this).SuspendLayout();
		((Control)lstTrMode).Font = new Font("Times New Roman", 12f);
		((ListControl)lstTrMode).FormattingEnabled = true;
		lstTrMode.ItemHeight = 19;
		((Control)lstTrMode).Location = new Point(16, 126);
		((Control)lstTrMode).Name = "lstTrMode";
		((Control)lstTrMode).Size = new Size(501, 384);
		((Control)lstTrMode).TabIndex = 90;
		lstTrMode.SelectedIndexChanged += lstTrMode_SelectedIndexChanged;
		dtEnd.CalendarFont = new Font("Times New Roman", 12f, (FontStyle)1);
		((Control)dtEnd).Font = new Font("Times New Roman", 12f);
		dtEnd.Format = (DateTimePickerFormat)4;
		((Control)dtEnd).Location = new Point(67, 87);
		((Control)dtEnd).Name = "dtEnd";
		((Control)dtEnd).Size = new Size(119, 26);
		((Control)dtEnd).TabIndex = 89;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = SystemColors.Control;
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = SystemColors.ControlText;
		((Control)label1).Location = new Point(12, 87);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)0;
		((Control)label1).Size = new Size(36, 19);
		((Control)label1).TabIndex = 88;
		((Control)label1).Text = "End:";
		dtStart.CalendarFont = new Font("Times New Roman", 12f, (FontStyle)1);
		((Control)dtStart).Font = new Font("Times New Roman", 12f);
		dtStart.Format = (DateTimePickerFormat)4;
		((Control)dtStart).Location = new Point(67, 58);
		((Control)dtStart).Name = "dtStart";
		((Control)dtStart).Size = new Size(119, 26);
		((Control)dtStart).TabIndex = 87;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(540, 465);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(104, 46);
		((Control)cmdExit).TabIndex = 86;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdWrite).BackColor = SystemColors.Control;
		((Control)cmdWrite).Cursor = Cursors.Default;
		((Control)cmdWrite).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdWrite).ForeColor = SystemColors.ControlText;
		((Control)cmdWrite).Location = new Point(540, 401);
		((Control)cmdWrite).Name = "cmdWrite";
		((Control)cmdWrite).RightToLeft = (RightToLeft)0;
		((Control)cmdWrite).Size = new Size(104, 46);
		((Control)cmdWrite).TabIndex = 85;
		((Control)cmdWrite).Text = "Write";
		((ButtonBase)cmdWrite).UseVisualStyleBackColor = false;
		((Control)cmdWrite).Click += cmdWrite_Click;
		((Control)cmdRead).BackColor = SystemColors.Control;
		((Control)cmdRead).Cursor = Cursors.Default;
		((Control)cmdRead).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdRead).ForeColor = SystemColors.ControlText;
		((Control)cmdRead).Location = new Point(540, 336);
		((Control)cmdRead).Name = "cmdRead";
		((Control)cmdRead).RightToLeft = (RightToLeft)0;
		((Control)cmdRead).Size = new Size(104, 46);
		((Control)cmdRead).TabIndex = 84;
		((Control)cmdRead).Text = "Read";
		((ButtonBase)cmdRead).UseVisualStyleBackColor = false;
		((Control)cmdRead).Click += cmdRead_Click;
		((Control)cmdUpdate).BackColor = SystemColors.Control;
		((Control)cmdUpdate).Cursor = Cursors.Default;
		((Control)cmdUpdate).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdUpdate).ForeColor = SystemColors.ControlText;
		((Control)cmdUpdate).Location = new Point(540, 127);
		((Control)cmdUpdate).Name = "cmdUpdate";
		((Control)cmdUpdate).RightToLeft = (RightToLeft)0;
		((Control)cmdUpdate).Size = new Size(104, 46);
		((Control)cmdUpdate).TabIndex = 83;
		((Control)cmdUpdate).Text = "Update";
		((ButtonBase)cmdUpdate).UseVisualStyleBackColor = false;
		((Control)cmdUpdate).Click += cmdUpdate_Click;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(12, 58);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(41, 19);
		((Control)lblEnrollData).TabIndex = 82;
		((Control)lblEnrollData).Text = "Start:";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 9);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(632, 28);
		((Control)lblMessage).TabIndex = 81;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = SystemColors.Control;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(213, 58);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(92, 19);
		((Control)label2).TabIndex = 91;
		((Control)label2).Text = "In/Out Status:";
		((Control)cmbTrMode).Font = new Font("Times New Roman", 12f);
		((ListControl)cmbTrMode).FormattingEnabled = true;
		cmbTrMode.Items.AddRange(new object[6] { "Duty On", "Duty Off", "OverTime On", "OverTime Off", "Return", "Go Out" });
		((Control)cmbTrMode).Location = new Point(218, 88);
		((Control)cmbTrMode).Name = "cmbTrMode";
		((Control)cmbTrMode).Size = new Size(226, 27);
		((Control)cmbTrMode).TabIndex = 92;
		((Control)cmdSaveFile).BackColor = SystemColors.Control;
		((Control)cmdSaveFile).Cursor = Cursors.Default;
		((Control)cmdSaveFile).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSaveFile).ForeColor = SystemColors.ControlText;
		((Control)cmdSaveFile).Location = new Point(540, 271);
		((Control)cmdSaveFile).Name = "cmdSaveFile";
		((Control)cmdSaveFile).RightToLeft = (RightToLeft)0;
		((Control)cmdSaveFile).Size = new Size(104, 46);
		((Control)cmdSaveFile).TabIndex = 94;
		((Control)cmdSaveFile).Text = "Save To File";
		((ButtonBase)cmdSaveFile).UseVisualStyleBackColor = false;
		((Control)cmdSaveFile).Visible = false;
		((Control)cmdSaveFile).Click += cmdSaveFile_Click;
		((Control)cmdLoadFile).BackColor = SystemColors.Control;
		((Control)cmdLoadFile).Cursor = Cursors.Default;
		((Control)cmdLoadFile).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdLoadFile).ForeColor = SystemColors.ControlText;
		((Control)cmdLoadFile).Location = new Point(540, 206);
		((Control)cmdLoadFile).Name = "cmdLoadFile";
		((Control)cmdLoadFile).RightToLeft = (RightToLeft)0;
		((Control)cmdLoadFile).Size = new Size(104, 46);
		((Control)cmdLoadFile).TabIndex = 93;
		((Control)cmdLoadFile).Text = "Load From File";
		((ButtonBase)cmdLoadFile).UseVisualStyleBackColor = false;
		((Control)cmdLoadFile).Visible = false;
		((Control)cmdLoadFile).Click += cmdLoadFile_Click;
		((FileDialog)OpenFileDlg).FileName = "openFileDialog1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(660, 530);
		((Control)this).Controls.Add((Control)(object)cmdSaveFile);
		((Control)this).Controls.Add((Control)(object)cmdLoadFile);
		((Control)this).Controls.Add((Control)(object)cmbTrMode);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)lstTrMode);
		((Control)this).Controls.Add((Control)(object)dtEnd);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)dtStart);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdWrite);
		((Control)this).Controls.Add((Control)(object)cmdRead);
		((Control)this).Controls.Add((Control)(object)cmdUpdate);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmTrMode";
		((Control)this).Text = "frmTrMode";
		((Form)this).Load += frmTrMode_Load;
		((Form)this).FormClosing += new FormClosingEventHandler(frmTrMode_FormClosing);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
