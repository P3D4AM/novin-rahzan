using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Business.Products.ProductSerial;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

public abstract class PrdSerialPersistanceRepositoryBase : PersistenceSyncRepository<PrdSerialBusiness, PrdSerial>, IPrdSerialRepository, ISyncRepository<PrdSerialBusiness>, IEditableDelivery, IRepository<PrdSerialBusiness>, INoGuidRepository<PrdSerialBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdSerialBusiness, PrdSerial>, IItemAsyncMapper<PrdSerial, PrdSerialBusiness>
	{
		public Task<PrdSerial> MapAsync(string contextGuid, PrdSerialBusiness source, PrdSerial destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdSerial();
				}
				destination.ProductGuid = source.ProductGuid;
				destination.Serial = source.Serial;
				destination.GarrantyCode = source.GarrantyCode;
				if (string.IsNullOrEmpty(destination.GarrantyCode))
				{
					destination.GarrantyCode = null;
				}
				destination.StartDate = source.StartDate;
				destination.EndDate = source.EndDate;
				destination.ProductGuid = source.ProductGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.IsTransfer = source.IsTransfer;
				destination.Status = source.Status;
				destination.TechnicalInfo1 = source.TechnicalInfo1;
				destination.TechnicalInfo2 = source.TechnicalInfo2;
				destination.TechnicalInfo3 = source.TechnicalInfo3;
				destination.InsertDate = source.InsertDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 309);
				throw;
			}
		}

		public Task<PrdSerialBusiness> MapAsync(string contextGuid, PrdSerial source, PrdSerialBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdSerialBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.Serial = source.Serial;
				destination.GarrantyCode = source.GarrantyCode;
				destination.StartDate = source.StartDate;
				destination.EndDate = source.EndDate;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.IsTransfer = source.IsTransfer;
				destination.Status = source.Status;
				destination.TechnicalInfo1 = source.TechnicalInfo1;
				destination.TechnicalInfo2 = source.TechnicalInfo2;
				destination.TechnicalInfo3 = source.TechnicalInfo3;
				destination.InsertDate = source.InsertDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 349);
				throw;
			}
		}

		public Task<List<PrdSerial>> MapListAsync(string contextGuid, IEnumerable<PrdSerialBusiness> source, List<PrdSerial> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdSerialBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdSerial> source, List<PrdSerialBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private PrdSerialDetailPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<PrdSerial, PrdSerialBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdSerialBusiness, PrdSerial> MapToDatabase => Mapper;

	public override IItemMapper<PrdSerial, PrdSerialBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public PrdSerialPersistanceRepositoryBase(ContextHolder db)
		: base(db)
	{
		_detRepo = new PrdSerialDetailPersistenceRepository(db);
	}

	public async Task<PrdSerialBusiness> GetAsync(string serialNumber, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 38));
			PrdSerialBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					PrdSerial source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdSerial>((IQueryable<PrdSerial>)c.PrdSerials, (Expression<Func<PrdSerial, bool>>)((PrdSerial p) => p.Serial == serialNumber), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 48);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override async Task<PrdSerialBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 60));
			PrdSerialBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					PrdSerial obj = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdSerial>((IQueryable<PrdSerial>)c.PrdSerials, (Expression<Func<PrdSerial, bool>>)((PrdSerial p) => p.Guid == guid), token);
					if (obj == null)
					{
						result = null;
					}
					else
					{
						List<PrdSerialDetailBusiness> detList = await _detRepo.GetAllAsync(guid, token, tranName);
						PrdSerialBusiness prdSerialBusiness = await MapFromDatabase.MapAsync(ContextGuid, obj, null, tranName, token);
						if (detList?.Any() ?? false)
						{
							prdSerialBusiness.AddDetList(detList, clearDets: true);
						}
						result = prdSerialBusiness;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 75);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByPrdGuidAsync(Guid prdGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 89));
			try
			{
				PrdSerial prdSerial = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdSerial>((IQueryable<PrdSerial>)c.PrdSerials, (Expression<Func<PrdSerial, bool>>)((PrdSerial x) => x.ProductGuid == prdGuid), token);
				if (prdSerial != null)
				{
					c.PrdSerials.Remove(prdSerial);
				}
				return new ReturnedSaveFuncInfo();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 100);
				return new ReturnedSaveFuncInfo(ex2, "", "RemoveByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 101);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_detRepo?.Dispose();
				_detRepo = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 117);
		}
	}

	public abstract Task<IEnumerable<PrdSerialBusiness>> GetAllAsync(ProductSerialFilter filter, CancellationToken token, string tranName);

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdSerialBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (items != null)
				{
					if (items != null && items.Count() > 0)
					{
						BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 129);
						foreach (PrdSerialBusiness item in items)
						{
							PrdSerial item2 = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item2, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 133);
							if (!ret.HasError && (item.DetList?.Any() ?? false))
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetList, item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 135);
							}
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 142);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 143);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PrdSerialBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				PrdSerial res = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 156));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(res, res.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 157);
				if (ret.HasError)
				{
					return ret;
				}
				IQueryable<PrdSerialDetails> queryable = ((IQueryable<PrdSerialDetails>)dbcontext.PrdSerialDetails).Where((PrdSerialDetails q) => q.MasterGuid == item.Guid);
				if (queryable != null && queryable.Any())
				{
					List<Guid> existGuidList = item.DetList?.Select((PrdSerialDetailBusiness q) => q.Guid)?.ToList();
					if (existGuidList?.Any() ?? false)
					{
						IQueryable<PrdSerialDetails> queryable2 = queryable.Where((PrdSerialDetails q) => !existGuidList.Contains(q.Guid));
						if (queryable2 != null && queryable2.Any())
						{
							dbcontext.PrdSerialDetails.RemoveRange((IEnumerable<PrdSerialDetails>)queryable2);
						}
					}
				}
				if (ret.HasError || !(item.DetList?.Any() ?? false))
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 171);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 175);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 176);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 184));
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				IQueryable<PrdSerialDetails> queryable = ((IQueryable<PrdSerialDetails>)acc_ModelContext.PrdSerialDetails).Where((PrdSerialDetails x) => x.DetailGuid == asGuid && x.AsType == asType);
				if (queryable == null || !queryable.Any())
				{
					return returnedSaveFuncInfo;
				}
				queryable?.Select((PrdSerialDetails q) => q.MasterGuid)?.Distinct();
				((DbContext)acc_ModelContext).Set<PrdSerialDetails>().RemoveRange((IEnumerable<PrdSerialDetails>)queryable);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 196);
				returnedSaveFuncInfo.AddReturnedValue(ex2, "", "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 197);
			}
			return returnedSaveFuncInfo;
		}
	}

	public abstract Task<IEnumerable<PrdSerialDetailReportBusiness>> GetAllAsync(string serial, CancellationToken token, string tranName);

	public virtual Task<IEnumerable<PrdSerialBusiness>> GetAllWithoutDetailAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 205));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				List<PrdSerial> list = await EntityFrameworkQueryableExtensions.ToListAsync<PrdSerial>(((IQueryable<PrdSerial>)c.PrdSerials).Where((PrdSerial q) => q.ServerStatus == ServerStatusForClient.None || q.ServerStatus == ServerStatusForClient.SendError), token);
				result = ((list != null && list.Any()) ? list.Select((PrdSerial q) => q.Guid).ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 216);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<ProductSerialTechnicalInfoSuggestModel>> GetAllTechnicalSuggestionAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllTechnicalSuggestionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 227));
		IEnumerable<ProductSerialTechnicalInfoSuggestModel> result;
		try
		{
			_ = 1;
			try
			{
				List<PrdSerial> list = await EntityFrameworkQueryableExtensions.ToListAsync<PrdSerial>(((IQueryable<PrdSerial>)c.PrdSerials).Where((PrdSerial q) => !string.IsNullOrEmpty(q.TechnicalInfo1) || !string.IsNullOrEmpty(q.TechnicalInfo2) || !string.IsNullOrEmpty(q.TechnicalInfo3)), token);
				if (list == null || !list.Any())
				{
					result = null;
				}
				else
				{
					IEnumerable<ProductSerialTechnicalInfoSuggestModel> enumerable = list.Select((PrdSerial q) => new ProductSerialTechnicalInfoSuggestModel
					{
						Ordinal = 1,
						Value = q.TechnicalInfo1
					});
					IEnumerable<ProductSerialTechnicalInfoSuggestModel> enumerable2 = list.Select((PrdSerial q) => new ProductSerialTechnicalInfoSuggestModel
					{
						Ordinal = 2,
						Value = q.TechnicalInfo2
					});
					IEnumerable<ProductSerialTechnicalInfoSuggestModel> enumerable3 = list.Select((PrdSerial q) => new ProductSerialTechnicalInfoSuggestModel
					{
						Ordinal = 3,
						Value = q.TechnicalInfo3
					});
					List<ProductSerialTechnicalInfoSuggestModel> list2 = new List<ProductSerialTechnicalInfoSuggestModel>();
					if (enumerable != null && enumerable.Any())
					{
						list2.AddRange(enumerable);
					}
					if (enumerable2 != null && enumerable2.Any())
					{
						list2.AddRange(enumerable2);
					}
					if (enumerable3 != null && enumerable3.Any())
					{
						list2.AddRange(enumerable3);
					}
					result = list2;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllTechnicalSuggestionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs", 259);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllTechnicalSuggestionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepositoryBase.cs");
			}
		}
		return result;
	}

	public virtual Task<IEnumerable<PrdSerialReportBusiness>> GetAllModelAsync(ProductSerialFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
