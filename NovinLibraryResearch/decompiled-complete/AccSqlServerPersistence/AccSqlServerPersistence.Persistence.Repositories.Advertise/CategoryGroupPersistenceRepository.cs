using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class CategoryGroupPersistenceRepository : PersistenceRepository<CategoryGroupBusiness, CategoryGroup>, ICategoryGroupRepository, IRepository<CategoryGroupBusiness>, INoGuidRepository<CategoryGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CategoryGroupBusiness, CategoryGroup>, IItemAsyncMapper<CategoryGroup, CategoryGroupBusiness>
	{
		public Task<CategoryGroup> MapAsync(string contextGuid, CategoryGroupBusiness source, CategoryGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CategoryGroup();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DivarCategoryList = source.DivarCategoryList;
				destination.SheypoorCategoryList = source.SheypoorCategoryList;
				destination.Name = source.Name;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CategoryGroupPersistenceRepository.cs", 64);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<CategoryGroupBusiness> MapAsync(string contextGuid, CategoryGroup source, CategoryGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CategoryGroupBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DivarCategoryList = source.DivarCategoryList;
				destination.SheypoorCategoryList = source.SheypoorCategoryList;
				destination.Name = source.Name;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CategoryGroupPersistenceRepository.cs", 89);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<CategoryGroup>> MapListAsync(string contextGuid, IEnumerable<CategoryGroupBusiness> source, List<CategoryGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CategoryGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<CategoryGroup> source, List<CategoryGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CategoryGroup, CategoryGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CategoryGroupBusiness, CategoryGroup> MapToDatabase => Mapper;

	public override IItemMapper<CategoryGroup, CategoryGroupBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal CategoryGroupPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CategoryGroupPersistenceRepository.cs", 31);
			throw;
		}
	}
}
