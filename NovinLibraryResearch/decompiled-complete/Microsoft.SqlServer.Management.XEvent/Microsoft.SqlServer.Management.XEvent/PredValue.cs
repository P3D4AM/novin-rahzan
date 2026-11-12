using System;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PredValue : Predicate
{
	private object value;

	public PredValue(object value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		this.value = value;
	}

	public override string ToString()
	{
		return value.ToString();
	}
}
