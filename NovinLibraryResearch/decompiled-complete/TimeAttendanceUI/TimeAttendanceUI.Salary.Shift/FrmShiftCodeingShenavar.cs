using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendance.ValidatorAttribute.Salary.FLRShift;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftCodeingShenavar : FormExtention
{
	private readonly ShiftCodeing _clsShiftCodeing;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	private NumericUpDown MinShenavar;

	private NumericUpDown HourShenavar;

	internal LabelX labelX6;

	private GroupPanel groupPanel1;

	private LabelX labelX8;

	private TextBoxX TxtDesc;

	private NumericUpDown MlBLMaxTimeCanExit;

	internal LabelX labelX17;

	private NumericUpDown HlBLMaxTimeCanExit;

	private CheckBoxX ChbNextDay;

	private LabelX lBLMaxTimeCanExit;

	private ToolTip toolTip1;

	private GroupPanel grpPanelPeakOvertime;

	private NumericUpDown MinutePeakOvertime;

	private NumericUpDown HourPeakOvertime;

	internal LabelX labelX18;

	internal ButtonX BtnHelp;

	private CheckBoxX ChBxShiftSeparation;

	private GroupBox grpBoxNextDay;

	private async Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[افزودن یک نوع تردد جدید]", out var value))
			{
				trDictionary.Add("[افزودن یک نوع تردد جدید]", Utilities.GetTranslate("[افزودن یک نوع تردد جدید]"));
			}
			if (!trDictionary.TryGetValue("[شیفت شناور]", out value))
			{
				trDictionary.Add("[شیفت شناور]", Utilities.GetTranslate("[شیفت شناور]"));
			}
			if (!trDictionary.TryGetValue("[ویرایش یک نوع تردد]", out value))
			{
				trDictionary.Add("[ویرایش یک نوع تردد]", Utilities.GetTranslate("[ویرایش یک نوع تردد]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingShenavar.cs", 30);
		}
	}

	public FrmShiftCodeingShenavar(string contextGuid, ShiftCodeing shiftCodeing)
		: base(contextGuid, true, true)
	{
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Invalid comparison between Unknown and I4
		Task.Run(async delegate
		{
			await SetTrDictionaryAsync();
		});
		InitializeComponent();
		_clsShiftCodeing = shiftCodeing;
		if (_clsShiftCodeing.Id <= 0)
		{
			((Control)this).Text = "افزودن یک نوع تردد جدید";
			if (trDictionary.TryGetValue("[افزودن یک نوع تردد جدید]", out var value))
			{
				((Control)this).Text = value;
			}
			((Control)TxtDesc).Text = "شیفت شناور";
			if (trDictionary.TryGetValue("[شیفت شناور]", out var value2))
			{
				((Control)TxtDesc).Text = value2;
			}
			return;
		}
		((Control)this).Text = "ویرایش یک نوع تردد";
		if (trDictionary.TryGetValue("[ویرایش یک نوع تردد]", out var value3))
		{
			((Control)this).Text = value3;
		}
		MinShenavar.Value = _clsShiftCodeing.M2 - _clsShiftCodeing.M1;
		HourShenavar.Value = _clsShiftCodeing.H2 - _clsShiftCodeing.H1;
		string text = Time.IntToTime(_clsShiftCodeing.MaxTimeCanExit);
		if (!string.IsNullOrEmpty(text))
		{
			HlBLMaxTimeCanExit.Value = int.Parse(text.Substring(0, 2));
			MlBLMaxTimeCanExit.Value = int.Parse(text.Substring(3, 2));
		}
		ChbNextDay.Checked = _clsShiftCodeing.NextDay;
		ChBxShiftSeparation.Checked = (int)_clsShiftCodeing.ShiftSeparation == 1;
		if (_clsShiftCodeing.PeakOvertime >= 1440)
		{
			_clsShiftCodeing.PeakOvertime = 1439;
		}
		text = Time.IntToTime((int)_clsShiftCodeing.PeakOvertime);
		if (!string.IsNullOrEmpty(text))
		{
			HourPeakOvertime.Value = int.Parse(text.Substring(0, 2));
			MinutePeakOvertime.Value = int.Parse(text.Substring(3, 2));
		}
		((Control)TxtDesc).Text = _clsShiftCodeing.Description;
		toolTip1.SetToolTip((Control)(object)lBLMaxTimeCanExit, Utilities.GetTranslate("[حداکثر مدت مجاز حضور معادل جمع مدت حضور موظفی بعلاوه مدت مجاز بعد از اتمام شیفت می باشد]"));
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(AcceptEvent);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
		SetTooltip();
	}

	private async void AcceptEvent(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			GetData();
			DialogResult val = ShiftCodeingCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, _clsShiftCodeing, isShowMessage: true);
			if ((int)val == 6 || (int)val == 5)
			{
				((FormExtention)this).ResetControlChanged();
				((Form)this).DialogResult = (DialogResult)((await _clsShiftCodeing.SaveAsync()) ? 1 : 2);
				((Form)this).Close();
			}
			else if ((int)val == 2)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AcceptEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingShenavar.cs", 113);
		}
	}

	private void GetData()
	{
		_clsShiftCodeing.NextDay = false;
		_clsShiftCodeing.TypeShift = (TypeShiftEnumType)2;
		_clsShiftCodeing.H1 = 0;
		_clsShiftCodeing.H2 = short.Parse(HourShenavar.Value.ToString());
		_clsShiftCodeing.M1 = 0;
		_clsShiftCodeing.M2 = short.Parse(MinShenavar.Value.ToString());
		_clsShiftCodeing.Description = ((Control)TxtDesc).Text;
		if (ChbNextDay.Checked)
		{
			_clsShiftCodeing.NextDay = true;
		}
		else
		{
			_clsShiftCodeing.NextDay = false;
		}
		_clsShiftCodeing.MaxTimeCanExit = (int)HlBLMaxTimeCanExit.Value * 60 + (int)MlBLMaxTimeCanExit.Value;
		_clsShiftCodeing.PeakOvertime = (short)(HourPeakOvertime.Value * 60m + MinutePeakOvertime.Value);
		_clsShiftCodeing.ShiftSeparation = (ShiftSeparationEnumType)(ChBxShiftSeparation.Checked ? 1 : 0);
	}

	private void SetTooltip()
	{
		string text = "[ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.]";
		toolTip1.SetToolTip((Control)(object)lBLMaxTimeCanExit, text);
		toolTip1.SetToolTip((Control)(object)HlBLMaxTimeCanExit, text);
		toolTip1.SetToolTip((Control)(object)MlBLMaxTimeCanExit, text);
	}

	private void ChbNextDay_CheckedChanged(object sender, EventArgs e)
	{
		((Control)grpBoxNextDay).Visible = ChbNextDay.Checked;
	}

	private void BtnHelp_Click(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, "", "[توضیحات انتقال از روز قبل]")).ShowDialog();
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
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Expected O, but got Unknown
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eab: Expected O, but got Unknown
		//IL_13d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13de: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		grpBoxNextDay = new GroupBox();
		ChBxShiftSeparation = new CheckBoxX();
		BtnHelp = new ButtonX();
		labelX17 = new LabelX();
		lBLMaxTimeCanExit = new LabelX();
		MlBLMaxTimeCanExit = new NumericUpDown();
		HlBLMaxTimeCanExit = new NumericUpDown();
		grpPanelPeakOvertime = new GroupPanel();
		MinutePeakOvertime = new NumericUpDown();
		HourPeakOvertime = new NumericUpDown();
		labelX18 = new LabelX();
		ChbNextDay = new CheckBoxX();
		labelX8 = new LabelX();
		TxtDesc = new TextBoxX();
		groupPanel1 = new GroupPanel();
		MinShenavar = new NumericUpDown();
		HourShenavar = new NumericUpDown();
		labelX6 = new LabelX();
		toolTip1 = new ToolTip(components);
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)grpBoxNextDay).SuspendLayout();
		((ISupportInitialize)MlBLMaxTimeCanExit).BeginInit();
		((ISupportInitialize)HlBLMaxTimeCanExit).BeginInit();
		((Control)grpPanelPeakOvertime).SuspendLayout();
		((ISupportInitialize)MinutePeakOvertime).BeginInit();
		((ISupportInitialize)HourPeakOvertime).BeginInit();
		((Control)groupPanel1).SuspendLayout();
		((ISupportInitialize)MinShenavar).BeginInit();
		((ISupportInitialize)HourShenavar).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(356, 412);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 353);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 353);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(224, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += AcceptEvent;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)grpBoxNextDay);
		((Control)groupPanel7).Controls.Add((Control)(object)grpPanelPeakOvertime);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbNextDay);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtDesc);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel1);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(333, 335);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderBottomWidth = 3;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 3;
		((PanelControl)groupPanel7).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 3;
		((PanelControl)groupPanel7).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 3;
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 0;
		((Control)grpBoxNextDay).Controls.Add((Control)(object)ChBxShiftSeparation);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)BtnHelp);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)labelX17);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)lBLMaxTimeCanExit);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)MlBLMaxTimeCanExit);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)HlBLMaxTimeCanExit);
		((Control)grpBoxNextDay).Location = new Point(3, 90);
		((Control)grpBoxNextDay).Name = "grpBoxNextDay";
		((Control)grpBoxNextDay).Size = new Size(314, 78);
		((Control)grpBoxNextDay).TabIndex = 2;
		grpBoxNextDay.TabStop = false;
		((Control)grpBoxNextDay).Visible = false;
		((Control)ChBxShiftSeparation).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChBxShiftSeparation).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChBxShiftSeparation).Location = new Point(3, 47);
		((Control)ChBxShiftSeparation).Name = "ChBxShiftSeparation";
		((Control)ChBxShiftSeparation).Size = new Size(312, 23);
		((BaseItemControl)ChBxShiftSeparation).Style = (eDotNetBarStyle)9;
		((Control)ChBxShiftSeparation).TabIndex = 4;
		((Control)ChBxShiftSeparation).Text = "[تردد ورود و خروج مجزا]";
		((Control)ChBxShiftSeparation).Visible = false;
		((Control)BtnHelp).AccessibleRole = (AccessibleRole)43;
		BtnHelp.ColorTable = (eButtonColor)6;
		((Control)BtnHelp).Location = new Point(0, 20);
		((Control)BtnHelp).Name = "BtnHelp";
		((Control)BtnHelp).Size = new Size(18, 20);
		((PopupItemControl)BtnHelp).Style = (eDotNetBarStyle)9;
		((Control)BtnHelp).TabIndex = 5;
		((Control)BtnHelp).TabStop = false;
		((Control)BtnHelp).Text = "i";
		((Control)BtnHelp).Click += BtnHelp_Click;
		((Control)labelX17).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX17).Location = new Point(66, 20);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(9, 21);
		((Control)labelX17).TabIndex = 2;
		((Control)labelX17).Text = ":";
		labelX17.TextAlignment = (StringAlignment)1;
		((Control)lBLMaxTimeCanExit).Anchor = (AnchorStyles)9;
		((BaseItemControl)lBLMaxTimeCanExit).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lBLMaxTimeCanExit).ForeColor = Color.DarkRed;
		((Control)lBLMaxTimeCanExit).Location = new Point(116, 20);
		((Control)lBLMaxTimeCanExit).Name = "lBLMaxTimeCanExit";
		((Control)lBLMaxTimeCanExit).Size = new Size(199, 21);
		((Control)lBLMaxTimeCanExit).TabIndex = 0;
		((Control)lBLMaxTimeCanExit).Text = "[حداکثر مدت مجاز حضور :]";
		lBLMaxTimeCanExit.TextAlignment = (StringAlignment)2;
		toolTip1.SetToolTip((Control)(object)lBLMaxTimeCanExit, "حداکثر مدت مجاز حضور معادل جمع مدت حضور موظفی بعلاوه مدت مجاز بعد از اتمام شیفت می باشد");
		((Control)MlBLMaxTimeCanExit).Anchor = (AnchorStyles)9;
		((Control)MlBLMaxTimeCanExit).Location = new Point(75, 20);
		MlBLMaxTimeCanExit.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MlBLMaxTimeCanExit).Name = "MlBLMaxTimeCanExit";
		((Control)MlBLMaxTimeCanExit).Size = new Size(39, 21);
		((Control)MlBLMaxTimeCanExit).TabIndex = 1;
		((UpDownBase)MlBLMaxTimeCanExit).TextAlign = (HorizontalAlignment)2;
		((Control)HlBLMaxTimeCanExit).Anchor = (AnchorStyles)9;
		((Control)HlBLMaxTimeCanExit).Location = new Point(24, 20);
		HlBLMaxTimeCanExit.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)HlBLMaxTimeCanExit).Name = "HlBLMaxTimeCanExit";
		((Control)HlBLMaxTimeCanExit).Size = new Size(39, 21);
		((Control)HlBLMaxTimeCanExit).TabIndex = 3;
		((UpDownBase)HlBLMaxTimeCanExit).TextAlign = (HorizontalAlignment)2;
		((PanelControl)grpPanelPeakOvertime).CanvasColor = SystemColors.Control;
		((PanelControl)grpPanelPeakOvertime).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)MinutePeakOvertime);
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)HourPeakOvertime);
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)labelX18);
		grpPanelPeakOvertime.DisabledBackColor = Color.Empty;
		((Control)grpPanelPeakOvertime).Location = new Point(45, 174);
		((Control)grpPanelPeakOvertime).Name = "grpPanelPeakOvertime";
		((Control)grpPanelPeakOvertime).Size = new Size(237, 62);
		((PanelControl)grpPanelPeakOvertime).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)grpPanelPeakOvertime).Style.BackColorGradientAngle = 90;
		((PanelControl)grpPanelPeakOvertime).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)grpPanelPeakOvertime).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderBottomWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)grpPanelPeakOvertime).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderLeftWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderRightWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderTopWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.CornerDiameter = 4;
		((PanelControl)grpPanelPeakOvertime).Style.CornerType = (eCornerType)2;
		((PanelControl)grpPanelPeakOvertime).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)grpPanelPeakOvertime).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)grpPanelPeakOvertime).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)grpPanelPeakOvertime).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)grpPanelPeakOvertime).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)grpPanelPeakOvertime).TabIndex = 3;
		((Control)grpPanelPeakOvertime).Text = "[سقف اضافه کاری به مدت ]";
		((Control)MinutePeakOvertime).Anchor = (AnchorStyles)9;
		((Control)MinutePeakOvertime).Location = new Point(131, 7);
		MinutePeakOvertime.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MinutePeakOvertime).Name = "MinutePeakOvertime";
		((Control)MinutePeakOvertime).Size = new Size(71, 21);
		((Control)MinutePeakOvertime).TabIndex = 0;
		((UpDownBase)MinutePeakOvertime).TextAlign = (HorizontalAlignment)2;
		((Control)HourPeakOvertime).Anchor = (AnchorStyles)9;
		((Control)HourPeakOvertime).Location = new Point(33, 7);
		HourPeakOvertime.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)HourPeakOvertime).Name = "HourPeakOvertime";
		((Control)HourPeakOvertime).Size = new Size(71, 21);
		((Control)HourPeakOvertime).TabIndex = 2;
		((UpDownBase)HourPeakOvertime).TextAlign = (HorizontalAlignment)2;
		((Control)labelX18).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX18).Location = new Point(107, 7);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(21, 21);
		((Control)labelX18).TabIndex = 1;
		((Control)labelX18).Text = ":";
		labelX18.TextAlignment = (StringAlignment)1;
		((Control)ChbNextDay).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbNextDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbNextDay).Location = new Point(3, 72);
		((Control)ChbNextDay).Name = "ChbNextDay";
		((Control)ChbNextDay).Size = new Size(318, 21);
		((BaseItemControl)ChbNextDay).Style = (eDotNetBarStyle)9;
		((Control)ChbNextDay).TabIndex = 1;
		((Control)ChbNextDay).Text = "[انتقال از روز قبل]";
		ChbNextDay.CheckedChanged += ChbNextDay_CheckedChanged;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(225, 242);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(92, 23);
		((Control)labelX8).TabIndex = 4;
		((Control)labelX8).Text = "[شرح] :";
		((Control)TxtDesc).Anchor = (AnchorStyles)15;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(3, 266);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(321, 60);
		((Control)TxtDesc).TabIndex = 0;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)MinShenavar);
		((Control)groupPanel1).Controls.Add((Control)(object)HourShenavar);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX6);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(46, 4);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(237, 53);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 0;
		((Control)groupPanel1).Text = "[مدت حضور موظفی در شیفت شناور]";
		((Control)MinShenavar).Anchor = (AnchorStyles)9;
		((Control)MinShenavar).Location = new Point(135, 3);
		MinShenavar.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MinShenavar).Name = "MinShenavar";
		((Control)MinShenavar).Size = new Size(71, 21);
		((Control)MinShenavar).TabIndex = 0;
		((UpDownBase)MinShenavar).TextAlign = (HorizontalAlignment)2;
		((Control)HourShenavar).Anchor = (AnchorStyles)9;
		((Control)HourShenavar).Location = new Point(32, 3);
		HourShenavar.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)HourShenavar).Name = "HourShenavar";
		((Control)HourShenavar).Size = new Size(71, 21);
		((Control)HourShenavar).TabIndex = 2;
		((UpDownBase)HourShenavar).TextAlign = (HorizontalAlignment)2;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX6).Location = new Point(108, 3);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(21, 21);
		((Control)labelX6).TabIndex = 1;
		((Control)labelX6).Text = ":";
		labelX6.TextAlignment = (StringAlignment)1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(356, 412);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmShiftCodeingShenavar";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FrmShiftCodeing";
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)grpBoxNextDay).ResumeLayout(false);
		((ISupportInitialize)MlBLMaxTimeCanExit).EndInit();
		((ISupportInitialize)HlBLMaxTimeCanExit).EndInit();
		((Control)grpPanelPeakOvertime).ResumeLayout(false);
		((ISupportInitialize)MinutePeakOvertime).EndInit();
		((ISupportInitialize)HourPeakOvertime).EndInit();
		((Control)groupPanel1).ResumeLayout(false);
		((ISupportInitialize)MinShenavar).EndInit();
		((ISupportInitialize)HourShenavar).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
