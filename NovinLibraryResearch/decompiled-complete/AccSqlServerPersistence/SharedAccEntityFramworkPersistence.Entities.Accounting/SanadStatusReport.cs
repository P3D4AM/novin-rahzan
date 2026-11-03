using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Factor;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting;

[Table("Acc_SanadStatusReport")]
public class SanadStatusReport : ISanadStatusReport, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int Id { get; set; }

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

	public Guid Guid { get; set; }

	public Document AsType { get; set; }

	[Required]
	[MaxLength(250)]
	public string Name { get; set; }

	public string Description { get; set; }

	public bool Status { get; set; }

	public string? MetaData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public int WarningMinutes { get; set; }

	public int ErrorMinutes { get; set; }

	public long? Id2From { get; set; }

	public long? Id2To { get; set; }

	public int InactivityDays { get; set; }

	public int TargetStatusId { get; set; }

	public ICollection<SanadStatusLog> SanadStatusLogOld { get; set; }

	public ICollection<SanadStatusLog> SanadStatusLogNew { get; set; }

	public ICollection<SanadStatusReportUpdates> SanadStatusReportUpdates { get; set; }
}
