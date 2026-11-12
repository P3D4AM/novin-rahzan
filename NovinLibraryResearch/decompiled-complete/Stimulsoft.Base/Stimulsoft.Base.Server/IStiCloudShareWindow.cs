using System;

namespace Stimulsoft.Base.Server;

public interface IStiCloudShareWindow
{
	string ReportItemKey { get; set; }

	IStiReport Report { get; set; }

	bool IsReportSaveCloud { get; set; }

	bool? ShowDialog(IntPtr intPtr);
}
