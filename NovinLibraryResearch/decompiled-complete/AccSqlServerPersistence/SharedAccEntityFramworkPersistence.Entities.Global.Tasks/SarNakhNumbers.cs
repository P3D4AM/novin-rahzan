using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

[Table("Global_SarNakhNumbers")]
public class SarNakhNumbers : ISarNakhNumbers
{
	public string Number { get; set; }

	public Guid SarNakhGuid { get; set; }

	[MaxLength(500)]
	public string? Title { get; set; }

	public SarNakh SarNakh { get; set; }
}
