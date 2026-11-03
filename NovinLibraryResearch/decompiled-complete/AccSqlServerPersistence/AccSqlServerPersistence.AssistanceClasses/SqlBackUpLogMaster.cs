using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class SqlBackUpLogMaster
{
	private static ENSource _source;

	private static EnBackUpType _type;

	private static Guid? _userAccGuid;

	private static IBackUpLog_GetUserName _userNameInterface;

	private static Guid _guid;

	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string Time => DateM.ToShortTimeString();

	public string DateSh => DateM.MiladiToCalture();

	public Guid UserAccGuid { get; set; }

	public string UserName { get; set; } = "";


	public string Path { get; set; }

	public EnBackUpStatus Status { get; set; }

	public string StatusName => Status.GetDisplay().GetTranslate();

	public ENSource Source { get; set; }

	public string SourceName => Source.GetDisplay().GetTranslate();

	public EnBackUpType Type { get; set; }

	public string TypeName => Type.GetDisplay().GetTranslate();

	public string Description { get; set; }

	private static async Task<SqlBackUpLogMaster> LoadDataAsync(string contextGuid, SqlDataReader dr, string connectionString)
	{
		try
		{
			SqlBackUpLogMaster item = new SqlBackUpLogMaster();
			if (((DbDataReader)(object)dr)["Guid"] != DBNull.Value)
			{
				item.Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["DateM"] != DBNull.Value)
			{
				item.DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["UserAccGuid"] != DBNull.Value)
			{
				item.UserAccGuid = (Guid)((DbDataReader)(object)dr)["UserAccGuid"];
				if (item.UserAccGuid == Guid.Empty)
				{
					item.UserName = "";
				}
				else if (_userNameInterface != null)
				{
					SqlBackUpLogMaster sqlBackUpLogMaster = item;
					sqlBackUpLogMaster.UserName = await _userNameInterface.GetUserNameAsync(contextGuid, item.UserAccGuid, connectionString);
				}
			}
			if (((DbDataReader)(object)dr)["Status"] != DBNull.Value)
			{
				item.Status = (EnBackUpStatus)((DbDataReader)(object)dr)["Status"];
			}
			if (((DbDataReader)(object)dr)["Path"] != DBNull.Value)
			{
				item.Path = ((DbDataReader)(object)dr)["Path"].ToString();
			}
			if (((DbDataReader)(object)dr)["Description"] != DBNull.Value)
			{
				item.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["Source"] != DBNull.Value)
			{
				item.Source = (ENSource)((DbDataReader)(object)dr)["Source"];
			}
			if (((DbDataReader)(object)dr)["Type"] != DBNull.Value)
			{
				item.Type = (EnBackUpType)((DbDataReader)(object)dr)["Type"];
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 66);
			throw;
		}
	}

	public static void Init(IBackUpLog_GetUserName userNameInterface)
	{
		_userNameInterface = userNameInterface;
	}

	public static async Task InitAsync(ISettingRepo repo, string connectionString, Guid masterGuid, ENSource source, EnBackUpType type, Guid? userAccGuid, IBackUpLog_GetUserName userNameInterface)
	{
		try
		{
			await RunScript.Instance.RunSqlScriptsAsync(repo, connectionString, source);
			_source = source;
			_type = type;
			_userAccGuid = userAccGuid;
			_userNameInterface = userNameInterface;
			_guid = masterGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 83);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(ISettingRepo repo, EnBackUpStatus status, string statusDesc, string desc, string path, string connectionString)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			await RunScript.Instance.RunSqlScriptsAsync(repo, connectionString, _source);
			SqlBackUpLogMaster log = new SqlBackUpLogMaster
			{
				Guid = _guid,
				Type = _type,
				DateM = DateTime.Now,
				Source = _source,
				UserAccGuid = (_userAccGuid ?? Guid.Empty),
				Description = desc,
				Status = status,
				Path = path
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await log.SaveAsync(connectionString), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 103);
			SqlBackUpLogSlaves obj = new SqlBackUpLogSlaves
			{
				Guid = Guid.NewGuid(),
				Status = status,
				MasterGuid = log.Guid,
				Description = desc,
				StatusDescription = statusDesc,
				DateM = DateTime.Now
			};
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(connectionString), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 114);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 118);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 119);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string connectionString)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand val = new SqlCommand("sp_SqlBackUp_Log_Save", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@guid", (object)Guid);
					cmd.Parameters.AddWithValue("@dateM", (object)DateM);
					cmd.Parameters.AddWithValue("@userGuid", (object)UserAccGuid);
					cmd.Parameters.AddWithValue("@status", (object)(short)Status);
					cmd.Parameters.AddWithValue("@path", (object)(Path ?? ""));
					cmd.Parameters.AddWithValue("@desc", (object)(Description ?? ""));
					cmd.Parameters.AddWithValue("@source", (object)Source);
					cmd.Parameters.AddWithValue("@type", (object)(short)Type);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
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
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 145);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 146);
		}
		return res;
	}

	public static async Task<List<SqlBackUpLogMaster>> GetAllAsync(string contextGuid, string connectionString, string search)
	{
		List<SqlBackUpLogMaster> ret = new List<SqlBackUpLogMaster>();
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				SqlCommand val = new SqlCommand("[sp_SqlBackUp_Log_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync())
						{
							List<SqlBackUpLogMaster> list = ret;
							list.Add(await LoadDataAsync(contextGuid, dr, connectionString));
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						foreach (string item in search.SplitString())
						{
							ret = ret.Where((SqlBackUpLogMaster q) => q.UserName.ToLower().Contains(item.ToLower()) || q.StatusName.ToLower().Contains(item.ToLower()) || q.Description.ToLower().Contains(item.ToLower()) || q.Path.ToLower().Contains(item.ToLower())).ToList();
						}
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
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 170);
		}
		return ret;
	}

	public static async Task<SqlBackUpLogMaster> GetLastAsync(string contextGuid, string connectionString, CancellationToken token)
	{
		_ = 3;
		try
		{
			SqlBackUpLogMaster ret = null;
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand val = new SqlCommand("[sp_BackUpLog_Master_GetLast]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = await LoadDataAsync(contextGuid, dr, connectionString);
						}
						((DbDataReader)(object)dr).Close();
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
			WebErrorLog.Instance.StartLog(ex, "", "GetLastAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogMaster.cs", 192);
			throw;
		}
	}
}
