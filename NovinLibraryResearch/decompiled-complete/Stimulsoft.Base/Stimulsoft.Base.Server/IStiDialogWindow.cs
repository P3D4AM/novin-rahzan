using System;

namespace Stimulsoft.Base.Server;

public interface IStiDialogWindow
{
	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr);
}
