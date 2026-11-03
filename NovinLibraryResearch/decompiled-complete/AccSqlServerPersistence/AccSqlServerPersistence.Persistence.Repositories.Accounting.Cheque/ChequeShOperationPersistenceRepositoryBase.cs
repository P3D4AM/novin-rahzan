using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Cheque;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Cheque;

internal abstract class ChequeShOperationPersistenceRepositoryBase : PersistenceSyncRepository<ChequeShOperationBusiness, ChequeShOperation>, IChequeShOperationRepository, ISyncRepository<ChequeShOperationBusiness>, IEditableDelivery, IRepository<ChequeShOperationBusiness>, INoGuidRepository<ChequeShOperationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ChequeShOperationBusiness, ChequeShOperation>, IItemAsyncMapper<ChequeShOperation, ChequeShOperationBusiness>, IItemMapper<ChequeShOperation, ChequeShOperationBusiness>
	{
		public ChequeShOperationBusiness Map(string contextGuid, ChequeShOperation source, ChequeShOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ChequeShOperationBusiness(contextGuid);
				}
				destination.DateM = source.DateM;
				destination.CashierGuid = source.CashierGuid;
				destination.SanadSt = source.SanadSt;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.Ordinal = source.Ordinal;
				destination.OldState = source.OldState;
				destination.NewState = source.NewState;
				destination.SanadNo = source.SanadNo;
				destination.ChequeGuid = source.ChequeGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.StatusId = source.StatusId;
				destination.DateUtc = source.DateUtc;
				destination.Price = source.Price;
				return destination;
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
				WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 227);
				throw;
			}
		}

		public Task<ChequeShOperation> MapAsync(string contextGuid, ChequeShOperationBusiness source, ChequeShOperation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ChequeShOperation();
				}
				destination.DateM = source.DateM;
				destination.CashierGuid = source.CashierGuid;
				destination.SanadSt = source.SanadSt;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.Ordinal = source.Ordinal;
				destination.OldState = source.OldState;
				destination.NewState = source.NewState;
				destination.SanadNo = source.SanadNo;
				destination.ChequeGuid = source.ChequeGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.StatusId = source.StatusId;
				destination.DateUtc = destination.DateM.ToUniversalTime();
				destination.Price = source.Price;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 270);
				throw;
			}
		}

		public Task<ChequeShOperationBusiness> MapAsync(string contextGuid, ChequeShOperation source, ChequeShOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public Task<List<ChequeShOperation>> MapListAsync(string contextGuid, IEnumerable<ChequeShOperationBusiness> source, List<ChequeShOperation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ChequeShOperationBusiness>> MapListAsync(string contextGuid, IEnumerable<ChequeShOperation> source, List<ChequeShOperationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemMapper<ChequeShOperation, ChequeShOperationBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override IItemAsyncMapper<ChequeShOperation, ChequeShOperationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ChequeShOperationBusiness, ChequeShOperation> MapToDatabase => Mapper;

	internal ChequeShOperationPersistenceRepositoryBase(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 42);
		}
	}

	public async Task<IEnumerable<ChequeShOperationBusiness>> GetAllAsync(ChequeShOperationFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 46));
		IEnumerable<ChequeShOperationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ChequeShOperation> queryable = (IQueryable<ChequeShOperation>)c.ChequeShOperation;
				if (filter != null)
				{
					if (filter.ChequeGuid.HasValue && filter.ChequeGuid != Guid.Empty)
					{
						queryable = queryable?.Where((ChequeShOperation q) => q.ChequeGuid == filter.ChequeGuid);
					}
					if (filter.LastStatus.HasValue)
					{
						queryable = queryable?.Where((ChequeShOperation q) => (int?)q.NewState == (int?)filter.LastStatus);
					}
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((ChequeShOperation q) => q.DateM >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((ChequeShOperation q) => q.DateM <= filter.DateTo);
					}
					if ((filter?.StatusId).GetValueOrDefault() > 0)
					{
						queryable = queryable?.Where((ChequeShOperation q) => q.StatusId == filter.StatusId.Value);
					}
				}
				List<ChequeShOperation> source = queryable?.ToList();
				result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 65);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 82));
				ChequeShOperation chequeShOperation = await dbcontext.ChequeShOperation.FindAsync(new object[1] { guid });
				chequeShOperation.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(chequeShOperation, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 85);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 90);
				ret.AddReturnedValue(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 91);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByChequeGuidAsync(Guid chequeGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs"))
		{
			int line = 0;
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 103));
				line = 1;
				IQueryable<ChequeShOperation> queryable = ((IQueryable<ChequeShOperation>)((DbContext)acc_ModelContext).Set<ChequeShOperation>()).Where((ChequeShOperation p) => p.ChequeGuid == chequeGuid);
				line = 2;
				if (queryable != null && queryable.Any())
				{
					line = 3;
					((DbContext)acc_ModelContext).Set<ChequeShOperation>().RemoveRange((IEnumerable<ChequeShOperation>)queryable);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 115);
				ret.AddReturnedValue(ex, "", "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 116);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ChequeShOperationBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 128));
				ChequeShOperation chequeShOperation = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				if (item.Guid == Guid.Empty)
				{
					throw new Exception($"Guid is empty. item type:{item.GetType()}");
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(chequeShOperation, chequeShOperation.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 132);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 136);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 137);
			}
			return ret;
		}
	}

	public abstract Task<List<ChequeShInstantModel>> GetAllChequeShInstantAsync(DateTime dateM, CancellationToken token, string tranName);

	public async Task<IEnumerable<ChequeShOperationReportBusiness>> GetAllReportAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 144));
		IEnumerable<ChequeShOperationReportBusiness> result;
		try
		{
			IQueryable<ChequeShOperation> queryable = ((IQueryable<ChequeShOperation>)acc_ModelContext.ChequeShOperation).Where((ChequeShOperation q) => q.ChequeGuid == chequeGuid);
			result = ((queryable != null && queryable.Any()) ? queryable.Select((ChequeShOperation q) => new ChequeShOperationReportBusiness
			{
				CashierGuid = q.CashierGuid,
				CashierName = q.Cashier.Name,
				DateM = q.DateM,
				Guid = q.Guid,
				GuidCheque = q.ChequeGuid,
				NewState = q.NewState,
				Number = q.Cheque.Number,
				OldState = q.OldState,
				Ordinal = q.Ordinal,
				Price = q.Cheque.Price,
				SanadNO = q.SanadNo,
				TafsilName = (q.Cheque.Aval2re ? q.Cheque.DebitA2reTafsil.Name : q.Cheque.PardakhtItem.FullName),
				CreditMoeinGuid = q.CreditMoeinGuid,
				CreditTafsilGuid = q.CreditTafsilGuid,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid
			})?.ToList() : null);
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs", 174);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}
}
