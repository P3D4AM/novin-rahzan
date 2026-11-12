using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public readonly struct TypeMappingInfo : IEquatable<TypeMappingInfo>
{
	public bool IsKeyOrIndex { get; }

	public int? Size { get; }

	public bool? IsUnicode { get; }

	public bool? IsRowVersion { get; }

	public int? Precision { get; }

	public int? Scale { get; }

	public Type ClrType { get; }

	public TypeMappingInfo([JetBrains.Annotations.NotNull] IProperty property)
		: this(property.FindPrincipals())
	{
	}

	public TypeMappingInfo([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> principals, bool? fallbackUnicode = null, int? fallbackSize = null, int? fallbackPrecision = null, int? fallbackScale = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principals, "principals");
		ValueConverter valueConverter = null;
		int? num = null;
		bool? flag = null;
		for (int i = 0; i < principals.Count; i++)
		{
			IProperty property = principals[i];
			if (valueConverter == null)
			{
				ValueConverter valueConverter2 = property.GetValueConverter();
				if (valueConverter2 != null)
				{
					valueConverter = valueConverter2;
				}
			}
			if (!num.HasValue)
			{
				int? maxLength = property.GetMaxLength();
				if (maxLength.HasValue)
				{
					num = maxLength;
				}
			}
			if (!flag.HasValue)
			{
				bool? flag2 = property.IsUnicode();
				if (flag2.HasValue)
				{
					flag = flag2;
				}
			}
		}
		ConverterMappingHints converterMappingHints = valueConverter?.MappingHints;
		IProperty property2 = principals[0];
		IsKeyOrIndex = property2.IsKeyOrForeignKey() || property2.IsIndex();
		Size = num ?? converterMappingHints?.Size ?? fallbackSize;
		IsUnicode = flag ?? converterMappingHints?.IsUnicode ?? fallbackUnicode;
		IsRowVersion = property2.IsConcurrencyToken && property2.ValueGenerated == ValueGenerated.OnAddOrUpdate;
		ClrType = (valueConverter?.ProviderClrType ?? property2.ClrType).UnwrapNullableType();
		Scale = converterMappingHints?.Scale ?? fallbackScale;
		Precision = converterMappingHints?.Precision ?? fallbackPrecision;
	}

	public TypeMappingInfo([JetBrains.Annotations.NotNull] MemberInfo member, bool? unicode = null, int? size = null, int? precision = null, int? scale = null)
		: this(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(member, "member").GetMemberType())
	{
		IsUnicode = unicode;
		Size = size;
		Precision = precision;
		Scale = scale;
	}

	public TypeMappingInfo([JetBrains.Annotations.CanBeNull] Type type = null, bool keyOrIndex = false, bool? unicode = null, int? size = null, bool? rowVersion = null, int? precision = null, int? scale = null)
	{
		ClrType = type?.UnwrapNullableType();
		IsKeyOrIndex = keyOrIndex;
		Size = size;
		IsUnicode = unicode;
		IsRowVersion = rowVersion;
		Precision = precision;
		Scale = scale;
	}

	public TypeMappingInfo(TypeMappingInfo source, ValueConverterInfo converter, bool? unicode = null, int? size = null, int? precision = null, int? scale = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		IsRowVersion = source.IsRowVersion;
		IsKeyOrIndex = source.IsKeyOrIndex;
		ConverterMappingHints mappingHints = converter.MappingHints;
		Size = size ?? source.Size ?? mappingHints?.Size;
		IsUnicode = unicode ?? source.IsUnicode ?? mappingHints?.IsUnicode;
		Scale = scale ?? source.Scale ?? mappingHints?.Scale;
		Precision = precision ?? source.Precision ?? mappingHints?.Precision;
		ClrType = converter.ProviderClrType.UnwrapNullableType();
	}

	public TypeMappingInfo WithConverter(in ValueConverterInfo converterInfo)
	{
		return new TypeMappingInfo(this, converterInfo);
	}

	public bool Equals(TypeMappingInfo other)
	{
		if (ClrType == other.ClrType && IsKeyOrIndex == other.IsKeyOrIndex && Size == other.Size && IsUnicode == other.IsUnicode && IsRowVersion == other.IsRowVersion && Precision == other.Precision)
		{
			return Scale == other.Scale;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj != null && obj.GetType() == GetType())
		{
			return Equals((TypeMappingInfo)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(ClrType, IsKeyOrIndex, Size, IsUnicode, IsRowVersion, Scale, Precision);
	}
}
