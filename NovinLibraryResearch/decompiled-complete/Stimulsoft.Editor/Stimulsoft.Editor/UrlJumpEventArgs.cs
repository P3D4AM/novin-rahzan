using System;

namespace Stimulsoft.Editor;

public class UrlJumpEventArgs : EventArgs
{
	public string Text;

	public bool Handled;

	public UrlJumpEventArgs(string text, bool handled)
	{
		Text = text;
		Handled = handled;
	}
}
