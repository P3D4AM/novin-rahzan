using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm.Calling;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Crm;
using EntityCache.Mapping.BusinessMappers.Crm.Calling;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters.Crm;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Crm;

public class CallingLogDetailCacheRepository : CacheRepositoryNoGuiId<CallingLogDetailBusiness, string>, ICallingLogDetailRepository, INoGuidRepository<CallingLogDetailBusiness>, IDisposable
{
	private ICallingLogDetailRepository _dbRepository;

	protected override TimingCache<string, CallingLogDetailBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CallingLogDetailBusiness, CallingLogDetailBusiness> Mapper => CallingLogDetailMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public CallingLogDetailCacheRepository(ICallingLogDetailRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<CallingLogDetailBusiness>)dbRepository, CachingTime)
	{
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			_dbRepository = dbRepository;
			TimingCache = new TimingCache<string, CallingLogDetailBusiness>(base.ContextGuid, Mapper, CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogDetailCacheRepository.cs", 36);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogDetailCacheRepository.cs", 47);
		}
	}

	public Task<CallingLogDetailBusiness> GetAsync(string uuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(uuid, token, tranName);
	}

	public Task<IEnumerable<CallingLogDetailBusiness>> GetAllAsync(CallingLogDetailFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}
}
