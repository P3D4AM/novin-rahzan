using System;
using System.Collections.Generic;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public interface IExceptionDestructurer
{
	Type[] TargetTypes { get; }

	void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException);
}
