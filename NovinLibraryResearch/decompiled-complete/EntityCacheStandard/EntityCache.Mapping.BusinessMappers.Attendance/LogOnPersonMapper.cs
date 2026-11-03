using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class LogOnPersonMapper : IItemAsyncMapper<LogOnPersonBusiness, WebAttLogOnPerson>, IItemAsyncMapper<LogOnPersonBusiness, LogOnPersonBusiness>, IItemAsyncMapper<WebAttLogOnPerson, LogOnPersonBusiness>
{
	public static LogOnPersonMapper Instance { get; private set; } = new LogOnPersonMapper();


	public Task<LogOnPersonBusiness> MapAsync(string contextGuid, WebAttLogOnPerson source, LogOnPersonBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LogOnPersonBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateTimeLogM = source.DateTimeLogM;
			destination.TrafficGuid = source.TrafficGuid;
			destination.PersonGuid = source.PersonGuid;
			destination.PersonNo = source.PersonNo;
			destination.DeviceId_ = source.DeviceId_;
			destination.DeviceSerial_ = source.DeviceSerial_;
			destination.TrafficName = source.TrafficName;
			destination.Calced = source.Calced;
			destination.TypeTaradod = source.TypeTaradod;
			destination.TypeTaradodVacationOrMission = source.TypeTaradodVacationOrMission;
			destination.Duration = source.Duration;
			destination.DateInserted = source.DateInserted;
			destination.IsVirtual = source.IsVirtual;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\LogOnPersonMapper.cs", 49);
			throw;
		}
	}

	public Task<LogOnPersonBusiness> MapAsync(string contextGuid, LogOnPersonBusiness source, LogOnPersonBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LogOnPersonBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateTimeLogM = source.DateTimeLogM;
			destination.TrafficGuid = source.TrafficGuid;
			destination.PersonGuid = source.PersonGuid;
			destination.PersonNo = source.PersonNo;
			destination.DeviceId_ = source.DeviceId_;
			destination.TrafficName = source.TrafficName;
			destination.Calced = source.Calced;
			destination.TypeTaradod = source.TypeTaradod;
			destination.TypeTaradodVacationOrMission = source.TypeTaradodVacationOrMission;
			destination.Duration = source.Duration;
			destination.DateInserted = source.DateInserted;
			destination.IsVirtual = source.IsVirtual;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\LogOnPersonMapper.cs", 84);
			throw;
		}
	}

	public Task<WebAttLogOnPerson> MapAsync(string contextGuid, LogOnPersonBusiness source, WebAttLogOnPerson destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAttLogOnPerson();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateTimeLogM = source.DateTimeLogM;
			destination.TrafficGuid = source.TrafficGuid;
			destination.PersonGuid = source.PersonGuid;
			destination.PersonNo = source.PersonNo;
			destination.DeviceId_ = source.DeviceId_;
			destination.DeviceSerial_ = source.DeviceSerial_;
			destination.TrafficName = source.TrafficName;
			destination.Calced = source.Calced;
			destination.TypeTaradod = source.TypeTaradod;
			destination.TypeTaradodVacationOrMission = source.TypeTaradodVacationOrMission;
			destination.Duration = source.Duration;
			destination.DateInserted = source.DateInserted;
			destination.IsVirtual = source.IsVirtual;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\LogOnPersonMapper.cs", 120);
			throw;
		}
	}

	public Task<List<LogOnPersonBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAttLogOnPerson> source, List<LogOnPersonBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LogOnPersonBusiness>> MapListAsync(string contextGuid, IEnumerable<LogOnPersonBusiness> source, List<LogOnPersonBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebAttLogOnPerson>> MapListAsync(string contextGuid, IEnumerable<LogOnPersonBusiness> source, List<WebAttLogOnPerson> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
