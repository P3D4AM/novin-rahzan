using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Customer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting;

public class TafsilSelectedBusiness : Serializable<TafsilSelectedBusiness>, ITafsilSelected, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IHasChangeStatus
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private List<TafsilSelectedDetListBusiness> _detList;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string Code { get; set; } = "";


	public string Title { get; set; } = "";


	[JsonIgnore]
	public int SubItem => DetList?.Count ?? 0;

	public short? Type { get; set; }

	public Guid UGuid { get; set; }

	public string Description { get; set; } = "";


	public IReadOnlyList<TafsilSelectedDetListBusiness> DetList
	{
		get
		{
			_detList = _detList?.Where((TafsilSelectedDetListBusiness p) => p != null)?.ToList() ?? new List<TafsilSelectedDetListBusiness>();
			return _detList;
		}
	}

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public TafsilSelectedBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<TafsilSelectedBusiness>> GetAllAsync(string contextGuid, TafsilSelectedFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilSelectedList.GetAllAsync(filter, token, tranName);
	}

	public static Task<TafsilSelectedBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilSelectedList.GetAsync(guid, token, tranName);
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilSelectedListBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 59), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 59);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 61);
						ret.throwExceptionIfError();
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITafsilSelectedList.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 63);
						goto end_IL_0121;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 58);
					goto end_IL_007d;
					end_IL_0121:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 67);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 68);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 73), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 73);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilSelectedBusiness> list, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (TafsilSelectedBusiness item in list)
			{
				current = checked(current + 1);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 88);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(list.Count(), current, item.Title, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 100);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 101);
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

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (Guid == Guid.Empty)
			{
				await res.AddErrorAsync("خطای Guid  تهی", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 120);
			}
			if (string.IsNullOrEmpty(Title))
			{
				await res.AddErrorAsync("عنوان را پر کنید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 121);
			}
			if (!(DetList?.Any() ?? false))
			{
				await res.AddErrorAsync("لیست بدون شخص نمی تواند ثبت شود", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 122);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 126);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 127);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilSelectedBusiness/RemoveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 141), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 141);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilSelectedList.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 143);
						goto end_IL_0112;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 140);
					goto end_IL_0076;
					end_IL_0112:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 147);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 148);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 153), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 153);
			}
		}
		return ret;
	}

	public void RemoveFromDetListAsync(TafsilSelectedDetListBusiness item = null, Guid? guid = null)
	{
		using (new JaegerMonitor("RemoveFromDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs"))
		{
			try
			{
				if (item != null)
				{
					TafsilSelectedDetListBusiness tafsilSelectedDetListBusiness = _detList?.FirstOrDefault((TafsilSelectedDetListBusiness p) => p.Guid == item.Guid);
					if (tafsilSelectedDetListBusiness != null)
					{
						_detList.Remove(tafsilSelectedDetListBusiness);
					}
				}
				else if (guid.HasValue)
				{
					TafsilSelectedDetListBusiness tafsilSelectedDetListBusiness2 = _detList?.FirstOrDefault(delegate(TafsilSelectedDetListBusiness p)
					{
						Guid guid2 = p.Guid;
						Guid? guid3 = guid;
						return guid2 == guid3;
					});
					if (tafsilSelectedDetListBusiness2 != null)
					{
						_detList.Remove(tafsilSelectedDetListBusiness2);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 175);
				throw;
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> AddToDetListAsync(TafsilSelectedDetListBusiness item)
	{
		using (new JaegerMonitor("AddToDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				if (item == null)
				{
					return Task.FromResult(returnedSaveFuncInfo);
				}
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				if (_detList == null)
				{
					_detList = new List<TafsilSelectedDetListBusiness>();
				}
				if (_detList.All((TafsilSelectedDetListBusiness p) => p.Guid != item.Guid && p.HGuid != item.HGuid))
				{
					item.MasterGuid = Guid;
					_detList.Add(item);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 196);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "AddToDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 197);
			}
			return Task.FromResult(returnedSaveFuncInfo);
		}
	}

	public Task<ReturnedSaveFuncInfo> AddRangeToDetListAsync(List<TafsilSelectedDetListBusiness> detList)
	{
		using (new JaegerMonitor("AddRangeToDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs"))
		{
			ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
			try
			{
				if (detList == null || !detList.Any())
				{
					return Task.FromResult(result);
				}
				if (_detList == null)
				{
					_detList = new List<TafsilSelectedDetListBusiness>();
				}
				foreach (TafsilSelectedDetListBusiness det in detList)
				{
					det.MasterGuid = Guid;
				}
				_detList.AddRange(detList);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedListBusiness.cs", 215);
				throw;
			}
			return Task.FromResult(result);
		}
	}
}
