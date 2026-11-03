using System;

namespace DevComponents.DotNetBar.Controls;

public class RemovingTokenEventArgs : EventArgs
{
	public readonly EditToken Token;

	public bool Cancel;

	public readonly eEventSource EventSource;

	public RemovingTokenEventArgs(EditToken token, eEventSource eventSource)
	{
		Token = token;
		EventSource = eventSource;
	}
}
