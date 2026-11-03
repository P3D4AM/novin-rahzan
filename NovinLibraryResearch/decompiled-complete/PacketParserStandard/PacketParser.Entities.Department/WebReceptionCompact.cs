using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities.Department;

public class WebReceptionCompact : Serializable<WebReceptionCompact>, INpDisposable, IDisposable
{
	public Guid Guid;

	public Guid TafsilGuid { get; set; }

	public int id { get; set; }

	public int SanadNo { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime Date { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public DateTime DateSabt { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string CustomerFullName { get; set; }

	public string CashierFullName { get; set; }

	public ENSource Source { get; set; }

	public string Description { get; set; }

	public int Id2 { get; set; }

	public bool? DeliveryStatus { get; set; }

	public decimal Sum { get; set; }
}
