namespace PacketParser.Entities.Interfaces;

public interface IEntityCache : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	EntityChangeSt ChangeStatus { get; set; }
}
