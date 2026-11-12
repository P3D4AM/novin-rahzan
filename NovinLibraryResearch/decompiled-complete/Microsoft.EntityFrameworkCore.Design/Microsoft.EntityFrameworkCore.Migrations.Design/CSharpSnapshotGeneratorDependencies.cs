using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public sealed class CSharpSnapshotGeneratorDependencies
{
	public ICSharpHelper CSharpHelper { get; }

	public IRelationalTypeMappingSource RelationalTypeMappingSource { get; }

	[EntityFrameworkInternal]
	public CSharpSnapshotGeneratorDependencies([JetBrains.Annotations.NotNull] ICSharpHelper csharpHelper, [JetBrains.Annotations.NotNull] IRelationalTypeMappingSource relationalTypeMappingSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(csharpHelper, "csharpHelper");
		CSharpHelper = csharpHelper;
		RelationalTypeMappingSource = relationalTypeMappingSource;
	}

	public CSharpSnapshotGeneratorDependencies With([JetBrains.Annotations.NotNull] ICSharpHelper csharpHelper)
	{
		return new CSharpSnapshotGeneratorDependencies(csharpHelper, RelationalTypeMappingSource);
	}

	public CSharpSnapshotGeneratorDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource relationalTypeMappingSource)
	{
		return new CSharpSnapshotGeneratorDependencies(CSharpHelper, relationalTypeMappingSource);
	}
}
