using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using Serilog;

namespace TimeAttendanceDAL.Dal.Taradod;

public class LogOnPersonel_audits
{
	public string Time { get; private set; } = "";


	public DateTime updated_at { get; private set; } = DateTime.Now.Date;


	public string updated_atSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(updated_at);

	private LogOnPersonel_audits(SqlDataReader dr)
	{
		if (dr != null)
		{
			LoadData_audits(dr);
		}
	}

	public static async Task<List<LogOnPersonel_audits>> GetAllAsync(string contextGuid, int logonPersonId, CancellationToken token = default(CancellationToken))
	{
		List<LogOnPersonel_audits> ret = new List<LogOnPersonel_audits>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_LogonPersonel_audits_GetAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@LogonPersonelId", (object)logonPersonId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new LogOnPersonel_audits(dr));
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

	private void LoadData_audits(SqlDataReader dr)
	{
		try
		{
			_ = ((DbDataReader)(object)dr)["Changeid"].ToString() != DBNull.Value.ToString();
			_ = ((DbDataReader)(object)dr)["LogonPersonelId"].ToString() != DBNull.Value.ToString();
			if (((DbDataReader)(object)dr)["Time"].ToString() != DBNull.Value.ToString())
			{
				Time = ((DbDataReader)(object)dr)["Time"].ToString();
			}
			if (((DbDataReader)(object)dr)["updated_at"].ToString() != DBNull.Value.ToString())
			{
				updated_at = (DateTime)((DbDataReader)(object)dr)["updated_at"];
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
