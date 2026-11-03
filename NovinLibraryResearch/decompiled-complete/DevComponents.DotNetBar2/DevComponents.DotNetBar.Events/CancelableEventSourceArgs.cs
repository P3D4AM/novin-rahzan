namespace DevComponents.DotNetBar.Events;

public class CancelableEventSourceArgs : EventSourceArgs
{
	public bool Cancel;

	public CancelableEventSourceArgs(eEventSource source)
		: base(source)
	{
	}

	public CancelableEventSourceArgs(eEventSource source, bool cancel)
		: base(source)
	{
		Cancel = cancel;
	}
}
