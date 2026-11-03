using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Attendance.Business;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Attandance;

public class CalenderMapper : IItemAsyncMapper<WebCalender, CalenderBusiness>, IItemAsyncMapper<CalenderBusiness, CalenderBusiness>
{
	public static CalenderMapper Instance { get; private set; } = new CalenderMapper();


	public Task<CalenderBusiness> MapAsync(string contextGuid, WebCalender source, CalenderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CalenderBusiness(contextGuid);
			}
			destination.Guid = Guid.NewGuid();
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.Monasebat = source.Monasebat ?? "";
			destination.STRasmi = source.STRasmi;
			destination.Description = source.Description ?? "";
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Attandance\\CalenderMapper.cs", 38);
			throw;
		}
	}

	public Task<CalenderBusiness> MapAsync(string contextGuid, CalenderBusiness source, CalenderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CalenderBusiness(contextGuid);
			}
			destination.Guid = Guid.NewGuid();
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.Monasebat = source.Monasebat ?? "";
			destination.STRasmi = source.STRasmi;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Attandance\\CalenderMapper.cs", 63);
			throw;
		}
	}

	public Task<List<CalenderBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCalender> source, List<CalenderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CalenderBusiness>> MapListAsync(string contextGuid, IEnumerable<CalenderBusiness> source, List<CalenderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
