using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Customer;

public class CusGroupMapper : IItemAsyncMapper<CusGroupBusiness, WebCusGroup>, IItemAsyncMapper<CusGroupBusiness, CusGroupBusiness>, IItemAsyncMapper<WebCusGroup, CusGroupBusiness>
{
	public static CusGroupMapper Instance { get; private set; } = new CusGroupMapper();


	public Task<WebCusGroup> MapAsync(string contextGuid, CusGroupBusiness source, WebCusGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCusGroup();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.MoeinDefaultGuid = source.MoeinDefaultGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CusGroupMapper.cs", 45);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebCusGroup>> MapListAsync(string contextGuid, IEnumerable<CusGroupBusiness> source, List<WebCusGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CusGroupBusiness> MapAsync(string contextGuid, CusGroupBusiness source, CusGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CusGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.MoeinDefaultGuid = source.MoeinDefaultGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CusGroupMapper.cs", 82);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CusGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<CusGroupBusiness> source, List<CusGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CusGroupBusiness> MapAsync(string contextGuid, WebCusGroup source, CusGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CusGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.MoeinDefaultGuid = source.MoeinDefaultGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CusGroupMapper.cs", 119);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CusGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCusGroup> source, List<CusGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
