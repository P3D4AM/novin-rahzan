using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ValueChangingSourceEventArgs<T1, T2, T3> : CancelEventArgs
{
	private T1 _OldValue;

	private T2 _NewValue;

	private T3 _EventSource;

	public T1 OldValue => _OldValue;

	public T2 NewValue => _NewValue;

	public T3 EventSource => _EventSource;

	public ValueChangingSourceEventArgs(T1 oldValue, T2 newValue, T3 eventSource)
	{
		_OldValue = oldValue;
		_NewValue = newValue;
		_EventSource = eventSource;
	}
}
