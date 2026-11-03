namespace PacketParser.Entities.Interfaces.Accounting.Gift;

public interface IGift : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	string Discription { get; set; }

	EnTypeChose Type { get; set; }
}
