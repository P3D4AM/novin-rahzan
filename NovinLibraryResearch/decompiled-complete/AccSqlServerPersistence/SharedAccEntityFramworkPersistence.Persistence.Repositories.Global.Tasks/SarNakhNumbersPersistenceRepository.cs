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

internal class SarNakhNumbersPersistenceRepository : PersistenceNoGuidRepository<SarNakhNumbersBusiness, SarNakhNumbers>, ISarNakhNumbersRepository, INoGuidRepository<SarNakhNumbersBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SarNakhNumbers, SarNakhNumbersBusiness>, IItemAsyncMapper<SarNakhNumbersBusiness, SarNakhNumbers>
	{
		public Task<SarNakhNumbersBusiness> MapAsync(string contextGuid, SarNakhNumbers source, SarNakhNumbersBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs"))
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
					destination.Number = source.Number;
					destination.Title = source.Title;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 149);
					throw;
				}
			}
		}

		public Task<SarNakhNumbers> MapAsync(string contextGuid, SarNakhNumbersBusiness source, SarNakhNumbers destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs"))
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
						destination = new SarNakhNumbers();
					}
					destination.SarNakhGuid = source.SarNakhGuid;
					destination.Number = source.Number;
					destination.Title = source.Title;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 173);
					throw;
				}
			}
		}

		public Task<List<SarNakhNumbersBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakhNumbers> source, List<SarNakhNumbersBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SarNakhNumbers>> MapListAsync(string contextGuid, IEnumerable<SarNakhNumbersBusiness> source, List<SarNakhNumbers> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<SarNakhNumbers, SarNakhNumbersBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SarNakhNumbersBusiness, SarNakhNumbers> MapToDatabase => Mapper;

	public SarNakhNumbersPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 38);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 47));
				IQueryable<SarNakhNumbers> queryable = ((IQueryable<SarNakhNumbers>)global_ModelContext.SarNakhNumbers).Where((SarNakhNumbers x) => x.SarNakhGuid == masterGuid);
				if (queryable != null && queryable.Any())
				{
					global_ModelContext.SarNakhNumbers.RemoveRange((IEnumerable<SarNakhNumbers>)queryable);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 54);
				ret.AddReturnedValue(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 55);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(SarNakhNumbersBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (item == null)
			{
				return ret;
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 66));
				SarNakhNumbers sarNakhNumbers = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(sarNakhNumbers, sarNakhNumbers.SarNakhGuid, sarNakhNumbers.Number), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 68);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 72);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 73);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<SarNakhNumbersBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs"))
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
					foreach (SarNakhNumbersBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 86);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 90);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 91);
			}
			return ret;
		}
	}

	public override Task<IEnumerable<SarNakhNumbersBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task GetAllAsync(SarNakhBusiness master, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 100));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					IQueryable<SarNakhNumbers> queryable = ((IQueryable<SarNakhNumbers>)c.SarNakhNumbers).Where((SarNakhNumbers x) => x.SarNakhGuid == master.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_011f;
					}
					foreach (SarNakhNumbers item in queryable)
					{
						IItemAsyncMapper<SarNakhNumbers, SarNakhNumbersBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = base.ContextGuid;
						SarNakhNumbers source = item;
						SarNakhNumbersBusiness det = await mapFromDatabase.MapAsync(contextGuid, source, await SarNakhNumbersBusiness.GetFactoryAsync(base.ContextGuid, master, token, tranName), tranName, token);
						await master.AddDetailNumbersListAsync(det);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs", 116);
					throw;
				}
				return;
				end_IL_010f:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhNumbersPersistenceRepository.cs");
				}
			}
		}
	}
}
