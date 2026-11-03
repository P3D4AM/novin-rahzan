namespace PacketParser.Entities.Interfaces.Advertise;

public interface ITitleBlackList : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Words { get; set; }
}
