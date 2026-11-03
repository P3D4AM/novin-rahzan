using System;

namespace Stimulsoft.Report;

public class StiException : Exception
{
	public StiException()
		: this(string.Empty)
	{
	}

	public StiException(string value)
		: base(value)
	{
	}
}
