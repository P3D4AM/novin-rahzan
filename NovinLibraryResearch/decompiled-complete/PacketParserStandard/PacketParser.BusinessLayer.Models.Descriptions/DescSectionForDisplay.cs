using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Descriptions;

public class DescSectionForDisplay
{
	public DescSection Section { get; set; }

	public string SectionForDisplay => Section.GetDisplay();

	public DescSectionForDisplay(DescSection model)
	{
		Section = model;
	}
}
