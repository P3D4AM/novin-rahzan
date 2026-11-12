using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

public class CreateTableBuilder<TColumns> : OperationBuilder<CreateTableOperation>
{
	private readonly IReadOnlyDictionary<PropertyInfo, AddColumnOperation> _columnMap;

	public CreateTableBuilder([JetBrains.Annotations.NotNull] CreateTableOperation operation, [JetBrains.Annotations.NotNull] IReadOnlyDictionary<PropertyInfo, AddColumnOperation> columnMap)
		: base(operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columnMap, "columnMap");
		_columnMap = columnMap;
	}

	public virtual OperationBuilder<AddForeignKeyOperation> ForeignKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Expression<Func<TColumns, object>> column, [JetBrains.Annotations.NotNull] string principalTable, [JetBrains.Annotations.NotNull] string principalColumn, [JetBrains.Annotations.CanBeNull] string principalSchema = null, ReferentialAction onUpdate = ReferentialAction.NoAction, ReferentialAction onDelete = ReferentialAction.NoAction)
	{
		return ForeignKey(name, column, principalTable, new string[1] { principalColumn }, principalSchema, onUpdate, onDelete);
	}

	public virtual OperationBuilder<AddForeignKeyOperation> ForeignKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Expression<Func<TColumns, object>> columns, [JetBrains.Annotations.NotNull] string principalTable, [JetBrains.Annotations.NotNull] string[] principalColumns, [JetBrains.Annotations.CanBeNull] string principalSchema = null, ReferentialAction onUpdate = ReferentialAction.NoAction, ReferentialAction onDelete = ReferentialAction.NoAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columns, "columns");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(principalTable, "principalTable");
		AddForeignKeyOperation addForeignKeyOperation = new AddForeignKeyOperation
		{
			Schema = Operation.Schema,
			Table = Operation.Name,
			Name = name,
			Columns = Map(columns),
			PrincipalSchema = principalSchema,
			PrincipalTable = principalTable,
			PrincipalColumns = principalColumns,
			OnUpdate = onUpdate,
			OnDelete = onDelete
		};
		Operation.ForeignKeys.Add(addForeignKeyOperation);
		return new OperationBuilder<AddForeignKeyOperation>(addForeignKeyOperation);
	}

	public virtual OperationBuilder<AddPrimaryKeyOperation> PrimaryKey([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Expression<Func<TColumns, object>> columns)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columns, "columns");
		AddPrimaryKeyOperation addPrimaryKeyOperation = new AddPrimaryKeyOperation
		{
			Schema = Operation.Schema,
			Table = Operation.Name,
			Name = name,
			Columns = Map(columns)
		};
		Operation.PrimaryKey = addPrimaryKeyOperation;
		return new OperationBuilder<AddPrimaryKeyOperation>(addPrimaryKeyOperation);
	}

	public virtual OperationBuilder<AddUniqueConstraintOperation> UniqueConstraint([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Expression<Func<TColumns, object>> columns)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columns, "columns");
		AddUniqueConstraintOperation addUniqueConstraintOperation = new AddUniqueConstraintOperation
		{
			Schema = Operation.Schema,
			Table = Operation.Name,
			Name = name,
			Columns = Map(columns)
		};
		Operation.UniqueConstraints.Add(addUniqueConstraintOperation);
		return new OperationBuilder<AddUniqueConstraintOperation>(addUniqueConstraintOperation);
	}

	public virtual OperationBuilder<CreateCheckConstraintOperation> CheckConstraint([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string sql)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sql, "sql");
		CreateCheckConstraintOperation createCheckConstraintOperation = new CreateCheckConstraintOperation
		{
			Schema = Operation.Schema,
			Table = Operation.Name,
			Name = name,
			Sql = sql
		};
		Operation.CheckConstraints.Add(createCheckConstraintOperation);
		return new OperationBuilder<CreateCheckConstraintOperation>(createCheckConstraintOperation);
	}

	public new virtual CreateTableBuilder<TColumns> Annotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] object value)
	{
		return (CreateTableBuilder<TColumns>)base.Annotation(name, value);
	}

	private string[] Map(LambdaExpression columns)
	{
		return (from c in columns.GetPropertyAccessList()
			select _columnMap[c].Name).ToArray();
	}
}
