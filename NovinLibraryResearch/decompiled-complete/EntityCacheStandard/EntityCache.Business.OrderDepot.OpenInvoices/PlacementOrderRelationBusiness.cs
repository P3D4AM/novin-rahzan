using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

namespace EntityCache.Business.OrderDepot.OpenInvoices;

public class PlacementOrderRelationBusiness : IPlacementOrderRelation, IHasContext
{
	private static SemaphoreSlim databaseSemaphor = new SemaphoreSlim(1, 1);

	public Guid PlacementGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public string ContextGuid { get; set; }

	public PlacementOrderRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<PlacementOrderRelationReportBusiness>> GetAllAsync(string contextGuid, Guid? placementGuid, Guid? orderGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.PlacementOrderRelation.GetAllModelAsync(placementGuid, orderGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<PlacementOrderRelationBusiness> list, bool awaitOnSemaphore, CancellationToken token = default(CancellationToken), string tranName = "")
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
				using SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(awaitOnSemaphore, "PlacementOrderRelationBusiness/SaveRangeAsync", 6000, token);
				if (dis.Token.IsCancellationRequested)
				{
					res.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 36);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autotran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 37), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 37);
				}
				trantionBeenCreate = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.PlacementOrderRelation.SaveRangeAsync(list, 1, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 39);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 43);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 44);
			}
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 49), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 49);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(string contextGuid, List<PlacementOrderRelationBusiness> list, bool awaitOnSemaphore, CancellationToken token = default(CancellationToken), string tranName = "")
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
				using SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(awaitOnSemaphore, "PlacementOrderRelationBusiness/RemoveRangeAsync", 6000, token);
				if (dis.Token.IsCancellationRequested)
				{
					res.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 62);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autotran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 63), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 63);
				}
				trantionBeenCreate = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.PlacementOrderRelation.RemoveRangeAsync(list, token, tranName), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 65);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 69);
				res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 70);
			}
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 75), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 75);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(string contextGuid, Guid orderGuid, bool awaitOnSemaphore, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(awaitOnSemaphore, "PlacementOrderRelationBusiness/RemoveRangeAsync", 6000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 88);
					}
					if (autotran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 89), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 89);
					}
					trantionBeenCreate = true;
					IEnumerable<PlacementOrderRelationReportBusiness> enumerable = await GetAllAsync(contextGuid, null, orderGuid, token, tranName);
					if (enumerable != null && enumerable.Any())
					{
						List<PlacementOrderRelationBusiness> list = new List<PlacementOrderRelationBusiness>();
						foreach (PlacementOrderRelationReportBusiness item in enumerable)
						{
							list.Add(new PlacementOrderRelationBusiness(contextGuid)
							{
								ContextGuid = contextGuid,
								OrderGuid = orderGuid,
								PlacementGuid = item.PlacementGuid
							});
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.PlacementOrderRelation.RemoveRangeAsync(list, token, tranName), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 101);
						goto end_IL_011a;
					}
					result = res;
					goto end_IL_0076;
					end_IL_011a:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 105);
				res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 106);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 111), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\PlacementOrderRelationBusiness.cs", 111);
			}
		}
		return res;
	}
}
