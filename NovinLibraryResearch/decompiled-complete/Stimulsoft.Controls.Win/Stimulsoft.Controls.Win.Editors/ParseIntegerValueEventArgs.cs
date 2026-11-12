using System;

namespace Stimulsoft.Controls.Win.Editors;

public class ParseIntegerValueEventArgs : EventArgs
{
	public readonly object ValueObject;

	public bool IsParsed;

	private int _ParsedValue;

	public int ParsedValue
	{
		get
		{
			return _ParsedValue;
		}
		set
		{
			_ParsedValue = value;
			IsParsed = true;
		}
	}

	public ParseIntegerValueEventArgs(object valueObject)
	{
		ValueObject = valueObject;
	}
}
