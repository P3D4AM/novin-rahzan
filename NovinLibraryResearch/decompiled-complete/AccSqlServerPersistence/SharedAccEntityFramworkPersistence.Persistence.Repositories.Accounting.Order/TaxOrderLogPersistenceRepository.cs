using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.OrderDepot;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Order;

internal class TaxOrderLogPersistenceRepository : PersistenceSyncRepository<TaxOrderLogBusiness, TaxOrderLog>, ITaxOrderLogRepository, ISyncRepository<TaxOrderLogBusiness>, IEditableDelivery, IRepository<TaxOrderLogBusiness>, INoGuidRepository<TaxOrderLogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TaxOrderLogBusiness, TaxOrderLog>, IItemAsyncMapper<TaxOrderLog, TaxOrderLogBusiness>, IItemAsyncMapper<TaxOrderLog, TaxOrderLogReportBusiness>
	{
		public Task<TaxOrderLog> MapAsync(string contextGuid, TaxOrderLogBusiness source, TaxOrderLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TaxOrderLog();
				}
				destination.Guid = source.Guid;
				destination.OrderGuid = source.OrderGuid;
				destination.TaxSendDate = source.TaxSendDate;
				destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
				destination.TaxResultErrorCode = source.TaxResultErrorCode;
				destination.TaxResultMessage = source.TaxResultMessage;
				destination.TaxUid = source.TaxUid;
				destination.TaxOrderId = source.TaxOrderId;
				destination.InsertDate = source.InsertDate;
				destination.IsSystem = source.IsSystem;
				destination.OldState = source.OldState;
				destination.NewState = source.NewState;
				destination.Subject = source.Subject;
				destination.TaxDeliveryDate = source.TaxDeliveryDate;
				destination.DeliveryDate = source.DeliveryDate;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Ordinal = source.Ordinal;
				destination.BaseTaxOrderId = source.BaseTaxOrderId;
				destination.InvoiceSeries = source.InvoiceSeries;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 183);
				throw;
			}
		}

		public Task<TaxOrderLogBusiness> MapAsync(string contextGuid, TaxOrderLog source, TaxOrderLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TaxOrderLogBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.OrderGuid = source.OrderGuid;
				destination.TaxSendDate = source.TaxSendDate;
				destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
				destination.TaxResultErrorCode = source.TaxResultErrorCode;
				destination.TaxResultMessage = source.TaxResultMessage;
				destination.TaxUid = source.TaxUid;
				destination.TaxOrderId = source.TaxOrderId;
				destination.InsertDate = source.InsertDate;
				destination.IsSystem = source.IsSystem;
				destination.OldState = source.OldState;
				destination.NewState = source.NewState;
				destination.Subject = source.Subject;
				destination.TaxDeliveryDate = source.TaxDeliveryDate;
				destination.DeliveryDate = source.DeliveryDate;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Ordinal = source.Ordinal;
				destination.BaseTaxOrderId = source.BaseTaxOrderId;
				destination.InvoiceSeries = source.InvoiceSeries;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 227);
				throw;
			}
		}

		public Task<TaxOrderLogReportBusiness> MapAsync(string contextGuid, TaxOrderLog source, TaxOrderLogReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TaxOrderLogReportBusiness();
				}
				destination.Guid = source.Guid;
				destination.OrderGuid = source.OrderGuid;
				destination.TaxSendDate = source.TaxSendDate;
				destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
				destination.TaxDeliveryDate = source.TaxDeliveryDate;
				destination.TaxResultErrorCode = source.TaxResultErrorCode;
				destination.TaxResultMessage = source.TaxResultMessage;
				destination.TaxUid = source.TaxUid;
				destination.TaxOrderId = source.TaxOrderId;
				destination.InsertDate = source.InsertDate;
				destination.OldState = source.OldState;
				destination.NewState = source.NewState;
				destination.Subject = source.Subject;
				destination.TotalSum = source.OrderItem.TotalSum;
				destination.Discount = source.OrderItem.Discount;
				destination.Tax = source.OrderItem.Tax;
				destination.OrderDate = source.OrderItem.Date;
				destination.TafsilName = source.OrderItem.CustomerFullName;
				destination.TafsilGuid = source.OrderItem.TafsilGuid;
				destination.Id2 = source.OrderItem.Id2;
				destination.BaseTaxOrderId = source.BaseTaxOrderId;
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 269);
				throw;
			}
		}

		public Task<List<TaxOrderLog>> MapListAsync(string contextGuid, IEnumerable<TaxOrderLogBusiness> source, List<TaxOrderLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TaxOrderLogBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxOrderLog> source, List<TaxOrderLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TaxOrderLogReportBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxOrderLog> source, List<TaxOrderLogReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<TaxOrderLog, TaxOrderLogBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<TaxOrderLog, TaxOrderLogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TaxOrderLogBusiness, TaxOrderLog> MapToDatabase => Mapper;

	public IItemAsyncMapper<TaxOrderLog, TaxOrderLogReportBusiness> MapToReport => Mapper;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public new bool IsDisposed { get; private set; }

	public TaxOrderLogPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 44);
		}
	}

	public async Task<IEnumerable<TaxOrderLogBusiness>> GetAllAsync(TaxOrderLogFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 48));
		IEnumerable<TaxOrderLogBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<TaxOrderLog> queryable = (IQueryable<TaxOrderLog>)c.TaxOrderLog;
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					if (filter != null)
					{
						if ((filter?.OrderGuid ?? Guid.Empty) != Guid.Empty)
						{
							queryable = queryable?.Where((TaxOrderLog q) => q.OrderGuid == filter.OrderGuid.Value);
						}
						if (!string.IsNullOrEmpty(filter?.TaxOrderId))
						{
							queryable = queryable?.Where((TaxOrderLog q) => q.TaxOrderId == filter.TaxOrderId);
						}
						if (!string.IsNullOrEmpty(filter?.TaxRefrencedNumber))
						{
							queryable = queryable?.Where((TaxOrderLog q) => q.TaxRefrenceNumber == filter.TaxRefrencedNumber);
						}
					}
					result = (await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token))?.OrderBy((TaxOrderLogBusiness q) => q.InsertDate);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 66);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<TaxOrderLogReportBusiness>> GetAllReportAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 77));
		IEnumerable<TaxOrderLogReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<TaxOrderLog> list = await EntityFrameworkQueryableExtensions.ToListAsync<TaxOrderLog>(((IQueryable<TaxOrderLog>)EntityFrameworkQueryableExtensions.Include<TaxOrderLog, AccSqlServerPersistence.Entities.Order>((IQueryable<TaxOrderLog>)c.TaxOrderLog, (Expression<Func<TaxOrderLog, AccSqlServerPersistence.Entities.Order>>)((TaxOrderLog q) => q.OrderItem))).Where((TaxOrderLog q) => q.OrderGuid == orderGuid), token);
				result = ((list != null && list.Any()) ? (await MapToReport.MapListAsync(ContextGuid, list, null, tranName, token))?.OrderBy((TaxOrderLogReportBusiness q) => q.InsertDate) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 89);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangAsync(List<Guid> lstGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs"))
		{
			_ = 1;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 105));
				List<TaxOrderLog> list = await EntityFrameworkQueryableExtensions.ToListAsync<TaxOrderLog>(((IQueryable<TaxOrderLog>)dbcontext.TaxOrderLog).Where((TaxOrderLog q) => lstGuid.Contains(q.Guid)), token);
				if (list == null || !list.Any())
				{
					return res;
				}
				foreach (TaxOrderLog item in list)
				{
					((EntityEntry)((DbContext)dbcontext).Entry<TaxOrderLog>(item)).State = (EntityState)2;
				}
				dbcontext.TaxOrderLog.RemoveRange((IEnumerable<TaxOrderLog>)list);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 113);
				res.AddReturnedValue(ex, "", "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 114);
			}
			return res;
		}
	}

	public async Task<short> GetNextOrdinalAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 120));
		using (new JaegerMonitor("GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs"))
		{
			short result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<TaxOrderLog> queryable = ((IQueryable<TaxOrderLog>)c.TaxOrderLog).Where((TaxOrderLog q) => q.OrderGuid == orderGuid);
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IOrderedQueryable<TaxOrderLog> orderedQueryable = queryable.OrderByDescending((TaxOrderLog x) => x.Ordinal);
						if (orderedQueryable == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<TaxOrderLog> queryable2 = orderedQueryable.Take(1);
							obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<TaxOrderLog>(queryable2, token) : null);
						}
					}
					result = (short)(((await (Task<TaxOrderLog>)obj)?.Ordinal ?? 0) + 1);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs", 132);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\TaxOrderLogPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
