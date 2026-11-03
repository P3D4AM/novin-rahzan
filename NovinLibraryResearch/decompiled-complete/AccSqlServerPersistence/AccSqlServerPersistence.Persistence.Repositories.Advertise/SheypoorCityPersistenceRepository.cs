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

internal class SheypoorCityPersistenceRepository : PersistenceRepository<SheypoorCityBusiness, SheypoorCity>, ISheypoorCityRepository, IRepository<SheypoorCityBusiness>, INoGuidRepository<SheypoorCityBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SheypoorCityBusiness, SheypoorCity>, IItemAsyncMapper<SheypoorCity, SheypoorCityBusiness>
	{
		public Task<SheypoorCity> MapAsync(string contextGuid, SheypoorCityBusiness source, SheypoorCity destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SheypoorCity();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityName = source.CityName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SheypoorCityPersistenceRepository.cs", 111);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<SheypoorCityBusiness> MapAsync(string contextGuid, SheypoorCity source, SheypoorCityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SheypoorCityBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityName = source.CityName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SheypoorCityPersistenceRepository.cs", 134);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<SheypoorCity>> MapListAsync(string contextGuid, IEnumerable<SheypoorCityBusiness> source, List<SheypoorCity> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SheypoorCityBusiness>> MapListAsync(string contextGuid, IEnumerable<SheypoorCity> source, List<SheypoorCityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<SheypoorCity, SheypoorCityBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SheypoorCityBusiness, SheypoorCity> MapToDatabase => Mapper;

	public override IItemMapper<SheypoorCity, SheypoorCityBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public SheypoorCityPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<SheypoorCityBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SheypoorCityPersistenceRepository.cs", 26));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SheypoorCityPersistenceRepository.cs"))
		{
			List<SheypoorCityBusiness> result;
			try
			{
				_ = 1;
				try
				{
					result = await MapFromDatabase.MapListAsync(ContextGuid, ((IEnumerable<SheypoorCity>)c.SheypoorCity)?.ToList());
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SheypoorCityPersistenceRepository.cs", 56);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SheypoorCityPersistenceRepository.cs");
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
				dbContext = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SheypoorCityPersistenceRepository.cs", 80);
			throw;
		}
	}
}
