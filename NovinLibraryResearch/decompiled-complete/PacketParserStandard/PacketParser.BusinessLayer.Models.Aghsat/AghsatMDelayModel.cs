using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.BusinessLayer.Models.Aghsat;

public class AghsatMDelayModel : IHasIsChecked, IHasJustGuid
{
	public Guid Guid { get; set; }

	public decimal VajhEltezam { get; set; }

	public DateTime DateSarresidM { get; set; }

	public string DateSarresidSh => DateSarresidM.MiladiToCalture();

	public DateTime EltezamStartDateM { get; set; }

	public string EltezamStartDateSh => EltezamStartDateM.MiladiToCalture();

	public int DelayDayCount { get; set; }

	public Guid TafsilGuid { get; set; }

	public long TafsilCode { get; set; }

	public string TafsilName { get; set; }

	public Guid MoeinGuid { get; set; }

	public int Id2 { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }

	public int? SanadNo { get; set; }

	public DateTime? VajhEltezamModified { get; set; }

	public decimal PenaltyPrice { get; set; }

	public bool IsChecked { get; set; }
}
