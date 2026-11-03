using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class GpsMapper : IItemAsyncMapper<GPSBusiness, WebGPS>, IItemAsyncMapper<GPSBusiness, GPSBusiness>, IItemAsyncMapper<WebGPS, GPSBusiness>
{
	public static GpsMapper Instance { get; private set; } = new GpsMapper();


	public Task<WebGPS> MapAsync(string contextGuid, GPSBusiness source, WebGPS destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebGPS();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Accuracy = source.Accuracy;
			destination.Activity = source.Activity;
			destination.Altitude = source.Altitude;
			destination.Bearing = source.Bearing;
			destination.ColorHex = source.ColorHex;
			destination.DateM = source.DateM;
			destination.DateMUtc = source.DateM.ToLocalTime();
			destination.Description = source.Description;
			destination.Distance = source.Distance;
			destination.FromLastPointDistance = source.FromLastPointDistance;
			destination.FromStartPointDistance = source.FromStartPointDistance;
			destination.GPSTime = source.GPSTime;
			destination.Icon = source.Icon;
			destination.Latitude = source.Latitude;
			destination.Longitude = source.Longitude;
			destination.PointDurationSeconds = source.PointDurationSeconds;
			destination.PreviewsID = source.PreviewsID;
			destination.Speed = source.Speed;
			destination.Time = source.Time;
			destination.UId = source.UId;
			destination.UserAccGuid = source.UserAccGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GpsMapper.cs", 52);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebGPS>> MapListAsync(string contextGuid, IEnumerable<GPSBusiness> source, List<WebGPS> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<GPSBusiness> MapAsync(string contextGuid, GPSBusiness source, GPSBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GPSBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Accuracy = source.Accuracy;
			destination.Activity = source.Activity;
			destination.Altitude = source.Altitude;
			destination.Bearing = source.Bearing;
			destination.ColorHex = source.ColorHex;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Distance = source.Distance;
			destination.FromLastPointDistance = source.FromLastPointDistance;
			destination.FromStartPointDistance = source.FromStartPointDistance;
			destination.GPSTime = source.GPSTime;
			destination.Icon = source.Icon;
			destination.Latitude = source.Latitude;
			destination.Longitude = source.Longitude;
			destination.PointDurationSeconds = source.PointDurationSeconds;
			destination.PreviewsID = source.PreviewsID;
			destination.Speed = source.Speed;
			destination.Time = source.Time;
			destination.UId = source.UId;
			destination.UserAccGuid = source.UserAccGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GpsMapper.cs", 96);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<GPSBusiness>> MapListAsync(string contextGuid, IEnumerable<GPSBusiness> source, List<GPSBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<GPSBusiness> MapAsync(string contextGuid, WebGPS source, GPSBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GPSBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Accuracy = source.Accuracy;
			destination.Activity = source.Activity;
			destination.Altitude = source.Altitude;
			destination.Bearing = source.Bearing;
			destination.ColorHex = source.ColorHex;
			destination.DateM = (source.DateMUtc.HasValue ? source.DateMUtc.Value.ToLocalTime() : source.DateM);
			destination.Description = source.Description;
			destination.Distance = source.Distance;
			destination.FromLastPointDistance = source.FromLastPointDistance;
			destination.FromStartPointDistance = source.FromStartPointDistance;
			destination.GPSTime = source.GPSTime;
			destination.Icon = source.Icon;
			destination.Latitude = source.Latitude;
			destination.Longitude = source.Longitude;
			destination.PointDurationSeconds = source.PointDurationSeconds;
			destination.PreviewsID = source.PreviewsID;
			destination.Speed = source.Speed;
			destination.Time = source.Time;
			destination.UId = source.UId;
			destination.UserAccGuid = source.UserAccGuid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GpsMapper.cs", 141);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<GPSBusiness>> MapListAsync(string contextGuid, IEnumerable<WebGPS> source, List<GPSBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
