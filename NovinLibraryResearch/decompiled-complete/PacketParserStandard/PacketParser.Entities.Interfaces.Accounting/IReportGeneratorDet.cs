namespace PacketParser.Entities.Interfaces.Accounting;

public interface IReportGeneratorDet : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string ParameterName { get; set; }

	EnReportGeneratorFilterType ParameterType { get; set; }

	short OrderIndex { get; set; }
}
