using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmBellInfo : Form
{
	private AxSBXPC bpc;

	private int mBellCount;

	private const int MAX_BELLCOUNT_DAY = 24;

	private const int dataLen = 72;

	private CheckBox[] chkValid;

	private TextBox[] txtHour;

	private TextBox[] txtMinute;

	private byte[] dbBellInfoList;

	private IContainer components = null;

	public CheckBox _chkValid_11;

	public TextBox _txtMinute_11;

	public TextBox _txtHour_11;

	public CheckBox _chkValid_10;

	public TextBox _txtMinute_10;

	public TextBox _txtHour_10;

	public CheckBox _chkValid_9;

	public TextBox _txtMinute_9;

	public TextBox _txtHour_9;

	public CheckBox _chkValid_8;

	public TextBox _txtMinute_8;

	public TextBox _txtHour_8;

	public TextBox txtBellCount;

	public TextBox _txtHour_7;

	public TextBox _txtMinute_7;

	public TextBox _txtMinute_6;

	public TextBox _txtHour_6;

	public TextBox _txtHour_5;

	public TextBox _txtMinute_5;

	public TextBox _txtMinute_4;

	public TextBox _txtHour_4;

	public TextBox _txtHour_3;

	public TextBox _txtMinute_3;

	public TextBox _txtMinute_2;

	public TextBox _txtHour_2;

	public TextBox _txtHour_1;

	public TextBox _txtMinute_1;

	public TextBox _txtMinute_0;

	public TextBox _txtHour_0;

	public CheckBox _chkValid_7;

	public CheckBox _chkValid_6;

	public CheckBox _chkValid_5;

	public CheckBox _chkValid_4;

	public CheckBox _chkValid_3;

	public CheckBox _chkValid_2;

	public CheckBox _chkValid_1;

	public Button cmdExit;

	public Button cmdWrite;

	public Button cmdRead;

	public Label _Label2_23;

	public Label _Label2_22;

	public Label _Label2_21;

	public Label _Label2_20;

	public Label _Label2_19;

	public Label _Label2_18;

	public Label _Label2_17;

	public Label _Label2_16;

	public Label _Label2_15;

	public Label _Label2_14;

	public Label _Label2_13;

	public Label _Label2_12;

	public Label _Label2_11;

	public Label _Label2_10;

	public Label _Label2_9;

	public Label _Label2_8;

	public Label Label5;

	public Label Label3;

	public Label Label4;

	public Label _Label2_7;

	public Label _Label2_6;

	public Label _Label2_5;

	public Label _Label2_4;

	public Label _Label2_3;

	public Label _Label2_2;

	public Label _Label2_1;

	public Label _Label2_0;

	public Label Label1;

	public Label lblMessage;

	public CheckBox _chkValid_23;

	public TextBox _txtMinute_23;

	public TextBox _txtHour_23;

	public CheckBox _chkValid_22;

	public TextBox _txtMinute_22;

	public TextBox _txtHour_22;

	public CheckBox _chkValid_21;

	public TextBox _txtMinute_21;

	public TextBox _txtHour_21;

	public CheckBox _chkValid_20;

	public TextBox _txtMinute_20;

	public TextBox _txtHour_20;

	public TextBox _txtHour_19;

	public TextBox _txtMinute_19;

	public TextBox _txtMinute_18;

	public TextBox _txtHour_18;

	public TextBox _txtHour_17;

	public TextBox _txtMinute_17;

	public TextBox _txtMinute_16;

	public TextBox _txtHour_16;

	public TextBox _txtHour_15;

	public TextBox _txtMinute_15;

	public TextBox _txtMinute_14;

	public TextBox _txtHour_14;

	public TextBox _txtHour_13;

	public TextBox _txtMinute_13;

	public TextBox _txtMinute_12;

	public TextBox _txtHour_12;

	public CheckBox _chkValid_19;

	public CheckBox _chkValid_18;

	public CheckBox _chkValid_17;

	public CheckBox _chkValid_16;

	public CheckBox _chkValid_15;

	public CheckBox _chkValid_14;

	public CheckBox _chkValid_13;

	public CheckBox _chkValid_12;

	public Label label2;

	public Label label6;

	public Label label7;

	public Label label8;

	public Label label9;

	public Label label10;

	public Label label11;

	public Label label12;

	public Label label13;

	public Label label14;

	public Label label15;

	public Label label16;

	public Label label17;

	public Label label18;

	public Label label19;

	public Label label20;

	public Label label21;

	public Label label22;

	public Label label23;

	public Label label24;

	public Label label25;

	public Label label26;

	public Label label27;

	public Label label28;

	public Label label29;

	public Label label30;

	public Label label31;

	public CheckBox _chkValid_0;

	public frmBellInfo(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void BellInfoInit()
	{
		txtHour = (TextBox[])(object)new TextBox[24];
		txtMinute = (TextBox[])(object)new TextBox[24];
		chkValid = (CheckBox[])(object)new CheckBox[24];
		dbBellInfoList = new byte[72];
		txtHour[0] = _txtHour_0;
		txtHour[1] = _txtHour_1;
		txtHour[2] = _txtHour_2;
		txtHour[3] = _txtHour_3;
		txtHour[4] = _txtHour_4;
		txtHour[5] = _txtHour_5;
		txtHour[6] = _txtHour_6;
		txtHour[7] = _txtHour_7;
		txtHour[8] = _txtHour_8;
		txtHour[9] = _txtHour_9;
		txtHour[10] = _txtHour_10;
		txtHour[11] = _txtHour_11;
		txtHour[12] = _txtHour_12;
		txtHour[13] = _txtHour_13;
		txtHour[14] = _txtHour_14;
		txtHour[15] = _txtHour_15;
		txtHour[16] = _txtHour_16;
		txtHour[17] = _txtHour_17;
		txtHour[18] = _txtHour_18;
		txtHour[19] = _txtHour_19;
		txtHour[20] = _txtHour_20;
		txtHour[21] = _txtHour_21;
		txtHour[22] = _txtHour_22;
		txtHour[23] = _txtHour_23;
		txtMinute[0] = _txtMinute_0;
		txtMinute[1] = _txtMinute_1;
		txtMinute[2] = _txtMinute_2;
		txtMinute[3] = _txtMinute_3;
		txtMinute[4] = _txtMinute_4;
		txtMinute[5] = _txtMinute_5;
		txtMinute[6] = _txtMinute_6;
		txtMinute[7] = _txtMinute_7;
		txtMinute[8] = _txtMinute_8;
		txtMinute[9] = _txtMinute_9;
		txtMinute[10] = _txtMinute_10;
		txtMinute[11] = _txtMinute_11;
		txtMinute[12] = _txtMinute_12;
		txtMinute[13] = _txtMinute_13;
		txtMinute[14] = _txtMinute_14;
		txtMinute[15] = _txtMinute_15;
		txtMinute[16] = _txtMinute_16;
		txtMinute[17] = _txtMinute_17;
		txtMinute[18] = _txtMinute_18;
		txtMinute[19] = _txtMinute_19;
		txtMinute[20] = _txtMinute_20;
		txtMinute[21] = _txtMinute_21;
		txtMinute[22] = _txtMinute_22;
		txtMinute[23] = _txtMinute_23;
		chkValid[0] = _chkValid_0;
		chkValid[1] = _chkValid_1;
		chkValid[2] = _chkValid_2;
		chkValid[3] = _chkValid_3;
		chkValid[4] = _chkValid_4;
		chkValid[5] = _chkValid_5;
		chkValid[6] = _chkValid_6;
		chkValid[7] = _chkValid_7;
		chkValid[8] = _chkValid_8;
		chkValid[9] = _chkValid_9;
		chkValid[10] = _chkValid_10;
		chkValid[11] = _chkValid_11;
		chkValid[12] = _chkValid_12;
		chkValid[13] = _chkValid_13;
		chkValid[14] = _chkValid_14;
		chkValid[15] = _chkValid_15;
		chkValid[16] = _chkValid_16;
		chkValid[17] = _chkValid_17;
		chkValid[18] = _chkValid_18;
		chkValid[19] = _chkValid_19;
		chkValid[20] = _chkValid_20;
		chkValid[21] = _chkValid_21;
		chkValid[22] = _chkValid_22;
		chkValid[23] = _chkValid_23;
	}

	private void ShowValue()
	{
		for (int i = 0; i < 24; i++)
		{
			if (dbBellInfoList[i] > 1)
			{
				dbBellInfoList[i] = 0;
			}
			chkValid[i].Checked = dbBellInfoList[i] == 1;
			((Control)txtHour[i]).Text = Convert.ToString(dbBellInfoList[i + 24]);
			((Control)txtMinute[i]).Text = Convert.ToString(dbBellInfoList[i + 48]);
		}
		((Control)txtBellCount).Text = Convert.ToString(mBellCount);
	}

	private bool GetValue()
	{
		for (int i = 0; i < 24; i++)
		{
			dbBellInfoList[i] = (byte)(chkValid[i].Checked ? 1 : 0);
			if (Convert.ToInt32((((Control)txtHour[i]).Text == "") ? "0" : ((Control)txtHour[i]).Text) < 0 || Convert.ToInt32((((Control)txtHour[i]).Text == "") ? "0" : ((Control)txtHour[i]).Text) > 23)
			{
				return false;
			}
			dbBellInfoList[i + 24] = Convert.ToByte((((Control)txtHour[i]).Text == "") ? "0" : ((Control)txtHour[i]).Text);
			if (Convert.ToInt32((((Control)txtMinute[i]).Text == "") ? "0" : ((Control)txtMinute[i]).Text) < 0 || Convert.ToInt32((((Control)txtMinute[i]).Text == "") ? "0" : ((Control)txtMinute[i]).Text) > 59)
			{
				return false;
			}
			dbBellInfoList[i + 48] = Convert.ToByte((((Control)txtMinute[i]).Text == "") ? "0" : ((Control)txtMinute[i]).Text);
		}
		mBellCount = Convert.ToInt32((((Control)txtBellCount).Text == "") ? "0" : ((Control)txtBellCount).Text);
		return true;
	}

	private void cmdRead_Click(object sender, EventArgs e)
	{
		bool flag = true;
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Waiting...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		GCHandle gCHandle = GCHandle.Alloc(dbBellInfoList, GCHandleType.Pinned);
		int dwBellInfo = gCHandle.AddrOfPinnedObject().ToInt32();
		if (bpc.GetBellTime(util.gMachineNumber, ref mBellCount, ref dwBellInfo))
		{
			ShowValue();
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		gCHandle.Free();
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdWrite_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Waiting...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (!GetValue())
		{
			util.MessageBox(new IntPtr(0), "Invalid Value.", "Bell info", 1u);
		}
		else
		{
			GCHandle gCHandle = GCHandle.Alloc(dbBellInfoList, GCHandleType.Pinned);
			int dwBellInfo = gCHandle.AddrOfPinnedObject().ToInt32();
			bool flag = bpc.SetBellTime(util.gMachineNumber, mBellCount, ref dwBellInfo);
			gCHandle.Free();
			if (flag)
			{
				ShowValue();
				((Control)lblMessage).Text = "Success!";
			}
			else
			{
				bpc.GetLastError(ref dwErrorCode);
				((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
			}
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void frmBellInfo_Load(object sender, EventArgs e)
	{
		BellInfoInit();
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Expected O, but got Unknown
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Expected O, but got Unknown
		//IL_0d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9e: Expected O, but got Unknown
		//IL_0e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e65: Expected O, but got Unknown
		//IL_0f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2c: Expected O, but got Unknown
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff0: Expected O, but got Unknown
		//IL_10ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b7: Expected O, but got Unknown
		//IL_1174: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Expected O, but got Unknown
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Expected O, but got Unknown
		//IL_1302: Unknown result type (might be due to invalid IL or missing references)
		//IL_130c: Expected O, but got Unknown
		//IL_13c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d3: Expected O, but got Unknown
		//IL_1490: Unknown result type (might be due to invalid IL or missing references)
		//IL_149a: Expected O, but got Unknown
		//IL_1557: Unknown result type (might be due to invalid IL or missing references)
		//IL_1561: Expected O, but got Unknown
		//IL_161e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1628: Expected O, but got Unknown
		//IL_16e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ef: Expected O, but got Unknown
		//IL_17ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b6: Expected O, but got Unknown
		//IL_1873: Unknown result type (might be due to invalid IL or missing references)
		//IL_187d: Expected O, but got Unknown
		//IL_193a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1944: Expected O, but got Unknown
		//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a08: Expected O, but got Unknown
		//IL_1ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acc: Expected O, but got Unknown
		//IL_1b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b90: Expected O, but got Unknown
		//IL_1c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c47: Expected O, but got Unknown
		//IL_1cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d01: Expected O, but got Unknown
		//IL_1db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbb: Expected O, but got Unknown
		//IL_1e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e75: Expected O, but got Unknown
		//IL_1f25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2f: Expected O, but got Unknown
		//IL_1fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe9: Expected O, but got Unknown
		//IL_2099: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a3: Expected O, but got Unknown
		//IL_2153: Unknown result type (might be due to invalid IL or missing references)
		//IL_215d: Expected O, but got Unknown
		//IL_220a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2214: Expected O, but got Unknown
		//IL_22ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f7: Expected O, but got Unknown
		//IL_23d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_23da: Expected O, but got Unknown
		//IL_24c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ca: Expected O, but got Unknown
		//IL_258e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2598: Expected O, but got Unknown
		//IL_265c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2666: Expected O, but got Unknown
		//IL_272a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2734: Expected O, but got Unknown
		//IL_27f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2802: Expected O, but got Unknown
		//IL_28c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28cd: Expected O, but got Unknown
		//IL_298e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2998: Expected O, but got Unknown
		//IL_2a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a60: Expected O, but got Unknown
		//IL_2b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b28: Expected O, but got Unknown
		//IL_2be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf0: Expected O, but got Unknown
		//IL_2cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cbb: Expected O, but got Unknown
		//IL_2d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d86: Expected O, but got Unknown
		//IL_2e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e51: Expected O, but got Unknown
		//IL_2f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f1c: Expected O, but got Unknown
		//IL_2fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe7: Expected O, but got Unknown
		//IL_30a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b2: Expected O, but got Unknown
		//IL_3173: Unknown result type (might be due to invalid IL or missing references)
		//IL_317d: Expected O, but got Unknown
		//IL_3238: Unknown result type (might be due to invalid IL or missing references)
		//IL_3242: Expected O, but got Unknown
		//IL_32fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3307: Expected O, but got Unknown
		//IL_33c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_33cf: Expected O, but got Unknown
		//IL_348d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3497: Expected O, but got Unknown
		//IL_3555: Unknown result type (might be due to invalid IL or missing references)
		//IL_355f: Expected O, but got Unknown
		//IL_361d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3627: Expected O, but got Unknown
		//IL_36e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ef: Expected O, but got Unknown
		//IL_37ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b7: Expected O, but got Unknown
		//IL_3875: Unknown result type (might be due to invalid IL or missing references)
		//IL_387f: Expected O, but got Unknown
		//IL_393d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3947: Expected O, but got Unknown
		//IL_3a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a0c: Expected O, but got Unknown
		//IL_3aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad4: Expected O, but got Unknown
		//IL_3b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b9c: Expected O, but got Unknown
		//IL_3c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c66: Expected O, but got Unknown
		//IL_3d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d30: Expected O, but got Unknown
		//IL_3de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ded: Expected O, but got Unknown
		//IL_3ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eb7: Expected O, but got Unknown
		//IL_3f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f81: Expected O, but got Unknown
		//IL_4034: Unknown result type (might be due to invalid IL or missing references)
		//IL_403e: Expected O, but got Unknown
		//IL_40fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4108: Expected O, but got Unknown
		//IL_41c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d2: Expected O, but got Unknown
		//IL_4285: Unknown result type (might be due to invalid IL or missing references)
		//IL_428f: Expected O, but got Unknown
		//IL_434f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4359: Expected O, but got Unknown
		//IL_4419: Unknown result type (might be due to invalid IL or missing references)
		//IL_4423: Expected O, but got Unknown
		//IL_44e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_44ed: Expected O, but got Unknown
		//IL_45ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_45b7: Expected O, but got Unknown
		//IL_4677: Unknown result type (might be due to invalid IL or missing references)
		//IL_4681: Expected O, but got Unknown
		//IL_4741: Unknown result type (might be due to invalid IL or missing references)
		//IL_474b: Expected O, but got Unknown
		//IL_480b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4815: Expected O, but got Unknown
		//IL_48d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_48df: Expected O, but got Unknown
		//IL_499f: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a9: Expected O, but got Unknown
		//IL_4a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a73: Expected O, but got Unknown
		//IL_4b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b3d: Expected O, but got Unknown
		//IL_4bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c07: Expected O, but got Unknown
		//IL_4cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd1: Expected O, but got Unknown
		//IL_4d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d9b: Expected O, but got Unknown
		//IL_4e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e65: Expected O, but got Unknown
		//IL_4f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f2c: Expected O, but got Unknown
		//IL_4fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ff3: Expected O, but got Unknown
		//IL_50b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_50ba: Expected O, but got Unknown
		//IL_516a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5174: Expected O, but got Unknown
		//IL_5227: Unknown result type (might be due to invalid IL or missing references)
		//IL_5231: Expected O, but got Unknown
		//IL_52e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ee: Expected O, but got Unknown
		//IL_53a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_53ab: Expected O, but got Unknown
		//IL_545e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5468: Expected O, but got Unknown
		//IL_551b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5525: Expected O, but got Unknown
		//IL_55d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e2: Expected O, but got Unknown
		//IL_5695: Unknown result type (might be due to invalid IL or missing references)
		//IL_569f: Expected O, but got Unknown
		//IL_575c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5766: Expected O, but got Unknown
		//IL_582a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5834: Expected O, but got Unknown
		//IL_58f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5902: Expected O, but got Unknown
		//IL_59c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_59d0: Expected O, but got Unknown
		//IL_5a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a9e: Expected O, but got Unknown
		//IL_5b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b6c: Expected O, but got Unknown
		//IL_5c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c3a: Expected O, but got Unknown
		//IL_5cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d08: Expected O, but got Unknown
		//IL_5dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dd6: Expected O, but got Unknown
		//IL_5e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ea1: Expected O, but got Unknown
		//IL_5f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f6f: Expected O, but got Unknown
		//IL_6033: Unknown result type (might be due to invalid IL or missing references)
		//IL_603d: Expected O, but got Unknown
		//IL_6101: Unknown result type (might be due to invalid IL or missing references)
		//IL_610b: Expected O, but got Unknown
		//IL_61cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_61d9: Expected O, but got Unknown
		//IL_629d: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a7: Expected O, but got Unknown
		//IL_636b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6375: Expected O, but got Unknown
		//IL_6439: Unknown result type (might be due to invalid IL or missing references)
		//IL_6443: Expected O, but got Unknown
		//IL_6504: Unknown result type (might be due to invalid IL or missing references)
		//IL_650e: Expected O, but got Unknown
		//IL_65cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_65d9: Expected O, but got Unknown
		//IL_669a: Unknown result type (might be due to invalid IL or missing references)
		//IL_66a4: Expected O, but got Unknown
		//IL_6768: Unknown result type (might be due to invalid IL or missing references)
		//IL_6772: Expected O, but got Unknown
		//IL_6836: Unknown result type (might be due to invalid IL or missing references)
		//IL_6840: Expected O, but got Unknown
		//IL_6904: Unknown result type (might be due to invalid IL or missing references)
		//IL_690e: Expected O, but got Unknown
		//IL_69d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_69dc: Expected O, but got Unknown
		//IL_6aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aaa: Expected O, but got Unknown
		//IL_6b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b78: Expected O, but got Unknown
		//IL_6c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c46: Expected O, but got Unknown
		_chkValid_11 = new CheckBox();
		_txtMinute_11 = new TextBox();
		_txtHour_11 = new TextBox();
		_chkValid_10 = new CheckBox();
		_txtMinute_10 = new TextBox();
		_txtHour_10 = new TextBox();
		_chkValid_9 = new CheckBox();
		_txtMinute_9 = new TextBox();
		_txtHour_9 = new TextBox();
		_chkValid_8 = new CheckBox();
		_txtMinute_8 = new TextBox();
		_txtHour_8 = new TextBox();
		txtBellCount = new TextBox();
		_txtHour_7 = new TextBox();
		_txtMinute_7 = new TextBox();
		_txtMinute_6 = new TextBox();
		_txtHour_6 = new TextBox();
		_txtHour_5 = new TextBox();
		_txtMinute_5 = new TextBox();
		_txtMinute_4 = new TextBox();
		_txtHour_4 = new TextBox();
		_txtHour_3 = new TextBox();
		_txtMinute_3 = new TextBox();
		_txtMinute_2 = new TextBox();
		_txtHour_2 = new TextBox();
		_txtHour_1 = new TextBox();
		_txtMinute_1 = new TextBox();
		_txtMinute_0 = new TextBox();
		_txtHour_0 = new TextBox();
		_chkValid_7 = new CheckBox();
		_chkValid_6 = new CheckBox();
		_chkValid_5 = new CheckBox();
		_chkValid_4 = new CheckBox();
		_chkValid_3 = new CheckBox();
		_chkValid_2 = new CheckBox();
		_chkValid_1 = new CheckBox();
		_chkValid_0 = new CheckBox();
		cmdExit = new Button();
		cmdWrite = new Button();
		cmdRead = new Button();
		_Label2_23 = new Label();
		_Label2_22 = new Label();
		_Label2_21 = new Label();
		_Label2_20 = new Label();
		_Label2_19 = new Label();
		_Label2_18 = new Label();
		_Label2_17 = new Label();
		_Label2_16 = new Label();
		_Label2_15 = new Label();
		_Label2_14 = new Label();
		_Label2_13 = new Label();
		_Label2_12 = new Label();
		_Label2_11 = new Label();
		_Label2_10 = new Label();
		_Label2_9 = new Label();
		_Label2_8 = new Label();
		Label5 = new Label();
		Label3 = new Label();
		Label4 = new Label();
		_Label2_7 = new Label();
		_Label2_6 = new Label();
		_Label2_5 = new Label();
		_Label2_4 = new Label();
		_Label2_3 = new Label();
		_Label2_2 = new Label();
		_Label2_1 = new Label();
		_Label2_0 = new Label();
		Label1 = new Label();
		lblMessage = new Label();
		_chkValid_23 = new CheckBox();
		_txtMinute_23 = new TextBox();
		_txtHour_23 = new TextBox();
		_chkValid_22 = new CheckBox();
		_txtMinute_22 = new TextBox();
		_txtHour_22 = new TextBox();
		_chkValid_21 = new CheckBox();
		_txtMinute_21 = new TextBox();
		_txtHour_21 = new TextBox();
		_chkValid_20 = new CheckBox();
		_txtMinute_20 = new TextBox();
		_txtHour_20 = new TextBox();
		_txtHour_19 = new TextBox();
		_txtMinute_19 = new TextBox();
		_txtMinute_18 = new TextBox();
		_txtHour_18 = new TextBox();
		_txtHour_17 = new TextBox();
		_txtMinute_17 = new TextBox();
		_txtMinute_16 = new TextBox();
		_txtHour_16 = new TextBox();
		_txtHour_15 = new TextBox();
		_txtMinute_15 = new TextBox();
		_txtMinute_14 = new TextBox();
		_txtHour_14 = new TextBox();
		_txtHour_13 = new TextBox();
		_txtMinute_13 = new TextBox();
		_txtMinute_12 = new TextBox();
		_txtHour_12 = new TextBox();
		_chkValid_19 = new CheckBox();
		_chkValid_18 = new CheckBox();
		_chkValid_17 = new CheckBox();
		_chkValid_16 = new CheckBox();
		_chkValid_15 = new CheckBox();
		_chkValid_14 = new CheckBox();
		_chkValid_13 = new CheckBox();
		_chkValid_12 = new CheckBox();
		label2 = new Label();
		label6 = new Label();
		label7 = new Label();
		label8 = new Label();
		label9 = new Label();
		label10 = new Label();
		label11 = new Label();
		label12 = new Label();
		label13 = new Label();
		label14 = new Label();
		label15 = new Label();
		label16 = new Label();
		label17 = new Label();
		label18 = new Label();
		label19 = new Label();
		label20 = new Label();
		label21 = new Label();
		label22 = new Label();
		label23 = new Label();
		label24 = new Label();
		label25 = new Label();
		label26 = new Label();
		label27 = new Label();
		label28 = new Label();
		label29 = new Label();
		label30 = new Label();
		label31 = new Label();
		((Control)this).SuspendLayout();
		((Control)_chkValid_11).BackColor = SystemColors.Control;
		((Control)_chkValid_11).Cursor = Cursors.Default;
		((Control)_chkValid_11).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_11).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_11).Location = new Point(129, 422);
		((Control)_chkValid_11).Name = "_chkValid_11";
		((Control)_chkValid_11).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_11).Size = new Size(13, 19);
		((Control)_chkValid_11).TabIndex = 132;
		((ButtonBase)_chkValid_11).UseVisualStyleBackColor = false;
		_txtMinute_11.AcceptsReturn = true;
		((Control)_txtMinute_11).BackColor = SystemColors.Window;
		((Control)_txtMinute_11).Cursor = Cursors.IBeam;
		((Control)_txtMinute_11).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_11).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_11).Location = new Point(221, 419);
		((TextBoxBase)_txtMinute_11).MaxLength = 0;
		((Control)_txtMinute_11).Name = "_txtMinute_11";
		((Control)_txtMinute_11).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_11).Size = new Size(42, 26);
		((Control)_txtMinute_11).TabIndex = 131;
		_txtHour_11.AcceptsReturn = true;
		((Control)_txtHour_11).BackColor = SystemColors.Window;
		((Control)_txtHour_11).Cursor = Cursors.IBeam;
		((Control)_txtHour_11).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_11).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_11).Location = new Point(162, 419);
		((TextBoxBase)_txtHour_11).MaxLength = 0;
		((Control)_txtHour_11).Name = "_txtHour_11";
		((Control)_txtHour_11).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_11).Size = new Size(42, 26);
		((Control)_txtHour_11).TabIndex = 130;
		((Control)_chkValid_10).BackColor = SystemColors.Control;
		((Control)_chkValid_10).Cursor = Cursors.Default;
		((Control)_chkValid_10).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_10).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_10).Location = new Point(129, 390);
		((Control)_chkValid_10).Name = "_chkValid_10";
		((Control)_chkValid_10).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_10).Size = new Size(13, 19);
		((Control)_chkValid_10).TabIndex = 128;
		((ButtonBase)_chkValid_10).UseVisualStyleBackColor = false;
		_txtMinute_10.AcceptsReturn = true;
		((Control)_txtMinute_10).BackColor = SystemColors.Window;
		((Control)_txtMinute_10).Cursor = Cursors.IBeam;
		((Control)_txtMinute_10).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_10).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_10).Location = new Point(221, 387);
		((TextBoxBase)_txtMinute_10).MaxLength = 0;
		((Control)_txtMinute_10).Name = "_txtMinute_10";
		((Control)_txtMinute_10).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_10).Size = new Size(42, 26);
		((Control)_txtMinute_10).TabIndex = 127;
		_txtHour_10.AcceptsReturn = true;
		((Control)_txtHour_10).BackColor = SystemColors.Window;
		((Control)_txtHour_10).Cursor = Cursors.IBeam;
		((Control)_txtHour_10).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_10).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_10).Location = new Point(162, 387);
		((TextBoxBase)_txtHour_10).MaxLength = 0;
		((Control)_txtHour_10).Name = "_txtHour_10";
		((Control)_txtHour_10).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_10).Size = new Size(42, 26);
		((Control)_txtHour_10).TabIndex = 126;
		((Control)_chkValid_9).BackColor = SystemColors.Control;
		((Control)_chkValid_9).Cursor = Cursors.Default;
		((Control)_chkValid_9).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_9).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_9).Location = new Point(129, 358);
		((Control)_chkValid_9).Name = "_chkValid_9";
		((Control)_chkValid_9).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_9).Size = new Size(13, 19);
		((Control)_chkValid_9).TabIndex = 124;
		((ButtonBase)_chkValid_9).UseVisualStyleBackColor = false;
		_txtMinute_9.AcceptsReturn = true;
		((Control)_txtMinute_9).BackColor = SystemColors.Window;
		((Control)_txtMinute_9).Cursor = Cursors.IBeam;
		((Control)_txtMinute_9).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_9).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_9).Location = new Point(221, 355);
		((TextBoxBase)_txtMinute_9).MaxLength = 0;
		((Control)_txtMinute_9).Name = "_txtMinute_9";
		((Control)_txtMinute_9).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_9).Size = new Size(42, 26);
		((Control)_txtMinute_9).TabIndex = 123;
		_txtHour_9.AcceptsReturn = true;
		((Control)_txtHour_9).BackColor = SystemColors.Window;
		((Control)_txtHour_9).Cursor = Cursors.IBeam;
		((Control)_txtHour_9).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_9).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_9).Location = new Point(162, 355);
		((TextBoxBase)_txtHour_9).MaxLength = 0;
		((Control)_txtHour_9).Name = "_txtHour_9";
		((Control)_txtHour_9).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_9).Size = new Size(42, 26);
		((Control)_txtHour_9).TabIndex = 122;
		((Control)_chkValid_8).BackColor = SystemColors.Control;
		((Control)_chkValid_8).Cursor = Cursors.Default;
		((Control)_chkValid_8).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_8).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_8).Location = new Point(129, 326);
		((Control)_chkValid_8).Name = "_chkValid_8";
		((Control)_chkValid_8).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_8).Size = new Size(13, 19);
		((Control)_chkValid_8).TabIndex = 120;
		((ButtonBase)_chkValid_8).UseVisualStyleBackColor = false;
		_txtMinute_8.AcceptsReturn = true;
		((Control)_txtMinute_8).BackColor = SystemColors.Window;
		((Control)_txtMinute_8).Cursor = Cursors.IBeam;
		((Control)_txtMinute_8).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_8).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_8).Location = new Point(221, 323);
		((TextBoxBase)_txtMinute_8).MaxLength = 0;
		((Control)_txtMinute_8).Name = "_txtMinute_8";
		((Control)_txtMinute_8).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_8).Size = new Size(42, 26);
		((Control)_txtMinute_8).TabIndex = 119;
		_txtHour_8.AcceptsReturn = true;
		((Control)_txtHour_8).BackColor = SystemColors.Window;
		((Control)_txtHour_8).Cursor = Cursors.IBeam;
		((Control)_txtHour_8).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_8).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_8).Location = new Point(162, 323);
		((TextBoxBase)_txtHour_8).MaxLength = 0;
		((Control)_txtHour_8).Name = "_txtHour_8";
		((Control)_txtHour_8).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_8).Size = new Size(42, 26);
		((Control)_txtHour_8).TabIndex = 118;
		txtBellCount.AcceptsReturn = true;
		((Control)txtBellCount).BackColor = SystemColors.Window;
		((Control)txtBellCount).Cursor = Cursors.IBeam;
		((Control)txtBellCount).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtBellCount).ForeColor = SystemColors.WindowText;
		((Control)txtBellCount).Location = new Point(299, 95);
		((TextBoxBase)txtBellCount).MaxLength = 0;
		((Control)txtBellCount).Name = "txtBellCount";
		((Control)txtBellCount).RightToLeft = (RightToLeft)0;
		((Control)txtBellCount).Size = new Size(39, 26);
		((Control)txtBellCount).TabIndex = 97;
		_txtHour_7.AcceptsReturn = true;
		((Control)_txtHour_7).BackColor = SystemColors.Window;
		((Control)_txtHour_7).Cursor = Cursors.IBeam;
		((Control)_txtHour_7).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_7).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_7).Location = new Point(162, 292);
		((TextBoxBase)_txtHour_7).MaxLength = 0;
		((Control)_txtHour_7).Name = "_txtHour_7";
		((Control)_txtHour_7).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_7).Size = new Size(42, 26);
		((Control)_txtHour_7).TabIndex = 96;
		_txtMinute_7.AcceptsReturn = true;
		((Control)_txtMinute_7).BackColor = SystemColors.Window;
		((Control)_txtMinute_7).Cursor = Cursors.IBeam;
		((Control)_txtMinute_7).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_7).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_7).Location = new Point(221, 292);
		((TextBoxBase)_txtMinute_7).MaxLength = 0;
		((Control)_txtMinute_7).Name = "_txtMinute_7";
		((Control)_txtMinute_7).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_7).Size = new Size(42, 26);
		((Control)_txtMinute_7).TabIndex = 95;
		_txtMinute_6.AcceptsReturn = true;
		((Control)_txtMinute_6).BackColor = SystemColors.Window;
		((Control)_txtMinute_6).Cursor = Cursors.IBeam;
		((Control)_txtMinute_6).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_6).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_6).Location = new Point(221, 264);
		((TextBoxBase)_txtMinute_6).MaxLength = 0;
		((Control)_txtMinute_6).Name = "_txtMinute_6";
		((Control)_txtMinute_6).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_6).Size = new Size(42, 26);
		((Control)_txtMinute_6).TabIndex = 94;
		_txtHour_6.AcceptsReturn = true;
		((Control)_txtHour_6).BackColor = SystemColors.Window;
		((Control)_txtHour_6).Cursor = Cursors.IBeam;
		((Control)_txtHour_6).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_6).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_6).Location = new Point(162, 264);
		((TextBoxBase)_txtHour_6).MaxLength = 0;
		((Control)_txtHour_6).Name = "_txtHour_6";
		((Control)_txtHour_6).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_6).Size = new Size(42, 26);
		((Control)_txtHour_6).TabIndex = 93;
		_txtHour_5.AcceptsReturn = true;
		((Control)_txtHour_5).BackColor = SystemColors.Window;
		((Control)_txtHour_5).Cursor = Cursors.IBeam;
		((Control)_txtHour_5).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_5).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_5).Location = new Point(162, 236);
		((TextBoxBase)_txtHour_5).MaxLength = 0;
		((Control)_txtHour_5).Name = "_txtHour_5";
		((Control)_txtHour_5).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_5).Size = new Size(42, 26);
		((Control)_txtHour_5).TabIndex = 92;
		_txtMinute_5.AcceptsReturn = true;
		((Control)_txtMinute_5).BackColor = SystemColors.Window;
		((Control)_txtMinute_5).Cursor = Cursors.IBeam;
		((Control)_txtMinute_5).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_5).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_5).Location = new Point(221, 236);
		((TextBoxBase)_txtMinute_5).MaxLength = 0;
		((Control)_txtMinute_5).Name = "_txtMinute_5";
		((Control)_txtMinute_5).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_5).Size = new Size(42, 26);
		((Control)_txtMinute_5).TabIndex = 91;
		_txtMinute_4.AcceptsReturn = true;
		((Control)_txtMinute_4).BackColor = SystemColors.Window;
		((Control)_txtMinute_4).Cursor = Cursors.IBeam;
		((Control)_txtMinute_4).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_4).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_4).Location = new Point(221, 208);
		((TextBoxBase)_txtMinute_4).MaxLength = 0;
		((Control)_txtMinute_4).Name = "_txtMinute_4";
		((Control)_txtMinute_4).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_4).Size = new Size(42, 26);
		((Control)_txtMinute_4).TabIndex = 90;
		_txtHour_4.AcceptsReturn = true;
		((Control)_txtHour_4).BackColor = SystemColors.Window;
		((Control)_txtHour_4).Cursor = Cursors.IBeam;
		((Control)_txtHour_4).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_4).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_4).Location = new Point(162, 208);
		((TextBoxBase)_txtHour_4).MaxLength = 0;
		((Control)_txtHour_4).Name = "_txtHour_4";
		((Control)_txtHour_4).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_4).Size = new Size(42, 26);
		((Control)_txtHour_4).TabIndex = 89;
		_txtHour_3.AcceptsReturn = true;
		((Control)_txtHour_3).BackColor = SystemColors.Window;
		((Control)_txtHour_3).Cursor = Cursors.IBeam;
		((Control)_txtHour_3).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_3).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_3).Location = new Point(162, 180);
		((TextBoxBase)_txtHour_3).MaxLength = 0;
		((Control)_txtHour_3).Name = "_txtHour_3";
		((Control)_txtHour_3).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_3).Size = new Size(42, 26);
		((Control)_txtHour_3).TabIndex = 88;
		_txtMinute_3.AcceptsReturn = true;
		((Control)_txtMinute_3).BackColor = SystemColors.Window;
		((Control)_txtMinute_3).Cursor = Cursors.IBeam;
		((Control)_txtMinute_3).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_3).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_3).Location = new Point(221, 180);
		((TextBoxBase)_txtMinute_3).MaxLength = 0;
		((Control)_txtMinute_3).Name = "_txtMinute_3";
		((Control)_txtMinute_3).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_3).Size = new Size(42, 26);
		((Control)_txtMinute_3).TabIndex = 87;
		_txtMinute_2.AcceptsReturn = true;
		((Control)_txtMinute_2).BackColor = SystemColors.Window;
		((Control)_txtMinute_2).Cursor = Cursors.IBeam;
		((Control)_txtMinute_2).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_2).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_2).Location = new Point(221, 153);
		((TextBoxBase)_txtMinute_2).MaxLength = 0;
		((Control)_txtMinute_2).Name = "_txtMinute_2";
		((Control)_txtMinute_2).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_2).Size = new Size(42, 26);
		((Control)_txtMinute_2).TabIndex = 86;
		_txtHour_2.AcceptsReturn = true;
		((Control)_txtHour_2).BackColor = SystemColors.Window;
		((Control)_txtHour_2).Cursor = Cursors.IBeam;
		((Control)_txtHour_2).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_2).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_2).Location = new Point(162, 153);
		((TextBoxBase)_txtHour_2).MaxLength = 0;
		((Control)_txtHour_2).Name = "_txtHour_2";
		((Control)_txtHour_2).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_2).Size = new Size(42, 26);
		((Control)_txtHour_2).TabIndex = 85;
		_txtHour_1.AcceptsReturn = true;
		((Control)_txtHour_1).BackColor = SystemColors.Window;
		((Control)_txtHour_1).Cursor = Cursors.IBeam;
		((Control)_txtHour_1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_1).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_1).Location = new Point(162, 125);
		((TextBoxBase)_txtHour_1).MaxLength = 0;
		((Control)_txtHour_1).Name = "_txtHour_1";
		((Control)_txtHour_1).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_1).Size = new Size(42, 26);
		((Control)_txtHour_1).TabIndex = 84;
		_txtMinute_1.AcceptsReturn = true;
		((Control)_txtMinute_1).BackColor = SystemColors.Window;
		((Control)_txtMinute_1).Cursor = Cursors.IBeam;
		((Control)_txtMinute_1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_1).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_1).Location = new Point(221, 125);
		((TextBoxBase)_txtMinute_1).MaxLength = 0;
		((Control)_txtMinute_1).Name = "_txtMinute_1";
		((Control)_txtMinute_1).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_1).Size = new Size(42, 26);
		((Control)_txtMinute_1).TabIndex = 83;
		_txtMinute_0.AcceptsReturn = true;
		((Control)_txtMinute_0).BackColor = SystemColors.Window;
		((Control)_txtMinute_0).Cursor = Cursors.IBeam;
		((Control)_txtMinute_0).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_0).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_0).Location = new Point(221, 97);
		((TextBoxBase)_txtMinute_0).MaxLength = 0;
		((Control)_txtMinute_0).Name = "_txtMinute_0";
		((Control)_txtMinute_0).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_0).Size = new Size(42, 26);
		((Control)_txtMinute_0).TabIndex = 82;
		_txtHour_0.AcceptsReturn = true;
		((Control)_txtHour_0).BackColor = SystemColors.Window;
		((Control)_txtHour_0).Cursor = Cursors.IBeam;
		((Control)_txtHour_0).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_0).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_0).Location = new Point(162, 97);
		((TextBoxBase)_txtHour_0).MaxLength = 0;
		((Control)_txtHour_0).Name = "_txtHour_0";
		((Control)_txtHour_0).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_0).Size = new Size(42, 26);
		((Control)_txtHour_0).TabIndex = 81;
		((Control)_chkValid_7).BackColor = SystemColors.Control;
		((Control)_chkValid_7).Cursor = Cursors.Default;
		((Control)_chkValid_7).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_7).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_7).Location = new Point(129, 295);
		((Control)_chkValid_7).Name = "_chkValid_7";
		((Control)_chkValid_7).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_7).Size = new Size(13, 19);
		((Control)_chkValid_7).TabIndex = 80;
		((ButtonBase)_chkValid_7).UseVisualStyleBackColor = false;
		((Control)_chkValid_6).BackColor = SystemColors.Control;
		((Control)_chkValid_6).Cursor = Cursors.Default;
		((Control)_chkValid_6).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_6).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_6).Location = new Point(129, 267);
		((Control)_chkValid_6).Name = "_chkValid_6";
		((Control)_chkValid_6).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_6).Size = new Size(13, 19);
		((Control)_chkValid_6).TabIndex = 79;
		((ButtonBase)_chkValid_6).UseVisualStyleBackColor = false;
		((Control)_chkValid_5).BackColor = SystemColors.Control;
		((Control)_chkValid_5).Cursor = Cursors.Default;
		((Control)_chkValid_5).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_5).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_5).Location = new Point(129, 239);
		((Control)_chkValid_5).Name = "_chkValid_5";
		((Control)_chkValid_5).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_5).Size = new Size(13, 19);
		((Control)_chkValid_5).TabIndex = 78;
		((ButtonBase)_chkValid_5).UseVisualStyleBackColor = false;
		((Control)_chkValid_4).BackColor = SystemColors.Control;
		((Control)_chkValid_4).Cursor = Cursors.Default;
		((Control)_chkValid_4).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_4).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_4).Location = new Point(129, 211);
		((Control)_chkValid_4).Name = "_chkValid_4";
		((Control)_chkValid_4).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_4).Size = new Size(13, 19);
		((Control)_chkValid_4).TabIndex = 77;
		((ButtonBase)_chkValid_4).UseVisualStyleBackColor = false;
		((Control)_chkValid_3).BackColor = SystemColors.Control;
		((Control)_chkValid_3).Cursor = Cursors.Default;
		((Control)_chkValid_3).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_3).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_3).Location = new Point(129, 183);
		((Control)_chkValid_3).Name = "_chkValid_3";
		((Control)_chkValid_3).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_3).Size = new Size(13, 19);
		((Control)_chkValid_3).TabIndex = 76;
		((ButtonBase)_chkValid_3).UseVisualStyleBackColor = false;
		((Control)_chkValid_2).BackColor = SystemColors.Control;
		((Control)_chkValid_2).Cursor = Cursors.Default;
		((Control)_chkValid_2).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_2).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_2).Location = new Point(129, 156);
		((Control)_chkValid_2).Name = "_chkValid_2";
		((Control)_chkValid_2).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_2).Size = new Size(13, 19);
		((Control)_chkValid_2).TabIndex = 75;
		((ButtonBase)_chkValid_2).UseVisualStyleBackColor = false;
		((Control)_chkValid_1).BackColor = SystemColors.Control;
		((Control)_chkValid_1).Cursor = Cursors.Default;
		((Control)_chkValid_1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_1).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_1).Location = new Point(129, 128);
		((Control)_chkValid_1).Name = "_chkValid_1";
		((Control)_chkValid_1).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_1).Size = new Size(13, 19);
		((Control)_chkValid_1).TabIndex = 74;
		((ButtonBase)_chkValid_1).UseVisualStyleBackColor = false;
		((Control)_chkValid_0).BackColor = SystemColors.Control;
		((Control)_chkValid_0).Cursor = Cursors.Default;
		((Control)_chkValid_0).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_0).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_0).Location = new Point(129, 100);
		((Control)_chkValid_0).Name = "_chkValid_0";
		((Control)_chkValid_0).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_0).Size = new Size(13, 19);
		((Control)_chkValid_0).TabIndex = 73;
		((ButtonBase)_chkValid_0).UseVisualStyleBackColor = false;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(433, 467);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(99, 34);
		((Control)cmdExit).TabIndex = 71;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdWrite).BackColor = SystemColors.Control;
		((Control)cmdWrite).Cursor = Cursors.Default;
		((Control)cmdWrite).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdWrite).ForeColor = SystemColors.ControlText;
		((Control)cmdWrite).Location = new Point(308, 467);
		((Control)cmdWrite).Name = "cmdWrite";
		((Control)cmdWrite).RightToLeft = (RightToLeft)0;
		((Control)cmdWrite).Size = new Size(99, 34);
		((Control)cmdWrite).TabIndex = 70;
		((Control)cmdWrite).Text = "Write";
		((ButtonBase)cmdWrite).UseVisualStyleBackColor = false;
		((Control)cmdWrite).Click += cmdWrite_Click;
		((Control)cmdRead).BackColor = SystemColors.Control;
		((Control)cmdRead).Cursor = Cursors.Default;
		((Control)cmdRead).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdRead).ForeColor = SystemColors.ControlText;
		((Control)cmdRead).Location = new Point(182, 467);
		((Control)cmdRead).Name = "cmdRead";
		((Control)cmdRead).RightToLeft = (RightToLeft)0;
		((Control)cmdRead).Size = new Size(99, 34);
		((Control)cmdRead).TabIndex = 69;
		((Control)cmdRead).Text = "Read";
		((ButtonBase)cmdRead).UseVisualStyleBackColor = false;
		((Control)cmdRead).Click += cmdRead_Click;
		((Control)_Label2_23).AutoSize = true;
		((Control)_Label2_23).BackColor = Color.Transparent;
		((Control)_Label2_23).Cursor = Cursors.Default;
		((Control)_Label2_23).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_23).ForeColor = SystemColors.ControlText;
		((Control)_Label2_23).Location = new Point(209, 427);
		((Control)_Label2_23).Name = "_Label2_23";
		((Control)_Label2_23).RightToLeft = (RightToLeft)0;
		((Control)_Label2_23).Size = new Size(14, 19);
		((Control)_Label2_23).TabIndex = 137;
		((Control)_Label2_23).Text = ":";
		((Control)_Label2_22).AutoSize = true;
		((Control)_Label2_22).BackColor = Color.Transparent;
		((Control)_Label2_22).Cursor = Cursors.Default;
		((Control)_Label2_22).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_22).ForeColor = SystemColors.ControlText;
		((Control)_Label2_22).Location = new Point(209, 395);
		((Control)_Label2_22).Name = "_Label2_22";
		((Control)_Label2_22).RightToLeft = (RightToLeft)0;
		((Control)_Label2_22).Size = new Size(14, 19);
		((Control)_Label2_22).TabIndex = 136;
		((Control)_Label2_22).Text = ":";
		((Control)_Label2_21).AutoSize = true;
		((Control)_Label2_21).BackColor = Color.Transparent;
		((Control)_Label2_21).Cursor = Cursors.Default;
		((Control)_Label2_21).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_21).ForeColor = SystemColors.ControlText;
		((Control)_Label2_21).Location = new Point(209, 363);
		((Control)_Label2_21).Name = "_Label2_21";
		((Control)_Label2_21).RightToLeft = (RightToLeft)0;
		((Control)_Label2_21).Size = new Size(14, 19);
		((Control)_Label2_21).TabIndex = 135;
		((Control)_Label2_21).Text = ":";
		((Control)_Label2_20).AutoSize = true;
		((Control)_Label2_20).BackColor = Color.Transparent;
		((Control)_Label2_20).Cursor = Cursors.Default;
		((Control)_Label2_20).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_20).ForeColor = SystemColors.ControlText;
		((Control)_Label2_20).Location = new Point(209, 331);
		((Control)_Label2_20).Name = "_Label2_20";
		((Control)_Label2_20).RightToLeft = (RightToLeft)0;
		((Control)_Label2_20).Size = new Size(14, 19);
		((Control)_Label2_20).TabIndex = 134;
		((Control)_Label2_20).Text = ":";
		((Control)_Label2_19).AutoSize = true;
		((Control)_Label2_19).BackColor = Color.Transparent;
		((Control)_Label2_19).Cursor = Cursors.Default;
		((Control)_Label2_19).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_19).ForeColor = SystemColors.ControlText;
		((Control)_Label2_19).Location = new Point(43, 422);
		((Control)_Label2_19).Name = "_Label2_19";
		((Control)_Label2_19).RightToLeft = (RightToLeft)0;
		((Control)_Label2_19).Size = new Size(68, 19);
		((Control)_Label2_19).TabIndex = 133;
		((Control)_Label2_19).Text = "Point 12:";
		((Control)_Label2_18).AutoSize = true;
		((Control)_Label2_18).BackColor = Color.Transparent;
		((Control)_Label2_18).Cursor = Cursors.Default;
		((Control)_Label2_18).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_18).ForeColor = SystemColors.ControlText;
		((Control)_Label2_18).Location = new Point(43, 390);
		((Control)_Label2_18).Name = "_Label2_18";
		((Control)_Label2_18).RightToLeft = (RightToLeft)0;
		((Control)_Label2_18).Size = new Size(68, 19);
		((Control)_Label2_18).TabIndex = 129;
		((Control)_Label2_18).Text = "Point 11:";
		((Control)_Label2_17).AutoSize = true;
		((Control)_Label2_17).BackColor = Color.Transparent;
		((Control)_Label2_17).Cursor = Cursors.Default;
		((Control)_Label2_17).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_17).ForeColor = SystemColors.ControlText;
		((Control)_Label2_17).Location = new Point(43, 358);
		((Control)_Label2_17).Name = "_Label2_17";
		((Control)_Label2_17).RightToLeft = (RightToLeft)0;
		((Control)_Label2_17).Size = new Size(68, 19);
		((Control)_Label2_17).TabIndex = 125;
		((Control)_Label2_17).Text = "Point 10:";
		((Control)_Label2_16).AutoSize = true;
		((Control)_Label2_16).BackColor = Color.Transparent;
		((Control)_Label2_16).Cursor = Cursors.Default;
		((Control)_Label2_16).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_16).ForeColor = SystemColors.ControlText;
		((Control)_Label2_16).Location = new Point(43, 326);
		((Control)_Label2_16).Name = "_Label2_16";
		((Control)_Label2_16).RightToLeft = (RightToLeft)0;
		((Control)_Label2_16).Size = new Size(60, 19);
		((Control)_Label2_16).TabIndex = 121;
		((Control)_Label2_16).Text = "Point 9:";
		((Control)_Label2_15).AutoSize = true;
		((Control)_Label2_15).BackColor = Color.Transparent;
		((Control)_Label2_15).Cursor = Cursors.Default;
		((Control)_Label2_15).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_15).ForeColor = SystemColors.ControlText;
		((Control)_Label2_15).Location = new Point(209, 100);
		((Control)_Label2_15).Name = "_Label2_15";
		((Control)_Label2_15).RightToLeft = (RightToLeft)0;
		((Control)_Label2_15).Size = new Size(14, 19);
		((Control)_Label2_15).TabIndex = 117;
		((Control)_Label2_15).Text = ":";
		((Control)_Label2_14).AutoSize = true;
		((Control)_Label2_14).BackColor = Color.Transparent;
		((Control)_Label2_14).Cursor = Cursors.Default;
		((Control)_Label2_14).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_14).ForeColor = SystemColors.ControlText;
		((Control)_Label2_14).Location = new Point(209, 295);
		((Control)_Label2_14).Name = "_Label2_14";
		((Control)_Label2_14).RightToLeft = (RightToLeft)0;
		((Control)_Label2_14).Size = new Size(14, 19);
		((Control)_Label2_14).TabIndex = 116;
		((Control)_Label2_14).Text = ":";
		((Control)_Label2_13).AutoSize = true;
		((Control)_Label2_13).BackColor = Color.Transparent;
		((Control)_Label2_13).Cursor = Cursors.Default;
		((Control)_Label2_13).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_13).ForeColor = SystemColors.ControlText;
		((Control)_Label2_13).Location = new Point(209, 267);
		((Control)_Label2_13).Name = "_Label2_13";
		((Control)_Label2_13).RightToLeft = (RightToLeft)0;
		((Control)_Label2_13).Size = new Size(14, 19);
		((Control)_Label2_13).TabIndex = 115;
		((Control)_Label2_13).Text = ":";
		((Control)_Label2_12).AutoSize = true;
		((Control)_Label2_12).BackColor = Color.Transparent;
		((Control)_Label2_12).Cursor = Cursors.Default;
		((Control)_Label2_12).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_12).ForeColor = SystemColors.ControlText;
		((Control)_Label2_12).Location = new Point(209, 239);
		((Control)_Label2_12).Name = "_Label2_12";
		((Control)_Label2_12).RightToLeft = (RightToLeft)0;
		((Control)_Label2_12).Size = new Size(14, 19);
		((Control)_Label2_12).TabIndex = 114;
		((Control)_Label2_12).Text = ":";
		((Control)_Label2_11).AutoSize = true;
		((Control)_Label2_11).BackColor = Color.Transparent;
		((Control)_Label2_11).Cursor = Cursors.Default;
		((Control)_Label2_11).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_11).ForeColor = SystemColors.ControlText;
		((Control)_Label2_11).Location = new Point(209, 211);
		((Control)_Label2_11).Name = "_Label2_11";
		((Control)_Label2_11).RightToLeft = (RightToLeft)0;
		((Control)_Label2_11).Size = new Size(14, 19);
		((Control)_Label2_11).TabIndex = 113;
		((Control)_Label2_11).Text = ":";
		((Control)_Label2_10).AutoSize = true;
		((Control)_Label2_10).BackColor = Color.Transparent;
		((Control)_Label2_10).Cursor = Cursors.Default;
		((Control)_Label2_10).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_10).ForeColor = SystemColors.ControlText;
		((Control)_Label2_10).Location = new Point(209, 183);
		((Control)_Label2_10).Name = "_Label2_10";
		((Control)_Label2_10).RightToLeft = (RightToLeft)0;
		((Control)_Label2_10).Size = new Size(14, 19);
		((Control)_Label2_10).TabIndex = 112;
		((Control)_Label2_10).Text = ":";
		((Control)_Label2_9).AutoSize = true;
		((Control)_Label2_9).BackColor = Color.Transparent;
		((Control)_Label2_9).Cursor = Cursors.Default;
		((Control)_Label2_9).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_9).ForeColor = SystemColors.ControlText;
		((Control)_Label2_9).Location = new Point(209, 156);
		((Control)_Label2_9).Name = "_Label2_9";
		((Control)_Label2_9).RightToLeft = (RightToLeft)0;
		((Control)_Label2_9).Size = new Size(14, 19);
		((Control)_Label2_9).TabIndex = 111;
		((Control)_Label2_9).Text = ":";
		((Control)_Label2_8).AutoSize = true;
		((Control)_Label2_8).BackColor = Color.Transparent;
		((Control)_Label2_8).Cursor = Cursors.Default;
		((Control)_Label2_8).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_8).ForeColor = SystemColors.ControlText;
		((Control)_Label2_8).Location = new Point(209, 128);
		((Control)_Label2_8).Name = "_Label2_8";
		((Control)_Label2_8).RightToLeft = (RightToLeft)0;
		((Control)_Label2_8).Size = new Size(14, 19);
		((Control)_Label2_8).TabIndex = 110;
		((Control)_Label2_8).Text = ":";
		((Control)Label5).AutoSize = true;
		((Control)Label5).BackColor = Color.Transparent;
		((Control)Label5).Cursor = Cursors.Default;
		((Control)Label5).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label5).ForeColor = SystemColors.ControlText;
		((Control)Label5).Location = new Point(32, 71);
		((Control)Label5).Name = "Label5";
		((Control)Label5).RightToLeft = (RightToLeft)0;
		((Control)Label5).Size = new Size(74, 19);
		((Control)Label5).TabIndex = 109;
		((Control)Label5).Text = "Bell Point";
		((Control)Label3).AutoSize = true;
		((Control)Label3).BackColor = Color.Transparent;
		((Control)Label3).Cursor = Cursors.Default;
		((Control)Label3).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label3).ForeColor = SystemColors.ControlText;
		((Control)Label3).Location = new Point(109, 71);
		((Control)Label3).Name = "Label3";
		((Control)Label3).RightToLeft = (RightToLeft)0;
		((Control)Label3).Size = new Size(64, 19);
		((Control)Label3).TabIndex = 108;
		((Control)Label3).Text = "UseFlag";
		((Control)Label4).AutoSize = true;
		((Control)Label4).BackColor = Color.Transparent;
		((Control)Label4).Cursor = Cursors.Default;
		((Control)Label4).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label4).ForeColor = SystemColors.ControlText;
		((Control)Label4).Location = new Point(178, 71);
		((Control)Label4).Name = "Label4";
		((Control)Label4).RightToLeft = (RightToLeft)0;
		((Control)Label4).Size = new Size(80, 19);
		((Control)Label4).TabIndex = 107;
		((Control)Label4).Text = "Start Time";
		((Control)_Label2_7).AutoSize = true;
		((Control)_Label2_7).BackColor = Color.Transparent;
		((Control)_Label2_7).Cursor = Cursors.Default;
		((Control)_Label2_7).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_7).ForeColor = SystemColors.ControlText;
		((Control)_Label2_7).Location = new Point(43, 295);
		((Control)_Label2_7).Name = "_Label2_7";
		((Control)_Label2_7).RightToLeft = (RightToLeft)0;
		((Control)_Label2_7).Size = new Size(60, 19);
		((Control)_Label2_7).TabIndex = 106;
		((Control)_Label2_7).Text = "Point 8:";
		((Control)_Label2_6).AutoSize = true;
		((Control)_Label2_6).BackColor = Color.Transparent;
		((Control)_Label2_6).Cursor = Cursors.Default;
		((Control)_Label2_6).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_6).ForeColor = SystemColors.ControlText;
		((Control)_Label2_6).Location = new Point(43, 267);
		((Control)_Label2_6).Name = "_Label2_6";
		((Control)_Label2_6).RightToLeft = (RightToLeft)0;
		((Control)_Label2_6).Size = new Size(60, 19);
		((Control)_Label2_6).TabIndex = 105;
		((Control)_Label2_6).Text = "Point 7:";
		((Control)_Label2_5).AutoSize = true;
		((Control)_Label2_5).BackColor = Color.Transparent;
		((Control)_Label2_5).Cursor = Cursors.Default;
		((Control)_Label2_5).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_5).ForeColor = SystemColors.ControlText;
		((Control)_Label2_5).Location = new Point(43, 239);
		((Control)_Label2_5).Name = "_Label2_5";
		((Control)_Label2_5).RightToLeft = (RightToLeft)0;
		((Control)_Label2_5).Size = new Size(60, 19);
		((Control)_Label2_5).TabIndex = 104;
		((Control)_Label2_5).Text = "Point 6:";
		((Control)_Label2_4).AutoSize = true;
		((Control)_Label2_4).BackColor = Color.Transparent;
		((Control)_Label2_4).Cursor = Cursors.Default;
		((Control)_Label2_4).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_4).ForeColor = SystemColors.ControlText;
		((Control)_Label2_4).Location = new Point(43, 211);
		((Control)_Label2_4).Name = "_Label2_4";
		((Control)_Label2_4).RightToLeft = (RightToLeft)0;
		((Control)_Label2_4).Size = new Size(60, 19);
		((Control)_Label2_4).TabIndex = 103;
		((Control)_Label2_4).Text = "Point 5:";
		((Control)_Label2_3).AutoSize = true;
		((Control)_Label2_3).BackColor = Color.Transparent;
		((Control)_Label2_3).Cursor = Cursors.Default;
		((Control)_Label2_3).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_3).ForeColor = SystemColors.ControlText;
		((Control)_Label2_3).Location = new Point(43, 183);
		((Control)_Label2_3).Name = "_Label2_3";
		((Control)_Label2_3).RightToLeft = (RightToLeft)0;
		((Control)_Label2_3).Size = new Size(60, 19);
		((Control)_Label2_3).TabIndex = 102;
		((Control)_Label2_3).Text = "Point 4:";
		((Control)_Label2_2).AutoSize = true;
		((Control)_Label2_2).BackColor = Color.Transparent;
		((Control)_Label2_2).Cursor = Cursors.Default;
		((Control)_Label2_2).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_2).ForeColor = SystemColors.ControlText;
		((Control)_Label2_2).Location = new Point(43, 156);
		((Control)_Label2_2).Name = "_Label2_2";
		((Control)_Label2_2).RightToLeft = (RightToLeft)0;
		((Control)_Label2_2).Size = new Size(60, 19);
		((Control)_Label2_2).TabIndex = 101;
		((Control)_Label2_2).Text = "Point 3:";
		((Control)_Label2_1).AutoSize = true;
		((Control)_Label2_1).BackColor = Color.Transparent;
		((Control)_Label2_1).Cursor = Cursors.Default;
		((Control)_Label2_1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_1).ForeColor = SystemColors.ControlText;
		((Control)_Label2_1).Location = new Point(43, 128);
		((Control)_Label2_1).Name = "_Label2_1";
		((Control)_Label2_1).RightToLeft = (RightToLeft)0;
		((Control)_Label2_1).Size = new Size(60, 19);
		((Control)_Label2_1).TabIndex = 100;
		((Control)_Label2_1).Text = "Point 2:";
		((Control)_Label2_0).AutoSize = true;
		((Control)_Label2_0).BackColor = Color.Transparent;
		((Control)_Label2_0).Cursor = Cursors.Default;
		((Control)_Label2_0).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_Label2_0).ForeColor = SystemColors.ControlText;
		((Control)_Label2_0).Location = new Point(43, 100);
		((Control)_Label2_0).Name = "_Label2_0";
		((Control)_Label2_0).RightToLeft = (RightToLeft)0;
		((Control)_Label2_0).Size = new Size(60, 19);
		((Control)_Label2_0).TabIndex = 99;
		((Control)_Label2_0).Text = "Point 1:";
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = Color.Transparent;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(280, 71);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(89, 19);
		((Control)Label1).TabIndex = 98;
		((Control)Label1).Text = "Bell Count :";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(14, 20);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(654, 28);
		((Control)lblMessage).TabIndex = 72;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)_chkValid_23).BackColor = SystemColors.Control;
		((Control)_chkValid_23).Cursor = Cursors.Default;
		((Control)_chkValid_23).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_23).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_23).Location = new Point(485, 422);
		((Control)_chkValid_23).Name = "_chkValid_23";
		((Control)_chkValid_23).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_23).Size = new Size(13, 19);
		((Control)_chkValid_23).TabIndex = 195;
		((ButtonBase)_chkValid_23).UseVisualStyleBackColor = false;
		_txtMinute_23.AcceptsReturn = true;
		((Control)_txtMinute_23).BackColor = SystemColors.Window;
		((Control)_txtMinute_23).Cursor = Cursors.IBeam;
		((Control)_txtMinute_23).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_23).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_23).Location = new Point(577, 419);
		((TextBoxBase)_txtMinute_23).MaxLength = 0;
		((Control)_txtMinute_23).Name = "_txtMinute_23";
		((Control)_txtMinute_23).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_23).Size = new Size(42, 26);
		((Control)_txtMinute_23).TabIndex = 194;
		_txtHour_23.AcceptsReturn = true;
		((Control)_txtHour_23).BackColor = SystemColors.Window;
		((Control)_txtHour_23).Cursor = Cursors.IBeam;
		((Control)_txtHour_23).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_23).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_23).Location = new Point(518, 419);
		((TextBoxBase)_txtHour_23).MaxLength = 0;
		((Control)_txtHour_23).Name = "_txtHour_23";
		((Control)_txtHour_23).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_23).Size = new Size(42, 26);
		((Control)_txtHour_23).TabIndex = 193;
		((Control)_chkValid_22).BackColor = SystemColors.Control;
		((Control)_chkValid_22).Cursor = Cursors.Default;
		((Control)_chkValid_22).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_22).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_22).Location = new Point(485, 390);
		((Control)_chkValid_22).Name = "_chkValid_22";
		((Control)_chkValid_22).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_22).Size = new Size(13, 19);
		((Control)_chkValid_22).TabIndex = 191;
		((ButtonBase)_chkValid_22).UseVisualStyleBackColor = false;
		_txtMinute_22.AcceptsReturn = true;
		((Control)_txtMinute_22).BackColor = SystemColors.Window;
		((Control)_txtMinute_22).Cursor = Cursors.IBeam;
		((Control)_txtMinute_22).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_22).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_22).Location = new Point(577, 387);
		((TextBoxBase)_txtMinute_22).MaxLength = 0;
		((Control)_txtMinute_22).Name = "_txtMinute_22";
		((Control)_txtMinute_22).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_22).Size = new Size(42, 26);
		((Control)_txtMinute_22).TabIndex = 190;
		_txtHour_22.AcceptsReturn = true;
		((Control)_txtHour_22).BackColor = SystemColors.Window;
		((Control)_txtHour_22).Cursor = Cursors.IBeam;
		((Control)_txtHour_22).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_22).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_22).Location = new Point(518, 387);
		((TextBoxBase)_txtHour_22).MaxLength = 0;
		((Control)_txtHour_22).Name = "_txtHour_22";
		((Control)_txtHour_22).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_22).Size = new Size(42, 26);
		((Control)_txtHour_22).TabIndex = 189;
		((Control)_chkValid_21).BackColor = SystemColors.Control;
		((Control)_chkValid_21).Cursor = Cursors.Default;
		((Control)_chkValid_21).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_21).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_21).Location = new Point(485, 358);
		((Control)_chkValid_21).Name = "_chkValid_21";
		((Control)_chkValid_21).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_21).Size = new Size(13, 19);
		((Control)_chkValid_21).TabIndex = 187;
		((ButtonBase)_chkValid_21).UseVisualStyleBackColor = false;
		_txtMinute_21.AcceptsReturn = true;
		((Control)_txtMinute_21).BackColor = SystemColors.Window;
		((Control)_txtMinute_21).Cursor = Cursors.IBeam;
		((Control)_txtMinute_21).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_21).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_21).Location = new Point(577, 355);
		((TextBoxBase)_txtMinute_21).MaxLength = 0;
		((Control)_txtMinute_21).Name = "_txtMinute_21";
		((Control)_txtMinute_21).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_21).Size = new Size(42, 26);
		((Control)_txtMinute_21).TabIndex = 186;
		_txtHour_21.AcceptsReturn = true;
		((Control)_txtHour_21).BackColor = SystemColors.Window;
		((Control)_txtHour_21).Cursor = Cursors.IBeam;
		((Control)_txtHour_21).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_21).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_21).Location = new Point(518, 355);
		((TextBoxBase)_txtHour_21).MaxLength = 0;
		((Control)_txtHour_21).Name = "_txtHour_21";
		((Control)_txtHour_21).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_21).Size = new Size(42, 26);
		((Control)_txtHour_21).TabIndex = 185;
		((Control)_chkValid_20).BackColor = SystemColors.Control;
		((Control)_chkValid_20).Cursor = Cursors.Default;
		((Control)_chkValid_20).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_20).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_20).Location = new Point(485, 326);
		((Control)_chkValid_20).Name = "_chkValid_20";
		((Control)_chkValid_20).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_20).Size = new Size(13, 19);
		((Control)_chkValid_20).TabIndex = 183;
		((ButtonBase)_chkValid_20).UseVisualStyleBackColor = false;
		_txtMinute_20.AcceptsReturn = true;
		((Control)_txtMinute_20).BackColor = SystemColors.Window;
		((Control)_txtMinute_20).Cursor = Cursors.IBeam;
		((Control)_txtMinute_20).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_20).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_20).Location = new Point(577, 323);
		((TextBoxBase)_txtMinute_20).MaxLength = 0;
		((Control)_txtMinute_20).Name = "_txtMinute_20";
		((Control)_txtMinute_20).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_20).Size = new Size(42, 26);
		((Control)_txtMinute_20).TabIndex = 182;
		_txtHour_20.AcceptsReturn = true;
		((Control)_txtHour_20).BackColor = SystemColors.Window;
		((Control)_txtHour_20).Cursor = Cursors.IBeam;
		((Control)_txtHour_20).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_20).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_20).Location = new Point(518, 323);
		((TextBoxBase)_txtHour_20).MaxLength = 0;
		((Control)_txtHour_20).Name = "_txtHour_20";
		((Control)_txtHour_20).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_20).Size = new Size(42, 26);
		((Control)_txtHour_20).TabIndex = 181;
		_txtHour_19.AcceptsReturn = true;
		((Control)_txtHour_19).BackColor = SystemColors.Window;
		((Control)_txtHour_19).Cursor = Cursors.IBeam;
		((Control)_txtHour_19).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_19).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_19).Location = new Point(518, 292);
		((TextBoxBase)_txtHour_19).MaxLength = 0;
		((Control)_txtHour_19).Name = "_txtHour_19";
		((Control)_txtHour_19).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_19).Size = new Size(42, 26);
		((Control)_txtHour_19).TabIndex = 161;
		_txtMinute_19.AcceptsReturn = true;
		((Control)_txtMinute_19).BackColor = SystemColors.Window;
		((Control)_txtMinute_19).Cursor = Cursors.IBeam;
		((Control)_txtMinute_19).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_19).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_19).Location = new Point(577, 292);
		((TextBoxBase)_txtMinute_19).MaxLength = 0;
		((Control)_txtMinute_19).Name = "_txtMinute_19";
		((Control)_txtMinute_19).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_19).Size = new Size(42, 26);
		((Control)_txtMinute_19).TabIndex = 160;
		_txtMinute_18.AcceptsReturn = true;
		((Control)_txtMinute_18).BackColor = SystemColors.Window;
		((Control)_txtMinute_18).Cursor = Cursors.IBeam;
		((Control)_txtMinute_18).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_18).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_18).Location = new Point(577, 264);
		((TextBoxBase)_txtMinute_18).MaxLength = 0;
		((Control)_txtMinute_18).Name = "_txtMinute_18";
		((Control)_txtMinute_18).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_18).Size = new Size(42, 26);
		((Control)_txtMinute_18).TabIndex = 159;
		_txtHour_18.AcceptsReturn = true;
		((Control)_txtHour_18).BackColor = SystemColors.Window;
		((Control)_txtHour_18).Cursor = Cursors.IBeam;
		((Control)_txtHour_18).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_18).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_18).Location = new Point(518, 264);
		((TextBoxBase)_txtHour_18).MaxLength = 0;
		((Control)_txtHour_18).Name = "_txtHour_18";
		((Control)_txtHour_18).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_18).Size = new Size(42, 26);
		((Control)_txtHour_18).TabIndex = 158;
		_txtHour_17.AcceptsReturn = true;
		((Control)_txtHour_17).BackColor = SystemColors.Window;
		((Control)_txtHour_17).Cursor = Cursors.IBeam;
		((Control)_txtHour_17).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_17).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_17).Location = new Point(518, 236);
		((TextBoxBase)_txtHour_17).MaxLength = 0;
		((Control)_txtHour_17).Name = "_txtHour_17";
		((Control)_txtHour_17).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_17).Size = new Size(42, 26);
		((Control)_txtHour_17).TabIndex = 157;
		_txtMinute_17.AcceptsReturn = true;
		((Control)_txtMinute_17).BackColor = SystemColors.Window;
		((Control)_txtMinute_17).Cursor = Cursors.IBeam;
		((Control)_txtMinute_17).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_17).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_17).Location = new Point(577, 236);
		((TextBoxBase)_txtMinute_17).MaxLength = 0;
		((Control)_txtMinute_17).Name = "_txtMinute_17";
		((Control)_txtMinute_17).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_17).Size = new Size(42, 26);
		((Control)_txtMinute_17).TabIndex = 156;
		_txtMinute_16.AcceptsReturn = true;
		((Control)_txtMinute_16).BackColor = SystemColors.Window;
		((Control)_txtMinute_16).Cursor = Cursors.IBeam;
		((Control)_txtMinute_16).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_16).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_16).Location = new Point(577, 208);
		((TextBoxBase)_txtMinute_16).MaxLength = 0;
		((Control)_txtMinute_16).Name = "_txtMinute_16";
		((Control)_txtMinute_16).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_16).Size = new Size(42, 26);
		((Control)_txtMinute_16).TabIndex = 155;
		_txtHour_16.AcceptsReturn = true;
		((Control)_txtHour_16).BackColor = SystemColors.Window;
		((Control)_txtHour_16).Cursor = Cursors.IBeam;
		((Control)_txtHour_16).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_16).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_16).Location = new Point(518, 208);
		((TextBoxBase)_txtHour_16).MaxLength = 0;
		((Control)_txtHour_16).Name = "_txtHour_16";
		((Control)_txtHour_16).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_16).Size = new Size(42, 26);
		((Control)_txtHour_16).TabIndex = 154;
		_txtHour_15.AcceptsReturn = true;
		((Control)_txtHour_15).BackColor = SystemColors.Window;
		((Control)_txtHour_15).Cursor = Cursors.IBeam;
		((Control)_txtHour_15).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_15).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_15).Location = new Point(518, 180);
		((TextBoxBase)_txtHour_15).MaxLength = 0;
		((Control)_txtHour_15).Name = "_txtHour_15";
		((Control)_txtHour_15).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_15).Size = new Size(42, 26);
		((Control)_txtHour_15).TabIndex = 153;
		_txtMinute_15.AcceptsReturn = true;
		((Control)_txtMinute_15).BackColor = SystemColors.Window;
		((Control)_txtMinute_15).Cursor = Cursors.IBeam;
		((Control)_txtMinute_15).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_15).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_15).Location = new Point(577, 180);
		((TextBoxBase)_txtMinute_15).MaxLength = 0;
		((Control)_txtMinute_15).Name = "_txtMinute_15";
		((Control)_txtMinute_15).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_15).Size = new Size(42, 26);
		((Control)_txtMinute_15).TabIndex = 152;
		_txtMinute_14.AcceptsReturn = true;
		((Control)_txtMinute_14).BackColor = SystemColors.Window;
		((Control)_txtMinute_14).Cursor = Cursors.IBeam;
		((Control)_txtMinute_14).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_14).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_14).Location = new Point(577, 153);
		((TextBoxBase)_txtMinute_14).MaxLength = 0;
		((Control)_txtMinute_14).Name = "_txtMinute_14";
		((Control)_txtMinute_14).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_14).Size = new Size(42, 26);
		((Control)_txtMinute_14).TabIndex = 151;
		_txtHour_14.AcceptsReturn = true;
		((Control)_txtHour_14).BackColor = SystemColors.Window;
		((Control)_txtHour_14).Cursor = Cursors.IBeam;
		((Control)_txtHour_14).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_14).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_14).Location = new Point(518, 153);
		((TextBoxBase)_txtHour_14).MaxLength = 0;
		((Control)_txtHour_14).Name = "_txtHour_14";
		((Control)_txtHour_14).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_14).Size = new Size(42, 26);
		((Control)_txtHour_14).TabIndex = 150;
		_txtHour_13.AcceptsReturn = true;
		((Control)_txtHour_13).BackColor = SystemColors.Window;
		((Control)_txtHour_13).Cursor = Cursors.IBeam;
		((Control)_txtHour_13).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_13).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_13).Location = new Point(518, 125);
		((TextBoxBase)_txtHour_13).MaxLength = 0;
		((Control)_txtHour_13).Name = "_txtHour_13";
		((Control)_txtHour_13).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_13).Size = new Size(42, 26);
		((Control)_txtHour_13).TabIndex = 149;
		_txtMinute_13.AcceptsReturn = true;
		((Control)_txtMinute_13).BackColor = SystemColors.Window;
		((Control)_txtMinute_13).Cursor = Cursors.IBeam;
		((Control)_txtMinute_13).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_13).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_13).Location = new Point(577, 125);
		((TextBoxBase)_txtMinute_13).MaxLength = 0;
		((Control)_txtMinute_13).Name = "_txtMinute_13";
		((Control)_txtMinute_13).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_13).Size = new Size(42, 26);
		((Control)_txtMinute_13).TabIndex = 148;
		_txtMinute_12.AcceptsReturn = true;
		((Control)_txtMinute_12).BackColor = SystemColors.Window;
		((Control)_txtMinute_12).Cursor = Cursors.IBeam;
		((Control)_txtMinute_12).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtMinute_12).ForeColor = SystemColors.WindowText;
		((Control)_txtMinute_12).Location = new Point(577, 97);
		((TextBoxBase)_txtMinute_12).MaxLength = 0;
		((Control)_txtMinute_12).Name = "_txtMinute_12";
		((Control)_txtMinute_12).RightToLeft = (RightToLeft)0;
		((Control)_txtMinute_12).Size = new Size(42, 26);
		((Control)_txtMinute_12).TabIndex = 147;
		_txtHour_12.AcceptsReturn = true;
		((Control)_txtHour_12).BackColor = SystemColors.Window;
		((Control)_txtHour_12).Cursor = Cursors.IBeam;
		((Control)_txtHour_12).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_txtHour_12).ForeColor = SystemColors.WindowText;
		((Control)_txtHour_12).Location = new Point(518, 97);
		((TextBoxBase)_txtHour_12).MaxLength = 0;
		((Control)_txtHour_12).Name = "_txtHour_12";
		((Control)_txtHour_12).RightToLeft = (RightToLeft)0;
		((Control)_txtHour_12).Size = new Size(42, 26);
		((Control)_txtHour_12).TabIndex = 146;
		((Control)_chkValid_19).BackColor = SystemColors.Control;
		((Control)_chkValid_19).Cursor = Cursors.Default;
		((Control)_chkValid_19).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_19).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_19).Location = new Point(485, 295);
		((Control)_chkValid_19).Name = "_chkValid_19";
		((Control)_chkValid_19).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_19).Size = new Size(13, 19);
		((Control)_chkValid_19).TabIndex = 145;
		((ButtonBase)_chkValid_19).UseVisualStyleBackColor = false;
		((Control)_chkValid_18).BackColor = SystemColors.Control;
		((Control)_chkValid_18).Cursor = Cursors.Default;
		((Control)_chkValid_18).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_18).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_18).Location = new Point(485, 267);
		((Control)_chkValid_18).Name = "_chkValid_18";
		((Control)_chkValid_18).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_18).Size = new Size(13, 19);
		((Control)_chkValid_18).TabIndex = 144;
		((ButtonBase)_chkValid_18).UseVisualStyleBackColor = false;
		((Control)_chkValid_17).BackColor = SystemColors.Control;
		((Control)_chkValid_17).Cursor = Cursors.Default;
		((Control)_chkValid_17).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_17).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_17).Location = new Point(485, 239);
		((Control)_chkValid_17).Name = "_chkValid_17";
		((Control)_chkValid_17).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_17).Size = new Size(13, 19);
		((Control)_chkValid_17).TabIndex = 143;
		((ButtonBase)_chkValid_17).UseVisualStyleBackColor = false;
		((Control)_chkValid_16).BackColor = SystemColors.Control;
		((Control)_chkValid_16).Cursor = Cursors.Default;
		((Control)_chkValid_16).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_16).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_16).Location = new Point(485, 211);
		((Control)_chkValid_16).Name = "_chkValid_16";
		((Control)_chkValid_16).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_16).Size = new Size(13, 19);
		((Control)_chkValid_16).TabIndex = 142;
		((ButtonBase)_chkValid_16).UseVisualStyleBackColor = false;
		((Control)_chkValid_15).BackColor = SystemColors.Control;
		((Control)_chkValid_15).Cursor = Cursors.Default;
		((Control)_chkValid_15).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_15).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_15).Location = new Point(485, 183);
		((Control)_chkValid_15).Name = "_chkValid_15";
		((Control)_chkValid_15).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_15).Size = new Size(13, 19);
		((Control)_chkValid_15).TabIndex = 141;
		((ButtonBase)_chkValid_15).UseVisualStyleBackColor = false;
		((Control)_chkValid_14).BackColor = SystemColors.Control;
		((Control)_chkValid_14).Cursor = Cursors.Default;
		((Control)_chkValid_14).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_14).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_14).Location = new Point(485, 156);
		((Control)_chkValid_14).Name = "_chkValid_14";
		((Control)_chkValid_14).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_14).Size = new Size(13, 19);
		((Control)_chkValid_14).TabIndex = 140;
		((ButtonBase)_chkValid_14).UseVisualStyleBackColor = false;
		((Control)_chkValid_13).BackColor = SystemColors.Control;
		((Control)_chkValid_13).Cursor = Cursors.Default;
		((Control)_chkValid_13).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_13).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_13).Location = new Point(485, 128);
		((Control)_chkValid_13).Name = "_chkValid_13";
		((Control)_chkValid_13).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_13).Size = new Size(13, 19);
		((Control)_chkValid_13).TabIndex = 139;
		((ButtonBase)_chkValid_13).UseVisualStyleBackColor = false;
		((Control)_chkValid_12).BackColor = SystemColors.Control;
		((Control)_chkValid_12).Cursor = Cursors.Default;
		((Control)_chkValid_12).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_chkValid_12).ForeColor = SystemColors.ControlText;
		((Control)_chkValid_12).Location = new Point(485, 100);
		((Control)_chkValid_12).Name = "_chkValid_12";
		((Control)_chkValid_12).RightToLeft = (RightToLeft)0;
		((Control)_chkValid_12).Size = new Size(13, 19);
		((Control)_chkValid_12).TabIndex = 138;
		((ButtonBase)_chkValid_12).UseVisualStyleBackColor = false;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(565, 427);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(14, 19);
		((Control)label2).TabIndex = 200;
		((Control)label2).Text = ":";
		((Control)label6).AutoSize = true;
		((Control)label6).BackColor = Color.Transparent;
		((Control)label6).Cursor = Cursors.Default;
		((Control)label6).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label6).ForeColor = SystemColors.ControlText;
		((Control)label6).Location = new Point(565, 395);
		((Control)label6).Name = "label6";
		((Control)label6).RightToLeft = (RightToLeft)0;
		((Control)label6).Size = new Size(14, 19);
		((Control)label6).TabIndex = 199;
		((Control)label6).Text = ":";
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = Color.Transparent;
		((Control)label7).Cursor = Cursors.Default;
		((Control)label7).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label7).ForeColor = SystemColors.ControlText;
		((Control)label7).Location = new Point(565, 363);
		((Control)label7).Name = "label7";
		((Control)label7).RightToLeft = (RightToLeft)0;
		((Control)label7).Size = new Size(14, 19);
		((Control)label7).TabIndex = 198;
		((Control)label7).Text = ":";
		((Control)label8).AutoSize = true;
		((Control)label8).BackColor = Color.Transparent;
		((Control)label8).Cursor = Cursors.Default;
		((Control)label8).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label8).ForeColor = SystemColors.ControlText;
		((Control)label8).Location = new Point(565, 331);
		((Control)label8).Name = "label8";
		((Control)label8).RightToLeft = (RightToLeft)0;
		((Control)label8).Size = new Size(14, 19);
		((Control)label8).TabIndex = 197;
		((Control)label8).Text = ":";
		((Control)label9).AutoSize = true;
		((Control)label9).BackColor = Color.Transparent;
		((Control)label9).Cursor = Cursors.Default;
		((Control)label9).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label9).ForeColor = SystemColors.ControlText;
		((Control)label9).Location = new Point(399, 422);
		((Control)label9).Name = "label9";
		((Control)label9).RightToLeft = (RightToLeft)0;
		((Control)label9).Size = new Size(68, 19);
		((Control)label9).TabIndex = 196;
		((Control)label9).Text = "Point 24:";
		((Control)label10).AutoSize = true;
		((Control)label10).BackColor = Color.Transparent;
		((Control)label10).Cursor = Cursors.Default;
		((Control)label10).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label10).ForeColor = SystemColors.ControlText;
		((Control)label10).Location = new Point(399, 390);
		((Control)label10).Name = "label10";
		((Control)label10).RightToLeft = (RightToLeft)0;
		((Control)label10).Size = new Size(68, 19);
		((Control)label10).TabIndex = 192;
		((Control)label10).Text = "Point 23:";
		((Control)label11).AutoSize = true;
		((Control)label11).BackColor = Color.Transparent;
		((Control)label11).Cursor = Cursors.Default;
		((Control)label11).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label11).ForeColor = SystemColors.ControlText;
		((Control)label11).Location = new Point(399, 358);
		((Control)label11).Name = "label11";
		((Control)label11).RightToLeft = (RightToLeft)0;
		((Control)label11).Size = new Size(68, 19);
		((Control)label11).TabIndex = 188;
		((Control)label11).Text = "Point 22:";
		((Control)label12).AutoSize = true;
		((Control)label12).BackColor = Color.Transparent;
		((Control)label12).Cursor = Cursors.Default;
		((Control)label12).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label12).ForeColor = SystemColors.ControlText;
		((Control)label12).Location = new Point(399, 326);
		((Control)label12).Name = "label12";
		((Control)label12).RightToLeft = (RightToLeft)0;
		((Control)label12).Size = new Size(68, 19);
		((Control)label12).TabIndex = 184;
		((Control)label12).Text = "Point 21:";
		((Control)label13).AutoSize = true;
		((Control)label13).BackColor = Color.Transparent;
		((Control)label13).Cursor = Cursors.Default;
		((Control)label13).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label13).ForeColor = SystemColors.ControlText;
		((Control)label13).Location = new Point(565, 100);
		((Control)label13).Name = "label13";
		((Control)label13).RightToLeft = (RightToLeft)0;
		((Control)label13).Size = new Size(14, 19);
		((Control)label13).TabIndex = 180;
		((Control)label13).Text = ":";
		((Control)label14).AutoSize = true;
		((Control)label14).BackColor = Color.Transparent;
		((Control)label14).Cursor = Cursors.Default;
		((Control)label14).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label14).ForeColor = SystemColors.ControlText;
		((Control)label14).Location = new Point(565, 295);
		((Control)label14).Name = "label14";
		((Control)label14).RightToLeft = (RightToLeft)0;
		((Control)label14).Size = new Size(14, 19);
		((Control)label14).TabIndex = 179;
		((Control)label14).Text = ":";
		((Control)label15).AutoSize = true;
		((Control)label15).BackColor = Color.Transparent;
		((Control)label15).Cursor = Cursors.Default;
		((Control)label15).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label15).ForeColor = SystemColors.ControlText;
		((Control)label15).Location = new Point(565, 267);
		((Control)label15).Name = "label15";
		((Control)label15).RightToLeft = (RightToLeft)0;
		((Control)label15).Size = new Size(14, 19);
		((Control)label15).TabIndex = 178;
		((Control)label15).Text = ":";
		((Control)label16).AutoSize = true;
		((Control)label16).BackColor = Color.Transparent;
		((Control)label16).Cursor = Cursors.Default;
		((Control)label16).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label16).ForeColor = SystemColors.ControlText;
		((Control)label16).Location = new Point(565, 239);
		((Control)label16).Name = "label16";
		((Control)label16).RightToLeft = (RightToLeft)0;
		((Control)label16).Size = new Size(14, 19);
		((Control)label16).TabIndex = 177;
		((Control)label16).Text = ":";
		((Control)label17).AutoSize = true;
		((Control)label17).BackColor = Color.Transparent;
		((Control)label17).Cursor = Cursors.Default;
		((Control)label17).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label17).ForeColor = SystemColors.ControlText;
		((Control)label17).Location = new Point(565, 211);
		((Control)label17).Name = "label17";
		((Control)label17).RightToLeft = (RightToLeft)0;
		((Control)label17).Size = new Size(14, 19);
		((Control)label17).TabIndex = 176;
		((Control)label17).Text = ":";
		((Control)label18).AutoSize = true;
		((Control)label18).BackColor = Color.Transparent;
		((Control)label18).Cursor = Cursors.Default;
		((Control)label18).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label18).ForeColor = SystemColors.ControlText;
		((Control)label18).Location = new Point(565, 183);
		((Control)label18).Name = "label18";
		((Control)label18).RightToLeft = (RightToLeft)0;
		((Control)label18).Size = new Size(14, 19);
		((Control)label18).TabIndex = 175;
		((Control)label18).Text = ":";
		((Control)label19).AutoSize = true;
		((Control)label19).BackColor = Color.Transparent;
		((Control)label19).Cursor = Cursors.Default;
		((Control)label19).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label19).ForeColor = SystemColors.ControlText;
		((Control)label19).Location = new Point(565, 156);
		((Control)label19).Name = "label19";
		((Control)label19).RightToLeft = (RightToLeft)0;
		((Control)label19).Size = new Size(14, 19);
		((Control)label19).TabIndex = 174;
		((Control)label19).Text = ":";
		((Control)label20).AutoSize = true;
		((Control)label20).BackColor = Color.Transparent;
		((Control)label20).Cursor = Cursors.Default;
		((Control)label20).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label20).ForeColor = SystemColors.ControlText;
		((Control)label20).Location = new Point(565, 128);
		((Control)label20).Name = "label20";
		((Control)label20).RightToLeft = (RightToLeft)0;
		((Control)label20).Size = new Size(14, 19);
		((Control)label20).TabIndex = 173;
		((Control)label20).Text = ":";
		((Control)label21).AutoSize = true;
		((Control)label21).BackColor = Color.Transparent;
		((Control)label21).Cursor = Cursors.Default;
		((Control)label21).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label21).ForeColor = SystemColors.ControlText;
		((Control)label21).Location = new Point(388, 71);
		((Control)label21).Name = "label21";
		((Control)label21).RightToLeft = (RightToLeft)0;
		((Control)label21).Size = new Size(74, 19);
		((Control)label21).TabIndex = 172;
		((Control)label21).Text = "Bell Point";
		((Control)label22).AutoSize = true;
		((Control)label22).BackColor = Color.Transparent;
		((Control)label22).Cursor = Cursors.Default;
		((Control)label22).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label22).ForeColor = SystemColors.ControlText;
		((Control)label22).Location = new Point(465, 71);
		((Control)label22).Name = "label22";
		((Control)label22).RightToLeft = (RightToLeft)0;
		((Control)label22).Size = new Size(64, 19);
		((Control)label22).TabIndex = 171;
		((Control)label22).Text = "UseFlag";
		((Control)label23).AutoSize = true;
		((Control)label23).BackColor = Color.Transparent;
		((Control)label23).Cursor = Cursors.Default;
		((Control)label23).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label23).ForeColor = SystemColors.ControlText;
		((Control)label23).Location = new Point(534, 71);
		((Control)label23).Name = "label23";
		((Control)label23).RightToLeft = (RightToLeft)0;
		((Control)label23).Size = new Size(80, 19);
		((Control)label23).TabIndex = 170;
		((Control)label23).Text = "Start Time";
		((Control)label24).AutoSize = true;
		((Control)label24).BackColor = Color.Transparent;
		((Control)label24).Cursor = Cursors.Default;
		((Control)label24).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label24).ForeColor = SystemColors.ControlText;
		((Control)label24).Location = new Point(399, 295);
		((Control)label24).Name = "label24";
		((Control)label24).RightToLeft = (RightToLeft)0;
		((Control)label24).Size = new Size(68, 19);
		((Control)label24).TabIndex = 169;
		((Control)label24).Text = "Point 20:";
		((Control)label25).AutoSize = true;
		((Control)label25).BackColor = Color.Transparent;
		((Control)label25).Cursor = Cursors.Default;
		((Control)label25).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label25).ForeColor = SystemColors.ControlText;
		((Control)label25).Location = new Point(399, 267);
		((Control)label25).Name = "label25";
		((Control)label25).RightToLeft = (RightToLeft)0;
		((Control)label25).Size = new Size(68, 19);
		((Control)label25).TabIndex = 168;
		((Control)label25).Text = "Point 19:";
		((Control)label26).AutoSize = true;
		((Control)label26).BackColor = Color.Transparent;
		((Control)label26).Cursor = Cursors.Default;
		((Control)label26).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label26).ForeColor = SystemColors.ControlText;
		((Control)label26).Location = new Point(399, 239);
		((Control)label26).Name = "label26";
		((Control)label26).RightToLeft = (RightToLeft)0;
		((Control)label26).Size = new Size(68, 19);
		((Control)label26).TabIndex = 167;
		((Control)label26).Text = "Point 18:";
		((Control)label27).AutoSize = true;
		((Control)label27).BackColor = Color.Transparent;
		((Control)label27).Cursor = Cursors.Default;
		((Control)label27).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label27).ForeColor = SystemColors.ControlText;
		((Control)label27).Location = new Point(399, 211);
		((Control)label27).Name = "label27";
		((Control)label27).RightToLeft = (RightToLeft)0;
		((Control)label27).Size = new Size(68, 19);
		((Control)label27).TabIndex = 166;
		((Control)label27).Text = "Point 17:";
		((Control)label28).AutoSize = true;
		((Control)label28).BackColor = Color.Transparent;
		((Control)label28).Cursor = Cursors.Default;
		((Control)label28).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label28).ForeColor = SystemColors.ControlText;
		((Control)label28).Location = new Point(399, 183);
		((Control)label28).Name = "label28";
		((Control)label28).RightToLeft = (RightToLeft)0;
		((Control)label28).Size = new Size(68, 19);
		((Control)label28).TabIndex = 165;
		((Control)label28).Text = "Point 16:";
		((Control)label29).AutoSize = true;
		((Control)label29).BackColor = Color.Transparent;
		((Control)label29).Cursor = Cursors.Default;
		((Control)label29).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label29).ForeColor = SystemColors.ControlText;
		((Control)label29).Location = new Point(399, 156);
		((Control)label29).Name = "label29";
		((Control)label29).RightToLeft = (RightToLeft)0;
		((Control)label29).Size = new Size(68, 19);
		((Control)label29).TabIndex = 164;
		((Control)label29).Text = "Point 15:";
		((Control)label30).AutoSize = true;
		((Control)label30).BackColor = Color.Transparent;
		((Control)label30).Cursor = Cursors.Default;
		((Control)label30).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label30).ForeColor = SystemColors.ControlText;
		((Control)label30).Location = new Point(399, 128);
		((Control)label30).Name = "label30";
		((Control)label30).RightToLeft = (RightToLeft)0;
		((Control)label30).Size = new Size(68, 19);
		((Control)label30).TabIndex = 163;
		((Control)label30).Text = "Point 14:";
		((Control)label31).AutoSize = true;
		((Control)label31).BackColor = Color.Transparent;
		((Control)label31).Cursor = Cursors.Default;
		((Control)label31).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label31).ForeColor = SystemColors.ControlText;
		((Control)label31).Location = new Point(399, 100);
		((Control)label31).Name = "label31";
		((Control)label31).RightToLeft = (RightToLeft)0;
		((Control)label31).Size = new Size(68, 19);
		((Control)label31).TabIndex = 162;
		((Control)label31).Text = "Point 13:";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(680, 513);
		((Control)this).Controls.Add((Control)(object)_chkValid_23);
		((Control)this).Controls.Add((Control)(object)_txtMinute_23);
		((Control)this).Controls.Add((Control)(object)_txtHour_23);
		((Control)this).Controls.Add((Control)(object)_chkValid_22);
		((Control)this).Controls.Add((Control)(object)_txtMinute_22);
		((Control)this).Controls.Add((Control)(object)_txtHour_22);
		((Control)this).Controls.Add((Control)(object)_chkValid_21);
		((Control)this).Controls.Add((Control)(object)_txtMinute_21);
		((Control)this).Controls.Add((Control)(object)_txtHour_21);
		((Control)this).Controls.Add((Control)(object)_chkValid_20);
		((Control)this).Controls.Add((Control)(object)_txtMinute_20);
		((Control)this).Controls.Add((Control)(object)_txtHour_20);
		((Control)this).Controls.Add((Control)(object)_txtHour_19);
		((Control)this).Controls.Add((Control)(object)_txtMinute_19);
		((Control)this).Controls.Add((Control)(object)_txtMinute_18);
		((Control)this).Controls.Add((Control)(object)_txtHour_18);
		((Control)this).Controls.Add((Control)(object)_txtHour_17);
		((Control)this).Controls.Add((Control)(object)_txtMinute_17);
		((Control)this).Controls.Add((Control)(object)_txtMinute_16);
		((Control)this).Controls.Add((Control)(object)_txtHour_16);
		((Control)this).Controls.Add((Control)(object)_txtHour_15);
		((Control)this).Controls.Add((Control)(object)_txtMinute_15);
		((Control)this).Controls.Add((Control)(object)_txtMinute_14);
		((Control)this).Controls.Add((Control)(object)_txtHour_14);
		((Control)this).Controls.Add((Control)(object)_txtHour_13);
		((Control)this).Controls.Add((Control)(object)_txtMinute_13);
		((Control)this).Controls.Add((Control)(object)_txtMinute_12);
		((Control)this).Controls.Add((Control)(object)_txtHour_12);
		((Control)this).Controls.Add((Control)(object)_chkValid_19);
		((Control)this).Controls.Add((Control)(object)_chkValid_18);
		((Control)this).Controls.Add((Control)(object)_chkValid_17);
		((Control)this).Controls.Add((Control)(object)_chkValid_16);
		((Control)this).Controls.Add((Control)(object)_chkValid_15);
		((Control)this).Controls.Add((Control)(object)_chkValid_14);
		((Control)this).Controls.Add((Control)(object)_chkValid_13);
		((Control)this).Controls.Add((Control)(object)_chkValid_12);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label6);
		((Control)this).Controls.Add((Control)(object)label7);
		((Control)this).Controls.Add((Control)(object)label8);
		((Control)this).Controls.Add((Control)(object)label9);
		((Control)this).Controls.Add((Control)(object)label10);
		((Control)this).Controls.Add((Control)(object)label11);
		((Control)this).Controls.Add((Control)(object)label12);
		((Control)this).Controls.Add((Control)(object)label13);
		((Control)this).Controls.Add((Control)(object)label14);
		((Control)this).Controls.Add((Control)(object)label15);
		((Control)this).Controls.Add((Control)(object)label16);
		((Control)this).Controls.Add((Control)(object)label17);
		((Control)this).Controls.Add((Control)(object)label18);
		((Control)this).Controls.Add((Control)(object)label19);
		((Control)this).Controls.Add((Control)(object)label20);
		((Control)this).Controls.Add((Control)(object)label21);
		((Control)this).Controls.Add((Control)(object)label22);
		((Control)this).Controls.Add((Control)(object)label23);
		((Control)this).Controls.Add((Control)(object)label24);
		((Control)this).Controls.Add((Control)(object)label25);
		((Control)this).Controls.Add((Control)(object)label26);
		((Control)this).Controls.Add((Control)(object)label27);
		((Control)this).Controls.Add((Control)(object)label28);
		((Control)this).Controls.Add((Control)(object)label29);
		((Control)this).Controls.Add((Control)(object)label30);
		((Control)this).Controls.Add((Control)(object)label31);
		((Control)this).Controls.Add((Control)(object)_chkValid_11);
		((Control)this).Controls.Add((Control)(object)_txtMinute_11);
		((Control)this).Controls.Add((Control)(object)_txtHour_11);
		((Control)this).Controls.Add((Control)(object)_chkValid_10);
		((Control)this).Controls.Add((Control)(object)_txtMinute_10);
		((Control)this).Controls.Add((Control)(object)_txtHour_10);
		((Control)this).Controls.Add((Control)(object)_chkValid_9);
		((Control)this).Controls.Add((Control)(object)_txtMinute_9);
		((Control)this).Controls.Add((Control)(object)_txtHour_9);
		((Control)this).Controls.Add((Control)(object)_chkValid_8);
		((Control)this).Controls.Add((Control)(object)_txtMinute_8);
		((Control)this).Controls.Add((Control)(object)_txtHour_8);
		((Control)this).Controls.Add((Control)(object)txtBellCount);
		((Control)this).Controls.Add((Control)(object)_txtHour_7);
		((Control)this).Controls.Add((Control)(object)_txtMinute_7);
		((Control)this).Controls.Add((Control)(object)_txtMinute_6);
		((Control)this).Controls.Add((Control)(object)_txtHour_6);
		((Control)this).Controls.Add((Control)(object)_txtHour_5);
		((Control)this).Controls.Add((Control)(object)_txtMinute_5);
		((Control)this).Controls.Add((Control)(object)_txtMinute_4);
		((Control)this).Controls.Add((Control)(object)_txtHour_4);
		((Control)this).Controls.Add((Control)(object)_txtHour_3);
		((Control)this).Controls.Add((Control)(object)_txtMinute_3);
		((Control)this).Controls.Add((Control)(object)_txtMinute_2);
		((Control)this).Controls.Add((Control)(object)_txtHour_2);
		((Control)this).Controls.Add((Control)(object)_txtHour_1);
		((Control)this).Controls.Add((Control)(object)_txtMinute_1);
		((Control)this).Controls.Add((Control)(object)_txtMinute_0);
		((Control)this).Controls.Add((Control)(object)_txtHour_0);
		((Control)this).Controls.Add((Control)(object)_chkValid_7);
		((Control)this).Controls.Add((Control)(object)_chkValid_6);
		((Control)this).Controls.Add((Control)(object)_chkValid_5);
		((Control)this).Controls.Add((Control)(object)_chkValid_4);
		((Control)this).Controls.Add((Control)(object)_chkValid_3);
		((Control)this).Controls.Add((Control)(object)_chkValid_2);
		((Control)this).Controls.Add((Control)(object)_chkValid_1);
		((Control)this).Controls.Add((Control)(object)_chkValid_0);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdWrite);
		((Control)this).Controls.Add((Control)(object)cmdRead);
		((Control)this).Controls.Add((Control)(object)_Label2_23);
		((Control)this).Controls.Add((Control)(object)_Label2_22);
		((Control)this).Controls.Add((Control)(object)_Label2_21);
		((Control)this).Controls.Add((Control)(object)_Label2_20);
		((Control)this).Controls.Add((Control)(object)_Label2_19);
		((Control)this).Controls.Add((Control)(object)_Label2_18);
		((Control)this).Controls.Add((Control)(object)_Label2_17);
		((Control)this).Controls.Add((Control)(object)_Label2_16);
		((Control)this).Controls.Add((Control)(object)_Label2_15);
		((Control)this).Controls.Add((Control)(object)_Label2_14);
		((Control)this).Controls.Add((Control)(object)_Label2_13);
		((Control)this).Controls.Add((Control)(object)_Label2_12);
		((Control)this).Controls.Add((Control)(object)_Label2_11);
		((Control)this).Controls.Add((Control)(object)_Label2_10);
		((Control)this).Controls.Add((Control)(object)_Label2_9);
		((Control)this).Controls.Add((Control)(object)_Label2_8);
		((Control)this).Controls.Add((Control)(object)Label5);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Controls.Add((Control)(object)_Label2_7);
		((Control)this).Controls.Add((Control)(object)_Label2_6);
		((Control)this).Controls.Add((Control)(object)_Label2_5);
		((Control)this).Controls.Add((Control)(object)_Label2_4);
		((Control)this).Controls.Add((Control)(object)_Label2_3);
		((Control)this).Controls.Add((Control)(object)_Label2_2);
		((Control)this).Controls.Add((Control)(object)_Label2_1);
		((Control)this).Controls.Add((Control)(object)_Label2_0);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmBellInfo";
		((Control)this).Text = "frmBellInfo";
		((Form)this).Load += frmBellInfo_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
