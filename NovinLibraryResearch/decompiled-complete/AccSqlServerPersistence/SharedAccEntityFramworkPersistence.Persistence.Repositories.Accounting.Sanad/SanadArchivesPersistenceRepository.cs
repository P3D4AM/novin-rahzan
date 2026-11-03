using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Sanad;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Sanad;

internal class SanadArchivesPersistenceRepository : PersistenceSyncRepository<SanadArchivesBusiness, SanadArchives>, ISanadArchivesRepository, ISyncRepository<SanadArchivesBusiness>, IEditableDelivery, IRepository<SanadArchivesBusiness>, INoGuidRepository<SanadArchivesBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SanadArchivesBusiness, SanadArchives>, IItemAsyncMapper<SanadArchives, SanadArchivesBusiness>
	{
		public Task<SanadArchives> MapAsync(string contextGuid, SanadArchivesBusiness source, SanadArchives destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadArchives();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.AsType = source.AsType;
				destination.AsGuid = source.AsGuid;
				destination.Price = source.Price;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.Data = source.Data;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadArchivesPersistenceRepository.cs", 77);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<SanadArchivesBusiness> MapAsync(string contextGuid, SanadArchives source, SanadArchivesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadArchivesBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.AsType = source.AsType;
				destination.AsGuid = source.AsGuid;
				destination.Price = source.Price;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.Data = source.Data;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadArchivesPersistenceRepository.cs", 111);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<SanadArchives>> MapListAsync(string contextGuid, IEnumerable<SanadArchivesBusiness> source, List<SanadArchives> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SanadArchivesBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadArchives> source, List<SanadArchivesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemMapper<SanadArchives, SanadArchivesBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SanadArchives, SanadArchivesBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SanadArchivesBusiness, SanadArchives> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public SanadArchivesPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadArchivesPersistenceRepository.cs", 42);
		}
	}
}
