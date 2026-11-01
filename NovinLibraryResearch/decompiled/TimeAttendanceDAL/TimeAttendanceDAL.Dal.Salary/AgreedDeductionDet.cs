using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class AgreedDeductionDet : IHasContext
{
	private Fish _clsFish;

	public string ContextGuid { get; set; }

	public int FishId { get; set; }

	public string MonthYear { get; set; } = "";


	public decimal Price { get; set; }

	public decimal Remnant { get; set; }

	[JsonIgnore]
	public bool Checked { get; set; } = true;


	public async Task<Fish> GetClsFishAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			Fish fish = _clsFish;
			if (fish == null)
			{
				fish = (_clsFish = await Fish.GetAsync(ContextGuid, FishId, loadClsTaradod: false, token));
			}
			return fish;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<int> GetFishNoAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			return (await GetClsFishAsync(token)).FishNo;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<string> GetDateCaltureDescAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			ICalendar caltureCalender = LanguageAndCalendar.Instance.CaltureCalender;
			string monthName = caltureCalender.GetMonthName((await GetClsFishAsync(token)).DateStartM);
			string translate = Utilities.GetTranslate(" [ماه سال] ");
			return monthName + translate + Calendar.MiladiToCalture((await GetClsFishAsync(token)).DateStartM).Substring(0, 4);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public AgreedDeductionDet(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
