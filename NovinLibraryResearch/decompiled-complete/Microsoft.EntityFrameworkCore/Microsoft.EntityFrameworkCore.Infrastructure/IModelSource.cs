using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IModelSource
{
	IModel GetModel([JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] IConventionSetBuilder conventionSetBuilder);
}
