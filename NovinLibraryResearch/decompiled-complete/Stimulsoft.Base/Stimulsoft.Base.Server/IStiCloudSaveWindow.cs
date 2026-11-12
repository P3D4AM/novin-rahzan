using System;

namespace Stimulsoft.Base.Server;

public interface IStiCloudSaveWindow
{
	string ReportItemKey { get; }

	IStiReport Report { get; }

	bool? ShowDialog(IntPtr intPtr, double? left, double? top);
}
