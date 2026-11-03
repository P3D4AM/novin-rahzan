using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PrdGroupMapper : IItemAsyncMapper<PrdGroupBusiness, WebPrdGroup>, IItemAsyncMapper<PrdGroupBusiness, PrdGroupBusiness>, IItemAsyncMapper<WebPrdGroup, PrdGroupBusiness>
{
	public static PrdGroupMapper Instance { get; private set; } = new PrdGroupMapper();


	public Task<WebPrdGroup> MapAsync(string contextGuid, PrdGroupBusiness source, WebPrdGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdGroup();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.HtmlFull = source.HtmlFull;
			destination.HtmlQuick = source.HtmlQuick;
			destination.Name = source.Name;
			destination.Pic = source.Pic;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.TechInfoData = source.TechInfoData;
			destination.ParentGuid = source.ParentGuid;
			destination.Level = source.Level;
			destination.Code = source.Code;
			destination.FullCode = source.FullCode;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Slug = source.Slug;
			destination.Type = source.Type;
			destination.ShortName = source.ShortName;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdGroupMapper.cs", 52);
			throw;
		}
	}

	public Task<PrdGroupBusiness> MapAsync(string contextGuid, PrdGroupBusiness source, PrdGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.HtmlFull = source.HtmlFull;
			destination.HtmlQuick = source.HtmlQuick;
			destination.Name = source.Name;
			destination.Pic = source.Pic;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.TechInfoData = source.TechInfoData;
			destination.ParentGuid = source.ParentGuid;
			destination.Level = source.Level;
			destination.Code = source.Code;
			destination.FullCode = source.FullCode;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Slug = source.Slug;
			destination.ChangeStatus = source.ChangeStatus;
			destination.Type = source.Type;
			destination.ShortName = source.ShortName;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdGroupMapper.cs", 95);
			throw;
		}
	}

	public Task<PrdGroupBusiness> MapAsync(string contextGuid, WebPrdGroup source, PrdGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.HtmlFull = source.HtmlFull;
			destination.HtmlQuick = source.HtmlQuick;
			destination.Name = source.Name;
			destination.Pic = source.Pic;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.TechInfoData = source.TechInfoData;
			destination.ParentGuid = source.ParentGuid;
			destination.Level = source.Level;
			destination.Code = source.Code;
			destination.FullCode = source.FullCode;
			destination.TechInfoData = source.TechInfoData;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Slug = source.Slug;
			destination.Type = source.Type;
			destination.ShortName = source.ShortName;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdGroupMapper.cs", 138);
			throw;
		}
	}

	public Task<List<PrdGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdGroup> source, List<PrdGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdGroupBusiness> source, List<PrdGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebPrdGroup>> MapListAsync(string contextGuid, IEnumerable<PrdGroupBusiness> source, List<WebPrdGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
