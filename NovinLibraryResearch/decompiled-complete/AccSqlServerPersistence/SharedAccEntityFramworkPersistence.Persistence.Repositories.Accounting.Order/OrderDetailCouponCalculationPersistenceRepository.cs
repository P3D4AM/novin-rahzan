using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.OrderDepot.OrderCouponCalculation;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Order;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Order;

internal class OrderDetailCouponCalculationPersistenceRepository : PersistenceRepository<OrderDetailCouponCalculationBusiness, OrderDetailCouponCalculation>, IOrderDetailCouponCalculationRepository, IRepository<OrderDetailCouponCalculationBusiness>, INoGuidRepository<OrderDetailCouponCalculationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<OrderDetailCouponCalculationBusiness, OrderDetailCouponCalculation>, IItemAsyncMapper<OrderDetailCouponCalculation, OrderDetailCouponCalculationBusiness>
	{
		public Task<OrderDetailCouponCalculation> MapAsync(string contextGuid, OrderDetailCouponCalculationBusiness source, OrderDetailCouponCalculation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OrderDetailCouponCalculation();
				}
				destination.Guid = source.Guid;
				destination.ProductGuid = source.ProductGuid;
				destination.Quantity = source.Quantity;
				destination.Price = source.Price;
				destination.Discount = source.Discount;
				destination.Modified = source.Modified;
				destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
				destination.RowSum = source.RowSum;
				destination.RefrencedId = source.RefrencedId;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 121);
				throw;
			}
		}

		public Task<OrderDetailCouponCalculationBusiness> MapAsync(string contextGuid, OrderDetailCouponCalculation source, OrderDetailCouponCalculationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OrderDetailCouponCalculationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.ProductGuid = source.ProductGuid;
				destination.Quantity = source.Quantity;
				destination.Price = source.Price;
				destination.Discount = source.Discount;
				destination.Modified = source.Modified;
				destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
				destination.RowSum = source.RowSum;
				destination.RefrencedId = source.RefrencedId;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 151);
				throw;
			}
		}

		public Task<List<OrderDetailCouponCalculation>> MapListAsync(string contextGuid, IEnumerable<OrderDetailCouponCalculationBusiness> source, List<OrderDetailCouponCalculation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OrderDetailCouponCalculationBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailCouponCalculation> source, List<OrderDetailCouponCalculationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<OrderDetailCouponCalculation, OrderDetailCouponCalculationBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<OrderDetailCouponCalculation, OrderDetailCouponCalculationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<OrderDetailCouponCalculationBusiness, OrderDetailCouponCalculation> MapToDatabase => Mapper;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public OrderDetailCouponCalculationPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 40);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrencedAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		using (new JaegerMonitor("GetNextRefrencedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextRefrencedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 46));
			try
			{
				int max = 0;
				if (((IQueryable<OrderDetailCouponCalculation>)acc_ModelContext.OrderDetailCouponCalculation).Count() > 0)
				{
					max = ((IQueryable<OrderDetailCouponCalculation>)acc_ModelContext.OrderDetailCouponCalculation)?.Max((OrderDetailCouponCalculation q) => q.RefrencedId) ?? 0;
				}
				IQueryable<OrderDetailCouponCalculation> orderDetailCouponCalculation;
				if (max > 1000)
				{
					orderDetailCouponCalculation = (IQueryable<OrderDetailCouponCalculation>)acc_ModelContext.OrderDetailCouponCalculation;
					if (orderDetailCouponCalculation != null && orderDetailCouponCalculation.Any())
					{
						acc_ModelContext.OrderDetailCouponCalculation.RemoveRange((IEnumerable<OrderDetailCouponCalculation>)orderDetailCouponCalculation);
					}
					max = 0;
				}
				orderDetailCouponCalculation = ((IQueryable<OrderDetailCouponCalculation>)acc_ModelContext.OrderDetailCouponCalculation).Where((OrderDetailCouponCalculation q) => q.RefrencedId < max - 5);
				if (orderDetailCouponCalculation != null && orderDetailCouponCalculation.Any())
				{
					acc_ModelContext.OrderDetailCouponCalculation.RemoveRange((IEnumerable<OrderDetailCouponCalculation>)orderDetailCouponCalculation);
				}
				ret.value = max + 1;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNextRefrencedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 65);
				ret.AddReturnedValue(ex, "", "GetNextRefrencedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 66);
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetNextRefrencedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs");
				}
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<OrderDetailCouponCalculationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 82));
				await ((DbContext)c).AddRangeAsync((IEnumerable<object>)(await MapToDatabase.MapListAsync(ContextGuid, items, null, tranName, token)), token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 88);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetailCouponCalculationPersistenceRepository.cs", 89);
			}
			return ret;
		}
	}
}
