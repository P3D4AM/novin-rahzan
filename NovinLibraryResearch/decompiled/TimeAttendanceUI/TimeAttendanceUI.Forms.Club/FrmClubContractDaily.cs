using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRClub;

namespace TimeAttendanceUI.Forms.Club;

public class FrmClubContractDaily : FormExtention, IClubForms
{
	private readonly TimeAttendanceDAL.Dal.FLRClub.Club _club;

	private IClubForms _nextPage;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	private CheckBox ChkFri;

	private CheckBox ChkThu;

	private CheckBox ChkWed;

	private CheckBox ChkTue;

	private CheckBox ChkMon;

	private CheckBox ChkSun;

	private CheckBox ChkSat;

	internal LabelX labelX8;

	internal DateBoxFa TxtEndDate;

	internal DateBoxFa TXTStartDate;

	internal LabelX labelX10;

	internal LabelX labelX1;

	public FrmClubContractDaily(string contextGuid, TimeAttendanceDAL.Dal.FLRClub.Club club)
		: base(contextGuid, true, true)
	{
		try
		{
			InitializeComponent();
			_club = club;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 21);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((FormExtention)this).OnLoad(e);
			((Control)base.statusStrip1).Visible = false;
			FillFrmFields();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 34);
		}
	}

	public void FillFrmFields()
	{
		try
		{
			((Control)TXTStartDate).Text = Calendar.MiladiToCalture(_club.ContractStartDate);
			((Control)TxtEndDate).Text = Calendar.MiladiToCalture(_club.ContractEndDate);
			EnableChkEvents(enable: false);
			ChkSat.Checked = _club.ContractDays[0];
			ChkSun.Checked = _club.ContractDays[1];
			ChkMon.Checked = _club.ContractDays[2];
			ChkTue.Checked = _club.ContractDays[3];
			ChkWed.Checked = _club.ContractDays[4];
			ChkThu.Checked = _club.ContractDays[5];
			ChkFri.Checked = _club.ContractDays[6];
			EnableChkEvents(enable: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFrmFields", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 56);
		}
	}

	private void EnableChkEvents(bool enable)
	{
		try
		{
			if (enable)
			{
				ChkSat.CheckedChanged += ChkDays_CheckedChanged;
				ChkSun.CheckedChanged += ChkDays_CheckedChanged;
				ChkMon.CheckedChanged += ChkDays_CheckedChanged;
				ChkTue.CheckedChanged += ChkDays_CheckedChanged;
				ChkWed.CheckedChanged += ChkDays_CheckedChanged;
				ChkThu.CheckedChanged += ChkDays_CheckedChanged;
				ChkFri.CheckedChanged += ChkDays_CheckedChanged;
			}
			else
			{
				ChkSat.CheckedChanged -= ChkDays_CheckedChanged;
				ChkSun.CheckedChanged -= ChkDays_CheckedChanged;
				ChkMon.CheckedChanged -= ChkDays_CheckedChanged;
				ChkTue.CheckedChanged -= ChkDays_CheckedChanged;
				ChkWed.CheckedChanged -= ChkDays_CheckedChanged;
				ChkThu.CheckedChanged -= ChkDays_CheckedChanged;
				ChkFri.CheckedChanged -= ChkDays_CheckedChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnableChkEvents", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 87);
		}
	}

	public IClubForms NextPage()
	{
		try
		{
			return _nextPage ?? (_nextPage = new FrmClubAccountInfo(((CacheBaseForm)this).ContextGuid, _club));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NextPage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 98);
		}
		return null;
	}

	private void ChkDays_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			_club.ContractDays[0] = ChkSat.Checked;
			_club.ContractDays[1] = ChkSun.Checked;
			_club.ContractDays[2] = ChkMon.Checked;
			_club.ContractDays[3] = ChkTue.Checked;
			_club.ContractDays[4] = ChkWed.Checked;
			_club.ContractDays[5] = ChkThu.Checked;
			_club.ContractDays[6] = ChkFri.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkDays_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 117);
		}
	}

	private void TXTStartDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_club.ContractStartDate = TXTStartDate.MiladiDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTStartDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 128);
		}
	}

	private void TxtEndDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_club.ContractEndDate = TxtEndDate.MiladiDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtEndDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractDaily.cs", 140);
		}
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
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Expected O, but got Unknown
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0959: Expected O, but got Unknown
		//IL_0a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0e: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Expected O, but got Unknown
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Expected O, but got Unknown
		//IL_0bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf7: Expected O, but got Unknown
		//IL_0d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d89: Expected O, but got Unknown
		//IL_0db5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbf: Expected O, but got Unknown
		//IL_0f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f17: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		labelX1 = new LabelX();
		ChkFri = new CheckBox();
		ChkThu = new CheckBox();
		ChkWed = new CheckBox();
		ChkTue = new CheckBox();
		ChkMon = new CheckBox();
		ChkSun = new CheckBox();
		ChkSat = new CheckBox();
		labelX8 = new LabelX();
		TxtEndDate = new DateBoxFa();
		TXTStartDate = new DateBoxFa();
		labelX10 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 500);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel4).Controls.Add((Control)(object)ChkFri);
		((Control)groupPanel4).Controls.Add((Control)(object)ChkThu);
		((Control)groupPanel4).Controls.Add((Control)(object)ChkWed);
		((Control)groupPanel4).Controls.Add((Control)(object)ChkTue);
		((Control)groupPanel4).Controls.Add((Control)(object)ChkMon);
		((Control)groupPanel4).Controls.Add((Control)(object)ChkSun);
		((Control)groupPanel4).Controls.Add((Control)(object)ChkSat);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel4).Controls.Add((Control)(object)TxtEndDate);
		((Control)groupPanel4).Controls.Add((Control)(object)TXTStartDate);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX10);
		groupPanel4.DisabledBackColor = Color.Empty;
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 500);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "قرارداد روزهای خاص";
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(381, 137);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(138, 23);
		((Control)labelX1).TabIndex = 31;
		((Control)labelX1).Text = "انتخاب روزهای حضور:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)ChkFri).AutoSize = true;
		((Control)ChkFri).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkFri).ForeColor = Color.Blue;
		((Control)ChkFri).Location = new Point(321, 304);
		((Control)ChkFri).Name = "ChkFri";
		((Control)ChkFri).Size = new Size(54, 17);
		((Control)ChkFri).TabIndex = 30;
		((Control)ChkFri).Text = "جمعه";
		((ButtonBase)ChkFri).UseVisualStyleBackColor = true;
		ChkFri.CheckedChanged += ChkDays_CheckedChanged;
		((Control)ChkThu).AutoSize = true;
		((Control)ChkThu).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkThu).ForeColor = Color.Blue;
		((Control)ChkThu).Location = new Point(302, 281);
		((Control)ChkThu).Name = "ChkThu";
		((Control)ChkThu).Size = new Size(73, 17);
		((Control)ChkThu).TabIndex = 29;
		((Control)ChkThu).Text = "پنج شنبه";
		((ButtonBase)ChkThu).UseVisualStyleBackColor = true;
		ChkThu.CheckedChanged += ChkDays_CheckedChanged;
		((Control)ChkWed).AutoSize = true;
		((Control)ChkWed).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkWed).ForeColor = Color.Blue;
		((Control)ChkWed).Location = new Point(296, 258);
		((Control)ChkWed).Name = "ChkWed";
		((Control)ChkWed).Size = new Size(79, 17);
		((Control)ChkWed).TabIndex = 28;
		((Control)ChkWed).Text = "چهار شنبه";
		((ButtonBase)ChkWed).UseVisualStyleBackColor = true;
		ChkWed.CheckedChanged += ChkDays_CheckedChanged;
		((Control)ChkTue).AutoSize = true;
		((Control)ChkTue).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkTue).ForeColor = Color.Blue;
		((Control)ChkTue).Location = new Point(300, 235);
		((Control)ChkTue).Name = "ChkTue";
		((Control)ChkTue).Size = new Size(75, 17);
		((Control)ChkTue).TabIndex = 27;
		((Control)ChkTue).Text = "سه شنبه";
		((ButtonBase)ChkTue).UseVisualStyleBackColor = true;
		ChkTue.CheckedChanged += ChkDays_CheckedChanged;
		((Control)ChkMon).AutoSize = true;
		((Control)ChkMon).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkMon).ForeColor = Color.Blue;
		((Control)ChkMon).Location = new Point(307, 212);
		((Control)ChkMon).Name = "ChkMon";
		((Control)ChkMon).Size = new Size(68, 17);
		((Control)ChkMon).TabIndex = 26;
		((Control)ChkMon).Text = "دو شنبه";
		((ButtonBase)ChkMon).UseVisualStyleBackColor = true;
		ChkMon.CheckedChanged += ChkDays_CheckedChanged;
		((Control)ChkSun).AutoSize = true;
		((Control)ChkSun).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkSun).ForeColor = Color.Blue;
		((Control)ChkSun).Location = new Point(303, 189);
		((Control)ChkSun).Name = "ChkSun";
		((Control)ChkSun).Size = new Size(72, 17);
		((Control)ChkSun).TabIndex = 25;
		((Control)ChkSun).Text = "یک شنبه";
		((ButtonBase)ChkSun).UseVisualStyleBackColor = true;
		ChkSun.CheckedChanged += ChkDays_CheckedChanged;
		((Control)ChkSat).AutoSize = true;
		((Control)ChkSat).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkSat).ForeColor = Color.Blue;
		((Control)ChkSat).Location = new Point(322, 166);
		((Control)ChkSat).Name = "ChkSat";
		((Control)ChkSat).Size = new Size(53, 17);
		((Control)ChkSat).TabIndex = 24;
		((Control)ChkSat).Text = "شنبه";
		((ButtonBase)ChkSat).UseVisualStyleBackColor = true;
		ChkSat.CheckedChanged += ChkDays_CheckedChanged;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(381, 98);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(138, 23);
		((Control)labelX8).TabIndex = 20;
		((Control)labelX8).Text = "تاریخ پایان قرارداد:";
		labelX8.TextAlignment = (StringAlignment)2;
		TxtEndDate.AllowNull = false;
		TxtEndDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtEndDate).Border.CornerType = (eCornerType)1;
		TxtEndDate.EnterToTab = true;
		TxtEndDate.EscToClose = true;
		TxtEndDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtEndDate.FocusBackColor = Color.Yellow;
		TxtEndDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtEndDate.FocusForeColor = Color.Blue;
		TxtEndDate.FocusTextSelect = true;
		((Control)TxtEndDate).Font = new Font("Times New Roman", 12f);
		((Control)TxtEndDate).Location = new Point(224, 96);
		TxtEndDate.MessageEmptyShowDialog = false;
		TxtEndDate.MessegeEmpty = "";
		TxtEndDate.MessegeEmptyInFormRight = true;
		TxtEndDate.MessegeEmptyShowInForm = false;
		TxtEndDate.MiladiDate = new DateTime(2022, 6, 26, 16, 37, 51, 452);
		((Control)TxtEndDate).Name = "TxtEndDate";
		((Control)TxtEndDate).RightToLeft = (RightToLeft)0;
		TxtEndDate.ShowNameOfDayOnToolTip = false;
		TxtEndDate.ShowToolTipText = false;
		((Control)TxtEndDate).Size = new Size(151, 20);
		((Control)TxtEndDate).TabIndex = 21;
		((Control)TxtEndDate).Text = "26/06/2022";
		((TextBox)TxtEndDate).TextAlign = (HorizontalAlignment)2;
		TxtEndDate.TootTipDelay = 500;
		((Control)TxtEndDate).TextChanged += TxtEndDate_TextChanged;
		TXTStartDate.AllowNull = false;
		((Control)TXTStartDate).Anchor = (AnchorStyles)13;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTStartDate).Location = new Point(224, 57);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2022, 6, 26, 16, 37, 51, 455);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		TXTStartDate.ShowNameOfDayOnToolTip = false;
		TXTStartDate.ShowToolTipText = false;
		((Control)TXTStartDate).Size = new Size(151, 20);
		((Control)TXTStartDate).TabIndex = 19;
		((Control)TXTStartDate).Text = "26/06/2022";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((Control)TXTStartDate).TextChanged += TXTStartDate_TextChanged;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(381, 53);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(138, 23);
		((Control)labelX10).TabIndex = 18;
		((Control)labelX10).Text = "تاریخ شروع قرارداد:";
		labelX10.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmClubContractDaily";
		((Control)this).Text = "FrmCheckOutVacation";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel4).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	bool IClubForms.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IClubForms.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void IClubForms.Show()
	{
		((Control)this).Show();
	}
}
