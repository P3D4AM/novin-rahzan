using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface ITypeMappingSource
{
	CoreTypeMapping FindMapping([JetBrains.Annotations.NotNull] IProperty property);

	CoreTypeMapping FindMapping([JetBrains.Annotations.NotNull] MemberInfo member);

	CoreTypeMapping FindMapping([JetBrains.Annotations.NotNull] Type type);
}
