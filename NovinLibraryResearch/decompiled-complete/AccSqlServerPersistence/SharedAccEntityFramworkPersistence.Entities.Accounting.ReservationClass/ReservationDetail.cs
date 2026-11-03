using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Reservation;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

[Table("Acc_ReservationDetail")]
public class ReservationDetail : IReservationDetail, IHasJustGuid, IHasMasterGuid
{
	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid PlacementGuid { get; set; }

	public Guid? OrderGuid { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public string? Description { get; set; }

	public int PeopleCount { get; set; }

	public Reservation MasterItem { get; set; }

	public Placement PlacementItem { get; set; }

	public Order OrderItem { get; set; }
}
