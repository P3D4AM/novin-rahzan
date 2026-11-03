using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class OpenInvoiceOrderRelationMapper : IItemAsyncMapper<OpenInvoiceOrderRelationBusiness, OpenInvoiceOrderRelationBusiness>
{
	public static OpenInvoiceOrderRelationMapper Instance { get; private set; } = new OpenInvoiceOrderRelationMapper();


	public Task<OpenInvoiceOrderRelationBusiness> MapAsync(string contextGuid, OpenInvoiceOrderRelationBusiness source, OpenInvoiceOrderRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new OpenInvoiceOrderRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.InvoiceGuid = source.InvoiceGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OpenInvoices\\OpenInvoiceOrderRelationMapper.cs", 33);
			throw;
		}
	}

	public Task<List<OpenInvoiceOrderRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<OpenInvoiceOrderRelationBusiness> source, List<OpenInvoiceOrderRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
