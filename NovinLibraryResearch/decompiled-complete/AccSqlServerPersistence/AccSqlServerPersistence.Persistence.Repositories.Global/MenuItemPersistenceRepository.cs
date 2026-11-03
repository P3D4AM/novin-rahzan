using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Global;

internal class MenuItemPersistenceRepository : PersistenceMasterRepository<MenuItemBusiness, MenuItem>, IMenuItemRepository, IPersistenceMasterRepository<MenuItemBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<MenuItemBusiness, MenuItem>, IItemAsyncMapper<MenuItem, MenuItemBusiness>
	{
		public Task<MenuItem> MapAsync(string contextGuid, MenuItemBusiness source, MenuItem destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new MenuItem();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Type = source.Type;
				destination.TypeGuid = source.TypeGuid;
				destination.Ordinal = source.Ordinal;
				destination.MasterGuid = source.MasterGuid;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 112);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<MenuItemBusiness> MapAsync(string contextGuid, MenuItem source, MenuItemBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new MenuItemBusiness(contextGuid, null);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Type = source.Type;
				destination.TypeGuid = source.TypeGuid;
				destination.SetOrdinalChange(EnSystem.Mapper, source.Ordinal, this);
				destination.MasterGuid = source.MasterGuid;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 139);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<MenuItem>> MapListAsync(string contextGuid, IEnumerable<MenuItemBusiness> source, List<MenuItem> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<MenuItemBusiness>> MapListAsync(string contextGuid, IEnumerable<MenuItem> source, List<MenuItemBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _disposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<MenuItem, MenuItemBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<MenuItemBusiness, MenuItem> MapToDatabase => Mapper;

	public MenuItemPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_disposed)
			{
				_disposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 38);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 47));
				List<MenuItem> list = await EntityFrameworkQueryableExtensions.ToListAsync<MenuItem>(((IQueryable<MenuItem>)dbcontext.MenuItems).Where((MenuItem x) => x.Guid == guid), token);
				if (list != null && list.Any())
				{
					dbcontext.MenuItems.RemoveRange((IEnumerable<MenuItem>)list);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 54);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 55);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<MenuItemBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (items != null)
				{
					if (items != null && items.Count() > 0)
					{
						BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 69);
						foreach (MenuItemBusiness item in items)
						{
							MenuItem menuItem = await MapToDatabase.MapAsync(base.ContextGuid, item, new MenuItem(), tranName, token);
							menuItem.MasterGuid = masterGuid;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(menuItem, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 74);
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 81);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuItemPersistenceRepository.cs", 82);
			}
			return ret;
		}
	}
}
