using System;
using PacketParser.Entities.SerializeData;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ITafsilGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Description { get; set; }

	string Tbl { get; set; }

	long CodeMin { get; set; }

	long CodeMax { get; set; }

	Guid MoeinSellGuid { get; set; }

	Guid MoeinBuyGuid { get; set; }

	Guid MoeinBackSellGuid { get; set; }

	Guid MoeinBackBuyGuid { get; set; }

	Guid MoeinReceptionGuid { get; set; }

	Guid MoeinPardakhtGuid { get; set; }

	Guid MoeinAmaniMaNazdDigaranGuid { get; set; }

	Guid MoeinAmaniDigaranNazdMaGuid { get; set; }

	ServerStatusForClient ServerStatus { get; set; }

	EnTafsilGroupType Type { get; set; }

	DefualtBazaryabGuidInTafsilGroup BazaryabDefaultGuid { get; set; }
}
