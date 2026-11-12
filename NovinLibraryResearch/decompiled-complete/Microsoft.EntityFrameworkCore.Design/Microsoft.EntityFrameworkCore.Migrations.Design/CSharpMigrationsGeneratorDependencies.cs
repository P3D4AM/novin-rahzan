using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public sealed class CSharpMigrationsGeneratorDependencies
{
	public ICSharpHelper CSharpHelper { get; }

	public ICSharpMigrationOperationGenerator CSharpMigrationOperationGenerator { get; }

	public ICSharpSnapshotGenerator CSharpSnapshotGenerator { get; }

	[EntityFrameworkInternal]
	public CSharpMigrationsGeneratorDependencies([JetBrains.Annotations.NotNull] ICSharpHelper csharpHelper, [JetBrains.Annotations.NotNull] ICSharpMigrationOperationGenerator csharpMigrationOperationGenerator, [JetBrains.Annotations.NotNull] ICSharpSnapshotGenerator csharpSnapshotGenerator)
	{
		CSharpHelper = csharpHelper;
		CSharpMigrationOperationGenerator = csharpMigrationOperationGenerator;
		CSharpSnapshotGenerator = csharpSnapshotGenerator;
	}

	public CSharpMigrationsGeneratorDependencies With([JetBrains.Annotations.NotNull] ICSharpHelper csharpHelper)
	{
		return new CSharpMigrationsGeneratorDependencies(csharpHelper, CSharpMigrationOperationGenerator, CSharpSnapshotGenerator);
	}

	public CSharpMigrationsGeneratorDependencies With([JetBrains.Annotations.NotNull] ICSharpMigrationOperationGenerator csharpMigrationOperationGenerator)
	{
		return new CSharpMigrationsGeneratorDependencies(CSharpHelper, csharpMigrationOperationGenerator, CSharpSnapshotGenerator);
	}

	public CSharpMigrationsGeneratorDependencies With([JetBrains.Annotations.NotNull] ICSharpSnapshotGenerator csharpSnapshotGenerator)
	{
		return new CSharpMigrationsGeneratorDependencies(CSharpHelper, CSharpMigrationOperationGenerator, csharpSnapshotGenerator);
	}
}
