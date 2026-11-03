using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_PrdAnbarRelation")]
public class PrdAnbarRelation : IPrdAnbarRelation, IHasModified, IHasMetaData
{
	[Column(Order = 0)]
	public Guid PrdGuid { get; set; }

	[Column(Order = 1)]
	public Guid AnbarGuid { get; set; }

	public decimal Min { get; set; }

	public decimal Max { get; set; }

	public decimal Amount { get; set; }

	public DateTime Modified { get; set; }

	public Product Product { get; set; }

	public Depot Depot { get; set; }

	public string? MetaData { get; set; }
}
