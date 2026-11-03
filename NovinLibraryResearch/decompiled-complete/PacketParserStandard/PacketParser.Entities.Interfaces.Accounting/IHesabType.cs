namespace PacketParser.Entities.Interfaces.Accounting;

public interface IHesabType : IHasJustGuid, IHasStatus
{
	string Name { get; set; }

	short Code { get; set; }
}
