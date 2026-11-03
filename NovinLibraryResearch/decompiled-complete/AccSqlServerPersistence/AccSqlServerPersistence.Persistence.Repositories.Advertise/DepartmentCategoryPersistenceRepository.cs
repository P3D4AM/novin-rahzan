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

internal class DepartmentCategoryPersistenceRepository : PersistenceRepository<DepartmentCategoryBusiness, DepartmentCategory>, IDepartmentCategoryRepository, IRepository<DepartmentCategoryBusiness>, INoGuidRepository<DepartmentCategoryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DepartmentCategoryBusiness, DepartmentCategory>, IItemAsyncMapper<DepartmentCategory, DepartmentCategoryBusiness>
	{
		public Task<DepartmentCategory> MapAsync(string contextGuid, DepartmentCategoryBusiness source, DepartmentCategory destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DepartmentCategory();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DepartmentGuid = source.DepartmentGuid;
				destination.FirstLevel = source.FirstLevel;
				destination.SecondLevel = source.SecondLevel;
				destination.ThirdLevel = source.ThirdLevel;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DepartmentCategoryPersistenceRepository.cs", 96);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<DepartmentCategoryBusiness> MapAsync(string contextGuid, DepartmentCategory source, DepartmentCategoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DepartmentCategoryBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DepartmentGuid = source.DepartmentGuid;
				destination.FirstLevel = source.FirstLevel;
				destination.SecondLevel = source.SecondLevel;
				destination.ThirdLevel = source.ThirdLevel;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DepartmentCategoryPersistenceRepository.cs", 123);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<DepartmentCategory>> MapListAsync(string contextGuid, IEnumerable<DepartmentCategoryBusiness> source, List<DepartmentCategory> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DepartmentCategoryBusiness>> MapListAsync(string contextGuid, IEnumerable<DepartmentCategory> source, List<DepartmentCategoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<DepartmentCategory, DepartmentCategoryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DepartmentCategoryBusiness, DepartmentCategory> MapToDatabase => Mapper;

	public override IItemMapper<DepartmentCategory, DepartmentCategoryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal DepartmentCategoryPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<DepartmentCategoryBusiness>> GetAllAsync(Guid depGuid, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DepartmentCategoryPersistenceRepository.cs", 26));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DepartmentCategoryPersistenceRepository.cs"))
		{
			IEnumerable<DepartmentCategoryBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<DepartmentCategory> queryable = ((IQueryable<DepartmentCategory>)c.DepartmentCategories)?.Where((DepartmentCategory q) => q.DepartmentGuid == depGuid);
					if (status.HasValue)
					{
						queryable = queryable?.Where((DepartmentCategory q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList());
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DepartmentCategoryPersistenceRepository.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DepartmentCategoryPersistenceRepository.cs");
				}
			}
			return result;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DepartmentCategoryPersistenceRepository.cs", 61);
			throw;
		}
	}
}
