using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components;

public interface IStiBusinessObject : IStiEnumerator
{
	StiBusinessObject BusinessObject { get; }

	string BusinessObjectGuid { get; set; }

	bool IsBusinessObjectEmpty { get; }
}
