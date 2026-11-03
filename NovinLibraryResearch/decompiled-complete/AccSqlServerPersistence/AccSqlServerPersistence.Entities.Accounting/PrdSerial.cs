using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_PrdSerial")]
public class PrdSerial : IPrdSerial, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	[MaxLength(200)]
	public string? Serial { get; set; }

	[MaxLength(200)]
	public string? GarrantyCode { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

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

	public bool Status { get; set; } = true;


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public bool IsTransfer { get; set; }

	public string? MetaData { get; set; }

	[MaxLength(500)]
	public string? TechnicalInfo1 { get; set; }

	[MaxLength(500)]
	public string? TechnicalInfo2 { get; set; }

	[MaxLength(500)]
	public string? TechnicalInfo3 { get; set; }

	public DateTime InsertDate { get; set; }

	public Product ProductItem { get; set; }

	public ICollection<PrdSerialDetails> DetailItem { get; set; }
}
