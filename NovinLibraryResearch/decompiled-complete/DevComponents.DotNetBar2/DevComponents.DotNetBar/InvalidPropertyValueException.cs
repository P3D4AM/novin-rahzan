using System;

namespace DevComponents.DotNetBar;

public class InvalidPropertyValueException : Exception
{
	public InvalidPropertyValueException(string message)
		: base(message)
	{
	}
}
