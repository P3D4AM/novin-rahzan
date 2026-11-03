using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;
using EntityCache;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Core.Repository.Accounting.Details;
using EntityCache.Mapping.BusinessMappers.Sanad;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories;

public abstract class ReceptionPersistenceRepositoryBase : IReceptionRepository, IEntityRepository<ReceptionBusiness, ReceptionReportBusiness>, IEditableDelivery, IDisposable
{
	private class mapper : IItemAsyncMapper<ReceptionBusiness, Reception>, IItemAsyncMapper<Reception, ReceptionBusiness>, IItemAsyncMapper<Reception, ReceptionReportBusiness>
	{
		private IReceptionCheckRepository _receptionCheckRepo;

		private SanadDetPersistenceRepository _receptionEnteghalRepo;

		private ReceptionHavalePersistenceRepository _receptionHavaleRepo;

		private ReceptionNaghdPersistenceRepository _receptionNaghRepo;

		private mapper()
		{
		}

		public mapper(IReceptionCheckRepository receptionCheckRepo, SanadDetPersistenceRepository receptionEnteghalRepo, ReceptionHavalePersistenceRepository receptionHavaleRepo, ReceptionNaghdPersistenceRepository receptionNaghRepo)
		{
			_receptionCheckRepo = receptionCheckRepo;
			_receptionEnteghalRepo = receptionEnteghalRepo;
			_receptionHavaleRepo = receptionHavaleRepo;
			_receptionNaghRepo = receptionNaghRepo;
		}

		public async Task<Reception> MapAsync(string contextGuid, ReceptionBusiness source, Reception destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
			{
				_ = 7;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new Reception();
					}
					destination.Guid = source.Guid;
					destination.DeliveryStatus = source.DeliveryStatus;
					destination.IsChecked = source.IsChecked;
					destination.Source = source.Source;
					destination.Modified = source.Modified;
					destination.IsTemp = source.IsTemp;
					destination.AndroidIme = source.AndroidIme;
					destination.CpuSerial = source.CpuSerial;
					destination.Date = source.Date;
					destination.DateSabt = source.DateSabt;
					destination.Description = source.Description;
					destination.HddSerial = source.HddSerial;
					Reception reception = destination;
					reception.Id2 = await source.GetId2Async(token, tranName);
					reception = destination;
					reception.FullName = (await source.GetTafsilAsync(EnSystem.Mapper, token, tranName))?.Name ?? "";
					destination.TafsilGuid = source.TafsilGuid;
					destination.UserGuid = source.UserGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.Ip = source.Ip;
					destination.LkSerial = source.LkSerial;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.Sum = source.Sum;
					destination.SumNaghd = source.SumNaghd;
					destination.SumBank = source.SumBank;
					destination.SumCheck = source.SumCheck;
					destination.SumEnteghal = source.SumEnteghal;
					if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
					{
						reception = destination;
						reception.AfterAccount = await source.GetAfterAccountAsync(EnSystem.Mapper, token, tranName);
						reception = destination;
						reception.BeforeAccount = await source.GetBeforeAccountAsync(EnSystem.Mapper, token, tranName);
					}
					destination.Status = source.Status;
					destination.DependencyType = source.DependencyType;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.PaymentMethod = source.PaymentMethod;
					destination.TransferDate = source.TransferDate;
					destination.StatusId = source.StatusId;
					destination.OrderGuid = source.OrderGuid;
					destination.OrderType = source.OrderType;
					destination.DateMoaser = source.DateMoaser;
					if (source.MetaData != null)
					{
						destination.MetaData = Json.ToStringJson(source.MetaData);
					}
					reception = destination;
					reception.ListCheck = await ((ReceptionCheckMasterPersistenceRepository)_receptionCheckRepo).MapToDatabase.MapListAsync(contextGuid, source.ListCheck, destination.ListCheck.ToList(), tranName, token);
					reception = destination;
					reception.ListNaghd = await _receptionNaghRepo.MapToDatabase.MapListAsync(contextGuid, source.ListNaghd, destination.ListNaghd.ToList(), tranName, token);
					reception = destination;
					reception.ListBank = await _receptionHavaleRepo.MapToDatabase.MapListAsync(contextGuid, source.ListBank, destination.ListBank.ToList(), tranName, token);
					reception = destination;
					reception.ListEnteghal = await _receptionEnteghalRepo.MapToDatabase.MapListAsync(contextGuid, source.ListEnteghal, destination.ListEnteghal?.ToList(), tranName, token);
					destination.DateUtc = destination.Date.ToUniversalTime();
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 789);
					throw;
				}
			}
		}

		public async Task<ReceptionBusiness> MapAsync(string contextGuid, Reception source, ReceptionBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.DeliveryStatus = source.DeliveryStatus;
				destination.IsChecked = source.IsChecked;
				destination.Source = source.Source;
				destination.Modified = source.Modified;
				destination.IsTemp = source.IsTemp;
				destination.AndroidIme = source.AndroidIme;
				destination.CpuSerial = source.CpuSerial;
				destination.Date = source.Date;
				destination.DateSabt = source.DateSabt;
				destination.Description = source.Description;
				destination.HddSerial = source.HddSerial;
				destination.SetId2(source.Id2);
				await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper, token, tranName);
				destination.UserGuid = source.UserGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.Ip = source.Ip;
				destination.LkSerial = source.LkSerial;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
				{
					destination.SetBeforeAccount(source.BeforeAccount);
				}
				destination.SetAfterAccount(source.AfterAccount);
				destination.Status = source.Status;
				destination.DependencyType = source.DependencyType;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.PaymentMethod = source.PaymentMethod;
				destination.DateMoaser = source.DateMoaser;
				destination.TransferDate = source.TransferDate;
				destination.StatusId = source.StatusId;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<ReceptionMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 838);
				}
				destination.DateUtc = source.DateUtc;
				destination.OrderGuid = source.OrderGuid;
				destination.OrderType = source.OrderType;
				destination.TafsilGuid2 = source.TafsilGuid2;
				destination.TafsilGuid3 = source.TafsilGuid3;
				destination.AddRangeToDetList(await ((ReceptionCheckMasterPersistenceRepository)_receptionCheckRepo).MapFromDatabase.MapListAsync(contextGuid, source.ListCheck, destination?.ListCheck.ToList(), tranName, token), EnSystem.Mapper);
				destination.AddRangeToDetList(await _receptionNaghRepo.MapFromDatabase.MapListAsync(contextGuid, source.ListNaghd, destination?.ListNaghd.ToList(), tranName, token), EnSystem.Mapper);
				destination.AddRangeToDetList(await _receptionHavaleRepo.MapFromDatabase.MapListAsync(contextGuid, source.ListBank, destination?.ListBank.ToList(), tranName, token), EnSystem.Mapper);
				destination.AddRangeToDetList(await _receptionEnteghalRepo.MapFromDatabase.MapListAsync(contextGuid, source.ListEnteghal, destination?.ListEnteghal.ToList(), tranName, token), EnSystem.Mapper);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 863);
				throw;
			}
		}

		public Task<List<Reception>> MapListAsync(string contextGuid, IEnumerable<ReceptionBusiness> source, List<Reception> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReceptionBusiness>> MapListAsync(string contextGuid, IEnumerable<Reception> source, List<ReceptionBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<ReceptionReportBusiness> MapAsync(string contextGuid, Reception source, ReceptionReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionReportBusiness(contextGuid);
				}
				destination.DeliveryStatus = source.DeliveryStatus;
				destination.IsChecked = source.IsChecked;
				destination.Source = source.Source;
				destination.AndroidIme = source.AndroidIme;
				destination.CpuSerial = source.CpuSerial;
				destination.DateM = source.Date;
				destination.DateSabt = source.DateSabt;
				destination.Description = source.Description;
				destination.HddSerial = source.HddSerial;
				destination.Id2 = source.Id2;
				destination.TafsilGuid = source.TafsilGuid;
				destination.CashierGuid = source.UserGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.Ip = source.Ip;
				destination.LkSerial = source.LkSerial;
				destination.HostRefrenced = source.HostRefrenced;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				destination.SumNaghd = source.SumNaghd;
				destination.SumHavale = source.SumBank;
				destination.SumChequeSh = source.SumCheck;
				destination.SumEnteghal = source.SumEnteghal;
				destination.AfterAccount = source.AfterAccount;
				destination.BeforeAccount = source.BeforeAccount;
				destination.Status = source.Status;
				destination.DependencyType = source.DependencyType;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.DeliveryDate = source.DeliveryDate;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.IsTemp = source.IsTemp;
				destination.ServerStatus = source.ServerStatus;
				destination.PaymentMethod = source.PaymentMethod;
				destination.TransferDate = source.TransferDate;
				destination.StatusId = source.StatusId;
				destination.SumBankPos = (source.ReceptionHavaleItem?.Where((ReceptionHavale q) => q.Type == ENHavaleType.POS)?.Sum((ReceptionHavale q) => q.Price)).GetValueOrDefault();
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 927);
				throw;
			}
		}

		public Task<List<ReceptionReportBusiness>> MapListAsync(string contextGuid, IEnumerable<Reception> source, List<ReceptionReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper _mapper;

	private bool _isDisposed;

	protected ContextHolder dbContext;

	private IReceptionCheckRepository _receptionCheckRepo;

	private SanadDetPersistenceRepository _receptionEnteghalRepo;

	private ReceptionHavalePersistenceRepository _receptionHavaleRepo;

	private ReceptionNaghdPersistenceRepository _receptionNaghRepo;

	private ITableRelationShipRepository _relationShipRepo;

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => dbContext.Context.Guid;

	public IItemAsyncMapper<Reception, ReceptionBusiness> MapFromDatabase => _mapper;

	public IItemAsyncMapper<ReceptionBusiness, Reception> MapToDatabase => _mapper;

	public IItemAsyncMapper<Reception, ReceptionReportBusiness> MapFromDatabaseToReport => _mapper;

	public ReceptionPersistenceRepositoryBase(ContextHolder dbContext, IReceptionCheckRepository recCheque, ITableRelationShipRepository attachmentRepo)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			try
			{
				this.dbContext = dbContext;
				_receptionCheckRepo = recCheque;
				_receptionEnteghalRepo = new SanadDetPersistenceRepository(dbContext);
				_receptionHavaleRepo = new ReceptionHavalePersistenceRepository(dbContext);
				_receptionNaghRepo = new ReceptionNaghdPersistenceRepository(dbContext);
				_relationShipRepo = attachmentRepo;
				_mapper = new mapper(_receptionCheckRepo, _receptionEnteghalRepo, _receptionHavaleRepo, _receptionNaghRepo);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 60);
			}
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new RowNotInTableException();
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 71));
				Reception reception = await dbcontext.Receptions.FindAsync(new object[1] { guid });
				reception.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(reception, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 74);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 79);
				ret.AddReturnedValue(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 80);
			}
			return ret;
		}
	}

	public async Task<ReceptionBusiness> GetAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 86));
		int line = 0;
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReceptionBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 7;
				try
				{
					Reception res = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Reception>((IQueryable<Reception>)c.Receptions, (Expression<Func<Reception, bool>>)((Reception x) => x.Guid == guid), token);
					if (res == null)
					{
						result = null;
					}
					else
					{
						ReceptionBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, res, new ReceptionBusiness(ContextGuid, order), tranName, token);
						line = 1;
						ret.ChangeStatus = EntityChangeSt.Edited;
						List<ReceptionCheckBusiness> list = await _receptionCheckRepo.GetAllAsync(ret.Guid, token, tranName);
						if (system == EnSystem.UserInterface)
						{
							list = list?.Where((ReceptionCheckBusiness q) => q.Status != ENUMCheck.BatelShode)?.ToList();
						}
						ret.AddRangeToDetList(list, system);
						line = 2;
						ReceptionBusiness receptionBusiness = ret;
						receptionBusiness.AddRangeToDetList(await _receptionNaghRepo.GetAllAsync(ret.Guid, token, tranName), system);
						line = 3;
						receptionBusiness = ret;
						receptionBusiness.AddRangeToDetList(await _receptionHavaleRepo.GetAllAsync(ret.Guid, token, tranName), system);
						line = 4;
						List<Sanad_DetBusiness> detList = (await _receptionEnteghalRepo.GetAllAsync(ret.Guid, Document.Reception, null, null, token, tranName))?.Where((Sanad_DetBusiness p) => (p.TafsilGuid ?? Guid.Empty) != res.TafsilGuid || p.MoeinGuid != res.MoeinGuid)?.ToList();
						ret.AddRangeToDetList(detList, system);
						IEnumerable<TableRelationShipBusiness> enumerable = await _relationShipRepo.GetAllRelatedSanadAsync(guid, Document.Reception, token, tranName);
						if (enumerable != null && enumerable.Any())
						{
							await ret.AddListToLinkedSanadsAsync(enumerable);
						}
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
					WebErrorLog.Instance.StartLog(ex3, $"The Error Line Is : {line}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 121);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 132));
		using (new JaegerMonitor("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			List<Guid> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Reception> receptions = (IQueryable<Reception>)c.Receptions;
					receptions = receptions.Where((Reception p) => p.TransferDate == null);
					result = ((receptions != null && receptions.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<Guid>(receptions.Select((Reception q) => q.Guid), token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 145);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
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
		using (new JaegerMonitor("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 158));
				IQueryable<Order> queryable = ((IQueryable<Order>)context.Order).Where((Order x) => x.Guid == filter.ObjectGuid);
				Order order = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>(queryable, token) : null);
				order.TransferDate = filter.TransferDate;
				((DbContext)context).Update<Order>(order);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 166);
				ret.AddReturnedValue(ex2, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 167);
			}
			return ret;
		}
	}

	public async Task<List<ReceptionReportBusiness>> GetAllAsync(KhazaneFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 173));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			List<ReceptionReportBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					filter.Description = filter.Description?.FixString()?.RemoveCharactersForLinkSearch() ?? "";
					IQueryable<Reception> source = (IQueryable<Reception>)EntityFrameworkQueryableExtensions.Include<Reception, Cashier>((IQueryable<Reception>)EntityFrameworkQueryableExtensions.Include<Reception, ICollection<ReceptionHavale>>((IQueryable<Reception>)EntityFrameworkQueryableExtensions.Include<Reception, Tafsil>((IQueryable<Reception>)c.Receptions, (Expression<Func<Reception, Tafsil>>)((Reception q) => q.TafsilItem)), (Expression<Func<Reception, ICollection<ReceptionHavale>>>)((Reception q) => q.ReceptionHavaleItem)), (Expression<Func<Reception, Cashier>>)((Reception q) => q.CashierItem));
					if (filter.Id2?.Any() ?? false)
					{
						source = source.Where((Reception p) => filter.Id2.Contains(p.Id2));
					}
					if (filter.Type.HasValue)
					{
						source = source.Where((Reception q) => (int?)q.DependencyType == (int?)filter.Type);
					}
					if (filter.SumFrom > 0m)
					{
						source = source.Where((Reception q) => q.Sum >= filter.SumFrom);
					}
					if (filter.SumTo > 0m)
					{
						source = source.Where((Reception q) => q.Sum <= filter.SumTo);
					}
					if (filter.Id2From > 0)
					{
						source = source.Where((Reception q) => q.Id2 >= filter.Id2From);
					}
					if (filter.Id2To > 0)
					{
						source = source.Where((Reception q) => q.Id2 <= filter.Id2To);
					}
					if (filter.DateFrom.HasValue)
					{
						source = source.Where((Reception q) => q.Date >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						source = source.Where((Reception q) => q.Date <= filter.DateTo);
					}
					foreach (string word in filter.Description?.SplitString()?.Distinct() ?? new List<string>())
					{
						if (!string.IsNullOrEmpty(word))
						{
							source = source.Where((Reception p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%"));
						}
					}
					if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
					{
						source = source.Where((Reception q) => q.TafsilGuid == filter.TafsilGuid);
					}
					if (filter.Sources != null && (filter.Sources?.Count ?? 0) > 0)
					{
						Expression<Func<Reception, bool>> expression = PredicateBuilder.False<Reception>();
						foreach (ENSource item in filter.Sources)
						{
							expression = expression.Or((Reception x) => x.Source == item);
						}
						source = source.Where(expression);
					}
					if (filter.IsSent.HasValue && filter.IsSent.GetValueOrDefault() && !string.IsNullOrEmpty(filter.HostRefrences))
					{
						source = source.Where((Reception q) => q.HostRefrenced == filter.HostRefrences);
					}
					if (filter.IsTemp.HasValue)
					{
						source = source.Where((Reception q) => (bool?)q.IsTemp == filter.IsTemp);
					}
					if (!string.IsNullOrEmpty(filter.CustomerFullName))
					{
						source = source.Where((Reception q) => q.FullName.Contains(filter.CustomerFullName));
					}
					if (filter.Id2 != null && filter.Id2.Count > 0)
					{
						source = source.Where((Reception q) => filter.Id2.Contains(q.Id2));
					}
					if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						source = source.Where((Reception q) => q.UserGuid == filter.CashierGuid.Value);
					}
					source = source.Where((Reception q) => q.Sum != 0m || q.SumBank != 0m || q.SumCheck != 0m || q.SumEnteghal != 0m || q.SumNaghd != 0m);
					if ((filter.CustomerGroupGuid ?? Guid.Empty) != Guid.Empty)
					{
						source = (IQueryable<Reception>)EntityFrameworkQueryableExtensions.ThenInclude<Reception, Tafsil, Customer>(EntityFrameworkQueryableExtensions.Include<Reception, Tafsil>(source, (Expression<Func<Reception, Tafsil>>)((Reception reception) => reception.TafsilItem)), (Expression<Func<Tafsil, Customer>>)((Tafsil tafsil) => tafsil.Customer));
						source = source.Where((Reception p) => p.TafsilItem.Customer.GroupGuid == filter.CustomerGroupGuid.Value);
					}
					if (filter?.NaghdFrom.HasValue ?? false)
					{
						source = source.Where((Reception q) => q.SumNaghd >= filter.NaghdFrom.Value);
					}
					if (filter?.NaghdTo.HasValue ?? false)
					{
						source = source.Where((Reception q) => q.SumNaghd <= filter.NaghdTo.Value);
					}
					if (filter?.HavaleFrom.HasValue ?? false)
					{
						source = source.Where((Reception q) => q.SumBank >= filter.HavaleFrom.Value);
					}
					if (filter?.HavaleTo.HasValue ?? false)
					{
						source = source.Where((Reception q) => q.SumBank <= filter.HavaleTo.Value);
					}
					if (filter?.ChequeFrom.HasValue ?? false)
					{
						source = source.Where((Reception q) => q.SumCheck >= filter.ChequeFrom.Value);
					}
					if (filter?.ChequeTo.HasValue ?? false)
					{
						source = source.Where((Reception q) => q.SumCheck <= filter.ChequeTo.Value);
					}
					if ((filter?.StatusId).GetValueOrDefault() > 0)
					{
						source = source.Where((Reception q) => q.StatusId == filter.StatusId.Value);
					}
					if ((filter?.ServerStatuses?.Any((ServerStatusForClient p) => p != ServerStatusForClient.None)).GetValueOrDefault())
					{
						source = source?.Where((Reception q) => filter.ServerStatuses.Contains(q.ServerStatus));
					}
					if (source == null || !source.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<Reception, ReceptionReportBusiness> mapFromDatabaseToReport = MapFromDatabaseToReport;
						string contextGuid = ContextGuid;
						result = await mapFromDatabaseToReport.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<Reception>(source, default(CancellationToken)), new List<ReceptionReportBusiness>(), tranName, token);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 234);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(ReceptionBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 251));
				Reception reception = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				dbcontext.Receptions.Remove(reception);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 257);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 258);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(ReceptionBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				Reception res = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 272));
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar && item.ChangeStatus == EntityChangeSt.New && !string.IsNullOrEmpty(item?.MetaData?.Message))
				{
					string jsonMessage = Json.ToStringJson(item.MetaData.Message);
					if (((IQueryable<Reception>)dbcontext.Receptions).FirstOrDefault((Reception q) => !string.IsNullOrEmpty(q.MetaData) && q.MetaData.Contains(jsonMessage)) != null)
					{
						return ret;
					}
				}
				if (res.OrderGuid == Guid.Empty)
				{
					res.OrderGuid = null;
					res.OrderType = null;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(res, res.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 290);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await RemoveDetAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 292);
				if (item.ListCheck?.Any() ?? false)
				{
					foreach (ReceptionCheckBusiness item2 in item.ListCheck)
					{
						item2.ServerStatus = ServerStatusForClient.None;
						if (item2.DebitMoeinGuid == Guid.Empty)
						{
							item2.DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29976;
						}
						if ((short)item2.Status == 0)
						{
							item2.Status = ENUMCheck.Mojod;
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await (_receptionCheckRepo?.SaveAsync(item.ListCheck.ToList(), item.Guid, tryCount, token, tranName)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 302);
				}
				List<Sanad_DetBusiness> lstEnteghatNew = new List<Sanad_DetBusiness>();
				foreach (Sanad_DetBusiness sanadDetBusiness in item.ListEnteghal ?? new List<Sanad_DetBusiness>())
				{
					if (sanadDetBusiness.AsType != Document.Reception)
					{
						throw new Exception("asType is nul equal recepion");
					}
					if (sanadDetBusiness.AsGuid != item.Guid)
					{
						throw new Exception($"asGuid({item.Guid}) is nul equal recepion({item.Guid})");
					}
					lstEnteghatNew.Add(sanadDetBusiness);
					Sanad_DetBusiness temp = await SanadDetMapper.Instance.MapAsync(ContextGuid, sanadDetBusiness, new Sanad_DetBusiness(ContextGuid, EnSystem.None, item.Guid, Document.Reception, sanadDetBusiness.Ordinal), tranName, token);
					temp.Guid = Guid.NewGuid();
					temp.Debit = sanadDetBusiness.Credit;
					temp.Credit = sanadDetBusiness.Debit;
					await temp.SetMoeinGuidAsync(item.MoeinGuid, token, tranName);
					await temp.SetTafsilGuidAsync(item.TafsilGuid, tranName, token);
					if (temp.MoeinGuid == sanadDetBusiness.MoeinGuid && (temp.TafsilGuid ?? Guid.Empty) == (sanadDetBusiness.TafsilGuid ?? Guid.Empty))
					{
						throw new ArgumentException("طرف حساب انتقال با طرف حساب برگه دریافت یکسان است.".GetTranslate());
					}
					lstEnteghatNew.Add(temp);
				}
				List<Sanad_DetBusiness> list = await _receptionEnteghalRepo.GetAllAsync(item.Guid, Document.Reception, null, null, token, tranName);
				if (list != null && list.Any())
				{
					list = list.Where((Sanad_DetBusiness p) => !lstEnteghatNew.Select((Sanad_DetBusiness p) => p.Guid).Contains(p.Guid)).ToList();
				}
				if (list != null && list.Any())
				{
					foreach (Sanad_DetBusiness item3 in list)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await _receptionEnteghalRepo.RemoveRangeAsync(item3.Guid, Document.Reception, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 330);
					}
				}
				if (lstEnteghatNew.Count > 0)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _receptionEnteghalRepo.SaveRangeAsync(lstEnteghatNew, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 332);
				}
				if ((item?.ListBank?.Any()).GetValueOrDefault())
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _receptionHavaleRepo.SaveAsync(item.ListBank?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 335);
				}
				if ((item?.ListNaghd?.Any()).GetValueOrDefault())
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _receptionNaghRepo.SaveAsync(item.ListNaghd?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 338);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _relationShipRepo.RemoveRangeAsync(item.Guid, Document.Reception, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 340);
				((DbContext)dbcontext).SaveChanges();
				TableRelationShipBusiness tableRelationShipBusiness = await item.GetLinkedSanadAsync(token, tranName);
				if (tableRelationShipBusiness != null)
				{
					tableRelationShipBusiness.SanadGuid = item.Guid;
					tableRelationShipBusiness.SanadType = Document.Reception;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _relationShipRepo.SaveAsync(tableRelationShipBusiness, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 347);
					((DbContext)dbcontext).SaveChanges();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 353);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 354);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 365));
				IQueryable<Reception> queryable = ((IQueryable<Reception>)dbcontext.Receptions).Where((Reception x) => x.Guid == item.Guid);
				Reception reception = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Reception>(queryable, token) : null);
				if (reception == null)
				{
					ret.AddReturnedValue(ReturnedState.Error, "برگه دریافت با شناسه".GetTranslate() + $" Guid:{item.Guid}" + "یافت نشد.".GetTranslate(), "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 369);
					return ret;
				}
				reception.HostRefrenced = item.HostRefrenced;
				reception.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				reception.DeliveryMessage = item.Message;
				reception.DeliveryShortMessage = item.Shortmessage;
				reception.IsTemp = !item.SaveResultStatus;
				((DbContext)dbcontext).Update<Reception>(reception);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 381);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 382);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 393));
				IQueryable<Reception> queryable = ((IQueryable<Reception>)dbcontext.Receptions).Where((Reception x) => x.Guid == item.Guid);
				Reception reception = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Reception>(queryable, token) : null);
				if (reception == null)
				{
					return ret.AddError("دریافتی با شناسه وارد شده یافت نشد.".GetTranslate() + $"\r\nGuid:{item.Guid}", "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 396);
				}
				if (item.DeliveryStatus)
				{
					reception.ServerStatus = (string.IsNullOrEmpty(reception.HostRefrenced) ? ServerStatusForClient.DirectDelivery : ServerStatusForClient.Delivered);
				}
				else
				{
					reception.ServerStatus = ServerStatusForClient.DeliveryError;
				}
				reception.DeliveryDate = item.DeliveryDate;
				reception.DeliveryId = item.DeliveryId;
				reception.DeliveryMessage = item.Message;
				reception.DeliveryShortMessage = item.ShortMessage;
				if (!item.DeliveryStatus)
				{
					reception.IsTemp = true;
				}
				((DbContext)dbcontext).Update<Reception>(reception);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 415);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 416);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAllAsync(short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 427));
				acc_ModelContext.Receptions.RemoveRange((IEnumerable<Reception>)(((IEnumerable<Reception>)acc_ModelContext.Receptions)?.ToList()));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 432);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 433);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> RemoveDetAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _receptionEnteghalRepo.RemoveRangeAsync(guid, Document.Reception, token, tranName), ReturnedState.None, "RemoveDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 443);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _receptionHavaleRepo.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 444);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _receptionNaghRepo.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 445);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _relationShipRepo.RemoveRangeAsync(guid, Document.Reception, token, tranName), ReturnedState.None, "RemoveDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 446);
				return ret;
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
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 452);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 465));
				Reception reception = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Reception>((IQueryable<Reception>)dbcontext.Receptions, (Expression<Func<Reception, bool>>)((Reception w) => w.Guid == guid), token);
				if (reception == null)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await RemoveDetAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 469);
				List<ReceptionCheck> list = ((IQueryable<ReceptionCheck>)dbcontext.ReceptionCheck).Where((ReceptionCheck p) => p.MasterGuid == guid).ToList();
				if (list != null && list.Any())
				{
					foreach (ReceptionCheck item in list)
					{
						if (item.Status == ENUMCheck.Mojod)
						{
							item.Status = ENUMCheck.BatelShode;
							((DbContext)dbcontext).Update<ReceptionCheck>(item);
						}
						else if (item.Status != ENUMCheck.BatelShode)
						{
							throw new Exception("برگه دریافت".GetTranslate() + $" ({reception.Id2})" + " حاوی چک".GetTranslate() + " (" + item.Status.GetDisplay().GetTranslate() + ") " + " قابل ابطال نمیباشد.".GetTranslate());
						}
					}
					reception.Sum = 0m;
					reception.SumCheck = 0m;
					reception.SumBank = 0m;
					reception.SumEnteghal = 0m;
					reception.SumNaghd = 0m;
					reception.Status = false;
					((DbContext)dbcontext).Update<Reception>(reception);
				}
				else
				{
					dbcontext.Receptions.Remove(reception);
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 501);
				throw;
			}
			return ret;
		}
	}

	public async Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 508));
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
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
					List<int> list = await EntityFrameworkQueryableExtensions.ToListAsync<int>(((IQueryable<Reception>)c.Receptions).Select((Reception q) => q.Id2), token);
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
					IQueryable<Reception> queryable = ((IQueryable<Reception>)c.Receptions).OrderByDescending((Reception x) => x.Id2).Take(1);
					Reception reception = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Reception>(queryable, token) : null);
					result = ((reception == null) ? 1 : (reception.Id2 + 1));
					end_IL_00f6:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 533);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 544));
		using (new JaegerMonitor("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			Guid result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Reception> queryable = ((IQueryable<Reception>)c.Receptions).Where((Reception x) => x.Id2 == id2);
					result = (await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Reception>(queryable, token) : null))?.Guid ?? Guid.Empty;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 556);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<ReceptionBusiness>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 566));
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			List<ReceptionBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<ReceptionBusiness> ret = new List<ReceptionBusiness>();
					List<Reception> list = await EntityFrameworkQueryableExtensions.ToListAsync<Reception>(((IQueryable<Reception>)c.Receptions).Where((Reception e) => e.ServerStatus == ServerStatusForClient.None && !e.IsTemp), default(CancellationToken));
					if (list != null && list.Count > 0)
					{
						foreach (Reception item in list)
						{
							if (item != null)
							{
								List<ReceptionBusiness> list2 = ret;
								list2.Add(await GetAsync(item.Guid, null, EnSystem.SignalR, token, ""));
							}
						}
					}
					result = ret;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 582);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReceptionReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 593));
		using (new JaegerMonitor("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReceptionReportBusiness result;
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
						Reception reception = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Reception>((IQueryable<Reception>)c.Receptions, (Expression<Func<Reception, bool>>)((Reception x) => x.Guid == guid), token);
						result = ((reception != null) ? (await MapFromDatabaseToReport.MapAsync(ContextGuid, reception, new ReceptionReportBusiness(ContextGuid), tranName, token)) : null);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 607);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public Task Add2CacheAsync(ReceptionBusiness item, CancellationToken token, string tranName)
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

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_receptionCheckRepo?.Dispose();
				_receptionCheckRepo = null;
				_receptionEnteghalRepo?.Dispose();
				_receptionEnteghalRepo = null;
				_receptionHavaleRepo?.Dispose();
				_receptionHavaleRepo = null;
				_receptionNaghRepo?.Dispose();
				_receptionNaghRepo = null;
				_relationShipRepo?.Dispose();
				_relationShipRepo = null;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 644);
		}
	}

	public Task<ReceptionBusiness> GetFromDbAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName)
	{
		return GetAsync(guid, order, system, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetReceptionTypeBySellAsync(List<Guid> lst1, List<Guid> lst2, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReceptionBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, null, EnSystem.None, token, tranName);
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
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 653));
		List<ENSource> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ENSource> queryable = ((IQueryable<Reception>)c.Receptions).Select((Reception q) => q.Source);
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
				WebErrorLog.Instance.StartLog(ex, "", "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 661);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 681));
				Reception reception = await context.Receptions.FindAsync(new object[1] { guid });
				if (reception == null)
				{
					return res;
				}
				reception.StatusId = (short)statusId;
				reception.Modified = DateTime.Now;
				reception.ServerStatus = ServerStatusForClient.None;
				((DbContext)context).Update<Reception>(reception);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 692);
				res.AddReturnedValue(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionPersistenceRepositoryBase.cs", 693);
			}
			return res;
		}
	}
}
