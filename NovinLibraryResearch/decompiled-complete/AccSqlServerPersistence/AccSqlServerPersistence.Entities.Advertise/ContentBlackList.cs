using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_ContentBlackList")]
public class ContentBlackList : IContentBlackList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	[Required]
	public Guid Guid { get; set; }

	[Required]
	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[Required]
	public string Words { get; set; }
}
