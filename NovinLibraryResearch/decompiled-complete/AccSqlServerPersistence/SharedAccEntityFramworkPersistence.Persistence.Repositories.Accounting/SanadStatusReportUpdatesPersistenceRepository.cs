using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

internal class SanadStatusReportUpdatesPersistenceRepository : PersistenceSyncRepository<SanadStatusReportUpdatesBusiness, SanadStatusReportUpdates>, ISanadStatusReportUpdatesRepository, ISyncRepository<SanadStatusReportUpdatesBusiness>, IEditableDelivery, IRepository<SanadStatusReportUpdatesBusiness>, INoGuidRepository<SanadStatusReportUpdatesBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SanadStatusReportUpdatesBusiness, SanadStatusReportUpdates>, IItemAsyncMapper<SanadStatusReportUpdates, SanadStatusReportUpdatesBusiness>
	{
		public Task<SanadStatusReportUpdates> MapAsync(string contextGuid, SanadStatusReportUpdatesBusiness source, SanadStatusReportUpdates destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadStatusReportUpdates();
				}
				destination.Guid = source.Guid;
				destination.AsGuid = source.AsGuid;
				destination.Modified = source.Modified;
				destination.AsType = source.AsType;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.StatusId = source.StatusId;
				destination.CashierGuid = source.CashierGuid;
				destination.Ordinal = source.Ordinal;
				destination.DateM = source.DateM;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportUpdatesPersistenceRepository.cs", 76);
				throw;
			}
		}

		public Task<SanadStatusReportUpdatesBusiness> MapAsync(string contextGuid, SanadStatusReportUpdates source, SanadStatusReportUpdatesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadStatusReportUpdatesBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.AsGuid = source.AsGuid;
				destination.Modified = source.Modified;
				destination.AsType = source.AsType;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.StatusId = source.StatusId;
				destination.CashierGuid = source.CashierGuid;
				destination.Ordinal = source.Ordinal;
				destination.DateM = source.DateM;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportUpdatesPersistenceRepository.cs", 113);
				throw;
			}
		}

		public Task<List<SanadStatusReportUpdates>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReportUpdatesBusiness> source, List<SanadStatusReportUpdates> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SanadStatusReportUpdatesBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReportUpdates> source, List<SanadStatusReportUpdatesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<SanadStatusReportUpdates, SanadStatusReportUpdatesBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SanadStatusReportUpdates, SanadStatusReportUpdatesBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SanadStatusReportUpdatesBusiness, SanadStatusReportUpdates> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public SanadStatusReportUpdatesPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = false;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusReportUpdatesPersistenceRepository.cs", 39);
		}
	}
}
