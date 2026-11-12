using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IModelValidator
{
	void Validate([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger);
}
