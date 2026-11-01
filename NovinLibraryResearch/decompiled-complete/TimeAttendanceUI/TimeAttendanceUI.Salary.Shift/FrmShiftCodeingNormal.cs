using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary.FLRShift;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftCodeingNormal : FormExtention
{
	private ShiftCodeing _clsShiftCodeing;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	private GroupPanel groupPanel3;

	private LabelX lblTakhirKh;

	private LabelX TimeKhoroj2;

	private LabelX labelX16;

	private LabelX lblTajilKh;

	private NumericUpDown TakhirKh;

	private NumericUpDown TajilKh;

	private LabelX TimeKhoroj1;

	private CheckBoxX ChbNextDay;

	private NumericUpDown Min2;

	internal LabelX labelX2;

	private NumericUpDown Hour2;

	private LabelX labelX13;

	private GroupPanel groupPanel2;

	private LabelX labelX15;

	private NumericUpDown Hour1;

	private LabelX lblTajilV;

	private LabelX lblTakhirV;

	private NumericUpDown Min1;

	private LabelX timeVorod2;

	private LabelX TimeVorod1;

	internal LabelX labelX7;

	private NumericUpDown TakhirV;

	private LabelX labelX12;

	private NumericUpDown TajilV;

	private LabelX labelX8;

	private TextBoxX TxtDesc;

	internal LabelX labelX5;

	internal LabelX labelX10;

	internal LabelX labelX9;

	internal LabelX labelX1;

	internal LabelX labelX3;

	internal LabelX labelX4;

	private NumericUpDown MlBLMaxTimeCanExit;

	internal LabelX labelX11;

	private NumericUpDown HlBLMaxTimeCanExit;

	private LabelX lBLMaxTimeCanExit;

	private ToolTip toolTip1;

	private GroupPanel grpPanelPeakOvertime;

	private NumericUpDown MinutePeakOvertime;

	private NumericUpDown HourPeakOvertime;

	internal LabelX labelX18;

	internal ButtonX BtnHelp;

	private CheckBoxX ChBxShiftSeparation;

	private GroupBox grpBoxNextDay;

	private Panel panelPerform;

	internal ButtonX btnHurryDelayPerform;

	private CheckBoxX chbxHurryDelayPerform;

	private NumericUpDown txtLunchTime;

	private LabelX labelX19;

	private NumericUpDown txtHurStartLunch;

	private NumericUpDown txtMinStartLunch;

	internal LabelX labelX14;

	private LabelX labelX6;

	internal ButtonX btnHelpLunchTime;

	private CheckBoxX ChkVacationInLunch;

	private LabelX labelX17;

	private Panel panel1;

	internal ButtonX BtnJarimeInfo;

	private NumericUpDown JarimeV;

	private CheckBoxX ChkJarime;

	private CheckBoxX ChkCalcEnteghalInSrcDate;

	private ComboBoxEx CmbTypeTaradodInShift;

	private ComboItem Normal;

	private ComboItem ShiftWork1;

	private ComboItem ShiftWork2;

	private ComboItem ShiftWork3;

	private ComboItem NightWork;

	private CheckBoxX ChkAdamKasreKar;

	private LabelX LblTashvighType;

	internal ButtonX BtnSelectMazaya;

	private LabelX labelX31;

	private CheckBoxX ChkTashvighShift;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
			}
			return true;
		}
	}

	public DateTime Vorod => DateTime.Now.Date.AddHours((int)Hour1.Value).AddMinutes((int)Min1.Value);

	public DateTime Khoroj => DateTime.Now.Date.AddHours((int)Hour2.Value).AddMinutes((int)Min2.Value);

	public DateTime VorodStart => Vorod.AddMinutes((double)(-TajilV.Value));

	public DateTime VorodEnd => Vorod.AddMinutes((double)TakhirV.Value);

	public DateTime KhorojStart => Khoroj.AddMinutes((double)(-TajilKh.Value));

	public DateTime KhorojEnd => Khoroj.AddMinutes((double)TakhirKh.Value);

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmShiftCodeingNormal(string contextGuid)
		: base(contextGuid, true, true)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(0, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs");
		((Control)this).Text = "افزودن یک نوع تردد جدید";
	}

	public FrmShiftCodeingNormal(string contextGuid, short pId)
		: base(contextGuid, true, true)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected I4, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Invalid comparison between Unknown and I4
		InitializeComponent();
		((Control)this).Text = "ویرایش یک نوع تردد";
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(pId, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs");
			Hour1.Value = _clsShiftCodeing.H1;
			Hour2.Value = _clsShiftCodeing.H2;
			Min1.Value = _clsShiftCodeing.M1;
			Min2.Value = _clsShiftCodeing.M2;
			string text = Time.IntToTime(_clsShiftCodeing.MaxTimeCanExit);
			HlBLMaxTimeCanExit.Value = int.Parse(text.Substring(0, 2));
			MlBLMaxTimeCanExit.Value = int.Parse(text.Substring(3, 2));
			ChbNextDay.Checked = _clsShiftCodeing.NextDay;
			ChkCalcEnteghalInSrcDate.Checked = _clsShiftCodeing.CalcEnteghalInSrcDate;
			ChkTashvighShift.Checked = _clsShiftCodeing.HasTashvighiInShift;
			ChkAdamKasreKar.Checked = _clsShiftCodeing.HasTashvighIfNotKKInBeforeShift;
			((Control)LblTashvighType).Text = _clsShiftCodeing.MazayaIdForTashvigh.ToString();
			ChkAdamKasreKar.Checked = _clsShiftCodeing.HasTashvighIfNotKKInBeforeShift;
			ChkTashvighShift.Checked = _clsShiftCodeing.HasTashvighiInShift;
			((Control)LblTashvighType).Text = _clsShiftCodeing.MazayaIdForTashvigh.ToString();
			((ListControl)CmbTypeTaradodInShift).SelectedIndex = (int)_clsShiftCodeing.TypeTaradodInShift;
			((Control)ChkCalcEnteghalInSrcDate).Enabled = ChbNextDay.Checked;
			CheckBoxX chkTashvighShift = ChkTashvighShift;
			bool enabled = (((Control)ChkAdamKasreKar).Enabled = true);
			((Control)chkTashvighShift).Enabled = enabled;
			ChBxShiftSeparation.Checked = (int)_clsShiftCodeing.ShiftSeparation == 1;
			chbxHurryDelayPerform.Checked = _clsShiftCodeing.HurryDelayPerform;
			ChkJarime.Checked = _clsShiftCodeing.ChkJarime;
			((Control)txtLunchTime).Text = _clsShiftCodeing.LunchTime.ToString();
			((Control)txtMinStartLunch).Text = (_clsShiftCodeing.StartLunchTime % 60).ToString();
			((Control)txtHurStartLunch).Text = (_clsShiftCodeing.StartLunchTime / 60).ToString();
			TakhirV.Value = _clsShiftCodeing.TakhirV;
			JarimeV.Value = _clsShiftCodeing.JarimeV;
			TakhirKh.Value = _clsShiftCodeing.TakhirKh;
			TajilV.Value = _clsShiftCodeing.TajilV;
			TajilKh.Value = _clsShiftCodeing.TajilKh;
			if (_clsShiftCodeing.PeakOvertime >= 1440)
			{
				_clsShiftCodeing.PeakOvertime = 1439;
			}
			text = Time.IntToTime((int)_clsShiftCodeing.PeakOvertime);
			HourPeakOvertime.Value = int.Parse(text.Substring(0, 2));
			MinutePeakOvertime.Value = int.Parse(text.Substring(3, 2));
			((Control)TxtDesc).Text = _clsShiftCodeing.Description;
			ChangeTime();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 125);
		}
	}

	private async Task LoadDataAsync(short shiftCodeId, CancellationToken token)
	{
		try
		{
			if (shiftCodeId > 0)
			{
				_clsShiftCodeing = await ShiftCodeing.GetAsync(((CacheBaseForm)this).ContextGuid, shiftCodeId, token);
			}
			else
			{
				_clsShiftCodeing = new ShiftCodeing();
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		try
		{
			((FormExtention)this).OnLoad(e);
			InitAccItems();
			((FormExtention)this).OK_Click += new CrmEventHandler(AcceptEvent);
			((Control)BtnCancel).Click += base.BtnCancel_Click;
			SetTooltip();
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftAdvance || (int)AccessLevelClass.Instance.LevelHozor != 5)
			{
				CheckBoxX chbNextDay = ChbNextDay;
				NumericUpDown takhirV = TakhirV;
				NumericUpDown takhirKh = TakhirKh;
				NumericUpDown tajilV = TajilV;
				NumericUpDown tajilKh = TajilKh;
				CheckBoxX chkJarime = ChkJarime;
				CheckBoxX chkCalcEnteghalInSrcDate = ChkCalcEnteghalInSrcDate;
				CheckBoxX chkTashvighShift = ChkTashvighShift;
				bool flag2 = (((Control)ChkAdamKasreKar).Enabled = false);
				bool flag4 = (((Control)chkTashvighShift).Enabled = flag2);
				bool flag6 = (((Control)chkCalcEnteghalInSrcDate).Enabled = flag4);
				bool flag8 = (((Control)chkJarime).Enabled = flag6);
				bool flag10 = (((Control)tajilKh).Enabled = flag8);
				bool flag12 = (((Control)tajilV).Enabled = flag10);
				bool flag14 = (((Control)takhirKh).Enabled = flag12);
				bool enabled = (((Control)takhirV).Enabled = flag14);
				((Control)chbNextDay).Enabled = enabled;
			}
			((Control)CmbTypeTaradodInShift).Visible = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 175);
		}
	}

	private void InitAccItems()
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftAdvance)
			{
				((Control)lblTajilV).Enabled = false;
				((Control)TajilV).Enabled = false;
				((Control)lblTakhirV).Enabled = false;
				((Control)TakhirV).Enabled = false;
				((Control)JarimeV).Enabled = false;
				((Control)ChkJarime).Enabled = false;
				((Control)lblTajilKh).Enabled = false;
				((Control)TajilKh).Enabled = false;
				((Control)lblTakhirKh).Enabled = false;
				((Control)TakhirKh).Enabled = false;
				((Control)ChbNextDay).Enabled = false;
				((Control)ChkCalcEnteghalInSrcDate).Enabled = false;
				CheckBoxX chkAdamKasreKar = ChkAdamKasreKar;
				bool enabled = (((Control)ChkTashvighShift).Enabled = false);
				((Control)chkAdamKasreKar).Enabled = enabled;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitAccItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 205);
		}
	}

	private void Hour1_ValueChanged(object sender, EventArgs e)
	{
		ChangeTime();
	}

	private void ChangeTime()
	{
		try
		{
			((Control)TimeVorod1).Text = VorodStart.Hour + ":" + VorodStart.Minute;
			((Control)timeVorod2).Text = VorodEnd.Hour + ":" + VorodEnd.Minute;
			((Control)TimeKhoroj1).Text = KhorojStart.Hour + ":" + KhorojStart.Minute;
			((Control)TimeKhoroj2).Text = KhorojEnd.Hour + ":" + KhorojEnd.Minute;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChangeTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 225);
		}
	}

	private void ChbNextDay_CheckedChanged(object sender, EventArgs e)
	{
		((Control)ChkCalcEnteghalInSrcDate).Enabled = ChbNextDay.Checked;
	}

	private void SetTooltip()
	{
		try
		{
			string text = "ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.";
			toolTip1.SetToolTip((Control)(object)lBLMaxTimeCanExit, text);
			toolTip1.SetToolTip((Control)(object)HlBLMaxTimeCanExit, text);
			toolTip1.SetToolTip((Control)(object)MlBLMaxTimeCanExit, text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTooltip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 244);
		}
	}

	private void BtnHelp_Click(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, "", "توضیحات انتقال از روز قبل")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnHelp_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 255);
		}
	}

	private void GetData()
	{
		try
		{
			_clsShiftCodeing.H1 = short.Parse(Hour1.Value.ToString());
			_clsShiftCodeing.H2 = short.Parse(Hour2.Value.ToString());
			_clsShiftCodeing.M1 = short.Parse(Min1.Value.ToString());
			_clsShiftCodeing.M2 = short.Parse(Min2.Value.ToString());
			_clsShiftCodeing.TakhirV = short.Parse(TakhirV.Value.ToString());
			_clsShiftCodeing.JarimeV = short.Parse(JarimeV.Value.ToString());
			_clsShiftCodeing.TakhirKh = short.Parse(TakhirKh.Value.ToString());
			_clsShiftCodeing.TajilV = short.Parse(TajilV.Value.ToString());
			_clsShiftCodeing.TajilKh = short.Parse(TajilKh.Value.ToString());
			_clsShiftCodeing.Description = ((Control)TxtDesc).Text;
			_clsShiftCodeing.NextDay = ChbNextDay.Checked;
			_clsShiftCodeing.CalcEnteghalInSrcDate = ChkCalcEnteghalInSrcDate.Checked;
			_clsShiftCodeing.HasTashvighiInShift = ChkTashvighShift.Checked;
			_clsShiftCodeing.HasTashvighIfNotKKInBeforeShift = ChkAdamKasreKar.Checked;
			_clsShiftCodeing.MazayaIdForTashvigh = int.Parse(((Control)LblTashvighType).Text);
			_clsShiftCodeing.TypeTaradodInShift = (TypeTaradodInShiftEnum)(short)((ListControl)CmbTypeTaradodInShift).SelectedIndex;
			if (_clsShiftCodeing.H2 < 12 && _clsShiftCodeing.NextDay && HlBLMaxTimeCanExit.Value < (decimal)_clsShiftCodeing.H2)
			{
				HlBLMaxTimeCanExit.Value = _clsShiftCodeing.H2 + 1;
			}
			_clsShiftCodeing.MaxTimeCanExit = (int)HlBLMaxTimeCanExit.Value * 60 + (int)MlBLMaxTimeCanExit.Value;
			_clsShiftCodeing.PeakOvertime = (short)(HourPeakOvertime.Value * 60m + MinutePeakOvertime.Value);
			_clsShiftCodeing.TypeShift = (TypeShiftEnumType)1;
			_clsShiftCodeing.HurryDelayPerform = chbxHurryDelayPerform.Checked;
			_clsShiftCodeing.ChkJarime = ChkJarime.Checked;
			_clsShiftCodeing.LunchTime = Utilities.ParseToShort(((Control)txtLunchTime).Text, (short)0);
			_clsShiftCodeing.StartLunchTime = (short)(Utilities.ParseToShort(((Control)txtHurStartLunch).Text, (short)0) * 60 + Utilities.ParseToShort(((Control)txtMinStartLunch).Text, (short)0));
			_clsShiftCodeing.ShiftSeparation = (ShiftSeparationEnumType)(ChBxShiftSeparation.Checked ? 1 : 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 300);
		}
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AcceptEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 328);
		}
	}

	private void btnHurryDelayPerform_Click(object sender, EventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = "در حالت عادی کارکرد براساس زمان ورود و خروج محاسبه میشود ";
			text += "  و اگر این گزینه انتخاب شود کارکرد تعجیل و تاخیر در کارکرد اعمال میشود. ";
			text += " مثلا اگر شروع شیفت ساعت 7 باشد و تعجیل ورود داشته باشیم 10 دقیقه ";
			text += "  و شخص ساعت 06:52 ورود داشته باشد کارکرد از ساعت 7 محاسبه میشود.";
			text += "  دقت داشته باشید که اضافه کار و کسر کار طبق روال قبل محاسبه میشود.";
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, text, "محاسبه کارکرد (تعجیل و تاخیر)")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnHurryDelayPerform_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 348);
		}
	}

	private void btnJarime_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = "در صورتیکه تیک گزینه محاسبه جریمه فعال باشد \r\n";
			text += "و شخصی بیش از مدت زمان مشخص شده در ورود تاخیر داشته باشد برای وی آن روز جریمه محسوب خواهد شد\r\n ";
			text += "و بر اساس مبلغ جریمه تعیین شده در قراردادش، به ازاء هر روز مبلغ ذکر شده از حقوق وی کسر خواهد گردید";
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, text, "محاسبه جریمه")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnJarime_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeingNormal.cs", 363);
		}
	}

	private void btnHelpLunchTime_Click(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("تایم ناهار مدت زمانی است که پرسنل می توانند از زمان تعیین شده به مدت مشخص شده در قسمت تایم ناهار، به امورات شخصی از قبیل ناهار ");
		stringBuilder.AppendLine(" نماز و یا استراحت بپردازند ");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("بدیهی است این مدت زمان در کارکرد شخص لحاظ نمی گردد");
		stringBuilder.AppendLine("به عبارتی در محاسبه کارکرد، اضافه کار، کسرکار و ... بی تاثیر است");
		((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, stringBuilder.ToString(), "توضیحات تایم ناهار")).ShowDialog();
	}

	private void ChkJarime_CheckedChanged(object sender, EventArgs e)
	{
		((Control)JarimeV).Enabled = ChkJarime.Checked;
	}

	private void BtnSelectMazaya_Click(object sender, EventArgs e)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
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
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Expected O, but got Unknown
		//IL_104d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1057: Expected O, but got Unknown
		//IL_12ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b7: Expected O, but got Unknown
		//IL_1a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a33: Expected O, but got Unknown
		//IL_1ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee1: Expected O, but got Unknown
		//IL_2386: Unknown result type (might be due to invalid IL or missing references)
		//IL_2390: Expected O, but got Unknown
		//IL_2425: Unknown result type (might be due to invalid IL or missing references)
		//IL_242f: Expected O, but got Unknown
		//IL_24c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ce: Expected O, but got Unknown
		//IL_29ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b8: Expected O, but got Unknown
		//IL_2e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e7b: Expected O, but got Unknown
		//IL_32c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_32d0: Expected O, but got Unknown
		//IL_3364: Unknown result type (might be due to invalid IL or missing references)
		//IL_336e: Expected O, but got Unknown
		//IL_35b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35c3: Expected O, but got Unknown
		//IL_3645: Unknown result type (might be due to invalid IL or missing references)
		//IL_364f: Expected O, but got Unknown
		//IL_375e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3768: Expected O, but got Unknown
		//IL_38aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_38b4: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		CmbTypeTaradodInShift = new ComboBoxEx();
		Normal = new ComboItem();
		ShiftWork1 = new ComboItem();
		ShiftWork2 = new ComboItem();
		ShiftWork3 = new ComboItem();
		NightWork = new ComboItem();
		panel1 = new Panel();
		labelX17 = new LabelX();
		BtnJarimeInfo = new ButtonX();
		JarimeV = new NumericUpDown();
		ChkJarime = new CheckBoxX();
		ChkVacationInLunch = new CheckBoxX();
		btnHelpLunchTime = new ButtonX();
		txtHurStartLunch = new NumericUpDown();
		txtMinStartLunch = new NumericUpDown();
		labelX14 = new LabelX();
		txtLunchTime = new NumericUpDown();
		labelX6 = new LabelX();
		labelX19 = new LabelX();
		panelPerform = new Panel();
		btnHurryDelayPerform = new ButtonX();
		chbxHurryDelayPerform = new CheckBoxX();
		grpBoxNextDay = new GroupBox();
		ChkCalcEnteghalInSrcDate = new CheckBoxX();
		HlBLMaxTimeCanExit = new NumericUpDown();
		lBLMaxTimeCanExit = new LabelX();
		BtnHelp = new ButtonX();
		ChBxShiftSeparation = new CheckBoxX();
		labelX11 = new LabelX();
		MlBLMaxTimeCanExit = new NumericUpDown();
		ChbNextDay = new CheckBoxX();
		grpPanelPeakOvertime = new GroupPanel();
		MinutePeakOvertime = new NumericUpDown();
		HourPeakOvertime = new NumericUpDown();
		labelX18 = new LabelX();
		labelX8 = new LabelX();
		TxtDesc = new TextBoxX();
		groupPanel3 = new GroupPanel();
		labelX5 = new LabelX();
		labelX10 = new LabelX();
		labelX9 = new LabelX();
		lblTakhirKh = new LabelX();
		TimeKhoroj2 = new LabelX();
		labelX16 = new LabelX();
		lblTajilKh = new LabelX();
		TakhirKh = new NumericUpDown();
		TajilKh = new NumericUpDown();
		TimeKhoroj1 = new LabelX();
		Min2 = new NumericUpDown();
		labelX2 = new LabelX();
		Hour2 = new NumericUpDown();
		labelX13 = new LabelX();
		groupPanel2 = new GroupPanel();
		labelX1 = new LabelX();
		labelX15 = new LabelX();
		Hour1 = new NumericUpDown();
		lblTajilV = new LabelX();
		lblTakhirV = new LabelX();
		Min1 = new NumericUpDown();
		timeVorod2 = new LabelX();
		TimeVorod1 = new LabelX();
		labelX7 = new LabelX();
		labelX3 = new LabelX();
		TakhirV = new NumericUpDown();
		labelX12 = new LabelX();
		TajilV = new NumericUpDown();
		labelX4 = new LabelX();
		toolTip1 = new ToolTip(components);
		ChkAdamKasreKar = new CheckBoxX();
		LblTashvighType = new LabelX();
		BtnSelectMazaya = new ButtonX();
		labelX31 = new LabelX();
		ChkTashvighShift = new CheckBoxX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)JarimeV).BeginInit();
		((ISupportInitialize)txtHurStartLunch).BeginInit();
		((ISupportInitialize)txtMinStartLunch).BeginInit();
		((ISupportInitialize)txtLunchTime).BeginInit();
		((Control)panelPerform).SuspendLayout();
		((Control)grpBoxNextDay).SuspendLayout();
		((ISupportInitialize)HlBLMaxTimeCanExit).BeginInit();
		((ISupportInitialize)MlBLMaxTimeCanExit).BeginInit();
		((Control)grpPanelPeakOvertime).SuspendLayout();
		((ISupportInitialize)MinutePeakOvertime).BeginInit();
		((ISupportInitialize)HourPeakOvertime).BeginInit();
		((Control)groupPanel3).SuspendLayout();
		((ISupportInitialize)TakhirKh).BeginInit();
		((ISupportInitialize)TajilKh).BeginInit();
		((ISupportInitialize)Min2).BeginInit();
		((ISupportInitialize)Hour2).BeginInit();
		((Control)groupPanel2).SuspendLayout();
		((ISupportInitialize)Hour1).BeginInit();
		((ISupportInitialize)Min1).BeginInit();
		((ISupportInitialize)TakhirV).BeginInit();
		((ISupportInitialize)TajilV).BeginInit();
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
		((Control)panelEx1).Size = new Size(479, 511);
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
		((Control)BtnCancel).Location = new Point(12, 474);
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
		((Control)BtnAccept).Location = new Point(118, 474);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += AcceptEvent;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)ChkAdamKasreKar);
		((Control)groupPanel7).Controls.Add((Control)(object)LblTashvighType);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnSelectMazaya);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX31);
		((Control)groupPanel7).Controls.Add((Control)(object)ChkTashvighShift);
		((Control)groupPanel7).Controls.Add((Control)(object)CmbTypeTaradodInShift);
		((Control)groupPanel7).Controls.Add((Control)(object)panel1);
		((Control)groupPanel7).Controls.Add((Control)(object)ChkVacationInLunch);
		((Control)groupPanel7).Controls.Add((Control)(object)btnHelpLunchTime);
		((Control)groupPanel7).Controls.Add((Control)(object)txtHurStartLunch);
		((Control)groupPanel7).Controls.Add((Control)(object)txtMinStartLunch);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX14);
		((Control)groupPanel7).Controls.Add((Control)(object)txtLunchTime);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX6);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX19);
		((Control)groupPanel7).Controls.Add((Control)(object)panelPerform);
		((Control)groupPanel7).Controls.Add((Control)(object)grpBoxNextDay);
		((Control)groupPanel7).Controls.Add((Control)(object)grpPanelPeakOvertime);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtDesc);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel3);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel2);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(457, 456);
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
		((Control)CmbTypeTaradodInShift).Anchor = (AnchorStyles)13;
		((ListControl)CmbTypeTaradodInShift).DisplayMember = "Name";
		((ComboBox)CmbTypeTaradodInShift).DrawMode = (DrawMode)1;
		((ComboBox)CmbTypeTaradodInShift).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbTypeTaradodInShift).FormattingEnabled = true;
		((ComboBox)CmbTypeTaradodInShift).ItemHeight = 16;
		CmbTypeTaradodInShift.Items.AddRange(new object[5] { Normal, ShiftWork1, ShiftWork2, ShiftWork3, NightWork });
		((Control)CmbTypeTaradodInShift).Location = new Point(12, 367);
		((Control)CmbTypeTaradodInShift).Name = "CmbTypeTaradodInShift";
		((Control)CmbTypeTaradodInShift).Size = new Size(93, 22);
		CmbTypeTaradodInShift.Style = (eDotNetBarStyle)9;
		((Control)CmbTypeTaradodInShift).TabIndex = 23;
		((ListControl)CmbTypeTaradodInShift).ValueMember = "Id";
		Normal.Text = "عادی";
		Normal.Value = "0";
		ShiftWork1.Text = "نوبت کاری1";
		ShiftWork1.Value = "1";
		ShiftWork2.Text = "نوبت کاری2";
		ShiftWork2.Value = "2";
		ShiftWork3.Text = "نوبت کاری3";
		ShiftWork3.Value = "3";
		NightWork.Text = "شبکاری";
		NightWork.Value = "4";
		((Control)panel1).Controls.Add((Control)(object)labelX17);
		((Control)panel1).Controls.Add((Control)(object)BtnJarimeInfo);
		((Control)panel1).Controls.Add((Control)(object)JarimeV);
		((Control)panel1).Controls.Add((Control)(object)ChkJarime);
		((Control)panel1).Location = new Point(12, 157);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(429, 29);
		((Control)panel1).TabIndex = 3;
		((Control)labelX17).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).ForeColor = Color.Purple;
		((Control)labelX17).Location = new Point(93, 4);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(135, 21);
		((Control)labelX17).TabIndex = 14;
		((Control)labelX17).Text = "[دقیقه،  جریمه لحاظ گردد]";
		((Control)BtnJarimeInfo).AccessibleRole = (AccessibleRole)43;
		((Control)BtnJarimeInfo).Location = new Point(3, 3);
		((Control)BtnJarimeInfo).Name = "BtnJarimeInfo";
		((Control)BtnJarimeInfo).Size = new Size(21, 23);
		((PopupItemControl)BtnJarimeInfo).Style = (eDotNetBarStyle)9;
		((Control)BtnJarimeInfo).TabIndex = 1;
		((Control)BtnJarimeInfo).TabStop = false;
		((Control)BtnJarimeInfo).Text = "?";
		((Control)BtnJarimeInfo).Click += btnJarime_Click;
		((Control)JarimeV).Anchor = (AnchorStyles)9;
		((Control)JarimeV).Enabled = false;
		((Control)JarimeV).Location = new Point(234, 4);
		JarimeV.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)JarimeV).Name = "JarimeV";
		((Control)JarimeV).Size = new Size(49, 21);
		((Control)JarimeV).TabIndex = 21;
		((UpDownBase)JarimeV).TextAlign = (HorizontalAlignment)2;
		JarimeV.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		((Control)ChkJarime).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChkJarime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkJarime).Location = new Point(288, 3);
		((Control)ChkJarime).Name = "ChkJarime";
		((Control)ChkJarime).Size = new Size(137, 23);
		((Control)ChkJarime).TabIndex = 0;
		((Control)ChkJarime).Text = "[در صورت تاخیر ورود بیش از  ]";
		ChkJarime.TextColor = Color.Purple;
		ChkJarime.CheckedChanged += ChkJarime_CheckedChanged;
		((Control)ChkVacationInLunch).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChkVacationInLunch).BackgroundStyle.CornerType = (eCornerType)1;
		ChkVacationInLunch.Checked = true;
		ChkVacationInLunch.CheckState = (CheckState)1;
		ChkVacationInLunch.CheckValue = "Y";
		((Control)ChkVacationInLunch).Font = new Font("Tahoma", 7f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ChkVacationInLunch).Location = new Point(223, 341);
		((Control)ChkVacationInLunch).Name = "ChkVacationInLunch";
		((Control)ChkVacationInLunch).Size = new Size(213, 21);
		((BaseItemControl)ChkVacationInLunch).Style = (eDotNetBarStyle)9;
		((Control)ChkVacationInLunch).TabIndex = 20;
		((Control)ChkVacationInLunch).Text = "[مرخصی داخل بازه استراحت جزء مرخصی محاسبه شود]";
		((Control)btnHelpLunchTime).AccessibleRole = (AccessibleRole)43;
		btnHelpLunchTime.ColorTable = (eButtonColor)6;
		btnHelpLunchTime.ImageFixedSize = new Size(22, 21);
		((Control)btnHelpLunchTime).Location = new Point(229, 310);
		((Control)btnHelpLunchTime).Name = "btnHelpLunchTime";
		((Control)btnHelpLunchTime).Size = new Size(19, 21);
		((PopupItemControl)btnHelpLunchTime).Style = (eDotNetBarStyle)9;
		((Control)btnHelpLunchTime).TabIndex = 14;
		((Control)btnHelpLunchTime).TabStop = false;
		((Control)btnHelpLunchTime).Text = "?";
		((Control)btnHelpLunchTime).Click += btnHelpLunchTime_Click;
		((Control)txtHurStartLunch).Anchor = (AnchorStyles)9;
		((Control)txtHurStartLunch).Location = new Point(265, 311);
		txtHurStartLunch.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)txtHurStartLunch).Name = "txtHurStartLunch";
		((Control)txtHurStartLunch).Size = new Size(34, 21);
		((Control)txtHurStartLunch).TabIndex = 11;
		((UpDownBase)txtHurStartLunch).TextAlign = (HorizontalAlignment)2;
		((Control)txtMinStartLunch).Anchor = (AnchorStyles)9;
		((Control)txtMinStartLunch).Location = new Point(309, 311);
		txtMinStartLunch.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)txtMinStartLunch).Name = "txtMinStartLunch";
		((Control)txtMinStartLunch).Size = new Size(37, 21);
		((Control)txtMinStartLunch).TabIndex = 9;
		((UpDownBase)txtMinStartLunch).TextAlign = (HorizontalAlignment)2;
		((Control)labelX14).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX14).Location = new Point(300, 311);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(10, 21);
		((Control)labelX14).TabIndex = 10;
		((Control)labelX14).Text = ":";
		labelX14.TextAlignment = (StringAlignment)1;
		((Control)txtLunchTime).Anchor = (AnchorStyles)9;
		((Control)txtLunchTime).Location = new Point(229, 284);
		txtLunchTime.Maximum = new decimal(new int[4] { 240, 0, 0, 0 });
		((Control)txtLunchTime).Name = "txtLunchTime";
		((Control)txtLunchTime).Size = new Size(67, 21);
		((Control)txtLunchTime).TabIndex = 7;
		((UpDownBase)txtLunchTime).TextAlign = (HorizontalAlignment)2;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(350, 311);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(86, 21);
		((Control)labelX6).TabIndex = 8;
		((Control)labelX6).Text = "[ساعت شروع نهار :]";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX19).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Location = new Point(301, 282);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).Size = new Size(137, 21);
		((Control)labelX19).TabIndex = 6;
		((Control)labelX19).Text = "[مدت زمان استراحت(دقیقه)]";
		labelX19.TextAlignment = (StringAlignment)2;
		((Control)panelPerform).Controls.Add((Control)(object)btnHurryDelayPerform);
		((Control)panelPerform).Controls.Add((Control)(object)chbxHurryDelayPerform);
		((Control)panelPerform).Location = new Point(12, 189);
		((Control)panelPerform).Name = "panelPerform";
		((Control)panelPerform).Size = new Size(429, 29);
		((Control)panelPerform).TabIndex = 2;
		((Control)btnHurryDelayPerform).AccessibleRole = (AccessibleRole)43;
		((Control)btnHurryDelayPerform).Location = new Point(3, 3);
		((Control)btnHurryDelayPerform).Name = "btnHurryDelayPerform";
		((Control)btnHurryDelayPerform).Size = new Size(21, 23);
		((PopupItemControl)btnHurryDelayPerform).Style = (eDotNetBarStyle)9;
		((Control)btnHurryDelayPerform).TabIndex = 1;
		((Control)btnHurryDelayPerform).TabStop = false;
		((Control)btnHurryDelayPerform).Text = "?";
		((Control)btnHurryDelayPerform).Click += btnHurryDelayPerform_Click;
		((Control)chbxHurryDelayPerform).Anchor = (AnchorStyles)9;
		((BaseItemControl)chbxHurryDelayPerform).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbxHurryDelayPerform).Location = new Point(114, 3);
		((Control)chbxHurryDelayPerform).Name = "chbxHurryDelayPerform";
		((Control)chbxHurryDelayPerform).Size = new Size(312, 23);
		((BaseItemControl)chbxHurryDelayPerform).Style = (eDotNetBarStyle)9;
		((Control)chbxHurryDelayPerform).TabIndex = 0;
		((Control)chbxHurryDelayPerform).Text = "[کارکرد بر اساس تعجیل و تاخیر در زمان ورود و خروج محاسبه شود]";
		chbxHurryDelayPerform.TextColor = Color.Purple;
		((Control)grpBoxNextDay).Controls.Add((Control)(object)ChkCalcEnteghalInSrcDate);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)HlBLMaxTimeCanExit);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)lBLMaxTimeCanExit);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)BtnHelp);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)ChBxShiftSeparation);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)labelX11);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)MlBLMaxTimeCanExit);
		((Control)grpBoxNextDay).Controls.Add((Control)(object)ChbNextDay);
		((Control)grpBoxNextDay).Location = new Point(12, 263);
		((Control)grpBoxNextDay).Name = "grpBoxNextDay";
		((Control)grpBoxNextDay).Size = new Size(212, 99);
		((Control)grpBoxNextDay).TabIndex = 5;
		grpBoxNextDay.TabStop = false;
		((Control)ChkCalcEnteghalInSrcDate).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChkCalcEnteghalInSrcDate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkCalcEnteghalInSrcDate).Location = new Point(3, 62);
		((Control)ChkCalcEnteghalInSrcDate).Name = "ChkCalcEnteghalInSrcDate";
		((Control)ChkCalcEnteghalInSrcDate).Size = new Size(199, 21);
		((BaseItemControl)ChkCalcEnteghalInSrcDate).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcEnteghalInSrcDate).TabIndex = 6;
		((Control)ChkCalcEnteghalInSrcDate).Text = "[محاسبه تردد انتقال یافته در روز مبدا]";
		((Control)HlBLMaxTimeCanExit).Anchor = (AnchorStyles)9;
		((Control)HlBLMaxTimeCanExit).Location = new Point(21, 35);
		HlBLMaxTimeCanExit.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)HlBLMaxTimeCanExit).Name = "HlBLMaxTimeCanExit";
		((Control)HlBLMaxTimeCanExit).Size = new Size(30, 21);
		((Control)HlBLMaxTimeCanExit).TabIndex = 3;
		((UpDownBase)HlBLMaxTimeCanExit).TextAlign = (HorizontalAlignment)2;
		((Control)lBLMaxTimeCanExit).Anchor = (AnchorStyles)9;
		((BaseItemControl)lBLMaxTimeCanExit).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lBLMaxTimeCanExit).ForeColor = Color.DarkRed;
		((Control)lBLMaxTimeCanExit).Location = new Point(94, 35);
		((Control)lBLMaxTimeCanExit).Name = "lBLMaxTimeCanExit";
		((Control)lBLMaxTimeCanExit).Size = new Size(108, 21);
		((Control)lBLMaxTimeCanExit).TabIndex = 0;
		((Control)lBLMaxTimeCanExit).Text = "[حداکثر زمان خروج مجاز:]";
		lBLMaxTimeCanExit.TextAlignment = (StringAlignment)2;
		((Control)BtnHelp).AccessibleRole = (AccessibleRole)43;
		BtnHelp.ColorTable = (eButtonColor)6;
		((Control)BtnHelp).Location = new Point(1, 35);
		((Control)BtnHelp).Name = "BtnHelp";
		((Control)BtnHelp).Size = new Size(19, 21);
		((PopupItemControl)BtnHelp).Style = (eDotNetBarStyle)9;
		((Control)BtnHelp).TabIndex = 5;
		((Control)BtnHelp).TabStop = false;
		((Control)BtnHelp).Text = "?";
		((Control)BtnHelp).Click += BtnHelp_Click;
		((Control)ChBxShiftSeparation).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChBxShiftSeparation).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChBxShiftSeparation).Location = new Point(-9, 10);
		((Control)ChBxShiftSeparation).Name = "ChBxShiftSeparation";
		((Control)ChBxShiftSeparation).Size = new Size(116, 23);
		((BaseItemControl)ChBxShiftSeparation).Style = (eDotNetBarStyle)9;
		((Control)ChBxShiftSeparation).TabIndex = 4;
		((Control)ChBxShiftSeparation).Text = "[تردد ورود و خروج مجزا]";
		((Control)ChBxShiftSeparation).Visible = false;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX11).Location = new Point(51, 35);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(12, 21);
		((Control)labelX11).TabIndex = 2;
		((Control)labelX11).Text = ":";
		labelX11.TextAlignment = (StringAlignment)1;
		((Control)MlBLMaxTimeCanExit).Anchor = (AnchorStyles)9;
		((Control)MlBLMaxTimeCanExit).Location = new Point(61, 35);
		MlBLMaxTimeCanExit.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MlBLMaxTimeCanExit).Name = "MlBLMaxTimeCanExit";
		((Control)MlBLMaxTimeCanExit).Size = new Size(32, 21);
		((Control)MlBLMaxTimeCanExit).TabIndex = 1;
		((UpDownBase)MlBLMaxTimeCanExit).TextAlign = (HorizontalAlignment)2;
		((Control)ChbNextDay).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbNextDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbNextDay).Location = new Point(113, 12);
		((Control)ChbNextDay).Name = "ChbNextDay";
		((Control)ChbNextDay).Size = new Size(89, 21);
		((BaseItemControl)ChbNextDay).Style = (eDotNetBarStyle)9;
		((Control)ChbNextDay).TabIndex = 4;
		((Control)ChbNextDay).Text = "[انتقال از روز قبل]";
		ChbNextDay.CheckedChanged += ChbNextDay_CheckedChanged;
		((PanelControl)grpPanelPeakOvertime).CanvasColor = SystemColors.Control;
		((PanelControl)grpPanelPeakOvertime).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)MinutePeakOvertime);
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)HourPeakOvertime);
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)labelX18);
		grpPanelPeakOvertime.DisabledBackColor = Color.Empty;
		((Control)grpPanelPeakOvertime).Location = new Point(229, 227);
		((Control)grpPanelPeakOvertime).Name = "grpPanelPeakOvertime";
		((Control)grpPanelPeakOvertime).Size = new Size(212, 49);
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
		((Control)grpPanelPeakOvertime).Text = "[سقف اضافه کاری پایان شیفت تا ساعت]";
		((Control)MinutePeakOvertime).Anchor = (AnchorStyles)9;
		((Control)MinutePeakOvertime).Location = new Point(124, 3);
		MinutePeakOvertime.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MinutePeakOvertime).Name = "MinutePeakOvertime";
		((Control)MinutePeakOvertime).Size = new Size(71, 21);
		((Control)MinutePeakOvertime).TabIndex = 0;
		((UpDownBase)MinutePeakOvertime).TextAlign = (HorizontalAlignment)2;
		((Control)HourPeakOvertime).Anchor = (AnchorStyles)9;
		((Control)HourPeakOvertime).Location = new Point(31, 3);
		HourPeakOvertime.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)HourPeakOvertime).Name = "HourPeakOvertime";
		((Control)HourPeakOvertime).Size = new Size(71, 21);
		((Control)HourPeakOvertime).TabIndex = 2;
		((UpDownBase)HourPeakOvertime).TextAlign = (HorizontalAlignment)2;
		((Control)labelX18).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX18).Location = new Point(107, 3);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(21, 21);
		((Control)labelX18).TabIndex = 1;
		((Control)labelX18).Text = ":";
		labelX18.TextAlignment = (StringAlignment)1;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(151, 377);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(47, 23);
		((Control)labelX8).TabIndex = 12;
		((Control)labelX8).Text = "[شرح] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)15;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(12, 401);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(186, 46);
		((Control)TxtDesc).TabIndex = 13;
		((PanelControl)groupPanel3).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel3).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel3).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel3).Controls.Add((Control)(object)labelX10);
		((Control)groupPanel3).Controls.Add((Control)(object)labelX9);
		((Control)groupPanel3).Controls.Add((Control)(object)lblTakhirKh);
		((Control)groupPanel3).Controls.Add((Control)(object)TimeKhoroj2);
		((Control)groupPanel3).Controls.Add((Control)(object)labelX16);
		((Control)groupPanel3).Controls.Add((Control)(object)lblTajilKh);
		((Control)groupPanel3).Controls.Add((Control)(object)TakhirKh);
		((Control)groupPanel3).Controls.Add((Control)(object)TajilKh);
		((Control)groupPanel3).Controls.Add((Control)(object)TimeKhoroj1);
		((Control)groupPanel3).Controls.Add((Control)(object)Min2);
		((Control)groupPanel3).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel3).Controls.Add((Control)(object)Hour2);
		((Control)groupPanel3).Controls.Add((Control)(object)labelX13);
		groupPanel3.DisabledBackColor = Color.Empty;
		((Control)groupPanel3).Location = new Point(12, 23);
		((Control)groupPanel3).Name = "groupPanel3";
		((Control)groupPanel3).Size = new Size(212, 131);
		((PanelControl)groupPanel3).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel3).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel3).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel3).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel3).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel3).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel3).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel3).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel3).Style.CornerDiameter = 4;
		((PanelControl)groupPanel3).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel3).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel3).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel3).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel3).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel3).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel3).TabIndex = 1;
		((Control)groupPanel3).Text = "[ساعت پایان]";
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX5).Location = new Point(113, 30);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(12, 21);
		((Control)labelX5).TabIndex = 4;
		((Control)labelX5).Text = ":";
		labelX5.TextAlignment = (StringAlignment)1;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(113, 84);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(12, 21);
		((Control)labelX10).TabIndex = 7;
		((Control)labelX10).Text = ":";
		labelX10.TextAlignment = (StringAlignment)1;
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX9).Location = new Point(113, 57);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(12, 21);
		((Control)labelX9).TabIndex = 7;
		((Control)labelX9).Text = ":";
		labelX9.TextAlignment = (StringAlignment)1;
		((Control)lblTakhirKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTakhirKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTakhirKh).Location = new Point(130, 30);
		((Control)lblTakhirKh).Name = "lblTakhirKh";
		((Control)lblTakhirKh).Size = new Size(71, 21);
		((Control)lblTakhirKh).TabIndex = 3;
		((Control)lblTakhirKh).Text = "[تاخیر در خروج ]";
		lblTakhirKh.TextAlignment = (StringAlignment)2;
		((Control)TimeKhoroj2).Anchor = (AnchorStyles)9;
		((BaseItemControl)TimeKhoroj2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TimeKhoroj2).ForeColor = Color.DarkRed;
		((Control)TimeKhoroj2).Location = new Point(3, 84);
		((Control)TimeKhoroj2).Name = "TimeKhoroj2";
		((Control)TimeKhoroj2).Size = new Size(39, 21);
		((Control)TimeKhoroj2).TabIndex = 12;
		((Control)TimeKhoroj2).Text = "-------";
		((Control)labelX16).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).ForeColor = SystemColors.ControlText;
		((Control)labelX16).Location = new Point(46, 84);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(15, 21);
		((Control)labelX16).TabIndex = 11;
		((Control)labelX16).Text = "[تا] :";
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)lblTajilKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTajilKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTajilKh).Location = new Point(130, 57);
		((Control)lblTajilKh).Name = "lblTajilKh";
		((Control)lblTajilKh).Size = new Size(71, 21);
		((Control)lblTajilKh).TabIndex = 6;
		((Control)lblTajilKh).Text = "[تعجیل درخروج]";
		lblTajilKh.TextAlignment = (StringAlignment)2;
		((Control)TakhirKh).Anchor = (AnchorStyles)9;
		((Control)TakhirKh).Location = new Point(37, 30);
		TakhirKh.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TakhirKh).Name = "TakhirKh";
		((Control)TakhirKh).Size = new Size(71, 21);
		((Control)TakhirKh).TabIndex = 5;
		((UpDownBase)TakhirKh).TextAlign = (HorizontalAlignment)2;
		TakhirKh.ValueChanged += Hour1_ValueChanged;
		((Control)TajilKh).Anchor = (AnchorStyles)9;
		((Control)TajilKh).Location = new Point(37, 57);
		TajilKh.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TajilKh).Name = "TajilKh";
		((Control)TajilKh).Size = new Size(71, 21);
		((Control)TajilKh).TabIndex = 8;
		((UpDownBase)TajilKh).TextAlign = (HorizontalAlignment)2;
		TajilKh.ValueChanged += Hour1_ValueChanged;
		((Control)TimeKhoroj1).Anchor = (AnchorStyles)9;
		((BaseItemControl)TimeKhoroj1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TimeKhoroj1).ForeColor = Color.DarkRed;
		((Control)TimeKhoroj1).Location = new Point(67, 84);
		((Control)TimeKhoroj1).Name = "TimeKhoroj1";
		((Control)TimeKhoroj1).Size = new Size(41, 21);
		((Control)TimeKhoroj1).TabIndex = 10;
		((Control)TimeKhoroj1).Text = "------";
		((Control)Min2).Anchor = (AnchorStyles)9;
		((Control)Min2).Location = new Point(130, 3);
		Min2.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)Min2).Name = "Min2";
		((Control)Min2).Size = new Size(71, 21);
		((Control)Min2).TabIndex = 0;
		((UpDownBase)Min2).TextAlign = (HorizontalAlignment)2;
		Min2.ValueChanged += Hour1_ValueChanged;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(113, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(12, 21);
		((Control)labelX2).TabIndex = 1;
		((Control)labelX2).Text = ":";
		labelX2.TextAlignment = (StringAlignment)1;
		((Control)Hour2).Anchor = (AnchorStyles)9;
		((Control)Hour2).Location = new Point(37, 3);
		Hour2.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)Hour2).Name = "Hour2";
		((Control)Hour2).Size = new Size(71, 21);
		((Control)Hour2).TabIndex = 2;
		((UpDownBase)Hour2).TextAlign = (HorizontalAlignment)2;
		Hour2.ValueChanged += Hour1_ValueChanged;
		((Control)labelX13).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).ForeColor = SystemColors.ControlText;
		((Control)labelX13).Location = new Point(130, 84);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(71, 21);
		((Control)labelX13).TabIndex = 9;
		((Control)labelX13).Text = "[بازه خروج از]";
		labelX13.TextAlignment = (StringAlignment)2;
		((PanelControl)groupPanel2).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel2).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel2).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX15);
		((Control)groupPanel2).Controls.Add((Control)(object)Hour1);
		((Control)groupPanel2).Controls.Add((Control)(object)lblTajilV);
		((Control)groupPanel2).Controls.Add((Control)(object)lblTakhirV);
		((Control)groupPanel2).Controls.Add((Control)(object)Min1);
		((Control)groupPanel2).Controls.Add((Control)(object)timeVorod2);
		((Control)groupPanel2).Controls.Add((Control)(object)TimeVorod1);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel2).Controls.Add((Control)(object)TakhirV);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX12);
		((Control)groupPanel2).Controls.Add((Control)(object)TajilV);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX4);
		groupPanel2.DisabledBackColor = Color.Empty;
		((Control)groupPanel2).Location = new Point(229, 23);
		((Control)groupPanel2).Name = "groupPanel2";
		((Control)groupPanel2).Size = new Size(212, 131);
		((PanelControl)groupPanel2).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel2).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel2).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel2).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel2).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel2).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel2).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel2).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel2).Style.CornerDiameter = 4;
		((PanelControl)groupPanel2).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel2).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel2).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel2).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel2).TabIndex = 0;
		((Control)groupPanel2).Text = "[ساعت شروع]";
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(115, 84);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(10, 21);
		((Control)labelX1).TabIndex = 13;
		((Control)labelX1).Text = ":";
		labelX1.TextAlignment = (StringAlignment)1;
		((Control)labelX15).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).ForeColor = SystemColors.ControlText;
		((Control)labelX15).Location = new Point(53, 84);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(15, 23);
		((Control)labelX15).TabIndex = 11;
		((Control)labelX15).Text = "[تا] :";
		labelX15.TextAlignment = (StringAlignment)2;
		((Control)Hour1).Anchor = (AnchorStyles)9;
		((Control)Hour1).Location = new Point(39, 3);
		Hour1.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)Hour1).Name = "Hour1";
		((Control)Hour1).Size = new Size(71, 21);
		((Control)Hour1).TabIndex = 2;
		((UpDownBase)Hour1).TextAlign = (HorizontalAlignment)2;
		Hour1.ValueChanged += Hour1_ValueChanged;
		((Control)lblTajilV).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTajilV).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTajilV).Location = new Point(133, 57);
		((Control)lblTajilV).Name = "lblTajilV";
		((Control)lblTajilV).Size = new Size(66, 21);
		((Control)lblTajilV).TabIndex = 6;
		((Control)lblTajilV).Text = "[تعجیل در ورود ]";
		lblTajilV.TextAlignment = (StringAlignment)2;
		((Control)lblTakhirV).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTakhirV).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTakhirV).Location = new Point(133, 30);
		((Control)lblTakhirV).Name = "lblTakhirV";
		((Control)lblTakhirV).Size = new Size(63, 21);
		((Control)lblTakhirV).TabIndex = 3;
		((Control)lblTakhirV).Text = " [تاخیر در ورود]";
		lblTakhirV.TextAlignment = (StringAlignment)2;
		((Control)Min1).Anchor = (AnchorStyles)9;
		((Control)Min1).Location = new Point(133, 3);
		Min1.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)Min1).Name = "Min1";
		((Control)Min1).Size = new Size(71, 21);
		((Control)Min1).TabIndex = 0;
		((UpDownBase)Min1).TextAlign = (HorizontalAlignment)2;
		Min1.ValueChanged += Hour1_ValueChanged;
		((Control)timeVorod2).Anchor = (AnchorStyles)9;
		((BaseItemControl)timeVorod2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)timeVorod2).ForeColor = Color.DarkRed;
		((Control)timeVorod2).Location = new Point(3, 84);
		((Control)timeVorod2).Name = "timeVorod2";
		((Control)timeVorod2).Size = new Size(45, 23);
		((Control)timeVorod2).TabIndex = 12;
		((Control)timeVorod2).Text = "------";
		((Control)TimeVorod1).Anchor = (AnchorStyles)9;
		((BaseItemControl)TimeVorod1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TimeVorod1).ForeColor = Color.DarkRed;
		((Control)TimeVorod1).Location = new Point(74, 84);
		((Control)TimeVorod1).Name = "TimeVorod1";
		((Control)TimeVorod1).Size = new Size(36, 23);
		((Control)TimeVorod1).TabIndex = 10;
		((Control)TimeVorod1).Text = "-----";
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).Location = new Point(115, 3);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(10, 21);
		((Control)labelX7).TabIndex = 1;
		((Control)labelX7).Text = ":";
		labelX7.TextAlignment = (StringAlignment)1;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(115, 57);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(10, 21);
		((Control)labelX3).TabIndex = 7;
		((Control)labelX3).Text = ":";
		labelX3.TextAlignment = (StringAlignment)1;
		((Control)TakhirV).Anchor = (AnchorStyles)9;
		((Control)TakhirV).Location = new Point(39, 30);
		TakhirV.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TakhirV).Name = "TakhirV";
		((Control)TakhirV).Size = new Size(71, 21);
		((Control)TakhirV).TabIndex = 5;
		((UpDownBase)TakhirV).TextAlign = (HorizontalAlignment)2;
		TakhirV.ValueChanged += Hour1_ValueChanged;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).ForeColor = SystemColors.ControlText;
		((Control)labelX12).Location = new Point(133, 84);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(52, 23);
		((Control)labelX12).TabIndex = 9;
		((Control)labelX12).Text = "[بازه ورود از]";
		labelX12.TextAlignment = (StringAlignment)2;
		((Control)TajilV).Anchor = (AnchorStyles)9;
		((Control)TajilV).Location = new Point(39, 57);
		TajilV.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TajilV).Name = "TajilV";
		((Control)TajilV).Size = new Size(71, 21);
		((Control)TajilV).TabIndex = 8;
		((UpDownBase)TajilV).TextAlign = (HorizontalAlignment)2;
		TajilV.ValueChanged += Hour1_ValueChanged;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(115, 30);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(10, 21);
		((Control)labelX4).TabIndex = 4;
		((Control)labelX4).Text = ":";
		labelX4.TextAlignment = (StringAlignment)1;
		((BaseItemControl)ChkAdamKasreKar).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkAdamKasreKar).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)ChkAdamKasreKar).Location = new Point(223, 401);
		((Control)ChkAdamKasreKar).Name = "ChkAdamKasreKar";
		((Control)ChkAdamKasreKar).Size = new Size(194, 21);
		((BaseItemControl)ChkAdamKasreKar).Style = (eDotNetBarStyle)9;
		((Control)ChkAdamKasreKar).TabIndex = 45;
		((Control)ChkAdamKasreKar).Text = "نداشتن کسرکار شیفت قبل";
		((Control)LblTashvighType).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTashvighType).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTashvighType).Location = new Point(235, 378);
		((Control)LblTashvighType).Name = "LblTashvighType";
		((Control)LblTashvighType).Size = new Size(107, 21);
		((Control)LblTashvighType).TabIndex = 44;
		((Control)LblTashvighType).Text = "انتخاب نوع تشویقی";
		LblTashvighType.TextAlignment = (StringAlignment)2;
		((Control)BtnSelectMazaya).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSelectMazaya).Font = new Font("Tahoma", 15f, (FontStyle)1, (GraphicsUnit)2);
		((Control)BtnSelectMazaya).Location = new Point(209, 378);
		((Control)BtnSelectMazaya).Name = "BtnSelectMazaya";
		((Control)BtnSelectMazaya).Size = new Size(20, 21);
		((PopupItemControl)BtnSelectMazaya).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectMazaya).TabIndex = 43;
		((Control)BtnSelectMazaya).TabStop = false;
		((Control)BtnSelectMazaya).Text = "...";
		BtnSelectMazaya.TextColor = Color.Red;
		((Control)BtnSelectMazaya).Click += BtnSelectMazaya_Click;
		((Control)labelX31).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX31).Location = new Point(355, 378);
		((Control)labelX31).Name = "labelX31";
		((Control)labelX31).Size = new Size(72, 21);
		((Control)labelX31).TabIndex = 42;
		((Control)labelX31).Text = "نوع تشویق:";
		labelX31.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChkTashvighShift).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkTashvighShift).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)ChkTashvighShift).Location = new Point(216, 361);
		((Control)ChkTashvighShift).Name = "ChkTashvighShift";
		((Control)ChkTashvighShift).Size = new Size(220, 17);
		((BaseItemControl)ChkTashvighShift).Style = (eDotNetBarStyle)9;
		((Control)ChkTashvighShift).TabIndex = 41;
		((Control)ChkTashvighShift).Text = "مشوق حضور در شیفت";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(479, 511);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(495, 379);
		((Control)this).Name = "FrmShiftCodeingNormal";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فرم کدینگ شیفت]";
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)JarimeV).EndInit();
		((ISupportInitialize)txtHurStartLunch).EndInit();
		((ISupportInitialize)txtMinStartLunch).EndInit();
		((ISupportInitialize)txtLunchTime).EndInit();
		((Control)panelPerform).ResumeLayout(false);
		((Control)grpBoxNextDay).ResumeLayout(false);
		((ISupportInitialize)HlBLMaxTimeCanExit).EndInit();
		((ISupportInitialize)MlBLMaxTimeCanExit).EndInit();
		((Control)grpPanelPeakOvertime).ResumeLayout(false);
		((ISupportInitialize)MinutePeakOvertime).EndInit();
		((ISupportInitialize)HourPeakOvertime).EndInit();
		((Control)groupPanel3).ResumeLayout(false);
		((ISupportInitialize)TakhirKh).EndInit();
		((ISupportInitialize)TajilKh).EndInit();
		((ISupportInitialize)Min2).EndInit();
		((ISupportInitialize)Hour2).EndInit();
		((Control)groupPanel2).ResumeLayout(false);
		((ISupportInitialize)Hour1).EndInit();
		((ISupportInitialize)Min1).EndInit();
		((ISupportInitialize)TakhirV).EndInit();
		((ISupportInitialize)TajilV).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
