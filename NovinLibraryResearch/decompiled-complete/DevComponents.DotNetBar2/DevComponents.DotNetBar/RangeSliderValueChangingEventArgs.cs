using DevComponents.DotNetBar.Events;

namespace DevComponents.DotNetBar;

public class RangeSliderValueChangingEventArgs : EventSourceArgs
{
	public readonly RangeValue NewValue;

	public bool Cancel;

	public RangeSliderValueChangingEventArgs(eEventSource source, RangeValue newValue)
		: base(source)
	{
		NewValue = newValue;
	}
}
