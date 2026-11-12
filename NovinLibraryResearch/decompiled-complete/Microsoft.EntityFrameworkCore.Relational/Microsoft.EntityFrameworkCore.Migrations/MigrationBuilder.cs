using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public class MigrationBuilder
{
	public virtual string ActiveProvider { get; }

	public virtual List<MigrationOperation> Operations { get; } = new List<MigrationOperation>();


	public MigrationBuilder([JetBrains.Annotations.CanBeNull] string activeProvider)
	{
		ActiveProvider = activeProvider;
	}

	public virtual OperationBuilder<AddColumnOperation> AddColumn<T>([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string type = null, bool? unicode = null, int? maxLength = null, bool rowVersion = false, [JetBrains.Annotations.CanBeNull] string schema = null, bool nullable = false, [JetBrains.Annotations.CanBeNull] object defaultValue = null, [JetBrains.Annotations.CanBeNull] string defaultValueSql = null, [JetBrains.Annotations.CanBeNull] string computedColumnSql = null, bool? fixedLength = null, [JetBrains.Annotations.CanBeNull] string comment = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		AddColumnOperation addColumnOperation = new AddColumnOperation
		{
			Schema = schema,
			Table = table,
			Name = name,
			ClrType = typeof(T),
			ColumnType = type,
			IsUnicode = unicode,
			MaxLength = maxLength,
			IsRowVersion = rowVersion,
			IsNullable = nullable,
			DefaultValue = defaultValue,
			DefaultValueSql = defaultValueSql,
			ComputedColumnSql = computedColumnSql,
			IsFixedLength = fixedLength,
			Comment = comment
		};
		Operations.Add(addColumnOperation);
		return new OperationBuilder<AddColumnOperation>(addColumnOperation);
	}

	public virtual OperationBuilder<AddForeignKeyOperation> AddForeignKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.NotNull] string principalTable, [JetBrains.Annotations.CanBeNull] string schema = null, [JetBrains.Annotations.CanBeNull] string principalSchema = null, [JetBrains.Annotations.CanBeNull] string principalColumn = null, ReferentialAction onUpdate = ReferentialAction.NoAction, ReferentialAction onDelete = ReferentialAction.NoAction)
	{
		return AddForeignKey(name, table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, principalTable, schema, principalSchema, new string[1] { principalColumn }, onUpdate, onDelete);
	}

	public virtual OperationBuilder<AddForeignKeyOperation> AddForeignKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.NotNull] string principalTable, [JetBrains.Annotations.CanBeNull] string schema = null, [JetBrains.Annotations.CanBeNull] string principalSchema = null, [JetBrains.Annotations.CanBeNull] string[] principalColumns = null, ReferentialAction onUpdate = ReferentialAction.NoAction, ReferentialAction onDelete = ReferentialAction.NoAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(columns, "columns");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(principalTable, "principalTable");
		AddForeignKeyOperation addForeignKeyOperation = new AddForeignKeyOperation
		{
			Schema = schema,
			Table = table,
			Name = name,
			Columns = columns,
			PrincipalSchema = principalSchema,
			PrincipalTable = principalTable,
			PrincipalColumns = principalColumns,
			OnUpdate = onUpdate,
			OnDelete = onDelete
		};
		Operations.Add(addForeignKeyOperation);
		return new OperationBuilder<AddForeignKeyOperation>(addForeignKeyOperation);
	}

	public virtual OperationBuilder<AddPrimaryKeyOperation> AddPrimaryKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return AddPrimaryKey(name, table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, schema);
	}

	public virtual OperationBuilder<AddPrimaryKeyOperation> AddPrimaryKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(columns, "columns");
		AddPrimaryKeyOperation addPrimaryKeyOperation = new AddPrimaryKeyOperation
		{
			Schema = schema,
			Table = table,
			Name = name,
			Columns = columns
		};
		Operations.Add(addPrimaryKeyOperation);
		return new OperationBuilder<AddPrimaryKeyOperation>(addPrimaryKeyOperation);
	}

	public virtual OperationBuilder<AddUniqueConstraintOperation> AddUniqueConstraint([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return AddUniqueConstraint(name, table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, schema);
	}

	public virtual OperationBuilder<AddUniqueConstraintOperation> AddUniqueConstraint([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(columns, "columns");
		AddUniqueConstraintOperation addUniqueConstraintOperation = new AddUniqueConstraintOperation
		{
			Schema = schema,
			Table = table,
			Name = name,
			Columns = columns
		};
		Operations.Add(addUniqueConstraintOperation);
		return new OperationBuilder<AddUniqueConstraintOperation>(addUniqueConstraintOperation);
	}

	public virtual AlterOperationBuilder<AlterColumnOperation> AlterColumn<T>([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string type = null, bool? unicode = null, int? maxLength = null, bool rowVersion = false, [JetBrains.Annotations.CanBeNull] string schema = null, bool nullable = false, [JetBrains.Annotations.CanBeNull] object defaultValue = null, [JetBrains.Annotations.CanBeNull] string defaultValueSql = null, [JetBrains.Annotations.CanBeNull] string computedColumnSql = null, [JetBrains.Annotations.CanBeNull] Type oldClrType = null, [JetBrains.Annotations.CanBeNull] string oldType = null, bool? oldUnicode = null, int? oldMaxLength = null, bool oldRowVersion = false, bool oldNullable = false, [JetBrains.Annotations.CanBeNull] object oldDefaultValue = null, [JetBrains.Annotations.CanBeNull] string oldDefaultValueSql = null, [JetBrains.Annotations.CanBeNull] string oldComputedColumnSql = null, bool? fixedLength = null, bool? oldFixedLength = null, [JetBrains.Annotations.CanBeNull] string comment = null, [JetBrains.Annotations.CanBeNull] string oldComment = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		AlterColumnOperation alterColumnOperation = new AlterColumnOperation
		{
			Schema = schema,
			Table = table,
			Name = name,
			ClrType = typeof(T),
			ColumnType = type,
			IsUnicode = unicode,
			MaxLength = maxLength,
			IsRowVersion = rowVersion,
			IsNullable = nullable,
			DefaultValue = defaultValue,
			DefaultValueSql = defaultValueSql,
			ComputedColumnSql = computedColumnSql,
			IsFixedLength = fixedLength,
			Comment = comment,
			OldColumn = new ColumnOperation
			{
				ClrType = (oldClrType ?? typeof(T)),
				ColumnType = oldType,
				IsUnicode = oldUnicode,
				MaxLength = oldMaxLength,
				IsRowVersion = oldRowVersion,
				IsNullable = oldNullable,
				DefaultValue = oldDefaultValue,
				DefaultValueSql = oldDefaultValueSql,
				ComputedColumnSql = oldComputedColumnSql,
				IsFixedLength = oldFixedLength,
				Comment = oldComment
			}
		};
		Operations.Add(alterColumnOperation);
		return new AlterOperationBuilder<AlterColumnOperation>(alterColumnOperation);
	}

	public virtual AlterOperationBuilder<AlterDatabaseOperation> AlterDatabase()
	{
		AlterDatabaseOperation alterDatabaseOperation = new AlterDatabaseOperation();
		Operations.Add(alterDatabaseOperation);
		return new AlterOperationBuilder<AlterDatabaseOperation>(alterDatabaseOperation);
	}

	public virtual AlterOperationBuilder<AlterSequenceOperation> AlterSequence([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, int incrementBy = 1, long? minValue = null, long? maxValue = null, bool cyclic = false, int oldIncrementBy = 1, long? oldMinValue = null, long? oldMaxValue = null, bool oldCyclic = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		AlterSequenceOperation alterSequenceOperation = new AlterSequenceOperation
		{
			Schema = schema,
			Name = name,
			IncrementBy = incrementBy,
			MinValue = minValue,
			MaxValue = maxValue,
			IsCyclic = cyclic,
			OldSequence = new SequenceOperation
			{
				IncrementBy = oldIncrementBy,
				MinValue = oldMinValue,
				MaxValue = oldMaxValue,
				IsCyclic = oldCyclic
			}
		};
		Operations.Add(alterSequenceOperation);
		return new AlterOperationBuilder<AlterSequenceOperation>(alterSequenceOperation);
	}

	public virtual AlterOperationBuilder<AlterTableOperation> AlterTable([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, [JetBrains.Annotations.CanBeNull] string comment = null, [JetBrains.Annotations.CanBeNull] string oldComment = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		AlterTableOperation alterTableOperation = new AlterTableOperation
		{
			Schema = schema,
			Name = name,
			Comment = comment,
			OldTable = new TableOperation
			{
				Comment = oldComment
			}
		};
		Operations.Add(alterTableOperation);
		return new AlterOperationBuilder<AlterTableOperation>(alterTableOperation);
	}

	public virtual OperationBuilder<CreateIndexOperation> CreateIndex([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.CanBeNull] string schema = null, bool unique = false, [JetBrains.Annotations.CanBeNull] string filter = null)
	{
		return CreateIndex(name, table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, schema, unique, filter);
	}

	public virtual OperationBuilder<CreateIndexOperation> CreateIndex([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.CanBeNull] string schema = null, bool unique = false, [JetBrains.Annotations.CanBeNull] string filter = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(columns, "columns");
		CreateIndexOperation createIndexOperation = new CreateIndexOperation
		{
			Schema = schema,
			Table = table,
			Name = name,
			Columns = columns,
			IsUnique = unique,
			Filter = filter
		};
		Operations.Add(createIndexOperation);
		return new OperationBuilder<CreateIndexOperation>(createIndexOperation);
	}

	public virtual OperationBuilder<EnsureSchemaOperation> EnsureSchema([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		EnsureSchemaOperation ensureSchemaOperation = new EnsureSchemaOperation
		{
			Name = name
		};
		Operations.Add(ensureSchemaOperation);
		return new OperationBuilder<EnsureSchemaOperation>(ensureSchemaOperation);
	}

	public virtual OperationBuilder<CreateSequenceOperation> CreateSequence([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, long startValue = 1L, int incrementBy = 1, long? minValue = null, long? maxValue = null, bool cyclic = false)
	{
		return CreateSequence<long>(name, schema, startValue, incrementBy, minValue, maxValue, cyclic);
	}

	public virtual OperationBuilder<CreateSequenceOperation> CreateSequence<T>([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, long startValue = 1L, int incrementBy = 1, long? minValue = null, long? maxValue = null, bool cyclic = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		CreateSequenceOperation createSequenceOperation = new CreateSequenceOperation
		{
			Schema = schema,
			Name = name,
			ClrType = typeof(T),
			StartValue = startValue,
			IncrementBy = incrementBy,
			MinValue = minValue,
			MaxValue = maxValue,
			IsCyclic = cyclic
		};
		Operations.Add(createSequenceOperation);
		return new OperationBuilder<CreateSequenceOperation>(createSequenceOperation);
	}

	public virtual OperationBuilder<CreateCheckConstraintOperation> CreateCheckConstraint([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string sql, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		CreateCheckConstraintOperation createCheckConstraintOperation = new CreateCheckConstraintOperation
		{
			Schema = schema,
			Name = name,
			Table = table,
			Sql = sql
		};
		Operations.Add(createCheckConstraintOperation);
		return new OperationBuilder<CreateCheckConstraintOperation>(createCheckConstraintOperation);
	}

	public virtual CreateTableBuilder<TColumns> CreateTable<TColumns>([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Func<ColumnsBuilder, TColumns> columns, [JetBrains.Annotations.CanBeNull] string schema = null, [JetBrains.Annotations.CanBeNull] Action<CreateTableBuilder<TColumns>> constraints = null, [JetBrains.Annotations.CanBeNull] string comment = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columns, "columns");
		CreateTableOperation createTableOperation = new CreateTableOperation
		{
			Schema = schema,
			Name = name,
			Comment = comment
		};
		ColumnsBuilder arg = new ColumnsBuilder(createTableOperation);
		TColumns val = columns(arg);
		Dictionary<PropertyInfo, AddColumnOperation> dictionary = new Dictionary<PropertyInfo, AddColumnOperation>();
		foreach (PropertyInfo declaredProperty in typeof(TColumns).GetTypeInfo().DeclaredProperties)
		{
			AddColumnOperation instance = ((IInfrastructure<AddColumnOperation>)declaredProperty.GetMethod.Invoke(val, null)).Instance;
			if (instance.Name == null)
			{
				instance.Name = declaredProperty.Name;
			}
			dictionary.Add(declaredProperty, instance);
		}
		CreateTableBuilder<TColumns> createTableBuilder = new CreateTableBuilder<TColumns>(createTableOperation, dictionary);
		constraints?.Invoke(createTableBuilder);
		Operations.Add(createTableOperation);
		return createTableBuilder;
	}

	public virtual OperationBuilder<DropColumnOperation> DropColumn([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		DropColumnOperation dropColumnOperation = new DropColumnOperation
		{
			Schema = schema,
			Table = table,
			Name = name
		};
		Operations.Add(dropColumnOperation);
		return new OperationBuilder<DropColumnOperation>(dropColumnOperation);
	}

	public virtual OperationBuilder<DropForeignKeyOperation> DropForeignKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		DropForeignKeyOperation dropForeignKeyOperation = new DropForeignKeyOperation
		{
			Schema = schema,
			Table = table,
			Name = name
		};
		Operations.Add(dropForeignKeyOperation);
		return new OperationBuilder<DropForeignKeyOperation>(dropForeignKeyOperation);
	}

	public virtual OperationBuilder<DropIndexOperation> DropIndex([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string table = null, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		DropIndexOperation dropIndexOperation = new DropIndexOperation
		{
			Schema = schema,
			Table = table,
			Name = name
		};
		Operations.Add(dropIndexOperation);
		return new OperationBuilder<DropIndexOperation>(dropIndexOperation);
	}

	public virtual OperationBuilder<DropPrimaryKeyOperation> DropPrimaryKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		DropPrimaryKeyOperation dropPrimaryKeyOperation = new DropPrimaryKeyOperation
		{
			Schema = schema,
			Table = table,
			Name = name
		};
		Operations.Add(dropPrimaryKeyOperation);
		return new OperationBuilder<DropPrimaryKeyOperation>(dropPrimaryKeyOperation);
	}

	public virtual OperationBuilder<DropSchemaOperation> DropSchema([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		DropSchemaOperation dropSchemaOperation = new DropSchemaOperation
		{
			Name = name
		};
		Operations.Add(dropSchemaOperation);
		return new OperationBuilder<DropSchemaOperation>(dropSchemaOperation);
	}

	public virtual OperationBuilder<DropSequenceOperation> DropSequence([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		DropSequenceOperation dropSequenceOperation = new DropSequenceOperation
		{
			Schema = schema,
			Name = name
		};
		Operations.Add(dropSequenceOperation);
		return new OperationBuilder<DropSequenceOperation>(dropSequenceOperation);
	}

	public virtual OperationBuilder<DropCheckConstraintOperation> DropCheckConstraint([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		DropCheckConstraintOperation dropCheckConstraintOperation = new DropCheckConstraintOperation
		{
			Name = name,
			Table = table,
			Schema = schema
		};
		Operations.Add(dropCheckConstraintOperation);
		return new OperationBuilder<DropCheckConstraintOperation>(dropCheckConstraintOperation);
	}

	public virtual OperationBuilder<DropTableOperation> DropTable([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		DropTableOperation dropTableOperation = new DropTableOperation
		{
			Schema = schema,
			Name = name
		};
		Operations.Add(dropTableOperation);
		return new OperationBuilder<DropTableOperation>(dropTableOperation);
	}

	public virtual OperationBuilder<DropUniqueConstraintOperation> DropUniqueConstraint([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		DropUniqueConstraintOperation dropUniqueConstraintOperation = new DropUniqueConstraintOperation
		{
			Schema = schema,
			Table = table,
			Name = name
		};
		Operations.Add(dropUniqueConstraintOperation);
		return new OperationBuilder<DropUniqueConstraintOperation>(dropUniqueConstraintOperation);
	}

	public virtual OperationBuilder<RenameColumnOperation> RenameColumn([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string newName, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(newName, "newName");
		RenameColumnOperation renameColumnOperation = new RenameColumnOperation
		{
			Name = name,
			Schema = schema,
			Table = table,
			NewName = newName
		};
		Operations.Add(renameColumnOperation);
		return new OperationBuilder<RenameColumnOperation>(renameColumnOperation);
	}

	public virtual OperationBuilder<RenameIndexOperation> RenameIndex([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string newName, [JetBrains.Annotations.CanBeNull] string table = null, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(newName, "newName");
		RenameIndexOperation renameIndexOperation = new RenameIndexOperation
		{
			Schema = schema,
			Table = table,
			Name = name,
			NewName = newName
		};
		Operations.Add(renameIndexOperation);
		return new OperationBuilder<RenameIndexOperation>(renameIndexOperation);
	}

	public virtual OperationBuilder<RenameSequenceOperation> RenameSequence([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, [JetBrains.Annotations.CanBeNull] string newName = null, [JetBrains.Annotations.CanBeNull] string newSchema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		RenameSequenceOperation renameSequenceOperation = new RenameSequenceOperation
		{
			Name = name,
			Schema = schema,
			NewName = newName,
			NewSchema = newSchema
		};
		Operations.Add(renameSequenceOperation);
		return new OperationBuilder<RenameSequenceOperation>(renameSequenceOperation);
	}

	public virtual OperationBuilder<RenameTableOperation> RenameTable([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, [JetBrains.Annotations.CanBeNull] string newName = null, [JetBrains.Annotations.CanBeNull] string newSchema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		RenameTableOperation renameTableOperation = new RenameTableOperation
		{
			Schema = schema,
			Name = name,
			NewName = newName,
			NewSchema = newSchema
		};
		Operations.Add(renameTableOperation);
		return new OperationBuilder<RenameTableOperation>(renameTableOperation);
	}

	public virtual OperationBuilder<RestartSequenceOperation> RestartSequence([JetBrains.Annotations.NotNull] string name, long startValue = 1L, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		RestartSequenceOperation restartSequenceOperation = new RestartSequenceOperation
		{
			Name = name,
			Schema = schema,
			StartValue = startValue
		};
		Operations.Add(restartSequenceOperation);
		return new OperationBuilder<RestartSequenceOperation>(restartSequenceOperation);
	}

	public virtual OperationBuilder<SqlOperation> Sql([JetBrains.Annotations.NotNull] string sql, bool suppressTransaction = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql, "sql");
		SqlOperation sqlOperation = new SqlOperation
		{
			Sql = sql,
			SuppressTransaction = suppressTransaction
		};
		Operations.Add(sqlOperation);
		return new OperationBuilder<SqlOperation>(sqlOperation);
	}

	public virtual OperationBuilder<InsertDataOperation> InsertData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.CanBeNull] object value, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return InsertData(table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, new object[1] { value }, schema);
	}

	public virtual OperationBuilder<InsertDataOperation> InsertData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.NotNull] object[] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return InsertData(table, columns, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values")), schema);
	}

	public virtual OperationBuilder<InsertDataOperation> InsertData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.NotNull] object[] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return InsertData(table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values"), firstDimension: true), schema);
	}

	public virtual OperationBuilder<InsertDataOperation> InsertData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.NotNull] object[,] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columns, "columns");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values");
		InsertDataOperation insertDataOperation = new InsertDataOperation
		{
			Table = table,
			Schema = schema,
			Columns = columns,
			Values = values
		};
		Operations.Add(insertDataOperation);
		return new OperationBuilder<InsertDataOperation>(insertDataOperation);
	}

	public virtual OperationBuilder<DeleteDataOperation> DeleteData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string keyColumn, [JetBrains.Annotations.CanBeNull] object keyValue, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return DeleteData(table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyColumn, "keyValue") }, new object[1] { keyValue }, schema);
	}

	public virtual OperationBuilder<DeleteDataOperation> DeleteData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] keyColumns, [JetBrains.Annotations.NotNull] object[] keyValues, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return DeleteData(table, keyColumns, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyValues, "keyValues")), schema);
	}

	public virtual OperationBuilder<DeleteDataOperation> DeleteData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string keyColumn, [JetBrains.Annotations.NotNull] object[] keyValues, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return DeleteData(table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(keyColumn, "keyColumn") }, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyValues, "keyValues"), firstDimension: true), schema);
	}

	public virtual OperationBuilder<DeleteDataOperation> DeleteData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] keyColumns, [JetBrains.Annotations.NotNull] object[,] keyValues, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyColumns, "keyColumns");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyValues, "keyValues");
		DeleteDataOperation deleteDataOperation = new DeleteDataOperation
		{
			Table = table,
			Schema = schema,
			KeyColumns = keyColumns,
			KeyValues = keyValues
		};
		Operations.Add(deleteDataOperation);
		return new OperationBuilder<DeleteDataOperation>(deleteDataOperation);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string keyColumn, [JetBrains.Annotations.CanBeNull] object keyValue, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.CanBeNull] object value, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return UpdateData(table, keyColumn, keyValue, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, new object[1] { value }, schema);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string keyColumn, [JetBrains.Annotations.CanBeNull] object keyValue, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.NotNull] object[] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return UpdateData(table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(keyColumn, "keyColumn") }, new object[1] { keyValue }, columns, values, schema);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] keyColumns, [JetBrains.Annotations.NotNull] object[] keyValues, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.CanBeNull] object value, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return UpdateData(table, keyColumns, keyValues, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, new object[1] { value }, schema);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] keyColumns, [JetBrains.Annotations.NotNull] object[] keyValues, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.NotNull] object[] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return UpdateData(table, keyColumns, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyValues, "keyValues")), columns, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values")), schema);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string keyColumn, [JetBrains.Annotations.NotNull] object[] keyValues, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.NotNull] object[] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return UpdateData(table, keyColumn, keyValues, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values"), firstDimension: true), schema);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string keyColumn, [JetBrains.Annotations.NotNull] object[] keyValues, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.NotNull] object[,] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return UpdateData(table, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(keyColumn, "keyColumn") }, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyValues, "keyValues"), firstDimension: true), columns, values, schema);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] keyColumns, [JetBrains.Annotations.NotNull] object[,] keyValues, [JetBrains.Annotations.NotNull] string column, [JetBrains.Annotations.NotNull] object[] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return UpdateData(table, keyColumns, keyValues, new string[1] { Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(column, "column") }, ToMultidimensionalArray(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values"), firstDimension: true), schema);
	}

	public virtual OperationBuilder<UpdateDataOperation> UpdateData([JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string[] keyColumns, [JetBrains.Annotations.NotNull] object[,] keyValues, [JetBrains.Annotations.NotNull] string[] columns, [JetBrains.Annotations.NotNull] object[,] values, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(table, "table");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyColumns, "keyColumns");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyValues, "keyValues");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columns, "columns");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values");
		UpdateDataOperation updateDataOperation = new UpdateDataOperation
		{
			Table = table,
			Schema = schema,
			KeyColumns = keyColumns,
			KeyValues = keyValues,
			Columns = columns,
			Values = values
		};
		Operations.Add(updateDataOperation);
		return new OperationBuilder<UpdateDataOperation>(updateDataOperation);
	}

	private static object[,] ToMultidimensionalArray(object[] values, bool firstDimension = false)
	{
		object[,] array = (firstDimension ? new object[values.Length, 1] : new object[1, values.Length]);
		for (int i = 0; i < values.Length; i++)
		{
			if (firstDimension)
			{
				array[i, 0] = values[i];
			}
			else
			{
				array[0, i] = values[i];
			}
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
