using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ValueChangingEventArgs<T1, T2> : CancelEventArgs
{
	private T1 _OldValue;

	private T2 _NewValue;

	public T1 OldValue => _OldValue;

	public T2 NewValue => _NewValue;

	public ValueChangingEventArgs(T1 oldValue, T2 newValue)
	{
		_OldValue = oldValue;
		_NewValue = newValue;
	}
}
