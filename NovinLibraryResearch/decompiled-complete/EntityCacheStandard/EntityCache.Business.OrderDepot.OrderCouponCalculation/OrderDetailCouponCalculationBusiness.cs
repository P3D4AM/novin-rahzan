using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order;

namespace EntityCache.Business.OrderDepot.OrderCouponCalculation;

public class OrderDetailCouponCalculationBusiness : IOrderDetailCouponCalculation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal RowSum { get; set; }

	public Guid? DicountCouponDetailGuid { get; set; }

	public int RefrencedId { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public OrderDetailCouponCalculationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<int>> SaveAsync(string contextGuid, IEnumerable<OrderDetailCouponCalculationBusiness> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			using (await _databaseSemaphore.GaurdAsync(true, "OrderDetailCouponCalculationBusiness/SaveAsync", 60000, token))
			{
				ReturnedSaveFuncInfoWithValue<int> result;
				try
				{
					int num;
					_ = num - 1;
					_ = 2;
					try
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue;
						if (autoTran)
						{
							returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 42), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 42);
						}
						transactionHasBeenCreated = true;
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue2 = await GetNextRefrencedAsync(contextGuid, token, tranName);
						returnedSaveFuncInfoWithValue2.throwExceptionIfError();
						if (returnedSaveFuncInfoWithValue2.value <= 0)
						{
							throw new ArgumentException($"RefrenecdId from StimulQuery is {returnedSaveFuncInfoWithValue2.value}");
						}
						ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 47);
						ret.value = returnedSaveFuncInfoWithValue2.value;
						if (ret.HasError)
						{
							result = ret;
							goto IL_0551;
						}
						foreach (OrderDetailCouponCalculationBusiness item in lst)
						{
							item.RefrencedId = returnedSaveFuncInfoWithValue2.value;
						}
						if (!(lst?.Any() ?? false))
						{
							result = ret;
							goto IL_0551;
						}
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.OrderDetailCouponCalculation.SaveRangeAsync(lst, 10, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 52);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 56);
						ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 57);
					}
				}
				finally
				{
					if (autoTran && transactionHasBeenCreated)
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 62), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderDetailCouponCalculationBusiness.cs", 62);
					}
				}
				return ret;
				IL_0551:
				return result;
			}
		}
	}

	private static Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrencedAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.OrderDetailCouponCalculation.GetNextRefrencedAsync(token, tranName);
	}
}
