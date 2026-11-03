namespace PacketParser.Entities.Interfaces.Accounting;

public interface IEn2Fa : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string LName { get; set; }

	string FName { get; set; }
}
