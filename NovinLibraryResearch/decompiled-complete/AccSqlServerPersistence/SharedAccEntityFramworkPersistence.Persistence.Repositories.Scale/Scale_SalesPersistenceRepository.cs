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

internal class Scale_SalesPersistenceRepository : PersistenceRepository<Scale_SalesBusiness, Scale_Sales>, IScale_SalesRepository, IRepository<Scale_SalesBusiness>, INoGuidRepository<Scale_SalesBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Sales, Scale_SalesBusiness>, IItemAsyncMapper<Scale_SalesBusiness, Scale_Sales>
	{
		public Task<Scale_SalesBusiness> MapAsync(string contextGuid, Scale_Sales source, Scale_SalesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_SalesBusiness(contextGuid);
				}
				destination.TotalBarcode = source.TotalBarcode;
				destination.MemoryGuid = source.MemoryGuid;
				destination.DateM = source.DateM;
				destination.DateSH = source.DateSH;
				destination.ReadDateM = source.ReadDateM;
				destination.MyProperty = source.MyProperty;
				destination.ReadDateSH = source.ReadDateSH;
				destination.PortIP = source.PortIP;
				destination.Count = source.Count;
				destination.Sum = source.Sum;
				destination.Tax = source.Tax;
				destination.Discount = source.Discount;
				destination.TotalSum = source.TotalSum;
				destination.ScaleNumber = source.ScaleNumber;
				destination.ScaleGuid = source.ScaleGuid;
				destination.FactorNumber = source.FactorNumber;
				destination.ClerkNumber = source.ClerkNumber;
				destination.ScaleName = source.ScaleName;
				destination.AccountingSum = source.AccountingSum;
				destination.AccountingDiscount = source.AccountingDiscount;
				destination.AccountingTax = source.AccountingTax;
				destination.AccountingTotalSum = source.AccountingTotalSum;
				destination.AccountingRegisterGuid = source.AccountingRegisterGuid;
				destination.AccountingRegisterTime = source.AccountingRegisterTime;
				destination.CustomerFullName = source.CustomerFullName;
				destination.AccountingFactorNumber = source.AccountingFactorNumber;
				destination.AccountingRegisterStatus = source.AccountingRegisterStatus;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SalesPersistenceRepository.cs", 90);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Sales> MapAsync(string contextGuid, Scale_SalesBusiness source, Scale_Sales destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Sales();
				}
				destination.TotalBarcode = source.TotalBarcode;
				destination.MemoryGuid = source.MemoryGuid;
				destination.DateM = source.DateM;
				destination.DateSH = source.DateSH;
				destination.ReadDateM = source.ReadDateM;
				destination.MyProperty = source.MyProperty;
				destination.ReadDateSH = source.ReadDateSH;
				destination.PortIP = source.PortIP;
				destination.Count = source.Count;
				destination.Sum = source.Sum;
				destination.Tax = source.Tax;
				destination.Discount = source.Discount;
				destination.TotalSum = source.TotalSum;
				destination.ScaleNumber = source.ScaleNumber;
				destination.ScaleGuid = source.ScaleGuid;
				destination.FactorNumber = source.FactorNumber;
				destination.ClerkNumber = source.ClerkNumber;
				destination.ScaleName = source.ScaleName;
				destination.AccountingSum = source.AccountingSum;
				destination.AccountingDiscount = source.AccountingDiscount;
				destination.AccountingTax = source.AccountingTax;
				destination.AccountingTotalSum = source.AccountingTotalSum;
				destination.AccountingRegisterGuid = source.AccountingRegisterGuid;
				destination.AccountingRegisterTime = source.AccountingRegisterTime;
				destination.CustomerFullName = source.CustomerFullName;
				destination.AccountingFactorNumber = source.AccountingFactorNumber;
				destination.AccountingRegisterStatus = source.AccountingRegisterStatus;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SalesPersistenceRepository.cs", 140);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_SalesBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Sales> source, List<Scale_SalesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Sales>> MapListAsync(string contextGuid, IEnumerable<Scale_SalesBusiness> source, List<Scale_Sales> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Sales, Scale_SalesBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Sales, Scale_SalesBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_SalesBusiness, Scale_Sales> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_SalesPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SalesPersistenceRepository.cs", 40);
		}
	}
}
