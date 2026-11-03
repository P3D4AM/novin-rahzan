using System;
using System.Drawing;

namespace DevComponents.DotNetBar.Controls;

public class TokenEditorPopupEventArgs : EventArgs
{
	public Point PopupLocation = Point.Empty;

	public readonly Size PopupSize;

	public TokenEditorPopupEventArgs(Point popupLocation, Size popupSize)
	{
		PopupLocation = popupLocation;
		PopupSize = popupSize;
	}
}
