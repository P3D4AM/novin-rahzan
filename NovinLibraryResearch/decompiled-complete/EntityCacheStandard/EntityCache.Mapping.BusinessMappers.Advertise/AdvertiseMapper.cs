using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class AdvertiseMapper : IItemAsyncMapper<AdvertiseBusiness, AdvertiseBusiness>
{
	public static AdvertiseMapper Instance { get; private set; } = new AdvertiseMapper();


	public Task<AdvertiseBusiness> MapAsync(string contextGuid, AdvertiseBusiness source, AdvertiseBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AdvertiseBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContentList = source.ContentList;
			destination.DivatCategoryList = source.DivatCategoryList;
			destination.GroupGuid = source.GroupGuid;
			destination.ImageList = source.ImageList;
			destination.IsPishNevis = source.IsPishNevis;
			destination.Price = source.Price;
			destination.SheypoorCategoryList = source.SheypoorCategoryList;
			destination.TitleList = source.TitleList;
			destination.Name = source.Name;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\AdvertiseMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<AdvertiseBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvertiseBusiness> source, List<AdvertiseBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
