using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IMigrationsSqlGenerator
{
	IReadOnlyList<MigrationCommand> Generate([JetBrains.Annotations.NotNull] IReadOnlyList<MigrationOperation> operations, [JetBrains.Annotations.CanBeNull] IModel model = null);
}
