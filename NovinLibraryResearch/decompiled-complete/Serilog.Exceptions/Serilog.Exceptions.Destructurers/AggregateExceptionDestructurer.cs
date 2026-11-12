using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class AggregateExceptionDestructurer : ExceptionDestructurer
{
	public override Type[] TargetTypes => new Type[1] { typeof(AggregateException) };

	public override void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		base.Destructure(exception, propertiesBag, destructureException);
		AggregateException ex = (AggregateException)exception;
		propertiesBag.AddProperty("InnerExceptions", ex.InnerExceptions.Select(destructureException).ToList());
	}
}
