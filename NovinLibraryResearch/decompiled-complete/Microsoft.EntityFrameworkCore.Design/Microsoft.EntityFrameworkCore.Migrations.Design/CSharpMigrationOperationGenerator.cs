using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public class CSharpMigrationOperationGenerator : ICSharpMigrationOperationGenerator
{
	protected virtual CSharpMigrationOperationGeneratorDependencies Dependencies { get; }

	private ICSharpHelper Code => Dependencies.CSharpHelper;

	public CSharpMigrationOperationGenerator([JetBrains.Annotations.NotNull] CSharpMigrationOperationGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual void Generate(string builderName, IReadOnlyList<MigrationOperation> operations, IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(builderName, "builderName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		bool flag = true;
		foreach (MigrationOperation operation in operations)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				builder.AppendLine().AppendLine();
			}
			builder.Append(builderName);
			Generate((dynamic)operation, builder);
			builder.Append(";");
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] MigrationOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		throw new InvalidOperationException(DesignStrings.UnknownOperation(operation.GetType()));
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddColumnOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append(".AddColumn<").Append(Code.Reference(operation.ClrType)).AppendLine(">(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table));
			if (operation.ColumnType != null)
			{
				builder.AppendLine(",").Append("type: ").Append(Code.Literal(operation.ColumnType));
			}
			if (operation.IsUnicode == false)
			{
				builder.AppendLine(",").Append("unicode: false");
			}
			if (operation.IsFixedLength == true)
			{
				builder.AppendLine(",").Append("fixedLength: true");
			}
			if (operation.MaxLength.HasValue)
			{
				builder.AppendLine(",").Append("maxLength: ").Append(Code.Literal(operation.MaxLength.Value));
			}
			if (operation.IsRowVersion)
			{
				builder.AppendLine(",").Append("rowVersion: true");
			}
			builder.AppendLine(",").Append("nullable: ").Append(Code.Literal(operation.IsNullable));
			if (operation.DefaultValueSql != null)
			{
				builder.AppendLine(",").Append("defaultValueSql: ").Append(Code.Literal(operation.DefaultValueSql));
			}
			else if (operation.ComputedColumnSql != null)
			{
				builder.AppendLine(",").Append("computedColumnSql: ").Append(Code.Literal(operation.ComputedColumnSql));
			}
			else if (operation.DefaultValue != null)
			{
				builder.AppendLine(",").Append("defaultValue: ").Append(Code.UnknownLiteral(operation.DefaultValue));
			}
			if (operation.Comment != null)
			{
				builder.AppendLine(",").Append("comment: ").Append(Code.Literal(operation.Comment));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddForeignKeyOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".AddForeignKey(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.AppendLine(",");
			if (operation.Columns.Length == 1)
			{
				builder.Append("column: ").Append(Code.Literal(operation.Columns[0]));
			}
			else
			{
				builder.Append("columns: ").Append(Code.Literal(operation.Columns));
			}
			if (operation.PrincipalSchema != null)
			{
				builder.AppendLine(",").Append("principalSchema: ").Append(Code.Literal(operation.PrincipalSchema));
			}
			builder.AppendLine(",").Append("principalTable: ").Append(Code.Literal(operation.PrincipalTable))
				.AppendLine(",");
			if (operation.PrincipalColumns.Length == 1)
			{
				builder.Append("principalColumn: ").Append(Code.Literal(operation.PrincipalColumns[0]));
			}
			else
			{
				builder.Append("principalColumns: ").Append(Code.Literal(operation.PrincipalColumns));
			}
			if (operation.OnUpdate != 0)
			{
				builder.AppendLine(",").Append("onUpdate: ").Append(Code.Literal(operation.OnUpdate));
			}
			if (operation.OnDelete != 0)
			{
				builder.AppendLine(",").Append("onDelete: ").Append(Code.Literal(operation.OnDelete));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddPrimaryKeyOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".AddPrimaryKey(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.AppendLine(",");
			if (operation.Columns.Length == 1)
			{
				builder.Append("column: ").Append(Code.Literal(operation.Columns[0]));
			}
			else
			{
				builder.Append("columns: ").Append(Code.Literal(operation.Columns));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddUniqueConstraintOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".AddUniqueConstraint(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.AppendLine(",");
			if (operation.Columns.Length == 1)
			{
				builder.Append("column: ").Append(Code.Literal(operation.Columns[0]));
			}
			else
			{
				builder.Append("columns: ").Append(Code.Literal(operation.Columns));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateCheckConstraintOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".CreateCheckConstraint(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.AppendLine(",")
				.Append("sql: ")
				.Append(Code.Literal(operation.Sql))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterColumnOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append(".AlterColumn<").Append(Code.Reference(operation.ClrType)).AppendLine(">(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table));
			if (operation.ColumnType != null)
			{
				builder.AppendLine(",").Append("type: ").Append(Code.Literal(operation.ColumnType));
			}
			if (operation.IsUnicode == false)
			{
				builder.AppendLine(",").Append("unicode: false");
			}
			if (operation.IsFixedLength == true)
			{
				builder.AppendLine(",").Append("fixedLength: true");
			}
			if (operation.MaxLength.HasValue)
			{
				builder.AppendLine(",").Append("maxLength: ").Append(Code.Literal(operation.MaxLength.Value));
			}
			if (operation.IsRowVersion)
			{
				builder.AppendLine(",").Append("rowVersion: true");
			}
			builder.AppendLine(",").Append("nullable: ").Append(Code.Literal(operation.IsNullable));
			if (operation.DefaultValueSql != null)
			{
				builder.AppendLine(",").Append("defaultValueSql: ").Append(Code.Literal(operation.DefaultValueSql));
			}
			else if (operation.ComputedColumnSql != null)
			{
				builder.AppendLine(",").Append("computedColumnSql: ").Append(Code.Literal(operation.ComputedColumnSql));
			}
			else if (operation.DefaultValue != null)
			{
				builder.AppendLine(",").Append("defaultValue: ").Append(Code.UnknownLiteral(operation.DefaultValue));
			}
			if (operation.Comment != null)
			{
				builder.AppendLine(",").Append("comment: ").Append(Code.Literal(operation.Comment));
			}
			if (operation.OldColumn.ClrType != null)
			{
				builder.AppendLine(",").Append("oldClrType: typeof(").Append(Code.Reference(operation.OldColumn.ClrType))
					.Append(")");
			}
			if (operation.OldColumn.ColumnType != null)
			{
				builder.AppendLine(",").Append("oldType: ").Append(Code.Literal(operation.OldColumn.ColumnType));
			}
			if (operation.OldColumn.IsUnicode == false)
			{
				builder.AppendLine(",").Append("oldUnicode: false");
			}
			if (operation.OldColumn.IsFixedLength == true)
			{
				builder.AppendLine(",").Append("oldFixedLength: true");
			}
			if (operation.OldColumn.MaxLength.HasValue)
			{
				builder.AppendLine(",").Append("oldMaxLength: ").Append(Code.Literal(operation.OldColumn.MaxLength.Value));
			}
			if (operation.OldColumn.IsRowVersion)
			{
				builder.AppendLine(",").Append("oldRowVersion: true");
			}
			if (operation.OldColumn.IsNullable)
			{
				builder.AppendLine(",").Append("oldNullable: true");
			}
			if (operation.OldColumn.DefaultValueSql != null)
			{
				builder.AppendLine(",").Append("oldDefaultValueSql: ").Append(Code.Literal(operation.OldColumn.DefaultValueSql));
			}
			else if (operation.OldColumn.ComputedColumnSql != null)
			{
				builder.AppendLine(",").Append("oldComputedColumnSql: ").Append(Code.Literal(operation.OldColumn.ComputedColumnSql));
			}
			else if (operation.OldColumn.DefaultValue != null)
			{
				builder.AppendLine(",").Append("oldDefaultValue: ").Append(Code.UnknownLiteral(operation.OldColumn.DefaultValue));
			}
			if (operation.OldColumn.Comment != null)
			{
				builder.AppendLine(",").Append("oldComment: ").Append(Code.Literal(operation.OldColumn.Comment));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
			OldAnnotations(operation.OldColumn.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterDatabaseOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append(".AlterDatabase()");
		using (builder.Indent())
		{
			Annotations(operation.GetAnnotations(), builder);
			OldAnnotations(operation.OldDatabase.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterSequenceOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".AlterSequence(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			if (operation.IncrementBy != 1)
			{
				builder.AppendLine(",").Append("incrementBy: ").Append(Code.Literal(operation.IncrementBy));
			}
			if (operation.MinValue.HasValue)
			{
				builder.AppendLine(",").Append("minValue: ").Append(Code.Literal(operation.MinValue));
			}
			if (operation.MaxValue.HasValue)
			{
				builder.AppendLine(",").Append("maxValue: ").Append(Code.Literal(operation.MaxValue));
			}
			if (operation.IsCyclic)
			{
				builder.AppendLine(",").Append("cyclic: true");
			}
			if (operation.OldSequence.IncrementBy != 1)
			{
				builder.AppendLine(",").Append("oldIncrementBy: ").Append(Code.Literal(operation.OldSequence.IncrementBy));
			}
			if (operation.OldSequence.MinValue.HasValue)
			{
				builder.AppendLine(",").Append("oldMinValue: ").Append(Code.Literal(operation.OldSequence.MinValue));
			}
			if (operation.OldSequence.MaxValue.HasValue)
			{
				builder.AppendLine(",").Append("oldMaxValue: ").Append(Code.Literal(operation.OldSequence.MaxValue));
			}
			if (operation.OldSequence.IsCyclic)
			{
				builder.AppendLine(",").Append("oldCyclic: true");
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
			OldAnnotations(operation.OldSequence.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterTableOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".AlterTable(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			if (operation.Comment != null)
			{
				builder.AppendLine(",").Append("comment: ").Append(Code.Literal(operation.Comment));
			}
			if (operation.OldTable.Comment != null)
			{
				builder.AppendLine(",").Append("oldComment: ").Append(Code.Literal(operation.OldTable.Comment));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
			OldAnnotations(operation.OldTable.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateIndexOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".CreateIndex(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.AppendLine(",");
			if (operation.Columns.Length == 1)
			{
				builder.Append("column: ").Append(Code.Literal(operation.Columns[0]));
			}
			else
			{
				builder.Append("columns: ").Append(Code.Literal(operation.Columns));
			}
			if (operation.IsUnique)
			{
				builder.AppendLine(",").Append("unique: true");
			}
			if (operation.Filter != null)
			{
				builder.AppendLine(",").Append("filter: ").Append(Code.Literal(operation.Filter));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] EnsureSchemaOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".EnsureSchema(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name)).Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateSequenceOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append(".CreateSequence");
		if (operation.ClrType != typeof(long))
		{
			builder.Append("<").Append(Code.Reference(operation.ClrType)).Append(">");
		}
		builder.AppendLine("(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			if (operation.StartValue != 1)
			{
				builder.AppendLine(",").Append("startValue: ").Append(Code.Literal(operation.StartValue));
			}
			if (operation.IncrementBy != 1)
			{
				builder.AppendLine(",").Append("incrementBy: ").Append(Code.Literal(operation.IncrementBy));
			}
			if (operation.MinValue.HasValue)
			{
				builder.AppendLine(",").Append("minValue: ").Append(Code.Literal(operation.MinValue.Value));
			}
			if (operation.MaxValue.HasValue)
			{
				builder.AppendLine(",").Append("maxValue: ").Append(Code.Literal(operation.MaxValue.Value));
			}
			if (operation.IsCyclic)
			{
				builder.AppendLine(",").Append("cyclic: true");
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".CreateTable(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").AppendLine("columns: table => new").AppendLine("{");
			Dictionary<string, string> map = new Dictionary<string, string>();
			using (builder.Indent())
			{
				List<string> scope = new List<string>();
				for (int i = 0; i < operation.Columns.Count; i++)
				{
					AddColumnOperation addColumnOperation = operation.Columns[i];
					string text = Code.Identifier(addColumnOperation.Name, scope);
					map.Add(addColumnOperation.Name, text);
					builder.Append(text).Append(" = table.Column<").Append(Code.Reference(addColumnOperation.ClrType))
						.Append(">(");
					if (text != addColumnOperation.Name)
					{
						builder.Append("name: ").Append(Code.Literal(addColumnOperation.Name)).Append(", ");
					}
					if (addColumnOperation.ColumnType != null)
					{
						builder.Append("type: ").Append(Code.Literal(addColumnOperation.ColumnType)).Append(", ");
					}
					if (addColumnOperation.IsUnicode == false)
					{
						builder.Append("unicode: false, ");
					}
					if (addColumnOperation.IsFixedLength == true)
					{
						builder.Append("fixedLength: true, ");
					}
					if (addColumnOperation.MaxLength.HasValue)
					{
						builder.Append("maxLength: ").Append(Code.Literal(addColumnOperation.MaxLength.Value)).Append(", ");
					}
					if (addColumnOperation.IsRowVersion)
					{
						builder.Append("rowVersion: true, ");
					}
					builder.Append("nullable: ").Append(Code.Literal(addColumnOperation.IsNullable));
					if (addColumnOperation.DefaultValueSql != null)
					{
						builder.Append(", defaultValueSql: ").Append(Code.Literal(addColumnOperation.DefaultValueSql));
					}
					else if (addColumnOperation.ComputedColumnSql != null)
					{
						builder.Append(", computedColumnSql: ").Append(Code.Literal(addColumnOperation.ComputedColumnSql));
					}
					else if (addColumnOperation.DefaultValue != null)
					{
						builder.Append(", defaultValue: ").Append(Code.UnknownLiteral(addColumnOperation.DefaultValue));
					}
					if (addColumnOperation.Comment != null)
					{
						builder.Append(", comment: ").Append(Code.Literal(addColumnOperation.Comment));
					}
					builder.Append(")");
					using (builder.Indent())
					{
						Annotations(addColumnOperation.GetAnnotations(), builder);
					}
					if (i != operation.Columns.Count - 1)
					{
						builder.Append(",");
					}
					builder.AppendLine();
				}
			}
			builder.AppendLine("},").AppendLine("constraints: table =>").AppendLine("{");
			using (builder.Indent())
			{
				if (operation.PrimaryKey != null)
				{
					builder.Append("table.PrimaryKey(").Append(Code.Literal(operation.PrimaryKey.Name)).Append(", ")
						.Append(Code.Lambda(operation.PrimaryKey.Columns.Select((string c) => map[c]).ToList()))
						.Append(")");
					using (builder.Indent())
					{
						Annotations(operation.PrimaryKey.GetAnnotations(), builder);
					}
					builder.AppendLine(";");
				}
				foreach (AddUniqueConstraintOperation uniqueConstraint in operation.UniqueConstraints)
				{
					builder.Append("table.UniqueConstraint(").Append(Code.Literal(uniqueConstraint.Name)).Append(", ")
						.Append(Code.Lambda(uniqueConstraint.Columns.Select((string c) => map[c]).ToList()))
						.Append(")");
					using (builder.Indent())
					{
						Annotations(uniqueConstraint.GetAnnotations(), builder);
					}
					builder.AppendLine(";");
				}
				foreach (CreateCheckConstraintOperation checkConstraint in operation.CheckConstraints)
				{
					builder.Append("table.CheckConstraint(").Append(Code.Literal(checkConstraint.Name)).Append(", ")
						.Append(Code.Literal(checkConstraint.Sql))
						.Append(")");
					using (builder.Indent())
					{
						Annotations(checkConstraint.GetAnnotations(), builder);
					}
					builder.AppendLine(";");
				}
				foreach (AddForeignKeyOperation foreignKey in operation.ForeignKeys)
				{
					builder.AppendLine("table.ForeignKey(");
					using (builder.Indent())
					{
						builder.Append("name: ").Append(Code.Literal(foreignKey.Name)).AppendLine(",")
							.Append((foreignKey.Columns.Length == 1) ? "column: " : "columns: ")
							.Append(Code.Lambda(foreignKey.Columns.Select((string c) => map[c]).ToList()));
						if (foreignKey.PrincipalSchema != null)
						{
							builder.AppendLine(",").Append("principalSchema: ").Append(Code.Literal(foreignKey.PrincipalSchema));
						}
						builder.AppendLine(",").Append("principalTable: ").Append(Code.Literal(foreignKey.PrincipalTable))
							.AppendLine(",");
						if (foreignKey.PrincipalColumns.Length == 1)
						{
							builder.Append("principalColumn: ").Append(Code.Literal(foreignKey.PrincipalColumns[0]));
						}
						else
						{
							builder.Append("principalColumns: ").Append(Code.Literal(foreignKey.PrincipalColumns));
						}
						if (foreignKey.OnUpdate != 0)
						{
							builder.AppendLine(",").Append("onUpdate: ").Append(Code.Literal(foreignKey.OnUpdate));
						}
						if (foreignKey.OnDelete != 0)
						{
							builder.AppendLine(",").Append("onDelete: ").Append(Code.Literal(foreignKey.OnDelete));
						}
						builder.Append(")");
						Annotations(foreignKey.GetAnnotations(), builder);
					}
					builder.AppendLine(";");
				}
			}
			builder.Append("}");
			if (operation.Comment != null)
			{
				builder.AppendLine(",").Append("comment: ").Append(Code.Literal(operation.Comment));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropColumnOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropColumn(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropForeignKeyOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropForeignKey(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropIndexOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropIndex(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropPrimaryKeyOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropPrimaryKey(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropSchemaOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropSchema(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name)).Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropSequenceOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropSequence(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropTableOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropTable(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropUniqueConstraintOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropUniqueConstraint(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropCheckConstraintOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DropCheckConstraint(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameColumnOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".RenameColumn(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.AppendLine(",")
				.Append("newName: ")
				.Append(Code.Literal(operation.NewName))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameIndexOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".RenameIndex(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("table: ").Append(Code.Literal(operation.Table))
				.AppendLine(",")
				.Append("newName: ")
				.Append(Code.Literal(operation.NewName))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameSequenceOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".RenameSequence(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			if (operation.NewName != null)
			{
				builder.AppendLine(",").Append("newName: ").Append(Code.Literal(operation.NewName));
			}
			if (operation.NewSchema != null)
			{
				builder.AppendLine(",").Append("newSchema: ").Append(Code.Literal(operation.NewSchema));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameTableOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".RenameTable(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			if (operation.NewName != null)
			{
				builder.AppendLine(",").Append("newName: ").Append(Code.Literal(operation.NewName));
			}
			if (operation.NewSchema != null)
			{
				builder.AppendLine(",").Append("newSchema: ").Append(Code.Literal(operation.NewSchema));
			}
			builder.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RestartSequenceOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".RestartSequence(");
		using (builder.Indent())
		{
			builder.Append("name: ").Append(Code.Literal(operation.Name));
			if (operation.Schema != null)
			{
				builder.AppendLine(",").Append("schema: ").Append(Code.Literal(operation.Schema));
			}
			builder.AppendLine(",").Append("startValue: ").Append(Code.Literal(operation.StartValue))
				.Append(")");
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] SqlOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append(".Sql(").Append(Code.Literal(operation.Sql)).Append(")");
		using (builder.Indent())
		{
			Annotations(operation.GetAnnotations(), builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] InsertDataOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".InsertData(");
		using (builder.Indent())
		{
			if (operation.Schema != null)
			{
				builder.Append("schema: ").Append(Code.Literal(operation.Schema)).AppendLine(",");
			}
			builder.Append("table: ").Append(Code.Literal(operation.Table)).AppendLine(",");
			if (operation.Columns.Length == 1)
			{
				builder.Append("column: ").Append(Code.Literal(operation.Columns[0]));
			}
			else
			{
				builder.Append("columns: ").Append(Code.Literal(operation.Columns));
			}
			builder.AppendLine(",");
			if (operation.Values.GetLength(0) == 1 && operation.Values.GetLength(1) == 1)
			{
				builder.Append("value: ").Append(Code.UnknownLiteral(operation.Values[0, 0]));
			}
			else if (operation.Values.GetLength(0) == 1)
			{
				builder.Append("values: ").Append(Code.Literal(ToOnedimensionalArray(operation.Values)));
			}
			else if (operation.Values.GetLength(1) == 1)
			{
				builder.Append("values: ").AppendLines(Code.Literal(ToOnedimensionalArray(operation.Values, firstDimension: true), vertical: true), skipFinalNewline: true);
			}
			else
			{
				builder.Append("values: ").AppendLines(Code.Literal(operation.Values), skipFinalNewline: true);
			}
			builder.Append(")");
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DeleteDataOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".DeleteData(");
		using (builder.Indent())
		{
			if (operation.Schema != null)
			{
				builder.Append("schema: ").Append(Code.Literal(operation.Schema)).AppendLine(",");
			}
			builder.Append("table: ").Append(Code.Literal(operation.Table)).AppendLine(",");
			if (operation.KeyColumns.Length == 1)
			{
				builder.Append("keyColumn: ").Append(Code.Literal(operation.KeyColumns[0]));
			}
			else
			{
				builder.Append("keyColumns: ").Append(Code.Literal(operation.KeyColumns));
			}
			builder.AppendLine(",");
			if (operation.KeyValues.GetLength(0) == 1 && operation.KeyValues.GetLength(1) == 1)
			{
				builder.Append("keyValue: ").Append(Code.UnknownLiteral(operation.KeyValues[0, 0]));
			}
			else if (operation.KeyValues.GetLength(0) == 1)
			{
				builder.Append("keyValues: ").Append(Code.Literal(ToOnedimensionalArray(operation.KeyValues)));
			}
			else if (operation.KeyValues.GetLength(1) == 1)
			{
				builder.Append("keyValues: ").AppendLines(Code.Literal(ToOnedimensionalArray(operation.KeyValues, firstDimension: true), vertical: true), skipFinalNewline: true);
			}
			else
			{
				builder.Append("keyValues: ").AppendLines(Code.Literal(operation.KeyValues), skipFinalNewline: true);
			}
			builder.Append(")");
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] UpdateDataOperation operation, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(".UpdateData(");
		using (builder.Indent())
		{
			if (operation.Schema != null)
			{
				builder.Append("schema: ").Append(Code.Literal(operation.Schema)).AppendLine(",");
			}
			builder.Append("table: ").Append(Code.Literal(operation.Table)).AppendLine(",");
			if (operation.KeyColumns.Length == 1)
			{
				builder.Append("keyColumn: ").Append(Code.Literal(operation.KeyColumns[0]));
			}
			else
			{
				builder.Append("keyColumns: ").Append(Code.Literal(operation.KeyColumns));
			}
			builder.AppendLine(",");
			if (operation.KeyValues.GetLength(0) == 1 && operation.KeyValues.GetLength(1) == 1)
			{
				builder.Append("keyValue: ").Append(Code.UnknownLiteral(operation.KeyValues[0, 0]));
			}
			else if (operation.KeyValues.GetLength(0) == 1)
			{
				builder.Append("keyValues: ").Append(Code.Literal(ToOnedimensionalArray(operation.KeyValues)));
			}
			else if (operation.KeyValues.GetLength(1) == 1)
			{
				builder.Append("keyValues: ").AppendLines(Code.Literal(ToOnedimensionalArray(operation.KeyValues, firstDimension: true), vertical: true), skipFinalNewline: true);
			}
			else
			{
				builder.Append("keyValues: ").AppendLines(Code.Literal(operation.KeyValues), skipFinalNewline: true);
			}
			builder.AppendLine(",");
			if (operation.Columns.Length == 1)
			{
				builder.Append("column: ").Append(Code.Literal(operation.Columns[0]));
			}
			else
			{
				builder.Append("columns: ").Append(Code.Literal(operation.Columns));
			}
			builder.AppendLine(",");
			if (operation.Values.GetLength(0) == 1 && operation.Values.GetLength(1) == 1)
			{
				builder.Append("value: ").Append(Code.UnknownLiteral(operation.Values[0, 0]));
			}
			else if (operation.Values.GetLength(0) == 1)
			{
				builder.Append("values: ").Append(Code.Literal(ToOnedimensionalArray(operation.Values)));
			}
			else if (operation.Values.GetLength(1) == 1)
			{
				builder.Append("values: ").AppendLines(Code.Literal(ToOnedimensionalArray(operation.Values, firstDimension: true), vertical: true), skipFinalNewline: true);
			}
			else
			{
				builder.Append("values: ").AppendLines(Code.Literal(operation.Values), skipFinalNewline: true);
			}
			builder.Append(")");
		}
	}

	protected virtual void Annotations([JetBrains.Annotations.NotNull] IEnumerable<Annotation> annotations, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotations, "annotations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		foreach (Annotation annotation in annotations)
		{
			if (annotation.Value != null)
			{
				builder.AppendLine().Append(".Annotation(").Append(Code.Literal(annotation.Name))
					.Append(", ")
					.Append(Code.UnknownLiteral(annotation.Value))
					.Append(")");
			}
		}
	}

	protected virtual void OldAnnotations([JetBrains.Annotations.NotNull] IEnumerable<Annotation> annotations, [JetBrains.Annotations.NotNull] IndentedStringBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotations, "annotations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		foreach (Annotation annotation in annotations)
		{
			builder.AppendLine().Append(".OldAnnotation(").Append(Code.Literal(annotation.Name))
				.Append(", ")
				.Append(Code.UnknownLiteral(annotation.Value))
				.Append(")");
		}
	}

	private static object[] ToOnedimensionalArray(object[,] values, bool firstDimension = false)
	{
		object[] array = new object[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = (firstDimension ? values[i, 0] : values[0, i]);
		}
		return array;
	}
}
