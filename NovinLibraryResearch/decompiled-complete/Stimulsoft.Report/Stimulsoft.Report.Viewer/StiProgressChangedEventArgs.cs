using System;

namespace Stimulsoft.Report.Viewer;

public class StiProgressChangedEventArgs : EventArgs
{
	public int Value { get; }

	public int Maximum { get; }

	public StiProgressChangedEventArgs(int value, int maximum)
	{
		Value = value;
		Maximum = maximum;
	}
}
