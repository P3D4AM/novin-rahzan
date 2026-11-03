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
using PacketParser.Entities.Interfaces.Accounting.Selection;

namespace EntityCache.Business.Accounting.Selection.ProductGroup;

public class PrdGroupSelectedListBusiness : IPrdGroupSelected, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private List<PrdGroupSelectedDetListBusiness> _detList;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public string Code { get; set; }

	public string Title { get; set; }

	public Guid CashierGuid { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	[JsonIgnore]
	public int SubItem => DetList?.Count ?? 0;

	public IReadOnlyList<PrdGroupSelectedDetListBusiness> DetList
	{
		get
		{
			_detList = _detList?.Where((PrdGroupSelectedDetListBusiness p) => p != null)?.ToList() ?? new List<PrdGroupSelectedDetListBusiness>();
			return _detList;
		}
	}

	public PrdGroupSelectedListBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public void RemoveFromDetList(PrdGroupSelectedDetListBusiness item = null, Guid? guid = null)
	{
		using (new JaegerMonitor("RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs"))
		{
			try
			{
				if (item != null)
				{
					PrdGroupSelectedDetListBusiness prdGroupSelectedDetListBusiness = _detList?.FirstOrDefault((PrdGroupSelectedDetListBusiness p) => p.Guid == item.Guid);
					if (prdGroupSelectedDetListBusiness != null)
					{
						_detList.Remove(prdGroupSelectedDetListBusiness);
					}
				}
				else if (guid.HasValue)
				{
					PrdGroupSelectedDetListBusiness prdGroupSelectedDetListBusiness2 = _detList?.FirstOrDefault(delegate(PrdGroupSelectedDetListBusiness p)
					{
						Guid guid2 = p.Guid;
						Guid? guid3 = guid;
						return guid2 == guid3;
					});
					if (prdGroupSelectedDetListBusiness2 != null)
					{
						_detList.Remove(prdGroupSelectedDetListBusiness2);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 65);
				throw;
			}
		}
	}

	public void AddToDetList(PrdGroupSelectedDetListBusiness item)
	{
		using (new JaegerMonitor("AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs"))
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
						_detList = new List<PrdGroupSelectedDetListBusiness>();
					}
					if (_detList.All((PrdGroupSelectedDetListBusiness p) => p.Guid != item.Guid && p.GetGroupGuid() != item.GetGroupGuid()))
					{
						item.MasterGuid = Guid;
						_detList.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 85);
				throw;
			}
		}
	}

	public void AddRangeToDetList(List<PrdGroupSelectedDetListBusiness> detList)
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs"))
		{
			try
			{
				if (detList == null || detList.Count <= 0)
				{
					return;
				}
				if (_detList == null)
				{
					_detList = new List<PrdGroupSelectedDetListBusiness>();
				}
				foreach (PrdGroupSelectedDetListBusiness det in detList)
				{
					det.MasterGuid = Guid;
				}
				_detList.AddRange(detList);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 103);
				throw;
			}
		}
	}

	public static Task<IEnumerable<PrdGroupSelectedListBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductGroupSelected.GetAllAsync(token, tranName);
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdGroupSelectedListBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 120), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 120);
						}
						transactionHasBeenCreated = true;
						ret.AddReturnedValue(CheckValidation(system), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 122);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ProductGroupSelected.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 123);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 119);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 128);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 129);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 134), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 134);
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
				returnedSaveFuncInfo.AddError(system, "خطای Guid  تهی", "Guid", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 144);
			}
			if (string.IsNullOrEmpty(Title))
			{
				returnedSaveFuncInfo.AddError(system, "عنوان را پر کنید", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 145);
			}
			if (DetList == null || DetList.Count < 1)
			{
				returnedSaveFuncInfo.AddError(system, "لیست بدون گروه کالا نمی تواند ثبت شود", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 147);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 151);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 152);
		}
		return returnedSaveFuncInfo;
	}

	public static Task<PrdGroupSelectedListBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductGroupSelected.GetAsync(guid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				CancellationToken token = default(CancellationToken);
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdGroupSelectedListBusiness/RemoveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 170), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 170);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ProductGroupSelected.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 172);
						goto end_IL_011e;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 169);
					goto end_IL_0076;
					end_IL_011e:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 176);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 177);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 182), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListBusiness.cs", 182);
			}
		}
		return ret;
	}
}
