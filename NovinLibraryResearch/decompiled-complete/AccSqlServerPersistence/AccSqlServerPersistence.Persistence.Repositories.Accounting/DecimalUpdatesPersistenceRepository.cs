using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class DecimalUpdatesPersistenceRepository : PersistenceRepository<DecimalUpdatesBusiness, DecimalUpdates>, IDecimalUpdatesRepository, IRepository<DecimalUpdatesBusiness>, INoGuidRepository<DecimalUpdatesBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DecimalUpdatesBusiness, DecimalUpdates>, IItemAsyncMapper<DecimalUpdates, DecimalUpdatesBusiness>
	{
		public Task<DecimalUpdates> MapAsync(string contextGuid, DecimalUpdatesBusiness source, DecimalUpdates destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new DecimalUpdates();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DecimalGuid = source.DecimalGuid;
					destination.DateM = source.DateM;
					destination.Price = source.Price;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs", 91);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<DecimalUpdatesBusiness> MapAsync(string contextGuid, DecimalUpdates source, DecimalUpdatesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new DecimalUpdatesBusiness(contextGuid);
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DecimalGuid = source.DecimalGuid;
					destination.DateM = source.DateM;
					destination.Price = source.Price;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs", 118);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<DecimalUpdates>> MapListAsync(string contextGuid, IEnumerable<DecimalUpdatesBusiness> source, List<DecimalUpdates> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DecimalUpdatesBusiness>> MapListAsync(string contextGuid, IEnumerable<DecimalUpdates> source, List<DecimalUpdatesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<DecimalUpdates, DecimalUpdatesBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DecimalUpdatesBusiness, DecimalUpdates> MapToDatabase => Mapper;

	public override IItemMapper<DecimalUpdates, DecimalUpdatesBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public DecimalUpdatesPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<DecimalUpdatesBusiness>> GetAllByDecimalGuidAsync(Guid decimalGuid, bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllByDecimalGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByDecimalGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs", 26));
			IEnumerable<DecimalUpdatesBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<DecimalUpdates> queryable = ((IQueryable<DecimalUpdates>)c.DecimalUpdates).Where((DecimalUpdates p) => p.DecimalGuid == decimalGuid);
					if (status.HasValue)
					{
						queryable = queryable.Where((DecimalUpdates q) => (bool?)q.Status == status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<DecimalUpdates, DecimalUpdatesBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = (await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<DecimalUpdates>(queryable, token), null, tranName, token))?.OrderBy((DecimalUpdatesBusiness q) => q.DateM)?.ToList();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllByDecimalGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs", 39);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllByDecimalGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalUpdatesPersistenceRepository.cs", 60);
		}
	}
}
