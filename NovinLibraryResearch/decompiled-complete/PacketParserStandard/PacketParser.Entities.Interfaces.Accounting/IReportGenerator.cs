namespace PacketParser.Entities.Interfaces.Accounting;

public interface IReportGenerator : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	string Description { get; set; }

	string Rpt { get; set; }

	int Version { get; set; }

	StiType? Type { get; set; }

	Document? Document { get; set; }

	MaliYearSanadType MaliYearSanadType { get; set; }
}
