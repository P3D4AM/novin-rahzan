using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class TaxOrderLogMapper : IItemAsyncMapper<TaxOrderLogBusiness, TaxOrderLogBusiness>, IItemAsyncMapper<TaxOrderLogBusiness, WebTaxOrderLog>, IItemAsyncMapper<WebTaxOrderLog, TaxOrderLogBusiness>
{
	public static TaxOrderLogMapper Instance { get; private set; } = new TaxOrderLogMapper();


	public Task<TaxOrderLogBusiness> MapAsync(string contextGuid, TaxOrderLogBusiness source, TaxOrderLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TaxOrderLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.OrderGuid = source.OrderGuid;
			destination.TaxSendDate = source.TaxSendDate;
			destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
			destination.TaxResultErrorCode = source.TaxResultErrorCode;
			destination.TaxResultMessage = source.TaxResultMessage;
			destination.TaxUid = source.TaxUid;
			destination.TaxOrderId = source.TaxOrderId;
			destination.InsertDate = source.InsertDate;
			destination.IsSystem = source.IsSystem;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.Subject = source.Subject;
			destination.TaxDeliveryDate = source.TaxDeliveryDate;
			destination.DeliveryDate = source.DeliveryDate;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Ordinal = source.Ordinal;
			destination.BaseTaxOrderId = source.BaseTaxOrderId;
			destination.Modified = source.Modified;
			destination.InvoiceSeries = source.InvoiceSeries;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\TaxOrderLogMapper.cs", 57);
			throw;
		}
	}

	public Task<WebTaxOrderLog> MapAsync(string contextGuid, TaxOrderLogBusiness source, WebTaxOrderLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTaxOrderLog();
			}
			destination.Guid = source.Guid;
			destination.OrderGuid = source.OrderGuid;
			destination.TaxSendDate = source.TaxSendDate;
			destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
			destination.TaxResultErrorCode = source.TaxResultErrorCode;
			destination.TaxResultMessage = source.TaxResultMessage;
			destination.TaxUid = source.TaxUid;
			destination.TaxOrderId = source.TaxOrderId;
			destination.InsertDate = source.InsertDate;
			destination.IsSystem = source.IsSystem;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.Subject = source.Subject;
			destination.TaxDeliveryDate = source.TaxDeliveryDate;
			destination.DeliveryDate = source.DeliveryDate;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Ordinal = source.Ordinal;
			destination.BaseTaxOrderId = source.BaseTaxOrderId;
			destination.Modified = source.Modified;
			destination.InvoiceSeries = source.InvoiceSeries;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\TaxOrderLogMapper.cs", 99);
			throw;
		}
	}

	public Task<TaxOrderLogBusiness> MapAsync(string contextGuid, WebTaxOrderLog source, TaxOrderLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TaxOrderLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.OrderGuid = source.OrderGuid;
			destination.TaxSendDate = source.TaxSendDate;
			destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
			destination.TaxResultErrorCode = source.TaxResultErrorCode;
			destination.TaxResultMessage = source.TaxResultMessage;
			destination.TaxUid = source.TaxUid;
			destination.TaxOrderId = source.TaxOrderId;
			destination.InsertDate = source.InsertDate;
			destination.IsSystem = source.IsSystem;
			destination.OldState = source.OldState;
			destination.NewState = source.NewState;
			destination.Subject = source.Subject;
			destination.TaxDeliveryDate = source.TaxDeliveryDate;
			destination.DeliveryDate = source.DeliveryDate;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Ordinal = source.Ordinal;
			destination.BaseTaxOrderId = source.BaseTaxOrderId;
			destination.Modified = source.Modified;
			destination.InvoiceSeries = source.InvoiceSeries;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\TaxOrderLogMapper.cs", 142);
			throw;
		}
	}

	public Task<List<TaxOrderLogBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxOrderLogBusiness> source, List<TaxOrderLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebTaxOrderLog>> MapListAsync(string contextGuid, IEnumerable<TaxOrderLogBusiness> source, List<WebTaxOrderLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TaxOrderLogBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTaxOrderLog> source, List<TaxOrderLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
