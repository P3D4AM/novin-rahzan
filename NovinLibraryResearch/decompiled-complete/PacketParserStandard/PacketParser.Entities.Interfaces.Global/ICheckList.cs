namespace PacketParser.Entities.Interfaces.Global;

public interface ICheckList
{
	string Title { get; set; }

	bool IsChecked { get; set; }

	int Ordinal { get; set; }
}
