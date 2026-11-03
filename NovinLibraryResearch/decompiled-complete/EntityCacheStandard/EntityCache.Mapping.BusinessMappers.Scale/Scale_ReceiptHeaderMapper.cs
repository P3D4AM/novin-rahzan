using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_ReceiptHeaderMapper : IItemAsyncMapper<Scale_ReceiptHeaderBusiness, Scale_ReceiptHeaderBusiness>
{
	public static Scale_ReceiptHeaderMapper Instance { get; private set; } = new Scale_ReceiptHeaderMapper();


	public Task<Scale_ReceiptHeaderBusiness> MapAsync(string contextGuid, Scale_ReceiptHeaderBusiness source, Scale_ReceiptHeaderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_ReceiptHeaderBusiness(contextGuid);
			}
			destination.WorkstationIp = source.WorkstationIp;
			destination.SerialNumber = source.SerialNumber;
			destination.ReceiptNumber = source.ReceiptNumber;
			destination.ReceiptTime = source.ReceiptTime;
			destination.GhorfehName = source.GhorfehName;
			destination.WorkstationNumber = source.WorkstationNumber;
			destination.UserName = source.UserName;
			destination.InvoiceType = source.InvoiceType;
			destination.DiscountSupercharge = source.DiscountSupercharge;
			destination.TotalAmounts = source.TotalAmounts;
			destination.TotalWeights = source.TotalWeights;
			destination.TotalItems = source.TotalItems;
			destination.TotalItemFromPlu = source.TotalItemFromPlu;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_ReceiptHeaderMapper.cs", 44);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_ReceiptHeaderBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_ReceiptHeaderBusiness> source, List<Scale_ReceiptHeaderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
