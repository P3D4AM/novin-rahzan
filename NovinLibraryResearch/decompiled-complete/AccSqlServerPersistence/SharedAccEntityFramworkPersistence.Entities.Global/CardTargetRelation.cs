using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Global;
using PacketParser.Entities.Interfaces.Global;

namespace SharedAccEntityFramworkPersistence.Entities.Global;

[Table("Global_CardTargetRelation")]
public class CardTargetRelation : ICardTargetRelation
{
	public Guid TargetGuid { get; set; }

	public Guid CardGuid { get; set; }

	public Card CardItem { get; set; }
}
