using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repository;
using EntityCache;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Sanad;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories;

public abstract class PardakhtPersistenceRepositoryBase : IPardakhtRepository, IEntityRepository<PardakhtBusiness, PardakhtReportBusiness>, IEditableDelivery, IDisposable
{
	private class mapper : IItemAsyncMapper<PardakhtBusiness, Pardakht>, IItemAsyncMapper<Pardakht, PardakhtBusiness>, IItemAsyncMapper<Pardakht, PardakhtReportBusiness>
	{
		private PardakhtBankPersistenceRepository _pardakhtBankRepo;

		private PardakhtCheckMPersistanceRepository _pardakhtCheckMRepo;

		private PardakhtCheckShMasterPersistanceRepository _pardakhtCheckShRepo;

		private SanadDetPersistenceRepository _pardakhtEnteghalShRepo;

		private PardakhtNaghdPersistenceRepository _pardakhtNaghdRepo;

		public mapper(PardakhtCheckMPersistanceRepository pardakhtCheckMRepo, PardakhtCheckShMasterPersistanceRepository pardakhtCheckShRepo, SanadDetPersistenceRepository pardakhtEnteghalRepo, PardakhtBankPersistenceRepository pardakhtbankRepo, PardakhtNaghdPersistenceRepository pardakhtNaghRepo)
		{
			_pardakhtCheckShRepo = pardakhtCheckShRepo;
			_pardakhtEnteghalShRepo = pardakhtEnteghalRepo;
			_pardakhtBankRepo = pardakhtbankRepo;
			_pardakhtCheckMRepo = pardakhtCheckMRepo;
			_pardakhtNaghdRepo = pardakhtNaghRepo;
		}

		public async Task<Pardakht> MapAsync(string contextGuid, PardakhtBusiness source, Pardakht destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
			{
				_ = 8;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new Pardakht();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					Pardakht pardakht;
					if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
					{
						pardakht = destination;
						pardakht.AfterAccount = await source.GetAfterAccountAsync(EnSystem.Mapper, token, tranName);
						pardakht = destination;
						pardakht.BeforeAccount = await source.GetBeforeAccountAsync(EnSystem.Mapper, token, tranName);
					}
					destination.IsTemp = source.IsTemp;
					destination.AndroidIme = source.AndroidIme;
					pardakht = destination;
					pardakht.FullName = await source.GetFullNameAsync(EnSystem.Mapper, token, tranName);
					destination.CpuSerial = source.CpuSerial;
					destination.Date = source.Date;
					destination.DateSabt = source.DateSabt;
					destination.Description = source.Description;
					destination.HddSerial = source.HddSerial;
					pardakht = destination;
					pardakht.Id2 = await source.GetId2Async();
					destination.MoeinGuid = source.MoeinGuid;
					destination.TafsilGuid = source.TafsilGuid;
					destination.UserGuid = source.UserGuid;
					destination.Ip = source.Ip;
					destination.IsRead = source.IsRead;
					destination.LkSerial = source.LkSerial;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.Sum = source.Sum;
					destination.SumCheckM = source.SumCheckM;
					destination.SumCheckSh = source.SumCheckSh;
					destination.SumEnteghal = source.SumEnteghal;
					destination.SumBank = source.SumBank;
					destination.SumNaghd = source.SumNaghd;
					destination.StatusId = source.StatusId;
					destination.TransferDate = source.TransferDate;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					pardakht = destination;
					pardakht.Bank = await _pardakhtBankRepo.MapToDatabase.MapListAsync(contextGuid, source.Bank, destination.Bank?.ToList(), tranName, token);
					pardakht = destination;
					pardakht.CheckM = await _pardakhtCheckMRepo.MapToDatabase.MapListAsync(contextGuid, source.CheckM, destination?.CheckM.ToList(), tranName, token);
					pardakht = destination;
					pardakht.CheckSh = await _pardakhtCheckShRepo.MapToDatabase.MapListAsync(contextGuid, source.CheckSh, destination?.CheckSh.ToList(), tranName, token);
					pardakht = destination;
					pardakht.Naghd = await _pardakhtNaghdRepo.MapToDatabase.MapListAsync(contextGuid, source.Naghd, destination.Naghd?.ToList(), tranName, token);
					pardakht = destination;
					pardakht.Enteghal = await _pardakhtEnteghalShRepo.MapToDatabase.MapListAsync(contextGuid, source.Enteghal, destination.Enteghal.ToList(), tranName, token);
					if (source.MetaData != null)
					{
						destination.MetaData = Json.ToStringJson(source.MetaData);
					}
					destination.DateUtc = destination.Date.ToUniversalTime();
					destination.DateMoaser = source.DateMoaser;
					destination.TafsilGuid2 = source.TafsilGuid2;
					destination.TafsilGuid3 = source.TafsilGuid3;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 711);
					throw;
				}
			}
		}

		public async Task<PardakhtBusiness> MapAsync(string contextGuid, Pardakht source, PardakhtBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
			{
				_ = 5;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new PardakhtBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.SetAfterAccount(source.AfterAccount);
					destination.SetBeforeAccount(source.BeforeAccount);
					destination.IsTemp = source.IsTemp;
					destination.AndroidIme = source.AndroidIme;
					destination.TransferDate = source.TransferDate;
					destination.CpuSerial = source.CpuSerial;
					destination.Date = source.Date;
					destination.DateSabt = source.DateSabt;
					destination.Description = source.Description;
					destination.HddSerial = source.HddSerial;
					destination.SetId2(source.Id2, token);
					destination.MoeinGuid = source.MoeinGuid;
					await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper, token, tranName);
					destination.UserGuid = source.UserGuid;
					destination.Ip = source.Ip;
					destination.IsRead = source.IsRead;
					destination.LkSerial = source.LkSerial;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.StatusId = source.StatusId;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DateUtc = source.DateUtc;
					destination.DateMoaser = source.DateMoaser;
					destination.TafsilGuid2 = source.TafsilGuid2;
					destination.TafsilGuid3 = source.TafsilGuid3;
					destination.AddRangeToChequeMList(await _pardakhtCheckMRepo.MapFromDatabase.MapListAsync(contextGuid, source.CheckM, destination.CheckM?.ToList(), tranName, token), clearList: true, EnSystem.Mapper);
					destination.AddRangeToChequeShList(await _pardakhtCheckShRepo.MapFromDatabase.MapListAsync(contextGuid, source.CheckSh, destination.CheckSh?.ToList(), tranName, token), clearList: true, EnSystem.Mapper);
					destination.AddRangeToNaghdList(await _pardakhtNaghdRepo.MapFromDatabase.MapListAsync(contextGuid, source.Naghd, destination.Naghd?.ToList(), tranName, token), clearList: true, EnSystem.Mapper);
					destination.AddRangeToBankList(await _pardakhtBankRepo.MapFromDatabase.MapListAsync(contextGuid, source.Bank, destination.Bank?.ToList(), tranName, token), clearList: true, EnSystem.Mapper);
					destination.AddRangeToEnteghalList(await _pardakhtEnteghalShRepo.MapFromDatabase.MapListAsync(contextGuid, source.Enteghal, destination.Enteghal?.ToList(), tranName, token), clearList: true, EnSystem.Mapper);
					destination.Source = source.Source;
					if (!string.IsNullOrEmpty(source.MetaData))
					{
						destination.MetaData = source.MetaData.FromJson<PardakhtMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 775);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 782);
					throw;
				}
			}
		}

		public Task<List<Pardakht>> MapListAsync(string contextGuid, IEnumerable<PardakhtBusiness> source, List<Pardakht> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PardakhtBusiness>> MapListAsync(string contextGuid, IEnumerable<Pardakht> source, List<PardakhtBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<PardakhtReportBusiness> MapAsync(string contextGuid, Pardakht source, PardakhtReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PardakhtReportBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.AndroidIme = source.AndroidIme;
				destination.CpuSerial = source.CpuSerial;
				destination.DateM = source.Date;
				destination.DateSabt = source.DateSabt;
				destination.Description = source.Description;
				destination.HddSerial = source.HddSerial;
				destination.Id2 = source.Id2;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.CashierGuid = source.UserGuid;
				destination.Ip = source.Ip;
				destination.IsRead = source.IsRead;
				destination.LkSerial = source.LkSerial;
				destination.HostRefrenced = source.HostRefrenced;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = (short)source.SanadSt;
				destination.SumChequeM = source.SumCheckM;
				destination.SumChequeSh = source.SumCheckSh;
				destination.SumEnteghal = source.SumEnteghal;
				destination.SumHavale = source.SumBank;
				destination.SumNaghd = source.SumNaghd;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.AfterAccount = source.AfterAccount;
				destination.BeforeAccount = source.BeforeAccount;
				destination.IsTemp = source.IsTemp;
				destination.ServerStatus = source.ServerStatus;
				destination.TransferDate = source.TransferDate;
				destination.Source = source.Source;
				destination.StatusId = source.StatusId;
				destination.TafsilGroupGuid = source.TafsilItem?.TafsilGroupGuid ?? Guid.Empty;
				destination.CashierName = source.CashierItem?.Name;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 843);
				throw;
			}
		}

		public Task<List<PardakhtReportBusiness>> MapListAsync(string contextGuid, IEnumerable<Pardakht> source, List<PardakhtReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private PardakhtBankPersistenceRepository pardakhtBankRepo;

	private PardakhtCheckMPersistanceRepository pardakhtCheckMRepo;

	private PardakhtCheckShMasterPersistanceRepository pardakhtCheckShRepo;

	private SanadDetPersistenceRepository pardakhtEnteghalShRepo;

	private PardakhtNaghdPersistenceRepository pardakhtNaghdRepo;

	protected ContextHolder dbContext;

	private mapper _mapper { get; set; }

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => Context.Guid;

	public IItemAsyncMapper<Pardakht, PardakhtReportBusiness> MapFromDatabaseToReport => _mapper;

	public IItemAsyncMapper<Pardakht, PardakhtBusiness> MapFromDatabase => _mapper;

	public IItemAsyncMapper<PardakhtBusiness, Pardakht> MapToDatabase => _mapper;

	internal PardakhtPersistenceRepositoryBase(ContextHolder context)
	{
		try
		{
			pardakhtBankRepo = new PardakhtBankPersistenceRepository(context);
			pardakhtCheckMRepo = new PardakhtCheckMPersistanceRepository(context);
			pardakhtCheckShRepo = new PardakhtCheckShAccountingPersistanceRepository(context);
			pardakhtEnteghalShRepo = new SanadDetPersistenceRepository(context);
			pardakhtNaghdRepo = new PardakhtNaghdPersistenceRepository(context);
			dbContext = context;
			_mapper = new mapper(pardakhtCheckMRepo, pardakhtCheckShRepo, pardakhtEnteghalShRepo, pardakhtBankRepo, pardakhtNaghdRepo);
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
			WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 54);
			throw;
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				pardakhtBankRepo?.Dispose();
				pardakhtBankRepo = null;
				pardakhtCheckMRepo?.Dispose();
				pardakhtCheckMRepo = null;
				pardakhtCheckShRepo?.Dispose();
				pardakhtCheckShRepo = null;
				pardakhtEnteghalShRepo?.Dispose();
				pardakhtEnteghalShRepo = null;
				pardakhtNaghdRepo?.Dispose();
				pardakhtNaghdRepo = null;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 84);
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 98));
				Pardakht pardakht = await dbcontext.Pardakht.FindAsync(new object[1] { guid });
				pardakht.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(pardakht, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 101);
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
				WebErrorLog.Instance.StartLog(ex3, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 108);
				ret.AddReturnedValue(ex3, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 109);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 122));
				Pardakht pardakht = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Pardakht>((IQueryable<Pardakht>)dbcontext.Pardakht, (Expression<Func<Pardakht, bool>>)((Pardakht x) => x.Guid == item), token);
				if (pardakht == null)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtBankRepo.RemoveAsync(item, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 126);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheckMRepo.RemoveAsync(item, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 127);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtEnteghalShRepo.RemoveRangeAsync(item, Document.Pardakht, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 128);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtNaghdRepo.RemoveAsync(item, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 129);
				if (((IQueryable<PardakhtCheckSh>)dbcontext.PardakhtCheckSh).Where((PardakhtCheckSh p) => p.MasterGuid == item).ToList()?.Any() ?? false)
				{
					pardakht.Sum = 0m;
					pardakht.SumBank = 0m;
					pardakht.SumCheckM = 0m;
					pardakht.SumCheckSh = 0m;
					pardakht.SumEnteghal = 0m;
					pardakht.SumNaghd = 0m;
					pardakht.Status = false;
					((DbContext)dbcontext).Update<Pardakht>(pardakht);
				}
				else
				{
					dbcontext.Pardakht.Remove(pardakht);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 150);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 151);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<PardakhtReportBusiness>> GetAllAsync(KhazaneFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 158));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			IEnumerable<PardakhtReportBusiness> result;
			try
			{
				_ = 1;
				try
				{
					filter.Description = filter.Description?.FixString()?.RemoveCharactersForLinkSearch() ?? "";
					IQueryable<Pardakht> queryable = (IQueryable<Pardakht>)EntityFrameworkQueryableExtensions.Include<Pardakht, Cashier>((IQueryable<Pardakht>)EntityFrameworkQueryableExtensions.Include<Pardakht, Tafsil>((IQueryable<Pardakht>)c.Pardakht, (Expression<Func<Pardakht, Tafsil>>)((Pardakht q) => q.TafsilItem)), (Expression<Func<Pardakht, Cashier>>)((Pardakht q) => q.CashierItem));
					if (filter.Id2?.Any() ?? false)
					{
						queryable = queryable.Where((Pardakht p) => filter.Id2.Contains(p.Id2));
					}
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((Pardakht q) => q.Date >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((Pardakht q) => q.Date <= filter.DateTo);
					}
					if (filter.SumFrom > 0m)
					{
						queryable = queryable?.Where((Pardakht q) => q.Sum >= filter.SumFrom);
					}
					if (filter.SumTo > 0m && filter.SumTo < 999999999999m)
					{
						queryable = queryable?.Where((Pardakht q) => q.Sum <= filter.SumTo);
					}
					foreach (string word in filter.Description?.SplitString()?.Distinct() ?? new List<string>())
					{
						if (!string.IsNullOrEmpty(word))
						{
							queryable = queryable.Where((Pardakht p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%"));
						}
					}
					if (filter.IsSent.HasValue)
					{
						queryable = queryable?.Where((Pardakht q) => q.HostRefrenced == "");
					}
					if (filter.IsDelivered.HasValue)
					{
						queryable = queryable?.Where((Pardakht q) => q.DeliveryId > 0);
					}
					if (filter.IsTemp.HasValue)
					{
						queryable = queryable?.Where((Pardakht q) => q.IsTemp);
					}
					if (filter.Status.HasValue)
					{
						queryable = queryable?.Where((Pardakht q) => (bool?)q.Status == filter.Status);
					}
					if (filter.Id2From != 0)
					{
						queryable = queryable?.Where((Pardakht q) => q.Id2 >= filter.Id2From);
					}
					if (filter.Id2To != 0)
					{
						queryable = queryable?.Where((Pardakht q) => q.Id2 <= filter.Id2To);
					}
					if ((filter.TafsilGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((Pardakht q) => q.TafsilGuid == filter.TafsilGuid.Value);
					}
					if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((Pardakht q) => q.UserGuid == filter.CashierGuid.Value);
					}
					if (filter.HasSumNaghd.HasValue)
					{
						queryable = queryable?.Where((Pardakht q) => q.SumNaghd > 0m);
					}
					queryable = queryable?.Where((Pardakht q) => q.Sum != 0m || q.SumBank != 0m || q.SumCheckM != 0m || q.SumCheckSh != 0m || q.SumEnteghal != 0m || q.SumNaghd != 0m);
					if (filter.HesabType.GetValueOrDefault(HesabType.None) != HesabType.None && filter.HesabType.GetValueOrDefault(HesabType.None) != 0)
					{
						queryable = queryable?.Where((Pardakht q) => q.TafsilItem.HType == filter.HesabType.Value);
					}
					if (filter.CustomerGroupGuid.HasValue && (filter.CustomerGroupGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = (IQueryable<Pardakht>)EntityFrameworkQueryableExtensions.ThenInclude<Pardakht, Tafsil, Customer>(EntityFrameworkQueryableExtensions.Include<Pardakht, Tafsil>(queryable, (Expression<Func<Pardakht, Tafsil>>)((Pardakht pardakht) => pardakht.TafsilItem)), (Expression<Func<Tafsil, Customer>>)((Tafsil tafsil) => tafsil.Customer));
						queryable = queryable.Where((Pardakht p) => p.TafsilItem.Customer.GroupGuid == filter.CustomerGroupGuid.Value);
					}
					if (filter.Sources != null && (filter.Sources?.Count ?? 0) > 0)
					{
						Expression<Func<Pardakht, bool>> expression = PredicateBuilder.False<Pardakht>();
						foreach (ENSource item in filter.Sources)
						{
							expression = expression.Or((Pardakht x) => x.Source == item);
						}
						queryable = queryable?.Where(expression);
					}
					if ((filter?.ChequeMGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((Pardakht q) => q.PardakhtCheckMItem.Select((PardakhtCheckM w) => w.GuidCheck).Contains(filter.ChequeMGuid.Value));
					}
					if (filter?.NaghdFrom.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumNaghd >= filter.NaghdFrom.Value);
					}
					if (filter?.NaghdTo.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumNaghd <= filter.NaghdTo.Value);
					}
					if (filter?.HavaleFrom.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumBank >= filter.HavaleFrom.Value);
					}
					if (filter?.HavaleTo.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumBank <= filter.HavaleTo.Value);
					}
					if (filter?.ChequeFrom.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumCheckSh >= filter.ChequeFrom.Value);
					}
					if (filter?.ChequeTo.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumCheckSh <= filter.ChequeTo.Value);
					}
					if (filter?.ChequeMFrom.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumCheckM >= filter.ChequeMFrom.Value);
					}
					if (filter?.ChequeMTo.HasValue ?? false)
					{
						queryable = queryable.Where((Pardakht q) => q.SumCheckM <= filter.ChequeMTo.Value);
					}
					if ((filter?.StatusId).GetValueOrDefault() > 0)
					{
						queryable = queryable?.Where((Pardakht q) => q.StatusId == filter.StatusId.Value);
					}
					if ((filter?.ServerStatuses?.Any((ServerStatusForClient p) => p != ServerStatusForClient.None)).GetValueOrDefault())
					{
						queryable = queryable?.Where((Pardakht q) => filter.ServerStatuses.Contains(q.ServerStatus));
					}
					result = await MapFromDatabaseToReport.MapListAsync(ContextGuid, queryable?.ToList(), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 218);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(PardakhtBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		return SaveAsync(item, EnSystem.None, tryCount, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(PardakhtBusiness item, EnSystem system, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 236));
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar && item.ChangeStatus == EntityChangeSt.New && !string.IsNullOrEmpty(item?.MetaData?.Message))
				{
					string jsonMessage = Json.ToStringJson(item.MetaData.Message);
					if (((IQueryable<Pardakht>)dbcontext.Pardakht).FirstOrDefault((Pardakht q) => !string.IsNullOrEmpty(q.MetaData) && q.MetaData.Contains(jsonMessage)) != null)
					{
						return ret;
					}
				}
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				Pardakht pardakht = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(pardakht, pardakht.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 250);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtBankRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 251);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheckMRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 252);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtEnteghalShRepo.RemoveRangeAsync(item.Guid, Document.Pardakht, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 253);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtNaghdRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 254);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtBankRepo.SaveAsync(item.Bank?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 255);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheckMRepo.SaveAsync(item.CheckM?.ToList(), item, system, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 256);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheckShRepo.SaveAsync(item.CheckSh?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 257);
				if (item.Enteghal?.Any() ?? false)
				{
					List<Sanad_DetBusiness> lstEnteghatNew = new List<Sanad_DetBusiness>();
					foreach (Sanad_DetBusiness sanadDetBusiness in item.Enteghal)
					{
						if (sanadDetBusiness.AsType != Document.Pardakht)
						{
							throw new Exception("asType is nul equal Pardakht");
						}
						if (sanadDetBusiness.AsGuid != item.Guid)
						{
							throw new Exception($"asGuid({item.Guid}) is nul equal Pardakht({item.Guid})");
						}
						lstEnteghatNew.Add(sanadDetBusiness);
						Sanad_DetBusiness temp = new Sanad_DetBusiness(ContextGuid, EnSystem.None, item.Guid, Document.Pardakht, item.NextOrdinal(token));
						await SanadDetMapper.Instance.MapAsync(ContextGuid, sanadDetBusiness, temp, tranName, token);
						temp.Guid = Guid.NewGuid();
						temp.Debit = sanadDetBusiness.Credit;
						temp.Credit = sanadDetBusiness.Debit;
						await temp.SetMoeinGuidAsync(item.MoeinGuid, token, tranName);
						await temp.SetTafsilGuidAsync(item.TafsilGuid, tranName, token);
						if (temp.MoeinGuid == sanadDetBusiness.MoeinGuid && (temp.TafsilGuid ?? Guid.Empty) == (sanadDetBusiness.TafsilGuid ?? Guid.Empty))
						{
							throw new ArgumentException("طرف حساب انتقال با طرف حساب برگه پرداخت یکسان است.".GetTranslate());
						}
						lstEnteghatNew.Add(temp);
					}
					List<Sanad_DetBusiness> list = await pardakhtEnteghalShRepo.GetAllAsync(item.Guid, Document.Pardakht, null, null, token, tranName);
					if (list != null && list.Any())
					{
						list = list.Where((Sanad_DetBusiness p) => !lstEnteghatNew.Select((Sanad_DetBusiness p) => p.Guid).Contains(p.Guid)).ToList();
					}
					if (list != null && list.Any())
					{
						foreach (Sanad_DetBusiness item2 in list)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await pardakhtEnteghalShRepo.RemoveRangeAsync(item2.Guid, Document.Pardakht, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 286);
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await pardakhtEnteghalShRepo.SaveRangeAsync(lstEnteghatNew, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 287);
				}
				if (item.Naghd?.Any() ?? false)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await pardakhtNaghdRepo.SaveAsync(item.Naghd, item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 291);
				}
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
				WebErrorLog.Instance.StartLog(ex3, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 298);
				throw;
			}
			return ret;
		}
	}

	public async Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 305));
		using (new JaegerMonitor("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			Guid result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Pardakht>((IQueryable<Pardakht>)c.Pardakht, (Expression<Func<Pardakht, bool>>)((Pardakht x) => x.Id2 == id2), token))?.Guid ?? Guid.Empty;
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
					WebErrorLog.Instance.StartLog(ex3, "", "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 317);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 328));
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			int result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					if (statusId.GetValueOrDefault() <= 0)
					{
						goto IL_02ea;
					}
					SanadStatusReport status = await c.SanadStatusReport.FindAsync(new object[1] { statusId.Value });
					if ((status?.Id2From).GetValueOrDefault() <= 0)
					{
						goto IL_02ea;
					}
					List<int> list = await EntityFrameworkQueryableExtensions.ToListAsync<int>(((IQueryable<Pardakht>)c.Pardakht).Select((Pardakht q) => q.Id2), token);
					long num2 = status.Id2From.Value;
					while (true)
					{
						if (num2 < status.Id2To.Value)
						{
							if (!list.Contains((int)num2))
							{
								result = (int)num2;
								break;
							}
							num2++;
							continue;
						}
						result = (int)status.Id2From.Value;
						break;
					}
					goto end_IL_00f6;
					IL_02ea:
					IQueryable<Pardakht> queryable = ((IQueryable<Pardakht>)c.Pardakht).OrderByDescending((Pardakht x) => x.Id2).Take(1);
					Pardakht pardakht = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Pardakht>(queryable, token) : null);
					result = ((pardakht == null) ? 1 : (pardakht.Id2 + 1));
					end_IL_00f6:;
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
					WebErrorLog.Instance.StartLog(ex3, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 354);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<PardakhtBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 365));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			PardakhtBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 6;
				try
				{
					Pardakht res = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Pardakht>((IQueryable<Pardakht>)c.Pardakht, (Expression<Func<Pardakht, bool>>)((Pardakht x) => x.Guid == guid), token);
					if (res == null)
					{
						result = null;
					}
					else
					{
						PardakhtBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, res, null, tranName, token);
						ret.ChangeStatus = EntityChangeSt.Edited;
						PardakhtBusiness pardakhtBusiness = ret;
						pardakhtBusiness.AddRangeToBankList(await pardakhtBankRepo.GetAllAsync(ret.Guid, token, tranName), clearList: true, EnSystem.Mapper, token);
						pardakhtBusiness = ret;
						pardakhtBusiness.AddRangeToChequeMList(await pardakhtCheckMRepo.GetAllAsync(ret.Guid, token, tranName), clearList: true, EnSystem.Mapper, token);
						List<PardakhtCheckShBusiness> list = (await pardakhtCheckShRepo.GetAllAsync(ret.Guid, token, tranName))?.Where((PardakhtCheckShBusiness q) => q.Status != ENCheckSHStatus.BatelShode)?.ToList();
						if (list != null && list.Any())
						{
							ret.AddRangeToChequeShList(list, clearList: true, EnSystem.Mapper, token);
						}
						pardakhtBusiness = ret;
						pardakhtBusiness.AddRangeToNaghdList(await pardakhtNaghdRepo.GetAllAsync(ret.Guid, token, tranName), clearList: true, EnSystem.Mapper, token);
						List<Sanad_DetBusiness> list2 = (await pardakhtEnteghalShRepo.GetAllAsync(ret.Guid, Document.Pardakht, null, null, token, tranName))?.Where((Sanad_DetBusiness p) => (p.TafsilGuid ?? Guid.Empty) != res.TafsilGuid || p.MoeinGuid != res.MoeinGuid)?.ToList();
						ret.AddRangeToEnteghalList(list2, clearList: true, EnSystem.Mapper, token);
						ret.ComputeOrdinal(EnSystem.Mapper, token);
						result = ret;
					}
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 391);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext contex = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 408));
				IQueryable<Pardakht> queryable = ((IQueryable<Pardakht>)contex.Pardakht).Where((Pardakht x) => x.Guid == item.Guid);
				Pardakht pardakht = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Pardakht>(queryable, token) : null);
				if (pardakht == null)
				{
					return null;
				}
				pardakht.HostRefrenced = item.HostRefrenced;
				pardakht.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				pardakht.DeliveryMessage = item.Message;
				pardakht.DeliveryShortMessage = item.Shortmessage;
				pardakht.IsTemp = !item.SaveResultStatus;
				((DbContext)contex).Update<Pardakht>(pardakht);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 424);
				ret.AddReturnedValue(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 425);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext contex = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 437));
				IQueryable<Pardakht> queryable = ((IQueryable<Pardakht>)contex.Pardakht).Where((Pardakht x) => x.Guid == del.Guid);
				Pardakht pardakht = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Pardakht>(queryable, token) : null);
				if (pardakht == null)
				{
					return ret.AddError($"pardakht with guid({del.Guid}) not exists", "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 439);
				}
				if (del.DeliveryStatus)
				{
					pardakht.ServerStatus = (string.IsNullOrEmpty(pardakht.HostRefrenced) ? ServerStatusForClient.DirectDelivery : ServerStatusForClient.Delivered);
				}
				else
				{
					pardakht.ServerStatus = ServerStatusForClient.DeliveryError;
				}
				pardakht.DeliveryDate = DateTime.Now;
				pardakht.DeliveryId = del.DeliveryId;
				pardakht.DeliveryMessage = del.Message;
				pardakht.DeliveryShortMessage = del.ShortMessage;
				if (!del.DeliveryStatus)
				{
					pardakht.IsTemp = true;
				}
				((DbContext)contex).Update<Pardakht>(pardakht);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 457);
				ret.AddReturnedValue(ex2, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 458);
			}
			return ret;
		}
	}

	public async Task<List<PardakhtBusiness>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		int line = 0;
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 466));
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			List<PardakhtBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<PardakhtBusiness> ret = new List<PardakhtBusiness>();
					line = 1;
					List<Pardakht> list = await EntityFrameworkQueryableExtensions.ToListAsync<Pardakht>(((IQueryable<Pardakht>)c.Pardakht).Where((Pardakht e) => e.ServerStatus == ServerStatusForClient.None && !e.IsTemp), default(CancellationToken));
					line = 2;
					if (list != null && list.Count > 0)
					{
						foreach (Pardakht item in list)
						{
							if (item != null)
							{
								List<PardakhtBusiness> list2 = ret;
								list2.Add(await GetAsync(item.Guid, token, ""));
							}
						}
					}
					line = 3;
					result = ret;
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
					WebErrorLog.Instance.StartLog(ex3, $"Error Line {line}", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 487);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<PardakhtReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 498));
		using (new JaegerMonitor("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			PardakhtReportBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (token.IsCancellationRequested || _isDisposed)
					{
						result = null;
					}
					else
					{
						Pardakht pardakht = await c.Pardakht.FindAsync(new object[1] { guid });
						result = ((pardakht != null) ? (await MapFromDatabaseToReport.MapAsync(ContextGuid, pardakht, null, tranName, token)) : null);
					}
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 513);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public Task Add2CacheAsync(PardakhtBusiness item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 528));
		using (new JaegerMonitor("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			List<Guid> result;
			try
			{
				IQueryable<Pardakht> pardakht = (IQueryable<Pardakht>)acc_ModelContext.Pardakht;
				result = pardakht.Where((Pardakht p) => p.TransferDate == null)?.Select((Pardakht q) => q.Guid)?.ToList();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 540);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 557));
				Order order = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>(((IQueryable<Order>)context.Order).Where((Order x) => x.Guid == filter.ObjectGuid), token);
				order.TransferDate = filter.TransferDate;
				((DbContext)context).Update<Order>(order);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 565);
				ret.AddReturnedValue(ex2, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 566);
			}
			return ret;
		}
	}

	public Task<PardakhtBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 575));
		List<ENSource> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ENSource> queryable = ((IQueryable<Pardakht>)c.Pardakht).Select((Pardakht q) => q.Source);
				object obj;
				if (queryable == null)
				{
					obj = null;
				}
				else
				{
					IQueryable<ENSource> queryable2 = queryable.Distinct();
					obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ENSource>(queryable2, token) : null);
				}
				result = await (Task<List<ENSource>>)obj;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 583);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public void ResetCache()
	{
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 600));
				Pardakht pardakht = await context.Pardakht.FindAsync(new object[1] { guid });
				if (pardakht == null)
				{
					return res;
				}
				pardakht.StatusId = (short)statusId;
				pardakht.Modified = DateTime.Now;
				pardakht.ServerStatus = ServerStatusForClient.None;
				((DbContext)context).Update<Pardakht>(pardakht);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 611);
				res.AddReturnedValue(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtPersistenceRepositoryBase.cs", 612);
			}
			return res;
		}
	}
}
