using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using NPResourceManagement;

namespace TimeAttendanceUI.Forms.Other;

public class FrmTutorialVideo : CacheBaseForm
{
	public static string AssemblyVersion = "";

	public static string SerialLK = "";

	public static string VersionName = "";

	private IContainer components;

	private PanelEx panelEx1;

	private PanelEx panelEx2;

	private LinkLabel linkLblNP620;

	private LinkLabel linkLblNP586;

	private LinkLabel linkLblNP395;

	private LinkLabel linkLblNP2815;

	private LinkLabel linkLblNP621;

	private LinkLabel np261;

	private LinkLabel Hozor;

	private LinkLabel np586;

	private LinkLabel linkLabel3;

	private LinkLabel linkLabel2;

	private LinkLabel linkLabel1;

	private Label label1;

	private LinkLabel linkLabel4;

	private LinkLabel linkLabel5;

	private LinkLabel linkLabel6;

	private LinkLabel linkLabel7;

	private LinkLabel CreateShift;

	private LinkLabel linkLabel9;

	private LinkLabel linkLabel8;

	private LinkLabel linkLabel10;

	private LinkLabel linkLabel11;

	private LinkLabel linkLabel12;

	private LinkLabel linkLabel13;

	private LinkLabel linkLabel14;

	private LinkLabel linkLabel15;

	private LinkLabel linkLabel16;

	private LinkLabel linkLabel17;

	private LinkLabel linkLabel18;

	private Label label2;

	public FrmTutorialVideo(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	private void FrmTutorialVideo_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
	}

	private void linkLblNP620_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/Download/VideoHelp/Help620.mp4");
	}

	private void linkLblNP621_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/Download/VideoHelp/Help261.mp4");
	}

	private void linkLblNP2815_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/Download/VideoHelp/Help2815.mp4");
	}

	private void linkLblNP586_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/Download/VideoHelp/Help586.mp4");
	}

	private void linkLblNP395_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/Download/VideoHelp/Help395.mp4");
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/Help/Hozor/HELP-NP620.pdf");
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/Help/Hozor/HELP-NP2815.pdf");
	}

	private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/Help/Hozor/HELP-NP302.pdf");
	}

	private void np586_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/Help/Hozor/HELP-NP586.pdf");
	}

	private void Hozor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://novinp.ir/Download/HELP/Hozor/helpHozor.pdf");
	}

	private void np261_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/Help/Hozor/HELP-NP261.pdf");
	}

	private void CreateShift_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://novinp.ir/Download/HELP/Hozor/shifts.pdf");
	}

	private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/AttendanceHelp.zip");
	}

	private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/1-Year_Co.mp4");
	}

	private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/2-Device_Co.mp4");
	}

	private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/3-Office_Co.mp4");
	}

	private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/4-Shift_Co.mp4");
	}

	private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/5-Hesab_Co.mp4");
	}

	private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/6-Personnel_Co.mp4");
	}

	private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/7-Contract_Co.mp4");
	}

	private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/8-AttReports_Co1.mp4");
	}

	private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/9-StatusRep_Co.mp4");
	}

	private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/10-AggregationRep_Co100.mp4");
	}

	private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/11-salary_Co.mp4");
	}

	private void linkLabel18_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/12-backup_Co.mp4");
	}

	private void linkLabel17_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/13-SMS_Co.mp4");
	}

	private void linkLabel16_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("http://www.novinp.ir/download/VideoHelp/Attendance/14-users_Co.mp4");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((CacheBaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
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
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Expected O, but got Unknown
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Expected O, but got Unknown
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Expected O, but got Unknown
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_085c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Expected O, but got Unknown
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Expected O, but got Unknown
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Expected O, but got Unknown
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Expected O, but got Unknown
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a96: Expected O, but got Unknown
		//IL_0b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Expected O, but got Unknown
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Expected O, but got Unknown
		//IL_0bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc6: Expected O, but got Unknown
		//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf4: Expected O, but got Unknown
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c75: Expected O, but got Unknown
		//IL_0c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca3: Expected O, but got Unknown
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d24: Expected O, but got Unknown
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Expected O, but got Unknown
		//IL_0dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de1: Expected O, but got Unknown
		//IL_0e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0f: Expected O, but got Unknown
		//IL_0e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e90: Expected O, but got Unknown
		//IL_0eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebe: Expected O, but got Unknown
		//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3c: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Expected O, but got Unknown
		//IL_0fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe8: Expected O, but got Unknown
		//IL_100c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1016: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Expected O, but got Unknown
		//IL_10b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c2: Expected O, but got Unknown
		//IL_1136: Unknown result type (might be due to invalid IL or missing references)
		//IL_1140: Expected O, but got Unknown
		//IL_1164: Unknown result type (might be due to invalid IL or missing references)
		//IL_116e: Expected O, but got Unknown
		//IL_11f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fd: Expected O, but got Unknown
		//IL_1221: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Expected O, but got Unknown
		//IL_12b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bd: Expected O, but got Unknown
		//IL_12e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12eb: Expected O, but got Unknown
		//IL_135f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1369: Expected O, but got Unknown
		//IL_138d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1397: Expected O, but got Unknown
		//IL_140b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1415: Expected O, but got Unknown
		//IL_1439: Unknown result type (might be due to invalid IL or missing references)
		//IL_1443: Expected O, but got Unknown
		//IL_14b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c1: Expected O, but got Unknown
		//IL_14e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ef: Expected O, but got Unknown
		//IL_1563: Unknown result type (might be due to invalid IL or missing references)
		//IL_156d: Expected O, but got Unknown
		//IL_1591: Unknown result type (might be due to invalid IL or missing references)
		//IL_159b: Expected O, but got Unknown
		//IL_160f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1619: Expected O, but got Unknown
		//IL_163d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1647: Expected O, but got Unknown
		//IL_16bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c5: Expected O, but got Unknown
		//IL_16e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f3: Expected O, but got Unknown
		//IL_1767: Unknown result type (might be due to invalid IL or missing references)
		//IL_1771: Expected O, but got Unknown
		//IL_1795: Unknown result type (might be due to invalid IL or missing references)
		//IL_179f: Expected O, but got Unknown
		//IL_1813: Unknown result type (might be due to invalid IL or missing references)
		//IL_181d: Expected O, but got Unknown
		//IL_1841: Unknown result type (might be due to invalid IL or missing references)
		//IL_184b: Expected O, but got Unknown
		//IL_18bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c9: Expected O, but got Unknown
		//IL_18ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f7: Expected O, but got Unknown
		//IL_196b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1975: Expected O, but got Unknown
		//IL_19a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19af: Expected O, but got Unknown
		//IL_1a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a74: Expected O, but got Unknown
		//IL_1ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af0: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		panelEx2 = new PanelEx();
		linkLblNP586 = new LinkLabel();
		linkLblNP395 = new LinkLabel();
		linkLblNP2815 = new LinkLabel();
		linkLblNP621 = new LinkLabel();
		linkLblNP620 = new LinkLabel();
		label1 = new Label();
		linkLabel1 = new LinkLabel();
		linkLabel2 = new LinkLabel();
		linkLabel3 = new LinkLabel();
		np586 = new LinkLabel();
		Hozor = new LinkLabel();
		np261 = new LinkLabel();
		linkLabel4 = new LinkLabel();
		linkLabel5 = new LinkLabel();
		linkLabel6 = new LinkLabel();
		linkLabel7 = new LinkLabel();
		CreateShift = new LinkLabel();
		linkLabel9 = new LinkLabel();
		linkLabel8 = new LinkLabel();
		linkLabel10 = new LinkLabel();
		linkLabel11 = new LinkLabel();
		linkLabel12 = new LinkLabel();
		linkLabel13 = new LinkLabel();
		linkLabel14 = new LinkLabel();
		linkLabel15 = new LinkLabel();
		linkLabel16 = new LinkLabel();
		linkLabel17 = new LinkLabel();
		linkLabel18 = new LinkLabel();
		label2 = new Label();
		((Control)panelEx1).SuspendLayout();
		((Control)panelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)panelEx2);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(968, 641);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)panelEx2).Anchor = (AnchorStyles)15;
		panelEx2.CanvasColor = SystemColors.Control;
		panelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx2).Controls.Add((Control)(object)label2);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel16);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel17);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel18);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel10);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel11);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel12);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel13);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel14);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel15);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel4);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel5);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel6);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel7);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel8);
		((Control)panelEx2).Controls.Add((Control)(object)CreateShift);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel9);
		((Control)panelEx2).Controls.Add((Control)(object)np261);
		((Control)panelEx2).Controls.Add((Control)(object)Hozor);
		((Control)panelEx2).Controls.Add((Control)(object)np586);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel3);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel2);
		((Control)panelEx2).Controls.Add((Control)(object)linkLabel1);
		((Control)panelEx2).Controls.Add((Control)(object)label1);
		((Control)panelEx2).Controls.Add((Control)(object)linkLblNP586);
		((Control)panelEx2).Controls.Add((Control)(object)linkLblNP395);
		((Control)panelEx2).Controls.Add((Control)(object)linkLblNP2815);
		((Control)panelEx2).Controls.Add((Control)(object)linkLblNP621);
		((Control)panelEx2).Controls.Add((Control)(object)linkLblNP620);
		((Control)panelEx2).Location = new Point(7, 12);
		((Control)panelEx2).MaximumSize = new Size(950, 617);
		((Control)panelEx2).MinimumSize = new Size(950, 617);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(950, 617);
		panelEx2.Style.Alignment = (StringAlignment)1;
		panelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx2.Style.Border = (eBorderType)1;
		panelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx2.Style.BorderWidth = 2;
		panelEx2.Style.CornerType = (eCornerType)2;
		panelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx2.Style.GradientAngle = 90;
		((Control)panelEx2).TabIndex = 0;
		((Control)linkLblNP586).Anchor = (AnchorStyles)9;
		((Control)linkLblNP586).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLblNP586).Location = new Point(554, 106);
		((Control)linkLblNP586).Name = "linkLblNP586";
		((Control)linkLblNP586).Size = new Size(290, 19);
		((Control)linkLblNP586).TabIndex = 9;
		linkLblNP586.TabStop = true;
		((Control)linkLblNP586).Text = "دانلود فیلم آموزشی دستگاه NP586";
		linkLblNP586.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLblNP586_LinkClicked);
		((Control)linkLblNP395).Anchor = (AnchorStyles)9;
		((Control)linkLblNP395).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLblNP395).Location = new Point(554, 66);
		((Control)linkLblNP395).Name = "linkLblNP395";
		((Control)linkLblNP395).Size = new Size(290, 19);
		((Control)linkLblNP395).TabIndex = 9;
		linkLblNP395.TabStop = true;
		((Control)linkLblNP395).Text = "دانلود فیلم آموزشی دستگاه NP395";
		linkLblNP395.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLblNP395_LinkClicked);
		((Control)linkLblNP2815).Anchor = (AnchorStyles)9;
		((Control)linkLblNP2815).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLblNP2815).Location = new Point(554, 146);
		((Control)linkLblNP2815).Name = "linkLblNP2815";
		((Control)linkLblNP2815).Size = new Size(290, 19);
		((Control)linkLblNP2815).TabIndex = 9;
		linkLblNP2815.TabStop = true;
		((Control)linkLblNP2815).Text = "دانلود فیلم آموزشی دستگاه NP2815";
		linkLblNP2815.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLblNP2815_LinkClicked);
		((Control)linkLblNP621).Anchor = (AnchorStyles)9;
		((Control)linkLblNP621).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLblNP621).Location = new Point(554, 186);
		((Control)linkLblNP621).Name = "linkLblNP621";
		((Control)linkLblNP621).Size = new Size(290, 19);
		((Control)linkLblNP621).TabIndex = 9;
		linkLblNP621.TabStop = true;
		((Control)linkLblNP621).Text = "دانلود فیلم آموزشی دستگاه NP261";
		linkLblNP621.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLblNP621_LinkClicked);
		((Control)linkLblNP620).Anchor = (AnchorStyles)9;
		((Control)linkLblNP620).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLblNP620).Location = new Point(554, 226);
		((Control)linkLblNP620).Name = "linkLblNP620";
		((Control)linkLblNP620).Size = new Size(290, 19);
		((Control)linkLblNP620).TabIndex = 9;
		linkLblNP620.TabStop = true;
		((Control)linkLblNP620).Text = "دانلود فیلم آموزشی دستگاه NP620";
		linkLblNP620.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLblNP620_LinkClicked);
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Tahoma", 10f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label1).Location = new Point(553, 12);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(368, 17);
		((Control)label1).TabIndex = 10;
		((Control)label1).Text = "راهنمای آموزشی دستگاه های حضور و غیاب " + TextResourceManagement.TextResource.CompanyName;
		((Control)linkLabel1).Anchor = (AnchorStyles)9;
		((Control)linkLabel1).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel1).Location = new Point(554, 414);
		((Control)linkLabel1).Name = "linkLabel1";
		((Control)linkLabel1).Size = new Size(290, 19);
		((Control)linkLabel1).TabIndex = 11;
		linkLabel1.TabStop = true;
		((Control)linkLabel1).Text = "راهنمای دستگاه حضوروغیاب NP620";
		linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		((Control)linkLabel2).Anchor = (AnchorStyles)9;
		((Control)linkLabel2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel2).Location = new Point(554, 344);
		((Control)linkLabel2).Name = "linkLabel2";
		((Control)linkLabel2).Size = new Size(290, 19);
		((Control)linkLabel2).TabIndex = 12;
		linkLabel2.TabStop = true;
		((Control)linkLabel2).Text = "راهنمای دستگاه حضورو غیاب NP2815";
		linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		((Control)linkLabel3).Anchor = (AnchorStyles)9;
		((Control)linkLabel3).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel3).Location = new Point(554, 449);
		((Control)linkLabel3).Name = "linkLabel3";
		((Control)linkLabel3).Size = new Size(290, 19);
		((Control)linkLabel3).TabIndex = 13;
		linkLabel3.TabStop = true;
		((Control)linkLabel3).Text = "راهنمای دستگاه حضورو غیاب NP302";
		linkLabel3.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
		((Control)np586).Anchor = (AnchorStyles)9;
		((Control)np586).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)np586).Location = new Point(554, 309);
		((Control)np586).Name = "np586";
		((Control)np586).Size = new Size(290, 19);
		((Control)np586).TabIndex = 14;
		np586.TabStop = true;
		((Control)np586).Text = "راهنمای دستگاه حضورو غیاب NP586";
		np586.LinkClicked += new LinkLabelLinkClickedEventHandler(np586_LinkClicked);
		((Control)Hozor).Anchor = (AnchorStyles)9;
		((Control)Hozor).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Hozor).Location = new Point(42, 66);
		((Control)Hozor).Name = "Hozor";
		((Control)Hozor).Size = new Size(390, 14);
		((Control)Hozor).TabIndex = 15;
		Hozor.TabStop = true;
		((Control)Hozor).Text = "راهنمای نرم افزار حضور و غیاب- حقوق و دستمزد " + TextResourceManagement.TextResource.CompanyName + "\t";
		Hozor.LinkClicked += new LinkLabelLinkClickedEventHandler(Hozor_LinkClicked);
		((Control)np261).Anchor = (AnchorStyles)9;
		((Control)np261).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)np261).Location = new Point(554, 379);
		((Control)np261).Name = "np261";
		((Control)np261).Size = new Size(290, 19);
		((Control)np261).TabIndex = 16;
		np261.TabStop = true;
		((Control)np261).Text = "راهنمای دستگاه حضوروغیاب NP261";
		np261.LinkClicked += new LinkLabelLinkClickedEventHandler(np261_LinkClicked);
		((Control)linkLabel4).Anchor = (AnchorStyles)9;
		((Control)linkLabel4).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel4).Location = new Point(42, 252);
		((Control)linkLabel4).Name = "linkLabel4";
		((Control)linkLabel4).Size = new Size(390, 14);
		((Control)linkLabel4).TabIndex = 22;
		linkLabel4.TabStop = true;
		((Control)linkLabel4).Text = "بخش 4 - معرفی و تعریف شیفت کاری";
		linkLabel4.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel4_LinkClicked);
		((Control)linkLabel5).Anchor = (AnchorStyles)9;
		((Control)linkLabel5).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel5).Location = new Point(42, 221);
		((Control)linkLabel5).Name = "linkLabel5";
		((Control)linkLabel5).Size = new Size(390, 14);
		((Control)linkLabel5).TabIndex = 21;
		linkLabel5.TabStop = true;
		((Control)linkLabel5).Text = "بخش 3 - تعریف شرکت";
		linkLabel5.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel5_LinkClicked);
		((Control)linkLabel6).Anchor = (AnchorStyles)9;
		((Control)linkLabel6).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel6).Location = new Point(42, 190);
		((Control)linkLabel6).Name = "linkLabel6";
		((Control)linkLabel6).Size = new Size(390, 14);
		((Control)linkLabel6).TabIndex = 20;
		linkLabel6.TabStop = true;
		((Control)linkLabel6).Text = "بخش 2 - نعریف دستگاه حضوروغیاب ودریافت اطلاعات\t";
		linkLabel6.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel6_LinkClicked);
		((Control)linkLabel7).Anchor = (AnchorStyles)9;
		((Control)linkLabel7).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel7).Location = new Point(42, 159);
		((Control)linkLabel7).Name = "linkLabel7";
		((Control)linkLabel7).Size = new Size(390, 14);
		((Control)linkLabel7).TabIndex = 19;
		linkLabel7.TabStop = true;
		((Control)linkLabel7).Text = "بخش 1 - اجرای نرم افزار و تعریف شرکت\t";
		linkLabel7.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel7_LinkClicked);
		((Control)CreateShift).Anchor = (AnchorStyles)9;
		((Control)CreateShift).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)CreateShift).Location = new Point(42, 97);
		((Control)CreateShift).Name = "CreateShift";
		((Control)CreateShift).Size = new Size(390, 14);
		((Control)CreateShift).TabIndex = 18;
		CreateShift.TabStop = true;
		((Control)CreateShift).Text = "آموزش تعریف شیفت در نرم افزار حضور حقوق " + TextResourceManagement.TextResource.CompanyName + "\t";
		CreateShift.LinkClicked += new LinkLabelLinkClickedEventHandler(CreateShift_LinkClicked);
		((Control)linkLabel9).Anchor = (AnchorStyles)9;
		((Control)linkLabel9).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel9).Location = new Point(42, 128);
		((Control)linkLabel9).Name = "linkLabel9";
		((Control)linkLabel9).Size = new Size(390, 14);
		((Control)linkLabel9).TabIndex = 17;
		linkLabel9.TabStop = true;
		((Control)linkLabel9).Text = "آموزش نرم افزار حقوق و دستمزد " + TextResourceManagement.TextResource.CompanyName + "(کلیه بخش ها)\t";
		linkLabel9.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel9_LinkClicked);
		((Control)linkLabel8).Anchor = (AnchorStyles)9;
		((Control)linkLabel8).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel8).Location = new Point(42, 283);
		((Control)linkLabel8).Name = "linkLabel8";
		((Control)linkLabel8).Size = new Size(390, 14);
		((Control)linkLabel8).TabIndex = 17;
		linkLabel8.TabStop = true;
		((Control)linkLabel8).Text = "بخش 5 - معرفی و تعریف حسابها\t";
		linkLabel8.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel8_LinkClicked);
		((Control)linkLabel10).Anchor = (AnchorStyles)9;
		((Control)linkLabel10).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel10).Location = new Point(42, 438);
		((Control)linkLabel10).Name = "linkLabel10";
		((Control)linkLabel10).Size = new Size(390, 14);
		((Control)linkLabel10).TabIndex = 28;
		linkLabel10.TabStop = true;
		((Control)linkLabel10).Text = "بخش 10 - گزارش تجمیع تردد پرسنل\t";
		linkLabel10.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel10_LinkClicked);
		((Control)linkLabel11).Anchor = (AnchorStyles)9;
		((Control)linkLabel11).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel11).Location = new Point(42, 407);
		((Control)linkLabel11).Name = "linkLabel11";
		((Control)linkLabel11).Size = new Size(390, 14);
		((Control)linkLabel11).TabIndex = 27;
		linkLabel11.TabStop = true;
		((Control)linkLabel11).Text = "بخش 9 - گزارش وضعیت پرسنل\t";
		linkLabel11.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel11_LinkClicked);
		((Control)linkLabel12).Anchor = (AnchorStyles)9;
		((Control)linkLabel12).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel12).Location = new Point(42, 376);
		((Control)linkLabel12).Name = "linkLabel12";
		((Control)linkLabel12).Size = new Size(390, 14);
		((Control)linkLabel12).TabIndex = 26;
		linkLabel12.TabStop = true;
		((Control)linkLabel12).Text = "بخش 8 - مشاهده گزارشات تردد پرسنل\t";
		linkLabel12.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel12_LinkClicked);
		((Control)linkLabel13).Anchor = (AnchorStyles)9;
		((Control)linkLabel13).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel13).Location = new Point(42, 345);
		((Control)linkLabel13).Name = "linkLabel13";
		((Control)linkLabel13).Size = new Size(390, 14);
		((Control)linkLabel13).TabIndex = 25;
		linkLabel13.TabStop = true;
		((Control)linkLabel13).Text = "بخش 7- صدور قرارداد\t";
		linkLabel13.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel13_LinkClicked);
		((Control)linkLabel14).Anchor = (AnchorStyles)9;
		((Control)linkLabel14).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel14).Location = new Point(42, 469);
		((Control)linkLabel14).Name = "linkLabel14";
		((Control)linkLabel14).Size = new Size(390, 14);
		((Control)linkLabel14).TabIndex = 23;
		linkLabel14.TabStop = true;
		((Control)linkLabel14).Text = "بخش 11- صدور فیش حقوقی پرسنل\t";
		linkLabel14.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel14_LinkClicked);
		((Control)linkLabel15).Anchor = (AnchorStyles)9;
		((Control)linkLabel15).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel15).Location = new Point(42, 314);
		((Control)linkLabel15).Name = "linkLabel15";
		((Control)linkLabel15).Size = new Size(390, 14);
		((Control)linkLabel15).TabIndex = 24;
		linkLabel15.TabStop = true;
		((Control)linkLabel15).Text = "بخش 6 - تعریف پرسنل\t";
		linkLabel15.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel15_LinkClicked);
		((Control)linkLabel16).Anchor = (AnchorStyles)9;
		((Control)linkLabel16).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel16).Location = new Point(42, 562);
		((Control)linkLabel16).Name = "linkLabel16";
		((Control)linkLabel16).Size = new Size(390, 14);
		((Control)linkLabel16).TabIndex = 32;
		linkLabel16.TabStop = true;
		((Control)linkLabel16).Text = "بخش 14 - تعریف کاربران نرم افزار و تعیین سطوح دسترسی\t";
		linkLabel16.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel16_LinkClicked);
		((Control)linkLabel17).Anchor = (AnchorStyles)9;
		((Control)linkLabel17).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel17).Location = new Point(42, 531);
		((Control)linkLabel17).Name = "linkLabel17";
		((Control)linkLabel17).Size = new Size(390, 14);
		((Control)linkLabel17).TabIndex = 31;
		linkLabel17.TabStop = true;
		((Control)linkLabel17).Text = "بخش 13 - تنظیمات ارسال پیامک تردد پرسنل\t";
		linkLabel17.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel17_LinkClicked);
		((Control)linkLabel18).Anchor = (AnchorStyles)9;
		((Control)linkLabel18).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)linkLabel18).Location = new Point(42, 500);
		((Control)linkLabel18).Name = "linkLabel18";
		((Control)linkLabel18).Size = new Size(390, 14);
		((Control)linkLabel18).TabIndex = 30;
		linkLabel18.TabStop = true;
		((Control)linkLabel18).Text = "بخش 12 - پشتیبانگیری و بازگردانی اطلاعات نرم افزار\t";
		linkLabel18.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel18_LinkClicked);
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Tahoma", 10f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label2).Location = new Point(87, 12);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(360, 17);
		((Control)label2).TabIndex = 33;
		((Control)label2).Text = "راهنمای آموزشی نرم افزار حقوق و دستمزد " + TextResourceManagement.TextResource.CompanyName;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(968, 641);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(984, 680);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(984, 680);
		((Control)this).Name = "FrmTutorialVideo";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "دانلود فیلم های آموزشی";
		((Control)this).KeyDown += new KeyEventHandler(FrmTutorialVideo_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx2).ResumeLayout(false);
		((Control)panelEx2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
