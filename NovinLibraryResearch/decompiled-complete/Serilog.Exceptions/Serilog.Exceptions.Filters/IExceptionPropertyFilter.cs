using System;

namespace Serilog.Exceptions.Filters;

public interface IExceptionPropertyFilter
{
	bool ShouldPropertyBeFiltered(Exception exception, string propertyName, object? value);
}
