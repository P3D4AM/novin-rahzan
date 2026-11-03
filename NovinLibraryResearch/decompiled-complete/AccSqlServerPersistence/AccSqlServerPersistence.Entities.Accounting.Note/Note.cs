using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Note;

namespace AccSqlServerPersistence.Entities.Accounting.Note;

[Table("Acc_Note")]
public class Note : INote, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status { get; }

	[Required]
	public string Title { get; set; }

	[Required]
	public string Text { get; set; }

	public DateTime DateM { get; set; }

	public DateTime? DateSarResidM { get; set; }

	public NotePirority Pirority { get; set; }

	public NoteStatus Status { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid? EntityGuid { get; set; }

	public Document? EntityType { get; set; }

	public string? PirorityName { get; set; }

	public string? StatusName { get; set; }

	public NoteDisplayState DisplayState { get; set; }

	public string? MetaData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Cashier Cashier { get; set; }

	public NoteGroup Group { get; set; }

	public Tafsil Tafsil { get; set; }
}
