using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.Mapper;

public class CoupleTaradodMapper : IItemAsyncMapper<CoupleTaradod, WebAttCoupleTaradod>
{
	public static CoupleTaradodMapper Instance { get; } = new CoupleTaradodMapper();


	public Task<WebAttCoupleTaradod> MapAsync(string contextGuid, CoupleTaradod source, WebAttCoupleTaradod destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult<WebAttCoupleTaradod>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttCoupleTaradod();
			}
			destination.Ktaradod = LogonPersonalMapper.Instance.Map(contextGuid, source.Ktaradod);
			destination.Vtaradod = LogonPersonalMapper.Instance.Map(contextGuid, source.Vtaradod);
			destination.IsTaradodVirtual = source.IsTaradodVirtual;
			destination.DateM = source.DateM;
			destination.KTime = source.KTime;
			destination.VTime = source.VTime;
			destination.TypeTaradod = source.TypeTaradod;
			destination.DeviceId = source.DeviceId;
			destination.StatuseTaradodLst = source.StatuseTaradodLst;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return Task.FromResult<WebAttCoupleTaradod>(destination);
	}

	public Task<List<WebAttCoupleTaradod>> MapListAsync(string contextGuid, IEnumerable<CoupleTaradod> source, List<WebAttCoupleTaradod> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<CoupleTaradod, WebAttCoupleTaradod>(source, contextGuid, (IItemAsyncMapper<CoupleTaradod, WebAttCoupleTaradod>)this, destination, tranName, (short)20);
	}
}
