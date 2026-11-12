using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ValueChangedEventArgs<T1, T2> : EventArgs
{
	private T1 _OldValue;

	private T2 _NewValue;

	public T1 OldValue => _OldValue;

	public T2 NewValue => _NewValue;

	public ValueChangedEventArgs(T1 oldValue, T2 newValue)
	{
		_OldValue = oldValue;
		_NewValue = newValue;
	}
}
public class ValueChangedEventArgs<T1, T2, T3> : EventArgs
{
	private T1 _OldValue;

	private T2 _NewValue;

	private T3 _EventSource;

	public T1 OldValue => _OldValue;

	public T2 NewValue => _NewValue;

	public T3 EventSource => _EventSource;

	public ValueChangedEventArgs(T1 oldValue, T2 newValue, T3 eventSource)
	{
		_OldValue = oldValue;
		_NewValue = newValue;
		_EventSource = eventSource;
	}
}
