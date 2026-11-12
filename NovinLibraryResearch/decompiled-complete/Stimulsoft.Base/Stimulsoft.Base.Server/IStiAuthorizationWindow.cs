using System;

namespace Stimulsoft.Base.Server;

public interface IStiAuthorizationWindow
{
	bool CheckClose { get; set; }

	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr);
}
