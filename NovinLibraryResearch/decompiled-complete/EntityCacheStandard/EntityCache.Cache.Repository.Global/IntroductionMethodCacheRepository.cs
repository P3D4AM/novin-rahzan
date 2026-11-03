using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.Caching;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class IntroductionMethodCacheRepository : SyncCacheRepository<IntroductionMethodBusiness>, IIntroductionMethodRepository, ISyncRepository<IntroductionMethodBusiness>, IEditableDelivery, IRepository<IntroductionMethodBusiness>, INoGuidRepository<IntroductionMethodBusiness>, IDisposable
{
	private IIntroductionMethodRepository _dbRepository;

	public override bool HasModified => true;

	public override bool HasStatus => true;

	public override IItemAsyncMapper<IntroductionMethodBusiness, IntroductionMethodBusiness> Mapper => IntroductionMethodMapper.Instance;

	protected override TimingCache<Guid, IntroductionMethodBusiness> TimingCache { get; set; }

	public IntroductionMethodCacheRepository(IIntroductionMethodRepository dbRepo, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<IntroductionMethodBusiness>)dbRepo, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepo;
		TimingCache = new TimingCache<Guid, IntroductionMethodBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IntroductionMethodBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(name, token, tranName);
	}
}
