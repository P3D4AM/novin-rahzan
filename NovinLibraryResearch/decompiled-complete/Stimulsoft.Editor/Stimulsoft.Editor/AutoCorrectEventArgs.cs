using System;

namespace Stimulsoft.Editor;

public class AutoCorrectEventArgs : EventArgs
{
	public string Word;

	public string CorrectWord;

	public bool HasCorrection;
}
