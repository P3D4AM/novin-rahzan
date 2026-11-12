using System.IO;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class CSharpModelGenerator : ModelCodeGenerator
{
	private const string FileExtension = ".cs";

	public virtual ICSharpDbContextGenerator CSharpDbContextGenerator { get; }

	public virtual ICSharpEntityTypeGenerator CSharpEntityTypeGenerator { get; }

	public override string Language => "C#";

	public CSharpModelGenerator([JetBrains.Annotations.NotNull] ModelCodeGeneratorDependencies dependencies, [JetBrains.Annotations.NotNull] ICSharpDbContextGenerator cSharpDbContextGenerator, [JetBrains.Annotations.NotNull] ICSharpEntityTypeGenerator cSharpEntityTypeGenerator)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cSharpDbContextGenerator, "cSharpDbContextGenerator");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cSharpEntityTypeGenerator, "cSharpEntityTypeGenerator");
		CSharpDbContextGenerator = cSharpDbContextGenerator;
		CSharpEntityTypeGenerator = cSharpEntityTypeGenerator;
	}

	public override ScaffoldedModel GenerateModel(IModel model, ModelCodeGenerationOptions options)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		ScaffoldedModel scaffoldedModel = new ScaffoldedModel();
		string code = CSharpDbContextGenerator.WriteCode(model, options.ContextName, options.ConnectionString, options.ContextNamespace, options.ModelNamespace, options.UseDataAnnotations, options.SuppressConnectionStringWarning);
		string text = options.ContextName + ".cs";
		scaffoldedModel.ContextFile = new ScaffoldedFile
		{
			Path = ((options.ContextDir != null) ? Path.Combine(options.ContextDir, text) : text),
			Code = code
		};
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			code = CSharpEntityTypeGenerator.WriteCode(entityType, options.ModelNamespace, options.UseDataAnnotations);
			string path = entityType.DisplayName() + ".cs";
			scaffoldedModel.AdditionalFiles.Add(new ScaffoldedFile
			{
				Path = path,
				Code = code
			});
		}
		return scaffoldedModel;
	}
}
