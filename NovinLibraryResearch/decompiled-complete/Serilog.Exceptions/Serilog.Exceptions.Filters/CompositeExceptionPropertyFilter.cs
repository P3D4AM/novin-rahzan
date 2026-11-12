using System;

namespace Serilog.Exceptions.Filters;

public class CompositeExceptionPropertyFilter : IExceptionPropertyFilter
{
	private readonly IExceptionPropertyFilter[] filters;

	public CompositeExceptionPropertyFilter(params IExceptionPropertyFilter[] filters)
	{
		if (filters == null)
		{
			throw new ArgumentNullException("filters");
		}
		if (filters.Length == 0)
		{
			throw new ArgumentException(Resources.CannotBeEmpty, "filters");
		}
		for (int i = 0; i < filters.Length; i++)
		{
			if (filters[i] == null)
			{
				throw new ArgumentException($"Cannot create composite exception properties filter, because filter at index {i} is null", "filters");
			}
		}
		this.filters = filters;
	}

	public bool ShouldPropertyBeFiltered(Exception exception, string propertyName, object? value)
	{
		for (int i = 0; i < filters.Length; i++)
		{
			if (filters[i].ShouldPropertyBeFiltered(exception, propertyName, value))
			{
				return true;
			}
		}
		return false;
	}
}
