using System;

namespace Stimulsoft.Editor;

public class DrawHeaderEventArgs : EventArgs
{
	public string Tag;

	public string Text;

	public bool Handled;

	public DrawHeaderEventArgs(string tag)
	{
		Tag = tag;
	}
}
