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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class VisitorCityPersistanceRepository : PersistenceRepository<VisitorCityBusiness, VisitorCity>, IVisitorCityRepository, IRepository<VisitorCityBusiness>, INoGuidRepository<VisitorCityBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<VisitorCityBusiness, VisitorCity>, IItemAsyncMapper<VisitorCity, VisitorCityBusiness>
	{
		public Task<VisitorCity> MapAsync(string contextGuid, VisitorCityBusiness source, VisitorCity destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new VisitorCity();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.VisitorGuid = source.VisitorGuid;
				destination.CityGuid = source.CityGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 205);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<VisitorCityBusiness> MapAsync(string contextGuid, VisitorCity source, VisitorCityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new VisitorCityBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.VisitorGuid = source.VisitorGuid;
				destination.CityGuid = source.CityGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 229);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<VisitorCity>> MapListAsync(string contextGuid, IEnumerable<VisitorCityBusiness> source, List<VisitorCity> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<VisitorCityBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitorCity> source, List<VisitorCityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	internal class VisitorCityBusinessWithEmptyConstructor : VisitorCityBusiness
	{
		public VisitorCityBusinessWithEmptyConstructor()
			: base(Guid.Empty.ToString())
		{
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<VisitorCity, VisitorCityBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<VisitorCityBusiness, VisitorCity> MapToDatabase => Mapper;

	public override IItemMapper<VisitorCity, VisitorCityBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public VisitorCityPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<VisitorCityBusiness>> GetAllAsync(Guid visitorGuid, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 28));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs"))
		{
			IEnumerable<VisitorCityBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<VisitorCity> queryable = ((IEnumerable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, Visitor>((IQueryable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, City>((IQueryable<VisitorCity>)((DbContext)c).Set<VisitorCity>(), (Expression<Func<VisitorCity, City>>)((VisitorCity p) => p.City)), (Expression<Func<VisitorCity, Visitor>>)((VisitorCity q) => q.Visitor))).AsQueryable();
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<VisitorCity> queryable2 = queryable.Where((VisitorCity q) => q.VisitorGuid == visitorGuid);
						if (queryable2 == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<VisitorCityBusinessWithEmptyConstructor> queryable3 = queryable2.Select((VisitorCity source) => new VisitorCityBusinessWithEmptyConstructor
							{
								CityGuid = source.CityGuid,
								VisitorGuid = source.VisitorGuid,
								Guid = source.Guid,
								Modified = source.Modified,
								Status = source.Status,
								VisitorName = source.Visitor.FullName
							});
							obj = ((queryable3 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<VisitorCityBusinessWithEmptyConstructor>(queryable3, default(CancellationToken)) : null);
						}
					}
					result = await (Task<List<VisitorCityBusinessWithEmptyConstructor>>)obj;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 46);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByVisitorGuidAsync(Guid visitorGuid, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		_ = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 59));
		using (new JaegerMonitor("RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				await DeleteRangeAsync((await GetAllAsync(visitorGuid, tranName))?.Select((VisitorCityBusiness q) => q.Guid).ToList(), tryCount, default(CancellationToken), tranName);
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 69);
				throw;
			}
		}
	}

	public async Task<IEnumerable<VisitorCityBusiness>> GetAllCityAsync(Guid cityGuid, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllCityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 76));
		using (new JaegerMonitor("GetAllCityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs"))
		{
			IEnumerable<VisitorCityBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<VisitorCity> queryable = ((IEnumerable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, Visitor>((IQueryable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, City>((IQueryable<VisitorCity>)((DbContext)c).Set<VisitorCity>(), (Expression<Func<VisitorCity, City>>)((VisitorCity p) => p.City)), (Expression<Func<VisitorCity, Visitor>>)((VisitorCity q) => q.Visitor))).AsQueryable();
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<VisitorCity> queryable2 = queryable.Where((VisitorCity q) => q.CityGuid == cityGuid);
						if (queryable2 == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<VisitorCityBusinessWithEmptyConstructor> queryable3 = queryable2.Select((VisitorCity source) => new VisitorCityBusinessWithEmptyConstructor
							{
								CityGuid = source.CityGuid,
								VisitorGuid = source.VisitorGuid,
								Guid = source.Guid,
								Modified = source.Modified,
								Status = source.Status,
								VisitorName = source.Visitor.FullName
							});
							obj = ((queryable3 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<VisitorCityBusinessWithEmptyConstructor>(queryable3, default(CancellationToken)) : null);
						}
					}
					result = await (Task<List<VisitorCityBusinessWithEmptyConstructor>>)obj;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllCityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 94);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllCityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<IEnumerable<VisitorCityBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 105));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs"))
		{
			IEnumerable<VisitorCityBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<VisitorCity> queryable = ((IEnumerable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, Visitor>((IQueryable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, City>((IQueryable<VisitorCity>)((DbContext)c).Set<VisitorCity>(), (Expression<Func<VisitorCity, City>>)((VisitorCity p) => p.City)), (Expression<Func<VisitorCity, Visitor>>)((VisitorCity q) => q.Visitor))).AsQueryable();
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<VisitorCity> queryable2 = queryable.Where((VisitorCity q) => status == null || (bool?)q.Status == status);
						if (queryable2 == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<VisitorCityBusinessWithEmptyConstructor> queryable3 = queryable2.Select((VisitorCity source) => new VisitorCityBusinessWithEmptyConstructor
							{
								CityGuid = source.CityGuid,
								VisitorGuid = source.VisitorGuid,
								Guid = source.Guid,
								Modified = source.Modified,
								Status = source.Status,
								VisitorName = source.Visitor.FullName
							});
							obj = ((queryable3 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<VisitorCityBusinessWithEmptyConstructor>(queryable3, token) : null);
						}
					}
					result = await (Task<List<VisitorCityBusinessWithEmptyConstructor>>)obj;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 123);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<VisitorCityBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 134));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs"))
		{
			VisitorCityBusiness result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<VisitorCity> queryable = ((IEnumerable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, Visitor>((IQueryable<VisitorCity>)EntityFrameworkQueryableExtensions.Include<VisitorCity, City>((IQueryable<VisitorCity>)((DbContext)c).Set<VisitorCity>(), (Expression<Func<VisitorCity, City>>)((VisitorCity p) => p.City)), (Expression<Func<VisitorCity, Visitor>>)((VisitorCity q) => q.Visitor))).AsQueryable();
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<VisitorCityBusinessWithEmptyConstructor> queryable2 = queryable.Select((VisitorCity source) => new VisitorCityBusinessWithEmptyConstructor
						{
							CityGuid = source.CityGuid,
							VisitorGuid = source.VisitorGuid,
							Guid = source.Guid,
							Modified = source.Modified,
							Status = source.Status,
							VisitorName = source.Visitor.FullName
						});
						obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<VisitorCityBusinessWithEmptyConstructor>(queryable2, (Expression<Func<VisitorCityBusinessWithEmptyConstructor, bool>>)((VisitorCityBusinessWithEmptyConstructor q) => q.Guid == guid), token) : null);
					}
					result = await (Task<VisitorCityBusinessWithEmptyConstructor>)obj;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 151);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorCityPersistanceRepository.cs", 173);
			throw;
		}
	}
}
