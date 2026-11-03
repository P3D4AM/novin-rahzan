using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_SimCard")]
public class SimCard : ISimCard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public DateTime SheypoorModified { get; set; }

	public DateTime NextUseDivar { get; set; }

	public DateTime NextUseSheypoor { get; set; }

	public bool isSheypoorBlocked { get; set; }

	public bool isSendDivarAdv { get; set; }

	public bool isSendSheypoorAdv { get; set; }

	[Required]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public long Number { get; set; }

	[Required]
	public Guid OwnerGuid { get; set; }

	public int UseCount { get; set; }

	[DefaultValue("true")]
	public bool Status { get; set; }

	[Key]
	public Guid Guid { get; set; }

	[DefaultValue("getdate()")]
	public DateTime Modified { get; set; }

	public DateTime DivarModified { get; set; }

	public string? Operator { get; set; }

	public string? UserName { get; set; }
}
