namespace PacketParser.Entities.Interfaces.Accounting.Selection;

public interface IPrdGroupSelectedDetList : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	string Description { get; set; }
}
