using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ValueGenerationConvention : IEntityTypePrimaryKeyChangedConvention, IConvention, IForeignKeyAddedConvention, IForeignKeyRemovedConvention, IForeignKeyPropertiesChangedConvention, IEntityTypeBaseTypeChangedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ValueGenerationConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessForeignKeyAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		foreach (IConventionProperty property in relationshipBuilder.Metadata.Properties)
		{
			property.Builder.ValueGenerated(ValueGenerated.Never);
		}
	}

	public virtual void ProcessForeignKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionForeignKey foreignKey, IConventionContext<IConventionForeignKey> context)
	{
		OnForeignKeyRemoved(foreignKey.Properties);
	}

	public virtual void ProcessForeignKeyPropertiesChanged(IConventionRelationshipBuilder relationshipBuilder, IReadOnlyList<IConventionProperty> oldDependentProperties, IConventionKey oldPrincipalKey, IConventionContext<IConventionRelationshipBuilder> context)
	{
		IConventionForeignKey metadata = relationshipBuilder.Metadata;
		if (metadata.Properties.SequenceEqual(oldDependentProperties))
		{
			return;
		}
		OnForeignKeyRemoved(oldDependentProperties);
		if (relationshipBuilder.Metadata.Builder == null)
		{
			return;
		}
		foreach (IConventionProperty property in metadata.Properties)
		{
			property.Builder.ValueGenerated(ValueGenerated.Never);
		}
	}

	private void OnForeignKeyRemoved(IReadOnlyList<IConventionProperty> foreignKeyProperties)
	{
		foreach (IConventionProperty foreignKeyProperty in foreignKeyProperties)
		{
			IConventionKey conventionKey = foreignKeyProperty.FindContainingPrimaryKey();
			if (conventionKey == null)
			{
				foreignKeyProperty.Builder?.ValueGenerated(GetValueGenerated(foreignKeyProperty));
				continue;
			}
			foreach (IConventionProperty property in conventionKey.Properties)
			{
				property.Builder.ValueGenerated(GetValueGenerated(foreignKeyProperty));
			}
		}
	}

	public virtual void ProcessEntityTypePrimaryKeyChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey newPrimaryKey, IConventionKey previousPrimaryKey, IConventionContext<IConventionKey> context)
	{
		if (previousPrimaryKey != null)
		{
			foreach (IConventionProperty property in previousPrimaryKey.Properties)
			{
				property.Builder?.ValueGenerated(ValueGenerated.Never);
			}
		}
		if (newPrimaryKey?.Builder == null)
		{
			return;
		}
		foreach (IConventionProperty property2 in newPrimaryKey.Properties)
		{
			property2.Builder.ValueGenerated(GetValueGenerated(property2));
		}
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if (entityTypeBuilder.Metadata.BaseType != newBaseType)
		{
			return;
		}
		foreach (IConventionProperty property in entityTypeBuilder.Metadata.GetProperties())
		{
			property.Builder.ValueGenerated(GetValueGenerated(property));
		}
	}

	protected virtual ValueGenerated? GetValueGenerated([JetBrains.Annotations.NotNull] IConventionProperty property)
	{
		return GetValueGenerated((IProperty)property);
	}

	public static ValueGenerated? GetValueGenerated([JetBrains.Annotations.NotNull] IProperty property)
	{
		if (property.IsForeignKey() || !ShouldHaveGeneratedProperty(property.FindContainingPrimaryKey()) || !CanBeGenerated(property))
		{
			return null;
		}
		return ValueGenerated.OnAdd;
	}

	private static bool ShouldHaveGeneratedProperty(IKey key)
	{
		bool? flag = key?.DeclaringEntityType.IsOwned();
		if (key != null)
		{
			if (!flag.Value || key.Properties.Count((IProperty p) => !p.IsForeignKey()) != 1)
			{
				if (!flag.Value)
				{
					return key.Properties.Count == 1;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private static bool CanBeGenerated(IProperty property)
	{
		Type type = property.ClrType.UnwrapNullableType();
		if (!type.IsInteger() || !(type != typeof(byte)))
		{
			return type == typeof(Guid);
		}
		return true;
	}
}
