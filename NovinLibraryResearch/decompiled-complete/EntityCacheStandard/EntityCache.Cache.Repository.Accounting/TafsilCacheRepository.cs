using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Events;
using PacketParser.Filters;
using PacketParser.Mapping;
using Serilog;

namespace EntityCache.Cache.Repository.Accounting;

public class TafsilCacheRepository : SyncCacheRepository<TafsilBusiness>, ITafsilRepository, ISyncRepository<TafsilBusiness>, IEditableDelivery, IRepository<TafsilBusiness>, INoGuidRepository<TafsilBusiness>, IDisposable
{
	public bool _isDisposed;

	private ITafsilRepository _dbRepository;

	private List<HesabType> _CacheType = new List<HesabType>();

	private readonly ConcurrentDictionary<Guid, HashSet<WeakReference>> _tafsils = new ConcurrentDictionary<Guid, HashSet<WeakReference>>();

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	protected override TimingCache<Guid, TafsilBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<TafsilBusiness, TafsilBusiness> Mapper => TafsilMapper.Instance;

	public TafsilCacheRepository(ITafsilRepository dbRepository, int CachingTime)
		: base((ISyncRepository<TafsilBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TafsilBusiness>(base.ContextGuid, Mapper, CachingTime);
		HandeleEventsAsync();
	}

	private async Task HandeleEventsAsync()
	{
		try
		{
			if (!string.IsNullOrEmpty(base.ContextGuid) && ClsCache.GetCache(base.ContextGuid).Source != ENSource.Android_HesabYar)
			{
				while (ClsCache.GetCache(base.ContextGuid)?.BusinessAggregator == null)
				{
					await Task.Delay(100);
				}
				ClsCache.GetCache(base.ContextGuid).BusinessAggregator.Events.Tafsil.AccountChanged += Tafsil_AccountChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandeleEventsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 48);
		}
	}

	private void SubScribe(TafsilBusiness tafsil)
	{
		try
		{
			if (tafsil != null)
			{
				_tafsils.TryGetValue(tafsil.Guid, out var value);
				if (value == null)
				{
					value = new HashSet<WeakReference>();
					_tafsils.TryAdd(tafsil.Guid, value);
				}
				value.Add(new WeakReference(tafsil, trackResurrection: false));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SubScribe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 63);
		}
	}

	private void SubScribe(IEnumerable<TafsilBusiness> tafsils)
	{
		try
		{
			if (tafsils == null || !tafsils.Any())
			{
				return;
			}
			foreach (TafsilBusiness tafsil in tafsils)
			{
				SubScribe(tafsil);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SubScribe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 73);
		}
	}

	private async void Tafsil_AccountChanged(object sender, AccountChangedEventArgs e)
	{
		try
		{
			_tafsils.TryGetValue(e.TafsilGuid, out var weakRefrences);
			if (!(weakRefrences?.Any() ?? false))
			{
				TafsilBusiness tafsilBusiness = TimingCache.GetAll().FirstOrDefault((TafsilBusiness p) => p.Guid == e.TafsilGuid);
				if (tafsilBusiness != null)
				{
					SubScribe(tafsilBusiness);
					_tafsils.TryGetValue(e.TafsilGuid, out weakRefrences);
				}
			}
			if (!(weakRefrences?.Any() ?? false))
			{
				return;
			}
			TafsilBusiness tafsilBusiness2 = await TafsilBusiness.GetFromDatabaseAsync(base.ContextGuid, EnSystem.None, e.TafsilGuid, "", default(CancellationToken), "Tafsil_AccountChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs");
			if (tafsilBusiness2 == null)
			{
				throw new NullReferenceException("tafsilFromDb");
			}
			foreach (WeakReference item in weakRefrences.ToList())
			{
				if (!(item.Target is TafsilBusiness tafsilBusiness3))
				{
					weakRefrences.Remove(item);
				}
				else
				{
					tafsilBusiness3.Account = tafsilBusiness2.Account;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Tafsil_AccountChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 101);
		}
	}

	public async Task<TafsilBusiness> GetAsync(long tafsilCode, CancellationToken token, string traName)
	{
		TafsilBusiness result2 = TimingCache.FirstOrDefault((TafsilBusiness p) => p.Code == tafsilCode);
		if (result2 != null)
		{
			return result2;
		}
		result2 = await _dbRepository.GetAsync(tafsilCode, token, traName);
		if (result2 != null)
		{
			await TimingCache.UpdateTimeAsync(result2.Guid, result2, traName);
			SubScribe(result2);
		}
		return result2;
	}

	public Task<string> NextTafsilSayerAsync(Guid ParentGuid, CancellationToken token, string traName)
	{
		return _dbRepository.NextTafsilSayerAsync(ParentGuid, token, traName);
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAccountAsync(Guid guid, decimal accountChange, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await _dbRepository.UpdateAccountAsync(guid, accountChange, tryCount, token, tranName), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 122);
			TafsilBusiness tafsil = await _dbRepository.GetAsync(guid, token, tranName);
			if (tafsil != null)
			{
				await TimingCache.UpdateTimeAsync(tafsil.Guid, tafsil, tranName);
				decimal account = await ClsCache.GetCache(base.ContextGuid).CacheRepository.Account.GetAccountSumAsync(null, new List<Guid> { tafsil.Guid }, true, tranName, default(CancellationToken));
				await ClsCache.GetCache(base.ContextGuid).CacheRepository.ICustomerCompact.UpdateAccountAsync(guid, account, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 133);
			ret.AddReturnedValue(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 134);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSumAghsatAsync(Guid guid, decimal price, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await _dbRepository.UpdateSumAghsatAsync(guid, price, tryCount, token, tranName), ReturnedState.None, "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 143);
			TafsilBusiness tafsil = await GetAsync(guid, token, tranName);
			if (tafsil == null)
			{
				tafsil = await _dbRepository.GetAsync(guid, token, tranName);
			}
			if (tafsil != null)
			{
				await TimingCache.UpdateTimeAsync(tafsil.Guid, tafsil, tranName);
				await ClsCache.GetCache(base.ContextGuid).CacheRepository.ICustomerCompact.UpdateSumAghsatAsync(guid, tafsil.SumAghsat, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 154);
			ret.AddReturnedValue(ex, "", "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 155);
		}
		return ret;
	}

	public async Task<TafsilBusiness> GetByTypeAndCodeAsync(HesabType hesabType, long code, CancellationToken token, string traName)
	{
		TafsilBusiness tafsilBusiness = await _dbRepository.GetByTypeAndCodeAsync(hesabType, code, token, traName);
		SubScribe(tafsilBusiness);
		return tafsilBusiness;
	}

	public Task<ReturnedSaveFuncInfoWithValue<long>> NextCodeInGroupAsync(Guid tfGuid, EnSuggestionType suggestionMode, CancellationToken token, string tranName)
	{
		return _dbRepository.NextCodeInGroupAsync(tfGuid, suggestionMode, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_CacheType?.Clear();
				_CacheType = null;
				TafsilEventAggregator tafsilEventAggregator = ClsCache.GetCache(base.ContextGuid)?.BusinessAggregator?.Events?.Tafsil;
				if (tafsilEventAggregator != null)
				{
					tafsilEventAggregator.AccountChanged -= Tafsil_AccountChanged;
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 182);
		}
	}

	public Task<ConcurrentDictionary<Guid, decimal>> GetAccountsAsync(CancellationToken token, string traName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(TafsilBusiness tafsil, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		int line = 0;
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				line = 1;
				if (tafsil == null)
				{
					return ret;
				}
				TafsilBusiness existsTafsil = await GetAsync(tafsil.Guid, token, tranName);
				line = 2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _dbRepository.SaveAsync(tafsil, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 196);
				line = 3;
				if (!updateAccount)
				{
					tafsil = await _dbRepository.GetAsync(tafsil.Guid, token, tranName);
				}
				line = 4;
				if (tafsil == null)
				{
					return ret;
				}
				await TimingCache.UpdateTimeAsync(tafsil.Guid, tafsil, tranName);
				line = 50;
				if (tafsil == null)
				{
					throw new NullReferenceException("Tafsil is null!!");
				}
				line = 51;
				ClsCache cache = ClsCache.GetCache(base.ContextGuid) ?? throw new NullReferenceException("cache is null!!");
				line = 52;
				if (cache.CacheRepository?.ICustomerCompact == null)
				{
					throw new NullReferenceException("cache.CacheRepository?.ICustomerCompact is null!!");
				}
				line = 53;
				if (existsTafsil != null && existsTafsil.Account != tafsil.Account)
				{
					decimal account = await ClsCache.GetCache(base.ContextGuid).CacheRepository.Account.GetAccountSumAsync(null, new List<Guid> { tafsil.Guid }, true, tranName, default(CancellationToken));
					await cache.CacheRepository.ICustomerCompact.UpdateAccountAsync(tafsil.Guid, account, token, tranName);
				}
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(base.ContextGuid)?.IsDisposed ?? true)
				{
					Log.Warning(ex, $"Line:{line}//ContextGuid:{base.ContextGuid}//IsDisposed:{IsDisposed}//TafsilIsNull:{tafsil == null}//TimingCache is null:{TimingCache == null}//ClsCache.GetCacheHistory:{ClsCache.GetCacheHistory(base.ContextGuid)}");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ContextGuid:{base.ContextGuid}//IsDisposed:{IsDisposed}//TafsilIsNull:{tafsil == null}//TimingCache is null:{TimingCache == null}//ClsCache.GetCacheHistory:{ClsCache.GetCacheHistory(base.ContextGuid)}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 221);
				}
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilCacheRepository.cs", 222);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<TafsilBusiness>> GetAllAsync(EnSystem system, TafsilFilter filter, CancellationToken token, string tranName)
	{
		IEnumerable<TafsilBusiness> enumerable = await _dbRepository.GetAllAsync(system, filter, token, tranName);
		SubScribe(enumerable);
		return enumerable;
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilBusiness> list, CancellationToken token, string tranName)
	{
		return _dbRepository.SaveRangeAsync(list, token, tranName);
	}

	public Task<List<Guid>> CheckExistAsync(List<Guid> lst, CancellationToken token, string tranName)
	{
		return _dbRepository.CheckExistAsync(lst, token, tranName);
	}
}
