using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.Web;

internal class StiCacheGuid
{
	public string ReportKey { get; set; }

	public List<string> ReportGuids { get; set; } = new List<string>();


	public DateTime UpdateTime { get; set; }
}
