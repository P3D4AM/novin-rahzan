using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Sanad;

internal abstract class ViewSanadDetailCoverBasePersistenceRepository : PersistenceRepository<ViewSanadDetailCoverBusiness, ViewSanadDetailCover>, IViewSanadDetailCoverRepository, IRepository<ViewSanadDetailCoverBusiness>, INoGuidRepository<ViewSanadDetailCoverBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ViewSanadDetailCoverBusiness, ViewSanadDetailCover>, IItemAsyncMapper<ViewSanadDetailCover, ViewSanadDetailCoverBusiness>
	{
		public Task<ViewSanadDetailCover> MapAsync(string contextGuid, ViewSanadDetailCoverBusiness source, ViewSanadDetailCover destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewSanadDetailCover();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Debit = source.Debit;
				destination.Credit = source.Credit;
				destination.DecimalGuid = source.DecimalGuid;
				destination.Qt = source.Qt;
				destination.Serial = source.Serial;
				destination.AsType = source.AsType;
				destination.AsGuid = source.AsGuid;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.Date = source.Date;
				destination.CashierGuid = source.CashierGuid;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				destination.Id2 = source.Id2;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.Price = source.Price;
				destination.Qt2 = source.Qt2;
				destination.RDiscount = source.RDiscount;
				destination.RBazaryab = source.RBazaryab;
				destination.SanadDesc = source.SanadDesc;
				destination.RDesc = source.RDesc;
				destination.ProductGuid = source.ProductGuid;
				destination.DepotGuid = source.DepotGuid;
				destination.VahedGuid = source.VahedGuid;
				destination.DateMoaser = source.DateMoaser;
				destination.DecimalQt = source.DecimalQt;
				destination.DecimalPrice = source.DecimalPrice;
				destination.DetailGuid = source.DetailGuid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.AsTypeDocument = source.AsTypeDocument;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 139);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ViewSanadDetailCoverBusiness> MapAsync(string contextGuid, ViewSanadDetailCover source, ViewSanadDetailCoverBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewSanadDetailCoverBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Debit = source.Debit;
				destination.Credit = source.Credit;
				destination.DecimalGuid = source.DecimalGuid;
				destination.Qt = source.Qt;
				destination.Serial = source.Serial;
				destination.AsType = source.AsType;
				destination.AsGuid = source.AsGuid;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.Date = source.Date;
				destination.CashierGuid = source.CashierGuid;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				destination.Id2 = source.Id2;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.Price = source.Price;
				destination.Qt2 = source.Qt2;
				destination.RDiscount = source.RDiscount;
				destination.RBazaryab = source.RBazaryab;
				destination.SanadDesc = source.SanadDesc;
				destination.RDesc = source.RDesc;
				destination.ProductGuid = source.ProductGuid;
				destination.DepotGuid = source.DepotGuid;
				destination.VahedGuid = source.VahedGuid;
				destination.DateMoaser = source.DateMoaser;
				destination.DecimalQt = source.DecimalQt;
				destination.DecimalPrice = source.DecimalPrice;
				destination.DetailGuid = source.DetailGuid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 195);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ViewSanadDetailCover>> MapListAsync(string contextGuid, IEnumerable<ViewSanadDetailCoverBusiness> source, List<ViewSanadDetailCover> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ViewSanadDetailCoverBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewSanadDetailCover> source, List<ViewSanadDetailCoverBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private new bool IsDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<ViewSanadDetailCover, ViewSanadDetailCoverBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<ViewSanadDetailCover, ViewSanadDetailCoverBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ViewSanadDetailCoverBusiness, ViewSanadDetailCover> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public ViewSanadDetailCoverBasePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 41);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveArticlesAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs"))
		{
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 50));
				IQueryable<ViewSanadDetailCover> queryable = ((IQueryable<ViewSanadDetailCover>)acc_ModelContext.ViewSanadDetailCover).Where((ViewSanadDetailCover p) => p.AsGuid == asGuid && p.AsTypeDocument == asType);
				if (queryable != null && queryable.Any())
				{
					acc_ModelContext.ViewSanadDetailCover.RemoveRange((IEnumerable<ViewSanadDetailCover>)queryable);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 56);
				ret.AddReturnedValue(ex, "", "RemoveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 57);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveArticlesAsync(IEnumerable<ViewSanadDetailCoverBusiness> items, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (items == null || !items.Any())
		{
			return ret;
		}
		try
		{
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 1800000, "SaveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 68));
			foreach (ViewSanadDetailCoverBusiness item in items)
			{
				if (item != null)
				{
					ViewSanadDetailCover viewSanadDetailCover = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(viewSanadDetailCover, viewSanadDetailCover.Guid), ReturnedState.None, "SaveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 73);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 78);
			ret.AddReturnedValue(ex, "", "SaveArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverBasePersistenceRepository.cs", 79);
		}
		return ret;
	}

	public virtual Task<IEnumerable<ViewSanadDetailCoverBusiness>> GetAllInvalidRelationsAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
