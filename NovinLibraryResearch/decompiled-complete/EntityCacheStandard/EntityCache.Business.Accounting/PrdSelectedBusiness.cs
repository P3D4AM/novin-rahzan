using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting;

public class PrdSelectedBusiness : IPrdSelected, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IHasChangeStatus
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private List<PrdSelectedDetListBusiness> _detList;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string Code { get; set; } = "";


	public string Title { get; set; } = "";


	[JsonIgnore]
	public int SubItem => DetList?.Count ?? 0;

	public short? Type { get; set; }

	public Guid UserGuid { get; set; }

	public string Description { get; set; } = "";


	public IReadOnlyList<PrdSelectedDetListBusiness> DetList
	{
		get
		{
			_detList = _detList?.Where((PrdSelectedDetListBusiness p) => p != null)?.ToList() ?? new List<PrdSelectedDetListBusiness>();
			return _detList;
		}
	}

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public PrdSelectedBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public void RemoveFromDetList(PrdSelectedDetListBusiness item = null, Guid? guid = null)
	{
		using (new JaegerMonitor("RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs"))
		{
			try
			{
				if (item != null)
				{
					PrdSelectedDetListBusiness prdSelectedDetListBusiness = _detList?.FirstOrDefault((PrdSelectedDetListBusiness p) => p.Guid == item.Guid);
					if (prdSelectedDetListBusiness != null)
					{
						_detList.Remove(prdSelectedDetListBusiness);
					}
				}
				else if (guid.HasValue)
				{
					PrdSelectedDetListBusiness prdSelectedDetListBusiness2 = _detList?.FirstOrDefault(delegate(PrdSelectedDetListBusiness p)
					{
						Guid guid2 = p.Guid;
						Guid? guid3 = guid;
						return guid2 == guid3;
					});
					if (prdSelectedDetListBusiness2 != null)
					{
						_detList.Remove(prdSelectedDetListBusiness2);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 65);
				throw;
			}
		}
	}

	public void AddToDetList(PrdSelectedDetListBusiness item)
	{
		using (new JaegerMonitor("AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs"))
		{
			try
			{
				if (item != null)
				{
					if (item.Guid == Guid.Empty)
					{
						item.Guid = Guid.NewGuid();
					}
					if (_detList == null)
					{
						_detList = new List<PrdSelectedDetListBusiness>();
					}
					if (_detList.All((PrdSelectedDetListBusiness p) => p.Guid != item.Guid && p.GetPrdGuid() != item.GetPrdGuid()))
					{
						item.MasterGuid = Guid;
						_detList.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 86);
				throw;
			}
		}
	}

	public void AddRangeToDetList(List<PrdSelectedDetListBusiness> detList)
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs"))
		{
			try
			{
				if (detList == null || detList.Count <= 0)
				{
					return;
				}
				if (_detList == null)
				{
					_detList = new List<PrdSelectedDetListBusiness>();
				}
				foreach (PrdSelectedDetListBusiness det in detList)
				{
					det.MasterGuid = Guid;
				}
				_detList.AddRange(detList);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 105);
				throw;
			}
		}
	}

	public static Task<IEnumerable<PrdSelectedBusiness>> GetAllAsync(string contextGuid, string title = "", string description = "", CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSelectedList.GetAllAsync(title, description, token, tranName);
	}

	public static Task<PrdSelectedBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdSelectedList.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdSelectedListBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 125), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 125);
						}
						transactionHasBeenCreated = true;
						ret.AddReturnedValue(CheckValidation(system), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 127);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdSelectedList.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 128);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 124);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 133);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 134);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 139), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 139);
			}
		}
		return ret;
	}

	private ReturnedSaveFuncInfo CheckValidation(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (Guid == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError(system, "خطای Guid  تهی".GetTranslate(), "Guid", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 149);
			}
			if (string.IsNullOrEmpty(Title))
			{
				returnedSaveFuncInfo.AddError(system, "عنوان را پر کنید".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 150);
			}
			if (DetList == null || DetList.Count < 1)
			{
				returnedSaveFuncInfo.AddError(system, "لیست بدون کالا نمی تواند ثبت شود".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 152);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 156);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 157);
		}
		return returnedSaveFuncInfo;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				CancellationToken token = default(CancellationToken);
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdSelectedListBusiness/RemoveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 175), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 175);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdSelectedList.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 177);
						goto end_IL_011e;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 174);
					goto end_IL_0076;
					end_IL_011e:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 182);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 183);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 188), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 188);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<PrdSelectedBusiness> list, bool send2server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (PrdSelectedBusiness item in list)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 203);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 204);
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
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 216);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedListBusiness.cs", 217);
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
