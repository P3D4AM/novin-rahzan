using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using PacketParser.Attendance;
using PacketParser.Entities;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Reports;

public class FrmFilterTaradod : FormExtention
{
	private List<StatuseTaradodEnumType> _statuseTaradod;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TabItem tabItem1;

	private TabItem tabItem3;

	private TabItem tabItem2;

	private TabItem tabItem4;

	private CheckBoxX chBxSurcease;

	private CheckBoxX chBxMorakhsiRozane;

	private CheckBoxX chBxGheybat;

	private CheckBoxX chBxVOKHKamel;

	private CheckBoxX chBxExitHurry;

	private CheckBoxX chBxMorakhsiSaati;

	private CheckBoxX chBxArrivalDelay;

	private CheckBoxX chBxMission;

	private CheckBoxX chBxHourlyMission;

	private ButtonX btnSelectAll;

	private ButtonX btnUnSelectAll;

	private CheckBoxX chBxAdamGharardad;

	private CheckBoxX chBxLeavingWork;

	private CheckBoxX chBxVOKHNaghes;

	private CheckBoxX chBxAdamShift;

	private Label label1;

	public FrmFilterTaradod(string contextGuid, List<StatuseTaradodEnumType> statuseTaradod)
		: base(contextGuid, true, true)
	{
		InitializeComponent();
		_statuseTaradod = statuseTaradod;
		chBxGheybat.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 3 || (int)q == 13 || (int)q == 12);
		chBxSurcease.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 11);
		chBxMorakhsiRozane.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 5);
		chBxMission.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 14);
		chBxExitHurry.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 15);
		chBxArrivalDelay.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 16);
		chBxMorakhsiSaati.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 9);
		chBxHourlyMission.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 17);
		chBxAdamGharardad.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 4);
		chBxLeavingWork.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 18 || (int)q == 20 || (int)q == 19);
		chBxAdamShift.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 8);
		chBxVOKHKamel.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 1);
		chBxVOKHNaghes.Checked = _statuseTaradod.Any((StatuseTaradodEnumType q) => (int)q == 2);
	}

	protected override void OnLoad(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
		((BaseForm)this).NoCheckFormContentsChanged = true;
	}

	private void AcceptEvent(object sender, EventArgs e)
	{
		try
		{
			((FormExtention)this).ResetControlChanged();
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AcceptEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilterTaradod.cs", 59);
		}
	}

	public List<StatuseTaradodEnumType> GetStatuseTaradodSelected()
	{
		_statuseTaradod.Clear();
		if (chBxVOKHKamel.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)1);
		}
		if (chBxVOKHNaghes.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)2);
		}
		if (chBxGheybat.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)3);
			_statuseTaradod.Add((StatuseTaradodEnumType)13);
			_statuseTaradod.Add((StatuseTaradodEnumType)12);
		}
		if (chBxSurcease.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)11);
		}
		if (chBxMorakhsiRozane.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)5);
		}
		if (chBxMission.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)14);
		}
		if (chBxExitHurry.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)15);
		}
		if (chBxArrivalDelay.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)16);
		}
		if (chBxMorakhsiSaati.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)9);
		}
		if (chBxHourlyMission.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)17);
		}
		if (chBxAdamGharardad.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)4);
		}
		if (chBxAdamShift.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)8);
		}
		if (chBxLeavingWork.Checked)
		{
			_statuseTaradod.Add((StatuseTaradodEnumType)18);
			_statuseTaradod.Add((StatuseTaradodEnumType)20);
			_statuseTaradod.Add((StatuseTaradodEnumType)19);
		}
		if (_statuseTaradod.Count == 17)
		{
			_statuseTaradod.Clear();
		}
		return _statuseTaradod;
	}

	private void btnSelectAll_Click(object sender, EventArgs e)
	{
		_statuseTaradod.Clear();
		CheckBoxX obj = chBxVOKHKamel;
		CheckBoxX obj2 = chBxVOKHNaghes;
		CheckBoxX obj3 = chBxAdamShift;
		CheckBoxX obj4 = chBxGheybat;
		CheckBoxX obj5 = chBxSurcease;
		CheckBoxX obj6 = chBxMorakhsiRozane;
		CheckBoxX obj7 = chBxMission;
		CheckBoxX obj8 = chBxExitHurry;
		CheckBoxX obj9 = chBxArrivalDelay;
		CheckBoxX obj10 = chBxMorakhsiSaati;
		CheckBoxX obj11 = chBxHourlyMission;
		CheckBoxX obj12 = chBxAdamGharardad;
		bool flag2 = (chBxLeavingWork.Checked = true);
		bool flag4 = (obj12.Checked = flag2);
		bool flag6 = (obj11.Checked = flag4);
		bool flag8 = (obj10.Checked = flag6);
		bool flag10 = (obj9.Checked = flag8);
		bool flag12 = (obj8.Checked = flag10);
		bool flag14 = (obj7.Checked = flag12);
		bool flag16 = (obj6.Checked = flag14);
		bool flag18 = (obj5.Checked = flag16);
		bool flag20 = (obj4.Checked = flag18);
		bool flag22 = (obj3.Checked = flag20);
		bool @checked = (obj2.Checked = flag22);
		obj.Checked = @checked;
	}

	private void btnUnSelectAll_Click(object sender, EventArgs e)
	{
		_statuseTaradod.Clear();
		CheckBoxX obj = chBxVOKHKamel;
		CheckBoxX obj2 = chBxVOKHNaghes;
		CheckBoxX obj3 = chBxAdamShift;
		CheckBoxX obj4 = chBxGheybat;
		CheckBoxX obj5 = chBxSurcease;
		CheckBoxX obj6 = chBxMorakhsiRozane;
		CheckBoxX obj7 = chBxMission;
		CheckBoxX obj8 = chBxExitHurry;
		CheckBoxX obj9 = chBxArrivalDelay;
		CheckBoxX obj10 = chBxMorakhsiSaati;
		CheckBoxX obj11 = chBxHourlyMission;
		CheckBoxX obj12 = chBxAdamGharardad;
		bool flag2 = (chBxLeavingWork.Checked = false);
		bool flag4 = (obj12.Checked = flag2);
		bool flag6 = (obj11.Checked = flag4);
		bool flag8 = (obj10.Checked = flag6);
		bool flag10 = (obj9.Checked = flag8);
		bool flag12 = (obj8.Checked = flag10);
		bool flag14 = (obj7.Checked = flag12);
		bool flag16 = (obj6.Checked = flag14);
		bool flag18 = (obj5.Checked = flag16);
		bool flag20 = (obj4.Checked = flag18);
		bool flag22 = (obj3.Checked = flag20);
		bool @checked = (obj2.Checked = flag22);
		obj.Checked = @checked;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
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
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccd: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f05: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		label1 = new Label();
		chBxAdamShift = new CheckBoxX();
		chBxVOKHNaghes = new CheckBoxX();
		chBxLeavingWork = new CheckBoxX();
		chBxAdamGharardad = new CheckBoxX();
		chBxSurcease = new CheckBoxX();
		chBxExitHurry = new CheckBoxX();
		chBxHourlyMission = new CheckBoxX();
		chBxMorakhsiSaati = new CheckBoxX();
		chBxArrivalDelay = new CheckBoxX();
		chBxMission = new CheckBoxX();
		chBxMorakhsiRozane = new CheckBoxX();
		chBxGheybat = new CheckBoxX();
		chBxVOKHKamel = new CheckBoxX();
		BtnCancel = new ButtonX();
		btnSelectAll = new ButtonX();
		btnUnSelectAll = new ButtonX();
		BtnAccept = new ButtonX();
		tabItem1 = new TabItem(components);
		tabItem3 = new TabItem(components);
		tabItem2 = new TabItem(components);
		tabItem4 = new TabItem(components);
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)label1);
		((Control)panelEx1).Controls.Add((Control)(object)chBxAdamShift);
		((Control)panelEx1).Controls.Add((Control)(object)chBxVOKHNaghes);
		((Control)panelEx1).Controls.Add((Control)(object)chBxLeavingWork);
		((Control)panelEx1).Controls.Add((Control)(object)chBxAdamGharardad);
		((Control)panelEx1).Controls.Add((Control)(object)chBxSurcease);
		((Control)panelEx1).Controls.Add((Control)(object)chBxExitHurry);
		((Control)panelEx1).Controls.Add((Control)(object)chBxHourlyMission);
		((Control)panelEx1).Controls.Add((Control)(object)chBxMorakhsiSaati);
		((Control)panelEx1).Controls.Add((Control)(object)chBxArrivalDelay);
		((Control)panelEx1).Controls.Add((Control)(object)chBxMission);
		((Control)panelEx1).Controls.Add((Control)(object)chBxMorakhsiRozane);
		((Control)panelEx1).Controls.Add((Control)(object)chBxGheybat);
		((Control)panelEx1).Controls.Add((Control)(object)chBxVOKHKamel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)btnSelectAll);
		((Control)panelEx1).Controls.Add((Control)(object)btnUnSelectAll);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(315, 376);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Tahoma", 18f, (FontStyle)1);
		((Control)label1).Location = new Point(169, 14);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(28, 29);
		((Control)label1).TabIndex = 15;
		((Control)label1).Text = "}";
		((Control)chBxAdamShift).Anchor = (AnchorStyles)9;
		((Control)chBxAdamShift).AutoSize = true;
		((BaseItemControl)chBxAdamShift).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxAdamShift).Location = new Point(236, 56);
		((Control)chBxAdamShift).Name = "chBxAdamShift";
		((Control)chBxAdamShift).Size = new Size(65, 15);
		((BaseItemControl)chBxAdamShift).Style = (eDotNetBarStyle)9;
		((Control)chBxAdamShift).TabIndex = 14;
		((Control)chBxAdamShift).Text = "[عدم شیفت]";
		((Control)chBxVOKHNaghes).Anchor = (AnchorStyles)9;
		((Control)chBxVOKHNaghes).AutoSize = true;
		((BaseItemControl)chBxVOKHNaghes).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxVOKHNaghes).Location = new Point(206, 34);
		((Control)chBxVOKHNaghes).Name = "chBxVOKHNaghes";
		((Control)chBxVOKHNaghes).Size = new Size(95, 15);
		((BaseItemControl)chBxVOKHNaghes).Style = (eDotNetBarStyle)9;
		((Control)chBxVOKHNaghes).TabIndex = 13;
		((Control)chBxVOKHNaghes).Text = "[ورود خروج ناقص]";
		((Control)chBxLeavingWork).Anchor = (AnchorStyles)9;
		((Control)chBxLeavingWork).AutoSize = true;
		((BaseItemControl)chBxLeavingWork).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxLeavingWork).Location = new Point(243, 78);
		((Control)chBxLeavingWork).Name = "chBxLeavingWork";
		((Control)chBxLeavingWork).Size = new Size(58, 15);
		((BaseItemControl)chBxLeavingWork).Style = (eDotNetBarStyle)9;
		((Control)chBxLeavingWork).TabIndex = 12;
		((Control)chBxLeavingWork).Text = "[ترک کار]";
		((Control)chBxAdamGharardad).Anchor = (AnchorStyles)9;
		((Control)chBxAdamGharardad).AutoSize = true;
		((BaseItemControl)chBxAdamGharardad).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxAdamGharardad).Location = new Point(90, 22);
		((Control)chBxAdamGharardad).Name = "chBxAdamGharardad";
		((Control)chBxAdamGharardad).Size = new Size(78, 15);
		((BaseItemControl)chBxAdamGharardad).Style = (eDotNetBarStyle)9;
		((Control)chBxAdamGharardad).TabIndex = 11;
		((Control)chBxAdamGharardad).Text = "[عدم قرارداد]+";
		((Control)chBxSurcease).Anchor = (AnchorStyles)9;
		((Control)chBxSurcease).AutoSize = true;
		((BaseItemControl)chBxSurcease).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxSurcease).Location = new Point(239, 122);
		((Control)chBxSurcease).Name = "chBxSurcease";
		((Control)chBxSurcease).Size = new Size(62, 15);
		((BaseItemControl)chBxSurcease).Style = (eDotNetBarStyle)9;
		((Control)chBxSurcease).TabIndex = 2;
		((Control)chBxSurcease).Text = "[استراحت]";
		((Control)chBxExitHurry).Anchor = (AnchorStyles)9;
		((Control)chBxExitHurry).AutoSize = true;
		((BaseItemControl)chBxExitHurry).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxExitHurry).Location = new Point(226, 166);
		((Control)chBxExitHurry).Name = "chBxExitHurry";
		((Control)chBxExitHurry).Size = new Size(75, 15);
		((BaseItemControl)chBxExitHurry).Style = (eDotNetBarStyle)9;
		((Control)chBxExitHurry).TabIndex = 5;
		((Control)chBxExitHurry).Text = "[تعجیل خروج]";
		((Control)chBxHourlyMission).Anchor = (AnchorStyles)9;
		((Control)chBxHourlyMission).AutoSize = true;
		((BaseItemControl)chBxHourlyMission).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxHourlyMission).Location = new Point(213, 232);
		((Control)chBxHourlyMission).Name = "chBxHourlyMission";
		((Control)chBxHourlyMission).Size = new Size(88, 15);
		((BaseItemControl)chBxHourlyMission).Style = (eDotNetBarStyle)9;
		((Control)chBxHourlyMission).TabIndex = 8;
		((Control)chBxHourlyMission).Text = "[ماموریت ساعتی]";
		((Control)chBxMorakhsiSaati).Anchor = (AnchorStyles)9;
		((Control)chBxMorakhsiSaati).AutoSize = true;
		((BaseItemControl)chBxMorakhsiSaati).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxMorakhsiSaati).Location = new Point(214, 188);
		((Control)chBxMorakhsiSaati).Name = "chBxMorakhsiSaati";
		((Control)chBxMorakhsiSaati).Size = new Size(87, 15);
		((BaseItemControl)chBxMorakhsiSaati).Style = (eDotNetBarStyle)9;
		((Control)chBxMorakhsiSaati).TabIndex = 7;
		((Control)chBxMorakhsiSaati).Text = "[مرخصی ساعتی]";
		((Control)chBxArrivalDelay).Anchor = (AnchorStyles)9;
		((Control)chBxArrivalDelay).AutoSize = true;
		((BaseItemControl)chBxArrivalDelay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxArrivalDelay).Location = new Point(232, 144);
		((Control)chBxArrivalDelay).Name = "chBxArrivalDelay";
		((Control)chBxArrivalDelay).Size = new Size(69, 15);
		((BaseItemControl)chBxArrivalDelay).Style = (eDotNetBarStyle)9;
		((Control)chBxArrivalDelay).TabIndex = 6;
		((Control)chBxArrivalDelay).Text = "[تاخیر ورود]";
		((Control)chBxMission).Anchor = (AnchorStyles)9;
		((Control)chBxMission).AutoSize = true;
		((BaseItemControl)chBxMission).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxMission).Location = new Point(213, 254);
		((Control)chBxMission).Name = "chBxMission";
		((Control)chBxMission).Size = new Size(88, 15);
		((BaseItemControl)chBxMission).Style = (eDotNetBarStyle)9;
		((Control)chBxMission).TabIndex = 4;
		((Control)chBxMission).Text = "[ماموریت روزانه]";
		((Control)chBxMorakhsiRozane).Anchor = (AnchorStyles)9;
		((Control)chBxMorakhsiRozane).AutoSize = true;
		((BaseItemControl)chBxMorakhsiRozane).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxMorakhsiRozane).Location = new Point(213, 210);
		((Control)chBxMorakhsiRozane).Name = "chBxMorakhsiRozane";
		((Control)chBxMorakhsiRozane).Size = new Size(88, 15);
		((BaseItemControl)chBxMorakhsiRozane).Style = (eDotNetBarStyle)9;
		((Control)chBxMorakhsiRozane).TabIndex = 3;
		((Control)chBxMorakhsiRozane).Text = "[مرخصی روزانه]";
		((Control)chBxGheybat).Anchor = (AnchorStyles)9;
		((Control)chBxGheybat).AutoSize = true;
		((BaseItemControl)chBxGheybat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxGheybat).Location = new Point(253, 100);
		((Control)chBxGheybat).Name = "chBxGheybat";
		((Control)chBxGheybat).Size = new Size(48, 15);
		((BaseItemControl)chBxGheybat).Style = (eDotNetBarStyle)9;
		((Control)chBxGheybat).TabIndex = 1;
		((Control)chBxGheybat).Text = "[غیبت]";
		((Control)chBxVOKHKamel).Anchor = (AnchorStyles)9;
		((Control)chBxVOKHKamel).AutoSize = true;
		((BaseItemControl)chBxVOKHKamel).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxVOKHKamel).Location = new Point(209, 12);
		((Control)chBxVOKHKamel).Name = "chBxVOKHKamel";
		((Control)chBxVOKHKamel).Size = new Size(92, 15);
		((BaseItemControl)chBxVOKHKamel).Style = (eDotNetBarStyle)9;
		((Control)chBxVOKHKamel).TabIndex = 0;
		((Control)chBxVOKHKamel).Text = "[ورود خروج کامل]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 317);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(141, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 10;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)btnSelectAll).AccessibleRole = (AccessibleRole)43;
		((Control)btnSelectAll).Anchor = (AnchorStyles)6;
		btnSelectAll.ColorTable = (eButtonColor)6;
		((Control)btnSelectAll).Font = new Font("Tahoma", 6f, (FontStyle)1);
		btnSelectAll.Image = (Image)(object)Resources.check2;
		btnSelectAll.ImageFixedSize = new Size(15, 15);
		btnSelectAll.ImagePosition = (eImagePosition)1;
		((Control)btnSelectAll).Location = new Point(162, 276);
		((Control)btnSelectAll).Name = "btnSelectAll";
		((Control)btnSelectAll).RightToLeft = (RightToLeft)1;
		btnSelectAll.Shortcuts.Add((eShortcut)116);
		((Control)btnSelectAll).Size = new Size(146, 34);
		((PopupItemControl)btnSelectAll).Style = (eDotNetBarStyle)9;
		((Control)btnSelectAll).TabIndex = 9;
		((Control)btnSelectAll).Text = "   [انتخاب همه]";
		btnSelectAll.TextAlignment = (eButtonTextAlignment)0;
		((Control)btnSelectAll).Click += btnSelectAll_Click;
		((Control)btnUnSelectAll).AccessibleRole = (AccessibleRole)43;
		((Control)btnUnSelectAll).Anchor = (AnchorStyles)6;
		btnUnSelectAll.ColorTable = (eButtonColor)6;
		((Control)btnUnSelectAll).Font = new Font("Tahoma", 6.2f, (FontStyle)1);
		btnUnSelectAll.Image = (Image)(object)Resources.uncheck1;
		btnUnSelectAll.ImageFixedSize = new Size(15, 15);
		btnUnSelectAll.ImagePosition = (eImagePosition)1;
		((Control)btnUnSelectAll).Location = new Point(14, 276);
		((Control)btnUnSelectAll).Name = "btnUnSelectAll";
		((Control)btnUnSelectAll).RightToLeft = (RightToLeft)1;
		btnUnSelectAll.Shortcuts.Add((eShortcut)116);
		((Control)btnUnSelectAll).Size = new Size(141, 34);
		((PopupItemControl)btnUnSelectAll).Style = (eDotNetBarStyle)9;
		((Control)btnUnSelectAll).TabIndex = 9;
		((Control)btnUnSelectAll).Text = "  [پاک کردن همه]";
		btnUnSelectAll.TextAlignment = (eButtonTextAlignment)0;
		((Control)btnUnSelectAll).Click += btnUnSelectAll_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)14;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 9f, (FontStyle)1);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(162, 317);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(146, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 9;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += AcceptEvent;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[اطلاعات عمومی]";
		tabItem3.Name = "tabItem3";
		tabItem3.Text = "[اطلاعات استخدامی]";
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[اطلاعات حقوقی]";
		tabItem4.Name = "tabItem4";
		tabItem4.Text = "[مزایا و کسورات]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(315, 376);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmFilterTaradod";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فرم محدودسازی]";
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
