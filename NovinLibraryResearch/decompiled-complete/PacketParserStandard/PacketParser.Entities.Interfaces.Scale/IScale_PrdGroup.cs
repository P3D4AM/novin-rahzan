namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_PrdGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	string Name { get; set; }
}
