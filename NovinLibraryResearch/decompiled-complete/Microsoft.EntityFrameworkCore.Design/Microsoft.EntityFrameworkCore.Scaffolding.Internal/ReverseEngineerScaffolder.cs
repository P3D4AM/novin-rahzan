using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class ReverseEngineerScaffolder : IReverseEngineerScaffolder
{
	private readonly IDatabaseModelFactory _databaseModelFactory;

	private readonly IScaffoldingModelFactory _factory;

	private readonly ICSharpUtilities _cSharpUtilities;

	private readonly ICSharpHelper _code;

	private readonly INamedConnectionStringResolver _connectionStringResolver;

	private const string DbContextSuffix = "Context";

	private const string DefaultDbContextName = "ModelContext";

	private IModelCodeGeneratorSelector ModelCodeGeneratorSelector { get; }

	public ReverseEngineerScaffolder([JetBrains.Annotations.NotNull] IDatabaseModelFactory databaseModelFactory, [JetBrains.Annotations.NotNull] IScaffoldingModelFactory scaffoldingModelFactory, [JetBrains.Annotations.NotNull] IModelCodeGeneratorSelector modelCodeGeneratorSelector, [JetBrains.Annotations.NotNull] ICSharpUtilities cSharpUtilities, [JetBrains.Annotations.NotNull] ICSharpHelper cSharpHelper, [JetBrains.Annotations.NotNull] INamedConnectionStringResolver connectionStringResolver)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseModelFactory, "databaseModelFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(scaffoldingModelFactory, "scaffoldingModelFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelCodeGeneratorSelector, "modelCodeGeneratorSelector");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cSharpHelper, "cSharpHelper");
		_databaseModelFactory = databaseModelFactory;
		_factory = scaffoldingModelFactory;
		ModelCodeGeneratorSelector = modelCodeGeneratorSelector;
		_cSharpUtilities = cSharpUtilities;
		_code = cSharpHelper;
		_connectionStringResolver = connectionStringResolver;
	}

	public virtual ScaffoldedModel ScaffoldModel(string connectionString, DatabaseModelFactoryOptions databaseOptions, ModelReverseEngineerOptions modelOptions, ModelCodeGenerationOptions codeOptions)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(connectionString, "connectionString");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseOptions, "databaseOptions");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelOptions, "modelOptions");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(codeOptions, "codeOptions");
		if (!string.IsNullOrWhiteSpace(codeOptions.ContextName) && (!_cSharpUtilities.IsValidIdentifier(codeOptions.ContextName) || _cSharpUtilities.IsCSharpKeyword(codeOptions.ContextName)))
		{
			throw new ArgumentException(DesignStrings.ContextClassNotValidCSharpIdentifier(codeOptions.ContextName));
		}
		string text = _connectionStringResolver.ResolveConnectionString(connectionString);
		if (text != connectionString)
		{
			codeOptions.SuppressConnectionStringWarning = true;
		}
		if (codeOptions.ConnectionString == null)
		{
			codeOptions.ConnectionString = connectionString;
		}
		DatabaseModel databaseModel = _databaseModelFactory.Create(text, databaseOptions);
		IModel model = _factory.Create(databaseModel, modelOptions.UseDatabaseNames);
		if (model == null)
		{
			throw new InvalidOperationException(DesignStrings.ProviderReturnedNullModel(_factory.GetType().ShortDisplayName()));
		}
		if (string.IsNullOrEmpty(codeOptions.ContextName))
		{
			string databaseName = model.GetDatabaseName();
			codeOptions.ContextName = ((!string.IsNullOrEmpty(databaseName)) ? _code.Identifier(databaseName + "Context") : "ModelContext");
		}
		return ModelCodeGeneratorSelector.Select(codeOptions.Language).GenerateModel(model, codeOptions);
	}

	public virtual SavedModelFiles Save(ScaffoldedModel scaffoldedModel, string outputDir, bool overwriteFiles)
	{
		CheckOutputFiles(scaffoldedModel, outputDir, overwriteFiles);
		Directory.CreateDirectory(outputDir);
		string fullPath = Path.GetFullPath(Path.Combine(outputDir, scaffoldedModel.ContextFile.Path));
		Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
		File.WriteAllText(fullPath, scaffoldedModel.ContextFile.Code, Encoding.UTF8);
		List<string> list = new List<string>();
		foreach (ScaffoldedFile additionalFile in scaffoldedModel.AdditionalFiles)
		{
			string text = Path.Combine(outputDir, additionalFile.Path);
			File.WriteAllText(text, additionalFile.Code, Encoding.UTF8);
			list.Add(text);
		}
		return new SavedModelFiles(fullPath, list);
	}

	private static void CheckOutputFiles(ScaffoldedModel scaffoldedModel, string outputDir, bool overwriteFiles)
	{
		List<string> list = scaffoldedModel.AdditionalFiles.Select((ScaffoldedFile f) => f.Path).ToList();
		list.Insert(0, scaffoldedModel.ContextFile.Path);
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (string item in list)
		{
			string path = Path.Combine(outputDir, item);
			if (File.Exists(path))
			{
				list2.Add(item);
				if (File.GetAttributes(path).HasFlag(FileAttributes.ReadOnly))
				{
					list3.Add(item);
				}
			}
		}
		if (!overwriteFiles && list2.Count != 0)
		{
			throw new OperationException(DesignStrings.ExistingFiles(outputDir, string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, list2)));
		}
		if (list3.Count != 0)
		{
			throw new OperationException(DesignStrings.ReadOnlyFiles(outputDir, string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, list3)));
		}
	}
}
