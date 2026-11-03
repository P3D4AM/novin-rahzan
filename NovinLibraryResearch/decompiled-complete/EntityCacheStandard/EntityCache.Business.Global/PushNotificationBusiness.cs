using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace EntityCache.Business.Global;

public class PushNotificationBusiness : Serializable<PushNotificationBusiness>, IPushNotification, IHasJustGuid, IHasStatus, IHasContext
{
	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	public Guid Guid { get; set; }

	public string Message { get; set; }

	public string Title { get; set; }

	public EnPushNotificationType Type { get; set; }

	public short DisplayCount { get; set; }

	public ENSource Source { get; set; }

	public DateTime Date { get; set; }

	public DateTime ExpireDate { get; set; }

	public DateTime ModifiedDate { get; set; }

	public string ContextGuid { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string HelperUri { get; set; }

	public PushNotificationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<PushNotificationBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.PushNotification.GetAllAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, IEnumerable<PushNotificationBusiness> lstBussiness)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			List<PushNotificationBusiness> list = lstBussiness.ToList();
			if (!list.Any())
			{
				return ret;
			}
			foreach (PushNotificationBusiness item in list)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 46);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(10, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 47);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 52);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 53);
		}
		return ret;
	}

	public static async Task<IEnumerable<PushNotificationBusiness>> GetActiveNotificationAsync(string contextGuid, ENSource source, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.PushNotification.GetActiveNotifcation(source, token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public static async Task<DateTime> GetLastModifiedDateAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.PushNotification.GetLastModifiedDate(token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool tranactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				if (autoTran)
				{
					tranName = Guid.NewGuid().ToString();
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 82), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 82);
				}
				tranactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.PushNotification.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 84);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 88);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 89);
			}
		}
		finally
		{
			if (autoTran && tranactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 94), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\PushNotificationBusiness.cs", 94);
			}
		}
		return ret;
	}
}
