using System;
using System.Collections.Generic;
using System.Reflection;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class ReflectionTypeLoadExceptionDestructurer : ExceptionDestructurer
{
	public override Type[] TargetTypes => new Type[1] { typeof(ReflectionTypeLoadException) };

	public override void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		base.Destructure(exception, propertiesBag, destructureException);
		ReflectionTypeLoadException ex = (ReflectionTypeLoadException)exception;
		if (ex.LoaderExceptions != null)
		{
			propertiesBag.AddProperty("LoaderExceptions", GetLoaderExceptionsValue(ex.LoaderExceptions, destructureException));
		}
	}

	private static List<IReadOnlyDictionary<string, object?>> GetLoaderExceptionsValue(Exception?[] exceptions, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		List<IReadOnlyDictionary<string, object>> list = new List<IReadOnlyDictionary<string, object>>();
		foreach (Exception ex in exceptions)
		{
			if (ex != null)
			{
				IReadOnlyDictionary<string, object> readOnlyDictionary = destructureException(ex);
				if (readOnlyDictionary != null)
				{
					list.Add(readOnlyDictionary);
				}
			}
		}
		return list;
	}
}
