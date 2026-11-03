using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Uploader;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class BackupSchedulePersistenceRepository : PersistenceRepository<BackupScheduleBusiness, BackupSchedule>, IBackupScheduleRepository, IRepository<BackupScheduleBusiness>, INoGuidRepository<BackupScheduleBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BackupScheduleBusiness, BackupSchedule>, IItemAsyncMapper<BackupSchedule, BackupScheduleBusiness>
	{
		public Task<BackupSchedule> MapAsync(string contextGuid, BackupScheduleBusiness source, BackupSchedule destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSchedulePersistenceRepository.cs"))
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
						destination = new BackupSchedule();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Title = source.Title;
					destination.ConnectionString = source.ConnectionString;
					destination.BackupType = source.BackupType;
					destination.Houre = source.Houre;
					destination.CreateDate = source.CreateDate;
					destination.BackupPath = source.BackupPath;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSchedulePersistenceRepository.cs", 71);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<BackupScheduleBusiness> MapAsync(string contextGuid, BackupSchedule source, BackupScheduleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSchedulePersistenceRepository.cs"))
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
						destination = new BackupScheduleBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Title = source.Title;
					destination.ConnectionString = source.ConnectionString;
					destination.BackupType = source.BackupType;
					destination.Houre = source.Houre;
					destination.CreateDate = source.CreateDate;
					destination.BackupPath = source.BackupPath;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSchedulePersistenceRepository.cs", 102);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<BackupSchedule>> MapListAsync(string contextGuid, IEnumerable<BackupScheduleBusiness> source, List<BackupSchedule> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BackupScheduleBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupSchedule> source, List<BackupScheduleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BackupSchedule, BackupScheduleBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BackupScheduleBusiness, BackupSchedule> MapToDatabase => Mapper;

	public override IItemMapper<BackupSchedule, BackupScheduleBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public BackupSchedulePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSchedulePersistenceRepository.cs", 33);
		}
	}
}
