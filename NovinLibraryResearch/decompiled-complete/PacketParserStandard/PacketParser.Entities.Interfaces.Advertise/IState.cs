namespace PacketParser.Entities.Interfaces.Advertise;

public interface IState : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string StateName { get; set; }
}
