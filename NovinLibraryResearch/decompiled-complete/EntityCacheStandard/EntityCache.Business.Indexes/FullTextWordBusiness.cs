using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Indexes;

public class FullTextWordBusiness : IFullTextWord, IHasContext, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private bool _forceStop;

	private static BaseLocks _indexingSemaphore = new BaseLocks();

	private TaskCompletionSource<bool> forceUpdateIndex;

	private ENSource? source;

	public int Id { get; set; }

	public string Word { get; set; }

	public string ContextGuid { get; set; }

	public ENSource Source
	{
		get
		{
			if (string.IsNullOrEmpty(ContextGuid))
			{
				throw new ArgumentException("ContextGuid");
			}
			ENSource valueOrDefault = source.GetValueOrDefault();
			if (!source.HasValue)
			{
				valueOrDefault = ClsCache.GetCache(ContextGuid).Source;
				source = valueOrDefault;
			}
			return source.Value;
		}
		set
		{
			source = value;
		}
	}

	public bool IsDisposed => _isDisposed;

	public FullTextWordBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task UpdateFullTextSearchIndex(CancellationToken token)
	{
		await Task.Delay(60000);
		while (!IsDisposed && !_forceStop)
		{
			try
			{
				if (ClsCache.GetCache(ContextGuid) == null)
				{
					await Task.Delay(10000);
					if (IsDisposed || ClsCache.GetCache(ContextGuid) == null)
					{
						break;
					}
				}
				EnProductIndex mode = await GetIndexingModeAsync();
				HashSet<Guid> _lst = await ProductBusiness.GetAllForIndexingAsync(count: (Source != ENSource.Android_HesabYar) ? (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Products.CountForIndexing : 10, contextGuid: ContextGuid, mode: mode, token: token);
				forceUpdateIndex = new TaskCompletionSource<bool>();
				if (token.IsCancellationRequested || _forceStop)
				{
					break;
				}
				if ((await UpdateFullTextSearchIndex(_lst, mode, "", token)).HasError)
				{
					await Task.Delay(60000);
				}
				if (!(_lst?.Any() ?? false))
				{
					await Task.WhenAny(new Task[2]
					{
						Task.Delay(60000),
						forceUpdateIndex.Task
					});
				}
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 76);
				await Task.Delay(60000);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> UpdateFullTextSearchIndex(HashSet<Guid> lst, EnProductIndex indexMode, string tranName = "", CancellationToken tokenInput = default(CancellationToken), int timeOut = 60000)
	{
		using (new JaegerMonitor("UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			bool semaphoreHasBeenTaken = false;
			Guid identity = default(Guid);
			SemaphoreSlim semapahore = await _indexingSemaphore.GetSemaphoreAsync(ContextGuid);
			try
			{
				int num;
				_ = num - 1;
				_ = 6;
				try
				{
					await semapahore.WaitAsync();
					semaphoreHasBeenTaken = true;
					ClsCache cache = ClsCache.GetCache(ContextGuid);
					CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(tokenInput, new CancellationTokenSource(timeOut).Token).Token;
					Dictionary<Guid, HashSet<int>> NameAndCodeFieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> AllFieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> NameFieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> CodeFieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> DescFieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T1FieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T2FieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T3FieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T4FieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T5FieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T6FieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T7FieldItems = new Dictionary<Guid, HashSet<int>>();
					Dictionary<Guid, HashSet<int>> T8FieldItems = new Dictionary<Guid, HashSet<int>>();
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await cache.AccDatabaseManager.BeginTransactionAsync(tranName, timeOut, "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 115), ReturnedState.None, "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 115);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await cache.AccDatabaseManager.CommitTransactionAsync(tranName, disposedContext: false, "webRepo", "updateFulText...", 1), ReturnedState.None, "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 117);
					if (token.IsCancellationRequested || _forceStop)
					{
						return ret.AddErrorTokenIsCancelled("", "", "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 118);
					}
					IEnumerable<ProductForIndexingModel> productList = await ProductBusiness.GetAllForIndexingAsync(ContextGuid, lst, token, tranName);
					List<string> words = productList.SelectMany((ProductForIndexingModel p) => p.FullTextIndexOnAllFields())?.Distinct()?.ToList();
					Dictionary<string, int> existsOnDbWords = await cache.CacheRepository.IFullTextWordRepository.GetOrSaveAsync(words, token, tranName);
					List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
					foreach (Guid guid in lst)
					{
						identity = guid;
						ProductForIndexingModel productForIndexingModel = productList.SingleOrDefault((ProductForIndexingModel q) => q.Guid == guid) ?? throw new NullReferenceException("prd is null!!");
						if ((indexMode & EnProductIndex.NameAndCode) == EnProductIndex.NameAndCode)
						{
							FillIndexs(NameAndCodeFieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.FullTextIndexOnName_CodeFields());
						}
						if ((indexMode & EnProductIndex.AllFields) == EnProductIndex.AllFields)
						{
							FillIndexs(AllFieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.FullTextIndexOnAllFields());
						}
						if ((indexMode & EnProductIndex.Name) == EnProductIndex.Name)
						{
							FillIndexs(NameFieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.Name.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.Name) == EnProductIndex.Name && !string.IsNullOrEmpty(productForIndexingModel.NameForDisplay))
						{
							FillIndexs(NameFieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.NameForDisplay.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.Code) == EnProductIndex.Code)
						{
							FillIndexs(CodeFieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.IdProduct?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.Description) == EnProductIndex.Description)
						{
							FillIndexs(DescFieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.Description?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T1) == EnProductIndex.T1)
						{
							FillIndexs(T1FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T1?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T2) == EnProductIndex.T2)
						{
							FillIndexs(T2FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T2?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T3) == EnProductIndex.T3)
						{
							FillIndexs(T3FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T3?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T4) == EnProductIndex.T4)
						{
							FillIndexs(T4FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T4?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T5) == EnProductIndex.T5)
						{
							FillIndexs(T5FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T5?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T6) == EnProductIndex.T6)
						{
							FillIndexs(T6FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T6?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T7) == EnProductIndex.T7)
						{
							FillIndexs(T7FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T7?.PrepareForIndexing());
						}
						if ((indexMode & EnProductIndex.T8) == EnProductIndex.T8)
						{
							FillIndexs(T8FieldItems, productForIndexingModel.Guid, existsOnDbWords, productForIndexingModel.T8?.PrepareForIndexing());
						}
					}
					if ((indexMode & EnProductIndex.NameAndCode) == EnProductIndex.NameAndCode)
					{
						list.Add(cache.CacheRepository.IIxProductNameAndCodeFieldRepository.SaveListAsync(NameAndCodeFieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.AllFields) == EnProductIndex.AllFields)
					{
						list.Add(cache.CacheRepository.IIxProductAllFiels.SaveListAsync(AllFieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.Name) == EnProductIndex.Name)
					{
						list.Add(cache?.CacheRepository.IIxProductNameFieldRepository.SaveListAsync(NameFieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.Code) == EnProductIndex.Code)
					{
						list.Add(cache.CacheRepository.IIxProductCodeFieldRepository.SaveListAsync(CodeFieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.Description) == EnProductIndex.Description)
					{
						list.Add(cache.CacheRepository.IIxProductDescFieldRepository.SaveListAsync(DescFieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T1) == EnProductIndex.T1)
					{
						list.Add(cache.CacheRepository.IIxProductT1FieldRepository.SaveListAsync(T1FieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T2) == EnProductIndex.T2)
					{
						list.Add(cache.CacheRepository.IIxProductT2FieldRepository.SaveListAsync(T2FieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T3) == EnProductIndex.T3)
					{
						list.Add(cache.CacheRepository.IIxProductT3FieldRepository.SaveListAsync(T3FieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T4) == EnProductIndex.T4)
					{
						list.Add(cache.CacheRepository.IIxProductT4FieldRepository.SaveListAsync(T4FieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T5) == EnProductIndex.T5)
					{
						list.Add(cache.CacheRepository.IIxProductT5FieldRepository.SaveListAsync(T5FieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T6) == EnProductIndex.T6)
					{
						list.Add(cache.CacheRepository.IIxProductT6FieldRepository.SaveListAsync(T6FieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T7) == EnProductIndex.T7)
					{
						list.Add(cache.CacheRepository.IIxProductT7FieldRepository.SaveListAsync(T7FieldItems, tranName, token));
					}
					if ((indexMode & EnProductIndex.T8) == EnProductIndex.T8)
					{
						list.Add(cache.CacheRepository.IIxProductT8FieldRepository.SaveListAsync(T8FieldItems, tranName, token));
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 187);
					if (!ret.HasError)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.UpdateIndexTypeAsync(ContextGuid, lst, indexMode, token, tranName), ReturnedState.None, "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 189);
					}
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 191);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"\ud83d\udd11#guid: {identity}", "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 194);
					ret.AddReturnedValue(ex, "", "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 195);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 200), ReturnedState.None, "UpdateFullTextSearchIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 200);
				}
				if (semaphoreHasBeenTaken && !IsDisposed)
				{
					semapahore.Release();
				}
				if (ret.HasError && (lst?.Count ?? (-1)) > 1)
				{
					await Task.Delay(new Random().Next(300000));
					ret = await RetryIndexingBySplitAsync(lst, indexMode);
				}
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> RetryIndexingBySplitAsync(HashSet<Guid> lst, EnProductIndex indexMode)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			HashSet<Guid> hashSet = new HashSet<Guid>();
			HashSet<Guid> l2 = new HashSet<Guid>();
			int num = 0;
			foreach (Guid item in lst)
			{
				num = checked(num + 1);
				if (num <= lst.Count / 2)
				{
					hashSet.Add(item);
					continue;
				}
				break;
			}
			foreach (Guid item2 in lst.Except(hashSet).ToList())
			{
				l2.Add(item2);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await UpdateFullTextSearchIndex(hashSet, indexMode), ReturnedState.None, "RetryIndexingBySplitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 227);
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await UpdateFullTextSearchIndex(l2, indexMode), ReturnedState.None, "RetryIndexingBySplitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 228);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RetryIndexingBySplitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 232);
			ret.AddReturnedValue(ex, "", "RetryIndexingBySplitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 233);
		}
		return ret;
	}

	private static void FillIndexs(Dictionary<Guid, HashSet<int>> indexingItems, Guid guid, Dictionary<string, int> existsOnDbWords, List<string> wordForIndexing)
	{
		try
		{
			if (wordForIndexing != null && wordForIndexing.Any())
			{
				HashSet<int> hashSet = new HashSet<int>();
				foreach (string item in wordForIndexing)
				{
					if (existsOnDbWords.TryGetValue(item, out var value))
					{
						hashSet.Add(value);
						continue;
					}
					throw new InvalidOperationException();
				}
				if (indexingItems.TryGetValue(guid, out var _))
				{
					indexingItems.Remove(guid);
				}
				indexingItems.Add(guid, hashSet);
			}
			else
			{
				indexingItems.Add(guid, null);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillIndexs", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 257);
		}
	}

	public void ForceUpdateFullTextSearchIndex()
	{
		forceUpdateIndex?.TrySetResult(result: true);
	}

	private Task<EnProductIndex> GetIndexingModeAsync()
	{
		try
		{
			if (Source == ENSource.Android_HesabYar)
			{
				return Task.FromResult(EnProductIndex.NameAndCode);
			}
			return Task.FromResult((EnProductIndex)4095 | EnProductIndex.T8);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetIndexingModeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 282);
			throw;
		}
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Indexes\\FullTextWordBusiness.cs", 293);
		}
	}

	public void ForceStop()
	{
		_forceStop = true;
	}
}
