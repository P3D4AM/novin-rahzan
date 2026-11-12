using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ServicePropertyDiscoveryConvention : IEntityTypeAddedConvention, IConvention, IEntityTypeBaseTypeChangedConvention, IEntityTypeMemberIgnoredConvention, IModelFinalizedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ServicePropertyDiscoveryConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		Process(entityTypeBuilder);
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if (entityTypeBuilder.Metadata.BaseType == newBaseType)
		{
			Process(entityTypeBuilder);
		}
	}

	private void Process(IConventionEntityTypeBuilder entityTypeBuilder)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (!metadata.HasClrType())
		{
			return;
		}
		foreach (PropertyInfo propertyInfo in metadata.GetRuntimeProperties().Values)
		{
			string simpleMemberName = propertyInfo.GetSimpleMemberName();
			if (entityTypeBuilder.IsIgnored(simpleMemberName) || metadata.FindProperty(propertyInfo) != null || metadata.FindNavigation(propertyInfo) != null || !propertyInfo.IsCandidateProperty(needsWrite: true, publicOnly: false) || (propertyInfo.IsCandidateProperty() && Dependencies.TypeMappingSource.FindMapping(propertyInfo) != null))
			{
				continue;
			}
			IParameterBindingFactory parameterBindingFactory = Dependencies.ParameterBindingFactories.FindFactory(propertyInfo.PropertyType, simpleMemberName);
			if (parameterBindingFactory == null)
			{
				continue;
			}
			Dictionary<Type, HashSet<MemberInfo>> duplicateServiceProperties = GetDuplicateServiceProperties(metadata);
			if (duplicateServiceProperties != null && duplicateServiceProperties.TryGetValue(propertyInfo.PropertyType, out var value))
			{
				value.Add(propertyInfo);
				break;
			}
			IConventionServiceProperty conventionServiceProperty = metadata.GetServiceProperties().FirstOrDefault((IConventionServiceProperty p) => p.ClrType == propertyInfo.PropertyType);
			if (conventionServiceProperty != null)
			{
				if (ConfigurationSource.Convention.Overrides(conventionServiceProperty.GetConfigurationSource()))
				{
					conventionServiceProperty.DeclaringEntityType.RemoveServiceProperty(conventionServiceProperty.Name);
				}
				AddDuplicateServiceProperty(entityTypeBuilder, propertyInfo);
				AddDuplicateServiceProperty(entityTypeBuilder, conventionServiceProperty.GetIdentifyingMemberInfo());
				break;
			}
			entityTypeBuilder.ServiceProperty(propertyInfo)?.HasParameterBinding((ServiceParameterBinding)parameterBindingFactory.Bind(metadata, propertyInfo.PropertyType, propertyInfo.GetSimpleMemberName()));
		}
	}

	public virtual void ProcessEntityTypeMemberIgnored(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionContext<string> context)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		Dictionary<Type, HashSet<MemberInfo>> duplicateServiceProperties = GetDuplicateServiceProperties(metadata);
		if (duplicateServiceProperties == null)
		{
			return;
		}
		MemberInfo memberInfo = (MemberInfo)(((object)metadata.GetRuntimeProperties().Find(name)) ?? ((object)metadata.GetRuntimeFields().Find(name)));
		Type memberType = memberInfo.GetMemberType();
		if (duplicateServiceProperties.TryGetValue(memberType, out var value) && value.Remove(memberInfo) && value.Count == 1)
		{
			MemberInfo memberInfo2 = value.First();
			string simpleMemberName = memberInfo2.GetSimpleMemberName();
			IParameterBindingFactory parameterBindingFactory = Dependencies.ParameterBindingFactories.FindFactory(memberType, simpleMemberName);
			metadata.Builder.ServiceProperty(memberInfo2)?.HasParameterBinding((ServiceParameterBinding)parameterBindingFactory.Bind(metadata, memberType, simpleMemberName));
			duplicateServiceProperties.Remove(memberType);
			if (duplicateServiceProperties.Count == 0)
			{
				SetDuplicateServiceProperties(metadata.Builder, null);
			}
		}
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			Dictionary<Type, HashSet<MemberInfo>> duplicateServiceProperties = GetDuplicateServiceProperties(entityType);
			if (duplicateServiceProperties == null)
			{
				continue;
			}
			foreach (HashSet<MemberInfo> value in duplicateServiceProperties.Values)
			{
				foreach (MemberInfo item in value)
				{
					if (entityType.FindProperty(item.GetSimpleMemberName()) == null && entityType.FindNavigation(item.GetSimpleMemberName()) == null)
					{
						throw new InvalidOperationException(CoreStrings.AmbiguousServiceProperty(item.Name, item.GetMemberType().ShortDisplayName(), entityType.DisplayName()));
					}
				}
			}
			SetDuplicateServiceProperties(entityType.Builder, null);
		}
	}

	private static void AddDuplicateServiceProperty(IConventionEntityTypeBuilder entityTypeBuilder, MemberInfo serviceProperty)
	{
		Dictionary<Type, HashSet<MemberInfo>> dictionary = GetDuplicateServiceProperties(entityTypeBuilder.Metadata) ?? new Dictionary<Type, HashSet<MemberInfo>>(1);
		Type memberType = serviceProperty.GetMemberType();
		if (!dictionary.TryGetValue(memberType, out var value))
		{
			value = (dictionary[memberType] = new HashSet<MemberInfo>());
		}
		value.Add(serviceProperty);
		SetDuplicateServiceProperties(entityTypeBuilder, dictionary);
	}

	private static Dictionary<Type, HashSet<MemberInfo>> GetDuplicateServiceProperties(IConventionEntityType entityType)
	{
		return entityType.FindAnnotation("ServicePropertyDiscoveryConvention:DuplicateServiceProperties")?.Value as Dictionary<Type, HashSet<MemberInfo>>;
	}

	private static void SetDuplicateServiceProperties(IConventionEntityTypeBuilder entityTypeBuilder, Dictionary<Type, HashSet<MemberInfo>> duplicateServiceProperties)
	{
		entityTypeBuilder.HasAnnotation("ServicePropertyDiscoveryConvention:DuplicateServiceProperties", duplicateServiceProperties);
	}
}
