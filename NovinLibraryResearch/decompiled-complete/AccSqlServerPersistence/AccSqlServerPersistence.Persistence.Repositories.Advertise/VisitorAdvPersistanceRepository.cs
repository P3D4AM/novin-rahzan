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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class VisitorAdvPersistanceRepository : PersistenceRepository<VisitorAdvBusiness, VisitorAdv>, IVisitorAdvRepository, IRepository<VisitorAdvBusiness>, INoGuidRepository<VisitorAdvBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<VisitorAdvBusiness, VisitorAdv>, IItemAsyncMapper<VisitorAdv, VisitorAdvBusiness>
	{
		public Task<VisitorAdv> MapAsync(string contextGuid, VisitorAdvBusiness source, VisitorAdv destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs"))
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
						destination = new VisitorAdv();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.VisitorGuid = source.VisitorGuid;
					destination.AdvGuid = source.AdvGuid;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs", 108);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<VisitorAdvBusiness> MapAsync(string contextGuid, VisitorAdv source, VisitorAdvBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs"))
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
						destination = new VisitorAdvBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.VisitorGuid = source.VisitorGuid;
					destination.AdvGuid = source.AdvGuid;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs", 133);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<VisitorAdv>> MapListAsync(string contextGuid, IEnumerable<VisitorAdvBusiness> source, List<VisitorAdv> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<VisitorAdvBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitorAdv> source, List<VisitorAdvBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<VisitorAdv, VisitorAdvBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<VisitorAdvBusiness, VisitorAdv> MapToDatabase => Mapper;

	public override IItemMapper<VisitorAdv, VisitorAdvBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal VisitorAdvPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByVisitorGuidAsync(Guid visitorGuid, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs"))
		{
			_ = 1;
			try
			{
				await DeleteRangeAsync((await GetAllAsync(visitorGuid, tranName)).Select((VisitorAdvBusiness q) => q.Guid), tryCount, default(CancellationToken), tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs", 40);
				ret.AddReturnedValue(ex, "", "RemoveByVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs", 41);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<VisitorAdvBusiness>> GetAllAsync(Guid visitorGuid, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs", 48));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs"))
		{
			IEnumerable<VisitorAdvBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<VisitorAdv> source = ((IQueryable<VisitorAdv>)c.VisitorAdv)?.Where((VisitorAdv q) => q.VisitorGuid == visitorGuid).ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs", 57);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorAdvPersistanceRepository.cs", 77);
		}
	}
}
