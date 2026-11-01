using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Attendance.Entities;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Attendance;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.Dal.Taradod;

[Serializable]
[DoNotPruneType]
public class TaradodFull : ITaradodFull, IHasContext, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private CancellationTokenSource _updateProgressTokenSource;

	private CancellationTokenSource _updateProgressForEventTokenSource;

	private CancellationTokenSource _updateSummeryForEventTokenSource;

	private DateTime _date1;

	private DateTime _date2;

	public string ContextGuid { get; set; }

	public Person ClsPerson { get; private set; }

	public List<TaradodFullDetView> ListTaradodDets { get; set; } = new List<TaradodFullDetView>();


	public string Year { get; set; }

	public string Month { get; set; }

	public int CountAll => ListTaradodDets.Count;

	public int SumKarkerd { get; set; }

	public int SumMoazzafi { get; set; }

	public short CountKarkerd { get; set; }

	public int SumKasrkar { get; set; }

	public int SumKasrkarBegin { get; set; }

	public int SumKasrkarMid { get; set; }

	public int SumKasrkarEnd { get; set; }

	public int SumEzafekar { get; set; }

	public int SumEzafekarBegin { get; set; }

	public int SumEzafekarMid { get; set; }

	public int SumEzafekarEnd { get; set; }

	public short CountMorakhasiNotSalary { get; set; }

	public short CountMorakhasiWithSalary { get; set; }

	public short CountMorakhasi { get; set; }

	public short CountMorakhasiEstelaji { get; set; }

	public short CountMorakhasiEstehghaghi { get; set; }

	public short MissionDaily { get; set; }

	public short JarimeRoz { get; set; }

	public short CountDelayBeginShift { get; set; }

	public short CountGheybat { get; set; }

	public int GheybatMin { get; set; }

	public int SumGheybatMin { get; set; }

	public short CountTatil { get; set; }

	public short CountNaghes { get; set; }

	public short Countshift { get; set; }

	public int Sumshift { get; set; }

	public string SumShiftTime => Time.IntToTime(Sumshift);

	public DateTime DateMin
	{
		get
		{
			return _date1;
		}
		set
		{
			_date1 = value;
		}
	}

	public DateTime DateMax
	{
		get
		{
			return _date2;
		}
		set
		{
			_date2 = value;
		}
	}

	public int PersonId { get; set; }

	public long CodeTafsil { get; set; }

	public string FullName { get; set; }

	public short CountWorkdayHoghogh { get; set; }

	public short CountWorkdayBime { get; set; }

	public short CountWorkdayMaliyat { get; set; }

	public int SumWorkVacation { get; set; }

	public int SumWorkMission { get; set; }

	public int SumWorkNightWork { get; set; }

	public int SumNobatKari1 { get; set; }

	public int SumNobatKari2 { get; set; }

	public int SumNobatKari3 { get; set; }

	public int SumFriDayWork { get; set; }

	public int SumTatilDayWork { get; set; }

	public int SumJarimeRoz { get; set; }

	public string SumKarkerdTime => Time.IntToTime(SumKarkerd);

	public string SumMoazzafiTime => Time.IntToTime(SumMoazzafi);

	public string SumKasrkarTime => Time.IntToTime(SumKasrkar);

	public string SumKasrkarBeginTime => Time.IntToTime(SumKasrkarBegin);

	public string SumKasrkarMidTime => Time.IntToTime(SumKasrkarMid);

	public string SumKasrkarEndTime => Time.IntToTime(SumKasrkarEnd);

	public string SumEzafekarTime => Time.IntToTime(SumEzafekar);

	public string SumEzafekarBeginTime => Time.IntToTime(SumEzafekarBegin);

	public string SumEzafekarMidTime => Time.IntToTime(SumEzafekarMid);

	public string SumEzafekarEndTime => Time.IntToTime(SumEzafekarEnd);

	public string SumWorkVacationTime => Time.IntToTime(SumWorkVacation);

	public string SumWorkMissionTime => Time.IntToTime(SumWorkMission);

	public string SumWorkNightWorkTime => Time.IntToTime(SumWorkNightWork);

	public string SumNobatKari1Time => Time.IntToTime(SumNobatKari1);

	public string SumNobatKari2Time => Time.IntToTime(SumNobatKari2);

	public string SumNobatKari3Time => Time.IntToTime(SumNobatKari3);

	public string SumWorkHalioDayWorkTime => Time.IntToTime(SumFriDayWork);

	public string SumWorkTatilWorkTime => Time.IntToTime(SumTatilDayWork);

	public string SumMovazafiForDisplay => Time.IntToTime(SumMoazzafi) + ":00";

	public string SumKarkerdForDisplay
	{
		get
		{
			if (SumKarkerd > 0)
			{
				return Time.IntToTime(SumKarkerd) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumEzafekarForDisplay
	{
		get
		{
			if (SumEzafekar > 0)
			{
				return Time.IntToTime(SumEzafekar) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumKasrkarForDisplay
	{
		get
		{
			if (SumKasrkar > 0)
			{
				return Time.IntToTime(SumKasrkar) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumEzafekarBeginForDisplay
	{
		get
		{
			if (SumEzafekarBegin != 0)
			{
				return Time.IntToTime(SumEzafekarBegin) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumEzafekarMidForDisplay
	{
		get
		{
			if (SumEzafekarMid != 0)
			{
				return Time.IntToTime(SumEzafekarMid) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumEzafekarEndDisplay
	{
		get
		{
			if (SumEzafekarEnd != 0)
			{
				return Time.IntToTime(SumEzafekarEnd) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumKasrkarBeginForDisplay
	{
		get
		{
			if (SumKasrkarBegin != 0)
			{
				return Time.IntToTime(SumKasrkarBegin) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumKasrkarMidForDisplay
	{
		get
		{
			if (SumKasrkarMid != 0)
			{
				return Time.IntToTime(SumKasrkarMid) + ":00";
			}
			return "00:00:00";
		}
	}

	public string SumKasrkarEndDisplay
	{
		get
		{
			if (SumKasrkarEnd != 0)
			{
				return Time.IntToTime(SumKasrkarEnd) + ":00";
			}
			return "00:00:00";
		}
	}

	public bool IsDisposed => _isDisposed;

	public event Func<object, PercenteCompletedEventArgs, Task> OnProcced;

	public event Func<object, AttendanceReportSummeryEventArgs, Task> OnSummeryUpdate;

	public async Task<GharardadNew> GetClsGharardadAsync(CancellationToken token)
	{
		try
		{
			string contextGuid = ContextGuid;
			Person clsPerson = ClsPerson;
			return await GharardadNew.GetAsync(contextGuid, (clsPerson != null) ? ((PersonBase)clsPerson).Id : 0, _date1, _date2, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public TaradodFull(string contextGuid, TaradodFull pclsFull)
	{
		ContextGuid = contextGuid;
		try
		{
			ListTaradodDets = pclsFull.ListTaradodDets.ToList();
			ClsPerson = pclsFull.ClsPerson;
			_date1 = pclsFull._date1;
			_date2 = pclsFull._date2;
			Month = pclsFull.Month;
			SumKarkerd = pclsFull.SumKarkerd;
			SumMoazzafi = pclsFull.SumMoazzafi;
			CountKarkerd = pclsFull.CountKarkerd;
			SumKasrkar = pclsFull.SumKasrkar;
			SumKasrkarBegin = pclsFull.SumKasrkarBegin;
			CountDelayBeginShift = pclsFull.CountDelayBeginShift;
			SumKasrkarMid = pclsFull.SumKasrkarMid;
			SumKasrkarEnd = pclsFull.SumKasrkarEnd;
			SumEzafekar = pclsFull.SumEzafekar;
			SumEzafekarBegin = pclsFull.SumEzafekarBegin;
			SumEzafekarMid = pclsFull.SumEzafekarMid;
			SumEzafekarEnd = pclsFull.SumEzafekarEnd;
			CountMorakhasiNotSalary = pclsFull.CountMorakhasiNotSalary;
			CountMorakhasiWithSalary = pclsFull.CountMorakhasiWithSalary;
			CountMorakhasi = pclsFull.CountMorakhasi;
			CountMorakhasiEstehghaghi = pclsFull.CountMorakhasiEstehghaghi;
			CountMorakhasiEstelaji = pclsFull.CountMorakhasiEstelaji;
			MissionDaily = pclsFull.MissionDaily;
			JarimeRoz = pclsFull.JarimeRoz;
			Year = pclsFull.Year;
			CountGheybat = pclsFull.CountGheybat;
			GheybatMin = pclsFull.GheybatMin;
			CountTatil = pclsFull.CountTatil;
			CountNaghes = pclsFull.CountNaghes;
			Countshift = pclsFull.Countshift;
			Sumshift = pclsFull.Sumshift;
			DateMin = pclsFull.DateMin;
			DateMax = pclsFull.DateMax;
			PersonId = pclsFull.PersonId;
			CodeTafsil = pclsFull.CodeTafsil;
			FullName = pclsFull.FullName;
			CountWorkdayHoghogh = pclsFull.CountWorkdayHoghogh;
			CountWorkdayBime = pclsFull.CountWorkdayBime;
			CountWorkdayMaliyat = pclsFull.CountWorkdayMaliyat;
			SumWorkVacation = pclsFull.SumWorkVacation;
			SumWorkMission = pclsFull.SumWorkMission;
			SumWorkNightWork = pclsFull.SumWorkNightWork;
			SumNobatKari1 = pclsFull.SumNobatKari1;
			SumNobatKari2 = pclsFull.SumNobatKari2;
			SumNobatKari3 = pclsFull.SumNobatKari3;
			SumFriDayWork = pclsFull.SumFriDayWork;
			SumTatilDayWork = pclsFull.SumTatilDayWork;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFull.cs", 240);
		}
	}

	public TaradodFull(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<TaradodFull> CreateAsync(string contextGuid, bool calcAwaitableProperties, List<int> personIDs, DateTime date1, bool checkValidationVorodAndKHoroj, CancellationToken token)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"in {DateTime.Now} method CreateAsync ");
		try
		{
			date1 = date1.Date;
			TaradodFull taradodFull = new TaradodFull(contextGuid);
			stringBuilder.AppendLine($"line 220 in {DateTime.Now}=> date1:{date1}");
			return (!token.IsCancellationRequested) ? (await taradodFull.InitializeAsync(stringBuilder, personIDs, date1, checkValidationVorodAndKHoroj, token)) : null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFull.cs", 263);
			throw;
		}
	}

	public static async Task<TaradodFull> CreateAsync(string contextGuid, bool calcAwaitableProperties, List<Person> persons, DateTime date1, bool checkValidationVorodAndKHoroj, CancellationToken token)
	{
		try
		{
			date1 = date1.Date;
			TaradodFull taradodFull = new TaradodFull(contextGuid);
			return (!token.IsCancellationRequested) ? (await taradodFull.InitializeAsync(date1, date1, persons, null, checkValidationVorodAndKHoroj: true, token)) : null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFull.cs", 280);
			throw;
		}
	}

	public async Task<TaradodFull> InitializeAsync(StringBuilder sb, List<int> personIDs, DateTime date1, bool checkValidationVorodAndKHoroj, CancellationToken token)
	{
		_ = 1;
		try
		{
			sb.AppendLine($"in {DateTime.Now} method InitializeAsync");
			_date1 = date1;
			_date2 = date1.Date.AddDays(1.0).AddSeconds(-1.0);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			List<Person> persons = await Person.GetAllAsync(ContextGuid, personIDs, personStatus: true, token);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			await GetDetsAsync(sb, persons, checkValidationVorodAndKHoroj, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return this;
	}

	private async Task GetDetsAsync(StringBuilder sb, List<Person> persons, bool checkValidationVorodAndKHoroj, CancellationToken token)
	{
		sb.AppendLine($"in {DateTime.Now} method GetDets");
		try
		{
			int maxParalelTasks = 10;
			if (persons == null || persons.Count <= 0)
			{
				return;
			}
			TimeAttendanceSettings settings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			List<AttPersonLeaveBusiness> allPersonLeaves = (await AttPersonLeaveBusiness.GetAllAsync(ContextGuid, default(CancellationToken), "")).ToList();
			List<LogOnPersonel> allLogs = await LogOnPersonel.GetAllAsync(ContextGuid, _date1.Date, _date1.Date.AddDays(1.0), token);
			List<Morakhasidaily> allMorakhasidailies = await Morakhasidaily.GetSpecificDataAsync(ContextGuid, persons.Select((Person x) => ((PersonBase)x).Id)?.ToList() ?? new List<int>(), _date1.AddDays(-1.0), _date1.Date.AddDays(1.0), "", token);
			List<ATT_TaradodDet> allAtt_TaradodDets = await ATT_TaradodDet.GetAllAsync(ContextGuid, _date1.Date, _date1.Date.AddDays(1.0), token);
			Holiday holiday = await Holiday.GetAsync(ContextGuid, _date1, token);
			List<DeviceInfo> allDeviceInfo = await DeviceInfo.GetAllAsync(ContextGuid, token);
			ListTaradodDets = new List<TaradodFullDetView>();
			List<Task<TaradodFullDetView>> tasks = new List<Task<TaradodFullDetView>>();
			foreach (Person person in persons)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				GharardadNew gharardad = await GharardadNew.GetAsync(ContextGuid, ((PersonBase)person).Id, _date1, token);
				List<ShiftDetView> shiftsPerson = new List<ShiftDetView>();
				if (gharardad != null)
				{
					List<ShiftDetView> list = (await ShiftDetView.GetShiftDetViewOfDateAsync(ContextGuid, _date1, gharardad.ID, token))?.Where((ShiftDetView p) => p.ShiftId == gharardad.ShiftId)?.ToList();
					foreach (ShiftDetView item2 in list)
					{
						ShiftDetView shiftDetView = await ShiftDetView.Clone_ShiftDetViewAsync(ContextGuid, item2, _date1, item2.ClsShiftCoding, token);
						if (shiftDetView != null)
						{
							shiftsPerson.Add(shiftDetView);
						}
					}
				}
				sb.AppendLine($"line 258 in {DateTime.Now} =>_date1:{_date1} ");
				if (token.IsCancellationRequested)
				{
					return;
				}
				List<Morakhasidaily> personMorakhasi = allMorakhasidailies?.Where((Morakhasidaily p) => p.PersonId == ((PersonBase)person).Id)?.ToList();
				List<AttPersonLeaveBusiness> personLeaves = allPersonLeaves?.Where((AttPersonLeaveBusiness p) => p.PersonGuid == ((PersonBase)person).Guid)?.ToList();
				tasks.Add(Task.Run(() => TaradodFullDet.GetAsync_Improved(ContextGuid, settings, personLeaves, personMorakhasi, allLogs, allAtt_TaradodDets, shiftsPerson, allDeviceInfo, holiday, _date1, person, gharardad, checkValidationVorodAndKHoroj, token)));
				if (tasks.Count >= maxParalelTasks)
				{
					Task<TaradodFullDetView> t = await Task.WhenAny(tasks);
					TaradodFullDetView item = await t;
					ListTaradodDets.Add(item);
					tasks.Remove(t);
				}
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			List<TaradodFullDetView> listTaradodDets = ListTaradodDets;
			listTaradodDets.AddRange(await Task.WhenAll(tasks));
			if (!token.IsCancellationRequested)
			{
				ListTaradodDets = ListTaradodDets.OrderBy((TaradodFullDetView q) => q.Family + " " + q.FirstName).ToList();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static Task<TaradodFull> CreateAsync(string contextGuid, DateTime pDate1, DateTime pDate2, Person pperson, TaskCompletionSource<bool> notifyCompleted, bool checkValidationVorodAndKHoroj, CancellationToken token)
	{
		return new TaradodFull(contextGuid).InitializeAsync(pDate1, pDate2, pperson, notifyCompleted, checkValidationVorodAndKHoroj, token);
	}

	public async Task<TaradodFull> InitializeAsync(DateTime pDate1, DateTime pDate2, Person pperson, TaskCompletionSource<bool> notifyCompleted, bool checkValidationVorodAndKHoroj, CancellationToken token)
	{
		_ = 11;
		try
		{
			if (token.IsCancellationRequested)
			{
				return this;
			}
			_date1 = pDate1.Date;
			_date2 = pDate2.Date;
			DateTime startDate = _date1.Date;
			DateTime endDate = _date2.Date.AddDays(1.0).AddSeconds(-1.0);
			ClsPerson = pperson;
			new List<Task<TaradodFullDetView>>();
			if (ClsPerson == null || _date1 > _date2)
			{
				return this;
			}
			Year = Utilities.YearOfDateCalendar(Calendar.MiladiToCalture(pDate1)).ToString();
			Month = Utilities.MonthOfDateCalendar(Calendar.MiladiToCalture(pDate1)).ToString();
			await LogOnPersonel.DeleteAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id, startDate.AddSeconds(5.0), endDate.AddDays(1.0), (SavedTaradodEnumType)6, token);
			await LogOnPersonel.DeleteAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id, startDate.AddSeconds(5.0), endDate.AddDays(1.0), (SavedTaradodEnumType)4, token);
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			IEnumerable<AttPersonLeaveBusiness> allPersonLeaves = await AttPersonLeaveBusiness.GetAllAsync(ContextGuid, default(CancellationToken), "");
			List<Morakhasidaily> allMorakhasiDaily = await Morakhasidaily.GetAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id);
			List<ATT_TaradodDet> allAttTaradodDet = await ATT_TaradodDet.GetAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id, _date1, _date2, token);
			List<GharardadNew> allGharardads = (await GharardadNew.GetAllAsync(ContextGuid, new List<int>(1) { ((PersonBase)ClsPerson).Id }, pDate1, pDate2, token))?.ToList();
			GharardadNew clsGharardad = ((allGharardads != null && allGharardads.Count() == 1) ? allGharardads[0] : allGharardads.FirstOrDefault((GharardadNew p) => p.DateStartM <= _date1 && _date1 <= p.DateFinishM));
			List<Holiday> allHoliday = await Holiday.GetAllAsync(ContextGuid, _date1, _date2, isHolidayOnly: true);
			List<LogOnPersonel> allLogonpersonels = await LogOnPersonel.GetAllByPersonIdAsync(ContextGuid, ((PersonBase)ClsPerson).Id, _date1.AddDays(-1.0).Date, _date2.Date.AddDays(2.0).AddSeconds(-1.0), token);
			Dictionary<DateTime, List<LogOnPersonel>> logonPersonelsDictionary = GetLogonPersonelsDictionary(startDate.AddDays(-1.0), endDate.AddDays(1.0), allLogonpersonels);
			(from p in allPersonLeaves
				where p.PersonGuid == ((PersonBase)ClsPerson).Guid
				orderby p.DateFrom descending
				select p).ToList();
			new Dictionary<DateTime, List<ShiftDetView>>();
			Dictionary<DateTime, List<ShiftDetView>> ShiftDetViewsDictionary = await GetShiftDetViewsDictionary(startDate.AddDays(-1.0), endDate.AddDays(1.0), setting, allMorakhasiDaily, clsGharardad, token);
			List<DeviceInfo> allDeviceInfo = await DeviceInfo.GetAllAsync(ContextGuid, token);
			DateTime curDate = startDate;
			while (curDate <= endDate)
			{
				if (clsGharardad == null || clsGharardad.DateFinishM < curDate.Date)
				{
					clsGharardad = allGharardads.FirstOrDefault((GharardadNew p) => p.DateStartM.Date <= curDate.Date && curDate.Date <= p.DateFinishM.Date);
				}
				if (token.IsCancellationRequested)
				{
					return this;
				}
				List<AttPersonLeaveBusiness> allPersonLeaveBusiness = allPersonLeaves.Where((AttPersonLeaveBusiness p) => p.PersonGuid == ((PersonBase)ClsPerson).Guid && (int)p.Type == 0 && ((int)p.LeaveStatus == 0 || (int)p.LeaveStatus == 2) && p.DateFrom.Date <= curDate.Date && curDate.Date <= p.DateTo.Date).ToList();
				Morakhasidaily morakhasidailyCurrentDate = allMorakhasiDaily?.SingleOrDefault((Morakhasidaily p) => p.DateFrom.Date == curDate.Date && p.PersonId == ((PersonBase)ClsPerson).Id);
				Morakhasidaily morakhasidailyNextDate = allMorakhasiDaily?.SingleOrDefault((Morakhasidaily p) => p.DateFrom.Date == curDate.Date.AddDays(1.0) && p.PersonId == ((PersonBase)ClsPerson).Id);
				ATT_TaradodDet aTT_TaradodDetCurrentDate = allAttTaradodDet?.SingleOrDefault((ATT_TaradodDet p) => p.DateM.Date == curDate.Date);
				Holiday holidayCurrentDay = allHoliday.SingleOrDefault((Holiday p) => p.DateM.Date == curDate.Date);
				ShiftDetViewsDictionary.TryGetValue(curDate.Date, out var value);
				TaradodFullDetView item = await TaradodFullDet.GetAsyncNew(ContextGuid, setting, curDate, checkValidationVorodAndKHoroj, allPersonLeaveBusiness, ClsPerson, clsGharardad, morakhasidailyCurrentDate, morakhasidailyNextDate, aTT_TaradodDetCurrentDate, allDeviceInfo, logonPersonelsDictionary, holidayCurrentDay, value, token);
				ListTaradodDets.Add(item);
				curDate = curDate.AddDays(1.0);
			}
			if (token.IsCancellationRequested)
			{
				return this;
			}
			ListTaradodDets = ListTaradodDets.OrderBy((TaradodFullDetView q) => q.DateM).ToList();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			notifyCompleted?.TrySetResult(result: true);
		}
		return this;
	}

	public async Task<TaradodFull> InitializeAsync(DateTime pDate1, DateTime pDate2, List<Person> persons, TaskCompletionSource<bool> notifyCompleted, bool checkValidationVorodAndKHoroj, CancellationToken token)
	{
		_ = 11;
		try
		{
			if (token.IsCancellationRequested)
			{
				return this;
			}
			_date1 = pDate1.Date;
			_date2 = pDate2.Date;
			DateTime startDate = _date1.Date;
			DateTime endDate = _date2.Date.AddDays(1.0).AddSeconds(-1.0);
			foreach (Person person in persons)
			{
				ClsPerson = person;
				new List<Task<TaradodFullDetView>>();
				if (ClsPerson == null || _date1 > _date2)
				{
					return this;
				}
				Year = Utilities.YearOfDateCalendar(Calendar.MiladiToCalture(pDate1)).ToString();
				Month = Utilities.MonthOfDateCalendar(Calendar.MiladiToCalture(pDate1)).ToString();
				await LogOnPersonel.DeleteAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id, startDate.AddSeconds(5.0), endDate.AddDays(1.0), (SavedTaradodEnumType)6, token);
				await LogOnPersonel.DeleteAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id, startDate.AddSeconds(5.0), endDate.AddDays(1.0), (SavedTaradodEnumType)4, token);
				TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
				IEnumerable<AttPersonLeaveBusiness> allPersonLeaves = await AttPersonLeaveBusiness.GetAllAsync(ContextGuid, default(CancellationToken), "");
				List<Morakhasidaily> allMorakhasiDaily = await Morakhasidaily.GetAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id);
				List<ATT_TaradodDet> allAttTaradodDet = await ATT_TaradodDet.GetAllAsync(ContextGuid, ((PersonBase)ClsPerson).Id, _date1, _date2, token);
				List<GharardadNew> allGharardads = (await GharardadNew.GetAllAsync(ContextGuid, new List<int>(1) { ((PersonBase)ClsPerson).Id }, pDate1, pDate2, token))?.ToList();
				GharardadNew clsGharardad = ((allGharardads != null && allGharardads.Count() == 1) ? allGharardads[0] : allGharardads.FirstOrDefault((GharardadNew p) => p.DateStartM <= _date1 && _date1 <= p.DateFinishM));
				List<Holiday> allHoliday = await Holiday.GetAllAsync(ContextGuid, _date1, _date2, isHolidayOnly: true);
				List<LogOnPersonel> allLogonpersonels = await LogOnPersonel.GetAllByPersonIdAsync(ContextGuid, ((PersonBase)ClsPerson).Id, _date1.AddDays(-1.0).Date, _date2.Date.AddDays(2.0).AddSeconds(-1.0), token);
				Dictionary<DateTime, List<LogOnPersonel>> logonPersonelsDictionary = GetLogonPersonelsDictionary(startDate.AddDays(-1.0), endDate.AddDays(1.0), allLogonpersonels);
				(from p in allPersonLeaves
					where p.PersonGuid == ((PersonBase)ClsPerson).Guid
					orderby p.DateFrom descending
					select p).ToList();
				new Dictionary<DateTime, List<ShiftDetView>>();
				Dictionary<DateTime, List<ShiftDetView>> ShiftDetViewsDictionary = await GetShiftDetViewsDictionary(startDate.AddDays(-1.0), endDate.AddDays(1.0), setting, allMorakhasiDaily, clsGharardad, token);
				List<DeviceInfo> allDeviceInfo = await DeviceInfo.GetAllAsync(ContextGuid, token);
				DateTime curDate = startDate;
				while (curDate <= endDate)
				{
					if (clsGharardad == null || clsGharardad.DateFinishM < curDate.Date)
					{
						clsGharardad = allGharardads.FirstOrDefault((GharardadNew p) => p.DateStartM.Date <= curDate.Date && curDate.Date <= p.DateFinishM.Date);
					}
					if (token.IsCancellationRequested)
					{
						return this;
					}
					List<AttPersonLeaveBusiness> allPersonLeaveBusiness = allPersonLeaves.Where((AttPersonLeaveBusiness p) => p.PersonGuid == ((PersonBase)ClsPerson).Guid && (int)p.Type == 0 && ((int)p.LeaveStatus == 0 || (int)p.LeaveStatus == 2) && p.DateFrom.Date <= curDate.Date && curDate.Date <= p.DateTo.Date).ToList();
					Morakhasidaily morakhasidailyCurrentDate = allMorakhasiDaily?.SingleOrDefault((Morakhasidaily p) => p.DateFrom.Date == curDate.Date && p.PersonId == ((PersonBase)ClsPerson).Id);
					Morakhasidaily morakhasidailyNextDate = allMorakhasiDaily?.SingleOrDefault((Morakhasidaily p) => p.DateFrom.Date == curDate.Date.AddDays(1.0) && p.PersonId == ((PersonBase)ClsPerson).Id);
					ATT_TaradodDet aTT_TaradodDetCurrentDate = allAttTaradodDet?.SingleOrDefault((ATT_TaradodDet p) => p.DateM.Date == curDate.Date);
					Holiday holidayCurrentDay = allHoliday.SingleOrDefault((Holiday p) => p.DateM.Date == curDate.Date);
					ShiftDetViewsDictionary.TryGetValue(curDate.Date, out var value);
					TaradodFullDetView item = await TaradodFullDet.GetAsyncNew(ContextGuid, setting, curDate, checkValidationVorodAndKHoroj, allPersonLeaveBusiness, ClsPerson, clsGharardad, morakhasidailyCurrentDate, morakhasidailyNextDate, aTT_TaradodDetCurrentDate, allDeviceInfo, logonPersonelsDictionary, holidayCurrentDay, value, token);
					ListTaradodDets.Add(item);
					curDate = curDate.AddDays(1.0);
				}
			}
			if (token.IsCancellationRequested)
			{
				return this;
			}
			ListTaradodDets = ListTaradodDets.OrderBy((TaradodFullDetView q) => q.Family + " " + q.FirstName).ToList();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			notifyCompleted?.TrySetResult(result: true);
		}
		return this;
	}

	private Dictionary<DateTime, List<LogOnPersonel>> GetLogonPersonelsDictionary(DateTime fromDate, DateTime toDate, List<LogOnPersonel> allLogonpersonels)
	{
		Dictionary<DateTime, List<LogOnPersonel>> dictionary = new Dictionary<DateTime, List<LogOnPersonel>>();
		try
		{
			List<LogOnPersonel> list = new List<LogOnPersonel>();
			DateTime curDate = fromDate.Date;
			while (curDate <= toDate.Date)
			{
				list = (from p in allLogonpersonels
					where p.DateTimeLogM.Date == curDate
					orderby p.DateTimeLogM
					select p).ToList();
				if (list == null)
				{
					list = new List<LogOnPersonel>();
				}
				dictionary.Add(curDate, list);
				curDate = curDate.AddDays(1.0);
			}
			return dictionary;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task<Dictionary<DateTime, List<ShiftDetView>>> GetShiftDetViewsDictionary(DateTime fromDate, DateTime toDate, TimeAttendanceSettings setting, List<Morakhasidaily> allMorakhasiDaily, GharardadNew clsGharardad, CancellationToken token)
	{
		Dictionary<DateTime, List<ShiftDetView>> dic = new Dictionary<DateTime, List<ShiftDetView>>();
		try
		{
			if (clsGharardad != null)
			{
				new List<ShiftDetView>();
				DateTime curDate = fromDate;
				while (curDate <= toDate)
				{
					if (clsGharardad == null || clsGharardad.DateFinishM < curDate)
					{
						clsGharardad = await GharardadNew.GetAsync(ContextGuid, ((PersonBase)ClsPerson).Id, curDate, token);
					}
					if (clsGharardad == null)
					{
						curDate = curDate.AddDays(1.0);
						continue;
					}
					List<ShiftDetView> ShiftDetViews = await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, curDate, clsGharardad.ID, ((PersonBase)ClsPerson).Id, token);
					if (ShiftDetViews == null || ShiftDetViews.Count() == 0)
					{
						curDate = curDate.AddDays(1.0);
						continue;
					}
					bool wasOnVacationYeserday = allMorakhasiDaily?.Any((Morakhasidaily p) => p.PersonId == ((PersonBase)ClsPerson).Id && p.DateFrom.Date == curDate.AddDays(-1.0).Date) ?? false;
					bool hasVacationToday = allMorakhasiDaily?.Any((Morakhasidaily p) => p.PersonId == ((PersonBase)ClsPerson).Id && p.DateFrom.Date == curDate.Date) ?? false;
					dic.TryGetValue(curDate.AddDays(-1.0), out var value);
					await TaradodFullDet.FixShiftCodings(ContextGuid, wasOnVacationYeserday, hasVacationToday, setting.CalcEkWhenHasVacationOrMission, ShiftDetViews, value, token);
					dic.Add(curDate.Date, ShiftDetViews);
					curDate = curDate.AddDays(1.0);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return dic;
	}

	public async Task FillTaradodFullPropertysAsync(CancellationToken token)
	{
		_ = 4;
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			string text = Calendar.MiladiToCalture(_date1);
			Year = Utilities.YearOfDateCalendar(text).ToString();
			Month = LanguageAndCalendar.Instance.CaltureCalender.GetMonthName(text);
			TaradodFull taradodFull = this;
			TaradodFull taradodFull2 = this;
			int num2 = (SumMoazzafi = 0);
			int sumshift = (taradodFull2.SumKarkerd = num2);
			taradodFull.Sumshift = sumshift;
			TaradodFull taradodFull3 = this;
			TaradodFull taradodFull4 = this;
			num2 = (SumKasrkarEnd = 0);
			sumshift = (taradodFull4.SumKasrkarMid = num2);
			taradodFull3.SumKasrkarBegin = sumshift;
			TaradodFull taradodFull5 = this;
			TaradodFull taradodFull6 = this;
			num2 = (SumEzafekarEnd = 0);
			sumshift = (taradodFull6.SumEzafekarMid = num2);
			taradodFull5.SumEzafekarBegin = sumshift;
			CountDelayBeginShift = 0;
			TaradodFull taradodFull7 = this;
			short countMorakhasiNotSalary = (CountKarkerd = 0);
			taradodFull7.CountMorakhasiNotSalary = countMorakhasiNotSalary;
			TaradodFull taradodFull8 = this;
			TaradodFull taradodFull9 = this;
			TaradodFull taradodFull10 = this;
			TaradodFull taradodFull11 = this;
			TaradodFull taradodFull12 = this;
			TaradodFull taradodFull13 = this;
			TaradodFull taradodFull14 = this;
			TaradodFull taradodFull15 = this;
			TaradodFull taradodFull16 = this;
			TaradodFull taradodFull17 = this;
			short num10 = (CountMorakhasiEstelaji = 0);
			short num12 = (taradodFull17.CountMorakhasiEstehghaghi = num10);
			short num14 = (taradodFull16.CountMorakhasi = num12);
			short num16 = (taradodFull15.CountMorakhasiWithSalary = num14);
			short num18 = (taradodFull14.CountGheybat = num16);
			short num20 = (taradodFull13.CountTatil = num18);
			short num22 = (taradodFull12.CountNaghes = num20);
			short num24 = (taradodFull11.Countshift = num22);
			short num26 = (taradodFull10.CountWorkdayHoghogh = num24);
			countMorakhasiNotSalary = (taradodFull9.CountWorkdayBime = num26);
			taradodFull8.CountWorkdayMaliyat = countMorakhasiNotSalary;
			MissionDaily = 0;
			JarimeRoz = 0;
			DateMin = DateTime.MinValue;
			DateMax = DateTime.MaxValue;
			TaradodFull taradodFull18 = this;
			TaradodFull taradodFull19 = this;
			TaradodFull taradodFull20 = this;
			TaradodFull taradodFull21 = this;
			TaradodFull taradodFull22 = this;
			TaradodFull taradodFull23 = this;
			TaradodFull taradodFull24 = this;
			int num29 = (SumNobatKari3 = 0);
			int num31 = (taradodFull24.SumNobatKari2 = num29);
			int num33 = (taradodFull23.SumNobatKari1 = num31);
			int num35 = (taradodFull22.SumWorkNightWork = num33);
			int num37 = (taradodFull21.SumWorkMission = num35);
			num2 = (taradodFull20.SumWorkVacation = num37);
			sumshift = (taradodFull19.SumTatilDayWork = num2);
			taradodFull18.SumFriDayWork = sumshift;
			TaradodFull taradodFull25 = this;
			sumshift = (SumEzafekar = 0);
			taradodFull25.SumKasrkar = sumshift;
			if (token.IsCancellationRequested)
			{
				return;
			}
			if (ClsPerson != null)
			{
				PersonId = ((PersonBase)ClsPerson).Id;
				CodeTafsil = ((PersonBase)ClsPerson).CodeTafsil;
				FullName = Text.FixString(((PersonBase)ClsPerson).FullName);
			}
			else
			{
				CodeTafsil = 0L;
				FullName = "";
			}
			if (ListTaradodDets == null || ListTaradodDets.Count == 0)
			{
				return;
			}
			TaradodFull taradodFull26 = this;
			DateTime dateMin = (DateMax = ListTaradodDets[0].DateM);
			taradodFull26.DateMin = dateMin;
			foreach (TaradodFullDetView item in ListTaradodDets)
			{
				int karkard = await item.GetKarkardAsync(token);
				Morakhasidaily morakhasi = item.Morakhasi;
				bool isWorkDayHoghogh = item.IsWorkday((EnWorkdayEnumType)1);
				int sumShiftMin = item.SumShiftMinuts;
				StatuseTaradodEnumType statusTaradod = await item.GetStatuseTaradodAsync(token);
				if (token.IsCancellationRequested)
				{
					return;
				}
				SumKarkerd += karkard;
				if (karkard > 0 && sumShiftMin > 0)
				{
					SumMoazzafi += item.GetSumMoazafiMinutes();
				}
				int sumKasrkar = SumKasrkar;
				SumKasrkar = sumKasrkar + await item.GetKasrKarAsync(token);
				SumKasrkarBegin += item.KasreKarBeginShift;
				SumKasrkarMid += item.KasreKarMidShift;
				SumKasrkarEnd += item.KasreKarEndShift;
				sumKasrkar = SumEzafekar;
				SumEzafekar = sumKasrkar + await item.GetEzafeKarAsync(token);
				SumEzafekarBegin += item.EzafeKarBeginShift;
				SumEzafekarMid += item.EzafeKarMidShift;
				SumEzafekarEnd += item.EzafeKarEndShift;
				SumWorkVacation += item.WorkOnTypeTaradod((TypeTaradodEnumType)5);
				SumWorkMission += item.WorkOnTypeTaradod((TypeTaradodEnumType)7);
				SumWorkNightWork += item.WorkOnTypeTaradod((TypeTaradodEnumType)10);
				SumNobatKari1 += item.WorkOnTypeTaradod((TypeTaradodEnumType)91);
				SumNobatKari2 += item.WorkOnTypeTaradod((TypeTaradodEnumType)92);
				SumNobatKari3 += item.WorkOnTypeTaradod((TypeTaradodEnumType)93);
				SumFriDayWork += item.WorkOnTypeTaradod((TypeTaradodEnumType)8);
				SumTatilDayWork += item.WorkOnTypeTaradod((TypeTaradodEnumType)28);
				if (karkard > 0 && isWorkDayHoghogh)
				{
					CountKarkerd++;
				}
				if (morakhasi != null && !morakhasi.HasHoghogh)
				{
					CountMorakhasiNotSalary++;
				}
				if (morakhasi != null && morakhasi.HasHoghogh)
				{
					CountMorakhasiWithSalary++;
				}
				if (morakhasi != null)
				{
					EnPersonLeaveKind kind = morakhasi.Kind;
					switch (kind - 1)
					{
					case 0:
						CountMorakhasiEstelaji++;
						break;
					case 1:
						CountMorakhasiEstehghaghi++;
						break;
					case 4:
						MissionDaily++;
						break;
					}
					if (morakhasi.HasHoghogh && (int)morakhasi.Kind != 5)
					{
						CountMorakhasi++;
					}
				}
				if (item.JarimeRoz > 0)
				{
					JarimeRoz++;
				}
				if (item.KasreKarBeginShift > 0)
				{
					CountDelayBeginShift++;
				}
				StatuseTaradodEnumType val = statusTaradod;
				if (((int)val == 3 || val - 12 <= 1) ? true : false)
				{
					CountGheybat++;
					sumKasrkar = SumGheybatMin;
					SumGheybatMin = sumKasrkar + await item.GetGheybatMinAsync(token);
				}
				if ((int)statusTaradod == 7 || (int)statusTaradod == 6)
				{
					CountTatil++;
				}
				if ((int)statusTaradod == 2)
				{
					CountNaghes++;
				}
				if (sumShiftMin > 0)
				{
					Countshift++;
					Sumshift += sumShiftMin;
				}
				if (item.DateM < DateMin)
				{
					DateMin = item.DateM;
				}
				if (item.DateM > DateMax)
				{
					DateMax = item.DateM;
				}
				if (item.DateM.Date <= DateTime.Now.Date && isWorkDayHoghogh)
				{
					CountWorkdayHoghogh++;
				}
				if (item.DateM.Date <= DateTime.Now.Date && item.IsWorkday((EnWorkdayEnumType)0))
				{
					CountWorkdayBime++;
				}
				if (item.DateM.Date <= DateTime.Now.Date && item.IsWorkday((EnWorkdayEnumType)2))
				{
					CountWorkdayMaliyat++;
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FillTaradodFullPropertysAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFull.cs", 787);
			throw;
		}
	}

	public async Task<TaradodFull> SetConditionAsync(List<StatuseTaradodEnumType> statusTaradod, CancellationToken token = default(CancellationToken))
	{
		TaradodFull clsTemp = new TaradodFull(ContextGuid, this);
		try
		{
			if (statusTaradod.Count > 0)
			{
				IEnumerable<StatuseTaradodEnumType> query = statusTaradod.Where((StatuseTaradodEnumType q) => (int)q != 9 && (int)q != 17 && (int)q != 16 && (int)q != 15 && (int)q != 4);
				List<TaradodFullDetView> res = new List<TaradodFullDetView>();
				foreach (TaradodFullDetView item2 in ListTaradodDets)
				{
					if (query.Contains(await item2.GetStatuseTaradodAsync(token)))
					{
						res.Add(item2);
					}
				}
				List<TaradodFullDetView> second = ListTaradodDets.Where((TaradodFullDetView p) => p.CoupleTaradodReadOnly.Any((CoupleTaradod c) => c.StatuseTaradodLst.Any((StatuseTaradodEnumType c3) => statusTaradod.Any((StatuseTaradodEnumType c2) => c2 == c3)))).ToList();
				clsTemp.ListTaradodDets = res.Union(second).ToList();
				if (statusTaradod.Contains((StatuseTaradodEnumType)4))
				{
					List<TaradodFullDetView> list = null;
					List<TaradodFullDetView> list2 = null;
					List<TaradodFullDetView> list3 = null;
					if (statusTaradod.Contains((StatuseTaradodEnumType)2))
					{
						list = ListTaradodDets.Where((TaradodFullDetView p) => p.TaradodsCurrentDay.Count % 2 == 1).ToList();
					}
					if (statusTaradod.Contains((StatuseTaradodEnumType)1))
					{
						list2 = ListTaradodDets.Where((TaradodFullDetView p) => p.TaradodsCurrentDay.Count > 0 && p.TaradodsCurrentDay.Count % 2 == 0).ToList();
					}
					if (list != null)
					{
						clsTemp.ListTaradodDets = clsTemp.ListTaradodDets.Union(list).ToList();
					}
					if (list2 != null)
					{
						clsTemp.ListTaradodDets = clsTemp.ListTaradodDets.Union(list2).ToList();
					}
					if (list == null && list2 == null)
					{
						List<TaradodFullDetView> resultList = new List<TaradodFullDetView>();
						foreach (TaradodFullDetView item2 in ListTaradodDets)
						{
							if ((int)(await item2.GetStatuseTaradodAsync(token)) == 4)
							{
								resultList.Add(item2);
							}
						}
						list3 = resultList.ToList();
					}
					if (list3 != null)
					{
						clsTemp.ListTaradodDets = clsTemp.ListTaradodDets.Union(list3).ToList();
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SetConditionAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFull.cs", 851);
			throw;
		}
		return clsTemp;
	}

	public void Dispose()
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			_updateProgressTokenSource?.Cancel();
			_updateProgressTokenSource?.Dispose();
			_updateProgressTokenSource = null;
			_updateProgressForEventTokenSource?.Cancel();
			_updateProgressForEventTokenSource?.Dispose();
			_updateProgressForEventTokenSource = null;
			_updateSummeryForEventTokenSource?.Cancel();
			_updateSummeryForEventTokenSource?.Dispose();
			_updateSummeryForEventTokenSource = null;
			List<TaradodFullDetView> listTaradodDets = ListTaradodDets;
			if (listTaradodDets != null && listTaradodDets.Any())
			{
				foreach (TaradodFullDetView listTaradodDet in ListTaradodDets)
				{
					listTaradodDet?.Dispose();
				}
			}
			ListTaradodDets = null;
			this.OnProcced = null;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public async Task CalcSummeryTaradodFullDataAsync(CancellationToken token)
	{
		try
		{
			if (!token.IsCancellationRequested)
			{
				await FillTaradodFullPropertysAsync(token);
				if (this.OnSummeryUpdate != null)
				{
					_updateSummeryForEventTokenSource?.Cancel();
					_updateSummeryForEventTokenSource = new CancellationTokenSource();
					CancellationToken token2 = _updateSummeryForEventTokenSource.Token;
					CancellationToken token3 = CancellationTokenSource.CreateLinkedTokenSource(token, token2).Token;
					AttendanceReportSummeryEventArgs val = new AttendanceReportSummeryEventArgs(token3);
					Utilities.RaiseEvent<AttendanceReportSummeryEventArgs>(this.OnSummeryUpdate, (object)this, val, token3, 60, "CalcSummeryTaradodFullDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFull.cs");
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
