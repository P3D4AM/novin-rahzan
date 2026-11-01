using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Mapper;

public class TaradodDetMapper : IItemAsyncMapper<ATT_TaradodDet, WebAttTaradodDet>
{
	public static TaradodDetMapper Instance { get; } = new TaradodDetMapper();


	public Task<WebAttTaradodDet> MapAsync(string contextGuid, ATT_TaradodDet source, WebAttTaradodDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			if (source == null)
			{
				destination = null;
				return null;
			}
			if (destination == null)
			{
				destination = new WebAttTaradodDet();
			}
			destination.DateM = source.DateM;
			destination.VacationTime = source.VacationTime;
			destination.VacationStart = source.VacationStart;
			destination.VacationEnd = source.VacationEnd;
			destination.VacationDesc = source.VacationDesc;
			destination.EKTime = source.EKTime;
			destination.EKStart = source.EKStart;
			destination.EKEnd = source.EKEnd;
			destination.EKDesc = source.EKDesc;
			destination.KKTime = source.KKTime;
			destination.KKStart = source.KKStart;
			destination.KKEnd = source.KKEnd;
			destination.KKDesc = source.KKDesc;
			destination.MissionTime = source.MissionTime;
			destination.MissionStart = source.MissionStart;
			destination.MissionEnd = source.MissionEnd;
			destination.MissionDesc = source.MissionDesc;
			destination.FridayTime = source.FridayTime;
			destination.TatilTime = source.TatilTime;
			destination.FridayStart = source.FridayStart;
			destination.FridayEnd = source.FridayEnd;
			destination.FridayDesc = source.FridayDesc;
			destination.ShiftTime1 = source.ShiftTime1;
			destination.ShiftTime2 = source.ShiftTime2;
			destination.ShiftTime3 = source.ShiftTime3;
			destination.ShiftStart1 = source.ShiftStart1;
			destination.ShiftStart2 = source.ShiftStart2;
			destination.ShiftStart3 = source.ShiftStart3;
			destination.ShiftEnd1 = source.ShiftEnd1;
			destination.ShiftEnd2 = source.ShiftEnd2;
			destination.ShiftEnd3 = source.ShiftEnd3;
			destination.ShiftDesc1 = source.ShiftDesc1;
			destination.ShiftDesc2 = source.ShiftDesc2;
			destination.ShiftDesc3 = source.ShiftDesc3;
			destination.AbsentTime = source.AbsentTime;
			destination.AbsendStart = source.AbsendStart;
			destination.AbsentEnd = source.AbsentEnd;
			destination.AbsentDesc = source.AbsentDesc;
			destination.NightTime = source.NightTime;
			destination.NightStart = source.NightStart;
			destination.NightEnd = source.NightEnd;
			destination.NightDesc = source.NightDesc;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return Task.FromResult<WebAttTaradodDet>(destination);
	}

	public Task<List<WebAttTaradodDet>> MapListAsync(string contextGuid, IEnumerable<ATT_TaradodDet> source, List<WebAttTaradodDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<ATT_TaradodDet, WebAttTaradodDet>(source, contextGuid, (IItemAsyncMapper<ATT_TaradodDet, WebAttTaradodDet>)this, destination, tranName, (short)20);
	}
}
