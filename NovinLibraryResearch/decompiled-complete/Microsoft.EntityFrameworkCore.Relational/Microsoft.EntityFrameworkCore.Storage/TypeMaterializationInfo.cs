using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class TypeMaterializationInfo
{
	public virtual Type ProviderClrType { get; }

	public virtual Type ModelClrType { get; }

	public virtual RelationalTypeMapping Mapping { get; }

	public virtual IProperty Property { get; }

	public virtual int Index { get; }

	public virtual bool? IsFromLeftOuterJoin { get; }

	public TypeMaterializationInfo([JetBrains.Annotations.NotNull] Type modelClrType, [JetBrains.Annotations.CanBeNull] IProperty property, [JetBrains.Annotations.CanBeNull] IRelationalTypeMappingSource typeMappingSource, int index = -1)
		: this(modelClrType, property, typeMappingSource, null, index)
	{
	}

	public TypeMaterializationInfo([JetBrains.Annotations.NotNull] Type modelClrType, [JetBrains.Annotations.CanBeNull] IProperty property, [JetBrains.Annotations.CanBeNull] IRelationalTypeMappingSource typeMappingSource, bool? fromLeftOuterJoin, int index)
		: this(modelClrType, property, typeMappingSource, fromLeftOuterJoin, index, null)
	{
	}

	public TypeMaterializationInfo([JetBrains.Annotations.NotNull] Type modelClrType, [JetBrains.Annotations.CanBeNull] IProperty property, [JetBrains.Annotations.CanBeNull] IRelationalTypeMappingSource typeMappingSource, bool? fromLeftOuterJoin, int index = -1, [JetBrains.Annotations.CanBeNull] RelationalTypeMapping mapping = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelClrType, "modelClrType");
		if (mapping == null)
		{
			mapping = property?.GetRelationalTypeMapping() ?? typeMappingSource?.GetMapping(modelClrType);
		}
		ProviderClrType = mapping?.Converter?.ProviderClrType ?? modelClrType;
		ModelClrType = modelClrType;
		Mapping = mapping;
		Property = property;
		Index = index;
		IsFromLeftOuterJoin = fromLeftOuterJoin;
	}

	protected virtual bool Equals([JetBrains.Annotations.NotNull] TypeMaterializationInfo other)
	{
		if (ProviderClrType == other.ProviderClrType && ModelClrType == other.ModelClrType && object.Equals(Mapping, other.Mapping) && object.Equals(Property, other.Property) && Index == other.Index)
		{
			return IsFromLeftOuterJoin == other.IsFromLeftOuterJoin;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj.GetType() == GetType())
				{
					return Equals((TypeMaterializationInfo)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(ProviderClrType, ModelClrType, Mapping, Property, Index, IsFromLeftOuterJoin);
	}
}
