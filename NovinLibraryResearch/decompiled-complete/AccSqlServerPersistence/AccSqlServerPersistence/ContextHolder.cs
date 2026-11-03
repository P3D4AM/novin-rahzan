using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Accounting;

namespace AccSqlServerPersistence;

public class ContextHolder : IDatabaseManager, IDisposable, INpDisposable
{
	private SemaphoreSlim _freeContextSemaphore = new SemaphoreSlim(1, 1);

	private bool _isDisposed;

	private readonly ConcurrentDictionary<string, BaseModelContext> _dic = new ConcurrentDictionary<string, BaseModelContext>();

	private readonly ConcurrentDictionary<string, BaseModelContext> _dicContextsWithoutTransaction = new ConcurrentDictionary<string, BaseModelContext>();

	private readonly SemaphoreSlim _getSemaphore = new SemaphoreSlim(1, 1);

	public int _totalactiveContexts;

	internal readonly ConcurrentDictionary<string, string> _dicActiveContextsCallerNames = new ConcurrentDictionary<string, string>();

	public EnCacheConfig Config { get; private set; }

	public WebContextHolder Context { get; set; }

	public string ConnectionString { get; private set; }

	public short MaxLen { get; private set; }

	public bool IsDisposed => _isDisposed;

	public ContextHolder(EnCacheConfig config, string connectionString, WebContextHolder context)
	{
		Config = config;
		ConnectionString = connectionString;
		Context = context;
	}

	public async Task<ReturnedSaveFuncInfo> RollBackTransactionAsync(string tranName, bool disposeContext, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new NullReferenceException("tranName is null!!");
		}
		using (new JaegerMonitor("RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			BaseModelContext value = null;
			try
			{
				if (!_dic.TryRemove(tranName, out value))
				{
					throw new ArgumentException("RllbackContext not exists!!('" + tranName + "')");
				}
				if (value?.IsDisposed ?? true)
				{
					throw new ArgumentException("Model is null or disposed!! \r\nsourceFilePath: " + sourceFilePath + " \r\nmemberName: " + memberName + " \r\n" + $"LineNumber: {sourceLineNumber}");
				}
				if (value != null)
				{
					((DbContext)value).Database.RollbackTransaction();
				}
				await value.DisposeAsync("RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs");
			}
			catch (ArgumentException ex)
			{
				ret.AddReturnedValue(ex, "", "RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 70);
				WebErrorLog.Instance.StartLog(ex, "", "RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 71);
				throw;
			}
			catch (Exception ex2)
			{
				ret.AddReturnedValue(ex2, "", "RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 76);
				WebErrorLog.Instance.StartLog(ex2, $"File:{sourceFilePath}////memberName:{memberName}////Line:{sourceLineNumber}", "RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 77);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> BeginTransactionAsync(string tranName, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (!_dicActiveContextsCallerNames.ContainsKey(tranName))
		{
			string exceptionMessage = "";
			if (_dic.Count > 100)
			{
				exceptionMessage = $"Transactions Has Been Overflow \r\n TransactionCount:{_dic.Count} \r\n" + "✅DebugInfo: " + SendDebugInfo(_totalactiveContexts);
			}
			await UpdateContextSemaphoreAsync(tranName, sourceFilePath, memberName, sourceLineNumber, SendDebugInfo(_totalactiveContexts));
			if (!string.IsNullOrEmpty(exceptionMessage))
			{
				throw new Exception(exceptionMessage);
			}
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs"))
		{
			_ = 1;
			try
			{
				if (!_dic.TryGetValue(tranName, out var value))
				{
					string debugerMessage = SendDebugInfo(_totalactiveContexts);
					value = ContextFactory.GetInstance(Config, ConnectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber);
					value.OnDisposed += Model_OnDisposed;
					_dic.TryAdd(tranName, value);
				}
				if (!string.IsNullOrEmpty(ConnectionString))
				{
					await ((DbContext)value).Database.BeginTransactionAsync(default(CancellationToken));
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.ToLower().Contains("max pool size was reached".ToLower()))
				{
					SqlConnection.ClearAllPools();
				}
				WebErrorLog.Instance.StartLog(ex, "", "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 114);
				ret.AddReturnedValue(ex, "", "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 115);
			}
			return ret;
		}
	}

	private string SendDebugInfo(int contextCount)
	{
		string text = "";
		try
		{
			Dictionary<string, short> dictionary = new Dictionary<string, short>();
			if ((_dic?.Keys?.Any()).GetValueOrDefault())
			{
				foreach (string key3 in _dic.Keys)
				{
					if (_dic.TryGetValue(key3, out var value))
					{
						string key = value?.GetCallerName() ?? "Context is null";
						dictionary.TryGetValue(key, out var value2);
						dictionary.Remove(key);
						value2++;
						dictionary.Add(key, value2);
					}
				}
			}
			text = "هیچ تراکنش بازی درحال حاضر موجود نیست \r\n لطفا در Persistence های Get دنبال Dispose نشدن Context بگردید";
			if ((_dicContextsWithoutTransaction?.Keys?.Any()).GetValueOrDefault())
			{
				foreach (string key4 in _dicContextsWithoutTransaction.Keys)
				{
					if (_dicContextsWithoutTransaction.TryGetValue(key4, out var value3))
					{
						string callerName = value3.GetCallerName();
						dictionary.TryGetValue(callerName, out var value4);
						dictionary.Remove(callerName);
						value4++;
						dictionary.Add(callerName, value4);
					}
				}
			}
			if ((_dicActiveContextsCallerNames?.Keys?.Any()).GetValueOrDefault())
			{
				foreach (string key5 in _dicActiveContextsCallerNames.Keys)
				{
					if (_dicActiveContextsCallerNames.TryGetValue(key5, out var value5))
					{
						string key2 = value5;
						dictionary.TryGetValue(key2, out var value6);
						dictionary.Remove(key2);
						value6++;
						dictionary.Add(key2, value6);
					}
				}
			}
			foreach (string key6 in dictionary.Keys)
			{
				dictionary.TryGetValue(key6, out var value7);
				text += $"\ud83d\udccc #Caller ({key6}) - #Count =>{value7}\r\n";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendDebugInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 174);
		}
		return text;
	}

	public async Task<ReturnedSaveFuncInfo> CommitTransactionAsync(string tranName, bool disposeContext, string memberName, string sourceFilePath, int sourceLineNumber, string debugInfo = "")
	{
		using (new JaegerMonitor("CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			BaseModelContext context = null;
			if (string.IsNullOrEmpty(tranName))
			{
				throw new NullReferenceException("Transaction has been Empty !!!\r\n#memberName:" + memberName + "\r\n#sourceFilePath:" + sourceFilePath + "\r\n" + $"#sourceLineNumber:{sourceLineNumber}");
			}
			try
			{
				if (!_dic.TryRemove(tranName, out context))
				{
					throw new Exception("CommitContext!!!!!! error context not exists('" + tranName + "//ClassName:" + sourceFilePath + "//FuncName:" + memberName + ")");
				}
				if (!disposeContext)
				{
					_dic.TryAdd(tranName, context);
				}
				try
				{
					await context.SaveChangesAsync(default(CancellationToken), debugInfo);
				}
				catch (Exception ex)
				{
					ret.AddReturnedValue(ex, "", "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 197);
					WebErrorLog.Instance.StartLog(ex, "ClassName:" + sourceFilePath + "//FuncName:" + memberName, "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 198);
				}
				if (disposeContext && !ret.HasError && ((DbContext)context).Database.CurrentTransaction != null)
				{
					((DbContext)context).Database.CommitTransaction();
					RelationalDatabaseFacadeExtensions.CloseConnection(((DbContext)context).Database);
				}
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"File:{sourceFilePath}////memberName:{memberName}////Line:{sourceLineNumber}", "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 213);
				ret.AddReturnedValue(ex2, "", "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 214);
			}
			finally
			{
				if (ret.HasError)
				{
					_dic.TryAdd(tranName, context);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await RollBackTransactionAsync(tranName, disposeContext, memberName, sourceFilePath, sourceLineNumber), ReturnedState.None, "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 221);
					if (ret.HasError)
					{
						WebErrorLog.Instance.StartLog("RolBackTran:\r\n" + ret.ToString(), "", "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 223);
					}
				}
				else if (disposeContext && context != null)
				{
					await context.DisposeAsync("CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs");
				}
			}
			return ret;
		}
	}

	public BaseModelContext GetContext(string tranName, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		int num = 0;
		string text = "";
		if (!_dicActiveContextsCallerNames.ContainsKey(tranName))
		{
			if (_dic.Count > 100)
			{
				text = $"Transactions Has Been Overflow \r\n TransactionCount:{_dic.Count} \r\n" + "✅DebugInfo: " + SendDebugInfo(_totalactiveContexts);
			}
			UpdateContextSemaphoreAsync(tranName, sourceFilePath, memberName, sourceLineNumber, SendDebugInfo(_totalactiveContexts));
			if (!string.IsNullOrEmpty(text))
			{
				throw new Exception(text);
			}
		}
		try
		{
			num = 1;
			if (string.IsNullOrEmpty(tranName))
			{
				num = 2;
				string text2 = Guid.NewGuid().ToString();
				BaseModelContext instance = ContextFactory.GetInstance(Config, ConnectionString, text2, timeout, SendDebugInfo(_totalactiveContexts), memberName, sourceFilePath, sourceLineNumber);
				instance.OnDisposed += Model_OnDisposed;
				_dicContextsWithoutTransaction.TryAdd(text2, instance);
				num = 3;
				RelationalDatabaseFacadeExtensions.OpenConnection(((DbContext)instance).Database);
				num = 4;
				return instance;
			}
			num = 5;
			if (_dic.TryGetValue(tranName, out var value))
			{
				return value;
			}
			num = 7;
			throw new ArgumentException("TranName(" + tranName + ") doese not exists on dictionary!");
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, $"Line:{num}", "GetContext", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 268);
			throw;
		}
	}

	public async Task<BaseModelContext> GetContextAsync(string tranName, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (!_dicActiveContextsCallerNames.ContainsKey(tranName))
		{
			string exceptionMessage = "";
			if (_dic.Count > 100)
			{
				exceptionMessage = $"Transactions Has Been Overflow \r\n TransactionCount:{_dic.Count} \r\n" + "✅DebugInfo: " + SendDebugInfo(_totalactiveContexts);
			}
			await UpdateContextSemaphoreAsync(tranName, sourceFilePath, memberName, sourceLineNumber, SendDebugInfo(_totalactiveContexts));
			if (!string.IsNullOrEmpty(exceptionMessage))
			{
				throw new Exception(exceptionMessage);
			}
		}
		int line = 0;
		using SemaphoreUtil dis = await _getSemaphore.GaurdAsync(false, sourceFilePath + "/" + memberName, timeout, default(CancellationToken));
		if (dis.Token.IsCancellationRequested)
		{
			throw new TimeoutException("take async contextHolder semaphore timed out after 30 seconds");
		}
		try
		{
			line = 1;
			if (string.IsNullOrEmpty(tranName))
			{
				line = 2;
				string text = Guid.NewGuid().ToString();
				BaseModelContext model = ContextFactory.GetInstance(Config, ConnectionString, text, timeout, SendDebugInfo(_totalactiveContexts), memberName, sourceFilePath, sourceLineNumber);
				model.OnDisposed += Model_OnDisposed;
				_dicContextsWithoutTransaction.TryAdd(text, model);
				line = 3;
				await RelationalDatabaseFacadeExtensions.OpenConnectionAsync(((DbContext)model).Database, default(CancellationToken));
				line = 4;
				return model;
			}
			line = 5;
			if (_dic.TryGetValue(tranName, out var value))
			{
				return value;
			}
			line = 7;
			throw new ArgumentException("TranName(" + tranName + ") doese not exists on dictionary!");
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, $"Line:{line}", "GetContextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 314);
			throw;
		}
	}

	private async Task UpdateContextSemaphoreAsync(string tranName, string sourceFilePath, string memberName, int sourceLineNumber, string debugInfo)
	{
		try
		{
			if (!_dicActiveContextsCallerNames.ContainsKey(tranName))
			{
				_totalactiveContexts++;
				_dicActiveContextsCallerNames.TryAdd(tranName, $"{{{sourceFilePath}}}.{{{memberName}}} #Line_{sourceLineNumber}");
				if (_totalactiveContexts > 100)
				{
					string message = $"#ActiveContext_Has_Been_Overflow \r\n #ActiveContextCount:{_totalactiveContexts}\r\n #MemberName: {memberName} \r\n #ClassName: {sourceFilePath} \r\n #DebugInfo: {debugInfo}";
					await FreeContextsAsync();
					throw new InvalidOperationException(message);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateContextSemaphoreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 335);
		}
	}

	private async Task FreeContextsAsync()
	{
		_ = 1;
		try
		{
			await _freeContextSemaphore.WaitAsync();
			foreach (string key in _dicContextsWithoutTransaction.Keys)
			{
				if (_dicContextsWithoutTransaction.TryRemove(key, out var value) && value != null)
				{
					await value.DisposeAsync("FreeContextsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs");
				}
			}
			_totalactiveContexts = 0;
			_dicActiveContextsCallerNames.Clear();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FreeContextsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 349);
		}
		finally
		{
			_freeContextSemaphore.Release();
		}
	}

	private Task Model_OnDisposed(object sender, string transactionName)
	{
		try
		{
			BaseModelContext obj = (BaseModelContext)sender;
			_totalactiveContexts--;
			_dic.TryRemove(transactionName, out var value);
			_dicContextsWithoutTransaction.TryRemove(transactionName, out value);
			_dicActiveContextsCallerNames.TryRemove(transactionName, out var _);
			obj.OnDisposed -= Model_OnDisposed;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Model_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 368);
		}
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public Task<ReturnedSaveFuncInfoWithValue<bool>> ExistsAsync(string tranName)
	{
		BaseModelContext value;
		return Task.FromResult(new ReturnedSaveFuncInfoWithValue<bool>
		{
			value = _dic.TryGetValue(tranName, out value)
		});
	}

	public async Task<AccGlobalSettings> GetLocalSettingAsync(string contextGuid)
	{
		return await LocalSettingBusiness.GetSettingAsync(contextGuid);
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			_isDisposed = true;
			WebErrorLog.Instance.Information("this not an exception/// Context holder has been disposed!!", "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 381);
			foreach (BaseModelContext item in _dic?.Values ?? new List<BaseModelContext>())
			{
				if (item != null)
				{
					item?.DisposeAsync("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs");
				}
			}
			Context = null;
			ConnectionString = "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ContextHolder.cs", 390);
		}
	}
}
