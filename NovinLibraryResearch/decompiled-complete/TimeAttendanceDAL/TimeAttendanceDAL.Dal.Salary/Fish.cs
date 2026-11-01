using System;
using System.Collections;
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
using PacketParser.Entities;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Enums;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class Fish
{
	private TimeAttendanceSettings _setting;

	private short? _countKarKerd;

	private short? _countKarKerdBime;

	private short? _countKarKerdMaliat;

	private short? _countDays;

	private short? _countShift;

	private short? _countVacationWithHoghogh;

	private short? _countVacationNotHoghogh;

	private short? _CountMorakhasi;

	private short? _countGheybat;

	private short? _countNaghes;

	private short? _countTatil;

	private List<FishDetJson> _listFishDet;

	private Per_WorkTitle _perWorkTitle;

	private string _jobTitle;

	private string _jobCode;

	private List<AgreedDeduction> _agreedDeducations;

	private CodeingAccounting _codeacc;

	private short? _countWorkdayHoghogh;

	private double _vacationRemind = -100.0;

	private Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> _morakhasiItems;

	private string _RayVarzTaradodKardex;

	private decimal tax;

	private int? _moazafiMin;

	private int? _gheybatMin;

	public GharardadNew ClsGharardad { get; private set; }

	public Person ClsPerson { get; private set; }

	public TaradodFull ClsTaradod { get; set; }

	public string FullName
	{
		get
		{
			Person clsPerson = ClsPerson;
			return ((clsPerson != null) ? ((PersonBase)clsPerson).FullName : null) ?? "";
		}
	}

	public string ContextGuid { get; private set; }

	public int Id { get; private set; }

	public int PersonId { get; set; }

	public int FishNo { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string DateSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateM);

	[JsonIgnore]
	public string DateEndSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateEndM);

	[JsonIgnore]
	public string DateStartSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateStartM);

	public DateTime DateStartM { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string FishMonthYear => Calendar.MiladiToCalture(DateStartM).Substring(0, 7);

	public DateTime DateEndM { get; set; } = DateTime.MaxValue;


	public short Uid { get; set; }

	public int? IdSanad { get; set; }

	public string Description { get; set; } = "";


	public short St { get; set; }

	public short VacationST { get; set; }

	public short EditST { get; set; }

	public int KarkerdMin { get; set; }

	public decimal KarkerdPrice { get; set; }

	public int EzafeMin { get; set; }

	public decimal EzafePrice { get; set; }

	public int EzafeMin1 { get; set; }

	public decimal EzafePrice1 { get; set; }

	public int EzafeMin2 { get; set; }

	public decimal EzafePrice2 { get; set; }

	public int EzafeMin3 { get; set; }

	public decimal EzafePrice3 { get; set; }

	public int ShabMin { get; set; }

	public decimal ShabPrice { get; set; }

	public int NobatMin1 { get; set; }

	public int NobatMin2 { get; set; }

	public int NobatMin3 { get; set; }

	public decimal NobatPrice1 { get; set; }

	public decimal NobatPrice2 { get; set; }

	public decimal NobatPrice3 { get; set; }

	public int JomeMin { get; set; }

	public int TatilMin { get; set; }

	public int MoazafiMin
	{
		get
		{
			int valueOrDefault = _moazafiMin.GetValueOrDefault();
			if (!_moazafiMin.HasValue)
			{
				valueOrDefault = ClsTaradod?.SumMoazzafi ?? 0;
				_moazafiMin = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_moazafiMin = value;
		}
	}

	public int JarimeRoz { get; set; }

	public decimal JomePrice { get; set; }

	public decimal TatilPrice { get; set; }

	public decimal JarimePrice { get; set; }

	public int VacationMin { get; set; }

	public int VacationMinUsed { get; set; }

	public decimal VacationPrice { get; set; }

	public int GheybatRoze { get; set; }

	public int GheybatMin
	{
		get
		{
			int valueOrDefault = _gheybatMin.GetValueOrDefault();
			if (!_gheybatMin.HasValue)
			{
				valueOrDefault = ClsTaradod?.SumGheybatMin ?? 0;
				_gheybatMin = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_gheybatMin = value;
		}
	}

	public decimal GheybatPrice { get; set; }

	public double MissionRoz { get; set; }

	public decimal MissionPrice { get; set; }

	public int KasrMin { get; set; }

	public decimal KasrPrice { get; set; }

	public int KasrMin1 { get; set; }

	public decimal KasrPrice1 { get; set; }

	public int KasrMin2 { get; set; }

	public decimal KasrPrice2 { get; set; }

	public int KasrMin3 { get; set; }

	public decimal KasrPrice3 { get; set; }

	public short IsPrint { get; set; }

	public decimal SumKarkerd
	{
		get
		{
			try
			{
				if (ClsGharardad == null)
				{
					return 0m;
				}
				return St switch
				{
					3 => (ClsGharardad.ShiftId == 0) ? ((decimal)KarkerdMin / 60m * KarkerdPrice).RoundPrice() : ((decimal)MoazafiMin / 60m * KarkerdPrice).RoundPrice(), 
					2 => ((decimal)CountKarKerd * KarkerdPrice).RoundPrice(), 
					_ => KarkerdPrice.RoundPrice(), 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SumKarkerd", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 224);
			}
			return 0m;
		}
	}

	public decimal SumNobatKari1 => ((decimal)NobatMin1 / 60m * NobatPrice1).RoundPrice();

	public decimal SumNobatKari2 => ((decimal)NobatMin2 / 60m * NobatPrice2).RoundPrice();

	public decimal SumNobatKari3 => ((decimal)NobatMin3 / 60m * NobatPrice3).RoundPrice();

	public decimal SumVacation
	{
		get
		{
			try
			{
				if (VacationST == 0)
				{
					return 0m;
				}
				return ((decimal)VacationMin / 60m * VacationPrice).RoundPrice();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SumVacation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 251);
			}
			return 0m;
		}
	}

	public decimal SumJomeKari => ((decimal)JomeMin / 60m * JomePrice).RoundPrice();

	public decimal SumTatilKari => ((decimal)TatilMin / 60m * TatilPrice).RoundPrice();

	public decimal SumGheybat => ((decimal)GheybatRoze * GheybatPrice).RoundPrice();

	public decimal SumMission => ((decimal)MissionRoz * MissionPrice).RoundPrice();

	public decimal SumJarime => ((decimal)JarimeRoz * JarimePrice).RoundPrice();

	public decimal SumShabKari => ((decimal)ShabMin / 60m * ShabPrice).RoundPrice();

	public decimal SumEzafeKar => SumEzafeKar1 + SumEzafeKar2 + SumEzafeKar3;

	public decimal SumEzafeKar1 => ((decimal)EzafeMin1 / 60m * EzafePrice1).RoundPrice();

	public decimal SumEzafeKar2 => ((decimal)EzafeMin2 / 60m * EzafePrice2).RoundPrice();

	public decimal SumEzafeKar3 => ((decimal)EzafeMin3 / 60m * EzafePrice3).RoundPrice();

	public decimal SumKasrKar => SumKasrKar1 + SumKasrKar2 + SumKasrKar3;

	public decimal SumKasrKar1 => ((decimal)KasrMin1 / 60m * KasrPrice1).RoundPrice();

	public decimal SumKasrKar2 => ((decimal)KasrMin2 / 60m * KasrPrice2).RoundPrice();

	public decimal SumKasrKar3 => ((decimal)KasrMin3 / 60m * KasrPrice3).RoundPrice();

	public decimal SumMazaya => ListFishDet.Where((FishDetJson q) => q.TypeAccount == 1).Sum((FishDetJson p) => p.TmpValue);

	public decimal SumKosorat => ListFishDet.Where((FishDetJson q) => q.TypeAccount == 2).Sum((FishDetJson p) => p.TmpValue);

	public string JsonFishDet { get; private set; }

	public decimal SumShabKar => (decimal)ShabMin * ShabPrice;

	public decimal SumNobatKar
	{
		get
		{
			try
			{
				return (decimal)NobatMin1 * NobatPrice1 + (decimal)NobatMin2 * NobatPrice2 + (decimal)NobatMin3 * NobatPrice3;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				return 0m;
			}
		}
	}

	public decimal AVGNobatKary
	{
		get
		{
			try
			{
				int num = NobatMin1 + NobatMin2 + NobatMin3;
				if (num != 0)
				{
					return SumNobatKar / (decimal)num;
				}
				return 0m;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				return 0m;
			}
		}
	}

	public decimal SumJomeKary => (decimal)JomeMin * JomePrice;

	public decimal SumKhales => (GetSumKolKarkerd() + SumMazaya - SumKosorat).RoundPrice(_setting.RoundBelowPrice);

	[JsonIgnore]
	public List<FishDetJson> ListFishDet => _listFishDet ?? (_listFishDet = (string.IsNullOrEmpty(JsonFishDet) ? new List<FishDetJson>() : FishDetJson.Deserialize(JsonFishDet, EntityCacheConfig.CacheInstance.ContextGuid)));

	[JsonIgnore]
	public List<object> ListFishDetForReport
	{
		get
		{
			List<FishDetJson> listFishDet = ListFishDet;
			List<object> list = new List<object>();
			try
			{
				if (listFishDet != null && listFishDet.Count > 0)
				{
					list.AddRange(listFishDet.Select((FishDetJson item) => new FishDetJsonForHoghoghReport(item, FishNo)));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ListFishDetForReport", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 350);
			}
			return list;
		}
	}

	public short CountKarKerd
	{
		get
		{
			short valueOrDefault = _countKarKerd.GetValueOrDefault();
			if (!_countKarKerd.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountKarkerd ?? 0;
				_countKarKerd = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countKarKerd = value;
		}
	}

	public short CountWorkdayHoghogh
	{
		get
		{
			short valueOrDefault = _countWorkdayHoghogh.GetValueOrDefault();
			if (!_countWorkdayHoghogh.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountWorkdayHoghogh ?? 0;
				_countWorkdayHoghogh = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countWorkdayHoghogh = value;
		}
	}

	public short CountDaysOfMonth
	{
		get
		{
			short valueOrDefault = _countDays.GetValueOrDefault();
			if (!_countDays.HasValue)
			{
				valueOrDefault = (short)Calendar.EndDayOfMonth(DateEndSh);
				_countDays = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	public short Countshift
	{
		get
		{
			short valueOrDefault = _countShift.GetValueOrDefault();
			if (!_countShift.HasValue)
			{
				valueOrDefault = ClsTaradod?.Countshift ?? 0;
				_countShift = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countShift = value;
		}
	}

	public short CountMorakhasiWithSalary
	{
		get
		{
			short valueOrDefault = _countVacationWithHoghogh.GetValueOrDefault();
			if (!_countVacationWithHoghogh.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountMorakhasiWithSalary ?? 0;
				_countVacationWithHoghogh = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countVacationWithHoghogh = value;
		}
	}

	public short CountWorkdayBime
	{
		get
		{
			short valueOrDefault = _countKarKerdBime.GetValueOrDefault();
			if (!_countKarKerdBime.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountWorkdayBime ?? 0;
				_countKarKerdBime = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countKarKerdBime = value;
		}
	}

	public short CountWorkdayMaliyat
	{
		get
		{
			short valueOrDefault = _countKarKerdMaliat.GetValueOrDefault();
			if (!_countKarKerdMaliat.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountWorkdayMaliyat ?? 0;
				_countKarKerdMaliat = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countKarKerdMaliat = value;
		}
	}

	public short CountMorakhasiNotSalary
	{
		get
		{
			short valueOrDefault = _countVacationNotHoghogh.GetValueOrDefault();
			if (!_countVacationNotHoghogh.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountMorakhasiNotSalary ?? 0;
				_countVacationNotHoghogh = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countVacationNotHoghogh = value;
		}
	}

	public short CountMorakhasi
	{
		get
		{
			short valueOrDefault = _CountMorakhasi.GetValueOrDefault();
			if (!_CountMorakhasi.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountMorakhasi ?? 0;
				_CountMorakhasi = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_CountMorakhasi = value;
		}
	}

	public int MorakhasiEstelajiMin => (ClsTaradod?.CountMorakhasiEstelaji * ClsGharardad?.LeaveMinutes).GetValueOrDefault();

	public int MorakhasiEstehghaghiMin => (ClsTaradod?.CountMorakhasiEstehghaghi * ClsGharardad?.LeaveMinutes).GetValueOrDefault();

	public int MorakhasiNotSalaryMin => (ClsTaradod?.CountMorakhasiNotSalary * ClsGharardad?.LeaveMinutes).GetValueOrDefault();

	public short CountGheybat
	{
		get
		{
			short valueOrDefault = _countGheybat.GetValueOrDefault();
			if (!_countGheybat.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountGheybat ?? 0;
				_countGheybat = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countGheybat = value;
		}
	}

	public short CountNaghes
	{
		get
		{
			short valueOrDefault = _countNaghes.GetValueOrDefault();
			if (!_countNaghes.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountNaghes ?? 0;
				_countNaghes = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countNaghes = value;
		}
	}

	public short CountTatil
	{
		get
		{
			short valueOrDefault = _countTatil.GetValueOrDefault();
			if (!_countTatil.HasValue)
			{
				valueOrDefault = ClsTaradod?.CountTatil ?? 0;
				_countTatil = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
		set
		{
			_countTatil = value;
		}
	}

	public decimal FactorByInsurance
	{
		get
		{
			try
			{
				decimal num = 1m;
				num = (decimal)CountWorkdayBime / (decimal)CountDaysOfMonth;
				if (num > 1m)
				{
					num = 1m;
				}
				return Math.Round(num, 3);
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FactorByInsurance", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 805);
				throw;
			}
		}
	}

	public decimal FactorByTax
	{
		get
		{
			try
			{
				decimal num = 1m;
				num = (decimal)CountWorkdayMaliyat / (decimal)CountDaysOfMonth;
				if (num > 1m)
				{
					num = 1m;
				}
				return Math.Round(num, 3);
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FactorByTax", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 824);
				throw;
			}
		}
	}

	public decimal FactorByKarkardMin
	{
		get
		{
			try
			{
				decimal num = (decimal)KarkerdMin / (decimal)ClsGharardad.LeaveMinutes / (decimal)CountDaysOfMonth;
				if (num > 1m)
				{
					num = 1m;
				}
				return Math.Round(num, 3);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	public decimal FactorByCountKarkard
	{
		get
		{
			try
			{
				decimal num = 1m;
				num = (decimal)CountKarKerd / (decimal)CountDaysOfMonth;
				if (num > 1m)
				{
					num = 1m;
				}
				return Math.Round(num, 3);
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FactorByCountKarkard", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 861);
				throw;
			}
		}
	}

	public async Task<double> GetVacationRemindAaync(CancellationToken token)
	{
		_ = 2;
		try
		{
			await Task.Delay(100);
			if (token.IsCancellationRequested)
			{
				return 0.0;
			}
			if (_vacationRemind == -100.0)
			{
				Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> obj = await GetMorakhasiItemsAsync(token);
				int num;
				if (obj == null)
				{
					num = 0;
				}
				else
				{
					_ = obj.Item3;
					num = 1;
				}
				if (num != 0)
				{
					_vacationRemind = ((await GetMorakhasiItemsAsync(token))?.Item3).Value;
				}
			}
			if (_vacationRemind != -100.0)
			{
				return _vacationRemind;
			}
			return 0.0;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public void SetVacationRemind(double value)
	{
		_vacationRemind = value;
	}

	public decimal GetSumKolKarkerd()
	{
		return (SumKarkerd + SumMission + SumEzafeKar1 + SumEzafeKar2 + SumEzafeKar3 + SumShabKari + SumNobatKari1 + SumNobatKari2 + SumNobatKari3 + SumJomeKari + SumTatilKari + SumVacation - GetSumVacationUsed() - SumGheybat - SumJarime - SumKasrKar1 - SumKasrKar2 - SumKasrKar3).RoundPrice();
	}

	public decimal GetSumVacationUsed()
	{
		try
		{
			if (ClsGharardad == null || VacationST == 0)
			{
				return 0m;
			}
			return ((decimal)(VacationMinUsed + CountMorakhasi * ClsGharardad.LeaveMinutes) / 60m * VacationPrice).RoundPrice();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<string> GetJobTitleAsync(CancellationToken token)
	{
		string text = _jobTitle;
		if (text == null)
		{
			Per_WorkTitle per_WorkTitle = await GetPerWorkTitleAsync(token);
			Fish fish = this;
			string obj = per_WorkTitle?.Name ?? "---";
			string text2 = obj;
			fish._jobTitle = obj;
			text = text2;
		}
		return text;
	}

	public async Task<string> GetJobCodeAsync(CancellationToken token)
	{
		try
		{
			string text = _jobCode;
			if (text == null)
			{
				text = (_jobCode = (await GetPerWorkTitleAsync(token)).Code);
			}
			return text;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<Per_WorkTitle> GetPerWorkTitleAsync(CancellationToken token)
	{
		try
		{
			int? num = ClsGharardad?.JobId;
			Per_WorkTitle perWorkTitle;
			if (num.HasValue)
			{
				Per_WorkTitle per_WorkTitle = _perWorkTitle;
				if (per_WorkTitle == null)
				{
					per_WorkTitle = (_perWorkTitle = await Per_WorkTitle.GetAsync(ContextGuid, num.Value, token));
				}
				perWorkTitle = per_WorkTitle;
			}
			else
			{
				perWorkTitle = new Per_WorkTitle(ContextGuid);
			}
			_perWorkTitle = perWorkTitle;
			return _perWorkTitle;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>>> GetMorakhasiItemsAsync(CancellationToken token)
	{
		try
		{
			if (_morakhasiItems != null)
			{
				return _morakhasiItems;
			}
			GharardadNew clsGharardad = ClsGharardad;
			if (clsGharardad == null)
			{
				return null;
			}
			_morakhasiItems = await Morakhasidaily.CalcLeaveBalanceAsync(EntityCacheConfig.CacheInstance.ContextGuid, clsGharardad, token);
			return _morakhasiItems;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<List<AgreedDeduction>> GetAgreedDeducationsAsync(CancellationToken token)
	{
		try
		{
			List<AgreedDeduction> list = _agreedDeducations;
			if (list == null)
			{
				list = (_agreedDeducations = await AgreedDeduction.GetAllRemnantAsync(ContextGuid, PersonId.ToString(), ConstantValues.MinDateM, DateEndM, token));
			}
			return list;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<decimal> GetIncludedInsuranceAsync(CancellationToken token)
	{
		try
		{
			List<HesabDet> list = await GetIncludedInsuranceItemsAsync(token);
			if (list != null)
			{
				decimal num = list.Sum((HesabDet p) => p.debit);
				decimal num2 = list.Sum((HesabDet p) => p.credit) - num;
				if (num2 < 0m)
				{
					num2 = default(decimal);
				}
				return num2;
			}
			return default(decimal);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<List<HesabDet>> GetIncludedInsuranceItemsAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			List<HesabDet> hesabDetList = new List<HesabDet>();
			if (ClsGharardad == null)
			{
				return hesabDetList;
			}
			if (_codeacc != null && _codeacc.InsuranceBaseSalary)
			{
				GharardadNew clsGharardad = ClsGharardad;
				if (clsGharardad == null || (int)clsGharardad.InsuranceOption != 0)
				{
					LaborLaw laborLaw = _setting?.LaborLaw;
					await Task.Delay(100);
					if (token.IsCancellationRequested)
					{
						return hesabDetList;
					}
					if ((int)ClsGharardad.InsuranceOption == 1)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = $"حقوق پایه * تعداد روز محاسبه بیمه ({laborLaw.DailyWage} * {CountWorkdayBime})",
							credit = laborLaw.DailyWage * (decimal)CountWorkdayBime
						});
						hesabDetList.Add(new HesabDet
						{
							desc = $"حق بن * ضریب محاسبه بیمه ({laborLaw.Bon} * {FactorByInsurance})",
							credit = (laborLaw.Bon * FactorByInsurance).RoundPrice()
						});
						hesabDetList.Add(new HesabDet
						{
							desc = $"حق مسکن * ضریب محاسبه بیمه ({laborLaw.HaghMaskan} * {FactorByInsurance})",
							credit = (laborLaw.HaghMaskan * FactorByInsurance).RoundPrice()
						});
						return hesabDetList;
					}
					switch ((EnumsHoghogh.EnumkarkardSt)ClsGharardad.SalaryType)
					{
					case EnumsHoghogh.EnumkarkardSt.Monthly:
						hesabDetList.Add(new HesabDet
						{
							desc = $"مبلغ کارکرد ماهانه * ضریب محاسبه بیمه ({KarkerdPrice} * {FactorByInsurance})",
							credit = (KarkerdPrice * FactorByInsurance).RoundPrice()
						});
						break;
					case EnumsHoghogh.EnumkarkardSt.Daily:
						hesabDetList.Add(new HesabDet
						{
							desc = $"مبلغ کارکرد روزانه * تعداد روز بیمه ({KarkerdPrice} * {CountWorkdayBime})",
							credit = KarkerdPrice * (decimal)CountWorkdayBime
						});
						break;
					case EnumsHoghogh.EnumkarkardSt.Hourly:
						hesabDetList.Add(new HesabDet
						{
							desc = $"مبلغ کارکرد ساعتی * ساعت کارکرد معادل یکروز * تعداد روز بیمه ({KarkerdPrice} * {Math.Round(((decimal?)ClsGharardad?.LeaveMinutes) ?? 7.3333333333333333333333333333m, 2)} * {CountWorkdayBime})",
							credit = (KarkerdPrice * Math.Round(((decimal?)ClsGharardad?.LeaveMinutes) ?? 7.3333333333333333333333333333m, 2) * (decimal)CountWorkdayBime).RoundPrice()
						});
						break;
					}
					if (_codeacc.InsuranceOverTime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "کل اضافه کار",
							credit = SumEzafeKar1 + SumEzafeKar2 + SumEzafeKar3
						});
					}
					if (_codeacc.InsuranceLeaveTime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "کل کسر کار",
							debit = SumKasrKar1 + SumKasrKar2 + SumKasrKar3
						});
					}
					if (_codeacc.InsuranceAbsentTime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "غیبت",
							debit = SumGheybat
						});
					}
					if (_codeacc.InsuranceNightTime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "شب کاری",
							credit = SumShabKari
						});
					}
					if (_codeacc.InsuranceShiftTime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "کل نوبت کاری",
							credit = SumNobatKari1 + SumNobatKari2 + SumNobatKari3
						});
					}
					if (_codeacc.InsuranceHolidayTime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "جمعه کاری",
							credit = SumJomeKari
						});
					}
					if (_codeacc.InsuranceHolidayTime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "تعطیل کاری",
							credit = SumTatilKari
						});
					}
					if (_codeacc.InsuranceJarime)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "جریمه تاخیر",
							debit = SumJarime
						});
					}
					if (_codeacc.InsuranceMission)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "ماموریت",
							credit = SumMission
						});
					}
					if (_codeacc.InsuranceVacation)
					{
						hesabDetList.Add(new HesabDet
						{
							desc = "مبلغ تسویه مانده مرخصی",
							credit = SumVacation - GetSumVacationUsed()
						});
					}
					FishDetJson sanavatDet = ListFishDet?.FirstOrDefault((FishDetJson q) => q.Insurance && q.TypeAccount == 1 && q.HesabId == 4);
					if (sanavatDet != null)
					{
						decimal num = (await BaseYearsSalary.GetPriceSanavatAsync(EntityCacheConfig.CacheInstance.ContextGuid, ClsGharardad.DateStartM, DateEndM, ClsGharardad.GroupSanavat, ClsGharardad.Sanavat, token)).RoundPrice();
						if (sanavatDet.IsDecrease)
						{
							hesabDetList.Add(new HesabDet
							{
								desc = $"سنوات پایه * تعداد روز بیمه ({num} * {CountWorkdayBime})",
								credit = num * (decimal)CountWorkdayBime
							});
						}
						else
						{
							hesabDetList.Add(new HesabDet
							{
								desc = $"سنوات پایه * تعداد روز ماه ({num} * {CountDaysOfMonth})- عدم وابستگی به تعداد روز بیمه",
								credit = num * (decimal)CountDaysOfMonth
							});
						}
					}
					foreach (FishDetJson item in ListFishDet?.Where((FishDetJson q) => q.Insurance && q.TypeAccount == 1 && q.IsDecrease && q.HesabId != 4))
					{
						hesabDetList.Add(new HesabDet
						{
							desc = $"{item.Name} * ضریب محاسبه بیمه ({item.Credit} * {FactorByInsurance})",
							credit = (item.Credit * FactorByInsurance).RoundPrice()
						});
					}
					foreach (FishDetJson item2 in ListFishDet?.Where((FishDetJson q) => q.Insurance && q.TypeAccount == 1 && !q.IsDecrease && q.HesabId != 4))
					{
						hesabDetList.Add(new HesabDet
						{
							desc = item2.Name + "- غیر وابسته به کارکرد",
							credit = item2.Credit
						});
					}
					foreach (FishDetJson item3 in ListFishDet?.Where((FishDetJson q) => q.Insurance && q.TypeAccount == 2 && q.IsDecrease))
					{
						hesabDetList.Add(new HesabDet
						{
							desc = $"{item3.Name} *ضریب محاسبه بیمه ({item3.Debit} * {FactorByInsurance})",
							debit = (item3.Debit * FactorByInsurance).RoundPrice()
						});
					}
					foreach (FishDetJson item4 in ListFishDet?.Where((FishDetJson q) => q.Insurance && q.TypeAccount == 2 && !q.IsDecrease))
					{
						hesabDetList.Add(new HesabDet
						{
							desc = item4.Name + "- غیر وابسته به کارکرد",
							debit = item4.Debit
						});
					}
					return hesabDetList;
				}
			}
			return hesabDetList;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<decimal> GetIncludedTaxAsync(CancellationToken token)
	{
		try
		{
			List<HesabDet> list = await GetIncludedTaxItemsAsync(token);
			if (list != null)
			{
				decimal num = list.Sum((HesabDet p) => p.debit);
				decimal num2 = list.Sum((HesabDet p) => p.credit) - num;
				if (num2 < 0m)
				{
					num2 = default(decimal);
				}
				return num2;
			}
			return default(decimal);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<List<HesabDet>> GetIncludedTaxItemsAsync(CancellationToken token)
	{
		List<HesabDet> hesabDetList = new List<HesabDet>();
		GharardadNew clsGharardad = ClsGharardad;
		if (clsGharardad == null)
		{
			return hesabDetList;
		}
		try
		{
			if (_codeacc == null || !_codeacc.TaxBaseSalary)
			{
				return hesabDetList;
			}
			await Task.Delay(100);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			switch ((EnumsHoghogh.EnumkarkardSt)clsGharardad.SalaryType)
			{
			case EnumsHoghogh.EnumkarkardSt.Monthly:
				hesabDetList.Add(new HesabDet
				{
					desc = $"حقوق پایه * ضریب محاسبه مالیات ({KarkerdPrice} * {FactorByTax})",
					credit = KarkerdPrice * FactorByTax
				});
				break;
			case EnumsHoghogh.EnumkarkardSt.Daily:
				hesabDetList.Add(new HesabDet
				{
					desc = $"مبلغ کارکرد روزانه * تعداد روز محاسبه مالیات ({KarkerdPrice} * {CountWorkdayMaliyat})",
					credit = KarkerdPrice * (decimal)CountWorkdayMaliyat
				});
				break;
			case EnumsHoghogh.EnumkarkardSt.Hourly:
				hesabDetList.Add(new HesabDet
				{
					desc = $"مبلغ کارکرد ساعتی * ساعت کارکرد معادل یکروز * تعداد روز مالیات ({KarkerdPrice} * {Math.Round((decimal)clsGharardad.LeaveMinutes / 60m, 2)} * {CountWorkdayMaliyat})",
					credit = (KarkerdPrice * Math.Round((decimal)clsGharardad.LeaveMinutes / 60m, 2) * (decimal)CountWorkdayMaliyat).RoundPrice()
				});
				break;
			default:
				return hesabDetList;
			}
			if (_codeacc.TaxOverTime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "کل اضافه کار",
					credit = SumEzafeKar1 + SumEzafeKar2 + SumEzafeKar3
				});
			}
			if (_codeacc.TaxLeaveTime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "کل کسر کار",
					debit = SumKasrKar1 + SumKasrKar2 + SumKasrKar3
				});
			}
			if (_codeacc.TaxAbsentTime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "غیبت",
					debit = SumGheybat
				});
			}
			if (_codeacc.TaxNightTime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "شب کاری",
					credit = SumShabKari
				});
			}
			if (_codeacc.TaxShiftTime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "کل نوبت کاری",
					credit = SumNobatKari1 + SumNobatKari2 + SumNobatKari3
				});
			}
			if (_codeacc.TaxHolidayTime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "جمعه کاری",
					credit = SumJomeKari
				});
			}
			if (_codeacc.TaxHolidayTime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "تعطیل کاری",
					credit = SumTatilKari
				});
			}
			if (_codeacc.TaxJarime)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "جریمه تاخیر",
					debit = SumJarime
				});
			}
			if (_codeacc.TaxMission)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "ماموریت",
					credit = SumMission
				});
			}
			if (_codeacc.TaxVacation)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "مبلغ تسویه مانده مرخصی",
					credit = SumVacation - GetSumVacationUsed()
				});
			}
			FishDetJson sanavatDet = ListFishDet?.FirstOrDefault((FishDetJson q) => q.Taxable && q.TypeAccount == 1 && q.HesabId == 4);
			if (sanavatDet != null)
			{
				decimal num = (await BaseYearsSalary.GetPriceSanavatAsync(EntityCacheConfig.CacheInstance.ContextGuid, clsGharardad.DateStartM, DateEndM, clsGharardad.GroupSanavat, clsGharardad.Sanavat, token)).RoundPrice();
				if (sanavatDet.IsDecrease)
				{
					hesabDetList.Add(new HesabDet
					{
						desc = $"سنوات پایه * تعداد روز مالیات ({num} * {CountWorkdayMaliyat})",
						credit = num * (decimal)CountWorkdayMaliyat
					});
				}
				else
				{
					hesabDetList.Add(new HesabDet
					{
						desc = $"سنوات پایه * تعداد روز ماه ({num} * {CountDaysOfMonth}) - عدم وابستگی به تعداد روز مالیات",
						credit = num * (decimal)CountDaysOfMonth
					});
				}
			}
			foreach (FishDetJson item in ListFishDet?.Where((FishDetJson q) => q.Taxable && q.TypeAccount == 1 && q.IsDecrease && q.HesabId != 4))
			{
				hesabDetList.Add(new HesabDet
				{
					desc = $"{item.Name} * ضریب محاسبه مالیات ({item.Credit} * {FactorByTax})",
					credit = (item.Credit * FactorByTax).RoundPrice()
				});
			}
			foreach (FishDetJson item2 in ListFishDet?.Where((FishDetJson q) => q.Taxable && q.TypeAccount == 1 && !q.IsDecrease && q.HesabId != 4))
			{
				hesabDetList.Add(new HesabDet
				{
					desc = item2.Name + "- غیر وابسته به کارکرد",
					credit = item2.Credit
				});
			}
			FishDetJson fishDetJson = ListFishDet?.FirstOrDefault((FishDetJson q) => q.Taxable && q.TypeAccount == 2 && q.HesabId == 5);
			if (fishDetJson != null && fishDetJson.Debit > 0m)
			{
				hesabDetList.Add(new HesabDet
				{
					desc = "کسر درصد مبلغ بیمه از مالیات",
					debit = (decimal)_setting.ZaribMinesBimeOfTax / 100m * fishDetJson.Debit
				});
			}
			foreach (FishDetJson item3 in ListFishDet?.Where((FishDetJson q) => q.Taxable && q.TypeAccount == 2 && q.IsDecrease && q.HesabId != 5))
			{
				hesabDetList.Add(new HesabDet
				{
					desc = $"{item3.Name} * ضریب محاسبه مالیات ({item3.Debit} * {FactorByTax})",
					debit = (item3.Debit * FactorByTax).RoundPrice()
				});
			}
			foreach (FishDetJson item4 in ListFishDet?.Where((FishDetJson q) => q.Taxable && q.TypeAccount == 2 && !q.IsDecrease && q.HesabId != 5))
			{
				hesabDetList.Add(new HesabDet
				{
					desc = item4.Name + "- غیر وابسته به کارکرد",
					debit = item4.Debit
				});
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return hesabDetList;
	}

	public string GetRayVarzTaradodKardex()
	{
		try
		{
			if (string.IsNullOrEmpty(_RayVarzTaradodKardex))
			{
				_RayVarzTaradodKardex = GetRayVarzTaradodKardex_();
			}
			return _RayVarzTaradodKardex;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private string GetRayVarzTaradodKardex_()
	{
		string text = "";
		try
		{
			text += ((PersonBase)ClsPerson).CodeTafsil.ToString("00000000");
			int num = (EzafeMin1 + EzafeMin2 + EzafeMin3) / 60;
			int num2 = (EzafeMin1 + EzafeMin2 + EzafeMin3) % 60;
			text += num.ToString("000");
			text += num2.ToString("00");
			int num3 = (JomeMin + TatilMin) / 60;
			int num4 = (JomeMin + TatilMin) % 60;
			text += num3.ToString("000");
			text += num4.ToString("00");
			int num5 = NobatMin1 / 60;
			int num6 = NobatMin1 % 60;
			text += num5.ToString("000");
			text += num6.ToString("00");
			int num7 = NobatMin2 / 60;
			int num8 = NobatMin2 % 60;
			text += num7.ToString("000");
			text += num8.ToString("00");
			int num9 = NobatMin3 / 60;
			int num10 = NobatMin3 % 60;
			text += num9.ToString("000");
			text += num10.ToString("00");
			int num11 = ShabMin / 60;
			int num12 = ShabMin % 60;
			text += num11.ToString("000");
			text += num12.ToString("00");
			int num13 = (KasrMin1 + KasrMin2 + KasrMin3) / 60;
			int num14 = (KasrMin1 + KasrMin2 + KasrMin3) % 60;
			text += num13.ToString("000");
			text += num14.ToString("00");
			int num15 = GheybatMin / 60;
			int num16 = GheybatMin % 60;
			text += num15.ToString("000");
			text += num16.ToString("00");
			int num17 = MorakhasiEstelajiMin / 60;
			int num18 = MorakhasiEstelajiMin % 60;
			text += num17.ToString("000");
			text += num18.ToString("00");
			int num19 = MorakhasiNotSalaryMin / 60;
			int num20 = MorakhasiNotSalaryMin % 60;
			text += num19.ToString("000");
			text += num20.ToString("00");
			int num21 = MorakhasiEstehghaghiMin / 60;
			int num22 = MorakhasiEstehghaghiMin % 60;
			text += num21.ToString("000");
			return text + num22.ToString("00");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<Fish> GetAsync(string contextGuid, int id, bool loadClsTaradod, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			Fish fish = new Fish(contextGuid)
			{
				ContextGuid = contextGuid
			};
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Fish_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)id);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							fish = await LoadDataAsync(contextGuid, dr, loadClsTaradod, token);
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						return fish;
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<Fish> GetAsync(string contextGuid, SqlDataReader dr, bool loadClsTaradod, CancellationToken token)
	{
		try
		{
			return await LoadDataAsync(contextGuid, dr, loadClsTaradod, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private static async Task<Fish> LoadDataAsync(string contextGuid, SqlDataReader dr, bool loadClsTaradod, CancellationToken token)
	{
		_ = 3;
		try
		{
			Fish ret = new Fish(contextGuid)
			{
				ContextGuid = contextGuid
			};
			Fish fish = ret;
			fish._setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(EntityCacheConfig.CacheInstance.ContextGuid, token);
			ret._codeacc = ret._setting.CodeingAccounting;
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				ret.Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["FishNO"].ToString() != DBNull.Value.ToString())
			{
				ret.FishNo = (int)((DbDataReader)(object)dr)["FishNO"];
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				ret.DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["PersonID"].ToString() != DBNull.Value.ToString())
			{
				ret.PersonId = (int)((DbDataReader)(object)dr)["PersonID"];
			}
			if (((DbDataReader)(object)dr)["DateStartM"].ToString() != DBNull.Value.ToString())
			{
				ret.DateStartM = (DateTime)((DbDataReader)(object)dr)["DateStartM"];
			}
			if (((DbDataReader)(object)dr)["DateEndM"].ToString() != DBNull.Value.ToString())
			{
				ret.DateEndM = (DateTime)((DbDataReader)(object)dr)["DateEndM"];
			}
			if (((DbDataReader)(object)dr)["UID"].ToString() != DBNull.Value.ToString())
			{
				ret.Uid = (short)((DbDataReader)(object)dr)["UID"];
			}
			if (((DbDataReader)(object)dr)["IDSanad"].ToString() != DBNull.Value.ToString())
			{
				ret.IdSanad = (int)((DbDataReader)(object)dr)["IDSanad"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				ret.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				ret.St = (short)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["VacationST"].ToString() != DBNull.Value.ToString())
			{
				ret.VacationST = (short)((DbDataReader)(object)dr)["VacationST"];
			}
			if (((DbDataReader)(object)dr)["EditST"].ToString() != DBNull.Value.ToString())
			{
				ret.EditST = (short)((DbDataReader)(object)dr)["EditST"];
			}
			if (((DbDataReader)(object)dr)["KarkerdMin"].ToString() != DBNull.Value.ToString())
			{
				ret.KarkerdMin = (int)((DbDataReader)(object)dr)["KarkerdMin"];
			}
			if (((DbDataReader)(object)dr)["KarkerdPrice"].ToString() != DBNull.Value.ToString())
			{
				ret.KarkerdPrice = (decimal)((DbDataReader)(object)dr)["KarkerdPrice"];
			}
			if (((DbDataReader)(object)dr)["EzafeMin"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafeMin = (int)((DbDataReader)(object)dr)["EzafeMin"];
			}
			if (ret.EzafeMin < 0)
			{
				ret.EzafeMin = 0;
			}
			if (((DbDataReader)(object)dr)["EzafeMin1"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafeMin1 = (int)((DbDataReader)(object)dr)["EzafeMin1"];
			}
			if (ret.EzafeMin1 < 0)
			{
				ret.EzafeMin1 = 0;
			}
			if (((DbDataReader)(object)dr)["EzafeMin2"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafeMin2 = (int)((DbDataReader)(object)dr)["EzafeMin2"];
			}
			if (ret.EzafeMin2 < 0)
			{
				ret.EzafeMin2 = 0;
			}
			if (((DbDataReader)(object)dr)["EzafeMin3"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafeMin3 = (int)((DbDataReader)(object)dr)["EzafeMin3"];
			}
			if (ret.EzafeMin3 < 0)
			{
				ret.EzafeMin3 = 0;
			}
			if (((DbDataReader)(object)dr)["EzafePrice"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafePrice = (decimal)((DbDataReader)(object)dr)["EzafePrice"];
			}
			if (((DbDataReader)(object)dr)["EzafePrice1"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafePrice1 = (decimal)((DbDataReader)(object)dr)["EzafePrice1"];
			}
			if (((DbDataReader)(object)dr)["EzafePrice2"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafePrice2 = (decimal)((DbDataReader)(object)dr)["EzafePrice2"];
			}
			if (((DbDataReader)(object)dr)["EzafePrice3"].ToString() != DBNull.Value.ToString())
			{
				ret.EzafePrice3 = (decimal)((DbDataReader)(object)dr)["EzafePrice3"];
			}
			if (((DbDataReader)(object)dr)["ShabMin"].ToString() != DBNull.Value.ToString())
			{
				ret.ShabMin = (int)((DbDataReader)(object)dr)["ShabMin"];
			}
			if (ret.ShabMin < 0)
			{
				ret.ShabMin = 0;
			}
			if (((DbDataReader)(object)dr)["ShabPrice"].ToString() != DBNull.Value.ToString())
			{
				ret.ShabPrice = (decimal)((DbDataReader)(object)dr)["ShabPrice"];
			}
			if (((DbDataReader)(object)dr)["NobatMin1"].ToString() != DBNull.Value.ToString())
			{
				ret.NobatMin1 = (int)((DbDataReader)(object)dr)["NobatMin1"];
			}
			if (ret.NobatMin1 < 0)
			{
				ret.NobatMin1 = 0;
			}
			if (((DbDataReader)(object)dr)["NobatMin2"].ToString() != DBNull.Value.ToString())
			{
				ret.NobatMin2 = (int)((DbDataReader)(object)dr)["NobatMin2"];
			}
			if (ret.NobatMin2 < 0)
			{
				ret.NobatMin2 = 0;
			}
			if (((DbDataReader)(object)dr)["NobatMin3"].ToString() != DBNull.Value.ToString())
			{
				ret.NobatMin3 = (int)((DbDataReader)(object)dr)["NobatMin3"];
			}
			if (ret.NobatMin3 < 0)
			{
				ret.NobatMin3 = 0;
			}
			if (((DbDataReader)(object)dr)["NobatPrice1"].ToString() != DBNull.Value.ToString())
			{
				ret.NobatPrice1 = (decimal)((DbDataReader)(object)dr)["NobatPrice1"];
			}
			if (((DbDataReader)(object)dr)["NobatPrice2"].ToString() != DBNull.Value.ToString())
			{
				ret.NobatPrice2 = (decimal)((DbDataReader)(object)dr)["NobatPrice2"];
			}
			if (((DbDataReader)(object)dr)["NobatPrice3"].ToString() != DBNull.Value.ToString())
			{
				ret.NobatPrice3 = (decimal)((DbDataReader)(object)dr)["NobatPrice3"];
			}
			if (((DbDataReader)(object)dr)["JomeMin"].ToString() != DBNull.Value.ToString())
			{
				ret.JomeMin = (int)((DbDataReader)(object)dr)["JomeMin"];
			}
			if (ret.JomeMin < 0)
			{
				ret.JomeMin = 0;
			}
			if (((DbDataReader)(object)dr)["TatilMin"].ToString() != DBNull.Value.ToString())
			{
				ret.TatilMin = (int)((DbDataReader)(object)dr)["TatilMin"];
			}
			if (((DbDataReader)(object)dr)["Mozafimin"].ToString() != DBNull.Value.ToString())
			{
				ret.MoazafiMin = (int)((DbDataReader)(object)dr)["Mozafimin"];
			}
			if (ret.TatilMin < 0)
			{
				ret.TatilMin = 0;
			}
			if (((DbDataReader)(object)dr)["TatilPrice"].ToString() != DBNull.Value.ToString())
			{
				ret.TatilPrice = (decimal)((DbDataReader)(object)dr)["TatilPrice"];
			}
			if (((DbDataReader)(object)dr)["JomePrice"].ToString() != DBNull.Value.ToString())
			{
				ret.JomePrice = (decimal)((DbDataReader)(object)dr)["JomePrice"];
			}
			if (((DbDataReader)(object)dr)["MorakhasiMin"].ToString() != DBNull.Value.ToString())
			{
				ret.VacationMin = (int)((DbDataReader)(object)dr)["MorakhasiMin"];
			}
			if (ret.VacationMin < 0)
			{
				ret.VacationMin = 0;
			}
			if (((DbDataReader)(object)dr)["VacationRemind"].ToString() != DBNull.Value.ToString())
			{
				ret.SetVacationRemind((double)((DbDataReader)(object)dr)["VacationRemind"]);
			}
			if (((DbDataReader)(object)dr)["MorakhasiMinUsed"].ToString() != DBNull.Value.ToString())
			{
				ret.VacationMinUsed = (int)((DbDataReader)(object)dr)["MorakhasiMinUsed"];
			}
			if (((DbDataReader)(object)dr)["MorakhasiPrice"].ToString() != DBNull.Value.ToString())
			{
				ret.VacationPrice = (decimal)((DbDataReader)(object)dr)["MorakhasiPrice"];
			}
			if (((DbDataReader)(object)dr)["GheybatMin"].ToString() != DBNull.Value.ToString())
			{
				ret.GheybatRoze = (int)((DbDataReader)(object)dr)["GheybatMin"];
			}
			if (ret.GheybatRoze < 0)
			{
				ret.GheybatRoze = 0;
			}
			if (((DbDataReader)(object)dr)["GheybatPrice"].ToString() != DBNull.Value.ToString())
			{
				ret.GheybatPrice = (decimal)((DbDataReader)(object)dr)["GheybatPrice"];
			}
			if (((DbDataReader)(object)dr)["KasrMin"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrMin = (int)((DbDataReader)(object)dr)["KasrMin"];
			}
			if (ret.KasrMin < 0)
			{
				ret.KasrMin = 0;
			}
			if (((DbDataReader)(object)dr)["KasrMin1"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrMin1 = (int)((DbDataReader)(object)dr)["KasrMin1"];
			}
			if (ret.KasrMin < 0)
			{
				ret.KasrMin = 0;
			}
			if (((DbDataReader)(object)dr)["KasrMin2"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrMin2 = (int)((DbDataReader)(object)dr)["KasrMin2"];
			}
			if (ret.KasrMin < 0)
			{
				ret.KasrMin = 0;
			}
			if (((DbDataReader)(object)dr)["KasrMin3"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrMin3 = (int)((DbDataReader)(object)dr)["KasrMin3"];
			}
			if (ret.KasrMin < 0)
			{
				ret.KasrMin = 0;
			}
			if (((DbDataReader)(object)dr)["KasrPrice"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrPrice = (decimal)((DbDataReader)(object)dr)["KasrPrice"];
			}
			if (((DbDataReader)(object)dr)["KasrPrice1"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrPrice1 = (decimal)((DbDataReader)(object)dr)["KasrPrice1"];
			}
			if (((DbDataReader)(object)dr)["KasrPrice2"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrPrice2 = (decimal)((DbDataReader)(object)dr)["KasrPrice2"];
			}
			if (((DbDataReader)(object)dr)["KasrPrice3"].ToString() != DBNull.Value.ToString())
			{
				ret.KasrPrice3 = (decimal)((DbDataReader)(object)dr)["KasrPrice3"];
			}
			if (((DbDataReader)(object)dr)["IsPrint"].ToString() != DBNull.Value.ToString())
			{
				ret.IsPrint = (short)((DbDataReader)(object)dr)["IsPrint"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				ret.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["MissionPrice"].ToString() != DBNull.Value.ToString())
			{
				ret.MissionPrice = (decimal)((DbDataReader)(object)dr)["MissionPrice"];
			}
			if (((DbDataReader)(object)dr)["MissionMin"].ToString() != DBNull.Value.ToString())
			{
				ret.MissionRoz = (double)((DbDataReader)(object)dr)["MissionMin"];
			}
			if (ret.MissionRoz < 0.0)
			{
				ret.MissionRoz = 0.0;
			}
			if (((DbDataReader)(object)dr)["JarimePrice"].ToString() != DBNull.Value.ToString())
			{
				ret.JarimePrice = (decimal)((DbDataReader)(object)dr)["JarimePrice"];
			}
			if (((DbDataReader)(object)dr)["JarimeRoz"].ToString() != DBNull.Value.ToString())
			{
				ret.JarimeRoz = (int)((DbDataReader)(object)dr)["JarimeRoz"];
			}
			if (ret.JarimeRoz < 0)
			{
				ret.JarimeRoz = 0;
			}
			if (((DbDataReader)(object)dr)["CountKarKerd"].ToString() != DBNull.Value.ToString())
			{
				ret.CountKarKerd = (short)((DbDataReader)(object)dr)["CountKarKerd"];
			}
			if (((DbDataReader)(object)dr)["CountKarKerdBime"].ToString() != DBNull.Value.ToString())
			{
				ret.CountWorkdayBime = (short)((DbDataReader)(object)dr)["CountKarKerdBime"];
			}
			if (((DbDataReader)(object)dr)["CountKarKerdMaliat"].ToString() != DBNull.Value.ToString())
			{
				ret.CountWorkdayMaliyat = (short)((DbDataReader)(object)dr)["CountKarKerdMaliat"];
			}
			if (((DbDataReader)(object)dr)["CountShift"].ToString() != DBNull.Value.ToString())
			{
				ret.Countshift = (short)((DbDataReader)(object)dr)["CountShift"];
			}
			if (((DbDataReader)(object)dr)["CountMorakhasiWithHoghogh"].ToString() != DBNull.Value.ToString())
			{
				ret.CountMorakhasiWithSalary = (short)((DbDataReader)(object)dr)["CountMorakhasiWithHoghogh"];
			}
			if (((DbDataReader)(object)dr)["CountMorakhasiNotHoghogh"].ToString() != DBNull.Value.ToString())
			{
				ret.CountMorakhasiNotSalary = (short)((DbDataReader)(object)dr)["CountMorakhasiNotHoghogh"];
			}
			if (((DbDataReader)(object)dr)["CountMorakhasi"].ToString() != DBNull.Value.ToString())
			{
				ret.CountMorakhasi = (short)((DbDataReader)(object)dr)["CountMorakhasi"];
			}
			if (((DbDataReader)(object)dr)["Countgheybat"].ToString() != DBNull.Value.ToString())
			{
				ret.CountGheybat = (short)((DbDataReader)(object)dr)["Countgheybat"];
			}
			if (((DbDataReader)(object)dr)["Countnaghes"].ToString() != DBNull.Value.ToString())
			{
				ret.CountNaghes = (short)((DbDataReader)(object)dr)["Countnaghes"];
			}
			if (((DbDataReader)(object)dr)["CountTatil"].ToString() != DBNull.Value.ToString())
			{
				ret.CountTatil = (short)((DbDataReader)(object)dr)["CountTatil"];
			}
			if (((DbDataReader)(object)dr)["JsonFishDet"].ToString() != DBNull.Value.ToString())
			{
				ret.JsonFishDet = ((DbDataReader)(object)dr)["JsonFishDet"].ToString();
			}
			if (((DbDataReader)(object)dr)["JobTitle"].ToString() != DBNull.Value.ToString())
			{
				ret._jobTitle = ((DbDataReader)(object)dr)["JobTitle"].ToString();
			}
			fish = ret;
			fish.ClsPerson = await Person.GetByPersonIdAsync(contextGuid, ret.PersonId, token);
			fish = ret;
			fish.ClsGharardad = await GharardadNew.GetAsync(contextGuid, ret.PersonId, ret.DateStartM, ret.DateEndM, token);
			if (loadClsTaradod)
			{
				fish = ret;
				fish.ClsTaradod = await new TaradodFull(contextGuid).InitializeAsync(ret.DateStartM, ret.DateEndM, ret.ClsPerson, null, checkValidationVorodAndKHoroj: false, token);
			}
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, " Fish.LoadData");
			throw;
		}
	}

	public Fish(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private async Task LoadCLsFromGharardadAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			FishNo = await NextCodeAsync(ContextGuid, token);
			DateM = DateTime.Now;
			Uid = UserAttendance.SelectedUser.ID;
			GharardadNew clsGharardad = ClsGharardad;
			if (clsGharardad != null)
			{
				St = clsGharardad.SalaryType;
				VacationST = (clsGharardad.IsCheckoutVacation ? ((short)1) : ((short)0));
				KarkerdPrice = clsGharardad.SalaryPrice;
				EzafePrice = clsGharardad.PriceOverTime3;
				EzafePrice1 = clsGharardad.PriceOverTime;
				EzafePrice2 = clsGharardad.PriceOverTime2;
				EzafePrice3 = clsGharardad.PriceOverTime3;
				ShabPrice = clsGharardad.PriceNight;
				NobatPrice1 = clsGharardad.PriceNobatKari1;
				NobatPrice2 = clsGharardad.PriceNobatKari2;
				NobatPrice3 = clsGharardad.PriceNobatKari3;
				JomePrice = clsGharardad.PriceHoliday;
				TatilPrice = clsGharardad.PriceTatil;
				JarimePrice = clsGharardad.PriceJarime;
				VacationPrice = clsGharardad.PriceLeave;
				GheybatPrice = clsGharardad.PriceAbsent;
				MissionPrice = clsGharardad.PriceMission;
				KasrPrice = clsGharardad.PriceLowTime;
				KasrPrice1 = clsGharardad.PriceLowTime;
				KasrPrice2 = clsGharardad.PriceLowTime2;
				KasrPrice3 = clsGharardad.PriceLowTime3;
				await FillFishDetFromGharardadAsync(token);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task FillFishDetFromGharardadAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			GharardadNew clsGharardad = ClsGharardad;
			if (clsGharardad == null)
			{
				return;
			}
			ListFishDet.Clear();
			foreach (GharardadDetNew item in clsGharardad.ListGharardadDet)
			{
				if (item.HesabId == -1 || ListFishDet.FindIndex((FishDetJson p) => p.HesabId == item.HesabId) < 0)
				{
					FishDetJson fishDetJson = new FishDetJson(EntityCacheConfig.CacheInstance.ContextGuid)
					{
						HesabId = item.HesabId,
						Description = item.Desc,
						TypeAccount = item.TypeAccount,
						Name = item.Name,
						IsDecrease = item.IsDecrease,
						Formula = item.Formula,
						Privilege = item.Privilege
					};
					FishDetJson fishDetJson2 = fishDetJson;
					fishDetJson2.Taxable = await item.GetTaxableAsync(token);
					FishDetJson fishDetJson3 = fishDetJson;
					fishDetJson3.Insurance = await item.GetInsuranceAsync(token);
					fishDetJson.TmpValue = item.nValue;
					FishDetJson fishDetJson4 = fishDetJson;
					if (item.TypeAccount == 1)
					{
						fishDetJson4.Credit = item.nValue;
					}
					else
					{
						fishDetJson4.Debit = item.nValue;
					}
					ListFishDet.Add(fishDetJson4);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task CalculateFormulaInGharardadAsync(CancellationToken token)
	{
		try
		{
			GharardadNew clsGharardad = ClsGharardad;
			if (clsGharardad != null && ClsTaradod != null)
			{
				await clsGharardad.CalculateFormulaAsync(ClsTaradod, token);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task CalculateFormulaInFishAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			if (ListFishDet.Any((FishDetJson q) => !string.IsNullOrEmpty(q.Formula)))
			{
				GharardadNew clsGharardad = ClsGharardad;
				if (clsGharardad != null)
				{
					FillGharardadfromFish();
					if (token.IsCancellationRequested)
					{
						return;
					}
					foreach (GharardadDetNew item in clsGharardad.ListGharardadDet.Where((GharardadDetNew q) => q.Privilege > 0).ToList())
					{
						item.nValue = -1m;
					}
					if (token.IsCancellationRequested)
					{
						return;
					}
					await CalculateFormulaInGharardadAsync(token);
					await FillFishDetFromGharardadAsync(token);
				}
			}
			await CalcAgreedDeducationAsync(token);
			await CalcInsuranceTaxAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task CalcInsuranceTaxAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			GharardadNew clsGharardad = ClsGharardad;
			if (clsGharardad != null)
			{
				await CalculateFishDetForKarkerdAsync(token);
				int fishDetInsuranceidx = ListFishDet.FindIndex((FishDetJson q) => q.HesabId == 5 && !q.DisableCompute);
				if (fishDetInsuranceidx >= 0 && clsGharardad?.ListGharardadDet.FirstOrDefault((GharardadDetNew q) => q.HesabId == 5) != null)
				{
					decimal num = (await GetIncludedInsuranceAsync(token) * (decimal)(clsGharardad?.ZaribInsuredInsurance).Value / 100m).RoundPrice();
					FishDetJson fishDetJson = ListFishDet[fishDetInsuranceidx];
					decimal tmpValue = (ListFishDet[fishDetInsuranceidx].Debit = num);
					fishDetJson.TmpValue = tmpValue;
				}
				int fishDetTaxidx = ListFishDet.FindIndex((FishDetJson q) => q.HesabId == 6 && !q.DisableCompute);
				if (fishDetTaxidx >= 0)
				{
					decimal dvalue = await GetIncludedTaxAsync(token);
					TaxTable taxTable = await TaxTable.GetAsync(ContextGuid, (short)Utilities.GetYearSh(DateStartM), dvalue, token);
					tax = (taxTable.InitialValue + (dvalue - (taxTable.FromPrice - 1m)) * taxTable.TaxPercentage / 100m).RoundPrice();
					FishDetJson fishDetJson2 = ListFishDet[fishDetTaxidx];
					decimal tmpValue = (ListFishDet[fishDetTaxidx].Debit = tax);
					fishDetJson2.TmpValue = tmpValue;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task CalculateFishDetForKarkerdAsync(CancellationToken token)
	{
		try
		{
			foreach (FishDetJson item in ListFishDet)
			{
				GharardadNew clsGharardad = ClsGharardad;
				decimal factorByKarkardMin = FactorByKarkardMin;
				decimal decimalNumber;
				if (item.Debit > 0m)
				{
					decimalNumber = item.Debit;
					if (item.IsDecrease)
					{
						if (St == 3 && clsGharardad != null)
						{
							decimalNumber *= factorByKarkardMin;
						}
						else
						{
							decimalNumber *= FactorByCountKarkard;
						}
					}
				}
				else
				{
					decimalNumber = item.Credit;
					if (item.HesabId == 4 && clsGharardad != null)
					{
						decimal num = await BaseYearsSalary.GetPriceSanavatAsync(EntityCacheConfig.CacheInstance.ContextGuid, clsGharardad.DateStartM, DateEndM, clsGharardad.GroupSanavat, clsGharardad.Sanavat, token);
						decimalNumber = ((!item.IsDecrease) ? (num * (decimal)CountDaysOfMonth) : (num * (decimal)CountKarKerd));
					}
					else if (item.IsDecrease)
					{
						if (St == 3 && clsGharardad != null)
						{
							decimalNumber *= factorByKarkardMin;
						}
						else
						{
							decimalNumber *= FactorByCountKarkard;
						}
					}
				}
				item.TmpValue = decimalNumber.RoundPrice();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task CalcAgreedDeducationAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			foreach (AgreedDeduction item in await GetAgreedDeducationsAsync(token))
			{
				decimal xremnant = item.AgreedDeductionDets.FirstOrDefault((AgreedDeductionDet q) => q.MonthYear == FishMonthYear)?.Remnant ?? (item.AmountPayable - (from q in item.AgreedDeductionDets.Distinct(new AgreedDeductionDetComparer())
					where q.FishId != Id && q.MonthYear != FishMonthYear
					select q).Sum((AgreedDeductionDet p) => p.Price));
				decimal num = ((item.MonthlyInstallment < xremnant) ? item.MonthlyInstallment : xremnant);
				AgreedDeductionDet agreedDeductionDet = item.AgreedDeductionDets.FirstOrDefault((AgreedDeductionDet q) => q.FishId == Id);
				if (agreedDeductionDet != null)
				{
					agreedDeductionDet.Price = num;
				}
				else
				{
					item.AgreedDeductionDets.Add(new AgreedDeductionDet(ContextGuid)
					{
						FishId = Id,
						Price = num,
						MonthYear = FishMonthYear,
						Remnant = xremnant
					});
				}
				if (!ListFishDet.Any((FishDetJson q) => q.HesabId == item.HesabId))
				{
					FishDetJson fishDetJson = new FishDetJson(ContextGuid)
					{
						HesabId = item.HesabId,
						Debit = num,
						TmpValue = num
					};
					FishDetJson fishDetJson2 = fishDetJson;
					fishDetJson2.Name = await item.GetHesabTitleAsync(token);
					fishDetJson.TypeAccount = 2;
					fishDetJson.IsDecrease = false;
					FishDetJson fishDetJson3 = fishDetJson;
					fishDetJson3.Taxable = await item.GetTaxableAsync(token);
					FishDetJson fishDetJson4 = fishDetJson;
					fishDetJson4.Insurance = await item.GetInsuranceAsync(token);
					fishDetJson.Credit = 0m;
					fishDetJson.Description = "";
					fishDetJson.TotalPayable = item.AmountPayable;
					fishDetJson.RemainingBeforeBill = xremnant;
					fishDetJson.StartMounthPayM = item.DateCreatedM;
					ListFishDet.Add(fishDetJson);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private void FillGharardadfromFish()
	{
		try
		{
			ClsTaradod.CountWorkdayHoghogh = CountKarKerd;
			ClsTaradod.CountWorkdayBime = CountWorkdayBime;
			ClsTaradod.CountWorkdayMaliyat = CountWorkdayMaliyat;
			ClsTaradod.Countshift = Countshift;
			ClsTaradod.CountMorakhasiWithSalary = CountMorakhasiWithSalary;
			ClsTaradod.CountMorakhasi = CountMorakhasi;
			ClsTaradod.CountGheybat = CountGheybat;
			ClsTaradod.CountNaghes = CountNaghes;
			ClsTaradod.CountTatil = CountTatil;
			ClsTaradod.SumWorkVacation = VacationMinUsed;
			ClsTaradod.SumEzafekarBegin = EzafeMin1;
			ClsTaradod.SumEzafekarMid = EzafeMin2;
			ClsTaradod.SumEzafekarEnd = EzafeMin3;
			ClsTaradod.SumKasrkarBegin = KasrMin1;
			ClsTaradod.SumKasrkarMid = KasrMin2;
			ClsTaradod.SumKasrkarEnd = KasrMin3;
			ClsTaradod.CountGheybat = (short)GheybatRoze;
			ClsTaradod.SumWorkNightWork = ShabMin;
			ClsTaradod.SumNobatKari1 = NobatMin1;
			ClsTaradod.SumNobatKari2 = NobatMin2;
			ClsTaradod.SumNobatKari3 = NobatMin3;
			ClsTaradod.SumFriDayWork = JomeMin;
			ClsTaradod.SumTatilDayWork = TatilMin;
			ClsTaradod.SumMoazzafi = MoazafiMin;
			ClsTaradod.SumJarimeRoz = JarimeRoz;
			ClsTaradod.SumKarkerd = KarkerdMin;
			ClsTaradod.CountMorakhasiNotSalary = CountMorakhasiNotSalary;
			ClsGharardad.SalaryType = St;
			ClsGharardad.IsCheckoutVacation = VacationST == 1;
			ClsGharardad.SetSalaryPrice(KarkerdPrice);
			ClsGharardad.PriceOverTime = EzafePrice1;
			ClsGharardad.PriceOverTime2 = EzafePrice2;
			ClsGharardad.PriceOverTime3 = EzafePrice3;
			ClsGharardad.PriceNight = ShabPrice;
			ClsGharardad.PriceNobatKari1 = NobatPrice1;
			ClsGharardad.PriceNobatKari2 = NobatPrice2;
			ClsGharardad.PriceNobatKari3 = NobatPrice3;
			ClsGharardad.PriceHoliday = JomePrice;
			ClsGharardad.PriceTatil = TatilPrice;
			ClsGharardad.PriceJarime = JarimePrice;
			ClsGharardad.PriceLeave = VacationPrice;
			ClsGharardad.PriceAbsent = GheybatPrice;
			ClsGharardad.PriceMission = MissionPrice;
			ClsGharardad.PriceLowTime = KasrPrice1;
			ClsGharardad.PriceLowTime2 = KasrPrice2;
			ClsGharardad.PriceLowTime3 = KasrPrice3;
			FillGharardadFromFishDet();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private void FillGharardadFromFishDet()
	{
		try
		{
			ClsGharardad.ListGharardadDet.Clear();
			foreach (GharardadDetNew item in ListFishDet.Select((FishDetJson item) => new GharardadDetNew(ContextGuid)
			{
				HesabId = item.HesabId,
				nValue = ((item.TypeAccount == 1) ? item.Credit : item.Debit),
				Account = ((item.TypeAccount == 1) ? item.Credit : item.Debit),
				TypeAccount = item.TypeAccount,
				Name = item.Name,
				IsDecrease = item.IsDecrease,
				Formula = item.Formula,
				Privilege = item.Privilege,
				Desc = item.Description
			}))
			{
				ClsGharardad.ListGharardadDet.Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillGharardadFromFishDet", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 1983);
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		string param = "";
		try
		{
			if (EditST == 1 && IdSanad <= 0)
			{
				return false;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("", cn);
				try
				{
					line = 1;
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					line = 2;
					cmd.Parameters.AddWithValue("PersonID", (object)PersonId);
					line = 3;
					cmd.Parameters.AddWithValue("FishNO", (object)FishNo);
					line = 4;
					cmd.Parameters.AddWithValue("DateM", (object)DateM);
					line = 6;
					cmd.Parameters.AddWithValue("DateStartM", (object)DateStartM);
					line = 8;
					cmd.Parameters.AddWithValue("DateEndM", (object)DateEndM);
					line = 10;
					cmd.Parameters.AddWithValue("UID", (object)Uid);
					line = 11;
					cmd.Parameters.AddWithValue("IDSanad", (object)IdSanad.GetValueOrDefault());
					line = 12;
					cmd.Parameters.AddWithValue("Description", (object)Description);
					line = 13;
					cmd.Parameters.AddWithValue("St", (object)St);
					line = 13;
					cmd.Parameters.AddWithValue("VacationST", (object)VacationST);
					line = 13;
					cmd.Parameters.AddWithValue("EditST", (object)EditST);
					line = 14;
					cmd.Parameters.AddWithValue("KarkerdMin", (object)KarkerdMin);
					line = 15;
					cmd.Parameters.AddWithValue("KarkerdPrice", (object)KarkerdPrice);
					line = 16;
					cmd.Parameters.AddWithValue("EzafeMin", (object)EzafeMin);
					line = 16;
					cmd.Parameters.AddWithValue("EzafeMin1", (object)EzafeMin1);
					line = 16;
					cmd.Parameters.AddWithValue("EzafeMin2", (object)EzafeMin2);
					line = 16;
					cmd.Parameters.AddWithValue("EzafeMin3", (object)EzafeMin3);
					line = 17;
					cmd.Parameters.AddWithValue("EzafePrice", (object)EzafePrice);
					line = 17;
					cmd.Parameters.AddWithValue("EzafePrice1", (object)EzafePrice1);
					line = 17;
					cmd.Parameters.AddWithValue("EzafePrice2", (object)EzafePrice2);
					line = 17;
					cmd.Parameters.AddWithValue("EzafePrice3", (object)EzafePrice3);
					line = 18;
					cmd.Parameters.AddWithValue("ShabMin", (object)ShabMin);
					line = 19;
					cmd.Parameters.AddWithValue("ShabPrice", (object)ShabPrice);
					line = 20;
					cmd.Parameters.AddWithValue("NobatMin1", (object)NobatMin1);
					line = 20;
					cmd.Parameters.AddWithValue("NobatMin2", (object)NobatMin2);
					line = 20;
					cmd.Parameters.AddWithValue("NobatMin3", (object)NobatMin3);
					line = 21;
					cmd.Parameters.AddWithValue("NobatPrice1", (object)NobatPrice1);
					line = 21;
					cmd.Parameters.AddWithValue("NobatPrice2", (object)NobatPrice2);
					line = 21;
					cmd.Parameters.AddWithValue("NobatPrice3", (object)NobatPrice3);
					line = 22;
					cmd.Parameters.AddWithValue("JomeMin", (object)JomeMin);
					line = 22;
					cmd.Parameters.AddWithValue("TatilMin", (object)TatilMin);
					line = 22;
					cmd.Parameters.AddWithValue("Mozafimin", (object)MoazafiMin);
					line = 22;
					cmd.Parameters.AddWithValue("JarimeRoz", (object)JarimeRoz);
					line = 23;
					cmd.Parameters.AddWithValue("JomePrice", (object)JomePrice);
					line = 23;
					cmd.Parameters.AddWithValue("TatilPrice", (object)TatilPrice);
					line = 23;
					cmd.Parameters.AddWithValue("JarimePrice", (object)JarimePrice);
					line = 24;
					cmd.Parameters.AddWithValue("MorakhasiMin", (object)VacationMin);
					line = 24;
					SqlParameterCollection parameters = cmd.Parameters;
					parameters.AddWithValue("VacationRemind", (object)(await GetVacationRemindAaync(token)));
					line = 24;
					cmd.Parameters.AddWithValue("MorakhasiMinUsed", (object)VacationMinUsed);
					line = 25;
					cmd.Parameters.AddWithValue("MorakhasiPrice", (object)VacationPrice);
					line = 26;
					cmd.Parameters.AddWithValue("GheybatMin", (object)GheybatRoze);
					line = 27;
					cmd.Parameters.AddWithValue("GheybatPrice", (object)GheybatPrice);
					line = 28;
					cmd.Parameters.AddWithValue("KasrMin", (object)KasrMin);
					line = 28;
					cmd.Parameters.AddWithValue("KasrMin1", (object)KasrMin1);
					line = 28;
					cmd.Parameters.AddWithValue("KasrMin2", (object)KasrMin2);
					line = 28;
					cmd.Parameters.AddWithValue("KasrMin3", (object)KasrMin3);
					line = 29;
					cmd.Parameters.AddWithValue("KasrPrice", (object)KasrPrice);
					line = 29;
					cmd.Parameters.AddWithValue("KasrPrice1", (object)KasrPrice1);
					line = 29;
					cmd.Parameters.AddWithValue("KasrPrice2", (object)KasrPrice2);
					line = 29;
					cmd.Parameters.AddWithValue("KasrPrice3", (object)KasrPrice3);
					line = 30;
					cmd.Parameters.AddWithValue("IsPrint", (object)IsPrint);
					line = 31;
					cmd.Parameters.AddWithValue("SumKarkerd", (object)GetSumKolKarkerd());
					line = 32;
					cmd.Parameters.AddWithValue("SumMazaya", (object)SumMazaya);
					line = 33;
					cmd.Parameters.AddWithValue("SumKosorat", (object)SumKosorat);
					line = 34;
					cmd.Parameters.AddWithValue("SumKhales", (object)SumKhales);
					line = 35;
					cmd.Parameters.AddWithValue("CountKarKerd", (object)CountKarKerd);
					line = 35;
					cmd.Parameters.AddWithValue("CountKarKerdBime", (object)CountWorkdayBime);
					line = 35;
					cmd.Parameters.AddWithValue("CountKarKerdMaliat", (object)CountWorkdayMaliyat);
					line = 36;
					cmd.Parameters.AddWithValue("CountDays", (object)CountDaysOfMonth);
					line = 37;
					cmd.Parameters.AddWithValue("CountShift", (object)Countshift);
					line = 38;
					cmd.Parameters.AddWithValue("CountMorakhasiWithHoghogh", (object)CountMorakhasiWithSalary);
					line = 39;
					cmd.Parameters.AddWithValue("CountMorakhasiNotHoghogh", (object)CountMorakhasiNotSalary);
					line = 39;
					cmd.Parameters.AddWithValue("CountMorakhasi", (object)CountMorakhasi);
					line = 40;
					cmd.Parameters.AddWithValue("Countgheybat", (object)CountGheybat);
					line = 41;
					cmd.Parameters.AddWithValue("Countnaghes", (object)CountNaghes);
					line = 42;
					cmd.Parameters.AddWithValue("CountTatil", (object)CountTatil);
					line = 43;
					cmd.Parameters.AddWithValue("MissionPrice", (object)MissionPrice);
					line = 44;
					cmd.Parameters.AddWithValue("MissionRoz", (object)MissionRoz);
					line = 45;
					line = 46;
					JsonFishDet = FishDetJson.Serialize(ListFishDet);
					line = 47;
					cmd.Parameters.AddWithValue("JsonFishDet", (object)JsonFishDet);
					line = 48;
					await ((DbConnection)(object)cn).OpenAsync(token);
					param = ((IEnumerable)cmd.Parameters).Cast<SqlParameter>().Aggregate(param, (string current, SqlParameter parameter) => current + $" {((DbParameter)(object)parameter).ParameterName}={((DbParameter)(object)parameter).Value} ");
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_Fish_Insert]";
						line = 49;
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						line = 50;
						if (obj == null)
						{
							TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, "[ثبت فیش با خطا مواجه شد]", "", 3);
							return false;
						}
						Id = Convert.ToInt32(obj.ToString());
						line = 52;
						string pDesc = "[افزودن فیش حقوقی جدید]: " + ((PersonBase)ClsPerson).FullName + "  [از تاریخ]: " + Calendar.MiladiToCalture(DateStartM) + " [ToDate]: " + Calendar.MiladiToCalture(DateEndM);
						line = 53;
						await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc, "BillPrice.BillPriceInsert", Id.ToString(), Serialize(this));
					}
					else
					{
						line = 54;
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_Fish_Update]";
						line = 55;
						cmd.Parameters.AddWithValue("Id", (object)Id);
						line = 56;
						if (await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token) <= 0)
						{
							TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, $"[ویرایش فیش با شناسه] {Id}  [با خطا مواجه شد]", "", 3);
							return false;
						}
						line = 57;
						string pDesc2 = " [ویرایش فیش حقوقی]: " + ((PersonBase)ClsPerson).FullName + " [از تاریخ]: " + Calendar.MiladiToCalture(DateStartM) + "  [ToDate]: " + Calendar.MiladiToCalture(DateEndM);
						line = 58;
						await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc2, "BillPrice.BillPriceUpdate", Id.ToString(), Serialize(this));
						line = 59;
					}
					((DbConnection)(object)cn).Close();
					foreach (AgreedDeduction item in await GetAgreedDeducationsAsync(token))
					{
						foreach (AgreedDeductionDet agreedDeductionDet in item.AgreedDeductionDets)
						{
							line = 60;
							if (agreedDeductionDet.FishId == 0)
							{
								agreedDeductionDet.FishId = Id;
							}
						}
						line = 61;
						await item.SaveAsync(token);
						line = 62;
					}
					line = 63;
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
		catch (Exception ex)
		{
			Log.Error<string, string, int>(ex, $"line={line}***-parameters={param}" + ex.Message, "", "", line);
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Fish_DeleteRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)Id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					string pDesc = "[حذف فیش حقوقی]: " + ((PersonBase)ClsPerson).FullName + " [از تاریخ]: " + Calendar.MiladiToCalture(DateStartM) + "  [ToDate]: " + Calendar.MiladiToCalture(DateEndM);
					await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc, "BillPrice.BillPriceDelete", Id.ToString(), Serialize(this));
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<EnumkFishEdit.EnumFishEditSt> LockFishAsync(string contextGuid, int fishId, int userId, bool lockState, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Fish_Update_StEdit]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@FishID", (object)fishId);
					cmd.Parameters.AddWithValue("@UserID", (object)userId);
					cmd.Parameters.AddWithValue("@LockState", (object)lockState);
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					if (obj == null)
					{
						return EnumkFishEdit.EnumFishEditSt.Error;
					}
					if (!short.TryParse(obj.ToString(), out var result))
					{
						return EnumkFishEdit.EnumFishEditSt.Error;
					}
					return result switch
					{
						0 => EnumkFishEdit.EnumFishEditSt.Error, 
						1 => EnumkFishEdit.EnumFishEditSt.Ok, 
						2 => EnumkFishEdit.EnumFishEditSt.IsLockBefore, 
						3 => EnumkFishEdit.EnumFishEditSt.NotAccess, 
						_ => EnumkFishEdit.EnumFishEditSt.Error, 
					};
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task ConvertFishDetToJsonAsync(string contextGuid, CancellationToken token)
	{
		try
		{
			foreach (Fish fish in await GetAllAsync(contextGuid, new List<int>(), ConstantValues.MinDateM, DateTime.MaxValue, null, loadClsTaradod: false, token))
			{
				if (fish.EditST != 1)
				{
					fish._listFishDet = (await FishDet.GetAllAsync(contextGuid, fish.Id, token)).ConvertAll((FishDet p) => FishDetJson.FishDetToNew(contextGuid, p));
					await fish.SaveAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static Fish Deserialize(string jsonString)
	{
		return JsonConvert.DeserializeObject<Fish>(jsonString);
	}

	public static string Serialize(Fish fish)
	{
		return "";
	}

	public static async Task<List<Fish>> GetAllAsync(string contextGuid, List<int> personIDs, DateTime startDate, DateTime endDate, List<short> selectedUserIds, bool loadClsTaradod, CancellationToken token)
	{
		_ = 3;
		try
		{
			List<Fish> ret = new List<Fish>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Fish_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("personIDs", (object)string.Join(",", personIDs));
					cmd.Parameters.AddWithValue("startDate", (object)startDate);
					cmd.Parameters.AddWithValue("endDate", (object)endDate);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							List<Fish> list = ret;
							list.Add(await GetAsync(contextGuid, dr, loadClsTaradod, token));
						}
						((DbConnection)(object)cn).Close();
						if (selectedUserIds != null && selectedUserIds.Count > 0)
						{
							ret = (from gl in ret
								join fi in selectedUserIds on gl.Uid equals fi
								select gl).ToList();
						}
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

	public static async Task<List<Fish>> GetAllForInsuranceAsync(string contextGuid, List<int> personIDs, DateTime startDate, DateTime endDate, List<short> selectedUserIds, CancellationToken token)
	{
		_ = 3;
		try
		{
			List<Fish> ret = new List<Fish>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Fish_SelectAllForInsurance]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("personIDs", (object)string.Join(",", personIDs));
					cmd.Parameters.AddWithValue("startDate", (object)startDate);
					cmd.Parameters.AddWithValue("endDate", (object)endDate);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							List<Fish> list = ret;
							list.Add(await GetAsync(contextGuid, dr, loadClsTaradod: false, token));
						}
						((DbConnection)(object)cn).Close();
						if (selectedUserIds != null && selectedUserIds.Count > 0)
						{
							ret = (from gl in ret
								join fi in selectedUserIds on gl.Uid equals fi
								select gl).ToList();
						}
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<bool> CheckFishNoAsync(string contextGuid, int fishNo, int id, CancellationToken token)
	{
		_ = 1;
		try
		{
			if (id != 0)
			{
				return true;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select FishNO from [Fish] where FishNO=" + fishNo, cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					if (obj == null)
					{
						return true;
					}
					return false;
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<int> NextCodeAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select (isnull((select max(FishNO) from [Fish]),0))", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					int returnValue2 = 0;
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						returnValue2 = Convert.ToInt32(obj.ToString());
						returnValue2++;
					}
					((DbConnection)(object)cn).Close();
					return returnValue2;
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public decimal GetValueFishDet(int hesabid)
	{
		return ListFishDet.Where((FishDetJson q) => q.HesabId.Equals(hesabid)).Sum((FishDetJson p) => p.Credit - p.Debit);
	}

	public decimal GetValueEmployerinsurrance()
	{
		try
		{
			return ListFishDet.Where((FishDetJson q) => q.HesabId.Equals(5)).Sum((FishDetJson p) => p.Debit) * (decimal)_setting.LaborLaw.CoefficientInsuranceEmployer / (decimal)_setting.LaborLaw.CoefficientInsuranceInsured;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public decimal GetValueInsuranceUnemployment()
	{
		try
		{
			return ListFishDet.Where((FishDetJson q) => q.HesabId.Equals(5)).Sum((FishDetJson p) => p.Debit) * (decimal)_setting.LaborLaw.CoefficientInsuranceUnemployment / (decimal)_setting.LaborLaw.CoefficientInsuranceInsured;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetValueInsuranceUnemployment", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 2380);
			throw;
		}
	}

	public decimal GetValueTamin()
	{
		try
		{
			return ListFishDet.Where((FishDetJson q) => q.HesabId.Equals(5)).Sum((FishDetJson p) => p.Debit) * ((decimal)_setting.LaborLaw.CoefficientInsuranceEmployer + (decimal)_setting.LaborLaw.CoefficientInsuranceUnemployment + (decimal)_setting.LaborLaw.CoefficientInsuranceInsured) / (decimal)_setting.LaborLaw.CoefficientInsuranceInsured;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetValueTamin", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 2399);
			throw;
		}
	}

	public decimal GetValueTax()
	{
		try
		{
			return ListFishDet.Where((FishDetJson q) => q.HesabId.Equals(6)).Sum((FishDetJson p) => p.Debit);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetValueTax", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 2411);
			throw;
		}
	}

	public static async Task<Fish> FactoryAsync(string contextGuid, int personId, DateTime dateM1, DateTime dateM2, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			TaradodFull taradodFull = await TaradodFull.CreateAsync(contextGuid, dateM1, dateM2, await Person.GetByPersonIdAsync(contextGuid, personId, token), null, checkValidationVorodAndKHoroj: false, token);
			await taradodFull.FillTaradodFullPropertysAsync(token);
			return await FactoryAsync(contextGuid, taradodFull, token);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\Fish.cs", 2426);
			throw;
		}
	}

	public static async Task<Fish> FactoryAsync(string contextGuid, TaradodFull clsTaradodFull, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			Fish fish = new Fish(contextGuid)
			{
				ContextGuid = contextGuid
			};
			Fish fish2 = fish;
			fish2._setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
			fish._codeacc = fish._setting.CodeingAccounting;
			fish.ClsTaradod = clsTaradodFull;
			fish.ClsPerson = clsTaradodFull.ClsPerson;
			fish.PersonId = clsTaradodFull.PersonId;
			fish.DateStartM = clsTaradodFull.DateMin;
			fish.DateEndM = clsTaradodFull.DateMax;
			fish2 = fish;
			fish2.ClsGharardad = await GharardadNew.GetAsync(contextGuid, fish.PersonId, fish.DateStartM, fish.DateEndM, token);
			fish.CountKarKerd = clsTaradodFull.CountWorkdayHoghogh;
			fish.CountWorkdayBime = clsTaradodFull.CountWorkdayBime;
			fish.CountWorkdayMaliyat = clsTaradodFull.CountWorkdayMaliyat;
			fish.Countshift = clsTaradodFull.Countshift;
			fish.CountMorakhasiWithSalary = clsTaradodFull.CountMorakhasiWithSalary;
			fish.CountMorakhasi = clsTaradodFull.CountMorakhasi;
			fish.CountGheybat = clsTaradodFull.CountGheybat;
			fish.CountNaghes = clsTaradodFull.CountNaghes;
			fish.CountTatil = clsTaradodFull.CountTatil;
			fish.KasrMin1 = clsTaradodFull.SumKasrkarBegin;
			fish.KasrMin2 = clsTaradodFull.SumKasrkarMid;
			fish.KasrMin3 = clsTaradodFull.SumKasrkarEnd;
			fish.KasrMin = fish.KasrMin1 + fish.KasrMin2 + fish.KasrMin3;
			fish.VacationMinUsed = clsTaradodFull.SumWorkVacation;
			fish.VacationMin = (int)((fish.ClsGharardad?.LeaveDays ?? 0.0) * (double?)fish.ClsGharardad?.LeaveMinutes).GetValueOrDefault();
			fish.EzafeMin1 = clsTaradodFull.SumEzafekarBegin;
			fish.EzafeMin2 = clsTaradodFull.SumEzafekarMid;
			fish.EzafeMin3 = clsTaradodFull.SumEzafekarEnd;
			fish.EzafeMin = fish.EzafeMin1 + fish.EzafeMin2 + fish.EzafeMin3;
			fish.GheybatRoze = clsTaradodFull.CountGheybat;
			fish.ShabMin = clsTaradodFull.SumWorkNightWork;
			fish.NobatMin1 = clsTaradodFull.SumNobatKari1;
			fish.NobatMin2 = clsTaradodFull.SumNobatKari2;
			fish.NobatMin3 = clsTaradodFull.SumNobatKari3;
			fish.JomeMin = clsTaradodFull.SumFriDayWork;
			fish.TatilMin = clsTaradodFull.SumTatilDayWork;
			fish.MoazafiMin = clsTaradodFull.SumMoazzafi;
			fish.KarkerdMin = clsTaradodFull.SumKarkerd;
			fish.CountMorakhasiNotSalary = clsTaradodFull.CountMorakhasiNotSalary;
			double valueOrDefault = ((double)clsTaradodFull.SumWorkMission / (double?)fish.ClsGharardad?.LeaveMinutes).GetValueOrDefault();
			fish.MissionRoz = Math.Round((double)clsTaradodFull.MissionDaily + valueOrDefault, 1);
			fish.JarimeRoz = clsTaradodFull.JarimeRoz;
			await fish.LoadCLsFromGharardadAsync(token);
			return fish;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
