using System;

namespace Stimulsoft.Report.Export;

public class StiExportingEventArgs : EventArgs
{
	public int Maximum { get; }

	public int Value { get; }

	public int MaximumPass { get; }

	public int CurrentPass { get; }

	public StiExportingEventArgs(int value, int maximum, int currentPass, int maximumPass)
	{
		Value = value;
		Maximum = maximum;
		CurrentPass = currentPass;
		MaximumPass = maximumPass;
	}
}
