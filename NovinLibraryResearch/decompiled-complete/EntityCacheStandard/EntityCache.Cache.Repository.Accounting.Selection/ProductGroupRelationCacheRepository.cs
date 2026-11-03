using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Selection;
using EntityCache.Mapping.BusinessMappers.Accounting.Selection;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Selection;

public class ProductGroupRelationCacheRepository : CacheRepositoryNoGuiId<ProductGroupRelationBusiness, string>, IProductGroupRelationRepository, INoGuidRepository<ProductGroupRelationBusiness>, IDisposable
{
	private IProductGroupRelationRepository _dbRepository;

	public override IItemAsyncMapper<ProductGroupRelationBusiness, ProductGroupRelationBusiness> Mapper => ProductGroupRelationMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	protected override TimingCache<string, ProductGroupRelationBusiness> TimingCache { get; set; }

	public ProductGroupRelationCacheRepository(IProductGroupRelationRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<ProductGroupRelationBusiness>)dbRepository, CachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, ProductGroupRelationBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Selection\\ProductGroupRelationCacheRepository.cs", 42);
		}
	}

	public Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(Guid? productGuid, Guid? groupGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(productGuid, groupGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangeAsync(productGuid, token, tranName);
	}

	public Task<ConcurrentDictionary<Guid, List<Guid>>> GetDictionaryAsync(List<Guid> productGuidList, CancellationToken token, string tranName)
	{
		return _dbRepository.GetDictionaryAsync(productGuidList, token, tranName);
	}

	public Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(List<Guid> groupGuidList, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(groupGuidList, token, tranName);
	}
}
