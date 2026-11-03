using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Issabel;
using PacketParser.Mapping;

namespace EntityCache.CallCenter.Mapper;

public class IsabelleCallCenterMapper : IItemAsyncMapper<IsabelleCallCenterBase, IsabelleCallCenter>, IItemAsyncMapper<IsabelleCallCenter, IsabelleCallCenterBase>
{
	public static IsabelleCallCenterMapper Instance { get; private set; } = new IsabelleCallCenterMapper();


	public Task<IsabelleCallCenter> MapAsync(string contextGuid, IsabelleCallCenterBase source, IsabelleCallCenter destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new IsabelleCallCenter(contextGuid);
			}
			destination.Ip = source.Ip;
			destination.Port = source.Port;
			destination.Queues = source.Queues;
			destination.UserName = source.UserName;
			destination.Password = source.Password;
			destination.FriendllyName = source.FriendllyName;
			destination.LineReleaseCode = source.LineReleaseCode;
			destination.CallCenterStatus = source.CallCenterStatus;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\Mapper\\IsabelleCallCenterMapper.cs", 37);
			throw;
		}
	}

	public Task<IsabelleCallCenterBase> MapAsync(string contextGuid, IsabelleCallCenter source, IsabelleCallCenterBase destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new IsabelleCallCenterBase();
			}
			destination.Ip = source.Ip;
			destination.Port = source.Port;
			destination.Queues = source.Queues;
			destination.UserName = source.UserName;
			destination.Password = source.Password;
			destination.FriendllyName = source.FriendllyName;
			destination.LineReleaseCode = source.LineReleaseCode;
			destination.CallCenterStatus = source.CallCenterStatus;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CallCenter\\Mapper\\IsabelleCallCenterMapper.cs", 63);
			throw;
		}
	}

	public Task<List<IsabelleCallCenter>> MapListAsync(string contextGuid, IEnumerable<IsabelleCallCenterBase> source, List<IsabelleCallCenter> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<IsabelleCallCenterBase>> MapListAsync(string contextGuid, IEnumerable<IsabelleCallCenter> source, List<IsabelleCallCenterBase> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
