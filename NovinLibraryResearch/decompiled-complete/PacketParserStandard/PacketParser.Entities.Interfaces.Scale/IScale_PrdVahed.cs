namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_PrdVahed : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	string Name { get; set; }
}
