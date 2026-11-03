using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class OpenInvoiceMapper : IItemAsyncMapper<OpenInvoiceBusiness, OpenInvoiceBusiness>
{
	public static OpenInvoiceMapper Instance { get; private set; } = new OpenInvoiceMapper();


	public Task<OpenInvoiceBusiness> MapAsync(string contextGuid, OpenInvoiceBusiness source, OpenInvoiceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new OpenInvoiceBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.DateM = source.DateM;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.PeopleCount = source.PeopleCount;
			destination.SanadNo = source.SanadNo;
			destination.Sum = source.Sum;
			destination.Number = source.Number;
			destination.SettlementDate = source.SettlementDate;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OpenInvoices\\OpenInvoiceMapper.cs", 44);
			throw;
		}
	}

	public Task<List<OpenInvoiceBusiness>> MapListAsync(string contextGuid, IEnumerable<OpenInvoiceBusiness> source, List<OpenInvoiceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
