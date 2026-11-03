using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.SerializeData;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_TafsilGroup")]
public class TafsilGroup : ITafsilGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	[Required]
	public string Name { get; set; }

	public string? Tbl { get; set; }

	[Required]
	public string Description { get; set; }

	public long CodeMin { get; set; }

	public long CodeMax { get; set; }

	public bool Status { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid MoeinSellGuid { get; set; }

	public Guid MoeinBuyGuid { get; set; }

	public Guid MoeinBackSellGuid { get; set; }

	public Guid MoeinBackBuyGuid { get; set; }

	public Guid MoeinReceptionGuid { get; set; }

	public Guid MoeinPardakhtGuid { get; set; }

	public Guid MoeinAmaniMaNazdDigaranGuid { get; set; }

	public Guid MoeinAmaniDigaranNazdMaGuid { get; set; }

	public EnTafsilGroupType Type { get; set; }

	[NotMapped]
	public DefualtBazaryabGuidInTafsilGroup BazaryabDefaultGuid { get; set; }

	public string? BazaryabDefaultGuidData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public string? MetaData { get; set; }

	public ICollection<MoeinTafsilGroupRelations> MoeinTafsilLevelRelations { get; set; }

	public ICollection<Tafsil> TafsilRealtion { get; set; }

	public ICollection<CustomerGroup> CustomerGroupRealtion { get; set; }
}
