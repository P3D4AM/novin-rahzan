using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Gift;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Gift;

public class GiftDetailMapper : IItemAsyncMapper<GiftDetailBusiness, GiftDetailBusiness>, IItemAsyncMapper<WebGiftDetail, GiftDetailBusiness>, IItemAsyncMapper<GiftDetailBusiness, WebGiftDetail>
{
	public static GiftDetailMapper Instance { get; private set; } = new GiftDetailMapper();


	public Task<GiftDetailBusiness> MapAsync(string contextGuid, GiftDetailBusiness source, GiftDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GiftDetailBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.Count = source.Count;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Gift\\GiftDetailMapper.cs", 34);
			throw;
		}
	}

	public Task<GiftDetailBusiness> MapAsync(string contextGuid, WebGiftDetail source, GiftDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GiftDetailBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.Count = source.Count;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Gift\\GiftDetailMapper.cs", 55);
			throw;
		}
	}

	public Task<WebGiftDetail> MapAsync(string contextGuid, GiftDetailBusiness source, WebGiftDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebGiftDetail();
			}
			destination.ProductGuid = source.ProductGuid;
			destination.Count = source.Count;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Gift\\GiftDetailMapper.cs", 75);
			throw;
		}
	}

	public Task<List<GiftDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<GiftDetailBusiness> source, List<GiftDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GiftDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebGiftDetail> source, List<GiftDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebGiftDetail>> MapListAsync(string contextGuid, IEnumerable<GiftDetailBusiness> source, List<WebGiftDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
