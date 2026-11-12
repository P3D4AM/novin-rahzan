using System;

namespace Stimulsoft.Editor;

internal struct CWPSTRUCT
{
	public IntPtr lParam;

	public IntPtr wParam;

	public int message;

	public IntPtr hwnd;
}
