using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Kol")]
public class Kol : IKol, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[NotMapped]
	private DateTime _DateM;

	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Name { get; set; }

	public Guid GroupGuid { get; set; }

	public ENMahiat Mahiat { get; set; }

	public int Code { get; set; }

	public bool Status { get; set; }

	[Required]
	public string Description { get; set; }

	public bool System { get; set; }

	public decimal Account { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public DateTime DateM
	{
		get
		{
			return _DateM;
		}
		set
		{
			DateTime dateM = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_DateM = dateM;
		}
	}

	public byte[]? GardeshHesab { get; set; }

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

	public HesabGroup HesabGroup { get; set; }

	public ICollection<Moein> Moeins { get; set; }

	public ICollection<GardeshTafsil> GardeshTafsils { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails { get; set; }
}
