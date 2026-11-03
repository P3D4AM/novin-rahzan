using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Reservation;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

[Table("Acc_Reservation")]
public class Reservation : IReservation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid? OrderGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public string? Description { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	public bool Status { get; set; }

	public int Number { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Order OrderItem { get; set; }

	public Cashier CashierItem { get; set; }

	public Reception ReceptionItem { get; set; }

	public ICollection<ReservationDetail> Details { get; set; }
}
