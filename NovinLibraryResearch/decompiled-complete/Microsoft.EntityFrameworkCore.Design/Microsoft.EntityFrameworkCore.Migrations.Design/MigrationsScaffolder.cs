using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public class MigrationsScaffolder : IMigrationsScaffolder
{
	private readonly Type _contextType;

	private readonly string _activeProvider;

	protected virtual MigrationsScaffolderDependencies Dependencies { get; }

	public MigrationsScaffolder([JetBrains.Annotations.NotNull] MigrationsScaffolderDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		_contextType = dependencies.CurrentContext.Context.GetType();
		_activeProvider = dependencies.DatabaseProvider.Name;
		Dependencies = dependencies;
	}

	public virtual ScaffoldedMigration ScaffoldMigration([JetBrains.Annotations.NotNull] string migrationName, [JetBrains.Annotations.NotNull] string rootNamespace, [JetBrains.Annotations.CanBeNull] string subNamespace)
	{
		return ScaffoldMigration(migrationName, rootNamespace, subNamespace, null);
	}

	public virtual ScaffoldedMigration ScaffoldMigration(string migrationName, string rootNamespace, string subNamespace = null, string language = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationName, "migrationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(rootNamespace, "rootNamespace");
		if (Dependencies.MigrationsAssembly.FindMigrationId(migrationName) != null)
		{
			throw new OperationException(DesignStrings.DuplicateMigrationName(migrationName));
		}
		bool flag = false;
		if (string.IsNullOrEmpty(subNamespace))
		{
			flag = true;
			subNamespace = "Migrations";
		}
		KeyValuePair<string, TypeInfo> keyValuePair = Dependencies.MigrationsAssembly.Migrations.LastOrDefault();
		string text = rootNamespace + "." + subNamespace;
		if (flag)
		{
			text = GetNamespace(keyValuePair.Value?.AsType(), text);
		}
		string text2 = _contextType.Name;
		int num = text2.IndexOf('`');
		if (num != -1)
		{
			text2 = text2.Substring(0, num);
		}
		if (ContainsForeignMigrations(text))
		{
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder().Append(rootNamespace).Append(".Migrations.");
				if (text2.EndsWith("Context", StringComparison.Ordinal))
				{
					stringBuilder.Append(text2, 0, text2.Length - 7);
				}
				else
				{
					stringBuilder.Append(text2).Append("Migrations");
				}
				text = stringBuilder.ToString();
			}
			else
			{
				Dependencies.OperationReporter.WriteWarning(DesignStrings.ForeignMigrations(text));
			}
		}
		ModelSnapshot modelSnapshot = Dependencies.MigrationsAssembly.ModelSnapshot;
		IModel model = Dependencies.SnapshotModelProcessor.Process(modelSnapshot?.Model);
		IReadOnlyList<MigrationOperation> differences = Dependencies.MigrationsModelDiffer.GetDifferences(model, Dependencies.Model);
		IReadOnlyList<MigrationOperation> readOnlyList2;
		if (differences.Count <= 0)
		{
			IReadOnlyList<MigrationOperation> readOnlyList = new List<MigrationOperation>();
			readOnlyList2 = readOnlyList;
		}
		else
		{
			readOnlyList2 = Dependencies.MigrationsModelDiffer.GetDifferences(Dependencies.Model, model);
		}
		IReadOnlyList<MigrationOperation> downOperations = readOnlyList2;
		string migrationId = Dependencies.MigrationsIdGenerator.GenerateId(migrationName);
		string @namespace = GetNamespace(modelSnapshot?.GetType(), text);
		string text3 = text2 + "ModelSnapshot";
		if (modelSnapshot != null)
		{
			string name = modelSnapshot.GetType().Name;
			if (name != text3)
			{
				Dependencies.OperationReporter.WriteVerbose(DesignStrings.ReusingSnapshotName(name));
				text3 = name;
			}
		}
		if (differences.Any((MigrationOperation o) => o.IsDestructiveChange))
		{
			Dependencies.OperationReporter.WriteWarning(DesignStrings.DestructiveOperation);
		}
		IMigrationsCodeGenerator migrationsCodeGenerator = Dependencies.MigrationsCodeGeneratorSelector.Select(language);
		return new ScaffoldedMigration(migrationCode: migrationsCodeGenerator.GenerateMigration(text, migrationName, differences, downOperations), metadataCode: migrationsCodeGenerator.GenerateMetadata(text, _contextType, migrationName, migrationId, Dependencies.Model), snapshotCode: migrationsCodeGenerator.GenerateSnapshot(@namespace, _contextType, text3, Dependencies.Model), fileExtension: migrationsCodeGenerator.FileExtension, previousMigrationId: keyValuePair.Key, migrationId: migrationId, migrationSubNamespace: GetSubNamespace(rootNamespace, text), snapshotName: text3, snapshotSubNamespace: GetSubNamespace(rootNamespace, @namespace));
	}

	protected virtual string GetSubNamespace([JetBrains.Annotations.NotNull] string rootNamespace, [JetBrains.Annotations.NotNull] string @namespace)
	{
		if (!(@namespace == rootNamespace))
		{
			if (!@namespace.StartsWith(rootNamespace + ".", StringComparison.Ordinal))
			{
				return @namespace;
			}
			return @namespace.Substring(rootNamespace.Length + 1);
		}
		return string.Empty;
	}

	public virtual MigrationFiles RemoveMigration([JetBrains.Annotations.NotNull] string projectDir, [JetBrains.Annotations.NotNull] string rootNamespace, bool force)
	{
		return RemoveMigration(projectDir, rootNamespace, force, null);
	}

	public virtual MigrationFiles RemoveMigration(string projectDir, string rootNamespace, bool force, string language)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(projectDir, "projectDir");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(rootNamespace, "rootNamespace");
		MigrationFiles migrationFiles = new MigrationFiles();
		ModelSnapshot modelSnapshot = Dependencies.MigrationsAssembly.ModelSnapshot;
		if (modelSnapshot == null)
		{
			throw new OperationException(DesignStrings.NoSnapshot);
		}
		IMigrationsCodeGenerator migrationsCodeGenerator = Dependencies.MigrationsCodeGeneratorSelector.Select(language);
		IModel model = null;
		List<Migration> list = Dependencies.MigrationsAssembly.Migrations.Select((KeyValuePair<string, TypeInfo> m) => Dependencies.MigrationsAssembly.CreateMigration(m.Value, _activeProvider)).ToList();
		if (list.Count != 0)
		{
			Migration migration = list[list.Count - 1];
			model = migration.TargetModel;
			if (!Dependencies.MigrationsModelDiffer.HasDifferences(model, Dependencies.SnapshotModelProcessor.Process(modelSnapshot.Model)))
			{
				bool flag = false;
				try
				{
					flag = Dependencies.HistoryRepository.GetAppliedMigrations().Any((HistoryRow e) => e.MigrationId.Equals(migration.GetId(), StringComparison.OrdinalIgnoreCase));
				}
				catch (Exception ex) when (force)
				{
					Dependencies.OperationReporter.WriteVerbose(ex.ToString());
					Dependencies.OperationReporter.WriteWarning(DesignStrings.ForceRemoveMigration(migration.GetId(), ex.Message));
				}
				if (flag)
				{
					if (!force)
					{
						throw new OperationException(DesignStrings.RevertMigration(migration.GetId()));
					}
					Dependencies.Migrator.Migrate((list.Count > 1) ? list[list.Count - 2].GetId() : "0");
				}
				string text = migration.GetId() + migrationsCodeGenerator.FileExtension;
				string text2 = TryGetProjectFile(projectDir, text);
				if (text2 != null)
				{
					Dependencies.OperationReporter.WriteInformation(DesignStrings.RemovingMigration(migration.GetId()));
					File.Delete(text2);
					migrationFiles.MigrationFile = text2;
				}
				else
				{
					Dependencies.OperationReporter.WriteWarning(DesignStrings.NoMigrationFile(text, migration.GetType().ShortDisplayName()));
				}
				string fileName = migration.GetId() + ".Designer" + migrationsCodeGenerator.FileExtension;
				string text3 = TryGetProjectFile(projectDir, fileName);
				if (text3 != null)
				{
					File.Delete(text3);
					migrationFiles.MetadataFile = text3;
				}
				else
				{
					Dependencies.OperationReporter.WriteVerbose(DesignStrings.NoMigrationMetadataFile(text3));
				}
				model = ((list.Count > 1) ? list[list.Count - 2].TargetModel : null);
			}
			else
			{
				Dependencies.OperationReporter.WriteVerbose(DesignStrings.ManuallyDeleted);
			}
		}
		string name = modelSnapshot.GetType().Name;
		string text4 = name + migrationsCodeGenerator.FileExtension;
		string text5 = TryGetProjectFile(projectDir, text4);
		if (model == null)
		{
			if (text5 != null)
			{
				Dependencies.OperationReporter.WriteInformation(DesignStrings.RemovingSnapshot);
				File.Delete(text5);
				migrationFiles.SnapshotFile = text5;
			}
			else
			{
				Dependencies.OperationReporter.WriteWarning(DesignStrings.NoSnapshotFile(text4, modelSnapshot.GetType().ShortDisplayName()));
			}
		}
		else
		{
			string @namespace = modelSnapshot.GetType().Namespace;
			string contents = migrationsCodeGenerator.GenerateSnapshot(@namespace, _contextType, name, model);
			if (text5 == null)
			{
				text5 = Path.Combine(GetDirectory(projectDir, null, GetSubNamespace(rootNamespace, @namespace)), text4);
			}
			Dependencies.OperationReporter.WriteInformation(DesignStrings.RevertingSnapshot);
			File.WriteAllText(text5, contents, Encoding.UTF8);
		}
		return migrationFiles;
	}

	public virtual MigrationFiles Save(string projectDir, ScaffoldedMigration migration, string outputDir)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(projectDir, "projectDir");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migration, "migration");
		string siblingFileName = migration.PreviousMigrationId + migration.FileExtension;
		string text = outputDir ?? GetDirectory(projectDir, siblingFileName, migration.MigrationSubNamespace);
		string text2 = Path.Combine(text, migration.MigrationId + migration.FileExtension);
		string text3 = Path.Combine(text, migration.MigrationId + ".Designer" + migration.FileExtension);
		string text4 = migration.SnapshotName + migration.FileExtension;
		string obj = outputDir ?? GetDirectory(projectDir, text4, migration.SnapshotSubnamespace);
		string text5 = Path.Combine(obj, text4);
		Dependencies.OperationReporter.WriteVerbose(DesignStrings.WritingMigration(text2));
		Directory.CreateDirectory(text);
		File.WriteAllText(text2, migration.MigrationCode, Encoding.UTF8);
		File.WriteAllText(text3, migration.MetadataCode, Encoding.UTF8);
		Dependencies.OperationReporter.WriteVerbose(DesignStrings.WritingSnapshot(text5));
		Directory.CreateDirectory(obj);
		File.WriteAllText(text5, migration.SnapshotCode, Encoding.UTF8);
		return new MigrationFiles
		{
			MigrationFile = text2,
			MetadataFile = text3,
			SnapshotFile = text5
		};
	}

	protected virtual string GetNamespace([JetBrains.Annotations.CanBeNull] Type siblingType, [JetBrains.Annotations.NotNull] string defaultNamespace)
	{
		if (siblingType != null)
		{
			string @namespace = siblingType.Namespace;
			if (@namespace != defaultNamespace)
			{
				Dependencies.OperationReporter.WriteVerbose(DesignStrings.ReusingNamespace(siblingType.ShortDisplayName()));
				return @namespace;
			}
		}
		return defaultNamespace;
	}

	protected virtual string GetDirectory([JetBrains.Annotations.NotNull] string projectDir, [JetBrains.Annotations.CanBeNull] string siblingFileName, [JetBrains.Annotations.NotNull] string subnamespace)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(projectDir, "projectDir");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(subnamespace, "subnamespace");
		string text = Path.Combine(projectDir, Path.Combine(subnamespace.Split(new char[1] { '.' })));
		if (siblingFileName != null)
		{
			string text2 = TryGetProjectFile(projectDir, siblingFileName);
			if (text2 != null)
			{
				string directoryName = Path.GetDirectoryName(text2);
				if (!text.Equals(directoryName, StringComparison.OrdinalIgnoreCase))
				{
					Dependencies.OperationReporter.WriteVerbose(DesignStrings.ReusingNamespace(siblingFileName));
					return directoryName;
				}
			}
		}
		return text;
	}

	protected virtual string TryGetProjectFile([JetBrains.Annotations.NotNull] string projectDir, [JetBrains.Annotations.NotNull] string fileName)
	{
		return Directory.EnumerateFiles(projectDir, fileName, SearchOption.AllDirectories).FirstOrDefault();
	}

	private bool ContainsForeignMigrations(string migrationsNamespace)
	{
		return (from t in Dependencies.MigrationsAssembly.Assembly.GetConstructibleTypes()
			where t.Namespace == migrationsNamespace && t.IsSubclassOf(typeof(Migration))
			let contextTypeAttribute = t.GetCustomAttribute<DbContextAttribute>()
			where contextTypeAttribute != null && contextTypeAttribute.ContextType != _contextType
			select t).Any();
	}
}
