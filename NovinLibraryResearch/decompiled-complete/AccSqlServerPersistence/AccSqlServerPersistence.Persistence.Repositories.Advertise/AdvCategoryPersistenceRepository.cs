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

internal class AdvCategoryPersistenceRepository : PersistenceRepository<AdvCategoryBusiness, AdvCategory>, IAdvCategoryRepository, IRepository<AdvCategoryBusiness>, INoGuidRepository<AdvCategoryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AdvCategoryBusiness, AdvCategory>, IItemAsyncMapper<AdvCategory, AdvCategoryBusiness>
	{
		public Task<AdvCategory> MapAsync(string contextGuid, AdvCategoryBusiness source, AdvCategory destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvCategory();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.ParentGuid = source.ParentGuid;
				destination.Type = source.Type;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvCategoryPersistenceRepository.cs", 87);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AdvCategoryBusiness> MapAsync(string contextGuid, AdvCategory source, AdvCategoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvCategoryBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.ParentGuid = source.ParentGuid;
				destination.Type = source.Type;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvCategoryPersistenceRepository.cs", 112);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AdvCategory>> MapListAsync(string contextGuid, IEnumerable<AdvCategoryBusiness> source, List<AdvCategory> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AdvCategoryBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvCategory> source, List<AdvCategoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<AdvCategory, AdvCategoryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AdvCategoryBusiness, AdvCategory> MapToDatabase => Mapper;

	public override IItemMapper<AdvCategory, AdvCategoryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal AdvCategoryPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<AdvCategoryBusiness>> GetAllAsync(Guid parentGuid, AdvertiseType type, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvCategoryPersistenceRepository.cs", 23));
		using JaegerMonitor monitor = new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvCategoryPersistenceRepository.cs");
		IEnumerable<AdvCategoryBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<AdvCategory> queryable = ((IQueryable<AdvCategory>)c.AdvCategory).Where((AdvCategory q) => q.ParentGuid == parentGuid && q.Type == type);
				if (status.HasValue)
				{
					queryable = queryable?.Where((AdvCategory q) => (bool?)q.Status == status);
				}
				result = await MapFromDatabase.MapListAsync(ContextGuid, queryable.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvCategoryPersistenceRepository.cs", 33);
				throw;
			}
		}
		finally
		{
			monitor?.Dispose();
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvCategoryPersistenceRepository.cs");
			}
		}
		return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvCategoryPersistenceRepository.cs", 56);
			throw;
		}
	}
}
