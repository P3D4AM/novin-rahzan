using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class RasGiriMapper : IItemAsyncMapper<RasGiriBusiness, WebRasGiri>, IItemAsyncMapper<RasGiriBusiness, RasGiriBusiness>, IItemAsyncMapper<WebRasGiri, RasGiriBusiness>
{
	public static RasGiriMapper Instance { get; private set; } = new RasGiriMapper();


	public async Task<WebRasGiri> MapAsync(string contextGuid, RasGiriBusiness source, WebRasGiri destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebRasGiri();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Description = source.Description;
			WebRasGiri webRasGiri = destination;
			webRasGiri.Items = await RasGiriDetMapper.Instance.MapListAsync(contextGuid, source.Items, new List<WebRasGiriDet>(), tranName, token);
			destination.DateM = source.DateM;
			destination.BaseDateM = source.BaseDateM;
			destination.DayCount = source.DayCount;
			destination.RecordCount = source.RecordCount;
			destination.Sood_Mounthly = source.Sood_Mounthly;
			destination.Sood_Total = source.Sood_Total;
			destination.Sood_Percent = source.Sood_Percent;
			destination.Total_Sum = source.Total_Sum;
			destination.TafsilGuid = source.TafsilGuid;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\RasGiriMapper.cs", 45);
			throw;
		}
		return destination;
	}

	public Task<List<WebRasGiri>> MapListAsync(string contextGuid, IEnumerable<RasGiriBusiness> source, List<WebRasGiri> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<RasGiriBusiness> MapAsync(string contextGuid, RasGiriBusiness source, RasGiriBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new RasGiriBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			foreach (RasGiriDetBusiness item in await RasGiriDetMapper.Instance.MapListAsync(contextGuid, source.Items, new List<RasGiriDetBusiness>(), tranName, token))
			{
				destination.AddItem(item, token);
			}
			destination.BaseDateM = source.BaseDateM;
			destination.Sood_Percent = source.Sood_Percent;
			destination.TafsilGuid = source.TafsilGuid;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\RasGiriMapper.cs", 78);
			throw;
		}
		return destination;
	}

	public Task<List<RasGiriBusiness>> MapListAsync(string contextGuid, IEnumerable<RasGiriBusiness> source, List<RasGiriBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<RasGiriBusiness> MapAsync(string contextGuid, WebRasGiri source, RasGiriBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new RasGiriBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			foreach (RasGiriDetBusiness item in await RasGiriDetMapper.Instance.MapListAsync(contextGuid, source.Items, new List<RasGiriDetBusiness>(), tranName, token))
			{
				destination.AddItem(item, token);
			}
			destination.BaseDateM = source.BaseDateM;
			destination.Sood_Percent = source.Sood_Percent;
			destination.TafsilGuid = source.TafsilGuid;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\RasGiriMapper.cs", 111);
			throw;
		}
		return destination;
	}

	public Task<List<RasGiriBusiness>> MapListAsync(string contextGuid, IEnumerable<WebRasGiri> source, List<RasGiriBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
