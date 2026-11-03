using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core.Repository.Indexes;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Cache;

public class IxCacheRepository : INpDisposable, IDisposable, IIxRepository
{
	private bool _isDisposed;

	public bool IsDisposed => _isDisposed;

	internal IIxRepository DbRepository { get; private set; }

	public WebContextHolder Context => DbRepository?.Context;

	public string ContextGuid => Context?.Guid;

	public IxCacheRepository(IIxRepository dbRepository, int CachingTimeSeconds)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				DbRepository?.Dispose();
				DbRepository = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\IxCacheRepository.cs", 36);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveListAsync(Dictionary<Guid, HashSet<int>> lst, string tranName, CancellationToken token)
	{
		return DbRepository.SaveListAsync(lst, tranName, token);
	}
}
