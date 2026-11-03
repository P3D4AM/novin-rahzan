using System;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository.Global;

public class MenuItemCacheRepository : MasterCacheRepository<MenuItemBusiness>, IMenuItemRepository, IPersistenceMasterRepository<MenuItemBusiness>, IDisposable
{
	private IMenuItemRepository _dbRepository;

	public MenuItemCacheRepository(IMenuItemRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<MenuItemBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\MenuItemCacheRepository.cs", 25);
		}
	}
}
