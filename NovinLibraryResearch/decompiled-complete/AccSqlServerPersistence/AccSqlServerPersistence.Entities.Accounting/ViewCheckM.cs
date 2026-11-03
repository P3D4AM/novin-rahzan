using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_ViewCheckM")]
public class ViewCheckM : IViewCheckM, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string BankName { get; set; }

	public decimal Price { get; set; }

	public ENUMCheck Status { get; set; }

	public DateTime DateSanadM { get; set; }

	public DateTime DateCheckM { get; set; }

	public DateTime DateBargashtORVosolM { get; set; }

	[Required]
	public string DateToPlace { get; set; }

	[Required]
	public string Place { get; set; }

	[Required]
	public string Description { get; set; }

	[Required]
	public string Number { get; set; }

	public Guid GuidSanad { get; set; }

	public Guid PardakhtGuid { get; set; }

	public bool Personal { get; set; }

	[Required]
	public string HesabNum { get; set; }

	public bool Aval2re { get; set; }

	public long PoshtNomreh { get; set; }

	public Guid VisitorGuid { get; set; }

	public DateTime Modified { get; set; }

	public Guid PardazandeTafsilGuid { get; set; }

	bool IHasStatus.Status { get; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public bool IsEnteghali { get; set; }

	[MaxLength(100)]
	public string Sayadi_Name { get; internal set; }

	[MaxLength(100)]
	public string Sayadi_NationalCode { get; internal set; }

	[MaxLength(100)]
	public string Sayadi_Identity { get; internal set; }

	public bool SayadiRegistered { get; set; }

	public string? MetaData { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }
}
