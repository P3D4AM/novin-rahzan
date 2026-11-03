using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_TableRelationShip")]
public class TableRelationShip : ITableRelationShip, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public decimal Price { get; set; }

	public Guid SanadGuid { get; set; }

	public Document SanadType { get; set; }

	public Guid RelatedSanadGuid { get; set; }

	public Document RelatedSanadType { get; set; }
}
