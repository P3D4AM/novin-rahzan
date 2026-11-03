using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Bill;

public class TempBillMapper : IItemAsyncMapper<TempBillBusiness, TempBillBusiness>
{
	public static TempBillMapper Instance { get; private set; } = new TempBillMapper();


	public Task<TempBillBusiness> MapAsync(string contextGuid, TempBillBusiness source, TempBillBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TempBillBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.PlacementGuid = source.PlacementGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Bill\\TempBillMapper.cs", 32);
			throw;
		}
	}

	public Task<List<TempBillBusiness>> MapListAsync(string contextGuid, IEnumerable<TempBillBusiness> source, List<TempBillBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
