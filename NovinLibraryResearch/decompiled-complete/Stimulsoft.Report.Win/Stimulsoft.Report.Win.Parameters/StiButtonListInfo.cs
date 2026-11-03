using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Win.Parameters.List;

namespace Stimulsoft.Report.Win.Parameters;

public class StiButtonListInfo
{
	public StiVariable Variable { get; set; }

	public StiDialogInfo Info { get; set; }

	public StiListType Type { get; set; }

	public StiListButtonXControl Control { get; set; }
}
