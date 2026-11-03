namespace PacketParser.Entities.Interfaces.Advertise;

public interface IDivarCities : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string CityName { get; set; }
}
