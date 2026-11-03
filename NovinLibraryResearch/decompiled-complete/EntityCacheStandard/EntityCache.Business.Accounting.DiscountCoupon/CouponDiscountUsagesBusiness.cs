using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace EntityCache.Business.Accounting.DiscountCoupon;

public class CouponDiscountUsagesBusiness : ICouponDiscountUsages, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid TafsilGuid { get; set; }

	public Guid CouponGuid { get; set; }

	public string ContextGuid { get; set; }

	public CouponDiscountUsagesBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	internal static Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllAsync(string contexdGuid, Guid couponGuid, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contexdGuid).CacheRepository.CouponDiscountUsages.GetAllAsync(couponGuid, token, tranName);
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<CouponDiscountUsagesBusiness> list, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		try
		{
			_ = 2;
			try
			{
				if (!(list?.Any() ?? false))
				{
					return res;
				}
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "CouponDiscountUsagesBusiness/SaveRangeAsync", 6000, token);
				if (dis.Token.IsCancellationRequested)
				{
					res.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 38);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autotran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 39), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 39);
				}
				trantionBeenCreate = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscountUsages.SaveRangeAsync(list, 1, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 41);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 45);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 46);
			}
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 51), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountUsagesBusiness.cs", 51);
			}
		}
		return res;
	}
}
