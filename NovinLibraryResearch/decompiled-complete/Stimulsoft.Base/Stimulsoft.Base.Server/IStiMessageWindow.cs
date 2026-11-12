using System;

namespace Stimulsoft.Base.Server;

public interface IStiMessageWindow
{
	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr);
}
