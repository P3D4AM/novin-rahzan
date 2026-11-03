using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ShortCutMapper : IItemAsyncMapper<ShortCutBusiness, WebShortCut>, IItemAsyncMapper<ShortCutBusiness, ShortCutBusiness>, IItemAsyncMapper<WebShortCut, ShortCutBusiness>
{
	public static ShortCutMapper Instance { get; private set; } = new ShortCutMapper();


	public Task<WebShortCut> MapAsync(string contextGuid, ShortCutBusiness source, WebShortCut destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebShortCut();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.FormName = source.FormName;
			destination.Name = source.Name;
			destination.ShortCutKey = source.ShortCutKey;
			destination.Func = source.Func;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.BackColor = "";
			destination.ForeColor = "";
			destination.ImageSrc = source.ImageSrc;
			destination.Enable = source.Enable;
			destination.Visable = source.Visable;
			destination.SubItems = source.SubItems;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ShortCutMapper.cs", 44);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebShortCut>> MapListAsync(string contextGuid, IEnumerable<ShortCutBusiness> source, List<WebShortCut> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ShortCutBusiness> MapAsync(string contextGuid, ShortCutBusiness source, ShortCutBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ShortCutBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.FormName = source.FormName;
			destination.Name = source.Name;
			destination.ShortCutKey = source.ShortCutKey;
			destination.Func = source.Func;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.BackColor = "";
			destination.ForeColor = "";
			destination.ImageSrc = source.ImageSrc;
			destination.Enable = source.Enable;
			destination.Visable = source.Visable;
			destination.SubItems = source.SubItems;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ShortCutMapper.cs", 81);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ShortCutBusiness>> MapListAsync(string contextGuid, IEnumerable<ShortCutBusiness> source, List<ShortCutBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ShortCutBusiness> MapAsync(string contextGuid, WebShortCut source, ShortCutBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ShortCutBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.FormName = source.FormName;
			destination.Name = source.Name;
			destination.ShortCutKey = source.ShortCutKey;
			destination.Func = source.Func;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.BackColor = "";
			destination.ForeColor = "";
			destination.ImageSrc = source.ImageSrc;
			destination.Enable = source.Enable;
			destination.Visable = source.Visable;
			destination.SubItems = source.SubItems;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ShortCutMapper.cs", 119);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ShortCutBusiness>> MapListAsync(string contextGuid, IEnumerable<WebShortCut> source, List<ShortCutBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
