using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Business;

public class ChangeIntegrity
{
	public static async Task<ReturnedSaveFuncInfo> UpdateAccountDatabase(string contextGuid, EnSystem system, bool isDeleteAction, IChangeIntegrity item, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "", bool rollBack = false, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		using (new JaegerMonitor("UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 6;
				ReturnedSaveFuncInfo result;
				try
				{
					if (system == EnSystem.Transfer)
					{
						result = ret;
						goto end_IL_00ab;
					}
					if (autoCommit)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 28), ReturnedState.None, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 28);
					}
					transactionHasBeenCreated = true;
					List<MoeinChanges> list = await item.GetLstMoeinChangesAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave);
					if (list != null && list.Count > 0)
					{
						foreach (MoeinChanges item3 in list)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await MoeinBusiness.UpdateAccountAsync(contextGuid, item3.MoeinGuid, rollBack ? (-item3.Account) : item3.Account, tryCount, token, tranName), ReturnedState.None, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 34);
							if (!ret.HasError)
							{
								continue;
							}
							result = ret;
							goto end_IL_00ab;
						}
					}
					List<TafsilChanges> list2 = await item.GetLstTafsilChangesAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave);
					List<TafsilChanges> zeroTafsils = list2?.Where((TafsilChanges q) => q.Account == 0m)?.ToList();
					list2 = list2?.Where((TafsilChanges q) => q.Account != 0m)?.ToList();
					if (list2 != null && list2.Count > 0)
					{
						foreach (TafsilChanges item4 in list2)
						{
							if (item4.TafsilGuid != Guid.Empty)
							{
								AccountBusiness item2 = new AccountBusiness(contextGuid)
								{
									Modified = DateTime.Now,
									MoeinGuid = item4.MoeinGuid,
									ServerStatus = ServerStatusForClient.None,
									Status = true,
									TafsilGuid = item4.TafsilGuid,
									Account = (rollBack ? (-item4.Account) : item4.Account),
									Guid = Guid.NewGuid()
								};
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Account.BallanceAccountAsync(item2, tryCount, tranName, token), ReturnedState.None, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 56);
							}
							if (!ret.HasError)
							{
								continue;
							}
							result = ret;
							goto end_IL_00ab;
						}
					}
					if (zeroTafsils != null && zeroTafsils.Count > 0)
					{
						foreach (TafsilChanges tafsil in zeroTafsils)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateServerStatusAsync(contextGuid, tafsil.TafsilGuid, ServerStatusForClient.None, token, tranName), ReturnedState.None, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 64);
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Account.UpdateServerStatusAsync(tafsil.TafsilGuid, ServerStatusForClient.None, token, tranName), ReturnedState.None, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 66);
								if (!ret.HasError)
								{
									continue;
								}
								result = ret;
							}
							goto end_IL_00ab;
						}
					}
					goto end_IL_0084;
					end_IL_00ab:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 72);
					ret.AddReturnedValue(ex, "", "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 73);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 78), ReturnedState.None, "UpdateAccountDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 78);
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateProductsDatabase(string contextGuid, EnSystem system, bool isDeleteAction, IChangeIntegrity item, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "", bool rollBack = false)
	{
		using (new JaegerMonitor("UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					if (system == EnSystem.Transfer)
					{
						result = ret;
						goto end_IL_00a2;
					}
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					List<ProductsChanges> products = new List<ProductsChanges>();
					line = 1;
					if (autoTran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 96), ReturnedState.None, "UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 96);
					}
					transactionHasBeenCreated = true;
					line = 2;
					List<ProductsChanges> list = await item.GetLstProductsAnbarChangesAsync(system, token, tranName, isDeleteAction);
					if (item != null && list != null && list.Any())
					{
						foreach (ProductsChanges prd in list ?? new List<ProductsChanges>())
						{
							if (prd == null)
							{
								continue;
							}
							line = 10;
							if (source != ENSource.WordPressDebug && source != ENSource.WebsiteWordPress)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.UpdateAmountAsync(contextGuid, prd, rollBack, tryCount, tranName, token), ReturnedState.None, "UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 107);
							}
							line = 12;
							if (!ret.HasError)
							{
								line = 14;
								products.Add(prd);
								continue;
							}
							result = ret;
							goto end_IL_00a2;
						}
					}
					goto end_IL_008b;
					end_IL_00a2:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null{ClsCache.GetCache(contextGuid)?.DatabaseManager == null}", "UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 117);
					ret.AddReturnedValue(ex, "", "UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 118);
					goto end_IL_008b;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 123), ReturnedState.None, "UpdateProductsDatabase", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ChangeIntegrity.cs", 123);
				}
			}
			return ret;
		}
	}
}
