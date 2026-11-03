using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Business.Accounting.Hotel.ReportsT;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData.Hotel;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class ReceiptPersistenceRepository : PersistenceRepository<ReceiptBusiness, Receipt>, IReceiptRepository, IRepository<ReceiptBusiness>, INoGuidRepository<ReceiptBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReceiptBusiness, Receipt>, IItemAsyncMapper<Receipt, ReceiptBusiness>
	{
		public Task<Receipt> MapAsync(string contextGuid, ReceiptBusiness source, Receipt destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Receipt();
				}
				destination.Guid = source.Guid;
				destination.PresenterGuid = source.PresenterGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.CustomerGuid = source.CustomerGuid;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Number = source.Number;
				destination.Discount = source.Discount;
				destination.Penalty = source.Penalty;
				destination.PepolCount = source.PepolCount;
				destination.DateM = source.DateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 236);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ReceiptBusiness> MapAsync(string contextGuid, Receipt source, ReceiptBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceiptBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.PresenterGuid = source.PresenterGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.CustomerGuid = source.CustomerGuid;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Number = source.Number;
				destination.Discount = source.Discount;
				destination.PepolCount = source.PepolCount;
				destination.DateM = source.DateM;
				destination.ChangeStatus = EntityChangeSt.Edited;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<ReceiptMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 265);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 271);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Receipt>> MapListAsync(string contextGuid, IEnumerable<ReceiptBusiness> source, List<Receipt> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReceiptBusiness>> MapListAsync(string contextGuid, IEnumerable<Receipt> source, List<ReceiptBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private ReceiptDetailPersistenceRepository _detRepo;

	private CustomerReceiptRelationPersistenceRepository _customerRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Receipt, ReceiptBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Receipt, ReceiptBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReceiptBusiness, Receipt> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public ReceiptPersistenceRepository(ContextHolder db)
		: base(db)
	{
		try
		{
			_detRepo = new ReceiptDetailPersistenceRepository(db);
			_customerRepo = new CustomerReceiptRelationPersistenceRepository(db);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 44);
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
				_detRepo?.Dispose();
				_detRepo = null;
				_customerRepo?.Dispose();
				_customerRepo = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 62);
		}
	}

	public override async Task<ReceiptBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 66));
		ReceiptBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 4;
			try
			{
				Receipt receipt = await c.Receipt.FindAsync(new object[1] { guid });
				if (receipt == null)
				{
					result = null;
				}
				else
				{
					ReceiptBusiness mappedData = await MapFromDatabase.MapAsync(ContextGuid, receipt, null, tranName, token);
					IEnumerable<CustomerReceiptRelationBusiness> enumerable = await _customerRepo.GetAllAsync(guid, token, tranName);
					if (enumerable != null && enumerable.Any())
					{
						await mappedData.AddRangeToCustomerListAsync(enumerable, clearList: true);
					}
					await _detRepo.GetAllAsync(mappedData, token, tranName);
					result = mappedData;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 83);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ReceiptBusiness recipt, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName) || recipt.Guid == Guid.Empty)
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs"))
		{
			_ = 7;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 99));
				Receipt receipt = await MapToDatabase.MapAsync(ContextGuid, recipt, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(receipt, receipt.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 101);
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(recipt.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 103);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _customerRepo.RemoveRangeAsync(recipt.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 104);
				await ((DbContext)dbcontext).SaveChangesAsync(token);
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(recipt.ReceiptDetailList?.ToList(), recipt.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 107);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _customerRepo.SaveRangeAsync(recipt.CustomerList?.ToList(), tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 108);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 113);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 114);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs"))
		{
			_ = 3;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 125));
				Receipt recipt = await dbcontext.Receipt.FindAsync(new object[1] { guid });
				if (recipt == null)
				{
					return res;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 128);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _customerRepo.RemoveRangeAsync(recipt.Guid, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 129);
				if (res.HasError)
				{
					return res;
				}
				dbcontext.Receipt.Remove(recipt);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 136);
				res.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 137);
			}
			return res;
		}
	}

	public async Task<int> GetNextNumberRecipt(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextNumberRecipt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 143));
		using (new JaegerMonitor("GetNextNumberRecipt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Receipt>(((IQueryable<Receipt>)c.Receipt).OrderByDescending((Receipt x) => x.Number).Take(1), token))?.Number ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNextNumberRecipt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 154);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextNumberRecipt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<ReceiptReportBusiness>> GetAllAsync(ReceiptFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 165));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs"))
		{
			IEnumerable<ReceiptReportBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Receipt> queryable = (IQueryable<Receipt>)c.Receipt;
					if (filter != null)
					{
						if (filter.DateFrom.HasValue)
						{
							queryable = queryable?.Where((Receipt q) => q.DateM >= filter.DateFrom);
						}
						if (filter.DateTo.HasValue)
						{
							queryable = queryable?.Where((Receipt q) => q.DateM >= filter.DateTo);
						}
						if (filter.CustomerGuid.HasValue)
						{
							queryable = queryable?.Where((Receipt q) => q.CustomerGuid == filter.CustomerGuid);
						}
					}
					result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<ReceiptReportBusiness>(queryable.Select((Receipt q) => new ReceiptReportBusiness
					{
						CustomerName = q.Tafsil.Name,
						DateM = q.DateM,
						Description = q.Description,
						Discount = q.Discount,
						Guid = q.Guid,
						Number = q.Number,
						ProsenterName = q.PresenterTafsil.Name,
						TotalPrice = q.ReceiptDetails.Sum((ReceiptDetail p) => p.TotalPrice),
						UserName = q.Order.CashierItem.Name,
						TafsilGuid = q.CustomerGuid,
						PeopleCount = q.CustomerReceipt.Count()
					}), token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs", 196);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ReceiptPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
