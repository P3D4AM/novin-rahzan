using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_DibalSaleMapper : IItemAsyncMapper<Scale_DibalSaleBusiness, Scale_DibalSaleBusiness>
{
	public static Scale_DibalSaleMapper Instance { get; private set; } = new Scale_DibalSaleMapper();


	public Task<Scale_DibalSaleBusiness> MapAsync(string contextGuid, Scale_DibalSaleBusiness source, Scale_DibalSaleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_DibalSaleBusiness(contextGuid);
			}
			destination.Raw = source.Raw;
			destination.Date = source.Date;
			destination.RawDate = source.RawDate;
			destination.RawTime = source.RawTime;
			destination.Amount1 = source.Amount1;
			destination.Amount2 = source.Amount2;
			destination.Price1 = source.Price1;
			destination.Price2 = source.Price2;
			destination.Discount1 = source.Discount1;
			destination.Discount2 = source.Discount2;
			destination.AmountSign1 = source.AmountSign1;
			destination.AmountSign2 = source.AmountSign2;
			destination.AnimalLink1 = source.AnimalLink1;
			destination.AnimalLink2 = source.AnimalLink2;
			destination.AnimalNumber1 = source.AnimalNumber1;
			destination.AnimalNumber2 = source.AnimalNumber2;
			destination.ClientKey = source.ClientKey;
			destination.FactorNumber = source.FactorNumber;
			destination.OperationType1 = source.OperationType1;
			destination.OperationType2 = source.OperationType2;
			destination.Parsed = source.Parsed;
			destination.Rounding = source.Rounding;
			destination.RoundingSign = source.RoundingSign;
			destination.SectionMessageNumber = source.SectionMessageNumber;
			destination.SectionNumber = source.SectionNumber;
			destination.Seller = source.Seller;
			destination.TotalDiscount = source.TotalDiscount;
			destination.TotalSum = source.TotalSum;
			destination.TotalSumSign = source.TotalSumSign;
			destination.VatType1 = source.VatType1;
			destination.VatType2 = source.VatType2;
			destination.Weight1 = source.Weight1;
			destination.Weight2 = source.Weight2;
			destination.Key = source.Key;
			destination.Key2 = source.Key2;
			destination.Group = source.Group;
			destination.Code1 = source.Code1;
			destination.Code2 = source.Code2;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_DibalSaleMapper.cs", 68);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_DibalSaleBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_DibalSaleBusiness> source, List<Scale_DibalSaleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
