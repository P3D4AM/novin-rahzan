using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class ValueConverterSelector : IValueConverterSelector
{
	private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters = new ConcurrentDictionary<(Type, Type), ValueConverterInfo>();

	private static readonly Type[] _signedPreferred = new Type[5]
	{
		typeof(sbyte),
		typeof(short),
		typeof(int),
		typeof(long),
		typeof(decimal)
	};

	private static readonly Type[] _unsignedPreferred = new Type[8]
	{
		typeof(byte),
		typeof(short),
		typeof(ushort),
		typeof(int),
		typeof(uint),
		typeof(long),
		typeof(ulong),
		typeof(decimal)
	};

	private static readonly Type[] _floatingPreferred = new Type[3]
	{
		typeof(float),
		typeof(double),
		typeof(decimal)
	};

	private static readonly Type[] _charPreferred = new Type[7]
	{
		typeof(char),
		typeof(int),
		typeof(ushort),
		typeof(uint),
		typeof(long),
		typeof(ulong),
		typeof(decimal)
	};

	private static readonly Type[] _numerics = new Type[11]
	{
		typeof(int),
		typeof(long),
		typeof(short),
		typeof(byte),
		typeof(ulong),
		typeof(uint),
		typeof(ushort),
		typeof(sbyte),
		typeof(decimal),
		typeof(double),
		typeof(float)
	};

	protected virtual ValueConverterSelectorDependencies Dependencies { get; }

	public ValueConverterSelector([JetBrains.Annotations.NotNull] ValueConverterSelectorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelClrType, "modelClrType");
		if (modelClrType.IsEnum)
		{
			foreach (ValueConverterInfo item in FindNumericConventions(modelClrType, providerClrType, typeof(EnumToNumberConverter<, >), EnumToStringOrBytes))
			{
				yield return item;
			}
		}
		else if (modelClrType == typeof(bool))
		{
			foreach (ValueConverterInfo item2 in FindNumericConventions(typeof(bool), providerClrType, typeof(BoolToZeroOneConverter<>), null))
			{
				yield return item2;
			}
			if (providerClrType == null || providerClrType == typeof(string))
			{
				yield return BoolToStringConverter.DefaultInfo;
			}
			if (!(providerClrType == null) && !(providerClrType == typeof(byte[])))
			{
				yield break;
			}
			yield return _converters.GetOrAdd((modelClrType, typeof(byte[])), ((Type ModelClrType, Type ProviderClrType) k) => new ValueConverterInfo(modelClrType, typeof(byte[]), (ValueConverterInfo info) => new BoolToZeroOneConverter<byte>().ComposeWith(NumberToBytesConverter<byte>.DefaultInfo.Create()), new ConverterMappingHints(1)));
		}
		else if (modelClrType == typeof(char))
		{
			foreach (ValueConverterInfo item3 in ForChar(typeof(char), providerClrType))
			{
				yield return item3;
			}
		}
		else if (modelClrType == typeof(Guid))
		{
			if (providerClrType == null || providerClrType == typeof(byte[]))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(byte[])), ((Type ModelClrType, Type ProviderClrType) k) => GuidToBytesConverter.DefaultInfo);
			}
			if (providerClrType == null || providerClrType == typeof(string))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(string)), ((Type ModelClrType, Type ProviderClrType) k) => GuidToStringConverter.DefaultInfo);
			}
		}
		else if (modelClrType == typeof(byte[]))
		{
			if (providerClrType == null || providerClrType == typeof(string))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(string)), ((Type ModelClrType, Type ProviderClrType) k) => BytesToStringConverter.DefaultInfo);
			}
		}
		else if (modelClrType == typeof(Uri))
		{
			if (providerClrType == null || providerClrType == typeof(string))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(string)), ((Type ModelClrType, Type ProviderClrType) k) => UriToStringConverter.DefaultInfo);
			}
		}
		else if (modelClrType == typeof(string))
		{
			if (providerClrType == null || providerClrType == typeof(byte[]))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(byte[])), ((Type ModelClrType, Type ProviderClrType) k) => StringToBytesConverter.DefaultInfo);
			}
			else if (providerClrType.IsEnum)
			{
				yield return _converters.GetOrAdd((typeof(string), providerClrType), ((Type ModelClrType, Type ProviderClrType) k) => (ValueConverterInfo)typeof(StringToEnumConverter<>).MakeGenericType(k.ProviderClrType).GetAnyProperty("DefaultInfo").GetValue(null));
			}
			else if (_numerics.Contains(providerClrType))
			{
				foreach (ValueConverterInfo item4 in FindNumericConventions(typeof(string), providerClrType, typeof(StringToNumberConverter<>), null))
				{
					yield return item4;
				}
			}
			else if (providerClrType == typeof(DateTime))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(DateTime)), ((Type ModelClrType, Type ProviderClrType) k) => StringToDateTimeConverter.DefaultInfo);
			}
			else if (providerClrType == typeof(DateTimeOffset))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(DateTimeOffset)), ((Type ModelClrType, Type ProviderClrType) k) => StringToDateTimeOffsetConverter.DefaultInfo);
			}
			else if (providerClrType == typeof(TimeSpan))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(TimeSpan)), ((Type ModelClrType, Type ProviderClrType) k) => StringToTimeSpanConverter.DefaultInfo);
			}
			else if (providerClrType == typeof(Guid))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(Guid)), ((Type ModelClrType, Type ProviderClrType) k) => StringToGuidConverter.DefaultInfo);
			}
			else if (providerClrType == typeof(bool))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(bool)), ((Type ModelClrType, Type ProviderClrType) k) => StringToBoolConverter.DefaultInfo);
			}
			else if (providerClrType == typeof(char))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(char)), ((Type ModelClrType, Type ProviderClrType) k) => StringToCharConverter.DefaultInfo);
			}
			else if (providerClrType == typeof(Uri))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(Uri)), ((Type ModelClrType, Type ProviderClrType) k) => StringToUriConverter.DefaultInfo);
			}
		}
		else if (modelClrType == typeof(DateTime) || modelClrType == typeof(DateTimeOffset) || modelClrType == typeof(TimeSpan))
		{
			if (providerClrType == null || providerClrType == typeof(string))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(string)), ((Type ModelClrType, Type ProviderClrType) k) => (!(k.ModelClrType == typeof(DateTime))) ? ((!(k.ModelClrType == typeof(DateTimeOffset))) ? TimeSpanToStringConverter.DefaultInfo : DateTimeOffsetToStringConverter.DefaultInfo) : DateTimeToStringConverter.DefaultInfo);
			}
			if (providerClrType == null || providerClrType == typeof(long))
			{
				yield return _converters.GetOrAdd((modelClrType, typeof(long)), ((Type ModelClrType, Type ProviderClrType) k) => (!(k.ModelClrType == typeof(DateTime))) ? ((!(k.ModelClrType == typeof(DateTimeOffset))) ? TimeSpanToTicksConverter.DefaultInfo : DateTimeOffsetToBinaryConverter.DefaultInfo) : DateTimeToBinaryConverter.DefaultInfo);
			}
			if (!(providerClrType == null) && !(providerClrType == typeof(byte[])))
			{
				yield break;
			}
			yield return (modelClrType == typeof(DateTimeOffset)) ? _converters.GetOrAdd((modelClrType, typeof(byte[])), ((Type ModelClrType, Type ProviderClrType) k) => DateTimeOffsetToBytesConverter.DefaultInfo) : _converters.GetOrAdd((modelClrType, typeof(byte[])), ((Type ModelClrType, Type ProviderClrType) k) => new ValueConverterInfo(modelClrType, typeof(byte[]), (ValueConverterInfo i) => ((i.ModelClrType == typeof(DateTime)) ? DateTimeToBinaryConverter.DefaultInfo.Create() : TimeSpanToTicksConverter.DefaultInfo.Create()).ComposeWith(NumberToBytesConverter<long>.DefaultInfo.Create()), NumberToBytesConverter<long>.DefaultInfo.MappingHints));
		}
		else
		{
			if (!_numerics.Contains(modelClrType) || (!(providerClrType == null) && !(providerClrType == typeof(byte[])) && !(providerClrType == typeof(string)) && !_numerics.Contains(providerClrType)))
			{
				yield break;
			}
			foreach (ValueConverterInfo item5 in FindNumericConventions(modelClrType, providerClrType, typeof(CastingConverter<, >), NumberToStringOrBytes))
			{
				yield return item5;
			}
		}
	}

	private IEnumerable<ValueConverterInfo> ForChar(Type underlyingModelType, Type underlyingProviderType)
	{
		if (underlyingProviderType == null || underlyingProviderType == typeof(string))
		{
			yield return _converters.GetOrAdd((underlyingModelType, typeof(string)), ((Type ModelClrType, Type ProviderClrType) k) => CharToStringConverter.DefaultInfo);
		}
		foreach (ValueConverterInfo item in FindNumericConventions(underlyingModelType, underlyingProviderType, typeof(CastingConverter<, >), CharToBytes))
		{
			yield return item;
		}
	}

	private IEnumerable<ValueConverterInfo> CharToBytes(Type underlyingModelType, Type underlyingProviderType)
	{
		if (underlyingProviderType == null || underlyingProviderType == typeof(byte[]))
		{
			yield return _converters.GetOrAdd((underlyingModelType, typeof(byte[])), ((Type ModelClrType, Type ProviderClrType) k) => NumberToBytesConverter<char>.DefaultInfo);
		}
	}

	private IEnumerable<ValueConverterInfo> EnumToStringOrBytes(Type underlyingModelType, Type underlyingProviderType)
	{
		if (underlyingProviderType == null || underlyingProviderType == typeof(string))
		{
			yield return _converters.GetOrAdd((underlyingModelType, typeof(string)), ((Type ModelClrType, Type ProviderClrType) k) => (ValueConverterInfo)typeof(EnumToStringConverter<>).MakeGenericType(k.ModelClrType).GetAnyProperty("DefaultInfo").GetValue(null));
		}
		if (!(underlyingProviderType == null) && !(underlyingProviderType == typeof(byte[])))
		{
			yield break;
		}
		yield return _converters.GetOrAdd((underlyingModelType, typeof(byte[])), delegate((Type ModelClrType, Type ProviderClrType) k)
		{
			ValueConverterInfo toNumber = (ValueConverterInfo)typeof(EnumToNumberConverter<, >).MakeGenericType(k.ModelClrType, k.ModelClrType.GetEnumUnderlyingType()).GetAnyProperty("DefaultInfo").GetValue(null);
			ValueConverterInfo toBytes = (ValueConverterInfo)typeof(NumberToBytesConverter<>).MakeGenericType(k.ModelClrType.GetEnumUnderlyingType()).GetAnyProperty("DefaultInfo").GetValue(null);
			return new ValueConverterInfo(underlyingModelType, typeof(byte[]), (ValueConverterInfo i) => toNumber.Create().ComposeWith(toBytes.Create()), toBytes.MappingHints);
		});
	}

	private IEnumerable<ValueConverterInfo> NumberToStringOrBytes(Type underlyingModelType, Type underlyingProviderType)
	{
		if (underlyingProviderType == null || underlyingProviderType == typeof(string))
		{
			yield return _converters.GetOrAdd((underlyingModelType, typeof(string)), ((Type ModelClrType, Type ProviderClrType) k) => (ValueConverterInfo)typeof(NumberToStringConverter<>).MakeGenericType(k.ModelClrType).GetAnyProperty("DefaultInfo").GetValue(null));
		}
		if (underlyingProviderType == null || underlyingProviderType == typeof(byte[]))
		{
			yield return _converters.GetOrAdd((underlyingModelType, typeof(byte[])), ((Type ModelClrType, Type ProviderClrType) k) => (ValueConverterInfo)typeof(NumberToBytesConverter<>).MakeGenericType(k.ModelClrType).GetAnyProperty("DefaultInfo").GetValue(null));
		}
	}

	private IEnumerable<ValueConverterInfo> FindNumericConventions(Type modelType, Type providerType, Type converterType, Func<Type, Type, IEnumerable<ValueConverterInfo>> afterPreferred)
	{
		List<Type> usedTypes = new List<Type> { modelType };
		Type underlyingModelType = modelType.UnwrapEnumType();
		if (modelType.IsEnum)
		{
			foreach (ValueConverterInfo converterInfo in FindPreferredConversions(new Type[1] { underlyingModelType }, modelType, providerType, converterType))
			{
				yield return converterInfo;
				usedTypes.Add(converterInfo.ProviderClrType);
			}
		}
		foreach (ValueConverterInfo converterInfo in FindPreferredConversions(_signedPreferred, modelType, providerType, converterType))
		{
			if (!usedTypes.Contains(converterInfo.ProviderClrType))
			{
				yield return converterInfo;
				usedTypes.Add(converterInfo.ProviderClrType);
			}
		}
		if (underlyingModelType == typeof(byte) || underlyingModelType == typeof(uint) || underlyingModelType == typeof(ulong) || underlyingModelType == typeof(ushort))
		{
			foreach (ValueConverterInfo converterInfo in FindPreferredConversions(_unsignedPreferred, modelType, providerType, converterType))
			{
				if (!usedTypes.Contains(converterInfo.ProviderClrType))
				{
					yield return converterInfo;
					usedTypes.Add(converterInfo.ProviderClrType);
				}
			}
		}
		if (underlyingModelType == typeof(float) || underlyingModelType == typeof(double))
		{
			foreach (ValueConverterInfo converterInfo in FindPreferredConversions(_floatingPreferred, modelType, providerType, converterType))
			{
				yield return converterInfo;
				usedTypes.Add(converterInfo.ProviderClrType);
			}
		}
		if (underlyingModelType == typeof(char))
		{
			foreach (ValueConverterInfo converterInfo in FindPreferredConversions(_charPreferred, modelType, providerType, converterType))
			{
				yield return converterInfo;
				usedTypes.Add(converterInfo.ProviderClrType);
			}
		}
		if (afterPreferred != null)
		{
			foreach (ValueConverterInfo converterInfo in afterPreferred(modelType, providerType))
			{
				yield return converterInfo;
				usedTypes.Add(converterInfo.ProviderClrType);
			}
		}
		Type[] numerics = _numerics;
		foreach (Type type in numerics)
		{
			if ((providerType == null || providerType == type) && !usedTypes.Contains(type))
			{
				yield return _converters.GetOrAdd((modelType, type), ((Type ModelClrType, Type ProviderClrType) k) => (ValueConverterInfo)((converterType.GetTypeInfo().GenericTypeParameters.Length == 1) ? converterType.MakeGenericType(k.ProviderClrType) : converterType.MakeGenericType(k.ModelClrType, k.ProviderClrType)).GetAnyProperty("DefaultInfo").GetValue(null));
			}
		}
	}

	private IEnumerable<ValueConverterInfo> FindPreferredConversions(Type[] candidateTypes, Type modelType, Type providerType, Type converterType)
	{
		Type underlyingModelType = modelType.UnwrapEnumType();
		for (int i = 0; i < candidateTypes.Length; i++)
		{
			if (!(underlyingModelType == candidateTypes[i]))
			{
				continue;
			}
			if (!modelType.IsEnum)
			{
				i++;
			}
			for (; i < candidateTypes.Length; i++)
			{
				if (providerType == null || providerType == candidateTypes[i])
				{
					yield return _converters.GetOrAdd((modelType, candidateTypes[i]), ((Type ModelClrType, Type ProviderClrType) k) => (ValueConverterInfo)converterType.MakeGenericType(k.ModelClrType, k.ProviderClrType).GetAnyProperty("DefaultInfo").GetValue(null));
				}
			}
		}
	}
}
