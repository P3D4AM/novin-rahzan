using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Indexes;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Indexes;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Indexes;

public class FullTextWordCacheRepository : CacheRepositoryNoGuiId<FullTextWordBusiness, int>, IFullTextWordRepository, INoGuidRepository<FullTextWordBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected new IFullTextWordRepository DbRepository { get; set; }

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<FullTextWordBusiness, FullTextWordBusiness> Mapper => FullTextWordMapper.Instance;

	protected override TimingCache<int, FullTextWordBusiness> TimingCache { get; set; }

	protected TimingCache<string, FullTextWordBusiness> WordsTimingCache { get; set; }

	public FullTextWordCacheRepository(IFullTextWordRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<FullTextWordBusiness>)dbRepository, cachingTime)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Indexes\\FullTextWordCacheRepository.cs"))
		{
			try
			{
				DbRepository = dbRepository;
				TimingCache = new TimingCache<int, FullTextWordBusiness>(base.Context?.Guid, Mapper, cachingTime);
				WordsTimingCache = new TimingCache<string, FullTextWordBusiness>(base.Context?.Guid, Mapper, cachingTime);
				base.CachingTime = cachingTime;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Indexes\\FullTextWordCacheRepository.cs", 36);
				throw;
			}
		}
	}

	public Task<Dictionary<string, int>> GetOrSaveAsync(List<string> words, CancellationToken token, string tranName)
	{
		return DbRepository.GetOrSaveAsync(words, token, tranName);
	}

	public override void Dispose()
	{
		Dispose(isDisposing: true);
	}

	public override void Dispose(bool isDisposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				DbRepository?.Dispose();
				DbRepository = null;
				base.Dispose(disposing: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Indexes\\FullTextWordCacheRepository.cs", 103);
		}
	}
}
