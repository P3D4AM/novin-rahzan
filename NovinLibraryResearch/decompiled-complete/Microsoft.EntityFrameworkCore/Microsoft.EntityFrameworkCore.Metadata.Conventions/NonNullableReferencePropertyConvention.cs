using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class NonNullableReferencePropertyConvention : NonNullableConventionBase, IPropertyAddedConvention, IConvention, IPropertyFieldChangedConvention
{
	public NonNullableReferencePropertyConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	private void Process(IConventionPropertyBuilder propertyBuilder)
	{
		MemberInfo identifyingMemberInfo = propertyBuilder.Metadata.GetIdentifyingMemberInfo();
		if ((object)identifyingMemberInfo != null && IsNonNullableReferenceType(propertyBuilder.ModelBuilder, identifyingMemberInfo))
		{
			propertyBuilder.IsRequired(true);
		}
	}

	public virtual void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
	{
		Process(propertyBuilder);
	}

	public virtual void ProcessPropertyFieldChanged(IConventionPropertyBuilder propertyBuilder, FieldInfo newFieldInfo, FieldInfo oldFieldInfo, IConventionContext<FieldInfo> context)
	{
		Process(propertyBuilder);
	}
}
