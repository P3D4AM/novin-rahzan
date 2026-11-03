using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_ViewCheckSh")]
public class ViewCheckSh : IViewCheckSh, IHasJustGuid, IHasStatus, IDeliverableEntity, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Number { get; set; }

	public decimal Price { get; set; }

	public ENCheckSHStatus ChequeStatus { get; set; }

	[Required]
	public string BankName { get; set; }

	[Required]
	public string CustomerFullName { get; set; }

	public string? PardakhtDate { get; set; }

	[Obsolete("", true)]
	public int IDSanad { get; set; }

	public Guid GuidSanad { get; set; }

	public bool Aval2re { get; set; }

	public DateTime DateSarresidM { get; set; }

	[Required]
	public string PardakhtDesc { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	[NotMapped]
	bool IHasStatus.Status => true;

	public bool IsEnteghali { get; set; }

	public bool SayadiRegistered { get; set; }

	public string? MetaData { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }
}
