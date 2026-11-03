namespace PacketParser.Entities.Interfaces.Accounting.Tax;

public interface ITax : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Slug { get; set; }
}
