using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;
using EntityCache.Business.Global.Tasks.SarNakh;
using EntityCache.Core;
using EntityCache.Core.Repository.Global.Tasks;
using EntityCache.Mapping.Global.Tasks;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters.Global;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global.Tasks;

internal class SarNakhCacheRepository : CacheRepository<SarNakhBusiness>, ISarNakhRepository, IRepository<SarNakhBusiness>, INoGuidRepository<SarNakhBusiness>, IDisposable
{
	private ISarNakhRepository _dbRepository;

	private bool _isDisposed;

	public override IItemAsyncMapper<SarNakhBusiness, SarNakhBusiness> Mapper => SarNakhMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SarNakhBusiness> TimingCache { get; set; }

	public SarNakhCacheRepository(ISarNakhRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<SarNakhBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		try
		{
			_dbRepository = dbRepository;
			TimingCache = new TimingCache<Guid, SarNakhBusiness>(base.ContextGuid, Mapper, CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tasks\\SarNakh\\SarNakhCacheRepository.cs", 36);
			throw;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_dbRepository?.Dispose();
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\Tasks\\SarNakh\\SarNakhCacheRepository.cs", 53);
		}
	}

	public Task<IEnumerable<SarNakhReportBusiness>> GetAllAsync(SarNakhFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<List<string>> GetStateListAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetStateListAsync(token, tranName);
	}

	public Task<List<string>> GetCityListAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetCityListAsync(token, tranName);
	}

	public override Task<SarNakhBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}
}
