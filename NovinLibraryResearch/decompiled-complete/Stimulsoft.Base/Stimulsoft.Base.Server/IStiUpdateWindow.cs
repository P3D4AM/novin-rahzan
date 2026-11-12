using System;

namespace Stimulsoft.Base.Server;

public interface IStiUpdateWindow
{
	event EventHandler StartInstall;

	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr);
}
