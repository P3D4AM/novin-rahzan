using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal interface ISkinHook
{
	void PostMouseMove(IntPtr hWnd, Point mousePos);

	void PostMouseUp(IntPtr hWnd, Point mousePos);
}
