using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RelationshipDiscoveryConvention : IEntityTypeAddedConvention, IConvention, IEntityTypeIgnoredConvention, IEntityTypeBaseTypeChangedConvention, INavigationRemovedConvention, IEntityTypeMemberIgnoredConvention, INavigationAddedConvention, IForeignKeyOwnershipChangedConvention
{
	private class MemberInfoNameComparer : IComparer<MemberInfo>
	{
		public static readonly MemberInfoNameComparer Instance = new MemberInfoNameComparer();

		private MemberInfoNameComparer()
		{
		}

		public int Compare(MemberInfo x, MemberInfo y)
		{
			return StringComparer.Ordinal.Compare(x.Name, y.Name);
		}
	}

	private class RelationshipCandidate
	{
		public IConventionEntityTypeBuilder TargetTypeBuilder
		{
			[DebuggerStepThrough]
			get;
		}

		public List<PropertyInfo> NavigationProperties
		{
			[DebuggerStepThrough]
			get;
		}

		public List<PropertyInfo> InverseProperties
		{
			[DebuggerStepThrough]
			get;
		}

		public RelationshipCandidate(IConventionEntityTypeBuilder targetTypeBuilder, List<PropertyInfo> navigations, List<PropertyInfo> inverseNavigations)
		{
			TargetTypeBuilder = targetTypeBuilder;
			NavigationProperties = navigations;
			InverseProperties = inverseNavigations;
		}
	}

	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public RelationshipDiscoveryConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	private void DiscoverRelationships(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext context)
	{
		if (!entityTypeBuilder.Metadata.HasClrType() || entityTypeBuilder.ModelBuilder.IsIgnored(entityTypeBuilder.Metadata.ClrType))
		{
			return;
		}
		IReadOnlyList<RelationshipCandidate> relationshipCandidates = FindRelationshipCandidates(entityTypeBuilder);
		relationshipCandidates = RemoveIncompatibleWithExistingRelationships(relationshipCandidates, entityTypeBuilder);
		relationshipCandidates = RemoveInheritedInverseNavigations(relationshipCandidates);
		relationshipCandidates = RemoveSingleSidedBaseNavigations(relationshipCandidates, entityTypeBuilder);
		using (context.DelayConventions())
		{
			CreateRelationships(relationshipCandidates, entityTypeBuilder);
		}
	}

	private IReadOnlyList<RelationshipCandidate> FindRelationshipCandidates(IConventionEntityTypeBuilder entityTypeBuilder)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		IConventionModel model = metadata.Model;
		Dictionary<IConventionEntityType, RelationshipCandidate> dictionary = new Dictionary<IConventionEntityType, RelationshipCandidate>();
		IConventionForeignKey conventionForeignKey = entityTypeBuilder.Metadata.FindOwnership();
		if (conventionForeignKey == null && model.FindIsOwnedConfigurationSource(entityTypeBuilder.Metadata.ClrType).HasValue)
		{
			return dictionary.Values.ToList();
		}
		foreach (KeyValuePair<PropertyInfo, Type> navigationCandidate in GetNavigationCandidates(metadata))
		{
			PropertyInfo key = navigationCandidate.Key;
			Type value = navigationCandidate.Value;
			if (!IsCandidateNavigationProperty(entityTypeBuilder, key.GetSimpleMemberName(), key) || (model.FindIsOwnedConfigurationSource(value).HasValue && HasDeclaredAmbiguousNavigationsTo(metadata, value)))
			{
				continue;
			}
			IConventionEntityTypeBuilder targetEntityTypeBuilder = ((InternalEntityTypeBuilder)entityTypeBuilder).GetTargetEntityTypeBuilder(value, key, ConfigurationSource.Convention);
			if (targetEntityTypeBuilder == null)
			{
				continue;
			}
			IConventionEntityType metadata2 = targetEntityTypeBuilder.Metadata;
			if (metadata2.IsKeyless)
			{
				continue;
			}
			if (metadata.Builder == null)
			{
				foreach (RelationshipCandidate value4 in dictionary.Values)
				{
					IConventionEntityType metadata3 = value4.TargetTypeBuilder.Metadata;
					if (metadata3.Builder != null && metadata3.HasDefiningNavigation() && metadata3.DefiningEntityType.FindNavigation(metadata3.DefiningNavigationName) == null)
					{
						metadata3.Builder.ModelBuilder.HasNoEntityType(metadata3);
					}
				}
				return Array.Empty<RelationshipCandidate>();
			}
			if (!model.FindIsOwnedConfigurationSource(value).HasValue)
			{
				IConventionForeignKey conventionForeignKey2 = metadata2.FindOwnership();
				if (conventionForeignKey2 != null && (conventionForeignKey2.PrincipalEntityType != metadata || conventionForeignKey2.PrincipalToDependent?.Name != key.GetSimpleMemberName()) && (conventionForeignKey == null || conventionForeignKey.PrincipalEntityType != metadata2))
				{
					continue;
				}
			}
			if (dictionary.TryGetValue(metadata2, out var value2))
			{
				if ((metadata2 != metadata || !value2.InverseProperties.Contains(key)) && !value2.NavigationProperties.Contains(key))
				{
					value2.NavigationProperties.Add(key);
				}
				continue;
			}
			List<PropertyInfo> navigations = new List<PropertyInfo> { key };
			List<PropertyInfo> list = new List<PropertyInfo>();
			if (!metadata.IsKeyless)
			{
				foreach (KeyValuePair<PropertyInfo, Type> navigationCandidate2 in GetNavigationCandidates(metadata2))
				{
					PropertyInfo key2 = navigationCandidate2.Key;
					Type value3 = navigationCandidate2.Value;
					if ((!(value3 != metadata.ClrType) || (value3.IsAssignableFrom(metadata.ClrType) && (model.FindIsOwnedConfigurationSource(value).HasValue || metadata2.IsInOwnershipPath(metadata)))) && !key.IsSameAs(key2) && (conventionForeignKey == null || metadata2.IsInOwnershipPath(metadata) || (!metadata2.IsOwned() && model.FindIsOwnedConfigurationSource(value).HasValue) || (conventionForeignKey.PrincipalEntityType == metadata2 && !(conventionForeignKey.PrincipalToDependent?.Name != key2.GetSimpleMemberName()))) && (!metadata.HasDefiningNavigation() || metadata2.IsInDefinitionPath(metadata.ClrType) || (metadata.DefiningEntityType == metadata2 && !(metadata.DefiningNavigationName != key2.GetSimpleMemberName()))) && IsCandidateNavigationProperty(targetEntityTypeBuilder, key2.GetSimpleMemberName(), key2) && !list.Contains(key2))
					{
						list.Add(key2);
					}
				}
			}
			dictionary[metadata2] = new RelationshipCandidate(targetEntityTypeBuilder, navigations, list);
		}
		List<RelationshipCandidate> list2 = new List<RelationshipCandidate>();
		foreach (RelationshipCandidate value5 in dictionary.Values)
		{
			if (value5.TargetTypeBuilder.Metadata.Builder != null)
			{
				list2.Add(value5);
			}
			else if (value5.NavigationProperties.Count <= 1)
			{
				InternalEntityTypeBuilder targetEntityTypeBuilder2 = ((InternalEntityTypeBuilder)entityTypeBuilder).GetTargetEntityTypeBuilder(value5.TargetTypeBuilder.Metadata.ClrType, value5.NavigationProperties.Single(), ConfigurationSource.Convention);
				if (targetEntityTypeBuilder2 != null)
				{
					list2.Add(new RelationshipCandidate(targetEntityTypeBuilder2, value5.NavigationProperties, value5.InverseProperties));
				}
			}
		}
		return list2;
	}

	private static IReadOnlyList<RelationshipCandidate> RemoveIncompatibleWithExistingRelationships(IReadOnlyList<RelationshipCandidate> relationshipCandidates, IConventionEntityTypeBuilder entityTypeBuilder)
	{
		if (relationshipCandidates.Count == 0)
		{
			return relationshipCandidates;
		}
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		List<RelationshipCandidate> list = new List<RelationshipCandidate>();
		foreach (RelationshipCandidate relationshipCandidate in relationshipCandidates)
		{
			IConventionEntityTypeBuilder targetTypeBuilder = relationshipCandidate.TargetTypeBuilder;
			IConventionEntityType metadata2 = targetTypeBuilder.Metadata;
			while (relationshipCandidate.NavigationProperties.Count > 0)
			{
				PropertyInfo propertyInfo = relationshipCandidate.NavigationProperties[0];
				IConventionNavigation conventionNavigation = metadata.FindNavigation(propertyInfo.GetSimpleMemberName());
				if (conventionNavigation != null && (conventionNavigation.DeclaringEntityType != metadata || conventionNavigation.GetTargetType() != metadata2))
				{
					relationshipCandidate.NavigationProperties.Remove(propertyInfo);
					continue;
				}
				if (relationshipCandidate.NavigationProperties.Count == 1 && relationshipCandidate.InverseProperties.Count == 0)
				{
					break;
				}
				PropertyInfo propertyInfo2 = null;
				foreach (PropertyInfo inverseProperty in relationshipCandidate.InverseProperties)
				{
					if (IsCompatibleInverse(propertyInfo, inverseProperty, entityTypeBuilder, targetTypeBuilder))
					{
						if (!(propertyInfo2 == null))
						{
							goto end_IL_031a;
						}
						propertyInfo2 = inverseProperty;
					}
				}
				if (propertyInfo2 == null)
				{
					relationshipCandidate.NavigationProperties.Remove(propertyInfo);
					list.Add(new RelationshipCandidate(targetTypeBuilder, new List<PropertyInfo> { propertyInfo }, new List<PropertyInfo>()));
					if (relationshipCandidate.TargetTypeBuilder.Metadata == entityTypeBuilder.Metadata && relationshipCandidate.InverseProperties.Count > 0)
					{
						PropertyInfo item = relationshipCandidate.InverseProperties.First();
						if (!relationshipCandidate.NavigationProperties.Contains(item))
						{
							relationshipCandidate.NavigationProperties.Add(item);
						}
						relationshipCandidate.InverseProperties.Remove(item);
					}
					continue;
				}
				bool flag = true;
				foreach (PropertyInfo navigationProperty in relationshipCandidate.NavigationProperties)
				{
					if (navigationProperty != propertyInfo && IsCompatibleInverse(navigationProperty, propertyInfo2, entityTypeBuilder, targetTypeBuilder))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				relationshipCandidate.NavigationProperties.Remove(propertyInfo);
				relationshipCandidate.InverseProperties.Remove(propertyInfo2);
				list.Add(new RelationshipCandidate(targetTypeBuilder, new List<PropertyInfo> { propertyInfo }, new List<PropertyInfo> { propertyInfo2 }));
				if (relationshipCandidate.TargetTypeBuilder.Metadata == entityTypeBuilder.Metadata && relationshipCandidate.NavigationProperties.Count == 0 && relationshipCandidate.InverseProperties.Count > 0)
				{
					PropertyInfo item2 = relationshipCandidate.InverseProperties.First();
					if (!relationshipCandidate.NavigationProperties.Contains(item2))
					{
						relationshipCandidate.NavigationProperties.Add(item2);
					}
					relationshipCandidate.InverseProperties.Remove(item2);
				}
				continue;
				end_IL_031a:
				break;
			}
			if (relationshipCandidate.NavigationProperties.Count > 0 || relationshipCandidate.InverseProperties.Count > 0)
			{
				list.Add(relationshipCandidate);
			}
			else if (IsCandidateUnusedOwnedType(relationshipCandidate.TargetTypeBuilder.Metadata) && list.All((RelationshipCandidate c) => c.TargetTypeBuilder.Metadata != relationshipCandidate.TargetTypeBuilder.Metadata))
			{
				entityTypeBuilder.ModelBuilder.HasNoEntityType(relationshipCandidate.TargetTypeBuilder.Metadata);
			}
		}
		return list;
	}

	private static bool IsCompatibleInverse(PropertyInfo navigationProperty, PropertyInfo inversePropertyInfo, IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityTypeBuilder targetEntityTypeBuilder)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		IConventionNavigation conventionNavigation = metadata.FindNavigation(navigationProperty.GetSimpleMemberName());
		if (conventionNavigation != null && !CanMergeWith(conventionNavigation, inversePropertyInfo, targetEntityTypeBuilder))
		{
			return false;
		}
		IConventionNavigation conventionNavigation2 = targetEntityTypeBuilder.Metadata.FindNavigation(inversePropertyInfo.Name);
		if (conventionNavigation2 != null)
		{
			if (conventionNavigation2.DeclaringEntityType != targetEntityTypeBuilder.Metadata || !CanMergeWith(conventionNavigation2, navigationProperty, entityTypeBuilder))
			{
				return false;
			}
			IConventionEntityType targetType = conventionNavigation2.GetTargetType();
			if (!metadata.ClrType.GetTypeInfo().IsAssignableFrom(targetType.ClrType.GetTypeInfo()))
			{
				return false;
			}
		}
		return true;
	}

	private static bool CanMergeWith(IConventionNavigation existingNavigation, MemberInfo inverse, IConventionEntityTypeBuilder inverseEntityTypeBuilder)
	{
		IConventionForeignKey foreignKey = existingNavigation.ForeignKey;
		if (foreignKey.IsSelfReferencing() || foreignKey.GetRelatedEntityType(existingNavigation.DeclaringEntityType) == inverseEntityTypeBuilder.Metadata)
		{
			return foreignKey.Builder.CanSetNavigation(inverse, !existingNavigation.IsDependentToPrincipal());
		}
		return false;
	}

	private static IReadOnlyList<RelationshipCandidate> RemoveInheritedInverseNavigations(IReadOnlyList<RelationshipCandidate> relationshipCandidates)
	{
		if (relationshipCandidates.Count == 0)
		{
			return relationshipCandidates;
		}
		foreach (List<RelationshipCandidate> value in (from r in relationshipCandidates
			group r by r.TargetTypeBuilder.Metadata.GetRootType()).ToDictionary((IGrouping<IConventionEntityType, RelationshipCandidate> g) => g.Key, (IGrouping<IConventionEntityType, RelationshipCandidate> g) => g.ToList()).Values)
		{
			HashSet<RelationshipCandidate> filteredRelationshipCandidates = new HashSet<RelationshipCandidate>();
			foreach (RelationshipCandidate item in value)
			{
				RemoveInheritedInverseNavigations(item, value, filteredRelationshipCandidates);
			}
		}
		return relationshipCandidates;
	}

	private static void RemoveInheritedInverseNavigations(RelationshipCandidate relationshipCandidate, List<RelationshipCandidate> relationshipCandidatesHierarchy, HashSet<RelationshipCandidate> filteredRelationshipCandidates)
	{
		if (filteredRelationshipCandidates.Contains(relationshipCandidate) || (relationshipCandidate.NavigationProperties.Count > 1 && relationshipCandidate.InverseProperties.Count > 0) || relationshipCandidate.InverseProperties.Count > 1)
		{
			return;
		}
		filteredRelationshipCandidates.Add(relationshipCandidate);
		PropertyInfo inverseCandidate = relationshipCandidate.InverseProperties.FirstOrDefault();
		if (!(inverseCandidate != null))
		{
			return;
		}
		foreach (RelationshipCandidate item in relationshipCandidatesHierarchy.Where((RelationshipCandidate r) => r.TargetTypeBuilder != relationshipCandidate.TargetTypeBuilder && relationshipCandidate.TargetTypeBuilder.Metadata.IsAssignableFrom(r.TargetTypeBuilder.Metadata)))
		{
			item.InverseProperties.RemoveAll((PropertyInfo i) => i.GetSimpleMemberName() == inverseCandidate.GetSimpleMemberName());
			if (!filteredRelationshipCandidates.Contains(item))
			{
				RemoveInheritedInverseNavigations(item, relationshipCandidatesHierarchy, filteredRelationshipCandidates);
			}
		}
	}

	private static IReadOnlyList<RelationshipCandidate> RemoveSingleSidedBaseNavigations(IReadOnlyList<RelationshipCandidate> relationshipCandidates, IConventionEntityTypeBuilder entityTypeBuilder)
	{
		if (relationshipCandidates.Count == 0)
		{
			return relationshipCandidates;
		}
		List<RelationshipCandidate> list = new List<RelationshipCandidate>();
		foreach (RelationshipCandidate relationshipCandidate in relationshipCandidates)
		{
			if (relationshipCandidate.InverseProperties.Count > 0)
			{
				list.Add(relationshipCandidate);
				continue;
			}
			foreach (PropertyInfo item in relationshipCandidate.NavigationProperties.ToList())
			{
				if (entityTypeBuilder.Metadata.FindDerivedNavigations(item.GetSimpleMemberName()).Any((INavigation n) => n.FindInverse() != null))
				{
					relationshipCandidate.NavigationProperties.Remove(item);
				}
			}
			if (relationshipCandidate.NavigationProperties.Count > 0)
			{
				list.Add(relationshipCandidate);
			}
			else if (IsCandidateUnusedOwnedType(relationshipCandidate.TargetTypeBuilder.Metadata) && list.All((RelationshipCandidate c) => c.TargetTypeBuilder.Metadata != relationshipCandidate.TargetTypeBuilder.Metadata))
			{
				entityTypeBuilder.ModelBuilder.HasNoEntityType(relationshipCandidate.TargetTypeBuilder.Metadata);
			}
		}
		return list;
	}

	private void CreateRelationships(IEnumerable<RelationshipCandidate> relationshipCandidates, IConventionEntityTypeBuilder entityTypeBuilder)
	{
		List<IConventionEntityType> list = new List<IConventionEntityType>();
		foreach (RelationshipCandidate relationshipCandidate in relationshipCandidates)
		{
			IConventionEntityType entityType = entityTypeBuilder.Metadata;
			IConventionEntityType targetEntityType = relationshipCandidate.TargetTypeBuilder.Metadata;
			bool flag = (entityType.BaseType != null && HasAmbiguousNavigationsTo(entityType.BaseType, targetEntityType.ClrType)) || (targetEntityType.BaseType != null && HasAmbiguousNavigationsTo(targetEntityType.BaseType, entityType.ClrType));
			bool flag2 = relationshipCandidate.NavigationProperties.Count == 1 && relationshipCandidate.InverseProperties.Count == 1 && entityType.GetConfigurationSource() != 0 && targetEntityType.GetConfigurationSource() != 0 && targetEntityType.Model.IsOwned(entityType.ClrType) && targetEntityType.Model.IsOwned(targetEntityType.ClrType);
			if (flag2)
			{
				IConventionNavigation conventionNavigation = entityType.FindNavigation(relationshipCandidate.NavigationProperties.Single().GetSimpleMemberName());
				if (conventionNavigation != null && conventionNavigation.ForeignKey.DeclaringEntityType == targetEntityType && conventionNavigation.ForeignKey.GetPrincipalEndConfigurationSource().OverridesStrictly(ConfigurationSource.Convention))
				{
					flag2 = false;
				}
				else
				{
					IConventionNavigation conventionNavigation2 = targetEntityType.FindNavigation(relationshipCandidate.InverseProperties.Single().GetSimpleMemberName());
					if (conventionNavigation2 != null && conventionNavigation2.ForeignKey.PrincipalEntityType == targetEntityType && conventionNavigation2.ForeignKey.GetPrincipalEndConfigurationSource().OverridesStrictly(ConfigurationSource.Convention))
					{
						flag2 = false;
					}
				}
			}
			if ((relationshipCandidate.NavigationProperties.Count > 1 && relationshipCandidate.InverseProperties.Count > 0 && (!targetEntityType.Model.IsOwned(targetEntityType.ClrType) || entityType.IsInOwnershipPath(targetEntityType))) || relationshipCandidate.InverseProperties.Count > 1 || flag || flag2 || HasDeclaredAmbiguousNavigationsTo(entityType, targetEntityType.ClrType) || HasDeclaredAmbiguousNavigationsTo(targetEntityType, entityType.ClrType))
			{
				if (!flag)
				{
					IDiagnosticsLogger<DbLoggerCategory.Model> logger = Dependencies.Logger;
					IEnumerable<Tuple<MemberInfo, Type>> firstPropertyCollection;
					if (relationshipCandidate.NavigationProperties.Count != 0)
					{
						firstPropertyCollection = relationshipCandidate.NavigationProperties.Select((PropertyInfo n) => new Tuple<MemberInfo, Type>(n, entityType.ClrType));
					}
					else
					{
						IEnumerable<Tuple<MemberInfo, Type>> enumerable = new Tuple<MemberInfo, Type>[1]
						{
							new Tuple<MemberInfo, Type>(null, targetEntityType.ClrType)
						};
						firstPropertyCollection = enumerable;
					}
					IEnumerable<Tuple<MemberInfo, Type>> secondPropertyCollection;
					if (relationshipCandidate.InverseProperties.Count != 0)
					{
						secondPropertyCollection = relationshipCandidate.InverseProperties.Select((PropertyInfo n) => new Tuple<MemberInfo, Type>(n, targetEntityType.ClrType));
					}
					else
					{
						IEnumerable<Tuple<MemberInfo, Type>> enumerable = new Tuple<MemberInfo, Type>[1]
						{
							new Tuple<MemberInfo, Type>(null, targetEntityType.ClrType)
						};
						secondPropertyCollection = enumerable;
					}
					logger.MultipleNavigationProperties(firstPropertyCollection, secondPropertyCollection);
				}
				foreach (PropertyInfo item in relationshipCandidate.NavigationProperties.ToList())
				{
					IConventionNavigation conventionNavigation3 = entityType.FindDeclaredNavigation(item.GetSimpleMemberName());
					if (conventionNavigation3 != null && conventionNavigation3.ForeignKey.DeclaringEntityType.Builder.HasNoRelationship(conventionNavigation3.ForeignKey) == null && conventionNavigation3.ForeignKey.Builder.HasNavigation((string)null, conventionNavigation3.IsDependentToPrincipal(), fromDataAnnotation: false) == null)
					{
						relationshipCandidate.NavigationProperties.Remove(item);
					}
				}
				foreach (PropertyInfo item2 in relationshipCandidate.InverseProperties.ToList())
				{
					IConventionNavigation conventionNavigation4 = targetEntityType.FindDeclaredNavigation(item2.GetSimpleMemberName());
					if (conventionNavigation4 != null && conventionNavigation4.ForeignKey.DeclaringEntityType.Builder.HasNoRelationship(conventionNavigation4.ForeignKey) == null && conventionNavigation4.ForeignKey.Builder.HasNavigation((string)null, conventionNavigation4.IsDependentToPrincipal(), fromDataAnnotation: false) == null)
					{
						relationshipCandidate.InverseProperties.Remove(item2);
					}
				}
				if (!flag)
				{
					AddAmbiguous(entityTypeBuilder, relationshipCandidate.NavigationProperties, targetEntityType.ClrType);
					AddAmbiguous(targetEntityType.Builder, relationshipCandidate.InverseProperties, entityType.ClrType);
				}
				list.Add(targetEntityType);
				continue;
			}
			foreach (PropertyInfo navigationProperty in relationshipCandidate.NavigationProperties)
			{
				if (targetEntityType.Builder == null && !targetEntityType.Model.IsOwned(targetEntityType.ClrType))
				{
					continue;
				}
				if (InversePropertyAttributeConvention.IsAmbiguous(entityType, navigationProperty, targetEntityType))
				{
					list.Add(targetEntityType);
					continue;
				}
				bool flag3 = targetEntityType.Model.IsOwned(targetEntityType.ClrType) && !entityType.IsInOwnershipPath(targetEntityType);
				PropertyInfo propertyInfo = relationshipCandidate.InverseProperties.SingleOrDefault();
				if (propertyInfo == null)
				{
					if (flag3)
					{
						entityTypeBuilder.HasOwnership(targetEntityType.ClrType, navigationProperty);
					}
					else
					{
						entityTypeBuilder.HasRelationship(targetEntityType, navigationProperty);
					}
					continue;
				}
				if (InversePropertyAttributeConvention.IsAmbiguous(targetEntityType, propertyInfo, entityType))
				{
					list.Add(targetEntityType);
					continue;
				}
				if (flag3 && entityType.Model.IsOwned(entityType.ClrType))
				{
					IConventionNavigation conventionNavigation5 = targetEntityType.FindNavigation(propertyInfo.GetSimpleMemberName());
					if (propertyInfo.PropertyType.TryGetSequenceType() != null || targetEntityType.GetConfigurationSource() == ConfigurationSource.Explicit || (conventionNavigation5 != null && conventionNavigation5.ForeignKey.DeclaringEntityType == entityType && conventionNavigation5.ForeignKey.GetPrincipalEndConfigurationSource().OverridesStrictly(ConfigurationSource.Convention)))
					{
						flag3 = false;
					}
				}
				if (flag3)
				{
					entityTypeBuilder.HasOwnership(targetEntityType.ClrType, navigationProperty, propertyInfo);
				}
				else
				{
					entityTypeBuilder.HasRelationship(targetEntityType, navigationProperty, propertyInfo);
				}
			}
			if (relationshipCandidate.NavigationProperties.Count != 0)
			{
				continue;
			}
			if (relationshipCandidate.InverseProperties.Count == 0 || targetEntityType.Model.IsOwned(targetEntityType.ClrType))
			{
				list.Add(targetEntityType);
				continue;
			}
			foreach (PropertyInfo inverseProperty in relationshipCandidate.InverseProperties)
			{
				if (targetEntityType.Builder != null)
				{
					if (InversePropertyAttributeConvention.IsAmbiguous(targetEntityType, inverseProperty, entityType))
					{
						list.Add(targetEntityType);
					}
					else
					{
						targetEntityType.Builder.HasRelationship(entityTypeBuilder.Metadata, inverseProperty);
					}
				}
			}
		}
		foreach (IConventionEntityType item3 in list)
		{
			if (IsCandidateUnusedOwnedType(item3) && item3.DefiningEntityType.FindNavigation(item3.DefiningNavigationName) == null)
			{
				entityTypeBuilder.ModelBuilder.HasNoEntityType(item3);
			}
		}
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		DiscoverRelationships(entityTypeBuilder, context);
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		IConventionEntityTypeBuilder conventionEntityTypeBuilder = oldBaseType?.Builder;
		if (conventionEntityTypeBuilder != null)
		{
			DiscoverRelationships(conventionEntityTypeBuilder, context);
		}
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (metadata.BaseType != newBaseType)
		{
			return;
		}
		if (newBaseType != null)
		{
			foreach (string item in newBaseType.GetAllBaseTypesInclusive().SelectMany((IConventionEntityType et) => et.GetIgnoredMembers()))
			{
				ProcessEntityTypeMemberIgnoredOnBase(metadata, item);
			}
		}
		ApplyOnRelatedEntityTypes(metadata, context);
		foreach (IConventionEntityType item2 in metadata.GetDerivedTypesInclusive())
		{
			DiscoverRelationships(item2.Builder, context);
		}
	}

	private void ApplyOnRelatedEntityTypes(IConventionEntityType entityType, IConventionContext context)
	{
		foreach (IConventionEntityType item in (from fk in entityType.GetReferencingForeignKeys()
			select fk.DeclaringEntityType).Concat(from fk in entityType.GetForeignKeys()
			select fk.PrincipalEntityType).Distinct().ToList())
		{
			IConventionEntityTypeBuilder builder = item.Builder;
			DiscoverRelationships(builder, context);
		}
	}

	public virtual void ProcessNavigationRemoved(IConventionEntityTypeBuilder sourceEntityTypeBuilder, IConventionEntityTypeBuilder targetEntityTypeBuilder, string navigationName, MemberInfo memberInfo, IConventionContext<string> context)
	{
		if ((targetEntityTypeBuilder.Metadata.Builder != null || !sourceEntityTypeBuilder.ModelBuilder.IsIgnored(targetEntityTypeBuilder.Metadata.Name)) && IsCandidateNavigationProperty(sourceEntityTypeBuilder, navigationName, memberInfo))
		{
			Process(sourceEntityTypeBuilder.Metadata, navigationName, memberInfo, context);
		}
	}

	private void Process(IConventionEntityType entityType, string navigationName, MemberInfo memberInfo, IConventionContext context)
	{
		DiscoverRelationships(entityType.Builder, context);
		if (entityType.FindNavigation(navigationName) != null || IsAmbiguous(entityType, memberInfo))
		{
			return;
		}
		foreach (IConventionEntityType directlyDerivedType in entityType.GetDirectlyDerivedTypes())
		{
			Process(directlyDerivedType, navigationName, memberInfo, context);
		}
	}

	[JetBrains.Annotations.ContractAnnotation("memberInfo:null => false")]
	private static bool IsCandidateNavigationProperty(IConventionEntityTypeBuilder sourceEntityTypeBuilder, string navigationName, MemberInfo memberInfo)
	{
		if (memberInfo != null && sourceEntityTypeBuilder != null && !sourceEntityTypeBuilder.IsIgnored(navigationName) && sourceEntityTypeBuilder.Metadata.FindProperty(navigationName) == null && sourceEntityTypeBuilder.Metadata.FindServiceProperty(navigationName) == null && (!(memberInfo is PropertyInfo propertyInfo) || propertyInfo.GetIndexParameters().Length == 0))
		{
			if (sourceEntityTypeBuilder.Metadata.IsKeyless)
			{
				return (memberInfo as PropertyInfo)?.PropertyType.TryGetSequenceType() == null;
			}
			return true;
		}
		return false;
	}

	public virtual void ProcessEntityTypeIgnored(IConventionModelBuilder modelBuilder, string name, Type type, IConventionContext<string> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			if (RemoveAmbiguous(entityType, type))
			{
				DiscoverRelationships(entityType.Builder, context);
			}
		}
	}

	public virtual void ProcessEntityTypeMemberIgnored(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionContext<string> context)
	{
		bool flag = false;
		foreach (IConventionEntityType item in entityTypeBuilder.Metadata.GetDerivedTypesInclusive())
		{
			flag |= ProcessEntityTypeMemberIgnoredOnBase(item, name);
		}
		if (flag)
		{
			DiscoverRelationships(entityTypeBuilder, context);
		}
	}

	private bool ProcessEntityTypeMemberIgnoredOnBase(IConventionEntityType entityType, string name)
	{
		ImmutableSortedDictionary<MemberInfo, Type> ambiguousNavigations = GetAmbiguousNavigations(entityType);
		if (ambiguousNavigations == null)
		{
			return false;
		}
		KeyValuePair<MemberInfo, Type>? keyValuePair = null;
		foreach (KeyValuePair<MemberInfo, Type> item in ambiguousNavigations)
		{
			if (item.Key.GetSimpleMemberName() == name)
			{
				keyValuePair = item;
			}
		}
		if (!keyValuePair.HasValue)
		{
			return false;
		}
		Type value = keyValuePair.Value.Value;
		RemoveAmbiguous(entityType, value);
		EntityType entityType2 = ((InternalEntityTypeBuilder)entityType.Builder).GetTargetEntityTypeBuilder(value, keyValuePair.Value.Key, null)?.Metadata;
		if (entityType2 != null)
		{
			RemoveAmbiguous(entityType2, entityType.ClrType);
		}
		return true;
	}

	public virtual void ProcessNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation, IConventionContext<IConventionNavigation> context)
	{
		foreach (IConventionEntityType item in navigation.DeclaringEntityType.GetDerivedTypesInclusive())
		{
			bool num = RemoveAmbiguous(item, navigation.GetTargetType().ClrType);
			bool flag = RemoveAmbiguous(navigation.GetTargetType(), item.ClrType);
			if (num)
			{
				DiscoverRelationships(item.Builder, context);
			}
			if (flag)
			{
				DiscoverRelationships(navigation.GetTargetType().Builder, context);
			}
		}
		if (relationshipBuilder.Metadata.Builder == null)
		{
			context.StopProcessing();
		}
	}

	public virtual void ProcessForeignKeyOwnershipChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		DiscoverRelationships(relationshipBuilder.Metadata.DeclaringEntityType.Builder, context);
	}

	private Type FindCandidateNavigationPropertyType([JetBrains.Annotations.NotNull] PropertyInfo propertyInfo)
	{
		return Dependencies.MemberClassifier.FindCandidateNavigationPropertyType(propertyInfo);
	}

	private ImmutableSortedDictionary<PropertyInfo, Type> GetNavigationCandidates(IConventionEntityType entityType)
	{
		if (entityType.FindAnnotation("RelationshipDiscoveryConvention:NavigationCandidates")?.Value is ImmutableSortedDictionary<PropertyInfo, Type> result)
		{
			return result;
		}
		ImmutableSortedDictionary<PropertyInfo, Type>.Builder builder = ImmutableSortedDictionary.CreateBuilder<PropertyInfo, Type>(MemberInfoNameComparer.Instance);
		if (entityType.HasClrType())
		{
			foreach (PropertyInfo item in entityType.GetRuntimeProperties().Values.OrderBy((PropertyInfo p) => p.Name))
			{
				Type type = FindCandidateNavigationPropertyType(item);
				if (type != null)
				{
					builder[item] = type;
				}
			}
		}
		ImmutableSortedDictionary<PropertyInfo, Type> immutableSortedDictionary = builder.ToImmutable();
		SetNavigationCandidates(entityType.Builder, immutableSortedDictionary);
		return immutableSortedDictionary;
	}

	private static void SetNavigationCandidates(IConventionEntityTypeBuilder entityTypeBuilder, ImmutableSortedDictionary<PropertyInfo, Type> navigationCandidates)
	{
		entityTypeBuilder.HasAnnotation("RelationshipDiscoveryConvention:NavigationCandidates", navigationCandidates);
	}

	private static bool IsCandidateUnusedOwnedType(IConventionEntityType entityType)
	{
		if (entityType.HasDefiningNavigation())
		{
			return !entityType.GetForeignKeys().Any();
		}
		return false;
	}

	private static bool IsAmbiguous(IConventionEntityType entityType, MemberInfo navigationProperty)
	{
		while (entityType != null)
		{
			ImmutableSortedDictionary<MemberInfo, Type> ambiguousNavigations = GetAmbiguousNavigations(entityType);
			if (ambiguousNavigations != null && ambiguousNavigations.ContainsKey(navigationProperty))
			{
				return true;
			}
			entityType = entityType.BaseType;
		}
		return false;
	}

	private static bool HasAmbiguousNavigationsTo(IConventionEntityType sourceEntityType, Type targetClrType)
	{
		while (sourceEntityType != null)
		{
			if (HasDeclaredAmbiguousNavigationsTo(sourceEntityType, targetClrType))
			{
				return true;
			}
			sourceEntityType = sourceEntityType.BaseType;
		}
		return false;
	}

	private static bool HasDeclaredAmbiguousNavigationsTo(IConventionEntityType sourceEntityType, Type targetClrType)
	{
		return GetAmbiguousNavigations(sourceEntityType)?.ContainsValue(targetClrType) ?? false;
	}

	private static ImmutableSortedDictionary<MemberInfo, Type> GetAmbiguousNavigations(IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("RelationshipDiscoveryConvention:AmbiguousNavigations")?.Value as ImmutableSortedDictionary<MemberInfo, Type>;
	}

	private static void AddAmbiguous(IConventionEntityTypeBuilder entityTypeBuilder, IReadOnlyList<PropertyInfo> navigationProperties, Type targetType)
	{
		if (navigationProperties.Count != 0)
		{
			ImmutableSortedDictionary<MemberInfo, Type> currentAmbiguousNavigations = GetAmbiguousNavigations(entityTypeBuilder.Metadata);
			ImmutableSortedDictionary<MemberInfo, Type> immutableSortedDictionary = ImmutableSortedDictionary.CreateRange(MemberInfoNameComparer.Instance, from n in navigationProperties.Where(delegate(PropertyInfo n)
				{
					ImmutableSortedDictionary<MemberInfo, Type> immutableSortedDictionary2 = currentAmbiguousNavigations;
					return immutableSortedDictionary2 == null || !immutableSortedDictionary2.ContainsKey(n);
				})
				select new KeyValuePair<MemberInfo, Type>(n, targetType));
			if (currentAmbiguousNavigations != null)
			{
				immutableSortedDictionary = ((immutableSortedDictionary.Count > 0) ? currentAmbiguousNavigations.AddRange(immutableSortedDictionary) : currentAmbiguousNavigations);
			}
			SetAmbiguousNavigations(entityTypeBuilder, immutableSortedDictionary);
		}
	}

	private static bool RemoveAmbiguous(IConventionEntityType entityType, Type targetType)
	{
		ImmutableSortedDictionary<MemberInfo, Type> ambiguousNavigations = GetAmbiguousNavigations(entityType);
		if (ambiguousNavigations != null && !ambiguousNavigations.IsEmpty)
		{
			ImmutableSortedDictionary<MemberInfo, Type> immutableSortedDictionary = ambiguousNavigations;
			foreach (KeyValuePair<MemberInfo, Type> item in ambiguousNavigations)
			{
				if (targetType.IsAssignableFrom(item.Value))
				{
					immutableSortedDictionary = immutableSortedDictionary.Remove(item.Key);
				}
			}
			if (ambiguousNavigations.Count != immutableSortedDictionary.Count)
			{
				SetAmbiguousNavigations(entityType.Builder, immutableSortedDictionary);
				return true;
			}
		}
		return false;
	}

	private static void SetAmbiguousNavigations(IConventionEntityTypeBuilder entityTypeBuilder, ImmutableSortedDictionary<MemberInfo, Type> ambiguousNavigations)
	{
		entityTypeBuilder.HasAnnotation("RelationshipDiscoveryConvention:AmbiguousNavigations", ambiguousNavigations);
	}
}
