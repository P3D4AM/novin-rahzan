namespace PacketParser.Entities.Interfaces;

public interface IHasIsChecked : IHasJustGuid
{
	bool IsChecked { get; set; }
}
