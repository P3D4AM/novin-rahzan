namespace PacketParser.Entities.Interfaces.Accounting;

public interface IGhafase : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Part1 { get; set; }

	string Part2 { get; set; }

	string Part3 { get; set; }

	string Description { get; set; }
}
