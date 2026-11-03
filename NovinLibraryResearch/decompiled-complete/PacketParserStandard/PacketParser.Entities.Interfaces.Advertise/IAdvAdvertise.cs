using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IAdvAdvertise : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	Guid GroupGuid { get; set; }

	decimal Price { get; set; }

	bool IsPishNevis { get; set; }

	string ImageList { get; set; }

	string TitleList { get; set; }

	string ContentList { get; set; }

	string DivatCategoryList { get; set; }

	string SheypoorCategoryList { get; set; }
}
