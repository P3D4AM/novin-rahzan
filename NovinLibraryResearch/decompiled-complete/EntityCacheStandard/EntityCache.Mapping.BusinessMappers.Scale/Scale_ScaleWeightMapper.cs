using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_ScaleWeightMapper : IItemAsyncMapper<Scale_ScaleWeightBusiness, Scale_ScaleWeightBusiness>
{
	public static Scale_ScaleWeightMapper Instance { get; private set; } = new Scale_ScaleWeightMapper();


	public Task<Scale_ScaleWeightBusiness> MapAsync(string contextGuid, Scale_ScaleWeightBusiness source, Scale_ScaleWeightBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_ScaleWeightBusiness(contextGuid);
			}
			destination.ScaleGuid = source.ScaleGuid;
			destination.Date = source.Date;
			destination.UnitPrice = source.UnitPrice;
			destination.UnitPriceText = source.UnitPriceText;
			destination.Weight = source.Weight;
			destination.WeightText = source.WeightText;
			destination.Totalprice = source.Totalprice;
			destination.TotalPriceText = source.TotalPriceText;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_ScaleWeightMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_ScaleWeightBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_ScaleWeightBusiness> source, List<Scale_ScaleWeightBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
