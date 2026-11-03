using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Hotel;

public class CustomerReceiptRelationMapper : IItemAsyncMapper<CustomerReceiptRelationBusiness, CustomerReceiptRelationBusiness>
{
	public static CustomerReceiptRelationMapper Instance { get; private set; } = new CustomerReceiptRelationMapper();


	public Task<CustomerReceiptRelationBusiness> MapAsync(string contextGuid, CustomerReceiptRelationBusiness source, CustomerReceiptRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CustomerReceiptRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.CustomerGuid = source.CustomerGuid;
			destination.ReceiptGuid = source.ReceiptGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Hotel\\CustomerReceiptRelationMapper.cs", 32);
			throw;
		}
	}

	public Task<List<CustomerReceiptRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerReceiptRelationBusiness> source, List<CustomerReceiptRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
