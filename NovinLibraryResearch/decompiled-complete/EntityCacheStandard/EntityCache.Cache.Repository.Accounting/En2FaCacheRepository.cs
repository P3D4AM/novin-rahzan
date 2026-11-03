using System;
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

public class En2FaCacheRepository : CacheRepository<En2FaBusiness>, IEn2FaRepositpry, IRepository<En2FaBusiness>, INoGuidRepository<En2FaBusiness>, IDisposable
{
	private IEn2FaRepositpry dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, En2FaBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<En2FaBusiness, En2FaBusiness> Mapper => En2FaMapper.Instance;

	public En2FaCacheRepository(IEn2FaRepositpry dbRepository, int CachingTime)
		: base((IRepository<En2FaBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, En2FaBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\En2FaCacheRepository.cs", 36);
		}
	}

	public Task<string> ConvertFa(string en, CancellationToken token, string tranName)
	{
		return dbRepository.ConvertFa(en, token, tranName);
	}
}
