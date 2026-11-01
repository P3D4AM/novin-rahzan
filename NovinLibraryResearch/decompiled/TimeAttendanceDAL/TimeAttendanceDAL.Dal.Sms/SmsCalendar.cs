using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace TimeAttendanceDAL.Dal.Sms;

public class SmsCalendar : IHasContext
{
	public string ContextGuid { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public int GroupId { get; set; }

	public string GroupName { get; set; }

	public DateTime DateM { get; set; }

	public string DateCalture => Calendar.MiladiToCalture(DateM);

	public string Description { get; set; }

	public SmsCalendar(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<SmsCalendar> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SmsCalendar ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_SmsCalendar_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Guid", (object)guid);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(dr, contextGuid);
						}
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
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Sms\\SmsCalendar.cs", 46);
			throw;
		}
	}

	private static SmsCalendar LoadData(SqlDataReader dr, string contextGuid)
	{
		try
		{
			SmsCalendar smsCalendar = new SmsCalendar(contextGuid);
			if (((DbDataReader)(object)dr)["Guid"].ToString() != DBNull.Value.ToString())
			{
				smsCalendar.Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["GroupId"].ToString() != DBNull.Value.ToString())
			{
				smsCalendar.GroupId = (int)((DbDataReader)(object)dr)["GroupId"];
			}
			if (((DbDataReader)(object)dr)["GroupName"].ToString() != DBNull.Value.ToString())
			{
				smsCalendar.GroupName = ((DbDataReader)(object)dr)["GroupName"].ToString();
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				smsCalendar.DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				smsCalendar.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			return smsCalendar;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Sms\\SmsCalendar.cs", 64);
			throw;
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_SmsCalendar_Save", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Guid", (object)Guid);
					cmd.Parameters.AddWithValue("GroupId", (object)GroupId);
					cmd.Parameters.AddWithValue("Description", (object)Description);
					cmd.Parameters.AddWithValue("DateM", (object)DateM);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
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
				SqlCommand cmd = new SqlCommand("sp_SmsCalendar_DeleteRow", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Guid", (object)Guid);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
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

	public static async Task<List<SmsCalendar>> SelectAllAsync(string contextGuid, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<SmsCalendar> ret = new List<SmsCalendar>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_SmsCalendar_SelectAll", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(LoadData(dr, contextGuid));
						}
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
