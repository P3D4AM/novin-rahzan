using System;
using System.Collections;
using Serilog.Core;
using Serilog.Policies;

namespace Serilog.Configuration;

public class LoggerDestructuringConfiguration
{
	private readonly LoggerConfiguration _loggerConfiguration;

	private readonly Action<Type> _addScalar;

	private readonly Action<Type> _addDictionaryType;

	private readonly Action<IDestructuringPolicy> _addPolicy;

	private readonly Action<int> _setMaximumDepth;

	private readonly Action<int> _setMaximumStringLength;

	private readonly Action<int> _setMaximumCollectionCount;

	internal LoggerDestructuringConfiguration(LoggerConfiguration loggerConfiguration, Action<Type> addScalar, Action<Type> addDictionaryType, Action<IDestructuringPolicy> addPolicy, Action<int> setMaximumDepth, Action<int> setMaximumStringLength, Action<int> setMaximumCollectionCount)
	{
		_loggerConfiguration = Guard.AgainstNull(loggerConfiguration, "loggerConfiguration");
		_addScalar = Guard.AgainstNull(addScalar, "addScalar");
		_addDictionaryType = Guard.AgainstNull(addDictionaryType, "addDictionaryType");
		_addPolicy = Guard.AgainstNull(addPolicy, "addPolicy");
		_setMaximumDepth = Guard.AgainstNull(setMaximumDepth, "setMaximumDepth");
		_setMaximumStringLength = Guard.AgainstNull(setMaximumStringLength, "setMaximumStringLength");
		_setMaximumCollectionCount = Guard.AgainstNull(setMaximumCollectionCount, "setMaximumCollectionCount");
	}

	public LoggerConfiguration AsScalar(Type scalarType)
	{
		Guard.AgainstNull(scalarType, "scalarType");
		_addScalar(scalarType);
		return _loggerConfiguration;
	}

	public LoggerConfiguration AsScalar<TScalar>()
	{
		return AsScalar(typeof(TScalar));
	}

	public LoggerConfiguration With(params IDestructuringPolicy[] destructuringPolicies)
	{
		Guard.AgainstNull(destructuringPolicies, "destructuringPolicies");
		foreach (IDestructuringPolicy destructuringPolicy in destructuringPolicies)
		{
			if (destructuringPolicy == null)
			{
				throw new ArgumentException("Null policy is not allowed.");
			}
			_addPolicy(destructuringPolicy);
		}
		return _loggerConfiguration;
	}

	public LoggerConfiguration With<TDestructuringPolicy>() where TDestructuringPolicy : IDestructuringPolicy, new()
	{
		return With(new TDestructuringPolicy());
	}

	public LoggerConfiguration AsDictionary<T>() where T : IDictionary
	{
		_addDictionaryType(typeof(T));
		return _loggerConfiguration;
	}

	public LoggerConfiguration ByTransforming<TValue>(Func<TValue, object> transformation)
	{
		Func<TValue, object> transformation2 = transformation;
		Guard.AgainstNull(transformation2, "transformation");
		ProjectedDestructuringPolicy projectedDestructuringPolicy = new ProjectedDestructuringPolicy((Type t) => t == typeof(TValue), (object o) => transformation2((TValue)o));
		return With(projectedDestructuringPolicy);
	}

	public LoggerConfiguration ByTransformingWhere<TValue>(Func<Type, bool> predicate, Func<TValue, object> transformation)
	{
		Func<TValue, object> transformation2 = transformation;
		Guard.AgainstNull(predicate, "predicate");
		Guard.AgainstNull(transformation2, "transformation");
		ProjectedDestructuringPolicy projectedDestructuringPolicy = new ProjectedDestructuringPolicy(predicate, (object o) => transformation2((TValue)o));
		return With(projectedDestructuringPolicy);
	}

	public LoggerConfiguration ToMaximumDepth(int maximumDestructuringDepth)
	{
		if (maximumDestructuringDepth < 0)
		{
			throw new ArgumentOutOfRangeException("maximumDestructuringDepth", "Maximum destructuring depth must be positive.");
		}
		_setMaximumDepth(maximumDestructuringDepth);
		return _loggerConfiguration;
	}

	public LoggerConfiguration ToMaximumStringLength(int maximumStringLength)
	{
		if (maximumStringLength < 2)
		{
			throw new ArgumentOutOfRangeException("maximumStringLength", maximumStringLength, "Maximum string length must be at least two.");
		}
		_setMaximumStringLength(maximumStringLength);
		return _loggerConfiguration;
	}

	public LoggerConfiguration ToMaximumCollectionCount(int maximumCollectionCount)
	{
		if (maximumCollectionCount < 1)
		{
			throw new ArgumentOutOfRangeException("maximumCollectionCount", maximumCollectionCount, "Maximum collection length must be at least one.");
		}
		_setMaximumCollectionCount(maximumCollectionCount);
		return _loggerConfiguration;
	}
}
