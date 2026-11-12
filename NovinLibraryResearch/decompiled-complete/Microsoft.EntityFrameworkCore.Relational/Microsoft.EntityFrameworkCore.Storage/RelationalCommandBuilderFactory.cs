using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalCommandBuilderFactory : IRelationalCommandBuilderFactory
{
	public virtual RelationalCommandBuilderDependencies Dependencies { get; }

	public RelationalCommandBuilderFactory([JetBrains.Annotations.NotNull] RelationalCommandBuilderDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual IRelationalCommandBuilder Create()
	{
		return new RelationalCommandBuilder(Dependencies);
	}
}
