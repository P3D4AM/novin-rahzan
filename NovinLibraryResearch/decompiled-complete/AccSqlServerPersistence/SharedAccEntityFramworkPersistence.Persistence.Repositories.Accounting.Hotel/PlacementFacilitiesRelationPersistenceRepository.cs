using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class PlacementFacilitiesRelationPersistenceRepository : PersistenceNoGuidRepository<PlacementFacilitiesRelationBusiness, PlacementFacilitiesRelation>, IPlacementFacilitiesRelationRepository, INoGuidRepository<PlacementFacilitiesRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PlacementFacilitiesRelationBusiness, PlacementFacilitiesRelation>, IItemAsyncMapper<PlacementFacilitiesRelation, PlacementFacilitiesRelationBusiness>
	{
		public Task<PlacementFacilitiesRelation> MapAsync(string contextGuid, PlacementFacilitiesRelationBusiness source, PlacementFacilitiesRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PlacementFacilitiesRelation();
				}
				destination.PlacementGuid = source.PlacementGuid;
				destination.FacilitiesGuid = source.FacilitiesGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 138);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<PlacementFacilitiesRelationBusiness> MapAsync(string contextGuid, PlacementFacilitiesRelation source, PlacementFacilitiesRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PlacementFacilitiesRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.PlacementGuid = source.PlacementGuid;
				destination.FacilitiesGuid = source.FacilitiesGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 161);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<PlacementFacilitiesRelation>> MapListAsync(string contextGuid, IEnumerable<PlacementFacilitiesRelationBusiness> source, List<PlacementFacilitiesRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PlacementFacilitiesRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<PlacementFacilitiesRelation> source, List<PlacementFacilitiesRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<PlacementFacilitiesRelation, PlacementFacilitiesRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PlacementFacilitiesRelationBusiness, PlacementFacilitiesRelation> MapToDatabase => Mapper;

	public PlacementFacilitiesRelationPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 37);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PlacementFacilitiesRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 45));
			if (item == null)
			{
				return ret;
			}
			PlacementFacilitiesRelation placementFacilitiesRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(placementFacilitiesRelation, placementFacilitiesRelation.PlacementGuid, item.FacilitiesGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 48);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 52);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 53);
		}
		return ret;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PlacementFacilitiesRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (items == null || !items.Any())
		{
			return ret;
		}
		try
		{
			_ = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 64));
			foreach (PlacementFacilitiesRelationBusiness item in items)
			{
				if (item != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 68);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 73);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 74);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid placementGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 85));
				IQueryable<PlacementFacilitiesRelation> queryable = ((IQueryable<PlacementFacilitiesRelation>)((DbContext)acc_ModelContext).Set<PlacementFacilitiesRelation>()).Where((PlacementFacilitiesRelation x) => x.PlacementGuid == placementGuid);
				((DbContext)acc_ModelContext).Set<PlacementFacilitiesRelation>().RemoveRange((IEnumerable<PlacementFacilitiesRelation>)queryable);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 91);
				ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 92);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<PlacementFacilitiesRelationBusiness>> GetAllAsync(Guid placementGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 98));
		IEnumerable<PlacementFacilitiesRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<PlacementFacilitiesRelation> queryable = ((IQueryable<PlacementFacilitiesRelation>)c.PlacementFacilitieRelation).Where((PlacementFacilitiesRelation q) => q.PlacementGuid == placementGuid);
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs", 109);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementFacilitiesRelationPersistenceRepository.cs");
			}
		}
		return result;
	}
}
