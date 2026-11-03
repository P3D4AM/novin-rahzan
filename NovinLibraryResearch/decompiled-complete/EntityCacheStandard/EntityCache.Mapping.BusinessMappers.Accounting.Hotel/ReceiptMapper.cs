using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Hotel;

public class ReceiptMapper : IItemAsyncMapper<ReceiptBusiness, ReceiptBusiness>
{
	public static ReceiptMapper Instance { get; private set; } = new ReceiptMapper();


	public Task<ReceiptBusiness> MapAsync(string contextGuid, ReceiptBusiness source, ReceiptBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceiptBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.CustomerGuid = source.CustomerGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.PresenterGuid = source.PresenterGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Number = source.Number;
			destination.Description = source.Description;
			destination.Discount = source.Discount;
			destination.PepolCount = source.PepolCount;
			destination.DateM = source.DateM;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Hotel\\ReceiptMapper.cs", 42);
			throw;
		}
	}

	public Task<List<ReceiptBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceiptBusiness> source, List<ReceiptBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
