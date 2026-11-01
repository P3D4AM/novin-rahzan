using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.Dal.FLRShift;

public class ShiftDetView : IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(10, 10);

	private DateTime _currentDate = DateTime.Now;

	public ShiftDet.FriDayType Fritype;

	public string ContextGuid { get; set; }

	public short ShiftCodingId { get; set; }

	private int GharardadID { get; set; }

	public bool[] DayOfWeeks { get; set; } = new bool[7];


	public DateTime DateStartM { get; set; } = DateTime.Now;


	public DateTime DateFinishM { get; set; } = DateTime.Now;


	private DateTime GharardadDateStartM { get; set; } = DateTime.Now;


	private DateTime GharardadDateFinishM { get; set; } = DateTime.Now;


	public short PeakOvertime { get; set; }

	public short ShiftId { get; set; }

	public string Description { get; set; }

	public bool IsDayTatilInTaghvim { get; private set; }

	public bool WorkOnHolidaysWithOutFriDay { get; set; }

	public ShiftCodeing ClsShiftCoding { get; set; } = new ShiftCodeing();


	public int GetShiftLunchMinutes => ClsShiftCoding?.LunchTime ?? 0;

	public bool CheckIsWorkDay()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		try
		{
			if ((int)ClsShiftCoding.TypeShift == 11)
			{
				return false;
			}
			if (!IsDayTatilInTaghvim)
			{
				return true;
			}
			return (_currentDate.DayOfWeek != DayOfWeek.Friday) ? WorkOnHolidaysWithOutFriDay : (Fritype == ShiftDet.FriDayType.Elzami);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public ShiftDetView(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public ShiftDetView()
	{
	}

	public static async Task<ShiftDetView> Clone_ShiftDetViewAsync(string contextGuid, ShiftDetView source, DateTime dateForNewShift, ShiftCodeing newShiftCoding, CancellationToken token)
	{
		try
		{
			ShiftDetView ret = new ShiftDetView(contextGuid);
			for (int i = 0; i < 7; i++)
			{
				ret.DayOfWeeks[i] = source.DayOfWeeks[i];
			}
			ret.Fritype = source.Fritype;
			ret.IsDayTatilInTaghvim = source.IsDayTatilInTaghvim;
			ret.WorkOnHolidaysWithOutFriDay = source.WorkOnHolidaysWithOutFriDay;
			ret.DateStartM = source.DateStartM;
			ret.DateFinishM = source.DateFinishM;
			ret.GharardadDateStartM = source.GharardadDateStartM;
			ret.GharardadDateFinishM = source.GharardadDateFinishM;
			ret.ShiftId = source.ShiftId;
			ret.GharardadID = source.GharardadID;
			ret._currentDate = source._currentDate;
			ret.ShiftCodingId = source.ShiftCodingId;
			ret.ContextGuid = contextGuid;
			ret._currentDate = dateForNewShift;
			if (newShiftCoding == null)
			{
				await SetClsShiftCodingAsync(contextGuid, ret, dateForNewShift, token);
			}
			else
			{
				ret.ClsShiftCoding = newShiftCoding;
			}
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<ShiftDetView> GetAsync(string contextGuid, SqlDataReader dr, DateTime dateM, CancellationToken token)
	{
		try
		{
			ShiftDetView ret = LoadData(contextGuid, dr);
			ret.ContextGuid = contextGuid;
			ret._currentDate = dateM;
			await SetClsShiftCodingAsync(contextGuid, ret, dateM, token);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task SetClsShiftCodingAsync(string contextGuid, ShiftDetView ret, DateTime dateM, CancellationToken token)
	{
		try
		{
			ret.ClsShiftCoding = await ShiftCodeing.GetAsync(contextGuid, ret.ShiftCodingId, token);
			ret.ClsShiftCoding.CurrDate = dateM;
			if (ret.ShiftId == -1 || !ret.IsDayTatilInTaghvim)
			{
				return;
			}
			if (ret._currentDate.DayOfWeek == DayOfWeek.Friday)
			{
				if (ret.Fritype != ShiftDet.FriDayType.Elzami)
				{
					ret.ClsShiftCoding.M1 = 0;
					ret.ClsShiftCoding.M2 = 0;
					ret.ClsShiftCoding.H1 = 0;
					ret.ClsShiftCoding.H2 = 0;
					ret.ClsShiftCoding.S1 = 0;
					ret.ClsShiftCoding.S2 = 0;
					ShiftCodeing clsShiftCoding = ret.ClsShiftCoding;
					ShiftCodeing clsShiftCoding2 = ret.ClsShiftCoding;
					ShiftCodeing clsShiftCoding3 = ret.ClsShiftCoding;
					short num2 = (ret.ClsShiftCoding.TajilKh = 0);
					short num4 = (clsShiftCoding3.TakhirKh = num2);
					short tajilV = (clsShiftCoding2.TakhirV = num4);
					clsShiftCoding.TajilV = tajilV;
				}
			}
			else if (ret.IsDayTatilInTaghvim && !ret.WorkOnHolidaysWithOutFriDay)
			{
				ret.ClsShiftCoding.M1 = 0;
				ret.ClsShiftCoding.M2 = 0;
				ret.ClsShiftCoding.H1 = 0;
				ret.ClsShiftCoding.H2 = 0;
				ret.ClsShiftCoding.S1 = 0;
				ret.ClsShiftCoding.S2 = 0;
				ShiftCodeing clsShiftCoding4 = ret.ClsShiftCoding;
				ShiftCodeing clsShiftCoding5 = ret.ClsShiftCoding;
				ShiftCodeing clsShiftCoding6 = ret.ClsShiftCoding;
				short num2 = (ret.ClsShiftCoding.TajilKh = 0);
				short num4 = (clsShiftCoding6.TakhirKh = num2);
				short tajilV = (clsShiftCoding5.TakhirV = num4);
				clsShiftCoding4.TajilV = tajilV;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static ShiftDetView LoadData(string contextGuid, SqlDataReader dr)
	{
		try
		{
			ShiftDetView shiftDetView = new ShiftDetView(contextGuid);
			if (((DbDataReader)(object)dr)["ShiftCodingID"] != DBNull.Value)
			{
				shiftDetView.ShiftCodingId = (short)((DbDataReader)(object)dr)["ShiftCodingID"];
			}
			if (((DbDataReader)(object)dr)["Sat"] != DBNull.Value)
			{
				shiftDetView.DayOfWeeks[0] = (bool)((DbDataReader)(object)dr)["Sat"];
			}
			if (((DbDataReader)(object)dr)["Sun"] != DBNull.Value)
			{
				shiftDetView.DayOfWeeks[1] = (bool)((DbDataReader)(object)dr)["Sun"];
			}
			if (((DbDataReader)(object)dr)["Mon"] != DBNull.Value)
			{
				shiftDetView.DayOfWeeks[2] = (bool)((DbDataReader)(object)dr)["Mon"];
			}
			if (((DbDataReader)(object)dr)["Tue"] != DBNull.Value)
			{
				shiftDetView.DayOfWeeks[3] = (bool)((DbDataReader)(object)dr)["Tue"];
			}
			if (((DbDataReader)(object)dr)["Wed"] != DBNull.Value)
			{
				shiftDetView.DayOfWeeks[4] = (bool)((DbDataReader)(object)dr)["Wed"];
			}
			if (((DbDataReader)(object)dr)["Tur"] != DBNull.Value)
			{
				shiftDetView.DayOfWeeks[5] = (bool)((DbDataReader)(object)dr)["Tur"];
			}
			if (((DbDataReader)(object)dr)["Fri"] != DBNull.Value)
			{
				shiftDetView.DayOfWeeks[6] = (bool)((DbDataReader)(object)dr)["Fri"];
			}
			if (((DbDataReader)(object)dr)["FriType"] != DBNull.Value)
			{
				shiftDetView.Fritype = (ShiftDet.FriDayType)short.Parse(((DbDataReader)(object)dr)["FriType"].ToString());
			}
			if (((DbDataReader)(object)dr)["Holidays_Tatil"] != DBNull.Value)
			{
				shiftDetView.IsDayTatilInTaghvim = (bool)((DbDataReader)(object)dr)["Holidays_Tatil"];
			}
			if (((DbDataReader)(object)dr)["Shift_Tatital"] != DBNull.Value)
			{
				shiftDetView.WorkOnHolidaysWithOutFriDay = (bool)((DbDataReader)(object)dr)["Shift_Tatital"];
			}
			if (((DbDataReader)(object)dr)["DateStartM"] != DBNull.Value)
			{
				shiftDetView.DateStartM = (DateTime)((DbDataReader)(object)dr)["DateStartM"];
			}
			if (((DbDataReader)(object)dr)["DateFinishM"] != DBNull.Value)
			{
				shiftDetView.DateFinishM = (DateTime)((DbDataReader)(object)dr)["DateFinishM"];
			}
			if (((DbDataReader)(object)dr)["PeakOvertime"] != DBNull.Value)
			{
				shiftDetView.PeakOvertime = (short)((DbDataReader)(object)dr)["PeakOvertime"];
			}
			if (((DbDataReader)(object)dr)["ShiftID"] != DBNull.Value)
			{
				shiftDetView.ShiftId = (short)((DbDataReader)(object)dr)["ShiftID"];
			}
			return shiftDetView;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<ShiftDetView>> GetShiftDetViewListAsync(string contextGuid, DateTime pDateM, int pGharardadId, int personId, CancellationToken token)
	{
		_ = 4;
		try
		{
			List<ShiftDetView> fixTodayShift = new List<ShiftDetView>();
			if (token.IsCancellationRequested)
			{
				return null;
			}
			List<ShiftDetView> todayShifts = (await GetShiftDetViewOfDateAsync(contextGuid, pDateM, pGharardadId, token))?.OrderBy((ShiftDetView p) => p.ClsShiftCoding.Vorod).ToList();
			if (token.IsCancellationRequested)
			{
				return null;
			}
			List<ShiftDetView> list = (await GetShiftDetViewOfDateAsync(contextGuid, pDateM.AddDays(-1.0), pGharardadId, token))?.OrderBy((ShiftDetView p) => p.ClsShiftCoding.Vorod).ToList();
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (list != null && list.Count > 0)
			{
				ShiftDetView lastYShift = list[list.Count - 1];
				ShiftCodeing clsShiftCodingLY = lastYShift.ClsShiftCoding;
				if (clsShiftCodingLY.NextDay && clsShiftCodingLY.KhorojEnd < clsShiftCodingLY.Vorod)
				{
					if (token.IsCancellationRequested)
					{
						return null;
					}
					Morakhasidaily morakhasidaily = await Morakhasidaily.GetAsync(contextGuid, pDateM.AddDays(-1.0), personId, token);
					if (morakhasidaily == null || morakhasidaily.Id <= 0)
					{
						if (token.IsCancellationRequested)
						{
							return null;
						}
						ShiftCodeing shiftCodeing = ShiftCodeing.Clone(lastYShift.ClsShiftCoding);
						shiftCodeing.H1 = 0;
						shiftCodeing.M1 = 0;
						shiftCodeing.S1 = 0;
						shiftCodeing.TajilV = 0;
						shiftCodeing.TakhirV = 0;
						shiftCodeing.JarimeV = 0;
						shiftCodeing.CurrDate = lastYShift.ClsShiftCoding.CurrDate.AddDays(1.0);
						if (clsShiftCodingLY.LunchTime > 0 && (double)clsShiftCodingLY.StartLunchTime > clsShiftCodingLY.Vorod.TimeOfDay.TotalMinutes && clsShiftCodingLY.StartLunchTime + clsShiftCodingLY.LunchTime <= 1440)
						{
							shiftCodeing.LunchTime = 0;
							shiftCodeing.StartLunchTime = 0;
						}
						fixTodayShift.Add(await Clone_ShiftDetViewAsync(contextGuid, lastYShift, pDateM, shiftCodeing, token));
					}
				}
				else if (clsShiftCodingLY.NextDay && clsShiftCodingLY.KhorojEnd > clsShiftCodingLY.Vorod && clsShiftCodingLY.PeakOvertime > 0 && clsShiftCodingLY.PeakOvertimeTimeFormat < clsShiftCodingLY.Vorod)
				{
					ShiftCodeing shiftCodeing2 = ShiftCodeing.Clone(lastYShift.ClsShiftCoding);
					shiftCodeing2.H1 = 0;
					shiftCodeing2.M1 = 0;
					shiftCodeing2.S1 = 0;
					shiftCodeing2.TajilV = 0;
					shiftCodeing2.TakhirV = 0;
					shiftCodeing2.JarimeV = 0;
					shiftCodeing2.H2 = 0;
					shiftCodeing2.M2 = 0;
					shiftCodeing2.S2 = 5;
					shiftCodeing2.TajilKh = 0;
					shiftCodeing2.TakhirKh = 0;
					shiftCodeing2.CurrDate = clsShiftCodingLY.CurrDate.AddDays(1.0);
					shiftCodeing2.LunchTime = 0;
					shiftCodeing2.StartLunchTime = 0;
					if (token.IsCancellationRequested)
					{
						return null;
					}
					fixTodayShift.Add(await Clone_ShiftDetViewAsync(contextGuid, lastYShift, pDateM, shiftCodeing2, token));
				}
			}
			if (todayShifts != null && todayShifts.Count > 0)
			{
				ShiftCodeing clsShiftCoding = todayShifts[todayShifts.Count - 1].ClsShiftCoding;
				if (clsShiftCoding.NextDay && clsShiftCoding.KhorojEnd < clsShiftCoding.Vorod)
				{
					clsShiftCoding.H2 = 23;
					clsShiftCoding.M2 = 59;
					clsShiftCoding.S2 = 59;
					clsShiftCoding.TajilKh = 0;
					clsShiftCoding.TakhirKh = 0;
					if (clsShiftCoding.LunchTime == 0 || (double)clsShiftCoding.StartLunchTime < clsShiftCoding.Vorod.TimeOfDay.TotalMinutes)
					{
						clsShiftCoding.StartLunchTime = 0;
						clsShiftCoding.LunchTime = 0;
					}
					if (clsShiftCoding.PeakOvertime > 0 && clsShiftCoding.PeakOvertimeTimeFormat < clsShiftCoding.Vorod)
					{
						clsShiftCoding.PeakOvertime = 0;
					}
				}
				else if (clsShiftCoding.NextDay && clsShiftCoding.KhorojEnd > clsShiftCoding.Vorod && clsShiftCoding.PeakOvertime > 0 && clsShiftCoding.PeakOvertimeTimeFormat < clsShiftCoding.Vorod)
				{
					clsShiftCoding.PeakOvertime = 0;
				}
				fixTodayShift.AddRange(todayShifts);
			}
			return token.IsCancellationRequested ? null : fixTodayShift.OrderBy((ShiftDetView p) => p.ClsShiftCoding.H1).ToList();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<ShiftDetView>> GetShiftDetViewOfDateAsync(string contextGuid, DateTime pDateM, int pGharardadId, CancellationToken token)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _databaseSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			List<ShiftDetView> res = new List<ShiftDetView>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("SP_ShiftDetView_SelectRowByDate", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@GharardadID", (object)pGharardadId);
					cmd.Parameters.AddWithValue("@Date", (object)pDateM);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							List<ShiftDetView> list = res;
							list.Add(await GetAsync(contextGuid, dr, pDateM, token));
						}
						((DbConnection)(object)cn).Close();
						return res;
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
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_databaseSemaphore.Release();
			}
		}
	}

	public static async Task<List<ShiftDetView>> GetAllShiftDetViewOfDateAsync(string contextGuid, DateTime pDateM, int gharardadId, CancellationToken token)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _databaseSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			List<ShiftDetView> res = new List<ShiftDetView>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("SP_ShiftDetView_GetAllByDate", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@Date", (object)pDateM);
					cmd.Parameters.AddWithValue("@GharardadId", (object)gharardadId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							List<ShiftDetView> list = res;
							list.Add(await GetAsync(contextGuid, dr, pDateM, token));
						}
						((DbConnection)(object)cn).Close();
						return res;
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
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_databaseSemaphore.Release();
			}
		}
	}

	public static async Task<ShiftDetView> ConvertToShiftDetViewAsync(string contextGuid, ShiftProgrammableDet pProgrammableDet, CancellationToken token)
	{
		try
		{
			if (pProgrammableDet != null)
			{
				ShiftDetView xDetView = new ShiftDetView(contextGuid)
				{
					_currentDate = DateTime.Now,
					ShiftCodingId = pProgrammableDet.ShiftCodingID,
					DateStartM = pProgrammableDet.DateStartM,
					DateFinishM = pProgrammableDet.DateFinishM,
					Description = pProgrammableDet.Desc,
					Fritype = pProgrammableDet.FriDayType,
					ShiftId = -1,
					WorkOnHolidaysWithOutFriDay = pProgrammableDet.WorkOnHolidays
				};
				for (int i = 0; i < 7; i++)
				{
					xDetView.DayOfWeeks[i] = true;
				}
				await SetClsShiftCodingAsync(contextGuid, xDetView, xDetView._currentDate, token);
				return xDetView;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConvertToShiftDetViewAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRShift\\ShiftDetView.cs", 489);
			throw;
		}
		return null;
	}

	public int GetLunchMinutes(IEnumerable<CoupleTaradod> coupleTaradod)
	{
		try
		{
			int num = 0;
			foreach (CoupleTaradod item in coupleTaradod)
			{
				if (item.VTime == item.KTime)
				{
					if (item.VTime == "00:00")
					{
						break;
					}
				}
				else
				{
					short startLunchTime = ClsShiftCoding.StartLunchTime;
					short lunchTimeEnd = (short)(ClsShiftCoding.LunchTime + ClsShiftCoding.StartLunchTime);
					num += item.GetLunchMinutes(startLunchTime, lunchTimeEnd);
				}
			}
			return num;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> IsCalcInSourceDateAsync()
	{
		try
		{
			return ClsShiftCoding.CalcEnteghalInSrcDate && ClsShiftCoding.Vorod.TimeOfDay <= new TimeSpan(0, 0, 0, 1);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
