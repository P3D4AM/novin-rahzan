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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class RegionPersistenceRepository : PersistenceRepository<RegionBusiness, Region>, IRegionRepository, IRepository<RegionBusiness>, INoGuidRepository<RegionBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<RegionBusiness, Region>, IItemAsyncMapper<Region, RegionBusiness>
	{
		public Task<Region> MapAsync(string contextGuid, RegionBusiness source, Region destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Region();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityGuid = source.CityGuid;
				destination.Name = source.Name;
				destination.Type = source.Type;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 117);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<RegionBusiness> MapAsync(string contextGuid, Region source, RegionBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new RegionBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityGuid = source.CityGuid;
				destination.Name = source.Name;
				destination.Type = source.Type;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 142);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Region>> MapListAsync(string contextGuid, IEnumerable<RegionBusiness> source, List<Region> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<RegionBusiness>> MapListAsync(string contextGuid, IEnumerable<Region> source, List<RegionBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Region, RegionBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<RegionBusiness, Region> MapToDatabase => Mapper;

	public override IItemMapper<Region, RegionBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public RegionPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<RegionBusiness>> GetAllAsync(Guid cityGuid, AdvertiseType type, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 25));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs"))
		{
			IEnumerable<RegionBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Region> queryable = ((IQueryable<Region>)c.Regions)?.Where((Region q) => q.CityGuid == cityGuid && q.Type == type);
					if (status.HasValue)
					{
						queryable = queryable?.Where((Region q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 35);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByTypeAsync(AdvertiseType type, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 53));
				List<Region> list = ((IQueryable<Region>)adv_ModelContext.Regions)?.Where((Region p) => p.Type == type).ToList();
				if (list.Any())
				{
					adv_ModelContext.Regions.RemoveRange((IEnumerable<Region>)list);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 65);
				ret.AddReturnedValue(ex, "", "RemoveByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 66);
			}
			return ret;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\RegionPersistenceRepository.cs", 84);
			throw;
		}
	}
}
