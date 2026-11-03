using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_HyperLink")]
public class HyperLink : Serializable<IHyperLink>, IHyperLink, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[Required]
	public string Category { get; set; }

	public bool Enable { get; set; }

	public string? Icon { get; set; }

	public short Order { get; set; }

	public Guid ParentGuid { get; set; }

	public int ParentId { get; set; }

	[Required]
	public string Style { get; set; }

	[Required]
	public string Target { get; set; }

	[Required]
	public string Title { get; set; }

	public string? Url { get; set; }

	public int Id { get; set; }
}
