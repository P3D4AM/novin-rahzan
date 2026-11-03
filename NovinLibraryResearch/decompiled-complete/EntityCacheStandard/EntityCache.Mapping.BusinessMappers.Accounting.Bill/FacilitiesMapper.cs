using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Bill;

public class FacilitiesMapper : IItemAsyncMapper<FacilitiesBusiness, FacilitiesBusiness>
{
	public static FacilitiesMapper Instance { get; private set; } = new FacilitiesMapper();


	public Task<FacilitiesBusiness> MapAsync(string contextGuid, FacilitiesBusiness source, FacilitiesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FacilitiesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Bill\\FacilitiesMapper.cs", 35);
			throw;
		}
	}

	public Task<List<FacilitiesBusiness>> MapListAsync(string contextGuid, IEnumerable<FacilitiesBusiness> source, List<FacilitiesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
