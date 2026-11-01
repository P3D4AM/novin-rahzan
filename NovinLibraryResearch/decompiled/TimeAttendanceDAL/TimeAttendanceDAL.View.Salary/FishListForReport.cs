using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser;
using PacketParser.Attendance.Entities;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendanceDAL.View.Salary;

[Serializable]
[DoNotPruneType]
public class FishListForReport
{
	public int FishNo { get; set; }

	public int IdSanad { get; set; }

	public string DateCalture { get; set; }

	public DateTime DateStartM { get; set; }

	public DateTime DateEndM { get; set; }

	public long CodeTafsil { get; set; }

	public string FullName { get; set; }

	public decimal SumKarkerd { get; set; }

	public decimal SumMazaya { get; set; }

	public decimal SumKosorat { get; set; }

	public decimal SumKhales { get; set; }

	public static List<FishListForReport> FillList(List<Fish> pLstFish, List<int> plstId)
	{
		try
		{
			if (plstId != null && plstId.Count > 0)
			{
				pLstFish = pLstFish.Where((Fish pr) => plstId.Any((int p) => p.Equals(pr.Id))).ToList();
			}
			List<FishListForReport> list = new List<FishListForReport>();
			foreach (Fish item in pLstFish)
			{
				FishListForReport fishListForReport = new FishListForReport();
				fishListForReport.FishNo = item.FishNo;
				if (item.IdSanad.HasValue)
				{
					fishListForReport.IdSanad = item.IdSanad.Value;
				}
				fishListForReport.DateCalture = Calendar.MiladiToCalture(item.DateM);
				fishListForReport.DateStartM = item.DateStartM;
				fishListForReport.DateEndM = item.DateEndM;
				fishListForReport.CodeTafsil = ((PersonBase)item.ClsPerson).CodeTafsil;
				fishListForReport.FullName = ((PersonBase)item.ClsPerson).FullName;
				fishListForReport.SumKarkerd = item.SumKarkerd;
				fishListForReport.SumMazaya = item.SumMazaya;
				fishListForReport.SumKosorat = item.SumKosorat;
				fishListForReport.SumKhales = item.SumKhales;
				list.Add(fishListForReport);
			}
			return list;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
