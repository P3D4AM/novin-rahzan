using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global.Tasks;

public class SarNakhLocationMapper : IItemAsyncMapper<SarNakhLocationBusiness, SarNakhLocationBusiness>
{
	public static SarNakhLocationMapper Instance { get; private set; } = new SarNakhLocationMapper();


	public Task<SarNakhLocationBusiness> MapAsync(string contextGuid, SarNakhLocationBusiness source, SarNakhLocationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new ArgumentNullException("Please Pass New Destination");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.ContextGuid = contextGuid;
			destination.SarNakhGuid = source.SarNakhGuid;
			destination.Email = source.Email;
			destination.WebSite = source.WebSite;
			destination.Address = source.Address;
			destination.State = source.State;
			destination.City = source.City;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Tasks\\SarNakh\\SarNakhLocationMapper.cs", 38);
			throw;
		}
	}

	public Task<List<SarNakhLocationBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakhLocationBusiness> source, List<SarNakhLocationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
