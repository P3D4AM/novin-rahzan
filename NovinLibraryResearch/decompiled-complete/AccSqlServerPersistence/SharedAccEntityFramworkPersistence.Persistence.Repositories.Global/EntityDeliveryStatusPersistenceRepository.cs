using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global;

internal class EntityDeliveryStatusPersistenceRepository : PersistenceNoGuidRepository<EntityDeliveryStatusBusiness, EntityDeliveryStatus>, IEntityDeliveryStatusRepository, INoGuidRepository<EntityDeliveryStatusBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<EntityDeliveryStatus, EntityDeliveryStatusBusiness>, IItemAsyncMapper<EntityDeliveryStatusBusiness, EntityDeliveryStatus>
	{
		public Task<EntityDeliveryStatusBusiness> MapAsync(string contextGuid, EntityDeliveryStatus source, EntityDeliveryStatusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new EntityDeliveryStatusBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.EntityGuid = source.EntityGuid;
				destination.EntityPart = source.EntityPart;
				destination.IsTemp = source.IsTemp;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 140);
				throw;
			}
		}

		public Task<EntityDeliveryStatus> MapAsync(string contextGuid, EntityDeliveryStatusBusiness source, EntityDeliveryStatus destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new EntityDeliveryStatus();
				}
				destination.EntityGuid = source.EntityGuid;
				destination.EntityPart = source.EntityPart;
				destination.IsTemp = source.IsTemp;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 161);
				throw;
			}
		}

		public Task<List<EntityDeliveryStatusBusiness>> MapListAsync(string contextGuid, IEnumerable<EntityDeliveryStatus> source, List<EntityDeliveryStatusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<EntityDeliveryStatus>> MapListAsync(string contextGuid, IEnumerable<EntityDeliveryStatusBusiness> source, List<EntityDeliveryStatus> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<EntityDeliveryStatus, EntityDeliveryStatusBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<EntityDeliveryStatusBusiness, EntityDeliveryStatus> MapToDatabase => Mapper;

	public EntityDeliveryStatusPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 40);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(EntityDeliveryStatusBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 49));
				EntityDeliveryStatus entityDeliveryStatus = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(entityDeliveryStatus, entityDeliveryStatus.EntityPart, entityDeliveryStatus.IsTemp, entityDeliveryStatus.EntityGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 51);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 55);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 56);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<EntityDeliveryStatusBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (!(items?.Any() ?? false))
				{
					return res;
				}
				foreach (EntityDeliveryStatusBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 69);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 73);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 74);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid entityGuid, EntityPart entityPart, bool isTemp, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (entityGuid == Guid.Empty)
				{
					return res.AddError("parameter is null", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 85);
				}
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 86));
				EntityDeliveryStatus entityDeliveryStatus = await dbcontext.EntityDeliveryStatus.FindAsync(new object[3] { entityPart, isTemp, entityGuid });
				if (entityDeliveryStatus == null)
				{
					return res;
				}
				((DbContext)dbcontext).Remove<EntityDeliveryStatus>(entityDeliveryStatus);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 93);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 94);
			}
			return res;
		}
	}

	public async Task<IEnumerable<EntityDeliveryStatusBusiness>> GetAllAsync(EntityPart entityPart, bool isTemp, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 100));
		IEnumerable<EntityDeliveryStatusBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<EntityDeliveryStatus> list = await EntityFrameworkQueryableExtensions.ToListAsync<EntityDeliveryStatus>(((IQueryable<EntityDeliveryStatus>)c.EntityDeliveryStatus).Where((EntityDeliveryStatus q) => q.EntityPart == entityPart && q.IsTemp == isTemp), token);
				result = ((list != null && list.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, list, null, tranName, token)) : null);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs", 110);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\EntityDeliveryStatusPersistenceRepository.cs");
			}
		}
		return result;
	}
}
