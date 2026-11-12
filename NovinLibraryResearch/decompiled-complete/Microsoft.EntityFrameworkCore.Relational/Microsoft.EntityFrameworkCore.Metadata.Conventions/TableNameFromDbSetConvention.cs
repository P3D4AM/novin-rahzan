using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class TableNameFromDbSetConvention : IEntityTypeAddedConvention, IConvention, IEntityTypeBaseTypeChangedConvention
{
	private readonly IDictionary<Type, DbSetProperty> _sets;

	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public TableNameFromDbSetConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		_sets = dependencies.SetFinder.CreateClrTypeDbSetMapping(dependencies.ContextType);
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if (_sets != null)
		{
			IConventionEntityType metadata = entityTypeBuilder.Metadata;
			if (oldBaseType == null && newBaseType != null)
			{
				entityTypeBuilder.ToTable(null);
			}
			else if (oldBaseType != null && newBaseType == null && metadata.ClrType != null && _sets.ContainsKey(metadata.ClrType))
			{
				entityTypeBuilder.ToTable(_sets[metadata.ClrType].Name);
			}
		}
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (metadata.BaseType == null && metadata.ClrType != null && _sets.ContainsKey(metadata.ClrType))
		{
			entityTypeBuilder.ToTable(_sets[metadata.ClrType].Name);
		}
	}
}
