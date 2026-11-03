using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class CityPersistanceRepository : PersistenceRepository<CityBusiness, City>, ICityRepository, IRepository<CityBusiness>, INoGuidRepository<CityBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CityBusiness, City>, IItemAsyncMapper<City, CityBusiness>
	{
		public Task<City> MapAsync(string contextGuid, CityBusiness source, City destination, string tranName, CancellationToken token = default(CancellationToken))
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
					destination = new City();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityName = source.CityName;
				destination.StateGuid = source.StateGuid;
				destination.Weight = source.Weight;
				destination.isDivarCity = source.isDivarCity;
				destination.isSheypoorCity = source.isSheypoorCity;
				destination.isNiazKadeCity = source.isNiazKadeCity;
				destination.isNiazmandyHaCity = source.isNiazmandyHaCity;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 259);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<CityBusiness> MapAsync(string contextGuid, City source, CityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CityBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CityName = source.CityName;
				destination.StateGuid = source.StateGuid;
				destination.Weight = source.Weight;
				destination.isDivarCity = source.isDivarCity;
				destination.isSheypoorCity = source.isSheypoorCity;
				destination.isNiazKadeCity = source.isNiazKadeCity;
				destination.isNiazmandyHaCity = source.isNiazmandyHaCity;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 288);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<City>> MapListAsync(string contextGuid, IEnumerable<CityBusiness> source, List<City> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CityBusiness>> MapListAsync(string contextGuid, IEnumerable<City> source, List<CityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	internal class CityBusinessWithEmptyConstructor : CityBusiness
	{
		public CityBusinessWithEmptyConstructor()
			: base(Guid.Empty.ToString())
		{
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<City, CityBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CityBusiness, City> MapToDatabase => Mapper;

	public override IItemMapper<City, CityBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public CityPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<CityBusiness>> GetAllAsync(string search, Guid StateGuid, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 25));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs"))
		{
			IEnumerable<CityBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<string> list = search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<City> source2 = (IQueryable<City>)EntityFrameworkQueryableExtensions.Include<City, State>((IQueryable<City>)c.Cities, (Expression<Func<City, State>>)((City q) => q.State));
						if (list != null)
						{
							foreach (string word in list)
							{
								if (!string.IsNullOrEmpty(word))
								{
									source2 = source2.Where((City p) => DbFunctionsExtensions.Like(EF.Functions, p.CityName, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.State.StateName, $"%{word}%"));
								}
							}
						}
						if (StateGuid != Guid.Empty)
						{
							source2 = source2.Where((City p) => p.StateGuid == StateGuid);
						}
						if (status.HasValue)
						{
							source2 = source2.Where((City p) => p.Status == ((bool?)status).Value);
						}
						source2 = from x in source2
							orderby x.StateGuid, x.CityName
							select x;
						IQueryable<CityBusinessWithEmptyConstructor> queryable = source2.Select((City source) => new CityBusinessWithEmptyConstructor
						{
							CityName = source.CityName,
							Guid = source.Guid,
							isDivarCity = source.isDivarCity,
							isNiazKadeCity = source.isNiazKadeCity,
							isNiazmandyHaCity = source.isNiazmandyHaCity,
							isSheypoorCity = source.isSheypoorCity,
							Modified = source.Modified,
							StateGuid = source.StateGuid,
							Status = source.Status,
							Weight = source.Weight,
							StateName = source.State.StateName
						});
						result = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<CityBusinessWithEmptyConstructor>(queryable, token) : null);
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 61);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CityBusiness> GetAsync(string Name, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 73));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs"))
		{
			CityBusiness result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<CityBusinessWithEmptyConstructor> queryable = from source in ((IEnumerable<City>)EntityFrameworkQueryableExtensions.Include<City, State>((IQueryable<City>)((DbContext)c).Set<City>(), (Expression<Func<City, State>>)((City p) => p.State))).AsQueryable()
						select new CityBusinessWithEmptyConstructor
						{
							CityName = source.CityName,
							Guid = source.Guid,
							isDivarCity = source.isDivarCity,
							isNiazKadeCity = source.isNiazKadeCity,
							isNiazmandyHaCity = source.isNiazmandyHaCity,
							isSheypoorCity = source.isSheypoorCity,
							Modified = source.Modified,
							StateGuid = source.StateGuid,
							Status = source.Status,
							Weight = source.Weight,
							StateName = source.State.StateName
						};
					result = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CityBusinessWithEmptyConstructor>(queryable, (Expression<Func<CityBusinessWithEmptyConstructor, bool>>)((CityBusinessWithEmptyConstructor q) => q.CityName == Name), default(CancellationToken)) : null);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 95);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<CityBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 105));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs"))
		{
			CityBusiness result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<CityBusinessWithEmptyConstructor> queryable = from source in ((IEnumerable<City>)EntityFrameworkQueryableExtensions.Include<City, State>((IQueryable<City>)((DbContext)c).Set<City>(), (Expression<Func<City, State>>)((City p) => p.State))).AsQueryable()
						select new CityBusinessWithEmptyConstructor
						{
							CityName = source.CityName,
							Guid = source.Guid,
							isDivarCity = source.isDivarCity,
							isNiazKadeCity = source.isNiazKadeCity,
							isNiazmandyHaCity = source.isNiazmandyHaCity,
							isSheypoorCity = source.isSheypoorCity,
							Modified = source.Modified,
							StateGuid = source.StateGuid,
							Status = source.Status,
							Weight = source.Weight,
							StateName = source.State.StateName
						};
					result = await ((queryable != null) ? EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<CityBusinessWithEmptyConstructor>(queryable, (Expression<Func<CityBusinessWithEmptyConstructor, bool>>)((CityBusinessWithEmptyConstructor q) => q.Guid == guid), token) : null);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 127);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task UpdateAllCityChecked(AdvertiseType type, short tryCount, string tranName)
	{
		using (new JaegerMonitor("UpdateAllCityChecked", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs"))
		{
			Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateAllCityChecked", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 138));
			try
			{
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)adv_ModelContext).Database, "sp_City_UpdateAllCityFromSite {0}", new object[1] { (short)type });
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAllCityChecked", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 145);
				throw;
			}
		}
	}

	public async Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 153));
		using (new JaegerMonitor("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs"))
		{
			bool result;
			try
			{
				City city = ((IQueryable<City>)adv_ModelContext.Cities).FirstOrDefault((City p) => p.CityName == name && p.Guid != guid);
				result = city == null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 162);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<CityBusiness>> GetAllAsync(Guid stateGuid, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 174));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs"))
		{
			IEnumerable<CityBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<City> source2 = ((IQueryable<City>)EntityFrameworkQueryableExtensions.Include<City, State>(c.Cities.AsQueryable(), (Expression<Func<City, State>>)((City q) => q.State))).Where((City q) => q.StateGuid == stateGuid);
					if (status.HasValue)
					{
						source2 = source2.Where((City q) => (bool?)q.Status == status);
					}
					IQueryable<CityBusinessWithEmptyConstructor> queryable = source2.Select((City source) => new CityBusinessWithEmptyConstructor
					{
						CityName = source.CityName,
						Guid = source.Guid,
						isDivarCity = source.isDivarCity,
						isNiazKadeCity = source.isNiazKadeCity,
						isNiazmandyHaCity = source.isNiazmandyHaCity,
						isSheypoorCity = source.isSheypoorCity,
						Modified = source.Modified,
						StateGuid = source.StateGuid,
						Status = source.Status,
						Weight = source.Weight,
						StateName = source.State.StateName
					});
					result = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<CityBusinessWithEmptyConstructor>(queryable, default(CancellationToken)) : null);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 198);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\CityPersistanceRepository.cs", 222);
			throw;
		}
	}
}
