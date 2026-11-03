using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Properties;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class RunScript
{
	private static List<string> _lstDatabaseNamesThanDropNonDboExecutes = new List<string>();

	private static HashSet<string> _initializedList = new HashSet<string>();

	public static RunScript Instance { get; } = new RunScript();


	public void ResetInitializeList()
	{
		_initializedList = new HashSet<string>();
	}

	internal async Task<ReturnedSaveFuncInfo> ExecuteSqlLogScripts(ISettingRepo repo, ENSource source, string connectionString)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await RunAsync(repo, "SqlBackUpScripts", repo != null, source, Resources.SqlBackUpScripts, connectionString), ReturnedState.None, "ExecuteSqlLogScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 38);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ExecuteSqlLogScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 42);
			res.AddReturnedValue(ex, "", "ExecuteSqlLogScripts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 43);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RunSqlScriptsAsync(ISettingRepo repo, string connectionString, ENSource source)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (_initializedList.Contains(connectionString))
			{
				return ret;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ExecuteSqlLogScripts(repo, source, connectionString), ReturnedState.None, "RunSqlScriptsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 54);
			foreach (SqlQuery item in await SqlQuery.GetAllAsync(connectionString, source))
			{
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await RunAsync(repo, $"CustomQuery_{item.Guid}", checkCrcBeforeExecution: false, source, item.Query, connectionString), ReturnedState.None, "RunSqlScriptsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 58);
			}
			if (!ret.HasError)
			{
				_initializedList.Add(connectionString);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RunSqlScriptsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 64);
			ret.AddReturnedValue(ex, "", "RunSqlScriptsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 65);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RunAsync(ISettingRepo repo, string scriptTitle, bool checkCrcBeforeExecution, ENSource source, string script, string connectionString, int timeOut = 30, CancellationToken token = default(CancellationToken))
	{
		if (checkCrcBeforeExecution && repo == null)
		{
			throw new ArgumentNullException("repo");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(script))
		{
			return ret;
		}
		using (new JaegerMonitor("RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs"))
		{
			long scriptCrc = script.CalcCrc();
			Stopwatch sw = Stopwatch.StartNew();
			RunScriptInformation jsonData = null;
			string key = "RunScript_" + scriptTitle;
			bool scriptExecuteShouldBeRun = true;
			try
			{
				if (checkCrcBeforeExecution)
				{
					string text = await repo.GetValueAsync(connectionString, key, token);
					if (!string.IsNullOrEmpty(text))
					{
						jsonData = text.FromJson<RunScriptInformation>("RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 89);
						if (jsonData.LastSuccessExcecutionWasSuccess.HasValue && jsonData.LastSuccessExcecutionWasSuccess.Value && jsonData.Crc == scriptCrc)
						{
							scriptExecuteShouldBeRun = false;
							return ret;
						}
					}
				}
				if (jsonData == null)
				{
					jsonData = new RunScriptInformation();
				}
				SqlConnection cn = new SqlConnection(connectionString);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(timeOut * 1000).Token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RunAsync(repo, scriptTitle, checkCrcBeforeExecution, source, script, cn, connectionString, null, timeOut), ReturnedState.None, "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 102);
					((DbConnection)(object)cn).Close();
				}
				finally
				{
					((IDisposable)cn)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 107);
				ret.AddReturnedValue(ex, "", "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 108);
			}
			finally
			{
				if (sw.ElapsedMilliseconds > 30000)
				{
					ret.AddWarning("اجرای اسکریپت".GetTranslate() + "  (" + scriptTitle + ")  " + "به میزان".GetTranslate() + " " + $" {sw.ElapsedMilliseconds}(ms)" + " " + " طول کشید.".GetTranslate(), "", "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 114);
				}
				if (scriptExecuteShouldBeRun && checkCrcBeforeExecution)
				{
					jsonData.Crc = scriptCrc;
					jsonData.LastExcecutionDuration = sw.ElapsedMilliseconds;
					jsonData.LastExcecutionError = ret.ErrorMessage;
					jsonData.LastSuccessExcecutionWasSuccess = !ret.HasError;
					Task.Run(() => repo.SaveSettingAsync(connectionString, key, Json.ToStringJson(jsonData), token));
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RunAsync(ISettingRepo repo, string scriptTitle, bool checkCrcBeforeExecution, ENSource source, string script, SqlConnection cn, string connectionString, SqlTransaction transaction = null, int timeOut = 30, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(script))
		{
			return ret;
		}
		long scriptCrc = script.CalcCrc();
		Stopwatch sw = Stopwatch.StartNew();
		using (new JaegerMonitor("RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs"))
		{
			string key = "RunScript_" + scriptTitle;
			bool scriptExecuteShouldBeRun = true;
			RunScriptInformation jsonData = null;
			try
			{
				if (checkCrcBeforeExecution)
				{
					string text = await repo.GetValueAsync(connectionString, key, token);
					if (!string.IsNullOrEmpty(text))
					{
						jsonData = text.FromJson<RunScriptInformation>("RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 145);
						if (jsonData.LastSuccessExcecutionWasSuccess.HasValue && jsonData.LastSuccessExcecutionWasSuccess.Value && jsonData.Crc == scriptCrc)
						{
							scriptExecuteShouldBeRun = false;
							return ret;
						}
					}
				}
				if (jsonData == null)
				{
					jsonData = new RunScriptInformation();
				}
				Task.Run(() => DropNonDboStoredProcedures(connectionString, token));
				if (string.IsNullOrEmpty(script))
				{
					return ret;
				}
				string[] separator = new string[7] { "go\r", "go\r\n", "\ngo\n", "\ngo\r\n", "\tgo\t", " go ", "\rgo\r" };
				script = script.Replace("GO", "go").Replace("Go", "go").Replace("gO", "go");
				string[] array = script.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				((DbCommand)val).CommandTimeout = timeOut;
				SqlCommand cmd = val;
				try
				{
					if (transaction != null)
					{
						cmd.Transaction = transaction;
					}
					string[] array2 = array;
					foreach (string item in array2)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ExecuteAsync(source, item, cmd, Guid.NewGuid(), connectionString, token), ReturnedState.None, "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 166);
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 170);
				ret.AddReturnedValue(ex, "", "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 171);
			}
			finally
			{
				if (sw.ElapsedMilliseconds > 30000)
				{
					ret.AddWarning($"اجرای اسکریپت ({scriptTitle}) به میزان {sw.ElapsedMilliseconds}(ms) طول کشید.", "", "RunAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 177);
				}
				if (scriptExecuteShouldBeRun)
				{
					jsonData.Crc = scriptCrc;
					jsonData.LastExcecutionDuration = sw.ElapsedMilliseconds;
					jsonData.LastExcecutionError = ret.ErrorMessage;
					jsonData.LastSuccessExcecutionWasSuccess = !ret.HasError;
					if (checkCrcBeforeExecution)
					{
						Task.Run(() => repo.SaveSettingAsync(connectionString, key, Json.ToStringJson(jsonData), token));
					}
				}
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> ExecuteAsync(ENSource source, string item, SqlCommand cmd, Guid guid, string connectionString, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("ExecuteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs"))
		{
			_ = 1;
			try
			{
				((DbCommand)(object)cmd).CommandText = item;
				if (((DbConnection)(object)cmd.Connection).State != ConnectionState.Open)
				{
					await ((DbConnection)(object)cmd.Connection).OpenAsync(token).ConfigureAwait(continueOnCapturedContext: false);
				}
				await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				Task.Run(() => RemoveDependenciesAsync(ex, cmd));
				Task.Run(() => LogMessageAsync(ex, item, ((DbCommand)(object)cmd).CommandTimeout, source, guid, token));
				WebErrorLog.Instance.StartLog(ex, "error in command " + item, "ExecuteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 207);
				ret.AddReturnedValue(ReturnedState.Error, ex.Message + "\r\nQuery:" + item, "", "ExecuteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 208);
			}
			return new ReturnedSaveFuncInfo();
		}
	}

	private async Task DropNonDboStoredProcedures(string connectionString, CancellationToken token)
	{
		_ = 3;
		try
		{
			string initialCatalog = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
			if (_lstDatabaseNamesThanDropNonDboExecutes.Contains(initialCatalog.FixString()))
			{
				return;
			}
			_lstDatabaseNamesThanDropNonDboExecutes.Add(initialCatalog.FixString());
			string text = "select 'drop procedure [' + SPECIFIC_SCHEMA + '].[' + SPECIFIC_NAME + ']' from[" + initialCatalog + "].information_schema.routines where SPECIFIC_SCHEMA<>'dbo' and routine_type = 'PROCEDURE'";
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				List<string> lstDropCommands = new List<string>();
				SqlCommand val = new SqlCommand(text, cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync())
						{
							if (((DbDataReader)(object)dr)[0].ToString() != DBNull.Value.ToString() && !string.IsNullOrEmpty(((DbDataReader)(object)dr)[0].ToString()))
							{
								lstDropCommands.Add(((DbDataReader)(object)dr)[0].ToString());
							}
						}
						((DbDataReader)(object)dr).Close();
						if (lstDropCommands != null && lstDropCommands.Any())
						{
							foreach (string item in lstDropCommands)
							{
								((DbCommand)(object)cmd).CommandText = item;
								await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
							}
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
		catch (Exception ex)
		{
			WebErrorLog instance = WebErrorLog.Instance;
			object obj = connectionString ?? "null";
			if (obj == null)
			{
				obj = "";
			}
			instance.StartLog(ex, (string)obj, "DropNonDboStoredProcedures", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 242);
		}
	}

	private async Task RemoveDependenciesAsync(Exception exception, SqlCommand cmd)
	{
		if (!exception.Message.ToLower().Contains("The statistics".ToLower()) || !exception.Message.ToLower().Contains("is dependent on column".ToLower()))
		{
			return;
		}
		try
		{
			string text = exception.Message.Split("'".ToCharArray())[1];
			((DbCommand)(object)cmd).CommandText = "SELECT 'DROP STATISTICS'  + QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name) + '.' + QUOTENAME(st.name) AS sql FROM sys.stats AS st INNER JOIN sys.tables AS t ON st.object_id = t.object_id WHERE QUOTENAME(st.name) ='[" + text + "]'";
			SqlDataReader dr = await cmd.ExecuteReaderAsync();
			try
			{
				if (await ((DbDataReader)(object)dr).ReadAsync())
				{
					string commandText = ((DbDataReader)(object)dr)[0].ToString();
					((DbDataReader)(object)dr).Close();
					((DbCommand)(object)cmd).CommandText = commandText;
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
				}
			}
			finally
			{
				((IDisposable)dr)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDependenciesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 264);
		}
	}

	private Task LogMessageAsync(Exception execption, string item, int timeOut, ENSource source, Guid guid, CancellationToken token)
	{
		try
		{
			if (timeOut < 3)
			{
				return Task.CompletedTask;
			}
			StringBuilder stringBuilder = new StringBuilder(100000);
			stringBuilder.AppendLine("Source: #" + source.GetDisplay().Replace(" ", "_"));
			stringBuilder.AppendLine($"TimeOut: #T{timeOut}");
			stringBuilder.AppendLine("exception:" + execption.Message);
			if (!string.IsNullOrEmpty(execption?.InnerException?.Message))
			{
				stringBuilder.AppendLine("❗\ufe0finnerException:" + (execption.InnerException?.Message ?? "null"));
			}
			if (!string.IsNullOrEmpty(execption?.InnerException?.InnerException?.Message))
			{
				stringBuilder.AppendLine("‼\ufe0finnerException2:" + (execption.InnerException?.InnerException?.Message ?? "null"));
			}
			stringBuilder.AppendLine("Query:" + item);
			WebTelegramMessage.Get_QueryTimedOut_bot().Sent2web(stringBuilder.ToString(), guid, "LogMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 285);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LogMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\RunScript.cs", 287);
		}
		return Task.CompletedTask;
	}
}
