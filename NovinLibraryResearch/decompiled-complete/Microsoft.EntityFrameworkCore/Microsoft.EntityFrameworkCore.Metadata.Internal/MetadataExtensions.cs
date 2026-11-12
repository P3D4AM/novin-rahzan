using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class MetadataExtensions
{
	public static TConcrete AsConcreteMetadataType<TInterface, TConcrete>([JetBrains.Annotations.NotNull] TInterface @interface, [JetBrains.Annotations.NotNull] string methodName) where TConcrete : class
	{
		return (@interface as TConcrete) ?? throw new NotSupportedException(CoreStrings.CustomMetadata(methodName, typeof(TInterface).ShortDisplayName(), @interface.GetType().ShortDisplayName()));
	}
}
