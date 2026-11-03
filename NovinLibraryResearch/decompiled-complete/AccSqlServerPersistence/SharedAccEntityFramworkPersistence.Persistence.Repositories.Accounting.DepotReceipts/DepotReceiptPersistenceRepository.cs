using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.DepotReceipt;
using EntityCache.Models.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.DepotReceipts;

internal class DepotReceiptPersistenceRepository : PersistenceRepository<DepotReceiptBusiness, DepotReceipt>, IDepotReceiptRepository, IRepository<DepotReceiptBusiness>, INoGuidRepository<DepotReceiptBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DepotReceipt, DepotReceiptBusiness>, IItemAsyncMapper<DepotReceiptBusiness, DepotReceipt>, IItemAsyncMapper<DepotReceipt, DepotReceiptReportBusiness>
	{
		public Task<DepotReceiptBusiness> MapAsync(string contextGuid, DepotReceipt source, DepotReceiptBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DepotReceiptBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.Id2 = source.Id2;
				destination.DateM = source.DateM;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Description = source.Description;
				destination.IsTemp = source.IsTemp;
				destination.CashierGuid = source.CashierGuid;
				destination.CarNumber = source.CarNumber;
				destination.DriverName = source.DriverName;
				destination.Type = source.Type;
				destination.BarnameNo = source.BarnameNo;
				destination.DriverPhone = source.DriverPhone;
				destination.RefrencedNumber = source.RefrencedNumber;
				destination.OrderGuid = source.OrderGuid;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 291);
				throw;
			}
		}

		public Task<DepotReceipt> MapAsync(string contextGuid, DepotReceiptBusiness source, DepotReceipt destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DepotReceipt();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.Id2 = source.Id2;
				destination.DateM = source.DateM;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Description = source.Description;
				destination.IsTemp = source.IsTemp;
				destination.CashierGuid = source.CashierGuid;
				destination.CarNumber = source.CarNumber;
				destination.DriverName = source.DriverName;
				destination.Type = source.Type;
				destination.BarnameNo = source.BarnameNo;
				destination.DriverPhone = source.DriverPhone;
				destination.RefrencedNumber = source.RefrencedNumber;
				destination.OrderGuid = source.OrderGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 328);
				throw;
			}
		}

		public Task<DepotReceiptReportBusiness> MapAsync(string contextGuid, DepotReceipt source, DepotReceiptReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DepotReceiptReportBusiness();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.Id2 = source.Id2;
				destination.DateM = source.DateM;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Description = source.Description;
				destination.IsTemp = source.IsTemp;
				destination.CashierGuid = source.CashierGuid;
				destination.CarNumber = source.CarNumber;
				destination.DriverName = source.DriverName;
				destination.Type = source.Type;
				destination.BarnameNo = source.BarnameNo;
				destination.DriverPhone = source.DriverPhone;
				destination.RefrencedNumber = source.RefrencedNumber;
				destination.CashierName = source.CashierItem?.Name ?? "";
				destination.TafsilName = source.TafsilItem?.Name ?? "";
				destination.OrderGuid = source.OrderGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 367);
				throw;
			}
		}

		public Task<List<DepotReceiptBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotReceipt> source, List<DepotReceiptBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DepotReceipt>> MapListAsync(string contextGuid, IEnumerable<DepotReceiptBusiness> source, List<DepotReceipt> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DepotReceiptReportBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotReceipt> source, List<DepotReceiptReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private DepotReceiptDetailPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<DepotReceipt, DepotReceiptBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DepotReceiptBusiness, DepotReceipt> MapToDatabase => Mapper;

	public IItemAsyncMapper<DepotReceipt, DepotReceiptReportBusiness> MapFromDatabaseToReport { get; set; } = new mapper();


	public override IItemMapper<DepotReceipt, DepotReceiptBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public new bool IsDisposed { get; private set; }

	public DepotReceiptPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
		_detRepo = new DepotReceiptDetailPersistenceRepository(dbContext);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				_detRepo?.Dispose();
				_detRepo = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 50);
		}
	}

	public async Task<int> GetNextNumberAsync(EnDepotReceiptType type, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 54));
		using (new JaegerMonitor("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<DepotReceipt>((from q in (IQueryable<DepotReceipt>)c.DepotReceipt
						where q.Type == type
						select q into x
						orderby x.Id2 descending
						select x).Take(1), token))?.Id2 ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 66);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<Guid> GetGuidById2Async(EnDepotReceiptType type, int id2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetGuidById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 77));
		Guid result;
		try
		{
			_ = 1;
			try
			{
				result = (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<DepotReceipt>((IQueryable<DepotReceipt>)c.DepotReceipt, (Expression<Func<DepotReceipt, bool>>)((DepotReceipt x) => x.Type == type && x.Id2 == id2), token))?.Guid ?? Guid.Empty;
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetGuidById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 88);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetGuidById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(DepotReceiptBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 104));
				DepotReceipt depotReceipt = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(depotReceipt, depotReceipt.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 106);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 108);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetailList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 110);
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
				WebErrorLog.Instance.StartLog(ex3, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 117);
				ret.AddReturnedValue(ex3, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 118);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<DepotReceiptReportBusiness>> GetAllAsync(DepotReceiptFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 124));
		IEnumerable<DepotReceiptReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<DepotReceipt> queryable = (IQueryable<DepotReceipt>)EntityFrameworkQueryableExtensions.Include<DepotReceipt, Cashier>((IQueryable<DepotReceipt>)EntityFrameworkQueryableExtensions.Include<DepotReceipt, Tafsil>((IQueryable<DepotReceipt>)EntityFrameworkQueryableExtensions.Include<DepotReceipt, ICollection<DepotReceiptDetail>>((IQueryable<DepotReceipt>)c.DepotReceipt, (Expression<Func<DepotReceipt, ICollection<DepotReceiptDetail>>>)((DepotReceipt q) => q.DepotReceiptDetail)), (Expression<Func<DepotReceipt, Tafsil>>)((DepotReceipt q) => q.TafsilItem)), (Expression<Func<DepotReceipt, Cashier>>)((DepotReceipt q) => q.CashierItem));
				if (filter != null)
				{
					queryable = queryable?.Where((DepotReceipt q) => q.Type == filter.Type);
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((DepotReceipt q) => q.DateM >= filter.DateFrom.Value);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((DepotReceipt q) => q.DateM <= filter.DateTo.Value);
					}
					if (filter.IsTemp.HasValue)
					{
						queryable = queryable?.Where((DepotReceipt q) => q.IsTemp == filter.IsTemp.Value);
					}
					if ((filter?.TafsilGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((DepotReceipt q) => q.TafsilGuid == filter.TafsilGuid.Value);
					}
					if ((filter?.CashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((DepotReceipt q) => q.CashierGuid == filter.CashierGuid.Value);
					}
					if (filter.SanadAffective.HasValue)
					{
						queryable = ((!filter.SanadAffective.GetValueOrDefault()) ? queryable?.Where((DepotReceipt q) => q.DepotReceiptDetail != null && q.DepotReceiptDetail.Any((DepotReceiptDetail w) => (w.ReferenceGuid ?? Guid.Empty) == Guid.Empty)) : queryable?.Where((DepotReceipt q) => q.DepotReceiptDetail != null && q.DepotReceiptDetail.Any((DepotReceiptDetail w) => (w.ReferenceGuid ?? Guid.Empty) != Guid.Empty)));
					}
				}
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabaseToReport.MapListAsync(ContextGuid, queryable, null, tranName, token))?.OrderBy((DepotReceiptReportBusiness q) => q.DateM) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 152);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<DepotReceiptBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 163));
		DepotReceiptBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				DepotReceipt depotReceipt = await c.DepotReceipt.FindAsync(new object[1] { guid });
				if (depotReceipt == null)
				{
					result = null;
				}
				else
				{
					DepotReceiptBusiness mappedItem = await MapFromDatabase.MapAsync(ContextGuid, depotReceipt, null, tranName, token);
					await _detRepo.GetAllAsync(mappedItem, token, tranName);
					result = mappedItem;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 176);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 192));
				DepotReceipt depotReceipt = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<DepotReceipt>((IQueryable<DepotReceipt>)c.DepotReceipt, (Expression<Func<DepotReceipt, bool>>)((DepotReceipt p) => p.Guid == guid), token);
				if (depotReceipt == null)
				{
					return res;
				}
				c.DepotReceipt.Remove(depotReceipt);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 196);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 201);
				res.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 202);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateRefrenceGuidAsync(List<SanadAttchmentModel> model, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(model?.Any() ?? false))
				{
					return res;
				}
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 214));
				foreach (SanadAttchmentModel item in model)
				{
					if (item != null)
					{
						DepotReceiptDetail depotReceiptDetail = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<DepotReceiptDetail>((IQueryable<DepotReceiptDetail>)c.DepotReceiptDetail, (Expression<Func<DepotReceiptDetail, bool>>)((DepotReceiptDetail q) => q.Guid == item.AsGuid), token);
						depotReceiptDetail.ReferenceGuid = item.SanadDetailGuid;
						depotReceiptDetail.Modified = DateTime.Now;
						((DbContext)c).Update<DepotReceiptDetail>(depotReceiptDetail);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 227);
				res.AddReturnedValue(ex2, "", "UpdateRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 228);
			}
			return res;
		}
	}

	public async Task<List<Guid>> GetAllByOrderGuidAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 234));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				if (orderGuid == Guid.Empty)
				{
					result = null;
				}
				else
				{
					List<DepotReceipt> list = await EntityFrameworkQueryableExtensions.ToListAsync<DepotReceipt>(((IQueryable<DepotReceipt>)c.DepotReceipt).Where((DepotReceipt q) => q.OrderGuid == orderGuid), token);
					result = ((list != null && list.Any()) ? list.Select((DepotReceipt q) => q.Guid).ToList() : null);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs", 244);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptPersistenceRepository.cs");
			}
		}
		return result;
	}
}
