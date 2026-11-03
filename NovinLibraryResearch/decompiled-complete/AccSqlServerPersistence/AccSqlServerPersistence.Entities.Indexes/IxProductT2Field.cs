using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Indexes;

[Table("Acc_IxProductT2Field")]
public class IxProductT2Field : IIndexEntity
{
	[Column(Order = 1)]
	public int WordId { get; set; }

	[Column(Order = 2)]
	public Guid EntityGuid { get; set; }

	public FullTextWords FullTextWords { get; set; }
}
