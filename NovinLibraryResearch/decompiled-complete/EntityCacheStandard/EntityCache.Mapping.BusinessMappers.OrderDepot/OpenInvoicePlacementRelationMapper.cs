using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class OpenInvoicePlacementRelationMapper : IItemAsyncMapper<OpenInvoicePlacementRelationBusiness, OpenInvoicePlacementRelationBusiness>
{
	public static OpenInvoicePlacementRelationMapper Instance { get; private set; } = new OpenInvoicePlacementRelationMapper();


	public Task<OpenInvoicePlacementRelationBusiness> MapAsync(string contextGuid, OpenInvoicePlacementRelationBusiness source, OpenInvoicePlacementRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new OpenInvoicePlacementRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.InvoiceGuid = source.InvoiceGuid;
			destination.PlacementGuid = source.PlacementGuid;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OpenInvoices\\OpenInvoicePlacementRelationMapper.cs", 33);
			throw;
		}
	}

	public Task<List<OpenInvoicePlacementRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<OpenInvoicePlacementRelationBusiness> source, List<OpenInvoicePlacementRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
