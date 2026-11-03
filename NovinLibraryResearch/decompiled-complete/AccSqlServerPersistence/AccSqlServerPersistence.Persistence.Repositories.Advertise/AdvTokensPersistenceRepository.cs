using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class AdvTokensPersistenceRepository : PersistenceRepository<AdvTokensBusiness, AdvTokens>, IAdvTokensRepository, IRepository<AdvTokensBusiness>, INoGuidRepository<AdvTokensBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AdvTokensBusiness, AdvTokens>, IItemAsyncMapper<AdvTokens, AdvTokensBusiness>
	{
		public Task<AdvTokens> MapAsync(string contextGuid, AdvTokensBusiness source, AdvTokens destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvTokens();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Number = source.Number;
				destination.Type = source.Type;
				destination.Token = source.Token;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 159);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AdvTokensBusiness> MapAsync(string contextGuid, AdvTokens source, AdvTokensBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvTokensBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Number = source.Number;
				destination.Type = source.Type;
				destination.Token = source.Token;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 184);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AdvTokens>> MapListAsync(string contextGuid, IEnumerable<AdvTokensBusiness> source, List<AdvTokens> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AdvTokensBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvTokens> source, List<AdvTokensBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<AdvTokens, AdvTokensBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AdvTokensBusiness, AdvTokens> MapToDatabase => Mapper;

	public override IItemMapper<AdvTokens, AdvTokensBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal AdvTokensPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<AdvTokensBusiness> GetBySimCardAsync(long simCardNumber, AdvertiseType type, string tranName)
	{
		using (new JaegerMonitor("GetBySimCardAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs"))
		{
			Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetBySimCardAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 23));
			AdvTokensBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					AdvTokens token = new AdvTokens();
					IEnumerable<AdvTokensBusiness> source = await GetAllAsync(simCardNumber, type, true, tranName);
					if (source.Count() == 1)
					{
						token = ((IQueryable<AdvTokens>)c.AdvTokens).FirstOrDefault((AdvTokens q) => q.Number == simCardNumber && q.Type == type);
					}
					else if (source.Count() > 1)
					{
						token = ((IQueryable<AdvTokens>)c.AdvTokens).OrderByDescending((AdvTokens q) => q.Modified).First((AdvTokens q) => q.Number == simCardNumber && q.Type == type);
					}
					result = await MapFromDatabase.MapAsync(ContextGuid, token);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetBySimCardAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 38);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetBySimCardAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(long simcardNumber, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 49));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs"))
		{
			IEnumerable<AdvTokensBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<AdvTokens> queryable = ((IQueryable<AdvTokens>)c.AdvTokens).Where((AdvTokens q) => q.Number == simcardNumber);
					if (status.HasValue)
					{
						queryable = queryable?.Where((AdvTokens q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList());
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 59);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(long simcardNumber, AdvertiseType type, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 70));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs"))
		{
			IEnumerable<AdvTokensBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<AdvTokens> queryable = ((IQueryable<AdvTokens>)c.AdvTokens).Where((AdvTokens q) => q.Number == simcardNumber && q.Type == type);
					if (status.HasValue)
					{
						queryable = queryable?.Where((AdvTokens q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList());
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 81);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvTokensBusiness>> GetAllAsync(AdvertiseType type, bool? status, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs"))
		{
			Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 94));
			IEnumerable<AdvTokensBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<AdvTokens> queryable = ((IQueryable<AdvTokens>)c.AdvTokens).Where((AdvTokens q) => q.Type == type);
					if (status.HasValue)
					{
						queryable = queryable?.Where((AdvTokens q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList());
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 103);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvTokensPersistenceRepository.cs", 126);
			throw;
		}
	}
}
