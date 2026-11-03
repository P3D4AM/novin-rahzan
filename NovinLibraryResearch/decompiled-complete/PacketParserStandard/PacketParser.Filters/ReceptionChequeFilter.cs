using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class ReceptionChequeFilter
{
	public Guid? TafsilGuid { get; set; }

	public ENUMCheck? Status { get; set; }

	public string Number { get; set; } = "";


	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public long PoshtNomre { get; set; }

	public Guid? MasterGuid { get; set; }

	public DateTime? MasterDateFrom { get; set; }

	public DateTime? MasterDateTo { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public bool? IsSayadiRegistered { get; set; }

	public Guid? PlaceGuid { get; set; }

	public bool? IsPersonal { get; set; }
}
