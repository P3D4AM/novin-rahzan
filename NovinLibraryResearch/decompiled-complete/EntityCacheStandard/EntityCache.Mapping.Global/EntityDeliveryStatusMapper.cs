using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class EntityDeliveryStatusMapper : IItemAsyncMapper<EntityDeliveryStatusBusiness, EntityDeliveryStatusBusiness>
{
	public static EntityDeliveryStatusMapper Instance { get; set; } = new EntityDeliveryStatusMapper();


	public Task<EntityDeliveryStatusBusiness> MapAsync(string contextGuid, EntityDeliveryStatusBusiness source, EntityDeliveryStatusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new EntityDeliveryStatusBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityPart = source.EntityPart;
			destination.IsTemp = source.IsTemp;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\EntityDeliveryStatusMapper.cs", 34);
			throw;
		}
	}

	public Task<List<EntityDeliveryStatusBusiness>> MapListAsync(string contextGuid, IEnumerable<EntityDeliveryStatusBusiness> source, List<EntityDeliveryStatusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
