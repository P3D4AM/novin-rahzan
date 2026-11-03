namespace PacketParser.Entities.Interfaces.Accounting;

public interface INaghz : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Message { get; set; }

	short Count { get; set; }
}
