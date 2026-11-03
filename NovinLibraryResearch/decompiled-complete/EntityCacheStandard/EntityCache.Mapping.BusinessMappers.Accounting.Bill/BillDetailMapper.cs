using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Bill;

public class BillDetailMapper : IItemAsyncMapper<BillDetailBusiness, BillDetailBusiness>
{
	public static BillDetailMapper Instance { get; private set; } = new BillDetailMapper();


	public Task<BillDetailBusiness> MapAsync(string contextGuid, BillDetailBusiness source, BillDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BillDetailBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.PlacementGuid = source.PlacementGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Bill\\BillDetailMapper.cs", 33);
			throw;
		}
	}

	public Task<List<BillDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<BillDetailBusiness> source, List<BillDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
