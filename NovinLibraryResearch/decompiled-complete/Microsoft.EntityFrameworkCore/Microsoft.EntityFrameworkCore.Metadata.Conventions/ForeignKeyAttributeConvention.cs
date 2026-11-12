using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ForeignKeyAttributeConvention : IForeignKeyAddedConvention, IConvention, IModelFinalizedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ForeignKeyAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessForeignKeyAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationshipBuilder, "relationshipBuilder");
		IConventionForeignKey foreignKey = relationshipBuilder.Metadata;
		MemberInfo memberInfo = FindForeignKeyAttributeOnProperty(foreignKey.PrincipalEntityType, foreignKey.PrincipalToDependent?.Name);
		MemberInfo memberInfo2 = FindForeignKeyAttributeOnProperty(foreignKey.DeclaringEntityType, foreignKey.DependentToPrincipal?.Name);
		if (memberInfo2 != null && memberInfo != null)
		{
			Dependencies.Logger.ForeignKeyAttributesOnBothPropertiesWarning(foreignKey.PrincipalToDependent, foreignKey.DependentToPrincipal, memberInfo, memberInfo2);
			relationshipBuilder = SplitNavigationsToSeparateRelationships(relationshipBuilder);
			if (relationshipBuilder == null)
			{
				context.StopProcessing();
				return;
			}
			memberInfo = null;
		}
		IReadOnlyList<string> readOnlyList = FindCandidateDependentPropertiesThroughNavigation(relationshipBuilder, pointsToPrincipal: false);
		IReadOnlyList<string> readOnlyList2 = FindCandidateDependentPropertiesThroughNavigation(relationshipBuilder, pointsToPrincipal: true);
		if (readOnlyList2 != null && readOnlyList != null)
		{
			Dependencies.Logger.ForeignKeyAttributesOnBothNavigationsWarning(relationshipBuilder.Metadata.DependentToPrincipal, relationshipBuilder.Metadata.PrincipalToDependent);
			relationshipBuilder = SplitNavigationsToSeparateRelationships(relationshipBuilder);
			if (relationshipBuilder == null)
			{
				context.StopProcessing();
				return;
			}
			readOnlyList = null;
		}
		IReadOnlyList<string> readOnlyList3 = readOnlyList2 ?? readOnlyList;
		bool flag = readOnlyList != null;
		bool flag2 = readOnlyList2 != null;
		bool flag3 = false;
		IReadOnlyList<string> propertyNames;
		if (readOnlyList3 == null || readOnlyList3.Count == 0)
		{
			if (memberInfo2 == null && memberInfo == null)
			{
				return;
			}
			if (memberInfo2 != null)
			{
				propertyNames = new List<string> { memberInfo2.GetSimpleMemberName() };
				flag2 = true;
			}
			else
			{
				if (foreignKey.PrincipalToDependent.IsCollection())
				{
					context.StopProcessing();
					return;
				}
				flag3 = true;
				propertyNames = new List<string> { memberInfo.GetSimpleMemberName() };
				flag = true;
			}
		}
		else
		{
			propertyNames = readOnlyList3;
			if (memberInfo2 == null && memberInfo == null)
			{
				if (readOnlyList != null && foreignKey.IsUnique)
				{
					flag3 = true;
				}
			}
			else
			{
				MemberInfo memberInfo3 = memberInfo2 ?? memberInfo;
				if (readOnlyList3.Count != 1 || !object.Equals(readOnlyList3.First(), memberInfo3.GetSimpleMemberName()))
				{
					Dependencies.Logger.ConflictingForeignKeyAttributesOnNavigationAndPropertyWarning((readOnlyList2 != null) ? relationshipBuilder.Metadata.DependentToPrincipal : relationshipBuilder.Metadata.PrincipalToDependent, memberInfo3);
					relationshipBuilder = SplitNavigationsToSeparateRelationships(relationshipBuilder);
					if (relationshipBuilder == null)
					{
						context.StopProcessing();
						return;
					}
					flag = false;
					propertyNames = readOnlyList2 ?? new List<string> { memberInfo2.GetSimpleMemberName() };
				}
				if (memberInfo2 != null)
				{
					flag2 = true;
				}
				else
				{
					flag3 = true;
				}
			}
		}
		IConventionRelationshipBuilder conventionRelationshipBuilder = relationshipBuilder;
		if (flag2)
		{
			conventionRelationshipBuilder = conventionRelationshipBuilder.HasNavigation(conventionRelationshipBuilder.Metadata.DependentToPrincipal.Name, pointsToPrincipal: true, fromDataAnnotation: true);
		}
		if (flag)
		{
			conventionRelationshipBuilder = conventionRelationshipBuilder.HasNavigation(conventionRelationshipBuilder.Metadata.PrincipalToDependent.Name, pointsToPrincipal: false, fromDataAnnotation: true);
		}
		if (flag3)
		{
			conventionRelationshipBuilder = conventionRelationshipBuilder.HasEntityTypes(foreignKey.DeclaringEntityType, foreignKey.PrincipalEntityType, fromDataAnnotation: true);
		}
		else
		{
			IReadOnlyList<IConventionProperty> readOnlyList4 = foreignKey.DeclaringEntityType.FindProperties(propertyNames);
			if (readOnlyList4 != null && foreignKey.DeclaringEntityType.FindForeignKeys(readOnlyList4).FirstOrDefault((IConventionForeignKey fk) => fk != foreignKey && fk.PrincipalEntityType == foreignKey.PrincipalEntityType && fk.GetConfigurationSource() == ConfigurationSource.DataAnnotation && fk.GetPropertiesConfigurationSource() == ConfigurationSource.DataAnnotation) != null)
			{
				throw new InvalidOperationException(CoreStrings.ConflictingForeignKeyAttributes(readOnlyList4.Format(), foreignKey.DeclaringEntityType.DisplayName()));
			}
		}
		conventionRelationshipBuilder = conventionRelationshipBuilder?.HasForeignKey(propertyNames, fromDataAnnotation: true);
		if (conventionRelationshipBuilder != null)
		{
			context.StopProcessingIfChanged(conventionRelationshipBuilder);
		}
	}

	private static IConventionRelationshipBuilder SplitNavigationsToSeparateRelationships(IConventionRelationshipBuilder relationshipBuilder)
	{
		IConventionForeignKey metadata = relationshipBuilder.Metadata;
		string name = metadata.DependentToPrincipal.Name;
		string name2 = metadata.PrincipalToDependent.Name;
		if (GetInversePropertyAttribute(metadata.PrincipalToDependent) != null || GetInversePropertyAttribute(metadata.DependentToPrincipal) != null)
		{
			throw new InvalidOperationException(CoreStrings.InvalidRelationshipUsingDataAnnotations(name, metadata.DeclaringEntityType.DisplayName(), name2, metadata.PrincipalEntityType.DisplayName()));
		}
		relationshipBuilder = relationshipBuilder.HasNavigation((string)null, pointsToPrincipal: false, fromDataAnnotation: true);
		if (relationshipBuilder != null)
		{
			if (metadata.PrincipalEntityType.Builder.HasRelationship(metadata.DeclaringEntityType, name2, null, setTargetAsPrincipal: false, fromDataAnnotation: true) != null)
			{
				return relationshipBuilder;
			}
			return null;
		}
		return null;
	}

	private static ForeignKeyAttribute GetForeignKeyAttribute(IConventionTypeBase entityType, string propertyName)
	{
		return entityType.GetRuntimeProperties()?.Values.FirstOrDefault((PropertyInfo p) => string.Equals(p.GetSimpleMemberName(), propertyName, StringComparison.OrdinalIgnoreCase) && Attribute.IsDefined(p, typeof(ForeignKeyAttribute), inherit: true))?.GetCustomAttribute<ForeignKeyAttribute>(inherit: true);
	}

	private static ForeignKeyAttribute GetForeignKeyAttribute(IConventionNavigation navigation)
	{
		return GetAttribute<ForeignKeyAttribute>(navigation.GetIdentifyingMemberInfo());
	}

	private static InversePropertyAttribute GetInversePropertyAttribute(IConventionNavigation navigation)
	{
		return GetAttribute<InversePropertyAttribute>(navigation.GetIdentifyingMemberInfo());
	}

	private static TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
	{
		if (memberInfo == null || !Attribute.IsDefined(memberInfo, typeof(TAttribute), inherit: true))
		{
			return null;
		}
		return memberInfo.GetCustomAttribute<TAttribute>(inherit: true);
	}

	[JetBrains.Annotations.ContractAnnotation("navigationName:null => null")]
	private MemberInfo FindForeignKeyAttributeOnProperty(IConventionEntityType entityType, string navigationName)
	{
		if (string.IsNullOrWhiteSpace(navigationName) || !entityType.HasClrType())
		{
			return null;
		}
		MemberInfo memberInfo = null;
		foreach (MemberInfo item in entityType.GetRuntimeProperties().Values.Cast<MemberInfo>().Concat(entityType.GetRuntimeFields().Values))
		{
			if (!entityType.Builder.IsIgnored(item.GetSimpleMemberName()) && Attribute.IsDefined(item, typeof(ForeignKeyAttribute), inherit: true) && !(item.GetCustomAttribute<ForeignKeyAttribute>(inherit: true).Name != navigationName) && (!(item is PropertyInfo propertyInfo) || !(FindCandidateNavigationPropertyType(propertyInfo) != null)))
			{
				if (memberInfo != null)
				{
					throw new InvalidOperationException(CoreStrings.CompositeFkOnProperty(navigationName, entityType.DisplayName()));
				}
				memberInfo = item;
			}
		}
		if (memberInfo != null)
		{
			ForeignKeyAttribute foreignKeyAttribute = GetForeignKeyAttribute(entityType, navigationName);
			if (foreignKeyAttribute != null && foreignKeyAttribute.Name != memberInfo.GetSimpleMemberName())
			{
				throw new InvalidOperationException(CoreStrings.FkAttributeOnPropertyNavigationMismatch(memberInfo.Name, navigationName, entityType.DisplayName()));
			}
		}
		return memberInfo;
	}

	private Type FindCandidateNavigationPropertyType([JetBrains.Annotations.NotNull] PropertyInfo propertyInfo)
	{
		return Dependencies.MemberClassifier.FindCandidateNavigationPropertyType(propertyInfo);
	}

	private static IReadOnlyList<string> FindCandidateDependentPropertiesThroughNavigation(IConventionRelationshipBuilder relationshipBuilder, bool pointsToPrincipal)
	{
		IConventionNavigation navigation = (pointsToPrincipal ? relationshipBuilder.Metadata.DependentToPrincipal : relationshipBuilder.Metadata.PrincipalToDependent);
		ForeignKeyAttribute val = ((navigation != null) ? GetForeignKeyAttribute(navigation) : null);
		if (val != null)
		{
			List<string> list = (from p in val.Name.Split(new char[1] { ',' })
				select p.Trim()).ToList();
			if (list.Any(string.IsNullOrWhiteSpace))
			{
				throw new InvalidOperationException(CoreStrings.InvalidPropertyListOnNavigation(navigation.Name, navigation.DeclaringEntityType.DisplayName()));
			}
			Type navigationPropertyTargetType = navigation.DeclaringEntityType.GetRuntimeProperties()[navigation.Name].PropertyType;
			{
				foreach (PropertyInfo item in from p in navigation.DeclaringEntityType.GetRuntimeProperties().Values
					where p.PropertyType == navigationPropertyTargetType && p.GetSimpleMemberName() != navigation.Name
					orderby p.GetSimpleMemberName()
					select p)
				{
					ForeignKeyAttribute attribute = GetAttribute<ForeignKeyAttribute>((MemberInfo)item);
					if (((attribute != null) ? attribute.Name : null) == val.Name)
					{
						throw new InvalidOperationException(CoreStrings.MultipleNavigationsSameFk(navigation.DeclaringEntityType.DisplayName(), attribute.Name));
					}
				}
				return list;
			}
		}
		return null;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			foreach (IConventionNavigation declaredNavigation in entityType.GetDeclaredNavigations())
			{
				if (declaredNavigation.IsCollection())
				{
					IConventionForeignKey foreignKey = declaredNavigation.ForeignKey;
					if (FindForeignKeyAttributeOnProperty(foreignKey.PrincipalEntityType, declaredNavigation.Name) != null)
					{
						throw new InvalidOperationException(CoreStrings.FkAttributeOnNonUniquePrincipal(declaredNavigation.Name, foreignKey.PrincipalEntityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName()));
					}
				}
			}
		}
	}
}
