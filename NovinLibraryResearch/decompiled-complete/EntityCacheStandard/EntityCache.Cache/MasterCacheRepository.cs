using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Cache;

public class MasterCacheRepository<T> : IDisposable, IPersistenceMasterRepository<T> where T : class, IHasContext
{
	private IPersistenceMasterRepository<T> Dbrepository;

	public bool IsDisposed { get; private set; }

	public MasterCacheRepository(IPersistenceMasterRepository<T> dbRepository, int CachingTime)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		Dbrepository = dbRepository;
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<T> items, Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return Dbrepository.SaveAsync(items, MasterGuid, tryCount, token, tranName);
	}

	public virtual Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return Dbrepository.RemoveAsync(MasterGuid, trycount, token, tranName);
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Dbrepository?.Dispose();
				Dbrepository = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\MasterCacheRepository.cs", 49);
		}
	}

	public Task<List<T>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		return Dbrepository.GetAllAsync(MasterGuid, token, tranName);
	}
}
