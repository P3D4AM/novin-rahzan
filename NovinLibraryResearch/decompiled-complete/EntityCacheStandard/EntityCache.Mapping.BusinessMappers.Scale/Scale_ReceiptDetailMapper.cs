using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_ReceiptDetailMapper : IItemAsyncMapper<Scale_ReceiptDetailBusiness, Scale_ReceiptDetailBusiness>
{
	public static Scale_ReceiptDetailMapper Instance { get; private set; } = new Scale_ReceiptDetailMapper();


	public Task<Scale_ReceiptDetailBusiness> MapAsync(string contextGuid, Scale_ReceiptDetailBusiness source, Scale_ReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_ReceiptDetailMapper.cs", 45);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_ReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_ReceiptDetailBusiness> source, List<Scale_ReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
