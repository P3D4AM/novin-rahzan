using System;

namespace Stimulsoft.Editor;

public class WordSpellEventArgs : EventArgs
{
	public string Text;

	public bool Correct = true;

	public int ColorStyle;

	public WordSpellEventArgs(string text, bool correct, int colorStyle)
	{
		Text = text;
		Correct = correct;
		ColorStyle = colorStyle;
	}
}
