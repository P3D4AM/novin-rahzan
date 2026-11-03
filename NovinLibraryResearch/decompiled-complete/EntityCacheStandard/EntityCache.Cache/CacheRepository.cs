using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Cache;

public abstract class CacheRepository<T> : CacheRepositoryNoGuiId<T, Guid>, IRepository<T>, INoGuidRepository<T>, IDisposable where T : class, IHasJustGuid, IHasContext, IHasStatus
{
	private bool _isCached;

	private bool _isDisposed;

	protected bool isCached
	{
		get
		{
			if (_isCached)
			{
				return Count() > 0;
			}
			return false;
		}
		set
		{
			_isCached = value;
		}
	}

	public bool CachingEnable { get; private set; }

	protected new IRepository<T> DbRepository => (IRepository<T>)base.DbRepository;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	protected CacheRepository(IRepository<T> dbRepository, int CachingTime, bool cachingEnable = true)
		: base((INoGuidRepository<T>)dbRepository, CachingTime)
	{
		CachingEnable = cachingEnable;
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (TimingCache != null && !CachingEnable)
				{
					TimingCache.OnCollect -= TimingCache_OnCollect;
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 44);
		}
	}

	public virtual async Task<T> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		try
		{
			line = 1;
			if (guid == Guid.Empty)
			{
				return null;
			}
			line = 2;
			T ret = null;
			if (CachingEnable)
			{
				TimingCache.TryGetValue(guid, out ret);
			}
			line = 3;
			if (ret == null)
			{
				line = 4;
				ret = await DbRepository.GetAsync(guid, token, tranName);
				line = 5;
				if (ret != null && CachingEnable)
				{
					await TimingCache.UpdateTimeAsync(ret.Guid, ret, tranName);
				}
				line = 7;
			}
			line = 8;
			return ret;
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
			WebErrorLog.Instance.StartLog(ex3, $"Line:{line}//TimingCache is null:{TimingCache == null}//isDisposed:{IsDisposed}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 77);
			throw;
		}
	}

	public virtual Task<T> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return DbRepository.GetFromDatabaseAsync(guid, token, tranName);
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			try
			{
				await LoadCacheAsync(token, tranName);
				List<T> list = null;
				if (CachingEnable)
				{
					list = TimingCache.GetAll();
				}
				if (status.HasValue)
				{
					return list?.Where((T x) => x.Status == status);
				}
				return list;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 102);
				throw;
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			_ = 1;
			try
			{
				line = 1;
				ReturnedSaveFuncInfo ret = await DbRepository.SaveAsync(item, tryCount, token, tranName);
				line = 2;
				if (ret.HasError)
				{
					return ret;
				}
				line = 3;
				if (CachingEnable)
				{
					await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
				}
				line = 4;
				return ret;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"IsDisposed:{IsDisposed}//Line:{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 128);
				return new ReturnedSaveFuncInfo(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 129);
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		if (!(items ?? new List<T>()).Any())
		{
			return new ReturnedSaveFuncInfo();
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				line = 1;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await base.SaveRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 143);
				line = 2;
				if (ret.HasError)
				{
					return ret;
				}
				line = 3;
				if (CachingEnable)
				{
					foreach (T item in items)
					{
						if (item != null)
						{
							await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
						}
					}
				}
				line = 4;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"line:{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 156);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 157);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			try
			{
				HashSet<Guid> hashSet = new HashSet<Guid>();
				foreach (Guid item2 in items)
				{
					hashSet.Add(item2);
				}
				if (CachingEnable)
				{
					foreach (T item3 in TimingCache.GetAll())
					{
						if (!hashSet.Contains(item3.Guid))
						{
							TimingCache.TryRemove(item3.Guid, out var _);
						}
					}
				}
				return await DbRepository.KeepRangeAsync(items, tryCount, token, tranName);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 182);
				return new ReturnedSaveFuncInfo(ex2, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 183);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			try
			{
				HashSet<Guid> hashSet = new HashSet<Guid>();
				foreach (Guid item2 in items)
				{
					hashSet.Add(item2);
				}
				if (CachingEnable)
				{
					foreach (T item3 in TimingCache.GetAll())
					{
						if (hashSet.Contains(item3.Guid))
						{
							TimingCache.TryRemove(item3.Guid, out var _);
						}
					}
				}
				return await DbRepository.DeleteRangeAsync(items, tryCount, token, tranName);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 206);
				return new ReturnedSaveFuncInfo(ex2, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 207);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			try
			{
				if (CachingEnable)
				{
					TimingCache.TryRemove(guid, out var _);
				}
				return await DbRepository.RemoveAsync(guid, tryCount, token, tranName);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 224);
				return new ReturnedSaveFuncInfo(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 225);
			}
		}
	}

	private Task TimingCache_OnCollect(object arg1, List<Guid> arg2)
	{
		if (IsDisposed)
		{
			return Task.CompletedTask;
		}
		using (new JaegerMonitor("TimingCache_OnCollect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			try
			{
				TimingCache.OnCollect -= TimingCache_OnCollect;
				_isCached = false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TimingCache_OnCollect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 238);
			}
			return Task.CompletedTask;
		}
	}

	protected async Task LoadCacheAsync(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor($"LoadCache___type:{typeof(T)}_cached:{isCached}", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs"))
		{
			int line = 0;
			try
			{
				if (!CachingEnable || isCached)
				{
					return;
				}
				line = 1;
				await Locks.DbReadSemaphore.WaitAsync();
				line = 2;
				try
				{
					if (isCached)
					{
						return;
					}
					line = 3;
					IEnumerable<T> enumerable = await DbRepository.GetAllAsync(null, token, tranName);
					line = 4;
					TimingCache.OnCollect += TimingCache_OnCollect;
					line = 5;
					if (enumerable != null && enumerable.Any())
					{
						line = 6;
						foreach (T item in enumerable)
						{
							line = 7;
							await TimingCache.UpdateTimeAsync(item.Guid, item, tranName);
						}
					}
					isCached = true;
				}
				finally
				{
					Locks.DbReadSemaphore.Release();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "LoadCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 278);
				throw;
			}
		}
	}

	public virtual T Get(Guid guid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int num = 0;
		try
		{
			num = 1;
			if (guid == Guid.Empty)
			{
				return null;
			}
			num = 2;
			T ret = null;
			if (CachingEnable)
			{
				TimingCache.TryGetValue(guid, out ret);
			}
			num = 3;
			if (ret == null)
			{
				num = 4;
				ret = DbRepository.Get(guid, token, tranName);
				num = 5;
				if (ret != null && CachingEnable)
				{
					TimingCache.UpdateTimeAsync(ret.Guid, ret, tranName);
				}
				num = 7;
			}
			num = 8;
			return ret;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{num}//TimingCache is null:{TimingCache == null}//isDisposed:{IsDisposed}", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\CacheRepository.cs", 311);
			throw;
		}
	}
}
