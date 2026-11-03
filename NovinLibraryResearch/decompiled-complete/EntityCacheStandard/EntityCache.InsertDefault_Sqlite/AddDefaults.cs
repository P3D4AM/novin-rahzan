using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.InsertDefault_Sqlite;

public class AddDefaults
{
	public static async Task<ReturnedSaveFuncInfo> InsertDefaultDataAsync(string contextGuid, EnSystem system)
	{
		CancellationToken token = default(CancellationToken);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((ClsCache.GetCache(contextGuid) ?? throw new NullReferenceException("Cache is null!!")).CacheRepository == null)
			{
				throw new NullReferenceException("cache.CacheRepository is null!!");
			}
			List<HesabTypeBusiness> lstHesabsList = DefualtHesabTypeObjs.ListAllHesabTypeDefault(contextGuid);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HesabTypeBusiness.SaveRangeAsync(token, lstHesabsList, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\AddDefaults.cs", 24);
			if (ret.HasError)
			{
				ret.throwExceptionIfError();
			}
			List<HesabGroupBusiness> lstHesabsList2 = DefualtHesabGroupObjs.ListAllHesabGroup(contextGuid);
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HesabGroupBusiness.SaveRangeAsync(system, token, lstHesabsList2, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\AddDefaults.cs", 28);
			if (ret.HasError)
			{
				ret.throwExceptionIfError();
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\AddDefaults.cs", 33);
			WebErrorLog.Instance.StartLog(ex, "", "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\AddDefaults.cs", 34);
		}
		return ret;
	}
}
