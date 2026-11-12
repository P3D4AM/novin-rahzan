using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IEntityTypeMemberIgnoredConvention : IConvention
{
	void ProcessEntityTypeMemberIgnored([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] IConventionContext<string> context);
}
