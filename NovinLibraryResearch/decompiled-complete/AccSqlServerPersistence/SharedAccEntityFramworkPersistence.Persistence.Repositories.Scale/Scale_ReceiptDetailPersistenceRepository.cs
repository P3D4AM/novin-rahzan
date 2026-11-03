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

internal class Scale_ReceiptDetailPersistenceRepository : PersistenceRepository<Scale_ReceiptDetailBusiness, Scale_ReceiptDetail>, IScale_ReceiptDetailRepository, IRepository<Scale_ReceiptDetailBusiness>, INoGuidRepository<Scale_ReceiptDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_ReceiptDetail, Scale_ReceiptDetailBusiness>, IItemAsyncMapper<Scale_ReceiptDetailBusiness, Scale_ReceiptDetail>
	{
		public Task<Scale_ReceiptDetailBusiness> MapAsync(string contextGuid, Scale_ReceiptDetail source, Scale_ReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_ReceiptDetailBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.HeaderRowGuid = source.HeaderRowGuid;
				destination.ItemNumber = source.ItemNumber;
				destination.ProductGuid = source.ProductGuid;
				destination.Name = source.Name;
				destination.Price = source.Price;
				destination.Discount = source.Discount;
				destination.TotalDiscount = source.TotalDiscount;
				destination.PluTax = source.PluTax;
				destination.TotalDetailTax = source.TotalDetailTax;
				destination.WeightQuantity = source.WeightQuantity;
				destination.Unit = source.Unit;
				destination.TotalPrice = source.TotalPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ReceiptDetailPersistenceRepository.cs", 83);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_ReceiptDetail> MapAsync(string contextGuid, Scale_ReceiptDetailBusiness source, Scale_ReceiptDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_ReceiptDetail();
				}
				destination.Guid = source.Guid;
				destination.HeaderRowGuid = source.HeaderRowGuid;
				destination.ItemNumber = source.ItemNumber;
				destination.ProductGuid = source.ProductGuid;
				destination.Name = source.Name;
				destination.Price = source.Price;
				destination.Discount = source.Discount;
				destination.TotalDiscount = source.TotalDiscount;
				destination.PluTax = source.PluTax;
				destination.TotalDetailTax = source.TotalDetailTax;
				destination.WeightQuantity = source.WeightQuantity;
				destination.Unit = source.Unit;
				destination.TotalPrice = source.TotalPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ReceiptDetailPersistenceRepository.cs", 120);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_ReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_ReceiptDetail> source, List<Scale_ReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_ReceiptDetail>> MapListAsync(string contextGuid, IEnumerable<Scale_ReceiptDetailBusiness> source, List<Scale_ReceiptDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_ReceiptDetail, Scale_ReceiptDetailBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_ReceiptDetail, Scale_ReceiptDetailBusiness> MapFromDatabase
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_ReceiptDetailBusiness, Scale_ReceiptDetail> MapToDatabase
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasModified
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public new bool IsDisposed { get; private set; }

	public Scale_ReceiptDetailPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ReceiptDetailPersistenceRepository.cs", 47);
		}
	}
}
