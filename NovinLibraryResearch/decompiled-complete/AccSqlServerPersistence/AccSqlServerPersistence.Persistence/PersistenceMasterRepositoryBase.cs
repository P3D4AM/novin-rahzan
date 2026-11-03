using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence;

public abstract class PersistenceMasterRepositoryBase<T, U> : IPersistenceMasterRepository<T>, IDisposable where T : class, IHasContext where U : class
{
	protected ContextHolder dbContext;

	private bool _isDisposed;

	public bool IsDisposed => _isDisposed;

	public abstract IItemAsyncMapper<U, T> MapFromDatabase { get; }

	public abstract IItemAsyncMapper<T, U> MapToDatabase { get; }

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => Context.Guid;

	public PersistenceMasterRepositoryBase(ContextHolder dbContext)
	{
		this.dbContext = dbContext;
	}

	public abstract Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<T> items, Guid masterGuid, short trycount, CancellationToken token, string tranName);

	public abstract Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short trycount, CancellationToken token, string tranName);

	public abstract Task<List<T>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName);

	public virtual void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				dbContext = null;
				dbContext?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryBase.cs", 42);
		}
	}
}
