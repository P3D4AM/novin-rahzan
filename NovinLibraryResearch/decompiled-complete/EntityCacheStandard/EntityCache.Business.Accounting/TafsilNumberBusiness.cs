using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using Serilog;

namespace EntityCache.Business.Accounting;

public class TafsilNumberBusiness : Serializable<TafsilNumberBusiness>, ITafsilNumbers, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _getSemaphore = new SemaphoreSlim(1, 20);

	public Guid TafsilGuid { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public long OriginalNumber { get; set; }

	public string Title { get; set; }

	[JsonIgnore]
	public short Prefix
	{
		get
		{
			return new WebTafsilNumbers(OriginalNumber).Prefix;
		}
		set
		{
		}
	}

	[JsonIgnore]
	public bool Mobile => new WebTafsilNumbers(OriginalNumber).Mobile;

	[JsonIgnore]
	public long CalcedNumber
	{
		get
		{
			return new WebTafsilNumbers(OriginalNumber).CalcedNumber;
		}
		set
		{
		}
	}

	[JsonIgnore]
	public string TitleWithNumber => "(" + Title + " : " + OriginalNumber + ")";

	public string ContextGuid { get; set; }

	public byte[] EditImage => ImageResourceManager.EditImage;

	public byte[] DeleteImage => ImageResourceManager.OrderDeleteImage;

	public short Ordinal { get; set; }

	[JsonIgnore]
	public bool IsOddOrdinal => Ordinal % 2 != 0;

	public TafsilNumberBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<List<TafsilNumberBusiness>> GetAsync(string contextGuid, Guid tafsilGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs"))
		{
			bool semaphoreHasBeenTaken = false;
			try
			{
				semaphoreHasBeenTaken = await _getSemaphore.WaitAsync(60000);
				return await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetAllAsync(tafsilGuid, token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 50);
				throw;
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_getSemaphore.Release();
				}
			}
		}
	}

	public static async Task<List<TafsilNumberReportBusiness>> GetAllReportAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetAllReportAsync(token, tranName);
	}

	public static async Task<List<TafsilNumberBusiness>> GetInfoByCalcNumberAsync(string contextGuid, long number, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetInfoByCalcNumberAsync(number, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, string contextGuid, List<TafsilNumberBusiness> lst, Guid tafsilGuid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", bool AutoRemoveDuplicateNumbers = false)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		int line = 1;
		checked
		{
			try
			{
				_ = 4;
				ReturnedSaveFuncInfo result;
				try
				{
					using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilNumberBusiness/SaveRangeAsync", 60000, token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 69);
					}
					else
					{
						lst = lst?.Where((TafsilNumberBusiness p) => p.OriginalNumber > 0)?.ToList();
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 73), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 72);
						}
						line = 2;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Remove(contextGuid, tafsilGuid, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 75);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (lst == null || !lst.Any())
							{
								goto IL_0619;
							}
							line = 3;
							var list = (from p in lst
								group p by p.OriginalNumber into @group
								select new
								{
									number = @group.Key,
									count = @group.Count()
								} into p
								where p.count > 1
								select p)?.ToList();
							line = 4;
							if (list == null || !list.Any())
							{
								goto IL_050c;
							}
							if (AutoRemoveDuplicateNumbers)
							{
								foreach (var item in list)
								{
									for (int i = 0; i < item.count - 2; i++)
									{
										lst.Remove(lst.FirstOrDefault((TafsilNumberBusiness p) => p.OriginalNumber == item.number));
										line = 6;
									}
								}
								line = 7;
								goto IL_050c;
							}
							result = new ReturnedSaveFuncInfo(new Exception("شماره تکراری در لیست".GetTranslate() + " : " + string.Join(",", list.Select(p => p.number))), "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 96);
						}
					}
					goto end_IL_0123;
					IL_0619:
					line = 82;
					if (autoTran)
					{
						line = 83;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 111), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 111);
					}
					goto end_IL_006c;
					IL_050c:
					foreach (TafsilNumberBusiness item2 in lst)
					{
						item2.TafsilGuid = tafsilGuid;
					}
					line = 80;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.SaveRangeAsync(lst.ToList(), tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 103);
					line = 81;
					goto IL_0619;
					end_IL_0123:;
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 114);
					goto end_IL_006c;
				}
				catch (Exception ex)
				{
					ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 117);
					if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ITafsilNumberRepository == null)
					{
						Log.Warning(ex, $"line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository is null!!  The lst IS{lst == null}");
					}
					else
					{
						WebErrorLog.Instance.StartLog(ex, $"line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository is not null.  The lst IS{lst == null}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 121);
					}
					goto end_IL_006c;
				}
				return result;
				end_IL_006c:;
			}
			finally
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 126), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 126);
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilNumberBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 139);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 140), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 140);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITafsilNumberRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 142);
						if (!ret.HasError)
						{
							goto end_IL_011b;
						}
						result = ret;
					}
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 147);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 148);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 153), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 153);
			}
		}
		return ret;
	}

	public static async Task<List<TafsilNumberBusiness>> GetInfoAsync(string contextGuid, long number, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			List<TafsilNumberBusiness> res1 = (await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetInfoAsync(number, status, token, tranName)) ?? new List<TafsilNumberBusiness>();
			if (number > 9999999 && !res1.Any())
			{
				WebTafsilNumbers webTafsilNumbers = new WebTafsilNumbers(number);
				if (webTafsilNumbers.Mobile)
				{
					return res1;
				}
				List<TafsilNumberBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetInfoByCalcNumberAsync(webTafsilNumbers.CalcedNumber, token, tranName);
				if (list != null && list.Any())
				{
					foreach (TafsilNumberBusiness item in list)
					{
						if (res1.SingleOrDefault((TafsilNumberBusiness p) => item.TafsilGuid == p.TafsilGuid) == null)
						{
							res1.Add(item);
						}
					}
				}
			}
			return res1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 176);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> Remove(string contextGuid, List<Guid> tafsilGuids, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilNumberBusiness/Remove", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						line = 1;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoCommit)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 192), ReturnedState.None, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 192);
						}
						transactionHasBeenCreated = true;
						line = 2;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.RemoveAsync(tafsilGuids, tryCount, token, tranName), ReturnedState.None, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 195);
						goto end_IL_0119;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 190);
					goto end_IL_007d;
					end_IL_0119:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"removeList/Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(contextGuid)?.DatabaseManager == null}", "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 199);
				ret.AddReturnedValue(ex, "", "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 200);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoCommit && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 205), ReturnedState.None, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 205);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> Remove(string contextGuid, Guid tafsilGuid, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilNumberBusiness/Remove", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						line = 1;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoCommit)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 221), ReturnedState.None, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 221);
						}
						transactionHasBeenCreated = true;
						line = 2;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.RemoveAsync(tafsilGuid, tryCount, token, tranName), ReturnedState.None, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 224);
						goto end_IL_0119;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 219);
					goto end_IL_007d;
					end_IL_0119:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ITafsilNumberRepository == null)
				{
					Log.Warning(ex, $"remove single/Line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository disposed:{ClsCache.GetCache(contextGuid)?.CacheRepository?.ITafsilNumberRepository == null}");
					throw;
				}
				WebErrorLog.Instance.StartLog(ex, $"remove single/Line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository disposed:{ClsCache.GetCache(contextGuid)?.CacheRepository?.ITafsilNumberRepository == null}", "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 234);
				ret.AddReturnedValue(ex, "", "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 235);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoCommit && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 240), ReturnedState.None, "Remove", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 240);
			}
		}
		return ret;
	}

	public static Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid tafsilGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.RemoveAsync(tafsilGuid, tryCount, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid tafsilGuid, long number, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			try
			{
				if ((await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilNumberBusiness/Remove", 60000, token)).Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 254);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoCommit)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 255), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 255);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.RemoveAsync(tafsilGuid, number, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 257);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 261);
				throw;
			}
		}
		finally
		{
			if (autoCommit && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 267), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 267);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(List<TafsilNumberBusiness> tafsilNumbers, EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (tafsilNumbers != null && tafsilNumbers.Any((TafsilNumberBusiness c) => c.Guid != Guid && c.OriginalNumber == OriginalNumber))
				{
					ret.AddReturnedValue(ReturnedState.Error, "شماره وارد شده تکراری است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 278);
				}
				List<TafsilNumberBusiness> list = await GetInfoAsync(ContextGuid, OriginalNumber, true, "", token);
				if (list != null && list.Count((TafsilNumberBusiness p) => p.TafsilGuid != TafsilGuid) > 0)
				{
					foreach (TafsilNumberBusiness item in list)
					{
						string text = (await TafsilBusiness.GetAsync(ContextGuid, system, item.TafsilGuid, tranName, token, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs"))?.Name;
						ret.AddReturnedValue(ReturnedState.Error, "شماره ".GetTranslate() + $"{OriginalNumber} " + "قبلا برای ".GetTranslate() + " " + text + "ثبت شده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 284);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 289);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 290);
			}
			finally
			{
				if (ret.HasError && system != EnSystem.UserInterface)
				{
					WebErrorLog.Instance.StartLog("TafsilNumber Validation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 295);
				}
			}
			return ret;
		}
	}

	public static Task<List<string>> GetTitleListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetTitleListAsync(token, tranName);
	}

	public static Task<List<TafsilNumberBusiness>> GetAllAsync(string contextGuid, Guid tafsilGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetAllAsync(tafsilGuid, token, tranName);
	}

	public string NumberForDisplay()
	{
		try
		{
			if (Mobile)
			{
				return $"0{CalcedNumber}";
			}
			if (OriginalNumber > 0)
			{
				return OriginalNumber.ToString();
			}
			if (CalcedNumber > 0)
			{
				return CalcedNumber.ToString();
			}
			return "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NumberForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 312);
			throw;
		}
	}

	public static async Task<List<TafsilNumberBusiness>> GetAllAsync(string contextGuid, List<Guid> guids, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (!(guids?.Any() ?? false))
			{
				return null;
			}
			if (guids.Count == 1)
			{
				return await GetAsync(contextGuid, guids.Single(), token, tranName);
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilNumberRepository.GetAllAsync(guids, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TafsilNumberBusiness.cs", 327);
			throw;
		}
	}
}
