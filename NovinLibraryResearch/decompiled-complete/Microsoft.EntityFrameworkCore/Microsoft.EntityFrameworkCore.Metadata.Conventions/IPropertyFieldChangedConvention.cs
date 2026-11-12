using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IPropertyFieldChangedConvention : IConvention
{
	void ProcessPropertyFieldChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] FieldInfo newFieldInfo, [JetBrains.Annotations.CanBeNull] FieldInfo oldFieldInfo, [JetBrains.Annotations.NotNull] IConventionContext<FieldInfo> context);
}
