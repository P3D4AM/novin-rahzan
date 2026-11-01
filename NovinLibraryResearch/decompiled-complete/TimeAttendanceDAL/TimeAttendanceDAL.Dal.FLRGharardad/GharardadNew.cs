using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Attendance.InterFaces;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Enums;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.View.Shift;

namespace TimeAttendanceDAL.Dal.FLRGharardad;

[Serializable]
[DoNotPruneType]
public class GharardadNew : IContract, IHasContext
{
	public static List<GharardadNew> Gharardads;

	public static DateTime LastUpdateTime = DateTime.Now.AddSeconds(-120.0);

	private TimeAttendanceSettings _setting;

	private List<GharardadDetNew> _listGharardadDet;

	private ShiftViewModel _shiftViewModel;

	public short _isPayMalist;

	private InsuranceGharadadEnumType _insuranceOption = (InsuranceGharadadEnumType)(-1);

	private TaxGharadadEnumType _taxOption = (TaxGharadadEnumType)(-1);

	private short _salaryType = -1;

	private short _leaveMinutes;

	private decimal priceOverTime;

	private decimal priceOverTime2;

	private decimal priceOverTime3;

	private decimal priceLowTime;

	private decimal priceLowTime2;

	private decimal priceLowTime3;

	private decimal priceMission;

	private decimal priceTatil;

	private decimal priceHoliday;

	private decimal _leaveHours;

	private short salaryType;

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public Person ClsPerson { get; set; }

	public string JsonGharardadDet { get; set; }

	public int ID { get; set; }

	public string No { get; set; } = "";


	public string Peyvast { get; set; } = "";


	public DateTime DateM { get; set; } = DateTime.Now;


	public int PersonId { get; set; }

	public short CompanyId { get; set; }

	public short ShiftId { get; set; }

	public string ShiftName { get; set; } = "";


	public string Description { get; set; } = "";


	public DateTime DateStartM { get; set; } = DateTime.Now;


	public DateTime DateFinishM { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string DateStartCalture => Calendar.MiladiToCalture(DateStartM);

	[JsonIgnore]
	public string DateFinishCalture => Calendar.MiladiToCalture(DateFinishM);

	public int? JobId { get; set; } = 0;


	public bool Married { get; set; }

	public short CountChild { get; set; }

	public short GharardadStatus { get; set; } = 1;


	public short? PartId { get; set; } = 0;


	public short? RecruitmentSt { get; set; } = 0;


	public short? RecruitmentType { get; set; } = 0;


	public short? PlaceId { get; set; } = 0;


	public short? BankType { get; set; } = 0;


	public short? BankId { get; set; } = 0;


	public string BankNo { get; set; } = "";


	public string ShabaNo { get; set; } = "";


	public string FullName
	{
		get
		{
			Person clsPerson = ClsPerson;
			return ((clsPerson != null) ? ((PersonBase)clsPerson).FullName : null) ?? "";
		}
	}

	public short SalaryType
	{
		get
		{
			return _salaryType;
		}
		set
		{
			try
			{
				short num = _salaryType;
				_salaryType = value;
				switch ((EnumsHoghogh.EnumkarkardSt)num)
				{
				case EnumsHoghogh.EnumkarkardSt.Monthly:
					switch ((EnumsHoghogh.EnumkarkardSt)value)
					{
					case EnumsHoghogh.EnumkarkardSt.Daily:
						SetSalaryPrice(SalaryOneDay);
						break;
					case EnumsHoghogh.EnumkarkardSt.Hourly:
						SetSalaryPrice(SalaryOneHour);
						break;
					}
					break;
				case EnumsHoghogh.EnumkarkardSt.Daily:
					switch ((EnumsHoghogh.EnumkarkardSt)value)
					{
					case EnumsHoghogh.EnumkarkardSt.Monthly:
						SetSalaryPrice(SalaryOneMonth);
						break;
					case EnumsHoghogh.EnumkarkardSt.Hourly:
						SetSalaryPrice(SalaryOneHour);
						break;
					}
					break;
				case EnumsHoghogh.EnumkarkardSt.Hourly:
					switch ((EnumsHoghogh.EnumkarkardSt)value)
					{
					case EnumsHoghogh.EnumkarkardSt.Monthly:
						SetSalaryPrice(SalaryOneMonth);
						break;
					case EnumsHoghogh.EnumkarkardSt.Daily:
						SetSalaryPrice(SalaryOneDay);
						break;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	[JsonIgnore]
	public string SalaryTypeName => (EnumsHoghogh.EnumkarkardSt)_salaryType switch
	{
		EnumsHoghogh.EnumkarkardSt.Daily => "روزانه", 
		EnumsHoghogh.EnumkarkardSt.Monthly => "ماهانه", 
		EnumsHoghogh.EnumkarkardSt.Hourly => "ساعتی", 
		_ => "نامشخص", 
	};

	[JsonIgnore]
	public string GharardadStatusName => GharardadStatus switch
	{
		1 => "جاری", 
		2 => "مختومه", 
		3 => "مختومه و تمدید", 
		_ => "نامشخص", 
	};

	public decimal SalaryOneHour => (SalaryOneDay / Math.Round((decimal)LeaveMinutes / 60m, 2)).RoundPrice();

	public decimal SalaryOneHourWithBaseSanavat => ((SalaryOneDay + PriceBaseYearSalary) / LeaveHours).RoundPrice();

	public decimal SalaryOneDay { get; set; }

	public decimal SalaryOneMonth => ((decimal)_setting.LaborLaw.DaysOnMonth * SalaryOneDay).RoundPrice(_setting.RoundBelowPrice);

	public decimal SalaryPrice
	{
		get
		{
			try
			{
				return (EnumsHoghogh.EnumkarkardSt)_salaryType switch
				{
					EnumsHoghogh.EnumkarkardSt.Monthly => SalaryOneMonth, 
					EnumsHoghogh.EnumkarkardSt.Daily => SalaryOneDay, 
					EnumsHoghogh.EnumkarkardSt.Hourly => SalaryOneHour, 
					_ => 0m, 
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	public decimal PriceBaseYearSalary { get; set; }

	public decimal PriceBaseYearSalaryOnHour => (PriceBaseYearSalary / LeaveHours).RoundPrice();

	public bool CalcRunTimeByLaw { get; set; }

	public decimal PriceOverTime
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceOverTime;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceOverTime = value;
		}
	}

	public decimal PriceOverTime2
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceOverTime2;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceOverTime2 = value;
		}
	}

	public decimal PriceOverTime3
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceOverTime3;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceOverTime3 = value;
		}
	}

	public decimal PriceNight { get; set; }

	public decimal PriceNobatKari1 { get; set; }

	public decimal PriceNobatKari2 { get; set; }

	public decimal PriceNobatKari3 { get; set; }

	public decimal PriceHoliday
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceHoliday;
			}
			return (SalaryOneHourWithBaseSanavat * 180m / 100m).RoundPrice();
		}
		set
		{
			priceHoliday = value;
		}
	}

	public decimal PriceTatil
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceTatil;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceTatil = value;
		}
	}

	public decimal PriceJarime { get; set; }

	public decimal PriceLeave { get; set; }

	public decimal PriceAbsent { get; set; }

	public decimal PriceMission
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceMission;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceMission = value;
		}
	}

	public decimal PriceLowTime
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceLowTime;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceLowTime = value;
		}
	}

	public decimal PriceLowTime2
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceLowTime2;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceLowTime2 = value;
		}
	}

	public decimal PriceLowTime3
	{
		get
		{
			if (!CalcRunTimeByLaw)
			{
				return priceLowTime3;
			}
			return (SalaryOneHourWithBaseSanavat * 140m / 100m).RoundPrice();
		}
		set
		{
			priceLowTime3 = value;
		}
	}

	public double LeaveDays { get; set; } = 26.0;


	public double TotalLeaveDaysInGharardad
	{
		get
		{
			if (!IsCheckoutVacation)
			{
				return LeaveDays;
			}
			return LeaveDays * (double)((DateFinishM - DateStartM).Days / 30);
		}
	}

	public double LeaveSaved { get; set; }

	public short LeaveMinutes
	{
		get
		{
			if (_leaveMinutes != 0)
			{
				return _leaveMinutes;
			}
			return 440;
		}
		set
		{
			_leaveMinutes = value;
		}
	}

	public decimal LeaveHours
	{
		get
		{
			if (LeaveMinutes == 0)
			{
				return 0m;
			}
			_leaveHours = Math.Round((decimal)LeaveMinutes / 60m, 2);
			return _leaveHours;
		}
		set
		{
			_leaveHours = value;
		}
	}

	public short Sanavat { get; set; }

	public short GroupSanavat { get; set; }

	public bool IsMotahel { get; set; }

	public string MoeenEmployerInsurance { get; set; } = "";


	public string TafsilEmployerInsurance { get; set; } = "";


	public float ZaribEmployerInsurance { get; set; } = 20f;


	public string MoeenInsuredInsurance { get; set; } = "";


	public string TafsilInsuredInsurance { get; set; } = "";


	public float ZaribInsuredInsurance { get; set; } = 7f;


	public string MoeenUnemploymentInsurance { get; set; } = "";


	public string TafsilUnemploymentInsurance { get; set; } = "";


	public bool IsCheckoutVacation { get; set; }

	[JsonIgnore]
	public float ZaribUnemploymentInsurance { get; set; } = 3f;


	[JsonIgnore]
	public InsuranceGharadadEnumType InsuranceOption
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if ((int)_insuranceOption == -1)
				{
					string text = Convert.ToString(_isPayMalist, 2);
					short num = 0;
					if (!string.IsNullOrEmpty(text))
					{
						num = Convert.ToInt16((text.Length < 4) ? text : text.Substring(text.Length - 4), 2);
					}
					_insuranceOption = (InsuranceGharadadEnumType)num;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
			return _insuranceOption;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_insuranceOption = value;
		}
	}

	[JsonIgnore]
	public string InsuranceOptionName
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected I4, but got Unknown
			try
			{
				InsuranceGharadadEnumType insuranceOption = InsuranceOption;
				return (int)insuranceOption switch
				{
					1 => "حداقل قانون کار", 
					0 => "فاقد بیمه", 
					2 => "بر اساس فیش حقوقی", 
					_ => "نامشخص", 
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	[JsonIgnore]
	public TaxGharadadEnumType TaxOption
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if ((int)_taxOption == -1)
				{
					int num = _isPayMalist >> 4;
					short num2 = 0;
					if (num > 0)
					{
						string text = Convert.ToString(num, 2);
						if (!string.IsNullOrEmpty(text))
						{
							num2 = Convert.ToInt16((text.Length < 3) ? text : text.Substring(text.Length - 3), 2);
						}
					}
					_taxOption = (TaxGharadadEnumType)num2;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
			return _taxOption;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_taxOption = value;
		}
	}

	[JsonIgnore]
	public List<GharardadDetNew> ListGharardadDet => _listGharardadDet ?? (_listGharardadDet = (string.IsNullOrEmpty(JsonGharardadDet) ? new List<GharardadDetNew>() : GharardadDetNew.Deserialize(EntityCacheConfig.CacheInstance.ContextGuid, JsonGharardadDet)));

	[JsonIgnore]
	public decimal SumMazaya => ListGharardadDet.Where((GharardadDetNew q) => q.TypeAccount == 1).Sum((GharardadDetNew s) => s.nValue);

	[JsonIgnore]
	public decimal SumKosorat => ListGharardadDet.Where((GharardadDetNew q) => q.TypeAccount == 2).Sum((GharardadDetNew s) => s.nValue);

	public decimal SumHoghMazaya => SalaryOneMonth + SumMazaya - SumKosorat;

	[JsonIgnore]
	public bool IsTemp { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public GharardadNew(string contextGuid, TimeAttendanceSettings setting)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		_setting = setting;
	}

	public void SetSalaryPrice(decimal value)
	{
		try
		{
			switch ((EnumsHoghogh.EnumkarkardSt)_salaryType)
			{
			case EnumsHoghogh.EnumkarkardSt.Monthly:
				SalaryOneDay = Math.Ceiling(value / 30m);
				break;
			case EnumsHoghogh.EnumkarkardSt.Daily:
				SalaryOneDay = value.RoundPrice();
				break;
			case EnumsHoghogh.EnumkarkardSt.Hourly:
			{
				decimal num = (value * Math.Round((decimal)LeaveMinutes / 60m, 2)).RoundPrice();
				if (Math.Abs(num - _setting.LaborLaw.DailyWage) < 10m)
				{
					num = _setting.LaborLaw.DailyWage;
				}
				SalaryOneDay = num;
				break;
			}
			default:
				SalaryOneDay = 0m;
				break;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<ShiftViewModel> GetShiftViewModelAsync(CancellationToken token)
	{
		try
		{
			ShiftViewModel shiftViewModel = _shiftViewModel;
			if (shiftViewModel == null)
			{
				shiftViewModel = (_shiftViewModel = await ShiftViewModel.GetAsync(EntityCacheConfig.CacheInstance.ContextGuid, ShiftId, token));
			}
			return shiftViewModel;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<GharardadNew> FactoryAsync(string contextGuid, int personId = 0, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			GharardadNew ret = new GharardadNew(contextGuid, await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid));
			string text = Calendar.MiladiToCalture(DateTime.Now);
			string text2 = text.Substring(0, 4) + "/01/01";
			string text3 = $"{Utilities.YearOfDateCalendar(text)}/12/{LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonthInt((short)12, Utilities.YearOfDateCalendar(text))}";
			ret.DateStartM = Utilities.CalendarToMiladi(text2);
			ret.DateFinishM = Utilities.CalendarToMiladi(text3);
			GharardadNew gharardadNew = ret;
			gharardadNew.No = (await NextGharardadAsync(contextGuid)).ToString();
			ret.LeaveMinutes = 440;
			ret.LeaveDays = 26.0;
			if (personId > 0)
			{
				ret.PersonId = personId;
			}
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private short IsPayMalist()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected I4, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected I4, but got Unknown
		try
		{
			string text = Convert.ToString((short)(int)InsuranceOption, 2).PadLeft(4, '0');
			short isPayMalist = Convert.ToInt16(Convert.ToString((short)(int)TaxOption, 2).PadLeft(3, '0') + text, 2);
			_isPayMalist = isPayMalist;
			return _isPayMalist;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token)
	{
		_ = 4;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					DateStartM = new DateTime(DateStartM.Year, DateStartM.Month, DateStartM.Day, 0, 0, 0);
					DateFinishM = new DateTime(DateFinishM.Year, DateFinishM.Month, DateFinishM.Day, 23, 59, 59);
					cmd.Parameters.AddWithValue("NO", (object)Text.FixString(No));
					cmd.Parameters.AddWithValue("Peyvast", (object)Text.FixString(Peyvast));
					cmd.Parameters.AddWithValue("DateM", (object)DateM);
					cmd.Parameters.AddWithValue("PersonID", (object)PersonId);
					cmd.Parameters.AddWithValue("CompanyID", (object)CompanyId);
					cmd.Parameters.AddWithValue("ShiftID", (object)ShiftId);
					cmd.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
					cmd.Parameters.AddWithValue("DateStartM", (object)DateStartM);
					cmd.Parameters.AddWithValue("DateFinishM", (object)DateFinishM);
					cmd.Parameters.AddWithValue("JobID", (object)JobId);
					cmd.Parameters.AddWithValue("Married", (object)Married);
					cmd.Parameters.AddWithValue("CountChild", (object)CountChild);
					cmd.Parameters.AddWithValue("PartID", (object)PartId);
					cmd.Parameters.AddWithValue("RecruitmentST", (object)RecruitmentSt);
					cmd.Parameters.AddWithValue("GharardadStatus", (object)GharardadStatus);
					cmd.Parameters.AddWithValue("RecruitmentType", (object)RecruitmentType);
					cmd.Parameters.AddWithValue("PlaceID", (object)PlaceId);
					cmd.Parameters.AddWithValue("BankType", (object)BankType);
					cmd.Parameters.AddWithValue("BankID", (object)BankId);
					cmd.Parameters.AddWithValue("BankNo", (object)BankNo);
					cmd.Parameters.AddWithValue("ShabaNo", (object)ShabaNo);
					cmd.Parameters.AddWithValue("SalaryType", (object)SalaryType);
					cmd.Parameters.AddWithValue("Salary", (object)SalaryPrice);
					cmd.Parameters.AddWithValue("PriceOverTime", (object)PriceOverTime);
					cmd.Parameters.AddWithValue("PriceOverTime2", (object)PriceOverTime2);
					cmd.Parameters.AddWithValue("PriceOverTime3", (object)PriceOverTime3);
					cmd.Parameters.AddWithValue("PriceNight", (object)PriceNight);
					cmd.Parameters.AddWithValue("PriceHoliday", (object)PriceHoliday);
					cmd.Parameters.AddWithValue("PriceTatil", (object)PriceTatil);
					cmd.Parameters.AddWithValue("PriceJarime", (object)PriceJarime);
					cmd.Parameters.AddWithValue("PriceLeave", (object)PriceLeave);
					cmd.Parameters.AddWithValue("PriceAbsent", (object)PriceAbsent);
					cmd.Parameters.AddWithValue("PriceMission", (object)PriceMission);
					cmd.Parameters.AddWithValue("PriceLowTime", (object)PriceLowTime);
					cmd.Parameters.AddWithValue("PriceLowTime2", (object)PriceLowTime2);
					cmd.Parameters.AddWithValue("PriceLowTime3", (object)PriceLowTime3);
					cmd.Parameters.AddWithValue("LeaveDays", (object)LeaveDays);
					cmd.Parameters.AddWithValue("LeaveSaved", (object)LeaveSaved);
					cmd.Parameters.AddWithValue("LeaveMinutes", (object)LeaveMinutes);
					cmd.Parameters.AddWithValue("Sanavat", (object)Sanavat);
					cmd.Parameters.AddWithValue("GroupSanavat", (object)GroupSanavat);
					cmd.Parameters.AddWithValue("isHeadFamily", (object)IsMotahel);
					cmd.Parameters.AddWithValue("MoeenEmployerInsurance", (object)MoeenEmployerInsurance);
					cmd.Parameters.AddWithValue("TafzilEmployerInsurance", (object)TafsilEmployerInsurance);
					cmd.Parameters.AddWithValue("ZaribEmployerInsurance", (object)ZaribEmployerInsurance);
					cmd.Parameters.AddWithValue("MoeenInsuredInsurance", (object)MoeenInsuredInsurance);
					cmd.Parameters.AddWithValue("TafzilInsuredInsurance", (object)TafsilInsuredInsurance);
					cmd.Parameters.AddWithValue("ZaribInsuredInsurance", (object)ZaribInsuredInsurance);
					cmd.Parameters.AddWithValue("MoeenUnemploymentInsurance", (object)MoeenUnemploymentInsurance);
					cmd.Parameters.AddWithValue("TafzilUnemploymentInsurance", (object)TafsilUnemploymentInsurance);
					cmd.Parameters.AddWithValue("ZaribUnemploymentInsurance", (object)ZaribUnemploymentInsurance);
					cmd.Parameters.AddWithValue("IsPayMalist", (object)IsPayMalist());
					cmd.Parameters.AddWithValue("IsCheckoutVacation", (object)IsCheckoutVacation);
					JsonGharardadDet = GharardadDetNew.Serialize(ListGharardadDet);
					cmd.Parameters.AddWithValue("JsonGharardadDet", (object)JsonGharardadDet);
					cmd.Parameters.AddWithValue("PriceNobatKari1", (object)PriceNobatKari1);
					cmd.Parameters.AddWithValue("PriceNobatKari2", (object)PriceNobatKari2);
					cmd.Parameters.AddWithValue("PriceNobatKari3", (object)PriceNobatKari3);
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (ID <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "sp_Gharardad_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							ID = Convert.ToInt32(obj.ToString());
						}
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "[افزودن قرارداد جدید] : " + FullName + " [و شماره قرارداد] : " + No, "Gharardad.GharardadInsert", ID.ToString(), Serialize(this));
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "sp_Gharardad_Update";
						cmd.Parameters.AddWithValue("Id", (object)ID);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
						await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, "[ویرایش قرارداد] " + FullName + " [و شماره قرارداد]: " + No, "Gharardad.GharardadUpdate", ID.ToString(), Serialize(this));
					}
					((DbConnection)(object)cn).Close();
					Reset();
					return true;
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandText = "sp_Gharardad_DeleteRow";
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)ID);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					string pDesc = "[حذف قرارداد : قرارداد] " + FullName + " [تاریخ قرارداد از] : " + Calendar.MiladiToCalture(DateStartM) + " [الی] : " + Calendar.MiladiToCalture(DateFinishM);
					await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid, pDesc, "Gharardad.GharardadDelete", ID.ToString(), Serialize(this));
					return true;
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ConvertGharardadDetToJsonAsync(string contextGuid, CancellationToken token)
	{
		try
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			foreach (GharardadNew gharardad in await GetAllAsync(contextGuid, new List<int>(), ConstantValues.MinDateM, DateTime.MaxValue, token))
			{
				gharardad._listGharardadDet = (await GharardadDet.SelectAllByGharardadAsync(contextGuid, gharardad.ID, token)).ConvertAll((GharardadDet p) => GharardadDetNew.GharardadDetToNew(contextGuid, p));
				await gharardad.SaveAsync(token);
			}
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpadetTypeAccountGharardadDetAsync(string contextGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			List<GharardadNew> listGharardads = await GetAllAsync(contextGuid, new List<int>(), ConstantValues.MinDateM, DateTime.MaxValue, token);
			List<FishHesab> qFishHesabs = await FishHesab.SelectAllAsync(contextGuid, getAllField: true, token);
			foreach (GharardadNew item2 in listGharardads)
			{
				foreach (GharardadDetNew item in item2.ListGharardadDet)
				{
					FishHesab fishHesab = qFishHesabs.FirstOrDefault((FishHesab q) => q.Id.Equals(item.HesabId));
					if (fishHesab != null)
					{
						item.TypeAccount = fishHesab.Type;
					}
				}
				await item2.SaveAsync(token);
			}
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static GharardadNew Deserialize(string jsonString)
	{
		return JsonConvert.DeserializeObject<GharardadNew>(jsonString);
	}

	public static string Serialize(GharardadNew fish)
	{
		return JsonConvert.SerializeObject((object)fish);
	}

	public static async Task<int> NextGharardadAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select (isnull((select max([dbo].[STRING2INT](NO)) from [Gharardad]),0))", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					if (obj != null)
					{
						return int.Parse(obj.ToString()) + 1;
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return 1;
	}

	public void RemoveAt(int index)
	{
		try
		{
			ListGharardadDet?.RemoveAt(index);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public void AddToList(GharardadDetNew cls)
	{
		try
		{
			cls.Order = (short)ListGharardadDet.Count;
			ListGharardadDet.Add(cls);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public GharardadDetNew GetHesabId(int pHesabId)
	{
		try
		{
			return ListGharardadDet.FirstOrDefault((GharardadDetNew p) => p.HesabId == pHesabId);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<decimal> AddNewGharardadDetAsync(int pBaseHesabOnHkgogh, decimal pZarib = 1m, CancellationToken token = default(CancellationToken))
	{
		try
		{
			decimal num = -1m;
			GharardadDetNew gharardadDetNew = ListGharardadDet.FirstOrDefault((GharardadDetNew q) => q.HesabId == pBaseHesabOnHkgogh);
			if (gharardadDetNew != null)
			{
				num = gharardadDetNew.nValue;
			}
			if (num < 0m)
			{
				GharardadDetNew gharardadDetNew2 = await GharardadDetNew.GetAsync(ContextGuid, pBaseHesabOnHkgogh, token);
				gharardadDetNew2.nValue *= pZarib;
				gharardadDetNew2.Account = gharardadDetNew2.nValue;
				AddToList(gharardadDetNew2);
				num = gharardadDetNew2.nValue;
			}
			return num;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<decimal> AddNewGharardadDetAsync(int pBaseHesabOnHkgogh, short startYear, int groupSanavat, int sanavat, CancellationToken token)
	{
		try
		{
			decimal xval = -1m;
			PriceBaseYearSalary = await BaseYearsSalary.GetPriceSanavatAsync(ContextGuid, DateStartM, DateTime.Now, groupSanavat, sanavat, token);
			using (IEnumerator<GharardadDetNew> enumerator = ListGharardadDet.Where((GharardadDetNew item) => item.HesabId == pBaseHesabOnHkgogh).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					GharardadDetNew current = enumerator.Current;
					decimal num2 = (current.nValue = (30m * PriceBaseYearSalary).RoundPrice());
					xval = num2;
				}
			}
			if (xval < 0m)
			{
				GharardadDetNew gharardadDetNew = await GharardadDetNew.GetAsync(ContextGuid, pBaseHesabOnHkgogh, token);
				gharardadDetNew.nValue = (30m * PriceBaseYearSalary).RoundPrice();
				if (gharardadDetNew.nValue > 0m)
				{
					gharardadDetNew.Account = gharardadDetNew.nValue;
					AddToList(gharardadDetNew);
				}
				xval = gharardadDetNew.nValue;
			}
			return xval;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task CalculateFormulaAsync(TaradodFull pTaradodFull, CancellationToken token)
	{
		_ = 1;
		try
		{
			List<GharardadDetNew> query = ListGharardadDet.OrderBy((GharardadDetNew q) => q.Privilege).ToList();
			FormulaList formula = await FormulaList.GetAsync(EntityCacheConfig.CacheInstance.ContextGuid, pTaradodFull, this, token);
			foreach (GharardadDetNew gharardadDetNew in query)
			{
				FormulaClass xFormulaClass = new FormulaClass(gharardadDetNew.Formula, gharardadDetNew.Privilege, gharardadDetNew.Desc);
				if (gharardadDetNew.nValue != 0m)
				{
					xFormulaClass.Value = gharardadDetNew.nValue;
				}
				formula.FormulaClasses.Add(xFormulaClass);
				if (xFormulaClass.Value == -1m)
				{
					FormulaClass formulaClass = xFormulaClass;
					formulaClass.Value = await formula.GetValueAsync(xFormulaClass, token);
				}
				gharardadDetNew.nValue = xFormulaClass.Value;
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<Company> GetCompanyAsync(string contextGuid, CancellationToken token)
	{
		return await Company.GetAsync(contextGuid, CompanyId, token);
	}

	public static async Task<ReturnedSaveFuncInfo> CopyGharardadForPersonAsync(string contextGuid, List<int> lstPersonIds, GharardadNew sampleGharardad, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			foreach (int lstPersonId in lstPersonIds)
			{
				sampleGharardad.PersonId = lstPersonId;
				sampleGharardad.ID = 0;
				sampleGharardad.No = (await NextGharardadAsync(contextGuid, token)).ToString();
				ReturnedSaveFuncInfo val = await sampleGharardad.CheckValidationAsync(token);
				if (val.HasError)
				{
					result = val;
				}
				else
				{
					await sampleGharardad.SaveAsync(token);
				}
			}
			return result;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(No))
			{
				result.AddReturnedValue((ReturnedState)2, "[لطفا شماره قرارداد را وارد کنید.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1074);
			}
			if (CompanyId <= 0)
			{
				result.AddReturnedValue((ReturnedState)2, "[تعریف شرکت الزامی است.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1078);
			}
			if (ShiftId < -1)
			{
				result.AddReturnedValue((ReturnedState)4, "[نوع شیفت تعریف نشده است.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1082);
			}
			if (ShiftId == 0)
			{
				result.AddReturnedValue((ReturnedState)1, "[نوع شیفت تعریف نشده است.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1085);
			}
			if (PersonId <= 0)
			{
				result.AddReturnedValue((ReturnedState)2, "[تعریف پرسنل الزامی است.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1089);
			}
			if (LeaveMinutes < 60)
			{
				result.AddReturnedValue((ReturnedState)2, "[مقدار مرخص ساعتی معادل یک روز کاری نباید کمتر از یک ساعت باشد.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1093);
			}
			if (DateStartM > DateFinishM)
			{
				result.AddReturnedValue((ReturnedState)2, "[تاریخ شروع قرارداد نباید از تاریخ پایان قرارداد بیشتر باشد.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1097);
			}
			GharardadNew gharardadNew = await GetAsync(ContextGuid, PersonId, DateStartM, DateFinishM, token);
			if (gharardadNew != null && gharardadNew.ID > 0 && ID != gharardadNew?.ID)
			{
				result.AddReturnedValue((ReturnedState)2, "[بازه تاریخ شروع قرارداد تا تاریخ پایان قرارداد با قرارداد قبلی شخص تداخل دارد ]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1102);
			}
			if (SalaryType == 0 && SalaryPrice != 0m)
			{
				result.AddReturnedValue((ReturnedState)2, "[نوع پرداخت حقوق را مشخص نمائید]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1106);
			}
			if (ShiftId == 0 && ListGharardadDet.Any((GharardadDetNew q) => q.IsDecrease))
			{
				result.AddReturnedValue((ReturnedState)2, "[چون نوع شیفت تعریف نشده است مزایا و کسورات نمیتوانند بر اساس کارکرد محاسبه شوند.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1110);
			}
			if (ShiftId == 0 && SalaryType != 3)
			{
				result.AddReturnedValue((ReturnedState)2, "[چون نوع شیفت تعریف نشده است نوع پرداخت حقوق باید ساعتی باشد.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1114);
			}
			if ((from q in await AgreedDeduction.GetAllAsync(EntityCacheConfig.CacheInstance.ContextGuid, PersonId.ToString(), DateStartM, DateFinishM, token)
				join p in ListGharardadDet on q.HesabId equals p.HesabId
				select q).ToList().Count > 0)
			{
				result.AddReturnedValue((ReturnedState)2, "[برخی از کسورات در کسورات توافقی استفاده شده است و نمیتوانید در اینجا استفاده نمائید.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1120);
			}
			bool res2 = true;
			if (ListGharardadDet.Any((GharardadDetNew q) => q.HesabId == 6))
			{
				res2 = false;
				int num = Utilities.YearOfDateCalendar(Calendar.MiladiToCalture(DateStartM));
				if (await TaxTable.RecordCountAsync(EntityCacheConfig.CacheInstance.ContextGuid, (short)num, token) > 0)
				{
					res2 = true;
				}
			}
			if (!res2)
			{
				result.AddReturnedValue((ReturnedState)4, "[گزینه مالیات را انتخاب کرده اید ولی جدول مالیاتی برای این سال تهیه نشده است.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1134);
			}
			if (LeaveDays < 0.0)
			{
				result.AddReturnedValue((ReturnedState)2, "[تعداد روزهای مرخصی در بازه قرارداد نمیتواند منفی باشد.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1138);
			}
			if (IsCheckoutVacation && PriceLeave <= 0m)
			{
				result.AddReturnedValue((ReturnedState)4, "[چون مانده مرخصی درپایان هر ماه تسویه میشود بنابراین فیلد مرخصی ساعتی بایستی مقدار داشته باشد.]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1142);
			}
			if (SalaryType > 0 && SalaryPrice <= 0m)
			{
				result.AddReturnedValue((ReturnedState)2, "[مبلغ حقوق را وارد نمائید]", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRGharardad\\GharardadNew.cs", 1146);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return result;
	}

	public static async Task<GharardadNew> GetAsync(string contextGuid, int personId, DateTime dateStartM, DateTime dateFinishM, CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			GharardadNew ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_Gharardad_SelectRowByPersonIdAndDate", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@personId", (object)personId);
					cmd.Parameters.AddWithValue("@dateStartM", (object)dateStartM);
					cmd.Parameters.AddWithValue("@dateFinishM", (object)dateFinishM);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = await LoadDataAsync(contextGuid, dr, await Person.GetAllAsync(contextGuid, new List<int>(1) { personId }, personStatus: true), token);
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						return ret;
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static Task<GharardadNew> GetAsync(string contextGuid, int personId, DateTime date, CancellationToken token = default(CancellationToken))
	{
		return GetAsync(contextGuid, personId, date, date, token);
	}

	private static async Task<GharardadNew> LoadDataAsync(string contextGuid, SqlDataReader dr, List<Person> persons, CancellationToken token)
	{
		_ = 1;
		try
		{
			GharardadNew gharardad = new GharardadNew(contextGuid, await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid));
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				gharardad.ID = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["NO"].ToString() != DBNull.Value.ToString())
			{
				gharardad.No = ((DbDataReader)(object)dr)["NO"].ToString();
			}
			if (((DbDataReader)(object)dr)["Peyvast"].ToString() != DBNull.Value.ToString())
			{
				gharardad.Peyvast = ((DbDataReader)(object)dr)["Peyvast"].ToString();
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				gharardad.DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["PersonID"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PersonId = (int)((DbDataReader)(object)dr)["PersonID"];
			}
			GharardadNew gharardadNew = gharardad;
			Person clsPerson = ((persons != null) ? persons.SingleOrDefault((Person p) => ((PersonBase)p).Id == gharardad.PersonId) : (await Person.GetByPersonIdAsync(contextGuid, gharardad.PersonId)));
			gharardadNew.ClsPerson = clsPerson;
			if (((DbDataReader)(object)dr)["GharardadStatus"].ToString() != DBNull.Value.ToString())
			{
				gharardad.GharardadStatus = (short)((DbDataReader)(object)dr)["GharardadStatus"];
			}
			if (((DbDataReader)(object)dr)["CompanyID"].ToString() != DBNull.Value.ToString())
			{
				gharardad.CompanyId = (short)((DbDataReader)(object)dr)["CompanyID"];
			}
			if (((DbDataReader)(object)dr)["ShiftID"].ToString() != DBNull.Value.ToString())
			{
				gharardad.ShiftId = (short)((DbDataReader)(object)dr)["ShiftID"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				gharardad.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["DateStartM"].ToString() != DBNull.Value.ToString())
			{
				gharardad.DateStartM = (DateTime)((DbDataReader)(object)dr)["DateStartM"];
			}
			if (((DbDataReader)(object)dr)["DateFinishM"].ToString() != DBNull.Value.ToString())
			{
				gharardad.DateFinishM = (DateTime)((DbDataReader)(object)dr)["DateFinishM"];
			}
			if (((DbDataReader)(object)dr)["JobID"].ToString() != DBNull.Value.ToString())
			{
				gharardad.JobId = (int)((DbDataReader)(object)dr)["JobID"];
			}
			if (((DbDataReader)(object)dr)["Married"].ToString() != DBNull.Value.ToString())
			{
				gharardad.Married = (bool)((DbDataReader)(object)dr)["Married"];
			}
			if (((DbDataReader)(object)dr)["CountChild"].ToString() != DBNull.Value.ToString())
			{
				gharardad.CountChild = (short)((DbDataReader)(object)dr)["CountChild"];
			}
			if (((DbDataReader)(object)dr)["PartID"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PartId = (short)((DbDataReader)(object)dr)["PartID"];
			}
			if (((DbDataReader)(object)dr)["RecruitmentST"].ToString() != DBNull.Value.ToString())
			{
				gharardad.RecruitmentSt = (short)((DbDataReader)(object)dr)["RecruitmentST"];
			}
			if (((DbDataReader)(object)dr)["RecruitmentType"].ToString() != DBNull.Value.ToString())
			{
				gharardad.RecruitmentType = (short)((DbDataReader)(object)dr)["RecruitmentType"];
			}
			if (((DbDataReader)(object)dr)["PlaceID"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PlaceId = (short)((DbDataReader)(object)dr)["PlaceID"];
			}
			if (((DbDataReader)(object)dr)["BankType"].ToString() != DBNull.Value.ToString())
			{
				gharardad.BankType = (short)((DbDataReader)(object)dr)["BankType"];
			}
			if (((DbDataReader)(object)dr)["BankID"].ToString() != DBNull.Value.ToString())
			{
				gharardad.BankId = (short)((DbDataReader)(object)dr)["BankID"];
			}
			if (((DbDataReader)(object)dr)["BankNo"].ToString() != DBNull.Value.ToString())
			{
				gharardad.BankNo = ((DbDataReader)(object)dr)["BankNo"].ToString();
			}
			if (((DbDataReader)(object)dr)["ShabaNo"].ToString() != DBNull.Value.ToString())
			{
				gharardad.ShabaNo = ((DbDataReader)(object)dr)["ShabaNo"].ToString();
			}
			if (((DbDataReader)(object)dr)["SalaryType"].ToString() != DBNull.Value.ToString())
			{
				gharardad._salaryType = (short)((DbDataReader)(object)dr)["SalaryType"];
			}
			if (((DbDataReader)(object)dr)["LeaveMinutes"].ToString() != DBNull.Value.ToString())
			{
				gharardad.LeaveMinutes = (short)((DbDataReader)(object)dr)["LeaveMinutes"];
			}
			if (((DbDataReader)(object)dr)["Salary"].ToString() != DBNull.Value.ToString())
			{
				gharardad.SetSalaryPrice((decimal)((DbDataReader)(object)dr)["Salary"]);
			}
			if (((DbDataReader)(object)dr)["PriceOverTime"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceOverTime = (decimal)((DbDataReader)(object)dr)["PriceOverTime"];
			}
			if (((DbDataReader)(object)dr)["PriceOverTime2"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceOverTime2 = (decimal)((DbDataReader)(object)dr)["PriceOverTime2"];
			}
			if (((DbDataReader)(object)dr)["PriceOverTime3"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceOverTime3 = (decimal)((DbDataReader)(object)dr)["PriceOverTime3"];
			}
			if (((DbDataReader)(object)dr)["PriceNight"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceNight = (decimal)((DbDataReader)(object)dr)["PriceNight"];
			}
			if (((DbDataReader)(object)dr)["PriceNobatKari1"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceNobatKari1 = (decimal)((DbDataReader)(object)dr)["PriceNobatKari1"];
			}
			if (((DbDataReader)(object)dr)["PriceNobatKari2"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceNobatKari2 = (decimal)((DbDataReader)(object)dr)["PriceNobatKari2"];
			}
			if (((DbDataReader)(object)dr)["PriceNobatKari3"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceNobatKari3 = (decimal)((DbDataReader)(object)dr)["PriceNobatKari3"];
			}
			if (((DbDataReader)(object)dr)["PriceHoliday"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceHoliday = (decimal)((DbDataReader)(object)dr)["PriceHoliday"];
			}
			if (((DbDataReader)(object)dr)["PriceTatil"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceTatil = (decimal)((DbDataReader)(object)dr)["PriceTatil"];
			}
			if (((DbDataReader)(object)dr)["PriceJarime"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceJarime = (decimal)((DbDataReader)(object)dr)["PriceJarime"];
			}
			if (((DbDataReader)(object)dr)["PriceLeave"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceLeave = (decimal)((DbDataReader)(object)dr)["PriceLeave"];
			}
			if (((DbDataReader)(object)dr)["PriceAbsent"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceAbsent = (decimal)((DbDataReader)(object)dr)["PriceAbsent"];
			}
			if (((DbDataReader)(object)dr)["PriceMission"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceMission = (decimal)((DbDataReader)(object)dr)["PriceMission"];
			}
			if (((DbDataReader)(object)dr)["PriceLowTime"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceLowTime = (decimal)((DbDataReader)(object)dr)["PriceLowTime"];
			}
			if (((DbDataReader)(object)dr)["PriceLowTime2"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceLowTime2 = (decimal)((DbDataReader)(object)dr)["PriceLowTime2"];
			}
			if (((DbDataReader)(object)dr)["PriceLowTime3"].ToString() != DBNull.Value.ToString())
			{
				gharardad.PriceLowTime3 = (decimal)((DbDataReader)(object)dr)["PriceLowTime3"];
			}
			if (((DbDataReader)(object)dr)["LeaveDays"].ToString() != DBNull.Value.ToString())
			{
				gharardad.LeaveDays = (double)((DbDataReader)(object)dr)["LeaveDays"];
			}
			if (((DbDataReader)(object)dr)["LeaveSaved"].ToString() != DBNull.Value.ToString())
			{
				gharardad.LeaveSaved = (double)((DbDataReader)(object)dr)["LeaveSaved"];
			}
			if (((DbDataReader)(object)dr)["Sanavat"].ToString() != DBNull.Value.ToString())
			{
				gharardad.Sanavat = (short)((DbDataReader)(object)dr)["Sanavat"];
			}
			if (((DbDataReader)(object)dr)["GroupSanavat"].ToString() != DBNull.Value.ToString())
			{
				gharardad.GroupSanavat = (short)((DbDataReader)(object)dr)["GroupSanavat"];
			}
			if (((DbDataReader)(object)dr)["isHeadFamily"].ToString() != DBNull.Value.ToString())
			{
				gharardad.IsMotahel = (bool)((DbDataReader)(object)dr)["isHeadFamily"];
			}
			if (((DbDataReader)(object)dr)["MoeenEmployerInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.MoeenEmployerInsurance = ((DbDataReader)(object)dr)["MoeenEmployerInsurance"].ToString();
			}
			if (((DbDataReader)(object)dr)["TafzilEmployerInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.TafsilEmployerInsurance = ((DbDataReader)(object)dr)["TafzilEmployerInsurance"].ToString();
			}
			if (((DbDataReader)(object)dr)["ZaribEmployerInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.ZaribEmployerInsurance = float.Parse(((DbDataReader)(object)dr)["ZaribEmployerInsurance"].ToString());
			}
			if (((DbDataReader)(object)dr)["MoeenInsuredInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.MoeenInsuredInsurance = ((DbDataReader)(object)dr)["MoeenInsuredInsurance"].ToString();
			}
			if (((DbDataReader)(object)dr)["TafzilInsuredInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.TafsilInsuredInsurance = ((DbDataReader)(object)dr)["TafzilInsuredInsurance"].ToString();
			}
			if (((DbDataReader)(object)dr)["ZaribInsuredInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.ZaribInsuredInsurance = float.Parse(((DbDataReader)(object)dr)["ZaribInsuredInsurance"].ToString());
			}
			if (((DbDataReader)(object)dr)["MoeenUnemploymentInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.MoeenUnemploymentInsurance = ((DbDataReader)(object)dr)["MoeenUnemploymentInsurance"].ToString();
			}
			if (((DbDataReader)(object)dr)["TafzilUnemploymentInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.TafsilUnemploymentInsurance = ((DbDataReader)(object)dr)["TafzilUnemploymentInsurance"].ToString();
			}
			if (((DbDataReader)(object)dr)["ZaribUnemploymentInsurance"].ToString() != DBNull.Value.ToString())
			{
				gharardad.ZaribUnemploymentInsurance = float.Parse(((DbDataReader)(object)dr)["ZaribUnemploymentInsurance"].ToString());
			}
			if (((DbDataReader)(object)dr)["IsPayMalist"].ToString() != DBNull.Value.ToString())
			{
				gharardad._isPayMalist = (short)((DbDataReader)(object)dr)["IsPayMalist"];
			}
			if (((DbDataReader)(object)dr)["JsonGharardadDet"].ToString() != DBNull.Value.ToString())
			{
				gharardad.JsonGharardadDet = ((DbDataReader)(object)dr)["JsonGharardadDet"].ToString();
			}
			if (((DbDataReader)(object)dr)["IsCheckoutVacation"].ToString() != DBNull.Value.ToString())
			{
				gharardad.IsCheckoutVacation = (bool)((DbDataReader)(object)dr)["IsCheckoutVacation"];
			}
			return gharardad;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static async Task<GharardadNew> GetAsync(string contextGuid, int id, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			GharardadNew ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_Gharardad_SelectRow", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@Id", (object)id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = await LoadDataAsync(contextGuid, dr, null, token);
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						return ret;
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static void Reset()
	{
		LastUpdateTime = DateTime.Now.AddSeconds(-120.0);
	}

	public static async Task<List<GharardadNew>> GetAllAsync(string contextGuid, List<int> listPersonId, DateTime GStartDate, DateTime GEndDate, CancellationToken token)
	{
		try
		{
			if (LastUpdateTime.AddSeconds(60.0) < DateTime.Now)
			{
				LastUpdateTime = DateTime.Now;
				List<GharardadNew> ret = new List<GharardadNew>();
				SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
				try
				{
					SqlCommand val = new SqlCommand("[sp_Gharardad_SelectAll]", cn);
					((DbCommand)val).CommandType = CommandType.StoredProcedure;
					SqlCommand cmd = val;
					try
					{
						cmd.Parameters.AddWithValue("lstPersonIds", (object)"");
						cmd.Parameters.AddWithValue("startDate", (object)DateTime.Now.AddYears(-10));
						cmd.Parameters.AddWithValue("endDate", (object)DateTime.Now.AddYears(10));
						await ((DbConnection)(object)cn).OpenAsync(token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
						try
						{
							if (((DbDataReader)(object)dr).HasRows)
							{
								List<Person> persons = await Person.GetAllAsync(contextGuid, listPersonId, personStatus: true, token);
								while (await ((DbDataReader)(object)dr).ReadAsync(token))
								{
									token.ThrowIfCancellationRequested();
									List<GharardadNew> list = ret;
									list.Add(await LoadDataAsync(contextGuid, dr, persons, token));
								}
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)cn).Close();
							Gharardads = ret;
						}
						finally
						{
							((IDisposable)dr)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)cmd)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cn)?.Dispose();
				}
			}
			List<int> list2 = listPersonId;
			return (list2 != null && list2.Count > 0) ? Gharardads.Where((GharardadNew p) => listPersonId.Contains(p.PersonId) && ((p.DateStartM <= GStartDate && p.DateFinishM >= GEndDate) || (p.DateFinishM >= GStartDate && p.DateFinishM <= GEndDate) || (p.DateStartM >= GStartDate && p.DateStartM <= GEndDate))).ToList() : Gharardads.Where((GharardadNew p) => (p.DateStartM <= GStartDate && p.DateFinishM >= GEndDate) || (p.DateFinishM >= GStartDate && p.DateFinishM <= GEndDate) || (p.DateStartM >= GStartDate && p.DateStartM <= GEndDate)).ToList();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}
}
