using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class MenuItemMapper : IItemAsyncMapper<MenuItemBusiness, WebMenuItem>, IItemAsyncMapper<MenuItemBusiness, MenuItemBusiness>, IItemAsyncMapper<WebMenuItem, MenuItemBusiness>
{
	public Task<WebMenuItem> MapAsync(string contextGuid, MenuItemBusiness source, WebMenuItem destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebMenuItem();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Type = source.Type;
			destination.TypeGuid = source.TypeGuid;
			destination.Ordinal = source.Ordinal;
			destination.MasterGuid = source.MasterGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\MenuItemMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<MenuItemBusiness> MapAsync(string contextGuid, MenuItemBusiness source, MenuItemBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MenuItemBusiness(contextGuid, null);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Type = source.Type;
			destination.TypeGuid = source.TypeGuid;
			destination.Ordinal = source.Ordinal;
			destination.MasterGuid = source.MasterGuid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\MenuItemMapper.cs", 62);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<MenuItemBusiness> MapAsync(string contextGuid, WebMenuItem source, MenuItemBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MenuItemBusiness(contextGuid, null);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Type = source.Type;
			destination.TypeGuid = source.TypeGuid;
			destination.Ordinal = source.Ordinal;
			destination.MasterGuid = source.MasterGuid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\MenuItemMapper.cs", 88);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebMenuItem>> MapListAsync(string contextGuid, IEnumerable<MenuItemBusiness> source, List<WebMenuItem> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MenuItemBusiness>> MapListAsync(string contextGuid, IEnumerable<MenuItemBusiness> source, List<MenuItemBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MenuItemBusiness>> MapListAsync(string contextGuid, IEnumerable<WebMenuItem> source, List<MenuItemBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
