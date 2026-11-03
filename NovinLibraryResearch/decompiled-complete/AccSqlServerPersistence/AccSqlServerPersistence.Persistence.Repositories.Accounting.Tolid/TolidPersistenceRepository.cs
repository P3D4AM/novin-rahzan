using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidPersistenceRepository : PersistenceSyncRepository<TolidBusiness, AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>, ITolidRepository, IEntityRepository<TolidBusiness, TolidReportBusiness>, IEditableDelivery, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidBusiness, AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>, IItemAsyncMapper<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid, TolidBusiness>, IItemAsyncMapper<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid, TolidReportBusiness>
	{
		public async Task<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> MapAsync(string contextGuid, TolidBusiness source, AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs"))
			{
				_ = 2;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid();
					}
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.CashierGuid = source.CashierGuid;
					AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid tolid = destination;
					tolid.Id2 = await source.GetId2Async(token, tranName);
					destination.MahsolMoeinGuid = source.MahsolMoeinGuid;
					destination.MahsolTafsilGuid = source.MahsolTafsilGuid;
					destination.MavadMoeinGuid = source.MavadMoeinGuid;
					destination.MavadTafsilGuid = source.MavadTafsilGuid;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.SumHazine = source.HazineSum;
					tolid = destination;
					tolid.SumMahsol = await source.GetExportSumAsync(EnSystem.Mapper, token, tranName);
					tolid = destination;
					tolid.SumMavad = await source.GetMavadSumAsync(EnSystem.Mapper, token, tranName);
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.SanadAffective = source.SanadAffective;
					destination.KardexAffective = source.KardexAffective;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 348);
					throw;
				}
			}
		}

		public async Task<TolidBusiness> MapAsync(string contextGuid, AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid source, TolidBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs"))
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
						destination = await TolidBusiness.GenerateNewAsync(contextGuid, EnSystem.Mapper, token);
					}
					destination.ContextGuid = contextGuid;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.CashierGuid = source.CashierGuid;
					destination.SetId2(source.Id2);
					destination.MahsolMoeinGuid = source.MahsolMoeinGuid;
					destination.MahsolTafsilGuid = source.MahsolTafsilGuid;
					destination.MavadMoeinGuid = source.MavadMoeinGuid;
					destination.MavadTafsilGuid = source.MavadTafsilGuid;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.SanadAffective = source.SanadAffective;
					destination.KardexAffective = source.KardexAffective;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 391);
					throw;
				}
			}
		}

		public Task<TolidReportBusiness> MapAsync(string contextGuid, AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid source, TolidReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs"))
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
						destination = new TolidReportBusiness();
					}
					destination.Id2 = source.Id2;
					destination.SumHazine = source.SumHazine;
					destination.SumMahsol = source.SumMahsol;
					destination.SumMavad = source.SumMavad;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.CashierGuid = source.CashierGuid;
					destination.MahsolMoeinGuid = source.MahsolMoeinGuid;
					destination.MahsolTafsilGuid = source.MahsolTafsilGuid;
					destination.MavadMoeinGuid = source.MavadMoeinGuid;
					destination.MavadTafsilGuid = source.MavadTafsilGuid;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.SanadAffective = source.SanadAffective;
					destination.KardexAffective = source.KardexAffective;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 429);
					throw;
				}
			}
		}

		public Task<List<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>> MapListAsync(string contextGuid, IEnumerable<TolidBusiness> source, List<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidBusiness>> MapListAsync(string contextGuid, IEnumerable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> source, List<TolidBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidReportBusiness>> MapListAsync(string contextGuid, IEnumerable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> source, List<TolidReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private TolidMavadPersistenceRepository _mavad;

	private TolidExportPersistenceRepository _export;

	private TolidHazinePersistenceRepository _hazine;

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasModified { get; }

	public override bool HasStatus { get; }

	public override IItemMapper<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid, TolidBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid, TolidBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidBusiness, AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> MapToDatabase => Mapper;

	public IItemAsyncMapper<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid, TolidReportBusiness> MapToReport => Mapper;

	internal TolidPersistenceRepository(ContextHolder context)
		: base(context)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs"))
		{
			try
			{
				dbContext = context;
				_mavad = new TolidMavadPersistenceRepository(context);
				_hazine = new TolidHazinePersistenceRepository(context);
				_export = new TolidExportPersistenceRepository(context);
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
				WebErrorLog.Instance.StartLog(ex3, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 51);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 63));
				AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid tolid = await dbcontext.Tolid.FindAsync(new object[1] { guid });
				tolid.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(tolid, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 66);
			}
			catch (ThreadAbortException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 68);
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 69);
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 72);
				ret.AddReturnedValue(ex3, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 73);
			}
			return ret;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				_hazine?.Dispose();
				_hazine = null;
				_export?.Dispose();
				_export = null;
				_mavad?.Dispose();
				_mavad = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 96);
		}
	}

	public Task<TolidReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TolidBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 102));
		try
		{
			AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid tolid = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(tolid, tolid.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 106);
			if (res.HasError)
			{
				return res;
			}
			OrderBusiness orderBusiness = await item.GetMavadListAsync(EnSystem.None, tranName, token);
			List<TolidMavadBusiness> list = null;
			if ((orderBusiness?.ListKala?.Any()).GetValueOrDefault())
			{
				list = await TolidMavadMapper.Instance.MapListAsync(ContextGuid, orderBusiness.ListKala, new List<TolidMavadBusiness>(), tranName, token);
				foreach (TolidMavadBusiness item2 in list)
				{
					item2.Guid = Guid.NewGuid();
				}
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _mavad.SaveAsync(item.Guid, list, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 116);
			if (res.HasError)
			{
				return res;
			}
			OrderBusiness orderBusiness2 = await item.GetExportListAsync(EnSystem.None, tranName, token);
			List<TolidExportBusiness> list2 = null;
			if ((orderBusiness2?.ListKala?.Any()).GetValueOrDefault())
			{
				list2 = await TolidExportMapper.Instance.MapListAsync(ContextGuid, orderBusiness2.ListKala, new List<TolidExportBusiness>(), tranName, token);
				foreach (TolidExportBusiness item3 in list2)
				{
					item3.Guid = Guid.NewGuid();
				}
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _export.SaveAsync(item.Guid, list2, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 127);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _hazine.SaveAsync(item.Guid, item.HazineList as List<TolidHazineBusiness>, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 131);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 135);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 136);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task Add2CacheAsync(TolidBusiness item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<IEnumerable<TolidBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 144));
		IEnumerable<TolidBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 8;
			try
			{
				IQueryable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> tolid = (IQueryable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>)c.Tolid;
				List<TolidBusiness> busList = await MapFromDatabase.MapListAsync(ContextGuid, tolid, null, tranName, token);
				if (!(busList?.Any() ?? false))
				{
					result = null;
				}
				else
				{
					foreach (TolidBusiness item in busList)
					{
						List<TolidExportBusiness> exportList = await _export.GetAllAsync(item.Guid, token, tranName);
						List<TolidHazineBusiness> hazineList = await _hazine.GetAllAsync(item.Guid, token, tranName);
						List<TolidMavadBusiness> mavadList = await _mavad.GetAllAsync(item.Guid, token, tranName);
						if (exportList?.Any() ?? false)
						{
							TolidBusiness tolidBusiness = item;
							await tolidBusiness.AddExportListAsync(EnSystem.Mapper, await TolidExportMapper.Instance.MapListAsync(ContextGuid, exportList, new List<OrderDetailBusiness>(), tranName, token));
						}
						if (mavadList?.Any() ?? false)
						{
							TolidBusiness tolidBusiness = item;
							await tolidBusiness.AddMavadListAsync(EnSystem.Mapper, await TolidMavadMapper.Instance.MapListAsync(ContextGuid, mavadList, new List<OrderDetailBusiness>(), tranName, token));
						}
						if (hazineList != null && hazineList.Count > 0)
						{
							await item.AddHazineListAsync(hazineList, EnSystem.None);
						}
					}
					result = busList?.OrderBy((TolidBusiness q) => q.DateM)?.ToList();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 170);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 184));
			AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid obj = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>((IQueryable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>)c.Tolid, (Expression<Func<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid, bool>>)((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.Guid == guid), token);
			if (obj == null)
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _export.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 187);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _hazine.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 189);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _mavad.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 191);
			if (res.HasError)
			{
				return res;
			}
			c.Tolid.Remove(obj);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 197);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 198);
		}
		return res;
	}

	public override async Task<TolidBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 204));
		TolidBusiness item = null;
		try
		{
			int num;
			_ = num - 1;
			_ = 4;
			try
			{
				AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid tolid = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>((IQueryable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>)c.Tolid, (Expression<Func<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid, bool>>)((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.Guid == guid), token);
				if (tolid == null)
				{
					return null;
				}
				item = await MapFromDatabase.MapAsync(ContextGuid, tolid, null, tranName, token);
				await _export.GetAllAsync(item, token, tranName);
				await _mavad.GetAllAsync(item, token, tranName);
				await _hazine.GetAllAsync(item, token, tranName);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 220);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs");
			}
		}
		return item;
	}

	public override Task<TolidBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public override Task<IEnumerable<TolidBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return GetAllAsync(token, tranName);
	}

	public async Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 234));
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>(((IQueryable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>)c.Tolid).OrderByDescending((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid x) => x.Id2).Take(1), token))?.Id2 ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex3, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 246);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<TolidReportBusiness>> GetAllReportAsync(TolidFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 257));
		IEnumerable<TolidReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid> queryable = (IQueryable<AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid>)c.Tolid;
				if (filter != null)
				{
					filter.Description = filter.Description?.FixString()?.RemoveCharactersForLinkSearch() ?? "";
					List<int> numbers = new List<int>();
					foreach (int item in filter?.NumberList?.Distinct() ?? new List<int>())
					{
						if (item > 0)
						{
							numbers.Add(item);
						}
					}
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.DateM >= filter.DateFrom.Value);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.DateM <= filter.DateTo.Value);
					}
					if (numbers?.Any() ?? false)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid p) => numbers.Contains(p.Id2));
					}
					if (filter.NumberFrom != 0)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.Id2 >= filter.NumberFrom);
					}
					if (filter.NumberTo != 0)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.Id2 <= filter.NumberTo);
					}
					if (filter.SanadAffective.HasValue)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.SanadAffective == filter.SanadAffective.Value);
					}
					if (filter.KardexAffective.HasValue)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.KardexAffective == filter.KardexAffective.Value);
					}
					if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid q) => q.CashierGuid == filter.CashierGuid.Value);
					}
					foreach (string word in filter.Description?.SplitString()?.Distinct() ?? new List<string>())
					{
						if (!string.IsNullOrEmpty(word))
						{
							queryable = queryable?.Where((AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%"));
						}
					}
				}
				List<TolidReportBusiness> list = await MapToReport.MapListAsync(ContextGuid, queryable?.ToList(), null, tranName, token);
				result = ((list != null && list.Any()) ? list?.OrderBy((TolidReportBusiness q) => q.DateM)?.ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs", 290);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidPersistenceRepository.cs");
			}
		}
		return result;
	}

	public Task<TolidBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	Task<List<TolidBusiness>> IEntityRepository<TolidBusiness, TolidReportBusiness>.GetAllNotSentAsync(CancellationToken token, string tranName)
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
}
