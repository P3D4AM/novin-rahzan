using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using FormCustom;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Attendance.Entities;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.Dal.Taradod;

public class TaradodFullDet : ITaradodFullDet, IHasContext
{
	private CoupleTaradod[] _coupleTaradodNextDay;

	private int _lunchTime = -1;

	private Color _colorKk = Color.DarkRed;

	private CoupleTaradod[] _coupleTaradodCurrentDay;

	private short _dayofShMonth;

	private TimeSpan? _ezafeKar;

	private TimeSpan? _kasreKar;

	private TimeSpan? _hozor;

	private int _onFriDayWorkKarkard;

	private int _onTatilWorkKarkard;

	private int _shiftWorkKarkard1;

	private int _shiftWorkKarkard2;

	private int _shiftWorkKarkard3;

	private int _nightWorkKarkard;

	private int _sumShiftMinuts;

	private List<ShiftDetView> _shiftDetViewsNextDay;

	private string _ekTime = "";

	private string _kkTime = "";

	private int? _sumMoazafiMinutes;

	private string taradodStatusName = "";

	private CancellationTokenSource _calcStatusTaradodToken;

	private SemaphoreSlim _calcStatusTaradodsemaphore = new SemaphoreSlim(1, 1);

	public CoupleTaradod[] CoupleTaradodCurrentDay => _coupleTaradodCurrentDay ?? (_coupleTaradodCurrentDay = CreateCoupleTaradodsCurrentDay());

	public List<DeviceInfo> Devices { get; set; }

	public TimeAttendanceSettings Setting { get; private set; }

	private CoupleTaradod[] CoupleTaradodNextDay => _coupleTaradodNextDay ?? (_coupleTaradodNextDay = CreateCoupleTaradodsNextDay());

	public IEnumerable<AttPersonLeaveBusiness> PersonLeaves { get; set; }

	public string ContextGuid { get; set; }

	public Person PersonInfo { get; set; }

	public List<HardwareCodeCUSView> PersonHardwareCodes { get; set; }

	public ATT_TaradodDet AttTaradodDet { get; private set; }

	public List<LogOnPersonel> TaradodsCurrentDay { get; private set; }

	private Holiday Holiday { get; set; }

	public List<LogOnPersonel> TaradodsNextDay { get; private set; }

	public List<LogOnPersonel> TaradodsYesterDay { get; private set; }

	public List<ShiftDetView> ShiftDetViews { get; private set; }

	public GharardadNew Contract { get; private set; }

	public bool ReloadNextRow { get; set; }

	public string TaradodStatusName
	{
		get
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Invalid comparison between Unknown and I4
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Invalid comparison between Unknown and I4
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Invalid comparison between Unknown and I4
			if (!string.IsNullOrEmpty(taradodStatusName) && taradodStatusName != "در حال پردازش...")
			{
				return taradodStatusName;
			}
			string text = "در حال پردازش...";
			if (TaradodStatus.HasValue)
			{
				text = Utilities.GetDisplay<StatuseTaradodEnumType>(TaradodStatus.Value);
			}
			string text2 = LanguageAndCalendar.Instance.GetValue(text);
			StatuseTaradodEnumType? taradodStatus = TaradodStatus;
			bool flag;
			if (taradodStatus.HasValue)
			{
				StatuseTaradodEnumType valueOrDefault = taradodStatus.GetValueOrDefault();
				if ((int)valueOrDefault == 5 || (int)valueOrDefault == 14)
				{
					flag = true;
					goto IL_0085;
				}
			}
			flag = false;
			goto IL_0085;
			IL_0085:
			if (flag && Morakhasi != null)
			{
				text2 = Morakhasi.TypeMorakhasiName;
				if (Morakhasi.Desc.Trim().Length > 0)
				{
					text2 = text2 + ":" + Morakhasi.Desc.Trim();
				}
			}
			taradodStatus = TaradodStatus;
			if (taradodStatus.HasValue && (int)taradodStatus.GetValueOrDefault() == 6)
			{
				text2 = TatilRasmiToolTip ?? "";
			}
			taradodStatusName = text2;
			return taradodStatusName;
		}
	}

	public string ManulaEzafeKarTime => Time.IntToTime(AttTaradodDet?.EKTime ?? 0);

	public string ManulaKasreKarTime => Time.IntToTime(AttTaradodDet?.KKTime ?? 0);

	public string ManulaMissionTime => Time.IntToTime(AttTaradodDet?.MissionTime ?? 0);

	public string ManulaVacationTime => Time.IntToTime(AttTaradodDet?.VacationTime ?? 0);

	public string ManulaFridayTime => Time.IntToTime(AttTaradodDet?.FridayTime ?? 0);

	public string ManulaShiftTime1 => Time.IntToTime(AttTaradodDet?.ShiftTime1 ?? 0);

	public string ManulaShiftTime2 => Time.IntToTime(AttTaradodDet?.ShiftTime2 ?? 0);

	public string ManulaShiftTime3 => Time.IntToTime(AttTaradodDet?.ShiftTime3 ?? 0);

	public string ManulaAbsentTime => Time.IntToTime(AttTaradodDet?.AbsentTime ?? 0);

	public string ManulaNightTime => Time.IntToTime(AttTaradodDet?.NightTime ?? 0);

	public Color ColorManulaEzafeKar
	{
		get
		{
			if ((AttTaradodDet?.EKTime ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaKasreKarTime
	{
		get
		{
			if ((AttTaradodDet?.KKTime ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaMissionTime
	{
		get
		{
			if ((AttTaradodDet?.MissionTime ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaVacationTime
	{
		get
		{
			ATT_TaradodDet attTaradodDet = AttTaradodDet;
			if (attTaradodDet == null || attTaradodDet.VacationTime < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaFridayTime
	{
		get
		{
			ATT_TaradodDet attTaradodDet = AttTaradodDet;
			if (attTaradodDet == null || attTaradodDet.FridayTime < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaShiftTime1
	{
		get
		{
			if ((AttTaradodDet?.ShiftTime1 ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaShiftTime2
	{
		get
		{
			if ((AttTaradodDet?.ShiftTime2 ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaShiftTime3
	{
		get
		{
			if ((AttTaradodDet?.ShiftTime3 ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaAbsentTime
	{
		get
		{
			if ((AttTaradodDet?.AbsentTime ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public Color ColorManulaNightTime
	{
		get
		{
			if ((AttTaradodDet?.NightTime ?? 0) < 0)
			{
				return Color.LightPink;
			}
			return Color.FromArgb(255, 206, 255, 206);
		}
	}

	public string MorSaatiTime => Time.IntToTime(WorkOnTypeTaradod((TypeTaradodEnumType)5));

	public string MissionTime => Time.IntToTime(WorkOnTypeTaradod((TypeTaradodEnumType)7));

	public long CodeTafsil => ((PersonBase)PersonInfo).CodeTafsil;

	public string FullName => ((PersonBase)PersonInfo).FullName;

	public string Family => ((PersonBase)PersonInfo).Family;

	public string FirstName => ((PersonBase)PersonInfo).Name;

	public DateTime DateM { get; set; }

	public string ShiftTime => Time.IntToTime(SumShiftMinuts);

	public string Dayofweek => LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(DateM);

	public bool CanEditV => UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodMain;

	public bool CanEditK => UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodMain;

	public Color ColorEk { get; set; } = Color.DarkGreen;


	public string DateSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateM);

	public int DetailStatusTypeTaradod { get; set; }

	public string TatilRasmiToolTip
	{
		get
		{
			try
			{
				object result;
				if (Holiday != null)
				{
					Holiday holiday = Holiday;
					result = ((holiday != null && holiday.StRasmi && Holiday.DateM.DayOfWeek != DayOfWeek.Friday) ? Holiday.Monasebat : null);
				}
				else
				{
					result = null;
				}
				return (string)result;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	public string MonasebatToolTip => Holiday?.Monasebat ?? "";

	public string MorakhasiToolTip
	{
		get
		{
			object obj;
			if (Contract != null)
			{
				obj = Morakhasi?.Desc;
				if (obj == null)
				{
					return "";
				}
			}
			else
			{
				obj = "";
			}
			return (string)obj;
		}
	}

	public string ManualMissinToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.MissionTime != 0) ? attTaradodDet.MissionDesc : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualMissinToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 218);
				throw;
			}
		}
	}

	public string ManualVacationToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.VacationTime != 0) ? attTaradodDet.VacationDesc : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualVacationToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 236);
				throw;
			}
		}
	}

	public string ManualKasreKarToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.KKTime != 0) ? attTaradodDet.KKDesc : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualKasreKarToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 254);
				throw;
			}
		}
	}

	public string ManualEzafeKarToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.EKTime != 0) ? attTaradodDet.EKDesc : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualEzafeKarToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 272);
				throw;
			}
		}
	}

	public string ManualFriDayToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.FridayTime != 0) ? attTaradodDet.FridayDesc : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualFriDayToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 290);
				throw;
			}
		}
	}

	public string ManualShift1ToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.ShiftTime1 != 0) ? attTaradodDet.ShiftDesc1 : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualShift1ToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 308);
				throw;
			}
		}
	}

	public string ManualShift2ToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.ShiftTime2 != 0) ? attTaradodDet.ShiftDesc2 : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualShift2ToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 326);
				throw;
			}
		}
	}

	public string ManualShift3ToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.ShiftTime3 != 0) ? attTaradodDet.ShiftDesc3 : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualShift3ToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 344);
				throw;
			}
		}
	}

	public string ManualAbsentToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.AbsentTime != 0) ? attTaradodDet.AbsentDesc : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualAbsentToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 362);
				throw;
			}
		}
	}

	public string ManualNightToolTip
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return "";
				}
				ATT_TaradodDet attTaradodDet = AttTaradodDet;
				return (attTaradodDet.NightTime != 0) ? attTaradodDet.NightDesc : "";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ManualNightToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 380);
				throw;
			}
		}
	}

	public short DayofShMonth
	{
		get
		{
			try
			{
				string text = Calendar.MiladiToCalture(DateM);
				_dayofShMonth = (short)LanguageAndCalendar.Instance.CaltureCalender.GetDayOfDate(text);
				return _dayofShMonth;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DayofShMonth", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 415);
				throw;
			}
		}
	}

	public Morakhasidaily Morakhasi { get; private set; }

	public Morakhasidaily MorakhasiNextDay { get; private set; }

	public List<CoupleTaradod> CoupleTaradodReadOnly => CoupleTaradodCurrentDay.Where((CoupleTaradod p) => p.VTime != p.KTime).ToList();

	public int KasreKarBeginShift { get; set; }

	public int JarimeRoz { get; set; }

	public int KasreKarEndShift { get; set; }

	public int KasreKarMidShift { get; set; }

	public int EzafeKarBeginShift { get; set; }

	public int EzafeKarEndShift { get; set; }

	public int EzafeKarMidShift { get; set; }

	public TimeSpan? Hozor => _hozor;

	public TimeSpan? EzafeKar => _ezafeKar;

	public TimeSpan? KasreKar => _kasreKar;

	public StatuseTaradodEnumType? TaradodStatus { get; private set; }

	public int LunchTime
	{
		get
		{
			try
			{
				if (_lunchTime < 0)
				{
					ShiftDetView shiftDetView = ShiftDetViews?.FirstOrDefault((ShiftDetView q) => q.ClsShiftCoding.LunchTime > 0);
					_lunchTime = ((shiftDetView != null) ? CalcLunchTime(shiftDetView) : 0);
				}
				return _lunchTime;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LunchTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 737);
				throw;
			}
		}
	}

	public int SumShiftMinuts
	{
		get
		{
			try
			{
				if (Contract == null)
				{
					return 0;
				}
				_sumShiftMinuts = CalcShiftDetViewsTime();
				return _sumShiftMinuts;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SumShiftMinuts", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 753);
				throw;
			}
		}
	}

	private async Task<List<ShiftDetView>> GetShiftDetViewsNextDayAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (Contract == null)
			{
				return null;
			}
			if (_shiftDetViewsNextDay == null)
			{
				_shiftDetViewsNextDay = await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, DateM.Date + new TimeSpan(1, 23, 59, 59), Contract.ID, Contract.PersonId, token);
			}
			await FixShiftCodingIfHasVacationAsync(ContextGuid, "GetShiftDetViewsNextDayAsync", DateM.Date + new TimeSpan(1, 23, 59, 59), ((PersonBase)PersonInfo).Id, Setting.CalcEkWhenHasVacationOrMission, MorakhasiNextDay?.Id ?? 0, _shiftDetViewsNextDay, token);
			return _shiftDetViewsNextDay;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetShiftDetViewsNextDayAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 64);
			throw;
		}
	}

	public async Task<string> GetKkTimeAsync(CancellationToken token)
	{
		return Time.IntToTime(await GetKasrKarAsync(token));
	}

	public async Task<string> GetEkTimeAsync(CancellationToken token)
	{
		return Time.IntToTime(await GetEzafeKarAsync(token));
	}

	public async Task<string> GetHozorTimeAsync(CancellationToken token)
	{
		return Time.IntToTime(await GetKarkardAsync(token));
	}

	public async Task<string> GetGheybatTimeAsync(CancellationToken token)
	{
		try
		{
			return Time.IntToTime(await GetGheybatMinAsync(token));
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

	public async Task<int> GetGheybatMinAsync(CancellationToken token)
	{
		try
		{
			if (Contract == null || AttTaradodDet == null)
			{
				return 0;
			}
			ATT_TaradodDet attDet = AttTaradodDet;
			int sumShiftMin = SumShiftMinuts;
			return (await GetKarkardAsync(token) <= 0 && sumShiftMin > 0) ? (sumShiftMin + attDet.AbsentTime) : attDet.AbsentTime;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetGheybatMinAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 398);
			throw;
		}
	}

	public Color GetColorKk()
	{
		try
		{
			return (_kasreKar.Value.TotalMinutes < 0.0) ? StringExtensions.ToColor(CoupleTaradod.ClsColor.UnCorrectTaradodColor, default(Color)) : _colorKk;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<int> GetKasrKarAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (_kasreKar.HasValue)
			{
				return (int)_kasreKar.Value.TotalMinutes;
			}
			bool flag = Contract == null;
			if (!flag)
			{
				StatuseTaradodEnumType val = await GetStatuseTaradodAsync(token);
				bool flag2 = ((val - 2 <= 1 || val - 11 <= 2 || val - 18 <= 2) ? true : false);
				flag = flag2;
			}
			if (flag)
			{
				_kasreKar = new TimeSpan(0L);
				TaradodFullDet taradodFullDet = this;
				TaradodFullDet taradodFullDet2 = this;
				int num2 = (KasreKarEndShift = 0);
				int kasreKarBeginShift = (taradodFullDet2.KasreKarMidShift = num2);
				taradodFullDet.KasreKarBeginShift = kasreKarBeginShift;
				return (int)_kasreKar.Value.TotalMinutes;
			}
			int num4 = await GetKarkardAsync(token);
			List<ShiftDetView> shiftDetViews = ShiftDetViews;
			if (shiftDetViews == null || shiftDetViews.Count <= 0 || num4 <= 0 || Contract.ShiftId == 0)
			{
				_kasreKar = default(TimeSpan);
				TaradodFullDet taradodFullDet3 = this;
				TaradodFullDet taradodFullDet4 = this;
				int num2 = (KasreKarEndShift = 0);
				int kasreKarBeginShift = (taradodFullDet4.KasreKarMidShift = num2);
				taradodFullDet3.KasreKarBeginShift = kasreKarBeginShift;
				return (int)_kasreKar.Value.TotalMinutes;
			}
			ATT_TaradodDet attDet = AttTaradodDet ?? new ATT_TaradodDet(ContextGuid);
			ShiftCodeing clsShiftCoding = ShiftDetViews[0].ClsShiftCoding;
			int sumShiftMinuts = SumShiftMinuts;
			if ((int)clsShiftCoding.TypeShift == 2)
			{
				int lunchTime = LunchTime;
				short lunchTime2 = clsShiftCoding.LunchTime;
				if (sumShiftMinuts - lunchTime2 + attDet.KKTime > num4 - lunchTime)
				{
					_kasreKar = TimeSpan.FromMinutes(sumShiftMinuts - num4);
					if (lunchTime > 0)
					{
						_kasreKar = _kasreKar.Value.Add(TimeSpan.FromMinutes(lunchTime - lunchTime2));
					}
					_kasreKar = _kasreKar.Value.Add(TimeSpan.FromMinutes(attDet.KKTime));
					KasreKarBeginShift = (int)_kasreKar.Value.TotalMinutes;
					TaradodFullDet taradodFullDet5 = this;
					int kasreKarBeginShift = (KasreKarEndShift = 0);
					taradodFullDet5.KasreKarMidShift = kasreKarBeginShift;
				}
				else
				{
					_kasreKar = default(TimeSpan);
					TaradodFullDet taradodFullDet6 = this;
					TaradodFullDet taradodFullDet7 = this;
					int num2 = (KasreKarEndShift = 0);
					int kasreKarBeginShift = (taradodFullDet7.KasreKarMidShift = num2);
					taradodFullDet6.KasreKarBeginShift = kasreKarBeginShift;
				}
				return (int)_kasreKar.Value.TotalMinutes;
			}
			_kasreKar = TimeSpan.FromMinutes(await CalKasrKarAsync(token));
			_kasreKar = _kasreKar.Value.Add(TimeSpan.FromMinutes(attDet.KKTime));
			if (-attDet.KKTime <= KasreKarBeginShift)
			{
				KasreKarBeginShift += attDet.KKTime;
			}
			else if (-attDet.KKTime <= KasreKarMidShift)
			{
				KasreKarMidShift += attDet.KKTime;
			}
			else if (-attDet.KKTime <= KasreKarEndShift)
			{
				KasreKarEndShift += attDet.KKTime;
			}
			foreach (ShiftDetView item in ShiftDetViews.Where((ShiftDetView q) => q.ClsShiftCoding.LunchTime > 0 && q.ClsShiftCoding.Vorod.TimeOfDay != q.ClsShiftCoding.Khoroj.TimeOfDay))
			{
				int num10 = item.GetShiftLunchMinutes - item.GetLunchMinutes(CoupleTaradodReadOnly);
				_kasreKar = _kasreKar.Value.Add(-TimeSpan.FromMinutes(num10));
				if (KasreKarMidShift >= num10)
				{
					KasreKarMidShift -= num10;
				}
				else if (KasreKarEndShift >= num10)
				{
					KasreKarEndShift -= num10;
				}
				else if (KasreKarBeginShift >= num10)
				{
					KasreKarBeginShift -= num10;
				}
			}
			if (_kasreKar.Value < new TimeSpan(0L))
			{
				_kasreKar = new TimeSpan(0L);
				TaradodFullDet taradodFullDet8 = this;
				TaradodFullDet taradodFullDet9 = this;
				int num2 = (KasreKarEndShift = 0);
				int kasreKarBeginShift = (taradodFullDet9.KasreKarMidShift = num2);
				taradodFullDet8.KasreKarBeginShift = kasreKarBeginShift;
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetKasrKarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 536);
			throw;
		}
		return _kasreKar.HasValue ? ((int)_kasreKar.Value.TotalMinutes) : 0;
	}

	public async Task<int> GetEzafeKarAsync(CancellationToken token)
	{
		_ = 8;
		try
		{
			ATT_TaradodDet attDet = AttTaradodDet;
			short? num = Contract?.ShiftId;
			if (num.HasValue && num.GetValueOrDefault() == 0)
			{
				_ezafeKar = TimeSpan.FromMinutes(attDet?.EKTime ?? 0);
				EzafeKarEndShift = (int)_ezafeKar.Value.TotalMinutes;
				return (int)_ezafeKar.Value.TotalMinutes;
			}
			if (!_ezafeKar.HasValue)
			{
				StatuseTaradodEnumType statusTaradod = await GetStatuseTaradodAsync(token);
				ShiftDetView shiftDetView = ShiftDetViews?.FirstOrDefault();
				ShiftCodeing firstShiftCoding = null;
				if (shiftDetView != null)
				{
					firstShiftCoding = shiftDetView.ClsShiftCoding;
				}
				if (Contract != null)
				{
					List<ShiftDetView> shiftDetViews = ShiftDetViews;
					if (shiftDetViews != null && shiftDetViews.Count > 0 && !firstShiftCoding.NotCalcEKForShift && (int)firstShiftCoding.TypeShift == 2)
					{
						int karkard = await GetKarkardAsync(token);
						int sumShiftMin = SumShiftMinuts;
						if (sumShiftMin == 0)
						{
							_ezafeKar = ((!Setting.CodeingAccounting.IsCalcHoliday) ? TimeSpan.FromMinutes(karkard) : new TimeSpan(0L));
						}
						else if (karkard > (sumShiftMin + attDet?.KKTime).GetValueOrDefault() && (int)statusTaradod != 2 && (int)statusTaradod != 3)
						{
							int fridaywork = await GetOnFriDayWorkKarkardAsync(token);
							int tatilWork = await GetOnTatilWorkKarkardAsync(token);
							int shiftWork1 = await GetShiftWorkKarkard1Aaync(token);
							int shiftWork2 = await GetShiftWorkKarkard2Async(token);
							int shiftWork3 = await GetShiftWorkKarkard3Async(token);
							int num2 = await GetNightWorkKarkardAsync(token);
							_ezafeKar = TimeSpan.FromMinutes(karkard - sumShiftMin - (attDet?.KKTime ?? 0) - fridaywork - tatilWork - shiftWork1 - shiftWork2 - shiftWork3 - num2);
							if (_ezafeKar.Value < new TimeSpan(0L))
							{
								_ezafeKar = new TimeSpan(0L);
							}
							if (_ezafeKar.Value > new TimeSpan(0L) && firstShiftCoding.PeakOvertime > 0 && _ezafeKar.Value > TimeSpan.FromMinutes(firstShiftCoding.PeakOvertime))
							{
								_ezafeKar = TimeSpan.FromMinutes(firstShiftCoding.PeakOvertime);
							}
						}
						else
						{
							_ezafeKar = new TimeSpan(0L);
						}
						EzafeKarEndShift = (int)_ezafeKar.Value.TotalMinutes;
						TaradodFullDet taradodFullDet = this;
						int ezafeKarBeginShift = (EzafeKarMidShift = 0);
						taradodFullDet.EzafeKarBeginShift = ezafeKarBeginShift;
						goto IL_072b;
					}
				}
				_ezafeKar = TimeSpan.FromMinutes(await CalEzafeKarAsync(token));
				goto IL_072b;
			}
			goto IL_0782;
			IL_0782:
			if (_ezafeKar.HasValue)
			{
				return (int)_ezafeKar.Value.TotalMinutes;
			}
			goto end_IL_003d;
			IL_072b:
			if (attDet != null)
			{
				_ezafeKar = _ezafeKar.Value.Add(TimeSpan.FromMinutes(attDet.EKTime));
				EzafeKarEndShift += attDet.EKTime;
			}
			goto IL_0782;
			end_IL_003d:;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetEzafeKarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 614);
			throw;
		}
		return 0;
	}

	public async Task<int> GetKarkardAsync(CancellationToken token)
	{
		try
		{
			if (!_hozor.HasValue)
			{
				await CalcKarkerdAsync(token);
			}
			return (int)_hozor.Value.TotalMinutes;
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

	public async Task<int> GetOnFriDayWorkKarkardAsync(CancellationToken token)
	{
		try
		{
			_ = _onFriDayWorkKarkard;
			return _onFriDayWorkKarkard;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<int> GetOnTatilWorkKarkardAsync(CancellationToken token)
	{
		try
		{
			_ = _onTatilWorkKarkard;
			return _onTatilWorkKarkard;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<int> GetShiftWorkKarkard1Aaync(CancellationToken token)
	{
		try
		{
			_ = _shiftWorkKarkard1;
			return _shiftWorkKarkard1;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<int> GetShiftWorkKarkard2Async(CancellationToken token)
	{
		try
		{
			_ = _shiftWorkKarkard2;
			return _shiftWorkKarkard2;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<int> GetShiftWorkKarkard3Async(CancellationToken token)
	{
		try
		{
			_ = _shiftWorkKarkard3;
			return _shiftWorkKarkard3;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<int> GetNightWorkKarkardAsync(CancellationToken token)
	{
		try
		{
			_ = _nightWorkKarkard;
			return _nightWorkKarkard;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public int GetSumMoazafiMinutes()
	{
		try
		{
			if (Contract == null)
			{
				return 0;
			}
			int valueOrDefault = _sumMoazafiMinutes.GetValueOrDefault();
			if (!_sumMoazafiMinutes.HasValue)
			{
				valueOrDefault = CalcMoazafiTime();
				_sumMoazafiMinutes = valueOrDefault;
			}
			return _sumMoazafiMinutes.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetSumMoazafiMinutes", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 769);
			throw;
		}
	}

	public async Task<StatuseTaradodEnumType> GetStatuseTaradodAsync(CancellationToken internalToken = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			_calcStatusTaradodToken?.Cancel();
			_calcStatusTaradodToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { internalToken });
			CancellationToken token = _calcStatusTaradodToken.Token;
			token.ThrowIfCancellationRequested();
			await _calcStatusTaradodsemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			if ((int)TaradodStatus.GetValueOrDefault() == 0)
			{
				StatuseTaradodEnumType value = await CalcStatuseTaradodAsync(token);
				token.ThrowIfCancellationRequested();
				TaradodStatus = value;
			}
			return TaradodStatus.Value;
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
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_calcStatusTaradodsemaphore.Release();
			}
		}
	}

	public static async Task<TaradodFullDetView> GetAsync_(string contextGuid, List<DeviceInfo> devices, DateTime date, Person person, GharardadNew pClsGharardad, bool checkValidationVorodAndKHoroj, CancellationToken token = default(CancellationToken))
	{
		try
		{
			TaradodFullDetView ret = new TaradodFullDetView();
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ret.Devices = devices;
			TaradodFullDetView taradodFullDetView = ret;
			taradodFullDetView.Setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
			new StringBuilder().AppendLine($"method TaradodFullDet in {DateTime.Now}");
			ret.ContextGuid = contextGuid;
			ret.DateM = date;
			ret.Contract = pClsGharardad;
			ret.PersonInfo = person;
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			IEnumerable<AttPersonLeaveBusiness> source = await AttPersonLeaveBusiness.GetAllAsync(ret.ContextGuid, default(CancellationToken), "");
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			(from p in source
				where p.PersonGuid == ((PersonBase)person).Guid
				orderby p.DateFrom descending
				select p).ToList();
			ret.PersonLeaves = source.Where((AttPersonLeaveBusiness p) => p.PersonGuid == ((PersonBase)person).Guid && (int)p.Type == 0 && ((int)p.LeaveStatus == 0 || (int)p.LeaveStatus == 2) && p.DateFrom.Date <= date.Date && date.Date <= p.DateTo.Date).ToList();
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			taradodFullDetView = ret;
			taradodFullDetView.Morakhasi = await Morakhasidaily.GetAsync(contextGuid, date, ((PersonBase)person).Id, token);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			taradodFullDetView = ret;
			taradodFullDetView.AttTaradodDet = await ATT_TaradodDet.GetAsync(contextGuid, ((PersonBase)person).Id, ret.DateM, token);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			taradodFullDetView = ret;
			taradodFullDetView.TaradodsCurrentDay = await LogOnPersonel.GetAllByPersonIdAsync(contextGuid, ((PersonBase)person).Id, ret.DateM, token);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			taradodFullDetView = ret;
			taradodFullDetView.TaradodsNextDay = await LogOnPersonel.GetAllByPersonIdAsync(contextGuid, ((PersonBase)person).Id, date.AddDays(1.0), token);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			taradodFullDetView = ret;
			taradodFullDetView.Holiday = await Holiday.GetAsync(contextGuid, date, token);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			if (pClsGharardad != null)
			{
				taradodFullDetView = ret;
				taradodFullDetView.ShiftDetViews = await ShiftDetView.GetShiftDetViewListAsync(contextGuid, ret.DateM, pClsGharardad.ID, ((PersonBase)person).Id, token);
				if (token.IsCancellationRequested)
				{
					return ret;
				}
				await FixShiftCodingIfHasVacationAsync(contextGuid, "GetAsync_", ret.DateM, ((PersonBase)person).Id, ret.Setting.CalcEkWhenHasVacationOrMission, ret.Morakhasi?.Id ?? 0, ret.ShiftDetViews, token);
			}
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			if (checkValidationVorodAndKHoroj)
			{
				CheckValidationVorodAndKHoroj(ret.CoupleTaradodCurrentDay, token);
			}
			await ret.CalcEk_Kk_DataAsync(token);
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

	public static async Task<TaradodFullDetView> GetAsync_Improved(string contextGuid, TimeAttendanceSettings settings, List<AttPersonLeaveBusiness> allPersonLeaves, List<Morakhasidaily> allMorakhasidailies, List<LogOnPersonel> allLogs, List<ATT_TaradodDet> allTaradodDets, List<ShiftDetView> shiftDetViewsPerson, List<DeviceInfo> allDeviceInfo, Holiday holiday, DateTime date, Person person, GharardadNew pClsGharardad, bool checkValidationVorodAndKHoroj, CancellationToken token = default(CancellationToken))
	{
		try
		{
			TaradodFullDetView ret = new TaradodFullDetView
			{
				Devices = allDeviceInfo
			};
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ret.Setting = settings;
			new StringBuilder().AppendLine($"method TaradodFullDet in {DateTime.Now}");
			ret.ContextGuid = contextGuid;
			ret.DateM = date;
			ret.Contract = pClsGharardad;
			ret.PersonInfo = person;
			ret.ShiftDetViews = shiftDetViewsPerson;
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			(from p in allPersonLeaves
				where p.PersonGuid == ((PersonBase)person).Guid
				orderby p.DateFrom descending
				select p).ToList();
			ret.PersonLeaves = allPersonLeaves.Where((AttPersonLeaveBusiness p) => p.PersonGuid == ((PersonBase)person).Guid && (int)p.Type == 0 && ((int)p.LeaveStatus == 0 || (int)p.LeaveStatus == 2) && p.DateFrom.Date <= date.Date && date.Date <= p.DateTo.Date).ToList();
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ret.Morakhasi = allMorakhasidailies?.SingleOrDefault((Morakhasidaily p) => p.PersonId == ((PersonBase)person).Id && p.DateFrom == date);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ret.AttTaradodDet = allTaradodDets?.SingleOrDefault((ATT_TaradodDet p) => p.DateM == date && p.PersonId == ((PersonBase)person).Id);
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ret.TaradodsCurrentDay = allLogs?.Where((LogOnPersonel p) => p.DateTimeLogM >= date.Date && p.DateTimeLogM < date.Date.AddDays(1.0) && p.PersonId == ((PersonBase)person).Id).ToList();
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ret.TaradodsNextDay = allLogs?.Where((LogOnPersonel p) => p.DateTimeLogM >= date.Date.AddDays(1.0) && p.DateTimeLogM < date.Date.AddDays(2.0) && p.PersonId == ((PersonBase)person).Id).ToList();
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			ret.Holiday = holiday;
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			if (pClsGharardad != null)
			{
				await FixShiftCodingIfHasVacationAsync(contextGuid, "GetAsync_Improved", ret.DateM, ((PersonBase)person).Id, ret.Setting.CalcEkWhenHasVacationOrMission, ret.Morakhasi?.Id ?? 0, ret.ShiftDetViews, token);
			}
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			if (checkValidationVorodAndKHoroj)
			{
				CheckValidationVorodAndKHoroj(ret.CoupleTaradodCurrentDay, token);
			}
			await ret.CalcEk_Kk_DataAsync(token);
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

	public static async Task<TaradodFullDetView> GetAsyncNew(string contextGuid, TimeAttendanceSettings setting, DateTime currentDate, bool checkValidationVorodAndKHoroj, IEnumerable<AttPersonLeaveBusiness> AllPersonLeaveBusiness, Person person, GharardadNew pClsGharardad, Morakhasidaily morakhasidailyCurrentDate, Morakhasidaily morakhasidailyNextDate, ATT_TaradodDet aTT_TaradodDetCurrentDate, List<DeviceInfo> allDeviceInfo, Dictionary<DateTime, List<LogOnPersonel>> allLogonPersonels, Holiday holidayCurrentDay, List<ShiftDetView> shiftDetViewsCurrentDate, CancellationToken token = default(CancellationToken))
	{
		try
		{
			allLogonPersonels.TryGetValue(currentDate.Date, out var taradodsCurrentDay);
			allLogonPersonels.TryGetValue(currentDate.Date.AddDays(1.0), out var taradodsNextDay);
			List<HardwareCodeCUSView> personHardwareCodes = (await person.GetHardWareCodesAsync())?.ToList();
			if (morakhasidailyCurrentDate != null && morakhasidailyCurrentDate.Id > 0)
			{
				await FixShiftCodingIfHasVacationAsync(contextGuid, "", currentDate, ((PersonBase)person).Id, CalcEkWhenHasVacationOrMission: true, morakhasidailyCurrentDate.Id, shiftDetViewsCurrentDate, token);
			}
			TaradodFullDetView ret = new TaradodFullDetView
			{
				Devices = allDeviceInfo,
				Setting = setting,
				ContextGuid = contextGuid,
				DateM = currentDate,
				Contract = pClsGharardad,
				PersonInfo = person,
				PersonHardwareCodes = personHardwareCodes,
				PersonLeaves = AllPersonLeaveBusiness.Where((AttPersonLeaveBusiness p) => p.PersonGuid == ((PersonBase)person).Guid && (int)p.Type == 0 && ((int)p.LeaveStatus == 0 || (int)p.LeaveStatus == 2) && p.DateFrom.Date <= currentDate.Date && currentDate.Date <= p.DateTo.Date).ToList(),
				Morakhasi = morakhasidailyCurrentDate,
				MorakhasiNextDay = morakhasidailyNextDate,
				AttTaradodDet = (aTT_TaradodDetCurrentDate ?? new ATT_TaradodDet(contextGuid)),
				TaradodsCurrentDay = taradodsCurrentDay?.OrderBy((LogOnPersonel p) => p.DateTimeLogM).ToList(),
				TaradodsNextDay = taradodsNextDay?.OrderBy((LogOnPersonel p) => p.DateTimeLogM).ToList(),
				Holiday = holidayCurrentDay,
				ShiftDetViews = shiftDetViewsCurrentDate
			};
			await ret.CalcTaradodAsyncNew(token);
			allLogonPersonels.Remove(currentDate.Date);
			allLogonPersonels.Add(currentDate.Date, ret.TaradodsCurrentDay);
			allLogonPersonels.Remove(currentDate.Date.AddDays(1.0));
			allLogonPersonels.Add(currentDate.Date.AddDays(1.0), ret.TaradodsNextDay);
			if (checkValidationVorodAndKHoroj)
			{
				CheckValidationVorodAndKHoroj(ret.CoupleTaradodCurrentDay, token);
			}
			await ret.CalcEk_Kk_DataAsync(token);
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

	public static void CheckValidationVorodAndKHoroj(CoupleTaradod[] coupleTaradods, CancellationToken token)
	{
		try
		{
			foreach (CoupleTaradod coupleTaradod in coupleTaradods)
			{
				if (token.IsCancellationRequested)
				{
					break;
				}
				coupleTaradod.CheckValidVk();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task<int> CalKasrKarAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			int sumKk = 0;
			if (Contract == null)
			{
				return 0;
			}
			if (Setting.NotCalkEK_KK)
			{
				return 0;
			}
			if (ShiftDetViews == null || ShiftDetViews.Count == 0)
			{
				return 0;
			}
			foreach (ShiftDetView shift in ShiftDetViews)
			{
				ShiftCodeing shiftCoding = shift.ClsShiftCoding;
				if (shiftCoding.Vorod >= shiftCoding.Khoroj || (shiftCoding.CalcEnteghalInSrcDate && shiftCoding.Vorod.TimeOfDay <= new TimeSpan(0, 0, 0, 1)))
				{
					continue;
				}
				if (shiftCoding.NextDay && shiftCoding.Vorod.TimeOfDay <= new TimeSpan(0, 0, 0, 1))
				{
					Morakhasidaily obj = await Morakhasidaily.GetAsync(ContextGuid, DateM.AddDays(-1.0).Date, ((PersonBase)PersonInfo).Id, token);
					if (obj != null && obj.Id > 0)
					{
						continue;
					}
				}
				var (num, num2, num3) = await CalcKasrOfShiftAsync(shift, CoupleTaradodCurrentDay, token);
				if (num > 0)
				{
					KasreKarBeginShift += num;
					sumKk += num;
					if (shiftCoding.ChkJarime && (!shiftCoding.NextDay || !(shiftCoding.Vorod.TimeOfDay <= new TimeSpan(0, 0, 0, 1))) && num > shiftCoding.JarimeV)
					{
						JarimeRoz = 1;
					}
				}
				if (num2 > 0)
				{
					KasreKarMidShift += num2;
					sumKk += num2;
				}
				if (num3 > 0)
				{
					KasreKarEndShift += num3;
					sumKk += num3;
				}
			}
			if (!ShiftDetViews[ShiftDetViews.Count - 1].ClsShiftCoding.CalcEnteghalInSrcDate)
			{
				return sumKk;
			}
			List<ShiftDetView> list = await GetShiftDetViewsNextDayAsync(token);
			ShiftCodeing shiftCodeing = new ShiftCodeing();
			if (list != null && list.Count > 0)
			{
				shiftCodeing = list[0].ClsShiftCoding;
			}
			if (list == null || list.Count <= 0 || !shiftCodeing.CalcEnteghalInSrcDate || shiftCodeing.Vorod.TimeOfDay > new TimeSpan(0, 0, 1))
			{
				return sumKk;
			}
			var (num4, num5, num6) = await CalcKasrOfShiftAsync(list[0], (from p in CoupleTaradodNextDay
				where !(p.KTime == "00:00") || !(p.VTime == "00:00")
				orderby p.VTime
				select p).ToArray(), token);
			if (num4 > 0)
			{
				KasreKarBeginShift += num4;
				sumKk += num4;
			}
			if (num5 > 0)
			{
				KasreKarMidShift += num5;
				sumKk += num5;
			}
			if (num6 > 0)
			{
				KasreKarEndShift += num6;
				sumKk += num6;
			}
			return sumKk;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "day : " + _dayofShMonth, "CalKasrKarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1125);
		}
		return 0;
	}

	public static async Task FixShiftCodingIfHasVacationAsync(string contextGuid, string caller, DateTime date, int personId, bool CalcEkWhenHasVacationOrMission, int morakhadiDailyId, List<ShiftDetView> shiftDetViews, CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested || !CalcEkWhenHasVacationOrMission)
			{
				return;
			}
			foreach (ShiftDetView shift in shiftDetViews)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				ShiftCodeing shiftCodeing = ShiftCodeing.Clone(shift.ClsShiftCoding);
				if (shiftCodeing.NextDay && shiftCodeing.Vorod.TimeOfDay == new TimeSpan(0, 0, 0))
				{
					Morakhasidaily obj = await Morakhasidaily.GetAsync(contextGuid, date.AddDays(-1.0).Date, personId, token);
					if (obj != null && obj.Id > 0)
					{
						shiftCodeing.H1 = 0;
						shiftCodeing.M1 = 0;
						shiftCodeing.S1 = 0;
						shiftCodeing.H2 = 0;
						shiftCodeing.M2 = 0;
						shiftCodeing.S2 = 0;
					}
				}
				else if (morakhadiDailyId > 0)
				{
					shiftCodeing.H1 = 0;
					shiftCodeing.M1 = 0;
					shiftCodeing.S1 = 0;
					shiftCodeing.H2 = 0;
					shiftCodeing.M2 = 0;
					shiftCodeing.S2 = 0;
				}
				shift.ClsShiftCoding = shiftCodeing;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task FixShiftCodings(string contextGuid, bool wasOnVacationYeserday, bool hasVacationToday, bool CalcEkWhenHasVacationOrMission, List<ShiftDetView> todayShiftDetViews, List<ShiftDetView> yesterdayShiftDetViews, CancellationToken token)
	{
		try
		{
			List<ShiftDetView> list = todayShiftDetViews;
			if (list != null && list.Count > 0)
			{
				ShiftCodeing shiftCodeing = todayShiftDetViews.LastOrDefault()?.ClsShiftCoding;
				if (shiftCodeing != null && shiftCodeing.NextDay && (shiftCodeing.KhorojEnd < shiftCodeing.Vorod || (shiftCodeing.KhorojEnd > shiftCodeing.Vorod && shiftCodeing.PeakOvertime > 0 && shiftCodeing.PeakOvertimeTimeFormat < shiftCodeing.Vorod)))
				{
					shiftCodeing.H1 = 23;
					shiftCodeing.M1 = 59;
					shiftCodeing.S1 = 59;
					shiftCodeing.TajilKh = 0;
					shiftCodeing.TakhirKh = 0;
					if (shiftCodeing.LunchTime == 0 || (double)shiftCodeing.StartLunchTime < shiftCodeing.Vorod.TimeOfDay.TotalMinutes)
					{
						shiftCodeing.StartLunchTime = 0;
						shiftCodeing.LunchTime = 0;
					}
					if (shiftCodeing.PeakOvertime > 0 && shiftCodeing.PeakOvertimeTimeFormat < shiftCodeing.Vorod)
					{
						shiftCodeing.PeakOvertime = 0;
					}
				}
			}
			if (yesterdayShiftDetViews == null || yesterdayShiftDetViews.Count <= 0)
			{
				return;
			}
			ShiftDetView shiftDetView = yesterdayShiftDetViews.LastOrDefault();
			ShiftCodeing clsShiftCoding = shiftDetView.ClsShiftCoding;
			if (!clsShiftCoding.NextDay || wasOnVacationYeserday)
			{
				return;
			}
			ShiftCodeing shiftCodeing2 = ShiftCodeing.Clone(shiftDetView.ClsShiftCoding);
			shiftCodeing2.H1 = 0;
			shiftCodeing2.M1 = 0;
			shiftCodeing2.S1 = 0;
			shiftCodeing2.TajilV = 0;
			shiftCodeing2.TakhirV = 0;
			shiftCodeing2.JarimeV = 0;
			shiftCodeing2.CurrDate = clsShiftCoding.CurrDate.AddDays(1.0);
			if (clsShiftCoding.LunchTime > 0 && (double)clsShiftCoding.StartLunchTime > clsShiftCoding.Vorod.TimeOfDay.TotalMinutes && clsShiftCoding.StartLunchTime + clsShiftCoding.LunchTime <= 1440)
			{
				shiftCodeing2.LunchTime = 0;
				shiftCodeing2.StartLunchTime = 0;
			}
			if (clsShiftCoding.KhorojEnd < clsShiftCoding.Vorod || (clsShiftCoding.KhorojEnd > clsShiftCoding.Vorod && clsShiftCoding.PeakOvertime > 0 && clsShiftCoding.PeakOvertimeTimeFormat < clsShiftCoding.Vorod))
			{
				ShiftDetView item = await ShiftDetView.Clone_ShiftDetViewAsync(contextGuid, shiftDetView, todayShiftDetViews.First().ClsShiftCoding.CurrDate, shiftCodeing2, token);
				if (todayShiftDetViews == null)
				{
					todayShiftDetViews = new List<ShiftDetView>();
				}
				todayShiftDetViews.Add(item);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task<(int kkFirst, int kkMid, int kkEnd)> CalcKasrOfShiftAsync(ShiftDetView shift, CoupleTaradod[] coupleTaradodList, CancellationToken token)
	{
		int item = 0;
		int num = 0;
		int item2 = 0;
		try
		{
			if (Contract == null || shift == null)
			{
				return (kkFirst: item, kkMid: num, kkEnd: item2);
			}
			List<CoupleTaradod> list = CouplesOfShift(shift, coupleTaradodList);
			ShiftCodeing clsShiftCoding = shift.ClsShiftCoding;
			if (list != null && list.Count <= 0)
			{
				item = CoupleTaradod.TimeDiff(clsShiftCoding.Vorod, clsShiftCoding.Khoroj);
				return (kkFirst: item, kkMid: 0, kkEnd: 0);
			}
			if (list != null && list.Count > 0)
			{
				if (list[0].Vtaradod.DateTimeLogM > clsShiftCoding.VorodEnd)
				{
					item = CoupleTaradod.TimeDiff(clsShiftCoding.Vorod, list[0].Vtaradod.DateTimeLogM);
				}
				if (list[list.Count - 1].Ktaradod.DateTimeLogM < clsShiftCoding.KhorojStart && (int)list[list.Count - 1].Ktaradod.TypeTaradodVacationOrMission != 5 && (int)list[list.Count - 1].Ktaradod.TypeTaradodVacationOrMission != 7)
				{
					item2 = CoupleTaradod.TimeDiff(list[list.Count - 1].Ktaradod.DateTimeLogM, clsShiftCoding.Khoroj);
				}
			}
			if (list != null && list.Count > 1)
			{
				for (int i = 1; i < list.Count; i++)
				{
					num += CoupleTaradod.TimeDiff(list[i - 1].Ktaradod.DateTimeLogM, list[i].Vtaradod.DateTimeLogM);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalcKasrOfShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1293);
		}
		return (kkFirst: item, kkMid: num, kkEnd: item2);
	}

	private async Task<int> CalEzafeKarAsync(CancellationToken token)
	{
		_ = 4;
		try
		{
			int sumEk = 0;
			TaradodFullDet taradodFullDet = this;
			TaradodFullDet taradodFullDet2 = this;
			int num2 = (EzafeKarMidShift = 0);
			int ezafeKarEndShift = (taradodFullDet2.EzafeKarBeginShift = num2);
			taradodFullDet.EzafeKarEndShift = ezafeKarEndShift;
			if (Setting.NotCalkEK_KK)
			{
				return 0;
			}
			_ = CoupleTaradodCurrentDay[0];
			for (int j = 0; j < 8; j++)
			{
				CoupleTaradod couple2 = CoupleTaradodCurrentDay[j];
				if ((!(couple2.VTime == "00:00") || !(couple2.KTime == "00:00")) && await CanCalcEzafekarForThisCoupleAsync(couple2, coupleIsInToday: true, token))
				{
					var (num4, num5, num6) = await CalcEzafeOfCoupleAsync(couple2, ShiftDetViews, Setting.CalcEzafeOutOfShift, j == 0, token);
					if (num4 > 0)
					{
						EzafeKarBeginShift += num4;
						sumEk += num4;
					}
					if (num5 > 0)
					{
						EzafeKarMidShift += num5;
						sumEk += num5;
					}
					if (num6 > 0)
					{
						EzafeKarEndShift += num6;
						sumEk += num6;
					}
				}
			}
			if (ShiftDetViews != null && ShiftDetViews.Count != 0)
			{
				List<ShiftDetView> shiftDetViews = ShiftDetViews;
				if (shiftDetViews == null || shiftDetViews.Where((ShiftDetView p) => (int)p.ClsShiftCoding.TypeShift == 11).Count() <= 0)
				{
					goto IL_02d5;
				}
			}
			if (!Setting.CalcEzafeOutOfShift)
			{
				return sumEk;
			}
			if (!Setting.AutomaticCalcMovetoNextDay)
			{
				return sumEk;
			}
			goto IL_02d5;
			IL_02d5:
			for (int j = 0; j < 8; j++)
			{
				CoupleTaradod couple2 = CoupleTaradodNextDay[j];
				if (!(couple2.VTime == "00:00") || !(couple2.KTime == "00:00"))
				{
					if (!(await CanCalcEzafekarForThisCoupleAsync(couple2, coupleIsInToday: false, token)))
					{
						break;
					}
					CoupleTaradod couple3 = couple2;
					var (num7, num8, num9) = await CalcEzafeOfCoupleAsync(couple3, await GetShiftDetViewsNextDayAsync(token), Setting.CalcEzafeOutOfShift, j == 0, token);
					if (num7 > 0)
					{
						EzafeKarBeginShift += num7;
						sumEk += num7;
					}
					if (num8 > 0)
					{
						EzafeKarMidShift += num8;
						sumEk += num8;
					}
					if (num9 > 0)
					{
						EzafeKarEndShift += num9;
						sumEk += num9;
					}
				}
			}
			return sumEk;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalEzafeKarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1372);
		}
		return 0;
	}

	private async Task<int> GetEzafeKarEndAsync(CoupleTaradod couple, ShiftDetView shift, CancellationToken token)
	{
		if (shift == null || couple == null)
		{
			return 0;
		}
		int num = 0;
		try
		{
			ShiftCodeing clsShiftCoding = shift.ClsShiftCoding;
			if (couple.Ktaradod?.DateTimeLogM > clsShiftCoding?.KhorojEnd)
			{
				if (couple.Vtaradod?.DateTimeLogM > clsShiftCoding?.KhorojEnd)
				{
					if (clsShiftCoding.PeakOvertimeTimeFormat > couple.Ktaradod.DateTimeLogM || clsShiftCoding.PeakOvertime == 0)
					{
						num = CoupleTaradod.TimeDiff(couple.Vtaradod.DateTimeLogM, couple.Ktaradod.DateTimeLogM);
						if (couple.Ktaradod.DateTimeLogM.TimeOfDay >= new TimeSpan(23, 59, 59) && clsShiftCoding.CalcEnteghalInSrcDate && !Setting.CalcEkInSourceDate)
						{
							CoupleTaradod coupleTaradod = CoupleTaradodNextDay[0];
							if (coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay <= new TimeSpan(0, 0, 1))
							{
								num += CoupleTaradod.TimeDiff(coupleTaradod.Vtaradod.DateTimeLogM, coupleTaradod.Ktaradod.DateTimeLogM);
							}
						}
					}
					else
					{
						num = CoupleTaradod.TimeDiff(couple.Vtaradod.DateTimeLogM, clsShiftCoding.PeakOvertimeTimeFormat);
					}
				}
				else
				{
					num = CoupleTaradod.TimeDiff(clsShiftCoding.Khoroj, couple.Ktaradod.DateTimeLogM);
					if (clsShiftCoding.PeakOvertime > 0)
					{
						int num2 = clsShiftCoding.PeakOvertime - (int)clsShiftCoding.Khoroj.TimeOfDay.TotalMinutes;
						if (num2 > 0 && num > num2)
						{
							num = num2;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetEzafeKarEndAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1420);
		}
		return num;
	}

	private int EzafeKarBegin(CoupleTaradod couple, ShiftDetView shift)
	{
		try
		{
			if (shift == null || couple == null)
			{
				return 0;
			}
			int result = 0;
			ShiftCodeing clsShiftCoding = shift.ClsShiftCoding;
			if (couple.Vtaradod?.DateTimeLogM < clsShiftCoding?.VorodStart)
			{
				result = CoupleTaradod.TimeDiff(couple.Vtaradod.DateTimeLogM, clsShiftCoding.Vorod);
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EzafeKarBegin", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1437);
			throw;
		}
	}

	private int EzafeKarMid(ShiftDetView firstShift, ShiftDetView secoundShift)
	{
		try
		{
			if (firstShift == null || secoundShift == null)
			{
				return 0;
			}
			int result = 0;
			ShiftCodeing clsShiftCoding = firstShift.ClsShiftCoding;
			ShiftCodeing clsShiftCoding2 = secoundShift.ClsShiftCoding;
			if (clsShiftCoding.KhorojEnd < clsShiftCoding2.VorodStart)
			{
				result = CoupleTaradod.TimeDiff(clsShiftCoding.Khoroj, clsShiftCoding2.Vorod);
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EzafeKarMid", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1458);
			throw;
		}
	}

	private int EzafeKarOutOfShift(CoupleTaradod couple)
	{
		try
		{
			if (couple?.Vtaradod == null || couple.Ktaradod == null)
			{
				return 0;
			}
			if (!Setting.CalcEzafeOutOfShift)
			{
				return 0;
			}
			return CoupleTaradod.TimeDiff(couple.Vtaradod.DateTimeLogM, couple.Ktaradod.DateTimeLogM);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EzafeKarOutOfShift", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1475);
			throw;
		}
	}

	private async Task<(int ekFirst, int ekMid, int ekEnd)> CalcEzafeOfCoupleAsync(CoupleTaradod couple, List<ShiftDetView> shiftDetViewLst, bool calcEzafeKarOutOfShift, bool isFirstCouple, CancellationToken token)
	{
		int first = 0;
		int mid = 0;
		int end = 0;
		try
		{
			List<ShiftDetView> list = null;
			if (Contract != null && shiftDetViewLst != null)
			{
				list = ShiftsOfCouple(couple, shiftDetViewLst);
			}
			if ((list == null || list.Count <= 0) && calcEzafeKarOutOfShift)
			{
				int num = EzafeKarOutOfShift(couple);
				if (shiftDetViewLst != null && shiftDetViewLst.Count > 0 && !isFirstCouple)
				{
					ShiftDetView shiftDetView = shiftDetViewLst[shiftDetViewLst.Count - 1];
					ShiftCodeing clsShiftCoding = shiftDetView.ClsShiftCoding;
					if (couple.Ktaradod.DateTimeLogM < clsShiftCoding.Khoroj)
					{
						int count = shiftDetViewLst.Count;
						while (count-- > 0)
						{
							shiftDetView = shiftDetViewLst[count];
							if (couple.Ktaradod.DateTimeLogM >= clsShiftCoding.Khoroj)
							{
								break;
							}
						}
					}
					if (couple.Ktaradod.DateTimeLogM < clsShiftCoding.Khoroj)
					{
						end += num;
					}
					else
					{
						int num2 = end;
						end = num2 + await GetEzafeKarEndAsync(couple, shiftDetView, token);
					}
					return (ekFirst: 0, ekMid: 0, ekEnd: end);
				}
				end += num;
				return (ekFirst: 0, ekMid: 0, ekEnd: end);
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					ShiftCodeing clsShiftCoding2 = list[i].ClsShiftCoding;
					if (list.Count <= 1 || !(clsShiftCoding2.Vorod == clsShiftCoding2.Khoroj))
					{
						if (i == 0)
						{
							first += EzafeKarBegin(couple, list[i]);
						}
						if (i == list.Count - 1)
						{
							int num2 = end;
							end = num2 + await GetEzafeKarEndAsync(couple, list[i], token);
							return (ekFirst: first, ekMid: mid, ekEnd: end);
						}
						mid += EzafeKarMid(list[i], list[i + 1]);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalcEzafeOfCoupleAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1541);
		}
		return (ekFirst: first, ekMid: mid, ekEnd: end);
	}

	private List<ShiftDetView> ShiftsOfCouple(CoupleTaradod couple, List<ShiftDetView> shiftDetViewLst)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		try
		{
			List<ShiftDetView> list = new List<ShiftDetView>();
			if (shiftDetViewLst != null && shiftDetViewLst.Count > 0)
			{
				list.AddRange(shiftDetViewLst.Where((ShiftDetView shift) => couple?.Vtaradod?.DateTimeLogM.TimeOfDay < shift.ClsShiftCoding?.Khoroj.TimeOfDay).TakeWhile((ShiftDetView shift) => shift.ClsShiftCoding?.Vorod.TimeOfDay < couple?.Ktaradod?.DateTimeLogM.TimeOfDay));
				if (list.Count == 0 && (int)shiftDetViewLst[0].ClsShiftCoding.TypeShift == 2)
				{
					list.Add(shiftDetViewLst[0]);
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ShiftsOfCouple", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1567);
			throw;
		}
	}

	private List<CoupleTaradod> CouplesOfShift(ShiftDetView shift, CoupleTaradod[] coupleTaradodList)
	{
		try
		{
			List<CoupleTaradod> list = new List<CoupleTaradod>();
			if (shift != null && coupleTaradodList != null && coupleTaradodList.Length > 0)
			{
				ShiftCodeing clsShiftCoding = shift.ClsShiftCoding;
				foreach (CoupleTaradod coupleTaradod in coupleTaradodList)
				{
					if (coupleTaradod.Vtaradod == null || coupleTaradod.Ktaradod == null || clsShiftCoding == null || (coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay > new TimeSpan(0, 0, 0) && coupleTaradod.Ktaradod.DateTimeLogM.TimeOfDay == new TimeSpan(0, 0, 0)))
					{
						break;
					}
					if (coupleTaradod.Vtaradod.DateTimeLogM == coupleTaradod.Ktaradod.DateTimeLogM)
					{
						if (coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay == new TimeSpan(0, 0, 0))
						{
							break;
						}
					}
					else if (coupleTaradod.Vtaradod.DateTimeLogM.AddSeconds(1.0) >= clsShiftCoding.VorodStart && coupleTaradod.Vtaradod.DateTimeLogM <= clsShiftCoding.KhorojEnd)
					{
						list.Add(coupleTaradod);
					}
					else if (coupleTaradod.Ktaradod.DateTimeLogM >= clsShiftCoding.VorodStart && coupleTaradod.Ktaradod.DateTimeLogM <= clsShiftCoding.KhorojEnd)
					{
						list.Add(coupleTaradod);
					}
					else if (coupleTaradod.Vtaradod.DateTimeLogM <= clsShiftCoding.VorodStart && coupleTaradod.Ktaradod.DateTimeLogM >= clsShiftCoding.KhorojEnd)
					{
						list.Add(coupleTaradod);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CouplesOfShift", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1607);
			throw;
		}
	}

	private async Task<bool> CanCalcEzafekarForThisCoupleAsync(CoupleTaradod couple, bool coupleIsInToday, CancellationToken token)
	{
		_ = 3;
		try
		{
			if (Contract == null)
			{
				return false;
			}
			if (couple?.Vtaradod == null || couple.Ktaradod == null)
			{
				return false;
			}
			if (couple.VTime == "00:00" && couple.KTime == "00:00")
			{
				return false;
			}
			if (couple.Vtaradod.Id <= 0 || couple.Ktaradod.Id <= 0)
			{
				return false;
			}
			if (couple.Vtaradod.DateTimeLogM >= couple.Ktaradod.DateTimeLogM)
			{
				return false;
			}
			TypeTaradodEnumType typeTaradod = couple.TypeTaradod;
			if (((int)typeTaradod == 8 || (int)typeTaradod == 28) ? true : false)
			{
				return false;
			}
			if (Setting.AutomaticCalcMovetoNextDay && Setting.CalcEkInSourceDate && (int)couple.Vtaradod.IsVirtual == 6)
			{
				return !coupleIsInToday;
			}
			List<ShiftDetView> shiftLst = ((!coupleIsInToday) ? (await GetShiftDetViewsNextDayAsync(token)) : ShiftDetViews);
			ShiftDetView shiftDetView = await GetShiftAsync(couple, shiftLst, token);
			ShiftCodeing shiftCodeing = null;
			if (shiftDetView != null)
			{
				shiftCodeing = shiftDetView.ClsShiftCoding;
			}
			if (shiftCodeing == null)
			{
				if ((int)couple.Vtaradod.IsVirtual == 6)
				{
					List<ShiftDetView> shiftYesterdayList = await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, DateM.AddDays(-1.0), Contract.ID, Contract.PersonId, token);
					await FixShiftCodingIfHasVacationAsync(ContextGuid, "CanCalcEzafekarForThisCoupleAsync", DateM, Contract.PersonId, Setting.CalcEkWhenHasVacationOrMission, Morakhasi?.Id ?? 0, shiftYesterdayList, token);
					if (shiftYesterdayList != null && shiftYesterdayList.Count > 0)
					{
						ShiftCodeing clsShiftCoding = shiftYesterdayList[shiftYesterdayList.Count - 1].ClsShiftCoding;
						if (clsShiftCoding == null)
						{
							return false;
						}
						if (clsShiftCoding.NotCalcEKForShift)
						{
							return false;
						}
						if (clsShiftCoding.CalcEnteghalInSrcDate && (int)shiftYesterdayList[shiftYesterdayList.Count - 1].ClsShiftCoding.TypeShift == 2)
						{
							return false;
						}
					}
				}
				return coupleIsInToday && Setting.CalcEzafeOutOfShift;
			}
			if (shiftCodeing.NotCalcEKForShift)
			{
				return false;
			}
			if (coupleIsInToday)
			{
				if (shiftCodeing.CalcEnteghalInSrcDate && shiftCodeing.Vorod.TimeOfDay <= new TimeSpan(0, 0, 0, 1))
				{
					return false;
				}
				return true;
			}
			if (shiftCodeing != null && shiftCodeing.CalcEnteghalInSrcDate && shiftCodeing.Vorod.TimeOfDay <= new TimeSpan(0, 0, 0, 1))
			{
				if ((int)shiftCodeing.TypeShift == 2 && (int)couple.Vtaradod.IsVirtual != 6)
				{
					return false;
				}
				return true;
			}
			return Setting.AutomaticCalcMovetoNextDay && Setting.CalcEkInSourceDate && (int)couple.Vtaradod.IsVirtual == 6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CanCalcEzafekarForThisCoupleAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1689);
			return false;
		}
	}

	private async Task CalcKarkerdAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_hozor = new TimeSpan(0L);
			if (Contract == null || Contract.DateFinishM < DateM)
			{
				foreach (CoupleTaradod item in CoupleTaradodReadOnly)
				{
					if (item.Ktaradod != null && item.Vtaradod.DateTimeLogM < item.Ktaradod.DateTimeLogM)
					{
						_hozor = _hozor.Value.Add(TimeSpan.FromMinutes(CoupleTaradod.TimeDiff(item.Vtaradod.DateTimeLogM, item.Ktaradod.DateTimeLogM)));
					}
				}
				return;
			}
			await ClacAllOfTodayKarkardTypes(token);
			int lunchMinutesOfShifts = GetLunchMinutesOfShifts(ShiftDetViews);
			if (lunchMinutesOfShifts > 0)
			{
				_hozor = _hozor.Value.Add(-TimeSpan.FromMinutes(lunchMinutesOfShifts));
			}
			await ClacAllOfKarkardTypesEnteghali(lunchMinutesOfShifts > 0, token);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CalcKarkerdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1723);
			throw;
		}
	}

	private async Task ClacAllOfTodayKarkardTypes(CancellationToken token)
	{
		_ = 5;
		try
		{
			foreach (CoupleTaradod coupleToday in CoupleTaradodReadOnly)
			{
				if (coupleToday.Ktaradod == null)
				{
					break;
				}
				if (coupleToday.Vtaradod.DateTimeLogM >= coupleToday.Ktaradod.DateTimeLogM || (Setting.AutomaticCalcMovetoNextDay && Setting.CalcEkInSourceDate && (int)coupleToday.Vtaradod.IsVirtual == 6))
				{
					continue;
				}
				ShiftDetView shiftDetView = await Shifts.GetShiftAsync(ShiftDetViews, coupleToday.Vtaradod.DateTimeLogM, token);
				if (shiftDetView == null)
				{
					if ((int)coupleToday.Vtaradod.IsVirtual == 6)
					{
						List<ShiftDetView> shiftYesterday = await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, DateM.AddDays(-1.0), Contract.ID, Contract.PersonId, token);
						await FixShiftCodingIfHasVacationAsync(ContextGuid, "ClacAllOfTodayKarkardTypes", DateM, Contract.PersonId, Setting.CalcEkWhenHasVacationOrMission, Morakhasi?.Id ?? 0, shiftYesterday, token);
						if (shiftYesterday != null && shiftYesterday.Count > 0 && shiftYesterday[shiftYesterday.Count - 1].ClsShiftCoding.CalcEnteghalInSrcDate)
						{
							continue;
						}
					}
					await CalcAllOfkarkardTypesAsync(coupleToday, ShiftDetViews, token);
				}
				else if (!(await shiftDetView.IsCalcInSourceDateAsync()) || !coupleToday.IsEnteghaliTaradod)
				{
					await CalcAllOfkarkardTypesAsync(coupleToday, ShiftDetViews, token);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task ClacAllOfKarkardTypesEnteghali(bool isLunchCalcedBefore, CancellationToken token)
	{
		_ = 8;
		try
		{
			List<ShiftDetView> shiftDetViewsNextDay = await GetShiftDetViewsNextDayAsync(token);
			for (int i = 0; i < 8; i++)
			{
				CoupleTaradod coupleNextDay = CoupleTaradodNextDay[i];
				if (coupleNextDay == null || coupleNextDay.Vtaradod == null || coupleNextDay.Ktaradod == null)
				{
					break;
				}
				LogOnPersonel vtaradod = coupleNextDay.Vtaradod;
				if (vtaradod != null && vtaradod.Id == 0)
				{
					break;
				}
				LogOnPersonel ktaradod = coupleNextDay.Ktaradod;
				if (ktaradod != null && ktaradod.Id == 0)
				{
					break;
				}
				ShiftDetView shiftNextDay2 = await Shifts.GetShiftAsync(shiftDetViewsNextDay, coupleNextDay.Vtaradod.DateTimeLogM, token);
				ShiftDetView shiftDetView = shiftNextDay2;
				if (shiftDetView != null && shiftDetView.ClsShiftCoding?.TypeShift == (TypeShiftEnumType?)2 && (int)coupleNextDay.Vtaradod.IsVirtual != 6 && (int)coupleNextDay.Vtaradod.IsVirtual != 3)
				{
					break;
				}
				if ((int)coupleNextDay.Vtaradod.IsVirtual != 6)
				{
					continue;
				}
				TimeSpan.FromMinutes(CoupleTaradod.TimeDiff(coupleNextDay.Vtaradod.DateTimeLogM, coupleNextDay.Ktaradod.DateTimeLogM));
				if (Setting.CalcEkInSourceDate)
				{
					await CalcAllOfkarkardTypesAsync(coupleNextDay, shiftDetViewsNextDay, token);
					continue;
				}
				if ((shiftDetViewsNextDay?.Any() ?? false) && await shiftDetViewsNextDay[0].IsCalcInSourceDateAsync())
				{
					await CalcAllOfkarkardTypesAsync(coupleNextDay, shiftDetViewsNextDay, token);
					continue;
				}
				bool flag = shiftNextDay2 != null;
				if (flag)
				{
					flag = await shiftNextDay2.IsCalcInSourceDateAsync();
				}
				if (flag)
				{
					await CalcAllOfkarkardTypesAsync(coupleNextDay, shiftDetViewsNextDay, token);
					continue;
				}
				flag = shiftNextDay2 == null;
				if (!flag)
				{
					flag = !(await shiftNextDay2.IsCalcInSourceDateAsync());
				}
				if (flag)
				{
					break;
				}
			}
			IEnumerable<ShiftDetView> enumerable = shiftDetViewsNextDay?.Where((ShiftDetView q) => q.ClsShiftCoding.LunchTime > 0 && q.ClsShiftCoding.VorodStart.TimeOfDay != q.ClsShiftCoding.KhorojEnd.TimeOfDay);
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (ShiftDetView shiftNextDay2 in enumerable)
			{
				if (!(await shiftNextDay2.IsCalcInSourceDateAsync()) || isLunchCalcedBefore)
				{
					continue;
				}
				ShiftCodeing clsShiftCoding = shiftNextDay2.ClsShiftCoding;
				if (clsShiftCoding == null || (int)clsShiftCoding.TypeShift != 2)
				{
					int lunchMinutes = shiftNextDay2.GetLunchMinutes(CoupleTaradodNextDay);
					if (lunchMinutes > 0)
					{
						_hozor = _hozor.Value.Add(-TimeSpan.FromMinutes(lunchMinutes));
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private int GetLunchMinutesOfShifts(List<ShiftDetView> shiftDetViews)
	{
		int num = 0;
		try
		{
			IEnumerable<ShiftDetView> enumerable = ShiftDetViews?.Where((ShiftDetView q) => q.ClsShiftCoding.LunchTime > 0 && q.ClsShiftCoding.VorodStart.TimeOfDay != q.ClsShiftCoding.KhorojEnd.TimeOfDay);
			if (enumerable != null)
			{
				foreach (ShiftDetView item in enumerable)
				{
					num += item.GetLunchMinutes(CoupleTaradodReadOnly);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return num;
	}

	private async Task CalcAllOfkarkardTypesAsync(CoupleTaradod couple, List<ShiftDetView> shiftDetViews, CancellationToken token)
	{
		try
		{
			int num = await couple.KarkardAsync(shiftDetViews, token);
			if (num <= 0)
			{
				return;
			}
			_hozor += TimeSpan.FromMinutes(num);
			TypeTaradodEnumType typeTaradod = couple.TypeTaradod;
			if ((int)typeTaradod <= 10)
			{
				if ((int)typeTaradod == 8)
				{
					_onFriDayWorkKarkard += num;
					return;
				}
				if ((int)typeTaradod == 10)
				{
					_nightWorkKarkard += CalcMoazafiTimeOfCouple(couple);
					return;
				}
			}
			else
			{
				if ((int)typeTaradod == 28)
				{
					_onTatilWorkKarkard += num;
					return;
				}
				switch (typeTaradod - 91)
				{
				case 0:
					_shiftWorkKarkard1 += CalcMoazafiTimeOfCouple(couple);
					return;
				case 1:
					_shiftWorkKarkard2 += CalcMoazafiTimeOfCouple(couple);
					return;
				case 2:
					_shiftWorkKarkard3 += CalcMoazafiTimeOfCouple(couple);
					return;
				}
			}
			List<ShiftDetView> list = ShiftsOfCouple(couple, shiftDetViews);
			if (list == null || !list.Any())
			{
				return;
			}
			ShiftCodeing clsShiftCoding = list[0].ClsShiftCoding;
			if (clsShiftCoding != null)
			{
				TypeTaradodInShiftEnum typeTaradodInShift = clsShiftCoding.TypeTaradodInShift;
				switch (typeTaradodInShift - 1)
				{
				case 0:
					_shiftWorkKarkard1 += CalcMoazafiTimeOfCouple(couple);
					break;
				case 1:
					_shiftWorkKarkard2 += CalcMoazafiTimeOfCouple(couple);
					break;
				case 2:
					_shiftWorkKarkard3 += CalcMoazafiTimeOfCouple(couple);
					break;
				case 3:
					_nightWorkKarkard += CalcMoazafiTimeOfCouple(couple);
					break;
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CalcAllOfkarkardTypesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1927);
			throw;
		}
	}

	private int CalcLunchTime(ShiftDetView qu)
	{
		try
		{
			return (Contract != null) ? qu.GetLunchMinutes(CoupleTaradodReadOnly) : 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalcLunchTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1939);
			throw;
		}
	}

	public int WorkOnTypeTaradod(TypeTaradodEnumType ptype)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected I4, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected I4, but got Unknown
		try
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				num += CoupleTaradodCurrentDay[i].TimeWorkOnTypeTaradod(ptype);
			}
			ATT_TaradodDet attTaradodDet = AttTaradodDet;
			switch (ptype - 5)
			{
			default:
				if ((int)ptype != 28)
				{
					switch (ptype - 91)
					{
					case 0:
						num += attTaradodDet?.ShiftTime1 ?? 0;
						break;
					case 1:
						num += attTaradodDet?.ShiftTime2 ?? 0;
						break;
					case 2:
						num += attTaradodDet?.ShiftTime3 ?? 0;
						break;
					}
				}
				else
				{
					num += attTaradodDet?.TatilTime ?? 0;
				}
				break;
			case 0:
				num += attTaradodDet?.VacationTime ?? 0;
				break;
			case 2:
				num += attTaradodDet?.MissionTime ?? 0;
				break;
			case 3:
				num += attTaradodDet?.FridayTime ?? 0;
				break;
			case 5:
				num += attTaradodDet?.NightTime ?? 0;
				break;
			case 1:
			case 4:
				break;
			}
			return num;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "WorkOnTypeTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 1983);
		}
		return 0;
	}

	private int CalcShiftDetViewsTime()
	{
		try
		{
			int num = 0;
			if (Contract == null)
			{
				return num;
			}
			List<ShiftDetView> shiftDetViews = ShiftDetViews;
			if (shiftDetViews == null || shiftDetViews.Count <= 0)
			{
				return num;
			}
			foreach (ShiftDetView shiftDetView in ShiftDetViews)
			{
				ShiftCodeing clsShiftCoding = shiftDetView.ClsShiftCoding;
				if (clsShiftCoding.H1 > clsShiftCoding.H2)
				{
					num += (clsShiftCoding.H2 - clsShiftCoding.H1 + 24) * 60 + (clsShiftCoding.M2 - clsShiftCoding.M1);
					continue;
				}
				num += (clsShiftCoding.H2 - clsShiftCoding.H1) * 60 + (clsShiftCoding.M2 - clsShiftCoding.M1);
				if (clsShiftCoding.S2 - clsShiftCoding.S1 > 30)
				{
					num++;
				}
			}
			return num;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalcShiftDetViewsTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2017);
			throw;
		}
	}

	private int CalcMoazafiTime()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Invalid comparison between Unknown and I4
		if (Contract == null)
		{
			return 0;
		}
		int num = 0;
		try
		{
			int num2 = -1;
			int num3 = 0;
			foreach (CoupleTaradod item in CoupleTaradodReadOnly)
			{
				foreach (ShiftDetView item2 in ShiftsOfCouple(item, ShiftDetViews))
				{
					ShiftCodeing clsShiftCoding = item2.ClsShiftCoding;
					num3 += item.GetLunchMinutes(clsShiftCoding.StartLunchTime, (short)(clsShiftCoding.StartLunchTime + clsShiftCoding.LunchTime));
					if (clsShiftCoding.NextDay && clsShiftCoding.CalcEnteghalInSrcDate && item.IsEnteghaliTaradod)
					{
						continue;
					}
					double num4;
					double num5;
					if ((int)clsShiftCoding.TypeShift == 2)
					{
						if (num2 == -1)
						{
							num2 = (int)Math.Round(clsShiftCoding.Khoroj.TimeOfDay.TotalMinutes);
						}
						num4 = Math.Round(item.Vtaradod.DateTimeLogM.TimeOfDay.TotalMinutes);
						num5 = Math.Round(item.Ktaradod.DateTimeLogM.TimeOfDay.TotalMinutes);
					}
					else
					{
						num4 = ((item.Vtaradod.DateTimeLogM.TimeOfDay <= clsShiftCoding.Vorod.TimeOfDay) ? Math.Round(clsShiftCoding.Vorod.TimeOfDay.TotalMinutes) : Math.Round(item.Vtaradod.DateTimeLogM.TimeOfDay.TotalMinutes));
						num5 = ((item.Ktaradod.DateTimeLogM.TimeOfDay >= clsShiftCoding.Khoroj.TimeOfDay) ? Math.Round(clsShiftCoding.Khoroj.TimeOfDay.TotalMinutes) : Math.Round(item.Ktaradod.DateTimeLogM.TimeOfDay.TotalMinutes));
					}
					if (num5 > num4)
					{
						num += (int)(num5 - num4);
					}
				}
			}
			CoupleTaradod[] coupleTaradodNextDay = CoupleTaradodNextDay;
			foreach (CoupleTaradod coupleTaradod in coupleTaradodNextDay)
			{
				if (coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay == coupleTaradod.Ktaradod.DateTimeLogM.TimeOfDay)
				{
					continue;
				}
				foreach (ShiftDetView item3 in ShiftsOfCouple(coupleTaradod, ShiftDetViews))
				{
					ShiftCodeing clsShiftCoding2 = item3.ClsShiftCoding;
					if (!clsShiftCoding2.NextDay || !clsShiftCoding2.CalcEnteghalInSrcDate || !(coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay.TotalMinutes < 1.0))
					{
						continue;
					}
					double num6;
					double num7;
					if ((int)clsShiftCoding2.TypeShift == 2)
					{
						if (num2 == -1)
						{
							num2 = (int)Math.Round(clsShiftCoding2.Khoroj.TimeOfDay.TotalMinutes);
						}
						num6 = Math.Round(coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay.TotalMinutes);
						num7 = Math.Round(coupleTaradod.Ktaradod.DateTimeLogM.TimeOfDay.TotalMinutes);
					}
					else
					{
						num6 = ((coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay <= clsShiftCoding2.Vorod.TimeOfDay) ? Math.Round(clsShiftCoding2.Vorod.TimeOfDay.TotalMinutes) : Math.Round(coupleTaradod.Vtaradod.DateTimeLogM.TimeOfDay.TotalMinutes));
						num7 = ((coupleTaradod.Ktaradod.DateTimeLogM.TimeOfDay >= clsShiftCoding2.Khoroj.TimeOfDay) ? Math.Round(clsShiftCoding2.Khoroj.TimeOfDay.TotalMinutes) : Math.Round(coupleTaradod.Ktaradod.DateTimeLogM.TimeOfDay.TotalMinutes));
					}
					if (num7 > num6)
					{
						num += (int)(num7 - num6);
					}
				}
			}
			if (num2 > 0 && num > num2)
			{
				num = num2;
			}
			return num - num3;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalcMoazafiTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2111);
			throw;
		}
	}

	private int CalcMoazafiTimeOfCouple(CoupleTaradod couple)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		int num = 0;
		try
		{
			int num2 = -1;
			int num3 = 0;
			foreach (ShiftDetView item in ShiftsOfCouple(couple, ShiftDetViews))
			{
				ShiftCodeing clsShiftCoding = item.ClsShiftCoding;
				num3 += couple.GetLunchMinutes(clsShiftCoding.StartLunchTime, (short)(clsShiftCoding.StartLunchTime + clsShiftCoding.LunchTime));
				double num4;
				double num5;
				if ((int)clsShiftCoding.TypeShift == 2)
				{
					if (num2 == -1)
					{
						num2 = (int)Math.Round(clsShiftCoding.Khoroj.TimeOfDay.TotalMinutes);
					}
					num4 = Math.Round(couple.Vtaradod.DateTimeLogM.TimeOfDay.TotalMinutes);
					num5 = Math.Round(couple.Ktaradod.DateTimeLogM.TimeOfDay.TotalMinutes);
				}
				else
				{
					num4 = ((couple.Vtaradod.DateTimeLogM.TimeOfDay <= clsShiftCoding.Vorod.TimeOfDay) ? Math.Round(clsShiftCoding.Vorod.TimeOfDay.TotalMinutes) : Math.Round(couple.Vtaradod.DateTimeLogM.TimeOfDay.TotalMinutes));
					num5 = ((couple.Ktaradod.DateTimeLogM.TimeOfDay >= clsShiftCoding.Khoroj.TimeOfDay) ? Math.Round(clsShiftCoding.Khoroj.TimeOfDay.TotalMinutes) : Math.Round(couple.Ktaradod.DateTimeLogM.TimeOfDay.TotalMinutes));
				}
				if (num5 > num4)
				{
					num += (int)(num5 - num4);
				}
			}
			if (num2 > 0 && num > num2)
			{
				num = num2;
			}
			return num - num3;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CalcMoazafiTimeOfCouple", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2164);
			throw;
		}
	}

	[Obsolete]
	public async Task<string> GetNameStatuseTaradodAsync(CancellationToken token)
	{
		try
		{
			StatuseTaradodEnumType val = await GetStatuseTaradodAsync(token);
			string display = Utilities.GetDisplay<StatuseTaradodEnumType>(val);
			string text = LanguageAndCalendar.Instance.GetValue(display);
			bool flag = (((int)val == 5 || (int)val == 14) ? true : false);
			if (flag && Morakhasi != null)
			{
				text = Morakhasi.TypeMorakhasiName;
				if (Morakhasi.Desc.Trim().Length > 0)
				{
					text = text + ":" + Morakhasi.Desc.Trim();
				}
			}
			if ((int)val == 6)
			{
				text = TatilRasmiToolTip ?? "";
			}
			return text;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public string GetShiftToolTip()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		try
		{
			if (Contract == null)
			{
				return "";
			}
			string text = "";
			List<ShiftDetView> shiftDetViews = ShiftDetViews;
			if (shiftDetViews == null || shiftDetViews.Count <= 0)
			{
				return text;
			}
			foreach (ShiftDetView shiftDetView in ShiftDetViews)
			{
				ShiftCodeing clsShiftCoding = shiftDetView.ClsShiftCoding;
				text = ((!(text.Trim() == "")) ? (text + ",[شیفت ورود] :" + Calendar.DateToFormatTime(clsShiftCoding.Vorod) + "\n[شیفت خروج] :" + Calendar.DateToFormatTime(clsShiftCoding.Khoroj) + "\n" + shiftDetView.Description) : (((int)clsShiftCoding.TypeShift != 2) ? (((int)clsShiftCoding.TypeShift != 11) ? ("[شیفت ورود] :" + Calendar.DateToFormatTime(clsShiftCoding.Vorod) + "\n[شیفت خروج] :" + Calendar.DateToFormatTime(clsShiftCoding.Khoroj) + "\n" + shiftDetView.Description) : "[شیفت استراحت]") : ("[شیفت شناور]  \n" + Calendar.DateToFormatTime(clsShiftCoding.Khoroj) + "\n" + shiftDetView.Description)));
			}
			return Utilities.GetTranslate(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetShiftToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2232);
		}
		return "";
	}

	public string GetHozorToolTip()
	{
		try
		{
			if (Contract == null)
			{
				return "";
			}
			string text = "";
			if (ShiftDetViews?.FirstOrDefault((ShiftDetView q) => q.ClsShiftCoding.HurryDelayPerform && (q.ClsShiftCoding.VorodEnd > q.ClsShiftCoding.VorodStart || q.ClsShiftCoding.KhorojEnd > q.ClsShiftCoding.KhorojStart)) != null)
			{
				text = "[با توجه به داشتن تعجیل و تاخیر در ورود و خروج کارکرد براساس شیفت محاسبه میشود]";
			}
			return Utilities.GetTranslate(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetHozorToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2251);
		}
		return "";
	}

	public static CoupleTaradod[] CreateCoupleTaradods(string contextGuid, List<LogOnPersonel> logOnPersonels, DateTime dateM, int personId)
	{
		try
		{
			CoupleTaradod[] array = new CoupleTaradod[8];
			for (int i = 0; i < 8; i++)
			{
				LogOnPersonel logOnPersonel = new LogOnPersonel(contextGuid, dateM);
				array[i] = new CoupleTaradod(contextGuid, dateM)
				{
					Ktaradod = logOnPersonel,
					Vtaradod = logOnPersonel,
					PersonNo = 0,
					DeviceId = 0,
					PersonId = personId,
					DateM = dateM.Date
				};
			}
			if (logOnPersonels != null && logOnPersonels.Any())
			{
				bool flag = true;
				int num = 0;
				foreach (LogOnPersonel logOnPersonel2 in logOnPersonels)
				{
					if (num < 8)
					{
						if (flag)
						{
							array[num].Vtaradod = logOnPersonel2;
							flag = false;
						}
						else
						{
							array[num].Ktaradod = logOnPersonel2;
							flag = true;
							num++;
						}
						continue;
					}
					break;
				}
			}
			return array;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private CoupleTaradod[] CreateCoupleTaradodsCurrentDay()
	{
		try
		{
			CoupleTaradod[] array = new CoupleTaradod[8];
			Stopwatch stopwatch = Stopwatch.StartNew();
			long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			_ = stopwatch.ElapsedMilliseconds;
			int personNo = 0;
			int deviceId = 0;
			if (PersonHardwareCodes == null)
			{
				List<HardwareCodeCUSView> list2 = (PersonHardwareCodes = HardwareCodeCUSView.Get(ContextGuid, ((PersonBase)PersonInfo).Id));
			}
			if (PersonHardwareCodes != null && PersonHardwareCodes.Count() > 0)
			{
				personNo = int.Parse(PersonHardwareCodes?.First()?.HardwareCode ?? "0");
				deviceId = (PersonHardwareCodes?.First()?.DeviceId).GetValueOrDefault(1);
			}
			int id = ((PersonBase)PersonInfo).Id;
			if (Devices == null)
			{
				Log.Error("devices is null in CreateCoupleTaradodsCurrentDay");
			}
			DeviceInfo deviceInfo = Devices?.SingleOrDefault((DeviceInfo p) => p.Id == deviceId);
			if (deviceInfo == null)
			{
				deviceInfo = Devices.FirstOrDefault();
				if (deviceInfo != null)
				{
					deviceId = deviceInfo.Id;
				}
			}
			if (deviceInfo == null)
			{
				Log.Error($"device is null in CreateCoupleTaradodsCurrentDay\r\n deviceid= {deviceId}");
			}
			for (int i = 0; i < 8; i++)
			{
				LogOnPersonel logOnPersonel = new LogOnPersonel(ContextGuid, DateM.Date)
				{
					DeviceId_ = (deviceInfo?.Id ?? 1),
					DeviceNo = (deviceInfo?.DeviceNo ?? 1),
					PersonNo = personNo
				};
				array[i] = new CoupleTaradod(ContextGuid, DateM)
				{
					Ktaradod = logOnPersonel,
					Vtaradod = logOnPersonel,
					PersonNo = personNo,
					DeviceId = deviceId,
					PersonId = id,
					DateM = DateM.Date
				};
			}
			_ = stopwatch.ElapsedMilliseconds;
			elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			List<LogOnPersonel> taradodsCurrentDay = TaradodsCurrentDay;
			if (taradodsCurrentDay != null && taradodsCurrentDay.Any())
			{
				bool flag = true;
				int num = 0;
				foreach (LogOnPersonel item in TaradodsCurrentDay)
				{
					if (num < 8)
					{
						if (flag)
						{
							array[num].Vtaradod = item;
							flag = false;
						}
						else
						{
							array[num].Ktaradod = item;
							flag = true;
							num++;
						}
						continue;
					}
					break;
				}
			}
			_ = stopwatch.ElapsedMilliseconds;
			elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			return array;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private CoupleTaradod[] CreateCoupleTaradodsNextDay()
	{
		CoupleTaradod[] array = new CoupleTaradod[8];
		try
		{
			int personNo = 0;
			short deviceId = 0;
			if (PersonHardwareCodes == null)
			{
				List<HardwareCodeCUSView> list2 = (PersonHardwareCodes = HardwareCodeCUSView.Get(ContextGuid, ((PersonBase)PersonInfo).Id));
			}
			if (PersonHardwareCodes != null && PersonHardwareCodes.Count() > 0)
			{
				personNo = int.Parse(PersonHardwareCodes?.First()?.HardwareCode ?? "0");
				deviceId = (PersonHardwareCodes?.First()?.DeviceId).GetValueOrDefault(1);
			}
			if (Devices == null)
			{
				Log.Error("devices is null in CreateCoupleTaradodsNextDay");
			}
			DeviceInfo deviceInfo = Devices?.SingleOrDefault((DeviceInfo p) => p.Id == deviceId);
			if (deviceInfo == null)
			{
				Log.Error($"device is null in CreateCoupleTaradodsNextDay\r\n deviceid= {deviceId}");
			}
			int id = ((PersonBase)PersonInfo).Id;
			for (int i = 0; i < 8; i++)
			{
				LogOnPersonel logOnPersonel = new LogOnPersonel(ContextGuid, DateM.AddDays(1.0).Date)
				{
					DeviceNo = (deviceInfo?.DeviceNo ?? 0),
					DeviceId_ = (deviceInfo?.Id ?? 0),
					PersonNo = personNo
				};
				array[i] = new CoupleTaradod(ContextGuid, DateM.AddDays(1.0).Date)
				{
					Ktaradod = logOnPersonel,
					Vtaradod = logOnPersonel,
					PersonNo = personNo,
					DeviceId = deviceId,
					PersonId = id,
					DateM = DateM.AddDays(1.0).Date
				};
			}
			List<LogOnPersonel> taradodsNextDay = TaradodsNextDay;
			if (taradodsNextDay != null && taradodsNextDay.Count > 0)
			{
				bool flag = true;
				int num = 0;
				foreach (LogOnPersonel item in TaradodsNextDay)
				{
					if (num < 8)
					{
						if (flag)
						{
							array[num].Vtaradod = item;
							flag = false;
						}
						else
						{
							array[num].Ktaradod = item;
							flag = true;
							num++;
						}
						continue;
					}
					break;
				}
			}
			return array;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateCoupleTaradodsNextDay", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2492);
			throw;
		}
	}

	public static async Task<ShiftDetView> GetShiftAsync(string contextGuid, DateTime taradodDatetime, int contractId, int personId, bool calcEkWhenHasVacationOrMission, int morakhadiDailyId, List<ShiftDetView> shiftLst = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (shiftLst == null)
			{
				shiftLst = await ShiftDetView.GetShiftDetViewListAsync(contextGuid, taradodDatetime, contractId, personId, token);
				await FixShiftCodingIfHasVacationAsync(contextGuid, "GetShiftAsync", taradodDatetime, personId, calcEkWhenHasVacationOrMission, morakhadiDailyId, shiftLst, token);
			}
			return await Shifts.GetShiftAsync(shiftLst, taradodDatetime, token);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2510);
			throw;
		}
	}

	private async Task<ShiftDetView> GetShiftAsync(CoupleTaradod couple, List<ShiftDetView> shiftLst, CancellationToken token)
	{
		_ = 1;
		try
		{
			if (shiftLst == null)
			{
				shiftLst = await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, couple.Vtaradod.DateTimeLogM, Contract.ID, Contract.PersonId, token);
				await FixShiftCodingIfHasVacationAsync(ContextGuid, "GetShiftAsync", DateM, ((PersonBase)PersonInfo).Id, Setting.CalcEkWhenHasVacationOrMission, Morakhasi?.Id ?? 0, shiftLst, token);
			}
			return Shifts.GetShiftOfCouple(shiftLst, couple);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2528);
			throw;
		}
	}

	private async Task<StatuseTaradodEnumType> CalcStatuseTaradodAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			int countTardod = 0;
			CoupleTaradod[] coupleTaradodCurDay = CoupleTaradodCurrentDay;
			for (int i = 0; i < 8; i++)
			{
				if (coupleTaradodCurDay != null && (coupleTaradodCurDay[i].Vtaradod.Id != 0 || coupleTaradodCurDay[i].Vtaradod.DateTimeLogM.TimeOfDay.TotalSeconds > 0.0))
				{
					countTardod++;
				}
				if (coupleTaradodCurDay != null && (coupleTaradodCurDay[i].Ktaradod.Id != 0 || coupleTaradodCurDay[i].Ktaradod.DateTimeLogM.TimeOfDay.TotalSeconds > 0.0))
				{
					countTardod++;
				}
			}
			if (countTardod % 2 != 0)
			{
				return (StatuseTaradodEnumType)2;
			}
			GharardadNew contract = Contract;
			bool flag = ((contract == null || contract.ID <= 0) ? true : false);
			if (flag || DateM > Contract.DateFinishM || DateM < Contract.DateStartM)
			{
				return (StatuseTaradodEnumType)((((PersonBase)PersonInfo).DateMFinishW > ConstantValues.MinDateM && ((PersonBase)PersonInfo).DateMFinishW < DateM) ? 18 : 4);
			}
			if (((int?)Morakhasi?.TypeId).GetValueOrDefault() == 5)
			{
				return (StatuseTaradodEnumType)14;
			}
			Morakhasidaily morakhasi = Morakhasi;
			if (morakhasi != null && morakhasi.Id > 0)
			{
				return (StatuseTaradodEnumType)5;
			}
			List<ShiftDetView> shiftDetViews = ShiftDetViews;
			if (shiftDetViews != null && shiftDetViews.Count > 0 && ShiftDetViews.Any((ShiftDetView p) => (int)p.ClsShiftCoding.TypeShift == 11))
			{
				return (StatuseTaradodEnumType)11;
			}
			int num = await GetKarkardAsync(token);
			Holiday holiday = Holiday;
			if (holiday != null && holiday.StRasmi && Holiday.DateM.DayOfWeek != DayOfWeek.Friday)
			{
				int num2;
				if (!(((PersonBase)PersonInfo).DateMFinishW > ConstantValues.MinDateM) || !(((PersonBase)PersonInfo).DateMFinishW < DateM))
				{
					shiftDetViews = ShiftDetViews;
					if (shiftDetViews == null || shiftDetViews.Count <= 0 || !ShiftDetViews.Any((ShiftDetView p) => p.WorkOnHolidaysWithOutFriDay) || num > 0)
					{
						shiftDetViews = ShiftDetViews;
						num2 = ((shiftDetViews != null && shiftDetViews.Count > 0 && ShiftDetViews.Any((ShiftDetView p) => p.WorkOnHolidaysWithOutFriDay) && num > 0 && TaradodsCurrentDay.Count % 2 == 0) ? 1 : ((countTardod % 2 != 0) ? 2 : 6));
					}
					else
					{
						num2 = 12;
					}
				}
				else
				{
					num2 = 19;
				}
				return (StatuseTaradodEnumType)num2;
			}
			holiday = Holiday;
			if (holiday != null && holiday.DateM.DayOfWeek == DayOfWeek.Friday)
			{
				int num3;
				if (!(((PersonBase)PersonInfo).DateMFinishW > ConstantValues.MinDateM) || !(((PersonBase)PersonInfo).DateMFinishW < DateM))
				{
					shiftDetViews = ShiftDetViews;
					num3 = ((shiftDetViews != null && shiftDetViews.Count > 0 && ShiftDetViews.Any((ShiftDetView p) => p.Fritype == ShiftDet.FriDayType.Elzami) && num <= 0) ? 13 : 7);
				}
				else
				{
					num3 = 20;
				}
				return (StatuseTaradodEnumType)num3;
			}
			if (SumShiftMinuts == 0 && num <= 0)
			{
				return (StatuseTaradodEnumType)((((PersonBase)PersonInfo).DateMFinishW > ConstantValues.MinDateM && ((PersonBase)PersonInfo).DateMFinishW < DateM) ? 18 : 8);
			}
			if (SumShiftMinuts > 0 && num <= 0 && coupleTaradodCurDay[0].VTime == "00:00")
			{
				if (((PersonBase)PersonInfo).DateMFinishW > ConstantValues.MinDateM && ((PersonBase)PersonInfo).DateMFinishW < DateM)
				{
					return (StatuseTaradodEnumType)18;
				}
				if (ShiftDetViews != null)
				{
					List<ShiftDetView> list = ShiftDetViews.Where((ShiftDetView p) => p.ClsShiftCoding.Vorod.TimeOfDay != p.ClsShiftCoding.Khoroj.TimeOfDay).ToList();
					ShiftCodeing clsShiftCoding = list[0].ClsShiftCoding;
					if (list != null && list.Count == 1 && clsShiftCoding.Vorod.TimeOfDay <= new TimeSpan(0, 0, 1) && (int)clsShiftCoding.TypeShift != 2)
					{
						int num4;
						if (countTardod % 2 == 0)
						{
							holiday = Holiday;
							num4 = ((holiday != null && holiday.StRasmi) ? 7 : 8);
						}
						else
						{
							num4 = 2;
						}
						return (StatuseTaradodEnumType)num4;
					}
					if (countTardod % 2 != 0)
					{
						return (StatuseTaradodEnumType)2;
					}
					if (coupleTaradodCurDay[0].Vtaradod.DateTimeLogM.Date > DateTime.Now.Date)
					{
						return (StatuseTaradodEnumType)22;
					}
					if (clsShiftCoding != null)
					{
						return (StatuseTaradodEnumType)((coupleTaradodCurDay[0].Vtaradod.DateTimeLogM.Date == DateTime.Now.Date && clsShiftCoding.Vorod.TimeOfDay >= DateTime.Now.TimeOfDay) ? 22 : 3);
					}
				}
			}
			int count = 0;
			for (int j = 0; j < 8; j++)
			{
				if (coupleTaradodCurDay != null && (coupleTaradodCurDay[j].Vtaradod.Id != 0 || coupleTaradodCurDay[j].Vtaradod.DateTimeLogM.TimeOfDay.TotalSeconds > 0.0))
				{
					count++;
				}
				if (coupleTaradodCurDay != null && (coupleTaradodCurDay[j].Ktaradod.Id != 0 || coupleTaradodCurDay[j].Ktaradod.DateTimeLogM.TimeOfDay.TotalSeconds > 0.0))
				{
					count++;
				}
				if (coupleTaradodCurDay != null && (int)coupleTaradodCurDay[j].TypeTaradod == 7)
				{
					if (coupleTaradodCurDay != null)
					{
						coupleTaradodCurDay[j].StatuseTaradodLst.Add((StatuseTaradodEnumType)17);
					}
					DetailStatusTypeTaradod += 2;
				}
				if (coupleTaradodCurDay != null && (int)coupleTaradodCurDay[j].TypeTaradod == 5)
				{
					if (coupleTaradodCurDay != null)
					{
						coupleTaradodCurDay[j].StatuseTaradodLst.Add((StatuseTaradodEnumType)9);
					}
					DetailStatusTypeTaradod++;
				}
			}
			if (coupleTaradodCurDay != null && count > 0 && (int)coupleTaradodCurDay[0].TypeTaradod != 7 && (int)coupleTaradodCurDay[0].TypeTaradod != 5 && coupleTaradodCurDay != null)
			{
				ShiftDetView shiftDetView = await GetShiftAsync(coupleTaradodCurDay[0], ShiftDetViews, token);
				if (shiftDetView != null)
				{
					ShiftCodeing clsShiftCoding2 = shiftDetView.ClsShiftCoding;
					if (shiftDetView != null && (int)clsShiftCoding2.TypeShift != 2 && coupleTaradodCurDay[0].Vtaradod.DateTimeLogM > clsShiftCoding2.VorodEnd)
					{
						coupleTaradodCurDay[0].StatuseTaradodLst.Add((StatuseTaradodEnumType)16);
						DetailStatusTypeTaradod += 8;
					}
				}
			}
			if (count > 0 && count % 2 == 0)
			{
				int idxx2 = count / 2;
				idxx2--;
				if (coupleTaradodCurDay != null && (int)coupleTaradodCurDay[idxx2].TypeTaradod != 7 && (int)coupleTaradodCurDay[idxx2].TypeTaradod != 5)
				{
					ShiftDetView shiftDetView2 = await GetShiftAsync(coupleTaradodCurDay[idxx2], ShiftDetViews, token);
					if (shiftDetView2 != null)
					{
						ShiftCodeing clsShiftCoding3 = shiftDetView2.ClsShiftCoding;
						if (coupleTaradodCurDay != null && shiftDetView2 != null && (int)clsShiftCoding3.TypeShift != 2 && coupleTaradodCurDay[idxx2].Ktaradod.DateTimeLogM < clsShiftCoding3.KhorojEnd)
						{
							coupleTaradodCurDay[0].StatuseTaradodLst.Add((StatuseTaradodEnumType)15);
							DetailStatusTypeTaradod += 4;
						}
					}
				}
				return (StatuseTaradodEnumType)1;
			}
			return (StatuseTaradodEnumType)2;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CalcStatuseTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 2706);
			throw;
		}
	}

	public async Task CalcTaradodAsyncNew(CancellationToken token)
	{
		_ = 22;
		try
		{
			int idx = 0;
			ShiftDetView shift = null;
			DateTime endOfShift = default(DateTime);
			DateTime beginOfShift = default(DateTime);
			DateTime beginOfLunch = default(DateTime);
			DateTime endOfLunch = default(DateTime);
			bool thisDayHasShift = false;
			await CalcEnteghaliTaradodsAsync(token);
			List<LogOnPersonel> tempTaradodList = new List<LogOnPersonel>();
			for (int i = 0; i < TaradodsCurrentDay.Count; i++)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				LogOnPersonel beforeTaradod = ((i > 0) ? TaradodsCurrentDay[i - 1] : null);
				LogOnPersonel curTaradod = TaradodsCurrentDay[i];
				LogOnPersonel nextTaradod = ((i + 1 != TaradodsCurrentDay.Count) ? TaradodsCurrentDay[i + 1] : null);
				ShiftDetView oldShift = shift;
				if (Contract != null)
				{
					shift = await Shifts.GetShiftAsync(ShiftDetViews, curTaradod.DateTimeLogM, token);
				}
				if (shift != null)
				{
					thisDayHasShift = true;
				}
				if (shift == null && oldShift != null && i % 2 != 0)
				{
					shift = oldShift;
				}
				ShiftCodeing shiftCodeing = null;
				if (shift != null)
				{
					shiftCodeing = shift.ClsShiftCoding;
					endOfShift = shiftCodeing.Khoroj;
					beginOfShift = shiftCodeing.Vorod;
					beginOfLunch = curTaradod.DateTimeLogM.Date.AddMinutes(shiftCodeing.StartLunchTime);
					endOfLunch = curTaradod.DateTimeLogM.Date.AddMinutes(shiftCodeing.StartLunchTime + shiftCodeing.LunchTime);
				}
				if (i % 2 == 0)
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					bool flag = shift != null;
					if (flag)
					{
						TypeTaradodEnumType typeTaradodVacationOrMission = curTaradod.TypeTaradodVacationOrMission;
						bool flag2 = (((int)typeTaradodVacationOrMission == 5 || (int)typeTaradodVacationOrMission == 7) ? true : false);
						flag = flag2;
					}
					bool flag3 = flag && curTaradod.DateTimeLogM > shiftCodeing.Vorod;
					if (flag3)
					{
						bool flag2 = i == 0;
						if (!flag2)
						{
							object obj = shift;
							flag2 = obj != await Shifts.GetShiftAsync(ShiftDetViews, beforeTaradod.DateTimeLogM, token);
						}
						flag3 = flag2 || beforeTaradod.DateTimeLogM < shiftCodeing.Vorod;
					}
					if (flag3)
					{
						if (!shiftCodeing.CalcVacationInLunch && shiftCodeing.LunchTime > 0)
						{
							if (curTaradod.DateTimeLogM < beginOfLunch)
							{
								tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, beginOfShift, curTaradod.DateTimeLogM));
								idx++;
								int duration = GetDuration(beginOfShift, curTaradod.DateTimeLogM);
								curTaradod.Duration = duration;
								if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync1", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
								{
									tempTaradodList.Add(curTaradod);
								}
							}
							else if (curTaradod.DateTimeLogM < endOfLunch)
							{
								tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, beginOfShift, beginOfLunch));
								idx++;
								int duration2 = GetDuration(beginOfShift, beginOfLunch);
								curTaradod.Duration = duration2;
								if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync2", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
								{
									tempTaradodList.Add(curTaradod);
								}
							}
							else
							{
								tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, beginOfShift, beginOfLunch));
								idx++;
								tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, endOfLunch, curTaradod.DateTimeLogM));
								idx++;
								int duration3 = GetDuration(beginOfShift, beginOfLunch);
								duration3 += GetDuration(endOfLunch, curTaradod.DateTimeLogM);
								curTaradod.Duration = duration3;
								if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync3", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
								{
									tempTaradodList.Add(curTaradod);
								}
							}
						}
						else
						{
							tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, beginOfShift, curTaradod.DateTimeLogM));
							idx++;
							int duration4 = GetDuration(beginOfShift, curTaradod.DateTimeLogM);
							curTaradod.Duration = duration4;
							if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync4", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
							{
								tempTaradodList.Add(curTaradod);
							}
						}
					}
					else
					{
						tempTaradodList.Add(curTaradod);
					}
				}
				else
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					idx++;
					bool flag3 = shift != null;
					if (flag3)
					{
						TypeTaradodEnumType typeTaradodVacationOrMission = curTaradod.TypeTaradodVacationOrMission;
						bool flag = (((int)typeTaradodVacationOrMission == 5 || (int)typeTaradodVacationOrMission == 7) ? true : false);
						flag3 = flag;
					}
					if (flag3)
					{
						if ((int)shiftCodeing.TypeShift == 2)
						{
							DateTime khorujeShiftShanavar = TaradodsCurrentDay[0].DateTimeLogM.AddMinutes(shiftCodeing.Khoroj.TimeOfDay.TotalMinutes);
							if (curTaradod.DateTimeLogM < khorujeShiftShanavar)
							{
								if (nextTaradod != null && nextTaradod.DateTimeLogM < khorujeShiftShanavar)
								{
									int duration5 = GetDuration(curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM);
									curTaradod.Duration = duration5;
									if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync5", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
									{
										tempTaradodList.Add(curTaradod);
									}
									tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM));
								}
								else
								{
									int duration6 = GetDuration(curTaradod.DateTimeLogM, khorujeShiftShanavar);
									curTaradod.Duration = duration6;
									if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync6", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
									{
										tempTaradodList.Add(curTaradod);
									}
									tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, khorujeShiftShanavar));
								}
								idx++;
							}
						}
						else if (curTaradod.DateTimeLogM > shiftCodeing.Vorod && curTaradod.DateTimeLogM < shiftCodeing.Khoroj)
						{
							if (nextTaradod == null || nextTaradod.DateTimeLogM >= endOfShift)
							{
								if (!shiftCodeing.CalcVacationInLunch && shiftCodeing.LunchTime > 0)
								{
									if (curTaradod.DateTimeLogM < beginOfLunch)
									{
										int duration7 = GetDuration(curTaradod.DateTimeLogM, beginOfLunch);
										duration7 += GetDuration(endOfLunch, endOfShift);
										curTaradod.Duration = duration7;
										if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync7", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
										{
											tempTaradodList.Add(curTaradod);
										}
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, beginOfLunch));
										idx++;
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, endOfLunch, endOfShift));
										idx++;
									}
									else if (curTaradod.DateTimeLogM < endOfLunch)
									{
										int duration8 = GetDuration(endOfLunch, endOfShift);
										curTaradod.Duration = duration8;
										if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync8", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
										{
											tempTaradodList.Add(curTaradod);
										}
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, endOfLunch, endOfShift));
										idx++;
									}
									else
									{
										int duration9 = GetDuration(curTaradod.DateTimeLogM, endOfShift);
										curTaradod.Duration = duration9;
										if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync9", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
										{
											tempTaradodList.Add(curTaradod);
										}
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, endOfShift));
										idx++;
									}
								}
								else
								{
									int duration10 = GetDuration(curTaradod.DateTimeLogM, endOfShift);
									curTaradod.Duration = duration10;
									if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync10", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
									{
										tempTaradodList.Add(curTaradod);
									}
									tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, endOfShift));
									if (endOfShift.TimeOfDay == new TimeSpan(23, 59, 59) && (TaradodsNextDay?.Any() ?? false))
									{
										LogOnPersonel firstTaradod = TaradodsNextDay[0];
										List<ShiftDetView> list = await GetShiftDetViewsNextDayAsync(token);
										if (list != null && list.Any() && firstTaradod.DateTimeLogM < list[0].ClsShiftCoding.KhorojEnd)
										{
											firstTaradod.TypeTaradodVacationOrMission = curTaradod.TypeTaradodVacationOrMission;
										}
									}
									idx++;
								}
							}
							else if (nextTaradod.DateTimeLogM > endOfLunch)
							{
								if (!shiftCodeing.CalcVacationInLunch && shiftCodeing.LunchTime > 0)
								{
									if (curTaradod.DateTimeLogM < beginOfLunch)
									{
										int duration11 = GetDuration(curTaradod.DateTimeLogM, beginOfLunch);
										duration11 += GetDuration(endOfLunch, nextTaradod.DateTimeLogM);
										curTaradod.Duration = duration11;
										if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync11", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
										{
											tempTaradodList.Add(curTaradod);
										}
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, beginOfLunch));
										idx++;
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, endOfLunch, nextTaradod.DateTimeLogM));
										idx++;
									}
									else if (curTaradod.DateTimeLogM < endOfLunch)
									{
										int duration12 = GetDuration(endOfLunch, nextTaradod.DateTimeLogM);
										curTaradod.Duration = duration12;
										if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync12", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
										{
											tempTaradodList.Add(curTaradod);
										}
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, endOfLunch, nextTaradod.DateTimeLogM));
										idx++;
									}
									else
									{
										int duration13 = GetDuration(curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM);
										curTaradod.Duration = duration13;
										if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync13", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
										{
											tempTaradodList.Add(curTaradod);
										}
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM));
										idx++;
									}
								}
								else
								{
									int duration14 = GetDuration(curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM);
									curTaradod.Duration = duration14;
									if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync14", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
									{
										tempTaradodList.Add(curTaradod);
									}
									tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM));
									idx++;
								}
							}
							else if (nextTaradod.DateTimeLogM > beginOfLunch && nextTaradod.DateTimeLogM < endOfLunch)
							{
								if (!shiftCodeing.CalcVacationInLunch && shiftCodeing.LunchTime > 0)
								{
									if (curTaradod.DateTimeLogM < beginOfLunch)
									{
										int duration15 = GetDuration(curTaradod.DateTimeLogM, beginOfLunch);
										curTaradod.Duration = duration15;
										if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync15", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
										{
											tempTaradodList.Add(curTaradod);
										}
										tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, beginOfLunch));
										idx++;
									}
								}
								else
								{
									int duration16 = GetDuration(curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM);
									curTaradod.Duration = duration16;
									if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync16", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
									{
										tempTaradodList.Add(curTaradod);
									}
									tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM));
									idx++;
								}
							}
							else if (nextTaradod.DateTimeLogM < beginOfLunch)
							{
								int duration17 = GetDuration(curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM);
								curTaradod.Duration = duration17;
								if (await curTaradod.SaveAsync("TaradodFullDet.calcTaradodAsync17", token, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs") > 0)
								{
									tempTaradodList.Add(curTaradod);
								}
								tempTaradodList.AddRange(GetTaradodsVacationOrMissionNew(curTaradod, curTaradod.DateTimeLogM, nextTaradod.DateTimeLogM));
								idx++;
							}
						}
					}
					else
					{
						tempTaradodList.Add(curTaradod);
					}
				}
				if (!tempTaradodList.Contains(curTaradod))
				{
					tempTaradodList.Add(curTaradod);
				}
			}
			TaradodsCurrentDay = tempTaradodList;
			if (token.IsCancellationRequested)
			{
				return;
			}
			ResetCoupleTaradodCurrentDay();
			if (!token.IsCancellationRequested)
			{
				Stopwatch sw22 = Stopwatch.StartNew();
				if (shift == null)
				{
					await UpdateCoupleTypesAsync(thisDayHasShift: false, token);
				}
				else
				{
					ShiftCodeing clsShiftCoding = shift.ClsShiftCoding;
					await UpdateCoupleTypesAsync(thisDayHasShift && clsShiftCoding.Vorod != clsShiftCoding.Khoroj, token);
				}
				_ = sw22.ElapsedMilliseconds;
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "Day : " + DayofShMonth, "CalcTaradodAsyncNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 3467);
			throw;
		}
	}

	private void ResetCoupleTaradodCurrentDay()
	{
		_coupleTaradodCurrentDay = null;
	}

	private int GetDuration(DateTime fromDateTime, DateTime toDateTime)
	{
		try
		{
			return CoupleTaradod.TimeDiff(fromDateTime, toDateTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDuration", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 3598);
		}
		return 0;
	}

	private async Task<List<LogOnPersonel>> AddCoupleEnteghaliOfBeforeDateAsync(List<LogOnPersonel> taradodList, CancellationToken token)
	{
		_ = 4;
		try
		{
			ShiftDetView shiftEnteghali = ShiftDetViews?.FirstOrDefault((ShiftDetView p) => p.ClsShiftCoding.NextDay && p.ClsShiftCoding.Vorod.TimeOfDay <= new TimeSpan(0, 0, 1));
			if (shiftEnteghali == null)
			{
				return taradodList;
			}
			DateTime date = DateM.AddDays(-1.0).Date;
			List<LogOnPersonel> list = await LogOnPersonel.GetAllByPersonIdAsync(ContextGuid, ((PersonBase)PersonInfo).Id, date, token);
			if (list == null)
			{
				return taradodList;
			}
			LogOnPersonel lastTaradodBeforeDay = list.LastOrDefault();
			if (lastTaradodBeforeDay == null)
			{
				return taradodList;
			}
			ShiftDetView shiftDetView = await GetShiftAsync(ContextGuid, lastTaradodBeforeDay.DateTimeLogM, Contract.ID, ((PersonBase)PersonInfo).Id, Setting.CalcEkWhenHasVacationOrMission, Morakhasi.Id, null, token);
			TypeTaradodEnumType typeTaradodVacationOrMission = lastTaradodBeforeDay.TypeTaradodVacationOrMission;
			bool flag = (((int)typeTaradodVacationOrMission == 5 || (int)typeTaradodVacationOrMission == 7) ? true : false);
			if (flag && shiftDetView.ClsShiftCoding.KhorojEnd.TimeOfDay >= new TimeSpan(23, 59, 59))
			{
				if (!(TaradodsCurrentDay?.Any() ?? false))
				{
					taradodList = GetTaradodsVacationOrMissionNew(lastTaradodBeforeDay, DateM.Date, shiftEnteghali.ClsShiftCoding.Khoroj);
				}
				else
				{
					ShiftDetView shiftDetView2 = await GetShiftAsync(ContextGuid, TaradodsCurrentDay[0].DateTimeLogM, Contract.ID, ((PersonBase)PersonInfo).Id, Setting.CalcEkWhenHasVacationOrMission, Morakhasi.Id, ShiftDetViews, token);
					taradodList = ((shiftDetView2 != null && !(shiftDetView2.ClsShiftCoding.Vorod.TimeOfDay > new TimeSpan(0, 0, 1))) ? ((!(TaradodsCurrentDay[0].DateTimeLogM.TimeOfDay > shiftDetView2.ClsShiftCoding.Khoroj.TimeOfDay)) ? GetTaradodsVacationOrMissionNew(lastTaradodBeforeDay, DateM.Date, TaradodsCurrentDay[0].DateTimeLogM) : GetTaradodsVacationOrMissionNew(lastTaradodBeforeDay, DateM.Date, shiftEnteghali.ClsShiftCoding.Khoroj)) : GetTaradodsVacationOrMissionNew(lastTaradodBeforeDay, DateM.Date, shiftEnteghali.ClsShiftCoding.Khoroj));
				}
			}
			else if (TaradodsCurrentDay?.Any() ?? false)
			{
				LogOnPersonel firstTaradod = TaradodsCurrentDay.First();
				ShiftDetView shiftDetView3 = await GetShiftAsync(ContextGuid, firstTaradod.DateTimeLogM, Contract.ID, ((PersonBase)PersonInfo).Id, Setting.CalcEkWhenHasVacationOrMission, Morakhasi.Id, ShiftDetViews, token);
				if (shiftDetView3 != null && shiftDetView3.ClsShiftCoding.NextDay && shiftDetView3.ClsShiftCoding.Vorod.TimeOfDay.TotalSeconds == 0.0)
				{
					LogOnPersonel item = new LogOnPersonel(ContextGuid, shiftDetView3.ClsShiftCoding.Vorod)
					{
						IsVirtual = (SavedTaradodEnumType)4,
						TypeTaradodVacationOrMission = firstTaradod.TypeTaradodVacationOrMission
					};
					taradodList.Add(item);
				}
				LogOnPersonel lastTaradod = TaradodsCurrentDay.Last();
				if (firstTaradod != lastTaradod)
				{
					shiftDetView3 = await GetShiftAsync(ContextGuid, lastTaradod.DateTimeLogM, Contract.ID, ((PersonBase)PersonInfo).Id, Setting.CalcEkWhenHasVacationOrMission, Morakhasi.Id, ShiftDetViews, token);
					if (shiftDetView3 != null && shiftDetView3.ClsShiftCoding.NextDay && shiftDetView3.ClsShiftCoding.Khoroj.TimeOfDay.TotalSeconds >= 86399.0)
					{
						LogOnPersonel item2 = new LogOnPersonel(ContextGuid, shiftDetView3.ClsShiftCoding.Khoroj)
						{
							IsVirtual = (SavedTaradodEnumType)4,
							TypeTaradodVacationOrMission = lastTaradod.TypeTaradodVacationOrMission
						};
						taradodList.Add(item2);
					}
				}
			}
			return taradodList;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task CalcEnteghaliTaradodsAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			if (TaradodsCurrentDay == null || TaradodsCurrentDay.Count % 2 == 0 || TaradodsNextDay == null)
			{
				return;
			}
			LogOnPersonel lastTaradod = TaradodsCurrentDay.LastOrDefault();
			if (lastTaradod == null || (int)lastTaradod.IsVirtual == 6)
			{
				return;
			}
			LogOnPersonel firstTaradodNextDay = TaradodsNextDay?.FirstOrDefault();
			if (firstTaradodNextDay == null || firstTaradodNextDay.DateTimeLogM.TimeOfDay.TotalSeconds < 2.0)
			{
				return;
			}
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			if (ShiftDetViews != null && ShiftDetViews.Count > 0)
			{
				ShiftDetView shiftDetView = ShiftDetViews?.LastOrDefault((ShiftDetView p) => p.ClsShiftCoding.NextDay);
				if (shiftDetView == null)
				{
					return;
				}
				if ((int)shiftDetView.ClsShiftCoding.TypeShift == 2)
				{
					double num = Math.Round((new TimeSpan(23, 59, 59) - lastTaradod.DateTimeLogM.TimeOfDay).TotalMinutes);
					int num2 = (int)firstTaradodNextDay.DateTimeLogM.TimeOfDay.TotalMinutes;
					if (num + (double)num2 > (double)shiftDetView.ClsShiftCoding.MaxTimeCanExit)
					{
						return;
					}
				}
				else if (firstTaradodNextDay.DateTimeLogM.TimeOfDay.TotalMinutes > (double)shiftDetView.ClsShiftCoding.MaxTimeCanExit)
				{
					return;
				}
			}
			else if (!timeAttendanceSettings.AutomaticCalcMovetoNextDay)
			{
				return;
			}
			LogOnPersonel enteghali = GetTaradodEnteghali24Today(lastTaradod);
			if (enteghali != null)
			{
				if (enteghali.DeviceId_ <= 0)
				{
					enteghali.DeviceId_ = 1;
				}
				enteghali.TypeTaradodVacationOrMission = (TypeTaradodEnumType)34;
				LogOnPersonel logOnPersonel = enteghali;
				logOnPersonel.Id = await enteghali.SaveAsync("CalcEnteghaliTaradods", token, "CalcEnteghaliTaradodsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs");
				TaradodsCurrentDay.Add(enteghali);
			}
			LogOnPersonel enteghali0 = GetTaradodEnteghaliZeroTomarrow(lastTaradod);
			if (enteghali0 != null)
			{
				if (enteghali0.DeviceId_ <= 0)
				{
					enteghali0.DeviceId_ = 1;
				}
				enteghali0.TypeTaradodVacationOrMission = (TypeTaradodEnumType)34;
				LogOnPersonel logOnPersonel = enteghali0;
				logOnPersonel.Id = await enteghali0.SaveAsync("CalcEnteghaliTaradods", token, "CalcEnteghaliTaradodsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs");
				TaradodsNextDay?.Add(enteghali0);
				TaradodsNextDay = TaradodsNextDay.OrderBy((LogOnPersonel p) => p.DateTimeLogM).ToList();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private List<LogOnPersonel> GetTaradodsVacationOrMissionNew(LogOnPersonel taradodForClone, DateTime fromTime, DateTime toTime)
	{
		try
		{
			List<LogOnPersonel> list = new List<LogOnPersonel>();
			LogOnPersonel logOnPersonel = taradodForClone.Clone();
			logOnPersonel.DateTimeLogM = fromTime;
			logOnPersonel.IsVirtual = (SavedTaradodEnumType)4;
			if (logOnPersonel.DateTimeLogM.TimeOfDay.TotalSeconds == 0.0)
			{
				logOnPersonel.DateTimeLogM = logOnPersonel.DateTimeLogM.AddSeconds(1.0);
			}
			LogOnPersonel logOnPersonel2 = taradodForClone.Clone();
			logOnPersonel2.DateTimeLogM = toTime;
			logOnPersonel2.IsVirtual = (SavedTaradodEnumType)4;
			list.Add(logOnPersonel);
			list.Add(logOnPersonel2);
			return list;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static List<LogOnPersonel> GetCoupleTaradodEnteghali(LogOnPersonel taradodForCopyFrom, DateTime fromTime, DateTime toTime)
	{
		try
		{
			List<LogOnPersonel> list = new List<LogOnPersonel>();
			LogOnPersonel logOnPersonel = taradodForCopyFrom.Clone();
			logOnPersonel.DateTimeLogM = fromTime;
			logOnPersonel.IsVirtual = (SavedTaradodEnumType)6;
			LogOnPersonel logOnPersonel2 = taradodForCopyFrom.Clone();
			logOnPersonel2.DateTimeLogM = toTime;
			logOnPersonel2.IsVirtual = (SavedTaradodEnumType)6;
			list.Add(logOnPersonel);
			list.Add(logOnPersonel2);
			return list;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static LogOnPersonel GetTaradodEnteghali24Today(LogOnPersonel taradodForCopyFrom)
	{
		try
		{
			LogOnPersonel logOnPersonel = taradodForCopyFrom.Clone();
			logOnPersonel.DateTimeLogM = logOnPersonel.DateTimeLogM.Date.AddSeconds(86399.0);
			logOnPersonel.IsVirtual = (SavedTaradodEnumType)6;
			return logOnPersonel;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static LogOnPersonel GetTaradodEnteghaliZeroTomarrow(LogOnPersonel taradodForCopyFrom)
	{
		try
		{
			LogOnPersonel logOnPersonel = taradodForCopyFrom.Clone();
			logOnPersonel.DateTimeLogM = logOnPersonel.DateTimeLogM.Date.AddDays(1.0).AddSeconds(1.0);
			logOnPersonel.IsVirtual = (SavedTaradodEnumType)6;
			return logOnPersonel;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task RemoveEnteghliTaradodsOfCurDateAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			LogOnPersonel lastTaradodEnteghaliCurDate = TaradodsCurrentDay.Find((LogOnPersonel p) => (int)p.IsVirtual == 6 && p.DateTimeLogM.TimeOfDay >= new TimeSpan(23, 59, 59));
			if (lastTaradodEnteghaliCurDate != null)
			{
				await lastTaradodEnteghaliCurDate.DeleteAsync(token);
				TaradodsCurrentDay.Remove(lastTaradodEnteghaliCurDate);
			}
			LogOnPersonel x = TaradodsNextDay?.FirstOrDefault((LogOnPersonel p) => (int)p.IsVirtual != 6);
			if (x == null)
			{
				return;
			}
			List<LogOnPersonel> list = TaradodsNextDay.Where((LogOnPersonel p) => p.DateTimeLogM <= x.DateTimeLogM && (int)p.IsVirtual == 6).ToList();
			if (list.Count <= 0)
			{
				return;
			}
			foreach (LogOnPersonel logOnPersonel in list)
			{
				await logOnPersonel.DeleteAsync(token);
				TaradodsNextDay.Remove(logOnPersonel);
				ResetCoupleTaradodNextDay();
				ReloadNextRow = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RemoveEnteghliTaradodsOfCurDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 3914);
		}
	}

	private async Task RemoveEnteghliTaradodsOfCurDateNewAsync(CancellationToken token)
	{
		try
		{
			IEnumerable<LogOnPersonel> enumerable = TaradodsCurrentDay.Where((LogOnPersonel p) => (int)p.IsVirtual == 6);
			if (enumerable == null)
			{
				return;
			}
			foreach (LogOnPersonel taradod in enumerable)
			{
				await taradod.DeleteAsync(token);
				TaradodsCurrentDay.Remove(taradod);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "RemoveEnteghliTaradodsOfCurDateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 3939);
			throw;
		}
	}

	private async Task UpdateCoupleTypesAsync(bool thisDayHasShift, CancellationToken token)
	{
		_ = 2;
		try
		{
			Stopwatch sw = Stopwatch.StartNew();
			long start = sw.ElapsedMilliseconds;
			CoupleTaradod[] coupleTaradods = CoupleTaradodCurrentDay;
			_ = sw.ElapsedMilliseconds - start;
			start = sw.ElapsedMilliseconds;
			bool shouldCalcHoliday = Setting.CodeingAccounting.IsCalcHoliday;
			bool isFriday = coupleTaradods[0].DateM.DayOfWeek == DayOfWeek.Friday;
			bool isHoliday = (await Holiday.GetAsync(ContextGuid, coupleTaradods[0].DateM))?.StRasmi ?? false;
			List<int> listSw = new List<int>();
			List<int> listSw2 = new List<int>();
			CoupleTaradod[] array = coupleTaradods;
			foreach (CoupleTaradod coupleTaradod in array)
			{
				if (coupleTaradod.VTime == "00:00" && coupleTaradod.KTime == "00:00")
				{
					coupleTaradod.TypeTaradod = (TypeTaradodEnumType)3;
					continue;
				}
				TypeTaradodEnumType typeTaradodVacationOrMission;
				if (coupleTaradod.IsTaradodVirtual)
				{
					typeTaradodVacationOrMission = coupleTaradod.Vtaradod.TypeTaradodVacationOrMission;
					if (((int)typeTaradodVacationOrMission == 5 || (int)typeTaradodVacationOrMission == 7) ? true : false)
					{
						coupleTaradod.TypeTaradod = coupleTaradod.Vtaradod.TypeTaradodVacationOrMission;
						continue;
					}
					typeTaradodVacationOrMission = coupleTaradod.Ktaradod.TypeTaradodVacationOrMission;
					if (((int)typeTaradodVacationOrMission == 5 || (int)typeTaradodVacationOrMission == 7) ? true : false)
					{
						coupleTaradod.TypeTaradod = coupleTaradod.Ktaradod.TypeTaradodVacationOrMission;
					}
					continue;
				}
				typeTaradodVacationOrMission = coupleTaradod.Vtaradod.TypeTaradod;
				if (((int)typeTaradodVacationOrMission == 10 || typeTaradodVacationOrMission - 91 <= 2) ? true : false)
				{
					coupleTaradod.TypeTaradod = coupleTaradod.Vtaradod.TypeTaradod;
					continue;
				}
				typeTaradodVacationOrMission = coupleTaradod.Ktaradod.TypeTaradod;
				if (((int)typeTaradodVacationOrMission == 10 || typeTaradodVacationOrMission - 91 <= 2) ? true : false)
				{
					coupleTaradod.TypeTaradod = coupleTaradod.Ktaradod.TypeTaradod;
					continue;
				}
				if (isHoliday && shouldCalcHoliday && !thisDayHasShift)
				{
					if ((int)coupleTaradod.Vtaradod.IsVirtual != 6)
					{
						coupleTaradod.TypeTaradod = (TypeTaradodEnumType)(isFriday ? 8 : 28);
					}
					else if (Contract != null)
					{
						List<ShiftDetView> shiftDetViews = ShiftDetViews;
						long num = sw.ElapsedMilliseconds - start;
						start = sw.ElapsedMilliseconds;
						listSw.Add((int)num);
						if (!thisDayHasShift)
						{
							List<ShiftDetView> shiftDetsYesterday = await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, DateM.AddDays(-1.0), Contract.ID, Contract.PersonId, token);
							await FixShiftCodingIfHasVacationAsync(ContextGuid, "UpdateCoupleTypesAsync", DateM, ((PersonBase)PersonInfo).Id, Setting.CalcEkWhenHasVacationOrMission, Morakhasi?.Id ?? 0, shiftDetsYesterday, token);
							long num2 = sw.ElapsedMilliseconds - start;
							start = sw.ElapsedMilliseconds;
							listSw2.Add((int)num2);
							coupleTaradod.TypeTaradod = (TypeTaradodEnumType)((shiftDetsYesterday == null || shiftDetsYesterday.Count <= 0 || shiftDetsYesterday[shiftDetsYesterday.Count - 1].ClsShiftCoding.CalcEnteghalInSrcDate) ? 3 : (isFriday ? 8 : 28));
						}
						else
						{
							coupleTaradod.TypeTaradod = (TypeTaradodEnumType)((shiftDetViews == null || shiftDetViews.Count <= 0 || shiftDetViews[0].ClsShiftCoding.CalcEnteghalInSrcDate) ? 3 : (isFriday ? 8 : 28));
						}
					}
					continue;
				}
				List<ShiftDetView> list = null;
				if (Contract != null)
				{
					list = ShiftsOfCouple(coupleTaradod, ShiftDetViews);
				}
				if (list != null && list.Count > 0)
				{
					TypeTaradodInShiftEnum typeTaradodInShift = list[0].ClsShiftCoding.TypeTaradodInShift;
					switch ((int)typeTaradodInShift)
					{
					case 0:
						coupleTaradod.TypeTaradod = (TypeTaradodEnumType)34;
						break;
					case 1:
						coupleTaradod.TypeTaradod = (TypeTaradodEnumType)91;
						break;
					case 2:
						coupleTaradod.TypeTaradod = (TypeTaradodEnumType)92;
						break;
					case 3:
						coupleTaradod.TypeTaradod = (TypeTaradodEnumType)93;
						break;
					case 4:
						coupleTaradod.TypeTaradod = (TypeTaradodEnumType)10;
						break;
					default:
						coupleTaradod.TypeTaradod = (TypeTaradodEnumType)3;
						break;
					}
				}
				else
				{
					coupleTaradod.TypeTaradod = (TypeTaradodEnumType)3;
				}
			}
			_ = sw.ElapsedMilliseconds;
			_ = sw.ElapsedMilliseconds;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UpdateCoupleTypesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 4059);
		}
	}

	private async Task AddToTaradodNextDayAsync(LogOnPersonel taradodNextDay, CancellationToken token)
	{
		try
		{
			LogOnPersonel logOnPersonel = TaradodsNextDay.Find((LogOnPersonel p) => p.Id == taradodNextDay.Id);
			if (logOnPersonel != null)
			{
				TaradodsNextDay.Remove(logOnPersonel);
			}
			else if ((int)taradodNextDay.IsVirtual == 4)
			{
				logOnPersonel = TaradodsNextDay.Find((LogOnPersonel p) => p.DateTimeLogM == taradodNextDay.DateTimeLogM && (int)p.IsVirtual == 4);
				if (logOnPersonel != null)
				{
					TaradodsNextDay.Remove(logOnPersonel);
				}
			}
			TaradodsNextDay.Add(taradodNextDay);
			TaradodsNextDay = TaradodsNextDay.OrderBy((LogOnPersonel p) => p.DateTimeLogM).ToList();
			ResetCoupleTaradodNextDay();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddToTaradodNextDayAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 4160);
		}
	}

	private async Task AddToTaradodCurrentDayAsync(LogOnPersonel taradodCurrentDay, CancellationToken token)
	{
		try
		{
			LogOnPersonel logOnPersonel = TaradodsCurrentDay.Find((LogOnPersonel p) => p.Id == taradodCurrentDay.Id);
			if (logOnPersonel != null)
			{
				TaradodsCurrentDay.Remove(logOnPersonel);
			}
			else if ((int)taradodCurrentDay.IsVirtual == 4)
			{
				logOnPersonel = TaradodsCurrentDay.Find((LogOnPersonel p) => p.DateTimeLogM == taradodCurrentDay.DateTimeLogM && (int)p.IsVirtual == 4);
				if (logOnPersonel != null)
				{
					TaradodsCurrentDay.Remove(logOnPersonel);
				}
			}
			TaradodsCurrentDay.Add(taradodCurrentDay);
			TaradodsCurrentDay = TaradodsCurrentDay.OrderBy((LogOnPersonel p) => p.DateTimeLogM).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddToTaradodCurrentDayAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 4184);
		}
	}

	private void ResetCoupleTaradodNextDay()
	{
		_coupleTaradodNextDay = null;
	}

	public bool IsWorkday(EnWorkdayEnumType pEnWorkday)
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected I4, but got Unknown
		try
		{
			if (Contract == null || Contract.DateFinishM < DateM)
			{
				if (Setting.NotKarkardWhenNoGharardad)
				{
					return false;
				}
				return Hozor.Value.TotalMinutes > 0.0;
			}
			double karkard = Hozor.Value.TotalMinutes;
			if (karkard > 0.0)
			{
				Holiday holiday = Holiday;
				if (holiday == null || holiday.DateM.DayOfWeek != DayOfWeek.Friday)
				{
					return true;
				}
			}
			if ((Morakhasi?.Id ?? 0) > 0)
			{
				return (int)pEnWorkday switch
				{
					0 => Morakhasi.HasBimeh, 
					1 => Morakhasi.HasHoghogh, 
					2 => Morakhasi.HasMaliyat, 
					_ => false, 
				};
			}
			List<ShiftDetView> shiftDetViews = ShiftDetViews;
			if (shiftDetViews == null || shiftDetViews.Count <= 0)
			{
				return karkard > 0.0;
			}
			if (ShiftDetViews.Any((ShiftDetView shiftDet) => (int)shiftDet.ClsShiftCoding.TypeShift == 11))
			{
				return true;
			}
			Holiday holiday2 = Holiday;
			if (holiday2 != null && holiday2.DateM.DayOfWeek == DayOfWeek.Friday)
			{
				return ShiftDetViews.Any((ShiftDetView p) => p.Fritype == ShiftDet.FriDayType.EkhtiyariAndKarkard || (p.Fritype == ShiftDet.FriDayType.Elzami && karkard > 0.0));
			}
			return ShiftDetViews.Any((ShiftDetView shiftdet) => shiftdet.IsDayTatilInTaghvim && !shiftdet.WorkOnHolidaysWithOutFriDay);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsWorkday", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDet.cs", 4368);
			throw;
		}
	}

	private static async Task<bool> CalcTaradodsEnteghaliAsync(string contextGuid, int personId, DateTime FromDate, DateTime ToDate, List<ShiftDetView> shiftDetViews, CancellationToken token)
	{
		_ = 1;
		try
		{
			List<LogOnPersonel> allTaradods = await LogOnPersonel.GetAllByPersonIdAsync(contextGuid, personId, FromDate.Date.AddDays(-1.0), ToDate.Date.AddDays(2.0), token);
			if (allTaradods == null || allTaradods.Count() <= 0)
			{
				return true;
			}
			DateTime currentDate = FromDate;
			while (currentDate.Date <= ToDate.Date)
			{
				await CalcTaradodsEnteghaliForDateAsync(contextGuid, allTaradods, currentDate, shiftDetViews, token);
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
		return false;
	}

	private static async Task CalcTaradodsEnteghaliForDateAsync(string contextGuid, List<LogOnPersonel> allTaradods, DateTime currentDate, List<ShiftDetView> shiftDetViews, CancellationToken token)
	{
		try
		{
			List<LogOnPersonel> processedTaradodList = new List<LogOnPersonel>();
			IEnumerable<ShiftDetView> enumerable = shiftDetViews?.Where((ShiftDetView p) => p.ClsShiftCoding.NextDay);
			if (enumerable == null || enumerable.Count() <= 0 || allTaradods == null || allTaradods.Count() <= 0)
			{
				return;
			}
			DateTime yesterdayDate = currentDate.Date.AddDays(-1.0);
			DateTime tomarrowDate = currentDate.Date.AddDays(1.0);
			List<LogOnPersonel> todayTaradods = allTaradods.Where((LogOnPersonel p) => p.DateTimeLogM >= currentDate.Date && p.DateTimeLogM <= currentDate.Date.AddSeconds(86399.0)).ToList();
			IEnumerable<LogOnPersonel> enumerable2 = allTaradods.Where((LogOnPersonel p) => p.DateTimeLogM >= yesterdayDate.Date && p.DateTimeLogM <= yesterdayDate.Date.AddSeconds(86399.0));
			IEnumerable<LogOnPersonel> enumerable3 = allTaradods.Where((LogOnPersonel p) => p.DateTimeLogM >= tomarrowDate.Date && p.DateTimeLogM <= tomarrowDate.Date.AddSeconds(86399.0));
			LogOnPersonel lastTaradodYesterday = enumerable2?.Last();
			enumerable3?.First();
			LogOnPersonel firstTaradodToday = todayTaradods?.First();
			LogOnPersonel lastTaradodToday = todayTaradods?.Last();
			if (lastTaradodYesterday == null)
			{
				return;
			}
			ShiftDetView shiftEnteghaliMorning = enumerable.FirstOrDefault((ShiftDetView p) => p.ClsShiftCoding.Vorod.TimeOfDay <= new TimeSpan(0, 0, 1));
			if (shiftEnteghaliMorning == null)
			{
				return;
			}
			TypeTaradodEnumType typeTaradodVacationOrMission = lastTaradodYesterday.TypeTaradodVacationOrMission;
			bool flag = (((int)typeTaradodVacationOrMission == 5 || (int)typeTaradodVacationOrMission == 7) ? true : false);
			if (flag && lastTaradodYesterday.DateTimeLogM.TimeOfDay >= new TimeSpan(23, 59, 59))
			{
				if (!(todayTaradods?.Any() ?? false))
				{
					processedTaradodList.AddRange(GetCoupleTaradodEnteghali(lastTaradodYesterday, currentDate.Date, shiftEnteghaliMorning.ClsShiftCoding.Khoroj));
					return;
				}
				ShiftDetView shiftDetView = await Shifts.GetShiftAsync(shiftDetViews, firstTaradodToday.DateTimeLogM, token);
				if (shiftDetView == null || shiftDetView.ClsShiftCoding.Vorod.TimeOfDay > new TimeSpan(0, 0, 1))
				{
					processedTaradodList.AddRange(GetCoupleTaradodEnteghali(lastTaradodYesterday, currentDate.Date, shiftEnteghaliMorning.ClsShiftCoding.Khoroj));
				}
				else if (firstTaradodToday.DateTimeLogM.TimeOfDay > shiftDetView.ClsShiftCoding.Khoroj.TimeOfDay)
				{
					processedTaradodList.AddRange(GetCoupleTaradodEnteghali(lastTaradodYesterday, currentDate.Date, shiftEnteghaliMorning.ClsShiftCoding.Khoroj));
				}
				else
				{
					processedTaradodList.AddRange(GetCoupleTaradodEnteghali(lastTaradodYesterday, currentDate.Date, firstTaradodToday.DateTimeLogM));
				}
			}
			else
			{
				if (!(todayTaradods?.Any() ?? false))
				{
					return;
				}
				ShiftDetView shiftDetView2 = await Shifts.GetShiftAsync(shiftDetViews, firstTaradodToday.DateTimeLogM, token);
				if (shiftDetView2 != null && shiftDetView2.ClsShiftCoding.NextDay && shiftDetView2.ClsShiftCoding.Vorod.TimeOfDay.TotalSeconds == 0.0)
				{
					LogOnPersonel item = new LogOnPersonel(contextGuid, shiftDetView2.ClsShiftCoding.Vorod)
					{
						IsVirtual = (SavedTaradodEnumType)6,
						TypeTaradodVacationOrMission = firstTaradodToday.TypeTaradodVacationOrMission
					};
					todayTaradods.Add(item);
				}
				if (firstTaradodToday != lastTaradodToday)
				{
					shiftDetView2 = await Shifts.GetShiftAsync(shiftDetViews, lastTaradodToday.DateTimeLogM, token);
					if (shiftDetView2 != null && shiftDetView2.ClsShiftCoding.NextDay && shiftDetView2.ClsShiftCoding.Khoroj.TimeOfDay.TotalSeconds >= 86399.0)
					{
						LogOnPersonel item2 = new LogOnPersonel(contextGuid, shiftDetView2.ClsShiftCoding.Khoroj)
						{
							IsVirtual = (SavedTaradodEnumType)4,
							TypeTaradodVacationOrMission = lastTaradodToday.TypeTaradodVacationOrMission
						};
						todayTaradods.Add(item2);
					}
				}
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
}
