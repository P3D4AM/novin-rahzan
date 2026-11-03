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
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Settings;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories;

public abstract class LocalSettingPersistenceRepositoryBase : PersistenceNoGuidRepository<LocalSettingBusiness, LocalSetting>, ILocalSettingRepository, INoGuidRepository<LocalSettingBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<LocalSettingBusiness, LocalSetting>, IItemAsyncMapper<LocalSetting, LocalSettingBusiness>
	{
		public Task<LocalSetting> MapAsync(string contextGuid, LocalSettingBusiness source, LocalSetting destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new LocalSetting();
				}
				destination.Key = source.Key;
				destination.Value = source.Value;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 223);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<LocalSettingBusiness> MapAsync(string contextGuid, LocalSetting source, LocalSettingBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new LocalSettingBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Key = source.Key;
				destination.Value = source.Value;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 247);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<LocalSetting>> MapListAsync(string contextGuid, IEnumerable<LocalSettingBusiness> source, List<LocalSetting> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<LocalSettingBusiness>> MapListAsync(string contextGuid, IEnumerable<LocalSetting> source, List<LocalSettingBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<LocalSetting, LocalSettingBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<LocalSettingBusiness, LocalSetting> MapToDatabase => Mapper;

	public LocalSettingPersistenceRepositoryBase(ContextHolder context)
		: base(context)
	{
	}

	public async Task<string> GetAsync(string Name, CancellationToken token, string tranName)
	{
		Global_ModelContext context = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 26));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs"))
		{
			string result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<LocalSetting>((IQueryable<LocalSetting>)context.LocalSettings, (Expression<Func<LocalSetting, bool>>)((LocalSetting x) => x.Key == Name), token))?.Value;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await context.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(LocalSettingBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext context = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 53));
				LocalSetting item2 = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await context.AddOrUpdateAsync(item2, item.Value), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 55);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 59);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 60);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LocalSettingBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (LocalSettingBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 73);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 77);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 78);
			}
			return ret;
		}
	}

	public string Get(string Name, CancellationToken token, string tranName)
	{
		Global_ModelContext global_ModelContext = (Global_ModelContext)dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 85);
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs"))
		{
			try
			{
				return ((IQueryable<LocalSetting>)global_ModelContext.LocalSettings).FirstOrDefault((LocalSetting x) => x.Key == Name)?.Value;
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
				WebErrorLog.Instance.StartLog(ex3, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 96);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					global_ModelContext.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs");
				}
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string name, string value, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 111));
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await global_ModelContext.AddOrUpdateAsync(new LocalSetting
				{
					Key = name,
					Value = value
				}, name), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 114);
				if (name.ToLower() == "Acc_AbonmanSettings".ToLower())
				{
					ActivationAbonmanSettings activationAbonmanSettings = (Get(name, token, tranName) ?? "").FromJson<ActivationAbonmanSettings>("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 123);
					ActivationAbonmanSettings activationAbonmanSettings2 = value.FromJson<ActivationAbonmanSettings>("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 124);
					if (activationAbonmanSettings2.Version < activationAbonmanSettings.Version)
					{
						throw new ArgumentException($"setting version is not valid!   new:{activationAbonmanSettings2.Version}/ exists:{activationAbonmanSettings.Version}");
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 131);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 132);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(string name, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 145));
				LocalSetting localSetting = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<LocalSetting>((IQueryable<LocalSetting>)dbcontext.LocalSettings, (Expression<Func<LocalSetting, bool>>)((LocalSetting x) => x.Value == name), token);
				if (localSetting != null)
				{
					dbcontext.LocalSettings.Remove(localSetting);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 151);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 152);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(IEnumerable<LocalSettingBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 165));
				LocalSetting localSetting = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<LocalSetting>((IQueryable<LocalSetting>)dbcontext.LocalSettings, token);
				dbcontext.LocalSettings.Remove(localSetting);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 172);
				ret.AddReturnedValue(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 173);
			}
			return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\LocalSettings\\LocalSettingPersistenceRepositoryBase.cs", 192);
		}
	}

	public abstract Task<VersionModel> GetCacheVersionAsync(string sqlConnection, CancellationToken token);

	public abstract Task<VersionModel> GetExeVersionAsync(string sqlConnection, CancellationToken token);
}
