using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Business.Advertise;

public class VisitorAdvBusiness : IVisitorAdv, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid VisitorGuid { get; set; }

	public Guid AdvGuid { get; set; }

	public string ContextGuid { get; set; }

	public VisitorAdvBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, IEnumerable<VisitorAdvBusiness> Lst, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (Lst == null || !Lst.Any())
			{
				return ret;
			}
			foreach (VisitorAdvBusiness item in Lst)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 33);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 37);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 38);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 50), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 50);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IVisitorAdv.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 52);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 56);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 57);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 62), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 62);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<VisitorAdvBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVisitorAdv.GetAllAsync(status, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveByVisitorGuidAsync(string contextGuid, Guid visitorGuid, short tryCount = 10, string tranName = null)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 78), ReturnedState.None, "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 78);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IVisitorAdv.RemoveByVisitorGuidAsync(visitorGuid, tryCount, tranName), ReturnedState.None, "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 80);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 84);
				ret.AddReturnedValue(ex, "", "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 85);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 90), ReturnedState.None, "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Advertise\\VisitorAdvBusiness.cs", 90);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<VisitorAdvBusiness>> GetAllAsync(string contextGuid, Guid visitorGuid, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVisitorAdv.GetAllAsync(visitorGuid, tranName);
	}
}
