namespace PacketParser.Entities.Interfaces;

public interface IParDarDet : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	decimal Price { get; }
}
