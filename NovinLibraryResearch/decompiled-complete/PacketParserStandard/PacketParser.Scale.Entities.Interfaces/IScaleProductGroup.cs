namespace PacketParser.Scale.Entities.Interfaces;

public interface IScaleProductGroup
{
	short Id { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	bool St { get; set; }
}
