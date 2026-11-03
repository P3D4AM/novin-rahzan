using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class VisitPersistenceRepository : PersistenceSyncRepository<VisitBusiness, Visit>, IVisitRepository, ISyncRepository<VisitBusiness>, IEditableDelivery, IRepository<VisitBusiness>, INoGuidRepository<VisitBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<VisitBusiness, Visit>, IItemAsyncMapper<Visit, VisitBusiness>
	{
		public Task<Visit> MapAsync(string contextGuid, VisitBusiness source, Visit destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Visit();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ScheduledDate = source.ScheduledDate;
				destination.StartDate = source.StartDate;
				destination.CreationdDate = source.CreationdDate;
				destination.Description = source.Description;
				destination.VisitNo = source.VisitNo;
				destination.VisitIsCompleted = source.VisitIsCompleted;
				destination.SerializedTicketGuids = source.GetSerializedTicketGuids();
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VisitPersistenceRepository.cs", 73);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<VisitBusiness> MapAsync(string contextGuid, Visit source, VisitBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VisitPersistenceRepository.cs"))
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
						destination = new VisitBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ScheduledDate = source.ScheduledDate;
					destination.StartDate = source.StartDate;
					destination.CreationdDate = source.CreationdDate;
					destination.Description = source.Description;
					destination.VisitNo = source.VisitNo;
					destination.VisitIsCompleted = source.VisitIsCompleted;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VisitPersistenceRepository.cs", 111);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Visit>> MapListAsync(string contextGuid, IEnumerable<VisitBusiness> source, List<Visit> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<VisitBusiness>> MapListAsync(string contextGuid, IEnumerable<Visit> source, List<VisitBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Visit, VisitBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<VisitBusiness, Visit> MapToDatabase => Mapper;

	public override IItemMapper<Visit, VisitBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal VisitPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VisitPersistenceRepository.cs", 30);
		}
	}
}
