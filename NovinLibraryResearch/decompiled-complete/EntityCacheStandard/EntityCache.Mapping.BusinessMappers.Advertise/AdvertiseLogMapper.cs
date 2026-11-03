using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class AdvertiseLogMapper : IItemAsyncMapper<AdvertiseLogBusiness, AdvertiseLogBusiness>
{
	public static AdvertiseLogMapper Instance { get; private set; } = new AdvertiseLogMapper();


	public Task<AdvertiseLogBusiness> MapAsync(string contextGuid, AdvertiseLogBusiness source, AdvertiseLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AdvertiseLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Content = source.Content;
			destination.SimCardNumber = source.SimCardNumber;
			destination.State = source.State;
			destination.City = source.City;
			destination.Region = source.Region;
			destination.Price = source.Price;
			destination.Category = source.Category;
			destination.SubCategory1 = source.SubCategory1;
			destination.SubCategory2 = source.SubCategory2;
			destination.URL = source.URL;
			destination.IP = source.IP;
			destination.UpdateDesc = source.UpdateDesc;
			destination.VisitCount = source.VisitCount;
			destination.ImagesPathList = source.ImagesPathList?.ToList();
			destination.DateSh = source.DateSh;
			destination.DateM = source.DateM;
			destination.Adv = source.Adv;
			destination.MasterVisitorGuid = source.MasterVisitorGuid;
			destination.SlaveVisitorGuid = source.SlaveVisitorGuid;
			destination.StatusCode = source.StatusCode;
			destination.AdvType = source.AdvType;
			destination.LastUpdate = source.LastUpdate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\AdvertiseLogMapper.cs", 55);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<AdvertiseLogBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvertiseLogBusiness> source, List<AdvertiseLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
