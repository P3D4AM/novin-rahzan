using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.AssistanceClasses;
using AccSqlServerPersistence.Properties;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Settings;
using PacketParser.Settings.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories;

public class LocalSettingPersistenceRepository : LocalSettingPersistenceRepositoryBase
{
	private static ConcurrentDictionary<string, bool> _scriptRuned = new ConcurrentDictionary<string, bool>();

	public LocalSettingPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<VersionModel> GetCacheVersionAsync(string sqlConnection, CancellationToken token)
	{
		VersionModel ret = null;
		try
		{
			string query = "if EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='Acc_LocalSetting')  if (EXISTS (select[Value] as 'version','Acc_LocalSetting' as 'table' from Acc_LocalSetting where [key] = 'AccCacheVersion') ) begin   select[Value] as 'version','Acc_LocalSetting' as 'table' from Acc_LocalSetting where [key] = 'AccCacheVersion'   return  end ";
			SqlConnection cn = new SqlConnection(sqlConnection);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand cmd = new SqlCommand(query, cn);
				try
				{
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = new VersionModel
							{
								Version = ((DbDataReader)(object)dr)[0].ToString(),
								TableName = ((DbDataReader)(object)dr)[1].ToString()
							};
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "GetCacheVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 52);
			throw;
		}
	}

	public static async Task<VersionModel> GetDbRefreshVerAsync(string sqlConnection, CancellationToken token)
	{
		VersionModel ret = null;
		try
		{
			string Query = "if EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='Acc_LocalSetting')  if (EXISTS (select[Value] as 'version','Acc_LocalSetting' as 'table' from Acc_LocalSetting where [key] = 'DBRefVer') ) begin   select[Value] as 'version','Acc_LocalSetting' as 'table' from Acc_LocalSetting where [key] = 'DBRefVer'   return  end  IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'ApllicationSettings')  select Val as 'version','ApllicationSettings' as 'table' from ApllicationSettings where [Name] = 'DBRefVer'  else  select '' as 'version','' as 'table' ";
			SqlConnection cn = new SqlConnection(sqlConnection);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand cmd = new SqlCommand(Query, cn);
				try
				{
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = new VersionModel
							{
								Version = ((DbDataReader)(object)dr)[0].ToString(),
								TableName = ((DbDataReader)(object)dr)[1].ToString()
							};
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "GetDbRefreshVerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 90);
			throw;
		}
	}

	public override async Task<VersionModel> GetExeVersionAsync(string sqlConnection, CancellationToken token)
	{
		VersionModel ret = null;
		try
		{
			string query = "if EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='Acc_LocalSetting')  if (EXISTS (select[Value] as 'version','Acc_LocalSetting' as 'table' from Acc_LocalSetting where [key] = 'AccExeVersion') ) begin   select[Value] as 'version','Acc_LocalSetting' as 'table' from Acc_LocalSetting where [key] = 'AccExeVersion'   return  end ";
			SqlConnection cn = new SqlConnection(sqlConnection);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand cmd = new SqlCommand(query, cn);
				try
				{
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = new VersionModel
							{
								Version = ((DbDataReader)(object)dr)[0].ToString(),
								TableName = ((DbDataReader)(object)dr)[1].ToString()
							};
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "GetExeVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 124);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, string sqlConnectionString, string key, string value, string scriptTitle, string creatSpScript, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!((await SqlServerTableInformation.GetAllAsync(sqlConnectionString, "Acc_LocalSetting"))?.Any() ?? false))
				{
					return ret;
				}
				if (!string.IsNullOrEmpty(creatSpScript))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), scriptTitle, checkCrcBeforeExecution: true, ENSource.None, creatSpScript, sqlConnectionString, 30, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 139);
				}
				key = key.FixString();
				value = value.FixString();
				SqlConnection cn = new SqlConnection(sqlConnectionString);
				try
				{
					SqlCommand val = new SqlCommand("sp_Acc_LocalSetting_Save", cn);
					((DbCommand)val).CommandType = CommandType.StoredProcedure;
					SqlCommand cmd = val;
					try
					{
						cmd.Parameters.AddWithValue("Key", (object)key);
						cmd.Parameters.AddWithValue("value", (object)value);
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						if (key.ToLower() == "Acc_AbonmanSettings".ToLower())
						{
							ActivationAbonmanSettings activationAbonmanSettings = ((await GetValueAsync(contextGuid, sqlConnectionString, key, token)) ?? "").FromJson<ActivationAbonmanSettings>("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 151);
							ActivationAbonmanSettings activationAbonmanSettings2 = value.FromJson<ActivationAbonmanSettings>("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 152);
							if (activationAbonmanSettings2.Version < activationAbonmanSettings.Version)
							{
								throw new ArgumentException($"setting version is not valid!   new:{activationAbonmanSettings2.Version}/ exists:{activationAbonmanSettings.Version}");
							}
						}
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
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
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 161);
				ret.AddReturnedValue(ret, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 162);
			}
			return ret;
		}
	}

	public static async Task<string> GetValueAsync(string contextGuid, string sqlConnectionString, string key, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs"))
		{
			string ret = "";
			try
			{
				if (!((await SqlServerTableInformation.GetAllAsync(sqlConnectionString, "Acc_LocalSetting"))?.Any() ?? false))
				{
					return "";
				}
				if (!_scriptRuned.TryGetValue(sqlConnectionString, out var _))
				{
					_scriptRuned.TryAdd(sqlConnectionString, value: true);
					await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_Global_sp", checkCrcBeforeExecution: true, ENSource.None, Resources.Global_sp, sqlConnectionString, 30, token);
				}
				key = key.FixString();
				SqlConnection cn = new SqlConnection(sqlConnectionString);
				try
				{
					SqlCommand cmd = new SqlCommand("sp_Acc_LocalSetting_GetValue", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Key", (object)key);
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
						try
						{
							if (await ((DbDataReader)(object)dr).ReadAsync())
							{
								ret = ((DbDataReader)(object)dr).GetString(0);
							}
							((DbDataReader)(object)dr).Close();
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
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepository.cs", 197);
				throw;
			}
			return ret;
		}
	}
}
