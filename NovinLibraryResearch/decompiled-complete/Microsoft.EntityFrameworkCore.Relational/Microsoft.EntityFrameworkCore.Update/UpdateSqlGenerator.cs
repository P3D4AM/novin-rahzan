using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Update;

public abstract class UpdateSqlGenerator : IUpdateSqlGenerator
{
	protected virtual UpdateSqlGeneratorDependencies Dependencies { get; }

	protected virtual ISqlGenerationHelper SqlGenerationHelper => Dependencies.SqlGenerationHelper;

	protected UpdateSqlGenerator([JetBrains.Annotations.NotNull] UpdateSqlGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual ResultSetMapping AppendInsertOperation(StringBuilder commandStringBuilder, ModificationCommand command, int commandPosition)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		string tableName = command.TableName;
		string schema = command.Schema;
		IReadOnlyList<ColumnModification> columnModifications = command.ColumnModifications;
		List<ColumnModification> writeOperations = columnModifications.Where((ColumnModification o) => o.IsWrite).ToList();
		List<ColumnModification> list = columnModifications.Where((ColumnModification o) => o.IsRead).ToList();
		AppendInsertCommand(commandStringBuilder, tableName, schema, writeOperations);
		if (list.Count > 0)
		{
			List<ColumnModification> conditionOperations = columnModifications.Where((ColumnModification o) => o.IsKey).ToList();
			return AppendSelectAffectedCommand(commandStringBuilder, tableName, schema, list, conditionOperations, commandPosition);
		}
		return ResultSetMapping.NoResultSet;
	}

	public virtual ResultSetMapping AppendUpdateOperation(StringBuilder commandStringBuilder, ModificationCommand command, int commandPosition)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		string tableName = command.TableName;
		string schema = command.Schema;
		IReadOnlyList<ColumnModification> columnModifications = command.ColumnModifications;
		List<ColumnModification> writeOperations = columnModifications.Where((ColumnModification o) => o.IsWrite).ToList();
		List<ColumnModification> conditionOperations = columnModifications.Where((ColumnModification o) => o.IsCondition).ToList();
		List<ColumnModification> list = columnModifications.Where((ColumnModification o) => o.IsRead).ToList();
		AppendUpdateCommand(commandStringBuilder, tableName, schema, writeOperations, conditionOperations);
		if (list.Count > 0)
		{
			List<ColumnModification> conditionOperations2 = columnModifications.Where((ColumnModification o) => o.IsKey).ToList();
			return AppendSelectAffectedCommand(commandStringBuilder, tableName, schema, list, conditionOperations2, commandPosition);
		}
		return AppendSelectAffectedCountCommand(commandStringBuilder, tableName, schema, commandPosition);
	}

	public virtual ResultSetMapping AppendDeleteOperation(StringBuilder commandStringBuilder, ModificationCommand command, int commandPosition)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		string tableName = command.TableName;
		string schema = command.Schema;
		List<ColumnModification> conditionOperations = command.ColumnModifications.Where((ColumnModification o) => o.IsCondition).ToList();
		AppendDeleteCommand(commandStringBuilder, tableName, schema, conditionOperations);
		return AppendSelectAffectedCountCommand(commandStringBuilder, tableName, schema, commandPosition);
	}

	protected virtual void AppendInsertCommand([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> writeOperations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(writeOperations, "writeOperations");
		AppendInsertCommandHeader(commandStringBuilder, name, schema, writeOperations);
		AppendValuesHeader(commandStringBuilder, writeOperations);
		AppendValues(commandStringBuilder, writeOperations);
		commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator);
	}

	protected virtual void AppendUpdateCommand([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> writeOperations, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> conditionOperations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(writeOperations, "writeOperations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conditionOperations, "conditionOperations");
		AppendUpdateCommandHeader(commandStringBuilder, name, schema, writeOperations);
		AppendWhereClause(commandStringBuilder, conditionOperations);
		commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator);
	}

	protected virtual void AppendDeleteCommand([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> conditionOperations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conditionOperations, "conditionOperations");
		AppendDeleteCommandHeader(commandStringBuilder, name, schema);
		AppendWhereClause(commandStringBuilder, conditionOperations);
		commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator);
	}

	protected virtual ResultSetMapping AppendSelectAffectedCountCommand([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, int commandPosition)
	{
		return ResultSetMapping.NoResultSet;
	}

	protected virtual ResultSetMapping AppendSelectAffectedCommand([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> readOperations, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> conditionOperations, int commandPosition)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(readOperations, "readOperations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conditionOperations, "conditionOperations");
		AppendSelectCommandHeader(commandStringBuilder, readOperations);
		AppendFromClause(commandStringBuilder, name, schema);
		AppendWhereAffectedClause(commandStringBuilder, conditionOperations);
		commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator).AppendLine();
		return ResultSetMapping.LastInResultSet;
	}

	protected virtual void AppendInsertCommandHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		commandStringBuilder.Append("INSERT INTO ");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
		if (operations.Count > 0)
		{
			commandStringBuilder.Append(" (").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName);
			}).Append(")");
		}
	}

	protected virtual void AppendDeleteCommandHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		commandStringBuilder.Append("DELETE FROM ");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
	}

	protected virtual void AppendUpdateCommandHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		commandStringBuilder.Append("UPDATE ");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
		commandStringBuilder.Append(" SET ").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
		{
			helper.DelimitIdentifier(sb, o.ColumnName);
			sb.Append(" = ");
			if (!o.UseCurrentValueParameter)
			{
				AppendSqlLiteral(sb, o.Value, o.Property);
			}
			else
			{
				helper.GenerateParameterNamePlaceholder(sb, o.ParameterName);
			}
		});
	}

	protected virtual void AppendSelectCommandHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		commandStringBuilder.Append("SELECT ").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
		{
			helper.DelimitIdentifier(sb, o.ColumnName);
		});
	}

	protected virtual void AppendFromClause([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		commandStringBuilder.AppendLine().Append("FROM ");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
	}

	protected virtual void AppendValuesHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		commandStringBuilder.AppendLine();
		commandStringBuilder.Append((operations.Count > 0) ? "VALUES " : "DEFAULT VALUES");
	}

	protected virtual void AppendValues([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		if (operations.Count <= 0)
		{
			return;
		}
		commandStringBuilder.Append("(").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
		{
			if (o.IsWrite)
			{
				if (!o.UseCurrentValueParameter)
				{
					AppendSqlLiteral(sb, o.Value, o.Property);
				}
				else
				{
					helper.GenerateParameterNamePlaceholder(sb, o.ParameterName);
				}
			}
			else
			{
				sb.Append("DEFAULT");
			}
		}).Append(")");
	}

	protected virtual void AppendWhereClause([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		if (operations.Count > 0)
		{
			commandStringBuilder.AppendLine().Append("WHERE ").AppendJoin(operations, delegate(StringBuilder sb, ColumnModification v)
			{
				AppendWhereCondition(sb, v, v.UseOriginalValueParameter);
			}, " AND ");
		}
	}

	protected virtual void AppendWhereAffectedClause([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		commandStringBuilder.AppendLine().Append("WHERE ");
		AppendRowsAffectedWhereCondition(commandStringBuilder, 1);
		if (operations.Count <= 0)
		{
			return;
		}
		commandStringBuilder.Append(" AND ").AppendJoin(operations, delegate(StringBuilder sb, ColumnModification v)
		{
			if (v.IsKey)
			{
				if (v.IsRead)
				{
					AppendIdentityWhereCondition(sb, v);
				}
				else
				{
					AppendWhereCondition(sb, v, v.UseOriginalValueParameter);
				}
			}
		}, " AND ");
	}

	protected abstract void AppendRowsAffectedWhereCondition([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, int expectedRowsAffected);

	protected virtual void AppendWhereCondition([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] ColumnModification columnModification, bool useOriginalValue)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandStringBuilder, "commandStringBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columnModification, "columnModification");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, columnModification.ColumnName);
		if ((useOriginalValue ? columnModification.OriginalValue : columnModification.Value) == null)
		{
			commandStringBuilder.Append(" IS NULL");
			return;
		}
		commandStringBuilder.Append(" = ");
		if (!columnModification.UseCurrentValueParameter && !columnModification.UseOriginalValueParameter)
		{
			AppendSqlLiteral(commandStringBuilder, columnModification.Value, columnModification.Property);
		}
		else
		{
			SqlGenerationHelper.GenerateParameterNamePlaceholder(commandStringBuilder, useOriginalValue ? columnModification.OriginalParameterName : columnModification.ParameterName);
		}
	}

	protected abstract void AppendIdentityWhereCondition([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] ColumnModification columnModification);

	public virtual void AppendBatchHeader(StringBuilder commandStringBuilder)
	{
	}

	public virtual string GenerateNextSequenceValueOperation(string name, string schema)
	{
		StringBuilder stringBuilder = new StringBuilder();
		AppendNextSequenceValueOperation(stringBuilder, name, schema);
		return stringBuilder.ToString();
	}

	public virtual void AppendNextSequenceValueOperation(StringBuilder commandStringBuilder, string name, string schema)
	{
		commandStringBuilder.Append("SELECT NEXT VALUE FOR ");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name"), schema);
	}

	private void AppendSqlLiteral(StringBuilder commandStringBuilder, object value, IProperty property)
	{
		RelationalTypeMapping relationalTypeMapping = ((property != null) ? Dependencies.TypeMappingSource.FindMapping(property) : null);
		if (relationalTypeMapping == null)
		{
			relationalTypeMapping = Dependencies.TypeMappingSource.GetMappingForValue(value);
		}
		commandStringBuilder.Append(relationalTypeMapping.GenerateProviderValueSqlLiteral(value));
	}
}
