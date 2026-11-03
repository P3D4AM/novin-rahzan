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

internal abstract class ChequeMOperationPersistenceRepositoryBase : PersistenceSyncRepository<ChequeMOperationBusiness, ChequeMOperation>, IChequeMOperationRepository, ISyncRepository<ChequeMOperationBusiness>, IEditableDelivery, IRepository<ChequeMOperationBusiness>, INoGuidRepository<ChequeMOperationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ChequeMOperationBusiness, ChequeMOperation>, IItemAsyncMapper<ChequeMOperation, ChequeMOperationBusiness>, IItemMapper<ChequeMOperation, ChequeMOperationBusiness>
	{
		public ChequeMOperationBusiness Map(string contextGuid, ChequeMOperation source, ChequeMOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ChequeMOperationBusiness(contextGuid);
				}
				destination.GuidCheque = source.GuidCheque;
				destination.DateM = source.DateM;
				destination.CashierGuid = source.CashierGuid;
				destination.SanadNO = source.SanadNO;
				destination.SanadSt = source.SanadSt;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DateSabt = source.DateSabt;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.Avale2re_Transfer = source.Avale2re_Transfer;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.Ordinal = source.Ordinal;
				destination.OldState = source.OldState;
				destination.NewState = source.NewState;
				destination.IsCreditTafsilAffective = source.IsCreditTafsilAffective;
				destination.IsDebitTafsilAffective = source.IsDebitTafsilAffective;
				destination.Description = source.Description;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.AsType = source.AsType;
				destination.AsGuid = source.AsGuid;
				destination.IsSanadAffective = source.IsSanadAffective;
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
				WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 303);
				throw;
			}
		}

		public Task<ChequeMOperation> MapAsync(string contextGuid, ChequeMOperationBusiness source, ChequeMOperation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ChequeMOperation();
				}
				destination.GuidCheque = source.GuidCheque;
				destination.DateM = source.DateM;
				destination.CashierGuid = source.CashierGuid;
				destination.SanadNO = source.SanadNO;
				destination.SanadSt = source.SanadSt;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DateSabt = source.DateSabt;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.Avale2re_Transfer = source.Avale2re_Transfer;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.Ordinal = source.Ordinal;
				destination.OldState = source.OldState;
				destination.NewState = source.NewState;
				destination.IsCreditTafsilAffective = source.IsCreditTafsilAffective;
				destination.IsDebitTafsilAffective = source.IsDebitTafsilAffective;
				destination.Description = source.Description;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.AsType = source.AsType;
				destination.AsGuid = source.AsGuid;
				destination.IsSanadAffective = source.IsSanadAffective;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 354);
				throw;
			}
		}

		public Task<ChequeMOperationBusiness> MapAsync(string contextGuid, ChequeMOperation source, ChequeMOperationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public Task<List<ChequeMOperation>> MapListAsync(string contextGuid, IEnumerable<ChequeMOperationBusiness> source, List<ChequeMOperation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ChequeMOperationBusiness>> MapListAsync(string contextGuid, IEnumerable<ChequeMOperation> source, List<ChequeMOperationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemMapper<ChequeMOperation, ChequeMOperationBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override IItemAsyncMapper<ChequeMOperation, ChequeMOperationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ChequeMOperationBusiness, ChequeMOperation> MapToDatabase => Mapper;

	internal ChequeMOperationPersistenceRepositoryBase(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 42);
		}
	}

	public async Task<short> GetNextOrdinalAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 46));
		short result;
		try
		{
			IQueryable<ChequeMOperation> queryable = ((IQueryable<ChequeMOperation>)acc_ModelContext.ChequeMOperation).Where((ChequeMOperation q) => q.GuidCheque == chequeGuid);
			result = (short)((queryable == null || !queryable.Any()) ? 1 : ((short)(queryable.Max((ChequeMOperation q) => q.Ordinal) + 1)));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 57);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<ChequeMOperationBusiness>> GetAllAsync(ChequeMOperationFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 68));
		IEnumerable<ChequeMOperationBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				IQueryable<ChequeMOperation> queryable = (IQueryable<ChequeMOperation>)c.ChequeMOperation;
				if (filter != null)
				{
					if (filter.ChequeGuid.HasValue && filter.ChequeGuid != Guid.Empty)
					{
						queryable = queryable?.Where((ChequeMOperation q) => q.GuidCheque == filter.ChequeGuid);
					}
					if (filter.LastStatus.HasValue)
					{
						queryable = queryable?.Where((ChequeMOperation q) => (int?)q.NewState == (int?)filter.LastStatus);
					}
					if (filter.SanadNO > 0)
					{
						queryable = queryable?.Where((ChequeMOperation q) => q.SanadNO == filter.SanadNO);
					}
					if (filter.AsGuid.HasValue && filter.AsGuid != Guid.Empty)
					{
						queryable = queryable?.Where((ChequeMOperation q) => q.AsGuid == filter.AsGuid);
					}
					if (filter.AsType.HasValue)
					{
						queryable = queryable?.Where((ChequeMOperation q) => (int?)q.AsType == (int?)filter.AsType);
					}
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((ChequeMOperation q) => q.DateM >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((ChequeMOperation q) => q.DateM <= filter.DateTo);
					}
					if ((filter?.StatusId).GetValueOrDefault() > 0)
					{
						queryable = queryable?.Where((ChequeMOperation q) => q.StatusId == filter.StatusId.Value);
					}
				}
				List<ChequeMOperation> source = await EntityFrameworkQueryableExtensions.ToListAsync<ChequeMOperation>(queryable, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 90);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs");
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
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 107));
				ChequeMOperation chequeMOperation = await dbcontext.ChequeMOperation.FindAsync(new object[1] { guid });
				chequeMOperation.SanadNO = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(chequeMOperation, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 110);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 115);
				ret.AddReturnedValue(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 116);
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
		using (new JaegerMonitor("RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs"))
		{
			int line = 0;
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 128));
				line = 1;
				IQueryable<ChequeMOperation> queryable = ((IQueryable<ChequeMOperation>)((DbContext)acc_ModelContext).Set<ChequeMOperation>()).Where((ChequeMOperation p) => p.GuidCheque == chequeGuid);
				line = 2;
				if (queryable != null && queryable.Any())
				{
					line = 3;
					((DbContext)acc_ModelContext).Set<ChequeMOperation>().RemoveRange((IEnumerable<ChequeMOperation>)queryable);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 140);
				ret.AddReturnedValue(ex, "", "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 141);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<ChequeMOperationReportBusiness>> GetAllReportAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 147));
		IEnumerable<ChequeMOperationReportBusiness> result;
		try
		{
			IQueryable<ChequeMOperation> queryable = ((IQueryable<ChequeMOperation>)acc_ModelContext.ChequeMOperation).Where((ChequeMOperation q) => q.GuidCheque == chequeGuid);
			result = ((queryable != null && queryable.Any()) ? queryable.Select((ChequeMOperation q) => new ChequeMOperationReportBusiness
			{
				CashierGuid = q.CashierGuid,
				CashierName = q.Cashier.Name,
				DateM = q.DateM,
				Guid = q.Guid,
				GuidCheque = q.GuidCheque,
				NewState = q.NewState,
				Number = q.Cheque.Number,
				OldState = q.OldState,
				Ordinal = q.Ordinal,
				Price = q.Cheque.Price,
				SanadNO = q.SanadNO,
				TafsilName = (q.Cheque.Aval2re ? q.Cheque.A2re_TafsilItem.Name : q.Cheque.ReceptionItem.FullName),
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 177);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> IncreaseOrinalAsync(Guid chequeGuid, short orinal, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "IncreaseOrinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 191));
			List<ChequeMOperation> list = await EntityFrameworkQueryableExtensions.ToListAsync<ChequeMOperation>(((IQueryable<ChequeMOperation>)c.ChequeMOperation).Where((ChequeMOperation q) => q.GuidCheque == chequeGuid && q.Ordinal >= orinal), token);
			if (list == null || !list.Any())
			{
				return res;
			}
			foreach (ChequeMOperation item in list)
			{
				item.Ordinal++;
			}
			((DbContext)c).UpdateRange((IEnumerable<object>)list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IncreaseOrinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 201);
			res.AddReturnedValue(ex, "", "IncreaseOrinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 202);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateDataBaseOperationsAsync(List<Tuple<Guid, short>> tuples, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(tuples?.Any() ?? false))
			{
				return res;
			}
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 212));
			foreach (Tuple<Guid, short> item in tuples)
			{
				ChequeMOperation chequeMOperation = await c.ChequeMOperation.FindAsync(new object[1] { item.Item1 });
				if (chequeMOperation != null)
				{
					chequeMOperation.Ordinal = item.Item2;
					((DbContext)c).Update<ChequeMOperation>(chequeMOperation);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 223);
			res.AddReturnedValue(ex, "", "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 224);
		}
		return res;
	}

	public abstract Task<List<MoghayeratChequeAvalDoreModel>> GetAllAvalDoreMoghyerat(Guid chequeMojodGuid, Guid chequeVagozarGuid, CancellationToken token, string tranName);

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ChequeMOperationBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 237));
				ChequeMOperation chequeMOperation = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				if (item.Guid == Guid.Empty)
				{
					throw new Exception($"Guid is empty. item type:{item.GetType()}");
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(chequeMOperation, chequeMOperation.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 241);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 245);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepositoryBase.cs", 246);
			}
			return ret;
		}
	}

	public abstract Task<List<ChequeMInstantModel>> GetAllChequeMInstantAsync(DateTime dateM, CancellationToken token, string tranName);
}
