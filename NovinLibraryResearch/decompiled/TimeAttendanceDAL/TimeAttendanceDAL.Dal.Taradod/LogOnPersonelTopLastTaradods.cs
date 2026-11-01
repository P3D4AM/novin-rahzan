using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Attendance;
using Serilog;

namespace TimeAttendanceDAL.Dal.Taradod;

public class LogOnPersonelTopLastTaradods
{
	private string personFullName;

	private string trafficTypeName;

	public DateTime DateTimeLogM { get; set; } = DateTime.MinValue;


	public DateTime GharardadEndDateM { get; private set; }

	public string GharardadEndDateClature => Calendar.MiladiToCalture(GharardadEndDateM);

	public short GharardadStatus { get; private set; }

	public string GharardadStatusName { get; private set; }

	public string dateCalture => $"{Calendar.MiladiToCalture(DateTimeLogM)} {DateTimeLogM.TimeOfDay.Hours:D2}:{DateTimeLogM.TimeOfDay.Minutes:D2}";

	public string DeviceName { get; set; }

	public int personNo { get; set; }

	public int personId { get; set; }

	public string PersonFullName
	{
		get
		{
			if (string.IsNullOrEmpty(personFullName))
			{
				PersonFullName = $"عدم تخصیص کد ({personNo})";
			}
			return personFullName;
		}
		set
		{
			personFullName = value;
		}
	}

	public TypeTaradodEnumType TrafficType { get; set; }

	public string TrafficTypeName
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(trafficTypeName))
			{
				trafficTypeName = Utilities.GetDisplay<TypeTaradodEnumType>(TrafficType);
				if (trafficTypeName.Contains("خروج") || trafficTypeName.Contains("ورود"))
				{
					trafficTypeName = "تردد عادی";
				}
			}
			return trafficTypeName;
		}
	}

	private LogOnPersonelTopLastTaradods(SqlDataReader dr)
	{
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((DbDataReader)(object)dr)["personNo"].ToString() != DBNull.Value.ToString())
			{
				personNo = (int)((DbDataReader)(object)dr)["personNo"];
			}
			if (((DbDataReader)(object)dr)["personId"].ToString() != DBNull.Value.ToString())
			{
				personId = (int)((DbDataReader)(object)dr)["personId"];
			}
			if (((DbDataReader)(object)dr)["DeviceName"].ToString() != DBNull.Value.ToString())
			{
				DeviceName = ((DbDataReader)(object)dr)["DeviceName"].ToString();
			}
			if (((DbDataReader)(object)dr)["PersonFullName"].ToString() != DBNull.Value.ToString())
			{
				PersonFullName = ((DbDataReader)(object)dr)["PersonFullName"].ToString();
			}
			if (((DbDataReader)(object)dr)["DateTimeLogM"].ToString() != DBNull.Value.ToString())
			{
				DateTimeLogM = (DateTime)((DbDataReader)(object)dr)["DateTimeLogM"];
			}
			TrafficType = (TypeTaradodEnumType)((!(((DbDataReader)(object)dr)["TrafficType"].ToString() != DBNull.Value.ToString())) ? 34 : ((int)(TypeTaradodEnumType)((DbDataReader)(object)dr)["TrafficType"]));
			if (((DbDataReader)(object)dr)["GharardadEndDate"].ToString() != DBNull.Value.ToString())
			{
				GharardadEndDateM = (DateTime)((DbDataReader)(object)dr)["GharardadEndDate"];
			}
			if (((DbDataReader)(object)dr)["GharardadStatus"].ToString() != DBNull.Value.ToString())
			{
				GharardadStatus = (short)((DbDataReader)(object)dr)["GharardadStatus"];
			}
			if (((DbDataReader)(object)dr)["GharardadStatusName"].ToString() != DBNull.Value.ToString())
			{
				GharardadStatusName = ((DbDataReader)(object)dr)["GharardadStatusName"].ToString();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<LogOnPersonelTopLastTaradods>> GetAsync(string contextGuid, int countLastTaradods, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<LogOnPersonelTopLastTaradods> ret = new List<LogOnPersonelTopLastTaradods>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_LogonPersonel_SelectTopLastTaradods]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					((DbCommand)(object)cmd).CommandTimeout = ((DbConnection)(object)cmd.Connection).ConnectionTimeout;
					cmd.Parameters.AddWithValue("@Count", (object)countLastTaradods);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new LogOnPersonelTopLastTaradods(dr));
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

	public static async Task<List<LogOnPersonelTopLastTaradods>> GetAsync(string contextGuid, int countLastTaradods, int deviceId, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<LogOnPersonelTopLastTaradods> ret = new List<LogOnPersonelTopLastTaradods>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_LogonPersonel_SelectTopLastTaradodsBydeviceId]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					((DbCommand)(object)cmd).CommandTimeout = ((DbConnection)(object)cmd.Connection).ConnectionTimeout;
					cmd.Parameters.AddWithValue("@Count", (object)countLastTaradods);
					cmd.Parameters.AddWithValue("@deviceId", (object)deviceId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new LogOnPersonelTopLastTaradods(dr));
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
