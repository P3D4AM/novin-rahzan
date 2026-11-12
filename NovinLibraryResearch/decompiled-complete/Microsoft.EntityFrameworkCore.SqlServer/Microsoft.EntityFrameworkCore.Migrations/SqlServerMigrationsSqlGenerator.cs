using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public class SqlServerMigrationsSqlGenerator : MigrationsSqlGenerator
{
	private readonly IMigrationsAnnotationProvider _migrationsAnnotations;

	private IReadOnlyList<MigrationOperation> _operations;

	private int _variableCounter;

	public SqlServerMigrationsSqlGenerator([JetBrains.Annotations.NotNull] MigrationsSqlGeneratorDependencies dependencies, [JetBrains.Annotations.NotNull] IMigrationsAnnotationProvider migrationsAnnotations)
		: base(dependencies)
	{
		_migrationsAnnotations = migrationsAnnotations;
	}

	public override IReadOnlyList<MigrationCommand> Generate(IReadOnlyList<MigrationOperation> operations, IModel model)
	{
		_operations = operations;
		try
		{
			return base.Generate(operations, model);
		}
		finally
		{
			_operations = null;
		}
	}

	protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (!(operation is SqlServerCreateDatabaseOperation operation2))
		{
			if (operation is SqlServerDropDatabaseOperation operation3)
			{
				Generate(operation3, model, builder);
			}
			else
			{
				base.Generate(operation, model, builder);
			}
		}
		else
		{
			Generate(operation2, model, builder);
		}
	}

	protected override void Generate(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate)
	{
		if (!terminate && operation.Comment != null)
		{
			throw new ArgumentException("When generating migrations SQL for AddColumnOperation, can't produce unterminated SQL with comments");
		}
		if (IsIdentity(operation))
		{
			operation.DefaultValue = null;
		}
		base.Generate(operation, model, builder, terminate: false);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			if (operation.Comment != null)
			{
				AddDescription(builder, operation.Comment, operation.Schema, operation.Table, operation.Name);
			}
			builder.EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
		}
	}

	protected override void Generate(AddForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		base.Generate(operation, model, builder, terminate: false);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
		}
	}

	protected override void Generate(AddPrimaryKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		base.Generate(operation, model, builder, terminate: false);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
		}
	}

	protected override void Generate(AlterColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		IEnumerable<IIndex> indexes = null;
		IProperty property = FindProperty(model, operation.Schema, operation.Table, operation.Name);
		if (operation.ComputedColumnSql != null)
		{
			DropColumnOperation dropColumnOperation = new DropColumnOperation
			{
				Schema = operation.Schema,
				Table = operation.Table,
				Name = operation.Name
			};
			if (property != null)
			{
				dropColumnOperation.AddAnnotations(_migrationsAnnotations.ForRemove(property));
			}
			AddColumnOperation addColumnOperation = new AddColumnOperation
			{
				Schema = operation.Schema,
				Table = operation.Table,
				Name = operation.Name,
				ClrType = operation.ClrType,
				ColumnType = operation.ColumnType,
				IsUnicode = operation.IsUnicode,
				MaxLength = operation.MaxLength,
				IsRowVersion = operation.IsRowVersion,
				IsNullable = operation.IsNullable,
				DefaultValue = operation.DefaultValue,
				DefaultValueSql = operation.DefaultValueSql,
				ComputedColumnSql = operation.ComputedColumnSql,
				IsFixedLength = operation.IsFixedLength
			};
			addColumnOperation.AddAnnotations(operation.GetAnnotations());
			indexes = GetIndexesToRebuild(property, operation).ToList();
			DropIndexes(indexes, builder);
			Generate(dropColumnOperation, model, builder, terminate: false);
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			Generate(addColumnOperation, model, builder, terminate: false);
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			CreateIndexes(indexes, builder);
			builder.EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
			return;
		}
		bool flag = false;
		if (IsOldColumnSupported(model))
		{
			if (IsIdentity(operation) != IsIdentity(operation.OldColumn))
			{
				throw new InvalidOperationException(SqlServerStrings.AlterIdentityColumn);
			}
			string obj = operation.ColumnType ?? GetColumnType(operation.Schema, operation.Table, operation.Name, operation, model);
			string text = operation.OldColumn.ColumnType ?? GetColumnType(operation.Schema, operation.Table, operation.Name, operation.OldColumn, model);
			flag = obj != text || (!operation.IsNullable && operation.OldColumn.IsNullable);
		}
		if (flag)
		{
			indexes = GetIndexesToRebuild(property, operation).ToList();
			DropIndexes(indexes, builder);
		}
		DropDefaultConstraint(operation.Schema, operation.Table, operation.Name, builder);
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ALTER COLUMN ");
		AlterColumnOperation alterColumnOperation = new AlterColumnOperation
		{
			Schema = operation.Schema,
			Table = operation.Table,
			Name = operation.Name,
			ClrType = operation.ClrType,
			ColumnType = operation.ColumnType,
			IsUnicode = operation.IsUnicode,
			IsFixedLength = operation.IsFixedLength,
			MaxLength = operation.MaxLength,
			IsRowVersion = operation.IsRowVersion,
			IsNullable = operation.IsNullable,
			ComputedColumnSql = operation.ComputedColumnSql,
			OldColumn = operation.OldColumn
		};
		alterColumnOperation.AddAnnotations(from a in operation.GetAnnotations()
			where a.Name != "SqlServer:ValueGenerationStrategy" && a.Name != "SqlServer:Identity"
			select a);
		ColumnDefinition(operation.Schema, operation.Table, operation.Name, alterColumnOperation, model, builder);
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		if (operation.DefaultValue != null || operation.DefaultValueSql != null)
		{
			builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ADD");
			DefaultValue(operation.DefaultValue, operation.DefaultValueSql, operation.ColumnType, builder);
			builder.Append(" FOR ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		}
		if (operation.OldColumn.Comment != operation.Comment)
		{
			bool flag2 = operation.OldColumn.Comment != null;
			if (flag2)
			{
				DropDescription(builder, operation.Schema, operation.Table, operation.Name);
			}
			if (operation.Comment != null)
			{
				AddDescription(builder, operation.Comment, operation.Schema, operation.Table, operation.Name, flag2);
			}
		}
		if (flag)
		{
			CreateIndexes(indexes, builder);
		}
		builder.EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
	}

	protected override void Generate(RenameIndexOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (string.IsNullOrEmpty(operation.Table))
		{
			throw new InvalidOperationException(SqlServerStrings.IndexTableRequired);
		}
		Rename(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema) + "." + Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name), operation.NewName, "INDEX", builder);
		builder.EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
	}

	protected override void Generate(RenameSequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		string text = operation.Name;
		if (operation.NewName != null && operation.NewName != text)
		{
			Rename(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema), operation.NewName, builder);
			text = operation.NewName;
		}
		if (operation.NewSchema != operation.Schema && (operation.NewSchema != null || !HasLegacyRenameOperations(model)))
		{
			Transfer(operation.NewSchema, operation.Schema, text, builder);
		}
		builder.EndCommand();
	}

	protected override void Generate(RestartSequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER SEQUENCE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema)).Append(" RESTART WITH ")
			.Append(IntegerConstant(operation.StartValue))
			.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		if (!terminate && operation.Comment != null)
		{
			throw new ArgumentException("When generating migrations SQL for CreateTableOperation, can't produce unterminated SQL with comments");
		}
		base.Generate(operation, model, builder, terminate: false);
		bool flag = IsMemoryOptimized(operation);
		if (flag)
		{
			builder.AppendLine();
			using (builder.Indent())
			{
				builder.AppendLine("WITH");
				using (builder.Indent())
				{
					builder.Append("(MEMORY_OPTIMIZED = ON)");
				}
			}
		}
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		bool flag2 = true;
		if (operation.Comment != null)
		{
			AddDescription(builder, operation.Comment, operation.Schema, operation.Name);
			flag2 = false;
		}
		foreach (AddColumnOperation item in operation.Columns.Where((AddColumnOperation c) => c.Comment != null))
		{
			AddDescription(builder, item.Comment, operation.Schema, operation.Name, item.Name, !flag2);
			flag2 = false;
		}
		builder.EndCommand(flag);
	}

	protected override void Generate(RenameTableOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		string text = operation.Name;
		if (operation.NewName != null && operation.NewName != text)
		{
			Rename(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema), operation.NewName, builder);
			text = operation.NewName;
		}
		if (operation.NewSchema != operation.Schema && (operation.NewSchema != null || !HasLegacyRenameOperations(model)))
		{
			Transfer(operation.NewSchema, operation.Schema, text, builder);
		}
		builder.EndCommand();
	}

	protected override void Generate(DropTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		base.Generate(operation, model, builder, terminate: false);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Name));
		}
	}

	protected override void Generate(CreateIndexOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		List<string> list = operation.Columns.Where((string c) => FindProperty(model, operation.Schema, operation.Table, c)?.IsColumnNullable() ?? true).ToList();
		bool flag = IsMemoryOptimized(operation, model, operation.Schema, operation.Table);
		if (flag)
		{
			builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ADD INDEX ")
				.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
				.Append(" ");
			if (operation.IsUnique && list.Count == 0)
			{
				builder.Append("UNIQUE ");
			}
			IndexTraits(operation, model, builder);
			builder.Append("(").Append(ColumnList(operation.Columns)).Append(")");
		}
		else
		{
			base.Generate(operation, model, builder, terminate: false);
			if (operation.Filter == null && UseLegacyIndexFilters(model))
			{
				bool? flag2 = operation["SqlServer:Clustered"] as bool?;
				if (operation.IsUnique && flag2 != true && list.Count != 0)
				{
					builder.Append(" WHERE ");
					for (int i = 0; i < list.Count; i++)
					{
						if (i != 0)
						{
							builder.Append(" AND ");
						}
						builder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(list[i])).Append(" IS NOT NULL");
					}
				}
			}
		}
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(flag);
		}
	}

	protected override void Generate(DropPrimaryKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		base.Generate(operation, model, builder, terminate: false);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
		}
	}

	protected override void Generate(EnsureSchemaOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (!string.Equals(operation.Name, "dbo", StringComparison.OrdinalIgnoreCase))
		{
			RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
			builder.Append("IF SCHEMA_ID(").Append(mapping.GenerateSqlLiteral(operation.Name)).Append(") IS NULL EXEC(")
				.Append(mapping.GenerateSqlLiteral("CREATE SCHEMA " + Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name) + Dependencies.SqlGenerationHelper.StatementTerminator))
				.Append(")")
				.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
				.EndCommand();
		}
	}

	protected override void Generate(CreateSequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("CREATE SEQUENCE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema));
		if (operation.ClrType != typeof(long))
		{
			RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(operation.ClrType);
			builder.Append(" AS ").Append(mapping.StoreType);
		}
		builder.Append(" START WITH ").Append(IntegerConstant(operation.StartValue));
		SequenceOptions(operation, model, builder);
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] SqlServerCreateDatabaseOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("CREATE DATABASE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
		if (!string.IsNullOrEmpty(operation.FileName))
		{
			RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
			string text = ExpandFileName(operation.FileName);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			string text2 = Path.ChangeExtension(text, ".ldf");
			string value = fileNameWithoutExtension + "_log";
			text2 = text2.Insert(text2.Length - ".ldf".Length, "_log");
			builder.AppendLine().Append("ON (NAME = ").Append(mapping.GenerateSqlLiteral(fileNameWithoutExtension))
				.Append(", FILENAME = ")
				.Append(mapping.GenerateSqlLiteral(text))
				.Append(")")
				.AppendLine()
				.Append("LOG ON (NAME = ")
				.Append(mapping.GenerateSqlLiteral(value))
				.Append(", FILENAME = ")
				.Append(mapping.GenerateSqlLiteral(text2))
				.Append(")");
		}
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(suppressTransaction: true).AppendLine("IF SERVERPROPERTY('EngineEdition') <> 5")
			.AppendLine("BEGIN");
		using (builder.Indent())
		{
			builder.Append("ALTER DATABASE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" SET READ_COMMITTED_SNAPSHOT ON")
				.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		}
		builder.Append("END").AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(suppressTransaction: true);
	}

	private static string ExpandFileName(string fileName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(fileName, "fileName");
		if (fileName.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
		{
			string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
			if (string.IsNullOrEmpty(text))
			{
				text = AppDomain.CurrentDomain.BaseDirectory;
			}
			fileName = Path.Combine(text, fileName.Substring("|DataDirectory|".Length));
		}
		return Path.GetFullPath(fileName);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] SqlServerDropDatabaseOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine("IF SERVERPROPERTY('EngineEdition') <> 5").AppendLine("BEGIN");
		using (builder.Indent())
		{
			builder.Append("ALTER DATABASE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" SET SINGLE_USER WITH ROLLBACK IMMEDIATE")
				.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		}
		builder.Append("END").AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(suppressTransaction: true)
			.Append("DROP DATABASE ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
			.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
			.EndCommand(suppressTransaction: true);
	}

	protected override void Generate(AlterDatabaseOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (operation["SqlServer:EditionOptions"] is string text)
		{
			builder.AppendLine("BEGIN").AppendLine("DECLARE @db_name NVARCHAR(MAX) = DB_NAME();").AppendLine("EXEC(N'ALTER DATABASE ' + @db_name + ' MODIFY ( ")
				.Append(text.Replace("'", "''"))
				.AppendLine(" );');")
				.AppendLine("END")
				.AppendLine();
		}
		if (!IsMemoryOptimized(operation))
		{
			builder.EndCommand();
			return;
		}
		builder.AppendLine("IF SERVERPROPERTY('IsXTPSupported') = 1 AND SERVERPROPERTY('EngineEdition') <> 5");
		using (builder.Indent())
		{
			builder.AppendLine("BEGIN").AppendLine("IF NOT EXISTS (");
			using (builder.Indent())
			{
				builder.Append("SELECT 1 FROM [sys].[filegroups] [FG] ").Append("JOIN [sys].[database_files] [F] ON [FG].[data_space_id] = [F].[data_space_id] ").AppendLine("WHERE [FG].[type] = N'FX' AND [F].[type] = 2)");
			}
			using (builder.Indent())
			{
				builder.AppendLine("BEGIN").AppendLine("ALTER DATABASE CURRENT SET AUTO_CLOSE OFF;").AppendLine("DECLARE @db_name NVARCHAR(MAX) = DB_NAME();")
					.AppendLine("DECLARE @fg_name NVARCHAR(MAX);")
					.AppendLine("SELECT TOP(1) @fg_name = [name] FROM [sys].[filegroups] WHERE [type] = N'FX';")
					.AppendLine()
					.AppendLine("IF @fg_name IS NULL");
				using (builder.Indent())
				{
					builder.AppendLine("BEGIN").AppendLine("SET @fg_name = @db_name + N'_MODFG';").AppendLine("EXEC(N'ALTER DATABASE CURRENT ADD FILEGROUP [' + @fg_name + '] CONTAINS MEMORY_OPTIMIZED_DATA;');")
						.AppendLine("END");
				}
				builder.AppendLine().AppendLine("DECLARE @path NVARCHAR(MAX);").Append("SELECT TOP(1) @path = [physical_name] FROM [sys].[database_files] ")
					.AppendLine("WHERE charindex('\\', [physical_name]) > 0 ORDER BY [file_id];")
					.AppendLine("IF (@path IS NULL)")
					.IncrementIndent()
					.AppendLine("SET @path = '\\' + @db_name;")
					.DecrementIndent()
					.AppendLine()
					.AppendLine("DECLARE @filename NVARCHAR(MAX) = right(@path, charindex('\\', reverse(@path)) - 1);")
					.AppendLine("SET @filename = REPLACE(left(@filename, len(@filename) - charindex('.', reverse(@filename))), '''', '''''') + N'_MOD';")
					.AppendLine("DECLARE @new_path NVARCHAR(MAX) = REPLACE(CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS NVARCHAR(MAX)), '''', '''''') + @filename;")
					.AppendLine()
					.AppendLine("EXEC(N'");
				using (builder.Indent())
				{
					builder.AppendLine("ALTER DATABASE CURRENT").AppendLine("ADD FILE (NAME=''' + @filename + ''', filename=''' + @new_path + ''')").AppendLine("TO FILEGROUP [' + @fg_name + '];')");
				}
				builder.AppendLine("END");
			}
			builder.AppendLine("END");
		}
		builder.AppendLine().AppendLine("IF SERVERPROPERTY('IsXTPSupported') = 1").AppendLine("EXEC(N'");
		using (builder.Indent())
		{
			builder.AppendLine("ALTER DATABASE CURRENT").AppendLine("SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT ON;')");
		}
		builder.EndCommand(suppressTransaction: true);
	}

	protected override void Generate(AlterTableOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		if (IsMemoryOptimized(operation) ^ IsMemoryOptimized(operation.OldTable))
		{
			throw new InvalidOperationException(SqlServerStrings.AlterMemoryOptimizedTable);
		}
		if (operation.OldTable.Comment != operation.Comment)
		{
			bool flag = operation.OldTable.Comment != null;
			if (flag)
			{
				DropDescription(builder, operation.Schema, operation.Name);
			}
			if (operation.Comment != null)
			{
				AddDescription(builder, operation.Comment, operation.Schema, operation.Name, null, flag);
			}
		}
		builder.EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Name));
	}

	protected override void Generate(DropForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		base.Generate(operation, model, builder, terminate: false);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
		}
	}

	protected override void Generate(DropIndexOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		bool flag = IsMemoryOptimized(operation, model, operation.Schema, operation.Table);
		if (flag)
		{
			builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" DROP INDEX ")
				.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
		}
		else
		{
			builder.Append("DROP INDEX ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" ON ")
				.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema));
		}
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(flag);
		}
	}

	protected override void Generate(DropColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		DropDefaultConstraint(operation.Schema, operation.Table, operation.Name, builder);
		base.Generate(operation, model, builder, terminate: false);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(IsMemoryOptimized(operation, model, operation.Schema, operation.Table));
		}
	}

	protected override void Generate(RenameColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Rename(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema) + "." + Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name), operation.NewName, "COLUMN", builder);
		builder.EndCommand();
	}

	protected override void Generate(SqlOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		string[] array = Regex.Split(Regex.Replace(operation.Sql, "\\\\\\r?\\n", string.Empty, RegexOptions.None, TimeSpan.FromMilliseconds(1000.0)), "^\\s*(GO[ \\t]+[0-9]+|GO)(?:\\s+|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline, TimeSpan.FromMilliseconds(1000.0));
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].StartsWith("GO", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(array[i]))
			{
				continue;
			}
			int num = 1;
			if (i != array.Length - 1 && array[i + 1].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
			{
				Match match = Regex.Match(array[i + 1], "([0-9]+)", RegexOptions.None, TimeSpan.FromMilliseconds(1000.0));
				if (match.Success)
				{
					num = int.Parse(match.Value);
				}
			}
			for (int j = 0; j < num; j++)
			{
				builder.Append(array[i]);
				if (i == array.Length - 1)
				{
					builder.AppendLine();
				}
				EndStatement(builder, operation.SuppressTransaction);
			}
		}
	}

	protected override void Generate(InsertDataOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		GenerateIdentityInsert(builder, operation, on: true);
		StringBuilder stringBuilder = new StringBuilder();
		((SqlServerUpdateSqlGenerator)Dependencies.UpdateSqlGenerator).AppendBulkInsertOperation(stringBuilder, operation.GenerateModificationCommands(model).ToList(), 0);
		builder.Append(stringBuilder.ToString());
		GenerateIdentityInsert(builder, operation, on: false);
		if (terminate)
		{
			builder.EndCommand();
		}
	}

	private void GenerateIdentityInsert(MigrationCommandListBuilder builder, InsertDataOperation operation, bool on)
	{
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		builder.Append("IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE").Append(" [name] IN (").Append(string.Join(", ", operation.Columns.Select(mapping.GenerateSqlLiteral)))
			.Append(") AND [object_id] = OBJECT_ID(")
			.Append(mapping.GenerateSqlLiteral(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)))
			.AppendLine("))");
		using (builder.Indent())
		{
			builder.Append("SET IDENTITY_INSERT ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(on ? " ON" : " OFF")
				.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		}
	}

	protected override void SequenceOptions(string schema, string name, SequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append(" INCREMENT BY ").Append(IntegerConstant(operation.IncrementBy));
		if (operation.MinValue.HasValue)
		{
			builder.Append(" MINVALUE ").Append(IntegerConstant(operation.MinValue.Value));
		}
		else
		{
			builder.Append(" NO MINVALUE");
		}
		if (operation.MaxValue.HasValue)
		{
			builder.Append(" MAXVALUE ").Append(IntegerConstant(operation.MaxValue.Value));
		}
		else
		{
			builder.Append(" NO MAXVALUE");
		}
		builder.Append(operation.IsCyclic ? " CYCLE" : " NO CYCLE");
	}

	protected override void ColumnDefinition(string schema, string table, string name, ColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		base.ColumnDefinition(schema, table, name, operation, model, builder);
		string text = operation["SqlServer:Identity"] as string;
		if (text != null || operation["SqlServer:ValueGenerationStrategy"] as SqlServerValueGenerationStrategy? == SqlServerValueGenerationStrategy.IdentityColumn)
		{
			builder.Append(" IDENTITY");
			if (!string.IsNullOrEmpty(text) && text != "1, 1")
			{
				builder.Append("(").Append(text).Append(")");
			}
		}
	}

	protected override void ComputedColumnDefinition(string schema, string table, string name, ColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(name)).Append(" AS ").Append(operation.ComputedColumnSql);
	}

	protected virtual void Rename([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string newName, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Rename(name, newName, null, builder);
	}

	protected virtual void Rename([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string newName, [JetBrains.Annotations.CanBeNull] string type, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(newName, "newName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		builder.Append("EXEC sp_rename ").Append(mapping.GenerateSqlLiteral(name)).Append(", ")
			.Append(mapping.GenerateSqlLiteral(newName));
		if (type != null)
		{
			builder.Append(", ").Append(mapping.GenerateSqlLiteral(type));
		}
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
	}

	protected virtual void Transfer([JetBrains.Annotations.CanBeNull] string newSchema, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (newSchema == null)
		{
			RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
			builder.AppendLine("DECLARE @defaultSchema sysname = SCHEMA_NAME();").Append("EXEC(").Append("N'ALTER SCHEMA [' + @defaultSchema + ")
				.Append(mapping.GenerateSqlLiteral("] TRANSFER " + Dependencies.SqlGenerationHelper.DelimitIdentifier(name, schema) + ";"))
				.AppendLine(");");
		}
		else
		{
			builder.Append("ALTER SCHEMA ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(newSchema)).Append(" TRANSFER ")
				.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(name, schema))
				.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		}
	}

	protected override void IndexTraits(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		bool? flag = operation["SqlServer:Clustered"] as bool?;
		if (flag.HasValue)
		{
			builder.Append(flag.Value ? "CLUSTERED " : "NONCLUSTERED ");
		}
	}

	protected override void IndexOptions(CreateIndexOperation operation, IModel model, MigrationCommandListBuilder builder)
	{
		if (operation["SqlServer:Include"] is IReadOnlyList<string> { Count: >0 } readOnlyList)
		{
			builder.Append(" INCLUDE (");
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				builder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(readOnlyList[i]));
				if (i != readOnlyList.Count - 1)
				{
					builder.Append(", ");
				}
			}
			builder.Append(")");
		}
		base.IndexOptions(operation, model, builder);
		object obj = operation["SqlServer:Online"];
		bool flag = default(bool);
		int num;
		if (obj is bool)
		{
			flag = (bool)obj;
			num = 1;
		}
		else
		{
			num = 0;
		}
		if (((uint)num & (flag ? 1u : 0u)) != 0)
		{
			builder.Append(" WITH (ONLINE = ON)");
		}
	}

	protected override void ForeignKeyAction(ReferentialAction referentialAction, MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (referentialAction == ReferentialAction.Restrict)
		{
			builder.Append("NO ACTION");
		}
		else
		{
			base.ForeignKeyAction(referentialAction, builder);
		}
	}

	protected virtual void DropDefaultConstraint([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string columnName, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(tableName, "tableName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(columnName, "columnName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		string o = "@var" + _variableCounter++;
		builder.Append("DECLARE ").Append(o).AppendLine(" sysname;")
			.Append("SELECT ")
			.Append(o)
			.AppendLine(" = [d].[name]")
			.AppendLine("FROM [sys].[default_constraints] [d]")
			.AppendLine("INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]")
			.Append("WHERE ([d].[parent_object_id] = OBJECT_ID(")
			.Append(mapping.GenerateSqlLiteral(Dependencies.SqlGenerationHelper.DelimitIdentifier(tableName, schema)))
			.Append(") AND [c].[name] = ")
			.Append(mapping.GenerateSqlLiteral(columnName))
			.AppendLine(");")
			.Append("IF ")
			.Append(o)
			.Append(" IS NOT NULL EXEC(")
			.Append(mapping.GenerateSqlLiteral("ALTER TABLE " + Dependencies.SqlGenerationHelper.DelimitIdentifier(tableName, schema) + " DROP CONSTRAINT ["))
			.Append(" + ")
			.Append(o)
			.Append(" + ']")
			.Append(Dependencies.SqlGenerationHelper.StatementTerminator)
			.Append("')")
			.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
	}

	protected virtual IEnumerable<IIndex> GetIndexesToRebuild([JetBrains.Annotations.CanBeNull] IProperty property, [JetBrains.Annotations.NotNull] MigrationOperation currentOperation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentOperation, "currentOperation");
		if (property == null)
		{
			yield break;
		}
		List<CreateIndexOperation> createIndexOperations = _operations.SkipWhile((MigrationOperation o) => o != currentOperation).Skip(1).OfType<CreateIndexOperation>()
			.ToList();
		foreach (IIndex item in property.DeclaringEntityType.GetIndexes().Concat(property.DeclaringEntityType.GetDerivedTypes().SelectMany((IEntityType et) => et.GetDeclaredIndexes())))
		{
			string indexName = item.GetName();
			if (createIndexOperations.Any((CreateIndexOperation o) => o.Name == indexName))
			{
				continue;
			}
			if (item.Properties.Any((IProperty p) => p == property))
			{
				yield return item;
				continue;
			}
			IReadOnlyList<string> includeProperties = item.GetIncludeProperties();
			if (includeProperties != null && includeProperties.Contains(property.Name))
			{
				yield return item;
			}
		}
	}

	protected virtual void DropIndexes([JetBrains.Annotations.NotNull] IEnumerable<IIndex> indexes, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexes, "indexes");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		foreach (IIndex index in indexes)
		{
			DropIndexOperation dropIndexOperation = new DropIndexOperation
			{
				Schema = index.DeclaringEntityType.GetSchema(),
				Table = index.DeclaringEntityType.GetTableName(),
				Name = index.GetName()
			};
			dropIndexOperation.AddAnnotations(_migrationsAnnotations.ForRemove(index));
			Generate(dropIndexOperation, index.DeclaringEntityType.Model, builder, terminate: false);
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		}
	}

	protected virtual void CreateIndexes([JetBrains.Annotations.NotNull] IEnumerable<IIndex> indexes, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexes, "indexes");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		foreach (IIndex index in indexes)
		{
			CreateIndexOperation createIndexOperation = new CreateIndexOperation
			{
				IsUnique = index.IsUnique,
				Name = index.GetName(),
				Schema = index.DeclaringEntityType.GetSchema(),
				Table = index.DeclaringEntityType.GetTableName(),
				Columns = index.Properties.Select((IProperty p) => p.GetColumnName()).ToArray(),
				Filter = index.GetFilter()
			};
			createIndexOperation.AddAnnotations(_migrationsAnnotations.For(index));
			Generate(createIndexOperation, index.DeclaringEntityType.Model, builder, terminate: false);
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		}
	}

	protected virtual void AddDescription([JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, [JetBrains.Annotations.CanBeNull] string description, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string column = null, bool omitSchemaVariable = false)
	{
		RelationalTypeMapping stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		string o;
		if (schema == null)
		{
			if (!omitSchemaVariable)
			{
				builder.Append("DECLARE @defaultSchema AS sysname").AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
				builder.Append("SET @defaultSchema = SCHEMA_NAME()").AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			o = "@defaultSchema";
		}
		else
		{
			o = Literal(schema);
		}
		builder.Append("EXEC sp_addextendedproperty 'MS_Description', ").Append(Literal(description)).Append(", 'SCHEMA', ")
			.Append(o)
			.Append(", 'TABLE', ")
			.Append(Literal(table));
		if (column != null)
		{
			builder.Append(", 'COLUMN', ").Append(Literal(column));
		}
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		string Literal(string s)
		{
			return stringTypeMapping.GenerateSqlLiteral(s);
		}
	}

	protected virtual void DropDescription([JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string column = null, bool omitSchemaVariable = false)
	{
		RelationalTypeMapping stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		string o;
		if (schema == null)
		{
			if (!omitSchemaVariable)
			{
				builder.Append("DECLARE @defaultSchema AS sysname").AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
				builder.Append("SET @defaultSchema = SCHEMA_NAME()").AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			o = "@defaultSchema";
		}
		else
		{
			o = Literal(schema);
		}
		builder.Append("EXEC sp_dropextendedproperty 'MS_Description', 'SCHEMA', ").Append(o).Append(", 'TABLE', ")
			.Append(Literal(table));
		if (column != null)
		{
			builder.Append(", 'COLUMN', ").Append(Literal(column));
		}
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		string Literal(string s)
		{
			return stringTypeMapping.GenerateSqlLiteral(s);
		}
	}

	protected virtual bool UseLegacyIndexFilters([JetBrains.Annotations.CanBeNull] IModel model)
	{
		if (TryGetVersion(model, out var version))
		{
			return VersionComparer.Compare(version, "2.0.0") < 0;
		}
		return true;
	}

	private string IntegerConstant(long value)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}", value);
	}

	private bool IsMemoryOptimized(Annotatable annotatable, IModel model, string schema, string tableName)
	{
		return (annotatable["SqlServer:MemoryOptimized"] as bool?) ?? FindEntityTypes(model, schema, tableName)?.Any((IEntityType t) => t.IsMemoryOptimized()) ?? false;
	}

	private static bool IsMemoryOptimized(Annotatable annotatable)
	{
		return annotatable["SqlServer:MemoryOptimized"] as bool? == true;
	}

	private static bool IsIdentity(ColumnOperation operation)
	{
		if (operation["SqlServer:Identity"] == null)
		{
			return operation["SqlServer:ValueGenerationStrategy"] as SqlServerValueGenerationStrategy? == SqlServerValueGenerationStrategy.IdentityColumn;
		}
		return true;
	}
}
