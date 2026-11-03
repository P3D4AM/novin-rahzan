using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global.Tasks;

public class SarNakhNumbersMapper : IItemAsyncMapper<SarNakhNumbersBusiness, SarNakhNumbersBusiness>, IItemAsyncMapper<SarNakhNumbersBusiness, TafsilNumberBusiness>
{
	public static SarNakhNumbersMapper Instance { get; private set; } = new SarNakhNumbersMapper();


	public Task<SarNakhNumbersBusiness> MapAsync(string contextGuid, SarNakhNumbersBusiness source, SarNakhNumbersBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.Number = source.Number;
			destination.Title = source.Title;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Tasks\\SarNakh\\SarNakhNumbersMapper.cs", 36);
			throw;
		}
	}

	public Task<TafsilNumberBusiness> MapAsync(string contextGuid, SarNakhNumbersBusiness source, TafsilNumberBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilNumberBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Title = source.Title;
			destination.OriginalNumber = source.Number.ParseToLong(0L);
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Tasks\\SarNakh\\SarNakhNumbersMapper.cs", 57);
			throw;
		}
	}

	public Task<List<SarNakhNumbersBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakhNumbersBusiness> source, List<SarNakhNumbersBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TafsilNumberBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakhNumbersBusiness> source, List<TafsilNumberBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
