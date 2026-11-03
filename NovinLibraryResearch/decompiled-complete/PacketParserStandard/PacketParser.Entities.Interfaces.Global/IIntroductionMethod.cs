namespace PacketParser.Entities.Interfaces.Global;

public interface IIntroductionMethod : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }
}
