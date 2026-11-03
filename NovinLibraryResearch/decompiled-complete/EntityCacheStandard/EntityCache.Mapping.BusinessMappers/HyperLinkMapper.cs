using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class HyperLinkMapper : IItemAsyncMapper<HyperLinkBusiness, WebHyperLink>, IItemAsyncMapper<HyperLinkBusiness, HyperLinkBusiness>, IItemAsyncMapper<WebHyperLink, HyperLinkBusiness>
{
	public static HyperLinkMapper Instance { get; private set; } = new HyperLinkMapper();


	public Task<WebHyperLink> MapAsync(string contextGuid, HyperLinkBusiness source, WebHyperLink destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebHyperLink();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Category = source.Category;
			destination.Enable = source.Enable;
			destination.Icon = source.Icon;
			destination.Order = source.Order;
			destination.ParentGuid = source.ParentGuid;
			destination.Style = source.Style;
			destination.Target = source.Target;
			destination.Title = source.Title;
			destination.Url = source.Url;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\HyperLinkMapper.cs", 42);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebHyperLink>> MapListAsync(string contextGuid, IEnumerable<HyperLinkBusiness> source, List<WebHyperLink> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<HyperLinkBusiness> MapAsync(string contextGuid, HyperLinkBusiness source, HyperLinkBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new HyperLinkBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Category = source.Category;
			destination.Enable = source.Enable;
			destination.Icon = source.Icon;
			destination.Order = source.Order;
			destination.ParentGuid = source.ParentGuid;
			destination.Style = source.Style;
			destination.Target = source.Target;
			destination.Title = source.Title;
			destination.Url = source.Url;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\HyperLinkMapper.cs", 77);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<HyperLinkBusiness>> MapListAsync(string contextGuid, IEnumerable<HyperLinkBusiness> source, List<HyperLinkBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<HyperLinkBusiness> MapAsync(string contextGuid, WebHyperLink source, HyperLinkBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new HyperLinkBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Category = source.Category;
			destination.Enable = source.Enable;
			destination.Icon = source.Icon;
			destination.Order = source.Order;
			destination.ParentGuid = source.ParentGuid;
			destination.Style = source.Style;
			destination.Target = source.Target;
			destination.Title = source.Title;
			destination.Url = source.Url;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\HyperLinkMapper.cs", 112);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<HyperLinkBusiness>> MapListAsync(string contextGuid, IEnumerable<WebHyperLink> source, List<HyperLinkBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
