using System;

namespace Stimulsoft.Base.Server;

public interface IStiUnsaveReportWindow
{
	string SelectedReportPath { get; }

	bool? ShowDialog();

	bool? ShowDialog(IntPtr owner);
}
