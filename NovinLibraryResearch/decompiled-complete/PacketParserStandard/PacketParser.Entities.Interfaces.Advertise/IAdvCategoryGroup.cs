namespace PacketParser.Entities.Interfaces.Advertise;

public interface IAdvCategoryGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string DivarCategoryList { get; set; }

	string SheypoorCategoryList { get; set; }
}
