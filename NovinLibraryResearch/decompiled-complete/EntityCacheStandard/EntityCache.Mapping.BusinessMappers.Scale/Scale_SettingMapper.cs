using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_SettingMapper : IItemAsyncMapper<Scale_SettingBusiness, Scale_SettingBusiness>
{
	public static Scale_SettingMapper Instance { get; private set; } = new Scale_SettingMapper();


	public Task<Scale_SettingBusiness> MapAsync(string contextGuid, Scale_SettingBusiness source, Scale_SettingBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_SettingBusiness(contextGuid);
			}
			destination.Name = source.Name;
			destination.Val = source.Val;
			destination.UserGuid = source.UserGuid;
			destination.Modified = source.Modified;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_SettingMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_SettingBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_SettingBusiness> source, List<Scale_SettingBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
