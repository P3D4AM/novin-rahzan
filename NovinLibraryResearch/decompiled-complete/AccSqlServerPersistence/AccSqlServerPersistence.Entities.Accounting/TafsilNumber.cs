using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_TafsilNumbers")]
public class TafsilNumber : ITafsilNumbers
{
	[Column(Order = 1)]
	public Guid TafsilGuid { get; set; }

	[Column(Order = 2)]
	public long OriginalNumber { get; set; }

	public long CalcedNumber { get; set; }

	public short Prefix { get; set; }

	[Required]
	public string Title { get; set; }

	public short Ordinal { get; set; }

	public Tafsil TafsilItem { get; set; }
}
