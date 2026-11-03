using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Core;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Persistence;

public abstract class PersistenceMasterRepositoryNoGuid<T, U> : PersistenceMasterRepositoryBase<T, U>, IPersistenceMasterRepository<T>, IDisposable where T : class, IHasContext where U : class, IHasMasterGuid, new()
{
	public PersistenceMasterRepositoryNoGuid(ContextHolder dbContext)
		: base(dbContext)
	{
		base.dbContext = dbContext;
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs", 25);
				List<U> list = await EntityFrameworkQueryableExtensions.ToListAsync<U>(((IQueryable<U>)((DbContext)dbcontext).Set<U>()).Where((U x) => x.MasterGuid == masterGuid), token);
				if (list != null && list.Any())
				{
					((DbSet<_003F>)(object)((DbContext)dbcontext).Set<U>()).RemoveRange((IEnumerable<_003F>)list);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs", 32);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs", 33);
			}
			return ret;
		}
	}

	public override async Task<List<T>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs", 39);
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs"))
		{
			List<T> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<U> source = await EntityFrameworkQueryableExtensions.ToListAsync<U>(((IQueryable<U>)((DbContext)c).Set<U>()).Where((U x) => x.MasterGuid == masterGuid), token);
					result = await MapFromDatabase.MapListAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs", 51);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\PersistenceMasterRepositoryNoGuid.cs");
				}
			}
			return result;
		}
	}
}
