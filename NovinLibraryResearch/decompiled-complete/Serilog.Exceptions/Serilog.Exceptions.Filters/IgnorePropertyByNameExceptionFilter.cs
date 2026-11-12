using System;

namespace Serilog.Exceptions.Filters;

public class IgnorePropertyByNameExceptionFilter : IExceptionPropertyFilter
{
	private readonly string[] propertiesToIgnore;

	public IgnorePropertyByNameExceptionFilter(params string[] propertiesToIgnore)
	{
		this.propertiesToIgnore = propertiesToIgnore;
	}

	public bool ShouldPropertyBeFiltered(Exception exception, string propertyName, object? value)
	{
		if (propertiesToIgnore == null)
		{
			return false;
		}
		for (int i = 0; i < propertiesToIgnore.Length; i++)
		{
			if (propertiesToIgnore[i].Equals(propertyName, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}
}
