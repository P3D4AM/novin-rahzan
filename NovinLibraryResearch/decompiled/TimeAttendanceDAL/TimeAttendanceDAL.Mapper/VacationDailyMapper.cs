using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Mapper;

public class VacationDailyMapper : IItemAsyncMapper<AttPersonLeaveBusiness, Morakhasidaily>
{
	public static VacationDailyMapper Instance { get; } = new VacationDailyMapper();


	public async Task<Morakhasidaily> MapAsync(string contextGuid, AttPersonLeaveBusiness source, Morakhasidaily destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Task<List<Morakhasidaily>> MapListAsync(string contextGuid, IEnumerable<AttPersonLeaveBusiness> source, List<Morakhasidaily> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<AttPersonLeaveBusiness, Morakhasidaily>(source, contextGuid, (IItemAsyncMapper<AttPersonLeaveBusiness, Morakhasidaily>)this, destination, tranName, (short)20);
	}
}
