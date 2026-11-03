using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Interfaces;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using PacketParser.Filters;

namespace EntityCache.Business.Global;

public class SaveTempBusiness : ISaveTemp, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public static int Delay => 20000;

	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public DateTime DateM { get; set; }

	public int Id2 { get; set; }

	public string Description { get; set; }

	public byte[] SerialedData { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public SaveTempBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveOrderAsync(string contextGuid, EnSystem system, OrderBusiness order, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		try
		{
			if (order == null)
			{
				return res;
			}
			await order._modifiedDetListSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			SaveTempBusiness saveTempBusiness = new SaveTempBusiness(contextGuid)
			{
				AsGuid = order.Guid,
				AsType = order.FactorType,
				DateM = DateTime.Now,
				Guid = Guid.NewGuid()
			};
			SaveTempBusiness saveTempBusiness2 = saveTempBusiness;
			saveTempBusiness2.Id2 = await order.GetId2Async(system, "", token);
			saveTempBusiness.Description = order.Description;
			saveTempBusiness.Modified = DateTime.Now;
			saveTempBusiness.Status = true;
			SaveTempBusiness obj = saveTempBusiness;
			byte[] data = Json.ToBytesJson(await OrderMapper.Instance.MapAsync(contextGuid, order, new WebOrder(), "", token));
			saveTempBusiness = obj;
			saveTempBusiness.SerialedData = await GZip.Compress(data);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 66);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 70);
			res.AddReturnedValue(ex, "", "SaveOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 71);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				order?._modifiedDetListSemaphore?.Release();
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveReceptionAsync(string contextGuid, ReceptionBusiness reception)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken token = default(CancellationToken);
			if (reception == null)
			{
				return res;
			}
			SaveTempBusiness saveTempBusiness = new SaveTempBusiness(contextGuid)
			{
				AsGuid = reception.Guid,
				AsType = Document.Reception,
				DateM = DateTime.Now,
				Guid = Guid.NewGuid()
			};
			SaveTempBusiness saveTempBusiness2 = saveTempBusiness;
			saveTempBusiness2.Id2 = await reception.GetId2Async(token);
			saveTempBusiness.Description = reception.Description;
			saveTempBusiness.Modified = DateTime.Now;
			saveTempBusiness.Status = true;
			SaveTempBusiness obj = saveTempBusiness;
			byte[] data = Json.ToBytesJson(await ReceptionMapper.Instance.MapAsync(contextGuid, reception, new WebReception(), "", token));
			saveTempBusiness = obj;
			saveTempBusiness.SerialedData = await GZip.Compress(data);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 99);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 103);
			res.AddReturnedValue(ex, "", "SaveReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 104);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SavePardakhtAsync(string contextGuid, PardakhtBusiness pardakht, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (pardakht == null)
			{
				return res;
			}
			SaveTempBusiness saveTempBusiness = new SaveTempBusiness(contextGuid)
			{
				AsGuid = pardakht.Guid,
				AsType = Document.Pardakht,
				DateM = DateTime.Now,
				Guid = Guid.NewGuid()
			};
			SaveTempBusiness saveTempBusiness2 = saveTempBusiness;
			saveTempBusiness2.Id2 = await pardakht.GetId2Async(token);
			saveTempBusiness.Description = pardakht.Description;
			saveTempBusiness.Modified = DateTime.Now;
			saveTempBusiness.Status = true;
			SaveTempBusiness obj = saveTempBusiness;
			byte[] data = Json.ToBytesJson(await PardakhtMapper.Instance.MapAsync(contextGuid, pardakht, new WebPardakht(), "", token));
			saveTempBusiness = obj;
			saveTempBusiness.SerialedData = await GZip.Compress(data);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SavePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 129);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SavePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 133);
			res.AddReturnedValue(ex, "", "SavePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 134);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveSanadAsync(string contextGuid, SanadBusiness sanad)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken token = default(CancellationToken);
			if (sanad == null)
			{
				return res;
			}
			SaveTempBusiness obj = new SaveTempBusiness(contextGuid)
			{
				AsGuid = sanad.Guid,
				AsType = Document.Sanad,
				DateM = DateTime.Now,
				Guid = Guid.NewGuid(),
				Id2 = sanad.SanadNo,
				Description = sanad.Description,
				Modified = DateTime.Now,
				Status = true
			};
			byte[] data = Json.ToBytesJson(await SanadMapper.Instance.MapAsync(contextGuid, sanad, new WebSanad(), "", token));
			SaveTempBusiness saveTempBusiness = obj;
			saveTempBusiness.SerialedData = await GZip.Compress(data);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 161);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 165);
			res.AddReturnedValue(ex, "", "SaveSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 166);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAnbarGardaniAsync(string contextGuid, PrdAnbarGardaniBusiness anbar)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken token = default(CancellationToken);
			if (anbar == null)
			{
				return res;
			}
			SaveTempBusiness obj = new SaveTempBusiness(contextGuid)
			{
				AsGuid = anbar.Guid,
				AsType = Document.AnbarGardani,
				DateM = DateTime.Now,
				Guid = Guid.NewGuid(),
				Id2 = anbar.Number,
				Description = anbar.Description,
				Modified = DateTime.Now,
				Status = true
			};
			byte[] data = Json.ToBytesJson(await PrdAnbarGardaniMapper.Instance.MapAsync(contextGuid, anbar, new WebPrdAnbarGardani(), "", token));
			SaveTempBusiness saveTempBusiness = obj;
			saveTempBusiness.SerialedData = await GZip.Compress(data);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveAnbarGardaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 193);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAnbarGardaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 197);
			res.AddReturnedValue(ex, "", "SaveAnbarGardaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 198);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SavePardakhtAsync(string contextGuid, IBaseKhazane khazane, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (khazane == null)
			{
				return res;
			}
			SaveTempBusiness saveTempBusiness = new SaveTempBusiness(contextGuid)
			{
				AsGuid = khazane.Guid,
				DateM = DateTime.Now,
				Guid = Guid.NewGuid()
			};
			SaveTempBusiness saveTempBusiness2 = saveTempBusiness;
			saveTempBusiness2.Id2 = await khazane.GetId2Async(token);
			saveTempBusiness.Description = khazane.Description;
			saveTempBusiness.Modified = DateTime.Now;
			saveTempBusiness.Status = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SavePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 229);
			res.AddReturnedValue(ex, "", "SavePardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 230);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "SaveTempBusinesss/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 244), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 244);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.SaveTemp.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 246);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 243);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 248);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 251);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 252);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 257), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 257);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<SaveTempViewModel>> GetAllAsync(string contextGuid, SaveTempFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (ClsCache.GetCache(contextGuid) ?? throw new NullReferenceException("cache with contextGuid(" + contextGuid + ") is null")).CacheRepository.SaveTemp.GetAllAsync(filter, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(string contextGuid, Guid asGuid, Document asType, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 275), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 275);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.SaveTemp.RemoveRangeAsync(asGuid, asType, token, tranName), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 277);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 281);
				res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 282);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 287), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 287);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 299), ReturnedState.None, "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 299);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.SaveTemp.RemoveRangeByDateAsync(token, tranName), ReturnedState.None, "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 301);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 305);
				res.AddReturnedValue(ex, "", "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 306);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 311), ReturnedState.None, "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SaveTempBusiness.cs", 311);
			}
		}
		return res;
	}

	public static Task<SaveTempBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.SaveTemp.GetAsync(guid, token, tranName);
	}
}
