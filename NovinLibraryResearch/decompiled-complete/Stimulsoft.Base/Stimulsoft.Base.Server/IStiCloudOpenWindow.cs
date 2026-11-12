using System;

namespace Stimulsoft.Base.Server;

public interface IStiCloudOpenWindow
{
	string ReportItemKey { get; }

	IStiReport Report { get; }

	bool? ShowDialog(IntPtr intPtr, double? left, double? top);
}
