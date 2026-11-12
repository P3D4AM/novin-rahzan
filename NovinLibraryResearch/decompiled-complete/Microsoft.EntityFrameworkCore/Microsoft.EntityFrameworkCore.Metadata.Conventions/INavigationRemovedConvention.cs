using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface INavigationRemovedConvention : IConvention
{
	void ProcessNavigationRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder sourceEntityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder targetEntityTypeBuilder, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.CanBeNull] MemberInfo memberInfo, [JetBrains.Annotations.NotNull] IConventionContext<string> context);
}
