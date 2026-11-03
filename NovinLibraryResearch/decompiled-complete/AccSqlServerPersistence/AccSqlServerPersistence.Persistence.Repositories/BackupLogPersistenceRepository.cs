using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Uploader;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class BackupLogPersistenceRepository : PersistenceNoGuidRepository<BackupLogBusiness, BackupLog>, IBackupLogRepository, INoGuidRepository<BackupLogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BackupLogBusiness, BackupLog>, IItemAsyncMapper<BackupLog, BackupLogBusiness>
	{
		public Task<BackupLog> MapAsync(string contextGuid, BackupLogBusiness source, BackupLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs"))
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
						destination = new BackupLog();
					}
					destination.Id = source.Id;
					destination.DPId = source.DPId;
					destination.FileId = source.FileId;
					destination.LastBackup = source.LastBackup;
					destination.Status = source.Status;
					destination.Path = source.Path;
					destination.LastStateChange = source.LastStateChange;
					destination.Message = source.Message;
					destination.RetryCount = source.RetryCount;
					destination.TotalCrc = source.TotalCrc;
					destination.URL = source.URL;
					destination.FileName = source.FileName;
					destination.BackupType = source.BackupType;
					destination.IsDelete = source.IsDelete;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 146);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<BackupLogBusiness> MapAsync(string contextGuid, BackupLog source, BackupLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs"))
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
						destination = new BackupLogBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Id = source.Id;
					destination.DPId = source.DPId;
					destination.FileId = source.FileId;
					destination.LastBackup = source.LastBackup;
					destination.Status = source.Status;
					destination.Path = source.Path;
					destination.LastStateChange = source.LastStateChange;
					destination.Message = source.Message;
					destination.RetryCount = source.RetryCount;
					destination.TotalCrc = source.TotalCrc;
					destination.URL = source.URL;
					destination.FileName = source.FileName;
					destination.BackupType = source.BackupType;
					destination.IsDelete = source.IsDelete;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 183);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<BackupLog>> MapListAsync(string contextGuid, IEnumerable<BackupLogBusiness> source, List<BackupLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BackupLogBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupLog> source, List<BackupLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BackupLog, BackupLogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BackupLogBusiness, BackupLog> MapToDatabase => Mapper;

	public BackupLogPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public async Task<BackupLogBusiness> GetAsync(Guid fileId, long crc, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 24));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs"))
		{
			BackupLogBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					BackupLog source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<BackupLog>((IQueryable<BackupLog>)c.BackUpLog, (Expression<Func<BackupLog, bool>>)((BackupLog f) => f.FileId == fileId && f.TotalCrc == crc), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 35);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs");
				}
			}
			return result;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 56);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(BackupLogBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 68);
				BackupLog backupLog = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(backupLog, backupLog.Id), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 70);
				return ret;
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 76);
				throw;
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<BackupLogBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 90));
				foreach (BackupLog item in await Mapper.MapListAsync(base.ContextGuid, items?.ToList(), null, tranName, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.Id), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 94);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 98);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupLogPersistenceRepository.cs", 99);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> RemoveAllAsync(int tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
