using System;

namespace Stimulsoft.Controls.Win.Editors;

public class ParseDoubleValueEventArgs : EventArgs
{
	public readonly object ValueObject;

	public bool IsParsed;

	private double _ParsedValue;

	public double ParsedValue
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

	public ParseDoubleValueEventArgs(object valueObject)
	{
		ValueObject = valueObject;
	}
}
