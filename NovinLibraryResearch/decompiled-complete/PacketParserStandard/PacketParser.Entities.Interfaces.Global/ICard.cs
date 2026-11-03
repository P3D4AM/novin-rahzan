namespace PacketParser.Entities.Interfaces.Global;

public interface ICard : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	EnCardTheme Theme { get; set; }

	string ToolTipMessage { get; set; }

	byte[] Image { get; set; }

	string BackColor { get; set; }

	EnCardType Type { get; set; }
}
