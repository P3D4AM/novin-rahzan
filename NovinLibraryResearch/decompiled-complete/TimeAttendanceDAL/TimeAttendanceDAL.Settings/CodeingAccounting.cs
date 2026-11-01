using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendanceDAL.Settings;

[Serializable]
[DoNotPruneType]
public class CodeingAccounting
{
	public long? MoeinBaseSalary { get; set; }

	public long? TafsilBaseSalary { get; set; }

	public long? MoeinMission { get; set; }

	public long? TafsilMission { get; set; }

	public long? MoeinJarime { get; set; }

	public long? TafsilJarime { get; set; }

	public long? MoeinTatilKari { get; set; }

	public long? TafsilTatilKari { get; set; }

	public bool InsuranceBaseSalary { get; set; } = true;


	public bool TaxBaseSalary { get; set; } = true;


	public bool InsuranceMission { get; set; } = true;


	public bool TaxMission { get; set; } = true;


	public long? MoeinOverTime { get; set; }

	public long? TafsilOverTime { get; set; }

	public bool InsuranceOverTime { get; set; } = true;


	public bool TaxOverTime { get; set; } = true;


	public long? MoeinShiftTime { get; set; }

	public long? TafsilShiftTime { get; set; }

	public bool InsuranceShiftTime { get; set; } = true;


	public bool TaxShiftTime { get; set; } = true;


	public long? MoeinHolidayTime { get; set; }

	public long? TafsilHolidayTime { get; set; }

	public bool InsuranceHolidayTime { get; set; } = true;


	public bool TaxHolidayTime { get; set; } = true;


	public bool InsuranceTatilKari { get; set; } = true;


	public bool TaxTatilKari { get; set; } = true;


	public long? MoeinNightTime { get; set; }

	public long? TafsilNightTime { get; set; }

	public bool InsuranceNightTime { get; set; } = true;


	public bool TaxNightTime { get; set; } = true;


	public long? MoeinVacation { get; set; }

	public long? TafsilVacation { get; set; }

	public bool InsuranceVacation { get; set; } = true;


	public bool TaxVacation { get; set; } = true;


	public long? MoeinLeaveTime { get; set; }

	public long? TafsilLeaveTime { get; set; }

	public bool InsuranceLeaveTime { get; set; } = true;


	public bool TaxLeaveTime { get; set; } = true;


	public bool InsuranceJarime { get; set; } = true;


	public bool TaxJarime { get; set; } = true;


	public long? MoeinAbsentTime { get; set; }

	public long? TafsilAbsentTime { get; set; }

	public bool InsuranceAbsentTime { get; set; } = true;


	public bool TaxAbsentTime { get; set; } = true;


	public long? MoeinEmployer { get; set; }

	public long? TafsilEmployer { get; set; }

	public long? MoeinUnEmployMent { get; set; }

	public long? TafsilUnEmployMent { get; set; }

	public long? TafsilTaminGov { get; set; }

	public long? MoeinTaminGov { get; set; }

	public long? MoeinTaxGov { get; set; }

	public long? TafsilTaxGov { get; set; }

	public long? MoeinRoundFish { get; set; }

	public long? TafsilRoundFish { get; set; }

	public bool IsCalcHoliday { get; set; }

	public int RewardId { get; private set; }

	public int CompensationId { get; private set; }

	public int AdvanceMoneyId { get; private set; }

	public int LoanId { get; private set; }

	public int Additions1 { get; set; }

	public int Additions2 { get; private set; }

	public int Additions3 { get; private set; }

	public int Fraction1 { get; set; }

	public int Fraction2 { get; set; }

	public int Fraction3 { get; set; }

	public int Fraction4 { get; private set; }

	internal async Task InitSalaryIdsAsync(string contextGuid, CancellationToken token)
	{
		List<FishHesab> plst = await FishHesab.SelectAllAsync(contextGuid, getAllField: false, token);
		RewardId = GetNameFromList(plst, "عیدی");
		CompensationId = GetNameFromList(plst, "پاداش");
		AdvanceMoneyId = GetNameFromList(plst, "مساعده");
		LoanId = GetNameFromList(plst, "وام");
		Tuple<string, int> lastMazaya = GetLastMazaya(plst, 0);
		Additions1 = lastMazaya.Item2;
		lastMazaya = GetLastMazaya(plst, 1);
		Additions2 = lastMazaya.Item2;
		lastMazaya = GetLastMazaya(plst, 2);
		Additions3 = lastMazaya.Item2;
		lastMazaya = GetLastKosorat(plst, 0);
		Fraction1 = lastMazaya.Item2;
		lastMazaya = GetLastKosorat(plst, 1);
		Fraction2 = lastMazaya.Item2;
		lastMazaya = GetLastKosorat(plst, 2);
		Fraction3 = lastMazaya.Item2;
		lastMazaya = GetLastKosorat(plst, 3);
		Fraction4 = lastMazaya.Item2;
	}

	private int GetNameFromList(List<FishHesab> plst, string psearch)
	{
		return plst.FirstOrDefault((FishHesab p) => p.Name.Contains(psearch))?.Id ?? 0;
	}

	private Tuple<string, int> GetLastMazaya(List<FishHesab> plst, int idx)
	{
		List<int> lstid = new List<int> { RewardId, CompensationId, AdvanceMoneyId, LoanId };
		List<FishHesab> list = (from c in plst
			where !lstid.Select((int o) => o).Contains(c.Id) && c.Type == 1
			select c into or
			orderby or.Name
			select or).ToList();
		if (list.Count <= idx)
		{
			return new Tuple<string, int>("", 0);
		}
		return new Tuple<string, int>(list[idx].Name, list[idx].Id);
	}

	private Tuple<string, int> GetLastKosorat(List<FishHesab> plst, int idx)
	{
		List<int> lstid = new List<int> { AdvanceMoneyId, LoanId, 6, 5 };
		List<FishHesab> list = (from c in plst
			where !lstid.Select((int o) => o).Contains(c.Id) && c.Type == 2
			select c into or
			orderby or.Name
			select or).ToList();
		if (list.Count <= idx)
		{
			return new Tuple<string, int>("", 0);
		}
		return new Tuple<string, int>(list[idx].Name, list[idx].Id);
	}

	public async Task<string> GetTitleTafsilAsync(int codemoein, int codetafsil)
	{
		string text = "";
		string text2 = codemoein.ToString() + codetafsil;
		if (text2 == MoeinTaxGov.ToString() + TafsilTaxGov)
		{
			text = "[اداره مالیات]";
		}
		else if (text2 == MoeinTaminGov.ToString() + TafsilTaminGov)
		{
			text = "[سازمان تامین اجتماعی]";
		}
		else if (text2 == MoeinLeaveTime.ToString() + TafsilLeaveTime)
		{
			text = "[کسرکار]";
		}
		else if (text2 == MoeinJarime.ToString() + TafsilJarime)
		{
			text = "[جریمه تاخیر]";
		}
		else if (text2 == MoeinAbsentTime.ToString() + TafsilAbsentTime)
		{
			text = "[غیبت]";
		}
		else if (text2 == MoeinUnEmployMent.ToString() + TafsilUnEmployMent)
		{
			text = "[3% حق بیمه بیکاری]";
		}
		else if (text2 == MoeinBaseSalary.ToString() + TafsilBaseSalary)
		{
			text = "[حقوق پایه]";
		}
		else if (text2 == MoeinMission.ToString() + TafsilMission)
		{
			text = "[حق ماموریت]";
		}
		else if (text2 == MoeinEmployer.ToString() + TafsilEmployer)
		{
			text = "[20% بیمه سهم کارفرما]";
		}
		else if (text2 == MoeinVacation.ToString() + TafsilVacation)
		{
			text = "[مرخصی]";
		}
		else if (text2 == MoeinNightTime.ToString() + TafsilNightTime)
		{
			text = "[شب کاری]";
		}
		else if (text2 == MoeinHolidayTime.ToString() + TafsilHolidayTime)
		{
			text = "[جمعه کاری]";
		}
		else if (text2 == MoeinTatilKari.ToString() + TafsilTatilKari)
		{
			text = "[تعطیل کاری]";
		}
		else if (text2 == MoeinShiftTime.ToString() + TafsilShiftTime)
		{
			text = "[NobatKari]";
		}
		else if (text2 == MoeinOverTime.ToString() + TafsilOverTime)
		{
			text = "[اضافه کار]";
		}
		return Utilities.GetTranslate(text);
	}

	public bool IsEmptyAccountingCodeing()
	{
		if (MoeinBaseSalary == 0)
		{
			return true;
		}
		if (TafsilBaseSalary == 0)
		{
			return true;
		}
		if (MoeinMission == 0)
		{
			return true;
		}
		if (TafsilMission == 0)
		{
			return true;
		}
		if (MoeinOverTime == 0)
		{
			return true;
		}
		if (TafsilOverTime == 0)
		{
			return true;
		}
		if (MoeinShiftTime == 0)
		{
			return true;
		}
		if (TafsilShiftTime == 0)
		{
			return true;
		}
		if (MoeinHolidayTime == 0)
		{
			return true;
		}
		if (TafsilHolidayTime == 0)
		{
			return true;
		}
		if (MoeinTatilKari == 0)
		{
			return true;
		}
		if (TafsilTatilKari == 0)
		{
			return true;
		}
		if (MoeinNightTime == 0)
		{
			return true;
		}
		if (TafsilNightTime == 0)
		{
			return true;
		}
		if (MoeinVacation == 0)
		{
			return true;
		}
		if (TafsilVacation == 0)
		{
			return true;
		}
		if (MoeinLeaveTime == 0)
		{
			return true;
		}
		if (TafsilLeaveTime == 0)
		{
			return true;
		}
		if (MoeinJarime == 0)
		{
			return true;
		}
		if (TafsilJarime == 0)
		{
			return true;
		}
		if (MoeinAbsentTime == 0)
		{
			return true;
		}
		if (TafsilAbsentTime == 0)
		{
			return true;
		}
		if (MoeinEmployer == 0)
		{
			return true;
		}
		if (TafsilEmployer == 0)
		{
			return true;
		}
		if (MoeinUnEmployMent == 0)
		{
			return true;
		}
		if (TafsilUnEmployMent == 0)
		{
			return true;
		}
		if (TafsilTaminGov == 0)
		{
			return true;
		}
		if (MoeinTaminGov == 0)
		{
			return true;
		}
		if (MoeinTaxGov == 0)
		{
			return true;
		}
		if (TafsilTaxGov == 0)
		{
			return true;
		}
		return false;
	}
}
