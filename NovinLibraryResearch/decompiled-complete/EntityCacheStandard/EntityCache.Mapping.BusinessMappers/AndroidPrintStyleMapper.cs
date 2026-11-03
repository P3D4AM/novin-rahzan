using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class AndroidPrintStyleMapper : IItemAsyncMapper<AndroidPrintStyle, AndroidPrintStyle>
{
	public static AndroidPrintStyleMapper Instance { get; private set; } = new AndroidPrintStyleMapper();


	public Task<AndroidPrintStyle> MapAsync(string contextGuid, AndroidPrintStyle source, AndroidPrintStyle destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AndroidPrintStyle();
			}
			destination.Name = source.Name;
			destination.Delay = source.Delay;
			destination.PrintType = source.PrintType;
			destination.PrintFormat = source.PrintFormat;
			destination.ConnectoinType = source.ConnectoinType;
			destination.ConnectionValue = source.ConnectionValue;
			destination.Html = source.Html;
			destination.UserId = source.UserId;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\AndroidPrintStyleMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<AndroidPrintStyle>> MapListAsync(string contextGuid, IEnumerable<AndroidPrintStyle> source, List<AndroidPrintStyle> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
