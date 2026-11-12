using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Policies;

namespace Serilog.Capturing;

internal class PropertyValueConverter : ILogEventPropertyFactory, ILogEventPropertyValueFactory
{
	private class DepthLimiter : ILogEventPropertyValueFactory
	{
		[ThreadStatic]
		private static int _currentDepth;

		private readonly int _maximumDestructuringDepth;

		private readonly PropertyValueConverter _propertyValueConverter;

		public DepthLimiter(int maximumDepth, PropertyValueConverter propertyValueConverter)
		{
			_maximumDestructuringDepth = maximumDepth;
			_propertyValueConverter = propertyValueConverter;
		}

		public static void SetCurrentDepth(int depth)
		{
			_currentDepth = depth;
		}

		public LogEventPropertyValue CreatePropertyValue(object? value, Destructuring destructuring)
		{
			int currentDepth = _currentDepth;
			LogEventPropertyValue? result = DefaultIfMaximumDepth(currentDepth) ?? _propertyValueConverter.CreatePropertyValue(value, destructuring, currentDepth + 1);
			_currentDepth = currentDepth;
			return result;
		}

		LogEventPropertyValue ILogEventPropertyValueFactory.CreatePropertyValue(object? value, bool destructureObjects)
		{
			int currentDepth = _currentDepth;
			LogEventPropertyValue? result = DefaultIfMaximumDepth(currentDepth) ?? _propertyValueConverter.CreatePropertyValue(value, destructureObjects, currentDepth + 1);
			_currentDepth = currentDepth;
			return result;
		}

		private LogEventPropertyValue? DefaultIfMaximumDepth(int depth)
		{
			if (depth == _maximumDestructuringDepth)
			{
				SelfLog.WriteLine("Maximum destructuring depth reached.");
				return ScalarValue.Null;
			}
			return null;
		}
	}

	private static readonly HashSet<Type> BuiltInScalarTypes = new HashSet<Type>
	{
		typeof(decimal),
		typeof(string),
		typeof(DateTime),
		typeof(DateTimeOffset),
		typeof(TimeSpan),
		typeof(Guid),
		typeof(Uri)
	};

	private readonly IDestructuringPolicy[] _destructuringPolicies;

	private readonly Type[] _dictionaryTypes;

	private readonly IScalarConversionPolicy[] _scalarConversionPolicies;

	private readonly DepthLimiter _depthLimiter;

	private readonly int _maximumStringLength;

	private readonly int _maximumCollectionCount;

	private readonly bool _propagateExceptions;

	public PropertyValueConverter(int maximumDestructuringDepth, int maximumStringLength, int maximumCollectionCount, IEnumerable<Type> additionalScalarTypes, IEnumerable<Type> additionalDictionaryTypes, IEnumerable<IDestructuringPolicy> additionalDestructuringPolicies, bool propagateExceptions)
	{
		Guard.AgainstNull(additionalScalarTypes, "additionalScalarTypes");
		Guard.AgainstNull(additionalDestructuringPolicies, "additionalDestructuringPolicies");
		if (maximumDestructuringDepth < 0)
		{
			throw new ArgumentOutOfRangeException("maximumDestructuringDepth");
		}
		if (maximumStringLength < 2)
		{
			throw new ArgumentOutOfRangeException("maximumStringLength");
		}
		if (maximumCollectionCount < 1)
		{
			throw new ArgumentOutOfRangeException("maximumCollectionCount");
		}
		_propagateExceptions = propagateExceptions;
		_maximumStringLength = maximumStringLength;
		_maximumCollectionCount = maximumCollectionCount;
		_scalarConversionPolicies = new IScalarConversionPolicy[4]
		{
			new PrimitiveScalarConversionPolicy(),
			new SimpleScalarConversionPolicy(BuiltInScalarTypes.Concat(additionalScalarTypes)),
			new EnumScalarConversionPolicy(),
			new ByteArrayScalarConversionPolicy()
		};
		_destructuringPolicies = additionalDestructuringPolicies.Concat(new IDestructuringPolicy[2]
		{
			new DelegateDestructuringPolicy(),
			new ReflectionTypesScalarDestructuringPolicy()
		}).ToArray();
		_dictionaryTypes = additionalDictionaryTypes.ToArray();
		_depthLimiter = new DepthLimiter(maximumDestructuringDepth, this);
	}

	public LogEventProperty CreateProperty(string name, object? value, bool destructureObjects = false)
	{
		return new LogEventProperty(name, CreatePropertyValue(value, destructureObjects));
	}

	public LogEventPropertyValue CreatePropertyValue(object? value, bool destructureObjects = false)
	{
		return CreatePropertyValue(value, destructureObjects, 1);
	}

	public LogEventPropertyValue CreatePropertyValue(object? value, Destructuring destructuring)
	{
		try
		{
			return CreatePropertyValue(value, destructuring, 1);
		}
		catch (Exception ex)
		{
			SelfLog.WriteLine("Exception caught while converting property value: {0}", ex);
			if (_propagateExceptions)
			{
				throw;
			}
			return new ScalarValue("Capturing the property value threw an exception: " + ex.GetType().Name);
		}
	}

	private LogEventPropertyValue CreatePropertyValue(object? value, bool destructureObjects, int depth)
	{
		return CreatePropertyValue(value, destructureObjects ? Destructuring.Destructure : Destructuring.Default, depth);
	}

	private LogEventPropertyValue CreatePropertyValue(object? value, Destructuring destructuring, int depth)
	{
		if (value == null)
		{
			return ScalarValue.Null;
		}
		switch (destructuring)
		{
		case Destructuring.Stringify:
			return Stringify(value);
		case Destructuring.Destructure:
			if (value is string text)
			{
				value = TruncateIfNecessary(text);
			}
			break;
		}
		if (value is string)
		{
			return new ScalarValue(value);
		}
		IScalarConversionPolicy[] scalarConversionPolicies = _scalarConversionPolicies;
		for (int i = 0; i < scalarConversionPolicies.Length; i++)
		{
			if (scalarConversionPolicies[i].TryConvertToScalar(value, out ScalarValue result))
			{
				return result;
			}
		}
		DepthLimiter.SetCurrentDepth(depth);
		if (destructuring == Destructuring.Destructure)
		{
			IDestructuringPolicy[] destructuringPolicies = _destructuringPolicies;
			for (int i = 0; i < destructuringPolicies.Length; i++)
			{
				if (destructuringPolicies[i].TryDestructure(value, _depthLimiter, out LogEventPropertyValue result2))
				{
					return result2;
				}
			}
		}
		Type type = value.GetType();
		if (TryConvertEnumerable(value, type, destructuring, out LogEventPropertyValue result3))
		{
			return result3;
		}
		if (TryConvertValueTuple(value, type, destructuring, out LogEventPropertyValue result4))
		{
			return result4;
		}
		if (TryConvertCompilerGeneratedType(value, type, destructuring, out LogEventPropertyValue result5))
		{
			return result5;
		}
		return new ScalarValue(value.ToString() ?? "");
	}

	private bool TryConvertEnumerable(object value, Type type, Destructuring destructuring, [NotNullWhen(true)] out LogEventPropertyValue? result)
	{
		if (value is IEnumerable enumerable)
		{
			if (TryGetDictionary(value, type, out IDictionary dictionary))
			{
				result = new DictionaryValue(MapToDictionaryElements(dictionary, destructuring));
				return true;
			}
			if (enumerable is IList list && list.Count <= _maximumCollectionCount)
			{
				if (list.Count == 0)
				{
					result = SequenceValue.Empty;
				}
				else
				{
					LogEventPropertyValue[] array = new LogEventPropertyValue[list.Count];
					for (int i = 0; i < list.Count; i++)
					{
						array[i] = _depthLimiter.CreatePropertyValue(list[i], destructuring);
					}
					result = new SequenceValue(array);
				}
			}
			else
			{
				result = new SequenceValue(MapToSequenceElements(enumerable, destructuring));
			}
			return true;
		}
		result = null;
		return false;
		IEnumerable<KeyValuePair<ScalarValue, LogEventPropertyValue>> MapToDictionaryElements(IDictionary dictionaryEntries, Destructuring destructure)
		{
			int count2 = 0;
			foreach (DictionaryEntry dictionaryEntry in dictionaryEntries)
			{
				int num2 = count2 + 1;
				count2 = num2;
				if (num2 > _maximumCollectionCount)
				{
					yield break;
				}
				KeyValuePair<ScalarValue, LogEventPropertyValue> keyValuePair = new KeyValuePair<ScalarValue, LogEventPropertyValue>((ScalarValue)_depthLimiter.CreatePropertyValue(dictionaryEntry.Key, destructure), _depthLimiter.CreatePropertyValue(dictionaryEntry.Value, destructure));
				if (keyValuePair.Key.Value != null)
				{
					yield return keyValuePair;
				}
			}
		}
		IEnumerable<LogEventPropertyValue> MapToSequenceElements(IEnumerable sequence, Destructuring destructure)
		{
			int count = 0;
			foreach (object item in sequence)
			{
				int num = count + 1;
				count = num;
				if (num > _maximumCollectionCount)
				{
					yield break;
				}
				yield return _depthLimiter.CreatePropertyValue(item, destructure);
			}
		}
	}

	private bool TryConvertValueTuple(object value, Type type, Destructuring destructuring, [NotNullWhen(true)] out LogEventPropertyValue? result)
	{
		if (!(value is ITuple tuple))
		{
			result = null;
			return false;
		}
		LogEventPropertyValue[] array = new LogEventPropertyValue[tuple.Length];
		for (int i = 0; i < tuple.Length; i++)
		{
			object value2 = tuple[i];
			array[i] = _depthLimiter.CreatePropertyValue(value2, destructuring);
		}
		result = new SequenceValue(array);
		return true;
	}

	private bool TryConvertCompilerGeneratedType(object value, Type type, Destructuring destructuring, [NotNullWhen(true)] out LogEventPropertyValue? result)
	{
		if (destructuring == Destructuring.Destructure)
		{
			string text = type.Name;
			if (text.Length <= 0 || IsCompilerGeneratedType(type))
			{
				text = null;
			}
			result = new StructureValue(GetProperties(value, type), text);
			return true;
		}
		result = null;
		return false;
	}

	private LogEventPropertyValue Stringify(object value)
	{
		string text = value.ToString();
		return new ScalarValue((text == null) ? "" : TruncateIfNecessary(text));
	}

	private string TruncateIfNecessary(string text)
	{
		if (text.Length > _maximumStringLength)
		{
			return text.Substring(0, _maximumStringLength - 1) + "…";
		}
		return text;
	}

	private bool TryGetDictionary(object value, Type valueType, [NotNullWhen(true)] out IDictionary? dictionary)
	{
		if (value is IDictionary dictionary2)
		{
			if (_dictionaryTypes.Contains(valueType))
			{
				dictionary = dictionary2;
				return true;
			}
			if (valueType.IsConstructedGenericType)
			{
				Type genericTypeDefinition = valueType.GetGenericTypeDefinition();
				if ((genericTypeDefinition == typeof(Dictionary<, >) || genericTypeDefinition == typeof(ReadOnlyDictionary<, >)) && IsValidDictionaryKeyType(valueType.GenericTypeArguments[0]))
				{
					dictionary = dictionary2;
					return true;
				}
			}
		}
		dictionary = null;
		return false;
	}

	private static bool IsValidDictionaryKeyType(Type valueType)
	{
		if (!valueType.IsPrimitive && !BuiltInScalarTypes.Contains(valueType))
		{
			return valueType.IsEnum;
		}
		return true;
	}

	private IEnumerable<LogEventProperty> GetProperties(object value, Type type)
	{
		foreach (PropertyInfo item in type.GetPropertiesRecursive())
		{
			object value2;
			try
			{
				value2 = item.GetValue(value);
			}
			catch (TargetParameterCountException)
			{
				SelfLog.WriteLine("The property accessor {0} is a non-default indexer", item);
				continue;
			}
			catch (TargetInvocationException ex2)
			{
				SelfLog.WriteLine("The property accessor {0} threw exception: {1}", item, ex2);
				if (_propagateExceptions)
				{
					throw;
				}
				value2 = "The property accessor threw an exception: " + ex2.InnerException?.GetType().Name;
			}
			catch (NotSupportedException)
			{
				SelfLog.WriteLine("The property accessor {0} is not supported via Reflection API", item);
				if (_propagateExceptions)
				{
					throw;
				}
				value2 = "Accessing this property is not supported via Reflection API";
			}
			yield return new LogEventProperty(item.Name, _depthLimiter.CreatePropertyValue(value2, Destructuring.Destructure));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsCompilerGeneratedType(Type type)
	{
		if (!type.IsGenericType || !type.IsSealed || type.Namespace != null)
		{
			return false;
		}
		string name = type.Name;
		if (name[0] != '<')
		{
			if (name.Length > 2 && name[0] == 'V' && name[1] == 'B')
			{
				return name[2] == '$';
			}
			return false;
		}
		return true;
	}
}
