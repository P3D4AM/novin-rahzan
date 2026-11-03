using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class FareOrderCreditMapper : IItemAsyncMapper<FareOrderCreditBusiness, WebFareOrderCredit>, IItemAsyncMapper<FareOrderCreditBusiness, FareOrderCreditBusiness>, IItemAsyncMapper<WebFareOrderCredit, FareOrderCreditBusiness>
{
	public static FareOrderCreditMapper Instance { get; private set; } = new FareOrderCreditMapper();


	public Task<WebFareOrderCredit> MapAsync(string contextGuid, FareOrderCreditBusiness source, WebFareOrderCredit destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebFareOrderCredit();
			}
			destination.Guid = source.Guid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Status = source.Status;
			destination.ViewOrder = source.ViewOrder;
			destination.TafsilName = source.TafsilName;
			destination.TafsilCode = source.TafsilCode;
			destination.MoeinName = source.MoeinName;
			destination.Modified = source.Modified;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\FareOrderCreditMapper.cs", 41);
			throw;
		}
	}

	public Task<FareOrderCreditBusiness> MapAsync(string contextGuid, FareOrderCreditBusiness source, FareOrderCreditBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FareOrderCreditBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Status = source.Status;
			destination.ViewOrder = source.ViewOrder;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\FareOrderCreditMapper.cs", 66);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<FareOrderCreditBusiness> MapAsync(string contextGuid, WebFareOrderCredit source, FareOrderCreditBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FareOrderCreditBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Status = source.Status;
			destination.ViewOrder = source.ViewOrder;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\FareOrderCreditMapper.cs", 91);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebFareOrderCredit>> MapListAsync(string contextGuid, IEnumerable<FareOrderCreditBusiness> source, List<WebFareOrderCredit> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<FareOrderCreditBusiness>> MapListAsync(string contextGuid, IEnumerable<FareOrderCreditBusiness> source, List<FareOrderCreditBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<FareOrderCreditBusiness>> MapListAsync(string contextGuid, IEnumerable<WebFareOrderCredit> source, List<FareOrderCreditBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
