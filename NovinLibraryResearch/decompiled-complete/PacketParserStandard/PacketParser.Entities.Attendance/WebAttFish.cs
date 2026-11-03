using System;
using System.Collections.Generic;

namespace PacketParser.Entities.Attendance;

public class WebAttFish
{
	public List<WebAttFishDetail> ListFishDetails { get; set; }

	public int Id { get; set; }

	public int PersonId { get; set; }

	public int FishNo { get; set; }

	public DateTime DateStartM { get; set; }

	public string DateStartSh { get; set; }

	public DateTime DateEndM { get; set; }

	public string DateEndSh { get; set; }

	public string Description { get; set; }

	public short St { get; set; }

	public short VacationST { get; set; }

	public int KarkerdMin { get; set; }

	public decimal KarkerdPrice { get; set; }

	public int EzafeMin { get; set; }

	public decimal EzafePrice { get; set; }

	public int EzafeMin1 { get; set; }

	public decimal EzafePrice1 { get; set; }

	public int EzafeMin2 { get; set; }

	public decimal EzafePrice2 { get; set; }

	public int EzafeMin3 { get; set; }

	public decimal EzafePrice3 { get; set; }

	public int ShabMin { get; set; }

	public decimal ShabPrice { get; set; }

	public int NobatMin1 { get; set; }

	public int NobatMin2 { get; set; }

	public int NobatMin3 { get; set; }

	public decimal NobatPrice1 { get; set; }

	public decimal NobatPrice2 { get; set; }

	public decimal NobatPrice3 { get; set; }

	public int JomeMin { get; set; }

	public int TatilMin { get; set; }

	public int Mozafimin { get; set; }

	public int JarimeRoz { get; set; }

	public decimal JomePrice { get; set; }

	public decimal TatilPrice { get; set; }

	public decimal JarimePrice { get; set; }

	public int VacationMin { get; set; }

	public decimal VacationPrice { get; set; }

	public double VacationRemind { get; set; }

	public int VacationMinUsed { get; set; }

	public int GheybatRoze { get; set; }

	public int GheybatMin { get; set; }

	public decimal GheybatPrice { get; set; }

	public double MissionRoz { get; set; }

	public decimal MissionPrice { get; set; }

	public int KasrMin { get; set; }

	public decimal KasrPrice { get; set; }

	public int KasrMin1 { get; set; }

	public decimal KasrPrice1 { get; set; }

	public int KasrMin2 { get; set; }

	public decimal KasrPrice2 { get; set; }

	public int KasrMin3 { get; set; }

	public decimal KasrPrice3 { get; set; }

	public short IsPrint { get; set; }

	public decimal SumKarkerd { get; set; }

	public decimal SumEzafehKar1 { get; set; }

	public decimal SumEzafehKar2 { get; set; }

	public decimal SumEzafehKar3 { get; set; }

	public decimal SumShabKar { get; set; }

	public decimal SumVacation { get; set; }

	public decimal SumVacationRemind { get; set; }

	public decimal SumNobatKar { get; set; }

	public decimal AVGNobatKary { get; set; }

	public decimal SumJomeKary { get; set; }

	public decimal SumKasrKar1 { get; set; }

	public decimal SumKasrKar2 { get; set; }

	public decimal SumKasrKar3 { get; set; }

	public decimal SumGheybat { get; set; }

	public decimal SumMission { get; set; }

	public decimal SumTotalKarkard { get; set; }

	public decimal SumMazaya { get; set; }

	public decimal SumKosorat { get; set; }

	public decimal SumKhales { get; set; }

	public short CountKarKerd { get; set; }

	public short CountKarKerdBime { get; set; }

	public short CountKarKerdMaliat { get; set; }

	public short CountDaysOfMonth { get; set; }

	public short CountShift { get; set; }

	public short CountVacationWithHoghogh { get; set; }

	public short CountVacationNotHoghogh { get; set; }

	public short CountMorakhasi { get; set; }

	public short CountGheybat { get; set; }

	public short CountNaghes { get; set; }

	public short CountTatil { get; set; }

	[Obsolete("Use List", true)]
	public string JsonFishDet { get; set; }

	public DateTime SodourDate { get; set; }
}
