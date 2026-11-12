using System;
using System.Collections.Generic;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class ArgumentOutOfRangeExceptionDestructurer : ArgumentExceptionDestructurer
{
	public override Type[] TargetTypes => new Type[1] { typeof(ArgumentOutOfRangeException) };

	public override void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		base.Destructure(exception, propertiesBag, destructureException);
		ArgumentOutOfRangeException ex = (ArgumentOutOfRangeException)exception;
		propertiesBag.AddProperty("ActualValue", ex.ActualValue);
	}
}
