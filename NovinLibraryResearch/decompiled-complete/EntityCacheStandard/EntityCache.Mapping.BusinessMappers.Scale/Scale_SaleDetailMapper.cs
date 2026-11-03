using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_SaleDetailMapper : IItemAsyncMapper<Scale_SaleDetailBusiness, Scale_SaleDetailBusiness>
{
	public static Scale_SaleDetailMapper Instance { get; private set; } = new Scale_SaleDetailMapper();


	public Task<Scale_SaleDetailBusiness> MapAsync(string contextGuid, Scale_SaleDetailBusiness source, Scale_SaleDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_SaleDetailMapper.cs", 48);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_SaleDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_SaleDetailBusiness> source, List<Scale_SaleDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
