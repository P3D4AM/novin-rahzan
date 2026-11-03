using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.App;

public interface IStiComponentUI
{
	StiReport Report { get; }

	StiPage Page { get; set; }

	bool IsDesigning { get; }

	bool IsEnabled { get; }

	StiEvent GetDefaultEvent();
}
