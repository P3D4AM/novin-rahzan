using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal interface IMessageHandlerClient
{
	bool IsModal { get; }

	bool OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam);

	bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam);

	bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam);

	bool OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam);

	bool OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam);

	bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam);
}
