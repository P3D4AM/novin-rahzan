namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IFacilities : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	decimal Price { get; set; }
}
