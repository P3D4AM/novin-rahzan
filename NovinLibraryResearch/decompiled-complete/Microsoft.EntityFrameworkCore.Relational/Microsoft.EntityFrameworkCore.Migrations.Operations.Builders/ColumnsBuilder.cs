using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

public class ColumnsBuilder
{
	private readonly CreateTableOperation _createTableOperation;

	public ColumnsBuilder([JetBrains.Annotations.NotNull] CreateTableOperation createTableOperation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(createTableOperation, "createTableOperation");
		_createTableOperation = createTableOperation;
	}

	public virtual OperationBuilder<AddColumnOperation> Column<T>([JetBrains.Annotations.CanBeNull] string type = null, bool? unicode = null, int? maxLength = null, bool rowVersion = false, [JetBrains.Annotations.CanBeNull] string name = null, bool nullable = false, [JetBrains.Annotations.CanBeNull] object defaultValue = null, [JetBrains.Annotations.CanBeNull] string defaultValueSql = null, [JetBrains.Annotations.CanBeNull] string computedColumnSql = null, bool? fixedLength = null, [JetBrains.Annotations.CanBeNull] string comment = null)
	{
		AddColumnOperation addColumnOperation = new AddColumnOperation
		{
			Schema = _createTableOperation.Schema,
			Table = _createTableOperation.Name,
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
		_createTableOperation.Columns.Add(addColumnOperation);
		return new OperationBuilder<AddColumnOperation>(addColumnOperation);
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
