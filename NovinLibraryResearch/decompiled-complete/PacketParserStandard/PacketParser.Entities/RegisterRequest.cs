using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Mapping;

namespace PacketParser.Entities;

public class RegisterRequest : IItemAsyncMapper<RegisterRequest, RegisterRequest>
{
	public int RequestCount { get; set; }

	public DateTime LastRequest { get; set; }

	public Task<RegisterRequest> MapAsync(string contextGuid, RegisterRequest source, RegisterRequest destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new RegisterRequest();
			}
			destination.RequestCount = source.RequestCount;
			destination.LastRequest = source.LastRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\RegisterRequest.cs", 29);
		}
		return Task.FromResult(destination);
	}

	public Task<List<RegisterRequest>> MapListAsync(string contextGuid, IEnumerable<RegisterRequest> source, List<RegisterRequest> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
