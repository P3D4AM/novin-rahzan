using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Settings;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmCalOverLowTime : BaseForm
{
	private GharardadNew _clsGharardad;

	private int _DaysofGharardad;

	private int _MonthOfGharardad;

	private decimal _SalaryBaseOneDay;

	private decimal _Sumkosor;

	private decimal _SumMazaya;

	private double _ZaribEmployerInsurance = 23.0;

	private double _ZaribEzafeDay = 1.4;

	private double _ZaribFriday = 1.8;

	private double _ZaribTatil = 1.4;

	private int _CountDayForSanavat = 30;

	private int _CountDayForPadash = 60;

	private int _CountNotHolidayOnGharardad;

	private int _CountVacationOnGharardad;

	private int _CountKhalesDayOnGharardad;

	private double _KarkardTimeOneDay;

	private decimal _MazayaMashmuleBime;

	private decimal _SumSalaryOnGharardad;

	private decimal _SumBenefit;

	private decimal _SumEmployerInsurance;

	private decimal _SumEmployerInsuranceMazaya;

	private decimal _SumSanavat;

	private decimal _SumPadash;

	private decimal _SumSalaryCol;

	private decimal _SumSalaryOneDay;

	private decimal _SumSalaryOneClock;

	private decimal _PriceKasreKar;

	private decimal _PriceEzafeKar;

	private decimal _PriceTatilKari;

	private decimal _PriceFriday;

	private CancellationTokenSource _loadDataToken;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnAccept;

	private LabelX labelX4;

	private LabelX labelX3;

	private LabelX labelX2;

	private TextBoxFa TxtPriceEzafe;

	private TextBoxFa txtCountNotHolidayOnGharardad;

	private TextBoxFa txtSalaryOnDay;

	private TextBoxFa txtSumSalaryOnGharardad;

	private LabelX labelX8;

	private LabelX labelX6;

	private LabelX labelX5;

	private LabelX labelX11;

	private TextBoxFa txtDaysofGharardad;

	private TextBoxFa txtSumKolSalary;

	private TextBoxFa txtMonthOfGharardad;

	private TextBoxFa txtSumKolMazayaOneMonth;

	private TextBoxFa txtSumMazayaOneMonth;

	private LabelX labelX15;

	private LabelX labelX14;

	private LabelX labelX18;

	private LabelX labelX17;

	private LabelX labelX20;

	private LabelX labelX16;

	private LabelX labelX13;

	private LabelX labelX12;

	private LabelX labelX10;

	private LabelX labelX9;

	private LabelX labelX27;

	private LabelX labelX34;

	private LabelX labelX33;

	private LabelX labelX41;

	private LabelX labelX38;

	private TextBoxFa txtDaysofGharardadForBime;

	private TextBoxFa txtSumEmployerInsurance;

	private TextBoxFa txtSalaryOnDayPadash;

	private LabelX labelX23;

	private LabelX labelX26;

	private LabelX labelX35;

	private LabelX labelX32;

	private LabelX labelX40;

	private LabelX labelX37;

	private LabelX labelX24;

	private LabelX labelX25;

	private LabelX labelX31;

	private LabelX labelX39;

	private LabelX labelX36;

	private GroupPanel groupPanel1;

	private TextBoxFa txtCountDayForPadash;

	private TextBoxFa txtCountDayForSanavat;

	private TextBoxFa txtSumPadash;

	private TextBoxFa txtSumSanavat;

	private TextBoxFa txtSalaryOnDayBime;

	private TextBoxFa txtSalaryOnDaySanavat;

	private LabelX labelX46;

	private LabelX labelX44;

	private LabelX labelX45;

	private LabelX labelX43;

	private LabelX labelX42;

	private TextBoxFa txtZaribEmployerInsurance;

	private LabelX labelX47;

	private TextBoxFa txtSumSalaryOnDay;

	private TextBoxFa txtSalaryOnHour;

	private TextBoxFa TxtPriceLow;

	private LabelX labelX19;

	private TextBoxFa txtCountKhalesDayOnGharardad;

	private LabelX labelX1;

	private TextBoxFa txtCountVacationOnGharardad;

	private TextBoxFa TxtPriceFriday;

	private LabelX labelX7;

	private TextBoxFa TxtPriceTatil;

	private LabelX labelX21;

	private LabelX labelX22;

	private LabelX labelX53;

	private LabelX labelX52;

	private TextBoxFa txtSumEmployerInsurance2;

	private LabelX labelX50;

	private LabelX labelX30;

	private LabelX labelX29;

	private LabelX labelX28;

	private TextBoxFa txtMonthofGharardadForBime2;

	private LabelX labelX51;

	private LabelX labelX54;

	private LabelX labelX48;

	private TextBoxFa txtZaribEmployerInsurance2;

	private LabelX labelX49;

	private TextBoxFa txtMazayaMashmuleBime;

	private ButtonX BtnCancel;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public FrmCalOverLowTime(string contextGuid, GharardadNew gharardad)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		_clsGharardad = gharardad;
		InitializeComponent();
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, FillPropertiesAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCalOverLowTime.cs");
			CalculateItems();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCalOverLowTime.cs", 75);
		}
	}

	private async Task FillPropertiesAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (_clsGharardad != null)
			{
				TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
				_DaysofGharardad = (_clsGharardad.DateFinishM - _clsGharardad.DateStartM).Days + 1;
				_MonthOfGharardad = _DaysofGharardad / 30;
				_Sumkosor = _clsGharardad.ListGharardadDet.Where((GharardadDetNew p) => p.TypeAccount != 1 && p.TypeAccount != -1).Sum((GharardadDetNew p) => p.nValue);
				_SumMazaya = _clsGharardad.ListGharardadDet.Where((GharardadDetNew p) => p.TypeAccount == 1).Sum((GharardadDetNew p) => p.nValue);
				_SalaryBaseOneDay = _clsGharardad.SalaryOneDay;
				_CountNotHolidayOnGharardad = await Holiday.CountNotHolidayOnGharardadAsync(((CacheBaseForm)this).ContextGuid, _clsGharardad.DateStartM, _clsGharardad.DateFinishM, token);
				_CountVacationOnGharardad = (int)_clsGharardad.TotalLeaveDaysInGharardad;
				_CountKhalesDayOnGharardad = _CountNotHolidayOnGharardad - _CountVacationOnGharardad;
				_ZaribEzafeDay = (double)(setting.ZaribEzafeDay + 100) / 100.0;
				_ZaribFriday = (double)(setting.ZaribFriday + 100) / 100.0;
				_ZaribTatil = (double)(setting.ZaribTatil + 100) / 100.0;
				_KarkardTimeOneDay = Math.Round((double)_clsGharardad.LeaveMinutes / 60.0, 2);
				_MazayaMashmuleBime = setting.LaborLaw.Bon + setting.LaborLaw.HaghMaskan;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillPropertiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCalOverLowTime.cs", 101);
		}
	}

	private void FillForm()
	{
		try
		{
			ChangeEventsState(release: true);
			((Control)txtSalaryOnDay).Text = _SalaryBaseOneDay.ToString();
			((Control)txtDaysofGharardad).Text = _DaysofGharardad.ToString();
			((Control)txtSumSalaryOnGharardad).Text = _SumSalaryOnGharardad.ToString();
			((Control)txtSumMazayaOneMonth).Text = _SumMazaya.ToString();
			((Control)txtMonthOfGharardad).Text = _MonthOfGharardad.ToString();
			((Control)txtSumKolMazayaOneMonth).Text = _SumBenefit.ToString();
			((Control)txtZaribEmployerInsurance).Text = _ZaribEmployerInsurance.ToString();
			((Control)txtZaribEmployerInsurance2).Text = _ZaribEmployerInsurance.ToString();
			((Control)txtSalaryOnDayBime).Text = _SalaryBaseOneDay.ToString();
			((Control)txtMazayaMashmuleBime).Text = _MazayaMashmuleBime.ToString();
			((Control)txtDaysofGharardadForBime).Text = _DaysofGharardad.ToString();
			((Control)txtMonthofGharardadForBime2).Text = _MonthOfGharardad.ToString();
			((Control)txtSumEmployerInsurance).Text = _SumEmployerInsurance.ToString();
			((Control)txtSumEmployerInsurance2).Text = _SumEmployerInsuranceMazaya.ToString();
			((Control)txtSalaryOnDaySanavat).Text = _SalaryBaseOneDay.ToString();
			((Control)txtSumSanavat).Text = _SumSanavat.ToString();
			((Control)txtCountDayForSanavat).Text = _CountDayForSanavat.ToString();
			((Control)txtSalaryOnDayPadash).Text = _SalaryBaseOneDay.ToString();
			((Control)txtCountDayForPadash).Text = _CountDayForPadash.ToString();
			((Control)txtSumPadash).Text = _SumPadash.ToString();
			((Control)txtSumKolSalary).Text = _SumSalaryCol.ToString();
			((Control)txtCountNotHolidayOnGharardad).Text = _CountNotHolidayOnGharardad.ToString();
			((Control)txtCountVacationOnGharardad).Text = _CountVacationOnGharardad.ToString();
			((Control)txtCountKhalesDayOnGharardad).Text = _CountKhalesDayOnGharardad.ToString();
			((Control)txtSumSalaryOnDay).Text = _SumSalaryOneDay.ToString("N0");
			((Control)txtSalaryOnHour).Text = _SumSalaryOneClock.ToString("N0");
			((Control)TxtPriceEzafe).Text = _PriceEzafeKar.ToString("N0");
			((Control)TxtPriceTatil).Text = _PriceTatilKari.ToString("N0");
			((Control)TxtPriceFriday).Text = _PriceFriday.ToString("N0");
			((Control)TxtPriceLow).Text = _PriceKasreKar.ToString("N0");
			ChangeEventsState(release: false);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCalOverLowTime.cs", 150);
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	public decimal GetPriceEzafe()
	{
		return _PriceEzafeKar;
	}

	public decimal GetPriceFriday()
	{
		return _PriceFriday;
	}

	public decimal GetPriceTatil()
	{
		return _PriceTatilKari;
	}

	public decimal GetPriceLow()
	{
		return _PriceKasreKar;
	}

	private void txtSalaryOnDay_TextChanged(object sender, EventArgs e)
	{
		decimal.TryParse(((Control)txtSalaryOnDay).Text, out _SalaryBaseOneDay);
		int.TryParse(((Control)txtDaysofGharardad).Text, out _DaysofGharardad);
		decimal.TryParse(((Control)txtSumMazayaOneMonth).Text, out _SumMazaya);
		int.TryParse(((Control)txtMonthOfGharardad).Text, out _MonthOfGharardad);
		double.TryParse(((Control)txtZaribEmployerInsurance).Text, out _ZaribEmployerInsurance);
		int.TryParse(((Control)txtCountDayForSanavat).Text, out _CountDayForSanavat);
		int.TryParse(((Control)txtCountDayForPadash).Text, out _CountDayForPadash);
		int.TryParse(((Control)txtCountNotHolidayOnGharardad).Text, out _CountNotHolidayOnGharardad);
		int.TryParse(((Control)txtCountVacationOnGharardad).Text, out _CountVacationOnGharardad);
		_CountKhalesDayOnGharardad = _CountNotHolidayOnGharardad - _CountVacationOnGharardad;
		CalculateItems();
		FillForm();
	}

	private void CalculateItems()
	{
		try
		{
			_SumSalaryOnGharardad = (decimal)_DaysofGharardad * _SalaryBaseOneDay;
			_SumBenefit = (decimal)_MonthOfGharardad * _SumMazaya;
			_SumEmployerInsurance = (decimal)(_ZaribEmployerInsurance / 100.0 * (double)(_SalaryBaseOneDay * (decimal)_DaysofGharardad));
			_SumEmployerInsuranceMazaya = (decimal)(_ZaribEmployerInsurance / 100.0 * (double)(_MazayaMashmuleBime * (decimal)_MonthOfGharardad));
			_SumSanavat = (decimal)_CountDayForSanavat * _SalaryBaseOneDay;
			_SumPadash = (decimal)_CountDayForPadash * _SalaryBaseOneDay;
			_SumSalaryCol = _SumSalaryOnGharardad + _SumBenefit + _SumEmployerInsurance + _SumEmployerInsuranceMazaya + _SumSanavat + _SumPadash;
			_SumSalaryOneDay = _SumSalaryCol / (decimal)_CountKhalesDayOnGharardad;
			_SumSalaryOneClock = _SumSalaryOneDay / (decimal)_KarkardTimeOneDay;
			_PriceEzafeKar = _SumSalaryOneClock * (decimal)_ZaribEzafeDay;
			_PriceKasreKar = _SumSalaryOneClock * (decimal)_ZaribEzafeDay;
			_PriceFriday = _SumSalaryOneClock * (decimal)_ZaribFriday;
			_PriceTatilKari = _SumSalaryOneClock * (decimal)_ZaribTatil;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalculateItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCalOverLowTime.cs", 202);
		}
	}

	private void ChangeEventsState(bool release)
	{
		try
		{
			if (release)
			{
				((Control)txtSalaryOnDay).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtDaysofGharardad).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtSumMazayaOneMonth).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtMonthOfGharardad).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtZaribEmployerInsurance).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtCountDayForSanavat).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtCountDayForPadash).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtCountNotHolidayOnGharardad).TextChanged -= txtSalaryOnDay_TextChanged;
				((Control)txtCountVacationOnGharardad).TextChanged -= txtSalaryOnDay_TextChanged;
			}
			else
			{
				((Control)txtSalaryOnDay).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtDaysofGharardad).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtSumMazayaOneMonth).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtMonthOfGharardad).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtZaribEmployerInsurance).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtCountDayForSanavat).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtCountDayForPadash).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtCountNotHolidayOnGharardad).TextChanged += txtSalaryOnDay_TextChanged;
				((Control)txtCountVacationOnGharardad).TextChanged += txtSalaryOnDay_TextChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChangeEventsState", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCalOverLowTime.cs", 238);
		}
	}

	private void FrmCalOverLowTime_Load(object sender, EventArgs e)
	{
		try
		{
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmCalOverLowTime_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCalOverLowTime.cs", 249);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
	}

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
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
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
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Expected O, but got Unknown
		//IL_0d1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d29: Expected O, but got Unknown
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcb: Expected O, but got Unknown
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Expected O, but got Unknown
		//IL_1043: Unknown result type (might be due to invalid IL or missing references)
		//IL_104d: Expected O, but got Unknown
		//IL_10a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b1: Expected O, but got Unknown
		//IL_1279: Unknown result type (might be due to invalid IL or missing references)
		//IL_1283: Expected O, but got Unknown
		//IL_12db: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e5: Expected O, but got Unknown
		//IL_177b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1785: Expected O, but got Unknown
		//IL_19a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ae: Expected O, but got Unknown
		//IL_1c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4b: Expected O, but got Unknown
		//IL_1e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e49: Expected O, but got Unknown
		//IL_2154: Unknown result type (might be due to invalid IL or missing references)
		//IL_215e: Expected O, but got Unknown
		//IL_2361: Unknown result type (might be due to invalid IL or missing references)
		//IL_236b: Expected O, but got Unknown
		//IL_24f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2502: Expected O, but got Unknown
		//IL_267f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2689: Expected O, but got Unknown
		//IL_282c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2836: Expected O, but got Unknown
		//IL_29ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d8: Expected O, but got Unknown
		//IL_2b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b54: Expected O, but got Unknown
		//IL_2bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be4: Expected O, but got Unknown
		//IL_2c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c65: Expected O, but got Unknown
		//IL_2cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce6: Expected O, but got Unknown
		//IL_2d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d76: Expected O, but got Unknown
		//IL_2dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e06: Expected O, but got Unknown
		//IL_2e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e95: Expected O, but got Unknown
		//IL_2f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f28: Expected O, but got Unknown
		//IL_30d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_30dd: Expected O, but got Unknown
		//IL_3285: Unknown result type (might be due to invalid IL or missing references)
		//IL_328f: Expected O, but got Unknown
		//IL_3434: Unknown result type (might be due to invalid IL or missing references)
		//IL_343e: Expected O, but got Unknown
		//IL_35df: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e9: Expected O, but got Unknown
		//IL_3767: Unknown result type (might be due to invalid IL or missing references)
		//IL_3771: Expected O, but got Unknown
		//IL_3918: Unknown result type (might be due to invalid IL or missing references)
		//IL_3922: Expected O, but got Unknown
		//IL_3aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ab9: Expected O, but got Unknown
		//IL_3c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c50: Expected O, but got Unknown
		//IL_3ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de7: Expected O, but got Unknown
		//IL_3f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f7f: Expected O, but got Unknown
		//IL_4120: Unknown result type (might be due to invalid IL or missing references)
		//IL_412a: Expected O, but got Unknown
		//IL_42b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c1: Expected O, but got Unknown
		//IL_4451: Unknown result type (might be due to invalid IL or missing references)
		//IL_445b: Expected O, but got Unknown
		//IL_45e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_45f2: Expected O, but got Unknown
		//IL_477e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4788: Expected O, but got Unknown
		//IL_4909: Unknown result type (might be due to invalid IL or missing references)
		//IL_4913: Expected O, but got Unknown
		//IL_4aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ac4: Expected O, but got Unknown
		//IL_4b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b53: Expected O, but got Unknown
		//IL_4c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c40: Expected O, but got Unknown
		//IL_4d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d2b: Expected O, but got Unknown
		//IL_4dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dcd: Expected O, but got Unknown
		//IL_4ee0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eea: Expected O, but got Unknown
		//IL_4ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5004: Expected O, but got Unknown
		//IL_509c: Unknown result type (might be due to invalid IL or missing references)
		//IL_50a6: Expected O, but got Unknown
		//IL_5141: Unknown result type (might be due to invalid IL or missing references)
		//IL_514b: Expected O, but got Unknown
		//IL_51e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_51ed: Expected O, but got Unknown
		//IL_5285: Unknown result type (might be due to invalid IL or missing references)
		//IL_528f: Expected O, but got Unknown
		//IL_5327: Unknown result type (might be due to invalid IL or missing references)
		//IL_5331: Expected O, but got Unknown
		//IL_53c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_53d3: Expected O, but got Unknown
		//IL_546b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5475: Expected O, but got Unknown
		//IL_550d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5517: Expected O, but got Unknown
		//IL_55af: Unknown result type (might be due to invalid IL or missing references)
		//IL_55b9: Expected O, but got Unknown
		//IL_5651: Unknown result type (might be due to invalid IL or missing references)
		//IL_565b: Expected O, but got Unknown
		//IL_56f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_56fc: Expected O, but got Unknown
		//IL_5797: Unknown result type (might be due to invalid IL or missing references)
		//IL_57a1: Expected O, but got Unknown
		//IL_5839: Unknown result type (might be due to invalid IL or missing references)
		//IL_5843: Expected O, but got Unknown
		//IL_58da: Unknown result type (might be due to invalid IL or missing references)
		//IL_58e4: Expected O, but got Unknown
		//IL_597c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5986: Expected O, but got Unknown
		//IL_5bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c07: Expected O, but got Unknown
		//IL_5c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c97: Expected O, but got Unknown
		//IL_5d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d27: Expected O, but got Unknown
		//IL_5dad: Unknown result type (might be due to invalid IL or missing references)
		//IL_5db7: Expected O, but got Unknown
		//IL_5e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e46: Expected O, but got Unknown
		//IL_5ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ed9: Expected O, but got Unknown
		//IL_5f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fa3: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		txtMonthofGharardadForBime2 = new TextBoxFa();
		labelX51 = new LabelX();
		labelX54 = new LabelX();
		labelX48 = new LabelX();
		txtZaribEmployerInsurance2 = new TextBoxFa();
		labelX49 = new LabelX();
		txtMazayaMashmuleBime = new TextBoxFa();
		labelX22 = new LabelX();
		groupPanel1 = new GroupPanel();
		TxtPriceTatil = new TextBoxFa();
		labelX21 = new LabelX();
		TxtPriceFriday = new TextBoxFa();
		labelX7 = new LabelX();
		labelX19 = new LabelX();
		txtCountKhalesDayOnGharardad = new TextBoxFa();
		labelX1 = new LabelX();
		txtCountVacationOnGharardad = new TextBoxFa();
		labelX12 = new LabelX();
		labelX13 = new LabelX();
		labelX14 = new LabelX();
		txtSumSalaryOnDay = new TextBoxFa();
		labelX15 = new LabelX();
		txtSumKolSalary = new TextBoxFa();
		txtSalaryOnHour = new TextBoxFa();
		txtCountNotHolidayOnGharardad = new TextBoxFa();
		TxtPriceLow = new TextBoxFa();
		TxtPriceEzafe = new TextBoxFa();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX27 = new LabelX();
		labelX53 = new LabelX();
		labelX34 = new LabelX();
		labelX52 = new LabelX();
		labelX33 = new LabelX();
		labelX41 = new LabelX();
		labelX38 = new LabelX();
		labelX11 = new LabelX();
		txtDaysofGharardad = new TextBoxFa();
		txtCountDayForPadash = new TextBoxFa();
		txtCountDayForSanavat = new TextBoxFa();
		txtDaysofGharardadForBime = new TextBoxFa();
		txtMonthOfGharardad = new TextBoxFa();
		txtSumPadash = new TextBoxFa();
		txtSumSanavat = new TextBoxFa();
		txtSumEmployerInsurance2 = new TextBoxFa();
		txtSumEmployerInsurance = new TextBoxFa();
		txtZaribEmployerInsurance = new TextBoxFa();
		txtSalaryOnDayBime = new TextBoxFa();
		txtSalaryOnDaySanavat = new TextBoxFa();
		txtSalaryOnDayPadash = new TextBoxFa();
		txtSumKolMazayaOneMonth = new TextBoxFa();
		txtSumSalaryOnGharardad = new TextBoxFa();
		txtSumMazayaOneMonth = new TextBoxFa();
		txtSalaryOnDay = new TextBoxFa();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		labelX47 = new LabelX();
		labelX46 = new LabelX();
		labelX50 = new LabelX();
		labelX44 = new LabelX();
		labelX8 = new LabelX();
		labelX23 = new LabelX();
		labelX45 = new LabelX();
		labelX43 = new LabelX();
		labelX18 = new LabelX();
		labelX42 = new LabelX();
		labelX30 = new LabelX();
		labelX26 = new LabelX();
		labelX29 = new LabelX();
		labelX35 = new LabelX();
		labelX32 = new LabelX();
		labelX40 = new LabelX();
		labelX37 = new LabelX();
		labelX24 = new LabelX();
		labelX17 = new LabelX();
		labelX20 = new LabelX();
		labelX16 = new LabelX();
		labelX10 = new LabelX();
		labelX9 = new LabelX();
		labelX6 = new LabelX();
		labelX5 = new LabelX();
		labelX28 = new LabelX();
		labelX25 = new LabelX();
		labelX31 = new LabelX();
		labelX39 = new LabelX();
		labelX36 = new LabelX();
		labelX2 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)TxtPriceTatil);
		((Control)panelEx1).Controls.Add((Control)(object)txtMonthofGharardadForBime2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX21);
		((Control)panelEx1).Controls.Add((Control)(object)labelX51);
		((Control)panelEx1).Controls.Add((Control)(object)TxtPriceFriday);
		((Control)panelEx1).Controls.Add((Control)(object)labelX54);
		((Control)panelEx1).Controls.Add((Control)(object)labelX7);
		((Control)panelEx1).Controls.Add((Control)(object)labelX48);
		((Control)panelEx1).Controls.Add((Control)(object)txtZaribEmployerInsurance2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX49);
		((Control)panelEx1).Controls.Add((Control)(object)txtMazayaMashmuleBime);
		((Control)panelEx1).Controls.Add((Control)(object)labelX22);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)labelX27);
		((Control)panelEx1).Controls.Add((Control)(object)labelX53);
		((Control)panelEx1).Controls.Add((Control)(object)labelX34);
		((Control)panelEx1).Controls.Add((Control)(object)labelX52);
		((Control)panelEx1).Controls.Add((Control)(object)labelX33);
		((Control)panelEx1).Controls.Add((Control)(object)labelX41);
		((Control)panelEx1).Controls.Add((Control)(object)labelX38);
		((Control)panelEx1).Controls.Add((Control)(object)TxtPriceLow);
		((Control)panelEx1).Controls.Add((Control)(object)labelX11);
		((Control)panelEx1).Controls.Add((Control)(object)TxtPriceEzafe);
		((Control)panelEx1).Controls.Add((Control)(object)txtDaysofGharardad);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)txtCountDayForPadash);
		((Control)panelEx1).Controls.Add((Control)(object)txtCountDayForSanavat);
		((Control)panelEx1).Controls.Add((Control)(object)txtDaysofGharardadForBime);
		((Control)panelEx1).Controls.Add((Control)(object)txtMonthOfGharardad);
		((Control)panelEx1).Controls.Add((Control)(object)txtSumPadash);
		((Control)panelEx1).Controls.Add((Control)(object)txtSumSanavat);
		((Control)panelEx1).Controls.Add((Control)(object)txtSumEmployerInsurance2);
		((Control)panelEx1).Controls.Add((Control)(object)txtSumEmployerInsurance);
		((Control)panelEx1).Controls.Add((Control)(object)txtZaribEmployerInsurance);
		((Control)panelEx1).Controls.Add((Control)(object)txtSalaryOnDayBime);
		((Control)panelEx1).Controls.Add((Control)(object)txtSalaryOnDaySanavat);
		((Control)panelEx1).Controls.Add((Control)(object)txtSalaryOnDayPadash);
		((Control)panelEx1).Controls.Add((Control)(object)txtSumKolMazayaOneMonth);
		((Control)panelEx1).Controls.Add((Control)(object)txtSumSalaryOnGharardad);
		((Control)panelEx1).Controls.Add((Control)(object)txtSumMazayaOneMonth);
		((Control)panelEx1).Controls.Add((Control)(object)txtSalaryOnDay);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX47);
		((Control)panelEx1).Controls.Add((Control)(object)labelX46);
		((Control)panelEx1).Controls.Add((Control)(object)labelX50);
		((Control)panelEx1).Controls.Add((Control)(object)labelX44);
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Controls.Add((Control)(object)labelX23);
		((Control)panelEx1).Controls.Add((Control)(object)labelX45);
		((Control)panelEx1).Controls.Add((Control)(object)labelX43);
		((Control)panelEx1).Controls.Add((Control)(object)labelX18);
		((Control)panelEx1).Controls.Add((Control)(object)labelX42);
		((Control)panelEx1).Controls.Add((Control)(object)labelX30);
		((Control)panelEx1).Controls.Add((Control)(object)labelX26);
		((Control)panelEx1).Controls.Add((Control)(object)labelX29);
		((Control)panelEx1).Controls.Add((Control)(object)labelX35);
		((Control)panelEx1).Controls.Add((Control)(object)labelX32);
		((Control)panelEx1).Controls.Add((Control)(object)labelX40);
		((Control)panelEx1).Controls.Add((Control)(object)labelX37);
		((Control)panelEx1).Controls.Add((Control)(object)labelX24);
		((Control)panelEx1).Controls.Add((Control)(object)labelX17);
		((Control)panelEx1).Controls.Add((Control)(object)labelX20);
		((Control)panelEx1).Controls.Add((Control)(object)labelX16);
		((Control)panelEx1).Controls.Add((Control)(object)labelX10);
		((Control)panelEx1).Controls.Add((Control)(object)labelX9);
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)labelX28);
		((Control)panelEx1).Controls.Add((Control)(object)labelX25);
		((Control)panelEx1).Controls.Add((Control)(object)labelX31);
		((Control)panelEx1).Controls.Add((Control)(object)labelX39);
		((Control)panelEx1).Controls.Add((Control)(object)labelX36);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1027, 493);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		txtMonthofGharardadForBime2.AllowEmpty = true;
		txtMonthofGharardadForBime2.BackColorTextBoxWhenFocus = Color.Yellow;
		txtMonthofGharardadForBime2.BackColorTextBoxWhenLostFocus = Color.White;
		txtMonthofGharardadForBime2.BackcolorTooltip = Color.Red;
		txtMonthofGharardadForBime2.CurrencyFormat = "";
		((Control)txtMonthofGharardadForBime2).ForeColor = Color.Red;
		txtMonthofGharardadForBime2.ForeColorTooltip = Color.Black;
		txtMonthofGharardadForBime2.GetColoresFromSettingDLL = true;
		txtMonthofGharardadForBime2.HandleKeydown = true;
		((Control)txtMonthofGharardadForBime2).Location = new Point(549, 94);
		((TextBoxBase)txtMonthofGharardadForBime2).MaxLength = 19;
		((Control)txtMonthofGharardadForBime2).Name = "txtMonthofGharardadForBime2";
		((TextBoxBase)txtMonthofGharardadForBime2).ReadOnly = true;
		txtMonthofGharardadForBime2.SendShiftTabAfterDown = true;
		txtMonthofGharardadForBime2.SendShiftTabAfterEnter = true;
		txtMonthofGharardadForBime2.SendTabAfterEnter = true;
		txtMonthofGharardadForBime2.SendTabAfterUP = true;
		txtMonthofGharardadForBime2.ShowHorofi = (ENShowHorofi)3;
		txtMonthofGharardadForBime2.ShowToolTipText = true;
		((Control)txtMonthofGharardadForBime2).Size = new Size(51, 21);
		((Control)txtMonthofGharardadForBime2).TabIndex = 67;
		((Control)txtMonthofGharardadForBime2).Text = "0";
		txtMonthofGharardadForBime2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtMonthofGharardadForBime2.ToolTipPostFix = "";
		txtMonthofGharardadForBime2.TootTipDelay = 1000;
		txtMonthofGharardadForBime2.Value = new decimal(new int[4]);
		((BaseItemControl)labelX51).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX51).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX51).ForeColor = Color.Maroon;
		((Control)labelX51).Location = new Point(529, 94);
		((Control)labelX51).Name = "labelX51";
		((Control)labelX51).Size = new Size(14, 21);
		((Control)labelX51).TabIndex = 68;
		((Control)labelX51).Text = "*";
		labelX51.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX54).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX54).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX54).ForeColor = Color.Maroon;
		((Control)labelX54).Location = new Point(606, 94);
		((Control)labelX54).Name = "labelX54";
		((Control)labelX54).Size = new Size(14, 21);
		((Control)labelX54).TabIndex = 66;
		((Control)labelX54).Text = "=";
		labelX54.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX48).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX48).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)labelX48).ForeColor = Color.Black;
		((Control)labelX48).Location = new Point(391, 94);
		((Control)labelX48).Name = "labelX48";
		((Control)labelX48).Size = new Size(14, 21);
		((Control)labelX48).TabIndex = 65;
		((Control)labelX48).Text = "%";
		labelX48.TextAlignment = (StringAlignment)2;
		txtZaribEmployerInsurance2.AllowEmpty = true;
		((Control)txtZaribEmployerInsurance2).BackColor = SystemColors.Window;
		txtZaribEmployerInsurance2.BackColorTextBoxWhenFocus = Color.Yellow;
		txtZaribEmployerInsurance2.BackColorTextBoxWhenLostFocus = Color.White;
		txtZaribEmployerInsurance2.BackcolorTooltip = Color.Red;
		txtZaribEmployerInsurance2.CurrencyFormat = "";
		((Control)txtZaribEmployerInsurance2).ForeColor = Color.Red;
		txtZaribEmployerInsurance2.ForeColorTooltip = Color.Black;
		txtZaribEmployerInsurance2.GetColoresFromSettingDLL = true;
		txtZaribEmployerInsurance2.HandleKeydown = true;
		((Control)txtZaribEmployerInsurance2).Location = new Point(350, 94);
		((TextBoxBase)txtZaribEmployerInsurance2).MaxLength = 19;
		((Control)txtZaribEmployerInsurance2).Name = "txtZaribEmployerInsurance2";
		((TextBoxBase)txtZaribEmployerInsurance2).ReadOnly = true;
		txtZaribEmployerInsurance2.SendShiftTabAfterDown = true;
		txtZaribEmployerInsurance2.SendShiftTabAfterEnter = true;
		txtZaribEmployerInsurance2.SendTabAfterEnter = true;
		txtZaribEmployerInsurance2.SendTabAfterUP = true;
		txtZaribEmployerInsurance2.ShowHorofi = (ENShowHorofi)3;
		txtZaribEmployerInsurance2.ShowToolTipText = true;
		((Control)txtZaribEmployerInsurance2).Size = new Size(40, 21);
		((Control)txtZaribEmployerInsurance2).TabIndex = 64;
		((Control)txtZaribEmployerInsurance2).Text = "0";
		txtZaribEmployerInsurance2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtZaribEmployerInsurance2.ToolTipPostFix = "";
		txtZaribEmployerInsurance2.TootTipDelay = 1000;
		txtZaribEmployerInsurance2.Value = new decimal(new int[4]);
		((BaseItemControl)labelX49).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX49).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX49).ForeColor = Color.Maroon;
		((Control)labelX49).Location = new Point(417, 94);
		((Control)labelX49).Name = "labelX49";
		((Control)labelX49).Size = new Size(14, 21);
		((Control)labelX49).TabIndex = 63;
		((Control)labelX49).Text = "*";
		labelX49.TextAlignment = (StringAlignment)2;
		txtMazayaMashmuleBime.AllowEmpty = true;
		txtMazayaMashmuleBime.BackColorTextBoxWhenFocus = Color.Yellow;
		txtMazayaMashmuleBime.BackColorTextBoxWhenLostFocus = Color.White;
		txtMazayaMashmuleBime.BackcolorTooltip = Color.Red;
		txtMazayaMashmuleBime.CurrencyFormat = "";
		((Control)txtMazayaMashmuleBime).ForeColor = Color.Red;
		txtMazayaMashmuleBime.ForeColorTooltip = Color.Black;
		txtMazayaMashmuleBime.GetColoresFromSettingDLL = true;
		txtMazayaMashmuleBime.HandleKeydown = true;
		((Control)txtMazayaMashmuleBime).Location = new Point(431, 94);
		((TextBoxBase)txtMazayaMashmuleBime).MaxLength = 19;
		((Control)txtMazayaMashmuleBime).Name = "txtMazayaMashmuleBime";
		((TextBoxBase)txtMazayaMashmuleBime).ReadOnly = true;
		txtMazayaMashmuleBime.SendShiftTabAfterDown = true;
		txtMazayaMashmuleBime.SendShiftTabAfterEnter = true;
		txtMazayaMashmuleBime.SendTabAfterEnter = true;
		txtMazayaMashmuleBime.SendTabAfterUP = true;
		txtMazayaMashmuleBime.ShowHorofi = (ENShowHorofi)3;
		txtMazayaMashmuleBime.ShowToolTipText = true;
		((Control)txtMazayaMashmuleBime).Size = new Size(92, 21);
		((Control)txtMazayaMashmuleBime).TabIndex = 62;
		((Control)txtMazayaMashmuleBime).Text = "0";
		txtMazayaMashmuleBime.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtMazayaMashmuleBime.ToolTipPostFix = "";
		txtMazayaMashmuleBime.TootTipDelay = 1000;
		txtMazayaMashmuleBime.Value = new decimal(new int[4]);
		((BaseItemControl)labelX22).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX22).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)labelX22).ForeColor = Color.Black;
		((Control)labelX22).Location = new Point(391, 67);
		((Control)labelX22).Name = "labelX22";
		((Control)labelX22).Size = new Size(14, 21);
		((Control)labelX22).TabIndex = 61;
		((Control)labelX22).Text = "%";
		labelX22.TextAlignment = (StringAlignment)2;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)labelX19);
		((Control)groupPanel1).Controls.Add((Control)(object)txtCountKhalesDayOnGharardad);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel1).Controls.Add((Control)(object)txtCountVacationOnGharardad);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX12);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX13);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX14);
		((Control)groupPanel1).Controls.Add((Control)(object)txtSumSalaryOnDay);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX15);
		((Control)groupPanel1).Controls.Add((Control)(object)txtSumKolSalary);
		((Control)groupPanel1).Controls.Add((Control)(object)txtSalaryOnHour);
		((Control)groupPanel1).Controls.Add((Control)(object)txtCountNotHolidayOnGharardad);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(350, 175);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(431, 246);
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
		TxtPriceTatil.AllowEmpty = true;
		TxtPriceTatil.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceTatil.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceTatil.BackcolorTooltip = Color.Red;
		TxtPriceTatil.CurrencyFormat = "";
		((Control)TxtPriceTatil).ForeColor = Color.Red;
		TxtPriceTatil.ForeColorTooltip = Color.Black;
		TxtPriceTatil.GetColoresFromSettingDLL = true;
		TxtPriceTatil.HandleKeydown = true;
		((Control)TxtPriceTatil).Location = new Point(5, 205);
		((TextBoxBase)TxtPriceTatil).MaxLength = 19;
		((Control)TxtPriceTatil).Name = "TxtPriceTatil";
		((TextBoxBase)TxtPriceTatil).ReadOnly = true;
		TxtPriceTatil.SendShiftTabAfterDown = true;
		TxtPriceTatil.SendShiftTabAfterEnter = true;
		TxtPriceTatil.SendTabAfterEnter = true;
		TxtPriceTatil.SendTabAfterUP = true;
		TxtPriceTatil.ShowHorofi = (ENShowHorofi)3;
		TxtPriceTatil.ShowToolTipText = true;
		((Control)TxtPriceTatil).Size = new Size(153, 21);
		((Control)TxtPriceTatil).TabIndex = 19;
		((Control)TxtPriceTatil).Text = "0";
		TxtPriceTatil.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceTatil.ToolTipPostFix = "";
		TxtPriceTatil.TootTipDelay = 1000;
		TxtPriceTatil.Value = new decimal(new int[4]);
		((Control)TxtPriceTatil).Visible = false;
		((BaseItemControl)labelX21).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX21).Location = new Point(165, 205);
		((Control)labelX21).Name = "labelX21";
		((Control)labelX21).Size = new Size(155, 21);
		((Control)labelX21).TabIndex = 18;
		((Control)labelX21).Text = "[ارزش یک ساعت تعطیل کاری] :";
		labelX21.TextAlignment = (StringAlignment)2;
		((Control)labelX21).Visible = false;
		TxtPriceFriday.AllowEmpty = true;
		TxtPriceFriday.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceFriday.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceFriday.BackcolorTooltip = Color.Red;
		TxtPriceFriday.CurrencyFormat = "";
		((Control)TxtPriceFriday).ForeColor = Color.Red;
		TxtPriceFriday.ForeColorTooltip = Color.Black;
		TxtPriceFriday.GetColoresFromSettingDLL = true;
		TxtPriceFriday.HandleKeydown = true;
		((Control)TxtPriceFriday).Location = new Point(5, 228);
		((TextBoxBase)TxtPriceFriday).MaxLength = 19;
		((Control)TxtPriceFriday).Name = "TxtPriceFriday";
		((TextBoxBase)TxtPriceFriday).ReadOnly = true;
		TxtPriceFriday.SendShiftTabAfterDown = true;
		TxtPriceFriday.SendShiftTabAfterEnter = true;
		TxtPriceFriday.SendTabAfterEnter = true;
		TxtPriceFriday.SendTabAfterUP = true;
		TxtPriceFriday.ShowHorofi = (ENShowHorofi)3;
		TxtPriceFriday.ShowToolTipText = true;
		((Control)TxtPriceFriday).Size = new Size(153, 21);
		((Control)TxtPriceFriday).TabIndex = 17;
		((Control)TxtPriceFriday).Text = "0";
		TxtPriceFriday.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceFriday.ToolTipPostFix = "";
		TxtPriceFriday.TootTipDelay = 1000;
		TxtPriceFriday.Value = new decimal(new int[4]);
		((Control)TxtPriceFriday).Visible = false;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(165, 228);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(155, 21);
		((Control)labelX7).TabIndex = 16;
		((Control)labelX7).Text = " [ارزش یک ساعت جمعه کاری] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)labelX7).Visible = false;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Location = new Point(180, 103);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).Size = new Size(242, 21);
		((Control)labelX19).TabIndex = 6;
		((Control)labelX19).Text = "[خالص کارکرد طی مدت قرارداد (روز)] :";
		labelX19.TextAlignment = (StringAlignment)2;
		txtCountKhalesDayOnGharardad.AllowEmpty = true;
		txtCountKhalesDayOnGharardad.BackColorTextBoxWhenFocus = Color.Yellow;
		txtCountKhalesDayOnGharardad.BackColorTextBoxWhenLostFocus = Color.White;
		txtCountKhalesDayOnGharardad.BackcolorTooltip = Color.Red;
		txtCountKhalesDayOnGharardad.CurrencyFormat = "";
		((Control)txtCountKhalesDayOnGharardad).ForeColor = Color.Red;
		txtCountKhalesDayOnGharardad.ForeColorTooltip = Color.Black;
		txtCountKhalesDayOnGharardad.GetColoresFromSettingDLL = true;
		txtCountKhalesDayOnGharardad.HandleKeydown = true;
		((Control)txtCountKhalesDayOnGharardad).Location = new Point(20, 103);
		((TextBoxBase)txtCountKhalesDayOnGharardad).MaxLength = 19;
		((Control)txtCountKhalesDayOnGharardad).Name = "txtCountKhalesDayOnGharardad";
		((TextBoxBase)txtCountKhalesDayOnGharardad).ReadOnly = true;
		txtCountKhalesDayOnGharardad.SendShiftTabAfterDown = true;
		txtCountKhalesDayOnGharardad.SendShiftTabAfterEnter = true;
		txtCountKhalesDayOnGharardad.SendTabAfterEnter = true;
		txtCountKhalesDayOnGharardad.SendTabAfterUP = true;
		txtCountKhalesDayOnGharardad.ShowHorofi = (ENShowHorofi)3;
		txtCountKhalesDayOnGharardad.ShowToolTipText = true;
		((Control)txtCountKhalesDayOnGharardad).Size = new Size(153, 21);
		((Control)txtCountKhalesDayOnGharardad).TabIndex = 7;
		((Control)txtCountKhalesDayOnGharardad).Text = "0";
		txtCountKhalesDayOnGharardad.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtCountKhalesDayOnGharardad.ToolTipPostFix = "";
		txtCountKhalesDayOnGharardad.TootTipDelay = 1000;
		txtCountKhalesDayOnGharardad.Value = new decimal(new int[4]);
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(180, 70);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(242, 21);
		((Control)labelX1).TabIndex = 4;
		((Control)labelX1).Text = "[تعداد روز مرخصی در مدت قرارداد] :";
		labelX1.TextAlignment = (StringAlignment)2;
		txtCountVacationOnGharardad.AllowEmpty = true;
		txtCountVacationOnGharardad.BackColorTextBoxWhenFocus = Color.Yellow;
		txtCountVacationOnGharardad.BackColorTextBoxWhenLostFocus = Color.White;
		txtCountVacationOnGharardad.BackcolorTooltip = Color.Red;
		txtCountVacationOnGharardad.CurrencyFormat = "";
		((Control)txtCountVacationOnGharardad).ForeColor = Color.Red;
		txtCountVacationOnGharardad.ForeColorTooltip = Color.Black;
		txtCountVacationOnGharardad.GetColoresFromSettingDLL = true;
		txtCountVacationOnGharardad.HandleKeydown = true;
		((Control)txtCountVacationOnGharardad).Location = new Point(20, 70);
		((TextBoxBase)txtCountVacationOnGharardad).MaxLength = 19;
		((Control)txtCountVacationOnGharardad).Name = "txtCountVacationOnGharardad";
		txtCountVacationOnGharardad.SendShiftTabAfterDown = true;
		txtCountVacationOnGharardad.SendShiftTabAfterEnter = true;
		txtCountVacationOnGharardad.SendTabAfterEnter = true;
		txtCountVacationOnGharardad.SendTabAfterUP = true;
		txtCountVacationOnGharardad.ShowHorofi = (ENShowHorofi)3;
		txtCountVacationOnGharardad.ShowToolTipText = true;
		((Control)txtCountVacationOnGharardad).Size = new Size(153, 21);
		((Control)txtCountVacationOnGharardad).TabIndex = 5;
		((Control)txtCountVacationOnGharardad).Text = "0";
		txtCountVacationOnGharardad.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtCountVacationOnGharardad.ToolTipPostFix = "";
		txtCountVacationOnGharardad.TootTipDelay = 1000;
		txtCountVacationOnGharardad.Value = new decimal(new int[4]);
		((Control)txtCountVacationOnGharardad).TextChanged += txtSalaryOnDay_TextChanged;
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Location = new Point(180, 4);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(242, 21);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "[کل هزینه حقوق طی مدت قرارداد] :";
		labelX12.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).Location = new Point(180, 37);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(242, 21);
		((Control)labelX13).TabIndex = 2;
		((Control)labelX13).Text = "[تعداد روز مفید کاری در طول قرارداد] :";
		labelX13.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Location = new Point(180, 136);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(242, 21);
		((Control)labelX14).TabIndex = 8;
		((Control)labelX14).Text = "[هزینه حقوق خالص یک روز کاری] :";
		labelX14.TextAlignment = (StringAlignment)2;
		txtSumSalaryOnDay.AllowEmpty = true;
		txtSumSalaryOnDay.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumSalaryOnDay.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumSalaryOnDay.BackcolorTooltip = Color.Red;
		txtSumSalaryOnDay.CurrencyFormat = "";
		((Control)txtSumSalaryOnDay).ForeColor = Color.Red;
		txtSumSalaryOnDay.ForeColorTooltip = Color.Black;
		txtSumSalaryOnDay.GetColoresFromSettingDLL = true;
		txtSumSalaryOnDay.HandleKeydown = true;
		((Control)txtSumSalaryOnDay).Location = new Point(20, 136);
		((TextBoxBase)txtSumSalaryOnDay).MaxLength = 19;
		((Control)txtSumSalaryOnDay).Name = "txtSumSalaryOnDay";
		((TextBoxBase)txtSumSalaryOnDay).ReadOnly = true;
		txtSumSalaryOnDay.SendShiftTabAfterDown = true;
		txtSumSalaryOnDay.SendShiftTabAfterEnter = true;
		txtSumSalaryOnDay.SendTabAfterEnter = true;
		txtSumSalaryOnDay.SendTabAfterUP = true;
		txtSumSalaryOnDay.ShowHorofi = (ENShowHorofi)3;
		txtSumSalaryOnDay.ShowToolTipText = true;
		((Control)txtSumSalaryOnDay).Size = new Size(153, 21);
		((Control)txtSumSalaryOnDay).TabIndex = 9;
		((Control)txtSumSalaryOnDay).Text = "0";
		txtSumSalaryOnDay.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumSalaryOnDay.ToolTipPostFix = "";
		txtSumSalaryOnDay.TootTipDelay = 1000;
		txtSumSalaryOnDay.Value = new decimal(new int[4]);
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).Location = new Point(180, 169);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(242, 21);
		((Control)labelX15).TabIndex = 10;
		((Control)labelX15).Text = "[ارزش یک ساعت کار] :";
		labelX15.TextAlignment = (StringAlignment)2;
		txtSumKolSalary.AllowEmpty = true;
		txtSumKolSalary.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumKolSalary.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumKolSalary.BackcolorTooltip = Color.Red;
		txtSumKolSalary.CurrencyFormat = "";
		((Control)txtSumKolSalary).ForeColor = Color.Red;
		txtSumKolSalary.ForeColorTooltip = Color.Black;
		txtSumKolSalary.GetColoresFromSettingDLL = true;
		txtSumKolSalary.HandleKeydown = true;
		((Control)txtSumKolSalary).Location = new Point(20, 4);
		((TextBoxBase)txtSumKolSalary).MaxLength = 19;
		((Control)txtSumKolSalary).Name = "txtSumKolSalary";
		((TextBoxBase)txtSumKolSalary).ReadOnly = true;
		txtSumKolSalary.SendShiftTabAfterDown = true;
		txtSumKolSalary.SendShiftTabAfterEnter = true;
		txtSumKolSalary.SendTabAfterEnter = true;
		txtSumKolSalary.SendTabAfterUP = true;
		txtSumKolSalary.ShowHorofi = (ENShowHorofi)3;
		txtSumKolSalary.ShowToolTipText = true;
		((Control)txtSumKolSalary).Size = new Size(153, 21);
		((Control)txtSumKolSalary).TabIndex = 1;
		((Control)txtSumKolSalary).Text = "0";
		txtSumKolSalary.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumKolSalary.ToolTipPostFix = "";
		txtSumKolSalary.TootTipDelay = 1000;
		txtSumKolSalary.Value = new decimal(new int[4]);
		txtSalaryOnHour.AllowEmpty = true;
		txtSalaryOnHour.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSalaryOnHour.BackColorTextBoxWhenLostFocus = Color.White;
		txtSalaryOnHour.BackcolorTooltip = Color.Red;
		txtSalaryOnHour.CurrencyFormat = "";
		((Control)txtSalaryOnHour).ForeColor = Color.Red;
		txtSalaryOnHour.ForeColorTooltip = Color.Black;
		txtSalaryOnHour.GetColoresFromSettingDLL = true;
		txtSalaryOnHour.HandleKeydown = true;
		((Control)txtSalaryOnHour).Location = new Point(20, 169);
		((TextBoxBase)txtSalaryOnHour).MaxLength = 19;
		((Control)txtSalaryOnHour).Name = "txtSalaryOnHour";
		((TextBoxBase)txtSalaryOnHour).ReadOnly = true;
		txtSalaryOnHour.SendShiftTabAfterDown = true;
		txtSalaryOnHour.SendShiftTabAfterEnter = true;
		txtSalaryOnHour.SendTabAfterEnter = true;
		txtSalaryOnHour.SendTabAfterUP = true;
		txtSalaryOnHour.ShowHorofi = (ENShowHorofi)3;
		txtSalaryOnHour.ShowToolTipText = true;
		((Control)txtSalaryOnHour).Size = new Size(153, 21);
		((Control)txtSalaryOnHour).TabIndex = 11;
		((Control)txtSalaryOnHour).Text = "0";
		txtSalaryOnHour.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSalaryOnHour.ToolTipPostFix = "";
		txtSalaryOnHour.TootTipDelay = 1000;
		txtSalaryOnHour.Value = new decimal(new int[4]);
		txtCountNotHolidayOnGharardad.AllowEmpty = true;
		txtCountNotHolidayOnGharardad.BackColorTextBoxWhenFocus = Color.Yellow;
		txtCountNotHolidayOnGharardad.BackColorTextBoxWhenLostFocus = Color.White;
		txtCountNotHolidayOnGharardad.BackcolorTooltip = Color.Red;
		txtCountNotHolidayOnGharardad.CurrencyFormat = "";
		((Control)txtCountNotHolidayOnGharardad).ForeColor = Color.Red;
		txtCountNotHolidayOnGharardad.ForeColorTooltip = Color.Black;
		txtCountNotHolidayOnGharardad.GetColoresFromSettingDLL = true;
		txtCountNotHolidayOnGharardad.HandleKeydown = true;
		((Control)txtCountNotHolidayOnGharardad).Location = new Point(20, 37);
		((TextBoxBase)txtCountNotHolidayOnGharardad).MaxLength = 19;
		((Control)txtCountNotHolidayOnGharardad).Name = "txtCountNotHolidayOnGharardad";
		txtCountNotHolidayOnGharardad.SendShiftTabAfterDown = true;
		txtCountNotHolidayOnGharardad.SendShiftTabAfterEnter = true;
		txtCountNotHolidayOnGharardad.SendTabAfterEnter = true;
		txtCountNotHolidayOnGharardad.SendTabAfterUP = true;
		txtCountNotHolidayOnGharardad.ShowHorofi = (ENShowHorofi)3;
		txtCountNotHolidayOnGharardad.ShowToolTipText = true;
		((Control)txtCountNotHolidayOnGharardad).Size = new Size(153, 21);
		((Control)txtCountNotHolidayOnGharardad).TabIndex = 3;
		((Control)txtCountNotHolidayOnGharardad).Text = "0";
		txtCountNotHolidayOnGharardad.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtCountNotHolidayOnGharardad.ToolTipPostFix = "";
		txtCountNotHolidayOnGharardad.TootTipDelay = 1000;
		txtCountNotHolidayOnGharardad.Value = new decimal(new int[4]);
		((Control)txtCountNotHolidayOnGharardad).TextChanged += txtSalaryOnDay_TextChanged;
		TxtPriceLow.AllowEmpty = true;
		TxtPriceLow.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceLow.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceLow.BackcolorTooltip = Color.Red;
		TxtPriceLow.CurrencyFormat = "";
		((Control)TxtPriceLow).ForeColor = Color.Red;
		TxtPriceLow.ForeColorTooltip = Color.Black;
		TxtPriceLow.GetColoresFromSettingDLL = true;
		TxtPriceLow.HandleKeydown = true;
		((Control)TxtPriceLow).Location = new Point(5, 251);
		((TextBoxBase)TxtPriceLow).MaxLength = 19;
		((Control)TxtPriceLow).Name = "TxtPriceLow";
		((TextBoxBase)TxtPriceLow).ReadOnly = true;
		TxtPriceLow.SendShiftTabAfterDown = true;
		TxtPriceLow.SendShiftTabAfterEnter = true;
		TxtPriceLow.SendTabAfterEnter = true;
		TxtPriceLow.SendTabAfterUP = true;
		TxtPriceLow.ShowHorofi = (ENShowHorofi)3;
		TxtPriceLow.ShowToolTipText = true;
		((Control)TxtPriceLow).Size = new Size(153, 21);
		((Control)TxtPriceLow).TabIndex = 15;
		((Control)TxtPriceLow).Text = "0";
		TxtPriceLow.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceLow.ToolTipPostFix = "";
		TxtPriceLow.TootTipDelay = 1000;
		TxtPriceLow.Value = new decimal(new int[4]);
		((Control)TxtPriceLow).Visible = false;
		TxtPriceEzafe.AllowEmpty = true;
		TxtPriceEzafe.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceEzafe.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceEzafe.BackcolorTooltip = Color.Red;
		TxtPriceEzafe.CurrencyFormat = "";
		((Control)TxtPriceEzafe).ForeColor = Color.Red;
		TxtPriceEzafe.ForeColorTooltip = Color.Black;
		TxtPriceEzafe.GetColoresFromSettingDLL = true;
		TxtPriceEzafe.HandleKeydown = true;
		((Control)TxtPriceEzafe).Location = new Point(5, 182);
		((TextBoxBase)TxtPriceEzafe).MaxLength = 19;
		((Control)TxtPriceEzafe).Name = "TxtPriceEzafe";
		((TextBoxBase)TxtPriceEzafe).ReadOnly = true;
		TxtPriceEzafe.SendShiftTabAfterDown = true;
		TxtPriceEzafe.SendShiftTabAfterEnter = true;
		TxtPriceEzafe.SendTabAfterEnter = true;
		TxtPriceEzafe.SendTabAfterUP = true;
		TxtPriceEzafe.ShowHorofi = (ENShowHorofi)3;
		TxtPriceEzafe.ShowToolTipText = true;
		((Control)TxtPriceEzafe).Size = new Size(153, 21);
		((Control)TxtPriceEzafe).TabIndex = 13;
		((Control)TxtPriceEzafe).Text = "0";
		TxtPriceEzafe.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceEzafe.ToolTipPostFix = "";
		TxtPriceEzafe.TootTipDelay = 1000;
		TxtPriceEzafe.Value = new decimal(new int[4]);
		((Control)TxtPriceEzafe).Visible = false;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(165, 182);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(155, 21);
		((Control)labelX4).TabIndex = 12;
		((Control)labelX4).Text = "[ارزش یک ساعت اضافه کار] :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Visible = false;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(165, 251);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(155, 21);
		((Control)labelX3).TabIndex = 14;
		((Control)labelX3).Text = "[ارزش یک ساعت کسرکار] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Visible = false;
		((BaseItemControl)labelX27).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX27).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX27).Location = new Point(258, 39);
		((Control)labelX27).Name = "labelX27";
		((Control)labelX27).Size = new Size(92, 21);
		((Control)labelX27).TabIndex = 15;
		((Control)labelX27).Text = "[ماههای قرارداد]";
		labelX27.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX53).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX53).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX53).Location = new Point(12, 94);
		((Control)labelX53).Name = "labelX53";
		((Control)labelX53).Size = new Size(116, 21);
		((Control)labelX53).TabIndex = 39;
		((Control)labelX53).Text = "[حق بیمه سهم کارفرما]";
		((BaseItemControl)labelX34).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX34).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX34).Location = new Point(12, 67);
		((Control)labelX34).Name = "labelX34";
		((Control)labelX34).Size = new Size(116, 21);
		((Control)labelX34).TabIndex = 39;
		((Control)labelX34).Text = "[حق بیمه سهم کارفرما]";
		((BaseItemControl)labelX52).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX52).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX52).Location = new Point(258, 94);
		((Control)labelX52).Name = "labelX52";
		((Control)labelX52).Size = new Size(84, 21);
		((Control)labelX52).TabIndex = 35;
		((Control)labelX52).Text = "[ماههای قرارداد]";
		labelX52.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX33).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX33).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX33).Location = new Point(258, 67);
		((Control)labelX33).Name = "labelX33";
		((Control)labelX33).Size = new Size(84, 21);
		((Control)labelX33).TabIndex = 35;
		((Control)labelX33).Text = "[روزهای قرارداد]";
		labelX33.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX41).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX41).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX41).Location = new Point(258, 12);
		((Control)labelX41).Name = "labelX41";
		((Control)labelX41).Size = new Size(84, 21);
		((Control)labelX41).TabIndex = 6;
		((Control)labelX41).Text = "[روزهای قرارداد]";
		labelX41.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX38).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX38).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX38).Location = new Point(258, 148);
		((Control)labelX38).Name = "labelX38";
		((Control)labelX38).Size = new Size(84, 21);
		((Control)labelX38).TabIndex = 55;
		((Control)labelX38).Text = "[تعداد روز پاداش]";
		labelX38.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX11).Location = new Point(258, 121);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(84, 21);
		((Control)labelX11).TabIndex = 46;
		((Control)labelX11).Text = "[تعداد روز سنوات]";
		labelX11.TextAlignment = (StringAlignment)2;
		txtDaysofGharardad.AllowEmpty = true;
		txtDaysofGharardad.BackColorTextBoxWhenFocus = Color.Yellow;
		txtDaysofGharardad.BackColorTextBoxWhenLostFocus = Color.White;
		txtDaysofGharardad.BackcolorTooltip = Color.Red;
		txtDaysofGharardad.CurrencyFormat = "";
		((Control)txtDaysofGharardad).ForeColor = Color.Red;
		txtDaysofGharardad.ForeColorTooltip = Color.Black;
		txtDaysofGharardad.GetColoresFromSettingDLL = true;
		txtDaysofGharardad.HandleKeydown = true;
		((Control)txtDaysofGharardad).Location = new Point(549, 12);
		((TextBoxBase)txtDaysofGharardad).MaxLength = 19;
		((Control)txtDaysofGharardad).Name = "txtDaysofGharardad";
		txtDaysofGharardad.SendShiftTabAfterDown = true;
		txtDaysofGharardad.SendShiftTabAfterEnter = true;
		txtDaysofGharardad.SendTabAfterEnter = true;
		txtDaysofGharardad.SendTabAfterUP = true;
		txtDaysofGharardad.ShowHorofi = (ENShowHorofi)3;
		txtDaysofGharardad.ShowToolTipText = true;
		((Control)txtDaysofGharardad).Size = new Size(51, 21);
		((Control)txtDaysofGharardad).TabIndex = 3;
		((Control)txtDaysofGharardad).Text = "0";
		txtDaysofGharardad.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtDaysofGharardad.ToolTipPostFix = "";
		txtDaysofGharardad.TootTipDelay = 1000;
		txtDaysofGharardad.Value = new decimal(new int[4]);
		((Control)txtDaysofGharardad).TextChanged += txtSalaryOnDay_TextChanged;
		txtCountDayForPadash.AllowEmpty = true;
		((Control)txtCountDayForPadash).BackColor = SystemColors.Window;
		txtCountDayForPadash.BackColorTextBoxWhenFocus = Color.Yellow;
		txtCountDayForPadash.BackColorTextBoxWhenLostFocus = Color.White;
		txtCountDayForPadash.BackcolorTooltip = Color.Red;
		txtCountDayForPadash.CurrencyFormat = "";
		((Control)txtCountDayForPadash).ForeColor = Color.Red;
		txtCountDayForPadash.ForeColorTooltip = Color.Black;
		txtCountDayForPadash.GetColoresFromSettingDLL = true;
		txtCountDayForPadash.HandleKeydown = true;
		((Control)txtCountDayForPadash).Location = new Point(549, 148);
		((TextBoxBase)txtCountDayForPadash).MaxLength = 19;
		((Control)txtCountDayForPadash).Name = "txtCountDayForPadash";
		txtCountDayForPadash.SendShiftTabAfterDown = true;
		txtCountDayForPadash.SendShiftTabAfterEnter = true;
		txtCountDayForPadash.SendTabAfterEnter = true;
		txtCountDayForPadash.SendTabAfterUP = true;
		txtCountDayForPadash.ShowHorofi = (ENShowHorofi)3;
		txtCountDayForPadash.ShowToolTipText = true;
		((Control)txtCountDayForPadash).Size = new Size(51, 21);
		((Control)txtCountDayForPadash).TabIndex = 52;
		((Control)txtCountDayForPadash).Text = "0";
		txtCountDayForPadash.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtCountDayForPadash.ToolTipPostFix = "";
		txtCountDayForPadash.TootTipDelay = 1000;
		txtCountDayForPadash.Value = new decimal(new int[4]);
		((Control)txtCountDayForPadash).TextChanged += txtSalaryOnDay_TextChanged;
		txtCountDayForSanavat.AllowEmpty = true;
		((Control)txtCountDayForSanavat).BackColor = SystemColors.Window;
		txtCountDayForSanavat.BackColorTextBoxWhenFocus = Color.Yellow;
		txtCountDayForSanavat.BackColorTextBoxWhenLostFocus = Color.White;
		txtCountDayForSanavat.BackcolorTooltip = Color.Red;
		txtCountDayForSanavat.CurrencyFormat = "";
		((Control)txtCountDayForSanavat).ForeColor = Color.Red;
		txtCountDayForSanavat.ForeColorTooltip = Color.Black;
		txtCountDayForSanavat.GetColoresFromSettingDLL = true;
		txtCountDayForSanavat.HandleKeydown = true;
		((Control)txtCountDayForSanavat).Location = new Point(549, 121);
		((TextBoxBase)txtCountDayForSanavat).MaxLength = 19;
		((Control)txtCountDayForSanavat).Name = "txtCountDayForSanavat";
		txtCountDayForSanavat.SendShiftTabAfterDown = true;
		txtCountDayForSanavat.SendShiftTabAfterEnter = true;
		txtCountDayForSanavat.SendTabAfterEnter = true;
		txtCountDayForSanavat.SendTabAfterUP = true;
		txtCountDayForSanavat.ShowHorofi = (ENShowHorofi)3;
		txtCountDayForSanavat.ShowToolTipText = true;
		((Control)txtCountDayForSanavat).Size = new Size(51, 21);
		((Control)txtCountDayForSanavat).TabIndex = 43;
		((Control)txtCountDayForSanavat).Text = "0";
		txtCountDayForSanavat.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtCountDayForSanavat.ToolTipPostFix = "";
		txtCountDayForSanavat.TootTipDelay = 1000;
		txtCountDayForSanavat.Value = new decimal(new int[4]);
		((Control)txtCountDayForSanavat).TextChanged += txtSalaryOnDay_TextChanged;
		txtDaysofGharardadForBime.AllowEmpty = true;
		txtDaysofGharardadForBime.BackColorTextBoxWhenFocus = Color.Yellow;
		txtDaysofGharardadForBime.BackColorTextBoxWhenLostFocus = Color.White;
		txtDaysofGharardadForBime.BackcolorTooltip = Color.Red;
		txtDaysofGharardadForBime.CurrencyFormat = "";
		((Control)txtDaysofGharardadForBime).ForeColor = Color.Red;
		txtDaysofGharardadForBime.ForeColorTooltip = Color.Black;
		txtDaysofGharardadForBime.GetColoresFromSettingDLL = true;
		txtDaysofGharardadForBime.HandleKeydown = true;
		((Control)txtDaysofGharardadForBime).Location = new Point(549, 67);
		((TextBoxBase)txtDaysofGharardadForBime).MaxLength = 19;
		((Control)txtDaysofGharardadForBime).Name = "txtDaysofGharardadForBime";
		((TextBoxBase)txtDaysofGharardadForBime).ReadOnly = true;
		txtDaysofGharardadForBime.SendShiftTabAfterDown = true;
		txtDaysofGharardadForBime.SendShiftTabAfterEnter = true;
		txtDaysofGharardadForBime.SendTabAfterEnter = true;
		txtDaysofGharardadForBime.SendTabAfterUP = true;
		txtDaysofGharardadForBime.ShowHorofi = (ENShowHorofi)3;
		txtDaysofGharardadForBime.ShowToolTipText = true;
		((Control)txtDaysofGharardadForBime).Size = new Size(51, 21);
		((Control)txtDaysofGharardadForBime).TabIndex = 30;
		((Control)txtDaysofGharardadForBime).Text = "0";
		txtDaysofGharardadForBime.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtDaysofGharardadForBime.ToolTipPostFix = "";
		txtDaysofGharardadForBime.TootTipDelay = 1000;
		txtDaysofGharardadForBime.Value = new decimal(new int[4]);
		txtMonthOfGharardad.AllowEmpty = true;
		txtMonthOfGharardad.BackColorTextBoxWhenFocus = Color.Yellow;
		txtMonthOfGharardad.BackColorTextBoxWhenLostFocus = Color.White;
		txtMonthOfGharardad.BackcolorTooltip = Color.Red;
		txtMonthOfGharardad.CurrencyFormat = "";
		((Control)txtMonthOfGharardad).ForeColor = Color.Red;
		txtMonthOfGharardad.ForeColorTooltip = Color.Black;
		txtMonthOfGharardad.GetColoresFromSettingDLL = true;
		txtMonthOfGharardad.HandleKeydown = true;
		((Control)txtMonthOfGharardad).Location = new Point(549, 39);
		((TextBoxBase)txtMonthOfGharardad).MaxLength = 19;
		((Control)txtMonthOfGharardad).Name = "txtMonthOfGharardad";
		txtMonthOfGharardad.SendShiftTabAfterDown = true;
		txtMonthOfGharardad.SendShiftTabAfterEnter = true;
		txtMonthOfGharardad.SendTabAfterEnter = true;
		txtMonthOfGharardad.SendTabAfterUP = true;
		txtMonthOfGharardad.ShowHorofi = (ENShowHorofi)3;
		txtMonthOfGharardad.ShowToolTipText = true;
		((Control)txtMonthOfGharardad).Size = new Size(51, 21);
		((Control)txtMonthOfGharardad).TabIndex = 12;
		((Control)txtMonthOfGharardad).Text = "0";
		txtMonthOfGharardad.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtMonthOfGharardad.ToolTipPostFix = "";
		txtMonthOfGharardad.TootTipDelay = 1000;
		txtMonthOfGharardad.Value = new decimal(new int[4]);
		((Control)txtMonthOfGharardad).TextChanged += txtSalaryOnDay_TextChanged;
		txtSumPadash.AllowEmpty = true;
		txtSumPadash.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumPadash.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumPadash.BackcolorTooltip = Color.Red;
		txtSumPadash.CurrencyFormat = "";
		((Control)txtSumPadash).ForeColor = Color.Red;
		txtSumPadash.ForeColorTooltip = Color.Black;
		txtSumPadash.GetColoresFromSettingDLL = true;
		txtSumPadash.HandleKeydown = true;
		((Control)txtSumPadash).Location = new Point(626, 148);
		((TextBoxBase)txtSumPadash).MaxLength = 19;
		((Control)txtSumPadash).Name = "txtSumPadash";
		((TextBoxBase)txtSumPadash).ReadOnly = true;
		txtSumPadash.SendShiftTabAfterDown = true;
		txtSumPadash.SendShiftTabAfterEnter = true;
		txtSumPadash.SendTabAfterEnter = true;
		txtSumPadash.SendTabAfterUP = true;
		txtSumPadash.ShowHorofi = (ENShowHorofi)3;
		txtSumPadash.ShowToolTipText = true;
		((Control)txtSumPadash).Size = new Size(155, 21);
		((Control)txtSumPadash).TabIndex = 50;
		((Control)txtSumPadash).Text = "0";
		txtSumPadash.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumPadash.ToolTipPostFix = "";
		txtSumPadash.TootTipDelay = 1000;
		txtSumPadash.Value = new decimal(new int[4]);
		txtSumSanavat.AllowEmpty = true;
		txtSumSanavat.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumSanavat.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumSanavat.BackcolorTooltip = Color.Red;
		txtSumSanavat.CurrencyFormat = "";
		((Control)txtSumSanavat).ForeColor = Color.Red;
		txtSumSanavat.ForeColorTooltip = Color.Black;
		txtSumSanavat.GetColoresFromSettingDLL = true;
		txtSumSanavat.HandleKeydown = true;
		((Control)txtSumSanavat).Location = new Point(626, 121);
		((TextBoxBase)txtSumSanavat).MaxLength = 19;
		((Control)txtSumSanavat).Name = "txtSumSanavat";
		((TextBoxBase)txtSumSanavat).ReadOnly = true;
		txtSumSanavat.SendShiftTabAfterDown = true;
		txtSumSanavat.SendShiftTabAfterEnter = true;
		txtSumSanavat.SendTabAfterEnter = true;
		txtSumSanavat.SendTabAfterUP = true;
		txtSumSanavat.ShowHorofi = (ENShowHorofi)3;
		txtSumSanavat.ShowToolTipText = true;
		((Control)txtSumSanavat).Size = new Size(155, 21);
		((Control)txtSumSanavat).TabIndex = 41;
		((Control)txtSumSanavat).Text = "0";
		txtSumSanavat.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumSanavat.ToolTipPostFix = "";
		txtSumSanavat.TootTipDelay = 1000;
		txtSumSanavat.Value = new decimal(new int[4]);
		txtSumEmployerInsurance2.AllowEmpty = true;
		txtSumEmployerInsurance2.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumEmployerInsurance2.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumEmployerInsurance2.BackcolorTooltip = Color.Red;
		txtSumEmployerInsurance2.CurrencyFormat = "";
		((Control)txtSumEmployerInsurance2).ForeColor = Color.Red;
		txtSumEmployerInsurance2.ForeColorTooltip = Color.Black;
		txtSumEmployerInsurance2.GetColoresFromSettingDLL = true;
		txtSumEmployerInsurance2.HandleKeydown = true;
		((Control)txtSumEmployerInsurance2).Location = new Point(626, 94);
		((TextBoxBase)txtSumEmployerInsurance2).MaxLength = 19;
		((Control)txtSumEmployerInsurance2).Name = "txtSumEmployerInsurance2";
		((TextBoxBase)txtSumEmployerInsurance2).ReadOnly = true;
		txtSumEmployerInsurance2.SendShiftTabAfterDown = true;
		txtSumEmployerInsurance2.SendShiftTabAfterEnter = true;
		txtSumEmployerInsurance2.SendTabAfterEnter = true;
		txtSumEmployerInsurance2.SendTabAfterUP = true;
		txtSumEmployerInsurance2.ShowHorofi = (ENShowHorofi)3;
		txtSumEmployerInsurance2.ShowToolTipText = true;
		((Control)txtSumEmployerInsurance2).Size = new Size(155, 21);
		((Control)txtSumEmployerInsurance2).TabIndex = 28;
		((Control)txtSumEmployerInsurance2).Text = "0";
		txtSumEmployerInsurance2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumEmployerInsurance2.ToolTipPostFix = "";
		txtSumEmployerInsurance2.TootTipDelay = 1000;
		txtSumEmployerInsurance2.Value = new decimal(new int[4]);
		txtSumEmployerInsurance.AllowEmpty = true;
		txtSumEmployerInsurance.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumEmployerInsurance.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumEmployerInsurance.BackcolorTooltip = Color.Red;
		txtSumEmployerInsurance.CurrencyFormat = "";
		((Control)txtSumEmployerInsurance).ForeColor = Color.Red;
		txtSumEmployerInsurance.ForeColorTooltip = Color.Black;
		txtSumEmployerInsurance.GetColoresFromSettingDLL = true;
		txtSumEmployerInsurance.HandleKeydown = true;
		((Control)txtSumEmployerInsurance).Location = new Point(626, 67);
		((TextBoxBase)txtSumEmployerInsurance).MaxLength = 19;
		((Control)txtSumEmployerInsurance).Name = "txtSumEmployerInsurance";
		((TextBoxBase)txtSumEmployerInsurance).ReadOnly = true;
		txtSumEmployerInsurance.SendShiftTabAfterDown = true;
		txtSumEmployerInsurance.SendShiftTabAfterEnter = true;
		txtSumEmployerInsurance.SendTabAfterEnter = true;
		txtSumEmployerInsurance.SendTabAfterUP = true;
		txtSumEmployerInsurance.ShowHorofi = (ENShowHorofi)3;
		txtSumEmployerInsurance.ShowToolTipText = true;
		((Control)txtSumEmployerInsurance).Size = new Size(155, 21);
		((Control)txtSumEmployerInsurance).TabIndex = 28;
		((Control)txtSumEmployerInsurance).Text = "0";
		txtSumEmployerInsurance.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumEmployerInsurance.ToolTipPostFix = "";
		txtSumEmployerInsurance.TootTipDelay = 1000;
		txtSumEmployerInsurance.Value = new decimal(new int[4]);
		txtZaribEmployerInsurance.AllowEmpty = true;
		((Control)txtZaribEmployerInsurance).BackColor = SystemColors.Window;
		txtZaribEmployerInsurance.BackColorTextBoxWhenFocus = Color.Yellow;
		txtZaribEmployerInsurance.BackColorTextBoxWhenLostFocus = Color.White;
		txtZaribEmployerInsurance.BackcolorTooltip = Color.Red;
		txtZaribEmployerInsurance.CurrencyFormat = "";
		((Control)txtZaribEmployerInsurance).ForeColor = Color.Red;
		txtZaribEmployerInsurance.ForeColorTooltip = Color.Black;
		txtZaribEmployerInsurance.GetColoresFromSettingDLL = true;
		txtZaribEmployerInsurance.HandleKeydown = true;
		((Control)txtZaribEmployerInsurance).Location = new Point(350, 67);
		((TextBoxBase)txtZaribEmployerInsurance).MaxLength = 19;
		((Control)txtZaribEmployerInsurance).Name = "txtZaribEmployerInsurance";
		txtZaribEmployerInsurance.SendShiftTabAfterDown = true;
		txtZaribEmployerInsurance.SendShiftTabAfterEnter = true;
		txtZaribEmployerInsurance.SendTabAfterEnter = true;
		txtZaribEmployerInsurance.SendTabAfterUP = true;
		txtZaribEmployerInsurance.ShowHorofi = (ENShowHorofi)3;
		txtZaribEmployerInsurance.ShowToolTipText = true;
		((Control)txtZaribEmployerInsurance).Size = new Size(40, 21);
		((Control)txtZaribEmployerInsurance).TabIndex = 34;
		((Control)txtZaribEmployerInsurance).Text = "0";
		txtZaribEmployerInsurance.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtZaribEmployerInsurance.ToolTipPostFix = "";
		txtZaribEmployerInsurance.TootTipDelay = 1000;
		txtZaribEmployerInsurance.Value = new decimal(new int[4]);
		((Control)txtZaribEmployerInsurance).TextChanged += txtSalaryOnDay_TextChanged;
		txtSalaryOnDayBime.AllowEmpty = true;
		txtSalaryOnDayBime.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSalaryOnDayBime.BackColorTextBoxWhenLostFocus = Color.White;
		txtSalaryOnDayBime.BackcolorTooltip = Color.Red;
		txtSalaryOnDayBime.CurrencyFormat = "";
		((Control)txtSalaryOnDayBime).ForeColor = Color.Red;
		txtSalaryOnDayBime.ForeColorTooltip = Color.Black;
		txtSalaryOnDayBime.GetColoresFromSettingDLL = true;
		txtSalaryOnDayBime.HandleKeydown = true;
		((Control)txtSalaryOnDayBime).Location = new Point(431, 67);
		((TextBoxBase)txtSalaryOnDayBime).MaxLength = 19;
		((Control)txtSalaryOnDayBime).Name = "txtSalaryOnDayBime";
		((TextBoxBase)txtSalaryOnDayBime).ReadOnly = true;
		txtSalaryOnDayBime.SendShiftTabAfterDown = true;
		txtSalaryOnDayBime.SendShiftTabAfterEnter = true;
		txtSalaryOnDayBime.SendTabAfterEnter = true;
		txtSalaryOnDayBime.SendTabAfterUP = true;
		txtSalaryOnDayBime.ShowHorofi = (ENShowHorofi)3;
		txtSalaryOnDayBime.ShowToolTipText = true;
		((Control)txtSalaryOnDayBime).Size = new Size(92, 21);
		((Control)txtSalaryOnDayBime).TabIndex = 32;
		((Control)txtSalaryOnDayBime).Text = "0";
		txtSalaryOnDayBime.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSalaryOnDayBime.ToolTipPostFix = "";
		txtSalaryOnDayBime.TootTipDelay = 1000;
		txtSalaryOnDayBime.Value = new decimal(new int[4]);
		txtSalaryOnDaySanavat.AllowEmpty = true;
		txtSalaryOnDaySanavat.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSalaryOnDaySanavat.BackColorTextBoxWhenLostFocus = Color.White;
		txtSalaryOnDaySanavat.BackcolorTooltip = Color.Red;
		txtSalaryOnDaySanavat.CurrencyFormat = "";
		((Control)txtSalaryOnDaySanavat).ForeColor = Color.Red;
		txtSalaryOnDaySanavat.ForeColorTooltip = Color.Black;
		txtSalaryOnDaySanavat.GetColoresFromSettingDLL = true;
		txtSalaryOnDaySanavat.HandleKeydown = true;
		((Control)txtSalaryOnDaySanavat).Location = new Point(350, 121);
		((TextBoxBase)txtSalaryOnDaySanavat).MaxLength = 19;
		((Control)txtSalaryOnDaySanavat).Name = "txtSalaryOnDaySanavat";
		((TextBoxBase)txtSalaryOnDaySanavat).ReadOnly = true;
		txtSalaryOnDaySanavat.SendShiftTabAfterDown = true;
		txtSalaryOnDaySanavat.SendShiftTabAfterEnter = true;
		txtSalaryOnDaySanavat.SendTabAfterEnter = true;
		txtSalaryOnDaySanavat.SendTabAfterUP = true;
		txtSalaryOnDaySanavat.ShowHorofi = (ENShowHorofi)3;
		txtSalaryOnDaySanavat.ShowToolTipText = true;
		((Control)txtSalaryOnDaySanavat).Size = new Size(173, 21);
		((Control)txtSalaryOnDaySanavat).TabIndex = 45;
		((Control)txtSalaryOnDaySanavat).Text = "0";
		txtSalaryOnDaySanavat.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSalaryOnDaySanavat.ToolTipPostFix = "";
		txtSalaryOnDaySanavat.TootTipDelay = 1000;
		txtSalaryOnDaySanavat.Value = new decimal(new int[4]);
		txtSalaryOnDayPadash.AllowEmpty = true;
		txtSalaryOnDayPadash.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSalaryOnDayPadash.BackColorTextBoxWhenLostFocus = Color.White;
		txtSalaryOnDayPadash.BackcolorTooltip = Color.Red;
		txtSalaryOnDayPadash.CurrencyFormat = "";
		((Control)txtSalaryOnDayPadash).ForeColor = Color.Red;
		txtSalaryOnDayPadash.ForeColorTooltip = Color.Black;
		txtSalaryOnDayPadash.GetColoresFromSettingDLL = true;
		txtSalaryOnDayPadash.HandleKeydown = true;
		((Control)txtSalaryOnDayPadash).Location = new Point(350, 148);
		((TextBoxBase)txtSalaryOnDayPadash).MaxLength = 19;
		((Control)txtSalaryOnDayPadash).Name = "txtSalaryOnDayPadash";
		((TextBoxBase)txtSalaryOnDayPadash).ReadOnly = true;
		txtSalaryOnDayPadash.SendShiftTabAfterDown = true;
		txtSalaryOnDayPadash.SendShiftTabAfterEnter = true;
		txtSalaryOnDayPadash.SendTabAfterEnter = true;
		txtSalaryOnDayPadash.SendTabAfterUP = true;
		txtSalaryOnDayPadash.ShowHorofi = (ENShowHorofi)3;
		txtSalaryOnDayPadash.ShowToolTipText = true;
		((Control)txtSalaryOnDayPadash).Size = new Size(173, 21);
		((Control)txtSalaryOnDayPadash).TabIndex = 54;
		((Control)txtSalaryOnDayPadash).Text = "0";
		txtSalaryOnDayPadash.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSalaryOnDayPadash.ToolTipPostFix = "";
		txtSalaryOnDayPadash.TootTipDelay = 1000;
		txtSalaryOnDayPadash.Value = new decimal(new int[4]);
		txtSumKolMazayaOneMonth.AllowEmpty = true;
		txtSumKolMazayaOneMonth.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumKolMazayaOneMonth.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumKolMazayaOneMonth.BackcolorTooltip = Color.Red;
		txtSumKolMazayaOneMonth.CurrencyFormat = "";
		((Control)txtSumKolMazayaOneMonth).ForeColor = Color.Red;
		txtSumKolMazayaOneMonth.ForeColorTooltip = Color.Black;
		txtSumKolMazayaOneMonth.GetColoresFromSettingDLL = true;
		txtSumKolMazayaOneMonth.HandleKeydown = true;
		((Control)txtSumKolMazayaOneMonth).Location = new Point(626, 39);
		((TextBoxBase)txtSumKolMazayaOneMonth).MaxLength = 19;
		((Control)txtSumKolMazayaOneMonth).Name = "txtSumKolMazayaOneMonth";
		((TextBoxBase)txtSumKolMazayaOneMonth).ReadOnly = true;
		txtSumKolMazayaOneMonth.SendShiftTabAfterDown = true;
		txtSumKolMazayaOneMonth.SendShiftTabAfterEnter = true;
		txtSumKolMazayaOneMonth.SendTabAfterEnter = true;
		txtSumKolMazayaOneMonth.SendTabAfterUP = true;
		txtSumKolMazayaOneMonth.ShowHorofi = (ENShowHorofi)3;
		txtSumKolMazayaOneMonth.ShowToolTipText = true;
		((Control)txtSumKolMazayaOneMonth).Size = new Size(155, 21);
		((Control)txtSumKolMazayaOneMonth).TabIndex = 10;
		((Control)txtSumKolMazayaOneMonth).Text = "0";
		txtSumKolMazayaOneMonth.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumKolMazayaOneMonth.ToolTipPostFix = "";
		txtSumKolMazayaOneMonth.TootTipDelay = 1000;
		txtSumKolMazayaOneMonth.Value = new decimal(new int[4]);
		txtSumSalaryOnGharardad.AllowEmpty = true;
		txtSumSalaryOnGharardad.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumSalaryOnGharardad.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumSalaryOnGharardad.BackcolorTooltip = Color.Red;
		txtSumSalaryOnGharardad.CurrencyFormat = "";
		((Control)txtSumSalaryOnGharardad).ForeColor = Color.Red;
		txtSumSalaryOnGharardad.ForeColorTooltip = Color.Black;
		txtSumSalaryOnGharardad.GetColoresFromSettingDLL = true;
		txtSumSalaryOnGharardad.HandleKeydown = true;
		((Control)txtSumSalaryOnGharardad).Location = new Point(626, 12);
		((TextBoxBase)txtSumSalaryOnGharardad).MaxLength = 19;
		((Control)txtSumSalaryOnGharardad).Name = "txtSumSalaryOnGharardad";
		((TextBoxBase)txtSumSalaryOnGharardad).ReadOnly = true;
		txtSumSalaryOnGharardad.SendShiftTabAfterDown = true;
		txtSumSalaryOnGharardad.SendShiftTabAfterEnter = true;
		txtSumSalaryOnGharardad.SendTabAfterEnter = true;
		txtSumSalaryOnGharardad.SendTabAfterUP = true;
		txtSumSalaryOnGharardad.ShowHorofi = (ENShowHorofi)3;
		txtSumSalaryOnGharardad.ShowToolTipText = true;
		((Control)txtSumSalaryOnGharardad).Size = new Size(155, 21);
		((Control)txtSumSalaryOnGharardad).TabIndex = 1;
		((Control)txtSumSalaryOnGharardad).Text = "0";
		txtSumSalaryOnGharardad.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumSalaryOnGharardad.ToolTipPostFix = "";
		txtSumSalaryOnGharardad.TootTipDelay = 1000;
		txtSumSalaryOnGharardad.Value = new decimal(new int[4]);
		txtSumMazayaOneMonth.AllowEmpty = true;
		txtSumMazayaOneMonth.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSumMazayaOneMonth.BackColorTextBoxWhenLostFocus = Color.White;
		txtSumMazayaOneMonth.BackcolorTooltip = Color.Red;
		txtSumMazayaOneMonth.CurrencyFormat = "";
		((Control)txtSumMazayaOneMonth).ForeColor = Color.Red;
		txtSumMazayaOneMonth.ForeColorTooltip = Color.Black;
		txtSumMazayaOneMonth.GetColoresFromSettingDLL = true;
		txtSumMazayaOneMonth.HandleKeydown = true;
		((Control)txtSumMazayaOneMonth).Location = new Point(350, 39);
		((TextBoxBase)txtSumMazayaOneMonth).MaxLength = 19;
		((Control)txtSumMazayaOneMonth).Name = "txtSumMazayaOneMonth";
		txtSumMazayaOneMonth.SendShiftTabAfterDown = true;
		txtSumMazayaOneMonth.SendShiftTabAfterEnter = true;
		txtSumMazayaOneMonth.SendTabAfterEnter = true;
		txtSumMazayaOneMonth.SendTabAfterUP = true;
		txtSumMazayaOneMonth.ShowHorofi = (ENShowHorofi)3;
		txtSumMazayaOneMonth.ShowToolTipText = true;
		((Control)txtSumMazayaOneMonth).Size = new Size(173, 21);
		((Control)txtSumMazayaOneMonth).TabIndex = 14;
		((Control)txtSumMazayaOneMonth).Text = "0";
		txtSumMazayaOneMonth.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSumMazayaOneMonth.ToolTipPostFix = "";
		txtSumMazayaOneMonth.TootTipDelay = 1000;
		txtSumMazayaOneMonth.Value = new decimal(new int[4]);
		((Control)txtSumMazayaOneMonth).TextChanged += txtSalaryOnDay_TextChanged;
		txtSalaryOnDay.AllowEmpty = true;
		((Control)txtSalaryOnDay).BackColor = SystemColors.Window;
		txtSalaryOnDay.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSalaryOnDay.BackColorTextBoxWhenLostFocus = Color.White;
		txtSalaryOnDay.BackcolorTooltip = Color.Red;
		txtSalaryOnDay.CurrencyFormat = "";
		((Control)txtSalaryOnDay).ForeColor = Color.Red;
		txtSalaryOnDay.ForeColorTooltip = Color.Black;
		txtSalaryOnDay.GetColoresFromSettingDLL = true;
		txtSalaryOnDay.HandleKeydown = true;
		((Control)txtSalaryOnDay).Location = new Point(350, 12);
		((TextBoxBase)txtSalaryOnDay).MaxLength = 19;
		((Control)txtSalaryOnDay).Name = "txtSalaryOnDay";
		txtSalaryOnDay.SendShiftTabAfterDown = true;
		txtSalaryOnDay.SendShiftTabAfterEnter = true;
		txtSalaryOnDay.SendTabAfterEnter = true;
		txtSalaryOnDay.SendTabAfterUP = true;
		txtSalaryOnDay.ShowHorofi = (ENShowHorofi)3;
		txtSalaryOnDay.ShowToolTipText = true;
		((Control)txtSalaryOnDay).Size = new Size(173, 21);
		((Control)txtSalaryOnDay).TabIndex = 5;
		((Control)txtSalaryOnDay).Text = "0";
		txtSalaryOnDay.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtSalaryOnDay.ToolTipPostFix = "";
		txtSalaryOnDay.TootTipDelay = 1000;
		txtSalaryOnDay.Value = new decimal(new int[4]);
		((Control)txtSalaryOnDay).TextChanged += txtSalaryOnDay_TextChanged;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(165, 427);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(177, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 60;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو] (Esc)";
		((Control)BtnCancel).Visible = false;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		BtnAccept.DialogResult = (DialogResult)2;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(350, 427);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(431, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 59;
		((Control)BtnAccept).Text = "[بسیار خوب ]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((BaseItemControl)labelX47).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX47).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX47).ForeColor = Color.Maroon;
		((Control)labelX47).Location = new Point(417, 67);
		((Control)labelX47).Name = "labelX47";
		((Control)labelX47).Size = new Size(14, 21);
		((Control)labelX47).TabIndex = 33;
		((Control)labelX47).Text = "*";
		labelX47.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX46).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX46).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX46).ForeColor = Color.Maroon;
		((Control)labelX46).Location = new Point(529, 148);
		((Control)labelX46).Name = "labelX46";
		((Control)labelX46).Size = new Size(14, 21);
		((Control)labelX46).TabIndex = 53;
		((Control)labelX46).Text = "*";
		labelX46.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX50).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX50).Location = new Point(787, 94);
		((Control)labelX50).Name = "labelX50";
		((Control)labelX50).Size = new Size(228, 21);
		((Control)labelX50).TabIndex = 27;
		((Control)labelX50).Text = "[حق بیمه مزایا سهم کارفرما طی قرارداد] :";
		labelX50.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX44).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX44).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX44).ForeColor = Color.Maroon;
		((Control)labelX44).Location = new Point(529, 121);
		((Control)labelX44).Name = "labelX44";
		((Control)labelX44).Size = new Size(14, 21);
		((Control)labelX44).TabIndex = 44;
		((Control)labelX44).Text = "*";
		labelX44.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(787, 67);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(228, 21);
		((Control)labelX8).TabIndex = 27;
		((Control)labelX8).Text = "[حق بیمه حقوق سهم کارفرما طی قرارداد] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX23).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX23).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX23).ForeColor = Color.Maroon;
		((Control)labelX23).Location = new Point(529, 67);
		((Control)labelX23).Name = "labelX23";
		((Control)labelX23).Size = new Size(14, 21);
		((Control)labelX23).TabIndex = 31;
		((Control)labelX23).Text = "*";
		labelX23.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX45).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX45).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX45).ForeColor = Color.Maroon;
		((Control)labelX45).Location = new Point(606, 148);
		((Control)labelX45).Name = "labelX45";
		((Control)labelX45).Size = new Size(14, 21);
		((Control)labelX45).TabIndex = 51;
		((Control)labelX45).Text = "=";
		labelX45.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX43).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX43).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX43).ForeColor = Color.Maroon;
		((Control)labelX43).Location = new Point(606, 121);
		((Control)labelX43).Name = "labelX43";
		((Control)labelX43).Size = new Size(14, 21);
		((Control)labelX43).TabIndex = 42;
		((Control)labelX43).Text = "=";
		labelX43.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX18).ForeColor = Color.Maroon;
		((Control)labelX18).Location = new Point(529, 39);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(14, 21);
		((Control)labelX18).TabIndex = 13;
		((Control)labelX18).Text = "*";
		labelX18.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX42).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX42).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX42).ForeColor = Color.Maroon;
		((Control)labelX42).Location = new Point(606, 67);
		((Control)labelX42).Name = "labelX42";
		((Control)labelX42).Size = new Size(14, 21);
		((Control)labelX42).TabIndex = 29;
		((Control)labelX42).Text = "=";
		labelX42.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX30).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX30).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX30).ForeColor = Color.Maroon;
		((Control)labelX30).Location = new Point(134, 94);
		((Control)labelX30).Name = "labelX30";
		((Control)labelX30).Size = new Size(14, 21);
		((Control)labelX30).TabIndex = 38;
		((Control)labelX30).Text = "*";
		labelX30.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX26).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX26).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX26).ForeColor = Color.Maroon;
		((Control)labelX26).Location = new Point(238, 39);
		((Control)labelX26).Name = "labelX26";
		((Control)labelX26).Size = new Size(14, 21);
		((Control)labelX26).TabIndex = 16;
		((Control)labelX26).Text = "*";
		labelX26.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX29).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX29).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX29).ForeColor = Color.Maroon;
		((Control)labelX29).Location = new Point(238, 94);
		((Control)labelX29).Name = "labelX29";
		((Control)labelX29).Size = new Size(14, 21);
		((Control)labelX29).TabIndex = 36;
		((Control)labelX29).Text = "*";
		labelX29.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX35).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX35).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX35).ForeColor = Color.Maroon;
		((Control)labelX35).Location = new Point(134, 67);
		((Control)labelX35).Name = "labelX35";
		((Control)labelX35).Size = new Size(14, 21);
		((Control)labelX35).TabIndex = 38;
		((Control)labelX35).Text = "*";
		labelX35.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX32).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX32).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX32).ForeColor = Color.Maroon;
		((Control)labelX32).Location = new Point(238, 67);
		((Control)labelX32).Name = "labelX32";
		((Control)labelX32).Size = new Size(14, 21);
		((Control)labelX32).TabIndex = 36;
		((Control)labelX32).Text = "*";
		labelX32.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX40).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX40).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX40).ForeColor = Color.Maroon;
		((Control)labelX40).Location = new Point(238, 12);
		((Control)labelX40).Name = "labelX40";
		((Control)labelX40).Size = new Size(14, 21);
		((Control)labelX40).TabIndex = 7;
		((Control)labelX40).Text = "*";
		labelX40.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX37).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX37).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX37).ForeColor = Color.Maroon;
		((Control)labelX37).Location = new Point(238, 148);
		((Control)labelX37).Name = "labelX37";
		((Control)labelX37).Size = new Size(14, 21);
		((Control)labelX37).TabIndex = 56;
		((Control)labelX37).Text = "*";
		labelX37.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX24).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX24).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX24).ForeColor = Color.Maroon;
		((Control)labelX24).Location = new Point(238, 121);
		((Control)labelX24).Name = "labelX24";
		((Control)labelX24).Size = new Size(14, 21);
		((Control)labelX24).TabIndex = 47;
		((Control)labelX24).Text = "*";
		labelX24.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX17).ForeColor = Color.Maroon;
		((Control)labelX17).Location = new Point(529, 12);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(14, 21);
		((Control)labelX17).TabIndex = 4;
		((Control)labelX17).Text = "*";
		labelX17.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX20).ForeColor = Color.Maroon;
		((Control)labelX20).Location = new Point(606, 39);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).Size = new Size(14, 21);
		((Control)labelX20).TabIndex = 11;
		((Control)labelX20).Text = "=";
		labelX20.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX16).ForeColor = Color.Maroon;
		((Control)labelX16).Location = new Point(606, 12);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(14, 21);
		((Control)labelX16).TabIndex = 2;
		((Control)labelX16).Text = "=";
		labelX16.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(787, 148);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(228, 21);
		((Control)labelX10).TabIndex = 49;
		((Control)labelX10).Text = "[عیدی] :";
		labelX10.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(787, 121);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(228, 21);
		((Control)labelX9).TabIndex = 40;
		((Control)labelX9).Text = "[سنوات] :";
		labelX9.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(787, 39);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(228, 21);
		((Control)labelX6).TabIndex = 9;
		((Control)labelX6).Text = "[جمع کل مزایا] :";
		labelX6.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(787, 12);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(228, 21);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[حقوق سالانه] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX28).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX28).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX28).Location = new Point(149, 94);
		((Control)labelX28).Name = "labelX28";
		((Control)labelX28).Size = new Size(91, 21);
		((Control)labelX28).TabIndex = 37;
		((Control)labelX28).Text = "[مزایای مشمول بیمه]";
		labelX28.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX25).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX25).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX25).Location = new Point(155, 39);
		((Control)labelX25).Name = "labelX25";
		((Control)labelX25).Size = new Size(93, 21);
		((Control)labelX25).TabIndex = 17;
		((Control)labelX25).Text = "[مزایا طی یک ماه]";
		labelX25.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX31).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX31).Location = new Point(157, 67);
		((Control)labelX31).Name = "labelX31";
		((Control)labelX31).Size = new Size(91, 21);
		((Control)labelX31).TabIndex = 37;
		((Control)labelX31).Text = "[پایه حقوق روزانه]";
		labelX31.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX39).Location = new Point(157, 12);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(91, 21);
		((Control)labelX39).TabIndex = 8;
		((Control)labelX39).Text = "[پایه حقوق روزانه]";
		labelX39.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX36).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX36).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX36).Location = new Point(157, 148);
		((Control)labelX36).Name = "labelX36";
		((Control)labelX36).Size = new Size(91, 21);
		((Control)labelX36).TabIndex = 57;
		((Control)labelX36).Text = "[پایه حقوق روزانه]";
		labelX36.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)labelX2).Location = new Point(157, 121);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(91, 21);
		((Control)labelX2).TabIndex = 48;
		((Control)labelX2).Text = "[پایه حقوق روزانه]";
		labelX2.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnAccept;
		((Form)this).ClientSize = new Size(1027, 493);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(425, 228);
		((Control)this).Name = "FrmCalOverLowTime";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[محاسبه پارامترهای حقوق طی مدت قرارداد]";
		((Form)this).Load += FrmCalOverLowTime_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)groupPanel1).ResumeLayout(false);
		((Control)groupPanel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
