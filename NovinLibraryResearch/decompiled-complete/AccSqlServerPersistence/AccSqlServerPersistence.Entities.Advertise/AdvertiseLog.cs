using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_AdvertiseLog")]
public class AdvertiseLog : IAdvertiseLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	[Required]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	[MaxLength(500)]
	[Required]
	public string Title { get; set; }

	[Required]
	public string Content { get; set; }

	[Required]
	public long SimCardNumber { get; set; }

	[MaxLength(50)]
	[Required]
	public string State { get; set; }

	[MaxLength(50)]
	[Required]
	public string City { get; set; }

	[MaxLength(50)]
	[Required]
	public string Region { get; set; }

	[Required]
	public decimal Price { get; set; }

	[MaxLength(50)]
	[Required]
	public string Category { get; set; }

	[MaxLength(50)]
	[Required]
	public string SubCategory1 { get; set; }

	[MaxLength(50)]
	[Required]
	public string SubCategory2 { get; set; }

	[MaxLength(500)]
	[Required]
	public string URL { get; set; }

	[MaxLength(30)]
	public string? IP { get; set; }

	[MaxLength(1000)]
	[Required]
	public string UpdateDesc { get; set; }

	[Required]
	public int VisitCount { get; set; }

	[MaxLength(50)]
	[Required]
	public string DateSh { get; set; }

	[Required]
	public DateTime DateM { get; set; }

	[MaxLength(200)]
	[Required]
	public string Adv { get; set; }

	[Required]
	public Guid MasterVisitorGuid { get; set; }

	[Required]
	public Guid SlaveVisitorGuid { get; set; }

	[Required]
	public AdvStatusCode StatusCode { get; set; }

	public AdvertiseType AdvType { get; set; }

	public DateTime LastUpdate { get; set; }
}
