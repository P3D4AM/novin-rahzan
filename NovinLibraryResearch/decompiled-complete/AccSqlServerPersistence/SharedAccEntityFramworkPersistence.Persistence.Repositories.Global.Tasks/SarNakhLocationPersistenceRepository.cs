using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global.Tasks;
using EntityCache.Core;
using EntityCache.Core.Repository.Global.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.Tasks;

internal class SarNakhLocationPersistenceRepository : PersistenceNoGuidRepository<SarNakhLocationBusiness, SarNakhLocation>, ISarNakhLocationRepository, INoGuidRepository<SarNakhLocationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SarNakhLocationBusiness, SarNakhLocation>, IItemAsyncMapper<SarNakhLocation, SarNakhLocationBusiness>
	{
		public Task<SarNakhLocation> MapAsync(string contextGuid, SarNakhLocationBusiness source, SarNakhLocation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs"))
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
						destination = new SarNakhLocation();
					}
					destination.SarNakhGuid = source.SarNakhGuid;
					destination.Email = source.Email;
					destination.WebSite = source.WebSite;
					destination.Address = source.Address;
					destination.State = source.State;
					destination.City = source.City;
					destination.Ordinal = source.Ordinal;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 152);
					throw;
				}
			}
		}

		public Task<SarNakhLocationBusiness> MapAsync(string contextGuid, SarNakhLocation source, SarNakhLocationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs"))
			{
				if (destination == null)
				{
					new ArgumentNullException("Please Pass New Destination");
				}
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					destination.ContextGuid = contextGuid;
					destination.SarNakhGuid = source.SarNakhGuid;
					destination.Email = source.Email;
					destination.WebSite = source.WebSite;
					destination.Address = source.Address;
					destination.State = source.State;
					destination.City = source.City;
					destination.Ordinal = source.Ordinal;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 181);
					throw;
				}
			}
		}

		public Task<List<SarNakhLocation>> MapListAsync(string contextGuid, IEnumerable<SarNakhLocationBusiness> source, List<SarNakhLocation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SarNakhLocationBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakhLocation> source, List<SarNakhLocationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<SarNakhLocation, SarNakhLocationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SarNakhLocationBusiness, SarNakhLocation> MapToDatabase => Mapper;

	public SarNakhLocationPersistenceRepository(ContextHolder db)
		: base(db)
	{
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 38);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(SarNakhLocationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (item == null)
			{
				return ret;
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 47));
				SarNakhLocation sarNakhLocation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(sarNakhLocation, sarNakhLocation.SarNakhGuid, sarNakhLocation.Ordinal), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 49);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 53);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 54);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<SarNakhLocationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (items == null)
			{
				return ret;
			}
			try
			{
				if (items != null && items.Any())
				{
					foreach (SarNakhLocationBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 67);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 71);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 72);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 83));
				IQueryable<SarNakhLocation> queryable = ((IQueryable<SarNakhLocation>)global_ModelContext.SarNakhLocations).Where((SarNakhLocation x) => x.SarNakhGuid == masterGuid);
				if (queryable != null && queryable.Any())
				{
					global_ModelContext.SarNakhLocations.RemoveRange((IEnumerable<SarNakhLocation>)queryable);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 90);
				ret.AddReturnedValue(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 91);
			}
			return ret;
		}
	}

	public override Task<IEnumerable<SarNakhLocationBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task GetAllAsync(SarNakhBusiness master, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 100));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					IQueryable<SarNakhLocation> queryable = ((IQueryable<SarNakhLocation>)c.SarNakhLocations).Where((SarNakhLocation x) => x.SarNakhGuid == master.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_011f;
					}
					foreach (SarNakhLocation item in queryable)
					{
						IItemAsyncMapper<SarNakhLocation, SarNakhLocationBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = base.ContextGuid;
						SarNakhLocation source = item;
						SarNakhLocationBusiness det = await mapFromDatabase.MapAsync(contextGuid, source, await SarNakhLocationBusiness.GetFactoryAsync(base.ContextGuid, master, token), tranName, token);
						await master.AddDetailLocationListAsync(det);
					}
					goto end_IL_010f;
					end_IL_011f:;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs", 116);
					throw;
				}
				return;
				end_IL_010f:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhLocationPersistenceRepository.cs");
				}
			}
		}
	}
}
