using System;

namespace Stimulsoft.Editor;

public class NotifyEventArgs : EventArgs
{
	public NotifyState State;

	public int FirstChanged;

	public int LastChanged;

	public bool Update;

	public NotifyEventArgs()
	{
	}

	public NotifyEventArgs(NotifyState state, int first, int last, bool update)
	{
		State = state;
		FirstChanged = first;
		LastChanged = last;
		Update = update;
	}
}
