using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public sealed class CSharpMigrationOperationGeneratorDependencies
{
	public ICSharpHelper CSharpHelper { get; }

	[EntityFrameworkInternal]
	public CSharpMigrationOperationGeneratorDependencies([JetBrains.Annotations.NotNull] ICSharpHelper csharpHelper)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(csharpHelper, "csharpHelper");
		CSharpHelper = csharpHelper;
	}

	public CSharpMigrationOperationGeneratorDependencies With([JetBrains.Annotations.NotNull] ICSharpHelper csharpHelper)
	{
		return new CSharpMigrationOperationGeneratorDependencies(csharpHelper);
	}
}
