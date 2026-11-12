using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class InversePropertyAttributeConvention : NavigationAttributeConventionBase<InversePropertyAttribute>, IModelFinalizedConvention, IConvention
{
	public InversePropertyAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	public override void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, MemberInfo navigationMemberInfo, Type targetClrType, InversePropertyAttribute attribute, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		Process(entityTypeBuilder, navigationMemberInfo, targetClrType, attribute);
	}

	private void Process(IConventionEntityTypeBuilder entityTypeBuilder, MemberInfo navigationMemberInfo, Type targetClrType, InversePropertyAttribute attribute)
	{
		if (entityTypeBuilder.CanAddNavigation(navigationMemberInfo.GetSimpleMemberName(), fromDataAnnotation: true))
		{
			InternalEntityTypeBuilder targetEntityTypeBuilder = ((InternalEntityTypeBuilder)entityTypeBuilder).GetTargetEntityTypeBuilder(targetClrType, navigationMemberInfo, ConfigurationSource.DataAnnotation);
			if (targetEntityTypeBuilder != null)
			{
				ConfigureInverseNavigation(entityTypeBuilder, navigationMemberInfo, targetEntityTypeBuilder, attribute);
			}
		}
	}

	private IConventionRelationshipBuilder ConfigureInverseNavigation(IConventionEntityTypeBuilder entityTypeBuilder, MemberInfo navigationMemberInfo, IConventionEntityTypeBuilder targetEntityTypeBuilder, InversePropertyAttribute attribute)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		Type clrType = targetEntityTypeBuilder.Metadata.ClrType;
		PropertyInfo propertyInfo = targetEntityTypeBuilder.Metadata.GetRuntimeProperties().Values.FirstOrDefault((PropertyInfo p) => string.Equals(p.GetSimpleMemberName(), attribute.Property, StringComparison.OrdinalIgnoreCase));
		if (propertyInfo == null || !Dependencies.MemberClassifier.FindCandidateNavigationPropertyType(propertyInfo).GetTypeInfo().IsAssignableFrom(metadata.ClrType.GetTypeInfo()))
		{
			throw new InvalidOperationException(CoreStrings.InvalidNavigationWithInverseProperty(navigationMemberInfo.Name, metadata.DisplayName(), attribute.Property, clrType.ShortDisplayName()));
		}
		if (object.Equals(propertyInfo, navigationMemberInfo))
		{
			throw new InvalidOperationException(CoreStrings.SelfReferencingNavigationWithInverseProperty(navigationMemberInfo.Name, metadata.DisplayName(), navigationMemberInfo.Name, metadata.DisplayName()));
		}
		if (Attribute.IsDefined(propertyInfo, typeof(InversePropertyAttribute)) && ((MemberInfo)propertyInfo).GetCustomAttribute<InversePropertyAttribute>(inherit: true).Property != navigationMemberInfo.GetSimpleMemberName())
		{
			throw new InvalidOperationException(CoreStrings.InversePropertyMismatch(navigationMemberInfo.Name, metadata.DisplayName(), propertyInfo.Name, targetEntityTypeBuilder.Metadata.DisplayName()));
		}
		List<(MemberInfo, IConventionEntityType)> referencingNavigationsWithAttribute = AddInverseNavigation(metadata, navigationMemberInfo, targetEntityTypeBuilder.Metadata, propertyInfo);
		(MemberInfo, IConventionEntityType)? tuple = FindAmbiguousInverse(navigationMemberInfo, metadata, referencingNavigationsWithAttribute);
		IConventionEntityType baseType = targetEntityTypeBuilder.Metadata.BaseType;
		while (!tuple.HasValue && baseType != null)
		{
			Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)> inverseNavigations = GetInverseNavigations(baseType);
			if (inverseNavigations != null && inverseNavigations.TryGetValue(propertyInfo.Name, out var value))
			{
				referencingNavigationsWithAttribute = value.Item2;
				tuple = FindAmbiguousInverse(navigationMemberInfo, metadata, referencingNavigationsWithAttribute);
			}
			baseType = baseType.BaseType;
		}
		if (tuple.HasValue)
		{
			IConventionNavigation conventionNavigation = targetEntityTypeBuilder.Metadata.FindNavigation(propertyInfo)?.FindInverse();
			IConventionEntityType entityType = conventionNavigation?.DeclaringEntityType;
			if (conventionNavigation != null && IsAmbiguousInverse(conventionNavigation.GetIdentifyingMemberInfo(), entityType, referencingNavigationsWithAttribute))
			{
				IConventionForeignKey foreignKey = conventionNavigation.ForeignKey;
				if (foreignKey.IsOwnership || foreignKey.DeclaringEntityType.Builder.HasNoRelationship(foreignKey, fromDataAnnotation: true) == null)
				{
					foreignKey.Builder.HasNavigation((string)null, conventionNavigation.IsDependentToPrincipal(), fromDataAnnotation: true);
				}
			}
			IConventionNavigation conventionNavigation2 = metadata.FindNavigation(navigationMemberInfo);
			if (conventionNavigation2 != null)
			{
				IConventionForeignKey foreignKey2 = conventionNavigation2.ForeignKey;
				if (foreignKey2.IsOwnership || foreignKey2.DeclaringEntityType.Builder.HasNoRelationship(foreignKey2, fromDataAnnotation: true) == null)
				{
					foreignKey2.Builder.HasNavigation((string)null, conventionNavigation2.IsDependentToPrincipal(), fromDataAnnotation: true);
				}
			}
			IConventionNavigation conventionNavigation3 = FindActualEntityType(tuple.Value.Item2).FindNavigation(tuple.Value.Item1);
			if (conventionNavigation3 != null)
			{
				IConventionForeignKey foreignKey3 = conventionNavigation3.ForeignKey;
				if (foreignKey3.IsOwnership || foreignKey3.DeclaringEntityType.Builder.HasNoRelationship(foreignKey3, fromDataAnnotation: true) == null)
				{
					foreignKey3.Builder.HasNavigation((string)null, conventionNavigation3.IsDependentToPrincipal(), fromDataAnnotation: true);
				}
			}
			return metadata.FindNavigation(navigationMemberInfo)?.ForeignKey.Builder;
		}
		IConventionForeignKey conventionForeignKey = metadata.FindOwnership();
		if (conventionForeignKey != null && conventionForeignKey.PrincipalEntityType == targetEntityTypeBuilder.Metadata && conventionForeignKey.PrincipalToDependent?.GetIdentifyingMemberInfo() != propertyInfo)
		{
			Dependencies.Logger.NonOwnershipInverseNavigationWarning(metadata, navigationMemberInfo, targetEntityTypeBuilder.Metadata, propertyInfo, conventionForeignKey.PrincipalToDependent?.GetIdentifyingMemberInfo());
			return null;
		}
		if (metadata.DefiningEntityType != null && metadata.DefiningEntityType == targetEntityTypeBuilder.Metadata && metadata.DefiningNavigationName != propertyInfo.GetSimpleMemberName())
		{
			Dependencies.Logger.NonDefiningInverseNavigationWarning(metadata, navigationMemberInfo, targetEntityTypeBuilder.Metadata, propertyInfo, metadata.DefiningEntityType.GetRuntimeProperties()[metadata.DefiningNavigationName]);
			return null;
		}
		if (!metadata.Model.FindIsOwnedConfigurationSource(metadata.ClrType).HasValue || metadata.IsInOwnershipPath(targetEntityTypeBuilder.Metadata))
		{
			return targetEntityTypeBuilder.HasRelationship(metadata, propertyInfo, navigationMemberInfo, setTargetAsPrincipal: false, fromDataAnnotation: true);
		}
		return targetEntityTypeBuilder.HasOwnership(entityTypeBuilder.Metadata.ClrType, propertyInfo, navigationMemberInfo, fromDataAnnotation: true);
	}

	public override void ProcessEntityTypeIgnored(IConventionModelBuilder modelBuilder, Type type, MemberInfo navigationMemberInfo, Type targetClrType, InversePropertyAttribute attribute, IConventionContext<string> context)
	{
		Type declaringType = navigationMemberInfo.DeclaringType;
		if (modelBuilder.Metadata.FindEntityType(declaringType) != null)
		{
			return;
		}
		foreach (IConventionEntityType item in modelBuilder.Metadata.FindLeastDerivedEntityTypes(declaringType, (IConventionEntityType t) => !t.Builder.IsIgnored(navigationMemberInfo.GetSimpleMemberName(), fromDataAnnotation: true)))
		{
			Process(item.Builder, navigationMemberInfo, targetClrType, attribute);
		}
	}

	public override void ProcessNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation, InversePropertyAttribute attribute, IConventionContext<IConventionNavigation> context)
	{
		if (relationshipBuilder.Metadata.DeclaringEntityType.HasDefiningNavigation() || relationshipBuilder.Metadata.DeclaringEntityType.IsOwned() || relationshipBuilder.Metadata.PrincipalEntityType.HasDefiningNavigation() || relationshipBuilder.Metadata.PrincipalEntityType.IsOwned())
		{
			return;
		}
		IConventionRelationshipBuilder conventionRelationshipBuilder = ConfigureInverseNavigation(navigation.DeclaringEntityType.Builder, navigation.GetIdentifyingMemberInfo(), navigation.GetTargetType().Builder, attribute);
		if (conventionRelationshipBuilder != relationshipBuilder)
		{
			if (conventionRelationshipBuilder == null)
			{
				context.StopProcessingIfChanged(null);
				return;
			}
			IConventionNavigation result = (navigation.IsDependentToPrincipal() ? conventionRelationshipBuilder.Metadata.DependentToPrincipal : conventionRelationshipBuilder.Metadata.PrincipalToDependent);
			context.StopProcessingIfChanged(result);
		}
	}

	public override void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, MemberInfo navigationMemberInfo, Type targetClrType, InversePropertyAttribute attribute, IConventionContext<IConventionEntityType> context)
	{
		Type clrType = entityTypeBuilder.Metadata.ClrType;
		if (!(navigationMemberInfo.DeclaringType != clrType))
		{
			return;
		}
		if (newBaseType == null)
		{
			Process(entityTypeBuilder, navigationMemberInfo, targetClrType, attribute);
			return;
		}
		IConventionEntityType conventionEntityType = entityTypeBuilder.Metadata.Model.FindEntityType(targetClrType);
		if (conventionEntityType != null)
		{
			RemoveInverseNavigation(entityTypeBuilder.Metadata, navigationMemberInfo, conventionEntityType);
		}
	}

	public override void ProcessEntityTypeMemberIgnored(IConventionEntityTypeBuilder entityTypeBuilder, MemberInfo navigationMemberInfo, Type targetClrType, InversePropertyAttribute attribute, IConventionContext<string> context)
	{
		EntityType entityType = ((InternalEntityTypeBuilder)entityTypeBuilder).GetTargetEntityTypeBuilder(targetClrType, navigationMemberInfo, null)?.Metadata;
		if (entityType != null)
		{
			RemoveInverseNavigation(entityTypeBuilder.Metadata, navigationMemberInfo, entityType);
		}
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		IConventionModel metadata = modelBuilder.Metadata;
		foreach (IConventionEntityType entityType in metadata.GetEntityTypes())
		{
			Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)> inverseNavigations = GetInverseNavigations(entityType);
			if (inverseNavigations == null)
			{
				continue;
			}
			foreach (var value2 in inverseNavigations.Values)
			{
				foreach (var item2 in value2.Item2)
				{
					(MemberInfo, IConventionEntityType)? tuple = FindAmbiguousInverse(item2.Item1, item2.Item2, value2.Item2);
					IConventionEntityType baseType = entityType.BaseType;
					while (!tuple.HasValue && baseType != null)
					{
						Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)> inverseNavigations2 = GetInverseNavigations(baseType);
						if (inverseNavigations2 != null && inverseNavigations2.TryGetValue(value2.Item1.Name, out var value))
						{
							List<(MemberInfo, IConventionEntityType)> item = value.Item2;
							tuple = FindAmbiguousInverse(item2.Item1, item2.Item2, item);
						}
						baseType = baseType.BaseType;
					}
					if (tuple.HasValue)
					{
						Dependencies.Logger.MultipleInversePropertiesSameTargetWarning(new Tuple<MemberInfo, Type>[2]
						{
							Tuple.Create(item2.Item1, item2.Item2.ClrType),
							Tuple.Create(tuple.Value.Item1, tuple.Value.Item2.ClrType)
						}, value2.Item1, entityType.ClrType);
						break;
					}
				}
			}
		}
		foreach (IConventionEntityType entityType2 in metadata.GetEntityTypes())
		{
			entityType2.RemoveAnnotation("InversePropertyAttributeConvention:InverseNavigations");
		}
	}

	public static bool IsAmbiguous([JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo navigation, [JetBrains.Annotations.NotNull] IConventionEntityType targetEntityType)
	{
		if (!Attribute.IsDefined(navigation, typeof(InversePropertyAttribute)))
		{
			return false;
		}
		while (targetEntityType != null)
		{
			Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)> inverseNavigations = GetInverseNavigations(targetEntityType);
			if (inverseNavigations != null)
			{
				foreach (var value in inverseNavigations.Values)
				{
					if (value.Item1.GetMemberType().IsAssignableFrom(entityType.ClrType) && IsAmbiguousInverse(navigation, entityType, value.Item2))
					{
						return true;
					}
				}
			}
			targetEntityType = targetEntityType.BaseType;
		}
		return false;
	}

	private static bool IsAmbiguousInverse(MemberInfo navigation, IConventionEntityType entityType, List<(MemberInfo, IConventionEntityType)> referencingNavigationsWithAttribute)
	{
		return FindAmbiguousInverse(navigation, entityType, referencingNavigationsWithAttribute).HasValue;
	}

	private static (MemberInfo, IConventionEntityType)? FindAmbiguousInverse(MemberInfo navigation, IConventionEntityType entityType, List<(MemberInfo, IConventionEntityType)> referencingNavigationsWithAttribute)
	{
		List<(MemberInfo, IConventionEntityType)> list = null;
		(MemberInfo, IConventionEntityType)? result = null;
		foreach (var item in referencingNavigationsWithAttribute)
		{
			IConventionEntityType conventionEntityType = FindActualEntityType(item.Item2);
			if (conventionEntityType == null || conventionEntityType.Builder.IsIgnored(item.Item1.GetSimpleMemberName(), fromDataAnnotation: true))
			{
				if (list == null)
				{
					list = new List<(MemberInfo, IConventionEntityType)>();
				}
				list.Add(item);
			}
			else if (!item.Item1.IsSameAs(navigation) || !entityType.IsSameHierarchy(conventionEntityType))
			{
				result = item;
				break;
			}
		}
		if (list != null)
		{
			foreach (var item2 in list)
			{
				referencingNavigationsWithAttribute.Remove(item2);
			}
		}
		return result;
	}

	private static List<(MemberInfo, IConventionEntityType)> AddInverseNavigation(IConventionEntityType entityType, MemberInfo navigation, IConventionEntityType targetEntityType, MemberInfo inverseNavigation)
	{
		Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)> dictionary = GetInverseNavigations(targetEntityType);
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)>();
			SetInverseNavigations(targetEntityType.Builder, dictionary);
		}
		List<(MemberInfo, IConventionEntityType)> list;
		if (!dictionary.TryGetValue(inverseNavigation.Name, out var value))
		{
			list = new List<(MemberInfo, IConventionEntityType)>();
			dictionary[inverseNavigation.Name] = (inverseNavigation, list);
		}
		else
		{
			list = value.Item2;
		}
		foreach (var item in list)
		{
			if (item.Item1.IsSameAs(navigation) && item.Item2.ClrType == entityType.ClrType && FindActualEntityType(item.Item2) == entityType)
			{
				return list;
			}
		}
		list.Add((navigation, entityType));
		return list;
	}

	private static void RemoveInverseNavigation(IConventionEntityType entityType, MemberInfo navigation, IConventionEntityType targetEntityType)
	{
		Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)> inverseNavigations = GetInverseNavigations(targetEntityType);
		if (inverseNavigations == null)
		{
			return;
		}
		foreach (var value in inverseNavigations.Values)
		{
			MemberInfo item = value.Item1;
			List<(MemberInfo, IConventionEntityType)> item2 = value.Item2;
			for (int i = 0; i < item2.Count; i++)
			{
				(MemberInfo, IConventionEntityType) tuple = item2[i];
				if (!tuple.Item1.IsSameAs(navigation) || !(tuple.Item2.ClrType == entityType.ClrType) || FindActualEntityType(tuple.Item2) != entityType)
				{
					continue;
				}
				item2.RemoveAt(i);
				if (item2.Count == 0)
				{
					inverseNavigations.Remove(item.Name);
				}
				if (item2.Count == 1)
				{
					IConventionEntityType conventionEntityType = FindActualEntityType(item2[0].Item2);
					if (conventionEntityType != null)
					{
						targetEntityType.Builder.HasRelationship(conventionEntityType, item, item2[0].Item1, setTargetAsPrincipal: false, fromDataAnnotation: true);
					}
				}
				return;
			}
		}
	}

	private static IConventionEntityType FindActualEntityType(IConventionEntityType entityType)
	{
		return ((Model)entityType.Model).FindActualEntityType((EntityType)entityType);
	}

	private static Dictionary<string, (MemberInfo Navigation, List<(MemberInfo, IConventionEntityType)> References)> GetInverseNavigations(IConventionAnnotatable entityType)
	{
		return entityType.FindAnnotation("InversePropertyAttributeConvention:InverseNavigations")?.Value as Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)>;
	}

	private static void SetInverseNavigations(IConventionAnnotatableBuilder entityTypeBuilder, Dictionary<string, (MemberInfo, List<(MemberInfo, IConventionEntityType)>)> inverseNavigations)
	{
		entityTypeBuilder.HasAnnotation("InversePropertyAttributeConvention:InverseNavigations", inverseNavigations);
	}
}
