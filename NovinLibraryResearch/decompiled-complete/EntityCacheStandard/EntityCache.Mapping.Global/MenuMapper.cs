using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class MenuMapper : IItemAsyncMapper<MenuBusiness, WebMenu>, IItemAsyncMapper<MenuBusiness, MenuBusiness>, IItemAsyncMapper<WebMenu, MenuBusiness>
{
	public static MenuMapper Instance { get; private set; } = new MenuMapper();


	public Task<WebMenu> MapAsync(string contextGuid, MenuBusiness source, WebMenu destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebMenu();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.RowCount = source.RowCount;
			destination.ColumnCount = source.ColumnCount;
			destination.Title = source.Title;
			destination.MasterGuid = source.MasterGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\MenuMapper.cs", 38);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<MenuBusiness> MapAsync(string contextGuid, MenuBusiness source, MenuBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MenuBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.RowCount = source.RowCount;
			destination.ColumnCount = source.ColumnCount;
			destination.Title = source.Title;
			destination.ContextGuid = contextGuid;
			destination.MasterGuid = source.MasterGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\MenuMapper.cs", 65);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<MenuBusiness> MapAsync(string contextGuid, WebMenu source, MenuBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MenuBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.RowCount = source.RowCount;
			destination.ColumnCount = source.ColumnCount;
			destination.Title = source.Title;
			destination.ContextGuid = contextGuid;
			destination.MasterGuid = source.MasterGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\MenuMapper.cs", 92);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebMenu>> MapListAsync(string contextGuid, IEnumerable<MenuBusiness> source, List<WebMenu> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MenuBusiness>> MapListAsync(string contextGuid, IEnumerable<MenuBusiness> source, List<MenuBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MenuBusiness>> MapListAsync(string contextGuid, IEnumerable<WebMenu> source, List<MenuBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
