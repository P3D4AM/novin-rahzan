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

internal class DivarCityPersistenceRepository : PersistenceRepository<DivarCityBusiness, DivarCity>, IDivarCityRepository, IRepository<DivarCityBusiness>, INoGuidRepository<DivarCityBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DivarCityBusiness, DivarCity>, IItemAsyncMapper<DivarCity, DivarCityBusiness>
	{
		public Task<DivarCity> MapAsync(string contextGuid, DivarCityBusiness source, DivarCity destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DivarCity();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityName = source.CityName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DivarCityPersistenceRepository.cs", 113);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<DivarCityBusiness> MapAsync(string contextGuid, DivarCity source, DivarCityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DivarCityBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityName = source.CityName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DivarCityPersistenceRepository.cs", 136);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<DivarCity>> MapListAsync(string contextGuid, IEnumerable<DivarCityBusiness> source, List<DivarCity> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DivarCityBusiness>> MapListAsync(string contextGuid, IEnumerable<DivarCity> source, List<DivarCityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<DivarCity, DivarCityBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DivarCityBusiness, DivarCity> MapToDatabase => Mapper;

	public override IItemMapper<DivarCity, DivarCityBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public DivarCityPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<DivarCityBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DivarCityPersistenceRepository.cs", 27));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DivarCityPersistenceRepository.cs"))
		{
			List<DivarCityBusiness> result;
			try
			{
				_ = 1;
				try
				{
					result = await MapFromDatabase.MapListAsync(ContextGuid, ((IEnumerable<DivarCity>)c.DivarCity)?.ToList());
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DivarCityPersistenceRepository.cs", 57);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DivarCityPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\DivarCityPersistenceRepository.cs", 82);
			throw;
		}
	}
}
