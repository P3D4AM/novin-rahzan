namespace PacketParser.Entities.Interfaces;

public interface ISuggestedBank : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string ShobeCode { get; set; }

	string AccountNo { get; set; }

	string BankName { get; set; }
}
