using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Accounting;

namespace AccSqlServerPersistence;

public class SignalRContextHolder : IDatabaseManager, IDisposable, INpDisposable
{
	private bool _isDisposed;

	private IContextHolderSignalRClient _service;

	public EnCacheConfig Config { get; private set; }

	public WebContextHolder Context { get; set; }

	public bool IsDisposed => _isDisposed;

	public SignalRContextHolder(EnCacheConfig config, WebContextHolder context)
	{
		try
		{
			Config = config;
			Context = context;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 37);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RollBackTransactionAsync(string tranName, bool disposeContext, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _service.RollBackTransactionAsync(Config, tranName, disposeContext), ReturnedState.None, "RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 45);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 49);
			res.AddReturnedValue(ex, "", "RollBackTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 50);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> BeginTransactionAsync(string tranName, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _service.BeginTransactionAsync(Config, tranName, timeout), ReturnedState.None, "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 59);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 63);
			res.AddReturnedValue(ex, "", "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 64);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> CommitTransactionAsync(string tranName, bool disposeContext, string memberName, string sourceFilePath, int sourceLineNumber, string debugInfo = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _service.CommitTransactionAsync(Config, tranName, disposeContext), ReturnedState.None, "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 73);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 77);
			res.AddReturnedValue(ex, "", "CommitTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 78);
		}
		return res;
	}

	public Task<BaseModelContext> GetContextAsync(string tranName, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public async Task<AccGlobalSettings> GetLocalSettingAsync(string contextGuid)
	{
		return await LocalSettingBusiness.GetSettingAsync(contextGuid);
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				_service.DisposeAsync();
				Context = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\SignalRContextHolder.cs", 97);
		}
	}

	public Task<ReturnedSaveFuncInfoWithValue<bool>> ExistsAsync(string tranName)
	{
		return _service.ExistsAsync(Config, tranName);
	}
}
