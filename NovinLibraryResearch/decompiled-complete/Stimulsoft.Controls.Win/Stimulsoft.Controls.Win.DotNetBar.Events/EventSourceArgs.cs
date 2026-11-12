using System;

namespace Stimulsoft.Controls.Win.DotNetBar.Events;

public class EventSourceArgs : EventArgs
{
	public readonly eEventSource Source = eEventSource.Code;

	public readonly eShortcut Key;

	public EventSourceArgs(eEventSource source)
		: this(source, eShortcut.None)
	{
	}

	public EventSourceArgs(eEventSource source, eShortcut key)
	{
		Source = source;
		Key = key;
	}
}
