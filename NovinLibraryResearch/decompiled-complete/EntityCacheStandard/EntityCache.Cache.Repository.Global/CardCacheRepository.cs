using System;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class CardCacheRepository : CacheRepository<CardBusiness>, ICardRepository, IRepository<CardBusiness>, INoGuidRepository<CardBusiness>, IDisposable
{
	private ICardRepository _dbRepository;

	public override IItemAsyncMapper<CardBusiness, CardBusiness> Mapper => CardMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CardBusiness> TimingCache { get; set; }

	public CardCacheRepository(ICardRepository dbRepository, int CachingTime)
		: base((IRepository<CardBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CardBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\CardCacheRepository.cs", 35);
		}
	}
}
