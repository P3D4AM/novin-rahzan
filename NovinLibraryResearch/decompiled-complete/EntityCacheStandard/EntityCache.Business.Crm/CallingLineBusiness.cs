using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;

namespace EntityCache.Business.Crm;

public class CallingLineBusiness : ICallingLine, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string ComputerName { get; set; }

	public short DeviceNumber { get; set; }

	public short LineNumber { get; set; }

	public string Number { get; set; } = "";


	public bool IsCo { get; set; }

	public Guid CashierGuid { get; set; }

	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public async Task<string> GetCashierNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await CashierBusiness.GetAsync(ContextGuid, CashierGuid, token, tranName))?.Name ?? "";
	}

	private CallingLineBusiness()
	{
	}

	public static Task<CallingLineBusiness> FactoryAsync(string contextGuid)
	{
		return Task.FromResult(new CallingLineBusiness
		{
			ContextGuid = contextGuid,
			CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
		});
	}

	public static async Task<CallingLineBusiness> GetAsync(string contextGuid, string computerName, short deviceNumber, short lineNumber, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		CallingLineBusiness ret = await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLine.GetAsync(computerName, deviceNumber, lineNumber, token, tranName);
		try
		{
			if (ret != null)
			{
				return ret;
			}
			if (deviceNumber == 0 && lineNumber == 0)
			{
				throw new Exception($"☺\ufe0f☺\ufe0f☺\ufe0f☺\ufe0f \r\n LineNumber:{lineNumber}\r\n DeviceNumber:{deviceNumber}\r\n ☺\ufe0f☺\ufe0f☺\ufe0f☺\ufe0f");
			}
			ret = await FactoryAsync(contextGuid);
			ret.IsCo = true;
			ret.ComputerName = computerName;
			ret.DeviceNumber = deviceNumber;
			ret.LineNumber = lineNumber;
			ret.Guid = Guid.NewGuid();
			ret.Status = true;
			await ret.SaveAsync(token, awaitOnSemaphore: true, 10);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 64);
			throw;
		}
		return ret;
	}

	public static Task<CallingLineBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (guid == Guid.Empty)
			{
				return Task.FromResult<CallingLineBusiness>(null);
			}
			return ClsCache.GetCache(contextGuid).CacheRepository.ICallingLine.GetAsync(guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 80);
			return Task.FromResult<CallingLineBusiness>(null);
		}
	}

	private static Task<IEnumerable<CallingLineBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICallingLine.GetAllAsync(token, tranName);
	}

	public static async Task<List<CallingLineBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, bool? status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<CallingLineBusiness> source = await GetAllAsync(contextGuid, token);
				search.SplitString();
				return token.IsCancellationRequested ? null : source.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 107);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CallingLineBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 123), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 123);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICallingLine.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 125);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 122);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 130);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 131);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 136), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 136);
			}
		}
		return ret;
	}

	public static async Task<CallingLineBusiness> GetAsync(string contextGuid, string number, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ICallingLine.GetAsync(number, token, tranName);
	}

	public static async Task<CallingLineBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? guid = null, string description = "", string tranName = "")
	{
		CallingLineBusiness callingLineBusiness;
		try
		{
			callingLineBusiness = await FactoryAsync(contextGuid);
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				callingLineBusiness.Guid = guid.Value;
			}
			else
			{
				callingLineBusiness.Guid = Guid.NewGuid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Crm\\Calling\\CallingLineBusiness.cs", 162);
			throw;
		}
		return callingLineBusiness;
	}
}
