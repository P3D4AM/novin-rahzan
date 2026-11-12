using System.Collections.Generic;
using Serilog.Exceptions.Destructurers;
using Serilog.Exceptions.Filters;

namespace Serilog.Exceptions.Core;

public interface IDestructuringOptions
{
	string RootName { get; }

	int DestructuringDepth { get; }

	IEnumerable<IExceptionDestructurer> Destructurers { get; }

	IExceptionPropertyFilter? Filter { get; }

	bool DisableReflectionBasedDestructurer { get; }
}
