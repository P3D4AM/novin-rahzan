using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class OwnerMapper : IItemAsyncMapper<OwnerBusiness, OwnerBusiness>
{
	public static OwnerMapper Instance { get; private set; } = new OwnerMapper();


	public Task<OwnerBusiness> MapAsync(string contextGuid, OwnerBusiness source, OwnerBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new OwnerBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Family = source.Family;
			destination.VisitorGuid = source.VisitorGuid;
			destination.Status = source.Status;
			destination.IsChecked = source.IsChecked;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\OwnerMapper.cs", 37);
			throw;
		}
	}

	public Task<List<OwnerBusiness>> MapListAsync(string contextGuid, IEnumerable<OwnerBusiness> source, List<OwnerBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
