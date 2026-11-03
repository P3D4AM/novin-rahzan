using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class DescCacheRepository : SyncCacheRepository<DescBusiness>, IDescRepository, ISyncRepository<DescBusiness>, IEditableDelivery, IRepository<DescBusiness>, INoGuidRepository<DescBusiness>, IDisposable
{
	private IDescRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, DescBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<DescBusiness, DescBusiness> Mapper => DescMapper.Instance;

	public DescCacheRepository(IDescRepository dbRepository, int CachingTime)
		: base((ISyncRepository<DescBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, DescBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<DescBusiness>> GetAllAsync(DescSection _section, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(_section, status, token, tranName);
	}

	public Task<IEnumerable<DescBusiness>> GetAllAsync(string search, DescSection Section, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(search, Section, token, status, tranName);
	}

	public override void Dispose()
	{
		try
		{
			dbRepository = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\DescCacheRepository.cs", 48);
		}
	}
}
