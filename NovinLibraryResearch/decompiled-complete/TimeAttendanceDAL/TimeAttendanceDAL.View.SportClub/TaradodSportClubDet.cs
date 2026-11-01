using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.View.SportClub;

public class TaradodSportClubDet : IHasContext
{
	private GharardadNew _gharadad;

	private List<ShiftDetView> _shiftDetViewsSrc;

	private Color _colorKk = Color.Black;

	private readonly CoupleTaradod[] _coupleTaradod = new CoupleTaradod[8];

	private short _dayofShMonth;

	private int? _karkard;

	private int? _sumShiftMinuts;

	private StatuseTaradodEnumType _statuseTaradod;

	public string ContextGuid { get; set; }

	private Person _person11 { get; set; }

	public long CodeTafsil => ((PersonBase)Person()).CodeTafsil;

	public string FullName => ((PersonBase)Person()).FullName;

	public DateTime DateM { get; private set; }

	public string Dayofweek => LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(DateM);

	public bool CanEditV => UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodMain;

	public bool CanEditK => UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodMain;

	public Color ColorEk { get; private set; } = Color.Black;


	public string DateSh => Calendar.MiladiToCalture(DateM);

	public short DayofShMonth
	{
		get
		{
			_dayofShMonth = (short)Calendar.GetDayOfDateSh(DateSh);
			return _dayofShMonth;
		}
	}

	public async Task<List<ShiftDetView>> GetShiftDetViewsAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (_shiftDetViewsSrc != null)
			{
				return _shiftDetViewsSrc;
			}
			GharardadNew gharardadNew = await GetGharadadAsync(token);
			if (gharardadNew == null)
			{
				return new List<ShiftDetView>();
			}
			_shiftDetViewsSrc = await ShiftDetView.GetShiftDetViewListAsync(ContextGuid, DateM, gharardadNew.ID, gharardadNew.PersonId, token);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetShiftDetViewsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 44);
			throw;
		}
		return new List<ShiftDetView>();
	}

	public async Task<GharardadNew> GetGharadadAsync(CancellationToken token)
	{
		try
		{
			GharardadNew gharardadNew = _gharadad;
			if (gharardadNew == null)
			{
				gharardadNew = (_gharadad = await GharardadNew.GetAsync(ContextGuid, ((PersonBase)_person11).Id, DateM, token));
			}
			return gharardadNew;
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

	public async Task<string> GetHozorTimeAsync(CancellationToken token = default(CancellationToken))
	{
		return Time.IntToTime(await GetKarkardAsync(token));
	}

	public async Task<string> GetShiftTimeAsync(CancellationToken token = default(CancellationToken))
	{
		return Time.IntToTime(await GetSumShiftMinutsAsync(token));
	}

	public async Task<string> GetNameStatuseTaradodAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			StatuseTaradodEnumType val = await GetStatuseTaradodAsync(token);
			if ((int)val == 0)
			{
				return "";
			}
			if (((int)val == 4 || (int)val == 8) ? true : false)
			{
				return "فاقد پکیج";
			}
			if ((val - 1 <= 1 || val - 15 <= 1) ? true : false)
			{
				return "حضور در باشگاه";
			}
			if (((int)val == 3 || (int)val == 13 || (int)val == 18) ? true : false)
			{
				return "عدم حضور در باشگاه";
			}
			return "";
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetNameStatuseTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 139);
			throw;
		}
	}

	public async Task<string> GetShiftToolTipAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetShiftToolTipAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 185);
			return "error!";
		}
	}

	public CoupleTaradod CoupleTaradod(int index)
	{
		if (index >= 0 && index < 8)
		{
			return _coupleTaradod[index];
		}
		return new CoupleTaradod(ContextGuid, DateM);
	}

	public async Task<int> GetKarkardAsync(CancellationToken token = default(CancellationToken))
	{
		if (!_karkard.HasValue)
		{
			_karkard = await GetCalcKarkerdAsync(token);
		}
		return _karkard.Value;
	}

	private async Task<int> GetCalcKarkerdAsync(CancellationToken token = default(CancellationToken))
	{
		int xk = 0;
		try
		{
			List<ShiftDetView> shift = await GetShiftDetViewsAsync(token);
			for (int i = 0; i < 8; i++)
			{
				token.ThrowIfCancellationRequested();
				CoupleTaradod coupleTaradod = CoupleTaradod(i);
				int num = xk;
				xk = num + await coupleTaradod.KarkardAsync(shift, token);
			}
			return xk;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetCalcKarkerdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 228);
			throw;
		}
	}

	public int WorkOnTypeTaradod(TypeTaradodEnumType ptype)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				num += CoupleTaradod(i).TimeWorkOnTypeTaradod(ptype);
			}
			return num;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "WorkOnTypeTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 247);
			throw;
		}
	}

	public async Task<int> GetSumShiftMinutsAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!_sumShiftMinuts.HasValue)
			{
				_sumShiftMinuts = await CalcShiftAsync(token);
			}
			return _sumShiftMinuts.Value;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetSumShiftMinutsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 263);
			throw;
		}
	}

	private async Task<int> CalcShiftAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CalcShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 298);
			throw;
		}
	}

	public static async Task<TaradodSportClubDet> GetAsync(string contextGuid, DateTime pdate, Person pclsPerson, bool getTarardodOnly = false, CancellationToken token = default(CancellationToken))
	{
		TaradodSportClubDet ret = new TaradodSportClubDet
		{
			ContextGuid = contextGuid,
			DateM = pdate,
			_person11 = pclsPerson
		};
		try
		{
			for (int i = 0; i < 8; i++)
			{
				ret._coupleTaradod[i] = new CoupleTaradod(ret.ContextGuid, ret.DateM);
				ret.CoupleTaradod(i).Vtaradod = new LogOnPersonel(ret.ContextGuid, ret.DateM.Date);
				ret.CoupleTaradod(i).Ktaradod = new LogOnPersonel(ret.ContextGuid, ret.DateM.Date);
				ret.CoupleTaradod(i).PersonId = ((PersonBase)ret._person11).Id;
				ret.CoupleTaradod(i).DateM = ret.DateM;
			}
			await ret.CalcTaradodAsync(ret.DateM, ((PersonBase)ret._person11).Id);
			if (!getTarardodOnly)
			{
				CoupleTaradod[] coupleTaradod = ret._coupleTaradod;
				for (int j = 0; j < coupleTaradod.Length; j++)
				{
					coupleTaradod[j].CheckValidVk();
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 345);
			throw;
		}
		return ret;
	}

	public async Task<ShiftDetView> GetShiftAsync(DateTime ptime, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 377);
			throw;
		}
	}

	public DateTime SetTime(DateTime date, string time)
	{
		return new DateTime(date.Year, date.Month, date.Day, int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(3, 2)), 0);
	}

	public async Task<StatuseTaradodEnumType> GetStatuseTaradodAsync(CancellationToken token)
	{
		try
		{
			if ((int)_statuseTaradod == 0)
			{
				_statuseTaradod = await CalcStatuseTaradodAsync(token);
			}
			return _statuseTaradod;
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

	private async Task<StatuseTaradodEnumType> CalcStatuseTaradodAsync(CancellationToken token)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CalcStatuseTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 448);
			throw;
		}
	}

	public Person Person()
	{
		return _person11;
	}

	private async Task<bool> CalcTaradodAsync(DateTime pDateM, int PID, CancellationToken token = default(CancellationToken))
	{
		new StringBuilder().AppendLine($"in {DateTime.Now} method CalcTaradod");
		try
		{
			throw new NotImplementedException();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "Day : " + DayofShMonth, "CalcTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 581);
			throw;
		}
	}

	public async Task<bool> WorkdayAsync(EnWorkdayEnumType pEnWorkday, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "WorkdayAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClubDet.cs", 617);
			throw;
		}
	}
}
