using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalTypeMappingSource : ITypeMappingSource
{
	new RelationalTypeMapping FindMapping([JetBrains.Annotations.NotNull] IProperty property);

	new RelationalTypeMapping FindMapping([JetBrains.Annotations.NotNull] MemberInfo member);

	new RelationalTypeMapping FindMapping([JetBrains.Annotations.NotNull] Type type);

	RelationalTypeMapping FindMapping([JetBrains.Annotations.NotNull] string storeTypeName);

	RelationalTypeMapping FindMapping([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.CanBeNull] string storeTypeName, bool keyOrIndex = false, bool? unicode = null, int? size = null, bool? rowVersion = null, bool? fixedLength = null, int? precision = null, int? scale = null);
}
