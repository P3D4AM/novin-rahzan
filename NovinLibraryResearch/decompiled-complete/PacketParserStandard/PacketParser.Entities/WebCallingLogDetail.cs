using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;

namespace PacketParser.Entities;

public class WebCallingLogDetail : Serializable<WebCallingLogDetail>, ICallingLogDetail, INpDisposable, IDisposable, IDeliverableEntity
{
	public Guid MasterGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public DateTime? PickUpDateM { get; set; }

	public DateTime? HangUpDateM { get; set; }

	public DateTime DateInsert { get; set; }

	public DateTime Modified { get; set; }

	public EnumNotification Status { get; set; }

	public string Uuid { get; set; }

	public string LinkedId { get; set; }

	public string CallFrom_Number { get; set; }

	public string CallFrom_Name { get; set; }

	public string CallTo_Number { get; set; }

	public string CallTo_Name { get; set; }

	public string Description { get; set; }

	public short Rings { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public int Version { get; set; }

	public Guid? TicketGuid { get; set; }
}
