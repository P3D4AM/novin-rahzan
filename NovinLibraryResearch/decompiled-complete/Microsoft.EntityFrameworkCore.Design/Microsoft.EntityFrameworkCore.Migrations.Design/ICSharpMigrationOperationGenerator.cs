using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public interface ICSharpMigrationOperationGenerator
{
	void Generate([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IReadOnlyList<MigrationOperation> operations, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder);
}
