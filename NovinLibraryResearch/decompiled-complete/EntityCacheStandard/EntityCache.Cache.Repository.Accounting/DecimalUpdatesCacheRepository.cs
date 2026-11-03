using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

internal class DecimalUpdatesCacheRepository : CacheRepository<DecimalUpdatesBusiness>, IDecimalUpdatesRepository, IRepository<DecimalUpdatesBusiness>, INoGuidRepository<DecimalUpdatesBusiness>, IDisposable
{
	private new IDecimalUpdatesRepository DbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<DecimalUpdatesBusiness, DecimalUpdatesBusiness> Mapper => DecimalUpdatedMapper.Instance;

	protected override TimingCache<Guid, DecimalUpdatesBusiness> TimingCache { get; set; }

	internal DecimalUpdatesCacheRepository(IRepository<DecimalUpdatesBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = (IDecimalUpdatesRepository)dbRepository;
		TimingCache = new TimingCache<Guid, DecimalUpdatesBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<DecimalUpdatesBusiness>> GetAllByDecimalGuidAsync(Guid decimalGuid, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAllByDecimalGuidAsync(decimalGuid, status, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\DecimalUpdatesCacheRepository.cs", 44);
		}
	}
}
