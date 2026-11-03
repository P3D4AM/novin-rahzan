namespace PacketParser.Entities.Interfaces.Advertise;

public interface IContentBlackList : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Words { get; set; }
}
