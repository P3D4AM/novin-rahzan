using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Global_MetaData")]
public class MetaData : IMetaData, IMetaData_Base
{
	[Key]
	[Column(Order = 0)]
	public Guid EntityGuid { get; set; }

	[Key]
	[Column(Order = 1)]
	public EntityPart EntityType { get; set; }

	[Key]
	[Column(Order = 2)]
	[Required]
	public string Key { get; set; }

	[Required]
	public string Value { get; set; }

	public EnDataType DataType { get; set; }
}
