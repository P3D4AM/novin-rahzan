using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace EntityCache.Business.Global;

public class TempBarcodePrintBusiness : ITempBarcodePrint, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid DetailGuid { get; set; }

	public string Barcode { get; set; }

	public string GuarantyCode { get; set; }

	public Document AsType { get; set; }

	public string MasterDescription { get; set; }

	public string DetailDescription { get; set; }

	public string TempDescription { get; set; }

	public DateTime MasterDate { get; set; }

	public DateTime? GuarantyStartDate { get; set; }

	public DateTime? GuarantyEndDate { get; set; }

	public string ContextGuid { get; set; }

	public TempBarcodePrintBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, IEnumerable<TempBarcodePrintBusiness> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
			if (!(lst?.Any() ?? false))
			{
				return res;
			}
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			using (await _databaseSemaphore.GaurdAsync(true, "TempBarcodePrintBusiness/SaveRangeAsync", 60000, token))
			{
				ReturnedSaveFuncInfo result;
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
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 50), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 50);
						}
						if (res.HasError)
						{
							result = res;
							goto IL_051f;
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.TempBarcode.RemoveRangeAsync(lst.First().MasterGuid, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 53);
						if (res.HasError)
						{
							result = res;
							goto IL_051f;
						}
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.TempBarcode.SaveRangeAsync(lst, 1, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 55);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 59);
						res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 60);
					}
				}
				finally
				{
					if (autoTran && transactionHasBeenCreated)
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 65), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\TempBarcodePrintBusiness.cs", 65);
					}
				}
				return res;
				IL_051f:
				return result;
			}
		}
	}
}
