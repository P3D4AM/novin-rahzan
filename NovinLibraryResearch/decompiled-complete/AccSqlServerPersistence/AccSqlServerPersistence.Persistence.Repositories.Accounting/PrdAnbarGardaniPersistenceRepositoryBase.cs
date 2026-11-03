using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Records.AnbarGardani;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal abstract class PrdAnbarGardaniPersistenceRepositoryBase : PersistenceSyncRepository<PrdAnbarGardaniBusiness, PrdAnbarGardani>, IPrdAnbarGardaniRepository, IEntityRepository<PrdAnbarGardaniBusiness, PrdAnbarGardaniReportBusiness>, IEditableDelivery, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdAnbarGardaniBusiness, PrdAnbarGardani>, IItemAsyncMapper<PrdAnbarGardani, PrdAnbarGardaniBusiness>, IItemAsyncMapper<PrdAnbarGardani, PrdAnbarGardaniReportBusiness>
	{
		public async Task<PrdAnbarGardani> MapAsync(string contextGuid, PrdAnbarGardaniBusiness source, PrdAnbarGardani destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
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
						destination = new PrdAnbarGardani();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateM = source.DateM;
					destination.DateSabt = source.DateSabt;
					destination.Number = source.Number;
					destination.Type = source.Type;
					PrdAnbarGardani prdAnbarGardani = destination;
					prdAnbarGardani.SanadNo = await source.GetSanadNumberAsync(token, tranName);
					destination.SanadSt = source.SanadSt;
					destination.KasrDebitTafsilGuid = source.KasrDebitTafsilGuid;
					destination.KasrDebitMoeinGuid = source.KasrDebitMoeinGuid;
					destination.KasrCreditTafsilGuid = source.KasrCreditTafsilGuid;
					destination.KasrCreditMoeinGuid = source.KasrCreditMoeinGuid;
					destination.EzafeDebitTafsilGuid = source.EzafeDebitTafsilGuid;
					destination.EzafeCreditTafsilGuid = source.EzafeCreditTafsilGuid;
					destination.EzafeDebitMoeinGuid = source.EzafeDebitMoeinGuid;
					destination.EzafeCreditMoeinGuid = source.EzafeCreditMoeinGuid;
					destination.KasrSum = source.KasrSum;
					destination.EzafeSum = source.EzafeSum;
					destination.IsTemp = source.IsTemp;
					destination.UserGuid = source.UserGuid;
					destination.Description = source.Description;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.StatusId = source.StatusId;
					destination.MetaData = Json.ToStringJson(source.MetaData);
					destination.DateUtc = destination.DateM.ToUniversalTime();
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 500);
					throw;
				}
				return destination;
			}
		}

		public Task<PrdAnbarGardaniBusiness> MapAsync(string contextGuid, PrdAnbarGardani source, PrdAnbarGardaniBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
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
						destination = new PrdAnbarGardaniBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateM = source.DateM;
					destination.DateSabt = source.DateSabt;
					destination.Number = source.Number;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.KasrDebitTafsilGuid = source.KasrDebitTafsilGuid;
					destination.KasrDebitMoeinGuid = source.KasrDebitMoeinGuid;
					destination.KasrCreditTafsilGuid = source.KasrCreditTafsilGuid;
					destination.KasrCreditMoeinGuid = source.KasrCreditMoeinGuid;
					destination.EzafeDebitTafsilGuid = source.EzafeDebitTafsilGuid;
					destination.EzafeCreditTafsilGuid = source.EzafeCreditTafsilGuid;
					destination.EzafeDebitMoeinGuid = source.EzafeDebitMoeinGuid;
					destination.EzafeCreditMoeinGuid = source.EzafeCreditMoeinGuid;
					destination.UserGuid = source.UserGuid;
					destination.Description = source.Description;
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.IsTemp = source.IsTemp;
					destination.StatusId = source.StatusId;
					destination.MetaData = source.MetaData.FromJson<AnbarGardaniMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 543);
					destination.Type = source.Type;
					destination.DateUtc = source.DateUtc;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 551);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<PrdAnbarGardaniReportBusiness> MapAsync(string contextGuid, PrdAnbarGardani source, PrdAnbarGardaniReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
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
						destination = new PrdAnbarGardaniReportBusiness();
					}
					destination.Guid = source.Guid;
					destination.DateM = source.DateM;
					destination.Number = source.Number;
					destination.SanadNo = source.SanadNo;
					destination.KasrSum = source.KasrSum;
					destination.EzafeSum = source.EzafeSum;
					destination.Description = source.Description;
					destination.ServerStatus = source.ServerStatus;
					destination.IsTemp = source.IsTemp;
					destination.UserName = source.CashierItem?.Name ?? "";
					destination.StatusId = source.StatusId;
					destination.Type = source.Type;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 584);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<PrdAnbarGardani>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardaniBusiness> source, List<PrdAnbarGardani> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdAnbarGardaniBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardani> source, List<PrdAnbarGardaniBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdAnbarGardaniReportBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardani> source, List<PrdAnbarGardaniReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	protected PrdSerialPersistanceRepositoryBase _serialRepo;

	protected IPrdAnbargardaniDetRepository prdAnbarGardaniDet;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasModified => true;

	public override bool HasStatus => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<PrdAnbarGardani, PrdAnbarGardaniBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdAnbarGardaniBusiness, PrdAnbarGardani> MapToDatabase => Mapper;

	public IItemAsyncMapper<PrdAnbarGardani, PrdAnbarGardaniReportBusiness> MapToReport => Mapper;

	public override IItemMapper<PrdAnbarGardani, PrdAnbarGardaniBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public PrdAnbarGardaniPersistenceRepositoryBase(ContextHolder db, IPrdAnbargardaniDetRepository prdAnbargardaniDetRepository)
		: base(db)
	{
		_serialRepo = new PrdSerialPersistanceRepository(db);
		prdAnbarGardaniDet = prdAnbargardaniDetRepository;
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 52));
				PrdAnbarGardani prdAnbarGardani = await dbcontext.PrdAnbarGardani.FindAsync(new object[1] { guid });
				prdAnbarGardani.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(prdAnbarGardani, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 55);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 60);
				ret.AddReturnedValue(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 61);
			}
			return ret;
		}
	}

	public Task ClearCachedAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<int> DbCountAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public abstract Task<PrdAnbarGardaniReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName);

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PrdAnbarGardaniBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 75));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				PrdAnbarGardani prdAnbarGardani = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(prdAnbarGardani, prdAnbarGardani.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 78);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await prdAnbarGardaniDet.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 80);
				if ((item?.ListAnbarGardaniDet?.Any()).GetValueOrDefault())
				{
					foreach (PrdAnbarGardaniDetBusiness item2 in item.ListAnbarGardaniDet)
					{
						item2.MasterGuid = item.Guid;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await prdAnbarGardaniDet.SaveAsync(item.ListAnbarGardaniDet?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 86);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 92);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 93);
			}
			return ret;
		}
	}

	public Task Add2CacheAsync(PrdAnbarGardaniBusiness item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<IEnumerable<PrdAnbarGardaniBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 102));
			IEnumerable<PrdAnbarGardaniBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					DbSet<PrdAnbarGardani> prdAnbarGardani = c.PrdAnbarGardani;
					List<PrdAnbarGardani> source = await ((prdAnbarGardani != null) ? EntityFrameworkQueryableExtensions.ToListAsync<PrdAnbarGardani>((IQueryable<PrdAnbarGardani>)prdAnbarGardani, token) : null);
					List<PrdAnbarGardaniBusiness> query = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
					foreach (PrdAnbarGardaniBusiness item in query)
					{
						await prdAnbarGardaniDet.GetAllAsync(item, token, tranName);
					}
					result = query;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 115);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 134));
				PrdAnbarGardani prdAnbarGardani = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<PrdAnbarGardani>((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani, (Expression<Func<PrdAnbarGardani, bool>>)((PrdAnbarGardani p) => p.Guid == guid), token);
				if (prdAnbarGardani == null)
				{
					return ret;
				}
				c.PrdAnbarGardani.Remove(prdAnbarGardani);
				IQueryable<PrdAnbarGardaniDet> obj = ((IQueryable<PrdAnbarGardaniDet>)((DbContext)c).Set<PrdAnbarGardaniDet>()).Where((PrdAnbarGardaniDet x) => x.MasterGuid == guid);
				foreach (PrdAnbarGardaniDet item in obj)
				{
					if (item != null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await _serialRepo.RemoveRangAsync(item.Guid, Document.AnbarGardani, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 142);
					}
				}
				((DbContext)c).Set<PrdAnbarGardaniDet>().RemoveRange((IEnumerable<PrdAnbarGardaniDet>)obj);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 149);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 150);
			}
			return ret;
		}
	}

	public override async Task<PrdAnbarGardaniBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 157));
			PrdAnbarGardaniBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					PrdAnbarGardani prdAnbarGardani = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdAnbarGardani>((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani, (Expression<Func<PrdAnbarGardani, bool>>)((PrdAnbarGardani x) => x.Guid == guid), token);
					if (prdAnbarGardani == null)
					{
						result = null;
					}
					else
					{
						PrdAnbarGardaniBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, prdAnbarGardani, null, tranName, token);
						await prdAnbarGardaniDet.GetAllAsync(ret, token, tranName);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 171);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<PrdAnbarGardaniBusiness> GetByNumberAsync(int number, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 183));
			PrdAnbarGardaniBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					PrdAnbarGardani prdAnbarGardani = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdAnbarGardani>((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani, (Expression<Func<PrdAnbarGardani, bool>>)((PrdAnbarGardani x) => x.Number == number), token);
					if (prdAnbarGardani == null)
					{
						result = null;
					}
					else
					{
						PrdAnbarGardaniBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, prdAnbarGardani, null, tranName, token);
						await prdAnbarGardaniDet.GetAllAsync(ret, token, tranName);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 197);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<PrdAnbarGardaniBusiness> GetAsync(int sanadNumber, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 209));
			PrdAnbarGardaniBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					PrdAnbarGardani prdAnbarGardani = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdAnbarGardani>((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani, (Expression<Func<PrdAnbarGardani, bool>>)((PrdAnbarGardani x) => x.SanadNo == sanadNumber), token);
					if (prdAnbarGardani == null)
					{
						result = null;
					}
					else
					{
						PrdAnbarGardaniBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, prdAnbarGardani, null, tranName, token);
						await prdAnbarGardaniDet.GetAllAsync(ret, token, tranName);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 223);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> NextNumberAsync(int? statusId, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 234));
		using (new JaegerMonitor("NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
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
					List<int> list = await EntityFrameworkQueryableExtensions.ToListAsync<int>(((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani).Select((PrdAnbarGardani q) => q.Number), token);
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
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<PrdAnbarGardani>(((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani).OrderByDescending((PrdAnbarGardani x) => x.Number).Take(1), token))?.Number ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex3, "", "NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 259);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<List<PrdAnbarGardaniDetBusiness>> GetAllAsync(PrdAnbarGardaniBusiness master, AnbarGardaniFilter filter, CancellationToken token, string tranName);

	public Task<PrdAnbarGardaniBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_serialRepo?.Dispose();
				_serialRepo = null;
				prdAnbarGardaniDet?.Dispose();
				prdAnbarGardaniDet = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 283);
		}
	}

	public virtual async Task<List<PrdAnbarGardaniReportBusiness>> GetAllAsync(AnbarGardaniFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 287));
		List<PrdAnbarGardaniReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				IQueryable<PrdAnbarGardani> queryable = (IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani;
				if (!string.IsNullOrEmpty(filter.Description))
				{
					foreach (string word in filter.Description?.FixString()?.SplitString())
					{
						if (!string.IsNullOrEmpty(word))
						{
							queryable = queryable?.Where((PrdAnbarGardani p) => p.Description.Contains(word) || p.Number == word.ParseToInt());
						}
					}
				}
				if (filter.DateFrom.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => q.DateM >= filter.DateFrom);
				}
				if (filter.DateTo.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => q.DateM <= filter.DateTo);
				}
				if (filter.Type.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => (int?)q.Type == (int?)filter.Type);
				}
				if ((filter?.StatusId).GetValueOrDefault() > 0)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => q.StatusId == filter.StatusId.Value);
				}
				queryable = queryable?.OrderBy((PrdAnbarGardani x) => x.DateM);
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					List<PrdAnbarGardaniReportBusiness> list;
					if (token.IsCancellationRequested)
					{
						list = null;
					}
					else
					{
						IItemAsyncMapper<PrdAnbarGardani, PrdAnbarGardaniReportBusiness> mapToReport = MapToReport;
						string contextGuid = ContextGuid;
						list = await mapToReport.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<PrdAnbarGardani>(queryable, token), null, tranName, token);
					}
					result = list;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 312);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		return base.SetSaveResult(item, token, tryCount, tranName);
	}

	public async Task<Guid> GetDepotGuidAsync(Guid detailGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 325));
			Guid result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdAnbarGardaniDet>((IQueryable<PrdAnbarGardaniDet>)c.PrdAnbarGardaniDet, (Expression<Func<PrdAnbarGardaniDet, bool>>)((PrdAnbarGardaniDet x) => x.Guid == detailGuid), token))?.DptGuid ?? Guid.Empty;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 337);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateTypeAsync(Guid guid, SanadType type, CancellationToken token, short tryCount, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 351));
			if (type == SanadType.Eftetahiye || type == SanadType.Ekhtetamiye)
			{
				IQueryable<PrdAnbarGardani> queryable = ((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani).Where((PrdAnbarGardani q) => q.Type == type);
				if (queryable != null && queryable.Any())
				{
					foreach (PrdAnbarGardani item in queryable)
					{
						item.Type = SanadType.Movaghat;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 361);
					}
				}
			}
			PrdAnbarGardani prdAnbarGardani = await c.PrdAnbarGardani.FindAsync(new object[1] { guid });
			if (prdAnbarGardani != null)
			{
				prdAnbarGardani.Type = type;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(prdAnbarGardani, prdAnbarGardani.Guid), ReturnedState.None, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 369);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 374);
			res.AddReturnedValue(ex, "", "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 375);
		}
		return res;
	}

	async Task<List<PrdAnbarGardaniBusiness>> IEntityRepository<PrdAnbarGardaniBusiness, PrdAnbarGardaniReportBusiness>.GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 381));
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			List<PrdAnbarGardaniBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<PrdAnbarGardani> list = ((IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani)?.Where((PrdAnbarGardani e) => e.IsTemp == false && (e.ServerStatus == ServerStatusForClient.DeliveryError || e.ServerStatus == ServerStatusForClient.None || e.ServerStatus == ServerStatusForClient.SendError)).ToList();
					result = ((list != null && list.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, list, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 398);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateDateAsync(Guid guid, DateTime date, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 412));
			PrdAnbarGardani prdAnbarGardani = await c.PrdAnbarGardani.FindAsync(new object[1] { guid });
			if (prdAnbarGardani != null)
			{
				prdAnbarGardani.DateM = date;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(prdAnbarGardani, prdAnbarGardani.Guid), ReturnedState.None, "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 417);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 422);
			res.AddReturnedValue(ex, "", "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 423);
		}
		return res;
	}

	public virtual Task<List<AnbarGardaniEkhtetamRecord>> GetAllEkhtetamRecordsAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 435));
				PrdAnbarGardani prdAnbarGardani = await context.PrdAnbarGardani.FindAsync(new object[1] { guid });
				if (prdAnbarGardani == null)
				{
					return res;
				}
				prdAnbarGardani.StatusId = (short)statusId;
				prdAnbarGardani.Modified = DateTime.Now;
				prdAnbarGardani.ServerStatus = ServerStatusForClient.None;
				((DbContext)context).Update<PrdAnbarGardani>(prdAnbarGardani);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 446);
				res.AddReturnedValue(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniPersistenceRepositoryBase.cs", 447);
			}
			return res;
		}
	}
}
