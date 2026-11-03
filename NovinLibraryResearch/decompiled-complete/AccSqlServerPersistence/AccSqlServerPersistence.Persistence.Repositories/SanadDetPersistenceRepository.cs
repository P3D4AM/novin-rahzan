using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class SanadDetPersistenceRepository : PersistenceRepository<Sanad_DetBusiness, Sanad_Det>, ISanad_DetRepository, IRepository<Sanad_DetBusiness>, INoGuidRepository<Sanad_DetBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Sanad_DetBusiness, Sanad_Det>, IItemAsyncMapper<Sanad_Det, Sanad_DetBusiness>
	{
		public Task<Sanad_Det> MapAsync(string contextGuid, Sanad_DetBusiness source, Sanad_Det destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs"))
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
						destination = new Sanad_Det();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MoeinGuid = source.MoeinGuid;
					destination.TafsilGuid = source.TafsilGuid;
					destination.Debit = source.Debit;
					destination.Credit = source.Credit;
					destination.DecimalGuid = source.DecimalGuid;
					destination.Description = source.RowDesc;
					destination.Qt = source.Qt;
					destination.AsType = source.AsType;
					destination.AsGuid = source.AsGuid;
					destination.Serial = source.Serial;
					destination.DateSarresid = source.DateSarresid;
					destination.Ordinal = source.Ordinal;
					destination.MetaData = Json.ToStringJson(source.MetaData);
					destination.TafsilGuid2 = source.TafsilGuid2;
					destination.TafsilGuid3 = source.TafsilGuid3;
					destination.TafsilGuid4 = source.TafsilGuid4;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 159);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public async Task<Sanad_DetBusiness> MapAsync(string contextGuid, Sanad_Det source, Sanad_DetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs"))
			{
				_ = 4;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new Sanad_DetBusiness(contextGuid, EnSystem.Mapper, source.AsGuid, source.AsType, source.Ordinal);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					await destination.SetMoeinGuidAsync(source.MoeinGuid, token, tranName);
					await destination.SetTafsilGuidAsync(source.TafsilGuid, tranName, token);
					destination.Debit = source.Debit;
					destination.Credit = source.Credit;
					destination.DecimalGuid = source.DecimalGuid;
					destination.RowDesc = source.Description;
					destination.Qt = source.Qt;
					destination.AsType = source.AsType;
					destination.AsGuid = source.AsGuid;
					destination.Serial = source.Serial;
					destination.DateSarresid = source.DateSarresid;
					destination.MetaData = source.MetaData.FromJson<SanadDetMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 189);
					await destination.SetTafsil2GuidAsync(source.TafsilGuid2, tranName, token);
					await destination.SetTafsil3GuidAsync(source.TafsilGuid3, tranName, token);
					await destination.SetTafsil4GuidAsync(source.TafsilGuid4, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 198);
					throw;
				}
				return destination;
			}
		}

		public Task<List<Sanad_Det>> MapListAsync(string contextGuid, IEnumerable<Sanad_DetBusiness> source, List<Sanad_Det> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Sanad_DetBusiness>> MapListAsync(string contextGuid, IEnumerable<Sanad_Det> source, List<Sanad_DetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Sanad_Det, Sanad_DetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Sanad_DetBusiness, Sanad_Det> MapToDatabase => Mapper;

	public override bool HasStatus { get; }

	public override bool HasModified { get; }

	public override IItemMapper<Sanad_Det, Sanad_DetBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public SanadDetPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<Sanad_DetBusiness>> GetAllAsync(Guid SanadGuid, Document SanadType, Guid? MoeinGuid, Guid? TafsilGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 34));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs"))
		{
			List<Sanad_DetBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Sanad_Det> queryable = ((IQueryable<Sanad_Det>)((DbContext)c).Set<Sanad_Det>()).Where((Sanad_Det x) => x.AsGuid == SanadGuid && x.AsType == SanadType);
					result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token))?.OrderBy((Sanad_DetBusiness q) => q.Ordinal)?.ToList() : null);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 48);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<ReturnedSaveFuncInfo> DeleteBySanadNoAsync(int SanadNo, Guid SanadGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<Sanad_DetBusiness> items, SanadBusiness master, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (items == null || !items.Any())
		{
			throw new Exception("this is user log/ items is null or empty/ sender must changed");
		}
		try
		{
			BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 1800000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 69);
			foreach (Sanad_DetBusiness item in items)
			{
				if (item == null)
				{
					throw new ArgumentException("this is user log/item of list is null!");
				}
				Sanad_Det sanad_Det = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(sanad_Det, sanad_Det.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 76);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 81);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 82);
		}
		return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 95);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveListToSanadDetAsync(int SanadGuid, Document SanadType, List<Sanad_DetBusiness> lst, Guid SanadMoeinGuid, Guid SanadTafsilGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 1800000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 109);
				List<Sanad_Det> list = ((IQueryable<Sanad_Det>)((DbContext)baseModelContext).Set<Sanad_Det>()).Where((Sanad_Det x) => x.AsGuid == asGuid && x.AsType == asType)?.ToList();
				if (list != null && list.Any())
				{
					((DbContext)baseModelContext).Set<Sanad_Det>().RemoveRange((IEnumerable<Sanad_Det>)list);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 117);
				ret.AddReturnedValue(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadDetPersistenceRepository.cs", 118);
			}
			return ret;
		}
	}
}
