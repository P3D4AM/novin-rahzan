using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence;

public abstract class PersistenceRepository<T, U> : IRepository<T>, INoGuidRepository<T>, IDisposable where T : class, IHasJustGuid, IHasContext where U : class, IHasJustGuid, new()
{
	protected ContextHolder dbContext;

	public bool IsDisposed { get; private set; }

	public abstract IItemMapper<U, T> MapFromDatabaseNoneAsync { get; }

	public abstract IItemAsyncMapper<U, T> MapFromDatabase { get; }

	public abstract IItemAsyncMapper<T, U> MapToDatabase { get; }

	public abstract bool HasStatus { get; }

	public abstract bool HasModified { get; }

	public virtual WebContextHolder Context => dbContext.Context;

	public virtual string ContextGuid => Context?.Guid;

	internal PersistenceRepository(ContextHolder db)
	{
		dbContext = db;
	}

	public async Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 25);
		int result;
		try
		{
			result = ((IQueryable<U>)((DbContext)baseModelContext).Set<U>()).Count();
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
			WebErrorLog.Instance.StartLog(ex2, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 34);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await baseModelContext.DisposeAsync("DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs");
			}
		}
		return result;
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 59);
				U val = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				if (item.Guid == Guid.Empty)
				{
					throw new Exception($"Guid is empty. item type:{item.GetType()}");
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(val, val.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 63);
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 69);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 70);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		if (items == null || !items.Any())
		{
			throw new Exception("this is user log/ items is null or empty/ sender must changed");
		}
		try
		{
			line = 1;
			BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 85);
			line = 2;
			foreach (T item in items)
			{
				if (item == null)
				{
					WebErrorLog.Instance.StartLog("this is user log/item of list is null!", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 91);
					return ret;
				}
				line = 3;
				U val = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				line = 4;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(val, val.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 97);
				line = 5;
			}
			line = 6;
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 105);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 106);
		}
		return ret;
	}

	public virtual Task<IEnumerable<T>> GetAllAsync(CancellationToken token, string tranName)
	{
		return GetAllAsync(true, token, tranName);
	}

	public virtual Task<T> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 115);
		IEnumerable<T> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				DbSet<U> obj = ((DbContext)c).Set<U>();
				List<U> source = await ((obj != null) ? EntityFrameworkQueryableExtensions.ToListAsync<U>((IQueryable<U>)obj, token) : null);
				result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 126);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs");
			}
		}
		return result;
	}

	public virtual async Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 143);
				List<U> source = ((baseModelContext == null) ? null : ((IEnumerable<U>)((DbContext)baseModelContext).Set<U>())?.ToList());
				IEnumerable<Guid> source2 = source.Select((U x) => x.Guid);
				IEnumerable<Guid> removeList = source2.Where((Guid p) => !items.Contains(p));
				IEnumerable<Guid> enumerable = removeList;
				if (enumerable != null && enumerable.Count() > 0)
				{
					source = source.Where((U p) => removeList.Contains(p.Guid)).ToList();
					if (source != null)
					{
						((DbSet<_003F>)(object)((DbContext)baseModelContext).Set<U>()).RemoveRange((IEnumerable<_003F>)source);
					}
				}
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
				WebErrorLog.Instance.StartLog(ex3, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 159);
				ret.AddReturnedValue(ex3, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 160);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!(items?.Any() ?? false))
				{
					return ret;
				}
				BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 173);
				List<U> list = ((IQueryable<U>)((DbContext)baseModelContext).Set<U>()).Where((U p) => items.Contains(p.Guid)).ToList();
				if (baseModelContext != null)
				{
					((DbSet<_003F>)(object)((DbContext)baseModelContext).Set<U>())?.RemoveRange((IEnumerable<_003F>)list);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 181);
				ret.AddReturnedValue(ex2, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 182);
			}
			return ret;
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs"))
		{
			_ = 1;
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, $"PersistenceRepository.GetAsync(by Guid)//GenericType(T):{typeof(T)}", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 194);
				U val = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<U>((IQueryable<U>)((DbContext)dbcontext).Set<U>(), (Expression<Func<U, bool>>)((U p) => p.Guid == guid), default(CancellationToken));
				if (val != null)
				{
					((EntityEntry)((DbContext)dbcontext).Entry<U>(val)).State = (EntityState)2;
					((DbSet<_003F>)(object)((DbContext)dbcontext).Set<U>()).Remove(val);
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 205);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 206);
			}
			return ret;
		}
	}

	public virtual async Task<T> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 213);
		T result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				U val = await ((DbSet<_003F>)(object)((DbContext)c).Set<U>()).FindAsync(new object[1] { guid });
				result = ((val != null) ? (await MapFromDatabase.MapAsync(ContextGuid, val, null, tranName, token)) : null);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 225);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs");
			}
		}
		return result;
	}

	public virtual T Get(Guid guid, CancellationToken token, string tranName)
	{
		int num = 0;
		BaseModelContext context = dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 237);
		try
		{
			U source = ((DbSet<_003F>)(object)((DbContext)context).Set<U>()).Find(new object[1] { guid });
			num = 2;
			return MapFromDatabaseNoneAsync.Map(ContextGuid, source, null, tranName, token);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, $"The Error Line Is :{num}", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 248);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				context.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs");
			}
		}
	}

	public virtual void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceRepository.cs", 266);
		}
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
