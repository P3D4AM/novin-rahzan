using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Scale;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Scale;

internal class Scale_SaleDetailPersistenceRepository : PersistenceRepository<Scale_SaleDetailBusiness, Scale_SaleDetail>, IScale_SaleDetailRepository, IRepository<Scale_SaleDetailBusiness>, INoGuidRepository<Scale_SaleDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_SaleDetail, Scale_SaleDetailBusiness>, IItemAsyncMapper<Scale_SaleDetailBusiness, Scale_SaleDetail>
	{
		public Task<Scale_SaleDetailBusiness> MapAsync(string contextGuid, Scale_SaleDetail source, Scale_SaleDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_SaleDetailBusiness(contextGuid);
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.ProductCode = source.ProductCode;
				destination.ProductName = source.ProductName;
				destination.PluNumber = source.PluNumber;
				destination.UnitName = source.UnitName;
				destination.UnitPrice = source.UnitPrice;
				destination.Discount = source.Discount;
				destination.Tax = source.Tax;
				destination.TotalPrice = source.TotalPrice;
				destination.QT = source.QT;
				destination.IM = source.IM;
				destination.Order = source.Order;
				destination.AccountingUnitPrice = source.AccountingUnitPrice;
				destination.AccountingDiscount = source.AccountingDiscount;
				destination.MyProperty = source.MyProperty;
				destination.AccountingTax = source.AccountingTax;
				destination.AccountingTotalPrice = source.AccountingTotalPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SaleDetailPersistenceRepository.cs", 85);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_SaleDetail> MapAsync(string contextGuid, Scale_SaleDetailBusiness source, Scale_SaleDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_SaleDetail();
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.ProductCode = source.ProductCode;
				destination.ProductName = source.ProductName;
				destination.PluNumber = source.PluNumber;
				destination.UnitName = source.UnitName;
				destination.UnitPrice = source.UnitPrice;
				destination.Discount = source.Discount;
				destination.Tax = source.Tax;
				destination.TotalPrice = source.TotalPrice;
				destination.QT = source.QT;
				destination.IM = source.IM;
				destination.Order = source.Order;
				destination.AccountingUnitPrice = source.AccountingUnitPrice;
				destination.AccountingDiscount = source.AccountingDiscount;
				destination.MyProperty = source.MyProperty;
				destination.AccountingTax = source.AccountingTax;
				destination.AccountingTotalPrice = source.AccountingTotalPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SaleDetailPersistenceRepository.cs", 124);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_SaleDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_SaleDetail> source, List<Scale_SaleDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_SaleDetail>> MapListAsync(string contextGuid, IEnumerable<Scale_SaleDetailBusiness> source, List<Scale_SaleDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_SaleDetail, Scale_SaleDetailBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_SaleDetail, Scale_SaleDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_SaleDetailBusiness, Scale_SaleDetail> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_SaleDetailPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SaleDetailPersistenceRepository.cs", 45);
		}
	}
}
