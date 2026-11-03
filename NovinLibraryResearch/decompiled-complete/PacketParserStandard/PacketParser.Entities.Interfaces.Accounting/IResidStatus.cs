namespace PacketParser.Entities.Interfaces.Accounting;

public interface IResidStatus : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Description { get; set; }
}
