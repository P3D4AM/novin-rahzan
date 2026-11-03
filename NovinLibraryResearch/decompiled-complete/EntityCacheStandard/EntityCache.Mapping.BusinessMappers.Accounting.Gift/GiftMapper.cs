using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Gift;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Gift;

public class GiftMapper : IItemAsyncMapper<GiftBusiness, GiftBusiness>, IItemAsyncMapper<WebGift, GiftBusiness>, IItemAsyncMapper<GiftBusiness, WebGift>
{
	public static GiftMapper Instance { get; private set; } = new GiftMapper();


	public async Task<GiftBusiness> MapAsync(string contextGuid, GiftBusiness source, GiftBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GiftBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.Title = source.Title;
			destination.Discription = source.Discription;
			destination.Type = source.Type;
			destination.MetaData = source.MetaData;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				await destination.AddRangeToDetailListAsync(await GiftDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<GiftDetailBusiness>(), tranName, token), token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Gift\\GiftMapper.cs", 46);
			throw;
		}
	}

	public async Task<GiftBusiness> MapAsync(string contextGuid, WebGift source, GiftBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GiftBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.Title = source.Title;
			destination.Discription = source.Discription;
			destination.Type = source.Type;
			destination.MetaData = source.MetaData;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				await destination.AddRangeToDetailListAsync(await GiftDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, null, tranName, token), token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Gift\\GiftMapper.cs", 78);
			throw;
		}
	}

	public async Task<WebGift> MapAsync(string contextGuid, GiftBusiness source, WebGift destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebGift();
			}
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.Title = source.Title;
			destination.Discription = source.Discription;
			destination.Type = source.Type;
			destination.MetaData = source.MetaData;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				WebGift webGift = destination;
				webGift.DetailList = await GiftDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<WebGiftDetail>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Gift\\GiftMapper.cs", 106);
			throw;
		}
	}

	public Task<List<GiftBusiness>> MapListAsync(string contextGuid, IEnumerable<GiftBusiness> source, List<GiftBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GiftBusiness>> MapListAsync(string contextGuid, IEnumerable<WebGift> source, List<GiftBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebGift>> MapListAsync(string contextGuid, IEnumerable<GiftBusiness> source, List<WebGift> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
