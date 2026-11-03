using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData.CashierLog;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class CashiersLogPersistenceRepository : PersistenceSyncRepository<CashiersLogBusiness, CashiersLog>, ICashiersLogRepository, ISyncRepository<CashiersLogBusiness>, IEditableDelivery, IRepository<CashiersLogBusiness>, INoGuidRepository<CashiersLogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CashiersLogBusiness, CashiersLog>, IItemAsyncMapper<CashiersLog, CashiersLogBusiness>
	{
		public Task<CashiersLog> MapAsync(string contextGuid, CashiersLogBusiness source, CashiersLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs"))
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
						destination = new CashiersLog();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.Title = source.Title;
					destination.DateM = source.DateM;
					destination.CashierGuid = source.CashierGuid;
					destination.OldSerializedData = source.OldSerializedData;
					destination.NewSerializedData = source.NewSerializedData;
					destination.Action = source.Action;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.System = source.System;
					destination.DateUtc = destination.DateM.ToUniversalTime();
					destination.MetaData = Json.ToStringJson(source.MetaData);
					destination.Source = source.Source;
					destination.ApplicationVersion = source.ApplicationVersion;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs", 120);
					throw;
				}
			}
		}

		public Task<CashiersLogBusiness> MapAsync(string contextGuid, CashiersLog source, CashiersLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs"))
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
						destination = new CashiersLogBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.Title = source.Title;
					destination.DateM = source.DateM;
					destination.CashierGuid = source.CashierGuid;
					destination.OldSerializedData = source.OldSerializedData;
					destination.NewSerializedData = source.NewSerializedData;
					destination.Action = source.Action;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.System = source.System;
					destination.DateUtc = source.DateUtc;
					destination.CashierName = source?.Cashier?.Name ?? "";
					destination.Source = source.Source;
					destination.ApplicationVersion = source.ApplicationVersion;
					if (!string.IsNullOrEmpty(source.MetaData))
					{
						destination.MetaData = source.MetaData.FromJson<CashierLogMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs", 158);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs", 165);
					throw;
				}
			}
		}

		public Task<List<CashiersLog>> MapListAsync(string contextGuid, IEnumerable<CashiersLogBusiness> source, List<CashiersLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CashiersLogBusiness>> MapListAsync(string contextGuid, IEnumerable<CashiersLog> source, List<CashiersLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<CashiersLog, CashiersLogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CashiersLogBusiness, CashiersLog> MapToDatabase => Mapper;

	public override IItemMapper<CashiersLog, CashiersLogBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public new bool IsDisposed { get; private set; }

	internal CashiersLogPersistenceRepository(ContextHolder db)
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
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs", 42);
		}
	}

	public async Task<List<CashiersLogBusiness>> GetAllAsync(CashierLogFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs", 46));
		List<CashiersLogBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<CashiersLog> queryable = (IQueryable<CashiersLog>)EntityFrameworkQueryableExtensions.Include<CashiersLog, Cashier>((IQueryable<CashiersLog>)c.CashiersLogs, (Expression<Func<CashiersLog, Cashier>>)((CashiersLog q) => q.Cashier));
				if (filter.CashierGuid.HasValue && filter.CashierGuid != Guid.Empty)
				{
					queryable = queryable?.Where((CashiersLog q) => q.CashierGuid == filter.CashierGuid);
				}
				if (filter.SanadGuid.HasValue && filter.SanadGuid != Guid.Empty)
				{
					queryable = queryable?.Where((CashiersLog q) => q.SanadGuid == filter.SanadGuid);
				}
				foreach (string word in filter.Description?.FixString()?.RemoveCharactersForLinkSearch()?.SplitString()?.Distinct()?.ToList() ?? new List<string>())
				{
					queryable = queryable?.Where((CashiersLog q) => q.Title.Contains(word));
				}
				if (filter.Date1.HasValue)
				{
					queryable = queryable?.Where((CashiersLog q) => q.DateM >= filter.Date1);
				}
				if (filter.Date2.HasValue)
				{
					queryable = queryable?.Where((CashiersLog q) => q.DateM <= filter.Date2);
				}
				if (filter.SanadType.HasValue && filter.SanadType != EnLog.None)
				{
					queryable = queryable?.Where((CashiersLog q) => (int?)q.SanadType == (int?)filter.SanadType);
				}
				result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList(), null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs", 70);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashiersLogPersistenceRepository.cs");
			}
		}
		return result;
	}
}
