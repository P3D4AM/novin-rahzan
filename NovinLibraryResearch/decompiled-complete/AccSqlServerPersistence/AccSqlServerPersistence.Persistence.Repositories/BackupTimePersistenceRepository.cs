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

internal class BackupTimePersistenceRepository : PersistenceRepository<BackupTimeBusiness, BackupTime>, IBackupTimeRepository, IRepository<BackupTimeBusiness>, INoGuidRepository<BackupTimeBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BackupTimeBusiness, BackupTime>, IItemAsyncMapper<BackupTime, BackupTimeBusiness>
	{
		public Task<BackupTime> MapAsync(string contextGuid, BackupTimeBusiness source, BackupTime destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupTimePersistenceRepository.cs"))
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
						destination = new BackupTime();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ParentId = source.ParentId;
					destination.DayOfWeek = source.DayOfWeek;
					destination.DateTime = source.DateTime;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupTimePersistenceRepository.cs", 68);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<BackupTimeBusiness> MapAsync(string contextGuid, BackupTime source, BackupTimeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupTimePersistenceRepository.cs"))
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
						destination = new BackupTimeBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ParentId = source.ParentId;
					destination.DayOfWeek = source.DayOfWeek;
					destination.DateTime = source.DateTime;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupTimePersistenceRepository.cs", 96);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<BackupTime>> MapListAsync(string contextGuid, IEnumerable<BackupTimeBusiness> source, List<BackupTime> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BackupTimeBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupTime> source, List<BackupTimeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BackupTime, BackupTimeBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BackupTimeBusiness, BackupTime> MapToDatabase => Mapper;

	public override IItemMapper<BackupTime, BackupTimeBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public BackupTimePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupTimePersistenceRepository.cs", 33);
		}
	}
}
