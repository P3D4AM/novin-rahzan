using System;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Server;

public interface IStiPublishWindow
{
	bool IsNeedToCheck { get; set; }

	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr);

	bool? ShowDialog(IntPtr intPtr, PointD centerOwner);
}
