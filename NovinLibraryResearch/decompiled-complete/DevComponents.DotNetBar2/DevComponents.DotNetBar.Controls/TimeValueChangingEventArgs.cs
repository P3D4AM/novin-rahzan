using System;

namespace DevComponents.DotNetBar.Controls;

public class TimeValueChangingEventArgs : EventArgs
{
	public readonly DateTime Time;

	public TimeValueChangingEventArgs(DateTime time)
	{
		Time = time;
	}
}
