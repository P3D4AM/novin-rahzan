using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class PrdSelectedMapper : IItemAsyncMapper<PrdSelectedBusiness, WebPrdSelected>, IItemAsyncMapper<PrdSelectedBusiness, PrdSelectedBusiness>, IItemAsyncMapper<WebPrdSelected, PrdSelectedBusiness>
{
	public static PrdSelectedMapper Instance { get; private set; } = new PrdSelectedMapper();


	public Task<WebPrdSelected> MapAsync(string contextGuid, PrdSelectedBusiness source, WebPrdSelected destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdSelected();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Code = source.Code;
			destination.Title = source.Title;
			destination.Type = source.Type;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Product\\PrdSelectedMapper.cs", 40);
			throw;
		}
	}

	public async Task<PrdSelectedBusiness> MapAsync(string contextGuid, PrdSelectedBusiness source, PrdSelectedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new PrdSelectedBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Code = source.Code;
			destination.Title = source.Title;
			destination.Type = source.Type;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			if (source.DetList?.Any() ?? false)
			{
				destination.AddRangeToDetList(await PrdSelectedDetListMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<PrdSelectedDetListBusiness>(), tranName, token));
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Product\\PrdSelectedMapper.cs", 73);
			throw;
		}
	}

	public async Task<PrdSelectedBusiness> MapAsync(string contextGuid, WebPrdSelected source, PrdSelectedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new PrdSelectedBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Code = source.Code;
			destination.Title = source.Title;
			destination.Type = source.Type;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			if (source.DetList != null)
			{
				PrdSelectedBusiness prdSelectedBusiness = destination;
				prdSelectedBusiness.AddRangeToDetList(await PrdSelectedDetListMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<PrdSelectedDetListBusiness>(), tranName, token));
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Product\\PrdSelectedMapper.cs", 102);
			throw;
		}
	}

	public Task<List<WebPrdSelected>> MapListAsync(string contextGuid, IEnumerable<PrdSelectedBusiness> source, List<WebPrdSelected> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdSelectedBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdSelectedBusiness> source, List<PrdSelectedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdSelectedBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdSelected> source, List<PrdSelectedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
