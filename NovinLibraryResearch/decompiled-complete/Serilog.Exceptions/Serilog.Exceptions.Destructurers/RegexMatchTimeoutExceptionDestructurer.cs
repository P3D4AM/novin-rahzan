using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class RegexMatchTimeoutExceptionDestructurer : ExceptionDestructurer
{
	public override Type[] TargetTypes => new Type[1] { typeof(RegexMatchTimeoutException) };

	public override void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		base.Destructure(exception, propertiesBag, destructureException);
		RegexMatchTimeoutException ex = (RegexMatchTimeoutException)exception;
		propertiesBag.AddProperty("Input", ex.Input);
		propertiesBag.AddProperty("Pattern", ex.Pattern);
		propertiesBag.AddProperty("MatchTimeout", ex.MatchTimeout.ToString("c"));
	}
}
