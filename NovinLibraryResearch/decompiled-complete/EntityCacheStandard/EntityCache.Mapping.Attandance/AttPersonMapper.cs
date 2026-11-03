using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Attandance;

public class AttPersonMapper : IItemAsyncMapper<AttPersonBusiness, AttPersonBusiness>
{
	public static AttPersonMapper Instance { get; private set; } = new AttPersonMapper();


	public Task<AttPersonBusiness> MapAsync(string contextGuid, AttPersonBusiness source, AttPersonBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AttPersonBusiness(contextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttPersonMapper.cs", 28);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<AttPersonBusiness>> MapListAsync(string contextGuid, IEnumerable<AttPersonBusiness> source, List<AttPersonBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
