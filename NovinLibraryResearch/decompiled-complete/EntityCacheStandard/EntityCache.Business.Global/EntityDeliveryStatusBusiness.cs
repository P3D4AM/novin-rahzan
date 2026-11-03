using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace EntityCache.Business.Global;

public class EntityDeliveryStatusBusiness : IEntityDeliveryStatus, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public EntityPart EntityPart { get; set; }

	public bool IsTemp { get; set; }

	public Guid EntityGuid { get; set; }

	public string ContextGuid { get; set; }

	public EntityDeliveryStatusBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "EntityDeliveryStatusBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 34);
					}
					else
					{
						ClsCache cache = ClsCache.GetCache(ContextGuid);
						if (cache == null)
						{
							result = res;
						}
						else
						{
							if (autoTran)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await cache.GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 37), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 37);
							}
							transactionHasBeenCreated = true;
							if (!res.HasError)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await cache.CacheRepository.EntityDeliveryStatus.SaveAsync(this, 0, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 40);
								goto end_IL_011b;
							}
							result = res;
						}
					}
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 44);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 45);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 50), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 50);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid entityGuid, EntityPart entityPart, bool isTemp, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "EntityDeliveryStatusBusiness/RemoveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 63);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 64), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 64);
						}
						transactionHasBeenCreated = true;
						if (!res.HasError)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.EntityDeliveryStatus.RemoveAsync(entityGuid, entityPart, isTemp, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 67);
							goto end_IL_0112;
						}
						result = res;
					}
					goto end_IL_0076;
					end_IL_0112:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 71);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 72);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 77), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\EntityDeliveryStatusBusiness.cs", 77);
			}
		}
		return res;
	}

	public static Task<IEnumerable<EntityDeliveryStatusBusiness>> GetAllAsync(string contextGuid, EntityPart entityPart, bool isTemp, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.EntityDeliveryStatus.GetAllAsync(entityPart, isTemp, token, tranName);
	}
}
