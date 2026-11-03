using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting;

public class MoeinTafsilGroupRelationsBusiness : IMoeinTafsilGroupRelations, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid MoeinGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public string ContextGuid { get; set; }

	public EnMoeinTafsilRelationLevel Level { get; set; } = EnMoeinTafsilRelationLevel.One;


	public MoeinTafsilGroupRelationsBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<MoeinTafsilGroupRelationsBusiness>> GetAllAsync(string contextGuid, Guid? moeinGuid, Guid? tafsilGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IMoeinTafsilGroupRelations.GetAllAsync(moeinGuid, tafsilGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineTafsilGroupRelationBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 39), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 39);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IMoeinTafsilGroupRelations.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 41);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 38);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 45);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 46);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 51), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 51);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveMoeinRelationsAsync(string contextGuid, Guid moeinGuid, List<Guid> lstTafsilLevelGuid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = null, CancellationToken token = default(CancellationToken))
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
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineTafsilGroupRelationBusiness/SaveRelationAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 64);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 65), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 65);
						}
						transactionHasBeenCreated = true;
						IEnumerable<MoeinTafsilGroupRelationsBusiness> enumerable = await GetAllAsync(contextGuid, moeinGuid, null);
						if (enumerable != null && enumerable.Any())
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await RemoveAsync(contextGuid, moeinGuid, null, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 69);
						}
						if (!ret.HasError)
						{
							if (lstTafsilLevelGuid != null)
							{
								foreach (Guid item2 in lstTafsilLevelGuid)
								{
									if (item2 == Guid.Empty)
									{
										continue;
									}
									MoeinTafsilGroupRelationsBusiness item = new MoeinTafsilGroupRelationsBusiness(contextGuid)
									{
										MoeinGuid = moeinGuid,
										TafsilGroupGuid = item2
									};
									ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoeinTafsilGroupRelations.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 81);
									if (!ret.HasError)
									{
										continue;
									}
									result = ret;
									goto end_IL_0092;
								}
							}
							goto end_IL_013a;
						}
						result = ret;
					}
					goto end_IL_0092;
					end_IL_013a:;
				}
				goto end_IL_0081;
				end_IL_0092:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 87);
				ret.AddReturnedValue(ex, "", "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 88);
				goto end_IL_0081;
			}
			return result;
			end_IL_0081:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 93), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 93);
			}
			if (ClsCache.GetCache(contextGuid).BufferEnable && ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar)
			{
				Task.Run(() => MoeinBusiness.ResendNotSentsAsync(contextGuid, default(CancellationToken), "", 20));
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveTafsilRelationsAsync(string contextGuid, Guid tafsilGroupGuid, List<Guid> lstMoeinGuid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = null, CancellationToken token = default(CancellationToken))
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
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineTafsilGroupRelationBusiness/SaveRelationAsync", 60000, token))
				{
					line = 1;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 111);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 112), ReturnedState.None, "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 112);
						}
						transactionHasBeenCreated = true;
						line = 2;
						IEnumerable<MoeinTafsilGroupRelationsBusiness> enumerable = await GetAllAsync(contextGuid, null, tafsilGroupGuid, token);
						line = 3;
						if (enumerable != null && enumerable.Any())
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await RemoveAsync(contextGuid, null, tafsilGroupGuid, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 118);
						}
						line = 4;
						if (!ret.HasError)
						{
							line = 5;
							if (lstMoeinGuid != null)
							{
								foreach (Guid item2 in lstMoeinGuid)
								{
									line = 6;
									if (item2 == Guid.Empty)
									{
										continue;
									}
									line = 7;
									MoeinTafsilGroupRelationsBusiness item = new MoeinTafsilGroupRelationsBusiness(contextGuid)
									{
										MoeinGuid = item2,
										TafsilGroupGuid = tafsilGroupGuid
									};
									line = 8;
									ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoeinTafsilGroupRelations.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 134);
									line = 9;
									if (!ret.HasError)
									{
										continue;
									}
									result = ret;
									goto end_IL_0099;
								}
							}
							goto end_IL_0141;
						}
						result = ret;
					}
					goto end_IL_0099;
					end_IL_0141:;
				}
				goto end_IL_0088;
				end_IL_0099:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 141);
				ret.AddReturnedValue(ex, "", "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 142);
				goto end_IL_0088;
			}
			return result;
			end_IL_0088:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 147), ReturnedState.None, "SaveTafsilRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 147);
			}
			if (ClsCache.GetCache(contextGuid).BufferEnable && ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar)
			{
				Task.Run(() => MoeinBusiness.ResendNotSentsAsync(contextGuid, default(CancellationToken), "", 20));
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveMoeinRelationsAsync(string contextGuid, Guid moeinGuid, List<MoeinTafsilGroupRelationsBusiness> lstTafsilLevelGuid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = null, CancellationToken token = default(CancellationToken))
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
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineTafsilGroupRelationBusiness/SaveRelationAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 163);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 164), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 164);
						}
						transactionHasBeenCreated = true;
						IEnumerable<MoeinTafsilGroupRelationsBusiness> enumerable = await GetAllAsync(contextGuid, moeinGuid, null, token);
						if (enumerable != null && enumerable.Any())
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await RemoveAsync(contextGuid, moeinGuid, null, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 168);
						}
						if (!ret.HasError)
						{
							if (lstTafsilLevelGuid != null)
							{
								foreach (MoeinTafsilGroupRelationsBusiness item in lstTafsilLevelGuid)
								{
									ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoeinTafsilGroupRelations.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 174);
									if (!ret.HasError)
									{
										continue;
									}
									result = ret;
									goto end_IL_0092;
								}
							}
							goto end_IL_013a;
						}
						result = ret;
					}
					goto end_IL_0092;
					end_IL_013a:;
				}
				goto end_IL_0081;
				end_IL_0092:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 180);
				ret.AddReturnedValue(ex, "", "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 181);
				goto end_IL_0081;
			}
			return result;
			end_IL_0081:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 186), ReturnedState.None, "SaveMoeinRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 186);
			}
			if (ClsCache.GetCache(contextGuid).BufferEnable && ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar)
			{
				Task.Run(() => MoeinBusiness.ResendNotSentsAsync(contextGuid, default(CancellationToken), "", 20));
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid? moeinGuid = null, Guid? tafsilGroupGuid = null, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineTafsilGroupRelationBusiness/RemoveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 203), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 203);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoeinTafsilGroupRelations.RemoveAsync(moeinGuid, tafsilGroupGuid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 205);
						goto end_IL_012e;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 202);
					goto end_IL_0092;
					end_IL_012e:;
				}
				goto end_IL_0081;
				end_IL_0092:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 209);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 210);
				goto end_IL_0081;
			}
			return result;
			end_IL_0081:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 215), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 215);
			}
			if (ClsCache.GetCache(contextGuid).BufferEnable && ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar)
			{
				Task.Run(() => MoeinBusiness.ResendNotSentsAsync(contextGuid, default(CancellationToken), "", 20));
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<MoeinTafsilGroupRelationsBusiness> list, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			int current = 0;
			if (list == null || !list.Any())
			{
				return ret;
			}
			foreach (MoeinTafsilGroupRelationsBusiness item in list)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 233);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 234);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(list.Count(), current, "", token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 244);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinTafsilGroupRelationsBusiness.cs", 245);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(list?.Count() ?? 0, list?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}
}
