using System;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Server;

public interface IStiTrialDaysLeftWindow
{
	bool? ShowDialog();

	bool? ShowDialog(IntPtr intPtr, PointD centerOwner);

	bool? ShowDialog(IntPtr intPtr);
}
