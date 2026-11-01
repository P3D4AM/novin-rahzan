using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace TimeAttendanceDAL.Dal.Other;

public class Messages : IHasContext
{
	[JsonIgnore]
	public string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public string Message { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public DateTime ModifiedDate { get; set; }

	public int ForBelowVersions { get; set; }

	public int RepeatCount { get; set; }

	public Messages(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<List<string>> GetMessagesAsync(string contextGuid, DateTime dateM, int version, CancellationToken token)
	{
		List<string> ret = new List<string>();
		try
		{
			List<Messages> list = await new Messages(contextGuid).GetAllFromDbAsync(dateM, version, token);
			if (list != null)
			{
				ret = (from p in list
					orderby p.ModifiedDate
					select p.Message).ToList();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Messages.cs", 46);
		}
		return ret;
	}

	public async Task<List<Messages>> GetAllFromDbAsync(DateTime dateM, int version, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<Messages> ret = new List<Messages>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Messages_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@dateM", (object)dateM);
					cmd.Parameters.AddWithValue("@Ver", (object)version);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							Messages item = LoadData(dr);
							ret.Add(item);
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

	private Messages LoadData(SqlDataReader dr)
	{
		Messages messages = new Messages(ContextGuid);
		try
		{
			if (((DbDataReader)(object)dr)["Guid"].ToString() != DBNull.Value.ToString())
			{
				messages.Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["Message"].ToString() != DBNull.Value.ToString())
			{
				messages.Message = ((DbDataReader)(object)dr)["Message"].ToString();
			}
			if (((DbDataReader)(object)dr)["FromDate"].ToString() != DBNull.Value.ToString())
			{
				messages.FromDate = (DateTime)((DbDataReader)(object)dr)["FromDate"];
			}
			if (((DbDataReader)(object)dr)["ToDate"].ToString() != DBNull.Value.ToString())
			{
				messages.ToDate = (DateTime)((DbDataReader)(object)dr)["ToDate"];
			}
			if (((DbDataReader)(object)dr)["ModifiedDate"].ToString() != DBNull.Value.ToString())
			{
				messages.ModifiedDate = (DateTime)((DbDataReader)(object)dr)["ModifiedDate"];
			}
			if (((DbDataReader)(object)dr)["ForBelowVersions"].ToString() != DBNull.Value.ToString())
			{
				messages.ForBelowVersions = (int)((DbDataReader)(object)dr)["ForBelowVersions"];
			}
			if (((DbDataReader)(object)dr)["RepeatCount"].ToString() != DBNull.Value.ToString())
			{
				messages.RepeatCount = (int)((DbDataReader)(object)dr)["RepeatCount"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Messages.cs", 95);
		}
		return messages;
	}
}
