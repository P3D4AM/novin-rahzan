using PacketParser.Scale.Entities.Interfaces;

namespace PacketParser.Scale.Entities;

public class BaseScaleProductVahed : IScalePrdVahed
{
	public string Description { get; set; }

	public short Id { get; set; }

	public string Name { get; set; }

	public bool Status { get; set; }
}
