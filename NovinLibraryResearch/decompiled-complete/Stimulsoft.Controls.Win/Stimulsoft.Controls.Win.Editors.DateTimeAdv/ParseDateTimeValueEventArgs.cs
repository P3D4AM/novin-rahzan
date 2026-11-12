using System;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class ParseDateTimeValueEventArgs : EventArgs
{
	public readonly object ValueObject;

	public bool IsParsed;

	private DateTime _ParsedValue = DateTimeGroup.MinDateTime;

	public DateTime ParsedValue
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

	public ParseDateTimeValueEventArgs(object valueObject)
	{
		ValueObject = valueObject;
	}
}
