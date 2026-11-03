namespace PacketParser.Entities.Interfaces;

public interface IInternetUpdate
{
	FileManagement_Parts Source { get; set; }

	string Version { get; set; }
}
