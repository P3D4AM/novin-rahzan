namespace PacketParser.Entities.Interfaces.Accounting.ProductAttrebutes;

public interface IProductAttributeValue : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	string Name { get; set; }

	string Slug { get; set; }
}
