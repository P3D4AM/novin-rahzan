using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Global;

internal class PushNotificationPersistenceRepository : PersistenceRepository<PushNotificationBusiness, PushNotification>, IPushNotificationRepository, IRepository<PushNotificationBusiness>, INoGuidRepository<PushNotificationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PushNotificationBusiness, PushNotification>, IItemAsyncMapper<PushNotification, PushNotificationBusiness>
	{
		public Task<PushNotification> MapAsync(string contextGuid, PushNotificationBusiness source, PushNotification destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new PushNotification();
					}
					destination.Guid = source.Guid;
					destination.Date = source.Date;
					destination.DisplayCount = source.DisplayCount;
					destination.ExpireDate = source.ExpireDate;
					destination.Message = source.Message;
					destination.Title = source.Title;
					destination.Source = source.Source;
					destination.Type = source.Type;
					destination.ModifiedDate = source.ModifiedDate;
					destination.HelperUri = source.HelperUri;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 155);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<PushNotificationBusiness> MapAsync(string contextGuid, PushNotification source, PushNotificationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new PushNotificationBusiness(contextGuid);
					}
					destination.Guid = source.Guid;
					destination.Date = source.Date;
					destination.DisplayCount = source.DisplayCount;
					destination.ExpireDate = source.ExpireDate;
					destination.Message = source.Message;
					destination.Title = source.Title;
					destination.Source = source.Source;
					destination.Type = source.Type;
					destination.ModifiedDate = source.ModifiedDate;
					destination.HelperUri = source.HelperUri;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 184);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<PushNotification>> MapListAsync(string contextGuid, IEnumerable<PushNotificationBusiness> source, List<PushNotification> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PushNotificationBusiness>> MapListAsync(string contextGuid, IEnumerable<PushNotification> source, List<PushNotificationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<PushNotification, PushNotificationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PushNotificationBusiness, PushNotification> MapToDatabase => Mapper;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public override IItemMapper<PushNotification, PushNotificationBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PushNotificationBusiness pushNotificationBusiness, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Global_ModelContext context = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 31));
				PushNotification item = await MapToDatabase.MapAsync(ContextGuid, pushNotificationBusiness, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await context.AddOrUpdateAsync(item, pushNotificationBusiness.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 33);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 37);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 38);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PushNotificationBusiness> lstPushNotification, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				List<PushNotificationBusiness> list = lstPushNotification.ToList();
				if (list.Any())
				{
					foreach (PushNotificationBusiness item in list)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 53);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 57);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 58);
			}
			return ret;
		}
	}

	public async Task<DateTime> GetLastModifiedDate(CancellationToken token, string tranName)
	{
		Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastModifiedDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 64));
		DateTime ret = DateTime.Now.AddDays(-60.0);
		try
		{
			if (global_ModelContext.PushNotification != null && ((IQueryable<PushNotification>)global_ModelContext.PushNotification).Any())
			{
				ret = (((IQueryable<PushNotification>)global_ModelContext.PushNotification)?.Max((PushNotification q) => q.ModifiedDate)).Value;
			}
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetLastModifiedDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 75);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await global_ModelContext.DisposeAsync("GetLastModifiedDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs");
			}
		}
		return ret;
	}

	public async Task<IEnumerable<PushNotificationBusiness>> GetActiveNotifcation(ENSource source, CancellationToken token, string tranName)
	{
		Global_ModelContext context = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetActiveNotifcation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 89));
		IEnumerable<PushNotificationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<PushNotification> source2 = ((IQueryable<PushNotification>)context.PushNotification).Where((PushNotification p) => p.ExpireDate > DateTime.Now && p.Source == source);
				result = await MapFromDatabase.MapListAsync(ContextGuid, source2, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetActiveNotifcation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 99);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await context.DisposeAsync("GetActiveNotifcation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public PushNotificationPersistenceRepository(ContextHolder context)
		: base(context)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PushNotificationPersistenceRepository.cs", 126);
		}
	}
}
