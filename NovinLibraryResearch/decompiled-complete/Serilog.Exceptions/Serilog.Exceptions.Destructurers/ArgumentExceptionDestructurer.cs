using System;
using System.Collections.Generic;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class ArgumentExceptionDestructurer : ExceptionDestructurer
{
	public override Type[] TargetTypes => new Type[2]
	{
		typeof(ArgumentException),
		typeof(ArgumentNullException)
	};

	public override void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		base.Destructure(exception, propertiesBag, destructureException);
		ArgumentException ex = (ArgumentException)exception;
		propertiesBag.AddProperty("ParamName", ex.ParamName);
	}
}
