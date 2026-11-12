using System;
using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions.Destructurers;

namespace Serilog.Exceptions.Core;

public sealed class ExceptionEnricher : ILogEventEnricher
{
	private readonly IExceptionDestructurer reflectionBasedDestructurer;

	private readonly Dictionary<Type, IExceptionDestructurer> destructurers;

	private readonly IDestructuringOptions destructuringOptions;

	public ExceptionEnricher()
		: this(new DestructuringOptionsBuilder().WithDefaultDestructurers())
	{
	}

	public ExceptionEnricher(IDestructuringOptions destructuringOptions)
	{
		this.destructuringOptions = destructuringOptions ?? throw new ArgumentNullException("destructuringOptions");
		reflectionBasedDestructurer = new ReflectionBasedDestructurer(destructuringOptions.DestructuringDepth);
		destructurers = new Dictionary<Type, IExceptionDestructurer>();
		foreach (IExceptionDestructurer destructurer in this.destructuringOptions.Destructurers)
		{
			Type[] targetTypes = destructurer.TargetTypes;
			foreach (Type key in targetTypes)
			{
				destructurers.Add(key, destructurer);
			}
		}
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		if (propertyFactory == null)
		{
			throw new ArgumentNullException("propertyFactory");
		}
		if (logEvent.Exception != null)
		{
			IReadOnlyDictionary<string, object> readOnlyDictionary = DestructureException(logEvent.Exception);
			if (readOnlyDictionary != null)
			{
				logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(destructuringOptions.RootName, readOnlyDictionary, destructureObjects: true));
			}
		}
	}

	private IReadOnlyDictionary<string, object?>? DestructureException(Exception exception)
	{
		Type type = exception.GetType();
		if (destructurers.TryGetValue(type, out IExceptionDestructurer value))
		{
			ExceptionPropertiesBag exceptionPropertiesBag = new ExceptionPropertiesBag(exception, destructuringOptions.Filter);
			value.Destructure(exception, exceptionPropertiesBag, DestructureException);
			return exceptionPropertiesBag.GetResultDictionary();
		}
		if (!destructuringOptions.DisableReflectionBasedDestructurer)
		{
			ExceptionPropertiesBag exceptionPropertiesBag2 = new ExceptionPropertiesBag(exception, destructuringOptions.Filter);
			reflectionBasedDestructurer.Destructure(exception, exceptionPropertiesBag2, DestructureException);
			return exceptionPropertiesBag2.GetResultDictionary();
		}
		return null;
	}
}
