using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_SalesMapper : IItemAsyncMapper<Scale_SalesBusiness, Scale_SalesBusiness>
{
	public static Scale_SalesMapper Instance { get; private set; } = new Scale_SalesMapper();


	public Task<Scale_SalesBusiness> MapAsync(string contextGuid, Scale_SalesBusiness source, Scale_SalesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_SalesMapper.cs", 58);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_SalesBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_SalesBusiness> source, List<Scale_SalesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
