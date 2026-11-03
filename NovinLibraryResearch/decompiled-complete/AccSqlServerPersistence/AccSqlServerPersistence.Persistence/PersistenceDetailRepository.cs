using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Core;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence;

public abstract class PersistenceDetailRepository<T, U> : IDetailRepository<T>, IEditableDelivery, IDisposable where T : class, IHasGuid, IDeliverableEntity, IHasContext, IHasChangeStatus where U : class, IHasGuid, IDeliverableEntity, new()
{
	private bool _isDisposed;

	public ContextHolder dbContext;

	public abstract IItemMapper<U, T> MapFromDatabase_noneAsync { get; }

	public abstract IItemAsyncMapper<U, T> MapFromDatabase { get; }

	public abstract IItemAsyncMapper<T, U> MapToDatabase { get; }

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => dbContext.Context.Guid;

	public async Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		BaseModelContext c = null;
		int result;
		try
		{
			c = await dbContext.GetContextAsync(tranName, 60000, "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 29);
			result = ((!status.HasValue) ? ((IQueryable<U>)((DbContext)c).Set<U>()).Count() : ((!status.Value) ? ((IQueryable<U>)((DbContext)c).Set<U>()).Count((U q) => !q.Status) : ((IQueryable<U>)((DbContext)c).Set<U>()).Count((U q) => q.Status)));
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
			WebErrorLog.Instance.StartLog(ex2, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 38);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs");
			}
		}
		return result;
	}

	internal PersistenceDetailRepository(ContextHolder dbcontext)
	{
		dbContext = dbcontext;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		BaseModelContext context = await dbContext.GetContextAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 63);
		using (new JaegerMonitor("DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (items == null || !items.Any())
				{
					return new ReturnedSaveFuncInfo();
				}
				foreach (Guid guid in items)
				{
					U val = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>((IQueryable<U>)((DbContext)context).Set<U>(), (Expression<Func<U, bool>>)((U p) => p.Guid == guid), token);
					if (val != null)
					{
						((DbSet<_003F>)(object)((DbContext)context).Set<U>()).Remove(val);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 83);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 84);
			}
			return ret;
		}
	}

	public virtual async Task<T> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = null;
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			T result;
			try
			{
				_ = 2;
				try
				{
					c = await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 96);
					U source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<U>((IQueryable<U>)((DbContext)c).Set<U>(), (Expression<Func<U, bool>>)((U x) => x.Guid == guid), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 105);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual T Get(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext baseModelContext = null;
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			try
			{
				baseModelContext = dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 121);
				U source = ((IQueryable<U>)((DbContext)baseModelContext).Set<U>()).SingleOrDefault((U x) => x.Guid == guid);
				return MapFromDatabase_noneAsync.Map(ContextGuid, source, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex3, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 130);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					baseModelContext.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs");
				}
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 148);
				U val = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>((IQueryable<U>)((DbContext)dbcontext).Set<U>(), (Expression<Func<U, bool>>)((U x) => x.Guid == item), token);
				if (val != null)
				{
					((DbSet<_003F>)(object)((DbContext)dbcontext).Set<U>()).Remove(val);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 154);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 155);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 167);
				IEnumerable<Guid> enumerable = ((from x in ((IEnumerable<U>)((DbContext)dbcontext).Set<U>())?.ToList()
					select x.Guid)).Except(items);
				if (!enumerable.Any())
				{
					return new ReturnedSaveFuncInfo();
				}
				foreach (Guid guid in enumerable)
				{
					U val = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>((IQueryable<U>)((DbContext)dbcontext).Set<U>(), (Expression<Func<U, bool>>)((U p) => p.Guid == guid), token);
					if (val != null)
					{
						((DbSet<_003F>)(object)((DbContext)dbcontext).Set<U>()).Remove(val);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 184);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 185);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 199);
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				U val = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(val, val.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 206);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 215);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 216);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			_ = 1;
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 228);
				foreach (U item in await MapToDatabase.MapListAsync(ContextGuid, items?.ToList(), null, tranName, token))
				{
					item.ServerStatus = ServerStatusForClient.Delivered;
					((DbContext)dbcontext).Update<U>(item);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 243);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 244);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 259);
				IQueryable<U> queryable = ((IQueryable<U>)((DbContext)dbcontext).Set<U>()).Where((U x) => x.Guid == item.Guid);
				U val = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>(queryable, token) : null);
				if (val == null)
				{
					return ret;
				}
				val.HostRefrenced = item.HostRefrenced;
				if (!item.SaveResultStatus)
				{
					val.ServerStatus = ServerStatusForClient.SendError;
				}
				else if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					val.ServerStatus = ServerStatusForClient.Delivered;
				}
				else
				{
					val.ServerStatus = ServerStatusForClient.Sent;
				}
				val.DeliveryMessage = item.Message;
				val.DeliveryShortMessage = item.Shortmessage;
				((DbContext)dbcontext).Update<U>(val);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 278);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 286);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 287);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 298);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 299);
				}
				token.ThrowIfCancellationRequested();
				IQueryable<U> queryable = ((IQueryable<U>)((DbContext)dbcontext).Set<U>()).Where((U x) => x.Guid == del.Guid);
				U val = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>(queryable, default(CancellationToken)) : null);
				if (val == null)
				{
					return ret;
				}
				val.ServerStatus = (del.DeliveryStatus ? ServerStatusForClient.Delivered : ServerStatusForClient.DeliveryError);
				val.DeliveryDate = DateTime.Now;
				val.DeliveryId = del.DeliveryId;
				val.DeliveryMessage = del.Message;
				val.DeliveryShortMessage = del.ShortMessage;
				((DbContext)dbcontext).Update<U>(val);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 314);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 322);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 323);
			}
			return ret;
		}
	}

	public virtual async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = null;
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs"))
		{
			List<Guid> result;
			try
			{
				c = await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 334);
				IOrderedEnumerable<Guid> source = from x in (from x in (IQueryable<U>)((DbContext)c).Set<U>()
						where x.ServerStatus != ServerStatusForClient.Delivered && x.ServerStatus != ServerStatusForClient.DirectDelivery && x.ServerStatus != ServerStatusForClient.Sent
						select x into p
						select p.Guid).ToList()
					orderby Guid.NewGuid()
					select x;
				result = source.ToList();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 347);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual void Dispose()
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceDetailRepository.cs", 366);
		}
	}
}
