using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.ModelClass;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Sanad;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal abstract class SanadPersistanceRepositoryBase : PersistenceSyncRepository<SanadBusiness, Sanads>, ISanadRepository, ISyncRepository<SanadBusiness>, IEditableDelivery, IRepository<SanadBusiness>, INoGuidRepository<SanadBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SanadBusiness, Sanads>, IItemAsyncMapper<Sanads, SanadBusiness>, IItemAsyncMapper<SanadDetRoznameKolModel, SanadDetRoznameKolBusiness>, IItemAsyncMapper<SanadReportModel, SanadReportBusiness>
	{
		public async Task<Sanads> MapAsync(string contextGuid, SanadBusiness source, Sanads destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
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
						destination = new Sanads();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					Sanads sanads = destination;
					sanads.SanadNo = await source.GetSanadNoAsync(token, tranName);
					destination.SanadSt = source.SanadSt;
					destination.UserGuid = source.UserGuid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.StatusId = source.StatusId;
					destination.SanadType = source.SanadType;
					destination.IsTemp = source.IsTemp;
					destination.MetaData = Json.ToStringJson(source.MetaData);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 358);
					throw;
				}
				return destination;
			}
		}

		public Task<SanadBusiness> MapAsync(string contextGuid, Sanads source, SanadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
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
						destination = new SanadBusiness(contextGuid, source.SanadNo);
					}
					destination.ContextGuid = contextGuid;
					destination.Id = source.Id;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.UserGuid = source.UserGuid;
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.StatusId = source.StatusId;
					destination.SanadType = source.SanadType;
					destination.IsTemp = source.IsTemp;
					destination.MetaData = source.MetaData.FromJson<SanadMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 393);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 399);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Sanads>> MapListAsync(string contextGuid, IEnumerable<SanadBusiness> source, List<Sanads> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SanadBusiness>> MapListAsync(string contextGuid, IEnumerable<Sanads> source, List<SanadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<SanadDetRoznameKolBusiness> MapAsync(string contextGuid, SanadDetRoznameKolModel source, SanadDetRoznameKolBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
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
						destination = new SanadDetRoznameKolBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.CodeKol = source.CodeKol;
					destination.Credit = source.Credit;
					destination.DateM = source.Datesh.ShamsiToMiladi();
					destination.Debit = source.Debit;
					destination.SanadNo = source.SanadNo;
					destination.KolName = source.KolName;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 431);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<SanadDetRoznameKolBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadDetRoznameKolModel> source, List<SanadDetRoznameKolBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<SanadReportBusiness> MapAsync(string contextGuid, SanadReportModel source, SanadReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
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
						destination = new SanadReportBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateM = source.DateM;
					destination.DateSh = source.DateSh;
					destination.SanadSt = source.SanadSt;
					destination.SanadType = source.SanadType;
					destination.Sum = source.Sum;
					destination.UserGuid = source.UserGuid;
					destination.Description = source.Description;
					destination.SanadNo = source.SanadNo;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 465);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<SanadReportBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadReportModel> source, List<SanadReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	protected SanadDetPersistenceRepository _manualArticles;

	protected ViewSanadDetailPersistenceRepositoryBase _autoArticles;

	public IItemAsyncMapper<SanadReportModel, SanadReportBusiness> MapFromSanadReport { get; set; } = new mapper();


	public IItemAsyncMapper<SanadDetRoznameKolModel, SanadDetRoznameKolBusiness> MapFromRoznameKolModel { get; set; } = new mapper();


	public override IItemAsyncMapper<Sanads, SanadBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SanadBusiness, Sanads> MapToDatabase => Mapper;

	public override IItemMapper<Sanads, SanadBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public SanadPersistanceRepositoryBase(ContextHolder db)
		: base(db)
	{
		_manualArticles = new SanadDetPersistenceRepository(db);
		_autoArticles = new ViewSanadDetailPersistenceRepository(db);
	}

	public async Task<SanadBusiness> GetAsync(int SanadNO, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 55));
			SanadBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 9;
				try
				{
					SanadBusiness ret = null;
					Sanads sanads = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Sanads>((IQueryable<Sanads>)c.Sanads, (Expression<Func<Sanads, bool>>)((Sanads p) => p.SanadNo == SanadNO), token);
					if (sanads == null)
					{
						List<WebSanad_Det> list = await _autoArticles.GetAllSanadDetailsAsync(SanadNO, token, tranName);
						if (list != null && list.Any())
						{
							WebSanad_Det webSanad_Det = list.FirstOrDefault();
							ret = new SanadBusiness(ContextGuid, webSanad_Det.SanadNo)
							{
								Description = webSanad_Det?.Description,
								Status = true,
								DateM = webSanad_Det.DateM,
								SanadSt = webSanad_Det.SanadST,
								Modified = DateTime.Now,
								UserGuid = webSanad_Det.CashierGuid
							};
							if ((webSanad_Det?.AsGuid ?? Guid.Empty) != Guid.Empty)
							{
								ret.Guid = webSanad_Det.AsGuid;
							}
							else
							{
								ret.Guid = Guid.NewGuid();
							}
							if (webSanad_Det.SanadNo != 0 && (list == null || !list.Any((WebSanad_Det q) => q.AsType == Document.Sanad)))
							{
								ret.ChangeStatus = EntityChangeSt.Edited;
							}
							ret.SanadNo = SanadNO;
							SanadBusiness sanadBusiness = ret;
							await sanadBusiness.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(ContextGuid, list, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.None, appendCurrentItems: false, token);
						}
						goto IL_07f4;
					}
					ret = await MapFromDatabase.MapAsync(ContextGuid, sanads, null, tranName, token);
					if (ret.SanadType != SanadType.Note)
					{
						List<WebSanad_Det> list2 = await _autoArticles.GetAllSanadDetailsAsync(SanadNO, token, tranName);
						if (list2 != null && list2.Any())
						{
							await ret.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(ContextGuid, list2, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.Mapper, appendCurrentItems: false, token);
						}
						goto IL_07f4;
					}
					List<Sanad_DetBusiness> list3 = await _manualArticles.GetAllAsync(ret.Guid, Document.Sanad, null, null, token, tranName);
					if (list3 != null && list3.Any())
					{
						await ret.AddRangeToListSanadAsync(list3, EnSystem.Mapper, appendCurrentItems: false, token);
					}
					result = ret;
					goto end_IL_014e;
					IL_07f4:
					result = ret;
					end_IL_014e:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 107);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override async Task<SanadBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 119));
			SanadBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					Sanads source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Sanads>((IQueryable<Sanads>)c.Sanads, (Expression<Func<Sanads, bool>>)((Sanads p) => p.Guid == guid), token);
					SanadBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
					if (ret != null)
					{
						SanadBusiness sanadBusiness = ret;
						await sanadBusiness.AddRangeToListSanadAsync(await _manualArticles.GetAllAsync(ret.Guid, Document.Sanad, null, null, token, tranName), EnSystem.None, appendCurrentItems: false, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 132);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 145));
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				Sanads sanads = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Sanads>((IQueryable<Sanads>)context.Sanads, (Expression<Func<Sanads, bool>>)((Sanads p) => p.Guid == item.Guid), token);
				if (sanads == null)
				{
					return ret;
				}
				sanads.HostRefrenced = item.HostRefrenced;
				sanads.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				sanads.DeliveryMessage = item.Message;
				sanads.DeliveryShortMessage = item.Shortmessage;
				((DbContext)context).Update<Sanads>(sanads);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 161);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 164);
				ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 165);
			}
			return ret;
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> SanadSortAsync(CancellationToken token, string tranName);

	public async Task<SanadSt> GetSanadStBySanadNoAsync(int sanadNo, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 173));
			SanadSt result;
			try
			{
				_ = 1;
				try
				{
					SanadSt sanadSt = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Sanads>((IQueryable<Sanads>)c.Sanads, (Expression<Func<Sanads, bool>>)((Sanads p) => p.SanadNo == sanadNo), token))?.SanadSt ?? SanadSt.None;
					result = sanadSt;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 181);
					result = SanadSt.None;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetSanadStBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public Task<bool> ChangeSanadStAsync(string SanadNOs, SanadSt ST, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(SanadBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<int> NextIdAsync(CancellationToken token, string tranName)
	{
		return _autoArticles.NextSanadNoAsync(token, tranName);
	}

	public abstract Task<int> UpdateVSanadDetailAsync(short mode, Guid asGuid, int sanadNo, int oldSanadNo, string oldDateSh, Guid cashierGuid, short tryCount, CancellationToken token, string tranName);

	public async Task<int> GetSanadNoByGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSanadNoByGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetSanadNoByGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 199));
			int result;
			try
			{
				_ = 1;
				try
				{
					int num = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Sanads>((IQueryable<Sanads>)c.Sanads, (Expression<Func<Sanads, bool>>)((Sanads p) => p.Guid == guid), token))?.SanadNo ?? 0;
					result = num;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetSanadNoByGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 209);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetSanadNoByGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<bool> IsVSanadDetailAsync(int SanadNo, CancellationToken token, string tranName)
	{
		try
		{
			return (await _autoArticles.GetAllSanadDetailsAsync(SanadNo, token, tranName)).Count > 0;
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
			WebErrorLog.Instance.StartLog(ex2, "", "IsVSanadDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 229);
			throw;
		}
	}

	public abstract Task<IEnumerable<SanadReportBusiness>> GetAllAsync(SanadFilter filter, CancellationToken token, string tranName);

	public abstract Task<List<SanadDetReportBusiness>> GetAllRoznameSanadDetAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token);

	public abstract Task<List<SanadDetRoznameKolBusiness>> GetAllSanadDetRoznameKolAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token);

	public abstract Task<List<MoeinTafsilReportModel>> GetMoein2TafsilsRptAsync(DateTime date1, DateTime date2, Guid moeinGuid, string tranName, CancellationToken token);

	public abstract Task<List<MoeinTafsilReportModel>> GetTafsil2MoeinsRptAsync(DateTime date1, DateTime date2, Guid tafsilGuid, string tranName, CancellationToken token);

	public abstract Task<List<SanadDetRoznameKolBusiness>> GetTestTarazNameAsync(DateTime date, string tranName, CancellationToken token);

	public async Task<SanadSt> GetSanadStByGuidTypeAsync(Guid SanadGuid, Document SanadType, CancellationToken token, string tranName)
	{
		return (await GetAsync(SanadGuid, SanadType, LoadDets: false, token, tranName))?.SanadSt ?? SanadSt.None;
	}

	public abstract Task<SanadBusiness> GetAsync(Guid SanadGuid, Document SanadType, bool LoadDets, CancellationToken token, string tranName);

	public Task<SanadBusiness> GetFromDbAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		return GetAsync(sanadGuid, sanadType, LoadDets: true, token, tranName);
	}

	public abstract Task<List<SanadDetReportBusiness>> GetAllGardeshSanadAsync(ENHesabLevel level, DateTime? date1, DateTime? date2, long? code1, long? code2, long? codeMoein, Guid? kolGuid, string tranName, int? maxCount, short maxParalelThreads, CancellationToken token);

	public abstract Task<ReturnedSaveFuncInfo> RemoveAsync(Guid asGuid, Document asType, short tryCount, CancellationToken token, string tranName);

	public abstract Task<List<MoeinTafsilReportModel>> GetTafsilTajmeeReportAsync(DateTime date1, DateTime date2, string moeinCode1, string moeinCode2, string tafsilCode1, string tafsilCode2, CancellationToken token, string tranName);

	public abstract Task<List<SanadDetReportBusiness>> GetMoeinTafsilGardeshAsync(DateTime d1, DateTime d2, Guid tafsilGuid, string moeinCode1, string moeinCode2, string tranName, CancellationToken token);

	public Task<Guid> AsId_To_AsGuidAsync(int asId, Document asType, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public abstract Task<ReturnedSaveFuncInfo> ChangeSanadNoAsync(List<int> oldSanadNo, int newSanadNo, string description, DateTime date, CancellationToken token, string tranName);

	public abstract Task<ReturnedSaveFuncInfo> ChangeSanadStatusAsync(string sansdNos, SanadSt st, short tryCount, CancellationToken token, string tranName);

	public abstract Task<int> CountOfAutoSanadWithDiffrentDateAsync(int sanadNo, DateTime dateM, Guid asGuid1, short asType1, Guid asGuid2, short asType2, CancellationToken token, string tranName);

	public Task<List<Sanad_DetBusiness>> GetSanadTempAsync(CancellationToken token, string tranName)
	{
		return GetStaticSanadTempAsync(dbContext, token, tranName);
	}

	public Task<SanadBusiness> GetEkhtetamieSanadAsync(CancellationToken token, string tranName)
	{
		return GetStaticEkhtetamieSanadAsync(dbContext, token, tranName);
	}

	public abstract Task<SanadBusiness> GetStaticEkhtetamieSanadAsync(ContextHolder dbContext, CancellationToken token, string tranName);

	public abstract Task<List<Sanad_DetBusiness>> GetStaticSanadTempAsync(ContextHolder dbContext, CancellationToken token, string tranName);

	public abstract Task<bool> CheckGhestAsync(Guid sanadGuid, CancellationToken token, string tranName);

	public async Task<Guid> GetGuidBySanadNoAsync(int sanadNo, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 259));
			Guid result;
			try
			{
				_ = 1;
				try
				{
					Guid guid = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Sanads>((IQueryable<Sanads>)c.Sanads, (Expression<Func<Sanads, bool>>)((Sanads p) => p.SanadNo == sanadNo), token))?.Guid ?? Guid.Empty;
					result = guid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 269);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetGuidBySanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs");
				}
			}
			return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 287);
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> SaveAsync(string caller, SanadBusiness item, EnSystem system, int oldSanadNo, CancellationToken token, string tranName, IEnumerable<ViewSanadDetailCoverBusiness> lst, params Tuple<Document, Guid>[] autoSanadItemsForSave);

	public abstract Task<decimal> GetBeforeAccountAsync(Guid tafsilGuid, Guid asGuid, Document asType, CancellationToken token, string tranName);

	public abstract Task<DateTime?> GetSanadBargashtDateAsync(decimal price, Guid debitTafsilGuid, Guid creditTafsilGuid, int sanadNo, CancellationToken token, string tranName);

	public abstract Task<List<Sanad_DetBusiness>> GetSanadTemporaryAsync(CancellationToken token, string tranName);

	public virtual Task<IEnumerable<SanadGhestViewModel>> GetAllSanadGhestAsync(Guid sanadGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> RemoveDuplicateSanadNoAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 302));
				Sanads sanads = await context.Sanads.FindAsync(new object[1] { guid });
				if (sanads == null)
				{
					return res;
				}
				sanads.StatusId = (short)statusId;
				sanads.Modified = DateTime.Now;
				sanads.ServerStatus = ServerStatusForClient.None;
				((DbContext)context).Update<Sanads>(sanads);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 313);
				res.AddReturnedValue(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPersistanceRepositoryBase.cs", 314);
			}
			return res;
		}
	}
}
