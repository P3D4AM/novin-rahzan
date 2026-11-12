using System;
using System.Collections.Generic;
using System.Threading;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class OperationCanceledExceptionDestructurer : ExceptionDestructurer
{
	private const string CancellationTokenTrue = "CancellationRequested: true";

	private const string CancellationTokenFalse = "CancellationRequested: false";

	private static readonly Type[] TargetExceptionTypes = new Type[1] { typeof(OperationCanceledException) };

	public override Type[] TargetTypes => TargetExceptionTypes;

	public override void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		base.Destructure(exception, propertiesBag, destructureException);
		OperationCanceledException ex = (OperationCanceledException)exception;
		CancellationToken cancellationToken = ex.CancellationToken;
		propertiesBag.AddProperty("CancellationToken", DestructureCancellationToken(in cancellationToken));
	}

	internal static object DestructureCancellationToken(in CancellationToken cancellationToken)
	{
		CancellationToken cancellationToken2 = cancellationToken;
		if (!cancellationToken2.IsCancellationRequested)
		{
			return "CancellationRequested: false";
		}
		return "CancellationRequested: true";
	}
}
