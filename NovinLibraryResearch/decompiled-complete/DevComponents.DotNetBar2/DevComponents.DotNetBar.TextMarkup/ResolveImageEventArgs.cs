using System;
using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

public class ResolveImageEventArgs : EventArgs
{
	public bool Handled;

	public string Key = "";

	public Image ResolvedImage;
}
