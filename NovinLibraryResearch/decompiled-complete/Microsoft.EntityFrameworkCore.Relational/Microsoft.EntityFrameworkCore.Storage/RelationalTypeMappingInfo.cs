using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public readonly struct RelationalTypeMappingInfo : IEquatable<RelationalTypeMappingInfo>
{
	private readonly TypeMappingInfo _coreTypeMappingInfo;

	public string StoreTypeName { get; }

	public string StoreTypeNameBase { get; }

	public int? Size => _coreTypeMappingInfo.Size;

	public int? Precision => _coreTypeMappingInfo.Precision;

	public int? Scale => _coreTypeMappingInfo.Scale;

	public bool? IsFixedLength { get; }

	public bool IsKeyOrIndex => _coreTypeMappingInfo.IsKeyOrIndex;

	public bool? IsUnicode => _coreTypeMappingInfo.IsUnicode;

	public bool? IsRowVersion => _coreTypeMappingInfo.IsRowVersion;

	public Type ClrType => _coreTypeMappingInfo.ClrType;

	public RelationalTypeMappingInfo([JetBrains.Annotations.NotNull] IProperty property)
		: this(property.FindPrincipals())
	{
	}

	public RelationalTypeMappingInfo([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> principals, [JetBrains.Annotations.CanBeNull] string storeTypeName = null, [JetBrains.Annotations.CanBeNull] string storeTypeNameBase = null, bool? fallbackUnicode = null, bool? fixedLength = null, int? fallbackSize = null, int? fallbackPrecision = null, int? fallbackScale = null)
	{
		_coreTypeMappingInfo = new TypeMappingInfo(principals, fallbackUnicode, fallbackSize, fallbackPrecision, fallbackScale);
		IsFixedLength = fixedLength;
		StoreTypeName = storeTypeName;
		StoreTypeNameBase = storeTypeNameBase;
	}

	public RelationalTypeMappingInfo([JetBrains.Annotations.NotNull] string storeTypeName, [JetBrains.Annotations.NotNull] string storeTypeNameBase, bool? unicode, int? size, int? precision, int? scale)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(storeTypeName, "storeTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(storeTypeNameBase, "storeTypeNameBase");
		_coreTypeMappingInfo = new TypeMappingInfo(null, keyOrIndex: false, unicode, size, null, precision, scale);
		StoreTypeName = storeTypeName;
		StoreTypeNameBase = storeTypeNameBase;
		IsFixedLength = null;
	}

	public RelationalTypeMappingInfo([JetBrains.Annotations.NotNull] MemberInfo member, [JetBrains.Annotations.CanBeNull] string storeTypeName = null, [JetBrains.Annotations.CanBeNull] string storeTypeNameBase = null, bool? unicode = null, int? size = null, int? precision = null, int? scale = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(member, "member");
		_coreTypeMappingInfo = new TypeMappingInfo(member, unicode, size, precision, scale);
		StoreTypeName = storeTypeName;
		StoreTypeNameBase = storeTypeNameBase;
		IsFixedLength = null;
	}

	public RelationalTypeMappingInfo(in RelationalTypeMappingInfo source, in ValueConverterInfo converter)
	{
		_coreTypeMappingInfo = new TypeMappingInfo(source._coreTypeMappingInfo, converter, source.IsUnicode, source.Size, source.Precision, source.Scale);
		ConverterMappingHints mappingHints = converter.MappingHints;
		StoreTypeName = source.StoreTypeName;
		StoreTypeNameBase = source.StoreTypeNameBase;
		IsFixedLength = source.IsFixedLength ?? (mappingHints as RelationalConverterMappingHints)?.IsFixedLength;
	}

	public RelationalTypeMappingInfo([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.CanBeNull] string storeTypeName = null, [JetBrains.Annotations.CanBeNull] string storeTypeNameBase = null, bool keyOrIndex = false, bool? unicode = null, int? size = null, bool? rowVersion = null, bool? fixedLength = null, int? precision = null, int? scale = null)
	{
		_coreTypeMappingInfo = new TypeMappingInfo(type, keyOrIndex, unicode, size, rowVersion, precision, scale);
		IsFixedLength = fixedLength;
		StoreTypeName = storeTypeName;
		StoreTypeNameBase = storeTypeNameBase;
	}

	public RelationalTypeMappingInfo WithConverter(in ValueConverterInfo converterInfo)
	{
		return new RelationalTypeMappingInfo(in this, in converterInfo);
	}

	public bool Equals(RelationalTypeMappingInfo other)
	{
		if (_coreTypeMappingInfo.Equals(other._coreTypeMappingInfo) && IsFixedLength == other.IsFixedLength)
		{
			return StoreTypeName == other.StoreTypeName;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj != null && obj.GetType() == GetType())
		{
			return Equals((RelationalTypeMappingInfo)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_coreTypeMappingInfo, StoreTypeName, IsFixedLength);
	}
}
