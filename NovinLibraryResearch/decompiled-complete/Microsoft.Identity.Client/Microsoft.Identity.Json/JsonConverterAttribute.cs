using System;

namespace Microsoft.Identity.Json;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
internal sealed class JsonConverterAttribute : Attribute
{
	private readonly Type _converterType;

	public Type ConverterType => _converterType;

	public object[] ConverterParameters { get; }

	public JsonConverterAttribute(Type converterType)
	{
		_converterType = converterType ?? throw new ArgumentNullException("converterType");
	}

	public JsonConverterAttribute(Type converterType, params object[] converterParameters)
		: this(converterType)
	{
		ConverterParameters = converterParameters;
	}
}
