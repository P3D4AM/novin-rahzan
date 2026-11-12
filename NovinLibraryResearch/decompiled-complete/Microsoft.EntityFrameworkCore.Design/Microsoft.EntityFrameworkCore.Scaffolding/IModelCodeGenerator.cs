using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public interface IModelCodeGenerator : ILanguageBasedService
{
	ScaffoldedModel GenerateModel([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] ModelCodeGenerationOptions options);
}
