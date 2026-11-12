using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class TypeMappingSource : TypeMappingSourceBase
{
	private readonly ConcurrentDictionary<(TypeMappingInfo, Type, ValueConverter), CoreTypeMapping> _explicitMappings = new ConcurrentDictionary<(TypeMappingInfo, Type, ValueConverter), CoreTypeMapping>();

	protected TypeMappingSource([JetBrains.Annotations.NotNull] TypeMappingSourceDependencies dependencies)
		: base(dependencies)
	{
	}

	private CoreTypeMapping FindMappingWithConversion(in TypeMappingInfo mappingInfo, [JetBrains.Annotations.CanBeNull] IReadOnlyList<IProperty> principals)
	{
		Type type = null;
		ValueConverter valueConverter = null;
		if (principals != null)
		{
			for (int i = 0; i < principals.Count; i++)
			{
				IProperty property = principals[i];
				if (type == null)
				{
					Type providerClrType = property.GetProviderClrType();
					if (providerClrType != null)
					{
						type = providerClrType.UnwrapNullableType();
					}
				}
				if (valueConverter == null)
				{
					ValueConverter valueConverter2 = property.GetValueConverter();
					if (valueConverter2 != null)
					{
						valueConverter = valueConverter2;
					}
				}
			}
		}
		CoreTypeMapping orAdd = _explicitMappings.GetOrAdd((mappingInfo, type, valueConverter), delegate((TypeMappingInfo, Type, ValueConverter) k)
		{
			(TypeMappingInfo, Type, ValueConverter) tuple = k;
			TypeMappingInfo mappingInfo2 = tuple.Item1;
			Type item = tuple.Item2;
			ValueConverter item2 = tuple.Item3;
			CoreTypeMapping coreTypeMapping = ((item == null || item == mappingInfo2.ClrType) ? FindMapping(in mappingInfo2) : null);
			if (coreTypeMapping == null)
			{
				Type clrType = mappingInfo2.ClrType;
				if (clrType != null)
				{
					foreach (ValueConverterInfo item3 in Dependencies.ValueConverterSelector.Select(clrType, item))
					{
						ValueConverterInfo converterInfo = item3;
						TypeMappingInfo mappingInfo3 = mappingInfo2.WithConverter(in converterInfo);
						coreTypeMapping = FindMapping(in mappingInfo3);
						if (coreTypeMapping == null && item != null)
						{
							foreach (ValueConverterInfo item4 in Dependencies.ValueConverterSelector.Select(item))
							{
								ValueConverterInfo converterInfo2 = item4;
								TypeMappingInfo mappingInfo4 = mappingInfo3.WithConverter(in converterInfo2);
								coreTypeMapping = FindMapping(in mappingInfo4);
								if (coreTypeMapping != null)
								{
									coreTypeMapping = coreTypeMapping.Clone(converterInfo2.Create());
									break;
								}
							}
						}
						if (coreTypeMapping != null)
						{
							coreTypeMapping = coreTypeMapping.Clone(converterInfo.Create());
							break;
						}
					}
				}
			}
			if (coreTypeMapping != null && item2 != null)
			{
				coreTypeMapping = coreTypeMapping.Clone(item2);
			}
			return coreTypeMapping;
		});
		ValidateMapping(orAdd, principals?[0]);
		return orAdd;
	}

	public override CoreTypeMapping FindMapping(IProperty property)
	{
		CoreTypeMapping coreTypeMapping = property.FindTypeMapping();
		if (coreTypeMapping != null)
		{
			return coreTypeMapping;
		}
		IReadOnlyList<IProperty> principals = property.FindPrincipals();
		TypeMappingInfo mappingInfo = new TypeMappingInfo(principals);
		return FindMappingWithConversion(in mappingInfo, principals);
	}

	public override CoreTypeMapping FindMapping(Type type)
	{
		TypeMappingInfo mappingInfo = new TypeMappingInfo(type);
		return FindMappingWithConversion(in mappingInfo, null);
	}

	public override CoreTypeMapping FindMapping(MemberInfo member)
	{
		TypeMappingInfo mappingInfo = new TypeMappingInfo(member);
		return FindMappingWithConversion(in mappingInfo, null);
	}
}
