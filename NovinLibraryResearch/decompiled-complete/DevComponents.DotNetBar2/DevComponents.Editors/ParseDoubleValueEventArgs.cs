using System;

namespace DevComponents.Editors;

public class ParseDoubleValueEventArgs : EventArgs
{
	public readonly object ValueObject;

	public bool IsParsed;

	private double __2348;

	public double ParsedValue
	{
		get
		{
			return __2348;
		}
		set
		{
			__2348 = value;
			IsParsed = true;
		}
	}

	public ParseDoubleValueEventArgs(object valueObject)
	{
		ValueObject = valueObject;
	}
}
