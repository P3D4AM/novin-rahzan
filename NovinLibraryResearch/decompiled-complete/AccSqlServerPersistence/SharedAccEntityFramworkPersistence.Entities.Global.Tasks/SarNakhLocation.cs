using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

[Table("Global_SarNakhLocation")]
public class SarNakhLocation : ISarNakhLocation
{
	public Guid SarNakhGuid { get; set; }

	public int Ordinal { get; set; }

	[MaxLength(500)]
	public string? Email { get; set; }

	[MaxLength(500)]
	public string? WebSite { get; set; }

	[MaxLength(500)]
	public string? Address { get; set; }

	[MaxLength(500)]
	public string? State { get; set; }

	[MaxLength(500)]
	public string? City { get; set; }

	public SarNakh SarNakh { get; set; }
}
