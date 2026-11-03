using System;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Models.Accounting;

public class AccountTreeModel
{
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public string Name { get; set; }

	public long Code { get; set; }

	public ENHesabLevel Level { get; set; }

	public short LevelShort => (short)Level;

	public string LevelName => Level.GetDisplay().GetTranslate();

	public decimal Account { get; set; }
}
