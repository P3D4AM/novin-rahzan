using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Mapper;

public class MorakhasiMapper : IItemAsyncMapper<Morakhasi, WebAttMorakhasi>
{
	public static MorakhasiMapper Instance { get; } = new MorakhasiMapper();


	public Task<WebAttMorakhasi> MapAsync(string contextGuid, Morakhasi source, WebAttMorakhasi destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult<WebAttMorakhasi>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttMorakhasi();
			}
			destination.SumVacationSaati = source.SumVacationSaati;
			destination.SumVacationLeft = source.SumVacationLeft;
			destination.SumVacationSaatiEquivalentDay = source.SumVacationSaatiEquivalentDay;
			destination.SumVacationUsed = source.SumVacationUsed;
			destination.GharardadId = source.GharardadId;
			destination.TotalVacationDaysLeft = source.TotalVacationDaysLeft;
			destination.TotalVacationHourLeft = source.TotalVacationHourLeft;
			destination.VacationDaysSavedFromGharardad = source.VacationDaysSavedFromGharardad;
			destination.VacationDaysFromGharardad = source.VacationDaysFromGharardad;
			destination.RemindVacationToNow = source.RemindVacationToNow;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return Task.FromResult<WebAttMorakhasi>(destination);
	}

	public Task<List<WebAttMorakhasi>> MapListAsync(string contextGuid, IEnumerable<Morakhasi> source, List<WebAttMorakhasi> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<Morakhasi, WebAttMorakhasi>(source, contextGuid, (IItemAsyncMapper<Morakhasi, WebAttMorakhasi>)this, destination, tranName, (short)20);
	}
}
