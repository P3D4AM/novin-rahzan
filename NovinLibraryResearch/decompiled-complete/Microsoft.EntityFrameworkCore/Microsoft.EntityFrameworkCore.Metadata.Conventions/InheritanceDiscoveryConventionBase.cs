using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public abstract class InheritanceDiscoveryConventionBase
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	protected InheritanceDiscoveryConventionBase([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	protected virtual IConventionEntityType FindClosestBaseType([JetBrains.Annotations.NotNull] IConventionEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Type clrType = entityType.ClrType;
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "ClrType");
		Type baseType = clrType.GetTypeInfo().BaseType;
		IConventionEntityType conventionEntityType = null;
		while (baseType != null && conventionEntityType == null)
		{
			conventionEntityType = entityType.Model.FindEntityType(baseType);
			baseType = baseType.GetTypeInfo().BaseType;
		}
		return conventionEntityType;
	}
}
