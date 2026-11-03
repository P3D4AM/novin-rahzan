namespace PacketParser.Entities.Interfaces.Accounting;

public interface IProductAttribute : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Slug { get; set; }

	short Ordinal { get; set; }
}
