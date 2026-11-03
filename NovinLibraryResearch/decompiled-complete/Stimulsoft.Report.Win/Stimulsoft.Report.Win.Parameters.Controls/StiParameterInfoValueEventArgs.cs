using System;

namespace Stimulsoft.Report.Win.Parameters.Controls;

public class StiParameterInfoValueEventArgs : EventArgs
{
	public string Label { get; }

	public object NewValue { get; }

	public StiParameterInfoValueEventArgs(object newValue, string label)
	{
		Label = label;
		NewValue = newValue;
	}
}
