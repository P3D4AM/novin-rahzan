using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public abstract class PropertyAttributeConventionBase<TAttribute> : IPropertyAddedConvention, IConvention, IPropertyFieldChangedConvention where TAttribute : Attribute
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	protected PropertyAttributeConventionBase([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
		MemberInfo identifyingMemberInfo = propertyBuilder.Metadata.GetIdentifyingMemberInfo();
		if (!(identifyingMemberInfo == null))
		{
			Process(propertyBuilder, identifyingMemberInfo, (IReadableConventionContext)context);
		}
	}

	public virtual void ProcessPropertyFieldChanged(IConventionPropertyBuilder propertyBuilder, FieldInfo newFieldInfo, FieldInfo oldFieldInfo, IConventionContext<FieldInfo> context)
	{
		if (newFieldInfo != null && propertyBuilder.Metadata.PropertyInfo == null)
		{
			Process(propertyBuilder, newFieldInfo, (IReadableConventionContext)context);
		}
	}

	private void Process(IConventionPropertyBuilder propertyBuilder, MemberInfo memberInfo, IReadableConventionContext context)
	{
		if (!Attribute.IsDefined(memberInfo, typeof(TAttribute), inherit: true))
		{
			return;
		}
		foreach (TAttribute customAttribute in memberInfo.GetCustomAttributes<TAttribute>(inherit: true))
		{
			ProcessPropertyAdded(propertyBuilder, customAttribute, memberInfo, context);
			if (context.ShouldStopProcessing())
			{
				break;
			}
		}
	}

	protected abstract void ProcessPropertyAdded([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.NotNull] TAttribute attribute, [JetBrains.Annotations.NotNull] MemberInfo clrMember, [JetBrains.Annotations.NotNull] IConventionContext context);
}
