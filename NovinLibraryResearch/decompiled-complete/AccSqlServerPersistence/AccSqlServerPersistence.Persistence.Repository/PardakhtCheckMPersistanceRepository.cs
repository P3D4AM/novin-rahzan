using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using Core.Repository;
using EntityCache;
using EntityCache.Business;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Core;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters.Configs;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository;

internal class PardakhtCheckMPersistanceRepository : PersistenceMasterRepository<PardakhtCheckMBusiness, PardakhtCheckM>, IPardakhtCheckMRepository, IPersistenceMasterRepository<PardakhtCheckMBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PardakhtCheckMBusiness, PardakhtCheckM>, IItemAsyncMapper<PardakhtCheckM, PardakhtCheckMBusiness>
	{
		public async Task<PardakhtCheckM> MapAsync(string contextGuid, PardakhtCheckMBusiness source, PardakhtCheckM destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs"))
			{
				_ = 1;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new PardakhtCheckM();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Description = source.RowDesc ?? "";
					destination.GuidCheck = source.GuidCheck;
					destination.SayadiName = source.SayadiName;
					destination.SayadiNationalCode = source.SayadiNationalCode;
					destination.Price = source.Price;
					destination.MasterGuid = source.MasterGuid;
					PardakhtCheckM pardakhtCheckM = destination;
					pardakhtCheckM.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token, tranName);
					pardakhtCheckM = destination;
					pardakhtCheckM.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token, tranName);
					destination.SayadiRegistered = source.SayadiRegistered;
					destination.Ordinal = source.Ordinal;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 203);
					throw;
				}
				return destination;
			}
		}

		public Task<PardakhtCheckMBusiness> MapAsync(string contextGuid, PardakhtCheckM source, PardakhtCheckMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs"))
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
						destination = new PardakhtCheckMBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.RowDesc = source.Description;
					destination.GuidCheck = source.GuidCheck;
					destination.SetPrice(EnSystem.Mapper, source.Price);
					destination.SetCreditMoeinGuid(source.CreditMoeinGuid);
					destination.SetCreditTafsilGuid(source.CreditTafsilGuid);
					destination.MasterGuid = source.MasterGuid;
					destination.SayadiName = source.SayadiName;
					destination.SayadiNationalCode = source.SayadiNationalCode;
					destination.SayadiRegistered = source.SayadiRegistered;
					destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 237);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<PardakhtCheckM>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckMBusiness> source, List<PardakhtCheckM> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PardakhtCheckMBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckM> source, List<PardakhtCheckMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private ViewCheckMPersistenceRepository _view;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<PardakhtCheckM, PardakhtCheckMBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PardakhtCheckMBusiness, PardakhtCheckM> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public PardakhtCheckMPersistanceRepository(ContextHolder db)
		: base(db)
	{
		_view = new ViewCheckMPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				_view?.Dispose();
				_view = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 44);
		}
	}

	public async Task<IEnumerable<PardakhtCheckMBusiness>> GetAllWithChequeGuidAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllWithChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 48));
		IEnumerable<PardakhtCheckMBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				ENSource source = ClsCache.GetCache(base.ContextGuid).Source;
				IOrderedQueryable<PardakhtCheckM> ret = ((IQueryable<PardakhtCheckM>)c.PardakhtCheckM).Where((PardakhtCheckM q) => q.GuidCheck == chequeGuid)?.OrderBy((PardakhtCheckM q) => q.PardakhtItem.DateSabt);
				result = ((ret != null) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, ret, null, tranName, token)) : ((source == ENSource.Accounting) ? null : ((await _view.GetAsync(chequeGuid, token, tranName) != null) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, ret, null, tranName, token)) : null)));
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllWithChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 63);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllWithChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckMBusiness> items, PardakhtBusiness masterItem, EnSystem system, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (items?.Any((PardakhtCheckMBusiness p) => p.Guid == Guid.Empty) ?? false)
			{
				throw new ArgumentException("one or more items in list contains empty value for field Guid");
			}
			try
			{
				if (items != null)
				{
					if (items != null && items.Count() > 0)
					{
						List<PardakhtCheckM> des = await MapToDatabase.MapListAsync(base.ContextGuid, items, null, tranName, token);
						Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 83));
						foreach (PardakhtCheckM item in des)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (system != EnSystem.Transfer)
							{
								ChequeMOperation chequeOperation = await dbcontext.ChequeMOperation.FindAsync(new object[1] { item.Guid });
								ReceptionCheck receptionCheck = await dbcontext.ReceptionCheck.FindAsync(new object[1] { item.GuidCheck });
								if (chequeOperation == null || (chequeOperation.AsGuid != item.MasterGuid && system != EnSystem.Transfer))
								{
									ChequeMOperationConfig config = new ChequeMOperationConfig
									{
										CashierGuid = masterItem.UserGuid,
										ChequeGuid = item.GuidCheck,
										DateM = masterItem.Date,
										System = EnSystem.None,
										Guid = item.Guid,
										SanadNo = masterItem.SanadNo,
										IsSanadAffective = (receptionCheck != null && !receptionCheck.IsEnteghali)
									};
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.KharjKardanAsync(base.ContextGuid, config, masterItem.Guid, masterItem.MoeinGuid, masterItem.TafsilGuid, 0, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 102);
								}
							}
							item.MasterGuid = masterItem.Guid;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 107);
						}
						if (!ret.HasError)
						{
							ClsCache.GetCache(base.ContextGuid).BusinessAggregator.Events.ChequM.RaiseOperationSaved(this, EventArgs.Empty);
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 117);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 118);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckMBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (items?.Any((PardakhtCheckMBusiness p) => p.Guid == Guid.Empty) ?? false)
			{
				throw new ArgumentException("one or more items in list contains empty value for field Guid");
			}
			try
			{
				if (items != null)
				{
					if (items != null && items.Count() > 0)
					{
						List<PardakhtCheckM> des = await MapToDatabase.MapListAsync(base.ContextGuid, items, null, tranName, token);
						Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 133));
						foreach (PardakhtCheckM item in des)
						{
							item.MasterGuid = masterGuid;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 137);
						}
						if (!ret.HasError)
						{
							ClsCache.GetCache(base.ContextGuid).BusinessAggregator.Events.ChequM.RaiseOperationSaved(this, EventArgs.Empty);
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 147);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 148);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<PardakhtCheckMBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 154));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs"))
		{
			IEnumerable<PardakhtCheckMBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<PardakhtCheckM> list = await EntityFrameworkQueryableExtensions.ToListAsync<PardakhtCheckM>((IQueryable<PardakhtCheckM>)c.PardakhtCheckM, token);
					result = ((list != null && list.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, list, null, tranName, token)) : null);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs", 165);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMPersistanceRepository.cs");
				}
			}
			return result;
		}
	}
}
