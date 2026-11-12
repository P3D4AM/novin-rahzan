using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public class MigrationsSqlGenerator : IMigrationsSqlGenerator
{
	private static readonly IReadOnlyDictionary<Type, Action<MigrationsSqlGenerator, MigrationOperation, IModel, MigrationCommandListBuilder>> _generateActions = new Dictionary<Type, Action<MigrationsSqlGenerator, MigrationOperation, IModel, MigrationCommandListBuilder>>
	{
		{
			typeof(AddColumnOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AddColumnOperation)o, m, b);
			}
		},
		{
			typeof(AddForeignKeyOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AddForeignKeyOperation)o, m, b);
			}
		},
		{
			typeof(AddPrimaryKeyOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AddPrimaryKeyOperation)o, m, b);
			}
		},
		{
			typeof(AddUniqueConstraintOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AddUniqueConstraintOperation)o, m, b);
			}
		},
		{
			typeof(AlterColumnOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AlterColumnOperation)o, m, b);
			}
		},
		{
			typeof(AlterDatabaseOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AlterDatabaseOperation)o, m, b);
			}
		},
		{
			typeof(AlterSequenceOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AlterSequenceOperation)o, m, b);
			}
		},
		{
			typeof(AlterTableOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((AlterTableOperation)o, m, b);
			}
		},
		{
			typeof(CreateCheckConstraintOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((CreateCheckConstraintOperation)o, m, b);
			}
		},
		{
			typeof(CreateIndexOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((CreateIndexOperation)o, m, b);
			}
		},
		{
			typeof(CreateSequenceOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((CreateSequenceOperation)o, m, b);
			}
		},
		{
			typeof(CreateTableOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((CreateTableOperation)o, m, b);
			}
		},
		{
			typeof(DropColumnOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropColumnOperation)o, m, b);
			}
		},
		{
			typeof(DropForeignKeyOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropForeignKeyOperation)o, m, b);
			}
		},
		{
			typeof(DropIndexOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropIndexOperation)o, m, b);
			}
		},
		{
			typeof(DropPrimaryKeyOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropPrimaryKeyOperation)o, m, b);
			}
		},
		{
			typeof(DropSchemaOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropSchemaOperation)o, m, b);
			}
		},
		{
			typeof(DropSequenceOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropSequenceOperation)o, m, b);
			}
		},
		{
			typeof(DropTableOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropTableOperation)o, m, b);
			}
		},
		{
			typeof(DropUniqueConstraintOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropUniqueConstraintOperation)o, m, b);
			}
		},
		{
			typeof(DropCheckConstraintOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DropCheckConstraintOperation)o, m, b);
			}
		},
		{
			typeof(EnsureSchemaOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((EnsureSchemaOperation)o, m, b);
			}
		},
		{
			typeof(RenameColumnOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((RenameColumnOperation)o, m, b);
			}
		},
		{
			typeof(RenameIndexOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((RenameIndexOperation)o, m, b);
			}
		},
		{
			typeof(RenameSequenceOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((RenameSequenceOperation)o, m, b);
			}
		},
		{
			typeof(RenameTableOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((RenameTableOperation)o, m, b);
			}
		},
		{
			typeof(RestartSequenceOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((RestartSequenceOperation)o, m, b);
			}
		},
		{
			typeof(SqlOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((SqlOperation)o, m, b);
			}
		},
		{
			typeof(InsertDataOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((InsertDataOperation)o, m, b);
			}
		},
		{
			typeof(DeleteDataOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((DeleteDataOperation)o, m, b);
			}
		},
		{
			typeof(UpdateDataOperation),
			delegate(MigrationsSqlGenerator g, MigrationOperation o, IModel m, MigrationCommandListBuilder b)
			{
				g.Generate((UpdateDataOperation)o, m, b);
			}
		}
	};

	protected virtual MigrationsSqlGeneratorDependencies Dependencies { get; }

	protected virtual IUpdateSqlGenerator SqlGenerator => Dependencies.UpdateSqlGenerator;

	protected virtual IComparer<string> VersionComparer { get; } = new SemanticVersionComparer();


	public MigrationsSqlGenerator([JetBrains.Annotations.NotNull] MigrationsSqlGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual IReadOnlyList<MigrationCommand> Generate(IReadOnlyList<MigrationOperation> operations, IModel model = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		MigrationCommandListBuilder migrationCommandListBuilder = new MigrationCommandListBuilder(Dependencies);
		foreach (MigrationOperation operation in operations)
		{
			Generate(operation, model, migrationCommandListBuilder);
		}
		return migrationCommandListBuilder.GetCommandList();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] MigrationOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Type type = operation.GetType();
		if (!_generateActions.TryGetValue(type, out var value))
		{
			throw new InvalidOperationException(RelationalStrings.UnknownOperation(GetType().ShortDisplayName(), type));
		}
		value(this, operation, model, builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ADD ");
		ColumnDefinition(operation, model, builder);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddForeignKeyOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ADD ");
		ForeignKeyConstraint(operation, model, builder);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddPrimaryKeyOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ADD ");
		PrimaryKeyConstraint(operation, model, builder);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AddUniqueConstraintOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ADD ");
		UniqueConstraint(operation, model, builder);
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateCheckConstraintOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ADD ");
		CheckConstraint(operation, model, builder);
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		throw new NotImplementedException();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterDatabaseOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameIndexOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		throw new NotImplementedException();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER SEQUENCE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema));
		SequenceOptions(operation, model, builder);
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] AlterTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		throw new NotImplementedException();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateIndexOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("CREATE ");
		if (operation.IsUnique)
		{
			builder.Append("UNIQUE ");
		}
		IndexTraits(operation, model, builder);
		builder.Append("INDEX ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" ON ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema))
			.Append(" (")
			.Append(ColumnList(operation.Columns))
			.Append(")");
		IndexOptions(operation, model, builder);
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] EnsureSchemaOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		throw new NotImplementedException();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("CREATE SEQUENCE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema));
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(operation.ClrType);
		if (operation.ClrType != typeof(long))
		{
			builder.Append(" AS ").Append(mapping.StoreType);
			mapping = Dependencies.TypeMappingSource.GetMapping(typeof(long));
		}
		builder.Append(" START WITH ").Append(mapping.GenerateSqlLiteral(operation.StartValue));
		SequenceOptions(operation, model, builder);
		builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("CREATE TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema)).AppendLine(" (");
		using (builder.Indent())
		{
			CreateTableColumns(operation, model, builder);
			CreateTableConstraints(operation, model, builder);
			builder.AppendLine();
		}
		builder.Append(")");
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" DROP COLUMN ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropForeignKeyOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" DROP CONSTRAINT ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropIndexOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		throw new NotImplementedException();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropPrimaryKeyOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" DROP CONSTRAINT ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropSchemaOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("DROP SCHEMA ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("DROP SEQUENCE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema)).AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("DROP TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema));
		if (terminate)
		{
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropUniqueConstraintOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" DROP CONSTRAINT ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
			.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DropCheckConstraintOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" DROP CONSTRAINT ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
			.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		throw new NotImplementedException();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RenameSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		throw new NotImplementedException();
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] RestartSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(long));
		builder.Append("ALTER SEQUENCE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema)).Append(" RESTART WITH ")
			.Append(mapping.GenerateSqlLiteral(operation.StartValue))
			.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] SqlOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.AppendLine(operation.Sql);
		EndStatement(builder, operation.SuppressTransaction);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] InsertDataOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ModificationCommand item in operation.GenerateModificationCommands(model))
		{
			SqlGenerator.AppendInsertOperation(stringBuilder, item, 0);
		}
		builder.Append(stringBuilder.ToString());
		if (terminate)
		{
			EndStatement(builder);
		}
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] DeleteDataOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ModificationCommand item in operation.GenerateModificationCommands(model))
		{
			SqlGenerator.AppendDeleteOperation(stringBuilder, item, 0);
		}
		builder.Append(stringBuilder.ToString());
		EndStatement(builder);
	}

	protected virtual void Generate([JetBrains.Annotations.NotNull] UpdateDataOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ModificationCommand item in operation.GenerateModificationCommands(model))
		{
			SqlGenerator.AppendUpdateOperation(stringBuilder, item, 0);
		}
		builder.Append(stringBuilder.ToString());
		EndStatement(builder);
	}

	protected virtual void SequenceOptions([JetBrains.Annotations.NotNull] AlterSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		SequenceOptions(operation.Schema, operation.Name, operation, model, builder);
	}

	protected virtual void SequenceOptions([JetBrains.Annotations.NotNull] CreateSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		SequenceOptions(operation.Schema, operation.Name, operation, model, builder);
	}

	protected virtual void SequenceOptions([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] SequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(int));
		RelationalTypeMapping mapping2 = Dependencies.TypeMappingSource.GetMapping(typeof(long));
		builder.Append(" INCREMENT BY ").Append(mapping.GenerateSqlLiteral(operation.IncrementBy));
		if (operation.MinValue.HasValue)
		{
			builder.Append(" MINVALUE ").Append(mapping2.GenerateSqlLiteral(operation.MinValue));
		}
		else
		{
			builder.Append(" NO MINVALUE");
		}
		if (operation.MaxValue.HasValue)
		{
			builder.Append(" MAXVALUE ").Append(mapping2.GenerateSqlLiteral(operation.MaxValue));
		}
		else
		{
			builder.Append(" NO MAXVALUE");
		}
		builder.Append(operation.IsCyclic ? " CYCLE" : " NO CYCLE");
	}

	protected virtual void CreateTableColumns([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		for (int i = 0; i < operation.Columns.Count; i++)
		{
			AddColumnOperation operation2 = operation.Columns[i];
			ColumnDefinition(operation2, model, builder);
			if (i != operation.Columns.Count - 1)
			{
				builder.AppendLine(",");
			}
		}
	}

	protected virtual void ColumnDefinition([JetBrains.Annotations.NotNull] AddColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		ColumnDefinition(operation.Schema, operation.Table, operation.Name, operation, model, builder);
	}

	protected virtual void ColumnDefinition([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] ColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (operation.ComputedColumnSql != null)
		{
			ComputedColumnDefinition(schema, table, name, operation, model, builder);
			return;
		}
		string text = operation.ColumnType ?? GetColumnType(schema, table, name, operation, model);
		builder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(name)).Append(" ").Append(text);
		builder.Append(operation.IsNullable ? " NULL" : " NOT NULL");
		DefaultValue(operation.DefaultValue, operation.DefaultValueSql, text, builder);
	}

	protected virtual void ComputedColumnDefinition([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] ColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		throw new NotImplementedException();
	}

	protected virtual string GetColumnType([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] ColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		bool keyOrIndex = false;
		IProperty property = FindProperty(model, schema, table, name);
		if (property != null)
		{
			if (operation.IsUnicode == property.IsUnicode() && operation.MaxLength == property.GetMaxLength() && operation.IsFixedLength.GetValueOrDefault() == property.IsFixedLength() && operation.IsRowVersion == (property.IsConcurrencyToken && property.ValueGenerated == ValueGenerated.OnAddOrUpdate))
			{
				return Dependencies.TypeMappingSource.FindMapping(property).StoreType;
			}
			keyOrIndex = property.IsKey() || property.IsForeignKey();
		}
		return Dependencies.TypeMappingSource.FindMapping(operation.ClrType, null, keyOrIndex, operation.IsUnicode, operation.MaxLength, operation.IsRowVersion, operation.IsFixedLength).StoreType;
	}

	protected virtual void DefaultValue([JetBrains.Annotations.CanBeNull] object defaultValue, [JetBrains.Annotations.CanBeNull] string defaultValueSql, [JetBrains.Annotations.CanBeNull] string columnType, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (defaultValueSql != null)
		{
			builder.Append(" DEFAULT (").Append(defaultValueSql).Append(")");
		}
		else if (defaultValue != null)
		{
			RelationalTypeMapping relationalTypeMapping = ((columnType != null) ? Dependencies.TypeMappingSource.FindMapping(defaultValue.GetType(), columnType) : null);
			if (relationalTypeMapping == null)
			{
				relationalTypeMapping = Dependencies.TypeMappingSource.GetMappingForValue(defaultValue);
			}
			builder.Append(" DEFAULT ").Append(relationalTypeMapping.GenerateSqlLiteral(defaultValue));
		}
	}

	protected virtual void CreateTableConstraints([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		CreateTablePrimaryKeyConstraint(operation, model, builder);
		CreateTableUniqueConstraints(operation, model, builder);
		CreateTableCheckConstraints(operation, model, builder);
		CreateTableForeignKeys(operation, model, builder);
	}

	protected virtual void CreateTableForeignKeys([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		foreach (AddForeignKeyOperation foreignKey in operation.ForeignKeys)
		{
			builder.AppendLine(",");
			ForeignKeyConstraint(foreignKey, model, builder);
		}
	}

	protected virtual void ForeignKeyConstraint([JetBrains.Annotations.NotNull] AddForeignKeyOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (operation.Name != null)
		{
			builder.Append("CONSTRAINT ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" ");
		}
		builder.Append("FOREIGN KEY (").Append(ColumnList(operation.Columns)).Append(") REFERENCES ")
			.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.PrincipalTable, operation.PrincipalSchema));
		if (operation.PrincipalColumns != null)
		{
			builder.Append(" (").Append(ColumnList(operation.PrincipalColumns)).Append(")");
		}
		if (operation.OnUpdate != 0)
		{
			builder.Append(" ON UPDATE ");
			ForeignKeyAction(operation.OnUpdate, builder);
		}
		if (operation.OnDelete != 0)
		{
			builder.Append(" ON DELETE ");
			ForeignKeyAction(operation.OnDelete, builder);
		}
	}

	protected virtual void CreateTablePrimaryKeyConstraint([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (operation.PrimaryKey != null)
		{
			builder.AppendLine(",");
			PrimaryKeyConstraint(operation.PrimaryKey, model, builder);
		}
	}

	protected virtual void PrimaryKeyConstraint([JetBrains.Annotations.NotNull] AddPrimaryKeyOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (operation.Name != null)
		{
			builder.Append("CONSTRAINT ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" ");
		}
		builder.Append("PRIMARY KEY ");
		IndexTraits(operation, model, builder);
		builder.Append("(").Append(ColumnList(operation.Columns)).Append(")");
	}

	protected virtual void CreateTableUniqueConstraints([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		foreach (AddUniqueConstraintOperation uniqueConstraint in operation.UniqueConstraints)
		{
			builder.AppendLine(",");
			UniqueConstraint(uniqueConstraint, model, builder);
		}
	}

	protected virtual void UniqueConstraint([JetBrains.Annotations.NotNull] AddUniqueConstraintOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (operation.Name != null)
		{
			builder.Append("CONSTRAINT ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" ");
		}
		builder.Append("UNIQUE ");
		IndexTraits(operation, model, builder);
		builder.Append("(").Append(ColumnList(operation.Columns)).Append(")");
	}

	protected virtual void CreateTableCheckConstraints([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		foreach (CreateCheckConstraintOperation checkConstraint in operation.CheckConstraints)
		{
			builder.AppendLine(",");
			CheckConstraint(checkConstraint, model, builder);
		}
	}

	protected virtual void CheckConstraint([JetBrains.Annotations.NotNull] CreateCheckConstraintOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		if (operation.Name != null)
		{
			builder.Append("CONSTRAINT ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" ");
		}
		builder.Append("CHECK ");
		builder.Append("(").Append(operation.Sql).Append(")");
	}

	protected virtual void IndexTraits([JetBrains.Annotations.NotNull] MigrationOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
	}

	protected virtual void IndexOptions([JetBrains.Annotations.NotNull] CreateIndexOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		if (!string.IsNullOrEmpty(operation.Filter))
		{
			builder.Append(" WHERE ").Append(operation.Filter);
		}
	}

	protected virtual void ForeignKeyAction(ReferentialAction referentialAction, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		switch (referentialAction)
		{
		case ReferentialAction.Restrict:
			builder.Append("RESTRICT");
			break;
		case ReferentialAction.Cascade:
			builder.Append("CASCADE");
			break;
		case ReferentialAction.SetNull:
			builder.Append("SET NULL");
			break;
		case ReferentialAction.SetDefault:
			builder.Append("SET DEFAULT");
			break;
		}
	}

	protected virtual IEnumerable<IEntityType> FindEntityTypes([JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string tableName)
	{
		return (from t in model?.GetEntityTypes()
			where t.GetTableName() == tableName && t.GetSchema() == schema && !t.IsIgnoredByMigrations()
			select t);
	}

	protected virtual IProperty FindProperty([JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string columnName)
	{
		return FindEntityTypes(model, schema, tableName)?.SelectMany((IEntityType e) => e.GetDeclaredProperties()).FirstOrDefault((IProperty p) => p.GetColumnName() == columnName);
	}

	protected virtual void EndStatement([JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool suppressTransaction = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		builder.EndCommand(suppressTransaction);
	}

	protected virtual string ColumnList([JetBrains.Annotations.NotNull] string[] columns)
	{
		return string.Join(", ", columns.Select(Dependencies.SqlGenerationHelper.DelimitIdentifier));
	}

	protected virtual bool IsOldColumnSupported([JetBrains.Annotations.CanBeNull] IModel model)
	{
		if (TryGetVersion(model, out var version))
		{
			return VersionComparer.Compare(version, "1.1.0") >= 0;
		}
		return false;
	}

	protected virtual bool HasLegacyRenameOperations([JetBrains.Annotations.CanBeNull] IModel model)
	{
		if (TryGetVersion(model, out var version))
		{
			return VersionComparer.Compare(version, "2.1.0") < 0;
		}
		return true;
	}

	protected virtual bool TryGetVersion([JetBrains.Annotations.CanBeNull] IModel model, out string version)
	{
		if (!(model?["ProductVersion"] is string text))
		{
			version = null;
			return false;
		}
		version = text;
		return true;
	}
}
