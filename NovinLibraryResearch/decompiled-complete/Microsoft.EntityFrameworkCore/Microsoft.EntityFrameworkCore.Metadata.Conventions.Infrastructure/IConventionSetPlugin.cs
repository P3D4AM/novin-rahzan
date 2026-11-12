using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

public interface IConventionSetPlugin
{
	ConventionSet ModifyConventions([JetBrains.Annotations.NotNull] ConventionSet conventionSet);
}
