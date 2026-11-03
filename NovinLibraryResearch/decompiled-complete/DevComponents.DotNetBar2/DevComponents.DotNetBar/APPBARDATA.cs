using System;

namespace DevComponents.DotNetBar;

public struct APPBARDATA
{
	public uint cbSize;

	public IntPtr hWnd;

	public uint uCallbackMessage;

	public ABE uEdge;

	public RECT rc;

	public int lParam;
}
