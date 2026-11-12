using System;

namespace Stimulsoft.Editor;

public class HyperTextEventArgs : EventArgs
{
	public string Text;

	public bool IsHyperText;

	public HyperTextEventArgs(string text, bool isHyperText)
	{
		Text = text;
		IsHyperText = isHyperText;
	}
}
