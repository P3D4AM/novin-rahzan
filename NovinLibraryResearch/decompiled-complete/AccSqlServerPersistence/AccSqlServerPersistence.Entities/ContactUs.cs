using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_ContactUs")]
public class ContactUs : Serializable<ContactUs>, IContactUs, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public int Id { get; set; }

	[Key]
	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[Required]
	public string Address { get; set; }

	[Required]
	public string CodeMeli { get; set; }

	public DateTime? DateBirth { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	[Required]
	public string Description { get; set; }

	[Required]
	public string Email { get; set; }

	[Required]
	public string Family { get; set; }

	[Required]
	public string Fax { get; set; }

	public bool Jensiat { get; set; }

	[Required]
	public string Message { get; set; }

	[Required]
	public string Mob { get; set; }

	[Required]
	public string Mob2 { get; set; }

	[Required]
	public string NahveAshnaee { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string Ostan { get; set; }

	[Required]
	public string SabegheKari { get; set; }

	[Required]
	public string Senf { get; set; }

	[Required]
	public string Shahrestan { get; set; }

	[Required]
	public string Shoghl { get; set; }

	[Required]
	public string Subject { get; set; }

	[Required]
	public string Tahsilat { get; set; }

	[Required]
	public string Tel { get; set; }

	[Required]
	public string Tel2 { get; set; }

	public TypeContact Type { get; set; }

	[Required]
	public string WebSite { get; set; }

	[Required]
	public string Zip { get; set; }

	[Required]
	public string Ip { get; set; }

	public int ReferencedId { get; set; }

	public string? SerializedData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }
}
