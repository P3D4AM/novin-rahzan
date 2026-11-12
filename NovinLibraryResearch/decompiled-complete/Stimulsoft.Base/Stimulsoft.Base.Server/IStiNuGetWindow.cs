using System;

namespace Stimulsoft.Base.Server;

public interface IStiNuGetWindow
{
	bool IsDownloaded { get; set; }

	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr);

	void Close();
}
