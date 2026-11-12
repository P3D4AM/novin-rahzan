using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;
using Server.Core.Assistance;
using Server.Core.Connection;

namespace Server.Core.Mapper;

public class ConnectionMapper : IItemAsyncMapper<ClsConnectionData, ConnectionData>, IItemAsyncMapper<ConnectionData, ClsConnectionData>
{
	public static ConnectionMapper Instance { get; private set; } = new ConnectionMapper();


	public async Task<ConnectionData> MapAsync(string contextGuid, ClsConnectionData source, ConnectionData destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			WebUser user = await Initializer.Dal.User.LoadDataAsync(source.UserId);
			if (destination == null)
			{
				destination = new ClsConnectionData(user, null);
			}
			destination.UserDepGuid = source.UserDepGuid;
			destination.AndroidIme = source.AndroidIme;
			destination.ClientVersion = source.ClientVersion;
			destination.CompressedMode = source.CompressedMode;
			destination.ConnectionStartTime = source.ConnectionStartTime;
			destination.Context = source.Context;
			destination.CustomerTafsilGuid = source.CustomerTafsilGuid;
			destination.DbGuid = source.DbGuid;
			destination.DepId = source.DepId;
			destination.Guid = source.Guid;
			destination.HashId = source.HashId;
			destination.Ip = source.Ip;
			destination.LkSerial = source.LkSerial;
			destination.Source = source.Source;
			destination.TotalRecivedBytes = source.TotalRecivedBytes;
			destination.TotalRecivedForDisplay = source.TotalRecivedForDisplay;
			destination.UserAccGuid = source.UserAccGuid;
			destination.UserId = source.UserId;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Mapper\\ConnectionMapper.cs", 47);
			throw;
		}
	}

	public Task<List<ConnectionData>> MapListAsync(string contextGuid, IEnumerable<ClsConnectionData> source, List<ConnectionData> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ClsConnectionData> MapAsync(string contextGuid, ConnectionData source, ClsConnectionData destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			WebUser user = await Initializer.Dal.User.LoadDataAsync(source.UserId);
			if (destination == null)
			{
				destination = new ClsConnectionData(user, null);
			}
			destination.UserDepGuid = source.UserDepGuid;
			destination.AndroidIme = source.AndroidIme;
			destination.ClientVersion = source.ClientVersion;
			destination.CompressedMode = source.CompressedMode;
			destination.ConnectionStartTime = source.ConnectionStartTime;
			destination.Context = source.Context;
			destination.CustomerTafsilGuid = source.CustomerTafsilGuid;
			destination.DbGuid = source.DbGuid;
			destination.DepId = source.DepId;
			destination.Guid = source.Guid;
			destination.HashId = source.HashId;
			destination.Ip = source.Ip;
			destination.LkSerial = source.LkSerial;
			destination.Source = source.Source;
			destination.TotalRecivedBytes = source.TotalRecivedBytes;
			destination.TotalRecivedForDisplay = source.TotalRecivedForDisplay;
			destination.UserAccGuid = source.UserAccGuid;
			destination.UserId = source.UserId;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Mapper\\ConnectionMapper.cs", 86);
			throw;
		}
	}

	public Task<List<ClsConnectionData>> MapListAsync(string contextGuid, IEnumerable<ConnectionData> source, List<ClsConnectionData> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
