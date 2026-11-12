using System;

namespace Stimulsoft.Editor;

public class ClosingEventArgs : EventArgs
{
	public bool Accepted;

	public bool UseIndent = true;

	public bool Handled;

	public string Text;

	public ICodeCompletionProvider Provider;

	public ClosingEventArgs(bool accepted, ICodeCompletionProvider provider)
	{
		Accepted = accepted;
		Provider = provider;
	}
}
