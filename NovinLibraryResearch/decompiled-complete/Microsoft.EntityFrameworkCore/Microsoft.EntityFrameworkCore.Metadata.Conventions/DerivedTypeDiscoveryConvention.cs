using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class DerivedTypeDiscoveryConvention : InheritanceDiscoveryConventionBase, IEntityTypeAddedConvention, IConvention
{
	public DerivedTypeDiscoveryConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		IConventionEntityType entityType = entityTypeBuilder.Metadata;
		Type clrType = entityType.ClrType;
		if (clrType == null || entityType.HasDefiningNavigation())
		{
			return;
		}
		IConventionModel model = entityType.Model;
		foreach (IConventionEntityType item in (from t in model.GetEntityTypes()
			where t != entityType && t.HasClrType() && !t.HasDefiningNavigation() && t.FindDeclaredOwnership() == null && !model.FindIsOwnedConfigurationSource(t.ClrType).HasValue && ((t.BaseType == null && clrType.GetTypeInfo().IsAssignableFrom(t.ClrType.GetTypeInfo())) || (t.BaseType == entityType.BaseType && FindClosestBaseType(t) == entityType))
			select t).ToList())
		{
			item.Builder.HasBaseType(entityType);
		}
	}
}
