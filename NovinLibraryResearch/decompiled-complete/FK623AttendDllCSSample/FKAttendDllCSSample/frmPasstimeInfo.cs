using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace FKAttendDllCSSample;

public class frmPasstimeInfo : Form
{
	public GroupBox Frame4;

	public TextBox txtGroupMatch9;

	public TextBox txtGroupMatch8;

	public TextBox txtGroupMatch7;

	public TextBox txtGroupMatch6;

	public TextBox txtGroupMatch5;

	public TextBox txtGroupMatch4;

	public TextBox txtGroupMatch3;

	public TextBox txtGroupMatch2;

	public TextBox txtGroupMatch1;

	public TextBox txtGroupMatch0;

	public Button cmdSetGroupMatch;

	public Button cmdGetGroupMatch;

	public Label _lblLabel_10;

	public Label lblLabel9;

	public ComboBox cmbDoorState;

	public GroupBox Frame2;

	public TextBox txtUserPassTime2;

	public TextBox txtUserPassTime1;

	public TextBox txtUserPassTime0;

	public TextBox txtUserID;

	public TextBox txtGroupID;

	public Button cmdGetUserPasstime;

	public Button cmdSetUserPassTime;

	public Label lblLabel3;

	public Label lblLabel2;

	public Label lblLabel1;

	public Label lblLabel0;

	public Label lblUserID;

	public Button cmdSetDoorState;

	public GroupBox Frame1;

	public Button cmdWrite;

	public Button cmdRead;

	public TextBox txtPassTimeID;

	public Label lblLabel8;

	public Label lblWeek0;

	public Label lblWeek6;

	public Label lblWeek5;

	public Label lblWeek4;

	public Label lblWeek3;

	public Label lblWeek2;

	public Label lblWeek1;

	public Label lblMidSep6;

	public TextBox txtEndHour0;

	public TextBox txtEndMinute0;

	public TextBox txtStartMinute0;

	public TextBox txtStartHour0;

	public TextBox txtEndHour4;

	public TextBox txtEndMinute4;

	public TextBox txtEndMinute3;

	public TextBox txtEndHour3;

	public TextBox txtEndHour2;

	public TextBox txtEndMinute2;

	public TextBox txtEndMinute1;

	public TextBox txtEndHour1;

	public TextBox txtStartMinute4;

	public TextBox txtStartHour4;

	public TextBox txtStartHour3;

	public TextBox txtStartMinute3;

	public TextBox txtStartMinute2;

	public TextBox txtStartHour2;

	public TextBox txtStartHour1;

	public TextBox txtStartMinute1;

	public TextBox txtStartHour5;

	public TextBox txtStartHour6;

	public TextBox txtStartMinute5;

	public TextBox txtStartMinute6;

	public TextBox txtEndHour5;

	public TextBox txtEndHour6;

	public TextBox txtEndMinute5;

	public TextBox txtEndMinute6;

	public Label lblStartSep6;

	public Label lblEndSep6;

	public Label lblEndSep5;

	public Label lblEndSep4;

	public Label lblEndSep3;

	public Label lblEndSep2;

	public Label lblEndSep1;

	public Label lblEndSep0;

	public Label lblStartSep5;

	public Label lblStartSep4;

	public Label lblStartSep3;

	public Label lblStartSep2;

	public Label lblStartSep1;

	public Label lblMidSep5;

	public Label lblMidSep4;

	public Label lblMidSep0;

	public Label lblStartSep0;

	public Label lblMidSep3;

	public Label lblMidSep2;

	public Label lblMidSep1;

	public Label lblStartTime;

	public Label lblEndTime;

	public Button cmdGetDoorState;

	public Label lblMessage;

	public GroupBox Frame3;

	public TextBox txtGroupNum;

	public TextBox txtGroupPassTime2;

	public TextBox txtGroupPassTime1;

	public TextBox txtGroupPassTime0;

	public Button cmdGetGroupPassTime;

	public Button cmdSetGroupPassTime;

	public Label _lblLabel_7;

	public Label lblLabel6;

	public Label lblLabel5;

	public Label lblLabel4;

	private Container components = null;

	private PASSCTRLTIME mPassTimeInfo;

	private USERPASSINFO mUserPassInfo;

	private GROUPPASSINFO mGroupPassInfo;

	private GROUPMATCHINFO mGroupMatchInfo;

	private TextBox[] mtxtStartHour = (TextBox[])(object)new TextBox[7];

	private TextBox[] mtxtStartMinute = (TextBox[])(object)new TextBox[7];

	private TextBox[] mtxtEndHour = (TextBox[])(object)new TextBox[7];

	private TextBox[] mtxtEndMinute = (TextBox[])(object)new TextBox[7];

	private TextBox[] mtxtUserPassTime = (TextBox[])(object)new TextBox[3];

	private TextBox[] mtxtGroupPassTime = (TextBox[])(object)new TextBox[3];

	private TextBox[] mtxtGroupMatch = (TextBox[])(object)new TextBox[10];

	private int[] vValue = new int[28];

	public frmPasstimeInfo()
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
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f6: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Expected O, but got Unknown
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b7: Expected O, but got Unknown
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a96: Expected O, but got Unknown
		//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c57: Expected O, but got Unknown
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_0e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e18: Expected O, but got Unknown
		//IL_0eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef7: Expected O, but got Unknown
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc9: Expected O, but got Unknown
		//IL_10a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Expected O, but got Unknown
		//IL_1182: Unknown result type (might be due to invalid IL or missing references)
		//IL_118c: Expected O, but got Unknown
		//IL_123d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1247: Expected O, but got Unknown
		//IL_1305: Unknown result type (might be due to invalid IL or missing references)
		//IL_130f: Expected O, but got Unknown
		//IL_14ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f6: Expected O, but got Unknown
		//IL_15c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d0: Expected O, but got Unknown
		//IL_16a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b2: Expected O, but got Unknown
		//IL_178a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1794: Expected O, but got Unknown
		//IL_186c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1876: Expected O, but got Unknown
		//IL_194e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1958: Expected O, but got Unknown
		//IL_1a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1d: Expected O, but got Unknown
		//IL_1af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afa: Expected O, but got Unknown
		//IL_1bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bda: Expected O, but got Unknown
		//IL_1c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca2: Expected O, but got Unknown
		//IL_1d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6a: Expected O, but got Unknown
		//IL_1e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2f: Expected O, but got Unknown
		//IL_1edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee6: Expected O, but got Unknown
		//IL_1f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9e: Expected O, but got Unknown
		//IL_25f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2602: Expected O, but got Unknown
		//IL_26c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cd: Expected O, but got Unknown
		//IL_27a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b0: Expected O, but got Unknown
		//IL_2896: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a0: Expected O, but got Unknown
		//IL_296b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2975: Expected O, but got Unknown
		//IL_2a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2f: Expected O, but got Unknown
		//IL_2add: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae7: Expected O, but got Unknown
		//IL_2b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba2: Expected O, but got Unknown
		//IL_2c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5d: Expected O, but got Unknown
		//IL_2d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d18: Expected O, but got Unknown
		//IL_2dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd3: Expected O, but got Unknown
		//IL_2e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8e: Expected O, but got Unknown
		//IL_2f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f53: Expected O, but got Unknown
		//IL_3014: Unknown result type (might be due to invalid IL or missing references)
		//IL_301e: Expected O, but got Unknown
		//IL_30f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3100: Expected O, but got Unknown
		//IL_31d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e2: Expected O, but got Unknown
		//IL_32ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c4: Expected O, but got Unknown
		//IL_3399: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a3: Expected O, but got Unknown
		//IL_347e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3488: Expected O, but got Unknown
		//IL_3563: Unknown result type (might be due to invalid IL or missing references)
		//IL_356d: Expected O, but got Unknown
		//IL_3648: Unknown result type (might be due to invalid IL or missing references)
		//IL_3652: Expected O, but got Unknown
		//IL_372d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3737: Expected O, but got Unknown
		//IL_3812: Unknown result type (might be due to invalid IL or missing references)
		//IL_381c: Expected O, but got Unknown
		//IL_38f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3901: Expected O, but got Unknown
		//IL_39d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_39e3: Expected O, but got Unknown
		//IL_3abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac5: Expected O, but got Unknown
		//IL_3ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3baa: Expected O, but got Unknown
		//IL_3c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c8c: Expected O, but got Unknown
		//IL_3d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6e: Expected O, but got Unknown
		//IL_3e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e53: Expected O, but got Unknown
		//IL_3f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f38: Expected O, but got Unknown
		//IL_4010: Unknown result type (might be due to invalid IL or missing references)
		//IL_401a: Expected O, but got Unknown
		//IL_40ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f9: Expected O, but got Unknown
		//IL_41d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_41db: Expected O, but got Unknown
		//IL_42b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_42bc: Expected O, but got Unknown
		//IL_4393: Unknown result type (might be due to invalid IL or missing references)
		//IL_439d: Expected O, but got Unknown
		//IL_4477: Unknown result type (might be due to invalid IL or missing references)
		//IL_4481: Expected O, but got Unknown
		//IL_455b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4565: Expected O, but got Unknown
		//IL_463f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4649: Expected O, but got Unknown
		//IL_4723: Unknown result type (might be due to invalid IL or missing references)
		//IL_472d: Expected O, but got Unknown
		//IL_4807: Unknown result type (might be due to invalid IL or missing references)
		//IL_4811: Expected O, but got Unknown
		//IL_48eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_48f5: Expected O, but got Unknown
		//IL_49b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_49bd: Expected O, but got Unknown
		//IL_4a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a88: Expected O, but got Unknown
		//IL_4b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b53: Expected O, but got Unknown
		//IL_4c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c1e: Expected O, but got Unknown
		//IL_4cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce9: Expected O, but got Unknown
		//IL_4daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4db4: Expected O, but got Unknown
		//IL_4e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e7c: Expected O, but got Unknown
		//IL_4f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f44: Expected O, but got Unknown
		//IL_5002: Unknown result type (might be due to invalid IL or missing references)
		//IL_500c: Expected O, but got Unknown
		//IL_50ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d4: Expected O, but got Unknown
		//IL_5192: Unknown result type (might be due to invalid IL or missing references)
		//IL_519c: Expected O, but got Unknown
		//IL_525a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5264: Expected O, but got Unknown
		//IL_531f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5329: Expected O, but got Unknown
		//IL_53ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_53f4: Expected O, but got Unknown
		//IL_54b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_54bf: Expected O, but got Unknown
		//IL_557d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5587: Expected O, but got Unknown
		//IL_5642: Unknown result type (might be due to invalid IL or missing references)
		//IL_564c: Expected O, but got Unknown
		//IL_570d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5717: Expected O, but got Unknown
		//IL_57d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_57e2: Expected O, but got Unknown
		//IL_58a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_58aa: Expected O, but got Unknown
		//IL_5965: Unknown result type (might be due to invalid IL or missing references)
		//IL_596f: Expected O, but got Unknown
		//IL_5a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a2a: Expected O, but got Unknown
		//IL_5b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b1a: Expected O, but got Unknown
		//IL_5cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cb7: Expected O, but got Unknown
		//IL_5d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d95: Expected O, but got Unknown
		//IL_5e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e77: Expected O, but got Unknown
		//IL_5f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f59: Expected O, but got Unknown
		//IL_6031: Unknown result type (might be due to invalid IL or missing references)
		//IL_603b: Expected O, but got Unknown
		//IL_6103: Unknown result type (might be due to invalid IL or missing references)
		//IL_610d: Expected O, but got Unknown
		//IL_61e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ea: Expected O, but got Unknown
		//IL_62c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ca: Expected O, but got Unknown
		//IL_6378: Unknown result type (might be due to invalid IL or missing references)
		//IL_6382: Expected O, but got Unknown
		//IL_6440: Unknown result type (might be due to invalid IL or missing references)
		//IL_644a: Expected O, but got Unknown
		//IL_6505: Unknown result type (might be due to invalid IL or missing references)
		//IL_650f: Expected O, but got Unknown
		Frame4 = new GroupBox();
		txtGroupMatch9 = new TextBox();
		txtGroupMatch8 = new TextBox();
		txtGroupMatch7 = new TextBox();
		txtGroupMatch6 = new TextBox();
		txtGroupMatch5 = new TextBox();
		txtGroupMatch4 = new TextBox();
		txtGroupMatch3 = new TextBox();
		txtGroupMatch2 = new TextBox();
		txtGroupMatch1 = new TextBox();
		txtGroupMatch0 = new TextBox();
		cmdSetGroupMatch = new Button();
		cmdGetGroupMatch = new Button();
		_lblLabel_10 = new Label();
		lblLabel9 = new Label();
		cmbDoorState = new ComboBox();
		Frame2 = new GroupBox();
		txtUserPassTime2 = new TextBox();
		txtUserPassTime1 = new TextBox();
		txtUserPassTime0 = new TextBox();
		txtUserID = new TextBox();
		txtGroupID = new TextBox();
		cmdGetUserPasstime = new Button();
		cmdSetUserPassTime = new Button();
		lblLabel3 = new Label();
		lblLabel2 = new Label();
		lblLabel1 = new Label();
		lblLabel0 = new Label();
		lblUserID = new Label();
		cmdSetDoorState = new Button();
		Frame1 = new GroupBox();
		cmdWrite = new Button();
		cmdRead = new Button();
		txtPassTimeID = new TextBox();
		lblLabel8 = new Label();
		lblWeek0 = new Label();
		lblWeek6 = new Label();
		lblWeek5 = new Label();
		lblWeek4 = new Label();
		lblWeek3 = new Label();
		lblWeek2 = new Label();
		lblWeek1 = new Label();
		lblMidSep6 = new Label();
		txtEndHour0 = new TextBox();
		txtEndMinute0 = new TextBox();
		txtStartMinute0 = new TextBox();
		txtStartHour0 = new TextBox();
		txtEndHour4 = new TextBox();
		txtEndMinute4 = new TextBox();
		txtEndMinute3 = new TextBox();
		txtEndHour3 = new TextBox();
		txtEndHour2 = new TextBox();
		txtEndMinute2 = new TextBox();
		txtEndMinute1 = new TextBox();
		txtEndHour1 = new TextBox();
		txtStartMinute4 = new TextBox();
		txtStartHour4 = new TextBox();
		txtStartHour3 = new TextBox();
		txtStartMinute3 = new TextBox();
		txtStartMinute2 = new TextBox();
		txtStartHour2 = new TextBox();
		txtStartHour1 = new TextBox();
		txtStartMinute1 = new TextBox();
		txtStartHour5 = new TextBox();
		txtStartHour6 = new TextBox();
		txtStartMinute5 = new TextBox();
		txtStartMinute6 = new TextBox();
		txtEndHour5 = new TextBox();
		txtEndHour6 = new TextBox();
		txtEndMinute5 = new TextBox();
		txtEndMinute6 = new TextBox();
		lblStartSep6 = new Label();
		lblEndSep6 = new Label();
		lblEndSep5 = new Label();
		lblEndSep4 = new Label();
		lblEndSep3 = new Label();
		lblEndSep2 = new Label();
		lblEndSep1 = new Label();
		lblEndSep0 = new Label();
		lblStartSep5 = new Label();
		lblStartSep4 = new Label();
		lblStartSep3 = new Label();
		lblStartSep2 = new Label();
		lblStartSep1 = new Label();
		lblMidSep5 = new Label();
		lblMidSep4 = new Label();
		lblMidSep0 = new Label();
		lblStartSep0 = new Label();
		lblMidSep3 = new Label();
		lblMidSep2 = new Label();
		lblMidSep1 = new Label();
		lblStartTime = new Label();
		lblEndTime = new Label();
		cmdGetDoorState = new Button();
		lblMessage = new Label();
		Frame3 = new GroupBox();
		txtGroupNum = new TextBox();
		txtGroupPassTime2 = new TextBox();
		txtGroupPassTime1 = new TextBox();
		txtGroupPassTime0 = new TextBox();
		cmdGetGroupPassTime = new Button();
		cmdSetGroupPassTime = new Button();
		_lblLabel_7 = new Label();
		lblLabel6 = new Label();
		lblLabel5 = new Label();
		lblLabel4 = new Label();
		((Control)Frame4).SuspendLayout();
		((Control)Frame2).SuspendLayout();
		((Control)Frame1).SuspendLayout();
		((Control)Frame3).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)Frame4).BackColor = SystemColors.Control;
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch9);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch8);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch7);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch6);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch5);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch4);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch3);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch2);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch1);
		((Control)Frame4).Controls.Add((Control)(object)txtGroupMatch0);
		((Control)Frame4).Controls.Add((Control)(object)cmdSetGroupMatch);
		((Control)Frame4).Controls.Add((Control)(object)cmdGetGroupMatch);
		((Control)Frame4).Controls.Add((Control)(object)_lblLabel_10);
		((Control)Frame4).Controls.Add((Control)(object)lblLabel9);
		((Control)Frame4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Frame4).ForeColor = SystemColors.ControlText;
		((Control)Frame4).Location = new Point(414, 304);
		((Control)Frame4).Name = "Frame4";
		((Control)Frame4).RightToLeft = (RightToLeft)0;
		((Control)Frame4).Size = new Size(330, 173);
		((Control)Frame4).TabIndex = 117;
		Frame4.TabStop = false;
		((Control)Frame4).Text = "Unlock Group";
		txtGroupMatch9.AcceptsReturn = true;
		((Control)txtGroupMatch9).BackColor = SystemColors.Window;
		((Control)txtGroupMatch9).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch9).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch9).Location = new Point(222, 97);
		((TextBoxBase)txtGroupMatch9).MaxLength = 0;
		((Control)txtGroupMatch9).Name = "txtGroupMatch9";
		((Control)txtGroupMatch9).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch9).Size = new Size(41, 25);
		((Control)txtGroupMatch9).TabIndex = 87;
		((Control)txtGroupMatch9).Text = "0";
		txtGroupMatch9.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch8.AcceptsReturn = true;
		((Control)txtGroupMatch8).BackColor = SystemColors.Window;
		((Control)txtGroupMatch8).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch8).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch8).Location = new Point(173, 97);
		((TextBoxBase)txtGroupMatch8).MaxLength = 0;
		((Control)txtGroupMatch8).Name = "txtGroupMatch8";
		((Control)txtGroupMatch8).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch8).Size = new Size(41, 25);
		((Control)txtGroupMatch8).TabIndex = 86;
		((Control)txtGroupMatch8).Text = "0";
		txtGroupMatch8.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch7.AcceptsReturn = true;
		((Control)txtGroupMatch7).BackColor = SystemColors.Window;
		((Control)txtGroupMatch7).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch7).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch7).Location = new Point(124, 97);
		((TextBoxBase)txtGroupMatch7).MaxLength = 0;
		((Control)txtGroupMatch7).Name = "txtGroupMatch7";
		((Control)txtGroupMatch7).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch7).Size = new Size(41, 25);
		((Control)txtGroupMatch7).TabIndex = 85;
		((Control)txtGroupMatch7).Text = "0";
		txtGroupMatch7.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch6.AcceptsReturn = true;
		((Control)txtGroupMatch6).BackColor = SystemColors.Window;
		((Control)txtGroupMatch6).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch6).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch6).Location = new Point(75, 97);
		((TextBoxBase)txtGroupMatch6).MaxLength = 0;
		((Control)txtGroupMatch6).Name = "txtGroupMatch6";
		((Control)txtGroupMatch6).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch6).Size = new Size(41, 25);
		((Control)txtGroupMatch6).TabIndex = 84;
		((Control)txtGroupMatch6).Text = "0";
		txtGroupMatch6.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch5.AcceptsReturn = true;
		((Control)txtGroupMatch5).BackColor = SystemColors.Window;
		((Control)txtGroupMatch5).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch5).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch5).Location = new Point(26, 97);
		((TextBoxBase)txtGroupMatch5).MaxLength = 0;
		((Control)txtGroupMatch5).Name = "txtGroupMatch5";
		((Control)txtGroupMatch5).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch5).Size = new Size(41, 25);
		((Control)txtGroupMatch5).TabIndex = 83;
		((Control)txtGroupMatch5).Text = "0";
		txtGroupMatch5.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch4.AcceptsReturn = true;
		((Control)txtGroupMatch4).BackColor = SystemColors.Window;
		((Control)txtGroupMatch4).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch4).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch4).Location = new Point(222, 41);
		((TextBoxBase)txtGroupMatch4).MaxLength = 0;
		((Control)txtGroupMatch4).Name = "txtGroupMatch4";
		((Control)txtGroupMatch4).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch4).Size = new Size(41, 25);
		((Control)txtGroupMatch4).TabIndex = 82;
		((Control)txtGroupMatch4).Text = "0";
		txtGroupMatch4.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch3.AcceptsReturn = true;
		((Control)txtGroupMatch3).BackColor = SystemColors.Window;
		((Control)txtGroupMatch3).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch3).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch3).Location = new Point(173, 41);
		((TextBoxBase)txtGroupMatch3).MaxLength = 0;
		((Control)txtGroupMatch3).Name = "txtGroupMatch3";
		((Control)txtGroupMatch3).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch3).Size = new Size(41, 25);
		((Control)txtGroupMatch3).TabIndex = 81;
		((Control)txtGroupMatch3).Text = "0";
		txtGroupMatch3.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch2.AcceptsReturn = true;
		((Control)txtGroupMatch2).BackColor = SystemColors.Window;
		((Control)txtGroupMatch2).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch2).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch2).Location = new Point(124, 41);
		((TextBoxBase)txtGroupMatch2).MaxLength = 0;
		((Control)txtGroupMatch2).Name = "txtGroupMatch2";
		((Control)txtGroupMatch2).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch2).Size = new Size(41, 25);
		((Control)txtGroupMatch2).TabIndex = 80;
		((Control)txtGroupMatch2).Text = "0";
		txtGroupMatch2.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch1.AcceptsReturn = true;
		((Control)txtGroupMatch1).BackColor = SystemColors.Window;
		((Control)txtGroupMatch1).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch1).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch1).Location = new Point(75, 41);
		((TextBoxBase)txtGroupMatch1).MaxLength = 0;
		((Control)txtGroupMatch1).Name = "txtGroupMatch1";
		((Control)txtGroupMatch1).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch1).Size = new Size(41, 25);
		((Control)txtGroupMatch1).TabIndex = 79;
		((Control)txtGroupMatch1).Text = "0";
		txtGroupMatch1.TextAlign = (HorizontalAlignment)2;
		txtGroupMatch0.AcceptsReturn = true;
		((Control)txtGroupMatch0).BackColor = SystemColors.Window;
		((Control)txtGroupMatch0).Cursor = Cursors.IBeam;
		((Control)txtGroupMatch0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupMatch0).ForeColor = SystemColors.WindowText;
		((Control)txtGroupMatch0).Location = new Point(26, 41);
		((TextBoxBase)txtGroupMatch0).MaxLength = 0;
		((Control)txtGroupMatch0).Name = "txtGroupMatch0";
		((Control)txtGroupMatch0).RightToLeft = (RightToLeft)0;
		((Control)txtGroupMatch0).Size = new Size(41, 25);
		((Control)txtGroupMatch0).TabIndex = 78;
		((Control)txtGroupMatch0).Text = "0";
		txtGroupMatch0.TextAlign = (HorizontalAlignment)2;
		((Control)cmdSetGroupMatch).BackColor = SystemColors.Control;
		((Control)cmdSetGroupMatch).Cursor = Cursors.Default;
		((Control)cmdSetGroupMatch).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetGroupMatch).ForeColor = SystemColors.ControlText;
		((Control)cmdSetGroupMatch).Location = new Point(146, 130);
		((Control)cmdSetGroupMatch).Name = "cmdSetGroupMatch";
		((Control)cmdSetGroupMatch).RightToLeft = (RightToLeft)0;
		((Control)cmdSetGroupMatch).Size = new Size(120, 31);
		((Control)cmdSetGroupMatch).TabIndex = 77;
		((Control)cmdSetGroupMatch).Text = "Set Match";
		((ButtonBase)cmdSetGroupMatch).UseVisualStyleBackColor = false;
		((Control)cmdSetGroupMatch).Click += cmdSetGroupMatch_Click;
		((Control)cmdGetGroupMatch).BackColor = SystemColors.Control;
		((Control)cmdGetGroupMatch).Cursor = Cursors.Default;
		((Control)cmdGetGroupMatch).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetGroupMatch).ForeColor = SystemColors.ControlText;
		((Control)cmdGetGroupMatch).Location = new Point(16, 130);
		((Control)cmdGetGroupMatch).Name = "cmdGetGroupMatch";
		((Control)cmdGetGroupMatch).RightToLeft = (RightToLeft)0;
		((Control)cmdGetGroupMatch).Size = new Size(120, 31);
		((Control)cmdGetGroupMatch).TabIndex = 76;
		((Control)cmdGetGroupMatch).Text = "Get Match";
		((ButtonBase)cmdGetGroupMatch).UseVisualStyleBackColor = false;
		((Control)cmdGetGroupMatch).Click += cmdGetGroupMatch_Click;
		((Control)_lblLabel_10).BackColor = SystemColors.Control;
		((Control)_lblLabel_10).Cursor = Cursors.Default;
		((Control)_lblLabel_10).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)_lblLabel_10).ForeColor = SystemColors.ControlText;
		((Control)_lblLabel_10).Location = new Point(23, 78);
		((Control)_lblLabel_10).Name = "_lblLabel_10";
		((Control)_lblLabel_10).RightToLeft = (RightToLeft)0;
		((Control)_lblLabel_10).Size = new Size(234, 16);
		((Control)_lblLabel_10).TabIndex = 100;
		((Control)_lblLabel_10).Text = "    6          7           8          9         10";
		((Control)lblLabel9).BackColor = SystemColors.Control;
		((Control)lblLabel9).Cursor = Cursors.Default;
		((Control)lblLabel9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel9).ForeColor = SystemColors.ControlText;
		((Control)lblLabel9).Location = new Point(22, 22);
		((Control)lblLabel9).Name = "lblLabel9";
		((Control)lblLabel9).RightToLeft = (RightToLeft)0;
		((Control)lblLabel9).Size = new Size(234, 16);
		((Control)lblLabel9).TabIndex = 99;
		((Control)lblLabel9).Text = "    1          2           3          4          5";
		((Control)cmbDoorState).BackColor = SystemColors.Window;
		((Control)cmbDoorState).Cursor = Cursors.Default;
		cmbDoorState.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbDoorState).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbDoorState).ForeColor = SystemColors.WindowText;
		cmbDoorState.Items.AddRange(new object[4] { "Reset", "Open", "Close", "Open...Close" });
		((Control)cmbDoorState).Location = new Point(12, 438);
		((Control)cmbDoorState).Name = "cmbDoorState";
		((Control)cmbDoorState).RightToLeft = (RightToLeft)0;
		((Control)cmbDoorState).Size = new Size(130, 27);
		((Control)cmbDoorState).TabIndex = 118;
		((Control)Frame2).BackColor = SystemColors.Control;
		((Control)Frame2).Controls.Add((Control)(object)txtUserPassTime2);
		((Control)Frame2).Controls.Add((Control)(object)txtUserPassTime1);
		((Control)Frame2).Controls.Add((Control)(object)txtUserPassTime0);
		((Control)Frame2).Controls.Add((Control)(object)txtUserID);
		((Control)Frame2).Controls.Add((Control)(object)txtGroupID);
		((Control)Frame2).Controls.Add((Control)(object)cmdGetUserPasstime);
		((Control)Frame2).Controls.Add((Control)(object)cmdSetUserPassTime);
		((Control)Frame2).Controls.Add((Control)(object)lblLabel3);
		((Control)Frame2).Controls.Add((Control)(object)lblLabel2);
		((Control)Frame2).Controls.Add((Control)(object)lblLabel1);
		((Control)Frame2).Controls.Add((Control)(object)lblLabel0);
		((Control)Frame2).Controls.Add((Control)(object)lblUserID);
		((Control)Frame2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Frame2).ForeColor = SystemColors.ControlText;
		((Control)Frame2).Location = new Point(414, 4);
		((Control)Frame2).Name = "Frame2";
		((Control)Frame2).RightToLeft = (RightToLeft)0;
		((Control)Frame2).Size = new Size(330, 145);
		((Control)Frame2).TabIndex = 115;
		Frame2.TabStop = false;
		((Control)Frame2).Text = "User Access";
		txtUserPassTime2.AcceptsReturn = true;
		((Control)txtUserPassTime2).BackColor = SystemColors.Window;
		((Control)txtUserPassTime2).Cursor = Cursors.IBeam;
		((Control)txtUserPassTime2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserPassTime2).ForeColor = SystemColors.WindowText;
		((Control)txtUserPassTime2).Location = new Point(272, 64);
		((TextBoxBase)txtUserPassTime2).MaxLength = 0;
		((Control)txtUserPassTime2).Name = "txtUserPassTime2";
		((Control)txtUserPassTime2).RightToLeft = (RightToLeft)0;
		((Control)txtUserPassTime2).Size = new Size(26, 28);
		((Control)txtUserPassTime2).TabIndex = 70;
		((Control)txtUserPassTime2).Text = "0";
		txtUserPassTime2.TextAlign = (HorizontalAlignment)2;
		txtUserPassTime1.AcceptsReturn = true;
		((Control)txtUserPassTime1).BackColor = SystemColors.Window;
		((Control)txtUserPassTime1).Cursor = Cursors.IBeam;
		((Control)txtUserPassTime1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserPassTime1).ForeColor = SystemColors.WindowText;
		((Control)txtUserPassTime1).Location = new Point(203, 64);
		((TextBoxBase)txtUserPassTime1).MaxLength = 0;
		((Control)txtUserPassTime1).Name = "txtUserPassTime1";
		((Control)txtUserPassTime1).RightToLeft = (RightToLeft)0;
		((Control)txtUserPassTime1).Size = new Size(26, 28);
		((Control)txtUserPassTime1).TabIndex = 69;
		((Control)txtUserPassTime1).Text = "0";
		txtUserPassTime1.TextAlign = (HorizontalAlignment)2;
		txtUserPassTime0.AcceptsReturn = true;
		((Control)txtUserPassTime0).BackColor = SystemColors.Window;
		((Control)txtUserPassTime0).Cursor = Cursors.IBeam;
		((Control)txtUserPassTime0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserPassTime0).ForeColor = SystemColors.WindowText;
		((Control)txtUserPassTime0).Location = new Point(136, 64);
		((TextBoxBase)txtUserPassTime0).MaxLength = 0;
		((Control)txtUserPassTime0).Name = "txtUserPassTime0";
		((Control)txtUserPassTime0).RightToLeft = (RightToLeft)0;
		((Control)txtUserPassTime0).Size = new Size(26, 28);
		((Control)txtUserPassTime0).TabIndex = 68;
		((Control)txtUserPassTime0).Text = "0";
		txtUserPassTime0.TextAlign = (HorizontalAlignment)2;
		txtUserID.AcceptsReturn = true;
		((Control)txtUserID).BackColor = SystemColors.Window;
		((Control)txtUserID).Cursor = Cursors.IBeam;
		((Control)txtUserID).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserID).ForeColor = SystemColors.WindowText;
		((Control)txtUserID).Location = new Point(128, 24);
		((TextBoxBase)txtUserID).MaxLength = 0;
		((Control)txtUserID).Name = "txtUserID";
		((Control)txtUserID).RightToLeft = (RightToLeft)0;
		((Control)txtUserID).Size = new Size(58, 27);
		((Control)txtUserID).TabIndex = 67;
		((Control)txtUserID).Text = "1";
		txtUserID.TextAlign = (HorizontalAlignment)2;
		txtGroupID.AcceptsReturn = true;
		((Control)txtGroupID).BackColor = SystemColors.Window;
		((Control)txtGroupID).Cursor = Cursors.IBeam;
		((Control)txtGroupID).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupID).ForeColor = SystemColors.WindowText;
		((Control)txtGroupID).Location = new Point(59, 64);
		((TextBoxBase)txtGroupID).MaxLength = 0;
		((Control)txtGroupID).Name = "txtGroupID";
		((Control)txtGroupID).RightToLeft = (RightToLeft)0;
		((Control)txtGroupID).Size = new Size(33, 28);
		((Control)txtGroupID).TabIndex = 66;
		((Control)txtGroupID).Text = "1";
		((Control)cmdGetUserPasstime).BackColor = SystemColors.Control;
		((Control)cmdGetUserPasstime).Cursor = Cursors.Default;
		((Control)cmdGetUserPasstime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetUserPasstime).ForeColor = SystemColors.ControlText;
		((Control)cmdGetUserPasstime).Location = new Point(42, 104);
		((Control)cmdGetUserPasstime).Name = "cmdGetUserPasstime";
		((Control)cmdGetUserPasstime).RightToLeft = (RightToLeft)0;
		((Control)cmdGetUserPasstime).Size = new Size(120, 31);
		((Control)cmdGetUserPasstime).TabIndex = 65;
		((Control)cmdGetUserPasstime).Text = "Get User";
		((ButtonBase)cmdGetUserPasstime).UseVisualStyleBackColor = false;
		((Control)cmdGetUserPasstime).Click += cmdGetUserPasstime_Click;
		((Control)cmdSetUserPassTime).BackColor = SystemColors.Control;
		((Control)cmdSetUserPassTime).Cursor = Cursors.Default;
		((Control)cmdSetUserPassTime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetUserPassTime).ForeColor = SystemColors.ControlText;
		((Control)cmdSetUserPassTime).Location = new Point(178, 104);
		((Control)cmdSetUserPassTime).Name = "cmdSetUserPassTime";
		((Control)cmdSetUserPassTime).RightToLeft = (RightToLeft)0;
		((Control)cmdSetUserPassTime).Size = new Size(120, 31);
		((Control)cmdSetUserPassTime).TabIndex = 64;
		((Control)cmdSetUserPassTime).Text = "Set User ";
		((ButtonBase)cmdSetUserPassTime).UseVisualStyleBackColor = false;
		((Control)cmdSetUserPassTime).Click += cmdSetUserPassTime_Click;
		((Control)lblLabel3).BackColor = SystemColors.Control;
		((Control)lblLabel3).Cursor = Cursors.Default;
		((Control)lblLabel3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel3).ForeColor = SystemColors.ControlText;
		((Control)lblLabel3).Location = new Point(232, 67);
		((Control)lblLabel3).Name = "lblLabel3";
		((Control)lblLabel3).RightToLeft = (RightToLeft)0;
		((Control)lblLabel3).Size = new Size(38, 24);
		((Control)lblLabel3).TabIndex = 91;
		((Control)lblLabel3).Text = "TZ3";
		lblLabel3.TextAlign = (ContentAlignment)4;
		((Control)lblLabel2).BackColor = SystemColors.Control;
		((Control)lblLabel2).Cursor = Cursors.Default;
		((Control)lblLabel2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel2).ForeColor = SystemColors.ControlText;
		((Control)lblLabel2).Location = new Point(164, 67);
		((Control)lblLabel2).Name = "lblLabel2";
		((Control)lblLabel2).RightToLeft = (RightToLeft)0;
		((Control)lblLabel2).Size = new Size(37, 24);
		((Control)lblLabel2).TabIndex = 90;
		((Control)lblLabel2).Text = "TZ2";
		lblLabel2.TextAlign = (ContentAlignment)4;
		((Control)lblLabel1).BackColor = SystemColors.Control;
		((Control)lblLabel1).Cursor = Cursors.Default;
		((Control)lblLabel1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel1).ForeColor = SystemColors.ControlText;
		((Control)lblLabel1).Location = new Point(98, 67);
		((Control)lblLabel1).Name = "lblLabel1";
		((Control)lblLabel1).RightToLeft = (RightToLeft)0;
		((Control)lblLabel1).Size = new Size(37, 24);
		((Control)lblLabel1).TabIndex = 89;
		((Control)lblLabel1).Text = "TZ1";
		lblLabel1.TextAlign = (ContentAlignment)4;
		((Control)lblLabel0).BackColor = SystemColors.Control;
		((Control)lblLabel0).Cursor = Cursors.Default;
		((Control)lblLabel0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel0).ForeColor = SystemColors.ControlText;
		((Control)lblLabel0).Location = new Point(8, 59);
		((Control)lblLabel0).Name = "lblLabel0";
		((Control)lblLabel0).RightToLeft = (RightToLeft)0;
		((Control)lblLabel0).Size = new Size(48, 40);
		((Control)lblLabel0).TabIndex = 88;
		((Control)lblLabel0).Text = "Group (1~5)";
		((Control)lblUserID).BackColor = SystemColors.Control;
		((Control)lblUserID).Cursor = Cursors.Default;
		((Control)lblUserID).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblUserID).ForeColor = SystemColors.ControlText;
		((Control)lblUserID).Location = new Point(56, 28);
		((Control)lblUserID).Name = "lblUserID";
		((Control)lblUserID).RightToLeft = (RightToLeft)0;
		((Control)lblUserID).Size = new Size(72, 25);
		((Control)lblUserID).TabIndex = 71;
		((Control)lblUserID).Text = "User ID";
		((Control)cmdSetDoorState).BackColor = SystemColors.Control;
		((Control)cmdSetDoorState).Cursor = Cursors.Default;
		((Control)cmdSetDoorState).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetDoorState).ForeColor = SystemColors.ControlText;
		((Control)cmdSetDoorState).Location = new Point(150, 434);
		((Control)cmdSetDoorState).Name = "cmdSetDoorState";
		((Control)cmdSetDoorState).RightToLeft = (RightToLeft)0;
		((Control)cmdSetDoorState).Size = new Size(120, 33);
		((Control)cmdSetDoorState).TabIndex = 111;
		((Control)cmdSetDoorState).Text = "Set Door State";
		((ButtonBase)cmdSetDoorState).UseVisualStyleBackColor = false;
		((Control)cmdSetDoorState).Click += cmdSetDoorState_Click;
		((Control)Frame1).BackColor = SystemColors.Control;
		((Control)Frame1).Controls.Add((Control)(object)cmdWrite);
		((Control)Frame1).Controls.Add((Control)(object)cmdRead);
		((Control)Frame1).Controls.Add((Control)(object)txtPassTimeID);
		((Control)Frame1).Controls.Add((Control)(object)lblLabel8);
		((Control)Frame1).Controls.Add((Control)(object)lblWeek0);
		((Control)Frame1).Controls.Add((Control)(object)lblWeek6);
		((Control)Frame1).Controls.Add((Control)(object)lblWeek5);
		((Control)Frame1).Controls.Add((Control)(object)lblWeek4);
		((Control)Frame1).Controls.Add((Control)(object)lblWeek3);
		((Control)Frame1).Controls.Add((Control)(object)lblWeek2);
		((Control)Frame1).Controls.Add((Control)(object)lblWeek1);
		((Control)Frame1).Controls.Add((Control)(object)lblMidSep6);
		((Control)Frame1).Controls.Add((Control)(object)txtEndHour0);
		((Control)Frame1).Controls.Add((Control)(object)txtEndMinute0);
		((Control)Frame1).Controls.Add((Control)(object)txtStartMinute0);
		((Control)Frame1).Controls.Add((Control)(object)txtStartHour0);
		((Control)Frame1).Controls.Add((Control)(object)txtEndHour4);
		((Control)Frame1).Controls.Add((Control)(object)txtEndMinute4);
		((Control)Frame1).Controls.Add((Control)(object)txtEndMinute3);
		((Control)Frame1).Controls.Add((Control)(object)txtEndHour3);
		((Control)Frame1).Controls.Add((Control)(object)txtEndHour2);
		((Control)Frame1).Controls.Add((Control)(object)txtEndMinute2);
		((Control)Frame1).Controls.Add((Control)(object)txtEndMinute1);
		((Control)Frame1).Controls.Add((Control)(object)txtEndHour1);
		((Control)Frame1).Controls.Add((Control)(object)txtStartMinute4);
		((Control)Frame1).Controls.Add((Control)(object)txtStartHour4);
		((Control)Frame1).Controls.Add((Control)(object)txtStartHour3);
		((Control)Frame1).Controls.Add((Control)(object)txtStartMinute3);
		((Control)Frame1).Controls.Add((Control)(object)txtStartMinute2);
		((Control)Frame1).Controls.Add((Control)(object)txtStartHour2);
		((Control)Frame1).Controls.Add((Control)(object)txtStartHour1);
		((Control)Frame1).Controls.Add((Control)(object)txtStartMinute1);
		((Control)Frame1).Controls.Add((Control)(object)txtStartHour5);
		((Control)Frame1).Controls.Add((Control)(object)txtStartHour6);
		((Control)Frame1).Controls.Add((Control)(object)txtStartMinute5);
		((Control)Frame1).Controls.Add((Control)(object)txtStartMinute6);
		((Control)Frame1).Controls.Add((Control)(object)txtEndHour5);
		((Control)Frame1).Controls.Add((Control)(object)txtEndHour6);
		((Control)Frame1).Controls.Add((Control)(object)txtEndMinute5);
		((Control)Frame1).Controls.Add((Control)(object)txtEndMinute6);
		((Control)Frame1).Controls.Add((Control)(object)lblStartSep6);
		((Control)Frame1).Controls.Add((Control)(object)lblEndSep6);
		((Control)Frame1).Controls.Add((Control)(object)lblEndSep5);
		((Control)Frame1).Controls.Add((Control)(object)lblEndSep4);
		((Control)Frame1).Controls.Add((Control)(object)lblEndSep3);
		((Control)Frame1).Controls.Add((Control)(object)lblEndSep2);
		((Control)Frame1).Controls.Add((Control)(object)lblEndSep1);
		((Control)Frame1).Controls.Add((Control)(object)lblEndSep0);
		((Control)Frame1).Controls.Add((Control)(object)lblStartSep5);
		((Control)Frame1).Controls.Add((Control)(object)lblStartSep4);
		((Control)Frame1).Controls.Add((Control)(object)lblStartSep3);
		((Control)Frame1).Controls.Add((Control)(object)lblStartSep2);
		((Control)Frame1).Controls.Add((Control)(object)lblStartSep1);
		((Control)Frame1).Controls.Add((Control)(object)lblMidSep5);
		((Control)Frame1).Controls.Add((Control)(object)lblMidSep4);
		((Control)Frame1).Controls.Add((Control)(object)lblMidSep0);
		((Control)Frame1).Controls.Add((Control)(object)lblStartSep0);
		((Control)Frame1).Controls.Add((Control)(object)lblMidSep3);
		((Control)Frame1).Controls.Add((Control)(object)lblMidSep2);
		((Control)Frame1).Controls.Add((Control)(object)lblMidSep1);
		((Control)Frame1).Controls.Add((Control)(object)lblStartTime);
		((Control)Frame1).Controls.Add((Control)(object)lblEndTime);
		((Control)Frame1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Frame1).ForeColor = SystemColors.ControlText;
		((Control)Frame1).Location = new Point(12, 68);
		((Control)Frame1).Name = "Frame1";
		((Control)Frame1).RightToLeft = (RightToLeft)0;
		((Control)Frame1).Size = new Size(390, 342);
		((Control)Frame1).TabIndex = 113;
		Frame1.TabStop = false;
		((Control)Frame1).Text = "PassTime Info";
		((Control)cmdWrite).BackColor = SystemColors.Control;
		((Control)cmdWrite).Cursor = Cursors.Default;
		((Control)cmdWrite).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdWrite).ForeColor = SystemColors.ControlText;
		((Control)cmdWrite).Location = new Point(280, 299);
		((Control)cmdWrite).Name = "cmdWrite";
		((Control)cmdWrite).RightToLeft = (RightToLeft)0;
		((Control)cmdWrite).Size = new Size(75, 33);
		((Control)cmdWrite).TabIndex = 116;
		((Control)cmdWrite).Text = "Write";
		((ButtonBase)cmdWrite).UseVisualStyleBackColor = false;
		((Control)cmdWrite).Click += cmdWrite_Click;
		((Control)cmdRead).BackColor = SystemColors.Control;
		((Control)cmdRead).Cursor = Cursors.Default;
		((Control)cmdRead).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdRead).ForeColor = SystemColors.ControlText;
		((Control)cmdRead).Location = new Point(175, 299);
		((Control)cmdRead).Name = "cmdRead";
		((Control)cmdRead).RightToLeft = (RightToLeft)0;
		((Control)cmdRead).Size = new Size(83, 33);
		((Control)cmdRead).TabIndex = 115;
		((Control)cmdRead).Text = "Read";
		((ButtonBase)cmdRead).UseVisualStyleBackColor = false;
		((Control)cmdRead).Click += cmdRead_Click;
		txtPassTimeID.AcceptsReturn = true;
		((Control)txtPassTimeID).BackColor = SystemColors.Window;
		((Control)txtPassTimeID).Cursor = Cursors.IBeam;
		((Control)txtPassTimeID).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtPassTimeID).ForeColor = SystemColors.WindowText;
		((Control)txtPassTimeID).Location = new Point(72, 299);
		((TextBoxBase)txtPassTimeID).MaxLength = 0;
		((Control)txtPassTimeID).Name = "txtPassTimeID";
		((Control)txtPassTimeID).RightToLeft = (RightToLeft)0;
		((Control)txtPassTimeID).Size = new Size(58, 27);
		((Control)txtPassTimeID).TabIndex = 113;
		((Control)txtPassTimeID).Text = "1";
		txtPassTimeID.TextAlign = (HorizontalAlignment)2;
		((Control)lblLabel8).BackColor = SystemColors.Control;
		((Control)lblLabel8).Cursor = Cursors.Default;
		((Control)lblLabel8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel8).ForeColor = SystemColors.ControlText;
		((Control)lblLabel8).Location = new Point(8, 291);
		((Control)lblLabel8).Name = "lblLabel8";
		((Control)lblLabel8).RightToLeft = (RightToLeft)0;
		((Control)lblLabel8).Size = new Size(56, 40);
		((Control)lblLabel8).TabIndex = 114;
		((Control)lblLabel8).Text = "   TZ (1~50)";
		((Control)lblWeek0).BackColor = Color.Transparent;
		((Control)lblWeek0).Cursor = Cursors.Default;
		((Control)lblWeek0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblWeek0).ForeColor = SystemColors.ControlText;
		((Control)lblWeek0).Location = new Point(34, 74);
		((Control)lblWeek0).Name = "lblWeek0";
		((Control)lblWeek0).RightToLeft = (RightToLeft)0;
		((Control)lblWeek0).Size = new Size(40, 20);
		((Control)lblWeek0).TabIndex = 112;
		((Control)lblWeek0).Text = "Sun";
		((Control)lblWeek6).BackColor = Color.Transparent;
		((Control)lblWeek6).Cursor = Cursors.Default;
		((Control)lblWeek6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblWeek6).ForeColor = SystemColors.ControlText;
		((Control)lblWeek6).Location = new Point(34, 242);
		((Control)lblWeek6).Name = "lblWeek6";
		((Control)lblWeek6).RightToLeft = (RightToLeft)0;
		((Control)lblWeek6).Size = new Size(40, 20);
		((Control)lblWeek6).TabIndex = 111;
		((Control)lblWeek6).Text = "Sat";
		((Control)lblWeek5).BackColor = Color.Transparent;
		((Control)lblWeek5).Cursor = Cursors.Default;
		((Control)lblWeek5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblWeek5).ForeColor = SystemColors.ControlText;
		((Control)lblWeek5).Location = new Point(34, 214);
		((Control)lblWeek5).Name = "lblWeek5";
		((Control)lblWeek5).RightToLeft = (RightToLeft)0;
		((Control)lblWeek5).Size = new Size(40, 20);
		((Control)lblWeek5).TabIndex = 110;
		((Control)lblWeek5).Text = "Fri";
		((Control)lblWeek4).BackColor = Color.Transparent;
		((Control)lblWeek4).Cursor = Cursors.Default;
		((Control)lblWeek4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblWeek4).ForeColor = SystemColors.ControlText;
		((Control)lblWeek4).Location = new Point(34, 186);
		((Control)lblWeek4).Name = "lblWeek4";
		((Control)lblWeek4).RightToLeft = (RightToLeft)0;
		((Control)lblWeek4).Size = new Size(40, 20);
		((Control)lblWeek4).TabIndex = 109;
		((Control)lblWeek4).Text = "Thu";
		((Control)lblWeek3).BackColor = Color.Transparent;
		((Control)lblWeek3).Cursor = Cursors.Default;
		((Control)lblWeek3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblWeek3).ForeColor = SystemColors.ControlText;
		((Control)lblWeek3).Location = new Point(34, 158);
		((Control)lblWeek3).Name = "lblWeek3";
		((Control)lblWeek3).RightToLeft = (RightToLeft)0;
		((Control)lblWeek3).Size = new Size(40, 20);
		((Control)lblWeek3).TabIndex = 108;
		((Control)lblWeek3).Text = "Wen";
		((Control)lblWeek2).BackColor = Color.Transparent;
		((Control)lblWeek2).Cursor = Cursors.Default;
		((Control)lblWeek2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblWeek2).ForeColor = SystemColors.ControlText;
		((Control)lblWeek2).Location = new Point(34, 130);
		((Control)lblWeek2).Name = "lblWeek2";
		((Control)lblWeek2).RightToLeft = (RightToLeft)0;
		((Control)lblWeek2).Size = new Size(40, 20);
		((Control)lblWeek2).TabIndex = 107;
		((Control)lblWeek2).Text = "Tue";
		((Control)lblWeek1).BackColor = Color.Transparent;
		((Control)lblWeek1).Cursor = Cursors.Default;
		((Control)lblWeek1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblWeek1).ForeColor = SystemColors.ControlText;
		((Control)lblWeek1).Location = new Point(34, 102);
		((Control)lblWeek1).Name = "lblWeek1";
		((Control)lblWeek1).RightToLeft = (RightToLeft)0;
		((Control)lblWeek1).Size = new Size(40, 20);
		((Control)lblWeek1).TabIndex = 106;
		((Control)lblWeek1).Text = "Mon";
		((Control)lblMidSep6).AutoSize = true;
		((Control)lblMidSep6).BackColor = Color.Transparent;
		((Control)lblMidSep6).Cursor = Cursors.Default;
		((Control)lblMidSep6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMidSep6).ForeColor = SystemColors.ControlText;
		((Control)lblMidSep6).Location = new Point(200, 243);
		((Control)lblMidSep6).Name = "lblMidSep6";
		((Control)lblMidSep6).RightToLeft = (RightToLeft)0;
		((Control)lblMidSep6).Size = new Size(18, 19);
		((Control)lblMidSep6).TabIndex = 56;
		((Control)lblMidSep6).Text = "~";
		txtEndHour0.AcceptsReturn = true;
		((Control)txtEndHour0).BackColor = SystemColors.Window;
		((Control)txtEndHour0).Cursor = Cursors.IBeam;
		((Control)txtEndHour0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndHour0).ForeColor = SystemColors.WindowText;
		((Control)txtEndHour0).Location = new Point(242, 74);
		((TextBoxBase)txtEndHour0).MaxLength = 2;
		((Control)txtEndHour0).Name = "txtEndHour0";
		((Control)txtEndHour0).RightToLeft = (RightToLeft)0;
		((Control)txtEndHour0).Size = new Size(42, 26);
		((Control)txtEndHour0).TabIndex = 28;
		((Control)txtEndHour0).Text = "0";
		txtEndHour0.TextAlign = (HorizontalAlignment)2;
		txtEndMinute0.AcceptsReturn = true;
		((Control)txtEndMinute0).BackColor = SystemColors.Window;
		((Control)txtEndMinute0).Cursor = Cursors.IBeam;
		((Control)txtEndMinute0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndMinute0).ForeColor = SystemColors.WindowText;
		((Control)txtEndMinute0).Location = new Point(306, 74);
		((TextBoxBase)txtEndMinute0).MaxLength = 2;
		((Control)txtEndMinute0).Name = "txtEndMinute0";
		((Control)txtEndMinute0).RightToLeft = (RightToLeft)0;
		((Control)txtEndMinute0).Size = new Size(42, 26);
		((Control)txtEndMinute0).TabIndex = 27;
		((Control)txtEndMinute0).Text = "0";
		txtEndMinute0.TextAlign = (HorizontalAlignment)2;
		txtStartMinute0.AcceptsReturn = true;
		((Control)txtStartMinute0).BackColor = SystemColors.Window;
		((Control)txtStartMinute0).Cursor = Cursors.IBeam;
		((Control)txtStartMinute0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartMinute0).ForeColor = SystemColors.WindowText;
		((Control)txtStartMinute0).Location = new Point(138, 74);
		((TextBoxBase)txtStartMinute0).MaxLength = 2;
		((Control)txtStartMinute0).Name = "txtStartMinute0";
		((Control)txtStartMinute0).RightToLeft = (RightToLeft)0;
		((Control)txtStartMinute0).Size = new Size(42, 26);
		((Control)txtStartMinute0).TabIndex = 26;
		((Control)txtStartMinute0).Text = "0";
		txtStartMinute0.TextAlign = (HorizontalAlignment)2;
		txtStartHour0.AcceptsReturn = true;
		((Control)txtStartHour0).BackColor = SystemColors.Window;
		((Control)txtStartHour0).Cursor = Cursors.IBeam;
		((Control)txtStartHour0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartHour0).ForeColor = SystemColors.WindowText;
		((Control)txtStartHour0).Location = new Point(74, 74);
		((TextBoxBase)txtStartHour0).MaxLength = 2;
		((Control)txtStartHour0).Name = "txtStartHour0";
		((Control)txtStartHour0).RightToLeft = (RightToLeft)0;
		((Control)txtStartHour0).Size = new Size(42, 26);
		((Control)txtStartHour0).TabIndex = 25;
		((Control)txtStartHour0).Text = "0";
		txtStartHour0.TextAlign = (HorizontalAlignment)2;
		txtEndHour4.AcceptsReturn = true;
		((Control)txtEndHour4).BackColor = SystemColors.Window;
		((Control)txtEndHour4).Cursor = Cursors.IBeam;
		((Control)txtEndHour4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndHour4).ForeColor = SystemColors.WindowText;
		((Control)txtEndHour4).Location = new Point(242, 186);
		((TextBoxBase)txtEndHour4).MaxLength = 2;
		((Control)txtEndHour4).Name = "txtEndHour4";
		((Control)txtEndHour4).RightToLeft = (RightToLeft)0;
		((Control)txtEndHour4).Size = new Size(42, 26);
		((Control)txtEndHour4).TabIndex = 24;
		((Control)txtEndHour4).Text = "0";
		txtEndHour4.TextAlign = (HorizontalAlignment)2;
		txtEndMinute4.AcceptsReturn = true;
		((Control)txtEndMinute4).BackColor = SystemColors.Window;
		((Control)txtEndMinute4).Cursor = Cursors.IBeam;
		((Control)txtEndMinute4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndMinute4).ForeColor = SystemColors.WindowText;
		((Control)txtEndMinute4).Location = new Point(306, 186);
		((TextBoxBase)txtEndMinute4).MaxLength = 2;
		((Control)txtEndMinute4).Name = "txtEndMinute4";
		((Control)txtEndMinute4).RightToLeft = (RightToLeft)0;
		((Control)txtEndMinute4).Size = new Size(42, 26);
		((Control)txtEndMinute4).TabIndex = 23;
		((Control)txtEndMinute4).Text = "0";
		txtEndMinute4.TextAlign = (HorizontalAlignment)2;
		txtEndMinute3.AcceptsReturn = true;
		((Control)txtEndMinute3).BackColor = SystemColors.Window;
		((Control)txtEndMinute3).Cursor = Cursors.IBeam;
		((Control)txtEndMinute3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndMinute3).ForeColor = SystemColors.WindowText;
		((Control)txtEndMinute3).Location = new Point(306, 158);
		((TextBoxBase)txtEndMinute3).MaxLength = 2;
		((Control)txtEndMinute3).Name = "txtEndMinute3";
		((Control)txtEndMinute3).RightToLeft = (RightToLeft)0;
		((Control)txtEndMinute3).Size = new Size(42, 26);
		((Control)txtEndMinute3).TabIndex = 22;
		((Control)txtEndMinute3).Text = "0";
		txtEndMinute3.TextAlign = (HorizontalAlignment)2;
		txtEndHour3.AcceptsReturn = true;
		((Control)txtEndHour3).BackColor = SystemColors.Window;
		((Control)txtEndHour3).Cursor = Cursors.IBeam;
		((Control)txtEndHour3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndHour3).ForeColor = SystemColors.WindowText;
		((Control)txtEndHour3).Location = new Point(242, 158);
		((TextBoxBase)txtEndHour3).MaxLength = 2;
		((Control)txtEndHour3).Name = "txtEndHour3";
		((Control)txtEndHour3).RightToLeft = (RightToLeft)0;
		((Control)txtEndHour3).Size = new Size(42, 26);
		((Control)txtEndHour3).TabIndex = 21;
		((Control)txtEndHour3).Text = "0";
		txtEndHour3.TextAlign = (HorizontalAlignment)2;
		txtEndHour2.AcceptsReturn = true;
		((Control)txtEndHour2).BackColor = SystemColors.Window;
		((Control)txtEndHour2).Cursor = Cursors.IBeam;
		((Control)txtEndHour2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndHour2).ForeColor = SystemColors.WindowText;
		((Control)txtEndHour2).Location = new Point(242, 130);
		((TextBoxBase)txtEndHour2).MaxLength = 2;
		((Control)txtEndHour2).Name = "txtEndHour2";
		((Control)txtEndHour2).RightToLeft = (RightToLeft)0;
		((Control)txtEndHour2).Size = new Size(42, 26);
		((Control)txtEndHour2).TabIndex = 20;
		((Control)txtEndHour2).Text = "0";
		txtEndHour2.TextAlign = (HorizontalAlignment)2;
		txtEndMinute2.AcceptsReturn = true;
		((Control)txtEndMinute2).BackColor = SystemColors.Window;
		((Control)txtEndMinute2).Cursor = Cursors.IBeam;
		((Control)txtEndMinute2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndMinute2).ForeColor = SystemColors.WindowText;
		((Control)txtEndMinute2).Location = new Point(306, 130);
		((TextBoxBase)txtEndMinute2).MaxLength = 2;
		((Control)txtEndMinute2).Name = "txtEndMinute2";
		((Control)txtEndMinute2).RightToLeft = (RightToLeft)0;
		((Control)txtEndMinute2).Size = new Size(42, 26);
		((Control)txtEndMinute2).TabIndex = 19;
		((Control)txtEndMinute2).Text = "0";
		txtEndMinute2.TextAlign = (HorizontalAlignment)2;
		txtEndMinute1.AcceptsReturn = true;
		((Control)txtEndMinute1).BackColor = SystemColors.Window;
		((Control)txtEndMinute1).Cursor = Cursors.IBeam;
		((Control)txtEndMinute1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndMinute1).ForeColor = SystemColors.WindowText;
		((Control)txtEndMinute1).Location = new Point(306, 102);
		((TextBoxBase)txtEndMinute1).MaxLength = 2;
		((Control)txtEndMinute1).Name = "txtEndMinute1";
		((Control)txtEndMinute1).RightToLeft = (RightToLeft)0;
		((Control)txtEndMinute1).Size = new Size(42, 26);
		((Control)txtEndMinute1).TabIndex = 18;
		((Control)txtEndMinute1).Text = "0";
		txtEndMinute1.TextAlign = (HorizontalAlignment)2;
		txtEndHour1.AcceptsReturn = true;
		((Control)txtEndHour1).BackColor = SystemColors.Window;
		((Control)txtEndHour1).Cursor = Cursors.IBeam;
		((Control)txtEndHour1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndHour1).ForeColor = SystemColors.WindowText;
		((Control)txtEndHour1).Location = new Point(242, 102);
		((TextBoxBase)txtEndHour1).MaxLength = 2;
		((Control)txtEndHour1).Name = "txtEndHour1";
		((Control)txtEndHour1).RightToLeft = (RightToLeft)0;
		((Control)txtEndHour1).Size = new Size(42, 26);
		((Control)txtEndHour1).TabIndex = 17;
		((Control)txtEndHour1).Text = "0";
		txtEndHour1.TextAlign = (HorizontalAlignment)2;
		txtStartMinute4.AcceptsReturn = true;
		((Control)txtStartMinute4).BackColor = SystemColors.Window;
		((Control)txtStartMinute4).Cursor = Cursors.IBeam;
		((Control)txtStartMinute4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartMinute4).ForeColor = SystemColors.WindowText;
		((Control)txtStartMinute4).Location = new Point(138, 186);
		((TextBoxBase)txtStartMinute4).MaxLength = 2;
		((Control)txtStartMinute4).Name = "txtStartMinute4";
		((Control)txtStartMinute4).RightToLeft = (RightToLeft)0;
		((Control)txtStartMinute4).Size = new Size(42, 26);
		((Control)txtStartMinute4).TabIndex = 16;
		((Control)txtStartMinute4).Text = "0";
		txtStartMinute4.TextAlign = (HorizontalAlignment)2;
		txtStartHour4.AcceptsReturn = true;
		((Control)txtStartHour4).BackColor = SystemColors.Window;
		((Control)txtStartHour4).Cursor = Cursors.IBeam;
		((Control)txtStartHour4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartHour4).ForeColor = SystemColors.WindowText;
		((Control)txtStartHour4).Location = new Point(74, 186);
		((TextBoxBase)txtStartHour4).MaxLength = 2;
		((Control)txtStartHour4).Name = "txtStartHour4";
		((Control)txtStartHour4).RightToLeft = (RightToLeft)0;
		((Control)txtStartHour4).Size = new Size(42, 26);
		((Control)txtStartHour4).TabIndex = 15;
		((Control)txtStartHour4).Text = "0";
		txtStartHour4.TextAlign = (HorizontalAlignment)2;
		txtStartHour3.AcceptsReturn = true;
		((Control)txtStartHour3).BackColor = SystemColors.Window;
		((Control)txtStartHour3).Cursor = Cursors.IBeam;
		((Control)txtStartHour3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartHour3).ForeColor = SystemColors.WindowText;
		((Control)txtStartHour3).Location = new Point(74, 158);
		((TextBoxBase)txtStartHour3).MaxLength = 2;
		((Control)txtStartHour3).Name = "txtStartHour3";
		((Control)txtStartHour3).RightToLeft = (RightToLeft)0;
		((Control)txtStartHour3).Size = new Size(42, 26);
		((Control)txtStartHour3).TabIndex = 14;
		((Control)txtStartHour3).Text = "0";
		txtStartHour3.TextAlign = (HorizontalAlignment)2;
		txtStartMinute3.AcceptsReturn = true;
		((Control)txtStartMinute3).BackColor = SystemColors.Window;
		((Control)txtStartMinute3).Cursor = Cursors.IBeam;
		((Control)txtStartMinute3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartMinute3).ForeColor = SystemColors.WindowText;
		((Control)txtStartMinute3).Location = new Point(138, 158);
		((TextBoxBase)txtStartMinute3).MaxLength = 2;
		((Control)txtStartMinute3).Name = "txtStartMinute3";
		((Control)txtStartMinute3).RightToLeft = (RightToLeft)0;
		((Control)txtStartMinute3).Size = new Size(42, 26);
		((Control)txtStartMinute3).TabIndex = 13;
		((Control)txtStartMinute3).Text = "0";
		txtStartMinute3.TextAlign = (HorizontalAlignment)2;
		txtStartMinute2.AcceptsReturn = true;
		((Control)txtStartMinute2).BackColor = SystemColors.Window;
		((Control)txtStartMinute2).Cursor = Cursors.IBeam;
		((Control)txtStartMinute2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartMinute2).ForeColor = SystemColors.WindowText;
		((Control)txtStartMinute2).Location = new Point(138, 130);
		((TextBoxBase)txtStartMinute2).MaxLength = 2;
		((Control)txtStartMinute2).Name = "txtStartMinute2";
		((Control)txtStartMinute2).RightToLeft = (RightToLeft)0;
		((Control)txtStartMinute2).Size = new Size(42, 26);
		((Control)txtStartMinute2).TabIndex = 12;
		((Control)txtStartMinute2).Text = "0";
		txtStartMinute2.TextAlign = (HorizontalAlignment)2;
		txtStartHour2.AcceptsReturn = true;
		((Control)txtStartHour2).BackColor = SystemColors.Window;
		((Control)txtStartHour2).Cursor = Cursors.IBeam;
		((Control)txtStartHour2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartHour2).ForeColor = SystemColors.WindowText;
		((Control)txtStartHour2).Location = new Point(74, 130);
		((TextBoxBase)txtStartHour2).MaxLength = 2;
		((Control)txtStartHour2).Name = "txtStartHour2";
		((Control)txtStartHour2).RightToLeft = (RightToLeft)0;
		((Control)txtStartHour2).Size = new Size(42, 26);
		((Control)txtStartHour2).TabIndex = 11;
		((Control)txtStartHour2).Text = "0";
		txtStartHour2.TextAlign = (HorizontalAlignment)2;
		txtStartHour1.AcceptsReturn = true;
		((Control)txtStartHour1).BackColor = SystemColors.Window;
		((Control)txtStartHour1).Cursor = Cursors.IBeam;
		((Control)txtStartHour1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartHour1).ForeColor = SystemColors.WindowText;
		((Control)txtStartHour1).Location = new Point(74, 102);
		((TextBoxBase)txtStartHour1).MaxLength = 2;
		((Control)txtStartHour1).Name = "txtStartHour1";
		((Control)txtStartHour1).RightToLeft = (RightToLeft)0;
		((Control)txtStartHour1).Size = new Size(42, 26);
		((Control)txtStartHour1).TabIndex = 10;
		((Control)txtStartHour1).Text = "0";
		txtStartHour1.TextAlign = (HorizontalAlignment)2;
		txtStartMinute1.AcceptsReturn = true;
		((Control)txtStartMinute1).BackColor = SystemColors.Window;
		((Control)txtStartMinute1).Cursor = Cursors.IBeam;
		((Control)txtStartMinute1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartMinute1).ForeColor = SystemColors.WindowText;
		((Control)txtStartMinute1).Location = new Point(138, 102);
		((TextBoxBase)txtStartMinute1).MaxLength = 2;
		((Control)txtStartMinute1).Name = "txtStartMinute1";
		((Control)txtStartMinute1).RightToLeft = (RightToLeft)0;
		((Control)txtStartMinute1).Size = new Size(42, 26);
		((Control)txtStartMinute1).TabIndex = 9;
		((Control)txtStartMinute1).Text = "0";
		txtStartMinute1.TextAlign = (HorizontalAlignment)2;
		txtStartHour5.AcceptsReturn = true;
		((Control)txtStartHour5).BackColor = SystemColors.Window;
		((Control)txtStartHour5).Cursor = Cursors.IBeam;
		((Control)txtStartHour5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartHour5).ForeColor = SystemColors.WindowText;
		((Control)txtStartHour5).Location = new Point(74, 214);
		((TextBoxBase)txtStartHour5).MaxLength = 2;
		((Control)txtStartHour5).Name = "txtStartHour5";
		((Control)txtStartHour5).RightToLeft = (RightToLeft)0;
		((Control)txtStartHour5).Size = new Size(42, 26);
		((Control)txtStartHour5).TabIndex = 8;
		((Control)txtStartHour5).Text = "0";
		txtStartHour5.TextAlign = (HorizontalAlignment)2;
		txtStartHour6.AcceptsReturn = true;
		((Control)txtStartHour6).BackColor = SystemColors.Window;
		((Control)txtStartHour6).Cursor = Cursors.IBeam;
		((Control)txtStartHour6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartHour6).ForeColor = SystemColors.WindowText;
		((Control)txtStartHour6).Location = new Point(74, 242);
		((TextBoxBase)txtStartHour6).MaxLength = 2;
		((Control)txtStartHour6).Name = "txtStartHour6";
		((Control)txtStartHour6).RightToLeft = (RightToLeft)0;
		((Control)txtStartHour6).Size = new Size(42, 26);
		((Control)txtStartHour6).TabIndex = 7;
		((Control)txtStartHour6).Text = "0";
		txtStartHour6.TextAlign = (HorizontalAlignment)2;
		txtStartMinute5.AcceptsReturn = true;
		((Control)txtStartMinute5).BackColor = SystemColors.Window;
		((Control)txtStartMinute5).Cursor = Cursors.IBeam;
		((Control)txtStartMinute5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartMinute5).ForeColor = SystemColors.WindowText;
		((Control)txtStartMinute5).Location = new Point(138, 214);
		((TextBoxBase)txtStartMinute5).MaxLength = 2;
		((Control)txtStartMinute5).Name = "txtStartMinute5";
		((Control)txtStartMinute5).RightToLeft = (RightToLeft)0;
		((Control)txtStartMinute5).Size = new Size(42, 26);
		((Control)txtStartMinute5).TabIndex = 6;
		((Control)txtStartMinute5).Text = "0";
		txtStartMinute5.TextAlign = (HorizontalAlignment)2;
		txtStartMinute6.AcceptsReturn = true;
		((Control)txtStartMinute6).BackColor = SystemColors.Window;
		((Control)txtStartMinute6).Cursor = Cursors.IBeam;
		((Control)txtStartMinute6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtStartMinute6).ForeColor = SystemColors.WindowText;
		((Control)txtStartMinute6).Location = new Point(138, 242);
		((TextBoxBase)txtStartMinute6).MaxLength = 2;
		((Control)txtStartMinute6).Name = "txtStartMinute6";
		((Control)txtStartMinute6).RightToLeft = (RightToLeft)0;
		((Control)txtStartMinute6).Size = new Size(42, 26);
		((Control)txtStartMinute6).TabIndex = 5;
		((Control)txtStartMinute6).Text = "0";
		txtStartMinute6.TextAlign = (HorizontalAlignment)2;
		txtEndHour5.AcceptsReturn = true;
		((Control)txtEndHour5).BackColor = SystemColors.Window;
		((Control)txtEndHour5).Cursor = Cursors.IBeam;
		((Control)txtEndHour5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndHour5).ForeColor = SystemColors.WindowText;
		((Control)txtEndHour5).Location = new Point(242, 214);
		((TextBoxBase)txtEndHour5).MaxLength = 2;
		((Control)txtEndHour5).Name = "txtEndHour5";
		((Control)txtEndHour5).RightToLeft = (RightToLeft)0;
		((Control)txtEndHour5).Size = new Size(42, 26);
		((Control)txtEndHour5).TabIndex = 4;
		((Control)txtEndHour5).Text = "0";
		txtEndHour5.TextAlign = (HorizontalAlignment)2;
		txtEndHour6.AcceptsReturn = true;
		((Control)txtEndHour6).BackColor = SystemColors.Window;
		((Control)txtEndHour6).Cursor = Cursors.IBeam;
		((Control)txtEndHour6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndHour6).ForeColor = SystemColors.WindowText;
		((Control)txtEndHour6).Location = new Point(242, 242);
		((TextBoxBase)txtEndHour6).MaxLength = 2;
		((Control)txtEndHour6).Name = "txtEndHour6";
		((Control)txtEndHour6).RightToLeft = (RightToLeft)0;
		((Control)txtEndHour6).Size = new Size(42, 26);
		((Control)txtEndHour6).TabIndex = 3;
		((Control)txtEndHour6).Text = "0";
		txtEndHour6.TextAlign = (HorizontalAlignment)2;
		txtEndMinute5.AcceptsReturn = true;
		((Control)txtEndMinute5).BackColor = SystemColors.Window;
		((Control)txtEndMinute5).Cursor = Cursors.IBeam;
		((Control)txtEndMinute5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndMinute5).ForeColor = SystemColors.WindowText;
		((Control)txtEndMinute5).Location = new Point(306, 214);
		((TextBoxBase)txtEndMinute5).MaxLength = 2;
		((Control)txtEndMinute5).Name = "txtEndMinute5";
		((Control)txtEndMinute5).RightToLeft = (RightToLeft)0;
		((Control)txtEndMinute5).Size = new Size(42, 26);
		((Control)txtEndMinute5).TabIndex = 2;
		((Control)txtEndMinute5).Text = "0";
		txtEndMinute5.TextAlign = (HorizontalAlignment)2;
		txtEndMinute6.AcceptsReturn = true;
		((Control)txtEndMinute6).BackColor = SystemColors.Window;
		((Control)txtEndMinute6).Cursor = Cursors.IBeam;
		((Control)txtEndMinute6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEndMinute6).ForeColor = SystemColors.WindowText;
		((Control)txtEndMinute6).Location = new Point(306, 242);
		((TextBoxBase)txtEndMinute6).MaxLength = 2;
		((Control)txtEndMinute6).Name = "txtEndMinute6";
		((Control)txtEndMinute6).RightToLeft = (RightToLeft)0;
		((Control)txtEndMinute6).Size = new Size(42, 26);
		((Control)txtEndMinute6).TabIndex = 1;
		((Control)txtEndMinute6).Text = "0";
		txtEndMinute6.TextAlign = (HorizontalAlignment)2;
		((Control)lblStartSep6).AutoSize = true;
		((Control)lblStartSep6).BackColor = Color.Transparent;
		((Control)lblStartSep6).Cursor = Cursors.Default;
		((Control)lblStartSep6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartSep6).ForeColor = SystemColors.ControlText;
		((Control)lblStartSep6).Location = new Point(122, 246);
		((Control)lblStartSep6).Name = "lblStartSep6";
		((Control)lblStartSep6).RightToLeft = (RightToLeft)0;
		((Control)lblStartSep6).Size = new Size(12, 19);
		((Control)lblStartSep6).TabIndex = 55;
		((Control)lblStartSep6).Text = ":";
		((Control)lblEndSep6).AutoSize = true;
		((Control)lblEndSep6).BackColor = Color.Transparent;
		((Control)lblEndSep6).Cursor = Cursors.Default;
		((Control)lblEndSep6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndSep6).ForeColor = SystemColors.ControlText;
		((Control)lblEndSep6).Location = new Point(290, 246);
		((Control)lblEndSep6).Name = "lblEndSep6";
		((Control)lblEndSep6).RightToLeft = (RightToLeft)0;
		((Control)lblEndSep6).Size = new Size(12, 19);
		((Control)lblEndSep6).TabIndex = 54;
		((Control)lblEndSep6).Text = ":";
		((Control)lblEndSep5).AutoSize = true;
		((Control)lblEndSep5).BackColor = Color.Transparent;
		((Control)lblEndSep5).Cursor = Cursors.Default;
		((Control)lblEndSep5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndSep5).ForeColor = SystemColors.ControlText;
		((Control)lblEndSep5).Location = new Point(290, 218);
		((Control)lblEndSep5).Name = "lblEndSep5";
		((Control)lblEndSep5).RightToLeft = (RightToLeft)0;
		((Control)lblEndSep5).Size = new Size(12, 19);
		((Control)lblEndSep5).TabIndex = 53;
		((Control)lblEndSep5).Text = ":";
		((Control)lblEndSep4).AutoSize = true;
		((Control)lblEndSep4).BackColor = Color.Transparent;
		((Control)lblEndSep4).Cursor = Cursors.Default;
		((Control)lblEndSep4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndSep4).ForeColor = SystemColors.ControlText;
		((Control)lblEndSep4).Location = new Point(290, 190);
		((Control)lblEndSep4).Name = "lblEndSep4";
		((Control)lblEndSep4).RightToLeft = (RightToLeft)0;
		((Control)lblEndSep4).Size = new Size(12, 19);
		((Control)lblEndSep4).TabIndex = 52;
		((Control)lblEndSep4).Text = ":";
		((Control)lblEndSep3).AutoSize = true;
		((Control)lblEndSep3).BackColor = Color.Transparent;
		((Control)lblEndSep3).Cursor = Cursors.Default;
		((Control)lblEndSep3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndSep3).ForeColor = SystemColors.ControlText;
		((Control)lblEndSep3).Location = new Point(290, 162);
		((Control)lblEndSep3).Name = "lblEndSep3";
		((Control)lblEndSep3).RightToLeft = (RightToLeft)0;
		((Control)lblEndSep3).Size = new Size(12, 19);
		((Control)lblEndSep3).TabIndex = 51;
		((Control)lblEndSep3).Text = ":";
		((Control)lblEndSep2).AutoSize = true;
		((Control)lblEndSep2).BackColor = Color.Transparent;
		((Control)lblEndSep2).Cursor = Cursors.Default;
		((Control)lblEndSep2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndSep2).ForeColor = SystemColors.ControlText;
		((Control)lblEndSep2).Location = new Point(290, 134);
		((Control)lblEndSep2).Name = "lblEndSep2";
		((Control)lblEndSep2).RightToLeft = (RightToLeft)0;
		((Control)lblEndSep2).Size = new Size(12, 19);
		((Control)lblEndSep2).TabIndex = 50;
		((Control)lblEndSep2).Text = ":";
		((Control)lblEndSep1).AutoSize = true;
		((Control)lblEndSep1).BackColor = Color.Transparent;
		((Control)lblEndSep1).Cursor = Cursors.Default;
		((Control)lblEndSep1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndSep1).ForeColor = SystemColors.ControlText;
		((Control)lblEndSep1).Location = new Point(290, 106);
		((Control)lblEndSep1).Name = "lblEndSep1";
		((Control)lblEndSep1).RightToLeft = (RightToLeft)0;
		((Control)lblEndSep1).Size = new Size(12, 19);
		((Control)lblEndSep1).TabIndex = 49;
		((Control)lblEndSep1).Text = ":";
		((Control)lblEndSep0).AutoSize = true;
		((Control)lblEndSep0).BackColor = Color.Transparent;
		((Control)lblEndSep0).Cursor = Cursors.Default;
		((Control)lblEndSep0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndSep0).ForeColor = SystemColors.ControlText;
		((Control)lblEndSep0).Location = new Point(290, 78);
		((Control)lblEndSep0).Name = "lblEndSep0";
		((Control)lblEndSep0).RightToLeft = (RightToLeft)0;
		((Control)lblEndSep0).Size = new Size(12, 19);
		((Control)lblEndSep0).TabIndex = 48;
		((Control)lblEndSep0).Text = ":";
		((Control)lblStartSep5).AutoSize = true;
		((Control)lblStartSep5).BackColor = Color.Transparent;
		((Control)lblStartSep5).Cursor = Cursors.Default;
		((Control)lblStartSep5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartSep5).ForeColor = SystemColors.ControlText;
		((Control)lblStartSep5).Location = new Point(122, 218);
		((Control)lblStartSep5).Name = "lblStartSep5";
		((Control)lblStartSep5).RightToLeft = (RightToLeft)0;
		((Control)lblStartSep5).Size = new Size(12, 19);
		((Control)lblStartSep5).TabIndex = 47;
		((Control)lblStartSep5).Text = ":";
		((Control)lblStartSep4).AutoSize = true;
		((Control)lblStartSep4).BackColor = Color.Transparent;
		((Control)lblStartSep4).Cursor = Cursors.Default;
		((Control)lblStartSep4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartSep4).ForeColor = SystemColors.ControlText;
		((Control)lblStartSep4).Location = new Point(122, 190);
		((Control)lblStartSep4).Name = "lblStartSep4";
		((Control)lblStartSep4).RightToLeft = (RightToLeft)0;
		((Control)lblStartSep4).Size = new Size(12, 19);
		((Control)lblStartSep4).TabIndex = 46;
		((Control)lblStartSep4).Text = ":";
		((Control)lblStartSep3).AutoSize = true;
		((Control)lblStartSep3).BackColor = Color.Transparent;
		((Control)lblStartSep3).Cursor = Cursors.Default;
		((Control)lblStartSep3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartSep3).ForeColor = SystemColors.ControlText;
		((Control)lblStartSep3).Location = new Point(122, 162);
		((Control)lblStartSep3).Name = "lblStartSep3";
		((Control)lblStartSep3).RightToLeft = (RightToLeft)0;
		((Control)lblStartSep3).Size = new Size(12, 19);
		((Control)lblStartSep3).TabIndex = 45;
		((Control)lblStartSep3).Text = ":";
		((Control)lblStartSep2).AutoSize = true;
		((Control)lblStartSep2).BackColor = Color.Transparent;
		((Control)lblStartSep2).Cursor = Cursors.Default;
		((Control)lblStartSep2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartSep2).ForeColor = SystemColors.ControlText;
		((Control)lblStartSep2).Location = new Point(122, 134);
		((Control)lblStartSep2).Name = "lblStartSep2";
		((Control)lblStartSep2).RightToLeft = (RightToLeft)0;
		((Control)lblStartSep2).Size = new Size(12, 19);
		((Control)lblStartSep2).TabIndex = 44;
		((Control)lblStartSep2).Text = ":";
		((Control)lblStartSep1).AutoSize = true;
		((Control)lblStartSep1).BackColor = Color.Transparent;
		((Control)lblStartSep1).Cursor = Cursors.Default;
		((Control)lblStartSep1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartSep1).ForeColor = SystemColors.ControlText;
		((Control)lblStartSep1).Location = new Point(122, 106);
		((Control)lblStartSep1).Name = "lblStartSep1";
		((Control)lblStartSep1).RightToLeft = (RightToLeft)0;
		((Control)lblStartSep1).Size = new Size(12, 19);
		((Control)lblStartSep1).TabIndex = 43;
		((Control)lblStartSep1).Text = ":";
		((Control)lblMidSep5).AutoSize = true;
		((Control)lblMidSep5).BackColor = Color.Transparent;
		((Control)lblMidSep5).Cursor = Cursors.Default;
		((Control)lblMidSep5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMidSep5).ForeColor = SystemColors.ControlText;
		((Control)lblMidSep5).Location = new Point(200, 215);
		((Control)lblMidSep5).Name = "lblMidSep5";
		((Control)lblMidSep5).RightToLeft = (RightToLeft)0;
		((Control)lblMidSep5).Size = new Size(18, 19);
		((Control)lblMidSep5).TabIndex = 42;
		((Control)lblMidSep5).Text = "~";
		((Control)lblMidSep4).AutoSize = true;
		((Control)lblMidSep4).BackColor = Color.Transparent;
		((Control)lblMidSep4).Cursor = Cursors.Default;
		((Control)lblMidSep4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMidSep4).ForeColor = SystemColors.ControlText;
		((Control)lblMidSep4).Location = new Point(200, 187);
		((Control)lblMidSep4).Name = "lblMidSep4";
		((Control)lblMidSep4).RightToLeft = (RightToLeft)0;
		((Control)lblMidSep4).Size = new Size(18, 19);
		((Control)lblMidSep4).TabIndex = 41;
		((Control)lblMidSep4).Text = "~";
		((Control)lblMidSep0).AutoSize = true;
		((Control)lblMidSep0).BackColor = Color.Transparent;
		((Control)lblMidSep0).Cursor = Cursors.Default;
		((Control)lblMidSep0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMidSep0).ForeColor = SystemColors.ControlText;
		((Control)lblMidSep0).Location = new Point(200, 75);
		((Control)lblMidSep0).Name = "lblMidSep0";
		((Control)lblMidSep0).RightToLeft = (RightToLeft)0;
		((Control)lblMidSep0).Size = new Size(18, 19);
		((Control)lblMidSep0).TabIndex = 36;
		((Control)lblMidSep0).Text = "~";
		((Control)lblStartSep0).AutoSize = true;
		((Control)lblStartSep0).BackColor = Color.Transparent;
		((Control)lblStartSep0).Cursor = Cursors.Default;
		((Control)lblStartSep0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartSep0).ForeColor = SystemColors.ControlText;
		((Control)lblStartSep0).Location = new Point(122, 78);
		((Control)lblStartSep0).Name = "lblStartSep0";
		((Control)lblStartSep0).RightToLeft = (RightToLeft)0;
		((Control)lblStartSep0).Size = new Size(12, 19);
		((Control)lblStartSep0).TabIndex = 35;
		((Control)lblStartSep0).Text = ":";
		((Control)lblMidSep3).AutoSize = true;
		((Control)lblMidSep3).BackColor = Color.Transparent;
		((Control)lblMidSep3).Cursor = Cursors.Default;
		((Control)lblMidSep3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMidSep3).ForeColor = SystemColors.ControlText;
		((Control)lblMidSep3).Location = new Point(200, 159);
		((Control)lblMidSep3).Name = "lblMidSep3";
		((Control)lblMidSep3).RightToLeft = (RightToLeft)0;
		((Control)lblMidSep3).Size = new Size(18, 19);
		((Control)lblMidSep3).TabIndex = 34;
		((Control)lblMidSep3).Text = "~";
		((Control)lblMidSep2).AutoSize = true;
		((Control)lblMidSep2).BackColor = Color.Transparent;
		((Control)lblMidSep2).Cursor = Cursors.Default;
		((Control)lblMidSep2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMidSep2).ForeColor = SystemColors.ControlText;
		((Control)lblMidSep2).Location = new Point(200, 131);
		((Control)lblMidSep2).Name = "lblMidSep2";
		((Control)lblMidSep2).RightToLeft = (RightToLeft)0;
		((Control)lblMidSep2).Size = new Size(18, 19);
		((Control)lblMidSep2).TabIndex = 32;
		((Control)lblMidSep2).Text = "~";
		((Control)lblMidSep1).AutoSize = true;
		((Control)lblMidSep1).BackColor = Color.Transparent;
		((Control)lblMidSep1).Cursor = Cursors.Default;
		((Control)lblMidSep1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMidSep1).ForeColor = SystemColors.ControlText;
		((Control)lblMidSep1).Location = new Point(200, 103);
		((Control)lblMidSep1).Name = "lblMidSep1";
		((Control)lblMidSep1).RightToLeft = (RightToLeft)0;
		((Control)lblMidSep1).Size = new Size(18, 19);
		((Control)lblMidSep1).TabIndex = 31;
		((Control)lblMidSep1).Text = "~";
		((Control)lblStartTime).AutoSize = true;
		((Control)lblStartTime).BackColor = Color.Transparent;
		((Control)lblStartTime).Cursor = Cursors.Default;
		((Control)lblStartTime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblStartTime).ForeColor = SystemColors.ControlText;
		((Control)lblStartTime).Location = new Point(90, 50);
		((Control)lblStartTime).Name = "lblStartTime";
		((Control)lblStartTime).RightToLeft = (RightToLeft)0;
		((Control)lblStartTime).Size = new Size(72, 19);
		((Control)lblStartTime).TabIndex = 30;
		((Control)lblStartTime).Text = "Start Time";
		((Control)lblEndTime).AutoSize = true;
		((Control)lblEndTime).BackColor = Color.Transparent;
		((Control)lblEndTime).Cursor = Cursors.Default;
		((Control)lblEndTime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEndTime).ForeColor = SystemColors.ControlText;
		((Control)lblEndTime).Location = new Point(258, 50);
		((Control)lblEndTime).Name = "lblEndTime";
		((Control)lblEndTime).RightToLeft = (RightToLeft)0;
		((Control)lblEndTime).Size = new Size(67, 19);
		((Control)lblEndTime).TabIndex = 29;
		((Control)lblEndTime).Text = "End Time";
		((Control)cmdGetDoorState).BackColor = SystemColors.Control;
		((Control)cmdGetDoorState).Cursor = Cursors.Default;
		((Control)cmdGetDoorState).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetDoorState).ForeColor = SystemColors.ControlText;
		((Control)cmdGetDoorState).Location = new Point(282, 433);
		((Control)cmdGetDoorState).Name = "cmdGetDoorState";
		((Control)cmdGetDoorState).RightToLeft = (RightToLeft)0;
		((Control)cmdGetDoorState).Size = new Size(120, 33);
		((Control)cmdGetDoorState).TabIndex = 112;
		((Control)cmdGetDoorState).Text = "Get Door State";
		((ButtonBase)cmdGetDoorState).UseVisualStyleBackColor = false;
		((Control)cmdGetDoorState).Click += cmdGetDoorState_Click;
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 9);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(390, 28);
		((Control)lblMessage).TabIndex = 114;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)Frame3).BackColor = SystemColors.Control;
		((Control)Frame3).Controls.Add((Control)(object)txtGroupNum);
		((Control)Frame3).Controls.Add((Control)(object)txtGroupPassTime2);
		((Control)Frame3).Controls.Add((Control)(object)txtGroupPassTime1);
		((Control)Frame3).Controls.Add((Control)(object)txtGroupPassTime0);
		((Control)Frame3).Controls.Add((Control)(object)cmdGetGroupPassTime);
		((Control)Frame3).Controls.Add((Control)(object)cmdSetGroupPassTime);
		((Control)Frame3).Controls.Add((Control)(object)_lblLabel_7);
		((Control)Frame3).Controls.Add((Control)(object)lblLabel6);
		((Control)Frame3).Controls.Add((Control)(object)lblLabel5);
		((Control)Frame3).Controls.Add((Control)(object)lblLabel4);
		((Control)Frame3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Frame3).ForeColor = SystemColors.ControlText;
		((Control)Frame3).Location = new Point(414, 154);
		((Control)Frame3).Name = "Frame3";
		((Control)Frame3).RightToLeft = (RightToLeft)0;
		((Control)Frame3).Size = new Size(330, 146);
		((Control)Frame3).TabIndex = 116;
		Frame3.TabStop = false;
		((Control)Frame3).Text = "Group Access";
		txtGroupNum.AcceptsReturn = true;
		((Control)txtGroupNum).BackColor = SystemColors.Window;
		((Control)txtGroupNum).Cursor = Cursors.IBeam;
		((Control)txtGroupNum).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupNum).ForeColor = SystemColors.WindowText;
		((Control)txtGroupNum).Location = new Point(141, 24);
		((TextBoxBase)txtGroupNum).MaxLength = 0;
		((Control)txtGroupNum).Name = "txtGroupNum";
		((Control)txtGroupNum).RightToLeft = (RightToLeft)0;
		((Control)txtGroupNum).Size = new Size(34, 27);
		((Control)txtGroupNum).TabIndex = 98;
		((Control)txtGroupNum).Text = "1";
		txtGroupNum.TextAlign = (HorizontalAlignment)2;
		txtGroupPassTime2.AcceptsReturn = true;
		((Control)txtGroupPassTime2).BackColor = SystemColors.Window;
		((Control)txtGroupPassTime2).Cursor = Cursors.IBeam;
		((Control)txtGroupPassTime2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupPassTime2).ForeColor = SystemColors.WindowText;
		((Control)txtGroupPassTime2).Location = new Point(233, 63);
		((TextBoxBase)txtGroupPassTime2).MaxLength = 0;
		((Control)txtGroupPassTime2).Name = "txtGroupPassTime2";
		((Control)txtGroupPassTime2).RightToLeft = (RightToLeft)0;
		((Control)txtGroupPassTime2).Size = new Size(26, 28);
		((Control)txtGroupPassTime2).TabIndex = 94;
		((Control)txtGroupPassTime2).Text = "0";
		txtGroupPassTime2.TextAlign = (HorizontalAlignment)2;
		txtGroupPassTime1.AcceptsReturn = true;
		((Control)txtGroupPassTime1).BackColor = SystemColors.Window;
		((Control)txtGroupPassTime1).Cursor = Cursors.IBeam;
		((Control)txtGroupPassTime1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupPassTime1).ForeColor = SystemColors.WindowText;
		((Control)txtGroupPassTime1).Location = new Point(149, 64);
		((TextBoxBase)txtGroupPassTime1).MaxLength = 0;
		((Control)txtGroupPassTime1).Name = "txtGroupPassTime1";
		((Control)txtGroupPassTime1).RightToLeft = (RightToLeft)0;
		((Control)txtGroupPassTime1).Size = new Size(26, 28);
		((Control)txtGroupPassTime1).TabIndex = 93;
		((Control)txtGroupPassTime1).Text = "0";
		txtGroupPassTime1.TextAlign = (HorizontalAlignment)2;
		txtGroupPassTime0.AcceptsReturn = true;
		((Control)txtGroupPassTime0).BackColor = SystemColors.Window;
		((Control)txtGroupPassTime0).Cursor = Cursors.IBeam;
		((Control)txtGroupPassTime0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtGroupPassTime0).ForeColor = SystemColors.WindowText;
		((Control)txtGroupPassTime0).Location = new Point(60, 64);
		((TextBoxBase)txtGroupPassTime0).MaxLength = 0;
		((Control)txtGroupPassTime0).Name = "txtGroupPassTime0";
		((Control)txtGroupPassTime0).RightToLeft = (RightToLeft)0;
		((Control)txtGroupPassTime0).Size = new Size(26, 28);
		((Control)txtGroupPassTime0).TabIndex = 92;
		((Control)txtGroupPassTime0).Text = "0";
		txtGroupPassTime0.TextAlign = (HorizontalAlignment)2;
		((Control)cmdGetGroupPassTime).BackColor = SystemColors.Control;
		((Control)cmdGetGroupPassTime).Cursor = Cursors.Default;
		((Control)cmdGetGroupPassTime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetGroupPassTime).ForeColor = SystemColors.ControlText;
		((Control)cmdGetGroupPassTime).Location = new Point(42, 104);
		((Control)cmdGetGroupPassTime).Name = "cmdGetGroupPassTime";
		((Control)cmdGetGroupPassTime).RightToLeft = (RightToLeft)0;
		((Control)cmdGetGroupPassTime).Size = new Size(120, 31);
		((Control)cmdGetGroupPassTime).TabIndex = 74;
		((Control)cmdGetGroupPassTime).Text = "Get Group";
		((ButtonBase)cmdGetGroupPassTime).UseVisualStyleBackColor = false;
		((Control)cmdGetGroupPassTime).Click += cmdGetGroupPassTime_Click;
		((Control)cmdSetGroupPassTime).BackColor = SystemColors.Control;
		((Control)cmdSetGroupPassTime).Cursor = Cursors.Default;
		((Control)cmdSetGroupPassTime).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetGroupPassTime).ForeColor = SystemColors.ControlText;
		((Control)cmdSetGroupPassTime).Location = new Point(178, 104);
		((Control)cmdSetGroupPassTime).Name = "cmdSetGroupPassTime";
		((Control)cmdSetGroupPassTime).RightToLeft = (RightToLeft)0;
		((Control)cmdSetGroupPassTime).Size = new Size(120, 31);
		((Control)cmdSetGroupPassTime).TabIndex = 73;
		((Control)cmdSetGroupPassTime).Text = "Set Group";
		((ButtonBase)cmdSetGroupPassTime).UseVisualStyleBackColor = false;
		((Control)cmdSetGroupPassTime).Click += cmdSetGroupPassTime_Click;
		((Control)_lblLabel_7).BackColor = SystemColors.Control;
		((Control)_lblLabel_7).Cursor = Cursors.Default;
		((Control)_lblLabel_7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)_lblLabel_7).ForeColor = SystemColors.ControlText;
		((Control)_lblLabel_7).Location = new Point(80, 16);
		((Control)_lblLabel_7).Name = "_lblLabel_7";
		((Control)_lblLabel_7).RightToLeft = (RightToLeft)0;
		((Control)_lblLabel_7).Size = new Size(56, 40);
		((Control)_lblLabel_7).TabIndex = 104;
		((Control)_lblLabel_7).Text = "Group (1~5)";
		((Control)lblLabel6).BackColor = SystemColors.Control;
		((Control)lblLabel6).Cursor = Cursors.Default;
		((Control)lblLabel6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel6).ForeColor = SystemColors.ControlText;
		((Control)lblLabel6).Location = new Point(191, 67);
		((Control)lblLabel6).Name = "lblLabel6";
		((Control)lblLabel6).RightToLeft = (RightToLeft)0;
		((Control)lblLabel6).Size = new Size(41, 24);
		((Control)lblLabel6).TabIndex = 97;
		((Control)lblLabel6).Text = "TZ3";
		lblLabel6.TextAlign = (ContentAlignment)4;
		((Control)lblLabel5).BackColor = SystemColors.Control;
		((Control)lblLabel5).Cursor = Cursors.Default;
		((Control)lblLabel5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel5).ForeColor = SystemColors.ControlText;
		((Control)lblLabel5).Location = new Point(106, 67);
		((Control)lblLabel5).Name = "lblLabel5";
		((Control)lblLabel5).RightToLeft = (RightToLeft)0;
		((Control)lblLabel5).Size = new Size(42, 24);
		((Control)lblLabel5).TabIndex = 96;
		((Control)lblLabel5).Text = "TZ2";
		lblLabel5.TextAlign = (ContentAlignment)4;
		((Control)lblLabel4).BackColor = SystemColors.Control;
		((Control)lblLabel4).Cursor = Cursors.Default;
		((Control)lblLabel4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblLabel4).ForeColor = SystemColors.ControlText;
		((Control)lblLabel4).Location = new Point(16, 67);
		((Control)lblLabel4).Name = "lblLabel4";
		((Control)lblLabel4).RightToLeft = (RightToLeft)0;
		((Control)lblLabel4).Size = new Size(42, 24);
		((Control)lblLabel4).TabIndex = 95;
		((Control)lblLabel4).Text = "TZ1";
		lblLabel4.TextAlign = (ContentAlignment)4;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(756, 489);
		((Control)this).Controls.Add((Control)(object)Frame4);
		((Control)this).Controls.Add((Control)(object)cmbDoorState);
		((Control)this).Controls.Add((Control)(object)Frame2);
		((Control)this).Controls.Add((Control)(object)cmdSetDoorState);
		((Control)this).Controls.Add((Control)(object)Frame1);
		((Control)this).Controls.Add((Control)(object)cmdGetDoorState);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)Frame3);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmPasstimeInfo";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Pass Time Info";
		((Form)this).Load += frmPasstimeInfo_Load;
		((Control)Frame4).ResumeLayout(false);
		((Control)Frame2).ResumeLayout(false);
		((Control)Frame1).ResumeLayout(false);
		((Control)Frame1).PerformLayout();
		((Control)Frame3).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void frmPasstimeInfo_Load(object sender, EventArgs e)
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Expected O, but got Unknown
		mPassTimeInfo.PassCtrlTime = new PASSTIME[7];
		for (int i = 0; i < 7; i++)
		{
			mPassTimeInfo.PassCtrlTime[i].StartHour = 0;
			mPassTimeInfo.PassCtrlTime[i].StartMinute = 0;
			mPassTimeInfo.PassCtrlTime[i].EndHour = 0;
			mPassTimeInfo.PassCtrlTime[i].EndMinute = 0;
		}
		mUserPassInfo.UserPassID = new byte[3];
		mGroupPassInfo.GroupPassID = new byte[3];
		mGroupMatchInfo.GroupMatch = new short[10];
		foreach (object item in (ArrangedElementCollection)((Control)Frame1).Controls)
		{
			if (item == null)
			{
				continue;
			}
			string name = ((Control)item).Name;
			string text = "txtStartHour";
			int length = text.Length;
			if (name.Length > length && name.Substring(0, length) == text)
			{
				int i = Convert.ToInt32(name.Substring(length, name.Length - length));
				mtxtStartHour[i] = (TextBox)item;
				continue;
			}
			text = "txtStartMinute";
			length = text.Length;
			if (name.Length > length && name.Substring(0, length) == text)
			{
				int i = Convert.ToInt32(name.Substring(length, name.Length - length));
				mtxtStartMinute[i] = (TextBox)item;
				continue;
			}
			text = "txtEndHour";
			length = text.Length;
			if (name.Length > length && name.Substring(0, length) == text)
			{
				int i = Convert.ToInt32(name.Substring(length, name.Length - length));
				mtxtEndHour[i] = (TextBox)item;
				continue;
			}
			text = "txtEndMinute";
			length = text.Length;
			if (name.Length > length && name.Substring(0, length) == text)
			{
				int i = Convert.ToInt32(name.Substring(length, name.Length - length));
				mtxtEndMinute[i] = (TextBox)item;
			}
		}
		foreach (object item2 in (ArrangedElementCollection)((Control)Frame2).Controls)
		{
			if (item2 != null)
			{
				string name = ((Control)item2).Name;
				string text = "txtUserPassTime";
				int length = text.Length;
				if (name.Length > length && name.Substring(0, length) == text)
				{
					int i = Convert.ToInt32(name.Substring(length, name.Length - length));
					mtxtUserPassTime[i] = (TextBox)item2;
				}
			}
		}
		foreach (object item3 in (ArrangedElementCollection)((Control)Frame3).Controls)
		{
			if (item3 != null)
			{
				string name = ((Control)item3).Name;
				string text = "txtGroupPassTime";
				int length = text.Length;
				if (name.Length > length && name.Substring(0, length) == text)
				{
					int i = Convert.ToInt32(name.Substring(length, name.Length - length));
					mtxtGroupPassTime[i] = (TextBox)item3;
				}
			}
		}
		foreach (object item4 in (ArrangedElementCollection)((Control)Frame4).Controls)
		{
			if (item4 != null)
			{
				string name = ((Control)item4).Name;
				string text = "txtGroupMatch";
				int length = text.Length;
				if (name.Length > length && name.Substring(0, length) == text)
				{
					int i = Convert.ToInt32(name.Substring(length, name.Length - length));
					mtxtGroupMatch[i] = (TextBox)item4;
				}
			}
		}
		ShowValue();
		((ListControl)cmbDoorState).SelectedIndex = 0;
	}

	private void cmdRead_Click(object sender, EventArgs e)
	{
		int num = 1;
		int num2 = Marshal.SizeOf(typeof(PASSCTRLTIME));
		byte[] array = new byte[num2];
		((Control)cmdRead).Enabled = false;
		((Control)lblMessage).Text = "";
		Application.DoEvents();
		int num3 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num3 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdRead).Enabled = true;
			return;
		}
		if (FKAttendDLL.IsNumeric(((Control)txtPassTimeID).Text))
		{
			num = Convert.ToInt32(((Control)txtPassTimeID).Text);
		}
		if (num < 1 || num > 50)
		{
			num = 1;
		}
		((Control)txtPassTimeID).Text = Convert.ToString(num).Trim();
		num3 = FKAttendDLL.FK_GetPassTime(FKAttendDLL.nCommHandleIndex, num, array, num2);
		if (num3 == 1)
		{
			((Control)lblMessage).Text = "Success!";
			mPassTimeInfo = (PASSCTRLTIME)FKAttendDLL.ConvertByteArrayToStructure(array, typeof(PASSCTRLTIME));
			ShowValue();
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num3);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdRead).Enabled = true;
	}

	private void cmdWrite_Click(object sender, EventArgs e)
	{
		int num = Marshal.SizeOf(typeof(PASSCTRLTIME));
		byte[] array = new byte[num];
		((Control)cmdWrite).Enabled = false;
		((Control)lblMessage).Text = "";
		Application.DoEvents();
		int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdWrite).Enabled = true;
			return;
		}
		GetValue();
		int num3 = 1;
		if (FKAttendDLL.IsNumeric(((Control)txtPassTimeID).Text))
		{
			num3 = Convert.ToInt32(((Control)txtPassTimeID).Text);
		}
		if (num3 < 1 || num3 > 50)
		{
			num3 = 1;
		}
		((Control)txtPassTimeID).Text = num3.ToString().Trim();
		FKAttendDLL.ConvertStructureToByteArray(mPassTimeInfo, array);
		num2 = FKAttendDLL.FK_SetPassTime(FKAttendDLL.nCommHandleIndex, num3, array, num);
		if (num2 == 1)
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num2);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdWrite).Enabled = true;
	}

	private void cmdSetDoorState_Click(object sender, EventArgs e)
	{
		int anStatusVal = -1;
		((Control)cmdSetDoorState).Enabled = false;
		((Control)lblMessage).Text = "Setting Door ...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetDoorState).Enabled = true;
			return;
		}
		switch (((ListControl)cmbDoorState).SelectedIndex)
		{
		case 0:
			anStatusVal = 0;
			break;
		case 1:
			anStatusVal = 1;
			break;
		case 2:
			anStatusVal = 2;
			break;
		case 3:
			anStatusVal = 3;
			break;
		}
		num = FKAttendDLL.FK_SetDoorStatus(FKAttendDLL.nCommHandleIndex, anStatusVal);
		if (num == 1)
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetDoorState).Enabled = true;
	}

	private void cmdGetDoorState_Click(object sender, EventArgs e)
	{
		int apnStatusVal = -1;
		((Control)cmdGetDoorState).Enabled = false;
		((Control)lblMessage).Text = "Getting Door State...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetDoorState).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_GetDoorStatus(FKAttendDLL.nCommHandleIndex, ref apnStatusVal);
		if (num == 1)
		{
			switch (apnStatusVal)
			{
			case 0:
				((Control)lblMessage).Text = "State Reset!";
				break;
			case 1:
				((Control)lblMessage).Text = "Door Open!";
				break;
			case 2:
				((Control)lblMessage).Text = "Door Close!";
				break;
			case 3:
				((Control)lblMessage).Text = "Command Open... Door Close!";
				break;
			default:
				((Control)lblMessage).Text = "Door - Unknown!";
				break;
			}
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetDoorState).Enabled = true;
	}

	private void cmdGetUserPasstime_Click(object sender, EventArgs e)
	{
		int apnGroupID = 0;
		int num = Marshal.SizeOf(typeof(USERPASSINFO));
		byte[] array = new byte[num];
		string text = "";
		((Control)cmdGetUserPasstime).Enabled = false;
		((Control)lblMessage).Text = "Getting...";
		Application.DoEvents();
		int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetUserPasstime).Enabled = true;
			return;
		}
		int anEnrollNumber = Convert.ToInt32(((Control)txtUserID).Text);
		text = new string(' ', 256);
		num2 = FKAttendDLL.FK_GetUserPassTime(FKAttendDLL.nCommHandleIndex, anEnrollNumber, ref apnGroupID, array, num);
		if (num2 == 1)
		{
			((Control)txtGroupID).Text = apnGroupID.ToString();
			((Control)lblMessage).Text = "Success!";
			mUserPassInfo = (USERPASSINFO)FKAttendDLL.ConvertByteArrayToStructure(array, typeof(USERPASSINFO));
			ShowValue();
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num2);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetUserPasstime).Enabled = true;
	}

	private void cmdSetUserPassTime_Click(object sender, EventArgs e)
	{
		int num = Marshal.SizeOf(typeof(USERPASSINFO));
		byte[] array = new byte[num];
		((Control)cmdSetUserPassTime).Enabled = false;
		((Control)lblMessage).Text = "Setting...";
		Application.DoEvents();
		int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetUserPassTime).Enabled = true;
			return;
		}
		GetValue();
		FKAttendDLL.ConvertStructureToByteArray(mUserPassInfo, array);
		int anEnrollNumber = Convert.ToInt32(((Control)txtUserID).Text);
		int anGroupID = Convert.ToInt32(((Control)txtGroupID).Text);
		num2 = FKAttendDLL.FK_SetUserPassTime(FKAttendDLL.nCommHandleIndex, anEnrollNumber, anGroupID, array, num);
		if (num2 == 1)
		{
			((Control)lblMessage).Text = "Success!";
			ShowValue();
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num2);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetUserPassTime).Enabled = true;
	}

	private void cmdGetGroupPassTime_Click(object sender, EventArgs e)
	{
		int num = Marshal.SizeOf(typeof(GROUPPASSINFO));
		byte[] array = new byte[num];
		((Control)cmdGetGroupPassTime).Enabled = false;
		((Control)lblMessage).Text = "Getting...";
		Application.DoEvents();
		int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetGroupPassTime).Enabled = true;
			return;
		}
		int anGroupID = Convert.ToInt32(((Control)txtGroupNum).Text);
		num2 = FKAttendDLL.FK_GetGroupPassTime(FKAttendDLL.nCommHandleIndex, anGroupID, array, num);
		if (num2 == 1)
		{
			((Control)lblMessage).Text = "Success!";
			mGroupPassInfo = (GROUPPASSINFO)FKAttendDLL.ConvertByteArrayToStructure(array, typeof(GROUPPASSINFO));
			ShowValue();
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num2);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetGroupPassTime).Enabled = true;
	}

	private void cmdSetGroupPassTime_Click(object sender, EventArgs e)
	{
		int num = Marshal.SizeOf(typeof(GROUPPASSINFO));
		byte[] array = new byte[num];
		((Control)cmdSetGroupPassTime).Enabled = false;
		((Control)lblMessage).Text = "Setting...";
		Application.DoEvents();
		int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetGroupPassTime).Enabled = true;
			return;
		}
		GetValue();
		FKAttendDLL.ConvertStructureToByteArray(mGroupPassInfo, array);
		int anGroupID = Convert.ToInt32(((Control)txtGroupNum).Text);
		num2 = FKAttendDLL.FK_SetGroupPassTime(FKAttendDLL.nCommHandleIndex, anGroupID, array, num);
		if (num2 == 1)
		{
			((Control)lblMessage).Text = "Success!";
			ShowValue();
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num2);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetGroupPassTime).Enabled = true;
	}

	private void cmdGetGroupMatch_Click(object sender, EventArgs e)
	{
		int num = Marshal.SizeOf(typeof(GROUPMATCHINFO));
		byte[] array = new byte[num];
		((Control)cmdGetGroupMatch).Enabled = false;
		((Control)lblMessage).Text = "Getting...";
		Application.DoEvents();
		int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetGroupMatch).Enabled = true;
			return;
		}
		num2 = FKAttendDLL.FK_GetGroupMatch(FKAttendDLL.nCommHandleIndex, array, num);
		if (num2 == 1)
		{
			((Control)lblMessage).Text = "Success!";
			mGroupMatchInfo = (GROUPMATCHINFO)FKAttendDLL.ConvertByteArrayToStructure(array, typeof(GROUPMATCHINFO));
			ShowValue();
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num2);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetGroupMatch).Enabled = true;
	}

	private void cmdSetGroupMatch_Click(object sender, EventArgs e)
	{
		int num = Marshal.SizeOf(typeof(GROUPMATCHINFO));
		byte[] array = new byte[num];
		((Control)cmdSetGroupMatch).Enabled = false;
		((Control)lblMessage).Text = "Setting...";
		Application.DoEvents();
		int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetGroupMatch).Enabled = true;
			return;
		}
		GetValue();
		FKAttendDLL.ConvertStructureToByteArray(mGroupMatchInfo, array);
		num2 = FKAttendDLL.FK_SetGroupMatch(FKAttendDLL.nCommHandleIndex, array, num);
		if (num2 == 1)
		{
			((Control)lblMessage).Text = "Success!";
			ShowValue();
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num2);
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetGroupMatch).Enabled = true;
	}

	private void ShowValue()
	{
		for (int i = 0; i < 7; i++)
		{
			((Control)mtxtStartHour[i]).Text = mPassTimeInfo.PassCtrlTime[i].StartHour.ToString();
			((Control)mtxtStartMinute[i]).Text = mPassTimeInfo.PassCtrlTime[i].StartMinute.ToString();
			((Control)mtxtEndHour[i]).Text = mPassTimeInfo.PassCtrlTime[i].EndHour.ToString();
			((Control)mtxtEndMinute[i]).Text = mPassTimeInfo.PassCtrlTime[i].EndMinute.ToString();
		}
		for (int i = 0; i < 3; i++)
		{
			((Control)mtxtUserPassTime[i]).Text = mUserPassInfo.UserPassID[i].ToString();
		}
		for (int i = 0; i < 3; i++)
		{
			((Control)mtxtGroupPassTime[i]).Text = mGroupPassInfo.GroupPassID[i].ToString();
		}
		for (int i = 0; i < 10; i++)
		{
			((Control)mtxtGroupMatch[i]).Text = mGroupMatchInfo.GroupMatch[i].ToString();
		}
	}

	private void GetValue()
	{
		for (int i = 0; i < 7; i++)
		{
			mPassTimeInfo.PassCtrlTime[i].StartHour = Convert.ToByte(((Control)mtxtStartHour[i]).Text);
			mPassTimeInfo.PassCtrlTime[i].StartMinute = Convert.ToByte(((Control)mtxtStartMinute[i]).Text);
			mPassTimeInfo.PassCtrlTime[i].EndHour = Convert.ToByte(((Control)mtxtEndHour[i]).Text);
			mPassTimeInfo.PassCtrlTime[i].EndMinute = Convert.ToByte(((Control)mtxtEndMinute[i]).Text);
		}
		for (int i = 0; i < 3; i++)
		{
			mUserPassInfo.UserPassID[i] = Convert.ToByte(((Control)mtxtUserPassTime[i]).Text);
		}
		for (int i = 0; i < 3; i++)
		{
			mGroupPassInfo.GroupPassID[i] = Convert.ToByte(((Control)mtxtGroupPassTime[i]).Text);
		}
		for (int i = 0; i < 10; i++)
		{
			mGroupMatchInfo.GroupMatch[i] = Convert.ToByte(((Control)mtxtGroupMatch[i]).Text);
		}
	}
}
