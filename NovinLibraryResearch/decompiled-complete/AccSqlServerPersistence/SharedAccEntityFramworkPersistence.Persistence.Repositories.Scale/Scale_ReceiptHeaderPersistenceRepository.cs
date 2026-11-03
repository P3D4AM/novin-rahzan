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

internal class Scale_ReceiptHeaderPersistenceRepository : PersistenceRepository<Scale_ReceiptHeaderBusiness, Scale_ReceiptHeader>, IScale_ReceiptHeaderRepository, IRepository<Scale_ReceiptHeaderBusiness>, INoGuidRepository<Scale_ReceiptHeaderBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_ReceiptHeader, Scale_ReceiptHeaderBusiness>, IItemAsyncMapper<Scale_ReceiptHeaderBusiness, Scale_ReceiptHeader>
	{
		public Task<Scale_ReceiptHeaderBusiness> MapAsync(string contextGuid, Scale_ReceiptHeader source, Scale_ReceiptHeaderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_ReceiptHeaderBusiness(contextGuid);
				}
				destination.WorkstationIp = source.WorkstationIp;
				destination.SerialNumber = source.SerialNumber;
				destination.ReceiptNumber = source.ReceiptNumber;
				destination.ReceiptTime = source.ReceiptTime;
				destination.GhorfehName = source.GhorfehName;
				destination.WorkstationNumber = source.WorkstationNumber;
				destination.UserName = source.UserName;
				destination.InvoiceType = source.InvoiceType;
				destination.DiscountSupercharge = source.DiscountSupercharge;
				destination.TotalAmounts = source.TotalAmounts;
				destination.TotalWeights = source.TotalWeights;
				destination.TotalItems = source.TotalItems;
				destination.TotalItemFromPlu = source.TotalItemFromPlu;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ReceiptHeaderPersistenceRepository.cs", 79);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_ReceiptHeader> MapAsync(string contextGuid, Scale_ReceiptHeaderBusiness source, Scale_ReceiptHeader destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_ReceiptHeader();
				}
				destination.WorkstationIp = source.WorkstationIp;
				destination.SerialNumber = source.SerialNumber;
				destination.ReceiptNumber = source.ReceiptNumber;
				destination.ReceiptTime = source.ReceiptTime;
				destination.GhorfehName = source.GhorfehName;
				destination.WorkstationNumber = source.WorkstationNumber;
				destination.UserName = source.UserName;
				destination.InvoiceType = source.InvoiceType;
				destination.DiscountSupercharge = source.DiscountSupercharge;
				destination.TotalAmounts = source.TotalAmounts;
				destination.TotalWeights = source.TotalWeights;
				destination.TotalItems = source.TotalItems;
				destination.TotalItemFromPlu = source.TotalItemFromPlu;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ReceiptHeaderPersistenceRepository.cs", 115);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_ReceiptHeaderBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_ReceiptHeader> source, List<Scale_ReceiptHeaderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_ReceiptHeader>> MapListAsync(string contextGuid, IEnumerable<Scale_ReceiptHeaderBusiness> source, List<Scale_ReceiptHeader> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_ReceiptHeader, Scale_ReceiptHeaderBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_ReceiptHeader, Scale_ReceiptHeaderBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_ReceiptHeaderBusiness, Scale_ReceiptHeader> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_ReceiptHeaderPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ReceiptHeaderPersistenceRepository.cs", 44);
		}
	}
}
