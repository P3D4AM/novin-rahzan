using System;

namespace Stimulsoft.Editor;

public class ShowingEventArgs : EventArgs
{
	public ICodeCompletionProvider Provider;

	public bool NeedShow;

	public ShowingEventArgs(ICodeCompletionProvider provider)
	{
		Provider = provider;
		NeedShow = true;
	}
}
