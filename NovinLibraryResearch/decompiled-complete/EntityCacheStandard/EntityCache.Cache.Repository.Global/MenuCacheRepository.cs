using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class MenuCacheRepository : CacheRepository<MenuBusiness>, IMenuRepository, IRepository<MenuBusiness>, INoGuidRepository<MenuBusiness>, IDisposable
{
	private IMenuRepository _dbRepository;

	public override IItemAsyncMapper<MenuBusiness, MenuBusiness> Mapper => MenuMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, MenuBusiness> TimingCache { get; set; }

	public MenuCacheRepository(IMenuRepository dbRepository, int CachingTime)
		: base((IRepository<MenuBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, MenuBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override Task<MenuBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateAsync(Guid masterGuid, List<Guid> guids, CancellationToken token, string tranName)
	{
		return _dbRepository.UpdateAsync(masterGuid, guids, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\MenuCacheRepository.cs", 41);
		}
	}
}
