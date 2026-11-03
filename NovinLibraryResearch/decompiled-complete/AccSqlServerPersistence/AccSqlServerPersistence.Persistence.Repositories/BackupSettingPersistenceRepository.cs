using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Uploader;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class BackupSettingPersistenceRepository : PersistenceNoGuidRepository<BackupSettingsBusiness, BackupSettings>, IBackupSettingRepository, INoGuidRepository<BackupSettingsBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BackupSettingsBusiness, BackupSettings>, IItemAsyncMapper<BackupSettings, BackupSettingsBusiness>
	{
		public Task<BackupSettings> MapAsync(string contextGuid, BackupSettingsBusiness source, BackupSettings destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs"))
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
						destination = new BackupSettings();
					}
					destination.Id = source.Id;
					destination.Username = source.Username;
					destination.Password = source.Password;
					destination.Startup = source.Startup;
					destination.Zip = source.Zip;
					destination.RememberMe = source.RememberMe;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 107);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<BackupSettingsBusiness> MapAsync(string contextGuid, BackupSettings source, BackupSettingsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs"))
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
						destination = new BackupSettingsBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Id = source.Id;
					destination.Username = source.Username;
					destination.Password = source.Password;
					destination.Startup = source.Startup;
					destination.Zip = source.Zip;
					destination.RememberMe = source.RememberMe;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 136);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<BackupSettings>> MapListAsync(string contextGuid, IEnumerable<BackupSettingsBusiness> source, List<BackupSettings> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BackupSettingsBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupSettings> source, List<BackupSettingsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BackupSettings, BackupSettingsBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BackupSettingsBusiness, BackupSettings> MapToDatabase => Mapper;

	public BackupSettingPersistenceRepository(ContextHolder context)
		: base(context)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 32);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(BackupSettingsBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 45));
				BackupSettings backupSettings = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(backupSettings, backupSettings.Id), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 47);
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 53);
				throw;
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<BackupSettingsBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (BackupSettingsBusiness item in items)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 68);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 72);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Upload\\BackupSettingPersistenceRepository.cs", 73);
		}
		return ret;
	}
}
