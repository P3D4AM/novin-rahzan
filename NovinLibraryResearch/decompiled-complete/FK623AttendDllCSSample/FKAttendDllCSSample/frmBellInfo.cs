using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace FKAttendDllCSSample;

public class frmBellInfo : Form
{
	public Label Label4;

	public Label Label3;

	public TextBox txtHour23;

	public TextBox txtMinute23;

	public TextBox txtMinute22;

	public TextBox txtHour22;

	public TextBox txtHour21;

	public TextBox txtMinute21;

	public TextBox txtMinute20;

	public TextBox txtHour20;

	public TextBox txtHour19;

	public TextBox txtMinute19;

	public TextBox txtMinute18;

	public TextBox txtHour18;

	public TextBox txtHour17;

	public TextBox txtMinute17;

	public TextBox txtMinute16;

	public TextBox txtHour16;

	public CheckBox chkValid23;

	public CheckBox chkValid22;

	public CheckBox chkValid21;

	public CheckBox chkValid20;

	public CheckBox chkValid19;

	public CheckBox chkValid18;

	public CheckBox chkValid17;

	public CheckBox chkValid16;

	public Label Label2_40;

	public Label Label2_47;

	public Label Label2_46;

	public Label Label2_45;

	public Label Label2_44;

	public Label Label2_43;

	public Label Label2_42;

	public Label Label2_41;

	public Label Label2_39;

	public Label Label2_38;

	public Label Label2_37;

	public Label Label2_36;

	public Label Label2_35;

	public Label Label2_34;

	public Label Label2_33;

	public Label Label2_32;

	public TextBox txtHour15;

	public TextBox txtMinute15;

	public TextBox txtMinute14;

	public TextBox txtHour14;

	public TextBox txtHour13;

	public TextBox txtMinute13;

	public TextBox txtMinute12;

	public TextBox txtHour12;

	public TextBox txtHour11;

	public TextBox txtMinute11;

	public TextBox txtMinute10;

	public TextBox txtHour10;

	public TextBox txtHour9;

	public TextBox txtMinute9;

	public TextBox txtMinute8;

	public TextBox txtHour8;

	public CheckBox chkValid15;

	public CheckBox chkValid14;

	public CheckBox chkValid13;

	public CheckBox chkValid12;

	public CheckBox chkValid11;

	public CheckBox chkValid10;

	public CheckBox chkValid9;

	public CheckBox chkValid8;

	public Label Label2_24;

	public Label Label2_31;

	public Label Label2_30;

	public Label Label2_29;

	public Label Label2_28;

	public Label Label2_27;

	public Label Label2_26;

	public Label Label2_25;

	public Label Label2_23;

	public Label Label2_22;

	public Label Label2_21;

	public Label Label2_20;

	public Label Label2_19;

	public Label Label2_18;

	public Label Label2_17;

	public Label Label2_16;

	public TextBox txtBellCount;

	public TextBox txtHour7;

	public TextBox txtMinute7;

	public TextBox txtMinute6;

	public TextBox txtHour6;

	public TextBox txtHour5;

	public TextBox txtMinute5;

	public TextBox txtMinute4;

	public TextBox txtHour4;

	public TextBox txtHour3;

	public TextBox txtMinute3;

	public TextBox txtMinute2;

	public TextBox txtHour2;

	public TextBox txtHour1;

	public TextBox txtMinute1;

	public TextBox txtMinute0;

	public TextBox txtHour0;

	public CheckBox chkValid7;

	public CheckBox chkValid6;

	public CheckBox chkValid5;

	public CheckBox chkValid4;

	public CheckBox chkValid3;

	public CheckBox chkValid2;

	public CheckBox chkValid1;

	public CheckBox chkValid0;

	public Button cmdWrite;

	public Button cmdRead;

	public Label Label2_8;

	public Label Label2_15;

	public Label Label2_14;

	public Label Label2_13;

	public Label Label2_12;

	public Label Label2_11;

	public Label Label2_10;

	public Label Label2_9;

	public Label lblCaption0;

	public Label Label2_7;

	public Label Label2_6;

	public Label Label2_5;

	public Label Label2_4;

	public Label Label2_3;

	public Label Label2_2;

	public Label Label2_1;

	public Label Label2_0;

	public Label Label1;

	public Label lblMessage;

	private Container components = null;

	private int mnBellCount;

	private BELLINFO mBellInfo;

	private CheckBox[] fchkValid = (CheckBox[])(object)new CheckBox[24];

	private TextBox[] ftxtHour = (TextBox[])(object)new TextBox[24];

	private TextBox[] ftxtMinute = (TextBox[])(object)new TextBox[24];

	public frmBellInfo()
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
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Expected O, but got Unknown
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Expected O, but got Unknown
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Expected O, but got Unknown
		//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Expected O, but got Unknown
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Expected O, but got Unknown
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		//IL_0cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdc: Expected O, but got Unknown
		//IL_0d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Expected O, but got Unknown
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e70: Expected O, but got Unknown
		//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3a: Expected O, but got Unknown
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1004: Expected O, but got Unknown
		//IL_10c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Expected O, but got Unknown
		//IL_118b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1195: Expected O, but got Unknown
		//IL_1252: Unknown result type (might be due to invalid IL or missing references)
		//IL_125c: Expected O, but got Unknown
		//IL_1319: Unknown result type (might be due to invalid IL or missing references)
		//IL_1323: Expected O, but got Unknown
		//IL_13d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13dd: Expected O, but got Unknown
		//IL_14a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ab: Expected O, but got Unknown
		//IL_156f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1579: Expected O, but got Unknown
		//IL_163d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1647: Expected O, but got Unknown
		//IL_170b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1715: Expected O, but got Unknown
		//IL_17d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e3: Expected O, but got Unknown
		//IL_18a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b1: Expected O, but got Unknown
		//IL_1972: Unknown result type (might be due to invalid IL or missing references)
		//IL_197c: Expected O, but got Unknown
		//IL_1a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a47: Expected O, but got Unknown
		//IL_1b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b12: Expected O, but got Unknown
		//IL_1bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be0: Expected O, but got Unknown
		//IL_1ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cae: Expected O, but got Unknown
		//IL_1d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7c: Expected O, but got Unknown
		//IL_1e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4a: Expected O, but got Unknown
		//IL_1f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f18: Expected O, but got Unknown
		//IL_1fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe6: Expected O, but got Unknown
		//IL_20a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b1: Expected O, but got Unknown
		//IL_2168: Unknown result type (might be due to invalid IL or missing references)
		//IL_2172: Expected O, but got Unknown
		//IL_2229: Unknown result type (might be due to invalid IL or missing references)
		//IL_2233: Expected O, but got Unknown
		//IL_22ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f4: Expected O, but got Unknown
		//IL_23ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b5: Expected O, but got Unknown
		//IL_246c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2476: Expected O, but got Unknown
		//IL_252d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2537: Expected O, but got Unknown
		//IL_25eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f5: Expected O, but got Unknown
		//IL_26b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c0: Expected O, but got Unknown
		//IL_2780: Unknown result type (might be due to invalid IL or missing references)
		//IL_278a: Expected O, but got Unknown
		//IL_284a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2854: Expected O, but got Unknown
		//IL_2914: Unknown result type (might be due to invalid IL or missing references)
		//IL_291e: Expected O, but got Unknown
		//IL_29de: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e8: Expected O, but got Unknown
		//IL_2aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab2: Expected O, but got Unknown
		//IL_2b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7c: Expected O, but got Unknown
		//IL_2c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c46: Expected O, but got Unknown
		//IL_2d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d10: Expected O, but got Unknown
		//IL_2dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dda: Expected O, but got Unknown
		//IL_2e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ea4: Expected O, but got Unknown
		//IL_2f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6e: Expected O, but got Unknown
		//IL_302e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3038: Expected O, but got Unknown
		//IL_30f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ff: Expected O, but got Unknown
		//IL_31bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c6: Expected O, but got Unknown
		//IL_3283: Unknown result type (might be due to invalid IL or missing references)
		//IL_328d: Expected O, but got Unknown
		//IL_333d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3347: Expected O, but got Unknown
		//IL_340b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3415: Expected O, but got Unknown
		//IL_34d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e3: Expected O, but got Unknown
		//IL_35a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b1: Expected O, but got Unknown
		//IL_3675: Unknown result type (might be due to invalid IL or missing references)
		//IL_367f: Expected O, but got Unknown
		//IL_3743: Unknown result type (might be due to invalid IL or missing references)
		//IL_374d: Expected O, but got Unknown
		//IL_3811: Unknown result type (might be due to invalid IL or missing references)
		//IL_381b: Expected O, but got Unknown
		//IL_38dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_38e6: Expected O, but got Unknown
		//IL_39a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b1: Expected O, but got Unknown
		//IL_3a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7c: Expected O, but got Unknown
		//IL_3b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b4a: Expected O, but got Unknown
		//IL_3c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c18: Expected O, but got Unknown
		//IL_3cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce6: Expected O, but got Unknown
		//IL_3daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3db4: Expected O, but got Unknown
		//IL_3e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e82: Expected O, but got Unknown
		//IL_3f46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f50: Expected O, but got Unknown
		//IL_4011: Unknown result type (might be due to invalid IL or missing references)
		//IL_401b: Expected O, but got Unknown
		//IL_40d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_40dc: Expected O, but got Unknown
		//IL_4193: Unknown result type (might be due to invalid IL or missing references)
		//IL_419d: Expected O, but got Unknown
		//IL_4254: Unknown result type (might be due to invalid IL or missing references)
		//IL_425e: Expected O, but got Unknown
		//IL_4315: Unknown result type (might be due to invalid IL or missing references)
		//IL_431f: Expected O, but got Unknown
		//IL_43d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_43e0: Expected O, but got Unknown
		//IL_4497: Unknown result type (might be due to invalid IL or missing references)
		//IL_44a1: Expected O, but got Unknown
		//IL_4555: Unknown result type (might be due to invalid IL or missing references)
		//IL_455f: Expected O, but got Unknown
		//IL_4620: Unknown result type (might be due to invalid IL or missing references)
		//IL_462a: Expected O, but got Unknown
		//IL_46ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_46f4: Expected O, but got Unknown
		//IL_47b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_47bb: Expected O, but got Unknown
		//IL_487b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4885: Expected O, but got Unknown
		//IL_4945: Unknown result type (might be due to invalid IL or missing references)
		//IL_494f: Expected O, but got Unknown
		//IL_4a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a16: Expected O, but got Unknown
		//IL_4ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4add: Expected O, but got Unknown
		//IL_4b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ba7: Expected O, but got Unknown
		//IL_4c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c71: Expected O, but got Unknown
		//IL_4d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d38: Expected O, but got Unknown
		//IL_4df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dff: Expected O, but got Unknown
		//IL_4ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ec9: Expected O, but got Unknown
		//IL_4f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f93: Expected O, but got Unknown
		//IL_5050: Unknown result type (might be due to invalid IL or missing references)
		//IL_505a: Expected O, but got Unknown
		//IL_5114: Unknown result type (might be due to invalid IL or missing references)
		//IL_511e: Expected O, but got Unknown
		//IL_51db: Unknown result type (might be due to invalid IL or missing references)
		//IL_51e5: Expected O, but got Unknown
		//IL_52a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ac: Expected O, but got Unknown
		//IL_5359: Unknown result type (might be due to invalid IL or missing references)
		//IL_5363: Expected O, but got Unknown
		//IL_5424: Unknown result type (might be due to invalid IL or missing references)
		//IL_542e: Expected O, but got Unknown
		//IL_54ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f9: Expected O, but got Unknown
		//IL_55ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c4: Expected O, but got Unknown
		//IL_5685: Unknown result type (might be due to invalid IL or missing references)
		//IL_568f: Expected O, but got Unknown
		//IL_5750: Unknown result type (might be due to invalid IL or missing references)
		//IL_575a: Expected O, but got Unknown
		//IL_581b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5825: Expected O, but got Unknown
		//IL_58e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ed: Expected O, but got Unknown
		//IL_59ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_59b5: Expected O, but got Unknown
		//IL_5a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a9b: Expected O, but got Unknown
		//IL_5b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b81: Expected O, but got Unknown
		//IL_5c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c4c: Expected O, but got Unknown
		//IL_5d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d1a: Expected O, but got Unknown
		//IL_5dde: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de8: Expected O, but got Unknown
		//IL_5eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eb6: Expected O, but got Unknown
		//IL_5f7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f84: Expected O, but got Unknown
		//IL_6048: Unknown result type (might be due to invalid IL or missing references)
		//IL_6052: Expected O, but got Unknown
		//IL_6116: Unknown result type (might be due to invalid IL or missing references)
		//IL_6120: Expected O, but got Unknown
		//IL_61e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_61eb: Expected O, but got Unknown
		//IL_629f: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a9: Expected O, but got Unknown
		//IL_635d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6367: Expected O, but got Unknown
		//IL_641b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6425: Expected O, but got Unknown
		//IL_64d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_64e3: Expected O, but got Unknown
		//IL_6597: Unknown result type (might be due to invalid IL or missing references)
		//IL_65a1: Expected O, but got Unknown
		//IL_6655: Unknown result type (might be due to invalid IL or missing references)
		//IL_665f: Expected O, but got Unknown
		//IL_6713: Unknown result type (might be due to invalid IL or missing references)
		//IL_671d: Expected O, but got Unknown
		//IL_67ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_67d8: Expected O, but got Unknown
		//IL_6889: Unknown result type (might be due to invalid IL or missing references)
		//IL_6893: Expected O, but got Unknown
		//IL_6957: Unknown result type (might be due to invalid IL or missing references)
		//IL_6961: Expected O, but got Unknown
		//IL_731e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7328: Expected O, but got Unknown
		Label4 = new Label();
		Label3 = new Label();
		txtHour23 = new TextBox();
		txtMinute23 = new TextBox();
		txtMinute22 = new TextBox();
		txtHour22 = new TextBox();
		txtHour21 = new TextBox();
		txtMinute21 = new TextBox();
		txtMinute20 = new TextBox();
		txtHour20 = new TextBox();
		txtHour19 = new TextBox();
		txtMinute19 = new TextBox();
		txtMinute18 = new TextBox();
		txtHour18 = new TextBox();
		txtHour17 = new TextBox();
		txtMinute17 = new TextBox();
		txtMinute16 = new TextBox();
		txtHour16 = new TextBox();
		chkValid23 = new CheckBox();
		chkValid22 = new CheckBox();
		chkValid21 = new CheckBox();
		chkValid20 = new CheckBox();
		chkValid19 = new CheckBox();
		chkValid18 = new CheckBox();
		chkValid17 = new CheckBox();
		chkValid16 = new CheckBox();
		Label2_40 = new Label();
		Label2_47 = new Label();
		Label2_46 = new Label();
		Label2_45 = new Label();
		Label2_44 = new Label();
		Label2_43 = new Label();
		Label2_42 = new Label();
		Label2_41 = new Label();
		Label2_39 = new Label();
		Label2_38 = new Label();
		Label2_37 = new Label();
		Label2_36 = new Label();
		Label2_35 = new Label();
		Label2_34 = new Label();
		Label2_33 = new Label();
		Label2_32 = new Label();
		txtHour15 = new TextBox();
		txtMinute15 = new TextBox();
		txtMinute14 = new TextBox();
		txtHour14 = new TextBox();
		txtHour13 = new TextBox();
		txtMinute13 = new TextBox();
		txtMinute12 = new TextBox();
		txtHour12 = new TextBox();
		txtHour11 = new TextBox();
		txtMinute11 = new TextBox();
		txtMinute10 = new TextBox();
		txtHour10 = new TextBox();
		txtHour9 = new TextBox();
		txtMinute9 = new TextBox();
		txtMinute8 = new TextBox();
		txtHour8 = new TextBox();
		chkValid15 = new CheckBox();
		chkValid14 = new CheckBox();
		chkValid13 = new CheckBox();
		chkValid12 = new CheckBox();
		chkValid11 = new CheckBox();
		chkValid10 = new CheckBox();
		chkValid9 = new CheckBox();
		chkValid8 = new CheckBox();
		Label2_24 = new Label();
		Label2_31 = new Label();
		Label2_30 = new Label();
		Label2_29 = new Label();
		Label2_28 = new Label();
		Label2_27 = new Label();
		Label2_26 = new Label();
		Label2_25 = new Label();
		Label2_23 = new Label();
		Label2_22 = new Label();
		Label2_21 = new Label();
		Label2_20 = new Label();
		Label2_19 = new Label();
		Label2_18 = new Label();
		Label2_17 = new Label();
		Label2_16 = new Label();
		txtBellCount = new TextBox();
		txtHour7 = new TextBox();
		txtMinute7 = new TextBox();
		txtMinute6 = new TextBox();
		txtHour6 = new TextBox();
		txtHour5 = new TextBox();
		txtMinute5 = new TextBox();
		txtMinute4 = new TextBox();
		txtHour4 = new TextBox();
		txtHour3 = new TextBox();
		txtMinute3 = new TextBox();
		txtMinute2 = new TextBox();
		txtHour2 = new TextBox();
		txtHour1 = new TextBox();
		txtMinute1 = new TextBox();
		txtMinute0 = new TextBox();
		txtHour0 = new TextBox();
		chkValid7 = new CheckBox();
		chkValid6 = new CheckBox();
		chkValid5 = new CheckBox();
		chkValid4 = new CheckBox();
		chkValid3 = new CheckBox();
		chkValid2 = new CheckBox();
		chkValid1 = new CheckBox();
		chkValid0 = new CheckBox();
		cmdWrite = new Button();
		cmdRead = new Button();
		Label2_8 = new Label();
		Label2_15 = new Label();
		Label2_14 = new Label();
		Label2_13 = new Label();
		Label2_12 = new Label();
		Label2_11 = new Label();
		Label2_10 = new Label();
		Label2_9 = new Label();
		lblCaption0 = new Label();
		Label2_7 = new Label();
		Label2_6 = new Label();
		Label2_5 = new Label();
		Label2_4 = new Label();
		Label2_3 = new Label();
		Label2_2 = new Label();
		Label2_1 = new Label();
		Label2_0 = new Label();
		Label1 = new Label();
		lblMessage = new Label();
		((Control)this).SuspendLayout();
		((Control)Label4).BackColor = Color.Transparent;
		((Control)Label4).Cursor = Cursors.Default;
		((Control)Label4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label4).ForeColor = SystemColors.ControlText;
		((Control)Label4).Location = new Point(452, 66);
		((Control)Label4).Name = "Label4";
		((Control)Label4).RightToLeft = (RightToLeft)0;
		((Control)Label4).Size = new Size(202, 20);
		((Control)Label4).TabIndex = 257;
		((Control)Label4).Text = "Point   UseFlag   Start Time";
		((Control)Label3).BackColor = Color.Transparent;
		((Control)Label3).Cursor = Cursors.Default;
		((Control)Label3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label3).ForeColor = SystemColors.ControlText;
		((Control)Label3).Location = new Point(232, 66);
		((Control)Label3).Name = "Label3";
		((Control)Label3).RightToLeft = (RightToLeft)0;
		((Control)Label3).Size = new Size(202, 20);
		((Control)Label3).TabIndex = 256;
		((Control)Label3).Text = "Point   UseFlag   Start Time";
		txtHour23.AcceptsReturn = true;
		((Control)txtHour23).BackColor = SystemColors.Window;
		((Control)txtHour23).Cursor = Cursors.IBeam;
		((Control)txtHour23).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour23).ForeColor = SystemColors.WindowText;
		((Control)txtHour23).Location = new Point(546, 287);
		((TextBoxBase)txtHour23).MaxLength = 0;
		((Control)txtHour23).Name = "txtHour23";
		((Control)txtHour23).RightToLeft = (RightToLeft)0;
		((Control)txtHour23).Size = new Size(42, 28);
		((Control)txtHour23).TabIndex = 238;
		txtMinute23.AcceptsReturn = true;
		((Control)txtMinute23).BackColor = SystemColors.Window;
		((Control)txtMinute23).Cursor = Cursors.IBeam;
		((Control)txtMinute23).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute23).ForeColor = SystemColors.WindowText;
		((Control)txtMinute23).Location = new Point(606, 287);
		((TextBoxBase)txtMinute23).MaxLength = 0;
		((Control)txtMinute23).Name = "txtMinute23";
		((Control)txtMinute23).RightToLeft = (RightToLeft)0;
		((Control)txtMinute23).Size = new Size(42, 28);
		((Control)txtMinute23).TabIndex = 239;
		txtMinute22.AcceptsReturn = true;
		((Control)txtMinute22).BackColor = SystemColors.Window;
		((Control)txtMinute22).Cursor = Cursors.IBeam;
		((Control)txtMinute22).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute22).ForeColor = SystemColors.WindowText;
		((Control)txtMinute22).Location = new Point(606, 259);
		((TextBoxBase)txtMinute22).MaxLength = 0;
		((Control)txtMinute22).Name = "txtMinute22";
		((Control)txtMinute22).RightToLeft = (RightToLeft)0;
		((Control)txtMinute22).Size = new Size(42, 28);
		((Control)txtMinute22).TabIndex = 236;
		txtHour22.AcceptsReturn = true;
		((Control)txtHour22).BackColor = SystemColors.Window;
		((Control)txtHour22).Cursor = Cursors.IBeam;
		((Control)txtHour22).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour22).ForeColor = SystemColors.WindowText;
		((Control)txtHour22).Location = new Point(546, 259);
		((TextBoxBase)txtHour22).MaxLength = 0;
		((Control)txtHour22).Name = "txtHour22";
		((Control)txtHour22).RightToLeft = (RightToLeft)0;
		((Control)txtHour22).Size = new Size(42, 28);
		((Control)txtHour22).TabIndex = 235;
		txtHour21.AcceptsReturn = true;
		((Control)txtHour21).BackColor = SystemColors.Window;
		((Control)txtHour21).Cursor = Cursors.IBeam;
		((Control)txtHour21).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour21).ForeColor = SystemColors.WindowText;
		((Control)txtHour21).Location = new Point(546, 231);
		((TextBoxBase)txtHour21).MaxLength = 0;
		((Control)txtHour21).Name = "txtHour21";
		((Control)txtHour21).RightToLeft = (RightToLeft)0;
		((Control)txtHour21).Size = new Size(42, 28);
		((Control)txtHour21).TabIndex = 232;
		txtMinute21.AcceptsReturn = true;
		((Control)txtMinute21).BackColor = SystemColors.Window;
		((Control)txtMinute21).Cursor = Cursors.IBeam;
		((Control)txtMinute21).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute21).ForeColor = SystemColors.WindowText;
		((Control)txtMinute21).Location = new Point(606, 231);
		((TextBoxBase)txtMinute21).MaxLength = 0;
		((Control)txtMinute21).Name = "txtMinute21";
		((Control)txtMinute21).RightToLeft = (RightToLeft)0;
		((Control)txtMinute21).Size = new Size(42, 28);
		((Control)txtMinute21).TabIndex = 233;
		txtMinute20.AcceptsReturn = true;
		((Control)txtMinute20).BackColor = SystemColors.Window;
		((Control)txtMinute20).Cursor = Cursors.IBeam;
		((Control)txtMinute20).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute20).ForeColor = SystemColors.WindowText;
		((Control)txtMinute20).Location = new Point(606, 203);
		((TextBoxBase)txtMinute20).MaxLength = 0;
		((Control)txtMinute20).Name = "txtMinute20";
		((Control)txtMinute20).RightToLeft = (RightToLeft)0;
		((Control)txtMinute20).Size = new Size(42, 28);
		((Control)txtMinute20).TabIndex = 230;
		txtHour20.AcceptsReturn = true;
		((Control)txtHour20).BackColor = SystemColors.Window;
		((Control)txtHour20).Cursor = Cursors.IBeam;
		((Control)txtHour20).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour20).ForeColor = SystemColors.WindowText;
		((Control)txtHour20).Location = new Point(546, 203);
		((TextBoxBase)txtHour20).MaxLength = 0;
		((Control)txtHour20).Name = "txtHour20";
		((Control)txtHour20).RightToLeft = (RightToLeft)0;
		((Control)txtHour20).Size = new Size(42, 28);
		((Control)txtHour20).TabIndex = 229;
		txtHour19.AcceptsReturn = true;
		((Control)txtHour19).BackColor = SystemColors.Window;
		((Control)txtHour19).Cursor = Cursors.IBeam;
		((Control)txtHour19).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour19).ForeColor = SystemColors.WindowText;
		((Control)txtHour19).Location = new Point(546, 175);
		((TextBoxBase)txtHour19).MaxLength = 0;
		((Control)txtHour19).Name = "txtHour19";
		((Control)txtHour19).RightToLeft = (RightToLeft)0;
		((Control)txtHour19).Size = new Size(42, 28);
		((Control)txtHour19).TabIndex = 226;
		txtMinute19.AcceptsReturn = true;
		((Control)txtMinute19).BackColor = SystemColors.Window;
		((Control)txtMinute19).Cursor = Cursors.IBeam;
		((Control)txtMinute19).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute19).ForeColor = SystemColors.WindowText;
		((Control)txtMinute19).Location = new Point(606, 175);
		((TextBoxBase)txtMinute19).MaxLength = 0;
		((Control)txtMinute19).Name = "txtMinute19";
		((Control)txtMinute19).RightToLeft = (RightToLeft)0;
		((Control)txtMinute19).Size = new Size(42, 28);
		((Control)txtMinute19).TabIndex = 227;
		txtMinute18.AcceptsReturn = true;
		((Control)txtMinute18).BackColor = SystemColors.Window;
		((Control)txtMinute18).Cursor = Cursors.IBeam;
		((Control)txtMinute18).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute18).ForeColor = SystemColors.WindowText;
		((Control)txtMinute18).Location = new Point(606, 148);
		((TextBoxBase)txtMinute18).MaxLength = 0;
		((Control)txtMinute18).Name = "txtMinute18";
		((Control)txtMinute18).RightToLeft = (RightToLeft)0;
		((Control)txtMinute18).Size = new Size(42, 28);
		((Control)txtMinute18).TabIndex = 224;
		txtHour18.AcceptsReturn = true;
		((Control)txtHour18).BackColor = SystemColors.Window;
		((Control)txtHour18).Cursor = Cursors.IBeam;
		((Control)txtHour18).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour18).ForeColor = SystemColors.WindowText;
		((Control)txtHour18).Location = new Point(546, 148);
		((TextBoxBase)txtHour18).MaxLength = 0;
		((Control)txtHour18).Name = "txtHour18";
		((Control)txtHour18).RightToLeft = (RightToLeft)0;
		((Control)txtHour18).Size = new Size(42, 28);
		((Control)txtHour18).TabIndex = 223;
		txtHour17.AcceptsReturn = true;
		((Control)txtHour17).BackColor = SystemColors.Window;
		((Control)txtHour17).Cursor = Cursors.IBeam;
		((Control)txtHour17).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour17).ForeColor = SystemColors.WindowText;
		((Control)txtHour17).Location = new Point(546, 120);
		((TextBoxBase)txtHour17).MaxLength = 0;
		((Control)txtHour17).Name = "txtHour17";
		((Control)txtHour17).RightToLeft = (RightToLeft)0;
		((Control)txtHour17).Size = new Size(42, 28);
		((Control)txtHour17).TabIndex = 220;
		txtMinute17.AcceptsReturn = true;
		((Control)txtMinute17).BackColor = SystemColors.Window;
		((Control)txtMinute17).Cursor = Cursors.IBeam;
		((Control)txtMinute17).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute17).ForeColor = SystemColors.WindowText;
		((Control)txtMinute17).Location = new Point(606, 120);
		((TextBoxBase)txtMinute17).MaxLength = 0;
		((Control)txtMinute17).Name = "txtMinute17";
		((Control)txtMinute17).RightToLeft = (RightToLeft)0;
		((Control)txtMinute17).Size = new Size(42, 28);
		((Control)txtMinute17).TabIndex = 221;
		txtMinute16.AcceptsReturn = true;
		((Control)txtMinute16).BackColor = SystemColors.Window;
		((Control)txtMinute16).Cursor = Cursors.IBeam;
		((Control)txtMinute16).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute16).ForeColor = SystemColors.WindowText;
		((Control)txtMinute16).Location = new Point(606, 92);
		((TextBoxBase)txtMinute16).MaxLength = 0;
		((Control)txtMinute16).Name = "txtMinute16";
		((Control)txtMinute16).RightToLeft = (RightToLeft)0;
		((Control)txtMinute16).Size = new Size(42, 28);
		((Control)txtMinute16).TabIndex = 218;
		txtHour16.AcceptsReturn = true;
		((Control)txtHour16).BackColor = SystemColors.Window;
		((Control)txtHour16).Cursor = Cursors.IBeam;
		((Control)txtHour16).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour16).ForeColor = SystemColors.WindowText;
		((Control)txtHour16).Location = new Point(546, 92);
		((TextBoxBase)txtHour16).MaxLength = 0;
		((Control)txtHour16).Name = "txtHour16";
		((Control)txtHour16).RightToLeft = (RightToLeft)0;
		((Control)txtHour16).Size = new Size(42, 28);
		((Control)txtHour16).TabIndex = 217;
		((Control)chkValid23).BackColor = SystemColors.Control;
		((Control)chkValid23).Cursor = Cursors.Default;
		((Control)chkValid23).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid23).ForeColor = SystemColors.ControlText;
		((Control)chkValid23).Location = new Point(520, 290);
		((Control)chkValid23).Name = "chkValid23";
		((Control)chkValid23).RightToLeft = (RightToLeft)0;
		((Control)chkValid23).Size = new Size(14, 20);
		((Control)chkValid23).TabIndex = 237;
		((Control)chkValid23).Text = "";
		((ButtonBase)chkValid23).UseVisualStyleBackColor = false;
		((Control)chkValid22).BackColor = SystemColors.Control;
		((Control)chkValid22).Cursor = Cursors.Default;
		((Control)chkValid22).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid22).ForeColor = SystemColors.ControlText;
		((Control)chkValid22).Location = new Point(520, 262);
		((Control)chkValid22).Name = "chkValid22";
		((Control)chkValid22).RightToLeft = (RightToLeft)0;
		((Control)chkValid22).Size = new Size(14, 20);
		((Control)chkValid22).TabIndex = 234;
		((Control)chkValid22).Text = "";
		((ButtonBase)chkValid22).UseVisualStyleBackColor = false;
		((Control)chkValid21).BackColor = SystemColors.Control;
		((Control)chkValid21).Cursor = Cursors.Default;
		((Control)chkValid21).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid21).ForeColor = SystemColors.ControlText;
		((Control)chkValid21).Location = new Point(520, 234);
		((Control)chkValid21).Name = "chkValid21";
		((Control)chkValid21).RightToLeft = (RightToLeft)0;
		((Control)chkValid21).Size = new Size(14, 20);
		((Control)chkValid21).TabIndex = 231;
		((Control)chkValid21).Text = "";
		((ButtonBase)chkValid21).UseVisualStyleBackColor = false;
		((Control)chkValid20).BackColor = SystemColors.Control;
		((Control)chkValid20).Cursor = Cursors.Default;
		((Control)chkValid20).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid20).ForeColor = SystemColors.ControlText;
		((Control)chkValid20).Location = new Point(520, 206);
		((Control)chkValid20).Name = "chkValid20";
		((Control)chkValid20).RightToLeft = (RightToLeft)0;
		((Control)chkValid20).Size = new Size(14, 20);
		((Control)chkValid20).TabIndex = 228;
		((Control)chkValid20).Text = "";
		((ButtonBase)chkValid20).UseVisualStyleBackColor = false;
		((Control)chkValid19).BackColor = SystemColors.Control;
		((Control)chkValid19).Cursor = Cursors.Default;
		((Control)chkValid19).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid19).ForeColor = SystemColors.ControlText;
		((Control)chkValid19).Location = new Point(520, 178);
		((Control)chkValid19).Name = "chkValid19";
		((Control)chkValid19).RightToLeft = (RightToLeft)0;
		((Control)chkValid19).Size = new Size(14, 20);
		((Control)chkValid19).TabIndex = 225;
		((Control)chkValid19).Text = "";
		((ButtonBase)chkValid19).UseVisualStyleBackColor = false;
		((Control)chkValid18).BackColor = SystemColors.Control;
		((Control)chkValid18).Cursor = Cursors.Default;
		((Control)chkValid18).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid18).ForeColor = SystemColors.ControlText;
		((Control)chkValid18).Location = new Point(520, 151);
		((Control)chkValid18).Name = "chkValid18";
		((Control)chkValid18).RightToLeft = (RightToLeft)0;
		((Control)chkValid18).Size = new Size(14, 20);
		((Control)chkValid18).TabIndex = 222;
		((Control)chkValid18).Text = "";
		((ButtonBase)chkValid18).UseVisualStyleBackColor = false;
		((Control)chkValid17).BackColor = SystemColors.Control;
		((Control)chkValid17).Cursor = Cursors.Default;
		((Control)chkValid17).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid17).ForeColor = SystemColors.ControlText;
		((Control)chkValid17).Location = new Point(520, 123);
		((Control)chkValid17).Name = "chkValid17";
		((Control)chkValid17).RightToLeft = (RightToLeft)0;
		((Control)chkValid17).Size = new Size(14, 20);
		((Control)chkValid17).TabIndex = 219;
		((Control)chkValid17).Text = "";
		((ButtonBase)chkValid17).UseVisualStyleBackColor = false;
		((Control)chkValid16).BackColor = SystemColors.Control;
		((Control)chkValid16).Cursor = Cursors.Default;
		((Control)chkValid16).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid16).ForeColor = SystemColors.ControlText;
		((Control)chkValid16).Location = new Point(520, 96);
		((Control)chkValid16).Name = "chkValid16";
		((Control)chkValid16).RightToLeft = (RightToLeft)0;
		((Control)chkValid16).Size = new Size(14, 20);
		((Control)chkValid16).TabIndex = 216;
		((Control)chkValid16).Text = "";
		((ButtonBase)chkValid16).UseVisualStyleBackColor = false;
		((Control)Label2_40).BackColor = Color.Transparent;
		((Control)Label2_40).Cursor = Cursors.Default;
		((Control)Label2_40).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_40).ForeColor = SystemColors.ControlText;
		((Control)Label2_40).Location = new Point(596, 96);
		((Control)Label2_40).Name = "Label2_40";
		((Control)Label2_40).RightToLeft = (RightToLeft)0;
		((Control)Label2_40).Size = new Size(6, 20);
		((Control)Label2_40).TabIndex = 255;
		((Control)Label2_40).Text = ":";
		Label2_40.TextAlign = (ContentAlignment)32;
		((Control)Label2_47).BackColor = Color.Transparent;
		((Control)Label2_47).Cursor = Cursors.Default;
		((Control)Label2_47).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_47).ForeColor = SystemColors.ControlText;
		((Control)Label2_47).Location = new Point(596, 290);
		((Control)Label2_47).Name = "Label2_47";
		((Control)Label2_47).RightToLeft = (RightToLeft)0;
		((Control)Label2_47).Size = new Size(6, 20);
		((Control)Label2_47).TabIndex = 254;
		((Control)Label2_47).Text = ":";
		Label2_47.TextAlign = (ContentAlignment)32;
		((Control)Label2_46).BackColor = Color.Transparent;
		((Control)Label2_46).Cursor = Cursors.Default;
		((Control)Label2_46).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_46).ForeColor = SystemColors.ControlText;
		((Control)Label2_46).Location = new Point(596, 262);
		((Control)Label2_46).Name = "Label2_46";
		((Control)Label2_46).RightToLeft = (RightToLeft)0;
		((Control)Label2_46).Size = new Size(6, 20);
		((Control)Label2_46).TabIndex = 253;
		((Control)Label2_46).Text = ":";
		Label2_46.TextAlign = (ContentAlignment)32;
		((Control)Label2_45).BackColor = Color.Transparent;
		((Control)Label2_45).Cursor = Cursors.Default;
		((Control)Label2_45).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_45).ForeColor = SystemColors.ControlText;
		((Control)Label2_45).Location = new Point(596, 234);
		((Control)Label2_45).Name = "Label2_45";
		((Control)Label2_45).RightToLeft = (RightToLeft)0;
		((Control)Label2_45).Size = new Size(6, 20);
		((Control)Label2_45).TabIndex = 252;
		((Control)Label2_45).Text = ":";
		Label2_45.TextAlign = (ContentAlignment)32;
		((Control)Label2_44).BackColor = Color.Transparent;
		((Control)Label2_44).Cursor = Cursors.Default;
		((Control)Label2_44).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_44).ForeColor = SystemColors.ControlText;
		((Control)Label2_44).Location = new Point(596, 206);
		((Control)Label2_44).Name = "Label2_44";
		((Control)Label2_44).RightToLeft = (RightToLeft)0;
		((Control)Label2_44).Size = new Size(6, 20);
		((Control)Label2_44).TabIndex = 251;
		((Control)Label2_44).Text = ":";
		Label2_44.TextAlign = (ContentAlignment)32;
		((Control)Label2_43).BackColor = Color.Transparent;
		((Control)Label2_43).Cursor = Cursors.Default;
		((Control)Label2_43).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_43).ForeColor = SystemColors.ControlText;
		((Control)Label2_43).Location = new Point(596, 178);
		((Control)Label2_43).Name = "Label2_43";
		((Control)Label2_43).RightToLeft = (RightToLeft)0;
		((Control)Label2_43).Size = new Size(6, 20);
		((Control)Label2_43).TabIndex = 250;
		((Control)Label2_43).Text = ":";
		Label2_43.TextAlign = (ContentAlignment)32;
		((Control)Label2_42).BackColor = Color.Transparent;
		((Control)Label2_42).Cursor = Cursors.Default;
		((Control)Label2_42).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_42).ForeColor = SystemColors.ControlText;
		((Control)Label2_42).Location = new Point(596, 151);
		((Control)Label2_42).Name = "Label2_42";
		((Control)Label2_42).RightToLeft = (RightToLeft)0;
		((Control)Label2_42).Size = new Size(6, 20);
		((Control)Label2_42).TabIndex = 249;
		((Control)Label2_42).Text = ":";
		Label2_42.TextAlign = (ContentAlignment)32;
		((Control)Label2_41).BackColor = Color.Transparent;
		((Control)Label2_41).Cursor = Cursors.Default;
		((Control)Label2_41).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_41).ForeColor = SystemColors.ControlText;
		((Control)Label2_41).Location = new Point(596, 123);
		((Control)Label2_41).Name = "Label2_41";
		((Control)Label2_41).RightToLeft = (RightToLeft)0;
		((Control)Label2_41).Size = new Size(6, 20);
		((Control)Label2_41).TabIndex = 248;
		((Control)Label2_41).Text = ":";
		Label2_41.TextAlign = (ContentAlignment)32;
		((Control)Label2_39).BackColor = Color.Transparent;
		((Control)Label2_39).Cursor = Cursors.Default;
		((Control)Label2_39).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_39).ForeColor = SystemColors.ControlText;
		((Control)Label2_39).Location = new Point(450, 290);
		((Control)Label2_39).Name = "Label2_39";
		((Control)Label2_39).RightToLeft = (RightToLeft)0;
		((Control)Label2_39).Size = new Size(66, 20);
		((Control)Label2_39).TabIndex = 247;
		((Control)Label2_39).Text = "Point 24:";
		((Control)Label2_38).BackColor = Color.Transparent;
		((Control)Label2_38).Cursor = Cursors.Default;
		((Control)Label2_38).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_38).ForeColor = SystemColors.ControlText;
		((Control)Label2_38).Location = new Point(450, 262);
		((Control)Label2_38).Name = "Label2_38";
		((Control)Label2_38).RightToLeft = (RightToLeft)0;
		((Control)Label2_38).Size = new Size(66, 20);
		((Control)Label2_38).TabIndex = 246;
		((Control)Label2_38).Text = "Point 23:";
		((Control)Label2_37).BackColor = Color.Transparent;
		((Control)Label2_37).Cursor = Cursors.Default;
		((Control)Label2_37).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_37).ForeColor = SystemColors.ControlText;
		((Control)Label2_37).Location = new Point(450, 234);
		((Control)Label2_37).Name = "Label2_37";
		((Control)Label2_37).RightToLeft = (RightToLeft)0;
		((Control)Label2_37).Size = new Size(66, 20);
		((Control)Label2_37).TabIndex = 245;
		((Control)Label2_37).Text = "Point 22:";
		((Control)Label2_36).BackColor = Color.Transparent;
		((Control)Label2_36).Cursor = Cursors.Default;
		((Control)Label2_36).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_36).ForeColor = SystemColors.ControlText;
		((Control)Label2_36).Location = new Point(450, 206);
		((Control)Label2_36).Name = "Label2_36";
		((Control)Label2_36).RightToLeft = (RightToLeft)0;
		((Control)Label2_36).Size = new Size(66, 20);
		((Control)Label2_36).TabIndex = 244;
		((Control)Label2_36).Text = "Point 21:";
		((Control)Label2_35).BackColor = Color.Transparent;
		((Control)Label2_35).Cursor = Cursors.Default;
		((Control)Label2_35).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_35).ForeColor = SystemColors.ControlText;
		((Control)Label2_35).Location = new Point(450, 178);
		((Control)Label2_35).Name = "Label2_35";
		((Control)Label2_35).RightToLeft = (RightToLeft)0;
		((Control)Label2_35).Size = new Size(66, 20);
		((Control)Label2_35).TabIndex = 243;
		((Control)Label2_35).Text = "Point 20:";
		((Control)Label2_34).BackColor = Color.Transparent;
		((Control)Label2_34).Cursor = Cursors.Default;
		((Control)Label2_34).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_34).ForeColor = SystemColors.ControlText;
		((Control)Label2_34).Location = new Point(450, 151);
		((Control)Label2_34).Name = "Label2_34";
		((Control)Label2_34).RightToLeft = (RightToLeft)0;
		((Control)Label2_34).Size = new Size(66, 20);
		((Control)Label2_34).TabIndex = 242;
		((Control)Label2_34).Text = "Point 19:";
		((Control)Label2_33).BackColor = Color.Transparent;
		((Control)Label2_33).Cursor = Cursors.Default;
		((Control)Label2_33).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_33).ForeColor = SystemColors.ControlText;
		((Control)Label2_33).Location = new Point(450, 123);
		((Control)Label2_33).Name = "Label2_33";
		((Control)Label2_33).RightToLeft = (RightToLeft)0;
		((Control)Label2_33).Size = new Size(66, 20);
		((Control)Label2_33).TabIndex = 241;
		((Control)Label2_33).Text = "Point 18:";
		((Control)Label2_32).BackColor = Color.Transparent;
		((Control)Label2_32).Cursor = Cursors.Default;
		((Control)Label2_32).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_32).ForeColor = SystemColors.ControlText;
		((Control)Label2_32).Location = new Point(450, 96);
		((Control)Label2_32).Name = "Label2_32";
		((Control)Label2_32).RightToLeft = (RightToLeft)0;
		((Control)Label2_32).Size = new Size(66, 20);
		((Control)Label2_32).TabIndex = 240;
		((Control)Label2_32).Text = "Point 17:";
		txtHour15.AcceptsReturn = true;
		((Control)txtHour15).BackColor = SystemColors.Window;
		((Control)txtHour15).Cursor = Cursors.IBeam;
		((Control)txtHour15).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour15).ForeColor = SystemColors.WindowText;
		((Control)txtHour15).Location = new Point(326, 287);
		((TextBoxBase)txtHour15).MaxLength = 0;
		((Control)txtHour15).Name = "txtHour15";
		((Control)txtHour15).RightToLeft = (RightToLeft)0;
		((Control)txtHour15).Size = new Size(42, 28);
		((Control)txtHour15).TabIndex = 198;
		txtMinute15.AcceptsReturn = true;
		((Control)txtMinute15).BackColor = SystemColors.Window;
		((Control)txtMinute15).Cursor = Cursors.IBeam;
		((Control)txtMinute15).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute15).ForeColor = SystemColors.WindowText;
		((Control)txtMinute15).Location = new Point(386, 287);
		((TextBoxBase)txtMinute15).MaxLength = 0;
		((Control)txtMinute15).Name = "txtMinute15";
		((Control)txtMinute15).RightToLeft = (RightToLeft)0;
		((Control)txtMinute15).Size = new Size(42, 28);
		((Control)txtMinute15).TabIndex = 199;
		txtMinute14.AcceptsReturn = true;
		((Control)txtMinute14).BackColor = SystemColors.Window;
		((Control)txtMinute14).Cursor = Cursors.IBeam;
		((Control)txtMinute14).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute14).ForeColor = SystemColors.WindowText;
		((Control)txtMinute14).Location = new Point(386, 259);
		((TextBoxBase)txtMinute14).MaxLength = 0;
		((Control)txtMinute14).Name = "txtMinute14";
		((Control)txtMinute14).RightToLeft = (RightToLeft)0;
		((Control)txtMinute14).Size = new Size(42, 28);
		((Control)txtMinute14).TabIndex = 196;
		txtHour14.AcceptsReturn = true;
		((Control)txtHour14).BackColor = SystemColors.Window;
		((Control)txtHour14).Cursor = Cursors.IBeam;
		((Control)txtHour14).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour14).ForeColor = SystemColors.WindowText;
		((Control)txtHour14).Location = new Point(326, 259);
		((TextBoxBase)txtHour14).MaxLength = 0;
		((Control)txtHour14).Name = "txtHour14";
		((Control)txtHour14).RightToLeft = (RightToLeft)0;
		((Control)txtHour14).Size = new Size(42, 28);
		((Control)txtHour14).TabIndex = 195;
		txtHour13.AcceptsReturn = true;
		((Control)txtHour13).BackColor = SystemColors.Window;
		((Control)txtHour13).Cursor = Cursors.IBeam;
		((Control)txtHour13).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour13).ForeColor = SystemColors.WindowText;
		((Control)txtHour13).Location = new Point(326, 231);
		((TextBoxBase)txtHour13).MaxLength = 0;
		((Control)txtHour13).Name = "txtHour13";
		((Control)txtHour13).RightToLeft = (RightToLeft)0;
		((Control)txtHour13).Size = new Size(42, 28);
		((Control)txtHour13).TabIndex = 192;
		txtMinute13.AcceptsReturn = true;
		((Control)txtMinute13).BackColor = SystemColors.Window;
		((Control)txtMinute13).Cursor = Cursors.IBeam;
		((Control)txtMinute13).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute13).ForeColor = SystemColors.WindowText;
		((Control)txtMinute13).Location = new Point(386, 231);
		((TextBoxBase)txtMinute13).MaxLength = 0;
		((Control)txtMinute13).Name = "txtMinute13";
		((Control)txtMinute13).RightToLeft = (RightToLeft)0;
		((Control)txtMinute13).Size = new Size(42, 28);
		((Control)txtMinute13).TabIndex = 193;
		txtMinute12.AcceptsReturn = true;
		((Control)txtMinute12).BackColor = SystemColors.Window;
		((Control)txtMinute12).Cursor = Cursors.IBeam;
		((Control)txtMinute12).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute12).ForeColor = SystemColors.WindowText;
		((Control)txtMinute12).Location = new Point(386, 203);
		((TextBoxBase)txtMinute12).MaxLength = 0;
		((Control)txtMinute12).Name = "txtMinute12";
		((Control)txtMinute12).RightToLeft = (RightToLeft)0;
		((Control)txtMinute12).Size = new Size(42, 28);
		((Control)txtMinute12).TabIndex = 190;
		txtHour12.AcceptsReturn = true;
		((Control)txtHour12).BackColor = SystemColors.Window;
		((Control)txtHour12).Cursor = Cursors.IBeam;
		((Control)txtHour12).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour12).ForeColor = SystemColors.WindowText;
		((Control)txtHour12).Location = new Point(326, 203);
		((TextBoxBase)txtHour12).MaxLength = 0;
		((Control)txtHour12).Name = "txtHour12";
		((Control)txtHour12).RightToLeft = (RightToLeft)0;
		((Control)txtHour12).Size = new Size(42, 28);
		((Control)txtHour12).TabIndex = 189;
		txtHour11.AcceptsReturn = true;
		((Control)txtHour11).BackColor = SystemColors.Window;
		((Control)txtHour11).Cursor = Cursors.IBeam;
		((Control)txtHour11).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour11).ForeColor = SystemColors.WindowText;
		((Control)txtHour11).Location = new Point(326, 175);
		((TextBoxBase)txtHour11).MaxLength = 0;
		((Control)txtHour11).Name = "txtHour11";
		((Control)txtHour11).RightToLeft = (RightToLeft)0;
		((Control)txtHour11).Size = new Size(42, 28);
		((Control)txtHour11).TabIndex = 186;
		txtMinute11.AcceptsReturn = true;
		((Control)txtMinute11).BackColor = SystemColors.Window;
		((Control)txtMinute11).Cursor = Cursors.IBeam;
		((Control)txtMinute11).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute11).ForeColor = SystemColors.WindowText;
		((Control)txtMinute11).Location = new Point(386, 175);
		((TextBoxBase)txtMinute11).MaxLength = 0;
		((Control)txtMinute11).Name = "txtMinute11";
		((Control)txtMinute11).RightToLeft = (RightToLeft)0;
		((Control)txtMinute11).Size = new Size(42, 28);
		((Control)txtMinute11).TabIndex = 187;
		txtMinute10.AcceptsReturn = true;
		((Control)txtMinute10).BackColor = SystemColors.Window;
		((Control)txtMinute10).Cursor = Cursors.IBeam;
		((Control)txtMinute10).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute10).ForeColor = SystemColors.WindowText;
		((Control)txtMinute10).Location = new Point(386, 148);
		((TextBoxBase)txtMinute10).MaxLength = 0;
		((Control)txtMinute10).Name = "txtMinute10";
		((Control)txtMinute10).RightToLeft = (RightToLeft)0;
		((Control)txtMinute10).Size = new Size(42, 28);
		((Control)txtMinute10).TabIndex = 184;
		txtHour10.AcceptsReturn = true;
		((Control)txtHour10).BackColor = SystemColors.Window;
		((Control)txtHour10).Cursor = Cursors.IBeam;
		((Control)txtHour10).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour10).ForeColor = SystemColors.WindowText;
		((Control)txtHour10).Location = new Point(326, 148);
		((TextBoxBase)txtHour10).MaxLength = 0;
		((Control)txtHour10).Name = "txtHour10";
		((Control)txtHour10).RightToLeft = (RightToLeft)0;
		((Control)txtHour10).Size = new Size(42, 28);
		((Control)txtHour10).TabIndex = 183;
		txtHour9.AcceptsReturn = true;
		((Control)txtHour9).BackColor = SystemColors.Window;
		((Control)txtHour9).Cursor = Cursors.IBeam;
		((Control)txtHour9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour9).ForeColor = SystemColors.WindowText;
		((Control)txtHour9).Location = new Point(326, 120);
		((TextBoxBase)txtHour9).MaxLength = 0;
		((Control)txtHour9).Name = "txtHour9";
		((Control)txtHour9).RightToLeft = (RightToLeft)0;
		((Control)txtHour9).Size = new Size(42, 28);
		((Control)txtHour9).TabIndex = 180;
		txtMinute9.AcceptsReturn = true;
		((Control)txtMinute9).BackColor = SystemColors.Window;
		((Control)txtMinute9).Cursor = Cursors.IBeam;
		((Control)txtMinute9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute9).ForeColor = SystemColors.WindowText;
		((Control)txtMinute9).Location = new Point(386, 120);
		((TextBoxBase)txtMinute9).MaxLength = 0;
		((Control)txtMinute9).Name = "txtMinute9";
		((Control)txtMinute9).RightToLeft = (RightToLeft)0;
		((Control)txtMinute9).Size = new Size(42, 28);
		((Control)txtMinute9).TabIndex = 181;
		txtMinute8.AcceptsReturn = true;
		((Control)txtMinute8).BackColor = SystemColors.Window;
		((Control)txtMinute8).Cursor = Cursors.IBeam;
		((Control)txtMinute8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute8).ForeColor = SystemColors.WindowText;
		((Control)txtMinute8).Location = new Point(386, 92);
		((TextBoxBase)txtMinute8).MaxLength = 0;
		((Control)txtMinute8).Name = "txtMinute8";
		((Control)txtMinute8).RightToLeft = (RightToLeft)0;
		((Control)txtMinute8).Size = new Size(42, 28);
		((Control)txtMinute8).TabIndex = 178;
		txtHour8.AcceptsReturn = true;
		((Control)txtHour8).BackColor = SystemColors.Window;
		((Control)txtHour8).Cursor = Cursors.IBeam;
		((Control)txtHour8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour8).ForeColor = SystemColors.WindowText;
		((Control)txtHour8).Location = new Point(326, 92);
		((TextBoxBase)txtHour8).MaxLength = 0;
		((Control)txtHour8).Name = "txtHour8";
		((Control)txtHour8).RightToLeft = (RightToLeft)0;
		((Control)txtHour8).Size = new Size(42, 28);
		((Control)txtHour8).TabIndex = 177;
		((Control)chkValid15).BackColor = SystemColors.Control;
		((Control)chkValid15).Cursor = Cursors.Default;
		((Control)chkValid15).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid15).ForeColor = SystemColors.ControlText;
		((Control)chkValid15).Location = new Point(300, 290);
		((Control)chkValid15).Name = "chkValid15";
		((Control)chkValid15).RightToLeft = (RightToLeft)0;
		((Control)chkValid15).Size = new Size(14, 20);
		((Control)chkValid15).TabIndex = 197;
		((Control)chkValid15).Text = "";
		((ButtonBase)chkValid15).UseVisualStyleBackColor = false;
		((Control)chkValid14).BackColor = SystemColors.Control;
		((Control)chkValid14).Cursor = Cursors.Default;
		((Control)chkValid14).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid14).ForeColor = SystemColors.ControlText;
		((Control)chkValid14).Location = new Point(300, 262);
		((Control)chkValid14).Name = "chkValid14";
		((Control)chkValid14).RightToLeft = (RightToLeft)0;
		((Control)chkValid14).Size = new Size(14, 20);
		((Control)chkValid14).TabIndex = 194;
		((Control)chkValid14).Text = "";
		((ButtonBase)chkValid14).UseVisualStyleBackColor = false;
		((Control)chkValid13).BackColor = SystemColors.Control;
		((Control)chkValid13).Cursor = Cursors.Default;
		((Control)chkValid13).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid13).ForeColor = SystemColors.ControlText;
		((Control)chkValid13).Location = new Point(300, 234);
		((Control)chkValid13).Name = "chkValid13";
		((Control)chkValid13).RightToLeft = (RightToLeft)0;
		((Control)chkValid13).Size = new Size(14, 20);
		((Control)chkValid13).TabIndex = 191;
		((Control)chkValid13).Text = "";
		((ButtonBase)chkValid13).UseVisualStyleBackColor = false;
		((Control)chkValid12).BackColor = SystemColors.Control;
		((Control)chkValid12).Cursor = Cursors.Default;
		((Control)chkValid12).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid12).ForeColor = SystemColors.ControlText;
		((Control)chkValid12).Location = new Point(300, 206);
		((Control)chkValid12).Name = "chkValid12";
		((Control)chkValid12).RightToLeft = (RightToLeft)0;
		((Control)chkValid12).Size = new Size(14, 20);
		((Control)chkValid12).TabIndex = 188;
		((Control)chkValid12).Text = "";
		((ButtonBase)chkValid12).UseVisualStyleBackColor = false;
		((Control)chkValid11).BackColor = SystemColors.Control;
		((Control)chkValid11).Cursor = Cursors.Default;
		((Control)chkValid11).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid11).ForeColor = SystemColors.ControlText;
		((Control)chkValid11).Location = new Point(300, 178);
		((Control)chkValid11).Name = "chkValid11";
		((Control)chkValid11).RightToLeft = (RightToLeft)0;
		((Control)chkValid11).Size = new Size(14, 20);
		((Control)chkValid11).TabIndex = 185;
		((Control)chkValid11).Text = "";
		((ButtonBase)chkValid11).UseVisualStyleBackColor = false;
		((Control)chkValid10).BackColor = SystemColors.Control;
		((Control)chkValid10).Cursor = Cursors.Default;
		((Control)chkValid10).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid10).ForeColor = SystemColors.ControlText;
		((Control)chkValid10).Location = new Point(300, 151);
		((Control)chkValid10).Name = "chkValid10";
		((Control)chkValid10).RightToLeft = (RightToLeft)0;
		((Control)chkValid10).Size = new Size(14, 20);
		((Control)chkValid10).TabIndex = 182;
		((Control)chkValid10).Text = "";
		((ButtonBase)chkValid10).UseVisualStyleBackColor = false;
		((Control)chkValid9).BackColor = SystemColors.Control;
		((Control)chkValid9).Cursor = Cursors.Default;
		((Control)chkValid9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid9).ForeColor = SystemColors.ControlText;
		((Control)chkValid9).Location = new Point(300, 123);
		((Control)chkValid9).Name = "chkValid9";
		((Control)chkValid9).RightToLeft = (RightToLeft)0;
		((Control)chkValid9).Size = new Size(14, 20);
		((Control)chkValid9).TabIndex = 179;
		((Control)chkValid9).Text = "";
		((ButtonBase)chkValid9).UseVisualStyleBackColor = false;
		((Control)chkValid8).BackColor = SystemColors.Control;
		((Control)chkValid8).Cursor = Cursors.Default;
		((Control)chkValid8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid8).ForeColor = SystemColors.ControlText;
		((Control)chkValid8).Location = new Point(300, 96);
		((Control)chkValid8).Name = "chkValid8";
		((Control)chkValid8).RightToLeft = (RightToLeft)0;
		((Control)chkValid8).Size = new Size(14, 20);
		((Control)chkValid8).TabIndex = 176;
		((Control)chkValid8).Text = "";
		((ButtonBase)chkValid8).UseVisualStyleBackColor = false;
		((Control)Label2_24).BackColor = Color.Transparent;
		((Control)Label2_24).Cursor = Cursors.Default;
		((Control)Label2_24).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_24).ForeColor = SystemColors.ControlText;
		((Control)Label2_24).Location = new Point(376, 96);
		((Control)Label2_24).Name = "Label2_24";
		((Control)Label2_24).RightToLeft = (RightToLeft)0;
		((Control)Label2_24).Size = new Size(6, 20);
		((Control)Label2_24).TabIndex = 215;
		((Control)Label2_24).Text = ":";
		Label2_24.TextAlign = (ContentAlignment)32;
		((Control)Label2_31).BackColor = Color.Transparent;
		((Control)Label2_31).Cursor = Cursors.Default;
		((Control)Label2_31).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_31).ForeColor = SystemColors.ControlText;
		((Control)Label2_31).Location = new Point(376, 290);
		((Control)Label2_31).Name = "Label2_31";
		((Control)Label2_31).RightToLeft = (RightToLeft)0;
		((Control)Label2_31).Size = new Size(6, 20);
		((Control)Label2_31).TabIndex = 214;
		((Control)Label2_31).Text = ":";
		Label2_31.TextAlign = (ContentAlignment)32;
		((Control)Label2_30).BackColor = Color.Transparent;
		((Control)Label2_30).Cursor = Cursors.Default;
		((Control)Label2_30).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_30).ForeColor = SystemColors.ControlText;
		((Control)Label2_30).Location = new Point(376, 262);
		((Control)Label2_30).Name = "Label2_30";
		((Control)Label2_30).RightToLeft = (RightToLeft)0;
		((Control)Label2_30).Size = new Size(6, 20);
		((Control)Label2_30).TabIndex = 213;
		((Control)Label2_30).Text = ":";
		Label2_30.TextAlign = (ContentAlignment)32;
		((Control)Label2_29).BackColor = Color.Transparent;
		((Control)Label2_29).Cursor = Cursors.Default;
		((Control)Label2_29).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_29).ForeColor = SystemColors.ControlText;
		((Control)Label2_29).Location = new Point(376, 234);
		((Control)Label2_29).Name = "Label2_29";
		((Control)Label2_29).RightToLeft = (RightToLeft)0;
		((Control)Label2_29).Size = new Size(6, 20);
		((Control)Label2_29).TabIndex = 212;
		((Control)Label2_29).Text = ":";
		Label2_29.TextAlign = (ContentAlignment)32;
		((Control)Label2_28).BackColor = Color.Transparent;
		((Control)Label2_28).Cursor = Cursors.Default;
		((Control)Label2_28).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_28).ForeColor = SystemColors.ControlText;
		((Control)Label2_28).Location = new Point(376, 206);
		((Control)Label2_28).Name = "Label2_28";
		((Control)Label2_28).RightToLeft = (RightToLeft)0;
		((Control)Label2_28).Size = new Size(6, 20);
		((Control)Label2_28).TabIndex = 211;
		((Control)Label2_28).Text = ":";
		Label2_28.TextAlign = (ContentAlignment)32;
		((Control)Label2_27).BackColor = Color.Transparent;
		((Control)Label2_27).Cursor = Cursors.Default;
		((Control)Label2_27).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_27).ForeColor = SystemColors.ControlText;
		((Control)Label2_27).Location = new Point(376, 178);
		((Control)Label2_27).Name = "Label2_27";
		((Control)Label2_27).RightToLeft = (RightToLeft)0;
		((Control)Label2_27).Size = new Size(6, 20);
		((Control)Label2_27).TabIndex = 210;
		((Control)Label2_27).Text = ":";
		Label2_27.TextAlign = (ContentAlignment)32;
		((Control)Label2_26).BackColor = Color.Transparent;
		((Control)Label2_26).Cursor = Cursors.Default;
		((Control)Label2_26).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_26).ForeColor = SystemColors.ControlText;
		((Control)Label2_26).Location = new Point(376, 151);
		((Control)Label2_26).Name = "Label2_26";
		((Control)Label2_26).RightToLeft = (RightToLeft)0;
		((Control)Label2_26).Size = new Size(6, 20);
		((Control)Label2_26).TabIndex = 209;
		((Control)Label2_26).Text = ":";
		Label2_26.TextAlign = (ContentAlignment)32;
		((Control)Label2_25).BackColor = Color.Transparent;
		((Control)Label2_25).Cursor = Cursors.Default;
		((Control)Label2_25).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_25).ForeColor = SystemColors.ControlText;
		((Control)Label2_25).Location = new Point(376, 123);
		((Control)Label2_25).Name = "Label2_25";
		((Control)Label2_25).RightToLeft = (RightToLeft)0;
		((Control)Label2_25).Size = new Size(6, 20);
		((Control)Label2_25).TabIndex = 208;
		((Control)Label2_25).Text = ":";
		Label2_25.TextAlign = (ContentAlignment)32;
		((Control)Label2_23).BackColor = Color.Transparent;
		((Control)Label2_23).Cursor = Cursors.Default;
		((Control)Label2_23).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_23).ForeColor = SystemColors.ControlText;
		((Control)Label2_23).Location = new Point(230, 290);
		((Control)Label2_23).Name = "Label2_23";
		((Control)Label2_23).RightToLeft = (RightToLeft)0;
		((Control)Label2_23).Size = new Size(66, 20);
		((Control)Label2_23).TabIndex = 207;
		((Control)Label2_23).Text = "Point 16:";
		((Control)Label2_22).BackColor = Color.Transparent;
		((Control)Label2_22).Cursor = Cursors.Default;
		((Control)Label2_22).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_22).ForeColor = SystemColors.ControlText;
		((Control)Label2_22).Location = new Point(230, 262);
		((Control)Label2_22).Name = "Label2_22";
		((Control)Label2_22).RightToLeft = (RightToLeft)0;
		((Control)Label2_22).Size = new Size(66, 20);
		((Control)Label2_22).TabIndex = 206;
		((Control)Label2_22).Text = "Point 15:";
		((Control)Label2_21).BackColor = Color.Transparent;
		((Control)Label2_21).Cursor = Cursors.Default;
		((Control)Label2_21).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_21).ForeColor = SystemColors.ControlText;
		((Control)Label2_21).Location = new Point(230, 234);
		((Control)Label2_21).Name = "Label2_21";
		((Control)Label2_21).RightToLeft = (RightToLeft)0;
		((Control)Label2_21).Size = new Size(66, 20);
		((Control)Label2_21).TabIndex = 205;
		((Control)Label2_21).Text = "Point 14:";
		((Control)Label2_20).BackColor = Color.Transparent;
		((Control)Label2_20).Cursor = Cursors.Default;
		((Control)Label2_20).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_20).ForeColor = SystemColors.ControlText;
		((Control)Label2_20).Location = new Point(230, 206);
		((Control)Label2_20).Name = "Label2_20";
		((Control)Label2_20).RightToLeft = (RightToLeft)0;
		((Control)Label2_20).Size = new Size(66, 20);
		((Control)Label2_20).TabIndex = 204;
		((Control)Label2_20).Text = "Point 13:";
		((Control)Label2_19).BackColor = Color.Transparent;
		((Control)Label2_19).Cursor = Cursors.Default;
		((Control)Label2_19).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_19).ForeColor = SystemColors.ControlText;
		((Control)Label2_19).Location = new Point(230, 178);
		((Control)Label2_19).Name = "Label2_19";
		((Control)Label2_19).RightToLeft = (RightToLeft)0;
		((Control)Label2_19).Size = new Size(66, 20);
		((Control)Label2_19).TabIndex = 203;
		((Control)Label2_19).Text = "Point 12:";
		((Control)Label2_18).BackColor = Color.Transparent;
		((Control)Label2_18).Cursor = Cursors.Default;
		((Control)Label2_18).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_18).ForeColor = SystemColors.ControlText;
		((Control)Label2_18).Location = new Point(230, 151);
		((Control)Label2_18).Name = "Label2_18";
		((Control)Label2_18).RightToLeft = (RightToLeft)0;
		((Control)Label2_18).Size = new Size(66, 20);
		((Control)Label2_18).TabIndex = 202;
		((Control)Label2_18).Text = "Point 11:";
		((Control)Label2_17).BackColor = Color.Transparent;
		((Control)Label2_17).Cursor = Cursors.Default;
		((Control)Label2_17).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_17).ForeColor = SystemColors.ControlText;
		((Control)Label2_17).Location = new Point(230, 123);
		((Control)Label2_17).Name = "Label2_17";
		((Control)Label2_17).RightToLeft = (RightToLeft)0;
		((Control)Label2_17).Size = new Size(66, 20);
		((Control)Label2_17).TabIndex = 201;
		((Control)Label2_17).Text = "Point 10:";
		((Control)Label2_16).BackColor = Color.Transparent;
		((Control)Label2_16).Cursor = Cursors.Default;
		((Control)Label2_16).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_16).ForeColor = SystemColors.ControlText;
		((Control)Label2_16).Location = new Point(230, 96);
		((Control)Label2_16).Name = "Label2_16";
		((Control)Label2_16).RightToLeft = (RightToLeft)0;
		((Control)Label2_16).Size = new Size(66, 20);
		((Control)Label2_16).TabIndex = 200;
		((Control)Label2_16).Text = "Point 9:";
		txtBellCount.AcceptsReturn = true;
		((Control)txtBellCount).BackColor = SystemColors.Window;
		((Control)txtBellCount).Cursor = Cursors.IBeam;
		((Control)txtBellCount).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtBellCount).ForeColor = SystemColors.WindowText;
		((Control)txtBellCount).Location = new Point(330, 334);
		((TextBoxBase)txtBellCount).MaxLength = 0;
		((Control)txtBellCount).Name = "txtBellCount";
		((Control)txtBellCount).RightToLeft = (RightToLeft)0;
		((Control)txtBellCount).Size = new Size(39, 29);
		((Control)txtBellCount).TabIndex = 135;
		txtHour7.AcceptsReturn = true;
		((Control)txtHour7).BackColor = SystemColors.Window;
		((Control)txtHour7).Cursor = Cursors.IBeam;
		((Control)txtHour7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour7).ForeColor = SystemColors.WindowText;
		((Control)txtHour7).Location = new Point(106, 287);
		((TextBoxBase)txtHour7).MaxLength = 0;
		((Control)txtHour7).Name = "txtHour7";
		((Control)txtHour7).RightToLeft = (RightToLeft)0;
		((Control)txtHour7).Size = new Size(42, 28);
		((Control)txtHour7).TabIndex = 155;
		txtMinute7.AcceptsReturn = true;
		((Control)txtMinute7).BackColor = SystemColors.Window;
		((Control)txtMinute7).Cursor = Cursors.IBeam;
		((Control)txtMinute7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute7).ForeColor = SystemColors.WindowText;
		((Control)txtMinute7).Location = new Point(166, 287);
		((TextBoxBase)txtMinute7).MaxLength = 0;
		((Control)txtMinute7).Name = "txtMinute7";
		((Control)txtMinute7).RightToLeft = (RightToLeft)0;
		((Control)txtMinute7).Size = new Size(42, 28);
		((Control)txtMinute7).TabIndex = 156;
		txtMinute6.AcceptsReturn = true;
		((Control)txtMinute6).BackColor = SystemColors.Window;
		((Control)txtMinute6).Cursor = Cursors.IBeam;
		((Control)txtMinute6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute6).ForeColor = SystemColors.WindowText;
		((Control)txtMinute6).Location = new Point(166, 259);
		((TextBoxBase)txtMinute6).MaxLength = 0;
		((Control)txtMinute6).Name = "txtMinute6";
		((Control)txtMinute6).RightToLeft = (RightToLeft)0;
		((Control)txtMinute6).Size = new Size(42, 28);
		((Control)txtMinute6).TabIndex = 153;
		txtHour6.AcceptsReturn = true;
		((Control)txtHour6).BackColor = SystemColors.Window;
		((Control)txtHour6).Cursor = Cursors.IBeam;
		((Control)txtHour6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour6).ForeColor = SystemColors.WindowText;
		((Control)txtHour6).Location = new Point(106, 259);
		((TextBoxBase)txtHour6).MaxLength = 0;
		((Control)txtHour6).Name = "txtHour6";
		((Control)txtHour6).RightToLeft = (RightToLeft)0;
		((Control)txtHour6).Size = new Size(42, 28);
		((Control)txtHour6).TabIndex = 152;
		txtHour5.AcceptsReturn = true;
		((Control)txtHour5).BackColor = SystemColors.Window;
		((Control)txtHour5).Cursor = Cursors.IBeam;
		((Control)txtHour5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour5).ForeColor = SystemColors.WindowText;
		((Control)txtHour5).Location = new Point(106, 231);
		((TextBoxBase)txtHour5).MaxLength = 0;
		((Control)txtHour5).Name = "txtHour5";
		((Control)txtHour5).RightToLeft = (RightToLeft)0;
		((Control)txtHour5).Size = new Size(42, 28);
		((Control)txtHour5).TabIndex = 149;
		txtMinute5.AcceptsReturn = true;
		((Control)txtMinute5).BackColor = SystemColors.Window;
		((Control)txtMinute5).Cursor = Cursors.IBeam;
		((Control)txtMinute5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute5).ForeColor = SystemColors.WindowText;
		((Control)txtMinute5).Location = new Point(166, 231);
		((TextBoxBase)txtMinute5).MaxLength = 0;
		((Control)txtMinute5).Name = "txtMinute5";
		((Control)txtMinute5).RightToLeft = (RightToLeft)0;
		((Control)txtMinute5).Size = new Size(42, 28);
		((Control)txtMinute5).TabIndex = 150;
		txtMinute4.AcceptsReturn = true;
		((Control)txtMinute4).BackColor = SystemColors.Window;
		((Control)txtMinute4).Cursor = Cursors.IBeam;
		((Control)txtMinute4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute4).ForeColor = SystemColors.WindowText;
		((Control)txtMinute4).Location = new Point(166, 203);
		((TextBoxBase)txtMinute4).MaxLength = 0;
		((Control)txtMinute4).Name = "txtMinute4";
		((Control)txtMinute4).RightToLeft = (RightToLeft)0;
		((Control)txtMinute4).Size = new Size(42, 28);
		((Control)txtMinute4).TabIndex = 147;
		txtHour4.AcceptsReturn = true;
		((Control)txtHour4).BackColor = SystemColors.Window;
		((Control)txtHour4).Cursor = Cursors.IBeam;
		((Control)txtHour4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour4).ForeColor = SystemColors.WindowText;
		((Control)txtHour4).Location = new Point(106, 203);
		((TextBoxBase)txtHour4).MaxLength = 0;
		((Control)txtHour4).Name = "txtHour4";
		((Control)txtHour4).RightToLeft = (RightToLeft)0;
		((Control)txtHour4).Size = new Size(42, 28);
		((Control)txtHour4).TabIndex = 146;
		txtHour3.AcceptsReturn = true;
		((Control)txtHour3).BackColor = SystemColors.Window;
		((Control)txtHour3).Cursor = Cursors.IBeam;
		((Control)txtHour3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour3).ForeColor = SystemColors.WindowText;
		((Control)txtHour3).Location = new Point(106, 175);
		((TextBoxBase)txtHour3).MaxLength = 0;
		((Control)txtHour3).Name = "txtHour3";
		((Control)txtHour3).RightToLeft = (RightToLeft)0;
		((Control)txtHour3).Size = new Size(42, 28);
		((Control)txtHour3).TabIndex = 143;
		txtMinute3.AcceptsReturn = true;
		((Control)txtMinute3).BackColor = SystemColors.Window;
		((Control)txtMinute3).Cursor = Cursors.IBeam;
		((Control)txtMinute3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute3).ForeColor = SystemColors.WindowText;
		((Control)txtMinute3).Location = new Point(166, 175);
		((TextBoxBase)txtMinute3).MaxLength = 0;
		((Control)txtMinute3).Name = "txtMinute3";
		((Control)txtMinute3).RightToLeft = (RightToLeft)0;
		((Control)txtMinute3).Size = new Size(42, 28);
		((Control)txtMinute3).TabIndex = 144;
		txtMinute2.AcceptsReturn = true;
		((Control)txtMinute2).BackColor = SystemColors.Window;
		((Control)txtMinute2).Cursor = Cursors.IBeam;
		((Control)txtMinute2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute2).ForeColor = SystemColors.WindowText;
		((Control)txtMinute2).Location = new Point(166, 148);
		((TextBoxBase)txtMinute2).MaxLength = 0;
		((Control)txtMinute2).Name = "txtMinute2";
		((Control)txtMinute2).RightToLeft = (RightToLeft)0;
		((Control)txtMinute2).Size = new Size(42, 28);
		((Control)txtMinute2).TabIndex = 141;
		txtHour2.AcceptsReturn = true;
		((Control)txtHour2).BackColor = SystemColors.Window;
		((Control)txtHour2).Cursor = Cursors.IBeam;
		((Control)txtHour2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour2).ForeColor = SystemColors.WindowText;
		((Control)txtHour2).Location = new Point(106, 148);
		((TextBoxBase)txtHour2).MaxLength = 0;
		((Control)txtHour2).Name = "txtHour2";
		((Control)txtHour2).RightToLeft = (RightToLeft)0;
		((Control)txtHour2).Size = new Size(42, 28);
		((Control)txtHour2).TabIndex = 140;
		txtHour1.AcceptsReturn = true;
		((Control)txtHour1).BackColor = SystemColors.Window;
		((Control)txtHour1).Cursor = Cursors.IBeam;
		((Control)txtHour1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour1).ForeColor = SystemColors.WindowText;
		((Control)txtHour1).Location = new Point(106, 120);
		((TextBoxBase)txtHour1).MaxLength = 0;
		((Control)txtHour1).Name = "txtHour1";
		((Control)txtHour1).RightToLeft = (RightToLeft)0;
		((Control)txtHour1).Size = new Size(42, 28);
		((Control)txtHour1).TabIndex = 137;
		txtMinute1.AcceptsReturn = true;
		((Control)txtMinute1).BackColor = SystemColors.Window;
		((Control)txtMinute1).Cursor = Cursors.IBeam;
		((Control)txtMinute1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute1).ForeColor = SystemColors.WindowText;
		((Control)txtMinute1).Location = new Point(166, 120);
		((TextBoxBase)txtMinute1).MaxLength = 0;
		((Control)txtMinute1).Name = "txtMinute1";
		((Control)txtMinute1).RightToLeft = (RightToLeft)0;
		((Control)txtMinute1).Size = new Size(42, 28);
		((Control)txtMinute1).TabIndex = 138;
		txtMinute0.AcceptsReturn = true;
		((Control)txtMinute0).BackColor = SystemColors.Window;
		((Control)txtMinute0).Cursor = Cursors.IBeam;
		((Control)txtMinute0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtMinute0).ForeColor = SystemColors.WindowText;
		((Control)txtMinute0).Location = new Point(166, 92);
		((TextBoxBase)txtMinute0).MaxLength = 0;
		((Control)txtMinute0).Name = "txtMinute0";
		((Control)txtMinute0).RightToLeft = (RightToLeft)0;
		((Control)txtMinute0).Size = new Size(42, 28);
		((Control)txtMinute0).TabIndex = 134;
		txtHour0.AcceptsReturn = true;
		((Control)txtHour0).BackColor = SystemColors.Window;
		((Control)txtHour0).Cursor = Cursors.IBeam;
		((Control)txtHour0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtHour0).ForeColor = SystemColors.WindowText;
		((Control)txtHour0).Location = new Point(106, 92);
		((TextBoxBase)txtHour0).MaxLength = 0;
		((Control)txtHour0).Name = "txtHour0";
		((Control)txtHour0).RightToLeft = (RightToLeft)0;
		((Control)txtHour0).Size = new Size(42, 28);
		((Control)txtHour0).TabIndex = 132;
		((Control)chkValid7).BackColor = SystemColors.Control;
		((Control)chkValid7).Cursor = Cursors.Default;
		((Control)chkValid7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid7).ForeColor = SystemColors.ControlText;
		((Control)chkValid7).Location = new Point(80, 290);
		((Control)chkValid7).Name = "chkValid7";
		((Control)chkValid7).RightToLeft = (RightToLeft)0;
		((Control)chkValid7).Size = new Size(14, 20);
		((Control)chkValid7).TabIndex = 154;
		((Control)chkValid7).Text = "";
		((ButtonBase)chkValid7).UseVisualStyleBackColor = false;
		((Control)chkValid6).BackColor = SystemColors.Control;
		((Control)chkValid6).Cursor = Cursors.Default;
		((Control)chkValid6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid6).ForeColor = SystemColors.ControlText;
		((Control)chkValid6).Location = new Point(80, 262);
		((Control)chkValid6).Name = "chkValid6";
		((Control)chkValid6).RightToLeft = (RightToLeft)0;
		((Control)chkValid6).Size = new Size(14, 20);
		((Control)chkValid6).TabIndex = 151;
		((Control)chkValid6).Text = "";
		((ButtonBase)chkValid6).UseVisualStyleBackColor = false;
		((Control)chkValid5).BackColor = SystemColors.Control;
		((Control)chkValid5).Cursor = Cursors.Default;
		((Control)chkValid5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid5).ForeColor = SystemColors.ControlText;
		((Control)chkValid5).Location = new Point(80, 234);
		((Control)chkValid5).Name = "chkValid5";
		((Control)chkValid5).RightToLeft = (RightToLeft)0;
		((Control)chkValid5).Size = new Size(14, 20);
		((Control)chkValid5).TabIndex = 148;
		((Control)chkValid5).Text = "";
		((ButtonBase)chkValid5).UseVisualStyleBackColor = false;
		((Control)chkValid4).BackColor = SystemColors.Control;
		((Control)chkValid4).Cursor = Cursors.Default;
		((Control)chkValid4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid4).ForeColor = SystemColors.ControlText;
		((Control)chkValid4).Location = new Point(80, 206);
		((Control)chkValid4).Name = "chkValid4";
		((Control)chkValid4).RightToLeft = (RightToLeft)0;
		((Control)chkValid4).Size = new Size(14, 20);
		((Control)chkValid4).TabIndex = 145;
		((Control)chkValid4).Text = "";
		((ButtonBase)chkValid4).UseVisualStyleBackColor = false;
		((Control)chkValid3).BackColor = SystemColors.Control;
		((Control)chkValid3).Cursor = Cursors.Default;
		((Control)chkValid3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid3).ForeColor = SystemColors.ControlText;
		((Control)chkValid3).Location = new Point(80, 178);
		((Control)chkValid3).Name = "chkValid3";
		((Control)chkValid3).RightToLeft = (RightToLeft)0;
		((Control)chkValid3).Size = new Size(14, 20);
		((Control)chkValid3).TabIndex = 142;
		((Control)chkValid3).Text = "";
		((ButtonBase)chkValid3).UseVisualStyleBackColor = false;
		((Control)chkValid2).BackColor = SystemColors.Control;
		((Control)chkValid2).Cursor = Cursors.Default;
		((Control)chkValid2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid2).ForeColor = SystemColors.ControlText;
		((Control)chkValid2).Location = new Point(80, 151);
		((Control)chkValid2).Name = "chkValid2";
		((Control)chkValid2).RightToLeft = (RightToLeft)0;
		((Control)chkValid2).Size = new Size(14, 20);
		((Control)chkValid2).TabIndex = 139;
		((Control)chkValid2).Text = "";
		((ButtonBase)chkValid2).UseVisualStyleBackColor = false;
		((Control)chkValid1).BackColor = SystemColors.Control;
		((Control)chkValid1).Cursor = Cursors.Default;
		((Control)chkValid1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid1).ForeColor = SystemColors.ControlText;
		((Control)chkValid1).Location = new Point(80, 123);
		((Control)chkValid1).Name = "chkValid1";
		((Control)chkValid1).RightToLeft = (RightToLeft)0;
		((Control)chkValid1).Size = new Size(14, 20);
		((Control)chkValid1).TabIndex = 136;
		((Control)chkValid1).Text = "";
		((ButtonBase)chkValid1).UseVisualStyleBackColor = false;
		((Control)chkValid0).BackColor = SystemColors.Control;
		((Control)chkValid0).Cursor = Cursors.Default;
		((Control)chkValid0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkValid0).ForeColor = SystemColors.ControlText;
		((Control)chkValid0).Location = new Point(80, 96);
		((Control)chkValid0).Name = "chkValid0";
		((Control)chkValid0).RightToLeft = (RightToLeft)0;
		((Control)chkValid0).Size = new Size(14, 20);
		((Control)chkValid0).TabIndex = 130;
		((Control)chkValid0).Text = "";
		((ButtonBase)chkValid0).UseVisualStyleBackColor = false;
		((Control)cmdWrite).BackColor = SystemColors.Control;
		((Control)cmdWrite).Cursor = Cursors.Default;
		((Control)cmdWrite).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdWrite).ForeColor = SystemColors.ControlText;
		((Control)cmdWrite).Location = new Point(554, 334);
		((Control)cmdWrite).Name = "cmdWrite";
		((Control)cmdWrite).RightToLeft = (RightToLeft)0;
		((Control)cmdWrite).Size = new Size(99, 34);
		((Control)cmdWrite).TabIndex = 133;
		((Control)cmdWrite).Text = "Write";
		((ButtonBase)cmdWrite).UseVisualStyleBackColor = false;
		((Control)cmdWrite).Click += cmdWrite_Click;
		((Control)cmdRead).BackColor = SystemColors.Control;
		((Control)cmdRead).Cursor = Cursors.Default;
		((Control)cmdRead).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdRead).ForeColor = SystemColors.ControlText;
		((Control)cmdRead).Location = new Point(442, 334);
		((Control)cmdRead).Name = "cmdRead";
		((Control)cmdRead).RightToLeft = (RightToLeft)0;
		((Control)cmdRead).Size = new Size(99, 34);
		((Control)cmdRead).TabIndex = 131;
		((Control)cmdRead).Text = "Read";
		((ButtonBase)cmdRead).UseVisualStyleBackColor = false;
		((Control)cmdRead).Click += cmdRead_Click;
		((Control)Label2_8).BackColor = Color.Transparent;
		((Control)Label2_8).Cursor = Cursors.Default;
		((Control)Label2_8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_8).ForeColor = SystemColors.ControlText;
		((Control)Label2_8).Location = new Point(156, 96);
		((Control)Label2_8).Name = "Label2_8";
		((Control)Label2_8).RightToLeft = (RightToLeft)0;
		((Control)Label2_8).Size = new Size(6, 20);
		((Control)Label2_8).TabIndex = 175;
		((Control)Label2_8).Text = ":";
		Label2_8.TextAlign = (ContentAlignment)32;
		((Control)Label2_15).BackColor = Color.Transparent;
		((Control)Label2_15).Cursor = Cursors.Default;
		((Control)Label2_15).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_15).ForeColor = SystemColors.ControlText;
		((Control)Label2_15).Location = new Point(156, 290);
		((Control)Label2_15).Name = "Label2_15";
		((Control)Label2_15).RightToLeft = (RightToLeft)0;
		((Control)Label2_15).Size = new Size(6, 20);
		((Control)Label2_15).TabIndex = 174;
		((Control)Label2_15).Text = ":";
		Label2_15.TextAlign = (ContentAlignment)32;
		((Control)Label2_14).BackColor = Color.Transparent;
		((Control)Label2_14).Cursor = Cursors.Default;
		((Control)Label2_14).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_14).ForeColor = SystemColors.ControlText;
		((Control)Label2_14).Location = new Point(156, 262);
		((Control)Label2_14).Name = "Label2_14";
		((Control)Label2_14).RightToLeft = (RightToLeft)0;
		((Control)Label2_14).Size = new Size(6, 20);
		((Control)Label2_14).TabIndex = 173;
		((Control)Label2_14).Text = ":";
		Label2_14.TextAlign = (ContentAlignment)32;
		((Control)Label2_13).BackColor = Color.Transparent;
		((Control)Label2_13).Cursor = Cursors.Default;
		((Control)Label2_13).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_13).ForeColor = SystemColors.ControlText;
		((Control)Label2_13).Location = new Point(156, 234);
		((Control)Label2_13).Name = "Label2_13";
		((Control)Label2_13).RightToLeft = (RightToLeft)0;
		((Control)Label2_13).Size = new Size(6, 20);
		((Control)Label2_13).TabIndex = 172;
		((Control)Label2_13).Text = ":";
		Label2_13.TextAlign = (ContentAlignment)32;
		((Control)Label2_12).BackColor = Color.Transparent;
		((Control)Label2_12).Cursor = Cursors.Default;
		((Control)Label2_12).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_12).ForeColor = SystemColors.ControlText;
		((Control)Label2_12).Location = new Point(156, 206);
		((Control)Label2_12).Name = "Label2_12";
		((Control)Label2_12).RightToLeft = (RightToLeft)0;
		((Control)Label2_12).Size = new Size(6, 20);
		((Control)Label2_12).TabIndex = 171;
		((Control)Label2_12).Text = ":";
		Label2_12.TextAlign = (ContentAlignment)32;
		((Control)Label2_11).BackColor = Color.Transparent;
		((Control)Label2_11).Cursor = Cursors.Default;
		((Control)Label2_11).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_11).ForeColor = SystemColors.ControlText;
		((Control)Label2_11).Location = new Point(156, 178);
		((Control)Label2_11).Name = "Label2_11";
		((Control)Label2_11).RightToLeft = (RightToLeft)0;
		((Control)Label2_11).Size = new Size(6, 20);
		((Control)Label2_11).TabIndex = 170;
		((Control)Label2_11).Text = ":";
		Label2_11.TextAlign = (ContentAlignment)32;
		((Control)Label2_10).BackColor = Color.Transparent;
		((Control)Label2_10).Cursor = Cursors.Default;
		((Control)Label2_10).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_10).ForeColor = SystemColors.ControlText;
		((Control)Label2_10).Location = new Point(156, 151);
		((Control)Label2_10).Name = "Label2_10";
		((Control)Label2_10).RightToLeft = (RightToLeft)0;
		((Control)Label2_10).Size = new Size(6, 20);
		((Control)Label2_10).TabIndex = 169;
		((Control)Label2_10).Text = ":";
		Label2_10.TextAlign = (ContentAlignment)32;
		((Control)Label2_9).BackColor = Color.Transparent;
		((Control)Label2_9).Cursor = Cursors.Default;
		((Control)Label2_9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_9).ForeColor = SystemColors.ControlText;
		((Control)Label2_9).Location = new Point(156, 123);
		((Control)Label2_9).Name = "Label2_9";
		((Control)Label2_9).RightToLeft = (RightToLeft)0;
		((Control)Label2_9).Size = new Size(6, 20);
		((Control)Label2_9).TabIndex = 168;
		((Control)Label2_9).Text = ":";
		Label2_9.TextAlign = (ContentAlignment)32;
		((Control)lblCaption0).BackColor = Color.Transparent;
		((Control)lblCaption0).Cursor = Cursors.Default;
		((Control)lblCaption0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblCaption0).ForeColor = SystemColors.ControlText;
		((Control)lblCaption0).Location = new Point(12, 66);
		((Control)lblCaption0).Name = "lblCaption0";
		((Control)lblCaption0).RightToLeft = (RightToLeft)0;
		((Control)lblCaption0).Size = new Size(202, 20);
		((Control)lblCaption0).TabIndex = 167;
		((Control)lblCaption0).Text = "Point   UseFlag   Start Time";
		((Control)Label2_7).BackColor = Color.Transparent;
		((Control)Label2_7).Cursor = Cursors.Default;
		((Control)Label2_7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_7).ForeColor = SystemColors.ControlText;
		((Control)Label2_7).Location = new Point(10, 290);
		((Control)Label2_7).Name = "Label2_7";
		((Control)Label2_7).RightToLeft = (RightToLeft)0;
		((Control)Label2_7).Size = new Size(66, 20);
		((Control)Label2_7).TabIndex = 166;
		((Control)Label2_7).Text = "Point 8:";
		((Control)Label2_6).BackColor = Color.Transparent;
		((Control)Label2_6).Cursor = Cursors.Default;
		((Control)Label2_6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_6).ForeColor = SystemColors.ControlText;
		((Control)Label2_6).Location = new Point(10, 262);
		((Control)Label2_6).Name = "Label2_6";
		((Control)Label2_6).RightToLeft = (RightToLeft)0;
		((Control)Label2_6).Size = new Size(66, 20);
		((Control)Label2_6).TabIndex = 165;
		((Control)Label2_6).Text = "Point 7:";
		((Control)Label2_5).BackColor = Color.Transparent;
		((Control)Label2_5).Cursor = Cursors.Default;
		((Control)Label2_5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_5).ForeColor = SystemColors.ControlText;
		((Control)Label2_5).Location = new Point(10, 234);
		((Control)Label2_5).Name = "Label2_5";
		((Control)Label2_5).RightToLeft = (RightToLeft)0;
		((Control)Label2_5).Size = new Size(66, 20);
		((Control)Label2_5).TabIndex = 164;
		((Control)Label2_5).Text = "Point 6:";
		((Control)Label2_4).BackColor = Color.Transparent;
		((Control)Label2_4).Cursor = Cursors.Default;
		((Control)Label2_4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_4).ForeColor = SystemColors.ControlText;
		((Control)Label2_4).Location = new Point(10, 206);
		((Control)Label2_4).Name = "Label2_4";
		((Control)Label2_4).RightToLeft = (RightToLeft)0;
		((Control)Label2_4).Size = new Size(66, 20);
		((Control)Label2_4).TabIndex = 163;
		((Control)Label2_4).Text = "Point 5:";
		((Control)Label2_3).BackColor = Color.Transparent;
		((Control)Label2_3).Cursor = Cursors.Default;
		((Control)Label2_3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_3).ForeColor = SystemColors.ControlText;
		((Control)Label2_3).Location = new Point(10, 178);
		((Control)Label2_3).Name = "Label2_3";
		((Control)Label2_3).RightToLeft = (RightToLeft)0;
		((Control)Label2_3).Size = new Size(66, 20);
		((Control)Label2_3).TabIndex = 162;
		((Control)Label2_3).Text = "Point 4:";
		((Control)Label2_2).BackColor = Color.Transparent;
		((Control)Label2_2).Cursor = Cursors.Default;
		((Control)Label2_2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_2).ForeColor = SystemColors.ControlText;
		((Control)Label2_2).Location = new Point(10, 151);
		((Control)Label2_2).Name = "Label2_2";
		((Control)Label2_2).RightToLeft = (RightToLeft)0;
		((Control)Label2_2).Size = new Size(66, 20);
		((Control)Label2_2).TabIndex = 161;
		((Control)Label2_2).Text = "Point 3:";
		((Control)Label2_1).BackColor = Color.Transparent;
		((Control)Label2_1).Cursor = Cursors.Default;
		((Control)Label2_1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_1).ForeColor = SystemColors.ControlText;
		((Control)Label2_1).Location = new Point(10, 123);
		((Control)Label2_1).Name = "Label2_1";
		((Control)Label2_1).RightToLeft = (RightToLeft)0;
		((Control)Label2_1).Size = new Size(66, 20);
		((Control)Label2_1).TabIndex = 160;
		((Control)Label2_1).Text = "Point 2:";
		((Control)Label2_0).BackColor = Color.Transparent;
		((Control)Label2_0).Cursor = Cursors.Default;
		((Control)Label2_0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2_0).ForeColor = SystemColors.ControlText;
		((Control)Label2_0).Location = new Point(10, 96);
		((Control)Label2_0).Name = "Label2_0";
		((Control)Label2_0).RightToLeft = (RightToLeft)0;
		((Control)Label2_0).Size = new Size(66, 20);
		((Control)Label2_0).TabIndex = 159;
		((Control)Label2_0).Text = "Point 1:";
		((Control)Label1).BackColor = Color.Transparent;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(241, 340);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(91, 19);
		((Control)Label1).TabIndex = 158;
		((Control)Label1).Text = "Bell Count :";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(15, 15);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(635, 28);
		((Control)lblMessage).TabIndex = 157;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Form)this).AutoScaleBaseSize = new Size(8, 19);
		((Form)this).ClientSize = new Size(664, 382);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)txtHour23);
		((Control)this).Controls.Add((Control)(object)txtMinute23);
		((Control)this).Controls.Add((Control)(object)txtMinute22);
		((Control)this).Controls.Add((Control)(object)txtHour22);
		((Control)this).Controls.Add((Control)(object)txtHour21);
		((Control)this).Controls.Add((Control)(object)txtMinute21);
		((Control)this).Controls.Add((Control)(object)txtMinute20);
		((Control)this).Controls.Add((Control)(object)txtHour20);
		((Control)this).Controls.Add((Control)(object)txtHour19);
		((Control)this).Controls.Add((Control)(object)txtMinute19);
		((Control)this).Controls.Add((Control)(object)txtMinute18);
		((Control)this).Controls.Add((Control)(object)txtHour18);
		((Control)this).Controls.Add((Control)(object)txtHour17);
		((Control)this).Controls.Add((Control)(object)txtMinute17);
		((Control)this).Controls.Add((Control)(object)txtMinute16);
		((Control)this).Controls.Add((Control)(object)txtHour16);
		((Control)this).Controls.Add((Control)(object)txtHour15);
		((Control)this).Controls.Add((Control)(object)txtMinute15);
		((Control)this).Controls.Add((Control)(object)txtMinute14);
		((Control)this).Controls.Add((Control)(object)txtHour14);
		((Control)this).Controls.Add((Control)(object)txtHour13);
		((Control)this).Controls.Add((Control)(object)txtMinute13);
		((Control)this).Controls.Add((Control)(object)txtMinute12);
		((Control)this).Controls.Add((Control)(object)txtHour12);
		((Control)this).Controls.Add((Control)(object)txtHour11);
		((Control)this).Controls.Add((Control)(object)txtMinute11);
		((Control)this).Controls.Add((Control)(object)txtMinute10);
		((Control)this).Controls.Add((Control)(object)txtHour10);
		((Control)this).Controls.Add((Control)(object)txtHour9);
		((Control)this).Controls.Add((Control)(object)txtMinute9);
		((Control)this).Controls.Add((Control)(object)txtMinute8);
		((Control)this).Controls.Add((Control)(object)txtHour8);
		((Control)this).Controls.Add((Control)(object)txtBellCount);
		((Control)this).Controls.Add((Control)(object)txtHour7);
		((Control)this).Controls.Add((Control)(object)txtMinute7);
		((Control)this).Controls.Add((Control)(object)txtMinute6);
		((Control)this).Controls.Add((Control)(object)txtHour6);
		((Control)this).Controls.Add((Control)(object)txtHour5);
		((Control)this).Controls.Add((Control)(object)txtMinute5);
		((Control)this).Controls.Add((Control)(object)txtMinute4);
		((Control)this).Controls.Add((Control)(object)txtHour4);
		((Control)this).Controls.Add((Control)(object)txtHour3);
		((Control)this).Controls.Add((Control)(object)txtMinute3);
		((Control)this).Controls.Add((Control)(object)txtMinute2);
		((Control)this).Controls.Add((Control)(object)txtHour2);
		((Control)this).Controls.Add((Control)(object)txtHour1);
		((Control)this).Controls.Add((Control)(object)txtMinute1);
		((Control)this).Controls.Add((Control)(object)txtMinute0);
		((Control)this).Controls.Add((Control)(object)txtHour0);
		((Control)this).Controls.Add((Control)(object)chkValid23);
		((Control)this).Controls.Add((Control)(object)chkValid22);
		((Control)this).Controls.Add((Control)(object)chkValid21);
		((Control)this).Controls.Add((Control)(object)chkValid20);
		((Control)this).Controls.Add((Control)(object)chkValid19);
		((Control)this).Controls.Add((Control)(object)chkValid18);
		((Control)this).Controls.Add((Control)(object)chkValid17);
		((Control)this).Controls.Add((Control)(object)chkValid16);
		((Control)this).Controls.Add((Control)(object)Label2_40);
		((Control)this).Controls.Add((Control)(object)Label2_47);
		((Control)this).Controls.Add((Control)(object)Label2_46);
		((Control)this).Controls.Add((Control)(object)Label2_45);
		((Control)this).Controls.Add((Control)(object)Label2_44);
		((Control)this).Controls.Add((Control)(object)Label2_43);
		((Control)this).Controls.Add((Control)(object)Label2_42);
		((Control)this).Controls.Add((Control)(object)Label2_41);
		((Control)this).Controls.Add((Control)(object)Label2_39);
		((Control)this).Controls.Add((Control)(object)Label2_38);
		((Control)this).Controls.Add((Control)(object)Label2_37);
		((Control)this).Controls.Add((Control)(object)Label2_36);
		((Control)this).Controls.Add((Control)(object)Label2_35);
		((Control)this).Controls.Add((Control)(object)Label2_34);
		((Control)this).Controls.Add((Control)(object)Label2_33);
		((Control)this).Controls.Add((Control)(object)Label2_32);
		((Control)this).Controls.Add((Control)(object)chkValid15);
		((Control)this).Controls.Add((Control)(object)chkValid14);
		((Control)this).Controls.Add((Control)(object)chkValid13);
		((Control)this).Controls.Add((Control)(object)chkValid12);
		((Control)this).Controls.Add((Control)(object)chkValid11);
		((Control)this).Controls.Add((Control)(object)chkValid10);
		((Control)this).Controls.Add((Control)(object)chkValid9);
		((Control)this).Controls.Add((Control)(object)chkValid8);
		((Control)this).Controls.Add((Control)(object)Label2_24);
		((Control)this).Controls.Add((Control)(object)Label2_31);
		((Control)this).Controls.Add((Control)(object)Label2_30);
		((Control)this).Controls.Add((Control)(object)Label2_29);
		((Control)this).Controls.Add((Control)(object)Label2_28);
		((Control)this).Controls.Add((Control)(object)Label2_27);
		((Control)this).Controls.Add((Control)(object)Label2_26);
		((Control)this).Controls.Add((Control)(object)Label2_25);
		((Control)this).Controls.Add((Control)(object)Label2_23);
		((Control)this).Controls.Add((Control)(object)Label2_22);
		((Control)this).Controls.Add((Control)(object)Label2_21);
		((Control)this).Controls.Add((Control)(object)Label2_20);
		((Control)this).Controls.Add((Control)(object)Label2_19);
		((Control)this).Controls.Add((Control)(object)Label2_18);
		((Control)this).Controls.Add((Control)(object)Label2_17);
		((Control)this).Controls.Add((Control)(object)Label2_16);
		((Control)this).Controls.Add((Control)(object)chkValid7);
		((Control)this).Controls.Add((Control)(object)chkValid6);
		((Control)this).Controls.Add((Control)(object)chkValid5);
		((Control)this).Controls.Add((Control)(object)chkValid4);
		((Control)this).Controls.Add((Control)(object)chkValid3);
		((Control)this).Controls.Add((Control)(object)chkValid2);
		((Control)this).Controls.Add((Control)(object)chkValid1);
		((Control)this).Controls.Add((Control)(object)chkValid0);
		((Control)this).Controls.Add((Control)(object)cmdWrite);
		((Control)this).Controls.Add((Control)(object)cmdRead);
		((Control)this).Controls.Add((Control)(object)Label2_8);
		((Control)this).Controls.Add((Control)(object)Label2_15);
		((Control)this).Controls.Add((Control)(object)Label2_14);
		((Control)this).Controls.Add((Control)(object)Label2_13);
		((Control)this).Controls.Add((Control)(object)Label2_12);
		((Control)this).Controls.Add((Control)(object)Label2_11);
		((Control)this).Controls.Add((Control)(object)Label2_10);
		((Control)this).Controls.Add((Control)(object)Label2_9);
		((Control)this).Controls.Add((Control)(object)lblCaption0);
		((Control)this).Controls.Add((Control)(object)Label2_7);
		((Control)this).Controls.Add((Control)(object)Label2_6);
		((Control)this).Controls.Add((Control)(object)Label2_5);
		((Control)this).Controls.Add((Control)(object)Label2_4);
		((Control)this).Controls.Add((Control)(object)Label2_3);
		((Control)this).Controls.Add((Control)(object)Label2_2);
		((Control)this).Controls.Add((Control)(object)Label2_1);
		((Control)this).Controls.Add((Control)(object)Label2_0);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Font = new Font("Times New Roman", 12f, (FontStyle)1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Location = new Point(3, 29);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmBellInfo";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Setting Bell Time";
		((Form)this).Load += frmBellInfo_Load;
		((Control)this).ResumeLayout(false);
	}

	private void cmdRead_Click(object sender, EventArgs e)
	{
		byte[] array = new byte[1024];
		((Control)cmdRead).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdRead).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_GetBellTime(FKAttendDLL.nCommHandleIndex, ref mnBellCount, array);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		if (num == 1)
		{
			object obj = FKAttendDLL.ConvertByteArrayToStructure(array, typeof(BELLINFO));
			if (obj == null)
			{
				return;
			}
			mBellInfo = (BELLINFO)obj;
			ShowValue();
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdRead).Enabled = true;
	}

	private void cmdWrite_Click(object sender, EventArgs e)
	{
		byte[] array = new byte[1024];
		((Control)cmdWrite).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		GetValue();
		FKAttendDLL.ConvertStructureToByteArray(mBellInfo, array);
		int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdWrite).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_SetBellTime(FKAttendDLL.nCommHandleIndex, mnBellCount, array);
		((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdWrite).Enabled = true;
	}

	private void frmBellInfo_Load(object sender, EventArgs e)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		mBellInfo.mValid = new byte[24];
		mBellInfo.mHour = new byte[24];
		mBellInfo.mMinute = new byte[24];
		foreach (object item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (item == null)
			{
				continue;
			}
			string name = ((Control)item).Name;
			string text = "chkValid";
			int length = text.Length;
			if (name.Length > length && name.Substring(0, length) == text)
			{
				int num = Convert.ToInt32(name.Substring(length, name.Length - length));
				fchkValid[num] = (CheckBox)item;
				continue;
			}
			text = "txtHour";
			length = text.Length;
			if (name.Length > length && name.Substring(0, length) == text)
			{
				int num = Convert.ToInt32(name.Substring(length, name.Length - length));
				ftxtHour[num] = (TextBox)item;
				continue;
			}
			text = "txtMinute";
			length = text.Length;
			if (name.Length > length && name.Substring(0, length) == text)
			{
				int num = Convert.ToInt32(name.Substring(length, name.Length - length));
				ftxtMinute[num] = (TextBox)item;
			}
		}
		ShowValue();
	}

	private void ShowValue()
	{
		for (int i = 0; i < 24; i++)
		{
			((Control)ftxtHour[i]).Text = mBellInfo.mHour[i].ToString();
			((Control)ftxtMinute[i]).Text = mBellInfo.mMinute[i].ToString();
			if (mBellInfo.mValid[i] > 1)
			{
				mBellInfo.mValid[i] = 0;
			}
			fchkValid[i].CheckState = (CheckState)(mBellInfo.mValid[i] == 1);
		}
		((Control)txtBellCount).Text = Convert.ToString(mnBellCount);
	}

	private void GetValue()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 24; i++)
		{
			mBellInfo.mHour[i] = Convert.ToByte(((Control)ftxtHour[i]).Text);
			mBellInfo.mMinute[i] = Convert.ToByte(((Control)ftxtMinute[i]).Text);
			mBellInfo.mValid[i] = Convert.ToByte(fchkValid[i].CheckState);
		}
		mnBellCount = Convert.ToInt32(((Control)txtBellCount).Text);
	}
}
