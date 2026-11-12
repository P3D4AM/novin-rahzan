using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class ModelCustomizer : IModelCustomizer
{
	protected virtual ModelCustomizerDependencies Dependencies { get; }

	public ModelCustomizer([JetBrains.Annotations.NotNull] ModelCustomizerDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual void Customize(ModelBuilder modelBuilder, DbContext context)
	{
		context.OnModelCreating(modelBuilder);
	}
}
