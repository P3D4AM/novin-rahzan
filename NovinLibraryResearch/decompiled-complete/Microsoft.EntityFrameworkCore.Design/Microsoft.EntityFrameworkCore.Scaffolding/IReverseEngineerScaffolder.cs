using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public interface IReverseEngineerScaffolder
{
	ScaffoldedModel ScaffoldModel([JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.NotNull] DatabaseModelFactoryOptions databaseOptions, [JetBrains.Annotations.NotNull] ModelReverseEngineerOptions modelOptions, [JetBrains.Annotations.NotNull] ModelCodeGenerationOptions codeOptions);

	SavedModelFiles Save([JetBrains.Annotations.NotNull] ScaffoldedModel scaffoldedModel, [JetBrains.Annotations.CanBeNull] string outputDir, bool overwriteFiles);
}
