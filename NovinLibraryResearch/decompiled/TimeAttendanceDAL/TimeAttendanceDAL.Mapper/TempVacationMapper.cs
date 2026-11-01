using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Mapper;

public class TempVacationMapper : IItemAsyncMapper<Morakhasidaily, WebAttTempVacations>
{
	public static TempVacationMapper Instance { get; private set; } = new TempVacationMapper();


	public Task<WebAttTempVacations> MapAsync(string contextGuid, Morakhasidaily source, WebAttTempVacations destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult<WebAttTempVacations>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttTempVacations();
			}
			destination.Guid = source.Guid;
			destination.PersonGuid = source.PersonGuid;
			destination.Reason = source.Desc;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.Kind = source.Kind;
			destination.Type = source.Type;
			return Task.FromResult<WebAttTempVacations>(destination);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Task<List<WebAttTempVacations>> MapListAsync(string contextGuid, IEnumerable<Morakhasidaily> source, List<WebAttTempVacations> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<Morakhasidaily, WebAttTempVacations>(source, contextGuid, (IItemAsyncMapper<Morakhasidaily, WebAttTempVacations>)this, destination, tranName, (short)20);
	}
}
