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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository;

internal abstract class GardeshTafsil_TablePersistanceRepositoryBase : PersistenceRepository<GardeshTafsilBusiness, GardeshTafsil>, IGardeshTafsilRepository, IRepository<GardeshTafsilBusiness>, INoGuidRepository<GardeshTafsilBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<GardeshTafsilBusiness, GardeshTafsil>, IItemAsyncMapper<GardeshTafsil, GardeshTafsilBusiness>
	{
		public Task<GardeshTafsil> MapAsync(string contextGuid, GardeshTafsilBusiness source, GardeshTafsil destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new GardeshTafsil();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Date = source.Date;
				destination.DepotGuid = source.DepotGuid;
				destination.Description = source.Description;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.EnAsType = source.EnAsType;
				destination.AsTypeEnum = source.AsTypeEnum;
				destination.Debit = source.Debit;
				destination.Credit = source.Credit;
				destination.SanadNO = source.SanadNO;
				destination.SanadSt = source.SanadSt;
				destination.Serial = source.Serial;
				destination.Price = source.Price;
				destination.Discount = source.Discount;
				destination.DiscountPercent = source.DiscountPercent;
				destination.Id2 = source.Id2;
				destination.QT = source.QT;
				destination.QT2 = source.QT2;
				destination.Bazaryab = source.Bazaryab;
				destination.VGuid = source.VGuid;
				destination.PrdGuid = source.GetPrdGuid();
				destination.SanadDesc = source.SanadDesc;
				destination.DateMoaser = source.DateMoaser;
				destination.Rem = source.Rem;
				destination.RowIndex = source.RowIndex;
				destination.DecimalGuid = source.DecimalGuid;
				destination.RowDesc = source.RowDesc;
				destination.TafsilGuid = source.TafsilGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.KolGuid = source.KolGuid;
				destination.UGuid = source.UGuid;
				destination.DecimalQt = source.DecimalQt;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 239);
				throw;
			}
			return Task.FromResult(destination);
		}

		public async Task<GardeshTafsilBusiness> MapAsync(string contextGuid, GardeshTafsil source, GardeshTafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new GardeshTafsilBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Date = source.Date;
					destination.DepotGuid = source.DepotGuid;
					destination.Description = source.Description;
					destination.AsGuid = source.AsGuid;
					destination.AsType = source.AsType;
					destination.EnAsType = source.EnAsType;
					destination.AsTypeEnum = source.AsTypeEnum;
					destination.Debit = source.Debit;
					destination.Credit = source.Credit;
					destination.SanadNO = source.SanadNO;
					destination.SanadSt = source.SanadSt;
					destination.Serial = source.Serial;
					destination.Price = source.Price;
					destination.Discount = source.Discount;
					destination.Id2 = source.Id2;
					destination.QT = source.QT;
					destination.QT2 = source.QT2;
					destination.Bazaryab = source.Bazaryab;
					destination.VGuid = source.VGuid;
					await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper, setProduct: true);
					destination.SanadDesc = source.SanadDesc;
					destination.DateMoaser = source.DateMoaser;
					destination.Rem = source.Rem;
					destination.RowIndex = source.RowIndex;
					destination.DecimalGuid = source.DecimalGuid;
					destination.RowDesc = source.RowDesc;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.KolGuid = source.KolGuid;
					destination.UGuid = source.UGuid;
					destination.DecimalQt = source.DecimalQt;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 298);
					throw;
				}
				return destination;
			}
		}

		public Task<List<GardeshTafsil>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> source, List<GardeshTafsil> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<GardeshTafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsil> source, List<GardeshTafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<GardeshTafsil, GardeshTafsilBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<GardeshTafsilBusiness, GardeshTafsil> MapToDatabase => Mapper;

	public override IItemMapper<GardeshTafsil, GardeshTafsilBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public GardeshTafsil_TablePersistanceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<GardeshTafsilBusiness>> GetAllAsync(GardeshTafsiliFilter filter, EnSystem system, string tranName, CancellationToken token)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 29));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs"))
		{
			IEnumerable<GardeshTafsilBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<GardeshTafsil> res = ((IQueryable<GardeshTafsil>)c.GardeshTafsil).Where((GardeshTafsil p) => p.TafsilGuid == filter.TafsilGuid);
					if (filter.Date1.HasValue)
					{
						res = res?.Where((GardeshTafsil q) => q.Date >= filter.Date1);
					}
					if (filter.Date2.HasValue)
					{
						res = res?.Where((GardeshTafsil q) => q.Date <= filter.Date2);
					}
					if ((filter?.MoeinGuids?.Any()).GetValueOrDefault())
					{
						res = res?.Where((GardeshTafsil q) => filter.MoeinGuids.Contains(q.MoeinGuid));
					}
					if (filter.KolGuid.HasValue)
					{
						res = res?.Where((GardeshTafsil q) => q.KolGuid == filter.KolGuid);
					}
					List<GardeshTafsil> list = await EntityFrameworkQueryableExtensions.ToListAsync<GardeshTafsil>((IQueryable<GardeshTafsil>)res.OrderBy((GardeshTafsil x) => x.Date), token);
					if (list != null && list.Any())
					{
						for (int i = 0; i < res.Count() - 1; i++)
						{
							list[i].RowIndex = i + 1;
						}
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, list, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 51);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<IEnumerable<GardeshTafsilBusiness>> GetAllFromVSanadDetAsync(List<Guid> listTafsilGuid, EnSystem system, int top, DateTime? date1, DateTime? date2, List<Guid> listMoeinGuid, Guid? kolGuid, bool? status, string tranName, CancellationToken token);

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<GardeshTafsilBusiness> items, short tryCount, string tranName, CancellationToken token)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (GardeshTafsilBusiness item in items)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddError(EnSystem.None, "token Canceled!", "token", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 72);
					}
					await SaveAsync(item, tryCount, tranName, token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 78);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 79);
			}
			return ret;
		}
	}

	public async Task<decimal> GardeshBallanceAsync(Guid? tafsilGuid, Guid? moeinGuid, DateTime? date1, DateTime? date2, string tranName, CancellationToken token)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GardeshBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 87));
		using (new JaegerMonitor("GardeshBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs"))
		{
			decimal result;
			try
			{
				IQueryable<GardeshTafsil> queryable = (IQueryable<GardeshTafsil>)acc_ModelContext.GardeshTafsil;
				if (tafsilGuid.HasValue)
				{
					queryable = queryable.Where((GardeshTafsil q) => q.TafsilGuid == tafsilGuid);
				}
				if (moeinGuid.HasValue)
				{
					queryable = queryable.Where((GardeshTafsil q) => q.MoeinGuid == moeinGuid);
				}
				if (date1.HasValue)
				{
					queryable = queryable.Where((GardeshTafsil q) => q.Date >= date1);
				}
				if (date2.HasValue)
				{
					queryable = queryable.Where((GardeshTafsil q) => q.Date <= date2);
				}
				List<GardeshTafsil> list = queryable?.ToList();
				result = ((list != null && list.Count > 0) ? (list.Sum((GardeshTafsil q) => q.Debit) - list.Sum((GardeshTafsil q) => q.Credit)) : default(decimal));
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
				WebErrorLog.Instance.StartLog(ex2, "", "GardeshBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 105);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GardeshBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<GardeshTafsilBusiness> GetLastTransactionAsync(Guid tafsilGuid, EnSystem system, string tranName, CancellationToken token)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 116));
		using (new JaegerMonitor("GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs"))
		{
			GardeshTafsilBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IOrderedQueryable<GardeshTafsil> orderedQueryable = from p in (IQueryable<GardeshTafsil>)c.GardeshTafsil
						where p.TafsilGuid == tafsilGuid
						select p into q
						orderby q.Date descending
						select q;
					IItemAsyncMapper<GardeshTafsil, GardeshTafsilBusiness> mapFromDatabase = MapFromDatabase;
					Task<GardeshTafsilBusiness> task;
					if (mapFromDatabase != null)
					{
						string contextGuid = ContextGuid;
						task = mapFromDatabase.MapAsync(contextGuid, await ((orderedQueryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<GardeshTafsil>((IQueryable<GardeshTafsil>)orderedQueryable, token) : null), null, tranName, token);
					}
					else
					{
						task = null;
					}
					result = await task;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 127);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> kolGuid, IEnumerable<Guid> moeinGuid, IEnumerable<Guid?> tafsilGuid, short tryCount, string tranName, CancellationToken token);

	public async Task<ReturnedSaveFuncInfo> SaveAsync(GardeshTafsilBusiness item, short tryCount, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext dbContext1 = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 146));
				GardeshTafsil item2 = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbContext1.AddOrUpdateAsync(item2, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 148);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 152);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 153);
			}
			return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepositoryBase.cs", 172);
		}
	}
}
