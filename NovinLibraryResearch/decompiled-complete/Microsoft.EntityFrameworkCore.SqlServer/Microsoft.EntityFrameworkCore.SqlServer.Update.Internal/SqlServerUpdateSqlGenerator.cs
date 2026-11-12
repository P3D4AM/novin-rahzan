using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;

public class SqlServerUpdateSqlGenerator : UpdateSqlGenerator, ISqlServerUpdateSqlGenerator, IUpdateSqlGenerator
{
	private const string InsertedTableBaseName = "@inserted";

	private const string ToInsertTableAlias = "i";

	private const string PositionColumnName = "_Position";

	private const string PositionColumnDeclaration = "[_Position] [int]";

	private const string FullPositionColumnName = "i._Position";

	public SqlServerUpdateSqlGenerator([JetBrains.Annotations.NotNull] UpdateSqlGeneratorDependencies dependencies)
		: base(dependencies)
	{
	}

	public virtual ResultSetMapping AppendBulkInsertOperation(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition)
	{
		if (modificationCommands.Count == 1 && modificationCommands[0].ColumnModifications.All(delegate(ColumnModification o)
		{
			if (o.IsKey && o.IsRead)
			{
				IProperty property2 = o.Property;
				if (property2 == null)
				{
					return false;
				}
				return property2.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn;
			}
			return true;
		}))
		{
			return AppendInsertOperation(commandStringBuilder, modificationCommands[0], commandPosition);
		}
		List<ColumnModification> list = modificationCommands[0].ColumnModifications.Where((ColumnModification o) => o.IsRead).ToList();
		List<ColumnModification> list2 = modificationCommands[0].ColumnModifications.Where((ColumnModification o) => o.IsWrite).ToList();
		List<ColumnModification> keyOperations = modificationCommands[0].ColumnModifications.Where((ColumnModification o) => o.IsKey).ToList();
		bool flag = list2.Count == 0;
		List<ColumnModification> list3 = modificationCommands[0].ColumnModifications.Where(delegate(ColumnModification o)
		{
			IProperty property = o.Property;
			return property == null || property.GetValueGenerationStrategy() != SqlServerValueGenerationStrategy.IdentityColumn;
		}).ToList();
		if (flag)
		{
			if (list3.Count == 0 || list.Count == 0)
			{
				foreach (ModificationCommand modificationCommand in modificationCommands)
				{
					AppendInsertOperation(commandStringBuilder, modificationCommand, commandPosition);
				}
				if (list.Count != 0)
				{
					return ResultSetMapping.LastInResultSet;
				}
				return ResultSetMapping.NoResultSet;
			}
			if (list3.Count > 1)
			{
				list3.RemoveRange(1, list3.Count - 1);
			}
		}
		if (list.Count == 0)
		{
			return AppendBulkInsertWithoutServerValues(commandStringBuilder, modificationCommands, list2);
		}
		if (flag)
		{
			return AppendBulkInsertWithServerValuesOnly(commandStringBuilder, modificationCommands, commandPosition, list3, keyOperations, list);
		}
		if (modificationCommands[0].Entries.SelectMany((IUpdateEntry e) => e.EntityType.GetAllBaseTypesInclusive()).Any((IEntityType e) => e.IsMemoryOptimized()))
		{
			if (!list3.Any((ColumnModification o) => o.IsRead && o.IsKey))
			{
				foreach (ModificationCommand modificationCommand2 in modificationCommands)
				{
					AppendInsertOperation(commandStringBuilder, modificationCommand2, commandPosition++);
				}
			}
			else
			{
				foreach (ModificationCommand modificationCommand3 in modificationCommands)
				{
					AppendInsertOperationWithServerKeys(commandStringBuilder, modificationCommand3, keyOperations, list, commandPosition++);
				}
			}
			return ResultSetMapping.LastInResultSet;
		}
		return AppendBulkInsertWithServerValues(commandStringBuilder, modificationCommands, commandPosition, list2, keyOperations, list);
	}

	private ResultSetMapping AppendBulkInsertWithoutServerValues(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, List<ColumnModification> writeOperations)
	{
		string tableName = modificationCommands[0].TableName;
		string schema = modificationCommands[0].Schema;
		AppendInsertCommandHeader(commandStringBuilder, tableName, schema, writeOperations);
		AppendValuesHeader(commandStringBuilder, writeOperations);
		AppendValues(commandStringBuilder, writeOperations);
		for (int i = 1; i < modificationCommands.Count; i++)
		{
			commandStringBuilder.AppendLine(",");
			AppendValues(commandStringBuilder, modificationCommands[i].ColumnModifications.Where((ColumnModification o) => o.IsWrite).ToList());
		}
		commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator);
		return ResultSetMapping.NoResultSet;
	}

	private ResultSetMapping AppendBulkInsertWithServerValues(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition, List<ColumnModification> writeOperations, List<ColumnModification> keyOperations, List<ColumnModification> readOperations)
	{
		AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, keyOperations, "[_Position] [int]");
		string tableName = modificationCommands[0].TableName;
		string schema = modificationCommands[0].Schema;
		AppendMergeCommandHeader(commandStringBuilder, tableName, schema, "i", modificationCommands, writeOperations, "_Position");
		AppendOutputClause(commandStringBuilder, keyOperations, "@inserted", commandPosition, "i._Position");
		commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator);
		AppendSelectCommand(commandStringBuilder, readOperations, keyOperations, "@inserted", commandPosition, tableName, schema, "_Position");
		return ResultSetMapping.NotLastInResultSet;
	}

	private ResultSetMapping AppendBulkInsertWithServerValuesOnly(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition, List<ColumnModification> nonIdentityOperations, List<ColumnModification> keyOperations, List<ColumnModification> readOperations)
	{
		AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, keyOperations);
		string tableName = modificationCommands[0].TableName;
		string schema = modificationCommands[0].Schema;
		AppendInsertCommandHeader(commandStringBuilder, tableName, schema, nonIdentityOperations);
		AppendOutputClause(commandStringBuilder, keyOperations, "@inserted", commandPosition);
		AppendValuesHeader(commandStringBuilder, nonIdentityOperations);
		AppendValues(commandStringBuilder, nonIdentityOperations);
		for (int i = 1; i < modificationCommands.Count; i++)
		{
			commandStringBuilder.AppendLine(",");
			AppendValues(commandStringBuilder, nonIdentityOperations);
		}
		commandStringBuilder.Append(SqlGenerationHelper.StatementTerminator);
		AppendSelectCommand(commandStringBuilder, readOperations, keyOperations, "@inserted", commandPosition, tableName, schema);
		return ResultSetMapping.NotLastInResultSet;
	}

	private void AppendMergeCommandHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string toInsertTableAlias, [JetBrains.Annotations.NotNull] IReadOnlyList<ModificationCommand> modificationCommands, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> writeOperations, string additionalColumns = null)
	{
		commandStringBuilder.Append("MERGE ");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
		commandStringBuilder.Append(" USING (");
		AppendValuesHeader(commandStringBuilder, writeOperations);
		AppendValues(commandStringBuilder, writeOperations, "0");
		for (int i = 1; i < modificationCommands.Count; i++)
		{
			commandStringBuilder.AppendLine(",");
			AppendValues(commandStringBuilder, modificationCommands[i].ColumnModifications.Where((ColumnModification o) => o.IsWrite).ToList(), i.ToString(CultureInfo.InvariantCulture));
		}
		commandStringBuilder.Append(") AS ").Append(toInsertTableAlias).Append(" (")
			.AppendJoin(writeOperations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName);
			});
		if (additionalColumns != null)
		{
			commandStringBuilder.Append(", ").Append(additionalColumns);
		}
		commandStringBuilder.Append(")").AppendLine(" ON 1=0").AppendLine("WHEN NOT MATCHED THEN");
		commandStringBuilder.Append("INSERT ").Append("(").AppendJoin(writeOperations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
		{
			helper.DelimitIdentifier(sb, o.ColumnName);
		})
			.Append(")");
		AppendValuesHeader(commandStringBuilder, writeOperations);
		commandStringBuilder.Append("(").AppendJoin(writeOperations, toInsertTableAlias, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, string alias, ISqlGenerationHelper helper)
		{
			sb.Append(alias).Append(".");
			helper.DelimitIdentifier(sb, o.ColumnName);
		}).Append(")");
	}

	private void AppendValues(StringBuilder commandStringBuilder, IReadOnlyList<ColumnModification> operations, string additionalLiteral)
	{
		if (operations.Count <= 0)
		{
			return;
		}
		commandStringBuilder.Append("(").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
		{
			if (o.IsWrite)
			{
				helper.GenerateParameterName(sb, o.ParameterName);
			}
			else
			{
				sb.Append("DEFAULT");
			}
		}).Append(", ")
			.Append(additionalLiteral)
			.Append(")");
	}

	private void AppendDeclareTable(StringBuilder commandStringBuilder, string name, int index, IReadOnlyList<ColumnModification> operations, string additionalColumns = null)
	{
		commandStringBuilder.Append("DECLARE ").Append(name).Append(index)
			.Append(" TABLE (")
			.AppendJoin(operations, this, delegate(StringBuilder sb, ColumnModification o, SqlServerUpdateSqlGenerator generator)
			{
				generator.SqlGenerationHelper.DelimitIdentifier(sb, o.ColumnName);
				sb.Append(" ").Append(generator.GetTypeNameForCopy(o.Property));
			});
		if (additionalColumns != null)
		{
			commandStringBuilder.Append(", ").Append(additionalColumns);
		}
		commandStringBuilder.Append(")").AppendLine(SqlGenerationHelper.StatementTerminator);
	}

	private string GetTypeNameForCopy(IProperty property)
	{
		string text = property.GetColumnType();
		if (text == null)
		{
			text = property.FindFirstPrincipal()?.GetColumnType() ?? Dependencies.TypeMappingSource.FindMapping(property.ClrType)?.StoreType;
		}
		if (!(property.ClrType == typeof(byte[])) || text == null || (!text.Equals("rowversion", StringComparison.OrdinalIgnoreCase) && !text.Equals("timestamp", StringComparison.OrdinalIgnoreCase)))
		{
			return text;
		}
		if (!property.IsNullable)
		{
			return "binary(8)";
		}
		return "varbinary(8)";
	}

	private void AppendOutputClause(StringBuilder commandStringBuilder, IReadOnlyList<ColumnModification> operations, string tableName, int tableIndex, string additionalColumns = null)
	{
		commandStringBuilder.AppendLine().Append("OUTPUT ").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
		{
			sb.Append("INSERTED.");
			helper.DelimitIdentifier(sb, o.ColumnName);
		});
		if (additionalColumns != null)
		{
			commandStringBuilder.Append(", ").Append(additionalColumns);
		}
		commandStringBuilder.AppendLine().Append("INTO ").Append(tableName)
			.Append(tableIndex);
	}

	private ResultSetMapping AppendInsertOperationWithServerKeys(StringBuilder commandStringBuilder, ModificationCommand command, IReadOnlyList<ColumnModification> keyOperations, IReadOnlyList<ColumnModification> readOperations, int commandPosition)
	{
		string tableName = command.TableName;
		string schema = command.Schema;
		List<ColumnModification> operations = command.ColumnModifications.Where((ColumnModification o) => o.IsWrite).ToList();
		AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, keyOperations);
		AppendInsertCommandHeader(commandStringBuilder, tableName, schema, operations);
		AppendOutputClause(commandStringBuilder, keyOperations, "@inserted", commandPosition);
		AppendValuesHeader(commandStringBuilder, operations);
		AppendValues(commandStringBuilder, operations);
		commandStringBuilder.Append(SqlGenerationHelper.StatementTerminator);
		return AppendSelectCommand(commandStringBuilder, readOperations, keyOperations, "@inserted", commandPosition, tableName, schema);
	}

	private ResultSetMapping AppendSelectCommand(StringBuilder commandStringBuilder, IReadOnlyList<ColumnModification> readOperations, IReadOnlyList<ColumnModification> keyOperations, string insertedTableName, int insertedTableIndex, string tableName, string schema, string orderColumn = null)
	{
		commandStringBuilder.AppendLine().Append("SELECT ").AppendJoin(readOperations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
		{
			helper.DelimitIdentifier(sb, o.ColumnName, "t");
		})
			.Append(" FROM ");
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, tableName, schema);
		commandStringBuilder.AppendLine(" t").Append("INNER JOIN ").Append(insertedTableName)
			.Append(insertedTableIndex)
			.Append(" i")
			.Append(" ON ")
			.AppendJoin(keyOperations, delegate(StringBuilder sb, ColumnModification c)
			{
				sb.Append("(");
				SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
				sb.Append(" = ");
				SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
				sb.Append(")");
			}, " AND ");
		if (orderColumn != null)
		{
			commandStringBuilder.AppendLine().Append("ORDER BY ");
			SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, orderColumn, "i");
		}
		commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator).AppendLine();
		return ResultSetMapping.LastInResultSet;
	}

	protected override ResultSetMapping AppendSelectAffectedCountCommand(StringBuilder commandStringBuilder, string name, string schema, int commandPosition)
	{
		commandStringBuilder.Append("SELECT @@ROWCOUNT").AppendLine(SqlGenerationHelper.StatementTerminator).AppendLine();
		return ResultSetMapping.LastInResultSet;
	}

	public override void AppendBatchHeader(StringBuilder commandStringBuilder)
	{
		commandStringBuilder.Append("SET NOCOUNT ON").AppendLine(SqlGenerationHelper.StatementTerminator);
	}

	protected override void AppendIdentityWhereCondition(StringBuilder commandStringBuilder, ColumnModification columnModification)
	{
		SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, columnModification.ColumnName);
		commandStringBuilder.Append(" = ");
		commandStringBuilder.Append("scope_identity()");
	}

	protected override void AppendRowsAffectedWhereCondition(StringBuilder commandStringBuilder, int expectedRowsAffected)
	{
		commandStringBuilder.Append("@@ROWCOUNT = ").Append(expectedRowsAffected.ToString(CultureInfo.InvariantCulture));
	}
}
