using System;
using System.Collections.Generic;
using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

public class WebAttContract : IContract
{
	private short salaryType;

	private decimal salaryOneMonth;

	private decimal salaryPrice;

	private decimal sumHoghMazaya;

	public WebAttCompany Company { get; set; }

	public List<WebAttContractDetail> DetailList { get; set; }

	[Obsolete("Use List", true)]
	public string JsonGharardadDet { get; set; }

	public int ID { get; set; }

	public string No { get; set; }

	public string Peyvast { get; set; }

	public DateTime DateM { get; set; }

	public int PersonId { get; set; }

	public short CompanyId { get; set; }

	public short ShiftId { get; set; }

	public string Description { get; set; }

	public DateTime DateStartM { get; set; }

	public DateTime DateFinishM { get; set; }

	public string DateStartCalture { get; set; }

	public string DateFinishCalture { get; set; }

	public int? JobId { get; set; }

	public bool Married { get; set; }

	public short CountChild { get; set; }

	public short GharardadStatus { get; set; }

	public short? PartId { get; set; }

	public short? RecruitmentSt { get; set; }

	public short? RecruitmentType { get; set; }

	public short? PlaceId { get; set; }

	public short? BankType { get; set; }

	public short? BankId { get; set; }

	public string BankNo { get; set; }

	public string ShabaNo { get; set; }

	public decimal SalaryOneHour { get; set; }

	public decimal SalaryOneDay { get; set; }

	public decimal PriceOverTime { get; set; }

	public decimal PriceOverTime2 { get; set; }

	public decimal PriceOverTime3 { get; set; }

	public decimal PriceNight { get; set; }

	public decimal PriceNobatKari1 { get; set; }

	public decimal PriceNobatKari2 { get; set; }

	public decimal PriceNobatKari3 { get; set; }

	public decimal PriceHoliday { get; set; }

	public decimal PriceTatil { get; set; }

	public decimal PriceJarime { get; set; }

	public decimal PriceLeave { get; set; }

	public decimal PriceAbsent { get; set; }

	public decimal PriceMission { get; set; }

	public decimal PriceLowTime { get; set; }

	public decimal PriceLowTime2 { get; set; }

	public decimal PriceLowTime3 { get; set; }

	public double LeaveDays { get; set; }

	public double LeaveSaved { get; set; }

	public short LeaveMinutes { get; set; }

	public short Sanavat { get; set; }

	public short GroupSanavat { get; set; }

	public bool IsMotahel { get; set; }

	public string MoeenEmployerInsurance { get; set; }

	public string TafsilEmployerInsurance { get; set; }

	public float ZaribEmployerInsurance { get; set; }

	public string MoeenInsuredInsurance { get; set; }

	public string TafsilInsuredInsurance { get; set; }

	public float ZaribInsuredInsurance { get; set; }

	public string MoeenUnemploymentInsurance { get; set; }

	public string TafsilUnemploymentInsurance { get; set; }

	public bool IsCheckoutVacation { get; set; }

	public float ZaribUnemploymentInsurance { get; set; }

	public InsuranceGharadadEnumType InsuranceOption { get; set; }

	public TaxGharadadEnumType TaxOption { get; set; }

	public decimal SumMazaya { get; set; }

	public decimal SumKosorat { get; set; }

	public bool IsTemp { get; }

	public short GetSalaryType()
	{
		return salaryType;
	}

	public void SetSalaryType(short value)
	{
		salaryType = value;
	}

	public decimal GetSalaryOneMonth()
	{
		return salaryOneMonth;
	}

	public void SetSalaryOneMonth(decimal value)
	{
		salaryOneMonth = value;
	}

	public decimal GetSalaryPrice()
	{
		return salaryPrice;
	}

	public void SetSalaryPrice(decimal value)
	{
		salaryPrice = value;
	}

	public decimal GetSumHoghMazaya()
	{
		return sumHoghMazaya;
	}

	public void SetSumHoghMazaya(decimal value)
	{
		sumHoghMazaya = value;
	}
}
