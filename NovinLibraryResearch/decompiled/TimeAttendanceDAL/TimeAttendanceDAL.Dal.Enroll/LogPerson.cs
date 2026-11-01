using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using Serilog;

namespace TimeAttendanceDAL.Dal.Enroll;

public class LogPerson
{
	public int PersonId { get; set; }

	public string DateTimeLogsh { get; set; }

	public DateTime DateTimeLogM1 { get; set; }

	public DateTime DateTimeLogM2 { get; set; }

	public int Duration { get; set; }

	public LogPerson(IDataRecord dr)
	{
		try
		{
			if (dr["PersonId"].ToString() != DBNull.Value.ToString())
			{
				PersonId = (int)dr["PersonId"];
			}
			if (dr["DateTimeLogM1"].ToString() != DBNull.Value.ToString())
			{
				DateTimeLogM1 = (DateTime)dr["DateTimeLogM1"];
			}
			if (dr["DateTimeLogM2"].ToString() != DBNull.Value.ToString())
			{
				DateTimeLogM2 = (DateTime)dr["DateTimeLogM2"];
			}
			if (dr["Duration"].ToString() != DBNull.Value.ToString())
			{
				Duration = (int)dr["Duration"];
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public static async Task<List<LogPerson>> GetSessionNoAsync(string contextGuid, int personid, DateTime startDateM, DateTime endDateM, CancellationToken token)
	{
		List<LogPerson> ret = new List<LogPerson>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_LogonPersonel_Duration]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("personId", (object)personid);
					cmd.Parameters.AddWithValue("StartDate", (object)startDateM);
					cmd.Parameters.AddWithValue("EndDate", (object)endDateM);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new LogPerson((IDataRecord)dr));
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

	internal static List<LogPerson> GetSessionNo(string contextGuid, int personId, DateTime dateStartMGharardad, DateTime dateFinishMGharardad)
	{
		throw new NotImplementedException();
	}
}
