using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tax;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Tax;
using EntityCache.Mapping.BusinessMappers.Accounting.Tax;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Tax;

public class TaxCacheRepository : SyncCacheRepository<TaxBusiness>, ITaxRepository, ISyncRepository<TaxBusiness>, IEditableDelivery, IRepository<TaxBusiness>, INoGuidRepository<TaxBusiness>, IDisposable
{
	private ITaxRepository _dbRepository;

	public override IItemAsyncMapper<TaxBusiness, TaxBusiness> Mapper => TaxMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TaxBusiness> TimingCache { get; set; }

	public TaxCacheRepository(ITaxRepository dbRepo, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<TaxBusiness>)dbRepo, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepo;
		TimingCache = new TimingCache<Guid, TaxBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Tax\\TaxCacheRepository.cs", 38);
		}
	}

	public Task<IEnumerable<TaxReportBusiness>> GetAllReportAsync(bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllReportAsync(status, token, tranName);
	}

	public Task<TaxReportBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(name, token, tranName);
	}

	public override Task<TaxBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}

	public Task<TaxReportBusiness> GetBySlugAsync(string slug, CancellationToken token, string tranName)
	{
		return _dbRepository.GetBySlugAsync(slug, token, tranName);
	}
}
