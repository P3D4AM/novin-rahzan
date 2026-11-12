using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class KeyAttributeConvention : PropertyAttributeConventionBase<KeyAttribute>, IModelFinalizedConvention, IConvention
{
	public KeyAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, KeyAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		IConventionEntityType declaringEntityType = propertyBuilder.Metadata.DeclaringEntityType;
		if (declaringEntityType.BaseType != null)
		{
			return;
		}
		IConventionEntityTypeBuilder builder = declaringEntityType.Builder;
		IConventionKey conventionKey = builder.Metadata.FindPrimaryKey();
		List<string> list = new List<string> { propertyBuilder.Metadata.Name };
		if (conventionKey != null && declaringEntityType.GetPrimaryKeyConfigurationSource() == ConfigurationSource.DataAnnotation)
		{
			list.AddRange(from p in conventionKey.Properties
				where !p.Name.Equals(propertyBuilder.Metadata.Name, StringComparison.OrdinalIgnoreCase)
				select p.Name);
			if (list.Count > 1)
			{
				list.Sort(StringComparer.OrdinalIgnoreCase);
				builder.HasNoKey(conventionKey, fromDataAnnotation: true);
			}
		}
		builder.PrimaryKey(builder.GetOrCreateProperties(list, fromDataAnnotation: true), fromDataAnnotation: true);
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			if (entityType.BaseType == null)
			{
				IConventionKey conventionKey = entityType.FindPrimaryKey();
				if (conventionKey != null && conventionKey.Properties.Count > 1 && entityType.GetPrimaryKeyConfigurationSource() == ConfigurationSource.DataAnnotation)
				{
					throw new InvalidOperationException(CoreStrings.CompositePKWithDataAnnotation(entityType.DisplayName()));
				}
				continue;
			}
			foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
			{
				MemberInfo identifyingMemberInfo = declaredProperty.GetIdentifyingMemberInfo();
				if (identifyingMemberInfo != null && Attribute.IsDefined(identifyingMemberInfo, typeof(KeyAttribute), inherit: true))
				{
					throw new InvalidOperationException(CoreStrings.KeyAttributeOnDerivedEntity(entityType.DisplayName(), declaredProperty.Name));
				}
			}
		}
	}
}
