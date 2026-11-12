using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public abstract class ModelCodeGenerator : IModelCodeGenerator, ILanguageBasedService
{
	public abstract string Language { get; }

	protected virtual ModelCodeGeneratorDependencies Dependencies { get; }

	protected ModelCodeGenerator([JetBrains.Annotations.NotNull] ModelCodeGeneratorDependencies dependencies)
	{
		Dependencies = Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public abstract ScaffoldedModel GenerateModel(IModel model, ModelCodeGenerationOptions options);
}
