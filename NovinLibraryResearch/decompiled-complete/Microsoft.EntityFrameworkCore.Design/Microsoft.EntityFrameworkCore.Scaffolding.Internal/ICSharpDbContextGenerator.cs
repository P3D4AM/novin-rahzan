using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public interface ICSharpDbContextGenerator
{
	string WriteCode([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] string contextName, [JetBrains.Annotations.NotNull] string connectionString, string contextNamespace, string modelNamespace, bool useDataAnnotations, bool suppressConnectionStringWarning);
}
