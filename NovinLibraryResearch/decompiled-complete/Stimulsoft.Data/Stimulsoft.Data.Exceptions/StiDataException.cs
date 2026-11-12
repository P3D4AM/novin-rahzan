using System;

namespace Stimulsoft.Data.Exceptions;

public abstract class StiDataException : Exception
{
	public StiDataException()
	{
	}

	public StiDataException(string message)
		: base(message)
	{
	}
}
