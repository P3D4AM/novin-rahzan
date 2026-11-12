using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IMigrationsModelDiffer
{
	bool HasDifferences([JetBrains.Annotations.CanBeNull] IModel source, [JetBrains.Annotations.CanBeNull] IModel target);

	IReadOnlyList<MigrationOperation> GetDifferences([JetBrains.Annotations.CanBeNull] IModel source, [JetBrains.Annotations.CanBeNull] IModel target);
}
