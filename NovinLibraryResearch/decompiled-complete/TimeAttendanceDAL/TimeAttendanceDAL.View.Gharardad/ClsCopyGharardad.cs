using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.View.Shift;

namespace TimeAttendanceDAL.View.Gharardad;

public class ClsCopyGharardad : IHasContext
{
	private ShiftViewModel _shiftViewModel;

	public string ContextGuid { get; set; }

	public int No { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => Calendar.MiladiToCalture(DateM);

	public short ShiftId { get; set; }

	public DateTime DateStartM { get; set; }

	public DateTime DateFinishM { get; set; }

	public CopyGharardadEnum CopyGharardadType { get; set; }

	public decimal IncreasePercentage { get; set; }

	public int BaseYear { get; set; }

	public bool IsSalary { get; set; }

	public bool IsOverTime { get; set; }

	public bool IsShift { get; set; }

	public bool IsHoliday { get; set; }

	public bool IsNight { get; set; }

	public bool IsVacation { get; set; }

	public bool IsLowTime { get; set; }

	public bool IsAbsent { get; set; }

	public decimal PriceSalary { get; set; }

	public decimal PriceOverTime { get; set; }

	public decimal PriceOverTime2 { get; set; }

	public decimal PriceOverTime3 { get; set; }

	public decimal PriceNobatKari1 { get; set; }

	public decimal PriceNobatKari2 { get; set; }

	public decimal PriceNobatKari3 { get; set; }

	public decimal PriceHoliday { get; set; }

	public decimal PriceTatil { get; set; }

	public decimal PriceNight { get; set; }

	public decimal PriceVacation { get; set; }

	public decimal PriceLowTime { get; set; }

	public decimal PriceLowTime2 { get; set; }

	public decimal PriceLowTime3 { get; set; }

	public decimal PriceAbsent { get; set; }

	public decimal PriceMission { get; set; }

	public List<FishHesab> ListMazaya { get; set; }

	public List<FishHesab> ListKosorat { get; set; }

	public List<GharardadNew> LstGharardads { get; set; }

	public List<FishHesab> ListHesab { get; set; }

	public async Task<ShiftViewModel> GetShiftViewModelAsync(CancellationToken token)
	{
		try
		{
			ShiftViewModel shiftViewModel = _shiftViewModel;
			if (shiftViewModel == null)
			{
				shiftViewModel = (_shiftViewModel = await ShiftViewModel.GetAsync(ContextGuid, ShiftId, token));
			}
			return shiftViewModel;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<ClsCopyGharardad> GetAsync(string contextGuid, List<GharardadNew> lstGharardadDetNew, CancellationToken token)
	{
		try
		{
			ClsCopyGharardad ret = new ClsCopyGharardad
			{
				ContextGuid = contextGuid,
				LstGharardads = lstGharardadDetNew,
				ListMazaya = new List<FishHesab>(),
				ListKosorat = new List<FishHesab>(),
				ShiftId = 0
			};
			ClsCopyGharardad clsCopyGharardad = ret;
			clsCopyGharardad.No = await GharardadNew.NextGharardadAsync(ret.ContextGuid, token);
			string text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now).Substring(0, 5) + "01/01";
			ret.DateStartM = LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(text);
			text = text.Substring(0, 5) + "12/29";
			ret.DateFinishM = LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(text);
			ret.DateM = DateTime.Now;
			ret.CopyGharardadType = (CopyGharardadEnum)1;
			ClsCopyGharardad clsCopyGharardad2 = ret;
			ClsCopyGharardad clsCopyGharardad3 = ret;
			ClsCopyGharardad clsCopyGharardad4 = ret;
			ClsCopyGharardad clsCopyGharardad5 = ret;
			ClsCopyGharardad clsCopyGharardad6 = ret;
			ClsCopyGharardad clsCopyGharardad7 = ret;
			ClsCopyGharardad clsCopyGharardad8 = ret;
			bool flag2 = (ret.IsAbsent = true);
			bool flag4 = (clsCopyGharardad8.IsLowTime = flag2);
			bool flag6 = (clsCopyGharardad7.IsVacation = flag4);
			bool flag8 = (clsCopyGharardad6.IsNight = flag6);
			bool flag10 = (clsCopyGharardad5.IsHoliday = flag8);
			bool flag12 = (clsCopyGharardad4.IsShift = flag10);
			bool isSalary = (clsCopyGharardad3.IsOverTime = flag12);
			clsCopyGharardad2.IsSalary = isSalary;
			ret.InitMinValuesFromWebService();
			Task.Run(async delegate
			{
				ClsCopyGharardad clsCopyGharardad9 = ret;
				clsCopyGharardad9.ListHesab = await FishHesab.SelectAllAsync(ret.ContextGuid, getAllField: true, token);
			});
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private void InitMinValuesFromWebService()
	{
		PriceSalary = 100m;
		PriceOverTime = 100m;
		PriceOverTime2 = 100m;
		PriceOverTime3 = 100m;
		PriceNobatKari1 = 100m;
		PriceNobatKari2 = 100m;
		PriceNobatKari3 = 100m;
		PriceHoliday = 100m;
		PriceTatil = 100m;
		PriceNight = 100m;
		PriceVacation = 100m;
		PriceLowTime = 100m;
		PriceLowTime2 = 100m;
		PriceLowTime3 = 100m;
		PriceAbsent = 100m;
		PriceMission = 100m;
	}

	public async Task CreateNewGharardadAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			foreach (GharardadNew lstGharardad in LstGharardads)
			{
				if (lstGharardad.DateFinishM >= DateStartM)
				{
					lstGharardad.DateFinishM = DateStartM.AddDays(-1.0);
					await lstGharardad.SaveAsync(token);
				}
			}
			int gNo = No;
			foreach (GharardadNew lstGharardad2 in LstGharardads)
			{
				lstGharardad2.ID = 0;
				lstGharardad2.No = gNo.ToString();
				lstGharardad2.DateStartM = DateStartM;
				lstGharardad2.DateFinishM = DateFinishM;
				lstGharardad2.DateM = DateM;
				lstGharardad2.LeaveSaved = 0.0;
				lstGharardad2.SetSalaryPrice(PriceSalary);
				lstGharardad2.PriceOverTime = PriceOverTime;
				lstGharardad2.PriceOverTime2 = PriceOverTime2;
				lstGharardad2.PriceOverTime3 = PriceOverTime3;
				lstGharardad2.PriceNobatKari1 = PriceNobatKari1;
				lstGharardad2.PriceNobatKari2 = PriceNobatKari2;
				lstGharardad2.PriceNobatKari3 = PriceNobatKari3;
				lstGharardad2.PriceHoliday = PriceHoliday;
				lstGharardad2.PriceTatil = PriceTatil;
				lstGharardad2.PriceNight = PriceNight;
				lstGharardad2.PriceLeave = PriceVacation;
				lstGharardad2.PriceLowTime = PriceLowTime;
				lstGharardad2.PriceLowTime2 = PriceLowTime2;
				lstGharardad2.PriceLowTime3 = PriceLowTime3;
				lstGharardad2.PriceAbsent = PriceAbsent;
				lstGharardad2.PriceMission = PriceMission;
				foreach (GharardadDetNew gharardadDet in lstGharardad2.ListGharardadDet)
				{
					FishHesab fishHesab = ListHesab.FirstOrDefault((FishHesab q) => q.Id.Equals(gharardadDet.HesabId));
					if (fishHesab != null)
					{
						gharardadDet.nValue = fishHesab.Account;
					}
				}
				await lstGharardad2.SaveAsync(token);
				gNo++;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
