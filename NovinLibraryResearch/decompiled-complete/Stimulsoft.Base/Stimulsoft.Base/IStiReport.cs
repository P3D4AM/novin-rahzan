using System.Collections.Generic;

namespace Stimulsoft.Base;

public interface IStiReport : IStiApp, IStiAppCell
{
	IEnumerable<IStiReportPage> FetchPages();
}
