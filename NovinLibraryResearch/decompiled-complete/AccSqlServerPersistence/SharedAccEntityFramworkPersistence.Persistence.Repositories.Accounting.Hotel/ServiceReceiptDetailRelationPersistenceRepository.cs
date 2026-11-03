using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class ServiceReceiptDetailRelationPersistenceRepository : PersistenceNoGuidRepository<ServiceReceiptDetailRelationBusiness, ServiceReceiptDetailRelation>, IServiceReceiptDetailRelationRepository, INoGuidRepository<ServiceReceiptDetailRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ServiceReceiptDetailRelationBusiness, ServiceReceiptDetailRelation>, IItemAsyncMapper<ServiceReceiptDetailRelation, ServiceReceiptDetailRelationBusiness>
	{
		public Task<ServiceReceiptDetailRelation> MapAsync(string contextGuid, ServiceReceiptDetailRelationBusiness source, ServiceReceiptDetailRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ServiceReceiptDetailRelation();
				}
				destination.ServiceGuid = source.ServiceGuid;
				destination.ReceiptDetailGuid = source.ReceiptDetailGuid;
				destination.DiscountPrice = source.DiscountPrice;
				destination.DiscountPercent = source.DiscountPercent;
				destination.AvarezPercent = source.AvarezPercent;
				destination.AvarezPrice = source.AvarezPrice;
				destination.TaxPercent = source.TaxPercent;
				destination.TaxPrice = source.TaxPrice;
				destination.Price = source.Price;
				destination.Quantity = source.Quantity;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ServiceReceiptDetailRelationPersistenceRepository.cs", 69);
			}
			return Task.FromResult(destination);
		}

		public Task<ServiceReceiptDetailRelationBusiness> MapAsync(string contextGuid, ServiceReceiptDetailRelation source, ServiceReceiptDetailRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ServiceReceiptDetailRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.ServiceGuid = source.ServiceGuid;
				destination.ReceiptDetailGuid = source.ReceiptDetailGuid;
				destination.DiscountPrice = source.DiscountPrice;
				destination.DiscountPercent = source.DiscountPercent;
				destination.AvarezPercent = source.AvarezPercent;
				destination.AvarezPrice = source.AvarezPrice;
				destination.TaxPercent = source.TaxPercent;
				destination.TaxPrice = source.TaxPrice;
				destination.Price = source.Price;
				destination.Quantity = source.Quantity;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ServiceReceiptDetailRelationPersistenceRepository.cs", 94);
			}
			return Task.FromResult(destination);
		}

		public Task<List<ServiceReceiptDetailRelation>> MapListAsync(string contextGuid, IEnumerable<ServiceReceiptDetailRelationBusiness> source, List<ServiceReceiptDetailRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ServiceReceiptDetailRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ServiceReceiptDetailRelation> source, List<ServiceReceiptDetailRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ServiceReceiptDetailRelation, ServiceReceiptDetailRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ServiceReceiptDetailRelationBusiness, ServiceReceiptDetailRelation> MapToDatabase => Mapper;

	public ServiceReceiptDetailRelationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\ServiceReceiptDetailRelationPersistenceRepository.cs", 36);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(ServiceReceiptDetailRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ServiceReceiptDetailRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
