using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class AdvertisePersistenceRepository : PersistenceRepository<AdvertiseBusiness, AccSqlServerPersistence.Entities.Advertise.Advertise>, IAdvertiseRepository, IRepository<AdvertiseBusiness>, INoGuidRepository<AdvertiseBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AdvertiseBusiness, AccSqlServerPersistence.Entities.Advertise.Advertise>, IItemAsyncMapper<AccSqlServerPersistence.Entities.Advertise.Advertise, AdvertiseBusiness>
	{
		public Task<AccSqlServerPersistence.Entities.Advertise.Advertise> MapAsync(string contextGuid, AdvertiseBusiness source, AccSqlServerPersistence.Entities.Advertise.Advertise destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AccSqlServerPersistence.Entities.Advertise.Advertise();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ContentList = source.ContentList;
				destination.DivatCategoryList = source.DivatCategoryList;
				destination.GroupGuid = source.GroupGuid;
				destination.ImageList = source.ImageList;
				destination.IsPishNevis = source.IsPishNevis;
				destination.Price = source.Price;
				destination.SheypoorCategoryList = source.SheypoorCategoryList;
				destination.TitleList = source.TitleList;
				destination.Name = source.Name;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertisePersistenceRepository.cs", 94);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AdvertiseBusiness> MapAsync(string contextGuid, AccSqlServerPersistence.Entities.Advertise.Advertise source, AdvertiseBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvertiseBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ContentList = source.ContentList;
				destination.DivatCategoryList = source.DivatCategoryList;
				destination.GroupGuid = source.GroupGuid;
				destination.ImageList = source.ImageList;
				destination.IsPishNevis = source.IsPishNevis;
				destination.Price = source.Price;
				destination.SheypoorCategoryList = source.SheypoorCategoryList;
				destination.TitleList = source.TitleList;
				destination.Name = source.Name;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertisePersistenceRepository.cs", 125);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AccSqlServerPersistence.Entities.Advertise.Advertise>> MapListAsync(string contextGuid, IEnumerable<AdvertiseBusiness> source, List<AccSqlServerPersistence.Entities.Advertise.Advertise> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AdvertiseBusiness>> MapListAsync(string contextGuid, IEnumerable<AccSqlServerPersistence.Entities.Advertise.Advertise> source, List<AdvertiseBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<AccSqlServerPersistence.Entities.Advertise.Advertise, AdvertiseBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AdvertiseBusiness, AccSqlServerPersistence.Entities.Advertise.Advertise> MapToDatabase => Mapper;

	public override IItemMapper<AccSqlServerPersistence.Entities.Advertise.Advertise, AdvertiseBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal AdvertisePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbContext = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertisePersistenceRepository.cs", 31);
			throw;
		}
	}

	public async Task<IEnumerable<AdvertiseBusiness>> GetAllAsync(Guid groupGuid, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertisePersistenceRepository.cs", 39));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertisePersistenceRepository.cs"))
		{
			IEnumerable<AdvertiseBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<AccSqlServerPersistence.Entities.Advertise.Advertise> queryable = (IQueryable<AccSqlServerPersistence.Entities.Advertise.Advertise>)c.Advertise;
					if (groupGuid != Guid.Empty)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Advertise.Advertise q) => q.GroupGuid == groupGuid);
					}
					if (status.HasValue)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Advertise.Advertise q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList());
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertisePersistenceRepository.cs", 50);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertisePersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
