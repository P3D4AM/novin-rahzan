using System;

namespace Stimulsoft.Base.Server;

public interface IStiSubscriptionsWindow
{
	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr);
}
