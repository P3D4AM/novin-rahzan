using System;

namespace DevComponents.DotNetBar;

public class ValueChangedEventArgs<T1, T2> : EventArgs
{
	private T1 __1FCE;

	private T2 __1FCF;

	public T1 OldValue => __1FCE;

	public T2 NewValue => __1FCF;

	public ValueChangedEventArgs(T1 oldValue, T2 newValue)
	{
		__1FCE = oldValue;
		__1FCF = newValue;
	}
}
public class ValueChangedEventArgs<T1, T2, T3> : EventArgs
{
	private T1 __1FCE;

	private T2 __1FCF;

	private T3 _ῐ;

	public T1 OldValue => __1FCE;

	public T2 NewValue => __1FCF;

	public T3 EventSource => _ῐ;

	public ValueChangedEventArgs(T1 oldValue, T2 newValue, T3 eventSource)
	{
		__1FCE = oldValue;
		__1FCF = newValue;
		_ῐ = eventSource;
	}
}
