using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public abstract class NonNullableConventionBase : IModelFinalizedConvention, IConvention
{
	private class NonNullabilityConventionState
	{
		public Type NullableAttrType;

		public Type NullableContextAttrType;

		public FieldInfo NullableFlagsFieldInfo;

		public FieldInfo NullableContextFlagFieldInfo;

		public Dictionary<Type, bool> TypeCache { get; } = new Dictionary<Type, bool>();

	}

	private const string StateAnnotationName = "NonNullableConventionState";

	private const string NullableAttributeFullName = "System.Runtime.CompilerServices.NullableAttribute";

	private const string NullableContextAttributeFullName = "System.Runtime.CompilerServices.NullableContextAttribute";

	private const string MaybeNullAttributeFullName = "System.Diagnostics.CodeAnalysis.MaybeNullAttribute";

	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	protected NonNullableConventionBase([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	protected virtual bool IsNonNullableReferenceType([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		if (memberInfo.GetMemberType().IsValueType)
		{
			return false;
		}
		NonNullabilityConventionState orInitializeState = GetOrInitializeState(modelBuilder);
		if ((memberInfo is FieldInfo fieldInfo) ? fieldInfo.CustomAttributes.Any((CustomAttributeData a) => a.AttributeType.FullName == "System.Diagnostics.CodeAnalysis.MaybeNullAttribute") : (memberInfo is PropertyInfo propertyInfo && propertyInfo.GetMethod?.ReturnParameter?.CustomAttributes.FirstOrDefault((CustomAttributeData a) => a.AttributeType.FullName == "System.Diagnostics.CodeAnalysis.MaybeNullAttribute") != null))
		{
			return false;
		}
		Attribute attribute = Attribute.GetCustomAttributes(memberInfo).FirstOrDefault((Attribute a) => a.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute");
		if (attribute != null)
		{
			Type type = attribute.GetType();
			if (type != orInitializeState.NullableAttrType)
			{
				orInitializeState.NullableFlagsFieldInfo = type.GetField("NullableFlags");
				orInitializeState.NullableAttrType = type;
			}
			if (orInitializeState.NullableFlagsFieldInfo?.GetValue(attribute) is byte[] source)
			{
				return source.FirstOrDefault() == 1;
			}
		}
		Type declaringType = memberInfo.DeclaringType;
		if (declaringType != null)
		{
			if (orInitializeState.TypeCache.TryGetValue(declaringType, out var value))
			{
				return value;
			}
			Attribute attribute2 = Attribute.GetCustomAttributes(declaringType).FirstOrDefault((Attribute a) => a.GetType().FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
			if (attribute2 != null)
			{
				Type type2 = attribute2.GetType();
				if (type2 != orInitializeState.NullableContextAttrType)
				{
					orInitializeState.NullableContextFlagFieldInfo = type2.GetField("Flag");
					orInitializeState.NullableContextAttrType = type2;
				}
				if (orInitializeState.NullableContextFlagFieldInfo?.GetValue(attribute2) is byte b)
				{
					return orInitializeState.TypeCache[declaringType] = b == 1;
				}
			}
			return orInitializeState.TypeCache[declaringType] = false;
		}
		return false;
	}

	private NonNullabilityConventionState GetOrInitializeState(IConventionModelBuilder modelBuilder)
	{
		return (NonNullabilityConventionState)(modelBuilder.Metadata.FindAnnotation("NonNullableConventionState") ?? modelBuilder.Metadata.AddAnnotation("NonNullableConventionState", new NonNullabilityConventionState())).Value;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		modelBuilder.Metadata.RemoveAnnotation("NonNullableConventionState");
	}
}
