using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmAdjust : Form
{
	public TextBox txtRestoredHour;

	public TextBox txtAdjustedHour;

	public TextBox txtRestoredMinute;

	public TextBox txtAdjustedMinute;

	public Button cmdSetAdjustInfo;

	public Button cmdGetAdjustInfo;

	public TextBox txtAdjustedDay;

	public TextBox txtRestoredDay;

	public TextBox txtAdjustedMonth;

	public TextBox txtRestoredMonth;

	public ComboBox cmbAdjustedState;

	public Label Label6;

	public Label Label5;

	public Label Label4;

	public Label Label3;

	public Label Label1;

	public Label lblChangeFlag;

	public Label Label42;

	public Label Label43;

	public Label lblMessage;

	public ComboBox cmbRestoredState;

	public Label label7;

	public Label label8;

	public Label label2;

	public Label label9;

	public Label label10;

	private Container components = null;

	public frmAdjust()
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
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Expected O, but got Unknown
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Expected O, but got Unknown
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Expected O, but got Unknown
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Expected O, but got Unknown
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Expected O, but got Unknown
		//IL_0bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c02: Expected O, but got Unknown
		//IL_0cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cda: Expected O, but got Unknown
		//IL_0da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daf: Expected O, but got Unknown
		//IL_0e77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e81: Expected O, but got Unknown
		//IL_0f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f56: Expected O, but got Unknown
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Expected O, but got Unknown
		//IL_10d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e3: Expected O, but got Unknown
		//IL_11ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b8: Expected O, but got Unknown
		//IL_1294: Unknown result type (might be due to invalid IL or missing references)
		//IL_129e: Expected O, but got Unknown
		//IL_1369: Unknown result type (might be due to invalid IL or missing references)
		//IL_1373: Expected O, but got Unknown
		//IL_143e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1448: Expected O, but got Unknown
		//IL_1513: Unknown result type (might be due to invalid IL or missing references)
		//IL_151d: Expected O, but got Unknown
		//IL_15e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f2: Expected O, but got Unknown
		txtRestoredHour = new TextBox();
		txtAdjustedHour = new TextBox();
		txtRestoredMinute = new TextBox();
		txtAdjustedMinute = new TextBox();
		cmdSetAdjustInfo = new Button();
		cmdGetAdjustInfo = new Button();
		txtAdjustedDay = new TextBox();
		txtRestoredDay = new TextBox();
		txtAdjustedMonth = new TextBox();
		txtRestoredMonth = new TextBox();
		cmbAdjustedState = new ComboBox();
		Label6 = new Label();
		Label5 = new Label();
		Label4 = new Label();
		Label3 = new Label();
		Label1 = new Label();
		lblChangeFlag = new Label();
		Label42 = new Label();
		Label43 = new Label();
		lblMessage = new Label();
		cmbRestoredState = new ComboBox();
		label7 = new Label();
		label8 = new Label();
		label2 = new Label();
		label9 = new Label();
		label10 = new Label();
		((Control)this).SuspendLayout();
		txtRestoredHour.AcceptsReturn = true;
		((Control)txtRestoredHour).BackColor = SystemColors.Window;
		((Control)txtRestoredHour).Cursor = Cursors.IBeam;
		((Control)txtRestoredHour).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtRestoredHour).ForeColor = SystemColors.WindowText;
		((Control)txtRestoredHour).Location = new Point(209, 124);
		((TextBoxBase)txtRestoredHour).MaxLength = 32;
		((Control)txtRestoredHour).Name = "txtRestoredHour";
		((Control)txtRestoredHour).RightToLeft = (RightToLeft)0;
		((Control)txtRestoredHour).Size = new Size(34, 26);
		((Control)txtRestoredHour).TabIndex = 51;
		txtAdjustedHour.AcceptsReturn = true;
		((Control)txtAdjustedHour).BackColor = SystemColors.Window;
		((Control)txtAdjustedHour).Cursor = Cursors.IBeam;
		((Control)txtAdjustedHour).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtAdjustedHour).ForeColor = SystemColors.WindowText;
		((Control)txtAdjustedHour).Location = new Point(209, 84);
		((TextBoxBase)txtAdjustedHour).MaxLength = 32;
		((Control)txtAdjustedHour).Name = "txtAdjustedHour";
		((Control)txtAdjustedHour).RightToLeft = (RightToLeft)0;
		((Control)txtAdjustedHour).Size = new Size(34, 26);
		((Control)txtAdjustedHour).TabIndex = 46;
		txtRestoredMinute.AcceptsReturn = true;
		((Control)txtRestoredMinute).BackColor = SystemColors.Window;
		((Control)txtRestoredMinute).Cursor = Cursors.IBeam;
		((Control)txtRestoredMinute).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtRestoredMinute).ForeColor = SystemColors.WindowText;
		((Control)txtRestoredMinute).Location = new Point(258, 124);
		((TextBoxBase)txtRestoredMinute).MaxLength = 32;
		((Control)txtRestoredMinute).Name = "txtRestoredMinute";
		((Control)txtRestoredMinute).RightToLeft = (RightToLeft)0;
		((Control)txtRestoredMinute).Size = new Size(34, 26);
		((Control)txtRestoredMinute).TabIndex = 52;
		txtAdjustedMinute.AcceptsReturn = true;
		((Control)txtAdjustedMinute).BackColor = SystemColors.Window;
		((Control)txtAdjustedMinute).Cursor = Cursors.IBeam;
		((Control)txtAdjustedMinute).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtAdjustedMinute).ForeColor = SystemColors.WindowText;
		((Control)txtAdjustedMinute).Location = new Point(258, 84);
		((TextBoxBase)txtAdjustedMinute).MaxLength = 32;
		((Control)txtAdjustedMinute).Name = "txtAdjustedMinute";
		((Control)txtAdjustedMinute).RightToLeft = (RightToLeft)0;
		((Control)txtAdjustedMinute).Size = new Size(34, 26);
		((Control)txtAdjustedMinute).TabIndex = 47;
		((Control)cmdSetAdjustInfo).BackColor = SystemColors.Control;
		((Control)cmdSetAdjustInfo).Cursor = Cursors.Default;
		((Control)cmdSetAdjustInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetAdjustInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdSetAdjustInfo).Location = new Point(262, 173);
		((Control)cmdSetAdjustInfo).Name = "cmdSetAdjustInfo";
		((Control)cmdSetAdjustInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdSetAdjustInfo).Size = new Size(115, 28);
		((Control)cmdSetAdjustInfo).TabIndex = 55;
		((Control)cmdSetAdjustInfo).Text = "Set";
		((ButtonBase)cmdSetAdjustInfo).UseVisualStyleBackColor = false;
		((Control)cmdSetAdjustInfo).Click += cmdSetAdjustInfo_Click;
		((Control)cmdGetAdjustInfo).BackColor = SystemColors.Control;
		((Control)cmdGetAdjustInfo).Cursor = Cursors.Default;
		((Control)cmdGetAdjustInfo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetAdjustInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetAdjustInfo).Location = new Point(142, 173);
		((Control)cmdGetAdjustInfo).Name = "cmdGetAdjustInfo";
		((Control)cmdGetAdjustInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetAdjustInfo).Size = new Size(115, 28);
		((Control)cmdGetAdjustInfo).TabIndex = 54;
		((Control)cmdGetAdjustInfo).Text = "Get";
		((ButtonBase)cmdGetAdjustInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetAdjustInfo).Click += cmdGetAdjustInfo_Click;
		txtAdjustedDay.AcceptsReturn = true;
		((Control)txtAdjustedDay).BackColor = SystemColors.Window;
		((Control)txtAdjustedDay).Cursor = Cursors.IBeam;
		((Control)txtAdjustedDay).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtAdjustedDay).ForeColor = SystemColors.WindowText;
		((Control)txtAdjustedDay).Location = new Point(158, 84);
		((TextBoxBase)txtAdjustedDay).MaxLength = 32;
		((Control)txtAdjustedDay).Name = "txtAdjustedDay";
		((Control)txtAdjustedDay).RightToLeft = (RightToLeft)0;
		((Control)txtAdjustedDay).Size = new Size(34, 26);
		((Control)txtAdjustedDay).TabIndex = 45;
		txtRestoredDay.AcceptsReturn = true;
		((Control)txtRestoredDay).BackColor = SystemColors.Window;
		((Control)txtRestoredDay).Cursor = Cursors.IBeam;
		((Control)txtRestoredDay).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtRestoredDay).ForeColor = SystemColors.WindowText;
		((Control)txtRestoredDay).Location = new Point(158, 124);
		((TextBoxBase)txtRestoredDay).MaxLength = 32;
		((Control)txtRestoredDay).Name = "txtRestoredDay";
		((Control)txtRestoredDay).RightToLeft = (RightToLeft)0;
		((Control)txtRestoredDay).Size = new Size(34, 26);
		((Control)txtRestoredDay).TabIndex = 50;
		txtAdjustedMonth.AcceptsReturn = true;
		((Control)txtAdjustedMonth).BackColor = SystemColors.Window;
		((Control)txtAdjustedMonth).Cursor = Cursors.IBeam;
		((Control)txtAdjustedMonth).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtAdjustedMonth).ForeColor = SystemColors.WindowText;
		((Control)txtAdjustedMonth).Location = new Point(110, 84);
		((TextBoxBase)txtAdjustedMonth).MaxLength = 32;
		((Control)txtAdjustedMonth).Name = "txtAdjustedMonth";
		((Control)txtAdjustedMonth).RightToLeft = (RightToLeft)0;
		((Control)txtAdjustedMonth).Size = new Size(34, 26);
		((Control)txtAdjustedMonth).TabIndex = 44;
		txtRestoredMonth.AcceptsReturn = true;
		((Control)txtRestoredMonth).BackColor = SystemColors.Window;
		((Control)txtRestoredMonth).Cursor = Cursors.IBeam;
		((Control)txtRestoredMonth).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtRestoredMonth).ForeColor = SystemColors.WindowText;
		((Control)txtRestoredMonth).Location = new Point(110, 124);
		((TextBoxBase)txtRestoredMonth).MaxLength = 32;
		((Control)txtRestoredMonth).Name = "txtRestoredMonth";
		((Control)txtRestoredMonth).RightToLeft = (RightToLeft)0;
		((Control)txtRestoredMonth).Size = new Size(34, 26);
		((Control)txtRestoredMonth).TabIndex = 49;
		((Control)cmbAdjustedState).BackColor = SystemColors.Window;
		((Control)cmbAdjustedState).Cursor = Cursors.Default;
		cmbAdjustedState.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbAdjustedState).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbAdjustedState).ForeColor = SystemColors.WindowText;
		cmbAdjustedState.Items.AddRange(new object[3] { "None", "-1", "+1" });
		((Control)cmbAdjustedState).Location = new Point(305, 84);
		((Control)cmbAdjustedState).Name = "cmbAdjustedState";
		((Control)cmbAdjustedState).RightToLeft = (RightToLeft)0;
		((Control)cmbAdjustedState).Size = new Size(83, 27);
		((Control)cmbAdjustedState).TabIndex = 48;
		cmbAdjustedState.SelectedIndexChanged += cmbAdjustedState_SelectedIndexChanged;
		((Control)Label6).AutoSize = true;
		((Control)Label6).BackColor = SystemColors.Control;
		((Control)Label6).Cursor = Cursors.Default;
		((Control)Label6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label6).ForeColor = SystemColors.ControlText;
		((Control)Label6).Location = new Point(245, 127);
		((Control)Label6).Name = "Label6";
		((Control)Label6).RightToLeft = (RightToLeft)0;
		((Control)Label6).Size = new Size(12, 19);
		((Control)Label6).TabIndex = 65;
		((Control)Label6).Text = ":";
		Label6.TextAlign = (ContentAlignment)4;
		((Control)Label5).AutoSize = true;
		((Control)Label5).BackColor = SystemColors.Control;
		((Control)Label5).Cursor = Cursors.Default;
		((Control)Label5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label5).ForeColor = SystemColors.ControlText;
		((Control)Label5).Location = new Point(245, 87);
		((Control)Label5).Name = "Label5";
		((Control)Label5).RightToLeft = (RightToLeft)0;
		((Control)Label5).Size = new Size(12, 19);
		((Control)Label5).TabIndex = 64;
		((Control)Label5).Text = ":";
		Label5.TextAlign = (ContentAlignment)4;
		((Control)Label4).AutoSize = true;
		((Control)Label4).BackColor = SystemColors.Control;
		((Control)Label4).Cursor = Cursors.Default;
		((Control)Label4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label4).ForeColor = SystemColors.ControlText;
		((Control)Label4).Location = new Point(144, 128);
		((Control)Label4).Name = "Label4";
		((Control)Label4).RightToLeft = (RightToLeft)0;
		((Control)Label4).Size = new Size(15, 19);
		((Control)Label4).TabIndex = 63;
		((Control)Label4).Text = "-";
		Label4.TextAlign = (ContentAlignment)4;
		((Control)Label3).AutoSize = true;
		((Control)Label3).BackColor = SystemColors.Control;
		((Control)Label3).Cursor = Cursors.Default;
		((Control)Label3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label3).ForeColor = SystemColors.ControlText;
		((Control)Label3).Location = new Point(144, 88);
		((Control)Label3).Name = "Label3";
		((Control)Label3).RightToLeft = (RightToLeft)0;
		((Control)Label3).Size = new Size(15, 19);
		((Control)Label3).TabIndex = 62;
		((Control)Label3).Text = "-";
		Label3.TextAlign = (ContentAlignment)4;
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(110, 60);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(37, 19);
		((Control)Label1).TabIndex = 60;
		((Control)Label1).Text = "MM";
		Label1.TextAlign = (ContentAlignment)4;
		((Control)lblChangeFlag).AutoSize = true;
		((Control)lblChangeFlag).BackColor = SystemColors.Control;
		((Control)lblChangeFlag).Cursor = Cursors.Default;
		((Control)lblChangeFlag).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblChangeFlag).ForeColor = SystemColors.ControlText;
		((Control)lblChangeFlag).Location = new Point(298, 60);
		((Control)lblChangeFlag).Name = "lblChangeFlag";
		((Control)lblChangeFlag).RightToLeft = (RightToLeft)0;
		((Control)lblChangeFlag).Size = new Size(90, 19);
		((Control)lblChangeFlag).TabIndex = 58;
		((Control)lblChangeFlag).Text = "Change State";
		lblChangeFlag.TextAlign = (ContentAlignment)4;
		((Control)Label42).AutoSize = true;
		((Control)Label42).BackColor = Color.Transparent;
		((Control)Label42).Cursor = Cursors.Default;
		((Control)Label42).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label42).ForeColor = SystemColors.ControlText;
		((Control)Label42).Location = new Point(22, 87);
		((Control)Label42).Name = "Label42";
		((Control)Label42).RightToLeft = (RightToLeft)0;
		((Control)Label42).Size = new Size(83, 19);
		((Control)Label42).TabIndex = 57;
		((Control)Label42).Text = "Adjusted on";
		((Control)Label43).AutoSize = true;
		((Control)Label43).BackColor = Color.Transparent;
		((Control)Label43).Cursor = Cursors.Default;
		((Control)Label43).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label43).ForeColor = SystemColors.ControlText;
		((Control)Label43).Location = new Point(22, 128);
		((Control)Label43).Name = "Label43";
		((Control)Label43).RightToLeft = (RightToLeft)0;
		((Control)Label43).Size = new Size(83, 19);
		((Control)Label43).TabIndex = 56;
		((Control)Label43).Text = "Restored on";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(14, 12);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(361, 28);
		((Control)lblMessage).TabIndex = 59;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)cmbRestoredState).BackColor = SystemColors.Window;
		((Control)cmbRestoredState).Cursor = Cursors.Default;
		cmbRestoredState.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbRestoredState).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbRestoredState).ForeColor = SystemColors.WindowText;
		cmbRestoredState.Items.AddRange(new object[3] { "None", "-1", "+1" });
		((Control)cmbRestoredState).Location = new Point(305, 124);
		((Control)cmbRestoredState).Name = "cmbRestoredState";
		((Control)cmbRestoredState).RightToLeft = (RightToLeft)0;
		((Control)cmbRestoredState).Size = new Size(83, 27);
		((Control)cmbRestoredState).TabIndex = 53;
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = SystemColors.Control;
		((Control)label7).Cursor = Cursors.Default;
		((Control)label7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label7).ForeColor = SystemColors.ControlText;
		((Control)label7).Location = new Point(158, 60);
		((Control)label7).Name = "label7";
		((Control)label7).RightToLeft = (RightToLeft)0;
		((Control)label7).Size = new Size(31, 19);
		((Control)label7).TabIndex = 60;
		((Control)label7).Text = "DD";
		label7.TextAlign = (ContentAlignment)4;
		((Control)label8).AutoSize = true;
		((Control)label8).BackColor = SystemColors.Control;
		((Control)label8).Cursor = Cursors.Default;
		((Control)label8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label8).ForeColor = SystemColors.ControlText;
		((Control)label8).Location = new Point(144, 60);
		((Control)label8).Name = "label8";
		((Control)label8).RightToLeft = (RightToLeft)0;
		((Control)label8).Size = new Size(15, 19);
		((Control)label8).TabIndex = 62;
		((Control)label8).Text = "-";
		label8.TextAlign = (ContentAlignment)4;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = SystemColors.Control;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(209, 60);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(31, 19);
		((Control)label2).TabIndex = 60;
		((Control)label2).Text = "HH";
		label2.TextAlign = (ContentAlignment)4;
		((Control)label9).AutoSize = true;
		((Control)label9).BackColor = SystemColors.Control;
		((Control)label9).Cursor = Cursors.Default;
		((Control)label9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label9).ForeColor = SystemColors.ControlText;
		((Control)label9).Location = new Point(258, 60);
		((Control)label9).Name = "label9";
		((Control)label9).RightToLeft = (RightToLeft)0;
		((Control)label9).Size = new Size(37, 19);
		((Control)label9).TabIndex = 60;
		((Control)label9).Text = "MM";
		label9.TextAlign = (ContentAlignment)4;
		((Control)label10).AutoSize = true;
		((Control)label10).BackColor = SystemColors.Control;
		((Control)label10).Cursor = Cursors.Default;
		((Control)label10).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label10).ForeColor = SystemColors.ControlText;
		((Control)label10).Location = new Point(245, 60);
		((Control)label10).Name = "label10";
		((Control)label10).RightToLeft = (RightToLeft)0;
		((Control)label10).Size = new Size(12, 19);
		((Control)label10).TabIndex = 64;
		((Control)label10).Text = ":";
		label10.TextAlign = (ContentAlignment)4;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(470, 230);
		((Control)this).Controls.Add((Control)(object)txtRestoredHour);
		((Control)this).Controls.Add((Control)(object)txtAdjustedHour);
		((Control)this).Controls.Add((Control)(object)txtRestoredMinute);
		((Control)this).Controls.Add((Control)(object)txtAdjustedMinute);
		((Control)this).Controls.Add((Control)(object)txtAdjustedDay);
		((Control)this).Controls.Add((Control)(object)txtRestoredDay);
		((Control)this).Controls.Add((Control)(object)txtAdjustedMonth);
		((Control)this).Controls.Add((Control)(object)txtRestoredMonth);
		((Control)this).Controls.Add((Control)(object)Label6);
		((Control)this).Controls.Add((Control)(object)label10);
		((Control)this).Controls.Add((Control)(object)Label5);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Controls.Add((Control)(object)label8);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)label9);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label7);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblChangeFlag);
		((Control)this).Controls.Add((Control)(object)Label42);
		((Control)this).Controls.Add((Control)(object)Label43);
		((Control)this).Controls.Add((Control)(object)cmdSetAdjustInfo);
		((Control)this).Controls.Add((Control)(object)cmdGetAdjustInfo);
		((Control)this).Controls.Add((Control)(object)cmbAdjustedState);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)cmbRestoredState);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmAdjust";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Adjuste/Restore Info";
		((Form)this).Load += frmAdjust_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void cmdGetAdjustInfo_Click(object sender, EventArgs e)
	{
		int apnAdjustedState = 0;
		int apnAdjustedHour = 0;
		int apnAdjustedMonth = 0;
		int apnAdjustedDay = 0;
		int apnAdjustedMinute = 0;
		int apnRestoredState = 0;
		int apnRestoredHour = 0;
		int apnRestoredMonth = 0;
		int apnRestoredDay = 0;
		int apnRestoredMinte = 0;
		((Control)lblMessage).Text = "";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			return;
		}
		num = FKAttendDLL.FK_GetAdjustInfo(FKAttendDLL.nCommHandleIndex, ref apnAdjustedState, ref apnAdjustedMonth, ref apnAdjustedDay, ref apnAdjustedHour, ref apnAdjustedMinute, ref apnRestoredState, ref apnRestoredMonth, ref apnRestoredDay, ref apnRestoredHour, ref apnRestoredMinte);
		if (num == 1)
		{
			if (apnAdjustedState < 3)
			{
				((ListControl)cmbAdjustedState).SelectedIndex = apnAdjustedState;
			}
			else
			{
				((ListControl)cmbAdjustedState).SelectedIndex = 0;
			}
			((Control)txtAdjustedMonth).Text = apnAdjustedMonth.ToString();
			((Control)txtAdjustedDay).Text = apnAdjustedDay.ToString();
			((Control)txtAdjustedHour).Text = apnAdjustedHour.ToString();
			((Control)txtAdjustedMinute).Text = apnAdjustedMinute.ToString();
			if (apnRestoredState < 3)
			{
				((ListControl)cmbRestoredState).SelectedIndex = apnRestoredState;
			}
			else
			{
				((ListControl)cmbRestoredState).SelectedIndex = 0;
			}
			((Control)txtRestoredMonth).Text = apnRestoredMonth.ToString();
			((Control)txtRestoredDay).Text = apnRestoredDay.ToString();
			((Control)txtRestoredHour).Text = apnRestoredHour.ToString();
			((Control)txtRestoredMinute).Text = apnRestoredMinte.ToString();
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			((Control)lblMessage).Text = "Failed";
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
	}

	private void cmdSetAdjustInfo_Click(object sender, EventArgs e)
	{
		((Control)lblMessage).Text = "";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			return;
		}
		int selectedIndex = ((ListControl)cmbAdjustedState).SelectedIndex;
		int anAdjustedMonth = Convert.ToInt32(((Control)txtAdjustedMonth).Text);
		int anAdjustedDay = Convert.ToInt32(((Control)txtAdjustedDay).Text);
		int anAdjustedHour = Convert.ToInt32(((Control)txtAdjustedHour).Text);
		int anAdjustedMinute = Convert.ToInt32(((Control)txtAdjustedMinute).Text);
		int selectedIndex2 = ((ListControl)cmbRestoredState).SelectedIndex;
		int anRestoredMonth = Convert.ToInt32(((Control)txtRestoredMonth).Text);
		int anRestoredDay = Convert.ToInt32(((Control)txtRestoredDay).Text);
		int anRestoredHour = Convert.ToInt32(((Control)txtRestoredHour).Text);
		int anRestoredMinte = Convert.ToInt32(((Control)txtRestoredMinute).Text);
		num = FKAttendDLL.FK_SetAdjustInfo(FKAttendDLL.nCommHandleIndex, selectedIndex, anAdjustedMonth, anAdjustedDay, anAdjustedHour, anAdjustedMinute, selectedIndex2, anRestoredMonth, anRestoredDay, anRestoredHour, anRestoredMinte);
		if (num == 1)
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			((Control)lblMessage).Text = "Failed!";
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
	}

	private void cmbAdjustedState_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)cmbAdjustedState).SelectedIndex == 0)
		{
			((ListControl)cmbRestoredState).SelectedIndex = 0;
		}
		if (((ListControl)cmbAdjustedState).SelectedIndex == 1)
		{
			((ListControl)cmbRestoredState).SelectedIndex = 2;
		}
		if (((ListControl)cmbAdjustedState).SelectedIndex == 2)
		{
			((ListControl)cmbRestoredState).SelectedIndex = 1;
		}
	}

	private void frmAdjust_Load(object sender, EventArgs e)
	{
		((ListControl)cmbAdjustedState).SelectedIndex = 0;
		((ListControl)cmbRestoredState).SelectedIndex = 0;
		((Control)txtAdjustedMonth).Text = "1";
		((Control)txtAdjustedDay).Text = "1";
		((Control)txtAdjustedHour).Text = "0";
		((Control)txtAdjustedMinute).Text = "0";
		((Control)txtRestoredMonth).Text = "1";
		((Control)txtRestoredDay).Text = "1";
		((Control)txtRestoredHour).Text = "0";
		((Control)txtRestoredMinute).Text = "0";
	}
}
