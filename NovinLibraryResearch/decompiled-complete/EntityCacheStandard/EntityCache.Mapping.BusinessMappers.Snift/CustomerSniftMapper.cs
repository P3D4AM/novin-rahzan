using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Snifts;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Snift;

public class CustomerSniftMapper : IItemAsyncMapper<CusCompactBusiness, WebCustomerSnifts>
{
	public static CustomerSniftMapper Instance { get; private set; } = new CustomerSniftMapper();


	public Task<WebCustomerSnifts> MapAsync(string contextGuid, CusCompactBusiness source, WebCustomerSnifts destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCustomerSnifts();
			}
			destination.Name = source.Name;
			destination.Address = source.Address1 + " \r\n " + source.Address2;
			destination.Guid = source.Guid;
			destination.Title = source.TitleName;
			destination.Group = source.GroupName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Snift\\CustomerSniftMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebCustomerSnifts>> MapListAsync(string contextGuid, IEnumerable<CusCompactBusiness> source, List<WebCustomerSnifts> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
