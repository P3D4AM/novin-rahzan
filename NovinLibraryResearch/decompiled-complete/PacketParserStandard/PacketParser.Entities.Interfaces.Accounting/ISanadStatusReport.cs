namespace PacketParser.Entities.Interfaces.Accounting;

public interface ISanadStatusReport : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Id { get; set; }

	Document AsType { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	int WarningMinutes { get; set; }

	int ErrorMinutes { get; set; }

	long? Id2From { get; set; }

	long? Id2To { get; set; }

	int InactivityDays { get; set; }

	int TargetStatusId { get; set; }
}
