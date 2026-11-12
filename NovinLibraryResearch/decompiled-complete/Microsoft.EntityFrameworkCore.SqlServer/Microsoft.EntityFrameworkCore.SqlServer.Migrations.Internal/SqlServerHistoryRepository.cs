using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

public class SqlServerHistoryRepository : HistoryRepository
{
	protected override string ExistsSql
	{
		get
		{
			RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
			return "SELECT OBJECT_ID(" + mapping.GenerateSqlLiteral(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema)) + ")" + SqlGenerationHelper.StatementTerminator;
		}
	}

	public SqlServerHistoryRepository([JetBrains.Annotations.NotNull] HistoryRepositoryDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override bool InterpretExistsResult(object value)
	{
		return value != DBNull.Value;
	}

	public override string GetCreateIfNotExistsScript()
	{
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		StringBuilder stringBuilder = new StringBuilder().Append("IF OBJECT_ID(").Append(mapping.GenerateSqlLiteral(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))).AppendLine(") IS NULL")
			.AppendLine("BEGIN");
		using (StringReader stringReader = new StringReader(GetCreateScript()))
		{
			bool flag = true;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.AppendLine();
				}
				if (text.Length != 0)
				{
					stringBuilder.Append("    ").Append(text);
				}
			}
		}
		stringBuilder.AppendLine().Append("END").AppendLine(SqlGenerationHelper.StatementTerminator);
		return stringBuilder.ToString();
	}

	public override string GetBeginIfNotExistsScript(string migrationId)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationId, "migrationId");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		return new StringBuilder().Append("IF NOT EXISTS(SELECT * FROM ").Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema)).Append(" WHERE ")
			.Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
			.Append(" = ")
			.Append(mapping.GenerateSqlLiteral(migrationId))
			.AppendLine(")")
			.Append("BEGIN")
			.ToString();
	}

	public override string GetBeginIfExistsScript(string migrationId)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationId, "migrationId");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		return new StringBuilder().Append("IF EXISTS(SELECT * FROM ").Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema)).Append(" WHERE ")
			.Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
			.Append(" = ")
			.Append(mapping.GenerateSqlLiteral(migrationId))
			.AppendLine(")")
			.Append("BEGIN")
			.ToString();
	}

	public override string GetEndIfScript()
	{
		return new StringBuilder().Append("END").AppendLine(SqlGenerationHelper.StatementTerminator).ToString();
	}
}
