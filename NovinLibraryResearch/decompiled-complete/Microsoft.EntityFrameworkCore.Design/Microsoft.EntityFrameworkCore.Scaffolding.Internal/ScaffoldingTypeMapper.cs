using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class ScaffoldingTypeMapper : IScaffoldingTypeMapper
{
	private readonly IRelationalTypeMappingSource _typeMappingSource;

	public ScaffoldingTypeMapper([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		_typeMappingSource = typeMappingSource;
	}

	public virtual TypeScaffoldingInfo FindMapping(string storeType, bool keyOrIndex, bool rowVersion)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(storeType, "storeType");
		RelationalTypeMapping relationalTypeMapping = _typeMappingSource.FindMapping(storeType);
		if (relationalTypeMapping == null)
		{
			return null;
		}
		bool inferred = false;
		bool? scaffoldUnicode = null;
		bool? scaffoldFixedLength = null;
		int? scaffoldMaxLength = null;
		if (relationalTypeMapping.ClrType == typeof(byte[]))
		{
			RelationalTypeMapping relationalTypeMapping2 = _typeMappingSource.FindMapping(typeof(byte[]), null, keyOrIndex, null, rowVersion: rowVersion, size: relationalTypeMapping.Size, fixedLength: relationalTypeMapping.IsFixedLength);
			if (relationalTypeMapping2.StoreType.Equals(storeType, StringComparison.OrdinalIgnoreCase))
			{
				inferred = true;
				scaffoldFixedLength = ((_typeMappingSource.FindMapping(typeof(byte[]), null, keyOrIndex, null, rowVersion: rowVersion, size: relationalTypeMapping.Size, fixedLength: false).IsFixedLength != relationalTypeMapping2.IsFixedLength) ? new bool?(relationalTypeMapping2.IsFixedLength) : null);
				scaffoldMaxLength = ((_typeMappingSource.FindMapping(typeof(byte[]), null, keyOrIndex, null, null, rowVersion, relationalTypeMapping.IsFixedLength).Size != relationalTypeMapping2.Size) ? relationalTypeMapping2.Size : null);
			}
		}
		else if (relationalTypeMapping.ClrType == typeof(string))
		{
			RelationalTypeMapping relationalTypeMapping3 = _typeMappingSource.FindMapping(typeof(string), null, keyOrIndex, relationalTypeMapping.IsUnicode, relationalTypeMapping.Size, null, relationalTypeMapping.IsFixedLength);
			if (relationalTypeMapping3.StoreType.Equals(storeType, StringComparison.OrdinalIgnoreCase))
			{
				inferred = true;
				scaffoldUnicode = ((_typeMappingSource.FindMapping(typeof(string), null, keyOrIndex, true, relationalTypeMapping.Size, null, relationalTypeMapping.IsFixedLength).IsUnicode != relationalTypeMapping3.IsUnicode) ? new bool?(relationalTypeMapping3.IsUnicode) : null);
				scaffoldFixedLength = ((_typeMappingSource.FindMapping(typeof(string), null, keyOrIndex, relationalTypeMapping.IsUnicode, relationalTypeMapping.Size, null, false).IsFixedLength != relationalTypeMapping3.IsFixedLength) ? new bool?(relationalTypeMapping3.IsFixedLength) : null);
				scaffoldMaxLength = ((_typeMappingSource.FindMapping(typeof(string), null, keyOrIndex, relationalTypeMapping.IsUnicode, null, null, relationalTypeMapping.IsFixedLength).Size != relationalTypeMapping3.Size) ? relationalTypeMapping3.Size : null);
			}
		}
		else if (string.Equals(_typeMappingSource.FindMapping(relationalTypeMapping.ClrType)?.StoreType, storeType, StringComparison.OrdinalIgnoreCase) && relationalTypeMapping.ClrType.UnwrapNullableType() != typeof(decimal))
		{
			inferred = true;
		}
		return new TypeScaffoldingInfo(relationalTypeMapping.ClrType, inferred, scaffoldUnicode, scaffoldMaxLength, scaffoldFixedLength);
	}
}
