using System;
using Newtonsoft.Json;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.InterFaces;

public interface IContract
{
	string JsonGharardadDet { get; set; }

	int ID { get; set; }

	string No { get; set; }

	string Peyvast { get; set; }

	DateTime DateM { get; set; }

	int PersonId { get; set; }

	short CompanyId { get; set; }

	short ShiftId { get; set; }

	string Description { get; set; }

	DateTime DateStartM { get; set; }

	DateTime DateFinishM { get; set; }

	[JsonIgnore]
	string DateStartCalture { get; }

	[JsonIgnore]
	string DateFinishCalture { get; }

	int? JobId { get; set; }

	bool Married { get; set; }

	short CountChild { get; set; }

	short GharardadStatus { get; set; }

	short? PartId { get; set; }

	short? RecruitmentSt { get; set; }

	short? RecruitmentType { get; set; }

	short? PlaceId { get; set; }

	short? BankType { get; set; }

	short? BankId { get; set; }

	string BankNo { get; set; }

	string ShabaNo { get; set; }

	decimal SalaryOneHour { get; }

	decimal SalaryOneDay { get; set; }

	decimal PriceOverTime { get; set; }

	decimal PriceOverTime2 { get; set; }

	decimal PriceOverTime3 { get; set; }

	decimal PriceNight { get; set; }

	decimal PriceNobatKari1 { get; set; }

	decimal PriceNobatKari2 { get; set; }

	decimal PriceNobatKari3 { get; set; }

	decimal PriceHoliday { get; set; }

	decimal PriceTatil { get; set; }

	decimal PriceJarime { get; set; }

	decimal PriceLeave { get; set; }

	decimal PriceAbsent { get; set; }

	decimal PriceMission { get; set; }

	decimal PriceLowTime { get; set; }

	decimal PriceLowTime2 { get; set; }

	decimal PriceLowTime3 { get; set; }

	double LeaveDays { get; set; }

	double LeaveSaved { get; set; }

	short LeaveMinutes { get; set; }

	short Sanavat { get; set; }

	short GroupSanavat { get; set; }

	bool IsMotahel { get; set; }

	string MoeenEmployerInsurance { get; set; }

	string TafsilEmployerInsurance { get; set; }

	float ZaribEmployerInsurance { get; set; }

	string MoeenInsuredInsurance { get; set; }

	string TafsilInsuredInsurance { get; set; }

	float ZaribInsuredInsurance { get; set; }

	string MoeenUnemploymentInsurance { get; set; }

	string TafsilUnemploymentInsurance { get; set; }

	bool IsCheckoutVacation { get; set; }

	float ZaribUnemploymentInsurance { get; }

	InsuranceGharadadEnumType InsuranceOption { get; }

	TaxGharadadEnumType TaxOption { get; }

	decimal SumMazaya { get; }

	decimal SumKosorat { get; }

	bool IsTemp { get; }
}
