using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class DepotPersistenceRepository : PersistenceSyncRepository<DepotBusiness, Depot>, IDepotRepository, ISyncRepository<DepotBusiness>, IEditableDelivery, IRepository<DepotBusiness>, INoGuidRepository<DepotBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DepotBusiness, Depot>, IItemAsyncMapper<Depot, DepotBusiness>, IItemMapper<Depot, DepotBusiness>
	{
		public DepotBusiness Map(string contextGuid, Depot source, DepotBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new DepotBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Description = source.Description;
					destination.NegativeAmount = source.NegativeAmount;
					destination.Name = source.Name;
					destination.Status = source.Status;
					destination.Guid = source.Guid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.Modified = ((source.Modified < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : source.Modified);
					return destination;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 166);
					throw;
				}
			}
		}

		public Task<Depot> MapAsync(string contextGuid, DepotBusiness source, Depot destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs"))
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
						destination = new Depot();
					}
					destination.Description = source.Description;
					destination.NegativeAmount = source.NegativeAmount;
					destination.Name = source.Name;
					destination.Status = source.Status;
					destination.Guid = source.Guid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.Modified = ((source.Modified < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : source.Modified);
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 199);
					throw;
				}
			}
		}

		public Task<DepotBusiness> MapAsync(string contextGuid, Depot source, DepotBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public Task<List<Depot>> MapListAsync(string contextGuid, IEnumerable<DepotBusiness> source, List<Depot> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DepotBusiness>> MapListAsync(string contextGuid, IEnumerable<Depot> source, List<DepotBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Depot, DepotBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DepotBusiness, Depot> MapToDatabase => Mapper;

	public override IItemMapper<Depot, DepotBusiness> MapFromDatabaseNoneAsync => Mapper;

	internal DepotPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<DepotBusiness> GetByNameAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 25));
		using (new JaegerMonitor("GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs"))
		{
			DepotBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Depot source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Depot>((IQueryable<Depot>)c.Depot, (Expression<Func<Depot, bool>>)((Depot x) => x.Name == name), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(DepotBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext dbContext1 = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 53));
				Depot item2 = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbContext1.AddOrUpdateAsync(item2, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 55);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 59);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 60);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<DepotBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (DepotBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 74);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 78);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 79);
			}
			return ret;
		}
	}

	public override async Task<IEnumerable<DepotBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 86));
		IEnumerable<DepotBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<Depot> queryable = (IQueryable<Depot>)c.Depot;
				if (status.HasValue)
				{
					queryable = queryable?.Where((Depot q) => (bool?)q.Status == status);
				}
				result = await Mapper.MapListAsync(ContextGuid, queryable, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 97);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs");
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
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DepotPersistenceRepository.cs", 120);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
