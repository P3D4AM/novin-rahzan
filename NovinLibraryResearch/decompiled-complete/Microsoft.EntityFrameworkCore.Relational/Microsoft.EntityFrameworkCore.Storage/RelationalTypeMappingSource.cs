using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class RelationalTypeMappingSource : TypeMappingSourceBase, IRelationalTypeMappingSource, ITypeMappingSource
{
	private readonly ConcurrentDictionary<(RelationalTypeMappingInfo, Type, ValueConverter), RelationalTypeMapping> _explicitMappings = new ConcurrentDictionary<(RelationalTypeMappingInfo, Type, ValueConverter), RelationalTypeMapping>();

	protected virtual RelationalTypeMappingSourceDependencies RelationalDependencies { get; }

	protected RelationalTypeMappingSource([JetBrains.Annotations.NotNull] TypeMappingSourceDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalTypeMappingSourceDependencies relationalDependencies)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		RelationalDependencies = relationalDependencies;
	}

	protected virtual RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
	{
		foreach (IRelationalTypeMappingSourcePlugin plugin in RelationalDependencies.Plugins)
		{
			RelationalTypeMapping relationalTypeMapping = plugin.FindMapping(in mappingInfo);
			if (relationalTypeMapping != null)
			{
				return relationalTypeMapping;
			}
		}
		return null;
	}

	protected override CoreTypeMapping FindMapping(in TypeMappingInfo mappingInfo)
	{
		throw new InvalidOperationException("FindMapping on a 'RelationalTypeMappingSource' with a non-relational 'TypeMappingInfo'.");
	}

	private RelationalTypeMapping FindMappingWithConversion(in RelationalTypeMappingInfo mappingInfo, [JetBrains.Annotations.CanBeNull] IReadOnlyList<IProperty> principals)
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
		RelationalTypeMapping orAdd = _explicitMappings.GetOrAdd((mappingInfo, type, valueConverter), delegate((RelationalTypeMappingInfo, Type, ValueConverter) k)
		{
			(RelationalTypeMappingInfo, Type, ValueConverter) tuple = k;
			RelationalTypeMappingInfo mappingInfo2 = tuple.Item1;
			Type item = tuple.Item2;
			ValueConverter item2 = tuple.Item3;
			RelationalTypeMapping relationalTypeMapping = ((item == null || item == mappingInfo2.ClrType) ? FindMapping(in mappingInfo2) : null);
			if (relationalTypeMapping == null)
			{
				Type clrType = mappingInfo2.ClrType;
				if (clrType != null)
				{
					foreach (ValueConverterInfo item3 in Dependencies.ValueConverterSelector.Select(clrType, item))
					{
						ValueConverterInfo converterInfo = item3;
						RelationalTypeMappingInfo mappingInfo3 = mappingInfo2.WithConverter(in converterInfo);
						relationalTypeMapping = FindMapping(in mappingInfo3);
						if (relationalTypeMapping == null && item != null)
						{
							foreach (ValueConverterInfo item4 in Dependencies.ValueConverterSelector.Select(item))
							{
								ValueConverterInfo converterInfo2 = item4;
								RelationalTypeMappingInfo mappingInfo4 = mappingInfo3.WithConverter(in converterInfo2);
								relationalTypeMapping = FindMapping(in mappingInfo4);
								if (relationalTypeMapping != null)
								{
									relationalTypeMapping = (RelationalTypeMapping)relationalTypeMapping.Clone(converterInfo2.Create());
									break;
								}
							}
						}
						if (relationalTypeMapping != null)
						{
							relationalTypeMapping = (RelationalTypeMapping)relationalTypeMapping.Clone(converterInfo.Create());
							break;
						}
					}
				}
			}
			if (relationalTypeMapping != null && item2 != null)
			{
				relationalTypeMapping = (RelationalTypeMapping)relationalTypeMapping.Clone(item2);
			}
			return relationalTypeMapping;
		});
		ValidateMapping(orAdd, principals?[0]);
		return orAdd;
	}

	public override CoreTypeMapping FindMapping(IProperty property)
	{
		RelationalTypeMapping relationalTypeMapping = property.FindRelationalMapping();
		if (relationalTypeMapping != null)
		{
			return relationalTypeMapping;
		}
		IReadOnlyList<IProperty> readOnlyList = property.FindPrincipals();
		string text = null;
		bool? fixedLength = null;
		for (int i = 0; i < readOnlyList.Count; i++)
		{
			IProperty property2 = readOnlyList[i];
			if (text == null)
			{
				string text2 = (string)property2["Relational:ColumnType"];
				if (text2 != null)
				{
					text = text2;
				}
			}
			if (!fixedLength.HasValue)
			{
				fixedLength = property2.IsFixedLength();
			}
		}
		bool? unicode;
		int? size;
		int? precision;
		int? scale;
		string storeTypeNameBase = ParseStoreTypeName(text, out unicode, out size, out precision, out scale);
		RelationalTypeMappingInfo mappingInfo = new RelationalTypeMappingInfo(readOnlyList, text, storeTypeNameBase, unicode, fixedLength, size, precision, scale);
		return FindMappingWithConversion(in mappingInfo, readOnlyList);
	}

	public override CoreTypeMapping FindMapping(Type type)
	{
		RelationalTypeMappingInfo mappingInfo = new RelationalTypeMappingInfo(type);
		return FindMappingWithConversion(in mappingInfo, null);
	}

	public override CoreTypeMapping FindMapping(MemberInfo member)
	{
		ColumnAttribute customAttribute = member.GetCustomAttribute<ColumnAttribute>(inherit: true);
		RelationalTypeMappingInfo mappingInfo;
		if (customAttribute != null)
		{
			string typeName = customAttribute.TypeName;
			bool? unicode;
			int? size;
			int? precision;
			int? scale;
			string storeTypeNameBase = ParseStoreTypeName(customAttribute.TypeName, out unicode, out size, out precision, out scale);
			mappingInfo = new RelationalTypeMappingInfo(member, typeName, storeTypeNameBase, unicode, size, precision, scale);
			return FindMappingWithConversion(in mappingInfo, null);
		}
		mappingInfo = new RelationalTypeMappingInfo(member);
		return FindMappingWithConversion(in mappingInfo, null);
	}

	public virtual RelationalTypeMapping FindMapping(string storeTypeName)
	{
		bool? unicode;
		int? size;
		int? precision;
		int? scale;
		string storeTypeNameBase = ParseStoreTypeName(storeTypeName, out unicode, out size, out precision, out scale);
		RelationalTypeMappingInfo mappingInfo = new RelationalTypeMappingInfo(storeTypeName, storeTypeNameBase, unicode, size, precision, scale);
		return FindMappingWithConversion(in mappingInfo, null);
	}

	public virtual RelationalTypeMapping FindMapping(Type type, string storeTypeName, bool keyOrIndex = false, bool? unicode = null, int? size = null, bool? rowVersion = null, bool? fixedLength = null, int? precision = null, int? scale = null)
	{
		string storeTypeNameBase = null;
		if (storeTypeName != null)
		{
			storeTypeNameBase = ParseStoreTypeName(storeTypeName, out var unicode2, out var size2, out var precision2, out var scale2);
			if (!size.HasValue)
			{
				size = size2;
			}
			if (!precision.HasValue)
			{
				precision = precision2;
			}
			if (!scale.HasValue)
			{
				scale = scale2;
			}
			if (!unicode.HasValue)
			{
				unicode = unicode2;
			}
		}
		RelationalTypeMappingInfo mappingInfo = new RelationalTypeMappingInfo(type, storeTypeName, storeTypeNameBase, keyOrIndex, unicode, size, rowVersion, fixedLength, precision, scale);
		return FindMappingWithConversion(in mappingInfo, null);
	}

	RelationalTypeMapping IRelationalTypeMappingSource.FindMapping(IProperty property)
	{
		return (RelationalTypeMapping)FindMapping(property);
	}

	RelationalTypeMapping IRelationalTypeMappingSource.FindMapping(Type type)
	{
		return (RelationalTypeMapping)FindMapping(type);
	}

	RelationalTypeMapping IRelationalTypeMappingSource.FindMapping(MemberInfo member)
	{
		return (RelationalTypeMapping)FindMapping(member);
	}

	protected virtual string ParseStoreTypeName([JetBrains.Annotations.CanBeNull] string storeTypeName, out bool? unicode, out int? size, out int? precision, out int? scale)
	{
		unicode = null;
		size = null;
		precision = null;
		scale = null;
		if (storeTypeName != null)
		{
			int num = storeTypeName.IndexOf("(", StringComparison.Ordinal);
			if (num > 0)
			{
				int num2 = storeTypeName.IndexOf(")", num + 1, StringComparison.Ordinal);
				if (num2 > num)
				{
					int num3 = storeTypeName.IndexOf(",", num + 1, StringComparison.Ordinal);
					int result3;
					if (num3 > num && num3 < num2)
					{
						if (int.TryParse(storeTypeName.Substring(num + 1, num3 - num - 1), out var result))
						{
							precision = result;
						}
						if (int.TryParse(storeTypeName.Substring(num3 + 1, num2 - num3 - 1), out var result2))
						{
							scale = result2;
						}
					}
					else if (int.TryParse(storeTypeName.Substring(num + 1, num2 - num - 1).Trim(), out result3))
					{
						size = result3;
						precision = result3;
					}
					return storeTypeName.Substring(0, num).Trim();
				}
			}
		}
		return storeTypeName;
	}
}
