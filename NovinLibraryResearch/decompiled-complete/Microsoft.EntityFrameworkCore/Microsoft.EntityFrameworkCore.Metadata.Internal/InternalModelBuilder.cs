using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalModelBuilder : InternalAnnotatableBuilder<Model>, IConventionModelBuilder, IConventionAnnotatableBuilder
{
	public override InternalModelBuilder ModelBuilder => this;

	IConventionModel IConventionModelBuilder.Metadata => Metadata;

	public InternalModelBuilder([JetBrains.Annotations.NotNull] Model metadata)
		: base(metadata)
	{
	}

	public virtual InternalEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource, bool? shouldBeOwned = false)
	{
		TypeIdentity type = new TypeIdentity(name);
		return Entity(in type, configurationSource, shouldBeOwned);
	}

	public virtual InternalEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] Type type, ConfigurationSource configurationSource, bool? shouldBeOwned = false)
	{
		TypeIdentity type2 = new TypeIdentity(type, Metadata);
		return Entity(in type2, configurationSource, shouldBeOwned);
	}

	private InternalEntityTypeBuilder Entity(in TypeIdentity type, ConfigurationSource configurationSource, bool? shouldBeOwned)
	{
		if (IsIgnored(in type, configurationSource))
		{
			return null;
		}
		Type type2 = type.Type;
		EntityType entityType = ((type2 == null) ? Metadata.FindEntityType(type.Name) : Metadata.FindEntityType(type2));
		if (shouldBeOwned == false && (ShouldBeOwnedType(in type) || (entityType != null && entityType.IsOwned())))
		{
			throw new InvalidOperationException(CoreStrings.ClashingOwnedEntityType((type2 == null) ? type.Name : type2.ShortDisplayName()));
		}
		if (shouldBeOwned == true && entityType != null)
		{
			if (!entityType.IsOwned() && configurationSource == ConfigurationSource.Explicit && entityType.GetConfigurationSource() == ConfigurationSource.Explicit)
			{
				throw new InvalidOperationException(CoreStrings.ClashingNonOwnedEntityType(entityType.DisplayName()));
			}
			foreach (EntityType derivedType in entityType.GetDerivedTypes())
			{
				if (!derivedType.IsOwned() && configurationSource == ConfigurationSource.Explicit && derivedType.GetConfigurationSource() == ConfigurationSource.Explicit)
				{
					throw new InvalidOperationException(CoreStrings.ClashingNonOwnedDerivedEntityType(entityType.DisplayName(), derivedType.DisplayName()));
				}
			}
		}
		if (entityType != null)
		{
			entityType.UpdateConfigurationSource(configurationSource);
			return entityType.Builder;
		}
		Metadata.RemoveIgnored(type.Name);
		return ((type2 == null) ? Metadata.AddEntityType(type.Name, configurationSource) : Metadata.AddEntityType(type2, configurationSource))?.Builder;
	}

	public virtual InternalEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType, ConfigurationSource configurationSource)
	{
		TypeIdentity type = new TypeIdentity(name);
		return Entity(in type, definingNavigationName, definingEntityType, configurationSource);
	}

	public virtual InternalEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType, ConfigurationSource configurationSource)
	{
		TypeIdentity type2 = new TypeIdentity(type, Metadata);
		return Entity(in type2, definingNavigationName, definingEntityType, configurationSource);
	}

	private InternalEntityTypeBuilder Entity(in TypeIdentity type, string definingNavigationName, EntityType definingEntityType, ConfigurationSource configurationSource)
	{
		if (IsIgnored(in type, configurationSource))
		{
			return null;
		}
		Type type2 = type.Type ?? Metadata.FindClrType(type.Name);
		EntityType entityType = ((type2 == null) ? Metadata.FindEntityType(type.Name, definingNavigationName, definingEntityType) : Metadata.FindEntityType(type2, definingNavigationName, definingEntityType));
		if (entityType == null)
		{
			EntityType entityType2 = ((type2 == null) ? Metadata.FindEntityType(type.Name) : Metadata.FindEntityType(type2));
			IConventionBatch conventionBatch = null;
			EntityType.Snapshot snapshot = null;
			if (entityType2 != null)
			{
				if (!configurationSource.Overrides(entityType2.GetConfigurationSource()))
				{
					return null;
				}
				conventionBatch = ModelBuilder.Metadata.ConventionDispatcher.DelayConventions();
				snapshot = InternalEntityTypeBuilder.DetachAllMembers(entityType2);
				HasNoEntityType(entityType2, configurationSource);
			}
			if (type2 == null)
			{
				Metadata.RemoveIgnored(type.Name);
				entityType = Metadata.AddEntityType(type.Name, definingNavigationName, definingEntityType, configurationSource);
			}
			else
			{
				Metadata.RemoveIgnored(type.Name);
				entityType = Metadata.AddEntityType(type2, definingNavigationName, definingEntityType, configurationSource);
			}
			if (conventionBatch != null)
			{
				snapshot.Attach(entityType.Builder);
				conventionBatch.Dispose();
			}
		}
		else
		{
			entityType.UpdateConfigurationSource(configurationSource);
		}
		return entityType?.Builder;
	}

	public virtual IConventionOwnedEntityTypeBuilder Owned([JetBrains.Annotations.NotNull] Type type, ConfigurationSource configurationSource)
	{
		if (IsIgnored(type, configurationSource))
		{
			return null;
		}
		Metadata.RemoveIgnored(type);
		MutableModelExtensions.AddOwned(Metadata, type);
		EntityType entityType = Metadata.FindEntityType(type);
		EntityType entityType2 = entityType;
		if (entityType2 != null && !entityType2.GetForeignKeys().Any((ForeignKey fk) => fk.IsOwnership))
		{
			if (!configurationSource.Overrides(entityType.GetConfigurationSource()))
			{
				return null;
			}
			if (entityType.GetConfigurationSource() == ConfigurationSource.Explicit)
			{
				throw new InvalidOperationException(CoreStrings.ClashingNonOwnedEntityType(entityType.DisplayName()));
			}
			ForeignKey foreignKey = entityType.GetForeignKeys().FirstOrDefault((ForeignKey fk) => fk.PrincipalToDependent != null && !fk.PrincipalEntityType.IsInOwnershipPath(entityType) && !fk.PrincipalEntityType.IsInDefinitionPath(type));
			if (foreignKey != null)
			{
				if (foreignKey.Builder.IsOwnership(true, configurationSource) == null)
				{
					return null;
				}
			}
			else if (!entityType.Builder.RemoveNonOwnershipRelationships(null, configurationSource))
			{
				return null;
			}
		}
		return new InternalOwnedEntityTypeBuilder();
	}

	private bool ShouldBeOwnedType(in TypeIdentity type)
	{
		if (type.Type != null)
		{
			return Metadata.IsOwned(type.Type);
		}
		return false;
	}

	public virtual bool IsIgnored([JetBrains.Annotations.NotNull] Type type, ConfigurationSource configurationSource)
	{
		TypeIdentity type2 = new TypeIdentity(type, Metadata);
		return IsIgnored(in type2, configurationSource);
	}

	public virtual bool IsIgnored([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		TypeIdentity type = new TypeIdentity(name);
		return IsIgnored(in type, configurationSource);
	}

	private bool IsIgnored(in TypeIdentity type, ConfigurationSource configurationSource)
	{
		if (configurationSource == ConfigurationSource.Explicit)
		{
			return false;
		}
		ConfigurationSource? configurationSource2 = Metadata.FindIgnoredConfigurationSource(type.Name);
		if (configurationSource2.HasValue)
		{
			return configurationSource2.Value.Overrides(configurationSource);
		}
		return false;
	}

	public virtual InternalModelBuilder Ignore([JetBrains.Annotations.NotNull] Type type, ConfigurationSource configurationSource)
	{
		TypeIdentity type2 = new TypeIdentity(type, Metadata);
		return Ignore(in type2, configurationSource);
	}

	public virtual InternalModelBuilder Ignore([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		TypeIdentity type = new TypeIdentity(name);
		return Ignore(in type, configurationSource);
	}

	private InternalModelBuilder Ignore(in TypeIdentity type, ConfigurationSource configurationSource)
	{
		string name = type.Name;
		ConfigurationSource? configurationSource2 = Metadata.FindIgnoredConfigurationSource(name);
		if (configurationSource2.HasValue)
		{
			if (configurationSource.Overrides(configurationSource2) && configurationSource != configurationSource2)
			{
				Metadata.AddIgnored(name, configurationSource);
			}
			return this;
		}
		if (!CanIgnore(in type, configurationSource))
		{
			return null;
		}
		using (Metadata.ConventionDispatcher.DelayConventions())
		{
			foreach (EntityType item in Metadata.GetEntityTypes(name).ToList())
			{
				HasNoEntityType(item, configurationSource);
				if (item.HasClrType())
				{
					Metadata.AddIgnored(item.ClrType, configurationSource);
				}
				else
				{
					Metadata.AddIgnored(item.Name, configurationSource);
				}
			}
			if (type.Type == null)
			{
				Metadata.AddIgnored(name, configurationSource);
			}
			else
			{
				Metadata.RemoveOwned(type.Type);
				Metadata.AddIgnored(type.Type, configurationSource);
			}
			return this;
		}
	}

	public virtual bool CanIgnore([JetBrains.Annotations.NotNull] Type type, ConfigurationSource configurationSource)
	{
		TypeIdentity type2 = new TypeIdentity(type, Metadata);
		return CanIgnore(in type2, configurationSource);
	}

	public virtual bool CanIgnore([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		TypeIdentity type = new TypeIdentity(name);
		return CanIgnore(in type, configurationSource);
	}

	private bool CanIgnore(in TypeIdentity type, ConfigurationSource configurationSource)
	{
		string name = type.Name;
		if (Metadata.FindIgnoredConfigurationSource(name).HasValue)
		{
			return true;
		}
		if (ShouldBeOwnedType(in type) && configurationSource != 0)
		{
			return false;
		}
		if (Metadata.GetEntityTypes(name).Any((EntityType o) => !configurationSource.Overrides(o.GetConfigurationSource())))
		{
			return false;
		}
		return true;
	}

	public virtual InternalModelBuilder HasNoEntityType([JetBrains.Annotations.NotNull] EntityType entityType, ConfigurationSource configurationSource)
	{
		ConfigurationSource configurationSource2 = entityType.GetConfigurationSource();
		if (!configurationSource.Overrides(configurationSource2))
		{
			return null;
		}
		using (Metadata.ConventionDispatcher.DelayConventions())
		{
			InternalEntityTypeBuilder builder = entityType.Builder;
			foreach (ForeignKey item in entityType.GetDeclaredForeignKeys().ToList())
			{
				builder.HasNoRelationship(item, configurationSource);
			}
			foreach (ForeignKey item2 in entityType.GetDeclaredReferencingForeignKeys().ToList())
			{
				item2.DeclaringEntityType.Builder.HasNoRelationship(item2, configurationSource);
			}
			foreach (EntityType item3 in entityType.GetDirectlyDerivedTypes().ToList())
			{
				item3.Builder.HasBaseType(entityType.BaseType, configurationSource);
			}
			foreach (EntityType item4 in (from e in Metadata.GetEntityTypes()
				where e.DefiningEntityType == entityType
				select e).ToList())
			{
				HasNoEntityType(item4, configurationSource);
			}
			Metadata.RemoveEntityType(entityType);
			return this;
		}
	}

	public virtual InternalModelBuilder UseChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, ConfigurationSource configurationSource)
	{
		if (CanSetChangeTrackingStrategy(changeTrackingStrategy, configurationSource))
		{
			Metadata.SetChangeTrackingStrategy(changeTrackingStrategy, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, ConfigurationSource configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetChangeTrackingStrategyConfigurationSource()))
		{
			return Metadata.GetChangeTrackingStrategy() == changeTrackingStrategy;
		}
		return true;
	}

	public virtual InternalModelBuilder UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		if (CanSetPropertyAccessMode(propertyAccessMode, configurationSource))
		{
			Metadata.SetPropertyAccessMode(propertyAccessMode, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetPropertyAccessModeConfigurationSource()))
		{
			return Metadata.GetPropertyAccessMode() == propertyAccessMode;
		}
		return true;
	}

	IConventionEntityTypeBuilder IConventionModelBuilder.Entity(string name, bool? shouldBeOwned, bool fromDataAnnotation)
	{
		return Entity(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention, shouldBeOwned);
	}

	IConventionEntityTypeBuilder IConventionModelBuilder.Entity(Type type, bool? shouldBeOwned, bool fromDataAnnotation)
	{
		return Entity(type, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention, shouldBeOwned);
	}

	IConventionEntityTypeBuilder IConventionModelBuilder.Entity(string name, string definingNavigationName, IConventionEntityType definingEntityType, bool fromDataAnnotation)
	{
		return Entity(name, definingNavigationName, (EntityType)definingEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionModelBuilder.Entity(Type type, string definingNavigationName, IConventionEntityType definingEntityType, bool fromDataAnnotation)
	{
		return Entity(type, definingNavigationName, (EntityType)definingEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionOwnedEntityTypeBuilder IConventionModelBuilder.Owned(Type type, bool fromDataAnnotation)
	{
		return Owned(type, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionModelBuilder.IsIgnored(Type type, bool fromDataAnnotation)
	{
		return IsIgnored(type, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionModelBuilder.IsIgnored(string name, bool fromDataAnnotation)
	{
		return IsIgnored(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionModelBuilder IConventionModelBuilder.Ignore(Type type, bool fromDataAnnotation)
	{
		return Ignore(type, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionModelBuilder IConventionModelBuilder.Ignore(string name, bool fromDataAnnotation)
	{
		return Ignore(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionModelBuilder IConventionModelBuilder.HasNoEntityType(IConventionEntityType entityType, bool fromDataAnnotation)
	{
		return HasNoEntityType((EntityType)entityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionModelBuilder.CanIgnore(Type type, bool fromDataAnnotation)
	{
		return CanIgnore(type, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionModelBuilder.CanIgnore(string name, bool fromDataAnnotation)
	{
		return CanIgnore(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionModelBuilder IConventionModelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation)
	{
		return UseChangeTrackingStrategy(changeTrackingStrategy, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionModelBuilder.CanSetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation)
	{
		return CanSetChangeTrackingStrategy(changeTrackingStrategy, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionModelBuilder IConventionModelBuilder.UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation)
	{
		return UsePropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionModelBuilder.CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation)
	{
		return CanSetPropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
