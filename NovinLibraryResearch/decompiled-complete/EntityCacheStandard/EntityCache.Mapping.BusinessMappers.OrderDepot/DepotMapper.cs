using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class DepotMapper : IItemAsyncMapper<DepotBusiness, WebDepot>, IItemAsyncMapper<DepotBusiness, DepotBusiness>, IItemAsyncMapper<WebDepot, DepotBusiness>
{
	public static DepotMapper Instance { get; private set; } = new DepotMapper();


	public Task<WebDepot> MapAsync(string contextGuid, DepotBusiness source, WebDepot destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDepot();
			}
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.NegativeAmount = source.NegativeAmount;
			destination.Manfi = source.NegativeAmount != ReturnedState.Error;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\DepotMapper.cs", 43);
			throw;
		}
	}

	public Task<List<WebDepot>> MapListAsync(string contextGuid, IEnumerable<DepotBusiness> source, List<WebDepot> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<DepotBusiness> MapAsync(string contextGuid, DepotBusiness source, DepotBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DepotBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.NegativeAmount = source.NegativeAmount;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\DepotMapper.cs", 76);
			throw;
		}
	}

	public Task<List<DepotBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotBusiness> source, List<DepotBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<DepotBusiness> MapAsync(string contextGuid, WebDepot source, DepotBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DepotBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.NegativeAmount = source.NegativeAmount;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\DepotMapper.cs", 109);
			throw;
		}
	}

	public Task<List<DepotBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDepot> source, List<DepotBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
