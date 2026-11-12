using System;
using System.Drawing;

namespace Stimulsoft.Editor;

public class ReadFormattedTextEventArgs : EventArgs
{
	public string Text;

	public Color ForeColor;

	public Color BackColor;

	public FontStyle FontStyle;

	public object UserData;
}
