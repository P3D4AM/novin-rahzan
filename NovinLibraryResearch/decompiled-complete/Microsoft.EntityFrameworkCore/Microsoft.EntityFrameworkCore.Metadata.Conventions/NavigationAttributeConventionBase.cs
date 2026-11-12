using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public abstract class NavigationAttributeConventionBase<TAttribute> : IEntityTypeAddedConvention, IConvention, IEntityTypeIgnoredConvention, IEntityTypeBaseTypeChangedConvention, INavigationAddedConvention, IEntityTypeMemberIgnoredConvention where TAttribute : Attribute
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	protected NavigationAttributeConventionBase([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (!metadata.HasClrType())
		{
			return;
		}
		foreach (PropertyInfo item in metadata.GetRuntimeProperties().Values.OrderBy((PropertyInfo p) => p.Name))
		{
			Type type = FindCandidateNavigationWithAttributePropertyType(item);
			if (type == null)
			{
				continue;
			}
			foreach (TAttribute customAttribute in item.GetCustomAttributes<TAttribute>(inherit: true))
			{
				ProcessEntityTypeAdded(entityTypeBuilder, item, type, customAttribute, context);
				if (((ConventionContext<IConventionEntityTypeBuilder>)context).ShouldStopProcessing())
				{
					return;
				}
			}
		}
	}

	public virtual void ProcessEntityTypeIgnored(IConventionModelBuilder modelBuilder, string name, Type type, IConventionContext<string> context)
	{
		if (type == null)
		{
			return;
		}
		foreach (PropertyInfo item in from p in type.GetRuntimeProperties()
			orderby p.Name
			select p)
		{
			Type type2 = FindCandidateNavigationWithAttributePropertyType(item);
			if (type2 == null)
			{
				continue;
			}
			type.IsDefined(typeof(TAttribute));
			item.IsDefined(typeof(TAttribute));
			foreach (TAttribute customAttribute in item.GetCustomAttributes<TAttribute>(inherit: true))
			{
				ProcessEntityTypeIgnored(modelBuilder, type, item, type2, customAttribute, context);
				if (((ConventionContext<string>)context).ShouldStopProcessing())
				{
					return;
				}
			}
		}
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (!metadata.HasClrType() || entityTypeBuilder.Metadata.BaseType != newBaseType)
		{
			return;
		}
		foreach (PropertyInfo item in metadata.GetRuntimeProperties().Values.OrderBy((PropertyInfo p) => p.Name))
		{
			Type type = FindCandidateNavigationWithAttributePropertyType(item);
			if (type == null)
			{
				continue;
			}
			foreach (TAttribute customAttribute in item.GetCustomAttributes<TAttribute>(inherit: true))
			{
				ProcessEntityTypeBaseTypeChanged(entityTypeBuilder, newBaseType, oldBaseType, item, type, customAttribute, context);
				if (((ConventionContext<IConventionEntityType>)context).ShouldStopProcessing())
				{
					return;
				}
			}
		}
	}

	public virtual void ProcessNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation, IConventionContext<IConventionNavigation> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationshipBuilder, "relationshipBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		foreach (TAttribute attribute in GetAttributes<TAttribute>(navigation.DeclaringEntityType, navigation))
		{
			ProcessNavigationAdded(relationshipBuilder, navigation, attribute, context);
			if (((IReadableConventionContext)context).ShouldStopProcessing())
			{
				break;
			}
		}
	}

	public virtual void ProcessEntityTypeMemberIgnored(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionContext<string> context)
	{
		PropertyInfo propertyInfo = entityTypeBuilder.Metadata.GetRuntimeProperties()?.Find(name);
		if (propertyInfo == null)
		{
			return;
		}
		Type type = FindCandidateNavigationWithAttributePropertyType(propertyInfo);
		if (type == null)
		{
			return;
		}
		foreach (TAttribute customAttribute in propertyInfo.GetCustomAttributes<TAttribute>(inherit: true))
		{
			ProcessEntityTypeMemberIgnored(entityTypeBuilder, propertyInfo, type, customAttribute, context);
			if (((ConventionContext<string>)context).ShouldStopProcessing())
			{
				break;
			}
		}
	}

	private Type FindCandidateNavigationWithAttributePropertyType([JetBrains.Annotations.NotNull] PropertyInfo propertyInfo)
	{
		Type type = Dependencies.MemberClassifier.FindCandidateNavigationPropertyType(propertyInfo);
		if (!(type == null) && Attribute.IsDefined(propertyInfo, typeof(TAttribute), inherit: true))
		{
			return type;
		}
		return null;
	}

	protected static IEnumerable<TCustomAttribute> GetAttributes<TCustomAttribute>([JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IConventionNavigation navigation) where TCustomAttribute : Attribute
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		MemberInfo identifyingMemberInfo = navigation.GetIdentifyingMemberInfo();
		if (!entityType.HasClrType() || identifyingMemberInfo == null)
		{
			return Enumerable.Empty<TCustomAttribute>();
		}
		if (!Attribute.IsDefined(identifyingMemberInfo, typeof(TCustomAttribute), inherit: true))
		{
			return Enumerable.Empty<TCustomAttribute>();
		}
		return identifyingMemberInfo.GetCustomAttributes<TCustomAttribute>(inherit: true);
	}

	public virtual void ProcessEntityTypeAdded([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] Type targetClrType, [JetBrains.Annotations.NotNull] TAttribute attribute, [JetBrains.Annotations.NotNull] IConventionContext<IConventionEntityTypeBuilder> context)
	{
		throw new NotImplementedException();
	}

	public virtual void ProcessEntityTypeIgnored([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] Type targetClrType, [JetBrains.Annotations.NotNull] TAttribute attribute, [JetBrains.Annotations.NotNull] IConventionContext<string> context)
	{
		throw new NotImplementedException();
	}

	public virtual void ProcessEntityTypeBaseTypeChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] IConventionEntityType newBaseType, [JetBrains.Annotations.CanBeNull] IConventionEntityType oldBaseType, [JetBrains.Annotations.NotNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] Type targetClrType, [JetBrains.Annotations.NotNull] TAttribute attribute, [JetBrains.Annotations.NotNull] IConventionContext<IConventionEntityType> context)
	{
		throw new NotImplementedException();
	}

	public virtual void ProcessNavigationAdded([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IConventionNavigation navigation, [JetBrains.Annotations.NotNull] TAttribute attribute, [JetBrains.Annotations.NotNull] IConventionContext<IConventionNavigation> context)
	{
		throw new NotImplementedException();
	}

	public virtual void ProcessEntityTypeMemberIgnored([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] Type targetClrType, [JetBrains.Annotations.NotNull] TAttribute attribute, [JetBrains.Annotations.NotNull] IConventionContext<string> context)
	{
		throw new NotImplementedException();
	}
}
