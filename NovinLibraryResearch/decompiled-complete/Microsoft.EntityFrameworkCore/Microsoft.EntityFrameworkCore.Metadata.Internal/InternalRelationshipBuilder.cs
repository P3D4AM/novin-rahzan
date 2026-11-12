using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalRelationshipBuilder : InternalModelItemBuilder<ForeignKey>, IConventionRelationshipBuilder, IConventionAnnotatableBuilder
{
	[Flags]
	private enum Resolution
	{
		None = 0,
		Remove = 1,
		ResetToPrincipal = 2,
		ResetToDependent = 4,
		ResetDependentProperties = 8
	}

	IConventionForeignKey IConventionRelationshipBuilder.Metadata
	{
		[DebuggerStepThrough]
		get
		{
			return Metadata;
		}
	}

	public InternalRelationshipBuilder([JetBrains.Annotations.NotNull] ForeignKey foreignKey, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(foreignKey, modelBuilder)
	{
	}

	public virtual InternalRelationshipBuilder HasNavigation([JetBrains.Annotations.CanBeNull] string name, bool pointsToPrincipal, ConfigurationSource configurationSource)
	{
		if (!pointsToPrincipal)
		{
			return HasNavigations(null, MemberIdentity.Create(name), configurationSource);
		}
		return HasNavigations(MemberIdentity.Create(name), null, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasNavigation([JetBrains.Annotations.CanBeNull] MemberInfo property, bool pointsToPrincipal, ConfigurationSource configurationSource)
	{
		if (!pointsToPrincipal)
		{
			return HasNavigations(null, MemberIdentity.Create(property), configurationSource);
		}
		return HasNavigations(MemberIdentity.Create(property), null, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasNavigations([JetBrains.Annotations.CanBeNull] string navigationToPrincipalName, [JetBrains.Annotations.CanBeNull] string navigationToDependentName, ConfigurationSource configurationSource)
	{
		return HasNavigations(MemberIdentity.Create(navigationToPrincipalName), MemberIdentity.Create(navigationToDependentName), configurationSource);
	}

	public virtual InternalRelationshipBuilder HasNavigations([JetBrains.Annotations.CanBeNull] MemberInfo navigationToPrincipal, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToDependent, ConfigurationSource configurationSource)
	{
		return HasNavigations(MemberIdentity.Create(navigationToPrincipal), MemberIdentity.Create(navigationToDependent), configurationSource);
	}

	public virtual InternalRelationshipBuilder HasNavigations(MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, ConfigurationSource configurationSource)
	{
		return HasNavigations(navigationToPrincipal, navigationToDependent, Metadata.PrincipalEntityType, Metadata.DeclaringEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasNavigations([JetBrains.Annotations.CanBeNull] string navigationToPrincipalName, [JetBrains.Annotations.CanBeNull] string navigationToDependentName, [JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		return HasNavigations(MemberIdentity.Create(navigationToPrincipalName), MemberIdentity.Create(navigationToDependentName), principalEntityType, dependentEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasNavigations([JetBrains.Annotations.CanBeNull] MemberInfo navigationToPrincipal, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToDependent, [JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		return HasNavigations(MemberIdentity.Create(navigationToPrincipal), MemberIdentity.Create(navigationToDependent), principalEntityType, dependentEntityType, configurationSource);
	}

	private InternalRelationshipBuilder HasNavigations(MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, EntityType principalEntityType, EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		string text = navigationToPrincipal?.Name;
		string text2 = navigationToDependent?.Name;
		if ((!navigationToPrincipal.HasValue || navigationToPrincipal.Value.Name == Metadata.DependentToPrincipal?.Name) && (!navigationToDependent.HasValue || navigationToDependent.Value.Name == Metadata.PrincipalToDependent?.Name))
		{
			Metadata.UpdateConfigurationSource(configurationSource);
			if (navigationToPrincipal.HasValue)
			{
				Metadata.UpdateDependentToPrincipalConfigurationSource(configurationSource);
				if (text != null)
				{
					principalEntityType.RemoveIgnored(text);
				}
			}
			if (navigationToDependent.HasValue)
			{
				Metadata.UpdatePrincipalToDependentConfigurationSource(configurationSource);
				if (text2 != null)
				{
					principalEntityType.RemoveIgnored(text2);
				}
			}
			return this;
		}
		bool shouldThrow = configurationSource == ConfigurationSource.Explicit;
		if (text != null && navigationToPrincipal.Value.MemberInfo == null && dependentEntityType.HasClrType())
		{
			MemberInfo memberInfo = FindCompatibleClrMember(text, dependentEntityType, principalEntityType, shouldThrow);
			if (memberInfo != null)
			{
				navigationToPrincipal = MemberIdentity.Create(memberInfo);
			}
		}
		if (text2 != null && navigationToDependent.Value.MemberInfo == null && principalEntityType.HasClrType())
		{
			MemberInfo memberInfo2 = FindCompatibleClrMember(text2, principalEntityType, dependentEntityType, shouldThrow);
			if (memberInfo2 != null)
			{
				navigationToDependent = MemberIdentity.Create(memberInfo2);
			}
		}
		if (!CanSetNavigations(navigationToPrincipal, navigationToDependent, principalEntityType, dependentEntityType, configurationSource, shouldThrow, overrideSameSource: true, out var shouldInvert, out var shouldBeUnique, out var removeOppositeNavigation, out var removeConflictingNavigations, out var changeRelatedTypes))
		{
			return null;
		}
		if (removeOppositeNavigation)
		{
			if (!navigationToPrincipal.HasValue)
			{
				navigationToPrincipal = MemberIdentity.None;
			}
			if (!navigationToDependent.HasValue)
			{
				navigationToDependent = MemberIdentity.None;
			}
		}
		IReadOnlyList<Property> dependentProperties = null;
		IReadOnlyList<Property> principalProperties = null;
		if (shouldInvert == true)
		{
			EntityType entityType = principalEntityType;
			principalEntityType = dependentEntityType;
			dependentEntityType = entityType;
			MemberIdentity? memberIdentity = navigationToPrincipal;
			navigationToPrincipal = navigationToDependent;
			navigationToDependent = memberIdentity;
			if (Metadata.GetPropertiesConfigurationSource() == configurationSource)
			{
				dependentProperties = Array.Empty<Property>();
			}
			if (Metadata.GetPrincipalKeyConfigurationSource() == configurationSource)
			{
				principalProperties = Array.Empty<Property>();
			}
		}
		InternalRelationshipBuilder internalRelationshipBuilder;
		if (shouldInvert == true || removeConflictingNavigations || changeRelatedTypes)
		{
			internalRelationshipBuilder = ReplaceForeignKey(configurationSource, principalEntityType.Builder, dependentEntityType.Builder, navigationToPrincipal, navigationToDependent, dependentProperties, null, principalProperties, shouldBeUnique, null, null, null, shouldInvert == true || changeRelatedTypes, shouldInvert.HasValue ? new ConfigurationSource?(configurationSource) : null, shouldInvert == true);
			if (internalRelationshipBuilder == null)
			{
				return null;
			}
		}
		else
		{
			using IConventionBatch batch = Metadata.DeclaringEntityType.Model.ConventionDispatcher.DelayConventions();
			internalRelationshipBuilder = this;
			Metadata.UpdateConfigurationSource(configurationSource);
			if (shouldBeUnique.HasValue)
			{
				IsUnique(shouldBeUnique.Value, configurationSource);
			}
			else
			{
				IsUnique(null, ConfigurationSource.Convention);
			}
			if (navigationToPrincipal.HasValue)
			{
				if (navigationToDependent.HasValue)
				{
					Metadata.HasPrincipalToDependent((string)null, configurationSource);
				}
				MemberInfo memberInfo3 = navigationToPrincipal.Value.MemberInfo;
				if (text != null)
				{
					Metadata.DeclaringEntityType.RemoveIgnored(text);
					if (Metadata.DeclaringEntityType.ClrType != null && memberInfo3 == null)
					{
						throw new InvalidOperationException(CoreStrings.NoClrNavigation(text, Metadata.DeclaringEntityType.DisplayName()));
					}
				}
				if (memberInfo3 != null)
				{
					Metadata.HasDependentToPrincipal(memberInfo3, configurationSource);
				}
				else
				{
					Metadata.HasDependentToPrincipal(text, configurationSource);
				}
			}
			if (navigationToDependent.HasValue)
			{
				MemberInfo memberInfo4 = navigationToDependent.Value.MemberInfo;
				if (text2 != null)
				{
					Metadata.PrincipalEntityType.RemoveIgnored(text2);
					if (Metadata.DeclaringEntityType.ClrType != null && memberInfo4 == null)
					{
						throw new InvalidOperationException(CoreStrings.NoClrNavigation(text2, Metadata.PrincipalEntityType.DisplayName()));
					}
				}
				if (memberInfo4 != null)
				{
					Metadata.HasPrincipalToDependent(memberInfo4, configurationSource);
				}
				else
				{
					Metadata.HasPrincipalToDependent(text2, configurationSource);
				}
			}
			internalRelationshipBuilder = batch.Run(internalRelationshipBuilder);
		}
		if (internalRelationshipBuilder == null || ((!navigationToPrincipal.HasValue || !(internalRelationshipBuilder.Metadata.DependentToPrincipal?.Name != navigationToPrincipal.Value.Name)) && (!navigationToDependent.HasValue || !(internalRelationshipBuilder.Metadata.PrincipalToDependent?.Name != navigationToDependent.Value.Name))) || ((!navigationToDependent.HasValue || !(internalRelationshipBuilder.Metadata.DependentToPrincipal?.Name != navigationToDependent.Value.Name)) && (!navigationToPrincipal.HasValue || !(internalRelationshipBuilder.Metadata.PrincipalToDependent?.Name != navigationToPrincipal.Value.Name))))
		{
			return internalRelationshipBuilder;
		}
		return null;
	}

	private static MemberInfo FindCompatibleClrMember(string navigationName, EntityType sourceType, EntityType targetType, bool shouldThrow)
	{
		MemberInfo memberInfo = sourceType.FindClrMember(navigationName);
		if (Navigation.IsCompatible(navigationName, memberInfo, sourceType, targetType, null, shouldThrow))
		{
			return memberInfo;
		}
		return null;
	}

	public virtual bool CanSetNavigation([JetBrains.Annotations.CanBeNull] MemberInfo property, bool pointsToPrincipal, ConfigurationSource? configurationSource)
	{
		bool? shouldBeUnique;
		bool removeOppositeNavigation;
		bool removeConflictingNavigations;
		return CanSetNavigation(MemberIdentity.Create(property), pointsToPrincipal, configurationSource, shouldThrow: false, overrideSameSource: true, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations);
	}

	public virtual bool CanSetNavigation([JetBrains.Annotations.CanBeNull] string name, bool pointsToPrincipal, ConfigurationSource? configurationSource)
	{
		bool? shouldBeUnique;
		bool removeOppositeNavigation;
		bool removeConflictingNavigations;
		return CanSetNavigation(MemberIdentity.Create(name), pointsToPrincipal, configurationSource, shouldThrow: false, overrideSameSource: true, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations);
	}

	private bool CanSetNavigation(string name, bool pointsToPrincipal, ConfigurationSource? configurationSource, bool overrideSameSource)
	{
		bool? shouldBeUnique;
		bool removeOppositeNavigation;
		bool removeConflictingNavigations;
		return CanSetNavigation(MemberIdentity.Create(name), pointsToPrincipal, configurationSource, shouldThrow: false, overrideSameSource, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations);
	}

	private bool CanSetNavigation(MemberIdentity navigation, bool pointsToPrincipal, ConfigurationSource? configurationSource, bool shouldThrow, bool overrideSameSource, out bool? shouldBeUnique, out bool removeOppositeNavigation, out bool removeConflictingNavigations)
	{
		bool? shouldInvert;
		if (!pointsToPrincipal)
		{
			return CanSetNavigations(null, navigation, configurationSource, shouldThrow, overrideSameSource, out shouldInvert, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations);
		}
		return CanSetNavigations(navigation, null, configurationSource, shouldThrow, overrideSameSource, out shouldInvert, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations);
	}

	public virtual bool CanSetNavigations([JetBrains.Annotations.CanBeNull] MemberInfo navigationToPrincipal, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToDependent, ConfigurationSource? configurationSource)
	{
		bool? shouldInvert;
		bool? shouldBeUnique;
		bool removeOppositeNavigation;
		bool removeConflictingNavigations;
		return CanSetNavigations(MemberIdentity.Create(navigationToPrincipal), MemberIdentity.Create(navigationToDependent), configurationSource, shouldThrow: false, overrideSameSource: true, out shouldInvert, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations);
	}

	public virtual bool CanSetNavigations([JetBrains.Annotations.CanBeNull] string navigationToPrincipalName, [JetBrains.Annotations.CanBeNull] string navigationToDependentName, ConfigurationSource? configurationSource)
	{
		bool? shouldInvert;
		bool? shouldBeUnique;
		bool removeOppositeNavigation;
		bool removeConflictingNavigations;
		return CanSetNavigations(MemberIdentity.Create(navigationToPrincipalName), MemberIdentity.Create(navigationToDependentName), configurationSource, shouldThrow: false, overrideSameSource: true, out shouldInvert, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations);
	}

	private bool CanSetNavigations(MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, ConfigurationSource? configurationSource, bool shouldThrow, bool overrideSameSource, out bool? shouldInvert, out bool? shouldBeUnique, out bool removeOppositeNavigation, out bool removeConflictingNavigations)
	{
		bool changeRelatedTypes;
		return CanSetNavigations(navigationToPrincipal, navigationToDependent, Metadata.PrincipalEntityType, Metadata.DeclaringEntityType, configurationSource, shouldThrow, overrideSameSource, out shouldInvert, out shouldBeUnique, out removeOppositeNavigation, out removeConflictingNavigations, out changeRelatedTypes);
	}

	private bool CanSetNavigations(MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, EntityType principalEntityType, EntityType dependentEntityType, ConfigurationSource? configurationSource, bool shouldThrow, bool overrideSameSource, out bool? shouldInvert, out bool? shouldBeUnique, out bool removeOppositeNavigation, out bool removeConflictingNavigations, out bool changeRelatedTypes)
	{
		shouldInvert = null;
		shouldBeUnique = null;
		removeOppositeNavigation = false;
		removeConflictingNavigations = false;
		changeRelatedTypes = false;
		if ((!navigationToPrincipal.HasValue || navigationToPrincipal.Value.Name == Metadata.DependentToPrincipal?.Name) && (!navigationToDependent.HasValue || navigationToDependent.Value.Name == Metadata.PrincipalToDependent?.Name))
		{
			return true;
		}
		if (!configurationSource.HasValue)
		{
			return false;
		}
		string text = navigationToPrincipal?.Name;
		if (navigationToPrincipal.HasValue && text != Metadata.DependentToPrincipal?.Name)
		{
			if (!configurationSource.Value.Overrides(Metadata.GetDependentToPrincipalConfigurationSource()) || (!overrideSameSource && configurationSource == Metadata.GetDependentToPrincipalConfigurationSource()))
			{
				return false;
			}
			if (text != null)
			{
				if (dependentEntityType.Builder.IsIgnored(text, configurationSource))
				{
					return false;
				}
				if (!navigationToDependent.HasValue && text == Metadata.PrincipalToDependent?.Name && Metadata.IsIntraHierarchical())
				{
					if (!configurationSource.Value.Overrides(Metadata.GetPrincipalToDependentConfigurationSource()) || (!overrideSameSource && configurationSource == Metadata.GetPrincipalToDependentConfigurationSource()))
					{
						return false;
					}
					removeOppositeNavigation = true;
				}
			}
		}
		string text2 = navigationToDependent?.Name;
		if (navigationToDependent.HasValue && text2 != Metadata.PrincipalToDependent?.Name)
		{
			if (!configurationSource.Value.Overrides(Metadata.GetPrincipalToDependentConfigurationSource()) || (!overrideSameSource && configurationSource == Metadata.GetPrincipalToDependentConfigurationSource()))
			{
				return false;
			}
			if (text2 != null)
			{
				if (principalEntityType.Builder.IsIgnored(text2, configurationSource))
				{
					return false;
				}
				if (!navigationToPrincipal.HasValue && text2 == Metadata.DependentToPrincipal?.Name && Metadata.IsIntraHierarchical())
				{
					if (!configurationSource.Value.Overrides(Metadata.GetDependentToPrincipalConfigurationSource()) || (!overrideSameSource && configurationSource == Metadata.GetDependentToPrincipalConfigurationSource()))
					{
						return false;
					}
					removeOppositeNavigation = true;
				}
			}
		}
		MemberInfo memberInfo = navigationToPrincipal?.MemberInfo;
		MemberInfo memberInfo2 = navigationToDependent?.MemberInfo;
		bool? shouldBeUnique2 = null;
		if (memberInfo != null && !IsCompatible(memberInfo, pointsToPrincipal: false, principalEntityType.ClrType, dependentEntityType.ClrType, shouldThrow: false, out shouldBeUnique2))
		{
			shouldInvert = false;
		}
		if (memberInfo2 != null && !IsCompatible(memberInfo2, pointsToPrincipal: true, principalEntityType.ClrType, dependentEntityType.ClrType, shouldThrow: false, out var shouldBeUnique3))
		{
			shouldInvert = false;
		}
		if (memberInfo != null && !IsCompatible(memberInfo, pointsToPrincipal: true, dependentEntityType.ClrType, principalEntityType.ClrType, shouldThrow && shouldInvert.HasValue, out shouldBeUnique3))
		{
			if (shouldInvert.HasValue)
			{
				return false;
			}
			shouldInvert = true;
		}
		if (memberInfo2 != null && !IsCompatible(memberInfo2, pointsToPrincipal: false, dependentEntityType.ClrType, principalEntityType.ClrType, shouldThrow && shouldInvert.HasValue, out shouldBeUnique))
		{
			if (shouldInvert.HasValue)
			{
				return false;
			}
			shouldInvert = true;
		}
		if (shouldInvert == true)
		{
			shouldBeUnique = shouldBeUnique2;
		}
		if (shouldBeUnique.HasValue && Metadata.IsUnique != shouldBeUnique && !configurationSource.Overrides(Metadata.GetIsUniqueConfigurationSource()))
		{
			return false;
		}
		if (!shouldBeUnique.HasValue && (Metadata.IsUnique || configurationSource.OverridesStrictly(Metadata.GetIsUniqueConfigurationSource())) && ((memberInfo2 != null && shouldInvert != true) || (memberInfo != null && shouldInvert == true)))
		{
			shouldBeUnique = true;
		}
		removeConflictingNavigations = (from r in FindRelationships(principalEntityType, dependentEntityType, navigationToPrincipal, navigationToDependent, null, null)
			where r.Metadata != Metadata
			select r).Distinct().Any();
		shouldBeUnique3 = shouldInvert;
		if (shouldBeUnique3 == false && !removeConflictingNavigations && (principalEntityType != Metadata.PrincipalEntityType || dependentEntityType != Metadata.DeclaringEntityType))
		{
			if (memberInfo != null && !IsCompatible(memberInfo, pointsToPrincipal: true, Metadata.DeclaringEntityType.ClrType, Metadata.PrincipalEntityType.ClrType, shouldThrow: false, out shouldBeUnique3))
			{
				changeRelatedTypes = true;
				return true;
			}
			if (memberInfo2 != null && !IsCompatible(memberInfo2, pointsToPrincipal: false, Metadata.DeclaringEntityType.ClrType, Metadata.PrincipalEntityType.ClrType, shouldThrow: false, out shouldBeUnique3))
			{
				changeRelatedTypes = true;
				return true;
			}
		}
		return true;
	}

	private static bool IsCompatible([JetBrains.Annotations.NotNull] MemberInfo navigationProperty, bool pointsToPrincipal, [JetBrains.Annotations.NotNull] Type dependentType, [JetBrains.Annotations.NotNull] Type principalType, bool shouldThrow, out bool? shouldBeUnique)
	{
		shouldBeUnique = null;
		if (!pointsToPrincipal)
		{
			bool flag = Navigation.IsCompatible(navigationProperty, principalType, dependentType, false, shouldThrow: false);
			bool flag2 = Navigation.IsCompatible(navigationProperty, principalType, dependentType, true, shouldThrow: false);
			if (flag != flag2)
			{
				shouldBeUnique = flag;
			}
			else if (!flag)
			{
				if (shouldThrow)
				{
					Navigation.IsCompatible(navigationProperty, principalType, dependentType, null, shouldThrow: true);
				}
				return false;
			}
		}
		else if (!Navigation.IsCompatible(navigationProperty, dependentType, principalType, false, shouldThrow))
		{
			return false;
		}
		return true;
	}

	public virtual InternalRelationshipBuilder HasField([JetBrains.Annotations.CanBeNull] string fieldName, bool pointsToPrincipal, ConfigurationSource configurationSource)
	{
		Navigation navigation = (pointsToPrincipal ? Metadata.DependentToPrincipal : Metadata.PrincipalToDependent);
		if (navigation == null)
		{
			throw new InvalidOperationException(CoreStrings.NoNavigation(pointsToPrincipal ? Metadata.DeclaringEntityType.DisplayName() : Metadata.PrincipalEntityType.DisplayName(), Metadata.Properties.Format()));
		}
		if (navigation.FieldInfo?.GetSimpleMemberName() == fieldName || configurationSource.Overrides(navigation.GetFieldInfoConfigurationSource()))
		{
			navigation.SetField(fieldName, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetField([JetBrains.Annotations.CanBeNull] string fieldName, bool pointsToPrincipal, ConfigurationSource? configurationSource)
	{
		Navigation navigation = (pointsToPrincipal ? Metadata.DependentToPrincipal : Metadata.PrincipalToDependent);
		if (navigation == null)
		{
			return false;
		}
		if (fieldName != null && configurationSource.Overrides(navigation.GetFieldInfoConfigurationSource()))
		{
			FieldInfo fieldInfo = PropertyBase.GetFieldInfo(fieldName, navigation.DeclaringEntityType, navigation.Name, shouldThrow: false);
			if (fieldInfo != null)
			{
				return PropertyBase.IsCompatible(fieldInfo, navigation.ClrType, navigation.DeclaringType.ClrType, navigation.Name, shouldThrow: false);
			}
			return false;
		}
		return navigation.FieldInfo?.GetSimpleMemberName() == fieldName;
	}

	public virtual InternalRelationshipBuilder HasField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool pointsToPrincipal, ConfigurationSource configurationSource)
	{
		Navigation navigation = (pointsToPrincipal ? Metadata.DependentToPrincipal : Metadata.PrincipalToDependent);
		if (navigation == null)
		{
			throw new InvalidOperationException(CoreStrings.NoNavigation(pointsToPrincipal ? Metadata.DeclaringEntityType.DisplayName() : Metadata.PrincipalEntityType.DisplayName(), Metadata.Properties.Format()));
		}
		if (configurationSource.Overrides(navigation.GetFieldInfoConfigurationSource()) || object.Equals(navigation.FieldInfo, fieldInfo))
		{
			navigation.SetField(fieldInfo, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool pointsToPrincipal, ConfigurationSource? configurationSource)
	{
		Navigation navigation = (pointsToPrincipal ? Metadata.DependentToPrincipal : Metadata.PrincipalToDependent);
		if (navigation != null)
		{
			if (!configurationSource.Overrides(navigation.GetFieldInfoConfigurationSource()) || (!(fieldInfo == null) && !PropertyBase.IsCompatible(fieldInfo, navigation.ClrType, Metadata.DeclaringEntityType.ClrType, navigation.Name, shouldThrow: false)))
			{
				return object.Equals(navigation.FieldInfo, fieldInfo);
			}
			return true;
		}
		return false;
	}

	public virtual InternalRelationshipBuilder IsEagerLoaded(bool? eagerLoaded, bool pointsToPrincipal, ConfigurationSource configurationSource)
	{
		if (CanSetIsEagerLoaded(eagerLoaded, pointsToPrincipal, configurationSource))
		{
			(pointsToPrincipal ? Metadata.DependentToPrincipal : Metadata.PrincipalToDependent).SetIsEagerLoaded(eagerLoaded, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetIsEagerLoaded(bool? eagerLoaded, bool pointsToPrincipal, ConfigurationSource? configurationSource)
	{
		Navigation navigation = (pointsToPrincipal ? Metadata.DependentToPrincipal : Metadata.PrincipalToDependent);
		if (navigation != null)
		{
			if (!configurationSource.Overrides(navigation.GetIsEagerLoadedConfigurationSource()))
			{
				return navigation.IsEagerLoaded() == eagerLoaded;
			}
			return true;
		}
		return false;
	}

	public virtual InternalRelationshipBuilder IsRequired(bool? required, ConfigurationSource configurationSource)
	{
		if (!CanSetIsRequired(required, configurationSource))
		{
			return null;
		}
		return Metadata.SetIsRequired(required, configurationSource)?.Builder;
	}

	public virtual bool CanSetIsRequired(bool? required, ConfigurationSource? configurationSource)
	{
		if (Metadata.IsRequired != required)
		{
			return configurationSource.Overrides(Metadata.GetIsRequiredConfigurationSource());
		}
		return true;
	}

	public virtual InternalRelationshipBuilder IsOwnership(bool? ownership, ConfigurationSource configurationSource)
	{
		if (Metadata.IsOwnership == ownership)
		{
			return Metadata.SetIsOwnership(ownership, configurationSource).Builder;
		}
		if (!ownership.HasValue || !configurationSource.Overrides(Metadata.GetIsOwnershipConfigurationSource()))
		{
			return null;
		}
		EntityType declaringEntityType = Metadata.DeclaringEntityType;
		if (ownership.Value)
		{
			ForeignKey foreignKey = declaringEntityType.GetDeclaredForeignKeys().SingleOrDefault((ForeignKey fk) => fk.IsOwnership);
			List<ForeignKey> list = (from fk in declaringEntityType.GetDeclaredReferencingForeignKeys()
				where fk.IsOwnership && fk.DeclaringEntityType.ClrType == Metadata.PrincipalEntityType.ClrType
				select fk).ToList();
			if (list.Any((ForeignKey fk) => !configurationSource.Overrides(fk.GetConfigurationSource())))
			{
				return null;
			}
			InternalRelationshipBuilder internalRelationshipBuilder;
			if (declaringEntityType.HasDefiningNavigation())
			{
				if (foreignKey != null && !configurationSource.Overrides(foreignKey.GetConfigurationSource()))
				{
					return null;
				}
				internalRelationshipBuilder = (Metadata.SetIsOwnership(true, configurationSource)?.Builder)?.OnDelete(DeleteBehavior.Cascade, ConfigurationSource.Convention);
				if (internalRelationshipBuilder == null)
				{
					return null;
				}
				if (foreignKey?.Builder != null)
				{
					foreignKey.DeclaringEntityType.Builder.HasNoRelationship(foreignKey, configurationSource);
				}
				{
					foreach (ForeignKey item in list)
					{
						if (item.Builder != null)
						{
							item.DeclaringEntityType.Builder.HasNoRelationship(item, configurationSource);
						}
					}
					return internalRelationshipBuilder;
				}
			}
			if (foreignKey != null)
			{
				if (!Metadata.GetConfigurationSource().Overrides(ConfigurationSource.Explicit) && Metadata.PrincipalEntityType.IsInDefinitionPath(Metadata.DeclaringEntityType.ClrType))
				{
					return null;
				}
				internalRelationshipBuilder = (Metadata.SetIsOwnership(true, configurationSource)?.Builder)?.OnDelete(DeleteBehavior.Cascade, ConfigurationSource.Convention);
				if (internalRelationshipBuilder == null)
				{
					return null;
				}
				using IConventionBatch batch = ModelBuilder.Metadata.ConventionDispatcher.DelayConventions();
				foreach (ForeignKey item2 in list)
				{
					item2.DeclaringEntityType.Builder.HasNoRelationship(item2, configurationSource);
				}
				ForeignKey metadata = internalRelationshipBuilder.Metadata;
				metadata.DeclaringEntityType.Builder.HasNoRelationship(metadata, metadata.GetConfigurationSource());
				if (foreignKey.Builder.IsWeakTypeDefinition(configurationSource) == null)
				{
					return null;
				}
				EntityType entityType = ((declaringEntityType.ClrType == null) ? ModelBuilder.Entity(declaringEntityType.Name, Metadata.PrincipalToDependent.Name, Metadata.PrincipalEntityType, declaringEntityType.GetConfigurationSource()).Metadata : ModelBuilder.Entity(declaringEntityType.ClrType, Metadata.PrincipalToDependent.Name, Metadata.PrincipalEntityType, declaringEntityType.GetConfigurationSource()).Metadata);
				internalRelationshipBuilder = internalRelationshipBuilder.Attach(entityType.Builder);
				ModelBuilder.Metadata.ConventionDispatcher.Tracker.Update(Metadata, internalRelationshipBuilder.Metadata);
				return batch.Run(internalRelationshipBuilder);
			}
			internalRelationshipBuilder = (Metadata.SetIsOwnership(true, configurationSource)?.Builder)?.OnDelete(DeleteBehavior.Cascade, ConfigurationSource.Convention);
			if (internalRelationshipBuilder == null && Metadata.PrincipalEntityType.Builder != null && Metadata.PrincipalToDependent != null)
			{
				internalRelationshipBuilder = Metadata.PrincipalEntityType.FindNavigation(Metadata.PrincipalToDependent.Name)?.ForeignKey.Builder;
			}
			using (IConventionBatch batch2 = ModelBuilder.Metadata.ConventionDispatcher.DelayConventions())
			{
				foreach (ForeignKey item3 in list)
				{
					item3.DeclaringEntityType.Builder.HasNoRelationship(item3, configurationSource);
				}
				if (internalRelationshipBuilder != null)
				{
					internalRelationshipBuilder.Metadata.DeclaringEntityType.Builder.HasBaseType((Type)null, configurationSource);
					if (!internalRelationshipBuilder.Metadata.DeclaringEntityType.Builder.RemoveNonOwnershipRelationships(internalRelationshipBuilder.Metadata, configurationSource))
					{
						return null;
					}
					return batch2.Run(internalRelationshipBuilder);
				}
			}
			return null;
		}
		return Metadata.SetIsOwnership(false, configurationSource)?.Builder;
	}

	public virtual bool CanSetIsOwnership(bool? ownership, ConfigurationSource? configurationSource)
	{
		if (Metadata.IsOwnership != ownership)
		{
			return configurationSource.Overrides(Metadata.GetIsOwnershipConfigurationSource());
		}
		return true;
	}

	public virtual InternalRelationshipBuilder IsWeakTypeDefinition(ConfigurationSource configurationSource)
	{
		if (Metadata.DeclaringEntityType.HasDefiningNavigation())
		{
			return this;
		}
		EntityType entityType = ((!(Metadata.DeclaringEntityType.ClrType == null)) ? ModelBuilder.Entity(Metadata.DeclaringEntityType.ClrType, Metadata.PrincipalToDependent.Name, Metadata.PrincipalEntityType, Metadata.DeclaringEntityType.GetConfigurationSource()).Metadata : ModelBuilder.Entity(Metadata.DeclaringEntityType.Name, Metadata.PrincipalToDependent.Name, Metadata.PrincipalEntityType, Metadata.DeclaringEntityType.GetConfigurationSource()).Metadata);
		ForeignKey foreignKey = entityType.GetForeignKeys().SingleOrDefault((ForeignKey fk) => fk.IsOwnership);
		if (foreignKey == null)
		{
			return Metadata.Builder;
		}
		ModelBuilder.Metadata.ConventionDispatcher.Tracker.Update(Metadata, foreignKey);
		return foreignKey.Builder;
	}

	public virtual InternalRelationshipBuilder OnDelete(DeleteBehavior? deleteBehavior, ConfigurationSource configurationSource)
	{
		if (!CanSetDeleteBehavior(deleteBehavior, configurationSource))
		{
			return null;
		}
		return Metadata.SetDeleteBehavior(deleteBehavior, configurationSource)?.Builder;
	}

	public virtual bool CanSetDeleteBehavior(DeleteBehavior? deleteBehavior, ConfigurationSource? configurationSource)
	{
		if (Metadata.DeleteBehavior != deleteBehavior)
		{
			return configurationSource.Overrides(Metadata.GetDeleteBehaviorConfigurationSource());
		}
		return true;
	}

	public virtual InternalRelationshipBuilder IsUnique(bool? unique, ConfigurationSource configurationSource)
	{
		if (Metadata.IsUnique == unique)
		{
			return Metadata.SetIsUnique(unique, configurationSource).Builder;
		}
		if (!CanSetIsUnique(unique, configurationSource, out var resetToDependent))
		{
			return null;
		}
		using IConventionBatch batch = Metadata.DeclaringEntityType.Model.ConventionDispatcher.DelayConventions();
		InternalRelationshipBuilder internalRelationshipBuilder = this;
		if (resetToDependent)
		{
			internalRelationshipBuilder = internalRelationshipBuilder.HasNavigations(null, MemberIdentity.None, configurationSource);
			if (internalRelationshipBuilder == null)
			{
				return null;
			}
		}
		internalRelationshipBuilder = internalRelationshipBuilder.Metadata.SetIsUnique(unique, configurationSource)?.Builder;
		return batch.Run(internalRelationshipBuilder);
	}

	public virtual bool CanSetIsUnique(bool? unique, ConfigurationSource? configurationSource)
	{
		bool resetToDependent;
		return CanSetIsUnique(unique, configurationSource, out resetToDependent);
	}

	private bool CanSetIsUnique(bool? unique, ConfigurationSource? configurationSource, out bool resetToDependent)
	{
		resetToDependent = false;
		if (Metadata.IsUnique == unique)
		{
			return true;
		}
		if (!configurationSource.HasValue || !configurationSource.Value.Overrides(Metadata.GetIsUniqueConfigurationSource()))
		{
			return false;
		}
		Navigation principalToDependent = Metadata.PrincipalToDependent;
		if (principalToDependent != null && !principalToDependent.IsShadowProperty() && !Navigation.IsCompatible(Metadata.PrincipalToDependent.GetIdentifyingMemberInfo(), Metadata.PrincipalEntityType.ClrType, Metadata.DeclaringEntityType.ClrType, !unique, shouldThrow: false))
		{
			if (!configurationSource.Value.Overrides(Metadata.GetPrincipalToDependentConfigurationSource()))
			{
				return false;
			}
			resetToDependent = true;
		}
		return true;
	}

	public virtual InternalRelationshipBuilder DependentEntityType([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder dependentEntityTypeBuilder, ConfigurationSource configurationSource)
	{
		return DependentEntityType(dependentEntityTypeBuilder.Metadata, configurationSource);
	}

	public virtual InternalRelationshipBuilder DependentEntityType([JetBrains.Annotations.NotNull] Type dependentType, ConfigurationSource configurationSource)
	{
		return DependentEntityType(ModelBuilder.Entity(dependentType, configurationSource, false).Metadata, configurationSource);
	}

	public virtual InternalRelationshipBuilder DependentEntityType([JetBrains.Annotations.NotNull] string dependentTypeName, ConfigurationSource configurationSource)
	{
		return DependentEntityType(ModelBuilder.Entity(dependentTypeName, configurationSource, false).Metadata, configurationSource);
	}

	public virtual InternalRelationshipBuilder DependentEntityType([JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityType, "dependentEntityType");
		InternalRelationshipBuilder internalRelationshipBuilder = this;
		if (Metadata.DeclaringEntityType.IsAssignableFrom(dependentEntityType))
		{
			ConfigurationSource? principalEndConfigurationSource = Metadata.GetPrincipalEndConfigurationSource();
			if (!principalEndConfigurationSource.HasValue || !principalEndConfigurationSource.GetValueOrDefault().Overrides(configurationSource))
			{
				Metadata.UpdatePrincipalEndConfigurationSource(configurationSource);
				internalRelationshipBuilder = (InternalRelationshipBuilder)ModelBuilder.Metadata.ConventionDispatcher.OnForeignKeyPrincipalEndChanged(internalRelationshipBuilder);
			}
			return internalRelationshipBuilder;
		}
		if (!dependentEntityType.IsAssignableFrom(Metadata.DeclaringEntityType) && configurationSource != 0)
		{
			return null;
		}
		return HasEntityTypes(Metadata.PrincipalEntityType, dependentEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder PrincipalEntityType([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder principalEntityTypeBuilder, ConfigurationSource configurationSource)
	{
		return PrincipalEntityType(principalEntityTypeBuilder.Metadata, configurationSource);
	}

	public virtual InternalRelationshipBuilder PrincipalEntityType([JetBrains.Annotations.NotNull] Type principalType, ConfigurationSource configurationSource)
	{
		return PrincipalEntityType(ModelBuilder.Entity(principalType, configurationSource, false).Metadata, configurationSource);
	}

	public virtual InternalRelationshipBuilder PrincipalEntityType([JetBrains.Annotations.NotNull] string principalTypeName, ConfigurationSource configurationSource)
	{
		return PrincipalEntityType(ModelBuilder.Entity(principalTypeName, configurationSource, false).Metadata, configurationSource);
	}

	public virtual InternalRelationshipBuilder PrincipalEntityType([JetBrains.Annotations.NotNull] EntityType principalEntityType, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		InternalRelationshipBuilder internalRelationshipBuilder = this;
		if (Metadata.PrincipalEntityType.IsAssignableFrom(principalEntityType))
		{
			ConfigurationSource? principalEndConfigurationSource = Metadata.GetPrincipalEndConfigurationSource();
			if (!principalEndConfigurationSource.HasValue || !principalEndConfigurationSource.GetValueOrDefault().Overrides(configurationSource))
			{
				Metadata.UpdatePrincipalEndConfigurationSource(configurationSource);
				internalRelationshipBuilder = (InternalRelationshipBuilder)ModelBuilder.Metadata.ConventionDispatcher.OnForeignKeyPrincipalEndChanged(internalRelationshipBuilder);
			}
			return internalRelationshipBuilder;
		}
		if (!principalEntityType.IsAssignableFrom(Metadata.PrincipalEntityType) && configurationSource != 0)
		{
			return null;
		}
		return HasEntityTypes(principalEntityType, Metadata.DeclaringEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasEntityTypes([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		return HasEntityTypes(principalEntityType, dependentEntityType, configurationSource, configurationSource);
	}

	private InternalRelationshipBuilder HasEntityTypes([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource? principalEndConfigurationSource, ConfigurationSource configurationSource)
	{
		if ((Metadata.PrincipalEntityType == principalEntityType && Metadata.DeclaringEntityType == dependentEntityType) || (Metadata.PrincipalEntityType == principalEntityType.LeastDerivedType(Metadata.PrincipalEntityType) && Metadata.DeclaringEntityType == dependentEntityType.LeastDerivedType(Metadata.DeclaringEntityType)))
		{
			if (principalEndConfigurationSource.HasValue)
			{
				ConfigurationSource? principalEndConfigurationSource2 = Metadata.GetPrincipalEndConfigurationSource();
				if (!principalEndConfigurationSource2.HasValue || !principalEndConfigurationSource2.GetValueOrDefault().Overrides(principalEndConfigurationSource))
				{
					Metadata.UpdatePrincipalEndConfigurationSource(principalEndConfigurationSource.Value);
					return (InternalRelationshipBuilder)ModelBuilder.Metadata.ConventionDispatcher.OnForeignKeyPrincipalEndChanged(this);
				}
			}
			return this;
		}
		if (!CanSetRelatedTypes(principalEntityType, dependentEntityType, strictPrincipal: true, null, null, configurationSource, shouldThrow: true, out var shouldInvert, out var shouldResetToPrincipal, out var shouldResetToDependent, out var shouldResetPrincipalProperties, out var shouldResetDependentProperties, out var shouldBeUnique) && configurationSource != 0)
		{
			return null;
		}
		IReadOnlyList<Property> readOnlyList = Array.Empty<Property>();
		IReadOnlyList<Property> readOnlyList2 = Array.Empty<Property>();
		if (shouldInvert)
		{
			principalEntityType = principalEntityType.LeastDerivedType(Metadata.DeclaringEntityType);
			dependentEntityType = dependentEntityType.LeastDerivedType(Metadata.PrincipalEntityType);
			if (Metadata.GetIsRequiredConfigurationSource() != ConfigurationSource.Explicit)
			{
				Metadata.SetIsRequiredConfigurationSource(null);
			}
		}
		else
		{
			principalEntityType = principalEntityType.LeastDerivedType(Metadata.PrincipalEntityType);
			dependentEntityType = dependentEntityType.LeastDerivedType(Metadata.DeclaringEntityType);
			object obj;
			if (!shouldResetDependentProperties)
			{
				ConfigurationSource? principalEndConfigurationSource2 = Metadata.GetPropertiesConfigurationSource();
				obj = ((principalEndConfigurationSource2.HasValue && principalEndConfigurationSource2.GetValueOrDefault().Overrides(configurationSource)) ? dependentEntityType.Builder.GetActualProperties(Metadata.Properties, configurationSource) : null);
			}
			else
			{
				obj = readOnlyList;
			}
			readOnlyList = (IReadOnlyList<Property>)obj;
			object obj2;
			if (!shouldResetPrincipalProperties)
			{
				ConfigurationSource? principalEndConfigurationSource2 = Metadata.GetPrincipalKeyConfigurationSource();
				obj2 = ((principalEndConfigurationSource2.HasValue && principalEndConfigurationSource2.GetValueOrDefault().Overrides(configurationSource)) ? principalEntityType.Builder.GetActualProperties(Metadata.PrincipalKey.Properties, configurationSource) : null);
			}
			else
			{
				obj2 = readOnlyList2;
			}
			readOnlyList2 = (IReadOnlyList<Property>)obj2;
		}
		return ReplaceForeignKey(configurationSource, principalEntityType.Builder, dependentEntityType.Builder, shouldResetToPrincipal ? new MemberIdentity?(MemberIdentity.None) : null, shouldResetToDependent ? new MemberIdentity?(MemberIdentity.None) : null, readOnlyList, null, readOnlyList2, shouldBeUnique, null, null, null, removeCurrent: true, principalEndConfigurationSource, shouldInvert);
	}

	public virtual bool CanSetRelatedTypes([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource? configurationSource)
	{
		bool shouldInvert;
		bool shouldResetToPrincipal;
		bool shouldResetToDependent;
		bool shouldResetPrincipalProperties;
		bool shouldResetDependentProperties;
		bool? shouldBeUnique;
		return CanSetRelatedTypes(principalEntityType, dependentEntityType, strictPrincipal: true, null, null, configurationSource, shouldThrow: false, out shouldInvert, out shouldResetToPrincipal, out shouldResetToDependent, out shouldResetPrincipalProperties, out shouldResetDependentProperties, out shouldBeUnique);
	}

	public virtual bool CanInvert([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> newForeignKeyProperties, ConfigurationSource? configurationSource)
	{
		if (configurationSource.Overrides(Metadata.GetPrincipalEndConfigurationSource()))
		{
			if (newForeignKeyProperties != null)
			{
				return CanSetForeignKey(newForeignKeyProperties, Metadata.PrincipalEntityType, configurationSource);
			}
			return true;
		}
		return false;
	}

	public virtual InternalRelationshipBuilder ReuniquifyTemporaryProperties(bool force)
	{
		if (!force && (Metadata.GetPropertiesConfigurationSource().HasValue || !Metadata.DeclaringEntityType.Builder.ShouldReuniquifyTemporaryProperties(Metadata)))
		{
			return Metadata.Builder;
		}
		InternalRelationshipBuilder internalRelationshipBuilder = this;
		using IConventionBatch batch = Metadata.DeclaringEntityType.Model.ConventionDispatcher.DelayConventions();
		List<Property> source = Metadata.Properties.Where((Property p) => p.IsShadowProperty() && ConfigurationSource.Convention.Overrides(p.GetConfigurationSource())).ToList();
		List<Key> list = source.SelectMany((Property p) => from k in p.GetContainingKeys()
			where ConfigurationSource.Convention.Overrides(k.GetConfigurationSource())
			select k).Distinct().ToList();
		List<RelationshipSnapshot> list2 = null;
		foreach (Key item in list)
		{
			foreach (ForeignKey item2 in item.GetReferencingForeignKeys().ToList())
			{
				if (list2 == null)
				{
					list2 = new List<RelationshipSnapshot>();
				}
				list2.Add(InternalEntityTypeBuilder.DetachRelationship(item2));
			}
		}
		List<(InternalKeyBuilder, ConfigurationSource?)> list3 = InternalEntityTypeBuilder.DetachKeys(list);
		List<InternalIndexBuilder> list4 = InternalEntityTypeBuilder.DetachIndexes(source.SelectMany((Property p) => p.GetContainingIndexes()).Distinct());
		internalRelationshipBuilder = internalRelationshipBuilder.HasForeignKey((IReadOnlyList<Property>)null, ConfigurationSource.Convention);
		if (list4 != null)
		{
			foreach (InternalIndexBuilder item3 in list4)
			{
				item3.Attach(item3.Metadata.DeclaringEntityType.Builder);
			}
		}
		if (list3 != null)
		{
			foreach (var item4 in list3)
			{
				item4.Item1.Attach(Metadata.DeclaringEntityType.RootType().Builder, item4.Item2);
			}
		}
		if (list2 != null)
		{
			foreach (RelationshipSnapshot item5 in list2)
			{
				item5.Attach();
			}
		}
		return batch.Run(internalRelationshipBuilder);
	}

	public virtual InternalRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<MemberInfo> properties, ConfigurationSource configurationSource)
	{
		return HasForeignKey(properties, Metadata.DeclaringEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, ConfigurationSource configurationSource)
	{
		return HasForeignKey(propertyNames, Metadata.DeclaringEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<MemberInfo> properties, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		return HasForeignKey(dependentEntityType.Builder.GetOrCreateProperties(properties, configurationSource), dependentEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		return HasForeignKey(dependentEntityType.Builder.GetOrCreateProperties(propertyNames, configurationSource, Metadata.PrincipalKey.Properties, Metadata.GetIsRequiredConfigurationSource().HasValue && Metadata.IsRequired, !Metadata.GetPrincipalKeyConfigurationSource().HasValue && Metadata.PrincipalEntityType.FindPrimaryKey() == null), dependentEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		return HasForeignKey(properties, Metadata.DeclaringEntityType, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, ConfigurationSource configurationSource)
	{
		if (properties == null)
		{
			if (configurationSource.Overrides(Metadata.GetPropertiesConfigurationSource()))
			{
				return ReplaceForeignKey(configurationSource, null, null, null, null, Array.Empty<Property>());
			}
			return null;
		}
		properties = dependentEntityType.Builder.GetActualProperties(properties, configurationSource);
		if (Metadata.Properties.SequenceEqual(properties))
		{
			Metadata.UpdatePropertiesConfigurationSource(configurationSource);
			InternalRelationshipBuilder internalRelationshipBuilder = this;
			if (!Metadata.IsSelfReferencing())
			{
				ConfigurationSource? principalEndConfigurationSource = Metadata.GetPrincipalEndConfigurationSource();
				if (!principalEndConfigurationSource.HasValue || !principalEndConfigurationSource.GetValueOrDefault().Overrides(configurationSource))
				{
					Metadata.UpdatePrincipalEndConfigurationSource(configurationSource);
					internalRelationshipBuilder = (InternalRelationshipBuilder)ModelBuilder.Metadata.ConventionDispatcher.OnForeignKeyPrincipalEndChanged(internalRelationshipBuilder);
				}
			}
			return internalRelationshipBuilder;
		}
		if (!CanSetForeignKey(properties, dependentEntityType, configurationSource, out var resetPrincipalKey))
		{
			return null;
		}
		return ReplaceForeignKey(configurationSource, null, dependentEntityType.Builder, null, null, properties, null, resetPrincipalKey ? Array.Empty<Property>() : null, null, null, null, null, principalEndConfigurationSource: configurationSource, removeCurrent: !Property.AreCompatible(properties, Metadata.DeclaringEntityType));
	}

	public virtual bool CanSetForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, ConfigurationSource? configurationSource)
	{
		IReadOnlyList<Property> readOnlyList = Metadata.DeclaringEntityType.FindProperties(propertyNames);
		bool resetPrincipalKey;
		if (readOnlyList != null)
		{
			return CanSetForeignKey(readOnlyList, null, configurationSource, out resetPrincipalKey);
		}
		return configurationSource.Overrides(Metadata.GetPropertiesConfigurationSource());
	}

	public virtual bool CanSetForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource? configurationSource)
	{
		bool resetPrincipalKey;
		return CanSetForeignKey(properties, null, configurationSource, out resetPrincipalKey);
	}

	private bool CanSetForeignKey(IReadOnlyList<Property> properties, EntityType dependentEntityType, ConfigurationSource? configurationSource, bool overrideSameSource = true)
	{
		bool resetPrincipalKey;
		return CanSetForeignKey(properties, dependentEntityType, configurationSource, out resetPrincipalKey, overrideSameSource);
	}

	private bool CanSetForeignKey(IReadOnlyList<Property> properties, EntityType dependentEntityType, ConfigurationSource? configurationSource, out bool resetPrincipalKey, bool overrideSameSource = true)
	{
		resetPrincipalKey = false;
		if (properties == null || !Metadata.Properties.SequenceEqual(properties))
		{
			return CanSetForeignKey(properties, dependentEntityType, Metadata.PrincipalKey.Properties, Metadata.PrincipalEntityType, configurationSource, out resetPrincipalKey, overrideSameSource);
		}
		return true;
	}

	private bool CanSetForeignKey(IReadOnlyList<Property> properties, EntityType dependentEntityType, IReadOnlyList<Property> principalKeyProperties, EntityType principalEntityType, ConfigurationSource? configurationSource, out bool resetPrincipalKey, bool overrideSameSource = true)
	{
		resetPrincipalKey = false;
		if (!configurationSource.Overrides(Metadata.GetPropertiesConfigurationSource()) || (!overrideSameSource && configurationSource == Metadata.GetPropertiesConfigurationSource()))
		{
			return false;
		}
		if (properties == null)
		{
			return true;
		}
		if (dependentEntityType == null)
		{
			dependentEntityType = Metadata.DeclaringEntityType;
		}
		if (dependentEntityType.BaseType != null && configurationSource != ConfigurationSource.Explicit && properties.Any((Property p) => p.GetContainingKeys().Any((Key k) => k.DeclaringEntityType != dependentEntityType)))
		{
			return false;
		}
		if ((dependentEntityType != Metadata.DeclaringEntityType && dependentEntityType == Metadata.PrincipalEntityType) || (properties.Count != 0 && !ForeignKey.AreCompatible(principalKeyProperties, properties, principalEntityType, dependentEntityType, shouldThrow: false)))
		{
			if (!configurationSource.HasValue || !configurationSource.Overrides(Metadata.GetPrincipalKeyConfigurationSource()))
			{
				return false;
			}
			resetPrincipalKey = true;
		}
		return true;
	}

	public virtual InternalRelationshipBuilder HasPrincipalKey([JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> properties, ConfigurationSource configurationSource)
	{
		return HasPrincipalKey(Metadata.PrincipalEntityType.Builder.GetOrCreateProperties(properties, configurationSource), configurationSource);
	}

	public virtual InternalRelationshipBuilder HasPrincipalKey([JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames, ConfigurationSource configurationSource)
	{
		return HasPrincipalKey(Metadata.PrincipalEntityType.Builder.GetOrCreateProperties(propertyNames, configurationSource), configurationSource);
	}

	public virtual InternalRelationshipBuilder HasPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		if (properties == null)
		{
			if (configurationSource.Overrides(Metadata.GetPrincipalKeyConfigurationSource()))
			{
				return ReplaceForeignKey(configurationSource, null, null, null, null, null, null, Array.Empty<Property>());
			}
			return null;
		}
		properties = Metadata.PrincipalEntityType.Builder.GetActualProperties(properties, configurationSource);
		if (Metadata.PrincipalKey.Properties.SequenceEqual(properties))
		{
			Metadata.UpdatePrincipalKeyConfigurationSource(configurationSource);
			InternalRelationshipBuilder internalRelationshipBuilder = this;
			if (!Metadata.IsSelfReferencing())
			{
				ConfigurationSource? principalEndConfigurationSource = Metadata.GetPrincipalEndConfigurationSource();
				if (!principalEndConfigurationSource.HasValue || !principalEndConfigurationSource.GetValueOrDefault().Overrides(configurationSource))
				{
					Metadata.UpdatePrincipalEndConfigurationSource(configurationSource);
					internalRelationshipBuilder = (InternalRelationshipBuilder)ModelBuilder.Metadata.ConventionDispatcher.OnForeignKeyPrincipalEndChanged(internalRelationshipBuilder);
				}
			}
			return internalRelationshipBuilder;
		}
		if (!CanSetPrincipalKey(properties, configurationSource, out var resetDependent, out var oldNameDependentProperties))
		{
			return null;
		}
		return ReplaceForeignKey(configurationSource, null, null, null, null, principalProperties: properties, dependentProperties: resetDependent ? Array.Empty<Property>() : null, principalEndConfigurationSource: configurationSource, oldNameDependentProperties: oldNameDependentProperties, isUnique: null, isRequired: null, isOwnership: null, deleteBehavior: null, removeCurrent: oldNameDependentProperties != null);
	}

	public virtual bool CanSetPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, ConfigurationSource? configurationSource)
	{
		IReadOnlyList<Property> readOnlyList = Metadata.PrincipalEntityType.FindProperties(propertyNames);
		bool resetDependent;
		IReadOnlyList<Property> oldNameDependentProperties;
		if (readOnlyList != null)
		{
			return CanSetPrincipalKey(readOnlyList, configurationSource, out resetDependent, out oldNameDependentProperties);
		}
		return configurationSource.Overrides(Metadata.GetPrincipalKeyConfigurationSource());
	}

	public virtual bool CanSetPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource? configurationSource)
	{
		bool resetDependent;
		IReadOnlyList<Property> oldNameDependentProperties;
		return CanSetPrincipalKey(properties, configurationSource, out resetDependent, out oldNameDependentProperties);
	}

	private bool CanSetPrincipalKey(IReadOnlyList<Property> properties, ConfigurationSource? configurationSource, out bool resetDependent, out IReadOnlyList<Property> oldNameDependentProperties)
	{
		resetDependent = false;
		oldNameDependentProperties = null;
		if (Metadata.PrincipalKey.Properties.SequenceEqual(properties))
		{
			return true;
		}
		if (!configurationSource.Overrides(Metadata.GetPrincipalKeyConfigurationSource()))
		{
			return false;
		}
		if (properties == null)
		{
			return true;
		}
		if (!ForeignKey.AreCompatible(properties, Metadata.Properties, Metadata.PrincipalEntityType, Metadata.DeclaringEntityType, shouldThrow: false))
		{
			if (!configurationSource.Value.Overrides(Metadata.GetPropertiesConfigurationSource()))
			{
				return false;
			}
			if (Metadata.GetPropertiesConfigurationSource().Overrides(ConfigurationSource.DataAnnotation) && Metadata.Properties.All((Property p) => ConfigurationSource.Convention.Overrides(p.GetTypeConfigurationSource()) && p.IsShadowProperty()))
			{
				oldNameDependentProperties = Metadata.Properties;
			}
			resetDependent = true;
		}
		return true;
	}

	private InternalRelationshipBuilder ReplaceForeignKey(ConfigurationSource configurationSource, InternalEntityTypeBuilder principalEntityTypeBuilder = null, InternalEntityTypeBuilder dependentEntityTypeBuilder = null, MemberIdentity? navigationToPrincipal = null, MemberIdentity? navigationToDependent = null, IReadOnlyList<Property> dependentProperties = null, IReadOnlyList<Property> oldNameDependentProperties = null, IReadOnlyList<Property> principalProperties = null, bool? isUnique = null, bool? isRequired = null, bool? isOwnership = null, DeleteBehavior? deleteBehavior = null, bool removeCurrent = false, ConfigurationSource? principalEndConfigurationSource = null, bool oldRelationshipInverted = false)
	{
		if (principalEntityTypeBuilder == null)
		{
			principalEntityTypeBuilder = (oldRelationshipInverted ? Metadata.DeclaringEntityType.Builder : Metadata.PrincipalEntityType.Builder);
		}
		if (dependentEntityTypeBuilder == null)
		{
			dependentEntityTypeBuilder = (oldRelationshipInverted ? Metadata.PrincipalEntityType.Builder : Metadata.DeclaringEntityType.Builder);
		}
		ConfigurationSource? principalToDependentConfigurationSource;
		if (!navigationToPrincipal.HasValue)
		{
			if (oldRelationshipInverted)
			{
				principalToDependentConfigurationSource = Metadata.GetPrincipalToDependentConfigurationSource();
				navigationToPrincipal = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? new MemberIdentity?(Metadata.PrincipalToDependent.CreateMemberIdentity()) : navigationToPrincipal);
			}
			else
			{
				principalToDependentConfigurationSource = Metadata.GetDependentToPrincipalConfigurationSource();
				navigationToPrincipal = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? new MemberIdentity?(Metadata.DependentToPrincipal.CreateMemberIdentity()) : navigationToPrincipal);
			}
		}
		if (!navigationToDependent.HasValue)
		{
			if (oldRelationshipInverted)
			{
				principalToDependentConfigurationSource = Metadata.GetDependentToPrincipalConfigurationSource();
				navigationToDependent = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? new MemberIdentity?(Metadata.DependentToPrincipal.CreateMemberIdentity()) : navigationToDependent);
			}
			else
			{
				principalToDependentConfigurationSource = Metadata.GetPrincipalToDependentConfigurationSource();
				navigationToDependent = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? new MemberIdentity?(Metadata.PrincipalToDependent.CreateMemberIdentity()) : navigationToDependent);
			}
		}
		if (dependentProperties == null)
		{
			principalToDependentConfigurationSource = Metadata.GetPropertiesConfigurationSource();
			dependentProperties = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource) && !oldRelationshipInverted) ? Metadata.Properties : null);
		}
		if (principalProperties == null)
		{
			principalToDependentConfigurationSource = Metadata.GetPrincipalKeyConfigurationSource();
			principalProperties = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource) && !oldRelationshipInverted) ? Metadata.PrincipalKey.Properties : null);
		}
		bool? flag = isUnique;
		if (!flag.HasValue)
		{
			principalToDependentConfigurationSource = Metadata.GetIsUniqueConfigurationSource();
			isUnique = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? new bool?(Metadata.IsUnique) : null);
		}
		flag = isRequired;
		if (!flag.HasValue)
		{
			principalToDependentConfigurationSource = Metadata.GetIsRequiredConfigurationSource();
			isRequired = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? new bool?(Metadata.IsRequired) : null);
		}
		flag = isOwnership;
		if (!flag.HasValue)
		{
			principalToDependentConfigurationSource = Metadata.GetIsOwnershipConfigurationSource();
			isOwnership = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource) && !oldRelationshipInverted) ? new bool?(Metadata.IsOwnership) : null);
		}
		DeleteBehavior? deleteBehavior2 = deleteBehavior;
		if (!deleteBehavior2.HasValue)
		{
			principalToDependentConfigurationSource = Metadata.GetDeleteBehaviorConfigurationSource();
			deleteBehavior = ((principalToDependentConfigurationSource.HasValue && principalToDependentConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? new DeleteBehavior?(Metadata.DeleteBehavior) : null);
		}
		principalToDependentConfigurationSource = principalEndConfigurationSource;
		if (!principalToDependentConfigurationSource.HasValue)
		{
			principalEndConfigurationSource = ((principalEntityTypeBuilder.Metadata != dependentEntityTypeBuilder.Metadata && ((principalProperties != null && principalProperties.Count > 0) || (dependentProperties != null && dependentProperties.Count > 0) || (navigationToDependent.HasValue && isUnique == false) || isOwnership == true)) ? new ConfigurationSource?(configurationSource) : null);
		}
		principalEndConfigurationSource = principalEndConfigurationSource.Max(Metadata.GetPrincipalEndConfigurationSource());
		return ReplaceForeignKey(principalEntityTypeBuilder, dependentEntityTypeBuilder, navigationToPrincipal, navigationToDependent, dependentProperties, oldNameDependentProperties, principalProperties, isUnique, isRequired, isOwnership, deleteBehavior, removeCurrent, oldRelationshipInverted, principalEndConfigurationSource, configurationSource);
	}

	private InternalRelationshipBuilder ReplaceForeignKey([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder principalEntityTypeBuilder, [JetBrains.Annotations.NotNull] InternalEntityTypeBuilder dependentEntityTypeBuilder, MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> oldNameDependentProperties, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> principalProperties, bool? isUnique, bool? isRequired, bool? isOwnership, DeleteBehavior? deleteBehavior, bool removeCurrent, bool oldRelationshipInverted, ConfigurationSource? principalEndConfigurationSource, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityTypeBuilder, "principalEntityTypeBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityTypeBuilder, "dependentEntityTypeBuilder");
		using IConventionBatch batch = Metadata.DeclaringEntityType.Model.ConventionDispatcher.DelayConventions();
		InternalRelationshipBuilder internalRelationshipBuilder = GetOrCreateRelationshipBuilder(principalEntityTypeBuilder.Metadata, dependentEntityTypeBuilder.Metadata, navigationToPrincipal ?? ((Metadata.DependentToPrincipal == null) ? null : new MemberIdentity?(Metadata.DependentToPrincipal.CreateMemberIdentity())), navigationToDependent ?? ((Metadata.PrincipalToDependent == null) ? null : new MemberIdentity?(Metadata.PrincipalToDependent.CreateMemberIdentity())), (dependentProperties != null && dependentProperties.Count > 0) ? dependentProperties : null, oldNameDependentProperties, (principalProperties != null && principalProperties.Count > 0) ? principalProperties : null, isRequired, removeCurrent, principalEndConfigurationSource, configurationSource, out var existingRelationshipInverted);
		if (internalRelationshipBuilder == null)
		{
			return null;
		}
		ConfigurationSource? principalEndConfigurationSource2 = internalRelationshipBuilder.Metadata.GetPrincipalEndConfigurationSource();
		bool flag = principalEndConfigurationSource.HasValue && principalEndConfigurationSource.Value.Overrides(principalEndConfigurationSource2);
		if (existingRelationshipInverted == true && !flag)
		{
			oldRelationshipInverted = !oldRelationshipInverted;
			existingRelationshipInverted = false;
			InternalEntityTypeBuilder internalEntityTypeBuilder = principalEntityTypeBuilder;
			principalEntityTypeBuilder = dependentEntityTypeBuilder;
			dependentEntityTypeBuilder = internalEntityTypeBuilder;
			MemberIdentity? memberIdentity = navigationToPrincipal;
			navigationToPrincipal = navigationToDependent;
			navigationToDependent = memberIdentity;
			dependentProperties = null;
			principalProperties = null;
		}
		string text = ((!oldRelationshipInverted) ? Metadata.DependentToPrincipal?.Name : Metadata.PrincipalToDependent?.Name);
		string text2 = ((!oldRelationshipInverted) ? Metadata.PrincipalToDependent?.Name : Metadata.DependentToPrincipal?.Name);
		ConfigurationSource configurationSource2 = Metadata.GetConfigurationSource();
		if ((dependentProperties != null && dependentProperties.Count > 0) || navigationToPrincipal?.Name != null || navigationToDependent?.Name != null)
		{
			configurationSource2 = configurationSource2.Max(configurationSource);
		}
		internalRelationshipBuilder.Metadata.UpdateConfigurationSource(configurationSource2);
		bool flag2 = internalRelationshipBuilder.Metadata.DependentToPrincipal != null && ((existingRelationshipInverted == false && navigationToPrincipal.HasValue && navigationToPrincipal.Value.Name != internalRelationshipBuilder.Metadata.DependentToPrincipal.Name) || (existingRelationshipInverted == true && navigationToDependent.HasValue && navigationToDependent.Value.Name != internalRelationshipBuilder.Metadata.DependentToPrincipal.Name));
		bool flag3 = internalRelationshipBuilder.Metadata.PrincipalToDependent != null && ((existingRelationshipInverted == false && navigationToDependent.HasValue && navigationToDependent.Value.Name != internalRelationshipBuilder.Metadata.PrincipalToDependent.Name) || (existingRelationshipInverted == true && navigationToPrincipal.HasValue && navigationToPrincipal.Value.Name != internalRelationshipBuilder.Metadata.PrincipalToDependent.Name));
		if (flag2 || flag3)
		{
			internalRelationshipBuilder = internalRelationshipBuilder.HasNavigations(flag2 ? new MemberIdentity?(MemberIdentity.None) : null, flag3 ? new MemberIdentity?(MemberIdentity.None) : null, configurationSource) ?? internalRelationshipBuilder;
		}
		internalRelationshipBuilder = internalRelationshipBuilder.HasEntityTypes(principalEntityTypeBuilder.Metadata, dependentEntityTypeBuilder.Metadata, principalEndConfigurationSource, configurationSource) ?? internalRelationshipBuilder;
		dependentProperties = oldNameDependentProperties ?? dependentProperties;
		if (dependentProperties != null || principalProperties != null)
		{
			bool flag4 = false;
			ConfigurationSource? left = null;
			if (dependentProperties != null)
			{
				dependentProperties = dependentEntityTypeBuilder.GetActualProperties(dependentProperties, configurationSource);
				left = configurationSource;
				if (PropertyListComparer.Instance.Equals(Metadata.Properties, dependentProperties) && !oldRelationshipInverted)
				{
					left = left.Max(Metadata.GetPropertiesConfigurationSource());
				}
				if (left.HasValue)
				{
					if (internalRelationshipBuilder.Metadata.Properties.SequenceEqual(dependentProperties))
					{
						internalRelationshipBuilder.HasForeignKey(dependentProperties, left.Value);
					}
					else if (dependentProperties.Count > 0 || (!removeCurrent && Metadata == internalRelationshipBuilder.Metadata))
					{
						flag4 = true;
					}
				}
			}
			ConfigurationSource? left2 = null;
			if (principalProperties != null)
			{
				principalProperties = principalEntityTypeBuilder.GetActualProperties(principalProperties, configurationSource);
				left2 = configurationSource;
				if (PropertyListComparer.Instance.Equals(principalProperties, internalRelationshipBuilder.Metadata.PrincipalKey.Properties) && !oldRelationshipInverted)
				{
					left2 = left2.Max(Metadata.GetPrincipalKeyConfigurationSource());
				}
				if (left2.HasValue)
				{
					if (internalRelationshipBuilder.Metadata.PrincipalKey.Properties.SequenceEqual(principalProperties))
					{
						internalRelationshipBuilder.HasPrincipalKey(principalProperties, left2.Value);
					}
					else if (principalProperties.Count > 0 || (!removeCurrent && Metadata == internalRelationshipBuilder.Metadata))
					{
						flag4 = true;
					}
				}
			}
			if (flag4)
			{
				Key principalKey = null;
				if (principalProperties != null && principalProperties.Count != 0)
				{
					principalKey = principalEntityTypeBuilder.Metadata.RootType().Builder.HasKey(principalProperties, configurationSource).Metadata;
				}
				ForeignKey metadata = internalRelationshipBuilder.Metadata;
				internalRelationshipBuilder = metadata.DeclaringEntityType.Builder.UpdateForeignKey(metadata, (dependentProperties != null && dependentProperties.Count == 0) ? null : dependentProperties, principalKey, navigationToPrincipal?.Name, isRequired, null);
				if (left.HasValue && (dependentProperties == null || dependentProperties.Count != 0))
				{
					internalRelationshipBuilder.Metadata.UpdatePropertiesConfigurationSource(left.Value);
				}
				if (left2.HasValue && principalProperties.Count != 0)
				{
					internalRelationshipBuilder.Metadata.UpdatePrincipalKeyConfigurationSource(left2.Value);
				}
			}
		}
		if (isUnique.HasValue)
		{
			ConfigurationSource configurationSource3 = configurationSource;
			if (isUnique.Value == Metadata.IsUnique)
			{
				configurationSource3 = configurationSource3.Max(Metadata.GetIsUniqueConfigurationSource());
			}
			internalRelationshipBuilder = internalRelationshipBuilder.IsUnique(isUnique.Value, configurationSource3) ?? internalRelationshipBuilder;
		}
		if (isRequired.HasValue)
		{
			ConfigurationSource configurationSource4 = configurationSource;
			if (isRequired.Value == Metadata.IsRequired)
			{
				configurationSource4 = configurationSource4.Max(Metadata.GetIsRequiredConfigurationSource());
			}
			internalRelationshipBuilder = internalRelationshipBuilder.IsRequired(isRequired.Value, configurationSource4) ?? internalRelationshipBuilder;
		}
		if (deleteBehavior.HasValue)
		{
			ConfigurationSource configurationSource5 = configurationSource;
			if (deleteBehavior.Value == Metadata.DeleteBehavior)
			{
				configurationSource5 = configurationSource5.Max(Metadata.GetDeleteBehaviorConfigurationSource());
			}
			internalRelationshipBuilder = internalRelationshipBuilder.OnDelete(deleteBehavior.Value, configurationSource5) ?? internalRelationshipBuilder;
		}
		if (navigationToPrincipal.HasValue)
		{
			ConfigurationSource configurationSource6 = configurationSource;
			if (navigationToPrincipal.Value.Name == text)
			{
				ConfigurationSource? right = (oldRelationshipInverted ? Metadata.GetPrincipalToDependentConfigurationSource() : Metadata.GetDependentToPrincipalConfigurationSource());
				configurationSource6 = configurationSource6.Max(right);
			}
			internalRelationshipBuilder = internalRelationshipBuilder.HasNavigations(navigationToPrincipal, null, configurationSource6) ?? internalRelationshipBuilder;
			Navigation navigation = (oldRelationshipInverted ? Metadata.PrincipalToDependent : Metadata.DependentToPrincipal);
			if (navigation != null && navigation != internalRelationshipBuilder.Metadata.DependentToPrincipal)
			{
				internalRelationshipBuilder.Metadata.DependentToPrincipal?.Builder.MergeAnnotationsFrom(navigation);
			}
		}
		if (navigationToDependent.HasValue)
		{
			ConfigurationSource configurationSource7 = configurationSource;
			if (navigationToDependent.Value.Name == text2)
			{
				ConfigurationSource? right2 = (oldRelationshipInverted ? Metadata.GetDependentToPrincipalConfigurationSource() : Metadata.GetPrincipalToDependentConfigurationSource());
				configurationSource7 = configurationSource7.Max(right2);
			}
			internalRelationshipBuilder = internalRelationshipBuilder.HasNavigations(null, navigationToDependent, configurationSource7) ?? internalRelationshipBuilder;
			Navigation navigation2 = (oldRelationshipInverted ? Metadata.DependentToPrincipal : Metadata.PrincipalToDependent);
			if (navigation2 != null && navigation2 != internalRelationshipBuilder.Metadata.PrincipalToDependent)
			{
				internalRelationshipBuilder.Metadata.PrincipalToDependent?.Builder.MergeAnnotationsFrom(navigation2);
			}
		}
		if (isOwnership.HasValue)
		{
			ConfigurationSource configurationSource8 = configurationSource;
			if (isOwnership.Value == Metadata.IsOwnership)
			{
				configurationSource8 = configurationSource8.Max(Metadata.GetIsOwnershipConfigurationSource());
			}
			internalRelationshipBuilder = internalRelationshipBuilder.IsOwnership(isOwnership.Value, configurationSource8) ?? internalRelationshipBuilder;
		}
		else if (!oldRelationshipInverted && Metadata.GetIsOwnershipConfigurationSource().HasValue)
		{
			internalRelationshipBuilder = internalRelationshipBuilder.IsOwnership(Metadata.IsOwnership, Metadata.GetIsOwnershipConfigurationSource().Value) ?? internalRelationshipBuilder;
		}
		if (Metadata != internalRelationshipBuilder.Metadata)
		{
			internalRelationshipBuilder.MergeAnnotationsFrom(Metadata);
		}
		return batch.Run(internalRelationshipBuilder);
	}

	private InternalRelationshipBuilder GetOrCreateRelationshipBuilder(EntityType principalEntityType, EntityType dependentEntityType, MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, IReadOnlyList<Property> dependentProperties, IReadOnlyList<Property> oldNameDependentProperties, IReadOnlyList<Property> principalProperties, bool? isRequired, bool removeCurrent, ConfigurationSource? principalEndConfigurationSource, ConfigurationSource? configurationSource, out bool? existingRelationshipInverted)
	{
		existingRelationshipInverted = null;
		List<InternalRelationshipBuilder> list = (from r in FindRelationships(principalEntityType, dependentEntityType, navigationToPrincipal, navigationToDependent, dependentProperties, principalProperties ?? principalEntityType.FindPrimaryKey()?.Properties)
			where r.Metadata != Metadata
			select r).Distinct().ToList();
		List<InternalRelationshipBuilder> list2 = new List<InternalRelationshipBuilder>();
		List<Tuple<InternalRelationshipBuilder, bool, Resolution, bool>> list3 = new List<Tuple<InternalRelationshipBuilder, bool, Resolution, bool>>();
		bool shouldResetPrincipalProperties;
		bool? shouldBeUnique;
		foreach (InternalRelationshipBuilder item5 in list)
		{
			bool flag = true;
			bool? flag2 = null;
			bool item = false;
			Resolution resolution = Resolution.None;
			string text = navigationToPrincipal?.Name;
			string text2 = navigationToDependent?.Name;
			if (text != null)
			{
				if (text == item5.Metadata.DependentToPrincipal?.Name && dependentEntityType.IsSameHierarchy(item5.Metadata.DeclaringEntityType))
				{
					if (item5.CanSetNavigation(null, pointsToPrincipal: true, configurationSource, overrideSameSource: false))
					{
						resolution |= Resolution.ResetToPrincipal;
						shouldResetPrincipalProperties = flag2.GetValueOrDefault();
						if (!flag2.HasValue)
						{
							shouldResetPrincipalProperties = false;
							flag2 = shouldResetPrincipalProperties;
						}
					}
					else if (item5.CanSetNavigation((string)null, pointsToPrincipal: true, configurationSource) && (item5.Metadata.GetConfigurationSource() == ConfigurationSource.Explicit || text2 != null || item5.Metadata.PrincipalToDependent == null || !item5.Metadata.DeclaringEntityType.IsStrictlyDerivedFrom(dependentEntityType)))
					{
						if (text2 != null && item5.Metadata.PrincipalToDependent != null && text2 != item5.Metadata.PrincipalToDependent.Name)
						{
							item = true;
						}
						resolution |= Resolution.ResetToPrincipal;
						flag2 = true;
					}
					else
					{
						flag = false;
					}
				}
				else if (text == item5.Metadata.PrincipalToDependent?.Name && dependentEntityType.IsSameHierarchy(item5.Metadata.PrincipalEntityType))
				{
					if (item5.CanSetNavigation(null, pointsToPrincipal: false, configurationSource, overrideSameSource: false))
					{
						resolution |= Resolution.ResetToDependent;
						shouldResetPrincipalProperties = flag2.GetValueOrDefault();
						if (!flag2.HasValue)
						{
							shouldResetPrincipalProperties = false;
							flag2 = shouldResetPrincipalProperties;
						}
					}
					else if (item5.CanSetNavigation((string)null, pointsToPrincipal: false, configurationSource) && (item5.Metadata.GetConfigurationSource() == ConfigurationSource.Explicit || text2 != null || item5.Metadata.DependentToPrincipal == null || !item5.Metadata.PrincipalEntityType.IsStrictlyDerivedFrom(dependentEntityType)))
					{
						if (text2 != null && item5.Metadata.DependentToPrincipal != null && text2 != item5.Metadata.DependentToPrincipal.Name)
						{
							item = true;
						}
						resolution |= Resolution.ResetToDependent;
						flag2 = true;
					}
					else
					{
						flag = false;
					}
				}
			}
			if (text2 != null)
			{
				if (text2 == item5.Metadata.PrincipalToDependent?.Name && principalEntityType.IsSameHierarchy(item5.Metadata.PrincipalEntityType))
				{
					if (item5.CanSetNavigation(null, pointsToPrincipal: false, configurationSource, overrideSameSource: false))
					{
						resolution |= Resolution.ResetToDependent;
						shouldResetPrincipalProperties = flag2.GetValueOrDefault();
						if (!flag2.HasValue)
						{
							shouldResetPrincipalProperties = false;
							flag2 = shouldResetPrincipalProperties;
						}
					}
					else if (item5.CanSetNavigation((string)null, pointsToPrincipal: false, configurationSource) && (item5.Metadata.GetConfigurationSource() == ConfigurationSource.Explicit || text != null || item5.Metadata.DependentToPrincipal == null || !item5.Metadata.PrincipalEntityType.IsStrictlyDerivedFrom(principalEntityType)))
					{
						if (text != null && item5.Metadata.DependentToPrincipal != null && text != item5.Metadata.DependentToPrincipal.Name)
						{
							item = true;
						}
						resolution |= Resolution.ResetToDependent;
						flag2 = true;
					}
					else
					{
						flag = false;
					}
				}
				else if (text2 == item5.Metadata.DependentToPrincipal?.Name && principalEntityType.IsSameHierarchy(item5.Metadata.DeclaringEntityType))
				{
					if (item5.CanSetNavigation(null, pointsToPrincipal: true, configurationSource, overrideSameSource: false))
					{
						resolution |= Resolution.ResetToPrincipal;
						shouldResetPrincipalProperties = flag2.GetValueOrDefault();
						if (!flag2.HasValue)
						{
							shouldResetPrincipalProperties = false;
							flag2 = shouldResetPrincipalProperties;
						}
					}
					else if (item5.CanSetNavigation((string)null, pointsToPrincipal: true, configurationSource) && (item5.Metadata.GetConfigurationSource() == ConfigurationSource.Explicit || text != null || item5.Metadata.PrincipalToDependent == null || !item5.Metadata.DeclaringEntityType.IsStrictlyDerivedFrom(principalEntityType)))
					{
						if (text != null && item5.Metadata.PrincipalToDependent != null && text != item5.Metadata.PrincipalToDependent.Name)
						{
							item = true;
						}
						resolution |= Resolution.ResetToPrincipal;
						flag2 = true;
					}
					else
					{
						flag = false;
					}
				}
			}
			if (dependentProperties != null && item5.Metadata.Properties.SequenceEqual(dependentProperties))
			{
				if (item5.CanSetForeignKey(null, null, configurationSource, overrideSameSource: false))
				{
					resolution |= Resolution.ResetDependentProperties;
					shouldResetPrincipalProperties = flag2.GetValueOrDefault();
					if (!flag2.HasValue)
					{
						shouldResetPrincipalProperties = false;
						flag2 = shouldResetPrincipalProperties;
					}
				}
				else if (item5.CanSetForeignKey((IReadOnlyList<Property>)null, configurationSource))
				{
					resolution |= Resolution.ResetDependentProperties;
					flag2 = true;
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				shouldBeUnique = flag2;
				if ((shouldBeUnique ?? true) && configurationSource.HasValue && item5.Metadata.DeclaringEntityType.Builder.CanRemoveForeignKey(item5.Metadata, configurationSource.Value))
				{
					resolution |= Resolution.Remove;
				}
				shouldBeUnique = flag2;
				list3.Add(Tuple.Create(item5, shouldBeUnique ?? true, resolution, item));
			}
			else
			{
				list2.Add(item5);
			}
		}
		InternalRelationshipBuilder newRelationshipBuilder = null;
		foreach (InternalRelationshipBuilder item6 in list2.Concat(from r in list3.Where((Tuple<InternalRelationshipBuilder, bool, Resolution, bool> r) => r.Item2).Concat(list3.Where((Tuple<InternalRelationshipBuilder, bool, Resolution, bool> r) => !r.Item2))
			select r.Item1))
		{
			if (item6.CanSetRelatedTypes(principalEntityType, dependentEntityType, principalEndConfigurationSource.HasValue && principalEndConfigurationSource.Overrides(Metadata.GetPrincipalEndConfigurationSource()), navigationToPrincipal, navigationToDependent, configurationSource, shouldThrow: false, out var shouldInvert, out var shouldResetToPrincipal, out var shouldResetToDependent, out shouldResetPrincipalProperties, out var _, out shouldBeUnique) && (configurationSource == ConfigurationSource.Explicit || !(shouldResetToPrincipal || shouldResetToDependent) || (navigationToPrincipal.HasValue && !navigationToPrincipal.Value.IsNone() && navigationToDependent.HasValue && !navigationToDependent.GetValueOrDefault().IsNone()) || item6.Metadata.DependentToPrincipal == null || item6.Metadata.PrincipalToDependent == null || ((shouldInvert || !principalEntityType.IsAssignableFrom(item6.Metadata.PrincipalEntityType) || !dependentEntityType.IsAssignableFrom(item6.Metadata.DeclaringEntityType)) && (!shouldInvert || !principalEntityType.IsAssignableFrom(item6.Metadata.DeclaringEntityType) || !dependentEntityType.IsAssignableFrom(item6.Metadata.PrincipalEntityType)))) && (dependentProperties == null || Property.AreCompatible(dependentProperties, item6.Metadata.DeclaringEntityType)) && (principalProperties == null || Property.AreCompatible(principalProperties, item6.Metadata.PrincipalEntityType)))
			{
				existingRelationshipInverted = shouldInvert;
				newRelationshipBuilder = item6;
				break;
			}
		}
		if (list2.Any((InternalRelationshipBuilder r) => r != newRelationshipBuilder))
		{
			return null;
		}
		List<Property> list4 = dependentProperties?.Where((Property p) => p.GetConfigurationSource() == ConfigurationSource.Convention && p.IsShadowProperty()).ToList();
		Index index = ((list4 != null && list4.Count > 0 && dependentEntityType.FindIndex(list4) == null) ? dependentEntityType.Builder.HasIndex(list4, ConfigurationSource.Convention).Metadata : null);
		List<Property> list5 = principalProperties?.Where((Property p) => p.GetConfigurationSource() == ConfigurationSource.Convention && p.IsShadowProperty()).ToList();
		Index index2 = ((list5 != null && list5.Count > 0 && principalEntityType.FindIndex(list5) == null) ? principalEntityType.Builder.HasIndex(list5, ConfigurationSource.Convention).Metadata : null);
		List<ForeignKey> list6 = new List<ForeignKey>();
		if (Metadata.Builder == null)
		{
			list6.Add(Metadata);
		}
		else if (removeCurrent || newRelationshipBuilder != null)
		{
			list6.Add(Metadata);
			Metadata.DeclaringEntityType.Builder.HasNoRelationship(Metadata, ConfigurationSource.Explicit);
		}
		else
		{
			existingRelationshipInverted = false;
			newRelationshipBuilder = Metadata.Builder;
		}
		foreach (Tuple<InternalRelationshipBuilder, bool, Resolution, bool> item7 in list3)
		{
			InternalRelationshipBuilder internalRelationshipBuilder = item7.Item1;
			bool item2 = item7.Item2;
			Resolution item3 = item7.Item3;
			bool item4 = item7.Item4;
			if (item2 && configurationSource == ConfigurationSource.Explicit && item4)
			{
				ForeignKey metadata = internalRelationshipBuilder.Metadata;
				throw new InvalidOperationException(CoreStrings.ConflictingRelationshipNavigation(principalEntityType.DisplayName(), navigationToDependent?.Name, dependentEntityType.DisplayName(), navigationToPrincipal?.Name, metadata.PrincipalEntityType.DisplayName(), metadata.PrincipalToDependent.Name, metadata.DeclaringEntityType.DisplayName(), metadata.DependentToPrincipal.Name));
			}
			if (internalRelationshipBuilder == newRelationshipBuilder)
			{
				continue;
			}
			if (item3.HasFlag(Resolution.Remove))
			{
				list6.Add(internalRelationshipBuilder.Metadata);
				internalRelationshipBuilder.Metadata.DeclaringEntityType.Builder.HasNoRelationship(internalRelationshipBuilder.Metadata, ConfigurationSource.Explicit);
				continue;
			}
			if (item3.HasFlag(Resolution.ResetToPrincipal))
			{
				internalRelationshipBuilder = internalRelationshipBuilder.HasNavigations(MemberIdentity.None, null, internalRelationshipBuilder.Metadata.GetConfigurationSource());
			}
			if (item3.HasFlag(Resolution.ResetToDependent))
			{
				internalRelationshipBuilder = internalRelationshipBuilder.HasNavigations(null, MemberIdentity.None, internalRelationshipBuilder.Metadata.GetConfigurationSource());
			}
			if (internalRelationshipBuilder.Metadata.Builder != null)
			{
				ForeignKey metadata2 = internalRelationshipBuilder.Metadata;
				if (metadata2.DependentToPrincipal == null && metadata2.PrincipalToDependent == null && metadata2.DeclaringEntityType.Builder.HasNoRelationship(metadata2, ConfigurationSource.Convention) != null)
				{
					list6.Add(metadata2);
				}
				if (item3.HasFlag(Resolution.ResetDependentProperties))
				{
					ForeignKey metadata3 = internalRelationshipBuilder.Metadata;
					internalRelationshipBuilder.HasForeignKey((IReadOnlyList<Property>)null, metadata3.GetConfigurationSource());
				}
			}
		}
		if (newRelationshipBuilder == null)
		{
			Key key = ((principalProperties != null) ? principalEntityType.RootType().Builder.HasKey(principalProperties, configurationSource).Metadata : principalEntityType.FindPrimaryKey());
			if (key != null)
			{
				if (oldNameDependentProperties != null || (dependentProperties != null && !ForeignKey.AreCompatible(key.Properties, dependentProperties, principalEntityType, dependentEntityType, shouldThrow: false) && dependentProperties.All((Property p) => ConfigurationSource.Convention.Overrides(p.GetTypeConfigurationSource()) && p.IsShadowProperty())))
				{
					dependentProperties = oldNameDependentProperties ?? dependentProperties;
					if (key.Properties.Count == dependentProperties.Count)
					{
						PropertiesSnapshot propertiesSnapshot = InternalEntityTypeBuilder.DetachProperties(dependentProperties);
						dependentProperties = dependentEntityType.Builder.GetOrCreateProperties(dependentProperties.Select((Property p) => p.Name).ToList(), ConfigurationSource.Convention, key.Properties, isRequired.GetValueOrDefault());
						propertiesSnapshot.Attach(dependentEntityType.Builder);
					}
				}
				if (dependentProperties != null && !ForeignKey.AreCompatible(key.Properties, dependentProperties, principalEntityType, dependentEntityType, shouldThrow: false))
				{
					if (principalProperties == null)
					{
						key = null;
					}
					else
					{
						dependentProperties = null;
					}
				}
			}
			newRelationshipBuilder = dependentEntityType.Builder.CreateForeignKey(principalEntityType.Builder, dependentProperties, key, navigationToPrincipal?.Name, isRequired, ConfigurationSource.Convention);
		}
		foreach (ForeignKey item8 in list6)
		{
			Metadata.DeclaringEntityType.Model.ConventionDispatcher.Tracker.Update(item8, newRelationshipBuilder.Metadata);
		}
		if (index?.Builder != null)
		{
			dependentEntityType.RemoveIndex(index.Properties);
		}
		if (index2?.Builder != null)
		{
			index2.DeclaringEntityType.RemoveIndex(index2.Properties);
		}
		return newRelationshipBuilder;
	}

	private static IReadOnlyList<InternalRelationshipBuilder> FindRelationships([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, IReadOnlyList<Property> dependentProperties, IReadOnlyList<Property> principalProperties)
	{
		List<InternalRelationshipBuilder> list = new List<InternalRelationshipBuilder>();
		if (navigationToPrincipal?.Name != null)
		{
			list.AddRange(from n in dependentEntityType.FindNavigationsInHierarchy(navigationToPrincipal.Value.Name)
				select n.ForeignKey.Builder);
		}
		if (navigationToDependent?.Name != null)
		{
			list.AddRange(from n in principalEntityType.FindNavigationsInHierarchy(navigationToDependent.Value.Name)
				select n.ForeignKey.Builder);
		}
		if (dependentProperties != null)
		{
			if (principalProperties != null)
			{
				Key key = principalEntityType.FindKey(principalProperties);
				if (key != null)
				{
					list.AddRange(from fk in dependentEntityType.FindForeignKeysInHierarchy(dependentProperties, key, principalEntityType)
						select fk.Builder);
				}
			}
			else
			{
				list.AddRange(from fk in dependentEntityType.FindForeignKeysInHierarchy(dependentProperties)
					where fk.PrincipalEntityType == principalEntityType
					select fk.Builder);
			}
		}
		return list;
	}

	public static InternalRelationshipBuilder FindCurrentRelationshipBuilder([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> principalProperties)
	{
		InternalRelationshipBuilder internalRelationshipBuilder = null;
		foreach (InternalRelationshipBuilder item in FindRelationships(principalEntityType, dependentEntityType, navigationToPrincipal, navigationToDependent, dependentProperties, principalProperties).Distinct())
		{
			if (!item.Metadata.DeclaringEntityType.IsAssignableFrom(dependentEntityType) || !item.Metadata.PrincipalEntityType.IsAssignableFrom(principalEntityType))
			{
				continue;
			}
			ForeignKey metadata = item.Metadata;
			if (((principalEntityType.IsSameHierarchy(dependentEntityType) && (!navigationToPrincipal.HasValue || navigationToPrincipal.Value.Name == metadata.PrincipalToDependent?.Name) && (!navigationToDependent.HasValue || navigationToDependent.Value.Name == metadata.DependentToPrincipal?.Name)) || ((!navigationToPrincipal.HasValue || !(metadata.DependentToPrincipal?.Name != navigationToPrincipal.Value.Name)) && (!navigationToDependent.HasValue || !(metadata.PrincipalToDependent?.Name != navigationToDependent.Value.Name)))) && (dependentProperties == null || metadata.Properties.SequenceEqual(dependentProperties)) && (principalProperties == null || metadata.PrincipalKey.Properties.SequenceEqual(principalProperties)))
			{
				if (internalRelationshipBuilder != null)
				{
					return null;
				}
				internalRelationshipBuilder = item;
			}
		}
		return internalRelationshipBuilder;
	}

	public virtual InternalRelationshipBuilder Attach([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder entityTypeBuilder)
	{
		ConfigurationSource configurationSource = Metadata.GetConfigurationSource();
		Model model = Metadata.DeclaringEntityType.Model;
		InternalEntityTypeBuilder internalEntityTypeBuilder;
		EntityType entityType;
		if (Metadata.PrincipalEntityType.Builder != null)
		{
			internalEntityTypeBuilder = Metadata.PrincipalEntityType.Builder;
			entityType = Metadata.PrincipalEntityType;
		}
		else if (Metadata.PrincipalEntityType.Name == entityTypeBuilder.Metadata.Name)
		{
			internalEntityTypeBuilder = entityTypeBuilder;
			entityType = entityTypeBuilder.Metadata;
		}
		else
		{
			entityType = model.FindEntityType(Metadata.PrincipalEntityType.Name);
			if (entityType == null)
			{
				if (!model.EntityTypeShouldHaveDefiningNavigation(Metadata.PrincipalEntityType.Name) || !Metadata.PrincipalEntityType.HasDefiningNavigation())
				{
					return null;
				}
				entityType = model.FindEntityType(Metadata.PrincipalEntityType.Name, Metadata.PrincipalEntityType.DefiningNavigationName, Metadata.PrincipalEntityType.DefiningEntityType);
				if (entityType == null)
				{
					return null;
				}
			}
			internalEntityTypeBuilder = entityType.Builder;
		}
		if (!Metadata.GetConfigurationSource().Overrides(ConfigurationSource.Explicit) && entityType.FindOwnership() != null && Metadata.DependentToPrincipal != null && !Metadata.IsOwnership)
		{
			return null;
		}
		InternalEntityTypeBuilder internalEntityTypeBuilder2;
		EntityType entityType2;
		if (Metadata.DeclaringEntityType.Builder != null)
		{
			internalEntityTypeBuilder2 = Metadata.DeclaringEntityType.Builder;
			entityType2 = Metadata.DeclaringEntityType;
		}
		else if (Metadata.DeclaringEntityType.Name == entityTypeBuilder.Metadata.Name && (entityType != entityTypeBuilder.Metadata || !entityType.HasDefiningNavigation()))
		{
			internalEntityTypeBuilder2 = entityTypeBuilder;
			entityType2 = entityTypeBuilder.Metadata;
		}
		else
		{
			entityType2 = model.FindEntityType(Metadata.DeclaringEntityType.Name);
			if (entityType2 == null)
			{
				using (ModelBuilder.Metadata.ConventionDispatcher.DelayConventions())
				{
					if (model.EntityTypeShouldHaveDefiningNavigation(Metadata.DeclaringEntityType.Name))
					{
						if (Metadata.DeclaringEntityType.HasDefiningNavigation())
						{
							entityType2 = model.FindEntityType(Metadata.DeclaringEntityType.Name, Metadata.DeclaringEntityType.DefiningNavigationName, Metadata.DeclaringEntityType.DefiningEntityType);
						}
						if (entityType2 == null)
						{
							if (!Metadata.IsOwnership || Metadata.PrincipalToDependent == null)
							{
								return null;
							}
							entityType2 = ((!model.HasOtherEntityTypesWithDefiningNavigation(Metadata.DeclaringEntityType)) ? ((Metadata.DeclaringEntityType.ClrType == null) ? model.Builder.Entity(Metadata.DeclaringEntityType.Name, configurationSource, false).Metadata : model.Builder.Entity(Metadata.DeclaringEntityType.ClrType, configurationSource, false).Metadata) : ((Metadata.DeclaringEntityType.ClrType == null) ? model.AddEntityType(Metadata.DeclaringEntityType.Name, Metadata.PrincipalToDependent.Name, entityType, configurationSource) : model.AddEntityType(Metadata.DeclaringEntityType.ClrType, Metadata.PrincipalToDependent.Name, entityType, configurationSource)));
						}
					}
					else
					{
						entityType2 = ((Metadata.DeclaringEntityType.ClrType == null) ? model.AddEntityType(Metadata.DeclaringEntityType.Name, configurationSource) : model.AddEntityType(Metadata.DeclaringEntityType.ClrType, configurationSource));
					}
				}
			}
			internalEntityTypeBuilder2 = entityType2.Builder;
		}
		if (!Metadata.GetConfigurationSource().Overrides(ConfigurationSource.Explicit) && ((entityType2.HasDefiningNavigation() && (Metadata.PrincipalToDependent?.Name != entityType2.DefiningNavigationName || entityType != entityType2.DefiningEntityType)) || (entityType2.FindOwnership() != null && Metadata.PrincipalToDependent != null)))
		{
			return null;
		}
		if (entityType2.GetForeignKeys().Contains(Metadata, Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance))
		{
			return Metadata.Builder;
		}
		ConfigurationSource? principalKeyConfigurationSource = Metadata.GetPrincipalKeyConfigurationSource();
		IReadOnlyList<Property> readOnlyList = ((principalKeyConfigurationSource.HasValue && principalKeyConfigurationSource.GetValueOrDefault().Overrides(configurationSource)) ? (internalEntityTypeBuilder.GetActualProperties(Metadata.PrincipalKey.Properties, configurationSource) ?? new List<Property>()) : new List<Property>());
		if (readOnlyList.Count == 0)
		{
			principalKeyConfigurationSource = Metadata.GetPropertiesConfigurationSource();
			if (!principalKeyConfigurationSource.HasValue || !principalKeyConfigurationSource.GetValueOrDefault().Overrides(ConfigurationSource.Explicit))
			{
				goto IL_0530;
			}
		}
		principalKeyConfigurationSource = Metadata.GetPropertiesConfigurationSource();
		if (!principalKeyConfigurationSource.HasValue || !principalKeyConfigurationSource.GetValueOrDefault().Overrides(configurationSource))
		{
			goto IL_0530;
		}
		IReadOnlyList<Property> readOnlyList2 = internalEntityTypeBuilder2.GetActualProperties(Metadata.Properties, configurationSource) ?? new List<Property>();
		goto IL_055c;
		IL_0530:
		readOnlyList2 = new List<Property>();
		goto IL_055c;
		IL_055c:
		if (readOnlyList2.Count != 0)
		{
			if (!CanSetForeignKey(readOnlyList2, entityType2, (readOnlyList.Count != 0) ? readOnlyList : Metadata.PrincipalKey.Properties, entityType, configurationSource, out var resetPrincipalKey))
			{
				readOnlyList2 = new List<Property>();
			}
			else if (resetPrincipalKey)
			{
				readOnlyList = new List<Property>();
			}
		}
		return ReplaceForeignKey(configurationSource, internalEntityTypeBuilder, internalEntityTypeBuilder2, null, null, readOnlyList2, null, readOnlyList);
	}

	public static bool AreCompatible([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToPrincipal, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToDependent, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> principalProperties, bool? isUnique, ConfigurationSource? configurationSource)
	{
		return ForeignKey.AreCompatible(principalEntityType, dependentEntityType, navigationToPrincipal, navigationToDependent, dependentProperties, principalProperties, isUnique, configurationSource == ConfigurationSource.Explicit);
	}

	private bool CanSetRelatedTypes(EntityType principalEntityType, EntityType dependentEntityType, bool strictPrincipal, MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, ConfigurationSource? configurationSource, bool shouldThrow, out bool shouldInvert, out bool shouldResetToPrincipal, out bool shouldResetToDependent, out bool shouldResetPrincipalProperties, out bool shouldResetDependentProperties, out bool? shouldBeUnique)
	{
		shouldInvert = false;
		shouldResetToPrincipal = false;
		shouldResetToDependent = false;
		shouldResetPrincipalProperties = false;
		shouldResetDependentProperties = false;
		shouldBeUnique = null;
		bool num = principalEntityType.IsSameHierarchy(dependentEntityType) && ((navigationToPrincipal.HasValue && navigationToPrincipal.Value.Name == Metadata.PrincipalToDependent?.Name) || (navigationToDependent.HasValue && navigationToDependent.Value.Name == Metadata.DependentToPrincipal?.Name));
		bool flag = false;
		if (!num && CanSetRelatedTypes(principalEntityType, dependentEntityType, navigationToPrincipal, navigationToDependent, configurationSource, inverted: false, shouldThrow: false, out shouldResetToPrincipal, out shouldResetToDependent, out shouldResetPrincipalProperties, out shouldResetDependentProperties, out shouldBeUnique))
		{
			if (!shouldResetToPrincipal && !shouldResetToDependent)
			{
				return true;
			}
			flag = true;
		}
		bool flag2 = configurationSource?.Overrides(Metadata.GetPrincipalEndConfigurationSource()) ?? false;
		if ((!strictPrincipal || flag2) && CanSetRelatedTypes(dependentEntityType, principalEntityType, navigationToDependent, navigationToPrincipal, configurationSource, strictPrincipal, shouldThrow: false, out var shouldResetToPrincipal2, out var shouldResetToDependent2, out var _, out var _, out var shouldBeUnique2) && (!flag || (!shouldResetToPrincipal2 && !shouldResetToDependent2)))
		{
			shouldInvert = true;
			shouldResetToPrincipal = shouldResetToDependent2;
			shouldResetToDependent = shouldResetToPrincipal2;
			shouldBeUnique = shouldBeUnique2;
			return true;
		}
		if (!flag && flag2 && shouldThrow)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypesNotInRelationship(dependentEntityType.DisplayName(), principalEntityType.DisplayName(), Metadata.DeclaringEntityType.DisplayName(), Metadata.PrincipalEntityType.DisplayName()));
		}
		return flag;
	}

	private bool CanSetRelatedTypes(EntityType principalEntityType, EntityType dependentEntityType, MemberIdentity? navigationToPrincipal, MemberIdentity? navigationToDependent, ConfigurationSource? configurationSource, bool inverted, bool shouldThrow, out bool shouldResetToPrincipal, out bool shouldResetToDependent, out bool shouldResetPrincipalProperties, out bool shouldResetDependentProperties, out bool? shouldBeUnique)
	{
		shouldResetToPrincipal = false;
		shouldResetToDependent = false;
		shouldResetPrincipalProperties = false;
		shouldResetDependentProperties = false;
		shouldBeUnique = null;
		if (!Metadata.DeclaringEntityType.IsSameHierarchy(dependentEntityType))
		{
			return false;
		}
		if (!Metadata.PrincipalEntityType.IsSameHierarchy(principalEntityType))
		{
			return false;
		}
		bool removeConflictingNavigations;
		bool removeOppositeNavigation;
		if (navigationToPrincipal.HasValue)
		{
			if (!configurationSource.HasValue || !CanSetNavigation(navigationToPrincipal.Value, pointsToPrincipal: true, configurationSource.Value, shouldThrow, overrideSameSource: true, out var _, out removeConflictingNavigations, out removeOppositeNavigation))
			{
				return false;
			}
			if (Metadata.DependentToPrincipal != null && navigationToPrincipal.Value.Name != Metadata.DependentToPrincipal.Name)
			{
				shouldResetToPrincipal = true;
			}
		}
		else
		{
			bool? shouldBeUnique3 = null;
			MemberInfo memberInfo = Metadata.DependentToPrincipal?.GetIdentifyingMemberInfo();
			if (memberInfo != null && !IsCompatible(memberInfo, !inverted, inverted ? principalEntityType.ClrType : dependentEntityType.ClrType, inverted ? dependentEntityType.ClrType : principalEntityType.ClrType, shouldThrow, out shouldBeUnique3))
			{
				if (!configurationSource.HasValue || !CanSetNavigation((string)null, pointsToPrincipal: true, (ConfigurationSource?)configurationSource.Value))
				{
					return false;
				}
				shouldResetToPrincipal = true;
			}
			if (inverted)
			{
				shouldBeUnique = shouldBeUnique3;
			}
		}
		if (navigationToDependent.HasValue)
		{
			if (!configurationSource.HasValue || !CanSetNavigation(navigationToDependent.Value, pointsToPrincipal: false, configurationSource.Value, shouldThrow, overrideSameSource: true, out var shouldBeUnique4, out removeOppositeNavigation, out removeConflictingNavigations))
			{
				return false;
			}
			if (Metadata.PrincipalToDependent != null && navigationToDependent.Value.Name != Metadata.PrincipalToDependent.Name)
			{
				shouldResetToDependent = true;
			}
			if (shouldBeUnique4.HasValue)
			{
				shouldBeUnique = shouldBeUnique4;
			}
		}
		else
		{
			bool? shouldBeUnique5 = null;
			MemberInfo memberInfo2 = Metadata.PrincipalToDependent?.GetIdentifyingMemberInfo();
			if (memberInfo2 != null && !IsCompatible(memberInfo2, inverted, inverted ? principalEntityType.ClrType : dependentEntityType.ClrType, inverted ? dependentEntityType.ClrType : principalEntityType.ClrType, shouldThrow, out shouldBeUnique5))
			{
				if (!configurationSource.HasValue || !CanSetNavigation((string)null, pointsToPrincipal: false, (ConfigurationSource?)configurationSource.Value))
				{
					return false;
				}
				shouldResetToDependent = true;
			}
			if (!inverted && shouldBeUnique5.HasValue)
			{
				shouldBeUnique = shouldBeUnique5;
			}
		}
		if (shouldBeUnique.HasValue && !CanSetIsUnique(shouldBeUnique.Value, configurationSource, out removeConflictingNavigations))
		{
			return false;
		}
		if (!Property.AreCompatible(Metadata.PrincipalKey.Properties, principalEntityType))
		{
			if (!configurationSource.HasValue || !configurationSource.Value.Overrides(Metadata.GetPrincipalKeyConfigurationSource()))
			{
				return false;
			}
			shouldResetPrincipalProperties = true;
		}
		if (!Property.AreCompatible(Metadata.Properties, dependentEntityType))
		{
			if (!configurationSource.HasValue || !configurationSource.Value.Overrides(Metadata.GetPropertiesConfigurationSource()))
			{
				return false;
			}
			shouldResetDependentProperties = true;
		}
		return true;
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasEntityTypes(IConventionEntityType principalEntityType, IConventionEntityType dependentEntityType, bool fromDataAnnotation)
	{
		return HasEntityTypes((EntityType)principalEntityType, (EntityType)dependentEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanInvert(IReadOnlyList<IConventionProperty> newForeignKeyProperties, bool fromDataAnnotation)
	{
		return CanInvert((IReadOnlyList<Property>)newForeignKeyProperties, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> properties, bool fromDataAnnotation)
	{
		return HasForeignKey(properties, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasForeignKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return HasForeignKey((properties as IReadOnlyList<Property>) ?? properties?.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> properties, bool fromDataAnnotation)
	{
		return CanSetForeignKey(properties, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetForeignKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return CanSetForeignKey((properties as IReadOnlyList<Property>) ?? properties?.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> properties, bool fromDataAnnotation)
	{
		return HasPrincipalKey(properties, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasPrincipalKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return HasPrincipalKey((properties as IReadOnlyList<Property>) ?? properties?.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> properties, bool fromDataAnnotation)
	{
		return CanSetPrincipalKey(properties, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetPrincipalKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return CanSetPrincipalKey((properties as IReadOnlyList<Property>) ?? properties?.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasNavigation(string name, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return HasNavigation(name, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasNavigation(MemberInfo property, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return HasNavigation(property, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasNavigations(string navigationToPrincipalName, string navigationToDependentName, bool fromDataAnnotation)
	{
		return HasNavigations(navigationToPrincipalName, navigationToDependentName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasNavigations(MemberInfo navigationToPrincipal, MemberInfo navigationToDependent, bool fromDataAnnotation)
	{
		return HasNavigations(navigationToPrincipal, navigationToDependent, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetNavigation(MemberInfo property, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return CanSetNavigation(property, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetNavigation(string name, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return CanSetNavigation(name, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetNavigations(MemberInfo navigationToPrincipal, MemberInfo navigationToDependent, bool fromDataAnnotation)
	{
		return CanSetNavigations(navigationToPrincipal, navigationToDependent, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasField(string fieldName, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return HasField(fieldName, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.HasField(FieldInfo fieldInfo, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return HasField(fieldInfo, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetField(string fieldName, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return CanSetField(fieldName, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetField(FieldInfo fieldInfo, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return CanSetField(fieldInfo, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.IsEagerLoaded(bool? eagerLoaded, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return IsEagerLoaded(eagerLoaded, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetIsEagerLoaded(bool? eagerLoaded, bool pointsToPrincipal, bool fromDataAnnotation)
	{
		return CanSetIsEagerLoaded(eagerLoaded, pointsToPrincipal, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetNavigations(string navigationToPrincipalName, string navigationToDependentName, bool fromDataAnnotation)
	{
		return CanSetNavigations(navigationToPrincipalName, navigationToDependentName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.IsRequired(bool? required, bool fromDataAnnotation)
	{
		return IsRequired(required, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetIsRequired(bool? required, bool fromDataAnnotation)
	{
		return CanSetIsRequired(required, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.IsOwnership(bool? ownership, bool fromDataAnnotation)
	{
		return IsOwnership(ownership, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetIsOwnership(bool? ownership, bool fromDataAnnotation)
	{
		return CanSetIsOwnership(ownership, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.OnDelete(DeleteBehavior? deleteBehavior, bool fromDataAnnotation)
	{
		return OnDelete(deleteBehavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetOnDelete(DeleteBehavior? deleteBehavior, bool fromDataAnnotation)
	{
		return CanSetDeleteBehavior(deleteBehavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionRelationshipBuilder.IsUnique(bool? unique, bool fromDataAnnotation)
	{
		return IsUnique(unique, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionRelationshipBuilder.CanSetIsUnique(bool? unique, bool fromDataAnnotation)
	{
		return CanSetIsUnique(unique, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
