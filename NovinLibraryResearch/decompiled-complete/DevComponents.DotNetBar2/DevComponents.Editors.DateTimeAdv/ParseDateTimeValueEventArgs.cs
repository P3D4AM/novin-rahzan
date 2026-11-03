using System;

namespace DevComponents.Editors.DateTimeAdv;

public class ParseDateTimeValueEventArgs : EventArgs
{
	public readonly object ValueObject;

	public bool IsParsed;

	private DateTime __2348 = DateTimeGroup.MinDateTime;

	public DateTime ParsedValue
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

	public ParseDateTimeValueEventArgs(object valueObject)
	{
		ValueObject = valueObject;
	}
}
