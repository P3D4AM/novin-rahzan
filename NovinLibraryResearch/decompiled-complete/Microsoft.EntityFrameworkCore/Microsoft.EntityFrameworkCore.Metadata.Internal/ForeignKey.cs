using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ForeignKey : ConventionAnnotatable, IMutableForeignKey, IForeignKey, IAnnotatable, IMutableAnnotatable, IConventionForeignKey, IConventionAnnotatable
{
	private DeleteBehavior? _deleteBehavior;

	private bool? _isUnique;

	private bool _isRequired;

	private bool? _isOwnership;

	private ConfigurationSource _configurationSource;

	private ConfigurationSource? _propertiesConfigurationSource;

	private ConfigurationSource? _principalKeyConfigurationSource;

	private ConfigurationSource? _isUniqueConfigurationSource;

	private ConfigurationSource? _isRequiredConfigurationSource;

	private ConfigurationSource? _deleteBehaviorConfigurationSource;

	private ConfigurationSource? _principalEndConfigurationSource;

	private ConfigurationSource? _isOwnershipConfigurationSource;

	private ConfigurationSource? _dependentToPrincipalConfigurationSource;

	private ConfigurationSource? _principalToDependentConfigurationSource;

	public virtual IReadOnlyList<Property> Properties
	{
		[DebuggerStepThrough]
		get;
		private set; }

	public virtual Key PrincipalKey
	{
		[DebuggerStepThrough]
		get;
		private set; }

	public virtual EntityType DeclaringEntityType
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual EntityType PrincipalEntityType
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual InternalRelationshipBuilder Builder
	{
		[DebuggerStepThrough]
		get;
		[DebuggerStepThrough]
		[param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual Navigation DependentToPrincipal
	{
		[DebuggerStepThrough]
		get;
		private set; }

	public virtual Navigation PrincipalToDependent
	{
		[DebuggerStepThrough]
		get;
		private set; }

	public virtual bool IsUnique
	{
		get
		{
			return _isUnique ?? DefaultIsUnique;
		}
		set
		{
			SetIsUnique(value, ConfigurationSource.Explicit);
		}
	}

	private static bool DefaultIsUnique => false;

	public virtual bool IsRequired
	{
		get
		{
			return _isRequired;
		}
		set
		{
			SetIsRequired(value, ConfigurationSource.Explicit);
		}
	}

	private bool DefaultIsRequired => !Properties.Any((Property p) => p.IsNullable);

	public virtual DeleteBehavior DeleteBehavior
	{
		get
		{
			return _deleteBehavior ?? DefaultDeleteBehavior;
		}
		set
		{
			SetDeleteBehavior(value, ConfigurationSource.Explicit);
		}
	}

	private static DeleteBehavior DefaultDeleteBehavior => DeleteBehavior.ClientSetNull;

	public virtual bool IsOwnership
	{
		get
		{
			return _isOwnership ?? DefaultIsOwnership;
		}
		set
		{
			SetIsOwnership(value, ConfigurationSource.Explicit);
		}
	}

	private static bool DefaultIsOwnership => false;

	public virtual object DependentKeyValueFactory
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual Func<IDependentsMap> DependentsMapFactory
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	IReadOnlyList<IProperty> IForeignKey.Properties
	{
		[DebuggerStepThrough]
		get
		{
			return Properties;
		}
	}

	IKey IForeignKey.PrincipalKey
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalKey;
		}
	}

	IEntityType IForeignKey.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IEntityType IForeignKey.PrincipalEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalEntityType;
		}
	}

	INavigation IForeignKey.DependentToPrincipal
	{
		[DebuggerStepThrough]
		get
		{
			return DependentToPrincipal;
		}
	}

	INavigation IForeignKey.PrincipalToDependent
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalToDependent;
		}
	}

	IReadOnlyList<IMutableProperty> IMutableForeignKey.Properties
	{
		[DebuggerStepThrough]
		get
		{
			return Properties;
		}
	}

	IMutableKey IMutableForeignKey.PrincipalKey
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalKey;
		}
	}

	IMutableEntityType IMutableForeignKey.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IMutableEntityType IMutableForeignKey.PrincipalEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalEntityType;
		}
	}

	IMutableNavigation IMutableForeignKey.DependentToPrincipal
	{
		[DebuggerStepThrough]
		get
		{
			return DependentToPrincipal;
		}
	}

	IMutableNavigation IMutableForeignKey.PrincipalToDependent
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalToDependent;
		}
	}

	IConventionEntityType IConventionForeignKey.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IConventionEntityType IConventionForeignKey.PrincipalEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalEntityType;
		}
	}

	IConventionKey IConventionForeignKey.PrincipalKey
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalKey;
		}
	}

	IReadOnlyList<IConventionProperty> IConventionForeignKey.Properties
	{
		[DebuggerStepThrough]
		get
		{
			return Properties;
		}
	}

	IConventionNavigation IConventionForeignKey.DependentToPrincipal
	{
		[DebuggerStepThrough]
		get
		{
			return DependentToPrincipal;
		}
	}

	IConventionNavigation IConventionForeignKey.PrincipalToDependent
	{
		[DebuggerStepThrough]
		get
		{
			return PrincipalToDependent;
		}
	}

	public virtual DebugView<ForeignKey> DebugView => new DebugView<ForeignKey>(this, (ForeignKey m) => m.ToDebugString(singleLine: false));

	IConventionRelationshipBuilder IConventionForeignKey.Builder => Builder;

	public ForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.NotNull] Key principalKey, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, [JetBrains.Annotations.NotNull] EntityType principalEntityType, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(dependentProperties, "dependentProperties");
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(dependentProperties, "dependentProperties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalKey, "principalKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		Validate(dependentProperties, principalKey, dependentEntityType, principalEntityType);
		Properties = dependentProperties;
		PrincipalKey = principalKey;
		DeclaringEntityType = dependentEntityType;
		PrincipalEntityType = principalEntityType;
		_configurationSource = configurationSource;
		_isRequired = DefaultIsRequired;
		if (principalEntityType.FindKey(principalKey.Properties) != principalKey)
		{
			throw new InvalidOperationException(CoreStrings.ForeignKeyReferencedEntityKeyMismatch(principalKey.Properties.Format(), principalEntityType.DisplayName()));
		}
		Builder = new InternalRelationshipBuilder(this, dependentEntityType.Model.Builder);
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		_configurationSource = _configurationSource.Max(configurationSource);
		DeclaringEntityType.UpdateConfigurationSource(configurationSource);
		PrincipalEntityType.UpdateConfigurationSource(configurationSource);
	}

	protected override IConventionAnnotation OnAnnotationSet(string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
	{
		return Builder.ModelBuilder.Metadata.ConventionDispatcher.OnForeignKeyAnnotationChanged(Builder, name, annotation, oldAnnotation);
	}

	public virtual void SetProperties(IReadOnlyList<Property> properties, Key principalKey, ConfigurationSource? configurationSource)
	{
		Validate(properties, principalKey, DeclaringEntityType, PrincipalEntityType);
		IReadOnlyList<Property> properties2 = Properties;
		Key principalKey2 = PrincipalKey;
		DeclaringEntityType.OnForeignKeyUpdating(this);
		Properties = properties;
		PrincipalKey = principalKey;
		DeclaringEntityType.OnForeignKeyUpdated(this);
		if (configurationSource.HasValue)
		{
			UpdatePropertiesConfigurationSource(configurationSource.Value);
			UpdatePrincipalKeyConfigurationSource(configurationSource.Value);
		}
		DeclaringEntityType.Model.ConventionDispatcher.OnForeignKeyPropertiesChanged(Builder, properties2, principalKey2);
	}

	public virtual ConfigurationSource? GetPropertiesConfigurationSource()
	{
		return _propertiesConfigurationSource;
	}

	public virtual void UpdatePropertiesConfigurationSource(ConfigurationSource configurationSource)
	{
		_propertiesConfigurationSource = configurationSource.Max(_propertiesConfigurationSource);
		foreach (Property property in Properties)
		{
			property.UpdateConfigurationSource(configurationSource);
		}
	}

	public virtual ConfigurationSource? GetPrincipalKeyConfigurationSource()
	{
		return _principalKeyConfigurationSource;
	}

	public virtual void UpdatePrincipalKeyConfigurationSource(ConfigurationSource configurationSource)
	{
		_principalKeyConfigurationSource = configurationSource.Max(_principalKeyConfigurationSource);
		PrincipalKey.UpdateConfigurationSource(configurationSource);
	}

	public virtual ConfigurationSource? GetPrincipalEndConfigurationSource()
	{
		return _principalEndConfigurationSource;
	}

	public virtual void SetPrincipalEndConfigurationSource(ConfigurationSource? configurationSource)
	{
		_principalEndConfigurationSource = configurationSource;
	}

	public virtual void UpdatePrincipalEndConfigurationSource(ConfigurationSource configurationSource)
	{
		_principalEndConfigurationSource = configurationSource.Max(_principalEndConfigurationSource);
	}

	public virtual Navigation HasDependentToPrincipal([JetBrains.Annotations.CanBeNull] string name, ConfigurationSource configurationSource)
	{
		return Navigation(MemberIdentity.Create(name), configurationSource, pointsToPrincipal: true);
	}

	public virtual Navigation HasDependentToPrincipal([JetBrains.Annotations.CanBeNull] MemberInfo property, ConfigurationSource configurationSource)
	{
		return Navigation(MemberIdentity.Create(property), configurationSource, pointsToPrincipal: true);
	}

	public virtual ConfigurationSource? GetDependentToPrincipalConfigurationSource()
	{
		return _dependentToPrincipalConfigurationSource;
	}

	public virtual void UpdateDependentToPrincipalConfigurationSource(ConfigurationSource? configurationSource)
	{
		_dependentToPrincipalConfigurationSource = configurationSource.Max(_dependentToPrincipalConfigurationSource);
	}

	public virtual Navigation HasPrincipalToDependent([JetBrains.Annotations.CanBeNull] string name, ConfigurationSource configurationSource)
	{
		return Navigation(MemberIdentity.Create(name), configurationSource, pointsToPrincipal: false);
	}

	public virtual Navigation HasPrincipalToDependent([JetBrains.Annotations.CanBeNull] MemberInfo property, ConfigurationSource configurationSource)
	{
		return Navigation(MemberIdentity.Create(property), configurationSource, pointsToPrincipal: false);
	}

	public virtual ConfigurationSource? GetPrincipalToDependentConfigurationSource()
	{
		return _principalToDependentConfigurationSource;
	}

	public virtual void UpdatePrincipalToDependentConfigurationSource(ConfigurationSource? configurationSource)
	{
		_principalToDependentConfigurationSource = configurationSource.Max(_principalToDependentConfigurationSource);
	}

	private Navigation Navigation(MemberIdentity? propertyIdentity, ConfigurationSource configurationSource, bool pointsToPrincipal)
	{
		string text = propertyIdentity?.Name;
		if (pointsToPrincipal && PrincipalEntityType.IsKeyless)
		{
			throw new InvalidOperationException(CoreStrings.NavigationToKeylessType(text, PrincipalEntityType.DisplayName()));
		}
		if (!pointsToPrincipal && DeclaringEntityType.IsKeyless)
		{
			throw new InvalidOperationException(CoreStrings.NavigationToKeylessType(text, DeclaringEntityType.DisplayName()));
		}
		Navigation navigation = (pointsToPrincipal ? DependentToPrincipal : PrincipalToDependent);
		if (text == navigation?.Name)
		{
			if (pointsToPrincipal)
			{
				UpdateDependentToPrincipalConfigurationSource(configurationSource);
			}
			else
			{
				UpdatePrincipalToDependentConfigurationSource(configurationSource);
			}
			return navigation;
		}
		if (navigation != null)
		{
			if (pointsToPrincipal)
			{
				DeclaringEntityType.RemoveNavigation(navigation.Name);
			}
			else
			{
				PrincipalEntityType.RemoveNavigation(navigation.Name);
			}
		}
		Navigation navigation2 = null;
		MemberInfo memberInfo = propertyIdentity?.MemberInfo;
		if (memberInfo != null)
		{
			navigation2 = (pointsToPrincipal ? DeclaringEntityType.AddNavigation(memberInfo, this, pointsToPrincipal: true) : PrincipalEntityType.AddNavigation(memberInfo, this, pointsToPrincipal: false));
		}
		else if (text != null)
		{
			navigation2 = (pointsToPrincipal ? DeclaringEntityType.AddNavigation(text, this, pointsToPrincipal: true) : PrincipalEntityType.AddNavigation(text, this, pointsToPrincipal: false));
		}
		if (pointsToPrincipal)
		{
			DependentToPrincipal = navigation2;
			UpdateDependentToPrincipalConfigurationSource(configurationSource);
		}
		else
		{
			PrincipalToDependent = navigation2;
			UpdatePrincipalToDependentConfigurationSource(configurationSource);
		}
		if (navigation != null)
		{
			if (pointsToPrincipal)
			{
				DeclaringEntityType.Model.ConventionDispatcher.OnNavigationRemoved(DeclaringEntityType.Builder, PrincipalEntityType.Builder, navigation.Name, navigation.GetIdentifyingMemberInfo());
			}
			else
			{
				DeclaringEntityType.Model.ConventionDispatcher.OnNavigationRemoved(PrincipalEntityType.Builder, DeclaringEntityType.Builder, navigation.Name, navigation.GetIdentifyingMemberInfo());
			}
		}
		if (navigation2 != null)
		{
			navigation2 = (Navigation)DeclaringEntityType.Model.ConventionDispatcher.OnNavigationAdded(Builder, navigation2);
		}
		return navigation2 ?? navigation;
	}

	public virtual ForeignKey SetIsUnique(bool? unique, ConfigurationSource configurationSource)
	{
		bool isUnique = IsUnique;
		_isUnique = unique;
		if (!unique.HasValue)
		{
			_isUniqueConfigurationSource = null;
		}
		else
		{
			UpdateIsUniqueConfigurationSource(configurationSource);
		}
		if (IsUnique == isUnique)
		{
			return this;
		}
		return (ForeignKey)(DeclaringEntityType.Model.ConventionDispatcher.OnForeignKeyUniquenessChanged(Builder)?.Metadata);
	}

	public virtual ConfigurationSource? GetIsUniqueConfigurationSource()
	{
		return _isUniqueConfigurationSource;
	}

	public virtual void UpdateIsUniqueConfigurationSource(ConfigurationSource configurationSource)
	{
		_isUniqueConfigurationSource = configurationSource.Max(_isUniqueConfigurationSource);
	}

	public virtual ForeignKey SetIsRequired(bool? required, ConfigurationSource configurationSource)
	{
		bool isRequired = IsRequired;
		_isRequired = required ?? DefaultIsRequired;
		if (!required.HasValue)
		{
			_isRequiredConfigurationSource = null;
		}
		else
		{
			UpdateIsRequiredConfigurationSource(configurationSource);
		}
		if (isRequired == IsRequired)
		{
			return this;
		}
		return (ForeignKey)(DeclaringEntityType.Model.ConventionDispatcher.OnForeignKeyRequirednessChanged(Builder)?.Metadata);
	}

	public virtual ConfigurationSource? GetIsRequiredConfigurationSource()
	{
		return _isRequiredConfigurationSource;
	}

	public virtual void SetIsRequiredConfigurationSource(ConfigurationSource? configurationSource)
	{
		_isRequiredConfigurationSource = configurationSource;
	}

	public virtual void UpdateIsRequiredConfigurationSource(ConfigurationSource configurationSource)
	{
		_isRequiredConfigurationSource = configurationSource.Max(_isRequiredConfigurationSource);
	}

	public virtual ForeignKey SetDeleteBehavior(DeleteBehavior? deleteBehavior, ConfigurationSource configurationSource)
	{
		_deleteBehavior = deleteBehavior;
		if (!deleteBehavior.HasValue)
		{
			_deleteBehaviorConfigurationSource = null;
		}
		else
		{
			UpdateDeleteBehaviorConfigurationSource(configurationSource);
		}
		return this;
	}

	public virtual ConfigurationSource? GetDeleteBehaviorConfigurationSource()
	{
		return _deleteBehaviorConfigurationSource;
	}

	public virtual void UpdateDeleteBehaviorConfigurationSource(ConfigurationSource configurationSource)
	{
		_deleteBehaviorConfigurationSource = configurationSource.Max(_deleteBehaviorConfigurationSource);
	}

	public virtual ForeignKey SetIsOwnership(bool? ownership, ConfigurationSource configurationSource)
	{
		bool isOwnership = IsOwnership;
		_isOwnership = ownership;
		if (!_isOwnership.HasValue)
		{
			_isOwnershipConfigurationSource = null;
		}
		else
		{
			UpdateIsOwnershipConfigurationSource(configurationSource);
		}
		if (IsOwnership == isOwnership)
		{
			return this;
		}
		return (ForeignKey)(DeclaringEntityType.Model.ConventionDispatcher.OnForeignKeyOwnershipChanged(Builder)?.Metadata);
	}

	public virtual ConfigurationSource? GetIsOwnershipConfigurationSource()
	{
		return _isOwnershipConfigurationSource;
	}

	public virtual void UpdateIsOwnershipConfigurationSource(ConfigurationSource configurationSource)
	{
		_isOwnershipConfigurationSource = configurationSource.Max(_isOwnershipConfigurationSource);
	}

	public virtual IEnumerable<Navigation> FindNavigationsFromInHierarchy([JetBrains.Annotations.NotNull] EntityType entityType)
	{
		return ForeignKeyExtensions.FindNavigationsFromInHierarchy(this, entityType).Cast<Navigation>();
	}

	public virtual IEnumerable<Navigation> FindNavigationsTo([JetBrains.Annotations.NotNull] EntityType entityType)
	{
		return ForeignKeyExtensions.FindNavigationsTo(this, entityType).Cast<Navigation>();
	}

	public virtual EntityType ResolveOtherEntityType([JetBrains.Annotations.NotNull] EntityType entityType)
	{
		return (EntityType)Microsoft.EntityFrameworkCore.ForeignKeyExtensions.GetRelatedEntityType(this, entityType);
	}

	void IMutableForeignKey.SetProperties(IReadOnlyList<IMutableProperty> properties, IMutableKey principalKey)
	{
		SetProperties((IReadOnlyList<Property>)properties, (Key)principalKey, ConfigurationSource.Explicit);
	}

	IMutableNavigation IMutableForeignKey.HasDependentToPrincipal(string name)
	{
		return HasDependentToPrincipal(name, ConfigurationSource.Explicit);
	}

	IMutableNavigation IMutableForeignKey.HasDependentToPrincipal(MemberInfo property)
	{
		return HasDependentToPrincipal(property, ConfigurationSource.Explicit);
	}

	IMutableNavigation IMutableForeignKey.HasPrincipalToDependent(string name)
	{
		return HasPrincipalToDependent(name, ConfigurationSource.Explicit);
	}

	IMutableNavigation IMutableForeignKey.HasPrincipalToDependent(MemberInfo property)
	{
		return HasPrincipalToDependent(property, ConfigurationSource.Explicit);
	}

	void IConventionForeignKey.SetProperties(IReadOnlyList<IConventionProperty> properties, IConventionKey principalKey, bool fromDataAnnotation)
	{
		SetProperties((IReadOnlyList<Property>)properties, (Key)principalKey, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionNavigation IConventionForeignKey.HasDependentToPrincipal(string name, bool fromDataAnnotation)
	{
		return HasDependentToPrincipal(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionNavigation IConventionForeignKey.HasDependentToPrincipal(MemberInfo property, bool fromDataAnnotation)
	{
		return HasDependentToPrincipal(property, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionNavigation IConventionForeignKey.HasPrincipalToDependent(string name, bool fromDataAnnotation)
	{
		return HasPrincipalToDependent(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionNavigation IConventionForeignKey.HasPrincipalToDependent(MemberInfo property, bool fromDataAnnotation)
	{
		return HasPrincipalToDependent(property, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionForeignKey.SetIsUnique(bool? unique, bool fromDataAnnotation)
	{
		SetIsUnique(unique, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionForeignKey.SetIsRequired(bool? required, bool fromDataAnnotation)
	{
		SetIsRequired(required, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionForeignKey.SetIsOwnership(bool? ownership, bool fromDataAnnotation)
	{
		SetIsOwnership(ownership, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionForeignKey.SetDeleteBehavior(DeleteBehavior? deleteBehavior, bool fromDataAnnotation)
	{
		SetDeleteBehavior(deleteBehavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public override string ToString()
	{
		return this.ToDebugString();
	}

	private void Validate(IReadOnlyList<Property> properties, Key principalKey, EntityType declaringEntityType, EntityType principalEntityType)
	{
		for (int i = 0; i < properties.Count; i++)
		{
			Property property = properties[i];
			for (int j = i + 1; j < properties.Count; j++)
			{
				if (property == properties[j])
				{
					throw new InvalidOperationException(CoreStrings.DuplicatePropertyInList(properties.Format(), property.Name));
				}
			}
			Property property2 = declaringEntityType.FindProperty(property.Name);
			if (property2 == null || !property2.DeclaringEntityType.IsAssignableFrom(property.DeclaringEntityType) || property.Builder == null)
			{
				throw new InvalidOperationException(CoreStrings.ForeignKeyPropertiesWrongEntity(properties.Format(), declaringEntityType.DisplayName()));
			}
		}
		AreCompatible(principalEntityType, declaringEntityType, null, null, properties, principalKey.Properties, null, shouldThrow: true);
		ForeignKey foreignKey = declaringEntityType.FindForeignKeysInHierarchy(properties, principalKey, principalEntityType).FirstOrDefault();
		if (foreignKey != null)
		{
			throw new InvalidOperationException(CoreStrings.DuplicateForeignKey(properties.Format(), declaringEntityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), principalKey.Properties.Format(), principalEntityType.DisplayName()));
		}
		if (principalEntityType.Model != declaringEntityType.Model)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeModelMismatch(declaringEntityType.DisplayName(), principalEntityType.DisplayName()));
		}
	}

	public static bool AreCompatible([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToPrincipal, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToDependent, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> principalProperties, bool? unique, bool shouldThrow)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityType, "dependentEntityType");
		if (principalEntityType.HasDefiningNavigation() && principalEntityType == dependentEntityType)
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.ForeignKeySelfReferencingDependentEntityType(dependentEntityType.DisplayName()));
			}
			return false;
		}
		if (navigationToPrincipal != null && !Microsoft.EntityFrameworkCore.Metadata.Internal.Navigation.IsCompatible(navigationToPrincipal, dependentEntityType.ClrType, principalEntityType.ClrType, false, shouldThrow))
		{
			return false;
		}
		if (navigationToDependent != null && !Microsoft.EntityFrameworkCore.Metadata.Internal.Navigation.IsCompatible(navigationToDependent, principalEntityType.ClrType, dependentEntityType.ClrType, !unique, shouldThrow))
		{
			return false;
		}
		if (principalProperties != null && dependentProperties != null)
		{
			return AreCompatible(principalProperties, dependentProperties, principalEntityType, dependentEntityType, shouldThrow);
		}
		return true;
	}

	public static bool AreCompatible([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> principalProperties, [JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> dependentProperties, [JetBrains.Annotations.NotNull] IEntityType principalEntityType, [JetBrains.Annotations.NotNull] IEntityType dependentEntityType, bool shouldThrow)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalProperties, "principalProperties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentProperties, "dependentProperties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityType, "dependentEntityType");
		if (!ArePropertyCountsEqual(principalProperties, dependentProperties))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.ForeignKeyCountMismatch(dependentProperties.Format(), dependentEntityType.DisplayName(), principalProperties.Format(), principalEntityType.DisplayName()));
			}
			return false;
		}
		if (!ArePropertyTypesCompatible(principalProperties, dependentProperties))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.ForeignKeyTypeMismatch(dependentProperties.Format(), dependentEntityType.DisplayName(), principalProperties.Format(), principalEntityType.DisplayName()));
			}
			return false;
		}
		return true;
	}

	private static bool ArePropertyCountsEqual(IReadOnlyList<IProperty> principalProperties, IReadOnlyList<IProperty> dependentProperties)
	{
		return principalProperties.Count == dependentProperties.Count;
	}

	private static bool ArePropertyTypesCompatible(IReadOnlyList<IProperty> principalProperties, IReadOnlyList<IProperty> dependentProperties)
	{
		return principalProperties.Select((IProperty p) => p.ClrType.UnwrapNullableType()).SequenceEqual(dependentProperties.Select((IProperty p) => p.ClrType.UnwrapNullableType()));
	}
}
