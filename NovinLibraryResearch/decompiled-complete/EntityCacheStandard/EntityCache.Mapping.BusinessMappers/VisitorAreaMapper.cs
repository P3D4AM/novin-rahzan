using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class VisitorAreaMapper : IItemAsyncMapper<VisitorAreaBusiness, VisitorAreaBusiness>, IItemAsyncMapper<VisitorAreaBusiness, WebVisitorArea>, IItemAsyncMapper<WebVisitorArea, VisitorAreaBusiness>, IItemAsyncMapper<VisitorAreaBusiness, WebEntitySync>
{
	public static VisitorAreaMapper Instance { get; private set; } = new VisitorAreaMapper();


	public Task<VisitorAreaBusiness> MapAsync(string contextGuid, VisitorAreaBusiness source, VisitorAreaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new VisitorAreaBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.ParentGuid = source.ParentGuid;
			destination.Level = source.Level;
			destination.Code = source.Code;
			destination.FullCode = source.FullCode;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\VisitorAreaMapper.cs", 51);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebVisitorArea> MapAsync(string contextGuid, VisitorAreaBusiness source, WebVisitorArea destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebVisitorArea();
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.ParentGuid = source.ParentGuid;
			destination.Level = source.Level;
			destination.Code = source.Code;
			destination.FullCode = source.FullCode;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\VisitorAreaMapper.cs", 85);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<VisitorAreaBusiness> MapAsync(string contextGuid, WebVisitorArea source, VisitorAreaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new VisitorAreaBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.ParentGuid = source.ParentGuid;
			destination.Level = source.Level;
			destination.Code = source.Code;
			destination.FullCode = source.FullCode;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\VisitorAreaMapper.cs", 119);
			throw;
		}
		return Task.FromResult(destination);
	}

	public async Task<WebEntitySync> MapAsync(string contextGuid, VisitorAreaBusiness source, WebEntitySync destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebEntitySync();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Type = PacketType.VisitorArea;
			destination.Part = EntityPart.VisitorArea;
			destination.ServerStatus = source.ServerStatus;
			WebVisitorArea item = await MapAsync(contextGuid, source, new WebVisitorArea(), tranName, token);
			WebEntitySync webEntitySync = destination;
			webEntitySync.SerializedData = await GZip.Compress(Json.ToBytesJson(item));
			destination.ContextGuid = contextGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\VisitorAreaMapper.cs", 146);
			throw;
		}
	}

	public Task<List<VisitorAreaBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitorAreaBusiness> source, List<VisitorAreaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebVisitorArea>> MapListAsync(string contextGuid, IEnumerable<VisitorAreaBusiness> source, List<WebVisitorArea> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<VisitorAreaBusiness>> MapListAsync(string contextGuid, IEnumerable<WebVisitorArea> source, List<VisitorAreaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebEntitySync>> MapListAsync(string contextGuid, IEnumerable<VisitorAreaBusiness> source, List<WebEntitySync> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
