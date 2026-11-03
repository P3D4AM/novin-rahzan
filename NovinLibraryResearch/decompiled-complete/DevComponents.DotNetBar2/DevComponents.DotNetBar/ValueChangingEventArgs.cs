using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class ValueChangingEventArgs<T1, T2> : CancelEventArgs
{
	private T1 __1FCE;

	private T2 __1FCF;

	public T1 OldValue => __1FCE;

	public T2 NewValue => __1FCF;

	public ValueChangingEventArgs(T1 oldValue, T2 newValue)
	{
		__1FCE = oldValue;
		__1FCF = newValue;
	}
}
