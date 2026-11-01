using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Reports;

public class FrmGetDecision : BaseForm
{
	private ShiftDetView _shiftDetView;

	private string _ptime1;

	private string _ptime2;

	private TypeTaradodEnumType _traficTypeCalculate;

	private TypeTaradodEnumType _trafficTypeVacationOrMission;

	private string trFrmGetDecisionStr1 = "بازه تردد از ";

	private string trFrmGetDecisionStr2 = "  می باشد.";

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private LabelX LblTimeBetween;

	private LabelX LblSharh;

	private RadioButton RbtMission;

	private RadioButton RbtVacation;

	private Panel panel1;

	private RadioButton RbtNormal;

	private Panel panel2;

	private RadioButton RbtNormalWork;

	private RadioButton RbtFriDayWork;

	private RadioButton RbtNightWork;

	private RadioButton RbtShiftWork2;

	private RadioButton RbtShiftWork3;

	private RadioButton RbtShiftWork1;

	private RadioButton RbtTatilWork;

	private LabelX labelX4;

	private LabelX labelX2;

	public bool ShowVacation { get; set; } = true;


	public bool ShowMission { get; set; } = true;


	public bool ShowFriDayWork { get; set; } = true;


	public bool ShowShiftWork { get; set; } = true;


	public bool ShowNightWork { get; set; } = true;


	public bool ShowTatilWork { get; set; } = true;


	public bool HasShift { get; set; }

	public TypeTaradodEnumType TypeTaradodCalculateSelected
	{
		get
		{
			if (!RbtNightWork.Checked)
			{
				if (!RbtShiftWork1.Checked)
				{
					if (!RbtShiftWork2.Checked)
					{
						if (!RbtShiftWork3.Checked)
						{
							if (!RbtTatilWork.Checked)
							{
								if (!RbtFriDayWork.Checked)
								{
									return (TypeTaradodEnumType)34;
								}
								return (TypeTaradodEnumType)8;
							}
							return (TypeTaradodEnumType)28;
						}
						return (TypeTaradodEnumType)93;
					}
					return (TypeTaradodEnumType)92;
				}
				return (TypeTaradodEnumType)91;
			}
			return (TypeTaradodEnumType)10;
		}
	}

	public TypeTaradodEnumType TypeTaradodVacationOrMissionSelected
	{
		get
		{
			if (!RbtVacation.Checked)
			{
				if (!RbtMission.Checked)
				{
					return (TypeTaradodEnumType)34;
				}
				return (TypeTaradodEnumType)7;
			}
			return (TypeTaradodEnumType)5;
		}
	}

	public FrmGetDecision(string contextGuid, TypeTaradodEnumType traficTypeCalculate, TypeTaradodEnumType trafficTypeVacationOrMission, string time1, string time2, ShiftDetView shiftDetView)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_shiftDetView = shiftDetView;
		_ptime1 = time1;
		_ptime2 = time2;
		_traficTypeCalculate = traficTypeCalculate;
		_trafficTypeVacationOrMission = trafficTypeVacationOrMission;
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void SetRbtVacationOrMission(TypeTaradodEnumType type, bool hasShift)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		try
		{
			((Control)RbtVacation).Enabled = ShowVacation && hasShift;
			((Control)RbtMission).Enabled = ShowMission && hasShift;
			if ((int)type != 5)
			{
				if ((int)type == 7)
				{
					RbtMission.Checked = true;
				}
				else
				{
					RbtNormal.Checked = true;
				}
			}
			else
			{
				RbtVacation.Checked = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetRbtVacationOrMission", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmGetDecision.cs", 97);
		}
	}

	private void SetRbtTaradodTypeCalculation(TypeTaradodEnumType type, bool hasShift, bool shouldCalcHoliday, bool isHoliday, bool isFriday)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Invalid comparison between Unknown and I4
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Invalid comparison between Unknown and I4
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected I4, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Invalid comparison between Unknown and I4
		try
		{
			((Control)RbtNightWork).Enabled = ShowNightWork && hasShift && UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkNight;
			RadioButton rbtShiftWork = RbtShiftWork1;
			RadioButton rbtShiftWork2 = RbtShiftWork2;
			bool flag2 = (((Control)RbtShiftWork3).Enabled = ShowShiftWork && hasShift && UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkShift);
			bool enabled = (((Control)rbtShiftWork2).Enabled = flag2);
			((Control)rbtShiftWork).Enabled = enabled;
			((Control)RbtTatilWork).Enabled = ShowTatilWork && shouldCalcHoliday && isHoliday && UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkHoliday;
			((Control)RbtFriDayWork).Enabled = ShowFriDayWork && shouldCalcHoliday && isFriday && UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkHoliday;
			if ((int)type <= 10)
			{
				if ((int)type == 8)
				{
					RbtFriDayWork.Checked = true;
					return;
				}
				if ((int)type == 10)
				{
					RbtNightWork.Checked = true;
					return;
				}
			}
			else
			{
				if ((int)type == 28)
				{
					RbtTatilWork.Checked = true;
					return;
				}
				switch (type - 91)
				{
				case 0:
					RbtShiftWork1.Checked = true;
					return;
				case 1:
					RbtShiftWork2.Checked = true;
					return;
				case 2:
					RbtShiftWork3.Checked = true;
					return;
				}
			}
			RbtNormalWork.Checked = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetRbtTaradodTypeCalculation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmGetDecision.cs", 136);
		}
	}

	protected override async void OnLoad(EventArgs e)
	{
		_003C_003En__0(e);
		bool isFriday = false;
		bool isHoliday = false;
		bool hasShift = false;
		bool shouldCalcHoliday = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid)).CodeingAccounting.IsCalcHoliday;
		if (_shiftDetView != null)
		{
			ShiftCodeing shiftCoding = _shiftDetView.ClsShiftCoding;
			isFriday = shiftCoding.CurrDate.DayOfWeek == DayOfWeek.Friday;
			isHoliday = (await Holiday.GetAsync(((CacheBaseForm)this).ContextGuid, shiftCoding.CurrDate)).StRasmi;
			hasShift = shiftCoding.Vorod.TimeOfDay.TotalMinutes > 0.0 || shiftCoding.Khoroj > shiftCoding.Vorod;
		}
		if (!hasShift && !shouldCalcHoliday)
		{
			FMessageBox.Show("این تردد خارج از شیفت است لذا امکان تغییر وضعیت تردد وجود ندارد");
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		SetRbtVacationOrMission(_trafficTypeVacationOrMission, hasShift);
		SetRbtTaradodTypeCalculation(_traficTypeCalculate, hasShift, shouldCalcHoliday, isHoliday, isFriday);
		((Control)LblTimeBetween).Text = trFrmGetDecisionStr1 + _ptime1 + " الی " + _ptime2 + trFrmGetDecisionStr2;
		if (!((Control)RbtTatilWork).Visible && !((Control)RbtShiftWork1).Visible && !((Control)RbtShiftWork3).Visible)
		{
			((Control)LblSharh).Visible = true;
		}
	}

	private void FrmGetDecision_Load(object sender, EventArgs e)
	{
		try
		{
			trFrmGetDecisionStr1 = Utilities.GetTranslate(trFrmGetDecisionStr1);
			trFrmGetDecisionStr2 = Utilities.GetTranslate(trFrmGetDecisionStr2);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmGetDecision_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmGetDecision.cs", 183);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
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
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Expected O, but got Unknown
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Expected O, but got Unknown
		//IL_0dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dce: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		labelX4 = new LabelX();
		labelX2 = new LabelX();
		panel2 = new Panel();
		RbtNormalWork = new RadioButton();
		RbtFriDayWork = new RadioButton();
		RbtNightWork = new RadioButton();
		RbtShiftWork2 = new RadioButton();
		RbtShiftWork3 = new RadioButton();
		RbtShiftWork1 = new RadioButton();
		RbtTatilWork = new RadioButton();
		panel1 = new Panel();
		RbtNormal = new RadioButton();
		RbtMission = new RadioButton();
		RbtVacation = new RadioButton();
		LblTimeBetween = new LabelX();
		BtnCancel = new ButtonX();
		LblSharh = new LabelX();
		BtnAccept = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)panel2);
		((Control)panelEx1).Controls.Add((Control)(object)panel1);
		((Control)panelEx1).Controls.Add((Control)(object)LblTimeBetween);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)LblSharh);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(398, 342);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)labelX4).Location = new Point(12, 10);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(171, 19);
		((Control)labelX4).TabIndex = 20;
		((Control)labelX4).Text = "نحوه محاسبه تردد";
		labelX4.TextAlignment = (StringAlignment)1;
		labelX4.WordWrap = true;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)labelX2).Location = new Point(215, 10);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(171, 19);
		((Control)labelX2).TabIndex = 19;
		((Control)labelX2).Text = "نوع تردد ورود یا خروج";
		labelX2.TextAlignment = (StringAlignment)1;
		labelX2.WordWrap = true;
		panel2.BorderStyle = (BorderStyle)1;
		((Control)panel2).Controls.Add((Control)(object)RbtNormalWork);
		((Control)panel2).Controls.Add((Control)(object)RbtFriDayWork);
		((Control)panel2).Controls.Add((Control)(object)RbtNightWork);
		((Control)panel2).Controls.Add((Control)(object)RbtShiftWork2);
		((Control)panel2).Controls.Add((Control)(object)RbtShiftWork3);
		((Control)panel2).Controls.Add((Control)(object)RbtShiftWork1);
		((Control)panel2).Controls.Add((Control)(object)RbtTatilWork);
		((Control)panel2).Location = new Point(12, 32);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(172, 215);
		((Control)panel2).TabIndex = 18;
		((Control)RbtNormalWork).Location = new Point(3, 4);
		((Control)RbtNormalWork).Name = "RbtNormalWork";
		((Control)RbtNormalWork).RightToLeft = (RightToLeft)1;
		((Control)RbtNormalWork).Size = new Size(160, 23);
		((Control)RbtNormalWork).TabIndex = 16;
		RbtNormalWork.TabStop = true;
		((Control)RbtNormalWork).Text = "روزکاری";
		((ButtonBase)RbtNormalWork).UseVisualStyleBackColor = true;
		((Control)RbtFriDayWork).Location = new Point(3, 149);
		((Control)RbtFriDayWork).Name = "RbtFriDayWork";
		((Control)RbtFriDayWork).RightToLeft = (RightToLeft)1;
		((Control)RbtFriDayWork).Size = new Size(160, 23);
		((Control)RbtFriDayWork).TabIndex = 10;
		RbtFriDayWork.TabStop = true;
		((Control)RbtFriDayWork).Text = "جمعه کاری";
		((ButtonBase)RbtFriDayWork).UseVisualStyleBackColor = true;
		RbtNightWork.Checked = true;
		((Control)RbtNightWork).Location = new Point(3, 33);
		((Control)RbtNightWork).Name = "RbtNightWork";
		((Control)RbtNightWork).RightToLeft = (RightToLeft)1;
		((Control)RbtNightWork).Size = new Size(160, 23);
		((Control)RbtNightWork).TabIndex = 8;
		RbtNightWork.TabStop = true;
		((Control)RbtNightWork).Text = "شب کاری";
		((ButtonBase)RbtNightWork).UseVisualStyleBackColor = true;
		((Control)RbtShiftWork2).Location = new Point(3, 91);
		((Control)RbtShiftWork2).Name = "RbtShiftWork2";
		((Control)RbtShiftWork2).RightToLeft = (RightToLeft)1;
		((Control)RbtShiftWork2).Size = new Size(160, 23);
		((Control)RbtShiftWork2).TabIndex = 13;
		((Control)RbtShiftWork2).Text = "نوبت کاری عصر";
		((ButtonBase)RbtShiftWork2).UseVisualStyleBackColor = true;
		((Control)RbtShiftWork3).Location = new Point(3, 120);
		((Control)RbtShiftWork3).Name = "RbtShiftWork3";
		((Control)RbtShiftWork3).RightToLeft = (RightToLeft)1;
		((Control)RbtShiftWork3).Size = new Size(160, 23);
		((Control)RbtShiftWork3).TabIndex = 1;
		((Control)RbtShiftWork3).Text = "نوبت کاری شب";
		((ButtonBase)RbtShiftWork3).UseVisualStyleBackColor = true;
		((Control)RbtShiftWork1).Location = new Point(3, 62);
		((Control)RbtShiftWork1).Name = "RbtShiftWork1";
		((Control)RbtShiftWork1).RightToLeft = (RightToLeft)1;
		((Control)RbtShiftWork1).Size = new Size(160, 23);
		((Control)RbtShiftWork1).TabIndex = 12;
		RbtShiftWork1.TabStop = true;
		((Control)RbtShiftWork1).Text = "نوبت کاری صبح";
		((ButtonBase)RbtShiftWork1).UseVisualStyleBackColor = true;
		((Control)RbtTatilWork).Location = new Point(3, 178);
		((Control)RbtTatilWork).Name = "RbtTatilWork";
		((Control)RbtTatilWork).RightToLeft = (RightToLeft)1;
		((Control)RbtTatilWork).Size = new Size(160, 23);
		((Control)RbtTatilWork).TabIndex = 11;
		RbtTatilWork.TabStop = true;
		((Control)RbtTatilWork).Text = "تعطیل کاری";
		((ButtonBase)RbtTatilWork).UseVisualStyleBackColor = true;
		((Control)panel1).Anchor = (AnchorStyles)9;
		panel1.BorderStyle = (BorderStyle)1;
		((Control)panel1).Controls.Add((Control)(object)RbtNormal);
		((Control)panel1).Controls.Add((Control)(object)RbtMission);
		((Control)panel1).Controls.Add((Control)(object)RbtVacation);
		((Control)panel1).Location = new Point(215, 32);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(171, 215);
		((Control)panel1).TabIndex = 17;
		((Control)RbtNormal).Location = new Point(7, 4);
		((Control)RbtNormal).Name = "RbtNormal";
		((Control)RbtNormal).RightToLeft = (RightToLeft)1;
		((Control)RbtNormal).Size = new Size(154, 23);
		((Control)RbtNormal).TabIndex = 16;
		RbtNormal.TabStop = true;
		((Control)RbtNormal).Text = "تردد عادی";
		((ButtonBase)RbtNormal).UseVisualStyleBackColor = true;
		((Control)RbtMission).Location = new Point(7, 62);
		((Control)RbtMission).Name = "RbtMission";
		((Control)RbtMission).RightToLeft = (RightToLeft)1;
		((Control)RbtMission).Size = new Size(154, 23);
		((Control)RbtMission).TabIndex = 10;
		RbtMission.TabStop = true;
		((Control)RbtMission).Text = "ماموریت ساعتی";
		((ButtonBase)RbtMission).UseVisualStyleBackColor = true;
		RbtVacation.Checked = true;
		((Control)RbtVacation).Location = new Point(7, 33);
		((Control)RbtVacation).Name = "RbtVacation";
		((Control)RbtVacation).RightToLeft = (RightToLeft)1;
		((Control)RbtVacation).Size = new Size(154, 23);
		((Control)RbtVacation).TabIndex = 8;
		RbtVacation.TabStop = true;
		((Control)RbtVacation).Text = "مرخصی ساعتی";
		((ButtonBase)RbtVacation).UseVisualStyleBackColor = true;
		((Control)LblTimeBetween).Anchor = (AnchorStyles)14;
		((BaseItemControl)LblTimeBetween).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTimeBetween).Location = new Point(12, 276);
		((Control)LblTimeBetween).Name = "LblTimeBetween";
		((Control)LblTimeBetween).RightToLeft = (RightToLeft)1;
		((Control)LblTimeBetween).Size = new Size(374, 16);
		((Control)LblTimeBetween).TabIndex = 6;
		((Control)LblTimeBetween).Text = "[میباشد میخواهید به کدام نوع تغییر کند ؟]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 296);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 5;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)LblSharh).Anchor = (AnchorStyles)14;
		((BaseItemControl)LblSharh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSharh).ForeColor = Color.Red;
		((Control)LblSharh).Location = new Point(12, 255);
		((Control)LblSharh).Name = "LblSharh";
		((Control)LblSharh).RightToLeft = (RightToLeft)1;
		((Control)LblSharh).Size = new Size(374, 19);
		((Control)LblSharh).TabIndex = 7;
		((Control)LblSharh).Text = "[در صورت داشتن سطح 5 نرم افزار  این امکان برای شما وجود دارد   که نوع تردد را به جمعه کاری ، شب کاری و یا نوبت کاری تغییر دهید.]";
		((Control)LblSharh).Visible = false;
		LblSharh.WordWrap = true;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 296);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 4;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(398, 342);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmGetDecision";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[اصلاح نوع تردد]";
		((Form)this).Load += FrmGetDecision_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(EventArgs e)
	{
		((BaseForm)this).OnLoad(e);
	}
}
