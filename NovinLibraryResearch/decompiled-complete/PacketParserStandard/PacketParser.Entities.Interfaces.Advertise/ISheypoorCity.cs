namespace PacketParser.Entities.Interfaces.Advertise;

public interface ISheypoorCity : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string CityName { get; set; }
}
