using System;

namespace DevComponents.Editors;

public class ParseIntegerValueEventArgs : EventArgs
{
	public readonly object ValueObject;

	public bool IsParsed;

	private int __2348;

	public int ParsedValue
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

	public ParseIntegerValueEventArgs(object valueObject)
	{
		ValueObject = valueObject;
	}
}
