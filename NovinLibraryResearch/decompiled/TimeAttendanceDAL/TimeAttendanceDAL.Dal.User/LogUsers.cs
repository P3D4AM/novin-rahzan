using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Attendance.User;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.User;

[Serializable]
[DoNotPruneType]
public class LogUsers : IHasContext
{
	public SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public long Id { get; private set; }

	public Guid Guid { get; private set; }

	public int UserId { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string DateCalture => Calendar.MiladiToCalture(DateM);

	public string Description { get; set; } = "";


	public string ActionTypeName { get; set; } = "";


	public string LogData { get; set; } = "";


	public string LogDataOld { get; set; } = "";


	public string LogDataNew { get; set; } = "";


	public string Refrence { get; set; } = "";


	public string UserName { get; set; }

	public string TheTime => DateM.ToString("HH:mm");

	public string DescActionTypeName => AccessLevelClass.Instance.GetDescription(ActionTypeName);

	public LogUsers(string contextGuid)
	{
		ContextGuid = contextGuid;
		if (UserAttendance.SelectedUser != null)
		{
			UserId = UserAttendance.SelectedUser.ID;
		}
	}

	public static async Task<LogUsers> GetAsync(string contextGuid, long logId, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			LogUsers ret = new LogUsers(contextGuid)
			{
				ContextGuid = contextGuid
			};
			if (UserAttendance.SelectedUser != null)
			{
				ret.UserId = UserAttendance.SelectedUser.ID;
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ret.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_LogUsers_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)logId);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.LoadData(dr);
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

	public LogUsers(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		try
		{
			if (UserAttendance.SelectedUser != null)
			{
				UserId = UserAttendance.SelectedUser.ID;
			}
			LoadData(dr);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\LogUsers.cs", 111);
		}
	}

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				Id = (long)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Guid"].ToString() != DBNull.Value.ToString())
			{
				Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["UserID"].ToString() != DBNull.Value.ToString())
			{
				UserId = (int)((DbDataReader)(object)dr)["UserID"];
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["LogData"].ToString() != DBNull.Value.ToString())
			{
				LogData = ((DbDataReader)(object)dr)["LogData"].ToString();
			}
			if (((DbDataReader)(object)dr)["LogDataOld"].ToString() != DBNull.Value.ToString())
			{
				LogDataOld = ((DbDataReader)(object)dr)["LogDataOld"].ToString();
			}
			if (((DbDataReader)(object)dr)["LogDataNew"].ToString() != DBNull.Value.ToString())
			{
				LogDataNew = ((DbDataReader)(object)dr)["LogDataNew"].ToString();
			}
			if (((DbDataReader)(object)dr)["Refrence"].ToString() != DBNull.Value.ToString())
			{
				Refrence = ((DbDataReader)(object)dr)["Refrence"].ToString();
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				UserName = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["ActionTypeName"].ToString() != DBNull.Value.ToString())
			{
				ActionTypeName = ((DbDataReader)(object)dr)["ActionTypeName"].ToString();
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public WebLogUsers ToWebLogUsers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			return new WebLogUsers
			{
				Id = Id,
				Guid = Guid,
				UserId = UserId,
				DateM = DateM,
				Description = Description,
				ActionTypeName = ActionTypeName,
				LogData = LogData,
				LogDataOld = LogDataOld,
				LogDataNew = LogDataNew,
				Refrence = Refrence
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToWebLogUsers", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\LogUsers.cs", 159);
		}
		return null;
	}

	public async Task<bool> SaveAsync(short tryCount = 3, bool awaitOnSemaphore = true)
	{
		_ = 4;
		try
		{
			if (awaitOnSemaphore)
			{
				await semaphoreSlim.WaitAsync();
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("dbo.sp_LogUsers_Insert", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					if (Id <= 0)
					{
						if (UserId == 0)
						{
							UserId = 1;
						}
						cmd.Parameters.AddWithValue("UserID", (object)UserId);
						cmd.Parameters.AddWithValue("DateM", (object)DateM);
						cmd.Parameters.AddWithValue("ActionTypeName", (object)(ActionTypeName ?? ""));
						cmd.Parameters.AddWithValue("Description", (object)(Utilities.GetTranslate(Text.FixString(Description)) ?? ""));
						cmd.Parameters.AddWithValue("LogData", (object)(Utilities.GetTranslate(LogData) ?? ""));
						cmd.Parameters.AddWithValue("LogDataOld", (object)(Utilities.GetTranslate(LogDataOld) ?? ""));
						cmd.Parameters.AddWithValue("LogDataNew", (object)(Utilities.GetTranslate(LogDataNew) ?? ""));
						cmd.Parameters.AddWithValue("Refrence", (object)(Refrence ?? ""));
						((DbCommand)(object)cmd).CommandTimeout = 60;
						await ((DbConnection)(object)cn).OpenAsync();
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync();
						((DbConnection)(object)cn).Close();
						if (obj != null)
						{
							Id = Convert.ToInt64(obj.ToString());
						}
					}
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
			if (tryCount > 0)
			{
				await Task.Delay(3000);
				LogUsers logUsers = this;
				short num = (short)(tryCount - 1);
				tryCount = num;
				return await logUsers.SaveAsync(num, awaitOnSemaphore: false);
			}
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\LogUsers.cs", 218);
		}
		finally
		{
			if (awaitOnSemaphore)
			{
				semaphoreSlim.Release();
			}
		}
		return false;
	}

	public static async Task<List<LogUsers>> SelectAllAsync(string contextGuid, string pLstUsersId, DateTime dateM1, DateTime dateM2, string desc, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<LogUsers> ret = new List<LogUsers>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_LogUsers_SelectAll]", cn);
				try
				{
					cmd.Parameters.AddWithValue("LstUsersID", (object)pLstUsersId);
					cmd.Parameters.AddWithValue("DateM1", (object)dateM1);
					cmd.Parameters.AddWithValue("DateM2", (object)dateM2);
					cmd.Parameters.AddWithValue("Desc", (object)("%" + desc + "%"));
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new LogUsers(contextGuid, dr));
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

	public static async Task RegisterNewLogAsync(string contextGuid, string pDesc, string pActionName, string refrenceId, string jsonLogData)
	{
		try
		{
			LogUsers logUsers = new LogUsers(contextGuid)
			{
				Description = pDesc,
				ActionTypeName = pActionName,
				Refrence = refrenceId
			};
			if (!string.IsNullOrEmpty(jsonLogData))
			{
				logUsers.LogData = jsonLogData;
			}
			await logUsers.SaveAsync(3);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task RegisterNewLogAsync<T>(string contextGuid, string pDesc, string pActionName, T pLogData, string refrenceId)
	{
		try
		{
			await new LogUsers(contextGuid)
			{
				Description = pDesc,
				ActionTypeName = pActionName,
				Refrence = refrenceId
			}.SaveAsync(3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RegisterNewLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\LogUsers.cs", 293);
		}
	}

	public static async Task RegisterNewLogAsync(string contextGuid, string pDesc, string pActionName, string jsonLogDataOld, string jsonLogDataNew, string refrenceId)
	{
		try
		{
			LogUsers logUsers = new LogUsers(contextGuid)
			{
				Description = pDesc,
				ActionTypeName = pActionName,
				Refrence = refrenceId
			};
			if (jsonLogDataOld != null)
			{
				logUsers.LogData = jsonLogDataOld;
				logUsers.LogDataOld = jsonLogDataOld;
			}
			if (jsonLogDataNew != null)
			{
				logUsers.LogDataNew = jsonLogDataNew;
			}
			await logUsers.SaveAsync(3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RegisterNewLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\LogUsers.cs", 317);
		}
	}

	public static async Task<bool> RegisterNewLogAsync(string contextGuid, string pDesc, string pActionName)
	{
		try
		{
			await new LogUsers(contextGuid)
			{
				Description = pDesc,
				ActionTypeName = pActionName
			}.SaveAsync(3);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RegisterNewLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\LogUsers.cs", 333);
		}
		return false;
	}

	public T Deserialize<T>()
	{
		return JsonConvert.DeserializeObject<T>(LogData);
	}
}
