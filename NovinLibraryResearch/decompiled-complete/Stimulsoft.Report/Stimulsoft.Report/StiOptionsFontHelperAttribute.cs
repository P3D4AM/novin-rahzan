using System;

namespace Stimulsoft.Report;

public class StiOptionsFontHelperAttribute : Attribute
{
	public int Index { get; }

	public StiOptionsFontHelperAttribute(int index)
	{
		Index = index;
	}
}
