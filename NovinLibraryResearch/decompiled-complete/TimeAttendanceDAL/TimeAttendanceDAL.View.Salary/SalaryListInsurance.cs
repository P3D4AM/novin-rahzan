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
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Enums;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.View.Salary;

[Serializable]
[DoNotPruneType]
public class SalaryListInsurance : IHasContext
{
	public string ContextGuid { get; set; }

	public string FullName { get; set; }

	public string ShabaNo { get; set; }

	public string FatherName { get; set; }

	public string ShSh { get; set; }

	public string CodeMeli { get; set; }

	public string JobTitle { get; set; }

	public string SalaryTypeTitle { get; set; }

	public short SalaryType { get; set; }

	public int FishNo { get; set; }

	public string DateStartSh { get; set; }

	public long CodeMoein { get; set; }

	public int KarkerdMin { get; set; }

	public int Mozafimin { get; set; }

	public decimal KarkerdPrice { get; set; }

	public decimal KarkerdSumPrice { get; set; }

	public decimal SumKarkerd { get; set; }

	public decimal SumKolKarkerd { get; set; }

	public decimal SumKhales { get; set; }

	public short CountKarKerd { get; set; }

	public short CountKarKerdBime { get; set; }

	public short CountKarKerdMaliat { get; set; }

	public short CountNaghes { get; set; }

	public short CountTatil { get; set; }

	public short CountDays { get; set; }

	public short CountShift { get; set; }

	public decimal IncludedInsurance { get; set; }

	public decimal IncludedTax { get; set; }

	public int EzafeMin { get; set; }

	public decimal EzafePrice { get; set; }

	public decimal SumEzafeKar { get; set; }

	public int KasrMin { get; set; }

	public decimal SumKasrKar { get; set; }

	public decimal KasrPrice { get; set; }

	public decimal VacationPrice { get; set; }

	public int VacationMin { get; set; }

	public int VacationMinUsed { get; set; }

	public int MorakhasiEstelajiMin { get; set; }

	public int MorakhasiEstehghaghiMin { get; set; }

	public int MorakhasiNotSalaryMin { get; set; }

	public int GheybatMin { get; set; }

	public string RayVarzTaradodKardex { get; set; }

	public decimal SumVacation { get; set; }

	public short CountVacationWithHoghogh { get; set; }

	public short CountVacationNotHoghogh { get; set; }

	public int GheybatRoze { get; set; }

	public decimal GheybatPrice { get; set; }

	public decimal SumGheybat { get; set; }

	public short CountGheybat { get; set; }

	public double MissionRoz { get; set; }

	public decimal MissionPrice { get; set; }

	public decimal SumMission { get; set; }

	public double JarimeRoz { get; set; }

	public decimal JarimePrice { get; set; }

	public decimal SumJarime { get; set; }

	public int ShabMin { get; set; }

	public decimal ShabPrice { get; set; }

	public decimal SumShabKari { get; set; }

	public int NobatMin1 { get; set; }

	public int NobatMin2 { get; set; }

	public int NobatMin3 { get; set; }

	public decimal NobatPrice1 { get; set; }

	public decimal NobatPrice2 { get; set; }

	public decimal NobatPrice3 { get; set; }

	public decimal SumNobatKari1 { get; set; }

	public decimal SumNobatKari2 { get; set; }

	public decimal SumNobatKari3 { get; set; }

	public int JomeMin { get; set; }

	public decimal JomePrice { get; set; }

	public decimal SumJomeKari { get; set; }

	public decimal SumMazaya { get; set; }

	public decimal SumKosorat { get; set; }

	public decimal Grocery { get; set; }

	public decimal Maskan { get; set; }

	public decimal Taahol { get; set; }

	public decimal Spouse { get; set; }

	public decimal Reward { get; set; }

	public decimal SeverancePay { get; set; }

	public decimal Compensation { get; set; }

	public decimal Additions1 { get; set; }

	public decimal Additions2 { get; set; }

	public decimal Additions3 { get; set; }

	public decimal Tax { get; set; }

	public decimal Insurance { get; set; }

	public decimal AdvanceMoney { get; set; }

	public decimal Loan { get; set; }

	public decimal Fraction1 { get; set; }

	public decimal Fraction2 { get; set; }

	public decimal Fraction3 { get; set; }

	public decimal Fraction4 { get; set; }

	public decimal FractionOther { get; set; }

	public decimal NetPayable { get; set; }

	public short IsPrint { get; set; }

	public string JsonFishDet { get; private set; }

	public async Task<SalaryListInsurance> GetAsync(string contextGuid, Fish dr, CancellationToken token)
	{
		try
		{
			SalaryListInsurance salary = new SalaryListInsurance
			{
				ContextGuid = contextGuid
			};
			await salary.LoadDataAsync(dr, token);
			return salary;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task LoadDataAsync(Fish dr, CancellationToken token)
	{
		_ = 3;
		try
		{
			FishNo = dr.FishNo;
			DateStartSh = LanguageAndCalendar.Instance.CaltureCalender.GetMonthName(dr.DateStartM) + " سال " + Calendar.MiladiToCalture(dr.DateStartM).Substring(0, 4);
			SalaryType = dr.St;
			CodeMoein = ((PersonBase)dr.ClsPerson).CodeMoein;
			FullName = ((PersonBase)dr.ClsPerson).FullName;
			ShabaNo = dr.ClsGharardad?.ShabaNo;
			FatherName = ((PersonBase)dr.ClsPerson).FatherName;
			ShSh = ((PersonBase)dr.ClsPerson).Shsh;
			CodeMeli = ((PersonBase)dr.ClsPerson).CodeMeli;
			JobTitle = await dr.GetJobTitleAsync(token);
			KarkerdMin = dr.KarkerdMin;
			Mozafimin = dr.MoazafiMin;
			KarkerdPrice = dr.KarkerdPrice;
			SumKarkerd = dr.SumKarkerd;
			SumKolKarkerd = dr.GetSumKolKarkerd();
			CountKarKerd = dr.CountKarKerd;
			switch ((EnumsHoghogh.EnumkarkardSt)dr.St)
			{
			case EnumsHoghogh.EnumkarkardSt.Monthly:
				KarkerdSumPrice = dr.KarkerdPrice;
				SalaryTypeTitle = "ماهانه";
				break;
			case EnumsHoghogh.EnumkarkardSt.Daily:
				KarkerdSumPrice = KarkerdPrice * (decimal)CountKarKerd;
				SalaryTypeTitle = "روزانه";
				break;
			case EnumsHoghogh.EnumkarkardSt.Hourly:
				KarkerdSumPrice = (decimal)Mozafimin * KarkerdPrice / 60m;
				SalaryTypeTitle = "ساعتی";
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			SumKhales = dr.SumKhales;
			NetPayable = dr.SumKhales;
			CountKarKerdBime = dr.CountWorkdayBime;
			CountKarKerdMaliat = dr.CountWorkdayMaliyat;
			CountNaghes = dr.CountNaghes;
			CountTatil = dr.CountTatil;
			CountDays = dr.CountDaysOfMonth;
			CountShift = dr.Countshift;
			IncludedInsurance = await dr.GetIncludedInsuranceAsync(token);
			IncludedTax = await dr.GetIncludedTaxAsync(token);
			EzafeMin = dr.EzafeMin;
			EzafePrice = dr.EzafePrice;
			SumEzafeKar = dr.SumEzafeKar;
			KasrMin = dr.KasrMin;
			SumKasrKar = dr.SumKasrKar;
			KasrPrice = dr.KasrPrice;
			VacationPrice = dr.VacationPrice;
			VacationMin = dr.VacationMin;
			VacationMinUsed = dr.VacationMinUsed;
			MorakhasiEstehghaghiMin = dr.MorakhasiEstehghaghiMin;
			MorakhasiEstelajiMin = dr.MorakhasiEstelajiMin;
			MorakhasiNotSalaryMin = dr.MorakhasiNotSalaryMin;
			GheybatMin = dr.GheybatMin;
			RayVarzTaradodKardex = dr.GetRayVarzTaradodKardex();
			SumVacation = dr.SumVacation;
			CountVacationWithHoghogh = dr.CountMorakhasiWithSalary;
			CountVacationNotHoghogh = dr.CountMorakhasiNotSalary;
			GheybatRoze = dr.GheybatRoze;
			GheybatPrice = dr.GheybatPrice;
			SumGheybat = dr.SumGheybat;
			CountGheybat = dr.CountGheybat;
			MissionRoz = dr.MissionRoz;
			MissionPrice = dr.MissionPrice;
			SumMission = dr.SumMission;
			JarimeRoz = dr.JarimeRoz;
			JarimePrice = dr.JarimePrice;
			SumJarime = dr.SumJarime;
			ShabMin = dr.ShabMin;
			ShabPrice = dr.ShabPrice;
			SumShabKari = dr.SumShabKari;
			NobatMin1 = dr.NobatMin1;
			NobatPrice1 = dr.NobatPrice1;
			SumNobatKari1 = dr.SumNobatKari1;
			NobatMin2 = dr.NobatMin2;
			NobatPrice2 = dr.NobatPrice2;
			SumNobatKari2 = dr.SumNobatKari2;
			NobatMin3 = dr.NobatMin3;
			NobatPrice3 = dr.NobatPrice3;
			SumNobatKari3 = dr.SumNobatKari3;
			JomeMin = dr.JomeMin;
			JomePrice = dr.JomePrice;
			SumJomeKari = dr.SumJomeKari;
			SumMazaya = dr.SumMazaya;
			SumKosorat = dr.SumKosorat;
			Grocery = GetPriceFromFishDet(dr.ListFishDet, 3);
			Maskan = GetPriceFromFishDet(dr.ListFishDet, 1);
			Taahol = GetPriceFromFishDet(dr.ListFishDet, 7);
			Spouse = GetPriceFromFishDet(dr.ListFishDet, 2);
			SeverancePay = GetPriceFromFishDet(dr.ListFishDet, 4);
			Tax = GetPriceFromFishDet(dr.ListFishDet, 6);
			Insurance = GetPriceFromFishDet(dr.ListFishDet, 5);
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			Reward = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.RewardId);
			Compensation = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.CompensationId);
			AdvanceMoney = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.AdvanceMoneyId);
			Loan = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.LoanId);
			Additions1 = (decimal)dr.ShabMin * dr.ShabPrice / 60m + (decimal)dr.NobatMin1 * dr.NobatPrice1 / 60m + (decimal)dr.NobatMin2 * dr.NobatPrice2 / 60m + (decimal)dr.NobatMin3 * dr.NobatPrice3 / 60m + (decimal)dr.JomeMin * dr.JomePrice / 60m + (decimal)(dr.MissionRoz * (double)dr.MissionPrice);
			Additions2 = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.Additions1);
			Additions3 = SumMazaya - (Grocery + Maskan + Spouse + SeverancePay + Reward + Compensation + Additions2);
			Fraction1 = (decimal)dr.KasrMin * dr.KasrPrice / 60m + (decimal)dr.GheybatRoze * dr.GheybatPrice;
			Fraction2 = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.Fraction1);
			Fraction3 = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.Fraction2);
			Fraction4 = GetPriceFromFishDet(dr.ListFishDet, timeAttendanceSettings.CodeingAccounting.Fraction3);
			FractionOther = SumKosorat - (Tax + Insurance + AdvanceMoney + Loan + Fraction1 + Fraction2 + Fraction3 + Fraction4);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\SalaryListInsurance.cs", 299);
		}
	}

	private decimal GetPriceFromFishDet(List<FishDetJson> listFishDet, int index)
	{
		return listFishDet.FirstOrDefault((FishDetJson qu) => qu.HesabId.Equals(index))?.TmpValue ?? 0m;
	}

	private Tuple<string, decimal> GetToupleFromFishDet(List<FishDetJson> listFishDet, int index)
	{
		FishDetJson fishDetJson = listFishDet.FirstOrDefault((FishDetJson qu) => qu.HesabId.Equals(index));
		if (fishDetJson == null)
		{
			return new Tuple<string, decimal>("", 0m);
		}
		return new Tuple<string, decimal>(fishDetJson.Name, fishDetJson.TmpValue);
	}

	public static async Task<List<SalaryListInsurance>> GetAllAsync(string contextGuid, List<Fish> lstFishes, CancellationToken token)
	{
		List<SalaryListInsurance> ret = new List<SalaryListInsurance>();
		try
		{
			foreach (Fish lstFish in lstFishes)
			{
				List<SalaryListInsurance> list = ret;
				list.Add(await new SalaryListInsurance().GetAsync(contextGuid, lstFish, token));
			}
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
