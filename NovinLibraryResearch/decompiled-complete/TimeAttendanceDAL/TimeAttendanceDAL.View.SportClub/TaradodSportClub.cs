using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceDAL.View.SportClub;

[Serializable]
[DoNotPruneType]
public class TaradodSportClub : IHasContext
{
	private DateTime Date1;

	private DateTime Date2;

	private GharardadNew _gharardad;

	public string ContextGuid { get; set; }

	public Person ClsPerson { get; private set; }

	public List<TaradodSportClubDet> ListSportClubDets { get; set; }

	public string DateStartSh => Calendar.MiladiToCalture(Date1);

	public string DateEndSh => Calendar.MiladiToCalture(Date2);

	public int CountAll => ListSportClubDets.Count;

	public int SumKarkerd { get; set; }

	public string SumKarkerdTime { get; private set; }

	public int CountKarkerd { get; private set; }

	public short CountGheybat { get; set; }

	public int Sumshift { get; private set; }

	public async Task<GharardadNew> GetGharardadAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			GharardadNew gharardadNew = _gharardad;
			if (gharardadNew == null)
			{
				gharardadNew = (_gharardad = await GharardadNew.GetAsync(ContextGuid, ((PersonBase)ClsPerson).Id, Date1, Date2, token));
			}
			return gharardadNew;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public TaradodSportClub(string contextGuid)
	{
		ContextGuid = contextGuid;
		Date1 = DateTime.Now;
		Date2 = DateTime.Now;
		ListSportClubDets = new List<TaradodSportClubDet>();
	}

	public TaradodSportClub(string contextGuid, TaradodSportClub pclsFull)
	{
		ContextGuid = contextGuid;
		ListSportClubDets = pclsFull.ListSportClubDets.ToList();
		ClsPerson = pclsFull.ClsPerson;
		Date1 = pclsFull.Date1;
		Date2 = pclsFull.Date2;
		SumKarkerdTime = pclsFull.SumKarkerdTime;
		SumKarkerd = pclsFull.SumKarkerd;
		CountKarkerd = pclsFull.CountKarkerd;
		CountGheybat = pclsFull.CountGheybat;
		Sumshift = pclsFull.Sumshift;
	}

	private TaradodSportClub()
	{
	}

	public static async Task<TaradodSportClub> GetAsync(string contextGuid, DateTime pDate1, DateTime pDate2, Person pperson, CancellationToken token)
	{
		_ = 1;
		try
		{
			TaradodSportClub taradodSport = new TaradodSportClub
			{
				ContextGuid = contextGuid,
				Date1 = pDate1.Date,
				Date2 = pDate2.Date,
				ClsPerson = pperson,
				ListSportClubDets = new List<TaradodSportClubDet>()
			};
			int xxxx = taradodSport.Date2.Subtract(taradodSport.Date1).Days;
			for (int i = 0; i <= xxxx; i++)
			{
				List<TaradodSportClubDet> listSportClubDets = taradodSport.ListSportClubDets;
				listSportClubDets.Add(await TaradodSportClubDet.GetAsync(taradodSport.ContextGuid, taradodSport.Date1.Date.AddDays(i + 1).AddSeconds(-1.0), taradodSport.ClsPerson));
			}
			await taradodSport.FillPropertyAsync(token);
			return taradodSport;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task FillPropertyAsync(CancellationToken token)
	{
		int i = 0;
		try
		{
			i = 1;
			Calendar.MiladiToCalture(Date1);
			i = 4;
			TaradodSportClub taradodSportClub = this;
			TaradodSportClub taradodSportClub2 = this;
			int num2 = (SumKarkerd = 0);
			int sumshift = (taradodSportClub2.CountKarkerd = num2);
			taradodSportClub.Sumshift = sumshift;
			i = 5;
			CountGheybat = 0;
			i = 6;
			if (ListSportClubDets != null && ListSportClubDets.Count > 0)
			{
				i = 10;
				foreach (TaradodSportClubDet item in ListSportClubDets)
				{
					i = 13;
					int sumKarkerd = SumKarkerd;
					SumKarkerd = sumKarkerd + await item.GetKarkardAsync(token);
					i = 14;
					if (await item.GetKarkardAsync(token) > 0)
					{
						CountKarkerd++;
					}
					i = 24;
					bool flag = (int)(await item.GetStatuseTaradodAsync(token)) == 3;
					if (!flag)
					{
						flag = (int)(await item.GetStatuseTaradodAsync(token)) == 12;
					}
					bool flag2 = flag;
					if (!flag2)
					{
						flag2 = (int)(await item.GetStatuseTaradodAsync(token)) == 13;
					}
					if (flag2)
					{
						CountGheybat++;
					}
					i = 25;
					if (await item.GetSumShiftMinutsAsync(token) > 0)
					{
						i = 29;
						sumKarkerd = Sumshift;
						Sumshift = sumKarkerd + await item.GetSumShiftMinutsAsync(token);
						i = 30;
					}
					i = 31;
				}
			}
			i = 37;
			SumKarkerdTime = Time.IntToTime(SumKarkerd);
			i = 38;
			i = 53;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "Line Noooo:" + i, "FillPropertyAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\SportClub\\TaradodSportClub.cs", 156);
			throw;
		}
	}
}
