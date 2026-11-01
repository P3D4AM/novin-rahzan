using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Other;

[Serializable]
[DoNotPruneType]
public class Holiday : IHoliday, IHasContext
{
	private static ConcurrentDictionary<DateTime, Holiday> _buffer = new ConcurrentDictionary<DateTime, Holiday>();

	public string ContextGuid { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string Monasebat { get; set; } = "";


	public string Description { get; set; } = "";


	public string Day => LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(DateM);

	public string DateCalture => Calendar.MiladiToCalture(DateM);

	public bool StRasmi { get; set; } = true;


	public Holiday(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<Holiday> GetAsync(string contextGuid, DateTime dateM, CancellationToken token = default(CancellationToken))
	{
		try
		{
			Holiday holiday = new Holiday(contextGuid)
			{
				ContextGuid = contextGuid
			};
			await holiday.LoadDataAsync(dateM, token);
			return holiday;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Holiday(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	public async Task<bool> SaveAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("dbo.sp_Holiday_Save", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("DateM", (object)DateM);
					cmd.Parameters.AddWithValue("Monasebat", (object)Text.FixString(Monasebat));
					cmd.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
					cmd.Parameters.AddWithValue("STRasmi", (object)StRasmi);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					_buffer.TryRemove(DateM, out var _);
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

	public async Task<bool> DeleteAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Holiday_DeleteRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("DateM", (object)DateM);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					_buffer.TryRemove(DateM, out var _);
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

	public static async Task<List<Holiday>> GetAllAsync(string contextGuid, DateTime? startDateM = null, DateTime? endDateM = null, bool isHolidayOnly = false, CancellationToken token = default(CancellationToken))
	{
		List<Holiday> ret = new List<Holiday>();
		try
		{
			_buffer.Clear();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_Holiday_SelectAll]", cn);
				try
				{
					DateTime defaultValue = Utilities.CalendarToMiladi(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 4) + "/01/01").AddYears(-2).AddDays(1.0);
					DateTime defaultValue2 = Utilities.CalendarToMiladi(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 4) + "/01/01").AddYears(2);
					cmd.Parameters.AddWithValue("startDateM", (object)startDateM.GetValueOrDefault(defaultValue));
					cmd.Parameters.AddWithValue("endDateM", (object)endDateM.GetValueOrDefault(defaultValue2));
					cmd.Parameters.AddWithValue("isHolidayOnly", (object)isHolidayOnly);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new Holiday(contextGuid, dr));
						}
						((DbConnection)(object)cn).Close();
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
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task<bool> LoadDataAsync(DateTime dateM, CancellationToken token)
	{
		_ = 2;
		try
		{
			if (_buffer.TryGetValue(dateM, out var value) && value != null)
			{
				DateM = value.DateM;
				Monasebat = value.Monasebat;
				Description = value.Description;
				StRasmi = value.StRasmi;
				return true;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_Holiday_SelectRow", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@DateM", (object)dateM);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							LoadData(dr);
						}
						((DbConnection)(object)cn).Close();
						return true;
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

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (dr == null)
			{
				return false;
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() == "")
			{
				return false;
			}
			DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			if (((DbDataReader)(object)dr)["Monasebat"].ToString() != "")
			{
				Monasebat = ((DbDataReader)(object)dr)["Monasebat"].ToString();
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != "")
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["STRasmi"].ToString() != "")
			{
				StRasmi = (bool)((DbDataReader)(object)dr)["STRasmi"];
			}
			_buffer.TryRemove(DateM, out var _);
			_buffer.TryAdd(DateM, this);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Holiday.cs", 190);
			return false;
		}
	}

	public static async Task<int> CountNotHolidayOnYearAsync(string contextGuid, int curYear, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			int result;
			try
			{
				SqlCommand cmd = new SqlCommand("select count(*) from [Holiday]  where [STRasmi] = 0 and left(dateM,4) = '" + curYear + "'", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					result = ((obj != null) ? short.Parse(obj.ToString()) : 0);
					((DbConnection)(object)cn).Close();
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
			return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<int> CountNotHolidayOnGharardadAsync(string contextGuid, DateTime startDate, DateTime endDate, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			int result;
			try
			{
				SqlCommand cmd = new SqlCommand("select count(*) from [Holiday]  where [STRasmi] = 0 and dateM between '" + startDate.ToString() + "' and '" + endDate.ToString() + "'", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					result = ((obj != null) ? short.Parse(obj.ToString()) : 0);
					((DbConnection)(object)cn).Close();
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
			return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
