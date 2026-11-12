using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public readonly struct ValueConverterInfo
{
	private readonly Func<ValueConverterInfo, ValueConverter> _factory;

	public Type ModelClrType { get; }

	public Type ProviderClrType { get; }

	public ConverterMappingHints MappingHints { get; }

	public ValueConverterInfo([JetBrains.Annotations.NotNull] Type modelClrType, [JetBrains.Annotations.NotNull] Type providerClrType, [JetBrains.Annotations.NotNull] Func<ValueConverterInfo, ValueConverter> factory, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
	{
		_factory = factory;
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelClrType, "modelClrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(providerClrType, "providerClrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factory, "factory");
		ModelClrType = modelClrType;
		ProviderClrType = providerClrType;
		MappingHints = mappingHints;
	}

	public ValueConverter Create()
	{
		return _factory(this);
	}
}
