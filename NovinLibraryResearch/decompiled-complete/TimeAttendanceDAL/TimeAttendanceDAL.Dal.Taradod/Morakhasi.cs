using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;

namespace TimeAttendanceDAL.Dal.Taradod;

public class Morakhasi : IMorakhasi, IHasContext
{
	private double _sumVacationLeft;

	public string ContextGuid { get; set; }

	public int GharardadId { get; set; }

	public string FullName { get; set; }

	public int SumVacationUsed { get; set; }

	public int SumVacationSaati { get; set; }

	public double SumVacationLeft
	{
		get
		{
			return Math.Round(_sumVacationLeft, 2);
		}
		set
		{
			_sumVacationLeft = value;
		}
	}

	public string SumVacationSaatiTime => Time.IntToTime(SumVacationSaati);

	public string SumVacationSaatiEquivalentDay { get; set; }

	public double VacationDaysFromGharardad { get; set; }

	public double VacationDaysSavedFromGharardad { get; set; }

	public double SumVacationDaysForUsedByGharardad => VacationDaysFromGharardad + VacationDaysSavedFromGharardad;

	public Color VacationLeftColor
	{
		get
		{
			if (!(SumVacationLeft > 0.0))
			{
				return Color.Red;
			}
			return Color.Green;
		}
	}

	public int TotalVacationDaysLeft { get; set; }

	public string TotalVacationHourLeft { get; set; }

	public int RemindVacationToNow { get; set; }

	public void CalculateSumVacationLeft(short leaveMinutes)
	{
		try
		{
			List<string> list = SumVacationLeft.ToString().Split(new char[1] { '.' })?.ToList();
			if (list == null || !list.Any() || list.Count > 2)
			{
				return;
			}
			TotalVacationDaysLeft = Utilities.ParseToInt(list.First(), 0);
			if (list.Last() != null && list.Count != 1)
			{
				double num = Utilities.ParseToDouble("0." + list[1], 0.0, (short)3) * (double)leaveMinutes;
				if (!(num <= 0.0))
				{
					TotalVacationHourLeft = Time.IntToTime(num, "mm:ss");
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public static async Task<Morakhasi> GetAsync(string contextGuid, int gharardadId, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			Morakhasi ret = new Morakhasi
			{
				ContextGuid = contextGuid
			};
			if (gharardadId <= 0)
			{
				return ret;
			}
			GharardadNew gharardadNew = await GharardadNew.GetAsync(contextGuid, gharardadId, token);
			if (gharardadNew == null)
			{
				return ret;
			}
			Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> tuple = await Morakhasidaily.CalcLeaveBalanceAsync(contextGuid, gharardadNew, token);
			if (tuple == null)
			{
				return ret;
			}
			ret.SumVacationUsed = tuple.Item1;
			ret.SumVacationSaati = tuple.Item2;
			ret.SumVacationLeft = tuple.Item3;
			GharardadNew item = tuple.Item4;
			ret.FullName = item?.FullName;
			ret.SumVacationSaatiEquivalentDay = ((double)ret.SumVacationSaati / (double)item.LeaveMinutes).ToString("0.00");
			ret.VacationDaysSavedFromGharardad = (item?.LeaveSaved).Value;
			ret.VacationDaysFromGharardad = (item?.LeaveDays).Value;
			ret.CalculateSumVacationLeft(item.LeaveMinutes);
			int days = (item.DateFinishM - item.DateStartM).Days;
			int remindVacationToNow = (int)((double)(DateTime.Now - item.DateStartM).Days / (double)days * (item.LeaveSaved + item.TotalLeaveDaysInGharardad));
			ret.RemindVacationToNow = remindVacationToNow;
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
