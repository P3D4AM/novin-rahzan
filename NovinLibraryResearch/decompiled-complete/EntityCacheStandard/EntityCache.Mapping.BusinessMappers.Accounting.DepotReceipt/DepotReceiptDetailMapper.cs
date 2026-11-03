using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Business.OrderDepot.OrderComparer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.DepotReceipt;

public class DepotReceiptDetailMapper : IItemAsyncMapper<DepotReceiptDetailBusiness, DepotReceiptDetailBusiness>, IItemAsyncMapper<DepotReceiptDetailBusiness, OrderDetailBusiness>, IItemAsyncMapper<OrderDetailComparerWrapper, DepotReceiptDetailBusiness>
{
	public static DepotReceiptDetailMapper Instance { get; private set; } = new DepotReceiptDetailMapper();


	public async Task<DepotReceiptDetailBusiness> MapAsync(string contextGuid, DepotReceiptDetailBusiness source, DepotReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				return destination;
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
			await destination.SetDepotGuidAsync(source.DepotGuid, token, tranName);
			await destination.SetV2GuidAsync(EnSystem.Mapper, source.V2Guid, token, tranName);
			await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity, token, tranName);
			await destination.SetV2QtAsync(source.Quantity, token, tranName);
			destination.Ordinal = source.Ordinal;
			destination.ReferenceGuid = source.ReferenceGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepotReceipt\\DepotReceiptDetailMapper.cs", 45);
			throw;
		}
	}

	public async Task<OrderDetailBusiness> MapAsync(string contextGuid, DepotReceiptDetailBusiness source, OrderDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				return destination;
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			await destination.SetProductGuidAsync(EnSystem.Mapper, source.ProductGuid, isLoadOriginalPrice: true, tranName, token);
			await destination.SetDepotGuidAsync(source.DepotGuid, token);
			await destination.SetVGuidAsync(EnSystem.Mapper, source.V2Guid, token, tranName);
			await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity, token, tranName);
			await destination.SetV2QtAsync(EnSystem.Mapper, source.V2Quantity, token, tranName);
			destination.Ordinal = source.Ordinal;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepotReceipt\\DepotReceiptDetailMapper.cs", 74);
			throw;
		}
	}

	public async Task<DepotReceiptDetailBusiness> MapAsync(string contextGuid, OrderDetailComparerWrapper source, DepotReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				return destination;
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = Guid.NewGuid();
			destination.Modified = source.CurrentDetail.Modified;
			destination.Description = source.CurrentDetail.Description;
			destination.Status = source.CurrentDetail.Status;
			await destination.SetProductGuidAsync(source.CurrentProductGuid, EnSystem.Mapper, token, tranName);
			await destination.SetDepotGuidAsync(source.CurrentDetail.DepotGuid, token, tranName);
			await destination.SetV2GuidAsync(EnSystem.Mapper, source.CurrentDetail.VGuid, token, tranName);
			await destination.SetQuantityAsync(EnSystem.Mapper, source.CurrentQuantity, token, tranName);
			if (source.CurrentDetail.V2Qt.GetValueOrDefault() > 0m)
			{
				await destination.SetV2QtAsync(source.CurrentDetail.V2Qt, token, tranName);
			}
			destination.Ordinal = source.CurrentOrdinal;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepotReceipt\\DepotReceiptDetailMapper.cs", 104);
			throw;
		}
	}

	public Task<List<DepotReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotReceiptDetailBusiness> source, List<DepotReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotReceiptDetailBusiness> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<DepotReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailComparerWrapper> source, List<DepotReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
