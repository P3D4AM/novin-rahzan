using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class TafsilSelectedMapper : IItemAsyncMapper<TafsilSelectedBusiness, WebTafsilSelected>, IItemAsyncMapper<TafsilSelectedBusiness, TafsilSelectedBusiness>, IItemAsyncMapper<WebTafsilSelected, TafsilSelectedBusiness>
{
	public static TafsilSelectedMapper Instance { get; private set; } = new TafsilSelectedMapper();


	public Task<WebTafsilSelected> MapAsync(string contextGuid, TafsilSelectedBusiness source, WebTafsilSelected destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTafsilSelected();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Code = source.Code;
			destination.Title = source.Title;
			destination.Type = source.Type;
			destination.UGuid = source.UGuid;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Tafsil\\TafsilSelectedMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public async Task<TafsilSelectedBusiness> MapAsync(string contextGuid, WebTafsilSelected source, TafsilSelectedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilSelectedBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Code = source.Code;
			destination.Title = source.Title;
			destination.Type = source.Type;
			destination.UGuid = source.UGuid;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
			if (source.DetList != null)
			{
				TafsilSelectedBusiness tafsilSelectedBusiness = destination;
				await tafsilSelectedBusiness.AddRangeToDetListAsync(await TafsilSelectedDetListMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<TafsilSelectedDetListBusiness>(), tranName, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Tafsil\\TafsilSelectedMapper.cs", 70);
			throw;
		}
		return destination;
	}

	public async Task<TafsilSelectedBusiness> MapAsync(string contextGuid, TafsilSelectedBusiness source, TafsilSelectedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilSelectedBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Code = source.Code;
			destination.Title = source.Title;
			destination.Type = source.Type;
			destination.UGuid = source.UGuid;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
			if ((source?.DetList?.Any()).GetValueOrDefault())
			{
				await destination.AddRangeToDetListAsync(await TafsilSelectedDetListMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<TafsilSelectedDetListBusiness>(), tranName, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Tafsil\\TafsilSelectedMapper.cs", 102);
			throw;
		}
		return destination;
	}

	public Task<List<WebTafsilSelected>> MapListAsync(string contextGuid, IEnumerable<TafsilSelectedBusiness> source, List<WebTafsilSelected> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TafsilSelectedBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTafsilSelected> source, List<TafsilSelectedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TafsilSelectedBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilSelectedBusiness> source, List<TafsilSelectedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
