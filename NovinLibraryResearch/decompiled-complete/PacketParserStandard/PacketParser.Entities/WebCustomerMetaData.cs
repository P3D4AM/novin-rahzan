using System;

namespace PacketParser.Entities;

public class WebCustomerMetaData
{
	public Guid EntityGuid { get; set; }

	public EntityPart EntityType { get; set; }

	public string Key { get; set; }

	public string Value { get; set; }

	public EnDataType DataType { get; set; }

	public string ContextGuid { get; set; }

	public CustomerMetaKey MetaKey { get; set; }
}
