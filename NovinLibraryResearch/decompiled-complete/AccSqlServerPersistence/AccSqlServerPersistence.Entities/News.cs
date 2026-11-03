using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_News")]
public class News : Serializable<INews>, INews, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	[Required]
	public string Category { get; set; }

	public DateTime DateM { get; set; }

	[Required]
	public string FullContent { get; set; }

	[Required]
	public string KeyWords { get; set; }

	[Required]
	public string MetaDescription { get; set; }

	[Required]
	public string PicUrl { get; set; }

	[Required]
	public string QuickText { get; set; }

	[Required]
	public string Title { get; set; }

	public NewsType Type { get; set; }
}
