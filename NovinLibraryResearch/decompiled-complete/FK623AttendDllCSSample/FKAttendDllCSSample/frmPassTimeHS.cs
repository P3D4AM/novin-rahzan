using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmPassTimeHS : Form
{
	private TextBox[] mtxtStartHour;

	private TextBox[] mtxtStartMinute;

	private TextBox[] mtxtEndHour;

	private TextBox[] mtxtEndMinute;

	private IContainer components = null;

	public Label lblMessage;

	private GroupBox groupBox1;

	private Label label1;

	private ComboBox cmbTZN;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label9;

	private Label label8;

	private TextBox txtStartMinute1;

	private TextBox txtStartHour1;

	private Label label10;

	private Label label11;

	private TextBox txtEndMinute1;

	private TextBox txtEndHour1;

	private Label label12;

	private TextBox txtEndMinute2;

	private TextBox txtStartMinute2;

	private TextBox txtEndHour2;

	private TextBox txtStartHour2;

	private Label label15;

	private Label label14;

	private Label label13;

	private TextBox txtEndMinute6;

	private TextBox txtEndMinute5;

	private TextBox txtEndMinute4;

	private TextBox txtEndMinute3;

	private TextBox txtStartMinute6;

	private TextBox txtStartMinute5;

	private TextBox txtStartMinute4;

	private TextBox txtStartMinute3;

	private TextBox txtEndHour6;

	private TextBox txtEndHour5;

	private TextBox txtEndHour4;

	private TextBox txtEndHour3;

	private TextBox txtStartHour6;

	private TextBox txtStartHour5;

	private TextBox txtStartHour4;

	private TextBox txtStartHour3;

	private Label label27;

	private Label label24;

	private Label label21;

	private Label label26;

	private Label label18;

	private Label label23;

	private Label label20;

	private Label label25;

	private Label label17;

	private Label label22;

	private Label label19;

	private Label label16;

	private Button cmdReadTimeZone;

	private Button cmdWriteTimeZone;

	private GroupBox groupBox2;

	private Label label28;

	private Label label30;

	private Label label29;

	private TextBox txtEnrollNumber;

	private Label label37;

	private Label label36;

	private Label label35;

	private Label label34;

	private Label label33;

	private Label label32;

	private Label label31;

	private TextBox txtTimeZone7;

	private TextBox txtTimeZone6;

	private TextBox txtTimeZone5;

	private TextBox txtTimeZone4;

	private TextBox txtTimeZone3;

	private TextBox txtTimeZone2;

	private TextBox txtTimeZone1;

	private Button cmdReadUserWeekPassTime;

	private Button cmdWriteUserWeekPassTime;

	public frmPassTimeHS()
	{
		InitializeComponent();
	}

	private void frmPassTimeHS_Load(object sender, EventArgs e)
	{
		mtxtStartHour = (TextBox[])(object)new TextBox[6];
		mtxtStartMinute = (TextBox[])(object)new TextBox[6];
		mtxtEndHour = (TextBox[])(object)new TextBox[6];
		mtxtEndMinute = (TextBox[])(object)new TextBox[6];
		mtxtStartHour[0] = txtStartHour1;
		mtxtStartHour[1] = txtStartHour2;
		mtxtStartHour[2] = txtStartHour3;
		mtxtStartHour[3] = txtStartHour4;
		mtxtStartHour[4] = txtStartHour5;
		mtxtStartHour[5] = txtStartHour6;
		mtxtStartMinute[0] = txtStartMinute1;
		mtxtStartMinute[1] = txtStartMinute2;
		mtxtStartMinute[2] = txtStartMinute3;
		mtxtStartMinute[3] = txtStartMinute4;
		mtxtStartMinute[4] = txtStartMinute5;
		mtxtStartMinute[5] = txtStartMinute6;
		mtxtEndHour[0] = txtEndHour1;
		mtxtEndHour[1] = txtEndHour2;
		mtxtEndHour[2] = txtEndHour3;
		mtxtEndHour[3] = txtEndHour4;
		mtxtEndHour[4] = txtEndHour5;
		mtxtEndHour[5] = txtEndHour6;
		mtxtEndMinute[0] = txtEndMinute1;
		mtxtEndMinute[1] = txtEndMinute2;
		mtxtEndMinute[2] = txtEndMinute3;
		mtxtEndMinute[3] = txtEndMinute4;
		mtxtEndMinute[4] = txtEndMinute5;
		mtxtEndMinute[5] = txtEndMinute6;
		for (int i = 0; i < 255; i++)
		{
			cmbTZN.Items.Add((object)(i + 1));
		}
		for (int i = 0; i < 6; i++)
		{
			((Control)mtxtStartHour[i]).Text = "";
			((Control)mtxtStartMinute[i]).Text = "";
			((Control)mtxtEndHour[i]).Text = "";
			((Control)mtxtEndMinute[i]).Text = "";
		}
	}

	private void ShowTimeZoneValue(TIME_ZONE aTimeZone)
	{
		((ListControl)cmbTZN).SelectedIndex = aTimeZone.TimeZoneId - 1;
		for (int i = 0; i < 6; i++)
		{
			((Control)mtxtStartHour[i]).Text = Convert.ToString(aTimeZone.TimeSlots[i].StartHour);
			((Control)mtxtStartMinute[i]).Text = Convert.ToString(aTimeZone.TimeSlots[i].StartMinute);
			((Control)mtxtEndHour[i]).Text = Convert.ToString(aTimeZone.TimeSlots[i].EndHour);
			((Control)mtxtEndMinute[i]).Text = Convert.ToString(aTimeZone.TimeSlots[i].EndMinute);
		}
	}

	private void GetTimeZoneValue(ref TIME_ZONE aTimeZone)
	{
		aTimeZone.TimeZoneId = ((ListControl)cmbTZN).SelectedIndex + 1;
		for (int i = 0; i < 6; i++)
		{
			aTimeZone.TimeSlots[i].StartHour = (byte)FKAttendDLL.GetInt(((Control)mtxtStartHour[i]).Text);
			aTimeZone.TimeSlots[i].StartMinute = (byte)FKAttendDLL.GetInt(((Control)mtxtStartMinute[i]).Text);
			aTimeZone.TimeSlots[i].EndHour = (byte)FKAttendDLL.GetInt(((Control)mtxtEndHour[i]).Text);
			aTimeZone.TimeSlots[i].EndMinute = (byte)FKAttendDLL.GetInt(((Control)mtxtEndMinute[i]).Text);
		}
	}

	private void cmdReadTimeZone_Click(object sender, EventArgs e)
	{
		byte[] array = new byte[32];
		TIME_ZONE tIME_ZONE = default(TIME_ZONE);
		((Control)cmdReadTimeZone).Enabled = false;
		((Control)lblMessage).Text = "Working ...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdReadTimeZone).Enabled = true;
			return;
		}
		int num2 = ((ListControl)cmbTZN).SelectedIndex + 1;
		if (num2 < 1 || num2 > 255)
		{
			num2 = 1;
			((ListControl)cmbTZN).SelectedIndex = 0;
		}
		tIME_ZONE.Init();
		tIME_ZONE.TimeZoneId = num2;
		FKAttendDLL.ConvertStructureToByteArray(tIME_ZONE, array);
		num = FKAttendDLL.FK_HS_GetTimeZone(FKAttendDLL.nCommHandleIndex, array);
		if (num == 1)
		{
			((Control)lblMessage).Text = "Success !";
			tIME_ZONE = (TIME_ZONE)FKAttendDLL.ConvertByteArrayToStructure(array, typeof(TIME_ZONE));
			ShowTimeZoneValue(tIME_ZONE);
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdReadTimeZone).Enabled = true;
	}

	private void cmdWriteTimeZone_Click(object sender, EventArgs e)
	{
		byte[] array = new byte[32];
		TIME_ZONE aTimeZone = default(TIME_ZONE);
		((Control)cmdWriteTimeZone).Enabled = false;
		((Control)lblMessage).Text = "Working ...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdWriteTimeZone).Enabled = true;
			return;
		}
		aTimeZone.Init();
		GetTimeZoneValue(ref aTimeZone);
		if (aTimeZone.TimeZoneId < 1 || aTimeZone.TimeZoneId > 255)
		{
			aTimeZone.TimeZoneId = 1;
			((ListControl)cmbTZN).SelectedIndex = 0;
		}
		FKAttendDLL.ConvertStructureToByteArray(aTimeZone, array);
		num = FKAttendDLL.FK_HS_SetTimeZone(FKAttendDLL.nCommHandleIndex, array);
		if (num == 1)
		{
			((Control)lblMessage).Text = "Success !";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdWriteTimeZone).Enabled = true;
	}

	private void ShowUserWeekPassTimeValue(USER_WEEK_PASS_TIME aUserWeekPassTime)
	{
		((Control)txtEnrollNumber).Text = Convert.ToString(aUserWeekPassTime.UserId);
		((Control)txtTimeZone1).Text = Convert.ToString(aUserWeekPassTime.WeekPassTime[0]);
		((Control)txtTimeZone2).Text = Convert.ToString(aUserWeekPassTime.WeekPassTime[1]);
		((Control)txtTimeZone3).Text = Convert.ToString(aUserWeekPassTime.WeekPassTime[2]);
		((Control)txtTimeZone4).Text = Convert.ToString(aUserWeekPassTime.WeekPassTime[3]);
		((Control)txtTimeZone5).Text = Convert.ToString(aUserWeekPassTime.WeekPassTime[4]);
		((Control)txtTimeZone6).Text = Convert.ToString(aUserWeekPassTime.WeekPassTime[5]);
		((Control)txtTimeZone7).Text = Convert.ToString(aUserWeekPassTime.WeekPassTime[6]);
	}

	private void GetUserWeekPassTimeValue(ref USER_WEEK_PASS_TIME aUserWeekPassTime)
	{
		aUserWeekPassTime.UserId = FKAttendDLL.GetInt(((Control)txtEnrollNumber).Text);
		aUserWeekPassTime.WeekPassTime[0] = (byte)FKAttendDLL.GetInt(((Control)txtTimeZone1).Text);
		aUserWeekPassTime.WeekPassTime[1] = (byte)FKAttendDLL.GetInt(((Control)txtTimeZone2).Text);
		aUserWeekPassTime.WeekPassTime[2] = (byte)FKAttendDLL.GetInt(((Control)txtTimeZone3).Text);
		aUserWeekPassTime.WeekPassTime[3] = (byte)FKAttendDLL.GetInt(((Control)txtTimeZone4).Text);
		aUserWeekPassTime.WeekPassTime[4] = (byte)FKAttendDLL.GetInt(((Control)txtTimeZone5).Text);
		aUserWeekPassTime.WeekPassTime[5] = (byte)FKAttendDLL.GetInt(((Control)txtTimeZone6).Text);
		aUserWeekPassTime.WeekPassTime[6] = (byte)FKAttendDLL.GetInt(((Control)txtTimeZone7).Text);
	}

	private void cmdReadUserWeekPassTime_Click(object sender, EventArgs e)
	{
		byte[] array = new byte[16];
		USER_WEEK_PASS_TIME uSER_WEEK_PASS_TIME = default(USER_WEEK_PASS_TIME);
		uSER_WEEK_PASS_TIME.Init();
		uSER_WEEK_PASS_TIME.UserId = FKAttendDLL.GetInt(((Control)txtEnrollNumber).Text);
		((Control)cmdReadUserWeekPassTime).Enabled = false;
		((Control)lblMessage).Text = "Working ...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdReadUserWeekPassTime).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_HS_GetUserWeekPassTime(FKAttendDLL.nCommHandleIndex, array);
		if (num == 1)
		{
			((Control)lblMessage).Text = "Success !";
			uSER_WEEK_PASS_TIME = (USER_WEEK_PASS_TIME)FKAttendDLL.ConvertByteArrayToStructure(array, typeof(USER_WEEK_PASS_TIME));
			ShowUserWeekPassTimeValue(uSER_WEEK_PASS_TIME);
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdReadUserWeekPassTime).Enabled = true;
	}

	private void cmdWriteUserWeekPassTime_Click(object sender, EventArgs e)
	{
		byte[] array = new byte[16];
		USER_WEEK_PASS_TIME aUserWeekPassTime = default(USER_WEEK_PASS_TIME);
		aUserWeekPassTime.Init();
		GetUserWeekPassTimeValue(ref aUserWeekPassTime);
		((Control)cmdWriteUserWeekPassTime).Enabled = false;
		((Control)lblMessage).Text = "Working ...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdWriteUserWeekPassTime).Enabled = true;
			return;
		}
		FKAttendDLL.ConvertStructureToByteArray(aUserWeekPassTime, array);
		num = FKAttendDLL.FK_HS_SetUserWeekPassTime(FKAttendDLL.nCommHandleIndex, array);
		if (num == 1)
		{
			((Control)lblMessage).Text = "Success !";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdWriteUserWeekPassTime).Enabled = true;
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
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Expected O, but got Unknown
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Expected O, but got Unknown
		//IL_1f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f64: Expected O, but got Unknown
		lblMessage = new Label();
		groupBox1 = new GroupBox();
		cmdReadTimeZone = new Button();
		cmdWriteTimeZone = new Button();
		txtEndMinute6 = new TextBox();
		txtEndHour1 = new TextBox();
		txtEndMinute5 = new TextBox();
		txtEndMinute4 = new TextBox();
		txtEndMinute3 = new TextBox();
		txtEndMinute2 = new TextBox();
		txtEndMinute1 = new TextBox();
		txtStartMinute6 = new TextBox();
		txtStartMinute5 = new TextBox();
		txtStartMinute4 = new TextBox();
		txtStartMinute3 = new TextBox();
		txtStartMinute2 = new TextBox();
		txtStartMinute1 = new TextBox();
		txtEndHour6 = new TextBox();
		txtEndHour5 = new TextBox();
		txtEndHour4 = new TextBox();
		txtEndHour3 = new TextBox();
		txtEndHour2 = new TextBox();
		txtStartHour6 = new TextBox();
		txtStartHour5 = new TextBox();
		txtStartHour4 = new TextBox();
		txtStartHour3 = new TextBox();
		txtStartHour2 = new TextBox();
		txtStartHour1 = new TextBox();
		cmbTZN = new ComboBox();
		label7 = new Label();
		label27 = new Label();
		label6 = new Label();
		label24 = new Label();
		label5 = new Label();
		label21 = new Label();
		label4 = new Label();
		label26 = new Label();
		label18 = new Label();
		label23 = new Label();
		label3 = new Label();
		label20 = new Label();
		label15 = new Label();
		label25 = new Label();
		label17 = new Label();
		label22 = new Label();
		label9 = new Label();
		label19 = new Label();
		label14 = new Label();
		label16 = new Label();
		label12 = new Label();
		label13 = new Label();
		label11 = new Label();
		label10 = new Label();
		label8 = new Label();
		label2 = new Label();
		label1 = new Label();
		groupBox2 = new GroupBox();
		cmdWriteUserWeekPassTime = new Button();
		cmdReadUserWeekPassTime = new Button();
		txtTimeZone7 = new TextBox();
		txtTimeZone6 = new TextBox();
		txtTimeZone5 = new TextBox();
		txtTimeZone4 = new TextBox();
		txtTimeZone3 = new TextBox();
		txtTimeZone2 = new TextBox();
		txtTimeZone1 = new TextBox();
		txtEnrollNumber = new TextBox();
		label30 = new Label();
		label37 = new Label();
		label36 = new Label();
		label35 = new Label();
		label34 = new Label();
		label33 = new Label();
		label32 = new Label();
		label31 = new Label();
		label29 = new Label();
		label28 = new Label();
		((Control)groupBox1).SuspendLayout();
		((Control)groupBox2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 9);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(618, 28);
		((Control)lblMessage).TabIndex = 158;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)groupBox1).Controls.Add((Control)(object)cmdReadTimeZone);
		((Control)groupBox1).Controls.Add((Control)(object)cmdWriteTimeZone);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndMinute6);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndHour1);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndMinute5);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndMinute4);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndMinute3);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndMinute2);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndMinute1);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartMinute6);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartMinute5);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartMinute4);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartMinute3);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartMinute2);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartMinute1);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndHour6);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndHour5);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndHour4);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndHour3);
		((Control)groupBox1).Controls.Add((Control)(object)txtEndHour2);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartHour6);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartHour5);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartHour4);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartHour3);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartHour2);
		((Control)groupBox1).Controls.Add((Control)(object)txtStartHour1);
		((Control)groupBox1).Controls.Add((Control)(object)cmbTZN);
		((Control)groupBox1).Controls.Add((Control)(object)label7);
		((Control)groupBox1).Controls.Add((Control)(object)label27);
		((Control)groupBox1).Controls.Add((Control)(object)label6);
		((Control)groupBox1).Controls.Add((Control)(object)label24);
		((Control)groupBox1).Controls.Add((Control)(object)label5);
		((Control)groupBox1).Controls.Add((Control)(object)label21);
		((Control)groupBox1).Controls.Add((Control)(object)label4);
		((Control)groupBox1).Controls.Add((Control)(object)label26);
		((Control)groupBox1).Controls.Add((Control)(object)label18);
		((Control)groupBox1).Controls.Add((Control)(object)label23);
		((Control)groupBox1).Controls.Add((Control)(object)label3);
		((Control)groupBox1).Controls.Add((Control)(object)label20);
		((Control)groupBox1).Controls.Add((Control)(object)label15);
		((Control)groupBox1).Controls.Add((Control)(object)label25);
		((Control)groupBox1).Controls.Add((Control)(object)label17);
		((Control)groupBox1).Controls.Add((Control)(object)label22);
		((Control)groupBox1).Controls.Add((Control)(object)label9);
		((Control)groupBox1).Controls.Add((Control)(object)label19);
		((Control)groupBox1).Controls.Add((Control)(object)label14);
		((Control)groupBox1).Controls.Add((Control)(object)label16);
		((Control)groupBox1).Controls.Add((Control)(object)label12);
		((Control)groupBox1).Controls.Add((Control)(object)label13);
		((Control)groupBox1).Controls.Add((Control)(object)label11);
		((Control)groupBox1).Controls.Add((Control)(object)label10);
		((Control)groupBox1).Controls.Add((Control)(object)label8);
		((Control)groupBox1).Controls.Add((Control)(object)label2);
		((Control)groupBox1).Controls.Add((Control)(object)label1);
		((Control)groupBox1).Font = new Font("Times New Roman", 12f);
		((Control)groupBox1).Location = new Point(13, 52);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(341, 395);
		((Control)groupBox1).TabIndex = 159;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Time Zone";
		((Control)cmdReadTimeZone).Location = new Point(64, 351);
		((Control)cmdReadTimeZone).Name = "cmdReadTimeZone";
		((Control)cmdReadTimeZone).Size = new Size(119, 31);
		((Control)cmdReadTimeZone).TabIndex = 3;
		((Control)cmdReadTimeZone).Text = "Read";
		((ButtonBase)cmdReadTimeZone).UseVisualStyleBackColor = true;
		((Control)cmdReadTimeZone).Click += cmdReadTimeZone_Click;
		((Control)cmdWriteTimeZone).Location = new Point(202, 351);
		((Control)cmdWriteTimeZone).Name = "cmdWriteTimeZone";
		((Control)cmdWriteTimeZone).Size = new Size(119, 31);
		((Control)cmdWriteTimeZone).TabIndex = 3;
		((Control)cmdWriteTimeZone).Text = "Write";
		((ButtonBase)cmdWriteTimeZone).UseVisualStyleBackColor = true;
		((Control)cmdWriteTimeZone).Click += cmdWriteTimeZone_Click;
		((Control)txtEndMinute6).Location = new Point(285, 305);
		((Control)txtEndMinute6).Name = "txtEndMinute6";
		((Control)txtEndMinute6).Size = new Size(37, 26);
		((Control)txtEndMinute6).TabIndex = 2;
		((Control)txtEndHour1).Location = new Point(226, 126);
		((Control)txtEndHour1).Name = "txtEndHour1";
		((Control)txtEndHour1).Size = new Size(37, 26);
		((Control)txtEndHour1).TabIndex = 2;
		((Control)txtEndMinute5).Location = new Point(285, 269);
		((Control)txtEndMinute5).Name = "txtEndMinute5";
		((Control)txtEndMinute5).Size = new Size(37, 26);
		((Control)txtEndMinute5).TabIndex = 2;
		((Control)txtEndMinute4).Location = new Point(285, 233);
		((Control)txtEndMinute4).Name = "txtEndMinute4";
		((Control)txtEndMinute4).Size = new Size(37, 26);
		((Control)txtEndMinute4).TabIndex = 2;
		((Control)txtEndMinute3).Location = new Point(285, 197);
		((Control)txtEndMinute3).Name = "txtEndMinute3";
		((Control)txtEndMinute3).Size = new Size(37, 26);
		((Control)txtEndMinute3).TabIndex = 2;
		((Control)txtEndMinute2).Location = new Point(285, 161);
		((Control)txtEndMinute2).Name = "txtEndMinute2";
		((Control)txtEndMinute2).Size = new Size(37, 26);
		((Control)txtEndMinute2).TabIndex = 2;
		((Control)txtEndMinute1).Location = new Point(285, 126);
		((Control)txtEndMinute1).Name = "txtEndMinute1";
		((Control)txtEndMinute1).Size = new Size(37, 26);
		((Control)txtEndMinute1).TabIndex = 2;
		((Control)txtStartMinute6).Location = new Point(146, 305);
		((Control)txtStartMinute6).Name = "txtStartMinute6";
		((Control)txtStartMinute6).Size = new Size(37, 26);
		((Control)txtStartMinute6).TabIndex = 2;
		((Control)txtStartMinute5).Location = new Point(146, 269);
		((Control)txtStartMinute5).Name = "txtStartMinute5";
		((Control)txtStartMinute5).Size = new Size(37, 26);
		((Control)txtStartMinute5).TabIndex = 2;
		((Control)txtStartMinute4).Location = new Point(146, 233);
		((Control)txtStartMinute4).Name = "txtStartMinute4";
		((Control)txtStartMinute4).Size = new Size(37, 26);
		((Control)txtStartMinute4).TabIndex = 2;
		((Control)txtStartMinute3).Location = new Point(146, 197);
		((Control)txtStartMinute3).Name = "txtStartMinute3";
		((Control)txtStartMinute3).Size = new Size(37, 26);
		((Control)txtStartMinute3).TabIndex = 2;
		((Control)txtStartMinute2).Location = new Point(146, 161);
		((Control)txtStartMinute2).Name = "txtStartMinute2";
		((Control)txtStartMinute2).Size = new Size(37, 26);
		((Control)txtStartMinute2).TabIndex = 2;
		((Control)txtStartMinute1).Location = new Point(146, 126);
		((Control)txtStartMinute1).Name = "txtStartMinute1";
		((Control)txtStartMinute1).Size = new Size(37, 26);
		((Control)txtStartMinute1).TabIndex = 2;
		((Control)txtEndHour6).Location = new Point(226, 305);
		((Control)txtEndHour6).Name = "txtEndHour6";
		((Control)txtEndHour6).Size = new Size(37, 26);
		((Control)txtEndHour6).TabIndex = 2;
		((Control)txtEndHour5).Location = new Point(226, 269);
		((Control)txtEndHour5).Name = "txtEndHour5";
		((Control)txtEndHour5).Size = new Size(37, 26);
		((Control)txtEndHour5).TabIndex = 2;
		((Control)txtEndHour4).Location = new Point(226, 233);
		((Control)txtEndHour4).Name = "txtEndHour4";
		((Control)txtEndHour4).Size = new Size(37, 26);
		((Control)txtEndHour4).TabIndex = 2;
		((Control)txtEndHour3).Location = new Point(226, 197);
		((Control)txtEndHour3).Name = "txtEndHour3";
		((Control)txtEndHour3).Size = new Size(37, 26);
		((Control)txtEndHour3).TabIndex = 2;
		((Control)txtEndHour2).Location = new Point(226, 161);
		((Control)txtEndHour2).Name = "txtEndHour2";
		((Control)txtEndHour2).Size = new Size(37, 26);
		((Control)txtEndHour2).TabIndex = 2;
		((Control)txtStartHour6).Location = new Point(87, 305);
		((Control)txtStartHour6).Name = "txtStartHour6";
		((Control)txtStartHour6).Size = new Size(37, 26);
		((Control)txtStartHour6).TabIndex = 2;
		((Control)txtStartHour5).Location = new Point(87, 269);
		((Control)txtStartHour5).Name = "txtStartHour5";
		((Control)txtStartHour5).Size = new Size(37, 26);
		((Control)txtStartHour5).TabIndex = 2;
		((Control)txtStartHour4).Location = new Point(87, 233);
		((Control)txtStartHour4).Name = "txtStartHour4";
		((Control)txtStartHour4).Size = new Size(37, 26);
		((Control)txtStartHour4).TabIndex = 2;
		((Control)txtStartHour3).Location = new Point(87, 197);
		((Control)txtStartHour3).Name = "txtStartHour3";
		((Control)txtStartHour3).Size = new Size(37, 26);
		((Control)txtStartHour3).TabIndex = 2;
		((Control)txtStartHour2).Location = new Point(87, 161);
		((Control)txtStartHour2).Name = "txtStartHour2";
		((Control)txtStartHour2).Size = new Size(37, 26);
		((Control)txtStartHour2).TabIndex = 2;
		((Control)txtStartHour1).Location = new Point(87, 126);
		((Control)txtStartHour1).Name = "txtStartHour1";
		((Control)txtStartHour1).Size = new Size(37, 26);
		((Control)txtStartHour1).TabIndex = 2;
		((ListControl)cmbTZN).FormattingEnabled = true;
		((Control)cmbTZN).Location = new Point(168, 41);
		((Control)cmbTZN).Name = "cmbTZN";
		((Control)cmbTZN).Size = new Size(121, 27);
		((Control)cmbTZN).TabIndex = 1;
		((Control)label7).AutoSize = true;
		((Control)label7).Location = new Point(16, 308);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(46, 19);
		((Control)label7).TabIndex = 0;
		((Control)label7).Text = "TZ 6 :";
		((Control)label27).AutoSize = true;
		((Control)label27).Location = new Point(269, 307);
		((Control)label27).Name = "label27";
		((Control)label27).Size = new Size(12, 19);
		((Control)label27).TabIndex = 0;
		((Control)label27).Text = ":";
		((Control)label6).AutoSize = true;
		((Control)label6).Location = new Point(16, 272);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(46, 19);
		((Control)label6).TabIndex = 0;
		((Control)label6).Text = "TZ 5 :";
		((Control)label24).AutoSize = true;
		((Control)label24).Location = new Point(269, 271);
		((Control)label24).Name = "label24";
		((Control)label24).Size = new Size(12, 19);
		((Control)label24).TabIndex = 0;
		((Control)label24).Text = ":";
		((Control)label5).AutoSize = true;
		((Control)label5).Location = new Point(16, 236);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(46, 19);
		((Control)label5).TabIndex = 0;
		((Control)label5).Text = "TZ 4 :";
		((Control)label21).AutoSize = true;
		((Control)label21).Location = new Point(269, 235);
		((Control)label21).Name = "label21";
		((Control)label21).Size = new Size(12, 19);
		((Control)label21).TabIndex = 0;
		((Control)label21).Text = ":";
		((Control)label4).AutoSize = true;
		((Control)label4).Location = new Point(16, 200);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(46, 19);
		((Control)label4).TabIndex = 0;
		((Control)label4).Text = "TZ 3 :";
		((Control)label26).AutoSize = true;
		((Control)label26).Location = new Point(198, 308);
		((Control)label26).Name = "label26";
		((Control)label26).Size = new Size(18, 19);
		((Control)label26).TabIndex = 0;
		((Control)label26).Text = "~";
		((Control)label18).AutoSize = true;
		((Control)label18).Location = new Point(269, 199);
		((Control)label18).Name = "label18";
		((Control)label18).Size = new Size(12, 19);
		((Control)label18).TabIndex = 0;
		((Control)label18).Text = ":";
		((Control)label23).AutoSize = true;
		((Control)label23).Location = new Point(198, 272);
		((Control)label23).Name = "label23";
		((Control)label23).Size = new Size(18, 19);
		((Control)label23).TabIndex = 0;
		((Control)label23).Text = "~";
		((Control)label3).AutoSize = true;
		((Control)label3).Location = new Point(16, 164);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(46, 19);
		((Control)label3).TabIndex = 0;
		((Control)label3).Text = "TZ 2 :";
		((Control)label20).AutoSize = true;
		((Control)label20).Location = new Point(198, 236);
		((Control)label20).Name = "label20";
		((Control)label20).Size = new Size(18, 19);
		((Control)label20).TabIndex = 0;
		((Control)label20).Text = "~";
		((Control)label15).AutoSize = true;
		((Control)label15).Location = new Point(269, 163);
		((Control)label15).Name = "label15";
		((Control)label15).Size = new Size(12, 19);
		((Control)label15).TabIndex = 0;
		((Control)label15).Text = ":";
		((Control)label25).AutoSize = true;
		((Control)label25).Location = new Point(130, 307);
		((Control)label25).Name = "label25";
		((Control)label25).Size = new Size(12, 19);
		((Control)label25).TabIndex = 0;
		((Control)label25).Text = ":";
		((Control)label17).AutoSize = true;
		((Control)label17).Location = new Point(198, 200);
		((Control)label17).Name = "label17";
		((Control)label17).Size = new Size(18, 19);
		((Control)label17).TabIndex = 0;
		((Control)label17).Text = "~";
		((Control)label22).AutoSize = true;
		((Control)label22).Location = new Point(130, 271);
		((Control)label22).Name = "label22";
		((Control)label22).Size = new Size(12, 19);
		((Control)label22).TabIndex = 0;
		((Control)label22).Text = ":";
		((Control)label9).AutoSize = true;
		((Control)label9).Location = new Point(243, 94);
		((Control)label9).Name = "label9";
		((Control)label9).Size = new Size(66, 19);
		((Control)label9).TabIndex = 0;
		((Control)label9).Text = "End Time";
		((Control)label19).AutoSize = true;
		((Control)label19).Location = new Point(130, 235);
		((Control)label19).Name = "label19";
		((Control)label19).Size = new Size(12, 19);
		((Control)label19).TabIndex = 0;
		((Control)label19).Text = ":";
		((Control)label14).AutoSize = true;
		((Control)label14).Location = new Point(198, 164);
		((Control)label14).Name = "label14";
		((Control)label14).Size = new Size(18, 19);
		((Control)label14).TabIndex = 0;
		((Control)label14).Text = "~";
		((Control)label16).AutoSize = true;
		((Control)label16).Location = new Point(130, 199);
		((Control)label16).Name = "label16";
		((Control)label16).Size = new Size(12, 19);
		((Control)label16).TabIndex = 0;
		((Control)label16).Text = ":";
		((Control)label12).AutoSize = true;
		((Control)label12).Location = new Point(269, 128);
		((Control)label12).Name = "label12";
		((Control)label12).Size = new Size(12, 19);
		((Control)label12).TabIndex = 0;
		((Control)label12).Text = ":";
		((Control)label13).AutoSize = true;
		((Control)label13).Location = new Point(130, 163);
		((Control)label13).Name = "label13";
		((Control)label13).Size = new Size(12, 19);
		((Control)label13).TabIndex = 0;
		((Control)label13).Text = ":";
		((Control)label11).AutoSize = true;
		((Control)label11).Location = new Point(198, 129);
		((Control)label11).Name = "label11";
		((Control)label11).Size = new Size(18, 19);
		((Control)label11).TabIndex = 0;
		((Control)label11).Text = "~";
		((Control)label10).AutoSize = true;
		((Control)label10).Location = new Point(130, 128);
		((Control)label10).Name = "label10";
		((Control)label10).Size = new Size(12, 19);
		((Control)label10).TabIndex = 0;
		((Control)label10).Text = ":";
		((Control)label8).AutoSize = true;
		((Control)label8).Location = new Point(101, 94);
		((Control)label8).Name = "label8";
		((Control)label8).Size = new Size(71, 19);
		((Control)label8).TabIndex = 0;
		((Control)label8).Text = "Start Time";
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(16, 128);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(46, 19);
		((Control)label2).TabIndex = 0;
		((Control)label2).Text = "TZ 1 :";
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(40, 44);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(100, 19);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "TimeZone No :";
		((Control)groupBox2).Controls.Add((Control)(object)cmdWriteUserWeekPassTime);
		((Control)groupBox2).Controls.Add((Control)(object)cmdReadUserWeekPassTime);
		((Control)groupBox2).Controls.Add((Control)(object)txtTimeZone7);
		((Control)groupBox2).Controls.Add((Control)(object)txtTimeZone6);
		((Control)groupBox2).Controls.Add((Control)(object)txtTimeZone5);
		((Control)groupBox2).Controls.Add((Control)(object)txtTimeZone4);
		((Control)groupBox2).Controls.Add((Control)(object)txtTimeZone3);
		((Control)groupBox2).Controls.Add((Control)(object)txtTimeZone2);
		((Control)groupBox2).Controls.Add((Control)(object)txtTimeZone1);
		((Control)groupBox2).Controls.Add((Control)(object)txtEnrollNumber);
		((Control)groupBox2).Controls.Add((Control)(object)label30);
		((Control)groupBox2).Controls.Add((Control)(object)label37);
		((Control)groupBox2).Controls.Add((Control)(object)label36);
		((Control)groupBox2).Controls.Add((Control)(object)label35);
		((Control)groupBox2).Controls.Add((Control)(object)label34);
		((Control)groupBox2).Controls.Add((Control)(object)label33);
		((Control)groupBox2).Controls.Add((Control)(object)label32);
		((Control)groupBox2).Controls.Add((Control)(object)label31);
		((Control)groupBox2).Controls.Add((Control)(object)label29);
		((Control)groupBox2).Controls.Add((Control)(object)label28);
		((Control)groupBox2).Font = new Font("Times New Roman", 12f);
		((Control)groupBox2).Location = new Point(380, 61);
		((Control)groupBox2).Name = "groupBox2";
		((Control)groupBox2).Size = new Size(250, 383);
		((Control)groupBox2).TabIndex = 160;
		groupBox2.TabStop = false;
		((Control)groupBox2).Text = "User Time Zone";
		((Control)cmdWriteUserWeekPassTime).Location = new Point(137, 342);
		((Control)cmdWriteUserWeekPassTime).Name = "cmdWriteUserWeekPassTime";
		((Control)cmdWriteUserWeekPassTime).Size = new Size(95, 31);
		((Control)cmdWriteUserWeekPassTime).TabIndex = 2;
		((Control)cmdWriteUserWeekPassTime).Text = "Write";
		((ButtonBase)cmdWriteUserWeekPassTime).UseVisualStyleBackColor = true;
		((Control)cmdWriteUserWeekPassTime).Click += cmdWriteUserWeekPassTime_Click;
		((Control)cmdReadUserWeekPassTime).Location = new Point(32, 342);
		((Control)cmdReadUserWeekPassTime).Name = "cmdReadUserWeekPassTime";
		((Control)cmdReadUserWeekPassTime).Size = new Size(95, 31);
		((Control)cmdReadUserWeekPassTime).TabIndex = 2;
		((Control)cmdReadUserWeekPassTime).Text = "Read";
		((ButtonBase)cmdReadUserWeekPassTime).UseVisualStyleBackColor = true;
		((Control)cmdReadUserWeekPassTime).Click += cmdReadUserWeekPassTime_Click;
		((Control)txtTimeZone7).Location = new Point(132, 297);
		((Control)txtTimeZone7).Name = "txtTimeZone7";
		((Control)txtTimeZone7).Size = new Size(100, 26);
		((Control)txtTimeZone7).TabIndex = 1;
		((Control)txtTimeZone6).Location = new Point(132, 267);
		((Control)txtTimeZone6).Name = "txtTimeZone6";
		((Control)txtTimeZone6).Size = new Size(100, 26);
		((Control)txtTimeZone6).TabIndex = 1;
		((Control)txtTimeZone5).Location = new Point(132, 237);
		((Control)txtTimeZone5).Name = "txtTimeZone5";
		((Control)txtTimeZone5).Size = new Size(100, 26);
		((Control)txtTimeZone5).TabIndex = 1;
		((Control)txtTimeZone4).Location = new Point(132, 207);
		((Control)txtTimeZone4).Name = "txtTimeZone4";
		((Control)txtTimeZone4).Size = new Size(100, 26);
		((Control)txtTimeZone4).TabIndex = 1;
		((Control)txtTimeZone3).Location = new Point(132, 177);
		((Control)txtTimeZone3).Name = "txtTimeZone3";
		((Control)txtTimeZone3).Size = new Size(100, 26);
		((Control)txtTimeZone3).TabIndex = 1;
		((Control)txtTimeZone2).Location = new Point(132, 147);
		((Control)txtTimeZone2).Name = "txtTimeZone2";
		((Control)txtTimeZone2).Size = new Size(100, 26);
		((Control)txtTimeZone2).TabIndex = 1;
		((Control)txtTimeZone1).Location = new Point(132, 117);
		((Control)txtTimeZone1).Name = "txtTimeZone1";
		((Control)txtTimeZone1).Size = new Size(100, 26);
		((Control)txtTimeZone1).TabIndex = 1;
		((Control)txtEnrollNumber).Location = new Point(127, 38);
		((Control)txtEnrollNumber).Name = "txtEnrollNumber";
		((Control)txtEnrollNumber).Size = new Size(100, 26);
		((Control)txtEnrollNumber).TabIndex = 1;
		((Control)label30).AutoSize = true;
		((Control)label30).Location = new Point(139, 85);
		((Control)label30).Name = "label30";
		((Control)label30).Size = new Size(93, 19);
		((Control)label30).TabIndex = 0;
		((Control)label30).Text = "TimeZone No";
		((Control)label37).AutoSize = true;
		((Control)label37).Location = new Point(15, 305);
		((Control)label37).Name = "label37";
		((Control)label37).Size = new Size(74, 19);
		((Control)label37).TabIndex = 0;
		((Control)label37).Text = "Saturday : ";
		((Control)label36).AutoSize = true;
		((Control)label36).Location = new Point(15, 274);
		((Control)label36).Name = "label36";
		((Control)label36).Size = new Size(55, 19);
		((Control)label36).TabIndex = 0;
		((Control)label36).Text = "Friday :";
		((Control)label35).AutoSize = true;
		((Control)label35).Location = new Point(15, 243);
		((Control)label35).Name = "label35";
		((Control)label35).Size = new Size(76, 19);
		((Control)label35).TabIndex = 0;
		((Control)label35).Text = "Thursday : ";
		((Control)label34).AutoSize = true;
		((Control)label34).Location = new Point(15, 212);
		((Control)label34).Name = "label34";
		((Control)label34).Size = new Size(87, 19);
		((Control)label34).TabIndex = 0;
		((Control)label34).Text = "Wednesday :";
		((Control)label33).AutoSize = true;
		((Control)label33).Location = new Point(15, 181);
		((Control)label33).Name = "label33";
		((Control)label33).Size = new Size(66, 19);
		((Control)label33).TabIndex = 0;
		((Control)label33).Text = "Tuesday :";
		((Control)label32).AutoSize = true;
		((Control)label32).Location = new Point(15, 150);
		((Control)label32).Name = "label32";
		((Control)label32).Size = new Size(67, 19);
		((Control)label32).TabIndex = 0;
		((Control)label32).Text = "Monday :";
		((Control)label31).AutoSize = true;
		((Control)label31).Location = new Point(15, 119);
		((Control)label31).Name = "label31";
		((Control)label31).Size = new Size(61, 19);
		((Control)label31).TabIndex = 0;
		((Control)label31).Text = "Sunday :";
		((Control)label29).AutoSize = true;
		((Control)label29).Location = new Point(15, 85);
		((Control)label29).Name = "label29";
		((Control)label29).Size = new Size(73, 19);
		((Control)label29).TabIndex = 0;
		((Control)label29).Text = "Weekdays";
		((Control)label28).AutoSize = true;
		((Control)label28).Location = new Point(15, 40);
		((Control)label28).Name = "label28";
		((Control)label28).Size = new Size(105, 19);
		((Control)label28).TabIndex = 0;
		((Control)label28).Text = "Enroll Number :";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(650, 457);
		((Control)this).Controls.Add((Control)(object)groupBox2);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmPassTimeHS";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "PassTime Info (HS)";
		((Form)this).Load += frmPassTimeHS_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)groupBox2).ResumeLayout(false);
		((Control)groupBox2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
