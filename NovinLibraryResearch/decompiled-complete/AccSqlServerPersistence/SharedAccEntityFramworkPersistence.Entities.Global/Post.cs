using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace SharedAccEntityFramworkPersistence.Entities.Global;

[Table("Global_Post")]
public class Post : IPost, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public byte[] Image { get; set; }

	[MaxLength(100)]
	public string Link { get; set; }

	public DateTime Startdate { get; set; }

	public DateTime EndDate { get; set; }

	[MaxLength(500)]
	public string Title { get; set; }

	public EnTypePost Type { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;

}
