using System;
using System.Collections.Generic;
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
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class CashierCacheRepository : CacheRepository<CashierBusiness>, ICashierRepository, ISyncRepository<CashierBusiness>, IEditableDelivery, IRepository<CashierBusiness>, INoGuidRepository<CashierBusiness>, IDisposable
{
	private ICashierRepository dbRepository;

	public override IItemAsyncMapper<CashierBusiness, CashierBusiness> Mapper => CashierMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<Guid, CashierBusiness> TimingCache { get; set; }

	public CashierCacheRepository(ICashierRepository dbRepository, int CachingTime)
		: base((IRepository<CashierBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CashierBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override CashierBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<bool> CheckUserNameAsync(string uName, Guid _guid, CancellationToken token, string tranName)
	{
		return dbRepository.CheckUserNameAsync(uName, _guid, token, tranName);
	}

	public Task<CashierBusiness> GetAsync(string uName, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(uName, token, tranName);
	}

	public Task<CashierBusiness> GetByNameAsync(string name, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetByNameAsync(name, token, tranName);
	}

	public Task<IEnumerable<CashierBusiness>> GetAllAsync(Guid userGuid, bool? status, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(userGuid, status, token, tranName);
	}

	public async Task<CashierBusiness> GetByUsernameAsync(string username, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		CashierBusiness ret = await dbRepository.GetByUsernameAsync(username, token, tranName);
		WebErrorLog.Instance.Debug($"Cachier exists: {ret != null}. username is: {username}", "", "GetByUsernameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\CashierCacheRepository.cs", 59);
		if (ret != null && ret.Guid != Guid.Empty)
		{
			await TimingCache.UpdateTimeAsync(ret.Guid, ret, tranName);
		}
		return ret;
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllNotSentAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		return dbRepository.SetDeliveryAsync(del, token, tryCount, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		return dbRepository.SetSaveResult(item, token, tryCount, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateServerStatusAsync(guid, status, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\CashierCacheRepository.cs", 77);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		TimingCache.ClearAll();
	}
}
