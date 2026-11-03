using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Customers;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Customer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class CusCardCacheRepository : CacheRepository<CusCardBusiness>, ICusCardRepository, IRepository<CusCardBusiness>, INoGuidRepository<CusCardBusiness>, IDisposable
{
	private ICusCardRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CusCardBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CusCardBusiness, CusCardBusiness> Mapper => CusCardMapper.Instance;

	public CusCardCacheRepository(ICusCardRepository dbRepository, int CachingTime)
		: base((IRepository<CusCardBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CusCardBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<CusCardBusiness>> GetAllByCusGuidAsync(Guid custGuid, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllByCusGuidAsync(custGuid, status, token, tranName);
	}

	public Task<IEnumerable<CusCardBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(search, token, status, tranName);
	}

	public Task<CusCardBusiness> GetAsync(string cardNumber, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(cardNumber, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CusCardCacheRepository.cs", 53);
		}
	}

	public Task<IEnumerable<CustomerCardModel>> GetAllModelAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllModelAsync(token, tranName);
	}
}
