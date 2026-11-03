using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace AccSqlServerPersistence.Persistence;

public abstract class PersistenceSyncRepository<T, U> : PersistenceRepository<T, U>, ISyncRepository<T>, IEditableDelivery, IRepository<T>, INoGuidRepository<T>, IDisposable where T : class, IHasJustGuid, IDeliverableEntity, IHasContext where U : class, IHasJustGuid, IDeliverableEntity, new()
{
	private bool _isDisposed;

	internal PersistenceSyncRepository(ContextHolder db)
		: base(db)
	{
	}

	public new virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 30);
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 35);
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				U val = ((DbSet<_003F>)(object)((DbContext)baseModelContext).Set<U>()).Find(new object[1] { item.Guid });
				if (val == null)
				{
					return returnedSaveFuncInfo;
				}
				val.HostRefrenced = item.HostRefrenced;
				val.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				val.DeliveryMessage = item.Message;
				val.DeliveryShortMessage = item.Shortmessage;
				((DbContext)baseModelContext).Update<U>(val);
			}
			catch (OperationCanceledException exception)
			{
				returnedSaveFuncInfo.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 49);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 53);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 54);
			}
			return returnedSaveFuncInfo;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 64));
				T val = ((DbContext)acc_ModelContext).Find<T>(new object[1] { guid }) ?? throw new Exception($"Object not found \r\n ObjectGuid:{guid}");
				val.ServerStatus = status;
				((DbContext)acc_ModelContext).Update<T>(val);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 69);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 73);
				ret.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 74);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		if (del == null)
		{
			throw new ArgumentNullException("Delivery is null!! del");
		}
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbContext = await base.dbContext.GetContextAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 86);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 87);
				}
				IQueryable<U> queryable = ((IQueryable<U>)((DbContext)dbContext).Set<U>()).Where((U x) => x.Guid == del.Guid);
				U val = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>(queryable, token) : null);
				if (val == null)
				{
					return ret;
				}
				val.ServerStatus = (del.DeliveryStatus ? ServerStatusForClient.Delivered : ServerStatusForClient.DeliveryError);
				val.DeliveryDate = DateTime.Now;
				val.DeliveryId = del.DeliveryId;
				val.DeliveryMessage = del.Message;
				val.DeliveryShortMessage = del.ShortMessage;
				((DbContext)dbContext).Update<U>(val);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 97);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 101);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 102);
			}
			return ret;
		}
	}

	public virtual async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs"))
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 109);
			List<Guid> result;
			try
			{
				_ = 1;
				try
				{
					List<U> list = await EntityFrameworkQueryableExtensions.ToListAsync<U>(((IQueryable<U>)((DbContext)c).Set<U>()).Where((U x) => x.ServerStatus == ServerStatusForClient.None || x.ServerStatus == ServerStatusForClient.SendError), token);
					result = ((list.Count <= 0) ? null : list.Select((U q) => q.Guid).ToList());
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs", 119);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceSyncRepository.cs");
				}
			}
			return result;
		}
	}
}
