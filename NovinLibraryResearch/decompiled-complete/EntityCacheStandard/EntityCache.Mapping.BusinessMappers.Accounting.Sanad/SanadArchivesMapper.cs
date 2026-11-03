using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Sanad;

public class SanadArchivesMapper : IItemAsyncMapper<SanadArchivesBusiness, SanadArchivesBusiness>, IItemAsyncMapper<SanadArchivesBusiness, WebSanadArchives>, IItemAsyncMapper<WebSanadArchives, SanadArchivesBusiness>
{
	public static SanadArchivesMapper Instance { get; private set; } = new SanadArchivesMapper();


	public Task<SanadArchivesBusiness> MapAsync(string contextGuid, SanadArchivesBusiness source, SanadArchivesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadArchivesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.Price = source.Price;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.Data = source.Data;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadArchivesMapper.cs", 46);
			throw;
		}
	}

	public Task<WebSanadArchives> MapAsync(string contextGuid, SanadArchivesBusiness source, WebSanadArchives destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebSanadArchives();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.Price = source.Price;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.Data = source.Data;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadArchivesMapper.cs", 77);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<SanadArchivesBusiness> MapAsync(string contextGuid, WebSanadArchives source, SanadArchivesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadArchivesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.Price = source.Price;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.Data = source.Data;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadArchivesMapper.cs", 110);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<SanadArchivesBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadArchivesBusiness> source, List<SanadArchivesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebSanadArchives>> MapListAsync(string contextGuid, IEnumerable<SanadArchivesBusiness> source, List<WebSanadArchives> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<SanadArchivesBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSanadArchives> source, List<SanadArchivesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
