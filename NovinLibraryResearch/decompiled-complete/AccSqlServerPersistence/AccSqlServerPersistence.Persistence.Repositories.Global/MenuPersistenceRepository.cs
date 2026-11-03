using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

internal class MenuPersistenceRepository : PersistenceRepository<MenuBusiness, Menu>, IMenuRepository, IRepository<MenuBusiness>, INoGuidRepository<MenuBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<MenuBusiness, Menu>, IItemAsyncMapper<Menu, MenuBusiness>
	{
		public Task<Menu> MapAsync(string contextGuid, MenuBusiness source, Menu destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Menu();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.RowCount = source.RowCount;
				destination.ColumnCount = source.ColumnCount;
				destination.Title = source.Title;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 155);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<MenuBusiness> MapAsync(string contextGuid, Menu source, MenuBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new MenuBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.RowCount = source.RowCount;
				destination.ColumnCount = source.ColumnCount;
				destination.Title = source.Title;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 183);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Menu>> MapListAsync(string contextGuid, IEnumerable<MenuBusiness> source, List<Menu> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<MenuBusiness>> MapListAsync(string contextGuid, IEnumerable<Menu> source, List<MenuBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private MenuItemPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Menu, MenuBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Menu, MenuBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<MenuBusiness, Menu> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public MenuPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new MenuItemPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 40);
		}
	}

	public override async Task<MenuBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 44));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs"))
		{
			MenuBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					Menu menu = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Menu>((IQueryable<Menu>)c.Menu, (Expression<Func<Menu, bool>>)((Menu x) => x.Guid == guid), token);
					if (menu == null)
					{
						result = null;
					}
					else
					{
						MenuBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, menu, null, tranName, token);
						await ret.AddRangeToListSanadAsync(await _detRepo.GetAllAsync(guid, token, tranName));
						result = ret;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 60);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(MenuBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 77));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				Menu menu = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(menu, menu.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 80);
				List<MenuItemBusiness> list = await _detRepo.GetAllAsync(item.Guid, token, tranName);
				if (list != null && list.Any())
				{
					List<Guid> source = list.Select((MenuItemBusiness q) => q.Guid)?.ToList();
					List<Guid> detailGuids = item.DetailList?.Select((MenuItemBusiness q) => q.Guid)?.ToList() ?? new List<Guid>();
					IEnumerable<Guid> enumerable = source.Where((Guid q) => !detailGuids.Contains(q));
					if (enumerable != null && enumerable.Any())
					{
						foreach (Guid item2 in enumerable)
						{
							await _detRepo.RemoveAsync(item2, token, tranName);
						}
					}
				}
				if ((item?.DetailList?.Any((MenuItemBusiness q) => q.Guid != Guid.Empty)).GetValueOrDefault())
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetailList.Where((MenuItemBusiness q) => q.Guid != Guid.Empty).ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 95);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 100);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 101);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAsync(Guid masterGuid, List<Guid> guids, CancellationToken token, string tranname)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			using (new JaegerMonitor("UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs"))
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranname, 60000, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 112));
				foreach (Guid guid in guids)
				{
					Menu menu = await dbcontext.Menu.FindAsync(new object[1] { guid });
					if (menu != null)
					{
						menu.MasterGuid = masterGuid;
					}
					await dbcontext.AddOrUpdateAsync(menu, menu.Guid);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 123);
			res.AddReturnedValue(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MenuPersistenceRepository.cs", 124);
		}
		return res;
	}
}
