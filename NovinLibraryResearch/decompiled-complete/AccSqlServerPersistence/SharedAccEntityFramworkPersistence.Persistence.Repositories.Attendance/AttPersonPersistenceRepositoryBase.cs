using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal abstract class AttPersonPersistenceRepositoryBase : IAttPersonRepository, IRepository<AttPersonBusiness>, INoGuidRepository<AttPersonBusiness>, IDisposable
{
	private bool _isDisposed;

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => Context?.Guid;

	public ContextHolder dbContext { get; private set; }

	public bool IsDisposed => _isDisposed;

	internal AttPersonPersistenceRepositoryBase(ContextHolder db)
	{
		dbContext = db;
	}

	public virtual Task<int> CountInDepartmentAsync(Guid departmentGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Context?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs", 33);
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public AttPersonBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<AttPersonBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<AttPersonBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<AttPersonBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<AttPersonBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(AttPersonBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<AttPersonBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
