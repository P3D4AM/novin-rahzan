using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceDAL.Dal.Taradod;

[Serializable]
[DoNotPruneType]
public class JameTaradod : IHasContext
{
	public string ContextGuid { get; set; }

	public int PersonId { get; set; }

	public string FullName { get; set; } = "";


	public int CountAll { get; set; }

	public int GheybatCount { get; set; }

	public DateTime DateM1 { get; set; } = DateTime.Now;


	public DateTime DateM2 { get; set; } = DateTime.Now;


	public string DateSh1 => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateM1);

	public string DateSh2 => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateM2);

	public int GheybatSum { get; set; }

	public int KarkardCount { get; set; }

	public int KarkardSum { get; set; }

	public int ShiftCount { get; set; }

	public int ShiftSum { get; set; }

	public string SumShiftTime { get; set; }

	public int EkCount { get; set; }

	public int EkSum { get; set; }

	public string EkSumTime => (EkSum % 60).ToString().PadLeft(2, '0') + " : " + (EkSum / 60).ToString().PadLeft(2, '0');

	public int KkCount { get; set; }

	public int KkSum { get; set; }

	public string KkSumTime => (KkSum % 60).ToString().PadLeft(2, '0') + " : " + (KkSum / 60).ToString().PadLeft(2, '0');

	public int DayKari { get; set; }

	public int Taradodnaghes { get; set; }

	public int Vacationdaily { get; set; }

	public int KolShift { get; set; }

	public static async Task<JameTaradod> GetAsync(string contextGuid, int pid, DateTime d1, DateTime d2, CancellationToken token)
	{
		try
		{
			JameTaradod ret = new JameTaradod
			{
				ContextGuid = contextGuid
			};
			await ret.LoadDataAsync(pid, d1, d2, token);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task LoadDataAsync(int pid, DateTime d1, DateTime d2, CancellationToken token)
	{
		new StringBuilder().AppendLine($"in {DateTime.Now} method loadData");
		try
		{
			Person pperson = await Person.GetByPersonIdAsync(ContextGuid, pid, token);
			TaradodFull taradodFull = await TaradodFull.CreateAsync(ContextGuid, d1, d2, pperson, null, checkValidationVorodAndKHoroj: false, token);
			CountAll = taradodFull.ListTaradodDets.Count;
			ShiftCount = taradodFull.Countshift;
			Vacationdaily = taradodFull.CountMorakhasiWithSalary + taradodFull.CountMorakhasiNotSalary;
			GheybatCount = taradodFull.CountGheybat;
			Taradodnaghes = taradodFull.CountNaghes;
			DayKari = CountAll - taradodFull.CountTatil;
			EkSum = taradodFull.SumEzafekar;
			KkSum = taradodFull.SumKasrkar;
			FullName = taradodFull.FullName;
			PersonId = taradodFull.PersonId;
			ShiftSum = taradodFull.Sumshift;
			SumShiftTime = taradodFull.SumShiftTime;
			GheybatSum = taradodFull.CountGheybat;
			KarkardCount = taradodFull.CountKarkerd;
			KarkardSum = taradodFull.SumKarkerd;
			KolShift = taradodFull.Countshift;
			DateM1 = taradodFull.DateMin;
			DateM2 = taradodFull.DateMax;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\JameTaradod.cs", 88);
		}
	}
}
