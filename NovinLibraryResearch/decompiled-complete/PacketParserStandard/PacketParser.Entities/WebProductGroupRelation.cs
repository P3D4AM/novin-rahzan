using System;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces.Accounting.Selection;

namespace PacketParser.Entities;

public class WebProductGroupRelation : IProductGroupRelation
{
	[JsonIgnore]
	public Guid ProductGuid { get; set; }

	public Guid GroupGuid { get; set; }

	public short Ordinal { get; set; }
}
