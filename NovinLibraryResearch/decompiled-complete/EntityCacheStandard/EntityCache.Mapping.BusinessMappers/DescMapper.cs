using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class DescMapper : IItemAsyncMapper<DescBusiness, WebDesc>, IItemAsyncMapper<DescBusiness, DescBusiness>, IItemAsyncMapper<WebDesc, DescBusiness>
{
	public static DescMapper Instance { get; private set; } = new DescMapper();


	public Task<WebDesc> MapAsync(string contextGuid, DescBusiness source, WebDesc destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDesc();
			}
			destination.ServerStatus = source.ServerStatus;
			destination.Guid = source.Guid;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Section = source.Section;
			destination.Description = source.Description;
			destination.SourceType = source.SourceType;
			destination.Source = source.Source;
			destination.DestinationType = source.DestinationType;
			destination.Destination = source.Destination;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\DescMapper.cs", 46);
			throw;
		}
	}

	public Task<List<WebDesc>> MapListAsync(string contextGuid, IEnumerable<DescBusiness> source, List<WebDesc> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<DescBusiness> MapAsync(string contextGuid, DescBusiness source, DescBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DescBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.Guid = source.Guid;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Section = source.Section;
			destination.Description = source.Description;
			destination.SourceType = source.SourceType;
			destination.Source = source.Source;
			destination.DestinationType = source.DestinationType;
			destination.Destination = source.Destination;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\DescMapper.cs", 82);
			throw;
		}
	}

	public Task<List<DescBusiness>> MapListAsync(string contextGuid, IEnumerable<DescBusiness> source, List<DescBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<DescBusiness> MapAsync(string contextGuid, WebDesc source, DescBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DescBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.Guid = source.Guid;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Section = source.Section;
			destination.Description = source.Description;
			destination.SourceType = source.SourceType;
			destination.Source = source.Source;
			destination.DestinationType = source.DestinationType;
			destination.Destination = source.Destination;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\DescMapper.cs", 118);
			throw;
		}
	}

	public Task<List<DescBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDesc> source, List<DescBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
