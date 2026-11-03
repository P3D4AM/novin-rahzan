using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class MetaDataFilter
{
	public Guid? EntityGuid { get; set; }

	public EntityPart? EntityType { get; set; }

	public string Key { get; set; } = "";


	public string Value { get; set; } = "";


	public EnDataType? DataType { get; set; }
}
