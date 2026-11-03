using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Hotel;

internal class ServiceReceieptDetailRelationMapper : IItemAsyncMapper<ServiceReceiptDetailRelationBusiness, ServiceReceiptDetailRelationBusiness>
{
	public static ServiceReceieptDetailRelationMapper Instance { get; private set; } = new ServiceReceieptDetailRelationMapper();


	public Task<ServiceReceiptDetailRelationBusiness> MapAsync(string contextGuid, ServiceReceiptDetailRelationBusiness source, ServiceReceiptDetailRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Hotel\\ServiceReceiptDetailRelationMapper.cs", 41);
			throw;
		}
	}

	public Task<List<ServiceReceiptDetailRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ServiceReceiptDetailRelationBusiness> source, List<ServiceReceiptDetailRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
