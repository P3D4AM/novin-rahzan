using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Global;

namespace SharedAccEntityFramworkPersistence.Entities.Global;

[Table("Global_EntityDeliveryStatus")]
public class EntityDeliveryStatus : IEntityDeliveryStatus
{
	public EntityPart EntityPart { get; set; }

	public bool IsTemp { get; set; }

	public Guid EntityGuid { get; set; }
}
