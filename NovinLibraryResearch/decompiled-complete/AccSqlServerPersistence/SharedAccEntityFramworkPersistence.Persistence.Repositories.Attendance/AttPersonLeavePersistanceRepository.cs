using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Attendance;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal class AttPersonLeavePersistanceRepository : PersistenceSyncRepository<AttPersonLeaveBusiness, AttPersonLeave>, IAttPersonLeaveRepository, ISyncRepository<AttPersonLeaveBusiness>, IEditableDelivery, IRepository<AttPersonLeaveBusiness>, INoGuidRepository<AttPersonLeaveBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AttPersonLeaveBusiness, AttPersonLeave>, IItemAsyncMapper<AttPersonLeave, AttPersonLeaveBusiness>
	{
		public Task<AttPersonLeave> MapAsync(string contextGuid, AttPersonLeaveBusiness source, AttPersonLeave destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AttPersonLeave();
				}
				destination.Guid = source.Guid;
				destination.PersonGuid = source.PersonGuid;
				destination.LeaveStatus = source.LeaveStatus;
				destination.DateFrom = source.DateFrom;
				destination.DateTo = source.DateTo;
				destination.Reason = source.Reason;
				destination.Type = source.Type;
				destination.ManagerDate = source.ManagerDate;
				destination.ManagerMessage = source.ManagerMessage;
				destination.Modified = source.Modified;
				destination.Kind = source.Kind;
				destination.RequestDate = source.RequestDate;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Quiddity = source.Quiddity;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonLeavePersistanceRepository.cs", 106);
				throw;
			}
		}

		public Task<AttPersonLeaveBusiness> MapAsync(string contextGuid, AttPersonLeave source, AttPersonLeaveBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AttPersonLeaveBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.PersonGuid = source.PersonGuid;
				destination.LeaveStatus = source.LeaveStatus;
				destination.DateFrom = source.DateFrom;
				destination.DateTo = source.DateTo;
				destination.Reason = source.Reason;
				destination.Type = source.Type;
				destination.ManagerDate = source.ManagerDate;
				destination.ManagerMessage = source.ManagerMessage;
				destination.Modified = source.Modified;
				destination.Kind = source.Kind;
				destination.RequestDate = source.RequestDate;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Quiddity = source.Quiddity;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonLeavePersistanceRepository.cs", 146);
				throw;
			}
		}

		public Task<List<AttPersonLeave>> MapListAsync(string contextGuid, IEnumerable<AttPersonLeaveBusiness> source, List<AttPersonLeave> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AttPersonLeaveBusiness>> MapListAsync(string contextGuid, IEnumerable<AttPersonLeave> source, List<AttPersonLeaveBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<AttPersonLeave, AttPersonLeaveBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AttPersonLeaveBusiness, AttPersonLeave> MapToDatabase => Mapper;

	public override IItemMapper<AttPersonLeave, AttPersonLeaveBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public AttPersonLeavePersistanceRepository(ContextHolder dbContext)
		: base(dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonLeavePersistanceRepository.cs", 41);
		}
	}

	public override async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonLeavePersistanceRepository.cs"))
		{
			Att_ModelContext att_ModelContext = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonLeavePersistanceRepository.cs", 46));
			List<Guid> result;
			try
			{
				List<AttPersonLeave> list = ((IQueryable<AttPersonLeave>)att_ModelContext.AttPersonLeaves).Where((AttPersonLeave x) => x.ServerStatus == ServerStatusForClient.None || x.ServerStatus == ServerStatusForClient.SendError || x.ServerStatus == ServerStatusForClient.DeliveryError)?.ToList();
				result = ((list != null && list.Count <= 0) ? null : list?.Select((AttPersonLeave q) => q.Guid)?.ToList());
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonLeavePersistanceRepository.cs", 59);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await att_ModelContext.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonLeavePersistanceRepository.cs");
				}
			}
			return result;
		}
	}
}
