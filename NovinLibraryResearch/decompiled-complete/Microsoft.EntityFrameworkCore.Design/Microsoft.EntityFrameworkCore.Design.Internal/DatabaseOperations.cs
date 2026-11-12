using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class DatabaseOperations
{
	private readonly IOperationReporter _reporter;

	private readonly string _projectDir;

	private readonly string _rootNamespace;

	private readonly string _language;

	private readonly DesignTimeServicesBuilder _servicesBuilder;

	public DatabaseOperations([JetBrains.Annotations.NotNull] IOperationReporter reporter, [JetBrains.Annotations.NotNull] Assembly assembly, [JetBrains.Annotations.NotNull] Assembly startupAssembly, [JetBrains.Annotations.NotNull] string projectDir, [JetBrains.Annotations.NotNull] string rootNamespace, [JetBrains.Annotations.CanBeNull] string language, [JetBrains.Annotations.NotNull] string[] args)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reporter, "reporter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(startupAssembly, "startupAssembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(projectDir, "projectDir");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(rootNamespace, "rootNamespace");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(args, "args");
		_reporter = reporter;
		_projectDir = projectDir;
		_rootNamespace = rootNamespace;
		_language = language;
		_servicesBuilder = new DesignTimeServicesBuilder(assembly, startupAssembly, reporter, args);
	}

	public virtual SavedModelFiles ScaffoldContext([JetBrains.Annotations.NotNull] string provider, [JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.CanBeNull] string outputDir, [JetBrains.Annotations.CanBeNull] string outputContextDir, [JetBrains.Annotations.CanBeNull] string dbContextClassName, [JetBrains.Annotations.NotNull] IEnumerable<string> schemas, [JetBrains.Annotations.NotNull] IEnumerable<string> tables, bool useDataAnnotations, bool overwriteFiles, bool useDatabaseNames)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(provider, "provider");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(connectionString, "connectionString");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(schemas, "schemas");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(tables, "tables");
		outputDir = ((outputDir != null) ? Path.GetFullPath(Path.Combine(_projectDir, outputDir)) : _projectDir);
		outputContextDir = ((outputContextDir != null) ? Path.GetFullPath(Path.Combine(_projectDir, outputContextDir)) : outputDir);
		IReverseEngineerScaffolder requiredService = ServiceProviderServiceExtensions.GetRequiredService<IReverseEngineerScaffolder>(_servicesBuilder.Build(provider));
		string namespaceFromOutputPath = GetNamespaceFromOutputPath(outputDir);
		string namespaceFromOutputPath2 = GetNamespaceFromOutputPath(outputContextDir);
		ScaffoldedModel scaffoldedModel = requiredService.ScaffoldModel(connectionString, new DatabaseModelFactoryOptions(tables, schemas), new ModelReverseEngineerOptions
		{
			UseDatabaseNames = useDatabaseNames
		}, new ModelCodeGenerationOptions
		{
			UseDataAnnotations = useDataAnnotations,
			RootNamespace = _rootNamespace,
			ModelNamespace = namespaceFromOutputPath,
			ContextNamespace = namespaceFromOutputPath2,
			Language = _language,
			ContextDir = MakeDirRelative(outputDir, outputContextDir),
			ContextName = dbContextClassName
		});
		return requiredService.Save(scaffoldedModel, outputDir, overwriteFiles);
	}

	private string GetNamespaceFromOutputPath(string directoryPath)
	{
		string text = SubnamespaceFromOutputPath(_projectDir, directoryPath);
		if (!string.IsNullOrEmpty(text))
		{
			return _rootNamespace + "." + text;
		}
		return _rootNamespace;
	}

	private static string SubnamespaceFromOutputPath(string projectDir, string outputDir)
	{
		if (!outputDir.StartsWith(projectDir, StringComparison.Ordinal))
		{
			return null;
		}
		string text = outputDir.Substring(projectDir.Length);
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		return string.Join(".", text.Split(new char[2]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		}, StringSplitOptions.RemoveEmptyEntries));
	}

	private static string MakeDirRelative(string root, string path)
	{
		return Uri.UnescapeDataString(new Uri(NormalizeDir(root)).MakeRelativeUri(new Uri(NormalizeDir(path))).ToString()).Replace('/', Path.DirectorySeparatorChar);
	}

	private static string NormalizeDir(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return path;
		}
		char c = path[path.Length - 1];
		if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
		{
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			return path + directorySeparatorChar;
		}
		return path;
	}
}
