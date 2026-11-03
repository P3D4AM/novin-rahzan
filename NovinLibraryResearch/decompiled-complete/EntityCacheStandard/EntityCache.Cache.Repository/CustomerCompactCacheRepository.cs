using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Customer;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.Events;
using PacketParser.Filters.Customer;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class CustomerCompactCacheRepository : CompactCacheRepository<CusCompactBusiness>, ICustomerCompactRepository, ICompactRepository<CusCompactBusiness>, IDisposable
{
	private ICustomerCompactRepository dbRepository;

	private bool _isDisposed;

	private readonly ConcurrentDictionary<Guid, HashSet<WeakReference>> _customers = new ConcurrentDictionary<Guid, HashSet<WeakReference>>();

	private ClsCache _cache;

	[JsonIgnore]
	public ClsCache Cache
	{
		get
		{
			if (_cache == null)
			{
				_cache = ClsCache.GetCache(base.ContextGuid);
			}
			return _cache;
		}
	}

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

	public CustomerCompactCacheRepository(ICustomerCompactRepository dbRepository, int CachingTime)
		: base((ICompactRepository<CusCompactBusiness>)dbRepository, CachingTime, (IItemAsyncMapper<CusCompactBusiness, CusCompactBusiness>)CustomerMapper.Instance)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		HandeleEventsAsync();
	}

	private async Task HandeleEventsAsync()
	{
		try
		{
			while (ClsCache.GetCache(base.ContextGuid)?.BusinessAggregator == null)
			{
				await Task.Delay(100);
			}
			ClsCache.GetCache(base.ContextGuid).BusinessAggregator.Events.CusCompact.SumAghsatChanged += CusCompact_SumAghsatChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandeleEventsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 43);
		}
	}

	private async void CusCompact_SumAghsatChanged(object sender, AccountChangedEventArgs e)
	{
		try
		{
			_customers.TryGetValue(e.TafsilGuid, out var weakRefrences);
			if (!(weakRefrences?.Any() ?? false))
			{
				return;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetFromDatabaseAsync(base.ContextGuid, EnSystem.None, e.TafsilGuid, e.TranName, default(CancellationToken), "CusCompact_SumAghsatChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs");
			foreach (WeakReference item in weakRefrences.ToList())
			{
				if (!(item.Target is CusCompactBusiness cusCompactBusiness))
				{
					weakRefrences.Remove(item);
				}
				else
				{
					cusCompactBusiness.SumAghsat = tafsilBusiness.Account;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CusCompact_SumAghsatChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 63);
		}
	}

	private void SubScribe(CusCompactBusiness customer)
	{
		try
		{
			if (Cache.Source != ENSource.Android_HesabYar && customer != null)
			{
				_customers.TryGetValue(customer.Guid, out var value);
				if (value == null)
				{
					value = new HashSet<WeakReference>();
					_customers.TryAdd(customer.Guid, value);
				}
				WeakReference item = new WeakReference(customer, trackResurrection: false);
				value.Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SubScribe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 81);
		}
	}

	private void SubScribe(IEnumerable<CusCompactBusiness> customers)
	{
		using (new JaegerMonitor("SubScribe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs"))
		{
			try
			{
				if (Cache.Source == ENSource.Android_HesabYar || customers == null || !customers.Any())
				{
					return;
				}
				foreach (CusCompactBusiness customer in customers)
				{
					SubScribe(customer);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SubScribe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 94);
			}
		}
	}

	public async Task<CusCompactBusiness> GetAsync(long code, List<Guid> lstMoeinGuids, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		CusCompactBusiness cusCompactBusiness = (await GetAllAsync(lstMoeinGuids, null, token, tranName)).Where((CusCompactBusiness x) => x.Code == code)?.FirstOrDefault();
		SubScribe(cusCompactBusiness);
		return cusCompactBusiness;
	}

	public async Task<CusCompactBusiness> GetAsync(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName)
	{
		CusCompactBusiness cusCompactBusiness = await dbRepository.GetAsync(guid, lstMoeinGuids, token, tranName);
		SubScribe(cusCompactBusiness);
		return cusCompactBusiness;
	}

	public CusCompactBusiness Get(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName)
	{
		CusCompactBusiness cusCompactBusiness = dbRepository.Get(guid, lstMoeinGuids, token, tranName);
		SubScribe(cusCompactBusiness);
		return cusCompactBusiness;
	}

	public async Task<List<CusCompactBusiness>> GetAllAsync(List<Guid> lstMoeinGuids, bool? status, CancellationToken token, string tranName)
	{
		List<CusCompactBusiness> list = await dbRepository.GetAllAsync(lstMoeinGuids, status, token, tranName);
		if (Cache.Source == ENSource.Android_HesabYar)
		{
			return list;
		}
		Task.Run(delegate
		{
			SubScribe(list);
		});
		return list;
	}

	public new Task<ReturnedSaveFuncInfo> RemoveFromEntitiesAsync(IEnumerable<Guid> guids, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return base.RemoveFromEntitiesAsync(guids, tryCount, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> HasFaceImageAsync(string fileName, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = (await GetAllAsync(true, token, tranName)).Count((CusCompactBusiness x) => x.PicFace == fileName) > 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasFaceImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 140);
			ret.AddReturnedValue(ex, "", "HasFaceImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 141);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> HasSignImageAsync(string fileName, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = (await GetAllAsync(true, token, tranName)).Count((CusCompactBusiness x) => x.PicSign == fileName) > 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasSignImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 156);
			ret.AddReturnedValue(ex, "", "HasSignImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 157);
		}
		return ret;
	}

	public async Task<int> CountWithThisTitleAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return (await GetAllAsync(true, token, tranName)).Count((CusCompactBusiness x) => x.TitleGuid == guid);
	}

	public override void Dispose()
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			dbRepository = null;
			if (!string.IsNullOrEmpty(base.ContextGuid))
			{
				CusCompactEventAggregator cusCompactEventAggregator = ClsCache.GetCache(base.ContextGuid)?.BusinessAggregator?.Events?.CusCompact;
				if (cusCompactEventAggregator != null)
				{
					cusCompactEventAggregator.SumAghsatChanged -= CusCompact_SumAghsatChanged;
				}
			}
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 182);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		TimingCache?.ClearAll();
	}

	public async Task UpdateAccountAsync(Guid guid, decimal account, CancellationToken token, string tranName)
	{
		try
		{
			if (TimingCache.TryGetValue(guid, out var ret))
			{
				ret.Account = account;
				await TimingCache.UpdateTimeAsync(guid, ret, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 196);
			throw;
		}
	}

	public async Task UpdateSumAghsatAsync(Guid guid, decimal account, CancellationToken token, string tranName)
	{
		try
		{
			if (TimingCache.TryGetValue(guid, out var ret))
			{
				ret.SumAghsat = account;
				await TimingCache.UpdateTimeAsync(guid, ret, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerCompactCacheRepository.cs", 211);
			throw;
		}
	}

	public async Task<IEnumerable<CusCompactBusiness>> GetAllAsync(CustomerCompactFilter filter, CancellationToken token, string tranName)
	{
		IEnumerable<CusCompactBusiness> list = await dbRepository.GetAllAsync(filter, token, tranName);
		Task.Run(delegate
		{
			SubScribe(list);
		});
		return list;
	}

	public Task<IEnumerable<CusCompactBusiness>> GetAllForSendToServerSniftsAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllForSendToServerSniftsAsync(token, tranName);
	}
}
