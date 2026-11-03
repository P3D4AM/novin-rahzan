using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class BaseDataBaseAction
{
	public static BaseDataBaseAction Instance { get; } = new BaseDataBaseAction();


	public Task<CreateDatabaseResult> CreateDataBaseAsync(ISettingRepo repo, string dbName)
	{
		return CreateDataBaseAsync(repo, dbName, Settings.ServerConnectionsString);
	}

	public async Task SetScalarUdfInliningAsync(string connectionString)
	{
		_ = 1;
		try
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				return;
			}
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand cmd = new SqlCommand("ALTER DATABASE SCOPED CONFIGURATION SET TSQL_SCALAR_UDF_INLINING = OFF", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
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
			WebErrorLog.Instance.StartLog(ex, "", "SetScalarUdfInliningAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 31);
		}
	}

	public async Task<int> GetDataBaseVersionAsync(ISettingRepo repo, string connctionString, bool saveLog)
	{
		int ver;
		try
		{
			string version = "";
			SqlConnection cn = new SqlConnection(connctionString);
			try
			{
				SqlCommand cmd = new SqlCommand("Select SERVERPROPERTY('ProductVersion')", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync() && ((DbDataReader)(object)dr)[0] != DBNull.Value)
						{
							version = ((DbDataReader)(object)dr)[0].ToString();
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						if (string.IsNullOrEmpty(version))
						{
							return 0;
						}
						SqlVersion sqlVersion = new SqlVersion(version);
						ver = sqlVersion.SqlServerVersion;
						if (saveLog)
						{
							await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "دریافت نسخه دیتابیس".GetTranslate(), "نسخه دیتابیس".GetTranslate() + ":" + sqlVersion.Name, "", connctionString);
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
			WebErrorLog.Instance.StartLog(ex, "", "GetDataBaseVersionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 60);
			if (saveLog)
			{
				await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Error, "خطا در دریافت نسخه دیتابیس".GetTranslate(), ex.Message, "", connctionString);
			}
			ver = 0;
		}
		return ver;
	}

	public async Task<CreateDatabaseResult> CreateDataBaseAsync(ISettingRepo repo, string dbName, string serverConnectionString)
	{
		CreateDatabaseResult ret = new CreateDatabaseResult();
		if (string.IsNullOrEmpty(dbName))
		{
			ret.Status = EnCreateDataBase.databaseNameEmpty;
			ret.ReturnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "نام بانک اطلاعاتی خالی است.".GetTranslate(), "", "CreateDataBaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 75);
			return ret;
		}
		if (string.IsNullOrEmpty(serverConnectionString))
		{
			ret.Status = EnCreateDataBase.serverConnectionStringError;
			ret.ReturnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "رشته اتصال به سرور خالی است.".GetTranslate(), "", "CreateDataBaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 82);
			return ret;
		}
		try
		{
			SqlConnectionStringBuilder cbs = new SqlConnectionStringBuilder(serverConnectionString)
			{
				InitialCatalog = "master"
			};
			if (!(await IsExistDatabaseAsync(dbName, ((DbConnectionStringBuilder)(object)cbs).ConnectionString)).HasError)
			{
				ret.ReturnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "بانک اطلاعاتی با نام".GetTranslate() + "  " + dbName + " \r\n" + "در سرویس".GetTranslate() + "  " + serverConnectionString + "\r\n" + " در حال حاضر وجود دارد.".GetTranslate(), "", "CreateDataBaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 92);
				ret.Status = EnCreateDataBase.DatabaseExists;
				return ret;
			}
			SqlConnection cn = new SqlConnection(((DbConnectionStringBuilder)(object)cbs).ConnectionString);
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					string text = Assembly.GetEntryAssembly()?.Location;
					string dbPath = "";
					if (!string.IsNullOrEmpty(text))
					{
						FileInfo fileInfo = new FileInfo(text);
						dbPath = Path.Combine(fileInfo.DirectoryName ?? throw new InvalidOperationException(), "DataBase");
					}
					if (await Settings.SetDestinationFolderAsync(((DbConnection)(object)cn).ConnectionString) && !string.IsNullOrEmpty(dbPath))
					{
						((DbCommand)(object)cmd).CommandText = "CREATE DATABASE [DBName] ON  PRIMARY ( NAME = N'DBName',FILENAME = N'DBPath\\DBName.mdf', SIZE = 10240KB , FILEGROWTH = 10240KB )LOG ON ( NAME = N'DBName_log',FILENAME = N'DBPath\\DBName_log.ldf',SIZE=10240KB,FILEGROWTH=10%)";
						((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("DBName", dbName);
						((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("DBPath", dbPath);
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "CREATE DATABASE [DBName]";
						((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("DBName", dbName);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await SqlServerValidation.IsSqlVersionBiggerOrEqualThan2016Async(repo, ((DbConnection)(object)cn).ConnectionString);
					if (returnedSaveFuncInfo != null && returnedSaveFuncInfo.HasError)
					{
						ret.Status = EnCreateDataBase.SqlServerIsOld;
						ret.ReturnedSaveFuncInfo.AddError(returnedSaveFuncInfo.ErrorMessage, "", "CreateDataBaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 126);
						return ret;
					}
					string constr = ((DbConnection)(object)cn).ConnectionString.Replace("master", dbName);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
					((DbConnection)(object)cn).Close();
					ret.ConnectionString = constr;
					ret.Status = EnCreateDataBase.Success;
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
			if (ex.Message.ToLower().Contains("because it already exists".ToLower()))
			{
				ret.Status = EnCreateDataBase.DatabaseExists;
			}
			else
			{
				WebErrorLog.Instance.StartLog(ex, "dbName:" + dbName + "//ServerCn:" + serverConnectionString, "CreateDataBaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 144);
			}
			ret.ReturnedSaveFuncInfo.AddReturnedValue(ex, "", "CreateDataBaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 145);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> IsExistDatabaseAsync(string dbName, string connectionString, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT case when exists (select 1 from sys.Databases where Name = @DbName) then 1 else 0 end as DbExists", cn);
				try
				{
					cmd.Parameters.AddWithValue("@DbName", (object)dbName);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					int num = int.Parse((await ((DbCommand)(object)cmd).ExecuteScalarAsync(token)).ToString());
					((DbConnection)(object)cn).Close();
					if (num <= 0)
					{
						ret.AddError(EnSystem.None, "database(" + dbName + ") does not exists on Server(" + connectionString + ")", "", "IsExistDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 162);
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
			ret.AddReturnedValue(ex, "", "IsExistDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 166);
			WebErrorLog.Instance.StartLog($"dbname={dbName},connectionString={connectionString}, ex={ex}", "", "IsExistDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 167);
		}
		return ret;
	}

	public string GetDbName(string connectionString)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		string result = "";
		try
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				return result;
			}
			return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDbName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 183);
			return result;
		}
	}

	public async Task ChangeCompatibilityLevelAsync(string dbName, int compatibilityLevel, string connectionString)
	{
		_ = 1;
		try
		{
			if (string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(connectionString))
			{
				return;
			}
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand cmd = new SqlCommand("ALTER DATABASE [DBName] SET COMPATIBILITY_LEVEL = " + compatibilityLevel, cn);
				try
				{
					((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("DBName", dbName);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
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
			WebErrorLog.Instance.StartLog(ex, "", "ChangeCompatibilityLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 213);
		}
	}

	public async Task SetNumericRoundAbortAsync(string dbName, string connectionString)
	{
		_ = 1;
		try
		{
			if (string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(connectionString))
			{
				return;
			}
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand cmd = new SqlCommand("ALTER DATABASE [DBName] SET NUMERIC_ROUNDABORT OFF", cn);
				try
				{
					((DbCommand)(object)cmd).CommandText = ((DbCommand)(object)cmd).CommandText.Replace("DBName", dbName);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
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
			WebErrorLog.Instance.StartLog(ex, "", "SetNumericRoundAbortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\BaseDataBaseAction.cs", 227);
		}
	}
}
